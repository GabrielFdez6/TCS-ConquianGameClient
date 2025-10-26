using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServicePasswordRecovery;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.View.Profile; 
using ConquiánCliente.ViewModel.Validation;
using System.Linq;
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

        private PasswordUpdateMode _mode;
        public PasswordUpdateMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged(nameof(Mode));
                OnPropertyChanged(nameof(PageTitle)); 
            }
        }

        public bool IsEditProfileFlow { get; set; } = false;
        public string PageTitle
        {
            get
            {
                return Mode == PasswordUpdateMode.Change
                    ? Lang.EditDataEdit
                    : Lang.GlobalPasswordRecovery;
            }
        }
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }
        public string Token
        {
            get => token;
            set { token = value; OnPropertyChanged(nameof(Token)); }
        }
        public string NewPassword
        {
            get => newPassword;
            set { newPassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
        }
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

            try
            {
                recoveryClient = new PasswordRecoveryClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Lang.ErrorConnectingToServer, ex.Message), Lang.TitleConnectionError);
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

            bool success = await TryExecuteServiceCall(
                () => recoveryClient.RequestPasswordRecoveryAsync(Email, (int)this.Mode),
                Lang.ErrorRecoveryRequestFailed
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
            bool success = await TryExecuteServiceCall(
                () => recoveryClient.RequestPasswordRecoveryAsync(Email, (int)this.Mode),
                Lang.ErrorRecoveryRequestFailed
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

            bool success = await TryExecuteServiceCall(
                () => recoveryClient.ValidateRecoveryTokenAsync(Email, Token),
                Lang.ErrorVerificationCodeIncorrect
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

            try
            {
                IsLoading = true; 
                var client = new ServicePasswordRecovery.PasswordRecoveryClient();
                bool success = await client.ResetPasswordAsync(Email, Token, newPassword);

                if (success)
                {
                    if (IsEditProfileFlow)
                    {
                        page?.NavigationService?.Navigate(new UserProfilePage());
                    }
                    else
                    {
                        var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                        if (currentWindow is PasswordRecoveryMainFrame)
                        {
                            currentWindow.Close();
                        }

                        var loginWindow = new LogIn();
                        loginWindow.Show();
                    }
                }
                else
                {
                    MessageBox.Show(Lang.ErrorRecoveryRequestFailed, Lang.TitleError);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            catch (FaultException ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
            finally
            {
                IsLoading = false;
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
        private void ExecuteNavigateToLogin(object parameter)
        {
            var page = parameter as Page;
            var window = Window.GetWindow(page);

            var loginWindow = new LogIn();
            loginWindow.Show();
            window?.Close();
        }

        private async Task<bool> TryExecuteServiceCall(Func<Task<bool>> serviceCall, string businessErrorMessage)
        {
            if (recoveryClient == null)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                return false;
            }

            IsLoading = true;
            try
            {
                bool success = await serviceCall();

                if (!success)
                {
                    MessageBox.Show(businessErrorMessage, Lang.TitleError);
                }
                return success;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex is EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            else if (ex is CommunicationException)
            {
                MessageBox.Show(string.Format(Lang.ErrorConnectingToServer, ex.Message), Lang.TitleConnectionError);
            }
            else if (ex is TimeoutException)
            {
                MessageBox.Show(Lang.ErrorTimeout, Lang.TitleConnectionError);
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorGeneric, ex.Message), Lang.TitleError);
            }
        }
    }
}