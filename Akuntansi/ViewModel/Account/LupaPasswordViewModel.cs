using System.Data;
using System.Threading.Tasks;
using Akuntansi.Model.Account;
using Akuntansi.Model.Root;
using Akuntansi.View.Root;
using MySqlConnector;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Account
{
    public class LupaPasswordViewModel : LupaPasswordModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();
        private readonly PasswordHashModel passwordHashModel = new PasswordHashModel();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;
        private MySqlDataReader sqldr;

        private string conn;

        private INavigation _navigation;

        public LupaPasswordViewModel(INavigation navigation)
        {
            _navigation = navigation;

            CekUsername = new Command(CekUsername_Page);
            ResetPassword = new Command(ResetPassword_Page);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //Memeriksa Username Sebelum Mengganti Password
        private async void CekUsername_Page()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(NamaDepan) || string.IsNullOrEmpty(NamaBelakang))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Lengkapi Data Terlebih Dahulu", "OK");
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
                        IsBackLayerRevealed = false;
                        IsReadOnly = true;
                        IsEnabled = true;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Username Tidak Ditemukan", "OK");
                    }
                    IsBusy = false;
                    LoadingTxt = string.Empty;
                }
                else
                {
                    await _navigation.PushAsync(new ConnectionErrorView());
                }
            }
        }

        //Memperbarui Password Setelah Data Sudah Sesuai
        private async void ResetPassword_Page()
        {
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPass))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Lengkapi Data Terlebih Dahulu", "OK");
            }
            else if (!Password.Equals(ConfirmPass))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password Tidak Sama", "OK");
            }
            else
            {
                bool valid_connect = deviceCheckModel.CekJaringan;
                if (valid_connect)
                {
                    IsBusy = true;
                    LoadingTxt = "Memperbarui Password";
                    await Task.Delay(1000);
                    HubungDB();
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        sqlcmd = sqlconn.CreateCommand();
                        sqlcmd.CommandText = "Update Login Set Password = @Password Where Username = @Username";
                        sqlcmd.Parameters.AddWithValue("@Password", passwordHashModel.EncodePassword(Password));
                        sqlcmd.Parameters.AddWithValue("@Username", Username);
                        if (sqlcmd.ExecuteNonQuery() == 1)
                        {
                            await Application.Current.MainPage.DisplayAlert("Berhasil", "Password Berhasil Diubah", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Password Gagal Diubah", "OK");
                        }
                    }
                    IsBackLayerRevealed = true;
                    IsEnabled = false;
                    IsReadOnly = false;
                    IsBusy = false;
                    LoadingTxt = string.Empty;
                    sqlconn.Close();
                }
                else
                {
                    await _navigation.PushAsync(new ConnectionErrorView());
                }
            }
        }

        //Validasi Username Dan Password
        private bool CekData()
        {
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Username, Depan, Belakang From Login Where Username = @Username And Depan = @Depan And Belakang = @Belakang";
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqlcmd.Parameters.AddWithValue("@Depan", NamaDepan);
                sqlcmd.Parameters.AddWithValue("@Belakang", NamaBelakang);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.Read())
                {
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
