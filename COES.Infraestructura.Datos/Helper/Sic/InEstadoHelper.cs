using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_ESTADO
    /// </summary>
    public class InEstadoHelper : HelperBase
    {
        public InEstadoHelper(): base(Consultas.InEstadoSql)
        {
        }

        public InEstadoDTO Create(IDataReader dr)
        {
            InEstadoDTO entity = new InEstadoDTO();

            int iEstadocodi = dr.GetOrdinal(this.Estadocodi);
            if (!dr.IsDBNull(iEstadocodi)) entity.Estadocodi = Convert.ToInt32(dr.GetValue(iEstadocodi));

            int iEstadonomb = dr.GetOrdinal(this.Estadonomb);
            if (!dr.IsDBNull(iEstadonomb)) entity.Estadonomb = dr.GetString(iEstadonomb);

            int iEstadousucreacion = dr.GetOrdinal(this.Estadousucreacion);
            if (!dr.IsDBNull(iEstadousucreacion)) entity.Estadousucreacion = dr.GetString(iEstadousucreacion);

            int iEstadofeccreacion = dr.GetOrdinal(this.Estadofeccreacion);
            if (!dr.IsDBNull(iEstadofeccreacion)) entity.Estadofeccreacion = dr.GetDateTime(iEstadofeccreacion);

            int iEstadousumodificacion = dr.GetOrdinal(this.Estadousumodificacion);
            if (!dr.IsDBNull(iEstadousumodificacion)) entity.Estadousumodificacion = dr.GetString(iEstadousumodificacion);

            int iEstadofecmodificacion = dr.GetOrdinal(this.Estadofecmodificacion);
            if (!dr.IsDBNull(iEstadofecmodificacion)) entity.Estadofecmodificacion = dr.GetDateTime(iEstadofecmodificacion);

            int iEstadopadre = dr.GetOrdinal(this.Estadopadre);
            if (!dr.IsDBNull(iEstadopadre)) entity.Estadopadre = Convert.ToInt32(dr.GetValue(iEstadopadre));

            return entity;
        }


        #region Mapeo de Campos

        public string Estadocodi = "ESTADOCODI";
        public string Estadonomb = "ESTADONOMB";
        public string Estadousucreacion = "ESTADOUSUCREACION";
        public string Estadofeccreacion = "ESTADOFECCREACION";
        public string Estadousumodificacion = "ESTADOUSUMODIFICACION";
        public string Estadofecmodificacion = "ESTADOFECMODIFICACION";
        public string Estadopadre = "ESTADOPADRE";

        #endregion

        #region Querys SQL
        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/10/2017: FUNCIONES PERSONALIZADAS PARA ESTADOS
        //--------------------------------------------------------------------------------
        public string SqlListarComboEstadosMantenimiento
        {
            get { return base.GetSqlXml("ListarComboEstadosMantenimiento"); }
        }

        public string SqlListarComboEstadosConsultas
        {
            get { return base.GetSqlXml("ListarComboEstadosConsultas"); }
        }
        #endregion
    }
}
