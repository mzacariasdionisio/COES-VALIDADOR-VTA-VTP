using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamLineasFichaBHelper : HelperBase
    {
        public CamLineasFichaBHelper() : base(Consultas.CamLineasFichaBSql) { }

        public string SqlGetLineasFichaB
        {
            get { return base.GetSqlXml("GetLineasFichaB"); }
        }

        public string SqlSaveLineasFichaB
        {
            get { return base.GetSqlXml("SaveLineasFichaB"); }
        }

        public string SqlUpdateLineasFichaB
        {
            get { return base.GetSqlXml("UpdateLineasFichaB"); }
        }

        public string SqlGetLastLineasFichaBId
        {
            get { return base.GetSqlXml("GetLastLineasFichaBId"); }
        }

        public string SqlDeleteLineasFichaBById
        {
            get { return base.GetSqlXml("DeleteLineasFichaBById"); }
        }

        public string SqlGetLineasFichaBById
        {
            get { return base.GetSqlXml("GetLineasFichaBById"); }
        }
    }
}

