using IV_game.Domain.GameSession;
using IV_game.Domain.ProcedureSteps;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace IV_game.Presentation.Views;

public sealed partial class GameView : UserControl
{
    private readonly MainWindow _owner;
    private GameSession _session;
    private ProcedureOutcome? _lastOutcome;
    private SelectionOutcome? _lastSelectionOutcome;
    private readonly List<string> _pickedItemIds = new();

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
        _lastOutcome = null;
        _lastSelectionOutcome = null;
        _pickedItemIds.Clear();

        ProgressText.Text = $"Steg {_session.StepNumber} av {_session.TotalSteps}";
        ScoreText.Text = $"Poäng: {_session.Score}";
        StepProgress.Value = 100.0 * (_session.StepNumber - 1) / _session.TotalSteps;
        PhaseText.Text = $"{ProcedurePhaseText.Name(step.Phase)} – {_session.Patient.PatientName}";
        TitleText.Text = step.Title;
        PromptText.Text = step.Prompt;
        FeedbackBorder.Visibility = Visibility.Collapsed;

        RenderScene(step);
    }

    private void RenderScene(ProcedureStep step)
    {
        SceneItemsPanel.Children.Clear();
        SceneItemsPanel.RowDefinitions.Clear();
        SceneItemsPanel.ColumnDefinitions.Clear();
        BasketItems.Items.Clear();
        _pickedItemIds.Clear();

        switch (step.Kind)
        {
            case StepKind.ItemSelection:
                RenderItemSelectionScene(step);
                break;
            case StepKind.OrderSelection:
                RenderOrderSelectionScene(step);
                break;
            case StepKind.SiteSelection:
                RenderSiteSelectionScene(step);
                break;
            default:
                RenderMultipleChoiceScene(step);
                break;
        }
    }

    private void RenderMultipleChoiceScene(ProcedureStep step)
    {
        SceneHeaderText.Text = "Välj rätt alternativ:";
        BasketPanel.Visibility = Visibility.Collapsed;
        ResetOrderButton.Visibility = Visibility.Collapsed;

        foreach (StepAction action in step.Actions)
        {
            Button button = new()
            {
                Content = action.Label,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Tag = action.Id
            };
            button.Click += OnActionClick;
            SceneItemsPanel.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(button, SceneItemsPanel.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(button, 4);
            SceneItemsPanel.Children.Add(button);
        }
    }

    private void RenderItemSelectionScene(ProcedureStep step)
    {
        SceneHeaderText.Text = "Klicka på det du behöver i skåpet. Klicka igen för att lägga tillbaka.";
        BasketPanel.Visibility = Visibility.Visible;
        BasketHeaderText.Text = "Din bricka:";
        ConfirmSelectionButton.IsEnabled = false;
        ResetOrderButton.Visibility = Visibility.Collapsed;

        int columns = 4;
        for (int i = 0; i < columns; i++)
        {
            SceneItemsPanel.ColumnDefinitions.Add(new ColumnDefinition());
        }

        int index = 0;
        foreach (StepAction action in step.Actions)
        {
            int row = index / columns;
            int column = index % columns;
            while (SceneItemsPanel.RowDefinitions.Count <= row)
            {
                SceneItemsPanel.RowDefinitions.Add(new RowDefinition());
            }

            Button itemButton = CreateSceneItemButton(action);
            Grid.SetRow(itemButton, row);
            Grid.SetColumn(itemButton, column);
            SceneItemsPanel.Children.Add(itemButton);
            index++;
        }
    }

    private void RenderSiteSelectionScene(ProcedureStep step)
    {
        SceneHeaderText.Text = "Klicka på det stickställe du väljer:";
        BasketPanel.Visibility = Visibility.Collapsed;
        ResetOrderButton.Visibility = Visibility.Collapsed;

        int columns = 2;
        for (int i = 0; i < columns; i++)
        {
            SceneItemsPanel.ColumnDefinitions.Add(new ColumnDefinition());
        }

        int index = 0;
        foreach (StepAction action in step.Actions)
        {
            int row = index / columns;
            int column = index % columns;
            while (SceneItemsPanel.RowDefinitions.Count <= row)
            {
                SceneItemsPanel.RowDefinitions.Add(new RowDefinition());
            }

            Button itemButton = CreateSceneItemButton(action);
            Grid.SetRow(itemButton, row);
            Grid.SetColumn(itemButton, column);
            SceneItemsPanel.Children.Add(itemButton);
            index++;
        }
    }

    private void RenderOrderSelectionScene(ProcedureStep step)
    {
        SceneHeaderText.Text = "Klicka i rätt ordning. Klicka på \"Börja om\" för att nollställa.";
        BasketPanel.Visibility = Visibility.Visible;
        BasketHeaderText.Text = "Din ordning:";
        ConfirmSelectionButton.IsEnabled = false;
        ResetOrderButton.Visibility = Visibility.Visible;

        int columns = 2;
        for (int i = 0; i < columns; i++)
        {
            SceneItemsPanel.ColumnDefinitions.Add(new ColumnDefinition());
        }

        int index = 0;
        foreach (StepAction action in step.Actions)
        {
            int row = index / columns;
            int column = index % columns;
            while (SceneItemsPanel.RowDefinitions.Count <= row)
            {
                SceneItemsPanel.RowDefinitions.Add(new RowDefinition());
            }

            Button itemButton = CreateSceneItemButton(action);
            Grid.SetRow(itemButton, row);
            Grid.SetColumn(itemButton, column);
            SceneItemsPanel.Children.Add(itemButton);
            index++;
        }
    }

    private Button CreateSceneItemButton(StepAction action)
    {
        StackPanel panel = new() { Orientation = Orientation.Horizontal, Spacing = 12 };

        if (!string.IsNullOrEmpty(action.Icon))
        {
            Image icon = new()
            {
                Source = new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{action.Icon}.png")),
                Width = 48,
                Height = 48
            };
            panel.Children.Add(icon);
        }

        TextBlock label = new()
        {
            Text = action.Label,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center
        };
        panel.Children.Add(label);

        return new Button
        {
            Content = panel,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Tag = action.Id
        };
    }

    private void OnActionClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string actionId })
        {
            return;
        }

        ProcedureStep step = _session.CurrentStep!;
        if (step.Kind is StepKind.ItemSelection or StepKind.OrderSelection)
        {
            HandleItemPick(actionId);
            return;
        }

        _lastOutcome = AppServices.Instance.SubmitAnswer.Execute(_session, actionId);
        SetActionStates(enabled: false);
        HighlightChosenAction(actionId);
        ShowFeedback();
    }

    private void HandleItemPick(string actionId)
    {
        ProcedureStep step = _session.CurrentStep!;

        if (step.Kind == StepKind.ItemSelection)
        {
            if (_pickedItemIds.Contains(actionId))
            {
                _pickedItemIds.Remove(actionId);
            }
            else
            {
                _pickedItemIds.Add(actionId);
            }
            RefreshBasket();
            ConfirmSelectionButton.IsEnabled = _pickedItemIds.Count > 0;
            SetActionStates(enabled: true);
            return;
        }

        if (!_pickedItemIds.Contains(actionId))
        {
            _pickedItemIds.Add(actionId);
            RefreshBasket();
            ConfirmSelectionButton.IsEnabled = _pickedItemIds.Count > 0;
        }
    }

    private void OnResetOrderClick(object sender, RoutedEventArgs e)
    {
        _pickedItemIds.Clear();
        RefreshBasket();
        ConfirmSelectionButton.IsEnabled = false;
    }

    private void RefreshBasket()
    {
        BasketItems.Items.Clear();
        ProcedureStep step = _session.CurrentStep!;
        int position = 1;
        foreach (string id in _pickedItemIds)
        {
            StepAction action = step.Actions.FirstOrDefault(a => a.Id == id);
            if (action is null)
            {
                continue;
            }
            StackPanel entry = new() { Orientation = Orientation.Horizontal, Spacing = 6 };
            entry.Children.Add(new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 212)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Child = new TextBlock { Text = _session.CurrentStep!.Kind == StepKind.OrderSelection ? position.ToString() : "✓" }
            });
            entry.Children.Add(new TextBlock { Text = action.Label, VerticalAlignment = VerticalAlignment.Center });
            BasketItems.Items.Add(entry);
            position++;
        }
    }

    private void OnConfirmSelectionClick(object sender, RoutedEventArgs e)
    {
        if (_pickedItemIds.Count == 0)
        {
            return;
        }

        _lastSelectionOutcome = AppServices.Instance.SubmitSelection.Execute(_session, _pickedItemIds.ToList());
        SetActionStates(enabled: false);
        MarkSceneItemsWithVerdicts(_lastSelectionOutcome);
        ShowSelectionFeedback(_lastSelectionOutcome);
    }

    private void MarkSceneItemsWithVerdicts(SelectionOutcome outcome)
    {
        foreach (UIElement child in SceneItemsPanel.Children)
        {
            if (child is not Button { Tag: string actionId } button)
            {
                continue;
            }
            SelectionItemResult? result = outcome.ItemResults
                .FirstOrDefault(r => r.Action.Id == actionId);
            if (result is null)
            {
                continue;
            }
            Color color = result.Verdict switch
            {
                SelectionItemResult.ItemVerdict.CorrectPick => Color.FromArgb(255, 37, 170, 70),
                SelectionItemResult.ItemVerdict.MissingRequired => Color.FromArgb(255, 200, 140, 30),
                SelectionItemResult.ItemVerdict.WrongExtra => Color.FromArgb(255, 200, 50, 50),
                SelectionItemResult.ItemVerdict.CriticalWrong => Color.FromArgb(255, 160, 20, 20),
                _ => Color.FromArgb(255, 128, 128, 128)
            };
            button.Background = new SolidColorBrush(color);
            if (result.Verdict != SelectionItemResult.ItemVerdict.CorrectPick)
            {
                ToolTipService.SetToolTip(button, result.Action.Feedback);
            }
        }
    }

    private void SetActionStates(bool enabled)
    {
        foreach (UIElement child in SceneItemsPanel.Children)
        {
            if (child is Button button && button.Tag is string tag && tag != "reset")
            {
                button.IsEnabled = enabled;
            }
        }
        ResetOrderButton.IsEnabled = enabled;
    }

    private void HighlightChosenAction(string actionId)
    {
        foreach (UIElement child in SceneItemsPanel.Children)
        {
            if (child is Button { Tag: string id } button && id == actionId)
            {
                button.Background = new SolidColorBrush(
                    _lastOutcome!.IsCorrect ? Color.FromArgb(255, 37, 170, 70) : Color.FromArgb(255, 200, 50, 50));
            }
        }
    }

    private void ShowSelectionFeedback(SelectionOutcome outcome)
    {
        FeedbackTitle.Text = outcome.IsCriticalFailure
            ? "Kritiskt fel – patientsäkerhetsrisk!"
            : outcome.IsCorrect ? "Rätt!" : "Det finns saker att förbättra.";
        FeedbackText.Text = outcome.BuildFeedback();
        SourceText.Text = $"Källa: {outcome.SourceReference}";
        FeedbackBorder.Background = new SolidColorBrush(
            outcome.IsCriticalFailure ? Color.FromArgb(255, 60, 20, 20) :
            outcome.IsCorrect ? Color.FromArgb(255, 25, 70, 40) :
            Color.FromArgb(255, 70, 40, 25));
        FeedbackBorder.Visibility = Visibility.Visible;
        NextButton.Content = _session.IsFinished ? "Visa resultat" : "Nästa";
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
