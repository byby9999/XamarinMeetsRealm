using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class CustomUserData 
    {
        public string _id { get; set; }
        public string user_id { get; set; }
        public string canRead { get; set; }
        public string canWrite { get; set; }
    }
}
