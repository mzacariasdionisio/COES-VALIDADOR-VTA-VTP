using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf116Helper : HelperBase
    {
        public CamItcdf116Helper() : base(Consultas.CamItcdf116Sql) { }


        public string SqlGetItcdf116Codi
        {
            get { return base.GetSqlXml("GetItcdf116Codi"); }
        }

        public string SqlSaveItcdf116
        {
            get { return base.GetSqlXml("SaveItcdf116"); }
        }

        public string SqlUpdateItcdf116
        {
            get { return base.GetSqlXml("UpdateItcdf116"); }
        }

        public string SqlGetLastItcdf116Id
        {
            get { return base.GetSqlXml("GetLastItcdf116Id"); }
        }

        public string SqlDeleteItcdf116ById
        {
            get { return base.GetSqlXml("DeleteItcdf116ById"); }
        }

        public string SqlGetItcdf116ById
        {
            get { return base.GetSqlXml("GetItcdf116ById"); }
        }

    }
}
