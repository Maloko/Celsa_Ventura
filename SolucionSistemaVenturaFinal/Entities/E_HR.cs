using System;
namespace Entities
{
    public class E_HR
    {
        public int IdHR { get; set; }
        public string CodHR { get; set; }
        public int IdUC { get; set; }
        public DateTime FechaHR { get; set; }
        public string CodSolicitanteSAP { get; set; }
        public string NombreSolicitanteSAP { get; set; }
        public int IdEstadoHR { get; set; }
        public string Observacion { get; set; }
        public int IdUsuario { get; set; }
        public int FlagActivo { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
