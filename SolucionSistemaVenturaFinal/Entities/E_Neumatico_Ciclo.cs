namespace Entities
{
    public class E_Neumatico_Ciclo
    {
        public int IdNeumaticoCiclo{get;set;}
        public int IdNeumatico{get;set;}
        public int IdCiclo{get;set;}
        public decimal Frecuencia{get;set;}
        public decimal Contador{get;set;}
        public int IdEstadoNC{get;set;}
        public int FlagActivo{get;set;}
        public int IdUsuarioCreacion{get;set;}
        public int IdUsuarioModificacion{get;set;}
    }
}
