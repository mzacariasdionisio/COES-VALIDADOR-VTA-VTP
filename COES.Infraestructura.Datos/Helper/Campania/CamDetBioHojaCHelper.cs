using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetBioHojaCHelper : HelperBase
    {
        public CamDetBioHojaCHelper() : base(Consultas.CamDetBioHojaCSql) { }


        public string SqlGetDetBioHojaCCodi
        {
            get { return base.GetSqlXml("GetDetBioHojaCCodi"); }
        }

        public string SqlSaveDetBioHojaC
        {
            get { return base.GetSqlXml("SaveDetBioHojaC"); }
        }

        public string SqlUpdateDetBioHojaC
        {
            get { return base.GetSqlXml("UpdateDetBioHojaC"); }
        }

        public string SqlGetLastDetBioHojaCId
        {
            get { return base.GetSqlXml("GetLastDetBioHojaCId"); }
        }

        public string SqlDeleteDetBioHojaCById
        {
            get { return base.GetSqlXml("DeleteDetBioHojaCById"); }
        }

        public string SqlGetDetBioHojaCById
        {
            get { return base.GetSqlXml("GetDetBioHojaCById"); }
        }

    }
}
 

