using System.Data;
using Data;
using Entities;

namespace Business
{
    public class B_OTIProv
    {
        public int OTInforme_UpdateCascade(E_OTIProv E_OTIProv, DataTable tblOTIPComp_Actividad)
        {
            return D_OTIProv.OTInforme_UpdateCascade(E_OTIProv, tblOTIPComp_Actividad);
        }
        public DataTable OTInforme_List(E_OTIProv E_OTIProv)
        {
            return D_OTIProv.OTInforme_List(E_OTIProv);
        }
    }
}
