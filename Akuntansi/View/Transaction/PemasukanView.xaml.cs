using Akuntansi.ViewModel.Transaction;
using Xamarin.Forms;

namespace Akuntansi.View.Transaction
{
    public partial class PemasukanView : ContentPage
    {
        private readonly PemasukanViewModel pemasukanViewModel;

        public PemasukanView(string username)
        {
            InitializeComponent();
            pemasukanViewModel = new PemasukanViewModel(username, Navigation);
            BindingContext = pemasukanViewModel;
        }
    }
}
