using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1ADet2Helper : HelperBase
    {
        public CamFormatoD1ADet2Helper() : base(Consultas.CamFormatoD1ADet2Sql) { }


        public string SqlGetFormatoD1ADet2Codi
        {
            get { return base.GetSqlXml("GetFormatoD1ADet2ByACodi"); }
        }

        public string SqlSaveFormatoD1ADet2
        {
            get { return base.GetSqlXml("SaveFormatoD1ADet2"); }
        }

        public string SqlUpdateFormatoD1ADet2
        {
            get { return base.GetSqlXml("UpdateFormatoD1ADet2"); }
        }

        public string SqlGetLastFormatoD1ADet2Id
        {
            get { return base.GetSqlXml("GetLastFormatoD1ADet2Id"); }
        }

        public string SqlDeleteFormatoD1ADet2ById
        {
            get { return base.GetSqlXml("DeleteFormatoD1ADet2ById"); }
        }

        public string SqlGetFormatoD1ADet2ById
        {
            get { return base.GetSqlXml("GetFormatoD1ADet2ById"); }
        }
    }
}