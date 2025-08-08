namespace NurseCare;

public partial class PatientPage : ContentPage
{
    TurnBedriddenView TurnBedriddenView = new TurnBedriddenView();
	public PatientPage()
	{
		InitializeComponent();
		this.SizeChanged += PatientPage_SizeChanged;
        TurnBedridden.Children.Add(TurnBedriddenView);
    }
	private void PatientPage_SizeChanged(object sender, EventArgs e)
	{
        if (Width > Height)
        {
            // Landscape: Patient left, Nurse right
            VisualStateManager.GoToState(MainGrid, "Landscape");
            MainGrid.SetRow(MainGrid.Children[0], 0);
            MainGrid.SetColumn(MainGrid.Children[0], 0);
            MainGrid.SetRow(MainGrid.Children[1], 0);
            MainGrid.SetColumn(MainGrid.Children[1], 1);
        }
        else
        {
            // Portrait: Patient top, Nurse bottom
            VisualStateManager.GoToState(MainGrid, "Portrait");
            MainGrid.SetRow(MainGrid.Children[0], 0);
            MainGrid.SetColumn(MainGrid.Children[0], 0);
            MainGrid.SetRow(MainGrid.Children[1], 1);
            MainGrid.SetColumn(MainGrid.Children[1], 0);
        }
    }

    public void OnRegisterPatientClicked(object sender, EventArgs e)
	{
	}
}