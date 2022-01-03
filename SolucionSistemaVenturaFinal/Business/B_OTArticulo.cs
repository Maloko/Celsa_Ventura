using Entities;
using Data;


namespace Business
{
    public class B_OTArticulo
    {
        public string BodyEmail(E_OT E_OT)
        {
            return D_OTArticulo.BodyEmail(E_OT);
        }

        public string SubjectEmail(E_OT E_OT)
        {
            return D_OTArticulo.SubjectEmail(E_OT);
        }
    }
}
