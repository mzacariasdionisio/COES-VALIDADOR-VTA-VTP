using System;
using COES.Base.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaEolBHelper : HelperBase
    {
        public CamRegHojaEolBHelper() : base(Consultas.CamRegHojaEolBSql) { }


        public string SqlGetRegHojaEolBCodi
        {
            get { return base.GetSqlXml("GetRegHojaEolBCodi"); }
        }

        public string SqlSaveRegHojaEolB
        {
            get { return base.GetSqlXml("SaveRegHojaEolB"); }
        }

        public string SqlUpdateRegHojaEolB
        {
            get { return base.GetSqlXml("UpdateRegHojaEolB"); }
        }

        public string SqlGetLastRegHojaEolBId
        {
            get { return base.GetSqlXml("GetLastRegHojaEolBId"); }
        }

        public string SqlDeleteRegHojaEolBById
        {
            get { return base.GetSqlXml("DeleteRegHojaEolBById"); }
        }

        public string SqlGetRegHojaEolBById
        {
            get { return base.GetSqlXml("GetRegHojaEolBById"); }
        }
    }
}
