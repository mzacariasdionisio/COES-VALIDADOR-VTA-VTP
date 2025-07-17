using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ENVIODET
    /// </summary>
    public class MeEnviodetHelper : HelperBase
    {
        public MeEnviodetHelper(): base(Consultas.MeEnviodetSql)
        {

        }

        public MeEnviodetDTO Create(IDataReader dr)
        {
            MeEnviodetDTO entity = new MeEnviodetDTO();

            int iEnvdetcodi = dr.GetOrdinal(this.Envdetcodi);
            if (!dr.IsDBNull(iEnvdetcodi)) entity.Envdetcodi = Convert.ToInt32(dr.GetValue(iEnvdetcodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iEnvdetfpkcodi = dr.GetOrdinal(this.Envdetfpkcodi);
            if (!dr.IsDBNull(iEnvdetfpkcodi)) entity.Envdetfpkcodi = Convert.ToInt32(dr.GetValue(iEnvdetfpkcodi));

            int iEnvdetusucreacion = dr.GetOrdinal(this.Envdetusucreacion);
            if (!dr.IsDBNull(iEnvdetusucreacion)) entity.Envdetusucreacion = dr.GetString(iEnvdetusucreacion);

            int iEnvdetfeccreacion = dr.GetOrdinal(this.Envdetfeccreacion);
            if (!dr.IsDBNull(iEnvdetfeccreacion)) entity.Envdetfeccreacion = dr.GetDateTime(iEnvdetfeccreacion);

            int iEnvdetusumodificacion = dr.GetOrdinal(this.Envdetusumodificacion);
            if (!dr.IsDBNull(iEnvdetusumodificacion)) entity.Envdetusumodificacion = dr.GetString(iEnvdetusumodificacion);

            int iEnvdetfecmodificacion = dr.GetOrdinal(this.Envdetfecmodificacion);
            if (!dr.IsDBNull(iEnvdetfecmodificacion)) entity.Envdetfecmodificacion = dr.GetDateTime(iEnvdetfecmodificacion);


            return entity;
        }


        #region Mapeo de Campos

        public string Envdetcodi = "ENVDETCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Envdetfpkcodi = "ENVDETFPKCODI";
        public string Envdetusucreacion = "ENVDETUSUCREACION";
        public string Envdetfeccreacion = "ENVDETFECCREACION";
        public string Envdetusumodificacion = "ENVDETUSUMODIFICACION";
        public string Envdetfecmodificacion = "ENVDETFECMODIFICACION";

        #endregion

        #region Querys SQL
        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 20/10/2017: FUNCIONES PERSONALIZADAS PARA INTERVENCIONES
        //--------------------------------------------------------------------------------
        public string SqlGetMaxIdDetalleEnvio
        {
            get { return base.GetSqlXml("GetMaxIdDetalleEnvio"); }
        }

        public string SqlInsertarDetalleEnvio
        {
            get { return base.GetSqlXml("InsertarDetalleEnvio"); }
        }

        #endregion

        #region MODIFICACIONES ASSETEC - PARA INTERVENCIONES

        public string SqlObtenerEnvDetCodi
        {
            get { return base.GetSqlXml("ObtenerEnvDetCodi"); }
        }


        public string SqlEliminarDetalleEnvioFisicoPorIntervencionId
        {
            get { return base.GetSqlXml("EliminarEnvioDetalleFisicoPorIntervencionId"); }
        }

        public string SqlObtenerDetalleEnvioPorIntervencionId
        {
            get { return base.GetSqlXml("ObtenerEnvioDetallePorIntervencionId"); }
        }
        #endregion
    }
}
