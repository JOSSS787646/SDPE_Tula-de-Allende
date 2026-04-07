using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




/// <summary>
/// Clase estática que define los estados globales posibles de un documento.
/// 
/// Centraliza valores constantes para evitar inconsistencias y uso de strings
/// hardcodeados en el sistema.
/// 
/// Se utiliza para estandarizar el manejo de estados en la lógica de negocio.
/// </summary>
/// 

using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Domain.Constants
{
    public static class DocumentGlobalStatus
    {
        public const string NoAplica = "No aplica";
        public const string Pendiente = "Pendiente";
        public const string Observado = "Observado";
        public const string Cargado = "Cargado";
        public const string Completo = "Completo";
    }
}
