using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaBHelper : HelperBase
    {
        public CamRegHojaBHelper() : base(Consultas.CamRegHojaBSql) { }


        public string SqlGetRegHojaBProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaBProyCodi"); }
        }

        public string SqlSaveRegHojaB
        {
            get { return base.GetSqlXml("SaveRegHojaB"); }
        }

        public string SqlUpdateRegHojaB
        {
            get { return base.GetSqlXml("UpdateRegHojaB"); }
        }

        public string SqlGetLastRegHojaBId
        {
            get { return base.GetSqlXml("GetLastRegHojaBId"); }
        }

        public string SqlDeleteRegHojaBById
        {
            get { return base.GetSqlXml("DeleteRegHojaBById"); }
        }

        public string SqlGetRegHojaBById
        {
            get { return base.GetSqlXml("GetRegHojaBById"); }
        }


    }

}
