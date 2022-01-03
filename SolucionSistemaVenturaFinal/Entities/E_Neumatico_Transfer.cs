using System;
namespace Entities
{
    public class E_Neumatico_Transfer
    {
        public int IdNeumaticoTransfer { get; set; }
        public string CodNeumaticoTransfer { get; set; }
        public bool FlagActivo { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfilNeumatico {get;set;}
        public int IdEjeFind { get; set; }
        public int IdPosFind { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
