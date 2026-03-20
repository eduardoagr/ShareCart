using Syncfusion.Licensing;

namespace ShareCart;

public partial class App : Application {
    public App() {

        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjGyl/VkV+XU9AclRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3hTdURqWH5fd3ddRGNbU091XA==");

        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        return new Window(new AppShell());
    }
}