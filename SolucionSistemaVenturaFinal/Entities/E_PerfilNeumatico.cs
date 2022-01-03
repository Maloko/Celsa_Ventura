//**************************************************
//Nombre:         GEN_clsPerfilNeumatico
//Descripcion:    Clase GEN_clsPerfilNeumatico
//Creado:         Generador automático
//Fecha y hora:   24/07/2014 08:07 a.m.
//**************************************************

using System;
namespace Entities
{
    public class E_PerfilNeumatico
    {
        public int IdPerfilNeumatico { get; set; }
        public string CodPerfilNeumatico { get; set; }
        public string PerfilNeumatico { get; set; }
        public int NroEjes { get; set; }
        public int NroLlantaRepuesto { get; set; }
        public byte[] Foto { get; set; }
        public string Observacion { get; set; }
        public int IdEstadoPN { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string HostCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string HostModificacion { get; set; }

    }
}
