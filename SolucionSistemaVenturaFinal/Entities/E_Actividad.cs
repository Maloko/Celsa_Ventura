using System;
namespace Entities
{
    public class E_Actividad
    {
        public int IdActividad { get; set; }
        public string CodActividad { get; set; }
        public string Actividad { get; set; }
        public int IdEstadoActividad { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public string FechaCreacion { set; get; }
        public string HostCreacion { set; get; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { set; get; }
        public string HostModificacion { set; get; }
    }
}
