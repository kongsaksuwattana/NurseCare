namespace NurseCare;

public partial class TurnBedriddenView : ContentView
{
    SearchHNView SearchHNView = new SearchHNView();
    public TurnBedriddenView()
	{
		InitializeComponent();
        SearchViewGrid.Children.Add(SearchHNView);
    }

    private void SaveTurnButton_Clicked(object sender, EventArgs e)
    {

    }

    private void ChangeBedButton_Clicked(object sender, EventArgs e)
    {

    }
}