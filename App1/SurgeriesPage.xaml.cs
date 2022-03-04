using App1.Models;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AsyncTask = System.Threading.Tasks.Task;
using App1.Business;
using System.Diagnostics;
using static App1.Business.Configurations;

namespace App1.Views 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgeriesPage : ContentPage
    {
        public static Realms.Sync.App RealmApp;

        private static Realm medicalRealm;
        //private static Realm projectRealm;
        //private static Realm personRealm;

        public static string AppUserPartition = string.Empty;
        public static string ProjectPartition = string.Empty;
        public static string TenantPartition = string.Empty;

        public static int CurrentDataVersion = 1;
        public static int TopX = 10;

        public static string RealmAppId = Alex_AlexandraOrg_Project0_MyApp1;

        public ObservableCollection<Surgery> Items { get; set; }

        public SurgeriesPage()
        {
            InitializeComponent();
            
            Items = new ObservableCollection<Surgery>();
            MySurgeries.ItemsSource = Items;
            try
            {
                if (RealmApp == null)
                {
                    RealmApp = Realms.Sync.App.Create(RealmAppId);
                }
            }
            catch (Exception e)
            {
                DisplayAlert("Error connecting to Realm App", e.Message, "ok");
            }
        }

        protected override async void OnAppearing()
        {
            string userChosen = await DisplayActionSheet("Login as:", "Cancel", null,
                Alex.UserPartitions.Keys.ElementAt(0),
                Alex.UserPartitions.Keys.ElementAt(1),
                Alex.UserPartitions.Keys.ElementAt(2),
                Alex.UserPartitions.Keys.ElementAt(3));

            if (userChosen == null)
                return;

            string userEmail = Alex.UserPartitions[userChosen];
            string pass = userEmail.Split(new char[] { '@' })[0];

            CurrentDataVersion = Alex.DataVersionsMap_Partner[userChosen];

            //await DisplayActionSheet("Choose a person:", "Cancel", null, "tenant=1", "tenant=2", "tenant=3");
            //await DisplayActionSheet("Choose a project:", "Cancel", null, "project=A", "project=B", "project=C");
            string tenantOption = "tenant=1";
            string projectOption = "project=A";
            TenantPartition = tenantOption;
            ProjectPartition = projectOption;

            try
            {
                if (RealmApp == null) 
                {
                    RealmApp = Realms.Sync.App.Create(RealmAppId);
                }

                //LogInAsync method sets RealmApp.CurrentUser
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword(userEmail, pass));

                AppUserPartition = RealmApp.CurrentUser.Id;

            }
            catch (Realms.Exceptions.RealmException realmEx) 
            {
                await DisplayAlert(realmEx.GetBaseException().GetType().ToString(), 
                    realmEx.GetBaseException().Message, "ok");

                return;
            }

            SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);
            
            try
            {
                Stopwatch s = new Stopwatch();
                s.Start();
                medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);
                s.Stop();
            }
            catch (Exception e)
            {
                await DisplayAlert($"{e.InnerException.GetType()} Error", e.InnerException.Message, "ok");
            }
                 
            await PopulateItemsList(CurrentDataVersion);
        }

        private async AsyncTask PopulateItemsList(int dataVersion = 1)
        {
            try
            {
                var displayModels = medicalRealm.GetDisplayModels(Mode.PartnerCollections, dataVersion);

                var totalCount = medicalRealm.CountSurgeries(dataVersion);

                TotalEntries.Text = $"Showing {TopX} / {totalCount} entries";
                var firstX = displayModels.Take(TopX).ToList();
                   
                Items = new ObservableCollection<Surgery>(firstX);
                MySurgeries.ItemsSource = Items;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
            finally
            {
                Title.Text = $"{AppUserPartition}";

                Subtitle.Text = $"Data version: {CurrentDataVersion}";

                if (CurrentDataVersion < MaxVersionToUpgrade)
                {
                    UpdateVersion.IsVisible = true;
                    UpdateVersion.Text = $"Update v: {MaxVersionToUpgrade}";
                }
                else 
                {
                    UpdateVersion.IsVisible = false;
                }

                //var envValue = await RealmApp.CurrentUser.Functions.CallAsync("getEnvironmentValue");
                var normalValue = await RealmApp.CurrentUser.Functions.CallAsync("getNormalValue");

                Subtitle.Text += $" ({normalValue})";
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

            var toRemoveFromListView = Items.FirstOrDefault(i => i.Id.HasValue && i.Id.Equals(objectId));
            if (toRemoveFromListView != null)
            {
                Items.Remove(toRemoveFromListView);
                MySurgeries.ItemsSource = Items;
            }

            medicalRealm.RemoveSurgery(objectId, CurrentDataVersion);
            
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            string newName = await DisplayPromptAsync("Edit Surgery", "New Procedure Name:", initialValue: "edit_");

            medicalRealm.EditSurgery(objectId, newName, CurrentDataVersion);

            var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
            if (listViewItem != null && !listViewItem.Procedure.Name.Equals(newName))
            {
                listViewItem.Procedure.Name = newName;
                MySurgeries.ItemsSource = Items;
            }
        }

        private async void addButton_Clicked(object sender, EventArgs e)
        {
            string randomName = SurgeryBusiness.RandomName();

            string name = await DisplayPromptAsync("New Surgery Details", "Enter procedure name here", initialValue: randomName);

            if (name == null)
                return;

            try
            {
                medicalRealm.AddSurgery(name, AppUserPartition, CurrentDataVersion);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

            await PopulateItemsList(CurrentDataVersion);
        }

        private async void updateVersionButton_Clicked(object sender, EventArgs e) 
        {
            CurrentDataVersion = MaxVersionToUpgrade;

            Alex.DataVersionsMap_Partner[RealmApp.CurrentUser.Profile.Email] = MaxVersionToUpgrade;

            await PopulateItemsList(CurrentDataVersion);
        }

        protected override async void OnDisappearing()
        {
            if (medicalRealm != null)
                medicalRealm.Dispose();
            
            await RealmApp.CurrentUser.LogOutAsync();
            RealmApp = null;

            AppUserPartition = string.Empty;
            ProjectPartition = string.Empty;
            TenantPartition = string.Empty;

            MySurgeries.ItemsSource = null;
        }

        //public async Task<string> InitiateOtherRealms() 
        //{
        //    SyncConfiguration syncConfigProejct = new SyncConfiguration(ProjectPartition, RealmApp.CurrentUser);
        //    SyncConfiguration syncConfigPeople = new SyncConfiguration(TenantPartition, RealmApp.CurrentUser);
        //    var personRealm = await Realm.GetInstanceAsync(syncConfigPeople);
        //    var projectRealm = await Realm.GetInstanceAsync(syncConfigProejct);

        //    var people = personRealm.All<Person>();
        //    var preferences = personRealm.All<Preference>();

        //    var tasks = projectRealm.All<Models.Task>();
        //    var report = projectRealm.All<Report>();
        //    if (personRealm != null)
        //        personRealm.Dispose();
        //    if (projectRealm != null)
        //        projectRealm.Dispose();
        //    return $"{tenantOption}: {people.Count()} people, {preferences.Count()} prefs. {projectOption}: {tasks.Count()} tasks, {report.Count()} reports";
        //}
    }
}