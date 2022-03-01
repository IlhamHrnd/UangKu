using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Akuntansi.Model.Account
{
    public class ProfileModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ProfileModel()
        {

        }

        private CultureInfo culture = CultureInfo.CreateSpecificCulture("id-ID");

        public CultureInfo Culture
        {
            get { return culture; }
            set { SetProperty(ref culture, value); }
        }

        private DateTime time = DateTime.Now;

        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        private string month = string.Empty;

        public string Month
        {
            get { return month; }
            set { SetProperty(ref month, value); }
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

        private string namadepan = string.Empty;

        public string NamaDepan
        {
            get { return namadepan; }
            set { SetProperty(ref namadepan, value); }
        }

        private string namabelakang = string.Empty;

        public string NamaBelakang
        {
            get { return namabelakang; }
            set { SetProperty(ref namabelakang, value); }
        }

        private string imgakses = string.Empty;

        public string ImgAkses
        {
            get { return imgakses; }
            set { SetProperty(ref imgakses, value); }
        }

        private string akses = string.Empty;

        public string Akses
        {
            get { return akses; }
            set { SetProperty(ref akses, value); }
        }

        public string limit = string.Empty;

        public string Limit
        {
            get { return limit; }
            set { SetProperty(ref limit, value); }
        }

        private string username = string.Empty;

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
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
