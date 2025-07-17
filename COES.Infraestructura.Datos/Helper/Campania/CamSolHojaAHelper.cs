using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamSolHojaAHelper : HelperBase
    {
        public CamSolHojaAHelper() : base(Consultas.CamSolHojaASql) { }


        public string SqlGetSolHojaAProyCodi
        {
            get { return base.GetSqlXml("GetSolHojaAProyCodi"); }
        }

        public string SqlSaveSolHojaA
        {
            get { return base.GetSqlXml("SaveSolHojaA"); }
        }

        public string SqlUpdateSolHojaA
        {
            get { return base.GetSqlXml("UpdateSolHojaA"); }
        }

        public string SqlGetLastSolHojaAId
        {
            get { return base.GetSqlXml("GetLastSolHojaAId"); }
        }

        public string SqlDeleteSolHojaAById
        {
            get { return base.GetSqlXml("DeleteSolHojaAById"); }
        }

        public string SqlGetSolHojaAById
        {
            get { return base.GetSqlXml("GetSolHojaAById"); }
        }

    }
    }
