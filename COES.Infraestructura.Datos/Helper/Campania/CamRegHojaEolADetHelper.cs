using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaEolADetHelper : HelperBase
    {
        public CamRegHojaEolADetHelper() : base(Consultas.CamRegHojaEolADetSql) { }


        public string SqlGetRegHojaEolADetCodi
        {
            get { return base.GetSqlXml("GetRegHojaEolADetCodi"); }
        }

        public string SqlSaveRegHojaEolADet
        {
            get { return base.GetSqlXml("SaveRegHojaEolADet"); }
        }

        public string SqlUpdateRegHojaEolADet
        {
            get { return base.GetSqlXml("UpdateRegHojaEolADet"); }
        }

        public string SqlGetLastRegHojaEolADetId
        {
            get { return base.GetSqlXml("GetLastRegHojaEolADetId"); }
        }

        public string SqlDeleteRegHojaEolADetById
        {
            get { return base.GetSqlXml("DeleteRegHojaEolADetById"); }
        }

        public string SqlGetRegHojaEolADetById
        {
            get { return base.GetSqlXml("GetRegHojaEolADetById"); }
        }
    }

}