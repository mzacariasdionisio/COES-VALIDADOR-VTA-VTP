using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCatalogoHelper : HelperBase
    {
        public CamCatalogoHelper() : base(Consultas.CamCatalogoSql) { }

        public string SqlGetCatalogoXDesc
        {
            get { return base.GetSqlXml("GetCatalogoXDesc"); }
        }
    }
}
