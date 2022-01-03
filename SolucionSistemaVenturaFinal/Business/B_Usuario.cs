using System.Data;
using Data;
using Entities;

namespace Business
{
    public class B_Usuario
    {
        public int Usuario_Insert(E_Usuario obje)
        {
            return D_Usuario.Usuario_Insert(obje);
        }

        public DataTable Usuario_List(E_Usuario obje)
        {
            return D_Usuario.Usuario_List(obje);
        }

        public DataTable Usuario_GetItem(E_Usuario obje)
        {
            return D_Usuario.Usuario_GetItem(obje);
        }

        public int Usuario_Update(E_Usuario obje)
        {
            return D_Usuario.Usuario_Update(obje);
        }

        public int UsuarioBloqueo_Insert(E_Usuario objE)
        {
            return D_Usuario.UsuarioBloqueo_Insert(objE);
        }
        public DataTable UsuarioBloqueo_GetItem(E_Usuario objE)
        {
            return D_Usuario.UsuarioBloqueo_GetItem(objE);
        }

        #region REQUERIMIENTO_03_CELSA
        public DataTable UsuarioCorreoG()
        {
            return D_Usuario.Usuario_CorreoG();
        }
        #endregion
    }
}
