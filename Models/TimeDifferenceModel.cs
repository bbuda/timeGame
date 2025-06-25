using System.ComponentModel;

public class TimeDifferenceModel : INotifyPropertyChanged
{
    private int startHour, startMinute, endHour, endMinute;

    public int StartHour { get => startHour; set { startHour = value; OnPropertyChanged(nameof(StartHour)); } }
    public int StartMinute { get => startMinute; set { startMinute = value; OnPropertyChanged(nameof(StartMinute)); } }
    public int EndHour { get => endHour; set { endHour = value; OnPropertyChanged(nameof(EndHour)); } }
    public int EndMinute { get => endMinute; set { endMinute = value; OnPropertyChanged(nameof(EndMinute)); } }

    public int GetDifferenceInMinutes()
    {
        int start = StartHour * 60 + StartMinute;
        int end = EndHour * 60 + EndMinute;
        return (end - start + 1440) % 1440; // на случай перехода через полночь
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 