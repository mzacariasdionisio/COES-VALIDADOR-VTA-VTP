using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1AHelper : HelperBase
    {
        public CamFormatoD1AHelper() : base(Consultas.CamFormatoD1ASql) { }


        public string SqlGetFormatoD1ACodi
        {
            get { return base.GetSqlXml("GetFormatoD1ACodi"); }
        }

        public string SqlSaveFormatoD1A
        {
            get { return base.GetSqlXml("SaveFormatoD1A"); }
        }

        public string SqlUpdateFormatoD1A
        {
            get { return base.GetSqlXml("UpdateFormatoD1A"); }
        }

        public string SqlGetLastFormatoD1AId
        {
            get { return base.GetSqlXml("GetLastFormatoD1AId"); }
        }

        public string SqlDeleteFormatoD1AById
        {
            get { return base.GetSqlXml("DeleteFormatoD1AById"); }
        }

        public string SqlGetFormatoD1AById
        {
            get { return base.GetSqlXml("GetFormatoD1AById"); }
        }
    }
}
