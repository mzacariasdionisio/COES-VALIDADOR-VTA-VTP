using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESAMENSAJE
    /// </summary>
    public class SiEmpresamensajeHelper : HelperBase
    {
        public SiEmpresamensajeHelper(): base(Consultas.SiEmpresamensajeSql)
        {

        }

        public SiEmpresaMensajeDTO Create(IDataReader dr)
        {
            SiEmpresaMensajeDTO entity = new SiEmpresaMensajeDTO();

            int iEmpmsjcodi = dr.GetOrdinal(this.Empmsjcodi);
            if (!dr.IsDBNull(iEmpmsjcodi)) entity.Empmsjcodi = Convert.ToInt32(dr.GetValue(iEmpmsjcodi));

            int iMsgcodi = dr.GetOrdinal(this.Msgcodi);
            if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

            int iEnvdetcodi = dr.GetOrdinal(this.Envdetcodi);
            if (!dr.IsDBNull(iEnvdetcodi)) entity.Envdetcodi = Convert.ToInt32(dr.GetValue(iEnvdetcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmpmsjusucreacion = dr.GetOrdinal(this.Empmsjusucreacion);
            if (!dr.IsDBNull(iEmpmsjusucreacion)) entity.Empmsjusucreacion = dr.GetString(iEmpmsjusucreacion);

            int iEmpmsjfeccreacion = dr.GetOrdinal(this.Empmsjfeccreacion);
            if (!dr.IsDBNull(iEmpmsjfeccreacion)) entity.Empmsjfeccreacion = dr.GetDateTime(iEmpmsjfeccreacion);

            int iEmpmsjusumodificacion = dr.GetOrdinal(this.Empmsjusumodificacion);
            if (!dr.IsDBNull(iEmpmsjusumodificacion)) entity.Empmsjusumodificacion = dr.GetString(iEmpmsjusumodificacion);

            int iEmpmsjfecmodificacion = dr.GetOrdinal(this.Empmsjfecmodificacion);
            if (!dr.IsDBNull(iEmpmsjfecmodificacion)) entity.Empmsjfecmodificacion = dr.GetDateTime(iEmpmsjfecmodificacion);

            return entity;
        }
        
        #region Mapeo de Campos
        public string Empmsjcodi = "EMPMSJCODI";
        public string Msgcodi = "MSGCODI";
        public string Envdetcodi = "ENVDETCODI";
        public string Emprcodi = "EMPRCODI";
        public string Empmsjusucreacion = "EMPMSJUSUCREACION";
        public string Empmsjfeccreacion = "EMPMSJFECCREACION";
        public string Empmsjusumodificacion = "EMPMSJUSUMODIFICACION";
        public string Empmsjfecmodificacion = "EMPMSJFECMODIFICACION";
        #endregion

        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 02/04/2019: FUNCIONES PERSONALIZADAS PARA INTERVENCIONES
        //--------------------------------------------------------------------------------
        public string SqlEliminarEmpresaMensajeXEnvDetCodi
        {
            get { return base.GetSqlXml("EliminarEmpresaMensajeXEnvDetCodi"); }
        }
    }
}
