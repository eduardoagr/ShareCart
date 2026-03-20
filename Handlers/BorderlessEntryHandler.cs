using Microsoft.Maui.Handlers;

using ShareCart.Controls;

#if ANDROID
using Color = Android.Graphics.Color;
using Android.Graphics;
#elif IOS || MACCATALYST
using UIKit;
#endif

namespace ShareCart.Handlers;

public static class BorderlessEntryHandler {
    public static void Apply() {
        EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) => {
            if(view is BorderlessEntry) {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Color.Transparent);

#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UITextBorderStyle.None;

#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Style = null;
#endif
            }
        });
    }
}
