using SistemaDigitalizacionPolizas.Domain.Entities.AcquisitionRequest_Entities;

namespace SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities
{
    public class COG
    {
        public int idCog { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ApplicationDetail> Details { get; set; } = new List<ApplicationDetail>();
    }
}

