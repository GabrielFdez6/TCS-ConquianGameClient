using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.View;
using ConquiánCliente.View.Profile;
using ConquiánCliente.ViewModel.Validation;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Profile
{
    public class EditInfoViewModel : ViewModelBase
    {
        private PlayerDto _player;
        public PlayerDto Player
        {
            get => _player;
            set { _player = value; OnPropertyChanged(); }
        }

        private string _instagramLink;
        public string InstagramLink
        {
            get => _instagramLink;
            set { _instagramLink = value; OnPropertyChanged(); }
        }

        private string _facebookLink;
        public string FacebookLink
        {
            get => _facebookLink;
            set { _facebookLink = value; OnPropertyChanged(); }
        }

        public ICommand SaveChangesCommand { get; }
        public ICommand CancelCommand { get; }
        public EditInfoViewModel(PlayerDto playerDto)
        {
            Player = playerDto;
            SaveChangesCommand = new RelayCommand(ExecuteSaveChanges, CanExecuteSaveChanges);
            CancelCommand = new RelayCommand(ExecuteCancel);
            LoadPlayerSocials();
        }

        private void LoadPlayerSocials()
        {
            try
            {
                var client = new UserProfileClient();
                SocialDto[] socialsArray = client.GetPlayerSocials(Player.idPlayer);
                List<SocialDto> socials = socialsArray?.ToList() ?? new List<SocialDto>();

                InstagramLink = socials.FirstOrDefault(s => s.IdSocialType == 1)?.UserLink ?? "";
                FacebookLink = socials.FirstOrDefault(s => s.IdSocialType == 2)?.UserLink ?? "";
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            catch (FaultException ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        private bool CanExecuteSaveChanges(object parameter) => true;

        private void ExecuteSaveChanges(object parameter)
        {
            string nameError = SignUpValidator.ValidateName(Player.name);
            if (!string.IsNullOrEmpty(nameError)) { MessageBox.Show(nameError, Lang.TitleValidation); return; }

            string lastNameError = SignUpValidator.ValidateLastName(Player.lastName);
            if (!string.IsNullOrEmpty(lastNameError)) { MessageBox.Show(lastNameError, Lang.TitleValidation); return; }

            string nicknameError = SignUpValidator.ValidateNickname(Player.nickname);
            if (!string.IsNullOrEmpty(nicknameError)) { MessageBox.Show(nicknameError, Lang.TitleValidation); return; }

            try
            {
                var client = new UserProfileClient();
                bool profileUpdated = client.UpdatePlayer(this.Player);

                var socialsToUpdate = new List<SocialDto>();
                if (!string.IsNullOrWhiteSpace(InstagramLink))
                {
                    socialsToUpdate.Add(new SocialDto { IdSocialType = 1, UserLink = this.InstagramLink });
                }
                if (!string.IsNullOrWhiteSpace(FacebookLink))
                {
                    socialsToUpdate.Add(new SocialDto { IdSocialType = 2, UserLink = this.FacebookLink });
                }

                bool socialsUpdated = client.UpdatePlayerSocials(Player.idPlayer, socialsToUpdate.ToArray());

                if (profileUpdated)
                {
                    MessageBox.Show(Lang.InfoUpdateSuccess, Lang.TitleSuccess);
                    PlayerSession.CurrentPlayer.nickname = this.Player.nickname;
                    ExecuteCancel(null);
                }
                else
                {
                    MessageBox.Show(Lang.InfoUpdateFailed, Lang.TitleError);
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
        }

        private void ExecuteCancel(object parameter)
        {
            ProfileMainFrame.MainFrame.Navigate(new UserProfilePage());
        }
    }
}