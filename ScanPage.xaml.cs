using BarcodeScanning;
//using Java.Sql;
//using System;
//using Xamarin.Google.MLKit.Vision.Barcode.Common;

namespace NurseCare
{
    public partial class ScanPage : ContentPage
    {
        private readonly BarcodeDrawable _drawable = new();
        private readonly List<string> qualitys = new();
        private TaskCompletionSource<string> _barcodeResult = new();
        public Task<string> GetBarcodeAsync() => _barcodeResult.Task;
        event EventHandler<OnDetectionFinishedEventArg> DetectionFinished
        {
            add => Barcode.OnDetectionFinished += value;
            remove => Barcode.OnDetectionFinished -= value;
        }
        public ScanPage()
        {
            InitializeComponent();

            BackButton.Text = "<";

            qualitys.Add("Low");
            qualitys.Add("Medium");
            qualitys.Add("High");
            qualitys.Add("Highest");

            Quality.ItemsSource = qualitys;
            if (DeviceInfo.Platform != DevicePlatform.MacCatalyst)
                Quality.Title = "Quality";
        }

        protected override async void OnAppearing()
        {
            await Methods.AskForRequiredPermissionAsync();
            base.OnAppearing();

            Barcode.CameraEnabled = true;
            Graphics.Drawable = _drawable;            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //Barcode.CameraEnabled = false;
        }

        private void ContentPage_Unloaded(object sender, EventArgs e)
        {
            //Barcode.Handler?.DisconnectHandler();
        }

        private async void CameraView_OnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
        {
            _drawable.barcodeResults = e.BarcodeResults;
            Graphics.Invalidate();
            var barcode = e.BarcodeResults.FirstOrDefault();
            if (!string.IsNullOrEmpty(barcode?.RawValue))
            {
                DetectionFinished -= CameraView_OnDetectionFinished; // Unsubscribe to avoid multiple calls
                _barcodeResult.TrySetResult(barcode.RawValue);
                //await Navigation.PopAsync();
              
               await Shell.Current.GoToAsync("..");
            }

        }
        
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void CameraButton_Clicked(object sender, EventArgs e)
        {
            if (Barcode.CameraFacing == CameraFacing.Back)
                Barcode.CameraFacing = CameraFacing.Front;
            else
                Barcode.CameraFacing = CameraFacing.Back;
        }

        private void TorchButton_Clicked(object sender, EventArgs e)
        {
            if (Barcode.TorchOn)
                Barcode.TorchOn = false;
            else
                Barcode.TorchOn = true;
        }

        private void VibrateButton_Clicked(object sender, EventArgs e)
        {
            if (Barcode.VibrationOnDetected)
                Barcode.VibrationOnDetected = false;
            else
                Barcode.VibrationOnDetected = true;
        }

        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            if (Barcode.PauseScanning)
                Barcode.PauseScanning = false;
            else
                Barcode.PauseScanning = true;
        }

        private void Quality_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            if(picker.SelectedIndex > -1 && picker.SelectedIndex < 5)
                Barcode.CaptureQuality = (CaptureQuality)picker.SelectedIndex;
        }

        private class BarcodeDrawable : IDrawable
        {
            public IReadOnlySet<BarcodeResult>? barcodeResults;
            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                if (barcodeResults is not null && barcodeResults.Count > 0)
                {
                    canvas.StrokeSize = 2;
                    canvas.StrokeColor = Colors.Red;
                    var scale = 1 / canvas.DisplayScale;
                    canvas.Scale(scale, scale);
                    string resultText = string.Empty;
                    var barcode = barcodeResults.FirstOrDefault();
                    canvas.DrawRectangle(barcode.PreviewBoundingBox);
                    //foreach (var barcode in barcodeResults)
                    //{
                    //    canvas.DrawRectangle(barcode.PreviewBoundingBox);
                        
                        
                    //    //canvas.DrawString(barcode.RawValue, barcode.PreviewBoundingBox.Left, barcode.PreviewBoundingBox.Top - 20, HorizontalAlignment.Left, VerticalAlignment.Center, 0.5f);
                    //    // canvas.DrawString(barcode.RawValue, barcode.PreviewBoundingBox.Left + 20, barcode.PreviewBoundingBox.Top + 20, HorizontalAlignment.Left);
                    //}
                }
            }
        }
    }
}