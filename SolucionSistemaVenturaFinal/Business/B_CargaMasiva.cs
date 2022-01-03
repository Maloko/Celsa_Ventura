using System.Data;
using Data;
using Entities;

namespace Business
{
    public class B_CargaMasiva
    {
        public DataTable CargaMasiva_GetItem(E_CargaMasiva objE)
        {
            return D_CargaMasiva.CargaMasiva_GetItem(objE);
        }
    }
}
