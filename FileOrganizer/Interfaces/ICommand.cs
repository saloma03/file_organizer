namespace FileOrganizer.Interfaces
{
    public interface ICommand
    {
        void Execute();
        void Undo();

    }
}
