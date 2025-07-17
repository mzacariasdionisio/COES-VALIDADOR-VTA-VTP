using System;
using COES.Base.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaEolAHelper : HelperBase
    {
        public CamRegHojaEolAHelper() : base(Consultas.CamRegHojaEolASql) { }


        public string SqlGetRegHojaEolACodi
        {
            get { return base.GetSqlXml("GetRegHojaEolACodi"); }
        }

        public string SqlSaveRegHojaEolA
        {
            get { return base.GetSqlXml("SaveRegHojaEolA"); }
        }

        public string SqlUpdateRegHojaEolA
        {
            get { return base.GetSqlXml("UpdateRegHojaEolA"); }
        }

        public string SqlGetLastRegHojaEolAId
        {
            get { return base.GetSqlXml("GetLastRegHojaEolAId"); }
        }

        public string SqlDeleteRegHojaEolAById
        {
            get { return base.GetSqlXml("DeleteRegHojaEolAById"); }
        }

        public string SqlGetRegHojaEolAById
        {
            get { return base.GetSqlXml("GetRegHojaEolAById"); }
        }
    }
}
