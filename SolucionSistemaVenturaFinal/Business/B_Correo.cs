using System.Data;
using Data;

namespace Business
{
    public class B_Correo
    {
        public DataTable Correo_List()
        {
            return D_Correo.Correo_List();
        }
    }
}
