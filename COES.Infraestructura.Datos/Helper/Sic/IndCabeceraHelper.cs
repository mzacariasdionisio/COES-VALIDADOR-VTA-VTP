using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class IndCabeceraHelper : HelperBase
    {
        public IndCabeceraHelper() : base(Consultas.IndCabeceraSql)
        {
        }

        #region Mapeo de Campos
        public string Indcbrcodi = "INDCBRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ipericodi = "IPERICODI";
        public string Indcbrtipo = "INDCBRTIPO";
        public string Indcbrusucreacion = "INDCBRUSUCREACION";
        public string Indcbrfeccreacion = "INDCBRFECCREACION";
        #endregion

        #region Consultas
        public string SqlListCabecera
        {
            get { return base.GetSqlXml("ListCabecera"); }
        }
        #endregion
    }
}
