namespace ShareCart.Controls;

public partial class BorderlessEntry : Entry {

    public static readonly BindableProperty ShouldFocusProperty =
            BindableProperty.Create(
                nameof(ShouldFocus),
                typeof(bool),
                typeof(BorderlessEntry),
                false,
                propertyChanged: OnShouldFocusChanged);

    public bool ShouldFocus {
        get => (bool)GetValue(ShouldFocusProperty);
        set => SetValue(ShouldFocusProperty, value);
    }

    private static void OnShouldFocusChanged(BindableObject bindable, object oldValue, object newValue) {
        if(bindable is not BorderlessEntry entry)
            return;

        if(newValue is bool shouldFocus && shouldFocus) {
            MainThread.BeginInvokeOnMainThread(async () => {
                await Task.Yield();
                entry.Focus();
                entry.ShouldFocus = false; // reset
            });
        }
    }
}
