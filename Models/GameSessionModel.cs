using System.Collections.ObjectModel;
using System.ComponentModel;

public class GameSessionModel : INotifyPropertyChanged
{
    private int score;
    private int lives;
    private ObservableCollection<(int hour, int minute, bool isCorrect)> results = new();

    public int Score
    {
        get => score;
        set { score = value; OnPropertyChanged(nameof(Score)); }
    }

    public int Lives
    {
        get => lives;
        set { lives = value; OnPropertyChanged(nameof(Lives)); }
    }

    public ObservableCollection<(int hour, int minute, bool isCorrect)> Results
    {
        get => results;
        set { results = value; OnPropertyChanged(nameof(Results)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 