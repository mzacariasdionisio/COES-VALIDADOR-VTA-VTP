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
    /// Clase que contiene el mapeo de la tabla VTP_REPA_RECA_PEAJE_DETALLE
    /// </summary>
    public class VtpRepaRecaPeajeDetalleHelper : HelperBase
    {
        public VtpRepaRecaPeajeDetalleHelper(): base(Consultas.VtpRepaRecaPeajeDetalleSql)
        {
        }

        public VtpRepaRecaPeajeDetalleDTO Create(IDataReader dr)
        {
            VtpRepaRecaPeajeDetalleDTO entity = new VtpRepaRecaPeajeDetalleDTO();
   

            int iRrpdcodi = dr.GetOrdinal(this.Rrpdcodi);
            if (!dr.IsDBNull(iRrpdcodi)) entity.Rrpdcodi = Convert.ToInt32(dr.GetValue(iRrpdcodi));

            int iRrpecodi = dr.GetOrdinal(this.Rrpecodi);
            if (!dr.IsDBNull(iRrpecodi)) entity.Rrpecodi = Convert.ToInt32(dr.GetValue(iRrpecodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            int iRrpdporcentaje = dr.GetOrdinal(this.Rrpdporcentaje);
            if (!dr.IsDBNull(iRrpdporcentaje)) entity.Rrpdporcentaje = dr.GetDecimal(iRrpdporcentaje);

            int iRrpdusucreacion = dr.GetOrdinal(this.Rrpdusucreacion);
            if (!dr.IsDBNull(iRrpdusucreacion)) entity.Rrpdusucreacion = dr.GetString(iRrpdusucreacion);

            int iRrpdfeccreacion = dr.GetOrdinal(this.Rrpdfeccreacion);
            if (!dr.IsDBNull(iRrpdfeccreacion)) entity.Rrpdfeccreacion = dr.GetDateTime(iRrpdfeccreacion);

            int iRrpdusumodificacion = dr.GetOrdinal(this.Rrpdusumodificacion);
            if (!dr.IsDBNull(iRrpdusumodificacion)) entity.Rrpdusumodificacion = dr.GetString(iRrpdusumodificacion);

            int iRrpdfecmodificacion = dr.GetOrdinal(this.Rrpdfecmodificacion);
            if (!dr.IsDBNull(iRrpdfecmodificacion)) entity.Rrpdfecmodificacion = dr.GetDateTime(iRrpdfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos
        
        public string Rrpdcodi = "RRPDCODI";
        public string Rrpecodi = "RRPECODI";       
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Rrpdporcentaje = "RRPDPORCENTAJE";
        public string Rrpdusucreacion = "RRPDUSUCREACION";
        public string Rrpdfeccreacion = "RRPDFECCREACION";
        public string Rrpdusumodificacion = "RRPDUSUMODIFICACION";
        public string Rrpdfecmodificacion = "RRPDFECMODIFICACION";
        //Variables adicionales para mostrar en consultas
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public string SqlNumeroEmpresas
        {
            get { return base.GetSqlXml("GetMaxNumEmpresas"); }
        }
        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlDeleteByCriteriaRRPE
        {
            get { return base.GetSqlXml("DeleteByCriteriaRRPE"); }
        }
    }
}
