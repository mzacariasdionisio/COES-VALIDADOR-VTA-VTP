using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.RechazoCarga
{
    /// <summary>
    /// Clases con métodos del módulo RechazoCarga
    /// </summary>
    public class RechazoCargaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RechazoCargaAppServicio));

        #region Métodos Tabla RCA_CARGA_ESENCIAL

        /// <summary>
        /// Inserta un registro de la tabla RCA_CARGA_ESENCIAL
        /// </summary>
        public void SaveRcaCargaEsencial(RcaCargaEsencialDTO entity)
        {
            try
            {
                FactorySic.GetRcaCargaEsencialRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CARGA_ESENCIAL
        /// </summary>
        public void UpdateRcaCargaEsencial(RcaCargaEsencialDTO entity)
        {
            try
            {
                FactorySic.GetRcaCargaEsencialRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_CARGA_ESENCIAL
        /// </summary>
        public void DeleteRcaCargaEsencial(int rccarecodi)
        {
            try
            {
                FactorySic.GetRcaCargaEsencialRepository().Delete(rccarecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_CARGA_ESENCIAL
        /// </summary>
        public RcaCargaEsencialDTO GetByIdRcaCargaEsencial(int rccarecodi)
        {
            return FactorySic.GetRcaCargaEsencialRepository().GetById(rccarecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_CARGA_ESENCIAL
        /// </summary>
        public List<RcaCargaEsencialDTO> ListRcaCargaEsencials()
        {
            return FactorySic.GetRcaCargaEsencialRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaCargaEsencial
        /// </summary>
        public List<RcaCargaEsencialDTO> GetByCriteriaRcaCargaEsencials()
        {
            return FactorySic.GetRcaCargaEsencialRepository().GetByCriteria();
        }
        /// <summary>
        /// Permite listar carga esencial basado en criterios de busqueda
        /// </summary>
        /// <param name="vigente"></param>
        /// <param name="empresa"></param>
        /// <param name="documento"></param>
        /// <param name="cargaIni"></param>
        /// <param name="cargaFin"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="estadoRegistro"></param>
        /// <param name="origen"></param>
        /// <param name="regIni"></param>
        /// <param name="regFin"></param>
        /// <returns></returns>
        public List<RcaCargaEsencialDTO> ListarCargaEsencialFiltro(string vigente, string empresa, string documento, 
            string cargaIni, string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen, int regIni, int regFin)
        {
            return FactorySic.GetRcaCargaEsencialRepository().ListarCargaEsencialFiltro(vigente, empresa, documento, 
                cargaIni, cargaFin, fecIni, fecFin, estadoRegistro, origen, regIni, regFin);
        }

        public List<RcaCargaEsencialDTO> ListarCargaEsencialPorPuntoMedicion(string puntoMedicion, string empresa)
        {
            return FactorySic.GetRcaCargaEsencialRepository().ListarCargaEsencialPorPuntoMedicion(puntoMedicion, empresa);
        }

        /// <summary>
        /// Permite listar carga esencial historica
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns>
        public List<RcaCargaEsencialDTO> ListarCargaEsencialHistorial(int emprcodi, int equicodi, string estadoRegistro)
        {
            return FactorySic.GetRcaCargaEsencialRepository().ListarCargaEsencialHistorial(emprcodi, equicodi, estadoRegistro);
        }

        /// <summary>
        /// Permite obtener carga esencial por codigo
        /// </summary>
        /// <param name="rccarecodi"></param>
        /// <returns></returns>
        public RcaCargaEsencialDTO ObtenerCargaEsencialPorCodigo(int rccarecodi)
        {
            return FactorySic.GetRcaCargaEsencialRepository().ObtenerPorCodigo(rccarecodi);
        }

        public List<RcaCargaEsencialDTO> ListarCargaEsencialExcel(string vigente, string empresa, string documento,
            string cargaIni, string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen)
        {
            return FactorySic.GetRcaCargaEsencialRepository().ListarCargaEsencialExcel(vigente, empresa, documento,
                cargaIni, cargaFin, fecIni, fecFin, estadoRegistro, origen);
        }

        public int ListarCargaEsencialCount(string vigente, string empresa, string documento,
            string cargaIni, string cargaFin, string fecIni, string fecFin, string estadoRegistro, int origen)
        {
            return FactorySic.GetRcaCargaEsencialRepository().ListarCargaEsencialCount(vigente, empresa, documento,
                cargaIni, cargaFin, fecIni, fecFin, estadoRegistro, origen);
        }

        #endregion

        #region Métodos Tabla RCA_CUADRO_EJEC_USUARIO

        /// <summary>
        /// Inserta un registro de la tabla RCA_CUADRO_EJEC_USUARIO
        /// </summary>
        public void SaveRcaCuadroEjecUsuario(RcaCuadroEjecUsuarioDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroEjecUsuarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_EJEC_USUARIO
        /// </summary>
        public void UpdateRcaCuadroEjecUsuario(RcaCuadroEjecUsuarioDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroEjecUsuarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_CUADRO_EJEC_USUARIO
        /// </summary>
        public void DeleteRcaCuadroEjecUsuario(int rcejeucodi)
        {
            try
            {
                FactorySic.GetRcaCuadroEjecUsuarioRepository().Delete(rcejeucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_CUADRO_EJEC_USUARIO
        /// </summary>
        public RcaCuadroEjecUsuarioDTO GetByIdRcaCuadroEjecUsuario(int rcejeucodi)
        {
            return FactorySic.GetRcaCuadroEjecUsuarioRepository().GetById(rcejeucodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_CUADRO_EJEC_USUARIO
        /// </summary>
        public List<RcaCuadroEjecUsuarioDTO> ListRcaCuadroEjecUsuarios()
        {
            return FactorySic.GetRcaCuadroEjecUsuarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaCuadroEjecUsuario
        /// </summary>
        public List<RcaCuadroEjecUsuarioDTO> GetByCriteriaRcaCuadroEjecUsuarios(int rcproucodi)
        {
            return FactorySic.GetRcaCuadroEjecUsuarioRepository().GetByCriteria(rcproucodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programa"></param>
        /// <param name="cuadroPrograma"></param>
        /// <param name="codigoSuministrador"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public List<RcaCuadroEjecUsuarioDTO> ListFiltro(string programa, string cuadroPrograma, string codigoSuministrador, string tipoReporte)
        {
            return FactorySic.GetRcaCuadroEjecUsuarioRepository().ListFiltro(programa, cuadroPrograma, codigoSuministrador, tipoReporte);
        }

        #endregion

        #region Métodos Tabla RCA_CUADRO_PROG

        /// <summary>
        /// Inserta un registro de la tabla RCA_CUADRO_PROG
        /// </summary>
        public int SaveRcaCuadroProg(RcaCuadroProgDTO entity)
        {
            var res = 0;
            try
            {
                res = FactorySic.GetRcaCuadroProgRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return res;
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_PROG
        /// </summary>
        public void UpdateRcaCuadroProg(RcaCuadroProgDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_PROG
        /// </summary>
        public void UpdateRcaCuadroProgEstado(RcaCuadroProgDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgRepository().UpdateCuadroEstado(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Elimina un registro de la tabla RCA_CUADRO_PROG
        /// </summary>
        public void DeleteRcaCuadroProg(int rccuadcodi)
        {
            try
            {
                FactorySic.GetRcaCuadroProgRepository().Delete(rccuadcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRcaCuadroProgramaEjecucion(RcaCuadroProgDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgRepository().UpdateCuadroProgramaEjecucion(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_CUADRO_PROG
        /// </summary>
        public RcaCuadroProgDTO GetByIdRcaCuadroProg(int rccuadcodi)
        {
            return FactorySic.GetRcaCuadroProgRepository().GetById(rccuadcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_CUADRO_PROG
        /// </summary>
        public List<RcaCuadroProgDTO> ListRcaCuadroProgs()
        {
            return FactorySic.GetRcaCuadroProgRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaCuadroProg
        /// </summary>
        public List<RcaCuadroProgDTO> GetByCriteriaRcaCuadroProgs(string programa, string estadoCuadro)
        {
            return FactorySic.GetRcaCuadroProgRepository().GetByCriteria(programa, estadoCuadro);
        }
        public List<RcaCuadroProgDTO> ListCuadroEnvioArchivoPorPrograma(int rcprogcodi)
        {
            return FactorySic.GetRcaCuadroProgRepository().ListCuadroEnvioArchivoPorPrograma(rcprogcodi);
        }
        /// <summary>
        /// Permite listar cuadro programacion basado en criterios de busqueda
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="configuracion"></param>
        /// <param name="estado"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="energiaRechazadaInicio"></param>
        /// <param name="energiaRechazadaFin"></param>
        ///  /// <param name="sinPrograma"></param>
        /// <returns></returns>
        public List<RcaCuadroProgDTO> ListRcaCuadroProgFiltro(string horizonte, string configuracion, string estado, string fecIni, string fecFin, 
            string energiaRechazadaInicio, string energiaRechazadaFin, int sinPrograma)
        {
            return FactorySic.GetRcaCuadroProgRepository().ListRcaCuadroProgFiltro(horizonte, configuracion, estado, fecIni, fecFin, energiaRechazadaInicio, energiaRechazadaFin, sinPrograma);
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_PROG con el codigo de evento
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateRcaCuadroProgEvento(RcaCuadroProgDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgRepository().UpdateCuadroEvento(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla RCA_CUADRO_PROG_USUARIO

        /// <summary>
        /// Inserta un registro de la tabla RCA_CUADRO_PROG_USUARIO
        /// </summary>
        public void SaveRcaCuadroProgUsuario(RcaCuadroProgUsuarioDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_PROG_USUARIO
        /// </summary>
        public void UpdateRcaCuadroProgUsuario(RcaCuadroProgUsuarioDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_CUADRO_PROG_USUARIO
        /// </summary>
        public void DeleteRcaCuadroProgUsuario(int rcproucodi)
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().Delete(rcproucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_CUADRO_PROG_USUARIO
        /// </summary>
        public RcaCuadroProgUsuarioDTO GetByIdRcaCuadroProgUsuario(int rcproucodi)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().GetById(rcproucodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_CUADRO_PROG_USUARIO
        /// </summary>
        public List<RcaCuadroProgUsuarioDTO> ListRcaCuadroProgUsuarios()
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaCuadroProgUsuario
        /// </summary>
        public List<RcaCuadroProgUsuarioDTO> GetByCriteriaRcaCuadroProgUsuarios()
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().GetByCriteria();
        }

        public List<RcaCuadroProgUsuarioDTO> ListProgramaRechazoCarga(string empresasId, string codigoCuadroPrograma)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListProgramaRechazoCarga(empresasId, codigoCuadroPrograma);
        }

        public List<RcaCuadroProgUsuarioDTO> ListEmpresasProgramaRechazoCarga(string bloqueHorario, string zona, string estacion, decimal medicion, string nombreEmpresa, string empresasId, string equiposId)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListEmpresasProgramaRechazoCarga(bloqueHorario, zona, estacion, medicion, nombreEmpresa, empresasId, equiposId);
        }

        public List<AreaDTO> ListSubEstacion(int codigoZona) 
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListSubEstacion(codigoZona);
        }
        public List<RcaCuadroProgUsuarioDTO> ListEnvioArchivoMagnitud(int programa, int cuadro, int suministrador, string cumplio)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListEnvioArchivoMagnitud(programa, cuadro, suministrador, cumplio);
        }
        public List<SiEmpresaDTO> ListSuministrador(int rccuadcodi)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListSuministrador(rccuadcodi);
        }

        public void UpdateRcaCuadroProgUsuarioEstado(int codigoCuadroPrograma, string estado)
        {
            FactorySic.GetRcaCuadroProgUsuarioRepository().UpdateEstado(codigoCuadroPrograma, estado);
        }

        public List<AreaDTO> ListZonas(int codigoNivel)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListZonas(codigoNivel);
        }

        public int ListAntiguedadDatosDemandaUsuario()
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListAntiguedadDatos();
        }

        public string ListUltimoPeriodo()
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ListUltimoPeriodo();
        }
        public void EliminarDemandaUsuarioLibre()
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().EliminarDemandaUsuarioLibre();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public void CargarDemandaUsuarioLibreSicli(int indiceHP, int indiceHFP, string periodo, string fechaInicio, int idLectura, int tipoInfoCodi)
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().CargarDemandaUsuarioLibreSicli(indiceHP, indiceHFP, periodo, fechaInicio, idLectura, tipoInfoCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void CargarDemandaUsuarioLibre()
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().CargarDemandaUsuarioLibre();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void ActualizarDemandaUsuarioLibre(string fecha)
        {
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().ActualizarDemandaUsuarioLibre(fecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Metodos Tabla RCA_CUADRO_PROG_DISTRIB
        /// <summary>
        /// Inserta un registro de la tabla RCA_CUADRO_PROG_DISTRIB
        /// </summary>
        public void SaveRcaCuadroProgDistrib(RcaCuadroProgDistribDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgDistribRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_PROG_DISTRIB
        /// </summary>
        public void UpdateRcaCuadroProgDistrib(RcaCuadroProgDistribDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroProgDistribRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RCA_CUADRO_PROG_DISTRIB
        /// </summary>
       
        public List<RcaCuadroProgDistribDTO> ListCuadroProgDistrib(int codigoCuadroPrograma)
        {
            return FactorySic.GetRcaCuadroProgDistribRepository().ListCuadroProgDistrib(codigoCuadroPrograma);
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_CUADRO_PROG_DISTRIB
        /// </summary>
        public void DeleteRcaCuadroProgDistrib(int rcprodcodi)
        {
            try
            {
                FactorySic.GetRcaCuadroProgDistribRepository().Delete(rcprodcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla RCA_CUADRO_EJEC_USU_DET

        /// <summary>
        /// Inserta un registro de la tabla RCA_CUADRO_EJEC_USU_DET
        /// </summary>
        public void SaveRcaCuadroEjecDetUsuario(RcaCuadroEjecUsuarioDetDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroEjecUsuarioDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_CUADRO_EJEC_USU_DET
        /// </summary>
        public void UpdateRcaCuadroEjecDetUsuario(RcaCuadroEjecUsuarioDetDTO entity)
        {
            try
            {
                FactorySic.GetRcaCuadroEjecUsuarioDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoCuadroEjecucion"></param>
        /// <returns></returns>
        public List<RcaCuadroEjecUsuarioDetDTO> ListFiltro(int codigoCuadroEjecucion)
        {
            return FactorySic.GetRcaCuadroEjecUsuarioDetRepository().ListFiltro(codigoCuadroEjecucion);
        }

        /// <summary>
        /// Eliminar registros detalle por cliente de tabla RCA_CUADRO_EJEC_USU
        /// </summary>
        /// <param name="rcejeucodi"></param>
        public void DeletePorCliente(int rcejeucodi)
        {
            try
            {
                FactorySic.GetRcaCuadroEjecUsuarioDetRepository().DeletePorCliente(rcejeucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla RCA_ESQUEMA_UNIFILAR

        /// <summary>
        /// Inserta un registro de la tabla RCA_ESQUEMA_UNIFILAR
        /// </summary>
        public void SaveRcaEsquemaUnifilar(RcaEsquemaUnifilarDTO entity)
        {
            try
            {
                FactorySic.GetRcaEsquemaUnifilarRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_ESQUEMA_UNIFILAR
        /// </summary>
        public void UpdateRcaEsquemaUnifilar(RcaEsquemaUnifilarDTO entity)
        {
            try
            {
                FactorySic.GetRcaEsquemaUnifilarRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_ESQUEMA_UNIFILAR
        /// </summary>
        public void DeleteRcaEsquemaUnifilar(int rcesqucodi)
        {
            try
            {
                FactorySic.GetRcaEsquemaUnifilarRepository().Delete(rcesqucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_ESQUEMA_UNIFILAR
        /// </summary>
        public RcaEsquemaUnifilarDTO GetByIdRcaEsquemaUnifilar(int rcesqucodi)
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().GetById(rcesqucodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_ESQUEMA_UNIFILAR
        /// </summary>
        public List<RcaEsquemaUnifilarDTO> ListRcaEsquemaUnifilars()
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaEsquemaUnifilar
        /// </summary>
        public List<RcaEsquemaUnifilarDTO> GetByCriteriaRcaEsquemaUnifilars()
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar esquema unifilar para reporte Excel
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoSuminitro"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarExcel(string empresa, string codigoSuminitro, string fecIni, string fecFin, int origen)
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().ListarEsquemaUnifilarExcel(empresa, codigoSuminitro, fecIni, fecFin, origen);
        }

        /// <summary>
        /// Permite listar historial de un esquema unifilar
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarHistorial(int emprcodi, int equicodi)
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().ListarEsquemaUnifilarHistorial(emprcodi, equicodi);
        }

        /// <summary>
        /// Permite obtener esquema unifilar por codigo
        /// </summary>
        /// <param name="rccarecodi"></param>
        /// <returns></returns>
        public RcaEsquemaUnifilarDTO ObtenerEsquemaUnifilarlPorCodigo(int rccarecodi)
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().ObtenerPorCodigo(rccarecodi);
        }
        /// <summary>
        ///  Permite listar esquema unifilar basado en criterios de busqueda
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoSuminitro"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="origen"></param>
        /// <param name="regIni"></param>
        /// <param name="regFin"></param>
        /// <returns></returns>
        public List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarFiltro(string empresa, string codigoSuminitro, string fecIni, string fecFin, int origen, int regIni, int regFin)
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().ListarEsquemaUnifilarFiltro(empresa, codigoSuminitro, fecIni, fecFin, origen, regIni, regFin);
        }

        /// <summary>
        ///  Permite mostrar cantidad registros esquema unifilar basado en criterios de busqueda
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoSuminitro"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="origen"></param>
        /// <returns></returns>
        public int ListarEsquemaUnifilarCount(string empresa, string codigoSuminitro, string fecIni, string fecFin, int origen)
        {
            return FactorySic.GetRcaEsquemaUnifilarRepository().ListarEsquemaUnifilarCount(empresa, codigoSuminitro, fecIni, fecFin, origen);
        }

        #endregion

        #region Métodos Tabla RCA_PARAM_ESQUEMA

        /// <summary>
        /// Inserta un registro de la tabla RCA_PARAM_ESQUEMA
        /// </summary>
        public void SaveRcaParamEsquema(RcaParamEsquemaDTO entity)
        {
            try
            {
                FactorySic.GetRcaParamEsquemaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_PARAM_ESQUEMA
        /// </summary>
        public void UpdateRcaParamEsquema(RcaParamEsquemaDTO entity)
        {
            try
            {
                FactorySic.GetRcaParamEsquemaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_PARAM_ESQUEMA
        /// </summary>
        public void DeleteRcaParamEsquema(int rcparecodi)
        {
            try
            {
                FactorySic.GetRcaParamEsquemaRepository().Delete(rcparecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_PARAM_ESQUEMA
        /// </summary>
        public RcaParamEsquemaDTO GetByIdRcaParamEsquema(int rcparecodi)
        {
            return FactorySic.GetRcaParamEsquemaRepository().GetById(rcparecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_PARAM_ESQUEMA
        /// </summary>
        public List<RcaParamEsquemaDTO> ListRcaParamEsquemas()
        {
            return FactorySic.GetRcaParamEsquemaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaParamEsquema 
        /// </summary>
        public List<RcaParamEsquemaDTO> GetByCriteriaRcaParamEsquemas()
        {
            return FactorySic.GetRcaParamEsquemaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas por filtros en la tabla RcaParamEsquema
        /// </summary>
        public List<RcaParamEsquemaDTO> ListarRcaParamEsquemaPorFiltros(string anio, string tipoEmpresa)
        {
            return FactorySic.GetRcaParamEsquemaRepository().ListarPorFiltros(anio, tipoEmpresa);
        }

        /// <summary>
        /// Permite obtener la lista de los diferentes valores RCPAREANIO en la tabla RcaParamEsquema
        /// </summary>
        public List<int> ListarAniosParametroEsquema()
        {
            return FactorySic.GetRcaParamEsquemaRepository().ListarAniosParametroEsquema();
        }

        /// <summary>
        /// Permite obtener lista por punto de medicion
        /// </summary>
        /// <param name="listaPuntoMedicion"></param>
        /// <returns></returns>
        public List<RcaParamEsquemaDTO> ListarRcaParamEsquemaPorPuntoMedicion(string listaPuntoMedicion)
        {
            return FactorySic.GetRcaParamEsquemaRepository().ListarPorPuntoMedicion(listaPuntoMedicion);
        }

        #endregion

        #region Métodos Tabla RCA_PROGRAMA

        /// <summary>
        /// Inserta un registro de la tabla RCA_PROGRAMA
        /// </summary>
        public int SaveRcaPrograma(RcaProgramaDTO entity)
        {
            var res = 0;
            try
            {
                res = FactorySic.GetRcaProgramaRepository().Save(entity);
                //res = FactorySic.GetRcaProgramaRepository().CrearPrograma(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return res;
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_PROGRAMA
        /// </summary>
        public void UpdateRcaPrograma(RcaProgramaDTO entity)
        {
            try
            {
                FactorySic.GetRcaProgramaRepository().Update(entity);
                //FactorySic.GetRcaProgramaRepository().ActualizarPrograma(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_PROGRAMA
        /// </summary>
        public void DeleteRcaPrograma(int rcprogcodi, string usuario)
        {
            try
            {
                FactorySic.GetRcaProgramaRepository().Delete(rcprogcodi, usuario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_PROGRAMA
        /// </summary>
        public RcaProgramaDTO GetByIdRcaPrograma(int rcprogcodi)
        {
            return FactorySic.GetRcaProgramaRepository().GetById(rcprogcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_PROGRAMA
        /// </summary>
        public List<RcaProgramaDTO> ListRcaProgramas()
        {
            return FactorySic.GetRcaProgramaRepository().List();
        }

        public List<RcaProgramaDTO> ListProgramaEnvioArchivo(DateTime fechaReferencia)
        {
            return FactorySic.GetRcaProgramaRepository().ListProgramaEnvioArchivo(fechaReferencia);
        }

        public List<RcaProgramaDTO> ListProgramasRechazoCarga(bool muestraVigentes)
        {
            return FactorySic.GetRcaProgramaRepository().ListProgramaRechazoCarga(muestraVigentes);
        }

        public List<RcaProgramaDTO> ListProgramaFiltro(int horizonte, string codigoPrograma, string nombrePrograma, int reprograma)
        {
            return FactorySic.GetRcaProgramaRepository().ListProgramaFiltro(horizonte, codigoPrograma, nombrePrograma, reprograma);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaPrograma
        /// </summary>
        public List<RcaProgramaDTO> GetByCriteriaRcaProgramas(string codigoProgramaAbrev)
        {
            return FactorySic.GetRcaProgramaRepository().GetByCriteria(codigoProgramaAbrev);
        }
        /***
        public List<RcaProgramaRechazoCargaDTO> ListProgramaRechazoCarga(string empresasId, string codigoCuadroPrograma)
        {
            return FactorySic.GetRcaProgramaRepository().ListProgramaRechazoCarga(empresasId, codigoCuadroPrograma);
        }

        public List<RcaProgramaRechazoCargaDTO> ListEmpresasProgramaRechazoCarga(string zona, string estacion, decimal medicion, string nombreEmpresa, string empresasId, string equiposId)
        {
            return FactorySic.GetRcaProgramaRepository().ListEmpresasProgramaRechazoCarga(zona, estacion, medicion, nombreEmpresa, empresasId, equiposId);
        }
        **/
        #endregion

        #region Métodos Tabla RCA_REGISTRO_SVRM

        /// <summary>
        /// Inserta un registro de la tabla RCA_REGISTRO_SVRM
        /// </summary>
        public void SaveRcaRegistroSvrm(RcaRegistroSvrmDTO entity)
        {
            try
            {
                FactorySic.GetRcaRegistroSvrmRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RCA_REGISTRO_SVRM
        /// </summary>
        public void UpdateRcaRegistroSvrm(RcaRegistroSvrmDTO entity)
        {
            try
            {
                FactorySic.GetRcaRegistroSvrmRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RCA_REGISTRO_SVRM
        /// </summary>
        public void DeleteRcaRegistroSvrm(int rcsvrmcodi)
        {
            try
            {
                FactorySic.GetRcaRegistroSvrmRepository().Delete(rcsvrmcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RCA_REGISTRO_SVRM
        /// </summary>
        public RcaRegistroSvrmDTO GetByIdRcaRegistroSvrm(int rcsvrmcodi)
        {
            return FactorySic.GetRcaRegistroSvrmRepository().GetById(rcsvrmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RCA_REGISTRO_SVRM
        /// </summary>
        public List<RcaRegistroSvrmDTO> ListRcaRegistroSvrms()
        {
            return FactorySic.GetRcaRegistroSvrmRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RcaRegistroSvrm
        /// </summary>
        public List<RcaRegistroSvrmDTO> GetByCriteriaRcaRegistroSvrms()
        {
            return FactorySic.GetRcaRegistroSvrmRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener lista de registros Svrm basado en criterios de busqueda para reporte Excel
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoSuministro"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="maxDemComprometidaIni"></param>
        /// <param name="maxDemComprometidaFin"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns>
        public List<RcaRegistroSvrmDTO> ListRcaRegistroSvrmsExcel(string empresa, string codigoSuministro, string fecIni, 
            string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro)
        {
            return FactorySic.GetRcaRegistroSvrmRepository().ListRcaRegistroSvrmsExcel(empresa, codigoSuministro, fecIni, 
                fecFin, maxDemComprometidaIni, maxDemComprometidaFin, estadoRegistro);
        }

        /// <summary>
        /// Permite obtener registro Svrm por codigo
        /// </summary>
        /// <param name="Rcsvrmcodi"></param>
        /// <returns></returns>
        public RcaRegistroSvrmDTO ObtenerRegistroSvrmPorCodigo(int Rcsvrmcodi)
        {
            return FactorySic.GetRcaRegistroSvrmRepository().ObtenerPorCodigo(Rcsvrmcodi);
        }

        /// <summary>
        /// Permite obtener lista de registros Svrm basado en criterios de busqueda
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoSuministro"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="maxDemComprometidaIni"></param>
        /// <param name="maxDemComprometidaFin"></param>
        /// <param name="estadoRegistro"></param>
        /// <param name="regIni"></param>
        /// <param name="regFin"></param>
        /// <returns></returns>
        public List<RcaRegistroSvrmDTO> ListRcaRegistroSvrmsFiltro(string empresa, string codigoSuministro, string fecIni,
            string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro, int regIni, int regFin)
        {
            return FactorySic.GetRcaRegistroSvrmRepository().ListRcaRegistroSvrmsFiltro(empresa, codigoSuministro, fecIni,
                fecFin, maxDemComprometidaIni, maxDemComprometidaFin, estadoRegistro, regIni, regFin);
        }

        /// <summary>
        /// Permite obtener cantidad de registros Svrm basado en criterios de busqueda
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="codigoSuministro"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="maxDemComprometidaIni"></param>
        /// <param name="maxDemComprometidaFin"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns>
        public int ListRcaRegistroSvrmsCount(string empresa, string codigoSuministro, string fecIni,
          string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro)
        {
            return FactorySic.GetRcaRegistroSvrmRepository().ListRcaRegistroSvrmsCount(empresa, codigoSuministro, fecIni,
                fecFin, maxDemComprometidaIni, maxDemComprometidaFin, estadoRegistro);
        }


        #endregion

        #region  Métodos Tabla RCA_HORIZONTE
        /// <summary>
        /// Permite listar los tipos de horizonte
        /// </summary>
        /// <returns></returns>
        public List<RcaHorizonteProgDTO> ListHorizonteProg()
        {
            return FactorySic.GetRcaCuadroProgRepository().ListHorizonteProg();
        }
        #endregion

        #region Métodos Tabla RCA_CONFIGURACION
        /// <summary>
        /// Permite listar los tipos de configuracion
        /// </summary>
        /// <returns></returns>
        public List<RcaConfiguracionProgDTO> ListConfiguracionProg()
        {
            return FactorySic.GetRcaCuadroProgRepository().ListConfiguracionProg();
        }
        #endregion

        #region Métodos Tabla RCA_CUADRO_ESTADO
        /// <summary>
        /// Permite listar los estados del cuadro
        /// </summary>
        /// <returns></returns>
        public List<RcaCuadroEstadoDTO> ListEstadoCuadroProg()
        {
            return FactorySic.GetRcaCuadroProgRepository().ListEstadoCuadroProg();
        }
        #endregion

        #region Métodos Tabla RCA_DEMANDA_USUARIO
        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioReporte(string periodo, string codigoZona, string codigoPuntoMedicion,string empresa, string suministrador, int regIni, int regFin)
        {
            var lista = FactorySic.GetRcaDemandaUsuarioRepository().ListDemandaUsuarioReporte(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador, regIni, regFin);

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            foreach (var item in lista)
            {
                FormatearRcaDemandaUsuarioDTO(item, listaBloqueHorario);
            }

            return lista;
        }

        public int ListDemandaUsuarioReporteCount(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador)
        {
            return FactorySic.GetRcaDemandaUsuarioRepository().ListDemandaUsuarioReporteCount(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador);
        }
        public List<MeEnvioDTO> ListaPeriodoReporte(string fecha)
        {
            return FactorySic.GetRcaDemandaUsuarioRepository().ObtenerListaPeriodoReporte(fecha);
        }

        public int ObtenerDemandaUsuarioMaximoId()
        {
            return FactorySic.GetRcaDemandaUsuarioRepository().GetMaxId();
        }

        public void SaveRcaDemandaUsuario(RcaDemandaUsuarioDTO entity){

            try
            {
                FactorySic.GetRcaDemandaUsuarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }           
        }

        public void DeleteRcaDemandaUsuario(string periodo)
        {

            try
            {
                FactorySic.GetRcaDemandaUsuarioRepository().Delete(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EqEquipoDTO> ObtenerEquipos()
        {
            return FactorySic.GetRcaDemandaUsuarioRepository().ObtenerEquipos();
        }

        /// <summary>
        /// Muestra listado paginado de clientes que deden de validarse
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="regIni"></param>
        /// <param name="regFin"></param>
        /// <returns></returns>
        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioErroresPag(string periodo, int regIni, int regFin)
        {
            var lista = FactorySic.GetRcaDemandaUsuarioRepository().ListDemandaUsuarioErroresPag(periodo, regIni, regFin);

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            foreach (var item in lista)
            {
                FormatearRcaDemandaUsuarioDTO(item, listaBloqueHorario);
            }

            return lista;
        }

        /// <summary>
        /// Muestra listado total de clientes que deben de validarse
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioErroresExcel(string periodo)
        {
            var lista = FactorySic.GetRcaDemandaUsuarioRepository().ListDemandaUsuarioErroresExcel(periodo);

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            foreach (var item in lista)
            {
                FormatearRcaDemandaUsuarioDTO(item, listaBloqueHorario);
            }

            return lista;
        }

        /// <summary>
        /// Muestra listado para exportar a archivo Excel
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="codigoZona"></param>
        /// <param name="codigoPuntoMedicion"></param>
        /// <param name="empresa"></param>
        /// <param name="suministrador"></param>
        /// <returns></returns>
        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioReporteExcel(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador)
        {
            var lista = FactorySic.GetRcaDemandaUsuarioRepository().ListDemandaUsuarioReporteExcel(periodo, codigoZona, codigoPuntoMedicion, empresa, suministrador);

            //Hora punta segun fecha de vigencia
            List<SiParametroValorDTO> listaBloqueHorario = (new ParametroAppServicio()).ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            foreach (var item in lista)
            {
                FormatearRcaDemandaUsuarioDTO(item, listaBloqueHorario);
            }

            return lista;
        }

        private void FormatearRcaDemandaUsuarioDTO(RcaDemandaUsuarioDTO obj, List<SiParametroValorDTO> listaBloqueHorario)
        {
            MedidoresHelper.ObtenerValorHXPeriodoRcaDemandaUsuario96(ConstantesRepMaxDemanda.TipoHoraPunta,obj.Rcdeulfecmaxdem.Date, new List<RcaDemandaUsuarioDTO>() { obj},
                                            null, listaBloqueHorario, out decimal valorHP, out int hHP, out DateTime fechaHoraHP);

            MedidoresHelper.ObtenerValorHXPeriodoRcaDemandaUsuario96(ConstantesRepMaxDemanda.TipoFueraHoraPunta, obj.Rcdeulfecmaxdem.Date, new List<RcaDemandaUsuarioDTO>() { obj },
                                            null, listaBloqueHorario, out decimal valorFHP, out int hFHP, out DateTime fechaHoraFHP);

            obj.Rcdeuldemandahp = valorHP;
            obj.Rcdeuldemandahfp = valorFHP;
        }

        #endregion

        /// <summary>
        /// Permite listar los Suministradores modulo Rechazo Carga
        /// </summary>
        public List<RcaSuministradorDTO> ListRcaSuministradores()
        {
            return FactorySic.GetMePtosuministradorRepository().ListaSuministradoresRechazoCarga();
        }

        /// <summary>
        /// Permite listar los Tipo Empresa 
        /// </summary>
        public List<SiTipoempresaDTO> ListSiTipoEmpresa()
        {
            var codigos = new List<int> { 0, 2, 4 };
            var tiposEmpresa = FactorySic.GetSiTipoempresaRepository().List();
            return tiposEmpresa.Where(x => codigos.Contains(x.Tipoemprcodi)).OrderBy(x=> x.Tipoemprcodi).ToList();
        }

        /// <summary>
        /// Permite listar las empresas para rechazo carga por Tipo y Estado
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasRechazoCarga(string empresa, int tipoEmpresa, string estadoRegistro)
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasRechazoCarga(empresa, tipoEmpresa, estadoRegistro);
        }

        /// <summary>
        /// Permite listar los equipos por familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposPorFamilia(int emprcodi, int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPorFamiliaRechazoCarga(emprcodi, famcodi);   
        }

        #region Reportes Modulo Rechazo Carga

        /// <summary>
        /// Metodo Reporte Total Datos
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteTotalDatos(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteTotalDatos(codigoCuadroPrograma, eventoCTAF);
        }
        /// <summary>
        /// Metodo Reporte Demora Ejecucion
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteDemoraEjecucion(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteDemoraEjecucion(codigoCuadroPrograma, eventoCTAF);
        }
        /// <summary>
        ///  Metodo Reporte Demora Reestablecimiento
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteDemoraReestablecimiento(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteDemoraReestablecimiento(codigoCuadroPrograma, eventoCTAF);
        }
        /// <summary>
        /// Metodo Reporte Interrupciones Menores
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteInterrupcionesMenores(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteInterrupcionesMenores(codigoCuadroPrograma, eventoCTAF);
        }

        /// <summary>
        /// Metodo Reporte Demoras Finalizar 
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteDemorasFinalizar(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteDemorasFinalizar(codigoCuadroPrograma, eventoCTAF);
        }

        /// <summary>
        /// Metodo Reporte Interrupciones por Resarcimiento
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteDemorasResarcimiento(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteDemorasResarcimiento(codigoCuadroPrograma, eventoCTAF);
        }

        /// <summary>
        /// Metodo Reporte Evaluacion Cumplimiento
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteEvaluacionCumplimiento(int codigoCuadroPrograma, string eventoCTAF)
        {
            var listaClientes = new List<RcaCuadroProgUsuarioDTO>();
            try
            {
                FactorySic.GetRcaCuadroProgUsuarioRepository().DeleteTablasTemporalesReporteCumplimiento(eventoCTAF);
                
                var listaDatosInicio = FactorySic.GetRcaCuadroProgUsuarioRepository().ObtenerDatosEvaluacionCumplimiento(eventoCTAF);

                foreach(var cliente in listaDatosInicio)
                {
                    FactorySic.GetRcaCuadroProgUsuarioRepository().InsertarTablaTempEvaluacionCumplimiento(eventoCTAF, cliente.Emprcodi, cliente.Equicodi.Value,
                        cliente.Rcejeufechorinicio, cliente.Rcejeufechorfin);
                }

                FactorySic.GetRcaCuadroProgUsuarioRepository().ActualizacionCalculoPorMinutoEvaluacionCumplimiento(eventoCTAF);
                FactorySic.GetRcaCuadroProgUsuarioRepository().RegistrarIntervaloEvaluacionCumplimiento(eventoCTAF);

                var equicodiAnterior = 0;
                var fechaAnterior = DateTime.Now;
                var grupo = 0;

                var listaEvaluacionTemp = FactorySic.GetRcaCuadroProgUsuarioRepository().ObtenerValoresEvaluacionCliente(eventoCTAF);

                foreach(var cliente in listaEvaluacionTemp)
                {
                    if(cliente.Equicodi != equicodiAnterior)
                    {
                        grupo = 1;
                    }
                    else
                    {
                        if(cliente.Rcermcfechorini.AddMinutes(-15).ToString("dd/MM/yyyy HH:mm") != fechaAnterior.ToString("dd/MM/yyyy HH:mm"))
                        {
                            grupo++;
                        }
                    }

                    FactorySic.GetRcaCuadroProgUsuarioRepository().ActualizarEvaluacionCliente(eventoCTAF, grupo, cliente.Equicodi, cliente.Rcermcfechorini, cliente.Rcermcfechorfin);

                    equicodiAnterior = cliente.Equicodi;
                    fechaAnterior = cliente.Rcermcfechorfin;
                }


                listaEvaluacionTemp = FactorySic.GetRcaCuadroProgUsuarioRepository().ObtenerValoresGeneralesEvaluacionCumplimientoTmp(eventoCTAF);

                foreach (var cliente in listaEvaluacionTemp)
                {
                    var valorPromedio = FactorySic.GetRcaCuadroProgUsuarioRepository().ObtenerValorPromedioEvaluacionCumplimiento(eventoCTAF, cliente.Equicodi, cliente.Rcermcfechorini);
                    FactorySic.GetRcaCuadroProgUsuarioRepository().ActualizarValorPromedioEvaluacionCumplimiento(eventoCTAF, cliente.Equicodi, valorPromedio, cliente.Rcermcgrupo);
                }

                listaEvaluacionTemp = FactorySic.GetRcaCuadroProgUsuarioRepository().ObtenerDatosCalculoFinalTempEvaluacionCumplimiento(eventoCTAF);

                foreach (var cliente in listaEvaluacionTemp)
                {                   
                    FactorySic.GetRcaCuadroProgUsuarioRepository().ActualizarDatosCalculoFinalEvaluacionCumplimiento(eventoCTAF, cliente.Rcermcpotencianorechazada.Value, 
                        cliente.Rcermcpotenciapromprevia.Value, cliente.Rcermcfechorini, cliente.Rcermcfechorfin, cliente.Equicodi);
                }

                FactorySic.GetRcaCuadroProgUsuarioRepository().InsertarEvaluacionFinal(eventoCTAF);
                FactorySic.GetRcaCuadroProgUsuarioRepository().ActualizarEvaluacionFinal(eventoCTAF);

                listaClientes = FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteEvalucionCumplimientoRMC(codigoCuadroPrograma, eventoCTAF);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return listaClientes;
        }

        /// <summary>
        /// Metodo Reporte Interrupciones por Informe Técnico
        /// </summary>
        /// <param name="codigoCuadroPrograma"></param>
        /// <param name="eventoCTAF"></param>
        /// <returns></returns>
        public List<RcaCuadroProgUsuarioDTO> ReporteInterrrupInformeTecnico(int codigoCuadroPrograma, string eventoCTAF)
        {
            return FactorySic.GetRcaCuadroProgUsuarioRepository().ReporteInterrrupInformeTecnico(codigoCuadroPrograma, eventoCTAF);
        }

        #endregion

    }
}
