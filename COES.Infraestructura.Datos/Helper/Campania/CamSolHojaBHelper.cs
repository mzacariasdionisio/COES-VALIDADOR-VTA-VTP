using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSolHojaBHelper : HelperBase
    {
        public CamSolHojaBHelper() : base(Consultas.CamSolHojaBSql) { }


        public string SqlGetSolHojaBProyCodi
        {
            get { return base.GetSqlXml("GetSolHojaBProyCodi"); }
        }

        public string SqlSaveSolHojaB
        {
            get { return base.GetSqlXml("SaveSolHojaB"); }
        }

        public string SqlUpdateSolHojaB
        {
            get { return base.GetSqlXml("UpdateSolHojaB"); }
        }

        public string SqlGetLastSolHojaBId
        {
            get { return base.GetSqlXml("GetLastSolHojaBId"); }
        }

        public string SqlDeleteSolHojaBById
        {
            get { return base.GetSqlXml("DeleteSolHojaBById"); }
        }

        public string SqlGetSolHojaBById
        {
            get { return base.GetSqlXml("GetSolHojaBById"); }
        }

    }
}
