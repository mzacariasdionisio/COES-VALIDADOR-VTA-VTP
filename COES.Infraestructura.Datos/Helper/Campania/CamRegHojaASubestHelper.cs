 using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaASubestHelper : HelperBase
    {
        public CamRegHojaASubestHelper(): base(Consultas.CamRegHojaASubestSql) { }


        public string SqlGetRegHojaASubestProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaASubestProyCodi"); }
        }

        public string SqlSaveRegHojaASubest
        {
            get { return base.GetSqlXml("SaveRegHojaASubest"); }
        }

        public string SqlUpdateRegHojaASubest
        {
            get { return base.GetSqlXml("UpdateRegHojaASubest"); }
        }

        public string SqlGetLastRegHojaASubestId
        {
            get { return base.GetSqlXml("GetLastRegHojaASubestId"); }
        }

        public string SqlDeleteRegHojaASubestById
        {
            get { return base.GetSqlXml("DeleteRegHojaASubestById"); }
        }

        public string SqlGetRegHojaASubestById
        {
            get { return base.GetSqlXml("GetRegHojaASubestById"); }
        }


    }
}
