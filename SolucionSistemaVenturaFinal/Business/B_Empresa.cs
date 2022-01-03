using System.Data;
using Entities;
using Data;

namespace Business
{
    public class B_Empresa
    {
        public int Empresa_AutoUpdate()
        {
            return D_Empresa.Empresa_AutoUpdate();
        }
        public int Empresa_CargarLicencia(E_Empresa objE)
        {
            return D_Empresa.Empresa_CargarLicencia(objE);
        }

        public DataTable Empresa_GetItem(E_Empresa objE)
        {
            return D_Empresa.Empresa_GetItem(objE);
        }
    }
}
