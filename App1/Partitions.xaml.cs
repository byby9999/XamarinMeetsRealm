using App1.Models;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public static Realm medicalRealm;
        public static Realm peopleAndPreferencesRealm;
        public static Realm tasksAndReportsRealm;

        public static string AppUserPartition = string.Empty;
        public static string TenantPartition = string.Empty;
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
            string appUser = await DisplayActionSheet("Login to App as:", "Cancel", null,
                "test12@example.com", "test34@example.com", "test56@example.com", "test78@example.com");
            string pass = appUser.Split(new char[] { '@' })[0];

            string option1 = await DisplayActionSheet("Choose first partition:", "Cancel", null,
                "tenant=1", "tenant=2", "tenant=3");

            string option2 = await DisplayActionSheet("Choose second partition:", "cancel", null, 
                "project=A", "project=B", "project=C");

            if (option1 != TenantPartition || option2 != ProjectPartition)
            {
                //this sets a value to RealmApp.CurrentUser:
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword(appUser, pass));

                LoggedIn.Text = appUser;

                RealmsChosen.Text = $"{option1}, {option2}";

                TenantPartition = option1;
                ProjectPartition = option2;

                AppUserPartition = RealmApp.CurrentUser.Id;
            }
            await PopulateItemsList(option1, option2);
        }

        private async AsyncTask PopulateItemsList(string partition1, string partition2)
        {
            SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser)
            {
                OnProgress = progress =>
                {
                    Stats.Text = $"{progress.TransferredBytes}/{progress.TransferableBytes} downloaded";
                    Console.WriteLine($"Progress: {progress.TransferredBytes}/{progress.TransferableBytes}");
                }
            };
            SyncConfiguration syncConfig1 = new SyncConfiguration(partition1, RealmApp.CurrentUser);
            SyncConfiguration syncConfig2 = new SyncConfiguration(partition2, RealmApp.CurrentUser);

            try
            {
                medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);
                peopleAndPreferencesRealm = await Realm.GetInstanceAsync(syncConfig1);
                tasksAndReportsRealm = await Realm.GetInstanceAsync(syncConfig2);

                Stopwatch s = new Stopwatch();
                s.Start();
                var surgeries = medicalRealm.All<Surgery>();
                s.Stop();
                Stats.Text = $"Read {surgeries.Count()} surgeries in {s.ElapsedMilliseconds} ms";

                var people = peopleAndPreferencesRealm.All<Person>().ToList();
                var preferences = peopleAndPreferencesRealm.All<Preference>().ToList();

                foreach(var p in preferences) 
                {
                    PreferenceItems.Add(new PreferenceDisplayModel(p.Background, p.Font));
                }

                var tasks = tasksAndReportsRealm.All<Models.Task>().ToList();
                var reports = tasksAndReportsRealm.All<Report>().ToList();

                PeopleItems = new ObservableCollection<Person>(people);
                TaskItems = new ObservableCollection<Models.Task>(tasks);
                ReportItems = new ObservableCollection<Report>(reports);
                
                People.ItemsSource = PeopleItems;
                Preferences.ItemsSource = PreferenceItems;
                Tasks.ItemsSource = TaskItems;
                Reports.ItemsSource = ReportItems;

                PeopleCount.Text = $"People ({people.Count()})";
                PreferenceCount.Text = $"Preferences ({preferences.Count()})";

                TasksCount.Text = $"Tasks ({tasks.Count()})";
                ReportCount.Text = $"Reports ({reports.Count()})";

                int totalStatuses = 0;
                if (tasks.Count() > 0) 
                {
                    foreach(var task in tasks) 
                    {
                        var syncConfigStatus = new SyncConfiguration("task="+task.Id, RealmApp.CurrentUser);
                        var statusRealm = await Realm.GetInstanceAsync(syncConfigStatus);
                        var status = statusRealm.All<Status>();

                        totalStatuses += status.Count(); //should always be 1 per task

                        statusRealm.Dispose();
                    }

                    TasksCount.Text += $" - with {totalStatuses} statuses found.";
                }

            }
            catch (Exception e)
            {
                OnDisappearing();
                await DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override async void OnDisappearing()
        {
            medicalRealm.Dispose();
            peopleAndPreferencesRealm.Dispose();
            tasksAndReportsRealm.Dispose();
            await RealmApp.CurrentUser.LogOutAsync();

            AppUserPartition = string.Empty;
            TenantPartition = string.Empty;
            ProjectPartition = string.Empty;

            People.ItemsSource = null;
            Preferences.ItemsSource = null;
            Tasks.ItemsSource = null;
            Reports.ItemsSource = null;
        }
    }
}