using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Data;
using Entities;

namespace Business
{
    public  class B_TablaMaestra
    {
        public DataTable TablaMaestra_Combo(E_TablaMaestra E_TablaMaestra)
        {
            return D_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra); 
        }
        public int TablaMaestra_UpdateMasivo(E_TablaMaestra E_TablaMaestra, DataTable tblTablaMaestra)
        {
            return D_TablaMaestra.TablaMaestra_UpdateMasivo(E_TablaMaestra, tblTablaMaestra);
        }
    }
}
