namespace Entities
{
    public class E_Empresa
    {
        public int IdEmpresa { get; set; }
        public string RUC { get; set; }
        public string Empresa { get; set; }
        public string EmpresaAbrev { get; set; }
        public string Direccion { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Movil1 { get; set; }
        public string Movil2 { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public string URL { get; set; }
        public byte[] Licencia { get; set; }
        public int FlagActivo { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }

    }
}
