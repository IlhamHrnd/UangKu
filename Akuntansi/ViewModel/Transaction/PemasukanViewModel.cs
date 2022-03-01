using System;
using System.Collections.Generic;
using System.Data;
using Akuntansi.Model.Root;
using Akuntansi.Model.Transaction;
using Akuntansi.View.Root;
using MySqlConnector;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Transaction
{
    public class PemasukanViewModel : PemasukanModel
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

        private INavigation _navigation;

        public PemasukanViewModel(string username, INavigation navigation)
        {
            Username = username;
            _navigation = navigation;

            LoadData();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //Meload Beberapa Data Saat Menu Muncul
        private async void LoadData()
        {
            Month = Time.ToString("MMMM yyyy", Culture);

            dt = new DataTable();
            start = new DateTime(Time.Year, Time.Month, 1);
            end = start.AddMonths(1);
            ListPemasukan = new List<PemasukanModel>();
            bool valid_connect = deviceCheckModel.CekJaringan;
            if (valid_connect)
            {
                IsBusy = true;
                LoadingTxt = "Loading Data";
                HubungDB();
                if (sqlconn.State == ConnectionState.Open)
                {
                    sqlcmd = sqlconn.CreateCommand();
                    sqlcmd.CommandText = "Select Sum(Jumlah) AS Jumlah, Kategori From Transaksi Where Username = @Username And Tipe = @Tipe And Tanggal >= @TanggalAwal And Tanggal <= @TanggalAkhir Group By Kategori";
                    sqlcmd.Parameters.AddWithValue("@Username", Username);
                    sqlcmd.Parameters.AddWithValue("@Tipe", "Pemasukan");
                    sqlcmd.Parameters.AddWithValue("@TanggalAwal", start);
                    sqlcmd.Parameters.AddWithValue("@TanggalAkhir", end);
                    sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        string kategori = Convert.ToString(sqldr["Kategori"]);
                        double jumlah = Convert.ToDouble(sqldr["Jumlah"]);

                        var item = new PemasukanModel
                        {
                            ListKategori = kategori,
                            ListJumlahChart = jumlah,
                            ListJumlahString = jumlah.ToString("C", Culture)
                        };
                        ListPemasukan.Add(item);
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
