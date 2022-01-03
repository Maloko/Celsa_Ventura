using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Neumatico
    {        
        public static DataTable Neumatico_List(E_Neumatico E_Neumatico)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoN", SqlDbType.Int).Value = E_Neumatico.IdEstadoN;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Neumatico_Insert(E_Neumatico obje)
        {
            int IdNuevo = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = obje.IdNeumatico;
                cmd.Parameters["@IdNeumatico"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = obje.NroSerie;
                cmd.Parameters.Add("@CodigoSAP", SqlDbType.VarChar, 20).Value = obje.CodigoSAP;
                cmd.Parameters.Add("@DescripcionSAP", SqlDbType.VarChar, 100).Value = obje.DescripcionSAP;
                cmd.Parameters.Add("@IdDisenio", SqlDbType.Int).Value = obje.IdDisenio;
                cmd.Parameters.Add("@IdTipoBanda", SqlDbType.Int).Value = obje.IdTipoBanda;
                cmd.Parameters.Add("@FechaAlta", SqlDbType.DateTime).Value = obje.FechaAlta;
                //cmd.Parameters.Add("@FechaBaja", SqlDbType.DateTime).Value = obje.FechaBaja;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = obje.Observacion;
                cmd.Parameters.Add("@IdEstadoN", SqlDbType.Int).Value = obje.IdEstadoN;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                IdNuevo = cmd.ExecuteNonQuery();
                IdNuevo = Int32.Parse(cmd.Parameters["@IdNeumatico"].Value.ToString());
                cx.Close();
            }
            return IdNuevo;
        }
        public static int Neumatico_Update(E_Neumatico obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = obje.IdNeumatico;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = obje.NroSerie;
                cmd.Parameters.Add("@CodigoSAP", SqlDbType.VarChar, 20).Value = obje.CodigoSAP;
                cmd.Parameters.Add("@DescripcionSAP", SqlDbType.VarChar, 100).Value = obje.DescripcionSAP;
                cmd.Parameters.Add("@IdDisenio", SqlDbType.Int).Value = obje.IdDisenio;
                cmd.Parameters.Add("@IdTipoBanda", SqlDbType.Int).Value = obje.IdTipoBanda;
                cmd.Parameters.Add("@FechaAlta", SqlDbType.DateTime).Value = obje.FechaAlta;
                //cmd.Parameters.Add("@FechaBaja", SqlDbType.DateTime).Value = obje.FechaBaja;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = obje.Observacion;
                cmd.Parameters.Add("@IdEstadoN", SqlDbType.Int).Value = obje.IdEstadoN;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

        public static int Neumatico_UpdateBaja(E_Neumatico obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_UpdateBaja", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = obje.IdNeumatico;
                cmd.Parameters.Add("@FechaBaja", SqlDbType.VarChar,17).Value = obje.FechaBaja;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }

            return n;
        }

        public static DataTable Neumatico_GetItem(E_Neumatico obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = obje.IdNeumatico;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Neumatico_GetItemBySerie(E_Neumatico E_Neumatico)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_GetItemBySerie", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = E_Neumatico.IdNeumatico;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = E_Neumatico.NroSerie;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }


        public static DataTable Neumatico_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("Neumatico_Combo", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }


        public static int Neumatico_UpdateCascade(E_Neumatico E_Neumatico, DataTable tblCiclo)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                
                SqlCommand cmd = new SqlCommand("Neumatico_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Neumatico
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = E_Neumatico.IdNeumatico;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = E_Neumatico.NroSerie;
                cmd.Parameters.Add("@CodigoSAP", SqlDbType.VarChar, 20).Value = E_Neumatico.CodigoSAP;
                cmd.Parameters.Add("@DescripcionSAP", SqlDbType.VarChar, 100).Value = E_Neumatico.DescripcionSAP;
                cmd.Parameters.Add("@IdAlmacen", SqlDbType.Int).Value = E_Neumatico.IdAlmacen;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_Neumatico.IdUC;
                cmd.Parameters.Add("@IdDisenio", SqlDbType.Int).Value = E_Neumatico.IdDisenio;
                cmd.Parameters.Add("@IdTipoBanda", SqlDbType.Int).Value = E_Neumatico.IdTipoBanda;
                cmd.Parameters.Add("@FechaAlta", SqlDbType.DateTime).Value = E_Neumatico.FechaAlta;
                cmd.Parameters.Add("@FechaBaja", SqlDbType.VarChar,17).Value = E_Neumatico.FechaBaja;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_Neumatico.Observacion;
                cmd.Parameters.Add("@IdEstadoN", SqlDbType.Int).Value = E_Neumatico.IdEstadoN;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = E_Neumatico.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Neumatico.IdUsuarioCreacion;
                cmd.Parameters.Add("@NroSalidaMercancia", SqlDbType.Int).Value = E_Neumatico.NroSalidaMercancia;
                cmd.Parameters.Add("@NroLineaSalidaMercancia", SqlDbType.Int).Value = E_Neumatico.NroLineaSalidaMercancia;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_Neumatico.FechaModificacion;

                //Detalle
                cx.Open();
                cmd.Parameters.Add("@tblNeumatico_Ciclo", SqlDbType.Structured).Value = tblCiclo;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                rpta = Convert.ToInt32(cmd.Parameters["@IdError"].Value.ToString());
                //Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static DataTable Neumatico_UltimoMovimiento(E_Neumatico E_Neumatico)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_UltimoMovimiento", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = E_Neumatico.IdNeumatico;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Neumatico_InsertMasivo(E_Neumatico objE, DataTable tblNeumatico,DataTable tblNeumaticoCiclo)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_InsertMasivo", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@tblNeumatico", SqlDbType.Structured).Value = tblNeumatico;
                cmd.Parameters.Add("@tblNeumaticoCiclo", SqlDbType.Structured).Value = tblNeumaticoCiclo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.IdUsuarioCreacion;
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
