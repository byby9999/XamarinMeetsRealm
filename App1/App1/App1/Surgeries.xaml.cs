using App1.Models;
using Realms;
using Realms.Sync;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AsyncTask = System.Threading.Tasks.Task;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Surgeries : ContentPage
    {
        private const string appId = "detailedsurgeries-adgny";
        public static Realms.Sync.App RealmApp;

        public ObservableCollection<SurgeryWithDetail> Items { get; set; }
        public static Random random = new Random();

        public Surgeries()
        {
            InitializeComponent();
            Items = new ObservableCollection<SurgeryWithDetail>();
            RealmApp = Realms.Sync.App.Create(appId);
            _ = LoginToRealm();
        }

        protected override void OnAppearing()
        {
            _ = PopulateItemsList(random.Next(1, 101));
        }

        private async AsyncTask PopulateItemsList(int random)
        {
            string partition = random % 2 == 0 ?
                "doc=1014870" :
                "doc=11222";
            try
            {
                SyncConfiguration syncConfig = new SyncConfiguration(partition, RealmApp.CurrentUser);
                var surgeryRealm = await Realm.GetInstanceAsync(syncConfig);
                var surgeryList = surgeryRealm.All<SurgeryWithDetail>().ToList();
                var first20 = surgeryList.Take(20);
                var observableList = new ObservableCollection<SurgeryWithDetail>(first20.ToList());
                //int many = _surgeries.Count();
                ////To add an element into collection
                //surgeryRealm.Write(() =>
                //{
                //    surgeryRealm.Add(new SurgeryWithDetail("doc=11222"));
                //});
                ////To read all elements in collection
                //var allSurgs = surgeryRealm.All<SurgeryWithDetail>();
                MyListView.ItemsSource = observableList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async static AsyncTask LoginToRealm()
        {
            try
            {
                var user = await Task.WhenAll(RealmApp.LogInAsync(Credentials.Anonymous()));
            }
            catch (Exception e)
            {
                string message = e.Message;
                var user = await Task.WhenAll(RealmApp.LogInAsync(Credentials.Anonymous()));
            }
        }
    }
}
