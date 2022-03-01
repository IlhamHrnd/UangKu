using Akuntansi.ViewModel.Home;
using Xamarin.Forms;

namespace Akuntansi.View.Home
{
    public partial class HomeAdminView : ContentPage
    {
        private readonly HomeViewModel homeViewModel;

        public HomeAdminView(string username)
        {
            InitializeComponent();
            homeViewModel = new HomeViewModel(Navigation, username, Navigation_Home);
            BindingContext = homeViewModel;
        }

        protected override void OnAppearing()
        {
            homeViewModel.LoadData();
            homeViewModel.Greetings();
        }
    }
}
