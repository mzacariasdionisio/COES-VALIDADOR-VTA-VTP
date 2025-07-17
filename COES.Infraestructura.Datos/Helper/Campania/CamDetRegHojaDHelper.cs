using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetRegHojaDHelper : HelperBase
    {

        public CamDetRegHojaDHelper(): base(Consultas.CamDetRegHojaDSql) { }

        public string SqlGetDetRegHojaDCodi
        {
            get { return base.GetSqlXml("GetDetRegHojaDCodi"); }
        }

        public string SqlSaveDetRegHojaD
        {
            get { return base.GetSqlXml("SaveDetRegHojaD"); }
        }

        public string SqlUpdateDetRegHojaD
        {
            get { return base.GetSqlXml("UpdateDetRegHojaD"); }
        }

        public string SqlGetLastDetRegHojaDId
        {
            get { return base.GetSqlXml("GetLastDetRegHojaDId"); }
        }

        public string SqlDeleteDetRegHojaDById
        {
            get { return base.GetSqlXml("DeleteDetRegHojaDById"); }
        }

        public string SqlGetDetRegHojaDById
        {
            get { return base.GetSqlXml("GetDetRegHojaDById"); }
        }
    }
}
