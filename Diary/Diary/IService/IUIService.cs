using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diary.Service
{
   public interface IUIService
   {
        void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel");
        void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel");
        Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel");
        void Alert(string message, Action done = null, string title = "", string okButton = "OK");
        Task AlertAsync(string message, string title = "", string okButton = "OK");
        void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
        void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
        Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
        void CloseApp();
        void ShowToast(string msg, int sec = 6);
    }

    public class DialogFormResponse
    {
        public bool Ok { get; set; }
        public List<KeyValuePair<string, string>> Values { get; set; }
    }


    public class InputResponse
    {
        public bool Ok { get; set; }
        public string Text { get; set; }
    }
}
