using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_EQUIPO
    /// </summary>
    public interface IEqEquipoRepository
    {
        int Save(EqEquipoDTO entity);
        void Update(EqEquipoDTO entity);
        void Delete(int equicodi);
        void Delete_UpdateAuditoria(int equicodi, string user);
        EqEquipoDTO GetById(int equicodi);
        List<EqEquipoDTO> List();
        List<EqEquipoDTO> GetByCriteria();
        List<EqEquipoDTO> BasicList();
        List<EqEquipoDTO> ListadoCentralesOsinergmin();
        List<EqEquipoDTO> ListarEquiposxFiltro(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa,
            string sNombreEquipo, int idPadre);
        List<EqEquipoDTO> BuscarEquipoxNombre(string coincidencia, int nroPagina, int nroFilas);
        List<EqEquipoDTO> ListarEquipoxFamilias(params int[] iCodFamilias);
        List<EqEquipoDTO> ListarEquipoxFamiliasActivosyProyectos(params int[] iCodFamilias);
        List<EqEquipoDTO> ListarEquipoxFamiliasEmpresas(int[] iCodFamilias, int[] iEmpresas);
        List<EqEquipoDTO> ListarEquiposxFiltroPaginado(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa,
            string sNombreEquipo, int idPadre, int nroPagina, int nroFilas, ref int totalPaginas, ref int totalRegistros);
        EqEquipoDTO ObtenerDetalleEquipo(int idEquipo);
        List<EqEquipoDTO> ObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea);
        List<EqEquipoDTO> ListarCentralesXCombustible(int emprcodi, int tipocombustible);
        List<EqEquipoDTO> ListarCentralesXEmpresaGEN(string emprcodi);
        List<EqEquipoDTO> ListarCentralesXEmpresaGEN2(string emprcodi);
        List<EqEquipoDTO> ListarEquiposEnsayo(string equicodi);
        List<EqEquipoDTO> ListaEquipamientoPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre, int nroPagina, int nroFilas);
        int TotalEquipamiento(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre);
        List<EqEquipoDTO> ObtenerEquiposProcManiobras(int famCodi, int propcodi);
        List<EqEquipoDTO> GetByEmprFam(int emprcodi, int famcodi);
        List<EqEquipoDTO> ListaRecursosxCuenca(string empresas, string cuencas, string recursos);
        List<EqEquipoDTO> ListarGeneradoresTermicosPorModoOperacion(int grupocodi);
        List<EqEquipoDTO> ObtenerEquipoPorFamilia(int emprcodi, int famcodi);
        List<EqEquipoDTO> List(int minRowToFetch, int maxRowToFetch, EqFamiliaDTO eqFamiliaDto, EqEquipoDTO eqEquipoDto);
        int GetTotalRecords(EqFamiliaDTO eqFamiliaDto, EqEquipoDTO eqEquipoDto);
        void GetAll(EqFamiliaDTO eqFamiliaDto, out List<string[]> registros, out Dictionary<int, MetadataDTO> metaDatosDictionary);
        List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaHidraulicas(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin);
        List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaSolares(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin);
        List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaEolicas(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin);
        List<EqEquipoDTO> ObtenerPorPadre(int? idEquipo);
        List<EqEquipoDTO> ListarEquiposPorFamilia(string idFamilia);
        void UpdateOsinergmin(EqEquipoDTO equipoCOES);
        List<EqEquipoDTO> UpdateOsinergminCodigo(int? equicodi, string osinergmincodi);
        EqEquipoDTO GetByIdOsinergmin(int equicodi);
        EqEquipoDTO GetByCodOsinergmin(string codOsinergmin);
        List<EqEquipoDTO> ListarEquiposPropiedadesAGC(string fecha);
        List<EqEquipoDTO> ListarEquiposAGC();
        List<EqEquipoDTO> ListadoEquipoNombre(string famcodis);
        List<EqEquipoDTO> ListarEquiposPropiedades(int propcodi, DateTime fecha, int emprCodi, int areacodi, int famcodi, int nroPage, int pageSize);
        int TotalEquiposPropiedades(int emprCodi, int areacodi, int famCodi);


        List<EqEquipoDTO> ListarEquiposPrGrupo(string grupoCodi);
        List<EqEquipoDTO> CentralesXEmpresaHorasOperacion(int emprCodi);
        List<EqEquipoDTO> CentralesXEmpresaHorasOperacion(int emprCodi, string fecha);
        List<EqEquipoDTO> ListarCentralesXEmpresaXFamiliaGEN(string emprCodi, string sFamCodi);
        List<EqEquipoDTO> ListadoXEmpresa(int emprCodi);
        List<EqEquipoDTO> GetByEmprFam2(int grupoCodi, int famcodi);
        List<EqEquipoDTO> ListarCentralesXEmpresaXFamiliaGEN2(string sEmprCodi, string sFamCodi, string equiestado);
        List<EqEquipoDTO> ListarTopologiaEquipoPadres(string empresa, string familias, int tiporelcodi);
        List<EqEquipoDTO> ObtenerEquipoPadresHidrologicosCuenca();
        List<EqEquipoDTO> ObtenerEquipoPadresHidrologicosCuencaTodos();
        List<EqEquipoDTO> ObtenerEquipoPorAreaEmpresaTodos(int idEmpresa, int idArea);
        List<EqEquipoDTO> ListarUnidadesxEnsayo(int idEnsayo);
        //- alpha.HDT - 16/08/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener un equipo activo que coincidda con el código Osinergmin y el código de familia.
        /// Si se encuentra el equipo entonces no se crea un nuevo equipo.
        /// </summary>
        /// <param name="codOsinergmin"></param>
        /// <param name="codigoFamilia"></param>
        /// <returns></returns>
        EqEquipoDTO GetEqEquipo(string codOsinergmin, int codigoFamilia);
        EqEquipoDTO GetByIdEquipo(int idEquipo);
        List<EqEquipoDTO> ListByIdEquipo(string idEquipo);

        #region PR5
        List<EqEquipoDTO> ListarEquipoXAreasXTipoEquipos(string sTipoEquipos, string idArea);
        List<EqEquipoDTO> ListarEquipoXAreasXEmpresa(string idEmpresa, string idArea);
        List<EqEquipoDTO> ListarIngresoSalidaOperacionComercialSEIN(DateTime fechaini, DateTime fechafin, string famcodi);
        List<EqEquipoDTO> ListarIngresoOperacionComercialSEIN(DateTime fechaini, DateTime fechafin);
        List<EqEquipoDTO> ObtenerEquiposPorFamiliaOriglectcodi(int emprcodi, int famcodi, int origlectcodi);
        #endregion

        List<EqEquipoDTO> ListarEquiposTTIE(string famcodi);

        #region Rechazo Carga
        List<EqEquipoDTO> ObtenerEquipoPorFamiliaRechazoCarga(int emprcodi, int famcodi);
        #endregion
        #region Horas Operacion EMS
        List<EqEquipoDTO> GetByEmprFamCentral(int emprcodi, int famcodi, int idCentral);
        #endregion
        #region BarrasModeladas
        /// <summary>
        /// Lisrado de Equipos filtrados por grupocodi y Famcodi
        /// </summary>
        /// <param name="grupocodi">código grupocodi o -2 para evitar el filtro</param>
        /// <param name="famcodi">Código de familia o -2 para evitar filtro</param>
        /// <returns></returns>
        List<EqEquipoDTO> ListadoEquiposPorGrupoCodiFamilia(int grupocodi, int famcodi);
        /// <summary>
        /// Método que actualiza el código de PRGRUPO en uno o varios equipos
        /// </summary>
        /// <param name="sCodigosEquipo">Codigos de equipos separados por coma</param>
        /// <param name="iGrupoCodi">Código de PR_GRUPO</param>
        /// <param name="iFamcodi">Código de familia</param>
        /// <param name="sUsuario">Usuario que actualiza</param>
        void ActualizarGrupoCodiPorEquipoFamilia(string sCodigosEquipo, int? iGrupoCodi, int iFamcodi, string sUsuario);

        #endregion
        #region NotificacionesCambiosEquipamiento
        /// <summary>
        /// Método que retorna el listado de Equipos que fueron creados o modificados durante el periodo consultado
        /// </summary>
        /// <param name="dtFechaInicio">Fecha de Inicio</param>
        /// <param name="dtFechaFin">Fecha Fin</param>
        /// <returns>Listado Equipos</returns>
        List<EqEquipoDTO> ListadoEquiposModificados(DateTime dtFechaInicio, DateTime dtFechaFin);
        #endregion


        #region Transferencia de Equipos
        List<EqEquipoDTO> ListarEquipoXEmpresa(string idEmpresa);
        List<EqEquipoDTO> ListarEqEquipoXEmpresaOrigenMigracion(int idEmpresa);
        List<EqEquipoDTO> ListaEquiposSiEquipoMigrarByMigracodi(int idMigracion);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<EqEquipoDTO> ListaLineasDigsilent(string propcodiDigsilente, string famcodi);
        List<EqEquipoDTO> ListaPruebasAleatorias(DateTime fecIni);
        List<EqEquipoDTO> GetListaPotencias(DateTime f_);
        #endregion

        #region INTERVENCIONES
        List<EqEquipoDTO> ListarComboEquiposXUbicaciones(string IdArea);
        void ObtenerDatosEquipamiento(int IdEquipo, ref int IdUbicacion, ref int IdEmpresa, out int equicodi);

        // Metodos agregados para Procedimiento Maniobra
        List<EqEquipoDTO> ListarEquiposXIds(string idsEquipos);
        List<EqEquipoDTO> ObtenerEnlaces(string idsEquipos);

        List<EqEquipoDTO> ListarComboEquiposXUbicacionesXFamilia(string idArea, string idFamilia);
        List<EqEquipoDTO> ListarEquiposXTipoprograma(int evenclasecodi);
        List<EqEquipoDTO> ListarLineasValidas();
        List<EqEquipoDTO> ListarCeldasValidas();
        #endregion

        #region Numerales Datos Base
        List<EqEquipoDTO> ListaNumerales_DatosBase_5_6_6();
        #endregion

        #region Medidores de Generación PR15
        /// <summary>
        /// Retorna los equipos asociados al punto de medición
        /// </summary>
        /// <param name="ptomedicodi">Código de Punto de medición</param>
        /// <returns>Equipos asociado al punto de medición</returns>
        List<EqEquipoDTO> GetEquipoByPuntoMedicion(int ptomedicodi);
        #endregion

        #region Equipos sin datos de ficha técnica
        List<EqEquipoDTO> ListaEqEmpresaFamilia(int emprcodi, int famcodi);
        #endregion

        #region Mejoras Yupana
        List<EqEquipoDTO> ListarUnidadesxPlanta2(string pCodigos, int pFamilia);
        #endregion

        #region FICHA TÉCNICA
        int Save(EqEquipoDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<EqEquipoDTO> ListarSubestacionFT();

        #endregion

        #region Ficha tecnica 2023
        List<EqEquipoDTO> ListarPorEmpresaPropietaria(int emprcodi, int famcodi);
        List<EqEquipoDTO> ListarPorEmpresaCoPropietaria(int emprcodi, int famcodi);
        List<EqEquipoDTO> GetByEmprFamCentral2(int emprcodi, int famcodi, int idCentral);
        #endregion

        #region Demanda DPO - Iteracion 2
        List<EqEquipoDTO> ListaEquipoByEmpresa(int idEmpresa);
        #endregion

        #region CPPA.ASSETEC.2024
        List<EqEquipoDTO> ListaCentralesGeneracion();
        #endregion

        #region GESPROTEC-20241031
        List<EqEquipoDTO> ListaEquipoCOES(String idUbicacion, String idTipoEquipo, String nombreEquipo, string sProgramaExistente);

        List<EqEquipoDTO> ListaExportarEquipoCOES(string idUbicacion, string idTipoEquipo, string nombreEquipo);

        List<EqEquipoDTO> ListarMaestroCeldaProteccion();

        List<EqEquipoDTO> ListarMaestroReleProteccion(int tipoUso);

        List<EqEquipoDTO> ListarMaestroInterruptorProteccion();

        List<EqEquipoDTO> ListLineaEvaluacion(string equiCodi, string codigo, string ubicacion, string emprCodigo, string equiEstado);
        #endregion

        #region GESPROTECT - 20250206

        
        List<EqEquipoDTO> ListarMaestroEquiposLinea();

        List<EqEquipoDTO> ListarMaestroEquiposArea();

        List<EqEquipoDTO> ListarMaestroEquiposCondensador();

        List<EqEquipoDTO> ListarMaestroEquiposReactor();

        List<EqEquipoDTO> ListarMaestroEquiposCeldasAcoplamiento();

        List<EqEquipoDTO> ListarMaestroEquiposTransformador();
        #endregion


    }
}
