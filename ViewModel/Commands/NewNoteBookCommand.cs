using System;
using System.Windows.Input;
#pragma warning disable CS8618
namespace EvernoteClone.ViewModel.Commands
{
    public class NewNoteBookCommand : ICommand
    {
        public NotesVM NotesVM { get; set; }

        public NewNoteBookCommand(NotesVM notesVM)
        {
            NotesVM = notesVM;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;}
            remove { CommandManager.RequerySuggested -= value;}
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            NotesVM.CreateNoteBook();
        }
    }
}
