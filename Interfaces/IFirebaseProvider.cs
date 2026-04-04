using Firebase.Database;

namespace ShareCart.Interfaces;

public interface IFirebaseProvider {

    FirebaseClient Client { get; }

}
