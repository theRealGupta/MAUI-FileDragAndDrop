namespace FileDragAndDrop;

//using CommunityToolkit.Maui.Storage;
using System.IO;
using System.Threading;
using Foundation;
using UIKit;


public static class DragDropHelper
{
    public static void RegisterDrag(UIView view, Func<CancellationToken, Task<Stream>> content)
    {
        var dragInteraction = new UIDragInteraction(new DragInteractionDelegate()
        {
            Content = content
        });
        view.AddInteraction(dragInteraction);
    }

    public static void UnRegisterDrag(UIView view)
    {
        var dragInteractions = view.Interactions.OfType<UIDropInteraction>();
        foreach (var interaction in dragInteractions)
        {
            view.RemoveInteraction(interaction);
        }
    }

    public static void RegisterDrop(UIView view, Func<Stream, Task>? content)
    {
        var dropInteraction = new UIDropInteraction(new DropInteractionDelegate()
        {
            Content = content
        });
        view.AddInteraction(dropInteraction);
    }

    public static void UnRegisterDrop(UIView view)
    {
        var dropInteractions = view.Interactions.OfType<UIDropInteraction>();
        foreach (var interaction in dropInteractions)
        {
            view.RemoveInteraction(interaction);
        }
    }
}

class DragInteractionDelegate : UIDragInteractionDelegate
{
    public Func<CancellationToken, Task<Stream>>? Content { get; init; }

    public override UIDragItem[] GetItemsForBeginningSession(UIDragInteraction interaction, IUIDragSession session)
    {
        if (Content is null)
        {
            return Array.Empty<UIDragItem>();
        }

        var streamContent = Content.Invoke(CancellationToken.None).GetAwaiter().GetResult();
        var itemProvider = new NSItemProvider(NSData.FromStream(streamContent), UniformTypeIdentifiers.UTTypes.Image.Identifier);
        var dragItem = new UIDragItem(itemProvider);
        return new[] { dragItem };
    }
}


class DropInteractionDelegate : UIDropInteractionDelegate
{
    public Func<Stream, Task>? Content { get; init; }
    //IFileSaver fileSaver;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public override UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
    {
        return new UIDropProposal(UIDropOperation.Copy);
    }

    public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
    {
        Console.WriteLine("Perform Drop Action from Mac");
        if (Content is null)
        {
            return;
        }

            foreach (var item in session.Items)
            {
                try
                {

                    item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Image.Identifier, null, async (data, error) =>
                    {
                        if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
                        {
                            Console.WriteLine(nsData.Path);
                            //var sourcePath = nsData.Path;
                            //var bytes = await File.ReadAllBytesAsync(nsData.Path);
                            //var strem = new MemoryStream(bytes);
                            try
                            {
                                var destinationPath = "/Users/anupg/Pictures/Photo Scanner/" + Path.GetFileName(nsData.Path);
                                Console.WriteLine(destinationPath);
                                File.Copy(nsData.Path, destinationPath, true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                            nsData.Dispose();
                        }
                        
                    }  );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

       

    }

    //public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
    //{
    //    Console.WriteLine("Perform Drop Action from Mac");

    //    //var test = new UIKit.UIDragItem();

    //    //var test = new List<UIDragItem>();
    //    ////var itemprovder = UIDrag
    //    //foreach (var item in session.Items)
    //    //{
    //    //    var itemprovider = new NSItemProvider((NSObject) item.ItemProvider, UniformTypeIdentifiers.UTTypes.Image.Identifier);
    //    //    test.Add(new UIDragItem(itemprovider));
    //    //}

    //    if (Content is null)
    //    {
    //        return;
    //    }

    //    //Device.BeginInvokeOnMainThread(() => {
    //        MainThread.BeginInvokeOnMainThread( () => {

    //        foreach (var item in test)
    //                           {

    //                               Console.WriteLine(item.ItemProvider.SuggestedName);

    //                try
    //                {

    //                    item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Image.Identifier, null, async (data, error) =>
    //                    {
    //                        if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
    //                        {
    //                            Console.WriteLine(nsData.Path);
    //                            var sourcePath = nsData.Path;
    //                            var bytes = await File.ReadAllBytesAsync(nsData.Path);
    //                            var strem = new MemoryStream(bytes);
    //                            try
    //                            {

    //                                var destinationPath = "/Users/anupg/Pictures/Photo Scanner/" + item.ItemProvider.SuggestedName;
    //                                Console.WriteLine(destinationPath);

    //                                //var path = await fileSaver.SaveAsync(destinationPath, strem, cancellationTokenSource.Token);
    //                                File.Copy(nsData.Path, destinationPath, true);

    //                            }
    //                            catch (Exception e)
    //                            {
    //                                Console.WriteLine(e);
    //                            }

    //                        }

    //                    }



    //                                                      );
    //                }
    //                catch (Exception e)
    //                {
    //                    Console.WriteLine(e);
    //                }




    //                           }

    //                       }
    //                       );






    //}
}