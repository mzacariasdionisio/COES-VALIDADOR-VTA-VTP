using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf104Helper : HelperBase
    {
        public CamItcdf104Helper() : base(Consultas.CamItcdf104Sql) { }


        public string SqlGetItcdf104Codi
        {
            get { return base.GetSqlXml("GetItcdf104Codi"); }
        }

        public string SqlSaveItcdf104
        {
            get { return base.GetSqlXml("SaveItcdf104"); }
        }

        public string SqlUpdateItcdf104
        {
            get { return base.GetSqlXml("UpdateItcdf104"); }
        }

        public string SqlGetLastItcdf104Id
        {
            get { return base.GetSqlXml("GetLastItcdf104Id"); }
        }

        public string SqlDeleteItcdf104ById
        {
            get { return base.GetSqlXml("DeleteItcdf104ById"); }
        }

        public string SqlGetItcdf104ById
        {
            get { return base.GetSqlXml("GetItcdf104ById"); }
        }

    }
}
