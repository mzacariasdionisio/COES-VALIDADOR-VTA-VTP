using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1BDetHelper : HelperBase
    {
        public CamFormatoD1BDetHelper() : base(Consultas.CamFormatoD1BDetSql) { }


        public string SqlGetFormatoD1BDetCodi
        {
            get { return base.GetSqlXml("GetFormatoD1BDetCodi"); }
        }

        public string SqlSaveFormatoD1BDet
        {
            get { return base.GetSqlXml("SaveFormatoD1BDet"); }
        }

        public string SqlUpdateFormatoD1BDet
        {
            get { return base.GetSqlXml("UpdateFormatoD1BDet"); }
        }

        public string SqlGetLastFormatoD1BDetId
        {
            get { return base.GetSqlXml("GetLastFormatoD1BDetId"); }
        }

        public string SqlDeleteFormatoD1BDetById
        {
            get { return base.GetSqlXml("DeleteFormatoD1BDetById"); }
        }

        public string SqlGetFormatoD1BDetById
        {
            get { return base.GetSqlXml("GetFormatoD1BDetById"); }
        }
    }
}