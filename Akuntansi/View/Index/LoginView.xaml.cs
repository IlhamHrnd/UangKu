using Akuntansi.ViewModel.Index;
using Xamarin.Forms;

namespace Akuntansi.View.Index
{
    public partial class LoginView : ContentPage
    {
        private readonly LoginViewModel loginViewModel;

        public LoginView()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel(Navigation);
            BindingContext = loginViewModel;
        }
    }
}
