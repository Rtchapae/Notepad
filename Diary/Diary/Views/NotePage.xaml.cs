using Diary.ViewModels;
using System.Threading.Tasks;
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
        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform.Equals(Device.UWP))
            {
                OnClosePageRequested();
                return true;
            }
            else
            {
                OnClosePageRequested();
                return true;
            }
        }

        protected override void OnDisappearing()
        {
          //  OnClosePageRequested();
            base.OnDisappearing();
        }

        protected async void OnClosePageRequested()
        {
            var tdvm = (NoteViewModel)BindingContext;
            if (tdvm.Message != tdvm.OldMessage || tdvm.Title != tdvm.OldTitle) //проверка на сохранение
            {
                var result = await DisplayAlert("Подождите", "Вы внеесли изменения, желаете ли сохранить?", "Сохранить изменения", "Отмена");
                if (result)
                {
                    tdvm.Notes.SaveData.Invoke(tdvm);
                }
                else
                {
                    tdvm.Title = tdvm.OldTitle;
                    tdvm.Message = tdvm.OldMessage;
                }


            }
            await Navigation.PopAsync(true);
        }
    }
}
