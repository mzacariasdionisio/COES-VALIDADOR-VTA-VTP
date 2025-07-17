using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_RFRIA_UNIDADRESTRIC
    /// </summary>
    public class MeRfriaUnidadrestricHelper : HelperBase
    {
        public MeRfriaUnidadrestricHelper(): base(Consultas.MeRfriaUnidadrestricSql)
        {
        }

        public MeRfriaUnidadrestricDTO Create(IDataReader dr)
        {
            MeRfriaUnidadrestricDTO entity = new MeRfriaUnidadrestricDTO();

            int iUrfriacodi = dr.GetOrdinal(this.Urfriacodi);
            if (!dr.IsDBNull(iUrfriacodi)) entity.Urfriacodi = Convert.ToInt32(dr.GetValue(iUrfriacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iUrfriafechaperiodo = dr.GetOrdinal(this.Urfriafechaperiodo);
            if (!dr.IsDBNull(iUrfriafechaperiodo)) entity.Urfriafechaperiodo = dr.GetDateTime(iUrfriafechaperiodo);

            int iUrfriafechaini = dr.GetOrdinal(this.Urfriafechaini);
            if (!dr.IsDBNull(iUrfriafechaini)) entity.Urfriafechaini = dr.GetDateTime(iUrfriafechaini);

            int iUrfriafechafin = dr.GetOrdinal(this.Urfriafechafin);
            if (!dr.IsDBNull(iUrfriafechafin)) entity.Urfriafechafin = dr.GetDateTime(iUrfriafechafin);

            int iUrfriausucreacion = dr.GetOrdinal(this.Urfriausucreacion);
            if (!dr.IsDBNull(iUrfriausucreacion)) entity.Urfriausucreacion = dr.GetString(iUrfriausucreacion);

            int iUrfriafeccreacion = dr.GetOrdinal(this.Urfriafeccreacion);
            if (!dr.IsDBNull(iUrfriafeccreacion)) entity.Urfriafeccreacion = dr.GetDateTime(iUrfriafeccreacion);

            int iUrfriausumodificacion = dr.GetOrdinal(this.Urfriausumodificacion);
            if (!dr.IsDBNull(iUrfriausumodificacion)) entity.Urfriausumodificacion = dr.GetString(iUrfriausumodificacion);

            int iUrfriafecmodificacion = dr.GetOrdinal(this.Urfriafecmodificacion);
            if (!dr.IsDBNull(iUrfriafecmodificacion)) entity.Urfriafecmodificacion = dr.GetDateTime(iUrfriafecmodificacion);

            int iUrfriaactivo = dr.GetOrdinal(this.Urfriaactivo);
            if (!dr.IsDBNull(iUrfriaactivo)) entity.Urfriaactivo = Convert.ToInt32(dr.GetValue(iUrfriaactivo));

            int iUrfriaobservacion = dr.GetOrdinal(this.Urfriaobservacion);
            if (!dr.IsDBNull(iUrfriaobservacion)) entity.Urfriaobservacion = dr.GetString(iUrfriaobservacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Urfriacodi = "URFRIACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Urfriafechaperiodo = "URFRIAFECHAPERIODO";
        public string Urfriafechaini = "URFRIAFECHAINI";
        public string Urfriafechafin = "URFRIAFECHAFIN";
        public string Urfriausucreacion = "URFRIAUSUCREACION";
        public string Urfriafeccreacion = "URFRIAFECCREACION";
        public string Urfriausumodificacion = "URFRIAUSUMODIFICACION";
        public string Urfriafecmodificacion = "URFRIAFECMODIFICACION";
        public string Urfriaactivo = "URFRIAACTIVO";
        public string Urfriaobservacion = "URFRIAOBSERVACION";

        public string Empresanomb = "EMPRNOMB";
        public string Centralnomb = "CENTRALNOMB";
        public string Unidadnomb = "UNIDADNOMB";

        #endregion
    }
}
