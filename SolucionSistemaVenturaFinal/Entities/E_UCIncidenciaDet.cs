using System;
namespace Entities
{
    public class E_UCIncidenciaDet
    {
        public int IdUCIncidenciaDet { get; set; }
        public int IdUCIncidencia { get; set; }
        public int IdCiclo { get; set; }
        public double ContadorInicial { get; set; }
        public double ContadorFinal { get; set; }
        public int IdEstadoIncidenciaDet { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }
        public int IdUC { get; set; }
        public bool FlagNuevo { get; set; }
    }
}
