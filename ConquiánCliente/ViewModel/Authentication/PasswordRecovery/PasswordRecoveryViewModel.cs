using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServicePasswordRecovery;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConquiánCliente.View.Authentication.PasswordRecovery;

namespace ConquiánCliente.ViewModel.Authentication.PasswordRecovery
{
    public class PasswordRecoveryViewModel : ViewModelBase
    {
        private string email;
        private string token;
        private string newPassword;
        private string confirmPassword;
        private bool isLoading;
        private readonly IPasswordRecovery recoveryClient;

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
            string validationError = PasswordRecoveryValidator.ValidateEmail(Email);
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, Lang.TitleValidation);
                return;
            }

            bool success = await TryExecuteServiceCall(
                () => recoveryClient.RequestPasswordRecoveryAsync(Email),
                Lang.ErrorRecoveryRequestFailed
            );

            if (success)
            {
                var page = parameter as Page;
                page?.NavigationService?.Navigate(new CodeValidation(this));
            }
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
            string validationError = PasswordRecoveryValidator.ValidatePasswords(NewPassword, ConfirmPassword);
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, Lang.TitleValidation);
                return;
            }

            bool success = await TryExecuteServiceCall(
                () => recoveryClient.ResetPasswordAsync(Email, Token, NewPassword),
                Lang.ErrorPasswordResetFailed
            );

            if (success)
            {
                MessageBox.Show(Lang.SuccessPasswordReset, Lang.TitleRegistrationComplete);
                ExecuteNavigateToLogin(parameter);
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
            page?.NavigationService?.Navigate(new RequestRecovery());
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