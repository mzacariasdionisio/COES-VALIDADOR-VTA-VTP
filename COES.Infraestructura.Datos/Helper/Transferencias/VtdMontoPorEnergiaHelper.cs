using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTD_VALORIZACION y VTD_VALORIZACIONDETALLE
    /// </summary>
    public class VtdMontoPorEnergiaHelper : HelperBase
    {
        public VtdMontoPorEnergiaHelper(): base(Consultas.VtdValorizacionDetalleSql)
        {
        }
        public VtdMontoPorEnergiaDTO Create(IDataReader dr)
        {
            VtdMontoPorEnergiaDTO entity = new VtdMontoPorEnergiaDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValdretiro = dr.GetOrdinal(this.Valdretiro);
            if (!dr.IsDBNull(iValdretiro)) entity.Valdretiro = Convert.ToDecimal(dr.GetValue(iValdretiro));

            int iValdentrega = dr.GetOrdinal(this.Valdentrega);
            if (!dr.IsDBNull(iValdentrega)) entity.Valdentrega = Convert.ToDecimal(dr.GetValue(iValdentrega));
           
            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Valdretiro = "VALDRETIRO";
        public string Valdentrega = "VALDENTREGA";
        public string Valofecha = "VALOFECHA";

        #endregion
        
        //filtro por rango de fechas -Fit
        public string SqlListByDateRangeMontoPorEnergia
        {
            get { return GetSqlXml("ListByDateRangeMontoPorEnergia"); }
        }
        public string SqlListPageByDateRangeMontoPorEnergia
        {
            get { return GetSqlXml("ListPagedByDateRangeMontoPorEnergia"); }
        }

    }
}
