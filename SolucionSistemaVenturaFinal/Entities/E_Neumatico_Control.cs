using System;
namespace Entities
{
    public class E_Neumatico_Control
    {
        public int IdNC { get; set; }
        public string CodNC { get; set; }
        public int IdUC { get; set; }
        public DateTime FechaControl { get; set; }
        public int Ciclo { get; set; }
        public int IdEstadoNC { get; set; }
        public Boolean FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime HostModificacion { get; set; }
    }
}
