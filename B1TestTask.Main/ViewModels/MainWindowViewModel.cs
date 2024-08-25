using B1TestTask.Domain.Entities;
using B1TestTask.Main.Infastructure.Command;
using B1TestTask.Main.Services;
using B1TestTask.Main.Utilities.Events;
using B1TestTask.Main.Utilities.Extensions;
using B1TestTask.Main.ViewModels.Base;
using B1TestTask.Main.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace B1TestTask.Main.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        #region AppTitleProperty

        private string _title = "TestTask";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region CurrentDirectoryPathProperty

        private string _currentDirectoryPath = "Выберете папку";

        public string CurrentDirectoryPath
        {
            get => _currentDirectoryPath;
            set 
            {
                Set(ref _currentDirectoryPath, value);
                LoadFilesFromDirectoryCommand.Execute(value);
            }
        }

        #endregion

        #region DirectoryfilesProperty

        private ObservableCollection<string> _files = new();

        public ObservableCollection<string> Files
        {
            get => _files;
            set => Set(ref _files, value);
        }

        #endregion

        #region ProgressValueProperty

        private double _progressValue = 0;

        public double ProgressValue 
        {
            get => _progressValue;
            set => Set(ref _progressValue, value);
        }

        #endregion

        #region StatusTextProperty

        private string _statusText = "";

        public string StatusText 
        {
            get => _statusText;
            set => Set(ref _statusText, value);
        }

        #endregion

        #region LoadedLines

        private int _loadedLines = 0;

        public int LoadedLines
        {
            get => _loadedLines;
            set => Set(ref _loadedLines, value);
        }

        #endregion

        #region CounterVisabilityProperty

        private bool _isCounterVisible = false;

        public bool IsCounterVisible 
        {
            get => _isCounterVisible;
            set => Set(ref _isCounterVisible, value);
        }

        #endregion

        #region CanCommandsRunProperty

        private bool _canCommandsRun = true;

        public bool CanCommandsRun 
        {
            get => _canCommandsRun;
            set => Set(ref _canCommandsRun, value);
        }

        #endregion

        #region SavedExelFilesProperty

        private ObservableCollection<ExelReport> _savedExelFiles;

        public ObservableCollection<ExelReport> SavedExelFiles 
        {
            get => _savedExelFiles;
            set => Set(ref _savedExelFiles, value);
        }

        #endregion

        #region SelectedExelFileProperty

        private ExelReport _selectedExelFile;

        public ExelReport SelectedExelFile
        {
            get => _selectedExelFile;
            set => Set(ref _selectedExelFile, value);
        }

        #endregion

        #region CurrentFileData

        private ExelData _currentExelData;

        public ExelData CurrentExelData 
        {
            get => _currentExelData;
            set => Set(ref _currentExelData, value);
        }

        #endregion

        #endregion

        #region Commands

        #region AppCloseCommand

        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object? parametr)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object? parametr) => CanCommandsRun;

        #endregion

        #region SelectFolderCommand

        public ICommand SelectFolderCommand { get; }

        private void OnSelectFolderCommandExecuted(object? parameter)
        {
            using var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK && Directory.Exists(dialog.SelectedPath))
            {
                CurrentDirectoryPath = dialog.SelectedPath;
            }
        }

        private bool CanSelectFolderCommandExecute(object? parameter) => true;

        #endregion

        #region LoadFilesfromDirectoryCommand

        public ICommand LoadFilesFromDirectoryCommand { get; }

        private void OnLoadFilesfromDirectoryCommandExecuted(object? parameter) 
        {
            CanCommandsRun = false;
            var index = _currentDirectoryPath.Length + 1;

            var entries = Directory.GetFileSystemEntries(CurrentDirectoryPath)
                .Select(x => x[index..])
                .Where(x => x.EndsWith(".txt") || Path.GetExtension(x) == "");

            Files = new ObservableCollection<string>(entries);
            CanCommandsRun = true;
        }

        private bool CanLoadFilesfromDirectoryCommandExecute(object? parameter) 
        {
            return Directory.Exists(CurrentDirectoryPath) && CanCommandsRun;
        }

        #endregion

        #region GenerateFilesCommand

        public ICommand GenerateFilesCommand { get; }

        private async void OnGenerateFilesCommandExecuted(object? parameter) 
        {
            CanCommandsRun = false;
            StatusText = "Генерация файлов...";
            var sw = Stopwatch.StartNew();
            try 
            {
                CurrentDirectoryPath = await _fileService.GenerateFilesAsync(CurrentDirectoryPath);
            }
            finally 
            { 
                sw.Stop();
                var seconds = sw.Elapsed.TotalSeconds;
                ProgressValue = 0;
                StatusText = $"Генерация завершилась за {seconds} cек";
                await Task.Delay(3000);
                StatusText = string.Empty;
            }
            CanCommandsRun = true;
        }

        private bool CanGenerateFilesCommandExecute(object? parameter) 
        {
            return Directory.Exists(CurrentDirectoryPath) && CanCommandsRun;
        }

        #endregion

        #region LoadExelFileCommand

        public ICommand LoadExelFile { get; }

        private async void OnLoadExelFileCommandExecuted(object? parameter)
        {
            CanCommandsRun = false;

            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".xls",
                Filter = "Exel documents (.xls)|*.xls"
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = fileDialog.FileName;

                var data = await _exelService.SaveFileAsync(filePath);

                if (data != null) 
                {
                    
                }
            }

            UpdateData();
            CanCommandsRun = true;
        }

        private bool CanLoadExelFileCommandExecute(object? parameter)
        {
            return CanCommandsRun;
        }

        #endregion

        #region SaveFilecommand

        public ICommand SaveFileCommand { get; }

        private async void OnSaveFileCommandExecuted(object? parameter)
        {
            CanCommandsRun = false;
            var mergedFiles = Files.Where(x => x.Contains("merged"));

            StatusText = "Строк загружено = ";

            if (mergedFiles.Any()) 
            {
                var fileName = mergedFiles.First();

                await _fileService.SaveFile(Path.Combine(CurrentDirectoryPath, fileName));
            }

            StatusText = "Файл успешно загружен";
            await Task.Delay(3000);
            StatusText = "";
            LoadedLines = 0;
            CanCommandsRun = true;
        }

        private bool CanSaveFileCommandExecute(object? parameter)
        {
            var mergedFiles = Files.Where(x => x.Contains("merged"));

            return mergedFiles.Any() && CanCommandsRun;
        }
        #endregion

        #region MergeFilesCommand

        public ICommand MergeFilesCommand { get; }

        private async void OnMergeFilesCommandexecuted(object? parameter)
        {
            CanCommandsRun = false;
            var inputWindow = new InputWindow("Перед объединением, вы можете удалить строки содержащие заданное сочетание символов. Например \"abc\"");
            inputWindow.ShowDialog();

            if (inputWindow.OkButtonClicked)
            {
                StatusText = "Объединение файлов...";
                var sw = Stopwatch.StartNew();
                try
                {
                    var predicate = inputWindow.Result.GetPredicate();
                    var validLines = await _fileService.MergeFiles(CurrentDirectoryPath, predicate);

                    var messageBoxText = $"При объединении файлов было удалено {10_000_000 - validLines} строк с подстрокой '{inputWindow.Result}'";
                    var caption = "Information";
                    var button = MessageBoxButton.OK;
                    var icon = MessageBoxImage.Information;
                    System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
                finally
                {
                    sw.Stop();
                    var seconds = sw.Elapsed.TotalSeconds;
                    ProgressValue = 0;
                    StatusText = $"Файлы объединены за {seconds} cек";
                    //await Task.Delay(3000);
                    //StatusText = string.Empty;
                }
                
            }
            UpdateData();
            CanCommandsRun = true;
        }

        private bool CanMergeFilesCommandExecute(object? parameter)
        {
            return Directory.Exists(CurrentDirectoryPath)
                && Files
                .Where(
                    x => x.EndsWith(".txt")
                    && int.TryParse(x[..^4], out _))
                .Count() == 100
                && CanCommandsRun;
        }

        #endregion

        #endregion

        #region Services

        private IFilesService _fileService { get; }

        private IExelService _exelService { get; }

        #endregion

        public MainWindowViewModel() 
            : base() 
        {
            #region Commands

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SelectFolderCommand = new LambdaCommand(OnSelectFolderCommandExecuted, CanSelectFolderCommandExecute);
            LoadFilesFromDirectoryCommand = new LambdaCommand(OnLoadFilesfromDirectoryCommandExecuted, CanLoadFilesfromDirectoryCommandExecute);
            GenerateFilesCommand = new LambdaCommand(OnGenerateFilesCommandExecuted, CanGenerateFilesCommandExecute);
            MergeFilesCommand = new LambdaCommand(OnMergeFilesCommandexecuted, CanMergeFilesCommandExecute);
            SaveFileCommand = new LambdaCommand(OnSaveFileCommandExecuted, CanSaveFileCommandExecute);
            LoadExelFile = new LambdaCommand(OnLoadExelFileCommandExecuted, CanLoadExelFileCommandExecute);

            #endregion

            #region Services

            _fileService = App.Services.GetRequiredService<IFilesService>();
            _exelService = App.Services.GetRequiredService<IExelService>();
            _fileService.ProgressUpdated += FileService_ProgressUpdated;
            _fileService.LoadedLinesUpdated += FileService_LoadedLinesUpdated;

            #endregion

            UpdateData();
        }

        private void FileService_ProgressUpdated(object sender, ProgressUpdatedEventArgs e)
        {
            ProgressValue = e.ProgressValue;
        }

        private void FileService_LoadedLinesUpdated(object sender, LoadedLinesEventArgs e)
        {
            LoadedLines = e.LoadedLines;
        }

        private async void UpdateData() 
        {
            #region files
            if (Directory.Exists(CurrentDirectoryPath))
            {
                LoadFilesFromDirectoryCommand.Execute(this);
            }
            #endregion

            SavedExelFiles = new ObservableCollection<ExelReport>(await _exelService.GetReportsAsync());

            if (SelectedExelFile != null && SelectedExelFile.Id != 0)
            {
                CurrentExelData = await _exelService.DownloadFileAsync(SelectedExelFile.FilePath);
            }

        }

        protected override async void MethodsOnPropertyChanged(string? propertyName)
        {
            switch (propertyName) 
            {
                case "SelectedExelFile":
                    CurrentExelData = await _exelService.DownloadFileAsync(SelectedExelFile.FilePath);
                    break;
            }
        }
    }
}
