using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Business
{
    public static class ConfigValues
    {
        public const string AppId = "myapp-nktiq";

        /// <summary>
        ///  We can find here the corresponding IDs of each app client
        /// </summary>
        public static Dictionary<string, string> UserPartitionsMap = 
            new Dictionary<string, string>()
            {
                { "test12@example.com", "61c058e5559668e69ad62a8d" },
                { "test34@example.com", "61c066a7559668e69ad78346" },
                { "test56@example.com", "61c084f8f89724893240b892" },
                { "test78@example.com", "61c08508c0a82d01340e0458" }
            };

        /// Modify these to set each client's data version on app start. Values available now: 1, 2, 3
        public static Dictionary<string, int> UsersDataVersionsMap = 
            new Dictionary<string, int>
            {
                { "test12@example.com", 1},
                { "test34@example.com", 1},
                { "test56@example.com", 1},
                { "test78@example.com", 1}
            };


        /// Set this value as the Maximum data version available for all clients. 
        /// If a client's CurrentDataVersion < MaxVersionToUpgrade, client will see a button to "Update".
        /// The Update process will move the client to data version = MaxVersionToUpgrade
        /// Important - No intermediary steps will be done. e.g. if a client is in version 1 and MaxVersionToUpgrade is set to 3,
        /// and client chooses "Update", they will update directly from 1 to 3, skipping version 2.
        /// This way, we can ensure clients only update to latest versions available.
        public const int MaxVersionToUpgrade = 2;
    }
}
