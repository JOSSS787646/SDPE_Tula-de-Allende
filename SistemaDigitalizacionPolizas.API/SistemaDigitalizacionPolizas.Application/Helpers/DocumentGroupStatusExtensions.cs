using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Helpers
{


        //AQUI SE CAMBIAN LOS NOMBRES DEL ESTADO EL DOCUMENTO ACTUAL
        //Y EL COLOR QUE SE MOSTRARA EN LA INTERFAZ DE USUARIO
        public static class DocumentGroupStatusExtensions
        {
            // 🔥 Devuelve solo el label (sin color)
            public static string ToLabel(this DocumentGroupStatus status)
            {
                return status switch
                {
                    DocumentGroupStatus.Pendiente => "Pendiente",
                    DocumentGroupStatus.Cargado => "Cargado",
                    DocumentGroupStatus.Observado => "Observado",
                    DocumentGroupStatus.Completo => "Completo",
                    _ => "Desconocido"
                };
            }
        
    }
}
