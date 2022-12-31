namespace BlazorCrud.Client.Auth;

public interface ILoginService
{
    Task Login(string token);
    Task Logout();
}