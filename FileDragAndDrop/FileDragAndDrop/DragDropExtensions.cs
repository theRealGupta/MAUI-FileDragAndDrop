// DragDropExtensions.cs
//#if WINDOWS

//using Microsoft.Maui.Controls.PlatformConfiguration;
//using Microsoft.UI.Xaml;
//using Windows.ApplicationModel.DataTransfer;
//using Windows.Storage;
//using DataPackageOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation;
//using DragEventArgs = Microsoft.UI.Xaml.DragEventArgs;

//using System.Diagnostics;
//using Windows.Foundation;
using Microsoft.Maui.Platform;

//#endif 


namespace FileDragAndDrop;

//#if WINDOWS
//public static partial class DragDropHelper
//{
//    public static void RegisterDrag(UIElement element, Func<CancellationToken, Task<Stream>> content)
//    {
//        element.CanDrag = true;
//        element.DragStarting += async (s, e) =>
//        {
//            var stream = await content.Invoke(CancellationToken.None);
//            var storageFile = await CreateStorageFile(stream);
//            e.Data.SetStorageItems(new List<IStorageItem>()
//        {
//            storageFile
//        });
//        };
//    }

//    public static void RegisterDrop(UIElement element, Func<Stream, Task>? content)
//    {
//        element.AllowDrop = true;
//        element.Drop += async (s, e) =>
//        {
//            if (e.DataView.Contains(StandardDataFormats.StorageItems))
//            {
//                var items = await e.DataView.GetStorageItemsAsync();
//                foreach (var item in items)
//                {
//                    if (item is StorageFile file)
//                    {
//                        if (content is not null)
//                        {
//                            // content is a callback Func with a Stream argument
//                            // you could also change its signature to string and use
//                            // await content.Invoke(file.Path);

//                            var str = await file.OpenReadAsync();
//                            await content.Invoke(str.AsStream());


//                        }
//                    }
//                }
//            }
//        };
//        element.DragOver += OnDragOver;
//    }

//    public static void UnRegisterDrag(UIElement element)
//    {
//        element.CanDrag = false;
//    }

//    public static void UnRegisterDrop(UIElement element)
//    {
//        element.AllowDrop = false;
//        element.DragOver -= OnDragOver;
//    }

//    private static async void OnDragOver(object sender, DragEventArgs e)
//    {
//        if (e.DataView.Contains(StandardDataFormats.StorageItems))
//        {
//            var deferral = e.GetDeferral();
//            var extensions = new List<string> { ".json" };
//            var isAllowed = false;
//            var items = await e.DataView.GetStorageItemsAsync();
//            foreach (var item in items)
//            {
//                if (item is StorageFile file/* && extensions.Contains(file.FileType)*/)
//                {
//                    isAllowed = true;
//                    break;
//                }
//            }

//            e.AcceptedOperation = isAllowed ? DataPackageOperation.Copy : DataPackageOperation.None;
//            deferral.Complete();
//        }

//        e.AcceptedOperation = DataPackageOperation.None;
//    }

//    private static IAsyncOperation<StorageFile> CreateStorageFile(Stream imageStream)
//    {
//        var filename = "SampleImage.jpg";
//        return StorageFile.CreateStreamedFileAsync(filename, async stream => await StreamDataRequestedAsync(stream, imageStream), null);
//    }

//    private static async Task StreamDataRequestedAsync(StreamedFileDataRequest request, Stream imageDataStream)
//    {
//        try
//        {
//            await using (var outputStream = request.AsStreamForWrite())
//            {
//                await imageDataStream.CopyToAsync(outputStream);
//                await outputStream.FlushAsync();
//            }
//            request.Dispose();
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            request.FailAndClose(StreamedFileFailureMode.Incomplete);
//        }
//    }
//}
////# endif

public static class DragDropExtensions
{
    public static void RegisterDrag(this IElement element, IMauiContext? mauiContext, Func<CancellationToken, Task<Stream>> content)
    {
        //#if WINDOWS
        ArgumentNullException.ThrowIfNull(mauiContext);
        var view = element.ToPlatform(mauiContext);
        DragDropHelper.RegisterDrag(view, content);
        //#endif

    }

    public static void UnRegisterDrag(this IElement element, IMauiContext? mauiContext)
    {
        //#if WINDOWS
        ArgumentNullException.ThrowIfNull(mauiContext);
        var view = element.ToPlatform(mauiContext);
        DragDropHelper.UnRegisterDrag(view);
        //#endif

    }

    public static void RegisterDrop(this IElement element, IMauiContext? mauiContext, Func<Stream, Task>? content)
    {
        //#if WINDOWS
        ArgumentNullException.ThrowIfNull(mauiContext);
        var view = element.ToPlatform(mauiContext);
        DragDropHelper.RegisterDrop(view, content);
        //#endif

    }

    public static void UnRegisterDrop(this IElement element, IMauiContext? mauiContext)
    {
        //#if WINDOWS
        ArgumentNullException.ThrowIfNull(mauiContext);
        var view = element.ToPlatform(mauiContext);
        DragDropHelper.UnRegisterDrop(view);
        //#endif
    }
}
