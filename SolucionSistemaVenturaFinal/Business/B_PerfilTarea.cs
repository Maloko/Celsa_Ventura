using System.Data;
using Entities;

namespace Business
{
  public class B_PerfilTarea
  {

      public int PerfilTarea_Insert(E_PerfilTarea E_PerfilTarea)
      {
          int ds = Data.D_PerfilTarea.PerfilTarea_Insert(E_PerfilTarea);
          return ds;
      }

      public int PerfilTarea_Update(E_PerfilTarea E_PerfilTarea)
      {
          int ds = Data.D_PerfilTarea.PerfilTarea_Update(E_PerfilTarea);
          return ds;
      }

      public int PerfilTarea_Delete(int IdPerfilTarea)
      {
          int ds = Data.D_PerfilTarea.PerfilTarea_Delete(IdPerfilTarea);
          return ds;
      }

      public DataTable PerfilTarea_List(E_PerfilTarea E_PerfilTarea)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilTarea.PerfilTarea_List(E_PerfilTarea);
          return tbl;  
      }

      public DataTable PerfilTarea_GetItem(int IdPerfilTarea)
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilTarea.PerfilTarea_GetItem(IdPerfilTarea);
          return tbl;  
      }

      public DataTable PerfilTarea_Combo()
      {
          DataTable tbl = new DataTable();
          tbl = Data.D_PerfilTarea.PerfilTarea_Combo();
          return tbl;  
      }
  }
}
