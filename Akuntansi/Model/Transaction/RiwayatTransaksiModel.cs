using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Akuntansi.Model.Transaction
{
    public class RiwayatTransaksiModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RiwayatTransaksiModel()
        {

        }

        public Command UpdateCommand { get; set; }

        public List<RiwayatTransaksiModel> ListTransaksi { get; set; }
        public List<RiwayatTransaksiModel> ListKategori { get; set; }
        public List<RiwayatTransaksiModel> ListTipe { get; set; }

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

        private int transaksiid = 0;

        public int TransaksiID
        {
            get { return transaksiid; }
            set { SetProperty(ref transaksiid, value); }
        }

        private string transaksiketerangan = string.Empty;

        public string TransaksiKeterangan
        {
            get { return transaksiketerangan; }
            set { SetProperty(ref transaksiketerangan, value); }
        }

        private string transaksijumlah = string.Empty;

        public string TransaksiJumlah
        {
            get { return transaksijumlah; }
            set { SetProperty(ref transaksijumlah, value); }
        }

        private string transaksitipe = string.Empty;

        public string TransaksiTipe
        {
            get { return transaksitipe; }
            set { SetProperty(ref transaksitipe, value); }
        }

        private string transaksikategori = string.Empty;

        public string TransaksiKategori
        {
            get { return transaksikategori; }
            set { SetProperty(ref transaksikategori, value); }
        }

        private string transaksitanggal = string.Empty;

        public string TransaksiTanggal
        {
            get { return transaksitanggal; }
            set { SetProperty(ref transaksitanggal, value); }
        }

        private string tipe = string.Empty;

        public string Tipe
        {
            get { return tipe; }
            set { SetProperty(ref tipe, value); }
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

        private int swipeid = 0;

        public int SwipeID
        {
            get { return swipeid; }
            set { SetProperty(ref swipeid, value); }
        }

        private string swipeketerangan = string.Empty;

        public string SwipeKeterangan
        {
            get { return swipeketerangan; }
            set { SetProperty(ref swipeketerangan, value); }
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
