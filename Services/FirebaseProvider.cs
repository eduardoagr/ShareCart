using Firebase.Database;

using ShareCart.Interfaces;

namespace ShareCart.Services;

public class FirebaseProvider : IFirebaseProvider {

    public FirebaseClient Client { get; }

    public FirebaseProvider() {
        Client = new FirebaseClient("https://sharecart-5c350-default-rtdb.europe-west1.firebasedatabase.app/");
    }

}
