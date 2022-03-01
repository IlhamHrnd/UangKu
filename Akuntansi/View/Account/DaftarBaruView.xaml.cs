using Akuntansi.ViewModel.Account;
using Xamarin.Forms;

namespace Akuntansi.View.Account
{
    public partial class DaftarBaruView : ContentPage
    {
        private readonly DaftarBaruViewModel daftarBaruViewModel;

        public DaftarBaruView()
        {
            InitializeComponent();
            daftarBaruViewModel = new DaftarBaruViewModel(Navigation);
            BindingContext = daftarBaruViewModel;
        }
    }
}
