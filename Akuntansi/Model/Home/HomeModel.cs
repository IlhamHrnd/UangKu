using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Home
{
    public class HomeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public HomeModel()
        {

        }

        public Command HomeNavigation { get; set; }
        public Command TransaksiBaru { get; set; }
        public Command RiwayatTransaksi { get; set; }
        public Command LaporanPemasukan { get; set; }
        public Command Profile { get; set; }

        public List<HomeModel> ListTransaksiTerakhir { get; set; }

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

        private string listkeretangan = string.Empty;

        public string ListKeterangan
        {
            get { return listkeretangan; }
            set { SetProperty(ref listkeretangan, value); }
        }

        private string listtipe = string.Empty;

        public string ListTipe
        {
            get { return listtipe; }
            set { SetProperty(ref listtipe, value); }
        }

        private string listkategori = string.Empty;

        public string ListKategori
        {
            get { return listkategori; }
            set { SetProperty(ref listkategori, value); }
        }

        private string listjumlah = string.Empty;

        public string ListJumlah
        {
            get { return listjumlah; }
            set { SetProperty(ref listjumlah, value); }
        }

        private string listtanggal = string.Empty;

        public string ListTanggal
        {
            get { return listtanggal; }
            set { SetProperty(ref listtanggal, value); }
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

        private string greetingimg = string.Empty;

        public string GreetingIMG
        {
            get { return greetingimg; }
            set { SetProperty(ref greetingimg, value); }
        }

        private string greeting = string.Empty;

        public string Greeting
        {
            get { return greeting; }
            set { SetProperty(ref greeting, value); }
        }

        private string month = string.Empty;

        public string Month
        {
            get { return month; }
            set { SetProperty(ref month, value); }
        }

        private string pemasukan = string.Empty;

        public string Pemasukan
        {
            get { return pemasukan; }
            set { SetProperty(ref pemasukan, value); }
        }

        private string pengeluaran = string.Empty;

        public string Pengeluaran
        {
            get { return pengeluaran; }
            set { SetProperty(ref pengeluaran, value); }
        }

        private string total = string.Empty;

        public string Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private string limit = string.Empty;

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
