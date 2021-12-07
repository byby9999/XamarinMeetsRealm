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
        public static string CurrentClient = "";

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
                "Login:", "Cancel", null, "client=1", "client=2", "client=3", "client=4");
            
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
            _ = PopulateItemsList();
        }

        private async AsyncTask PopulateItemsList()
        {
            try
            {
                var surgeryList = Realm.All<SurgeryWithDetail>().ToList();

                var first10 = surgeryList.OrderBy(s => s.Surgeon.LastName).Take(10);
                var observableList = new ObservableCollection<SurgeryWithDetail>(first10.ToList());

                Items = observableList;
                MyListView.ItemsSource = Items;
                MyTitle.Text = CurrentClient;

            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
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
                    //await Realm.SyncSession.WaitForDownloadAsync();
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
                //await Realm.SyncSession.WaitForDownloadAsync();
            }
        }
        private async void addButton_Clicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Surgery Details", "Enter procedure name here", initialValue: "Test Surgery - Alex");
            var newSurgery = new SurgeryWithDetail(name, CurrentClient);

            Realm.Write(() =>
            {
                Realm.Add(newSurgery);
            });
            //await Realm.SyncSession.WaitForDownloadAsync();
            await PopulateItemsList();
        }


        protected override async void OnDisappearing()
        {
            Realm.Dispose();
            await RealmApp.CurrentUser.LogOutAsync();

            CurrentClient = "";
            MyListView.ItemsSource = null;
        }
    }
}