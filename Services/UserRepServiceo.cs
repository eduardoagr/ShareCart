using Firebase.Database;
using Firebase.Database.Query;

using ShareCart.Interfaces;
using ShareCart.Models;

using System.Text.Json;

namespace ShareCart.Services;

public class UserRepService(IFirebaseProvider provider) : IUserRepoService {

    private readonly FirebaseClient firebase = provider.Client;

    public Task<FirebaseUser> GetFirebaseUser(string UserId, string nodeName = "users") {

        var user = firebase.Child(nodeName).Child(UserId).OnceSingleAsync<FirebaseUser>();

        return user;
    }

    public async Task<IEnumerable<FirebaseUser>> GetFirebaseUsers(string nodeName = "users") {

        var firebaseUsers = await firebase
            .Child(nodeName)
            .OnceAsync<FirebaseUser>();

        return firebaseUsers.Select(x => {
            var item = x.Object;
            item.Id = x.Key;
            return item;
        });
    }


    public async Task<bool> SaveUserAsync(FirebaseUser user, string nodeName = "users") {

        await firebase.Child(nodeName).Child(user.Id).PutAsync(user);

        return true;
    }


    public Task UpdateFirebaseUser(string UserId, string Name, string ColorHex, string nodeName = "users") {
        var updateData = new Dictionary<string, object>
        {
            { "Name",  Name }
        };

        if(!string.IsNullOrEmpty(ColorHex))
            updateData.Add("BubbleColor", ColorHex);

        var jsonData = JsonSerializer.Serialize(updateData);


        return firebase.Child(nodeName).Child(UserId).PatchAsync(jsonData);
    }

    public async Task UpdateLastLoginAsync(string userId, string nodeName = "users") {

        await firebase.Child(nodeName).Child(userId).Child("LastLogin").PutAsync(DateTime.UtcNow);

    }
}

