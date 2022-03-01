using Akuntansi.ViewModel.Transaction;
using Xamarin.Forms;

namespace Akuntansi.View.Transaction
{
    public partial class TransaksiBaruView : ContentPage
    {
        private readonly TransaksiBaruViewModel transaksiBaruViewModel;

        public TransaksiBaruView(string username)
        {
            InitializeComponent();
            transaksiBaruViewModel = new TransaksiBaruViewModel(Navigation, username, Radio_Tipe, Picker_Kategori, Date_Transaksi);
            BindingContext = transaksiBaruViewModel;
        }
    }
}
