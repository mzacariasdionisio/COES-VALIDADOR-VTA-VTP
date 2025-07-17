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
    /// Clase que contiene el mapeo de la tabla VTP_PEAJE_CARGO_AJUSTE
    /// </summary>
    public class VtpPeajeCargoAjusteHelper : HelperBase
    {
        public VtpPeajeCargoAjusteHelper() : base(Consultas.VtpPeajeCargoAjusteSql)
        {
        }

        public VtpPeajeCargoAjusteDTO Create(IDataReader dr)
        {
            VtpPeajeCargoAjusteDTO entity = new VtpPeajeCargoAjusteDTO();

            int iPecajcodi = dr.GetOrdinal(this.Pecajcodi);
            if (!dr.IsDBNull(iPecajcodi)) entity.Pecajcodi = Convert.ToInt32(dr.GetValue(iPecajcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iPecajajuste = dr.GetOrdinal(this.Pecajajuste);
            if (!dr.IsDBNull(iPecajajuste)) entity.Pecajajuste = dr.GetDecimal(iPecajajuste);

            int iPecajusucreacion = dr.GetOrdinal(this.Pecajusucreacion);
            if (!dr.IsDBNull(iPecajusucreacion)) entity.Pecajusucreacion = dr.GetString(iPecajusucreacion);

            int iPecajfeccreacion = dr.GetOrdinal(this.Pecajfeccreacion);
            if (!dr.IsDBNull(iPecajfeccreacion)) entity.Pecajfeccreacion = dr.GetDateTime(iPecajfeccreacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Pecajcodi = "PECAJCODI";
        public string Pericodi = "PERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Pingcodi = "PINGCODI";
        public string Pecajajuste = "PECAJAJUSTE";
        public string Pecajusucreacion = "PECAJUSUCREACION";
        public string Pecajfeccreacion = "PECAJFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";
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
