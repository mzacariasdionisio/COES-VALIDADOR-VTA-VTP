using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Collections;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.OperacionesVarias;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico.Helper;
using System.Globalization;
using System.Text;


namespace COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico
{
    /// <summary>
    /// Clases con métodos del módulo ReservaFriaNodoEnergetico
    /// </summary>
    public class ReservaFriaNodoEnergeticoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReservaFriaNodoEnergeticoAppServicio));

        #region Métodos Tabla NR_CONCEPTO

        /// <summary>
        /// Inserta un registro de la tabla NR_CONCEPTO
        /// </summary>
        public void SaveNrConcepto(NrConceptoDTO entity)
        {
            try
            {
                FactorySic.GetNrConceptoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_CONCEPTO
        /// </summary>
        public void UpdateNrConcepto(NrConceptoDTO entity)
        {
            try
            {
                FactorySic.GetNrConceptoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_CONCEPTO
        /// </summary>
        public void DeleteNrConcepto(int nrcptcodi)
        {
            try
            {
                FactorySic.GetNrConceptoRepository().Delete(nrcptcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_CONCEPTO
        /// </summary>
        public NrConceptoDTO GetByIdNrConcepto(int nrcptcodi)
        {
            return FactorySic.GetNrConceptoRepository().GetById(nrcptcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_CONCEPTO
        /// </summary>
        public List<NrConceptoDTO> ListNrConceptos()
        {
            return FactorySic.GetNrConceptoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_CONCEPTO y NR_SUBMODULO
        /// </summary>
        public List<NrConceptoDTO> ListNrSubModuloConcepto()
        {
            return FactorySic.GetNrConceptoRepository().ListSubModuloConcepto();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrConcepto
        /// </summary>
        public List<NrConceptoDTO> GetByCriteriaNrConceptos()
        {
            return FactorySic.GetNrConceptoRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla NR_CONCEPTO
        /// </summary>
        public int SaveNrConceptoId(NrConceptoDTO entity)
        {
            return FactorySic.GetNrConceptoRepository().SaveNrConceptoId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_CONCEPTO
        /// </summary>
        public List<NrConceptoDTO> BuscarOperaciones(int nrsmodCodi, int nroPage, int pageSize)
        {
            return FactorySic.GetNrConceptoRepository().BuscarOperaciones(nrsmodCodi, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_CONCEPTO
        /// </summary>
        public int ObtenerNroFilas(int nrsmodCodi)
        {
            return FactorySic.GetNrConceptoRepository().ObtenerNroFilas(nrsmodCodi);
        }

        #endregion

        #region Métodos Tabla SI_PARAMETRO

        /// <summary>
        /// Inserta un registro de la tabla SI_PARAMETRO
        /// </summary>
        public void SaveSiParametro(SiParametroDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PARAMETRO
        /// </summary>
        public void UpdateSiParametro(SiParametroDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PARAMETRO
        /// </summary>
        public void DeleteSiParametro(int siparcodi)
        {
            try
            {
                FactorySic.GetSiParametroRepository().Delete(siparcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PARAMETRO
        /// </summary>
        public SiParametroDTO GetByIdSiParametro(int siparcodi)
        {
            return FactorySic.GetSiParametroRepository().GetById(siparcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PARAMETRO
        /// </summary>
        public List<SiParametroDTO> ListSiParametros()
        {
            return FactorySic.GetSiParametroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiParametro
        /// </summary>
        public List<SiParametroDTO> GetByCriteriaSiParametros()
        {
            return FactorySic.GetSiParametroRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SI_PARAMETRO
        /// </summary>
        public int SaveSiParametroId(SiParametroDTO entity)
        {
            return FactorySic.GetSiParametroRepository().SaveSiParametroId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SI_PARAMETRO
        /// </summary>
        public List<SiParametroDTO> BuscarOperaciones(string abreviatura, string descripcion, int nroPage, int pageSize)
        {
            return FactorySic.GetSiParametroRepository().BuscarOperaciones(abreviatura, descripcion, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SI_PARAMETRO
        /// </summary>
        public int ObtenerNroFilas(string abreviatura, string descripcion)
        {
            return FactorySic.GetSiParametroRepository().ObtenerNroFilas(abreviatura, descripcion);
        }

        #endregion

        #region Métodos Tabla SI_PARAMETRO_VALOR

        /// <summary>
        /// Inserta un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public void SaveSiParametroValor(SiParametroValorDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroValorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public void UpdateSiParametroValor(SiParametroValorDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroValorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public void DeleteSiParametroValor(int siparvcodi)
        {
            try
            {
                FactorySic.GetSiParametroValorRepository().Delete(siparvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public SiParametroValorDTO GetByIdSiParametroValor(int siparvcodi)
        {
            return FactorySic.GetSiParametroValorRepository().GetById(siparvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public List<SiParametroValorDTO> ListSiParametroValors()
        {
            return FactorySic.GetSiParametroValorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiParametroValor
        /// </summary>
        public List<SiParametroValorDTO> GetByCriteriaSiParametroValors()
        {
            return FactorySic.GetSiParametroValorRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public int SaveSiParametroValorId(SiParametroValorDTO entity)
        {
            return FactorySic.GetSiParametroValorRepository().SaveSiParametroValorId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public List<SiParametroValorDTO> BuscarOperaciones(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, int nroPage, int pageSize, string estado)
        {
            return FactorySic.GetSiParametroValorRepository().BuscarOperaciones(siparCodi, siparvFechaInicial, siparvFechaFinal, nroPage, pageSize, estado);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public int ObtenerNroFilas(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, string estado)
        {
            return FactorySic.GetSiParametroValorRepository().ObtenerNroFilas(siparCodi, siparvFechaInicial, siparvFechaFinal, estado);
        }

        #endregion

        #region Métodos Tabla NR_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla NR_PERIODO
        /// </summary>
        public void SaveNrPeriodo(NrPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetNrPeriodoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_PERIODO
        /// </summary>
        public void UpdateNrPeriodo(NrPeriodoDTO entity)
        {
            try
            {
                FactorySic.GetNrPeriodoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_PERIODO
        /// </summary>
        public void DeleteNrPeriodo(int nrpercodi)
        {
            try
            {
                FactorySic.GetNrPeriodoRepository().Delete(nrpercodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_PERIODO
        /// </summary>
        public NrPeriodoDTO GetByIdNrPeriodo(int nrpercodi)
        {
            return FactorySic.GetNrPeriodoRepository().GetById(nrpercodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_PERIODO
        /// </summary>
        public List<NrPeriodoDTO> ListNrPeriodos()
        {
            return FactorySic.GetNrPeriodoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrPeriodo
        /// </summary>
        public List<NrPeriodoDTO> GetByCriteriaNrPeriodos()
        {
            return FactorySic.GetNrPeriodoRepository().GetByCriteria();
        }

        /// <summary>
        /// Valida si existe el periodo y es vigente. Tabla NR_PERIODO
        /// </summary>
        public bool ExistePeriodoId(DateTime periodo)
        {
            List<NrPeriodoDTO> listaPeriodo = FactorySic.GetNrPeriodoRepository().GetByCriteria();

            //listaPeriodo = listaPeriodo.Where(x => x.Nrpermes == periodo && x.Nrpereliminado == "N").ToList();
            listaPeriodo = listaPeriodo.Where(x => x.Nrpermes == periodo).ToList();

            return (listaPeriodo.Count > 0);
        }


        /// <summary>
        /// Graba los datos de la tabla NR_PERIODO
        /// </summary>
        public int SaveNrPeriodoId(NrPeriodoDTO entity)
        {
            return FactorySic.GetNrPeriodoRepository().SaveNrPeriodoId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_PERIODO
        /// </summary>
        public List<NrPeriodoDTO> BuscarOperaciones(string estado, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetNrPeriodoRepository().BuscarOperaciones(estado, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_PERIODO
        /// </summary>
        public int ObtenerNroFilas(string estado, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactorySic.GetNrPeriodoRepository().ObtenerNroFilas(estado, fechaInicio, fechaFinal);
        }

        #endregion

        #region Métodos Tabla NR_PERIODO_RESUMEN

        /// <summary>
        /// Inserta un registro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public void SaveNrPeriodoResumen(NrPeriodoResumenDTO entity)
        {
            try
            {
                FactorySic.GetNrPeriodoResumenRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public void UpdateNrPeriodoResumen(NrPeriodoResumenDTO entity)
        {
            try
            {
                FactorySic.GetNrPeriodoResumenRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public void DeleteNrPeriodoResumen(int nrperrcodi)
        {
            try
            {
                FactorySic.GetNrPeriodoResumenRepository().Delete(nrperrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public NrPeriodoResumenDTO GetByIdNrPeriodoResumen(int nrperrcodi)
        {
            return FactorySic.GetNrPeriodoResumenRepository().GetById(nrperrcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public NrPeriodoResumenDTO GetByIdNrPeriodoResumen(int nrpercodi, int nrcptcodi)
        {
            return FactorySic.GetNrPeriodoResumenRepository().GetById(nrpercodi, nrcptcodi);
        }


        /// <summary>
        /// Permite listar todos los registros de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public List<NrPeriodoResumenDTO> ListNrPeriodoResumens()
        {
            return FactorySic.GetNrPeriodoResumenRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public List<NrPeriodoResumenDTO> ListNrPeriodoResumens(int idNrsmodCodi, int idNrperCodi)
        {
            return FactorySic.GetNrPeriodoResumenRepository().List(idNrsmodCodi, idNrperCodi);
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla NrPeriodoResumen
        /// </summary>
        public List<NrPeriodoResumenDTO> GetByCriteriaNrPeriodoResumens()
        {
            return FactorySic.GetNrPeriodoResumenRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public int SaveNrPeriodoResumenId(NrPeriodoResumenDTO entity)
        {
            return FactorySic.GetNrPeriodoResumenRepository().SaveNrPeriodoResumenId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public List<NrPeriodoResumenDTO> BuscarOperaciones(int nrsmodCodi, string estado, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetNrPeriodoResumenRepository().BuscarOperaciones(nrsmodCodi, estado, fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_PERIODO_RESUMEN
        /// </summary>
        public int ObtenerNroFilas(int nrsmodCodi, string estado, DateTime fechaInicio, DateTime fechaFinal)
        {
            return FactorySic.GetNrPeriodoResumenRepository().ObtenerNroFilas(nrsmodCodi, estado, fechaInicio, fechaFinal);
        }

        #endregion

        #region Métodos Tabla NR_POTENCIACONSIGNA

        /// <summary>
        /// Inserta un registro de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public void SaveNrPotenciaconsigna(NrPotenciaconsignaDTO entity)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public void UpdateNrPotenciaconsigna(NrPotenciaconsignaDTO entity)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public void DeleteNrPotenciaconsigna(int nrpccodi)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaRepository().Delete(nrpccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public NrPotenciaconsignaDTO GetByIdNrPotenciaconsigna(int nrpccodi)
        {
            return FactorySic.GetNrPotenciaconsignaRepository().GetById(nrpccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public List<NrPotenciaconsignaDTO> ListNrPotenciaconsignas()
        {
            return FactorySic.GetNrPotenciaconsignaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrPotenciaconsigna
        /// </summary>
        public List<NrPotenciaconsignaDTO> GetByCriteriaNrPotenciaconsignas()
        {
            return FactorySic.GetNrPotenciaconsignaRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public int SaveNrPotenciaconsignaId(NrPotenciaconsignaDTO entity)
        {
            return FactorySic.GetNrPotenciaconsignaRepository().SaveNrPotenciaconsignaId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public List<NrPotenciaconsignaDTO> BuscarOperaciones(int nrsmodCodi, int grupoCodi, DateTime nrpcFechaIni, DateTime nrpcFechaFin, string estado, int nroPage, int pageSize)
        {
            return FactorySic.GetNrPotenciaconsignaRepository().BuscarOperaciones(nrsmodCodi, grupoCodi, nrpcFechaIni, nrpcFechaFin, estado, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_POTENCIACONSIGNA
        /// </summary>
        public int ObtenerNroFilas(int nrsmodCodi, int grupoCodi, DateTime fechaInicio, DateTime fechaFinal, string estado)
        {
            return FactorySic.GetNrPotenciaconsignaRepository().ObtenerNroFilas(nrsmodCodi, grupoCodi, fechaInicio, fechaFinal, estado);
        }

        #endregion

        #region Métodos Tabla NR_POTENCIACONSIGNA_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public void SaveNrPotenciaconsignaDetalle(NrPotenciaconsignaDetalleDTO entity)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public void UpdateNrPotenciaconsignaDetalle(NrPotenciaconsignaDetalleDTO entity)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public void DeleteNrPotenciaconsignaDetalle(int nrpcdcodi)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaDetalleRepository().Delete(nrpcdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina todos los registro de la tabla NR_POTENCIACONSIGNA_DETALLE relacionados a tabla NR_POTENCIACONSIGNA
        /// </summary>
        public void DeleteNrPotenciaconsignaDetalleTotal(int nrpccodi)
        {
            try
            {
                FactorySic.GetNrPotenciaconsignaDetalleRepository().DeleteTotal(nrpccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public NrPotenciaconsignaDetalleDTO GetByIdNrPotenciaconsignaDetalle(int nrpcdcodi)
        {
            return FactorySic.GetNrPotenciaconsignaDetalleRepository().GetById(nrpcdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public List<NrPotenciaconsignaDetalleDTO> ListNrPotenciaconsignaDetalles()
        {
            return FactorySic.GetNrPotenciaconsignaDetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrPotenciaconsignaDetalle
        /// </summary>
        public List<NrPotenciaconsignaDetalleDTO> GetByCriteriaNrPotenciaconsignaDetalles()
        {
            return FactorySic.GetNrPotenciaconsignaDetalleRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public int SaveNrPotenciaconsignaDetalleId(NrPotenciaconsignaDetalleDTO entity)
        {
            return FactorySic.GetNrPotenciaconsignaDetalleRepository().SaveNrPotenciaconsignaDetalleId(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public List<NrPotenciaconsignaDetalleDTO> BuscarOperaciones(int nrpcCodi, DateTime nrpcdFecha, DateTime nrpcdFecCreacion, int nroPage, int pageSize)
        {
            return FactorySic.GetNrPotenciaconsignaDetalleRepository().BuscarOperaciones(nrpcCodi, nrpcdFecha, nrpcdFecCreacion, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_POTENCIACONSIGNA_DETALLE
        /// </summary>
        public int ObtenerNroFilas(int nrpcCodi, DateTime nrpcdFecha, DateTime nrpcdFecCreacion)
        {
            return FactorySic.GetNrPotenciaconsignaDetalleRepository().ObtenerNroFilas(nrpcCodi, nrpcdFecha, nrpcdFecCreacion);
        }

        #endregion

        #region Métodos Tabla NR_PROCESO

        /// <summary>
        /// Inserta un registro de la tabla NR_PROCESO
        /// </summary>
        public void SaveNrProceso(NrProcesoDTO entity)
        {
            try
            {
                FactorySic.GetNrProcesoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_PROCESO
        /// </summary>
        public void UpdateNrProceso(NrProcesoDTO entity)
        {
            try
            {
                FactorySic.GetNrProcesoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_PROCESO
        /// </summary>
        public void DeleteNrProceso(int nrprccodi)
        {
            try
            {
                FactorySic.GetNrProcesoRepository().Delete(nrprccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registro de la tabla NR_PROCESO según periodo y concepto
        /// </summary>
        public void DeleteNrProceso(int nrpercodi, int nrcptcodi)
        {
            try
            {
                FactorySic.GetNrProcesoRepository().Delete(nrpercodi, nrcptcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_PROCESO
        /// </summary>
        public NrProcesoDTO GetByIdNrProceso(int nrprccodi)
        {
            return FactorySic.GetNrProcesoRepository().GetById(nrprccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_PROCESO
        /// </summary>
        public List<NrProcesoDTO> ListNrProcesos()
        {
            return FactorySic.GetNrProcesoRepository().List();
        }

        /// <summary>
        /// Permite listar todas las observaciones automaticas de la tabla NR_PROCESO
        /// </summary>
        public string ListNrProcesosObservaciones(int nrperCodi)
        {
            return FactorySic.GetNrProcesoRepository().ListObservaciones(nrperCodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrProceso
        /// </summary>
        public List<NrProcesoDTO> GetByCriteriaNrProcesos()
        {
            return FactorySic.GetNrProcesoRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla NR_PROCESO
        /// </summary>
        public int SaveNrProcesoId(NrProcesoDTO entity)
        {
            return FactorySic.GetNrProcesoRepository().SaveNrProcesoId(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_PROCESO
        /// </summary>
        public List<NrProcesoDTO> BuscarOperaciones(string estado, int nrperCodi, int grupoCodi, int nrcptCodi, DateTime nrprcFechaInicio, DateTime nrprcFechaFin, int nroPage, int pageSize)
        {
            return FactorySic.GetNrProcesoRepository()
                .BuscarOperaciones(estado, nrperCodi, grupoCodi, nrcptCodi, nrprcFechaInicio, nrprcFechaFin, nroPage,
                    pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_PROCESO
        /// </summary>
        public int ObtenerNroFilas(string estado, int nrperCodi, int grupoCodi, int nrcptCodi, DateTime nrprcFechaInicio, DateTime nrprcFechaFin)
        {
            return FactorySic.GetNrProcesoRepository()
                .ObtenerNroFilas(estado, nrperCodi, grupoCodi, nrcptCodi, nrprcFechaInicio, nrprcFechaFin);
        }

        #endregion

        #region Métodos Tabla NR_SOBRECOSTO

        /// <summary>
        /// Inserta un registro de la tabla NR_SOBRECOSTO
        /// </summary>
        public void SaveNrSobrecosto(NrSobrecostoDTO entity)
        {
            try
            {
                FactorySic.GetNrSobrecostoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_SOBRECOSTO
        /// </summary>
        public void UpdateNrSobrecosto(NrSobrecostoDTO entity)
        {
            try
            {
                FactorySic.GetNrSobrecostoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_SOBRECOSTO
        /// </summary>
        public void DeleteNrSobrecosto(int nrsccodi)
        {
            try
            {
                FactorySic.GetNrSobrecostoRepository().Delete(nrsccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_SOBRECOSTO
        /// </summary>
        public NrSobrecostoDTO GetByIdNrSobrecosto(int nrsccodi)
        {
            return FactorySic.GetNrSobrecostoRepository().GetById(nrsccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_SOBRECOSTO
        /// </summary>
        public List<NrSobrecostoDTO> ListNrSobrecostos()
        {
            return FactorySic.GetNrSobrecostoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrSobrecosto
        /// </summary>
        public List<NrSobrecostoDTO> GetByCriteriaNrSobrecostos()
        {
            return FactorySic.GetNrSobrecostoRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla NR_SOBRECOSTO
        /// </summary>
        public int SaveNrSobrecostoId(NrSobrecostoDTO entity)
        {
            return FactorySic.GetNrSobrecostoRepository().SaveNrSobrecostoId(entity);
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla NR_SOBRECOSTO
        /// </summary>
        //public List<NrSobrecostoDTO> BuscarOperaciones(int grupoCodi, DateTime nrscFechaIni, DateTime nrscFechaFin, string estado, int nroPage, int pageSize)
        public List<NrSobrecostoDTO> BuscarOperaciones(DateTime nrscFechaIni, DateTime nrscFechaFin, string estado, int nroPage, int pageSize)
        {
            //return FactorySic.GetNrSobrecostoRepository().BuscarOperaciones(grupoCodi, nrscFechaIni, nrscFechaFin, estado, nroPage, pageSize);
            return FactorySic.GetNrSobrecostoRepository()
                .BuscarOperaciones(nrscFechaIni, nrscFechaFin, estado, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla NR_SOBRECOSTO
        /// </summary>
        //public int ObtenerNroFilas(int grupoCodi, DateTime nrscFechaIni, DateTime nrscFechaFin, string estado)
        public int ObtenerNroFilas(DateTime nrscFechaIni, DateTime nrscFechaFin, string estado)
        {
            //return FactorySic.GetNrSobrecostoRepository().ObtenerNroFilas(grupoCodi, nrscFechaIni, nrscFechaFin, estado);
            return FactorySic.GetNrSobrecostoRepository().ObtenerNroFilas(nrscFechaIni, nrscFechaFin, estado);
        }

        #endregion

        #region Métodos Tabla NR_SUBMODULO

        /// <summary>
        /// Inserta un registro de la tabla NR_SUBMODULO
        /// </summary>
        public void SaveNrSubmodulo(NrSubmoduloDTO entity)
        {
            try
            {
                FactorySic.GetNrSubmoduloRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla NR_SUBMODULO
        /// </summary>
        public void UpdateNrSubmodulo(NrSubmoduloDTO entity)
        {
            try
            {
                FactorySic.GetNrSubmoduloRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla NR_SUBMODULO
        /// </summary>
        public void DeleteNrSubmodulo(int nrsmodcodi)
        {
            try
            {
                FactorySic.GetNrSubmoduloRepository().Delete(nrsmodcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla NR_SUBMODULO
        /// </summary>
        public NrSubmoduloDTO GetByIdNrSubmodulo(int nrsmodcodi)
        {
            return FactorySic.GetNrSubmoduloRepository().GetById(nrsmodcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla NR_SUBMODULO
        /// </summary>
        public List<NrSubmoduloDTO> ListNrSubmodulos()
        {
            return FactorySic.GetNrSubmoduloRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla NrSubmodulo
        /// </summary>
        public List<NrSubmoduloDTO> GetByCriteriaNrSubmodulos()
        {
            return FactorySic.GetNrSubmoduloRepository().GetByCriteria();
        }

        #endregion

        #region Métodos de Procesamiento

        /// <summary>
        /// Permite obtener una fecha final según el periodo elegido
        /// </summary>
        /// <param name="fecha">fecha</param>
        /// <returns></returns>
        public DateTime ObtenerFechaFinal(DateTime fecha)
        {
            DateTime fechaFinal = fecha.AddDays(35);

            fechaFinal =
                DateTime.ParseExact(
                    "01/" + (fechaFinal.Month < 10 ? "0" : "") + fechaFinal.Month + "/" + fechaFinal.Year + " 23:59",
                    ConstantesReservaFriaNodoEnergetico.FormatoFechaHora, CultureInfo.InvariantCulture);
            fechaFinal = fechaFinal.AddDays(-1);

            return fechaFinal;
        }

        /// <summary>
        /// Permite procesar los conceptos por periodo
        /// </summary>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <returns></returns>
        public string ProcesarConceptoPeriodo(int nrcptcodi, int nrperCodi, string usuario)
        {
            int retorno = -1;
            string retornoFinal = "-1";

            NrConceptoDTO concepto = GetByIdNrConcepto(nrcptcodi);
            NrPeriodoDTO periodo = GetByIdNrPeriodo(nrperCodi);

            DateTime fechaInicio = periodo.Nrpermes;
            DateTime fechaFinal = ObtenerFechaFinal(fechaInicio);

            List<PrGrupoDTO> listaPrGrupo = FactorySic.GetPrGrupoRepository().List();

            List<PrGrupoDTO> listaReservaFria = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaNodoEnergetico = new List<PrGrupoDTO>();

            List<MePtomedicionDTO> listaPtoMedicion =
                FactorySic.GetMePtomedicionRepository().ListByOriglectcodi(ConstantesReservaFriaNodoEnergetico.OrigenLecturaDespacho + "," +
                                   ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores + "," +
                                   ConstantesReservaFriaNodoEnergetico.OrigenLecturaStock + "," +
                                   ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico + "," +
                                   ConstantesReservaFriaNodoEnergetico.OrigenLecturaReservaFria + "," +
                                   ConstantesReservaFriaNodoEnergetico.OrigenLecturaCombProgramado, DateTime.Now, DateTime.Now);

            List<ModoGrupoCentralEquipo> listaEquivalenciaReservaFria = new List<ModoGrupoCentralEquipo>();
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico = new List<ModoGrupoCentralEquipo>();


            if (concepto.Nrsmodcodi == ConstantesReservaFriaNodoEnergetico.ModuloReservaFria)
            {
                //equipos de reserva fría
                listaReservaFria = listaPrGrupo.Where(x => x.Gruporeservafria == 1 && x.Catecodi == 2).ToList();

                //mapeo de equivalencias //Modo - Grupo - Central - Equipo - Ptomedicodi
                listaEquivalenciaReservaFria = ObtenerEquivalenciaModoGrupoCentralEquipo(listaReservaFria,
                    listaPrGrupo, listaPtoMedicion);
            }
            else
            {
                if (concepto.Nrsmodcodi == ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico)
                {
                    //equipos de nodo energetico
                    listaNodoEnergetico = listaPrGrupo.Where(x => x.Gruponodoenergetico == 1).ToList();

                    //mapeo de equivalencias //Modo - Grupo - Central - Equipo - Ptomedicodi
                    listaEquivalenciaNodoEnergetico = ObtenerEquivalenciaModoGrupoCentralEquipo(listaNodoEnergetico,
                        listaPrGrupo, listaPtoMedicion);
                }
            }


            //RESERVA FRÍA
            if (concepto.Nrsmodcodi == ConstantesReservaFriaNodoEnergetico.ModuloReservaFria)
            {
                switch (nrcptcodi)
                {
                    //Horas de demora en el arranque
                    case ConstantesReservaFriaNodoEnergetico.RfDemoraEnElArranque:
                        retorno = CalcularRfDemoraArranque(nrcptcodi, nrperCodi, fechaInicio, fechaFinal,
                            listaReservaFria, listaEquivalenciaReservaFria, usuario);

                        //cod1:obs1
                        retornoFinal = (retorno == -1
                            ? retorno.ToString()
                            : ConstantesReservaFriaNodoEnergetico.RfDemoraEnElArranque + ":" + retorno);
                        break;
                    //Horas de Mantenimiento Programado Ejecutado y Correctivo Ejecutado
                    case ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoProgramadoEjecutado:
                    case ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoCorrectivoEjecutado:
                        retorno = CalcularRfNeHorasMantenimiento(nrperCodi, fechaInicio, fechaFinal,
                            listaReservaFria, listaEquivalenciaReservaFria,
                            ConstantesReservaFriaNodoEnergetico.ConceptoMantoProgEjecutado,
                            ConstantesReservaFriaNodoEnergetico.ConceptoMantoCorrEjecutado,
                            ConstantesReservaFriaNodoEnergetico.ToleranciaReservaFria, usuario);

                        //cod1:obs1,cod2:obs2
                        retornoFinal = (retorno == -1
                            ? retorno.ToString()
                            : ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoProgramadoEjecutado + ":" +
                              retorno + "," +
                              ConstantesReservaFriaNodoEnergetico.RfHorasMantenimientoCorrectivoEjecutado + ":" +
                              retorno);
                        break;

                    //Horas de EDE
                    case ConstantesReservaFriaNodoEnergetico.RfHorasEnergiaDejadaDeEntregar:
                        retorno = CalcularRfHorasEde(nrcptcodi, nrperCodi, fechaInicio, fechaFinal, listaReservaFria,
                            listaEquivalenciaReservaFria, usuario);

                        retornoFinal = (retorno == -1
                            ? retorno.ToString()
                            : ConstantesReservaFriaNodoEnergetico.RfHorasEnergiaDejadaDeEntregar + ":" + retorno);
                        break;

                    //Energia dejada de entregar
                    case ConstantesReservaFriaNodoEnergetico.RfEnergiaDejadaDeEntregar:
                        retorno = CalcularRfEde(nrcptcodi, nrperCodi, fechaInicio, fechaFinal, listaPrGrupo,
                            listaEquivalenciaReservaFria,
                            ConstantesReservaFriaNodoEnergetico.RfHorasEnergiaDejadaDeEntregar,
                            ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeReservaFria);

                        retornoFinal = (retorno == -1
                            ? retorno.ToString()
                            : ConstantesReservaFriaNodoEnergetico.RfEnergiaDejadaDeEntregar + ":" + retorno);
                        break;
                }
            }
            else
            {
                //NODO ENERGETICO
                if (concepto.Nrsmodcodi == ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico)
                {
                    switch (nrcptcodi)
                    {
                        //Horas de indisponibilidad Total Fortuita y Total Programada (P1-P2)
                        case ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada://hmpe
                        case ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalFortuita://hmce
                            retorno = CalcularRfNeHorasMantenimiento(nrperCodi, fechaInicio, fechaFinal,
                                listaNodoEnergetico, listaEquivalenciaNodoEnergetico,
                                ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada,
                                ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalFortuita,
                                ConstantesReservaFriaNodoEnergetico.ToleranciaReservaNodoEnergetico, usuario);

                            //cod1:obs1,cod2:obs2
                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada + ":" + retorno + "," +
                                  ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalFortuita + ":" + retorno);


                            break;

                        //Horas de Indisponibilidad Total fortuita y Parcial Fortuita (P3)
                        case ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada:
                        case ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita:
                            retorno = CalcularNeHorasIndisponParcialFortProg(nrperCodi, fechaInicio, fechaFinal,
                                listaNodoEnergetico, listaEquivalenciaNodoEnergetico, usuario);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada + ":" + retorno +
                                  "," +
                                  ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita + ":" + retorno);
                            break;

                        //EDE
                        case ConstantesReservaFriaNodoEnergetico.NeEdeIndispTotalFortuita:
                            retorno = CalcularNeEdeTotalFortuita(nrperCodi,
                                ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalFortuita, fechaInicio,
                                fechaFinal, listaNodoEnergetico, listaEquivalenciaNodoEnergetico, (int)concepto.Nrsmodcodi,
                                ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalFort);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeEdeIndispTotalFortuita + ":" + retorno);
                            break;
                        case ConstantesReservaFriaNodoEnergetico.NeEdeIndispTotalProgramada:
                            retorno = CalcularNeEdeTotalParcialProgramada(nrperCodi,
                                ConstantesReservaFriaNodoEnergetico.NeHorasIndispTotalProgramada, fechaInicio,
                                fechaFinal, listaNodoEnergetico,
                                listaEquivalenciaNodoEnergetico, (int)concepto.Nrsmodcodi,
                                ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalProg);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeEdeIndispTotalProgramada + ":" + retorno);
                            break;
                        case ConstantesReservaFriaNodoEnergetico.NeEdeIndispParcialFortuita:
                            retorno = CalcularNeEdeParcialFortuita(nrperCodi,
                                ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita, fechaInicio,
                                fechaFinal, listaNodoEnergetico, listaEquivalenciaNodoEnergetico, (int)concepto.Nrsmodcodi,
                                ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcFort);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeEdeIndispParcialFortuita + ":" + retorno);
                            break;
                        case ConstantesReservaFriaNodoEnergetico.NeEdeIndispParcialProgramada:
                            retorno = CalcularNeEdeTotalParcialProgramada(nrperCodi,
                                ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada, fechaInicio,
                                fechaFinal, listaNodoEnergetico,
                                listaEquivalenciaNodoEnergetico, (int)concepto.Nrsmodcodi,
                                ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcProg);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeEdeIndispParcialProgramada + ":" + retorno);
                            break;
                        case ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispTotalFortuita:
                            retorno = CalcularNeSobrecosto(nrcptcodi, nrperCodi, fechaInicio, fechaFinal,
                                ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalFort, listaPtoMedicion, usuario);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispTotalFortuita + ":" + retorno);
                            break;
                        case ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispParcialFortuita:
                            retorno = CalcularNeSobrecosto(nrcptcodi, nrperCodi, fechaInicio, fechaFinal,
                                ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispParcFort, listaPtoMedicion, usuario);

                            retornoFinal = (retorno == -1
                                ? retorno.ToString()
                                : ConstantesReservaFriaNodoEnergetico.NeSobrecostoIndispParcialFortuita + ":" + retorno);
                            break;
                    }
                }
            }

            return retornoFinal;

        }


        /// <summary>
        /// Permite calcular el sobrecosto del Nodo Energético
        /// </summary>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="lectcodi96">Código de lectura</param>
        /// <param name="listaPtoMedicion">Listado de punto de medición</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        public int CalcularNeSobrecosto(int nrcptcodi, int nrperCodi, DateTime fechaInicio, DateTime fechaFinal,
            int lectcodi96, List<MePtomedicionDTO> listaPtoMedicion, string usuario)
        {
            try
            {
                int numObservaciones = 0;

                DeleteNrProceso(nrperCodi, nrcptcodi);

                //lista de ede basado en MeMedicion96DTO
                List<MeMedicion96DTO> listaEde96 =
                    FactorySic.GetMeMedicion96Repository().GetByCriteria(ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                        Convert.ToInt32(ConstantesReservaFriaNodoEnergetico.ParametroTodos),
                        lectcodi96, fechaInicio, fechaFinal);

                List<NrSobrecostoDTO> listaSobrecosto =
                    BuscarOperaciones(fechaInicio, fechaFinal,
                        "N",
                        -1, -1);

                List<MePtomedicionDTO> listaNePtomedicion =
                    listaPtoMedicion.Where(
                        x => x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico).ToList();


                foreach (NrSobrecostoDTO itemSobrecosto in listaSobrecosto)
                {
                    DateTime fecha = itemSobrecosto.Nrscfecha.Value;
                    decimal sobrecosto = (decimal)itemSobrecosto.Nrscsobrecosto;

                    //obteniendo las unidades que intervienen
                    List<MeMedicion96DTO> listaMedicion96 = listaEde96.Where(x => x.Medifecha == fecha).ToList();
                    List<GrupoEde> listaGrupoEdeSobrecosto = new List<GrupoEde>();
                    decimal edeTotal = 0;

                    foreach (MeMedicion96DTO itemMedicion96 in listaMedicion96)
                    {
                        MePtomedicionDTO meptomedicion =
                            listaNePtomedicion.Where(x => x.Ptomedicodi == itemMedicion96.Ptomedicodi).FirstOrDefault();
                        int grupocodi = meptomedicion == null ? -1 : (int)meptomedicion.Grupocodi;

                        decimal ede = (decimal)itemMedicion96.Meditotal;

                        listaGrupoEdeSobrecosto.Add(new GrupoEde()
                        {
                            Grupocodi = grupocodi,
                            Ede = ede
                        });

                        edeTotal += ede;
                    }

                    //actualizando los sobrecostos
                    if (listaGrupoEdeSobrecosto.Count == 1)
                    {
                        GrupoEde ges = listaGrupoEdeSobrecosto[0];

                        //crear registro
                        CreaRegistroProceso(nrperCodi, ges.Grupocodi, nrcptcodi, fecha, fecha, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                            0, ges.Ede, -1, "N", "N", "A", "N", sobrecosto,
                            "", "", "", "N", 0, 0, usuario, DateTime.Now, null, null);
                    }
                    else
                    {
                        foreach (var itemGes in listaGrupoEdeSobrecosto)
                        {
                            if (edeTotal != 0)
                            {
                                //crear registro
                                CreaRegistroProceso(nrperCodi, itemGes.Grupocodi, nrcptcodi, fecha, fecha, 0, 0, 0, 0, 0,
                                    0,
                                    0, 0, 0, 0, itemGes.Ede, -1, "N", "N", "A", "N", sobrecosto * itemGes.Ede / edeTotal,
                                    "", "", "", "N", 0, 0, usuario, DateTime.Now, null, null);
                            }
                            else
                            {
                                //crear registro
                                CreaRegistroProceso(nrperCodi, itemGes.Grupocodi, nrcptcodi, fecha, fecha, 0, 0, 0, 0, 0,
                                    0,
                                    0, 0, 0, 0, itemGes.Ede, -1, "N", "N", "A", "N", 0,
                                    "", "", "", "N", 0, 0, usuario, DateTime.Now, null, null);
                            }
                        }
                    }
                }

                return numObservaciones;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite calcular la demora en el arranque (Reserva fría)
        /// </summary>
        /// <param name="nrcptcodi">código de concepto</param>
        /// <param name="nrperCodi">código de periodo</param>
        /// <param name="fechaInicio">fecha de inicio</param>
        /// <param name="fechaFinal">fecha final</param>
        /// <param name="listaReservaFria">Lista de grupos de reserva fría</param>
        /// <param name="listaEquivalenciaReservaFria">Lista de equivalencia de Reserva Fria</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        public int CalcularRfDemoraArranque(int nrcptcodi, int nrperCodi, DateTime fechaInicio, DateTime fechaFinal,
            List<PrGrupoDTO> listaReservaFria, List<ModoGrupoCentralEquipo> listaEquivalenciaReservaFria, string usuario)
        {

            try
            {
                int numObservaciones = 0;
                var listaGrupocodiModo = listaEquivalenciaReservaFria.Select(x => x.GrupocodiModo);
                string grupoCodiModo = "," + String.Join(",", listaGrupocodiModo) + ",";

                //eliminar registros de concepto y periodo            
                DeleteNrProceso(nrperCodi, nrcptcodi);

                #region Si el equipo sincroniza al SEIN

                //SI EL EQUIPO SINCRONIZA AL SEIN
                /*List<EveHoraoperacionDTO> listaHoraOperacion =
                        servHoraOperacion.GetByCriteria(fechaInicio, fechaFinal).Where
                            (x => x.Hophorordarranq != null && x.Grupocodi.ToString().IndexOf(grupoCodiModo) >= 0).ToList();*/
                List<EveHoraoperacionDTO> listaHoraOperacion =
                                        FactorySic.GetEveHoraoperacionRepository().GetByCriteria(fechaInicio, fechaFinal).Where
                                        (x => x.Grupocodi.HasValue && grupoCodiModo.Contains("," + ((int)x.Grupocodi).ToString() + ",") && x.Hophorordarranq.HasValue).ToList();
                //!(x.Hophorordarranq.HasValue) &&


                foreach (EveHoraoperacionDTO item in listaHoraOperacion)
                {
                    {
                        {
                            int ptomedicodi = listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == item.Grupocodi)
                                .FirstOrDefault()
                                .PtomedicodiMedidor; //(int)listaPtoMedicionGrupo[0].Ptomedicodi;

                            //Revisar si el valor de medidores es mayor a cero en medidores desde la Hora de Paralelo a bloque 15” siguiente

                            List<MeMedicion96DTO> listaMedidores =
                                FactorySic.GetMeMedicion96Repository().GetByCriteria(ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                    ptomedicodi, ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
                                    (DateTime)item.Hophorini, (DateTime)item.Hophorini).ToList();

                            if (listaMedidores.Count == 0)
                            {
                                //no hay datos
                            }
                            else
                            {
                                if (listaMedidores.Count == 1)
                                {
                                    //hay datos
                                    DateTime bloqueparalelo = (DateTime)item.Hophorini;
                                    int hi = 4 * bloqueparalelo.Hour + bloqueparalelo.Minute / 15;
                                    //Revisar si el valor de medidores es mayor a cero en medidores desde la Hora de Paralelo a bloque 15” siguiente
                                    int hiSgte = hi + 1;

                                    //listaMedidores[0].h
                                    decimal valor =
                                        (decimal)
                                        listaMedidores[0].GetType()
                                            .GetProperty("H" + hi.ToString())
                                            .GetValue(listaMedidores[0], null);
                                    decimal valorSgte =
                                        (decimal)
                                        listaMedidores[0].GetType()
                                            .GetProperty("H" + hiSgte.ToString())
                                            .GetValue(listaMedidores[0], null);

                                    decimal t0 = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion((int)item.Grupocodi,
                                            ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadTomaCarga,
                                            ((DateTime)item.Hophorini));// (tiempo de arranque en minutos)
                                    t0 = t0 / ((decimal)60.0); //tiempo en horas



                                    //SI
                                    if (valor > 0 && valorSgte > 0)
                                    {
                                        //demora en el arranque

                                        decimal t =
                                                (decimal)
                                                (((DateTime)item.Hophorini - (DateTime)item.Hophorordarranq).TotalHours);
                                        //Hora Paralelo – Hora Arranque;
                                        decimal t1 = t - t0;

                                        t1 = Math.Max(t1, 0);

                                        //crear registro
                                        CreaRegistroProceso(nrperCodi, (int)item.Grupocodi, nrcptcodi, (DateTime)item.Hophorini, (DateTime)item.Hophorfin, t1, t1 *
                                                                      ObtenerPropocionPropiedadModoCentral(
                                                                          (int)item.Grupocodi,
                                                                          ConstantesReservaFriaNodoEnergetico
                                                                              .ConcepcodiPotenciaEfectiva,
                                                                          (DateTime)item.Hophorini,
                                                                          listaEquivalenciaReservaFria), 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", "", "N", 0, 0, usuario, DateTime.Now, null, null);

                                    }
                                    else
                                    {
                                        //"Error. No hay potencia de medidores desde Hora de Paralelo a bloque 15 min. siguiente";
                                        string mensaje = ConstantesReservaFriaNodoEnergetico.NotaHoraParalelo;

                                        valorSgte = 0;
                                        DateTime horasgte = bloqueparalelo.AddMinutes(15);

                                        while (valorSgte == 0 || hiSgte > 96)
                                        {
                                            valorSgte =
                                                (decimal)
                                                listaMedidores[0].GetType()
                                                    .GetProperty("H" + hiSgte.ToString())
                                                    .GetValue(listaMedidores[0], null);
                                            horasgte = bloqueparalelo.AddMinutes(15);
                                            hiSgte++;
                                        }

                                        //no se llenan las horas de arranque
                                        //decimal t2 = (decimal)(horasgte - (DateTime)item.Hophorarranq).TotalHours;
                                        decimal t2 = (decimal)(horasgte - (DateTime)item.Hophorordarranq).TotalHours;
                                        //t2= Nueva hora paralelo – Hora arranque.

                                        decimal t1 = t2 - t0;

                                        t1 = Math.Max(t1, 0);

                                        //crear registro
                                        CreaRegistroProceso(nrperCodi, (int)item.Grupocodi, nrcptcodi,
                                            (DateTime)item.Hophorini, (DateTime)item.Hophorfin, t1, t1 *
                                                                                                      ObtenerPropocionPropiedadModoCentral
                                                                                                      (
                                                                                                          (int)
                                                                                                          item.Grupocodi,
                                                                                                          ConstantesReservaFriaNodoEnergetico
                                                                                                              .ConcepcodiPotenciaEfectiva,
                                                                                                          (DateTime)
                                                                                                          item.Hophorini,
                                                                                                          listaEquivalenciaReservaFria),
                                            0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", mensaje, "N", 0,
                                            0, usuario, DateTime.Now, null, null);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                List<EveEventoDTO> listaEvento = FactorySic.GetEveEventoRepository().ConsultaEventoExtranet(fechaInicio, fechaFinal,
                    ConstantesReservaFriaNodoEnergetico.CausaevencodiFallaEquipo, -1, -1);

                //lista de grupopadre (grupoPadre) contiene los grupocodi con equivalencia con equicodi

                //equipos de reserva fria (relacion equicodi-grupocodi en eq_equipo)
                var listaEquicodi = listaEquivalenciaReservaFria.Select(x => x.Equicodi);
                string equiCodi = String.Join(",", listaEquicodi);

                listaEvento = listaEvento.Where(x => x.Equicodi.ToString().IndexOf(equiCodi) >= 0
                                                     &&
                                                     x.Tipoevencodi ==
                                                     ConstantesReservaFriaNodoEnergetico.CausaevencodiFallaEquipo &&
                                                     x.Deleted == "N"
                                                     && x.Evenpreliminar == "S"
                                                     &&
                                                     x.Subcausacodi ==
                                                     ConstantesReservaFriaNodoEnergetico.SubcausacodiDemoraArranque)
                    .ToList();


                #region Si equipo no sincroniza al SEIN

                //Si equipo no sincroniza al SEIN: (hay registro en eventos y versión bitácora y tipo de evento. 
                //Causa: Falla en el arranque
                foreach (EveEventoDTO itemEvento in listaEvento)
                {
                    int tipoOperacion = (int)itemEvento.Subcausacodiop;

                    if (tipoOperacion != 0)
                    {
                        //t1= Hora final – Hora inicial
                        decimal t1 =
                            (decimal)(((DateTime)itemEvento.Evenfin - (DateTime)itemEvento.Evenini).TotalHours);

                        List<EveSubcausaeventoDTO> listaSubcausa =
                             FactorySic.GetEveSubcausaeventoRepository().ObtenerSubcausaEvento(tipoOperacion).ToList();

                        string tipoOperacionDescripcion = "NO DEFINIDO";
                        if (listaSubcausa.Count == 1)
                        {
                            tipoOperacionDescripcion = listaSubcausa[0].Subcausadesc;
                        }

                        //La demora en el arranque se contabiliza como máximo hasta las 24:00 h. 
                        //y el tiempo restante hasta su sincronización o declaración de disponibilidad se considera como 
                        //Mantenimiento Programado y Ejecutado
                        if (((DateTime)itemEvento.Evenfin).Day != ((DateTime)itemEvento.Evenini).Day)
                        {

                            DateTime fechaRecorre = (DateTime)itemEvento.Evenfin;

                            fechaRecorre =
                                DateTime.ParseExact(
                                    fechaRecorre.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFechaYMD),
                                    ConstantesReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);

                            string observacion = "";
                            bool continuar = true;

                            while (continuar)
                            {
                                //calculando el mantenimiento programado y ejecutado
                                List<EveEventoDTO> listaEventoDia =
                                    FactorySic.GetEveEventoRepository().ConsultaEventoExtranet(fechaRecorre, fechaRecorre,
                                            ConstantesReservaFriaNodoEnergetico.CausaevencodiFallaEquipo, -1, -1).Where
                                        (x => x.Evenpreliminar == "S" && x.Equicodi == itemEvento.Equicodi
                                              &&
                                              x.Subcausacodi ==
                                              ConstantesReservaFriaNodoEnergetico.SubcausacodiDeclaroDisponible)
                                        .ToList();

                                if ((listaEventoDia.Count == 0))
                                {
                                    continuar = false;
                                    observacion = ConstantesReservaFriaNodoEnergetico.NotaNoHayDeclaracDispo;// "No hay declaración de disponibilidad";

                                    ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                                        listaEquivalenciaReservaFria.Where(x => x.Equicodi == itemEvento.Equicodi)
                                            .FirstOrDefault();

                                    int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
                                    int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

                                    //Crear registro
                                    CreaRegistroProceso(nrperCodi, grupocodiGrupo,
                                        ConstantesReservaFriaNodoEnergetico.ConceptoMantoProgEjecutado,
                                        (DateTime)itemEvento.Evenini, (DateTime)itemEvento.Evenfin, t1, t1 *
                                                                                    ObtenerPropocionPropiedadModoCentral
                                                                                    (
                                                                                        grupocodiModo,
                                                                                        ConstantesReservaFriaNodoEnergetico
                                                                                            .ConcepcodiPotenciaEfectiva,
                                                                                        (DateTime)itemEvento.Evenini,
                                                                                        listaEquivalenciaReservaFria), 0,
                                        0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", observacion, "N", 0,
                                        0, usuario, DateTime.Now, null, null);

                                    break;

                                }
                                else
                                {
                                    if (listaEventoDia.Count == 1)
                                    {
                                        if (((DateTime)listaEventoDia[0].Evenini).Day !=
                                            ((DateTime)listaEventoDia[0].Evenfin).Day)
                                        {
                                            continuar = false;

                                            //grabar registro de registro de mantenimiento programado
                                            decimal horaMantoProgEjec =
                                                (decimal)
                                                ((DateTime)(listaEventoDia[0].Evenfin) -
                                                 (DateTime)(listaEventoDia[0].Evenini)).TotalHours;

                                            ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                                                listaEquivalenciaReservaFria.Where(
                                                        x => x.Equicodi == itemEvento.Equicodi)
                                                    .FirstOrDefault();

                                            int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
                                            int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

                                            //crear registro
                                            CreaRegistroProceso(nrperCodi, grupocodiGrupo, nrcptcodi, (DateTime)itemEvento.Evenini,
                                                (DateTime)itemEvento.Evenfin, t1, t1 *
                                                                        ObtenerPropocionPropiedadModoCentral(
                                                                            grupocodiModo,
                                                                            ConstantesReservaFriaNodoEnergetico
                                                                                .ConcepcodiPotenciaEfectiva,
                                                                            (DateTime)itemEvento.Evenini,
                                                                            listaEquivalenciaReservaFria), 0, 0, 0, 0, 0,
                                                0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", observacion, "N", 0, 0,
                                                usuario, DateTime.Now, null, null);
                                        }
                                    }
                                }

                                fechaRecorre = fechaRecorre.AddDays(1);
                            }
                        }
                        else
                        {
                            decimal t =
                                    (decimal)(((DateTime)itemEvento.Evenfin - (DateTime)itemEvento.Evenini).TotalHours);
                            //Hora Paralelo – Hora Arranque;

                            ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                                listaEquivalenciaReservaFria.Where(x => x.Equicodi == itemEvento.Equicodi)
                                    .FirstOrDefault();

                            int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
                            int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

                            //crear registro
                            CreaRegistroProceso(nrperCodi, grupocodiGrupo, nrcptcodi, (DateTime)itemEvento.Evenini,
                                (DateTime)itemEvento.Evenfin, t, t * ObtenerPropocionPropiedadModoCentral(
                                                           grupocodiModo,
                                                           ConstantesReservaFriaNodoEnergetico
                                                               .ConcepcodiPotenciaEfectiva,
                                                           (DateTime)itemEvento.Evenini,
                                                           listaEquivalenciaReservaFria), 0, 0, 0, 0, 0, 0, 0, 0, 0, -1,
                                "N", "N", "A", "N", 0, "", "", "", "N", 0, 0, usuario, DateTime.Now, null,
                                null);
                        }
                    }
                }

                #endregion

                return numObservaciones;
            }
            catch
            {
                return -1;
            }



            //try
            //{
            //    int numObservaciones = 0;
            //    var listaGrupocodiModo = listaEquivalenciaReservaFria.Select(x => x.GrupocodiModo);
            //    string grupoCodiModo = String.Join(",", listaGrupocodiModo);

            //    //eliminar registros de concepto y periodo            
            //    DeleteNrProceso(nrperCodi, nrcptcodi);

            //    #region Si el equipo sincroniza al SEIN

            //    //SI EL EQUIPO SINCRONIZA AL SEIN
            //    List<EveHoraoperacionDTO> listaHoraOperacion =
            //            FactorySic.GetEveHoraoperacionRepository().GetByCriteria(fechaInicio, fechaFinal).Where
            //                (x => x.Hophorordarranq != null && x.Grupocodi.ToString().IndexOf(grupoCodiModo) >= 0).ToList();

            //    foreach (EveHoraoperacionDTO item in listaHoraOperacion)
            //    {
            //        {
            //            {
            //                int ptomedicodi = listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == item.Grupocodi)
            //                    .FirstOrDefault()
            //                    .PtomedicodiMedidor; //(int)listaPtoMedicionGrupo[0].Ptomedicodi;

            //                //Revisar si el valor de medidores es mayor a cero en medidores desde la Hora de Paralelo a bloque 15” siguiente

            //                List<MeMedicion96DTO> listaMedidores =
            //                    FactorySic.GetMeMedicion96Repository().GetByCriteria(ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
            //                        ptomedicodi, ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
            //                        (DateTime)item.Hophorini, (DateTime)item.Hophorini).ToList();

            //                if (listaMedidores.Count == 0)
            //                {
            //                    //no hay datos
            //                }
            //                else
            //                {
            //                    if (listaMedidores.Count == 1)
            //                    {
            //                        //hay datos
            //                        DateTime bloqueparalelo = (DateTime)item.Hophorini;
            //                        int hi = 4 * bloqueparalelo.Hour + bloqueparalelo.Minute / 15;
            //                        //Revisar si el valor de medidores es mayor a cero en medidores desde la Hora de Paralelo a bloque 15” siguiente
            //                        int hiSgte = hi + 1;

            //                        //listaMedidores[0].h
            //                        decimal valor =
            //                            (decimal)
            //                            listaMedidores[0].GetType()
            //                                .GetProperty("H" + hi.ToString())
            //                                .GetValue(listaMedidores[0], null);
            //                        decimal valorSgte =
            //                            (decimal)
            //                            listaMedidores[0].GetType()
            //                                .GetProperty("H" + hiSgte.ToString())
            //                                .GetValue(listaMedidores[0], null);

            //                        decimal t0 = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion((int)item.Grupocodi,
            //                                ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadTomaCarga,
            //                                ((DateTime)item.Hophorini));// (tiempo de arranque en minutos)
            //                        t0 = t0 / ((decimal)60.0); //tiempo en horas


            //                        //SI
            //                        if (valor > 0 && valorSgte > 0)
            //                        {
            //                            //demora en el arranque

            //                            decimal t =
            //                                    (decimal)
            //                                    (((DateTime)item.Hophorini - (DateTime)item.Hophorordarranq).TotalHours);
            //                            //Hora Paralelo – Hora Arranque;
            //                            decimal t1 = t - t0;

            //                            t1 = Math.Max(t1, 0);

            //                            //crear registro
            //                            CreaRegistroProceso(nrperCodi, (int)item.Grupocodi, nrcptcodi, (DateTime)item.Hophorini, (DateTime)item.Hophorfin, t1, t1 *
            //                                                          ObtenerPropocionPropiedadModoCentral(
            //                                                              (int)item.Grupocodi,
            //                                                              ConstantesReservaFriaNodoEnergetico
            //                                                                  .ConcepcodiPotenciaEfectiva,
            //                                                              (DateTime)item.Hophorini,
            //                                                              listaEquivalenciaReservaFria), 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", "", "N", 0, 0, usuario, DateTime.Now, null, null);

            //                        }
            //                        else
            //                        {
            //                            //"Error. No hay potencia de medidores desde Hora de Paralelo a bloque 15 min. siguiente";
            //                            string mensaje = ConstantesReservaFriaNodoEnergetico.NotaHoraParalelo;

            //                            valorSgte = 0;
            //                            DateTime horasgte = bloqueparalelo.AddMinutes(15);

            //                            while (valorSgte == 0 || hiSgte > 96)
            //                            {
            //                                valorSgte =
            //                                    (decimal)
            //                                    listaMedidores[0].GetType()
            //                                        .GetProperty("H" + hiSgte.ToString())
            //                                        .GetValue(listaMedidores[0], null);
            //                                horasgte = bloqueparalelo.AddMinutes(15);
            //                                hiSgte++;
            //                            }

            //                            decimal t2 = (decimal)(horasgte - (DateTime)item.Hophorarranq).TotalHours;
            //                            //t2= Nueva hora paralelo – Hora arranque.

            //                            decimal t1 = t2 - t0;

            //                            t1 = Math.Max(t1, 0);

            //                            //crear registro
            //                            CreaRegistroProceso(nrperCodi, (int)item.Grupocodi, nrcptcodi,
            //                                (DateTime)item.Hophorini, (DateTime)item.Hophorfin, t1, t1 *
            //                                                                                          ObtenerPropocionPropiedadModoCentral
            //                                                                                          (
            //                                                                                              (int)
            //                                                                                              item.Grupocodi,
            //                                                                                              ConstantesReservaFriaNodoEnergetico
            //                                                                                                  .ConcepcodiPotenciaEfectiva,
            //                                                                                              (DateTime)
            //                                                                                              item.Hophorini,
            //                                                                                              listaEquivalenciaReservaFria),
            //                                0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", mensaje, "N", 0,
            //                                0, usuario, DateTime.Now, null, null);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    #endregion

            //    List<EveEventoDTO> listaEvento = FactorySic.GetEveEventoRepository().ConsultaEventoExtranet(fechaInicio, fechaFinal,
            //        ConstantesReservaFriaNodoEnergetico.CausaevencodiFallaEquipo, -1, -1);

            //    //lista de grupopadre (grupoPadre) contiene los grupocodi con equivalencia con equicodi

            //    //equipos de reserva fria (relacion equicodi-grupocodi en eq_equipo)
            //    var listaEquicodi = listaEquivalenciaReservaFria.Select(x => x.Equicodi);
            //    string equiCodi = String.Join(",", listaEquicodi);

            //    listaEvento = listaEvento.Where(x => x.Equicodi.ToString().IndexOf(equiCodi) >= 0
            //                                         &&
            //                                         x.Tipoevencodi ==
            //                                         ConstantesReservaFriaNodoEnergetico.CausaevencodiFallaEquipo &&
            //                                         x.Deleted == "N"
            //                                         && x.Evenpreliminar == "S"
            //                                         &&
            //                                         x.Subcausacodi ==
            //                                         ConstantesReservaFriaNodoEnergetico.SubcausacodiDemoraArranque)
            //        .ToList();


            //    #region Si equipo no sincroniza al SEIN

            //    //Si equipo no sincroniza al SEIN: (hay registro en eventos y versión bitácora y tipo de evento. 
            //    //Causa: Falla en el arranque
            //    foreach (EveEventoDTO itemEvento in listaEvento)
            //    {
            //        int tipoOperacion = (int)itemEvento.Subcausacodiop;

            //        if (tipoOperacion != 0)
            //        {
            //            //t1= Hora final – Hora inicial
            //            decimal t1 =
            //                (decimal)(((DateTime)itemEvento.Evenfin - (DateTime)itemEvento.Evenini).TotalHours);

            //            List<EveSubcausaeventoDTO> listaSubcausa =
            //                FactorySic.GetEveSubcausaeventoRepository().ObtenerSubcausaEvento(tipoOperacion).ToList();

            //            string tipoOperacionDescripcion = "NO DEFINIDO";
            //            if (listaSubcausa.Count == 1)
            //            {
            //                tipoOperacionDescripcion = listaSubcausa[0].Subcausadesc;
            //            }

            //            //La demora en el arranque se contabiliza como máximo hasta las 24:00 h. 
            //            //y el tiempo restante hasta su sincronización o declaración de disponibilidad se considera como 
            //            //Mantenimiento Programado y Ejecutado
            //            if (((DateTime)itemEvento.Evenfin).Day != ((DateTime)itemEvento.Evenini).Day)
            //            {
            //                DateTime fechaRecorre = (DateTime)itemEvento.Evenfin;
            //                fechaRecorre =
            //                    DateTime.ParseExact(
            //                        fechaRecorre.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFechaYMD),
            //                        ConstantesReservaFriaNodoEnergetico.FormatoFechaYMD, CultureInfo.InvariantCulture);

            //                string observacion = "";
            //                bool continuar = true;

            //                while (continuar)
            //                {
            //                    //calculando el mantenimiento programado y ejecutado
            //                    List<EveEventoDTO> listaEventoDia =
            //                        FactorySic.GetEveEventoRepository().ConsultaEventoExtranet(fechaRecorre, fechaRecorre,
            //                                ConstantesReservaFriaNodoEnergetico.CausaevencodiFallaEquipo, -1, -1).Where
            //                            (x => x.Evenpreliminar == "S" && x.Equicodi == itemEvento.Equicodi
            //                                  &&
            //                                  x.Subcausacodi ==
            //                                  ConstantesReservaFriaNodoEnergetico.SubcausacodiDeclaroDisponible)
            //                            .ToList();

            //                    if ((listaEventoDia.Count == 0))
            //                    {
            //                        continuar = false;
            //                        observacion = ConstantesReservaFriaNodoEnergetico.NotaNoHayDeclaracDispo;// "No hay declaración de disponibilidad";

            //                        ModoGrupoCentralEquipo modoGrupoCentralEquipo =
            //                            listaEquivalenciaReservaFria.Where(x => x.Equicodi == itemEvento.Equicodi)
            //                                .FirstOrDefault();

            //                        int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
            //                        int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

            //                        //Crear registro
            //                        CreaRegistroProceso(nrperCodi, grupocodiGrupo,
            //                            ConstantesReservaFriaNodoEnergetico.ConceptoMantoProgEjecutado,
            //                            (DateTime)itemEvento.Evenini, (DateTime)itemEvento.Evenfin, t1, t1 *
            //                                                                        ObtenerPropocionPropiedadModoCentral
            //                                                                        (
            //                                                                            grupocodiModo,
            //                                                                            ConstantesReservaFriaNodoEnergetico
            //                                                                                .ConcepcodiPotenciaEfectiva,
            //                                                                            (DateTime)itemEvento.Evenini,
            //                                                                            listaEquivalenciaReservaFria), 0,
            //                            0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", observacion, "N", 0,
            //                            0, usuario, DateTime.Now, null, null);

            //                        break;

            //                    }
            //                    else
            //                    {
            //                        if (listaEventoDia.Count == 1)
            //                        {
            //                            if (((DateTime)listaEventoDia[0].Evenini).Day !=
            //                                ((DateTime)listaEventoDia[0].Evenfin).Day)
            //                            {
            //                                continuar = false;

            //                                //grabar registro de registro de mantenimiento programado
            //                                decimal horaMantoProgEjec =
            //                                    (decimal)
            //                                    ((DateTime)(listaEventoDia[0].Evenfin) -
            //                                     (DateTime)(listaEventoDia[0].Evenini)).TotalHours;

            //                                ModoGrupoCentralEquipo modoGrupoCentralEquipo =
            //                                    listaEquivalenciaReservaFria.Where(
            //                                            x => x.Equicodi == itemEvento.Equicodi)
            //                                        .FirstOrDefault();

            //                                int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
            //                                int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

            //                                //crear registro
            //                                CreaRegistroProceso(nrperCodi, grupocodiGrupo, nrcptcodi, (DateTime)itemEvento.Evenini,
            //                                    (DateTime)itemEvento.Evenfin, t1, t1 *
            //                                                            ObtenerPropocionPropiedadModoCentral(
            //                                                                grupocodiModo,
            //                                                                ConstantesReservaFriaNodoEnergetico
            //                                                                    .ConcepcodiPotenciaEfectiva,
            //                                                                (DateTime)itemEvento.Evenini,
            //                                                                listaEquivalenciaReservaFria), 0, 0, 0, 0, 0,
            //                                    0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "", observacion, "N", 0, 0,
            //                                    usuario, DateTime.Now, null, null);
            //                            }
            //                        }
            //                    }

            //                    fechaRecorre = fechaRecorre.AddDays(1);
            //                }
            //            }
            //            else
            //            {
            //                decimal t =
            //                        (decimal)(((DateTime)itemEvento.Evenfin - (DateTime)itemEvento.Evenini).TotalHours);
            //                //Hora Paralelo – Hora Arranque;

            //                ModoGrupoCentralEquipo modoGrupoCentralEquipo =
            //                    listaEquivalenciaReservaFria.Where(x => x.Equicodi == itemEvento.Equicodi)
            //                        .FirstOrDefault();

            //                int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
            //                int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

            //                //crear registro
            //                CreaRegistroProceso(nrperCodi, grupocodiGrupo, nrcptcodi, (DateTime)itemEvento.Evenini,
            //                    (DateTime)itemEvento.Evenfin, t, t * ObtenerPropocionPropiedadModoCentral(
            //                                               grupocodiModo,
            //                                               ConstantesReservaFriaNodoEnergetico
            //                                                   .ConcepcodiPotenciaEfectiva,
            //                                               (DateTime)itemEvento.Evenini,
            //                                               listaEquivalenciaReservaFria), 0, 0, 0, 0, 0, 0, 0, 0, 0, -1,
            //                    "N", "N", "A", "N", 0, "", "", "", "N", 0, 0, usuario, DateTime.Now, null,
            //                    null);
            //            }
            //        }
            //    }

            //    #endregion

            //    return numObservaciones;
            //}
            //catch
            //{
            //    return -1;
            //}
        }

        /// <summary>
        /// Permite calcular las horas de mantenimiento programado y correctivo ejecutado
        /// </summary>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaUnidades">Lista de unidades</param>
        /// <param name="listaEquivalenciaUnidades">Lista de equivalencia de unidades</param>
        /// <param name="nrcptcodiHoraMantoProgEjec">Concepto de hora mantenimiento programado y ejecutado</param>
        /// <param name="nrcptcodiHoraMantoCorrEjec">Concepto de hora mantenimiento correctivo y ejecutado</param>
        /// <param name="parametroTolerancia">Código de parámetro de tolerancia</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        public int CalcularRfNeHorasMantenimiento(int nrperCodi, DateTime fechaInicio, DateTime fechaFinal,
    List<PrGrupoDTO> listaUnidades, List<ModoGrupoCentralEquipo> listaEquivalenciaUnidades,
    int nrcptcodiHoraMantoProgEjec, int nrcptcodiHoraMantoCorrEjec, int parametroTolerancia, string usuario)
        {

            try
            {
                int numObservaciones = 0;

                //eliminar registros de concepto y periodo 
                DeleteNrProceso(nrperCodi, nrcptcodiHoraMantoProgEjec);
                DeleteNrProceso(nrperCodi, nrcptcodiHoraMantoCorrEjec);

                var listaGrupocodi = listaUnidades.Select(x => x.Grupocodi);
                string grupoCodi = String.Join(",", listaGrupocodi);
                int[] arrGrupocodi = listaGrupocodi.ToArray();

                var listaGrupoPadre = listaUnidades.Select(x => x.Grupopadre);
                string grupoPadre = String.Join(",", listaGrupoPadre);

                var listaEquicodi = listaEquivalenciaUnidades.Select(x => x.Equicodi);
                var listaEquicodiCentral = listaEquivalenciaUnidades.Select(x => x.EquicodiCentral);
                string equicodis0 = String.Join(",", listaEquicodi);
                string equicodis1 = String.Join(",", listaEquicodiCentral);
                string equicodis = equicodis0 + "," + equicodis1;

                string fechaInicioDMY = fechaInicio.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);
                string fechaFinDMY = fechaFinal.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);

                List<EveManttoDTO> listaManttoEjec =
                    FactorySic.GetEveManttoRepository().ObtenerManttoEquipoClaseFecha(equicodis, fechaInicioDMY, fechaFinDMY,
                            ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado)
                        .Where(x => x.Evenindispo.Equals("F")).ToList();


                //.ToList();
                List<EveManttoDTO> listaManttoProg =
                    FactorySic.GetEveManttoRepository().ObtenerManttoEquipoClaseFecha(equicodis, fechaInicioDMY, fechaFinDMY,
                            ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario)
                        .Where(x => x.Evenindispo.Equals("F")).ToList();

                //Reducir mantenimientos
                listaManttoEjec = ReducirMantenimiento(listaManttoEjec);
                listaManttoProg = ReducirMantenimiento(listaManttoProg);

                string logListaMantoEjec = "";

                foreach (EveManttoDTO itemM in listaManttoEjec)
                {
                    logListaMantoEjec += itemM.Equicodi + "-" + "-" +
                        itemM.Evenini.ToString() + "-" + itemM.Evenfin.ToString() + "\r\n";

                }


                string logListaMantoProg = "";

                foreach (EveManttoDTO itemM in listaManttoEjec)
                {
                    logListaMantoProg += itemM.Equicodi + "-" + "-" +
                        itemM.Evenini.ToString() + "-" + itemM.Evenfin.ToString() + "\r\n";

                }


                List<SiParametroValorDTO> listaParametroValor =
                    BuscarOperaciones(
                        parametroTolerancia,
                        fechaInicio, fechaFinal, -1, -1, "N");

                DateTime fechaRec = fechaInicio;
                //double hmpe = 0; //horas de mantenimiento programado y ejecutado
                //double hmce = 0; //horas de mantenimiento correctivo y ejecutado

                while (fechaRec <= fechaFinal)
                {
                    //obtener tolerancia del día
                    double tolerancia = 0;// (double)ObtenerPametroValor(listaParametroValor, fechaRec);



                    //lista de equipos de reserva fría
                    foreach (var rfria in listaEquivalenciaUnidades)
                    {
                        string logListaMantoEjecUnid = "dia: " + fechaRec.ToString() + "\r\n";


                        List<EveManttoDTO> listaMantoEjecUnidad =
                            listaManttoEjec.Where(x => ((int)x.Equicodi).Equals(rfria.Equicodi) &&
                                                       fechaRec <= x.Evenini && x.Evenfin <= fechaRec.AddDays(1))
                                .ToList();

                        foreach (EveManttoDTO itemM in listaMantoEjecUnidad)
                        {
                            logListaMantoEjecUnid += itemM.Equicodi + "-" + "-" +
                                itemM.Evenini.ToString() + "-" + itemM.Evenfin.ToString() + "\r\n";

                        }


                        List<EveManttoDTO> listaManttoProgUnidad =
                            listaManttoProg.Where(x => (((int)x.Equicodi).Equals(rfria.Equicodi)) &&
                                                       fechaRec <= x.Evenini && x.Evenfin <= fechaRec.AddDays(1))
                                .ToList();


                        ManttoResultado mantoRes = new ManttoResultado();

                        //mantenimiento programado
                        foreach (EveManttoDTO mp in listaManttoProgUnidad)
                        {
                            EveIeodcuadroDTO ei = new EveIeodcuadroDTO();
                            ei.Ichorini = mp.Evenini;
                            ei.Ichorfin = mp.Evenfin;

                            mantoRes.AsignaMantoProgramado(ei, 1, 0);
                        }

                        foreach (EveManttoDTO me in listaMantoEjecUnidad)
                        {
                            EveIeodcuadroDTO ei = new EveIeodcuadroDTO();
                            ei.Ichorini = me.Evenini;
                            ei.Ichorfin = me.Evenfin;

                            mantoRes.AsignaMantoEjecutadoProgramado(ei, 2, 3, 0);
                        }

                        double hmpe = 0;// mantoRes.HoraIndispTotalProgramada();
                        double hmce = 0;// mantoRes.HoraIndispTotalFortuita();

                        List<EveIeodcuadroDTO> listaMantProgEjec = mantoRes.ListaIndispTotalProgramada(fechaRec);
                        List<EveIeodcuadroDTO> listaMantCorrEjec = mantoRes.ListaIndispTotalFortuita(fechaRec);


                        //Se realiza el cálculo horas-central

                        //propocion equipo-central
                        decimal proporcion = ObtenerPropocionPropiedadModoCentral(
                            rfria.GrupocodiModo,
                            ConstantesReservaFriaNodoEnergetico
                                .ConcepcodiPotenciaEfectiva,
                            fechaRec,
                            listaEquivalenciaUnidades);

                        decimal hmpeCentral = 0;// (decimal)hmpe * proporcion;
                        decimal hmceCentral = 0;// (decimal)hmce * proporcion;

                        //ingreso de mantenimiento correctivo ejecutado
                        //----

                        //grabar registro HMCE
                        ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                            listaEquivalenciaUnidades.Where(x => x.GrupocodiModo == rfria.GrupocodiModo)
                                .FirstOrDefault();

                        int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
                        int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

                        //crear regstro
                        foreach (EveIeodcuadroDTO item in listaMantCorrEjec)
                        {
                            hmce = ((DateTime)item.Ichorfin - (DateTime)item.Ichorini).TotalHours;
                            hmceCentral = (decimal)hmce * proporcion;

                            if (hmce != 0)
                            {
                                CreaRegistroProceso(nrperCodi, grupocodiModo,
                                    nrcptcodiHoraMantoCorrEjec,
                                    (DateTime)item.Ichorini, (DateTime)item.Ichorfin,
                                    (decimal)hmce,
                                    hmceCentral, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "",
                                    "", "N", 0, 0, usuario, DateTime.Now, null, null);
                            }
                        }


                        foreach (EveIeodcuadroDTO item in listaMantProgEjec)
                        {
                            hmpe = ((DateTime)item.Ichorfin - (DateTime)item.Ichorini).TotalHours;
                            hmpeCentral = (decimal)hmpe * proporcion;

                            if (hmpe != 0)
                            {
                                CreaRegistroProceso(nrperCodi, grupocodiModo,
                                    nrcptcodiHoraMantoProgEjec,
                                    (DateTime)item.Ichorini, (DateTime)item.Ichorfin,
                                    (decimal)hmpe,
                                    hmpeCentral, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "",
                                    "", "N", 0, 0, usuario, DateTime.Now, null, null);
                            }
                        }


                    }

                    fechaRec = fechaRec.AddDays(1);
                }

                return numObservaciones;
            }
            catch
            {

                return -1;
            }
        }



        /// <summary>
        /// Permite reducir los mantenimientos haciendo las uniones
        /// </summary>
        /// <param name="listaMantenimientoTemporal">lista de mantenimientos a unificar</param>
        /// <returns></returns>
        private List<EveManttoDTO> ReducirMantenimiento(List<EveManttoDTO> listaMantenimientoTemporal)
        {
            List<EveManttoDTO> listaMantto = new List<EveManttoDTO>();

            foreach (EveManttoDTO item1 in listaMantenimientoTemporal)
            {
                EveManttoDTO m1 = item1;

                foreach (EveManttoDTO item2 in listaMantenimientoTemporal)
                {
                    if (item1.Manttocodi != item2.Manttocodi)
                    {
                        if ((item1.Evenclasecodi == item2.Evenclasecodi) && (item1.Equicodi == item2.Equicodi))
                        {
                            if (((DateTime)item1.Evenini).ToString("dd/MM/yyyy") == ((DateTime)item2.Evenini).ToString("dd/MM/yyyy"))
                            {
                                //1
                                if (m1.Evenini >= item2.Evenini && m1.Evenfin <= item2.Evenfin)
                                {
                                    m1.Evenini = item2.Evenini;
                                    m1.Evenfin = item2.Evenfin;
                                }
                                else
                                {
                                    //2
                                    if (m1.Evenini <= item2.Evenini && m1.Evenfin >= item2.Evenfin)
                                    {
                                        //m1 es mayor
                                    }
                                    else
                                    {
                                        //3
                                        if (m1.Evenini <= item2.Evenini && m1.Evenini <= item2.Evenfin)
                                        {
                                            m1.Evenfin = item2.Evenfin;
                                        }
                                        else
                                        {
                                            //4
                                            if (m1.Evenfin <= item2.Evenini && m1.Evenfin <= item2.Evenfin)
                                            {
                                                m1.Evenini = item2.Evenfin;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                listaMantto.Add(m1);
            }


            //elimina duplicado
            List<EveManttoDTO> listaManttofinal = new List<EveManttoDTO>();

            foreach (EveManttoDTO item0 in listaMantto)
            {
                bool agregar = true;

                foreach (EveManttoDTO item1 in listaManttofinal)
                {
                    if (item1.Manttocodi != item0.Manttocodi)
                    {

                        if ((item0.Equicodi == item1.Equicodi) &&
                      (item0.Evenclasecodi == item1.Evenclasecodi) &&
                       (item0.Evenini == item1.Evenini) &&
                       (item0.Evenfin == item1.Evenfin)
                       )
                        {
                            agregar = false;
                            break;
                        }
                    }
                }

                if (agregar)
                {
                    listaManttofinal.Add(item0);
                }

            }

            return listaManttofinal;
        }


        /// <summary>
        /// Permite calcular las horas para la EDE (Energía dejada de entregar). Reserva Fría
        /// </summary>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaReservaFria">Lista de unidades de Reserva Fría</param>
        /// <param name="listaEquivalenciaReservaFria">Lista de equivalencia de Reserva Fría</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        public int CalcularRfHorasEde(int nrcptcodi, int nrperCodi, DateTime fechaInicio,
            DateTime fechaFinal, List<PrGrupoDTO> listaReservaFria,
            List<ModoGrupoCentralEquipo> listaEquivalenciaReservaFria, string usuario)
        {
            //string resultado="";
            try
            {
                int numObservaciones = 0;

                //eliminar registros de concepto y periodo            
                DeleteNrProceso(nrperCodi, nrcptcodi);

                //listado de parámetro RPF
                List<SiParametroValorDTO> listaParametroValorRpf =
                    BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Rpf,
                        fechaInicio, fechaFinal, -1, -1, "N");

                //configura las horas para luego realizar el EDE
                List<NrPotenciaconsignaDTO> listaPotenciaConsigna =
                    BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.ModuloReservaFria,
                        ConstantesReservaFriaNodoEnergetico.Todos, fechaInicio, fechaFinal,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1);

                foreach (var itemRfria in listaPotenciaConsigna)
                {

                    //obtener potencia efectiva del modo de operación
                    //obtener rpf
                    decimal rpf = ObtenerPametroValor(listaParametroValorRpf, (DateTime)itemRfria.Nrpcfecha);

                    decimal potenciaEfectivaModo = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion((int)itemRfria.Grupocodi,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                        (DateTime)itemRfria.Nrpcfecha);//((DateTime) itemRfria.Nrpcfecha).ToString(Constantes.FormatoFechaYMD));

                    ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                            listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == (int)itemRfria.Grupocodi).FirstOrDefault();
                    //int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;
                    int grupocodiModo = (int)itemRfria.Grupocodi;

                    //ModoGrupoCentralEquipo mgce = listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == grupocodiModo).FirstOrDefault();

                    //if (mgce != null)
                    {
                        //crear registro
                        CreaRegistroProceso(nrperCodi, grupocodiModo, nrcptcodi, (DateTime)itemRfria.Nrpcfecha,
                            ((DateTime)itemRfria.Nrpcfecha).AddDays(1), 0, 0, potenciaEfectivaModo * (1 - rpf), 0, 0,
                            potenciaEfectivaModo, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "",
                            (rpf == 0 ? ConstantesReservaFriaNodoEnergetico.NotaRpfCero : ""), "N", rpf, 0, usuario, DateTime.Now, null, null);

                    }

                }

                return numObservaciones;
            }
            catch
            {

                return -1;
            }
        }


        /// <summary>
        /// Permite calcular la EDE de Reserva Fría
        /// </summary>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaReservaFria">Lista de unidades de Reserva Fría</param>
        /// <param name="listaEquivalenciaReservaFria">Lista de equivalencia de Reserva Fría</param>
        /// <param name="nrcptcodiHoras">Código de concepto de horas</param>
        /// <param name="lectcodi96">Código de lectura</param>
        /// <returns></returns>
        public int CalcularRfEde(int nrcptcodi, int nrperCodi, DateTime fechaInicio, DateTime fechaFinal,
            List<PrGrupoDTO> listaReservaFria, List<ModoGrupoCentralEquipo> listaEquivalenciaReservaFria,
            int nrcptcodiHoras, int lectcodi96)
        {
            try
            {
                int numObservaciones = 0;

                //elimnando los datos de medicion96 de fecha a fecha.
                foreach (var unidadEquivalente in listaEquivalenciaReservaFria)
                {
                    FactorySic.GetMeMedicion96Repository().Delete(unidadEquivalente.PtomedicodiReservaFria,
                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fechaInicio, fechaFinal, lectcodi96);
                }

                int[] bloqueConsigna = new int[1];
                decimal[] potenciaConsigna = new decimal[1];

                //eliminar registros de concepto y periodo            
                DeleteNrProceso(nrperCodi, nrcptcodi);

                var listaGrupocodiModo = listaEquivalenciaReservaFria.Select(x => x.GrupocodiModo);
                string grupoCodiModo = "," + String.Join(",", listaGrupocodiModo) + ",";

                //configura las horas para luego realizar el EDE
                List<NrProcesoDTO> listaHorasEde =
                    BuscarOperaciones("N", nrperCodi, ConstantesReservaFriaNodoEnergetico.Todos, nrcptcodiHoras,
                        fechaInicio, fechaFinal, -1, -1);

                /*List<EveHoraoperacionDTO> listaHoraOperacion =
                    servHoraOperacion.GetByCriteria(fechaInicio, fechaFinal).Where
                        (x => x.Hophorordarranq != null && x.Grupocodi.ToString().IndexOf(grupoCodiModo) >= 0).ToList();*/

                List<EveHoraoperacionDTO> listaHoraOperacion =
                    FactorySic.GetEveHoraoperacionRepository().GetByCriteria(fechaInicio, fechaFinal).Where
                        (x => x.Grupocodi.HasValue && grupoCodiModo.Contains("," + ((int)x.Grupocodi).ToString() + ",") && x.Hophorordarranq.HasValue).ToList();


                //listado de parámetro RPF
                List<SiParametroValorDTO> listaParametroValorRpf =
                    BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Rpf,
                        fechaInicio, fechaFinal, -1, -1, "N");


                foreach (var itemRfria in listaHorasEde)
                {
                    decimal tomaCargaMwMin = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion((int)itemRfria.Grupocodi,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadTomaCarga,
                        ((DateTime)itemRfria.Nrprcfechainicio));

                    decimal reduccionCargaMwMin = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion((int)itemRfria.Grupocodi,
                                   ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadReduccionCarga,
                                   ((DateTime)itemRfria.Nrprcfechainicio));

                    ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                        listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == itemRfria.Grupocodi.Value)
                            .FirstOrDefault();

                    MeMedicion96DTO listaMedidores =
                        FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                ConstantesReservaFriaNodoEnergetico
                                    .TipoinfocodiPotenciaActiva,
                                modoGrupoCentralEquipo.PtomedicodiMedidor,
                                ConstantesReservaFriaNodoEnergetico
                                    .OrigenLecturaMedidores,
                                (DateTime)itemRfria.Nrprcfechainicio, (DateTime)itemRfria.Nrprcfechainicio)
                            .FirstOrDefault();

                    EveHoraoperacionDTO listaHoraOperacionElemento =
                        listaHoraOperacion.Where(
                            x =>
                                x.Hophorini >= itemRfria.Nrprcfechainicio && x.Hophorfin < itemRfria.Nrprcfechafin &&
                                x.Grupocodi == (int)itemRfria.Grupocodi).FirstOrDefault();


                    if (listaHoraOperacionElemento == null)
                    {
                        //listaHoraOperacionElemento = new EveHoraoperacionDTO();

                        // se actualiza el mensaje en el registro de horas EDE                        
                        try
                        {
                            NrProcesoDTO proc = GetByIdNrProceso(itemRfria.Nrprccodi);
                            proc.Nrprcobservacion = ConstantesReservaFriaNodoEnergetico.NotaNoHayHoraOperacion;
                            UpdateNrProceso(proc);
                        }
                        catch
                        {

                        }
                    }

                    decimal[] medidor96 = ObtenerMedidor96Arreglo(listaMedidores);

                    //obtener las potencias consigna
                    List<NrPotenciaconsignaDTO> listaPotenciaConsigna = BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ModuloReservaFria, (int)itemRfria.Grupocodi,
                        (DateTime)itemRfria.Nrprcfechainicio, (DateTime)itemRfria.Nrprcfechainicio,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1);
                    if (listaPotenciaConsigna.Count > 0)
                    {
                        foreach (var itemPotCon in listaPotenciaConsigna)
                        {
                            List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                                BuscarOperaciones(itemPotCon.Nrpccodi,
                                        (DateTime)itemRfria.Nrprcfechainicio, (DateTime)itemRfria.Nrprcfechainicio, -1,
                                        -1)
                                    .ToList();

                            listaPotConsgDet.Sort(
                                (p, q) =>
                                    DateTime.Compare((DateTime)p.Nrpcdfecha,
                                        (DateTime)q.Nrpcdfecha));

                            //cargando datos
                            bloqueConsigna = new int[listaPotConsgDet.Count];
                            potenciaConsigna = new decimal[listaPotConsgDet.Count];

                            int idx = 0;
                            foreach (var itemPotConDet in listaPotConsgDet)
                            {
                                bloqueConsigna[idx] = ObtenerBloqueConsignaSegundo((DateTime)itemPotConDet.Nrpcdfecha);
                                potenciaConsigna[idx] = (decimal)itemPotConDet.Nrpcdmw;
                                idx++;
                            }
                        }
                    }

                    if (bloqueConsigna.Length > 1 && potenciaConsigna.Length > 1)
                    {
                        //obtener rpf
                        decimal rpf = 0;// ObtenerPametroValor(listaParametroValorRpf, itemRfria.Nrprcfechainicio.Value);


                        EdeExcel ex = new EdeExcel
                        {
                            minutoSincronizacion = ConstantesReservaFriaNodoEnergetico.MinutoSincronizacion,
                            tomaCargaMwMin = tomaCargaMwMin,
                            reduccionCargaMwMin = reduccionCargaMwMin,
                            fechaFalla =
                                listaHoraOperacionElemento == null
                                    ? itemRfria.Nrprcfechainicio.Value.AddHours(24)
                                    : (listaHoraOperacionElemento.Hopfalla == "S"
                                        ? listaHoraOperacionElemento.Hophorfin
                                        : itemRfria.Nrprcfechainicio.Value.AddHours(24)),
                            fechaSincronizacionReal =
                                listaHoraOperacionElemento == null
                                    ? itemRfria.Nrprcfechainicio.Value.AddHours(24)
                                    : listaHoraOperacionElemento.Hophorini.Value,
                            medidor96 = medidor96,
                            bloqueConsigna = bloqueConsigna,
                            potenciaConsigna = potenciaConsigna,
                            potenciaLimite = itemRfria.Nrprcpotencialimite.Value,
                            rpf = rpf
                        };

                        ex.Calcular(true);

                        /*
                        ModoGrupoCentralEquipo modogrupo =
                            listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == itemRfria.Grupocodi)
                                .FirstOrDefault();
                         GrabarEde(ex.curvaEde15Minuto, modogrupo.PtomedicodiNodoEnergetico,
                            ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                            (DateTime)itemRfria.Nrprcfechainicio, lectcodi96, ex.edeTotal);
                         
                        */
                        int grupocodi = (int)itemRfria.Grupocodi;
                        ModoGrupoCentralEquipo mgce = listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == grupocodi).FirstOrDefault();

                        if (mgce != null)
                        {
                            int ptomedicodi = mgce.PtomedicodiReservaFria;

                            GrabarEde(ex.curvaEde15Minuto, ptomedicodi,
                            ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                            (DateTime)itemRfria.Nrprcfechainicio, lectcodi96, ex.edeTotal);

                        }
                    }
                }

                return numObservaciones;
            }
            catch
            {

                return -1;
            }
        }


        /// <summary>
        /// Permite obtener el bloque a nivel de segundo
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        private int ObtenerBloqueConsignaSegundo(DateTime fecha)
        {
            return fecha.Hour * 60 * 60 + fecha.Minute * 60 + fecha.Second;
        }


        /// <summary>
        /// Permite convertir un registro de medición a arreglo
        /// </summary>
        /// <param name="datos">DTO de Medicion96</param>
        /// <returns></returns>
        private decimal[] ObtenerMedidor96Arreglo(MeMedicion96DTO datos)
        {
            decimal[] datos96 = new decimal[96];

            if (datos == null)
            {
                for (int i = 0; i < 96; i++)
                {
                    datos96[i] = 0;
                }
            }
            else
            {
                for (int i = 0; i < 96; i++)
                {
                    datos96[i] = (decimal)
                        datos.GetType()
                            .GetProperty("H" + (i + 1))
                            .GetValue(datos, null);
                }
            }

            return datos96;
        }


        /// <summary>
        /// Permite calcular las horas de mantenimiento programado y correctivo ejecutado
        /// </summary>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaNodoEnergetico">Lista de unidades de Nodo Energético</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de Nodo Energético</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        public int CalcularNeHorasIndisponibilidadTotalFortuita(int nrcptcodi, int nrperCodi, DateTime fechaInicio,
            DateTime fechaFinal, List<PrGrupoDTO> listaNodoEnergetico,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, string usuario)
        {

            try
            {

                //eliminar registros de concepto y periodo            
                DeleteNrProceso(nrperCodi, nrcptcodi);


                var listaGrupocodi = listaNodoEnergetico.Select(x => x.Grupocodi);
                string grupoCodi = String.Join(",", listaGrupocodi);
                int[] arrGrupocodi = listaGrupocodi.ToArray();

                var listaGrupoPadre = listaNodoEnergetico.Select(x => x.Grupopadre);
                string grupoPadre = String.Join(",", listaGrupoPadre);

                string fechaInicioDMY = fechaInicio.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);
                string fechaFinDMY = fechaFinal.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);


                List<EveManttoDTO> listaManttoEjec =
                    FactorySic.GetEveManttoRepository().ObtenerManttoEquipoClaseFecha(grupoPadre, fechaInicioDMY, fechaFinDMY,
                            ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado)
                        .Where(x => x.Deleted == 0 && x.Evenindispo == "F")
                        .ToList();
                List<EveManttoDTO> listaManttoProg =
                    FactorySic.GetEveManttoRepository().ObtenerManttoEquipoClaseFecha(grupoPadre, fechaInicioDMY, fechaFinDMY,
                            ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario)
                        .Where(x => x.Deleted == 0 && x.Evenindispo == "F")
                        .ToList();

                //Reducir mantenimientos
                listaManttoEjec = ReducirMantenimiento(listaManttoEjec);
                listaManttoProg = ReducirMantenimiento(listaManttoProg);


                List<SiParametroValorDTO> listaParametroValorTolerancia =
                    BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ToleranciaReservaNodoEnergetico,
                        fechaInicio, fechaFinal, -1, -1, "N");

                DateTime fechaRec = fechaInicio;
                double hmpe = 0; //horas de mantenimiento programado y ejecutado
                double hmce = 0; //horas de mantenimiento correctivo y ejecutado

                while (fechaRec <= fechaFinal)
                {
                    //obtener tolerancia del día
                    double tolerancia = (double)ObtenerPametroValor(
                        listaParametroValorTolerancia, fechaRec);

                    //lista de equipos de reserva fría
                    foreach (var rfria in listaNodoEnergetico)
                    {
                        List<EveManttoDTO> listaMantoEjecUnidad =
                            listaManttoEjec.Where(x => x.Equipadre == rfria.Grupopadre &&
                                                       x.Evenini <= fechaRec && fechaRec.AddDays(1) <= x.Evenfin)
                                .ToList();

                        List<EveManttoDTO> listaManttoProgUnidad =
                            listaManttoProg.Where(x => x.Equipadre == rfria.Grupocodi &&
                                                       x.Evenini <= fechaRec && fechaRec.AddDays(1) <= x.Evenfin)
                                .ToList();

                        //o	Si no hay registro programado, se obtiene HMPE=0
                        if (listaManttoProgUnidad.Count <= 0)
                        {
                            hmpe = 0;
                        }
                        else
                        {
                            if (listaMantoEjecUnidad.Count >= 1)
                            {
                                //o	Si hay registro programado:

                                //EveManttoDTO lista
                                EveManttoDTO listaMantoEjecUnidad1 = listaMantoEjecUnidad[0];
                                EveManttoDTO listaManttoProgUnidad1 = listaManttoProgUnidad[0];

                                //Si (HFE - HIE) > (1+Tol)*(HFP-HIP), se obtiene 
                                //HMPE =(1+TOL)*(HFP-HIP) sino se obtiene HMPE=(HFE - HIE)
                                TimeSpan spanEjecutado =
                                    ((DateTime)listaMantoEjecUnidad1.Evenfin).Subtract(
                                        (DateTime)listaMantoEjecUnidad1.Evenini);
                                TimeSpan spanProgramado =
                                    ((DateTime)listaManttoProgUnidad1.Evenfin).Subtract(
                                        (DateTime)listaManttoProgUnidad1.Evenini);


                                if ((spanEjecutado.TotalSeconds) > (1 + tolerancia) * (spanProgramado.TotalSeconds))
                                {
                                    //HMPE =(1+TOL)*(HFP-HIP)
                                    hmpe = (1 + tolerancia) * spanProgramado.TotalHours;
                                }
                                else
                                {
                                    //HMPE=(HFE - HIE)
                                    hmpe = spanEjecutado.TotalHours;
                                }

                                //HMCE= (HFE-HIE) - HMPE
                                hmce = spanEjecutado.TotalHours - hmpe; //Xi expresado en horas
                                //Se almacena la descripción del mantenimiento ejecutado para cada caso
                                string descripcion = listaMantoEjecUnidad1.Evendescrip;

                                //Se realiza el cálculo horas-central

                                //propocion equipo-central
                                decimal proporcion = ObtenerPropocionPropiedadModoCentral(
                                    rfria.Grupocodi,
                                    ConstantesReservaFriaNodoEnergetico
                                        .ConcepcodiPotenciaEfectiva,
                                    (DateTime)listaMantoEjecUnidad1.Evenfin,
                                    listaEquivalenciaNodoEnergetico);

                                decimal hmpeCentral = (decimal)hmpe * proporcion;
                                decimal hmceCentral = (decimal)hmce * proporcion;

                                //ingreso de mantenimiento correctivo ejecutado
                                //----
                                {
                                    ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                                        listaEquivalenciaNodoEnergetico.Where(x => x.GrupocodiModo == rfria.Grupocodi)
                                            .FirstOrDefault();
                                    int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
                                    int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

                                    //crear registro
                                    CreaRegistroProceso(nrperCodi, grupocodiGrupo, nrcptcodi,
                                        (DateTime)listaMantoEjecUnidad1.Evenini,
                                        (DateTime)listaMantoEjecUnidad1.Evenfin, (decimal)hmce, hmceCentral, 0, 0, 0, 0, 0, 0, 0,
                                        0,
                                        0, -1, "N", "N", "A", "N", 0, "", "", descripcion, "N", 0, 0, usuario,
                                        DateTime.Now, null, null);
                                }

                                //----
                                {
                                    //existe el mantenimiento programado y ejecutado?
                                    //no existe el mantenimiento programado y ejecutado
                                    ModoGrupoCentralEquipo modoGrupoCentralEquipo =
                                        listaEquivalenciaNodoEnergetico.Where(x => x.GrupocodiModo == rfria.Grupocodi)
                                            .FirstOrDefault();


                                    List<NrProcesoDTO> listaMantoProgejecBD = BuscarOperaciones("N", nrperCodi,
                                        modoGrupoCentralEquipo.GrupoCodiGrupo, nrcptcodi,
                                        (DateTime)listaMantoEjecUnidad1.Evenini,
                                        (DateTime)listaMantoEjecUnidad1.Evenfin,
                                        -1, -1);

                                    if (listaMantoProgejecBD.Count == 0)
                                    {

                                        int grupocodiModo = modoGrupoCentralEquipo.GrupocodiModo;
                                        int grupocodiGrupo = modoGrupoCentralEquipo.GrupoCodiGrupo;

                                        //crear registro
                                        CreaRegistroProceso(nrperCodi, grupocodiGrupo, nrcptcodi,
                                            (DateTime)listaMantoEjecUnidad1.Evenini, (DateTime)listaMantoEjecUnidad1.Evenfin, (decimal)hmpe,
                                            hmpeCentral, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, "N", "N", "A", "N", 0, "", "",
                                            descripcion, "N", 0, (decimal)tolerancia, usuario, DateTime.Now,
                                            null, null);
                                        //----
                                    }
                                }
                            }
                        }
                    }

                    fechaRec = fechaRec.AddDays(1);
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// Permite obtener el ratio de un grupo y central conociendo el concepto, y fecha
        /// </summary>
        /// <param name="grupocodiModo">Código del modo de operación</param>
        /// <param name="concepcodi">Código de concepto</param>
        /// <param name="fecha"></param>
        /// <param name="listaEquivalenciaReservaFria"></param>
        /// <returns></returns>
        private decimal ObtenerPropocionPropiedadModoCentral(int grupocodiModo, int concepcodi, DateTime fecha, List<ModoGrupoCentralEquipo> listaEquivalenciaReservaFria)
        {
            decimal proporcion = 0;

            ModoGrupoCentralEquipo unidadEjecucion =
                listaEquivalenciaReservaFria.Where(x => x.GrupocodiModo == grupocodiModo).FirstOrDefault();

            decimal potenciaGrupo = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodiModo,
                ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                fecha);

            decimal potenciaCentral = potenciaGrupo;

            foreach (ModoGrupoCentralEquipo item in listaEquivalenciaReservaFria)
            {
                if (item.GrupoCodiCentral == unidadEjecucion.GrupoCodiCentral)
                {
                    if (item.GrupocodiModo != unidadEjecucion.GrupocodiModo)
                    {
                        potenciaCentral += FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(item.GrupocodiModo,
                            concepcodi,
                            fecha);
                    }
                }
            }

            if (potenciaCentral != 0)
            {
                proporcion = potenciaGrupo / potenciaCentral;
            }
            else
            {
                proporcion = 0;
            }

            return proporcion;
        }


        /// <summary>
        /// Permite obtener el ratio de un grupo y central conociendo el concepto, y fecha
        /// </summary>
        /// <param name="equicodi">Código del equipo</param>
        /// <param name="concepcodi">Código de concepto</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="listaEquivalenciaReservaFria">Lista de equivalencia de Reserva Fría</param>
        /// <returns></returns>
        private decimal ObtenerPropocionPropiedadEquipoCentral(int equicodi, int concepcodi, DateTime fecha, List<ModoGrupoCentralEquipo> listaEquivalenciaReservaFria)
        {
            decimal valor = 0;
            decimal proporcion = 0;

            ModoGrupoCentralEquipo unidadEjecucion =
                listaEquivalenciaReservaFria.Where(x => x.Equicodi == equicodi).FirstOrDefault();

            decimal potenciaGrupo = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEjecucion.GrupoCodiGrupo,
                ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                fecha);//fecha.ToString(Constantes.FormatoFechaYMD));

            decimal potenciaCentral = potenciaGrupo;
            foreach (ModoGrupoCentralEquipo item in listaEquivalenciaReservaFria)
            {

                if (item.GrupoCodiCentral == unidadEjecucion.GrupoCodiCentral)
                {
                    if (item.GrupocodiModo != unidadEjecucion.GrupocodiModo)
                    {
                        potenciaCentral += FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEjecucion.GrupoCodiGrupo,
                            concepcodi,
                            fecha);
                    }
                }
            }


            if (potenciaCentral != 0)
            {
                proporcion = potenciaGrupo / potenciaCentral;
            }
            else
            {
                proporcion = 0;
            }

            return proporcion;
        }


        //NODO ENERGETICO
        /// <summary>
        /// Pemite calcular las horas de indisponibilidad Parcial Fortuita y Parcial Programada (PC)
        /// </summary>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaNodoEnergetico">Lista de unidades de Nodo Energético</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de Nodo Energético</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        public int CalcularNeHorasIndisponParcialFortProg(int nrperCodi, DateTime fechaInicio, DateTime fechaFinal,
            List<PrGrupoDTO> listaNodoEnergetico, List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico,
            string usuario)
        {
            try
            {
                ArrayList FechaIngresoCentral = new ArrayList();

                int numObservaciones = 0;

                //eliminar registros de concepto y periodo    
                DeleteNrProceso(nrperCodi, ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita);
                DeleteNrProceso(nrperCodi, ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada);


                var listaGrupocodi = listaNodoEnergetico.Select(x => x.Grupocodi);
                string grupoCodi = String.Join(",", listaGrupocodi);
                int[] arrGrupocodi = listaGrupocodi.ToArray();

                var listaGrupoPadre = listaNodoEnergetico.Select(x => x.Grupopadre);
                string grupoPadre = String.Join(",", listaGrupoPadre);

                string fechaInicioDMY = fechaInicio.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);
                string fechaFinDMY = fechaFinal.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);

                DateTime fechaRec = fechaInicio;

                List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergeticoOrdenado =
                    listaEquivalenciaNodoEnergetico.OrderBy(x => x.GrupoCodiCentral).ToList();

                List<SiParametroValorDTO> listaParametroValorRatio =
                    BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.RatioReservaFriaPcalculadaAdjudicada,
                        fechaInicio, fechaFinal, -1, -1, "N");

                //listado de parámetro RPF
                List<SiParametroValorDTO> listaParametroValorRpf =
                    BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Rpf,
                        fechaInicio, fechaFinal, -1, -1, "N");

                string logtotal = "";

                while (fechaRec <= fechaFinal)
                {
                    logtotal += "fecha, " + fechaRec + "\r\n";

                    ModoGrupoCentralEquipo unidadEquivalente = listaEquivalenciaNodoEnergeticoOrdenado[0];
                    int centralAnterior = unidadEquivalente.GrupoCodiCentral;

                    decimal rendimiento = 0; //Rend i: rendimiento de la unidad
                    decimal rendimientoSuma = 0; //Rend i: rendimiento de la unidad //DEBE SER PROMEDIO!!!
                    decimal potenciaEjecutadaDia = 0; //EED: energía ejecutada del día//decimal energiaEjecutadaDia = 0; //EED: energía ejecutada del día
                    decimal volumenFinalCombustible = 0; //VFDC: volumen final de combustible
                    decimal potenciaSuma = 0;//decimal rendimientoEnergia = 0;


                    decimal potenciaSumaAntiguo = 0;//decimal rendimientoEnergiaAntiguo=0;
                    decimal rendimientoSumaAntiguo = 0;
                    decimal potenciaAdjudicadaSuma = 0;

                    //int grupocodiAntiguo = unidadEquivalente.GrupoCodiGrupo;
                    //int grupocodiActual = unidadEquivalente.GrupoCodiGrupo;
                    //int grupocodiAntiguo = unidadEquivalente.GrupocodiModo;
                    int grupocodiActual = unidadEquivalente.GrupocodiModo;

                    int ptomedicodiStockAntiguo = unidadEquivalente.PtomedicodiStock;

                    int ptomedicodiStock = -1;
                    int unidades = 0;
                    int unidadesAntiguo = 0;

                    int indice = 0;

                    List<ModoGrupoCentralEquipo> unidadEquivalenteCentral = new List<ModoGrupoCentralEquipo>();

                    Hashtable ListaFechaModoValor = new Hashtable();

                    ModoGrupoCentralEquipo unidadEquivalenteAntiguo = unidadEquivalente;
                    string logUnidad = "";
                    foreach (var rfria in listaEquivalenciaNodoEnergeticoOrdenado)
                    {
                        unidadEquivalente = listaEquivalenciaNodoEnergeticoOrdenado[indice]; //listaEquivalenciaNodoEnergeticoOrdenado[0];
                        //grupocodiActual = unidadEquivalente.GrupoCodiGrupo;
                        grupocodiActual = unidadEquivalente.GrupocodiModo;

                        int centralActual = unidadEquivalente.GrupoCodiCentral;
                        ptomedicodiStock = unidadEquivalente.PtomedicodiStock;

                        logUnidad += ",centralActual," + centralActual + ",grupo," + unidadEquivalente.GruponombGrupo;
                        logUnidad += ",grupocodiActual," + grupocodiActual + ",unidades," + unidades + ",centralActual," + centralActual + ",";

                        decimal potenciaAdjudicada = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                                ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada,
                                fechaRec);

                        IngresarPotenciaAdjudicada(ref ListaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec, potenciaAdjudicada);

                        logUnidad += "potenciaAdjudicada," + potenciaAdjudicada;

                        if (centralActual == centralAnterior)
                        {

                            rendimiento = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                                ConstantesReservaFriaNodoEnergetico.ConcepcodiRendimiento, fechaRec);
                            unidades++;
                            potenciaEjecutadaDia = 0;

                            unidadEquivalenteCentral.Add(unidadEquivalente);

                            List<MeMedicion48DTO> listaEjecutado =
                                FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(
                                    unidadEquivalente.PtomedicodiDespacho.ToString(),
                                    ConstantesReservaFriaNodoEnergetico.LectcodiEjecutado,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                    fechaRec.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha));
                            if (listaEjecutado.Count != 0)
                            {
                                for (int i = 1; i <= 48; i++)
                                {
                                    potenciaEjecutadaDia += (decimal)
                                        listaEjecutado[0].GetType()
                                            .GetProperty("H" + i)
                                            .GetValue(listaEjecutado[0], null);
                                }
                            }

                            rendimientoSuma += rendimiento;
                            potenciaSuma += potenciaEjecutadaDia;//rendimientoEnergia += rendimiento * energiaEjecutadaDia;

                            potenciaAdjudicadaSuma += potenciaAdjudicada;

                            logUnidad += ",rendimiento," + rendimiento + ",unidades," + unidades + ",potenciaEjecutadaDia," +
                                potenciaEjecutadaDia + ",rendimientoSuma," + rendimientoSuma + ",potenciaSuma," + potenciaSuma + ",potenciaAdjudicadaSuma," + potenciaAdjudicadaSuma;// +"\r\n";



                        }
                        else
                        {

                            int retorno = EvaluarHoraNeCasos(ptomedicodiStockAntiguo, fechaRec,
                                listaEquivalenciaNodoEnergetico,
                                listaParametroValorRatio, listaParametroValorRpf,
                                nrperCodi, unidadEquivalenteAntiguo, potenciaSumaAntiguo, rendimientoSumaAntiguo,
                                unidadesAntiguo, usuario, ref FechaIngresoCentral, potenciaAdjudicadaSuma, ref logUnidad, unidadEquivalenteCentral,
                                ref ListaFechaModoValor
                                );

                            logUnidad += "Evalua central,rendimientoSuma," + rendimientoSuma + ",potenciaSuma," + potenciaSuma + ",unidades," +
    unidades + ",potenciaAdjudicadaSuma," + potenciaAdjudicadaSuma;// +"\r\n";

                            unidadEquivalenteCentral = new List<ModoGrupoCentralEquipo>();
                            unidadEquivalenteCentral.Add(unidadEquivalente);

                            if (retorno == -1)
                                return -1;

                            //rendimiento = 0;
                            rendimientoSuma = rendimiento;
                            //energiaEjecutadaDia = 0;
                            potenciaSuma = potenciaEjecutadaDia;//rendimientoEnergia = rendimiento * energiaEjecutadaDia;
                            unidades = 1;//unidades = 0;
                            potenciaAdjudicadaSuma = potenciaAdjudicada;

                            potenciaEjecutadaDia = 0;



                        }

                        //grupocodiAntiguo = grupocodiActual;
                        centralAnterior = centralActual;
                        ptomedicodiStockAntiguo = ptomedicodiStock;
                        unidadEquivalenteAntiguo = unidadEquivalente;
                        unidadesAntiguo = unidades;

                        potenciaSumaAntiguo = potenciaSuma;
                        rendimientoSumaAntiguo = rendimientoSuma;


                        indice++;
                        //logtotal += logUnidad;

                        logUnidad += "\r\n";
                    }

                    //procesando el último grupo
                    int retorno2 = EvaluarHoraNeCasos(ptomedicodiStockAntiguo, fechaRec,
                        listaEquivalenciaNodoEnergetico,
                        listaParametroValorRatio, listaParametroValorRpf,
                        nrperCodi, unidadEquivalenteAntiguo, potenciaSumaAntiguo, rendimientoSumaAntiguo,
                        unidadesAntiguo, usuario, ref FechaIngresoCentral, potenciaAdjudicadaSuma, ref logUnidad, unidadEquivalenteCentral,
                        ref ListaFechaModoValor);

                    logUnidad += "Evalua central,rendimientoSuma," + rendimientoSuma + ",potenciaSuma," + potenciaSuma + ",unidades," +
                                unidades + ",potenciaAdjudicadaSuma," + potenciaAdjudicadaSuma;// +"\r\n";


                    logtotal += logUnidad + "\r\n";
                    if (retorno2 == -1)
                        return -1;

                    fechaRec = fechaRec.AddDays(1);
                }

                return numObservaciones;
            }
            catch
            {
                return -1;
            }
        }


        private void IngresarPotenciaAdjudicada(ref Hashtable ListaFechaModoValor, int modo, DateTime fecha, decimal valor)
        {
            string key = fecha.ToString("dd/MM/yyyy") + "*" + modo;

            if (!ListaFechaModoValor.ContainsKey(key))
            {
                ListaFechaModoValor.Add(key, valor);
            }
            else
            {
                ListaFechaModoValor[key] = valor;
            }
        }

        private decimal ObtenerPotenciaAdjudicada(ref Hashtable ListaFechaModoValor, int modo, DateTime fecha)
        {
            string key = fecha.ToString("dd/MM/yyyy") + "*" + modo;

            if (ListaFechaModoValor.ContainsKey(key))
            {
                return (decimal)ListaFechaModoValor[key];
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Permite calcular el EDE de la indisponibilidad Total Fortuita
        /// </summary>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="nrcptcodiHora">Código de concepto de hora</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="listaNodoEnergetico">Lista de unidades de Nodo Energético</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de Nodo Energético</param>
        /// <param name="nrsmodCodi">Código de módulo</param>
        /// <param name="lectcodi">Código de lectura</param>
        /// <returns></returns>
        public int CalcularNeEdeTotalParcialProgramada(int nrperCodi, int nrcptcodiHora, DateTime fechaInicio,
            DateTime fechaFinal, List<PrGrupoDTO> listaNodoEnergetico,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrsmodCodi, int lectcodi)
        {

            try
            {
                int numObservaciones = 0;

                //eliminando los datos de medicion96 de fecha a fecha.
                foreach (var unidadEquivalente in listaEquivalenciaNodoEnergetico)
                {
                    FactorySic.GetMeMedicion96Repository().Delete(unidadEquivalente.PtomedicodiCentralNodoEnergetico,
                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fechaInicio, fechaFinal,
                        lectcodi);
                }

                var listaGrupocodi = listaNodoEnergetico.Select(x => x.Grupocodi);
                string grupoCodi = String.Join(",", listaGrupocodi);
                int[] arrGrupocodi = listaGrupocodi.ToArray();

                var listaGrupoPadre = listaNodoEnergetico.Select(x => x.Grupopadre);
                string grupoPadre = String.Join(",", listaGrupoPadre);

                string fechaInicioDMY = fechaInicio.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);
                string fechaFinDMY = fechaFinal.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha);


                List<NrProcesoDTO> listaHoras = BuscarOperaciones("N", nrperCodi, 0, nrcptcodiHora,
                    fechaInicio, fechaFinal, -1, -1);

                foreach (var itemHora in listaHoras)
                {

                    DateTime fecha =
                        DateTime.ParseExact(((DateTime)itemHora.Nrprcfechainicio).ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha),
                            ConstantesReservaFriaNodoEnergetico.FormatoFecha, CultureInfo.InvariantCulture);

                    //a. Consignas ingresadas a nivel de unidad que se evalúa por central
                    /*List<NrPotenciaconsignaDTO> listaConsigna = BuscarOperaciones(nrsmodCodi,
                        (int)itemHora.Grupocodi, fecha, fecha, "N", -1, -1);*/

                    //b. PDO o RDO simulado por central (datos 30")
                    ModoGrupoCentralEquipo modogrupo =
                        listaEquivalenciaNodoEnergetico.Where(x => x.GrupocodiModo == itemHora.Grupocodi)
                            .FirstOrDefault();

                    int grupocodiCentral = modogrupo == null ? -1 : modogrupo.GrupoCodiCentral;

                    if (grupocodiCentral > 0)
                    {

                        MeMedicion48DTO datosMedicion48PdoSimulado =
                            FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(
                                modogrupo.PtomedicodiCentralNodoEnergetico.ToString(),
                                ConstantesReservaFriaNodoEnergetico.Lectcodi48PdoSimulado,
                                ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault();

                        if (datosMedicion48PdoSimulado != null) //.Count > 0
                        {
                            /*
                            MeMedicion48DTO datosMedicion48Pdo =
                                FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(
                                    modogrupo.PtomedicodiReservaFria.ToString(),
                                    ConstantesReservaFriaNodoEnergetico.LectcodiProgramado,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                    fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault();
                            */

                            MeMedicion48DTO datosMedicion48Pdo = ObtenerDatosGeneracionCentralNe(modogrupo,
                                listaEquivalenciaNodoEnergetico, ConstantesReservaFriaNodoEnergetico.LectcodiProgramado, ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                fecha);


                            //si existe el registro no se recalcula
                            MeMedicion96DTO listaEde = FactorySic.GetMeMedicion96Repository().GetById(lectcodi, datosMedicion48PdoSimulado.Medifecha, ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, modogrupo.PtomedicodiCentralNodoEnergetico);

                            if (listaEde == null)
                            {
                                CalcularNeEde((DateTime)itemHora.Nrprcfechainicio, (DateTime)itemHora.Nrprcfechafin,
                                    datosMedicion48PdoSimulado, lectcodi, modogrupo.PtomedicodiCentralNodoEnergetico, datosMedicion48Pdo);
                            }

                        }
                    }

                }

                return numObservaciones;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Calcula la EDE a nivel de central para el nodo energético
        /// </summary>
        /// <param name="modogrupo">configuración del modo</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista equivalencia del Nodo Energético</param>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="tipoinfocodi">Código de tipo de información</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        private MeMedicion48DTO ObtenerDatosGeneracionCentralNe(ModoGrupoCentralEquipo modogrupo,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico,
            int lectcodi, int tipoinfocodi, DateTime fecha)
        {
            MeMedicion48DTO me = new MeMedicion48DTO();

            decimal[] dat = new decimal[48];

            for (int i = 0; i < 48; i++)
            {
                dat[i] = 0;
            }

            int grupocodiCentral = modogrupo.GrupoCodiCentral;


            foreach (ModoGrupoCentralEquipo mgce in listaEquivalenciaNodoEnergetico)
            {
                if (mgce.GrupoCodiCentral == grupocodiCentral)
                {
                    int ptomedicodi = mgce.PtomedicodiDespacho;

                    MeMedicion48DTO itemMedIdeal =
                                        FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(
                                            ptomedicodi.ToString(),
                                            lectcodi,
                                            tipoinfocodi,
                                            fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault();

                    if (itemMedIdeal != null)
                    {
                        //suma de elementos
                        for (int hi = 1; hi <= 48; hi++)
                        {
                            decimal potencia =
                                (decimal)
                                itemMedIdeal.GetType()
                                    .GetProperty("H" + hi.ToString())
                                    .GetValue(itemMedIdeal, null);

                            dat[hi - 1] += potencia;
                        }

                    }


                }

            }

            //pasando los datos del arreglo al dto
            for (int hi = 0; hi < 48; hi++)
            {
                me.GetType().GetProperty("H" + (hi + 1)).SetValue(me, dat[hi]);
            }




            return me;

        }


        /// <summary>
        /// Permite calcular el EDE de la indisponibilidad Total Fortuita
        /// </summary>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="nrcptcodiHora">Código de concepto (hora)</param>
        /// <param name="fechaInicio">Fecha de Inicio</param>
        /// <param name="fechaFinal">Fecha de fin</param>
        /// <param name="listaNodoEnergetico">Lista de nodo energetico</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de nodo energetico</param>
        /// <param name="nrsmodCodi">Código de módulo</param>
        /// <param name="lectcodi96">Código de lectura de Energía dejada de entregar</param>
        /// <returns></returns>
        public int CalcularNeEdeTotalFortuita(int nrperCodi, int nrcptcodiHora, DateTime fechaInicio,
            DateTime fechaFinal, List<PrGrupoDTO> listaNodoEnergetico,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrsmodCodi, int lectcodi96)
        {

            try
            {
                int numObservaciones = 0;

                var listaGrupocodiModo = listaEquivalenciaNodoEnergetico.Select(x => x.GrupocodiModo);

                string grupoCodiModo = "," + String.Join(",", listaGrupocodiModo) + ",";


                var listaGrupocodi = listaNodoEnergetico.Select(x => x.Grupocodi);
                string grupoCodi = String.Join(",", listaGrupocodi);
                int[] arrGrupocodi = listaGrupocodi.ToArray();

                var listaGrupoPadre = listaNodoEnergetico.Select(x => x.Grupopadre);
                string grupoPadre = String.Join(",", listaGrupoPadre);


                //lista de horas
                List<NrProcesoDTO> listaHoras = BuscarOperaciones("N", nrperCodi, 0, nrcptcodiHora,
                    fechaInicio, fechaFinal, -1, -1);

                //lista de horas de operacion
                /*List<EveHoraoperacionDTO> listaHoraOperacion =
                    servHoraOperacion.GetByCriteria(fechaInicio, fechaFinal).Where
                        (x => x.Hophorordarranq != null && x.Grupocodi.ToString().IndexOf(grupoCodiModo) >= 0).ToList();*/

                List<EveHoraoperacionDTO> listaHoraOperacion =
                    FactorySic.GetEveHoraoperacionRepository().GetByCriteria(fechaInicio, fechaFinal).Where
                        (x => x.Grupocodi.HasValue && grupoCodiModo.Contains("," + ((int)x.Grupocodi).ToString() + ",") && x.Hophorordarranq.HasValue).ToList();


                //pasando a lista por central, fecha y unidades(modos de operacion) que la conforman
                List<CentralFechaUnidad> listaCentralFechaUnidad = ObtenerConversionUnidadCentral(listaHoras, listaEquivalenciaNodoEnergetico);

                //eliminando los datos de medicion96 de fecha a fecha.
                foreach (var unidadEquivalente in listaCentralFechaUnidad)
                {
                    FactorySic.GetMeMedicion96Repository().Delete(unidadEquivalente.PtomedicodiCentral,
                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fechaInicio, fechaFinal,
                        lectcodi96);
                }

                //listado de parámetro RPF
                List<SiParametroValorDTO> listaParametroValorRpf =
                    BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Rpf,
                        fechaInicio, fechaFinal, -1, -1, "N");

                //ahora se recorre la central. Se usa sus unidades asociadas
                foreach (CentralFechaUnidad itemCentFechUnid in listaCentralFechaUnidad)
                {
                    //Para la curva IDEAL: primera ocurrencia de los siguientes casos:

                    DateTime fecha = itemCentFechUnid.Fecha;

                    int[] bloqueConsigna = new int[1];
                    decimal[] potenciaConsigna = new decimal[1];

                    EdeExcel centralEde = null;

                    foreach (int grupocodimodo in itemCentFechUnid.GrupocodiModo)
                    {

                        //a. Consignas ingresadas a nivel de unidad que se evalúa por central
                        List<NrPotenciaconsignaDTO> listaConsigna = BuscarOperaciones(nrsmodCodi,
                            grupocodimodo, fecha, fecha, "N", -1, -1);

                        if (listaConsigna.Count > 0)
                        {
                            foreach (NrPotenciaconsignaDTO itemConsigna in listaConsigna)
                            {
                                List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                                    BuscarOperaciones((int)itemConsigna.Nrpccodi, fecha, fecha, -1, -1);

                                listaPotConsgDet.Sort(
                                    (p, q) => DateTime.Compare((DateTime)p.Nrpcdfecha, (DateTime)q.Nrpcdfecha));

                                bloqueConsigna = new int[listaPotConsgDet.Count];
                                potenciaConsigna = new decimal[listaPotConsgDet.Count];

                                int idx = 0;
                                foreach (var itemPotConDet in listaPotConsgDet)
                                {
                                    bloqueConsigna[idx] =
                                        ObtenerBloqueConsignaSegundo((DateTime)itemPotConDet.Nrpcdfecha);
                                    potenciaConsigna[idx] = (decimal)itemPotConDet.Nrpcdmw;
                                    idx++;
                                }
                            }

                            if (bloqueConsigna.Length > 1 && potenciaConsigna.Length > 1)
                            {
                                decimal tomaCargaMwMin = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodimodo,
                                    ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadTomaCarga,
                                    ((DateTime)fecha));

                                decimal reduccionCargaMwMin = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodimodo,
                                    ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadReduccionCarga,
                                    ((DateTime)fecha));

                                EveHoraoperacionDTO listaHoraOperacionElemento =
                                    listaHoraOperacion.Where(
                                        x =>
                                            x.Hophorini >= fecha && x.Hophorfin < fecha.AddHours(24) &&
                                            x.Grupocodi == grupocodimodo).FirstOrDefault();

                                decimal[] medidor96 = ObtenerMedidor96Arreglo(null);

                                //obtener rpf
                                decimal rpf = 0;// ObtenerPametroValor(listaParametroValorRpf, fecha);

                                EdeExcel ex = new EdeExcel
                                {
                                    minutoSincronizacion = ConstantesReservaFriaNodoEnergetico.MinutoSincronizacion,
                                    tomaCargaMwMin = tomaCargaMwMin,
                                    reduccionCargaMwMin = reduccionCargaMwMin,
                                    fechaFalla =
                                        listaHoraOperacionElemento == null
                                            ? fecha.AddHours(24)
                                            : (listaHoraOperacionElemento.Hopfalla == "S"
                                                ? listaHoraOperacionElemento.Hophorfin
                                                : fecha.AddHours(24)),
                                    fechaSincronizacionReal =
                                        listaHoraOperacionElemento == null
                                            ? fecha.AddHours(24)
                                            : (DateTime)listaHoraOperacionElemento.Hophorini.Value,
                                    medidor96 = medidor96,
                                    bloqueConsigna = bloqueConsigna,
                                    potenciaConsigna = potenciaConsigna,
                                    potenciaLimite = 100,
                                    rpf = rpf
                                };

                                ex.Calcular(false); //obtiene solo la curva ideal

                                if (centralEde == null)
                                {
                                    centralEde = ex;
                                }
                                else
                                {
                                    centralEde.SumarCurvaIdeal(ex.curvaIdeal);
                                }
                            }
                        }
                    }


                    if (centralEde != null)
                    {
                        decimal[] edeCentral = centralEde.CalcularEde15MinSinCurvaReal();
                        decimal edeTotal = centralEde.edeTotal;

                        //almacenar resultado
                        //grabar ede en formato 15"
                        GrabarEde(edeCentral, itemCentFechUnid.PtomedicodiCentral,
                            ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fecha, lectcodi96, edeTotal);
                    }
                    else
                    {
                        //b. PDO o RDO simulado por central (datos 30")
                        int grupocodiCentral = itemCentFechUnid.GrupoCodiCentral;
                        int ptomedicodiCentral = itemCentFechUnid.PtomedicodiCentral;

                        if (ptomedicodiCentral > 0)
                        {
                            MeMedicion48DTO datosMedicion48PdoSimulado =
                                FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(ptomedicodiCentral.ToString(),
                                    ConstantesReservaFriaNodoEnergetico.Lectcodi48PdoSimulado,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                    fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault(); //List<MeMedicion48DTO> 

                            if (datosMedicion48PdoSimulado != null) //.Count > 0
                            {
                                CalcularNeEde(fecha, fecha.AddHours(24), datosMedicion48PdoSimulado, lectcodi96,
                                    ptomedicodiCentral, null);
                            }
                            else
                            {
                                MeMedicion48DTO datosMedicion48RdoSimulado =
                                    FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(ptomedicodiCentral.ToString(),
                                        ConstantesReservaFriaNodoEnergetico.Lectcodi48RdoSimulado,
                                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                        fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault();


                                if (datosMedicion48RdoSimulado != null) //.Count > 0
                                {
                                    CalcularNeEde(fecha, fecha.AddHours(24), datosMedicion48RdoSimulado, lectcodi96,
                                        ptomedicodiCentral, null);
                                }
                                else
                                {
                                    //c. PDO o RDO por central (datos de Despacho)
                                    MeMedicion48DTO datosMedicion48Pdo =
                                        FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(
                                            itemCentFechUnid.PtomedicodiCentral.ToString(),
                                            ConstantesReservaFriaNodoEnergetico.LectcodiProgramado,
                                            ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                            fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault();

                                    if (datosMedicion48Pdo != null) //.Count > 0
                                    {
                                        CalcularNeEde(fecha, fecha.AddHours(24), datosMedicion48Pdo, lectcodi96,
                                            itemCentFechUnid.PtomedicodiCentral, null);
                                    }
                                    else
                                    {
                                        MeMedicion48DTO datosMedicion48Rdo =
                                            FactorySic.GetMeMedicion48Repository().ObtenerDatosPtoMedicionLectura(
                                                itemCentFechUnid.PtomedicodiCentral.ToString(),
                                                ConstantesReservaFriaNodoEnergetico.LectcodiReprograma,
                                                ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,//ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva,
                                                fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha)).FirstOrDefault();

                                        if (datosMedicion48Rdo != null) //.Count > 0
                                        {
                                            CalcularNeEde(fecha, fecha.AddHours(24), datosMedicion48Rdo, lectcodi96,
                                                itemCentFechUnid.PtomedicodiCentral, null);
                                        }
                                    }
                                }
                            }
                        }

                        //Para la curva REAL=0.                           
                    }
                }

                return numObservaciones;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }


        /// <summary>
        /// Permite calcular el EDE de la indisponibilidad Parcial Fortuita
        /// </summary>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="nrcptcodiHora">Código de concepto (hora)</param>
        /// <param name="fechaInicio">Fecha de Inicio</param>
        /// <param name="fechaFinal">Fecha de fin</param>
        /// <param name="listaNodoEnergetico">Lista de nodo energetico</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de nodo energetico</param>
        /// <param name="nrsmodCodi">Código de módulo</param>
        /// <param name="lectcodi96">Código de lectura de Energía dejada de entregar</param>
        /// <returns></returns>
        public int CalcularNeEdeParcialFortuita(int nrperCodi, int nrcptcodiHora, DateTime fechaInicio,
    DateTime fechaFinal, List<PrGrupoDTO> listaNodoEnergetico,
    List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrsmodCodi, int lectcodi96)
        {

            try
            {
                int numObservaciones = 0;
                var listaGrupocodiModo = listaEquivalenciaNodoEnergetico.Select(x => x.GrupocodiModo);
                string grupoCodiModo = "," + String.Join(",", listaGrupocodiModo) + ",";
                var listaGrupocodi = listaNodoEnergetico.Select(x => x.Grupocodi);
                string grupoCodi = String.Join(",", listaGrupocodi);
                int[] arrGrupocodi = listaGrupocodi.ToArray();
                var listaGrupoPadre = listaNodoEnergetico.Select(x => x.Grupopadre);
                string grupoPadre = String.Join(",", listaGrupoPadre);

                //lista de horas
                List<NrProcesoDTO> listaHoras = BuscarOperaciones("N", nrperCodi, 0, nrcptcodiHora,
                    fechaInicio, fechaFinal, -1, -1);

                //lista de horas de operacion
                /*List<EveHoraoperacionDTO> listaHoraOperacion =
                    servHoraOperacion.GetByCriteria(fechaInicio, fechaFinal).Where
                        (x => x.Hophorordarranq != null && x.Grupocodi.ToString().IndexOf(grupoCodiModo) >= 0).ToList();*/

                List<EveHoraoperacionDTO> listaHoraOperacion =
                    FactorySic.GetEveHoraoperacionRepository().GetByCriteria(fechaInicio, fechaFinal).Where
                        (x => x.Grupocodi.HasValue && grupoCodiModo.Contains("," + ((int)x.Grupocodi).ToString() + ",") && x.Hophorordarranq.HasValue).ToList();


                //pasando a lista por central, fecha y unidades(modos de operacion) que la conforman
                List<CentralFechaUnidad> listaCentralFechaUnidad = ObtenerConversionUnidadCentral(listaHoras,
                    listaEquivalenciaNodoEnergetico);


                //eliminando los datos de medicion96 de fecha a fecha.
                foreach (var unidadEquivalente in listaCentralFechaUnidad)
                {
                    FactorySic.GetMeMedicion96Repository().Delete(unidadEquivalente.PtomedicodiCentral,
                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fechaInicio, fechaFinal,
                        lectcodi96);
                }


                //listado de parámetro RPF
                List<SiParametroValorDTO> listaParametroValorRpf =
                    BuscarOperaciones(ConstantesReservaFriaNodoEnergetico.Rpf,
                        fechaInicio, fechaFinal, -1, -1, "N");


                //ahora se recorre la central. Se usa sus unidades asociadas
                foreach (CentralFechaUnidad itemCentFechUnid in listaCentralFechaUnidad)
                {
                    //caso 0 A: No hay falla. Evaluación a nivel de central
                    //caso 1 B: Si hay falla sin rearranque: no hay otro registro de hora de operacion siguiente a la falla
                    //caso 2 C: Si hay falla con rearranque: (hay otro registro de hora de operación siguiente a la falla de la misma unidad con hora de arranque ingresado)

                    int casoGeneral = -1;
                    DateTime fecha = itemCentFechUnid.Fecha;
                    EdeExcel centralEde = null;
                    DateTime horainicio = new DateTime(2000, 1, 1);
                    DateTime horaFalla = new DateTime(2000, 1, 1);
                    DateTime reArranquehora = new DateTime(2000, 1, 1);


                    //se requiere todas las unidades que conforman la central
                    foreach (ModoGrupoCentralEquipo mgce0 in listaEquivalenciaNodoEnergetico)
                    {
                        //EdeExcel centralEde = null;

                        if (mgce0.GrupoCodiCentral == itemCentFechUnid.GrupoCodiCentral)
                        {
                            //Para la curva IDEAL: primera ocurrencia de los siguientes casos:

                            //DateTime fecha = itemCentFechUnid.Fecha;

                            int[] bloqueConsigna = new int[1];
                            decimal[] potenciaConsigna = new decimal[1];


                            //int casoGeneral = -1;
                            //DateTime horainicio = new DateTime(2000, 1, 1);
                            //DateTime horaFalla = new DateTime(2000, 1, 1);
                            //DateTime reArranquehora = new DateTime(2000, 1, 1);


                            #region Configuracion EDE


                            //foreach (int grupocodimodo in mgce0.GrupocodiModo)//itemCentFechUnid.GrupocodiModo)
                            //foreach (ModoGrupoCentralEquipo grupocodimodo2 in mgce0)//itemCentFechUnid.GrupocodiModo)
                            {
                                int grupocodimodo = mgce0.GrupocodiModo;// grupocodimodo2.GrupocodiModo;

                                //a. Consignas ingresadas a nivel de unidad que se evalúa por central
                                List<NrPotenciaconsignaDTO> listaConsigna = BuscarOperaciones(nrsmodCodi,
                                    grupocodimodo, fecha, fecha, "N", -1, -1);

                                //obtener rpf
                                decimal rpf = 0;// ObtenerPametroValor(listaParametroValorRpf, fecha);

                                if (listaConsigna.Count > 0)
                                {

                                    #region Consigna


                                    foreach (NrPotenciaconsignaDTO itemConsigna in listaConsigna)
                                    {
                                        List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                                            BuscarOperaciones((int)itemConsigna.Nrpccodi, fecha, fecha, -1, -1);

                                        listaPotConsgDet.Sort(
                                            (p, q) => DateTime.Compare((DateTime)p.Nrpcdfecha, (DateTime)q.Nrpcdfecha));

                                        bloqueConsigna = new int[listaPotConsgDet.Count];
                                        potenciaConsigna = new decimal[listaPotConsgDet.Count];

                                        int idx = 0;
                                        foreach (var itemPotConDet in listaPotConsgDet)
                                        {
                                            bloqueConsigna[idx] =
                                                ObtenerBloqueConsignaSegundo((DateTime)itemPotConDet.Nrpcdfecha);
                                            potenciaConsigna[idx] = (decimal)itemPotConDet.Nrpcdmw;
                                            idx++;
                                        }


                                    }

                                    if (bloqueConsigna.Length > 1 && potenciaConsigna.Length > 1)
                                    {
                                        decimal tomaCargaMwMin = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodimodo,
                                            ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadTomaCarga,
                                            ((DateTime)fecha));

                                        decimal reduccionCargaMwMin = FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodimodo,
                                            ConstantesReservaFriaNodoEnergetico.ConcepcodiVelocidadReduccionCarga,
                                            ((DateTime)fecha));

                                        List<EveHoraoperacionDTO> listaHoraOperacionElemento =
                                            listaHoraOperacion.Where(
                                                x =>
                                                    x.Hophorini >= fecha && x.Hophorfin < fecha.AddHours(24) &&
                                                    x.Grupocodi == grupocodimodo).ToList();

                                        listaHoraOperacionElemento.Sort(
                                            (p, q) =>
                                                DateTime.Compare((DateTime)p.Hophorini,
                                                    (DateTime)q.Hophorini));

                                        ModoGrupoCentralEquipo modoGrupoCentralEquipo = listaEquivalenciaNodoEnergetico.Where(x =>
                                            x.GrupocodiModo == grupocodimodo).FirstOrDefault();

                                        MeMedicion96DTO listaMedidores = FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                            ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                            modoGrupoCentralEquipo.PtomedicodiMedidor, ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores, fecha, fecha).FirstOrDefault();


                                        decimal[] medidor96 = ObtenerMedidor96Arreglo(listaMedidores);
                                        int caso = -1;
                                        if (listaHoraOperacionElemento.Count == 0)
                                        {
                                            //caso no aplica
                                        }
                                        else
                                        {
                                            if (listaHoraOperacionElemento.Count == 1)
                                            {

                                                EveHoraoperacionDTO hop = listaHoraOperacionElemento[0];
                                                //if (hop.Hopfalla != null && hop.Hopfalla == "S")
                                                if (hop.Hopfalla != null && hop.Hopfalla == "F")
                                                {   //caso 1 B: Si hay falla sin rearranque

                                                    caso = 1; //Si hay falla sin re-arranque
                                                    //casoGeneral = Math.Max(casoGeneral, caso);

                                                    //if (casoGeneral < caso)
                                                    {
                                                        horainicio = (DateTime)hop.Hophorini;
                                                        horaFalla = (DateTime)hop.Hophorfin;
                                                        casoGeneral = 1;
                                                    }

                                                }
                                                else
                                                {
                                                    //caso 0 A: No hay falla

                                                    caso = 0; //A. Si no hay falla
                                                    //casoGeneral = Math.Max(casoGeneral, caso);

                                                    //if (casoGeneral < caso)
                                                    {
                                                        horainicio = (DateTime)hop.Hophorini;
                                                        horaFalla = fecha.AddHours(24);
                                                        casoGeneral = 0;
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (listaHoraOperacionElemento.Count == 2)
                                                {
                                                    EveHoraoperacionDTO hop0 = listaHoraOperacionElemento[0];
                                                    EveHoraoperacionDTO hop1 = listaHoraOperacionElemento[1];
                                                    //caso = 2;//Si hay falla con re-arranque
                                                    //casoGeneral = Math.Max(casoGeneral, caso);

                                                    //if (casoGeneral < caso)
                                                    {
                                                        horainicio = (DateTime)hop0.Hophorini;
                                                        horaFalla = (DateTime)hop0.Hophorfin;

                                                        reArranquehora = hop1.Hophorordarranq == null
                                                            ? (DateTime)hop0.Hophorfin
                                                            : (DateTime)hop1.Hophorordarranq;

                                                        casoGeneral = 2;
                                                    }
                                                }
                                            }
                                        }

                                        if (caso >= 0)
                                        {
                                            EdeExcel ex = new EdeExcel
                                            {
                                                minutoSincronizacion = ConstantesReservaFriaNodoEnergetico.MinutoSincronizacion,
                                                tomaCargaMwMin = tomaCargaMwMin,
                                                reduccionCargaMwMin = reduccionCargaMwMin,
                                                fechaFalla = horaFalla,
                                                fechaSincronizacionReal = horainicio,
                                                medidor96 = medidor96,
                                                bloqueConsigna = bloqueConsigna,
                                                potenciaConsigna = potenciaConsigna,
                                                potenciaLimite = 0,
                                                rpf = rpf
                                            };


                                            ex.Calcular(true); //obtiene la curva ideal y real

                                            if (centralEde == null)
                                            {
                                                centralEde = ex;
                                            }
                                            else
                                            {
                                                centralEde.SumarCurvaIdeal(ex.curvaIdeal);
                                                centralEde.SumarCurvaReal(ex.curvaReal);
                                            }
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    //sin consigna
                                    //debe sumar los medidores

                                    /*ModoGrupoCentralEquipo modoGrupoCentralEquipo = listaEquivalenciaNodoEnergetico.Where(x =>
                                            x.GrupocodiModo == grupocodimodo).FirstOrDefault();*/

                                    List<EveHoraoperacionDTO> listaHoraOperacionElemento =
                                           listaHoraOperacion.Where(
                                               x =>
                                                   x.Hophorini >= fecha && x.Hophorfin < fecha.AddHours(24) &&
                                                   x.Grupocodi == grupocodimodo).ToList();


                                    if (listaHoraOperacionElemento.Count > 0)
                                    {



                                        MeMedicion96DTO listaMedidores = FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                            ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                            mgce0.PtomedicodiMedidor, ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores, fecha, fecha).FirstOrDefault();


                                        if (listaMedidores != null)
                                        {


                                            decimal[] medidor96 = ObtenerMedidor96Arreglo(listaMedidores);

                                            EdeExcel ex = new EdeExcel
                                            {
                                                minutoSincronizacion = ConstantesReservaFriaNodoEnergetico.MinutoSincronizacion,
                                                tomaCargaMwMin = 0,
                                                reduccionCargaMwMin = 0,
                                                fechaFalla = horaFalla,
                                                fechaSincronizacionReal = horainicio,
                                                medidor96 = medidor96,
                                                bloqueConsigna = bloqueConsigna,
                                                potenciaConsigna = potenciaConsigna,
                                                potenciaLimite = 0,
                                                rpf = rpf
                                            };


                                            ex.Calcular(false); //obtiene la curva ideal y real

                                            ex.ObtenerCurvaRealDesdeMedidores(medidor96);
                                            ex.SumarCurvaReal(ex.curvaReal);


                                            if (centralEde == null)
                                            {
                                                centralEde = ex;
                                            }
                                            else
                                            {
                                                centralEde.SumarCurvaIdeal(ex.curvaIdeal);
                                                centralEde.SumarCurvaReal(ex.curvaReal);
                                            }

                                        }

                                    }

                                }
                            }
                            #endregion


                        }
                    }



                    #region Central EDE


                    if (centralEde != null)
                    {
                        //se varifica el caso máximo de:
                        //caso 0 A: No hay falla. Evaluación a nivel de central
                        //caso 1 B: Si hay falla sin rearranque: no hay otro registro de hora de operacion siguiente a la falla
                        //caso 2 C: Si hay falla con rearranque: (hay otro registro de hora de operación siguiente a la falla de la misma unidad con hora de arranque ingresado)

                        switch (casoGeneral)
                        {
                            case 0:
                                centralEde.CalcularEde();

                                //Almacenar resultado. Grabar ede en formato 15"
                                GrabarEde(centralEde.curvaEde15Minuto, itemCentFechUnid.PtomedicodiCentral,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fecha, lectcodi96, centralEde.edeTotal);
                                break;
                            case 1:
                                centralEde.CalcularEde();

                                //Almacenar resultado. Grabar ede en formato 15"
                                GrabarEde(centralEde.curvaEde15Minuto, itemCentFechUnid.PtomedicodiCentral,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fecha, lectcodi96, centralEde.edeTotal);
                                break;
                            case 2:
                                //decimal[] edeCentral = centralEde.CalcularEde15MinSinCurvaReal();
                                //decimal edeTotal = centralEde.edeTotal;

                                //EDE indisp fortuita parcial (hasta hora de falla)
                                //hallar EDE hasta hora de falla
                                decimal edeTotal15 = 0;
                                decimal[] ede15Min = centralEde.CalcularEde15(0, ObtenerBloqueConsignaSegundo(horaFalla),
                                    ref edeTotal15);
                                //EDE indisp fortuita parcial
                                //Almacenar resultado. Grabar ede en formato 15"
                                GrabarEde(ede15Min, itemCentFechUnid.PtomedicodiCentral, ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fecha, lectcodi96, edeTotal15);


                                //EDE indisp fortuita total (de hora de falla a hora de rearranque)
                                decimal edeTotal15Falla = 0;
                                decimal[] ede15MinFalla = centralEde.CalcularEde15(ObtenerBloqueConsignaSegundo(horaFalla), ObtenerBloqueConsignaSegundo(reArranquehora),
                                    ref edeTotal15Falla);

                                //EDE indisp fortuita total
                                //Almacenar resultado. Grabar ede en formato 15"
                                GrabarEde(ede15MinFalla, itemCentFechUnid.PtomedicodiCentral,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, fecha,
                                    ConstantesReservaFriaNodoEnergetico.Lectcodi96EdeNodoIndispTotalFort, edeTotal15Falla);

                                break;
                        }
                    }
                    #endregion



                }

                return numObservaciones;
            }
            catch (Exception e)
            {

                return -1;
            }
        }



        /// <summary>
        /// Permite realizar una conversión de unidades con sus respectivas centrales de generación
        /// </summary>
        /// <param name="listaHoras">Lista de horas</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de Nodo Energetico</param>
        /// <returns></returns>
        public List<CentralFechaUnidad> ObtenerConversionUnidadCentral(List<NrProcesoDTO> listaHoras, List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico)
        {
            List<CentralFechaUnidad> centFechaUnid = new List<CentralFechaUnidad>();

            foreach (NrProcesoDTO itemHoras in listaHoras)
            {

                List<ModoGrupoCentralEquipo> listaEquivalencia = listaEquivalenciaNodoEnergetico.Where(x => x.GrupocodiModo == itemHoras.Grupocodi).ToList();

                //filtrando
                foreach (ModoGrupoCentralEquipo itemEquivalencia in listaEquivalencia)
                {
                    int ptomedicodiCentralNodo = itemEquivalencia.PtomedicodiCentralNodoEnergetico;

                    //filtro por fecha y central                            
                    List<CentralFechaUnidad> listaCentFechaUnidFiltrada =
                        centFechaUnid.Where(
                            x =>
                                x.Fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha) ==
                                (((DateTime)itemHoras.Nrprcfechainicio)).ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha) &&
                                x.GrupoCodiCentral == itemEquivalencia.GrupoCodiCentral).ToList();


                    if (listaCentFechaUnidFiltrada.Count == 0)
                    {   //no existe como central y fecha

                        //crear unidad central, fecha y uidad
                        //DateTime fecha = Convert.ToDateTime(((DateTime)itemHoras.Nrprcfechainicio).ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha));
                        DateTime fecha = DateTime.ParseExact(
                            ((DateTime)itemHoras.Nrprcfechainicio).ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha),
                            ConstantesReservaFriaNodoEnergetico.FormatoFecha,
                            CultureInfo.InvariantCulture);


                        int grupocodicentral = itemEquivalencia.GrupoCodiCentral;
                        CentralFechaUnidad centfecunid = new CentralFechaUnidad(grupocodicentral, fecha, ptomedicodiCentralNodo);
                        centfecunid.AgregarGrupocodimodo((int)itemHoras.Grupocodi);
                        centFechaUnid.Add(centfecunid);
                    }
                    else
                    {
                        //existe como central y fecha

                        //existe como unidad?
                        foreach (var itemcenFechunid in centFechaUnid)
                        {
                            if (itemcenFechunid.Fecha.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha) ==
                                ((DateTime)(itemHoras.Nrprcfechainicio)).ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha) &&
                                itemcenFechunid.GrupoCodiCentral == itemEquivalencia.GrupoCodiCentral)
                            {
                                if (itemcenFechunid.GrupocodiModo.IndexOf((int)itemHoras.Grupocodi) < 0)
                                {
                                    itemcenFechunid.GrupocodiModo.Add((int)itemHoras.Grupocodi);
                                }
                            }
                        }
                    }
                }
            }

            return centFechaUnid;
        }


        /// <summary>
        /// Permite calcular el EDE dada una fecha inicial, final y datos cada 30 minutos
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="datosMedicionesIdeal">DTO de Medición48</param>
        /// <param name="lectcodi96">Código de lectura de 15 minutos</param>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <param name="itemMedReal">DTO de medición real</param>
        private void CalcularNeEde(DateTime fechaInicial, DateTime fechaFinal, MeMedicion48DTO datosMedicionesIdeal,
            int lectcodi96, int ptomedicodi, MeMedicion48DTO itemMedReal)
        {
            int h0, h1;
            //h0 = fechaInicial.Hour * 2 + (fechaInicial.Minute / 30);
            h0 = fechaInicial.Hour * 2 + (fechaInicial.Minute / 30);//+ 1;

            if (fechaInicial.Day != fechaFinal.Day)
            {
                h1 = 48;//h1 = 96;
            }
            else
            {
                h1 = fechaFinal.Hour * 2 + (fechaFinal.Minute / 30);
            }

            h0 += 1;

            if (h1 < h0)
                h1 = h0;

            MeMedicion48DTO itemMedIdeal = datosMedicionesIdeal;

            decimal[] resultadoEde = new decimal[96];
            decimal energiaTotal = 0;

            for (int i = 0; i < 96; i++)
            {
                resultadoEde[i] = 0;
            }

            if (itemMedIdeal != null)
            {
                int rec = 2 * h0 - 1;

                for (int hi = h0; hi <= h1; hi++)
                {
                    decimal potenciaIdeal =
                        (decimal)
                        itemMedIdeal.GetType()
                            .GetProperty("H" + hi.ToString())
                            .GetValue(itemMedIdeal, null);

                    decimal potenciaReal = itemMedReal == null
                        ? 0
                        : (decimal)
                        itemMedReal.GetType()
                            .GetProperty("H" + hi.ToString())
                            .GetValue(itemMedReal, null);

                    decimal potencia = potenciaIdeal - potenciaReal;

                    if (potencia > 0)
                    {
                        decimal energia = potencia / 2;

                        //energia prorrateada
                        if (hi == h0)
                        {
                            if (fechaInicial.Minute != 0)
                            {
                                //tiempo al bloque superior
                                int minuto = 30 - fechaInicial.Minute % 30;
                                //energía proporcional
                                energia *= minuto;
                            }
                        }

                        //energia prorrateada
                        if (hi == h1)
                        {
                            //tiempo al bloque inferior
                            if (fechaFinal.Minute != 0)
                            {
                                int minuto = fechaFinal.Minute % 30;
                                //energía proporcional
                                energia *= minuto;
                            }

                        }

                        resultadoEde[rec - 1] = energia / (decimal)2.0;//energia;
                        resultadoEde[rec] = energia / (decimal)2.0;//energia;
                        energiaTotal += energia;//2 * energia;
                        rec += 2;
                    }
                }
            }

            //grabar ede en formato 15"
            GrabarEde(resultadoEde, ptomedicodi,
                ConstantesReservaFriaNodoEnergetico.TipoinfocodiEnergiaActiva, itemMedIdeal.Medifecha, lectcodi96, energiaTotal);
        }


        /// <summary>
        /// Permite almacenar la EDE en la base de datos
        /// </summary>
        /// <param name="resultadoEde">arreglo EDE cada 15 minutos</param>
        /// <param name="ptomedicodi">Código de punto de medición</param>
        /// <param name="tipoinfocodi">Código de tipo de información</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="lectcodi">Código de lectura</param>
        /// <param name="mediTotal">Total de medición</param>
        private void GrabarEde(decimal[] resultadoEde, int ptomedicodi, int tipoinfocodi, DateTime fecha, int lectcodi, decimal mediTotal)
        {
            MeMedicion96DTO med96 = new MeMedicion96DTO();

            med96.Lectcodi = lectcodi;
            med96.Medifecha = fecha;
            med96.Tipoinfocodi = tipoinfocodi;
            med96.Ptomedicodi = ptomedicodi;
            med96.Meditotal = mediTotal;

            for (int hi = 0; hi < 96; hi++)
            {
                med96.GetType().GetProperty("H" + (hi + 1)).SetValue(med96, resultadoEde[hi]);
            }

            FactorySic.GetMeMedicion96Repository().Save(med96);
        }


        /// <summary>
        /// Permite evaluar las horas del nodo energetico
        /// </summary>
        /// <param name="ptomedicodiStock">Codigo de stock de combustible</param>
        /// <param name="fechaRec">Fecha a consultar</param>
        /// <param name="grupocodi">grupocodi</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de equivalencia de Nodo Energetico</param>
        /// <param name="listaParametroValorRatio">lista de Ratio</param>
        /// <param name="listaParametroValorRpf">lista de RPF</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="unidadEquivalente">Unidad equivalente</param>
        /// <param name="potenciaSuma">Producto de rendimiento y energía</param>
        /// <param name="rendimientoSuma">Suma de rendimiento</param>
        /// <param name="unidades">Número de unidades</param>
        /// <param name="usuario">Usuario del sistema</param>
        private int EvaluarHoraNeCasos(int ptomedicodiStock, DateTime fechaRec,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico,
            List<SiParametroValorDTO> listaParametroValorRatio, List<SiParametroValorDTO> listaParametroValorRpf,
            int nrperCodi, ModoGrupoCentralEquipo unidadEquivalente, decimal potenciaSuma, decimal rendimientoSuma,
            int unidades, string usuario, ref ArrayList FechaIngresoCentral, decimal potenciaAdjudicadaSuma, ref string logUnidad,
            List<ModoGrupoCentralEquipo> unidadEquivalenteCentral, ref Hashtable listaFechaModoValor)
        {

            try
            {

                #region diagrama simplificado
                /*
                 
                 (A)
                 (B)
                 if (C)>15%
                 {
                  (D)
                  (F)
                  if (E)>15%
                    HIPP <- (F)
                  else
                    HIPF <- (F)
                 }
                 else
                 { //evaluar modos de operacion
                   HIPP caso 1,2
                   HIPF caso 1,2                 
                 }
                 
                 
                 */
                #endregion
                #region Evaluacion casos

                //evaluación de la formula
                if (ptomedicodiStock != -1)
                {
                    List<MeMedicionxintervaloDTO> listastock = FactorySic.GetMeMedicionxintervaloRepository().List(ptomedicodiStock,
                        ConstantesReservaFriaNodoEnergetico.LectcodiStock,
                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiVolumen, fechaRec, fechaRec);

                    string observacion = "";

                    decimal volumenFinalDiarioCombustible = listastock.Count == 0
                        ? 0
                        : (decimal)listastock[0].Medinth1;
                    //pasando el volumen de gal a m3. m3*(galon/m3)
                    volumenFinalDiarioCombustible *= ConstantesReservaFriaNodoEnergetico.RatioM3aGalon;

                    if (volumenFinalDiarioCombustible == 0)
                    {
                        observacion += ConstantesReservaFriaNodoEnergetico.NotaNoHayVolumenFinalDiarioCombustible;
                    }

                    logUnidad += ",volumenFinalDiarioCombustible," + volumenFinalDiarioCombustible;

                    decimal potenciaCalculada = 0;
                    if (unidades > 0)
                    {   //*** A ***
                        potenciaCalculada = (decimal)(1 / 24.0) *
                                            (volumenFinalDiarioCombustible * rendimientoSuma /
                                             (decimal)(1000.0 * unidades) +
                                             potenciaSuma / (decimal)2.0);
                    }

                    logUnidad += ",unidades," + unidades + ",rendimientoSuma," + rendimientoSuma + ",potenciaSuma," + potenciaSuma + ",potenciaCalculada," + potenciaCalculada;

                    //*** B ***
                    decimal potenciaCalculadaPrima = Math.Min(600, potenciaCalculada);
                    logUnidad += ",potenciaCalculadaPrima," + potenciaCalculadaPrima;

                    decimal potenciaAdjudicadaCentral = potenciaAdjudicadaSuma;// FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(grupocodi,
                    //ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada,
                    //fechaRec);
                    logUnidad += ",potenciaAdjudicadaCentral," + potenciaAdjudicadaCentral;// +"\r\n";

                    if ((potenciaAdjudicadaCentral != 0) && (unidades > 0))
                    {
                        //*** C ***
                        decimal ratio = 1 - potenciaCalculadaPrima / potenciaAdjudicadaCentral;
                        logUnidad += ",1 - potenciaCalculadaPrima / potenciaAdjudicadaCentral," + ratio;

                        decimal ratioPotCalcPotAdjudicada = ObtenerPametroValor(
                            listaParametroValorRatio, fechaRec);

                        //ejemplo: consulta tabla pr_consumocomb : 
                        //combcodi:14 B5
                        //evenclasecodi:2 PROGRAMADO DIARIO
                        //tipoinfocodi:15 galones (*1)

                        logUnidad += ",ratio > ratioPotCalcPotAdjudicada," + (ratio > ratioPotCalcPotAdjudicada) + ",ratioPotCalcPotAdjudicada," + ratioPotCalcPotAdjudicada;

                        if (ratio > ratioPotCalcPotAdjudicada)
                        {

                            /*
                            MeMedicion1DTO volumen1 =
                                FactorySic.GetMeMedicion1Repository().GetByCriteria(fechaRec, fechaRec,
                                    ConstantesReservaFriaNodoEnergetico.LectcodiStockProgramado,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiBiodieselGalon,
                                    unidadEquivalente.PtomedicodiStock).FirstOrDefault();
                            */

                            MeMedicion1DTO volumen1 =
                                FactorySic.GetMeMedicion1Repository().GetByCriteria(fechaRec, fechaRec,
                                    ConstantesReservaFriaNodoEnergetico.LectcodiStockProgramado,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiBiodieselGalon,
                                    unidadEquivalente.PtomedicodiCombProgramado.ToString()).FirstOrDefault();


                            decimal volumenCombustibleProgramado = (volumen1 == null
                                ? 0
                                : (decimal)volumen1.H1);

                            if (volumenCombustibleProgramado == 0)
                            {
                                observacion += ConstantesReservaFriaNodoEnergetico.NotaNoHayCombustibleProgramado;
                            }

                            /*
                            PrConsumocombDTO consumoComb =
                                FactorySic.GetPrConsumocombRepository().BuscarOperaciones(fechaRec, fechaRec,
                                        ConstantesReservaFriaNodoEnergetico.CombcodiB5,
                                        unidadEquivalente.GrupoCodiCentral,
                                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiGalon,
                                        ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario, -1, -1)
                                    .FirstOrDefault();

                            decimal volumenProgDiarioCombustible = consumoComb == null ? 0 : consumoComb.Ccvfin.Value;
                            logUnidad+=",volumenProgDiarioCombustible,"+volumenProgDiarioCombustible;
                            */
                            //*** D ***
                            //obtener de BD (*1) 
                            decimal potenciaCalculadaProgramada = (1 / 24) * volumenCombustibleProgramado *
                                                                  rendimientoSuma / (1000 * unidades); //PCP

                            logUnidad += ",potenciaCalculadaProgramada," + potenciaCalculadaProgramada;
                            //*** F ***
                            //HIPX - Caso 1 : HoraPF
                            //•	Se obtiene el volumen de combustible programado de extranet (medicion1)
                            /*
                            MeMedicion1DTO volumen1 =
                                FactorySic.GetMeMedicion1Repository().GetByCriteria(fechaRec, fechaRec,
                                    ConstantesReservaFriaNodoEnergetico.LectcodiStockProgramado,
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiBiodieselGalon,
                                    unidadEquivalente.PtomedicodiStock).FirstOrDefault();

                            decimal volumenCombustibleProgramado = (volumen1 == null
                                ? 0
                                : (decimal)volumen1.H1);
                            */

                            logUnidad += ",volumenCombustibleProgramado," + volumenCombustibleProgramado;


                            //rendimiento de la unidad en modo Diesel

                            //P. Límite
                            decimal potenciaLimite = 0;//rendimientoSuma * volumenCombustibleProgramado / 24 +
                            // potenciaSuma / 2000;

                            //Potencia Restringida=P.Adjudicada-P.Límite
                            //decimal potenciaRestringida = potenciaAdjudicadaCentral - potenciaLimite;
                            decimal potenciaRestringida = potenciaAdjudicadaCentral - potenciaCalculadaPrima;
                            logUnidad += ",potenciaRestringida," + potenciaRestringida;
                            decimal horasPorCentral = 24 * potenciaRestringida / potenciaAdjudicadaCentral; //0;
                            logUnidad += ",horasPorCentral," + horasPorCentral;

                            //Si Potencia Restringida > 10% P. Adjudicada, calcular
                            //Xi: ((24)Potencia Restringida)/(P.Adjudicada)
                            //if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaAdjudicadaCentral)
                            /*
                            if (potenciaCalculadaPrima >potenciaCalculadaProgramada)
                            {
                                horasPorCentral = 24 * potenciaRestringida / potenciaAdjudicadaCentral;
                            }
                            */
                            //*** E ***
                            //if (1 - potenciaCalculadaProgramada / potenciaAdjudicadaCentral >  ratioPotCalcPotAdjudicada)
                            logUnidad += ",potenciaCalculadaPrima > potenciaCalculadaProgramada," + (potenciaCalculadaPrima > potenciaCalculadaProgramada);

                            if (potenciaCalculadaPrima > potenciaCalculadaProgramada)
                            {
                                //HIPP = HoraPF
                                decimal horasIndispParcProgramada = horasPorCentral;
                                logUnidad += ",horasIndispParcProgramada," + horasIndispParcProgramada;
                                //crear registro
                                if (FechaIngresoCentral.IndexOf(fechaRec.ToString("dd/MM/yyyy") + "*" + unidadEquivalente.GrupoCodiCentral) < 0)
                                {
                                    CreaRegistroProceso(nrperCodi, unidadEquivalente.GrupoCodiCentral,
                                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada, fechaRec,
                                        fechaRec.AddHours(24),
                                        0, horasIndispParcProgramada, potenciaLimite, potenciaRestringida,
                                        potenciaAdjudicadaCentral, 0,
                                        0, ratioPotCalcPotAdjudicada, volumenCombustibleProgramado, rendimientoSuma, 0, -1,
                                        "N",
                                        "N", "A", "N", 0, "", "", observacion, "N", 0, 0, usuario, DateTime.Now, null, null);

                                    FechaIngresoCentral.Add(fechaRec.ToString("dd/MM/yyyy") + "*" + unidadEquivalente.GrupoCodiCentral);
                                }
                            }
                            else
                            {
                                //HIPF = HoraPF
                                decimal horasIndispParcFort = horasPorCentral;
                                logUnidad += ",horasIndispParcFort," + horasIndispParcFort;
                                //crear registro
                                if (FechaIngresoCentral.IndexOf(fechaRec.ToString("dd/MM/yyyy") + "*" + unidadEquivalente.GrupoCodiCentral) < 0)
                                {
                                    CreaRegistroProceso(nrperCodi, unidadEquivalente.GrupoCodiCentral,
                                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada, fechaRec,
                                        fechaRec.AddHours(24),
                                        0, horasIndispParcFort, potenciaLimite, potenciaRestringida, potenciaAdjudicadaCentral, 0,
                                        0, ratioPotCalcPotAdjudicada, volumenCombustibleProgramado, rendimientoSuma, 0, -1,
                                        "N",
                                        "N", "A", "N", 0, "", "", observacion, "N", 0, 0, usuario, DateTime.Now, null, null);

                                    FechaIngresoCentral.Add(fechaRec.ToString("dd/MM/yyyy") + "*" + unidadEquivalente.GrupoCodiCentral);

                                }

                            }
                        }
                        else
                        {

                            foreach (ModoGrupoCentralEquipo mgce in unidadEquivalenteCentral)
                            {
                                //se evalúan los siguientes casos hasta obtener la primera ocurrencia y en el orden descrito
                                // HIPP - Caso 1, HIPP - Caso 2, HIPF - Caso 1, HIPF - Caso 2
                                bool registroCreado = false;

                                // HIPP - Caso 1
                                #region HIPP - Caso 1
                                registroCreado = ObtenerHorasIndispParcProgramCaso1(fechaRec,
                                    mgce, ratioPotCalcPotAdjudicada,
                                    listaEquivalenciaNodoEnergetico, nrperCodi,
                                    ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada,
                                    ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita,
                                    ConstantesReservaFriaNodoEnergetico.NotaHippcaso1,
                                    usuario, potenciaAdjudicadaCentral, listaFechaModoValor);


                                if (registroCreado)
                                {
                                    logUnidad += ",HIPP - Caso 1," + registroCreado;
                                }
                                #endregion

                                // HIPP - Caso 2
                                #region HIPP - Caso 2
                                //Operó con orden de máxima generación (en potencia consigna) con restricciones 
                                //(de Restricciones operativas y en horizonte Ejecutado)
                                if (!registroCreado)
                                {
                                    registroCreado = ObtenerHorasIndispParcProgramCaso2(fechaRec,
                                        mgce, ratioPotCalcPotAdjudicada, listaEquivalenciaNodoEnergetico,
                                        nrperCodi, ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialProgramada,
                                    ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita,
                                    ConstantesReservaFriaNodoEnergetico.NotaHippcaso2, usuario, potenciaAdjudicadaCentral,
                                    listaParametroValorRpf, listaFechaModoValor);
                                    logUnidad += ",HIPP - Caso 2," + registroCreado;

                                }
                                #endregion

                                // HIPF - Caso 1

                                #region HIPF - Caso 1

                                if (!registroCreado)
                                {
                                    registroCreado = ObtenerHorasIndispParcFortuitaCaso1(fechaRec,
                                        mgce, ratioPotCalcPotAdjudicada,
                                        listaEquivalenciaNodoEnergetico, nrperCodi,
                                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita,
                                        ConstantesReservaFriaNodoEnergetico.NotaHipfcaso1,
                                        usuario, potenciaAdjudicadaCentral, listaFechaModoValor, potenciaAdjudicadaCentral);
                                    logUnidad += ",HIPF - Caso 1," + registroCreado;

                                }


                                #endregion

                                // HIPF - Caso 2

                                #region HIPF - Caso 2

                                if (!registroCreado)
                                {
                                    registroCreado = ObtenerHorasIndispParcFortuitaCaso2(fechaRec,
                                        mgce,
                                        listaParametroValorRpf, ratioPotCalcPotAdjudicada,
                                        listaEquivalenciaNodoEnergetico, nrperCodi,
                                        ConstantesReservaFriaNodoEnergetico.NeHorasIndispParcialFortuita,
                                        ConstantesReservaFriaNodoEnergetico.NotaHipfcaso2, usuario, potenciaAdjudicadaCentral, listaFechaModoValor);

                                    logUnidad += ",HIPF - Caso 2," + registroCreado;

                                }

                                #endregion

                            }



                        }

                    }

                    logUnidad += "\r\n";
                }

                #endregion

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /*
        /// <summary>
        /// Permite obtener horas de indisponibilidad Parcial Programada o Fortuita (Caso 1)
        /// </summary>
        /// <param name="programada">Verdadero, si se desea calcular las horas de indisponibilidad programada</param>
        /// <param name="fechaRec">Fecha de consulta</param>
        /// <param name="unidadEquivalente">Unidad de equivalencia</param>
        /// <param name="ratioPotCalcPotAdjudicada">Ratio de Potencia Calculada y Adjudicada</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de unidad equivalente</param>
        /// <param name="nrperCodi">Codigo de periodo</param>
        /// <param name="nrcptcodi">Codigo de concepto</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        private bool ObtenerHorasIndispParcProgramFortuitaCaso1(bool programada, DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, decimal ratioPotCalcPotAdjudicada,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrperCodi, int nrcptcodi, string notaAutomatica, string usuario)
        {
            bool registroCreado = false;

            int evenclasecodi;

            if (programada)
            {
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            }
            else
            {
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            }

            // HIPP - Caso 1
            #region HIPP - Caso 1

            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVarias = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();

            if (listaOperacVarias.Count > 0)
            {
                //Se obtiene la Potencia límite desde el registro de Restricciones Operativas en horizonte Ejecutado cuyo valor es (A).
                decimal potenciaLimite = (decimal)listaOperacVarias[0].Icvalor1;

                //Potencia Restringida=P.Efectiva Unidad-(A)
                decimal potenciaEfectivaModo =
                    FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                        fechaRec);

                decimal potenciaRestringida = potenciaEfectivaModo - potenciaLimite;

                //Si la Potencia Restringida > 10% P. Efectiva, se calcula:
                if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                {
                    //X_i=((Hora final consigna(j+1)– Hora inicial consigna(j))*Potencia Restringida)/(P.Efectiva)
                    decimal horasCasoHipp = (decimal)((TimeSpan)
                    ((DateTime)listaOperacVarias[0].Ichorfin -
                     (DateTime)listaOperacVarias[0].Ichorini)).TotalHours;

                    decimal horasUnidad = horasCasoHipp * potenciaRestringida / potenciaEfectivaModo;

                    //Se hallan las Horas por Central:
                    //Horas=(Padjudicada(i))/((Suma Padjudicada(i)) X_i
                    decimal proporcion = ObtenerPropocionPropiedadModoCentral(unidadEquivalente.GrupocodiModo,
                        ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada, fechaRec,
                        listaEquivalenciaNodoEnergetico);
                    decimal horasCentral = horasUnidad * proporcion;

                    //crea registro
                    
                    //CreaRegistroProceso(nrperCodi,
                    //    unidadEquivalente.GrupoCodiGrupo, nrcptcodi,
                    //    (DateTime)listaOperacVarias[0].Ichorini, (DateTime)listaOperacVarias[0].Ichorfin,
                    //    horasUnidad,
                    //    horasCentral, 0, potenciaRestringida,
                    //    0, potenciaEfectivaModo,
                    //    0, ratioPotCalcPotAdjudicada, 0, 1,
                    //    0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                    //    0, 1, usuario,
                    //    DateTime.Now, null, null
                    //);
                    
                    CreaRegistroProceso(nrperCodi,
                        unidadEquivalente.GrupocodiModo, nrcptcodi,
                        (DateTime)listaOperacVarias[0].Ichorini, (DateTime)listaOperacVarias[0].Ichorfin,
                        horasUnidad,
                        horasCentral, 0, potenciaRestringida,
                        0, potenciaEfectivaModo,
                        0, ratioPotCalcPotAdjudicada, 0, 1,
                        0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                        0, 1, usuario,
                        DateTime.Now, null, null
                    );

                    registroCreado = true;

                }
            }

            #endregion
            
            return registroCreado;
        }
        */


        /// <summary>
        /// Permite obtener horas de indisponibilidad Parcial Programada (Caso 1)
        /// </summary>
        /// <param name="fechaRec">Fecha de consulta</param>
        /// <param name="unidadEquivalente">Unidad de equivalencia</param>
        /// <param name="ratioPotCalcPotAdjudicada">Ratio de Potencia Calculada y Adjudicada</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de unidad equivalente</param>
        /// <param name="nrperCodi">Codigo de periodo</param>
        /// <param name="nrcptcodiProgramado">Codigo de concepto</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        private bool ObtenerHorasIndispParcProgramCaso1(DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, decimal ratioPotCalcPotAdjudicada,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrperCodi,
            int nrcptcodiProgramado, int nrcptcodiFortuito, string notaAutomatica, string usuario,
            decimal potenciaAdjudicadaCentral, Hashtable listaFechaModoValor)
        {
            bool registroCreado = false;
            bool registroCreado1 = false;
            bool registroCreado2 = false;

            decimal potenciaEfectivaModo =
                    FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                        fechaRec);

            int evenclasecodi;

            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasProgramado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();



            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasEjecutado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();


            // HIPP - Caso 1
            #region HIPP - Caso 1

            if ((listaOperacVariasProgramado.Count > 0) && listaOperacVariasEjecutado.Count > 0)
            {
                ManttoResultado mantoRes = new ManttoResultado();

                foreach (EveIeodcuadroDTO ip in listaOperacVariasProgramado)
                {
                    decimal valor = ip.Icvalor2 == null ? 0 : (decimal)ip.Icvalor2;
                    mantoRes.AsignaMantoProgramado(ip, 1, valor);
                }

                foreach (EveIeodcuadroDTO ip in listaOperacVariasEjecutado)
                {
                    decimal valor = ip.Icvalor2 == null ? 0 : (decimal)ip.Icvalor2;
                    mantoRes.AsignaMantoEjecutadoProgramado(ip, 2, 3, valor);
                }


                List<EveIeodcuadroDTO> TramoReducidoProgramado = mantoRes.ListaReducidaIndispTotalProgramada(fechaRec);
                List<EveIeodcuadroDTO> TramoReducidoFortuito = mantoRes.ListaReducidaIndispTotalFortuita(fechaRec);

                decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);

                //***********
                //  HIPP
                //***********
                foreach (EveIeodcuadroDTO itemRedProg in TramoReducidoProgramado)
                {
                    decimal valorA = itemRedProg.Icvalor1 == null ? 0 : (decimal)itemRedProg.Icvalor1;
                    decimal valorB = itemRedProg.Icvalor2 == null ? 0 : (decimal)itemRedProg.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado1 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedProg, potenciaAdjudicadaCentral);

                    }

                }

                //***********
                // HIPF
                //***********

                foreach (EveIeodcuadroDTO itemRedFort in TramoReducidoFortuito)
                {
                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {
                        /*
                        registroCreado2 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedFort,potenciaAdjudicadaCentral);
                        */

                        /*
                        registroCreado2 = ObtenerHorasIndispParcProgramCaso1Auxiliar(0, valorB, fechaRec,
      unidadEquivalente, ratioPotCalcPotAdjudicada,
      listaEquivalenciaNodoEnergetico, nrperCodi,
      nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
      potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedFort, potenciaAdjudicadaCentral);
                        */



                        decimal potenciaRestringidaA = potenciaAdjudicadaUnidad - valorB;

                        decimal horasUnidad = ((decimal)((DateTime)itemRedFort.Ichorfin - (DateTime)itemRedFort.Ichorini).TotalHours) * potenciaRestringidaA / potenciaAdjudicadaUnidad;

                        decimal proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;
                        decimal horasCentral = horasUnidad * proporcion;

                        if (horasUnidad > 0)
                        {
                            CreaRegistroProceso(nrperCodi,
                    unidadEquivalente.GrupocodiModo, nrcptcodiFortuito,
                    (DateTime)itemRedFort.Ichorini, (DateTime)itemRedFort.Ichorfin,
                    horasUnidad,
                    horasCentral, 0, potenciaRestringidaA,
                    0, potenciaEfectivaModo,
                    0, ratioPotCalcPotAdjudicada, 0, 1,
                    0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                    0, 1, usuario,
                    DateTime.Now, null, null);
                            registroCreado = true;
                        }

                    }

                }


            }



            #endregion

            registroCreado = registroCreado1 || registroCreado2;

            return registroCreado;

        }

        private bool ObtenerHorasIndispParcProgramCaso1Auxiliar(decimal valorA, decimal valorB, DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, decimal ratioPotCalcPotAdjudicada,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrperCodi,
            int nrcptcodiProgramado, int nrcptcodiFortuito, string notaAutomatica, string usuario,
            decimal potenciaAdjudicadaUnidad, decimal potenciaRestringida, decimal potenciaEfectivaModo, EveIeodcuadroDTO itemRedProg, decimal potenciaAdjudicadaCentral)
        {
            bool registroCreado = false;
            decimal proporcion = 0;

            if (valorA > valorB)
            {
                //HIPP
                decimal horasUnidad = ((decimal)((DateTime)itemRedProg.Ichorfin - (DateTime)itemRedProg.Ichorini).TotalHours) * (potenciaAdjudicadaUnidad - valorA) / potenciaAdjudicadaUnidad;
                //decimal proporcion = ObtenerPropocionPropiedadModoCentral(unidadEquivalente.GrupocodiModo,
                //ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada, fechaRec,
                //listaEquivalenciaNodoEnergetico);
                proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;

                decimal horasCentral = horasUnidad * proporcion;

                if (horasUnidad > 0)
                {
                    CreaRegistroProceso(nrperCodi,
            unidadEquivalente.GrupocodiModo, nrcptcodiProgramado,
            (DateTime)itemRedProg.Ichorini, (DateTime)itemRedProg.Ichorfin,
            horasUnidad,
            horasCentral, 0, potenciaRestringida,
            0, potenciaEfectivaModo,
            0, ratioPotCalcPotAdjudicada, 0, 1,
            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
            0, 1, usuario,
            DateTime.Now, null, null);

                    registroCreado = true;

                }

                //HIPF
                horasUnidad = ((decimal)((DateTime)itemRedProg.Ichorfin - (DateTime)itemRedProg.Ichorini).TotalHours) * (valorA - valorB) / potenciaAdjudicadaUnidad;
                /*
                proporcion = ObtenerPropocionPropiedadModoCentral(unidadEquivalente.GrupocodiModo,
        ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada, fechaRec,
        listaEquivalenciaNodoEnergetico);
                */
                proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;
                horasCentral = horasUnidad * proporcion;

                if (horasUnidad > 0)
                {
                    CreaRegistroProceso(nrperCodi,
            unidadEquivalente.GrupocodiModo, nrcptcodiFortuito,
            (DateTime)itemRedProg.Ichorini, (DateTime)itemRedProg.Ichorfin,
            horasUnidad,
            horasCentral, 0, potenciaRestringida,
            0, potenciaEfectivaModo,
            0, ratioPotCalcPotAdjudicada, 0, 1,
            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
            0, 1, usuario,
            DateTime.Now, null, null);

                    registroCreado = true;

                }



            }
            else
            {
                //HIPP
                decimal potenciaRestringidaA = potenciaAdjudicadaUnidad - valorB;

                decimal horasUnidad = ((decimal)((DateTime)itemRedProg.Ichorfin - (DateTime)itemRedProg.Ichorini).TotalHours) * potenciaRestringidaA / potenciaAdjudicadaUnidad;
                /*
                decimal proporcion = ObtenerPropocionPropiedadModoCentral(unidadEquivalente.GrupocodiModo,
        ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada, fechaRec,
        listaEquivalenciaNodoEnergetico);
                */
                proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;
                decimal horasCentral = horasUnidad * proporcion;

                if (horasUnidad > 0)
                {
                    CreaRegistroProceso(nrperCodi,
            unidadEquivalente.GrupocodiModo, nrcptcodiProgramado,
            (DateTime)itemRedProg.Ichorini, (DateTime)itemRedProg.Ichorfin,
            horasUnidad,
            horasCentral, 0, potenciaRestringidaA,
            0, potenciaEfectivaModo,
            0, ratioPotCalcPotAdjudicada, 0, 1,
            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
            0, 1, usuario,
            DateTime.Now, null, null);
                    registroCreado = true;
                }

            }


            return registroCreado;
        }


        private bool ObtenerHorasIndispParcProgramCaso2Antiguo(DateTime fechaRec,
    ModoGrupoCentralEquipo unidadEquivalente, decimal ratioPotCalcPotAdjudicada,
    List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrperCodi,
    int nrcptcodiProgramado, int nrcptcodiFortuito, string notaAutomatica, string usuario,
    decimal potenciaAdjudicadaCentral, List<SiParametroValorDTO> listaParametroValorRpf, Hashtable listaFechaModoValor)
        {

            ManttoResultado mantoRes = new ManttoResultado();


            bool registroCreado = false;

            decimal potenciaEfectivaModo =
                    FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                        fechaRec);

            int evenclasecodi;

            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasProgramado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();



            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasEjecutado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();

            //potencia consigna
            List<NrPotenciaconsignaDTO> listaPotenciaConsigna = BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico,
                        unidadEquivalente.GrupocodiModo, fechaRec, fechaRec,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1)
                        .Where(x => (int)x.Grupocodi == unidadEquivalente.GrupocodiModo).ToList();
            //---
            if (listaPotenciaConsigna.Count > 0)
            {
                //cargando los programados
                foreach (EveIeodcuadroDTO ip in listaOperacVariasProgramado)
                {
                    decimal valor = ip.Icvalor2 == null ? 0 : (decimal)ip.Icvalor2;
                    mantoRes.AsignaMantoProgramado(ip, 1, valor);
                }

                //cargando el restricciones operativas base
                /*
                foreach (EveIeodcuadroDTO ie in listaOperacVariasEjecutado)
                {
                    decimal valor = ie.Icvalor2 == null ? 0 : (decimal)ie.Icvalor2;
                    mantoRes.AsignaRestrixOperaBase(ie, valor);
                }
                */



                //detalle del punto consigna
                foreach (var itemPotConsg in listaPotenciaConsigna)
                {

                    List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                        BuscarOperaciones(itemPotConsg.Nrpccodi, fechaRec, fechaRec, -1,
                                -1);

                    listaPotConsgDet.Sort(
                        (p, q) =>
                            DateTime.Compare((DateTime)p.Nrpcdfecha,
                                (DateTime)q.Nrpcdfecha));

                    for (int i = 0; i < listaPotConsgDet.Count; i++)
                    {
                        DateTime horaActual = (DateTime)listaPotConsgDet[i].Nrpcdfecha;
                        DateTime horaSiguiente = (i + 1 < listaPotConsgDet.Count) ?
                            (DateTime)listaPotConsgDet[i + 1].Nrpcdfecha : horaActual;


                        //no en máxima demanda e igual a la potencia límite
                        if (listaPotConsgDet[i].Nrpcdmaximageneracion != null &&
                            listaPotConsgDet[i].Nrpcdmaximageneracion.ToUpper() == "S")
                        {
                            int h0, h1;

                            ObtenerHiMedidores(horaActual, horaSiguiente, out  h0, out  h1,
                                ConstantesReservaFriaNodoEnergetico.Factor15);



                            //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                            List<MeMedicion96DTO> listaMedidores =
                                FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                    unidadEquivalente.PtomedicodiMedidor,
                                    ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
                                    fechaRec, fechaRec).ToList();

                            //potencia promedio de medidores
                            decimal potPromMedidor = 0;
                            int cuenta = 0;

                            foreach (MeMedicion96DTO itemMed in listaMedidores)
                            {
                                for (int hi = h0; hi <= h1; hi++)
                                {
                                    potPromMedidor +=
                                        (decimal)
                                        itemMed.GetType()
                                            .GetProperty("H" + hi.ToString())
                                            .GetValue(itemMed, null);
                                    cuenta++;
                                }
                            }

                            if (cuenta != 0)
                            {
                                potPromMedidor /= cuenta;


                                decimal rpf = ObtenerPametroValor(
                                    listaParametroValorRpf, fechaRec);

                                //B
                                decimal potencia1 = potPromMedidor * (1 + rpf);

                                //cargando potencia consigna y promedio
                                EveIeodcuadroDTO ejecPc = new EveIeodcuadroDTO();
                                ejecPc.Ichorini = horaActual;
                                ejecPc.Ichorfin = horaSiguiente;

                                decimal mw = listaPotConsgDet[i].Nrpcdmw == null ? 0 : (decimal)listaPotConsgDet[i].Nrpcdmw;

                                mantoRes.AsignaConsignaOperaBase(ejecPc, mw, potencia1);


                            }
                        }
                    }
                }

                //obtiene el nuevo ejecutado considerando restriccion operativa y consigna
                decimal[] ejecutado = mantoRes.ObtenerMatrizDesdeRestriccConsigna();

                mantoRes.AsignaMantoEjecutadoProgramado(ejecutado, 2, 3);

                List<EveIeodcuadroDTO> TramoReducidoProgramado = mantoRes.ListaReducidaIndispTotalProgramada(fechaRec);
                List<EveIeodcuadroDTO> TramoReducidoFortuito = mantoRes.ListaReducidaIndispTotalFortuita(fechaRec);


                decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);

                // HIPP - Caso 1
                #region HIPP - Caso 1




                //***********
                //  HIPP
                //***********
                foreach (EveIeodcuadroDTO itemRedProg in TramoReducidoProgramado)
                {
                    decimal valorA = itemRedProg.Icvalor1 == null ? 0 : (decimal)itemRedProg.Icvalor1;
                    decimal valorB = itemRedProg.Icvalor2 == null ? 0 : (decimal)itemRedProg.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado = registroCreado || ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedProg, potenciaAdjudicadaCentral);

                    }

                }

                //***********
                // HIPF
                //***********
                foreach (EveIeodcuadroDTO itemRedFort in TramoReducidoFortuito)
                {
                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado = registroCreado || ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedFort, potenciaAdjudicadaCentral);

                    }

                }





                #endregion

            }

            //---






            return registroCreado;

        }


        /// <summary>
        /// Permite obtener horas de indisponibilidad Parcial Programada (Caso 2)
        /// </summary>
        /// <param name="fechaRec">Fecha de consulta</param>
        /// <param name="unidadEquivalente">Unidad de equivalencia</param>
        /// <param name="ratioPotCalcPotAdjudicada">Ratio de Potencia Calculada y Adjudicada</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de unidad equivalente</param>
        /// <param name="nrperCodi">Codigo de periodo</param>
        /// <param name="nrcptcodiProgramado">Codigo de concepto</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        private bool ObtenerHorasIndispParcProgramCaso2(DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, decimal ratioPotCalcPotAdjudicada,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrperCodi,
            int nrcptcodiProgramado, int nrcptcodiFortuito, string notaAutomatica, string usuario,
            decimal potenciaAdjudicadaCentral, List<SiParametroValorDTO> listaParametroValorRpf, Hashtable listaFechaModoValor)
        {



            bool registroCreado = false;
            bool registroCreado1 = false;
            bool registroCreado2 = false;

            decimal potenciaEfectivaModo =
                    FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                        fechaRec);

            int evenclasecodi;



            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasProgramado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();


            /*
            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasEjecutado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();
            */

            //potencia consigna
            List<NrPotenciaconsignaDTO> listaPotenciaConsigna = BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico,
                        unidadEquivalente.GrupocodiModo, fechaRec, fechaRec,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1)
                        .Where(x => (int)x.Grupocodi == unidadEquivalente.GrupocodiModo).ToList();
            //---
            if ((listaPotenciaConsigna.Count > 0) && (listaOperacVariasProgramado.Count > 0))
            {


                //cargando el restricciones operativas base
                /*
                foreach (EveIeodcuadroDTO ie in listaOperacVariasEjecutado)
                {
                    decimal valor = ie.Icvalor2 == null ? 0 : (decimal)ie.Icvalor2;
                    mantoRes.AsignaRestrixOperaBase(ie, valor);
                }
                */



                //detalle del punto consigna
                foreach (var itemPotConsg in listaPotenciaConsigna)
                {

                    List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                        BuscarOperaciones(itemPotConsg.Nrpccodi, fechaRec, fechaRec, -1,
                                -1);

                    listaPotConsgDet.Sort(
                        (p, q) =>
                            DateTime.Compare((DateTime)p.Nrpcdfecha,
                                (DateTime)q.Nrpcdfecha));

                    for (int i = 0; i < listaPotConsgDet.Count; i++)
                    {
                        ManttoResultado mantoRes = new ManttoResultado();
                        //cargando los programados
                        foreach (EveIeodcuadroDTO ip in listaOperacVariasProgramado)
                        {
                            decimal valor = ip.Icvalor2 == null ? 0 : (decimal)ip.Icvalor2;
                            mantoRes.AsignaMantoProgramado(ip, 1, valor);
                        }


                        DateTime horaActual = (DateTime)listaPotConsgDet[i].Nrpcdfecha;
                        DateTime horaSiguiente = (i + 1 < listaPotConsgDet.Count) ?
                            (DateTime)listaPotConsgDet[i + 1].Nrpcdfecha : horaActual;


                        //no en máxima demanda e igual a la potencia límite
                        if (listaPotConsgDet[i].Nrpcdmaximageneracion != null &&
                            listaPotConsgDet[i].Nrpcdmaximageneracion.ToUpper() == "S")
                        {
                            int h0, h1;

                            ObtenerHiMedidores(horaActual, horaSiguiente, out  h0, out  h1,
                                ConstantesReservaFriaNodoEnergetico.Factor15);



                            //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                            List<MeMedicion96DTO> listaMedidores =
                                FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                    unidadEquivalente.PtomedicodiMedidor,
                                    ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
                                    fechaRec, fechaRec).ToList();

                            //potencia promedio de medidores
                            decimal potPromMedidor = 0;
                            int cuenta = 0;

                            foreach (MeMedicion96DTO itemMed in listaMedidores)
                            {
                                for (int hi = h0; hi <= h1; hi++)
                                {
                                    potPromMedidor +=
                                        (decimal)
                                        itemMed.GetType()
                                            .GetProperty("H" + hi.ToString())
                                            .GetValue(itemMed, null);
                                    cuenta++;
                                }
                            }

                            if (cuenta != 0)
                            {
                                potPromMedidor /= cuenta;


                                decimal rpf = ObtenerPametroValor(
                                    listaParametroValorRpf, fechaRec);

                                //B
                                decimal potencia1 = potPromMedidor * (1 + rpf);

                                //cargando potencia consigna y promedio
                                EveIeodcuadroDTO ejecPc = new EveIeodcuadroDTO();
                                ejecPc.Ichorini = horaActual;
                                ejecPc.Ichorfin = horaSiguiente;

                                decimal mw = listaPotConsgDet[i].Nrpcdmw == null ? 0 : (decimal)listaPotConsgDet[i].Nrpcdmw;


                                //mantoRes.AsignaConsignaOperaBase(ejecPc, mw, potencia1);
                                //mantoRes.AsignaMantoEjecutadoProgramado(ejecPc, 2, 3, mw);
                                mantoRes.AsignaMantoEjecutadoProgramado(ejecPc, 2, 3, potencia1);



                                List<EveIeodcuadroDTO> TramoReducidoProgramado = mantoRes.ListaReducidaIndispTotalProgramada(fechaRec);
                                List<EveIeodcuadroDTO> TramoReducidoFortuito = mantoRes.ListaReducidaIndispTotalFortuita(fechaRec);

                                decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);

                                //

                                //***********
                                //  HIPP
                                //***********
                                foreach (EveIeodcuadroDTO itemRedProg in TramoReducidoProgramado)
                                {
                                    decimal valorA = itemRedProg.Icvalor1 == null ? 0 : (decimal)itemRedProg.Icvalor1;
                                    decimal valorB = itemRedProg.Icvalor2 == null ? 0 : (decimal)itemRedProg.Icvalor2;

                                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {

                                        registroCreado1 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                                              listaEquivalenciaNodoEnergetico, nrperCodi,
                                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedProg, potenciaAdjudicadaCentral);

                                    }

                                }

                                //***********
                                // HIPF
                                //***********
                                foreach (EveIeodcuadroDTO itemRedFort in TramoReducidoFortuito)
                                {
                                    /*
                                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {

                                        registroCreado2 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                                              listaEquivalenciaNodoEnergetico, nrperCodi,
                                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedFort, potenciaAdjudicadaCentral);

                                    }
                                    */

                                    //---
                                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {

                                        decimal potenciaRestringidaA = potenciaAdjudicadaUnidad - valorB;

                                        decimal horasUnidad = ((decimal)((DateTime)itemRedFort.Ichorfin - (DateTime)itemRedFort.Ichorini).TotalHours) * potenciaRestringidaA / potenciaAdjudicadaUnidad;

                                        decimal proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;
                                        decimal horasCentral = horasUnidad * proporcion;

                                        if (horasUnidad > 0)
                                        {
                                            CreaRegistroProceso(nrperCodi,
                                    unidadEquivalente.GrupocodiModo, nrcptcodiFortuito,
                                    (DateTime)itemRedFort.Ichorini, (DateTime)itemRedFort.Ichorfin,
                                    horasUnidad,
                                    horasCentral, 0, potenciaRestringidaA,
                                    0, potenciaEfectivaModo,
                                    0, ratioPotCalcPotAdjudicada, 0, 1,
                                    0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                                    0, 1, usuario,
                                    DateTime.Now, null, null);
                                            registroCreado = true;
                                        }
                                    }

                                    //---


                                }

                                //


                            }
                        }
                    }
                }

                //obtiene el nuevo ejecutado considerando restriccion operativa y consigna
                //decimal[] ejecutado = mantoRes.ObtenerMatrizDesdeRestriccConsigna();
                /*
                //mantoRes.AsignaMantoEjecutadoProgramado(ejecutado, 2, 3);
                
                List<EveIeodcuadroDTO> TramoReducidoProgramado = mantoRes.ListaReducidaIndispTotalProgramada(fechaRec);
                List<EveIeodcuadroDTO> TramoReducidoFortuito = mantoRes.ListaReducidaIndispTotalFortuita(fechaRec);

                
                decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);
                */
                // HIPP - Caso 1
                #region HIPP - Caso 1




                //***********
                //  HIPP
                //***********
                /*
                foreach (EveIeodcuadroDTO itemRedProg in TramoReducidoProgramado)
                {
                    decimal valorA = itemRedProg.Icvalor1 == null ? 0 : (decimal)itemRedProg.Icvalor1;
                    decimal valorB = itemRedProg.Icvalor2 == null ? 0 : (decimal)itemRedProg.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado1 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedProg,potenciaAdjudicadaCentral);

                    }

                }
                */

                //***********
                // HIPF
                //***********
                /*
                foreach (EveIeodcuadroDTO itemRedFort in TramoReducidoFortuito)
                {
                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado2 =ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedFort,potenciaAdjudicadaCentral);

                    }

                }
                */





                #endregion

            }

            //---



            registroCreado = registroCreado1 || registroCreado2;


            return registroCreado;

        }

        /// <summary>
        /// Permite obtener horas de indisponibilidad Parcial Fortuita (Caso 1). [HIPF - Caso 1]
        /// </summary>
        /// <param name="programada">Verdadero, si se desea calcular las horas de indisponibilidad programada</param>
        /// <param name="fechaRec">Fecha de consulta</param>
        /// <param name="unidadEquivalente">Unidad de equivalencia</param>
        /// <param name="ratioPotCalcPotAdjudicada">Ratio de Potencia Calculada y Adjudicada</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de unidad equivalente</param>
        /// <param name="nrperCodi">Codigo de periodo</param>
        /// <param name="nrcptcodi">Codigo de concepto</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        private bool ObtenerHorasIndispParcFortuitaCaso1(DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, decimal ratioPotCalcPotAdjudicada,
            List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico, int nrperCodi, int nrcptcodi,
            string notaAutomatica, string usuario, decimal potenciaAdjudicada, Hashtable listaFechaModoValor, decimal potenciaAdjudicadaCentral)
        {
            bool registroCreado = false;

            int evenclasecodi;


            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;


            // HIPP - Caso 1
            #region HIPP - Caso 1

            //En este caso, no debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVarias = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();

            if (listaOperacVarias.Count == 0)
            {
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;

                listaOperacVarias = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();

                if (listaOperacVarias.Count > 0)
                {

                    //Se obtiene la Potencia límite desde el registro de Restricciones Operativas en horizonte Ejecutado cuyo valor es (A).
                    decimal potenciaLimite = (decimal)listaOperacVarias[0].Icvalor2;

                    //Potencia Restringida=P.Efectiva Unidad-(A)
                    decimal potenciaEfectivaModo =
                        FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                            ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                            fechaRec);

                    //decimal potenciaRestringida = potenciaEfectivaModo - potenciaLimite;
                    decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - potenciaLimite;


                    //Si la Potencia Restringida > 10% P. Efectiva, se calcula:
                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {
                        //X_i=((Hora final consigna(j+1)– Hora inicial consigna(j))*Potencia Restringida)/(P.Efectiva)
                        decimal horasCasoHipp = (decimal)((TimeSpan)
                        ((DateTime)listaOperacVarias[0].Ichorfin -
                         (DateTime)listaOperacVarias[0].Ichorini)).TotalHours;

                        //decimal horasUnidad = horasCasoHipp * potenciaRestringida / potenciaEfectivaModo;
                        decimal horasUnidad = horasCasoHipp * potenciaRestringida / potenciaAdjudicadaUnidad;

                        //Se hallan las Horas por Central:
                        //Horas=(Padjudicada(i))/((Suma Padjudicada(i)) X_i
                        decimal proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;// ObtenerPropocionPropiedadModoCentral(unidadEquivalente.GrupocodiModo,
                        //ConstantesReservaFriaNodoEnergetico.PotenciaAdjudicada, fechaRec,
                        //listaEquivalenciaNodoEnergetico);

                        decimal horasCentral = horasUnidad * proporcion;

                        //crea registro
                        /*
                        CreaRegistroProceso(nrperCodi,
                            unidadEquivalente.GrupoCodiGrupo, nrcptcodi,
                            (DateTime)listaOperacVarias[0].Ichorini, (DateTime)listaOperacVarias[0].Ichorfin,
                            horasUnidad,
                            horasCentral, 0, potenciaRestringida,
                            0, potenciaEfectivaModo,
                            0, ratioPotCalcPotAdjudicada, 0, 1,
                            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                            0, 1, usuario,
                            DateTime.Now, null, null
                        );
                        */
                        CreaRegistroProceso(nrperCodi,
                            unidadEquivalente.GrupocodiModo, nrcptcodi,
                            (DateTime)listaOperacVarias[0].Ichorini, (DateTime)listaOperacVarias[0].Ichorfin,
                            horasUnidad,
                            horasCentral, 0, potenciaRestringida,
                            0, potenciaEfectivaModo,
                            0, ratioPotCalcPotAdjudicada, 0, 1,
                            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                            0, 1, usuario,
                            DateTime.Now, null, null
                        );

                        registroCreado = true;

                    }
                }
            }

            #endregion

            return registroCreado;
        }

        /*
        /// <summary>
        /// Permite obtener horas de indisponibilidad Parcial Programada o Fortuita (Caso 2)
        /// </summary>
        /// <param name="programada">Verdadero, si se desea calcular las horas de indisponibilidad programada</param>
        /// <param name="fechaRec">Fecha de consulta</param>
        /// <param name="unidadEquivalente">Unidad de equivalencia</param>
        /// <param name="listaParametroValorRpf">Lista de valores RPF</param>
        /// <param name="ratioPotCalcPotAdjudicada">Ratio de Potencia Calculada y Adjudicada</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de unidad equivalente</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        private bool ObtenerHorasIndispParcProgramFortuitaCaso2(bool programada, DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, List<SiParametroValorDTO> listaParametroValorRpf,
            decimal ratioPotCalcPotAdjudicada, List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico,
            int nrperCodi, int nrcptcodi, string notaAutomatica, string usuario)
        {
            bool registroCreado = false;

            int evenclasecodi;

            if (programada)
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            else
            {
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            }

            #region HIPP - Caso 2


            //con restricciones (de Restricciones operativas y en horizonte Ejecutado)
            List<EveIeodcuadroDTO> listaOperacVarias = FactorySic.GetEveIeodcuadroRepository().BuscarOperaciones(
                evenclasecodi, ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat,
                    fechaRec, fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();

            if (listaOperacVarias.Count > 0)
            {
                //pasos previos: detectando las órdenes de máxima demanda


                List<NrPotenciaconsignaDTO> listaPotenciaConsigna = BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico,
                        unidadEquivalente.GrupocodiModo, fechaRec, fechaRec,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1);

                if (listaPotenciaConsigna.Count > 0)
                {
                    //detalle del punto consigna
                    foreach (var itemPotConsg in listaPotenciaConsigna)
                    {

                        List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                            BuscarOperaciones(0, fechaRec, fechaRec, -1,
                                    -1)
                                .Where(x => x.Nrpccodi == itemPotConsg.Nrpccodi)
                                .ToList();
                        listaPotConsgDet.Sort(
                            (p, q) =>
                                DateTime.Compare((DateTime)p.Nrpcdfecha,
                                    (DateTime)q.Nrpcdfecha));


                        DateTime? horaAnterior = null;

                        foreach (
                            NrPotenciaconsignaDetalleDTO itemPcdet in listaPotConsgDet)
                        {

                            DateTime horaActual = (DateTime)itemPcdet.Nrpcdfecha;

                            if (horaAnterior == null)
                                horaAnterior =
                                    Convert.ToDateTime(
                                        horaActual.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha));
                            //00 horas

                            if (itemPcdet.Nrpcdmaximageneracion.ToUpper() == "S")
                            {

                                //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                                int h0 = horaAnterior.Value.Hour * 4 +
                                         (horaAnterior.Value.Minute / 15);
                                int h1 = horaActual.Hour * 4 +
                                         (horaActual.Minute / 15);

                                //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                                List<MeMedicion96DTO> listaMedidores =
                                    FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                        unidadEquivalente.PtomedicodiMedidor,
                                        ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
                                        horaActual, horaActual).ToList();

                                //potencia promedio de medidores
                                decimal potPromMedidor = 0;
                                int cuenta = 0;

                                foreach (MeMedicion96DTO itemMed in listaMedidores)
                                {
                                    for (int hi = h0; hi <= h1; hi++)
                                    {
                                        potPromMedidor +=
                                            (decimal)
                                            listaMedidores[0].GetType()
                                                .GetProperty("H" + hi.ToString())
                                                .GetValue(listaMedidores[0], null);
                                        cuenta++;
                                    }
                                }

                                if (cuenta != 0)
                                {
                                    potPromMedidor /= cuenta;


                                    decimal rpf = ObtenerPametroValor(
                                        listaParametroValorRpf, fechaRec);

                                    decimal potencia1 = potPromMedidor * (1 + rpf);

                                    //potencia efectiva unidad
                                    decimal potenciaEfectivaModo =
                                        FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(
                                            unidadEquivalente.GrupocodiModo,
                                            ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva, fechaRec);

                                    //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF) cuyo valor es B

                                    //Potencia Restringida=P.Efectiva Unidad-(B)
                                    decimal potRestring = potenciaEfectivaModo -
                                                          potencia1;

                                    //Si la Potencia Restringida > 10% P. Efectiva, se calcula
                                    if (potRestring >
                                        ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {
                                        //X_i=((Hora final consigna(j+1)– Hora inicial consigna(j))*Potencia Restringida)/(P.Efectiva)
                                        decimal horaUnidad =
                                            ((decimal)(((DateTime)horaActual -
                                                         (DateTime)horaAnterior)
                                                .TotalHours)) *
                                            potRestring / potenciaEfectivaModo;

                                        //Se hallan las Horas por Central:
                                        //Horas=(Padjudicada(i))/(Suma Padjudicada(i)) X_i
                                        decimal proporcion = ObtenerPropocionPropiedadModoCentral
                                        (
                                            unidadEquivalente.GrupocodiModo,
                                            ConstantesReservaFriaNodoEnergetico
                                                .PotenciaAdjudicada,
                                            fechaRec,
                                            listaEquivalenciaNodoEnergetico);


                                        decimal horaCentral = horaUnidad * proporcion;

                                        //crea registro
                                        
                                        //CreaRegistroProceso(nrperCodi,
                                        //    unidadEquivalente.GrupoCodiGrupo, nrcptcodi,
                                        //    (DateTime)horaAnterior, horaActual,
                                        //    horaUnidad,
                                        //    horaCentral, potencia1, potRestring,
                                        //    0, potenciaEfectivaModo,
                                        //    potencia1, ratioPotCalcPotAdjudicada, 0, 1,
                                        //    0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                                        //    rpf, 1, usuario,
                                        //    DateTime.Now, null, null
                                        );

                                        CreaRegistroProceso(nrperCodi,
                                            unidadEquivalente.GrupocodiModo, nrcptcodi,
                                            (DateTime)horaAnterior, horaActual,
                                            horaUnidad,
                                            horaCentral, potencia1, potRestring,
                                            0, potenciaEfectivaModo,
                                            potencia1, ratioPotCalcPotAdjudicada, 0, 1,
                                            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                                            rpf, 1, usuario,
                                            DateTime.Now, null, null);

                                        registroCreado = true;
                                    }
                                }

                                horaAnterior = horaActual;
                            }
                        }
                    }
                }
            }

            #endregion

            return registroCreado;
        }
        */

        /// <summary>
        /// Permite obtener la columna h de medidores
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="h0"></param>
        /// <param name="?"></param>
        private void ObtenerHiMedidores(DateTime fechaInicial, DateTime fechaFinal, out int h0, out int h1, int factor)
        {
            int bloqueHora = 0;
            int minutos = 0;
            int bloqueMaximo = 0;

            if (factor == ConstantesReservaFriaNodoEnergetico.Factor15)
            {
                //granularidad de 15 minutos
                bloqueHora = 4;
                minutos = 15;
                bloqueMaximo = 96;
            }
            else
            {
                //granularidad de 30 minutos
                bloqueHora = 2;
                minutos = 30;
                bloqueMaximo = 48;
            }

            h0 = 0;
            h1 = 0;

            h0 = bloqueHora * fechaInicial.Hour + fechaInicial.Minute / minutos;

            h0 += 1;
            if (fechaInicial.Day != fechaFinal.Day)
                h1 = bloqueMaximo;
            else
                h1 = bloqueHora * fechaFinal.Hour + fechaFinal.Minute / minutos;

            if (h1 < h0)
                h1 = h0;

        }



        /// <summary>
        /// Permite obtener horas de indisponibilidad Parcial Fortuita (Caso 2). [HIPF - Caso 2]
        /// </summary>
        /// <param name="programada">Verdadero, si se desea calcular las horas de indisponibilidad programada</param>
        /// <param name="fechaRec">Fecha de consulta</param>
        /// <param name="unidadEquivalente">Unidad de equivalencia</param>
        /// <param name="listaParametroValorRpf">Lista de valores RPF</param>
        /// <param name="ratioPotCalcPotAdjudicada">Ratio de Potencia Calculada y Adjudicada</param>
        /// <param name="listaEquivalenciaNodoEnergetico">Lista de unidad equivalente</param>
        /// <param name="nrperCodi">Código de periodo</param>
        /// <param name="nrcptcodi">Código de concepto</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="usuario">Usuario del sistema</param>
        /// <returns></returns>
        private bool ObtenerHorasIndispParcFortuitaCaso2Antiguo(DateTime fechaRec,
            ModoGrupoCentralEquipo unidadEquivalente, List<SiParametroValorDTO> listaParametroValorRpf,
            decimal ratioPotCalcPotAdjudicada, List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico,
            int nrperCodi, int nrcptcodi, string notaAutomatica, string usuario, decimal potenciaAdjudicada)
        {
            bool registroCreado = false;
            /*
            int evenclasecodi;

            if (programada)
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            else
            {
                evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            }*/

            #region HIPF - Caso 2


            //con restricciones (de Restricciones operativas y en horizonte Ejecutado)
            /*
            List<EveIeodcuadroDTO> listaOperacVarias = FactorySic.GetEveIeodcuadroRepository().BuscarOperaciones(
                evenclasecodi, ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat,
                    fechaRec, fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();
            */

            //if (listaOperacVarias.Count > 0)
            {
                //pasos previos: detectando las órdenes de máxima demanda


                List<NrPotenciaconsignaDTO> listaPotenciaConsigna = BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico,
                        unidadEquivalente.GrupocodiModo, fechaRec, fechaRec,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1)
                        .Where(x => (int)x.Grupocodi == unidadEquivalente.GrupocodiModo).ToList();

                if (listaPotenciaConsigna.Count > 0)
                {
                    //detalle del punto consigna
                    foreach (var itemPotConsg in listaPotenciaConsigna)
                    {

                        List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                            BuscarOperaciones(itemPotConsg.Nrpccodi, fechaRec, fechaRec, -1,
                                    -1);

                        listaPotConsgDet.Sort(
                            (p, q) =>
                                DateTime.Compare((DateTime)p.Nrpcdfecha,
                                    (DateTime)q.Nrpcdfecha));


                        //DateTime? horaAnterior = null;


                        for (int i = 0; i < listaPotConsgDet.Count; i++)
                        {

                            //foreach (NrPotenciaconsignaDetalleDTO itemPcdet in listaPotConsgDet)
                            //{

                            DateTime horaActual = (DateTime)listaPotConsgDet[i].Nrpcdfecha;
                            DateTime horaSiguiente = (i + 1 < listaPotConsgDet.Count) ?
                                (DateTime)listaPotConsgDet[i + 1].Nrpcdfecha : horaActual;


                            /*if (horaAnterior == null)
                                horaAnterior =
                                    Convert.ToDateTime(
                                        horaActual.ToString(ConstantesReservaFriaNodoEnergetico.FormatoFecha));*/
                            //00 horas

                            if (listaPotConsgDet[i].Nrpcdmaximageneracion != null && listaPotConsgDet[i].Nrpcdmaximageneracion.ToUpper() == "S")
                            {
                                int h0, h1;
                                //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                                /*
                                    int h0 = horaActual.Hour * 4 +
                                             (horaActual.Minute / 15);
                                    h0++;
                                    
                                    if (h0 >= 96)
                                    {
                                        h0 = 96;
                                    }

                                    int h1 = h0;

                                    if (horaActual.Day != horaSiguiente.Day)
                                    {
                                        h1 = 96;
                                    }
                                    else
                                    {
                                        h1 = horaSiguiente.Hour * 4 + (horaSiguiente.Minute / 15);
                                        if (h1 == 0) h1 = 1;
                                    }
                                */

                                ObtenerHiMedidores(horaActual, horaSiguiente, out  h0, out  h1,
                                    ConstantesReservaFriaNodoEnergetico.Factor15);



                                //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                                List<MeMedicion96DTO> listaMedidores =
                                    FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                        ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                        unidadEquivalente.PtomedicodiMedidor,
                                        ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
                                        fechaRec, fechaRec).ToList();

                                //potencia promedio de medidores
                                decimal potPromMedidor = 0;
                                int cuenta = 0;

                                foreach (MeMedicion96DTO itemMed in listaMedidores)
                                {
                                    for (int hi = h0; hi <= h1; hi++)
                                    {
                                        potPromMedidor +=
                                            (decimal)
                                            itemMed.GetType()
                                                .GetProperty("H" + hi.ToString())
                                                .GetValue(itemMed, null);
                                        cuenta++;
                                    }
                                }

                                if (cuenta != 0)
                                {
                                    potPromMedidor /= cuenta;


                                    decimal rpf = ObtenerPametroValor(
                                        listaParametroValorRpf, fechaRec);

                                    //B
                                    decimal potencia1 = potPromMedidor * (1 + rpf);

                                    //potencia efectiva unidad
                                    decimal potenciaEfectivaModo =
                                        FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(
                                            unidadEquivalente.GrupocodiModo,
                                            ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva, fechaRec);

                                    //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF) cuyo valor es B

                                    //Potencia Restringida=P.Efectiva Unidad-(B)
                                    //decimal potRestring = potenciaEfectivaModo - potencia1;
                                    decimal potRestring = potenciaAdjudicada - potencia1;

                                    //Si la Potencia Restringida > 10% P. Efectiva, se calcula
                                    if (potRestring >
                                        ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {
                                        //X_i=((Hora final consigna(j+1)– Hora inicial consigna(j))*Potencia Restringida)/(P.Efectiva)
                                        decimal horaUnidad =
                                            ((decimal)(((DateTime)horaSiguiente -
                                                         (DateTime)horaActual)
                                                .TotalHours)) *
                                            potRestring / potenciaAdjudicada;

                                        //Se hallan las Horas por Central:
                                        //Horas=(Padjudicada(i))/(Suma Padjudicada(i)) X_i
                                        decimal proporcion = ObtenerPropocionPropiedadModoCentral
                                        (
                                            unidadEquivalente.GrupocodiModo,
                                            ConstantesReservaFriaNodoEnergetico
                                                .PotenciaAdjudicada,
                                            fechaRec,
                                            listaEquivalenciaNodoEnergetico);


                                        decimal horaCentral = horaUnidad * proporcion;

                                        //crea registro
                                        /*
                                        CreaRegistroProceso(nrperCodi,
                                            unidadEquivalente.GrupoCodiGrupo, nrcptcodi,
                                            (DateTime)horaAnterior, horaActual,
                                            horaUnidad,
                                            horaCentral, potencia1, potRestring,
                                            0, potenciaEfectivaModo,
                                            potencia1, ratioPotCalcPotAdjudicada, 0, 1,
                                            0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                                            rpf, 1, usuario,
                                            DateTime.Now, null, null
                                        );*/

                                        CreaRegistroProceso(nrperCodi,
                                            unidadEquivalente.GrupocodiModo, nrcptcodi,
                                            (DateTime)horaActual, horaSiguiente,
                                            horaUnidad,
                                            horaCentral, potencia1, potRestring,
                                            potenciaAdjudicada, potenciaEfectivaModo,
                                            potPromMedidor, ratioPotCalcPotAdjudicada, 0, 1,
                                            0, -1, "N", "N", "A", "", 0, "", "", notaAutomatica, "N",
                                            rpf, 0, usuario,
                                            DateTime.Now, null, null);

                                        registroCreado = true;
                                    }
                                }

                                //horaAnterior = horaActual;
                            }
                            //}
                        }


                    }
                }
            }

            #endregion

            return registroCreado;
        }


        private bool ObtenerHorasIndispParcFortuitaCaso2(DateTime fechaRec,
    ModoGrupoCentralEquipo unidadEquivalente, List<SiParametroValorDTO> listaParametroValorRpf,
            decimal ratioPotCalcPotAdjudicada, List<ModoGrupoCentralEquipo> listaEquivalenciaNodoEnergetico,
            int nrperCodi, int nrcptcodi, string notaAutomatica, string usuario, decimal potenciaAdjudicadaCentral, Hashtable listaFechaModoValor)
        {



            bool registroCreado = false;
            //bool registroCreado1 = false;
            //bool registroCreado2 = false;

            decimal potenciaEfectivaModo =
                    FactorySic.GetPrGrupodatRepository().GetValorModoOperacion(unidadEquivalente.GrupocodiModo,
                        ConstantesReservaFriaNodoEnergetico.ConcepcodiPotenciaEfectiva,
                        fechaRec);

            int evenclasecodi;


            /*
            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiProgramadoDiario;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasProgramado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();
            */


            /*
            evenclasecodi = ConstantesReservaFriaNodoEnergetico.EvenclasecodiEjecutado;
            //En este caso, si debe haber registros en Restricciones Operativas en horizonte Programado
            List<EveIeodcuadroDTO> listaOperacVariasEjecutado = FactorySic.GetEveIeodcuadroRepository()
                .BuscarOperaciones(
                    evenclasecodi,
                    ConstantesReservaFriaNodoEnergetico.SubcausacodiRestriccOperat, fechaRec,
                    fechaRec, -1, -1).Where(x => x.Equicodi == unidadEquivalente.Equicodi).ToList();
            */

            //potencia consigna
            List<NrPotenciaconsignaDTO> listaPotenciaConsigna = BuscarOperaciones(
                        ConstantesReservaFriaNodoEnergetico.ModuloNodoEnergetico,
                        unidadEquivalente.GrupocodiModo, fechaRec, fechaRec,
                        ConstantesReservaFriaNodoEnergetico.Vigente, -1, -1)
                        .Where(x => (int)x.Grupocodi == unidadEquivalente.GrupocodiModo).ToList();
            //---
            if (listaPotenciaConsigna.Count > 0)
            {


                //cargando el restricciones operativas base
                /*
                foreach (EveIeodcuadroDTO ie in listaOperacVariasEjecutado)
                {
                    decimal valor = ie.Icvalor2 == null ? 0 : (decimal)ie.Icvalor2;
                    mantoRes.AsignaRestrixOperaBase(ie, valor);
                }
                */



                //detalle del punto consigna
                foreach (var itemPotConsg in listaPotenciaConsigna)
                {

                    List<NrPotenciaconsignaDetalleDTO> listaPotConsgDet =
                        BuscarOperaciones(itemPotConsg.Nrpccodi, fechaRec, fechaRec, -1,
                                -1);

                    listaPotConsgDet.Sort(
                        (p, q) =>
                            DateTime.Compare((DateTime)p.Nrpcdfecha,
                                (DateTime)q.Nrpcdfecha));

                    for (int i = 0; i < listaPotConsgDet.Count; i++)
                    {
                        ManttoResultado mantoRes = new ManttoResultado();
                        //cargando los programados
                        /*
                        foreach (EveIeodcuadroDTO ip in listaOperacVariasProgramado)
                        {
                            decimal valor = ip.Icvalor2 == null ? 0 : (decimal)ip.Icvalor2;
                            mantoRes.AsignaMantoProgramado(ip, 1, valor);
                        }
                        */

                        DateTime horaActual = (DateTime)listaPotConsgDet[i].Nrpcdfecha;
                        DateTime horaSiguiente = (i + 1 < listaPotConsgDet.Count) ?
                            (DateTime)listaPotConsgDet[i + 1].Nrpcdfecha : horaActual;


                        //no en máxima demanda e igual a la potencia límite
                        if (listaPotConsgDet[i].Nrpcdmaximageneracion != null &&
                            listaPotConsgDet[i].Nrpcdmaximageneracion.ToUpper() == "S")
                        {
                            int h0, h1;

                            ObtenerHiMedidores(horaActual, horaSiguiente, out  h0, out  h1,
                                ConstantesReservaFriaNodoEnergetico.Factor15);



                            //Potencia 1=(Potencia promedio de medidores  desde (j)a (j+1))*(1+%RPF)
                            List<MeMedicion96DTO> listaMedidores =
                                FactorySic.GetMeMedicion96Repository().GetByCriteria(
                                    ConstantesReservaFriaNodoEnergetico.TipoinfocodiPotenciaActiva,
                                    unidadEquivalente.PtomedicodiMedidor,
                                    ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores,
                                    fechaRec, fechaRec).ToList();

                            //potencia promedio de medidores
                            decimal potPromMedidor = 0;
                            int cuenta = 0;

                            foreach (MeMedicion96DTO itemMed in listaMedidores)
                            {
                                for (int hi = h0; hi <= h1; hi++)
                                {
                                    potPromMedidor +=
                                        (decimal)
                                        itemMed.GetType()
                                            .GetProperty("H" + hi.ToString())
                                            .GetValue(itemMed, null);
                                    cuenta++;
                                }
                            }

                            if (cuenta != 0)
                            {
                                potPromMedidor /= cuenta;


                                decimal rpf = ObtenerPametroValor(
                                    listaParametroValorRpf, fechaRec);

                                //B
                                decimal potencia1 = potPromMedidor * (1 + rpf);

                                //cargando potencia consigna y promedio
                                EveIeodcuadroDTO ejecPc = new EveIeodcuadroDTO();
                                ejecPc.Ichorini = horaActual;
                                ejecPc.Ichorfin = horaSiguiente;

                                decimal mw = listaPotConsgDet[i].Nrpcdmw == null ? 0 : (decimal)listaPotConsgDet[i].Nrpcdmw;


                                //mantoRes.AsignaConsignaOperaBase(ejecPc, mw, potencia1);
                                //mantoRes.AsignaMantoEjecutadoProgramado(ejecPc, 2, 3, mw);
                                mantoRes.AsignaMantoEjecutadoProgramado(ejecPc, 2, 3, potencia1);



                                List<EveIeodcuadroDTO> TramoReducidoProgramado = mantoRes.ListaReducidaIndispTotalProgramada(fechaRec);
                                List<EveIeodcuadroDTO> TramoReducidoFortuito = mantoRes.ListaReducidaIndispTotalFortuita(fechaRec);

                                decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);

                                //
                                /*
                                //***********
                                //  HIPP
                                //***********
                                foreach (EveIeodcuadroDTO itemRedProg in TramoReducidoProgramado)
                                {
                                    decimal valorA = itemRedProg.Icvalor1 == null ? 0 : (decimal)itemRedProg.Icvalor1;
                                    decimal valorB = itemRedProg.Icvalor2 == null ? 0 : (decimal)itemRedProg.Icvalor2;

                                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {

                                        registroCreado1 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                                              listaEquivalenciaNodoEnergetico, nrperCodi,
                                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedProg, potenciaAdjudicadaCentral);

                                    }

                                }
                                */

                                //***********
                                // HIPF
                                //***********
                                foreach (EveIeodcuadroDTO itemRedFort in TramoReducidoFortuito)
                                {

                                    //---
                                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                                    {

                                        decimal potenciaRestringidaA = potenciaAdjudicadaUnidad - valorB;

                                        decimal horasUnidad = ((decimal)((DateTime)itemRedFort.Ichorfin - (DateTime)itemRedFort.Ichorini).TotalHours) * potenciaRestringidaA / potenciaAdjudicadaUnidad;

                                        decimal proporcion = potenciaAdjudicadaUnidad / potenciaAdjudicadaCentral;
                                        decimal horasCentral = horasUnidad * proporcion;

                                        if (horasUnidad > 0)
                                        {
                                            CreaRegistroProceso(nrperCodi,
                                    unidadEquivalente.GrupocodiModo, nrcptcodi,
                                    (DateTime)itemRedFort.Ichorini, (DateTime)itemRedFort.Ichorfin,
                                    horasUnidad,
                                    horasCentral, 0, potenciaRestringidaA,
                                    0, potenciaEfectivaModo,
                                    0, ratioPotCalcPotAdjudicada, 0, 1,
                                    0, -1, "N", "N", "A", "N", 0, "", "", notaAutomatica, "N",
                                    0, 1, usuario,
                                    DateTime.Now, null, null);
                                            registroCreado = true;
                                        }
                                    }

                                    //---


                                }

                                //


                            }
                        }
                    }
                }

                //obtiene el nuevo ejecutado considerando restriccion operativa y consigna
                //decimal[] ejecutado = mantoRes.ObtenerMatrizDesdeRestriccConsigna();
                /*
                //mantoRes.AsignaMantoEjecutadoProgramado(ejecutado, 2, 3);
                
                List<EveIeodcuadroDTO> TramoReducidoProgramado = mantoRes.ListaReducidaIndispTotalProgramada(fechaRec);
                List<EveIeodcuadroDTO> TramoReducidoFortuito = mantoRes.ListaReducidaIndispTotalFortuita(fechaRec);

                
                decimal potenciaAdjudicadaUnidad = ObtenerPotenciaAdjudicada(ref listaFechaModoValor, unidadEquivalente.GrupocodiModo, fechaRec);
                */
                // HIPP - Caso 1
                #region HIPP - Caso 1




                //***********
                //  HIPP
                //***********
                /*
                foreach (EveIeodcuadroDTO itemRedProg in TramoReducidoProgramado)
                {
                    decimal valorA = itemRedProg.Icvalor1 == null ? 0 : (decimal)itemRedProg.Icvalor1;
                    decimal valorB = itemRedProg.Icvalor2 == null ? 0 : (decimal)itemRedProg.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado1 = ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedProg,potenciaAdjudicadaCentral);

                    }

                }
                */

                //***********
                // HIPF
                //***********
                /*
                foreach (EveIeodcuadroDTO itemRedFort in TramoReducidoFortuito)
                {
                    decimal valorA = itemRedFort.Icvalor1 == null ? 0 : (decimal)itemRedFort.Icvalor1;
                    decimal valorB = itemRedFort.Icvalor2 == null ? 0 : (decimal)itemRedFort.Icvalor2;

                    decimal potenciaRestringida = potenciaAdjudicadaUnidad - valorB;

                    if (potenciaRestringida > ratioPotCalcPotAdjudicada * potenciaEfectivaModo)
                    {

                        registroCreado2 =ObtenerHorasIndispParcProgramCaso1Auxiliar(valorA, valorB, fechaRec,
                              unidadEquivalente, ratioPotCalcPotAdjudicada,
                              listaEquivalenciaNodoEnergetico, nrperCodi,
                              nrcptcodiProgramado, nrcptcodiFortuito, notaAutomatica, usuario,
                              potenciaAdjudicadaUnidad, potenciaRestringida, potenciaEfectivaModo, itemRedFort,potenciaAdjudicadaCentral);

                    }

                }
                */





                #endregion

            }

            //---



            //registroCreado = registroCreado1 || registroCreado2;


            return registroCreado;

        }


        /// <summary>
        /// Permite obtener el valor de un parámetro de acuerdo a una lista
        /// </summary>
        /// <param name="listaParametroValor">Lista que contiene los parámetros y valores</param>
        /// <param name="fecha">Fecha</param>
        /// <returns></returns>
        private decimal ObtenerPametroValor(List<SiParametroValorDTO> listaParametroValor, DateTime fecha)
        {
            //obtener rpf
            SiParametroValorDTO registroParametroValorRegistro =
                listaParametroValor.Where(
                    x =>
                        ((DateTime)x.Siparvfechainicial) <= fecha &&
                        fecha <= ((DateTime)x.Siparvfechafinal)).FirstOrDefault();

            decimal rpf = (registroParametroValorRegistro == null ? 0 : (decimal)registroParametroValorRegistro.Siparvvalor);

            return rpf;
        }


        public decimal ObtenerPametroValor(int parametro, DateTime fecha)
        {

            //listado de parámetro RPF
            List<SiParametroValorDTO> listaParametroValor =
                BuscarOperaciones(parametro,
                    fecha, fecha, -1, -1, "N");

            //obtener rpf
            SiParametroValorDTO registroParametroValorRegistro =
                listaParametroValor.Where(
                    x =>
                        ((DateTime)x.Siparvfechainicial) <= fecha &&
                        fecha <= ((DateTime)x.Siparvfechafinal)).FirstOrDefault();

            decimal rpf = (registroParametroValorRegistro == null ? 0 : (decimal)registroParametroValorRegistro.Siparvvalor);

            return rpf;
        }



        /// <summary>
        /// Permite crear un DTO del proceso y almacenarlo en la base de datos
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="grupocodi">Código de grupo</param>
        /// <param name="concepto">Concepto</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="horaUnidad">Horas de unidad</param>
        /// <param name="horaCentral">Horas de central</param>
        /// <param name="potenciaLimite">Potencia Límite</param>
        /// <param name="potenciaRestringida">Potencia Restringida</param>
        /// <param name="potenciaAdjudicada">Potencia Adjudicada</param>
        /// <param name="potenciaEfectiva">Potencia Efectiva</param>
        /// <param name="potenciaPromMedidor">Potencia promedio de medidor</param>
        /// <param name="porcentajeRestringidaEfectiva">Porcentaje de Potencia Restringida y efectiva</param>
        /// <param name="volumenCombustible">Volumen de combustible</param>
        /// <param name="rendimientoUnidad">Rendimiento de la unidad</param>
        /// <param name="energiaDejadaEntregar">Energía dejada de entregar</param>
        /// <param name="codigoPadre">Código de registro padre</param>
        /// <param name="exceptuaCoes">Exceptúa COES</param>
        /// <param name="exceptuaOsinergmin">Exceptúa OSINERGMIN</param>
        /// <param name="tipoIngreso">Tipo de ingreso</param>
        /// <param name="horaFalla">Hora de falla</param>
        /// <param name="sobrecosto">Sobrecosto</param>
        /// <param name="observacion">Observación</param>
        /// <param name="nota">Nota</param>
        /// <param name="notaAutomatica">Nota automática</param>
        /// <param name="filtrado">Registro Filtrado</param>
        /// <param name="rpf">Porcentaje de Reserva Primaria de Frecuencia</param>
        /// <param name="tolerancia">Valor de Tolerancia</param>
        /// <param name="usuarioCreacion">Usuario que crea el registro</param>
        /// <param name="fechaCreacion">Fecha de creación de registro</param>
        /// <param name="usuarioModificacion">Usuario que modifica el registro</param>
        /// <param name="fechaModificacion">Fecha de modificación de registro</param>
        private void CreaRegistroProceso(int? periodo, int grupocodi, int concepto, DateTime fechaInicio,
            DateTime fechaFin, decimal? horaUnidad, decimal? horaCentral, decimal? potenciaLimite,
            decimal? potenciaRestringida, decimal? potenciaAdjudicada, decimal? potenciaEfectiva,
            decimal? potenciaPromMedidor,
            decimal? porcentajeRestringidaEfectiva, decimal? volumenCombustible, decimal? rendimientoUnidad,
            decimal? energiaDejadaEntregar, decimal? codigoPadre,
            string exceptuaCoes, string exceptuaOsinergmin, string tipoIngreso, string horaFalla, decimal? sobrecosto,
            string observacion,
            string nota, string notaAutomatica, string filtrado, decimal? rpf, decimal? tolerancia,
            string usuarioCreacion,
            DateTime? fechaCreacion, string usuarioModificacion, DateTime? fechaModificacion)
        {

            NrProcesoDTO regProceso = new NrProcesoDTO();

            //regProceso.Nrprccodi = (codigo == null ? null : (int) codigo);
            regProceso.Nrpercodi = (int)periodo;
            regProceso.Grupocodi = grupocodi;
            regProceso.Nrcptcodi = concepto;
            regProceso.Nrprcfechainicio = fechaInicio;
            regProceso.Nrprcfechafin = fechaFin;
            regProceso.Nrprchoraunidad = (decimal)horaUnidad;
            regProceso.Nrprchoracentral = (decimal)horaCentral;
            regProceso.Nrprcpotencialimite = (decimal)potenciaLimite;
            regProceso.Nrprcpotenciarestringida = (decimal)potenciaRestringida;

            regProceso.Nrprcpotenciaadjudicada = (decimal)potenciaAdjudicada;
            regProceso.Nrprcpotenciaefectiva = (decimal)potenciaEfectiva;
            regProceso.Nrprcpotenciaprommedidor = (decimal)potenciaPromMedidor;
            regProceso.Nrprcprctjrestringefect = (decimal)porcentajeRestringidaEfectiva;
            regProceso.Nrprcvolumencombustible = 0;// (decimal)volumenCombustible * ConstantesReservaFriaNodoEnergetico.RatioGalonaM3;
            regProceso.Nrprcrendimientounidad = (decimal)rendimientoUnidad;
            regProceso.Nrprcede = (decimal)energiaDejadaEntregar;
            regProceso.Nrprcpadre = (int)codigoPadre;
            regProceso.Nrprcexceptuacoes = exceptuaCoes;
            regProceso.Nrprcexceptuaosinergmin = exceptuaOsinergmin;

            regProceso.Nrprctipoingreso = tipoIngreso;
            regProceso.Nrprchorafalla = horaFalla;
            regProceso.Nrprcsobrecosto = (decimal)sobrecosto;
            regProceso.Nrprcobservacion = observacion;
            regProceso.Nrprcnota = nota;
            regProceso.Nrprcnotaautomatica = notaAutomatica;
            regProceso.Nrprcfiltrado = filtrado;
            regProceso.Nrprcrpf = (decimal)rpf;
            regProceso.Nrprctolerancia = (decimal)tolerancia;
            regProceso.Nrprcusucreacion = usuarioCreacion;

            regProceso.Nrprcfeccreacion = fechaCreacion;

            regProceso.Nrprcusumodificacion = usuarioModificacion;
            if (fechaModificacion != null)
            {
                regProceso.Nrprcfecmodificacion = fechaModificacion;
            }

            SaveNrProceso(regProceso);

        }


        /// <summary>
        /// Permite obtener un listado de equivalencias de Modo, Grupo, Central, Equipo y Punto Medición
        /// </summary>
        /// <param name="listaGrupoFiltrado">Lista de grupos filtrado</param>
        /// <param name="listaPrGrupo">Lista de PrGrupo</param>
        /// <param name="listaPtoMedicion">Lista de Punto de medición</param>
        /// <returns></returns>
        public List<ModoGrupoCentralEquipo> ObtenerEquivalenciaModoGrupoCentralEquipo(
               List<PrGrupoDTO> listaGrupoFiltrado, List<PrGrupoDTO> listaPrGrupo, List<MePtomedicionDTO> listaPtoMedicion)
        {
            List<ModoGrupoCentralEquipo> listaEquivalenciaModoGrupoCentralEquipo = new List<ModoGrupoCentralEquipo>();

            foreach (PrGrupoDTO item in listaGrupoFiltrado)
            {
                int grupocodiModo = item.Grupocodi;
                int grupoCodiGrupo = (int)item.Grupopadre;

                PrGrupoDTO grupoPadre = listaPrGrupo.Where(x => x.Grupocodi == grupoCodiGrupo).FirstOrDefault();

                int grupoCodiCentral = (int)grupoPadre.Grupopadre;
                int equicodi =
                    FactorySic.GetEqEquipoRepository().ListarEquiposPrGrupo(grupoCodiGrupo.ToString()).FirstOrDefault().Equicodi;


                string gruponombModo = listaPrGrupo.Where(x => x.Grupocodi == grupocodiModo).FirstOrDefault().Gruponomb;
                string gruponombGrupo =
                    listaPrGrupo.Where(x => x.Grupocodi == grupoCodiGrupo).FirstOrDefault().Gruponomb;
                string gruponombCentral =
                    listaPrGrupo.Where(x => x.Grupocodi == grupoCodiCentral).FirstOrDefault().Gruponomb;
                string equinomb = FactorySic.GetEqEquipoRepository().GetById(equicodi).Equinomb;

                MePtomedicionDTO puntoMedicionpuntoMedicionMedidor =
                    listaPtoMedicion.Where(
                            x =>
                                x.Grupocodi == grupoCodiGrupo &&
                                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaMedidores)
                        .FirstOrDefault();

                int ptomedicodiMedidor = (puntoMedicionpuntoMedicionMedidor == null
                    ? -1
                    : puntoMedicionpuntoMedicionMedidor.Ptomedicodi);
                string ptomedidescMedidor = (puntoMedicionpuntoMedicionMedidor == null
                    ? ""
                    : puntoMedicionpuntoMedicionMedidor.Ptomedidesc);


                MePtomedicionDTO puntoMedicionpuntoMedicionDespacho =
                    listaPtoMedicion.Where(
                            x =>
                                x.Grupocodi == grupoCodiGrupo &&
                                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaDespacho)
                        .FirstOrDefault();

                int ptomedicodiDespacho = (puntoMedicionpuntoMedicionDespacho == null
                    ? -1
                    : puntoMedicionpuntoMedicionDespacho.Ptomedicodi);
                string ptomedidescDespacho = (puntoMedicionpuntoMedicionDespacho == null
                    ? ""
                    : puntoMedicionpuntoMedicionDespacho.Ptomedidesc);

                /*
                MePtomedicionDTO puntoMedicionpuntoMedicionPdo =
    listaPtoMedicion.Where(
            x =>
                x.Grupocodi == grupoCodiGrupo &&
                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLectur )
        .FirstOrDefault();

                int ptomedicodiPdo = (puntoMedicionpuntoMedicionPdo == null
                    ? -1
                    : puntoMedicionpuntoMedicionPdo.Ptomedicodi);
                string ptomedidescPdo = (puntoMedicionpuntoMedicionPdo == null
                    ? ""
                    : puntoMedicionpuntoMedicionPdo.Ptomedidesc);*/


                //codigo de central
                int equicodiCentral = (int)FactorySic.GetEqEquipoRepository().GetById(equicodi).Equipadre;
                string equinombCentral = FactorySic.GetEqEquipoRepository().GetById(equicodiCentral).Equiabrev;

                //código de ptomedicodi Stock
                MePtomedicionDTO puntoStock =
                    listaPtoMedicion.Where(
                        x =>
                            x.Equicodi == equicodiCentral &&
                            x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaStock &&
                            x.Tipoptomedicodi == ConstantesReservaFriaNodoEnergetico.TptomedicodiStock).FirstOrDefault();

                int ptomedicodiStock = puntoStock == null ? -1 : puntoStock.Ptomedicodi;
                string ptomedidescStock = puntoStock == null ? "_NO DEFINIDO" : puntoStock.Ptomedidesc;

                //codigo de ptomedicodi Nodo Energetico
                MePtomedicionDTO puntoNodoEnergetico =
                    listaPtoMedicion.Where(
                            x =>
                                x.Equicodi == equicodi &&
                                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico)
                        .FirstOrDefault();

                int ptomedicodiNodoEnergetico = puntoNodoEnergetico == null ? -1 : puntoNodoEnergetico.Ptomedicodi;
                string ptomedidescNodoEnergetico = puntoNodoEnergetico == null ? "_NO DEFINIDO" : puntoNodoEnergetico.Ptomedidesc;

                //codigo de ptomedicodi ReservaFria
                MePtomedicionDTO puntoReservaFria =
                    listaPtoMedicion.Where(
                            x =>
                                x.Equicodi == equicodi &&
                                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaReservaFria)
                        .FirstOrDefault();
                /*
                MePtomedicionDTO puntoReservaFria =
                    listaPtoMedicion.Where(
                            x =>
                                x.Grupocodi == grupoCodiGrupo &&
                                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaReservaFria)
                        .FirstOrDefault();
                */

                int ptomedicodiReservaFria = puntoReservaFria == null ? -1 : puntoReservaFria.Ptomedicodi;
                string ptomedidescReservaFria = puntoReservaFria == null ? "_NO DEFINIDO" : puntoReservaFria.Ptomedidesc;

                //codigo de ptomedicodi Central Nodo Energetico
                MePtomedicionDTO puntoMedicionCentralNodoEnergetico =
                    listaPtoMedicion.Where(
                            x =>
                                x.Grupocodi == grupoCodiCentral &&
                                x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaNodoEnergetico)
                        .FirstOrDefault();

                int ptomedicodiCentralNodoEnergetico = puntoMedicionCentralNodoEnergetico == null ? -1 : puntoMedicionCentralNodoEnergetico.Ptomedicodi;
                string ptomedidescCentralNodoEnergetico = puntoMedicionCentralNodoEnergetico == null ? "_NO DEFINIDO" : puntoMedicionCentralNodoEnergetico.Ptomedidesc;


                //código de ptomedicodi Volumen Programado
                MePtomedicionDTO puntoVolProg =
                    listaPtoMedicion.Where(
                        x =>
                            x.Grupocodi == grupoCodiCentral &&
                            x.Origlectcodi == ConstantesReservaFriaNodoEnergetico.OrigenLecturaCombProgramado &&
                            x.Tipoptomedicodi == ConstantesReservaFriaNodoEnergetico.TptomedicodiMedidaElectrica).FirstOrDefault();

                int ptomedicodiCombProg = puntoVolProg == null ? -1 : puntoVolProg.Ptomedicodi;
                string ptomedidescCombProg = puntoVolProg == null ? "_NO DEFINIDO" : puntoVolProg.Ptomedidesc;



                ModoGrupoCentralEquipo elemento = new ModoGrupoCentralEquipo
                {
                    GrupocodiModo = grupocodiModo,
                    GrupoCodiGrupo = grupoCodiGrupo,
                    GrupoCodiCentral = grupoCodiCentral,
                    Equicodi = equicodi,
                    PtomedicodiMedidor = ptomedicodiMedidor,
                    PtomedicodiDespacho = ptomedicodiDespacho,
                    EquicodiCentral = equicodiCentral,
                    PtomedicodiStock = ptomedicodiStock,
                    PtomedicodiNodoEnergetico = ptomedicodiNodoEnergetico,
                    PtomedicodiReservaFria = ptomedicodiReservaFria,
                    PtomedicodiCentralNodoEnergetico = ptomedicodiCentralNodoEnergetico,
                    PtomedicodiCombProgramado = ptomedicodiCombProg,

                    GruponombModo = gruponombModo,
                    GruponombGrupo = gruponombGrupo,
                    GruponombCentral = gruponombCentral,
                    EquiNomb = equinomb,
                    PtomedidescMedidor = ptomedidescMedidor,
                    PtomedidescDespacho = ptomedidescDespacho,
                    EquiNombCentral = equinombCentral,
                    PtomedidescStock = ptomedidescStock,
                    PtomedidescNodoEnergetico = ptomedidescNodoEnergetico,
                    PtomedidescReservaFria = ptomedidescReservaFria,
                    PtomedidescCentralNodoEnergetico = ptomedidescCentralNodoEnergetico,
                    PtomedidescCombProgramado = ptomedidescCombProg
                };

                listaEquivalenciaModoGrupoCentralEquipo.Add(elemento);
            }

            return listaEquivalenciaModoGrupoCentralEquipo;
        }

        public string NotaRpfCero { get; set; }


        #endregion

        #region Mediciones

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_LECTURA
        /// </summary>
        public List<MeLecturaDTO> ListMeLecturas()
        {
            return FactorySic.GetMeLecturaRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_TIPOINFORMACION
        /// </summary>
        public List<SiTipoinformacionDTO> ListSiTipoinformacions()
        {
            return FactorySic.GetSiTipoinformacionRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_MEDICION48
        /// </summary>
        public MeMedicion48DTO GetByIdMeMedicion48(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            return FactorySic.GetMeMedicion48Repository().GetById(lectcodi, medifecha, tipoinfocodi, ptomedicodi);
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_MEDICION48
        /// </summary>
        public void DeleteMeMedicion48(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi)
        {
            try
            {
                FactorySic.GetMeMedicion48Repository().Delete(lectcodi, medifecha, tipoinfocodi, ptomedicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba los datos de la tabla ME_MEDICION48
        /// </summary>
        public int SaveMeMedicion48Id(MeMedicion48DTO entity)
        {
            return FactorySic.GetMeMedicion48Repository().SaveMeMedicion48Id(entity);
        }


        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla ME_MEDICION48
        /// </summary>
        public List<MeMedicion48DTO> BuscarOperaciones(int lectcodi, int tipoinfocodi, int ptomedicodi, DateTime medifecha, DateTime lastdate, int nroPage, int pageSize)
        {
            return FactorySic.GetMeMedicion48Repository().BuscarOperaciones(lectcodi, tipoinfocodi, ptomedicodi, medifecha, lastdate, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla ME_MEDICION48
        /// </summary>
        public int ObtenerNroFilas(int lectcodi, int tipoinfocodi, int ptomedicodi, DateTime medifecha, DateTime lastdate)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerNroFilas(lectcodi, tipoinfocodi, ptomedicodi, medifecha, lastdate);
        }

        public List<PrGrupoDTO> ListarModoOperacionSubModulo(int subModulo)
        {
            return FactorySic.GetPrGrupoRepository().ListarModoOperacionSubModulo(subModulo);
        }


        public List<MePtomedicionDTO> ListPtoMedicion(string origlectcodi)
        {
            try
            {
                return FactorySic.GetMePtomedicionRepository().ListByOriglectcodi(origlectcodi, DateTime.Now, DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla EVE_HORAOPERACION
        /// </summary>
        public List<MeMedicion96DTO> GetByCriteriaMedicion96(int idTipoInformacion, int idPtoMedicion, int idLectura, DateTime fechaInicio,
            DateTime fechaFin)
        {
            return FactorySic.GetMeMedicion96Repository().GetByCriteria(idTipoInformacion, idPtoMedicion, idLectura, fechaInicio, fechaFin);
        }

        public PrGrupoDTO ObtenerModoOperacion(int iGrupoCodi)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerModoOperacion(iGrupoCodi);
        }

        /// <summary>
        /// Permite listar el maestro de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarMaestroEmpresas()
        {
            return FactorySGDoc.GetConsultaRepository().ListarMaestroEmpresas();

        }

        #endregion

        #region Reserva     

        public List<ReservaDTO> ObtenerReservaDiariaEjecutada(DateTime dtFecha)
        {
            return FactorySic.GetNrProcesoRepository().ObtenerReservaDiariaEjecutada(dtFecha);
        }
        #endregion
    }
}


public class ModoGrupoCentralEquipo
{
    public int GrupocodiModo;
    public int GrupoCodiGrupo;
    public int GrupoCodiCentral;
    public int Equicodi;
    public int PtomedicodiMedidor;
    public int PtomedicodiDespacho;
    public int EquicodiCentral;
    public int PtomedicodiStock;
    public int PtomedicodiNodoEnergetico;//PdoSimulado
    public int PtomedicodiReservaFria; //RdoSimulado    
    public int PtomedicodiCentralNodoEnergetico;
    public int PtomedicodiCombProgramado;

    public string GruponombModo;
    public string GruponombGrupo;
    public string GruponombCentral;
    public string EquiNomb;
    public string PtomedidescMedidor;
    public string PtomedidescDespacho;
    public string EquiNombCentral;
    public string PtomedidescStock;
    public string PtomedidescNodoEnergetico;//PdoSimulado
    public string PtomedidescReservaFria;//RdoSimulado    
    public string PtomedidescCentralNodoEnergetico;
    public string PtomedidescCombProgramado;
}


public class CentralFechaUnidad
{
    public int GrupoCodiCentral;
    public DateTime Fecha;
    public ArrayList GrupocodiModo = new ArrayList();

    public int PtomedicodiCentral;

    public CentralFechaUnidad(int grupocodiCentral, DateTime fecha, int ptomedicodiCentral)
    {
        GrupoCodiCentral = grupocodiCentral;
        Fecha = fecha;
        PtomedicodiCentral = ptomedicodiCentral;
    }

    public void AgregarGrupocodimodo(int grupocodimodo)
    {
        if (GrupocodiModo.IndexOf(grupocodimodo) < 0)
            GrupocodiModo.Add(grupocodimodo);
    }
}


public class GrupoEde
{
    public int Grupocodi;
    public decimal Ede;

}


/// <summary>
/// Clase que realiza los calculos EDE (Energia dejada de entregar). Su desarrollo se basó en una hoja que contiene varias fórmulas (EDE4_segundos.xlsx).
/// </summary>
public class EdeExcel
{
    public decimal[] medidor96 { get; set; }// = new decimal[96];
    //public decimal[] valorCorregido = new decimal[96];
    //public int[] delta = new int[96];
    //public decimal[] bitFalla = new decimal[96];
    //public int[] horaCorregida = new int[96];
    //public decimal[] deltaValorCorregido = new decimal[96];
    public int[] bloqueConsigna { get; set; }//;
    public decimal[] potenciaConsigna { get; set; }//;
    //public decimal[] deltaConsigna;
    public decimal[] curvaIdeal = new decimal[86400];
    //public decimal[] curvaFalla = new decimal[86400];
    //public decimal[] curvaReal1 = new decimal[86400];
    public decimal[] curvaReal = new decimal[86400];
    public decimal[] curvaEdeSegundo = new decimal[86400];
    public decimal[] curvaEde15Minuto = new decimal[96];

    //variables de configuracion
    //tiempo de sicronizacion
    public int minutoSincronizacion { get; set; }// Ej: 30 minutos (EN ARCHIVO DE CONSTANTES)
    //velocidad de toma de carga
    public decimal tomaCargaMwMin { get; set; }//= 6; //se extrae de ficha tecnica (FALTA)
    //velocidad de reducción de carga
    public decimal reduccionCargaMwMin { get; set; }//= 6; //se extrae de ficha tecnica (FALTA)    

    //hora de falla se debe extraer de hora de operacion 
    public DateTime? fechaFalla { get; set; }//Ejemplo: Convert.ToDateTime("2016-01-01 01:42:00");
    public DateTime fechaSincronizacionReal { get; set; }//Ejemplo: Convert.ToDateTime("2016-01-01 00:20:00");//de horas de operacion
    public decimal edeTotal = 0;
    public decimal potenciaLimite { get; set; }
    public decimal rpf { get; set; }

    /// <summary>
    /// Permite realizar el cálculo de la energía dejada de entregar
    /// </summary>
    /// <param name="calculoTotal"></param>
    public void Calcular(bool calculoTotal)
    {

        int segundoSincronizacion = 60 * minutoSincronizacion;
        int segundoSincronizReal = 60 * fechaSincronizacionReal.Minute;
        int bloqueFalla1 = -1;

        //2. potencia limite //EN NUEVO CALCULO YA NO HAY COALCULO DE POTENCIA LIMITE
        potenciaLimite = 0;
        //if (potenciaLimite == 0)
        //potenciaLimite = CalculaPotenciaLimite(medidor96);

        //if (potenciaLimite > 0)
        {

            //11. Delta consigna (Verificado)
            //deltaConsigna = ObtenerDeltaConsigna(potenciaConsigna);

            //12. Curva Ideal (Verificado) - NUEVO
            decimal potenciaInicial = 0;// 50;

            if (bloqueConsigna[0] == 0)
            {
                potenciaInicial = potenciaConsigna[0];
            }

            curvaIdeal = ObtenerCurvaIdeal(bloqueConsigna, potenciaConsigna, tomaCargaMwMin, reduccionCargaMwMin, potenciaInicial);

            if (calculoTotal)
            {

                //3. detectar bloque de falla
                //bloqueFalla1 = ObtenerBloqueFalla96(fechaFalla);
                //MessageBox.Show(bloqueFalla1+"");

                //4. pasos Calc1 (valor corregido) (Verificado)
                //valorCorregido = CalcularValorCorregido(medidor96, fechaSincronizacionReal, bloqueFalla1,
                //(DateTime)fechaFalla, potenciaLimite);

                //5. Delta (Verificado)
                //delta = CalcularDelta(medidor96, valorCorregido);


                //6. BitFalla (Verificado)
                //bitFalla = CalcularBitFalla(medidor96, bloqueFalla1, (DateTime)fechaFalla, valorCorregido,
                //potenciaLimite, fechaSincronizacionReal.Day == ((DateTime)fechaFalla).Day);

                //7. Hora corregida (Verificado)
                //horaCorregida = CalcularHoraCorregida(bitFalla, delta);

                //8. Delta valor Corregido (Verificado)
                //deltaValorCorregido = CalcularDeltaValorCorregido(valorCorregido);

                //13.A. Bloque Pre Falla (Verificado)
                //int bloquePreFalla = ObtenerBloquePreFalla(fechaFalla);

                //13.B. Energia Pre Falla (Verificado)
                //decimal energiaPreFalla = ObtenerEnergiaPreFalla(bloquePreFalla, valorCorregido);

                //13.C. Energia Post Falla (Verificado)
                //decimal energiaPostFalla = ObtenerEnergiaPostFalla(bloquePreFalla, valorCorregido);

                int segundoFalla = fechaSincronizacionReal.Day == ((DateTime)fechaFalla).Day ? ObtenerSegundoFalla(fechaFalla) : 86400; // (Verificado)
                //int segundoPreFalla = (bloquePreFalla + 1) * 15 * 60; // (Verificado)

                //13. Curva Falla (Verificado)
                //curvaFalla = ObtenerCurvaFalla(segundoFalla, energiaPreFalla, energiaPostFalla, segundoPreFalla);

                //14. Curva Real1 (Verificado)
                //curvaReal1 = ObtenerCurvaReal1(horaCorregida, valorCorregido, deltaValorCorregido, segundoSincronizReal);
                //string logCr1 = ObtenerLog(curvaReal1);

                //15. Curva Real (Verificado)
                //curvaReal = ObtenerCurvaReal(segundoFalla, curvaReal1, curvaFalla);

                curvaReal = ObtenerCurvaReal(bloqueConsigna, reduccionCargaMwMin, medidor96, curvaIdeal, segundoFalla, fechaSincronizacionReal);
                string logCr = ObtenerLog(curvaReal);

                //16. EDE al segundo (Verificado)
                curvaEdeSegundo = ObtenerCurvaEde(curvaIdeal, curvaReal);
                string logEs = ObtenerLog(curvaEdeSegundo);

                //17. EDE 15 minutos (Verificado)
                edeTotal = 0;
                curvaEde15Minuto = ObtenerCurvaEde15Min(curvaEdeSegundo, out edeTotal);
                string logEde15 = ObtenerLog(curvaEde15Minuto);

            }
        }
    }


    private string ObtenerLog(decimal[] dato)
    {
        StringBuilder sb = new StringBuilder(); ;
        for (int i = 0; i < dato.Length; i++)
        {
            sb.Append(i + "," + dato[i] + "\r\n");
        }

        string a = sb.ToString();
        return a;

    }

    /// <summary>
    /// Permite calcular la energía dejada de entregar en intervalos de 15 minutos
    /// </summary>
    /// <returns></returns>
    public decimal[] CalcularEde15MinSinCurvaReal()
    {
        for (int i = 0; i < 86400; i++)
        {
            curvaReal[i] = 0;
        }

        //17. EDE 15 minutos (Verificado)
        edeTotal = 0;
        curvaEde15Minuto = ObtenerCurvaEde15Min(curvaIdeal, out edeTotal);

        return curvaEde15Minuto;
    }


    /// <summary>
    /// Permite calcular la ede a nivel de segundo
    /// </summary>
    /// <param name="segundoInicio"></param>
    /// <param name="segundoFin"></param>
    /// <param name="edeTotal15"></param>
    /// <returns></returns>
    public decimal[] CalcularEde15(int segundoInicio, int segundoFin, ref decimal edeTotal15)
    {
        decimal[] ede15Min = new decimal[96];
        decimal[] edeSeg = new decimal[86400];
        decimal[] edeIdealTemp = new decimal[86400];
        decimal[] edeRealTemp = new decimal[86400];
        decimal[] curvaEdeSegundo = new decimal[86400];

        edeTotal15 = 0;

        for (int i = 0; i < 86400; i++)
        {
            if (segundoInicio <= i && i <= segundoFin)
            {
                edeIdealTemp[i] = curvaIdeal[i];
                edeRealTemp[i] = curvaReal[i];
            }
            else
            {
                edeIdealTemp[i] = 0;
                edeRealTemp[i] = 0;
            }

        }

        //16. EDE al segundo (Verificado)
        curvaEdeSegundo = ObtenerCurvaEde(edeIdealTemp, edeRealTemp);

        //17. EDE 15 minutos (Verificado)
        edeTotal15 = 0;
        ede15Min = ObtenerCurvaEde15Min(curvaEdeSegundo, out edeTotal15);

        return ede15Min;
    }


    /// <summary>
    /// Permite realizar la suma de la curva secundaria a una ideal
    /// </summary>
    /// <param name="curvaIdealSecundario"></param>
    public void SumarCurvaIdeal(decimal[] curvaIdealSecundario)
    {
        string logPrevio = ObtenerLog(curvaIdeal);

        for (int i = 0; i < 86400; i++)
        {
            curvaIdeal[i] += curvaIdealSecundario[i];
        }

        string logPost = ObtenerLog(curvaIdeal);
    }

    /// <summary>
    /// PErmite calcular la ede
    /// </summary>
    public void CalcularEde()
    {
        //16. EDE al segundo (Verificado)
        curvaEdeSegundo = ObtenerCurvaEde(curvaIdeal, curvaReal);

        //17. EDE 15 minutos (Verificado)
        edeTotal = 0;
        curvaEde15Minuto = ObtenerCurvaEde15Min(curvaEdeSegundo, out edeTotal);
    }


    /// <summary>
    /// Permite realizar la suma de la curva secundaria a una real
    /// </summary>
    /// <param name="curvaRealSecundario"></param>
    public void SumarCurvaReal(decimal[] curvaRealSecundario)
    {
        string logPrevio = ObtenerLog(curvaReal);

        for (int i = 0; i < 86400; i++)
        {
            curvaReal[i] += curvaRealSecundario[i];
        }

        string logPost = ObtenerLog(curvaReal);
    }

    /// <summary>
    /// Permite obtener una curva real desde medidores
    /// </summary>
    /// <param name="curvaMedidor">mediciones de 15 minutos (96 datos)</param>
    public decimal[] ObtenerCurvaRealDesdeMedidores(decimal[] curvaMedidor)
    {
        decimal[] curva = new decimal[86400];
        int idx = 0;

        for (int i = 0; i < 96; i++)
        {
            decimal datoMedidor = curvaMedidor[i];
            for (int j = 0; j < 900; j++)
            {
                curva[idx] = datoMedidor / (decimal)900.0;
                idx++;
            }
        }

        return curva;
    }


    /// <summary>
    /// Permie obtener la ede en intervalos de 15 minutos y obtiene la suma total
    /// </summary>
    /// <param name="curvaEdeSegundo">Curva Ede a nive de segundo</param>
    /// <param name="edeTotal">Total ede del día</param>
    /// <returns></returns>
    public decimal[] ObtenerCurvaEde15Min(decimal[] curvaEdeSegundo, out decimal edeTotal)
    {
        decimal[] curvaEde15 = new decimal[96];
        edeTotal = 0;

        for (int i = 0; i < 96; i++)
        {
            curvaEde15[i] = 0;
        }

        for (int i = 0; i < 86400; i++)
        {

            curvaEde15[i / 900] += curvaEdeSegundo[i] / 3600;
            //edeTotal += curvaEdeSegundo[i];
        }

        //se redondean los valores al tercer decimal
        for (int i = 0; i < 96; i++)
        {
            curvaEde15[i] = Math.Round(curvaEde15[i], 3);
            edeTotal += curvaEde15[i];
        }


        //edeTotal /= 3600;
        return curvaEde15;
    }


    /// <summary>
    /// PErmite obtener la curva ede neta
    /// </summary>
    /// <param name="curvaIdeal">Curva ideal Ede</param>
    /// <param name="curvaReal">Curva real Ede</param>
    /// <returns></returns>
    public decimal[] ObtenerCurvaEde(decimal[] curvaIdeal, decimal[] curvaReal)
    {
        decimal[] curvaEde = new decimal[86400];
        decimal delta = 0;


        for (int i = 0; i < 86400; i++)
        {
            delta = curvaIdeal[i] - curvaReal[i];

            if (delta < 0)
            {
                curvaEde[i] = 0;
            }
            else
            {
                curvaEde[i] = delta;
            }

        }

        return curvaEde;
    }

    /// <summary>
    /// Permite obtener la curva real según nuevas especificaciones SEV
    /// </summary>
    /// <param name="bloqueConsigna">Bloque consigna</param>
    /// <param name="velocDescarga">Velocidad de descarga</param>
    /// <param name="medidor96">Datos de medidores</param>
    /// <param name="curvaIdeal">Datos de la curva ideal</param>
    /// <param name="segundoFalla">Segundo de falla (0-86400)</param>
    /// <param name="fechaSincronizacionReal">Fecha de sincronización real</param>
    /// <returns></returns>
    private decimal[] ObtenerCurvaReal(int[] bloqueConsigna,
    decimal velocDescarga, decimal[] medidor96, decimal[] curvaIdeal, int segundoFalla,
    DateTime fechaSincronizacionReal)
    {
        decimal[] curvaReal = new decimal[86400];

        for (int i = 0; i < 86400; i++)
        {
            curvaReal[i] = 0;
        }

        //*************** 
        //*** primer bloque
        //***************
        int segundoInicial;

        segundoInicial = 3600 * fechaSincronizacionReal.Hour + 60 * fechaSincronizacionReal.Minute;

        /*
        if (bloqueConsigna !=null && bloqueConsigna.Length != 0)
        {
            segundoInicial = bloqueConsigna[0];
        }
        else
        {
            segundoInicial = 3600 * fechaSincronizacionReal.Hour + 60 * fechaSincronizacionReal.Minute;
        }
        */

        int segundo = segundoInicial % 3600;// segundoInicial - (int)(segundoInicial / 3600);
        int minutoInicial = (int)(segundoInicial / 60);
        int minutoInicialUsar = 15 - minutoInicial % 15;
        decimal factorInicial = minutoInicialUsar == 0 ? 1 : (decimal)(15 / (minutoInicialUsar * 1.0));

        int hora = segundoInicial / 3600;
        int minuto = (int)((segundoInicial - hora * 3600) / (60));

        int hIni = 4 * hora + minuto / 15;// +1;

        int hiPrimero = hIni;
        int hiSegundo = hIni + 1;

        //***************
        //***bloque final
        //***************
        int segundoFinal = bloqueConsigna[bloqueConsigna.Length - 1];

        //velocidad de descarga (MW/min ->MW/seg)
        velocDescarga = velocDescarga / 60;
        //int indice = bloqueConsigna.Length - 2 >= 0 ? bloqueConsigna.Length - 2 : bloqueConsigna.Length - 1;

        decimal ultimaPotencia = curvaIdeal[bloqueConsigna[bloqueConsigna.Length - 1] - 1];

        //int segundosPotenciaCero = (int)(potenciaConsigna[bloqueConsigna.Length - 2] / velocDescarga);
        int segundosPotenciaCero = (int)(ultimaPotencia / velocDescarga);
        int minutosPotenciaCero = (int)(segundosPotenciaCero / 60);

        int segundosTiempoCero = bloqueConsigna[bloqueConsigna.Length - 1] + segundosPotenciaCero;



        int hiPenultimo = (int)(segundosTiempoCero / (15 * 60));

        int minutosUltimo = (int)((segundosTiempoCero - hiPenultimo * 900) / 60);
        decimal factorFinal = minutosUltimo == 0 ? 1 : (decimal)(15 / (minutosUltimo * 1.0));

        int hiUltimo = hiPenultimo + 1;

        //pasando los datos a curva real

        //parte inicial
        for (int i = segundoInicial; i < (hiSegundo * 900); i++)
        {
            curvaReal[i] = factorInicial * medidor96[i / 900] * (1 + rpf);
        }

        //parte media
        for (int i = hiSegundo * 900; i < (hiPenultimo * 900); i++)
        {
            curvaReal[i] = medidor96[i / 900] * (1 + rpf);
        }

        //parte final
        for (int i = hiPenultimo * 900; i < (segundosTiempoCero); i++)
        {
            curvaReal[i] = factorFinal * medidor96[i / 900] * (1 + rpf);
        }


        //hallando falla
        for (int i = segundoFalla; i < 86400; i++)
        {
            curvaReal[i] = 0;
        }



        //imprimir
        StringBuilder sb = new StringBuilder();

        for (int j = 0; j < 86400; j++)
        {
            int hora1 = j / 3600;
            int min1 = (j - 3600 * hora1) / 60;
            int seg1 = j - (3600 * hora1 + 60 * min1);

            string hms = hora1 + ":" + min1 + ":" + seg1;

            sb.Append(j + "," + hms + "," + curvaReal[j] + "\r\n");
        }

        string cad = sb.ToString();


        return curvaReal;

    }


    /// <summary>
    /// Permite copiar la curva ede a partir de una curva real o de falla considerando la hora de falla
    /// </summary>
    /// <param name="segundoFalla">Segundo de falla</param>
    /// <param name="curvaReal1">Curva real</param>
    /// <param name="curvaFalla">Curva de falla</param>
    /// <returns></returns>
    public decimal[] ObtenerCurvaReal(int segundoFalla, decimal[] curvaReal1, decimal[] curvaFalla)
    {
        decimal[] curvaReal = new decimal[86400];

        for (int i = 0; i < 86400; i++)
        {
            if (i >= segundoFalla)
            {
                curvaReal[i] = 0;
            }
            else
            {
                if (curvaFalla[i] == 0)
                {
                    curvaReal[i] = curvaReal1[i];
                }
                else
                {
                    curvaReal[i] = curvaFalla[i];
                }

            }
        }

        return curvaReal;
    }


    /// <summary>
    /// Permite obtener una curva real secundaria a partir de las potencias consigna.
    /// </summary>
    /// <param name="bloqueConsigna"></param>
    /// <param name="potenciaConsigna"></param>
    /// <param name="deltaConsigna"></param>
    /// <param name="segundoSincronizacionReal"></param>
    /// <returns></returns>
    public decimal[] ObtenerCurvaReal1(int[] bloqueConsigna, decimal[] potenciaConsigna, decimal[] deltaConsigna,
        int segundoSincronizacionReal)
    {

        decimal[] curvaReal1 = new decimal[86400];

        //se reemplaza la por el segundo de sincronizacion real
        bloqueConsigna[0] = segundoSincronizacionReal;

        for (int i = 0; i < 86400; i++)
        {
            curvaReal1[i] = 0;
        }

        int segundoAnterior = 0;
        decimal potenciaAnterior = 0;

        for (int i = 0; i < bloqueConsigna.Length; i++)
        {
            int segundoActual = bloqueConsigna[i] == 86400 ? 86399 : bloqueConsigna[i];
            decimal potenciaActual = potenciaConsigna[i];
            decimal deltaConsign = deltaConsigna[i];

            //if (deltaConsign != 0)
            //{
            for (int j = segundoAnterior; j <= segundoActual; j++)
            {
                if (deltaConsign != 0)
                {
                    if (segundoActual != segundoAnterior)
                    {
                        curvaReal1[j] = potenciaAnterior +
                                        deltaConsign * (j - segundoAnterior) / (segundoActual - segundoAnterior);
                    }
                    else
                    {
                        curvaReal1[j] = potenciaAnterior + deltaConsign;
                    }
                }
                else
                {
                    curvaReal1[j] = potenciaAnterior;
                }
            }
            //}

            potenciaAnterior = potenciaActual;
            segundoAnterior = segundoActual;

        }

        return curvaReal1;
    }


    /// <summary>
    /// Permite obtener la curva de falla considerando la hora de falla y prefalla, así como la energía pre y post falla
    /// </summary>
    /// <param name="segundoFalla">Segundo de la falla</param>
    /// <param name="energiaPreFalla">Energía prefalla</param>
    /// <param name="energiaPostFalla">Energía postfalla</param>
    /// <param name="segundoPreFalla">Segundo prefalla</param>
    /// <returns></returns>
    public decimal[] ObtenerCurvaFalla(int segundoFalla, decimal energiaPreFalla, decimal energiaPostFalla, int segundoPreFalla)
    {
        decimal[] curvaFalla = new decimal[86400];


        for (int i = 0; i < 86400; i++)
        {

            if (i <= segundoPreFalla)
            {
                curvaFalla[i] = 0;
            }
            else
            {
                decimal form1;
                if (i >= segundoFalla)
                {
                    form1 = 0;
                }
                else
                {
                    form1 = energiaPreFalla -
                                    (i - segundoPreFalla) * (energiaPreFalla - energiaPostFalla) /
                                    (segundoFalla - segundoPreFalla);
                }

                curvaFalla[i] = form1;
            }
        }

        return curvaFalla;
    }


    /// <summary>
    /// Permite obtener la energía pre falla
    /// </summary>
    /// <param name="bloquePreFalla">Bloque 15 minutos pre falla</param>
    /// <param name="valorCorregido">Valor corregido de energía</param>
    /// <returns></returns>
    public decimal ObtenerEnergiaPreFalla(int bloquePreFalla, decimal[] valorCorregido)
    {
        decimal energia = 0;
        if (bloquePreFalla <= 96)
        {
            energia = valorCorregido[bloquePreFalla];
        }

        return energia;
    }


    /// <summary>
    /// Permite obtener la energía post falla
    /// </summary>
    /// <param name="bloquePostFalla">>Bloque 15 minutos post falla</param>
    /// <param name="valorCorregido">Valor corregido de energía</param>
    /// <returns></returns>
    public decimal ObtenerEnergiaPostFalla(int bloquePostFalla, decimal[] valorCorregido)
    {
        decimal energia = 0;

        if (bloquePostFalla < 96)
        {
            energia = valorCorregido[bloquePostFalla + 1];
        }

        return energia;
    }


    /// <summary>
    /// Permite obtener el segundo de la falla
    /// </summary>
    /// <param name="fechaFalla">Fecha de la falla</param>
    /// <returns></returns>
    public int ObtenerSegundoFalla(DateTime? fechaFalla)
    {
        int bloque;

        if (fechaFalla == null)
            bloque = 86400;
        else
        {
            DateTime fecha = (DateTime)fechaFalla;
            bloque = ObtenerBloqueConsignaSegundo(fecha);
        }

        return bloque;
    }


    /// <summary>
    /// Permite obtener el bloque de 15" de la pre-falla
    /// </summary>
    /// <param name="fechaFalla">Fecha de la falla</param>
    /// <returns></returns>
    public int ObtenerBloquePreFalla(DateTime? fechaFalla)
    {
        int bloque;

        if (fechaFalla == null)
            bloque = 96;
        else
        {
            DateTime fecha = (DateTime)fechaFalla;

            bloque = 4 * fecha.Hour + fecha.Minute / 15;

        }

        if (bloque > 0)
            bloque--;

        return bloque;
    }


    /// <summary>
    /// Pemite obtener la curva ideal auxiliar
    /// </summary>
    /// <param name="bloqueConsigna">Bloque consigna</param>
    /// <param name="deltaConsigna">Delta de la consigna</param>
    /// <param name="potenciaConsigna">Potencia consigna</param>
    /// <param name="segundoSincronizacion">Segundo de sincronización</param>
    /// <param name="tomaCargaMwMin">Velocidad de toma de carga</param>
    /// <returns></returns>

    private decimal[] ObtenerCurvaIdeal(int[] bloqueConsigna, decimal[] potenciaConsigna, decimal velocTomaCarga,
    decimal velocDescarga, decimal potenciaInicial)
    {


        int[] consignaSegundo = bloqueConsigna;// new int[arrHora.Count]; //segundo
        decimal[] consignaMw = potenciaConsigna;// new decimal[arrHora.Count]; //MW

        //carga de datos
        /*
        for (int i = 0; i < arrHora.Count; i++)
        {
            DateTime fecha = Convert.ToDateTime(arrHora[i]);
            consignaSegundo[i] = 3600 * fecha.Hour + 60 * fecha.Minute + fecha.Second;

            consignaMw[i] = Convert.ToDecimal(arrMw[i]);
        }
        */




        decimal velocTomaSeg = velocTomaCarga / 60;
        decimal velocDescargaSeg = velocDescarga / 60;


        decimal[] consigna0 = new decimal[86400];
        decimal[] consigna1 = new decimal[86400];

        decimal[] curvaIdeal = new decimal[86400];

        string cad0 = "";

        for (int i = 0; i < 86400; i++)
        {
            curvaIdeal[i] = 0;
            consigna0[i] = -50;
        }

        //si hay potencia inicial viene operando
        if (potenciaInicial > 0)
        {
            consigna0[0] = potenciaInicial;
            consigna0[1] = potenciaInicial;
            curvaIdeal[0] = potenciaInicial;

        }
        else
        {
            consigna1[0] = 0;
            curvaIdeal[0] = 0;

        }



        for (int i = 0; i < 86400; i++)
        {
            //curvaIdeal[i] = 0;

            for (int j = 0; j < consignaSegundo.Count(); j++)
            {
                if (i == consignaSegundo[j])
                {
                    consigna0[i] = consignaMw[j];
                    cad0 += i + "," + consigna0[i] + "\r\n";
                    continue;
                    //break;
                }
                else
                {

                    //consigna0[i] = -50;
                    //break;
                }

            }
        }

        //estableciendo las consignas 1
        //consigna1[0] = 0;
        //curvaIdeal[0] = 0;
        for (int i = 1; i < 86400; i++)
        {
            if (consigna0[i] < 0)
            {
                consigna1[i] = consigna1[i - 1];
            }
            else
            {
                consigna1[i] = consigna0[i];
            }

            //estableciendo la curva ideal
            if (consigna1[i] == curvaIdeal[i - 1])
            {
                curvaIdeal[i] = curvaIdeal[i - 1];
            }
            else
            {
                decimal nuevoDato = 0;

                if (curvaIdeal[i - 1] < consigna1[i])
                {
                    nuevoDato = curvaIdeal[i - 1] + velocTomaSeg;

                    if (nuevoDato > consigna1[i])
                    {
                        nuevoDato = consigna1[i];
                    }

                    curvaIdeal[i] = nuevoDato;
                }
                else
                {
                    //curvaIdeal[i] = curvaIdeal[i - 1] - velocDescargaSeg;
                    nuevoDato = curvaIdeal[i - 1] - velocDescargaSeg;

                    if (nuevoDato < consigna1[i])
                    {
                        nuevoDato = consigna1[i];
                    }

                    curvaIdeal[i] = nuevoDato;

                }
            }

        }



        //imprimir
        StringBuilder sb = new StringBuilder();

        for (int j = 0; j < 86400; j++)
        {
            int hora = j / 3600;
            int min = (j - 3600 * hora) / 60;
            int seg = j - (3600 * hora + 60 * min);

            string hms = hora + ":" + min + ":" + seg;

            sb.Append(j + "," + hms + "," + curvaIdeal[j] + "," + consigna0[j] + "\r\n");
        }

        string cad = sb.ToString();

        return curvaIdeal;


    }


    public decimal[] ObtenerCurvaIdealAntiguo(int[] bloqueConsigna, decimal[] deltaConsigna, decimal[] potenciaConsigna, int segundoSincronizacion, decimal tomaCargaMwMin)
    {
        decimal[] curvaIdeal = new decimal[86400];
        decimal consignaSgte = 0;
        decimal consignaSgte2 = 0;
        int tiempoSgte2 = 0;
        int tiempoSgte = 0;

        for (int i = 0; i < 86400; i++)
        {
            curvaIdeal[i] = 0;
        }

        int indice = 0;
        for (int i = 0; i < 86400; i++)
        {
            if (indice == 0)
            {
                decimal form9 = 0;
                if (i < bloqueConsigna[indice])
                {
                    form9 = potenciaConsigna[indice] * (i - segundoSincronizacion) /
                            (bloqueConsigna[indice] - segundoSincronizacion);
                }
                else
                {
                    form9 = potenciaConsigna[indice];
                    indice++;

                    consignaSgte = indice >= deltaConsigna.Length ? 0 : potenciaConsigna[indice - 1];
                    tiempoSgte = indice >= deltaConsigna.Length ? 86400 : bloqueConsigna[indice];

                    consignaSgte2 = indice + 1 >= deltaConsigna.Length ? potenciaConsigna[bloqueConsigna.Length - 1] : potenciaConsigna[indice];
                    tiempoSgte2 = indice + 1 >= deltaConsigna.Length ? bloqueConsigna[bloqueConsigna.Length - 1] : bloqueConsigna[indice + 1];
                }

                curvaIdeal[i] = (i < segundoSincronizacion) ? 0 : form9;


            }
            else
            {

                if (i <= tiempoSgte)
                {
                    curvaIdeal[i] = consignaSgte;
                }
                else
                {
                    decimal formi = 0;
                    decimal cte = Math.Abs(deltaConsigna[indice]) / tomaCargaMwMin * 60;// / 60 / 24; //segundos para llegar a nuevo punto
                    if (i <= bloqueConsigna[indice] + cte)
                    {
                        decimal num0 = deltaConsigna[indice];
                        decimal num1 = (i - bloqueConsigna[indice]);
                        decimal num = num0 * num1;
                        formi = num / cte;
                        curvaIdeal[i] = potenciaConsigna[indice - 1] + formi;
                    }
                    else
                    {
                        if (i <= tiempoSgte2)
                        {
                            curvaIdeal[i] = consignaSgte2;
                        }
                        else
                        {
                            indice++;
                            if (indice >= deltaConsigna.Length)
                                break;

                            consignaSgte = indice >= deltaConsigna.Length ? 0 : potenciaConsigna[indice - 1];
                            tiempoSgte = indice >= deltaConsigna.Length ? 86400 : bloqueConsigna[indice];

                            consignaSgte2 = indice + 1 >= deltaConsigna.Length ? potenciaConsigna[bloqueConsigna.Length - 1] : potenciaConsigna[indice];
                            tiempoSgte2 = indice + 1 >= deltaConsigna.Length ? bloqueConsigna[bloqueConsigna.Length - 1] : bloqueConsigna[indice + 1];

                            curvaIdeal[i] = consignaSgte;
                        }
                    }
                }
            }
        }

        return curvaIdeal;
    }


    /// <summary>
    /// Permite obtener el bloque consigna a nivel de segundo
    /// </summary>
    /// <param name="fecha">Fecha</param>
    /// <returns></returns>
    private int ObtenerBloqueConsignaSegundo(DateTime fecha)
    {
        return fecha.Hour * 60 * 60 + fecha.Minute * 60 + fecha.Second;
    }


    /// <summary>
    /// Permite obtener los delta de las potencias consigna
    /// </summary>
    /// <param name="potenciaConsigna">Potencia consigna</param>
    /// <returns></returns>
    private decimal[] ObtenerDeltaConsigna(decimal[] potenciaConsigna)
    {
        int longitud = potenciaConsigna.Length;

        decimal[] deltaConsigna = new decimal[longitud];

        deltaConsigna[0] = 0;
        for (int i = 1; i < longitud; i++)
        {
            deltaConsigna[i] = potenciaConsigna[i] - potenciaConsigna[i - 1];
        }

        return deltaConsigna;
    }


    /// <summary>
    /// Permite calcular el delta del valor corregido
    /// </summary>
    /// <param name="valorCorregido">Valor corregido</param>
    /// <returns></returns>
    private decimal[] CalcularDeltaValorCorregido(decimal[] valorCorregido)
    {
        decimal[] deltaValorCorregido = new decimal[96];

        deltaValorCorregido[0] = 0;

        for (int i = 1; i < 96; i++)
        {
            deltaValorCorregido[i] = valorCorregido[i] - valorCorregido[i - 1];
        }

        return deltaValorCorregido;
    }


    /// <summary>
    /// Permite calcular la hora corregida basado en el bit de falla
    /// </summary>
    /// <param name="bitFalla">Arreglo que contiene el bit de falla</param>
    /// <param name="delta">Arreglo de diferencias de energía</param>
    /// <returns></returns>
    private int[] CalcularHoraCorregida(decimal[] bitFalla, int[] delta)
    {
        int[] horaCorregida = new int[96];


        for (int i = 0; i < 96; i++)
        {
            try
            {
                //=+IF(AC8=0,AA8,AA8-AE8)
                horaCorregida[i] = bitFalla[i] == 0 ? 15 * 60 * (i + 1) : 15 * 60 * (i + 1) - delta[i];
            }
            catch
            {
                horaCorregida[i] = 0;
            }
        }

        return horaCorregida;
    }


    /// <summary>
    /// Permite calcular a hora de falla en un arreglo de 15 minutos
    /// </summary>
    /// <param name="medidor96"></param>
    /// <param name="bloqueFalla"></param>
    /// <param name="fechaFalla"></param>
    /// <param name="valorCorregido"></param>
    /// <param name="potenciaLimite"></param>
    /// <returns></returns>
    private decimal[] CalcularBitFalla(decimal[] medidor96, int bloqueFalla, DateTime fechaFalla, decimal[] valorCorregido, decimal potenciaLimite, bool calcularBit)
    {
        decimal[] bitfalla = new decimal[96];

        bitfalla[0] = 0;
        bitfalla[1] = 0;

        for (int i = 2; i < 96; i++)
        {
            if (calcularBit)
            {
                try
                {
                    //=IF(Z8="Falla",0,IF(IF(AB8=0,0,IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7))>$AD$3,1,0))
                    //=IF(Z8="Falla", 0, FORM5)
                    if (i == bloqueFalla)
                    {
                        bitfalla[i] = 0;
                    }
                    else
                    {
                        //FORM1= AB8*2*(AA8-AA7)/($D$20-AA7)-AG7
                        decimal form1 = medidor96[i] * 2 * 15 / (fechaFalla.Minute % 15) - valorCorregido[i - 1];
                        //FORM2= AB8*2-AG7
                        decimal form2 = medidor96[i] * 2 - valorCorregido[i - 1];
                        //FORM3= IF(Z8="Falla", FORM1, FORM2)
                        decimal form3 = i == bloqueFalla ? form1 : form2;
                        //FORM4= IF(AB8=0, 0, FORM3)
                        decimal form4 = medidor96[i] == 0 ? 0 : form3;
                        //FORM5= IF(FORM4>$AD$3,1,0)
                        decimal form5 = form4 > potenciaLimite ? 1 : 0;

                        bitfalla[i] = form5;
                    }

                }
                catch
                {
                    bitfalla[i] = 0;
                }
            }
            else
            {
                bitfalla[i] = 0;
            }

        }

        return bitfalla;
    }


    /// <summary>
    /// Permite calcular el delta (diferencias) entre valores corregidos
    /// </summary>
    /// <param name="medidor96"></param>
    /// <param name="valorCorregido"></param>
    /// <returns></returns>
    private int[] CalcularDelta(decimal[] medidor96, decimal[] valorCorregido)
    {
        int[] delta = new int[96];

        for (int i = 0; i < 96; i++)
        {
            try
            {
                //=((AB8-0.5*(AG7+AG8))*15/(AG8-0.5*(AG7+AG8)))/60/24
                decimal suma = valorCorregido[i - 1] + valorCorregido[i];

                delta[i] =
                    (int)
                    Math.Round(
                        (medidor96[i] - (decimal)0.5 * suma) * 15 / (valorCorregido[i] - (decimal)0.5 * suma) *
                        60, 0);
            }
            catch
            {
                delta[i] = 0;
            }

        }

        return delta;
    }


    /// <summary>
    /// Permite calcular el valor corregido de energía
    /// </summary>
    /// <param name="medidor96"></param>
    /// <param name="tiempoSincroniz"></param>
    /// <param name="bloqueFalla"></param>
    /// <param name="fechaFalla"></param>
    /// <param name="potenciaLimite"></param>
    /// <returns></returns>
    private decimal[] CalcularValorCorregidoAntiguo(decimal[] medidor96, DateTime tiempoSincroniz, int bloqueFalla, DateTime fechaFalla, decimal potenciaLimite)
    {

        decimal[] valorCorregido = new decimal[96];

        for (int i = 0; i < 96; i++)
        {
            valorCorregido[i] = 0;
        }

        //celda 1
        //=IF(AB7=0,0,AB7*2*(AA7-AA6)/(K7-H6))
        int bloque = 4 * tiempoSincroniz.Hour + tiempoSincroniz.Minute / 15;

        valorCorregido[bloque] = medidor96[bloque] == 0
            ? 0
            : medidor96[bloque] * 2 * 15 / (30 - tiempoSincroniz.Minute);

        //=IF(Z10="Falla",IF(AB10=0,0,IF(Z10="Falla",AB10*2*(AA10-AA9)/($D$20-AA9)-AG9,AB10*2-AG9)),
        //IF(IF(AB10=0,0,IF(Z10="Falla",AB10*2*(AA10-AA9)/($D$20-AA9)-AG9,AB10*2-AG9))>$AD$3,$AD$3,
        //IF(AB10=0,0,IF(Z10="Falla",AB10*2*(AA10-AA9)/($D$20-AA9)-AG9,AB10*2-AG9))))

        for (int i = bloque + 1; i < 96; i++)
        {
            if (i == bloqueFalla)
            {
                //Formula 7: IF(AB8=0,0,FORM6)
                if (medidor96[i] == 0)
                {
                    valorCorregido[i] = 0;
                }
                else
                {
                    //IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7)
                    if (i == bloqueFalla)
                    {
                        valorCorregido[i] = medidor96[i] * 2 * 15 / (fechaFalla.Minute % 15) - valorCorregido[i - 1];
                    }
                    else
                    {
                        valorCorregido[i] = medidor96[i] * 2 - valorCorregido[i - 1];
                    }
                }

            }
            else
            {

                //Formula 1: IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7)
                decimal form1 = 0;
                if (i == bloqueFalla)
                {
                    form1 = medidor96[i] * 2 * 15 / (fechaFalla.Minute % 15) - valorCorregido[i - 1];
                }
                else
                {
                    form1 = medidor96[i] * 2 - valorCorregido[i - 1];
                }

                //Formula 2: IF(AB8=0,0,FORM1)
                decimal form2 = medidor96[i] == 0 ? 0 : form1;

                //Formula 3: IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7)
                decimal form3 = form1;

                //Formula 4: IF(AB8=0,0,FORM3)
                decimal form4 = medidor96[i] == 0 ? 0 : form3;

                //Fomula 5: IF(FORM4>$AD$3,$AD$3,FORM2)
                decimal form5 = form4 > potenciaLimite ? potenciaLimite : form2;

                valorCorregido[i] = form5;

            }
        }

        return valorCorregido;
    }

    private decimal[] CalcularValorCorregido(decimal[] medidor96, DateTime tiempoSincroniz, int bloqueFalla, DateTime fechaFalla, decimal potenciaLimite)
    {

        decimal[] valorCorregido = new decimal[96];

        for (int i = 0; i < 96; i++)
        {
            valorCorregido[i] = 0;
        }

        //celda 1
        //=IF(AB7=0,0,AB7*2*(AA7-AA6)/(K7-H6))
        int bloque = 4 * tiempoSincroniz.Hour + tiempoSincroniz.Minute / 15;

        /*valorCorregido[bloque] = medidor96[bloque] == 0
            ? 0
            : medidor96[bloque] * 2 * 15 / (30 - tiempoSincroniz.Minute);
        */

        decimal cociente = 30 - tiempoSincroniz.Minute;
        if (cociente == 0)
        {
            cociente = 15;
        }

        valorCorregido[bloque] = medidor96[bloque] == 0
            ? 0
            : medidor96[bloque] * 2 * 15 / (cociente);

        //=IF(Z10="Falla",IF(AB10=0,0,IF(Z10="Falla",AB10*2*(AA10-AA9)/($D$20-AA9)-AG9,AB10*2-AG9)),
        //IF(IF(AB10=0,0,IF(Z10="Falla",AB10*2*(AA10-AA9)/($D$20-AA9)-AG9,AB10*2-AG9))>$AD$3,$AD$3,
        //IF(AB10=0,0,IF(Z10="Falla",AB10*2*(AA10-AA9)/($D$20-AA9)-AG9,AB10*2-AG9))))

        //for (int i = bloque + 1; i < 96; i++)
        for (int i = bloque + 1; i < 96; i++)
        {
            if (i == bloqueFalla)
            {
                //Formula 7: IF(AB8=0,0,FORM6)
                if (medidor96[i] == 0)
                {
                    valorCorregido[i] = 0;
                }
                else
                {
                    //IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7)
                    /*
                    if (i == bloqueFalla)
                    {
                        valorCorregido[i] = medidor96[i] * 2 * 15 / (fechaFalla.Minute % 15) - valorCorregido[i - 1];
                    }
                    else
                    {
                        valorCorregido[i] = medidor96[i] * 2 - valorCorregido[i - 1];
                    }
                    */
                    valorCorregido[i] = medidor96[i] * 2;

                }

            }
            else
            {

                //Formula 1: IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7)
                decimal form1 = 0;
                if (i == bloqueFalla)
                {
                    if (fechaFalla.Minute != 0)
                    {
                        form1 = medidor96[i] * 2 * 15 / (fechaFalla.Minute % 15) - valorCorregido[i - 1];
                    }
                    else
                    {
                        form1 = medidor96[i] * 2 - valorCorregido[i - 1];
                    }
                }
                else
                {
                    form1 = medidor96[i] * 2 - valorCorregido[i - 1];
                }

                //Formula 2: IF(AB8=0,0,FORM1)
                decimal form2 = medidor96[i] == 0 ? 0 : form1;

                //Formula 3: IF(Z8="Falla",AB8*2*(AA8-AA7)/($D$20-AA7)-AG7,AB8*2-AG7)
                decimal form3 = form1;

                //Formula 4: IF(AB8=0,0,FORM3)
                decimal form4 = medidor96[i] == 0 ? 0 : form3;

                //Fomula 5: IF(FORM4>$AD$3,$AD$3,FORM2)
                decimal form5 = form4 > potenciaLimite ? potenciaLimite : form2;

                valorCorregido[i] = form5;

            }
        }

        return valorCorregido;
    }


    /// <summary>
    /// Permite calcular la potencia límite
    /// </summary>
    /// <param name="medidor96">Datos de 15 minutos</param>
    /// <returns></returns>
    private decimal CalculaPotenciaLimite(decimal[] medidor96)
    {
        decimal maximo = -1000;

        for (int i = 0; i < 96; i++)
        {
            maximo = Math.Max(maximo, medidor96[i]);

        }

        return maximo;
    }


    /// <summary>
    /// Permite obtener el bloque de falla de 15 minutos
    /// </summary>
    /// <param name="fecha"></param>
    /// <returns></returns>
    private int ObtenerBloqueFalla96(DateTime? fecha)
    {
        int bloque = -1;

        if (fecha == null)
            bloque = -1;
        else
        {
            bloque = 4 * fecha.Value.Hour + fecha.Value.Minute / 15;
        }

        return bloque;
    }


}


public class ManttoResultado
{
    int[] minutoMant = new int[1441];
    decimal[] minutoProgramado = new decimal[1440];
    decimal[] minutoEjecutado = new decimal[1440];

    decimal[] minutoRestOpBase = new decimal[1440];
    decimal[] minutoPotPromMed = new decimal[1440];
    decimal[] potenciaConsigna = new decimal[1440];

    public ManttoResultado()
    {
        for (int i = 0; i < 1440; i++)
        {
            minutoMant[i] = 0;
            minutoProgramado[i] = 0;
            minutoEjecutado[i] = 0;

            minutoRestOpBase[i] = 0;
            minutoPotPromMed[i] = 0;
            potenciaConsigna[i] = 0;

        }

        minutoMant[1440] = 999;
    }

    public void AsignaMantoProgramado(EveIeodcuadroDTO manto, int codProg, decimal valorProg)
    {
        int minIni = 60 * manto.Ichorini.Value.Hour + manto.Ichorini.Value.Minute;
        int minFin = minIni;

        if (manto.Ichorini.Value.Day != manto.Ichorfin.Value.Day)
        {
            minFin = 1440;
        }
        else
        {
            minFin = 60 * manto.Ichorfin.Value.Hour + manto.Ichorfin.Value.Minute;
        }

        for (int i = minIni; i < minFin; i++)
        {
            minutoMant[i] = codProg;
            minutoProgramado[i] = valorProg;
        }

    }

    public void AsignaMantoEjecutadoProgramado(EveIeodcuadroDTO manto, int codEjec, int codEjecProg, decimal valorEjec)
    {

        int minIni = 60 * manto.Ichorini.Value.Hour + manto.Ichorini.Value.Minute;
        int minFin = minIni;

        if (manto.Ichorini.Value.Day != manto.Ichorfin.Value.Day)
        {
            minFin = 1440;
        }
        else
        {
            minFin = 60 * manto.Ichorfin.Value.Hour + manto.Ichorfin.Value.Minute;
        }

        for (int i = minIni; i < minFin; i++)
        {
            switch (minutoMant[i])
            {
                //no asignado
                case 0:
                    minutoMant[i] = codEjec;
                    break;
                //programado
                case 1:
                    minutoMant[i] = codEjecProg;
                    break;
                //puede ser ejecutado
                default:
                    minutoMant[i] = codEjec;
                    break;
            }

            minutoEjecutado[i] = valorEjec;
        }

    }

    public void AsignaRestrixOperaBase(EveIeodcuadroDTO manto, decimal valorEjec)
    {

        int minIni = 60 * manto.Ichorini.Value.Hour + manto.Ichorini.Value.Minute;
        int minFin = minIni;

        if (manto.Ichorini.Value.Day != manto.Ichorfin.Value.Day)
        {
            minFin = 1440;
        }
        else
        {
            minFin = 60 * manto.Ichorfin.Value.Hour + manto.Ichorfin.Value.Minute;
        }

        for (int i = minIni; i < minFin; i++)
        {

            minutoRestOpBase[i] = valorEjec;
        }

    }


    public void AsignaConsignaOperaBase(EveIeodcuadroDTO manto, decimal potenciaConsigna2, decimal valorPotPromMedidor)
    {

        int minIni = 60 * manto.Ichorini.Value.Hour + manto.Ichorini.Value.Minute;
        int minFin = minIni;

        if (manto.Ichorini.Value.Day != manto.Ichorfin.Value.Day)
        {
            minFin = 1440;
        }
        else
        {
            minFin = 60 * manto.Ichorfin.Value.Hour + manto.Ichorfin.Value.Minute;
        }

        for (int i = minIni; i < minFin; i++)
        {
            potenciaConsigna[i] = potenciaConsigna2;
            minutoPotPromMed[i] = valorPotPromMedidor;
        }

    }


    public decimal[] ObtenerMatrizDesdeRestriccConsigna()
    {
        decimal[] minutoEjecutado2 = new decimal[1440];

        for (int i = 0; i < 1440; i++)
        {
            if (potenciaConsigna[i] > 0)
            {
                if (minutoRestOpBase[i] > 0)
                {
                    minutoEjecutado2[i] = minutoRestOpBase[i];
                }
                else
                {
                    minutoEjecutado2[i] = minutoPotPromMed[i];
                }
            }
        }

        return minutoEjecutado2;

    }

    public void AsignaMantoEjecutadoProgramado(decimal[] manto2, int codEjec, int codEjecProg)
    {

        for (int i = 0; i < 1440; i++)
        {
            switch (minutoMant[i])
            {
                //no asignado
                case 0:
                    minutoMant[i] = codEjec;
                    break;
                //programado
                case 1:
                    minutoMant[i] = codEjecProg;
                    break;
                //puede ser ejecutado
                default:
                    minutoMant[i] = codEjec;
                    break;
            }

            //la potencia consigna debe ser igual a limite dado por el programado
            if (potenciaConsigna[i] == minutoProgramado[i])
            {
                minutoEjecutado[i] = manto2[i];
            }
            //minutoReferenciaEjecutado = ;
        }

    }


    public double HoraIndispTotalProgramada()
    {
        return HoraPorMantenimiento(3);
    }

    public double HoraIndispTotalFortuita()
    {
        return HoraPorMantenimiento(2);
    }

    public double HoraPorMantenimiento(int cod)
    {
        int hora = 0;
        string cad = "";
        for (int i = 0; i < 1440; i++)
        {
            hora += (minutoMant[i] == cod ? 1 : 0);
            cad += "[" + i + "]=" + minutoMant[i] + "\r\n";
        }

        return hora / 60.0;
    }

    public List<EveIeodcuadroDTO> ListaIndispTotalProgramada(DateTime fechaBase)
    {
        return ObtenerTramo(3, fechaBase);
    }

    public List<EveIeodcuadroDTO> ListaIndispTotalFortuita(DateTime fechaBase)
    {
        return ObtenerTramo(2, fechaBase);
    }

    public List<EveIeodcuadroDTO> ListaReducidaIndispTotalProgramada(DateTime fechaBase)
    {
        return ReducirTramo(3, fechaBase);
    }

    public List<EveIeodcuadroDTO> ListaReducidaIndispTotalFortuita(DateTime fechaBase)
    {
        return ReducirTramo(2, fechaBase);
    }




    public List<EveIeodcuadroDTO> ObtenerTramo(int cod, DateTime fechaBase)
    {
        //cod:  3 programado y ejecutado
        //      2 ejecutado

        bool inicioTramo = false;
        List<EveIeodcuadroDTO> lista = new List<EveIeodcuadroDTO>();
        int bloqueini = -1;
        int bloquefin = -1;
        int bloqueNew = -1;
        int bloqueOld = -1;

        /*
        int primerBloque = 0;

        for (int i = 0; i <= 1440; i++)
        {
            if (minutoMant[i] == cod)
            {
                inicioTramo = true;
                primerBloque = i;
                break;
            }
        }
        */

        for (int i = 0; i <= 1440; i++)
        {
            bloqueNew = minutoMant[i];

            if (minutoMant[i] == cod)
            {
                if (!inicioTramo)
                {
                    inicioTramo = true;
                    bloqueini = i;

                    //valorAnterior=()
                }
                else
                {


                }


            }
            else
            {
                string fechaCadena = "";

                if (bloqueOld == cod)
                {
                    if (inicioTramo)
                    {
                        bloquefin = i;

                        EveIeodcuadroDTO m = new EveIeodcuadroDTO();

                        int hora = bloqueini / 60;
                        int min = bloqueini - 60 * hora;
                        fechaCadena = fechaBase.ToString("yyyy-MM-dd");
                        m.Ichorini = Convert.ToDateTime(fechaCadena + " " + hora + ":" + min + ":00");
                        //m.Evenini = Convert.ToDateTime("2017-01-01").AddSeconds(bloqueini);


                        //m.Evenfin = Convert.ToDateTime("2017-01-01").AddSeconds(i);

                        if (i != 1440)
                        {
                            int hora2 = i / 60;
                            int min2 = i - 60 * hora2;
                            fechaCadena = fechaBase.ToString("yyyy-MM-dd");
                            m.Ichorfin = Convert.ToDateTime(fechaCadena + " " + hora2 + ":" + min2 + ":00");
                        }
                        else
                        {
                            fechaCadena = fechaBase.AddDays(1).ToString("yyyy-MM-dd");
                            m.Ichorfin = Convert.ToDateTime(fechaCadena + " 00:00:00");
                        }


                        lista.Add(m);
                        inicioTramo = false;
                    }
                }
            }

            bloqueOld = bloqueNew;
        }


        return lista;
    }


    public List<EveIeodcuadroDTO> ReducirTramo(int cod, DateTime fechaBase)
    {
        //cod:  3 programado y ejecutado
        //      2 ejecutado

        List<EveIeodcuadroDTO> lista = new List<EveIeodcuadroDTO>();
        int bloqueini = -1;

        string fechaCadena = "";

        //int[] SegundoOriginal = new int[1441];

        for (int i = 0; i < 1440; i++)
        {
            if (minutoMant[i] == cod)
            {
                int hora = i / 60;
                int min = i - 60 * hora;
                fechaCadena = fechaBase.ToString("yyyy-MM-dd");

                EveIeodcuadroDTO m = new EveIeodcuadroDTO();
                m.Ichorini = Convert.ToDateTime(fechaCadena + " " + hora + ":" + min + ":00");
                m.Ichorfin = m.Ichorini.Value.AddMinutes(1);
                m.Icvalor1 = minutoProgramado[i];
                m.Icvalor2 = minutoEjecutado[i];

                lista.Add(m);
            }

        }

        bool cambio = true;

        while (cambio)
        {
            cambio = false;
            for (int j = lista.Count - 1; j >= 1; j--)
            {
                if ((lista[j].Icvalor1 == lista[j - 1].Icvalor1) &&
                    (lista[j].Icvalor2 == lista[j - 1].Icvalor2) &&
                    (lista[j].Ichorini == lista[j - 1].Ichorfin)
                    )
                {
                    lista[j - 1].Ichorfin = lista[j].Ichorfin;
                    lista.RemoveAt(j);
                    cambio = true;
                    break;
                }

            }

        }


        return lista;
    }


}

