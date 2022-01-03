namespace Entities
{
    public class E_FormatoImpresion
    {
        public int IdFormatoImpresion { get; set; }
        public int IdMenu { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] File { get; set; }        
        public string Fechacreacion { set; get; }
        public string Fechamodificacion { set; get; }
        public int Flagactivo { set; get; }
        public string Hostcreacion { set; get; }
        public string Hostmodificacion { set; get; }
        public int Idusuariocreacion { set; get; }
        public int Idusuariomodificacion { set; get; }
    }
}
