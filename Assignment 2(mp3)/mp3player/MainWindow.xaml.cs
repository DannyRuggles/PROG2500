using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace mp3player
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isMetadataVisible = true;
        public bool IsMetadataVisible
        {
            get => _isMetadataVisible;
            set
            {
                _isMetadataVisible = value;
                NotifyPropertyChanged(nameof(IsMetadataVisible));
            }
        }

        public ICommand ToggleMetadataVisibilityCommand { get; private set; }
        

        private void ToggleMetadataVisibility()
        {
            IsMetadataVisible = !IsMetadataVisible;
        }

        public ICommand SaveMetadataCommand { get; private set; }

        private string songTitle;
        public string SongTitle
        {
            get => songTitle;
            set { songTitle = value; NotifyPropertyChanged(nameof(SongTitle)); }
        }

        private string songArtist;
        public string SongArtist
        {
            get => songArtist;
            set { songArtist = value; NotifyPropertyChanged(nameof(SongArtist)); }
        }

        private string songAlbum;
        public string SongAlbum
        {
            get => songAlbum;
            set { songAlbum = value; NotifyPropertyChanged(nameof(SongAlbum)); }
        }

        private string songYear;
        public string SongYear
        {
            get => songYear;
            set { songYear = value; NotifyPropertyChanged(nameof(SongYear)); }
        }
        private MediaController mediaController = new MediaController();
        private string currentFilePath;

        // properties
        private bool isNowPlayingScreenVisible;
        public bool IsNowPlayingScreenVisible
        {
            get { return isNowPlayingScreenVisible; }
            set { isNowPlayingScreenVisible = value; NotifyPropertyChanged(nameof(IsNowPlayingScreenVisible)); }
        }

        private bool isTagEditorScreenVisible;
        public bool IsTagEditorScreenVisible
        {
            get { return isTagEditorScreenVisible; }
            set { isTagEditorScreenVisible = value; NotifyPropertyChanged(nameof(IsTagEditorScreenVisible)); }
        }

        // commands
        public ICommand OpenCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        public ICommand HideCommand { get; private set; }


        // constructor
        public MainViewModel()
        {
            OpenCommand = new RelayCommand(OpenFile);
            PlayCommand = new RelayCommand(Play, CanExecutePlay);
            PauseCommand = new RelayCommand(Pause, CanExecutePause);
            StopCommand = new RelayCommand(Stop, CanExecuteStop);
            SaveMetadataCommand = new RelayCommand(SaveMetadata);
            ToggleMetadataVisibilityCommand = new RelayCommand(_ => ToggleMetadataVisibility()); // Corrected placement


        }

        // methods for commands
        private void SaveMetadata(object parameter)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                MessageBox.Show("No file selected.");
                return;
            }

            // Stop playback and close the file
            mediaController.Stop(); // Ensure playback is stopped
            mediaController.Close(); // Close the file so it's no longer in use

            try
            {
                // Assuming SongYear is a string, convert it to uint. Adjust error handling as needed.
                uint year = uint.TryParse(SongYear, out uint parsedYear) ? parsedYear : 0;

                TagEditor.WriteTags(currentFilePath, SongTitle, SongArtist, SongAlbum, year);
                MessageBox.Show("Metadata saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save metadata: {ex.Message}");
            }
            finally
            {
                // re-open the file for playback
                mediaController.Open(currentFilePath);
            }
        }

        private void OpenFile(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"; // Filter to only show MP3 files or all files
            if (openFileDialog.ShowDialog() == true)
            {
                currentFilePath = openFileDialog.FileName;
                mediaController.Open(currentFilePath);

                // read and display song metadata
                TagEditor.ReadTags(currentFilePath, out string title, out string artist, out string album, out uint year);
                SongTitle = $"{title}";
                SongArtist = $"{artist}";
                SongAlbum = $"{album}";
                SongYear = $"{year}";

                MessageBox.Show("File opened: " + currentFilePath);
            }
        }

        private bool CanExecutePlay(object parameter) => !string.IsNullOrEmpty(currentFilePath);
        private bool CanExecutePause(object parameter) => !string.IsNullOrEmpty(currentFilePath);
        private bool CanExecuteStop(object parameter) => !string.IsNullOrEmpty(currentFilePath);

        private void Play(object parameter)
        {
            mediaController.Play();
            MessageBox.Show("Playing");
        }

        private void Pause(object parameter)
        {
            mediaController.Pause();
            MessageBox.Show("Paused");
        }

        private void Stop(object parameter)
        {
            mediaController.Stop();
            MessageBox.Show("Stopped");
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

}
