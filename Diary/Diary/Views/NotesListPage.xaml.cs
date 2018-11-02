using Diary.IService;
using Diary.ViewModels;
using Xamarin.Forms;

namespace Diary.Views
{
    public partial class NotesListPage : ContentPage
    { 
        public NotesListPage(NoteListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            //  BindingContext = new NoteListViewModel((IUIService)App.Container.Resolve(typeof(IUIService))) { Navigation = this.Navigation };
        }

        public NoteListViewModel ViewModel
        {
            get => BindingContext as NoteListViewModel;
            set
            {
               value.Navigation = this.Navigation;
                BindingContext = value;
            } 
        }
        private void SearchNotes(string text)
        {
           ViewModel.SearchNotes(text);
        }
    }
}
