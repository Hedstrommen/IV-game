using IV_game.Application.UseCases;
using IV_game.Domain.GameSession;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
