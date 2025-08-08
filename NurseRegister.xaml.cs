using System;
using Microsoft.Maui.Controls;
namespace NurseCare;

public partial class NurseRegister : ContentPage
{
    UpdateTeamView UpdateTeamView { get; set; } = new UpdateTeamView();
    
    public NurseRegister()
    {
        InitializeComponent();
        this.SizeChanged += RegisterPage_SizeChanged;
        ChangeTeamView.Children.Add(UpdateTeamView);
    }

    private void RegisterPage_SizeChanged(object sender, EventArgs e)
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

    private void OnRegisterPatientClicked(object sender, EventArgs e)
    {
        // TODO: Implement patient registration logic
    }

    private void OnRegisterNurseClicked(object sender, EventArgs e)
    {
        // TODO: Implement nurse registration logic
    }
}