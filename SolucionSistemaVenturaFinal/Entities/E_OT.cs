using System;
namespace Entities
{
    public class E_OT
    {
        public int IdOT { get; set; }
        public string CodOT { get; set; }
        public string NombreOT { get; set; }
        public int IdTipoOT { get; set; }
        public int FlagSinUC { get; set; }
        public int IdUC { get; set; }
        public DateTime FechaProg { get; set; }
        public DateTime FechaReprog { get; set; }
        public DateTime FechaLiber { get; set; }
        public DateTime FechaCierre { get; set; }
        public string CodResponsable { get; set; }
        public string NombreResponsable { get; set; }
        public int IdTipoGeneracion { get; set; }
        public int IdEstadoOT { get; set; }
        public string MotivoPostergacion { get; set; }
        public string Observacion { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime HostModificacion { get; set; }
        public string CodUC { get; set; }
        public string IdsOTCompActividadEstado{ get; set; }
        public string IdHerramientaItems { get; set; }
        public int IsRegProveedor { get; set; }
        public int CodTipoAveria { get; set; }
    }
}
