﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Diary.IService;
using Diary.Models;
using Xamarin.Forms;
using Diary.Views;
using Java.Sql;

namespace Diary.ViewModels
{
   public class NoteListViewModel : BaseViewModel
    {
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
            var task = new Task(async () => await GetNotesFromDb());
            task.Start();
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

        public async Task GetNotesFromDb()
        {
           var notes = await App.Database.GetItemsAsync();
            foreach (var note in notes)
            {
                AllNotes.Add((NoteViewModel)ConvertToNote(note).note);
            }
        }

        private async void SaveNote(object noteObject)
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
                        await App.Database.SaveItemAsync((Note)ConvertToNote(note).note);
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

        private async void DeleteNote(object noteObject)
        {
            var note = noteObject as NoteViewModel;
            if (note != null)
            {
                AllNotes.Remove(note);
              await  App.Database.DeleteItemAsync((Note)ConvertToNote(note).note).ConfigureAwait(false); ;
            }
            Back();
        }

        private void CreateNote()
        {
            Navigation.PushAsync(new NotePage(new NoteViewModel() { Notes = this }));
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
               var noteModel = (Note) noteObject;
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
    }

}
