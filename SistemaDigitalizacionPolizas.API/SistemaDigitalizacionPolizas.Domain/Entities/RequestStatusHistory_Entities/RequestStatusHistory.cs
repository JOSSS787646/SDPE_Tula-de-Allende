using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.RequestStatusHistory_Entities
{
    public class RequestStatusHistory
    {
        public int IdRequestStatus { get; set; }

        public int? IdRequest { get; set; }

        public int? IdStatus { get; set; }

        public DateTime? StatusChangeDate { get; set; }

        public string? Observations { get; set; }

        public int? ResponsibleUserId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? Active { get; set; }


        // Navigation properties
        public AcquisitionRequest? Request { get; set; }

        public ApplicationStatus.ApplicationStatus? Status { get; set; }
    }
}
