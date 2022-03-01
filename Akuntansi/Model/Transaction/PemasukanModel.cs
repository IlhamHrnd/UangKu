using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Akuntansi.Model.Transaction
{
    public class PemasukanModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PemasukanModel()
        {

        }

        public List<PemasukanModel> ListPemasukan { get; set; }

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

        private string listkategori = string.Empty;

        public string ListKategori
        {
            get { return listkategori; }
            set { SetProperty(ref listkategori, value); }
        }

        private double listjumlahchart = 0;

        public double ListJumlahChart
        {
            get { return listjumlahchart; }
            set { SetProperty(ref listjumlahchart, value); }
        }

        private string listjumlahstring = string.Empty;

        public string ListJumlahString
        {
            get { return listjumlahstring; }
            set { SetProperty(ref listjumlahstring, value); }
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
