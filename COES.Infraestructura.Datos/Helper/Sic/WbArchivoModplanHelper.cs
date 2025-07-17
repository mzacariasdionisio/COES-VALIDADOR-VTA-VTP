using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_ARCHIVO_MODPLAN
    /// </summary>
    public class WbArchivoModplanHelper : HelperBase
    {
        public WbArchivoModplanHelper(): base(Consultas.WbArchivoModplanSql)
        {
        }

        public WbArchivoModplanDTO Create(IDataReader dr)
        {
            WbArchivoModplanDTO entity = new WbArchivoModplanDTO();

            int iArcmplcodi = dr.GetOrdinal(this.Arcmplcodi);
            if (!dr.IsDBNull(iArcmplcodi)) entity.Arcmplcodi = Convert.ToInt32(dr.GetValue(iArcmplcodi));

            int iVermplcodi = dr.GetOrdinal(this.Vermplcodi);
            if (!dr.IsDBNull(iVermplcodi)) entity.Vermplcodi = Convert.ToInt32(dr.GetValue(iVermplcodi));

            int iArcmplnombre = dr.GetOrdinal(this.Arcmplnombre);
            if (!dr.IsDBNull(iArcmplnombre)) entity.Arcmplnombre = dr.GetString(iArcmplnombre);

            int iArcmplindtc = dr.GetOrdinal(this.Arcmplindtc);
            if (!dr.IsDBNull(iArcmplindtc)) entity.Arcmplindtc = dr.GetString(iArcmplindtc);

            int iArcmplestado = dr.GetOrdinal(this.Arcmplestado);
            if (!dr.IsDBNull(iArcmplestado)) entity.Arcmplestado = dr.GetString(iArcmplestado);

            int iArcmplext = dr.GetOrdinal(this.Arcmplext);
            if (!dr.IsDBNull(iArcmplext)) entity.Arcmplext = dr.GetString(iArcmplext);

            int iArcmpltipo = dr.GetOrdinal(this.Arcmpltipo);
            if (!dr.IsDBNull(iArcmpltipo)) entity.Arcmpltipo = Convert.ToInt32(dr.GetValue(iArcmpltipo));

            int iArcmpldesc = dr.GetOrdinal(this.Arcmpldesc);
            if (!dr.IsDBNull(iArcmpldesc)) entity.Arcmpldesc = dr.GetString(iArcmpldesc);

            return entity;
        }


        #region Mapeo de Campos

        public string Arcmplcodi = "ARCMPLCODI";
        public string Vermplcodi = "VERMPLCODI";
        public string Arcmplnombre = "ARCMPLNOMBRE";
        public string Arcmplindtc = "ARCMPLINDTC";
        public string Arcmplestado = "ARCMPLESTADO";
        public string Arcmplext = "ARCMPLEXT";
        public string Arcmpltipo = "ARCMPLTIPO";
        public string Arcmpldesc = "ARCMPLDESC";

        #endregion

        public string SqlObtenerDocumento
        {
            get { return base.GetSqlXml("ObtenerDocumento"); }
        }
    }
}
