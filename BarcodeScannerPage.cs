using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace NurseCare;

public class BarcodeScannerPage : ContentPage
{
    public string BarcodeValue { get; private set; }
    private TaskCompletionSource<string> _barcodeResult = new();
    public Task<string> GetBarcodeAsync() => _barcodeResult.Task;


    public BarcodeScannerPage()
	{
        
        var barcodeReader = new CameraBarcodeReaderView
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            IsDetecting = true // Start scanning immediately
        };
        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All, // Set to all formats or specify the ones you need
            AutoRotate = true, // Automatically rotate the camera view
            TryHarder = true // Enable more aggressive scanning
        };
        barcodeReader.BarcodesDetected += (s, e) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var result = e.Results.FirstOrDefault();
                if (result != null)
                {
                    //DisplayAlert("Barcode Detected", result.Value, "OK");
                    BarcodeValue = result.Value; // Return the detected barcode value
                    barcodeReader.IsDetecting = false; // Stop scanning after detection
                                                       // 
                    _barcodeResult.TrySetResult(BarcodeValue); // Set the result for the task
                    await Navigation.PopAsync(); // Navigate back to the previous page
                }
            });
        };

        Content = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star }
            },
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star }
            },
            Children =
            {
                {barcodeReader}
            }
        };
        
    }
}