using Diary.Service;
using Diary.ViewModels;
using Xamarin.Forms;

namespace Diary.Views
{
    public partial class NotesListPage : ContentPage
    { 
        public NotesListPage()
        {
            InitializeComponent();
            BindingContext = new NoteListViewModel((IUIService)App.Container.Resolve(typeof(IUIService))) { Navigation = this.Navigation };
        }
    }
}
