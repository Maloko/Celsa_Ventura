using System.Data.SqlClient;
using System.Data;
using Entities;

namespace Data
{
    public class D_Neumatico_Ciclo
    {
        public static DataTable NeumaticoCiclo_List(E_Neumatico_Ciclo E_Neumatico_Ciclo)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Neumatico_Ciclo_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = E_Neumatico_Ciclo.IdNeumatico;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static int NeumaticoCiclo_Delete(E_Neumatico_Ciclo obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("NeumaticoCiclo_Delete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumaticoCiclo", SqlDbType.Int).Value = obje.IdNeumaticoCiclo;
                cn.Open();
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }

        public static int NeumaticoCiclo_DeleteMasivo(E_Neumatico_Ciclo obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("NeumaticoCiclo_DeleteMasivo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = obje.IdNeumatico;
                cn.Open();
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }

        public static int NeumaticoCiclo_Insert(E_Neumatico_Ciclo obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("NeumaticoCiclo_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNeumaticoCiclo", SqlDbType.Int).Value = obje.IdNeumaticoCiclo;
                cmd.Parameters.Add("@IdNeumatico", SqlDbType.Int).Value = obje.IdNeumatico;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = obje.IdCiclo;
                cmd.Parameters.Add("@Frecuencia", SqlDbType.Decimal, 20).Value = obje.Frecuencia;
                cmd.Parameters.Add("@Contador", SqlDbType.Decimal, 20).Value = obje.Contador;
                cmd.Parameters.Add("@IdEstado", SqlDbType.Int).Value = obje.IdEstadoNC;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cn.Open();
                n = cmd.ExecuteNonQuery();
            }
            return n;
        }


    }
}
