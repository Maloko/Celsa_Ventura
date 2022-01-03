using System;
namespace Entities
{
    public class E_Neumatico_ControlDet
    {
        public int IdNCD { get; set; }
        public int IdNC { get; set; }
        public int Posicion { get; set; }
        public int IdNeumatico { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorNuevo { get; set; }
        public Boolean FlagReencauche { get; set; }
        public string Observacion { get; set; }
        public int IdEstadoNCD { get; set; }
        public Boolean FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }
        public int IdUC { get; set; }
        public int IdCiclo { get; set; }
        public int IdUsuario { get; set; }
    }
}
