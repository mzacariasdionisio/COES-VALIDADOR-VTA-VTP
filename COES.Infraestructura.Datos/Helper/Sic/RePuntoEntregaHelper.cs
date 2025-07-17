using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_PUNTO_ENTREGA
    /// </summary>
    public class RePuntoEntregaHelper : HelperBase
    {
        public RePuntoEntregaHelper(): base(Consultas.RePuntoEntregaSql)
        {
        }

        public RePuntoEntregaDTO Create(IDataReader dr)
        {
            RePuntoEntregaDTO entity = new RePuntoEntregaDTO();

            int iRepentcodi = dr.GetOrdinal(this.Repentcodi);
            if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

            int iRepentnombre = dr.GetOrdinal(this.Repentnombre);
            if (!dr.IsDBNull(iRepentnombre)) entity.Repentnombre = dr.GetString(iRepentnombre);

            int iRentcodi = dr.GetOrdinal(this.Rentcodi);
            if (!dr.IsDBNull(iRentcodi)) entity.Rentcodi = Convert.ToInt32(dr.GetValue(iRentcodi));

            int iRepentestado = dr.GetOrdinal(this.Repentestado);
            if (!dr.IsDBNull(iRepentestado)) entity.Repentestado = dr.GetString(iRepentestado);

            int iRepentusucreacion = dr.GetOrdinal(this.Repentusucreacion);
            if (!dr.IsDBNull(iRepentusucreacion)) entity.Repentusucreacion = dr.GetString(iRepentusucreacion);

            int iRepentfeccreacion = dr.GetOrdinal(this.Repentfeccreacion);
            if (!dr.IsDBNull(iRepentfeccreacion)) entity.Repentfeccreacion = dr.GetDateTime(iRepentfeccreacion);

            int iRepentusumodificacion = dr.GetOrdinal(this.Repentusumodificacion);
            if (!dr.IsDBNull(iRepentusumodificacion)) entity.Repentusumodificacion = dr.GetString(iRepentusumodificacion);

            int iRepentfecmodificacion = dr.GetOrdinal(this.Repentfecmodificacion);
            if (!dr.IsDBNull(iRepentfecmodificacion)) entity.Repentfecmodificacion = dr.GetDateTime(iRepentfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Repentcodi = "REPENTCODI";
        public string Repentnombre = "REPENTNOMBRE";
        public string Rentcodi = "RENTCODI";
        public string Repentestado = "REPENTESTADO";
        public string Repentusucreacion = "REPENTUSUCREACION";
        public string Repentfeccreacion = "REPENTFECCREACION";
        public string Repentusumodificacion = "REPENTUSUMODIFICACION";
        public string Repentfecmodificacion = "REPENTFECMODIFICACION";

        #endregion
    }
}
