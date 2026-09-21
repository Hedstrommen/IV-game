using IV_game.Application.Abstractions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace IV_game.Presentation.Views;

public sealed partial class HighScoresView : UserControl
{
    private readonly MainWindow _owner;

    public HighScoresView(MainWindow owner)
    {
        _owner = owner;
        InitializeComponent();

        LoadScores();
    }

    private void LoadScores()
    {
        ScoreList.Items.Clear();

        try
        {
            IReadOnlyList<ScoreRecord> scores = AppServices.Instance.FinishGame.GetHighScores(10);

            if (scores.Count == 0)
            {
                ScoreList.Items.Add(new ListViewItem
                {
                    Content = new TextBlock
                    {
                        Text = "Inga spelade omgångar ännu. Spela ett fall för att hamna på listan!",
                        TextWrapping = TextWrapping.Wrap
                    },
                    IsEnabled = false
                });
                return;
            }

            int rank = 1;
            foreach (ScoreRecord record in scores)
            {
                ScoreList.Items.Add(new ListViewItem
                {
                    Content = new TextBlock
                    {
                        Text = $"#{rank}  {record.PlayerName} – {record.Score} poäng ({record.CorrectCount}/{record.TotalSteps} rätt) – {record.CompletedAt:yyyy-MM-dd HH:mm}",
                        TextWrapping = TextWrapping.Wrap
                    }
                });
                rank++;
            }
        }
        catch (Exception ex)
        {
            ScoreList.Items.Add(new ListViewItem
            {
                Content = new TextBlock { Text = $"Kunde inte läsa topplistan: {ex.Message}" },
                IsEnabled = false
            });
        }
    }

    private void OnBackClick(object sender, RoutedEventArgs e)
    {
        _owner.ShowStartView();
    }
}
