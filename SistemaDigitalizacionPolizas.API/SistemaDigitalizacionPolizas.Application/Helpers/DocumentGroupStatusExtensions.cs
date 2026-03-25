using SistemaDigitalizacionPolizas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Helpers
{
    public static class DocumentGroupStatusExtensions
    {

        //AQUI SE CAMBIAN LOS NOMBRES DEL ESTADO EL DOCUMENTO ACTUAL
        //Y EL COLOR QUE SE MOSTRARA EN LA INTERFAZ DE USUARIO
        public static (string label, string color) ToUi(this DocumentGroupStatus status)
        {
            return status switch
            {
                DocumentGroupStatus.Pendiente => ("Pendiente", "gray"),
                DocumentGroupStatus.Cargado => ("Cargado", "blue"),
                DocumentGroupStatus.EnRevision => ("En revisión", "orange"),
                DocumentGroupStatus.Completo => ("Completo", "green"),
                _ => ("Desconocido", "black")
            };
        }
    }
}
