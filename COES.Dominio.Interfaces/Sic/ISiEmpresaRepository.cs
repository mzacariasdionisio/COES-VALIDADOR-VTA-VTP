using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_EMPRESA
    /// </summary>
    public interface ISiEmpresaRepository
    {
        int Save(SiEmpresaDTO entity);
        void Update(SiEmpresaDTO entity);
        void Delete(int emprcodi);
        void UpdateRI(SiEmpresaDTO entity);
        SiEmpresaDTO GetById(int emprcodi);
        List<SiEmpresaDTO> List(int tipoemprcodi);
        List<SiEmpresaDTO> ListGeneral();
        List<SiEmpresaDTO> GetByCriteria(string tiposEmpresa);
        List<SiEmpresaDTO> GetEmpresaSistemaPorTipo(string tiposEmpresa);
        List<SiEmpresaDTO> GetByUser(string userlogin);
        List<SiEmpresaDTO> ObtenerEmpresasSEIN();
        List<SiEmpresaDTO> ObtenerEmpresasxCombustible(string tipocombustible);
        List<SiEmpresaDTO> ObtenerEmpresasxCombustiblexUsuario(string tipocombustible, string usuario);
        List<SiEmpresaDTO> ListEmpresasGeneradoras();
        List<SiEmpresaDTO> ListEmpresasClientes();
        List<SiEmpresaDTO> ListEmprResponsables(string criterio);
        List<SiEmpresaDTO> ObtenerEmpresasHidro();
        List<SiEmpresaDTO> ObtenerEmpresasPtoMedicion();
        List<SiEmpresaDTO> ListadoComboEmpresasPorTipo(int tipoemprcodi);
        List<SiEmpresaDTO> ObtenerEmpresaFormato(int idFormato);
        List<SiEmpresaDTO> ObtenerEmpresaFormatoMultiple(string idsFormatos);
        List<SiEmpresaDTO> ObtenerEmpresaFormatoPorEstado(int idFormato, string estado);
        List<SiEmpresaDTO> ObtenerEmpresaActivasFormato(int idFormato);
        List<SiEmpresaDTO> ListarEmpresaXFormato(string idFormatos);
        List<SiEmpresaDTO> ObtenerEmpresaFormatoxUsuario(int idFormato, int IdEmpresa);
        SiEmpresaDTO ListInfoCliente(string nombre);
        SiEmpresaDTO ListResponsable(string empresa);
        int ObtenerNroRegistrosBusqueda(string nombre, string ruc, int idTipoEmpresa, string empresein, string emprestado);
        List<SiEmpresaDTO> BuscarEmpresas(string nombre, string ruc, int idTipoEmpresa, string empresein, string emprestado,
            int nroPagina, int nroFilas);
        void DarBajaEmpresa(int idEmpresa, string usuario);
        bool VerificarExistenciaPorNombre(string nombre);
        bool VerificarExistenciaPorRuc(string ruc);
        SiEmpresaDTO ObtenerEmpresaPorRuc(string ruc);
        void ActualizarEstado(SiEmpresaDTO entity);
        List<SiEmpresaDTO> ExportarEmpresas(string nombre, string nroRuc, int idTipoEmpresa, string empresaSein,
            string estado);
        List<SiEmpresaDTO> ListaEmpresasPorTipoCumplimiento(int tipoemprcodi, int formato);
        List<SiEmpresaDTO> ListaEmailUsuariosEmpresas(string periodo, int formato, int modulo);
        List<SiEmpresaDTO> List(int minRowToFetch, int maxRowToFetch, SiEmpresaDTO siEmpresaDto);
        SiEmpresaDTO GetById(SiEmpresaDTO siEmpresaDto);
        int GetTotalRecords(SiEmpresaDTO siEmpresaDto);
        List<SiEmpresaDTO> ListarEmpresasxTipoEquipos(string tipoEquipos, string tiposEmpresa, string estadoEquipo = "-1");
        void GetAll(out List<string[]> registros, out Dictionary<int, MetadataDTO> metaDatosDictionary);

        void ActualizarRucFicticio(int id, string ruc);

        //- alpha.JDEL - Inicio 19/10/2016: Cambio para atender el requerimiento.
        SiEmpresaDTO GetByCodOsinergmin(string codigo);
        //- JDEL Fin
        //- pr16.JDEL - Inicio 20/10/2016: Cambio para atender el requerimiento.
        List<SiEmpresaDTO> ListaEmpresasSuministrador();
        List<SiEmpresaDTO> ObtenerEmpresasPuntoMedicion();
        //- JDEL Fin
        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la empresa que incluye el código Osinergmin.
        /// <remarks>Este método es usado para la sincronización de maestros.</remarks>
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        SiEmpresaDTO GetByIdOsinergmin(int emprcodi);

        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de empresas de acuerdo con la lista de tipos de empresas
        /// proporcionada.
        /// <remarks>Este método es usado para la sincronización de maestros.</remarks>
        /// </summary>
        /// <param name="listaIdTipoEmpresa"></param>
        /// <returns></returns>
        List<SiEmpresaDTO> ListarEmpresasPorTipo(List<int> listaIdTipoEmpresa);

        //- alpha.HDT - 31/10/2016: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite actualizar la empresa incluyendo el campo Osinergmin.
        /// <remarks>este método es usado para la sincronización de maestros.</remarks>
        /// </summary>
        /// <param name="empresaDTO"></param>
        void UpdateOsinergmin(SiEmpresaDTO empresaDTO);

        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.
        SiEmpresaDTO GetByCodigoOsinergmin(string codOsinergmin);
        //- JDEL Fin
        List<SiEmpresaDTO> ObtenerEmpresasGeneracion();
        // Inicio de Agregado - Sistema de Compensaciones
        List<SiEmpresaDTO> ListaEmpresasCompensacion();
        // Fin de Agregado - Sistema de Compensaciones
        //Agregado por STS
        List<SiEmpresaDTO> ListaEmpresasSeleccionadas(SgdEstadisticasDTO filterDto);
        //- alpha.HDT - 14/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener el objeto Empresa de acuerdo con el RUC.
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        SiEmpresaDTO GetUsuarioLibreByRuc(string ruc);
        List<SiEmpresaDTO> ObtenerEmpresasPorTipoSEIN(string tipoEmpresa);
        List<SiEmpresaDTO> ObtenerEmpresasPorTipo(string tipoEmpresa);
        int ObtenerEmpresaMigra(int empr, string fecha);

        #region PR5
        List<SiEmpresaDTO> ListarEmpresasXID(string idsEmpresas);
        #endregion
        #region Indisponibilidades
        List<SiEmpresaDTO> ListarEmpresasConCentralTermoxUsuario(string userlogin);
        List<SiEmpresaDTO> ListarEmpresasConCentralTermo();
        #endregion

        #region Sistema Rechazo Carga
        List<SiEmpresaDTO> ListaEmpresasRechazoCarga(string empresa, int tipoEmpresa, string estadoRegistro);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<SiEmpresaDTO> ListarEmpresasxCategoria(string catecodi);
        #endregion

        #region Transferencia Equipos 
        List<SiEmpresaDTO> ObtenerEmpresasActivasBajas();
        List<SiEmpresaDTO> ObtenerIdNameEmpresasActivasBajas();
        List<SiEmpresaDTO> ListarEmpresaEstadoActual(DateTime fechaConsulta);
        #endregion

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // ASSETEC.SGH - 30/07/2018: FUNCIONES PARA FILTROS DE EQUIPAMIENTO
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Intervenciones
        List<SiEmpresaDTO> ListarComboEmpresas();
        void ActualizarAbreviatura(int emprcodi, string emprabrev);
        #endregion

        #region MonitoreoMME
        List<SiEmpresaDTO> ListarEmpresaIntegranteMonitoreoMME(DateTime fechaIniPeriodo, DateTime fechaFinPeriodo);
        #endregion

        #region FIT - REGISTRO DE SEÑALES NO DISPONIBLES - ASOCIACION DE EMPRESA
        void UpdateAsociacionEmpresa(int emprcodi, int emprcodisp7);
        void EliminarAsociacionEmpresa(int emprcodi);
        #endregion

        #region Titularidad-Instalaciones-Empresas

        List<SiEmpresaDTO> ListarEmpresaVigenteByRango(DateTime fechaIniPeriodo, DateTime fechaFinPeriodo);

        #endregion

        #region Mejoras IEOD
        List<SiEmpresaDTO> ListarEmpresaPorOrigenPtoMedicion(int origlectcodi);
        List<SiEmpresaDTO> ListarEmpresaScada();
        #endregion

        #region FIT - VALORIZACION DIARIA
        List<SiEmpresaDTO> ListarEmpresasPorIds(string ids);
        #endregion

        #region DevolucionAportes
        List<SiEmpresaDTO> ListarEmpresaDevolucion();
        #endregion

        List<SiEmpresaDTO> ObtenerEmpresasProveedores(int tipoEmpresa);
        List<SiEmpresaDTO> ObtenerEmpresaPortalTramite(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial, int nroPagina, int pageSize);
        int ObtenerNroRegistrosBusquedaTramite(string tipoAgente, int tipoEmpresa, string indicador, string ruc, string razonSocial);
        void ActualizarDatosUsuarioTramite(int emprcodi, string indicador, DateTime? fecha);

        #region Aplicativo Extranet CTAF

        List<SiEmpresaDTO> ListarEmpresasEventoCTAF();

        #endregion

        #region FICHA TÉCNICA

        List<SiEmpresaDTO> ListarEmpresasFT();

        #endregion
        List<SiEmpresaDTO> ListadoEmpresasCentralesActivas();

        #region EMPRESA MME

        int SaveMME(SiEmpresaMMEDTO entity);
        void UpdateMME(SiEmpresaMMEDTO entity);
        SiEmpresaMMEDTO GetByIdMME(int emprmmecodi);
        List<SiEmpresaMMEDTO> BuscarEmpresasMME(int idTipoEmpresa);
        List<SiEmpresaMMEDTO> BuscarHistorialEmpresasMME(int emprcodi);
        List<SiEmpresaDTO> BuscarEmpresasporParticipacion(int tipo);

        #endregion

        #region Ficha Tecnica 2023
        List<SiEmpresaDTO> ListarPorTipo(int tipoemprcodi);
        List<SiEmpresaDTO> ListarEmpresaExtranetFT();
        #endregion

        #region Informes SGI

        List<SiEmpresaDTO> ObtenerEmpresasPorFormato(string formatos);

        List<SiEmpresaDTO> ObtenerEmpresasSEINPorFormato(string formatos);

        #endregion

        #region Demanda DPO - iteracion 2
        List<SiEmpresaDTO> ListaEmpresasByTipo(string tipo);
        #endregion

        #region CPPA.ASSETEC.2024 - iteracion 1 - 2
        List<SiEmpresaDTO> ListaEmpresasCPPA();
        List<SiEmpresaDTO> ListaEmpresasDescargaIntegrantes();
        #endregion
        #region Proteccion Equipos

        List<SiEmpresaDTO> ListarMaestroEmpresasProteccion();

        #endregion

        #region GESPROTEC-20241031
        List<SiEmpresaDTO> ListObtenerEmpresaSEINProtecciones();
        #endregion
    }
}
