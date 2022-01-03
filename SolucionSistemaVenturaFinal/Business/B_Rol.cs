using System.Data;
using Entities;
using Data;

namespace Business
{
    public class B_Rol
    {
        public DataTable Rol_GetItemByUsuario(E_Usuario objE)
        {
            return D_Rol.Rol_GetItemByUsuario(objE);
        }

        public DataTable Rol_Combo()
        {
            return D_Rol.Rol_Combo();
        }

        public DataTable Rol_List(E_Rol obje)
        {
            return D_Rol.Rol_List(obje);
        }
        public int Rol_Insert(E_Rol obje)
        {
            return D_Rol.Rol_Insert(obje);
        }
        public DataTable Rol_Get(E_Rol obje)
        {
            return D_Rol.Rol_Get(obje);
        }
        public int Rol_Update(E_Rol obje)
        {
            return D_Rol.Rol_Update(obje);
        }

        public DataTable Menu_List()
        {
            return D_Rol.Menu_List();
        }
        public int Rol_Menu_InsertMasivo(E_Rol obje)
        {
            return D_Rol.Rol_Menu_InsertMasivo(obje);
        }
        public void Rol_Menu_UpdateMasivo(E_Rol obje)
        {
            D_Rol.Rol_Menu_UpdateMasivo(obje);
        }
        public DataTable Rol_Menu_List(E_Rol obje)
        {
            return D_Rol.Rol_Menu_List(obje);
        }
    }
}
