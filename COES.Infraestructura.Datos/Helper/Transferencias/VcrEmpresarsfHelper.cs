using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_EMPRESARSF
    /// </summary>
    public class VcrEmpresarsfHelper : HelperBase
    {
        public VcrEmpresarsfHelper(): base(Consultas.VcrEmpresarsfSql)
        {
        }

        public VcrEmpresarsfDTO Create(IDataReader dr)
        {
            VcrEmpresarsfDTO entity = new VcrEmpresarsfDTO();

            int iVcersfcodi = dr.GetOrdinal(this.Vcersfcodi);
            if (!dr.IsDBNull(iVcersfcodi)) entity.Vcersfcodi = Convert.ToInt32(dr.GetValue(iVcersfcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iVcersfresvnosumins = dr.GetOrdinal(this.Vcersfresvnosumins);
            if (!dr.IsDBNull(iVcersfresvnosumins)) entity.Vcersfresvnosumins = dr.GetDecimal(iVcersfresvnosumins);

            int iVcersftermsuperavit = dr.GetOrdinal(this.Vcersftermsuperavit);
            if (!dr.IsDBNull(iVcersftermsuperavit)) entity.Vcersftermsuperavit = dr.GetDecimal(iVcersftermsuperavit);

            int iVcersfcostoportun = dr.GetOrdinal(this.Vcersfcostoportun);
            if (!dr.IsDBNull(iVcersfcostoportun)) entity.Vcersfcostoportun = dr.GetDecimal(iVcersfcostoportun);

            int iVcersfcompensacion = dr.GetOrdinal(this.Vcersfcompensacion);
            if (!dr.IsDBNull(iVcersfcompensacion)) entity.Vcersfcompensacion = dr.GetDecimal(iVcersfcompensacion);

            int iVcersfasignreserva = dr.GetOrdinal(this.Vcersfasignreserva);
            if (!dr.IsDBNull(iVcersfasignreserva)) entity.Vcersfasignreserva = dr.GetDecimal(iVcersfasignreserva);

            int iVcersfpagoincumpl = dr.GetOrdinal(this.Vcersfpagoincumpl);
            if (!dr.IsDBNull(iVcersfpagoincumpl)) entity.Vcersfpagoincumpl = dr.GetDecimal(iVcersfpagoincumpl);

            int iVcersfpagorsf = dr.GetOrdinal(this.Vcersfpagorsf);
            if (!dr.IsDBNull(iVcersfpagorsf)) entity.Vcersfpagorsf = dr.GetDecimal(iVcersfpagorsf);

            int iVcersfusucreacion = dr.GetOrdinal(this.Vcersfusucreacion);
            if (!dr.IsDBNull(iVcersfusucreacion)) entity.Vcersfusucreacion = dr.GetString(iVcersfusucreacion);

            int iVcersffeccreacion = dr.GetOrdinal(this.Vcersffeccreacion);
            if (!dr.IsDBNull(iVcersffeccreacion)) entity.Vcersffeccreacion = dr.GetDateTime(iVcersffeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcersfcodi = "VCERSFCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Vcersfresvnosumins = "VCERSFRESVNOSUMINS";
        public string Vcersftermsuperavit = "VCERSFTERMSUPERAVIT";
        public string Vcersfcostoportun = "VCERSFCOSTOPORTUN";
        public string Vcersfcompensacion = "VCERSFCOMPENSACION";
        public string Vcersfasignreserva = "VCERSFASIGNRESERVA";
        public string Vcersfpagoincumpl = "VCERSFPAGOINCUMPL";
        public string Vcersfpagorsf = "VCERSFPAGORSF";
        public string Vcersfusucreacion = "VCERSFUSUCREACION";
        public string Vcersffeccreacion = "VCERSFFECCREACION";

        //Atributos para la consulta
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public string SqlGetByIdTotalMes
        {
            get { return base.GetSqlXml("GetByIdTotalMes"); }
        }
    }
}
