using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RNT_PERIODO
    /// </summary>
    public class RntPeriodoHelper : HelperBase
    {
        public RntPeriodoHelper()
            : base(Consultas.RntPeriodoSql)
        {
        }

        public RntPeriodoDTO Create(IDataReader dr)
        {           
            RntPeriodoDTO entity = new RntPeriodoDTO();

            int iPeriodocodi = dr.GetOrdinal(this.Periodocodi);
            if (!dr.IsDBNull(iPeriodocodi)) entity.PeriodoCodi = Convert.ToInt32(dr.GetValue(iPeriodocodi));

            int iPerdestado = dr.GetOrdinal(this.Perdestado);
            if (!dr.IsDBNull(iPerdestado)) entity.PerdEstado = dr.GetString(iPerdestado);

            int iPerdanio = dr.GetOrdinal(this.Perdanio);
            if (!dr.IsDBNull(iPerdanio)) entity.PerdAnio = Convert.ToInt32(dr.GetValue(iPerdanio));

            int iPerdnombre = dr.GetOrdinal(this.Perdnombre);
            if (!dr.IsDBNull(iPerdnombre)) entity.PerdNombre = dr.GetString(iPerdnombre);

            int iPerdusuariocreacion = dr.GetOrdinal(this.Perdusuariocreacion);
            if (!dr.IsDBNull(iPerdusuariocreacion)) entity.PerdUsuarioCreacion = dr.GetString(iPerdusuariocreacion);

            int iPerdfechacreacion = dr.GetOrdinal(this.Perdfechacreacion);
            if (!dr.IsDBNull(iPerdfechacreacion)) entity.PerdFechaCreacion = dr.GetDateTime(iPerdfechacreacion);

            int iPerdusuarioupdate = dr.GetOrdinal(this.Perdusuarioupdate);
            if (!dr.IsDBNull(iPerdusuarioupdate)) entity.PerdUsuarioUpdate = dr.GetString(iPerdusuarioupdate);

            int iPerdfechaupdate = dr.GetOrdinal(this.Perdfechaupdate);
            if (!dr.IsDBNull(iPerdfechaupdate)) entity.PerdFechaUpdate = dr.GetDateTime(iPerdfechaupdate);

            int iPerdsemestre = dr.GetOrdinal(this.Perdsemestre);
            if (!dr.IsDBNull(iPerdsemestre)) entity.PerdSemestre = dr.GetString(iPerdsemestre);
                        
            return entity;
        }


        #region Mapeo de Campos

        public string Periodocodi = "PERDCODI";
        public string Perdestado = "PERDESTADO";
        public string Perdanio = "PERDANIO";
        public string Perdnombre = "PERDNOMBRE";
        public string Perdusuariocreacion = "PERDUSUARIOCREACION";
        public string Perdfechacreacion = "PERDFECHACREACION";
        public string Perdusuarioupdate = "PERDUSUARIOUPDATE";
        public string Perdfechaupdate = "PERDFECHAUPDATE";
        public string Perdsemestre = "PERDSEMESTRE";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
        public string SqlListCombo
        {
            get { return base.GetSqlXml("ListCombo"); }
        }
    }
}
