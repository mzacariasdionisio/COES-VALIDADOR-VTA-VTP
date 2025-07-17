using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdfp012Helper : HelperBase
    {
        public CamItcdfp012Helper() : base(Consultas.CamItcdfp012Sql) { }


        public string SqlGetItcdfp012Codi
        {
            get { return base.GetSqlXml("GetItcdfp012Codi"); }
        }

        public string SqlSaveItcdfp012
        {
            get { return base.GetSqlXml("SaveItcdfp012"); }
        }

        public string SqlUpdateItcdfp012
        {
            get { return base.GetSqlXml("UpdateItcdfp012"); }
        }

        public string SqlGetLastItcdfp012Id
        {
            get { return base.GetSqlXml("GetLastItcdfp012Id"); }
        }

        public string SqlDeleteItcdfp012ById
        {
            get { return base.GetSqlXml("DeleteItcdfp012ById"); }
        }

        public string SqlGetItcdfp012ById
        {
            get { return base.GetSqlXml("GetItcdfp012ById"); }
        }

    }
}
