using System;
namespace Entities
{
    public class E_UCIncidencia
    {
        public int IdUCIncidencia { get; set; }
        public string CodIncidencia { get; set; }
        public int IdUC { get; set; }
        public DateTime FechaHoraDesde { get; set; }
        public DateTime FechaHoraHasta { get; set; }
        public string CodSolicitante { get; set; }
        public string CodResponsable { get; set; }
        public int IdTipoIncidencia { get; set; }
        public string Observacion { get; set; }
        public int IdEstadoIncidencia { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaIncidencia { get; set; }

        public string PlacaSerie { get; set; }
        public int IdOrigenRegistro { get; set; }
        public string NroDocCorregir { get; set; }

    }
}
