using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Diary.IService;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(Diary.Droid.Service.UIService))]
namespace Diary.Droid.Service
{
   public  class UIService: IUIService
    {
        Activity CurrentActivity;
        public UIService(Activity activity) : base()
        {
            this.CurrentActivity = activity;
        }

        public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Confirm(message, confirmed => {
                if (confirmed)
                    okClicked();
            },
            title, okButton, cancelButton);
        }

        public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Application.SynchronizationContext.Post(ignored => {
                if (CurrentActivity == null) return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(okButton, delegate {
                            if (answer != null)
                                answer(true);
                        })
                        .SetNegativeButton(cancelButton, delegate {
                            if (answer != null)
                                answer(false);
                        })
                        .Show();
            }, null);
        }

        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            var tcs = new TaskCompletionSource<bool>();
            Confirm(message, tcs.SetResult, title, okButton, cancelButton);
            return tcs.Task;
        }

        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {
            Application.SynchronizationContext.Post(ignored => {
                if (CurrentActivity == null) return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(okButton, delegate {
                            if (done != null)
                                done();
                        })
                        .Show();
            }, null);
        }

        public Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            var tcs = new TaskCompletionSource<object>();
            Alert(message, () => tcs.SetResult(null), title, okButton);
            return tcs.Task;
        }

        public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            Input(message, (ok, text) => {
                if (ok)
                    okClicked(text);
            },
                placeholder, title, okButton, cancelButton, initialText);
        }

        public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            Application.SynchronizationContext.Post(ignored => {
                if (CurrentActivity == null) return;
                var input = new EditText(CurrentActivity) { Hint = hint, Text = initialText };

                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetView(input)
                        .SetPositiveButton(okButton, delegate {
                            if (answer != null)
                                answer(true, input.Text);
                        })
                        .SetNegativeButton(cancelButton, delegate {
                            if (answer != null)
                                answer(false, input.Text);
                        })
                        .Show();
            }, null);
        }

        public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            var tcs = new TaskCompletionSource<InputResponse>();
            Input(message, (ok, text) => tcs.SetResult(new InputResponse() { Ok = ok, Text = text }), placeholder, title, okButton, cancelButton, initialText);
            return tcs.Task;
        }     

        //public Task<DialogFormResponse> ShowDialogForm(DialogForm dialogForm, string message, string title = "", string okButton = "OK",
        //    string cancelButton = "Cancel")
        //{
        //    var tcs = new TaskCompletionSource<DialogFormResponse>();
        //    LinearLayout layout = new LinearLayout(CurrentActivity) { Orientation = Orientation.Vertical };
        //    List<KeyValuePair<string, Func<string>>> keyValue = new List<KeyValuePair<string, Func<string>>>();
        //    foreach (var input in dialogForm.Inputs)
        //    {
        //        EditText inputElement = new EditText(CurrentActivity) { Text = input.Value, Hint = input.Hint };
        //        keyValue.Add(new KeyValuePair<string, Func<string>>(input.Hint, () => inputElement.Text));
        //        layout.AddView(inputElement);
        //    }
        //    AlertDialog.Builder builder = new AlertDialog.Builder(CurrentActivity);
        //    builder.SetNegativeButton(cancelButton, (senderAlert, args) => { });
        //    builder.SetPositiveButton(okButton, (senderAlert, args) =>
        //    {
        //        var dialogResponse = new DialogFormResponse
        //        {
        //            Ok = true,
        //            Values = keyValue.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Invoke())).ToList()
        //        };
        //        tcs.SetResult(dialogResponse);
        //    });
        //    AlertDialog alertDialog = builder.Create();
        //    alertDialog.SetTitle(title);
        //    alertDialog.SetView(layout);
        //    alertDialog.Show();
        //    return tcs.Task;
        //}

        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        public void ShowToast(string msg, int sec = 6)
        {
            Toast.MakeText(CurrentActivity, msg, ToastLength.Short).Show();
        }

    }
}
