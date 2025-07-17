using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamBioHojaBHelper : HelperBase
    {

        public CamBioHojaBHelper() : base(Consultas.CamBioHojaBSql) { }


        public string SqlGetBioHojaBProyCodi
        {
            get { return base.GetSqlXml("GetBioHojaBProyCodi"); }
        }

        public string SqlSaveBioHojaB
        {
            get { return base.GetSqlXml("SaveBioHojaB"); }
        }

        public string SqlUpdateBioHojaB
        {
            get { return base.GetSqlXml("UpdateBioHojaB"); }
        }

        public string SqlGetLastBioHojaBId
        {
            get { return base.GetSqlXml("GetLastBioHojaBId"); }
        }

        public string SqlDeleteBioHojaBById
        {
            get { return base.GetSqlXml("DeleteBioHojaBById"); }
        }

        public string SqlGetBioHojaBById
        {
            get { return base.GetSqlXml("GetBioHojaBById"); }
        }
    }
}
