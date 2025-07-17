using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1ADet3Helper : HelperBase
    {
        public CamFormatoD1ADet3Helper() : base(Consultas.CamFormatoD1ADet3Sql) { }


        public string SqlGetFormatoD1ADet3Codi
        {
            get { return base.GetSqlXml("GetFormatoD1ADet3ByCodi"); }
        }

        public string SqlSaveFormatoD1ADet3
        {
            get { return base.GetSqlXml("SaveFormatoD1ADet3"); }
        }

        public string SqlUpdateFormatoD1ADet3
        {
            get { return base.GetSqlXml("UpdateFormatoD1ADet3"); }
        }

        public string SqlGetLastFormatoD1ADet3Id
        {
            get { return base.GetSqlXml("GetLastFormatoD1ADet3Id"); }
        }

        public string SqlDeleteFormatoD1ADet3ById
        {
            get { return base.GetSqlXml("DeleteFormatoD1ADet3ById"); }
        }

        public string SqlGetFormatoD1ADet3ById
        {
            get { return base.GetSqlXml("GetFormatoD1ADet3ById"); }
        }
    }

}
