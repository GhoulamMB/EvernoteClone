using System;

using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class EditCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public NotesVM NotesVM { get; set; }

        public EditCommand(NotesVM notesVM)
        {
            NotesVM = notesVM;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            NotesVM.StartEditing();
        }
    }
}
