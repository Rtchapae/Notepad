using System;
using Xamarin.Forms;

namespace Diary.Controls
{
    //TODO
   public class NoteListView: ListView
    {

        public NoteListView()
        {
            ItemTemplate = new DataTemplate(GetHookedCell);
        }

        Cell GetHookedCell()
        {
            var content = new ViewCell();
            content.BindingContextChanged += OnBindingContextChanged;
            return content;
        }

        public static readonly BindableProperty TemplateSelectorProperty = BindableProperty.Create("TemplateSelector", typeof(IDataTemplateSelector), typeof(NoteListView), defaultBindingMode: BindingMode.TwoWay);

        public IDataTemplateSelector TemplateSelector
        {
            get { return (IDataTemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }


        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            var cell = (ViewCell)sender;
            if (TemplateSelector != null)
            {
                var template = TemplateSelector.SelectTemplate(cell, cell.BindingContext);
                var ttt = (ViewCell)template.CreateContent();
                cell.View = (ttt).View;
            }
        }
    }


    public interface IDataTemplateSelector
    {
        DataTemplate SelectTemplate(object view, object dataItem);
    }

    public class VoitingDataTemplateSelector : IDataTemplateSelector
    {
        public DataTemplate SimpleTemplate { get; set; }
        public DataTemplate ComplexTemplate { get; set; }


        public DataTemplate SelectTemplate(object view, object dataItem)
        {
            return ComplexTemplate;
        }
    }
}
