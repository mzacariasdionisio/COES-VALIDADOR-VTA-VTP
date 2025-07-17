using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_CIRCUITO
    /// </summary>
    public class EqCircuitoHelper : HelperBase
    {
        public EqCircuitoHelper() : base(Consultas.EqCircuitoSql)
        {
        }

        public EqCircuitoDTO Create(IDataReader dr)
        {
            EqCircuitoDTO entity = new EqCircuitoDTO();

            int iCircfecmodificacion = dr.GetOrdinal(this.Circfecmodificacion);
            if (!dr.IsDBNull(iCircfecmodificacion)) entity.Circfecmodificacion = dr.GetDateTime(iCircfecmodificacion);

            int iCircusumodificacion = dr.GetOrdinal(this.Circusumodificacion);
            if (!dr.IsDBNull(iCircusumodificacion)) entity.Circusumodificacion = dr.GetString(iCircusumodificacion);

            int iCircfeccreacion = dr.GetOrdinal(this.Circfeccreacion);
            if (!dr.IsDBNull(iCircfeccreacion)) entity.Circfeccreacion = dr.GetDateTime(iCircfeccreacion);

            int iCircusucreacion = dr.GetOrdinal(this.Circusucreacion);
            if (!dr.IsDBNull(iCircusucreacion)) entity.Circusucreacion = dr.GetString(iCircusucreacion);

            int iCircestado = dr.GetOrdinal(this.Circestado);
            if (!dr.IsDBNull(iCircestado)) entity.Circestado = Convert.ToInt32(dr.GetValue(iCircestado));

            int iCircnomb = dr.GetOrdinal(this.Circnomb);
            if (!dr.IsDBNull(iCircnomb)) entity.Circnomb = dr.GetString(iCircnomb);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iCircodi = dr.GetOrdinal(this.Circodi);
            if (!dr.IsDBNull(iCircodi)) entity.Circodi = Convert.ToInt32(dr.GetValue(iCircodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Circfecmodificacion = "CIRCFECMODIFICACION";
        public string Circusumodificacion = "CIRCUSUMODIFICACION";
        public string Circfeccreacion = "CIRCFECCREACION";
        public string Circusucreacion = "CIRCUSUCREACION";
        public string Circestado = "CIRCESTADO";
        public string Circnomb = "CIRCNOMB";
        public string Equicodi = "EQUICODI";
        public string Circodi = "CIRCODI";

        public string Famabrev = "FAMABREV";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "Equinomb";
        public string Areanomb = "AREANOMB";

        #endregion

        

        public string SqlGetByEquicodi
        {
            get { return base.GetSqlXml("GetByEquicodi"); }
        }
        
        public string SqlGetByCircodis
        {
            get { return base.GetSqlXml("GetByCircodis"); }
        }
    }
}
