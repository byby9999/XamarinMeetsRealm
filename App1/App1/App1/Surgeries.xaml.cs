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

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Surgeries : ContentPage
    {
        private const string appId = "detailedsurgeries-adgny";
        private const string apiKey = "WrRHOfsxg2thPxl10grpDOGAtdL38tIlgVEwItGmKmMtvUozFXHUDKHgXtX1NYkz";
        public static Realms.Sync.App RealmApp;
        public static Realm surgeryRealm;

        public ObservableCollection<SurgeryWithDetail> Items { get; set; }
        public static Random random = new Random();
        public static string CurrentClient;
        public static string[] AllClients = new string[] { "client=1", "client=2" };

        public Surgeries()
        {
            InitializeComponent();
            CurrentClient = AllClients[random.Next(1, 101) % AllClients.Length];
            Items = new ObservableCollection<SurgeryWithDetail>();
            MyListView.ItemsSource = Items;
            RealmApp = Realms.Sync.App.Create(appId);
        }

        protected override void OnAppearing()
        {
            _ = PopulateItemsList();
        }

        private async AsyncTask PopulateItemsList()
        {
            if (RealmApp.CurrentUser == null)
            {
                var user = await RealmApp.LogInAsync(Credentials.Anonymous());
            }
            string partition = CurrentClient;
            try
            {
                SyncConfiguration syncConfig = new SyncConfiguration(partition, RealmApp.CurrentUser);
                surgeryRealm = await Realm.GetInstanceAsync(syncConfig);

                var surgeryList = surgeryRealm.All<SurgeryWithDetail>().ToList();

                var simpleList = surgeryRealm.All<Surgery>().ToList();

                var first10 = surgeryList.OrderBy(s => s.Surgeon.LastName).Take(10);
                var observableList = new ObservableCollection<SurgeryWithDetail>(first10.ToList());

                Items = observableList;
                MyListView.ItemsSource = Items;
                MyTitle.Text = partition;
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        private void removeButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);

            var entityToRemove = Items.FirstOrDefault(i => i.Id.HasValue && i.Id.Equals(objectId));
            if (entityToRemove != null)
            {
                Items.Remove(entityToRemove);
                MyListView.ItemsSource = Items;

                var toRemove = surgeryRealm.Find<SurgeryWithDetail>(objectId);

                if (toRemove != null)
                {
                    surgeryRealm.Write(() =>
                    {
                        surgeryRealm.Remove(toRemove);
                    });
                }
            }
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);

            string newName = await DisplayPromptAsync("Edit Surgery", "Choose a new Procedure Name for the surgery");

            var toUpdate = surgeryRealm.Find<SurgeryWithDetail>(objectId);
            if (toUpdate != null)
            {
                surgeryRealm.Write(() =>
                {
                    toUpdate.Procedure.Name = newName;

                    var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
                    if (listViewItem != null)
                    {
                        listViewItem.Procedure.Name = newName;
                        MyListView.ItemsSource = Items;
                    }
                });

            }
        }
        private async void addButton_Clicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Surgery Details", "Enter procedure name here", initialValue: " - Test Surgery - Alex");
            var newSurgery = new SurgeryWithDetail(name, CurrentClient);

            surgeryRealm.Write(() =>
            {
                surgeryRealm.Add(newSurgery);
            });
            await PopulateItemsList();
        }

        protected override async void OnDisappearing()
        {
            await RealmApp.CurrentUser.LogOutAsync();
        }

    }
}
