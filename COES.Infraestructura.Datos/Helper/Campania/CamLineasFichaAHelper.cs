using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamLineasFichaAHelper : HelperBase
    {
        public CamLineasFichaAHelper() : base(Consultas.CamLineasFichaASql) { }

        public string SqlGetLineasFichaA
        {
            get { return base.GetSqlXml("GetLineasFichaA"); }
        }

        public string SqlSaveLineasFichaA
        {
            get { return base.GetSqlXml("SaveLineasFichaA"); }
        }

        public string SqlUpdateLineasFichaA
        {
            get { return base.GetSqlXml("UpdateLineasFichaA"); }
        }

        public string SqlGetLastLineasFichaAId
        {
            get { return base.GetSqlXml("GetLastLineasFichaAId"); }
        }

        public string SqlDeleteLineasFichaAById
        {
            get { return base.GetSqlXml("DeleteLineasFichaAById"); }
        }

        public string SqlGetLineasFichaAById
        {
            get { return base.GetSqlXml("GetLineasFichaAById"); }
        }
    }
}

