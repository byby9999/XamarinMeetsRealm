using App1.Models;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AsyncTask = System.Threading.Tasks.Task;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Partitions : ContentPage
    {
        private const string appId = "myapp-nktiq";
        public static Realms.Sync.App RealmApp;

        public static Realm userRealm;
        public static Realm projectRealm;

        public static string UserPartition = string.Empty;
        public static string ProjectPartition = string.Empty;

        public static int TopX = 10;

        public static Dictionary<string, string> UserPartitionsMap = new Dictionary<string, string>()
        {
            { "client=1", "61c058e5559668e69ad62a8d" },
            { "client=1 (v2)", "61c058e5559668e69ad62a8d" },
            { "client=2", "61c066a7559668e69ad78346" },
            { "client=3", "61c084f8f89724893240b892" },
            { "client=4", "61c08508c0a82d01340e0458" }
        };

        public ObservableCollection<Person> PeopleItems { get; set; }
        public ObservableCollection<PreferenceDisplayModel> PreferenceItems { get; set; }
        public ObservableCollection<Report> ReportItems { get; set; }
        public ObservableCollection<Models.Task> TaskItems { get; set; }

        public Partitions()
        {
            InitializeComponent();
            
            PeopleItems = new ObservableCollection<Person>();
            PreferenceItems = new ObservableCollection<PreferenceDisplayModel>();
            TaskItems = new ObservableCollection<Models.Task>();
            ReportItems = new ObservableCollection<Report>();

            People.ItemsSource = PeopleItems;
            Preferences.ItemsSource = PreferenceItems;
            Tasks.ItemsSource = TaskItems;
            Reports.ItemsSource = ReportItems;

            try
            {
                if (RealmApp == null)
                    RealmApp = Realms.Sync.App.Create(appId);
            } 
            catch(Exception e) 
            {
                DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override async void OnAppearing()
        {
            string userOption = await DisplayActionSheet("Login as:", "Cancel", null, 
                "user=5", "user=2", "user=3");

            string projectOption = await DisplayActionSheet("choose project:", "cancel", null, 
                "project=A", "project=B", "project=C");

            if (userOption != UserPartition || projectOption != ProjectPartition)
            {
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword("test12@example.com", "test12"));

                ChosenUser.Text = userOption;
                ChosenProject.Text = projectOption;

                UserPartition = userOption;
                ProjectPartition = projectOption;
            }
            await PopulateItemsList(userOption, projectOption);
        }

        private async AsyncTask PopulateItemsList(string userPartition, string projectPartition)
        {
            SyncConfiguration syncConfigUsers = new SyncConfiguration(userPartition, RealmApp.CurrentUser);
            SyncConfiguration syncConfigProjects = new SyncConfiguration(projectPartition, RealmApp.CurrentUser);

            try
            {
                userRealm = await Realm.GetInstanceAsync(syncConfigUsers);
                projectRealm = await Realm.GetInstanceAsync(syncConfigProjects);

                var people = userRealm.All<Person>().ToList();
                var preferences = userRealm.All<Preference>().ToList();

                foreach(var p in preferences) 
                {
                    PreferenceItems.Add(new PreferenceDisplayModel(p.Background, p.Font));
                }

                var tasks = projectRealm.All<Models.Task>().ToList();
                var reports = projectRealm.All<Report>().ToList();

                PeopleItems = new ObservableCollection<Person>(people);
                TaskItems = new ObservableCollection<Models.Task>(tasks);
                ReportItems = new ObservableCollection<Report>(reports);
                
                People.ItemsSource = PeopleItems;
                Preferences.ItemsSource = PreferenceItems;
                Tasks.ItemsSource = TaskItems;
                Reports.ItemsSource = ReportItems;               

            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override async void OnDisappearing()
        {
            userRealm.Dispose();
            projectRealm.Dispose();
            await RealmApp.CurrentUser.LogOutAsync();

            UserPartition = string.Empty;
            ProjectPartition = string.Empty;
            People.ItemsSource = null;
            Preferences.ItemsSource = null;
            Tasks.ItemsSource = null;
            Reports.ItemsSource = null;
        }
    }
}