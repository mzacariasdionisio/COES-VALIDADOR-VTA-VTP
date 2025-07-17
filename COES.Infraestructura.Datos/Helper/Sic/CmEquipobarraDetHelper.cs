using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_EQUIPOBARRA_DET
    /// </summary>
    public class CmEquipobarraDetHelper : HelperBase
    {
        public CmEquipobarraDetHelper(): base(Consultas.CmEquipobarraDetSql)
        {
        }

        public CmEquipobarraDetDTO Create(IDataReader dr)
        {
            CmEquipobarraDetDTO entity = new CmEquipobarraDetDTO();

            int iCmebdecodi = dr.GetOrdinal(this.Cmebdecodi);
            if (!dr.IsDBNull(iCmebdecodi)) entity.Cmebdecodi = Convert.ToInt32(dr.GetValue(iCmebdecodi));

            int iCmeqbacodi = dr.GetOrdinal(this.Cmeqbacodi);
            if (!dr.IsDBNull(iCmeqbacodi)) entity.Cmeqbacodi = Convert.ToInt32(dr.GetValue(iCmeqbacodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCmebdeusucreacion = dr.GetOrdinal(this.Cmebdeusucreacion);
            if (!dr.IsDBNull(iCmebdeusucreacion)) entity.Cmebdeusucreacion = dr.GetString(iCmebdeusucreacion);

            int iCmebdefeccreacion = dr.GetOrdinal(this.Cmebdefeccreacion);
            if (!dr.IsDBNull(iCmebdefeccreacion)) entity.Cmebdefeccreacion = dr.GetDateTime(iCmebdefeccreacion);

            int iCmebdeusumodificacion = dr.GetOrdinal(this.Cmebdeusumodificacion);
            if (!dr.IsDBNull(iCmebdeusumodificacion)) entity.Cmebdeusumodificacion = dr.GetString(iCmebdeusumodificacion);

            int iCmebdefecmodificacion = dr.GetOrdinal(this.Cmebdefecmodificacion);
            if (!dr.IsDBNull(iCmebdefecmodificacion)) entity.Cmebdefecmodificacion = dr.GetDateTime(iCmebdefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmebdecodi = "CMEBDECODI";
        public string Cmeqbacodi = "CMEQBACODI";
        public string Barrcodi = "BARRCODI";
        public string Cmebdeusucreacion = "CMEBDEUSUCREACION";
        public string Cmebdefeccreacion = "CMEBDEFECCREACION";
        public string Cmebdeusumodificacion = "CMEBDEUSUMODIFICACION";
        public string Cmebdefecmodificacion = "CMEBDEFECMODIFICACION";
        public string Barrnombre = "BARRNOMBRE";
        #endregion
    }
}
