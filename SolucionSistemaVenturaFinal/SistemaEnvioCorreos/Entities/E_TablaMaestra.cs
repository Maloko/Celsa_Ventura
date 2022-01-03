using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class E_TablaMaestra
    {
        public int IdTabla { get; set; }
        public int IdColumna { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public int FlagActivo { get; set; }
        public int IdColumnaPadre { get; set; }

        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }
    }
}
