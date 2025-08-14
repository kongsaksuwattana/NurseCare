using NurseCare.DataAccess;
using NurseCare.Model;
namespace NurseCare;

public partial class SearchHNView : ContentView
{
    //CameraBarcodeReaderView barcodeReader;
    NurseCareDataQuery dataQuery = new NurseCareDataQuery();
    public Patient? searchPatient { get; set; } = null;
    public event EventHandler<Patient?>? PatientSelected;
    public bool isFromCamera { get; set; } = false;
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
        isFromCamera = true; // Set the flag to indicate that the search is from camera input
    }

    private async void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Patient? patient = await dataQuery.GetPatientByHN(SearchEntry.Text);
        if (patient != null)
        {
            searchPatient = patient;
            PatientSelected?.Invoke(this, searchPatient);
            // Display patient details or update UI as needed
          
        }
        else
        {
            searchPatient = null;            
        }

    }
}