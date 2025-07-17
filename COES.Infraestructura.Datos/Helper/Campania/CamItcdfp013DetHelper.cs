using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdfp013DetHelper : HelperBase
    {
        public CamItcdfp013DetHelper() : base(Consultas.CamItcdfp013DetSql) { }


        public string SqlGetItcdfp013DetCodi
        {
            get { return base.GetSqlXml("GetItcdfp013DetCodi"); }
        }

        public string SqlSaveItcdfp013Det
        {
            get { return base.GetSqlXml("SaveItcdfp013Det"); }
        }

        public string SqlUpdateItcdfp013Det
        {
            get { return base.GetSqlXml("UpdateItcdfp013Det"); }
        }

        public string SqlGetLastItcdfp013DetId
        {
            get { return base.GetSqlXml("GetLastItcdfp013DetId"); }
        }

        public string SqlDeleteItcdfp013DetById
        {
            get { return base.GetSqlXml("DeleteItcdfp013DetById"); }
        }

        public string SqlGetItcdfp013DetById
        {
            get { return base.GetSqlXml("GetItcdfp013DetById"); }
        }

    }
}
