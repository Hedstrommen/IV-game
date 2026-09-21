using IV_game.Domain.GameSession;
using IV_game.Domain.PatientCases;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace IV_game.Presentation.Views;

public sealed partial class StartView : UserControl
{
    private readonly MainWindow _owner;

    public StartView(MainWindow owner)
    {
        _owner = owner;
        InitializeComponent();

        LoadCases();
    }

    private void LoadCases()
    {
        try
        {
            IReadOnlyList<PatientCase> cases = AppServices.Instance.StartGame.GetAvailableCases();

            CaseList.Items.Clear();

            foreach (PatientCase patientCase in cases)
            {
                CaseList.Items.Add(new ListViewItem
                {
                    Content = BuildCaseItem(patientCase),
                    Tag = patientCase.Id
                });
            }
        }
        catch (Exception ex)
        {
            ShowError("Kunde inte läsa in patientfall.", ex);
        }
    }

    private static StackPanel BuildCaseItem(PatientCase patientCase)
    {
        return new StackPanel
        {
            Spacing = 4,
            Children =
            {
                new TextBlock
                {
                    Text = patientCase.PatientName,
                    FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                    FontSize = 16
                },
                new TextBlock
                {
                    Text = patientCase.Presentation,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["TextFillColorSecondaryBrush"]
                }
            }
        };
    }

    private void OnStartClick(object sender, RoutedEventArgs e)
    {
        if (CaseList.SelectedItem is not ListViewItem { Tag: string caseId })
        {
            return;
        }

        try
        {
            GameSession session = AppServices.Instance.StartGame.Start(caseId);
            _owner.StartGame(session);
        }
        catch (Exception ex)
        {
            ShowError("Kunde inte starta spelet.", ex);
        }
    }

    private void OnHighScoresClick(object sender, RoutedEventArgs e)
    {
        _owner.ShowHighScores();
    }

    private async void ShowError(string message, Exception ex)
    {
        ContentDialog dialog = new()
        {
            Title = "Fel",
            Content = $"{message}\n\n{ex.Message}",
            CloseButtonText = "OK",
            XamlRoot = XamlRoot
        };

        await dialog.ShowAsync();
    }
}
