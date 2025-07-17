using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_TIPOEVENTO
    /// </summary>
    public class EveTipoeventoHelper : HelperBase
    {
        public EveTipoeventoHelper(): base(Consultas.EveTipoeventoSql)
        {
        }

        public EveTipoeventoDTO Create(IDataReader dr)
        {
            EveTipoeventoDTO entity = new EveTipoeventoDTO();

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iTipoevendesc = dr.GetOrdinal(this.Tipoevendesc);
            if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iTipoevenabrev = dr.GetOrdinal(this.Tipoevenabrev);
            if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

            int iCateevencodi = dr.GetOrdinal(this.Cateevencodi);
            if (!dr.IsDBNull(iCateevencodi)) entity.Cateevencodi = Convert.ToInt32(dr.GetValue(iCateevencodi));

            return entity;
        }
        
        #region Mapeo de Campos
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Tipoevendesc = "TIPOEVENDESC";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Tipoevenabrev = "TIPOEVENABREV";
        public string Cateevencodi = "CATEEVENCODI";
        #endregion

        #region INTERVENCIONES
        public string SqlListarComboTipoIntervencionesMantenimiento
        {
            get { return base.GetSqlXml("ListarComboTipoIntervencionesMantenimiento"); }
        }

        public string SqlListarComboTipoIntervencionesConsulta
        {
            get { return base.GetSqlXml("ListarComboTipoIntervencionesConsulta"); }
        }
        #endregion
    }
}

