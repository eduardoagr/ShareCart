using ShareCart.Models;

using System.Collections.ObjectModel;

namespace ShareCart.Services {

    public static class ColorService {

        public static ObservableCollection<ColorOption> GetColors() {
            return
            [
            new ColorOption { Name = "Red",    Color = Color.FromArgb("#FF5252") },
            new ColorOption { Name = "Blue",   Color = Color.FromArgb("#448AFF") },
            new ColorOption { Name = "Green",  Color = Color.FromArgb("#4CAF50") },
            new ColorOption { Name = "Yellow", Color = Color.FromArgb("#FFEB3B") },
            new ColorOption { Name = "Purple", Color = Color.FromArgb("#9C27B0") },
            new ColorOption { Name = "Orange", Color = Color.FromArgb("#FF9800") }];
        }
    }
}
