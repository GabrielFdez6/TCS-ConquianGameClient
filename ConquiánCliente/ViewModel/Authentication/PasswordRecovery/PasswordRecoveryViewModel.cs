using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServicePasswordRecovery;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.View.Profile;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Authentication.PasswordRecovery
{
    public enum PasswordUpdateMode
    {
        Recovery = 0,
        Change = 1
    }

    public class PasswordRecoveryViewModel : ViewModelBase
    {
        private string email;
        private string token;
        private string newPassword;
        private string confirmPassword;
        private bool isLoading;
        private readonly IPasswordRecovery recoveryClient;
        private PasswordUpdateMode mode;
        private readonly IMessageResolver messageResolver;

        public PasswordUpdateMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                OnPropertyChanged(nameof(Mode));
                OnPropertyChanged(nameof(PageTitle));
            }
        }

        public bool IsEditProfileFlow { get; set; } = false;
        public string PageTitle => Mode == PasswordUpdateMode.Change ? Lang.EditDataEdit : Lang.GlobalPasswordRecovery;

        public string Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }
        public string Token { get => token; set { token = value; OnPropertyChanged(nameof(Token)); } }
        public string NewPassword { get => newPassword; set { newPassword = value; OnPropertyChanged(nameof(NewPassword)); } }
        public string ConfirmPassword { get => confirmPassword; set { confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); } }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand RequestRecoveryCommand { get; }
        public ICommand ValidateTokenCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToStartCommand { get; }

        public PasswordRecoveryViewModel()
        {
            Mode = PasswordUpdateMode.Recovery;
            RequestRecoveryCommand = new RelayCommand(ExecuteRequestRecovery, CanExecuteCommand);
            ValidateTokenCommand = new RelayCommand(ExecuteValidateToken, CanExecuteCommand);
            ResetPasswordCommand = new RelayCommand(ExecuteResetPassword, CanExecuteCommand);
            NavigateToLoginCommand = new RelayCommand(ExecuteNavigateToLogin);
            NavigateToStartCommand = new RelayCommand(ExecuteNavigateToStart, CanExecuteNavigateToStart);

            this.messageResolver = new ResourceMessageResolver();

            try
            {
                recoveryClient = new PasswordRecoveryClient();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format(Lang.ErrorConnectingToServer), Lang.TitleConnectionError);
            }
        }

        private bool CanExecuteCommand(object parameter)
        {
            return !IsLoading;
        }

        private async void ExecuteRequestRecovery(object parameter)
        {
            this.Mode = PasswordUpdateMode.Recovery;

            string validationError = PasswordRecoveryValidator.ValidateEmail(Email);
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, Lang.TitleValidation);
                return;
            }

            bool success = await TryExecuteServiceCall(async () =>
                await recoveryClient.RequestPasswordRecoveryAsync(Email, (int)this.Mode)
            );

            if (success)
            {
                var page = parameter as Page;
                page?.NavigationService?.Navigate(new CodeValidation(this));
            }
        }

        public async Task<bool> RequestChangePasswordTokenAsync()
        {
            this.Mode = PasswordUpdateMode.Change;

            bool success = await TryExecuteServiceCall(async () =>
                await recoveryClient.RequestPasswordRecoveryAsync(Email, (int)this.Mode)
            );

            return success;
        }

        private async void ExecuteValidateToken(object parameter)
        {
            string validationError = PasswordRecoveryValidator.ValidateToken(Token);
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, Lang.TitleValidation);
                return;
            }

            bool success = await TryExecuteServiceCall(async () =>
                await recoveryClient.ValidateRecoveryTokenAsync(Email, Token)
            );

            if (success)
            {
                var page = parameter as Page;
                page?.NavigationService?.Navigate(new ResetPassword(this));
            }
        }

        private async void ExecuteResetPassword(object parameter)
        {
            var page = parameter as Page;
            string validationError = PasswordRecoveryValidator.ValidatePasswords(NewPassword, ConfirmPassword);

            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, Lang.TitleValidation);
                return;
            }

            bool success = await TryExecuteServiceCall(async () =>
                await recoveryClient.ResetPasswordAsync(Email, Token, NewPassword)
            );

            if (success)
            {
                HandleSuccessfulReset(page);
            }
        }

        private void HandleSuccessfulReset(Page page)
        {
            if (IsEditProfileFlow)
            {
                page?.NavigationService?.Navigate(new UserProfilePage());
            }
            else
            {
                MessageBox.Show(Lang.SuccessPasswordReset, Lang.TitleSuccess);
                var loginWindow = new LogIn();
                loginWindow.Show();
                Window.GetWindow(page)?.Close();
            }
        }

        private bool CanExecuteNavigateToStart(object parameter)
        {
            return !IsLoading;
        }

        private void ExecuteNavigateToStart(object parameter)
        {
            this.Token = string.Empty;
            this.NewPassword = string.Empty;
            this.ConfirmPassword = string.Empty;
            var page = parameter as Page;

            if (IsEditProfileFlow)
            {
                if (page?.NavigationService?.CanGoBack == true)
                {
                    page.NavigationService.GoBack();
                }
            }
            else
            {
                page?.NavigationService?.Navigate(new RequestRecovery());
            }
        }

        private static void ExecuteNavigateToLogin(object parameter)
        {
            var page = parameter as Page;
            var window = Window.GetWindow(page);
            new LogIn().Show();
            window?.Close();
        }

        private async Task<bool> TryExecuteServiceCall(Func<Task> serviceCall)
        {
            bool executionSuccess = false;

            if (recoveryClient == null)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                return executionSuccess;
            }

            IsLoading = true;
            try
            {
                await serviceCall();
                executionSuccess = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                executionSuccess = false;
            }
            finally
            {
                IsLoading = false;
            }

            return executionSuccess;
        }

        private void HandleException(Exception ex)
        {
            if (ex is FaultException<ServiceFaultDto> fault)
            {
                int errorValue = (int)fault.Detail.ErrorType;
                var targetErrorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)errorValue;

                string errorMessage = messageResolver.GetMessage(targetErrorType);

                MessageBox.Show(errorMessage, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ex is EndpointNotFoundException || ex is CommunicationException || ex is TimeoutException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorGeneric, ex.Message), Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}