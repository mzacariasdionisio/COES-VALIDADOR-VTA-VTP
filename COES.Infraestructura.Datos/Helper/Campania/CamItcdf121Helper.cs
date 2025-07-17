using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf121Helper : HelperBase
    {
        public CamItcdf121Helper() : base(Consultas.CamItcdf121Sql) { }


        public string SqlGetItcdf121Codi
        {
            get { return base.GetSqlXml("GetItcdf121Codi"); }
        }

        public string SqlSaveItcdf121
        {
            get { return base.GetSqlXml("SaveItcdf121"); }
        }

        public string SqlUpdateItcdf121
        {
            get { return base.GetSqlXml("UpdateItcdf121"); }
        }

        public string SqlGetLastItcdf121Id
        {
            get { return base.GetSqlXml("GetLastItcdf121Id"); }
        }

        public string SqlDeleteItcdf121ById
        {
            get { return base.GetSqlXml("DeleteItcdf121ById"); }
        }

        public string SqlGetItcdf121ById
        {
            get { return base.GetSqlXml("GetItcdf121ById"); }
        }

    }
}
