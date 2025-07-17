using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GRUPODAT
    /// </summary>
    public class PrGrupodatHelper : HelperBase
    {
        public PrGrupodatHelper()
            : base(Consultas.PrGrupodatSql)
        {
        }

        public PrGrupodatDTO Create(IDataReader dr)
        {
            PrGrupodatDTO entity = new PrGrupodatDTO();

            int iFechadat = dr.GetOrdinal(this.Fechadat);
            if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iFormuladat = dr.GetOrdinal(this.Formuladat);
            if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);
            entity.Formuladat = (entity.Formuladat ?? "").Trim();

            int iDeleted = dr.GetOrdinal(this.Deleted);
            if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));

            int iFechaact = dr.GetOrdinal(this.Fechaact);
            if (!dr.IsDBNull(iFechaact)) entity.Fechaact = dr.GetDateTime(iFechaact);

            int iGdatcomentario = dr.GetOrdinal(this.Gdatcomentario);
            if (!dr.IsDBNull(iGdatcomentario)) entity.Gdatcomentario = dr.GetString(iGdatcomentario);

            int iGdatsustento = dr.GetOrdinal(this.Gdatsustento);
            if (!dr.IsDBNull(iGdatsustento)) entity.Gdatsustento = dr.GetString(iGdatsustento);

            int iGdatcheckcero = dr.GetOrdinal(this.Gdatcheckcero);
            if (!dr.IsDBNull(iGdatcheckcero)) entity.Gdatcheckcero = Convert.ToInt32(dr.GetValue(iGdatcheckcero));

            return entity;
        }


        #region Mapeo de Campos

        public string Fechadat = "FECHADAT";
        public string Concepcodi = "CONCEPCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Lastuser = "LASTUSER";
        public string Formuladat = "FORMULADAT";
        public string Deleted = "DELETED";
        public string Fechaact = "FECHAACT";
        public string Concepabrev = "CONCEPABREV";
        public string Conceppadre = "CONCEPPADRE";
        public string Concepunid = "CONCEPUNID";
        public string Gruponomb = "GRUPONOMB";
        public string Grupocentral = "GRUPOCENTRAL";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";

        public string Gdatcomentario = "GDATCOMENTARIO";
        public string Gdatsustento = "GDATSUSTENTO";
        public string Gdatcheckcero = "GDATCHECKCERO";
        public string Concepocultocomentario = "CONCEPOCULTOCOMENTARIO";

        #region FICHA TÉCNICA
        public string Concepnombficha = "CONCEPNOMBFICHA";
        public string Concepfichatec = "CONCEPFICHATEC";
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Concepdesc = "CONCEPDESC";
        public string Deleted2 = "DELETED2";
        public string Catecodi = "CATECODI";
        public string Conceppropeq = "CONCEPPROPEQ";
        public string Cambio = "CAMBIO";
        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";
        #endregion

        public string Equipadre = "EQUIPADRE";
        public string Central = "CENTRAL";
        public string FlagModoEspecial = "MODO_ESP";
        public string Grupocodimodo = "GRUPOCODIMODO";
        public string Famcodi = "FAMCODI";

        #region SIOSEIN
        public string Osinergcodi = "OSINERGCODI";
        public string Osinergcodi2 = "OSINERGCODI2";
        public string Osinergcodi3 = "OSINERGCODI3";    //SIOSEIN-PRIE-2021
        public string Grupocodi2 = "GRUPOCODI2";
        public string Fenergcodi = "FENERGCODI";
        public string Tipogenerrer = "TIPOGENERRER";
        #endregion

        #endregion

        public string SqlValoresModoOperacionGrupoDat
        {
            get { return base.GetSqlXml("SqlValoresModoOperacionGrupoDat"); }
        }

        public string SqlHistoricoValores
        {
            get { return base.GetSqlXml("SqlHistoricoValores"); }
        }

        #region "COSTO OPORTUNIDAD"
        public string SqlParametrosURSporGrupo
        {
            get { return base.GetSqlXml("ParametrosURSporGrupo"); }
        }

        public string SqlParametrosURSporGrupoHidro
        {
            get { return base.GetSqlXml("ParametrosURSporGrupoHidro"); }
        }
        #endregion

        public string SqlParametrosGeneralesURS
        {
            get { return base.GetSqlXml("ParametrosGeneralesURS"); }
        }

        public string SqlObtenerParametroPorCentral
        {
            get { return base.GetSqlXml("ObtenerParametroPorCentral"); }
        }

        public string SqlObtenerParametroPorConcepto
        {
            get { return base.GetSqlXml("ObtenerParametroPorConcepto"); }
        }

        public string SqlObtenerParametroCurvaConsumo
        {
            get { return base.GetSqlXml("ObtenerParametroCurvaConsumo"); }
        }

        public string SqlObtenerParametroGeneral
        {
            get { return base.GetSqlXml("ObtenerParametroGeneral"); }
        }

        public string SqlObtenerParametroModoOperacion
        {
            get { return base.GetSqlXml("ObtenerParametroModoOperacion"); }
        }

        public string SqlActualizarParametro
        {
            get { return base.GetSqlXml("ActualizarParametro"); }
        }

        // Inicio de Agregado - Sistema de Compensaciones
        public string SqlListaModosOperacion
        {
            get { return base.GetSqlXml("ListaModosOperacion"); }
        }

        public string SqlListaCentral
        {
            get { return base.GetSqlXml("ListaCentral"); }
        }

        public string SqlListaGrupo
        {
            get { return base.GetSqlXml("ListaGrupo"); }
        }

        public string SqlListaModo
        {
            get { return base.GetSqlXml("ListaModo"); }
        }

        public string SqlListaCabecera
        {
            get { return base.GetSqlXml("ListaCabecera"); }
        }

        public string SqlListaCabeceraBody
        {
            get { return base.GetSqlXml("ListaCabeceraBody"); }
        }

        public string SqlGetGrupoCodi
        {
            get { return base.GetSqlXml("GetGrupoCodi"); }
        }

        public string SqlListaModoOperacion
        {
            get { return base.GetSqlXml("ListaModoOperacion"); }
        }

        public string SqlObtenerParametroPorModoOperacionPorFecha
        {
            get { return base.GetSqlXml("ObtenerParametroPorModoOperacionPorFecha"); }
        }
        // Fin de Agregado - Sistema de Compensaciones

        public string SqlValorModoOperacion
        {
            get { return base.GetSqlXml("ValorModoOperacion"); }
        }

        #region NotificacionesCambiosEquipamiento
        public string SqlConceptosModificados
        {
            get { return base.GetSqlXml("ConceptosModificados"); }
        }
        #endregion

        #region Curva Consumo

        public string Curvcodi = "CURVCODI";
        public string GrupocodiNCP = "GRUPOCODINCP";
        public string Curvgrupocodiprincipal = "CURVGRUPOCODIPRINCIPAL";

        public string SqlObtenerParametroCurvaConsumoporGrupoCodi
        {
            get { return base.GetSqlXml("ObtenerParametroCurvaConsumoporGrupoCodi"); }
        }

        public string SqlObtenerParametroCurvaConsumoporFecha
        {
            get { return base.GetSqlXml("ObtenerParametroCurvaConsumoporFecha"); }
        }

        public string SqlObtenerIDCurvaPrincipal
        {
            get { return base.GetSqlXml("ObtenerBuscaIDCurvaPrincipal"); }
        }

        public string SqlObtenerFechaEdicionCurva
        {
            get { return base.GetSqlXml("ObtenerFechaEdicionCurva"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB

        public string SqlParametrosActualizadosPorFecha2
        {
            get { return base.GetSqlXml("ParametrosActualizadosPorFecha2"); }
        }

        public string SqlReporteControlCambios
        {
            get { return base.GetSqlXml("ReporteControlCambios"); }
        }

        #endregion

        public string SqlListaBarraModoOperacion
        {
            get { return base.GetSqlXml("ListaBarraModoOperacion"); }
        }

        #region SGOCOES func A

        public string SqlObtenerListaConfigScoSinac
        {
            get { return base.GetSqlXml("ObtenerListaConfigScoSinac"); }
        }

        public string SqlNroRegListaConfigScoSinac
        {
            get { return base.GetSqlXml("NroRegListaConfigScoSinac"); }
        }

        #endregion SGOCOES func A

        public string Emprnomb = "Emprnomb";
        public string Grupoactivo = "Grupoactivo";
        public string Grupoestado = "Grupoestado";
        public string Areanomb = "Areanomb";

        #region FIT - VALORIZACION DIARIA
        public string SqlObtenerParametroValorizacion
        {
            get { return base.GetSqlXml("ObtenerParametroValorizacion"); }
        }
        #endregion

        #region SIOSEIN2
        public string SqlGetByCriteriaConceptoFecha
        {
            get { return base.GetSqlXml("GetByCriteriaConceptoFecha"); }
        }
        public string SqlObtenerTodoParametroGeneral
        {
            get { return base.GetSqlXml("ObtenerTodoParametroGeneral"); }
        }

        public string SqlObtenerTodoParametroModoOperacion
        {
            get { return base.GetSqlXml("ObtenerTodoParametroModoOperacion"); }
        }
        #endregion

        #region Numerales Datos Base 
        public string Dia = "DIA";
        public string Formula = "FORMULA";


        public string SqlDatosBaseFecha_5_5_x
        {
            get { return base.GetSqlXml("ListaFechasDatosBase_5_5_x"); }
        }
        public string SqlDatosBase_5_5_n
        {
            get { return base.GetSqlXml("ListaDatosBase_5_5_n"); }
        }
        public string SqlDatosBase_5_5_2
        {
            get { return base.GetSqlXml("ListaDatosBase_5_5_2"); }
        }
        public string SqlDatosBase_5_6_4
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_4"); }
        }


        #endregion

        #region Subastas
        public string SqlParametrosConfiguracionPorFecha
        {
            get { return base.GetSqlXml("ParametrosConfiguracionPorFecha"); }
        }

        public string SqlBuscarEliminados
        {
            get { return base.GetSqlXml("BuscarEliminados"); }
        }
        #endregion

        public string SqlListarGrupoConValorModificado
        {
            get { return base.GetSqlXml("ListarGrupoConValorModificado"); }
        }

    }
}
