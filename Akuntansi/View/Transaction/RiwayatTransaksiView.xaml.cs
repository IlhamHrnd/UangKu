using System;
using Akuntansi.ViewModel.Transaction;
using Syncfusion.XForms.Backdrop;

namespace Akuntansi.View.Transaction
{
    public partial class RiwayatTransaksiView : SfBackdropPage
    {
        private readonly RiwayatTransaksiViewModel riwayatTransaksiViewModel;

        public RiwayatTransaksiView(string username)
        {
            InitializeComponent();
            riwayatTransaksiViewModel = new RiwayatTransaksiViewModel(Navigation, username, Search_Data, LV_Transaksi, Radio_Tipe, Picker_Kategori, Date_Transaksi);
            BindingContext = riwayatTransaksiViewModel;
        }

        private void Btn_Update_Clicked(object sender, EventArgs e)
        {
            riwayatTransaksiViewModel.UpdateData_Command();
        }

        private void Btn_Delete_Clicked(object sender, EventArgs e)
        {
            riwayatTransaksiViewModel.DeleteData_Command();
        }
    }
}
