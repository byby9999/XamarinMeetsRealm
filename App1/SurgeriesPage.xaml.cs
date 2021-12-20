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
        private const string appId = "detailedsurgeries-adgny";
        public static Realms.Sync.App RealmApp;
        public static Realm Realm;
        public static string CurrentClient = string.Empty;
        public static int TopX = 10;

        public ObservableCollection<SurgeryWithDetail> Items { get; set; }

        public SurgeriesPage()
        {
            InitializeComponent();
            
            Items = new ObservableCollection<SurgeryWithDetail>();
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
            string userOption = await DisplayActionSheet(
                "Login as:", "Cancel", null, "client=1", "client=2", "client=3", "client=4", "client=5");
            
            if (userOption != CurrentClient)
            {
                var user = await RealmApp.LogInAsync(Credentials.Anonymous());

                CurrentClient = userOption;
                SyncConfiguration syncConfig = new SyncConfiguration(CurrentClient, RealmApp.CurrentUser);
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
            _ = PopulateItemsList(CurrentClient);
        }

        private async AsyncTask PopulateItemsList(string client)
        {
            try
            {
                if (client != "client=3")
                {
                    MyTitle.Text = CurrentClient;
                    var surgeryList = Realm.All<SurgeryWithDetail>().ToList();
                    TotalEntries.Text = $"Showing {TopX} out of {surgeryList.Count} entries";

                    var firstX = surgeryList.OrderBy(s => s.Surgeon.LastName).Take(TopX);
                    var observableList = new ObservableCollection<SurgeryWithDetail>(firstX.ToList());

                    foreach (var item in observableList)
                    {
                        item.HasMessage = string.IsNullOrEmpty(item.Message) ? ' ' : 'M';

                        item.NewVersion = (item.Version.HasValue && item.Version == 3) ? "v3" : "";
                    }
                    Items = observableList;
                    MyListView.ItemsSource = Items;
                    
                }
                else 
                {
                    MyTitle.Text = CurrentClient;
                    var allSurgeriesV4 = Realm.All<SurgeryWithDetails_v4>().ToList();
                    var allSurgeriesV1 = Realm.All<SurgeryWithDetail>().ToList();
                    TotalEntries.Text = $"Showing {TopX} out of {allSurgeriesV4.Count} entries";
                    var observableList = new ObservableCollection<SurgeryWithDetail>();

                    var surgeryListV4 = allSurgeriesV4
                        .OrderBy(s => s.Surgeon.LastName)
                        .Take(TopX).ToList();

                    var surgeryListV1 = allSurgeriesV1
                        .OrderBy(s => s.Surgeon.LastName)
                        .Take(TopX)
                        .ToList();

                    foreach(var item in surgeryListV1) 
                    {
                        observableList.Add(item);
                    }
                    foreach (var item in surgeryListV4)
                    {
                        observableList.Add(
                            new SurgeryWithDetail
                            {
                                Id = item.Id,
                                Procedure = new Procedure
                                {
                                    Name = item.Procedure.Name
                                },
                                Surgeon = new Surgeon
                                {
                                    FirstName = item.Surgeon.FirstName,
                                    LastName = item.Surgeon.LastName,
                                    Title = item.Surgeon.Title
                                },
                                NewVersion = "v4",
                                HasMessage = ' ' 
                            });
                    }
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

                var toRemove = Realm.Find<SurgeryWithDetail>(objectId);

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
            
            var toUpdate = Realm.Find<SurgeryWithDetail>(objectId);
            if (toUpdate != null)
            {
                var procedure = toUpdate.Procedure;
                var newProcedure = new Procedure() 
                {
                    Code = procedure.Code
                };
                string newName = await DisplayPromptAsync("Edit Surgery",
                    "Choose a new Procedure Name for the surgery",
                    initialValue: toUpdate.Procedure.Name);
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

            try
            {
                var newSurgery = new SurgeryWithDetail(name, CurrentClient, "ANDROID");

                Realm.Write(() =>
                {
                    Realm.Add(newSurgery);
                }); 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
            await PopulateItemsList(CurrentClient);
        }


        protected override async void OnDisappearing()
        {
            Realm.Dispose();
            await RealmApp.CurrentUser.LogOutAsync();

            CurrentClient = string.Empty;
            MyListView.ItemsSource = null;
        }
    }
}