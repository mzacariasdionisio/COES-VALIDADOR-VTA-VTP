using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTD_MONTOCAPACIDAD
    /// </summary>
    public class VtdMontoPorCapacidadHelper : HelperBase
    {
        public VtdMontoPorCapacidadHelper(): base(Consultas.VtdValorizacionDetalleSql)
        {
        }

        public VtdMontoPorCapacidadDTO Create(IDataReader dr)
        {
            VtdMontoPorCapacidadDTO entity = new VtdMontoPorCapacidadDTO();


            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValdpfirremun = dr.GetOrdinal(this.Valdpfirremun);
            if (!dr.IsDBNull(iValdpfirremun)) entity.Valdpfirremun = Convert.ToDecimal(dr.GetValue(iValdpfirremun));

            int iValomr = dr.GetOrdinal(this.Valomr);
            if (!dr.IsDBNull(iValomr)) entity.Valomr = Convert.ToDecimal(dr.GetValue(iValomr));

            int iValopreciopotencia = dr.GetOrdinal(this.Valopreciopotencia);
            if (!dr.IsDBNull(iValopreciopotencia)) entity.Valopreciopotencia = Convert.ToDecimal(dr.GetValue(iValopreciopotencia));

            int iValddemandacoincidente = dr.GetOrdinal(this.Valddemandacoincidente);
            if (!dr.IsDBNull(iValddemandacoincidente)) entity.Valddemandacoincidente = Convert.ToDecimal(dr.GetValue(iValddemandacoincidente));

            int iValdmoncapacidad = dr.GetOrdinal(this.Valdmoncapacidad);
            if (!dr.IsDBNull(iValdmoncapacidad)) entity.Valdmoncapacidad = Convert.ToDecimal(dr.GetValue(iValdmoncapacidad));

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Valdpfirremun = "VALDPFIRREMUN";
        public string Valomr = "VALOMR";
        public string Valddemandacoincidente = "VALDDEMANDACOINCIDENTE";
        public string Valopreciopotencia = "VALOPRECIOPOTENCIA";
        public string Valdmoncapacidad = "VALDMONCAPACIDAD";
        public string Valofecha = "VALOFECHA";
        #endregion
        
        //filtro por rango de fechas -Fit
        public string SqlListByDateRange
        {
            get { return GetSqlXml("ListByDateRangeMontoPorCapacidad"); }
        }
        public string SqlListPageByDateRange
        {
            get { return GetSqlXml("ListPagedByDateRangeMontoPorCapacidad"); }
        }
    }
}
