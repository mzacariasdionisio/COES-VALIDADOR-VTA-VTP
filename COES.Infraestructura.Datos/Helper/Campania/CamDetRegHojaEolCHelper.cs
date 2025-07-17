using System;
using COES.Base.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetRegHojaEolCHelper : HelperBase
    {
        public CamDetRegHojaEolCHelper() : base(Consultas.CamDetRegHojaEolCSql) { }


        public string SqlGetDetRegHojaEolCCodi
        {
            get { return base.GetSqlXml("GetDetRegHojaEolCCodi"); }
        }

        public string SqlSaveDetRegHojaEolC
        {
            get { return base.GetSqlXml("SaveDetRegHojaEolC"); }
        }

        public string SqlUpdateDetRegHojaEolC
        {
            get { return base.GetSqlXml("UpdateDetRegHojaEolC"); }
        }

        public string SqlGetLastDetRegHojaEolCId
        {
            get { return base.GetSqlXml("GetLastDetRegHojaEolCId"); }
        }

        public string SqlDeleteDetRegHojaEolCById
        {
            get { return base.GetSqlXml("DeleteDetRegHojaEolCById"); }
        }

        public string SqlGetDetRegHojaEolCById
        {
            get { return base.GetSqlXml("GetDetRegHojaEolCById"); }
        }

    }
}
