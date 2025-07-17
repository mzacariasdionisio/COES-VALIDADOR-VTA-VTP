using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpGruporecursoHelper : HelperBase
    {
        public CpGruporecursoHelper() : base(Consultas.CpGruporecursoSql)
        {
        }

        public CpGruporecursoDTO Create(IDataReader dr)
        {
            CpGruporecursoDTO entity = new CpGruporecursoDTO();

            int iGrurecorden = dr.GetOrdinal(this.Grurecorden);
            if (!dr.IsDBNull(iGrurecorden)) entity.Grurecorden = Convert.ToInt32(dr.GetValue(iGrurecorden));

            int iGrurecval4 = dr.GetOrdinal(this.Grurecval4);
            if (!dr.IsDBNull(iGrurecval4)) entity.Grurecval4 = dr.GetDecimal(iGrurecval4);

            int iGrurecval3 = dr.GetOrdinal(this.Grurecval3);
            if (!dr.IsDBNull(iGrurecval3)) entity.Grurecval3 = dr.GetDecimal(iGrurecval3);

            int iGrurecval2 = dr.GetOrdinal(this.Grurecval2);
            if (!dr.IsDBNull(iGrurecval2)) entity.Grurecval2 = dr.GetDecimal(iGrurecval2);

            int iGrurecval1 = dr.GetOrdinal(this.Grurecval1);
            if (!dr.IsDBNull(iGrurecval1)) entity.Grurecval1 = dr.GetDecimal(iGrurecval1);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iRecurcodidet = dr.GetOrdinal(this.Recurcodidet);
            if (!dr.IsDBNull(iRecurcodidet)) entity.Recurcodidet = Convert.ToInt32(dr.GetValue(iRecurcodidet));
            
            return entity;
        }


        #region Mapeo de Campos

        public string Grurecorden = "GRURECORDEN";
        public string Grurecval4 = "GRURECVAL4";
        public string Grurecval3 = "GRURECVAL3";
        public string Grurecval2 = "GRURECVAL2";
        public string Grurecval1 = "GRURECVAL1";
        public string Topcodi = "TOPCODI";
        public string Recurcodi = "RECURCODI";
        public string Recurcodidet = "RECURCODIDET";
        public string Catcodi = "CATCODI";

        //Yupana Continuo
        public string Catcodimain = "CATCODIMAIN";
        public string Catcodisec = "CATCODISEC";
        public string Recurtoescenario = "RECURTOESCENARIO";
        public string Recurconsideragams = "RECURCONSIDERAGAMS";
        public string Recurcodisicoes = "RECURCODIDET";
        #endregion


        public string SqlObtenerRelacionURSSICOES
        {
            get { return base.GetSqlXml("ListRelacionURSSICOES"); }
        }

        //Yupana Continuo
        public string SqlListaGrupoPorCategoria
        {
            get { return base.GetSqlXml("ListaGrupoPorCategoria"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

        public string SqlGetByCriteriaFamilia
        {
            get { return base.GetSqlXml("GetByCriteriaFamilia"); }
        }
    }
}
