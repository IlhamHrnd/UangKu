using Akuntansi.ViewModel.Root;
using Xamarin.Forms;

namespace Akuntansi.View.Root
{
    public partial class ConnectionErrorView : ContentPage
    {
        private readonly ConnectionErrorViewModel connectionErrorViewModel;

        public ConnectionErrorView()
        {
            InitializeComponent();
            connectionErrorViewModel = new ConnectionErrorViewModel(Navigation);
            BindingContext = connectionErrorViewModel;
        }
    }
}
