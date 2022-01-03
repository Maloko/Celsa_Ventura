using System.Data;
using Entities;
namespace Business
{
  public class B_PerfilDetalle
  {
      public int PerfilDetalle_Insert(E_PerfilDetalle E_PerfilDetalle)
      {
          int ds = Data.D_PerfilDetalle.PerfilDetalle_Insert(E_PerfilDetalle);
          return ds;
      }

      public int PerfilDetalle_Update(E_PerfilDetalle E_PerfilDetalle)
      {
          int ds = Data.D_PerfilDetalle.PerfilDetalle_Update(E_PerfilDetalle);
          return ds;
      }

      public int PerfilDetalle_Delete(E_PerfilDetalle E_PerfilDetalle)
      {
          int ds = Data.D_PerfilDetalle.PerfilDetalle_Delete(E_PerfilDetalle);
          return ds;
      }

      public DataTable PerfilDetalle_List(E_Perfil E_Perfil)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilDetalle.PerfilDetalle_List(E_Perfil);
          return tbl;
      }

      public DataTable PerfilDetalle_GetItem(E_PerfilDetalle E_PerfilDetalle)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilDetalle.PerfilDetalle_GetItem(E_PerfilDetalle);
          return tbl;
      }

      public DataTable PerfilDetalle_Combo()
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilDetalle.PerfilDetalle_Combo();
          return tbl;
      }
  }
}
