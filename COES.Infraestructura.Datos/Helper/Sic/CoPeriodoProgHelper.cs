using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_PERIODO_PROG
    /// </summary>
    public class CoPeriodoProgHelper : HelperBase
    {
        public CoPeriodoProgHelper(): base(Consultas.CoPeriodoProgSql)
        {
        }

        public CoPeriodoProgDTO Create(IDataReader dr)
        {
            CoPeriodoProgDTO entity = new CoPeriodoProgDTO();

            int iPerprgcodi = dr.GetOrdinal(this.Perprgcodi);
            if (!dr.IsDBNull(iPerprgcodi)) entity.Perprgcodi = Convert.ToInt32(dr.GetValue(iPerprgcodi));

            int iPerprgvigencia = dr.GetOrdinal(this.Perprgvigencia);
            if (!dr.IsDBNull(iPerprgvigencia)) entity.Perprgvigencia = dr.GetDateTime(iPerprgvigencia);

            int iPerprgvalor = dr.GetOrdinal(this.Perprgvalor);
            if (!dr.IsDBNull(iPerprgvalor)) entity.Perprgvalor = dr.GetDecimal(iPerprgvalor);

            int iPerprgestado = dr.GetOrdinal(this.Perprgestado);
            if (!dr.IsDBNull(iPerprgestado)) entity.Perprgestado = dr.GetString(iPerprgestado);

            int iPerprgusucreacion = dr.GetOrdinal(this.Perprgusucreacion);
            if (!dr.IsDBNull(iPerprgusucreacion)) entity.Perprgusucreacion = dr.GetString(iPerprgusucreacion);

            int iPerprgfeccreacion = dr.GetOrdinal(this.Perprgfeccreacion);
            if (!dr.IsDBNull(iPerprgfeccreacion)) entity.Perprgfeccreacion = dr.GetDateTime(iPerprgfeccreacion);

            int iPerprgusumodificacion = dr.GetOrdinal(this.Perprgusumodificacion);
            if (!dr.IsDBNull(iPerprgusumodificacion)) entity.Perprgusumodificacion = dr.GetString(iPerprgusumodificacion);

            int iPerprgfecmodificacion = dr.GetOrdinal(this.Perprgfecmodificacion);
            if (!dr.IsDBNull(iPerprgfecmodificacion)) entity.Perprgfecmodificacion = dr.GetDateTime(iPerprgfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Perprgcodi = "PERPRGCODI";
        public string Perprgvigencia = "PERPRGVIGENCIA";
        public string Perprgvalor = "PERPRGVALOR";
        public string Perprgestado = "PERPRGESTADO";
        public string Perprgusucreacion = "PERPRGUSUCREACION";
        public string Perprgfeccreacion = "PERPRGFECCREACION";
        public string Perprgusumodificacion = "PERPRGUSUMODIFICACION";
        public string Perprgfecmodificacion = "PERPRGFECMODIFICACION";

        #endregion

        public string SqlObtenerPeriodoVigente
        {
            get { return base.GetSqlXml("ObtenerPeriodoVigente"); }
        }
    }
}
