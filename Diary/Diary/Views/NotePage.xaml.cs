using Diary.ViewModels;
using Xamarin.Forms;

namespace Diary.Views
{
    public partial class NotePage : ContentPage
    {
        public NoteViewModel ViewModel { get; private set; }
        public NotePage(NoteViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            this.BindingContext = ViewModel;
        }
    }
}
