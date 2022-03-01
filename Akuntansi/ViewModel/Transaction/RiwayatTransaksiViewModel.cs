using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Akuntansi.Model.Helper;
using Akuntansi.Model.Root;
using Akuntansi.Model.Transaction;
using Akuntansi.View.Root;
using MySqlConnector;
using Syncfusion.ListView.XForms;
using Syncfusion.SfPicker.XForms;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.Pickers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Transaction
{
    public class RiwayatTransaksiViewModel : RiwayatTransaksiModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();
        private readonly ListHelper listHelper = new ListHelper();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;
        private MySqlDataReader sqldr;

        private string conn;

        private INavigation _navigation;

        private SearchBar _search_data;

        private SfListView _lv_transaksi;

        private SfRadioGroup _radio_tipe;

        private SfPicker _picker_kategori;

        private SfDatePicker _date_transaksi;

        public RiwayatTransaksiViewModel(INavigation navigation, string username, SearchBar search_Data, SfListView lV_Transaksi, SfRadioGroup radio_Tipe, SfPicker picker_Kategori, SfDatePicker date_Transaksi)
        {
            _navigation = navigation;
            Username = username;
            _search_data = search_Data;
            _lv_transaksi = lV_Transaksi;
            _radio_tipe = radio_Tipe;
            _picker_kategori = picker_Kategori;
            _date_transaksi = date_Transaksi;

            LoadData();

            UpdateCommand = new Command(UpdateDatabase_Command);

            _search_data.TextChanged += _search_data_TextChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _radio_tipe.CheckedChanged += _radio_tipe_CheckedChanged;
            _picker_kategori.SelectionChanged += _picker_kategori_SelectionChanged;
            _date_transaksi.DateSelected += _date_transaksi_DateSelected;
            _lv_transaksi.Swiping += _lv_transaksi_Swiping;
            _lv_transaksi.SelectionChanging += _lv_transaksi_SelectionChanging;
            _lv_transaksi.SwipeEnded += _lv_transaksi_SwipeEnded;
        }

        //Meload Beberapa Data Saat Menu Muncul
        private async void LoadData()
        {
            ListTransaksi = new List<RiwayatTransaksiModel>();
            bool valid_connect = deviceCheckModel.CekJaringan;
            if (valid_connect)
            {
                HubungDB();
                if (sqlconn.State == ConnectionState.Open)
                {
                    sqlcmd = sqlconn.CreateCommand();
                    sqlcmd.CommandText = "Select * From Transaksi Where Username = @Username Order By ID Desc";
                    sqlcmd.Parameters.AddWithValue("@Username", Username);
                    sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        int id = Convert.ToInt32(sqldr["ID"]);
                        string keterangan = Convert.ToString(sqldr["Keterangan"]);
                        string tipe = Convert.ToString(sqldr["Tipe"]);
                        string kategori = Convert.ToString(sqldr["Kategori"]);
                        double jumlah = Convert.ToDouble(sqldr["Jumlah"]);
                        DateTime tanggal = Convert.ToDateTime(sqldr["Tanggal"]);

                        var item = new RiwayatTransaksiModel
                        {
                            TransaksiID = id,
                            TransaksiKeterangan = keterangan,
                            TransaksiJumlah = jumlah.ToString("C", Culture),
                            TransaksiTipe = tipe,
                            TransaksiKategori = kategori,
                            TransaksiTanggal = tanggal.ToString("dd MMMM yyyy", Culture)
                        };
                        ListTransaksi.Add(item);
                    }
                }
                sqlconn.Close();
            }
            else
            {
                await _navigation.PushAsync(new ConnectionErrorView());
            }

            ListTipe = new List<RiwayatTransaksiModel>
            {
                new RiwayatTransaksiModel() {Tipe = "Pemasukan"},
                new RiwayatTransaksiModel() {Tipe = "Pengeluaran"}
            };

            SelectedTanggal = Time.Date.ToString("yyyy-MM-dd");
        }

        //Mencari Data Sesuai Keyword
        private void _search_data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_lv_transaksi.DataSource != null)
            {
                _lv_transaksi.DataSource.Filter = FilterTransaksi;
                _lv_transaksi.DataSource.RefreshFilter();
            }
        }

        //Memeriksa Data Jika Tidak Null
        private bool FilterTransaksi(object obj)
        {
            if (_search_data.Text == null)
            {
                return true;
            }
            else
            {
                var transaksi = obj as RiwayatTransaksiModel;

                if (transaksi.TransaksiKeterangan.ToLower().Contains(_search_data.Text.ToLower()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //Memperbarui Data Yang Dipilih Untuk Disimpan Pada Database
        private async void UpdateDatabase_Command()
        {
            if (SwipeID != 0)
            {
                if (string.IsNullOrEmpty(Keterangan) || string.IsNullOrEmpty(Jumlah) || string.IsNullOrEmpty(SelectedTipe) || string.IsNullOrEmpty(SelectedKategori) || string.IsNullOrEmpty(SelectedTanggal))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Lengkapi Data Terlebih Dahulu", "OK");
                }
                else
                {
                    bool valid_connect = deviceCheckModel.CekJaringan;
                    if (valid_connect)
                    {
                        IsBusy = true;
                        LoadingTxt = "Memperbarui Transaksi";
                        await Task.Delay(1500);
                        HubungDB();
                        if (sqlconn.State == ConnectionState.Open)
                        {
                            sqlcmd = sqlconn.CreateCommand();
                            sqlcmd.CommandText = "Update Transaksi Set Keterangan = @Keterangan, Jumlah = @Jumlah, Tipe = @Tipe, Kategori = @Kategori, Tanggal = @Tanggal, Username = @Username Where ID = @ID";
                            sqlcmd.Parameters.AddWithValue("@Keterangan", Keterangan);
                            sqlcmd.Parameters.AddWithValue("@Jumlah", Jumlah);
                            sqlcmd.Parameters.AddWithValue("@Tipe", SelectedTipe);
                            sqlcmd.Parameters.AddWithValue("@Kategori", SelectedKategori);
                            sqlcmd.Parameters.AddWithValue("@Tanggal", SelectedTanggal);
                            sqlcmd.Parameters.AddWithValue("@Username", Username);
                            sqlcmd.Parameters.AddWithValue("@ID", SwipeID);
                            if (sqlcmd.ExecuteNonQuery() == 1)
                            {
                                await Application.Current.MainPage.DisplayAlert("Berhasil", "Transaksi Berhasil Diubah", "OK");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Transaksi Gagal Diubah", "OK");
                            }
                        }
                        IsBackLayerRevealed = true;
                        IsEnabled = false;
                        IsBusy = false;
                        LoadingTxt = string.Empty;
                        SwipeID = 0;
                        SwipeKeterangan = string.Empty;
                        Keterangan = string.Empty;
                        Jumlah = string.Empty;
                        SelectedTipe = string.Empty;
                        SelectedKategori = string.Empty;
                        SelectedTanggal = string.Empty;
                        sqlconn.Close();
                    }
                    else
                    {
                        await _navigation.PushAsync(new ConnectionErrorView());
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Data Tidak Ditemukan", "OK");
            }
        }

        //Memperbarui Data Yang Dipilih
        public async void UpdateData_Command()
        {
            if (SwipeID != 0)
            {
                IsEnabled = true;
                IsBackLayerRevealed = false;
                Keterangan = SwipeKeterangan;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Data Tidak Ditemukan", "OK");
            }
        }

        //Menghapus Data Yang Dipilih
        public async void DeleteData_Command()
        {
            if (SwipeID != 0)
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert("Hapus Transaksi", "Anda Yakin Ingin Menghapus Transaksi '" + SwipeID + "' ?", "Ya", "Tidak");
                if (confirm)
                {
                    bool valid_connect = deviceCheckModel.CekJaringan;
                    if (valid_connect)
                    {
                        IsBusy = true;
                        LoadingTxt = "Menghapus Transaksi";
                        await Task.Delay(1000);
                        HubungDB();
                        if (sqlconn.State == ConnectionState.Open)
                        {
                            sqlcmd = sqlconn.CreateCommand();
                            sqlcmd.CommandText = "Delete From Transaksi Where ID = @ID";
                            sqlcmd.Parameters.AddWithValue("@ID", SwipeID);
                            if (sqlcmd.ExecuteNonQuery() == 1)
                            {
                                await Application.Current.MainPage.DisplayAlert("Berhasil", "Transaksi Berhasil Dihapus", "OK");
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Transaksi Gagal Dihapus", "OK");
                            }
                        }
                        IsBusy = false;
                        LoadingTxt = string.Empty;
                        SwipeID = 0;
                        SwipeKeterangan = string.Empty;
                        sqlconn.Close();
                    }
                    else
                    {
                        await _navigation.PushAsync(new ConnectionErrorView());
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Data Tidak Ditemukan", "OK");
            }
        }

        //Mengubah List Pada Picker Saat Radio Button Dipilih
        private void _radio_tipe_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            if (_radio_tipe.CheckedItem != null)
            {
                SelectedTipe = _radio_tipe.CheckedItem.Text;

                switch (SelectedTipe)
                {
                    case "Pemasukan":
                        _picker_kategori.ItemsSource = listHelper.Pemasukan;
                        break;
                    case "Pengeluaran":
                        _picker_kategori.ItemsSource = listHelper.Pengeluaran;
                        break;
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

        //Mengembalikan Listview Ke Posisi Awal Jika Terswipe Jauh
        private void _lv_transaksi_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            if (e.SwipeOffset > 200)
            {
                _lv_transaksi.ResetSwipe();
                SwipeID = 0;
                SwipeKeterangan = string.Empty;
            }
        }

        //Mereset Listview Saat Item Yang Terselect Diubah
        private void _lv_transaksi_SelectionChanging(object sender, ItemSelectionChangingEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                e.Cancel = true;
            }
        }

        //Mengambil Data Transaksi Saat User Men Swipe ListView
        private void _lv_transaksi_Swiping(object sender, SwipingEventArgs e)
        {
            if (e.SwipeOffSet >= 180)
            {
                var Transaksi = e.ItemData as RiwayatTransaksiModel;
                SwipeID = Transaksi.TransaksiID;
                SwipeKeterangan = Transaksi.TransaksiKeterangan;
            }
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
