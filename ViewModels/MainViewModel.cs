using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TimeTrainer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ClockModel Clock { get; set; } = new();
        public SettingsModel Settings { get; set; }
            = SettingsService.Load();
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

        private bool showAnswer;
        public bool ShowAnswer
        {
            get => showAnswer;
            set { showAnswer = value; OnPropertyChanged(nameof(ShowAnswer)); OnPropertyChanged(nameof(DisplayedTime)); }
        }

        public string ClockTimeString => $"{Clock.Hour:D2}:{Clock.Minute:D2}";
        public string DisplayedTime => ShowAnswer ? ClockTimeString : "??:??";

        public ICommand CheckCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand ShowHelpCommand { get; }

        public MainViewModel()
        {
            CheckCommand = new RelayCommand(_ => CheckTime());
            OpenSettingsCommand = new RelayCommand(_ => OpenSettings());
            ShowHelpCommand = new RelayCommand(_ => ShowHelp());
            Clock.SetRandomTime();
            ShowAnswer = Settings.Mode == "training";
            OnPropertyChanged(nameof(DisplayedTime));
        }

        private void CheckTime()
        {
            if (int.TryParse(UserHour, out int h) && int.TryParse(UserMinute, out int m))
            {
                bool correct = Clock.IsTimeEqual(h, m);
                // Здесь можно добавить обработку результата (увеличить счет, уменьшить жизни и т.д.)
            }
            ShowAnswer = true;
            OnPropertyChanged(nameof(ClockTimeString));
        }

        private void OpenSettings()
        {
            Settings.Mode = Settings.Mode == "training" ? "quiz" : "training";
            SettingsService.Save(Settings);
            ShowAnswer = Settings.Mode == "training";
        }
        private void ShowHelp()
        {
            System.Windows.MessageBox.Show(
                "F1 - помощь\nEnter - проверить время",
                "Помощь");
        }

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