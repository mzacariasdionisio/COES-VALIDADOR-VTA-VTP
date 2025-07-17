using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamLineasFichaADet1Helper : HelperBase
    {
        public CamLineasFichaADet1Helper() : base(Consultas.CamLineasFichaADet1Sql) { }

        public string SqlGetLineasFichaADet1
        {
            get { return base.GetSqlXml("GetLineasFichaADet1"); }
        }

        public string SqlSaveLineasFichaADet1
        {
            get { return base.GetSqlXml("SaveLineasFichaADet1"); }
        }

        public string SqlUpdateLineasFichaADet1
        {
            get { return base.GetSqlXml("UpdateLineasFichaADet1"); }
        }

        public string SqlGetLastLineasFichaADet1Id
        {
            get { return base.GetSqlXml("GetLastLineasFichaADet1Id"); }
        }

        public string SqlDeleteLineasFichaADet1ById
        {
            get { return base.GetSqlXml("DeleteLineasFichaADet1ById"); }
        }

        public string SqlGetLineasFichaADet1ById
        {
            get { return base.GetSqlXml("GetLineasFichaADet1ById"); }
        }
    }
}

