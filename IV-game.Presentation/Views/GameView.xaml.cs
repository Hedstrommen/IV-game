using IV_game.Domain.GameSession;
using IV_game.Domain.ProcedureSteps;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace IV_game.Presentation.Views;

public sealed partial class GameView : UserControl
{
    private readonly MainWindow _owner;
    private GameSession _session;
    private ProcedureOutcome? _lastOutcome;

    public GameView(MainWindow owner, GameSession session)
    {
        _owner = owner;
        _session = session;

        InitializeComponent();

        RenderStep();
    }

    private void RenderStep()
    {
        if (_session.IsFinished)
        {
            ShowResult();
            return;
        }

        ProcedureStep step = _session.CurrentStep!;

        ProgressText.Text = $"Steg {_session.StepNumber} av {_session.TotalSteps}";
        ScoreText.Text = $"Poäng: {_session.Score}";
        StepProgress.Value = 100.0 * (_session.StepNumber - 1) / _session.TotalSteps;

        PhaseText.Text = $"{ProcedurePhaseText.Name(step.Phase)} – {_session.Patient.PatientName}";
        TitleText.Text = step.Title;
        PromptText.Text = step.Prompt;

        ActionsPanel.Children.Clear();
        FeedbackBorder.Visibility = Visibility.Collapsed;

        foreach (StepAction action in step.Actions)
        {
            Button button = new()
            {
                Content = action.Label,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Tag = action.Id
            };
            button.Click += OnActionClick;
            ActionsPanel.Children.Add(button);
        }
    }

    private void OnActionClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string actionId })
        {
            return;
        }

        _lastOutcome = AppServices.Instance.SubmitAnswer.Execute(_session, actionId);

        SetActionStates(enabled: false);
        HighlightChosenAction(actionId);
        ShowFeedback();
    }

    private void SetActionStates(bool enabled)
    {
        foreach (UIElement child in ActionsPanel.Children)
        {
            if (child is Button button)
            {
                button.IsEnabled = enabled;
            }
        }
    }

    private void HighlightChosenAction(string actionId)
    {
        foreach (UIElement child in ActionsPanel.Children)
        {
            if (child is Button { Tag: string id } button && id == actionId)
            {
                button.Background = new SolidColorBrush(
                    _lastOutcome!.IsCorrect ? Color.FromArgb(255, 37, 170, 70) : Color.FromArgb(255, 200, 50, 50));
            }
        }
    }

    private void ShowFeedback()
    {
        ProcedureOutcome outcome = _lastOutcome!;

        FeedbackTitle.Text = outcome.IsCriticalFailure
            ? "Kritiskt fel – patientsäkerhetsrisk!"
            : outcome.IsCorrect ? "Rätt!" : "Fel.";

        FeedbackText.Text = outcome.Feedback;
        SourceText.Text = $"Källa: {outcome.SourceReference}";

        FeedbackBorder.Background = new SolidColorBrush(
            outcome.IsCriticalFailure ? Color.FromArgb(255, 60, 20, 20) :
            outcome.IsCorrect ? Color.FromArgb(255, 25, 70, 40) :
            Color.FromArgb(255, 70, 40, 25));

        FeedbackBorder.Visibility = Visibility.Visible;

        NextButton.Content = _session.IsFinished ? "Visa resultat" : "Nästa";
    }

    private void OnNextClick(object sender, RoutedEventArgs e)
    {
        if (_session.IsFinished)
        {
            SaveScore();
            ShowResult();
        }
        else
        {
            RenderStep();
        }
    }

    private void SaveScore()
    {
        try
        {
            AppServices.Instance.FinishGame.SaveScore(_session, Environment.UserName);
        }
        catch
        {
        }
    }

    private void ShowResult()
    {
        _owner.ShowResult(_session);
    }
}
