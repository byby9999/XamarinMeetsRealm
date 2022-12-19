using App1.Models;
using Realms;
using Realms.Sync;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using Amazon;
using System.Threading.Tasks;
using App1.Business;

namespace App1.Views 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgeriesPage : ContentPage
    {
        public static Realms.Sync.App RealmApp;

        private static Realm Realm;

        public static int CurrentVersion = 1;
        public const int NextVersion = 2;
        public const string IMPLANT = "B148C594-2613-425F-9A8B-434F9AD99C55";
        public const string CONSUMABLE = "35149BF6-6351-4F2F-AD64-64D00CAD1481";

        public const string TENANT = "PARTNERCOLLECTIONS";
        public const string APPID = "genesis-partnercollections-svgjj";

        public static string CurrentUser = "test@partner.co";

        public static Dictionary<string, int> UsersVersionsMap = new Dictionary<string, int>() 
        { 
            { "test@partner.co", 1 }, 
            { "test2@partner.co", 2 }
        };

        public SurgeriesPage()
        {
            InitializeComponent();
            
            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;
            AWSConfigs.AWSRegion = "eu-west-1";
        }
        
        protected override async void OnAppearing()
        {
            try
            {
                string user = await DisplayPromptAsync(
                    title: "User", 
                    message: "choose test@partner.co for v1, test2@partner.co for v2", 
                    accept: "Login", 
                    initialValue: CurrentUser);

                CurrentUser = user;

                if (RealmApp == null) 
                {
                    RealmApp = Realms.Sync.App.Create(APPID);
                }
                CurrentVersion = UsersVersionsMap[CurrentUser];

                var countAll = await CountObjects(CurrentUser, CurrentVersion);

                UpdateAppTexts(CurrentUser, CurrentVersion, countAll);
            }

            catch (Realms.Exceptions.RealmException realmEx)
            {
                await DisplayAlert(realmEx.GetBaseException().GetType().ToString(), realmEx.GetBaseException().Message, "ok");
                return;
            }
        }

        private async Task<(int, int, int, int)> CountObjects(string userpass, int dataVersion = 1)
        {
            try 
            {
                var user = await RealmApp.LogInAsync(Credentials.EmailPassword(userpass, userpass));
                var config = new FlexibleSyncConfiguration(user);
                
                if (Realm != null) 
                { 
                    Realm.Dispose(); 
                }
                Realm = Realm.GetInstance(config);
                
                int countSurgeries = 0, countConsumables = 0, countItemBarcode = 0;
                int countImplantBarcode = 0, countConsumableBarcode = 0;
                Realm.Subscriptions.Update(() =>
                {
                    Realm.Subscriptions.RemoveAll();

                    var surgeriesQuery = Realm.All<Surgery>().Where(s => s.Partition == TENANT);
                    var consumablesQuery = Realm.All<Consumable>().Where(s => s.Partition == TENANT);

                    Realm.Subscriptions.Add(surgeriesQuery);
                    Realm.Subscriptions.Add(consumablesQuery);

                    switch (dataVersion)
                    {
                        case 1:
                            var itemBarcodeQuery = Realm.All<ItemBarcode>().Where(i => i.Partition == TENANT);

                            Realm.Subscriptions.Add(itemBarcodeQuery);
                            break;

                        case 2:
                            var implantsBarcodeQuery = Realm.All<ImplantBarcode>().Where(i => i.Partition == TENANT);
                            var consumablesBarcodeQuery = Realm.All<ConsumableBarcode>().Where(c => c.Partition == TENANT);

                            Realm.Subscriptions.Add(implantsBarcodeQuery);
                            Realm.Subscriptions.Add(consumablesBarcodeQuery);
                            break;
                    }
                });

                await Realm.Subscriptions.WaitForSynchronizationAsync();

                countSurgeries = Realm.All<Surgery>().Count();
                countConsumables = Realm.All<Consumable>().Count();

                switch (dataVersion)
                {
                    case 1:
                        countItemBarcode = Realm.All<ItemBarcode>().Count();
                        return (countSurgeries, countConsumables, countItemBarcode, 0);
                    case 2:
                        countImplantBarcode = Realm.All<ImplantBarcode>().Count();
                        countConsumableBarcode = Realm.All<ConsumableBarcode>().Count();
                        return (countSurgeries, countConsumables, countImplantBarcode, countConsumableBarcode);
                    default:
                        return (countSurgeries, countConsumables, countItemBarcode, 0);
                }
            }
            catch (Exception ex)
            {
                TotalEntries.Text = ex.Message;
                if (RealmApp.CurrentUser != null)
                {
                    await RealmApp.CurrentUser.LogOutAsync();
                }
                if (Realm != null)
                {
                    Realm.Dispose();
                }
                throw ex;
            }
        }

        protected override async void OnDisappearing()
        {
            if (RealmApp.CurrentUser != null) 
            {
                await RealmApp.CurrentUser.LogOutAsync();
            }
            if (Realm != null) 
            {
                Realm.Dispose();
            }
        }

        private void UpdateAppTexts(string user, int dataVersion, (int, int, int, int) count)
        {
            var username = user.Split('@')[0];

            MyTitle.Text = $"{username}@{TENANT} (v {dataVersion})";
            TotalEntries.Text = $"{count.Item1} Surgeries\n{count.Item2} Consumables";
            if (dataVersion == 1)
            {
                TotalEntries.Text += $"\n{count.Item3} ItemBarcodes";
                add.Text = "add 1 item barcode";
                edt.Text = "edit 1 item barcode";
                del.Text = "delete 1 item barcode";
            }
            else
            {
                TotalEntries.Text += $"\n{count.Item3} ImplantBarcodes\n{count.Item4} ConsumableBarcodes";
                add.Text = "add 1 implant/consumable barcode";
                edt.Text = "edit 1 implant/consumable barcode";
                del.Text = "delete 1 implant/consumable barcode";
            }
        }

        private async void add_Clicked(object sender, EventArgs e)
        {
            string type = await DisplayActionSheet("Choose Item Type", "Cancel", null, "implant", "consumable");
                
            Realm.Write(() =>
            {
                if (type == "implant")
                {
                    if (CurrentVersion == 1)
                    {
                        Realm.Add(ObjectGenerator.NewItemBarcode(IMPLANT));
                    }
                    else
                    {
                        Realm.Add(ObjectGenerator.NewImplantBarcode());
                    }
                }
                if (type == "consumable") 
                {
                    if (CurrentVersion == 1)
                    {
                        Realm.Add(ObjectGenerator.NewItemBarcode(CONSUMABLE));
                    }
                    else
                    {
                        Realm.Add(ObjectGenerator.NewConsumableBarcode());
                    }
                }
            });
            var countAfterInserts = await CountObjects(CurrentUser, CurrentVersion);

            UpdateAppTexts(CurrentUser, CurrentVersion, countAfterInserts);
        }


        private async void del_Clicked(object sender, EventArgs e)
        {
            if (CurrentVersion == 1)
            {
                var itemBarcode = Realm.All<ItemBarcode>().OrderByDescending(i => i.CreatedOn).First();

                Realm.Write(() =>
                {
                    itemBarcode.IsActive = false;
                });
            }
            else
            {
                var implantBarcode = Realm.All<ImplantBarcode>().OrderByDescending(i => i.CreatedOn).First();
                var consumableBarcode = Realm.All<ConsumableBarcode>().OrderByDescending(i => i.CreatedOn).First();

                Realm.Write(() =>
                {
                    implantBarcode.IsActive = false;
                    consumableBarcode.IsActive = false;
                });
            }

            var countAfterDelete = await CountObjects(CurrentUser, CurrentVersion);

            UpdateAppTexts(CurrentUser, CurrentVersion, countAfterDelete);
        }

        private void edt_Clicked(object sender, EventArgs e)
        {
            if (CurrentVersion == 1)
            {
                var itemBarcode = Realm.All<ItemBarcode>().OrderByDescending(i => i.CreatedOn).First();

                Realm.Write(() =>
                {
                    itemBarcode.UpdatedOn = DateTime.Now;
                    itemBarcode.UpdatedBy = "alex_poc";
                    itemBarcode.DeviceIdentifier = "update_" + itemBarcode.DeviceIdentifier;
                });
            }
            else
            {
                var implantBarcode = Realm.All<ImplantBarcode>().OrderByDescending(i => i.CreatedOn).First();
                var consumableBarcode = Realm.All<ConsumableBarcode>().OrderByDescending(i => i.CreatedOn).First();

                Realm.Write(() =>
                {
                    implantBarcode.UpdatedOn = DateTime.Now;
                    implantBarcode.UpdatedBy = "alex_poc";
                    implantBarcode.DeviceIdentifier = "update_" + implantBarcode.DeviceIdentifier;

                    consumableBarcode.UpdatedOn = DateTime.Now;
                    consumableBarcode.UpdatedBy = "alex_poc";
                    consumableBarcode.DeviceIdentifier = "update_" + consumableBarcode.DeviceIdentifier;
                });
            }
        }
    }
}