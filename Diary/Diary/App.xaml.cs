﻿using Diary.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Diary
{
    public partial class App : Application
    {
        private readonly IPlatformInitializer _initializer;
        public static readonly IDependencyContainer Container = new DependencyContainer();
        public App()
        {
            InitializeComponent();     
        }

        public App(IPlatformInitializer initializer) :this()
        {
            _initializer = initializer;
        }

        protected override void OnStart()
        {
            _initializer?.RegisterTypes(Container);
            MainPage = new NavigationPage(new NotesListPage());
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public interface IPlatformInitializer
    {
        void RegisterTypes(IDependencyContainer container);
    }
}
