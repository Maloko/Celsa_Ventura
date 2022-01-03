using System;
using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{
    public class D_Menu
    {
        public static DataTable Menu_ListaOpciones(int TipoLectura)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_MenuOpciones", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TipoLectura", SqlDbType.Int).Value = TipoLectura;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Menu_GetByFormulario(string Formulario)
        {
            int IdMenu;
            E_Menu e_Menu = new E_Menu();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_Menu_GetByFormulario", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Formulario", SqlDbType.NVarChar).Value = Formulario;
                cmd.Parameters.Add("@IdMenu", SqlDbType.Int).Value = e_Menu.IdMenu;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                IdMenu = Int32.Parse(cmd.Parameters["@IdMenu"].Value.ToString());
                cx.Close();
            }
            return IdMenu;
        }
    }
}
