using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1ADet5Helper : HelperBase
    {
        public CamFormatoD1ADet5Helper() : base(Consultas.CamFormatoD1ADet5Sql) { }


        public string SqlGetFormatoD1ADet5Codi
        {
            get { return base.GetSqlXml("GetFormatoD1ADet5Codi"); }
        }

        public string SqlSaveFormatoD1ADet5
        {
            get { return base.GetSqlXml("SaveFormatoD1ADet5"); }
        }

        public string SqlUpdateFormatoD1ADet5
        {
            get { return base.GetSqlXml("UpdateFormatoD1ADet5"); }
        }

        public string SqlGetLastFormatoD1ADet5Id
        {
            get { return base.GetSqlXml("GetLastFormatoD1ADet5Id"); }
        }

        public string SqlDeleteFormatoD1ADet5ById
        {
            get { return base.GetSqlXml("DeleteFormatoD1ADet5ById"); }
        }

        public string SqlGetFormatoD1ADet5ById
        {
            get { return base.GetSqlXml("GetFormatoD1ADet5ById"); }
        }
    }
}
