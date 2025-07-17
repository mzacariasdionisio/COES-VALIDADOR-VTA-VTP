using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSolHojaCHelper: HelperBase
    {
        public CamSolHojaCHelper() : base(Consultas.CamSolHojaCSql) { }


        public string SqlGetSolHojaCProyCodi
        {
            get { return base.GetSqlXml("GetSolHojaCProyCodi"); }
        }

        public string SqlSaveSolHojaC
        {
            get { return base.GetSqlXml("SaveSolHojaC"); }
        }

        public string SqlUpdateSolHojaC
        {
            get { return base.GetSqlXml("UpdateSolHojaC"); }
        }

        public string SqlGetLastSolHojaCId
        {
            get { return base.GetSqlXml("GetLastSolHojaCId"); }
        }

        public string SqlDeleteSolHojaCById
        {
            get { return base.GetSqlXml("DeleteSolHojaCById"); }
        }

        public string SqlGetSolHojaCById
        {
            get { return base.GetSqlXml("GetSolHojaCById"); }
        }

    }
}
