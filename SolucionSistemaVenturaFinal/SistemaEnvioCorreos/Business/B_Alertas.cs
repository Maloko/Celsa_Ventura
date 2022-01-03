using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using System.Data;
using Entities;

namespace Business
{
    public class B_Alertas
    {
        public DataSet Alerta_Call()
        {
            return D_Alertas.Alerta_Call();
        }

        public DataTable Alertas_Envio_Log_GetReenvios()
        {
            return D_Alertas.Alertas_Envio_Log_GetReenvios();
        }

        public DataTable Usuario_ListByFilterType()
        {
            return D_Alertas.Usuario_ListByFilterType();
        }

        public int Alertas_Envio_Log_UpdateCascade(E_Alertas E_Alertas, DataTable tblAlertasLog)
        {
            return D_Alertas.Alertas_Envio_Log_UpdateCascade(E_Alertas, tblAlertasLog);
        }

        public int Alertas_UpdateCascade(E_Alertas E_Alertas, DataTable tblAlertas)
        {
            return D_Alertas.Alertas_UpdateCascade(E_Alertas, tblAlertas); ;
        }
    }

}
