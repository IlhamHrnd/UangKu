using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Account
{
    public class DaftarBaruModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DaftarBaruModel()
        {

        }

        public Command DaftarBaru { get; set; }

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

        private string akses = "User";

        public string Akses
        {
            get { return akses; }
            set { SetProperty(ref akses, value); }
        }

        private int limit = 2000000;

        public int Limit
        {
            get { return limit; }
            set { SetProperty(ref limit, value); }
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

        private string confirmpass = string.Empty;

        public string ConfirmPass
        {
            get { return confirmpass; }
            set
            {
                confirmpass = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmPass"));
            }
        }

        private string namadepan = string.Empty;

        public string NamaDepan
        {
            get { return namadepan; }
            set
            {
                namadepan = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NamaDepan"));
            }
        }

        private string namabelakang = string.Empty;

        public string NamaBelakang
        {
            get { return namabelakang; }
            set
            {
                namabelakang = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NamaBelakang"));
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
