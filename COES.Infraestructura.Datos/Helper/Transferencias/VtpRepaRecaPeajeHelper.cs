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
    /// Clase que contiene el mapeo de la tabla VTP_REPA_RECA_PEAJE
    /// </summary>
    public class VtpRepaRecaPeajeHelper : HelperBase
    {
        public VtpRepaRecaPeajeHelper(): base(Consultas.VtpRepaRecaPeajeSql)
        {
        }

        public VtpRepaRecaPeajeDTO Create(IDataReader dr)
        {
            VtpRepaRecaPeajeDTO entity = new VtpRepaRecaPeajeDTO();

            int iRrpecodi = dr.GetOrdinal(this.Rrpecodi);
            if (!dr.IsDBNull(iRrpecodi)) entity.Rrpecodi = Convert.ToInt32(dr.GetValue(iRrpecodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iRrpenombre = dr.GetOrdinal(this.Rrpenombre);
            if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);

            int iRrpeusucreacion = dr.GetOrdinal(this.Rrpeusucreacion);
            if (!dr.IsDBNull(iRrpeusucreacion)) entity.Rrpeusucreacion = dr.GetString(iRrpeusucreacion);

            int iRrpefeccreacion = dr.GetOrdinal(this.Rrpefeccreacion);
            if (!dr.IsDBNull(iRrpefeccreacion)) entity.Rrpefeccreacion = dr.GetDateTime(iRrpefeccreacion);

            int iRrpeusumodificacion = dr.GetOrdinal(this.Rrpeusumodificacion);
            if (!dr.IsDBNull(iRrpeusumodificacion)) entity.Rrpeusumodificacion = dr.GetString(iRrpeusumodificacion);

            int iRrpefecmodificacion = dr.GetOrdinal(this.Rrpefecmodificacion);
            if (!dr.IsDBNull(iRrpefecmodificacion)) entity.Rrpefecmodificacion = dr.GetDateTime(iRrpefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Rrpecodi = "RRPECODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Rrpenombre = "RRPENOMBRE";
        public string Rrpeusucreacion = "RRPEUSUCREACION";
        public string Rrpefeccreacion = "RRPEFECCREACION";
        public string Rrpeusumodificacion = "RRPEUSUMODIFICACION";
        public string Rrpefecmodificacion = "RRPEFECMODIFICACION";

        #endregion

        public string SqlGetByNombre 
        {
            get { return base.GetSqlXml("GetByNombre"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }
    }
}
