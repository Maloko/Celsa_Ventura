using System.Data;
using Entities;
using Data;

namespace Business
{
    public class B_HI
    {
        public int HI_UpdateCascade(E_HI E_HI, DataTable tblHIComp, DataTable tblHIComp_Actividad, DataTable tblHITarea, DataTable tblHIDetalle, DataTable tblHIHorasDetalle)
        {
            return D_HI.HI_UpdateCascade(E_HI, tblHIComp, tblHIComp_Actividad, tblHITarea, tblHIDetalle, tblHIHorasDetalle);
        }

        public int HI_UpdateEstado(E_HI E_HI)
        {
            return D_HI.HI_UpdateEstado(E_HI);
        }

        public DataTable HI_List(E_HI E_HI)
        {
            return D_HI.HI_List(E_HI);
        }

        public DataTable HI_GetItem(E_HI E_HI)
        {
            return D_HI.HI_GetItem(E_HI);
        }

        public DataTable HIComp_List(E_HI E_HI)
        {
            return D_HI.HIComp_List(E_HI);
        }

        public DataTable HIComp_Actividad_List(E_HI E_HI)
        {
            return D_HI.HIComp_Actividad_List(E_HI);
        }

        public DataTable HIDetalle_List(E_HI E_HI)
        {
            return D_HI.HIDetalle_List(E_HI);
        }

        public DataTable HITarea_List(E_HI E_HI)
        {
            return D_HI.HITarea_List(E_HI);
        }

        public DataTable HIHorasDetalle_List(E_HI E_HI)
        {
            return D_HI.HIHorasDetalle_List(E_HI);
        }
    }
}
