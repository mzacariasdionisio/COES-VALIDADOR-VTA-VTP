using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VtdMontoPorExceso
    /// </summary>
    public class VtdMontoPorExcesoHelper : HelperBase
    {
        public VtdMontoPorExcesoHelper() : base(Consultas.VtdValorizacionDetalleSql)
        {
        }

        public VtdMontoPorExcesoDTO Create(IDataReader dr)
        {
            VtdMontoPorExcesoDTO entity = new VtdMontoPorExcesoDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValofrectotal = dr.GetOrdinal(this.Valofrectotal);
            if (!dr.IsDBNull(iValofrectotal)) entity.Valofrectotal = Convert.ToDecimal(dr.GetValue(iValofrectotal));

            int iValootrosequipos = dr.GetOrdinal(this.Valootrosequipos);
            if (!dr.IsDBNull(iValootrosequipos)) entity.Valootrosequipos = Convert.ToDecimal(dr.GetValue(iValootrosequipos));

            int iValocostofuerabanda = dr.GetOrdinal(this.Valocostofuerabanda);
            if (!dr.IsDBNull(iValocostofuerabanda)) entity.Valocostofuerabanda = Convert.ToDecimal(dr.GetValue(iValocostofuerabanda));

            int ivalocomptermrt = dr.GetOrdinal(this.Valocomptermrt);
            if (!dr.IsDBNull(ivalocomptermrt)) entity.Valocomptermrt = Convert.ToDecimal(dr.GetValue(ivalocomptermrt));

            int iValocargoconsumo = dr.GetOrdinal(this.Valdcargoconsumo);
            if (!dr.IsDBNull(iValocargoconsumo)) entity.Valdcargoconsumo = Convert.ToDecimal(dr.GetValue(iValocargoconsumo));

            int iValdportesadicional = dr.GetOrdinal(this.Valdaportesadicional);
            if (!dr.IsDBNull(iValdportesadicional)) entity.Valdaportesadicional = Convert.ToDecimal(dr.GetValue(iValdportesadicional));

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            return entity;
        }

        #region Mapeo de Campos
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Valofrectotal = "VALOFRECTOTAL";
        public string Valootrosequipos = "VALOOTROSEQUIPOS";
        public string Valocostofuerabanda = "VALOCOSTOFUERABANDA";
        public string Valocomptermrt = "VALOCOMPTERMRT";
        public string Valdcargoconsumo = "VALDCARGOCONSUMO";
        public string Valdaportesadicional = "VALDAPORTESADICIONAL";
        public string Valofecha = "VALOFECHA";

        #endregion

        //filtro por rango de fechas -Fit
        public string SqlListByDateRange
        {
            get { return GetSqlXml("ListByDateRangeMontoPorExceso"); }
        }
        public string SqlListPageByDateRange
        {
            get { return GetSqlXml("ListPagedByDateRangeMontoPorExceso"); }
        }

    }
}
