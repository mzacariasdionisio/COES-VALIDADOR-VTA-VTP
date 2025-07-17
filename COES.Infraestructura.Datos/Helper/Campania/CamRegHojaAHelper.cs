using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaAHelper : HelperBase
    {
        public CamRegHojaAHelper(): base(Consultas.CamRegHojaASql) { }


        public string SqlGetRegHojaAProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaAProyCodi"); }
        }

        public string SqlSaveRegHojaA
        {
            get { return base.GetSqlXml("SaveRegHojaA"); }
        }

        public string SqlUpdateRegHojaA
        {
            get { return base.GetSqlXml("UpdateRegHojaA"); }
        }

        public string SqlGetLastRegHojaAId
        {
            get { return base.GetSqlXml("GetLastRegHojaAId"); }
        }

        public string SqlDeleteRegHojaAById
        {
            get { return base.GetSqlXml("DeleteRegHojaAById"); }
        }

        public string SqlGetRegHojaAById
        {
            get { return base.GetSqlXml("GetRegHojaAById"); }
        }


    }
}
