using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class VtpTipoProcesoHelper : HelperBase
    {
        public VtpTipoProcesoHelper()
            : base(Consultas.VtpTipoProcesoSql)
        {
        }

        #region Mapeo de Campos

        public string Tipprocodi = "TIPPROCODI";
        public string Tipprodescripcion = "TIPPRODESCRIPCION";


        public string SqlListTipoProceso
        {
            get { return base.GetSqlXml("ListTipoProceso"); }
        }
        #endregion

    }
}
