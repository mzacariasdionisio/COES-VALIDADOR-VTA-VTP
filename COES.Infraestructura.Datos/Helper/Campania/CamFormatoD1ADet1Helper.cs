using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1ADet1Helper : HelperBase
    {
        public CamFormatoD1ADet1Helper() : base(Consultas.CamFormatoD1ADet1Sql) { }


        public string SqlGetFormatoD1ADet1Codi
        {
            get { return base.GetSqlXml("GetFormatoD1ADet1ByCodi"); }
        }

        public string SqlSaveFormatoD1ADet1
        {
            get { return base.GetSqlXml("SaveFormatoD1ADet1"); }
        }

        public string SqlUpdateFormatoD1ADet1
        {
            get { return base.GetSqlXml("UpdateFormatoD1ADet1"); }
        }

        public string SqlGetLastFormatoD1ADet1Id
        {
            get { return base.GetSqlXml("GetLastFormatoD1ADet1Id"); }
        }

        public string SqlDeleteFormatoD1ADet1ById
        {
            get { return base.GetSqlXml("DeleteFormatoD1ADet1ById"); }
        }

        public string SqlGetFormatoD1ADet1ById
        {
            get { return base.GetSqlXml("GetFormatoD1ADet1ById"); }
        }
    }
}