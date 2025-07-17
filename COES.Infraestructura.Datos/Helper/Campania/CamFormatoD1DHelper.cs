using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamFormatoD1DHelper : HelperBase
    {
        public CamFormatoD1DHelper() : base(Consultas.CamFormatoD1DSql) { }


        public string SqlGetFormatoD1DCodi
        {
            get { return base.GetSqlXml("GetFormatoD1DCodi"); }
        }

        public string SqlSaveFormatoD1D
        {
            get { return base.GetSqlXml("SaveFormatoD1D"); }
        }

        public string SqlUpdateFormatoD1D
        {
            get { return base.GetSqlXml("UpdateFormatoD1D"); }
        }

        public string SqlGetLastFormatoD1DId
        {
            get { return base.GetSqlXml("GetLastFormatoD1DId"); }
        }

        public string SqlDeleteFormatoD1DById
        {
            get { return base.GetSqlXml("DeleteFormatoD1DById"); }
        }

        public string SqlGetFormatoD1DById
        {
            get { return base.GetSqlXml("GetFormatoD1DById"); }
        }

    }
}