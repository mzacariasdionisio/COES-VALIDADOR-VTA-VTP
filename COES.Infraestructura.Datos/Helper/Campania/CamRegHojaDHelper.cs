using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaDHelper : HelperBase
    {
        public CamRegHojaDHelper() : base(Consultas.CamRegHojaDSql) { }


        public string SqlGetRegHojaDProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaDProyCodi"); }
        }

        public string SqlSaveRegHojaD
        {
            get { return base.GetSqlXml("SaveRegHojaD"); }
        }

        public string SqlUpdateRegHojaD
        {
            get { return base.GetSqlXml("UpdateRegHojaD"); }
        }

        public string SqlGetLastRegHojaDId
        {
            get { return base.GetSqlXml("GetLastRegHojaDId"); }
        }

        public string SqlDeleteRegHojaDById
        {
            get { return base.GetSqlXml("DeleteRegHojaDById"); }
        }
        public string SqlDeleteRegHojaDById2
        {
            get { return base.GetSqlXml("SqlDeleteRegHojaDById2"); }
        }

        public string SqlGetRegHojaDById
        {
            get { return base.GetSqlXml("GetRegHojaDById"); }
        }
    }
}
