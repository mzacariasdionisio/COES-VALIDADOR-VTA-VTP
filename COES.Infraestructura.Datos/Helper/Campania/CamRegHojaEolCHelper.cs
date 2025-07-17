using System;
using COES.Base.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaEolCHelper : HelperBase 
    {
        public CamRegHojaEolCHelper() : base(Consultas.CamRegHojaEolCSql) { }


        public string SqlGetRegHojaEolCCodi
        {
            get { return base.GetSqlXml("GetRegHojaEolCCodi"); }
        }

        public string SqlSaveRegHojaEolC
        {
            get { return base.GetSqlXml("SaveRegHojaEolC"); }
        }

        public string SqlUpdateRegHojaEolC
        {
            get { return base.GetSqlXml("UpdateRegHojaEolC"); }
        }

        public string SqlGetLastRegHojaEolCId
        {
            get { return base.GetSqlXml("GetLastRegHojaEolCId"); }
        }

        public string SqlDeleteRegHojaEolCById
        {
            get { return base.GetSqlXml("DeleteRegHojaEolCById"); }
        }

        public string SqlGetRegHojaEolCById
        {
            get { return base.GetSqlXml("GetRegHojaEolCById"); }
        }
    }
}
