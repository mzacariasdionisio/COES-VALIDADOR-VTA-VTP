using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1CDetHelper : HelperBase
    {
        public CamFormatoD1CDetHelper() : base(Consultas.CamFormatoD1CDetSql) { }


        public string SqlGetFormatoD1CDetCodi
        {
            get { return base.GetSqlXml("GetFormatoD1CDetCodi"); }
        }

        public string SqlSaveFormatoD1CDet
        {
            get { return base.GetSqlXml("SaveFormatoD1CDet"); }
        }

        public string SqlUpdateFormatoD1CDet
        {
            get { return base.GetSqlXml("UpdateFormatoD1CDet"); }
        }

        public string SqlGetLastFormatoD1CDetId
        {
            get { return base.GetSqlXml("GetLastFormatoD1CDetId"); }
        }

        public string SqlDeleteFormatoD1CDetById
        {
            get { return base.GetSqlXml("DeleteFormatoD1CDetById"); }
        }

        public string SqlGetFormatoD1CDetById
        {
            get { return base.GetSqlXml("GetFormatoD1CDetById"); }
        }
    }
}