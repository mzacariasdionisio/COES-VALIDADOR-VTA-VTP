using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_RELACION_TNA
    /// </summary>
    public class EqRelacionTnaHelper : HelperBase
    {
        public EqRelacionTnaHelper(): base(Consultas.EqRelacionTnaSql)
        {
        }

        public EqRelacionTnaDTO Create(IDataReader dr)
        {
            EqRelacionTnaDTO entity = new EqRelacionTnaDTO();

            int iReltnacodi = dr.GetOrdinal(this.Reltnacodi);
            if (!dr.IsDBNull(iReltnacodi)) entity.Reltnacodi = Convert.ToInt32(dr.GetValue(iReltnacodi));

            int iRelacioncodi = dr.GetOrdinal(this.Relacioncodi);
            if (!dr.IsDBNull(iRelacioncodi)) entity.Relacioncodi = Convert.ToInt32(dr.GetValue(iRelacioncodi));

            int iReltnanombre = dr.GetOrdinal(this.Reltnanombre);
            if (!dr.IsDBNull(iReltnanombre)) entity.Reltnanombre = dr.GetString(iReltnanombre);

            int iReltnaestado = dr.GetOrdinal(this.Reltnaestado);
            if (!dr.IsDBNull(iReltnaestado)) entity.Reltnaestado = dr.GetString(iReltnaestado);

            int iReltnausucreacion = dr.GetOrdinal(this.Reltnausucreacion);
            if (!dr.IsDBNull(iReltnausucreacion)) entity.Reltnausucreacion = dr.GetString(iReltnausucreacion);

            int iReltnafeccreacion = dr.GetOrdinal(this.Reltnafeccreacion);
            if (!dr.IsDBNull(iReltnafeccreacion)) entity.Reltnafeccreacion = dr.GetDateTime(iReltnafeccreacion);

            int iReltnausumodificacion = dr.GetOrdinal(this.Reltnausumodificacion);
            if (!dr.IsDBNull(iReltnausumodificacion)) entity.Reltnausumodificacion = dr.GetString(iReltnausumodificacion);

            int iReltnafecmodificacion = dr.GetOrdinal(this.Reltnafecmodificacion);
            if (!dr.IsDBNull(iReltnafecmodificacion)) entity.Reltnafecmodificacion = dr.GetDateTime(iReltnafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Reltnacodi = "RELTNACODI";
        public string Relacioncodi = "RELACIONCODI";
        public string Reltnanombre = "RELTNANOMBRE";
        public string Reltnaestado = "RELTNAESTADO";
        public string Reltnausucreacion = "RELTNAUSUCREACION";
        public string Reltnafeccreacion = "RELTNAFECCREACION";
        public string Reltnausumodificacion = "RELTNAUSUMODIFICACION";
        public string Reltnafecmodificacion = "RELTNAFECMODIFICACION";

        #endregion
    }
}
