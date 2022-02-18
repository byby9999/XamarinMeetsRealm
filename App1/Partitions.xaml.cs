using App1.Business;
using App1.Models;
using Realms;
using Realms.Sync;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AsyncTask = System.Threading.Tasks.Task;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Partitions : ContentPage
    {
        public static Realms.Sync.App RealmApp;

        public static Realm medicalRealm;
        public static Realm peopleAndPreferencesRealm;
        public static Realm tasksAndReportsRealm;

        public static string AppUserPartition = string.Empty;
        public static string TenantPartition = string.Empty;
        public static string ProjectPartition = string.Empty;

        public static int TopX = 10;

        public ObservableCollection<Surgery> Items { get; set; }

        public Partitions()
        {
            InitializeComponent();

            Items = new ObservableCollection<Surgery>();
            MySurgeries.ItemsSource = Items;

            try
            {
                if (RealmApp == null) 
                {
                    RealmApp = Realms.Sync.App.Create(Configurations.ChesterAccount_ChesterOrg_Project0_MyApp);
                }
            } 
            catch(Exception e) 
            {
                DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override async void OnAppearing()
        {
            string choice = await DisplayActionSheet("Login to App as:", "Cancel", null,
                Configurations.UserPartitions.Keys.ElementAt(0),
                Configurations.UserPartitions.Keys.ElementAt(1),
                Configurations.UserPartitions.Keys.ElementAt(2),
                Configurations.UserPartitions.Keys.ElementAt(3));

            string email = Configurations.UserPartitions[choice];
            string pass = email.Split(new char[] { '@' })[0];

            //string option1 = await DisplayActionSheet("Choose first partition:", "Cancel", null,
            //    "tenant=1", "tenant=2", "tenant=3");

            //string option2 = await DisplayActionSheet("Choose second partition:", "cancel", null, 
            //    "project=A", "project=B", "project=C");
            string option1 = "tenant=1";
            string option2 = "project=B";

            if (RealmApp == null) 
            {
                RealmApp = Realms.Sync.App.Create(Configurations.ChesterAccount_ChesterOrg_Project0_MyApp);
            }

            //this sets a value to RealmApp.CurrentUser:
            var user = await RealmApp.LogInAsync(Credentials.EmailPassword(email, pass));

            LoggedIn.Text = choice;
            RealmsChosen.Text = $"{option1}, {option2}";

            //TenantPartition = option1;
            //ProjectPartition = option2;

            AppUserPartition = RealmApp.CurrentUser.Id;
            
            await PopulateItemsList(option1, option2);
        }

        private async AsyncTask PopulateItemsList(string partition1, string partition2)
        {
            SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);
            
            try
            {
                medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);
                
                Stopwatch s = new Stopwatch();
                s.Start();
                var surgeries = medicalRealm.All<Surgery>();
                s.Stop();
                Stats.Text = $"Read {surgeries.Count()} surgeries in {s.ElapsedMilliseconds} ms";

                var displayModels = medicalRealm.GetDisplayModels(1);
                var totalCount = medicalRealm.CountSurgeries(1);

                string message = $"Showing {TopX} / {totalCount}.";

                var observableList = new ObservableCollection<Surgery>(displayModels);
                Items = observableList;
                MySurgeries.ItemsSource = Items;
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
            
            await RealmApp.CurrentUser.LogOutAsync();

            AppUserPartition = string.Empty;
            TenantPartition = string.Empty;
            ProjectPartition = string.Empty;

            //People.ItemsSource = null;
            //Preferences.ItemsSource = null;
            //Tasks.ItemsSource = null;
            //Reports.ItemsSource = null;
        }

        public void OtherRealms()
        {
            //SyncConfiguration syncConfig1 = new SyncConfiguration(partition1, RealmApp.CurrentUser);
            //SyncConfiguration syncConfig2 = new SyncConfiguration(partition2, RealmApp.CurrentUser);
            //peopleAndPreferencesRealm = await Realm.GetInstanceAsync(syncConfig1);
            //tasksAndReportsRealm = await Realm.GetInstanceAsync(syncConfig2);
            //var people = peopleAndPreferencesRealm.All<Person>().ToList();
            //var preferences = peopleAndPreferencesRealm.All<Preference>().ToList();

            //foreach(var p in preferences) 
            //{
            //    PreferenceItems.Add(new PreferenceDisplayModel(p.Background, p.Font));
            //}

            //var tasks = tasksAndReportsRealm.All<Models.Task>().ToList();
            //var reports = tasksAndReportsRealm.All<Report>().ToList();

            //PeopleItems = new ObservableCollection<Person>(people);
            //TaskItems = new ObservableCollection<Models.Task>(tasks);
            //ReportItems = new ObservableCollection<Report>(reports);
            //PeopleCount.Text = $"People ({people.Count()})";
            //PreferenceCount.Text = $"Preferences ({preferences.Count()})";

            //TasksCount.Text = $"Tasks ({tasks.Count()})";
            //ReportCount.Text = $"Reports ({reports.Count()})";

            //int totalStatuses = 0;
            //if (tasks.Count() > 0) 
            //{
            //    foreach(var task in tasks) 
            //    {
            //        var syncConfigStatus = new SyncConfiguration("task="+task.Id, RealmApp.CurrentUser);
            //        var statusRealm = await Realm.GetInstanceAsync(syncConfigStatus);
            //        var status = statusRealm.All<Status>();

            //        totalStatuses += status.Count(); //should always be 1 per task

            //        statusRealm.Dispose();
            //    }

            //    TasksCount.Text += $" - with {totalStatuses} statuses found.";
            //}
            //People.ItemsSource = PeopleItems;
            //Preferences.ItemsSource = PreferenceItems;
            //Tasks.ItemsSource = TaskItems;
            //Reports.ItemsSource = ReportItems;
            //peopleAndPreferencesRealm.Dispose();
            //tasksAndReportsRealm.Dispose();
        }
    }
}