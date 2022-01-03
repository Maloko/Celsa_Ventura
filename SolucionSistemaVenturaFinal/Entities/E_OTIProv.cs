using System;
namespace Entities
{
    public class E_OTIProv
    {
        public int IdOTInforme { get; set; }
        public int IdOT { get; set; }
        public string NombreFile { get; set; }
        public string RUCProv { get; set; }
        public string CodProveedor { get; set; }
        public string RazonSocialProv { get; set; }
        public string NroOCProv { get; set; }
        public double Costo { get; set; }
        public string Observacion { get; set; }
        public bool FlagActivo { get; set; }
        public DateTime FechaCierre { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstadoOT { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
