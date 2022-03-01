using Xamarin.Forms;
using Akuntansi.Model.Index;
using Xamarin.Essentials;
using Akuntansi.Model.Root;
using Akuntansi.View.Root;
using MySqlConnector;
using System.Data;
using System;
using System.Threading.Tasks;
using Akuntansi.View.Account;
using Akuntansi.View.Home;

namespace Akuntansi.ViewModel.Index
{
    public class LoginViewModel : LoginModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();
        private readonly PasswordHashModel passwordHashModel = new PasswordHashModel();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;
        private MySqlDataReader sqldr;

        private string conn;

        private INavigation _navigation;

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;

            Login = new Command(LoginPage);
            DaftarBaru = new Command(DaftarBaruPage);
            ResetPassword = new Command(ResetPasswordPage);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //Proses Saat User Ingin Login
        private async void LoginPage()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Harap Isi Semua Data Terlebih Dahulu", "OK");
            }
            else
            {
                bool valid_connect = deviceCheckModel.CekJaringan;
                if (valid_connect)
                {
                    IsBusy = true;
                    LoadingTxt = "Cek Username";
                    await Task.Delay(1500);
                    bool valid_data = CekData();
                    if (valid_data)
                    {
                        switch (Akses)
                        {
                            case "Admin":
                                LoginLog();
                                await _navigation.PushAsync(new HomeAdminView(Username));
                                break;

                            default:
                                LoginLog();
                                await _navigation.PushAsync(new HomeUserView(Username));
                                break;
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Username Tidak Ditemukan", "OK");
                    }
                }
                else
                {
                    await _navigation.PushAsync(new ConnectionErrorView());
                }
                IsBusy = false;
                LoadingTxt = string.Empty;
                Username = string.Empty;
                Password = string.Empty;
            }
        }

        //Validasi Username Dan Password
        private bool CekData()
        {
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Username, Password, Akses From Login Where Username = @Username And Password = @Password";
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqlcmd.Parameters.AddWithValue("@Password", passwordHashModel.EncodePassword(Password));
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.Read())
                {
                    Akses = Convert.ToString(sqldr["Akses"]);
                    sqlconn.Close();
                    return true;
                }
                else
                {
                    sqlconn.Close();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //Menambahkan Waktu User Login Pada DB
        private async void LoginLog()
        {
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Insert Into Login_Log Values(@Username, @Waktu)";
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqlcmd.Parameters.AddWithValue("@Waktu", Time.ToString("yyyy-MM-dd"));
                if (sqlcmd.ExecuteNonQuery() != 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Gagal Menambahkan User Log", "OK");
                }
            }
            sqlconn.Close();
        }

        //Menuju Halaman Daftar Baru
        private async void DaftarBaruPage()
        {
            await _navigation.PushAsync(new DaftarBaruView());
        }

        //Menuju Halaman Reset Password
        private async void ResetPasswordPage()
        {
            await _navigation.PushAsync(new LupaPasswordView());
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
