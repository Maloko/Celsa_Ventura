using System.Data;
using Entities;

namespace Business
{
  public class B_PerfilComp_Actividad
  {
      public int PerfilComp_Actividad_Insert(E_PerfilComp_Actividad E_PerfilComp_Actividad)
      {
          return Data.D_PerfilComp_Actividad.PerfilComp_Actividad_Insert(E_PerfilComp_Actividad);
      }

      public int PerfilComp_Actividad_Update(E_PerfilComp_Actividad E_PerfilComp_Actividad)
      {
          return Data.D_PerfilComp_Actividad.PerfilComp_Actividad_Insert(E_PerfilComp_Actividad);
      }

      public int PerfilComp_Actividad_Delete(E_PerfilComp_Actividad E_PerfilComp_Actividad)
      {
          return Data.D_PerfilComp_Actividad.PerfilComp_Actividad_Delete(E_PerfilComp_Actividad);
      }

      public DataTable PerfilComp_Actividad_List(E_Perfil E_Perfil)
      {
          return Data.D_PerfilComp_Actividad.PerfilComp_Actividad_List(E_Perfil);
      }

      public DataTable PerfilComp_Actividad_GetItem(E_PerfilComp_Actividad E_PerfilComp_Actividad)
      {
          return Data.D_PerfilComp_Actividad.PerfilComp_Actividad_GetItem(E_PerfilComp_Actividad);
      }

      public DataTable PerfilComp_Actividad_Combo()
      {
          return Data.D_PerfilComp_Actividad.PerfilComp_Actividad_Combo();
      }
  }
}
