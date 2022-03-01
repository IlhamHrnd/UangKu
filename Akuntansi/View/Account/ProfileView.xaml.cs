using Akuntansi.ViewModel.Account;
using Xamarin.Forms;

namespace Akuntansi.View.Account
{
    public partial class ProfileView : ContentPage
    {
        private readonly ProfileViewModel profileViewModel;

        public ProfileView(string username)
        {
            InitializeComponent();
            profileViewModel = new ProfileViewModel(username, Navigation);
            BindingContext = profileViewModel;
        }
    }
}
