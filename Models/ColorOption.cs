using CommunityToolkit.Mvvm.ComponentModel;

namespace ShareCart.Models {
    public partial class ColorOption : ObservableObject {

        public string Name { get; set; } = string.Empty;

        public Color Color { get; set; }

        [ObservableProperty]
        public partial bool IsSelected { get; set; }

        public string ColorHex => Color.ToHex();
    }

}
