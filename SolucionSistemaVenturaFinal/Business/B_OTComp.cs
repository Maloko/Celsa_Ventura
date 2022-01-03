using System.Data;
using Entities;
using Data;

namespace Business
{
    public class B_OTComp
    {
        public DataTable OTComp_List(E_OTComp E_OTComp)
        {
            return D_OTComp.OTComp_List(E_OTComp);
        }

        public DataSet OT_ListCascade(E_OTComp E_OTComp)
        {
            return D_OTComp.OT_ListCascade(E_OTComp);
        }

        public DataSet OT_ListCascadeDet(E_OTComp E_OTComp)
        {
            return D_OTComp.OT_ListCascadeDet(E_OTComp);
        }
        public int OTTarea_UpdateCascade(E_OT E_OT, DataTable tblOTActividad, DataTable tblOTTareaDetalle, DataTable tblOTHerramienta, DataTable tblOTArticulo, DataTable tblOTArticuloDet)
        {
            return D_OTComp.OTTarea_UpdateCascade(E_OT, tblOTActividad, tblOTTareaDetalle, tblOTHerramienta, tblOTArticulo, tblOTArticuloDet);
        }

    }
}
