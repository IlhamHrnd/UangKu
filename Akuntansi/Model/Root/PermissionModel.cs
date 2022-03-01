using Xamarin.Essentials;

namespace Akuntansi.Model.Root
{
    public class PermissionModel
    {
        public PermissionModel()
        {

        }

        public async void RequestPermission()
        {
            var StorageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (StorageStatus != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>();
            }
        }
    }
}
