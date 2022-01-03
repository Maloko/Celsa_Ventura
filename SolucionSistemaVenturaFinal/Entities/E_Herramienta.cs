using System;
namespace Entities
{
    public class E_Herramienta
    {
        public int IdHerramienta{get;set;}
        public string CodHerramienta{get;set;}
        public string Herramienta{get;set;}
        public int Cantidad{get;set;}
        public string Observacion{get;set;}
        public int IdEstadoH{get;set;}
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public string FechaCreacion { set; get; }
        public string HostCreacion { set; get; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { set; get; }
        public string HostModificacion { set; get; }
    }
}
