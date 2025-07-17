using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_FERIADO
    /// </summary>
    public class PmoFeriadoHelper : HelperBase
    {
        public PmoFeriadoHelper() : base(Consultas.PmoFeriadoSql)
        {
        }

        public PmoFeriadoDTO Create(IDataReader dr)
        {
            PmoFeriadoDTO entity = new PmoFeriadoDTO();

            int iPmfrdocodi = dr.GetOrdinal(this.Pmfrdocodi);
            if (!dr.IsDBNull(iPmfrdocodi)) entity.Pmfrdocodi = Convert.ToInt32(dr.GetValue(iPmfrdocodi));

            int iPmanopcodi = dr.GetOrdinal(this.Pmanopcodi);
            if (!dr.IsDBNull(iPmanopcodi)) entity.Pmanopcodi = Convert.ToInt32(dr.GetValue(iPmanopcodi));

            int iPmfrdofecha = dr.GetOrdinal(this.Pmfrdofecha);
            if (!dr.IsDBNull(iPmfrdofecha)) entity.Pmfrdofecha = dr.GetDateTime(iPmfrdofecha);

            int iPmfrdodescripcion = dr.GetOrdinal(this.Pmfrdodescripcion);
            if (!dr.IsDBNull(iPmfrdodescripcion)) entity.Pmfrdodescripcion = dr.GetString(iPmfrdodescripcion);

            int iPmfrdoestado = dr.GetOrdinal(this.Pmfrdoestado);
            if (!dr.IsDBNull(iPmfrdoestado)) entity.Pmfrdoestado = Convert.ToInt32(dr.GetValue(iPmfrdoestado));

            int iPmfrdousucreacion = dr.GetOrdinal(this.Pmfrdousucreacion);
            if (!dr.IsDBNull(iPmfrdousucreacion)) entity.Pmfrdousucreacion = dr.GetString(iPmfrdousucreacion);

            int iPmfrdofeccreacion = dr.GetOrdinal(this.Pmfrdofeccreacion);
            if (!dr.IsDBNull(iPmfrdofeccreacion)) entity.Pmfrdofeccreacion = dr.GetDateTime(iPmfrdofeccreacion);

            int iPmfrdousumodificacion = dr.GetOrdinal(this.Pmfrdousumodificacion);
            if (!dr.IsDBNull(iPmfrdousumodificacion)) entity.Pmfrdousumodificacion = dr.GetString(iPmfrdousumodificacion);

            int iPmfrdofecmodificacion = dr.GetOrdinal(this.Pmfrdofecmodificacion);
            if (!dr.IsDBNull(iPmfrdofecmodificacion)) entity.Pmfrdofecmodificacion = dr.GetDateTime(iPmfrdofecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Pmfrdocodi = "PMFRDOCODI";
        public string Pmanopcodi = "PMANOPCODI";
        public string Pmfrdofecha = "PMFRDOFECHA";
        public string Pmfrdodescripcion = "PMFRDODESCRIPCION";
        public string Pmfrdoestado = "PMFRDOESTADO";
        public string Pmfrdousucreacion = "PMFRDOUSUCREACION";
        public string Pmfrdofeccreacion = "PMFRDOFECCREACION";
        public string Pmfrdousumodificacion = "PMFRDOUSUMODIFICACION";
        public string Pmfrdofecmodificacion = "PMFRDOFECMODIFICACION";

        #endregion

        public string SqlUpdateEstadoBaja
        {
            get { return GetSqlXml("UpdateEstadoBaja"); }
        }
        public string SqlUpdateAprobar
        {
            get { return base.GetSqlXml("UpdateAprobar"); }
        }
    }
}
