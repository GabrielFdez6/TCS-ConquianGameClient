using System;
using System.Windows;
using System.Windows.Controls;

namespace ConquiánCliente.View
{
    public partial class ProfileMainFrame : Window
    {
        private static ProfileMainFrame instance;

        private bool isClosed = false;

        public static Frame MainFrame
        {
            get { return GetInstance().ProfileFrame; }
        }

        public static ProfileMainFrame GetInstance()
        {
            if (instance == null || instance.isClosed)
            {
                instance = new ProfileMainFrame();
            }
            return instance;
        }

        private ProfileMainFrame()
        {
            InitializeComponent();
            ProfileFrame.Navigate(new Profile.UserProfilePage());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.isClosed = true;
        }
    }
}