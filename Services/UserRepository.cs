using Firebase.Database;
using Firebase.Database.Query;

using ShareCart.Interfaea;
using ShareCart.Models;

namespace ShareCart.Services;

public class UserRepository : IUserRepository {

    private readonly FirebaseClient firebase =
         new("https://sharecart-5c350-default-rtdb.europe-west1.firebasedatabase.app/");

    public async Task SaveUserEmailAsync(string nodeName, User user) {
        await firebase
           .Child(nodeName)
           .Child(user.Id)
           .PutAsync(user);

    }
}

