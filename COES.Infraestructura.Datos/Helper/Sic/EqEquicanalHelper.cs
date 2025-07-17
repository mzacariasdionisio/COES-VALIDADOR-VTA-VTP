using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_EQUICANAL
    /// </summary>
    public class EqEquicanalHelper : HelperBase
    {
        public EqEquicanalHelper()
            : base(Consultas.EqEquicanalSql)
        {
        }

        public EqEquicanalDTO Create(IDataReader dr)
        {
            EqEquicanalDTO entity = new EqEquicanalDTO();

            int iEcancodi = dr.GetOrdinal(this.Ecancodi);
            if (!dr.IsDBNull(iEcancodi)) entity.Ecancodi = Convert.ToInt32(dr.GetValue(iEcancodi));

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iEcanestado = dr.GetOrdinal(this.Ecanestado);
            if (!dr.IsDBNull(iEcanestado)) entity.Ecanestado = dr.GetString(iEcanestado);

            int iEcanfactor = dr.GetOrdinal(this.Ecanfactor);
            if (!dr.IsDBNull(iEcanfactor)) entity.Ecanfactor = dr.GetDecimal(iEcanfactor);

            int iEcanusucreacion = dr.GetOrdinal(this.Ecanusucreacion);
            if (!dr.IsDBNull(iEcanusucreacion)) entity.Ecanusucreacion = dr.GetString(iEcanusucreacion);

            int iEcanfecmodificacion = dr.GetOrdinal(this.Ecanfecmodificacion);
            if (!dr.IsDBNull(iEcanfecmodificacion)) entity.Ecanfecmodificacion = dr.GetDateTime(iEcanfecmodificacion);

            int iEcanusumodificacion = dr.GetOrdinal(this.Ecanusumodificacion);
            if (!dr.IsDBNull(iEcanusumodificacion)) entity.Ecanusumodificacion = dr.GetString(iEcanusumodificacion);

            int iEcanfeccreacion = dr.GetOrdinal(this.Ecanfeccreacion);
            if (!dr.IsDBNull(iEcanfeccreacion)) entity.Ecanfeccreacion = dr.GetDateTime(iEcanfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Ecancodi = "ECANCODI";
        public string Areacode = "AREACODE";
        public string Canalcodi = "CANALCODI";
        public string Equicodi = "EQUICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ecanestado = "ECANESTADO";
        public string Ecanfactor = "ECANFACTOR";
        public string Ecanusucreacion = "ECANUSUCREACION";
        public string Ecanfecmodificacion = "ECANFECMODIFICACION";
        public string Ecanusumodificacion = "ECANUSUMODIFICACION";
        public string Ecanfeccreacion = "ECANFECCREACION";

        public string Equipadre = "EQUIPADRE";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Famcodi = "Famcodi";
        public string Famnomb = "Famnomb";
        public string Famabrev = "Famabrev";
        public string Tipoinfoabrev = "TIPOINFOABREV";

        public string Canalnomb = "CANALNOMB";
        public string Canalabrev = "CANALABREV";
        public string Zonanomb = "ZONANOMB";

        public string Areaabrev = "Areaabrev";
        public string Areadesc = "AREADESC";

        #endregion

        #region Mapeo de Querys

        public string SqlListarEquivalencia
        {
            get { return base.GetSqlXml("ListarEquivalencia"); }
        }

        #endregion
    }
}
