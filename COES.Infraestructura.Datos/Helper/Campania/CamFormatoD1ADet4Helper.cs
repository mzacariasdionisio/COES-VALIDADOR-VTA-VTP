using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1ADet4Helper : HelperBase
    {
        public CamFormatoD1ADet4Helper() : base(Consultas.CamFormatoD1ADet4Sql) { }


        public string SqlGetFormatoD1ADet4Codi
        {
            get { return base.GetSqlXml("GetFormatoD1ADet4ByCodi"); }
        }

        public string SqlSaveFormatoD1ADet4
        {
            get { return base.GetSqlXml("SaveFormatoD1ADet4"); }
        }

        public string SqlUpdateFormatoD1ADet4
        {
            get { return base.GetSqlXml("UpdateFormatoD1ADet4"); }
        }

        public string SqlGetLastFormatoD1ADet4Id
        {
            get { return base.GetSqlXml("GetLastFormatoD1ADet4Id"); }
        }

        public string SqlDeleteFormatoD1ADet4ById
        {
            get { return base.GetSqlXml("DeleteFormatoD1ADet4ById"); }
        }

        public string SqlGetFormatoD1ADet4ById
        {
            get { return base.GetSqlXml("GetFormatoD1ADet4ById"); }
        }
    }
}
