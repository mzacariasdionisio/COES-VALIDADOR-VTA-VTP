using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_EVENCLASE
    /// </summary>
    public class EveEvenclaseHelper : HelperBase
    {
        public EveEvenclaseHelper(): base(Consultas.EveEvenclaseSql)
        {
        }

        public EveEvenclaseDTO Create(IDataReader dr)
        {
            EveEvenclaseDTO entity = new EveEvenclaseDTO();

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iEvenclasedesc = dr.GetOrdinal(this.Evenclasedesc);
            if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iEvenclasetipo = dr.GetOrdinal(this.Evenclasetipo);
            if (!dr.IsDBNull(iEvenclasetipo)) entity.Evenclasetipo = dr.GetString(iEvenclasetipo);

            int iEvenclaseabrev = dr.GetOrdinal(this.Evenclaseabrev);
            if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);


            return entity;
        }


        #region Mapeo de Campos

        public string Evenclasecodi = "EVENCLASECODI";
        public string Evenclasedesc = "EVENCLASEDESC";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Evenclasetipo = "EVENCLASETIPO";
        public string Evenclaseabrev = "EVENCLASEABREV";

        #endregion


        #region INTERVENCIONES
        //--------------------------------------------------------------------------------
        // ASSETEC.SGH - 18/12/2017: FUNCIONES PERSONALIZADAS PARA TIPOS DE PROGRAMACIONES
        //--------------------------------------------------------------------------------
        public string SqlListarComboTiposProgramacionesMantenimiento
        {
            get { return base.GetSqlXml("ListarComboTiposProgramacionesMantenimiento"); }
        }

        public string SqlListarComboTiposProgramacionesConsulta
        {
            get { return base.GetSqlXml("ListarComboTiposProgramacionesConsulta"); }
        }

        public string SqlListarComboTiposProgramacionesReporte
        {
            get { return base.GetSqlXml("ListarComboTiposProgramacionesReporte"); }
        }

        public string SqlListarComboTiposProgramacionesConsultaCruzadas
        {
            get { return base.GetSqlXml("ListarComboTiposProgramacionesConsultaCruzadas"); }
        }

        public string SqlListarComboTiposProgramacionesReporteIntervencionesMayores
        {
            get { return base.GetSqlXml("ListarComboTiposProgramacionesReporteIntervencionesMayores"); }
        }

        #endregion

    }
}

