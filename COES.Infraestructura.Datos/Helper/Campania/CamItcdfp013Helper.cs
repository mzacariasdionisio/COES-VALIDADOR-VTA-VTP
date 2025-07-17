using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdfp013Helper : HelperBase
    {
        public CamItcdfp013Helper() : base(Consultas.CamItcdfp013Sql) { }


        public string SqlGetItcdfp013Codi
        {
            get { return base.GetSqlXml("GetItcdfp013Codi"); }
        }

        public string SqlSaveItcdfp013
        {
            get { return base.GetSqlXml("SaveItcdfp013"); }
        }

        public string SqlUpdateItcdfp013
        {
            get { return base.GetSqlXml("UpdateItcdfp013"); }
        }

        public string SqlGetLastItcdfp013Id
        {
            get { return base.GetSqlXml("GetLastItcdfp013Id"); }
        }

        public string SqlDeleteItcdfp013ById
        {
            get { return base.GetSqlXml("DeleteItcdfp013ById"); }
        }

        public string SqlGetItcdfp013ById
        {
            get { return base.GetSqlXml("GetItcdfp013ById"); }
        }

    }
}
