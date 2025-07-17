using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VtdMontoPorPeaje
    /// </summary>
    public class VtdMontoPorPeajeHelper : HelperBase
    {
        public VtdMontoPorPeajeHelper(): base(Consultas.VtdValorizacionDetalleSql)
        {
        }

        public VtdMontoPorPeajeDTO Create(IDataReader dr)
        {
            VtdMontoPorPeajeDTO entity = new VtdMontoPorPeajeDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValddemandacoincidente = dr.GetOrdinal(this.Valddemandacoincidente);
            if (!dr.IsDBNull(iValddemandacoincidente)) entity.Valddemandacoincidente = Convert.ToDecimal(dr.GetValue(iValddemandacoincidente));

            int iValdpeajeuni = dr.GetOrdinal(this.Valdpeajeuni);
            if (!dr.IsDBNull(iValdpeajeuni)) entity.Valdpeajeuni = Convert.ToDecimal(dr.GetValue(iValdpeajeuni));

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha); 

            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Valddemandacoincidente = "VALDDEMANDACOINCIDENTE";
        public string Valdpeajeuni = "VALDPEAJEUNI";
        public string Valofecha = "VALOFECHA";

        #endregion
        //filtro por rango de fechas -Fit
        public string SqlListByDateRange
        {
            get { return GetSqlXml("ListByDateRangeMontoPorPeaje"); }
        }
        public string SqlListPageByDateRange
        {
            get { return GetSqlXml("ListPagedByDateRangeMontoPorPeaje"); }
        }
    }
}
