using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class ExtRatioHelper: HelperBase
    {
        public ExtRatioHelper() : base(Consultas.ExtRatioSql)
        {
        }
        public string ERATCODI = "ERATCODI";
        public string EARCODI = "EARCODI";
        public string EAICODI = "EAICODI";
        public string ERATTOTINF = "ERATTOTINF";
        public string ERATENVINF = "ERATENVINF";
        public string ERATRATIO = "ERATRATIO";
        public string LASTDATE = "LASTDATE";

        public string SqlGetMaxCodi
        {
            get { return base.GetSqlXml("GetMaxCodi"); }
        }

    }
}
