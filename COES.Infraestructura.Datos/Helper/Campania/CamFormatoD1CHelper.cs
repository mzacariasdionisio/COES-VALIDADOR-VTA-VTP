using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1CHelper : HelperBase
    {
        public CamFormatoD1CHelper() : base(Consultas.CamFormatoD1CSql) { }


        public string SqlGetFormatoD1CCodi
        {
            get { return base.GetSqlXml("GetFormatoD1CCodi"); }
        }

        public string SqlSaveFormatoD1C
        {
            get { return base.GetSqlXml("SaveFormatoD1C"); }
        }

        public string SqlUpdateFormatoD1C
        {
            get { return base.GetSqlXml("UpdateFormatoD1C"); }
        }

        public string SqlGetLastFormatoD1CId
        {
            get { return base.GetSqlXml("GetLastFormatoD1CId"); }
        }

        public string SqlDeleteFormatoD1CById
        {
            get { return base.GetSqlXml("DeleteFormatoD1CById"); }
        }

        public string SqlGetFormatoD1CById
        {
            get { return base.GetSqlXml("GetFormatoD1CById"); }
        }

    }
}