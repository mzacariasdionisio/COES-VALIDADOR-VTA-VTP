using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_PERIODO
    /// </summary>
    public class CmPeriodoHelper : HelperBase
    {
        public CmPeriodoHelper(): base(Consultas.CmPeriodoSql)
        {
        }

        public CmPeriodoDTO Create(IDataReader dr)
        {
            CmPeriodoDTO entity = new CmPeriodoDTO();

            int iCmpercodi = dr.GetOrdinal(this.Cmpercodi);
            if (!dr.IsDBNull(iCmpercodi)) entity.Cmpercodi = Convert.ToInt32(dr.GetValue(iCmpercodi));

            int iCmperbase = dr.GetOrdinal(this.Cmperbase);
            if (!dr.IsDBNull(iCmperbase)) entity.Cmperbase = dr.GetString(iCmperbase);

            int iCmpermedia = dr.GetOrdinal(this.Cmpermedia);
            if (!dr.IsDBNull(iCmpermedia)) entity.Cmpermedia = dr.GetString(iCmpermedia);

            int iCmperpunta = dr.GetOrdinal(this.Cmperpunta);
            if (!dr.IsDBNull(iCmperpunta)) entity.Cmperpunta = dr.GetString(iCmperpunta);

            int iCmperestado = dr.GetOrdinal(this.Cmperestado);
            if (!dr.IsDBNull(iCmperestado)) entity.Cmperestado = dr.GetString(iCmperestado);

            int iCmpervigencia = dr.GetOrdinal(this.Cmpervigencia);
            if (!dr.IsDBNull(iCmpervigencia)) entity.Cmpervigencia = dr.GetDateTime(iCmpervigencia);

            int iCmperexpira = dr.GetOrdinal(this.Cmperexpira);
            if (!dr.IsDBNull(iCmperexpira)) entity.Cmperexpira = dr.GetDateTime(iCmperexpira);

            int iCmperusucreacion = dr.GetOrdinal(this.Cmperusucreacion);
            if (!dr.IsDBNull(iCmperusucreacion)) entity.Cmperusucreacion = dr.GetString(iCmperusucreacion);

            int iCmperfeccreacion = dr.GetOrdinal(this.Cmperfeccreacion);
            if (!dr.IsDBNull(iCmperfeccreacion)) entity.Cmperfeccreacion = dr.GetDateTime(iCmperfeccreacion);

            int iCmperusumodificacion = dr.GetOrdinal(this.Cmperusumodificacion);
            if (!dr.IsDBNull(iCmperusumodificacion)) entity.Cmperusumodificacion = dr.GetString(iCmperusumodificacion);

            int iCmperfecmodificacion = dr.GetOrdinal(this.Cmperfecmodificacion);
            if (!dr.IsDBNull(iCmperfecmodificacion)) entity.Cmperfecmodificacion = dr.GetDateTime(iCmperfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmpercodi = "CMPERCODI";
        public string Cmperbase = "CMPERBASE";
        public string Cmpermedia = "CMPERMEDIA";
        public string Cmperpunta = "CMPERPUNTA";
        public string Cmperestado = "CMPERESTADO";
        public string Cmpervigencia = "CMPERVIGENCIA";
        public string Cmperexpira = "CMPEREXPIRA";
        public string Cmperusucreacion = "CMPERUSUCREACION";
        public string Cmperfeccreacion = "CMPERFECCREACION";
        public string Cmperusumodificacion = "CMPERUSUMODIFICACION";
        public string Cmperfecmodificacion = "CMPERFECMODIFICACION";

        #endregion

        public string SqlObtenerHistorico
        {
            get { return base.GetSqlXml("ObtenerHistorico"); }
        }
    }
}
