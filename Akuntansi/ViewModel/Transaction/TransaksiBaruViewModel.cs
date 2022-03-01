using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Akuntansi.Model.Helper;
using Akuntansi.Model.Root;
using Akuntansi.Model.Transaction;
using Akuntansi.View.Root;
using MySqlConnector;
using Syncfusion.SfPicker.XForms;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.Pickers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Transaction
{
    public class TransaksiBaruViewModel : TransaksiBaruModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();
        private readonly ListHelper listHelper = new ListHelper();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;

        private string conn;

        private INavigation _navigation;

        private SfRadioGroup _radio_tipe;

        private SfPicker _picker_kategori;

        private SfDatePicker _date_transaksi;

        public TransaksiBaruViewModel(INavigation navigation, string username, SfRadioGroup radio_Tipe, SfPicker picker_Kategori, SfDatePicker date_Transaksi)
        {
            _navigation = navigation;
            Username = username;
            _radio_tipe = radio_Tipe;
            _picker_kategori = picker_Kategori;
            _date_transaksi = date_Transaksi;

            Greeting();
            LoadData();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _radio_tipe.CheckedChanged += _radio_tipe_CheckedChanged;
            _picker_kategori.SelectionChanged += _picker_kategori_SelectionChanged;
            _date_transaksi.DateSelected += _date_transaksi_DateSelected;

            TransaksiBaru = new Command(TransaksiBaruPage);
        }

        //Meload Data Dari Database
        private void LoadData()
        {
            ListTipe = new List<TransaksiBaruModel>
            {
                new TransaksiBaruModel() {Tipe = "Pemasukan"},
                new TransaksiBaruModel() {Tipe = "Pengeluaran"}
            };

            SelectedTanggal = Time.Date.ToString("yyyy-MM-dd");
        }

        //Mengubah List Pada Picker Saat Radio Button Dipilih
        private void _radio_tipe_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            if (_radio_tipe.CheckedItem != null)
            {
                SelectedTipe = _radio_tipe.CheckedItem.Text;

                if (SelectedTipe == "Pemasukan")
                {
                    _picker_kategori.ItemsSource = listHelper.Pemasukan;
                }
                else if (SelectedTipe == "Pengeluaran")
                {
                    _picker_kategori.ItemsSource = listHelper.Pengeluaran;
                }
            }
        }

        //Mengambil Data Saat Item Dipilih Dari Picker
        private void _picker_kategori_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            if (_picker_kategori.SelectedItem != null)
            {
                SelectedKategori = _picker_kategori.SelectedItem.ToString();
            }
        }

        //Mengambil Data Tanggal Saat User Memilih Tanggal
        private void _date_transaksi_DateSelected(object sender, Syncfusion.XForms.Pickers.DateChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                SelectedTanggal = _date_transaksi.Date.ToString("yyyy-MM-dd");
            }
        }

        //Menentukan Bulan Saat Ini
        private void Greeting()
        {
            Month = Time.ToString("MMMM", Culture);
        }

        //Detek Jika Koneksi Tiba Tiba Diubah
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            bool valid_connect = deviceCheckModel.CekJaringan;
            if (!valid_connect)
            {
                await _navigation.PushAsync(new ConnectionErrorView());
            }
        }

        //Proses Menambahkan Data Baru Kedalam Database
        private async void TransaksiBaruPage()
        {
            if (string.IsNullOrEmpty(Keterangan) || string.IsNullOrEmpty(Jumlah) || string.IsNullOrEmpty(SelectedTipe) || string.IsNullOrEmpty(SelectedKategori) || string.IsNullOrEmpty(SelectedTanggal))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Harap Isi Semua Data Terlebih Dahulu", "OK");
            }
            else
            {
                bool valid_connect = deviceCheckModel.CekJaringan;
                if (valid_connect)
                {
                    IsBusy = true;
                    LoadingTxt = "Menambahkan Data Baru";
                    await Task.Delay(1500);
                    HubungDB();
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        sqlcmd = sqlconn.CreateCommand();
                        sqlcmd.CommandText = "Insert Into Transaksi(Keterangan, Jumlah, Tipe, Kategori, Tanggal, Username) Values(@Keterangan, @Jumlah, @Tipe, @Kategori, @Tanggal, @Username)";
                        sqlcmd.Parameters.AddWithValue("@Keterangan", Keterangan);
                        sqlcmd.Parameters.AddWithValue("@Jumlah", Jumlah);
                        sqlcmd.Parameters.AddWithValue("@Tipe", SelectedTipe);
                        sqlcmd.Parameters.AddWithValue("@Kategori", SelectedKategori);
                        sqlcmd.Parameters.AddWithValue("@Tanggal", SelectedTanggal);
                        sqlcmd.Parameters.AddWithValue("@Username", Username);
                        if (sqlcmd.ExecuteNonQuery() == 1)
                        {
                            await Application.Current.MainPage.DisplayAlert("Berhasil", "Transaksi Berhasil Ditambahkan", "OK");
                        }
                    }
                    sqlconn.Close();
                    IsBusy = false;
                    LoadingTxt = string.Empty;
                }
                else
                {
                    await _navigation.PushAsync(new ConnectionErrorView());
                }
            }
        }

        //Menghubungkan Database
        private async void HubungDB()
        {
            conn = koneksiDBModel.DataDB;
            sqlconn = new MySqlConnection(conn);

            try
            {
                sqlconn.Open();
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        await Application.Current.MainPage.DisplayAlert("Gagal", "Gagal Terhubung Ke Server", "OK");
                        break;

                    case 1045:
                        await Application.Current.MainPage.DisplayAlert("Gagal", "Username Atau Password Server Salah", "OK");
                        break;

                    default:
                        await Application.Current.MainPage.DisplayAlert("Gagal", "Terjadi Error", "OK");
                        break;
                }
            }
        }
    }
}
