using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VtdValorizacion y vtdValorizaciondetalle
    /// </summary>
    public class VtdMontoSCeIOHelper : HelperBase
    {
        public VtdMontoSCeIOHelper() : base(Consultas.VtdValorizacionDetalleSql)
        {
        }
        public VtdMontoSCeIODTO Create(IDataReader dr)
        {
            VtdMontoSCeIODTO entity = new VtdMontoSCeIODTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iValdfpgm = dr.GetOrdinal(this.Valdfpgm);
            if (!dr.IsDBNull(iValdfpgm)) entity.Valdfpgm = Convert.ToDecimal(dr.GetValue(iValdfpgm));

            int iValoporcentajeperdida = dr.GetOrdinal(this.Valoporcentajeperdida);
            if (!dr.IsDBNull(iValoporcentajeperdida)) entity.Valoporcentajeperdida = Convert.ToDecimal(dr.GetValue(iValoporcentajeperdida));

            int iValdfactorp = dr.GetOrdinal(this.Valdfactorp);
            if (!dr.IsDBNull(iValdfactorp)) entity.Valdfactorp = Convert.ToDecimal(dr.GetValue(iValdfactorp));

            int iValdmcio = dr.GetOrdinal(this.valdmcio);
            if (!dr.IsDBNull(iValdmcio)) entity.Valdmcio = Convert.ToDecimal(dr.GetValue(iValdmcio));

            int iValdpdsc = dr.GetOrdinal(this.Valdpdsc);
            if (!dr.IsDBNull(iValdpdsc)) entity.Valdpdsc = Convert.ToDecimal(dr.GetValue(iValdpdsc));

            int iValdra = dr.GetOrdinal(this.Valoco);
            if (!dr.IsDBNull(iValdra)) entity.Valoco = Convert.ToDecimal(dr.GetValue(iValdra));

            int iValora = dr.GetOrdinal(this.Valora);
            if (!dr.IsDBNull(iValora)) entity.Valora = Convert.ToDecimal(dr.GetValue(iValora));

            int iValoraSub = dr.GetOrdinal(this.ValoraSub);
            if (!dr.IsDBNull(iValoraSub)) entity.ValoraSub = Convert.ToDecimal(dr.GetValue(iValoraSub));

            int iValoraBaj = dr.GetOrdinal(this.ValoraBaj);
            if (!dr.IsDBNull(iValoraBaj)) entity.ValoraBaj = Convert.ToDecimal(dr.GetValue(iValoraBaj));

            int iValodemandacoes = dr.GetOrdinal(this.Valodemandacoes);
            if (!dr.IsDBNull(iValodemandacoes)) entity.Valodemandacoes = Convert.ToDecimal(dr.GetValue(iValodemandacoes));

            int iValofactorreparto = dr.GetOrdinal(this.Valofactorreparto);
            if (!dr.IsDBNull(iValofactorreparto)) entity.Valofactorreparto = Convert.ToDecimal(dr.GetValue(iValofactorreparto));

            int iValdcompcostosoper = dr.GetOrdinal(this.Valocompcostosoper);
            if (!dr.IsDBNull(iValdcompcostosoper)) entity.Valocompcostosoper = Convert.ToDecimal(dr.GetValue(iValdcompcostosoper));

            int iValdofmax = dr.GetOrdinal(this.Valoofmax);
            if (!dr.IsDBNull(iValdofmax)) entity.Valoofmax = Convert.ToDecimal(dr.GetValue(iValdofmax));

            int iValdofmaxbaj = dr.GetOrdinal(this.ValoofmaxBaj);
            if (!dr.IsDBNull(iValdofmaxbaj)) entity.ValoofmaxBaj = Convert.ToDecimal(dr.GetValue(iValdofmaxbaj));

            int iValdpagoio = dr.GetOrdinal(this.Valdpagoio);
            if (!dr.IsDBNull(iValdpagoio)) entity.Valdpagoio = Convert.ToDecimal(dr.GetValue(iValdpagoio));

            int iValdpagosc = dr.GetOrdinal(this.Valdpagosc);
            if (!dr.IsDBNull(iValdpagosc)) entity.Valdpagosc = Convert.ToDecimal(dr.GetValue(iValdpagosc));

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            return entity;
        }
        #region Mapeo de Campos
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Valdfpgm = "VALDFPGM";
        public string Valoporcentajeperdida = "VALOPORCENTAJEPERDIDA";
        public string Valdfactorp = "VALDFACTORP";
        public string valdmcio = "VALDMCIO";
        public string Valdpdsc = "VALDPDSC";
        public string Valoco = "VALOCO";
        public string Valora = "VALORA";
        public string ValoraSub = "VALORASUB";
        public string ValoraBaj = "VALORABAJ";
        public string Valodemandacoes = "VALODEMANDACOES";
        public string Valofactorreparto = "VALOFACTORREPARTO";
        public string Valocompcostosoper = "VALOCOMPCOSTOSOPER";
        public string Valoofmax = "VALOOFMAX";
        public string ValoofmaxBaj = "VALOOFMAXBAJ";
        public string Valdpagoio = "VALDPAGOIO";
        public string Valdpagosc = "VALDPAGOSC";
        public string Valofecha = "VALOFECHA";

        #endregion
        //filtro por rango de fechas -Fit
        public string SqlListByDateRange
        {
            get { return GetSqlXml("ListByDateRangeMontoSCEIO"); }
        }
        public string SqlListPageByDateRange
        {
            get { return GetSqlXml("ListPagedByDateRangeMontoSCEIO"); }
        }
    }
}
