using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ABI_MEDIDORES_RESUMEN
    /// </summary>
    public class AbiMedidoresResumenHelper : HelperBase
    {
        public AbiMedidoresResumenHelper(): base(Consultas.AbiMedidoresResumenSql)
        {
        }

        public AbiMedidoresResumenDTO Create(IDataReader dr)
        {
            AbiMedidoresResumenDTO entity = new AbiMedidoresResumenDTO();

            int iMregencodi = dr.GetOrdinal(this.Mregencodi);
            if (!dr.IsDBNull(iMregencodi)) entity.Mregencodi = Convert.ToInt32(dr.GetValue(iMregencodi));

            int iMregenfecha = dr.GetOrdinal(this.Mregenfecha);
            if (!dr.IsDBNull(iMregenfecha)) entity.Mregenfecha = dr.GetDateTime(iMregenfecha);

            int iMregentotalsein = dr.GetOrdinal(this.Mregentotalsein);
            if (!dr.IsDBNull(iMregentotalsein)) entity.Mregentotalsein = dr.GetDecimal(iMregentotalsein);

            int iMregentotalexp = dr.GetOrdinal(this.Mregentotalexp);
            if (!dr.IsDBNull(iMregentotalexp)) entity.Mregentotalexp = dr.GetDecimal(iMregentotalexp);

            int iMregentotalimp = dr.GetOrdinal(this.Mregentotalimp);
            if (!dr.IsDBNull(iMregentotalimp)) entity.Mregentotalimp = dr.GetDecimal(iMregentotalimp);

            int iMregentotalnorte = dr.GetOrdinal(this.Mregentotalnorte);
            if (!dr.IsDBNull(iMregentotalnorte)) entity.Mregentotalnorte = dr.GetDecimal(iMregentotalnorte);

            int iMregentotalcentro = dr.GetOrdinal(this.Mregentotalcentro);
            if (!dr.IsDBNull(iMregentotalcentro)) entity.Mregentotalcentro = dr.GetDecimal(iMregentotalcentro);

            int iMregentotalsur = dr.GetOrdinal(this.Mregentotalsur);
            if (!dr.IsDBNull(iMregentotalsur)) entity.Mregentotalsur = dr.GetDecimal(iMregentotalsur);

            int iMregenmdhora = dr.GetOrdinal(this.Mregenmdhora);
            if (!dr.IsDBNull(iMregenmdhora)) entity.Mregenmdhora = dr.GetDateTime(iMregenmdhora);

            int iMregenmdsein = dr.GetOrdinal(this.Mregenmdsein);
            if (!dr.IsDBNull(iMregenmdsein)) entity.Mregenmdsein = dr.GetDecimal(iMregenmdsein);

            int iMregenmdexp = dr.GetOrdinal(this.Mregenmdexp);
            if (!dr.IsDBNull(iMregenmdexp)) entity.Mregenmdexp = dr.GetDecimal(iMregenmdexp);

            int iMregenmdimp = dr.GetOrdinal(this.Mregenmdimp);
            if (!dr.IsDBNull(iMregenmdimp)) entity.Mregenmdimp = dr.GetDecimal(iMregenmdimp);

            int iMregenmdhidro = dr.GetOrdinal(this.Mregenmdhidro);
            if (!dr.IsDBNull(iMregenmdhidro)) entity.Mregenmdhidro = dr.GetDecimal(iMregenmdhidro);

            int iMregenmdtermo = dr.GetOrdinal(this.Mregenmdtermo);
            if (!dr.IsDBNull(iMregenmdtermo)) entity.Mregenmdtermo = dr.GetDecimal(iMregenmdtermo);

            int iMregenmdeolico = dr.GetOrdinal(this.Mregenmdeolico);
            if (!dr.IsDBNull(iMregenmdeolico)) entity.Mregenmdeolico = dr.GetDecimal(iMregenmdeolico);

            int iMregenmdsolar = dr.GetOrdinal(this.Mregenmdsolar);
            if (!dr.IsDBNull(iMregenmdsolar)) entity.Mregenmdsolar = dr.GetDecimal(iMregenmdsolar);

            int iMregenhphora = dr.GetOrdinal(this.Mregenhphora);
            if (!dr.IsDBNull(iMregenhphora)) entity.Mregenhphora = dr.GetDateTime(iMregenhphora);

            int iMregenhpsein = dr.GetOrdinal(this.Mregenhpsein);
            if (!dr.IsDBNull(iMregenhpsein)) entity.Mregenhpsein = dr.GetDecimal(iMregenhpsein);

            int iMregenhpexp = dr.GetOrdinal(this.Mregenhpexp);
            if (!dr.IsDBNull(iMregenhpexp)) entity.Mregenhpexp = dr.GetDecimal(iMregenhpexp);

            int iMregenhpimp = dr.GetOrdinal(this.Mregenhpimp);
            if (!dr.IsDBNull(iMregenhpimp)) entity.Mregenhpimp = dr.GetDecimal(iMregenhpimp);

            int iMregenfhphora = dr.GetOrdinal(this.Mregenfhphora);
            if (!dr.IsDBNull(iMregenfhphora)) entity.Mregenfhphora = dr.GetDateTime(iMregenfhphora);

            int iMregenfhpsein = dr.GetOrdinal(this.Mregenfhpsein);
            if (!dr.IsDBNull(iMregenfhpsein)) entity.Mregenfhpsein = dr.GetDecimal(iMregenfhpsein);

            int iMregenfhpexp = dr.GetOrdinal(this.Mregenfhpexp);
            if (!dr.IsDBNull(iMregenfhpexp)) entity.Mregenfhpexp = dr.GetDecimal(iMregenfhpexp);

            int iMregenfhpimp = dr.GetOrdinal(this.Mregenfhpimp);
            if (!dr.IsDBNull(iMregenfhpimp)) entity.Mregenfhpimp = dr.GetDecimal(iMregenfhpimp);

            int iMregenmdnoiihora = dr.GetOrdinal(this.Mregenmdnoiihora);
            if (!dr.IsDBNull(iMregenmdnoiihora)) entity.Mregenmdnoiihora = dr.GetDateTime(iMregenmdnoiihora);

            int iMregenmdnoiisein = dr.GetOrdinal(this.Mregenmdnoiisein);
            if (!dr.IsDBNull(iMregenmdnoiisein)) entity.Mregenmdnoiisein = dr.GetDecimal(iMregenmdnoiisein);

            return entity;
        }


        #region Mapeo de Campos

        public string Mregencodi = "MREGENCODI";
        public string Mregenfecha = "MREGENFECHA";
        public string Mregentotalsein = "MREGENTOTALSEIN";
        public string Mregentotalexp = "MREGENTOTALEXP";
        public string Mregentotalimp = "MREGENTOTALIMP";
        public string Mregentotalnorte = "MREGENTOTALNORTE";
        public string Mregentotalcentro = "MREGENTOTALCENTRO";
        public string Mregentotalsur = "MREGENTOTALSUR";
        public string Mregenmdhora = "MREGENMDHORA";
        public string Mregenmdsein = "MREGENMDSEIN";
        public string Mregenmdexp = "MREGENMDEXP";
        public string Mregenmdimp = "MREGENMDIMP";
        public string Mregenmdhidro = "MREGENMDHIDRO";
        public string Mregenmdtermo = "MREGENMDTERMO";
        public string Mregenmdeolico = "MREGENMDEOLICO";
        public string Mregenmdsolar = "MREGENMDSOLAR";
        public string Mregenmdnoiihora = "MREGENMDNOIIHORA";
        public string Mregenmdnoiisein = "MREGENMDNOIISEIN";

        public string Mregenhphora = "MREGENHPHORA";
        public string Mregenhpsein = "MREGENHPSEIN";
        public string Mregenhpexp = "MREGENHPEXP";
        public string Mregenhpimp = "MREGENHPIMP";
        public string Mregenfhphora = "MREGENFHPHORA";
        public string Mregenfhpsein = "MREGENFHPSEIN";
        public string Mregenfhpexp = "MREGENFHPEXP";
        public string Mregenfhpimp = "MREGENFHPIMP";

        #endregion
    }
}
