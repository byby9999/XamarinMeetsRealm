using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace App1.Models
{
    public static class DisplayModels
    {
        public static List<SurgeryDisplay> GetFrom(IEnumerable<GenesisDbModels.Surgery> list)
        {
            List<SurgeryDisplay> result = new List<SurgeryDisplay>();
            foreach (var s in list)
            {
                var displayItem = new SurgeryDisplay
                {
                    Id = s.Id.ToString(),
                    SurgeonFullName = $"{s.Surgeon.Title} {s.Surgeon.Forename} {s.Surgeon.Surname}",
                    ProcedureName = s.Procedure.Name
                };
                
                result.Add(displayItem);
            }

            return result;
        }

        //public static List<SurgeryDisplay> GetFrom(IEnumerable<Surgery_v2> list)
        //{
        //    return list.Select(s =>
        //        new SurgeryDisplay
        //        {
        //            Id = s.Id.ToString(),
        //            SurgeonFullName = $"{s.Surgeon.Title} {s.Surgeon.FirstName} {s.Surgeon.LastName}",
        //            ProcedureName = s.Procedure.Name
        //        }).ToList();
        //}

        //public static List<SurgeryDisplay> GetFrom(IEnumerable<Surgery_v3> list)
        //{
        //    return list.Select(s =>
        //        new SurgeryDisplay
        //        {
        //            Id = s.Id.ToString(),
        //            SurgeonFullName = $"{s.Surgeon.Title} {s.Surgeon.FirstName} {s.Surgeon.LastName}",
        //            ProcedureName = s.Procedure.Name
        //        }).ToList();
        //}

        //public static List<SurgeryDisplay> GetFrom(IEnumerable<SurgeryGlobal> list)
        //{
        //    return list.Select(s =>
        //        new SurgeryDisplay
        //        {
        //            Id = s.Id.ToString(),
        //            SurgeonFullName = $"{s.Surgeon.Title} {s.Surgeon.FirstName} {s.Surgeon.LastName}",
        //            ProcedureName = s.Procedure.Name
        //        }).ToList();
        //}
    }


    public class PreferenceDisplayModel 
    {
        public PreferenceDisplayModel(string bg, string font)
        {
            Preference1 = "Background color: ";
            Preference1 += string.IsNullOrEmpty(bg) ? "-" : bg;

            Preference2 = "Font Size: ";
            Preference2 += string.IsNullOrEmpty(font) ? "-" : font;
        }
        public string Preference1 { get; set; }
        public string Preference2 { get; set; }
    }
}
