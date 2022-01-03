using System;
namespace Entities
{
    public class E_UCCompTransfer
    {
        public int IdUCCompTransfer { get; set; }
        public string CodUCCompTransfer { get; set; }
        public int IdUCComp { get; set; }
        public int IdTipoTransfer { get; set; }
        public DateTime FechaTransfer { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public int IdPerfil { get; set; }
        public int IdUCOrigen { get; set; }
        public int IdUCDestino { get; set; }
        public string Observacion { get; set; }
        public int IdEstadoTransfer { get; set; }
        
        public bool FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }

    }
}
