namespace FileDragAndDrop;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();

        //#if WINDOWS
        Loaded += (sender, args) =>
        {
            DropZone.RegisterDrop(Handler?.MauiContext, async stream =>
            {
                //await bindingContext.FileDroppedAsync(stream);
            });
        };

        Unloaded += (sender, args) =>
        {
            DropZone.UnRegisterDrop(Handler?.MauiContext);
        };
        //#endif
    }

    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //    count++;

    //    if (count == 1)
    //        CounterBtn.Text = $"Clicked {count} time";
    //    else
    //        CounterBtn.Text = $"Clicked {count} times";

    //    SemanticScreenReader.Announce(CounterBtn.Text);
    //}
}


