using B1TestTask.Main.Infastructure.Command;
using B1TestTask.Main.Services;
using B1TestTask.Main.Utilities.Extensions;
using B1TestTask.Main.ViewModels.Base;
using B1TestTask.Main.Views.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace B1TestTask.Main.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        #region AppTitleProperty

        private string _title = "B1";

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

        private ObservableCollection<string> _files;

        public ObservableCollection<string> Files
        {
            get => _files;
            set => Set(ref _files, value);
        }

        #endregion

        #region ProgressValueProperty

        private int _progressValue = 0;

        public int ProgressValue 
        {
            get => _progressValue;
            set => Set(ref _progressValue, value);
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

        private bool CanCloseApplicationCommandExecute(object? parametr) => true;

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
            var index = _currentDirectoryPath.Length + 1;

            var entries = Directory.GetFileSystemEntries(CurrentDirectoryPath)
                .Select(x => x[index..])
                .Where(x => x.EndsWith(".txt") || Path.GetExtension(x) == "");

            Files = new ObservableCollection<string>(entries);
        }

        private bool CanLoadFilesfromDirectoryCommandExecute(object? parameter) 
        {
            return Directory.Exists(CurrentDirectoryPath);
        }

        #endregion

        #region GenerateFilesCommand

        public ICommand GenerateFilesCommand { get; }

        private async void OnGenerateFilesCommandExecuted(object? parameter) 
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                CurrentDirectoryPath = await FileService.GenerateFilesAsync(CurrentDirectoryPath);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedTime = stopwatch.Elapsed;
                // Вывести время выполнения в лог или показать пользователю
                Console.WriteLine($"Время выполнения GenerateFilesAsync(): {elapsedTime.TotalMilliseconds} ms");
            }
        }

        private bool CanGenerateFilesCommandExecute(object? parameter) 
        {
            return Directory.Exists(CurrentDirectoryPath);
        }
        #endregion

        #region MergeFilesCommand

        public ICommand MergeFilesCommand { get; }

        private async void OnMergeFilesCommandexecuted(object? parameter)
        {
            var inputWindow = new InputWindow("Перед объединением, вы можете удалить строки содержащие заданное сочетание символов. Например \"abc\"");
            inputWindow.ShowDialog();

            var predicate = inputWindow.Result.GetPredicate();

            var countLines = await FileService.MergeFiles(CurrentDirectoryPath, predicate);

            Console.WriteLine($"строк загружено в файл: {countLines}");
        }

        private bool CanMergeFilesCommandExecute(object? parameter)
        {
            return Directory.Exists(CurrentDirectoryPath) && Directory.GetFiles(CurrentDirectoryPath).Where(x => x.EndsWith(".txt")).Count() == 100;
        }

        #endregion

        #endregion

        #region Services

        public IFilesService FileService { get; }

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
            #endregion

            #region Services

            FileService = new FilesService();

            #endregion
        }
    }
}
