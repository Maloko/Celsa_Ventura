using System;
using System.Data;
using System.Data.SqlClient;
using Entities;
namespace Data
{
    public sealed class D_Perfil
    {

        public int Perfil_InsertMasivo(E_Perfil E_Perfil, DataTable tblPerfilComp, DataTable tblPerfilComp_Actividad, DataTable tblPerfilComp_Ciclo,
            DataTable tblPerfilTarea, DataTable tblPerfilDetalleHerramienta, DataTable tblPerfilDetalleRepuesto, DataTable tblPerfilDetalleConsumible)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Perfil
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Perfil", SqlDbType.VarChar, 100).Value = E_Perfil.Perfil;
                cmd.Parameters.Add("@IdTipoUnidad", SqlDbType.VarChar, 2).Value = E_Perfil.Idtipounidad;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_Perfil.Idperfilneumatico;
                cmd.Parameters.Add("@IdEstadoP", SqlDbType.Int).Value = E_Perfil.Idestadop;
                cmd.Parameters.Add("@IdCicloDefecto", SqlDbType.Int).Value = E_Perfil.IdCicloDefecto;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Perfil.Flagactivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Perfil.Idusuariocreacion;
                //Detalles
                cmd.Parameters.Add("@tblPerfilComp", SqlDbType.Structured).Value = tblPerfilComp;
                cmd.Parameters.Add("@tblPerfilActividad", SqlDbType.Structured).Value = tblPerfilComp_Actividad;
                cmd.Parameters.Add("@tblPerfilTarea", SqlDbType.Structured).Value = tblPerfilTarea;
                cmd.Parameters.Add("@tblPerfilDetalleHerrEsp", SqlDbType.Structured).Value = tblPerfilDetalleHerramienta;
                cmd.Parameters.Add("@tblPerfilDetalleConsumible", SqlDbType.Structured).Value = tblPerfilDetalleConsumible;
                cmd.Parameters.Add("@tblPerfilDetalleRepuesto", SqlDbType.Structured).Value = tblPerfilDetalleRepuesto;
                cmd.Parameters.Add("@tblPerfilCompCiclo", SqlDbType.Structured).Value = tblPerfilComp_Ciclo;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_Perfil.Fechamodificacion;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int Perfil_Insert(E_Perfil E_Perfil)
        {
            int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                cmd.Parameters["@IdPerfil"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Perfil", SqlDbType.VarChar, 20).Value = E_Perfil.Perfil;
                cmd.Parameters.Add("@IdTipoUnidad", SqlDbType.VarChar, 2).Value = E_Perfil.Idtipounidad;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_Perfil.Idperfilneumatico;
                cmd.Parameters.Add("@IdEstadoP", SqlDbType.Int).Value = E_Perfil.Idestadop;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Perfil.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_Perfil.Idusuariocreacion;
                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdPerfil"].Value.ToString());
                cx.Close();
            }
            return Id;
        }

        public static DataTable Perfil_GetItem(E_Perfil E_Perfil)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Perfil_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Perfil_List()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Perfil_ComboWithPM()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_ComboWithPM", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Perfil_Update(E_Perfil E_Perfil)
        {
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                cmd.Parameters.Add("@CodPerfil", SqlDbType.VarChar, 20).Value = E_Perfil.Codperfil;
                cmd.Parameters.Add("@Perfil", SqlDbType.VarChar, 20).Value = E_Perfil.Perfil;
                cmd.Parameters.Add("@IdTipoUnidad", SqlDbType.Int).Value = E_Perfil.Idtipounidad;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_Perfil.Idperfilneumatico;
                cmd.Parameters.Add("@IdEstadoP", SqlDbType.Int).Value = E_Perfil.Idestadop;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Perfil.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_Perfil.Idusuariomodificacion;

                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
        }

        public static int Perfil_Delete(E_Perfil E_Perfil)
        {
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
        }

        public static DataTable Perfil_GetItemByDesc(E_Perfil E_Perfil)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_GetItemByDesc", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                cmd.Parameters.Add("@Perfil", SqlDbType.VarChar, 100).Value = E_Perfil.Perfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Perfil_CargaMasiva(E_Perfil objE, DataTable tblPerfil, DataTable tblPerfilComp, DataTable tblPCCiclo, DataTable tblPCActividad, DataTable tblPerfilTarea, DataTable tblPerfilDetalle)
        {
            int Error = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Perfil_InsertMasivo", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@tblPerfil", SqlDbType.Structured).Value = tblPerfil;
                cmd.Parameters.Add("@tblPerfilComp", SqlDbType.Structured).Value = tblPerfilComp;
                cmd.Parameters.Add("@tblPefilCompCiclo", SqlDbType.Structured).Value = tblPCCiclo;
                cmd.Parameters.Add("@tblPerfilCompActividad", SqlDbType.Structured).Value = tblPCActividad;
                cmd.Parameters.Add("@tblPerfilCompActividadTarea", SqlDbType.Structured).Value = tblPerfilTarea;
                cmd.Parameters.Add("@tblPerfilDetalle", SqlDbType.Structured).Value = tblPerfilDetalle;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.Idusuariocreacion;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Error = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return Error;
        }

        public static  E_Perfil GetPerfilByCodUC(E_UC E_UC)
        {
            E_Perfil objPerfil = null;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("Perfil_GetPerfilByCodUc", cx))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CodUC", SqlDbType.VarChar).Value = E_UC.CodUc;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objPerfil = new E_Perfil();
                            objPerfil.Idperfil = dr.GetInt32(0);
                            objPerfil.Codperfil = dr.GetString(1);
                            objPerfil.IdCicloDefecto = dr.GetInt32 (2);
                            objPerfil.Ciclo = dr.GetString(3);
                            objPerfil.Idestadop = dr.GetInt32(4);
                            objPerfil.Flagactivo = dr.GetBoolean(5);
                        }
                        dr.Close();
                    }
                }
            }
            return objPerfil;
        }

    }
}
