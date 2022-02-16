using App1.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Business
{
    public static class SurgeryBusiness 
    {
        public static List<Surgery> GetSurgeryList(this Realm realm) 
        {
            return realm.All<Surgery>().OrderBy(s => s.Surgeon.LastName).ToList();
        }
        public static List<Surgery_v2> GetSurgeryList_v2(this Realm realm)
        {
            return realm.All<Surgery_v2>().OrderBy(s => s.Surgeon.LastName).ToList();

        }
        public static List<Surgery_v3> GetSurgeryList_v3(this Realm realm)
        {
            return realm.All<Surgery_v3>().OrderBy(s => s.Surgeon.LastName).ToList();

        }
    }
}

