using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamLineasFichaBDetHelper : HelperBase
    {
        public CamLineasFichaBDetHelper() : base(Consultas.CamLineasFichaBDetSql) { }

        public string SqlGetLineasFichaBDet
        {
            get { return base.GetSqlXml("GetLineasFichaBDet"); }
        }

        public string SqlSaveLineasFichaBDet
        {
            get { return base.GetSqlXml("SaveLineasFichaBDet"); }
        }

        public string SqlUpdateLineasFichaBDet
        {
            get { return base.GetSqlXml("UpdateLineasFichaBDet"); }
        }

        public string SqlGetLastLineasFichaBDetId
        {
            get { return base.GetSqlXml("GetLastLineasFichaBDetId"); }
        }

        public string SqlDeleteLineasFichaBDetById
        {
            get { return base.GetSqlXml("DeleteLineasFichaBDetById"); }
        }

        public string SqlGetLineasFichaBDetById
        {
            get { return base.GetSqlXml("GetLineasFichaBDetById"); }
        }
    }
}

