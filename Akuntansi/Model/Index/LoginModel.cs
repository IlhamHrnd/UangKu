using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Index
{
    public class LoginModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginModel()
        {

        }

        public Command Login { get; set; }
        public Command ResetPassword { get; set; }
        public Command DaftarBaru { get; set; }

        private string akses = string.Empty;

        public string Akses
        {
            get { return akses; }
            set { SetProperty(ref akses, value); }
        }

        private DateTime time = DateTime.Now;

        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        private bool isbusy = false;

        public bool IsBusy
        {
            get { return isbusy; }
            set { SetProperty(ref isbusy, value); }
        }

        private string loadingtxt = string.Empty;

        public string LoadingTxt
        {
            get { return loadingtxt; }
            set { SetProperty(ref loadingtxt, value); }
        }

        private string username = string.Empty;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }
            else
            {
                backingStore = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}
