using System;
namespace Entities
{
    public class E_Programacion
    {
        public int IdProgramacionRegistrada { get; set; }
        public int IdFlagTodos { get; set; }
        public int TipoMantenimiento { get; set; }
        public DateTime FechaProgramacion { get; set; }
        public int FlagActividadPendiente { get; set; }
        public int FlagActivarPriorizacion { get; set; }
        public string Observacion { get; set; }
        public bool FlagActivo { get; set; }
        public int IdUsuarioCreacion { set; get; }
        public DateTime FechaCreacion { set; get; }
        public string HostCreacion { set; get; }
        public int IdUsuarioModificacion { set; get; }
        public DateTime FechaModificacion { set; get; }
        public string HostModificacion { set; get; }
    }
}
