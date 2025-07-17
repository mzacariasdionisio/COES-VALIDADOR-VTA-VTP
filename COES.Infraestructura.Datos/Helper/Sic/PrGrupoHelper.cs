using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GRUPO
    /// </summary>
    public class PrGrupoHelper : HelperBase
    {
        public PrGrupoHelper()
            : base(Consultas.PrGrupoSql)
        {
        }

        public PrGrupoDTO Create(IDataReader dr)
        {
            PrGrupoDTO entity = new PrGrupoDTO();

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iBarracodi = dr.GetOrdinal(this.Barracodi);
            if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iGrupoabrev = dr.GetOrdinal(this.Grupoabrev);
            if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

            int iGrupovmax = dr.GetOrdinal(this.Grupovmax);
            if (!dr.IsDBNull(iGrupovmax)) entity.Grupovmax = dr.GetDecimal(iGrupovmax);

            int iGrupovmin = dr.GetOrdinal(this.Grupovmin);
            if (!dr.IsDBNull(iGrupovmin)) entity.Grupovmin = dr.GetDecimal(iGrupovmin);

            int iGrupoorden = dr.GetOrdinal(this.Grupoorden);
            if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iGrupotipo = dr.GetOrdinal(this.Grupotipo);
            if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

            int iCatecodi = dr.GetOrdinal(this.Catecodi);
            if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

            int iGrupotipoc = dr.GetOrdinal(this.Grupotipoc);
            if (!dr.IsDBNull(iGrupotipoc)) entity.Grupotipoc = Convert.ToInt32(dr.GetValue(iGrupotipoc));

            int iGrupopadre = dr.GetOrdinal(this.Grupopadre);
            if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

            int iGrupoactivo = dr.GetOrdinal(this.Grupoactivo);
            if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);

            int iGrupoEstado = dr.GetOrdinal(this.GrupoEstado);
            if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

            int iGrupocomb = dr.GetOrdinal(this.Grupocomb);
            if (!dr.IsDBNull(iGrupocomb)) entity.Grupocomb = dr.GetString(iGrupocomb);

            int iOsicodi = dr.GetOrdinal(this.Osicodi);
            if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

            int iGrupocodi2 = dr.GetOrdinal(this.Grupocodi2);
            if (!dr.IsDBNull(iGrupocodi2)) entity.Grupocodi2 = Convert.ToInt32(dr.GetValue(iGrupocodi2));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iGruposub = dr.GetOrdinal(this.Gruposub);
            if (!dr.IsDBNull(iGruposub)) entity.Gruposub = dr.GetString(iGruposub);

            int iBarracodi2 = dr.GetOrdinal(this.Barracodi2);
            if (!dr.IsDBNull(iBarracodi2)) entity.Barracodi2 = Convert.ToInt32(dr.GetValue(iBarracodi2));

            int iBarramw1 = dr.GetOrdinal(this.Barramw1);
            if (!dr.IsDBNull(iBarramw1)) entity.Barramw1 = dr.GetDecimal(iBarramw1);

            int iBarramw2 = dr.GetOrdinal(this.Barramw2);
            if (!dr.IsDBNull(iBarramw2)) entity.Barramw2 = dr.GetDecimal(iBarramw2);

            int iGruponombncp = dr.GetOrdinal(this.Gruponombncp);
            if (!dr.IsDBNull(iGruponombncp)) entity.Gruponombncp = dr.GetString(iGruponombncp);

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

            int iTipogrupocodi = dr.GetOrdinal(this.Tipogrupocodi);
            if (!dr.IsDBNull(iTipogrupocodi)) entity.Tipogrupocodi = Convert.ToInt32(dr.GetValue(iTipogrupocodi));

            int iGrupointegrante = dr.GetOrdinal(this.Grupointegrante);
            if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

            int iTipoGenerRer = dr.GetOrdinal(this.TipoGenerRER);
            if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

            int iGrupotipocogen = dr.GetOrdinal(this.Grupotipocogen);
            if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

            int iGruponodoenergetico = dr.GetOrdinal(this.GrupoNodoEnergetico);
            if (!dr.IsDBNull(iGruponodoenergetico)) entity.Gruponodoenergetico = Convert.ToInt32(dr.GetValue(iGruponodoenergetico));

            int iGruporeservafria = dr.GetOrdinal(this.GrupoReservaFria);
            if (!dr.IsDBNull(iGruporeservafria)) entity.Gruporeservafria = Convert.ToInt32(dr.GetValue(iGruporeservafria));

            return entity;
        }


        #region Mapeo de Campos

        public string Fenergcodi = "FENERGCODI";
        public string Barracodi = "BARRACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string Grupovmax = "GRUPOVMAX";
        public string Grupovmin = "GRUPOVMIN";
        public string Grupoorden = "GRUPOORDEN";
        public string Emprcodi = "EMPRCODI";
        public string Grupotipo = "GRUPOTIPO";
        public string Catecodi = "CATECODI";
        public string Grupotipoc = "GRUPOTIPOC";
        public string Grupopadre = "GRUPOPADRE";
        public string Grupoactivo = "GRUPOACTIVO";
        public string Grupocomb = "GRUPOCOMB";
        public string Osicodi = "OSICODI";
        public string Grupocodi2 = "GRUPOCODI2";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Gruposub = "GRUPOSUB";
        public string Barracodi2 = "BARRACODI2";
        public string Barramw1 = "BARRAMW1";
        public string Barramw2 = "BARRAMW2";
        public string Gruponombncp = "GRUPONOMBNCP";
        public string Tipogrupocodi = "TIPOGRUPOCODI";
        public string DesTipoGrupo = "TIPOGRUPONOMB";
        public string EmprNomb = "EMPRNOMB";
        public string TipoGrupoCodi2 = "TIPOGRUPOCODI2";
        public string TipoGenerRER = "TIPOGENERRER";
        public string Fenergnomb = "FENERGNOMB";
        public string GrupoNodoEnergetico = "GRUPONODOENERGETICO";
        public string GrupoReservaFria = "GRUPORESERVAFRIA";

		//- alpha.HDT - 30/10/2016: Cambio para atender el requerimiento. 
        public string Osinergcodi = "OSINERGCODI";
        public string GrupoCodiSDDP = "GRUPOCODISDDP";

        //- alpha.HDT - 19/12/2016: Cambio para atender el requerimiento. 
        public string Fueosinergcodi = "FUEOSINERGCODI";

        public string Fenergpadre = "FENERGPADRE";
        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string GrupoEstado = "GRUPOESTADO";
        public string GrupoTension = "GRUPOTENSION";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string GrupoUsuModificacion = "GRUPOUSUMODIFICACION";
        public string GrupoFecModificacion = "GRUPOFECMODIFICACION";
        public string FlagModoEspecial = "MODO_ESP";
        public string Grupocodimodo = "GRUPOCODIMODO";
        public string Emprabrev = "EMPRABREV";
        public string Emprorden = "EMPRORDEN";
        public string Tgenercodi = "TGENERCODI";
        public string Tgenernomb = "TGENERNOMB";
        public string Idtv = "IDTV";

        #region MigracionSGOCOES-GrupoB
        public string GrupoCentral = "GRUPOCENTRAL";
        public string Catenomb = "CATENOMB";
        public string Areanomb = "AREANOMB";
        public string Areadesc = "AREADESC";
        public string Ptomedicodi = "Ptomedicodi";
        public string Famcodi = "Famcodi";
        public string Equiabrev = "Equiabrev";
        public string Digsilent = "Digsilent";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";
        public string Areacodi = "AREACODI";
        #endregion

        //cambio Compensaciones 201810
        public string HorasOperacion = "HORASOPERACION";
        public string Calificacion = "CALIFICACION";
        public string AccionCalculo = "ACCIONCALCULO";

        #region Titularidad-Instalaciones-Empresas

        public string EmprnombOrigen = "EMPRNOMB_ORIGEN";

        #endregion

        public string Grupotipomodo = "GRUPOTIPOMODO";
        public string Grupousucreacion = "GRUPOUSUCREACION";
        public string Grupofeccreacion = "GRUPOFECCREACION";

        #region Ficha Tecnica 2023
        public string Idelemento = "IDELEMENTO";
        public string Idempresaelemento = "IDEMPRESAELEMENTO";
        public string Nombempresaelemento = "NOMBEMPRESAELEMENTO";
        public string Idempresacopelemento = "IDEMPRESACOPELEMENTO";
        public string Nombempresacopelemento = "NOMBEMPRESACOPELEMENTO";
        public string Nombreelemento = "NOMBREELEMENTO";
        public string Tipoelemento = "TIPOELEMENTO";
        public string Areaelemento = "AREAELEMENTO";
        public string Estadoelemento = "ESTADOELEMENTO";
        #endregion

        #endregion

        public string SqlListarGeneradoresDespachoOsinergmin
        {
            get { return base.GetSqlXml("ListarGeneradoresDespachoOsinergmin"); }
        }

        public string SqlListaModosOperacionActivos
        {
            get { return base.GetSqlXml("ListaModosOperacionActivos"); }
        }

        public string SqlListaModosOperacion
        {
            get { return base.GetSqlXml("ListaModosOperacion"); }
        }

        public string SqlCambiarTipoGrupo
        {
            get { return base.GetSqlXml("CambiarTipoGrupo"); }
        }

        public string SqlModosOperacionCentral1
        {
            get { return base.GetSqlXml("ModosOperacionPorCentral1"); }
        }

        public string SqlModosOperacionCentral2
        {
            get { return base.GetSqlXml("ModosOperacionPorCentral2"); }
        }

        public string SqlObtenerCodigoModoOperacionPadre
        {
            get { return base.GetSqlXml("ObtenerCodigoModoOperacionPadre"); }
        }

        public string SqlModosOperacionxFiltro
        {
            get { return base.GetSqlXml("ModosOperacionxFiltro"); }
        }

        public string SqlCantidadModosOperacionxFiltro
        {
            get { return base.GetSqlXml("CantidadModosOperacionxFiltro"); }
        }

        public string SqlObtenerModoOperacion
        {
            get { return base.GetSqlXml("ObtenerModoOperacion"); }
        }

        public string SqlDatosVigentesPorModoOperacion
        {
            get { return base.GetSqlXml("DatosVigentesPorModoOperacion"); }
        }

        public string SqlListCentrales
        {
            get { return base.GetSqlXml("ListCentrales"); }
        }

        public string sqlParametrosModoOperacionCompensacion
        {
            get { return base.GetSqlXml("ParametrosModoOperacionCompensacion"); }
        }

        public string SqlObtenerTipoCombustiblePorCentral
        {
            get { return base.GetSqlXml("ObtenerTipoCombustiblePorCentral"); }
        }

        public string SqlListadoModosFuncionalesTermicosActivos
        {
            get { return base.GetSqlXml("ListadoModosFuncionalesTermicosActivos"); }
        }

        public string SqlObtenerArbolGrupoDespacho
        {
            get { return base.GetSqlXml("ObtenerArbolGrupoDespacho"); }
        }

        public string SqlObtenerCentralesPorGrupo
        {
            get { return base.GetSqlXml("ObtenerCentralesPorGrupo"); }
        }

        public string SqlListadoModosFuncionalesCostosVariables
        {
            get { return base.GetSqlXml("ListadoModosFuncionalesCostosVariables"); }
        }

        public string SqlModosOperacionNoConfigurados
        {
            get { return base.GetSqlXml("ModosOperacionNoConfigurados"); }
        }

        public string SqlModosOperacionConfigurados
        {
            get { return base.GetSqlXml("ModosOperacionConfigurados"); }
        }

        public string SqlReportePotenciaEfectivaTermicas
        {
            get { return base.GetSqlXml("ReportePotenciaEfectivaTermicas"); }
        }

        public string SqlListarModoOperacionCategoriaTermico
        {
            get { return base.GetSqlXml("ListarModoOperacionCategoriaTermico"); }
        }
		
		public string SqlGruposXCategoria
        {
            get { return base.GetSqlXml("ListaGruposXCategoria"); }
        }

        public string SqlListarUnidadesWithModoOperacionXCentralYEmpresa
        {
            get { return base.GetSqlXml("ListarUnidadesWithModoOperacionXCentralYEmpresa"); }
        }
        
        public string SqlListGrupoDespacho
        {
            get { return base.GetSqlXml("ListGrupoDespacho"); }
        }
        public string SqlListarGrupoPadre
        {
            get { return base.GetSqlXml("ListarGrupoPadre"); }
        }
        public string SqlListGrupoDespachoXFiltro
        {
            get { return base.GetSqlXml("ListGrupoDespachoXFiltro"); }
        }

        //- alpha.HDT - 30/10/2016: Cambio para atender el requerimiento. 
        public string SqlUpdateOsinergmin
        {
            get { return base.GetSqlXml("UpdateOsinergmin"); }
        }

        //- alpha.HDT - 30/10/2016: Cambio para atender el requerimiento. 
        public string GetByIdOsinergmin
        {
            get { return base.GetSqlXml("GetByIdOsinergmin"); }
        }

        public string SqlListarModoOperacionSubModulo
        {
            get { return base.GetSqlXml("ListarModoOperacionSubModulo"); }
        }

        #region "COSTO OPORTUNIDAD"
        public string SqlGetByIdNCP
        {
            get { return base.GetSqlXml("GetByIdNCP"); }
        }
        public string sqlGetByListaModosOperacionNCP
        {
            get { return base.GetSqlXml("GetByListaModosOperacionNCP"); }
        }
        public string sqlGetListaPotenciaEfectiva
        {
            get { return base.GetSqlXml("GetListaPotenciaEfectiva"); }
        }
        public string SqlGetByIdGrupoPadre
        {
            get { return base.GetSqlXml("GetByIdGrupoPadre"); }
        }
        #endregion

        #region PR5

        public string SqlListarAllUnidadTermoelectrica
        {
            get { return base.GetSqlXml("ListarAllUnidadTermoelectrica"); }
        }

        public string SqlListarAllGrupoRER
        {
            get { return base.GetSqlXml("ListarAllGrupoRER"); }
        }

        public string SqlListarAllGrupoCoGeneracion
        {
            get { return base.GetSqlXml("ListarAllGrupoCoGeneracion"); }
        }

        public string SqlListarAllGrupoGeneracion
        {
            get { return base.GetSqlXml("ListarAllGrupoGeneracion"); }
        }

        #endregion

        //-Pruebas aleatorias
        public string SqlListaModoOperacionDeEquipo
        {
            get { return base.GetSqlXml("ListaModoOperacionDeEquipo"); }
        }

        #region INDISPONIBILIDADES

        public string SqlListaPrGrupoCC
        {
            get { return base.GetSqlXml("ListaPrGrupoCC"); }
        }

        public string SqlListaUnidadesXModoOperacionIndisponibilidad
        {
            get { return base.GetSqlXml("ListaUnidadesXModoOperacionIndisponibilidad"); }
        }

        public string SqlListaByGrupopadre
        {
            get { return base.GetSqlXml("ListaByGrupopadre"); }
        }

        #endregion
        
        #region BarrasModeladas
        public string SqlListGrupoPorCategoriaPaginado
        {
            get { return base.GetSqlXml("ListGrupoPorCategoriaPaginado"); }
        }
        #endregion
        public string SqlListaCompensacionGrupo
        {
            get { return base.GetSqlXml("ListaCompensacionGrupo"); }
        }

        #region Curva de consumo de combustibles

        public string SqlObtenerCentralesPorGrupoCurva
        {
            get { return base.GetSqlXml("ObtenerCentralesPorGrupoCurva"); }
        }

        public string SqlObtenerGruposPorCodigo
        {
            get { return base.GetSqlXml("ObtenerGruposPorCodigo"); }
        }

        public string SqlListarDetalleGrupoCurva
        {
            get { return base.GetSqlXml("ListarDetalleGrupoCurva"); }
        }

        public string SqlUpdateNCP
        {
            get { return base.GetSqlXml("UpdateNCP"); }
        }

        public string SqlObtenerNCP
        {
            get { return base.GetSqlXml("ObtenerNCP"); }
        }

        #endregion

        #region Transferencia de Equipos 
        public string SqlListadoPrGrupoByEmpresa
        {
            get { return base.GetSqlXml("ListadoPrGrupoByEmpresa"); }
        }
        #endregion

        #region Curva Consumo

        public string Curvcodi = "CURVCODI";
        public string GRUPOCODINCP = "GRUPOCODINCP";
        public string CURVGRUPOCODIPRINCIPAL = "CURVGRUPOCODIPRINCIPAL";

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

        public string SqlListarModoOperacionXFamiliaAndEmpresa
        {
            get { return base.GetSqlXml("ListarModoOperacionXFamiliaAndEmpresa"); }
        }

        #region MigracionSGOCOES-GrupoB

        public string SqlListaPrGruposPaginado
        {
            get { return base.GetSqlXml("ListaPrGruposPaginado"); }
        }

        public string SqlTotalPrGrupos
        {
            get { return base.GetSqlXml("TotalPrGrupos"); }
        }

        #endregion

        //Cambio COmpensaciones 201810
        public string SqlGetListModosOperacion
        {
            get { return base.GetSqlXml("GetListModosOperacion"); }
        }
        public string SqlGetListModosIds
        {
            get { return base.GetSqlXml("GetListModosIds"); }
        }
        public string Grupointegrante = "Grupointegrante";

        public string SqlListarMOXEnsayo
        {
            get { return base.GetSqlXml("GetMOXensayo"); }
        }

        public string SqlListaModosOperacionActivosXCategoria
        {
            get { return base.GetSqlXml("ListaModosOperacionActivosXCategoria"); }
        }

        #region Titularidad-Instalaciones-Empresas

        public string SqlListarGrupoByMigracodi
        {
            get { return base.GetSqlXml("ListarGrupoByMigracodi"); }
        }

        #endregion

        #region Numerales Datos Base

        public string Comb = "COMB";

        public string SqlDatosBase_5_6_3
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_3"); }
        }
        #endregion

        #region SIOSEIN

        public string SqlListByIds
        {
            get { return base.GetSqlXml("ListByIds"); }
        }

        #endregion

        #region Pronóstico de la demanda

        //ASSETEC 20200106
        public string SqlBarraCategoria
        {
            get { return base.GetSqlXml("ListaBarrasxCategoria"); }
        }

        public string SqlActualizacionesCostosVariables
        {
            get { return base.GetSqlXml("ActualizacionesCostosVariables"); }
        }
        public string SqlCostosVariablesxActualizacion
        {
            get { return base.GetSqlXml("CostosVariablesxActualizacion"); }
        }
        #endregion

        public string SqlUpdateOsinergcodi
        {
            get { return base.GetSqlXml("UpdateOsinergcodi"); }
        }

        #region Mejoras Yupana

        public string SqlListaEquiposXModoOperacion
        {
            get { return base.GetSqlXml("ListaEquiposXModoOperacion"); }
        }
        #endregion

        public string SqlUpdateSddp
        {
            get { return base.GetSqlXml("UpdateSddp"); }
        }

        #region Demanda DPO
        public string SqlListaBarrasByCodigos
        {
            get { return base.GetSqlXml("ListaBarrasByCodigos"); }
        }
        #endregion

        public string SqlGruposModificados
        {
            get { return base.GetSqlXml("GruposModificados"); }
        }

        #region Ficha tecnica 2023
        public string SqlGetDatosGrupo
        {
            get { return base.GetSqlXml("GetDatosGrupo"); }
        }
        public string SqlListarPorEmpresaPropietaria
        {
            get { return base.GetSqlXml("ListarPorEmpresaPropietaria"); }
        }
        #endregion

    }
}
