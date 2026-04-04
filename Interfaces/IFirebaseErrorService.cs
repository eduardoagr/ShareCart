namespace ShareCart.Interfaces;

public interface IFirebaseErrorService {

    string GetMessage(Exception ex);
}
