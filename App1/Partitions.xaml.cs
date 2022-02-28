using App1.Business;
using App1.Models;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static App1.Business.Configurations;
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

        public static int Version = 1;
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
                    RealmApp = Realms.Sync.App.Create(Chester_ChesterOrg_Project0_MyApp);
                }
            } 
            catch(Exception e) 
            {
                DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override async void OnAppearing()
        {
            string userId = await DisplayActionSheet("Login to App as:", "Cancel", null,
                Chester.UserPartitions.Keys.ElementAt(0),
                Chester.UserPartitions.Keys.ElementAt(1),
                Chester.UserPartitions.Keys.ElementAt(2),
                Chester.UserPartitions.Keys.ElementAt(3));

            string email = Chester.UserPartitions[userId];
            string pass = email.Split(new char[] { '@' })[0];

            if (RealmApp == null) 
            {
                RealmApp = Realms.Sync.App.Create(Chester_ChesterOrg_Project0_MyApp);
            }

            //this sets a value to RealmApp.CurrentUser:
            await RealmApp.LogInAsync(Credentials.EmailPassword(email, pass));

            LoggedIn.Text = userId;

            Version = Chester.DataVersionsMap_SchemaV[userId];

            Subtitle.Text = $"* Schema version pattern * Version: {Version}";

            AppUserPartition = RealmApp.CurrentUser.Id;

            await PopulateItemsList();
        }

        private async AsyncTask PopulateItemsList()
        {
            SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);
            
            try
            {
                medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);
                
                Stopwatch s = new Stopwatch();
                s.Start();

                var displayModels = medicalRealm.GetDisplayModels(Mode.SchemaVersionPattern, Version);

                s.Stop();

                Stats.Text = $"Read {displayModels.Count()} objects in {s.ElapsedMilliseconds} ms. Showing only top {TopX}.";

                var firstX = displayModels.Take(TopX).ToList();

                var observableList = new ObservableCollection<Surgery>(firstX);
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
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            string newName = await DisplayPromptAsync("Edit Surgery", "New Procedure Name:", initialValue: "edit_");

            medicalRealm.EditSurgery_SchemaVersionPattern(objectId, newName);

            var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
            if (listViewItem != null && !listViewItem.Procedure.Name.Equals(newName))
            {
                listViewItem.Procedure.Name = newName;
                MySurgeries.ItemsSource = Items;
            }
        }
        private async void removeButton_Clicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Are you sure?", "You are about to delete the object. Are you sure?", "Yes", "No");
            if (!confirm)
                return;

            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);

            medicalRealm.RemoveSurgery_SchemaVersionPattern(objectId);

            var toRemoveFromListView = Items.FirstOrDefault(i => i.Id.HasValue && i.Id.Equals(objectId));
            if (toRemoveFromListView != null)
            {
                Items.Remove(toRemoveFromListView);
                MySurgeries.ItemsSource = Items;
            }
        }
        private async void addButton_Clicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Surgery Details", "Enter procedure name here", 
                initialValue: SurgeryBusiness.RandomName());

            if (name == null)
                return;

            try
            {
                medicalRealm.AddSurgery_SchemaVersionPattern(name, AppUserPartition, Version);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

            await PopulateItemsList();
        }

        public void OtherRealms()
        {
            //SyncConfiguration syncConfig1 = new SyncConfiguration(partition1, RealmApp.CurrentUser);
            //SyncConfiguration syncConfig2 = new SyncConfiguration(partition2, RealmApp.CurrentUser);
            //peopleAndPreferencesRealm = await Realm.GetInstanceAsync(syncConfig1);
            //tasksAndReportsRealm = await Realm.GetInstanceAsync(syncConfig2);
            //var people = peopleAndPreferencesRealm.All<Person>().ToList();
            //var preferences = peopleAndPreferencesRealm.All<Preference>().ToList();
            //string option1 = await DisplayActionSheet("Choose first partition:", "Cancel", null,
            //    "tenant=1", "tenant=2", "tenant=3");

            //string option2 = await DisplayActionSheet("Choose second partition:", "cancel", null, 
            //    "project=A", "project=B", "project=C");
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

            //People.ItemsSource = null;
            //Preferences.ItemsSource = null;
            //Tasks.ItemsSource = null;
            //Reports.ItemsSource = null;
        }
    }
}