using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamLineasFichaADet2Helper : HelperBase
    {
        public CamLineasFichaADet2Helper() : base(Consultas.CamLineasFichaADet2Sql) { }

        public string SqlGetLineasFichaADet2
        {
            get { return base.GetSqlXml("GetLineasFichaADet2"); }
        }

        public string SqlSaveLineasFichaADet2
        {
            get { return base.GetSqlXml("SaveLineasFichaADet2"); }
        }

        public string SqlUpdateLineasFichaADet2
        {
            get { return base.GetSqlXml("UpdateLineasFichaADet2"); }
        }

        public string SqlGetLastLineasFichaADet2Id
        {
            get { return base.GetSqlXml("GetLastLineasFichaADet2Id"); }
        }

        public string SqlDeleteLineasFichaADet2ById
        {
            get { return base.GetSqlXml("DeleteLineasFichaADet2ById"); }
        }

        public string SqlGetLineasFichaADet2ById
        {
            get { return base.GetSqlXml("GetLineasFichaADet2ById"); }
        }
    }
}

