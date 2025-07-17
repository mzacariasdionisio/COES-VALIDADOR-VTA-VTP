using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamBioHojaCHelper : HelperBase
    {

        public CamBioHojaCHelper() : base(Consultas.CamBioHojaCSql) { }


        public string SqlGetBioHojaCProyCodi
        {
            get { return base.GetSqlXml("GetBioHojaCProyCodi"); }
        }

        public string SqlSaveBioHojaC
        {
            get { return base.GetSqlXml("SaveBioHojaC"); }
        }

        public string SqlUpdateBioHojaC
        {
            get { return base.GetSqlXml("UpdateBioHojaC"); }
        }

        public string SqlGetLastBioHojaCId
        {
            get { return base.GetSqlXml("GetLastBioHojaCId"); }
        }

        public string SqlDeleteBioHojaCById
        {
            get { return base.GetSqlXml("DeleteBioHojaCById"); }
        }

        public string SqlGetBioHojaCById
        {
            get { return base.GetSqlXml("GetBioHojaCById"); }
        }
    }

}
