using IV_game.Domain.GameSession;
using IV_game.Presentation.Views;
using Microsoft.UI.Xaml;

namespace IV_game.Presentation;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ShowStartView();
    }

    public void ShowStartView()
    {
        MainContent.Content = new StartView(this);
        SubHeaderText.Text = "En simuleringsövning enligt Vårdhandboken – perifer venkateter";
    }

    public void StartGame(GameSession session)
    {
        MainContent.Content = new GameView(this, session);
    }

    public void ShowHighScores()
    {
        MainContent.Content = new HighScoresView(this);
    }

    public void ShowResult(GameSession session)
    {
        MainContent.Content = new ResultView(this, session);
    }
}
