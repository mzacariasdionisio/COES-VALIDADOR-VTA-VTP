using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdfp011Helper : HelperBase
    {
        public CamItcdfp011Helper() : base(Consultas.CamItcdfp011Sql) { }


        public string SqlGetItcdfp011Codi
        {
            get { return base.GetSqlXml("GetItcdfp011Codi"); }
        }

        public string SqlSaveItcdfp011
        {
            get { return base.GetSqlXml("SaveItcdfp011"); }
        }

        public string SqlUpdateItcdfp011
        {
            get { return base.GetSqlXml("UpdateItcdfp011"); }
        }

        public string SqlGetLastItcdfp011Id
        {
            get { return base.GetSqlXml("GetLastItcdfp011Id"); }
        }

        public string SqlDeleteItcdfp011ById
        {
            get { return base.GetSqlXml("DeleteItcdfp011ById"); }
        }

        public string SqlGetItcdfp011ById
        {
            get { return base.GetSqlXml("GetItcdfp011ById"); }
        }

    }
}
