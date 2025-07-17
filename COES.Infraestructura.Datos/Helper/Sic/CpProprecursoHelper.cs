using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_PROPRECURSO
    /// </summary>
    public class CpProprecursoHelper : HelperBase
    {
        public CpProprecursoHelper() : base(Consultas.CpProprecursoSql)
        {
        }

        public CpProprecursoDTO Create(IDataReader dr)
        {
            CpProprecursoDTO entity = new CpProprecursoDTO();

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iPropcodi = dr.GetOrdinal(this.Propcodi);
            if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

            int iVariaccodi = dr.GetOrdinal(this.Variaccodi);
            if (!dr.IsDBNull(iVariaccodi)) entity.Variaccodi = Convert.ToInt32(dr.GetValue(iVariaccodi));

            int iFechaproprecur = dr.GetOrdinal(this.Fechaproprecur);
            if (!dr.IsDBNull(iFechaproprecur)) entity.Fechaproprecur = dr.GetDateTime(iFechaproprecur);

            int iValor = dr.GetOrdinal(this.Valor);
            if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetString(iValor);

            return entity;
        }


        #region Mapeo de Campos

        public string Recurcodi = "RECURCODI";
        public string Topcodi = "TOPCODI";
        public string Propcodi = "PROPCODI";
        public string Variaccodi = "VARIACCODI";
        public string Fechaproprecur = "FECHAPROPRECUR";
        
        #region Yupana Continuo
        public string Catcodi = "Catcodi";
        public string Proporden = "PROPORDEN";

        public string GetSqlListarPropiedadxRecurso2
        {
            get { return base.GetSqlXml("ListarPropiedadxRecurso2"); }
        }

        public string SqlListarPropiedadxRecursoToGams
        {
            get { return base.GetSqlXml("ListarPropiedadxRecursoToGams"); }
        }

        public string GetSqlListarPropiedadxRecurso
        {
            get { return base.GetSqlXml("ListarPropiedadxRecurso"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }
        #endregion

        #endregion
    }
}
