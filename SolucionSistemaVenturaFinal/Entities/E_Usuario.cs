using System;
namespace Entities
{
    public class E_Usuario
    {
        public int IdBloqueo { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Codigo{get;set;}
        public string Apellidos{get;set;}
        public string Nombres{get;set;}
        public int IdRol{get;set;}
        public string Email{get;set;}
        public bool FlagManager{get;set;}
        public int FlagActivo{get;set;}
        public int IdUsuarioCreacion{get;set;}
        public int IdUsuarioModificacion{get;set;}
        public bool Licenciado { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
