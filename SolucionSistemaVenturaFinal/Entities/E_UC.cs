using System;
namespace Entities
{
    public class E_UC
    {
        public string CodUc { get; set; }
        public decimal ContadorAcum { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int FlagActivo { get; set; }
        public string HostCreacion { get; set; }
        public string HostModificacion { get; set; }
        public int IdPerfil { get; set; }
        public int IdPerfilNeumatico { get; set; }
        public string IdTipoUnidad { get; set; }
        public int IdUc { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public string Observacion { get; set; }
        public string PlacaSerie { get; set; }
        public int IdEstadoUC { get; set; }
    }
}
