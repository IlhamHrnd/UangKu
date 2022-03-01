using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfNavigationDrawer.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.SfPullToRefresh.XForms.iOS;
using Syncfusion.XForms.iOS.Backdrop;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.Cards;
using Syncfusion.XForms.iOS.Core;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.iOS.TextInputLayout;
using Syncfusion.XForms.Pickers.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Akuntansi.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App());
            SfAvatarViewRenderer.Init();
            SfBorderRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfButtonRenderer.Init();
            SfPullToRefreshRenderer.Init();
            SfBusyIndicatorRenderer.Init();
            SfBackdropPageRenderer.Init();
            SfCardViewRenderer.Init();
            SfListViewRenderer.Init();
            SfRadioButtonRenderer.Init();
            SfDatePickerRenderer.Init();
            SfPickerRenderer.Init();
            SfNavigationDrawerRenderer.Init();
            SfChartRenderer.Init();
            SfTabViewRenderer.Init();
            SfGradientViewRenderer.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}
