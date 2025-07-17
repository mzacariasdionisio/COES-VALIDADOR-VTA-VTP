using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf110Helper : HelperBase
    {
        public CamItcdf110Helper() : base(Consultas.CamItcdf110Sql) { }


        public string SqlGetItcdf110Codi
        {
            get { return base.GetSqlXml("GetItcdf110Codi"); }
        }

        public string SqlSaveItcdf110
        {
            get { return base.GetSqlXml("SaveItcdf110"); }
        }

        public string SqlUpdateItcdf110
        {
            get { return base.GetSqlXml("UpdateItcdf110"); }
        }

        public string SqlGetLastItcdf110Id
        {
            get { return base.GetSqlXml("GetLastItcdf110Id"); }
        }

        public string SqlDeleteItcdf110ById
        {
            get { return base.GetSqlXml("DeleteItcdf110ById"); }
        }

        public string SqlGetItcdf110ById
        {
            get { return base.GetSqlXml("GetItcdf110ById"); }
        }

    }
}
