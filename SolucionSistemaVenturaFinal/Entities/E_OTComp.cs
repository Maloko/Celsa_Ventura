using System;
namespace Entities
{
    public class E_OTComp
    {
        public int IdOTComp {set; get;}
        public int IdUCComp { set; get; }
        public int IdOT { set; get; }
        public string CodUC { set; get; }
        public int IdTipoDetalle { set; get; }
        public int IdItem { set; get; }
        public string NroSerie { set; get; }
        public string CodigoSAP { set; get; }
        public string DescripcionSAP { set; get; }
        public int IdEstadoOTComp { set; get; }
        public bool FlagActivo { set; get; }
        public int IdUsuarioCreacion { set; get; }
        public DateTime FechaCreacion { set; get; }
        public string HostCreacion { set; get; }
        public int IdUsuarioModificacion { set; get; }
        public DateTime FechaModificacion { set; get; }
        public string HostModificacion { set; get; }
    }
}
