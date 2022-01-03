namespace Entities
{
    public class E_HerramientaItem
    {
        public int IdHerramientaItem { get; set; }
        public int IdHerramienta { get; set; }
        public string NroSerie { get; set; }
        public int IdEstadoDisponible { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public string FechaCreacion { set; get; }
        public string HostCreacion { set; get; }
        public int IdUsuarioModificacion { get; set; }
        public string FechaModificacion { set; get; }
        public string HostModificacion { set; get; }
    }
}
