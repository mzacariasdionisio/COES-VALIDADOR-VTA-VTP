using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_SUBCAUSAEVENTO
    /// </summary>
    public class EveSubcausaeventoHelper : HelperBase
    {
        public EveSubcausaeventoHelper(): base(Consultas.EveSubcausaeventoSql)
        {
        }

        public EveSubcausaeventoDTO Create(IDataReader dr)
        {
            EveSubcausaeventoDTO entity = new EveSubcausaeventoDTO();

            int iSubcausadesc = dr.GetOrdinal(this.Subcausadesc);
            if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

            int iSubcausaabrev = dr.GetOrdinal(this.Subcausaabrev);
            if (!dr.IsDBNull(iSubcausaabrev)) entity.Subcausaabrev = dr.GetString(iSubcausaabrev);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iCausaevencodi = dr.GetOrdinal(this.Causaevencodi);
            if (!dr.IsDBNull(iCausaevencodi)) entity.Causaevencodi = Convert.ToInt32(dr.GetValue(iCausaevencodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Subcausadesc = "SUBCAUSADESC";
        public string Subcausaabrev = "SUBCAUSAABREV";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Causaevencodi = "CAUSAEVENCODI";
        public string Tipoevencodi = "TIPOEVENCODI";

        #region PR5
        public string SubcausaCmg = "SUBCAUSACMG";
        #endregion

	    #endregion

        public string SqlObtenerPorCausa
        {
            get { return base.GetSqlXml("ObtenerPorCausa");}
        }

        public string SqlObtenerSubcausaEvento
        {
            get { return base.GetSqlXml("ObtenerPorCausaEvento"); }
        }

        // Inicio de Agregado - Sistema de Compensaciones
        public string SqlListTipoOperacion
        {
            get { return base.GetSqlXml("ListTipoOperacion"); }
        }

        public string SqlGetSubCausaCodi
        {
            get { return base.GetSqlXml("GetSubCausaCodi"); }
        }
        // Fin de Agregado - Sistema de Compensaciones
        public string SqlListSubCausaEvento
        {
            get { return base.GetSqlXml("ListSubCausaEvento"); }
        }
        #region PR5
        public string SqlObtenerXCausaXCmg
        {
            get { return base.GetSqlXml("ObtenerXCausaXCmg"); }
        }

        public string SqlObtenerXCausaXID
        {
            get { return base.GetSqlXml("ObtenerXCausaXID"); }
        }
        #endregion
        
        public string SqlListarTipoOperacionHO
        {
            get { return base.GetSqlXml("ListarTipoOperacionHO"); }
        }

        #region INTERVENCIONES
        public string SqlListarComboCausasMantenimiento
        {
            get { return base.GetSqlXml("ListarComboCausasMantenimiento"); }
        }

        public string SqlListarComboCausasConsulta
        {
            get { return base.GetSqlXml("ListarComboCausasConsulta"); }
        }
        #endregion

        public string SqlObtenerSubcausaEventoByAreausuaria
        {
            get { return base.GetSqlXml("ObtenerSubcausaEventoByAreausuaria"); }
        }

        #region FIT - VALORIZACION DIARIA
        public string GetCodByAbrev
        {
            get { return GetSqlXml("GetCodByAbrev"); }
        }
        #endregion

        #region PRONOSTICO DEMANDA

        public string SqlUpdateBy
        {
            get { return base.GetSqlXml("UpdateBy"); }
        }
        #endregion

        #region SIOSEIN
        public string SqlGetListByIds
        {
            get { return GetSqlXml("GetListByIds"); }
        }
        #endregion
    }
}

