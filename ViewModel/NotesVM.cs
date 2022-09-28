using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
#pragma warning disable CS8618

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {

        #region Inotify_Implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        #region Events

        public event EventHandler SelectedNoteChanged;


        #endregion


        #region Properties
        public ObservableCollection<NoteBook> NoteBooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

        private NoteBook _selectedNoteBook;

        public NoteBook SelectedNoteBook
        {
            get { return _selectedNoteBook; }
            set
            {
                _selectedNoteBook = value;
                OnPropertyChanged(nameof(SelectedNoteBook));
                GetNotes();
            }
        }

        private Visibility _isVisible;

        public Visibility IsVisibile
        {
            get { return _isVisible; }
            set 
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisibile));
            }
        }

        private Note _selectedNote;

        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            { 
                _selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        #region Commands
        public NewNoteBookCommand NewNoteBook { get; set; }
        public NewNoteCommand NewNote { get; set; }
        public ExitCommand ExitCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditCommand EndEditCommand { get; set; }
        #endregion

        #region Constructor
        public NotesVM()
        {
            NewNoteBook = new(this);
            NewNote = new(this);
            ExitCommand = new(this);
            EditCommand = new(this);
            EndEditCommand = new(this);
            NoteBooks = new();
            Notes = new();
            IsVisibile = Visibility.Collapsed;
            //GetNoteBooks();
        }
        #endregion

        #region Methods
        public async void CreateNoteBook()
        {
            NoteBook newNoteBook = new()
            {
                Name = "New NoteBook",
                UserId = App.UserID
            };
            await DBHelper.Insert(newNoteBook);
            await GetNoteBooks();
        }

        public async Task CreateNote(string noteBookID)
        {
            Note newNote = new()
            {
                NoteBookId = noteBookID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Note For : {DateTime.Now}"
            };
            await DBHelper.Insert(newNote);
            await GetNotes();
        }

        public async Task GetNoteBooks()
        {
            var newNoteBooks = (await DBHelper.Read<NoteBook>()).Where(n=>n.UserId == App.UserID).ToList();
            NoteBooks.Clear();
            foreach (var item in newNoteBooks)
            {
                NoteBooks.Add(item);
            }
        }

        private async Task GetNotes()
        {
            if (SelectedNoteBook is not null)
            {
                List<Note> newNotes = (await DBHelper.Read<Note>()).Where(n => n.NoteBookId == SelectedNoteBook.Id).ToList();
                Notes.Clear();
                foreach (var item in newNotes)
                {
                    Notes.Add(item);
                }
            }
        }

        public void StartEditing()
        {
            IsVisibile = Visibility.Visible;
        }

        public async void StopEditing(NoteBook noteBook)
        {
            IsVisibile = Visibility.Collapsed;
            await DBHelper.Update(noteBook);
            await GetNoteBooks();
        }
        #endregion

    }
}
