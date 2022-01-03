using System.Data;
using Entities;
namespace Business
{
  public class B_PerfilComp_Ciclo
  {

      public int PerfilComp_Ciclo_Insert(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
      {
          int ds = Data.D_PerfilComp_Ciclo.PerfilComp_Ciclo_Insert(E_PerfilComp_Ciclo);
          return ds;
      }

      public int PerfilComp_Ciclo_Update(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
      {
          int ds = Data.D_PerfilComp_Ciclo.PerfilComp_Ciclo_Update(E_PerfilComp_Ciclo);
          return ds;
      }

      public int PerfilComp_Ciclo_Delete(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
      {
          int ds = Data.D_PerfilComp_Ciclo.PerfilComp_Ciclo_Delete(E_PerfilComp_Ciclo);
          return ds;
      }

      public DataTable PerfilComp_Ciclo_List(E_Perfil E_Perfil)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilComp_Ciclo.PerfilComp_Ciclo_List(E_Perfil);
          return tbl;
      }

      public DataTable PerfilComp_Ciclo_GetItem(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilComp_Ciclo.PerfilComp_Ciclo_GetItem(E_PerfilComp_Ciclo);
          return tbl;
      }

      public DataTable PerfilComp_Ciclo_Combo(string argServidor, string argBaseDato)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilComp_Ciclo.PerfilComp_Ciclo_Combo();
          return tbl;
      }
  }
}
