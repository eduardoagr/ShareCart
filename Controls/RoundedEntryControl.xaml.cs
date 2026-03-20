namespace ShareCart.Controls;

public partial class RoundedEntryControl : ContentView {
    public RoundedEntryControl() {
        InitializeComponent();
        EyeGlyph = "\ue8f4";
    }

    // 🔹 Is this a password field?
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(RoundedEntryControl),
            false,
            propertyChanged: OnPasswordPropertyChanged);

    public bool IsPassword {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    // 🔹 Placeholder
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(RoundedEntryControl),
            string.Empty);

    public string Placeholder {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    // 🔹 Text (TwoWay binding)
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(RoundedEntryControl),
            string.Empty,
            BindingMode.TwoWay);

    public string Text {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // 🔹 Eye icon
    public static readonly BindableProperty EyeGlyphProperty =
        BindableProperty.Create(
            nameof(EyeGlyph),
            typeof(string),
            typeof(RoundedEntryControl),
            string.Empty);

    public string EyeGlyph {
        get => (string)GetValue(EyeGlyphProperty);
        set => SetValue(EyeGlyphProperty, value);
    }

    // 🔹 Runtime state (hidden/visible)
    public static readonly BindableProperty IsPasswordHiddenProperty =
        BindableProperty.Create(
            nameof(IsPasswordHidden),
            typeof(bool),
            typeof(RoundedEntryControl),
            true,
            propertyChanged: OnPasswordPropertyChanged);

    public bool IsPasswordHidden {
        get => (bool)GetValue(IsPasswordHiddenProperty);
        set => SetValue(IsPasswordHiddenProperty, value);
    }

    // 🔹 FINAL computed value used by Entry
    public bool ComputedIsPassword => IsPassword && IsPasswordHidden;

    private static void OnPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
        var control = (RoundedEntryControl)bindable;
        control.OnPropertyChanged(nameof(ComputedIsPassword));
    }

    private void VisibilityLabel_Tapped(object sender, TappedEventArgs e) {
        IsPasswordHidden = !IsPasswordHidden;

        EyeGlyph = IsPasswordHidden ? "\ue8f4" : "\ue8f5";
    }
}