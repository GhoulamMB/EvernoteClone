using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class EndEditCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public NotesVM NotesVM { get; set; }

        public EndEditCommand(NotesVM notesVM)
        {
            NotesVM = notesVM;
        }


        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            NoteBook? noteBook = parameter as NoteBook;
            if (noteBook != null) { 
            NotesVM.StopEditing(noteBook!);
            }
        }
    }
}
