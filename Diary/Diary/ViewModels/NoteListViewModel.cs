using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Diary.IService;
using Diary.Models;
using Xamarin.Forms;
using Diary.Views;

namespace Diary.ViewModels
{
   public class NoteListViewModel : BaseViewModel
    {
        public ICommand CreateNoteCommand { protected set; get; }
        public ICommand DeleteNoteCommand { protected set; get; }
        public ICommand SaveNoteCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public delegate void Save(object obj);
        public Save SaveData;
        public INavigation Navigation { get; set; }
        private readonly IUIService _uiService;
        public NoteListViewModel(IUIService uiService)
        {
            _uiService = uiService;    
            GetNotesFromDb();
            CreateNoteCommand = new Command(CreateNote);
            DeleteNoteCommand = new Command(DeleteNote);
            SaveNoteCommand = new Command(SaveNote);
            BackCommand = new Command(Back);
            SaveData = SaveNote;
        }

        #region Property

        private ObservableCollection<NoteViewModel> _notesLeft;
        public ObservableCollection<NoteViewModel> NotesLeft
        {
            get { return _notesLeft; }
            set
            {
                _notesLeft = value;
                OnPropertyChanged(() => NotesLeft);
            }
        }
        private ObservableCollection<NoteViewModel> _notesAll;
        public ObservableCollection<NoteViewModel> AllNotes
        {
            get { return _notesAll; }
            set
            {
                _notesAll = value;
                OnPropertyChanged(() => AllNotes);
            }
        }
        private ObservableCollection<NoteViewModel> _notesRight;
        public ObservableCollection<NoteViewModel> NotesRight
        {
            get { return _notesRight; }
            set
            {
                _notesRight = value;
                OnPropertyChanged(() => NotesRight);
            }
        }


        private NoteViewModel _selectedNote;

        public NoteViewModel SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                if (SelectedNote != value)
                {
                    NoteViewModel temp = value;
                    temp.Notes = this;
                    _selectedNote = null;
                    OnPropertyChanged(() => SelectedNote);
                    Navigation.PushAsync(new NotePage(temp));
                }
            }
        }

        #endregion


        #region CommandMethods
        private void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveNote(object noteObject)
        {
            try
            {
                if (noteObject is NoteViewModel note && note.IsValid)
                {
                    note.Title = string.IsNullOrEmpty(note.Title)
                        ? DateTime.Today.ToShortDateString()
                        : note.Title;
                    note.OldMessage = note.Message;
                    note.OldTitle = note.Title;
                    if (AllNotes.Any(_ => _.Id == note.Id))
                    {
                        var old = AllNotes.First(_ => _.Id == note.Id);
                        int id = AllNotes.IndexOf(old);
                        AllNotes[id] = note;
                        App.Database.UpdateItem((Note) ConvertToNote(note).note);
                    }
                    else
                    {
                        note.Id = AllNotes.Count + 1;
                        AllNotes.Add(note);
                        App.Database.SaveItem((Note) ConvertToNote(note).note);
                    }
                }
                else
                    throw new Exception();

                ShowNotes(AllNotes);
                Back();
            }
            catch (Exception e)
            {
                _uiService.Alert("Введите текст");
            }
        }

        private void DeleteNote(object noteObject)
            {
                var note = noteObject as NoteViewModel;
                if (note != null)
                {
                    AllNotes.Remove(note);
                    App.Database.DeleteItem((Note)ConvertToNote(note).note);
                    ShowNotes(AllNotes);
                }
                Back();
            }

            private void CreateNote()
            {
                Navigation.PushAsync(new NotePage(new NoteViewModel() { Notes = this }));
            }

        #endregion


        public void GetNotesFromDb()
        {
            AllNotes = new ObservableCollection<NoteViewModel>();
            var notes =  App.Database.GetItems();
            foreach (var note in notes)
            {
                var item = (NoteViewModel)ConvertToNote(note).note;
                item.OldMessage = note.Message;
                item.OldTitle = note.Title;    
                AllNotes.Add(item);
            }
            ShowNotes(AllNotes);
        }

      

        private void ShowNotes(ObservableCollection<NoteViewModel> allNotes)
        {
            bool isLeftNote = true;
            NotesRight = new ObservableCollection<NoteViewModel>();
            NotesLeft = new ObservableCollection<NoteViewModel>();
            foreach (var allNote in allNotes)
            {
                allNote.MessagePreview = GetPreviewText(allNote.Message);
                if (isLeftNote)
                {
                    NotesLeft.Add(allNote);
                    isLeftNote = false;
                }
                else
                {
                    NotesRight.Add(allNote);
                    isLeftNote = true;
                }
            }
        }
        public (Type type, object note) ConvertToNote(object noteObject)
        {
            if (noteObject.GetType() == typeof(NoteViewModel))
            {
                var noteModel = (NoteViewModel)noteObject;
                var noteNew = new Note()
                {
                    Id = noteModel.Id,
                    Message = noteModel.Message,
                    Title = noteModel.Title
                };
                var result = (type: noteNew.GetType(), note: noteNew);
                return result;
            }
            else
            {
                var noteModel = (Note)noteObject;
                var noteNew = new NoteViewModel()
                {
                    Id = noteModel.Id,
                    Message = noteModel.Message,
                    Title = noteModel.Title
                };
                var result = (type: noteNew.GetType(), note: noteNew);
                return result;
            }

        }

        private string GetPreviewText(string message)
        {
            if (message.Length > 150)
                return $"{message.Substring(0, 150)}...";
            else
                return message;
        }

        public void SearchNotes(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var notes = AllNotes.Where(u => u.Message.Contains(text) || u.Title.Contains(text)).ToList();
                AllNotes = new ObservableCollection<NoteViewModel>(notes);
                ShowNotes(AllNotes);
            }
            else
            {
                GetNotesFromDb();
            }
        }


    }

}
