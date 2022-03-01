using Akuntansi.ViewModel.Home;
using Xamarin.Forms;

namespace Akuntansi.View.Home
{
    public partial class HomeUserView : ContentPage
    {
        private readonly HomeViewModel homeViewModel;

        public HomeUserView(string username)
        {
            InitializeComponent(); homeViewModel = new HomeViewModel(Navigation, username, Navigation_Home);
            BindingContext = homeViewModel;
        }

        protected override void OnAppearing()
        {
            homeViewModel.LoadData();
            homeViewModel.Greetings();
        }
    }
}
