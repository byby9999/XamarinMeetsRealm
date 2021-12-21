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
        public static Realm Realm;
        public static string CurrentPartition = string.Empty;
        public static int TopX = 10;

        public static Dictionary<string, string> UserPartitionsMap = new Dictionary<string, string>()
        {
            { "client=1", "61c058e5559668e69ad62a8d" },
            { "client=1 (v2)", "61c058e5559668e69ad62a8d" },
            { "client=2", "61c066a7559668e69ad78346" },
            { "client=3", "61c084f8f89724893240b892" },
            { "client=4", "61c08508c0a82d01340e0458" }
        };

        public ObservableCollection<Surgery> Items { get; set; }

        public SurgeriesPage()
        {
            InitializeComponent();
            
            Items = new ObservableCollection<Surgery>();
            MyListView.ItemsSource = Items;
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
            string userOption = await DisplayActionSheet(
                "Login as:", "Cancel", null, UserPartitionsMap.Keys.ElementAt(0), UserPartitionsMap.Keys.ElementAt(1), 
                UserPartitionsMap.Keys.ElementAt(2), UserPartitionsMap.Keys.ElementAt(3), UserPartitionsMap.Keys.ElementAt(4));

            if (userOption != CurrentPartition)
            {
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword("test12@example.com", "test12"));

                MyTitle.Text = userOption;
                CurrentPartition = UserPartitionsMap[userOption];
                
                SyncConfiguration syncConfig = new SyncConfiguration(CurrentPartition, RealmApp.CurrentUser);
                try
                {
                    Realm = await Realm.GetInstanceAsync(syncConfig);
                    if (Realm.SyncSession.State == SessionState.Inactive)
                    {
                        Realm.SyncSession.Start();
                    }
                }
                catch (Exception e) 
                {
                    await DisplayAlert("Error", e.Message, "ok");
                }
            }
            if (userOption.Contains("(v2)"))
                newVersion = true;
            await PopulateItemsList(newVersion);
        }

        private async AsyncTask PopulateItemsList(bool newVersion = false)
        {
            try
            {
                if (newVersion) 
                {
                    var surgeryListV2 = Realm.All<Surgery_v2>().ToList();
                    TotalEntries.Text = $"Showing {TopX} out of {surgeryListV2.Count} entries";

                    var firstXV2 = surgeryListV2.OrderBy(s => s.Surgeon.LastName).Take(TopX);
                    
                    var displayModels = DisplayModels.GetFrom(firstXV2);

                    Items = new ObservableCollection<Surgery>(displayModels);
                    MyListView.ItemsSource = Items;
                }
                else 
                {
                    var surgeryList = Realm.All<Surgery>().ToList();
                    TotalEntries.Text = $"Showing {TopX} out of {surgeryList.Count} entries";

                    var firstX = surgeryList.OrderBy(s => s.Surgeon.LastName).Take(TopX);

                    var displayModels = DisplayModels.GetFrom(firstX);

                    var observableList = new ObservableCollection<Surgery>(displayModels);

                    Items = observableList;
                    MyListView.ItemsSource = Items;
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
                MyListView.ItemsSource = Items;

                var toRemove = Realm.Find<Surgery>(objectId);

                if (toRemove != null)
                {
                    Realm.Write(() =>
                    {
                        Realm.Remove(toRemove);
                    }); 
                }
            }
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            
            var toUpdate = Realm.Find<Surgery>(objectId);
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

                Realm.Write(() =>
                {
                    toUpdate.Procedure = newProcedure;

                    var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
                    if (listViewItem != null && !listViewItem.Procedure.Name.Equals(newName))
                    {
                        listViewItem.Procedure.Name = newName;
                        MyListView.ItemsSource = Items;
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
                var newSurgery = new Surgery(name, CurrentPartition, "ANDROID");

                Realm.Write(() =>
                {
                    Realm.Add(newSurgery);
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
            Realm.Dispose();
            await RealmApp.CurrentUser.LogOutAsync();

            CurrentPartition = string.Empty;
            MyListView.ItemsSource = null;
        }
    }
}