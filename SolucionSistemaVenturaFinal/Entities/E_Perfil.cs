using System;
namespace Entities
{
    public class E_Perfil
    {
        public string Codperfil { set; get; }
        public string Fechacreacion { set; get; }
        public DateTime Fechamodificacion { set; get; }
        public bool Flagactivo { set; get; }
        public string Hostcreacion { set; get; }
        public string Hostmodificacion { set; get; }
        public int Idestadop { set; get; }
        public int Idperfil { set; get; }
        public int Idperfilneumatico { set; get; }
        public string Idtipounidad { set; get; }
        public int Idusuariocreacion { set; get; }
        public int Idusuariomodificacion { set; get; }
        public string Perfil { set; get; }
        public int IdCicloDefecto { get; set; }
        public int IdUC { get; set; } //Agregado para Filtro de Ciclo_ComboByUC
        public int IdTipoCiclo { get; set; } //Agregado para Filtro de CicloCombo
        public string Ciclo { set; get; }
    }
}
