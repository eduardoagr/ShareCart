using ShareCart.Models;

namespace ShareCart.Interfaces;

public interface IUserRepoService {

    Task<bool> SaveUserAsync(FirebaseUser user, string nodeName = "users");

    Task UpdateLastLoginAsync(string userId, string nodeName = "users");

    Task<IEnumerable<FirebaseUser>> GetFirebaseUsers(string nodeName = "users");

    Task<FirebaseUser> GetFirebaseUser(string UserId, string nodeName = "users");

    public Task UpdateFirebaseUser(string UserId, string Name, string ColorHex, string nodeName = "users");
}
