using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Business
{
    public static class Configurations
    {
        /// Set this value as the Maximum data version available for all clients. 
        /// If a client's CurrentDataVersion < MaxVersionToUpgrade, client will see a button to "Update".
        /// The Update process will move the client to data version = MaxVersionToUpgrade
        /// Important - No intermediary steps will be done. e.g. if a client is in version 1 and MaxVersionToUpgrade is set to 3,
        /// and client chooses "Update", they will update directly from 1 to 3, skipping version 2.
        /// This way, we can ensure clients only update to latest versions available.
        public const int MaxVersionToUpgrade = 3;

    }
}
