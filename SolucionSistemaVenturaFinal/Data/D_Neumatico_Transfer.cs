using System;
using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{
    public class D_Neumatico_Transfer
    {
        public static int Neumatico_Transfer_InsertMasivo(E_Neumatico_Transfer E_Neumatico_Transfer, DataTable tblNeumaticoTransferDet,DataTable tblDesecho)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("NeumaticoTransfer_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Neumatico
                cmd.Parameters.Add("@IdNeumaticoTransfer", SqlDbType.Int).Value = E_Neumatico_Transfer.IdNeumaticoTransfer;
                cmd.Parameters.Add("@CodNeumaticoTransfer", SqlDbType.VarChar, 20).Value = E_Neumatico_Transfer.CodNeumaticoTransfer;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Neumatico_Transfer.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Neumatico_Transfer.IdUsuario;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_Neumatico_Transfer.FechaModificacion;
                //Detalle
                cmd.Parameters.Add("@tblNeumaticoTransferDet", SqlDbType.Structured).Value = tblNeumaticoTransferDet;
                cmd.Parameters.Add("@tblNeumatico", SqlDbType.Structured).Value = tblDesecho;
                
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

    }
}
