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
    /// Clase que contiene el mapeo de la tabla VTP_PEAJE_EGRESO
    /// </summary>
    public class VtpPeajeEgresoHelper : HelperBase
    {
        public VtpPeajeEgresoHelper(): base(Consultas.VtpPeajeEgresoSql)
        {
        }

        public VtpPeajeEgresoDTO Create(IDataReader dr)
        {
            VtpPeajeEgresoDTO entity = new VtpPeajeEgresoDTO();

            int iPegrcodi = dr.GetOrdinal(this.Pegrcodi);
            if (!dr.IsDBNull(iPegrcodi)) entity.Pegrcodi = Convert.ToInt32(dr.GetValue(iPegrcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPegrestado = dr.GetOrdinal(this.Pegrestado);
            if (!dr.IsDBNull(iPegrestado)) entity.Pegrestado = Convert.ToString(dr.GetValue(iPegrestado));

            int iPegrplazo = dr.GetOrdinal(this.Pegrplazo);
            if (!dr.IsDBNull(iPegrplazo)) entity.Pegrplazo = Convert.ToString(dr.GetValue(iPegrplazo));

            int iPegrusucreacion = dr.GetOrdinal(this.Pegrusucreacion);
            if (!dr.IsDBNull(iPegrusucreacion)) entity.Pegrusucreacion = dr.GetString(iPegrusucreacion);

            int iPegrfeccreacion = dr.GetOrdinal(this.Pegrfeccreacion);
            if (!dr.IsDBNull(iPegrfeccreacion)) entity.Pegrfeccreacion = dr.GetDateTime(iPegrfeccreacion);

            int iPegrusumodificacion = dr.GetOrdinal(this.Pegrusumodificacion);
            if (!dr.IsDBNull(iPegrusumodificacion)) entity.Pegrusumodificacion = dr.GetString(iPegrusumodificacion);

            int iPegrfecmodificacion = dr.GetOrdinal(this.Pegrfecmodificacion);
            if (!dr.IsDBNull(iPegrfecmodificacion)) entity.Pegrfecmodificacion = dr.GetDateTime(iPegrfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pegrcodi = "PEGRCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Pegrestado = "PEGRESTADO";
        public string Pegrplazo = "PEGRPLAZO";
        public string Pegrusucreacion = "PEGRUSUCREACION";
        public string Pegrfeccreacion = "PEGRFECCREACION";
        public string Pegrusumodificacion = "PEGRUSUMODIFICACION";
        public string Pegrfecmodificacion = "PEGRFECMODIFICACION";
        public string Emprnomb = "EMPRNOMB";
       
        #endregion

        public string SqlUpdateByCriteria
        {
            get { return base.GetSqlXml("UpdateByCriteria"); }
        }

        public string SqlListView
        {
            get { return base.GetSqlXml("ListView"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlObtenerReportePorEmpresa
        {
            get { return base.GetSqlXml("ObtenerReportePorEmpresa"); }
        }

        public string SqlListConsulta
        {
            get { return base.GetSqlXml("ListConsulta"); }
        }
        public string SqlGetPreviusPeriod
        {
            get { return base.GetSqlXml("GetPreviusPeriod"); }
        }
    }
}
