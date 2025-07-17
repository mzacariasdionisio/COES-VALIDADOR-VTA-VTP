using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_EQUIPOBARRA
    /// </summary>
    public class CmEquipobarraHelper : HelperBase
    {
        public CmEquipobarraHelper(): base(Consultas.CmEquipobarraSql)
        {
        }

        public CmEquipobarraDTO Create(IDataReader dr)
        {
            CmEquipobarraDTO entity = new CmEquipobarraDTO();

            int iCmeqbacodi = dr.GetOrdinal(this.Cmeqbacodi);
            if (!dr.IsDBNull(iCmeqbacodi)) entity.Cmeqbacodi = Convert.ToInt32(dr.GetValue(iCmeqbacodi));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iCmeqbaestado = dr.GetOrdinal(this.Cmeqbaestado);
            if (!dr.IsDBNull(iCmeqbaestado)) entity.Cmeqbaestado = dr.GetString(iCmeqbaestado);

            int iCmeqbavigencia = dr.GetOrdinal(this.Cmeqbavigencia);
            if (!dr.IsDBNull(iCmeqbavigencia)) entity.Cmeqbavigencia = dr.GetDateTime(iCmeqbavigencia);

            int iCmeqbaexpira = dr.GetOrdinal(this.Cmeqbaexpira);
            if (!dr.IsDBNull(iCmeqbaexpira)) entity.Cmeqbaexpira = dr.GetDateTime(iCmeqbaexpira);

            int iCmeqbausucreacion = dr.GetOrdinal(this.Cmeqbausucreacion);
            if (!dr.IsDBNull(iCmeqbausucreacion)) entity.Cmeqbausucreacion = dr.GetString(iCmeqbausucreacion);

            int iCmeqbafeccreacion = dr.GetOrdinal(this.Cmeqbafeccreacion);
            if (!dr.IsDBNull(iCmeqbafeccreacion)) entity.Cmeqbafeccreacion = dr.GetDateTime(iCmeqbafeccreacion);

            int iCmeqbausumodificacion = dr.GetOrdinal(this.Cmeqbausumodificacion);
            if (!dr.IsDBNull(iCmeqbausumodificacion)) entity.Cmeqbausumodificacion = dr.GetString(iCmeqbausumodificacion);

            int iCmeqbafecmodificacion = dr.GetOrdinal(this.Cmeqbafecmodificacion);
            if (!dr.IsDBNull(iCmeqbafecmodificacion)) entity.Cmeqbafecmodificacion = dr.GetDateTime(iCmeqbafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmeqbacodi = "CMEQBACODI";
        public string Configcodi = "CONFIGCODI";
        public string Cmeqbaestado = "CMEQBAESTADO";
        public string Cmeqbavigencia = "CMEQBAVIGENCIA";
        public string Cmeqbaexpira = "CMEQBAEXPIRA";
        public string Cmeqbausucreacion = "CMEQBAUSUCREACION";
        public string Cmeqbafeccreacion = "CMEQBAFECCREACION";
        public string Cmeqbausumodificacion = "CMEQBAUSUMODIFICACION";
        public string Cmeqbafecmodificacion = "CMEQBAFECMODIFICACION";
        public string Equinomb = "EQUINOMB";

        #endregion

        public string SqlOtenerHistorico
        {
            get { return base.GetSqlXml("ObtenerHistorico"); }
        }
    }
}
