using System;
using System.Data;
using System.Data.SqlClient;
using Entities;
namespace Data
{
    public class D_UC
    {

        public static DataTable UC_List(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UC_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoUC", SqlDbType.Int).Value = E_UC.IdEstadoUC;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        public int UC_InsertMasivo(E_UC E_UC, DataTable tblPerfilComponentes, DataTable tblPerfilCompCiclo)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UC_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Perfil
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UC.IdUc;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar, 20).Value = E_UC.CodUc;
                cmd.Parameters.Add("@PlacaSerie", SqlDbType.VarChar, 20).Value = E_UC.PlacaSerie;
                cmd.Parameters.Add("@IdTipoUnidad", SqlDbType.VarChar, 30).Value = E_UC.IdTipoUnidad;
                cmd.Parameters.Add("@ContadorAcum", SqlDbType.Decimal).Value = E_UC.ContadorAcum;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_UC.IdPerfil;
                #region REQUERIMIENTO_01
                //cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_UC.IdPerfilNeumatico;
                #endregion
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_UC.Observacion;
                cmd.Parameters.Add("@IdEstadoUC", SqlDbType.Int).Value = E_UC.IdEstadoUC;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = 1;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_UC.IdUsuarioCreacion;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                //Detalles
                cmd.Parameters.Add("@tblUCComp", SqlDbType.Structured).Value = tblPerfilComponentes;
                cmd.Parameters.Add("@tblItem_Ciclo", SqlDbType.Structured).Value = tblPerfilCompCiclo;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_UC.FechaModificacion;

                cmd.Parameters.Add("@FechaInicioUso", SqlDbType.DateTime).Value = E_UC.FechaInicioUso;
                cmd.Parameters.Add("@ConContadorAutomatico", SqlDbType.Bit).Value = E_UC.ConContadorAutomatico;
                cmd.Parameters.Add("@FechaUltimoControl", SqlDbType.DateTime).Value = E_UC.FechaUltimoControl;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static DataTable UC_Neumatico_List(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UC_Neumatico_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UC.IdUc;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable UC_GetItem(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("UC_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UC.IdUc;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                return tbl;
            }
        }

        public static E_UC UC_GetItemByCodUC(E_UC E_UC)
        {
            E_UC objUc = null;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("UC_GetItemByCodUc", cx))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CodUC", SqlDbType.VarChar).Value = E_UC.CodUc;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objUc = new E_UC();
                            objUc.IdUc = dr.GetInt32(0);
                            objUc.CodUc = dr.GetString(1);
                            objUc.PlacaSerie = dr.GetString(2);
                            objUc.IdTipoUnidad = dr.GetString(3);
                            objUc.ContadorAcum = dr.GetDecimal(4);
                            objUc.IdPerfil = dr.GetInt32(5);
                            if (!dr.IsDBNull(6))
                            {
                                objUc.IdPerfilNeumatico = dr.GetInt32(6);
                            }
                            objUc.Observacion = dr.GetString(7);
                            objUc.IdEstadoUC = dr.GetInt32(8);
                            objUc.FlagActivo = dr.GetBoolean(9)==true ?1:0;
                        }
                        dr.Close();
                    }
                }
            }
            return objUc;
        }

        public static E_UC UC_GetItemByIdUC(E_UC E_UC)
        {
            E_UC objUc = null;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("UC_GetItemByIdUc", cx))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUC", SqlDbType.VarChar).Value = E_UC.IdUc;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objUc = new E_UC();
                            objUc.IdUc = dr.GetInt32(0);
                            objUc.CodUc = dr.GetString(1);
                            objUc.PlacaSerie = dr.GetString(2);
                            objUc.IdTipoUnidad = dr.GetString(3);
                            objUc.ContadorAcum = dr.GetDecimal(4);
                            objUc.IdPerfil = dr.GetInt32(5);
                            if (!dr.IsDBNull(6))
                            {
                                objUc.IdPerfilNeumatico = dr.GetInt32(6);
                            }
                            objUc.Observacion = dr.GetString(7);
                            objUc.IdEstadoUC = dr.GetInt32(8);
                            objUc.FlagActivo = dr.GetBoolean(9) == true ? 1 : 0;
                            objUc.ConContadorAutomatico = dr.GetBoolean(10);
                        }
                        dr.Close();
                    }
                }
            }
            return objUc;
        }


        public static DataTable UC_Combo(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("UC_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_UC.IdPerfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                return tbl;
            }
        }

        public static DataTable UC_ComboWithPN(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("UC_ComboWithPN", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_UC.IdPerfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                return tbl;
            }
        }
        public static DataTable UC_ComboByUC(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("UC_ComboByUC", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UC.IdUc;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                return tbl;
            }
        }
        public static DataTable UC_Neumatico_ListByUC(E_UC E_UC)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_ListByUC", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UC.IdUc;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        public static DataTable UC_ComboNeumatico()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter("UCNeumatico_Combo", cx);
                da.Fill(tbl);
                return tbl;
            }
        }

        public static DataTable UC_GetBeforeChange(string IdUc)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UC_GetBeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.VarChar).Value = IdUc;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int UCCambioEstado_UpdateCascade(E_UC E_UC, DataTable tblUCEstado)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCCambioEstado_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Perfil
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_UC.Observacion;
                cmd.Parameters.Add("@IdEstadoUC", SqlDbType.Int).Value = E_UC.IdEstadoUC;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = 1;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_UC.IdUsuarioCreacion;
                cmd.Parameters.Add("@tblUCCambioEstado", SqlDbType.Structured).Value = tblUCEstado;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_UC.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                //Detalles
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int UC_CargaMasiva(E_UC objE, DataTable tblUC, DataTable tblUCComp, DataTable tblItemCiclo)
        {
            int Error = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UC_InsertMasivo", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@tblUC", SqlDbType.Structured).Value = tblUC;
                cmd.Parameters.Add("@tblUCComp", SqlDbType.Structured).Value = tblUCComp;
                cmd.Parameters.Add("@tblItemCiclo", SqlDbType.Structured).Value = tblItemCiclo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.IdUsuarioCreacion;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Error = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return Error;
        }

        #region REQUERIMIENTO_03_CELSA
        public static DataTable ContadoresxUC_List(string IdUc,int cicloPorDefecto)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("ContadoresxUC_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar).Value = IdUc;
                cmd.Parameters.Add("@CodCiclo", SqlDbType.Int).Value = cicloPorDefecto;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }


        public static void UC_UpdateFechaUltimoControl(E_OT E_OT)
        {
            int nrofil = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UC_UpdateFechaUltimoControl", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OT.IdOT;
                cmd.Parameters.Add("@FechaCierre", SqlDbType.DateTime).Value = E_OT.FechaCierre;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_OT.IdUsuarioModificacion;
                cmd.ExecuteNonQuery();
                cx.Close();
            }
        }

        #endregion
    }
}
