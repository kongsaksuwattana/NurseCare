using Microsoft.Maui.Controls;
using System;
using System.Linq;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using ZXing.PDF417.Internal;
namespace NurseCare;

public partial class BarcodeScannerXZing : ContentPage
{
    private TaskCompletionSource<string> _barcodeResult = new();
    public Task<string> GetBarcodeAsync() => _barcodeResult.Task;
    public BarcodeScannerXZing()
	{
		InitializeComponent();
        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");

        var first = e.Results?.FirstOrDefault();
        if (first is not null)
        {
            Dispatcher.Dispatch(() =>
            {
                // Update BarcodeGeneratorView
                barcodeGenerator.ClearValue(BarcodeGeneratorView.ValueProperty);
                barcodeGenerator.Format = first.Format;
                barcodeGenerator.Value = first.Value;

                // Update Label
                _barcodeResult.TrySetResult(first.Value);
                Navigation.PopAsync();
                //ResultLabel.Text = $"Barcodes: {first.Format} -> {first.Value}";
            });
        }
    }
    void TorchButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.IsTorchOn = !barcodeView.IsTorchOn;
    }
}