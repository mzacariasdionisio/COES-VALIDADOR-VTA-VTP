using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class VtpTipoAplicacionHelper : HelperBase
    {
        public VtpTipoAplicacionHelper()
            : base(Consultas.VtpTipoAplicacionSql)
        {
        }

        #region Mapeo de Campos
        
            
        public string Tipaplinombre = "TIPAPLINOMBRE";
        public string Tipaplicodi = "TIPAPLICODI";

        public string SqlListTipoAplicacion
        {
            get { return base.GetSqlXml("ListTipoAplicacion"); }
        }
        #endregion

    }
}
