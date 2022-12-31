using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorCrud.Client.Helpers;

public static class IJSExtensions
{
    public static Task SaveAs(this IJSRuntime js, string fileName, byte[] file)
    {
        return js.InvokeAsync<object>("saveAsFile", 
            fileName,
            Convert.ToBase64String(file)).AsTask();
    }

    public static Task ShowAlert(this IJSRuntime js, string message)
    {
        return js.InvokeAsync<object>("Swal.fire", message).AsTask();
    }
    
    public static Task ShowAlert(this IJSRuntime js, string message, string title, TypeMessageSweetAlert type)
    {
        return js.InvokeAsync<object>("Swal.fire", title, message, type.ToString()).AsTask();
    }

    public static async Task<bool> Confirm(this IJSRuntime js, string title, string message, TypeMessageSweetAlert type)
    {
        return await js.InvokeAsync<bool>("customConfirm", title, message, type.ToString());
    }
    
    public static Task SetInLocalStorage(this IJSRuntime js, string key, string content) => js.InvokeAsync<object>(
            "localStorage.setItem",
            key, content
        ).AsTask();

    public static Task<string> GetFromLocalStorage(this IJSRuntime js, string key)
        => js.InvokeAsync<string>(
            "localStorage.getItem",
            key
        ).AsTask();

    public static Task RemoveItem(this IJSRuntime js, string key)
        => js.InvokeAsync<object>(
            "localStorage.removeItem",
            key).AsTask();

    public enum TypeMessageSweetAlert
    {
        question, warning, error, success, info
    }
}