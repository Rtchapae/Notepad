using Android.Content;
using Diary.Customs;
using Diary.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomListview), typeof(CustomListViewRenderer))]
namespace Diary.Droid.Renderers
{
   public class CustomListViewRenderer : ListViewRenderer
   {
       Context _context;
       public CustomListViewRenderer(Context context) : base(context)
       {
           _context = context;
       }
       protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
       {
           base.OnElementChanged(e);
           if (Control != null)
           {
               Control.VerticalScrollBarEnabled = false;
           }
       }
   }
}