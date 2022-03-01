using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Account
{
    public class LupaPasswordModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LupaPasswordModel()
        {

        }

        public Command ResetPassword { get; set; }
        public Command CekUsername { get; set; }

        private bool isreadonly = false;

        public bool IsReadOnly
        {
            get { return isreadonly; }
            set { SetProperty(ref isreadonly, value); }
        }

        private bool isenabled = false;

        public bool IsEnabled
        {
            get { return isenabled; }
            set { SetProperty(ref isenabled, value); }
        }

        private bool isbacklayerrevealed = true;

        public bool IsBackLayerRevealed
        {
            get { return isbacklayerrevealed; }
            set { SetProperty(ref isbacklayerrevealed, value); }
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
