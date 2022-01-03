using System;
namespace Entities
{
    public class E_ContadorDet
    {
        public int IdContadorDet { get; set; }
        public string CodUc { get; set; }
        public int IdOrigenRegistro { get; set; }
        public int IdEvento { get; set; }
        public int IdTipoOperacion { get; set; }
        public string NroDocOperacion { get; set; }
        public int IdDocCorregir { get; set; }
        public DateTime FechaHoraIni { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public double ContadorIni { get; set; }
        public double ContadorFin { get; set; }
        public string CodSolicitante { get; set; }
        public string CodResponsable { get; set; }
        public string Observacion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
