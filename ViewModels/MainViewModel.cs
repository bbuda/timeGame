using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TimeTrainer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ClockModel Clock { get; set; } = new();
        public SettingsModel Settings { get; set; } = new();
        public GameSessionModel Session { get; set; } = new();

        private string userHour;
        private string userMinute;
        public string UserHour
        {
            get => userHour;
            set { userHour = value; OnPropertyChanged(nameof(UserHour)); }
        }
        public string UserMinute
        {
            get => userMinute;
            set { userMinute = value; OnPropertyChanged(nameof(UserMinute)); }
        }

        public string ClockTimeString => $"{Clock.Hour:D2}:{Clock.Minute:D2}";

        public ICommand CheckCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand ShowHelpCommand { get; }

        public MainViewModel()
        {
            CheckCommand = new RelayCommand(_ => CheckTime());
            OpenSettingsCommand = new RelayCommand(_ => OpenSettings());
            ShowHelpCommand = new RelayCommand(_ => ShowHelp());
            Clock.SetRandomTime();
        }

        private void CheckTime()
        {
            if (int.TryParse(UserHour, out int h) && int.TryParse(UserMinute, out int m))
            {
                bool correct = Clock.IsTimeEqual(h, m);
                // Здесь можно добавить обработку результата (увеличить счет, уменьшить жизни и т.д.)
            }
            OnPropertyChanged(nameof(ClockTimeString));
        }

        private void OpenSettings() { /* Открыть окно настроек */ }
        private void ShowHelp() { /* Открыть окно помощи */ }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // RelayCommand — простая реализация ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => canExecute == null || canExecute(parameter);
        public void Execute(object parameter) => execute(parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
} 