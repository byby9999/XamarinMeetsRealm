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
    public partial class SurgeriesPage : ContentPage
    {
        private const string appId = "myapp-nktiq";
        public static Realms.Sync.App RealmApp;

        private static Realm medicalRealm;
        private static Realm projectRealm;
        private static Realm personRealm;

        public static string AppUserPartition = string.Empty;
        public static string ProjectPartition = string.Empty;
        public static string PersonPartition = string.Empty;

        public static int TopX = 10;

        public static Dictionary<string, string> UserPartitionsMap = new Dictionary<string, string>()
        {
            { "test12@example.com", "61c058e5559668e69ad62a8d" },
            { "test34@example.com", "61c066a7559668e69ad78346" },
            { "test56@example.com", "61c084f8f89724893240b892" },
            { "test78@example.com", "61c08508c0a82d01340e0458" }
        };

        public ObservableCollection<Surgery> Items { get; set; }

        public SurgeriesPage()
        {
            InitializeComponent();
            
            Items = new ObservableCollection<Surgery>();
            MySurgeries.ItemsSource = Items;
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
            bool newVersion = false;

            string emailChosen = await DisplayActionSheet("Login as:", "Cancel", null,
                "test12@example.com", "test34@example.com", "test56@example.com", "test78@example.com");

            string personOption = await DisplayActionSheet("Choose a person:", "Cancel", null,
                "person=1", "person=2", "person=3");

            string projectOption = await DisplayActionSheet("Choose a project:", "Cancel", null, 
                "project=A", "project=B", "project=C");

            if (personOption != PersonPartition || projectOption != ProjectPartition)
            {
                //This method sets RealmApp.CurrentUser
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword(emailChosen, emailChosen.Split(new char[] { '@' })[0]));

                Title.Text = $"{emailChosen}";

                AppUserPartition = RealmApp.CurrentUser.Id;
                PersonPartition = personOption;
                ProjectPartition = projectOption;
                
                SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);
                SyncConfiguration syncConfigProejct = new SyncConfiguration(ProjectPartition, RealmApp.CurrentUser);
                SyncConfiguration syncConfigPeople = new SyncConfiguration(PersonPartition, RealmApp.CurrentUser);
                try
                {
                    medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);
                    personRealm = await Realm.GetInstanceAsync(syncConfigPeople);
                    projectRealm = await Realm.GetInstanceAsync(syncConfigProejct);

                    if (medicalRealm.SyncSession.State == SessionState.Inactive)
                    {
                        medicalRealm.SyncSession.Start();
                    }

                    var people = personRealm.All<Person>();
                    var preferences = personRealm.All<Preference>();

                    var tasks = projectRealm.All<Models.Task>();
                    var report = projectRealm.All<Report>();

                    Subtitle.Text = $"{personOption}: {people.Count()} people, {preferences.Count()} prefs. {projectOption}: {tasks.Count()} tasks, {report.Count()} reports";

                }
                catch (Exception e) 
                {
                    await DisplayAlert("Error", e.Message, "ok");
                }
            }
            //if (personOption.Contains("(v2)"))
            //    newVersion = true;
            await PopulateItemsList(newVersion);
        }

        private async AsyncTask PopulateItemsList(bool newVersion = false)
        {
            try
            {
                if (newVersion) 
                {
                    var surgeryListV2 = medicalRealm.All<Surgery_v2>().ToList();
                    TotalEntries.Text = $"Showing {TopX} out of {surgeryListV2.Count} entries";

                    var firstXV2 = surgeryListV2.OrderBy(s => s.Surgeon.LastName).Take(TopX);
                    
                    var displayModels = DisplayModels.GetFrom(firstXV2);

                    Items = new ObservableCollection<Surgery>(displayModels);
                    MySurgeries.ItemsSource = Items;
                }
                else 
                {
                    var surgeryList = medicalRealm.All<Surgery>().ToList();
                    TotalEntries.Text = $"Showing {TopX} out of {surgeryList.Count} entries";

                    var firstX = surgeryList.OrderBy(s => s.Surgeon.LastName).Take(TopX);

                    var displayModels = DisplayModels.GetFrom(firstX);

                    var observableList = new ObservableCollection<Surgery>(displayModels);

                    Items = observableList;
                    MySurgeries.ItemsSource = Items;
                }  
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
        }


        private async void removeButton_Clicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Are you sure?", 
                "You are about to delete the object. Are you sure?", "Yes", "No");
            if (!confirm)
                return;

            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);

            var entityToRemove = Items.FirstOrDefault(i => i.Id.HasValue && i.Id.Equals(objectId));
            if (entityToRemove != null)
            {
                Items.Remove(entityToRemove);
                MySurgeries.ItemsSource = Items;

                var toRemove = medicalRealm.Find<Surgery>(objectId);

                if (toRemove != null)
                {
                    medicalRealm.Write(() =>
                    {
                        medicalRealm.Remove(toRemove);
                    }); 
                }
            }
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            
            var toUpdate = medicalRealm.Find<Surgery>(objectId);
            if (toUpdate != null)
            {
                var procedure = toUpdate.Procedure;
                var newProcedure = new Surgery_Procedure() 
                {
                    Code = procedure.Code
                };
                string newName = await DisplayPromptAsync("Edit Surgery",
                    "Choose a new Procedure Name for the surgery",
                    initialValue: toUpdate.Procedure.Name);

                if (newName == null)
                    return;
                
                newProcedure.Name = newName;

                medicalRealm.Write(() =>
                {
                    toUpdate.Procedure = newProcedure;

                    var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
                    if (listViewItem != null && !listViewItem.Procedure.Name.Equals(newName))
                    {
                        listViewItem.Procedure.Name = newName;
                        MySurgeries.ItemsSource = Items;
                    }
                    
                });
            }
        }
        private async void addButton_Clicked(object sender, EventArgs e)
        {
            Random r = new Random();
            int randomPosition;
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string randomName = "surgery-";

            for (int i = 0; i < 4; i++)
            {
                randomPosition = r.Next(0, 26);
                var randomLetter = alphabet[randomPosition];
                randomName += randomLetter;
            }

            string name = await DisplayPromptAsync("New Surgery Details", "Enter procedure name here", initialValue: randomName);

            if (name == null)
                return;

            try
            {
                var newSurgery = new Surgery(name, AppUserPartition, "ANDROID");

                medicalRealm.Write(() =>
                {
                    medicalRealm.Add(newSurgery);
                }); 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
            await PopulateItemsList();
        }


        protected override async void OnDisappearing()
        {
            medicalRealm.Dispose();
            personRealm.Dispose();
            projectRealm.Dispose();

            await RealmApp.CurrentUser.LogOutAsync();
            RealmApp = null;

            AppUserPartition = string.Empty;
            ProjectPartition = string.Empty;
            PersonPartition = string.Empty;

            MySurgeries.ItemsSource = null;
        }
    }
}