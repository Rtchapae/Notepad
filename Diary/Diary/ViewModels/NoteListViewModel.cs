using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Diary.Service;
using Xamarin.Forms;
using Diary.Views;

namespace Diary.ViewModels
{
   public class NoteListViewModel : BaseViewModel
    {
        public ObservableCollection<NoteViewModel> AllNotes { get; set; }
        public ICommand CreateNoteCommand { protected set; get; }
        public ICommand DeleteNoteCommand { protected set; get; }
        public ICommand SaveNoteCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        public INavigation Navigation { get; set; }
        private readonly IUIService _uiService;
        public NoteListViewModel(IUIService uiService)
        {
            _uiService = uiService;
            AllNotes = new ObservableCollection<NoteViewModel>();
            CreateNoteCommand = new Command(CreateNote);
            DeleteNoteCommand = new Command(DeleteNote);
            SaveNoteCommand = new Command(SaveNote);
            BackCommand = new Command(Back);
        }


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
                    _selectedNote = null;
                    OnPropertyChanged(() => SelectedNote);
                    Navigation.PushAsync(new NotePage(temp));
                }
            }
        }

   

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
                    if (AllNotes.Any(_ => _.Id == note.Id))
                    {
                        var old = AllNotes.First(_ => _.Id == note.Id);
                        int id = AllNotes.IndexOf(old);
                        AllNotes[id] = note;
                    }
                    else
                    {
                        note.Id = AllNotes.Count + 1;
                        AllNotes.Add(note);
                    }
                }
                else                
                    throw new Exception();
                bool isLeftNote = true;
                NotesRight = new ObservableCollection<NoteViewModel>();
                NotesLeft = new ObservableCollection<NoteViewModel>();
                foreach (var allNote in AllNotes)
                { 
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
            }
            Back();
        }

        private void CreateNote()
        {
            Navigation.PushAsync(new NotePage(new NoteViewModel() { Notes = this }));
        }     
    }
}
