using System;
namespace Entities
{
    public class E_Neumatico
    {
        public int IdNeumatico { get; set; }
        public string NroSerie { get; set; }
        public string CodigoSAP { get; set; }
        public string DescripcionSAP { get; set; }
        public int IdDisenio { get; set; }
        public int IdTipoBanda { get; set; }
        public DateTime FechaAlta { get; set; }
        public string FechaBaja { get; set; }
        public string Observacion { get; set; }
        public int IdEstadoN { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public int IdUC { get; set; }
        public int IdAlmacen { get; set; }
        public int NroSalidaMercancia { get; set; }
        public int NroLineaSalidaMercancia { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
