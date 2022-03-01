using System.Threading.Tasks;
using Akuntansi.Model.Root;
using Xamarin.Forms;

namespace Akuntansi.ViewModel.Root
{
    public class ConnectionErrorViewModel : ConnectionErrorModel
    {
        private readonly DeviceCheckModel deviceCheckModel = new DeviceCheckModel();

        private INavigation _navigation;

        public ConnectionErrorViewModel(INavigation navigation)
        {
            _navigation = navigation;

            Refresh = new Command(RefreshCommand);
        }

        //Merefresh Data Apa Koneksi Sudah Ada Atau Tidak
        private async void RefreshCommand()
        {
            bool valid_connect = deviceCheckModel.CekJaringan;
            IsRefreshing = true;
            await Task.Delay(1000);
            if (valid_connect)
            {
                await _navigation.PopAsync();
            }
        }
    }
}
