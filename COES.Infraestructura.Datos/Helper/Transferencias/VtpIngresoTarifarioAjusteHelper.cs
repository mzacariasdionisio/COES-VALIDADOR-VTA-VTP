using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
    /// </summary>
    public class VtpIngresoTarifarioAjusteHelper : HelperBase
    {
        public VtpIngresoTarifarioAjusteHelper() : base(Consultas.VtpIngresoTarifarioAjusteSql)
        {
        }

        public VtpIngresoTarifarioAjusteDTO Create(IDataReader dr)
        {
            VtpIngresoTarifarioAjusteDTO entity = new VtpIngresoTarifarioAjusteDTO();

            int iIngtajcodi = dr.GetOrdinal(this.Ingtajcodi);
            if (!dr.IsDBNull(iIngtajcodi)) entity.Ingtajcodi = Convert.ToInt32(dr.GetValue(iIngtajcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iEmprcodiping = dr.GetOrdinal(this.Emprcodiping);
            if (!dr.IsDBNull(iEmprcodiping)) entity.Emprcodiping = Convert.ToInt32(dr.GetValue(iEmprcodiping));

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iEmprcodingpot = dr.GetOrdinal(this.Emprcodingpot);
            if (!dr.IsDBNull(iEmprcodingpot)) entity.Emprcodingpot = Convert.ToInt32(dr.GetValue(iEmprcodingpot));

            int iIngtajajuste = dr.GetOrdinal(this.Ingtajajuste);
            if (!dr.IsDBNull(iIngtajajuste)) entity.Ingtajajuste = dr.GetDecimal(iIngtajajuste);

            int iIngtajusucreacion = dr.GetOrdinal(this.Ingtajusucreacion);
            if (!dr.IsDBNull(iIngtajusucreacion)) entity.Ingtajusucreacion = dr.GetString(iIngtajusucreacion);

            int iIngtajfeccreacion = dr.GetOrdinal(this.Ingtajfeccreacion);
            if (!dr.IsDBNull(iIngtajfeccreacion)) entity.Ingtajfeccreacion = dr.GetDateTime(iIngtajfeccreacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Ingtajcodi = "INGTAJCODI";
        public string Pericodi = "PERICODI";
        public string Emprcodiping = "EMPRCODIPING";
        public string Pingcodi = "PINGCODI";
        public string Emprcodingpot = "EMPRCODINGPOT";
        public string Ingtajajuste = "INGTAJAJUSTE";
        public string Ingtajusucreacion = "INGTAJUSUCREACION";
        public string Ingtajfeccreacion = "INGTAJFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombping = "EMPRNOMBPING";
        public string Emprnombingpot = "EMPRNOMBINGPOT";
        public string Pingnombre = "PINGNOMBRE";

        #endregion

        public string SqlDeleteByCriteria 
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlGetAjuste
        {
            get { return base.GetSqlXml("GetAjuste"); }
        }
    }
}
