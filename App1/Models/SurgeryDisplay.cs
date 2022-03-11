using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class SurgeryDisplay
    {
        public string Id { get; set; }
        public string ProcedureName { get; set; }

        public string SurgeonFullName { get; set; }

        public SurgeryDisplay GetFrom(GenesisDbModels.Surgery s) 
        {
            return new SurgeryDisplay
            {
                Id = s.Id.ToString(),
                ProcedureName = s.Procedure.Name,
                SurgeonFullName = $"{s.Surgeon.Title} {s.Surgeon.Surname} {s.Surgeon.Forename}"
            };
        }
    }
}
