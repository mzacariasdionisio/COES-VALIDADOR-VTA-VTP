using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetRegHojaCHelper : HelperBase
    {
        public CamDetRegHojaCHelper(): base(Consultas.CamDetRegHojaCSql) { }


        public string SqlGetDetRegHojaCFichaCCodi
        {
            get { return base.GetSqlXml("GetDetRegHojaCFichaCCodi"); }
        }

        public string SqlSaveDetRegHojaC
        {
            get { return base.GetSqlXml("SaveDetRegHojaC"); }
        }

        public string SqlUpdateDetRegHojaC
        {
            get { return base.GetSqlXml("UpdateDetRegHojaC"); }
        }

        public string SqlGetLastDetRegHojaCId
        {
            get { return base.GetSqlXml("GetLastDetRegHojaCId"); }
        }

        public string SqlDeleteDetRegHojaCById
        {
            get { return base.GetSqlXml("DeleteDetRegHojaCById"); }
        }

        public string SqlGetDetRegHojaCById
        {
            get { return base.GetSqlXml("GetDetRegHojaCById"); }
        }


    }
}
