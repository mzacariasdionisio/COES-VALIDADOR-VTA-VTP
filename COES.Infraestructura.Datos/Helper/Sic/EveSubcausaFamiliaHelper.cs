using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_SUBCAUSA_FAMILIA
    /// </summary>
    public class EveSubcausaFamiliaHelper : HelperBase
    {
        public EveSubcausaFamiliaHelper(): base(Consultas.EveSubcausaFamiliaSql)
        {
        }

        public EveSubcausaFamiliaDTO Create(IDataReader dr)
        {
            EveSubcausaFamiliaDTO entity = new EveSubcausaFamiliaDTO();

            int iScaufacodi = dr.GetOrdinal(this.Scaufacodi);
            if (!dr.IsDBNull(iScaufacodi)) entity.Scaufacodi = Convert.ToInt32(dr.GetValue(iScaufacodi));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iScaufaeliminado = dr.GetOrdinal(this.Scaufaeliminado);
            if (!dr.IsDBNull(iScaufaeliminado)) entity.Scaufaeliminado = dr.GetString(iScaufaeliminado);

            int iScaufausucreacion = dr.GetOrdinal(this.Scaufausucreacion);
            if (!dr.IsDBNull(iScaufausucreacion)) entity.Scaufausucreacion = dr.GetString(iScaufausucreacion);

            int iScaufafeccreacion = dr.GetOrdinal(this.Scaufafeccreacion);
            if (!dr.IsDBNull(iScaufafeccreacion)) entity.Scaufafeccreacion = dr.GetDateTime(iScaufafeccreacion);

            int iScaufausumodificacion = dr.GetOrdinal(this.Scaufausumodificacion);
            if (!dr.IsDBNull(iScaufausumodificacion)) entity.Scaufausumodificacion = dr.GetString(iScaufausumodificacion);

            int iScaufafecmodificacion = dr.GetOrdinal(this.Scaufafecmodificacion);
            if (!dr.IsDBNull(iScaufafecmodificacion)) entity.Scaufafecmodificacion = dr.GetDateTime(iScaufafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Scaufacodi = "SCAUFACODI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Famcodi = "FAMCODI";
        public string Scaufaeliminado = "SCAUFAELIMINADO";
        public string Scaufausucreacion = "SCAUFAUSUCREACION";
        public string Scaufafeccreacion = "SCAUFAFECCREACION";
        public string Scaufausumodificacion = "SCAUFAUSUMODIFICACION";
        public string Scaufafecmodificacion = "SCAUFAFECMODIFICACION";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Famabrev = "FAMABREV";
        public string Famnomb = "FAMNOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        public string SqlListFamilia
        {
            get { return base.GetSqlXml("ListFamiliaPorSubcausa"); }
        }

        #endregion
    }
}
