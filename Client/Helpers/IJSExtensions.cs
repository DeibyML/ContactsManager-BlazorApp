using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorCrud.Client.Helpers;

public static class IJSExtensions
{
    public static ValueTask<object> SaveAs(this IJSRuntime js, string fileName, byte[] file)
    {
        return js.InvokeAsync<object>("saveAsFile", 
            fileName,
            Convert.ToBase64String(file));
    }

    public static ValueTask<object> ShowAlert(this IJSRuntime js, string message)
    {
        return js.InvokeAsync<object>("Swal.fire", message);
    }
    
    public static ValueTask<object> ShowAlert(this IJSRuntime js, string message, string title, TypeMessageSweetAlert type)
    {
        return js.InvokeAsync<object>("Swal.fire", title, message, type.ToString());
    }

    public static async ValueTask<bool> Confirm(this IJSRuntime js, string title, string message, TypeMessageSweetAlert type)
    {
        return await js.InvokeAsync<bool>("customConfirm", title, message, type.ToString());
    }
    
    public static ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content) => js.InvokeAsync<object>(
            "localStorage.setItem",
            key, content
        );

    public static ValueTask<string> GetFromLocalStorage(this IJSRuntime js, string key)
        => js.InvokeAsync<string>(
            "localStorage.getItem",
            key
        );

    public static ValueTask<object> RemoveItem(this IJSRuntime js, string key)
        => js.InvokeAsync<object>(
            "localStorage.removeItem",
            key);

    public enum TypeMessageSweetAlert
    {
        question, warning, error, success, info
    }
}