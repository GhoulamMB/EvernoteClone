using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class ExitCommand : ICommand
    {
        public NotesVM NotesVM { get; set; }
        public ExitCommand(NotesVM notesVM)
        {
            NotesVM = notesVM;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
