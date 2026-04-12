namespace ShareCart.Models;

public class FirebaseUser {

    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Username => Email.Split("@")[0];

    public DateTime CreatedAt { get; set; }

    public DateTime LastLogin { get; set; }

    public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Email : Name;

    public string BubbleColor { get; set; } = "#3498db";
}
