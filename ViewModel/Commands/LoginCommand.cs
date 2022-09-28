using EvernoteClone.Model;
using System;
using System.Windows.Input;
#pragma warning disable CS8618
namespace EvernoteClone.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginVM LoginVM { get; set; }

        public LoginCommand(LoginVM loginVM)
        {
            LoginVM = loginVM;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;
            if(user == null)
            {
                return false;
            }
            if(string.IsNullOrEmpty(user.UserName) )
            {
                return false;
            }
            if(string.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            return true;
        }

        public void Execute(object? parameter)
        {
            LoginVM.Login();
        }
    }
}
