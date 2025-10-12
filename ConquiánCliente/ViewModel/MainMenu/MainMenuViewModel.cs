using ConquiánCliente.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.MainMenu
{
    public class MainMenuViewModel : ViewModelBase
    {
        public ICommand ViewProfileCommand { get; }
        public ICommand LogOutCommand { get; }

        public MainMenuViewModel()
        {
            ViewProfileCommand = new RelayCommand(ExecuteViewProfile);
            LogOutCommand = new RelayCommand(ExecuteLogOut);
        }

        private void ExecuteViewProfile(object obj)
        {
            UserProfile userProfile = new UserProfile();
            userProfile.Show();
            CloseCurrentWindow();
        }

        private void ExecuteLogOut(object obj)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
