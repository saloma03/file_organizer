using System.Collections.ObjectModel;

namespace FileOrganizer.Logging
{
    internal class ActionLogger
    {
        public static ObservableCollection<string> Logs { get; private set; } = new ObservableCollection<string>();

        public void Log(string message)
        {
            Logs.Add(message);
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void ClearLogs()
        {
            Logs.Clear();
        }


    }
}
