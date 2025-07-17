using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_CIRCUITO_DET
    /// </summary>
    public class EqCircuitoDetHelper : HelperBase
    {
        public EqCircuitoDetHelper() : base(Consultas.EqCircuitoDetSql)
        {
        }

        public EqCircuitoDetDTO Create(IDataReader dr)
        {
            EqCircuitoDetDTO entity = new EqCircuitoDetDTO();

            int iCircdtagrup = dr.GetOrdinal(this.Circdtagrup);
            if (!dr.IsDBNull(iCircdtagrup)) entity.Circdtagrup = Convert.ToInt32(dr.GetValue(iCircdtagrup));

            int iCircdtestado = dr.GetOrdinal(this.Circdtestado);
            if (!dr.IsDBNull(iCircdtestado)) entity.Circdtestado = Convert.ToInt32(dr.GetValue(iCircdtestado));

            int iCircdtfecvigencia = dr.GetOrdinal(this.Circdtfecvigencia);
            if (!dr.IsDBNull(iCircdtfecvigencia)) entity.Circdtfecvigencia = dr.GetDateTime(iCircdtfecvigencia);

            int iCircdtfecmodificacion = dr.GetOrdinal(this.Circdtfecmodificacion);
            if (!dr.IsDBNull(iCircdtfecmodificacion)) entity.Circdtfecmodificacion = dr.GetDateTime(iCircdtfecmodificacion);

            int iCircdtusumodificacion = dr.GetOrdinal(this.Circdtusumodificacion);
            if (!dr.IsDBNull(iCircdtusumodificacion)) entity.Circdtusumodificacion = dr.GetString(iCircdtusumodificacion);

            int iCircdtfeccreacion = dr.GetOrdinal(this.Circdtfeccreacion);
            if (!dr.IsDBNull(iCircdtfeccreacion)) entity.Circdtfeccreacion = dr.GetDateTime(iCircdtfeccreacion);

            int iCircdtusucreacion = dr.GetOrdinal(this.Circdtusucreacion);
            if (!dr.IsDBNull(iCircdtusucreacion)) entity.Circdtusucreacion = dr.GetString(iCircdtusucreacion);

            int iCircodi = dr.GetOrdinal(this.Circodi);
            if (!dr.IsDBNull(iCircodi)) entity.Circodi = Convert.ToInt32(dr.GetValue(iCircodi));

            int iEquicodihijo = dr.GetOrdinal(this.Equicodihijo);
            if (!dr.IsDBNull(iEquicodihijo)) entity.Equicodihijo = Convert.ToInt32(dr.GetValue(iEquicodihijo));

            int iCircdtcodi = dr.GetOrdinal(this.Circdtcodi);
            if (!dr.IsDBNull(iCircdtcodi)) entity.Circdtcodi = Convert.ToInt32(dr.GetValue(iCircdtcodi));

            int iCircodihijo = dr.GetOrdinal(this.Circodihijo);
            if (!dr.IsDBNull(iCircodihijo)) entity.Circodihijo = Convert.ToInt32(dr.GetValue(iCircodihijo));

            return entity;
        }

        #region Mapeo de Campos

        public string Circdtfecvigencia = "CIRCDTFECVIGENCIA";
        public string Circdtagrup = "CIRCDTAGRUP";
        public string Circdtestado = "CIRCDTESTADO";
        public string Circdtfecmodificacion = "CIRCDTFECMODIFICACION";
        public string Circdtusumodificacion = "CIRCDTUSUMODIFICACION";
        public string Circdtfeccreacion = "CIRCDTFECCREACION";
        public string Circdtusucreacion = "CIRCDTUSUCREACION";
        public string Circodi = "CIRCODI";
        public string Equicodihijo = "EQUICODIHIJO";
        public string Circdtcodi = "CIRCDTCODI";
        public string Circodihijo = "CIRCODIHIJO";

        public string Circnombhijo = "CIRCNOMBHIJO";
        public string Equinombhijo = "EquinombHIJO";
        public string Emprnombequihijo = "Emprnombequihijo";
        public string Emprnombcirchijo = "Emprnombcirchijo";

        public string EquicodiAsociado = "EquicodiAsociado";

        #endregion

        #region MCP

        public string SqlListarPorCircodis
        {
            get { return base.GetSqlXml("ListarPorCircodis"); }
        }

        public string SqlListarDependientesAgrupados
        {
            get { return base.GetSqlXml("ListarEquiposAgrupados"); }
        }

        #endregion
    }
}
