using System;
namespace Entities
{
    public class E_Tarea
    {
        public int IdTarea{get;set;}
        public string CodTarea{get;set;}
        public string Tarea{get;set;}
        public int IdActividad{get;set;}
        public string Actividad { get; set; }
        public int IdEstadoT{get;set;}
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion{get;set;}
        public string FechaCreacion{get;set;}
        public string HostCreacion{get;set;}
        public int IdUsuarioModificacion{get;set;}
        public DateTime FechaModificacion{get;set;}
        public string HostModificacion { get; set; }

    }
}
