using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable CS8618
namespace EvernoteClone.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler Authenticated;
        private bool _isShowingRegister = false;
        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                onPropertyChanged(nameof(User));
            }
        }

        private string _userName;

        public string username
        {
            get { return _userName; }
            set
            { 
                _userName = value;
                User = new()
                {
                    UserName = username,
                    Password = password
                };
                onPropertyChanged(nameof(username));
            }
        }

        private string _password;

        public string password
        {
            get { return _password; }
            set
            { 
                _password = value;
                User = new()
                {
                    UserName = username,
                    Password = password
                };
                onPropertyChanged(nameof(password));
            }
        }

        private string _name;

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                User = new()
                {
                    Name = name,
                    //LastName = lastname,
                    UserName = username,
                    Password = password,
                };
                onPropertyChanged(nameof(name));
            }
        }

        private string _lastname;

        public string lastname
        {
            get { return _lastname; }
            set
            {
                _name = value;
                User = new()
                {
                    Name = name,
                    LastName = lastname,
                    UserName = username,
                    Password = password,
                };
                onPropertyChanged(nameof(lastname));
            }
        }

        private string _confirmPassword;

        public string confirmPassword
        {
            get { return _confirmPassword; }
            set
            { 
                _confirmPassword = value;
                User = new()
                {
                    UserName = username,
                    Password = password,
                    Name = name,
                    LastName = lastname,
                    ConfirmPassword = confirmPassword
                };
                onPropertyChanged(nameof(confirmPassword));
            }
        }


        private Visibility _loginVisibility;

        public Visibility LoginVisibility
        {
            get { return _loginVisibility; }
            set 
            {
                _loginVisibility = value;
                onPropertyChanged(nameof(LoginVisibility));
            }
        }

        private Visibility _registerVisibility;

        public Visibility RegisterVisibility
        {
            get { return _registerVisibility; }
            set
            {
                _registerVisibility = value;
                onPropertyChanged(nameof(RegisterVisibility));
            }
        }

        #endregion

        #region Commands
        public RegisterCommand RegisterCommand { get; set; }
		public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }
        #endregion

        #region Constructor
        public LoginVM()
		{
            LoginVisibility = Visibility.Visible;
            RegisterVisibility = Visibility.Collapsed;
			RegisterCommand = new(this);
			LoginCommand = new(this);
            ShowRegisterCommand = new(this);
            User = new();
		}
        #endregion

        #region Methods
        public void onPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }



        public void SwitchViews()
        {
            _isShowingRegister = !_isShowingRegister;
            if (_isShowingRegister)
            {
                RegisterVisibility = Visibility.Visible;
                LoginVisibility = Visibility.Collapsed;
            }
            else
            {
                RegisterVisibility = Visibility.Collapsed;
                LoginVisibility = Visibility.Visible;
            }
        }
        public async Task<bool> Login()
        {
            bool isloggedin = await FirebaseHelper.Login(User);
            if(isloggedin)
            {
                Authenticated?.Invoke(this, new EventArgs());
                return true;
            }
            return false;
        }
        public async Task Register()
        {
            bool isRegistered = await FirebaseHelper.Register(User);
            if(isRegistered)
            {
                RegisterVisibility = Visibility.Collapsed;
                LoginVisibility = Visibility.Visible;
            }
        }
        #endregion
    }
}
