using IV_game.Application.UseCases;
using IV_game.Domain.GameSession;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IV_game.Presentation.Views;

public sealed partial class ResultView : UserControl
{
    private readonly MainWindow _owner;
    private readonly GameSession _session;

    public ResultView(MainWindow owner, GameSession session)
    {
        _owner = owner;
        _session = session;
        InitializeComponent();
        GameResult result = AppServices.Instance.FinishGame.BuildResult(session);
        ResultHeading.Text = $"Klart! Du genomförde fallet {result.PatientName}.";
        ResultGrade.Text = result.Grade;
        ScoreValue.Text = result.Score.ToString();
        CorrectValue.Text = $"{result.CorrectCount} / {result.TotalSteps}";
        CriticalValue.Text = result.CriticalFailures.ToString();
        PercentValue.Text = $"{result.PercentCorrect} %";
        ResultProgress.Value = result.PercentCorrect;
        BuildReview(session);
    }

    private void BuildReview(GameSession session)
    {
        IReadOnlyList<StepReview> reviews = AppServices.Instance.FinishGame.GetStepReviews(session);
        List<StepReview> toImprove = reviews.Where(r => !r.WasCorrect || r.WasCriticalFailure).ToList();

        ReviewHeaderText.Text = toImprove.Count == 0
            ? "Perfekt omgång! Alla moment utfördes korrekt enligt Vårdhandboken."
            : "Det här behöver du förbättra:";

        foreach (StepReview review in reviews.Where(r => !r.WasCorrect || r.WasCriticalFailure))
        {
            Border card = new()
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12),
                Background = new SolidColorBrush(
                    review.WasCriticalFailure ? Color.FromArgb(255, 60, 20, 20) : Color.FromArgb(255, 50, 40, 20)),
                BorderBrush = new SolidColorBrush(
                    review.WasCriticalFailure ? Color.FromArgb(255, 160, 20, 20) : Color.FromArgb(255, 200, 140, 30)),
                BorderThickness = new Thickness(1)
            };

            StackPanel content = new() { Spacing = 4 };
            TextBlock title = new()
            {
                Text = review.WasCriticalFailure
                    ? $"⚠ Kritiskt fel – {review.Step.Title}"
                    : $"Förbättra – {review.Step.Title}",
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            };
            content.Children.Add(title);

            if (!string.IsNullOrWhiteSpace(review.ChosenSummary))
            {
                content.Children.Add(new TextBlock
                {
                    Text = $"Du valde: {review.ChosenSummary}",
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 180, 180, 180))
                });
            }

            content.Children.Add(new TextBlock
            {
                Text = review.Feedback,
                TextWrapping = TextWrapping.Wrap
            });
            content.Children.Add(new TextBlock
            {
                Text = $"Källa: {review.SourceReference}",
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 180, 180, 180))
            });

            card.Child = content;
            ReviewList.Children.Add(card);
        }

        if (toImprove.Count == 0)
        {
            foreach (StepReview review in reviews)
            {
                ReviewList.Children.Add(new TextBlock
                {
                    Text = $"✓ {review.Step.Title}",
                    TextWrapping = TextWrapping.Wrap
                });
            }
        }
    }

    private void OnRestartClick(object sender, RoutedEventArgs e)
    {
        _owner.ShowStartView();
    }

    private void OnHighScoresClick(object sender, RoutedEventArgs e)
    {
        _owner.ShowHighScores();
    }
}
