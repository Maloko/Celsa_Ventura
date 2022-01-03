using System;
namespace Entities
{
    public class E_UCComp
    {
        public int IdUCComp { get; set; }
        public int IdPerfilComp { get; set; }
        public int IdUC { get; set; }
        public int IdTipoDetalle { get; set; }
        public int IdItem { get; set; }
        public string NroSerie { get; set; }
        public string CodigoSAP { get; set; }
        public string DescripcionSAP { get; set; }
        public int IdEstadoUCComp { get; set; }
        public bool FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int HostModificacion { get; set; }
        public int IdPerfil { get; set; }
    }
}
