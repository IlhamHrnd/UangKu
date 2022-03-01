using System;
using System.Collections.Generic;
using System.Data;
using Akuntansi.Model.Home;
using Akuntansi.Model.Root;
using Akuntansi.View.Account;
using Akuntansi.View.Root;
using Akuntansi.View.Transaction;
using MySqlConnector;
using Syncfusion.SfNavigationDrawer.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Home
{
    public class HomeViewModel : HomeModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;
        private MySqlDataReader sqldr;

        private string conn;

        private DateTime start;
        private DateTime end;

        private DataTable dt;

        private double pemasukan = 0;
        private double pengeluaran = 0;

        private INavigation _navigation;

        private SfNavigationDrawer _navigation_home;

        public HomeViewModel(INavigation navigation, string username, SfNavigationDrawer navigation_Home)
        {
            _navigation = navigation;
            Username = username;
            _navigation_home = navigation_Home;

            Greetings();
            LoadData();

            TransaksiBaru = new Command(TransaksiBaru_Page);
            RiwayatTransaksi = new Command(RiwayatTransaksi_Page);
            HomeNavigation = new Command(HomeNavigation_Page);
            LaporanPemasukan = new Command(LaporanPemasukan_Page);
            Profile = new Command(Profile_Page);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //Meload Beberapa Data Saat Menu Muncul
        public async void LoadData()
        {
            bool valid_connect = deviceCheckModel.CekJaringan;
            if (valid_connect)
            {
                IsBusy = true;
                LoadingTxt = "Loading Data";
                HubungDB();
                if (sqlconn.State == ConnectionState.Open)
                {
                    LapPemasukan();
                    LapPengeluaran();
                    LapTotal();
                    LapLimit();
                    LapTransaksi();
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

        //Menentukan Salaman Sesuai Waktu
        public void Greetings()
        {
            if (Time.Hour >= 0 && Time.Hour <= 10)
            {
                Greeting = "Pagi";
                GreetingIMG = "Pagi.png";
            }
            else if (Time.Hour > 10 && Time.Hour <= 15)
            {
                Greeting = "Siang";
                GreetingIMG = "Siang.png";
            }
            else if (Time.Hour > 15 && Time.Hour <= 19)
            {
                Greeting = "Sore";
                GreetingIMG = "Sore.png";
            }
            else
            {
                Greeting = "Malam";
                GreetingIMG = "Malam.png";
            }

            Month = Time.ToString("MMMM yyyy", Culture);
        }

        //Menghitung Pemasukan Bulan Saat Ini
        private void LapPemasukan()
        {
            dt = new DataTable();
            start = new DateTime(Time.Year, Time.Month, 1);
            end = start.AddMonths(1);
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Sum(Jumlah) As Total From Transaksi Where Tipe = @Tipe And Tanggal >= @TanggalAwal And Tanggal <= @TanggalAkhir And Username = @Username";
                sqlcmd.Parameters.AddWithValue("@Tipe", "Pemasukan");
                sqlcmd.Parameters.AddWithValue("@TanggalAwal", start);
                sqlcmd.Parameters.AddWithValue("@TanggalAkhir", end);
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    if (sqldr.IsDBNull(0))
                    {
                        pemasukan = Convert.ToDouble(0);
                    }
                    else
                    {
                        pemasukan = Convert.ToDouble(sqldr[0]);
                    }
                }
            }
            sqlconn.Close();
            Pemasukan = pemasukan.ToString("C", Culture);
        }

        //Menghitung Pengeluaran Bulan Ini
        private void LapPengeluaran()
        {
            dt = new DataTable();
            start = new DateTime(Time.Year, Time.Month, 1);
            end = start.AddMonths(1);
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Sum(Jumlah) As Total From Transaksi Where Tipe = @Tipe And Tanggal >= @TanggalAwal And Tanggal <= @TanggalAkhir And Username = @Username";
                sqlcmd.Parameters.AddWithValue("@Tipe", "Pengeluaran");
                sqlcmd.Parameters.AddWithValue("@TanggalAwal", start);
                sqlcmd.Parameters.AddWithValue("@TanggalAkhir", end);
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    if (sqldr.IsDBNull(0))
                    {
                        pengeluaran = Convert.ToDouble(0);
                    }
                    else
                    {
                        pengeluaran = Convert.ToDouble(sqldr[0]);
                    }
                }
            }
            sqlconn.Close();
            Pengeluaran = pengeluaran.ToString("C", Culture);
        }

        //Menghitung Total Pemasukan Dan Pengeluaran Perbulan
        private void LapTotal()
        {
            double total = pemasukan - pengeluaran;
            Total = total.ToString("C", Culture);
        }

        //Menampilkan Limit Pengguna Untuk Perbulannya
        private void LapLimit()
        {
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Username, DataLimit From Login Where Username = @Username";
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    double limit = Convert.ToDouble(sqldr["DataLimit"]);
                    Limit = limit.ToString("C", Culture);
                }
            }
            sqlconn.Close();
        }

        //Menampilkan Beberapa Transaksi Terakhir Yang Berlangsung
        private void LapTransaksi()
        {
            ListTransaksiTerakhir = new List<HomeModel>();
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Keterangan, Tipe, Kategori, Jumlah, Tanggal From Transaksi Where Username = @Username Order By ID Desc Limit 5";
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    string keterangan = Convert.ToString(sqldr["Keterangan"]);
                    string tipe = Convert.ToString(sqldr["Tipe"]);
                    string kategori = Convert.ToString(sqldr["Kategori"]);
                    double jumlah = Convert.ToDouble(sqldr["Jumlah"]);
                    DateTime tanggal = Convert.ToDateTime(sqldr["Tanggal"]);

                    var item = new HomeModel
                    {
                        ListKeterangan = keterangan,
                        ListTipe = tipe,
                        ListKategori = kategori,
                        ListJumlah = jumlah.ToString("C", Culture),
                        ListTanggal = tanggal.ToString("dd MMMM yyyy", Culture)
                    };
                    ListTransaksiTerakhir.Add(item);
                }
            }
            sqlconn.Close();
        }

        //Menuju Halaman Transaksi Baru
        private async void TransaksiBaru_Page()
        {
            await _navigation.PushAsync(new TransaksiBaruView(Username));
        }

        //Menuju Halaman Riwayat Transaksi
        private async void RiwayatTransaksi_Page()
        {
            await _navigation.PushAsync(new RiwayatTransaksiView(Username));
        }

        //Menuju Halaman Laporan Pemasukan
        private async void LaporanPemasukan_Page()
        {
            await _navigation.PushAsync(new PemasukanView(Username));
        }

        //Menuju Halaman Profile Account
        private async void Profile_Page()
        {
            await _navigation.PushAsync(new ProfileView(Username));
        }

        //Membuka Navigasi Pada Home
        private void HomeNavigation_Page()
        {
            _navigation_home.IsOpen = true;
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
