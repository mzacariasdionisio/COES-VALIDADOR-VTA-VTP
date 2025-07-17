using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetRegHojaASubestHelper : HelperBase
    {
        public CamDetRegHojaASubestHelper(): base(Consultas.CamDetRegHojaASubestSql) { }


        public string SqlGetDetRegHojaASubestFichaCCodi
        {
            get { return base.GetSqlXml("GetDetRegHojaASubestFichaCCodi"); }
        }

        public string SqlSaveDetRegHojaASubest
        {
            get { return base.GetSqlXml("SaveDetRegHojaASubest"); }
        }

        public string SqlUpdateDetRegHojaASubest
        {
            get { return base.GetSqlXml("UpdateDetRegHojaASubest"); }
        }

        public string SqlGetLastDetRegHojaASubestId
        {
            get { return base.GetSqlXml("GetLastDetRegHojaASubestId"); }
        }

        public string SqlDeleteDetRegHojaASubestById
        {
            get { return base.GetSqlXml("DeleteDetRegHojaASubestById"); }
        }

        public string SqlGetDetRegHojaASubestById
        {
            get { return base.GetSqlXml("GetDetRegHojaASubestById"); }
        }


    }
}
