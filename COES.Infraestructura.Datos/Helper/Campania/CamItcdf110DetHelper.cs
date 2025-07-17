using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf110DetHelper : HelperBase
    {
        public CamItcdf110DetHelper() : base(Consultas.CamItcdf110DetSql) { }


        public string SqlGetItcdf110DetCodi
        {
            get { return base.GetSqlXml("GetItcdf110DetCodi"); }
        }

        public string SqlSaveItcdf110Det
        {
            get { return base.GetSqlXml("SaveItcdf110Det"); }
        }

        public string SqlUpdateItcdf110Det
        {
            get { return base.GetSqlXml("UpdateItcdf110Det"); }
        }

        public string SqlGetLastItcdf110DetId
        {
            get { return base.GetSqlXml("GetLastItcdf110DetId"); }
        }

        public string SqlDeleteItcdf110DetById
        {
            get { return base.GetSqlXml("DeleteItcdf110DetById"); }
        }

        public string SqlGetItcdf110DetById
        {
            get { return base.GetSqlXml("GetItdf110DetById"); }
        }

    }
}
