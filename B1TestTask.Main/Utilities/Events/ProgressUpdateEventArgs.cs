namespace B1TestTask.Main.Utilities.Events
{
    public class ProgressUpdatedEventArgs : EventArgs
    {
        public double ProgressValue { get; }

        public ProgressUpdatedEventArgs(double progressValue)
        {
            ProgressValue = progressValue;
        }
    }
}
