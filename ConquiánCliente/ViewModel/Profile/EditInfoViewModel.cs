using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.View.Profile;
using ConquiánCliente.ViewModel.Authentication.PasswordRecovery;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages; 

namespace ConquiánCliente.ViewModel.Profile
{
    public class EditInfoViewModel : ViewModelBase
    {
        private const int INSTAGRAM_SOCIAL_TYPE = 1;
        private const int FACEBOOK_SOCIAL_TYPE = 2;
        private bool isLoading;
        private PlayerDto player;
        private string instagramLink;
        private string facebookLink;
        private readonly IMessageResolver messageResolver; 

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

        public PlayerDto Player
        {
            get => player;
            set { player = value; OnPropertyChanged(); }
        }

        public string InstagramLink
        {
            get => instagramLink;
            set { instagramLink = value; OnPropertyChanged(); }
        }

        public string FacebookLink
        {
            get => facebookLink;
            set { facebookLink = value; OnPropertyChanged(); }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand NavigateToChangePasswordCommand { get; }

        public EditInfoViewModel(PlayerDto playerDto)
        {
            Player = playerDto;
            this.messageResolver = new ResourceMessageResolver(); 

            SaveChangesCommand = new RelayCommand(ExecuteSaveChanges, CanExecuteSaveChanges);
            NavigateToChangePasswordCommand = new RelayCommand(ExecuteNavigateToChangePassword, CanExecuteChangePassword);
            CancelCommand = new RelayCommand(ExecuteCancel);
            LoadPlayerSocials();
        }

        private bool CanExecuteChangePassword(object parameter)
        {
            bool canExecute = !IsLoading;
            return canExecute;
        }

        private async void ExecuteNavigateToChangePassword(object parameter)
        {
            try
            {
                IsLoading = true;

                var passwordVM = new PasswordRecoveryViewModel();
                passwordVM.Email = PlayerSession.CurrentPlayer.email;
                passwordVM.IsEditProfileFlow = true;

                bool tokenRequestSucceeded = await passwordVM.RequestChangePasswordTokenAsync();

                if (tokenRequestSucceeded)
                {
                    NavigateToCodeValidation(parameter, passwordVM);
                }
            }
            catch (EndpointNotFoundException)
            {
                ShowServerUnavailableError();
            }
            catch (CommunicationException)
            {
                ShowServerUnavailableError();
            }
            catch (System.TimeoutException)
            {
                ShowServerUnavailableError();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NavigateToCodeValidation(object parameter, PasswordRecoveryViewModel passwordVM)
        {
            var page = parameter as Page;
            bool canNavigate = page?.NavigationService != null;

            if (canNavigate)
            {
                page.NavigationService.Navigate(new CodeValidation(passwordVM));
            }
        }

        private void ShowServerUnavailableError()
        {
            MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
        }

        private void LoadPlayerSocials()
        {
            try
            {
                var client = new UserProfileClient();
                SocialDto[] socialsArray = client.GetPlayerSocials(Player.idPlayer);

                List<SocialDto> socials = GetSocialsList(socialsArray);

                InstagramLink = ExtractInstagramLink(socials);
                FacebookLink = ExtractFacebookLink(socials);
            }
            catch (FaultException<ServiceUserProfile.ServiceFaultDto> fault)
            {
                HandleServiceFault(fault);
            }
            catch (EndpointNotFoundException)
            {
                ShowServerUnavailableError();
            }
            catch (CommunicationException)
            {
                ShowServerUnavailableError();
            }
            catch (System.TimeoutException)
            {
                ShowServerUnavailableError();
            }
        }

        private List<SocialDto> GetSocialsList(SocialDto[] socialsArray)
        {
            bool socialsExist = socialsArray != null;
            List<SocialDto> socials = socialsExist ? socialsArray.ToList() : new List<SocialDto>();
            return socials;
        }

        private string ExtractInstagramLink(List<SocialDto> socials)
        {
            var instagramSocial = socials.FirstOrDefault(s => s.IdSocialType == INSTAGRAM_SOCIAL_TYPE);
            string link = instagramSocial?.UserLink ?? string.Empty;
            return link;
        }

        private string ExtractFacebookLink(List<SocialDto> socials)
        {
            var facebookSocial = socials.FirstOrDefault(s => s.IdSocialType == FACEBOOK_SOCIAL_TYPE);
            string link = facebookSocial?.UserLink ?? string.Empty;
            return link;
        }

        private static bool CanExecuteSaveChanges(object parameter)
        {
            bool canExecute = true;
            return canExecute;
        }

        private void ExecuteSaveChanges(object parameter)
        {
            bool validationFailed = !ValidatePlayerData();
            if (validationFailed)
            {
                return;
            }

            bool passwordNeedsUpdate = ValidateAndUpdatePassword(parameter);
            if (passwordNeedsUpdate && Player.password == null)
            {
                return;
            }

            UpdatePlayerInformation();
        }

        private bool ValidatePlayerData()
        {
            bool nameIsValid = ValidateName();
            if (!nameIsValid)
            {
                return false;
            }

            bool lastNameIsValid = ValidateLastName();
            if (!lastNameIsValid)
            {
                return false;
            }

            bool nicknameIsValid = ValidateNickname();
            if (!nicknameIsValid)
            {
                return false;
            }

            bool allFieldsValid = true;
            return allFieldsValid;
        }

        private bool ValidateName()
        {
            string nameError = SignUpValidator.ValidateName(Player.name);
            bool hasError = !string.IsNullOrEmpty(nameError);

            if (hasError)
            {
                MessageBox.Show(nameError, Lang.TitleValidation);
                bool validationFailed = false;
                return validationFailed;
            }

            bool validationPassed = true;
            return validationPassed;
        }

        private bool ValidateLastName()
        {
            string lastNameError = SignUpValidator.ValidateLastName(Player.lastName);
            bool hasError = !string.IsNullOrEmpty(lastNameError);

            if (hasError)
            {
                MessageBox.Show(lastNameError, Lang.TitleValidation);
                bool validationFailed = false;
                return validationFailed;
            }

            bool validationPassed = true;
            return validationPassed;
        }

        private bool ValidateNickname()
        {
            string nicknameError = SignUpValidator.ValidateNickname(Player.nickname);
            bool hasError = !string.IsNullOrEmpty(nicknameError);

            if (hasError)
            {
                MessageBox.Show(nicknameError, Lang.TitleValidation);
                bool validationFailed = false;
                return validationFailed;
            }

            bool validationPassed = true;
            return validationPassed;
        }

        private bool ValidateAndUpdatePassword(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = ExtractPassword(passwordBox);

            bool passwordProvided = !string.IsNullOrEmpty(password);
            if (!passwordProvided)
            {
                bool noValidationNeeded = true;
                return noValidationNeeded;
            }

            string passwordError = SignUpValidator.ValidatePassword(password);
            bool hasError = !string.IsNullOrEmpty(passwordError);

            if (hasError)
            {
                MessageBox.Show(passwordError, Lang.TitleValidation);
                Player.password = null;
                bool validationFailed = false;
                return validationFailed;
            }

            this.Player.password = password;
            bool validationSucceeded = true;
            return validationSucceeded;
        }

        private string ExtractPassword(PasswordBox passwordBox)
        {
            bool passwordBoxExists = passwordBox != null;
            string password = passwordBoxExists ? passwordBox.Password : string.Empty;
            return password;
        }

        private void UpdatePlayerInformation()
        {
            try
            {
                var client = new UserProfileClient();

                client.UpdatePlayer(this.Player);

                var socialsToUpdate = BuildSocialsUpdateList();
                client.UpdatePlayerSocials(Player.idPlayer, socialsToUpdate.ToArray());

                MessageBox.Show(Lang.InfoUpdateSuccess, Lang.TitleSuccess);
                PlayerSession.CurrentPlayer.nickname = this.Player.nickname;
                ExecuteCancel(null);
            }
            catch (FaultException<ServiceUserProfile.ServiceFaultDto> fault)
            {
                HandleServiceFault(fault);
            }
            catch (EndpointNotFoundException)
            {
                ShowServerUnavailableError();
            }
            catch (CommunicationException)
            {
                ShowServerUnavailableError();
            }
            catch (System.TimeoutException)
            {
                ShowServerUnavailableError();
            }
        }

        private List<SocialDto> BuildSocialsUpdateList()
        {
            var socialsToUpdate = new List<SocialDto>();

            bool instagramLinkProvided = !string.IsNullOrWhiteSpace(InstagramLink);
            if (instagramLinkProvided)
            {
                var instagramSocial = new SocialDto
                {
                    IdSocialType = INSTAGRAM_SOCIAL_TYPE,
                    UserLink = this.InstagramLink
                };
                socialsToUpdate.Add(instagramSocial);
            }

            bool facebookLinkProvided = !string.IsNullOrWhiteSpace(FacebookLink);
            if (facebookLinkProvided)
            {
                var facebookSocial = new SocialDto
                {
                    IdSocialType = FACEBOOK_SOCIAL_TYPE,
                    UserLink = this.FacebookLink
                };
                socialsToUpdate.Add(facebookSocial);
            }

            return socialsToUpdate;
        }

        private void HandleServiceFault(FaultException<ServiceUserProfile.ServiceFaultDto> fault)
        {
            var errorType = (ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);
            MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
        }


        private static void ExecuteCancel(object parameter)
        {
            ProfileMainFrame.MainFrame.Navigate(new UserProfilePage());
        }
    }
}