using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConquiánCliente.View
{
    /// <summary>
    /// Interaction logic for ProfileMainFrame.xaml
    /// </summary>
    public partial class ProfileMainFrame : Window
    {
        public static Frame MainFrame;
        public ProfileMainFrame()
        {
            InitializeComponent();
            MainFrame = ProfileFrame;
            MainFrame.Navigate(new Profile.UserProfilePage());
        }
    }
}
