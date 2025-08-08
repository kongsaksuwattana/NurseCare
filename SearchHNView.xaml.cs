using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
namespace NurseCare;

public partial class SearchHNView : ContentView
{
    CameraBarcodeReaderView barcodeReader;

    public SearchHNView()
	{
		InitializeComponent();

	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        //BarcodeScannerPage barcodeScannerPage = new BarcodeScannerPage();

        //BarcodeScannerXZing barcodeScannerPage = new BarcodeScannerXZing();
        ScanPage barcodeScannerPage = new ScanPage();
        await Navigation.PushAsync(barcodeScannerPage);
        //await Navigation.PushAsync(barcodeScannerPage);
        SearchEntry.Text = await barcodeScannerPage.GetBarcodeAsync(); // Set the Entry text to the scanned barcode value



        //barcodeReader = new CameraBarcodeReaderView
        //{
        //    HorizontalOptions = LayoutOptions.Fill,
        //    VerticalOptions = LayoutOptions.Fill,
        //    IsDetecting = true, // Start scanning immediately
        //    IsVisible = true
        //};

        //barcodeReader.BarcodesDetected += (s, e) =>
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        var result = e.Results.FirstOrDefault();
        //        if (result != null)
        //        {
        //            //DisplayAlert("Barcode Detected", result.Value, "OK");
        //            SearchEntry.Text = result.Value; // Set the detected barcode value to the Entry
        //            barcodeReader.IsDetecting = false; // Stop scanning after detection
        //            barcodeReader.IsVisible = false;

        //        }
        //    });
        //};
        //CameraView.Children.Add(barcodeReader);
    }
}