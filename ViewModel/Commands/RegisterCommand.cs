using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
#pragma warning disable CS8618
namespace EvernoteClone.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {

        LoginVM loginVM { get; set; }

        public RegisterCommand(LoginVM loginVM)
        {
            this.loginVM = loginVM;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;
            if (user == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.UserName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            
            if(user.ConfirmPassword != user.Password) 
            {
                return false;
            }
            return true;
        }

        public void Execute(object? parameter)
        {
            loginVM.Register();
        }
    }
}
