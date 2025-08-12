using System.Windows;
using FileOrganizer.Interfaces;

using System.Diagnostics;

namespace FileOrganizer.Core
{
    public class UndoManager
    {
        public Stack<ICommand> history = new Stack<ICommand>();

        public void Execute(ICommand command)
        {
            command.Execute();
            history.Push(command);
            MessageBox.Show($"[UndoManager] Command added. Stack size: {history.Count}");
            Trace.WriteLine($"[UndoManager] Command added. Stack size: {history.Count}"); // For more visibility
        }

        public void Undo()
        {
            MessageBox.Show($"[UndoManager] Undo called. Stack size: {history.Count}");

            if (history.Count > 0)
            {
                ICommand command = history.Pop();
                MessageBox.Show($"[UndoManager] Executing undo for {command.GetType().Name}");
                command.Undo();
                MessageBox.Show($"[UndoManager] Undo completed. Stack size: {history.Count}");
            }
            else
            {
                MessageBox.Show("[UndoManager] No actions to undo - stack is empty");
                MessageBox.Show("No actions to undo.");
            }
        }
    }
}