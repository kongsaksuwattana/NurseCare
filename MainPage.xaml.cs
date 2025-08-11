namespace NurseCare
{
    public partial class MainPage : ContentPage
    {
       // int count = 0;

        public MainPage()
        {
            InitializeComponent();
            this.SizeChanged += RegisterPage_SizeChanged;
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
        public void OnRegisterNurseClicked(object sender, EventArgs e)
        {
            // Navigate to the RegisterNurse page
            Navigation.PushAsync(new NurseRegister());
        }
        public void OnRegisterPatientClicked(object sender, EventArgs e)
        {
            // Navigate to the RegisterPatient page
            Navigation.PushAsync(new PatientPage());
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

}
