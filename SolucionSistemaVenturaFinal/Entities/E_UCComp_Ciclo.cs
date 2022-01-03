using System;
namespace Entities
{
    public class E_UCComp_Ciclo
    {
        
        public int IdUCCompCiclo { get; set; }
        public int IdUCComp { get; set; }
        public int IdPerfilCompCiclo { get; set; }
        public double FrecuenciaCambio { get; set; }
        public double Contador { get; set; }
        public double FrecuenciaExtendida { get; set; }
        public bool FlagCicloPrincipal { get; set; }
        public int IdEstadoCiclo { get; set; }
        public bool FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }

        public int IdPerfil { get; set; }
    }
}
