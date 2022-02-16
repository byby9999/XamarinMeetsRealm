using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace App1.Models
{
    public static class DisplayModels
    {
        public static List<Surgery> GetFrom(IEnumerable<Surgery_v2> list)
        {
            return list.Select(s =>
                new Surgery
                {
                    Id = s.Id,
                    Surgeon = new Surgery_Surgeon
                    {
                        FirstName = s.Surgeon.FirstName,
                        LastName = s.Surgeon.LastName,
                        Title = s.Surgeon.Title,
                        Code = s.Surgeon.Code
                    },
                    Procedure = new Surgery_Procedure(s.Procedure.Code, s.Procedure.Name),
                    Extra = s.V.HasValue ? s.V.Value.ToString() : s.Message.Substring(0, 3) + "..."
                }).ToList();
        }

        public static List<Surgery> GetFrom(IEnumerable<Surgery_v3> list)
        {
            return list.Select(s =>
                new Surgery
                {
                    Id = s.Id,
                    Surgeon = new Surgery_Surgeon
                    {
                        FirstName = s.Surgeon.FirstName,
                        LastName = s.Surgeon.LastName,
                        Title = s.Surgeon.Title,
                        Code = s.Surgeon.Code
                    },
                    Procedure = new Surgery_Procedure(s.Procedure.Code, s.Procedure.Name),
                    Extra = s.V.HasValue ? s.V.Value.ToString() : s.Message.Substring(0, 3) + "..."
                }).ToList();
        }

        public static List<Surgery> GetFrom(IEnumerable<Surgery> list)
        {
            List<Surgery> result = new List<Surgery>();
            foreach(var s in list)
            {
                var displayItem = new Surgery
                {
                    Id = s.Id,
                    Surgeon = new Surgery_Surgeon
                    {
                        FirstName = s.Surgeon.FirstName,
                        LastName = s.Surgeon.LastName,
                        Title = s.Surgeon.Title,
                        Code = s.Surgeon.Code
                    },
                    Procedure = new Surgery_Procedure(s.Procedure.Code, s.Procedure.Name)
                };
                displayItem.Extra = s.V.HasValue ? s.V.Value.ToString() : "1";

                result.Add(displayItem);
            }

            return result;
        }
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
