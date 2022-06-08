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
        }

        protected override async void OnAppearing()
        {
            
        }

        private async AsyncTask PopulateItemsList()
        {
            SyncConfiguration syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);
            
            try
            {
                medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);
                
                Stopwatch s = new Stopwatch();
                s.Start();

                var displayModels = medicalRealm.All<Surgery>();

                s.Stop();

                //Stats.Text = $"Read {displayModels.Count()} objects in {s.ElapsedMilliseconds} ms. Showing only top {TopX}.";

                var firstX = displayModels.Take(TopX).ToList();

                var observableList = new ObservableCollection<Surgery>(firstX);
                Items = observableList;
                //MySurgeries.ItemsSource = Items;
            }
            catch (Exception e)
            {
                OnDisappearing();
                await DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override async void OnDisappearing()
        {
           
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            string newId = await DisplayPromptAsync("Edit Surgery", "New Procedure Id:", initialValue: "edit_");

            medicalRealm.EditSurgery(objectId, newId);

            var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
            if (listViewItem != null && !listViewItem.Procedure.ID.Equals(newId))
            {
                listViewItem.Procedure.ID = newId;
                //MySurgeries.ItemsSource = Items;
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

            medicalRealm.RemoveSurgery(objectId);

            var toRemoveFromListView = Items.FirstOrDefault(i => !string.IsNullOrEmpty(i.Procedure.ID) && i.Procedure.ID.Equals(objectId));
            if (toRemoveFromListView != null)
            {
                Items.Remove(toRemoveFromListView);
            }
        }
        private async void addButton_Clicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Surgery Details", "Enter procedure name here", 
                initialValue: "surgery-xxx");

            if (name == null)
                return;

            try
            {
                medicalRealm.AddSurgery(name, AppUserPartition, Version);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

            await PopulateItemsList();
        }

        public void OtherRealms()
        {
        }
    }
}