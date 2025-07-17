using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_DESPACHO_RESUMEN
    /// </summary>
    public class MeDespachoResumenHelper : HelperBase
    {
        public MeDespachoResumenHelper() : base(Consultas.MeDespachoResumenSql)
        {
        }

        public MeDespachoResumenDTO Create(IDataReader dr)
        {
            MeDespachoResumenDTO entity = new MeDespachoResumenDTO();

            int iDregencodi = dr.GetOrdinal(this.Dregencodi);
            if (!dr.IsDBNull(iDregencodi)) entity.Dregencodi = Convert.ToInt32(dr.GetValue(iDregencodi));

            int iDregenfecha = dr.GetOrdinal(this.Dregenfecha);
            if (!dr.IsDBNull(iDregenfecha)) entity.Dregenfecha = dr.GetDateTime(iDregenfecha);

            int iDregentipo = dr.GetOrdinal(this.Dregentipo);
            if (!dr.IsDBNull(iDregentipo)) entity.Dregentipo = Convert.ToInt32(dr.GetValue(iDregentipo));

            int iDregentotalsein = dr.GetOrdinal(this.Dregentotalsein);
            if (!dr.IsDBNull(iDregentotalsein)) entity.Dregentotalsein = dr.GetDecimal(iDregentotalsein);

            int iDregentotalexp = dr.GetOrdinal(this.Dregentotalexp);
            if (!dr.IsDBNull(iDregentotalexp)) entity.Dregentotalexp = dr.GetDecimal(iDregentotalexp);

            int iDregentotalimp = dr.GetOrdinal(this.Dregentotalimp);
            if (!dr.IsDBNull(iDregentotalimp)) entity.Dregentotalimp = dr.GetDecimal(iDregentotalimp);

            int iDregenmdhora = dr.GetOrdinal(this.Dregenmdhora);
            if (!dr.IsDBNull(iDregenmdhora)) entity.Dregenmdhora = dr.GetDateTime(iDregenmdhora);

            int iDregenmdsein = dr.GetOrdinal(this.Dregenmdsein);
            if (!dr.IsDBNull(iDregenmdsein)) entity.Dregenmdsein = dr.GetDecimal(iDregenmdsein);

            int iDregenmdexp = dr.GetOrdinal(this.Dregenmdexp);
            if (!dr.IsDBNull(iDregenmdexp)) entity.Dregenmdexp = dr.GetDecimal(iDregenmdexp);

            int iDregenmdimp = dr.GetOrdinal(this.Dregenmdimp);
            if (!dr.IsDBNull(iDregenmdimp)) entity.Dregenmdimp = dr.GetDecimal(iDregenmdimp);

            int iDregentotalnorte = dr.GetOrdinal(this.Dregentotalnorte);
            if (!dr.IsDBNull(iDregentotalnorte)) entity.Dregentotalnorte = dr.GetDecimal(iDregentotalnorte);

            int iDregentotalcentro = dr.GetOrdinal(this.Dregentotalcentro);
            if (!dr.IsDBNull(iDregentotalcentro)) entity.Dregentotalcentro = dr.GetDecimal(iDregentotalcentro);

            int iDregentotalsur = dr.GetOrdinal(this.Dregentotalsur);
            if (!dr.IsDBNull(iDregentotalsur)) entity.Dregentotalsur = dr.GetDecimal(iDregentotalsur);

            int iDregenmdhidro = dr.GetOrdinal(this.Dregenmdhidro);
            if (!dr.IsDBNull(iDregenmdhidro)) entity.Dregenmdhidro = dr.GetDecimal(iDregenmdhidro);

            int iDregenmdtermo = dr.GetOrdinal(this.Dregenmdtermo);
            if (!dr.IsDBNull(iDregenmdtermo)) entity.Dregenmdtermo = dr.GetDecimal(iDregenmdtermo);

            int iDregenmdeolico = dr.GetOrdinal(this.Dregenmdeolico);
            if (!dr.IsDBNull(iDregenmdeolico)) entity.Dregenmdeolico = dr.GetDecimal(iDregenmdeolico);

            int iDregenmdsolar = dr.GetOrdinal(this.Dregenmdsolar);
            if (!dr.IsDBNull(iDregenmdsolar)) entity.Dregenmdsolar = dr.GetDecimal(iDregenmdsolar);

            int iDregenhphora = dr.GetOrdinal(this.Dregenhphora);
            if (!dr.IsDBNull(iDregenhphora)) entity.Dregenhphora = dr.GetDateTime(iDregenhphora);

            int iDregenhpsein = dr.GetOrdinal(this.Dregenhpsein);
            if (!dr.IsDBNull(iDregenhpsein)) entity.Dregenhpsein = dr.GetDecimal(iDregenhpsein);

            int iDregenhpexp = dr.GetOrdinal(this.Dregenhpexp);
            if (!dr.IsDBNull(iDregenhpexp)) entity.Dregenhpexp = dr.GetDecimal(iDregenhpexp);

            int iDregenhpimp = dr.GetOrdinal(this.Dregenhpimp);
            if (!dr.IsDBNull(iDregenhpimp)) entity.Dregenhpimp = dr.GetDecimal(iDregenhpimp);

            int iDregenfhphora = dr.GetOrdinal(this.Dregenfhphora);
            if (!dr.IsDBNull(iDregenfhphora)) entity.Dregenfhphora = dr.GetDateTime(iDregenfhphora);

            int iDregenfhpsein = dr.GetOrdinal(this.Dregenfhpsein);
            if (!dr.IsDBNull(iDregenfhpsein)) entity.Dregenfhpsein = dr.GetDecimal(iDregenfhpsein);

            int iDregenfhpexp = dr.GetOrdinal(this.Dregenfhpexp);
            if (!dr.IsDBNull(iDregenfhpexp)) entity.Dregenfhpexp = dr.GetDecimal(iDregenfhpexp);

            int iDregenfhpimp = dr.GetOrdinal(this.Dregenfhpimp);
            if (!dr.IsDBNull(iDregenfhpimp)) entity.Dregenfhpimp = dr.GetDecimal(iDregenfhpimp);

            int iDregenmdnoiihora = dr.GetOrdinal(this.Dregenmdnoiihora);
            if (!dr.IsDBNull(iDregenmdnoiihora)) entity.Dregenmdnoiihora = dr.GetDateTime(iDregenmdnoiihora);

            int iDregenmdnoiisein = dr.GetOrdinal(this.Dregenmdnoiisein);
            if (!dr.IsDBNull(iDregenmdnoiisein)) entity.Dregenmdnoiisein = dr.GetDecimal(iDregenmdnoiisein);

            return entity;
        }

        #region Mapeo de Campos

        public string Dregencodi = "DREGENCODI";
        public string Dregenfecha = "DREGENFECHA";
        public string Dregentipo = "DREGENTIPO";
        public string Dregentotalsein = "DREGENTOTALSEIN";
        public string Dregentotalexp = "DREGENTOTALEXP";
        public string Dregentotalimp = "DREGENTOTALIMP";
        public string Dregenmdhora = "DREGENMDHORA";
        public string Dregenmdsein = "DREGENMDSEIN";
        public string Dregenmdexp = "DREGENMDEXP";
        public string Dregenmdimp = "DREGENMDIMP";
        public string Dregentotalnorte = "DREGENTOTALNORTE";
        public string Dregentotalcentro = "DREGENTOTALCENTRO";
        public string Dregentotalsur = "DREGENTOTALSUR";
        public string Dregenmdhidro = "DREGENMDHIDRO";
        public string Dregenmdtermo = "DREGENMDTERMO";
        public string Dregenmdeolico= "DREGENMDEOLICO";
        public string Dregenmdsolar = "DREGENMDSOLAR";
        public string Dregenhphora = "DREGENHPHORA";
        public string Dregenhpsein = "DREGENHPSEIN";
        public string Dregenhpexp = "DREGENHPEXP";
        public string Dregenhpimp = "DREGENHPIMP";
        public string Dregenfhphora = "DREGENFHPHORA";
        public string Dregenfhpsein = "DREGENFHPSEIN";
        public string Dregenfhpexp = "DREGENFHPEXP";
        public string Dregenfhpimp = "DREGENFHPIMP";
        public string Dregenmdnoiihora = "DREGENMDNOIIHORA";
        public string Dregenmdnoiisein = "DREGENMDNOIISEIN";

        #endregion
    }
}
