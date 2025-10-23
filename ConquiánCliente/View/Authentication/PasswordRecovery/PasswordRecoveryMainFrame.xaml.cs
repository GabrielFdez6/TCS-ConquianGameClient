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

namespace ConquiánCliente.View.Authentication.PasswordRecovery
{
    /// <summary>
    /// Lógica de interacción para PasswordRecoveryMainFrame.xaml
    /// </summary>
    public partial class PasswordRecoveryMainFrame : Window
    {
        public PasswordRecoveryMainFrame()
        {
            InitializeComponent();
            RecoveryFrame.Navigate(new RequestRecovery());
        }
    }
}
