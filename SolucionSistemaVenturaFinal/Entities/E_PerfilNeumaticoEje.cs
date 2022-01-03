using System;
namespace Entities
{
    public class E_PerfilNeumaticoEje
    {
        public int IdPerfilNeumaticoEje { get; set; }
        public int IdPerfilNeumatico { get; set; }
        public string Eje { get; set; }
        public int NroLlantas { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }
    }
}
