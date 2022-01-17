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
        public static string TenantPartition = string.Empty;

        public static int CurrentDataVersion = 1;
        public static int TopX = 10;

        public static Dictionary<string, string> UserPartitionsMap = new Dictionary<string, string>()
        {
            { "test12@example.com", "61c058e5559668e69ad62a8d" },
            { "test34@example.com", "61c066a7559668e69ad78346" },
            { "test56@example.com", "61c084f8f89724893240b892" },
            { "test78@example.com", "61c08508c0a82d01340e0458" }
        };

        //modify this to change a user's specific data version. Available now: 1, 2, 3
        public static Dictionary<string, int> UsersDataVersions = new Dictionary<string, int>
        {
            { "test12@example.com", 3},
            { "test34@example.com", 1},
            { "test56@example.com", 1},
            { "test78@example.com", 1}
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
            string emailChosen = await DisplayActionSheet("Login as:", "Cancel", null,
                "test12@example.com", "test34@example.com", "test56@example.com", "test78@example.com");

            CurrentDataVersion = UsersDataVersions[emailChosen];

            string tenantOption = await DisplayActionSheet("Choose a person:", "Cancel", null,
                "tenant=1", "tenant=2", "tenant=3");

            string projectOption = await DisplayActionSheet("Choose a project:", "Cancel", null, 
                "project=A", "project=B", "project=C");

            if (tenantOption != TenantPartition || projectOption != ProjectPartition)
            {
                //This method sets RealmApp.CurrentUser
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword(emailChosen, emailChosen.Split(new char[] { '@' })[0]));

                Title.Text = $"{emailChosen} (v{CurrentDataVersion})";

                AppUserPartition = RealmApp.CurrentUser.Id;
                TenantPartition = tenantOption;
                ProjectPartition = projectOption;
                
                SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);
                SyncConfiguration syncConfigProejct = new SyncConfiguration(ProjectPartition, RealmApp.CurrentUser);
                SyncConfiguration syncConfigPeople = new SyncConfiguration(TenantPartition, RealmApp.CurrentUser);
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

                    Subtitle.Text = $"{tenantOption}: {people.Count()} people, {preferences.Count()} prefs. {projectOption}: {tasks.Count()} tasks, {report.Count()} reports";

                }
                catch (Exception e) 
                {
                    await DisplayAlert("Error", e.Message, "ok");
                }
            }

            await PopulateItemsList(CurrentDataVersion);
        }

        private async AsyncTask PopulateItemsList(int dataVersion = 1)
        {
            try
            {
                switch (dataVersion) 
                {
                    case 3:
                        {
                            var surgeryListV3 = medicalRealm.All<Surgery_v3>().ToList();
                            TotalEntries.Text = $"Showing {TopX} out of {surgeryListV3.Count} entries";
                            var firstX = surgeryListV3.OrderBy(s => s.Surgeon.LastName).Take(TopX);
                            var displayModels = DisplayModels.GetFrom(firstX);

                            Items = new ObservableCollection<Surgery>(displayModels);
                            MySurgeries.ItemsSource = Items;

                            break;
                        }
                    case 2:
                        {
                            var surgeryListV2 = medicalRealm.All<Surgery_v2>().ToList();
                            TotalEntries.Text = $"Showing {TopX} out of {surgeryListV2.Count} entries";
                            var firstX = surgeryListV2.OrderBy(s => s.Surgeon.LastName).Take(TopX);
                            var displayModels = DisplayModels.GetFrom(firstX);

                            Items = new ObservableCollection<Surgery>(displayModels);
                            MySurgeries.ItemsSource = Items;

                            break;
                        }
                    case 1: 
                        {
                            var surgeryList = medicalRealm.All<Surgery>().ToList();
                            TotalEntries.Text = $"Showing {TopX} out of {surgeryList.Count} entries";
                            var firstX = surgeryList.OrderBy(s => s.Surgeon.LastName).Take(TopX);
                            var displayModels = DisplayModels.GetFrom(firstX);

                            var observableList = new ObservableCollection<Surgery>(displayModels);
                            Items = observableList;
                            MySurgeries.ItemsSource = Items;

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
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
                switch (CurrentDataVersion)
                {
                    case 3:
                        {
                            var surgeryToRemove = medicalRealm.Find<Surgery_v3>(objectId);
                            if (surgeryToRemove != null)
                            {
                                medicalRealm.Write(() =>
                                {
                                    medicalRealm.Remove(surgeryToRemove);
                                });
                            }
                            break;
                        }
                    case 2:
                        {
                            var surgeryToRemove = medicalRealm.Find<Surgery_v2>(objectId);
                            if (surgeryToRemove != null)
                            {
                                medicalRealm.Write(() =>
                                {
                                    medicalRealm.Remove(surgeryToRemove);
                                });
                            }
                            break;
                        }
                    case 1:
                        {
                            var surgeryToRemove = medicalRealm.Find<Surgery>(objectId);
                            if (surgeryToRemove != null)
                            {
                                medicalRealm.Write(() =>
                                {
                                    medicalRealm.Remove(surgeryToRemove);
                                });
                            }
                            break;
                        }
                } 
            }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            string newName = "";

            switch (CurrentDataVersion)
            {
                case 3:
                    var surgery3 = medicalRealm.Find<Surgery_v3>(objectId);
                    if (surgery3 != null)
                    {
                        var procedure = surgery3.Procedure;
                        var newProcedure = new Surgery_v3_Procedure(procedure.Code);
                        
                        newName = await DisplayPromptAsync("Edit Surgery", "New Procedure Name:", initialValue: surgery3.Procedure.Name);

                        if (newName == null)
                            return;

                        newProcedure.Name = newName;

                        medicalRealm.Write(() =>
                        {
                            surgery3.Procedure = newProcedure;
                        });
                    }
                    break;
                case 2:
                    var surgery2 = medicalRealm.Find<Surgery_v2>(objectId);
                    if (surgery2 != null)
                    {
                        var procedure = surgery2.Procedure;
                        var newProcedure = new Surgery_v2_Procedure(procedure.Code);
                        newName = await DisplayPromptAsync("Edit Surgery", "New Procedure Name:", initialValue: surgery2.Procedure.Name);

                        if (newName == null)
                            return;

                        newProcedure.Name = newName;

                        medicalRealm.Write(() =>
                        {
                            surgery2.Procedure = newProcedure;
                        });
                    }
                    break;
                case 1:
                    var surgery = medicalRealm.Find<Surgery>(objectId);
                    if (surgery != null)
                    {
                        var procedure = surgery.Procedure;
                        var newProcedure = new Surgery_Procedure(procedure.Code);
                        newName = await DisplayPromptAsync("Edit Surgery", "New Procedure Name:", initialValue: surgery.Procedure.Name);

                        if (newName == null)
                            return;

                        newProcedure.Name = newName;

                        medicalRealm.Write(() =>
                        {
                            surgery.Procedure = newProcedure;
                        });
                    }
                    break;
            }
            var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
            if (listViewItem != null && !listViewItem.Procedure.Name.Equals(newName))
            {
                listViewItem.Procedure.Name = newName;
                MySurgeries.ItemsSource = Items;
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
                switch (CurrentDataVersion) 
                {
                    case 3:
                        var surgeryV3 = new Surgery_v3(name, AppUserPartition, "bodyside-3");
                        medicalRealm.Write(() =>
                        {
                            medicalRealm.Add(surgeryV3);
                        });
                        break;
                    case 2:
                        var surgeryV2 = new Surgery_v2(name, AppUserPartition, "bodyside-2");
                        medicalRealm.Write(() =>
                        {
                            medicalRealm.Add(surgeryV2);
                        });
                        break;
                    case 1:
                        var surgeryV1 = new Surgery(name, AppUserPartition, "bodyside-1");
                        medicalRealm.Write(() =>
                        {
                            medicalRealm.Add(surgeryV1);
                        });
                        break;
                } 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

            await PopulateItemsList(CurrentDataVersion);
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
            TenantPartition = string.Empty;

            MySurgeries.ItemsSource = null;
        }
    }
}