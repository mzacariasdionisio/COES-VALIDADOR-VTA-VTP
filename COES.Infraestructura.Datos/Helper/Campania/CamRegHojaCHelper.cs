using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaCHelper : HelperBase
    {
        public CamRegHojaCHelper(): base(Consultas.CamRegHojaCSql) { }


        public string SqlGetRegHojaCProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaCProyCodi"); }
        }

        public string SqlSaveRegHojaC
        {
            get { return base.GetSqlXml("SaveRegHojaC"); }
        }

        public string SqlUpdateRegHojaC
        {
            get { return base.GetSqlXml("UpdateRegHojaC"); }
        }

        public string SqlGetLastRegHojaCId
        {
            get { return base.GetSqlXml("GetLastRegHojaCId"); }
        }

        public string SqlDeleteRegHojaCById
        {
            get { return base.GetSqlXml("DeleteRegHojaCById"); }
        }

        public string SqlGetRegHojaCById
        {
            get { return base.GetSqlXml("GetRegHojaCById"); }
        }


    }
}
