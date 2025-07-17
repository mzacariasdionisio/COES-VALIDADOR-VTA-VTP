using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class VtpAuditoriaProcesoHelper : HelperBase
    {
        public VtpAuditoriaProcesoHelper()
            : base(Consultas.VtpAuditoriaProcesoSql)
        {
        }

        #region Mapeo de Campos
      
        public string Audprocodi = "AUDPROCODI";
        public string Tipprocodi = "TIPPROCODI";
        public string Estdcodi = "ESTDCODI";
        public string Tipprodescripcion = "TIPPRODESCRIPCION";
        public string Estddescripcion = "ESTDDESCRIPCION";
        public string Audproproceso = "AUDPROPROCESO";
        public string Audprodescripcion = "AUDPRODESCRIPCION";
        public string Audprousucreacion = "AUDPROUSUCREACION";
        public string Audprofeccreacion = "AUDPROFECCREACION";
        public string Audprousumodificacion = "AUDPROUSUMODIFICACION";
        public string Audprofecmodificacion = "AUDPROFECMODIFICACION";
        public string NroPagina = "nropagina";
        public string PageSize = "pagesize";

        public string SqlListAuditoriaProcesoByFiltro
        {
            get { return base.GetSqlXml("ListAuditoriaProcesoByFiltro"); }
        }

        public string SqlNroRegistrosAuditoriaProceso
        {
            get { return base.GetSqlXml("NroRegistroAuditoriaProcesoByFiltro"); }
        }




        #endregion

    }
}
