using Akuntansi.Model.Root;
using Akuntansi.View.Index;
using Syncfusion.Licensing;
using Xamarin.Forms;

namespace Akuntansi
{
    public partial class App : Application
    {
        private readonly PermissionModel permissionModel = new PermissionModel();

        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense(AppLicenseModel.SerialKey);

            InitializeComponent();
            MainPage = new NavigationPage(new LoginView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            permissionModel.RequestPermission();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
