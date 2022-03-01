using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Transaction
{
    public class TransaksiBaruModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TransaksiBaruModel()
        {
            
        }

        public Command TransaksiBaru { get; set; }

        public List<TransaksiBaruModel> ListKategori { get; set; }
        public List<TransaksiBaruModel> ListTipe { get; set; }

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

        private string tipe = string.Empty;

        public string Tipe
        {
            get { return tipe; }
            set { SetProperty(ref tipe, value); }
        }

        private string kategori = string.Empty;

        public string Kategori
        {
            get { return kategori; }
            set { SetProperty(ref kategori, value); }
        }

        private string selectedtipe = string.Empty;

        public string SelectedTipe
        {
            get { return selectedtipe; }
            set { SetProperty(ref selectedtipe, value); }
        }

        private string selectedkategori = string.Empty;

        public string SelectedKategori
        {
            get { return selectedkategori; }
            set { SetProperty(ref selectedkategori, value); }
        }

        private string selectedtanggal = string.Empty;

        public string SelectedTanggal
        {
            get { return selectedtanggal; }
            set { SetProperty(ref selectedtanggal, value); }
        }

        private string username = string.Empty;

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string keterangan = string.Empty;

        public string Keterangan
        {
            get { return keterangan; }
            set
            {
                keterangan = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Keterangan"));
            }
        }

        private string jumlah = string.Empty;

        public string Jumlah
        {
            get { return jumlah; }
            set
            {
                jumlah = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Jumlah"));
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
