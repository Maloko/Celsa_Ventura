using System;
namespace Entities
{
    public class E_PM
    {
        public int IdPM { set; get; }
        public int IdPMComp { get; set; }
        public int IdPerfilComp { set; get; }
        public int IdPMActividad { get; set; }
        public string CodPM { set; get; }
        public string PM { set; get; }
        public int IdPerfil { set; get; }
        public int IdCiclo { set; get; }
        public double Porc01 { set; get; }
        public double Porc02 { set; get; }
        public int IdTipoOTDefecto { set; get; }
        public int IdEstadoPM { set; get; }
        public int Prioridad { set; get; }
        public bool FlagActivo { set; get; }
        public int IdUsuarioCreacion { set; get; }
        public DateTime FechaCreacion { set; get; }
        public string HostCreacion { set; get; }
        public int IdUsuarioModificacion { set; get; }
        public DateTime FechaModificacion { set; get; }
        public string HostModificacion { set; get; }
    }
}
