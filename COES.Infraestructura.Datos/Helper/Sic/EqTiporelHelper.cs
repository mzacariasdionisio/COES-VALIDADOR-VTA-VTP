using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_TIPOREL
    /// </summary>
    public class EqTiporelHelper : HelperBase
    {
        public EqTiporelHelper(): base(Consultas.EqTiporelSql)
        {
        }

        public EqTiporelDTO Create(IDataReader dr)
        {
            EqTiporelDTO entity = new EqTiporelDTO();

            int iTiporelcodi = dr.GetOrdinal(this.Tiporelcodi);
            if (!dr.IsDBNull(iTiporelcodi)) entity.Tiporelcodi = Convert.ToInt32(dr.GetValue(iTiporelcodi));

            int iTiporelnomb = dr.GetOrdinal(this.Tiporelnomb);
            if (!dr.IsDBNull(iTiporelnomb)) entity.Tiporelnomb = dr.GetString(iTiporelnomb);

            int iTiporelEstado = dr.GetOrdinal(this.TiporelEstado);
            if (!dr.IsDBNull(iTiporelEstado)) entity.TiporelEstado = dr.GetString(iTiporelEstado);

            int iTiporelUsuarioCreacion = dr.GetOrdinal(this.TiporelUsuarioCreacion);
            if (!dr.IsDBNull(iTiporelUsuarioCreacion)) entity.TiporelUsuarioCreacion = dr.GetString(iTiporelUsuarioCreacion);

            int iTiporelFechaCreacion = dr.GetOrdinal(this.TiporelFechaCreacion);
            if (!dr.IsDBNull(iTiporelFechaCreacion)) entity.TiporelFechaCreacion = dr.GetDateTime(iTiporelFechaCreacion);

            int iTiporelUsuarioUpdate = dr.GetOrdinal(this.TiporelUsuarioUpdate);
            if (!dr.IsDBNull(iTiporelUsuarioUpdate)) entity.TiporelUsuarioUpdate = dr.GetString(iTiporelUsuarioUpdate);

            int iTiporelFechaUpdate = dr.GetOrdinal(this.TiporelFechaUpdate);
            if (!dr.IsDBNull(iTiporelFechaUpdate)) entity.TiporelFechaUpdate = dr.GetDateTime(iTiporelFechaUpdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Tiporelcodi = "TIPORELCODI";
        public string Tiporelnomb = "TIPORELNOMB";
        public string TiporelEstado = "TIPORELESTADO";
        public string TiporelUsuarioCreacion = "TIPORELUSUARIOCREACION";
        public string TiporelFechaCreacion = "TIPORELFECHACREACION";
        public string TiporelUsuarioUpdate = "TIPORELUSUARIOUPDATE";
        public string TiporelFechaUpdate = "TIPORELFECHAUPDATE";

        #endregion
        public string SqlListadoxFiltro
        {
            get { return base.GetSqlXml("ListadoxFiltro"); }
        }
        public string SqlCantidadListadoFiltro
        {
            get { return base.GetSqlXml("CantidadListadoFiltro"); }
        }
    }
}

