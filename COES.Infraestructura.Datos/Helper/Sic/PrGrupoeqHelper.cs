using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GRUPOEQ
    /// </summary>
    public class PrGrupoeqHelper : HelperBase
    {
        public PrGrupoeqHelper() : base(Consultas.PrGrupoeqSql)
        {
        }

        public PrGrupoeqDTO Create(IDataReader dr)
        {
            PrGrupoeqDTO entity = new PrGrupoeqDTO();

            int iGeqcodi = dr.GetOrdinal(this.Geqcodi);
            if (!dr.IsDBNull(iGeqcodi)) entity.Geqcodi = Convert.ToInt32(dr.GetValue(iGeqcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGeqfeccreacion = dr.GetOrdinal(this.Geqfeccreacion);
            if (!dr.IsDBNull(iGeqfeccreacion)) entity.Geqfeccreacion = dr.GetDateTime(iGeqfeccreacion);

            int iGequsucreacion = dr.GetOrdinal(this.Gequsucreacion);
            if (!dr.IsDBNull(iGequsucreacion)) entity.Gequsucreacion = dr.GetString(iGequsucreacion);

            int iGeqfecmodificacion = dr.GetOrdinal(this.Geqfecmodificacion);
            if (!dr.IsDBNull(iGeqfecmodificacion)) entity.Geqfecmodificacion = dr.GetDateTime(iGeqfecmodificacion);

            int iGequsumodificacion = dr.GetOrdinal(this.Gequsumodificacion);
            if (!dr.IsDBNull(iGequsumodificacion)) entity.Gequsumodificacion = dr.GetString(iGequsumodificacion);

            int iGeqactivo = dr.GetOrdinal(this.Geqactivo);
            if (!dr.IsDBNull(iGeqactivo)) entity.Geqactivo = Convert.ToInt32(dr.GetValue(iGeqactivo));

            return entity;
        }

        #region Mapeo de Campos

        public string Geqcodi = "GEQCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Geqfeccreacion = "GEQFECCREACION";
        public string Gequsucreacion = "GEQUSUCREACION";
        public string Geqfecmodificacion = "GEQFECMODIFICACION";
        public string Gequsumodificacion = "GEQUSUMODIFICACION";
        public string Geqactivo = "GEQACTIVO";

        public string Grupoabrev = "GRUPOABREV";
        public string Emprcodi = "EMPRCODI";
        public string Equinomb = "EQUINOMB";
        public string Equiestado = "EQUIESTADO";
        public string Equiabrev = "EQUIABREV";
        public string Grupotipomodo = "GRUPOTIPOMODO";
        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string GrupoEstado = "GRUPOESTADO";
        public string Gruponomb = "GRUPONOMB";
        public string Equipadre = "EQUIPADRE";
        public string Osinergcodi = "OSINERGCODI";

        public string Fenergcodi = "FENERGCODI";
        public string Fenergnomb = "FENERGNOMB";

        #endregion
    }
}
