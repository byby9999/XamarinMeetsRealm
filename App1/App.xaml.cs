using App1.Business;
using App1.Models;
using App1.Services;
using App1.Views;
using Realms;
using Realms.Sync;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static App1.Business.Configurations;

namespace App1
{
    public partial class App : Application
    {
        public static Realms.Sync.App RealmApp;

        public static Realm medicalRealm;

        public static string AppUserPartition = ""; 

        public static int CurrentDataVersion = 1;
        public static int TopX = 20;

        //public static string RealmAppId = Configurations.ChesterAccount_ChesterOrg_Project0_MyApp;


        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }
    }
}
