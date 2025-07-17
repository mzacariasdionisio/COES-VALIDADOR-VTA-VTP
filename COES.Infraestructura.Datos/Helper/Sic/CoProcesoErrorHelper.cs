using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CO_PROCESO_ERROR
    /// </summary>
    public class CoProcesoErrorHelper : HelperBase
    {
        public CoProcesoErrorHelper(): base(Consultas.CoProcesoErrorSql)
        {
        }

        public CoProcesoErrorDTO Create(IDataReader dr)
        {
            CoProcesoErrorDTO entity = new CoProcesoErrorDTO();

            int iProerrcodi = dr.GetOrdinal(this.Proerrcodi);
            if (!dr.IsDBNull(iProerrcodi)) entity.Proerrcodi = Convert.ToInt32(dr.GetValue(iProerrcodi));

            int iProdiacodi = dr.GetOrdinal(this.Prodiacodi);
            if (!dr.IsDBNull(iProdiacodi)) entity.Prodiacodi = Convert.ToInt32(dr.GetValue(iProdiacodi));

            int iProerrmsg = dr.GetOrdinal(this.Proerrmsg);
            if (!dr.IsDBNull(iProerrmsg)) entity.Proerrmsg = dr.GetString(iProerrmsg);

            int iProerrtipo = dr.GetOrdinal(this.Proerrtipo);
            if (!dr.IsDBNull(iProerrtipo)) entity.Proerrtipo = dr.GetString(iProerrtipo);

            int iProerrusucreacion = dr.GetOrdinal(this.Proerrusucreacion);
            if (!dr.IsDBNull(iProerrusucreacion)) entity.Proerrusucreacion = dr.GetString(iProerrusucreacion);

            int iProerrfeccreacion = dr.GetOrdinal(this.Proerrfeccreacion);
            if (!dr.IsDBNull(iProerrfeccreacion)) entity.Proerrfeccreacion = dr.GetDateTime(iProerrfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Proerrcodi = "PROERRCODI";
        public string Prodiacodi = "PRODIACODI";
        public string Proerrmsg = "PROERRMSG";
        public string Proerrtipo = "PROERRTIPO";
        public string Proerrusucreacion = "PROERRUSUCREACION";
        public string Proerrfeccreacion = "PROERRFECCREACION";
        public string Tablanomb = "TABLANOMB";
        public string TableName = "CO_PROCESO_ERROR";
        #endregion

        public string SqlListarTablas
        {
            get { return base.GetSqlXml("ListarTablas"); }
        }

        public string SqlEliminarProcesoError
        {
            get { return base.GetSqlXml("EliminarProcesoError"); }
        }

        public string SqlListarPorDia
        {
            get { return base.GetSqlXml("ListarPorDia"); }
        }
        
    }
}
