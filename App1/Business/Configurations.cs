using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Business
{
    public static class Configurations
    {
        public const string  ChesterAccount_ChesterOrg_Project0_MyApp = "";

        public static Dictionary<string, string> UserPartitions = new Dictionary<string, string>()
        {
            { "61c058e5559668e69ad62a8d", "test12@example.com" },
            { "61c066a7559668e69ad78346", "test34@example.com" },
            { "61c084f8f89724893240b892", "test56@example.com" },
            { "61c08508c0a82d01340e0458", "test78@example.com" }
        };

        public static string DefaultPartition = UserPartitions.Keys.ElementAt(0);

        public static Dictionary<string, int> UsersDataVersionsMap = new Dictionary<string, int>()
        {
            { "61c058e5559668e69ad62a8d", 1 },
            { "61c066a7559668e69ad78346", 1 },
            { "61c084f8f89724893240b892", 1 },
            { "61c08508c0a82d01340e0458", 1 }
        };
       
        /// Set this value as the Maximum data version available for all clients. 
        /// If a client's CurrentDataVersion < MaxVersionToUpgrade, client will see a button to "Update".
        /// The Update process will move the client to data version = MaxVersionToUpgrade
        /// Important - No intermediary steps will be done. e.g. if a client is in version 1 and MaxVersionToUpgrade is set to 3,
        /// and client chooses "Update", they will update directly from 1 to 3, skipping version 2.
        /// This way, we can ensure clients only update to latest versions available.
        public const int MaxVersionToUpgrade = 3;
    }
}
