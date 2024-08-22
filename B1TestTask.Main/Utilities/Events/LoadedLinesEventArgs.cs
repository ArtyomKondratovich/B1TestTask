namespace B1TestTask.Main.Utilities.Events
{
    internal class LoadedLinesEventArgs : EventArgs
    {
        public int LoadedLines { get; }

        public LoadedLinesEventArgs(int loadedFiles)
        {
            LoadedLines = loadedFiles;
        }
    }
}
