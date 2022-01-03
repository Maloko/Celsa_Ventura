using System;
namespace Entities
{
    public class E_Rol
    {
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public int IdCobertura { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion {get;set;}
        public int IdUsuarioModificacion {get;set;}
        public DateTime FechaModificacion {get;set;}
        public string HostModificacion { get; set; }

        public int IdMenu { get; set; }

        public string IdMenus2Insert { get; set; }
        public string IdMenus2Update { get; set; }



    }
}
