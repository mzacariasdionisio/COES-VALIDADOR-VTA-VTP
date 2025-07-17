using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdfp011DetHelper : HelperBase
    {
        public CamItcdfp011DetHelper() : base(Consultas.CamItcdfp011DetSql) { }


        public string SqlGetItcdfp011DetCodi
        {
            get { return base.GetSqlXml("GetItcdfp011DetCodi"); }
        }

        public string SqlSaveItcdfp011Det
        {
            get { return base.GetSqlXml("SaveItcdfp011Det"); }
        }

        public string SqlUpdateItcdfp011Det
        {
            get { return base.GetSqlXml("UpdateItcdfp011Det"); }
        }

        public string SqlGetLastItcdfp011DetId
        {
            get { return base.GetSqlXml("GetLastItcdfp011DetId"); }
        }

        public string SqlDeleteItcdfp011DetById
        {
            get { return base.GetSqlXml("DeleteItcdfp011DetById"); }
        }

        public string SqlGetItcdfp011DetById
        {
            get { return base.GetSqlXml("GetItcdfp011DetById"); }
        }

        public string SqlGetItcdfp011DetByIdPag
        {
            get { return base.GetSqlXml("GetItcdfp011DetByIdPag"); }
        }

        public string SqlGetCloneItcdfp011DetById
        {
            get { return base.GetSqlXml("GetCloneItcdfp011DetById"); }
        }

    }
}
