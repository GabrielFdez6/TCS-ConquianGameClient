using ConquiánCliente.Utilities;
using System.Windows;

namespace ConquiánCliente.View.MainMenu
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            try
            {
                VolumeSlider.Value = Properties.Settings.Default.MusicVolume;
            }
            catch
            {
                VolumeSlider.Value = 50;
            }
        }

        private void ClickSpanish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "es-MX";
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }

        private void ClickEnglish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAndClose();
        }

        private void SaveAndClose()
        {
            Properties.Settings.Default.Save();
            this.DialogResult = true;
            this.Close();
        }

        private void VolumeSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioManager.Instance.SetVolume(e.NewValue);
            Properties.Settings.Default.MusicVolume = e.NewValue;
        }
    }
}
