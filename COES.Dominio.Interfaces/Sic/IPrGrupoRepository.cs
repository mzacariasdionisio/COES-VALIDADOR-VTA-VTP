using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GRUPO
    /// </summary>
    public interface IPrGrupoRepository
    {
        int Save(PrGrupoDTO entity);
        void Update(PrGrupoDTO entity);
        void Delete(int grupocodi);
        void Delete_UpdateAuditoria(int grupocodi, string username);
        PrGrupoDTO GetById(int grupocodi);
        List<PrGrupoDTO> List(string catecodis = "-1");
        List<PrGrupoDTO> ListByEmprCodiAndCatecodi(int emprCodi, int catecodi);
        List<PrGrupoDTO> GetByCriteria(int idTipoGrupo);
        List<GrupoGeneracionDTO> ListarGeneradoresDespachoOsinergmin();
        List<PrGrupoDTO> ListaModosOperacionActivos();
        List<PrGrupoDTO> ListaModosOperacion();
        void CambiarTipoGrupo(int idGrupo, int idTipoGrupo, int idTipoGrupo2, string tipoGenerRer, string userName, DateTime fecha);
        List<ModoOperacionDTO> ModoOperacionCentral1(int iCentral);
        List<ModoOperacionDTO> ModoOperacionCentral2(int iCentral);
        int ObtenerCodigoModoOperacionPadre(int iPrGrupo);
        List<PrGrupoDTO> ModosOperacionxFiltro(int idEmpresa, string sEstado, string sNombreModo, int nroPagina, int nroFilas);
        int CantidadModosOperacionxFiltro(int idEmpresa, string sEstado, string sNombreModo);
        PrGrupoDTO ObtenerModoOperacion(int iGrupoCodi);
        List<PrGrupoConceptoDato> ListarDatosVigentesPorModoOperacion(string sGrupoCodi, DateTime dtFecha, bool bFichaTecnica = false);
        List<PrGrupoDTO> ListCentrales(string tipocombustible, string emprcodi);
        List<ActualizacionCVDTO> ObtenerActualizacionesCostos(DateTime dtFechaInicio, DateTime dtFechaFin);
        List<ModoOperacionParametrosDTO> ListarModosOperacionParametros(int iGrupoCodi);
        List<ModoOperacionCostosDTO> ListarModosOperacionCostos();
        List<PrGrupoDTO> ObtenerTipoCombustiblePorCentral();
        List<PrGrupoDTO> ListadoModosFuncionalesTermicosActivos();
        List<PrGrupoDTO> ObtenerArbolGrupoDespacho(string empresas, string tipoCentral);
        List<PrGrupoDTO> ObtenerCentralesPorTipo(string tipoGrupo, string empresas);
        List<PrGrupoDTO> ListadoModosFuncionalesCostosVariables(DateTime fecha, string flagGrupoActivo);
        List<PrGrupoDTO> ListarModosOperacionNoConfigurados(int idEmpresa);
        List<CostoVariableDTO> ObtenerCostosVariablesPorActualizacion(int iCodActualizacion);
        List<PrGrupoDTO> ListarModosOperacionConfigurados(int idEmpresa);
        List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaTermicas(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin, string tipo);
        List<PrGrupoDTO> ListarModoOperacionCategoriaTermico();
        void UpdateOsinergmin(PrGrupoDTO entity);
        PrGrupoDTO GetByIdOsinergmin(int grupocodi);
        List<PrGrupoDTO> ListarGruposPorCategoria(List<int> lIdCategori);
        List<PrGrupoDTO> ListarUnidadesWithModoOperacionXCentralYEmpresa(int idCentral, string idEmpresa);
        List<PrGrupoDTO> ListaGruposxDespacho(string empresas, int nroPaginas, int nroFilas);

        int GruposxDespachoXFiltro(string empresas );
        List<PrGrupoDTO> ListarGrupoDespacho();

        List<PrGrupoDTO> ListarModoOperacionSubModulo(int subModulo);
        List<PrGrupoDTO> ListaCompensacionGrupo(int pecacodi);
        //-Pruebas aleatorias
        List<PrGrupoDTO> ListarModoOperacionDeEquipo(int equicodi, int catecodi);

        #region "COSTO OPORTUNIDAD"
        PrGrupoDTO GetByIdNCP(int grupocodincp);
        List<PrGrupoDTO> GetByListaModosOperacionNCP();
        List<PrGrupodatDTO> GetListaPotenciaEfectiva(DateTime dfecha, int porcentajerpf, int origlectcodi);
        List<PrGrupoDTO> GetByIdGrupoPadre(int grupourspadre);
        #endregion

        #region INDISPONIBILIDADES
        List<PrGrupoDTO> ListaPrGrupoCC();
        List<PrGrupoDTO> GetListaUnidadesXModoOperacionIndisponibilidad(int IdCentral);
        List<PrGrupoDTO> ListaByGrupopadre(int equipadre);
        #endregion

        #region PR5
        List<PrGrupoDTO> ListarAllUnidadTermoelectrica();
        List<PrGrupoDTO> ListarAllGrupoRER(DateTime fechaPeriodo);
        List<PrGrupoDTO> ListarAllGrupoCoGeneracion(DateTime fechaPeriodo);
        List<PrGrupoDTO> ListarAllGrupoGeneracion(DateTime fechaConsulta, string grupoactivo, string emprestado);
        #endregion

        List<PrGrupoDTO> ListaPrGruposPorCategoriaPaginado(int iCatecodi, string sEstado, int nroPagina, int nroFilas);
        List<PrGrupoDTO> ListarModoOperacionXFamiliaAndEmpresa(string iCodFamilias, string iEmpresas);

        #region Transferencia de Equipos
        List<PrGrupoDTO> ListarPrGruposXEmpresa(int idEmpresa);
        #endregion

        #region CURVA CONSUMO COMBUSTIBLE
        List<PrGrupoDTO> ListarDetalleGrupoCurva(int CurvCodi);
        List<PrGrupoDTO> ObtenerCentralesPorTipoCurva(string tipoGrupo);
        List<PrGrupoDTO> ObtenerGruposPorCodigo(string codigoGrupo);
        void UpdateNCP(int grupoCodiNCP, int grupoCodi);
        int ObtenerNCP(int CurvCodi);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<PrGrupoDTO> ListaPrGruposPaginado(int iEmpresa, string iCatecodi, string nombre, string sEstado, int nroPagina, int nroFilas, DateTime fechaDat, int esReservaFria, int esNodoEnergetico);
        int TotalPrGrupos(int iEmpresa, string iCatecodi, string nombre, string sEstado, DateTime fechaDat, int esReservaFria, int esNodoEnergetico);
        #endregion
        List<PrGrupoDTO> GetListaModosOperacion(int perianiomes, int pecacodi, int pecacodianterior);
        List<PrGrupoDTO> GetListaModosIds(string fechadatos);

        List<PrGrupoDTO> ListaModosOperacionActivosXCategoria(string catecodi);
        List<PrGrupoDTO> ListarMOXensayo(int idEnsayo);

        #region Titularidad-Instalaciones-Empresas

        List<PrGrupoDTO> ListarGrupoByMigracodi(int idMigracion);

        #endregion

        #region Numerales Datos Base
        List<PrGrupoDTO> ListaNumerales_DatosBase_5_6_3();
        List<PrGrupoDTO> ListByIds(string grupocodi);
        #endregion

        #region Pronóstico de la demanda
        //ASSETEC 20200106
        List<PrGrupoDTO> ListaBarraCategoria(int catecodi);
        #endregion

        #region Cálculo de Consumo de Combustible

        void UpdateOsinergcodi(int grupocodi, string osinergcodi);

        #endregion

        #region Mejoras Yupana
        List<PrGrupoDTO> ListaEquiposXModoOperacion(string idsEquipos);
        #endregion Mejoras Yupana

        void UpdateSddp(int grupocodi, int grupocodisddp);

        #region DemandaDPO
        List<PrGrupoDTO> ListaBarrasByCodigos(string codigos);
        #endregion

        List<PrGrupoDTO> ListadoGruposModificados(DateTime dtFechaInicio, DateTime dtFechaFin);

        #region Ficha tecnica 2023
        PrGrupoDTO ObtenerDatosGrupo(int grupocodi);
        List<PrGrupoDTO> ListarPorEmpresaPropietaria(int emprcodi, int catecodi);
        #endregion
    }
}
