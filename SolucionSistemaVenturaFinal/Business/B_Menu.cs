using System.Data;
using Data;

namespace Business
{
    public class B_Menu
    {
        public DataTable Menu_ListaOpciones(int TipoLectura)
        {
            DataTable tbl = new DataTable();
            tbl = D_Menu.Menu_ListaOpciones(TipoLectura);
            return tbl;
        }

        public static int Menu_GetByFormulario(string Formulario)
        {
            int IdMenu;
            IdMenu = D_Menu.Menu_GetByFormulario(Formulario);
            return IdMenu;
        }
    }
}
