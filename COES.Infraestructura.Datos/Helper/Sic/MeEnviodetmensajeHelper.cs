using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ENVIODETMENSAJE
    /// </summary>
    public class MeEnviodetmensajeHelper : HelperBase
    {
        public MeEnviodetmensajeHelper(): base(Consultas.MeEnviodetmensajeSql)
        {

        }

        public MeEnvioDetMensajeDTO Create(IDataReader dr)
        {
            MeEnvioDetMensajeDTO entity = new MeEnvioDetMensajeDTO();

            int iEdtmsjcodi = dr.GetOrdinal(this.Edtmsjcodi);
            if (!dr.IsDBNull(iEdtmsjcodi)) entity.Edtmsjcodi = Convert.ToInt32(dr.GetValue(iEdtmsjcodi));

            int iMsgcodi = dr.GetOrdinal(this.Msgcodi);
            if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

            int iEnvdetcodi = dr.GetOrdinal(this.Envdetcodi);
            if (!dr.IsDBNull(iEnvdetcodi)) entity.Envdetcodi = Convert.ToInt32(dr.GetValue(iEnvdetcodi));

            int iEdtmsjusucreacion = dr.GetOrdinal(this.Edtmsjusucreacion);
            if (!dr.IsDBNull(iEdtmsjusucreacion)) entity.Edtmsjusucreacion = dr.GetString(iEdtmsjusucreacion);

            int iEdtmsjfeccreacion = dr.GetOrdinal(this.Edtmsjfeccreacion);
            if (!dr.IsDBNull(iEdtmsjfeccreacion)) entity.Edtmsjfeccreacion = dr.GetDateTime(iEdtmsjfeccreacion);

            int iEdtmsjusumodificacion = dr.GetOrdinal(this.Edtmsjusumodificacion);
            if (!dr.IsDBNull(iEdtmsjusumodificacion)) entity.Edtmsjusumodificacion = dr.GetString(iEdtmsjusumodificacion);

            int iEdtmsjfecmodificacion = dr.GetOrdinal(this.Edtmsjfecmodificacion);
            if (!dr.IsDBNull(iEdtmsjfecmodificacion)) entity.Edtmsjfecmodificacion = dr.GetDateTime(iEdtmsjfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos
        public string Edtmsjcodi = "EDTMSJCODI";
        public string Msgcodi = "MSGCODI";
        public string Envdetcodi = "ENVDETCODI";
        public string Edtmsjusucreacion = "EDTMSJUSUCREACION";
        public string Edtmsjfeccreacion = "EDTMSJFECCREACION";
        public string Edtmsjusumodificacion = "EDTMSJUSUMODIFICACION";
        public string Edtmsjfecmodificacion = "EDTMSJFECMODIFICACION";
        #endregion

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 30/07/2018: FUNCIONES PERSONALIZADAS PARA INTERVENCIONES
        //--------------------------------------------------------------------------------
        public string SqlObtenerMsgCodi
        {
            get { return base.GetSqlXml("ObtenerMsgCodi"); }
        }

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 02/04/2019: FUNCIONES PERSONALIZADAS PARA INTERVENCIONES
        //--------------------------------------------------------------------------------
        public string SqlEliminarEnvDetMsgXEnvDetCodi
        {
            get { return base.GetSqlXml("EliminarEnvDetMsgXEnvDetCodi"); }
        }
    }
}
