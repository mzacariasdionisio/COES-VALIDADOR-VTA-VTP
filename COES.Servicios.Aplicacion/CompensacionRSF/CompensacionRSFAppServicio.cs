using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Equipamiento;
using System.Data;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using System.Globalization;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Titularidad;

namespace COES.Servicios.Aplicacion.CompensacionRSF
{
    /// <summary>
    /// Clases con métodos del módulo CompensacionRSF
    /// </summary>
    public class CompensacionRSFAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CompensacionRSFAppServicio));

        CostoOportunidadAppServicio servicioCostOport = new CostoOportunidadAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        BarraUrsAppServicio servicioBarraUrs = new BarraUrsAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        CostoMarginalAppServicio servicioCostoMarginal = new CostoMarginalAppServicio();
        CentralGeneracionAppServicio servicioCentral = new CentralGeneracionAppServicio();
        DespachoAppServicio wsDespacho = new DespachoAppServicio();
        ConsultaMedidoresAppServicio servicioConsultaMedidores = new ConsultaMedidoresAppServicio();
        FormatoMedicionAppServicio servicioFormatoMedicion = new FormatoMedicionAppServicio();

        #region Métodos Tabla VCR_ASIGNACIONPAGO

        /// <summary>
        /// Inserta un registro de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        public void SaveVcrAsignacionpago(VcrAsignacionpagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionpagoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        public void UpdateVcrAsignacionpago(VcrAsignacionpagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionpagoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrAsignacionpago(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionpagoRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        public VcrAsignacionpagoDTO GetByIdVcrAsignacionpago(int vcrapcodi)
        {
            return FactoryTransferencia.GetVcrAsignacionpagoRepository().GetById(vcrapcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrAsignacionpagoDTO> ListVcrAsignacionpagos(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrAsignacionpagoRepository().List(vcrecacodi);
        }

        /// <summary>
        /// Permite listar a las empresas de una revisión de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrAsignacionpagoDTO> ListVcrAsignacionpagosEmpresaMes(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrAsignacionpagoRepository().ListEmpresaMes(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrAsignacionpago
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="emprcodi">Idemtificador de la empresa</param>
        /// <param name="equicodicen">Identificador de la central de generación</param>
        /// <param name="equicodiuni">Identificador de la unidad de generación</param>
        public List<VcrAsignacionpagoDTO> GetByCriteriaVcrAsignacionpagos(int vcrecacodi, int emprcodi, int equicodicen, int equicodiuni)
        {
            return FactoryTransferencia.GetVcrAsignacionpagoRepository().GetByCriteria(vcrecacodi, emprcodi, equicodicen, equicodiuni);
        }

        /// <summary>
        /// Permite obtener el total de Asignacion de Pago para una Empresa, Central y Unidad en un mes de la tabla VCR_ASIGNACIONPAGO
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public VcrAsignacionpagoDTO GetByIdVcrAsignacionpagoMesUnidad(int vcrecacodi, int equicodiuni)
        {
            return FactoryTransferencia.GetVcrAsignacionpagoRepository().GetByIdMesUnidad(vcrecacodi, equicodiuni);
        }

        #endregion

        #region Métodos Tabla VCR_ASIGNACIONRESERVA

        /// <summary>
        /// Inserta un registro de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        public void SaveVcrAsignacionreserva(VcrAsignacionreservaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionreservaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        public void UpdateVcrAsignacionreserva(VcrAsignacionreservaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionreservaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrAsignacionreserva(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionreservaRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        public VcrAsignacionreservaDTO GetByIdVcrAsignacionreserva(int vcrarcodi)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().GetById(vcrarcodi);
        }

        /// <summary>
        /// Permite obtener el max(Vcrarmpa) : dMPA_id de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        public decimal GetByVcrAsignacionreservadMPA2020(int vcrecacodi, DateTime dDia)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().GetBydMPA2020(vcrecacodi, dDia);
        }

        /// <summary>
        /// Permite obtener un objeto con el max(Vcrarmpa) subida : dMPA_id y mas(iVcrarmpabajar) bajada de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        public VcrAsignacionreservaDTO GetByVcrAsignacionreservadMPA(int vcrecacodi, DateTime dDia)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().GetBydMPA(vcrecacodi, dDia);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="emprcodi">Identificador de la Empresa en SI_EMPRESA</param>
        public VcrAsignacionreservaDTO GetByIdVcrAsignacionreservaEmpresa(int vcrecacodi, int emprcodi)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().GetByIdEmpresa(vcrecacodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_ASIGNACIONRESERVA
        /// </summary>
        public List<VcrAsignacionreservaDTO> ListVcrAsignacionreservas()
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrAsignacionreserva
        /// </summary>
        public List<VcrAsignacionreservaDTO> GetByCriteriaVcrAsignacionreservas()
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_ASIGNACIONRESERVA por versión y urs
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código del urs</param>
        public List<VcrAsignacionreservaDTO> ListVcrAsignacionreservasPorMesURS(int vcrecacodi, int grupocodi)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().ListPorMesURS(vcrecacodi, grupocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_ASIGNACIONRESERVA por versión y dia
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="fecha">Fecha</param>
        public List<VcrAsignacionreservaDTO> GetByCriteriaVcrAsignacionReservaOferta(int vcrecacodi, DateTime fecha)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().GetByCriteriaVcrAsignacionReservaOferta(vcrecacodi, fecha);
        }


        #endregion

        #region Métodos Tabla VCR_CARGOINCUMPL

        /// <summary>
        /// Inserta un registro de la tabla VCR_CARGOINCUMPL
        /// </summary>
        public void SaveVcrCargoincumpl(VcrCargoincumplDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCargoincumplRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_CARGOINCUMPL
        /// </summary>
        public void UpdateVcrCargoincumpl(VcrCargoincumplDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCargoincumplRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_CARGOINCUMPL
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrCargoincumpl(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrCargoincumplRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_CARGOINCUMPL
        /// </summary>
        public VcrCargoincumplDTO GetByIdVcrCargoincumpl(int vcrecacodi, int equicodi)
        {
            return FactoryTransferencia.GetVcrCargoincumplRepository().GetById(vcrecacodi, equicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_CARGOINCUMPL
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrCargoincumplDTO> ListVcrCargoincumpls(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrCargoincumplRepository().List(vcrecacodi);
        }

        /// <summary>
        /// Permite Calcular los cargos por Incumplimiento x Grupo
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrCargoincumplDTO> ListVcrCargoIncumplGrupoCalculado(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrCargoincumplRepository().ListCargoIncumplGrupoCalculado(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrCargoincumpl
        /// </summary>
        public List<VcrCargoincumplDTO> GetByCriteriaVcrCargoincumpls()
        {
            return FactoryTransferencia.GetVcrCargoincumplRepository().GetByCriteria();
        }

        /// <summary>
        /// Todos los Grupos (Centrales) con obligación de prestar el servicio de RPF que operaron en el mes “n” con cargo de incumplimiento que tienen chek
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public decimal TotalMesServicioRSFConsiderados(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrCargoincumplRepository().TotalMesServicioRSFConsiderados(vcrecacodi);
        }
        #endregion

        #region Métodos Tabla VCR_CMPENSOPER

        /// <summary>
        /// Inserta un registro de la tabla VCR_CMPENSOPER
        /// </summary>
        public void SaveVcrCmpensoper(VcrCmpensoperDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCmpensoperRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_CMPENSOPER
        /// </summary>
        public void UpdateVcrCmpensoper(VcrCmpensoperDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCmpensoperRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_CMPENSOPER
        /// </summary>
        public void DeleteVcrCmpensoper(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrCmpensoperRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_CMPENSOPER
        /// </summary>
        public VcrCmpensoperDTO GetByIdVcrCmpensoper(int vcmpopcodi)
        {
            return FactoryTransferencia.GetVcrCmpensoperRepository().GetById(vcmpopcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_CMPENSOPER
        /// </summary>
        public List<VcrCmpensoperDTO> ListVcrCmpensopers(int vcrrecacodi)
        {
            return FactoryTransferencia.GetVcrCmpensoperRepository().List(vcrrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrCmpensoper
        /// </summary>
        public List<VcrCmpensoperDTO> GetByCriteriaVcrCmpensopers()
        {
            return FactoryTransferencia.GetVcrCmpensoperRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite exportar un archivo excel
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public string GenerarFormatoVcrCompOp(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            List<TrnBarraursDTO> listaUrs = this.servicioBarraUrs.ListURS();
            VcrRecalculoDTO entidadRecalculo = this.GetByIdVcrRecalculo(vcrecacodi);
            List<VcrCmpensoperDTO> ListaCompensacion = this.ListVcrCmpensopers(vcrecacodi);


            if (formato == 1)
            {
                fileName = "ReporteCompensacionOp.xlsx";
                ExcelDocument.GenerarFormatoVcrComOp(pathFile + fileName, listaUrs, entidadRecalculo, ListaCompensacion);
            }

            return fileName;
        }

        #endregion

        #region Métodos Tabla VCR_COSTOPORTDETALLE

        /// <summary>
        /// Inserta un registro de la tabla VCR_COSTOPORTDETALLE
        /// </summary>
        public void SaveVcrCostoportdetalle(VcrCostoportdetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCostoportdetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_COSTOPORTDETALLE
        /// </summary>
        public void UpdateVcrCostoportdetalle(VcrCostoportdetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCostoportdetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_COSTOPORTDETALLE
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrCostoportdetalle(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrCostoportdetalleRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_COSTOPORTDETALLE
        /// </summary>
        public VcrCostoportdetalleDTO GetByIdVcrCostoportdetalle(int vcrcopcodi)
        {
            return FactoryTransferencia.GetVcrCostoportdetalleRepository().GetById(vcrcopcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_COSTOPORTDETALLE
        /// </summary>
        public List<VcrCostoportdetalleDTO> ListVcrCostoportdetalles()
        {
            return FactoryTransferencia.GetVcrCostoportdetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VCR_COSTOPORTDETALLE
        /// </summary>
        public List<VcrCostoportdetalleDTO> GetByCriteriaVcrCostoportdetalles()
        {
            return FactoryTransferencia.GetVcrCostoportdetalleRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_COSTOPORTDETALLE por revisión, urs y equicodi
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código de la URS</param>
        /// <param name="equicodi">Código de la Unidad de generación</param>
        public List<VcrCostoportdetalleDTO> ListVcrCostoportdetallesPorMesURS(int vcrecacodi, int grupocodi, int equicodi)
        {
            return FactoryTransferencia.GetVcrCostoportdetalleRepository().ListPorMesURS(vcrecacodi, grupocodi, equicodi);
        }

        #endregion

        #region Métodos Tabla VCR_COSTOPORTUNIDAD

        /// <summary>
        /// Inserta un registro de la tabla VCR_COSTOPORTUNIDAD
        /// </summary>
        public void SaveVcrCostoportunidad(VcrCostoportunidadDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCostoportunidadRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_COSTOPORTUNIDAD
        /// </summary>
        public void UpdateVcrCostoportunidad(VcrCostoportunidadDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCostoportunidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_COSTOPORTUNIDAD
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrCostoportunidad(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrCostoportunidadRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_COSTOPORTUNIDAD
        /// </summary>
        public VcrCostoportunidadDTO GetByIdVcrCostoportunidad(int vcrcopcodi)
        {
            return FactoryTransferencia.GetVcrCostoportunidadRepository().GetById(vcrcopcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_COSTOPORTUNIDAD totalizado por empresa
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="emprcodi">Identificador de la Empresa en SI_EMPRESA</param>
        public VcrCostoportunidadDTO GetByIdVcrCostoportunidadEmpresa(int vcrecacodi, int emprcodi)
        {
            return FactoryTransferencia.GetVcrCostoportunidadRepository().GetByIdEmpresa(vcrecacodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_COSTOPORTUNIDAD
        /// </summary>
        public List<VcrCostoportunidadDTO> ListVcrCostoportunidads()
        {
            return FactoryTransferencia.GetVcrCostoportunidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrCostoportunidad
        /// </summary>
        public List<VcrCostoportunidadDTO> GetByCriteriaVcrCostoportunidads()
        {
            return FactoryTransferencia.GetVcrCostoportunidadRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VCR_COSTVARIABLE

        /// <summary>
        /// Inserta un registro de la tabla VCR_COSTVARIABLE
        /// </summary>
        public void SaveVcrCostvariable(VcrCostvariableDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCostvariableRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_COSTVARIABLE
        /// </summary>
        public void UpdateVcrCostvariable(VcrCostvariableDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrCostvariableRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_COSTVARIABLE
        /// </summary>
        public void DeleteVcrCostvariable(int vcvarcodi)
        {
            try
            {
                FactoryTransferencia.GetVcrCostvariableRepository().Delete(vcvarcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_COSTVARIABLE
        /// </summary>
        public VcrCostvariableDTO GetByIdVcrCostvariable(int vcvarcodi)
        {
            return FactoryTransferencia.GetVcrCostvariableRepository().GetById(vcvarcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_COSTVARIABLE
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="GrupoCodi">Código del URS de la tabla PR_GRUPO</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="vcvarfecha">Dia de consulta del costo variable</param>
        public List<VcrCostvariableDTO> ListVcrCostvariables(int vcrecacodi, int grupocodi, int equicodi, DateTime vcvarfecha)
        {
            return FactoryTransferencia.GetVcrCostvariableRepository().List(vcrecacodi, grupocodi, equicodi, vcvarfecha);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrCostvariable
        /// </summary>
        public List<VcrCostvariableDTO> GetByCriteriaVcrCostvariables(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrCostvariableRepository().GetByCriteria(vcrecacodi);
        }

        /// <summary>
        /// Permite exportar un archivo excel
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public string GenerarFormatoVcrCostVar(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            List<TrnBarraursDTO> listaUrs = this.servicioBarraUrs.ListURS();
            VcrRecalculoDTO entidadRecalculo = this.GetByIdVcrRecalculo(vcrecacodi);
            List<VcrCostvariableDTO> listaCostoVariable = this.GetByCriteriaVcrCostvariables(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteCostoVariable.xlsx";
                ExcelDocument.GenerarFormatoVcrCostVar(pathFile + fileName, listaUrs, entidadRecalculo, listaCostoVariable);
            }

            return fileName;
        }

        #endregion

        #region Métodos Tabla VCR_DESPACHOURS

        /// <summary>
        /// Inserta un registro de la tabla VCR_DESPACHOURS
        /// </summary>
        public void SaveVcrDespachours(VcrDespachoursDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrDespachoursRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_DESPACHOURS
        /// </summary>
        public void UpdateVcrDespachours(VcrDespachoursDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrDespachoursRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VCR_DESPACHOURS filtrado por vcrecacodi
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrDespachours(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrDespachoursRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_DESPACHOURS
        /// </summary>
        public VcrDespachoursDTO GetByIdVcrDespachours(int vcdurscodi)
        {
            return FactoryTransferencia.GetVcrDespachoursRepository().GetById(vcdurscodi);
        }

        /// <summary>
        /// Permite listar todos las Unidades de Generación de la tabla VCR_DESPACHOURS, filtrado por recalculo, urs y tipo
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="GrupoCodi">Código del URS de la tabla PR_GRUPO</param>
        /// <param name="Vcdurstipo">Tipo de despacho S: sin / C:con asignacion de reserva</param>
        public List<VcrDespachoursDTO> ListVcrDespachoursUnidadByUrsTipo(int vcrecacodi, int GrupoCodi, string Vcdurstipo)
        {
            return FactoryTransferencia.GetVcrDespachoursRepository().ListUnidadByUrsTipo(vcrecacodi, GrupoCodi, Vcdurstipo);
        }


        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_DESPACHOURS, filtrado por recalculo, urs, unidad, tipo y dia
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="GrupoCodi">Código del URS de la tabla PR_GRUPO</param>
        /// <param name="Equicodi">Identificador de la tabla EQ_EQUIPO</param>
        /// <param name="Vcdurstipo">Tipo de despacho S: sin / C:con asignacion de reserva</param>
        /// <param name="dFecha">Dia de consulta</param>
        public List<VcrDespachoursDTO> ListVcrDespachoursByUrsUnidadTipoDia(int vcrecacodi, int grupocodi, int equicodi, string vcdurstipo, DateTime vcdursfecha)
        {
            return FactoryTransferencia.GetVcrDespachoursRepository().ListByUrsUnidadTipoDia(vcrecacodi, grupocodi, equicodi, vcdurstipo, vcdursfecha);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrDespachours
        /// </summary>
        public List<VcrDespachoursDTO> GetByCriteriaVcrDespachourss()
        {
            return FactoryTransferencia.GetVcrDespachoursRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba la información de la base de datos de los Despachos de las URS
        /// </summary>
        public void GrabarDespacho(int vcrecacodi, string sUser, List<EveRsfdetalleDTO> listaReserEjec, List<MeMedicion48DTO> listaDespacho, List<MeMedicion48DTO> listaDespachoSin, List<MeMedicion48DTO> listaReserProg, List<MeMedicion48DTO> listaCruceReserva, DateTime fecha, int tipoReserva)
        {
            try
            {
                //Se esta copiando tal cual todo el procedimiento del archivo COES.Servicios.Aplicacion.CostoOportunidad.CostoOportunidadAppServicio:
                //GeneraTablaExcelDespacho
                List<EveRsfdetalleDTO> cabecera = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracionCO();
                int ancho = cabecera.Count();
                string sVcdurstipo = "";
                if (tipoReserva == 0) //Sin Reserva
                {
                    //titulo = "DESPACHO SIN ASIGNACIÓN DE RESERVA (MW)";
                    sVcdurstipo = "S";
                }
                else //Con Reserva
                {
                    //titulo = "DESPACHO CON ASIGNACIÓN DE RESERVA (MW)";
                    sVcdurstipo = "C";
                }

                int j = 0;
                decimal reservaPeriodo = 0;
                List<MeMedicion48DTO> listaReservaEjec = new List<MeMedicion48DTO>();
                listaCruceReserva = servicioCostOport.GetListaCruce(listaReserEjec, listaReserProg, listaReservaEjec, fecha);

                int minuto = -30; //Empieza de menos 30 para que el primer registro sea de 00:00 y asi se sincronice con el costo variable

                for (int k = 1; k <= 48; k++)
                {   //PARA CADA INTERVALO DE 30 MINUTOS
                    minuto = minuto + 30;
                    j = 0;
                    DateTime dVcdursfecha = fecha.AddMinutes(minuto);
                    foreach (EveRsfdetalleDTO config in cabecera)
                    {   //PARA CADA DE URS DE LA LISTA cabecera
                        //SI_EMPRESA
                        string sEmprnomb = config.Emprnomb;
                        COES.Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = new COES.Dominio.DTO.Transferencias.EmpresaDTO();
                        dtoEmpresa = this.servicioEmpresa.GetByNombre(config.Emprnomb);
                        if (dtoEmpresa == null)
                        {
                            //model.sError += "<br>El siguiente URS no existe: " + dtoOferta.Ursnomb;
                            continue;
                            //return Json(model);
                        }
                        int iEmprcodi = dtoEmpresa.EmprCodi;
                        //EQ_EQUIPO
                        //CentralGeneracionDTO dtoCentral = new CentralGeneracionDTO();
                        //dtoCentral = this.servicioCentral.GetByCentGeneTermoelectricaNombre(config.Gruponomb);
                        //if (dtoCentral == null)
                        //{
                        //    //model.sError += "<br>El siguiente URS no existe: " + dtoOferta.Ursnomb;
                        //    continue;
                        //    //return Json(model);
                        //}
                        int iEquicodi = (int)config.Equicodi;

                        int iGrupocodi = (int)config.Grupocodi;
                        string sGruponomb = (string)config.Ursnomb;

                        j++;
                        var regFind = listaCruceReserva.Find(x => x.Grupocodi == config.Grupocodi);
                        var regReserva = listaReservaEjec.Find(x => x.Grupocodi == config.Grupocodi);

                        decimal despachoValor = 0;
                        decimal despachoValorSin = 0;
                        /// Se busca Despacho

                        MeMedicion48DTO entity = listaDespacho.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                        if (entity != null)
                        {
                            despachoValor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        }
                        /// Se busca DespachiSin
                        entity = listaDespachoSin.Where(x => x.Grupourspadre == config.Grupocodi).FirstOrDefault();
                        if (entity != null && entity.GetType().GetProperty("H" + k).GetValue(entity, null) != null)
                        {
                            despachoValorSin = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                        }
                        ///
                        if (regReserva != null)
                        {
                            var reserPeriodo = (decimal?)regReserva.GetType().GetProperty("H" + k.ToString()).GetValue(regReserva, null);
                            if (reserPeriodo != null)
                            {
                                reservaPeriodo = (decimal)reserPeriodo;
                            }
                        }
                        decimal dVcdUrsDespacho = 0;
                        decimal valor;
                        if (regFind != null)
                        {
                            /// Calcular Despacho para intervalo
                            var reservaProg = (decimal?)regFind.GetType().GetProperty("H" + k.ToString()).GetValue(regFind, null);
                            if (reservaProg != null)
                            {
                                if (tipoReserva == 1)
                                {
                                    valor = CalcularCeldaDespacho(fecha, k, (int)config.Grupocodi, (int)reservaProg, despachoValor, reservaPeriodo, despachoValorSin);
                                }
                                else
                                {
                                    valor = despachoValor;
                                }
                                dVcdUrsDespacho = valor;
                            }
                            else
                            {
                                dVcdUrsDespacho = despachoValor;
                            }

                        }
                        else
                        {
                            // Copiar Despacho
                            dVcdUrsDespacho = despachoValor;
                        }
                        //INSERTAMOS EL REGISTRO DE DESPACHO
                        VcrDespachoursDTO dtoDespachoURS = new VcrDespachoursDTO();
                        dtoDespachoURS.Vcrecacodi = vcrecacodi;
                        dtoDespachoURS.Vcdurstipo = sVcdurstipo;
                        dtoDespachoURS.Grupocodi = iGrupocodi;
                        dtoDespachoURS.Gruponomb = sGruponomb;
                        dtoDespachoURS.Equicodi = iEquicodi;
                        dtoDespachoURS.Emprcodi = iEmprcodi;
                        dtoDespachoURS.Vcdursfecha = dVcdursfecha;
                        dtoDespachoURS.Vcdursdespacho = dVcdUrsDespacho;
                        dtoDespachoURS.Vcdursusucreacion = sUser;
                        this.SaveVcrDespachours(dtoDespachoURS);
                    }
                }
            }
            catch (Exception e)
            {
                string sError = e.Message;
            }
        }

        /// <summary>
        /// Calcula Valor final de celda con diferencia de reserva ejecutado vs programado
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="fila"></param>
        /// <param name="urscodi"></param>
        /// <param name="minutos"></param>
        /// <param name="despacho"></param>
        /// <param name="reserva"></param>
        /// <param name="rsfPorc"></param>
        /// <returns></returns>
        private decimal CalcularCeldaDespacho(DateTime fecha, int fila, int urscodi, int minutos, decimal despacho, decimal reserva, decimal despachoSin)
        {
            //Se esta copiando tal cual todo el procedimiento del archivo COES.Servicios.Aplicacion.CostoOportunidad.CostoOportunidadAppServicio:
            decimal valor = 0;
            decimal valorA = 0;
            decimal valorB = 0;
            decimal valorC = 0;
            decimal potRef = 0;
            decimal porcentajeRpf = 0;
            decimal potMinima = 0;
            decimal potEfectiva = 0;
            int modo = 0;
            fecha = fecha.AddMinutes(fila * 30);
            var lista = FactorySic.GetEveHoraoperacionRepository().GetHorasURS(fecha, fecha.AddDays(1)).Where(x => x.Grupourspadre == urscodi);
            foreach (var reg in lista)
            {
                if (reg.Hophorini <= fecha && reg.Hophorfin >= fecha)
                {
                    modo = (int)reg.Grupocodi;
                    break;
                }
            }
            var regbanda = FactorySic.GetCoBandancpRepository().GetByCriteria(fecha, modo).FirstOrDefault();
            var listaParametros = wsDespacho.ObtenerDatosMO_URS(modo, fecha);
            var oGrupo = FactorySic.GetPrGrupoRepository().GetById(modo);
            string tipo = oGrupo.Grupotipo;

            if (listaParametros.Count > 0)
            {
                porcentajeRpf = 0;
                potEfectiva = 0;
                potMinima = 0;
                for (int j = 0; j < listaParametros.Count; j++)
                {
                    if (tipo == "T")
                    {
                        switch (listaParametros[j].Concepcodi)
                        {
                            case ConstantesCostoOportunidad.PotenciaEfectiva:
                                potEfectiva = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) : ConstantesAppServicio.ErrorPotMax;
                                break;
                            case ConstantesCostoOportunidad.PotenciaMinima:
                                potMinima = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) : ConstantesAppServicio.ErrorPotMin;
                                break;
                            case ConstantesCostoOportunidad.PorcentajeRpf:
                                porcentajeRpf = this.AnalizarNumerico(listaParametros[j].Formuladat) ? Convert.ToDecimal(listaParametros[j].Formuladat) / 100 : ConstantesAppServicio.ErrorPorcRPF;
                                break;
                        }
                    }
                }
            }
            // calcular potencia de referencia
            if (regbanda != null)
            {
                valorA = despachoSin;
                potRef = (decimal)regbanda.Bandmin;
                valorB = (potRef + reserva) / (1 - porcentajeRpf);
                potRef = (decimal)regbanda.Bandmax;
                valorC = (potRef - reserva) / (1 + porcentajeRpf);
            }
            if (valorA < valorB)
                valor = valorB;
            if (valorA > valorC)
                valor = valorC;
            if (valorA > valorB && valorA < valorC)
                valor = valorA;
            // calcular valor final
            valor = (decimal)minutos / 30 * valor;
            return valor;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_DESPACHOURS
        /// </summary>
        public List<VcrDespachoursDTO> ListVcrDespachourss(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrDespachoursRepository().List(vcrecacodi);
        }

        #endregion

        #region Métodos Tabla VCR_EMPRESARSF

        /// <summary>
        /// Inserta un registro de la tabla VCR_EMPRESARSF
        /// </summary>
        public void SaveVcrEmpresarsf(VcrEmpresarsfDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrEmpresarsfRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_EMPRESARSF
        /// </summary>
        public void UpdateVcrEmpresarsf(VcrEmpresarsfDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrEmpresarsfRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_EMPRESARSF
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrEmpresarsf(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrEmpresarsfRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_EMPRESARSF
        /// </summary>
        public VcrEmpresarsfDTO GetByIdVcrEmpresarsf(int vcersfcodi)
        {
            return FactoryTransferencia.GetVcrEmpresarsfRepository().GetById(vcersfcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_EMPRESARSF
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrEmpresarsfDTO> ListVcrEmpresarsfs(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrEmpresarsfRepository().List(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrEmpresarsf
        /// </summary>
        public List<VcrEmpresarsfDTO> GetByCriteriaVcrEmpresarsfs()
        {
            return FactoryTransferencia.GetVcrEmpresarsfRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener un registro con los valores calculados de la tabla VCR_EMPRESARSF por recalculo y empresa
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public VcrEmpresarsfDTO GetByIdVcrEmpresarsfTotalMes(int vcrecacodi, int emprcodi)
        {
            return FactoryTransferencia.GetVcrEmpresarsfRepository().GetByIdTotalMes(vcrecacodi, emprcodi);
        }


        #endregion

        #region Métodos Tabla VCR_MEDBORNE

        /// <summary>
        /// Inserta un registro de la tabla VCR_MEDBORNE
        /// </summary>
        public void SaveVcrMedborne(VcrMedborneDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrMedborneRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta registros de la tabla VCR_MEDBORNE
        /// </summary>
        public void SaveVcrMedborneBulk(List<VcrMedborneDTO> entitys)
        {
            try
            {
                FactoryTransferencia.GetVcrMedborneRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_MEDBORNE
        /// </summary>
        public void UpdateVcrMedborne(VcrMedborneDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrMedborneRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_MEDBORNE
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void DeleteVcrMedborne(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrMedborneRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_MEDBORNE
        /// </summary>
        public VcrMedborneDTO GetByIdVcrMedborne(int vcrmebcodi)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().GetById(vcrmebcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_MEDBORNE
        /// </summary>
        public List<VcrMedborneDTO> ListVcrMedbornes()
        {
            return FactoryTransferencia.GetVcrMedborneRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrMedborne
        /// </summary>
        public List<VcrMedborneDTO> GetByCriteriaVcrMedbornes()
        {
            return FactoryTransferencia.GetVcrMedborneRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba la información de la base de datos de los Medidores de Bornes de Generación
        /// </summary>
        public int GrabarMedidorBorne(int vcrecacodi, string sUser, int tipoInfoCodi, string empresas, int central, string tiposGeneracion, DateTime fecInicio, DateTime fecFin)
        {
            int iNumReg = 0;
            string tiposEmpresa = "1,2,3,4,5";
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = servicioConsultaMedidores.ListarEmpresasRsfBorne(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            int lectcodi = 1;
            List<MeMedicion96DTO> listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInfoCodi
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);

            foreach (MeMedicion96DTO item in listActiva)
            {
                VcrMedborneDTO dtoMedborne = new VcrMedborneDTO();
                dtoMedborne.Vcrecacodi = vcrecacodi;
                string sNombre;

                int iEnelGreen = 11395;
                if (item.Emprcodi == iEnelGreen)
                    dtoMedborne.Emprcodi = 13783; //ENEL GREEN POWER PERU S.A. [B] -> ENEL GREEN POWER PERU S.A.C [A] //sNombre = item.Emprnomb;
                else
                    dtoMedborne.Emprcodi = item.Emprcodi;


                if (item.Equipadre == 0)
                {
                    sNombre = item.Central;
                    CentralGeneracionDTO dtoCentral = servicioCentral.GetByCentGeneNomb(sNombre);
                    if (dtoCentral != null)
                        dtoMedborne.Equicodicen = dtoCentral.CentGeneCodi;
                    else
                        sNombre = "No existe";
                }
                else
                    dtoMedborne.Equicodicen = item.Equipadre; //item.Central
                if (item.Equicodi == 0)
                {
                    sNombre = item.Equinomb;
                    CentralGeneracionDTO dtoUnidad = servicioCentral.GetByCentGeneNomb(sNombre);
                    if (dtoUnidad != null)
                        dtoMedborne.Equicodiuni = dtoUnidad.CentGeneCodi;
                    else
                        sNombre = "No existe";
                }
                else
                    dtoMedborne.Equicodiuni = item.Equicodi; //item.Equinomb

                dtoMedborne.Vcrmebfecha = (DateTime)item.Medifecha;
                dtoMedborne.Vcrmebptomed = item.Ptomedicodi.ToString(); //Identificador de la tabla PtoMedicion
                decimal dVcrMebPotenciaMededia = 0;
                for (int k = 1; k <= 96; k++)
                {
                    object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                    if (resultado != null)
                    {
                        dVcrMebPotenciaMededia += Convert.ToDecimal(resultado);
                    }
                }
                dtoMedborne.Vcrmebpotenciamed = dVcrMebPotenciaMededia / 4; //  (sum(96) Interv de Potencia / 4) = TOTAL ENERGIA ACTIVA  (expresado en MWh)
                //202012
                dtoMedborne.Vcrmebpotenciamedgrp = dVcrMebPotenciaMededia / 96;  // (sum(96) Interv de Potencia / 96) = POTENCIA MEDIA DE GRUPO  (expresado en potencia)
                dtoMedborne.Vcrmebpresencia = 0;
                if (dtoMedborne.Vcrmebpotenciamedgrp > 0)
                {
                    dtoMedborne.Vcrmebpresencia = 1;
                }
                dtoMedborne.Vcrmebusucreacion = sUser;
                this.SaveVcrMedborne(dtoMedborne);
                iNumReg++;
            }

            //GRANDES USUARIOS - ASSETEC 27/11/2019
            this.GrabarGrandesUsuarios(vcrecacodi, fecInicio, sUser);

            return iNumReg;
        }

        /// <summary>
        /// Permite grabar Unidades considerados en el cargo del incumplimiento
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="sUser">User del usuario logueado</param>
        public void GrabarUnidadesCargoIncumplimiento(int vcrecacodi, string sUser)
        {
            try
            {
                List<VcrMedbornecargoincpDTO> listaMBCargoIncp = new List<VcrMedbornecargoincpDTO>();
                while (listaMBCargoIncp.Count == 0)
                {
                    int vcrecacodiant = vcrecacodi - 1;
                    listaMBCargoIncp = this.ListVcrMedbornecargoincps(vcrecacodiant);
                }

                List<VcrMedborneDTO> listaMedBorne = this.ListVcrMedbornesDistintos(vcrecacodi);
                foreach (VcrMedborneDTO dtoMedborne in listaMedBorne)
                {


                    //unidades SI/NO considerados en el cargo del incumplimiento en la revisión antenrior
                    List<VcrMedbornecargoincpDTO> dtoMBCargoIncp = listaMBCargoIncp.FindAll(x => x.Emprcodi == dtoMedborne.Emprcodi);
                    dtoMBCargoIncp = dtoMBCargoIncp.FindAll(x => x.Equicodicen == dtoMedborne.Equicodicen);
                    dtoMBCargoIncp = dtoMBCargoIncp.FindAll(x => x.Equicodiuni == dtoMedborne.Equicodiuni);
                    string sVcmbciconsiderar = "S";
                    if (dtoMBCargoIncp.Count > 0)
                    {
                        sVcmbciconsiderar = dtoMBCargoIncp[0].Vcmbciconsiderar;
                    }

                    if (dtoMedborne.Emprcodi < 0)
                    {
                        sVcmbciconsiderar = "N";
                    }
                    //Grabando las unidades considerados en el cargo del incumplimiento - CU07, paso 2
                    VcrMedbornecargoincpDTO dtoMedbornecargoincp = new VcrMedbornecargoincpDTO();
                    dtoMedbornecargoincp.Vcrecacodi = vcrecacodi;
                    dtoMedbornecargoincp.Emprcodi = dtoMedborne.Emprcodi;
                    dtoMedbornecargoincp.Equicodicen = dtoMedborne.Equicodicen;
                    dtoMedbornecargoincp.Equicodiuni = dtoMedborne.Equicodiuni;
                    dtoMedbornecargoincp.Vcmbciconsiderar = sVcmbciconsiderar; // "S";
                    dtoMedbornecargoincp.Vcmbciusucreacion = sUser;

                    this.SaveVcrMedbornecargoincp(dtoMedbornecargoincp);

                    //Grabando las Unidades exoneradas – CU08
                    VcrUnidadexoneradaDTO dtoUnidadexonerada = new VcrUnidadexoneradaDTO();
                    dtoUnidadexonerada.Vcrecacodi = vcrecacodi;
                    dtoUnidadexonerada.Emprcodi = dtoMedborne.Emprcodi;
                    dtoUnidadexonerada.Equicodicen = dtoMedborne.Equicodicen;
                    dtoUnidadexonerada.Equicodiuni = dtoMedborne.Equicodiuni;
                    dtoUnidadexonerada.Vcruexonerar = "N";
                    dtoUnidadexonerada.Vcruexobservacion = "";
                    dtoUnidadexonerada.Vcruexusucreacion = sUser;

                    this.SaveVcrUnidadexonerada(dtoUnidadexonerada);
                }
            }
            catch (Exception e)
            {
                string sError = e.Message; //"-1"
                sError = e.StackTrace;
            }
        }


        /// <summary>
        /// Permite grabar el borne para los Grandes usuarios
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="fecInicio">Fecha de inicio</param>
        /// <param name="sUser">User del usuario logueado</param>
        public void GrabarGrandesUsuarios(int vcrecacodi, DateTime fecInicio, string sUser)
        {

            List<TrnInfoadicionalDTO> listaConceptos = new TransferenciasAppServicio().ListTrnInfoadicionals();

            foreach (TrnInfoadicionalDTO pConcepto in listaConceptos)
            {
                if (pConcepto.Infadicodi == -1001)
                {
                    continue;
                }

                int genemprcodi = pConcepto.Infadicodi;
                int periodo = this.GetByIdVcrRecalculo(vcrecacodi).Pericodi;
                int recacodi = this.GetByIdVcrRecalculo(vcrecacodi).Recacodi;

                //obteniendo id para consultar el detalle
                List<TransferenciaRetiroDetalleDTO> retiroDetalleList = new List<TransferenciaRetiroDetalleDTO>();
                List<TransferenciaRetiroDTO> retiroList = new List<TransferenciaRetiroDTO>();

                //trae las lista retiro
                retiroList = FactoryTransferencia.GetTransferenciaRetiroRepository().GetRetiroBy(periodo, recacodi, genemprcodi);
                //Recorre la lista de retiro

                if (retiroList.Count != 0)
                {
                    foreach (var item in retiroList)
                    {
                        retiroDetalleList = new List<TransferenciaRetiroDetalleDTO>();
                        //trae la lista detalle de retiro
                        retiroDetalleList = FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().ListByTransferenciaRetiroDay(item.TranRetiCodi);
                        //Contador para aumentar los dias al grabar.
                        int c = 0;

                        if (retiroDetalleList.Count != 0)
                        {
                            foreach (var detalle in retiroDetalleList)
                            {
                                //setearemos los datos a ggrabr en la entidad
                                VcrMedborneDTO borne = new VcrMedborneDTO();

                                borne.Vcrecacodi = vcrecacodi;
                                borne.Emprcodi = genemprcodi;
                                borne.Equicodicen = -1;
                                borne.Equicodiuni = -1;
                                borne.Vcrmebfecha = fecInicio.AddDays(c);
                                borne.Vcrmebptomed = "-1"; //Identificador de la tabla PtoMedicion
                                borne.Vcrmebpotenciamed = detalle.TranRetiDetaSumaDia; //(TOTAL ENERGIA ACTIVA  (MWh))
                                                                                       //202012
                                borne.Vcrmebpotenciamedgrp = detalle.TranRetiDetaSumaDia / 24;  // POTENCIA MEDIA DE GRUPO  (expresado en potencia)
                                borne.Vcrmebpresencia = 0;
                                if (borne.Vcrmebpotenciamedgrp > 0 && borne.Emprcodi > 0)
                                {
                                    borne.Vcrmebpresencia = 1;
                                }
                                //---------------------------------------------
                                borne.Vcrmebusucreacion = sUser;
                                c++;
                                //Save
                                this.SaveVcrMedborne(borne);
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Permite listar todos las Empresas, Centrales y Unidades de la tabla VCR_MEDBORNE
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrMedborneDTO> ListVcrMedbornesDistintos(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().ListDistintos(vcrecacodi);
        }

        /// <summary>
        /// Permite listar todas las Empresas, Centrales y Unidades de la tabla VCR_MEDBORNE de un Dia, exceptuando las unidades exoneradas del Pago de RSF
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="vcrmebfecha">Dia de consulta</param>
        public List<VcrMedborneDTO> ListVcrMedbornesDiaSinUnidExonRSF(int vcrecacodi, DateTime vcrmebfecha)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().ListDiaSinUnidExonRSF(vcrecacodi, vcrmebfecha);
        }

        public List<VcrMedborneDTO> ListMesConsideradosTotales(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().ListMesConsideradosTotales(vcrecacodi);
        }


        /// <summary>
        /// Permite listar todas las Empresas, Centrales y Unidades de la tabla VCR_MEDBORNE totalizando el mes de vcrmebpotenciamed
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrMedborneDTO> ListVcrMedbornesMes(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().ListMes(vcrecacodi);
        }

        /// <summary>
        /// Permite listar a las Empresas, Centrales y Unidades Consideradas en el pago de RSF de la tabla VCR_MEDBORNE totalizando el mes de vcrmebpotenciamed y con el atributo Considera = SI
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrMedborneDTO> ListVcrMedbornesMesConsiderados(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().ListMesConsiderados(vcrecacodi);
        }

        /// <summary>
        /// Producción mensual de energía activa del Grupo Unidades en Medición de Bornes, cargo de incumplimiento que tienen chek.
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public decimal TotalUnidNoExonRSF(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrMedborneRepository().TotalUnidNoExonRSF(vcrecacodi);
        }
        #endregion

        #region Métodos Tabla VCR_MEDBORNECARGOINCP

        /// <summary>
        /// Inserta un registro de la tabla VCR_MEDBORNECARGOINCP
        /// </summary>
        public void SaveVcrMedbornecargoincp(VcrMedbornecargoincpDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrMedbornecargoincpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_MEDBORNECARGOINCP
        /// </summary>
        public void UpdateVcrMedbornecargoincp(VcrMedbornecargoincpDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrMedbornecargoincpRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza una lista de registro de la tabla VCR_MEDBORNECARGOINCP NO considerados
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void UpdateVcrMedbornecargoincpVersionNO(int vcrecacodi, string sUser)
        {
            try
            {
                FactoryTransferencia.GetVcrMedbornecargoincpRepository().UpdateVersionNO(vcrecacodi, sUser);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza una lista de registro de la tabla VCR_MEDBORNECARGOINCP SI considerados
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void UpdateVcrMedbornecargoincpVersionSI(int vcrecacodi, string sUser, int vcmbcicodi)
        {
            try
            {
                FactoryTransferencia.GetVcrMedbornecargoincpRepository().UpdateVersionSI(vcrecacodi, sUser, vcmbcicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_MEDBORNECARGOINCP
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void DeleteVcrMedbornecargoincp(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrMedbornecargoincpRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_MEDBORNECARGOINCP
        /// </summary>
        public VcrMedbornecargoincpDTO GetByIdVcrMedbornecargoincp(int vcmbcicodi)
        {
            return FactoryTransferencia.GetVcrMedbornecargoincpRepository().GetById(vcmbcicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_MEDBORNECARGOINCP incluido los Nombres de Empresa, Central y Unidad
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public List<VcrMedbornecargoincpDTO> ListVcrMedbornecargoincps(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrMedbornecargoincpRepository().List(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrMedbornecargoincp
        /// </summary>
        public List<VcrMedbornecargoincpDTO> GetByCriteriaVcrMedbornecargoincps()
        {
            return FactoryTransferencia.GetVcrMedbornecargoincpRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VCR_OFERTA

        /// <summary>
        /// Inserta un registro de la tabla VCR_OFERTA
        /// </summary>
        public void SaveVcrOferta(VcrOfertaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrOfertaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_OFERTA
        /// </summary>
        public void UpdateVcrOferta(VcrOfertaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrOfertaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_OFERTA por revision
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void DeleteVcrOferta(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrOfertaRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_OFERTA
        /// </summary>
        public VcrOfertaDTO GetByIdVcrOferta(int vcrofecodi)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetById(vcrofecodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_OFERTA
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código del URS en la tala PR_GRUPO</param>
        /// <param name="dFecha">Dia de consulta</param>
        public VcrOfertaDTO GetByIdVcrOfertaMaxDia(int vcrecacodi, DateTime dFecha)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByIdMaxDia(vcrecacodi, dFecha);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_OFERTA
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código del URS en la tala PR_GRUPO</param>
        /// <param name="dFecha">Dia de consulta</param>
        public VcrOfertaDTO GetByIdVcrOfertaMaxDiaGrupoCodi(int vcrecacodi, int grupocodi, DateTime dFecha)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByIdMaxDiaGrupoCodi(vcrecacodi, grupocodi, dFecha);
        }

        /// <summary>
        /// Permite obtener el precio de la tabla VCR_OFERTA para una versión/urs/dia/intervalos Hi y Hf
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código del URS en la tala PR_GRUPO</param>
        /// <param name="dFecha">Dia de consulta</param>
        /// <param name="dHoraInicio">Hora Inicio de la Reserva Asignada</param>
        /// <param name="dHoraFinal">Hora Final de la Reserva Asignada</param>
        /// <param name="vcrofetipocarga">Tipo de Carga: Subida-> 1 / Bajada-> 2</param>
        public decimal GetOfertaMaxDiaGrupoCodiHiHf(int vcrecacodi, int grupocodi, DateTime dFecha, DateTime dHoraInicio, DateTime dHoraFinal, int vcrofetipocarga)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetOfertaMaxDiaGrupoCodiHiHf(vcrecacodi, grupocodi, dFecha, dHoraInicio, dHoraFinal, vcrofetipocarga);
        }

        /// <summary>
        /// Permite obtener el precio de la tabla VCR_OFERTA para una versión/urs/dia/intervalos Hi y Hf
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código del URS en la tala PR_GRUPO</param>
        /// <param name="dFecha">Dia de consulta</param>
        /// <param name="dHoraInicio">Hora Inicio de la Reserva Asignada</param>
        /// <param name="dHoraFinal">Hora Final de la Reserva Asignada</param>
        /// <param name="vcrofetipocarga">Tipo de Carga: Subida-> 1 / Bajada-> 2</param>
        public decimal GetOfertaMaxDiaGrupoCodiHiHf2020(int vcrecacodi, int grupocodi, DateTime dFecha, DateTime dHoraInicio, DateTime dHoraFinal, int vcrofetipocarga)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetOfertaMaxDiaGrupoCodiHiHf2020(vcrecacodi, grupocodi, dFecha, dHoraInicio, dHoraFinal, vcrofetipocarga);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_OFERTA
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="dFecha">Dia de consulta</param>
        public VcrOfertaDTO GetByIdVcrOfertaMaxDiaUrs(int vcrecacodi, DateTime dFecha)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByIdMaxDiaUrs(vcrecacodi, dFecha);
        }


        /// <summary>
        /// Permite obtener un registro de la tabla VCR_OFERTA
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código del URS en la tala PR_GRUPO</param>
        /// <param name="dFecha">Dia de consulta</param>
        public VcrOfertaDTO GetByIdVcrOfertaMaxMes(int vcrecacodi, DateTime dFecha)//, int grupocodi
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByIdMaxMes(vcrecacodi, dFecha); //, grupocodi
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_OFERTA
        /// </summary>
        public List<VcrOfertaDTO> ListVcrOfertas()
        {
            return FactoryTransferencia.GetVcrOfertaRepository().List();
        }
        //ASSETEC 20190115
        /// <summary>
        /// Permite listar todos los registros sin duplicados de la tabla VCR_OFERTA
        /// </summary>
        public List<VcrOfertaDTO> ListVcrOfertasSinDuplicados(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().ListSinDuplicados(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrOferta y obtener un solo registro
        /// </summary>
        public VcrOfertaDTO GetByCriteriaVcrOferta(int vcrecacodi, int grupocodi, DateTime vcrofefecha, string vcrofecodigoenv, DateTime vcrofehorinicio, int vcrofetipocarga)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByCriteriaOferta(vcrecacodi, grupocodi, vcrofefecha, vcrofecodigoenv, vcrofehorinicio, vcrofetipocarga);
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrOferta
        /// </summary>
        public List<VcrOfertaDTO> GetByCriteriaVcrOfertas()
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla FW_USER a partir del nombre de usuario
        /// </summary>
        public VcrOfertaDTO GetByFwUserByNombre(string Username)
        {
            return FactoryTransferencia.GetVcrOfertaRepository().GetByFwUserByNombre(Username);
        }

        #endregion

        #region Métodos Tabla VCR_PAGORSF

        /// <summary>
        /// Inserta un registro de la tabla VCR_PAGORSF
        /// </summary>
        public void SaveVcrPagorsf(VcrPagorsfDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrPagorsfRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_PAGORSF
        /// </summary>
        public void UpdateVcrPagorsf(VcrPagorsfDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrPagorsfRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_PAGORSF
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrPagorsf(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrPagorsfRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_PAGORSF
        /// </summary>
        public VcrPagorsfDTO GetByIdVcrPagorsf(int vcprsfcodi)
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().GetById(vcprsfcodi);
        }

        /// <summary>
        /// Permite obtener el calculo del Pagos por RSF para periodos anteriores al 2020.12
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        public VcrPagorsfDTO GetByIdVcrPagorsfUnidad2020(int vcrecacodi, int equicodi)
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().GetByIdUnidad2020(vcrecacodi, equicodi);
        }

        /// <summary>
        /// Permite obtener el calculo del Pagos por RSF para periodos posteriores al 2021.01
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        public VcrPagorsfDTO GetByIdVcrPagorsfUnidad(int vcrecacodi, int equicodi)
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().GetByIdUnidad(vcrecacodi, equicodi);
        }

        /// <summary>
        /// Permite obtener el calculo del Pagos por RSF para periodos posteriores al 2021.01
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO</param>
        public VcrPagorsfDTO GetByIdVcrPagorsfConcepto(int vcrecacodi, int equicodi, int empresa)
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().GetByIdUnidadPorEmpresa(vcrecacodi, equicodi, empresa);
        }

        /// <summary>
        /// Permite obtener el calculo del Pagos por RSF para calcular diferencia en calculo para empresas para migracion de empresas de tipo TRANSFERENCia dE EQuIPOS 
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public VcrPagorsfDTO GetByMigracionEquiposPorEmpresaOrigenxDestino(int vcrecacodi, int emprcodiorigen, int emprcodidestino)
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().GetByMigracionEquiposPorEmpresaOrigenxDestino(vcrecacodi, emprcodiorigen,emprcodidestino);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_PAGORSF
        /// </summary>
        public List<VcrPagorsfDTO> ListVcrPagorsfs()
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrPagorsf
        /// </summary>
        public List<VcrPagorsfDTO> GetByCriteriaVcrPagorsfs()
        {
            return FactoryTransferencia.GetVcrPagorsfRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VCR_PROVISIONBASE

        /// <summary>
        /// Inserta un registro de la tabla VCR_PROVISIONBASE
        /// </summary>
        public void SaveVcrProvisionbase(VcrProvisionbaseDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrProvisionbaseRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_PROVISIONBASE
        /// </summary>
        public void UpdateVcrProvisionbase(VcrProvisionbaseDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrProvisionbaseRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_PROVISIONBASE
        /// </summary>
        public void DeleteVcrProvisionbase(int vcrpbcodi)
        {
            try
            {
                FactoryTransferencia.GetVcrProvisionbaseRepository().Delete(vcrpbcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_PROVISIONBASE
        /// </summary>
        public VcrProvisionbaseDTO GetByIdVcrProvisionbase(int vcrpbcodi)
        {
            return FactoryTransferencia.GetVcrProvisionbaseRepository().GetById(vcrpbcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_PROVISIONBASE
        /// </summary>
        public List<VcrProvisionbaseDTO> ListVcrProvisionbases()
        {
            return FactoryTransferencia.GetVcrProvisionbaseRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrProvisionbase
        /// </summary>
        public List<VcrProvisionbaseDTO> GetByCriteriaVcrProvisionbases()
        {
            return FactoryTransferencia.GetVcrProvisionbaseRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_PROVISIONBASE
        /// </summary>
        /// <param name="grupocodi">Identificador de la Tabla PR_GRUPO</param>
        /// <param name="sPeriodo">Formato YYYYMM</param>
        public VcrProvisionbaseDTO GetByIdVcrProvisionbaseURS(int grupocodi, string sPeriodo)
        {
            return FactoryTransferencia.GetVcrProvisionbaseRepository().GetByIdURS(grupocodi, sPeriodo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_PROVISIONBASE incluye Gruponomb
        /// </summary>
        public List<VcrProvisionbaseDTO> ListVcrProvisionbasesIndex()
        {
            return FactoryTransferencia.GetVcrProvisionbaseRepository().ListIndex();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_PROVISIONBASE para mostrarlo en pop-up
        /// </summary>
        public VcrProvisionbaseDTO GetByIdVcrProvisionbaseView(int vcrpbcodi)
        {
            return FactoryTransferencia.GetVcrProvisionbaseRepository().GetByIdView(vcrpbcodi);
        }

        #endregion

        #region Métodos Tabla VCR_RECALCULO

        /// <summary>
        /// Inserta un registro de la tabla VCR_RECALCULO
        /// </summary>
        public void SaveVcrRecalculo(VcrRecalculoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrRecalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_RECALCULO
        /// </summary>
        public void UpdateVcrRecalculo(VcrRecalculoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrRecalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_RECALCULO por periodo y recalculo para UpDate
        /// </summary>
        public VcrRecalculoDTO GetByIdVcrRecalculoUpDate(int pericodi, int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().GetByIdUpDate(pericodi, vcrecacodi);
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_RECALCULO
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void DeleteVcrRecalculo(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrAsignacionpagoRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrAsignacionreservaRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrCargoincumplRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrCmpensoperRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrCostoportdetalleRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrCostoportunidadRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrCostvariableRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrDespachoursRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrEmpresarsfRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrMedborneRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrMedbornecargoincpRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrOfertaRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrPagorsfRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrReduccpagoejeRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrReservasignRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrServiciorsfRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrTermsuperavitRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrUnidadexoneradaRepository().Delete(vcrecacodi);
                FactoryTransferencia.GetVcrRecalculoRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_RECALCULO
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public VcrRecalculoDTO GetByIdVcrRecalculo(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().GetById(vcrecacodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_RECALCULO
        /// <param name="vcrinccodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public VcrRecalculoDTO GetByIncumplimiento(int vcrinccodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().GetByIncumplimiento(vcrinccodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_RECALCULO por periodo y recalculo
        /// </summary>
        public VcrRecalculoDTO GetByIdVcrRecalculoView(int pericodi, int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().GetByIdView(pericodi, vcrecacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_RECALCULO
        /// </summary>
        public List<VcrRecalculoDTO> ListVcrRecalculos(int pericodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().List(pericodi);
        }

        /// <summary>
        /// Permite realizar registro en la tabla VcrRecalculo
        /// </summary>
        public List<VcrRecalculoDTO> ListVcrRecalculosReg(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().ListReg(vcrecacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_RECALCULO sin parametro
        /// </summary>
        public List<VcrRecalculoDTO> ListVcrTodoRecalculos()
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().ListAllRecalculos();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrRecalculo
        /// </summary>
        public List<VcrRecalculoDTO> GetByCriteriaVcrRecalculos()
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_RECALCULO por periodo y recalculo y mostrarlo en ventana
        /// </summary>
        public VcrRecalculoDTO GetByIdVcrRecalculoViewIndex(int vcrecacodi, int pericodi)
        {
            return FactoryTransferencia.GetVcrRecalculoRepository().GetByIdViewIndex(vcrecacodi, pericodi);
        }

        #endregion

        #region Métodos Tabla VCR_REDUCCPAGOEJE

        /// <summary>
        /// Inserta un registro de la tabla VCR_REDUCCPAGOEJE
        /// </summary>
        public void SaveVcrReduccpagoeje(VcrReduccpagoejeDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrReduccpagoejeRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_REDUCCPAGOEJE
        /// </summary>
        public void UpdateVcrReduccpagoeje(VcrReduccpagoejeDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrReduccpagoejeRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_REDUCCPAGOEJE
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public void DeleteVcrReduccpagoeje(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrReduccpagoejeRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_REDUCCPAGOEJE
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="equicodi">Identificador de la tabla EQ_EQUIPO, para la unidad</param>
        public VcrReduccpagoejeDTO GetByIdVcrReduccpagoeje(int vcrecacodi, int equicodi)
        {
            return FactoryTransferencia.GetVcrReduccpagoejeRepository().GetById(vcrecacodi, equicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_REDUCCPAGOEJE
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        public List<VcrReduccpagoejeDTO> ListVcrReduccpagoejes(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrReduccpagoejeRepository().List(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrReduccpagoeje
        /// </summary>
        public List<VcrReduccpagoejeDTO> GetByCriteriaVcrReduccpagoejes()
        {
            return FactoryTransferencia.GetVcrReduccpagoejeRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VCR_RESERVASIGN

        /// <summary>
        /// Inserta un registro de la tabla VCR_RESERVASIGN
        /// </summary>
        public void SaveVcrReservasign(VcrReservasignDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrReservasignRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_RESERVASIGN
        /// </summary>
        public void UpdateVcrReservasign(VcrReservasignDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrReservasignRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_RESERVASIGN
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void DeleteVcrReservasign(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrReservasignRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_RESERVASIGN
        /// </summary>
        public VcrReservasignDTO GetByIdVcrReservasign(int vcrasgcodi)
        {
            return FactoryTransferencia.GetVcrReservasignRepository().GetById(vcrasgcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_RESERVASIGN
        /// </summary>
        public List<VcrReservasignDTO> ListVcrReservasigns()
        {
            return FactoryTransferencia.GetVcrReservasignRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrReservasign
        /// </summary>
        public List<VcrReservasignDTO> GetByCriteriaVcrReservasigns()
        {
            return FactoryTransferencia.GetVcrReservasignRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrReservasign por una URS en un dia
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Identificador de la tabla pr_grupo</param>
        /// <param name="dFecha">un dia</param>
        private List<VcrReservasignDTO> GetByCriteriaVcrReservasignsURSDia(int vcrecacodi, int grupocodi, DateTime dFecha)
        {
            return FactoryTransferencia.GetVcrReservasignRepository().GetByCriteriaURSDia(vcrecacodi, grupocodi, dFecha);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrReservasign por una URS en un dia
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="dFecha">un dia</param>
        private List<VcrReservasignDTO> GetByCriteriaVcrReservasignsDia(int vcrecacodi, DateTime dFecha)
        {
            return FactoryTransferencia.GetVcrReservasignRepository().GetByCriteriaDia(vcrecacodi, dFecha);
        }

        /// <summary>
        /// Permite Obtener la informacíón de la Reserva Asignada de SGOCOES/Eventos/Unid. Regulación Secundaria para el periodo 2021.01 en adelante
        /// </summary>
        public List<VcrEveRsfhoraDTO> ObtenerReservaAsignada(DateTime fecInicio, DateTime fecFin)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().ExportarReservaAsignadaSEV(fecInicio, fecFin);
        }

        /// <summary>
        /// Permite Obtener la informacíón de la Reserva Asignada de SGOCOES/Eventos/Unid. Regulación Secundaria para periodos anteriores al 2021.01
        /// </summary>
        public List<EveRsfhoraDTO> ObtenerReservaAsignada2020(DateTime fecInicio, DateTime fecFin)
        {
            return FactoryTransferencia.GetVcrAsignacionreservaRepository().ExportarReservaAsignadaSEV2020(fecInicio, fecFin);
        }

        /// <summary>
        /// Permite exportar los datos del SEV segun rango de fechas para antes del periodo 2021.01
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha fin</param>
        /// <param name="pathFile">Ruta donde se va alojar el archivo</param>
        /// <param name="fileName">Nombre del archivo</param>
        public void GenerarReporteReservaAsignadaSEV2020(DateTime fechaInicio, DateTime fechaFin, string pathFile, string fileName)
        {
            ExcelDocument.GenerarReporteReservaAsignadaSEV2020(pathFile + fileName, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite exportar los datos del SEV segun rango de fechas para información despues del 2021.01
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha fin</param>
        /// <param name="pathFile">Ruta donde se va alojar el archivo</param>
        /// <param name="fileName">Nombre del archivo</param>
        public void GenerarReporteReservaAsignadaSEV(DateTime fechaInicio, DateTime fechaFin, string pathFile, string fileName)
        {
            ExcelDocument.GenerarReporteReservaAsignadaSEV(pathFile + fileName, fechaInicio, fechaFin);
        }
        #endregion

        #region Métodos Tabla VCR_SERVICIORSF

        /// <summary>
        /// Inserta un registro de la tabla VCR_SERVICIORSF
        /// </summary>
        public void SaveVcrServiciorsf(VcrServiciorsfDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrServiciorsfRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_SERVICIORSF
        /// </summary>
        public void UpdateVcrServiciorsf(VcrServiciorsfDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrServiciorsfRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_SERVICIORSF
        /// </summary>
        /// <param name="vcrecacodi">Version de calculo</param>
        public void DeleteVcrServiciorsf(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrServiciorsfRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_SERVICIORSF
        /// </summary>
        public VcrServiciorsfDTO GetByIdVcrServiciorsf(int vcsrsfcodi)
        {
            return FactoryTransferencia.GetVcrServiciorsfRepository().GetById(vcsrsfcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_SERVICIORSF con los valores de un dia del Costo total del Servicio RSF 
        /// </summary>
        /// <param name="vcrecacodi">Version de calculo</param>
        /// <param name="vcsrsffecha">fecha de un dia</param>
        public VcrServiciorsfDTO GetByIdVcrServiciorsfValoresDia(int vcrecacodi, DateTime vcsrsffecha)
        {
            return FactoryTransferencia.GetVcrServiciorsfRepository().GetByIdValoresDia(vcrecacodi, vcsrsffecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_SERVICIORSF
        /// </summary>
        /// <param name="vcrecacodi">Version de calculo</param>
        public List<VcrServiciorsfDTO> ListVcrServiciorsfs(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrServiciorsfRepository().List(vcrecacodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrServiciorsf
        /// </summary>
        public List<VcrServiciorsfDTO> GetByCriteriaVcrServiciorsfs()
        {
            return FactoryTransferencia.GetVcrServiciorsfRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VCR_TERMSUPERAVIT

        /// <summary>
        /// Inserta un registro de la tabla VCR_TERMSUPERAVIT
        /// </summary>
        public void SaveVcrTermsuperavit(VcrTermsuperavitDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrTermsuperavitRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_TERMSUPERAVIT
        /// </summary>
        public void UpdateVcrTermsuperavit(VcrTermsuperavitDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrTermsuperavitRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_TERMSUPERAVIT
        /// </summary>
        /// <param name="vcrecacodi">Version de calculo</param>
        public void DeleteVcrTermsuperavit(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrTermsuperavitRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_TERMSUPERAVIT
        /// </summary>
        public VcrTermsuperavitDTO GetByIdVcrTermsuperavit(int vcrtscodi)
        {
            return FactoryTransferencia.GetVcrTermsuperavitRepository().GetById(vcrtscodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_TERMSUPERAVIT, por revisióngrupo y dia
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código de la URS</param>
        /// <param name="vcrtsfecha">Fecha de consulta</param>
        public VcrTermsuperavitDTO GetByIdVcrTermsuperavitDia(int vcrecacodi, int grupocodi, DateTime vcrtsfecha)
        {
            return FactoryTransferencia.GetVcrTermsuperavitRepository().GetByIdDia(vcrecacodi, grupocodi, vcrtsfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_TERMSUPERAVIT
        /// </summary>
        public List<VcrTermsuperavitDTO> ListVcrTermsuperavits()
        {
            return FactoryTransferencia.GetVcrTermsuperavitRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrTermsuperavit
        /// </summary>
        public List<VcrTermsuperavitDTO> GetByCriteriaVcrTermsuperavits()
        {
            return FactoryTransferencia.GetVcrTermsuperavitRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_TERMSUPERAVIT por revisión, urs y equicodi
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="grupocodi">Código de la URS</param>
        public List<VcrTermsuperavitDTO> ListVcrTermsuperavitsPorMesURS(int vcrecacodi, int grupocodi)
        {
            return FactoryTransferencia.GetVcrTermsuperavitRepository().ListPorMesURS(vcrecacodi, grupocodi);
        }

        #endregion

        #region Métodos Tabla VCR_UNIDADEXONERADA

        /// <summary>
        /// Inserta un registro de la tabla VCR_UNIDADEXONERADA
        /// </summary>
        public void SaveVcrUnidadexonerada(VcrUnidadexoneradaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrUnidadexoneradaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_UNIDADEXONERADA
        /// </summary>
        public void UpdateVcrUnidadexonerada(VcrUnidadexoneradaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrUnidadexoneradaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza una lista de registro de la tabla VCR_UNIDADEXONERADA NO exonerados
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void UpdateVcrUnidadexoneradaVersionNO(int vcrecacodi, string sUser)
        {
            try
            {
                FactoryTransferencia.GetVcrUnidadexoneradaRepository().UpdateVersionNO(vcrecacodi, sUser);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza una lista de registro de la tabla VCR_UNIDADEXONERADA SI exonerados
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public void UpdateVcrUnidadexoneradaVersionSI(int vcrecacodi, string sUser, int vcruexcodi)
        {
            try
            {
                FactoryTransferencia.GetVcrUnidadexoneradaRepository().UpdateVersionSI(vcrecacodi, sUser, vcruexcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_UNIDADEXONERADA
        /// </summary>
        /// <param name="vcrecacodi">Version de calculo</param>
        public void DeleteVcrUnidadexonerada(int vcrecacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrUnidadexoneradaRepository().Delete(vcrecacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_UNIDADEXONERADA
        /// </summary>
        public VcrUnidadexoneradaDTO GetByIdVcrUnidadexonerada(int vcruexcodi)
        {
            return FactoryTransferencia.GetVcrUnidadexoneradaRepository().GetById(vcruexcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_UNIDADEXONERADA
        /// </summary>
        public List<VcrUnidadexoneradaDTO> ListVcrUnidadexoneradas()
        {
            return FactoryTransferencia.GetVcrUnidadexoneradaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrUnidadexonerada
        /// </summary>
        public List<VcrUnidadexoneradaDTO> GetByCriteriaVcrUnidadexoneradas()
        {
            return FactoryTransferencia.GetVcrUnidadexoneradaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_UNIDADEXONERADA incluido los Nombres de Empresa, Central y Unidad
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// </summary>
        public List<VcrUnidadexoneradaDTO> ListVcrUnidadexoneradasParametro(int vcrecacodi)
        {
            return FactoryTransferencia.GetVcrUnidadexoneradaRepository().ListParametro(vcrecacodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_UNIDADEXONERADA y mostrarlo en un pop up
        /// </summary>
        public VcrUnidadexoneradaDTO GetByIdVcrUnidadexoneradaView(int vcruexcodi)
        {
            return FactoryTransferencia.GetVcrUnidadexoneradaRepository().GetByIdView(vcruexcodi);
        }

        #endregion

        #region Métodos Tabla VCR_VERDEFICIT

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERDEFICIT
        /// </summary>
        public void SaveVcrVerdeficit(VcrVerdeficitDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerdeficitRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERDEFICIT
        /// </summary>
        public void UpdateVcrVerdeficit(VcrVerdeficitDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerdeficitRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERDEFICIT
        /// </summary>
        public void DeleteVcrVerdeficit(int vcrvdecodi)
        {
            try
            {
                FactoryTransferencia.GetVcrVerdeficitRepository().Delete(vcrvdecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERDEFICIT
        /// </summary>
        public VcrVerdeficitDTO GetByIdVcrVerdeficit(int vcrvdecodi)
        {
            return FactoryTransferencia.GetVcrVerdeficitRepository().GetById(vcrvdecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERDEFICIT
        /// </summary>
        public List<VcrVerdeficitDTO> ListVcrVerdeficits()
        {
            return FactoryTransferencia.GetVcrVerdeficitRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERDEFICIT fitrado por revisión, grupo y fecha
        /// </summary>
        /// <param name="Vcrdsrcodi">Identificador de la tabla VCR_VERDEFICIT</param>
        /// <param name="grupocodi">Identificador de la tabla PR_GRUPO</param>
        /// <param name="vcrvdefecha">Dia de consulta - formato MM/DD/YYY HH:MM</param>
        public List<VcrVerdeficitDTO> ListVcrVerdeficitsDia(int vcrdsrcodi, int grupocodi, DateTime vcrvdefecha)
        {
            return FactoryTransferencia.GetVcrVerdeficitRepository().ListDia(vcrdsrcodi, grupocodi, vcrvdefecha);
        }


        /// <summary>
        /// Permite listar todos los periodos de la tabla VCR_VERDEFICIT fitrado por revisión y fecha
        /// </summary>
        /// <param name="Vcrdsrcodi">Identificador de la tabla VCR_VERDEFICIT</param>
        /// <param name="grupocodi">Identificador de la tabla PR_GRUPO</param>
        /// <param name="vcrvdefecha">Dia de consulta - formato MM/DD/YYYY</param>
        private List<VcrVerdeficitDTO> ListVcrVerdeficitsDiaHFHI(int vcrdsrcodi, DateTime vcrvdefecha)
        {
            return FactoryTransferencia.GetVcrVerdeficitRepository().ListDiaHFHI(vcrdsrcodi, vcrvdefecha);
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrVerdeficit
        /// </summary>
        public List<VcrVerdeficitDTO> GetByCriteriaVcrVerdeficits(int vcrdsrcodi)
        {
            return FactoryTransferencia.GetVcrVerdeficitRepository().GetByCriteria(vcrdsrcodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VCR_VERDEFICIT
        /// </summary>
        /// <param name="vc">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoVcrDef(int vcrdsrcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VcrVersiondsrnsDTO EntidadVersionDSRNS = this.GetByIdVcrVersiondsrns(vcrdsrcodi);
            //StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<VcrVerdeficitDTO> ListaDeficit = this.GetByCriteriaVcrVerdeficits(vcrdsrcodi);

            if (formato == 1)
            {
                fileName = "ReporteDeficit.xlsx";
                ExcelDocument.GenerarFormatoVcrDeficit(pathFile + fileName, EntidadVersionDSRNS, ListaDeficit);
            }

            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de los saldos
        /// </summary>
        /// <param name="vcrecacodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoVcrSaldos(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
            List<VcrCargoincumplDTO> ListaCargoIncumpl = this.ListVcrCargoincumpls(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteSaldos.xlsx";
                ExcelDocument.GenerarFormatoSaldos(pathFile + fileName, EntidadRecalculo, ListaCargoIncumpl);
            }

            return fileName;
        }

        #endregion

        #region Métodos Tabla VCR_VERPORCTRESERV

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERPORCTRESERV
        /// </summary>
        public void SaveVcrVerporctreserv(VcrVerporctreservDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerporctreservRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERPORCTRESERV
        /// </summary>
        public void UpdateVcrVerporctreserv(VcrVerporctreservDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerporctreservRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERPORCTRESERV
        /// </summary>
        public void DeleteVcrVerporctreserv(int vcrinccodi)
        {
            try
            {
                FactoryTransferencia.GetVcrVerporctreservRepository().Delete(vcrinccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERPORCTRESERV
        /// </summary>
        public VcrVerporctreservDTO GetByIdVcrVerporctreserv(int vcrinccodi, DateTime vcrvprfecha)
        {
            return FactoryTransferencia.GetVcrVerporctreservRepository().GetById(vcrinccodi, vcrvprfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERPORCTRESERV
        /// </summary>
        public List<VcrVerporctreservDTO> ListVcrVerporctreservs(int vcrinccodi, int equicodicen, int equicodiuni)
        {
            return FactoryTransferencia.GetVcrVerporctreservRepository().List(vcrinccodi, equicodicen, equicodiuni);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VCR_VERPORCTRESERV
        /// </summary>
        public List<VcrVerporctreservDTO> GetByCriteriaVcrVerporctreservs(int vcrinccodi)
        {
            return FactoryTransferencia.GetVcrVerporctreservRepository().GetByCriteria(vcrinccodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VCR_VERPORCTRESERV
        /// </summary>
        /// <param name="vcrinccodi">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoVcrRPNS(int vcrinccodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VcrVersionincplDTO EntidadPR21 = this.GetByIdVcrVersionincpl(vcrinccodi);
            //StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<VcrVerporctreservDTO> ListaDetalle = this.GetByCriteriaVcrVerporctreservs(vcrinccodi);

            if (formato == 1)
            {
                fileName = "ReportePorcentajeRPNS.xlsx";
                ExcelDocument.GenerarFormatoVcrRPNS(pathFile + fileName, EntidadPR21, ListaDetalle);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERPORCTRESERV de la Unidad de generación totalizado por VCRVPRRPNS 
        /// </summary>
        /// <param name="vcrinccodi">Código de la Versión de Incumplimiento</param>
        /// <param name="equicodiuni">Identificador de la tabla eq_equipo para las unidades</param>
        /// <param name="equicodicen">Identificador de la tabla eq_equipo para las Centrales</param>
        public VcrVerporctreservDTO GetByIdVcrVerporctreservPorUnidad(int vcrinccodi, int equicodiuni, int equicodicen)
        {
            return FactoryTransferencia.GetVcrVerporctreservRepository().GetByIdPorUnidad(vcrinccodi, equicodiuni, equicodicen);
        }
        #endregion

        #region Métodos Tabla VCR_VERINCUMPLIM

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERINCUMPLIM
        /// </summary>
        public void SaveVcrVerincumplim(VcrVerincumplimDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerincumplimRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERINCUMPLIM
        /// </summary>
        public void UpdateVcrVerincumplim(VcrVerincumplimDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerincumplimRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERINCUMPLIM
        /// </summary>
        public void DeleteVcrVerincumplim(int vcrinccodi)
        {
            try
            {
                FactoryTransferencia.GetVcrVerincumplimRepository().Delete(vcrinccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERINCUMPLIM
        /// </summary>
        public VcrVerincumplimDTO GetByIdVcrVerincumplim(int vcrinccodi, DateTime vcrvinfecha)
        {
            return FactoryTransferencia.GetVcrVerincumplimRepository().GetById(vcrinccodi, vcrvinfecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERINCUMPLIM
        /// </summary>
        public List<VcrVerincumplimDTO> ListVcrVerincumplims(int vcrinccodi, int equicodicen, int equicodiuni)
        {
            return FactoryTransferencia.GetVcrVerincumplimRepository().List(vcrinccodi, equicodicen, equicodiuni);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VCR_VERINCUMPLIM
        /// </summary>
        public List<VcrVerincumplimDTO> GetByCriteriaVcrVerincumplims(int vcrinccodi)
        {
            return FactoryTransferencia.GetVcrVerincumplimRepository().GetByCriteria(vcrinccodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VCR_VERINCUMPLIM
        /// </summary>
        /// <param name="vc">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoVcrPr21(int vcrinccodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VcrVersionincplDTO EntidadPR21 = this.GetByIdVcrVersionincpl(vcrinccodi);
            //StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<VcrVerincumplimDTO> ListaDetalle = this.GetByCriteriaVcrVerincumplims(vcrinccodi);

            if (formato == 1)
            {
                fileName = "ReporteIncumplimientoPR21.xlsx";
                ExcelDocument.GenerarFormatoVcrPR21(pathFile + fileName, EntidadPR21, ListaDetalle);
            }

            return fileName;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERINCUMPLIM de la Unidad de generación totalizado por VCRVINCUMPLI 
        /// </summary>
        /// <param name="vcrinccodi">Código de la Versión de Incumplimiento</param>
        /// <param name="equicodiuni">Identificador de la tabla eq_equipo</param>
        public VcrVerincumplimDTO GetByIdVcrVerincumplimPorUnidad(int vcrinccodi, int equicodiuni, int equicodicen)
        {
            return FactoryTransferencia.GetVcrVerincumplimRepository().GetByIdPorUnidad(vcrinccodi, equicodiuni, equicodicen);
        }
        #endregion

        #region Métodos Tabla VCR_VERRNS

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERRNS
        /// </summary>
        public void SaveVcrVerrns(VcrVerrnsDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerrnsRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERRNS
        /// </summary>
        public void UpdateVcrVerrns(VcrVerrnsDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVerrnsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERRNS
        /// </summary>
        /// <param name="vcrdsrcodi">Identificador de la tabla VCR_VERSIONDSRNS</param>
        /// <param name="vcvrnstipocarga">Tipo de carga</param>
        public void DeleteVcrVerrns(int vcrdsrcodi, int vcvrnstipocarga)
        {
            try
            {
                FactoryTransferencia.GetVcrVerrnsRepository().Delete(vcrdsrcodi, vcvrnstipocarga);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERRNS
        /// </summary>
        public VcrVerrnsDTO GetByIdVcrVerrns(int vcvrnscodi)
        {
            return FactoryTransferencia.GetVcrVerrnsRepository().GetById(vcvrnscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERRNS
        /// </summary>
        public List<VcrVerrnsDTO> ListVcrVerrnss()
        {
            return FactoryTransferencia.GetVcrVerrnsRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERRNS fitrado por revisión y fecha
        /// </summary>
        /// <param name="vcrdsrcodi">Identificador de la tabla VCR_VERSIONDSRNS</param>
        /// <param name="grupocodi">Identificador de la tabla PR_GRUPO</param>
        /// <param name="vcvrnsfecha">Dia de consulta</param>
        /// <param name="vcvrnstipocarga">Tipo de carga: subida->1 / bajada->2</param>
        public List<VcrVerrnsDTO> ListVcrVerrnssDia(int vcrdsrcodi, int grupocodi, DateTime vcvrnsfecha, int vcvrnstipocarga)
        {
            return FactoryTransferencia.GetVcrVerrnsRepository().ListDia(vcrdsrcodi, grupocodi, vcvrnsfecha, vcvrnstipocarga);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrVerrns
        /// </summary>
        /// <param name="vcrdsrcodi">Identificador de la tabla VCR_VERSIONDSRNS</param>
        /// <param name="vcvrnstipocarga">Tipo de carga</param>
        public List<VcrVerrnsDTO> GetByCriteriaVcrVerrnss(int vcrdsrcodi, int vcvrnstipocarga)
        {
            return FactoryTransferencia.GetVcrVerrnsRepository().GetByCriteria(vcrdsrcodi, vcvrnstipocarga);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VCR_VERRNS
        /// </summary>
        /// <param name="vcrdsrcodi">Código de la Versión de Recálculo< - VCR_VERSIONDSRNS</param>
        /// <param name="vcvrnstipocarga">Tipo de carga: Subida=1 / Bajada=2</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoVcrRNS(int vcrdsrcodi, int vcvrnstipocarga, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VcrVersiondsrnsDTO EntidadVersionDSRNS = this.GetByIdVcrVersiondsrns(vcrdsrcodi);
            EntidadVersionDSRNS.Vcrdsrusumodificacion = "Subida";
            if (vcvrnstipocarga == 2)
                EntidadVersionDSRNS.Vcrdsrusumodificacion = "Bajada";
            //StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<VcrVerrnsDTO> ListaRNS = this.GetByCriteriaVcrVerrnss(vcrdsrcodi, vcvrnstipocarga);

            if (formato == 1)
            {
                fileName = "ReporteRNS.xlsx";
                ExcelDocument.GenerarFormatoVcrRNS(pathFile + fileName, EntidadVersionDSRNS, ListaRNS);
            }

            return fileName;
        }

        #endregion

        #region Métodos Tabla VCR_VERSIONDSRNS

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public void SaveVcrVersiondsrns(VcrVersiondsrnsDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVersiondsrnsRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public void UpdateVcrVersiondsrns(VcrVersiondsrnsDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVersiondsrnsRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public void DeleteVcrVersiondsrns(int vcrdsrcodi)
        {
            try
            {
                FactoryTransferencia.GetVcrVersiondsrnsRepository().Delete(vcrdsrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public VcrVersiondsrnsDTO GetByIdVcrVersiondsrns(int vcrdsrcodi)
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().GetById(vcrdsrcodi);
        }

        /// <summary>
        /// Permite obtener un registro Abierto de la tabla VCR_VERSIONDSRNS por periodo
        /// </summary>
        /// <param name="pericodi">Periodo de calculo</param>
        public VcrVersiondsrnsDTO GetByIdVcrVersiondsrnsPeriodo(int pericodi)
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().GetByIdPeriodo(pericodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public VcrVersiondsrnsDTO GetByIdVcrVersiondsrnsView(int vcrdsrcodi, int pericodi)
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().GetByIdView(vcrdsrcodi, pericodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public List<VcrVersiondsrnsDTO> ListVcrVersiondsrnss()
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERSIONDSRNS
        /// </summary>
        public List<VcrVersiondsrnsDTO> ListVcrVersiondsrnssIndex()
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().ListIndex();
        }

        /// <summary>
        /// Permite obtener version en base al iPerCodi
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Lista de VcrVersiondsrnsDTO</returns>
        public List<VcrVersiondsrnsDTO> ListVcrVersionDSRNS(int iPerCodi)
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().ListVersion(iPerCodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrVersiondsrns
        /// </summary>
        public List<VcrVersiondsrnsDTO> GetByCriteriaVcrVersiondsrnss()
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite editar busca vcrdsrcodi y pericodi
        /// </summary>
        public VcrVersiondsrnsDTO GetByIdVcrVersiondsrnsEdit(int vcrdsrcodi, int pericodi)
        {
            return FactoryTransferencia.GetVcrVersiondsrnsRepository().GetByIdEdit(vcrdsrcodi, pericodi);
        }

        #endregion

        #region Métodos Tabla VCR_VERSIONINCPL

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERSIONINCPL
        /// </summary>
        public void SaveVcrVersionincpl(VcrVersionincplDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVersionincplRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERSIONINCPL
        /// </summary>
        public void UpdateVcrVersionincpl(VcrVersionincplDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVersionincplRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERSIONINCPL
        /// </summary>
        public void DeleteVcrVersionincpl(int vcrinccodi)
        {
            try
            {
                FactoryTransferencia.GetVcrVersionincplRepository().Delete(vcrinccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERSIONINCPL
        /// </summary>
        public VcrVersionincplDTO GetByIdVcrVersionincpl(int vcrinccodi)
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().GetById(vcrinccodi);
        }



        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERSIONINCPL
        /// </summary>
        public List<VcrVersionincplDTO> ListVcrVersionincplsIndex()
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().ListIncplIndex();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERSIONINCPL
        /// </summary>
        public List<VcrVersionincplDTO> ListVcrVersionincpls()
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().List();
        }

        /// <summary>
        /// Permite obtener version en base al iPerCodi
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Lista de VcrVersionincplDTO</returns>
        public List<VcrVersionincplDTO> ListVcrIncpl(int iPerCodi)
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().ListIncpl(iPerCodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrVersionincpl
        /// </summary>
        public List<VcrVersionincplDTO> GetByCriteriaVcrVersionincpls()
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().GetByCriteria();
        }

        public VcrVersionincplDTO GetByIdVcrVersionincplEdit(int vcrinccodi, int pericodi)
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().GetByIdEdit(vcrinccodi, pericodi);
        }

        public VcrVersionincplDTO GetByIdVcrIncplView(int vcrinccodi, int pericodi)
        {
            return FactoryTransferencia.GetVcrVersionincplRepository().GetByIdView(vcrinccodi, pericodi);
        }

        #endregion

        #region Métodos Tabla VCR_VERSUPERAVIT

        /// <summary>
        /// Inserta un registro de la tabla VCR_VERSUPERAVIT
        /// </summary>
        public void SaveVcrVersuperavit(VcrVersuperavitDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVersuperavitRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VCR_VERSUPERAVIT
        /// </summary>
        public void UpdateVcrVersuperavit(VcrVersuperavitDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVcrVersuperavitRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VCR_VERSUPERAVIT
        /// </summary>
        public void DeleteVcrVersuperavit(int vcrvsacodi)
        {
            try
            {
                FactoryTransferencia.GetVcrVersuperavitRepository().Delete(vcrvsacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VCR_VERSUPERAVIT
        /// </summary>
        public VcrVersuperavitDTO GetByIdVcrVersuperavit(int vcrvsacodi)
        {
            return FactoryTransferencia.GetVcrVersuperavitRepository().GetById(vcrvsacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERSUPERAVIT
        /// </summary>
        public List<VcrVersuperavitDTO> ListVcrVersuperavits()
        {
            return FactoryTransferencia.GetVcrVersuperavitRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VCR_VERSUPERAVIT fitrado por revisión y fecha
        /// </summary>
        /// <param name="Vcrdsrcodi">Identificador de la tabla VCR_VERSUPERAVIT</param>
        /// <param name="grupocodi">Identificador de la tabla PR_GRUPO</param>
        /// <param name="vcrvsafecha">Dia de consulta</param>
        public List<VcrVersuperavitDTO> ListVcrVersuperavitsDia(int vcrdsrcodi, int grupocodi, DateTime vcrvsafecha)
        {
            return FactoryTransferencia.GetVcrVersuperavitRepository().ListDia(vcrdsrcodi, grupocodi, vcrvsafecha);
        }

        //Agregado el 29-04-2019
        public List<VcrVersuperavitDTO> ListVcrVersuperavitsDiaURS(DateTime vcrvsafecha)
        {
            return FactoryTransferencia.GetVcrVersuperavitRepository().ListDiaURS(vcrvsafecha);
        }
        /// <summary>
        /// Permite realizar búsquedas en la tabla VcrVersuperavit
        /// </summary>
        public List<VcrVersuperavitDTO> GetByCriteriaVcrVersuperavits(int vcrdsrcodi)
        {
            return FactoryTransferencia.GetVcrVersuperavitRepository().GetByCriteria(vcrdsrcodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VCR_VERSUPERAVIT
        /// </summary>
        /// <param name="vc">Código de la Versión de Recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoVcrSup(int vcrdsrcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VcrVersiondsrnsDTO EntidadVersionDSRNS = this.GetByIdVcrVersiondsrns(vcrdsrcodi);
            //StRecalculoDTO EntidadRecalculo = this.GetByIdStRecalculo(strecacodi);
            List<VcrVersuperavitDTO> ListaSuperavit = this.GetByCriteriaVcrVersuperavits(vcrdsrcodi);

            if (formato == 1)
            {
                fileName = "ReporteSuperavit.xlsx";
                ExcelDocument.GenerarFormatoVcrSuperavit(pathFile + fileName, EntidadVersionDSRNS, ListaSuperavit);
            }

            return fileName;
        }

        #endregion

        #region Métodos Genelares

        /// <summary>
        /// Permite buscar un registro en la tabla EQ_EQUIPO por equicodi
        /// </summary>
        public EqEquipoDTO GetByEquicodi(int equicodi)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByEquicodi(equicodi);
        }

        /// <summary>
        /// Permite buscar un registro en la tabla EQ_EQUIPO por equinomb
        /// </summary>
        public EqEquipoDTO GetByEquiNomb(string equinomb)
        {
            return FactoryTransferencia.GetCentralGeneracionRepository().GetByEquiNomb(equinomb);
        }

        ///// <summary>
        ///// Permite listar todos los registros de la tabla EQ_EQUIPO por equipadre
        ///// </summary>
        //public List<EqEquipoDTO> GetByEquiPadre(int equicodi)
        //{
        //    return FactoryTransferencia.GetCentralGeneracionRepository().GetByEquiPadre(equicodi);
        //}     


        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="hoja"></param>
        public DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            return UtilCompensacionRSF.GeneraDataset(RutaArchivo, hoja);
        }


        /// <summary>
        /// Permite analizar si un dato es numérico
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private Boolean AnalizarNumerico(string valor)
        {
            Boolean bresult = false;
            decimal number3;
            bresult = decimal.TryParse(valor, out number3);

            return bresult;
        }

        /// <summary>
        /// Procedimiento que se encarga de ejecutar el procedimiento del calculo de RSF del periodo 2020.12 hacia atras
        /// </summary>
        /// <param name="pericodi">Periodo de calculo</param>
        /// <param name="vcrecacodi">Version de calculo</param>
        /// <param name="suser">Usuario conectado</param>
        public string ProcesarCalculo2020(int pericodi, int vcrecacodi, string suser)
        {
            string sResultado = "1";
            try
            {
                //Limpiamos Todos los calculos anteriores de la Version en Acción
                string sBorrar = this.EliminarCalculo(vcrecacodi);
                if (!sBorrar.Equals("1"))
                {
                    sResultado = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                    return sResultado;
                }
                //INICIALIZAMOS ALGUNAS VARIABLES GENERALES
                //Traemos la entidad de la versión de recalculo
                VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
                if (EntidadRecalculo.Vcrdsrcodi == null || EntidadRecalculo.Vcrdsrcodi == 0)
                {
                    sResultado = "Lo sentimos, La version de calculo no tiene asignado una version de Déficit, Superávit y Reserva No Suministrada";
                    return sResultado;
                }
                if (EntidadRecalculo.Vcrinccodi == null || EntidadRecalculo.Vcrinccodi == 0)
                {
                    sResultado = "Lo sentimos, La version de calculo no tiene asignado una version de incumplimiento";
                    return sResultado;
                }
                //FECHAS DE INICIO FINAL DEL MES
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string sPeriodo = EntidadPeriodo.AnioCodi.ToString() + sMes; //YYYYMM
                //Valores generales
                decimal dUnoEntre24 = (decimal)1 / 24;
                decimal dFactorCumplimiento = 0.85M;
                int iNumReg = 0;

                //INICIAMOS EL PROCESO DE CALCULO
                //Lista de URS
                List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURSCostoMarginal(pericodi, EntidadRecalculo.Recacodi);

                //ASSETEC 20190115
                #region Asignación de reserva (AR) – CU09.01
                //El proceso de cálculo para valorizar el costo del servicio por la Asignación de Reserva es el siguiente: 
                foreach (TrnBarraursDTO URS in listaURS)
                {

                    //PBF_URS: Potencia que corresponde a la Provisión Base Firme de la URS. //VCRPBPOTENCIABF
                    //Si la URS no posee Provisión Base Firme se considerará este valor como cero. Corresponde al valor indicado en el CU09.01. En MW.
                    decimal dProvisionBaseFirmeURS = 0;
                    decimal dPrecioPotenciaBaseFirmeURS = 0;
                    VcrProvisionbaseDTO dtoProvisionBase = this.GetByIdVcrProvisionbaseURS(URS.GrupoCodi, sPeriodo); //Para una URS
                    if (dtoProvisionBase != null)
                    {
                        //Existe provisión base para esta URS
                        dProvisionBaseFirmeURS = dtoProvisionBase.Vcrpbpotenciabf; //PBFi
                        dPrecioPotenciaBaseFirmeURS = dtoProvisionBase.Vcrpbpreciobf; //Precio_PBFi
                    }

                    //Para cada URS se calcula Asignación de Reserva de una URS en un Dia
                    DateTime dFecha = dFecInicio;

                    //decimal dHF_HI1 = (decimal)(dHoraFinal1).TotalHours;
                    while (dFecha <= dFecFin)
                    {
                        decimal dRA_PBF = 0;
                        decimal dRA_MA = 0;
                        decimal dMPA_id = 0; //Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-dia) para este URS

                        //Lista de Asignacion de reserva para poder ingresar el correcto valor de la oferta maxima del dia.
                        //RA (URS,t): Reserva Asignada en el periodo “t” (comprendido desde HI a HF) de la URS.
                        List<VcrReservasignDTO> listaReservaAsignada = this.GetByCriteriaVcrReservasignsURSDia(vcrecacodi, URS.GrupoCodi, dFecha);
                        foreach (VcrReservasignDTO dtoReservaAsinada in listaReservaAsignada)
                        {   //Para cada intervalo Ti
                            decimal dDiferencia = dtoReservaAsinada.Vcrasgreservasign - dProvisionBaseFirmeURS; //RA(urs,ti) - PBF(urs)
                            //OJO LA HORAS ESTAN EN FORMATO FECHA HAY QUE LLEVARLOS A HORAS / 24 O A MINUTOS / 24x60
                            DateTime dHoraFinal = (DateTime)dtoReservaAsinada.Vcrasghorfinal;
                            DateTime dHoraInicio = (DateTime)dtoReservaAsinada.Vcrasghorinicio;
                            decimal dHF_HI = (decimal)(dHoraFinal - dHoraInicio).TotalHours;
                            if (dDiferencia > 0)
                            {
                                //1.- RA_PBF = PBF_URS x (HF-HI) x 1/24 
                                dRA_PBF += dProvisionBaseFirmeURS * dHF_HI * dUnoEntre24;
                                //2.- RA_MA = (RA(urs,t) - PBF(urs)) x (HF-HI) x 1/24
                                dRA_MA += dDiferencia * dHF_HI * dUnoEntre24; //Math.Round(dDiferencia) * dHF_HI * dUnoEntre24;
                                //Buscamos el Precio en las ofertas para calcular el MPA(i,d) = max𝑖,𝑑(𝑅𝐴𝑖,𝑑,𝑡𝑖1 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥1; 𝑅𝐴𝑖,𝑑,𝑡𝑖2 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥2, … ; 𝑅𝐴𝑖,𝑑,𝑡𝑖𝑛× 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥𝑛) ,∀ 𝑡𝑥n
                                decimal dPoF_idt = this.GetOfertaMaxDiaGrupoCodiHiHf2020(vcrecacodi, URS.GrupoCodi, dFecha, dHoraInicio, dHoraFinal, ConstantesCompensacionRSF.TipoCargaSubir);
                                //Tomando la mayor oferta dMPA_id
                                if (dMPA_id < dPoF_idt)
                                    dMPA_id = dPoF_idt;
                            }
                            else
                            {
                                //Si: RA(urs,t) - PBF(urs) ≤ 0
                                //1.- RA_PBF = RA(urs,t) x (HF-HI) x 1/24
                                dRA_PBF += dtoReservaAsinada.Vcrasgreservasign * dHF_HI * dUnoEntre24;
                                //2.- RA_MA=0
                                dRA_MA += 0;
                            }
                        }
                        //Guardando la Asignación de Reserva (urs,dia)
                        VcrAsignacionreservaDTO dtoAsignacionreserva = new VcrAsignacionreservaDTO();
                        dtoAsignacionreserva.Vcrecacodi = vcrecacodi;
                        dtoAsignacionreserva.Grupocodi = URS.GrupoCodi;
                        dtoAsignacionreserva.Gruponomb = URS.GrupoNomb;
                        dtoAsignacionreserva.Vcrarfecha = dFecha;
                        dtoAsignacionreserva.Vcrarusucreacion = suser;
                        //Calculo la Asignación de Reserva(urs,dia): 
                        dtoAsignacionreserva.Vcrarrapbf = dRA_PBF;
                        dtoAsignacionreserva.Vcrarprbf = dPrecioPotenciaBaseFirmeURS;
                        dtoAsignacionreserva.Vcrarrama = dRA_MA;
                        dtoAsignacionreserva.Vcrarmpa = dMPA_id; //Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-dia) para este URS
                        dtoAsignacionreserva.Vcrarramaursra = 0; // Reserva Asignada correspondiente al Mercado de Ajuste entre todos los URS que estan en reserva asignada
                        dtoAsignacionreserva.Vcrarasignreserva = 0;// (dRA_PBF * dPrecioPotenciaBaseFirmeURS) / 30 + (dRA_MA * dMPAM) / 30; Lo vamos a registrar despues, cuando ya se tenga el dMPAM de todos los URS en el día

                        this.SaveVcrAsignacionreserva(dtoAsignacionreserva);
                        //Cambiamos de fecha
                        dFecha = dFecha.AddDays(1);
                    }
                    //Cambiamos de URS
                }
                //Actualizando la tabla AsignacionReserva:
                //El valor de la Reserva Asignada correspondiente al Mercado de Ajuste entre todos los URS
                //La Asignación de Reserva
                DateTime dDia = dFecInicio;
                while (dDia <= dFecFin)
                {
                    decimal dMPA_d = this.GetByVcrAsignacionreservadMPA2020(vcrecacodi, dDia); //max(Vcrarmpa): dMPA_id -> Mayor Precio Asignado de Ofertas del Mercado de Ajuste del día “d” (S/./MWmes) 
                    List<VcrAsignacionreservaDTO> ListaAsignacionReserva = this.GetByCriteriaVcrAsignacionReservaOferta(vcrecacodi, dDia);
                    foreach (var item in ListaAsignacionReserva)
                    {
                        item.Vcrarramaursra = dMPA_d;
                        item.Vcrarasignreserva = (item.Vcrarrapbf * item.Vcrarprbf) / 30 + (item.Vcrarrama * dMPA_d) / 30;
                        this.UpdateVcrAsignacionreserva(item);
                    }
                    dDia = dDia.AddDays(1);
                }

                #endregion

                #region Costo de Oportunidad (CO) – CU09.02
                //Lista de URS
                //Para cada URS se calcula su Costo Operativo
                foreach (TrnBarraursDTO URS in listaURS)
                {
                    TrnBarraursDTO dtoBarra = this.servicioBarraUrs.GetByIdUrs(URS.BarrCodi, URS.GrupoCodi);
                    DateTime dFecha = dFecInicio;
                    //Para cada dia se calcula el costo operativo de una URS
                    while (dFecha <= dFecFin)
                    {
                        decimal[] aCostoOportunidad = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosOportunidad
                        #region COSTO MARGINAL
                        //Lista de 48 registros Costos Marginales asociado a una URS
                        CostoMarginalDTO dtoCostoMarginal = this.servicioCostoMarginal.GetByIdCostoMarginalPorBarraDia(pericodi, EntidadRecalculo.Recacodi, dtoBarra.BarrCodi, dFecha.Day);
                        decimal[] aCostoMarginal = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosVariables
                        if (dtoCostoMarginal != null)
                        {
                            aCostoMarginal[0] = dtoCostoMarginal.CosMar1;
                            aCostoMarginal[1] = dtoCostoMarginal.CosMar3;
                            aCostoMarginal[2] = dtoCostoMarginal.CosMar5;
                            aCostoMarginal[3] = dtoCostoMarginal.CosMar7;
                            aCostoMarginal[4] = dtoCostoMarginal.CosMar9;
                            aCostoMarginal[5] = dtoCostoMarginal.CosMar11;
                            aCostoMarginal[6] = dtoCostoMarginal.CosMar13;
                            aCostoMarginal[7] = dtoCostoMarginal.CosMar15;
                            aCostoMarginal[8] = dtoCostoMarginal.CosMar17;
                            aCostoMarginal[9] = dtoCostoMarginal.CosMar19;
                            aCostoMarginal[10] = dtoCostoMarginal.CosMar21;
                            aCostoMarginal[11] = dtoCostoMarginal.CosMar23;
                            aCostoMarginal[12] = dtoCostoMarginal.CosMar25;
                            aCostoMarginal[13] = dtoCostoMarginal.CosMar27;
                            aCostoMarginal[14] = dtoCostoMarginal.CosMar29;
                            aCostoMarginal[15] = dtoCostoMarginal.CosMar31;
                            aCostoMarginal[16] = dtoCostoMarginal.CosMar33;
                            aCostoMarginal[17] = dtoCostoMarginal.CosMar35;
                            aCostoMarginal[18] = dtoCostoMarginal.CosMar37;
                            aCostoMarginal[19] = dtoCostoMarginal.CosMar39;
                            aCostoMarginal[20] = dtoCostoMarginal.CosMar41;
                            aCostoMarginal[21] = dtoCostoMarginal.CosMar43;
                            aCostoMarginal[22] = dtoCostoMarginal.CosMar45;
                            aCostoMarginal[23] = dtoCostoMarginal.CosMar47;
                            aCostoMarginal[24] = dtoCostoMarginal.CosMar49;
                            aCostoMarginal[25] = dtoCostoMarginal.CosMar51;
                            aCostoMarginal[26] = dtoCostoMarginal.CosMar53;
                            aCostoMarginal[27] = dtoCostoMarginal.CosMar55;
                            aCostoMarginal[28] = dtoCostoMarginal.CosMar57;
                            aCostoMarginal[29] = dtoCostoMarginal.CosMar59;
                            aCostoMarginal[30] = dtoCostoMarginal.CosMar61;
                            aCostoMarginal[31] = dtoCostoMarginal.CosMar63;
                            aCostoMarginal[32] = dtoCostoMarginal.CosMar65;
                            aCostoMarginal[33] = dtoCostoMarginal.CosMar67;
                            aCostoMarginal[34] = dtoCostoMarginal.CosMar69;
                            aCostoMarginal[35] = dtoCostoMarginal.CosMar71;
                            aCostoMarginal[36] = dtoCostoMarginal.CosMar73;
                            aCostoMarginal[37] = dtoCostoMarginal.CosMar75;
                            aCostoMarginal[38] = dtoCostoMarginal.CosMar77;
                            aCostoMarginal[39] = dtoCostoMarginal.CosMar79;
                            aCostoMarginal[40] = dtoCostoMarginal.CosMar81;
                            aCostoMarginal[41] = dtoCostoMarginal.CosMar83;
                            aCostoMarginal[42] = dtoCostoMarginal.CosMar85;
                            aCostoMarginal[43] = dtoCostoMarginal.CosMar87;
                            aCostoMarginal[44] = dtoCostoMarginal.CosMar89;
                            aCostoMarginal[45] = dtoCostoMarginal.CosMar91;
                            aCostoMarginal[46] = dtoCostoMarginal.CosMar93;
                            aCostoMarginal[47] = dtoCostoMarginal.CosMar95;
                        }
                        else
                        {
                            BarraDTO dtoBarraFaltante = servicioBarra.GetByIdBarra(dtoBarra.BarrCodi);
                            sResultado = "Lo sentimos, No esta registrado el Costo Marginal de la Barra:" + dtoBarraFaltante.BarrNombre + " [ Barracodi: " + dtoBarra.BarrCodi + " Periodo: " + pericodi + " / Revisión: " + EntidadRecalculo.Recacodi + "]";
                            return sResultado;
                        }
                        #endregion

                        //Para todas las Unidades de Generación, en el caso de Unidades térmicas corresponde al modo de operación mientras que ha Unidades hidráulicas a central. 
                        List<VcrDespachoursDTO> listaUnidadesGeneracion = this.ListVcrDespachoursUnidadByUrsTipo(vcrecacodi, URS.GrupoCodi, "S");
                        foreach (VcrDespachoursDTO dtoUnidad in listaUnidadesGeneracion)
                        {
                            //Lista de 48 registros Costos Variables de la URS
                            List<VcrCostvariableDTO> listaCostoVariable = this.ListVcrCostvariables(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, dFecha);
                            decimal[] aCostoVariable = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosVariables
                            iNumReg = 0;
                            foreach (VcrCostvariableDTO dtoCostoVariable in listaCostoVariable)
                            {
                                aCostoVariable[iNumReg++] = dtoCostoVariable.Vcvarcostvar;
                            }
                            //Lista de 48 registros de Despacho Sin Reserva de la URS
                            List<VcrDespachoursDTO> listaSinAsignacion = this.ListVcrDespachoursByUrsUnidadTipoDia(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, "S", dFecha);
                            decimal[] aSinAsignacion = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Despachos Sin Reserva Asignada
                            iNumReg = 0;
                            foreach (VcrDespachoursDTO dtoSinAsignacion in listaSinAsignacion)
                            {
                                aSinAsignacion[iNumReg++] = dtoSinAsignacion.Vcdursdespacho;
                            }
                            //Lista de 48 registros de Despacho Sin Reserva de la URS
                            List<VcrDespachoursDTO> listaConAsignacion = this.ListVcrDespachoursByUrsUnidadTipoDia(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, "C", dFecha);
                            decimal[] aConAsignacion = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Despachos Con Reserva Asignada
                            iNumReg = 0;
                            foreach (VcrDespachoursDTO dtoConAsignacion in listaConAsignacion)
                            {
                                aConAsignacion[iNumReg++] = dtoConAsignacion.Vcdursdespacho;
                            }
                            //For de 48 intervalos de 30 minutos de cada arreglo para aplicar la formula
                            for (int k = 0; k < 48; k++)
                            {
                                //Recorremos en intervalo de cada 30 minutos para aplicar la formula
                                VcrCostoportdetalleDTO dtoCostoportdetalle = new VcrCostoportdetalleDTO();
                                dtoCostoportdetalle.Vcrecacodi = vcrecacodi;
                                dtoCostoportdetalle.Grupocodi = URS.GrupoCodi;
                                dtoCostoportdetalle.Gruponomb = URS.GrupoNomb;
                                dtoCostoportdetalle.Vcrcodfecha = dFecha;
                                dtoCostoportdetalle.Equicodi = dtoUnidad.Equicodi;
                                dtoCostoportdetalle.Vcrcodinterv = k + 1;
                                dtoCostoportdetalle.Vcrcodpdo = Math.Max((aSinAsignacion[k] - aConAsignacion[k]), 0);
                                dtoCostoportdetalle.Vcrcodcmgcp = aCostoMarginal[k] * 1000; //para convertilo a MegaWaths
                                dtoCostoportdetalle.Vcrcodcv = aCostoVariable[k];
                                //CO(urs,t) = máximo([PDO(s,urs,t) - PDO(c,urs,t)],0) x [CMgCP(urs,t) - CV(urs,t)] x 0.5
                                decimal dCostoOportunidad = dtoCostoportdetalle.Vcrcodpdo * (dtoCostoportdetalle.Vcrcodcmgcp - dtoCostoportdetalle.Vcrcodcv) * (decimal)0.5;
                                //Si CO(urs,t) < 0 entonces CO(urs,t)=0
                                if (dCostoOportunidad < 0) dCostoOportunidad = 0;
                                //Almacenando resultado y acumulando el subtotal para cada unidad: CO(urs,t) = ∑(u=1)^NU [CO(u,t)] 
                                aCostoOportunidad[k] += dCostoOportunidad;
                                dtoCostoportdetalle.Vcrcodcostoportun = dCostoOportunidad;
                                dtoCostoportdetalle.Vcrcodusucreacion = suser;
                                this.SaveVcrCostoportdetalle(dtoCostoportdetalle); //Importante, para el reporte Costo de oportunidad – CU10.04
                            }
                        }
                        decimal dVcrCOpCosto = 0;
                        for (int k = 0; k < 48; k++)
                        {
                            dVcrCOpCosto += aCostoOportunidad[k];
                        }
                        //Grabamos el Costo Operativo del dia para una URS
                        VcrCostoportunidadDTO dtoCostoOportunidad = new VcrCostoportunidadDTO();
                        dtoCostoOportunidad.Vcrecacodi = vcrecacodi;
                        dtoCostoOportunidad.Grupocodi = URS.GrupoCodi;
                        dtoCostoOportunidad.Gruponomb = URS.GrupoNomb;
                        dtoCostoOportunidad.Vcrcopfecha = dFecha;
                        dtoCostoOportunidad.Vcrcopcosto = dVcrCOpCosto;
                        dtoCostoOportunidad.Vcrcopusucreacion = suser;
                        this.SaveVcrCostoportunidad(dtoCostoOportunidad);

                        //Cambiamos de fecha
                        dFecha = dFecha.AddDays(1);
                    }
                    //Cambiamos de URS
                }

                #endregion

                #region Términos de Superávit  – CU09.03
                //---------------------------------------------------------------------------------------------------------
                //ASSETEC 20181205
                decimal dPAO = EntidadRecalculo.Vcrecapaosinergmin; //Precio Máximo Asignado por OSINERGMIN (S/./MW-mes)
                decimal dKc = EntidadRecalculo.Vcrecakcalidad; //Parámetro configurable. Normalmente Kc = 1.
                //Traemos la lista de Versiones de Déficit, Superávit y Reserva No Suministrada – CU05 - VCR_VERSIONDSRNS
                VcrVersiondsrnsDTO dtoVersionDSRN = this.GetByIdVcrVersiondsrns((int)EntidadRecalculo.Vcrdsrcodi);
                if (dtoVersionDSRN != null)
                {   //Existe en el periodo, calculamos deficit, superavit y rns
                    DateTime dDiaCursor = dFecInicio;
                    //Para cada dia dentro del periodo calculo los Terminos Superavit  
                    while (dDiaCursor <= dFecFin)
                    {
                        //MPA: Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-mes)
                        decimal dMPd = this.GetByVcrAsignacionreservadMPA2020(vcrecacodi, dDiaCursor);
                        foreach (TrnBarraursDTO URS in listaURS)
                        {   //Insertamos por defecto todos los registros para el calculo
                            decimal dTotDT = 0;
                            decimal dTotST = 0;
                            decimal dTotSTURS = 0;
                            decimal dTotRNS = 0;
                            decimal dDeficit = 0;
                            decimal dSuperavit = 0;
                            decimal dReservaNoSuministrada = 0;
                            //Insertamos
                            VcrTermsuperavitDTO dtoTermsuperavit = new VcrTermsuperavitDTO();
                            dtoTermsuperavit.Vcrecacodi = vcrecacodi;
                            dtoTermsuperavit.Grupocodi = URS.GrupoCodi;
                            dtoTermsuperavit.Gruponomb = URS.GrupoNomb;
                            dtoTermsuperavit.Vcrtsfecha = dDiaCursor; //Información de un dia.
                            dtoTermsuperavit.Vcrtsmpa = dMPd;//dMPa;
                            dtoTermsuperavit.Vcrtsdefmwe = dTotDT;
                            dtoTermsuperavit.Vcrtssupmwe = dTotST;
                            dtoTermsuperavit.Vcrtsrnsmwe = dTotRNS;
                            dtoTermsuperavit.Vcrtsdeficit = dDeficit;
                            dtoTermsuperavit.Vcrtssuperavit = dSuperavit;
                            dtoTermsuperavit.Vcrtsresrvnosumn = dReservaNoSuministrada;
                            dtoTermsuperavit.Vcrtsusucreacion = suser;
                            this.SaveVcrTermsuperavit(dtoTermsuperavit);

                            #region listaSuperavit
                            //Superavit: Magnitud de Superávit de Reserva (reporte de la SEV). En MW.
                            //List<VcrVersuperavitDTO> listaSuperavit = this.ListVcrVersuperavitsDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor);
                            List<VcrVersuperavitDTO> listaSuperavit = this.ListVcrVersuperavitsDiaURS(dDiaCursor);
                            foreach (VcrVersuperavitDTO dtoSuperavit in listaSuperavit)
                            {
                                DateTime dfHoraInicio = (DateTime)dtoSuperavit.Vcrvsahorinicio;
                                DateTime dfHoraFinal = (DateTime)dtoSuperavit.Vcrvsahorfinal;

                                int dHoraF = dfHoraFinal.Hour;
                                int dMinuteF = dfHoraFinal.Minute;
                                //int dSegundoF = dfHoraFinal.Second;

                                decimal dHF_HI = 0;
                                if (dHoraF == 23 && dMinuteF == 59)
                                {
                                    DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                    dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60 / 24);
                                }
                                else
                                {
                                    dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                }

                                //dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                dTotSTURS += dtoSuperavit.Vcrvsasuperavit * dHF_HI;
                            }
                            //dSuperavit = dTotST * dMPd * dKc / 30;

                            #region ST = 0
                            if (dTotSTURS == 0)
                            {
                                //RNS
                                List<VcrVerrnsDTO> listaRNS = this.ListVcrVerrnssDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor, ConstantesCompensacionRSF.TipoCargaSubir);
                                foreach (VcrVerrnsDTO dtoRNS in listaRNS)
                                {
                                    DateTime dfHoraInicio = (DateTime)dtoRNS.Vcvrnshorinicio;
                                    DateTime dfHoraFinal = (DateTime)dtoRNS.Vcvrnshorfinal;

                                    int dHoraF = dfHoraFinal.Hour;
                                    int dMinuteF = dfHoraFinal.Minute;
                                    //int dSegundoF = dfHoraFinal.Second;

                                    decimal dHF_HI = 0;
                                    if (dHoraF == 23 && dMinuteF == 59)
                                    {
                                        DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                        dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }
                                    else
                                    {
                                        dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }

                                    dTotRNS += -1 * dtoRNS.Vcvrnsrns * dHF_HI;
                                }

                                //Calculo de DT y Reserva no suminstrada.
                                List<VcrVerdeficitDTO> listaDeficit = this.ListVcrVerdeficitsDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor);
                                foreach (VcrVerdeficitDTO dtoDeficit in listaDeficit)
                                {
                                    DateTime dfHoraInicio = (DateTime)dtoDeficit.Vcrvdehorinicio;
                                    DateTime dfHoraFinal = (DateTime)dtoDeficit.Vcrvdehorfinal;

                                    int dHoraF = dfHoraFinal.Hour;
                                    int dMinuteF = dfHoraFinal.Minute;
                                    //int dSegundoF = dfHoraFinal.Second;

                                    decimal dHF_HI = 0;
                                    if (dHoraF == 23 && dMinuteF == 59)
                                    {
                                        DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                        dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }
                                    else
                                    {
                                        dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }

                                    //decimal dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    dTotDT += -1 * dtoDeficit.Vcrvdedeficit * dHF_HI;

                                }
                                dReservaNoSuministrada = dTotRNS * dPAO / 30;
                                //Deficit del dia
                                dDeficit = 0;

                                //Superavit del dia
                                dSuperavit = 0;

                                //EXISTE EL REGISTRO DEL TERMINO SUPERAVIT en VCR_TERMSUPERAVIT
                                VcrTermsuperavitDTO dtoTermsuperavita = this.GetByIdVcrTermsuperavitDia(vcrecacodi, URS.GrupoCodi, dDiaCursor);
                                if (dtoTermsuperavit != null)
                                {   //actualizamos Vcrtssupmwe
                                    decimal dMWeq = (dTotDT - dTotRNS);
                                    dtoTermsuperavita.Vcrtsdefmwe = dMWeq;
                                    dtoTermsuperavita.Vcrtssupmwe = dTotST;
                                    dtoTermsuperavita.Vcrtsrnsmwe = dTotRNS;
                                    dtoTermsuperavita.Vcrtsdeficit = dDeficit;
                                    dtoTermsuperavita.Vcrtssuperavit = dSuperavit;
                                    dtoTermsuperavita.Vcrtsresrvnosumn = dReservaNoSuministrada;
                                    this.UpdateVcrTermsuperavit(dtoTermsuperavita);
                                }

                            }
                            #endregion

                            #region ST != 0
                            else
                            {
                                //Calculando el RNS y la Reserva no Suministrada
                                List<VcrVerrnsDTO> listaRNS = this.ListVcrVerrnssDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor, ConstantesCompensacionRSF.TipoCargaSubir);
                                foreach (VcrVerrnsDTO dtoRNS in listaRNS)
                                {
                                    DateTime dfHoraInicio = (DateTime)dtoRNS.Vcvrnshorinicio;
                                    DateTime dfHoraFinal = (DateTime)dtoRNS.Vcvrnshorfinal;

                                    int dHoraF = dfHoraFinal.Hour;
                                    int dMinuteF = dfHoraFinal.Minute;
                                    //int dSegundoF = dfHoraFinal.Second;

                                    decimal dHF_HI = 0;
                                    if (dHoraF == 23 && dMinuteF == 59)
                                    {
                                        DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                        dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }
                                    else
                                    {
                                        dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }

                                    //decimal dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    dTotRNS += dtoRNS.Vcvrnsrns * dHF_HI;
                                }
                                dReservaNoSuministrada = -1 * dTotRNS * dPAO / 30;

                                //Calculando el Deficit
                                List<VcrVerdeficitDTO> listaDeficit = this.ListVcrVerdeficitsDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor);
                                foreach (VcrVerdeficitDTO dtoDeficit in listaDeficit)
                                {
                                    DateTime dfHoraInicio = (DateTime)dtoDeficit.Vcrvdehorinicio;
                                    DateTime dfHoraFinal = (DateTime)dtoDeficit.Vcrvdehorfinal;

                                    int dHoraF = dfHoraFinal.Hour;
                                    int dMinuteF = dfHoraFinal.Minute;
                                    //int dSegundoF = dfHoraFinal.Second;

                                    decimal dHF_HI = 0;
                                    if (dHoraF == 23 && dMinuteF == 59)
                                    {
                                        DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                        dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }
                                    else
                                    {
                                        dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }

                                    //decimal dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    dTotDT += dtoDeficit.Vcrvdedeficit * dHF_HI;

                                }
                                //dDeficit = ((dTotDT * dMPd * dKc / 30) + dReservaNoSuministrada);
                                List<VcrVersuperavitDTO> listaSuperavitURS = this.ListVcrVersuperavitsDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor);
                                foreach (VcrVersuperavitDTO dtoSuperavit in listaSuperavitURS)
                                {
                                    DateTime dfHoraInicio = (DateTime)dtoSuperavit.Vcrvsahorinicio;
                                    DateTime dfHoraFinal = (DateTime)dtoSuperavit.Vcrvsahorfinal;

                                    int dHoraF = dfHoraFinal.Hour;
                                    int dMinuteF = dfHoraFinal.Minute;
                                    //int dSegundoF = dfHoraFinal.Second;

                                    decimal dHF_HI = 0;
                                    if (dHoraF == 23 && dMinuteF == 59)
                                    {
                                        DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                        dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }
                                    else
                                    {
                                        dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    }

                                    //decimal dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                    dTotST += dtoSuperavit.Vcrvsasuperavit * dHF_HI;
                                }
                                dSuperavit = dTotST * dMPd * dKc / 30;
                                #region Grabando Registros TerminosSuperavit
                                VcrTermsuperavitDTO dtoTermsuperavitb = this.GetByIdVcrTermsuperavitDia(vcrecacodi, URS.GrupoCodi, dDiaCursor);
                                if (dtoTermsuperavit != null)
                                {   //actualizamos
                                    decimal dMWeq = -1 * (dTotDT - dTotRNS);
                                    dDeficit = (dMWeq * dMPd * dKc) / 30;
                                    dtoTermsuperavitb.Vcrtsdefmwe = dMWeq;
                                    dtoTermsuperavitb.Vcrtssupmwe = dTotST;
                                    dtoTermsuperavitb.Vcrtsrnsmwe = dTotRNS;
                                    dtoTermsuperavitb.Vcrtsdeficit = dDeficit;
                                    dtoTermsuperavitb.Vcrtssuperavit = dSuperavit;
                                    dtoTermsuperavitb.Vcrtsresrvnosumn = dReservaNoSuministrada;
                                    this.UpdateVcrTermsuperavit(dtoTermsuperavitb);
                                }
                                #endregion
                            }

                            #endregion

                            #endregion
                        }
                        #region Solo si hay periodo dHF_HI en este dDiaCursor

                        #endregion
                        dDiaCursor = dDiaCursor.AddDays(1);
                    }//end para cada dia -> dDiaCursor
                }//end if dtoVersionDSRN != null
                #endregion

                //ASSETEC 20190115
                #region Costo total del Servicio RSF – CU09.04.01
                //Para cada Dia dentro del periodo calculo el Costo total del Servicio RSF
                decimal[] aServicioRSF = new decimal[40];
                decimal dTotalServicioRSF = 0;
                decimal dTotalServicioRSFsinRNS = 0;
                DateTime dVcsRsfFecha = dFecInicio;
                while (dVcsRsfFecha <= dFecFin)
                {
                    //CT(RSF,d) = ∑Asignación de Reserva(urs,d) + ∑Costo de Oportunidad(urs,d) + ∑COMP(urs,d) + Reserva No Suministrada(urs,d) --> antes - Reserva No Suministrada(urs,d) 
                    VcrServiciorsfDTO dtoServiciors = this.GetByIdVcrServiciorsfValoresDia(vcrecacodi, dVcsRsfFecha);
                    if (dtoServiciors != null)
                    {
                        //Grabamos el registro en VCR_SERVICIORSF
                        dtoServiciors.Vcrecacodi = vcrecacodi;
                        dtoServiciors.Vcsrscostotservrsf = dtoServiciors.Vcsrsfasignreserva + dtoServiciors.Vcsrsfcostportun + dtoServiciors.Vcsrsfcostotcomps + dtoServiciors.Vcsrsfresvnosumn; //20190115:Cambio en el ultimo signo
                        dtoServiciors.VcsrscostotservrsfSRns = dtoServiciors.Vcsrsfasignreserva + dtoServiciors.Vcsrsfcostportun + dtoServiciors.Vcsrsfcostotcomps; //aqui no se suma 𝑅𝑒𝑠𝑒𝑟𝑣𝑎 𝑁𝑜 𝑆𝑢𝑚𝑖𝑛𝑖𝑠𝑡𝑟𝑎𝑑a
                        dtoServiciors.Vcsrsffecha = dVcsRsfFecha;
                        dtoServiciors.Vcsrsfusucreacion = suser;
                        this.SaveVcrServiciorsf(dtoServiciors);
                        aServicioRSF[dVcsRsfFecha.Day] = dtoServiciors.Vcsrscostotservrsf; //CT(RSF,d)
                        dTotalServicioRSF += dtoServiciors.Vcsrscostotservrsf; //∑(d=1)^Nd CT(RSF,d)
                        dTotalServicioRSFsinRNS += dtoServiciors.VcsrscostotservrsfSRns; // Total del servicio para calculo de Cargo Incumplimiento
                    }
                    dVcsRsfFecha = dVcsRsfFecha.AddDays(1);
                }
                #endregion

                #region Asignación del Pago de RSF por cada Unidad de Generación del Sistema – CU09.04.02
                //Para cada Dia dentro del periodo calculo la Asignación de pago de RSF del unidad de generadcion “g” del día “d”
                DateTime dVcrAPFecha = dFecInicio;
                while (dVcrAPFecha <= dFecFin)
                {
                    //Para el Dia traemos la lista de Medicion de Bornes de cada Unidad de generación, exceptuando las unidades exoneradas del Pago de RSF
                    List<VcrMedborneDTO> listMedborneSinUnidExonRSF = this.ListVcrMedbornesDiaSinUnidExonRSF(vcrecacodi, dVcrAPFecha);
                    //Calculamos el Total ∑_g^Ng Pmed(g,d) :
                    decimal dTotalPmed = 0;
                    foreach (VcrMedborneDTO dtoMedborne in listMedborneSinUnidExonRSF)
                    {
                        dTotalPmed += dtoMedborne.Vcrmebpotenciamed;
                    }
                    if (dTotalPmed == 0) dTotalPmed = 1;
                    //Aplicamos: APRSF(g,d) = - [Pmed(g,d) / (∑_g^Ng Pmed(g,d) )] x CT(RSF,d)
                    foreach (VcrMedborneDTO dtoMedborne in listMedborneSinUnidExonRSF)
                    {
                        decimal dVcrapAsignPagoRSF = -(dtoMedborne.Vcrmebpotenciamed / dTotalPmed) * aServicioRSF[dVcrAPFecha.Day];
                        VcrAsignacionpagoDTO dtoAsignacionpago = new VcrAsignacionpagoDTO();
                        dtoAsignacionpago.Vcrecacodi = vcrecacodi;
                        dtoAsignacionpago.Emprcodi = dtoMedborne.Emprcodi;
                        dtoAsignacionpago.Equicodicen = dtoMedborne.Equicodicen;
                        dtoAsignacionpago.Equicodiuni = dtoMedborne.Equicodiuni;
                        dtoAsignacionpago.Vcrapfecha = dVcrAPFecha;
                        dtoAsignacionpago.Vcrapasignpagorsf = dVcrapAsignPagoRSF;
                        dtoAsignacionpago.Vcrapusucreacion = suser;
                        this.SaveVcrAsignacionpago(dtoAsignacionpago);
                    }
                    dVcrAPFecha = dVcrAPFecha.AddDays(1);
                }
                #endregion

                //ASSETEC 20190115 - en Formula de CargoINC(g,m): - 𝑅𝑒𝑠𝑒𝑟𝑣𝑎 𝑁𝑜 𝑆𝑢𝑚𝑖𝑛𝑖𝑠𝑡𝑟𝑎𝑑𝑎(𝑑) 
                #region Cargo por Incumplimiento de RPF – Preliminar – CU09.04.03
                //Traemos la lista de Unidades de generación del mes (g,m) - Potencia Media / y se calcula a partir de la medición en bornes con el atributo Considera = SI
                List<VcrMedborneDTO> listMedborneMes = this.ListVcrMedbornesMesConsiderados(vcrecacodi);//solo se suman de los que son considerados
                List<VcrMedborneDTO> listMedborne = this.ListVcrMedbornesMes(vcrecacodi);
                decimal dTotalVcrciCargoIncumpl = 0;
                decimal dTotalPmedMes = 0;
                //decimal dFactor = 0;
                //int dDiasMes = DateTime.DaysInMonth(dFecInicio.Year, dFecInicio.Month);
                //dFactor = (decimal)(dDiasMes*1.0)/24;
                foreach (VcrMedborneDTO dtoMedborne in listMedborne)
                {
                    //Calculamos el Total ∑(d=1)^Nd ∑g^Ng Pmed(g,d) : Sumatoria de las Potencias Medias de todas las Unidades de Generación
                    if (dtoMedborne.Emprcodi < 0)
                        continue; //ASSETEC 20191209
                    dTotalPmedMes += dtoMedborne.Vcrmebpotenciamed;
                }
                if (dTotalPmedMes == 0) dTotalPmedMes = 1;

                //Aplicamos la Formula de CargoINC(g,m) = [ ∑(d=1)^Nd (CT(RSF,d) - 𝑅𝑒𝑠𝑒𝑟𝑣𝑎 𝑁𝑜 𝑆𝑢𝑚𝑖𝑛𝑖𝑠𝑡𝑟𝑎𝑑𝑎(𝑑))/15  * (∑(d=1)^Nd Pmed(g,d)/∑(d=1)^Nd ∑g^Ng Pmed(g,d)) * ∑(d=1)^Nd INC(g,d) ] + Saldo(m-1)
                foreach (VcrMedborneDTO dtoMedborne in listMedborneMes)
                {
                    if (dtoMedborne.Emprcodi < 0)
                        continue;
                    //Para cada dtoMedborne.Equicodiuni
                    //Calculamos: ∑(d=1)^Nd INC(g,d) : Cantidad de Incumplimientos a la RPF de la Unidad de Generación “g” durante el mes “m”
                    VcrVerincumplimDTO dtoVerincumplim = this.GetByIdVcrVerincumplimPorUnidad((int)EntidadRecalculo.Vcrinccodi, dtoMedborne.Equicodiuni, dtoMedborne.Equicodicen);
                    //Traemos el saldo del mes anterior referente a la unidad
                    decimal dTotalSaldoAnterior = 0;
                    int dAnioMesPeriodo = EntidadPeriodo.PeriAnioMes;
                    int dAnioMesPeriodoAnterior = dAnioMesPeriodo - 1;
                    //ASSETEC 20181128--------------------------------------------------------------------
                    //dAnioMesPeriodo termina 01 que es enero, ejemplo 201801 => dAnioMesPeriodoAnterior es 201800
                    int iMes = dAnioMesPeriodoAnterior % 100;
                    if (iMes == 0)
                    {
                        //Corrigiendo Mes anterior (201800)
                        dAnioMesPeriodoAnterior = (dAnioMesPeriodoAnterior - 100) + 12; //dAnioMesPeriodoAnterior es 201700 + 12 = 201712
                    }
                    if (dAnioMesPeriodoAnterior > 0)
                    {
                        var EntidadPeriodoAnterior = this.servicioPeriodo.GetByAnioMes(dAnioMesPeriodoAnterior);
                        var RecalculoAnterior = this.ListVcrRecalculos(EntidadPeriodoAnterior.PeriCodi);
                        if (RecalculoAnterior.Count() != 0)
                        {
                            var UltimoRecalculoAnterior = this.ListVcrRecalculos(EntidadPeriodoAnterior.PeriCodi).OrderByDescending(x => x.Vcrecaversion).First();
                            var EntidadSaldoMesAnterior = this.GetByIdVcrCargoincumpl(UltimoRecalculoAnterior.Vcrecacodi, dtoMedborne.Equicodiuni);
                            if (EntidadSaldoMesAnterior != null)
                            {
                                dTotalSaldoAnterior = EntidadSaldoMesAnterior.Vcrcisaldomes;
                            }
                            else
                            {
                                dTotalSaldoAnterior = 0;
                            }
                        }
                    }
                    //--------------------------------------------------------------------
                    //ASSETEC 20190228
                    //Recordar que: ∑(d=1)^Nd Pmed(g,d) == dtoMedborne.Vcrmebpotenciamed
                    //decimal dVcrciCargoIncumplmes = (-1 * ((((dTotalServicioRSF - dTotalServicioRSFsinRNS) / 15) * (dtoMedborne.Vcrmebpotenciamed / dTotalPmedMes) * dtoVerincumplim.Vcrvincumpli))) + dTotalSaldoAnterior;

                    decimal dVcrciCargoIncumplmes = (-1 * ((((dTotalServicioRSFsinRNS) / 15) * (dtoMedborne.Vcrmebpotenciamed / dTotalPmedMes) * dtoVerincumplim.Vcrvincumpli))) + dTotalSaldoAnterior;
                    //Grabamos VCR_CARGOINCUMPL
                    VcrCargoincumplDTO dtoCargoincumpl = new VcrCargoincumplDTO();
                    dtoCargoincumpl.Vcrecacodi = vcrecacodi;
                    dtoCargoincumpl.Equicodi = dtoMedborne.Equicodiuni; //Ojo, aqui esta Emprcodi y Equicodicen
                    dtoCargoincumpl.Vcrcicargoincumplmes = dVcrciCargoIncumplmes - dTotalSaldoAnterior;//dVcrciCargoIncumplmes;
                    dtoCargoincumpl.Vcrcisaldoanterior = dTotalSaldoAnterior;
                    dtoCargoincumpl.Vcrcicargoincumpl = dVcrciCargoIncumplmes;//dVcrciCargoIncumplmes + dTotalSaldoAnterior;
                    dtoCargoincumpl.Vcrcicarginctransf = -1 * (dVcrciCargoIncumplmes); //+ dTotalSaldoAnterior);
                    dtoCargoincumpl.Vcrcisaldomes = 0;
                    dtoCargoincumpl.VcrcisaldomesAnterior = dTotalSaldoAnterior;
                    dtoCargoincumpl.Pericodidest = 0; //EntidadRecalculo.Vcrecacodidestino;
                    dtoCargoincumpl.Vcrciusucreacion = suser;
                    this.SaveVcrCargoincumpl(dtoCargoincumpl);

                    dTotalVcrciCargoIncumpl += dVcrciCargoIncumplmes; //+ dTotalSaldoAnterior);
                }
                #endregion

                #region Reducción del Pago por RSF ejecutado – CU09.04.04

                decimal dTotalVcrpeReduccPagoMax = 0;
                //Traemos la lista de Unidades de generación Considerados en el pago de RSF del mes (g,m), Esta Reducción de pago es aplicable a la Unidad realice o no RPF - TODOS
                List<VcrMedborneDTO> listMedborneMesConsiderados = this.ListVcrMedbornesMesConsiderados(vcrecacodi);
                foreach (VcrMedborneDTO dtoMedborne in listMedborneMesConsiderados)
                {
                    if (dtoMedborne.Emprcodi < 0)
                        continue;
                    //Calculamos: ∑(d=1)^Nd INC(g,d) : Cantidad de Incumplimientos a la RPF de la Unidad de Generación “g” durante el mes “m”
                    VcrVerincumplimDTO dtoVerincumplim = this.GetByIdVcrVerincumplimPorUnidad((int)EntidadRecalculo.Vcrinccodi, dtoMedborne.Equicodiuni, dtoMedborne.Equicodicen);

                    //Cumplimiento del mes: CUMP(g,m) = 1 - (∑(d=1)^Nd INC(g,d)) / Nd 
                    decimal dVcrpeCumplMes = 1 - dtoVerincumplim.Vcrvincumpli / iNumeroDiasMes;

                    //Obtenemos: ∑(d=1)^Nd APRSF(g,d) para la unidad de generación en el mes
                    VcrAsignacionpagoDTO dtoAsignacionpago = this.GetByIdVcrAsignacionpagoMesUnidad(vcrecacodi, dtoMedborne.Equicodiuni);
                    if (dVcrpeCumplMes >= dFactorCumplimiento)
                    {
                        dVcrpeCumplMes = 1;
                    }
                    //Reducción del Pago Máximo:
                    decimal dVcrpeReduccPagoMax = 0;
                    if (dtoAsignacionpago != null)
                    {
                        if (dVcrpeCumplMes < dFactorCumplimiento)
                        {
                            //ReduccPagoMáx(g,m) = (-CUMP(g,m)) x ∑(d=1)^Nd APRSF(g,d) 
                            dVcrpeReduccPagoMax = -1 * dVcrpeCumplMes * dtoAsignacionpago.Vcrapasignpagorsf;
                        }
                        else
                        {
                            //Si CUMP(g,m) ≥ 0.85
                            //ReduccPagoMáx(g,m) = (-) * ∑(d=1)^Nd APRSF(g,d) 
                            dVcrpeReduccPagoMax = -1 * dtoAsignacionpago.Vcrapasignpagorsf;
                        }
                    }
                    //Grabamos VCR_REDUCCPAGOEJE
                    VcrReduccpagoejeDTO dtoReduccpagoeje = new VcrReduccpagoejeDTO();
                    dtoReduccpagoeje.Vcrecacodi = vcrecacodi;
                    dtoReduccpagoeje.Equicodi = dtoMedborne.Equicodiuni;
                    dtoReduccpagoeje.Vcrpecumplmes = dVcrpeCumplMes;
                    dtoReduccpagoeje.Vcrpereduccpagomax = dVcrpeReduccPagoMax;
                    dtoReduccpagoeje.Vcrpereduccpagoeje = dVcrpeReduccPagoMax;
                    dtoReduccpagoeje.Vcrpeusucreacion = suser;
                    this.SaveVcrReduccpagoeje(dtoReduccpagoeje);

                    dTotalVcrpeReduccPagoMax += dVcrpeReduccPagoMax;
                }
                //Reducción del Pago Ejecutado
                if (Math.Abs(dTotalVcrpeReduccPagoMax) < Math.Abs(dTotalVcrciCargoIncumpl))
                {
                    //No hacemos nada: dtoReduccpagoeje.Vcrpereduccpagoeje = dVcrpeReduccPagoMax;
                }
                else if (dTotalVcrpeReduccPagoMax > 0)
                {
                    List<VcrReduccpagoejeDTO> listReduccpagoeje = this.ListVcrReduccpagoejes(vcrecacodi);
                    foreach (VcrReduccpagoejeDTO dtoReduccpagoeje in listReduccpagoeje)
                    {
                        dtoReduccpagoeje.Vcrpereduccpagoeje = -1 * dtoReduccpagoeje.Vcrpereduccpagomax * dTotalVcrciCargoIncumpl / dTotalVcrpeReduccPagoMax;
                        this.UpdateVcrReduccpagoeje(dtoReduccpagoeje);
                    }
                }

                #endregion

                #region Cargo por Incumplimiento de RPF transferido – CU09.04.05
                //Reducción del Pago Ejecutado
                if (Math.Abs(dTotalVcrpeReduccPagoMax) >= Math.Abs(dTotalVcrciCargoIncumpl))
                {
                    //No se hace nada
                }
                else if (dTotalVcrciCargoIncumpl > 0)
                {
                    List<VcrCargoincumplDTO> listCargoincumpl = this.ListVcrCargoincumpls(vcrecacodi);
                    foreach (VcrCargoincumplDTO dtoCargoincumpl in listCargoincumpl)
                    {
                        //dtoCargoincumpl.Vcrpereduccpagomax = 1;
                        //dtoCargoincumpl.Vcrcicargoincumpl = 1;
                        //dtoCargoincumpl.Vcrcicarginctransf = 1;
                        dtoCargoincumpl.Vcrcicarginctransf = dtoCargoincumpl.Vcrcicargoincumpl * dTotalVcrpeReduccPagoMax / dTotalVcrciCargoIncumpl;
                        dtoCargoincumpl.Vcrcisaldomes = dtoCargoincumpl.Vcrcicargoincumpl + dtoCargoincumpl.Vcrcicarginctransf;
                        this.UpdateVcrCargoincumpl(dtoCargoincumpl);
                    }
                }
                #endregion

                #region Pagos por RSF – CU09.04.06
                //En listMedborneMes, tenemos la lista de Unidades de Generación del Mes (g,m)
                List<VcrMedborneDTO> listMedborneSinUnidExonRSFAA = this.ListMesConsideradosTotales(vcrecacodi);
                foreach (VcrMedborneDTO dtoMedborneUnidadGeeracion in listMedborneSinUnidExonRSFAA)
                {
                    //En base de datos aplicamos la formula: PagoRSF(g,m)= ∑(d=1)^Nd APRSF(g,d) + ReduccPagoEjec(g,m)
                    VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfUnidad2020(vcrecacodi, dtoMedborneUnidadGeeracion.Equicodiuni);
                    if (dtoPagorsf != null)
                    {
                        dtoPagorsf.Vcrecacodi = vcrecacodi;
                        dtoPagorsf.Equicodi = dtoMedborneUnidadGeeracion.Equicodiuni;
                        dtoPagorsf.Vcprsfusucreacion = suser;
                        this.SaveVcrPagorsf(dtoPagorsf);
                    }
                }
                #endregion

                //ASSETEC 20190115
                #region Para los Reportes – CU09.04.07
                //Traemos la lista de empresa a partir de VcrAsignacionpago

                List<VcrAsignacionpagoDTO> listAsignacionpagoEmpresa = this.ListVcrAsignacionpagosEmpresaMes(vcrecacodi);
                foreach (VcrAsignacionpagoDTO dtoEmpresa in listAsignacionpagoEmpresa)
                {
                    VcrEmpresarsfDTO dtoEmpresarsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, dtoEmpresa.Emprcodi);

                    if (dtoEmpresa.Emprcodi < 0)
                    {
                        VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfUnidad2020(vcrecacodi, -1); //aplica para la empresa -1002: MINERA CERRO VERDE - GU
                        dtoEmpresarsf.Vcersfpagorsf = dtoPagorsf.Vcprsfpagorsf;
                    }
                    dtoEmpresarsf.Emprcodi = dtoEmpresa.Emprcodi;
                    dtoEmpresarsf.Vcrecacodi = vcrecacodi;
                    dtoEmpresarsf.Vcersfusucreacion = suser;
                    this.SaveVcrEmpresarsf(dtoEmpresarsf);
                }
                #endregion

            }
            catch (Exception e)
            {
                sResultado = e.StackTrace;
                sResultado = e.Message; //"-1";
            }
            return sResultado;
        }

        /// <summary>
        /// Procedimiento que se encarga de ejecutar el procedimiento del calculo de RSF del periodo 2021.01 en adelante 
        /// </summary>
        /// <param name="pericodi">Periodo de calculo</param>
        /// <param name="vcrecacodi">Version de calculo</param>
        /// <param name="suser">Usuario conectado</param>
        public string ProcesarCalculo(int pericodi, int vcrecacodi, string suser)
        {
            string sResultado = "1";
            try
            {
                //Limpiamos Todos los calculos anteriores de la Version en Acción
                string sBorrar = this.EliminarCalculo(vcrecacodi);
                if (!sBorrar.Equals("1"))
                {
                    sResultado = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                    return sResultado;
                }
                //INICIALIZAMOS ALGUNAS VARIABLES GENERALES
                //Traemos la entidad de la versión de recalculo
                VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
                if (EntidadRecalculo.Vcrdsrcodi == null || EntidadRecalculo.Vcrdsrcodi == 0)
                {
                    sResultado = "Lo sentimos, La version de calculo no tiene asignado una version de Déficit, Superávit y Reserva No Suministrada";
                    return sResultado;
                }
                if (EntidadRecalculo.Vcrinccodi == null || EntidadRecalculo.Vcrinccodi == 0)
                {
                    sResultado = "Lo sentimos, La version de calculo no tiene asignado una version de incumplimiento";
                    return sResultado;
                }
                //FECHAS DE INICIO FINAL DEL MES
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string sPeriodo = EntidadPeriodo.AnioCodi.ToString() + sMes; //YYYYMM
                //Valores generales
                decimal dUnoEntre24 = (decimal)1 / 24;
                //decimal dFactorCumplimiento = 0.85M;
                int iNumReg = 0;

                //INICIAMOS EL PROCESO DE CALCULO
                //Lista de URS
                List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURSCostoMarginal(pericodi, EntidadRecalculo.Recacodi);

                //ASSETEC 20190115
                #region Asignación de reserva (AR) – CU09.01
                //El proceso de cálculo para valorizar el costo del servicio por la Asignación de Reserva es el siguiente: 
                foreach (TrnBarraursDTO URS in listaURS)
                {
                    //PBF_URS: Potencia que corresponde a la Provisión Base Firme de la URS. 
                    //VCRPBPOTENCIABF -> Precio Base Firme Subir (PBF)
                    //VCRPBPOTENCIABFB-> Potencia Base Firme Bajar (PBFb)
                    //Si la URS no posee Provisión Base Firme se considerará este valor como cero. Corresponde al valor indicado en el CU09.01. En MW.
                    decimal dProvisionBaseFirmeSubirURS = 0;    //Subida
                    decimal dProvisionBaseFirmeBajarURS = 0;   //Bajada
                    decimal dPrecioPotenciaBaseFirmeSubirURS = 0;
                    decimal dPrecioPotenciaBaseFirmeBajarURS = 0;
                    VcrProvisionbaseDTO dtoProvisionBase = this.GetByIdVcrProvisionbaseURS(URS.GrupoCodi, sPeriodo); //Para una URS
                    if (dtoProvisionBase != null)
                    {
                        //Existe provisión base para esta URS
                        dProvisionBaseFirmeSubirURS = dtoProvisionBase.Vcrpbpotenciabf; //PBFi - Subida
                        dProvisionBaseFirmeBajarURS = dtoProvisionBase.Vcrpbpotenciabfb; //PBFbi - Bajada
                        dPrecioPotenciaBaseFirmeSubirURS = dtoProvisionBase.Vcrpbpreciobf; //Precio_PBFi - Subida
                        dPrecioPotenciaBaseFirmeBajarURS = dtoProvisionBase.Vcrpbpreciobfb; //Precio_PBFi - Bajada
                    }

                    //Para cada URS se calcula Asignación de Reserva de una URS en un Dia
                    DateTime dFecha = dFecInicio;
                    //decimal dHF_HI1 = (decimal)(dHoraFinal1).TotalHours;
                    while (dFecha <= dFecFin)
                    {
                        decimal dRA_PBF = 0;
                        decimal dRA_PBFBajar = 0;
                        decimal dRA_MA = 0;
                        decimal dRA_MABajar = 0;
                        decimal dMPA_id = 0; //Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-dia) para este URS
                        decimal dMPA_idBajar = 0;

                        //Lista de Asignacion de reserva para poder ingresar el correcto valor de la oferta maxima del dia.
                        //RA (URS,t): Reserva Asignada en el periodo “t” (comprendido desde HI a HF) de la URS.
                        #region PROCESANDO LA RESERVA ASIGNADA DE SUBIDA Y BAJADA
                        List<VcrReservasignDTO> listaReservaAsignada = this.GetByCriteriaVcrReservasignsURSDia(vcrecacodi, URS.GrupoCodi, dFecha);
                        foreach (VcrReservasignDTO dtoReservaAsinada in listaReservaAsignada)
                        {   //Para cada intervalo Ti 
                            DateTime dHoraFinal = (DateTime)dtoReservaAsinada.Vcrasghorfinal; //OJO minimo es OO:OO max: 23:59
                            DateTime dHoraInicio = (DateTime)dtoReservaAsinada.Vcrasghorinicio;
                            //LAS HORAS ESTAN EN FORMATO FECHA HAY QUE LLEVARLOS A HORAS / 24 
                            decimal dHF_HI = (decimal)(dHoraFinal - dHoraInicio).TotalHours;
                            //SUBIDA--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            decimal dDiferencia = dtoReservaAsinada.Vcrasgreservasign - dProvisionBaseFirmeSubirURS; //RA(urs,ti) - PBF(urs)
                            if (dDiferencia > 0)
                            {
                                //1.- RA_PBF = PBF_URS x (HF-HI) x 1/24 
                                dRA_PBF += dProvisionBaseFirmeSubirURS * dHF_HI * dUnoEntre24; //dRA_PBF: Reserva Asignada correspondiente a la Provisión Base Firme
                                //2.- RA_MA = (RA(urs,t) - PBF(urs)) x (HF-HI) x 1/24
                                dRA_MA += dDiferencia * dHF_HI * dUnoEntre24; //Math.Round(dDiferencia) * dHF_HI * dUnoEntre24;   //dRA_MA: Reserva Asignada correspondiente al Mercado de Ajuste

                                //Buscamos el Precio en las ofertas para calcular el MPA(i,d) = max𝑖,𝑑(𝑅𝐴𝑖,𝑑,𝑡𝑖1 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥1; 𝑅𝐴𝑖,𝑑,𝑡𝑖2 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥2, … ; 𝑅𝐴𝑖,𝑑,𝑡𝑖𝑛× 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥𝑛) ,∀ 𝑡𝑥n
                                decimal dPoF_idt = this.GetOfertaMaxDiaGrupoCodiHiHf(vcrecacodi, URS.GrupoCodi, dFecha, dHoraInicio, dHoraFinal, ConstantesCompensacionRSF.TipoCargaSubir);
                                //Tomando la mayor oferta dMPA_id
                                if (dMPA_id < dPoF_idt)
                                    dMPA_id = dPoF_idt;
                            }
                            else
                            {
                                //Si: RA(urs,t) - PBF(urs) ≤ 0
                                //1.- RA_PBF = RA(urs,t) x (HF-HI) x 1/24
                                dRA_PBF += dtoReservaAsinada.Vcrasgreservasign * dHF_HI * dUnoEntre24;
                                //2.- RA_MA=0
                                dRA_MA += 0;
                            }
                            //BAJADA--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            dDiferencia = dtoReservaAsinada.Vcrasgreservasignb - dProvisionBaseFirmeBajarURS; //RA(urs,ti) - PBF(urs)
                            if (dDiferencia > 0)
                            {
                                //1.- RA_PBF = PBF_URS x (HF-HI) x 1/24 
                                dRA_PBFBajar += dProvisionBaseFirmeBajarURS * dHF_HI * dUnoEntre24; //dRA_PBFBajar: Reserva Asignada correspondiente a la Provisión Base Firme
                                //2.- RA_MA = (RA(urs,t) - PBF(urs)) x (HF-HI) x 1/24
                                dRA_MABajar += dDiferencia * dHF_HI * dUnoEntre24; //Math.Round(dDiferencia) * dHF_HI * dUnoEntre24;   //dRA_MABajar: Reserva Asignada correspondiente al Mercado de Ajuste

                                //Buscamos el Precio en las ofertas para calcular el MPA(i,d) = max𝑖,𝑑(𝑅𝐴𝑖,𝑑,𝑡𝑖1 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥1; 𝑅𝐴𝑖,𝑑,𝑡𝑖2 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥2, … ; 𝑅𝐴𝑖,𝑑,𝑡𝑖𝑛× 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥𝑛) ,∀ 𝑡𝑥n
                                decimal dPoF_idt = this.GetOfertaMaxDiaGrupoCodiHiHf(vcrecacodi, URS.GrupoCodi, dFecha, dHoraInicio, dHoraFinal, ConstantesCompensacionRSF.TipoCargaBajar);
                                //Tomando la mayor oferta dMPA_idBajar
                                if (dMPA_idBajar < dPoF_idt)
                                    dMPA_idBajar = dPoF_idt;
                            }
                            else
                            {
                                //Si: RA(urs,t) - PBF(urs) ≤ 0
                                //1.- RA_PBF = RA(urs,t) x (HF-HI) x 1/24
                                dRA_PBFBajar += dtoReservaAsinada.Vcrasgreservasignb * dHF_HI * dUnoEntre24;
                                //2.- RA_MA=0
                                dRA_MABajar += 0;
                            }
                        }
                        #endregion

                        //Guardando la Asignación de Reserva (urs,dia) 1 URS / 1 DIA
                        VcrAsignacionreservaDTO dtoAsignacionreserva = new VcrAsignacionreservaDTO();
                        dtoAsignacionreserva.Vcrecacodi = vcrecacodi;
                        dtoAsignacionreserva.Grupocodi = URS.GrupoCodi;
                        dtoAsignacionreserva.Gruponomb = URS.GrupoNomb;
                        dtoAsignacionreserva.Vcrarfecha = dFecha;
                        dtoAsignacionreserva.Vcrarusucreacion = suser;
                        //Calculo la Asignación de Reserva(urs,dia): 
                        dtoAsignacionreserva.Vcrarrapbf = dRA_PBF;
                        dtoAsignacionreserva.Vcrarrapbfbajar = dRA_PBFBajar;
                        dtoAsignacionreserva.Vcrarprbf = dPrecioPotenciaBaseFirmeSubirURS;
                        dtoAsignacionreserva.Vcrarprbfbajar = dPrecioPotenciaBaseFirmeBajarURS;
                        dtoAsignacionreserva.Vcrarrama = dRA_MA;
                        dtoAsignacionreserva.Vcrarramabajar = dRA_MABajar;
                        dtoAsignacionreserva.Vcrarmpa = dMPA_id; //Subida -> Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-dia) para este URS
                        dtoAsignacionreserva.Vcrarmpabajar = dMPA_idBajar; //Bajada
                        dtoAsignacionreserva.Vcrarramaursra = 0; // Subida -> Reserva Asignada correspondiente al Mercado de Ajuste entre todos los URS que estan en reserva asignada
                        dtoAsignacionreserva.Vcrarramaursrabajar = 0; // Bajada
                        dtoAsignacionreserva.Vcrarasignreserva = 0;// (dRA_PBF * dPrecioPotenciaBaseFirmeURS) / 30 + (dRA_MA * dMPAM) / 30; Lo vamos a registrar despues, cuando ya se tenga el dMPAM de todos los URS en el día

                        this.SaveVcrAsignacionreserva(dtoAsignacionreserva);
                        //Cambiamos de fecha
                        dFecha = dFecha.AddDays(1);
                    }
                    //Cambiamos de URS
                }
                //Actualizando la tabla AsignacionReserva:
                //El valor de la Reserva Asignada correspondiente al Mercado de Ajuste entre todos los URS
                DateTime dDia = dFecInicio;
                while (dDia <= dFecFin)
                {
                    //max(Vcrarmpa): dMPA_id -> Mayor Precio Asignado de Ofertas del Mercado de Ajuste del día “d” (S/./MWmes) entre todos los URS de ese día
                    VcrAsignacionreservaDTO dtoAR = this.GetByVcrAsignacionreservadMPA(vcrecacodi, dDia); //retorna max(Vcrarmpa) de subida dMPA_d / bajada
                    List<VcrAsignacionreservaDTO> ListaAsignacionReserva = this.GetByCriteriaVcrAsignacionReservaOferta(vcrecacodi, dDia);
                    foreach (var item in ListaAsignacionReserva)
                    {
                        item.Vcrarramaursra = dtoAR.Vcrarmpa; // subida-> dMPA_d;
                        item.Vcrarramaursrabajar = dtoAR.Vcrarmpabajar;
                        //item.Vcrarasignreserva = (item.Vcrarrapbf * item.Vcrarprbf) / 30 + (item.Vcrarrama * dMPA_d) / 30;
                        //(∑_ti^Nti▒〖RA〗_(PBF,ti subir) ×PrBF subir+)/30  (∑_ti^Nti▒〖RA〗_(PBF,ti bajar)   ×PrBFbajar)/30 
                        item.Vcrarasignreserva = (item.Vcrarrapbf * item.Vcrarprbf) / 30 + (item.Vcrarrapbfbajar * item.Vcrarprbfbajar) / 30;
                        //(∑_ti^Nti▒〖RA〗_(MA,ti subir) ×〖MPA〗_d subir)/30+(∑_ti^Nti▒〖RA〗_(MA,ti bajar) ×〖MPA〗_d  bajar)/30
                        item.Vcrarasignreserva += (item.Vcrarrama * item.Vcrarramaursra) / 30 + (item.Vcrarramabajar * item.Vcrarramaursrabajar) / 30;
                        this.UpdateVcrAsignacionreserva(item);
                    }
                    dDia = dDia.AddDays(1);
                }

                #endregion

                #region Costo de Oportunidad (CO) – CU09.02
                //Lista de URS
                //Para cada URS se calcula su Costo Operativo
                foreach (TrnBarraursDTO URS in listaURS)
                {
                    TrnBarraursDTO dtoBarra = this.servicioBarraUrs.GetByIdUrs(URS.BarrCodi, URS.GrupoCodi);
                    DateTime dFecha = dFecInicio;
                    //Para cada dia se calcula el costo operativo de una URS
                    while (dFecha <= dFecFin)
                    {
                        decimal[] aCostoOportunidad = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosOportunidad
                        #region COSTO MARGINAL
                        //Lista de 48 registros Costos Marginales asociado a una URS
                        CostoMarginalDTO dtoCostoMarginal = this.servicioCostoMarginal.GetByIdCostoMarginalPorBarraDia(pericodi, EntidadRecalculo.Recacodi, dtoBarra.BarrCodi, dFecha.Day);
                        decimal[] aCostoMarginal = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosVariables
                        if (dtoCostoMarginal != null)
                        {
                            aCostoMarginal[0] = dtoCostoMarginal.CosMar1;
                            aCostoMarginal[1] = dtoCostoMarginal.CosMar3;
                            aCostoMarginal[2] = dtoCostoMarginal.CosMar5;
                            aCostoMarginal[3] = dtoCostoMarginal.CosMar7;
                            aCostoMarginal[4] = dtoCostoMarginal.CosMar9;
                            aCostoMarginal[5] = dtoCostoMarginal.CosMar11;
                            aCostoMarginal[6] = dtoCostoMarginal.CosMar13;
                            aCostoMarginal[7] = dtoCostoMarginal.CosMar15;
                            aCostoMarginal[8] = dtoCostoMarginal.CosMar17;
                            aCostoMarginal[9] = dtoCostoMarginal.CosMar19;
                            aCostoMarginal[10] = dtoCostoMarginal.CosMar21;
                            aCostoMarginal[11] = dtoCostoMarginal.CosMar23;
                            aCostoMarginal[12] = dtoCostoMarginal.CosMar25;
                            aCostoMarginal[13] = dtoCostoMarginal.CosMar27;
                            aCostoMarginal[14] = dtoCostoMarginal.CosMar29;
                            aCostoMarginal[15] = dtoCostoMarginal.CosMar31;
                            aCostoMarginal[16] = dtoCostoMarginal.CosMar33;
                            aCostoMarginal[17] = dtoCostoMarginal.CosMar35;
                            aCostoMarginal[18] = dtoCostoMarginal.CosMar37;
                            aCostoMarginal[19] = dtoCostoMarginal.CosMar39;
                            aCostoMarginal[20] = dtoCostoMarginal.CosMar41;
                            aCostoMarginal[21] = dtoCostoMarginal.CosMar43;
                            aCostoMarginal[22] = dtoCostoMarginal.CosMar45;
                            aCostoMarginal[23] = dtoCostoMarginal.CosMar47;
                            aCostoMarginal[24] = dtoCostoMarginal.CosMar49;
                            aCostoMarginal[25] = dtoCostoMarginal.CosMar51;
                            aCostoMarginal[26] = dtoCostoMarginal.CosMar53;
                            aCostoMarginal[27] = dtoCostoMarginal.CosMar55;
                            aCostoMarginal[28] = dtoCostoMarginal.CosMar57;
                            aCostoMarginal[29] = dtoCostoMarginal.CosMar59;
                            aCostoMarginal[30] = dtoCostoMarginal.CosMar61;
                            aCostoMarginal[31] = dtoCostoMarginal.CosMar63;
                            aCostoMarginal[32] = dtoCostoMarginal.CosMar65;
                            aCostoMarginal[33] = dtoCostoMarginal.CosMar67;
                            aCostoMarginal[34] = dtoCostoMarginal.CosMar69;
                            aCostoMarginal[35] = dtoCostoMarginal.CosMar71;
                            aCostoMarginal[36] = dtoCostoMarginal.CosMar73;
                            aCostoMarginal[37] = dtoCostoMarginal.CosMar75;
                            aCostoMarginal[38] = dtoCostoMarginal.CosMar77;
                            aCostoMarginal[39] = dtoCostoMarginal.CosMar79;
                            aCostoMarginal[40] = dtoCostoMarginal.CosMar81;
                            aCostoMarginal[41] = dtoCostoMarginal.CosMar83;
                            aCostoMarginal[42] = dtoCostoMarginal.CosMar85;
                            aCostoMarginal[43] = dtoCostoMarginal.CosMar87;
                            aCostoMarginal[44] = dtoCostoMarginal.CosMar89;
                            aCostoMarginal[45] = dtoCostoMarginal.CosMar91;
                            aCostoMarginal[46] = dtoCostoMarginal.CosMar93;
                            aCostoMarginal[47] = dtoCostoMarginal.CosMar95;
                        }
                        else
                        {
                            BarraDTO dtoBarraFaltante = servicioBarra.GetByIdBarra(dtoBarra.BarrCodi);
                            sResultado = "Lo sentimos, No esta registrado el Costo Marginal de la Barra:" + dtoBarraFaltante.BarrNombre + " [ Barracodi: " + dtoBarra.BarrCodi + " Periodo: " + pericodi + " / Revisión: " + EntidadRecalculo.Recacodi + "]";
                            return sResultado;
                        }
                        #endregion

                        //Para todas las Unidades de Generación, en el caso de Unidades térmicas corresponde al modo de operación mientras que ha Unidades hidráulicas a central. 
                        List<VcrDespachoursDTO> listaUnidadesGeneracion = this.ListVcrDespachoursUnidadByUrsTipo(vcrecacodi, URS.GrupoCodi, "S");
                        foreach (VcrDespachoursDTO dtoUnidad in listaUnidadesGeneracion)
                        {
                            //Lista de 48 registros Costos Variables de la URS
                            List<VcrCostvariableDTO> listaCostoVariable = this.ListVcrCostvariables(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, dFecha);
                            decimal[] aCostoVariable = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosVariables
                            iNumReg = 0;
                            foreach (VcrCostvariableDTO dtoCostoVariable in listaCostoVariable)
                            {
                                aCostoVariable[iNumReg++] = dtoCostoVariable.Vcvarcostvar;
                            }
                            //Lista de 48 registros de Despacho Sin Reserva de la URS
                            List<VcrDespachoursDTO> listaSinAsignacion = this.ListVcrDespachoursByUrsUnidadTipoDia(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, "S", dFecha);
                            decimal[] aSinAsignacion = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Despachos Sin Reserva Asignada
                            iNumReg = 0;
                            foreach (VcrDespachoursDTO dtoSinAsignacion in listaSinAsignacion)
                            {
                                aSinAsignacion[iNumReg++] = dtoSinAsignacion.Vcdursdespacho;
                            }
                            //Lista de 48 registros de Despacho Sin Reserva de la URS
                            List<VcrDespachoursDTO> listaConAsignacion = this.ListVcrDespachoursByUrsUnidadTipoDia(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, "C", dFecha);
                            decimal[] aConAsignacion = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Despachos Con Reserva Asignada
                            iNumReg = 0;
                            foreach (VcrDespachoursDTO dtoConAsignacion in listaConAsignacion)
                            {
                                aConAsignacion[iNumReg++] = dtoConAsignacion.Vcdursdespacho;
                            }
                            //For de 48 intervalos de 30 minutos de cada arreglo para aplicar la formula
                            for (int k = 0; k < 48; k++)
                            {
                                //Recorremos en intervalo de cada 30 minutos para aplicar la formula
                                VcrCostoportdetalleDTO dtoCostoportdetalle = new VcrCostoportdetalleDTO();
                                dtoCostoportdetalle.Vcrecacodi = vcrecacodi;
                                dtoCostoportdetalle.Grupocodi = URS.GrupoCodi;
                                dtoCostoportdetalle.Gruponomb = URS.GrupoNomb;
                                dtoCostoportdetalle.Vcrcodfecha = dFecha;
                                dtoCostoportdetalle.Equicodi = dtoUnidad.Equicodi;
                                dtoCostoportdetalle.Vcrcodinterv = k + 1;
                                dtoCostoportdetalle.Vcrcodpdo = Math.Max((aSinAsignacion[k] - aConAsignacion[k]), 0);
                                dtoCostoportdetalle.Vcrcodcmgcp = aCostoMarginal[k] * 1000; //para convertilo a MegaWaths
                                dtoCostoportdetalle.Vcrcodcv = aCostoVariable[k];
                                //CO(urs,t) = máximo([PDO(s,urs,t) - PDO(c,urs,t)],0) x [CMgCP(urs,t) - CV(urs,t)] x 0.5
                                decimal dCostoOportunidad = dtoCostoportdetalle.Vcrcodpdo * (dtoCostoportdetalle.Vcrcodcmgcp - dtoCostoportdetalle.Vcrcodcv) * (decimal)0.5;
                                //Si CO(urs,t) < 0 entonces CO(urs,t)=0
                                if (dCostoOportunidad < 0) dCostoOportunidad = 0;
                                //Almacenando resultado y acumulando el subtotal para cada unidad: CO(urs,t) = ∑(u=1)^NU [CO(u,t)] 
                                aCostoOportunidad[k] += dCostoOportunidad;
                                dtoCostoportdetalle.Vcrcodcostoportun = dCostoOportunidad;
                                dtoCostoportdetalle.Vcrcodusucreacion = suser;
                                this.SaveVcrCostoportdetalle(dtoCostoportdetalle); //Importante, para el reporte Costo de oportunidad – CU10.04
                            }
                        }
                        decimal dVcrCOpCosto = 0;
                        for (int k = 0; k < 48; k++)
                        {
                            dVcrCOpCosto += aCostoOportunidad[k];
                        }
                        //Grabamos el Costo Operativo del dia para una URS
                        VcrCostoportunidadDTO dtoCostoOportunidad = new VcrCostoportunidadDTO();
                        dtoCostoOportunidad.Vcrecacodi = vcrecacodi;
                        dtoCostoOportunidad.Grupocodi = URS.GrupoCodi;
                        dtoCostoOportunidad.Gruponomb = URS.GrupoNomb;
                        dtoCostoOportunidad.Vcrcopfecha = dFecha;
                        dtoCostoOportunidad.Vcrcopcosto = dVcrCOpCosto;
                        dtoCostoOportunidad.Vcrcopusucreacion = suser;
                        this.SaveVcrCostoportunidad(dtoCostoOportunidad);

                        //Cambiamos de fecha
                        dFecha = dFecha.AddDays(1);
                    }
                    //Cambiamos de URS
                }

                #endregion

                #region Términos de Superávit  – CU09.03
                //---------------------------------------------------------------------------------------------------------
                //ASSETEC 202010
                //El actual PR-22 ya no contempla los siguientes términos: Déficit de Reserva(DT) y Superávit de Reserva(ST). 
                //Por lo que se ha realizado ajustes en el aplicativo para que únicamente quede la RNS.
                decimal dPAO = EntidadRecalculo.Vcrecapaosinergmin; //Precio Máximo Asignado por OSINERGMIN (S/./MW-mes)
                decimal dKc = EntidadRecalculo.Vcrecakcalidad; //Parámetro configurable. Normalmente Kc = 1.
                //Traemos la lista de Versiones de Reserva No Suministrada – CU05 - VCR_VERSIONDSRNS
                VcrVersiondsrnsDTO dtoVersionDSRN = this.GetByIdVcrVersiondsrns((int)EntidadRecalculo.Vcrdsrcodi);
                if (dtoVersionDSRN != null)
                {   //Existe en el periodo, calculamos deficit, superavit y rns
                    DateTime dDiaCursor = dFecInicio;
                    //Para cada dia dentro del periodo calculo los Terminos Superavit  
                    while (dDiaCursor <= dFecFin)
                    {
                        //MPA: Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-mes)
                        decimal dMPd = 0; //202010-> this.GetByVcrAsignacionreservadMPA(vcrecacodi, dDiaCursor);
                        foreach (TrnBarraursDTO URS in listaURS)
                        {   //Insertamos por defecto todos los registros para el calculo
                            decimal dTotDT = 0;
                            decimal dTotST = 0;
                            //decimal dTotSTURS = 0;
                            decimal dTotRNS = 0;
                            decimal dDeficit = 0;
                            decimal dSuperavit = 0;
                            decimal dReservaNoSuministrada = 0;

                            #region NUEVO DESDE 202010 -> Calculo de la Reserva No Suministrada(i,d)
                            //Subida
                            List<VcrVerrnsDTO> listaRNS_Subir = this.ListVcrVerrnssDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor, ConstantesCompensacionRSF.TipoCargaSubir);
                            foreach (VcrVerrnsDTO dtoRNS in listaRNS_Subir)
                            {
                                DateTime dfHoraInicio = (DateTime)dtoRNS.Vcvrnshorinicio;
                                DateTime dfHoraFinal = (DateTime)dtoRNS.Vcvrnshorfinal;

                                int dHoraF = dfHoraFinal.Hour;
                                int dMinuteF = dfHoraFinal.Minute;
                                //int dSegundoF = dfHoraFinal.Second;
                                decimal dHF_HI = 0;
                                //dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                if (dHoraF == 23 && dMinuteF == 59)
                                {
                                    DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                    dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                else
                                {
                                    dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                dTotRNS += dtoRNS.Vcvrnsrns * dHF_HI;
                            }
                            //Bajada
                            List<VcrVerrnsDTO> listaRNS_Bajar = this.ListVcrVerrnssDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor, ConstantesCompensacionRSF.TipoCargaBajar);
                            foreach (VcrVerrnsDTO dtoRNS in listaRNS_Bajar)
                            {
                                DateTime dfHoraInicio = (DateTime)dtoRNS.Vcvrnshorinicio;
                                DateTime dfHoraFinal = (DateTime)dtoRNS.Vcvrnshorfinal;

                                int dHoraF = dfHoraFinal.Hour;
                                int dMinuteF = dfHoraFinal.Minute;
                                //int dSegundoF = dfHoraFinal.Second;
                                decimal dHF_HI = 0;
                                //dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                if (dHoraF == 23 && dMinuteF == 59)
                                {
                                    DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                    dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                else
                                {
                                    dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                dTotRNS += dtoRNS.Vcvrnsrns * dHF_HI;
                            }
                            dReservaNoSuministrada = -1 * dTotRNS * dPAO / 30;

                            #endregion
                            //Insertamos
                            VcrTermsuperavitDTO dtoTermsuperavit = new VcrTermsuperavitDTO();
                            dtoTermsuperavit.Vcrecacodi = vcrecacodi;
                            dtoTermsuperavit.Grupocodi = URS.GrupoCodi;
                            dtoTermsuperavit.Gruponomb = URS.GrupoNomb;
                            dtoTermsuperavit.Vcrtsfecha = dDiaCursor; //Información de un dia.
                            dtoTermsuperavit.Vcrtsmpa = dMPd;//dMPa;
                            dtoTermsuperavit.Vcrtsdefmwe = dTotDT;
                            dtoTermsuperavit.Vcrtssupmwe = dTotST;
                            dtoTermsuperavit.Vcrtsrnsmwe = dTotRNS;
                            dtoTermsuperavit.Vcrtsdeficit = dDeficit;
                            dtoTermsuperavit.Vcrtssuperavit = dSuperavit;
                            dtoTermsuperavit.Vcrtsresrvnosumn = dReservaNoSuministrada;
                            dtoTermsuperavit.Vcrtsusucreacion = suser;
                            this.SaveVcrTermsuperavit(dtoTermsuperavit);
                        }
                        dDiaCursor = dDiaCursor.AddDays(1);
                    }//end para cada dia -> dDiaCursor
                }//end if dtoVersionDSRN != null
                #endregion

                //ASSETEC 20190115
                #region Costo total del Servicio RSF – CU09.04.01
                //Para cada Dia dentro del periodo calculo el Costo total del Servicio RSF
                decimal[] aServicioRSF = new decimal[40];
                decimal dTotalServicioRSF = 0;
                decimal dTotalServicioRSFsinRNS = 0;
                DateTime dVcsRsfFecha = dFecInicio;
                while (dVcsRsfFecha <= dFecFin)
                {
                    //CT(RSF,d) = ∑Asignación de Reserva(urs,d) + ∑Costo de Oportunidad(urs,d) + ∑COMP(urs,d) + Reserva No Suministrada(urs,d) --> antes - Reserva No Suministrada(urs,d) 
                    VcrServiciorsfDTO dtoServiciors = this.GetByIdVcrServiciorsfValoresDia(vcrecacodi, dVcsRsfFecha);
                    if (dtoServiciors != null)
                    {
                        //Grabamos el registro en VCR_SERVICIORSF
                        dtoServiciors.Vcrecacodi = vcrecacodi;
                        dtoServiciors.Vcsrscostotservrsf = dtoServiciors.Vcsrsfasignreserva + dtoServiciors.Vcsrsfcostportun + dtoServiciors.Vcsrsfcostotcomps + dtoServiciors.Vcsrsfresvnosumn; //20190115:Cambio en el ultimo signo
                        dtoServiciors.VcsrscostotservrsfSRns = dtoServiciors.Vcsrsfasignreserva + dtoServiciors.Vcsrsfcostportun + dtoServiciors.Vcsrsfcostotcomps; //aqui no se suma 𝑅𝑒𝑠𝑒𝑟𝑣𝑎 𝑁𝑜 𝑆𝑢𝑚𝑖𝑛𝑖𝑠𝑡𝑟𝑎𝑑a
                        dtoServiciors.Vcsrsffecha = dVcsRsfFecha;
                        dtoServiciors.Vcsrsfusucreacion = suser;
                        this.SaveVcrServiciorsf(dtoServiciors);
                        aServicioRSF[dVcsRsfFecha.Day] = dtoServiciors.Vcsrscostotservrsf; //CT(RSF,d)
                        dTotalServicioRSF += dtoServiciors.Vcsrscostotservrsf; //∑(d=1)^Nd CT(RSF,d)
                        dTotalServicioRSFsinRNS += dtoServiciors.VcsrscostotservrsfSRns; // Total del servicio para calculo de Cargo Incumplimiento

                    }
                    dVcsRsfFecha = dVcsRsfFecha.AddDays(1);
                }
                #endregion

                #region Asignación del Pago de RSF por cada Unidad de Generación del Sistema – CU09.04.02
                //Para cada Dia dentro del periodo calculo la Asignación de pago de RSF del unidad de generadcion “g” del día “d”
                DateTime dVcrAPFecha = dFecInicio;
                while (dVcrAPFecha <= dFecFin)
                {
                    //Para el Dia traemos la lista de Medicion de Bornes de cada Unidad de generación, exceptuando las unidades exoneradas del Pago de RSF
                    List<VcrMedborneDTO> listMedborneSinUnidExonRSF = this.ListVcrMedbornesDiaSinUnidExonRSF(vcrecacodi, dVcrAPFecha);
                    //List<VcrMedborneDTO> listMedborneSinUnidExonRSF = this.ListVcrMedbornesMesConsiderados(vcrecacodi);
                    //Calculamos el Total ∑_g^Ng Pmed(g,d) :
                    decimal dTotalPmed = 0;
                    foreach (VcrMedborneDTO dtoMedborne in listMedborneSinUnidExonRSF)
                    {
                        dTotalPmed += dtoMedborne.Vcrmebpotenciamed;
                    }
                    if (dTotalPmed == 0) dTotalPmed = 1;
                    //Aplicamos: APRSF(g,d) = - [Pmed(g,d) / (∑_g^Ng Pmed(g,d) )] x CT(RSF,d)
                    foreach (VcrMedborneDTO dtoMedborne in listMedborneSinUnidExonRSF)
                    {
                        decimal dVcrapAsignPagoRSF = -(dtoMedborne.Vcrmebpotenciamed / dTotalPmed) * aServicioRSF[dVcrAPFecha.Day];
                        VcrAsignacionpagoDTO dtoAsignacionpago = new VcrAsignacionpagoDTO();
                        dtoAsignacionpago.Vcrecacodi = vcrecacodi;
                        dtoAsignacionpago.Emprcodi = dtoMedborne.Emprcodi;
                        dtoAsignacionpago.Equicodicen = dtoMedborne.Equicodicen;
                        dtoAsignacionpago.Equicodiuni = dtoMedborne.Equicodiuni;
                        dtoAsignacionpago.Vcrapfecha = dVcrAPFecha;
                        dtoAsignacionpago.Vcrapasignpagorsf = dVcrapAsignPagoRSF;
                        dtoAsignacionpago.Vcrapusucreacion = suser;
                        this.SaveVcrAsignacionpago(dtoAsignacionpago);
                    }
                    dVcrAPFecha = dVcrAPFecha.AddDays(1);
                }
                #endregion

                //ASSETEC 202012
                //Antes Cargo por Incumplimiento de RPF – Preliminar – CU09.04.03
                #region Cálculo del Cargo por Incumplimiento del Grupo (CargoINCg,n)     
                decimal dTotalSaldoAnterior = 0;
                decimal dTotalVcrciCargoIncumpl = 0; // CargoIncT
                //función que se encarga de:
                //[CargoINC](g,n) = -1 * ∑ (j=1)->D[(INC)(g,j) x %RA x(GenM)(g,j) x COR]
                List<VcrCargoincumplDTO> listaCargoincumpl = this.ListVcrCargoIncumplGrupoCalculado(vcrecacodi);
                foreach (VcrCargoincumplDTO dtoCargoincumpl in listaCargoincumpl)
                {
                    dtoCargoincumpl.Vcrecacodi = vcrecacodi;
                    //dtoCargoincumpl.Equicodi <= dtoCargoincumpl.Equicodiuni; //Ojo, aqui esta Emprcodi y Equicodicen
                    dtoCargoincumpl.Vcrcicargoincumplmes = dtoCargoincumpl.Vcrcicargoincumpl; // dVcrciCargoIncumplmes - dTotalSaldoAnterior;
                    dtoCargoincumpl.Vcrcisaldoanterior = dTotalSaldoAnterior;
                    //dtoCargoincumpl.Vcrcicargoincumpl <= vcrcicargoincumpl;
                    dtoCargoincumpl.Vcrcicarginctransf = 0; // -1 * (dVcrciCargoIncumplmes); 
                    dtoCargoincumpl.Vcrcisaldomes = 0;
                    dtoCargoincumpl.VcrcisaldomesAnterior = dTotalSaldoAnterior;

                    //Cálculo del cumplimiento del servicio de RPF
                    // (Cumpli)g = ∑(j=1)^D [ (1 - %RPNSd(j,g)) x P(j,g) ] / ∑(j=1)^D [P(j,g)]
                    if (dtoCargoincumpl.Pericodidest > 0)
                    {
                        dtoCargoincumpl.Vcrciincumplsrvrsf = dtoCargoincumpl.Vcrciincumplsrvrsf / dtoCargoincumpl.Pericodidest;
                    }
                    //else
                    //{
                    //    dtoCargoincumpl.Vcrciincumplsrvrsf = 0;
                    //}
                    dtoCargoincumpl.Pericodidest = 0; //EntidadRecalculo.Vcrecacodidestino;
                    dtoCargoincumpl.Vcrciincent = 0;
                    dtoCargoincumpl.Vcrciusucreacion = suser;

                    this.SaveVcrCargoincumpl(dtoCargoincumpl);
                    //[CargoIncT]n = Sumtoria[ |(CargoINC)(g,n)| ]
                    dTotalVcrciCargoIncumpl += Math.Abs(dtoCargoincumpl.Vcrcicargoincumpl);
                }
                #endregion

                #region Incentivo al cumplimiento
                //(CargoIncT)n = dTotalVcrciCargoIncumpl
                //--------------------------------------------------------------------------------------------------------------------------------
                //PEu -> Producción mensual de energía activa del Grupo Unidades en Medición de Bornes, cargo de incumplimiento que tienen chek.
                decimal dTotalPEuMes = 0;
                List<VcrMedborneDTO> listMedborneMes = this.ListVcrMedbornesMesConsiderados(vcrecacodi);//solo se suman de los que son considerados
                foreach (VcrMedborneDTO dtoMedborne in listMedborneMes)
                {
                    //Calculamos el Total ∑(j=1)^D (PE)g)
                    if (dtoMedborne.Emprcodi < 0)
                        continue; //ASSETEC 20191209
                    decimal dE_Cumpli_si_es_Urpf = 0;
                    dE_Cumpli_si_es_Urpf = dtoMedborne.Vcrmebpotenciamed * dtoMedborne.Vcrmebpotenciamedgrp; //Vcrmebpotenciamedgrp = cargo.incumpl.vcrciincumplsrvrsf
                    dTotalPEuMes += dE_Cumpli_si_es_Urpf;
                    dtoMedborne.Vcrmebpotenciamedgrp = dE_Cumpli_si_es_Urpf; //Temporalmente
                }
                if (dTotalPEuMes == 0) dTotalPEuMes = 1;
                //--------------------------------------------------------------------------------------------------------------------------------
                //Traemos la lista de los CargosIncumplimiento ya registrados
                listaCargoincumpl = this.ListVcrCargoincumpls(vcrecacodi);
                foreach (VcrCargoincumplDTO dtoCargoincumpl in listaCargoincumpl)
                {
                    //(Cumpli)g -> dtoCargoincumpl.Vcrciincumplsrvrsf
                    //FaC -> EntidadRecalculo.Vcrecafactcumpl
                    if (dtoCargoincumpl.Vcrciincumplsrvrsf > EntidadRecalculo.Vcrecafactcumpl)
                    {
                        //	Si (Cumpli)g > FaC, entonces:
                        //(Incent)g = (CargoIncT)n x [ ( (Cumpli)g x ∑(j=1)^D (PE)g) / ( ∑(U_RPF)(Cumpli)u x (PE)u ) ]
                        decimal dE_Cumpli_si_es_Urpf = 0;
                        var listaE_Cumpli_si_es_Urpf = listMedborneMes.Where(x => x.Equicodiuni == dtoCargoincumpl.Equicodi).FirstOrDefault();
                        if (listaE_Cumpli_si_es_Urpf != null)
                            dE_Cumpli_si_es_Urpf = listaE_Cumpli_si_es_Urpf.Vcrmebpotenciamedgrp;
                        dtoCargoincumpl.Vcrciincent = dTotalVcrciCargoIncumpl * (dE_Cumpli_si_es_Urpf / (dTotalPEuMes));
                    }
                    else
                    {
                        //	Si (Cumpli)g ≤ FaC, entonces:
                        dtoCargoincumpl.Vcrciincent = 0; //(Incent)g = 0
                    }
                    //Actualizar Cargo por Incumplimiento de RPF transferido – CU09.04.05
                    //(CargoINCtransferido)(g,n) = (CargoINC)(g,n) + (Incent)(g,n)
                    dtoCargoincumpl.Vcrcicarginctransf = dtoCargoincumpl.Vcrcicargoincumpl + dtoCargoincumpl.Vcrciincent;

                    this.UpdateVcrCargoincumpl(dtoCargoincumpl);
                }

                #endregion

                #region Pagos por RSF – CU09.04.06
                //En listMedborneMes, tenemos la lista de Unidades de Generación del Mes (g,m)
                List<VcrMedborneDTO> listMedborneSinUnidExonRSFAA = this.ListMesConsideradosTotales(vcrecacodi);
                foreach (VcrMedborneDTO dtoMedborneUnidadGeeracion in listMedborneSinUnidExonRSFAA)
                {
                    //En base de datos aplicamos la formula: PagoRSF(g,m)= ∑(d=1)^Nd APRSF(g,d) + ReduccPagoEjec(g,m)
                    VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfUnidad(vcrecacodi, dtoMedborneUnidadGeeracion.Equicodiuni); //ASSETEC: 202012, función no considera Reducción de Pago Ejecutado de la Unidad de Generación “g” durante el mes “m” 
                    if (dtoPagorsf != null)
                    {
                        dtoPagorsf.Vcrecacodi = vcrecacodi;
                        dtoPagorsf.Equicodi = dtoMedborneUnidadGeeracion.Equicodiuni;
                        dtoPagorsf.Vcprsfusucreacion = suser;
                        this.SaveVcrPagorsf(dtoPagorsf);
                    }
                }
                #endregion

                //ASSETEC 20190115
                #region Para los Reportes – CU09.04.07
                //Traemos la lista de empresa a partir de VcrAsignacionpago

                List<VcrAsignacionpagoDTO> listAsignacionpagoEmpresa = this.ListVcrAsignacionpagosEmpresaMes(vcrecacodi);
                foreach (VcrAsignacionpagoDTO dtoEmpresa in listAsignacionpagoEmpresa)
                {
                    VcrEmpresarsfDTO dtoEmpresarsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, dtoEmpresa.Emprcodi);

                    if (dtoEmpresa.Emprcodi < 0)
                    {
                        VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfUnidad(vcrecacodi, -1); //aplica para la empresa -1002: MINERA CERRO VERDE - GU
                        dtoEmpresarsf.Vcersfpagorsf = dtoPagorsf.Vcprsfpagorsf;
                    }
                    dtoEmpresarsf.Emprcodi = dtoEmpresa.Emprcodi;
                    dtoEmpresarsf.Vcrecacodi = vcrecacodi;
                    dtoEmpresarsf.Vcersfusucreacion = suser;
                    this.SaveVcrEmpresarsf(dtoEmpresarsf);
                }
                #endregion

            }
            catch (Exception e)
            {
                sResultado = e.StackTrace;
                sResultado = e.Message; //"-1";
            }
            return sResultado;
        }



        /// <summary>
        /// Procedimiento que se encarga de ejecutar el procedimiento del calculo de RSF del periodo 2021.01 en adelante 
        /// </summary>
        /// <param name="pericodi">Periodo de calculo</param>
        /// <param name="vcrecacodi">Version de calculo</param>
        /// <param name="suser">Usuario conectado</param>
        public string ProcesarCalculoDesdeFebrero2021(int pericodi, int vcrecacodi, string suser)
        {
            string sResultado = "1";
            try
            {
                //Limpiamos Todos los calculos anteriores de la Version en Acción
                string sBorrar = this.EliminarCalculo(vcrecacodi);
                if (!sBorrar.Equals("1"))
                {
                    sResultado = "Lo sentimos, No se pudo eliminar el proceso de cálculo: " + sBorrar;
                    return sResultado;
                }
                //INICIALIZAMOS ALGUNAS VARIABLES GENERALES
                //Traemos la entidad de la versión de recalculo
                VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
                if (EntidadRecalculo.Vcrdsrcodi == null || EntidadRecalculo.Vcrdsrcodi == 0)
                {
                    sResultado = "Lo sentimos, La version de calculo no tiene asignado una version de Déficit, Superávit y Reserva No Suministrada";
                    return sResultado;
                }
                if (EntidadRecalculo.Vcrinccodi == null || EntidadRecalculo.Vcrinccodi == 0)
                {
                    sResultado = "Lo sentimos, La version de calculo no tiene asignado una version de incumplimiento";
                    return sResultado;
                }
                //FECHAS DE INICIO FINAL DEL MES
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                int iNumeroDiasMes = System.DateTime.DaysInMonth(EntidadPeriodo.AnioCodi, EntidadPeriodo.MesCodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                var sFechaFin = iNumeroDiasMes + "/" + sMes + "/" + EntidadPeriodo.AnioCodi;
                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dFecFin = DateTime.ParseExact(sFechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string sPeriodo = EntidadPeriodo.AnioCodi.ToString() + sMes; //YYYYMM
                //Valores generales
                decimal dUnoEntre24 = (decimal)1 / 24;
                //decimal dFactorCumplimiento = 0.85M;
                int iNumReg = 0;

                //INICIAMOS EL PROCESO DE CALCULO
                //Lista de URS
                List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURSCostoMarginal(pericodi, EntidadRecalculo.Recacodi);

                //ASSETEC 20190115
                #region Asignación de reserva (AR) – CU09.01
                //El proceso de cálculo para valorizar el costo del servicio por la Asignación de Reserva es el siguiente: 
                foreach (TrnBarraursDTO URS in listaURS)
                {
                    //PBF_URS: Potencia que corresponde a la Provisión Base Firme de la URS. 
                    //VCRPBPOTENCIABF -> Precio Base Firme Subir (PBF)
                    //VCRPBPOTENCIABFB-> Potencia Base Firme Bajar (PBFb)
                    //Si la URS no posee Provisión Base Firme se considerará este valor como cero. Corresponde al valor indicado en el CU09.01. En MW.
                    decimal dProvisionBaseFirmeSubirURS = 0;    //Subida
                    decimal dProvisionBaseFirmeBajarURS = 0;   //Bajada
                    decimal dPrecioPotenciaBaseFirmeSubirURS = 0;
                    decimal dPrecioPotenciaBaseFirmeBajarURS = 0;
                    VcrProvisionbaseDTO dtoProvisionBase = this.GetByIdVcrProvisionbaseURS(URS.GrupoCodi, sPeriodo); //Para una URS
                    if (dtoProvisionBase != null)
                    {
                        //Existe provisión base para esta URS
                        dProvisionBaseFirmeSubirURS = dtoProvisionBase.Vcrpbpotenciabf; //PBFi - Subida
                        dProvisionBaseFirmeBajarURS = dtoProvisionBase.Vcrpbpotenciabfb; //PBFbi - Bajada
                        dPrecioPotenciaBaseFirmeSubirURS = dtoProvisionBase.Vcrpbpreciobf; //Precio_PBFi - Subida
                        dPrecioPotenciaBaseFirmeBajarURS = dtoProvisionBase.Vcrpbpreciobfb; //Precio_PBFi - Bajada
                    }

                    //Para cada URS se calcula Asignación de Reserva de una URS en un Dia
                    DateTime dFecha = dFecInicio;
                    //decimal dHF_HI1 = (decimal)(dHoraFinal1).TotalHours;
                    while (dFecha <= dFecFin)
                    {
                        decimal dRA_PBF = 0;
                        decimal dRA_PBFBajar = 0;
                        decimal dRA_MA = 0;
                        decimal dRA_MABajar = 0;
                        decimal dMPA_id = 0; //Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-dia) para este URS
                        decimal dMPA_idBajar = 0;

                        //Lista de Asignacion de reserva para poder ingresar el correcto valor de la oferta maxima del dia.
                        //RA (URS,t): Reserva Asignada en el periodo “t” (comprendido desde HI a HF) de la URS.
                        #region PROCESANDO LA RESERVA ASIGNADA DE SUBIDA Y BAJADA
                        List<VcrReservasignDTO> listaReservaAsignada = this.GetByCriteriaVcrReservasignsURSDia(vcrecacodi, URS.GrupoCodi, dFecha);
                        foreach (VcrReservasignDTO dtoReservaAsinada in listaReservaAsignada)
                        {   //Para cada intervalo Ti 
                            DateTime dHoraFinal = (DateTime)dtoReservaAsinada.Vcrasghorfinal; //OJO minimo es OO:OO max: 23:59
                            DateTime dHoraInicio = (DateTime)dtoReservaAsinada.Vcrasghorinicio;
                            //LAS HORAS ESTAN EN FORMATO FECHA HAY QUE LLEVARLOS A HORAS / 24 
                            decimal dHF_HI = (decimal)(dHoraFinal - dHoraInicio).TotalHours;
                            //SUBIDA--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            decimal dDiferencia = dtoReservaAsinada.Vcrasgreservasign - dProvisionBaseFirmeSubirURS; //RA(urs,ti) - PBF(urs)
                            if (dDiferencia > 0)
                            {
                                //1.- RA_PBF = PBF_URS x (HF-HI) x 1/24 
                                dRA_PBF += dProvisionBaseFirmeSubirURS * dHF_HI * dUnoEntre24; //dRA_PBF: Reserva Asignada correspondiente a la Provisión Base Firme
                                //2.- RA_MA = (RA(urs,t) - PBF(urs)) x (HF-HI) x 1/24
                                dRA_MA += dDiferencia * dHF_HI * dUnoEntre24; //Math.Round(dDiferencia) * dHF_HI * dUnoEntre24;   //dRA_MA: Reserva Asignada correspondiente al Mercado de Ajuste

                                //Buscamos el Precio en las ofertas para calcular el MPA(i,d) = max𝑖,𝑑(𝑅𝐴𝑖,𝑑,𝑡𝑖1 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥1; 𝑅𝐴𝑖,𝑑,𝑡𝑖2 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥2, … ; 𝑅𝐴𝑖,𝑑,𝑡𝑖𝑛× 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥𝑛) ,∀ 𝑡𝑥n
                                decimal dPoF_idt = this.GetOfertaMaxDiaGrupoCodiHiHf(vcrecacodi, URS.GrupoCodi, dFecha, dHoraInicio, dHoraFinal, ConstantesCompensacionRSF.TipoCargaSubir);
                                //Tomando la mayor oferta dMPA_id
                                if (dMPA_id < dPoF_idt)
                                    dMPA_id = dPoF_idt;
                            }
                            else
                            {
                                //Si: RA(urs,t) - PBF(urs) ≤ 0
                                //1.- RA_PBF = RA(urs,t) x (HF-HI) x 1/24
                                dRA_PBF += dtoReservaAsinada.Vcrasgreservasign * dHF_HI * dUnoEntre24;
                                //2.- RA_MA=0
                                dRA_MA += 0;
                            }
                            //BAJADA--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            dDiferencia = dtoReservaAsinada.Vcrasgreservasignb - dProvisionBaseFirmeBajarURS; //RA(urs,ti) - PBF(urs)
                            if (dDiferencia > 0)
                            {
                                //1.- RA_PBF = PBF_URS x (HF-HI) x 1/24 
                                dRA_PBFBajar += dProvisionBaseFirmeBajarURS * dHF_HI * dUnoEntre24; //dRA_PBFBajar: Reserva Asignada correspondiente a la Provisión Base Firme
                                //2.- RA_MA = (RA(urs,t) - PBF(urs)) x (HF-HI) x 1/24
                                dRA_MABajar += dDiferencia * dHF_HI * dUnoEntre24; //Math.Round(dDiferencia) * dHF_HI * dUnoEntre24;   //dRA_MABajar: Reserva Asignada correspondiente al Mercado de Ajuste

                                //Buscamos el Precio en las ofertas para calcular el MPA(i,d) = max𝑖,𝑑(𝑅𝐴𝑖,𝑑,𝑡𝑖1 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥1; 𝑅𝐴𝑖,𝑑,𝑡𝑖2 × 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥2, … ; 𝑅𝐴𝑖,𝑑,𝑡𝑖𝑛× 𝑃𝑜𝑓𝑖,𝑑,𝑡𝑥𝑛) ,∀ 𝑡𝑥n
                                decimal dPoF_idt = this.GetOfertaMaxDiaGrupoCodiHiHf(vcrecacodi, URS.GrupoCodi, dFecha, dHoraInicio, dHoraFinal, ConstantesCompensacionRSF.TipoCargaBajar);
                                //Tomando la mayor oferta dMPA_idBajar
                                if (dMPA_idBajar < dPoF_idt)
                                    dMPA_idBajar = dPoF_idt;
                            }
                            else
                            {
                                //Si: RA(urs,t) - PBF(urs) ≤ 0
                                //1.- RA_PBF = RA(urs,t) x (HF-HI) x 1/24
                                dRA_PBFBajar += dtoReservaAsinada.Vcrasgreservasignb * dHF_HI * dUnoEntre24;
                                //2.- RA_MA=0
                                dRA_MABajar += 0;
                            }
                        }
                        #endregion

                        //Guardando la Asignación de Reserva (urs,dia) 1 URS / 1 DIA
                        VcrAsignacionreservaDTO dtoAsignacionreserva = new VcrAsignacionreservaDTO();
                        dtoAsignacionreserva.Vcrecacodi = vcrecacodi;
                        dtoAsignacionreserva.Grupocodi = URS.GrupoCodi;
                        dtoAsignacionreserva.Gruponomb = URS.GrupoNomb;
                        dtoAsignacionreserva.Vcrarfecha = dFecha;
                        dtoAsignacionreserva.Vcrarusucreacion = suser;
                        //Calculo la Asignación de Reserva(urs,dia): 
                        dtoAsignacionreserva.Vcrarrapbf = dRA_PBF;
                        dtoAsignacionreserva.Vcrarrapbfbajar = dRA_PBFBajar;
                        dtoAsignacionreserva.Vcrarprbf = dPrecioPotenciaBaseFirmeSubirURS;
                        dtoAsignacionreserva.Vcrarprbfbajar = dPrecioPotenciaBaseFirmeBajarURS;
                        dtoAsignacionreserva.Vcrarrama = dRA_MA;
                        dtoAsignacionreserva.Vcrarramabajar = dRA_MABajar;
                        dtoAsignacionreserva.Vcrarmpa = dMPA_id; //Subida -> Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-dia) para este URS
                        dtoAsignacionreserva.Vcrarmpabajar = dMPA_idBajar; //Bajada
                        dtoAsignacionreserva.Vcrarramaursra = 0; // Subida -> Reserva Asignada correspondiente al Mercado de Ajuste entre todos los URS que estan en reserva asignada
                        dtoAsignacionreserva.Vcrarramaursrabajar = 0; // Bajada
                        dtoAsignacionreserva.Vcrarasignreserva = 0;// (dRA_PBF * dPrecioPotenciaBaseFirmeURS) / 30 + (dRA_MA * dMPAM) / 30; Lo vamos a registrar despues, cuando ya se tenga el dMPAM de todos los URS en el día

                        this.SaveVcrAsignacionreserva(dtoAsignacionreserva);
                        //Cambiamos de fecha
                        dFecha = dFecha.AddDays(1);
                    }
                    //Cambiamos de URS
                }
                //Actualizando la tabla AsignacionReserva:
                //El valor de la Reserva Asignada correspondiente al Mercado de Ajuste entre todos los URS
                DateTime dDia = dFecInicio;
                while (dDia <= dFecFin)
                {
                    //max(Vcrarmpa): dMPA_id -> Mayor Precio Asignado de Ofertas del Mercado de Ajuste del día “d” (S/./MWmes) entre todos los URS de ese día
                    VcrAsignacionreservaDTO dtoAR = this.GetByVcrAsignacionreservadMPA(vcrecacodi, dDia); //retorna max(Vcrarmpa) de subida dMPA_d / bajada
                    List<VcrAsignacionreservaDTO> ListaAsignacionReserva = this.GetByCriteriaVcrAsignacionReservaOferta(vcrecacodi, dDia);
                    foreach (var item in ListaAsignacionReserva)
                    {
                        item.Vcrarramaursra = dtoAR.Vcrarmpa; // subida-> dMPA_d;
                        item.Vcrarramaursrabajar = dtoAR.Vcrarmpabajar;
                        //item.Vcrarasignreserva = (item.Vcrarrapbf * item.Vcrarprbf) / 30 + (item.Vcrarrama * dMPA_d) / 30;
                        //(∑_ti^Nti▒〖RA〗_(PBF,ti subir) ×PrBF subir+)/30  (∑_ti^Nti▒〖RA〗_(PBF,ti bajar)   ×PrBFbajar)/30 
                        item.Vcrarasignreserva = (item.Vcrarrapbf * item.Vcrarprbf) / 30 + (item.Vcrarrapbfbajar * item.Vcrarprbfbajar) / 30;
                        //(∑_ti^Nti▒〖RA〗_(MA,ti subir) ×〖MPA〗_d subir)/30+(∑_ti^Nti▒〖RA〗_(MA,ti bajar) ×〖MPA〗_d  bajar)/30
                        item.Vcrarasignreserva += (item.Vcrarrama * item.Vcrarramaursra) / 30 + (item.Vcrarramabajar * item.Vcrarramaursrabajar) / 30;
                        this.UpdateVcrAsignacionreserva(item);
                    }
                    dDia = dDia.AddDays(1);
                }

                #endregion

                #region Costo de Oportunidad (CO) – CU09.02
                //Lista de URS
                //Para cada URS se calcula su Costo Operativo
                foreach (TrnBarraursDTO URS in listaURS)
                {
                    TrnBarraursDTO dtoBarra = this.servicioBarraUrs.GetByIdUrs(URS.BarrCodi, URS.GrupoCodi);
                    DateTime dFecha = dFecInicio;
                    //Para cada dia se calcula el costo operativo de una URS
                    while (dFecha <= dFecFin)
                    {
                        decimal[] aCostoOportunidad = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosOportunidad
                        #region COSTO MARGINAL
                        //Lista de 48 registros Costos Marginales asociado a una URS
                        CostoMarginalDTO dtoCostoMarginal = this.servicioCostoMarginal.GetByIdCostoMarginalPorBarraDia(pericodi, EntidadRecalculo.Recacodi, dtoBarra.BarrCodi, dFecha.Day);
                        decimal[] aCostoMarginal = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosVariables
                        if (dtoCostoMarginal != null)
                        {
                            aCostoMarginal[0] = dtoCostoMarginal.CosMar2;
                            aCostoMarginal[1] = dtoCostoMarginal.CosMar4;
                            aCostoMarginal[2] = dtoCostoMarginal.CosMar6;
                            aCostoMarginal[3] = dtoCostoMarginal.CosMar8;
                            aCostoMarginal[4] = dtoCostoMarginal.CosMar10;
                            aCostoMarginal[5] = dtoCostoMarginal.CosMar12;
                            aCostoMarginal[6] = dtoCostoMarginal.CosMar14;
                            aCostoMarginal[7] = dtoCostoMarginal.CosMar16;
                            aCostoMarginal[8] = dtoCostoMarginal.CosMar18;
                            aCostoMarginal[9] = dtoCostoMarginal.CosMar20;
                            aCostoMarginal[10] = dtoCostoMarginal.CosMar22;
                            aCostoMarginal[11] = dtoCostoMarginal.CosMar24;
                            aCostoMarginal[12] = dtoCostoMarginal.CosMar26;
                            aCostoMarginal[13] = dtoCostoMarginal.CosMar28;
                            aCostoMarginal[14] = dtoCostoMarginal.CosMar30;
                            aCostoMarginal[15] = dtoCostoMarginal.CosMar32;
                            aCostoMarginal[16] = dtoCostoMarginal.CosMar34;
                            aCostoMarginal[17] = dtoCostoMarginal.CosMar36;
                            aCostoMarginal[18] = dtoCostoMarginal.CosMar38;
                            aCostoMarginal[19] = dtoCostoMarginal.CosMar40;
                            aCostoMarginal[20] = dtoCostoMarginal.CosMar42;
                            aCostoMarginal[21] = dtoCostoMarginal.CosMar44;
                            aCostoMarginal[22] = dtoCostoMarginal.CosMar46;
                            aCostoMarginal[23] = dtoCostoMarginal.CosMar48;
                            aCostoMarginal[24] = dtoCostoMarginal.CosMar50;
                            aCostoMarginal[25] = dtoCostoMarginal.CosMar52;
                            aCostoMarginal[26] = dtoCostoMarginal.CosMar54;
                            aCostoMarginal[27] = dtoCostoMarginal.CosMar56;
                            aCostoMarginal[28] = dtoCostoMarginal.CosMar58;
                            aCostoMarginal[29] = dtoCostoMarginal.CosMar60;
                            aCostoMarginal[30] = dtoCostoMarginal.CosMar62;
                            aCostoMarginal[31] = dtoCostoMarginal.CosMar64;
                            aCostoMarginal[32] = dtoCostoMarginal.CosMar66;
                            aCostoMarginal[33] = dtoCostoMarginal.CosMar68;
                            aCostoMarginal[34] = dtoCostoMarginal.CosMar70;
                            aCostoMarginal[35] = dtoCostoMarginal.CosMar72;
                            aCostoMarginal[36] = dtoCostoMarginal.CosMar74;
                            aCostoMarginal[37] = dtoCostoMarginal.CosMar76;
                            aCostoMarginal[38] = dtoCostoMarginal.CosMar78;
                            aCostoMarginal[39] = dtoCostoMarginal.CosMar80;
                            aCostoMarginal[40] = dtoCostoMarginal.CosMar82;
                            aCostoMarginal[41] = dtoCostoMarginal.CosMar84;
                            aCostoMarginal[42] = dtoCostoMarginal.CosMar86;
                            aCostoMarginal[43] = dtoCostoMarginal.CosMar88;
                            aCostoMarginal[44] = dtoCostoMarginal.CosMar90;
                            aCostoMarginal[45] = dtoCostoMarginal.CosMar92;
                            aCostoMarginal[46] = dtoCostoMarginal.CosMar94;
                            aCostoMarginal[47] = dtoCostoMarginal.CosMar96;
                        }
                        else
                        {
                            BarraDTO dtoBarraFaltante = servicioBarra.GetByIdBarra(dtoBarra.BarrCodi);
                            sResultado = "Lo sentimos, No esta registrado el Costo Marginal de la Barra:" + dtoBarraFaltante.BarrNombre + " [ Barracodi: " + dtoBarra.BarrCodi + " Periodo: " + pericodi + " / Revisión: " + EntidadRecalculo.Recacodi + "]";
                            return sResultado;
                        }
                        #endregion

                        //Para todas las Unidades de Generación, en el caso de Unidades térmicas corresponde al modo de operación mientras que ha Unidades hidráulicas a central. 
                        List<VcrDespachoursDTO> listaUnidadesGeneracion = this.ListVcrDespachoursUnidadByUrsTipo(vcrecacodi, URS.GrupoCodi, "S");
                        foreach (VcrDespachoursDTO dtoUnidad in listaUnidadesGeneracion)
                        {
                            //Lista de 48 registros Costos Variables de la URS
                            List<VcrCostvariableDTO> listaCostoVariable = this.ListVcrCostvariables(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, dFecha);
                            decimal[] aCostoVariable = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los CostosVariables
                            iNumReg = 0;
                            foreach (VcrCostvariableDTO dtoCostoVariable in listaCostoVariable)
                            {
                                aCostoVariable[iNumReg++] = dtoCostoVariable.Vcvarcostvar;
                            }
                            //Lista de 48 registros de Despacho Sin Reserva de la URS
                            List<VcrDespachoursDTO> listaSinAsignacion = this.ListVcrDespachoursByUrsUnidadTipoDia(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, "S", dFecha);
                            decimal[] aSinAsignacion = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Despachos Sin Reserva Asignada
                            iNumReg = 0;
                            foreach (VcrDespachoursDTO dtoSinAsignacion in listaSinAsignacion)
                            {
                                aSinAsignacion[iNumReg++] = dtoSinAsignacion.Vcdursdespacho;
                            }
                            //Lista de 48 registros de Despacho Sin Reserva de la URS
                            List<VcrDespachoursDTO> listaConAsignacion = this.ListVcrDespachoursByUrsUnidadTipoDia(vcrecacodi, URS.GrupoCodi, dtoUnidad.Equicodi, "C", dFecha);
                            decimal[] aConAsignacion = new decimal[48]; // Donde se almacenan los intervalos de 30 minutos en un dia para los Despachos Con Reserva Asignada
                            iNumReg = 0;
                            foreach (VcrDespachoursDTO dtoConAsignacion in listaConAsignacion)
                            {
                                aConAsignacion[iNumReg++] = dtoConAsignacion.Vcdursdespacho;
                            }
                            //For de 48 intervalos de 30 minutos de cada arreglo para aplicar la formula
                            for (int k = 0; k < 48; k++)
                            {
                                //Recorremos en intervalo de cada 30 minutos para aplicar la formula
                                VcrCostoportdetalleDTO dtoCostoportdetalle = new VcrCostoportdetalleDTO();
                                dtoCostoportdetalle.Vcrecacodi = vcrecacodi;
                                dtoCostoportdetalle.Grupocodi = URS.GrupoCodi;
                                dtoCostoportdetalle.Gruponomb = URS.GrupoNomb;
                                dtoCostoportdetalle.Vcrcodfecha = dFecha;
                                dtoCostoportdetalle.Equicodi = dtoUnidad.Equicodi;
                                dtoCostoportdetalle.Vcrcodinterv = k + 1;
                                dtoCostoportdetalle.Vcrcodpdo = Math.Max((aSinAsignacion[k] - aConAsignacion[k]), 0);
                                dtoCostoportdetalle.Vcrcodcmgcp = aCostoMarginal[k] * 1000; //para convertilo a MegaWaths
                                dtoCostoportdetalle.Vcrcodcv = aCostoVariable[k];
                                //CO(urs,t) = máximo([PDO(s,urs,t) - PDO(c,urs,t)],0) x [CMgCP(urs,t) - CV(urs,t)] x 0.5
                                decimal dCostoOportunidad = dtoCostoportdetalle.Vcrcodpdo * (dtoCostoportdetalle.Vcrcodcmgcp - dtoCostoportdetalle.Vcrcodcv) * (decimal)0.5;
                                //Si CO(urs,t) < 0 entonces CO(urs,t)=0
                                if (dCostoOportunidad < 0) dCostoOportunidad = 0;
                                //Almacenando resultado y acumulando el subtotal para cada unidad: CO(urs,t) = ∑(u=1)^NU [CO(u,t)] 
                                aCostoOportunidad[k] += dCostoOportunidad;
                                dtoCostoportdetalle.Vcrcodcostoportun = dCostoOportunidad;
                                dtoCostoportdetalle.Vcrcodusucreacion = suser;
                                this.SaveVcrCostoportdetalle(dtoCostoportdetalle); //Importante, para el reporte Costo de oportunidad – CU10.04
                            }
                        }
                        decimal dVcrCOpCosto = 0;
                        for (int k = 0; k < 48; k++)
                        {
                            dVcrCOpCosto += aCostoOportunidad[k];
                        }
                        //Grabamos el Costo Operativo del dia para una URS
                        VcrCostoportunidadDTO dtoCostoOportunidad = new VcrCostoportunidadDTO();
                        dtoCostoOportunidad.Vcrecacodi = vcrecacodi;
                        dtoCostoOportunidad.Grupocodi = URS.GrupoCodi;
                        dtoCostoOportunidad.Gruponomb = URS.GrupoNomb;
                        dtoCostoOportunidad.Vcrcopfecha = dFecha;
                        dtoCostoOportunidad.Vcrcopcosto = dVcrCOpCosto;
                        dtoCostoOportunidad.Vcrcopusucreacion = suser;
                        this.SaveVcrCostoportunidad(dtoCostoOportunidad);

                        //Cambiamos de fecha
                        dFecha = dFecha.AddDays(1);
                    }
                    //Cambiamos de URS
                }

                #endregion

                #region Términos de Superávit  – CU09.03
                //---------------------------------------------------------------------------------------------------------
                //ASSETEC 202010
                //El actual PR-22 ya no contempla los siguientes términos: Déficit de Reserva(DT) y Superávit de Reserva(ST). 
                //Por lo que se ha realizado ajustes en el aplicativo para que únicamente quede la RNS.
                decimal dPAO = EntidadRecalculo.Vcrecapaosinergmin; //Precio Máximo Asignado por OSINERGMIN (S/./MW-mes)
                decimal dKc = EntidadRecalculo.Vcrecakcalidad; //Parámetro configurable. Normalmente Kc = 1.
                //Traemos la lista de Versiones de Reserva No Suministrada – CU05 - VCR_VERSIONDSRNS
                VcrVersiondsrnsDTO dtoVersionDSRN = this.GetByIdVcrVersiondsrns((int)EntidadRecalculo.Vcrdsrcodi);
                if (dtoVersionDSRN != null)
                {   //Existe en el periodo, calculamos deficit, superavit y rns
                    DateTime dDiaCursor = dFecInicio;
                    //Para cada dia dentro del periodo calculo los Terminos Superavit  
                    while (dDiaCursor <= dFecFin)
                    {
                        //MPA: Mayor Precio Asignado de Ofertas de Reserva del Mercado de Ajuste (S/./MW-mes)
                        decimal dMPd = 0; //202010-> this.GetByVcrAsignacionreservadMPA(vcrecacodi, dDiaCursor);
                        foreach (TrnBarraursDTO URS in listaURS)
                        {   //Insertamos por defecto todos los registros para el calculo
                            decimal dTotDT = 0;
                            decimal dTotST = 0;
                            //decimal dTotSTURS = 0;
                            decimal dTotRNS = 0;
                            decimal dDeficit = 0;
                            decimal dSuperavit = 0;
                            decimal dReservaNoSuministrada = 0;

                            #region NUEVO DESDE 202010 -> Calculo de la Reserva No Suministrada(i,d)
                            //Subida
                            List<VcrVerrnsDTO> listaRNS_Subir = this.ListVcrVerrnssDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor, ConstantesCompensacionRSF.TipoCargaSubir);
                            foreach (VcrVerrnsDTO dtoRNS in listaRNS_Subir)
                            {
                                DateTime dfHoraInicio = (DateTime)dtoRNS.Vcvrnshorinicio;
                                DateTime dfHoraFinal = (DateTime)dtoRNS.Vcvrnshorfinal;

                                int dHoraF = dfHoraFinal.Hour;
                                int dMinuteF = dfHoraFinal.Minute;
                                //int dSegundoF = dfHoraFinal.Second;
                                decimal dHF_HI = 0;
                                //dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                if (dHoraF == 23 && dMinuteF == 59)
                                {
                                    DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                    dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                else
                                {
                                    dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                dTotRNS += dtoRNS.Vcvrnsrns * dHF_HI;
                            }
                            //Bajada
                            List<VcrVerrnsDTO> listaRNS_Bajar = this.ListVcrVerrnssDia(dtoVersionDSRN.Vcrdsrcodi, URS.GrupoCodi, dDiaCursor, ConstantesCompensacionRSF.TipoCargaBajar);
                            foreach (VcrVerrnsDTO dtoRNS in listaRNS_Bajar)
                            {
                                DateTime dfHoraInicio = (DateTime)dtoRNS.Vcvrnshorinicio;
                                DateTime dfHoraFinal = (DateTime)dtoRNS.Vcvrnshorfinal;

                                int dHoraF = dfHoraFinal.Hour;
                                int dMinuteF = dfHoraFinal.Minute;
                                //int dSegundoF = dfHoraFinal.Second;
                                decimal dHF_HI = 0;
                                //dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60 / 24);
                                if (dHoraF == 23 && dMinuteF == 59)
                                {
                                    DateTime dHoraFinalRound = dfHoraFinal.AddMinutes(1);
                                    dHF_HI = (((decimal)(dHoraFinalRound - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                else
                                {
                                    dHF_HI = (((decimal)(dfHoraFinal - dfHoraInicio).TotalMinutes) / 60) / 24;
                                }
                                dTotRNS += dtoRNS.Vcvrnsrns * dHF_HI;
                            }
                            dReservaNoSuministrada = -1 * dTotRNS * dPAO / 30;

                            #endregion
                            //Insertamos
                            VcrTermsuperavitDTO dtoTermsuperavit = new VcrTermsuperavitDTO();
                            dtoTermsuperavit.Vcrecacodi = vcrecacodi;
                            dtoTermsuperavit.Grupocodi = URS.GrupoCodi;
                            dtoTermsuperavit.Gruponomb = URS.GrupoNomb;
                            dtoTermsuperavit.Vcrtsfecha = dDiaCursor; //Información de un dia.
                            dtoTermsuperavit.Vcrtsmpa = dMPd;//dMPa;
                            dtoTermsuperavit.Vcrtsdefmwe = dTotDT;
                            dtoTermsuperavit.Vcrtssupmwe = dTotST;
                            dtoTermsuperavit.Vcrtsrnsmwe = dTotRNS;
                            dtoTermsuperavit.Vcrtsdeficit = dDeficit;
                            dtoTermsuperavit.Vcrtssuperavit = dSuperavit;
                            dtoTermsuperavit.Vcrtsresrvnosumn = dReservaNoSuministrada;
                            dtoTermsuperavit.Vcrtsusucreacion = suser;
                            this.SaveVcrTermsuperavit(dtoTermsuperavit);
                        }
                        dDiaCursor = dDiaCursor.AddDays(1);
                    }//end para cada dia -> dDiaCursor
                }//end if dtoVersionDSRN != null
                #endregion

                //ASSETEC 20190115
                #region Costo total del Servicio RSF – CU09.04.01
                //Para cada Dia dentro del periodo calculo el Costo total del Servicio RSF
                decimal[] aServicioRSF = new decimal[40];
                decimal dTotalServicioRSF = 0;
                decimal dTotalServicioRSFsinRNS = 0;
                DateTime dVcsRsfFecha = dFecInicio;
                while (dVcsRsfFecha <= dFecFin)
                {
                    //CT(RSF,d) = ∑Asignación de Reserva(urs,d) + ∑Costo de Oportunidad(urs,d) + ∑COMP(urs,d) + Reserva No Suministrada(urs,d) --> antes - Reserva No Suministrada(urs,d) 
                    VcrServiciorsfDTO dtoServiciors = this.GetByIdVcrServiciorsfValoresDia(vcrecacodi, dVcsRsfFecha);
                    if (dtoServiciors != null)
                    {
                        //Grabamos el registro en VCR_SERVICIORSF
                        dtoServiciors.Vcrecacodi = vcrecacodi;
                        dtoServiciors.Vcsrscostotservrsf = dtoServiciors.Vcsrsfasignreserva + dtoServiciors.Vcsrsfcostportun + dtoServiciors.Vcsrsfcostotcomps + dtoServiciors.Vcsrsfresvnosumn; //20190115:Cambio en el ultimo signo
                        dtoServiciors.VcsrscostotservrsfSRns = dtoServiciors.Vcsrsfasignreserva + dtoServiciors.Vcsrsfcostportun + dtoServiciors.Vcsrsfcostotcomps; //aqui no se suma 𝑅𝑒𝑠𝑒𝑟𝑣𝑎 𝑁𝑜 𝑆𝑢𝑚𝑖𝑛𝑖𝑠𝑡𝑟𝑎𝑑a
                        dtoServiciors.Vcsrsffecha = dVcsRsfFecha;
                        dtoServiciors.Vcsrsfusucreacion = suser;
                        this.SaveVcrServiciorsf(dtoServiciors);
                        aServicioRSF[dVcsRsfFecha.Day] = dtoServiciors.Vcsrscostotservrsf; //CT(RSF,d)
                        dTotalServicioRSF += dtoServiciors.Vcsrscostotservrsf; //∑(d=1)^Nd CT(RSF,d)
                        dTotalServicioRSFsinRNS += dtoServiciors.VcsrscostotservrsfSRns; // Total del servicio para calculo de Cargo Incumplimiento

                    }
                    dVcsRsfFecha = dVcsRsfFecha.AddDays(1);
                }
                #endregion

                #region Asignación del Pago de RSF por cada Unidad de Generación del Sistema – CU09.04.02
                //Para cada Dia dentro del periodo calculo la Asignación de pago de RSF del unidad de generadcion “g” del día “d”
                DateTime dVcrAPFecha = dFecInicio;
                while (dVcrAPFecha <= dFecFin)
                {
                    //Para el Dia traemos la lista de Medicion de Bornes de cada Unidad de generación, exceptuando las unidades exoneradas del Pago de RSF
                    List<VcrMedborneDTO> listMedborneSinUnidExonRSF = this.ListVcrMedbornesDiaSinUnidExonRSF(vcrecacodi, dVcrAPFecha);
                    int totalRegistros1003 = listMedborneSinUnidExonRSF.Where(x => x.Emprcodi == -1003).Count();
                    int posicionRegistros1003 = 1;
                    decimal posicionRegistros1003dVcrapAsignPagoRSF = 0;
                    //List<VcrMedborneDTO> listMedborneSinUnidExonRSF = this.ListVcrMedbornesMesConsiderados(vcrecacodi);
                    //Calculamos el Total ∑_g^Ng Pmed(g,d) :
                    decimal dTotalPmed = 0;
                    foreach (VcrMedborneDTO dtoMedborne in listMedborneSinUnidExonRSF)
                    {
                        dTotalPmed += dtoMedborne.Vcrmebpotenciamed;
                    }
                    if (dTotalPmed == 0) dTotalPmed = 1;
                    //Aplicamos: APRSF(g,d) = - [Pmed(g,d) / (∑_g^Ng Pmed(g,d) )] x CT(RSF,d)
                    //bool declarado1003 = false;
                    foreach (VcrMedborneDTO dtoMedborne in listMedborneSinUnidExonRSF)
                    {
                        if (dtoMedborne.Emprcodi == -1003)
                        {
                            posicionRegistros1003dVcrapAsignPagoRSF = posicionRegistros1003dVcrapAsignPagoRSF + ((dtoMedborne.Vcrmebpotenciamed / dTotalPmed) * aServicioRSF[dVcrAPFecha.Day]);
                            if (posicionRegistros1003 == totalRegistros1003)
                            {
                                VcrAsignacionpagoDTO dtoAsignacionpago = new VcrAsignacionpagoDTO();
                                dtoAsignacionpago.Vcrecacodi = vcrecacodi;
                                dtoAsignacionpago.Emprcodi = dtoMedborne.Emprcodi;
                                dtoAsignacionpago.Equicodicen = dtoMedborne.Equicodicen;
                                dtoAsignacionpago.Equicodiuni = dtoMedborne.Equicodiuni;
                                dtoAsignacionpago.Vcrapfecha = dVcrAPFecha;
                                dtoAsignacionpago.Vcrapasignpagorsf = -(posicionRegistros1003dVcrapAsignPagoRSF);
                                dtoAsignacionpago.Vcrapusucreacion = suser;
                                this.SaveVcrAsignacionpago(dtoAsignacionpago);
                                posicionRegistros1003 = 0;
                            }
                            posicionRegistros1003++;

                        }
                        else
                        {
                            decimal dVcrapAsignPagoRSF = -(dtoMedborne.Vcrmebpotenciamed / dTotalPmed) * aServicioRSF[dVcrAPFecha.Day];
                            VcrAsignacionpagoDTO dtoAsignacionpago = new VcrAsignacionpagoDTO();
                            dtoAsignacionpago.Vcrecacodi = vcrecacodi;
                            dtoAsignacionpago.Emprcodi = dtoMedborne.Emprcodi;
                            dtoAsignacionpago.Equicodicen = dtoMedborne.Equicodicen;
                            dtoAsignacionpago.Equicodiuni = dtoMedborne.Equicodiuni;
                            dtoAsignacionpago.Vcrapfecha = dVcrAPFecha;
                            dtoAsignacionpago.Vcrapasignpagorsf = dVcrapAsignPagoRSF;
                            dtoAsignacionpago.Vcrapusucreacion = suser;
                            this.SaveVcrAsignacionpago(dtoAsignacionpago);
                        }
                    }
                    dVcrAPFecha = dVcrAPFecha.AddDays(1);
                }
                #endregion

                //ASSETEC 202012
                //Antes Cargo por Incumplimiento de RPF – Preliminar – CU09.04.03
                #region Cálculo del Cargo por Incumplimiento del Grupo (CargoINCg,n)     
                decimal dTotalSaldoAnterior = 0;
                decimal dTotalVcrciCargoIncumpl = 0; // CargoIncT
                //función que se encarga de:
                //[CargoINC](g,n) = -1 * ∑ (j=1)->D[(INC)(g,j) x %RA x(GenM)(g,j) x COR]
                List<VcrCargoincumplDTO> listaCargoincumpl = this.ListVcrCargoIncumplGrupoCalculado(vcrecacodi);
                foreach (VcrCargoincumplDTO dtoCargoincumpl in listaCargoincumpl)
                {
                    dtoCargoincumpl.Vcrecacodi = vcrecacodi;
                    //dtoCargoincumpl.Equicodi <= dtoCargoincumpl.Equicodiuni; //Ojo, aqui esta Emprcodi y Equicodicen
                    dtoCargoincumpl.Vcrcicargoincumplmes = dtoCargoincumpl.Vcrcicargoincumpl; // dVcrciCargoIncumplmes - dTotalSaldoAnterior;
                    dtoCargoincumpl.Vcrcisaldoanterior = dTotalSaldoAnterior;
                    //dtoCargoincumpl.Vcrcicargoincumpl <= vcrcicargoincumpl;
                    dtoCargoincumpl.Vcrcicarginctransf = 0; // -1 * (dVcrciCargoIncumplmes); 
                    dtoCargoincumpl.Vcrcisaldomes = 0;
                    dtoCargoincumpl.VcrcisaldomesAnterior = dTotalSaldoAnterior;

                    //Cálculo del cumplimiento del servicio de RPF
                    // (Cumpli)g = ∑(j=1)^D [ (1 - %RPNSd(j,g)) x P(j,g) ] / ∑(j=1)^D [P(j,g)]
                    if (dtoCargoincumpl.Pericodidest > 0 && dtoCargoincumpl.Equicodi > 0)
                    {
                        dtoCargoincumpl.Vcrciincumplsrvrsf = dtoCargoincumpl.Vcrciincumplsrvrsf / dtoCargoincumpl.Pericodidest;
                    }
                    //else
                    //{
                    //    dtoCargoincumpl.Vcrciincumplsrvrsf = 0;
                    //}
                    dtoCargoincumpl.Pericodidest = 0; //EntidadRecalculo.Vcrecacodidestino;
                    dtoCargoincumpl.Vcrciincent = 0;
                    dtoCargoincumpl.Vcrciusucreacion = suser;

                    this.SaveVcrCargoincumpl(dtoCargoincumpl);
                    //[CargoIncT]n = Sumtoria[ |(CargoINC)(g,n)| ]
                    dTotalVcrciCargoIncumpl += Math.Abs(dtoCargoincumpl.Vcrcicargoincumpl);
                }
                #endregion

                #region Incentivo al cumplimiento
                //(CargoIncT)n = dTotalVcrciCargoIncumpl
                //--------------------------------------------------------------------------------------------------------------------------------
                //PEu -> Producción mensual de energía activa del Grupo Unidades en Medición de Bornes, cargo de incumplimiento que tienen chek.
                decimal dTotalPEuMes = 0;
                List<VcrMedborneDTO> listMedborneMes = this.ListVcrMedbornesMesConsiderados(vcrecacodi);//solo se suman de los que son considerados
                foreach (VcrMedborneDTO dtoMedborne in listMedborneMes)
                {

                    //Calculamos el Total ∑(j=1)^D (PE)g)
                    //if (dtoMedborne.Vcrmebpotenciamedgrp > (decimal) 0.3)
                    //continue; //ASSETEC 20191209

                    //Calculamos el Total ∑(j=1)^D (PE)g)
                    if (dtoMedborne.Emprcodi < 0)
                        continue; //ASSETEC 20191209

                    decimal dE_Cumpli_si_es_Urpf = 0;
                    dE_Cumpli_si_es_Urpf = dtoMedborne.Vcrmebpotenciamed * dtoMedborne.Vcrmebpotenciamedgrp; //Vcrmebpotenciamedgrp = cargo.incumpl.vcrciincumplsrvrsf

                    //if (dtoMedborne.Vcrmebpotenciamedgrp > (decimal)0.3)
                    if (dtoMedborne.Vcrmebpotenciamedgrp > (decimal)EntidadRecalculo.Vcrecafactcumpl)
                        dTotalPEuMes += dE_Cumpli_si_es_Urpf;

                    dtoMedborne.Vcrmebpotenciamedgrp = dE_Cumpli_si_es_Urpf; //Temporalmente
                }
                if (dTotalPEuMes == 0) dTotalPEuMes = 1;
                //--------------------------------------------------------------------------------------------------------------------------------
                //Traemos la lista de los CargosIncumplimiento ya registrados
                listaCargoincumpl = this.ListVcrCargoincumpls(vcrecacodi);
                foreach (VcrCargoincumplDTO dtoCargoincumpl in listaCargoincumpl)
                {
                    if (dtoCargoincumpl.Equicodi < 0)
                    {
                        dtoCargoincumpl.Vcrciincumplsrvrsf = 0;
                        this.UpdateVcrCargoincumpl(dtoCargoincumpl);
                        continue;
                    }
                    //(Cumpli)g -> dtoCargoincumpl.Vcrciincumplsrvrsf
                    //FaC -> EntidadRecalculo.Vcrecafactcumpl
                    if (dtoCargoincumpl.Vcrciincumplsrvrsf > EntidadRecalculo.Vcrecafactcumpl)
                    {
                        //	Si (Cumpli)g > FaC, entonces:
                        //(Incent)g = (CargoIncT)n x [ ( (Cumpli)g x ∑(j=1)^D (PE)g) / ( ∑(U_RPF)(Cumpli)u x (PE)u ) ]
                        decimal dE_Cumpli_si_es_Urpf = 0;
                        var listaE_Cumpli_si_es_Urpf = listMedborneMes.Where(x => x.Equicodiuni == dtoCargoincumpl.Equicodi).FirstOrDefault();
                        if (listaE_Cumpli_si_es_Urpf != null)
                            dE_Cumpli_si_es_Urpf = listaE_Cumpli_si_es_Urpf.Vcrmebpotenciamedgrp;
                        dtoCargoincumpl.Vcrciincent = dTotalVcrciCargoIncumpl * (dE_Cumpli_si_es_Urpf / (dTotalPEuMes));
                    }
                    else
                    {
                        //	Si (Cumpli)g ≤ FaC, entonces:
                        dtoCargoincumpl.Vcrciincent = 0; //(Incent)g = 0
                    }
                    //Actualizar Cargo por Incumplimiento de RPF transferido – CU09.04.05
                    //(CargoINCtransferido)(g,n) = (CargoINC)(g,n) + (Incent)(g,n)
                    dtoCargoincumpl.Vcrcicarginctransf = dtoCargoincumpl.Vcrcicargoincumpl + dtoCargoincumpl.Vcrciincent;

                    this.UpdateVcrCargoincumpl(dtoCargoincumpl);
                }

                #endregion

                #region Pagos por RSF – CU09.04.06
                //En listMedborneMes, tenemos la lista de Unidades de Generación del Mes (g,m)
                List<VcrMedborneDTO> listMedborneSinUnidExonRSFAA = this.ListMesConsideradosTotales(vcrecacodi);
                foreach (VcrMedborneDTO dtoMedborneUnidadGeeracion in listMedborneSinUnidExonRSFAA)
                {
                    //En base de datos aplicamos la formula: PagoRSF(g,m)= ∑(d=1)^Nd APRSF(g,d) + ReduccPagoEjec(g,m)
                    //VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfUnidad(vcrecacodi, dtoMedborneUnidadGeeracion.Equicodiuni); //ASSETEC: 202012, función no considera Reducción de Pago Ejecutado de la Unidad de Generación “g” durante el mes “m” 
                    VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfConcepto(vcrecacodi, dtoMedborneUnidadGeeracion.Equicodiuni, dtoMedborneUnidadGeeracion.Emprcodi);
                    if (dtoPagorsf != null)
                    {
                        dtoPagorsf.Vcrecacodi = vcrecacodi;
                        dtoPagorsf.Equicodi = dtoMedborneUnidadGeeracion.Equicodiuni;
                        dtoPagorsf.Vcprsfusucreacion = suser;
                        this.SaveVcrPagorsf(dtoPagorsf);
                    }
                }
                #endregion

                //ASSETEC 20190115
                #region Para los Reportes – CU09.04.07
                //Traemos la lista de empresa a partir de VcrAsignacionpago

                TitularidadAppServicio servTitEmp = new TitularidadAppServicio();
                List<VcrAsignacionpagoDTO> listAsignacionpagoEmpresa = this.ListVcrAsignacionpagosEmpresaMes(vcrecacodi);
                foreach (VcrAsignacionpagoDTO dtoEmpresa in listAsignacionpagoEmpresa)
                {
                    int emprcodiConsulta = dtoEmpresa.Emprcodi;

                    VcrEmpresarsfDTO dtoEmpresarsf;

                    List<SiMigracionDTO> listTieeEmpMigracionOrigen = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(emprcodiConsulta, -2, "", 0);

                    List<SiMigracionDTO> listTieeEmpMigracionFinal = servTitEmp.ListarTransferenciasXEmpresaOrigenXEmpresaDestino(-2, emprcodiConsulta, "", 0);

                    if (listTieeEmpMigracionFinal.Count > 0 && listTieeEmpMigracionFinal.First().Migrafeccorte != null && dFecInicio <= listTieeEmpMigracionFinal.First().Migrafeccorte && listTieeEmpMigracionFinal.First().Tmopercodi == 3)
                    {
                        SiMigracionDTO migracion = servTitEmp.GetByIdSiMigracion(listTieeEmpMigracionFinal.First().Migracodi);
                        int emprcodiOrigenConsulta = migracion.Emprcodiorigen;
                        int emprcodiDestinoConsulta = migracion.Emprcodi;

                        VcrEmpresarsfDTO dtoEmpresaOrigenrsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, emprcodiOrigenConsulta);
                        VcrEmpresarsfDTO dtoEmpresaDestinorsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, emprcodiDestinoConsulta);

                        if(dtoEmpresaOrigenrsf != null && dtoEmpresaDestinorsf != null)
                        {
                            dtoEmpresarsf = new VcrEmpresarsfDTO
                            {
                                Vcrecacodi = dtoEmpresaOrigenrsf.Vcrecacodi,
                                Emprcodi = dtoEmpresaOrigenrsf.Emprcodi,

                                // Sumar los valores de dtoEmpresaOrigenrsf y dtoEmpresaDestinorsf
                                Vcersfresvnosumins = dtoEmpresaOrigenrsf.Vcersfresvnosumins + dtoEmpresaDestinorsf.Vcersfresvnosumins,
                                Vcersftermsuperavit = dtoEmpresaOrigenrsf.Vcersftermsuperavit + dtoEmpresaDestinorsf.Vcersftermsuperavit,
                                Vcersfcostoportun = dtoEmpresaOrigenrsf.Vcersfcostoportun + dtoEmpresaDestinorsf.Vcersfcostoportun,
                                Vcersfcompensacion = dtoEmpresaOrigenrsf.Vcersfcompensacion + dtoEmpresaDestinorsf.Vcersfcompensacion,
                                Vcersfasignreserva = dtoEmpresaOrigenrsf.Vcersfasignreserva + dtoEmpresaDestinorsf.Vcersfasignreserva,
                                Vcersfpagoincumpl = dtoEmpresaOrigenrsf.Vcersfpagoincumpl + dtoEmpresaDestinorsf.Vcersfpagoincumpl,
                                Vcersfpagorsf = dtoEmpresaOrigenrsf.Vcersfpagorsf + dtoEmpresaDestinorsf.Vcersfpagorsf,

                                // Asignar un valor específico sin sumar (por ejemplo, usar el valor de dtoEmpresaDestino)
                                Vcersfusucreacion = dtoEmpresaDestinorsf.Vcersfusucreacion,
                                Vcersffeccreacion = dtoEmpresaDestinorsf.Vcersffeccreacion,

                                Emprnomb = dtoEmpresaOrigenrsf.Emprnomb
                            };
                        }
                        else
                        {
                            dtoEmpresarsf = dtoEmpresaOrigenrsf;
                        }
                    }
                    else if (listTieeEmpMigracionOrigen.Count > 0 && listTieeEmpMigracionOrigen.First().Migrafeccorte != null && dFecInicio <= listTieeEmpMigracionOrigen.First().Migrafeccorte && listTieeEmpMigracionOrigen.First().Tmopercodi == 5)
                    {
                        SiMigracionDTO migracion = servTitEmp.GetByIdSiMigracion(listTieeEmpMigracionOrigen.First().Migracodi);
                        int emprcodiOrigenConsulta = migracion.Emprcodiorigen;
                        int emprcodiDestinoConsulta = migracion.Emprcodi;

                        dtoEmpresarsf = new VcrEmpresarsfDTO();

                        dtoEmpresarsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, emprcodiOrigenConsulta);

                        VcrPagorsfDTO dtoPagorsf = this.GetByMigracionEquiposPorEmpresaOrigenxDestino(vcrecacodi, emprcodiOrigenConsulta, emprcodiDestinoConsulta);

                        dtoEmpresarsf.Vcersfpagorsf = dtoEmpresarsf.Vcersfpagorsf + dtoPagorsf.Vcprsfpagorsf;
                    }

                    else if (listTieeEmpMigracionFinal.Count > 0 && listTieeEmpMigracionFinal.First().Migrafeccorte != null && dFecInicio <= listTieeEmpMigracionFinal.First().Migrafeccorte && listTieeEmpMigracionFinal.First().Tmopercodi == 5)
                    {
                        SiMigracionDTO migracion = servTitEmp.GetByIdSiMigracion(listTieeEmpMigracionFinal.First().Migracodi);
                        int emprcodiOrigenConsulta = migracion.Emprcodiorigen;
                        int emprcodiDestinoConsulta = migracion.Emprcodi;

                        dtoEmpresarsf = new VcrEmpresarsfDTO();

                        dtoEmpresarsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, emprcodiDestinoConsulta);

                        VcrPagorsfDTO dtoPagorsf = this.GetByMigracionEquiposPorEmpresaOrigenxDestino(vcrecacodi, emprcodiOrigenConsulta, emprcodiDestinoConsulta);

                        dtoEmpresarsf.Vcersfpagorsf = dtoEmpresarsf.Vcersfpagorsf - dtoPagorsf.Vcprsfpagorsf;
                    }
                    else
                    {
                        dtoEmpresarsf = this.GetByIdVcrEmpresarsfTotalMes(vcrecacodi, emprcodiConsulta);
                    }

                    if (dtoEmpresa.Emprcodi < 0)
                    {
                        VcrPagorsfDTO dtoPagorsf = this.GetByIdVcrPagorsfConcepto(vcrecacodi, -1, dtoEmpresa.Emprcodi); //aplica para la empresa -1002: MINERA CERRO VERDE - GU
                        dtoEmpresarsf.Vcersfpagorsf = dtoPagorsf.Vcprsfpagorsf;
                    }
                    dtoEmpresarsf.Emprcodi = dtoEmpresa.Emprcodi;
                    dtoEmpresarsf.Vcrecacodi = vcrecacodi;
                    dtoEmpresarsf.Vcersfusucreacion = suser;
                    this.SaveVcrEmpresarsf(dtoEmpresarsf);
                }
                #endregion

            }
            catch (Exception e)
            {
                sResultado = e.StackTrace;
                sResultado = e.Message; //"-1";
            }
            return sResultado;
        }

        /// <summary>
        /// Procedimiento que se encarga de eliminar el calculo de RSF
        /// </summary>
        /// <param name="vcrecacodi">Version de calculo</param>
        public string EliminarCalculo(int vcrecacodi)
        {
            try
            {
                //Elimina información de la tabla VCR_EMPRESARSF
                this.DeleteVcrEmpresarsf(vcrecacodi);

                //Elimina información de la tabla VCR_PAGORSF
                this.DeleteVcrPagorsf(vcrecacodi);

                //Elimina información de la tabla VCR_REDUCCPAGOEJE
                this.DeleteVcrReduccpagoeje(vcrecacodi);

                //Elimina información de la tabla VCR_CARGOINCUMPL
                this.DeleteVcrCargoincumpl(vcrecacodi);

                //Elimina información de la tabla VCR_ASIGNACIONPAGO
                this.DeleteVcrAsignacionpago(vcrecacodi);

                //Elimina información de la tabla VCR_SERVICIORSF
                this.DeleteVcrServiciorsf(vcrecacodi);

                //Elimina información de la tabla VCR_TERMSUPERAVIT
                this.DeleteVcrTermsuperavit(vcrecacodi);

                ////Elimina información de la tabla VCR_COSTOPORTDETALLE
                this.DeleteVcrCostoportdetalle(vcrecacodi);

                //Elimina información de la tabla VCR_COSTOPORTUNIDAD
                this.DeleteVcrCostoportunidad(vcrecacodi);

                //Elimina información de la tabla VCR_ASIGNACIONRESERVA
                this.DeleteVcrAsignacionreserva(vcrecacodi);

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Procedimiento que se encarga de traer la lista MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA ACTIVA (MW)
        /// </summary>
        public List<MeMedicion96DTO> listActivaMB(DateTime dFecInicio, DateTime dFecFin, string empresas, string tiposGeneracion, string tiposEmpresa)
        {
            int central = 1;
            string parametros = "1";

            List<int> idsEmpresas = servicioConsultaMedidores.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
            empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            string fuentes = servicioConsultaMedidores.GetFuenteSSAA(tiposGeneracion);
            List<MeMedicion96DTO> listActiva = new List<MeMedicion96DTO>();
            string[] lecturas = string.IsNullOrEmpty(parametros) ? new string[0] : parametros.Split(ConstantesAppServicio.CaracterComa);

            listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(dFecInicio, dFecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
            listActiva = listActiva.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();

            return listActiva;
        }


        #endregion

        #region Reportes del Calculo

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Superávit" CU10.01
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteSuperavit(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de URS
            List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURS();

            if (formato == 1)
            {
                fileName = "ReporteSuperavit.xlsx";
                ExcelDocument.GenerarReporteSuperavit(pathFile + fileName, EntidadRecalculo, listaURS);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reserva No Suministrada" CU10.02
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteReservaNoSuministrada(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de URS
            List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURS();

            if (formato == 1)
            {
                fileName = "ReporteReservaNoSuministrada.xlsx";
                ExcelDocument.GenerarReporteReservaNoSuministrada(pathFile + fileName, EntidadRecalculo, listaURS);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reserva Asignada" CU10.03
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteReservaAsignada2020(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de URS
            List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURS();

            if (formato == 1)
            {
                fileName = "ReporteReservaAsignada.xlsx";
                ExcelDocument.GenerarReporteReservaAsignada2020(pathFile + fileName, EntidadRecalculo, listaURS);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reserva Asignada" CU10.03
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteReservaAsignada(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de URS
            List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURS();

            if (formato == 1)
            {
                fileName = "ReporteReservaAsignada.xlsx";
                ExcelDocument.GenerarReporteReservaAsignada(pathFile + fileName, EntidadRecalculo, listaURS);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "CostoOportunidad" CU10.04
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteCostoOportunidad(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de URS
            List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURS();

            if (formato == 1)
            {
                fileName = "ReporteCostoOportunidad.xlsx";
                ExcelDocument.GenerarReporteCostoOportunidad(pathFile + fileName, EntidadRecalculo, listaURS);
            }
            return fileName;
        }


        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Compensacion" CU10.05
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteCompensacion(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);
            //Lista de URS
            List<TrnBarraursDTO> listaURS = this.servicioBarraUrs.ListURS();
            //Lista de comensaciones
            List<VcrCmpensoperDTO> ListaCompensacion = this.ListVcrCmpensopers(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteCompensacionOp.xlsx";
                ExcelDocument.GenerarFormatoVcrComOp(pathFile + fileName, listaURS, EntidadRecalculo, ListaCompensacion);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Asignación Pago Diario" CU10.06
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteAsignacionPagoDiario(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de Asignacionpago
            List<VcrAsignacionpagoDTO> listaAsignacionpago = this.ListVcrAsignacionpagos(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteAsignacionPagoDiario.xlsx";
                ExcelDocument.GenerarReporteAsignacionPagoDiario(pathFile + fileName, EntidadRecalculo, listaAsignacionpago);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Costo del Servicio RSF" CU10.07
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteCostoServicioRSFDiario(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de Serviciorsf
            List<VcrServiciorsfDTO> listaServiciorsf = this.ListVcrServiciorsfs(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteCostoServicioRSFDiario.xlsx";
                ExcelDocument.GenerarReporteCostoServicioRSFDiario(pathFile + fileName, EntidadRecalculo, listaServiciorsf);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Cuadro PR21" CU10.08 para el periodo posterior al 2021.01
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteCuadroPR21(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de MedidoresBorne
            List<VcrMedborneDTO> listaMedborne = this.ListVcrMedbornesMes(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteCuadroPR21.xlsx";
                //ASSETEC 20201204 -> Dos formas de mostrar e reporte PR-21
                if (EntidadPeriodo.PeriAnioMes >= ConstantesCompensacionRSF.Periodo2021)
                {   //NUEVO -> 2021.01 hacia adelante
                    ExcelDocument.GenerarReporteCuadroPR21(pathFile + fileName, EntidadRecalculo, listaMedborne);
                }
                else
                {   //ANTIGUO -> 2020.12 hacia atras
                    ExcelDocument.GenerarReporteCuadroPR212020(pathFile + fileName, EntidadRecalculo, listaMedborne);
                }
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la Información "Reporte Resumen" CU10.09
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="vcrecacodi">Código de la versión de recálculo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteResumen(int pericodi, int vcrecacodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            //Traemos la entidad de la versión de recalculo
            VcrRecalculoDTO EntidadRecalculo = this.GetByIdVcrRecalculoView(pericodi, vcrecacodi);

            //Lista de Empresas RSF
            List<VcrEmpresarsfDTO> listaEmpresarsf = this.ListVcrEmpresarsfs(vcrecacodi);

            if (formato == 1)
            {
                fileName = "ReporteResumen.xlsx";
                //ASSETEC 20201204 -> Dos formas de mostrar e reporte PR-21
                if (EntidadPeriodo.PeriAnioMes >= ConstantesCompensacionRSF.Periodo2021)
                {   //NUEVO -> 2021.01 hacia adelante
                    ExcelDocument.GenerarReporteResumen(pathFile + fileName, EntidadRecalculo, listaEmpresarsf);
                }
                else
                {   //ANTIGUO -> 2020.12 hacia atras
                    ExcelDocument.GenerarReporteResumen2020(pathFile + fileName, EntidadRecalculo, listaEmpresarsf);
                }
            }
            return fileName;
        }

        #endregion


    }
}