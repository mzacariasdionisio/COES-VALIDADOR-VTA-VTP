using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1BHelper : HelperBase
    {
        public CamFormatoD1BHelper() : base(Consultas.CamFormatoD1BSql) { }


        public string SqlGetFormatoD1BCodi
        {
            get { return base.GetSqlXml("GetFormatoD1BCodi"); }
        }

        public string SqlSaveFormatoD1B
        {
            get { return base.GetSqlXml("SaveFormatoD1B"); }
        }

        public string SqlUpdateFormatoD1B
        {
            get { return base.GetSqlXml("UpdateFormatoD1B"); }
        }

        public string SqlGetLastFormatoD1BId
        {
            get { return base.GetSqlXml("GetLastFormatoD1BId"); }
        }

        public string SqlDeleteFormatoD1BById
        {
            get { return base.GetSqlXml("DeleteFormatoD1BById"); }
        }

        public string SqlGetFormatoD1BById
        {
            get { return base.GetSqlXml("GetFormatoD1BById"); }
        }

    }
}