using System;
using System.ComponentModel;

public class ClockModel : INotifyPropertyChanged
{
    private int hour;
    private int minute;

    public int Hour
    {
        get => hour;
        set { hour = value; OnPropertyChanged(nameof(Hour)); }
    }

    public int Minute
    {
        get => minute;
        set { minute = value; OnPropertyChanged(nameof(Minute)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void SetRandomTime(Random rng = null)
    {
        rng ??= new Random();
        Hour = rng.Next(0, 12);
        Minute = rng.Next(0, 60);
    }

    public bool IsTimeEqual(int hour, int minute)
    {
        return Hour == hour && Minute == minute;
    }

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 