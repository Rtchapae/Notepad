using System;
using System.Globalization;
using Diary.Models;

namespace Diary.ViewModels
{
    public class NoteViewModel : BaseViewModel
    {
        public Note Note { get; set; }
        public NoteViewModel()
        {
            Note = new Note();
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(() => Title);
            }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(() => Message);
            }
        }



        private NoteListViewModel _notes;
        public NoteListViewModel Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(() => Notes);
                }
            }
        }
        public bool IsValid => !string.IsNullOrEmpty(Message?.Trim());
    }
}
