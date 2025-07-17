using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Titularidad;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    /// <summary>
    /// Clases con métodos del módulo Equipamiento
    /// </summary>
    public class EquipamientoAppServicio : AppServicioBase
    {
        FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        /// <summary>
        /// Propiedades de las coordenadas
        /// </summary>
        private const int PropiedadCoorX = 1814;
        private const int PropiedadCoorY = 1815;


        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EquipamientoAppServicio));

        #region Métodos Tabla EQ_EQUIPO

        /// <summary>
        /// Crea un registro nuevo en la tabla EQ_EQUIPO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Código del equipo creado</returns>
        public int SaveEqEquipo(EqEquipoDTO entity)
        {
            try
            {
                return FactorySic.GetEqEquipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_EQUIPO
        /// </summary>
        public void UpdateEqEquipo(EqEquipoDTO entity)
        {
            try
            {
                FactorySic.GetEqEquipoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_EQUIPO
        /// </summary>
        public void DeleteEqEquipo(int equicodi, string username)
        {
            try
            {
                FactorySic.GetEqEquipoRepository().Delete(equicodi);
                FactorySic.GetEqEquipoRepository().Delete_UpdateAuditoria(equicodi, username);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_EQUIPO
        /// </summary>
        public EqEquipoDTO GetByIdEqEquipo(int equicodi)
        {
            return FactorySic.GetEqEquipoRepository().GetById(equicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_EQUIPO
        /// </summary>
        public List<EqEquipoDTO> ListEqEquipos()
        {
            return FactorySic.GetEqEquipoRepository().List();
        }

        /// <summary>
        /// Permite listar los registros con campos basicos de la tabla EQ_EQUIPO
        /// </summary>
        public List<EqEquipoDTO> BasicListEqEquipos()
        {
            return FactorySic.GetEqEquipoRepository().BasicList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqEquipo
        /// </summary>
        public List<EqEquipoDTO> GetByCriteriaEqEquipos()
        {
            return FactorySic.GetEqEquipoRepository().GetByCriteria();
        }
        /// <summary>
        /// Listado de centrales para Osinergmin
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListadoCentralesOsinergmin()
        {
            return FactorySic.GetEqEquipoRepository().ListadoCentralesOsinergmin();
        }

        /// <summary>
        /// Listado de Equipos por filtro de Empresa, Familia, Tipo Empresa y Estado Equipo
        /// </summary>
        /// <param name="idEmpresa">Código Empresa</param>
        /// <param name="sEstado">Estado de Equipo</param>
        /// <param name="idTipoEquipo">Código Familia</param>
        /// <param name="idTipoEmpresa">Código Tipo Empresa</param>
        /// <param name="sNombreEquipo">Nombre de Equipo</param>
        /// <param name="idPadre">Código Padre Equipo * Usar -99 para evitar este filtro*</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposxFiltro(int idEmpresa, string sEstado, int idTipoEquipo, int idTipoEmpresa,
            string sNombreEquipo, int idPadre)
        {
            return FactorySic.GetEqEquipoRepository()
                .ListarEquiposxFiltro(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre);
        }

        public List<EqEquipoDTO> ListarEquiposxFiltroPaginado(int idEmpresa, string sEstado, int idTipoEquipo,
            int idTipoEmpresa, string sNombreEquipo, int idPadre, int nroPagina, int nroFilas, ref int totalPaginas,
            ref int totalRegistros)
        {
            return FactorySic.GetEqEquipoRepository()
                .ListarEquiposxFiltroPaginado(idEmpresa, sEstado, idTipoEquipo, idTipoEmpresa, sNombreEquipo, idPadre,
                nroPagina, nroFilas, ref totalPaginas, ref totalRegistros);
        }

        /// <summary>
        /// Permite obtener el detalle de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public EqEquipoDTO ObtenerDetalleEquipo(int idEquipo)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerDetalleEquipo(idEquipo);
        }

        /// <summary>
        /// Listado de Equipos filtrado por nombre.
        /// Datos solo de tabla Equipo
        /// </summary>
        /// <param name="coincidencia">Nombre del Equipo</param>
        /// <param name="nroPagina">N° de página</param>
        /// <param name="nroFilas">N° de Filas</param>
        /// <returns></returns>
        public List<EqEquipoDTO> BuscarEquipoxNombre(string coincidencia, int nroPagina, int nroFilas)
        {
            return FactorySic.GetEqEquipoRepository().BuscarEquipoxNombre(coincidencia, nroPagina, nroFilas);
        }

        /// <summary>
        /// Listado de Equipos filtrado por varias familias.
        /// Datos de Equipo, Familia, Empresa y Area
        /// </summary>
        /// <param name="iCodFamilias">Código de Familias</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoxFamilias(params int[] iCodFamilias)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquipoxFamilias(iCodFamilias);
        }
        /// <summary>
        /// Listado de Equipos filtrado por varias familias y empresas.
        /// Datos de Equipo, Familia, Empresa y Area
        /// </summary>
        /// <param name="iCodFamilias">Código de Familias</param>
        /// <param name="iEmpresas">Código de Empresas</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoxFamiliasxEmpresas(int[] iCodFamilias, int[] iEmpresas)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquipoxFamiliasEmpresas(iCodFamilias, iEmpresas);
        }

        /// <summary>
        /// Listado que Equipos para el nuevo módulo de Equipamiento
        /// </summary>
        /// <param name="iEmpresa"></param>
        /// <param name="iFamilia"></param>
        /// <param name="iTipoEmpresa"></param>
        /// <param name="iEquipo"></param>
        /// <param name="sEstado"></param>
        /// <param name="nombre"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaEquipamientoPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo,
            string sEstado, string nombre, int nroPagina, int nroFilas)
        {
            try
            {
                return FactorySic.GetEqEquipoRepository()
                    .ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre, nroPagina, nroFilas)
                                    .OrderBy(x => x.EMPRNOMB).ThenBy(x => x.AREANOMB).ThenBy(x => x.Equiabrev).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Cantidad de Equipos para el nuevo módulo de Equipamiento
        /// </summary>
        /// <param name="iEmpresa">Códgo de Empresa</param>
        /// <param name="iFamilia">Código de Familia</param>
        /// <param name="iTipoEmpresa">Código de Tipo Empresa</param>
        /// <param name="iEquipo">Código de Equipo</param>
        /// <param name="sEstado">Estado de equipo</param>
        /// <returns>Cantidad de Equipos</returns>
        public int TotalEquipamiento(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre)
        {
            try
            {
                return FactorySic.GetEqEquipoRepository()
                    .TotalEquipamiento(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los equipos con procedimientos de maniobras
        /// </summary>
        /// <param name="famCodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ObtenerEquiposProcManiobras(int famCodi, int propCodi)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquiposProcManiobras(famCodi, propCodi);
        }

        /// <summary>
        /// Permite actualizar el procedimiento de maniobra
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="idFamilia"></param>
        /// <param name="enlace"></param>
        /// <param name="lastuser"></param>
        public void ActualizarProcedimientoManiobra(int idPropiedad, int idEquipo, string enlace, string lastuser)
        {
            try
            {
                FactorySic.GetEqPropequiRepository().EliminarHistorico(idPropiedad, idEquipo);

                var entity = new EqPropequiDTO();
                entity.Equicodi = idEquipo;
                entity.Fechapropequi = DateTime.Today;
                entity.Propequifeccreacion = DateTime.Now;
                entity.Propequiusucreacion = lastuser;
                entity.Propcodi = idPropiedad;
                entity.Valor = enlace;

                SaveEqPropequi(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Permite obtener las coodenadas geográficas de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="coordenadaX"></param>
        /// <param name="coordenadaY"></param>
        public void ObtenerCoordenadasEquipo(int idEquipo, out string coordenadaX, out string coordenadaY)
        {
            coordenadaX = string.Empty;
            coordenadaY = string.Empty;

            var listX = FactorySic.GetEqPropequiRepository()
                    .ListarValoresHistoricosPropiedadPorEquipo(idEquipo, PropiedadCoorX.ToString()).Where(x => x.Propequideleted == 0).OrderByDescending(x => x.Fechapropequi).ToList();

            var listY = FactorySic.GetEqPropequiRepository()
                    .ListarValoresHistoricosPropiedadPorEquipo(idEquipo, PropiedadCoorY.ToString()).Where(x => x.Propequideleted == 0).OrderByDescending(x => x.Fechapropequi).ToList();

            if (listX.Count > 0) coordenadaX = listX[0].Valor;
            if (listY.Count > 0) coordenadaY = listY[0].Valor;
        }

        /// <summary>
        /// Permite grabar las coordenadas de un equipo
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="coordenadaX"></param>
        /// <param name="coordenadaY"></param>
        /// <param name="lastUser"></param>
        public void GrabarCoordenadas(int idEquipo, string coordenadaX, string coordenadaY, string lastUser)
        {
            try
            {
                var propCoordenadaX = new EqPropequiDTO
                {
                    Equicodi = idEquipo,
                    Fechapropequi = DateTime.Today,
                    Propequifeccreacion = DateTime.Now,
                    Propequiusucreacion = lastUser,
                    Propcodi = PropiedadCoorX,
                    Valor = coordenadaX
                };

                var propCoordenadaY = new EqPropequiDTO
                {
                    Equicodi = idEquipo,
                    Fechapropequi = DateTime.Today,
                    Propequifeccreacion = DateTime.Now,
                    Propequiusucreacion = lastUser,
                    Propcodi = PropiedadCoorY,
                    Valor = coordenadaY
                };

                this.SaveEqPropequi(propCoordenadaX);
                this.SaveEqPropequi(propCoordenadaY);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //inicio modificado
        public List<EqEquipoDTO> GetListaEquipoXEmpresaYFamilia(int emprcodi, int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPorFamilia(emprcodi, famcodi);
        }
        //fin modificado

        /// <summary>
        /// Devuelve lista de equipo por empresa y familia
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> GetByCriteriaEqequipo(int emprcodi, int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().GetByEmprFam(emprcodi, famcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_EQUIPO Con filtro familia y empresa
        /// </summary>
        public List<EqEquipoDTO> ListaEqEmprFamilia(int emprcodi, int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().ListaEqEmpresaFamilia(emprcodi, famcodi);
        }

        #endregion

        #region Métodos Tabla EQ_AREA

        /// <summary>
        /// Inserta un registro de la tabla EQ_AREA
        /// </summary>
        public void SaveEqArea(EqAreaDTO entity)
        {
            try
            {
                FactorySic.GetEqAreaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_AREA
        /// </summary>
        public void UpdateEqArea(EqAreaDTO entity)
        {
            try
            {
                FactorySic.GetEqAreaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_AREA
        /// </summary>
        public void DeleteEqArea(int areacodi, string username)
        {
            try
            {
                FactorySic.GetEqAreaRepository().Delete(areacodi);
                FactorySic.GetEqAreaRepository().Delete_UpdateAuditoria(areacodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_AREA
        /// </summary>
        public EqAreaDTO GetByIdEqArea(int areacodi)
        {
            return FactorySic.GetEqAreaRepository().GetById(areacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_AREA
        /// </summary>
        public List<EqAreaDTO> ListEqAreas()
        {
            return FactorySic.GetEqAreaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqArea
        /// </summary>
        public List<EqAreaDTO> GetByCriteriaEqAreas()
        {
            return FactorySic.GetEqAreaRepository().GetByCriteria();
        }

        /// <summary>
        /// Listado de áreas filtradas por tipo de área y nombre de área
        /// </summary>
        /// <param name="idTipoArea">Id de Tipo de Área, para evitar este filtro usar -99</param>
        /// <param name="strNombreArea">Nombre de Área</param>
        /// <param name="nroPagina">Cantidad de Página</param>
        /// <param name="nroFilas">Cantidad de Registros por Página</param>
        /// <returns></returns>
        public List<EqAreaDTO> ListarxFiltro(int idTipoArea, string strNombreArea, string strEstado, int nroPagina, int nroFilas)
        {
            return FactorySic.GetEqAreaRepository().ListarxFiltro(idTipoArea, strNombreArea, strEstado, nroPagina, nroFilas);
        }

        /// <summary>
        /// Obtiene la cantidad total de resultados de la Consulta ListarxFiltro
        /// </summary>
        /// <param name="idTipoArea">Id de Tipo de Área, para evitar este filtro usar -99</param>
        /// <param name="strNombreArea">Nombre de Área</param>
        /// <returns></returns>
        public int CantidadListarxFiltro(int idTipoArea, string strNombreArea, string strEstado)
        {
            return FactorySic.GetEqAreaRepository().CantidadListarxFiltro(idTipoArea, strNombreArea, strEstado);
        }

        public List<EqAreaDTO> ObtenerAreaPorEmpresa(int idEmpresa)
        {
            return FactorySic.GetEqAreaRepository().ObtenerAreaPorEmpresa(idEmpresa);
        }

        public List<EqEquipoDTO> ObtenerEquipoPorAreaEmpresa(int idEmpresa, int idArea)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPorAreaEmpresa(idEmpresa, idArea);
        }
        public List<EqEquipoDTO> ObtenerEquipoPadresHidrologicosCuenca()
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPadresHidrologicosCuenca();
        }
        public List<EqEquipoDTO> ObtenerEquipoPorAreaEmpresaTodos(int idEmpresa, int idArea)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPorAreaEmpresaTodos(idEmpresa, idArea);
        }
        public List<EqEquipoDTO> ObtenerEquipoPadresHidrologicosCuencaTodos()
        {
            return FactorySic.GetEqEquipoRepository().ObtenerEquipoPadresHidrologicosCuencaTodos();
        }
        /// <summary>
        /// Listado de todas las ubicaciones(áreas) activas
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ListaTodasAreasActivas()
        {
            return FactorySic.GetEqAreaRepository().ListaTodasAreasActivas();
        }
        /// <summary>
        /// Listado de todas las ubicaciones(áreas) activas filtradas por tipo área
        /// </summary>
        /// <param name="iTipoArea">Código tipo de área,para evitar este filtro usar -99</param>
        /// <returns></returns>
        public List<EqAreaDTO> ListaTodasAreasActivasPorTipoArea(int iTipoArea)
        {
            return FactorySic.GetEqAreaRepository().ListaTodasAreasActivasPorTipoArea(iTipoArea);
        }

        /// <summary>
        /// Permite listar las areas definidas como zonas y que estén activas
        /// </summary>
        /// revisar esto################################
        public List<EqAreaDTO> ListarZonasActivas()
        {
            return FactorySic.GetEqAreaRepository().ListarZonasActivas();
        }

        /// <summary>
        /// Permite listar las areas definidas como zonas (activas) y filtradas por niveles
        /// </summary>
        public List<EqAreaDTO> ListarZonasxNivel(int anivelcodi)
        {
            return FactorySic.GetEqAreaRepository().ListarZonasxNivel(anivelcodi);
        }

        /// <summary>
        /// Inserta una nueva zona en la tabla EQ_AREA
        /// </summary>
        public int InsertarEqArea(EqAreaDTO entity)
        {
            int id = -1;
            try
            {
                id = FactorySic.GetEqAreaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Listado de todas las áreas activas que no son zonas
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ListaSoloAreasActivas(int tuParametro)
        {
            var listX = FactorySic.GetEqAreaRepository().ListaSoloAreasActivas();
            if (tuParametro == 1)
            {
                listX = FactorySic.GetEqAreaRepository().ListaSoloAreasActivas(tuParametro);




                return listX;
            }
            else
            {
                return listX;
            }

        }

        #endregion

        #region Métodos Tabla EQ_AREANIVEL

        /// <summary>
        /// Inserta un registro de la tabla EQ_AREANIVEL
        /// </summary>
        public void SaveEqAreanivel(EqAreaNivelDTO entity)
        {
            try
            {
                FactorySic.GetEqAreanivelRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_AREANIVEL
        /// </summary>
        public void UpdateEqAreanivel(EqAreaNivelDTO entity)
        {
            try
            {
                FactorySic.GetEqAreanivelRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_AREANIVEL
        /// </summary>
        public void DeleteEqAreanivel(int anivelcodi)
        {
            try
            {
                FactorySic.GetEqAreanivelRepository().Delete(anivelcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_AREANIVEL
        /// </summary>
        public EqAreaNivelDTO GetByIdEqAreanivel(int anivelcodi)
        {
            return FactorySic.GetEqAreanivelRepository().GetById(anivelcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_AREANIVEL
        /// </summary>
        public List<EqAreaNivelDTO> ListEqAreanivels()
        {
            return FactorySic.GetEqAreanivelRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqAreanivel
        /// </summary>
        public List<EqAreaNivelDTO> GetByCriteriaEqAreanivels()
        {
            return FactorySic.GetEqAreanivelRepository().GetByCriteria();
        }
        #endregion

        #region Métodos Tabla EQ_AREAREL

        /// <summary>
        /// Inserta un registro de la tabla EQ_AREAREL
        /// </summary>
        public void SaveEqArearel(EqAreaRelDTO entity)
        {
            try
            {
                FactorySic.GetEqArearelRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_AREAREL
        /// </summary>
        public void UpdateEqArearel(EqAreaRelDTO entity)
        {
            try
            {
                FactorySic.GetEqArearelRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_AREAREL
        /// </summary>
        public void DeleteEqArearel(int arearlcodi, string username)
        {
            try
            {
                FactorySic.GetEqArearelRepository().Delete(arearlcodi);
                FactorySic.GetEqArearelRepository().Delete_UpdateAuditoria(arearlcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_AREAREL
        /// </summary>
        public EqAreaRelDTO GetByIdEqArearel(int arearlcodi)
        {
            return FactorySic.GetEqArearelRepository().GetById(arearlcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_AREAREL
        /// </summary>
        public List<EqAreaRelDTO> ListEqArearels()
        {
            return FactorySic.GetEqArearelRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqArearel
        /// </summary>
        public List<EqAreaRelDTO> GetByCriteriaEqArearels()
        {
            return FactorySic.GetEqArearelRepository().GetByCriteria();
        }

        //Zona
        /// <summary>
        /// Permite listar las areas perteneciente a una zona
        /// </summary>
        public List<EqAreaRelDTO> ListarAreasxAreapadre(int areacodi)
        {
            return FactorySic.GetEqArearelRepository().ListarAreasxAreapadre(areacodi);
        }

        //Zona
        /// <summary>
        /// Obtiene un área mediante el código de la zona a la que pertenece y su código de área.
        /// </summary>
        public EqAreaRelDTO GetxAreapadrexAreacodi(int areapadre, int areacodi)
        {
            return FactorySic.GetEqArearelRepository().GetxAreapadrexAreacodi(areapadre, areacodi);
        }

        #endregion

        #region Métodos Tabla EQ_FAMREL

        /// <summary>
        /// Inserta un registro de la tabla EQ_FAMREL
        /// </summary>
        public void SaveEqFamrel(EqFamrelDTO entity)
        {
            try
            {
                FactorySic.GetEqFamrelRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_FAMREL
        /// </summary>
        public void UpdateEqFamrel(EqFamrelDTO entity)
        {
            try
            {
                FactorySic.GetEqFamrelRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_FAMREL
        /// </summary>
        public void DeleteEqFamrel(int tiporelcodi, int famcodi1, int famcodi2, string username)
        {
            try
            {
                FactorySic.GetEqFamrelRepository().Delete(tiporelcodi, famcodi1, famcodi2);
                FactorySic.GetEqFamrelRepository().Delete_UpdateAuditoria(tiporelcodi, famcodi1, famcodi2, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_FAMREL
        /// </summary>
        public EqFamrelDTO GetByIdEqFamrel(int tiporelcodi, int famcodi1, int famcodi2)
        {
            return FactorySic.GetEqFamrelRepository().GetById(tiporelcodi, famcodi1, famcodi2);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_FAMREL
        /// </summary>
        public List<EqFamrelDTO> ListEqFamrels()
        {
            return FactorySic.GetEqFamrelRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqFamrel
        /// </summary>
        public List<EqFamrelDTO> GetByCriteriaEqFamrels()
        {
            return FactorySic.GetEqFamrelRepository().GetByCriteria();
        }
        /// <summary>
        /// Permite obtener el listado de registros de Familia Relación basado en el filtro de Tipo de relación y Estado
        /// </summary>
        /// <param name="iTipoRel">Código de tipo relación</param>
        /// <param name="strEstado">Código de estado</param>
        /// <returns></returns>
        public List<EqFamrelDTO> ListarFamRelPorTipoRelEstado(int iTipoRel, string strEstado)
        {
            return FactorySic.GetEqFamrelRepository().GetByTipoRel(iTipoRel, strEstado);
        }

        #endregion

        #region Métodos Tabla EQ_PROPEQUI

        /// <summary>
        /// Inserta un registro de la tabla EQ_PROPEQUI
        /// </summary>
        public void SaveEqPropequi(EqPropequiDTO entity)
        {
            entity.Fechapropequi = entity.Fechapropequi != null ? entity.Fechapropequi : DateTime.Now;
            entity.Propequifeccreacion = entity.Propequifeccreacion != null ? entity.Propequifeccreacion : DateTime.Now;
            entity.Valor = String.IsNullOrEmpty(entity.Valor) ? string.Empty : entity.Valor.Trim();
            entity.Propequiobservacion = String.IsNullOrEmpty(entity.Propequiobservacion) ? string.Empty : entity.Propequiobservacion.Trim();

            //verificar si existe historia para la misma fecha de vigencia
            List<EqPropequiDTO> listaHistXProp = this.ListarValoresHistoricosPropiedadPorEquipo(entity.Equicodi, entity.Propcodi.ToString()).Where(x => x.Fechapropequi == entity.Fechapropequi).ToList();
            EqPropequiDTO regActivoHist = listaHistXProp.Find(x => x.Propequideleted == 0);

            //si existe registro, crear una copia para guardar el historico
            if (regActivoHist != null)
            {
                bool existeDif = (entity.Valor != regActivoHist.Valor) || (entity.Propequiobservacion != regActivoHist.Propequiobservacion);

                if (existeDif)
                {
                    EqPropequiDTO regActivo = (EqPropequiDTO)regActivoHist.Clone();
                    regActivo.Valor = entity.Valor;
                    regActivo.Propequiobservacion = entity.Propequiobservacion;
                    regActivo.Propequideleted = 0;
                    regActivo.Propequideleted2 = 0;
                    regActivo.Propequifecmodificacion = entity.Propequifeccreacion;
                    regActivo.Propequiusumodificacion = entity.Propequiusucreacion;
                    UpdateEqPropequi(regActivo);

                    regActivoHist.Propequideleted = listaHistXProp.Max(x => x.Propequideleted) + 1;
                    SaveEqPropequiSinCorrelativo(regActivoHist);
                }
            }
            else
            {
                SaveEqPropequiSinCorrelativo(entity);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla EQ_PROPEQUI
        /// </summary>
        public void SaveEqPropequiSinCorrelativo(EqPropequiDTO entity)
        {
            try
            {
                FactorySic.GetEqPropequiRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_PROPEQUI
        /// </summary>
        public void UpdateEqPropequi(EqPropequiDTO entity)
        {
            try
            {
                entity.Fechapropequi = entity.Fechapropequi != null ? entity.Fechapropequi : DateTime.Now;
                //entity.Propequifecmodificacion = entity.Propequifecmodificacion != null ? entity.Propequifecmodificacion : DateTime.Now;

                FactorySic.GetEqPropequiRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_PROPEQUI
        /// </summary>
        public void DeleteEqPropequi(int propcodi, int equicodi, DateTime fechapropequi, string username)
        {
            try
            {
                FactorySic.GetEqPropequiRepository().Delete(propcodi, equicodi, fechapropequi);
                FactorySic.GetEqPropequiRepository().Delete_UpdateAuditoria(propcodi, equicodi, fechapropequi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_PROPEQUI
        /// </summary>
        public EqPropequiDTO GetByIdEqPropequi(int propcodi, int equicodi, DateTime fechapropequi)
        {
            try
            {
                return FactorySic.GetEqPropequiRepository().GetById(propcodi, equicodi, fechapropequi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_PROPEQUI
        /// </summary>
        public List<EqPropequiDTO> ListEqPropequis()
        {
            try
            {
                return FactorySic.GetEqPropequiRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqPropequi
        /// </summary>
        public List<EqPropequiDTO> GetByCriteriaEqPropequis()
        {
            try
            {
                return FactorySic.GetEqPropequiRepository().GetByCriteria();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<PropiedadEquipoHistDTO> ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int iEquipo)
        {
            try
            {
                var lista = new List<PropiedadEquipoHistDTO>();

                var oEquipo = FactorySic.GetEqEquipoRepository().GetById(iEquipo);
                List<EqPropequiDTO> listaPropequi = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(DateTime.Today, iEquipo.ToString(), oEquipo.Famcodi.Value.ToString(), "-1", "-1", string.Empty, ConstantesAppServicio.SI);

                foreach (var reg in listaPropequi)
                {
                    lista.Add(new PropiedadEquipoHistDTO()
                    {
                        PROPCODI = reg.Propcodi,
                        PROPFILE = !string.IsNullOrEmpty(reg.Propfile) ? reg.Propfile.Trim() : string.Empty,
                        PROPUNIDAD = !string.IsNullOrEmpty(reg.Propunidad) ? reg.Propunidad.Trim() : string.Empty,
                        VALOR = !string.IsNullOrEmpty(reg.Valor) ? reg.Valor.Trim() : string.Empty,
                        FechapropequiDesc = reg.Fechapropequi.Value.ToString(ConstantesAppServicio.FormatoFecha),
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        public List<EqPropequiDTO> ListarValoresVigentesPropiedades(DateTime fechaConsulta, int iEquipo, int famcodi, string sNombrePropiedad, string esFichaTecnica)
        {
            string filtro = esFichaTecnica;
            if (esFichaTecnica == "1") filtro = "-1";

            var lista = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(fechaConsulta, iEquipo.ToString(), famcodi.ToString(), "-1", "-1", sNombrePropiedad, filtro);

            if (esFichaTecnica == "1")
                lista = lista.Where(x => ListaPropiedadesAplicativos().Contains(x.Propcodi)).ToList();

            return lista;
        }

        public int TotalListarValoresVigentesPropiedadesPaginado(int iEquipo, string sNombrePropiedad)
        {
            try
            {
                var oEquipo = FactorySic.GetEqEquipoRepository().GetById(iEquipo);
                return FactorySic.GetEqPropequiRepository().TotalListarValoresVigentesPropiedadesPaginado(oEquipo.Famcodi.Value, sNombrePropiedad);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<EqPropequiDTO> ListarValoresHistoricosPropiedadPorEquipo(int iEquipo, string iPropiedad)
        {
            var listaPropHist = FactorySic.GetEqPropequiRepository().ListarValoresHistoricosPropiedadPorEquipo(iEquipo, iPropiedad)
                .OrderByDescending(x => x.Fechapropequi).ThenByDescending(x => x.Propequifecmodificacion).ThenByDescending(x => x.Propequifeccreacion).ToList();

            foreach (var reg in listaPropHist)
                FormatearPropequi(reg);

            return listaPropHist;
        }

        /// <summary>
        /// Permite obtener el valor de una propiedad
        /// </summary>
        /// <param name="idPropiedad"></param>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public string GetValorPropiedad(int idPropiedad, int idEquipo)
        {
            return FactorySic.GetEqPropequiRepository().GetValorPropiedad(idPropiedad, idEquipo);
        }

        /// <summary>
        /// Método que copia los últimos valores vigentes de un equipo a otro.
        /// </summary>
        /// <param name="iEquipoOriginal">Código Equipo original</param>
        /// <param name="iEquipoDestino">Código Equipo destino</param>
        /// <param name="usuario">Nombre de usuario</param>
        public void CopiarPropiedades(int iEquipoOriginal, int iEquipoDestino, string usuario)
        {
            try
            {
                Factory.FactorySic.GetEqPropequiRepository().CopiarValoresPropiedadEquipo(iEquipoOriginal, iEquipoDestino, usuario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla EQ_PROPIEDAD

        /// <summary>
        /// Inserta un registro de la tabla EQ_PROPIEDAD
        /// </summary>
        public int SaveEqPropiedad(EqPropiedadDTO entity)
        {
            int codigoPropiedad = -1;
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //guadar Propiedad
                        codigoPropiedad = FactorySic.GetEqPropiedadRepository().Save(entity, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return codigoPropiedad;
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_PROPIEDAD
        /// </summary>
        public void UpdateEqPropiedad(EqPropiedadDTO entity)
        {
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //actualizar Propiedad
                        FactorySic.GetEqPropiedadRepository().Update(entity, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_PROPIEDAD
        /// </summary>
        public void DeleteEqPropiedad(int propcodi, string username)
        {
            try
            {
                FactorySic.GetEqPropiedadRepository().Delete(propcodi);
                FactorySic.GetEqPropiedadRepository().Delete_UpdateAuditoria(propcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_PROPIEDAD
        /// </summary>
        public EqPropiedadDTO GetByIdEqPropiedad(int propcodi)
        {
            var reg = FactorySic.GetEqPropiedadRepository().GetById(propcodi);
            if (reg != null) FormatearPropiedades(reg, null);

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_PROPIEDAD
        /// </summary>
        public List<EqPropiedadDTO> ListEqPropiedads()
        {
            return FactorySic.GetEqPropiedadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas por familia (tipo equipo), nombre y ficha técnica
        /// </summary>
        public List<EqPropiedadDTO> GetByCriteriaEqPropiedades(int famcodi, string fichaTecnica, string nombre, int estado)
        {
            var lista = FactorySic.GetEqPropiedadRepository().GetByCriteria(famcodi, nombre, estado);

            //Obtiene lista de todos los Item de las ficha maestra
            List<FtFictecItemDTO> listaFicTemItems = new List<FtFictecItemDTO>();
            if (fichaTecnica == "S")
                listaFicTemItems = ObtenerItemFichaXtipoEquipoOficial(famcodi);

            foreach (var item in lista)
                this.FormatearPropiedades(item, listaFicTemItems);

            //Orden de acuerdo al filtro de ficha técnica
            lista = fichaTecnica == "S" ? lista.Where(x => x.Propfichaoficial == fichaTecnica).OrderBy(x => x.Orden).ToList() : lista.OrderBy(x => x.Propcodi).ToList();

            return lista;
        }

        
        /// <summary>
        /// Dar Formato de propiedades 
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearPropiedades(EqPropiedadDTO reg, List<FtFictecItemDTO> listaFicTemItems)
        {
            if (reg != null)
            {
                reg.Propnomb = (reg.Propnomb ?? "").Trim();
                reg.Propabrev = (reg.Propabrev ?? "").Trim();
                reg.Propdefinicion = (reg.Propdefinicion ?? "").Trim();
                reg.Propunidad = (reg.Propunidad ?? "").Trim();
                reg.NombreFamilia = (reg.NombreFamilia ?? "").Trim();
                reg.Propnombficha = (reg.Propnombficha ?? "").Trim();
                reg.Proptipo = (reg.Proptipo ?? "").Trim();

                reg.Propfichaoficial = (reg.Propfichaoficial ?? "").Trim();
                reg.PropfichaoficialDesc = reg.Propfichaoficial.ToUpper() == ConstantesEquipamientoAppServicio.Si ? "SI" :
                    reg.Propfichaoficial.ToUpper() == ConstantesEquipamientoAppServicio.No ? "NO" : reg.Propfichaoficial.ToUpper();

                reg.PropfeccreacionDesc = reg.Propfeccreacion != null ? reg.Propfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.PropfecmodificacionDesc = reg.Propfecmodificacion != null ? reg.Propfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;

                reg.UltimaModificacionFechaDesc = reg.Propfecmodificacion != null ? reg.PropfecmodificacionDesc : reg.PropfeccreacionDesc;
                reg.UltimaModificacionUsuarioDesc = reg.Propusumodificacion != null ? reg.Propusumodificacion : reg.Propusucreacion;

                reg.EstiloEstado = reg.Propactivo == 0 ? ConstantesEquipamientoAppServicio.EstiloBaja : "";

                if (listaFicTemItems != null && listaFicTemItems.Any())
                {
                    var fictecitem = listaFicTemItems.Find(x => x.Propcodi == reg.Propcodi);
                    reg.Orden = fictecitem != null ? fictecitem.OrdenNumerico : 999999999;
                }
            }
        }        


        /// <summary>
        /// Generar Excel de reporte plantilla
        /// </summary>
        /// <param name="listaPropiedades"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void GenerarExcelPropiedades(List<EqPropiedadDTO> listaPropiedades, string path, string fileName)
        {
            try
            {
                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("No existe el archivo: " + file + ".");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];
                    //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.First();

                    int row = 11;
                    int numeroItem = 1;

                    int columnNroItem = 1;
                    int columnCodPropiedad = columnNroItem + 1;
                    int columnNomb = columnCodPropiedad + 1;
                    int columnNombFicha = columnNomb + 1;
                    int columnAbrev = columnNombFicha + 1;
                    int columnDefinicion = columnAbrev + 1;
                    int columnUnidad = columnDefinicion + 1;
                    int columnTipoDato = columnUnidad + 1;
                    int columnLong1 = columnTipoDato + 1;
                    int columnLong2 = columnLong1 + 1;
                    int columnFichaTec = columnLong2 + 1;
                    int columnCodTipoEquipo = columnFichaTec + 1;
                    int columnLimInf = columnCodTipoEquipo + 1;
                    int columnLimSup = columnLimInf + 1;
                    int columnNombTipoEquipo = columnLimSup + 1;
                    int columnFechModif = columnNombTipoEquipo + 1;
                    int columnUsuModif = columnFechModif + 1;

                    foreach (var item in listaPropiedades)
                    {
                        if (item.Propcodi == 2223)
                        {
                            var asd = 0;
                        }
                        ws.Cells[row, columnNroItem].Value = numeroItem;
                        ws.Cells[row, columnCodPropiedad].Value = item.Propcodi;
                        ws.Cells[row, columnNomb].Value = item.Propnomb;
                        ws.Cells[row, columnNombFicha].Value = item.Propnombficha;
                        ws.Cells[row, columnAbrev].Value = item.Propabrev;
                        ws.Cells[row, columnDefinicion].Value = item.Propdefinicion;
                        ws.Cells[row, columnUnidad].Value = item.Propunidad;
                        ws.Cells[row, columnTipoDato].Value = item.Proptipo;
                        ws.Cells[row, columnLong1].Value = item.Proptipolong1;
                        ws.Cells[row, columnLong2].Value = item.Proptipolong2;
                        ws.Cells[row, columnFichaTec].Value = item.PropfichaoficialDesc;
                        ws.Cells[row, columnCodTipoEquipo].Value = item.Famcodi;
                        ws.Cells[row, columnLimInf].Value = item.Propliminf;
                        ws.Cells[row, columnLimSup].Value = item.Proplimsup;
                        ws.Cells[row, columnNombTipoEquipo].Value = item.NombreFamilia;
                        ws.Cells[row, columnFechModif].Value = item.UltimaModificacionFechaDesc;
                        ws.Cells[row, columnUsuModif].Value = item.UltimaModificacionUsuarioDesc;

                        UtilExcel.BorderCeldasLineaDelgada(ws, row, columnNroItem, row, columnUsuModif, "#000000", true);

                        row++;
                        numeroItem++;
                    }

                    //agregar filas adicionales para que el usuario registre nueva información
                    for (var i = 0; i < 10; i++)
                    {
                        ws.Cells[row, columnNroItem].Value = numeroItem;
                        UtilExcel.BorderCeldasLineaDelgada(ws, row, columnNroItem, row, columnUsuModif, "#000000", true);

                        row++;
                        numeroItem++;
                    }

                    xlPackage.Save();

                    //HOJA TIPO DE EQUIPO
                    ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaFamilia];
                    int fil = 2;
                    int columnIniData = 1;
                    var listaFamilias = FactorySic.GetEqFamiliaRepository().ListarFamiliasFT();

                    foreach (var item in listaFamilias)
                    {
                        ws2.Cells[fil, columnIniData++].Value = item.Famcodi;
                        ws2.Cells[fil, columnIniData++].Value = item.Tareaabrev;
                        ws2.Cells[fil, columnIniData++].Value = item.Famnomb;
                        ws2.Cells[fil, columnIniData++].Value = item.Famabrev;
                        fil++;
                        columnIniData = 1;
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }        

        /// <summary>
        /// Metodo para la validacion de los datos a importar
        /// </summary>
        public void ValidarPropiedadesAImportarXLSX(string path, string fileName, string strUsuario,
                                               out List<EqPropiedadDTO> lstRegPropiedadesCorrectos,
                                               out List<EqPropiedadDTO> lstRegPropiedadesErroneos,
                                               out List<EqPropiedadDTO> listaNuevo,
                                               out List<EqPropiedadDTO> listaModificado)
        {
            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsm", ".xlsx" }; // corregir
            if (!lExtensionPermitido.Contains(extension))
            {
                throw new ArgumentException("Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido));
            }

            DateTime fechaRegistro = DateTime.Now;

            // Obtener lista de propiedades actuales
            List<EqPropiedadDTO> propiedadesActuales = GetByCriteriaEqPropiedades(-2, "T", string.Empty, -1);

            //Listar Familias de la bd COES
            List<EqFamiliaDTO> listaFamilias = FactorySic.GetEqFamiliaRepository().List();
            listaFamilias.Add(new EqFamiliaDTO { Famcodi = 0, Famnomb = "TODOS" });

            #region Archivo xlsx

            string savePath = path + fileName;
            List<FilaExcelPropiedad> listaFilaMacro = ImportToDataTable(savePath);

            //Validación de registros macro
            lstRegPropiedadesCorrectos = new List<EqPropiedadDTO>();
            lstRegPropiedadesErroneos = new List<EqPropiedadDTO>();

            foreach (var regFila in listaFilaMacro)
            {
                if (regFila.NumItem == 185 || regFila.NumItem == 2062 || regFila.NumItem == 2063 || regFila.NumItem == 2064 || regFila.NumItem == 2065)
                {
                    var sdf = 0;
                }

                //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                string mensajeValidacion = this.ValidarLecturaExcel(regFila, listaFamilias);

                EqPropiedadDTO entity = new EqPropiedadDTO();
                entity.NroItem = regFila.NumItem;

                entity.Propcodi = regFila.Propcodi; // Código de propiedad 
                entity.Propnomb = regFila.Propnomb; // Nombre
                entity.Propnombficha = regFila.Propnombficha;

                entity.Propabrev = regFila.Propabrev;
                entity.Propdefinicion = regFila.Propdefinicion;
                entity.Propunidad = regFila.Propunidad;
                entity.Proptipo = regFila.Proptipo.Trim().ToUpper();

                //Capturar valor correcto según tipo de dato
                if (entity.Proptipo == "DECIMAL")
                {
                    entity.Proptipolong1 = regFila.Proptipolong1;
                    entity.Proptipolong2 = regFila.Proptipolong2;
                }
                else
                {
                    if (entity.Proptipo == "ENTERO" || entity.Proptipo == "CARACTER")
                    {
                        entity.Proptipolong1 = regFila.Proptipolong1;
                        entity.Proptipolong2 = null;
                    }
                    else
                    {
                        entity.Proptipolong1 = null;
                        entity.Proptipolong2 = null;
                    }
                }
                //limites
                entity.Propliminf = regFila.Propliminf;
                entity.Proplimsup = regFila.Proplimsup;
                entity.StrPropliminf = regFila.StrPropliminf;
                entity.StrProplimsup = regFila.StrProplimsup;
                entity.StrProptipolong1 = regFila.StrProptipolong1;
                entity.StrProptipolong2 = regFila.StrProptipolong2;

                entity.Propfichaoficial = regFila.Propfichaoficial == "SI" ? ConstantesEquipamientoAppServicio.Si : ConstantesEquipamientoAppServicio.No;
                entity.Famcodi = regFila.Famcodi;
                entity.NombreFamilia = regFila.NombreFamilia;

                //nuevos registros
                entity.Propactivo = 1;
                entity.Propusucreacion = strUsuario; // Usuario de creacion del registro
                entity.Propfeccreacion = fechaRegistro; // Fecha de creacion del registro

                // Si los datos son correctos VALIDO LA INFORMACION DE ACUERDO AL NEGOCIO
                if (mensajeValidacion == "")
                {
                    //Validar duplicados dentro de la plantilla
                    var propRepetido = ObtenerRegistroPorCriterio(entity, lstRegPropiedadesCorrectos);
                    if (propRepetido != null)
                    {
                        entity.Observaciones = "No se puede crear Propiedades duplicadas. Comparar con item N°" + propRepetido.NroItem;
                        lstRegPropiedadesErroneos.Add(entity);
                    }
                    else
                    {
                        //Validar duplicado en (BD)
                        var dtoPropiedadRee = ObtenerRegistroPorCriterio(entity, propiedadesActuales);
                        bool existeRegistroEnBD = dtoPropiedadRee != null;

                        if (existeRegistroEnBD && entity.Propcodi == dtoPropiedadRee.Propcodi)
                        {
                            var existeActualizacionDeBD = ExisteModificacionPropiedad(entity, dtoPropiedadRee);

                            if (existeActualizacionDeBD && dtoPropiedadRee.Propactivo == 1) // si hay cambios y es activo 
                            {
                                //capturar valores
                                dtoPropiedadRee.NroItem = entity.NroItem;
                                dtoPropiedadRee.Propnomb = entity.Propnomb ?? "";
                                dtoPropiedadRee.Propnombficha = entity.Propnombficha ?? "";
                                dtoPropiedadRee.Propabrev = entity.Propabrev ?? "";
                                dtoPropiedadRee.Propdefinicion = entity.Propdefinicion ?? "";
                                dtoPropiedadRee.Proptipo = entity.Proptipo ?? "";
                                dtoPropiedadRee.Propunidad = entity.Propunidad ?? "";
                                dtoPropiedadRee.Proptipolong1 = entity.Proptipolong1;
                                dtoPropiedadRee.Proptipolong2 = entity.Proptipolong2;
                                dtoPropiedadRee.Propliminf = entity.Propliminf;
                                dtoPropiedadRee.Proplimsup = entity.Proplimsup;
                                dtoPropiedadRee.Famcodi = entity.Famcodi;
                                dtoPropiedadRee.Propfichaoficial = entity.Propfichaoficial ?? "";
                                dtoPropiedadRee.Propfecmodificacion = fechaRegistro;
                                dtoPropiedadRee.Propusumodificacion = strUsuario;

                                entity = dtoPropiedadRee;//entidad para actualización
                                entity.ExisteCambio = existeActualizacionDeBD;// indicamos que tiene edición
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (existeRegistroEnBD && entity.Propcodi == -1)// si existe duplicado y es nuevo
                        {
                            entity.Observaciones = "Se encontró coincidencia con registro existente. No se puede crear duplicados";
                            lstRegPropiedadesErroneos.Add(entity);
                        }
                        else
                        {
                            lstRegPropiedadesCorrectos.Add(entity); // Es nuevo
                        }
                    }
                }
                else
                {
                    // Van los registros con formato incorrecto
                    entity.ChkMensaje = true; // para separar con formato incorrecto
                    entity.Observaciones = mensajeValidacion;

                    lstRegPropiedadesErroneos.Add(entity);
                }

                //Validaciones de campos limites inferior y superior
                string mensajeValidacionLongitudYLimites = this.ValidarCamposLongitudYLimites(regFila);

                if (mensajeValidacionLongitudYLimites != "")
                {
                    // Van los registros con formato incorrecto
                    entity.ChkMensaje = true; // para separar con formato incorrecto
                    entity.Observaciones = mensajeValidacionLongitudYLimites;

                    lstRegPropiedadesErroneos.Add(entity);
                }
            }

            #endregion

            listaNuevo = lstRegPropiedadesCorrectos.Where(x => x.Propcodi == -1).ToList(); // solo los nuevos
            listaModificado = lstRegPropiedadesCorrectos.Where(x => x.Propcodi > -1 && x.ExisteCambio).ToList(); // solo los que tienen cambios
        }

        /// <summary>
        /// Valida los campos longitud y limite
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <returns></returns>
        public string ValidarCamposLongitudYLimites(FilaExcelPropiedad filaExcel)
        {
            string salida = "";
            List<string> lMsgValidacion = new List<string>();
            string tipoDato = filaExcel.Proptipo;

            if (filaExcel.NumItem == 185 || filaExcel.NumItem == 2062 || filaExcel.NumItem == 2063 || filaExcel.NumItem == 2064 || filaExcel.NumItem == 2065)
            {
                var sdf = 0;
            }

            #region Validamos longitud 1 y 2
            string datoLong1 = filaExcel.StrProptipolong1;
            string datoLong2 = filaExcel.StrProptipolong2;
            if (tipoDato == "DECIMAL")
            {
                //Longitud 1                
                ValidarCampoLongitud(lMsgValidacion, datoLong1, "Longitud 1", 1, 15);

                //Longitud 2
                ValidarCampoLongitud(lMsgValidacion, datoLong2, "Longitud 2", 0, 10);
            }
            else
            {
                if (tipoDato == "ENTERO")
                {
                    //Longitud 1
                    ValidarCampoLongitud(lMsgValidacion, datoLong1, "Longitud 1", 1, 15);
                }
                else
                {
                    if (tipoDato == "CARACTER")
                    {
                        //Longitud 1
                        ValidarCampoLongitud(lMsgValidacion, datoLong1, "Longitud 1", 0, 9999);
                    }

                }
            }

            #endregion

            #region Validar campo Limites
            string datoLimiteInf = filaExcel.StrPropliminf;
            string datoLimiteSup = filaExcel.StrProplimsup;

            //Validacion para Limite Inferior
            ValidarCampoLimite(filaExcel, lMsgValidacion, tipoDato, datoLimiteInf, "Límite inferior");

            //Validacion para Limite Superior
            ValidarCampoLimite(filaExcel, lMsgValidacion, tipoDato, datoLimiteSup, "Límite superior");

            //Validacion para Limite superior e inferior            
            if (datoLimiteInf != null && datoLimiteInf != "")
            {
                if (datoLimiteSup != null && datoLimiteSup != "")
                {
                    if (EsDecimal(datoLimiteInf) && EsDecimal(datoLimiteSup))
                    {
                        decimal valorI = Decimal.Parse(datoLimiteInf);
                        decimal valorS = Decimal.Parse(datoLimiteSup);
                        if (valorI > valorS)
                        {
                            lMsgValidacion.Add("Límite superior debe ser mayor o igual al límite inferior");
                        }
                    }
                }
            }

            #endregion

            return string.Join(". ", lMsgValidacion);

        }

        /// <summary>
        /// Valida el campo limite del excel importado
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <param name="lMsgValidacion"></param>
        /// <param name="tipoDato"></param>
        /// <param name="datoLimite"></param>
        /// <param name="nombreLimite"></param>
        private void ValidarCampoLimite(FilaExcelPropiedad filaExcel, List<string> lMsgValidacion, string tipoDato, string datoLimite, string nombreLimite)
        {
            if (datoLimite != null && datoLimite != "")
            {
                if (EsDecimal(datoLimite))
                {
                    if (tipoDato == "DECIMAL")
                    {
                        bool hayParteEntera = false;
                        bool hayParteDecimal = false;
                        string parteEntera = "";
                        string parteDecimal = "";
                        string valorDato = datoLimite;

                        //verifico si la parte entera tiene data correcta
                        if (filaExcel.StrProptipolong1 != null && filaExcel.StrProptipolong1 != "")
                        {
                            if (EsEntero(filaExcel.StrProptipolong1))
                            {
                                parteEntera = filaExcel.StrProptipolong1;
                                hayParteEntera = true;
                            }
                        }
                        else
                        {
                            parteEntera = "15";//Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                            hayParteEntera = true;
                        }

                        //verifico si la parte decimal tiene data correcta
                        if (filaExcel.StrProptipolong2 != null && filaExcel.StrProptipolong2 != "")
                        {
                            if (EsEntero(filaExcel.StrProptipolong2))
                            {
                                parteDecimal = filaExcel.StrProptipolong2;
                                hayParteDecimal = true;
                            }
                        }
                        else
                        {
                            parteDecimal = "10";//Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                            hayParteDecimal = true;
                        }

                        //continuo solo si hay dato correcto para parte entera y decimal
                        if (hayParteEntera && hayParteDecimal)
                        {
                            if (EsEntero(parteEntera) && EsEntero(parteDecimal))
                            {
                                if (valorDato == null || valorDato == "")
                                {
                                }
                                else
                                {
                                    if (!EsDecimal(valorDato))
                                    {
                                        lMsgValidacion.Add(nombreLimite + ": Debe ser un campo numérico.");
                                        //validacion = validacion + "<li>Límite inferior: debe ser un campo numérico.</li>";//Campo Requerido
                                        //flag = false;
                                    }
                                    else
                                    {
                                        string strValDatoPositivo = valorDato.Replace("-", "");
                                        string strValDatoPositivoSinPunto = strValDatoPositivo.Replace(".", "");
                                        decimal valorDatoNumericoP = Decimal.Parse(strValDatoPositivo);
                                        int numCifrasTotales = strValDatoPositivoSinPunto.Length;
                                        decimal valParteEntera = Math.Truncate(valorDatoNumericoP);
                                        string strValParteEntera = valParteEntera + "";

                                        int numCifrasEntero = strValParteEntera.Length;
                                        int numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                        if (numCifrasEntero > Int32.Parse(parteEntera))
                                        {
                                            lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras enteras es " + parteEntera);
                                        }

                                        if (numCifrasDecimal > Int32.Parse(parteDecimal))
                                        {
                                            lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras decimales es " + parteDecimal);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (tipoDato == "ENTERO")
                        {
                            bool hayParteEntera = false;
                            string parteEntera = "";
                            string valorDato = datoLimite;

                            //verifico si la parte entera tiene data correcta
                            if (filaExcel.StrProptipolong1 != null && filaExcel.StrProptipolong1 != "")
                            {
                                if (EsEntero(filaExcel.StrProptipolong1))
                                {
                                    parteEntera = filaExcel.StrProptipolong1;
                                    hayParteEntera = true;
                                }
                            }
                            else
                            {
                                parteEntera = "15";//Dado que parteEntera y parteDecimal pueden ser nulos, en esos casos toman el maximo valor posible
                                hayParteEntera = true;
                            }

                            if (hayParteEntera)
                            {
                                if (EsEntero(parteEntera))
                                {
                                    if (valorDato == null || valorDato == "")
                                    {

                                    }
                                    else
                                    {
                                        if (!EsDecimal(valorDato))
                                        {
                                            lMsgValidacion.Add(nombreLimite + ": debe ser un campo numérico.");
                                        }
                                        else
                                        {
                                            string strValDatoPositivo = valorDato.Replace("-", "");
                                            string strValDatoPositivoSinPunto = strValDatoPositivo.Replace(".", "");
                                            decimal valorDatoNumericoP = Decimal.Parse(strValDatoPositivo);
                                            int numCifrasTotales = strValDatoPositivoSinPunto.Length;
                                            decimal valParteEntera = Math.Truncate(valorDatoNumericoP);
                                            string strValParteEntera = valParteEntera + "";

                                            int numCifrasEntero = strValParteEntera.Length;

                                            if (numCifrasTotales != numCifrasEntero)
                                            {
                                                lMsgValidacion.Add(nombreLimite + ": Debe ingresar una cifra entera correcta.");
                                            }

                                            if (numCifrasEntero > Int32.Parse(parteEntera))
                                            {
                                                lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras enteras es " + parteEntera);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (tipoDato == "FORMULA")
                            {
                                //Para este caso toman el maximo valor posible
                                string parteEntera = "15";
                                string parteDecimal = "10";
                                string valorDato = datoLimite;


                                if ((parteEntera != "") && (parteDecimal != ""))
                                {
                                    if (EsEntero(parteEntera) && EsEntero(parteDecimal))
                                    {
                                        if (valorDato == null || valorDato == "")
                                        {
                                        }
                                        else
                                        {
                                            if (!EsDecimal(valorDato))
                                            {
                                                lMsgValidacion.Add(nombreLimite + ": Debe ser un campo numérico");
                                            }
                                            else
                                            {
                                                string strValDatoPositivo = valorDato.Replace("-", "");
                                                string strValDatoPositivoSinPunto = strValDatoPositivo.Replace(".", "");
                                                decimal valorDatoNumericoP = Decimal.Parse(strValDatoPositivo);
                                                int numCifrasTotales = strValDatoPositivoSinPunto.Length;
                                                decimal valParteEntera = Math.Truncate(valorDatoNumericoP);
                                                string strValParteEntera = valParteEntera + "";

                                                int numCifrasEntero = strValParteEntera.Length;
                                                int numCifrasDecimal = numCifrasTotales - numCifrasEntero;

                                                if (numCifrasEntero > Int32.Parse(parteEntera))
                                                {
                                                    lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras enteras es " + parteEntera);
                                                }

                                                if (numCifrasDecimal > Int32.Parse(parteDecimal))
                                                {
                                                    lMsgValidacion.Add(nombreLimite + ": La máxima cantidad de cifras decimales es " + parteDecimal);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    lMsgValidacion.Add(nombreLimite + ": Debe ser un campo numérico");
                }

            }
        }

        /// <summary>
        /// Validacion de los campos de Longitud
        /// </summary>
        /// <param name="lMsgValidacion"></param>
        /// <param name="datoLong"></param>
        /// <param name="nomLongitud"></param>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        public void ValidarCampoLongitud(List<string> lMsgValidacion, string datoLong, string nomLongitud, int rangoIni, int rangoFin)
        {
            if (datoLong == null || datoLong == "")
            {
            }
            else
            {
                if (!EsEntero(datoLong))
                {
                    lMsgValidacion.Add(nomLongitud + ": Debe ser un campo entero entre [" + rangoIni + "-" + rangoFin + "]");
                }
                else
                {
                    Int32.TryParse(datoLong, out int datoLongNumerico);
                    if (datoLongNumerico < rangoIni || datoLongNumerico > rangoFin)
                    {
                        lMsgValidacion.Add(nomLongitud + ": El rango permitido es entre [" + rangoIni + "-" + rangoFin + "]");
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve objeto duplicado de una lista de propiedades
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listaPropiedades"></param>
        /// <returns></returns>
        public EqPropiedadDTO ObtenerRegistroPorCriterio(EqPropiedadDTO entity, List<EqPropiedadDTO> listaPropiedades)
        {
            EqPropiedadDTO dtoPropiedad = listaPropiedades.Where(x => (x.Propnomb.Trim().ToUpper() == entity.Propnomb.Trim().ToUpper()
                               && x.Famcodi == entity.Famcodi)
                               || (x.Propcodi > 0 && x.Propcodi == entity.Propcodi)).FirstOrDefault();

            return dtoPropiedad;
        }

        /// <summary>
        /// ExisteModificacionPropiedad
        /// </summary>
        /// <param name="regExcel"></param>
        /// <param name="regBD"></param>
        /// <returns></returns>
        private bool ExisteModificacionPropiedad(EqPropiedadDTO regExcel, EqPropiedadDTO regBD)
        {
            if (regExcel.NroItem == 185 || regExcel.NroItem == 2063 || regExcel.NroItem == 2064 || regExcel.NroItem == 2065)
            {
                var sdf = 0;
            }
            if (regExcel.Propnomb.Trim().ToUpper() != regBD.Propnomb.Trim().ToUpper()) return true;
            if (regExcel.Propnombficha.Trim().ToUpper() != regBD.Propnombficha.Trim().ToUpper()) return true;
            if (regExcel.Propabrev.Trim().ToUpper() != regBD.Propabrev.Trim().ToUpper()) return true;
            if (regExcel.Proptipo != regBD.Proptipo) return true;
            if (regExcel.Propfichaoficial.Trim().ToUpper() != regBD.Propfichaoficial.Trim().ToUpper()) return true;
            if (regExcel.Famcodi != regBD.Famcodi) return true;

            //valido longitudes
            if (regExcel.Proptipo == "DECIMAL")
            {
                if (regExcel.Proptipolong1 != regBD.Proptipolong1) return true;
                if (regExcel.Proptipolong2 != regBD.Proptipolong2) return true;

                //valido en caso el dato de long1 es un valor no entero
                if (regExcel.StrProptipolong1.Trim() != "")
                {
                    if (regBD.Proptipolong1 != null)
                    {
                        if (regExcel.StrProptipolong1.Trim() != regBD.Proptipolong1.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.Proptipolong1 != null)
                    {
                        return true;
                    }

                }

                //valido en caso el dato de long2 es un valor no entero
                if (regExcel.StrProptipolong2.Trim() != "")
                {
                    if (regBD.Proptipolong2 != null)
                    {
                        if (regExcel.StrProptipolong2.Trim() != regBD.Proptipolong2.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.Proptipolong2 != null)
                    {
                        return true;
                    }

                }
            }
            else
            {
                if (regExcel.Proptipo == "ENTERO")
                {
                    if (regExcel.Proptipolong1 != regBD.Proptipolong1) return true;

                    //valido en caso el dato de long1 es un valor no entero
                    if (regExcel.StrProptipolong1.Trim() != "")
                    {
                        if (regBD.Proptipolong1 != null)
                        {
                            if (regExcel.StrProptipolong1.Trim() != regBD.Proptipolong1.ToString().Trim()) return true;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        if (regBD.Proptipolong1 != null)
                        {
                            return true;
                        }

                    }
                }
            }

            //valido limites
            if (regExcel.Proptipo == "DECIMAL" || regExcel.Proptipo == "ENTERO" || regExcel.Proptipo == "FORMULA")
            {
                if (regExcel.Propliminf != regBD.Propliminf) return true;
                if (regExcel.Proplimsup != regBD.Proplimsup) return true;

                //valido en caso el dato de minInf es un valor no numerico
                if (regExcel.StrPropliminf.Trim() != "")
                {
                    if (regBD.Propliminf != null)
                    {
                        if (regExcel.StrPropliminf.Trim() != regBD.Propliminf.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.StrPropliminf != null)
                    {
                        return true;
                    }

                }

                //valido en caso el dato de minSup es un valor no numerico
                if (regExcel.StrProplimsup.Trim() != "")
                {
                    if (regBD.Proplimsup != null)
                    {
                        if (regExcel.StrProplimsup.Trim() != regBD.Proplimsup.ToString().Trim()) return true;
                    }
                    else
                        return true;
                }
                else
                {
                    if (regBD.StrProplimsup != null)
                    {
                        return true;
                    }

                }
            }


            return false;
        }

        /// <summary>
        /// Importa registros de un DataTable
        /// </summary>
        /// <param name="filePath">Directorio de archivos</param>  
        /// <returns>devuelve una cadena</returns>
        public static List<FilaExcelPropiedad> ImportToDataTable(string filePath)
        {
            List<FilaExcelPropiedad> listaMacro = new List<FilaExcelPropiedad>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexItem = 1;
            int indexCodPropiedad = indexItem + 1;
            int indexNomb = indexCodPropiedad + 1;
            int indexNombFicha = indexNomb + 1;
            int indexAbrev = indexNombFicha + 1;
            int indexDefinicion = indexAbrev + 1;
            int indexUnidad = indexDefinicion + 1;
            int indexTipoDato = indexUnidad + 1;
            int indexLong1 = indexTipoDato + 1;  //9
            int indexLong2 = indexLong1 + 1;  //10
            int indexFichaTec = indexLong2 + 1;
            int indexCodTipoEquipo = indexFichaTec + 1;
            int indexLimI = indexCodTipoEquipo + 1; // 13
            int indexLimS = indexLimI + 1; //14
            int indexNombTipoEquipo = indexLimS + 1;
            int indexFechModif = indexNombTipoEquipo + 1;
            int indexUsuModif = indexFechModif + 1;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 11;

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    if (row == 195 || row == 2073 || row == 2074 || row == 2075)
                    {
                        var sdf = 0;
                    }
                    var sItem = string.Empty;
                    if (worksheet.Cells[row, indexItem].Value != null) sItem = worksheet.Cells[row, indexItem].Value.ToString();
                    Int32.TryParse(sItem, out int numItem);

                    var sCodPropiedad = string.Empty;
                    if (worksheet.Cells[row, indexCodPropiedad].Value != null) sCodPropiedad = worksheet.Cells[row, indexCodPropiedad].Value.ToString();

                    var sNomb = string.Empty;
                    if (worksheet.Cells[row, indexNomb].Value != null) sNomb = worksheet.Cells[row, indexNomb].Value.ToString();

                    var sNombFicha = string.Empty;
                    if (worksheet.Cells[row, indexNombFicha].Value != null) sNombFicha = worksheet.Cells[row, indexNombFicha].Value.ToString();

                    var sAbrev = string.Empty;
                    if (worksheet.Cells[row, indexAbrev].Value != null) sAbrev = worksheet.Cells[row, indexAbrev].Value.ToString();

                    var sDefinicion = string.Empty;
                    if (worksheet.Cells[row, indexDefinicion].Value != null) sDefinicion = worksheet.Cells[row, indexDefinicion].Value.ToString();

                    var sUnidad = string.Empty;
                    if (worksheet.Cells[row, indexUnidad].Value != null) sUnidad = worksheet.Cells[row, indexUnidad].Value.ToString();

                    var sTipoDato = string.Empty;
                    if (worksheet.Cells[row, indexTipoDato].Value != null) sTipoDato = worksheet.Cells[row, indexTipoDato].Value.ToString();

                    var sLong1 = string.Empty;
                    if (worksheet.Cells[row, indexLong1].Value != null) sLong1 = worksheet.Cells[row, indexLong1].Value.ToString();

                    var sLong2 = string.Empty;
                    if (worksheet.Cells[row, indexLong2].Value != null) sLong2 = worksheet.Cells[row, indexLong2].Value.ToString();

                    var sLimI = string.Empty;
                    if (worksheet.Cells[row, indexLimI].Value != null) sLimI = worksheet.Cells[row, indexLimI].Value.ToString();

                    var sLimS = string.Empty;
                    if (worksheet.Cells[row, indexLimS].Value != null) sLimS = worksheet.Cells[row, indexLimS].Value.ToString();
                    
                    var sFichaTec = string.Empty;
                    if (worksheet.Cells[row, indexFichaTec].Value != null) sFichaTec = worksheet.Cells[row, indexFichaTec].Value.ToString();

                    var sCodTipoEquipo = string.Empty;
                    if (worksheet.Cells[row, indexCodTipoEquipo].Value != null) sCodTipoEquipo = worksheet.Cells[row, indexCodTipoEquipo].Value.ToString();

                    var sNombTipoEquipo = string.Empty;
                    if (worksheet.Cells[row, indexNombTipoEquipo].Value != null) sNombTipoEquipo = worksheet.Cells[row, indexNombTipoEquipo].Value.ToString();

                    var sFechModif = string.Empty;
                    if (worksheet.Cells[row, indexFechModif].Value != null) sFechModif = worksheet.Cells[row, indexFechModif].Value.ToString();

                    var sUsuModif = string.Empty;
                    if (worksheet.Cells[row, indexUsuModif].Value != null) sUsuModif = worksheet.Cells[row, indexUsuModif].Value.ToString();

                    int propcodi = 0;
                    int famcodi = 0;
                    int? longitud1 = null;
                    int? longitud2 = null;
                    decimal? limI = null;
                    decimal? limS = null;
                    //DateTime fechaModificacion = DateTime.MinValue;
                    try
                    {
                        sCodPropiedad = (sCodPropiedad ?? "").Trim();
                        sNomb = (sNomb ?? "").Trim();
                        sNombFicha = (sNombFicha ?? "").Trim();
                        sAbrev = (sAbrev ?? "").Trim();
                        sDefinicion = (sDefinicion ?? "").Trim();
                        sUnidad = (sUnidad ?? "").Trim();
                        sTipoDato = (sTipoDato ?? "").Trim().ToUpper();
                        sLong1 = (sLong1 ?? "").Trim();
                        sLong2 = (sLong2 ?? "").Trim();
                        sFichaTec = (sFichaTec ?? "").Trim();
                        sCodTipoEquipo = (sCodTipoEquipo ?? "").Trim();
                        sNombTipoEquipo = (sNombTipoEquipo ?? "").Trim();
                        sFechModif = (sFechModif ?? "").Trim();
                        sUsuModif = (sUsuModif ?? "").Trim();

                        propcodi = (int)(((double?)worksheet.Cells[row, indexCodPropiedad].Value) ?? -1);
                        if (sCodTipoEquipo != "")
                            famcodi = (int)(((double?)worksheet.Cells[row, indexCodTipoEquipo].Value) ?? 0);

                        var valL1 = worksheet.Cells[row, indexLong1].Value;
                        var valL2 = worksheet.Cells[row, indexLong2].Value;
                        var valLI = worksheet.Cells[row, indexLimI].Value;
                        var valLS = worksheet.Cells[row, indexLimS].Value;

                        longitud1 = valL1 != null ? (EsEntero(valL1.ToString()) ? ((int?)(((double?)worksheet.Cells[row, indexLong1].Value))) : null) : null;
                        longitud2 = valL2 != null ? (EsEntero(valL2.ToString()) ? ((int?)(((double?)worksheet.Cells[row, indexLong2].Value))) : null) : null;                        
                        limI = valLI != null ? ObtenerLimite(valLI.ToString()) : null;
                        limS = valLS != null ? ObtenerLimite(valLS.ToString()) : null;
                        //fechaModificacion = ((DateTime?)worksheet.Cells[row, indexFechModif].Value) ?? DateTime.MinValue;
                    }
                    catch (Exception ex)
                    {
                        //No es necesario registrar el error de la conversión de datos de todas las celdas (pueden ocurrir como un máximo de 28 mil líneas de log para archivos de 7000 líneas de datos). 
                        //El tratamiento de estos errores se realiza en otra función (ValidarLecturaMacro de EquipamientoAppServicio) 
                        //que luego genera un .csv para el usuario (funcion GenerarArchivoPropiedadesErroneasCSV de EquipamientoAppServicio)
                    }

                    if (string.IsNullOrEmpty(sNomb) && string.IsNullOrEmpty(sCodTipoEquipo))
                    {
                        continue;
                    }

                    var regMantto = new FilaExcelPropiedad()
                    {
                        Row = row,
                        NumItem = numItem,
                        Propnomb = sNomb,
                        Propnombficha = sNombFicha,
                        Propabrev = sAbrev,
                        Propdefinicion = sDefinicion,
                        Propunidad = sUnidad,
                        Proptipo = sTipoDato,
                        StrProptipolong1 = sLong1,
                        StrProptipolong2 = sLong2,
                        Propfichaoficial = sFichaTec.ToUpper(),
                        StrFamcodi = sCodTipoEquipo,
                        NombreFamilia = sNombTipoEquipo,
                        StrPropfecmodificacion = sFechModif,
                        Propusumodificacion = sUsuModif,

                        Propcodi = propcodi,
                        Famcodi = famcodi,
                        Proptipolong1 = longitud1,
                        Proptipolong2 = longitud2,
                        Propliminf = limI,
                        Proplimsup = limS,
                        StrPropliminf = sLimI,
                        StrProplimsup = sLimS
                        //Propfecmodificacion = fechaModificacion,
                    };

                    listaMacro.Add(regMantto);
                }
            }

            return listaMacro;
        }

        /// <summary>
        /// Devuelve el valor correspondiente a Limite
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static decimal? ObtenerLimite(string valor)
        {
            decimal? salida;

            //verifico si es un valor numerico o no
            bool flagSalida = decimal.TryParse(valor, out decimal numDecimal);

            if (flagSalida)
            {
                bool contieneComa = valor.Contains(",");
                bool contienePunto = valor.Contains(".");

                if (contieneComa)
                {
                    if (contienePunto)
                    {
                        salida = numDecimal;
                    }
                    else
                    {
                        valor = valor.Replace(',', '.');
                        bool flagSalida2 = decimal.TryParse(valor, out decimal numDecimal2);
                        salida = numDecimal2;
                    }
                }
                else
                {
                    if (contienePunto)
                    {

                        salida = numDecimal;
                    }
                    else
                    {
                        salida = numDecimal;
                    }
                }

            }
            else
            {
                salida = null;
            }

            return salida;
        }

        /// <summary>
        /// Verifica si un string es entero
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool EsEntero(string num)
        {
            bool salida = false;

            salida = int.TryParse(num, out int numEntero);

            return salida;
        }

        /// <summary>
        /// Verifica si un string es decimal
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool EsDecimal(string num)
        {
            bool salida = false;

            salida = decimal.TryParse(num, out decimal numDecimal);

            return salida;
        }
        
        /// <summary>
         /// Validación de propiedades al leer del excel importado
         /// </summary>
         /// <param name="filaExcel"></param>
         /// <param name="listaFamilias"></param>
         /// <returns></returns>
        public string ValidarLecturaExcel(FilaExcelPropiedad filaExcel, List<EqFamiliaDTO> listaFamilias)
        {
            string columnCodPropiedad = "Codigo Propiedad: ";
            string columnNomb = "Nombre: ";
            string columnNombFicha = "Nombre ficha técnica: ";
            string columnAbrev = "Abreviatura: ";
            string columnDefinicion = "Definición: ";
            string columnUnidad = "Unidad: ";
            string columnTipoDato = "Tipo de dato: ";
            string columnLong1 = "Longitud 1: ";
            string columnLong2 = "Longitud 2: ";
            string columnFichaTec = "Ficha técnica: ";
            string columnCodTipoEquipo = "Código tipo equipo: ";
            string columnNombTipoEquipo = "Nombre tipo equipo: ";

            List<string> lMsgValidacion = new List<string>();

            List<string> listadoTipo = new List<string>();
            listadoTipo.Add("DECIMAL");
            listadoTipo.Add("ENTERO");
            listadoTipo.Add("CARACTER");
            listadoTipo.Add("FECHA");
            listadoTipo.Add("AÑO");
            listadoTipo.Add("ARCHIVO");
            listadoTipo.Add("FORMULA");

            List<string> listadoFichaTecnica = new List<string>();
            listadoFichaTecnica.Add("SI");
            listadoFichaTecnica.Add("NO");

            // Validar Nombre propiedad
            if (String.IsNullOrEmpty(filaExcel.Propnomb))
            {
                lMsgValidacion.Add(columnNomb + "Esta vacío o en blanco");
            }
            else
            {
                if (filaExcel.Propnomb.Contains("\n"))
                    lMsgValidacion.Add(columnNomb + "Tiene salto de línea");
            }


            if (filaExcel.Propfichaoficial == "SI")
            {
                // Validar Nombre Ficha técnica
                if (String.IsNullOrEmpty(filaExcel.Propnombficha))
                {
                    lMsgValidacion.Add(columnNombFicha + "Esta vacío o en blanco");
                }
                else
                {
                    if (filaExcel.Propnombficha.Contains("\n"))
                        lMsgValidacion.Add(columnNombFicha + "Tiene salto de línea");
                }
            }

            if (String.IsNullOrEmpty(filaExcel.Propabrev))
            {
                lMsgValidacion.Add(columnAbrev + "Esta vacío o en blanco");
            }

            // tipo de dato
            if (String.IsNullOrEmpty(filaExcel.Proptipo))
            {
                lMsgValidacion.Add(columnTipoDato + "Esta vacio o en blanco");
            }
            else
            {
                if (!listadoTipo.Contains(filaExcel.Proptipo.Trim().ToUpper()))
                {
                    lMsgValidacion.Add(columnTipoDato + "El valor no es valido");
                }
            }

            //Longitud 1
            if (filaExcel.Proptipolong1 != null)
            {
                if (filaExcel.Proptipolong1.ToString().Length > 4)
                {
                    lMsgValidacion.Add(columnLong1 + "Supera 4 dígitos");
                }
            }

            //Longitud 2
            if (filaExcel.Proptipolong2 != null)
            {
                if (filaExcel.Proptipolong2.ToString().Length > 3)
                {
                    lMsgValidacion.Add(columnLong2 + "Supera 3 dígitos");
                }
            }

            if (String.IsNullOrEmpty(filaExcel.Propfichaoficial))
            {
                lMsgValidacion.Add(columnFichaTec + "Esta vacío o en blanco");
            }
            else
            {
                if (!listadoFichaTecnica.Contains(filaExcel.Propfichaoficial))
                {
                    lMsgValidacion.Add(columnFichaTec + "Valor no valido");
                }
            }

            if (String.IsNullOrEmpty(filaExcel.StrFamcodi))
            {
                lMsgValidacion.Add(columnCodTipoEquipo + "Esta vacío o en blanco");
            }
            else if (filaExcel.Famcodi < 0)
            {
                lMsgValidacion.Add(columnCodTipoEquipo + "No es número válido");
            }
            else
            {
                EqFamiliaDTO regFam = listaFamilias.Find(x => x.Famcodi == filaExcel.Famcodi);
                if (regFam == null)
                {
                    lMsgValidacion.Add(columnCodTipoEquipo + "Código de tipo equipo no existe");
                }
                else
                {
                    if (regFam.Famestado == ConstantesAppServicio.Baja)
                    {
                        lMsgValidacion.Add(columnCodTipoEquipo + "El tipo equipo no se encuentra activo");
                    }
                }
            }

            return string.Join(". ", lMsgValidacion);
        }

        /// <summary>
        /// Genera log de propiedades erroneoas
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lstRegPropiedadesErroneos"></param>
        /// <returns></returns>
        public string GenerarArchivoPropiedadesErroneasCSV(string path, List<EqPropiedadDTO> lstRegPropiedadesErroneos)
        {
            string sLine = string.Empty;
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameCsv = "";
            fileNameCsv = sFecha + "_LogPropiedadesImport" + ".csv";

            using (var dbProviderWriter = new StreamWriter(new FileStream(path + fileNameCsv, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sLine = "ÍTEM;OBSERVACIONES;CÓDIGO DE PROPIEDAD;NOMBRE;NOMBRE DE FICHA TÉCNICA;ABREVIATURA;DEFINICIÓN;UNIDAD;TIPO DE DATO;LONGITUD 1;LONGITUD 2;FICHA TÉCNICA;CÓDIGO DE TIPO DE EQUIPO;NOMBRE DE TIPO DE EQUIPO;FECHA ÚLTIMA MODIFICACIÓN;USUARIO ÚLTIMA MODIFICACIÓN";
                dbProviderWriter.WriteLine(sLine);

                foreach (EqPropiedadDTO entity in lstRegPropiedadesErroneos)
                {
                    sLine = this.CreateFilaErroneaPropiedadString(entity);
                    dbProviderWriter.WriteLine(sLine);
                }

                dbProviderWriter.Close();
            }
            return fileNameCsv;
        }

        /// <summary>
        /// Guarda y actualiza propiedades masivamente
        /// </summary>
        /// <param name="listaNuevo"></param>
        /// <param name="listaModificado"></param>
        /// <param name="usuario"></param>
        public void GuardarDatosMasivosPropiedades(List<EqPropiedadDTO> listaNuevo, List<EqPropiedadDTO> listaModificado, string usuario)
        {
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guarda Registros nuevos masivamente
                        if (listaNuevo != null && listaNuevo.Any())
                        {
                            foreach (var item in listaNuevo)
                            {
                                item.Propusucreacion = usuario;
                                item.Propfeccreacion = DateTime.Now;
                                item.Propactivo = 1;

                                var idPropiedad = FactorySic.GetEqPropiedadRepository().Save(item, connection, transaction);
                            }
                        }

                        //Actualiza Registros masivamente
                        if (listaModificado != null && listaModificado.Any())
                        {
                            foreach (var item in listaModificado)
                            {
                                item.Propusumodificacion = usuario;
                                item.Propfecmodificacion = DateTime.Now;
                                //item.Propactivo = 1;

                                FactorySic.GetEqPropiedadRepository().Update(item, connection, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        #region Util Propiedades

        /// <summary>
        /// Metodo que escribe una fila del archivo .CSV de propiedades Erroneas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateFilaErroneaPropiedadString(EqPropiedadDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            sLine += ((entity.Propcodi > -1) ? entity.Propcodi.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propnomb != null) ? entity.Propnomb.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propnombficha != null) ? entity.Propnombficha.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propabrev != null) ? entity.Propabrev.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propdefinicion != null) ? entity.Propdefinicion.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propunidad != null) ? entity.Propunidad.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Proptipo != null) ? entity.Proptipo.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Proptipolong1 != null) ? entity.Proptipolong1.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Proptipolong2 != null) ? entity.Proptipolong2.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propfichaoficial != null) ? entity.Propfichaoficial.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Famcodi > 0) ? entity.Famcodi.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.NombreFamilia != null) ? entity.NombreFamilia.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propfecmodificacion != null) ? entity.Propfecmodificacion.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propusumodificacion != null) ? entity.Propusumodificacion.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            return sLine;
        }

        /// <summary>
        /// Eliminar archivos que están en la carpeta reporte Cada vez que se ingrese al módulo de Propiedades equipamiento
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="currentUserSession"></param>
        /// <param name="fileName"></param>
        public void EliminarArchivosReporte()
        {
            try
            {
                string pathAlternativo = AppDomain.CurrentDomain.BaseDirectory + ConstantesEquipamientoAppServicio.RutaReportes;
                var listaDocumentos = FileServer.ListarArhivos(null, pathAlternativo);

                if (listaDocumentos.Any())
                {
                    foreach (var item in listaDocumentos)
                    {
                        //los archivos se guardan con prefijo 2018, 2019, 2020, 2021, 2022 ... entonces se eliminaran
                        if (item.FileName.StartsWith("201") || item.FileName.StartsWith("202"))
                        {
                            FileServer.DeleteBlob(item.FileName, pathAlternativo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("No se pudo eliminar el archivo del servidor.", ex);
            }
        }

        /// <summary>
        /// Obtener listado de items de la ficha tecnica maestra por tipo
        /// </summary>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<FtFictecItemDTO> ObtenerItemFichaXtipoEquipoOficial(int famcodi)
        {
            var listaFichasAll = FactorySic.GetFtFictecXTipoEquipoRepository().GetByCriteria("A");
            var fichaPrincipal = servFictec.GetFichaMaestraPrincipal(ConstantesFichaTecnica.FichaMaestraPortal);
            List<FtFictecItemDTO> listaItemGlobal = new List<FtFictecItemDTO>();
            if (fichaPrincipal != null)
            {
                var listaFictecdetXfm = servFictec.ListarAllFichaTecnicaByMaestra(fichaPrincipal.Fteccodi);
                List<int> listaCodifichas = listaFictecdetXfm.Select(x => x.Fteqcodi).ToList();

                var fichasfilter = listaFichasAll.Where(x => listaCodifichas.Contains(x.Fteqcodi)).ToList();
                fichasfilter = fichasfilter.Where(x => x.Famcodi == famcodi).ToList();

                FtFictecXTipoEquipoDTO fichaxtipoequipo = fichasfilter.Any() ? fichasfilter.First() : null;

                if (fichaxtipoequipo != null)
                {
                    List<FtFictecItemDTO> listaItems, listaAllItems;
                    List<TreeItemFichaTecnica> listaItemsJson;

                    var servFT = new FichaTecnicaAppServicio();
                    FTFiltroReporteExcel objFiltro = servFT.GetFichaYDatosXEquipoOModo(fichaxtipoequipo.Fteqcodi, -2, false, ConstantesFichaTecnica.INTRANET, DateTime.Today);
                    servFT.ListarTreeItemsFichaTecnica(objFiltro, out listaAllItems, out listaItems, out listaItemsJson);

                    listaItemGlobal = listaAllItems;
                }
            }

            return listaItemGlobal;
        }

        #endregion

        #endregion

        #region Métodos Tabla EQ_RELACION

        /// <summary>
        /// Inserta un registro de la tabla EQ_RELACION
        /// </summary>
        public void SaveEqRelacion(EqRelacionDTO entity)
        {
            try
            {
                FactorySic.GetEqRelacionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_RELACION
        /// </summary>
        public void UpdateEqRelacion(EqRelacionDTO entity)
        {
            try
            {
                FactorySic.GetEqRelacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_RELACION
        /// </summary>
        public void DeleteEqRelacion(int relacioncodi)
        {
            try
            {
                FactorySic.GetEqRelacionRepository().Delete(relacioncodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_RELACION
        /// </summary>
        public EqRelacionDTO GetByIdEqRelacion(int relacioncodi)
        {
            return FactorySic.GetEqRelacionRepository().GetById(relacioncodi);
        }

        #endregion

        #region Métodos Tabla EQ_TIPOAREA

        /// <summary>
        /// Inserta un registro de la tabla EQ_TIPOAREA
        /// </summary>
        public void SaveEqTipoarea(EqTipoareaDTO entity)
        {
            try
            {
                FactorySic.GetEqTipoareaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_TIPOAREA
        /// </summary>
        public void UpdateEqTipoarea(EqTipoareaDTO entity)
        {
            try
            {
                FactorySic.GetEqTipoareaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_TIPOAREA
        /// </summary>
        public void DeleteEqTipoarea(int tareacodi)
        {
            try
            {
                FactorySic.GetEqTipoareaRepository().Delete(tareacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_TIPOAREA
        /// </summary>
        public EqTipoareaDTO GetByIdEqTipoarea(int tareacodi)
        {
            return FactorySic.GetEqTipoareaRepository().GetById(tareacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_TIPOAREA
        /// </summary>
        public List<EqTipoareaDTO> ListEqTipoareas()
        {
            return FactorySic.GetEqTipoareaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqTipoarea
        /// </summary>
        public List<EqTipoareaDTO> GetByCriteriaEqTipoareas()
        {
            return FactorySic.GetEqTipoareaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EQ_TIPOREL

        /// <summary>
        /// Inserta un registro de la tabla EQ_TIPOREL
        /// </summary>
        public void SaveEqTiporel(EqTiporelDTO entity)
        {
            try
            {
                FactorySic.GetEqTiporelRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_TIPOREL
        /// </summary>
        public void UpdateEqTiporel(EqTiporelDTO entity)
        {
            try
            {
                FactorySic.GetEqTiporelRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_TIPOREL
        /// </summary>
        public void DeleteEqTiporel(int tiporelcodi)
        {
            try
            {
                FactorySic.GetEqTiporelRepository().Delete(tiporelcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_TIPOREL
        /// </summary>
        public EqTiporelDTO GetByIdEqTiporel(int tiporelcodi)
        {
            return FactorySic.GetEqTiporelRepository().GetById(tiporelcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_TIPOREL
        /// </summary>
        public List<EqTiporelDTO> ListEqTiporels()
        {
            return FactorySic.GetEqTiporelRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqTiporel
        /// </summary>
        public List<EqTiporelDTO> GetByCriteriaEqTiporels()
        {
            return FactorySic.GetEqTiporelRepository().GetByCriteria();
        }
        /// <summary>
        /// Listado de registros de Tipo de relación filtrado por nombre y estado
        /// </summary>
        /// <param name="strNombreTiporel">Nombre de tipo de relación</param>
        /// <param name="strEstado">Estado</param>
        /// <param name="nroPagina">Num. página</param>
        /// <param name="nroFilas">Num. filas</param>
        /// <returns></returns>
        public List<EqTiporelDTO> ListadoTipoRelxFiltroPaginado(string strNombreTiporel, string strEstado, int nroPagina, int nroFilas)
        {
            return FactorySic.GetEqTiporelRepository().ListarTipoRelxFiltro(strNombreTiporel, strEstado, nroPagina, nroFilas);
        }
        /// <summary>
        /// Retorna la cantidad total de registros que se obtienen luego de aplicar el filtro
        /// </summary>
        /// <param name="strNombreTiporel">Nombre de tipo de relación</param>
        /// <param name="strEstado">Estado</param>
        /// <returns></returns>
        public int CantidadTipoRelxFiltro(string strNombreTiporel, string strEstado)
        {
            return FactorySic.GetEqTiporelRepository().CantidadTipoRelxFiltro(strNombreTiporel, strEstado);
        }

        #endregion

        #region Métodos Tabla EQ_EQUIREL

        /// <summary>
        /// Graba equipo relacionado
        /// </summary>
        /// <param name="equipo"></param>
        public void SaveEqEquiRelDTO(EqEquirelDTO equipo)
        {
            FactorySic.GetEqEquirelRepository().Save(equipo);
        }

        /// <summary>
        /// Elimina equipo relacionado
        /// </summary>
        /// <param name="equipo"></param>
        public void DeleteEqEquiRelDTO(int equicodi1, int tiporelcodi, int equicodi2, string user)
        {
            FactorySic.GetEqEquirelRepository().Delete(equicodi1, tiporelcodi, equicodi2);
            FactorySic.GetEqEquirelRepository().Delete_UpdateAuditoria(equicodi1, tiporelcodi, equicodi2, user);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla Eq_Equirel
        /// </summary>
        public List<EqEquirelDTO> GetByCriteriaEqEquirels(int tiporelcodi)
        {
            var l = FactorySic.GetEqEquirelRepository().GetByCriteria(-1, tiporelcodi.ToString());

            foreach (var reg in l)
            {
                reg.EquirelfecmodificacionDesc = reg.Equirelfecmodificacion != null ? reg.Equirelfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
            }

            return l;
        }

        #endregion

        #region Métodos Tabla EQ_FAMILIA

        /// <summary>
        /// Inserta un registro de la tabla EQ_FAMILIA
        /// </summary>
        public void SaveEqFamilia(EqFamiliaDTO entity)
        {
            try
            {
                FactorySic.GetEqFamiliaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_FAMILIA
        /// </summary>
        public void UpdateEqFamilia(EqFamiliaDTO entity)
        {
            try
            {
                FactorySic.GetEqFamiliaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_FAMILIA
        /// </summary>
        public void DeleteEqFamilia(int famcodi, string username)
        {
            try
            {
                FactorySic.GetEqFamiliaRepository().Delete(famcodi);
                FactorySic.GetEqFamiliaRepository().Delete_UpdateAuditoria(famcodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_FAMILIA
        /// </summary>
        public EqFamiliaDTO GetByIdEqFamilia(int famcodi)
        {
            return FactorySic.GetEqFamiliaRepository().GetById(famcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_FAMILIA
        /// </summary>
        public List<EqFamiliaDTO> ListEqFamilias()
        {
            return FactorySic.GetEqFamiliaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EqFamilia
        /// </summary>
        public List<EqFamiliaDTO> GetByCriteriaEqFamilias(string strEstado)
        {
            return FactorySic.GetEqFamiliaRepository().GetByCriteria(strEstado);
        }

        /// <summary>
        /// Listas las familias de equipo con procedimiento de maniobras
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ObtenerFamiliasProcManiobras()
        {
            return FactorySic.GetEqFamiliaRepository().ObtenerFamiliasProcManiobras();
        }

        #endregion

        //inicio modificado
        #region Tabla EQ_CATEGORIA
        /// <summary>
        /// Inserta un registro de la tabla EQ_CATEGORIA
        /// </summary>
        public void SaveEqCategoria(EqCategoriaDTO entity)
        {
            try
            {
                FactorySic.GetEqCategoriaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_CATEGORIA
        /// </summary>
        public void UpdateEqCategoria(EqCategoriaDTO entity)
        {
            try
            {
                FactorySic.GetEqCategoriaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_CATEGORIA
        /// </summary>
        public void DeleteEqCategoria(int ctgcodi)
        {
            try
            {
                FactorySic.GetEqCategoriaRepository().Delete(ctgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_CATEGORIA
        /// </summary>
        public EqCategoriaDTO GetByIdEqCategoria(int ctgcodi)
        {
            return FactorySic.GetEqCategoriaRepository().GetById(ctgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA
        /// </summary>
        public List<EqCategoriaDTO> ListEqCategoriaByFamilia(int famcodi)
        {
            return FactorySic.GetEqCategoriaRepository().ListByFamiliaAndEstado(famcodi, ConstantesAppServicio.Activo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA
        /// </summary>
        public List<EqCategoriaDTO> ListEqCategoriaByFamiliaAndEstado(int famcodi, string estado)
        {
            return FactorySic.GetEqCategoriaRepository().ListByFamiliaAndEstado(famcodi, estado);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA filtrados por tipo de equipo
        /// </summary>
        public List<EqCategoriaDTO> ListEqCategoriaPadre(int famcodi, int ctgcodi)
        {
            return FactorySic.GetEqCategoriaRepository().ListPadre(famcodi, ctgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA que pueden ser asignados a equipos
        /// </summary>
        public List<EqCategoriaDTO> ListEqCategoriaClasificacion(int famcodi)
        {
            return FactorySic.GetEqCategoriaRepository().ListaCategoriaClasificacionByFamiliaAndEstado(famcodi, ConstantesAppServicio.Activo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA que son hijos
        /// </summary>
        public List<EqCategoriaDTO> ListCategoriaHijoByIdPadre(int famcodi, int ctgpadrecodi)
        {
            return FactorySic.GetEqCategoriaRepository().ListCategoriaHijoByIdPadre(famcodi, ctgpadrecodi);
        }

        //inicio agregado
        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA que son hijos por empresa
        /// </summary>
        public List<EqCategoriaDTO> ListCategoriaHijoByIdPadreAndEmpresa(int famcodi, int ctgpadrecodi, int emprcodi)
        {
            return FactorySic.GetEqCategoriaRepository().ListCategoriaHijoByIdPadreAndEmpresa(famcodi, ctgpadrecodi, emprcodi);
        }
        //fin agregado
        #endregion

        #region Tabla EQ_CATEGORIA_DETALLE
        /// <summary>
        /// Inserta un registro de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public void SaveEqCategoriaDetalle(EqCategoriaDetDTO entity)
        {
            try
            {
                FactorySic.GetEqCategoriaDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public void UpdateEqCategoriaDetalle(EqCategoriaDetDTO entity)
        {
            try
            {
                FactorySic.GetEqCategoriaDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public void DeleteEqCategoriaDetalle(int ctgcodi)
        {
            try
            {
                FactorySic.GetEqCategoriaDetalleRepository().Delete(ctgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public EqCategoriaDetDTO GetByIdEqCategoriaDetalle(int ctgcodi)
        {
            return FactorySic.GetEqCategoriaDetalleRepository().GetById(ctgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public List<EqCategoriaDetDTO> ListEqCategoriaDetalleByCategoria(int ctgcodi)
        {
            return FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstado(ctgcodi, ConstantesAppServicio.Activo);
        }

        //inicio agregado
        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_DETALLE por empresa
        /// </summary>
        public List<EqCategoriaDetDTO> ListEqCategoriaDetalleByCategoriaAndEmpresa(int ctgcodi, int emprcodi)
        {
            return FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstadoAndEmpresa(ctgcodi, ConstantesAppServicio.Activo, emprcodi);
        }
        //fin agregado

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_DETALLE
        /// </summary>
        public List<EqCategoriaDetDTO> ListEqCategoriaDetalleByCategoriaAndEstado(int ctgcodi, string estado)
        {
            return FactorySic.GetEqCategoriaDetalleRepository().ListByCategoriaAndEstado(ctgcodi, estado);
        }
        #endregion

        #region Tabla EQ_CATEGORIA_EQUIPO
        /// <summary>
        /// Inserta un registro de la tabla EQ_CATEGORIA_EQUIPO
        /// </summary>
        public void SaveEqCategoriaEquipo(EqCategoriaEquipoDTO entity)
        {
            try
            {
                FactorySic.GetEqCategoriaEquipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EQ_CATEGORIA_EQUIPO
        /// </summary>
        public void UpdateEqCategoriaEquipo(EqCategoriaEquipoDTO entity)
        {
            try
            {
                FactorySic.GetEqCategoriaEquipoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EQ_CATEGORIA_EQUIPO
        /// </summary>
        public void DeleteEqCategoriaEquipo(int ctgcodi)
        {
            try
            {
                FactorySic.GetEqCategoriaEquipoRepository().Delete(ctgcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EQ_CATEGORIA_EQUIPO
        /// </summary>
        public EqCategoriaEquipoDTO GetByIdEqCategoriaEquipo(int ctgcodi, int equicodi)
        {
            return FactorySic.GetEqCategoriaEquipoRepository().GetById(ctgcodi, equicodi);
        }

        public EqCategoriaEquipoDTO GetByIdEqCategoriaIdEquipo(int ctgcodi, int equicodi)
        {
            return FactorySic.GetEqCategoriaEquipoRepository().GetByIdEquipo(ctgcodi, equicodi);
        }

        public int TotalClasificacion(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, int iCategoria, int iSubclasificacion, string nombre)
        {
            return FactorySic.GetEqCategoriaEquipoRepository()
                .TotalClasificacion(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, iCategoria, iSubclasificacion, nombre);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_EQUIPO
        /// </summary>
        public List<EqCategoriaEquipoDTO> ListaClasificacionPaginado(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, int iCategoria, int iSubclasificacion, string nombre, int nroPagina, int nroFilas)
        {
            return FactorySic.GetEqCategoriaEquipoRepository()
                    .ListaPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, iCategoria, iSubclasificacion, nombre, nroPagina, nroFilas);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_EQUIPO segun categoria y equipo
        /// </summary>
        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaAndEquipo(int ctgdetcodi, int equicodi)
        {
            return FactorySic.GetEqCategoriaEquipoRepository().ListaClasificacionByCategoriaAndEquipo(ctgdetcodi, equicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_EQUIPO segun categoria y empresa
        /// </summary>
        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaAndEmpresa(int ctgdetcodi, int emprcodi)
        {
            List<EqCategoriaEquipoDTO> list = FactorySic.GetEqCategoriaEquipoRepository().ListaClasificacionByCategoriaAndEmpresa(ctgdetcodi, emprcodi);
            if (list.Count > 0)
            {
                int max = list.Select(x => x.Areanomb.Length).Max();

                foreach (EqCategoriaEquipoDTO item in list)
                {
                    int count = max - item.Areanomb.Length;
                    string espacio = string.Empty;
                    for (int i = 0; i <= count; i++)
                    {
                        espacio = espacio + "-";
                    }


                    item.Equinomb = item.Areanomb + espacio + " " + item.Equinomb;
                }

                return list.OrderBy(x => x.Equinomb).ToList();
            }
            return list;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_EQUIPO segun categoria padre y equipo
        /// </summary>
        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaPadreAndEquipo(int ctgpadrecodi, int equicodi)
        {
            return FactorySic.GetEqCategoriaEquipoRepository().ListaClasificacionByCategoriaPadreAndEquipo(ctgpadrecodi, equicodi);
        }

        //inicio agregado
        /// <summary>
        /// Permite listar todos los registros de la tabla EQ_CATEGORIA_EQUIPO segun categoria detalle
        /// </summary>
        public List<EqCategoriaEquipoDTO> ListaClasificacionByCategoriaDetalle(int ctgdetcodi)
        {
            return FactorySic.GetEqCategoriaEquipoRepository().ListaClasificacionByCategoriaDetalle(ctgdetcodi);
        }
        //fin agregado

        #endregion
        //fin modificado

        public List<EqEquipoDTO> ListarGeneradoresTermicosPorModoOperacion(int grupocodi)
        {
            return FactorySic.GetEqEquipoRepository().ListarGeneradoresTermicosPorModoOperacion(grupocodi);
        }

        #region REPORTE POTENCIA EFECTIVA

        public List<ReportePotenciaEmpresa> ReportePotencia(int tipoGeneracion, int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin, string tipoReporte, string combustible)
        {
            var resultadoHidro = new List<DetalleReportePotenciaDTO>();
            var resultadoTermo = new List<DetalleReportePotenciaDTO>();
            var resultadoSolar = new List<DetalleReportePotenciaDTO>();
            var resultadoEolica = new List<DetalleReportePotenciaDTO>();
            var lPeriodos = EPDate.GetPeriodos(fechaIni, fechaFin);
            switch (tipoGeneracion)
            {
                case 1://Hidraulica
                    resultadoHidro = FactorySic.GetEqEquipoRepository().DatosReportePotenciaEfectivaHidraulicas(iEmpresa, iCentral, fechaIni, fechaFin);
                    break;
                case 2://Térmica
                    resultadoTermo = FactorySic.GetPrGrupoRepository().DatosReportePotenciaEfectivaTermicas(iEmpresa, iCentral, fechaIni, fechaFin, tipoReporte);
                    break;
                case 3://Solar
                    resultadoSolar = FactorySic.GetEqEquipoRepository().DatosReportePotenciaEfectivaSolares(iEmpresa, iCentral, fechaIni, fechaFin);
                    break;
                case 4://Eolica
                    resultadoEolica = FactorySic.GetEqEquipoRepository().DatosReportePotenciaEfectivaEolicas(iEmpresa, iCentral, fechaIni, fechaFin);
                    break;
                default:
                    resultadoHidro = FactorySic.GetEqEquipoRepository().DatosReportePotenciaEfectivaHidraulicas(iEmpresa, iCentral, fechaIni, fechaFin);
                    resultadoTermo = FactorySic.GetPrGrupoRepository().DatosReportePotenciaEfectivaTermicas(iEmpresa, iCentral, fechaIni, fechaFin, tipoReporte);
                    resultadoSolar = FactorySic.GetEqEquipoRepository().DatosReportePotenciaEfectivaSolares(iEmpresa, iCentral, fechaIni, fechaFin);
                    resultadoEolica = FactorySic.GetEqEquipoRepository().DatosReportePotenciaEfectivaEolicas(iEmpresa, iCentral, fechaIni, fechaFin);
                    break;
            }

            //Hacer las agregaciones
            var listaDatosPorCentral = new List<ReportePotenciaCentralDTO>();
            //DATOS HIDRO
            var resultadoAgrupadoCentralH = resultadoHidro.GroupBy(t => t.Central).Select(g => g.ToList()).ToList();
            foreach (var listaAgrupadaCentralH in resultadoAgrupadoCentralH)
            {
                var datoCentral = new ReportePotenciaCentralDTO();

                datoCentral.TotalPe = new List<decimal?>();
                for (var i = 0; i < lPeriodos.Count; i++)
                {
                    datoCentral.TotalPe.Add(new decimal());
                }
                foreach (var detalle in listaAgrupadaCentralH)
                {
                    datoCentral.Empresa = detalle.Empresa;
                    datoCentral.GrupoCentral = detalle.Central;
                    for (var i = 0; i < lPeriodos.Count; i++)
                    {
                        datoCentral.TotalPe[i] = (datoCentral.TotalPe[i] ?? 0) + (detalle.Potencia[i] ?? 0);
                    }
                }


                datoCentral.TipoGeneracion = "H";
                datoCentral.DatosPotencia = listaAgrupadaCentralH;
                datoCentral.NombreTipoGeneracion = "Hidroeléctrica";
                listaDatosPorCentral.Add(datoCentral);
            }
            //DATOS SOLAR
            var resultadoAgrupadoCentralS = resultadoSolar.GroupBy(t => t.Central).Select(g => g.ToList()).ToList();
            foreach (var listaAgrupadaCentralS in resultadoAgrupadoCentralS)
            {
                var datoCentral = new ReportePotenciaCentralDTO();
                datoCentral.TotalPe = new List<decimal?>();
                for (var i = 0; i < lPeriodos.Count; i++)
                {
                    datoCentral.TotalPe.Add(new decimal());
                }
                foreach (var detalle in listaAgrupadaCentralS)
                {
                    datoCentral.Empresa = detalle.Empresa;
                    datoCentral.GrupoCentral = detalle.Central;
                    for (var i = 0; i < lPeriodos.Count; i++)
                    {
                        datoCentral.TotalPe[i] = (datoCentral.TotalPe[i] ?? 0) + (detalle.Potencia[i] ?? 0);
                    }
                }

                datoCentral.TipoGeneracion = "S";
                datoCentral.DatosPotencia = listaAgrupadaCentralS;
                datoCentral.NombreTipoGeneracion = "Solar";
                listaDatosPorCentral.Add(datoCentral);
            }
            //DATOS EOLICO
            var resultadoAgrupadoCentralE = resultadoEolica.GroupBy(t => t.Central).Select(g => g.ToList()).ToList();
            foreach (var listaAgrupadaCentralE in resultadoAgrupadoCentralE)
            {
                var datoCentral = new ReportePotenciaCentralDTO();
                datoCentral.TotalPe = new List<decimal?>();
                for (var i = 0; i < lPeriodos.Count; i++)
                {
                    datoCentral.TotalPe.Add(new decimal());
                }
                foreach (var detalle in listaAgrupadaCentralE)
                {
                    datoCentral.Empresa = detalle.Empresa;
                    datoCentral.GrupoCentral = detalle.Central;
                    for (var i = 0; i < lPeriodos.Count; i++)
                    {
                        datoCentral.TotalPe[i] = (datoCentral.TotalPe[i] ?? 0) + (detalle.Potencia[i] ?? 0);
                    }
                }

                datoCentral.TipoGeneracion = "E";
                datoCentral.DatosPotencia = listaAgrupadaCentralE;
                datoCentral.NombreTipoGeneracion = "Eólica";
                listaDatosPorCentral.Add(datoCentral);
            }
            //DATOS TERMICO
            var resultadoAgrupadoCentralT = resultadoTermo.GroupBy(t => t.Central).Select(g => g.ToList()).ToList();
            foreach (var listaAgrupadaCentralT in resultadoAgrupadoCentralT)
            {
                var datoCentral = new ReportePotenciaCentralDTO();
                datoCentral.TotalPe = new List<decimal?>();
                for (var i = 0; i < lPeriodos.Count; i++)
                {
                    datoCentral.TotalPe.Add(new decimal());
                }
                foreach (var detalle in listaAgrupadaCentralT)
                {
                    datoCentral.Empresa = detalle.Empresa;
                    datoCentral.GrupoCentral = detalle.Central;
                    for (var i = 0; i < lPeriodos.Count; i++)
                    {
                        datoCentral.TotalPe[i] = (datoCentral.TotalPe[i] ?? 0) + (detalle.Potencia[i] ?? 0);
                    }
                }

                datoCentral.TipoGeneracion = "T";
                datoCentral.DatosPotencia = listaAgrupadaCentralT;
                datoCentral.NombreTipoGeneracion = "Termoeléctrica";
                listaDatosPorCentral.Add(datoCentral);
            }

            var resultadoAgrupadoEmpresaTipoGeneracion = listaDatosPorCentral.GroupBy(t => new { t.Empresa, t.TipoGeneracion }).Select(q => q.ToList()).ToList();
            var listadoDatosEmpresaTipoGeneracion = new List<ReportePotenciaTipoGeneracion>();

            foreach (var listaEmpresaTipoGen in resultadoAgrupadoEmpresaTipoGeneracion)
            {
                var auxEmpresaTipoGen = new ReportePotenciaTipoGeneracion();

                auxEmpresaTipoGen.PotenciaEmpresaTipo = new List<decimal?>();
                for (var i = 0; i < lPeriodos.Count; i++)
                {
                    auxEmpresaTipoGen.PotenciaEmpresaTipo.Add(new decimal());
                }

                foreach (var oEmpresaTipoGen in listaEmpresaTipoGen)
                {
                    auxEmpresaTipoGen.Empresa = oEmpresaTipoGen.Empresa;
                    auxEmpresaTipoGen.TipoGeneracionGrupo = oEmpresaTipoGen.NombreTipoGeneracion;

                    for (var i = 0; i < lPeriodos.Count; i++)
                    {
                        auxEmpresaTipoGen.PotenciaEmpresaTipo[i] = (auxEmpresaTipoGen.PotenciaEmpresaTipo[i] ?? 0) + (oEmpresaTipoGen.TotalPe[i] ?? 0);
                    }
                }
                auxEmpresaTipoGen.DatosCental = listaEmpresaTipoGen;
                auxEmpresaTipoGen.TipoGeneracionTotal = auxEmpresaTipoGen.TipoGeneracionGrupo + " total";
                listadoDatosEmpresaTipoGeneracion.Add(auxEmpresaTipoGen);
            }
            var listadoPorEmpresas = listadoDatosEmpresaTipoGeneracion.GroupBy(t => t.Empresa).Select(q => q.ToList()).ToList();
            var listadoDatosEmpresas = new List<ReportePotenciaEmpresa>();
            foreach (var listaEmpresa in listadoPorEmpresas)
            {
                var auxPotenciaEmpresa = new ReportePotenciaEmpresa();
                auxPotenciaEmpresa.TotalPotenciaEmpresa = new List<decimal?>();
                for (var i = 0; i < lPeriodos.Count; i++)
                {
                    auxPotenciaEmpresa.TotalPotenciaEmpresa.Add(new decimal());
                }

                foreach (var datosEmpresa in listaEmpresa)
                {
                    auxPotenciaEmpresa.EmpresaGrupo = datosEmpresa.Empresa;
                    for (var i = 0; i < lPeriodos.Count; i++)
                    {
                        auxPotenciaEmpresa.TotalPotenciaEmpresa[i] = (auxPotenciaEmpresa.TotalPotenciaEmpresa[i] ?? 0) + (datosEmpresa.PotenciaEmpresaTipo[i] ?? 0);
                    }
                }


                listadoDatosEmpresas.Add(auxPotenciaEmpresa);
            }
            return listadoDatosEmpresas;
        }

        #endregion

        /// <summary>
        /// Método que retorna el listado de equipos y propiedades vigentes para los formatos AGC por Equipo
        /// </summary>
        /// <param name="fecha">fecha de vigencia</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposPropiedadesAGC(string fecha)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquiposPropiedadesAGC(fecha);
        }

        /// <summary>
        /// Permite obtener el último valor vigente de una propiedad y equipo para una fecha de vigencia
        /// </summary>
        /// <param name="propcodi">Código de propiedad</param>
        /// <param name="equicodi">Código de equipo</param>
        /// <param name="fecha">Fecha de vigencia</param>
        /// <returns></returns>
        public string ObtenerValorPropiedadEquipoFecha(int propcodi, int equicodi, string fecha)
        {
            return FactorySic.GetEqPropequiRepository().ObtenerValorPropiedadEquipoFecha(propcodi, equicodi, fecha);
        }

        /// <summary>
        /// Método que retorna el listado de equipos vigentes para los formatos AGC
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposAGC()
        {
            return FactorySic.GetEqEquipoRepository().ListarEquiposAGC();
        }

       /// <summary>
      /// Listado de equipos por código y nombre(abrev tipo area, area, abrev. equipo y abrev. nombre )
      /// </summary>
      /// <returns></returns>
      public List<EqEquipoDTO> ListadoEquipoNombre(string famcodis)
      {
          return FactorySic.GetEqEquipoRepository().ListadoEquipoNombre(famcodis);
      }

        /// <summary>
        /// Listado de equipos filtrados por su padre, lista TODOS los equipos sin filtro de estado
        /// </summary>
        /// <param name="equipadre">Código de equipo padre</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposPorPadre(int equipadre)
        {
            return FactorySic.GetEqEquipoRepository().ObtenerPorPadre(equipadre);
        }

        /// <summary>
        /// Método que retorna el listado de equipos y propiedades vigentes
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposPropiedades(int propcodi, DateTime fecha, int emprCodi, int areacodi, int famCodi, int nroPage, int pageSize)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquiposPropiedades(propcodi, fecha, emprCodi, areacodi, famCodi, nroPage, pageSize);
        }

        public int TotalEquiposPropiedades(int emprCodi, int areacodi, int famCodi)
        {
            return FactorySic.GetEqEquipoRepository().TotalEquiposPropiedades(emprCodi, areacodi, famCodi);
        }

        #region Medidores de Generación PR15
        /// <summary>
        /// Método que retorna el listado de equipos asociados a un punto de medición
        /// </summary>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <returns>Listado de equipos</returns>
        public List<EqEquipoDTO> ListarEquiposPorPuntoMedicion(int ptomedicodi)
        {
            return FactorySic.GetEqEquipoRepository().GetEquipoByPuntoMedicion(ptomedicodi);
        }
        #endregion

        #region Propiedades del Equipo

        /// <summary>
        /// Generar handson
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="iEquipo"></param>
        /// <param name="filtroFicha"></param>
        /// <param name="orden"></param>
        /// <param name="habilitarEdicion"></param>
        /// <param name="handson"></param>
        /// <param name="listado"></param>
        public void GetGridExcelWebPropiedadesEquipo(DateTime fechaConsulta, int iEquipo, string filtroFicha, bool habilitarEdicion
                                                , out HandsonModel handson, out List<EqPropequiDTO> listado, out List<int> listaDespuesFicha8)
        {
            var oEquipo = FactorySic.GetEqEquipoRepository().GetById(iEquipo);

            string strFechaConfig = System.Configuration.ConfigurationManager.AppSettings["FechaHoraProdVigencia"];
            DateTime fechaEnProdFicha8 = DateTime.ParseExact(strFechaConfig, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

            //Lista de propiedades
            List<EqPropiedadDTO> listaProp = this.ListEqPropiedads().ToList();
            listaProp = listaProp.Where(x => x.Famcodi == oEquipo.Famcodi || x.Famcodi == 0).ToList(); //-1
            if (filtroFicha == "S")
                listaProp = listaProp.Where(x => x.Propfichaoficial == "S").ToList();
            if (filtroFicha == "1")
                listaProp = listaProp.Where(x => ListaPropiedadesAplicativos().Contains(x.Propcodi)).ToList();

            //omitir las propiedades eliminadas
            List<int> propiedadesEliminadas = new List<int>() { 1834, 1833, 1557, 1526 };
            listaProp = listaProp.Where(x => !propiedadesEliminadas.Contains(x.Propcodi)).ToList();

            //excluir filas de las propiedades de Auditoria
            List<int> propiedadesSoloLectura = ListaPropiedadAuditoria();
            listaProp = listaProp.Where(x => !propiedadesSoloLectura.Contains(x.Propcodi)).ToList();

            //Lista de valores vigentes
            listado = this.ListarValoresVigentesPropiedades(fechaConsulta, iEquipo, oEquipo.Famcodi.Value, "", filtroFicha); //por defecto orden de Ficha tecnica
            listado = listado.Where(x => !propiedadesEliminadas.Contains(x.Propcodi)).ToList();
            listado = listado.Where(x => !propiedadesSoloLectura.Contains(x.Propcodi)).ToList();

            foreach (var reg in listado)
                FormatearPropequi(reg);

            foreach (var reg in listaProp)
            {
                var propexist = listado.Find(x => x.Propcodi == reg.Propcodi);
                if (propexist == null)
                {
                    listado.Add(new EqPropequiDTO()
                    {
                        Propcodi = reg.Propcodi,
                        Equicodi = iEquipo,
                        Famcodi = reg.Famcodi,
                        Propunidad = reg.Propunidad,
                        Propnomb = reg.Propnomb,
                        Propnombficha = reg.Propnombficha ?? "",
                        FechapropequiDesc = string.Empty,
                        Valor = string.Empty,
                        Propequicheckcero = 0,
                        Propequicomentario = string.Empty,
                        Propequisustento = string.Empty,
                        //Propequiobservacion = string.Empty,
                        UltimaModificacionFechaDesc = string.Empty,
                        UltimaModificacionUsuarioDesc = string.Empty,
                        Orden = reg.Orden,
                        Propfichaoficial = reg.Propfichaoficial,
                    });
                }
                else
                {
                    propexist.Famcodi = reg.Famcodi;
                    propexist.Orden = reg.Orden;
                    propexist.Propfichaoficial = reg.Propfichaoficial;
                    propexist.Propnombficha = reg.Propnombficha ?? "";
                }
            }

            //formatear orden según ficha maestra
            List<FtFictecItemDTO> listaFicTemItems = new List<FtFictecItemDTO>();
            if (filtroFicha == "S")
            {
                //Obtiene lista de todos los Item de las ficha maestra
                listaFicTemItems = ObtenerItemFichaXtipoEquipoOficial(oEquipo.Famcodi.Value);

                //formatear orden
                if (listaFicTemItems.Any())
                {
                    foreach (var item in listado)
                    {
                        var fictecitem = listaFicTemItems.Find(x => x.Propcodi == item.Propcodi);
                        item.Orden = fictecitem != null ? fictecitem.OrdenNumerico : 999999999;
                    }
                }
            }

            //Orden de acuerdo al filtro de ficha técnica
            listado = filtroFicha == "S" ? listado.OrderBy(x => x.Orden).ToList() : listado.OrderBy(x => x.Propcodi).ToList();

            //if (orden == "1")
            //    listado = listado.OrderBy(x => x.Famcodi).ThenBy(x => x.Orden ?? 10000).ToList();
            //if (orden == "2") //orden alfabetico
            //    listado = listado.OrderBy(x => x.Famcodi).ThenBy(x => x.Propnomb).ToList();

            //Header
            List<CabeceraRow> listaCabecera = new List<CabeceraRow>();
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propcodi", TituloRow = "Código", Ancho = 40, AlineacionHorizontal = "Izquierda", EsEditable = false, TipoDato = GridExcel.TipoTexto, NombreClase = "htRight htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propnomb", TituloRow = "Propiedad", Ancho = 250, AlineacionHorizontal = "Izquierda", EsEditable = false, TipoDato = GridExcel.TipoTexto, NombreClase = "htLeft htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propnombficha", TituloRow = "Nombre Ficha Técnica", Ancho = 250, AlineacionHorizontal = "Izquierda", EsEditable = false, TipoDato = GridExcel.TipoTexto, NombreClase = "htLeft htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propunidad", TituloRow = "Unidad", Ancho = 60, AlineacionHorizontal = "Izquierda", EsEditable = false, TipoDato = GridExcel.TipoTexto, NombreClase = "htCenter htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "FechapropequiDesc", TituloRow = "Fecha de Vigencia", Ancho = 80, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoFecha, NombreClase = "htCenter htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Valor", TituloRow = "Valor", Ancho = 90, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoTexto, NombreClase = "htCenter htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propequicheckcero", TituloRow = "", Ancho = 30, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoCheck, NombreClase = "htCenter htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propequicomentario", TituloRow = "Comentario", Ancho = 300, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoTexto, NombreClase = "htLeft htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Equiabrev", TituloRow = "", Ancho = 30, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoTexto, NombreClase = "htLeft htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Propequisustento", TituloRow = "Sustento", Ancho = 200, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoTexto, NombreClase = "htLeft htMiddle" });
            if (!habilitarEdicion) listaCabecera.Add(new CabeceraRow() { NombreRow = "UltimaModificacionUsuarioDesc", TituloRow = "Usuario modificación", Ancho = 100, AlineacionHorizontal = "Izquierda", EsEditable = false, TipoDato = GridExcel.TipoTexto, NombreClase = "htCenter htMiddle" });
            if (!habilitarEdicion) listaCabecera.Add(new CabeceraRow() { NombreRow = "UltimaModificacionFechaDesc", TituloRow = "Fecha modificación", Ancho = 100, AlineacionHorizontal = "Izquierda", EsEditable = false, TipoDato = GridExcel.TipoTexto, NombreClase = "htCenter htMiddle" });
            listaCabecera.Add(new CabeceraRow() { NombreRow = "Equinomb", TituloRow = "", Ancho = 30, AlineacionHorizontal = "Izquierda", EsEditable = habilitarEdicion, TipoDato = GridExcel.TipoTexto, NombreClase = "htCenter htMiddle" });

            string[] headers = listaCabecera.Select(x => x.TituloRow).ToArray();
            List<int> widths = listaCabecera.Select(x => x.Ancho).ToList();
            object[] columnas = new object[headers.Length];

            var nestedHeader = new NestedHeaders();
            var headerRow1 = new List<CellNestedHeader>();

            for (int m = 0; m < headers.Length; m++)
            {
                var cabecera = listaCabecera[m];

                //personalizar cabecera
                string claseCss = "";
                string title = "";
                if (cabecera.NombreRow == "Propequicheckcero") { claseCss = "icono_check_cero"; title = "Seleccionar Valor cero(0) correcto"; }
                //if (cabecera.NombreRow == "Propequicheckcero") { }
                CellNestedHeader f1 = new CellNestedHeader() { Label = cabecera.TituloRow, Class = claseCss, Title = title };
                headerRow1.Add(f1);

                string claseCol = habilitarEdicion && !cabecera.EsEditable ? " htDisabled" : string.Empty;
                object dtpConfig = new
                {
                    firstDay = 1,
                    showWeekNumber = true,
                    i18n = new
                    {
                        previousMonth = "Mes anterior",
                        nextMonth = "Mes siguiente",
                        months = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" },
                        weekdays = new string[] { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" },
                        weekdaysShort = new string[] { "Dom", "Lun", "Mar", "Mier", "Jue", "Vie", "Sab" }
                    }
                };
                columnas[m] = new
                {
                    data = cabecera.NombreRow,
                    readOnly = !habilitarEdicion || !cabecera.EsEditable,
                    type = cabecera.TipoDato,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    dateFormat = "DD/MM/YYYY",
                    correctFormat = true,
                    defaultDate = string.Empty,
                    format = string.Empty,
                    className = cabecera.NombreClase + claseCol,
                    datePickerConfig = dtpConfig,
                    checkedTemplate = "1",
                    uncheckedTemplate = "0"
                };
            }
            nestedHeader.ListCellNestedHeaders.Add(headerRow1);

            //Body
            List<string[]> listaDataHandson = new List<string[]>();
            listaDespuesFicha8 = new List<int>(); //Propiedades cuya fecha fecha de vigencia sí está validada

            int nFil = listado.Count + 1;
            int nCol = habilitarEdicion ? 11 : 13;
            short[][] matrizTipoEstado = new short[nFil][];
            for (int i = 0; i < nFil; i++)
            {
                matrizTipoEstado[i] = new short[nCol];

                //por defecto todas las celdas son de solo lectura
                for (int j = 0; j < nCol; j++)
                {
                    matrizTipoEstado[i][j] = -1;
                }
            }

            var filaActual = 0;
            foreach (var prop in listado)
            {
                List<string> matriz = new List<string>();

                matriz.Add(prop.Propcodi.ToString()); //0
                matriz.Add(prop.Propnomb); //1
                matriz.Add(prop.Propnombficha); //2
                matriz.Add(prop.Propunidad); //3
                matriz.Add(prop.FechapropequiDesc); //4
                matriz.Add(prop.Valor); //5
                matriz.Add(prop.Propequicheckcero.ToString()); //6
                matriz.Add(prop.Propequicomentario); //7
                matriz.Add(string.Empty + "."); //8
                matriz.Add(prop.Propequisustento); //9
                if (!habilitarEdicion) matriz.Add(prop.UltimaModificacionUsuarioDesc);
                if (!habilitarEdicion) matriz.Add(prop.UltimaModificacionFechaDesc);
                matriz.Add(string.Empty + ".");

                listaDataHandson.Add(matriz.ToArray());

                //Validación si la data es correcta
                int esCorrecto = prop.FechaCambio > fechaEnProdFicha8 ? 1 : 0;
                listaDespuesFicha8.Add(esCorrecto);

                //celdas editables
                if (habilitarEdicion && !propiedadesSoloLectura.Contains(prop.Propcodi))
                {
                    matrizTipoEstado[filaActual][4] = 1;
                    matrizTipoEstado[filaActual][5] = 1;
                    matrizTipoEstado[filaActual][6] = 1;
                    matrizTipoEstado[filaActual][7] = 1;
                    matrizTipoEstado[filaActual][8] = 1;
                    matrizTipoEstado[filaActual][9] = 1;
                    matrizTipoEstado[filaActual][10] = 1;
                }

                filaActual++;
            }

            handson = new HandsonModel();
            handson.NestedHeader = nestedHeader;
            handson.ListaExcelData = listaDataHandson.ToArray();
            handson.Headers = headers;
            handson.ListaColWidth = widths;
            handson.Columnas = columnas;
            handson.MatrizTipoEstado = matrizTipoEstado;
        }

        /// <summary>
        /// formatear data de bd
        /// </summary>
        /// <param name="prop"></param>
        public static void FormatearPropequi(EqPropequiDTO prop)
        {
            prop.Propnomb = String.IsNullOrEmpty(prop.Propnomb) ? string.Empty : prop.Propnomb.Trim();
            prop.Propunidad = String.IsNullOrEmpty(prop.Propunidad) ? string.Empty : prop.Propunidad.Trim();

            prop.FechapropequiDesc = prop.Fechapropequi.Value.ToString(ConstantesAppServicio.FormatoFecha);

            prop.Valor = String.IsNullOrEmpty(prop.Valor) ? string.Empty : prop.Valor.Trim();
            prop.Propequiobservacion = String.IsNullOrEmpty(prop.Propequiobservacion) ? string.Empty : prop.Propequiobservacion.Trim();
            prop.Propequicomentario = String.IsNullOrEmpty(prop.Propequicomentario) ? string.Empty : prop.Propequicomentario.Trim();
            prop.Propequisustento = String.IsNullOrEmpty(prop.Propequisustento) ? string.Empty : prop.Propequisustento.Trim();

            prop.FechaCambio = prop.Propequifecmodificacion != null ? prop.Propequifecmodificacion.Value : prop.Propequifeccreacion ?? new DateTime(2000, 1, 1);
            prop.FechaCambioDesc = prop.FechaCambio.ToString(ConstantesAppServicio.FormatoFecha);
            prop.UltimaModificacionFechaDesc = prop.FechaCambio.ToString(ConstantesAppServicio.FormatoFechaFull2);
            prop.UltimaModificacionUsuarioDesc = prop.Propequifecmodificacion != null ? (prop.Propequiusumodificacion ?? "") : (prop.Propequiusucreacion ?? "");

            if (!string.IsNullOrEmpty(prop.Propequisustento))
                prop.EsSustentoConfidencial = prop.Propequisustento.ToUpper().Contains("DescargarSustentoConfidencial?".ToUpper());
        }

        /// <summary>
        /// Método para actualizar las propiedades del equipo en un handsonweb
        /// </summary>
        /// <param name="listaPropequi"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="iEquipo"></param>
        /// <param name="filtroFicha"></param>
        /// <param name="orden"></param>
        /// <param name="usuario"></param>
        public void ActualizarListaPropiedades(List<EqPropequiDTO> listaPropequi, DateTime fechaConsulta, int iEquipo, string filtroFicha, string orden, string usuario)
        {
            var oEquipo = FactorySic.GetEqEquipoRepository().GetById(iEquipo);

            //excluir filas de las propiedades de Auditoria
            List<int> propiedadesSoloLectura = ListaPropiedadAuditoria();
            listaPropequi = listaPropequi.Where(x => !propiedadesSoloLectura.Contains(x.Propcodi)).ToList();

            //Validar datos del Handsontable
            string columnComentario = "Comentario: ";
            string columnSustento = "Sustento: ";
            foreach (var filaExcel in listaPropequi)
            {
                filaExcel.Propequicheckcero = (filaExcel.Valor ?? string.Empty).Trim() == "0" ? (filaExcel.Propequicheckcero ?? 0) : 0;
                filaExcel.Propequicomentario = filaExcel.Propequicomentario != null ? filaExcel.Propequicomentario.Trim() : string.Empty;
                filaExcel.Propequisustento = filaExcel.Propequisustento != null ? filaExcel.Propequisustento.Trim() : string.Empty;

                //Validar comentario
                if (!string.IsNullOrEmpty(filaExcel.Propequicomentario))
                {
                    if (filaExcel.Propequicomentario.Length > 500)
                        throw new ArgumentException(columnComentario + "no puede tener más de 500 caracteres");
                }

                //Validar sustento
                if (!string.IsNullOrEmpty(filaExcel.Propequisustento))
                {
                    if (filaExcel.Propequisustento.Length > 400)
                        throw new ArgumentException(columnSustento + "no puede tener más de 400 caracteres, actualmente contiene " + filaExcel.Propequisustento.Length + " caracteres.");
                }
            }

            //Datos de bd
            List<EqPropequiDTO> listadoBD = this.ListarValoresVigentesPropiedades(fechaConsulta, iEquipo, oEquipo.Famcodi.Value, "", filtroFicha); //por defecto orden de Ficha tecnica
            List<EqPropequiDTO> listaPropHist = this.ListarValoresHistoricosPropiedadPorEquipo(iEquipo, ConstantesAppServicio.ParametroDefecto);

            //Validar datos del formulario
            List<EqPropequiDTO> listaValido = new List<EqPropequiDTO>();
            List<EqPropequiDTO> listaDarDeBaja = new List<EqPropequiDTO>();
            DateTime fechaRegistro = DateTime.Now;

            foreach (var regInput in listaPropequi)
            {
                var regBD = listadoBD.Find(x => x.Propcodi == regInput.Propcodi);

                //filas que tienen datos validos en el handson (suficiente que tenga fecha de vigencia, el valor puede ser inclusive vacío)
                if (regInput.FechapropequiDesc != null && regInput.FechapropequiDesc.Trim() != "")
                {
                    EqPropequiDTO obj = new EqPropequiDTO()
                    {
                        Propcodi = regInput.Propcodi,
                        Equicodi = iEquipo,
                        Fechapropequi = DateTime.ParseExact(regInput.FechapropequiDesc, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Valor = regInput.Valor != null ? regInput.Valor.Trim() : string.Empty,
                        Propequicheckcero = (regInput.Valor ?? string.Empty).Trim() == "0" ? (regInput.Propequicheckcero ?? 0) : 0,
                        //Propequicheckcero = regInput.Propequicheckcero ?? 0,
                        Propequicomentario = regInput.Propequicomentario != null ? regInput.Propequicomentario.Trim() : string.Empty,
                        Propequisustento = regInput.Propequisustento != null ? regInput.Propequisustento.Trim() : string.Empty,
                        //Propequiobservacion = regInput.Propequiobservacion != null ? regInput.Propequiobservacion.Trim() : string.Empty,
                        Propequiusucreacion = usuario,
                        Propequifeccreacion = fechaRegistro,
                    };

                    listaValido.Add(obj);
                }
                else // filas que el usuario borró (el usuario quitó la fecha de vigencia)
                {
                    if (regBD != null)
                        listaDarDeBaja.Add(regBD);
                }
            }

            //Definir cuales son nuevos, updates (solo cambia check cero, comentario u observación) o eliminados (pasan a histórico)
            List<EqPropequiDTO> listaNew = new List<EqPropequiDTO>();
            List<EqPropequiDTO> listaUpdate = new List<EqPropequiDTO>();
            foreach (var regValido in listaValido)
            {
                // Listar histórico de la fecha de vigencia
                List<EqPropequiDTO> listaHistXProp = listaPropHist.Where(x => x.Propcodi == regValido.Propcodi && x.Fechapropequi == regValido.Fechapropequi).ToList();
                EqPropequiDTO regActivoHist = listaHistXProp.Find(x => x.Propequideleted == 0); // obtiene el registro vigente visible de ese día
                if (regActivoHist != null)
                {
                    // solo si se cambia el valor se considera que hay diferencia y se genera un nuevo registro 
                    if (regValido.Valor != regActivoHist.Valor)
                    {
                        EqPropequiDTO regActivo = (EqPropequiDTO)regActivoHist.Clone();
                        regActivo.Valor = regValido.Valor;
                        regActivo.Propequicheckcero = regValido.Propequicheckcero;
                        regActivo.Propequicomentario = regValido.Propequicomentario;
                        regActivo.Propequisustento = regValido.Propequisustento;
                        regActivo.Propequideleted = 0;
                        regActivo.Propequideleted2 = 0;
                        regActivo.Propequifecmodificacion = regValido.Propequifeccreacion;
                        regActivo.Propequiusumodificacion = regValido.Propequiusucreacion;
                        listaUpdate.Add(regActivo);

                        regActivoHist.Propequideleted = listaHistXProp.Max(x => x.Propequideleted) + 1;
                        listaNew.Add(regActivoHist);
                    }
                    else
                    {
                        //si hay cambios en columnas opcionales solo hacer update al registro visible
                        if (regValido.Propequicheckcero != regActivoHist.Propequicheckcero 
                            || regValido.Propequicomentario != regActivoHist.Propequicomentario 
                            || regValido.Propequisustento != regActivoHist.Propequisustento)
                        {
                            EqPropequiDTO regActivo = (EqPropequiDTO)regActivoHist.Clone();
                            regActivo.Propequicheckcero = regValido.Propequicheckcero;
                            regActivo.Propequicomentario = regValido.Propequicomentario;
                            regActivo.Propequisustento = regValido.Propequisustento;
                            regActivo.Propequideleted2 = regActivo.Propequideleted;
                            listaUpdate.Add(regActivo);
                        }
                    }
                }
                else
                {
                    //si no hay algun historico para ese día se considera un nuevo registro
                    regValido.Propequideleted = 0;
                    listaNew.Add(regValido);
                }
            }

            //crear registro para guardar historico de dato
            foreach (var reg in listaDarDeBaja)
            {
                // histórico del día específico
                List<EqPropequiDTO> listaHistXProp = listaPropHist.Where(x => x.Propcodi == reg.Propcodi && x.Fechapropequi == reg.Fechapropequi).ToList(); 

                EqPropequiDTO regActivoHist = listaHistXProp.Find(x => x.Propequideleted == 0);
                if (regActivoHist != null)
                {
                    regActivoHist.Propequifecmodificacion = fechaRegistro;
                    regActivoHist.Propequiusumodificacion = usuario;
                    regActivoHist.Propequideleted = 0;
                    regActivoHist.Propequideleted2 = listaHistXProp.Max(x => x.Propequideleted) + 1;
                    listaUpdate.Add(regActivoHist);
                }
            }

            //persistir en BD
            foreach (var reg in listaNew)
                this.SaveEqPropequiSinCorrelativo(reg);
            foreach (var reg in listaUpdate)
                this.UpdateEqPropequi(reg);
        }

        private List<int> ListaPropiedadesAplicativos()
        {
            List<int> listaProp = new List<int>();

            //Generador hidraulico 2
            listaProp.AddRange(new List<int>() { 164, 299, 298, 308, 953, 954, 967, 1069, 1530 });
            //Central hidraulica 4
            listaProp.AddRange(new List<int>() { 42, 46, 932, 941, 943, 1483 });

            //Generador termico 3
            listaProp.AddRange(new List<int>() { 49, 1070, 982, 189, 188, 1563, 645, 646, 319, 1070 });
            //Central termica 5
            listaProp.AddRange(new List<int>() { 53, 942, 944, 1516, 1517, 1816, 1563 });

            //GENERADOR SOLAR 36
            listaProp.AddRange(new List<int>() { });
            //CENTRAL SOLAR 37
            listaProp.AddRange(new List<int>() { 1710, 1709 });

            //GENERADOR EOLICO 38
            listaProp.AddRange(new List<int>() { });
            //CENTRAL EOLICA 39
            listaProp.AddRange(new List<int>() { 1602, 1601 });

            //barra 7
            listaProp.AddRange(new List<int>() { 1484 });

            //linea 8
            listaProp.AddRange(new List<int>() { 1068 });

            //trafos 9 10, svc 14, carga 35
            listaProp.AddRange(new List<int>() { 1076, 1077, 1078, 1071 });

            //operacion comercial
            listaProp.AddRange(new List<int>() { ConstantesAppServicio.PropiedadOperacionComercial });

            return listaProp;
        }

        /// <summary>
        /// Lista de propiedades que guardan el historico de los cambios de los campos generales de un equipo
        /// </summary>
        /// <returns></returns>
        public List<int> ListaPropiedadAuditoria()
        {
            List<int> listaProp = new List<int>();

            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEquinomb);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEquiabrev);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEquiabrev2);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaFamcodi);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaAreacodi);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEcodigo);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEquipadre);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEquitension);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEquimaniobra);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaOsinergcodi);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaOperadoremprcodi);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaOsinergcodigen);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaGrupocodi);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEstado);
            listaProp.Add(ConstantesAppServicio.PropiedadAuditoriaEmpresa);

            return listaProp;
        }

        /// <summary>
        /// Método para registrar las propiedades históricas al momento de Crear un equipo
        /// </summary>
        /// <param name="equipoCreado"></param>
        /// <param name="usuario"></param>
        public void RegistrarHistoricoCreacionEquipo(EqEquipoDTO equipoCreado, string usuario)
        {
            //registros a guardar
            var listaNuevo = ListarPropiedadesAuditoriaDeEquipo(equipoCreado, usuario);

            //guardar BD
            foreach (var propiedadEstado in listaNuevo)
            {
                SaveEqPropequi(propiedadEstado);
            }
        }

        private List<EqPropequiDTO> ListarPropiedadesAuditoriaDeEquipo(EqEquipoDTO equipoCreado, string usuario)
        {
            List<EqPropequiDTO> listaAuditoria = new List<EqPropequiDTO>() { };
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEquinomb, Valor = (equipoCreado.Equinomb ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEquiabrev, Valor = (equipoCreado.Equiabrev ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEquiabrev2, Valor = (equipoCreado.Equiabrev2 ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEmpresa, Valor = (equipoCreado.Emprcodi == null ? "" : equipoCreado.Emprcodi.ToString()) });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaFamcodi, Valor = (equipoCreado.Famcodi == null ? "" : equipoCreado.Famcodi.ToString()) });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaAreacodi, Valor = (equipoCreado.Areacodi == null ? "" : equipoCreado.Areacodi.ToString()) });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEcodigo, Valor = (equipoCreado.Ecodigo ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEquipadre, Valor = (equipoCreado.Equipadre == null ? "" : equipoCreado.Equipadre.ToString()) });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEquitension, Valor = (equipoCreado.Equitension == null ? "" : equipoCreado.Equitension.ToString()) });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEquimaniobra, Valor = (equipoCreado.EquiManiobra ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaOsinergcodi, Valor = (equipoCreado.Osinergcodi ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaEstado, Valor = (equipoCreado.Equiestado ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaOperadoremprcodi, Valor = equipoCreado.Operadoremprcodi.ToString() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaOsinergcodigen, Valor = (equipoCreado.OsinergcodiGen ?? "").Trim() });
            listaAuditoria.Add(new EqPropequiDTO() { Propcodi = ConstantesAppServicio.PropiedadAuditoriaGrupocodi, Valor = (equipoCreado.Grupocodi == null ? "" : equipoCreado.Grupocodi.ToString()) });

            foreach (var propiedadEstado in listaAuditoria)
            {
                propiedadEstado.Equicodi = equipoCreado.Equicodi;
                propiedadEstado.Fechapropequi = DateTime.Today;
                propiedadEstado.Propequifeccreacion = DateTime.Now;
                propiedadEstado.Propequiusucreacion = usuario;

            }

            return listaAuditoria;
        }

        /// <summary>
        /// Método para registrar cambios de las propiedades históricas al momento de Editar un equipo
        /// </summary>
        /// <param name="equipoOriginal"></param>
        /// <param name="equipoEditado"></param>
        /// <param name="usuario"></param>
        public void RegistrarHistoricoEdicionEquipo(EqEquipoDTO equipoOriginal, EqEquipoDTO equipoEditado, string usuario)
        {
            var listaAntes = ListarPropiedadesAuditoriaDeEquipo(equipoOriginal, usuario);
            var listaDespues = ListarPropiedadesAuditoriaDeEquipo(equipoEditado, usuario);

            List<EqPropequiDTO> listaUpdate = new List<EqPropequiDTO>();

            //registros con cambios
            foreach (var itemDespues in listaDespues)
            {
                var itemAntes = listaAntes.Find(x => x.Propcodi == itemDespues.Propcodi);
                if (itemAntes != null && itemAntes.Valor.ToUpper() != itemDespues.Valor.ToUpper())
                {
                    listaUpdate.Add(itemDespues);
                }
            }

            //guardar BD
            foreach (var propiedadEstado in listaUpdate)
            {
                SaveEqPropequi(propiedadEstado);
            }
        }

        /// <summary>
        /// Listar propiedades con valores vigentes de los campos de un equipo. 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<EqPropequiDTO> ListarPropiedadAuditoriaVigente(int equicodi)
        {
            //Obtener propiedades de auditoria
            string propiedadesSoloLectura = string.Join(",", ListaPropiedadAuditoria());
            List<EqPropiedadDTO> listaProp = FactorySic.GetEqPropiedadRepository().ListByIds(propiedadesSoloLectura);

            //Datos vigentes
            var fechaConsulta = DateTime.Today;
            var listado = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(fechaConsulta, equicodi.ToString(), "-1", "-1", propiedadesSoloLectura, string.Empty, "-1");
            foreach (var reg in listado)
                FormatearPropequi(reg);

            //dar formato a las filas para reporte web y excel
            foreach (var reg in listaProp)
            {
                var propexist = listado.Find(x => x.Propcodi == reg.Propcodi);
                if (propexist == null)
                {
                    listado.Add(new EqPropequiDTO()
                    {
                        Propcodi = reg.Propcodi,
                        Equicodi = equicodi,
                        Famcodi = reg.Famcodi,
                        Propunidad = reg.Propunidad,
                        Propnomb = reg.Propnomb,
                        Propnombficha = reg.Propnombficha ?? "",
                        FechapropequiDesc = string.Empty,
                        Valor = string.Empty,
                        Propequicheckcero = 0,
                        Propequicomentario = string.Empty,
                        Propequisustento = string.Empty,
                        //Propequiobservacion = string.Empty,
                        UltimaModificacionFechaDesc = string.Empty,
                        UltimaModificacionUsuarioDesc = string.Empty,
                        Orden = reg.Orden,
                        Propfichaoficial = reg.Propfichaoficial,
                    });
                }
                else
                {
                    propexist.Famcodi = reg.Famcodi;
                    propexist.Orden = reg.Orden;
                    propexist.Propfichaoficial = reg.Propfichaoficial;
                    propexist.Propnombficha = reg.Propnombficha ?? "";
                }
            }

            return listado.OrderBy(x => x.Orden).ToList();
        }

        #endregion

        #region Equipos sin datos de ficha técnica

        /// <summary>
        /// Mètodo que retorna la lista de equipos con cantidad de propiedades vacias
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="idFamilia"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EqPropequiDTO> ListarEquiposPropiedadesSinValor(int tipoEmprcodi, int emprcodi, int idFamilia, string equiestado, DateTime fechaini, DateTime fechaFin)
        {
            DateTime dtFechaInicio = new DateTime(1900, 1, 1);
            DateTime dtFechaFin = DateTime.Now;

            //listaEquipos
            var listEquipos = this.ListaEqEmprFamilia(emprcodi, idFamilia);

            if (tipoEmprcodi != -1)
                listEquipos = listEquipos.Where(x => x.Tipoemprcodi == tipoEmprcodi).ToList();

            listEquipos = equiestado == ConstantesAppServicio.ParametroDefecto ? listEquipos : listEquipos.Where(x => x.Equiestado == equiestado).ToList();

            //Lista de propiedades
            List<EqPropiedadDTO> listaProp = this.ListEqPropiedads().Where(x => x.Propfichaoficial == ConstantesAppServicio.SI).ToList();

            //Lista Valor de las propiedades
            List<EqPropequiDTO> listaPropVal = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(dtFechaFin, "-1", idFamilia.ToString(), emprcodi.ToString(), "-1", string.Empty, "S");
            listaPropVal = equiestado == ConstantesAppServicio.ParametroDefecto ? listaPropVal : listaPropVal.Where(x => x.Equiestado == equiestado).ToList();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //solo equipos que tienen famcodi en propiedades validas
            listEquipos = listEquipos.Where(x => listaProp.Any(y => y.Famcodi == x.Famcodi)).ToList();

            //lista de Tipo de equipos
            List<int> listaFamcodi = listEquipos.Select(x => x.Famcodi ?? 0).Distinct().ToList();

            //
            List<EqPropequiDTO> listEqPropSinValor = new List<EqPropequiDTO>();
            foreach (var famcodi in listaFamcodi)
            {
                //Propiedades válidas del equipo
                List<EqPropiedadDTO> listPropXFamcodi = listaProp.Where(x => x.Famcodi == famcodi).ToList(); //Lista de propiedades válidas que deberia tener el equipo

                List<EqPropequiDTO> listaPropValXFamcodi = listaPropVal.Where(x => x.Famcodi == famcodi).ToList();

                List<EqEquipoDTO> listaEquicodiXFamcodi = listEquipos.Where(x => x.Famcodi == famcodi).ToList();

                foreach (var eq in listaEquicodiXFamcodi)
                {
                    var listaPropValorEquipo = listaPropValXFamcodi.Where(x => x.Equicodi == eq.Equicodi).ToList(); //lista de valores de "TODAS" las propiedades del equipo repetidas incluso
                    var listaFinalEqPropValidate = listaPropValorEquipo.Where(x => listPropXFamcodi.Any(y => y.Propcodi == x.Propcodi)).ToList(); //filtro para solo quedarse con las propiedades con las válidas

                    if (listaFinalEqPropValidate.Count == 0)
                    {
                        EqPropequiDTO objEqPropVacias = new EqPropequiDTO()
                        {
                            Equicodi = eq.Equicodi,
                            Equinomb = eq.Equinomb,
                            Equiabrev = !string.IsNullOrEmpty(eq.Equiabrev) ? eq.Equiabrev : eq.Equinomb,
                            Emprcodi = eq.Emprcodi ?? 0,
                            Emprnomb = eq.Emprnomb,
                            Famcodi = eq.Famcodi ?? 0,
                            Famnomb = eq.Famnomb,
                            Areadesc = eq.Areadesc,
                            Osigrupocodi = EquipamientoHelper.EstiloEstado(eq.Equiestado),
                            Equiestado = eq.Equiestado.Trim(),
                            Propsinvacio = listPropXFamcodi.Count,
                            Proptotal = listPropXFamcodi.Count,
                            PropVaciasCount = listPropXFamcodi.Count.ToString("D3") + " / " + listPropXFamcodi.Count.ToString("D3")
                        };
                        listEqPropSinValor.Add(objEqPropVacias);
                        continue;
                    }

                    ///////////////
                    int contador = 0;
                    foreach (var prop in listaFinalEqPropValidate)
                    {
                        if (prop.Valor == null || prop.Valor == string.Empty)
                        {
                            contador++;
                        }
                        else
                        {
                            string tipoProp = listPropXFamcodi.Find(x => x.Propcodi == prop.Propcodi).Proptipo;
                            tipoProp = tipoProp != null ? tipoProp.Trim().ToUpper() : tipoProp;

                            int cant = 0;
                            switch (tipoProp)
                            {
                                case ConstantesAppServicio.TipoDecimal:
                                case ConstantesAppServicio.TipoNumerico:
                                case ConstantesAppServicio.TipoEntero:
                                    double num = 0;
                                    bool result = double.TryParse(prop.Valor, out num);

                                    cant = ConstantesAppServicio.ListValPropDecimal.Any(x => x == prop.Valor) ? 1 : result == false ? 1 : 0;
                                    contador = contador + cant;
                                    break;
                                case ConstantesAppServicio.TipoArchivo:
                                    cant = ConstantesAppServicio.ListValPropFile.Any(x => x == prop.Valor) ? 1 : 0;
                                    contador = contador + cant;
                                    break;
                                case ConstantesAppServicio.TipoN:
                                case ConstantesAppServicio.TipoString:
                                    cant = ConstantesAppServicio.ListValPropN.Any(x => x == prop.Valor) ? 1 : 0;
                                    contador = contador + cant;
                                    break;
                                default:
                                    cant = ConstantesAppServicio.ListValPropString.Any(x => x == prop.Valor) ? 1 : 0;
                                    contador = contador + cant;
                                    break;
                            }
                        }
                    }

                    // A la lista de todas las propiedades validas del equipo  se le quita las propiedades que tenian valor para quedarse con las que no tienen registro
                    var listaPropNoestan = listPropXFamcodi.Where(x => !listaFinalEqPropValidate.Any(y => y.Propcodi == x.Propcodi)).ToList();
                    int faltantes = listaPropNoestan.Count > 0 ? listaPropNoestan.Count : 0;
                    contador = contador + faltantes;

                    if (contador > 0)
                    {
                        EqPropequiDTO objEquipoProp = listaFinalEqPropValidate.First();
                        objEquipoProp.Equiabrev = !string.IsNullOrEmpty(objEquipoProp.Equiabrev) ? objEquipoProp.Equiabrev : objEquipoProp.Equinomb;
                        objEquipoProp.Osigrupocodi = EquipamientoHelper.EstiloEstado(objEquipoProp.Equiestado);
                        objEquipoProp.Propsinvacio = contador;
                        objEquipoProp.Proptotal = listPropXFamcodi.Count;
                        objEquipoProp.PropVaciasCount = contador.ToString("D3") + " / " + listPropXFamcodi.Count.ToString("D3");
                        listEqPropSinValor.Add(objEquipoProp);
                    }
                }
            }

            listEqPropSinValor = listEqPropSinValor.OrderBy(x => x.Emprnomb).ThenBy(x => x.Areadesc).ThenBy(y => y.Famnomb).ThenBy(z => z.Equinomb).ToList();

            return listEqPropSinValor;
        }

        /// <summary>
        /// Método que retorna el listado de las propiedades válidas con sus valores para el popup
        /// </summary>
        /// <param name="emprCodi"></param>
        /// <param name="equiCodi"></param>
        /// <param name="famCodi"></param>
        /// <returns></returns>
        public List<EqPropiedadDTO> ListaPropiedadesValidas(int emprCodi, int equiCodi, int famCodi)
        {
            DateTime dtFechaInicio = new DateTime(1900, 1, 1);
            DateTime dtFechaFin = DateTime.Now;

            //Lista de propiedades
            var listaProp = this.ListEqPropiedads().Where(x => x.Propfichaoficial == ConstantesAppServicio.SI).ToList();

            //Lista Valor de las propiedades
            var listaPropVal = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(dtFechaFin, "-1", famCodi.ToString(), emprCodi.ToString(), "-1", string.Empty, "S");

            //Propiedades válidas del equipo
            var listPropEquipo = listaProp.Where(x => x.Famcodi == famCodi).ToList(); //Lista de propiedades válidas que deberia tener el equipo

            var listaPropValorEquipo = listaPropVal.Where(x => x.Equicodi == equiCodi).ToList(); //lista de valores de "TODAS" las propiedades del equipo repetidas incluso
            var listaFinalEqPropValidate = listaPropValorEquipo.Where(x => listPropEquipo.Any(y => y.Propcodi == x.Propcodi)).ToList(); //filtro para solo quedarse con las propiedades con las válidas

            if (listaFinalEqPropValidate.Count() == 0)
            {
                listPropEquipo.ForEach(x => x.Valor = string.Empty);
                return listPropEquipo;
            }
            else
            {
                foreach (var item in listPropEquipo)
                {
                    var Propiedad = listaFinalEqPropValidate.Find(x => x.Propcodi == item.Propcodi);
                    if (Propiedad != null)
                    {
                        item.Valor = Propiedad.Valor;
                    }
                    else
                        item.Valor = string.Empty;
                }
                return listPropEquipo;
            }
        }

        /// <summary>
        /// Devuelve lista de familia xEmpresa
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarFamiliaXEmp(int idEmpresa)
        {
            List<EqPropequiDTO> listEqPropSinValor = new List<EqPropequiDTO>();
            //Lista de propiedades
            var listaProp = this.ListEqPropiedads().Where(x => x.Propfichaoficial == ConstantesAppServicio.SI).ToList();

            var famCodisPropiedades = listaProp.GroupBy(x => x.Famcodi).Select(g => g.Key).ToList();

            var listafamilias = new List<EqFamiliaDTO>();
            if (idEmpresa <= 0)
                listafamilias = this.ListEqFamilias().OrderBy(x => x.Famnomb).ToList();
            else
                listafamilias = FactorySic.GetEqFamiliaRepository().ListarFamiliaXEmp(idEmpresa);

            listafamilias = listafamilias.Where(x => famCodisPropiedades.Contains(x.Famcodi)).OrderBy(x => x.Famnomb).ToList();

            return listafamilias;
        }

        /// <summary>
        /// Devuelve lista de Empresas del Sein en Ficha Técnica
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasSeinFT()
        {
            //empresas del SEIN
            var empresasSEIN = FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();

            //lista de todos los Equipos
            var listEquipos = this.ListaEqEmprFamilia(-1, -1);

            //Lista de propiedades           
            var listaProp = this.ListEqPropiedads().Where(x => x.Propfichaoficial == ConstantesAppServicio.SI).ToList();
            var famCodisPropiedades = listaProp.GroupBy(x => x.Famcodi).Select(g => g.Key).ToList();

            //filtro para solo quedarse con los equipos validos con Famcodi en ficha técnica
            listEquipos = listEquipos.Where(x => famCodisPropiedades.Contains(x.Famcodi ?? 0)).ToList();

            //filtro para solo quedarse con las empresas que tengan equipos con tipo equipo en ficha tecnica
            var listaFinalEmpresas = empresasSEIN.Where(x => listEquipos.Any(y => y.Emprcodi == x.Emprcodi)).ToList();

            return listaFinalEmpresas;
        }

        #endregion

        #region Ingreso / Retiro Operación Comercial

        /// <summary>
        /// Método que lista todas las unidades de generación (y central) que tiene operación comercial
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="strfamcodis"></param>
        /// <param name="esAplicarTTIE"></param>
        /// <param name="flagSoloDatVigente"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquiposTienenOpComercial(DateTime fechaIni, DateTime fechaFin, string strfamcodis,
                                    out List<ResultadoValidacionAplicativo> listaMsj, bool esAplicarTTIE = true, bool flagSoloDatVigente = false)
        {
            List<EqEquipoDTO> listaSiOpComercial = new List<EqEquipoDTO>();
            List<EqEquipoDTO> listaGenOpComercial = new List<EqEquipoDTO>();
            List<EqEquipoDTO> listaCentralOpComercial = new List<EqEquipoDTO>();

            listaMsj = new List<ResultadoValidacionAplicativo>();

            //filtrar central y generador
            string famcodisQuery = ConstantesHorasOperacion.CodFamilias + "," + ConstantesHorasOperacion.CodFamiliasGeneradores;
            if (strfamcodis != ConstantesAppServicio.ParametroDefecto)
            {
                List<int> lFamcodiTmp = new List<int>();
                var famcodisTmp = strfamcodis.Split(',').Select(x => int.Parse(x)).ToList();
                foreach (var famcodi in famcodisTmp)
                {
                    if (famcodi == ConstantesHorasOperacion.IdTipoTermica || famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico)
                    {
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdTipoTermica);
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                    }

                    if (famcodi == ConstantesHorasOperacion.IdTipoHidraulica || famcodi == ConstantesHorasOperacion.IdGeneradorHidroelectrico)
                    {
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdTipoHidraulica);
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdGeneradorHidroelectrico);
                    }

                    if (famcodi == ConstantesHorasOperacion.IdTipoEolica || famcodi == ConstantesHorasOperacion.IdGeneradorEolica)
                    {
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdTipoEolica);
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdGeneradorEolica);
                    }

                    if (famcodi == ConstantesHorasOperacion.IdTipoSolar || famcodi == ConstantesHorasOperacion.IdGeneradorSolar)
                    {
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdTipoSolar);
                        lFamcodiTmp.Add(ConstantesHorasOperacion.IdGeneradorSolar);
                    }
                }

                famcodisQuery = string.Join(",", lFamcodiTmp);
            }

            //consultar equipos
            List<EqEquipoDTO> listaEqBD = FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(famcodisQuery);
            listaEqBD = listaEqBD.Where(x => x.Equiestado != ConstantesAppServicio.Eliminado && x.Equiestado != ConstantesAppServicio.Anulado).ToList();

            #region Aplicar TTIE para quitar equipos duplicados de la BD COES

            if (esAplicarTTIE)
            {
                TitularidadAppServicio servTitEmp = new TitularidadAppServicio();

                //Consulta el histórico de relación entre los equipos y las empresas
                List<SiHisempeqDataDTO> listaHist = servTitEmp.ListSiHisempeqDatas("-1").Where(x => x.Heqdatfecha <= fechaFin).ToList();

                listaEqBD = servTitEmp.SetTTIEequipoToEqEquipo(listaEqBD, listaHist);
            }

            #endregion

            //Operación comercial
            List<EqPropequiDTO> listaOperacionComercial = ListarEqPropequiXPropiedad(ConstantesAppServicio.PropiedadOperacionComercial.ToString(), famcodisQuery, flagSoloDatVigente, fechaIni);

            int[] famcodisGeneradores = ConstantesHorasOperacion.CodFamiliasGeneradores.Split(',').Select(x => int.Parse(x)).ToArray();
            var listaEqBDGen = listaEqBD.Where(x => famcodisGeneradores.Contains(x.Famcodi ?? 0)).ToList();
            foreach (var regEq in listaEqBDGen)
            {
                if (regEq.Equicodi == 21299)
                { }
                SetValorOperacionComercial(regEq.Equicodi, fechaIni, fechaFin, listaOperacionComercial, out string opComercial, out DateTime? fechaInicio, out DateTime? fechaRetiro);
                if (ConstantesAppServicio.SI == opComercial)
                {
                    regEq.Equifechiniopcom = fechaInicio;
                    regEq.Equifechfinopcom = fechaRetiro;
                    regEq.TieneNuevoIngresoOpComercial = fechaIni <= fechaInicio && fechaInicio <= fechaFin;
                    regEq.TieneNuevoRetiroOpComercial = fechaIni <= fechaRetiro && fechaRetiro <= fechaFin;
                    listaGenOpComercial.Add(regEq);
                }
            }

            List<int> listaEquipadre = listaGenOpComercial.Where(x => x.Equipadre > 0).Select(x => x.Equipadre.Value).ToList();
            listaCentralOpComercial = listaEqBD.Where(x => listaEquipadre.Contains(x.Equicodi)).ToList();
            foreach (var regCentral in listaCentralOpComercial)
            {
                SetValorOperacionComercial(regCentral.Equicodi, fechaIni, fechaFin, listaOperacionComercial, out string opComercial, out DateTime? fechaInicio, out DateTime? fechaRetiro);
                regCentral.Equifechiniopcom = fechaInicio;
                regCentral.Equifechfinopcom = fechaRetiro;
                regCentral.TieneNuevoIngresoOpComercial = fechaIni <= fechaInicio && fechaInicio <= fechaFin;
                regCentral.TieneNuevoRetiroOpComercial = fechaIni <= fechaRetiro && fechaRetiro <= fechaFin;

                if (ConstantesAppServicio.SI != opComercial)
                {
                    listaMsj.Add(new ResultadoValidacionAplicativo() { TipoFuenteDatoDesc = "Equipo", Descripcion = "[" + regCentral.Equicodi + "," + regCentral.Equinomb + "] no tiene datos de operación comercial." });
                }
            }

            //
            listaSiOpComercial.AddRange(listaGenOpComercial);
            listaSiOpComercial.AddRange(listaCentralOpComercial);

            //formatear data
            foreach (var reg in listaSiOpComercial)
            {
                if (famcodisGeneradores.Contains(reg.Famcodi ?? 0))
                {
                    var regPadre = listaSiOpComercial.Find(x => x.Equicodi == reg.Equipadre);
                    if (regPadre != null)
                    {
                        reg.Central = regPadre.Equinomb;
                        reg.Equipadre = regPadre.Equicodi;
                    }
                    else
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo() { TipoFuenteDatoDesc = "Equipo", Descripcion = "[" + reg.Equicodi + "," + reg.Equinomb + "] no tiene asociado una central." });
                    }
                }
                else
                {
                    reg.Central = reg.Equinomb;
                    reg.Equipadre = reg.Equicodi;
                }

                reg.Emprnomb = (reg.Emprnomb ?? "").Trim();
                reg.Equiabrev = !string.IsNullOrEmpty(reg.Equiabrev) ? reg.Equiabrev.Trim() : string.Empty;
                reg.Equinomb = !string.IsNullOrEmpty(reg.Equinomb) ? reg.Equinomb.Trim() : string.Empty;
                reg.Central = !string.IsNullOrEmpty(reg.Central) ? reg.Central.Trim() : string.Empty;
                reg.TipogenerrerDesc = reg.Tipogenerrer == ConstantesAppServicio.SI ? ConstantesAppServicio.SIDesc : string.Empty;
                reg.GrupotipocogenDesc = reg.Grupotipocogen == ConstantesAppServicio.SI ? ConstantesAppServicio.SIDesc : string.Empty;
            }

            //filtrar para salida
            var lFamcodisInput = strfamcodis.Split(',').Select(x => int.Parse(x)).ToList();
            listaSiOpComercial = listaSiOpComercial.Where(x => lFamcodisInput.Contains(x.Famcodi ?? 0)).ToList();

            return listaSiOpComercial;
        }

        private List<EqPropequiDTO> ListarEqPropequiXPropiedad(string propcodis, string famcodi, bool flagSoloDatVigente, DateTime fechaVigencia)
        {
            if (flagSoloDatVigente)
            {
                return FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedades(fechaVigencia, "-1", famcodi, "-1", propcodis, string.Empty, "-1");
            }
            else
            {
                return FactorySic.GetEqPropequiRepository().ListarValoresHistoricosPropiedadPorEquipo(-1, propcodis)
                                                           .Where(x => x.Propequideleted == 0).OrderByDescending(x => x.Fechapropequi).ToList();
            }
        }

        /// <summary>
        /// Establecer el valor de Operacion comercial
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaOperacionComercial"></param>
        /// <returns></returns>
        public static void SetValorOperacionComercial(int equicodi, DateTime fechaIni, DateTime fechaFin, List<EqPropequiDTO> listaOperacionComercial
                                                        , out string opComercial, out DateTime? fechaInicio, out DateTime? fechaRetiro)
        {
            opComercial = ConstantesAppServicio.NO;
            fechaInicio = null;
            fechaRetiro = null;

            var regPrimerDia = listaOperacionComercial.Find(x => x.Fechapropequi <= fechaIni && x.Equicodi == equicodi);
            var regExisteOpEnMes = listaOperacionComercial.Find(x => x.Valor == ConstantesAppServicio.SI && fechaIni < x.Fechapropequi && x.Fechapropequi <= fechaFin && x.Equicodi == equicodi);
            var regRetiroOpEnMes = listaOperacionComercial.Find(x => x.Valor == ConstantesAppServicio.NO && fechaIni <= x.Fechapropequi && x.Fechapropequi <= fechaFin.AddDays(1) && x.Equicodi == equicodi);

            //verificar si en el primer dia del mes tiene o no Op comercial
            if (regPrimerDia != null && regPrimerDia.Valor == ConstantesAppServicio.SI)
            {
                opComercial = ConstantesAppServicio.SI;
                fechaInicio = regPrimerDia.Fechapropequi;
            }

            //si en el mes inicia operacion comercial
            if (regExisteOpEnMes != null)
            {
                opComercial = ConstantesAppServicio.SI;
                fechaInicio = regExisteOpEnMes.Fechapropequi;
            }

            if (regRetiroOpEnMes != null)
                fechaRetiro = regRetiroOpEnMes.Fechapropequi;
        }

        #endregion

        #region Relación de equipos EQ_EQUIREL

        /// <summary>
        /// Validar y Guardar Relacion de Equipo
        /// </summary>
        /// <param name="reg"></param>
        public void RegistrarRelacionEquipo(EqEquirelDTO reg)
        {
            int tiporelcodi = reg.Tiporelcodi;
            EqTiporelDTO oTipoRel = this.GetByIdEqTiporel(tiporelcodi);

            //Validar existencia de relacion 
            List<EqFamrelDTO> lsFamRel = this.ListarFamRelPorTipoRelEstado(tiporelcodi, " ");
            EqFamrelDTO regFam = lsFamRel.Find(x => x.Famcodi1 == reg.Famcodi1 && x.Famcodi2 == reg.Famcodi2);

            if (regFam != null)
            {
                EqEquipoDTO eq1 = this.GetByIdEqEquipo(reg.Equicodi1);
                EqEquipoDTO eq2 = this.GetByIdEqEquipo(reg.Equicodi2);

                if (eq1.Famcodi != regFam.Famcodi1)
                {
                    throw new Exception("El equipo " + eq1.Equinomb + " no es un " + regFam.Famnomb1);
                }
                if (eq2.Famcodi != regFam.Famcodi2)
                {
                    throw new Exception("El equipo " + eq2.Equinomb + " no es un " + regFam.Famnomb2);
                }

                //Guardar
                reg.Equirelfecmodificacion = DateTime.Now;
                this.SaveEqEquiRelDTO(reg);
            }
            else
            {
                throw new Exception("No existe relación de Tipo de equipos definido para el registro a guardar.");
            }
        }

        /// <summary>
        /// Generar reporte Html de la relacion de equipos
        /// </summary>
        /// <param name="tiporelcodi"></param>
        /// <param name="famcodi1"></param>
        /// <param name="famcodi2"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarReporteEquipoRel(int tiporelcodi, int famcodi1, int famcodi2, string url)
        {
            EqTiporelDTO oTipoRel = this.GetByIdEqTiporel(tiporelcodi);

            List<EqEquirelDTO> listaData = this.GetByCriteriaEqEquirels(tiporelcodi);
            if (famcodi1 > 0 && famcodi2 > 0) listaData = listaData.Where(x => x.Famcodi1 == famcodi1 && x.Famcodi2 == famcodi2).ToList();

            StringBuilder str = new StringBuilder();

            //
            str.Append("<table id='tablaReporte' class='pretty tabla-icono ' >");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='' rowspan=2>Acción</th>");
            str.AppendFormat("<th style='' colspan=3>Tipo de equipo 1</th>");
            str.AppendFormat("<th style='' colspan=3>Tipo de equipo 2</th>");
            str.Append("<th style='' rowspan=2>Usuario Actualización</th>");
            str.Append("<th style='' rowspan=2>Fecha Actualización</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<th style=''>T.Eq</th>");
            str.Append("<th style=''>Empresa</th>");
            str.Append("<th style=''>Equipo</th>");

            str.Append("<th style=''>T.Eq</th>");
            str.Append("<th style=''>Empresa</th>");
            str.Append("<th style=''>Equipo</th>");
            str.Append("</tr>");

            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo
            foreach (var reg in listaData)
            {
                str.Append("<tr>");
                str.AppendFormat("<td>");
                str.AppendFormat("<a href='JavaScript: eliminarEquipoRelacion({0},{1}); '><img src='" + url + "Content/Images/btn-cancel.png' alt='Eliminar registro' title='Eliminar registro' /></a>", reg.Equicodi1, reg.Equicodi2);
                str.AppendFormat("</td>");
                str.AppendFormat("<td>{0}</td>", reg.Famnomb1);
                str.AppendFormat("<td>{0}</td>", reg.Emprnomb1);
                str.AppendFormat("<td>{0}</td>", reg.Equinomb1);

                str.AppendFormat("<td>{0}</td>", reg.Famnomb2);
                str.AppendFormat("<td>{0}</td>", reg.Emprnomb2);
                str.AppendFormat("<td>{0}</td>", reg.Equinomb2);
                str.AppendFormat("<td>{0}</td>", reg.Equirelusumodificacion);
                str.AppendFormat("<td>{0}</td>", reg.EquirelfecmodificacionDesc);
                str.Append("</tr>");
            }
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        #endregion

        #region Equipamiento SEIN Carga Masiva Propiedades

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iEmpresa"></param>
        /// <param name="iFamilia"></param>
        /// <param name="iTipoEmpresa"></param>
        /// <param name="iEquipo"></param>
        /// <param name="sEstado"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ExportarDatosPropiedades(int iEmpresa, int iFamilia, int iTipoEmpresa, int iEquipo, string sEstado, string nombre)
        {
            try
            {
                List<EqEquipoDTO> Equipos = new List<EqEquipoDTO>();
                Equipos = FactorySic.GetEqEquipoRepository().ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, nombre, 1, int.MaxValue);

                foreach (var oEquipo in Equipos)
                {
                    oEquipo.PropiedadesEquipo = FactorySic.GetEqPropequiRepository().ListarValoresVigentesPropiedadesPaginado(oEquipo.Equicodi, iFamilia, "", 1, int.MaxValue);
                }

                return Equipos;
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
        /// <param name="ValoresPropiedades"></param>
        /// <returns></returns>
        public bool GuardarPropiedadesMasivo(List<EqPropequiDTO> ValoresPropiedades)
        {
            try
            {
                //excluir filas de las propiedades de Auditoria
                List<int> propiedadesSoloLectura = ListaPropiedadAuditoria();
                ValoresPropiedades = ValoresPropiedades.Where(x => !propiedadesSoloLectura.Contains(x.Propcodi)).ToList();

                //Guardar en BD
                foreach (var item in ValoresPropiedades)
                {
                    var ValorPropiedadActual = FactorySic.GetEqPropequiRepository().GetValorPropiedad(item.Propcodi, item.Equicodi);

                    if (ValorPropiedadActual == null && item.Valor == string.Empty)
                        continue;

                    if (ValorPropiedadActual != item.Valor)
                        FactorySic.GetEqPropequiRepository().Save(item);

                    FactorySic.GetEqEquipoRepository().UpdateOsinergminCodigo(item.Equicodi, item.osinergcodi);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        #endregion

        #region FICHA TÉCNICA

        #region Ficha técnica: Carga nuevos equipos

        /// <summary>
        /// Generar Excel de reporte plantilla
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void GenerarExcelPlantilla(string path, string fileName)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEquipamientoAppServicio.HojaEmpresa);
                hojas.Add(ConstantesEquipamientoAppServicio.HojaFamilia);
                hojas.Add(ConstantesEquipamientoAppServicio.HojaUbicacion);
                hojas.Add(ConstantesEquipamientoAppServicio.HojaSubestacion);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + file + "No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];

                    xlPackage.Save();

                    foreach (var item in hojas)
                    {
                        GenerarFileExcelHoja(xlPackage, item);
                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHoja(ExcelPackage xlPackage, string hoja)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int row = 2;
            int columnIniData = 1;
            switch (hoja)
            {
                case ConstantesEquipamientoAppServicio.HojaEmpresa:
                    //obtener empresas
                    var listaempresas = FactorySic.GetSiEmpresaRepository().ListarEmpresasFT();

                    foreach (var item in listaempresas)
                    {
                        ws.Cells[row, columnIniData++].Value = item.Emprcodi;
                        ws.Cells[row, columnIniData++].Value = item.Emprnomb;
                        ws.Cells[row, columnIniData++].Value = item.Emprrazsocial;
                        ws.Cells[row, columnIniData++].Value = item.Tipoemprdesc;
                        row++;
                        columnIniData = 1;
                    }
                    break;
                case ConstantesEquipamientoAppServicio.HojaFamilia:
                    //obtener familias
                    var listaFamilias = FactorySic.GetEqFamiliaRepository().ListarFamiliasFT().Where(x => x.Famcodi > 0).ToList();

                    foreach (var item in listaFamilias)
                    {
                        ws.Cells[row, columnIniData++].Value = item.Famcodi;
                        ws.Cells[row, columnIniData++].Value = item.Tareaabrev;
                        ws.Cells[row, columnIniData++].Value = item.Famnomb;
                        ws.Cells[row, columnIniData++].Value = item.Famabrev;
                        row++;
                        columnIniData = 1;
                    }
                    break;
                case ConstantesEquipamientoAppServicio.HojaUbicacion:
                    //obtener Ubicaciones
                    var listaUbicaciones = FactorySic.GetEqAreaRepository().ListarUbicacionFT();

                    foreach (var item in listaUbicaciones)
                    {
                        ws.Cells[row, columnIniData++].Value = item.Areacodi;
                        ws.Cells[row, columnIniData++].Value = item.Tareaabrev;
                        ws.Cells[row, columnIniData++].Value = item.Areanomb;
                        ws.Cells[row, columnIniData++].Value = item.Areaabrev;
                        row++;
                        columnIniData = 1;
                    }
                    break;
                case ConstantesEquipamientoAppServicio.HojaSubestacion:
                    //obtener Subestación
                    var listaSubestaciones = FactorySic.GetEqEquipoRepository().ListarSubestacionFT();

                    foreach (var item in listaSubestaciones)
                    {
                        ws.Cells[row, columnIniData++].Value = item.Equicodi;
                        ws.Cells[row, columnIniData++].Value = item.Equinomb;
                        ws.Cells[row, columnIniData++].Value = item.Equiabrev;
                        ws.Cells[row, columnIniData++].Value = item.Areadesc;
                        ws.Cells[row, columnIniData++].Value = item.Emprnomb;
                        ws.Cells[row, columnIniData++].Value = item.Tipoemprdesc;
                        row++;
                        columnIniData = 1;
                    }
                    break;
            }

        }

        /// <summary>
        /// Metodo para la validacion de los datos a importar
        /// </summary>
        public void ValidarEquiposAImportarXLSX(string path, string fileName, string strUsuario,
                                               out List<EqEquipoDTO> lstRegEquiposCorrectos,
                                               out List<EqEquipoDTO> lstRegEquiposErroneos,
                                               out List<EqEquipoDTO> listaNuevo)
        {
            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsm", ".xlsx" }; // corregir
            if (!lExtensionPermitido.Contains(extension))
            {
                throw new ArgumentException("Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido));
            }

            DateTime fechaRegistro = DateTime.Now;

            // Obtener lista de equipos actuales
            List<EqEquipoDTO> equiposActuales = ListEqEquipos().Where(e => e.Equiestado == "A" || e.Equiestado == "P").ToList();

            //Listar Familias de la bd COES
            List<SiEmpresaDTO> listaEmpresas = FactorySic.GetSiEmpresaRepository().ListarEmpresasFT();
            List<EqFamiliaDTO> listaFamilias = FactorySic.GetEqFamiliaRepository().ListarFamiliasFT().Where(x => x.Famcodi > 0).ToList();
            List<EqAreaDTO> listaAreas = FactorySic.GetEqAreaRepository().ListarUbicacionFT();
            List<EqEquipoDTO> listaEquipos = FactorySic.GetEqEquipoRepository().ListarSubestacionFT();

            #region Archivo xlsx

            string savePath = path + fileName;
            List<FilaExcelEquipo> listaFilaMacro = ImportToDataTableEquipo(savePath);

            //Validación de registros macro
            lstRegEquiposCorrectos = new List<EqEquipoDTO>();
            lstRegEquiposErroneos = new List<EqEquipoDTO>();

            foreach (var regFila in listaFilaMacro)
            {
                //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                string mensajeValidacion = this.ValidarLecturaExcelEquipos(regFila, listaEmpresas, listaFamilias, listaAreas, listaEquipos);

                EqEquipoDTO entity = new EqEquipoDTO();
                entity.NroItem = regFila.NumItem;

                entity.Equicodi = regFila.Equicodi; // Código de equipos nuevo (-2) 
                entity.Emprcodi = regFila.Emprcodi;
                entity.Operadoremprcodi = regFila.Operadoremprcodi;
                entity.Famcodi = regFila.Famcodi;
                entity.Areacodi = regFila.Areacodi;

                entity.Equinomb = regFila.Equinomb;
                entity.Equiabrev = regFila.Equiabrev;
                entity.Equiabrev2 = regFila.Equiabrev2;
                entity.Equiestado = regFila.Equiestado == "ACTIVO" ? ConstantesEquipamientoAppServicio.Activo : regFila.Equiestado == "PROYECTO" ? ConstantesEquipamientoAppServicio.EnProyecto : "";
                entity.Equitension = regFila.Equitension;
                entity.Equipadre = regFila.Equipadre;
                entity.Grupocodi = -1; //default
                entity.EquiManiobra = "N"; //default
                entity.Osinergcodi = "0"; //default

                //nuevos registros
                entity.Lastuser = strUsuario; // Usuario de creacion del registro
                entity.Lastdate = fechaRegistro; // Fecha de creacion del registro

                // Si los datos son correctos VALIDO LA INFORMACION DE ACUERDO AL NEGOCIO
                if (mensajeValidacion == "")
                {
                    //Validar duplicados dentro de la plantilla
                    var equipoRepetido = ObtenerEquipoPorCriterio(entity, lstRegEquiposCorrectos);
                    if (equipoRepetido != null)
                    {
                        entity.Observaciones = "No se puede crear Equipos duplicados. Comparar con item N°" + equipoRepetido.NroItem;
                        lstRegEquiposErroneos.Add(entity);
                    }
                    else
                    {
                        //Validar duplicado en (BD)
                        var dtoEquipoRee = ObtenerEquipoPorCriterio(entity, equiposActuales);
                        bool existeRegistroEnBD = dtoEquipoRee != null;

                        if (existeRegistroEnBD) // si existe duplicado y es nuevo
                        {
                            entity.Observaciones = "Se encontró coincidencia con registro existente. No se puede crear duplicados";
                            lstRegEquiposErroneos.Add(entity);
                        }
                        else
                        {
                            lstRegEquiposCorrectos.Add(entity); // Es nuevo
                        }
                    }
                }
                else
                {
                    // Van los registros con formato incorrecto
                    entity.Observaciones = mensajeValidacion;

                    lstRegEquiposErroneos.Add(entity);
                }
            }

            #endregion

            listaNuevo = lstRegEquiposCorrectos.Where(x => x.Equicodi == -2).ToList(); // solo los nuevos
        }

        /// <summary>
        /// Devuelve objeto duplicado de una lista de equipos
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="listaEquipos"></param>
        /// <returns></returns>
        public EqEquipoDTO ObtenerEquipoPorCriterio(EqEquipoDTO entity, List<EqEquipoDTO> listaEquipos)
        {
            listaEquipos = listaEquipos.Where(x => x.Equinomb != null).ToList();
            EqEquipoDTO dtoEquipo = listaEquipos.Where(x => x.Equinomb.Trim().ToUpper() == entity.Equinomb.Trim().ToUpper()
                               && x.Areacodi == entity.Areacodi).FirstOrDefault();

            return dtoEquipo;
        }

        /// <summary>
        /// Importa registros de un DataTable
        /// </summary>
        /// <param name="filePath">Directorio de archivos</param>  
        /// <returns>devuelve una cadena</returns>
        public static List<FilaExcelEquipo> ImportToDataTableEquipo(string filePath)
        {
            List<FilaExcelEquipo> listaMacro = new List<FilaExcelEquipo>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexItem = 1;
            int indexCodEmpresa = indexItem + 1;
            int indexCodOperador = indexCodEmpresa + 1;
            int indexCodTipoEquipo = indexCodOperador + 1;
            int indexCodUbicacion = indexCodTipoEquipo + 1;
            int indexNomb = indexCodUbicacion + 1;
            int indexAbrev = indexNomb + 1;
            int indexAbrev2 = indexAbrev + 1;
            int indexEstado = indexAbrev2 + 1;
            int indexTension = indexEstado + 1;
            int indexCodSubEstacion = indexTension + 1;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 11;

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var sItem = string.Empty;
                    if (worksheet.Cells[row, indexItem].Value != null) sItem = worksheet.Cells[row, indexItem].Value.ToString();
                    Int32.TryParse(sItem, out int numItem);

                    var sCodEmpresa = string.Empty;
                    if (worksheet.Cells[row, indexCodEmpresa].Value != null) sCodEmpresa = worksheet.Cells[row, indexCodEmpresa].Value.ToString();

                    var sCodOperador = string.Empty;
                    if (worksheet.Cells[row, indexCodOperador].Value != null) sCodOperador = worksheet.Cells[row, indexCodOperador].Value.ToString();

                    var sCodTipoEquipo = string.Empty;
                    if (worksheet.Cells[row, indexCodTipoEquipo].Value != null) sCodTipoEquipo = worksheet.Cells[row, indexCodTipoEquipo].Value.ToString();

                    var sCodUbicacion = string.Empty;
                    if (worksheet.Cells[row, indexCodUbicacion].Value != null) sCodUbicacion = worksheet.Cells[row, indexCodUbicacion].Value.ToString();

                    var sNomb = string.Empty;
                    if (worksheet.Cells[row, indexNomb].Value != null) sNomb = worksheet.Cells[row, indexNomb].Value.ToString();

                    var sAbrev = string.Empty;
                    if (worksheet.Cells[row, indexAbrev].Value != null) sAbrev = worksheet.Cells[row, indexAbrev].Value.ToString();

                    var sAbrev2 = string.Empty;
                    if (worksheet.Cells[row, indexAbrev2].Value != null) sAbrev2 = worksheet.Cells[row, indexAbrev2].Value.ToString();

                    var sEstado = string.Empty;
                    if (worksheet.Cells[row, indexEstado].Value != null) sEstado = worksheet.Cells[row, indexEstado].Value.ToString();

                    var sTension = string.Empty;
                    if (worksheet.Cells[row, indexTension].Value != null) sTension = worksheet.Cells[row, indexTension].Value.ToString();

                    var sCodSubEstacion = string.Empty;
                    if (worksheet.Cells[row, indexCodSubEstacion].Value != null) sCodSubEstacion = worksheet.Cells[row, indexCodSubEstacion].Value.ToString();

                    int equicodi = -2;
                    int emprcodi = 0;
                    int Operadoremprcodi = 0;
                    int? famcodi = null;
                    int? areacodi = null;
                    int equipadre = -1;
                    decimal? equitension = null;
                    try
                    {
                        sCodEmpresa = (sCodEmpresa ?? "").Trim();
                        sCodOperador = (sCodEmpresa ?? "").Trim();
                        sCodTipoEquipo = (sCodTipoEquipo ?? "").Trim();
                        sCodUbicacion = (sCodUbicacion ?? "").Trim();
                        sNomb = (sNomb ?? "").Trim();
                        sAbrev = (sAbrev ?? "").Trim();
                        sAbrev2 = (sAbrev2 ?? "").Trim();
                        sEstado = (sEstado ?? "").Trim();
                        sTension = (sTension ?? "").Trim();
                        sCodSubEstacion = (sCodSubEstacion ?? "").Trim();

                        emprcodi = (int)(((double?)worksheet.Cells[row, indexCodEmpresa].Value) ?? 0);
                        Operadoremprcodi = (int)(((double?)worksheet.Cells[row, indexCodOperador].Value) ?? 0);
                        if (sCodTipoEquipo != "")
                            famcodi = (int)(((double?)worksheet.Cells[row, indexCodTipoEquipo].Value) ?? null);
                        if (sCodUbicacion != "")
                            areacodi = (int)(((double?)worksheet.Cells[row, indexCodUbicacion].Value) ?? null);
                        equipadre = (int)(((double?)worksheet.Cells[row, indexCodSubEstacion].Value) ?? -1);
                        if (sTension != "")
                            equitension = (decimal)(((double?)worksheet.Cells[row, indexTension].Value) ?? null);
                    }
                    catch (Exception ex)
                    {
                        //No es necesario registrar el error de la conversión de datos de todas las celdas (pueden ocurrir como un máximo de 28 mil líneas de log para archivos de 7000 líneas de datos). 
                        //El tratamiento de estos errores se realiza en otra función (ValidarLecturaExcelEquipos de EquipamientoAppServicio) 
                        //que luego genera un .csv para el usuario (funcion GenerarArchivoEquiposErroneasCSV de EquipamientoAppServicio)
                    }

                    if (string.IsNullOrEmpty(sNomb) && string.IsNullOrEmpty(sCodUbicacion))
                    {
                        continue;
                    }

                    var regMantto = new FilaExcelEquipo()
                    {
                        Row = row,
                        NumItem = numItem,

                        StrEmprcodi = sCodEmpresa,
                        StrOperador = sCodOperador,
                        StrFamcodi = sCodTipoEquipo,
                        StrUbicacion = sCodUbicacion,

                        Equinomb = sNomb,
                        Equiabrev = sAbrev,
                        Equiabrev2 = sAbrev2,
                        Equiestado = sEstado.ToUpper(),
                        StrEquitension = sTension,
                        StrSubestacion = sCodSubEstacion,

                        Equicodi = equicodi,
                        Emprcodi = emprcodi,
                        Operadoremprcodi = Operadoremprcodi,
                        Famcodi = famcodi,
                        Areacodi = areacodi,
                        Equipadre = equipadre,

                        Equitension = equitension
                    };

                    listaMacro.Add(regMantto);
                }
            }

            return listaMacro;
        }

        /// <summary>
        /// Validación de propiedades al leer del excel importado
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <param name="listaEmpresas"></param>
        /// <param name="listaFamilias"></param>
        /// <param name="listaAreas"></param>
        /// <param name="listaEquipos"></param>
        /// <returns></returns>
        public string ValidarLecturaExcelEquipos(FilaExcelEquipo filaExcel, List<SiEmpresaDTO> listaEmpresas, List<EqFamiliaDTO> listaFamilias, List<EqAreaDTO> listaAreas, List<EqEquipoDTO> listaEquipos)
        {
            string columnCodEmpresa = "Código Empresa: ";
            string columnCodOperador = "Código Operador: ";
            string columnCodTipoEquipo = "Código tipo equipo: ";
            string columnCodUbicacion = "Código Ubicación: ";
            string columnNomb = "Nombre: ";
            string columnAbrev = "Abreviatura: ";
            string columnAbrev2 = "Abreviatura 2: ";
            string columnEstado = "Estado: ";
            string columnTension = "Tensión: ";
            string columnCodSubestacion = "Código Subestación: ";

            List<string> lMsgValidacion = new List<string>();

            List<string> listadoEstado = new List<string>();
            listadoEstado.Add("ACTIVO");
            listadoEstado.Add("PROYECTO");

            //Validar codigo de Empresa
            if (String.IsNullOrEmpty(filaExcel.StrEmprcodi))
            {
                lMsgValidacion.Add(columnCodEmpresa + "Esta vacío o en blanco");
            }
            else if (filaExcel.Emprcodi < 0)
            {
                lMsgValidacion.Add(columnCodEmpresa + "No es número válido");
            }
            else
            {
                SiEmpresaDTO regEmp = listaEmpresas.Find(x => x.Emprcodi == filaExcel.Emprcodi);
                if (regEmp == null)
                {
                    lMsgValidacion.Add(columnCodEmpresa + "Código de Empresa no existe");
                }
                else
                {
                    filaExcel.Operadoremprcodi = filaExcel.Operadoremprcodi <= 0 ? filaExcel.Emprcodi : filaExcel.Operadoremprcodi;
                }
            }

            //Validar codigo de Operador
            if (filaExcel.Operadoremprcodi < 0)
            {
                lMsgValidacion.Add(columnCodOperador + "No es número válido");
            }
            else
            {
                SiEmpresaDTO regOper = listaEmpresas.Find(x => x.Emprcodi == filaExcel.Emprcodi);
                if (regOper == null)
                {
                    lMsgValidacion.Add(columnCodOperador + "Código de operador no existe");
                }
            }

            //Validar Código de Tipo de equipo
            if (String.IsNullOrEmpty(filaExcel.StrFamcodi))
            {
                lMsgValidacion.Add(columnCodTipoEquipo + "Esta vacío o en blanco");
            }
            else if (filaExcel.Famcodi < 0)
            {
                lMsgValidacion.Add(columnCodTipoEquipo + "No es número válido");
            }
            else
            {
                EqFamiliaDTO regFam = listaFamilias.Find(x => x.Famcodi == filaExcel.Famcodi);
                if (regFam == null)
                {
                    lMsgValidacion.Add(columnCodTipoEquipo + "Código de tipo equipo no existe");
                }
            }

            //Validar Código de Área
            if (String.IsNullOrEmpty(filaExcel.StrUbicacion))
            {
                lMsgValidacion.Add(columnCodUbicacion + "Esta vacío o en blanco");
            }
            else if (filaExcel.Areacodi < 0)
            {
                lMsgValidacion.Add(columnCodUbicacion + "No es número válido");
            }
            else
            {
                EqAreaDTO regArea = listaAreas.Find(x => x.Areacodi == filaExcel.Areacodi);
                if (regArea == null)
                {
                    lMsgValidacion.Add(columnCodUbicacion + "Ubicación de equipo no existe");
                }
                else
                {
                    EqFamiliaDTO regFam = listaFamilias.Find(x => x.Famcodi == filaExcel.Famcodi);
                    if (regFam != null)
                    {
                        EqAreaDTO regAreaFam = listaAreas.Find(x => x.Areacodi == filaExcel.Areacodi && x.Tareacodi == regFam.Tareacodi);
                        if (regAreaFam == null)
                        {
                            lMsgValidacion.Add(columnCodTipoEquipo + "Ubicación no es válida para el equipo seleccionado");
                        }
                    }
                }
            }

            // Validar Nombre Equipo
            if (String.IsNullOrEmpty(filaExcel.Equinomb))
            {
                lMsgValidacion.Add(columnNomb + "Esta vacío o en blanco");
            }
            else
            {
                if (filaExcel.Equinomb.Length > 80)
                {
                    lMsgValidacion.Add(columnNomb + "supera 80 caracteres");
                }
                else
                {
                    if (filaExcel.Equinomb.Contains("\n"))
                        lMsgValidacion.Add(columnNomb + "Tiene salto de línea");
                }
            }

            // Validar Abrev
            if (String.IsNullOrEmpty(filaExcel.Equiabrev))
            {
                lMsgValidacion.Add(columnAbrev + "Esta vacío o en blanco");
            }
            else
            {
                if (filaExcel.Equiabrev.Length > 25)
                {
                    lMsgValidacion.Add(columnAbrev + "Supera 25 caracteres");
                }
            }

            // Validar Abrev2
            if (String.IsNullOrEmpty(filaExcel.Equiabrev2))
            {
                lMsgValidacion.Add(columnAbrev2 + "Esta vacío o en blanco");
            }
            else
            {
                if (filaExcel.Equiabrev2.Length > 15)
                {
                    lMsgValidacion.Add(columnAbrev2 + "Supera 15 caracteres");
                }
            }

            // Validar Estado
            if (String.IsNullOrEmpty(filaExcel.Equiestado))
            {
                lMsgValidacion.Add(columnEstado + "Esta vacio o en blanco");
            }
            else
            {
                if (!listadoEstado.Contains(filaExcel.Equiestado))
                {
                    lMsgValidacion.Add(columnEstado + "El tipo de dato no es valido");
                }
            }

            if (filaExcel.Equitension < 0)
            {
                lMsgValidacion.Add(columnTension + "No es número válido");
            }

            //Validar Código de Subestación
            if (String.IsNullOrEmpty(filaExcel.StrSubestacion))
            {
                //lMsgValidacion.Add(columnCodSubestacion + "Esta vacío o en blanco");
            }
            else if (filaExcel.Equipadre < -1)
            {
                lMsgValidacion.Add(columnCodSubestacion + "No es número válido");
            }
            else
            {
                EqEquipoDTO regSubEst = listaEquipos.Find(x => x.Equicodi == filaExcel.Equipadre);
                if (regSubEst == null)
                {
                    lMsgValidacion.Add(columnCodSubestacion + "Subestación de equipo no existe");
                }
            }

            return string.Join(". ", lMsgValidacion);
        }

        /// <summary>
        /// Genera log de equipos erroneos
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lstRegEquiposErroneos"></param>
        /// <returns></returns>
        public string GenerarArchivoEquiposErroneosCSV(string path, List<EqEquipoDTO> lstRegEquiposErroneos)
        {
            string sLine = string.Empty;
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameCsv = "";
            fileNameCsv = sFecha + "_LogEquiposImport" + ".csv";

            using (var dbProviderWriter = new StreamWriter(new FileStream(path + fileNameCsv, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sLine = "ÍTEM;OBSERVACIONES;CÓDIGO DE EMPRESA;CÓDIGO DE OPERADOR;CÓDIGO DE TIPO DE EQUIPO;CÓDIGO DE UBICACIÓN;NOMBRE DE EQUIPO;ABREVIATURA;ABREVIATURA 2;ESTADO;Tensión;CÓDIGO DE SUBESTACIÓN";
                dbProviderWriter.WriteLine(sLine);
                foreach (EqEquipoDTO entity in lstRegEquiposErroneos)
                {
                    sLine = this.CreateFilaErroneaEquipoString(entity);
                    dbProviderWriter.WriteLine(sLine);
                }
                dbProviderWriter.Close();
            }
            return fileNameCsv;
        }

        /// <summary>
        /// Guarda y actualiza Equipos masivamente
        /// </summary>
        /// <param name="listaNuevo"></param>
        /// <param name="listaModificado"></param>
        /// <param name="usuario"></param>
        public void GuardarDatosMasivosEquipos(List<EqEquipoDTO> listaNuevo, string usuario)
        {
            //1. Crea el nuevo equipo asignado un código
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guarda Registros nuevos masivamente
                        if (listaNuevo != null && listaNuevo.Any())
                        {
                            foreach (var item in listaNuevo)
                            {
                                item.Lastuser = usuario;
                                item.Lastdate = DateTime.Now;

                                var equicodi = FactorySic.GetEqEquipoRepository().Save(item, connection, transaction);
                                item.Equicodi = equicodi;
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            foreach (var oEquipoNuevo in listaNuevo)
            {
                //2. Almacena cada campo general como histórico del equipo
                RegistrarHistoricoCreacionEquipo(oEquipoNuevo, usuario);

                //3. Crea la relación de inicio del equipo con la empresa para el TTIE.
                (new TitularidadAppServicio()).SaveSiHisempeqDataInicial(oEquipoNuevo.Emprcodi ?? 0, oEquipoNuevo.Equicodi, oEquipoNuevo.Equiestado, usuario);
            }
        }

        /// <summary>
        /// Metodo que escribe una fila del archivo .CSV de equipos Erroneos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateFilaErroneaEquipoString(EqEquipoDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            sLine += ((entity.Emprcodi != null) ? entity.Emprcodi.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Operadoremprcodi > 0) ? entity.Operadoremprcodi.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Famcodi != null) ? entity.Famcodi.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Areacodi != null) ? entity.Areacodi.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equinomb != null) ? entity.Equinomb.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equiabrev != null) ? entity.Equiabrev.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equiabrev2 != null) ? entity.Equiabrev2.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equiestado != "") ? entity.Equiestado.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equitension != null) ? entity.Equitension.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equipadre != -1) ? entity.Equipadre.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            return sLine;
        }

        #endregion

        #region Ficha Técnica: carga Masiva parámetros equipos

        /// <summary>
        /// Metodo para la validacion de los datos al importar carga masiva parámetros
        /// </summary>
        public void ValidarPropiedadesMasivoAImportar(string path, string fileName, string strUsuario,
                                               out List<EqPropequiDTO> lstRegPropiedadesCorrectos,
                                               out List<EqPropequiDTO> lstRegPropiedadesErroneos,
                                               out List<EqPropequiDTO> listaNuevo,
                                               out List<EqPropequiDTO> listaModificado)
        {
            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsm", ".xlsx" }; // corregir
            if (!lExtensionPermitido.Contains(extension))
            {
                throw new ArgumentException("Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido));
            }

            DateTime fechaRegistro = DateTime.Now;

            #region Archivo xlsx

            string savePath = path + fileName;
            List<FilaExcelPropiedadesEquipos> listaFilaMacro = ImportToDataTablePropiedadesEquipos(savePath);

            //Validación de registros macro
            lstRegPropiedadesCorrectos = new List<EqPropequiDTO>();
            lstRegPropiedadesErroneos = new List<EqPropequiDTO>();

            listaNuevo = new List<EqPropequiDTO>();
            listaModificado = new List<EqPropequiDTO>();

            foreach (var regFila in listaFilaMacro)
            {
                //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                string mensajeValidacion = this.ValidarLecturaPropEquiExcel(regFila);

                EqPropequiDTO entity = new EqPropequiDTO();
                entity.NroItem = regFila.Row;
                entity.Equicodi = regFila.Equicodi; // Código de equipo 
                entity.Propcodi = regFila.Propcodi; // Código de propiedad 
                entity.Fechapropequi = regFila.Fechapropequi; // fecha
                entity.FechapropequiDesc = entity.Fechapropequi != null ? entity.Fechapropequi.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
                entity.Valor = regFila.Valor;
                entity.Propequicomentario = regFila.Propequicomentario != null ? regFila.Propequicomentario.Trim() : string.Empty;
                entity.Propequisustento = regFila.Propequisustento != null ? regFila.Propequisustento.Trim() : string.Empty;

                //Capturar valor correcto según el valor
                if (entity.Valor == "0")
                {
                    entity.Propequicheckcero = regFila.StrPropequicheckcero == "SI" ? ConstantesEquipamientoAppServicio.ValorSi : ConstantesEquipamientoAppServicio.ValorNo;
                    entity.PropequicheckceroDesc = regFila.StrPropequicheckcero;
                }
                else
                    entity.Propequicheckcero = ConstantesEquipamientoAppServicio.ValorNo;

                //nuevos registros
                entity.Propequiusucreacion = strUsuario; // Usuario de creacion del registro
                entity.Propequifeccreacion = fechaRegistro; // Fecha de creacion del registro

                // Si los datos son correctos VALIDO LA INFORMACION DE ACUERDO AL NEGOCIO
                if (mensajeValidacion == "")
                {
                    // Lista histórico de propequi para la entidad
                    List<EqPropequiDTO> listaPropHist = this.ListarValoresHistoricosPropiedadPorEquipo(entity.Equicodi, ConstantesAppServicio.ParametroDefecto);

                    //Validar parámetros según la fecha de vigencia
                    if (entity.Fechapropequi != null && entity.Valor != null && entity.Valor.Trim() != "")
                    {
                        // Lista histórico para obtener el último vigente hasta el día que quiero registrar 
                        List<EqPropequiDTO> listaHistXProp = listaPropHist.Where(x => x.Propcodi == entity.Propcodi && x.Fechapropequi <= entity.Fechapropequi).OrderByDescending(x => x.Fechapropequi).ToList();
                        EqPropequiDTO regActivoHist = listaHistXProp.Find(x => x.Propequideleted == 0); // obtiene el último vigente

                        //verificar si es futura
                        if (entity.Fechapropequi >= DateTime.Today.AddDays(1))
                        {
                            if (regActivoHist != null && regActivoHist.Fechapropequi == entity.Fechapropequi)
                            {
                                bool existeDif = (entity.Valor != regActivoHist.Valor); // solo si se cambia el valor se considera que hay diferencia

                                if (existeDif)
                                {
                                    EqPropequiDTO regActivo = (EqPropequiDTO)regActivoHist.Clone();
                                    regActivo.Valor = entity.Valor;
                                    regActivo.Propequicheckcero = entity.Propequicheckcero;
                                    regActivo.Propequicomentario = entity.Propequicomentario;
                                    regActivo.Propequisustento = entity.Propequisustento;
                                    regActivo.Propequideleted = 0;
                                    regActivo.Propequideleted2 = 0;
                                    regActivo.Propequifecmodificacion = entity.Propequifeccreacion;
                                    regActivo.Propequiusumodificacion = entity.Propequiusucreacion;
                                    listaModificado.Add(regActivo);

                                    regActivoHist.Propequideleted = listaHistXProp.Max(x => x.Propequideleted) + 1;
                                    listaNuevo.Add(regActivoHist);
                                }
                                else
                                {
                                    //si hay cambios en columnas opcionales
                                    if (entity.Propequicheckcero != regActivoHist.Propequicheckcero || entity.Propequicomentario != regActivoHist.Propequicomentario || entity.Propequisustento != regActivoHist.Propequisustento)
                                    {
                                        EqPropequiDTO regActivo = (EqPropequiDTO)regActivoHist.Clone();
                                        regActivo.Propequicheckcero = entity.Propequicheckcero;
                                        regActivo.Propequicomentario = entity.Propequicomentario;
                                        regActivo.Propequisustento = entity.Propequisustento;
                                        regActivo.Propequideleted2 = regActivo.Propequideleted;
                                        listaModificado.Add(regActivo);
                                    }
                                }
                            }
                            else
                            {
                                entity.Propequideleted = 0;
                                listaNuevo.Add(entity);
                            }
                        }
                        else
                        {
                            bool existeModificacionPasado = true;
                            string msjModificacionPasado = "Fecha de vigencia: Se está registrando / editando información con fecha de vigencia actual o anterior.";

                            if (regActivoHist != null && regActivoHist.Fechapropequi == entity.Fechapropequi)
                            {
                                existeModificacionPasado = entity.Valor != regActivoHist.Valor;
                                if (existeModificacionPasado) msjModificacionPasado = "Valor: Se está registrando / editando información con fecha de vigencia actual o anterior.";

                                //SI CAMBIA EL VALOR NO SE TOMARÁ EN CUENTA POR NO SER FECHA FUTURA
                                //Solo si hay cambios en columnas opcionales se actualizará esos campos
                                if (entity.Propequicheckcero != regActivoHist.Propequicheckcero
                                        || entity.Propequicomentario != regActivoHist.Propequicomentario
                                        || entity.Propequisustento != regActivoHist.Propequisustento)
                                {
                                    existeModificacionPasado = false;

                                    EqPropequiDTO regActivo = (EqPropequiDTO)regActivoHist.Clone();
                                    regActivo.Propequicheckcero = entity.Propequicheckcero;
                                    regActivo.Propequicomentario = entity.Propequicomentario;
                                    regActivo.Propequisustento = entity.Propequisustento;
                                    regActivo.Propequideleted2 = regActivo.Propequideleted;

                                    listaModificado.Add(regActivo);
                                }
                            }

                            if (existeModificacionPasado)
                            {
                                // Van los registros con formato incorrecto
                                entity.Observaciones = msjModificacionPasado;

                                lstRegPropiedadesErroneos.Add(entity);
                            }
                        }
                    }
                }
                else
                {
                    // Van los registros con formato incorrecto
                    entity.Observaciones = mensajeValidacion;

                    lstRegPropiedadesErroneos.Add(entity);
                }
            }

            lstRegPropiedadesCorrectos.AddRange(listaNuevo);
            lstRegPropiedadesCorrectos.AddRange(listaModificado);

            #endregion
        }

        /// <summary>
        /// Importa registros de un DataTable
        /// </summary>
        /// <param name="filePath">Directorio de archivos</param>  
        /// <returns>devuelve una cadena</returns>
        public static List<FilaExcelPropiedadesEquipos> ImportToDataTablePropiedadesEquipos(string filePath)
        {
            List<FilaExcelPropiedadesEquipos> listaMacro = new List<FilaExcelPropiedadesEquipos>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 6;

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    int iFilaCodigoPropiedad = 2;
                    int iColumnaCodigoEquipo = 4;
                    int iColDataPropEqui = 13;

                    //int indexItem = 1;
                    int indexUnidad = 13;
                    int indexFecVigencia = indexUnidad + 1;
                    int indexValor = indexFecVigencia + 1;
                    int indexValorCeroCorrecto = indexValor + 1;
                    int indexComentario = indexValorCeroCorrecto + 1;
                    int indexSustento = indexComentario + 1;
                    int indexFechModif = indexSustento + 1;
                    int indexUsuModif = indexFechModif + 1;

                    var codigoEquipo = worksheet.Cells[row, iColumnaCodigoEquipo].Value.ToString();
                    if (string.IsNullOrEmpty(codigoEquipo))
                    {
                        continue;
                    }

                    while (worksheet.Cells[iFilaCodigoPropiedad, iColDataPropEqui].Value != null)
                    {
                        var sFechVigencia = string.Empty;
                        if (worksheet.Cells[row, indexFecVigencia].Value != null) sFechVigencia = worksheet.Cells[row, indexFecVigencia].Value.ToString();

                        var sValor = string.Empty;
                        if (worksheet.Cells[row, indexValor].Value != null) sValor = worksheet.Cells[row, indexValor].Value.ToString();

                        //si no tiene fecha o valor no se considera
                        if (string.IsNullOrEmpty(sFechVigencia) || string.IsNullOrEmpty(sValor))
                        {
                            iColDataPropEqui += 8;
                            indexFecVigencia += 8;
                            indexValor += 8;
                            indexValorCeroCorrecto += 8;
                            indexComentario += 8;
                            indexSustento += 8;

                            continue;
                        }

                        var sValCeroCorrecto = string.Empty;
                        if (worksheet.Cells[row, indexValorCeroCorrecto].Value != null) sValCeroCorrecto = worksheet.Cells[row, indexValorCeroCorrecto].Value.ToString();

                        var sComentario = string.Empty;
                        if (worksheet.Cells[row, indexComentario].Value != null) sComentario = worksheet.Cells[row, indexComentario].Value.ToString();

                        var sSustento = string.Empty;
                        if (worksheet.Cells[row, indexSustento].Value != null) sSustento = worksheet.Cells[row, indexSustento].Value.ToString();

                        int propcodi = 0;
                        int equicodi = 0;
                        DateTime? fechaVigencia = null;
                        try
                        {
                            sFechVigencia = (sFechVigencia ?? "").Trim();
                            sValor = (sValor ?? "").Trim();
                            sValCeroCorrecto = (sValCeroCorrecto ?? "").Trim();
                            sComentario = (sComentario ?? "").Trim();
                            sSustento = (sSustento ?? "").Trim();
                            equicodi = (int)(((double?)worksheet.Cells[row, iColumnaCodigoEquipo].Value));
                            propcodi = (int)(((double?)worksheet.Cells[2, iColDataPropEqui].Value));

                            // convertir a datetime
                            if (sFechVigencia != "")
                            {
                                var objFec = worksheet.Cells[row, indexFecVigencia].Value;
                                if (objFec is DateTime)
                                    fechaVigencia = (DateTime?)objFec;
                                else
                                    fechaVigencia = DateTime.ParseExact(sFechVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                        var regpropequipo = new FilaExcelPropiedadesEquipos()
                        {
                            Row = row,
                            //NumItem = numItem,
                            Valor = sValor,
                            StrFechapropequi = sFechVigencia,
                            Propequisustento = (sSustento ?? "").Trim(),
                            Propequicomentario = (sComentario ?? "").Trim(),
                            StrPropequicheckcero = sValCeroCorrecto.ToUpper(),
                            Propcodi = propcodi,
                            Equicodi = equicodi,
                            Fechapropequi = fechaVigencia
                        };

                        listaMacro.Add(regpropequipo);

                        iColDataPropEqui += 8;
                        indexFecVigencia += 8;
                        indexValor += 8;
                        indexValorCeroCorrecto += 8;
                        indexComentario += 8;
                        indexSustento += 8;
                    }
                }
            }

            return listaMacro;
        }

        /// <summary>
        /// Validación de propiedades carga masiva
        /// </summary>
        /// <param name="filaExcel"></param>
        /// <param name="listaFamilias"></param>
        /// <returns></returns>
        public string ValidarLecturaPropEquiExcel(FilaExcelPropiedadesEquipos filaExcel)
        {
            string columnFechVigencia = "Fecha vigencia: ";
            string columnValor = "Valor: ";
            string columnValorCheckCero = "Valor(0) correcto: ";
            string columnComentario = "Comentario: ";
            string columnSustento = "Sustento: ";

            List<string> lMsgValidacion = new List<string>();

            List<string> listaValorCheckCero = new List<string>();
            listaValorCheckCero.Add("SI");
            listaValorCheckCero.Add("NO");

            // Validar Valor
            if (filaExcel.Valor != null && filaExcel.Valor == "0")
            {
                if (String.IsNullOrEmpty(filaExcel.StrPropequicheckcero))
                {
                    lMsgValidacion.Add(columnValorCheckCero + "Esta vacío o en blanco ");
                }
                else
                {
                    if (!listaValorCheckCero.Contains(filaExcel.StrPropequicheckcero))
                    {
                        lMsgValidacion.Add(columnValorCheckCero + "Valor no valido ");
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(filaExcel.StrPropequicheckcero))
                {
                    lMsgValidacion.Add(columnValorCheckCero + " debe estar en blanco dado que el Valor es diferente de 0");
                }
            }

            //Validar comentario
            if (!string.IsNullOrEmpty(filaExcel.Propequicomentario))
            {
                if (filaExcel.Propequicomentario.Length > 500)
                    lMsgValidacion.Add(columnComentario + "no puede tener más de 500 caracteres");
            }

            //Validar sustento
            if (!string.IsNullOrEmpty(filaExcel.Propequisustento))
            {
                if (filaExcel.Propequisustento.Length > 400)
                    lMsgValidacion.Add(columnSustento + "no puede tener más de 400 caracteres");
            }

            return string.Join(". ", lMsgValidacion);
        }

        /// <summary>
        /// Guarda y actualiza parámetros de equipos masivamente
        /// </summary>
        /// <param name="listaNuevo"></param>
        /// <param name="listaModificado"></param>
        /// <param name="usuario"></param>
        public void CargaMasivaPropiedadesEquipos(List<EqPropequiDTO> listaNuevo, List<EqPropequiDTO> listaModificado, string usuario)
        {
            //excluir filas de las propiedades de Auditoria
            List<int> propiedadesSoloLectura = ListaPropiedadAuditoria();
            listaNuevo = listaNuevo.Where(x => !propiedadesSoloLectura.Contains(x.Propcodi)).ToList();
            listaModificado = listaModificado.Where(x => !propiedadesSoloLectura.Contains(x.Propcodi)).ToList();

            //Guardar en BD
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guarda Registros nuevos masivamente
                        if (listaNuevo != null && listaNuevo.Any())
                        {
                            foreach (var item in listaNuevo)
                            {
                                FactorySic.GetEqPropequiRepository().Save(item, connection, transaction);
                            }
                        }

                        //Actualiza Registros masivamente
                        if (listaModificado != null && listaModificado.Any())
                        {
                            foreach (var item in listaModificado)
                            {
                                item.Fechapropequi = item.Fechapropequi != null ? item.Fechapropequi : DateTime.Now;
                                //item.Propequifecmodificacion = item.Propequifecmodificacion != null ? item.Propequifecmodificacion : DateTime.Now;
                                FactorySic.GetEqPropequiRepository().Update(item, connection, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Genera log de parámetros de equipos erroneos
        /// </summary>
        /// <param name="path"></param>
        /// <param name="lstRegPropiedadesErroneos"></param>
        /// <returns></returns>
        public string GenerarArchivoLogParametrosErroneosCSV(string path, List<EqPropequiDTO> lstRegPropiedadesErroneos)
        {
            string sLine = string.Empty;
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameCsv = "";
            fileNameCsv = sFecha + "_LogPropiedadesImport" + ".csv";

            using (var dbProviderWriter = new StreamWriter(new FileStream(path + fileNameCsv, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sLine = "FILA;OBSERVACIONES;PROPIEDAD;FECHA DE VIGENCIA;VALOR;VALOR CERO(0) CORRECTO;COMENTARIO;SUSTENTO";
                dbProviderWriter.WriteLine(sLine);
                foreach (EqPropequiDTO entity in lstRegPropiedadesErroneos)
                {
                    sLine = this.CreateFilaErroneaParametroString(entity);
                    dbProviderWriter.WriteLine(sLine);
                }

                dbProviderWriter.Close();
            }
            return fileNameCsv;
        }

        /// <summary>
        /// Metodo que escribe una fila del archivo .CSV de propiedades Erroneas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string CreateFilaErroneaParametroString(EqPropequiDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            sLine += (entity.Propcodi.ToString()) + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Fechapropequi != null) ? entity.FechapropequiDesc : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Valor != null) ? entity.Valor.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.PropequicheckceroDesc != null) ? entity.PropequicheckceroDesc : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propequicomentario != null) ? entity.Propequicomentario.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Propequisustento != null) ? entity.Propequisustento.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            return sLine;
        }

        #endregion

        #region Ficha Tecnica 2023

        /// <summary>
        /// Devuelve el listado de proyectos asciados a un equipo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<FtExtReleqpryDTO> ObtenerListadoProyectosPorEquipo(int equicodi)
        {            
            return servFictec.ObtenerListadoProyectosPorEquipo(equicodi);
        }

        /// <summary>
        /// Devuelve el listado de empresas copropietarias de cierto equipo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        public List<FtExtReleqempltDTO> ObtenerListadoEmpresasCopropietarias(string strEquicodis)
        {
            return servFictec.ObtenerListadoEmpresasCopropietarias(strEquicodis); 
        }

        /// <summary>
        /// Devuelve el listado de proyectos activos
        /// </summary>
        /// <returns></returns>
        public List<FtExtProyectoDTO> ListarProyectosExistentes()
        {
            List<FtExtProyectoDTO> lstSalida = new List<FtExtProyectoDTO>();
            List<FtExtProyectoDTO> lstTemp = new List<FtExtProyectoDTO>();

            lstTemp = FactorySic.GetFtExtProyectoRepository().ListarPorEstado("A").OrderBy(x=>x.Emprnomb).ThenBy(x=>x.Ftpryeocodigo).ToList();
            lstSalida = lstTemp;
            return lstSalida;
        }

        /// <summary>
        /// Devuelve datos del proyecto 
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>        
        public FtExtProyectoDTO ObtenerDatosProyecto(int ftprycodi)
        {            
            return servFictec.GetByIdFtExtProyecto(ftprycodi);
        }

        /// <summary>
        /// Devuelve datos de la empresa 
        /// </summary>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>        
        public EmpresaCoes ObtenerDatosEmpresa(int emprcodi)
        {
            List<EmpresaCoes> lstEmpresas = servFictec.ListarEmpresasActivas();

            return lstEmpresas.Find(x=>x.Emprcodi == emprcodi);
        }

        /// <summary>
        /// Guarda los cambios en asignacion del equipo
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="strCambiosPE"></param>
        /// <param name="strCambiosELT"></param>
        /// <param name="usuario"></param>
        public void GuardarDatosAsignacionFT(int equicodi, string strCambiosPE, string strCambiosELT, string usuario)
        {
            List<FtExtReleqpryDTO> lstTotalPE = FactorySic.GetFtExtReleqpryRepository().ListarSoloEquipos();
            List<FtExtReleqempltDTO> lstTotalECo = FactorySic.GetFtExtReleqempltRepository().List();

            //Obtenemos listas de nuevos y editados para proyectos extranet
            ObtenerListaAgregadosProyectosExtranet(strCambiosPE, lstTotalPE, equicodi, usuario, out List <FtExtReleqpryDTO> lstPENuevos, out List <FtExtReleqpryDTO> lstPEEditados);

            //Obtenemos listas de nuevos y editados para empresas copropietarias
            ObtenerListaAgregadosEmpresasCopropietarias(strCambiosELT, lstTotalECo, equicodi, usuario, out List<FtExtReleqempltDTO> lstECoNuevos, out List<FtExtReleqempltDTO> lstECoEditados);

            GuardarDatosDeAsignacionTransaccionalmente(lstPENuevos, lstPEEditados, lstECoNuevos, lstECoEditados);           

        }


        /// <summary>
        /// Actualiza los cambios de asignacion de grupo/MO en proyectos en el modulo de asignacion de proyectos con etapas
        /// </summary>        
        /// <param name="strCambiosPE"></param>
        /// <param name="usuario"></param>
        public void ActualizarCambiosEnAsignacionDeProyectos(string strCambiosPE, string usuario)
        {
            if (strCambiosPE.Length > 0)
            {
                string[] separadorGrupo = { "??" };
                string[] separadorUnidad = { "$$" };
                string[] lstCambios = strCambiosPE.Split(separadorGrupo, System.StringSplitOptions.RemoveEmptyEntries);

                //Obtengo los FtExtEtempdetpry para los proyectos en lista
                List<int> lstFtprycodis = new List<int>();
                List<FtExtEtempdetpryDTO> lstRelProyectoEmpEtapa = new List<FtExtEtempdetpryDTO>();
                foreach (var cambio in lstCambios)
                {
                    string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                    if (lstDatos.Length > 0)
                    {
                        int miFtprycodi_ = Int32.Parse(lstDatos[1]);
                        lstFtprycodis.Add(miFtprycodi_);
                    }
                }
                string strFtprycodis = string.Join(",", lstFtprycodis);
                List<FtExtEtempdetpryDTO> lstRelPryEmpEtapa = strFtprycodis != "" ? FactorySic.GetFtExtEtempdetpryRepository().GetByProyectos(strFtprycodis) : new List<FtExtEtempdetpryDTO>();
                List<FtExtProyectoDTO> lstProyectosEnRel = strFtprycodis != "" ? FactorySic.GetFtExtProyectoRepository().ListarGrupo(strFtprycodis) : new List<FtExtProyectoDTO>();
                List<FtExtRelempetapaDTO> lstRelEE = lstRelPryEmpEtapa.Any() ? FactorySic.GetFtExtRelempetapaRepository().GetByProyectos(strFtprycodis) : new List<FtExtRelempetapaDTO>();

                //Por cada cambio (agrego o baja proyectos) para el equipo
                foreach (var cambio in lstCambios)
                {
                    string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                    if (lstDatos.Length > 0)
                    {
                        FtExtReleqpryDTO obj = new FtExtReleqpryDTO();

                        int miFtreqpcodi = Int32.Parse(lstDatos[0]);
                        int miFtprycodi = Int32.Parse(lstDatos[1]);
                        int miEstado = Int32.Parse(lstDatos[2]);
                        int esAgregado = Int32.Parse(lstDatos[3]);
                        int esEditado = Int32.Parse(lstDatos[4]);

                        //busco las etapas para cada proyecto, solo activos
                        List<FtExtEtempdetpryDTO> relPryEmpEtapaPorProyecto = lstRelPryEmpEtapa.Where(x => x.Ftprycodi == miFtprycodi && x.Feepryestado == ConstantesFichaTecnica.EstadoStrActivo).ToList();

                        //solo actualizo cambios para aquellos que tengan relacion con alguna etapa (1 o mas)
                        if (relPryEmpEtapaPorProyecto.Any())
                        {
                            //para cada etapa, actualizo
                            foreach (var rel in relPryEmpEtapaPorProyecto)
                            {
                                int fetempcodi = rel.Fetempcodi;
                                FtExtRelempetapaDTO ree = lstRelEE.Find(x => x.Fetempcodi == fetempcodi);

                                FtExtRelempetapaDTO grup = servFictec.ObtenerInformacionRelEmpresaEtapa(fetempcodi);

                                List<FtExtEtempdetpryDTO> lstProyectos = grup.ListaProyectos;
                                int accion = ConstantesFichaTecnica.AccionEditar;
                                int emprcodi = ree.Emprcodi;
                                int idEtapa = ree.Ftetcodi;

                                //Actualiza ventana asignacion proyectos
                                servFictec.GuardarDatosProyectoYRelaciones(accion, fetempcodi, emprcodi, idEtapa, lstProyectos, new List<FTRelacionEGP>(), new List<FTRelacionEGP>(), usuario);

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el listado de registros nuevos y editados de proyectos extranet
        /// </summary>
        /// <param name="strCambiosPE"></param>
        /// <param name="lstTotalPE"></param>
        /// <param name="lstNuevos"></param>
        /// <param name="lstEditados"></param>
        private void ObtenerListaAgregadosProyectosExtranet(string strCambiosPE, List<FtExtReleqpryDTO> lstTotalPE, int equicodi, string usuario, out List<FtExtReleqpryDTO> lstNuevos, out List<FtExtReleqpryDTO> lstEditados)
        {
            lstNuevos = new List<FtExtReleqpryDTO>();
            lstEditados = new List<FtExtReleqpryDTO>();

            if (strCambiosPE.Length > 0)
            {
                string[] separadorGrupo = { "??"};
                string[] separadorUnidad = { "$$" };
                string[] lstCambios = strCambiosPE.Split(separadorGrupo, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var cambio in lstCambios)
                {
                    string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                    if (lstDatos.Length > 0)
                    {
                        FtExtReleqpryDTO obj = new FtExtReleqpryDTO();

                        int miFtreqpcodi = Int32.Parse(lstDatos[0]);
                        int miFtprycodi = Int32.Parse(lstDatos[1]);
                        int miEstado = Int32.Parse(lstDatos[2]);
                        int esAgregado = Int32.Parse(lstDatos[3]);
                        int esEditado = Int32.Parse(lstDatos[4]);

                        if (miFtreqpcodi == -1) //Es agregado
                        {
                            //obj.Ftreqpcodi { get; set; }
                            obj.Equicodi = equicodi;
                            obj.Ftprycodi = miFtprycodi;
                            obj.Ftreqpestado = miEstado;
                            obj.Ftreqpusucreacion = usuario;
                            obj.Ftreqpfeccreacion = DateTime.Now;
                            obj.Ftreqpusumodificacion = usuario;
                            obj.Ftreqpfecmodificacion = DateTime.Now;
                            obj.Grupocodi = null;

                            lstNuevos.Add(obj);
                        }
                        else //es Editado
                        {
                            FtExtReleqpryDTO objEditado = lstTotalPE.Find(x => x.Ftreqpcodi == miFtreqpcodi);

                            //Agrego solo si su estado sufrio cambios
                            if(objEditado.Ftreqpestado != miEstado)
                            {
                                obj = objEditado;
                                obj.Ftreqpestado = miEstado;
                                obj.Ftreqpusumodificacion = usuario;
                                obj.Ftreqpfecmodificacion = DateTime.Now;

                                lstEditados.Add(obj);
                            }
                            
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el listado de registros nuevos y editados de empresas copropietarias
        /// </summary>
        /// <param name="strCambiosELT"></param>
        /// <param name="lstTotalECo"></param>
        /// <param name="equicodi"></param>
        /// <param name="usuario"></param>
        /// <param name="lstNuevos"></param>
        /// <param name="lstEditados"></param>
        private void ObtenerListaAgregadosEmpresasCopropietarias(string strCambiosELT, List<FtExtReleqempltDTO> lstTotalECo, int equicodi, string usuario, out List<FtExtReleqempltDTO> lstNuevos, out List<FtExtReleqempltDTO> lstEditados)
        {
            lstNuevos = new List<FtExtReleqempltDTO>();
            lstEditados = new List<FtExtReleqempltDTO>();

            //Solo si el equipo es Linea de transmision
            EqEquipoDTO e = FactorySic.GetEqEquipoRepository().GetById(equicodi);

            if (e.Famcodi == 8) // 8 : Lineas de transmision
            {
                if (strCambiosELT.Length > 0)
                {
                    string[] separadorGrupo = { "??" };
                    string[] separadorUnidad = { "$$" };
                    string[] lstCambios = strCambiosELT.Split(separadorGrupo, System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cambio in lstCambios)
                    {
                        string[] lstDatos = cambio.Split(separadorUnidad, System.StringSplitOptions.RemoveEmptyEntries);

                        if (lstDatos.Length > 0)
                        {
                            FtExtReleqempltDTO obj = new FtExtReleqempltDTO();

                            int miFtreqecodi = Int32.Parse(lstDatos[0]);
                            int miEmprcodi = Int32.Parse(lstDatos[1]);
                            int miEstado = Int32.Parse(lstDatos[2]);
                            int esAgregado = Int32.Parse(lstDatos[3]);
                            int esEditado = Int32.Parse(lstDatos[4]);

                            if (miFtreqecodi == -1) //Es agregado
                            {
                                //obj.Ftreqecodi { get; set; }
                                obj.Equicodi = equicodi;
                                obj.Emprcodi = miEmprcodi;
                                obj.Ftreqeestado = miEstado;
                                obj.Ftreqeusucreacion = usuario;
                                obj.Ftreqefeccreacion = DateTime.Now;
                                obj.Ftreqeusumodificacion = usuario;
                                obj.Ftreqefecmodificacion = DateTime.Now;

                                lstNuevos.Add(obj);
                            }
                            else //es Editado
                            {
                                FtExtReleqempltDTO objEditado = lstTotalECo.Find(x => x.Ftreqecodi == miFtreqecodi);

                                //Agrego solo si su estado sufrio cambios
                                if (objEditado.Ftreqeestado != miEstado)
                                {
                                    obj = objEditado;
                                    obj.Ftreqeestado = miEstado;
                                    obj.Ftreqeusumodificacion = usuario;
                                    obj.Ftreqefecmodificacion = DateTime.Now;

                                    lstEditados.Add(obj);
                                }

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve los datos de una relacion Equipo-Proyecto
        /// </summary>
        /// <param name="ftreqpcodi"></param>
        /// <returns></returns>
        public FtExtReleqpryDTO ObtenerDatosRelEquipoProyecto(int ftreqpcodi)
        {
            return servFictec.GetByIdFtExtReleqpry(ftreqpcodi);
        }

        /// <summary>
        /// Devuelve los datos de una relacion Empresa-LTx
        /// </summary>
        /// <param name="ftreqecodi"></param>
        /// <returns></returns>
        public FtExtReleqempltDTO ObtenerDatosRelEmpresaLT(int ftreqecodi)
        {
            return servFictec.GetByIdFtExtReleqemplt(ftreqecodi);
        }

        /// <summary>
        /// Guarda la informacion transaccionalmente
        /// </summary>
        /// <param name="lstPENuevos"></param>
        /// <param name="lstPEEditados"></param>
        /// <param name="lstECoNuevos"></param>
        /// <param name="lstECoEditados"></param>
        public void GuardarDatosDeAsignacionTransaccionalmente(List<FtExtReleqpryDTO> lstPENuevos, List<FtExtReleqpryDTO> lstPEEditados, List<FtExtReleqempltDTO> lstECoNuevos, List<FtExtReleqempltDTO> lstECoEditados)
        {
            DbTransaction tran = null;

            var UoW = FactorySic.UnitOfWork();

            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guardar los nuevos registros de proyectos extranet 
                        foreach (FtExtReleqpryDTO regNuevoPE in lstPENuevos)
                        {
                            int cod1 = FactorySic.GetFtExtReleqpryRepository().Save(regNuevoPE, connection, transaction);
                        }

                        //Actualizar los registros editados de proyectos extranet 
                        foreach (FtExtReleqpryDTO regEditadoPE in lstPEEditados)
                        {                            
                            FactorySic.GetFtExtReleqpryRepository().Update(regEditadoPE, connection, transaction);
                        }

                        //Guardar los nuevos registros de empresaas copropietarias
                        foreach (FtExtReleqempltDTO regNuevoECo in lstECoNuevos)
                        {
                            int cod2 = FactorySic.GetFtExtReleqempltRepository().Save(regNuevoECo, connection, transaction);
                        }

                        //Actualizar los registros editados de empresaas copropietarias 
                        foreach (FtExtReleqempltDTO regEditadoECo in lstECoEditados)
                        {
                            FactorySic.GetFtExtReleqempltRepository().Update(regEditadoECo, connection, transaction);
                        }


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            
        }

        public void GenerarExcelPlantillaRelEquipoProyecto(string path, string fileName)
        {
            try
            {
                List<string> hojas = new List<string>();
                hojas.Add(ConstantesEquipamientoAppServicio.HojaProyecto);
                hojas.Add(ConstantesEquipamientoAppServicio.HojaEquipo);

                string file = path + fileName;

                FileInfo fi = new FileInfo(file);
                // Revisar si existe
                if (!fi.Exists)
                {
                    throw new Exception("Archivo " + file + "No existe");
                }

                using (ExcelPackage xlPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];

                    xlPackage.Save();

                    foreach (var item in hojas)
                    {
                        GenerarFileExcelHojaRelEquipoProyecto(xlPackage, item);
                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        private void GenerarFileExcelHojaRelEquipoProyecto(ExcelPackage xlPackage, string hoja)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets[hoja];

            int row = 2;
            int columnIniData = 1;
            switch (hoja)
            {
                case ConstantesEquipamientoAppServicio.HojaProyecto:
                    //obtener proyectos con estado Activo
                    var listaPry = ListarProyectoPlantillaRelEquipoProyecto();
                    foreach (var item in listaPry)
                    {
                        ws.Cells[row, columnIniData++].Value = item.Ftprycodi;
                        ws.Cells[row, columnIniData++].Value = item.Emprnomb;
                        ws.Cells[row, columnIniData++].Value = item.Ftpryeocodigo;
                        ws.Cells[row, columnIniData++].Value = item.Ftpryeonombre;
                        ws.Cells[row, columnIniData++].Value = item.Ftprynombre;
                        row++;
                        columnIniData = 1;
                    }
                    break;
                case ConstantesEquipamientoAppServicio.HojaEquipo:
                    //obtener equipos con estado “Activo”, “En proyecto” o “Fuera de COES”
                    var listaEq = ListarEquipoPlantillaRelEquipoProyecto();

                    foreach (var item in listaEq)
                    {
                        ws.Cells[row, columnIniData++].Value = item.Equicodi;
                        ws.Cells[row, columnIniData++].Value = item.Equinomb;
                        ws.Cells[row, columnIniData++].Value = item.Equiabrev;
                        ws.Cells[row, columnIniData++].Value = item.EstadoDesc;
                        ws.Cells[row, columnIniData++].Value = item.Emprnomb;
                        ws.Cells[row, columnIniData++].Value = item.Famnomb;
                        ws.Cells[row, columnIniData++].Value = item.Areanomb;
                        row++;
                        columnIniData = 1;
                    }
                    break;
            }

        }

        private List<FtExtProyectoDTO> ListarProyectoPlantillaRelEquipoProyecto()
        {
            var listaPry = (new FichaTecnicaAppServicio()).ListarProyectos("-1", DateTime.Today.AddYears(-1), DateTime.Today).OrderBy(x => x.Ftprycodi).ToList();
            return listaPry;
        }

        private List<EqEquipoDTO> ListarEquipoPlantillaRelEquipoProyecto()
        {
            int iEmpresa = -2;
            int iFamilia = -2;
            int iTipoEmpresa = -2;
            int iEquipo = -2;
            string sEstado = " ";
            string sNombre = "";
            var listaEq = ListaEquipamientoPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, sEstado, sNombre, 1, Int32.MaxValue);
            listaEq = listaEq.Where(x => x.Equiestado == ConstantesAppServicio.Activo || x.Equiestado == ConstantesAppServicio.Proyecto
                                                            || x.Equiestado == ConstantesAppServicio.FueraCOES)
                            .OrderBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Famnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Equiabrev).ToList();

            foreach (var oEquipo in listaEq)
            {
                oEquipo.EstadoDesc = EquipamientoHelper.EstadoDescripcion(oEquipo.Equiestado);
            }

            return listaEq;
        }

        public void ValidarRelEquipoProyectoAImportarXLSX(string path, string fileName, string strUsuario,
                                               out List<FtExtReleqpryDTO> lstRegEquiposCorrectos,
                                               out List<FtExtReleqpryDTO> lstRegEquiposErroneos,
                                               out List<FtExtReleqpryDTO> listaNuevo)
        {
            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsm", ".xlsx" }; // corregir
            if (!lExtensionPermitido.Contains(extension))
            {
                throw new ArgumentException("Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido));
            }

            DateTime fechaRegistro = DateTime.Now;

            // BD 
            List<FtExtReleqpryDTO> relActuales = (new FichaTecnicaAppServicio()).ObtenerListadoProyectosPorEquipo(-1).Where(x => x.Ftreqpestado == 1).ToList();

            //data maestra de la bd COES
            var listaEq = ListarEquipoPlantillaRelEquipoProyecto();
            var listaPry = ListarProyectoPlantillaRelEquipoProyecto();

            #region Archivo xlsx

            string savePath = path + fileName;
            List<FilaExcelRelEquipoProyecto> listaFilaMacro = ImportToDataTableRelEquipoProyecto(savePath);

            //Validación de registros macro
            lstRegEquiposCorrectos = new List<FtExtReleqpryDTO>();
            lstRegEquiposErroneos = new List<FtExtReleqpryDTO>();

            foreach (var regFila in listaFilaMacro)
            {
                //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                string mensajeValidacion = this.ValidarLecturaExcelRelEquipoProyecto(regFila, listaEq, listaPry);

                FtExtReleqpryDTO entity = new FtExtReleqpryDTO();
                entity.NroItem = regFila.NumItem;

                entity.Equicodi = regFila.Equicodi;
                entity.Ftprycodi = regFila.Ftprycodi;
                entity.Grupocodi = null;
                entity.Emprnomb = regFila.StrEmpresaProyecto;
                entity.Ftpryeocodigo = regFila.StrCodigoEstudio;
                entity.Equinomb = regFila.StrNombreEquipo;
                entity.Equiabrev = regFila.StrAbreviaturaEq;
                entity.Equiestadodesc = regFila.StrEstadoEq;
                entity.Emprnomb2 = regFila.StrEmpresaEq;
                entity.Famnomb = regFila.StrTipoEquipo;
                entity.Areanomb = regFila.StrUbicacion;

                //nuevos registros
                entity.Ftreqpestado = 1;
                entity.Ftreqpusucreacion = strUsuario; // Usuario de creacion del registro
                entity.Ftreqpfeccreacion = fechaRegistro; // Fecha de creacion del registro

                // Si los datos son correctos VALIDO LA INFORMACION DE ACUERDO AL NEGOCIO
                if (mensajeValidacion == "")
                {
                    //Validar duplicados dentro de la plantilla
                    var regRepetido = ObtenerReleqpryPorCriterio(entity, lstRegEquiposCorrectos);
                    if (regRepetido != null)
                    {
                        entity.Observaciones = "No se puede crear Relaciones duplicadas. Comparar con item N°" + regRepetido.NroItem;
                        lstRegEquiposErroneos.Add(entity);
                    }
                    else
                    {
                        //Validar duplicado en (BD)
                        var dtoEquipoRee = ObtenerReleqpryPorCriterio(entity, relActuales);
                        bool existeRegistroEnBD = dtoEquipoRee != null;

                        if (existeRegistroEnBD) // si existe duplicado y es nuevo
                        {
                            entity.Ftreqpcodi = dtoEquipoRee.Ftreqpcodi;
                            entity.Observaciones = "Se encontró coincidencia con registro existente. No se puede crear duplicados.";
                            lstRegEquiposErroneos.Add(entity);
                        }
                        else
                        {
                            lstRegEquiposCorrectos.Add(entity); // Es nuevo
                        }
                    }
                }
                else
                {
                    // Van los registros con formato incorrecto
                    entity.Observaciones = mensajeValidacion;

                    lstRegEquiposErroneos.Add(entity);
                }
            }

            #endregion

            listaNuevo = lstRegEquiposCorrectos.Where(x => x.Ftreqpcodi == 0).ToList(); // solo los nuevos
        }

        private static List<FilaExcelRelEquipoProyecto> ImportToDataTableRelEquipoProyecto(string filePath)
        {
            List<FilaExcelRelEquipoProyecto> listaMacro = new List<FilaExcelRelEquipoProyecto>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexItem = 1;
            int indexCodProyecto = indexItem + 1;
            int indexCodEquipo = indexItem + 4;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesEquipamientoAppServicio.HojaPlantillaExcel];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 11;

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var sItem = string.Empty;
                    if (worksheet.Cells[row, indexItem].Value != null) sItem = worksheet.Cells[row, indexItem].Value.ToString();
                    Int32.TryParse(sItem, out int numItem);

                    var sCodProyecto = string.Empty;
                    if (worksheet.Cells[row, indexCodProyecto].Value != null) sCodProyecto = worksheet.Cells[row, indexCodProyecto].Value.ToString();

                    var sCodEquipo = string.Empty;
                    if (worksheet.Cells[row, indexCodEquipo].Value != null) sCodEquipo = worksheet.Cells[row, indexCodEquipo].Value.ToString();

                    int ftprycodi = -2;
                    int equicodi = 0;
                    try
                    {
                        sCodProyecto = (sCodProyecto ?? "").Trim();
                        sCodEquipo = (sCodEquipo ?? "").Trim();

                        ftprycodi = (int)(((double?)worksheet.Cells[row, indexCodProyecto].Value) ?? 0);
                        equicodi = (int)(((double?)worksheet.Cells[row, indexCodEquipo].Value) ?? 0);
                    }
                    catch (Exception ex)
                    {
                        //No es necesario registrar el error de la conversión de datos de todas las celdas (pueden ocurrir como un máximo de 28 mil líneas de log para archivos de 7000 líneas de datos). 
                        //El tratamiento de estos errores se realiza en otra función (ValidarLecturaExcelEquipos de EquipamientoAppServicio) 
                        //que luego genera un .csv para el usuario (funcion GenerarArchivoEquiposErroneasCSV de EquipamientoAppServicio)
                    }

                    var regFila = new FilaExcelRelEquipoProyecto()
                    {
                        Row = row,
                        NumItem = numItem,

                        StrCodigoProyecto = sCodProyecto,
                        StrCodigoEquipo = sCodEquipo,

                        Ftprycodi = ftprycodi,
                        Equicodi = equicodi,
                    };

                    listaMacro.Add(regFila);
                }
            }

            return listaMacro;
        }

        private string ValidarLecturaExcelRelEquipoProyecto(FilaExcelRelEquipoProyecto filaExcel, List<EqEquipoDTO> listaEq, List<FtExtProyectoDTO> listaPry)
        {
            string columnCodProyecto = "CÓDIGO PROYECTO: ";
            string columnCodEquipo = "CÓDIGO DE EQUIPO: ";

            List<string> lMsgValidacion = new List<string>();

            //Validar codigo de Empresa
            if (String.IsNullOrEmpty(filaExcel.StrCodigoProyecto))
            {
                lMsgValidacion.Add(columnCodProyecto + "Esta vacío o en blanco");
            }
            else if (filaExcel.Ftprycodi < 0)
            {
                lMsgValidacion.Add(columnCodProyecto + "No es número válido");
            }
            else
            {
                FtExtProyectoDTO regPry = listaPry.Find(x => x.Ftprycodi == filaExcel.Ftprycodi);
                if (regPry == null)
                {
                    lMsgValidacion.Add(columnCodProyecto + "Código de Proyecto no existe");
                }
                else
                {
                    filaExcel.StrEmpresaProyecto = regPry.Emprnomb;
                    filaExcel.StrCodigoEstudio = regPry.Ftpryeocodigo;
                }
            }

            //Validar Código de Tipo de equipo
            if (String.IsNullOrEmpty(filaExcel.StrCodigoEquipo))
            {
                lMsgValidacion.Add(columnCodEquipo + "Esta vacío o en blanco");
            }
            else if (filaExcel.Equicodi < 0)
            {
                lMsgValidacion.Add(columnCodEquipo + "No es número válido");
            }
            else
            {
                EqEquipoDTO regEq = listaEq.Find(x => x.Equicodi == filaExcel.Equicodi);
                if (regEq == null)
                {
                    lMsgValidacion.Add(columnCodEquipo + "Código de Equipo no existe");
                }
                else
                {
                    filaExcel.StrNombreEquipo = regEq.Equinomb;
                    filaExcel.StrAbreviaturaEq = regEq.Equiabrev;
                    filaExcel.StrEstadoEq = regEq.EquiestadoDesc;
                    filaExcel.StrEmpresaEq = regEq.Emprnomb;
                    filaExcel.StrTipoEquipo = regEq.Famnomb;
                    filaExcel.StrUbicacion = regEq.Areanomb;
                }
            }

            return string.Join(". ", lMsgValidacion);
        }

        private FtExtReleqpryDTO ObtenerReleqpryPorCriterio(FtExtReleqpryDTO entity, List<FtExtReleqpryDTO> listaRel)
        {
            FtExtReleqpryDTO dto = listaRel.Where(x => x.Equicodi == entity.Equicodi && x.Ftprycodi == entity.Ftprycodi).FirstOrDefault();

            return dto;
        }

        public string GenerarArchivoRelEquipoProyectoErroneosCSV(string path, List<FtExtReleqpryDTO> lstRegEquiposErroneos)
        {
            string sLine = string.Empty;
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameCsv = "";
            fileNameCsv = sFecha + "_LogEquiposImport" + ".csv";

            using (var dbProviderWriter = new StreamWriter(new FileStream(path + fileNameCsv, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sLine = "ÍTEM;OBSERVACIONES;CÓDIGO PROYECTO;Empresa PROYECTO;Código Estudio (EO);CÓDIGO DE EQUIPO;NOMBRE DE EQUIPO;ABREVIATURA;ESTADO;EMPRESA;TIPO DE EQUIPO;UBICACIÓN";
                dbProviderWriter.WriteLine(sLine);
                foreach (FtExtReleqpryDTO entity in lstRegEquiposErroneos)
                {
                    sLine = this.CreateFilaErroneaRelEquipoProyectoString(entity);
                    dbProviderWriter.WriteLine(sLine);
                }
                dbProviderWriter.Close();
            }
            return fileNameCsv;
        }

        private string CreateFilaErroneaRelEquipoProyectoString(FtExtReleqpryDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            sLine += (entity.Ftprycodi) + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += (entity.Emprnomb) + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += (entity.Ftpryeocodigo) + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += (entity.Equicodi) + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equinomb != null) ? entity.Equinomb.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equiabrev != null) ? entity.Equiabrev.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equiestadodesc != null) ? entity.Equiestadodesc.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Emprnomb2 != "") ? entity.Emprnomb2.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Famnomb != null) ? entity.Famnomb.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Areanomb != null) ? entity.Areanomb.ToString() : "") + ConstantesEquipamientoAppServicio.SeparadorCampoCSV;

            return sLine;
        }

        public void GuardarDatosMasivosRelEquipoProyecto(List<FtExtReleqpryDTO> listaNuevo, string usuario)
        {
            var UoW = FactorySic.UnitOfWork();
            using (var connection = UoW.BeginConnection())
            {
                using (var transaction = UoW.StartTransaction(connection))
                {
                    try
                    {
                        //Guarda Registros nuevos masivamente
                        if (listaNuevo != null && listaNuevo.Any())
                        {
                            foreach (var item in listaNuevo)
                            {
                                int cod1 = FactorySic.GetFtExtReleqpryRepository().Save(item, connection, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region GESPROTEC-20241031
        /// <summary>
        /// Devuelve el listado de equipos COES
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaEquipoCOES(String idUbicacion, String idTipoEquipo, String nombreEquipo, string sProgramaExistente)
        {
            return FactorySic.GetEqEquipoRepository().ListaEquipoCOES(idUbicacion, idTipoEquipo, nombreEquipo, sProgramaExistente);
        }

        /// <summary>
        /// Devuelve el listado de Areas del COES
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> ListEqAreasEquipamientoCOES()
        {
            return FactorySic.GetEqAreaRepository().ListAreaEquipamientoCOES();
        }

        /// <summary>
        /// Devuelve el listado de Familias del COES
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListEqFamiliasEquipamientoCOES()
        {
            return FactorySic.GetEqFamiliaRepository().ListFamiliaEquipamientoCOES();
        }

        /// <summary>
        /// Devuelve el listado para exportar excel de equipos COES
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListaExportarEquipoCOES(string idUbicacion, string idTipoEquipo, string nombreEquipo)
        {
            return FactorySic.GetEqEquipoRepository().ListaExportarEquipoCOES(idUbicacion, idTipoEquipo, nombreEquipo);
        }
        
        /// <summary>
        /// Devuelve el listado de los tipos para el formulario ubicación
        /// </summary>
        /// <returns></returns>
        public List<EqTipoareaDTO> ListProtecciones()
        {
            return FactorySic.GetEqTipoareaRepository().ListProtecciones();
        }
        #endregion
    }
}
