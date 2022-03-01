using Akuntansi.ViewModel.Account;
using Syncfusion.XForms.Backdrop;

namespace Akuntansi.View.Account
{
    public partial class LupaPasswordView : SfBackdropPage
    {
        private readonly LupaPasswordViewModel lupaPasswordViewModel;

        public LupaPasswordView()
        {
            InitializeComponent();
            lupaPasswordViewModel = new LupaPasswordViewModel(Navigation);
            BindingContext = lupaPasswordViewModel;
        }
    }
}
