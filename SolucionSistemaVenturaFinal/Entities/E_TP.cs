using System;
namespace Entities
{
    public class E_TP
    {
        public int IdUCCompTransfer { get; set; }

        public int IdPerfilCompOrigen { get; set; }
        public int IdPerfilCompPadreOrigen { get; set; }
        public int IdUCOrigen { get; set; }

        public int IdPerfilCompDestino { get; set; }
        public int IdPerfilCompPadreDestino { get; set; }
        public int IdUCDestino { get; set; }

        public int IdPerfilComp { get; set; }
        public int IdUCComp { get; set; }
        public int IdTipoTransfer { get; set; }
        public DateTime FechaTransfer { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string Observacion { get; set; }
        public string Bitacora { get; set; }
        public int IdEstadoTransfer { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
