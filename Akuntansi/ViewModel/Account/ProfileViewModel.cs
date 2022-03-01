using System;
using System.Data;
using Akuntansi.Model.Account;
using Akuntansi.Model.Root;
using Akuntansi.View.Root;
using MySqlConnector;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Account
{
    public class ProfileViewModel : ProfileModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;
        private MySqlDataReader sqldr;

        private string conn;

        private INavigation _navigation;

        public ProfileViewModel(string username, INavigation navigation)
        {
            Username = username;
            _navigation = navigation;

            LoadData();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //Meload Data Saat Form Pertama Dibuka
        private async void LoadData()
        {
            bool valid_connect = deviceCheckModel.CekJaringan;
            if (valid_connect)
            {
                IsBusy = true;
                LoadingTxt = "Loading Data";
                HubungDB();
                if (sqlconn.State == ConnectionState.Open)
                {
                    sqlcmd = sqlconn.CreateCommand();
                    sqlcmd.CommandText = "Select Depan, Belakang, Akses, DataLimit From Login Where Username = @Username";
                    sqlcmd.Parameters.AddWithValue("@Username", Username);
                    sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        string depan = Convert.ToString(sqldr["Depan"]);
                        string belakang = Convert.ToString(sqldr["Belakang"]);
                        string akses = Convert.ToString(sqldr["Akses"]);
                        double limit = Convert.ToDouble(sqldr["DataLimit"]);
                        NamaDepan = depan.ToString();
                        NamaBelakang = belakang.ToString();
                        Akses = akses.ToString();
                        Limit = limit.ToString("C", Culture);
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

            if (Akses == "Admin")
            {
                ImgAkses = "Admin.png";
            }
            else
            {
                ImgAkses = "User.png";
            }

            Month = Time.Date.ToString("MMMM yyyy", Culture);
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
