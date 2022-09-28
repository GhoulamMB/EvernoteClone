using EvernoteClone.Model;
using System;
using System.Windows.Input;
#pragma warning disable CS8618
namespace EvernoteClone.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM NotesVM { get; set; }

        public NewNoteCommand(NotesVM notesVM)
        {
            NotesVM = notesVM;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            NoteBook? selectedNoteBook = parameter as NoteBook;
            if (selectedNoteBook != null)
            {
                return true;
            }
            return false;
        }

        public void Execute(object? parameter)
        {
            NoteBook? selectedNoteBook = parameter as NoteBook;
            NotesVM.CreateNote(selectedNoteBook!.Id);
        }
    }
}
