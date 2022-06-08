using App1.Models;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App1.Business;
using static App1.Business.Configurations;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace App1.Views 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgeriesPage : ContentPage
    {
        public static Realms.Sync.App RealmApp;

        private static Realm medicalRealm;
        SyncConfiguration syncConfigMedical;

        public static string AppUserPartition = string.Empty;

        public static int CurrentDataVersion = 1;
        public static int TopX = 10;

        public static string RealmAppId { get; set; }

        public static Dictionary<string, int> DataVersionsMap = new Dictionary<string, int>() { { "user1@test.com", 1}, { "user2@test.com ", 1} };

        public ObservableCollection<Surgery> Items { get; set; }

        public SurgeriesPage()
        {
            InitializeComponent();
            
            Items = new ObservableCollection<Surgery>();
            MySurgeries.ItemsSource = Items;
            
        }
        
        protected override async void OnAppearing()
        {
            try
            {
                RealmApp = Realms.Sync.App.Create("application-alex-xaruv");

                var userPoolId = Configurations.CognitoUserPoolId;
                var appClientId = Configurations.CognitoAppClientId;
                var appClientSecret = Configurations.CognitoAppClientSecret;

                var username = "madalin.stefirca@maxcode.net";
                var password = "Abc 123!";

                var provider = new AmazonCognitoIdentityProviderClient(
                          new AnonymousAWSCredentials(), Amazon.RegionEndpoint.EUWest1);
                var userPool = new CognitoUserPool(userPoolId, appClientId, provider);
                var user = new CognitoUser(username, appClientId, userPool, provider, appClientSecret);
                var authRequest = new InitiateSrpAuthRequest() { Password = password };

                var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

                var token = authResponse.AuthenticationResult.IdToken;

                 var realmUser = await RealmApp.LogInAsync(Credentials.JWT(token));

                AppUserPartition = "1";

                syncConfigMedical = new SyncConfiguration(AppUserPartition, RealmApp.CurrentUser);

                await PopulateItemsList();
            }
            catch (Realms.Exceptions.RealmException realmEx) 
            {
                await DisplayAlert(realmEx.GetBaseException().GetType().ToString(), 
                    realmEx.GetBaseException().Message, "ok");

                return;
            }
        }

        private async Task PopulateItemsList()
        {
            try
            {
                medicalRealm = await Realm.GetInstanceAsync(syncConfigMedical);

                var items = medicalRealm.All<Surgery>().ToList();
                var count = items.Count();

                var firstX = items.Take(10).ToList();
                   
                //Items = new ObservableCollection<Surgery>(firstX);
               
                Device.BeginInvokeOnMainThread(() =>
                {
                    TotalEntries.Text = $"Showing top {TopX} / {count} entries";
                });

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

            var toRemoveFromListView = Items.FirstOrDefault(i => !string.IsNullOrEmpty(i.Id.ToString()) && i.Id.Equals(objectId));
            if (toRemoveFromListView != null)
            {
                Items.Remove(toRemoveFromListView);
                MySurgeries.ItemsSource = Items;
            }

            medicalRealm.RemoveSurgery(objectId);
            
        }
        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var entityId = button.CommandParameter.ToString();
            var objectId = new ObjectId(entityId);
            string newName = await DisplayPromptAsync("Edit Surgery", "New Procedure Name:", initialValue: "edit_");

            medicalRealm.EditSurgery(objectId, newName, CurrentDataVersion);

            var listViewItem = Items.FirstOrDefault(i => i.Id.Equals(objectId));
            if (listViewItem != null && !listViewItem.Procedure.ID.Equals(newName))
            {
                listViewItem.Procedure.ID = newName;
                MySurgeries.ItemsSource = Items;
            }
        }

        private async void addButton_Clicked(object sender, EventArgs e)
        {
        }

        private async void updateVersionButton_Clicked(object sender, EventArgs e) 
        {
            CurrentDataVersion = MaxVersionToUpgrade;

            DataVersionsMap[RealmApp.CurrentUser.Profile.Email] = MaxVersionToUpgrade;

            await PopulateItemsList();
        }

        protected override async void OnDisappearing()
        {
            if (medicalRealm != null)
                medicalRealm.Dispose();
            
            await RealmApp.CurrentUser.LogOutAsync();
            RealmApp = null;

            AppUserPartition = string.Empty;

            MySurgeries.ItemsSource = null;
        }
    }
}