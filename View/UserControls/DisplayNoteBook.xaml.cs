

using EvernoteClone.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EvernoteClone.View.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayNoteBook.xaml
    /// </summary>
    public partial class DisplayNoteBook
    {

        public NoteBook Notebook
        {
            get { return (NoteBook)GetValue(NotebookProperty); }
            set { SetValue(NotebookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotebookProperty =
            DependencyProperty.Register("Notebook", typeof(NoteBook), typeof(DisplayNoteBook), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNoteBook? notebookUserControl = d as DisplayNoteBook;

            if (notebookUserControl != null)
            {
                notebookUserControl.DataContext = notebookUserControl.Notebook;
            }
        }

        public DisplayNoteBook()
        {
            InitializeComponent();
        }
    }
}
