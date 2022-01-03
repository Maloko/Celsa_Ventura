using System.Data;
using System.Data.SqlClient;
using Entities;
using System.Text;

namespace Data
{
    public sealed class D_OTArticulo
    {
        public static DataTable DataEmail(E_OT E_OT)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("OTRepuestosDev_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OT.IdOT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                return tbl;
            }
        }

        public static string SubjectEmail(E_OT E_OT)
        {
            string strSubject = "";
            DataTable dtOTRep = new DataTable();
            dtOTRep = DataEmail(E_OT);
            strSubject = dtOTRep.Rows[0]["CodOT"].ToString();
            return strSubject;
        }

        public static string BodyEmail(E_OT E_OT)
        {
            string strBody = "";
            DataTable dtOTRep = new DataTable();
            dtOTRep = DataEmail(E_OT);

            StringBuilder sbBody = new StringBuilder();
            sbBody.Append("<html>");
            sbBody.Append("<body style='font-family:Verdana;'>");
            sbBody.Append("<p>Estimad@ Ejecutivo de Almacén: </p>");
            sbBody.Append("<p>Se envía la lista de repuestos a devolver:</p>");
            sbBody.Append("<table border='1' style='width:800px;font-size:12'>");
            sbBody.Append("<tr>");
            sbBody.Append("<td style='width:50px;'><center><b>#</b></center></td>");
            sbBody.Append("<td style='width:250PX;'><center><b>Código Repuestos</b></center></td>");
            sbBody.Append("<td style='width:250PX;'><center><b>Desc. Repuestos</b></center></td>");
            sbBody.Append("<td style='width:250PX;'><center><b>Cantidad a Devolver</b></center></td>");
            sbBody.Append("</tr>");
            for (int i = 0; i < dtOTRep.Rows.Count; i++)
            {
                sbBody.Append("<tr>");
                sbBody.Append("<td style='width:50px;'><center>" + (i+1).ToString() + "</center></td>");
                sbBody.Append("<td style='width:250px;'><center>" + dtOTRep.Rows[i]["CodigoSAP"].ToString() + "</center></td>");              
                sbBody.Append("<td style='width:250px;'><center>" + dtOTRep.Rows[i]["DescripcionSAP"].ToString() + "</center></td>");              
                sbBody.Append("<td style='width:250px;'><center>" + dtOTRep.Rows[i]["CANT_DEV"].ToString() + "</center></td>");
                sbBody.Append("</tr>");
            }                  
            sbBody.Append("</table>");
            sbBody.Append("<p>Saludos,</p>");
            sbBody.Append("<p>Atte. Logística</p>");
            sbBody.Append("</body>");
            sbBody.Append("</html>");
            strBody = sbBody.ToString();

            return strBody;
        }
    }
}
