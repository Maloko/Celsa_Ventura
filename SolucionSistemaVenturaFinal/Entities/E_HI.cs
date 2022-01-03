using System;
namespace Entities
{
    public class E_HI
    {
        public int IdHI { get; set; }
        public string CodHI { get; set; }
        public int IdUC { get; set; }
        public DateTime FechaInspeccion { get; set; }
        public int IdOT { get; set; }
        public int IdHR { get; set; }
        public Boolean FlagRequiereOT { get; set; }
        public Boolean FlagProgramado { get; set; }
        public string CodResponsableSAP { get; set; }
        public string NombreResponsableSAP { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public double KmInicial { get; set; }
        public double KmFinal { get; set; }
        public string Observacion { get; set; }
        public int IdEstadoHI { get; set; }
        public Boolean FlagActivo { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
