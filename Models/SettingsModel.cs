using System.ComponentModel;

public class SettingsModel : INotifyPropertyChanged
{
    private string theme = "light";
    private string backgroundColor = "#FFFFFF";
    private string difficulty = "easy";
    private string mode = "training";

    public string Theme
    {
        get => theme;
        set { theme = value; OnPropertyChanged(nameof(Theme)); }
    }

    public string BackgroundColor
    {
        get => backgroundColor;
        set { backgroundColor = value; OnPropertyChanged(nameof(BackgroundColor)); }
    }

    public string Difficulty
    {
        get => difficulty;
        set { difficulty = value; OnPropertyChanged(nameof(Difficulty)); }
    }

    public string Mode
    {
        get => mode;
        set { mode = value; OnPropertyChanged(nameof(Mode)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 