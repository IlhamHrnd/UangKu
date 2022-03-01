using Xamarin.Essentials;

namespace Akuntansi.Model.Root
{
    public class DeviceCheckModel
    {
        private bool cekjaringan()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CekJaringan
        {
            get { return cekjaringan(); }
        }
    }
}
