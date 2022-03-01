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
    public class DaftarBaruViewModel : DaftarBaruModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();
        private readonly KoneksiDBModel koneksiDBModel = new KoneksiDBModel();
        private readonly PasswordHashModel passwordHashModel = new PasswordHashModel();

        private MySqlConnection sqlconn;
        private MySqlCommand sqlcmd;
        private MySqlDataAdapter sqlda;

        private string conn;

        private DataSet ds;

        private INavigation _navigation;

        public DaftarBaruViewModel(INavigation navigation)
        {
            _navigation = navigation;

            DaftarBaru = new Command(DaftarBaruPage);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        //Memproses Data Yang Telah Di Isi
        private async void DaftarBaruPage()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPass) || string.IsNullOrEmpty(NamaDepan) || string.IsNullOrEmpty(NamaBelakang))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Lengkapi Data Terlebih Dahulu", "OK");
            }
            else if (Password != ConfirmPass)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password Tidak Sama", "OK");
            }
            else
            {
                bool valid_connect = deviceCheckModel.CekJaringan;
                if (valid_connect)
                {
                    IsBusy = true;
                    LoadingTxt = "Memeriksa Username";
                    await Task.Delay(1500);
                    bool valid_data = CekData();
                    if (valid_data)
                    {
                        HubungDB();
                        if (sqlconn.State == ConnectionState.Open)
                        {
                            LoadingTxt = "Menambahkan Username Baru";
                            await Task.Delay(1500);
                            sqlcmd = sqlconn.CreateCommand();
                            sqlcmd.CommandText = "Insert Into Login Values(@Username, @Password, @Depan, @Belakang, @Akses, @DataLimit)";
                            sqlcmd.Parameters.AddWithValue("@Username", Username);
                            sqlcmd.Parameters.AddWithValue("@Password", passwordHashModel.EncodePassword(Password));
                            sqlcmd.Parameters.AddWithValue("@Depan", NamaDepan);
                            sqlcmd.Parameters.AddWithValue("@Belakang", NamaBelakang);
                            sqlcmd.Parameters.AddWithValue("@Akses", Akses);
                            sqlcmd.Parameters.AddWithValue("@DataLimit", Limit);
                            if (sqlcmd.ExecuteNonQuery() == 1)
                            {
                                await Application.Current.MainPage.DisplayAlert("Berhasil", "Username Berhasil Ditambahkan", "OK");
                            }
                        }
                        Username = string.Empty;
                        Password = string.Empty;
                        ConfirmPass = string.Empty;
                        NamaDepan = string.Empty;
                        NamaBelakang = string.Empty;
                        sqlconn.Close();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Username Sudah Digunakan", "OK");
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

        //Memeriksa Username Sudah Ada Atau Tidak
        private bool CekData()
        {
            HubungDB();
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlcmd = sqlconn.CreateCommand();
                sqlcmd.CommandText = "Select Username From Login Where Username = @Username";
                sqlcmd.Parameters.AddWithValue("@Username", Username);
                sqlda = new MySqlDataAdapter(sqlcmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                int user = ds.Tables[0].Rows.Count;
                if (user > 0)
                {
                    ds.Clear();
                    sqlconn.Close();
                    return false;
                }
                else
                {
                    ds.Clear();
                    sqlconn.Close();
                    return true;
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
