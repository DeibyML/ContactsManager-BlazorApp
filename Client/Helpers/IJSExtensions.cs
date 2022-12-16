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
}