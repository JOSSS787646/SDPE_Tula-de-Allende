using SistemaDigitalizacionPolizas.Domain.Entities.Acquisition_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Areas_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Community_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestManager_Entities;
using SistemaDigitalizacionPolizas.Domain.Entities.Supplier_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities
{
    public class AcquisitionRequest
    {
        public int IdRequest { get; set; }

        // ===============================
        // Información del Negocio
        // ===============================

        public string? RequestNumber { get; set; }
        public DateTime? RequestDate { get; set; }

        public string? Justification { get; set; }
        public DateTime? AuthorizationDate { get; set; }

        public DateTime? CompleteMaximeDate { get; set; }

        public string? Observations { get; set; }


        public string? CFDI { get; set; }

        // ===============================
        // Foreign Keys
        // ===============================

        public int? IdAdministrativeUnit { get; set; }
        public int? IdProject { get; set; }
        public int? IdAcquisitionType { get; set; }
        public int? IdSupplier { get; set; }
        public int? IdApplicationStatus { get; set; }
        public int? IdFundingSource { get; set; }
        public int? IdAcquisitionClassification { get; set; }
        public int? IdProgram { get; set; }
        public int? IdCommunity { get; set; }
        public int? IdBeneficiary { get; set; }
        public int? IdPaymentPolicy { get; set; }

        // ===============================
        // Navigation Properties
        // ===============================

        public AdministrativeUnit? AdministrativeUnit { get; set; }
        public Proyect? Project { get; set; }
        public AcquisitionType? AcquisitionType { get; set; }
        public Supplier? Supplier { get; set; }
        public SistemaDigitalizacionPolizas.Domain.Entities.ApplicationStatus.ApplicationStatus? ApplicationStatus { get; set; } 
        public FundingSource? FundingSource { get; set; }
        public AcquisitionClassification? AcquisitionClassification { get; set; }
        public Prog? Program { get; set; }
        public Community? Community { get; set; }
        public Beneficiary? Beneficiary { get; set; }
        public PaymentPolicy? PaymentPolicy { get; set; }

        public ICollection<RequestDocumentException> DocumentExceptions { get; set; }
    = new List<RequestDocumentException>();

        public ICollection<RequestManager> RequestManagers { get; set; }
    = new List<RequestManager>();

     




        // 🔥 Historial (cuando lo actives)
        // public ICollection<ApplicationStatus> RequestStates { get; set; } = new List<RequestState>();

        // ===============================
        // Auditoría
        // ===============================

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool Active { get; set; }
    
    }
}

