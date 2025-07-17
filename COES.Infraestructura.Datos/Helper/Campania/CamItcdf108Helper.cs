using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf108Helper : HelperBase
    {
        public CamItcdf108Helper() : base(Consultas.CamItcdf108Sql) { }


        public string SqlGetItcdf108Codi
        {
            get { return base.GetSqlXml("GetItcdf108Codi"); }
        }

        public string SqlSaveItcdf108
        {
            get { return base.GetSqlXml("SaveItcdf108"); }
        }

        public string SqlUpdateItcdf108
        {
            get { return base.GetSqlXml("UpdateItcdf108"); }
        }

        public string SqlGetLastItcdf108Id
        {
            get { return base.GetSqlXml("GetLastItcdf108Id"); }
        }

        public string SqlDeleteItcdf108ById
        {
            get { return base.GetSqlXml("DeleteItcdf108ById"); }
        }

        public string SqlGetItcdf108ById
        {
            get { return base.GetSqlXml("GetItcdf108ById"); }
        }

    }
}
