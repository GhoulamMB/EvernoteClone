using EvernoteClone.ViewModel;
using EvernoteClone.ViewModel.Helpers;
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

namespace EvernoteClone.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginVM? LoginVM;
        public LoginWindow()
        {
            InitializeComponent();
            LoginVM = Resources["lvm"] as LoginVM;
            LoginVM!.Authenticated += LoginVM_Authenticated;
        }

        private void LoginVM_Authenticated(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
