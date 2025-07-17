using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.Transferencias.Helper;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COES.Servicios.Aplicacion.TransfPotencia
{
    /// <summary>
    /// Clases con métodos del módulo TransfPotencia
    /// </summary>
    public class TransfPotenciaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransfPotenciaAppServicio));
        //IndisponibilidadesAppServicio servIndis = new IndisponibilidadesAppServicio();
        SIOSEINAppServicio servSiosein = new SIOSEINAppServicio();
        CorreoAppServicio servCorreo = new CorreoAppServicio();

        #region Métodos Tabla VTP_EMPRESA_PAGO

        /// <summary>
        /// Inserta un registro de la tabla VTP_EMPRESA_PAGO
        /// </summary>
        public void SaveVtpEmpresaPago(VtpEmpresaPagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpEmpresaPagoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_EMPRESA_PAGO
        /// </summary>
        public void UpdateVtpEmpresaPago(VtpEmpresaPagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpEmpresaPagoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_EMPRESA_PAGO
        /// </summary>
        public void DeleteVtpEmpresaPago(int potepcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpEmpresaPagoRepository().Delete(potepcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros de la tabla VTP_EMPRESA_PAGO en un Mes de Valorización y Versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpEmpresaPago(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpEmpresaPagoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_EMPRESA_PAGO
        /// </summary>
        public VtpEmpresaPagoDTO GetByIdVtpEmpresaPago(int potepcodi)
        {
            return FactoryTransferencia.GetVtpEmpresaPagoRepository().GetById(potepcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_EMPRESA_PAGO
        /// </summary>
        public List<VtpEmpresaPagoDTO> ListVtpEmpresaPagos()
        {
            return FactoryTransferencia.GetVtpEmpresaPagoRepository().List();
        }

        /// <summary>
        /// Permite listar todas las empresas (Emprcodipago y Emprnombpago) de la tabla VTP_EMPRESA_PAGO que han realizado un pago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpEmpresaPagoDTO</returns>
        public List<VtpEmpresaPagoDTO> ListVtpEmpresaPagosPago(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpEmpresaPagoRepository().ListPago(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_EMPRESA_PAGO de las EmprnombPago, Emprnombcobro y potepmonto
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpEmpresaPagoDTO</returns>
        public List<VtpEmpresaPagoDTO> ListVtpEmpresaPagosCobro(int emprcodipago, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpEmpresaPagoRepository().ListCobro(emprcodipago, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpEmpresaPago
        /// </summary>
        public List<VtpEmpresaPagoDTO> GetByCriteriaVtpEmpresaPagos()
        {
            return FactoryTransferencia.GetVtpEmpresaPagoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite generar el Reporte de Saldos VTP - CU24
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteValorTransfPotencia(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            string fileName = string.Empty;
            hoja = null;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa = this.ListVtpSaldoEmpresas(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReporteValorTransfPotencia.xlsx";
                ExcelDocument.GenerarReporteValorTransfPotencia(pathFile + fileName, EntidadRecalculoPotencia, ListaSaldoEmpresa, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReporteValorTransfPotencia.pdf";
                //PdfDocument.GenerarReporteValorTransfPotencia(pathFile + fileName, EntidadRecalculoPotencia, ListaSaldoEmpresa, pathLogo);
            }

            return fileName;
        }

        /// <summary>
        /// Permite generar la Matriz de pagos VTP - CU25
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteMatriz(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            string fileName = string.Empty;
            hoja = null;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpEmpresaPagoDTO> ListaEmpresaPago = this.ListVtpEmpresaPagosPago(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReporteMatrizPagos.xlsx";
                ExcelDocument.GenerarReporteMatriz(pathFile + fileName, EntidadRecalculoPotencia, ListaEmpresaPago, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReporteMatrizPagos.pdf";
                PdfDocument.GenerarReporteMatriz(pathFile + fileName, EntidadRecalculoPotencia, ListaEmpresaPago, pathLogo);
            }

            return fileName;
        }
        #endregion

        #region Métodos Tabla VTP_INGRESO_POTEFR

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_POTEFR
        /// </summary>
        public int SaveVtpIngresoPotefr(VtpIngresoPotefrDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpIngresoPotefrRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_POTEFR
        /// </summary>
        public void UpdateVtpIngresoPotefr(VtpIngresoPotefrDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_POTEFR
        /// </summary>
        public void DeleteVtpIngresoPotefr(int ipefrcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrRepository().Delete(ipefrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla VTP_INGRESO_POTEFR
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        private void DeleteByCriteriaVtpIngresoPotefr(int pericodi, int recpotcodi)
        {
            {
                try
                {
                    FactoryTransferencia.GetVtpIngresoPotefrRepository().DeleteByCriteria(pericodi, recpotcodi);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_POTEFR
        /// </summary>
        public VtpIngresoPotefrDTO GetByIdVtpIngresoPotefr(int ipefrcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrRepository().GetById(ipefrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTEFR
        /// </summary>
        public List<VtpIngresoPotefrDTO> ListVtpIngresoPotefrs()
        {
            return FactoryTransferencia.GetVtpIngresoPotefrRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpIngresoPotefr
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpIngresoPotefrDTO> GetByCriteriaVtpIngresoPotefrs(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrRepository().GetByCriteria(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite obtener el bool true , false o null  para validar un nuevo registro en la tabla VtpIngresoPotefr
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public string GetResultSaveVtpIngresoPotefr(int? ipefrdia, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrRepository().GetResultSave(ipefrdia, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite obtener el bool true , false o null  para validar un update registro en la tabla VtpIngresoPotefr
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public string GetResultUpdateVtpIngresoPotefr(int ipefrcodi, int? ipefrdia, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrRepository().GetResultUpdate(ipefrcodi, ipefrdia, pericodi, recpotcodi);
        }

        /// <summary>
        /// Procesar Carga desde Potencia firme remunerable
        /// </summary>
        /// <param name="pfrreccodi"></param>
        /// <param name="pericodi"></param>
        /// <param name="recpotcodi"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int ProcesarCargaPfr(int pfrreccodi, int pericodi, int recpotcodi, string usuario)
        {
            PotenciaFirmeRemunerableAppServicio pfrCargaServicio = new PotenciaFirmeRemunerableAppServicio();
            var listaCabeceras = this.GetByCriteriaVtpIngresoPotefrs(pericodi, recpotcodi);
            int idCabecera = 0;
            try
            {
                // Eliminar detalle
                foreach (var item in listaCabeceras)
                {
                    this.DeleteByCriteriaVtpIngresoPotefrDetalle(item.Ipefrcodi, pericodi, recpotcodi);
                    //Elimina cabecera
                    //this.DeleteVtpIngresoPotefr(item.Ipefrcodi);
                }

                //obtener el reccálculo
                var recalculoPfrEnt = pfrCargaServicio.GetByIdPfrRecalculo(pfrreccodi);
                var regPeriodo = pfrCargaServicio.GetByIdPfrPeriodo(recalculoPfrEnt.Pfrpercodi);
                //obtener id del reporte de pfr
                var idReportePfr = pfrCargaServicio.GetUltimoPfrrptcodiXRecalculo(pfrreccodi, 7);
                var idReporteDatos = pfrCargaServicio.GetUltimoPfrrptcodiXRecalculo(pfrreccodi,4);

                //traer escenario con la data del reporte
                List<PfrReporteTotalDTO> listadoDatosPfr = pfrCargaServicio.ListPfrReporteTotalByReportecodi(idReportePfr).OrderBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();
                List<PfrEscenarioDTO> listaEscenario = pfrCargaServicio.ListPfrEscenariosByReportecodi(idReportePfr).OrderBy(x => x.Pfrescfecini).ToList();

                int i = 1;
                foreach (var reg in listaEscenario)
                {
                    reg.Numero = i;
                    i++;
                }

                List<PfrReporteTotalDTO> listadoDatosPe = pfrCargaServicio.ListPfrReporteTotalByReportecodi(idReporteDatos);
                List<PfrEscenarioDTO> listaEscenarioDatos = pfrCargaServicio.ListPfrEscenariosByReportecodi(idReporteDatos).OrderBy(x => x.Pfrescfecini).ToList();
                listaEscenarioDatos.ForEach(x => x.Numero++);

                int j = 1;
                foreach (var reg in listaEscenarioDatos)
                {
                    reg.Numero = j;
                    j++;
                }

                char inicio = 'A';
                VtpIngresoPotefrDTO cabecera = new VtpIngresoPotefrDTO();
                foreach (var item in listaEscenario)
                {
                    //Obtenemos los escenarios a usar
                    PfrEscenarioDTO regEscenarioDatos = listaEscenarioDatos.Find(x => x.Numero == item.Numero);
                    //filtrar datos por escenario
                    var listaDatosxEscenario = listadoDatosPe.Where(x => x.Pfresccodi == regEscenarioDatos.Pfresccodi).OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();

                    //dias por intervalos
                    var numDias = item.Pfrescfecfin.Day - item.Pfrescfecini.Day + 1;

                    cabecera.Ipefrintervalo = item.Numero;
                    cabecera.Ipefrdia = numDias;
                    cabecera.Ipefrdescripcion = inicio.ToString();
                    //guardar cabecera
                    cabecera.Pericodi = pericodi;
                    cabecera.Recpotcodi = recpotcodi;
                    cabecera.Ipefrusucreacion = usuario;
                    cabecera.Ipefrusumodificacion = usuario;
                    cabecera.Ipefrfeccreacion = DateTime.Now;
                    cabecera.Ipefrfecmodificacion = DateTime.Now;

                    if (listaCabeceras.Count() > 0)
                    {
                        var itemCabecera = listaCabeceras.First();
                        cabecera.Ipefrcodi = itemCabecera.Ipefrcodi;
                        UpdateVtpIngresoPotefr(cabecera);
                        listaCabeceras.Remove(itemCabecera);

                        idCabecera = cabecera.Ipefrcodi;
                    }
                    else
                    {
                        idCabecera = this.SaveVtpIngresoPotefr(cabecera);
                    }

                    //guardar detalle
                    var listaPfrxEsc = listadoDatosPfr.Where(x => x.Pfresccodi == item.Pfresccodi).ToList();

                    foreach (var reg in listaPfrxEsc)
                    {
                        VtpIngresoPotefrDetalleDTO detalle = new VtpIngresoPotefrDetalleDTO();
                        detalle.Emprcodi = reg.Emprcodi;
                        detalle.Cenequicodi = reg.Equipadre;
                        detalle.Uniequicodi = reg.Equicodi;
                        detalle.Unigrupocodi = reg.Grupocodi;
                        detalle.Ipefrdunidadnomb = reg.Pfrtotunidadnomb;
                        detalle.Ipefrdficticio = reg.Pfrtotficticio;

                        var regDatos = listaDatosxEscenario.Find(x => x.Emprcodi == reg.Emprcodi && x.Equicodi == reg.Equicodi && x.Grupocodi == reg.Grupocodi);
                        detalle.Ipefrdpoteefectiva = regDatos != null? regDatos.Pfrtotpe * 1000: regDatos.Pfrtotpe;

                        detalle.Ipefrdpotefirme = reg?.Pfrtotpf;
                        detalle.Ipefrdpotefirmeremunerable = reg?.Pfrtotpfr;

                        //Crear registro                                          
                        detalle.Ipefrcodi = idCabecera;
                        detalle.Pericodi = pericodi;
                        detalle.Recpotcodi = recpotcodi;
                        detalle.Ipefrdusucreacion = usuario;
                        detalle.Ipefrdusumodificacion = usuario;

                        this.SaveVtpIngresoPotefrDetalle(detalle);
                    }
                    inicio++;
                }

                return idCabecera;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla VTP_INGRESO_POTEFR_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        public void SaveVtpIngresoPotefrDetalle(VtpIngresoPotefrDetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        public void UpdateVtpIngresoPotefrDetalle(VtpIngresoPotefrDetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        /// <param name="ipefrdcodi">Código del intervalo de Potencia Efectiva, Firme y Firme Remunerable Detalle</param>
        public void DeleteVtpIngresoPotefrDetalle(int ipefrdcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().Delete(ipefrdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina registros de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        /// <param name="ipefrcodi">Código del intervalo de Potencia Efectiva, Firme y Firme Remunerable</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpIngresoPotefrDetalle(int ipefrcodi, int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().DeleteByCriteria(ipefrcodi, pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina registros de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpIngresoPotefrDetalleVersion(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().DeleteByCriteriaVersion(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        public VtpIngresoPotefrDetalleDTO GetByIdVtpIngresoPotefrDetalle(int ipefrdcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().GetById(ipefrdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        public List<VtpIngresoPotefrDetalleDTO> ListVtpIngresoPotefrDetalles()
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        /// <param name="ipefrcodi">Código del intervalo de Potencia Efectiva, Firme y Firme Remunerable</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotefrDetalleDTO</returns>
        public List<VtpIngresoPotefrDetalleDTO> GetByCriteriaVtpIngresoPotefrDetalles(int ipefrcodi, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().GetByCriteria(ipefrcodi, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpIngresoPotefrDetalle y devolver los registros totalizados por Central de generación (Unidad)
        /// </summary>
        /// <param name="ipefrcodi">Código del intervalo de Potencia Efectiva, Firme y Firme Remunerable</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotefrDetalleDTO</returns>
        public List<VtpIngresoPotefrDetalleDTO> GetByCriteriaVtpIngresoPotefrDetallesSumCentral(int ipefrcodi, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().GetByCriteriaSumCentral(ipefrcodi, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VTP_INGRESO_POTEFR_DETALLE
        /// </summary>
        /// <param name="ipefrcodi">Código del intervalo de Potencia Efectiva, Firme y Firme Remunerable</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="intervalo">Intervalo del grupo de dias</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoIngresoPotefrDetalle(int ipefrcodi, int pericodi, int recpotcodi, int intervalo, int formato, string pathFile, string pathLogo, bool flag)
        {
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpIngresoPotefrDetalleDTO> ListaIngresoPotefrDetalle = this.GetByCriteriaVtpIngresoPotefrDetalles(ipefrcodi, pericodi, recpotcodi);

            //var flag = false;
            if (formato == 1)
            {
                fileName = "ReporteIngresoPotenciaEfectivaFirmeRemunerada.xlsx";
                //ExcelDocument.GenerarFormatoIngresoPotefrDetalle(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoPotefrDetalle, intervalo);
                if (flag == true)
                {
                    ExcelDocument.GenerarFormatoIngresoPotefrDetallePfr(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoPotefrDetalle, intervalo);
                }
                else
                {
                    ExcelDocument.GenerarFormatoIngresoPotefrDetalle(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoPotefrDetalle, intervalo);
                }
            }

            return fileName;
        }

        /// <summary>
        /// método para guardar data en las tablas interval y promp
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="recpotcodi"></param>
        /// <param name="entidadRecalculoPotencia"></param>
        /// <param name="dIngresoPorPotenciaTotal"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string ProcesarValorizacionDetallePfr(int pericodi, int recpotcodi, VtpRecalculoPotenciaDTO entidadRecalculoPotencia, decimal dIngresoPorPotenciaTotal, string usuario)
        {
            PotenciaFirmeRemunerableAppServicio pfrCargaServicio = new PotenciaFirmeRemunerableAppServicio();
            PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
            string sResultado = "1";
            try
            {
                var listaCabeceras = this.GetByCriteriaVtpIngresoPotefrs(pericodi, recpotcodi);
                foreach (VtpIngresoPotefrDTO dtoIngresoPotEFR in listaCabeceras)
                {
                    //Para cada Intervalo: Determinar el IngresoPotenciaUnidad
                    var listaCabecerasDetalle = this.GetByCriteriaVtpIngresoPotefrDetallesSumCentral(dtoIngresoPotEFR.Ipefrcodi, pericodi, recpotcodi);

                    //omitimos los ficticios que tengan remunerable 0
                    var listaFicticiosPfr0 = listaCabecerasDetalle.Where(x => x.Ipefrdficticio == 1 && x.Ipefrdpotefirmeremunerable.GetValueOrDefault() == 0).ToList();
                    listaCabecerasDetalle = listaCabecerasDetalle.Except(listaFicticiosPfr0).ToList();

                    //Primero totalizamos en este intervalo PotFirmeRemurable del Periodo
                    decimal dPotFirmeRemurable = 0;
                    foreach (VtpIngresoPotefrDetalleDTO dtoIngresoPotEFRDetalle in listaCabecerasDetalle)
                    {
                        decimal dIngresoPreliminar = Convert.ToDecimal(dtoIngresoPotEFRDetalle.Ipefrdpotefirmeremunerable) * Convert.ToDecimal(entidadRecalculoPotencia.Recpotpreciopoteppm);
                        dPotFirmeRemurable += dIngresoPreliminar;
                    }
                    if (dPotFirmeRemurable == 0)
                    {
                        sResultado = "Lo sentimos, los datos de Potencia Efectiva, Firme y Firme Remunerable son nulas";
                        return sResultado;
                    }
                    decimal dFactorAjuste = dIngresoPorPotenciaTotal / dPotFirmeRemurable; //Factor de Ajuste del Ingreso Garantizado - OK
                                                                                           //Aplicamos la formula: IPUnidad = dIngresoPorPotenciaTotal * (PotEFR_Unidad/dPotFirmeRemurable)
                    foreach (VtpIngresoPotefrDetalleDTO dtoIngresoPotEFRDetalle in listaCabecerasDetalle)
                    {
                        decimal dPotIPUnidad = dFactorAjuste * Convert.ToDecimal(dtoIngresoPotEFRDetalle.Ipefrdpotefirmeremunerable) * Convert.ToDecimal(entidadRecalculoPotencia.Recpotpreciopoteppm);
                        //Insertamos La potencia Efectiva, Firme y Firme Remunerable de la Unidad en el intervalo
                        VtpIngresoPotUnidIntervlDTO dtoIngresoPotUnidIntervl = new VtpIngresoPotUnidIntervlDTO();
                        dtoIngresoPotUnidIntervl.Pericodi = pericodi;
                        dtoIngresoPotUnidIntervl.Recpotcodi = recpotcodi;
                        dtoIngresoPotUnidIntervl.Emprcodi = Convert.ToInt32(dtoIngresoPotEFRDetalle.Emprcodi);
                        dtoIngresoPotUnidIntervl.Equicodi = Convert.ToInt32(dtoIngresoPotEFRDetalle.Uniequicodi);
                        dtoIngresoPotUnidIntervl.Ipefrcodi = dtoIngresoPotEFR.Ipefrcodi; //Codigo del Intervalo
                        dtoIngresoPotUnidIntervl.Inpuinintervalo = dtoIngresoPotEFR.Ipefrintervalo; //Intervalo
                        dtoIngresoPotUnidIntervl.Inpuindia = Convert.ToInt32(dtoIngresoPotEFR.Ipefrdia); //Nro. dias en el intervalo
                        dtoIngresoPotUnidIntervl.Inpuinimporte = dPotIPUnidad;
                        dtoIngresoPotUnidIntervl.Inpuinusucreacion = usuario;

                        dtoIngresoPotUnidIntervl.Grupocodi = dtoIngresoPotEFRDetalle.Unigrupocodi;
                        dtoIngresoPotUnidIntervl.Inpuinunidadnomb = dtoIngresoPotEFRDetalle.Ipefrdunidadnomb;
                        dtoIngresoPotUnidIntervl.Inpuinficticio = dtoIngresoPotEFRDetalle.Ipefrdficticio;
                        //Grabamos
                        this.SaveVtpIngresoPotUnidIntervl(dtoIngresoPotUnidIntervl);
                    }
                } //Fin de Intervalos

                //La Potencia Firme Remunerable de cada Unidad (Central) en el mes de valorización: VTP_INGRESO_POTUNID_PROMD
                //Ingreso Garantizado por Potencia Firme por empresas
                var entidadPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                int iNumDiasMes = System.DateTime.DaysInMonth(entidadPeriodo.AnioCodi, entidadPeriodo.MesCodi);
                var listaIngresoPotenciaUnidad = this.ListVtpIngresoPotUnidIntervlSumIntervl(pericodi, recpotcodi);
                foreach (VtpIngresoPotUnidIntervlDTO dtoIngresoPotenciaUnidad in listaIngresoPotenciaUnidad)
                {
                    //IPUnidadPromedio = sum(dPotIPUnidad * dtoIngresoPotEFR.Ipefrdia)/NumeroDiasMes 
                    decimal dImportePromedio = dtoIngresoPotenciaUnidad.Inpuinimporte / iNumDiasMes;
                    VtpIngresoPotUnidPromdDTO dtoIngresoPotUnidPromd = new VtpIngresoPotUnidPromdDTO();
                    dtoIngresoPotUnidPromd.Pericodi = pericodi;
                    dtoIngresoPotUnidPromd.Recpotcodi = recpotcodi;
                    dtoIngresoPotUnidPromd.Emprcodi = dtoIngresoPotenciaUnidad.Emprcodi;
                    dtoIngresoPotUnidPromd.Equicodi = dtoIngresoPotenciaUnidad.Equicodi;
                    dtoIngresoPotUnidPromd.Inpuprimportepromd = dImportePromedio;
                    dtoIngresoPotUnidPromd.Inpuprusucreacion = usuario;

                    dtoIngresoPotUnidPromd.Grupocodi = dtoIngresoPotenciaUnidad.Grupocodi;
                    dtoIngresoPotUnidPromd.Inpuprficticio = dtoIngresoPotenciaUnidad.Inpuinficticio;
                    dtoIngresoPotUnidPromd.Inpuprunidadnomb = dtoIngresoPotenciaUnidad.Inpuinunidadnomb;
                    //Grabamos
                    this.SaveVtpIngresoPotUnidPromd(dtoIngresoPotUnidPromd);
                }


                return sResultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //SIOSEIN-PRIE-2021
        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTEFR_DETALLE DE LOS REGISTROS QUE NO ESTAN EN PRIE
        /// </summary>
        /// <param name="dFecha">Periodo de la tabla pr_potenciafirme</param>
        /// <param name="ipefrcodi">Código del intervalo de Potencia Efectiva, Firme y Firme Remunerable</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotefrDetalleDTO</returns>
        public List<VtpIngresoPotefrDetalleDTO> GetByCriteriaVtpIngresoPotefrDetallesSinPRIE(DateTime dFecha, int ipefrcodi, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().GetByCriteriaSinPRIE(dFecha, ipefrcodi, pericodi, recpotcodi);
        }

        #endregion

        #region PrimasRER.2023
        public List<VtpIngresoPotefrDetalleDTO> GetCentralUnidadByEmpresa(int emprcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().GetCentralUnidadByEmpresa(emprcodi);
        }
        #endregion




        #region Métodos Tabla VTP_INGRESO_POTENCIA

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_POTENCIA
        /// </summary>
        public void SaveVtpIngresoPotencia(VtpIngresoPotenciaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotenciaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_POTENCIA
        /// </summary>
        public void UpdateVtpIngresoPotencia(VtpIngresoPotenciaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotenciaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_POTENCIA
        /// </summary>
        public void DeleteVtpIngresoPotencia(int potipcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotenciaRepository().Delete(potipcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_POTENCIA por mes de valorización y versión de recalculo
        /// </summary>
        public void DeleteByCriteriaVtpIngresoPotencia(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotenciaRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_POTENCIA
        /// </summary>
        public VtpIngresoPotenciaDTO GetByIdVtpIngresoPotencia(int potipcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotenciaRepository().GetById(potipcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTENCIA
        /// </summary>
        public List<VtpIngresoPotenciaDTO> ListVtpIngresoPotencias()
        {
            return FactoryTransferencia.GetVtpIngresoPotenciaRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTENCIA, de un periodo y recalculo, mas el nombre de la empresa
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotenciaDTO</returns>
        public List<VtpIngresoPotenciaDTO> ListVtpIngresoPotenciaEmpresa(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotenciaRepository().ListEmpresa(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpIngresoPotencia
        /// </summary>
        public List<VtpIngresoPotenciaDTO> GetByCriteriaVtpIngresoPotencias()
        {
            return FactoryTransferencia.GetVtpIngresoPotenciaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VTP_INGRESO_POTUNID_INTERVL

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_POTUNID_INTERVL
        /// </summary>
        public void SaveVtpIngresoPotUnidIntervl(VtpIngresoPotUnidIntervlDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_POTUNID_INTERVL
        /// </summary>
        public void UpdateVtpIngresoPotUnidIntervl(VtpIngresoPotUnidIntervlDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_POTUNID_INTERVL
        /// </summary>
        public void DeleteVtpIngresoPotUnidIntervl(int inpuincodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().Delete(inpuincodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_INGRESO_POTUNID_INTERVL por mes de valorización y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpIngresoPotUnidIntervl(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_POTUNID_INTERVL
        /// </summary>
        public VtpIngresoPotUnidIntervlDTO GetByIdVtpIngresoPotUnidIntervl(int potipcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().GetById(potipcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTUNID_INTERVL
        /// </summary>
        public List<VtpIngresoPotUnidIntervlDTO> ListVtpIngresoPotUnidIntervl()
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().List();
        }

        /// <summary>
        /// Permite Listar los registros de la tabla VTP_INGRESO_POTUNID_INTERVL, Sumarizado(INPUINIMPORTE * INPUINDIA) por EmprCodi y Equicodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpIngresoPotUnidIntervlDTO> ListVtpIngresoPotUnidIntervlSumIntervl(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().ListSumIntervl(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite Listar los registros de la tabla VTP_INGRESO_POTUNID_INTERVL, Sumarizado(INPUINIMPORTE) por EmprCodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="emprcodi">Código de la Empresa de generación</param>
        /// <param name="ipefrcodi">Código del Ingreso por potencia Efectiva, Firme y Firme Remunerable</param>
        public List<VtpIngresoPotUnidIntervlDTO> ListVtpIngresoPotUnidIntervlSumIntervlEmpresa(int pericodi, int recpotcodi, int emprcodi, int ipefrcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().ListSumIntervlEmpresa(pericodi, recpotcodi, emprcodi, ipefrcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTUNID_INTERVL
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="emprcodi">Código de la Empresa de generación</param>
        /// <param name="equicodi">Código de la Central de generación</param>
        /// <param name="ipefrcodi">Código del Ingreso por potencia Efectiva, Firme y Firme Remunerable</param>
        public List<VtpIngresoPotUnidIntervlDTO> GetByCriteriaVtpIngresoPotUnidIntervl(int pericodi, int recpotcodi, int emprcodi, int equicodi, int ipefrcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidIntervlRepository().GetByCriteria(pericodi, recpotcodi, emprcodi, equicodi, ipefrcodi);
        }

        #endregion

        #region Métodos Tabla VTP_INGRESO_POTUNID_PROMD

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public void SaveVtpIngresoPotUnidPromd(VtpIngresoPotUnidPromdDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public void UpdateVtpIngresoPotUnidPromd(VtpIngresoPotUnidPromdDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public void DeleteVtpIngresoPotUnidPromd(int inpuprcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().Delete(inpuprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_INGRESO_POTUNID_INTERVL por mes de valorización y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpIngresoPotUnidPromd(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public VtpIngresoPotUnidPromdDTO GetByIdVtpIngresoPotUnidPromd(int inpuprcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetById(inpuprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public List<VtpIngresoPotUnidPromdDTO> ListVtpIngresoPotUnidPromd()
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTUNID_PROMD, sumarizado por importe promedio para cada empresa
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotUnidPromdDTO</returns>
        public List<VtpIngresoPotUnidPromdDTO> ListVtpIngresoPotUnidPromdEmpresa(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().ListEmpresa(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTUNID_PROMD, mas la empresa y central
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotUnidPromdDTO</returns>
        public List<VtpIngresoPotUnidPromdDTO> ListVtpIngresoPotUnidPromdEmpresaCentral(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().ListEmpresaCentral(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_POTUNID_PROMD, mas la empresa, central y unidad
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpIngresoPotUnidPromdDTO</returns>
        public List<VtpIngresoPotUnidPromdDTO> ListVtpIngresoPotUnidPromdEmpresaCentralUnidad(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().ListEmpresaCentral2(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public List<VtpIngresoPotUnidPromdDTO> GetByCriteriaVtpIngresoPotUnidPromd()
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite generar el Reporte de Ingresos por Potencia - CU23
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteIngresoPotencia(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            string fileName = string.Empty;
            hoja = null;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa = this.ListVtpIngresoPotUnidPromdEmpresaCentral(pericodi, recpotcodi);
            List<VtpIngresoPotefrDTO> ListaIngresoPotEFR = this.GetByCriteriaVtpIngresoPotefrs(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReporteIngresoPotencia.xlsx";
                if(EntidadRecalculoPotencia.Recpotcargapfr == 1)
                {
                    ListaIngresoPotenciaEmpresa = this.ListVtpIngresoPotUnidPromdEmpresaCentralUnidad(pericodi, recpotcodi);
                    ExcelDocument.GenerarReporteIngresoPotenciaPfr(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoPotenciaEmpresa, ListaIngresoPotEFR, out hoja);
                }
                else
                {
                    ExcelDocument.GenerarReporteIngresoPotencia(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoPotenciaEmpresa, ListaIngresoPotEFR, out hoja);
                }
            }
            if (formato == 2)
            {
                fileName = "ReporteIngresoPotencia.pdf";
                PdfDocument.GenerarReporteIngresoPotencia(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoPotenciaEmpresa, ListaIngresoPotEFR, pathLogo);
            }

            return fileName;
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByComparative(int pericodiini, int pericodifin, int emprcodi, int equicodi, ref List<String> lstPeriodos, int pegrtipinfo, int recpotcodiConsulta, int marca = 0)
        {
            List<VtpIngresoPotUnidPromdDTO> lstData, lstEmpresas = new List<VtpIngresoPotUnidPromdDTO>();
            try
            {
                lstData = FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetIngresoPotUnidPromdByComparative(pericodiini, pericodifin, emprcodi, equicodi);
                lstEmpresas = FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetIngresoPotUnidPromdByComparativeUnique(pericodiini, pericodifin, emprcodi, equicodi);
                if (recpotcodiConsulta != 0)
                {
                    if (recpotcodiConsulta == 1)
                    {
                        lstData = lstData.Where(x => x.Recpotcodi == 1).ToList(); //Mensuales
                    }
                    else
                    {
                        lstData = lstData.Where(x => x.Recpotcodi != 1).ToList();//Versiones
                    }
                }


                var lDataDinstinct = lstData.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                foreach (var item in lDataDinstinct)
                {
                    lstPeriodos.Add(item.Perinombre + " " + item.Recpotnombre);
                }
                foreach (var item in lstEmpresas)
                {
                    item.lstImportesPromd = new List<decimal>();
                    foreach (var item2 in lstPeriodos)
                    {
                        VtpIngresoPotUnidPromdDTO objObjectAmount = lstData.Where(x => x.Perinombre + " " + x.Recpotnombre == item2 && x.Emprcodi == item.Emprcodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        if (marca == 1)
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : objObjectAmount.Inpuprimportepromd); //Marca Consulta
                        }
                        else
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : Math.Round(objObjectAmount.Inpuprimportepromd, 2)); //Marca Consulta
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return lstEmpresas;
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public List<VtpIngresoPotUnidPromdDTO> GetIngresoPotUnidPromdByHistComp(int pericodiini, int recpotini, int pericodifin, int recpotfin, int emprcodi, ref List<String> lstPeriodos, int pegrtipinfo, int marca = 0)
        {
            List<VtpIngresoPotUnidPromdDTO> lstData, lstEmpresas = new List<VtpIngresoPotUnidPromdDTO>();
            try
            {
                lstData = FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetIngresoPotUnidPromdByCompHist(pericodiini, recpotini, pericodifin, recpotfin, emprcodi);
                lstEmpresas = FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetIngresoPotUnidPromdByCompHistUnique(pericodiini, recpotini, pericodifin, recpotfin, emprcodi);
                var lDataDinstinct = lstData.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                foreach (var item in lDataDinstinct)
                {
                    lstPeriodos.Add(item.Perinombre + " " + item.Recpotnombre);
                }
                foreach (var item in lstEmpresas)
                {
                    item.lstImportesPromd = new List<decimal>();
                    foreach (var item2 in lstPeriodos)
                    {
                        VtpIngresoPotUnidPromdDTO objObjectAmount = lstData.Where(x => x.Perinombre + " " + x.Recpotnombre == item2 && x.Emprcodi == item.Emprcodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        if (marca == 1)
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : objObjectAmount.Inpuprimportepromd);
                        }
                        else
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : Math.Round(objObjectAmount.Inpuprimportepromd, 2));
                        }

                    }
                    if (item.lstImportesPromd.Count == 0 || item.lstImportesPromd.Count == 1)
                    {
                        item.PorcentajeVariacion = 0;
                    }
                    else
                    {
                        item.PorcentajeVariacion = item.lstImportesPromd[0] == 0 ? 0 : ((item.lstImportesPromd[0] - item.lstImportesPromd[1]) / item.lstImportesPromd[0]);
                        item.PorcentajeVariacion = item.PorcentajeVariacion < 0 ? (item.PorcentajeVariacion * -100) : (item.PorcentajeVariacion * 100);
                        item.PorcentajeVariacion = Math.Round(item.PorcentajeVariacion, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return lstEmpresas;
        }



        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_EMPRESA_PAGO
        /// </summary>
        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparative(int pericodiini, int pericodifin, int emprcodi, ref List<String> lstPeriodos, int recpotcodiConsulta, int marca = 0)
        {
            List<VtpEmpresaPagoDTO> lstData = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstData2 = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresas = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresasCobro = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresasFinal = new List<VtpEmpresaPagoDTO>();
            try
            {
                lstData = FactoryTransferencia.GetVtpEmpresaPagoRepository().GetEmpresaPagoByComparative(pericodiini, pericodifin, emprcodi);
                lstData2 = FactoryTransferencia.GetVtpEmpresaPagoRepository().GetEmpresaPagoByComparativeHistorico(pericodiini, pericodifin, emprcodi);
                lstEmpresas = FactoryTransferencia.GetVtpEmpresaPagoRepository().GetEmpresaPagoByComparativeUnique(pericodiini, pericodifin, emprcodi);
                lstEmpresasCobro = FactoryTransferencia.GetVtpEmpresaPagoRepository().ListCobroConsultaHistoricos(emprcodi, pericodiini, pericodifin);
                if (recpotcodiConsulta != 0)
                {
                    if (recpotcodiConsulta == 1)
                    {
                        lstData = lstData.Where(x => x.Recpotcodi == 1).ToList(); //Mensuales
                        lstData2 = lstData2.Where(x => x.Recpotcodi == 1).ToList();
                    }
                    else
                    {
                        lstData = lstData.Where(x => x.Recpotcodi != 1).ToList();//Versiones
                        lstData2 = lstData2.Where(x => x.Recpotcodi != 1).ToList();
                    }
                }
                var lDataDinstinct1 = lstData.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                var lDataDinstinct2 = lstData2.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                var MyCombinedList = lDataDinstinct1.Concat(lDataDinstinct2);
                var lDataDinstinct = MyCombinedList.Distinct();
                foreach (var item in lDataDinstinct)
                {
                    lstPeriodos.Add(item.Perinombre + " " + item.Recpotnombre);
                }
                foreach (var item in lstEmpresas)
                {
                    item.lstImportesPromd = new List<decimal>();
                    foreach (var item2 in lstPeriodos)
                    {
                        decimal objObjectAmount = lstData.Where(x => x.Perinombre + " " + x.Recpotnombre == item2 && x.Emprcodipago == item.Emprcodipago).Sum(x => x.Potepmonto) * -1;
                        if (marca == 1)
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : objObjectAmount);
                        }
                        else
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : Math.Round(objObjectAmount, 2));
                        }

                    }
                    lstEmpresasFinal.Add(item);
                }
                foreach (var item2 in lstEmpresasCobro)
                {
                    item2.lstImportesPromd = new List<decimal>();
                    foreach (var item3 in lstPeriodos)
                    {
                        decimal objObjectAmount = lstData2.Where(x => x.Perinombre + " " + x.Recpotnombre == item3 && x.Emprcodicobro == item2.Emprcodicobro).Sum(x => x.Potepmonto);
                        if (marca == 1)
                        {
                            item2.lstImportesPromd.Add(objObjectAmount == null ? 0 : objObjectAmount);
                        }
                        else
                        {
                            item2.lstImportesPromd.Add(objObjectAmount == null ? 0 : Math.Round(objObjectAmount, 2));
                        }
                    }
                    lstEmpresasFinal.Add(item2);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return lstEmpresasFinal.Distinct().ToList();
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_POTUNID_PROMD
        /// </summary>
        public List<VtpEmpresaPagoDTO> GetEmpresaPagoByHist(int pericodiini, int recpotini, int pericodifin, int recpotfin, int emprcodi, ref List<String> lstPeriodos, int marca = 0)
        {
            List<VtpEmpresaPagoDTO> lstData = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstData2 = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresas = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresasCobro = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresasFinal = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> empresasFinal = new List<VtpEmpresaPagoDTO>();
            try
            {
                lstData = FactoryTransferencia.GetVtpEmpresaPagoRepository().GetEmpresaPagoByHist(pericodiini, recpotini, pericodifin, recpotfin, emprcodi);
                lstData2 = FactoryTransferencia.GetVtpEmpresaPagoRepository().GetEmpresaPagoByComparativeHistorico2(pericodiini, recpotini, pericodifin, recpotfin, emprcodi);
                lstEmpresas = FactoryTransferencia.GetVtpEmpresaPagoRepository().GetEmpresaPagoByHistUnique(pericodiini, recpotini, pericodifin, recpotfin, emprcodi);
                lstEmpresasCobro = FactoryTransferencia.GetVtpEmpresaPagoRepository().ListCobroConsultaHistoricos(emprcodi, pericodiini, pericodifin);

                var lDataDinstinct1 = lstData.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                var lDataDinstinct2 = lstData2.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                var MyCombinedList = lDataDinstinct1.Concat(lDataDinstinct2);
                var lDataDinstinct = MyCombinedList.Distinct();
                foreach (var item in lDataDinstinct)
                {
                    lstPeriodos.Add(item.Perinombre + " " + item.Recpotnombre);
                }

                foreach (var item in lstEmpresas)
                {
                    item.lstImportesPromd = new List<decimal>();
                    foreach (var item2 in lstPeriodos)
                    {
                        decimal objObjectAmount = lstData.Where(x => x.Perinombre + " " + x.Recpotnombre == item2 && x.Emprcodipago == item.Emprcodipago).Sum(x => x.Potepmonto) * -1;
                        if (marca == 1)
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : objObjectAmount);
                        }
                        else
                        {
                            item.lstImportesPromd.Add(objObjectAmount == null ? 0 : Math.Round(objObjectAmount, 2));
                        }

                    }
                    if (item.lstImportesPromd.Count == 0 || item.lstImportesPromd.Count == 1)
                    {
                        item.PorcentajeVariacion = 0;
                    }
                    else
                    {
                        item.PorcentajeVariacion = item.lstImportesPromd[0] == 0 ? 0 : ((item.lstImportesPromd[0] - item.lstImportesPromd[1]) / item.lstImportesPromd[0]);
                        item.PorcentajeVariacion = item.PorcentajeVariacion < 0 ? (item.PorcentajeVariacion * -100) : (item.PorcentajeVariacion * 100);
                        item.PorcentajeVariacion = Math.Round(item.PorcentajeVariacion, 2);
                    }
                    lstEmpresasFinal.Add(item);
                }
                foreach (var item2 in lstEmpresasCobro)
                {
                    item2.lstImportesPromd = new List<decimal>();
                    foreach (var item3 in lstPeriodos)
                    {
                        decimal objObjectAmount = lstData2.Where(x => x.Perinombre + " " + x.Recpotnombre == item3 && x.Emprcodicobro == item2.Emprcodicobro).Sum(x => x.Potepmonto);
                        if (marca == 1)
                        {
                            item2.lstImportesPromd.Add(objObjectAmount == null ? 0 : objObjectAmount);
                        }
                        else
                        {
                            item2.lstImportesPromd.Add(objObjectAmount == null ? 0 : Math.Round(objObjectAmount, 2));
                        }

                    }
                    if (item2.lstImportesPromd.Count == 0 || item2.lstImportesPromd.Count == 1)
                    {
                        item2.PorcentajeVariacion = 0;
                    }
                    else
                    {
                        item2.PorcentajeVariacion = item2.lstImportesPromd[0] == 0 ? 0 : ((item2.lstImportesPromd[0] - item2.lstImportesPromd[1]) / item2.lstImportesPromd[0]);
                        item2.PorcentajeVariacion = item2.PorcentajeVariacion < 0 ? (item2.PorcentajeVariacion * -100) : (item2.PorcentajeVariacion * 100);
                        item2.PorcentajeVariacion = Math.Round(item2.PorcentajeVariacion, 2);
                    }
                    lstEmpresasFinal.Add(item2);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return lstEmpresasFinal.Distinct().ToList();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByComparative(int pericodiini, int pericodifin, int emprcodi, int emprcargo, ref List<String> lstPeriodos, List<int> cargos, int recpotcodiConsulta, int marca = 0)
        {
            List<VtpPeajeEmpresaPagoDTO> lstEmpresaPago = this.ListVtpPeajeEmpresaPagoPeajePago(pericodiini, 1);
            List<VtpPeajeEmpresaPagoDTO> lstData, lstEmpresas = new List<VtpPeajeEmpresaPagoDTO>();
            List<VtpPeajeEmpresaPagoDTO> lstTemporal = new List<VtpPeajeEmpresaPagoDTO>();
            List<VtpPeajeEmpresaPagoDTO> lstFinal = new List<VtpPeajeEmpresaPagoDTO>();
            try
            {
                lstData = FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetPeajeEmpresaPagoByComparative(pericodiini, pericodifin, 2, 0);
                if (recpotcodiConsulta != 0)
                {
                    if (recpotcodiConsulta == 1)
                    {
                        lstData = lstData.Where(x => x.Recpotcodi == 1).ToList(); //Mensuales

                    }
                    else
                    {
                        lstData = lstData.Where(x => x.Recpotcodi != 1).ToList();//Versiones

                    }
                }
                var lDataDinstinct = lstData.Select(x => new { x.Perinombre, x.Recpotnombre }).Distinct().ToList();
                var lDataDinstinctPeriodos = lstData.Select(x => new { x.Perinombre, x.Recpotnombre, x.Pericodi, x.Recpotcodi }).Distinct().ToList();
                foreach (var item in lDataDinstinct)
                {
                    lstPeriodos.Add(item.Perinombre + " " + item.Recpotnombre);
                }
                int iEmprcodiPago = lstEmpresaPago[0].Emprcodipeaje;
                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroHistoricos(iEmprcodiPago, pericodiini, 1);
                int rowTemporal = 0;
                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                {
                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                    {
                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                    }
                    lstTemporal.Add(dtoEmpresaCobro);
                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(pericodiini, 1, iPingcodi);
                    if (dtoPeajeIngreso != null)
                    {
                        lstTemporal[rowTemporal].Pingnombre = dtoPeajeIngreso.Pingnombre;
                    }
                    rowTemporal++;
                }
                foreach (var item in lstTemporal)
                {
                    item.lstImportesPromd = new List<decimal>();
                    decimal total = 0;
                    foreach (var item2 in lstPeriodos)
                    {
                        total = 0;
                        var periodo = lDataDinstinctPeriodos.Find(y => y.Perinombre + " " + y.Recpotnombre == item2);
                        List<VtpPeajeEmpresaPagoDTO> lstPeajeEmpresaPago = FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetPeajeEmpresaPagoByEmprCodiAndRecPotCodi(item.Emprcodicargo, periodo.Pericodi, periodo.Recpotcodi, item.Pingcodi);
                        foreach (VtpPeajeEmpresaPagoDTO item3 in lstPeajeEmpresaPago)
                        {
                            total += item3.Pempagpeajepago + item3.Pempagsaldoanterior + item3.Pempagajuste;
                        }
                        if (marca == 1)
                        {
                            item.lstImportesPromd.Add(total);
                        }
                        else
                        {
                            item.lstImportesPromd.Add(Math.Round(total, 2));
                        }

                    }
                }
                if (emprcodi > 0)
                {
                    lstTemporal = lstTemporal.Where(x => x.Emprcodicargo == emprcodi).ToList();
                }

                if (cargos.Count > 0)
                {
                    for (int i = 0; i < cargos.Count; i++)
                    {
                        for (int j = 0; j < lstTemporal.Count; j++)
                        {
                            if (lstTemporal[j].Pingcodi == cargos[i])
                            {
                                lstFinal.Add(lstTemporal[j]);
                            }
                        }
                    }
                    lstFinal = lstFinal.OrderBy(item => item.Emprnombcargo).ToList();
                }
                else
                {
                    lstFinal = lstTemporal;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return lstFinal;
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByHist(int pericodiini, int recpotini, int pericodifin, int recpotfin, int emprcodi, ref List<String> lstPeriodos, int marca = 0)
        {
            List<VtpPeajeEmpresaPagoDTO> lstEmpresaPago = this.ListVtpPeajeEmpresaPagoPeajePago(pericodiini, 1);
            List<VtpPeajeEmpresaPagoDTO> lstData, lstEmpresas = new List<VtpPeajeEmpresaPagoDTO>();
            List<VtpPeajeEmpresaPagoDTO> lstTemporal = new List<VtpPeajeEmpresaPagoDTO>();
            try
            {
                lstData = FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetPeajeEmpresaPagoByComparative(pericodiini, pericodifin, 2, 0);
                var lDataDinstinct = lstData.Select(x => new { x.Perinombre, x.Recpotnombre, x.Pericodi, x.Recpotcodi }).Distinct().ToList();
                var lDataDinstinctPeriodos = lstData.Select(x => new { x.Perinombre, x.Recpotnombre, x.Pericodi, x.Recpotcodi }).Distinct().ToList();
                lDataDinstinct = lDataDinstinct.Where(item => (item.Pericodi == pericodiini && item.Recpotcodi == recpotini) || item.Pericodi == pericodifin && item.Recpotcodi == recpotfin).ToList();
                foreach (var item in lDataDinstinct)
                {
                    lstPeriodos.Add(item.Perinombre + " " + item.Recpotnombre);
                }
                int iEmprcodiPago = lstEmpresaPago[0].Emprcodipeaje;
                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobroHistoricos(iEmprcodiPago, pericodiini, 1);
                int rowTemporal = 0;
                foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                {
                    if (dtoEmpresaCobro.Emprcodicargo == 10582)
                    {
                        Dominio.DTO.Sic.SiEmpresaDTO empresa = Factory.FactorySic.GetSiEmpresaRepository().GetById(11153);
                        dtoEmpresaCobro.Emprnombcargo = empresa.Emprnomb;
                    }
                    lstTemporal.Add(dtoEmpresaCobro);
                    int iPingcodi = dtoEmpresaCobro.Pingcodi;
                    VtpPeajeIngresoDTO dtoPeajeIngreso = (new TransfPotenciaAppServicio()).GetByIdVtpPeajeIngreso(pericodiini, 1, iPingcodi);
                    if (dtoPeajeIngreso != null)
                    {
                        lstTemporal[rowTemporal].Pingnombre = dtoPeajeIngreso.Pingnombre;
                    }
                    rowTemporal++;
                }
                foreach (var item in lstTemporal)
                {
                    item.lstImportesPromd = new List<decimal>();
                    decimal total = 0;
                    foreach (var item2 in lstPeriodos)
                    {
                        total = 0;
                        var periodo = lDataDinstinctPeriodos.Find(y => y.Perinombre + " " + y.Recpotnombre == item2);
                        List<VtpPeajeEmpresaPagoDTO> lstPeajeEmpresaPago = FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetPeajeEmpresaPagoByEmprCodiAndRecPotCodi(item.Emprcodicargo, periodo.Pericodi, periodo.Recpotcodi, item.Pingcodi);
                        foreach (VtpPeajeEmpresaPagoDTO item3 in lstPeajeEmpresaPago)
                        {
                            total += item3.Pempagpeajepago + item3.Pempagsaldoanterior + item3.Pempagajuste;
                        }
                        if (marca == 1)
                        {
                            item.lstImportesPromd.Add(total);
                        }
                        else
                        {
                            item.lstImportesPromd.Add(Math.Round(total, 2));
                        }

                    }
                    if (item.lstImportesPromd.Count == 0 || item.lstImportesPromd.Count == 1)
                    {
                        item.PorcentajeVariacion = 0;
                    }
                    else
                    {
                        item.PorcentajeVariacion = item.lstImportesPromd[0] == 0 ? 0 : ((item.lstImportesPromd[0] - item.lstImportesPromd[1]) / item.lstImportesPromd[0]);
                        item.PorcentajeVariacion = item.PorcentajeVariacion < 0 ? (item.PorcentajeVariacion * -100) : (item.PorcentajeVariacion * 100);
                        item.PorcentajeVariacion = Math.Round(item.PorcentajeVariacion, 2);
                    }
                }
                if (emprcodi > 0)
                {
                    lstTemporal = lstTemporal.Where(x => x.Emprcodicargo == emprcodi).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return lstTemporal;
        }


        //CU21
        /// <summary>
        /// Permite obtener el Ingreso por potencia de una Central de forma directa
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="equicodi">Código de la Central</param>
        /// <returns>Lista de VtpIngresoPotUnidPromdDTO</returns>
        public VtpIngresoPotUnidPromdDTO GetByVtpIngresoPotUnidPromdCentral(int pericodi, int recpotcodi, int equicodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetByCentral(pericodi, recpotcodi, equicodi);
        }

        /// <summary>
        /// Permite obtener el Ingreso por potencia de una Central sumando las unidades
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="equicodi">Código de la Central</param>
        /// <returns>Lista de VtpIngresoPotUnidPromdDTO</returns>
        public VtpIngresoPotUnidPromdDTO GetByVtpIngresoPotUnidPromdCentralSumUnidades(int pericodi, int recpotcodi, int equicodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotUnidPromdRepository().GetByCentralSumUnidades(pericodi, recpotcodi, equicodi);
        }
        #endregion

        #region Métodos Tabla VTP_INGRESO_TARIFARIO_AJUSTE

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        public int SaveVtpIngresoTarifarioAjuste(VtpIngresoTarifarioAjusteDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        public void UpdateVtpIngresoTarifarioAjuste(VtpIngresoTarifarioAjusteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        public void DeleteVtpIngresoTarifarioAjuste(int ingtajcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().Delete(ingtajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_INGRESO_TARIFARIO_AJUSTE asociados a un mes de valorización
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        public void DeleteByCriteriaVtpIngresoTarifarioAjuste(int pericodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().DeleteByCriteria(pericodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        public VtpIngresoTarifarioAjusteDTO GetByIdVtpIngresoTarifarioAjuste(int ingtajcodi)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().GetById(ingtajcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        public List<VtpIngresoTarifarioAjusteDTO> ListVtpIngresoTarifarioAjuste()
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        public List<VtpIngresoTarifarioAjusteDTO> GetByCriteriaVtpIngresoTarifarioAjuste(int pericodi)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().GetByCriteria(pericodi);
        }

        /// <summary>
        /// Permite obtener el saldo a aplicar en este periodo de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización donde se aplica el ajuste</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <param name="pingcodi">Código del peaje ingreso</param>
        /// <returns>Ajuste del mes</returns>
        public decimal GetIngresoTarifarioAjuste(int pericodi, int emprcodiping, int pingcodi, int emprcodingpot)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioAjusteRepository().GetAjuste(pericodi, emprcodiping, pingcodi, emprcodingpot);
        }
        #endregion

        #region Métodos Tabla VTP_INGRESO_TARIFARIO

        /// <summary>
        /// Inserta un registro de la tabla VTP_INGRESO_TARIFARIO
        /// </summary>
        public int SaveVtpIngresoTarifario(VtpIngresoTarifarioDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpIngresoTarifarioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_INGRESO_TARIFARIO
        /// </summary>
        public void UpdateVtpIngresoTarifario(VtpIngresoTarifarioDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los registro de la tabla VTP_INGRESO_TARIFARIO, para un pericodi y recpotcodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="Pericodidestino">Código del Mes de valorización sonde se aplicara el saldo por cargo</param>
        public void UpdateVtpIngresoTarifarioPeriodoDestino(int pericodi, int recpotcodi, int ingtarpericodidest)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioRepository().UpdatePeriodoDestino(pericodi, recpotcodi, ingtarpericodidest);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_INGRESO_TARIFARIO
        /// </summary>
        public void DeleteVtpIngresoTarifario(int ingtarcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioRepository().Delete(ingtarcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_INGRESO_TARIFARIO por mes de valorización y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpIngresoTarifario(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpIngresoTarifarioRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_TARIFARIO
        /// </summary>
        public VtpIngresoTarifarioDTO GetByIdVtpIngresoTarifario(int ingtarcodi)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().GetById(ingtarcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_INGRESO_TARIFARIO consultado por pericodi, recpotcodi, pingcodi, emprecodiping. 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pingcodi">Código del cargo</param>
        /// <param name="emprcodingpot">Código del empresa</param>
        public VtpIngresoTarifarioDTO GetByIdVtpIngresoTarifarioSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodiping, int emprcodingpot)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().GetByIdSaldo(pericodi, recpotcodi, pingcodi, emprcodiping, emprcodingpot);
        }

        /// <summary>
        /// Permite obtener los saldos de las empresas anteriores
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="recpotcodi"></param>
        /// <param name="pingcodi"></param>
        /// <param name="emprcodiping"></param>
        /// <param name="emprcodingpot"></param>
        /// <returns></returns>
        public List<VtpIngresoTarifarioDTO> GetByCriteriaIngresoTarifarioSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodiping, int emprcodingpot)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().GetByCriteriaIngresoTarifarioSaldo(pericodi, recpotcodi, pingcodi, emprcodiping, emprcodingpot);
        }

        /// <summary>
        /// Permite obtener el saldo anterior para un periodo y empresa de la tabla VTP_INGRESO_TARIFARIO
        /// </summary>
        /// <param name="ingtarpericodidest">Código del Mes de valorización de saldos</param>
        /// <param name="pingcodi">Código del cargo</param>
        /// <param name="emprcodingpot">Código del empresa</param>
        /// <returns>El saldo de la empresa</returns>
        public decimal GetIngresoTarifarioSaldoAnterior(int ingtarpericodidest, int pingcodi, int emprcodiping, int emprcodingpot)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().GetSaldoAnterior(ingtarpericodidest, pingcodi, emprcodiping, emprcodingpot);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_TARIFARIO
        /// </summary>
        public List<VtpIngresoTarifarioDTO> ListVtpIngresoTarifarios()
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().List();
        }

        /// <summary>
        /// Permite listar todas las empresas (Emprcodingpot y Emprnombringpot) de la tabla VTP_INGRESO_TARIFARIO que han realizado un pago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpIngresoTarifarioDTO> ListVtpIngresoTarifariosEmpresaPago(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().ListEmpresaPago(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_TARIFARIO de las Emprnombringpot, Emprnombping y Ingtarimporte
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpIngresoTarifarioDTO> ListVtpIngresoTarifariosEmpresaCobro(int emprcodingpot, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().ListEmpresaCobro(emprcodingpot, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_INGRESO_TARIFARIO de las Emprnombringpot, Emprnombping y Ingtarimporte para multiples emprcodingpot
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpIngresoTarifarioDTO> ListVtpIngresoTarifarioEmpresaCobroParaMulEmprcodingpot(string emprcodingpot, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().ListEmpresaCobroParaMultEmprcodingpot(emprcodingpot, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpIngresoTarifario
        /// </summary>
        public List<VtpIngresoTarifarioDTO> GetByCriteriaVtpIngresoTarifarios()
        {
            return FactoryTransferencia.GetVtpIngresoTarifarioRepository().GetByCriteria();
        }


        /// <summary>
        /// Permite generar el Reporte de Compensación a transmisoras por ingreso tarifario - CU19
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteIngresoTarifario(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago = this.ListVtpIngresoTarifariosEmpresaPago(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReporteIngresoTarifario.xlsx";
                List<double> totales = new List<double>();
                List<double> mensules = new List<double>();
                List<double> saldos = new List<double>();
                ExcelDocument.GenerarReporteIngresoTarifario(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoTarifarioPago, out hoja, out totales, out mensules, out saldos);
            }
            if (formato == 2)
            {
                fileName = "ReporteIngresoTarifario.pdf";
                PdfDocument.GenerarReporteIngresoTarifario(pathFile + fileName, EntidadRecalculoPotencia, ListaIngresoTarifarioPago, pathLogo);
            }

            return fileName;
        }
        #endregion

        #region Métodos tabla VTP_PAGO_EGRESO

        /// <summary>
        /// Inserta un registro de la tabla VTP_PAGO_EGRESO
        /// </summary>
        public void SaveVtpPagoEgreso(VtpPagoEgresoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPagoEgresoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PAGO_EGRESO
        /// </summary>
        public void UpdateVtpPagoEgreso(VtpPagoEgresoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPagoEgresoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PAGO_EGRESO
        /// </summary>
        public void DeleteVtpPagoEgreso(int pagegrcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPagoEgresoRepository().Delete(pagegrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_PAGO_EGRESO por mes de valorización y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpPagoEgreso(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPagoEgresoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PAGO_EGRESO
        /// </summary>
        public VtpPagoEgresoDTO GetByIdVtpPagoEgreso(int pagegrcodi)
        {
            return FactoryTransferencia.GetVtpPagoEgresoRepository().GetById(pagegrcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PAGO_EGRESO, ademas trae el nombre de la empresa
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpPagoEgresoDTO> ListVtpPagoEgreso(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPagoEgresoRepository().List(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PAGO_EGRESO
        /// </summary>
        public List<VtpPagoEgresoDTO> GetByCriteriaVtpPagoEgreso(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPagoEgresoRepository().GetByCriteria(pericodi, recpotcodi);
        }

        #endregion

        #region Métodos Tabla VTP_PEAJE_CARGO

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_CARGO
        /// </summary>
        public int SaveVtpPeajeCargo(VtpPeajeCargoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpPeajeCargoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_CARGO
        /// </summary>
        public void UpdateVtpPeajeCargo(VtpPeajeCargoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los registro de la tabla VTP_PEAJE_CARGO, para un pericodi y recpotcodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="Pericodidestino">Código del Mes de valorización sonde se aplicara el saldo por cargo</param>
        public void UpdateVtpPeajeCargoPeriodoDestino(int pericodi, int recpotcodi, int pecarpericodidest)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoRepository().UpdatePeriodoDestino(pericodi, recpotcodi, pecarpericodidest);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_CARGO
        /// </summary>
        public void DeleteVtpPeajeCargo(int pecarcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoRepository().Delete(pecarcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_PEAJE_CARGO asociados a un mes de valorización y a una versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpPeajeCargo(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_CARGO
        /// </summary>
        public VtpPeajeCargoDTO GetByIdVtpPeajeCargo(int pecarcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().GetById(pecarcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_CARGO a partir de: pericodi, recpotcodi, emprcodi y pingcodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <param name="pingcodi">Código del peaje ingreso</param>
        /// <returns>VtpPeajeCargoDTO</returns>
        public VtpPeajeCargoDTO GetByIdVtpPeajeCargoSaldo(int pericodi, int recpotcodi, int emprcodi, int pingcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().GetByIdSaldo(pericodi, recpotcodi, emprcodi, pingcodi);
        }

        /// <summary>
        /// Permite obtener los registros con saldo a aplicar en este periodo de la tabla VTP_PEAJE_CARGO
        /// </summary>
        /// <param name="pecarpericodidest">Código del Mes de valorización donde se aplica el saldo</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <param name="pingcodi">Código del peaje ingreso</param>
        /// <returns>Saldo de meses anteriores</returns>
        public decimal GetPeajeCargoSaldoAnterior(int pecarpericodidest, int emprcodi, int pingcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().GetSaldoAnterior(pecarpericodidest, emprcodi, pingcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_CARGO
        /// </summary>
        public List<VtpPeajeCargoDTO> ListVtpPeajeCargo()
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().List();
        }

        /// <summary>
        /// Permite listar todas las empresas (EmprCodi y EmprNomb) de la tabla VTP_PEAJE_CARGO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeCargoDTO</returns>
        public List<VtpPeajeCargoDTO> ListVtpPeajeCargoEmpresa(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().ListEmpresa(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todas las empresas (EmprCodi y EmprNomb) de la tabla VTP_PEAJE_CARGO
        /// </summary>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeCargoDTO</returns>
        public List<VtpPeajeCargoDTO> ListVtpPeajeCargoPagoNo(int emprcodi, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().ListPagoNo(emprcodi.ToString(), pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todas las empresas (EmprCodi y EmprNomb) de la tabla VTP_PEAJE_CARGO
        /// </summary>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeCargoDTO</returns>
        public List<VtpPeajeCargoDTO> ListVtpPeajeCargoPagoNo(string emprcodi, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().ListPagoNo(emprcodi, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todas las empresas (EmprCodi y EmprNomb) de la tabla VTP_PEAJE_CARGO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pingcodi">Código del Cargo</param>
        /// <returns>Lista de VtpPeajeCargoDTO</returns>
        public List<VtpPeajeCargoDTO> ListVtpPeajeCargoPagoAdicional(int pericodi, int recpotcodi, int pingcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().ListPagoAdicional(pericodi, recpotcodi, pingcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_CARGO
        /// </summary>
        public List<VtpPeajeCargoDTO> GetByCriteriaVtpPeajeCargo()
        {
            return FactoryTransferencia.GetVtpPeajeCargoRepository().GetByCriteria();
        }
        /// <summary>
        /// Permite generar el Reporte de Compensaciones incluidas en el peaje por conexión - CU20
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReportePeajeRecaudado(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            string fileName = string.Empty;
            hoja = null;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeCargoDTO> ListaPeajeCargoEmpresa = this.ListVtpPeajeCargoEmpresa(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReportePeajeRecaudado.xlsx";
                ExcelDocument.GenerarReportePeajeRecaudado(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeCargoEmpresa, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReportePeajeRecaudado.pdf";
                PdfDocument.GenerarReportePeajeRecaudado(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeCargoEmpresa, pathLogo);
            }

            return fileName;
        }
        #endregion

        #region Métodos Tabla VTP_PEAJE_CARGO_AJUSTE

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        public int SaveVtpPeajeCargoAjuste(VtpPeajeCargoAjusteDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        public void UpdateVtpPeajeCargoAjuste(VtpPeajeCargoAjusteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        public void DeleteVtpPeajeCargoAjuste(int pecajcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().Delete(pecajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_PEAJE_CARGO_AJUSTE asociados a un mes de valorización
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        public void DeleteByCriteriaVtpPeajeCargoAjuste(int pericodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().DeleteByCriteria(pericodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        public VtpPeajeCargoAjusteDTO GetByIdVtpPeajeCargoAjuste(int pecajcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().GetById(pecajcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        public List<VtpPeajeCargoAjusteDTO> ListVtpPeajeCargoAjuste()
        {
            return FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        public List<VtpPeajeCargoAjusteDTO> GetByCriteriaVtpPeajeCargoAjuste(int pericodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().GetByCriteria(pericodi);
        }

        /// <summary>
        /// Permite obtener el saldo a aplicar en este periodo de la tabla VTP_PEAJE_CARGO_AJUSTE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización donde se aplica el ajuste</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <param name="pingcodi">Código del peaje ingreso</param>
        /// <returns>Ajuste del mes</returns>
        public decimal GetPeajeCargoAjuste(int pericodi, int emprcodi, int pingcodi)
        {
            return FactoryTransferencia.GetVtpPeajeCargoAjusteRepository().GetAjuste(pericodi, emprcodi, pingcodi);
        }
        #endregion

        #region Métodos Tabla VTP_PEAJE_EGRESO

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        public int SaveVtpPeajeEgreso(VtpPeajeEgresoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpPeajeEgresoRepository().Save(entity);

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        public void UpdateVtpPeajeEgreso(VtpPeajeEgresoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        public void DeleteVtpPeajeEgreso(int pegrcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoRepository().Delete(pegrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla VTP_PEAJE_EGRESO en un periodo y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        private void DeleteByCriteriaVtpPeajeEgreso(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        public VtpPeajeEgresoDTO GetByIdVtpPeajeEgreso(int pegrcodi)
        {
            try
            {
                return FactoryTransferencia.GetVtpPeajeEgresoRepository().GetById(pegrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpPeajeEgresoDTO> ListVtpPeajeEgresos(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoRepository().List(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpPeajeEgresoDTO> ListVtpPeajeEgresosView(int emprcodi, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoRepository().ListView(emprcodi, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpPeajeEgresoDTO> ObtenerReporteEnvioPorEmpresa(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoRepository().ObtenerReporteEnvioPorEmpresa(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpPeajeEgreso
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public VtpPeajeEgresoDTO GetByCriteriaVtpPeajeEgresos(int emprcodi, int pericodi, int recpotcodi)
        {
            try
            {
                return FactoryTransferencia.GetVtpPeajeEgresoRepository().GetByCriteria(emprcodi, pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registros de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void UpdateByCriteriaVtpPeajeEgreso(int emprcodi, int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoRepository().UpdateByCriteria(emprcodi, pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        ///  Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="periCodi"></param>
        /// <param name="recPotCodi"></param>
        /// <param name="pegrCodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="formato"></param>
        /// <param name="pathFile"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormatoPeajeEgreso(int periCodi, int recPotCodi, int pegrCodi, int emprcodi, int formato, string pathFile, string pathLogo)
        {
            BarraAppServicio servicioBarra = new BarraAppServicio();
            EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
            string fileName = string.Empty;
            try
            {
                PeriodoDTO periodo = new PeriodoAppServicio().GetByIdPeriodo(periCodi);
                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(periCodi, recPotCodi);
                VtpPeajeEgresoDTO EntidadPeajeEgreso = this.GetByIdVtpPeajeEgreso(pegrCodi);
                List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle = this.GetByCriteriaVtpPeajeEgresoDetallesNuevo(pegrCodi, recPotCodi, emprcodi, periCodi);
                List<BarraDTO> ListaBarras = servicioBarra.ListVista();
                List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas = servicioEmpresa.ListEmpresas();
                if (formato == 1)
                {
                    fileName = "ReportePeajeEgreso.xlsx";
                    if (periodo.PeriFormNuevo == 1)
                        ExcelDocument.GenerarFormatoPeajeEgresoNuevo(pathFile + fileName, EntidadRecalculoPotencia, EntidadPeajeEgreso, ListaPeajeEgresoDetalle, ListaEmpresas, ListaBarras);
                    else
                        ExcelDocument.GenerarFormatoPeajeEgreso(pathFile + fileName, EntidadRecalculoPotencia, EntidadPeajeEgreso, ListaPeajeEgresoDetalle, ListaEmpresas, ListaBarras);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return fileName;
        }

        /// <summary>
        ///  Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="periCodi"></param>
        /// <param name="recPotCodi"></param>
        /// <param name="pegrCodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="formato"></param>
        /// <param name="pathFile"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormatoPeajeEgresoEnvio(int periCodi, int recPotCodi, int emprcodi, int pegrCodi, int formato, string pathFile, string pathLogo)
        {
            BarraAppServicio servicioBarra = new BarraAppServicio();
            EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
            string fileName = string.Empty;
            try
            {
                PeriodoDTO periodo = new PeriodoAppServicio().GetByIdPeriodo(periCodi);
                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(periCodi, recPotCodi);
                VtpPeajeEgresoDTO EntidadPeajeEgreso = this.GetByIdVtpPeajeEgreso(pegrCodi);
                List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle = this.GetByCriteriaVtpPeajeEgresoDetalles(pegrCodi);
                List<BarraDTO> ListaBarras = servicioBarra.ListVista();
                List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas = servicioEmpresa.ListEmpresas();
                if (formato == 1)
                {
                    fileName = "ReportePeajeEgreso.xlsx";
                    if (periodo.PeriFormNuevo == 1)
                        ExcelDocument.GenerarFormatoPeajeEgresoNuevo(pathFile + fileName, EntidadRecalculoPotencia, EntidadPeajeEgreso, ListaPeajeEgresoDetalle, ListaEmpresas, ListaBarras);
                    else
                        ExcelDocument.GenerarFormatoPeajeEgreso(pathFile + fileName, EntidadRecalculoPotencia, EntidadPeajeEgreso, ListaPeajeEgresoDetalle, ListaEmpresas, ListaBarras);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return fileName;
        }



        /// <summary>
        ///  Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="periCodi"></param>
        /// <param name="recPotCodi"></param>
        /// <param name="pegrCodi"></param>
        /// <param name="formato"></param>
        /// <param name="pathFile"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarReporteConsultaHistoricos(int periini, int recpotini, int perifin, int recpotfin, int emprcodi, int equicodi, int formato, string pathFile, string pathLogo, int opt, int pegrtipinfo, string datos, int recpotcodiConsulta)
        {
            string fileName = string.Empty;
            List<VtpIngresoPotUnidPromdDTO> lstData = new List<VtpIngresoPotUnidPromdDTO>();
            List<VtpEmpresaPagoDTO> lstData2 = new List<VtpEmpresaPagoDTO>();
            List<VtpPeajeEmpresaPagoDTO> lstData3 = new List<VtpPeajeEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresasBack = new List<VtpEmpresaPagoDTO>();
            List<VtpEmpresaPagoDTO> lstEmpresasBack2 = new List<VtpEmpresaPagoDTO>();
            try
            {
                List<String> lstPeriodos = new List<String>();
                if (pegrtipinfo == 0)
                {
                    if (opt == 1)
                    {
                        lstData = this.GetIngresoPotUnidPromdByComparative(periini, perifin, emprcodi, equicodi, ref lstPeriodos, pegrtipinfo, recpotcodiConsulta, 1);
                    }
                    else
                    {
                        lstData = this.GetIngresoPotUnidPromdByHistComp(periini, recpotini, perifin, recpotfin, emprcodi, ref lstPeriodos, pegrtipinfo, 1);
                    }
                }
                else if (pegrtipinfo == 1)
                {
                    if (opt == 1)
                    {
                        lstData2 = this.GetEmpresaPagoByComparative(periini, perifin, emprcodi, ref lstPeriodos, recpotcodiConsulta, 1);
                    }
                    else
                    {
                        lstData2 = this.GetEmpresaPagoByHist(periini, recpotini, perifin, recpotfin, emprcodi, ref lstPeriodos, 1);
                    }
                    foreach (VtpEmpresaPagoDTO item2 in lstData2)
                    {
                        VtpEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodipago == item2.Emprcodipago);
                        if (itemEmpresa == null && item2.lstImportesPromd.Count() > 0)
                        {
                            lstEmpresasBack.Add(item2);
                        }
                    }
                    foreach (VtpEmpresaPagoDTO item3 in lstData2)
                    {
                        VtpEmpresaPagoDTO itemEmpresa = lstEmpresasBack.Find(x => x.Emprcodicobro == item3.Emprcodicobro && x.Emprcodicobro != 0);
                        if (itemEmpresa == null && item3.lstImportesPromd.Count() > 0)
                        {
                            lstEmpresasBack.Add(item3);
                        }
                    }
                    lstEmpresasBack = lstEmpresasBack
                         .GroupBy(p => p.lstImportesPromd.Sum())
                         .Select(g => g.First())
                         .OrderBy(x => x.Emprnombpago)
                         .ToList();
                    List<VtpEmpresaPagoDTO> empresasFinal = new List<VtpEmpresaPagoDTO>();
                    foreach (var empresa in lstEmpresasBack)
                    {
                        var empresafiltro = empresasFinal.FirstOrDefault(x => x.Emprnombpago == empresa.Emprnombpago);
                        if (empresafiltro != null)
                        {
                            for (int i = 0; i < empresafiltro.lstImportesPromd.Count(); ++i)
                            {
                                empresafiltro.lstImportesPromd[i] = empresafiltro.lstImportesPromd[i] + empresa.lstImportesPromd[i];
                            }
                        }
                        else
                        {
                            empresasFinal.Add(empresa);
                        }

                    }
                    foreach (var x in empresasFinal)
                    {
                        if (x.lstImportesPromd.Count == 0 || x.lstImportesPromd.Count == 1)
                        {
                            x.PorcentajeVariacion = 0;
                        }
                        else
                        {
                            x.PorcentajeVariacion = x.lstImportesPromd[0] == 0 ? 0 : ((x.lstImportesPromd[0] - x.lstImportesPromd[1]) / x.lstImportesPromd[0]);
                            x.PorcentajeVariacion = x.PorcentajeVariacion < 0 ? (x.PorcentajeVariacion * -100) : (x.PorcentajeVariacion * 100);
                            x.PorcentajeVariacion = Math.Round(x.PorcentajeVariacion, 2);
                        }
                    }
                    lstEmpresasBack2 = empresasFinal;
                }
                else
                {
                    if (opt == 1)
                    {
                        List<int> lstCargos = new List<int>();
                        if (datos != "[]")
                        {
                            string dataStr = JsonConvert.DeserializeObject<string>(datos);
                            string[] dataList = dataStr.Split(',');
                            for (int i = 0; i < dataList.Length; i++)
                            {
                                lstCargos.Add(int.Parse(dataList[i]));
                            }
                        }
                        lstData3 = this.GetPeajeEmpresaPagoByComparative(periini, perifin, emprcodi, equicodi, ref lstPeriodos, lstCargos, recpotcodiConsulta, 1);
                    }
                    else
                    {
                        lstData3 = this.GetPeajeEmpresaPagoByHist(periini, recpotini, perifin, recpotfin, emprcodi, ref lstPeriodos, 1);
                    }
                }

                if (formato == 1)
                {
                    fileName = opt == 1 ? "ReporteConsultaDatosHistoricos.xlsx" : "ReportComparacionDatosHistoricos.xlsx";
                    ExcelDocument.GenerarFormatoConsultaDatosHistoricos(pathFile + fileName, lstData, lstEmpresasBack2, lstData3, lstPeriodos, opt, pegrtipinfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return fileName;
        }


        /// <summary>
        ///  Permite generar el archivo de exportación de la tabla VTP_VALIDACION_ENVRIO
        /// </summary>
        /// <param name="pegrCodi"></param>
        /// <param name="formato"></param>
        /// <param name="pathFile"></param>
        /// <param name="pathLogo"></param>
        /// <returns></returns>
        public string GenerarFormatoValidacionesEnvio(int periCodi, int recPotCodi, int pegrCodi, string enterprisename, int formato, string pathFile, string pathLogo)
        {
            TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
            string fileName = string.Empty;
            try
            {
                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(periCodi, recPotCodi);
                VtpPeajeEgresoDTO EntidadPeajeEgreso = this.GetByIdVtpPeajeEgreso(pegrCodi);
                List<VtpValidacionEnvioDTO> lstValidacionEnvio = servicioTransfPotencia.GetValidacionEnvioByPegrcodi(pegrCodi);
                if (formato == 1)
                {
                    fileName = "VTP_" + enterprisename.Replace(" ", "") + "_Envio" + pegrCodi + "_Validaciones.xlsx";
                    ExcelDocument.GenerarFormatoValidacionesEnvio(pathFile + fileName, EntidadRecalculoPotencia, EntidadPeajeEgreso, lstValidacionEnvio);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pegrcodi">Código del Peaje Egreso</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoPeajeEgresoAnterior(int pericodi, int recpotcodi, int pegrcodi, int formato, string pathFile, string pathLogo)
        {
            BarraAppServicio servicioBarra = new BarraAppServicio();
            EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();

            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            VtpPeajeEgresoDTO EntidadPeajeEgreso = this.GetByIdVtpPeajeEgreso(pegrcodi);
            List<VtpPeajeEgresoDetalleDTO> ListaPeajeEgresoDetalle = this.ListVtpPeajeEgresoDetallesView(pegrcodi);
            List<BarraDTO> ListaBarras = servicioBarra.ListVista();
            List<COES.Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas = servicioEmpresa.ListEmpresas();
            if (formato == 1)
            {
                fileName = "ReportePeajeEgreso.xlsx";
                ExcelDocument.GenerarFormatoPeajeEgresoAnterior(pathFile + fileName, EntidadRecalculoPotencia, EntidadPeajeEgreso, ListaPeajeEgresoDetalle, ListaEmpresas, ListaBarras);
            }

            return fileName;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="plazo">Si la información se ha remitido en plazo</param>
        /// <param name="liquidacion">Si el envió forma parte de la VTP</param>
        public List<VtpPeajeEgresoDTO> ListVtpPeajeEgresosConsulta(int pericodi, int recpotcodi, int emprcodi, string plazo, string liquidacion)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoRepository().ListConsulta(pericodi, recpotcodi, emprcodi, plazo, liquidacion);
        }

        /// <summary>
        /// Permite Obtener la última revisión del periodo
        /// </summary>
        /// <param name="pericodi">Código del periodo</param>
        /// <param name="emprcodi">Codigo de empresa</param>
        public VtpPeajeEgresoDTO GetPreviusPeriod(int pericodi, int emprcodi)
        {
            VtpPeajeEgresoDTO vtPeajeEgreso = null;
            try
            {
                vtPeajeEgreso = FactoryTransferencia.GetVtpPeajeEgresoRepository().GetPreviusPeriod(pericodi , emprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return vtPeajeEgreso;
        }

        public bool EnviarCorreoNotificacionEnvioExtranet(VtpPeajeEgresoDTO entity, String emprNom, String periodo, String revision, String email, String path, String pathLogo)
        {
            SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(COES.Servicios.Aplicacion.Eventos.Helper.ConstantesExtranetCTAF.PlantcodiNotificacionEnvioExtranet);
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (plantilla != null)
                    {
                        //string file = this.GenerarFormatoValidacionesEnvio(entity.Pericodi, entity.Recpotcodi, entity.Pegrcodi, emprNom, 1, path, pathLogo);
                        List<String> files = new List<String>();
                        //files.Add(file);
                        string contenido = string.Format(Resources.ValidacionesEnvio, emprNom, periodo, revision, entity.Pegrcodi, email, entity.Pegrfeccreacion);
                        string asunto = plantilla.Plantasunto + " - " + emprNom + " - " + periodo + " - " + revision;
                        string from = TipoPlantillaCorreo.MailFrom;
                        string to = email + ";" + plantilla.Planticorreos;
                        if (!string.IsNullOrEmpty(contenido))
                        {
                            this.servCorreo.EnviarCorreo(from, to, "", "", asunto, contenido, plantilla.Plantcodi, path, files);                        
                        }
                    }
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                    return false;
                }
            }
        }

        #endregion

        #region Métodos Tabla VTP_PEAJE_EGRESO_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_EGRESO
        /// </summary>
        public int SaveVtpPeajeEgresoDetalle(VtpPeajeEgresoDetalleDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public void UpdateVtpPeajeEgresoDetalle(VtpPeajeEgresoDetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public void DeleteVtpPeajeEgresoDetalle(int pegrcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().Delete(pegrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de un periodo / versión de la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        private void DeleteByCriteriaVtpPeajeEgresoDetalle(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public VtpPeajeEgresoDetalleDTO GetByIdVtpPeajeEgresoDetalle(int pegrdcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetById(pegrdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public List<VtpPeajeEgresoDetalleDTO> ListVtpPeajeEgresoDetalles()
        {
            return FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public List<VtpPeajeEgresoDetalleDTO> GetByCriteriaVtpPeajeEgresoDetalles(int pegrcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetByCriteria(pegrcodi);
        }
        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public List<VtpPeajeEgresoDetalleDTO> GetByCriteriaVtpPeajeEgresoDetallesNuevo(int pegrCodi, int recpotodi, int emprcodi, int periCodi)
        {

            bool esPrimeraCarga = true;
            int recpotodiInicial = 1;
            int periCodiProduccion = Convert.ToInt32(ConfigurationManager.AppSettings["idPeriodo"].ToString());

            List<VtpPeajeEgresoDetalleDTO> lCodigos = new List<VtpPeajeEgresoDetalleDTO>();
            List<VtpPeajeEgresoDetalleDTO> lstPeajeEgresoAuxiliar = new List<VtpPeajeEgresoDetalleDTO>();

            try
            {
                VtpPeajeEgresoDTO objPeajeEgreso = FactoryTransferencia.GetVtpPeajeEgresoRepository().GetByCriteria(emprcodi, periCodi, recpotodi) ?? new VtpPeajeEgresoDTO();
                if (pegrCodi != 0) { objPeajeEgreso.Pegrcodi = pegrCodi; }
                string estado = FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetById(periCodi, recpotodi).Recpotestado?.ToUpper();
                if (estado == "CERRADO")
                {
                    lCodigos = FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetByCriteria(objPeajeEgreso.Pegrcodi);
                }
                else
                {
                    //if (periCodiProduccion == periCodi && recpotodi == recpotodiInicial && pegrCodi == 0) //Aqui obtiene las potencias de mi carga inicial con los datos
                    //lstPeajeEgresoAuxiliar = new LeerRepositorio().ListaCodigosPotencias();
                    //else
                    if (periCodiProduccion <= periCodi && recpotodi > recpotodiInicial && pegrCodi == 0) //Aqui debe obtener las potencias del periodo anterior por empresa solo para revisiones
                        lstPeajeEgresoAuxiliar = FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetByCriteriaPeriodoAnterior(emprcodi, periCodi);
                    else
                    {
                        esPrimeraCarga = false;
                        lstPeajeEgresoAuxiliar = FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetByCriteria((int)objPeajeEgreso?.Pegrcodi);
                    }
                    lCodigos = FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().ListarCodigosByEmprcodi(emprcodi, periCodi).Distinct().ToList();
                }

                // else if (periCodiProduccion > pegrCodi && recpotodi > recpotodiInicial && pegrCodi == 0) //Aqui debe obtener las potencias  actual que se registrar el recalculo
                //     lstPeajeEgresoAuxiliar = null;
                //else


                foreach (var item in lCodigos)
                {
                    VtpPeajeEgresoDetalleDTO findCodigoVtp = lstPeajeEgresoAuxiliar.Find(x => (x.Codcncodivtp ?? x.Coregecodvtp) == item.Coregecodvtp);
                    if (findCodigoVtp != null)
                    {

                        item.Pegrdpotecoincidente = findCodigoVtp.Pegrdpotecoincidente;
                        item.Pegrdpoteegreso = findCodigoVtp.Pegrdpoteegreso;
                        item.Pegrdpreciopote = findCodigoVtp.Pegrdpreciopote;
                        item.Pegrdpotedeclarada = findCodigoVtp.Pegrdpotedeclarada;
                        item.Pegrdpeajeunitario = findCodigoVtp.Pegrdpeajeunitario;
                        item.Pegrdfacperdida = findCodigoVtp.Pegrdfacperdida;
                        item.Pegrdcalidad = findCodigoVtp.Pegrdcalidad;

                        if (!esPrimeraCarga)
                        {
                            item.TipConNombre = findCodigoVtp.TipConNombre ?? item.TipConNombre;
                            item.TipConCondi = findCodigoVtp.TipConCondi > 0 ? findCodigoVtp.TipConCondi : item.TipConCondi;
                        }
                    }
                }

                //foreach (var item in lCodigosDistinct)
                //{
                //    var codigos = lCodigos.Where(x => x.Emprcodi == item.Emprcodi &&
                //                                  x.Barrcodi == item.Barrcodi &&
                //                                  x.Pegrdtipousuario == item.Pegrdtipousuario).ToList();

                //    var egresos = lstPeajeEgreso.Where(x => x.Emprcodi == item.Emprcodi &&
                //                                    x.Barrcodi == item.Barrcodi &&
                //                                    x.Pegrdtipousuario.ToUpper().Trim() == item.Pegrdtipousuario.ToUpper().Trim()).ToList();

                //    if (codigos.Count >= egresos.Count)
                //    {
                //        for (int i = 0; i < egresos.Count; i++)
                //        {
                //            codigos[i].Emprnomb = egresos[i].Emprnomb;
                //            codigos[i].Barrnombre = egresos[i].Barrnombre;
                //            codigos[i].Pegrdtipousuario = egresos[i].Pegrdtipousuario;

                //            codigos[i].Pegrdpotecoincidente = egresos[i].Pegrdpotecoincidente;
                //            codigos[i].Pegrdpoteegreso = egresos[i].Pegrdpoteegreso;
                //            codigos[i].Pegrdpreciopote = egresos[i].Pegrdpreciopote;
                //            codigos[i].Pegrdpotedeclarada = egresos[i].Pegrdpotedeclarada;
                //            codigos[i].Pegrdpeajeunitario = egresos[i].Pegrdpeajeunitario;

                //            codigos[i].Pegrdfacperdida = egresos[i].Pegrdfacperdida;
                //            codigos[i].Pegrdcalidad = egresos[i].Pegrdcalidad;
                //        }
                //    }
                //    else
                //    {
                //        for (int i = 0; i < egresos.Count; i++)
                //        {
                //            if (codigos.Count > i)
                //            {
                //                codigos[i].Emprnomb = egresos[i].Emprnomb;
                //                codigos[i].Barrnombre = egresos[i].Barrnombre;
                //                codigos[i].Pegrdtipousuario = egresos[i].Pegrdtipousuario;

                //                codigos[i].Pegrdpotecoincidente = egresos[i].Pegrdpotecoincidente;
                //                codigos[i].Pegrdpoteegreso = egresos[i].Pegrdpoteegreso;
                //                codigos[i].Pegrdpreciopote = egresos[i].Pegrdpreciopote;
                //                codigos[i].Pegrdpotedeclarada = egresos[i].Pegrdpotedeclarada;
                //                codigos[i].Pegrdpeajeunitario = egresos[i].Pegrdpeajeunitario;

                //                codigos[i].Pegrdfacperdida = egresos[i].Pegrdfacperdida;
                //                codigos[i].Pegrdcalidad = egresos[i].Pegrdcalidad;
                //            }
                //            else
                //            {
                //                //lCodigos.Add(egresos[i]);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return lCodigos;
        }


        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public List<VtpPeajeEgresoDetalleDTO> ListVtpPeajeEgresoDetallesView(int pegrcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().ListView(pegrcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EGRESO_DETALLE
        /// </summary>
        public VtpPeajeEgresoDetalleDTO GetByIdVtpPeajeEgresoMinfo(int pegrcodi, int emprcodi, int barrcodi, string pegrdtipousuario)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetByIdMinfo(pegrcodi, emprcodi, barrcodi, pegrdtipousuario);
        }

        /// <summary>
        /// Permite Obtener el detalle por id del peaje egreso y codigo vtp
        /// </summary>
        /// <param name="periCodi">Código del periodo</param>
        /// /// <param name="coregeCodVtp">Código vtp</param>
        public VtpPeajeEgresoDetalleDTO GetByPegrCodiAndCodVtp(int periCodi, string coregeCodVtp)
        {
            VtpPeajeEgresoDetalleDTO vtPeajeEgresoDetalle = null;
            try
            {
                vtPeajeEgresoDetalle = FactoryTransferencia.GetVtpPeajeEgresoDetalleRepository().GetByPegrCodiAndCodVtp(periCodi, coregeCodVtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return vtPeajeEgresoDetalle;
        }

        #endregion

        #region Métodos Tabla VTP_PEAJE_EGRESO_MINFO
        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_EGRESO_MINFO
        /// </summary>
        public void SaveVtpPeajeEgresoMinfo(VtpPeajeEgresoMinfoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_EGRESO_MINFO
        /// </summary>
        public void UpdateVtpPeajeEgresoMinfo(VtpPeajeEgresoMinfoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EGRESO_MINFO
        /// </summary>
        public void DeleteVtpPeajeEgresoMinfo(int pegrmicodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().Delete(pegrmicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla VTP_PEAJE_EGRESO_MINFO, de un mes de valorización y una versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public void DeleteByCriteriaVtpPeajeEgresoMinfo(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EGRESO_MINFO
        /// </summary>
        public VtpPeajeEgresoMinfoDTO GetByIdVtpPeajeEgresoMinfo(int pegrmicodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().GetById(pegrmicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EGRESO_MINFO
        /// </summary>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfo(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().List(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite traer la lista de empresas en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoCabecera(int pericodi, int recpotcodi, int recacodi)
        {
            //- Ajuste temportal para la revisión 04 de diciembre
            List<VtpPeajeEgresoMinfoDTO> result = new List<VtpPeajeEgresoMinfoDTO>();
            List<VtpPeajeEgresoMinfoDTO> list = FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListCabecera(pericodi, recpotcodi, recacodi);

            if (pericodi >= 47)
            {
                /*
                 --10481 - AGUAS Y ENERGIA PERU
                 --13 - LUZ DEL SUR
                 --10913 - SDE PIURA 
                select distinct a.emprcodipeaje as genemprcodi, trim(b.emprnomb) as genemprnombre
                from vtp_peaje_empresa_pago a
                inner join si_empresa b on a.emprcodipeaje = b.emprcodi and b.emprestado = 'A'
                where a.pempagpericodidest=:pericodi and b.emprcodi in (10481, 13, 10913)
                order by 2
                 */
                result = list.Where(x => x.Genemprcodi != 10481 && x.Genemprcodi != 13 && x.Genemprcodi != 10913).ToList();
            }
            else
            {
                result = list;
            }

            return result;
        }

        /// <summary>
        /// Permite traer todos los registros de una empresa en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoEmpresa(int pericodi, int recpotcodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListEmpresa(pericodi, recpotcodi, emprcodi);
        }

        /// <summary>
        /// Permite traer todos los registros de una empresa en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="periCodi">Código del Mes de valorización</param>
        /// <param name="recPotCodi">Código del Recálculo de Potencia</param>
        /// <param name="emprcodi">Código de empresa</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoEmpresaNuevo(int periCodi, int recPotCodi, int emprcodi)
        {
            List<CodigoConsolidadoDTO> lCodigos = new List<CodigoConsolidadoDTO>();
            List<VtpPeajeEgresoMinfoDTO> lPeajeEgreso = new List<VtpPeajeEgresoMinfoDTO>();

            lCodigos = FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListarCodigosVTP(emprcodi);
            lPeajeEgreso = FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListEmpresa(periCodi, recPotCodi, emprcodi);

            var lCodigosDistinct = lCodigos.Select(x => new { x.Clicodi, x.Barrcodi, x.Tipusunombre }).Distinct().ToList();

            try
            {
                foreach (var item in lCodigosDistinct)
                {
                    var codigos = lCodigos.Where(x => x.Clicodi == item.Clicodi &&
                                                  x.Barrcodi == item.Barrcodi &&
                                                  x.Tipusunombre == item.Tipusunombre).ToList();

                    var egresos = lPeajeEgreso.Where(x => x.Cliemprcodi == item.Clicodi &&
                                                    x.Barrcodi == item.Barrcodi &&
                                                    x.Pegrmitipousuario.ToUpper().Trim() == item.Tipusunombre.ToUpper().Trim()).ToList();

                    if (egresos.Count > 0)
                    {
                        if (egresos.Count >= codigos.Count)
                        {
                            for (int i = 0; i < codigos.Count; i++)
                            {
                                egresos[i].Coregecodvtp = codigos[i].Codcncodivtp;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < egresos.Count; i++)
                            {
                                egresos[i].Coregecodvtp = codigos[i].Codcncodivtp;
                            }
                        }
                    }

                }


            }
            catch (Exception e)
            {
                return lPeajeEgreso;
            }

            return lPeajeEgreso;
        }



        /// <summary>
        /// Permite traer todos los registros de una empresa en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoEmpresaRecalculo(int pericodi, int recpotcodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListEmpresaRecalculo(pericodi, recpotcodi, emprcodi);
        }

        /// <summary>
        /// Permite traer una lista de empresas con su valores de Potencia y Valorización en el mes de valorización y versión de recalculo: 
        /// Resultado Lista: EmprCodi, EmprNomb, Potencia Consumida, Valorización de consumos de la vista VW_VTP_PEAJE_EGRESO y VTP_RETIRO_POTESC
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoPotenciaValor(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListPotenciaValor(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite traer una lista de suma de valor de potencia efectiva, firme y remunerable por empresa
        /// </summary>
        /// <param name="ipefrcodis">Codigo's de ingreso de potencia efectiva ,firme y firme remunerable detalle .</param>
        /// <param name="periCodi">Codigo de periodo.</param>
        /// <param name="recpotcodi">Codigo de recalculo</param>
        /// <returns></returns>
        public List<VtpIngresoPotefrDetalleDTO> ObtenerPotenciaEFRSumPorEmpresa(string ipefrcodis, int periCodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpIngresoPotefrDetalleRepository().ObtenerPotenciaEFRSumPorEmpresa(ipefrcodis, periCodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar una búsquedas en la tabla VTP_PEAJE_EGRESO_MINFO para un mes de valorización y una versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVtpPeajeEgresoMinfo(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().GetByCriteria(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar una búsquedas en la vista VW_VTP_PEAJE_EGRESO para un mes de valorización y una versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVtpPeajeEgresoMinfoVista(int pericodi, int recpotcodi, int emprcodi, int cliemprcodi, int barrcodi, int barrcodifco, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, string pegrmicalidad2)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().GetByCriteriaVista(pericodi, recpotcodi, emprcodi, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);
        }

        /// <summary>
        /// Permite realizar una búsquedas en la vista VW_VTP_PEAJE_EGRESO para un mes de valorización y una versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVtpPeajeEgresoMinfoVistaNuevo(int pericodi, int recpotcodi, int emprcodi, int cliemprcodi, int barrcodi, int barrcodifco, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, string pegrmicalidad2)
        {
            List<VtpPeajeEgresoMinfoDTO> lPeajeEgreso = new List<VtpPeajeEgresoMinfoDTO>();
            List<VtpPeajeEgresoMinfoDTO> lCodigos = new List<VtpPeajeEgresoMinfoDTO>();

            try
            {
                lPeajeEgreso = FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().GetByCriteriaVistaNuevo(pericodi, recpotcodi, emprcodi, cliemprcodi, barrcodi, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);
                lCodigos = FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListarCodigosByCriteria(emprcodi, cliemprcodi, barrcodi, pegrmitipousuario, pericodi, pegrmilicitacion);

                var lCodigosDistinct = lCodigos.Select(x => new { x.Coregecodvtp }).Distinct().ToList();

                if (lPeajeEgreso.Count > 0)
                {
                    foreach (var item in lCodigosDistinct)
                    {
                        var codigos = lCodigos.Where(x => x.Coregecodvtp == item.Coregecodvtp).ToList();
                        var egresos = lPeajeEgreso.Where(x => x.Coregecodvtp == item.Coregecodvtp).ToList();

                        if (codigos.Count >= egresos.Count)
                        {
                            for (int i = 0; i < egresos.Count; i++)
                            {
                                codigos[i].Genemprnombre = egresos[i].Genemprnombre;
                                codigos[i].Cliemprnombre = egresos[i].Cliemprnombre;
                                codigos[i].Barrnombre = egresos[i].Barrnombre;

                                codigos[i].Tipconnombre = egresos[i].Tipconnombre;
                                codigos[i].Pegrmitipousuario = egresos[i].Pegrmitipousuario;
                                codigos[i].Pegrdpotecoincidente = egresos[i].Pegrdpotecoincidente;
                                codigos[i].Pegrmipoteegreso = egresos[i].Pegrmipoteegreso;
                                codigos[i].Pegrmipreciopote = egresos[i].Pegrmipreciopote;
                                codigos[i].Pegrmipotedeclarada = egresos[i].Pegrmipotedeclarada;
                                codigos[i].Pegrmipeajeunitario = egresos[i].Pegrmipeajeunitario;

                                codigos[i].Pegrmifacperdida = egresos[i].Pegrmifacperdida;
                                codigos[i].Pegrmicalidad = egresos[i].Pegrmicalidad;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < egresos.Count; i++)
                            {
                                if (codigos.Count > i)
                                {
                                    codigos[i].Genemprnombre = egresos[i].Genemprnombre;
                                    codigos[i].Cliemprnombre = egresos[i].Cliemprnombre;
                                    codigos[i].Barrnombre = egresos[i].Barrnombre;


                                    codigos[i].Tipconnombre = egresos[i].Tipconnombre;
                                    codigos[i].Pegrmitipousuario = egresos[i].Pegrmitipousuario;

                                    codigos[i].Pegrdpotecoincidente = egresos[i].Pegrdpotecoincidente;
                                    codigos[i].Pegrmipoteegreso = egresos[i].Pegrmipoteegreso;
                                    codigos[i].Pegrmipreciopote = egresos[i].Pegrmipreciopote;
                                    codigos[i].Pegrmipotedeclarada = egresos[i].Pegrmipotedeclarada;
                                    codigos[i].Pegrmipeajeunitario = egresos[i].Pegrmipeajeunitario;

                                    codigos[i].Pegrmifacperdida = egresos[i].Pegrmifacperdida;
                                    codigos[i].Pegrmicalidad = egresos[i].Pegrmicalidad;
                                }
                                else
                                {
                                    //lCodigos.Add(egresos[i]);
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(pegrmicalidad) && pegrmicalidad != "*" && pegrmicalidad2 == "*")
                    {
                        lCodigos = lCodigos.Where(val => val.Pegrmicalidad != null).ToList();
                        lCodigos = lCodigos.Where(val => val.Pegrmicalidad.Trim().ToUpper().Equals(pegrmicalidad.Trim().ToUpper())).ToList();
                    }
                    if (!String.IsNullOrEmpty(pegrmicalidad) && !String.IsNullOrEmpty(pegrmicalidad2) && pegrmicalidad2 != "*")
                    {
                        lCodigos = lCodigos.Where(val => val.Pegrmicalidad != null).ToList();
                        lCodigos = lCodigos.Where(val => val.Pegrmicalidad.Trim().ToUpper().Equals(pegrmicalidad.Trim().ToUpper()) || val.Pegrmicalidad.Trim().ToUpper().Equals(pegrmicalidad2.Trim().ToUpper())).ToList();
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return lCodigos;
        }

        /// <summary>
        /// Permite realizar una búsquedas en la vista VW_VTP_PEAJE_EGRESO de todos los registros faltantes comparados con el periodo anterior
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVtpPeajeEgresoMinfoFaltante(int pericodi, int recpotcodi, int pericodianterior, int recpotcodianterior)
        {
            return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().GetByCriteriaInfoFaltante(pericodi, recpotcodi, pericodianterior, recpotcodianterior);
        }

        /// <summary>
        /// Permite generar el archivo de exportación Información "ingresada para VTP y peajes" de la en la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="emprcodi">Código de la empresa</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoPeajeEgresoMinfo(int pericodi, int recpotcodi, int emprcodi, int formato, string pathFile, string pathLogo, int cliemprcodi, int barrcodi,
            int barrcodifco, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            PeriodoDTO EntidadPeriodo = new PeriodoAppServicio().GetByIdPeriodo(pericodi);

            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo;
            if (emprcodi == -10)
                ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfo(pericodi, recpotcodi);
            else
            {
                string pegrmicalidad2 = "*";
                if (pegrmitipousuario.Equals("")) pegrmitipousuario = "*";
                if (pegrmilicitacion.Equals("")) pegrmilicitacion = "*";
                if (pegrmicalidad.Equals(""))
                { pegrmicalidad = "*"; }
                else if (pegrmicalidad.IndexOf("/") > 0)
                {
                    string[] aCalidad = pegrmicalidad.Split('/');
                    pegrmicalidad = aCalidad[0];
                    pegrmicalidad2 = aCalidad[1];
                }
                ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfoVista(pericodi, recpotcodi, emprcodi, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);
            }

            #region Modificado para egejunin

            SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);

            foreach (VtpPeajeEgresoMinfoDTO item in ListaPeajeEgresoMinfo)
            {
                if (item.Genemprcodi == 10582)
                {
                    item.Genemprnombre = empresa.Emprnomb;
                }
            }

            #endregion

            if (formato == 1)
            {
                fileName = "ReportePeajeEgresoMejorInformacion.xlsx";
                if (EntidadPeriodo.PeriFormNuevo == 1)
                    ExcelDocument.GenerarFormatoPeajeEgresoMinfoFormatoNuevo(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgresoMinfo, out hoja);
                else
                    ExcelDocument.GenerarFormatoPeajeEgresoMinfo(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgresoMinfo, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReportePeajeEgresoMejorInformacion.pdf";
                PdfDocument.GenerarFormatoPeajeEgresoMinfo(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgresoMinfo, pathLogo);
            }

            return fileName;
        }

        /// <summary>
        /// Permite generar el archivo de exportación Información "ingresada para VTP y peajes" de la en la vista VW_VTP_PEAJE_EGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="emprcodi">Código de la empresa</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoPeajeEgresoMinfoNuevo(int pericodi, int recpotcodi, int emprcodi, int formato, string pathFile, string pathLogo, int cliemprcodi, int barrcodi,
            int barrcodifco, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo;
            if (emprcodi == -10)
                ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfo(pericodi, recpotcodi);
            else
            {
                string pegrmicalidad2 = "*";
                if (pegrmitipousuario.Equals("")) pegrmitipousuario = "*";
                if (pegrmilicitacion.Equals("")) pegrmilicitacion = "*";
                if (pegrmicalidad.Equals(""))
                { pegrmicalidad = "*"; }
                else if (pegrmicalidad.IndexOf("/") > 0)
                {
                    string[] aCalidad = pegrmicalidad.Split('/');
                    pegrmicalidad = aCalidad[0];
                    pegrmicalidad2 = aCalidad[1];
                }
                ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfoVistaNuevo(pericodi, recpotcodi, emprcodi, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);
            }

            #region Modificado para egejunin

            SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);

            foreach (VtpPeajeEgresoMinfoDTO item in ListaPeajeEgresoMinfo)
            {
                if (item.Genemprcodi == 10582)
                {
                    item.Genemprnombre = empresa.Emprnomb;
                }
            }

            #endregion

            if (formato == 1)
            {
                fileName = "ReportePeajeEgresoMejorInformacion.xlsx";
                ExcelDocument.GenerarFormatoPeajeEgresoMinfoNuevo(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgresoMinfo, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReportePeajeEgresoMejorInformacion.pdf";
                PdfDocument.GenerarFormatoPeajeEgresoMinfoNuevo(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgresoMinfo, pathLogo);
            }

            return fileName;
        }


        /// <summary>
        /// Permite generar el Reporte de Resumen de información VTP - CU21
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReportePotenciaValor(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            string fileName = string.Empty;
            hoja = null;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso = this.ListVtpPeajeEgresoMinfoPotenciaValor(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReportePotenciaValor.xlsx";
                ExcelDocument.GenerarReportePotenciaValor(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgreso, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReportePotenciaValor.pdf";
                PdfDocument.GenerarReportePotenciaValor(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEgreso, pathLogo);
            }

            return fileName;
        }


        /// <summary>
        /// Permite traer la lista de empresas en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO para copiar en una nueva versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoCabeceraRecalculo(int pericodi, int recpotcodi)
        {
            //- Ajuste temportal para la revisión 04 de diciembre
            List<VtpPeajeEgresoMinfoDTO> result = new List<VtpPeajeEgresoMinfoDTO>();
            List<VtpPeajeEgresoMinfoDTO> list = FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListCabeceraRecalculo(pericodi, recpotcodi);

            if (pericodi == 47 && recpotcodi == 5)
            {
                result = list.Where(x => x.Genemprcodi != 10481 && x.Genemprcodi != 13 && x.Genemprcodi != 10913).ToList();
            }
            else
            {
                result = list;
            }

            return result;
        }

        ///// <summary>
        ///// Permite traer todos los registros de una empresa en el mes de valorización y versión de recalculo: EmprCodi y EmprNomb de la vista VW_VTP_PEAJE_EGRESO
        ///// </summary>
        ///// <param name="pericodi">Código del Mes de valorización</param>
        ///// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        ///// <returns>Lista de VtpPeajeEgresoMinfoDTO</returns>
        //public List<VtpPeajeEgresoMinfoDTO> ListVtpPeajeEgresoMinfoEmpresa(int pericodi, int recpotcodi, int emprcodi)
        //{
        //    return FactoryTransferencia.GetVtpPeajeEgresoMinfoRepository().ListEmpresa(pericodi, recpotcodi, emprcodi);
        //}
        #endregion

        #region Métodos tabla VTP_PEAJE_EMPRESA

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_EMPRESA
        /// </summary>
        public void SaveVtpPeajeEmpresa(VtpPeajeEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_EMPRESA
        /// </summary>
        public void UpdateVtpPeajeEmpresa(VtpPeajeEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EMPRESA
        /// </summary>
        public void DeleteVtpPeajeEmpresa(int pempcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaRepository().Delete(pempcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_PEAJE_EMPRESA por mes de valorización y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpPeajeEmpresa(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EMPRESA
        /// </summary>
        public VtpPeajeEmpresaDTO GetByIdVtpPeajeEmpresa(int pempcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaRepository().GetById(pempcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EMPRESA
        /// </summary>
        public List<VtpPeajeEmpresaDTO> ListVtpPeajeEmpresa()
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EMPRESA
        /// </summary>
        public List<VtpPeajeEmpresaDTO> GetByCriteriaVtpPeajeEmpresa()
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla VTP_PEAJE_EMPRESA_AJUSTE

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        public int SaveVtpPeajeEmpresaAjuste(VtpPeajeEmpresaAjusteDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        public void UpdateVtpPeajeEmpresaAjuste(VtpPeajeEmpresaAjusteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        public void DeleteVtpPeajeEmpresaAjuste(int pecajcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().Delete(pecajcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_PEAJE_EMPRESA_AJUSTE asociados a un mes de valorización
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        public void DeleteByCriteriaVtpPeajeEmpresaAjuste(int pericodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().DeleteByCriteria(pericodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        public VtpPeajeEmpresaAjusteDTO GetByIdVtpPeajeEmpresaAjuste(int pecajcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().GetById(pecajcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        public List<VtpPeajeEmpresaAjusteDTO> ListVtpPeajeEmpresaAjuste()
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        public List<VtpPeajeEmpresaAjusteDTO> GetByCriteriaVtpPeajeEmpresaAjuste(int pericodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().GetByCriteria(pericodi);
        }

        /// <summary>
        /// Permite obtener el saldo a aplicar en este periodo de la tabla VTP_PEAJE_EMPRESA_AJUSTE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización donde se aplica el ajuste</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <param name="pingcodi">Código del peaje ingreso</param>
        /// <returns>Ajuste del mes</returns>
        public decimal GetPeajeEmpresaAjuste(int pericodi, int emprcodipeaje, int pingcodi, int emprcodicargo)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaAjusteRepository().GetAjuste(pericodi, emprcodipeaje, pingcodi, emprcodicargo);
        }
        #endregion

        #region Métodos tabla VTP_PEAJE_EMPRESA_PAGO

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public int SaveVtpPeajeEmpresaPago(VtpPeajeEmpresaPagoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public void UpdateVtpPeajeEmpresaPago(VtpPeajeEmpresaPagoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los registro de la tabla VTP_PEAJE_EMPRESA_PAGO, para un pericodi y recpotcodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="potsepericodidest">Código del Mes de valorización sonde se aplicara el saldo por cargo</param>
        public void UpdateVtpPeajeEmpresaPagoPeriodoDestino(int pericodi, int recpotcodi, int pempagpericodidest)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().UpdatePeriodoDestino(pericodi, recpotcodi, pempagpericodidest);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public void DeleteVtpPeajeEmpresaPago(int pempagcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().Delete(pempagcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpPeajeEmpresaPago(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public VtpPeajeEmpresaPagoDTO GetByIdVtpPeajeEmpresaPago(int pempagcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetById(pempagcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPago()
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().List();
        }

        /// <summary>
        /// Permite listar todas las empresas (Emprcodipeaje y Emprnombpeaje) de la tabla VTP_PEAJE_EMPRESA_PAGO que han realizado un pago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPagoPeajePago(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().ListPeajePago(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros (Pago=SI/Transmision=SI) de la tabla VTP_PEAJE_EMPRESA_PAGO de las EmprnombPeaje, EmprnombCargo y pempagpeajepago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPagoPeajeCobro(int emprcodipago, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().ListPeajeCobro(emprcodipago, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros (Pago=SI/Transmision=SI) de la tabla VTP_PEAJE_EMPRESA_PAGO de las EmprnombPeaje, EmprnombCargo y pempagpeajepago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPagoPeajeCobroHistoricos(int emprcodipago, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().ListPeajeCobroHistoricos(emprcodipago, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros (Pago=SI/Transmision=SI) de la tabla VTP_PEAJE_EMPRESA_PAGO de las EmprnombPeaje, EmprnombCargo y pempagpeajepago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPagoPeajeCobroSelect(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().ListPeajeCobroSelect(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros (Pago=SI/Transmision=SI) de la tabla VTP_PEAJE_EMPRESA_PAGO de las EmprnombPeaje, EmprnombCargo y pempagpeajepago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListGetByEmpresaGeneradora(int pericodi, int recpotcodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().GetByEmpresaGeneradora(pericodi, recpotcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros (Pago=SI/Transmision=NO) de la tabla VTP_PEAJE_EMPRESA_PAGO de las EmprnombPeaje, EmprnombCargo y pempagpeajepago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPagoPeajeCobroNoTransm(int emprcodipago, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().ListPeajeCobroNoTransm(emprcodipago, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de un reparto (Pago=SI/Transmision=NO) de la tabla VTP_PEAJE_EMPRESA_PAGO de las EmprnombPeaje, EmprnombCargo y pempagpeajepago
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeEmpresaPagoDTO</returns>
        public List<VtpPeajeEmpresaPagoDTO> ListVtpPeajeEmpresaPagoPeajeCobroReparto(int rrpecodi, int emprcodipago, int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().ListPeajeCobroReparto(rrpecodi, emprcodipago, pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        public List<VtpPeajeEmpresaPagoDTO> GetByCriteriaVtpPeajeEmpresaPago()
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener el saldo anterior para un periodo y empresa de la tabla VTP_PEAJE_EMPRESA_PAGO
        /// </summary>
        /// <param name="pempagpericodidest">Código del Mes de valorización de saldos</param>
        /// <param name="pingcodi">Código del cargo</param>
        /// <param name="emprcodipeaje">Código del empresa</param>
        /// <returns>El saldo de la empresa</returns>
        public decimal GetPeajeEmpresaPagoSaldoAnterior(int pempagpericodidest, int pingcodi, int emprcodipeaje, int emprcodicargo)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetSaldoAnterior(pempagpericodidest, pingcodi, emprcodipeaje, emprcodicargo);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EMPRESA_PAGO consultado por pericodi, recpotcodi, pingcodi, emprcodipeaje. 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pingcodi">Código del cargo</param>
        /// <param name="emprcodipeaje">Código del empresa</param>
        public List<VtpPeajeEmpresaPagoDTO> GetByIdVtpPeajeEmpresaPagoSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodipeaje, int emprcodicargo)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetByIdSaldo(pericodi, recpotcodi, pingcodi, emprcodipeaje, emprcodicargo);
        }

        /// <summary>
        /// Permite generar el Reporte de Compensación a transmisoras por peaje de conexión y transmisión - CU18
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReportePeajePagarse(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago = this.ListVtpPeajeEmpresaPagoPeajePago(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReportePeajePagarse.xlsx";
                List<double> totales = new List<double>();
                List<double> mensules = new List<double>();
                List<double> saldos = new List<double>();
                ExcelDocument.GenerarReportePeajePagarse(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEmpresaPago, out hoja, out totales, out mensules, out saldos);
            }
            if (formato == 2)
            {
                fileName = "ReportePeajePagarse.pdf";
                PdfDocument.GenerarReportePeajePagarse(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeEmpresaPago, pathLogo);
            }

            return fileName;
        }

        //CU21
        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_EMPRESA_PAGO consultado por pericodi, recpotcodi y el nombre del cargo prima. 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pingnombre">Nombre del CargoPrima</param>
        public VtpPeajeEmpresaPagoDTO GetByIdVtpPeajeEmpresaPagoByCargoPrima(int pericodi, int recpotcodi, string pingnombre)
        {
            return FactoryTransferencia.GetVtpPeajeEmpresaPagoRepository().GetByCargoPrima(pericodi, recpotcodi, pingnombre);
        }
        #endregion

        #region Métodos Tabla VTP_PEAJE_INGRESO

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        public void SaveVtpPeajeIngreso(VtpPeajeIngresoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeIngresoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        public void UpdateVtpPeajeIngreso(VtpPeajeIngresoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeIngresoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        public void UpdateVtpPeajeIngresoDesarrollo(VtpPeajeIngresoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeIngresoRepository().UpdateDesarrollo(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteVtpPeajeIngreso(int pericodi, int recpotcodi, int pingcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeIngresoRepository().Delete(pericodi, recpotcodi, pingcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla VTP_PEAJE_INGRESO dado un periodo y recalculo potencia
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpPeajeIngreso(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeIngresoRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        public VtpPeajeIngresoDTO GetByIdVtpPeajeIngreso(int pericodi, int recpotcodi, int pingcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().GetById(pericodi, recpotcodi, pingcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngresos()
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpPeajeIngreso
        /// </summary>
        public List<VtpPeajeIngresoDTO> GetByCriteriaVtpPeajeIngresos()
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_INGRESO, mas EmprNombre y RepartoPeajeNombre
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngresoView(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().ListView(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_INGRESO, mas EmprNombre y RepartoPeajeNombre donde Pago=SI
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngresoPagoSi(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().ListPagoSi(pericodi, recpotcodi);
        }


        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_INGRESO, mas EmprNombre y RepartoPeajeNombre donde Transmision=SI
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngresoTransmisionSi(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().ListTransmisionSi(pericodi, recpotcodi);
        }


        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_INGRESO, mas EmprNombre y RepartoPeajeNombre donde Transmision=SI se convierte en un solo registro
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngresoCargo(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().ListCargo(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        /// <param name="pingcodi">Código del Registro</param>
        /// <returns>VtpPeajeIngresoDTO</returns>
        public VtpPeajeIngresoDTO GetByIdVtpPeajeIngresoView(int pericodi, int recpotcodi, int pingcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().GetByIdView(pericodi, recpotcodi, pingcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="pingnombre">Nombre del Ingreso Tarifario</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public VtpPeajeIngresoDTO GetByNomIngTarVtpPeajeIngreso(int pericodi, int recpotcodi, string pingnombre)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().GetByNombreIngresoTarifario(pericodi, recpotcodi, pingnombre);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_INGRESO, donde PINGTARIMENSUAL != 0
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListVtpPeajeIngresoTarifarioMensual(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().ListIngresoTarifarioMensual(pericodi, recpotcodi);
        }

        #region PrimasRER.2023
        /// <summary>
        /// Permite listar los CargoPrimaRer segun su emprcodi
        /// </summary>
        /// <param name="emprcodi">Código de Empresa</param>
        /// <returns>Lista de VtpPeajeIngresoDTO</returns>
        public List<VtpPeajeIngresoDTO> ListCargoPrimaRER(int emprcodi)
        {
            return FactoryTransferencia.GetVtpPeajeIngresoRepository().ListCargoPrimaRER(emprcodi);
        }
        #endregion

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VTP_PEAJE_INGRESO
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoPeajeIngreso(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.ListVtpPeajeIngresoView(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReportePeajeIngreso.xlsx";
                ExcelDocument.GenerarFormatoPeajeIngreso(pathFile + fileName, EntidadRecalculoPotencia, ListaPeajeIngreso);
            }

            return fileName;
        }
        #endregion

        #region Métodos tabla VTP_PEAJE_SALDO_TRANSMISION

        /// <summary>
        /// Inserta un registro de la tabla VTP_PEAJE_SALDO_TRANSMISION
        /// </summary>
        public void SaveVtpPeajeSaldoTransmision(VtpPeajeSaldoTransmisionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_PEAJE_SALDO_TRANSMISION
        /// </summary>
        public void UpdateVtpPeajeSaldoTransmision(VtpPeajeSaldoTransmisionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_PEAJE_SALDO_TRANSMISION
        /// </summary>
        public void DeleteVtpPeajeSaldoTransmision(int pstrnscodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().Delete(pstrnscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_PEAJE_SALDO_TRANSMISION por mes de valorización y versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteByCriteriaVtpPeajeSaldoTransmision(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_SALDO_TRANSMISION
        /// </summary>
        public VtpPeajeSaldoTransmisionDTO GetByIdVtpPeajeSaldoTransmision(int pstrnscodi)
        {
            return FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().GetById(pstrnscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_PEAJE_SALDO_TRANSMISION
        /// </summary>
        public List<VtpPeajeSaldoTransmisionDTO> ListVtpPeajeSaldoTransmision()
        {
            return FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().List();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_PEAJE_SALDO_TRANSMISION, por mes de valorización, recalculo y empresa
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public VtpPeajeSaldoTransmisionDTO GetByIdVtpPeajeSaldoTransmisionEmpresa(int pericodi, int recpotcodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().GetByIdEmpresa(pericodi, recpotcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todas las empresas para el Reporte de Egresos
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpPeajeSaldoTransmisionDTO> ListVtpPeajeSaldoTransmisionEmpresaEgreso(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().ListEmpresaEgreso(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar los calculos para extraer los registros a insertar en la tabla VTP_PEAJE_SALDO_TRANSMISION (A3: “Saldo por Peaje Transmisión” de una empresa Generadora)
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public List<VtpPeajeSaldoTransmisionDTO> GetByCriteriaVtpPeajeSaldoTransmision(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpPeajeSaldoTransmisionRepository().GetByCriteria(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite generar el Reporte de Egresos por compra de potencia - CU22
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteEgresos(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            string fileName = string.Empty;
            hoja = null;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPagoEgresoDTO> ListaPagoEgreso = this.ListVtpPagoEgreso(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReporteEgreso.xlsx";
                ExcelDocument.GenerarReporteEgresos(pathFile + fileName, EntidadRecalculoPotencia, ListaPagoEgreso, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReporteEgreso.pdf";
                PdfDocument.GenerarReporteEgresos(pathFile + fileName, EntidadRecalculoPotencia, ListaPagoEgreso, pathLogo);
            }

            return fileName;
        }
        #endregion

        #region Métodos Tabla VTP_RECALCULO_POTENCIA

        /// <summary>
        /// Inserta un registro de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        public int SaveVtpRecalculoPotencia(VtpRecalculoPotenciaDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpRecalculoPotenciaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        public void UpdateVtpRecalculoPotencia(VtpRecalculoPotenciaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpRecalculoPotenciaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        public void DeleteVtpRecalculoPotencia(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRecalculoPotenciaRepository().Delete(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>VtpRecalculoPotenciaDTO</returns>
        public VtpRecalculoPotenciaDTO GetByIdVtpRecalculoPotencia(int pericodi, int recpotcodi)
        {
            try
            {
                return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetById(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<VtpRecalculoPotenciaDTO> ListVtpRecalculoPotencias()
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpRecalculoPotencia
        /// </summary>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<VtpRecalculoPotenciaDTO> GetByCriteriaVtpRecalculoPotencias()
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_RECALCULO_POTENCIA, mas Perinombre
        /// </summary>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<VtpRecalculoPotenciaDTO> ListVtpRecalculoPotenciasView()
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().ListView();
        }

        #region PrimasRER.2023
        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_RECALCULO_POTENCIA, segun en anio y mes
        /// </summary>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<VtpRecalculoPotenciaDTO> ListVTP(int anio, int mes)
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().ListVTP(anio, mes);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_RECALCULO_POTENCIA que este Cerrado
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>VtpRecalculoPotenciaDTO</returns>
        public VtpRecalculoPotenciaDTO GetByIdVtpRecalculoPotenciaCerrado(int pericodi, int recpotcodi)
        {
            try
            {
                return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetByIdCerrado(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_RECALCULO_POTENCIA
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>VtpRecalculoPotenciaDTO</returns>
        public VtpRecalculoPotenciaDTO GetByIdVtpRecalculoPotenciaView(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetByIdView(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_RECALCULO_POTENCIA filtrado por el pericodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<VtpRecalculoPotenciaDTO> ListByPericodiVtpRecalculoPotencia(int pericodi)
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().ListByPericodi(pericodi);
        }

        /// <summary>
        /// Permite obtener la ultima versión en el periodo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns>recpotcodi</returns>
        public int GetByMaxIdRecPotCodi(int pericodi)
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetByMaxIdRecPotCodi(pericodi);
        }

        //ASSETEC 202108 - TIEE
        /// <summary>
        /// Permite Lista de Periodos con la ultima versión de recalculo
        /// </summary>
        /// <returns>Lista de VtpRecalculoPotenciaDTO</returns>
        public List<VtpRecalculoPotenciaDTO> ListMaxRecalculoByPeriodo()
        {
            return FactoryTransferencia.GetVtpRecalculoPotenciaRepository().ListMaxRecalculoByPeriodo();
        }

        /// <summary>
        /// Permite migrar los saldos de VTP de la empresaorigen hacia empresadestino en un periodo y revisión
        /// </summary>
        /// <param name="emprcodiorigen">Empresa de origen de los saldos</param>
        /// <param name="emprcodidestino">Empresa de destino de los saldos</param>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recpotcodi">Versión del recalculo</param>
        /// <param name="sMensaje">Mensaje de error</param>
        /// <param name="sDetalle">Detalle de error</param>
        /// <returns></returns>
        public string MigrarSaldosVTP(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi, out string sMensaje, out string sDetalle)
        {
            string sSql = "";
            try
            {
                sMensaje = "";
                sDetalle = "";
                sSql = FactoryTransferencia.GetVtpRecalculoPotenciaRepository().MigrarSaldosVTP(emprcodiorigen, emprcodidestino, pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                sDetalle = ex.StackTrace;
            }
            return sSql;
        }

        /// <summary>
        /// Permite migrar la información de VTP de la empresa origen => destino de un periodo y versión de recalculo
        /// </summary>
        /// <param name="emprcodiorigen">Empresa de origen de los saldos</param>
        /// <param name="emprcodidestino">Empresa de destino de los saldos</param>
        /// <param name="pericodi">Periodo en que se ejecuta el recalculo</param>
        /// <param name="recpotcodi">Versión del recalculo</param>
        /// <param name="sMensaje">Mensaje de error</param>
        /// <param name="sDetalle">Detalle de error</param>
        /// <returns></returns>
        public string MigrarCalculoVTP(int emprcodiorigen, int emprcodidestino, int pericodi, int recpotcodi, out string sMensaje, out string sDetalle)
        {
            string sSql = "";
            try
            {
                sMensaje = "";
                sDetalle = "";
                sSql = FactoryTransferencia.GetVtpRecalculoPotenciaRepository().MigrarCalculoVTP(emprcodiorigen, emprcodidestino, pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                sDetalle = ex.StackTrace;
            }
            return sSql;
        }
        #endregion

        #region Métodos Tabla VTP_REPA_RECA_PEAJE

        /// <summary>
        /// Inserta un registro de la tabla VTP_REPA_RECA_PEAJE
        /// </summary>
        public int SaveVtpRepaRecaPeaje(VtpRepaRecaPeajeDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpRepaRecaPeajeRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_REPA_RECA_PEAJE
        /// </summary>
        public void UpdateVtpRepaRecaPeaje(VtpRepaRecaPeajeDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_REPA_RECA_PEAJE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        public void DeleteVtpRepaRecaPeaje(int pericodi, int recpotcodi, int rrpecodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeRepository().Delete(pericodi, recpotcodi, rrpecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla VTP_REPA_RECA_PEAJE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        private void DeleteByCriteriaVtpRepaRecaPeaje(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_REPA_RECA_PEAJE
        /// </summary>
        public VtpRepaRecaPeajeDTO GetByIdVtpRepaRecaPeaje(int pericodi, int recpotcodi, int rrpecodi)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeRepository().GetById(pericodi, recpotcodi, rrpecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_REPA_RECA_PEAJE
        /// </summary>
        public List<VtpRepaRecaPeajeDTO> ListVtpRepaRecaPeajes()
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpRepaRecaPeaje
        /// </summary>
        public List<VtpRepaRecaPeajeDTO> GetByCriteriaVtpRepaRecaPeajes(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeRepository().GetByCriteria(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpRepaRecaPeaje
        /// </summary>
        public VtpRepaRecaPeajeDTO GetByNombreVtpRepaRecaPeaje(int pericodi, int recpotcodi, string rrpenombre)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeRepository().GetByNombre(pericodi, recpotcodi, rrpenombre);
        }

        #endregion

        #region Métodos Tabla VTP_REPA_RECA_PEAJE_DETALLE

        /// <summary>
        /// Inserta un registro de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        public void SaveVtpRepaRecaPeajeDetalle(VtpRepaRecaPeajeDetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        public void UpdateVtpRepaRecaPeajeDetalle(VtpRepaRecaPeajeDetalleDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        public void DeleteVtpRepaRecaPeajeDetalle(int rrpdcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().Delete(rrpdcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        public VtpRepaRecaPeajeDetalleDTO GetByIdVtpRepaRecaPeajeDetalle(int rrpdcodi)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().GetById(rrpdcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        public List<VtpRepaRecaPeajeDetalleDTO> ListVtpRepaRecaPeajeDetalles(int pericodi, int recpotcodi, int rrpecodi)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().List(pericodi, recpotcodi, rrpecodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpRepaRecaPeajeDetalle
        /// </summary>
        public List<VtpRepaRecaPeajeDetalleDTO> GetByCriteriaVtpRepaRecaPeajeDetalles(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().GetByCriteria(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite obtener el numero maximo de empresas de la tabla VTP_REPA_RECA_PEAJE_DETALLE para contruir las columnas del formulario excel
        /// </summary>
        public int GetMaxNumEmpresasVtpRepaRecaPeajeDetalles(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().GetMaxNumEmpresas(pericodi, recpotcodi);
        }

        /// <summary>
        /// Elimina registros  de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// </summary>
        public void DeleteByCriteriaVtpRepaRecaPeajeDetalle(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina registros  de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// <param name="rrpecodi">Código de la tabla REPA_RECA_PEAJE</param>
        /// </summary>
        public void DeleteByCriteriaRRPEVtpRepaRecaPeajeDetalle(int pericodi, int recpotcodi, int rrpecodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRepaRecaPeajeDetalleRepository().DeleteByCriteriaRRPE(pericodi, recpotcodi, rrpecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VTP_REPA_RECA_PEAJE_DETALLE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoRepaRecaPeajeDetalle(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpRepaRecaPeajeDTO> ListaRepaRecaPeaje = this.GetByCriteriaVtpRepaRecaPeajes(pericodi, recpotcodi);
            List<VtpRepaRecaPeajeDetalleDTO> ListaRepaRecaPeajeDetalle = this.GetByCriteriaVtpRepaRecaPeajeDetalles(pericodi, recpotcodi);
            int iCantidadEmpresas = this.GetMaxNumEmpresasVtpRepaRecaPeajeDetalles(EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

            if (formato == 1)
            {
                fileName = "ReportePeajeEgreso.xlsx";
                ExcelDocument.GenerarFormatoRepaRecaPeajeDetalle(pathFile + fileName, EntidadRecalculoPotencia, ListaRepaRecaPeaje, ListaRepaRecaPeajeDetalle, iCantidadEmpresas);
            }

            return fileName;
        }

        #endregion

        #region Métodos Tabla VTP_RETIRO_POTESC

        /// <summary>
        /// Inserta un registro de la tabla VTP_RETIRO_POTESC
        /// </summary>
        public void SaveVtpRetiroPotesc(VtpRetiroPotescDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpRetiroPotescRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_RETIRO_POTESC
        /// </summary>
        public void UpdateVtpRetiroPotesc(VtpRetiroPotescDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpRetiroPotescRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_RETIRO_POTESC
        /// </summary>
        public void DeleteVtpRetiroPotesc(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpRetiroPotescRepository().Delete(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_RETIRO_POTESC
        /// </summary>
        public VtpRetiroPotescDTO GetByIdVtpRetiroPotesc(int rpsccodi)
        {
            return FactoryTransferencia.GetVtpRetiroPotescRepository().GetById(rpsccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_RETIRO_POTESC
        /// </summary>
        public List<VtpRetiroPotescDTO> ListVtpRetiroPotescs()
        {
            return FactoryTransferencia.GetVtpRetiroPotescRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpRetiroPotesc
        /// </summary>
        public List<VtpRetiroPotescDTO> GetByCriteriaVtpRetiroPotescs()
        {
            return FactoryTransferencia.GetVtpRetiroPotescRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_RETIRO_POTESC, mas EmprNombre y BARRNOMBRE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpRetiroPotescDTO</returns>
        public List<VtpRetiroPotescDTO> ListVtpRetiroPotenciaSCView(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRetiroPotescRepository().ListView(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar los registros de la tabla VTP_RETIRO_POTESC, agrupado por EmprNombre y la suma de (rpscprecioppb * rpscpoteegreso)
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpRetiroPotescDTO</returns>
        public List<VtpRetiroPotescDTO> ListVtpRetiroPotenciaSCByEmpresa(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpRetiroPotescRepository().ListByEmpresa(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite generar el archivo de exportación de la tabla VTP_RETIRO_POTESC
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoRetiroPotenciaSC(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo)
        {
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC = this.ListVtpRetiroPotenciaSCView(pericodi, recpotcodi);

            if (formato == 1)
            {
                fileName = "ReporteRetiroPotenciaSinContrato.xlsx";
                ExcelDocument.GenerarFormatoRetiroPotenciaSC(pathFile + fileName, EntidadRecalculoPotencia, ListaRetiroPotenciaSC);
            }

            return fileName;
        }

        /// <summary>
        /// Permite generar el Reporte de los Retiros de potencia sin contrato - CU17
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarReporteRetirosSC(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC = this.ListVtpRetiroPotenciaSCView(pericodi, recpotcodi);

            PeriodoDTO EntidadPeriodo = new PeriodoAppServicio().GetByIdPeriodo(pericodi);
            if (formato == 1)
            {
                fileName = "ReporteRetiroPotenciaSinContrato.xlsx";

                ExcelDocument.GenerarReporteRetiroSC(EntidadPeriodo.PeriFormNuevo, pathFile + fileName, EntidadRecalculoPotencia, ListaRetiroPotenciaSC, out hoja);
            }
            if (formato == 2)
            {
                fileName = "ReporteRetiroPotenciaSinContrato.pdf";
                PdfDocument.GenerarReporteRetiroSC(pathFile + fileName, EntidadRecalculoPotencia, ListaRetiroPotenciaSC, pathLogo);
            }

            return fileName;
        }

        public string GenerarReporteUnificado(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago = this.ListVtpPeajeEmpresaPagoPeajePago(pericodi, recpotcodi);
            List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago = this.ListVtpIngresoTarifariosEmpresaPago(pericodi, recpotcodi);
            List<VtpEmpresaPagoDTO> ListaEmpresaPago = this.ListVtpEmpresaPagosPago(pericodi, recpotcodi);
            List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso = this.ListVtpPeajeEgresoMinfoPotenciaValor(pericodi, recpotcodi);
            List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC = this.ListVtpRetiroPotenciaSCView(pericodi, recpotcodi);
            List<VtpPagoEgresoDTO> ListaPagoEgreso = this.ListVtpPagoEgreso(pericodi, recpotcodi);
            List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa = this.ListVtpIngresoPotUnidPromdEmpresaCentral(pericodi, recpotcodi);
            List<VtpIngresoPotefrDTO> ListaIngresoPotEFR = this.GetByCriteriaVtpIngresoPotefrs(pericodi, recpotcodi);
            List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa = this.ListVtpSaldoEmpresas(pericodi, recpotcodi);
            List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo;

            #region IngresoVtpNuevo

            string pegrmicalidad2 = "*", pegrmitipousuario = "*", pegrmilicitacion = "*", pegrmicalidad = "*";
            ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfoVista(pericodi, recpotcodi, 0, 0, 0, 0, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);

            SiEmpresaDTO empresa = FactorySic.GetSiEmpresaRepository().GetById(11153);

            foreach (VtpPeajeEgresoMinfoDTO item in ListaPeajeEgresoMinfo)
            {
                if (item.Genemprcodi == 10582)
                {
                    item.Genemprnombre = empresa.Emprnomb;
                }
            }

            #endregion

            PeriodoDTO EntidadPeriodo = new PeriodoAppServicio().GetByIdPeriodo(pericodi);
            if (formato == 1)
            {
                fileName = "ReporteUnificado.xlsx";

                ExcelDocument.GenerarReporteUnificadoNuevo(
                    pathFile + fileName,
                    EntidadRecalculoPotencia,
                    ListaPeajeEmpresaPago,
                    ListaIngresoTarifarioPago,
                    ListaEmpresaPago,
                    ListaPeajeEgresoMinfo,
                    EntidadPeriodo.PeriFormNuevo == 1,
                    ListaPeajeEgreso,
                    ListaRetiroPotenciaSC,
                    EntidadPeriodo,
                    ListaPagoEgreso,
                    ListaIngresoPotenciaEmpresa,
                    ListaIngresoPotEFR,
                    ListaSaldoEmpresa,
                    out hoja);
            }
            return fileName;
        }

        #endregion

        #region Métodos Tabla VTP_SALDO_EMPRESA_AJUSTE

        /// <summary>
        /// Inserta un registro de la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        public int SaveVtpSaldoEmpresaAjuste(VtpSaldoEmpresaAjusteDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        public void UpdateVtpSaldoEmpresaAjuste(VtpSaldoEmpresaAjusteDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        public void DeleteVtpSaldoEmpresaAjuste(int potseacodi)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().Delete(potseacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina todos los registros de la tabla VTP_SALDO_EMPRESA_AJUSTE asociados a un mes de valorización
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        public void DeleteByCriteriaVtpSaldoEmpresaAjuste(int pericodi)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().DeleteByCriteria(pericodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        public VtpSaldoEmpresaAjusteDTO GetByIdVtpSaldoEmpresaAjuste(int potseacodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().GetById(potseacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        public List<VtpSaldoEmpresaAjusteDTO> ListVtpSaldoEmpresaAjuste()
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        public List<VtpSaldoEmpresaAjusteDTO> GetByCriteriaVtpSaldoEmpresaAjuste(int pericodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().GetByCriteria(pericodi);
        }

        /// <summary>
        /// Permite obtener el saldo a aplicar en este periodo de la tabla VTP_SALDO_EMPRESA_AJUSTE
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización donde se aplica el ajuste</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <returns>Ajuste del mes</returns>
        public decimal GetSaldoEmpresaAjuste(int pericodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaAjusteRepository().GetAjuste(pericodi, emprcodi);
        }
        #endregion

        #region Métodos Tabla VTP_SALDO_EMPRESA

        /// <summary>
        /// Inserta un registro de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        public int SaveVtpSaldoEmpresa(VtpSaldoEmpresaDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpSaldoEmpresaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        public void UpdateVtpSaldoEmpresa(VtpSaldoEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los registro de la tabla VTP_SALDO_EMPRESA, para un pericodi y recpotcodi
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="potsepericodidest">Código del Mes de valorización sonde se aplicara el saldo por cargo</param>
        public void UpdateVtpSaldoEmpresaPeriodoDestino(int pericodi, int recpotcodi, int potsepericodidest)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaRepository().UpdatePeriodoDestino(pericodi, recpotcodi, potsepericodidest);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        public void DeleteVtpSaldoEmpresa(int potsecodi)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaRepository().Delete(potsecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registros de la tabla VTP_SALDO_EMPRESA para un Mes de valorización y una versión de recalculo
        /// </summary>
        public void DeleteByCriteriaVtpSaldoEmpresa(int pericodi, int recpotcodi)
        {
            try
            {
                FactoryTransferencia.GetVtpSaldoEmpresaRepository().DeleteByCriteria(pericodi, recpotcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        public VtpSaldoEmpresaDTO GetByIdVtpSaldoEmpresa(int potsecodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().GetById(potsecodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_SALDO_EMPRESA por pericodi, recpotcodi, emprcodi
        /// </summary>
        public VtpSaldoEmpresaDTO GetByIdVtpSaldoEmpresaSaldo(int pericodi, int recpotcodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().GetByIdSaldo(pericodi, recpotcodi, emprcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla VTP_SALDO_EMPRESA por pericodi, recpotcodi, emprcodi
        /// </summary>
        public List<VtpSaldoEmpresaDTO> GetByIdVtpSaldoEmpresaSaldoGeneral(int pericodi, int recpotcodi, int emprcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().GetByIdSaldoGeneral(pericodi, recpotcodi, emprcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        public List<VtpSaldoEmpresaDTO> ListVtpSaldoEmpresas(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().List(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite calcular los saldos de una empresa de generación
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpSaldoEmpresaDTO</returns>
        public List<VtpSaldoEmpresaDTO> ListVtpSaldoEmpresasCalculaSaldo(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().ListCalculaSaldo(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite obtener el saldo anterior para un periodo y empresa de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        /// <param name="pecarpericodidest">Código del Mes de valorización de saldos</param>
        /// <param name="emprcodi">Código del empresa</param>
        /// <returns>El saldo de la empresa</returns>
        public decimal GetSaldoEmpresaSaldoAnterior(int potsepericodidest, int emprcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().GetSaldoAnterior(potsepericodidest, emprcodi);
        }

        /// <summary>
        /// Permite listar las empresa con Saldo Positivo en un mes de valorización y recálculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpSaldoEmpresaDTO</returns>
        public List<VtpSaldoEmpresaDTO> ListVtpSaldoEmpresasPositiva(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().ListPositiva(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite listar las empresa con Saldo Negativo (más el total saldo negativo) en un mes de valorización y recálculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <returns>Lista de VtpSaldoEmpresaDTO</returns>
        public List<VtpSaldoEmpresaDTO> ListVtpSaldoEmpresasNegativa(int pericodi, int recpotcodi)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().ListNegativa(pericodi, recpotcodi);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla VtpSaldoEmpresa
        /// </summary>
        public List<VtpSaldoEmpresaDTO> GetByCriteriaVtpSaldoEmpresas()
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar los periodos que estan relacionados con los saldos de esta valorización
        /// </summary>
        /// <param name="potsepericodidest">Código del Mes de valorización</param>
        /// <returns>Lista de PeriodoDTO</returns>
        public List<VtpSaldoEmpresaDTO> ListPeriodosDestino(int potsepericodidest)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().ListPeriodosDestino(potsepericodidest);
        }

        /// <summary>
        /// Permite obtener el saldo de un periodo y empresa de la tabla VTP_SALDO_EMPRESA
        /// </summary>
        /// <param name="emprcodi">Código del empresa</param>
        /// <param name="pericodi">Código del Mes de valorización donde esta registrado el saldo</param>
        /// <param name="pecarpericodidest">Código del Mes de valorización a donde se aplica el saldo</param>
        /// <returns>VtpSaldoEmpresaDTO</returns>
        public VtpSaldoEmpresaDTO GetSaldoEmpresaPeriodo(int emprcodi, int pericodi, int potsepericodidest)
        {
            return FactoryTransferencia.GetVtpSaldoEmpresaRepository().GetSaldoEmpresaPeriodo(emprcodi, pericodi, potsepericodidest);
        }
        #endregion

        #region Métodos Tabla VTP_VALIDACION_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla VTP_VALIDACION_ENVIO
        /// </summary>
        public int SaveVtpValidacionEnvio(VtpValidacionEnvioDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetVtpValidacionEnvioRepository().Save(entity);

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Devuelve la lista de validaciones de envío por pegrcodi
        /// </summary>
        /// <param name="pegrcodi">Codigo de envío</param>
        /// <returns></returns>
        public List<VtpValidacionEnvioDTO> GetValidacionEnvioByPegrcodi(int pegrcodi)
        {
            try
            {
                return FactoryTransferencia.GetVtpValidacionEnvioRepository().GetValidacionEnvioByPegrcodi(pegrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        #endregion

        #region Métodos Genelares

        /// <summary>
        /// Elimina toda la información calculada de un mes de valoirización y una versión de recalculo de potencia
        /// </summary>
        /// <param name="pericodi">Mes de valoirización</param>
        /// <param name="recpotcodi">Versión de recalculo</param>
        /// <returns>1 si la eliminación fue correcta</returns>
        public string EliminarProceso(int pericodi, int recpotcodi)
        {
            try
            {
                //Elimina información de la tabla VTP_PEAJE_CARGO = Información de los cargos por peaje en una empresa
                this.DeleteByCriteriaVtpPeajeCargo(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PEAJE_EMPRESA = Almacena el importe recaudado de cada empresa
                this.DeleteByCriteriaVtpPeajeEmpresa(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PEAJE_EMPRESA_PAGO = Almacena el importe que la empresa (EMPRCODIPEAJE) paga a la empresa(EMPRCODICARGO)
                this.DeleteByCriteriaVtpPeajeEmpresaPago(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PEAJE_SALDO_TRANSMISION = Almacena el importe que la empresa (EMPRCODI) pago
                this.DeleteByCriteriaVtpPeajeSaldoTransmision(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_INGRESO_TARIFARIO = Almacena los Ingresos tarifarios por empresa (EMPRCODI)
                this.DeleteByCriteriaVtpIngresoTarifario(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PAGO_EGRESO = Almacena el importe por el pago de Egresos por empresa (EMPRCODI)
                this.DeleteByCriteriaVtpPagoEgreso(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_INGRESO_POTUNID_INTERVL = Almacena el importe por intrevalo de Potencia Firme Remunerable por cada empresa / unidad generadora
                this.DeleteByCriteriaVtpIngresoPotUnidIntervl(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_INGRESO_POTUNID_PROMD = Almacena el importe promedio en el mes de valorización de Potencia Firme Remunerable por cada empresa / unidad generadora
                this.DeleteByCriteriaVtpIngresoPotUnidPromd(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_INGRESO_POTENCIA = Almacena los ingresos por potencia por empresa generadora
                this.DeleteByCriteriaVtpIngresoPotencia(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_SALDO_EMPRESA = Almacena los saldos por empresa generadora
                this.DeleteByCriteriaVtpEmpresaPago(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_SALDO_EMPRESA = Almacena los saldos por empresa generadora
                this.DeleteByCriteriaVtpSaldoEmpresa(pericodi, recpotcodi);

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Elimina toda la información copiada de una versión a otra por un nuevo recalculo de potencia
        /// </summary>
        /// <param name="pericodi">Mes de valoirización</param>
        /// <param name="recpotcodi">Versión de recalculo</param>
        /// <returns>1 si la eliminación fue correcta</returns>
        public string EliminarVersion(int pericodi, int recpotcodi)
        {
            try
            {
                //Elimina información de la tabla VTP_PEAJE_INGRESO = Información de los cargos
                this.DeleteByCriteriaVtpPeajeIngreso(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_REPA_RECA_PEAJE_DETALLE
                this.DeleteByCriteriaVtpRepaRecaPeajeDetalle(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_REPA_RECA_PEA
                this.DeleteByCriteriaVtpRepaRecaPeaje(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_RETIRO_POTESC
                this.DeleteVtpRetiroPotesc(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_INGRESO_POTEFR_DETALLE
                this.DeleteByCriteriaVtpIngresoPotefrDetalleVersion(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_INGRESO_POTEFR
                this.DeleteByCriteriaVtpIngresoPotefr(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PEAJE_EGRESO_DETALLE
                this.DeleteByCriteriaVtpPeajeEgresoDetalle(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PEAJE_EGRESO
                this.DeleteByCriteriaVtpPeajeEgreso(pericodi, recpotcodi);

                //Elimina información de la tabla VTP_PEAJE_EGRESO_MINFO
                this.DeleteByCriteriaVtpPeajeEgresoMinfo(pericodi, recpotcodi);

                return "1";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public DataSet GeneraDataset(string RutaArchivo, int hoja)
        {
            return UtilTransfPotencia.GeneraDataset(RutaArchivo, hoja);
        }


        /// <summary>
        /// Permite generar el Reporte de Verificación
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código del Recálculo de Potencia</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string VerificarValorizacion(int pericodi, int recpotcodi, int formato, string pathFile, string pathLogo, out ExcelWorksheet hoja)
        {
            hoja = null;
            string fileName = string.Empty;
            decimal dTotalSaldo = 0;
            decimal dTotalMensual = 0;
            decimal dTotal = 0;
            decimal[] dTotalColum = new decimal[1000]; // Donde se almacenan los Totales por columnas
            int colum = 0;
            int row = 0;
            decimal dTotalPingPeajeMensual = 0;
            decimal dTotalPingTariMensual = 0;

            decimal dTotalEgresoPot = 0;
            decimal dTotalPotenciaSinContrato = 0;

            bool[] arrResult = new bool[14];

            try
            {
                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                //saldos vtp
                List<VtpSaldoEmpresaDTO> ListaSaldoEmpresa = this.ListVtpSaldoEmpresas(pericodi, recpotcodi);

                //evaluar formato del periodo
                PeriodoDTO oPeriodo = new PeriodoDTO();
                oPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);

                #region Verificación: Matriz de pagos de peaje
                //Verificación: Matriz de pagos de peaje
                List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaPago = this.ListVtpPeajeEmpresaPagoPeajePago(pericodi, recpotcodi);
                if (recpotcodi == 1)
                {
                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                    {
                        List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                        //iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                        {
                            //saldo
                            decimal dSaldoAnterior = Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                            dTotalSaldo += dSaldoAnterior;
                            //mensual
                            decimal dMes = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago);
                            dTotalMensual += dMes;

                            decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                            dTotal += dPeajePago;
                            //dTotalColum[colum] += dMes;
                            //colum++;
                        }
                    }

                    arrResult[0] = (dTotalSaldo.ToString("0.##") == 0.ToString("0.##"));

                    List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.ListVtpPeajeIngresoView(pericodi, recpotcodi);
                    foreach (var item in ListaPeajeIngreso)
                    {
                        if (item.Pingpeajemensual != null)
                        {
                            dTotalPingPeajeMensual += Convert.ToDecimal(item.Pingpeajemensual);
                        }
                    }

                    arrResult[1] = (dTotalMensual.ToString("0.##") == dTotalPingPeajeMensual.ToString("0.##"));
                    //arrResult[2] = (arrResult[0] == true && arrResult[1] == true);

                    if (dTotal.ToString("0.##") == (dTotalSaldo + dTotalMensual).ToString("0.##"))
                    {
                        arrResult[2] = true;
                    }
                    else
                    {
                        arrResult[2] = false;
                    }
                }

                if (recpotcodi > 1)
                {
                    foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaPago in ListaPeajeEmpresaPago)
                    {
                        List<VtpPeajeEmpresaPagoDTO> ListaPeajeEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpPeajeEmpresaPagoPeajeCobro(dtoEmpresaPago.Emprcodipeaje, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                        //iNumEmpresaCobro = ListaPeajeEmpresaCobro.Count();
                        foreach (VtpPeajeEmpresaPagoDTO dtoEmpresaCobro in ListaPeajeEmpresaCobro)
                        {
                            //saldo
                            decimal dSaldoAnterior = Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                            dTotalSaldo += dSaldoAnterior;
                            //total
                            decimal dPeajePago = Convert.ToDecimal(dtoEmpresaCobro.Pempagpeajepago) + Convert.ToDecimal(dtoEmpresaCobro.Pempagsaldoanterior) + Convert.ToDecimal(dtoEmpresaCobro.Pempagajuste);
                            dTotal += dPeajePago;

                        }

                    }

                    arrResult[0] = (dTotalSaldo == 0);

                    List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.ListVtpPeajeIngresoView(pericodi, recpotcodi);
                    foreach (var item in ListaPeajeIngreso)
                    {
                        if (item.Pingpeajemensual != null)
                        {
                            dTotalPingPeajeMensual += Convert.ToDecimal(item.Pingpeajemensual);
                        }
                    }

                    arrResult[1] = (dTotal.ToString("0.##") == dTotalPingPeajeMensual.ToString("0.##"));
                    if (dTotal.ToString("0.##") == (dTotalSaldo).ToString("0.##"))
                    {
                        arrResult[2] = true;
                    }
                    else
                    {
                        arrResult[2] = false;
                    }
                    //arrResult[2] = (arrResult[0] == true && arrResult[1] == true);
                }
                #endregion

                #region Verificación: Matriz de pagos de Ingresos tarifarios
                //Verificación: Matriz de pagos de ingreso tarifario
                List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioPago = this.ListVtpIngresoTarifariosEmpresaPago(pericodi, recpotcodi);
                dTotalSaldo = 0;
                dTotalMensual = 0;
                dTotal = 0;
                if (recpotcodi == 1)
                {
                    foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                    {
                        List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                        foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                        {
                            //saldos
                            decimal dSaldo = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                            dTotalSaldo += dSaldo;
                            //mensual
                            decimal dTotalMes = dtoIngresoTarifarioCobro.Ingtarimporte;
                            dTotalMensual += dTotalMes;
                            //total
                            decimal dIngtarimporte = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarimporte) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                            dTotal += dIngtarimporte;
                            colum++;
                        }
                    }
                    arrResult[3] = (dTotalSaldo.ToString("0.##") == 0.ToString("0.##"));

                    List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.ListVtpPeajeIngresoView(pericodi, recpotcodi);
                    foreach (var item in ListaPeajeIngreso)
                    {
                        if (item.Pingpeajemensual != null)
                        {
                            dTotalPingTariMensual += Convert.ToDecimal(item.Pingtarimensual);
                        }
                    }

                    arrResult[4] = (dTotalMensual.ToString("0.##") == dTotalPingTariMensual.ToString("0.##"));

                    if (dTotal.ToString("0.##") == (dTotalSaldo + dTotalMensual).ToString("0.##"))
                    {
                        arrResult[5] = true;
                    }
                    else
                    {
                        arrResult[5] = false;
                    }

                    //arrResult[5] = (arrResult[3] == true && arrResult[4] == true);


                }

                if (recpotcodi > 1)
                {
                    foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioPago in ListaIngresoTarifarioPago)
                    {
                        List<VtpIngresoTarifarioDTO> ListaIngresoTarifarioCobro = (new TransfPotenciaAppServicio()).ListVtpIngresoTarifariosEmpresaCobro(dtoIngresoTarifarioPago.Emprcodingpot, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);

                        foreach (VtpIngresoTarifarioDTO dtoIngresoTarifarioCobro in ListaIngresoTarifarioCobro)
                        {
                            //saldos
                            decimal dSaldo = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                            dTotalSaldo += dSaldo;
                            //total
                            decimal dIngtarimporte = Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarimporte) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarsaldoanterior) + Convert.ToDecimal(dtoIngresoTarifarioCobro.Ingtarajuste);
                            dTotal += dIngtarimporte;
                            colum++;
                        }
                    }
                    arrResult[3] = (dTotalSaldo.ToString("0.##") == 0.ToString("0.##"));

                    List<VtpPeajeIngresoDTO> ListaPeajeIngreso = this.ListVtpPeajeIngresoView(pericodi, recpotcodi);
                    foreach (var item in ListaPeajeIngreso)
                    {
                        if (item.Pingpeajemensual != null)
                        {
                            dTotalPingTariMensual += Convert.ToDecimal(item.Pingtarimensual);
                        }
                    }

                    arrResult[4] = (dTotal.ToString("0.##") == dTotalPingTariMensual.ToString("0.##"));
                    if (dTotal.ToString("0.##") == (dTotalSaldo).ToString("0.##"))
                    {
                        arrResult[5] = true;
                    }
                    else
                    {
                        arrResult[5] = false;
                    }
                    //arrResult[5] = (arrResult[3] == true && arrResult[4] == true);
                }
                #endregion

                #region Verificación: Matriz de pagos de potencia
                arrResult[6] = true;
                arrResult[7] = true;
                if (recpotcodi == 1)
                {
                    //matriz de pagos
                    IDictionary<int, decimal> emprPagos = new Dictionary<int, decimal>();
                    IDictionary<int, decimal> emprCobros = new Dictionary<int, decimal>();
                    List<VtpEmpresaPagoDTO> ListaEmpresaPago = this.ListVtpEmpresaPagosPago(pericodi, recpotcodi);

                    foreach (var item in ListaEmpresaPago)
                    {
                        int iEmprcodiPago = item.Emprcodipago;
                        List<VtpEmpresaPagoDTO> ListaEmpresaCobro = (new TransfPotenciaAppServicio()).ListVtpEmpresaPagosCobro(iEmprcodiPago, EntidadRecalculoPotencia.Pericodi, EntidadRecalculoPotencia.Recpotcodi);
                        foreach (var item2 in ListaEmpresaCobro)
                        {
                            decimal montoCobro = item2.Potepmonto;
                            if (emprCobros.ContainsKey(item2.Emprcodicobro))
                            {
                                emprCobros[item2.Emprcodicobro] += montoCobro;

                            }
                            else
                            {
                                emprCobros.Add(item2.Emprcodicobro, montoCobro);
                            }

                        }

                        decimal dTotalPago = ListaEmpresaCobro.Sum(x => x.Potepmonto);
                        emprPagos.Add(item.Emprcodipago, dTotalPago);

                    }
                    int cantEmpresaSaldo = 0;
                    //saldos vtp
                    var ListaSaldosSinCeros = ListaSaldoEmpresa.Where(x => (x.Potsesaldo + x.Potsesaldoanterior).ToString("0.##") != 0.ToString("0.##")).ToList();
                    foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in ListaSaldoEmpresa)
                    {
                        decimal dSaldoNeto = 0;

                        dSaldoNeto = dtoSaldoEmpresa.Potsesaldo + dtoSaldoEmpresa.Potsesaldoanterior;

                        if (dSaldoNeto.ToString("0.##") != 0.ToString("0.##"))
                        {
                            cantEmpresaSaldo++;
                        }

                        if (dSaldoNeto > 0)
                        {
                            if (emprCobros.ContainsKey(dtoSaldoEmpresa.Emprcodi))
                            {
                                if (emprCobros[dtoSaldoEmpresa.Emprcodi].ToString("0.##") == dSaldoNeto.ToString("0.##"))
                                {
                                    arrResult[6] = true;
                                }
                                else
                                {
                                    arrResult[6] = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (emprPagos.ContainsKey(dtoSaldoEmpresa.Emprcodi))
                            {
                                if (emprPagos[dtoSaldoEmpresa.Emprcodi].ToString("0.##") == Math.Abs(dSaldoNeto).ToString("0.##"))
                                {
                                    arrResult[7] = true;
                                }
                                else
                                {
                                    arrResult[7] = false;
                                    break;
                                }
                            }
                        }

                    }

                    arrResult[8] = ((emprCobros.Count + emprPagos.Count) == ListaSaldosSinCeros.Count);

                    //para Egresos saldos peajes
                    dTotalEgresoPot = ListaSaldoEmpresa.Sum(x => x.Potseegreso);

                }
                #endregion

                #region Egresos 
                decimal dTotalPotenciaBilateral = 0;
                decimal dTotalPotenciaLicitacion = 0;

                List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgreso = this.ListVtpPeajeEgresoMinfoPotenciaValor(pericodi, recpotcodi);

                foreach (var item in ListaPeajeEgreso)
                {
                    dTotalPotenciaBilateral += Convert.ToDecimal(item.Pegrmipotecalculada);
                    dTotalPotenciaLicitacion += Convert.ToDecimal(item.Pegrmipotedeclarada);
                }

                decimal? dTotalPotenciaEgreso = 0;


                if (oPeriodo.PeriFormNuevo == 1)
                {
                    List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfoVistaNuevo(pericodi, recpotcodi, 0, 0, 0, 0, "*", "*", "*", "*");

                    foreach (var item in ListaPeajeEgresoMinfo)
                    {
                        decimal? potencia = (item.Pegrdpotecoincidente == null) ? item.Pegrmipoteegreso : item.Pegrdpotecoincidente;
                        dTotalPotenciaEgreso += potencia;
                    }

                    arrResult[9] = true;
                    //((dTotalPotenciaBilateral + dTotalPotenciaLicitacion).ToString("0.###") == Convert.ToDecimal(dTotalPotenciaEgreso).ToString("0.###"));

                }
                else
                {
                    List<VtpPeajeEgresoMinfoDTO> ListaPeajeEgresoMinfo = this.GetByCriteriaVtpPeajeEgresoMinfoVista(pericodi, recpotcodi, 0, 0, 0, 0, "*", "*", "*", "*");

                    foreach (var item in ListaPeajeEgresoMinfo)
                    {
                        decimal? potencia = item.Pegrmipoteegreso;
                        dTotalPotenciaEgreso += potencia;
                    }

                    arrResult[9] = true;
                    //((dTotalPotenciaBilateral + dTotalPotenciaLicitacion).ToString("0.###") == Convert.ToDecimal(dTotalPotenciaEgreso).ToString("0.###"));

                }

                dTotalPotenciaSinContrato = Convert.ToDecimal(ListaPeajeEgreso.Sum(x => x.Pegrmipreciopote));

                #endregion

                #region Egresos saldos peajes
                decimal dTotalEgresoCompraPot = 0;
                decimal dTotalIngresoPot = 0;

                List<VtpPagoEgresoDTO> ListaPagoEgreso = this.ListVtpPagoEgreso(pericodi, recpotcodi);
                List<VtpIngresoPotUnidPromdDTO> ListaIngresoPotenciaEmpresa = this.ListVtpIngresoPotUnidPromdEmpresaCentral(pericodi, recpotcodi);

                dTotalEgresoCompraPot = ListaPagoEgreso.Sum(x => x.Pagegrpagoegreso);
                dTotalIngresoPot = ListaIngresoPotenciaEmpresa.Sum(x => x.Inpuprimportepromd);

                //if (dTotalEgresoPot.ToString("0.##") == dTotalEgresoCompraPot.ToString("0.##")
                //    && dTotalEgresoPot.ToString("0.##") == dTotalIngresoPot.ToString("0.##"))
                //{
                //    arrResult[10] = true;
                //}
                //else
                //{
                //    arrResult[10] = false;
                //}
                arrResult[10] = true;

                #endregion

                #region Retiros no cubiertos
                decimal dTotalRpscpoteegreso = 0;
                List<VtpRetiroPotescDTO> ListaRetiroPotenciaSC = this.ListVtpRetiroPotenciaSCView(pericodi, recpotcodi);
                dTotalRpscpoteegreso = Convert.ToDecimal(ListaRetiroPotenciaSC.Sum(x => x.Rpscpoteegreso));

                arrResult[11] = (dTotalPotenciaSinContrato.ToString("0.##") == dTotalRpscpoteegreso.ToString("0.##"));

                #endregion

                #region Saldos de Potencia

                decimal dSaldoMes = 0;
                int iCantPeriodo = 0;
                arrResult[12] = true;
                arrResult[13] = true;
                if (recpotcodi == 1)
                {
                    List<VtpSaldoEmpresaDTO> ListaPeriodoDestino = (new TransfPotenciaAppServicio()).ListPeriodosDestino(EntidadRecalculoPotencia.Pericodi);
                    iCantPeriodo = ListaPeriodoDestino.Count();
                    foreach (VtpSaldoEmpresaDTO dtoSaldoEmpresa in ListaSaldoEmpresa)
                    {
                        colum = 0;
                        foreach (VtpSaldoEmpresaDTO dtoPeriodo in ListaPeriodoDestino)
                        {
                            //- Linea agregada egjunin
                            decimal saldoAdicional = 0;
                            if (dtoSaldoEmpresa.Emprcodi == 11153)
                            {
                                VtpSaldoEmpresaDTO dtoSaldoAdicional = (new TransfPotenciaAppServicio()).GetSaldoEmpresaPeriodo(10582, dtoPeriodo.Pericodi, dtoSaldoEmpresa.Pericodi);

                                if (dtoSaldoAdicional != null)
                                {
                                    saldoAdicional = dtoSaldoAdicional.Potsesaldoreca;
                                }
                            }

                            VtpSaldoEmpresaDTO dtoSaldoAnterior = (new TransfPotenciaAppServicio()).GetSaldoEmpresaPeriodo(dtoSaldoEmpresa.Emprcodi, dtoPeriodo.Pericodi, dtoSaldoEmpresa.Pericodi);
                            if (dtoSaldoAnterior != null)
                            {
                                dSaldoMes += dtoSaldoAnterior.Potsesaldoreca + saldoAdicional;  //- Linea agregada egjunin
                            }
                            colum++;
                            if (iCantPeriodo == colum)
                            {
                                arrResult[13] = true;
                            }
                        }
                    }

                    arrResult[12] = (dSaldoMes.ToString("0.##") == 0.ToString("0.##"));

                }

                #endregion

                if (formato == 1)
                {
                    fileName = "ReporteVerificacionResultados.xlsx";
                    ExcelDocument.GenerarReporteVerificacionResultados(pathFile + fileName, EntidadRecalculoPotencia, arrResult, out hoja);
                }
            }
            catch (Exception ex)
            {

            }


            return fileName;
        }

        /// <summary>
        /// Listar central y unidad pfr
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="listaEmpresa"></param>
        /// <param name="listaCentral"></param>
        /// <param name="listaUnidad"></param>
        public void ListarEmpresaCentralUnidad(int pericodi, out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaUnidad)
        {
            PotenciaFirmeRemunerableAppServicio pfrCargaServicio = new PotenciaFirmeRemunerableAppServicio();
            PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
            PfrRecalculoDTO ultimoRecalculo = new PfrRecalculoDTO();

            //>>>>>>>>>>>>>>>>>
            var regPeriodo = servicioPeriodo.ListPeriodo().Find(x => x.PeriCodi == pericodi);
            var listaPeriodosPfr = pfrCargaServicio.ListPfrPeriodos();
            PfrPeriodoDTO periodoTransferencia = listaPeriodosPfr.Find(x => x.Pfrperanio == regPeriodo.AnioCodi && x.Pfrpermes == regPeriodo.MesCodi);

            if (periodoTransferencia != null)
            {
                ultimoRecalculo = pfrCargaServicio.GetByCriteriaPfrRecalculos(periodoTransferencia.Pfrpercodi).First();
            }
            //>>>>>>>>>>>>>>>>>>

            //obtener id del reporte de pfr
            var idReportePfr = pfrCargaServicio.GetUltimoPfrrptcodiXRecalculo(ultimoRecalculo.Pfrreccodi, 7);

            List<PfrReporteTotalDTO> listadoDatosPfr = pfrCargaServicio.ListPfrReporteTotalByReportecodi(idReportePfr).OrderBy(x => x.Central).ThenBy(x => x.Pfrtotunidadnomb).ToList();

            listaEmpresa = listadoDatosPfr
                .GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi ?? 0, Emprnomb = x.Key.Emprnomb })
                .OrderBy(x => x.Emprnomb)
                .ToList();
            listaCentral = listadoDatosPfr
                .GroupBy(x => new { x.Equipadre, x.Central })
                .Select(x => new EqEquipoDTO() { Equipadre = x.Key.Equipadre, Emprcodi = x.First().Emprcodi, Central = x.Key.Central })
                .ToList();
            listaUnidad = listadoDatosPfr
                .GroupBy(x => new { x.Equicodi, x.Grupocodi, x.Pfrtotficticio })
                .Select(x => new EqEquipoDTO() { Equicodi = x.Key.Equicodi.Value, Grupocodi = x.Key.Grupocodi, Pfrtotunidadnomb = x.First().Pfrtotunidadnomb, Equipadre = x.First().Equipadre, Pfrtotficticio = x.Key.Pfrtotficticio })
                .ToList();
        }

        #endregion

        #region Metodos Tabla EQ_CATEGORIA

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EqCategoriaDTO> GetByCriteriaEqCategorias()
        {
            return FactorySic.GetEqCategoriaRepository().GetByCriteriaEqCategorias();
        }
        #endregion

        #region Metodos Tabla EQ_CATEGORIA_DET

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EqCategoriaDetDTO> GetByCriteriaEqCategoriaDet(int ctgcodi)
        {
            return FactorySic.GetEqCategoriaDetalleRepository().GetByCriteria(ctgcodi);
        }
        #endregion

        #region CONTROLLER PotenciafirmeController

        #region CALCULO POTENCIA FIRME

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="origlectcodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> GetByCriteriaPtosMedicion(string equicodi, string origlectcodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetByCriteria2(equicodi, origlectcodi);
        }

        #endregion

        #region HISTORICO INDISPONIBILIDADES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list_IndDetcuadro7"></param>
        /// <returns></returns>
        public string ConsultaIndispoHtml(List<IndDetcuadro7DTO> data, List<EqEquipoDTO> unidades, DateTime mes)
        {
            StringBuilder strHtml = new StringBuilder();

            #region Cabecera
            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>EMPRESA</th>");
            strHtml.Append("<th>CENTRAL</th>");
            strHtml.Append("<th>UNIDAD</th>");
            strHtml.Append("<th>HIP</th>");
            strHtml.Append("<th>HIF</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var d in unidades)
            {
                var reg = data.Find(x => x.Equicodi == d.Equicodi && x.Cuadr7mes == mes.Month);

                if (reg != null)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td style='text-align:left'>" + reg.Emprnomb + "</td>");
                    strHtml.Append("<td style='text-align:left'>" + reg.Equinomb + "</td>");
                    strHtml.Append("<td style='text-align:left'>" + reg.Gruponomb + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + reg.Cuadr7hip + "</td>");
                    strHtml.Append("<td style='text-align:right'>" + reg.Cuadr7hif + "</td>");
                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</tbody>");
            #endregion

            return strHtml.ToString();
        }

        #endregion

        #region INDISPONIBILIDAD TEORICA

        public string ListaIndispoTeoricaHtml(List<EqCatpropiedadDTO> data, string url)
        {
            StringBuilder strHtml = new StringBuilder();

            #region Cabecera
            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>Propiedad</th>");
            strHtml.Append("<th>Usuario Creacion</th>");
            strHtml.Append("<th>Fecha Creacion</th>");
            strHtml.Append("<th>Estado</th>");
            strHtml.Append("<th>Accion</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var d in data)
            {
                string estado_ = string.Empty;
                switch (d.Eqcatpestado)
                {
                    case ConstantesAppServicio.Activo: estado_ = ConstantesAppServicio.ActivoDesc; break;
                    case ConstantesAppServicio.Baja: estado_ = ConstantesAppServicio.BajaDesc; break;
                    case ConstantesAppServicio.Anulado: estado_ = ConstantesAppServicio.AnuladoDesc; break;
                }

                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.Eqcatpnomb + "</td>");
                strHtml.Append("<td>" + d.Eqcatpusucreacion + "</td>");
                strHtml.Append("<td>" + d.Eqcatpfeccreacion + "</td>");
                strHtml.Append("<td>" + estado_ + "</td>");
                strHtml.Append("<td><a href='JavaScript:verReg(" + d.Eqcatpcodi + ")'><img src='" + url + "Content/Images/Trash.png' alt='Eliminar registro' title='Delete'></a></td>");
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            #endregion

            return strHtml.ToString();
        }

        public string ListaIndispoTeoricaDetHtml(List<EqCatpropvalorDTO> data, string url)
        {
            StringBuilder strHtml = new StringBuilder();

            #region Cabecera
            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>Propiedad</th>");
            strHtml.Append("<th>Valor</th>");
            strHtml.Append("<th>Fecha Regimiento</th>");
            strHtml.Append("<th>Usuario Creacion</th>");
            strHtml.Append("<th>Fecha Creacion</th>");
            strHtml.Append("<th>Accion</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            #endregion

            #region cuerpo
            strHtml.Append("<tbody>");
            foreach (var d in data)
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td>" + d.Eqcatpnomb + "</td>");
                strHtml.Append("<td>" + d.Eqctpvvalor + "</td>");
                strHtml.Append("<td>" + d.Eqctpvfechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td>" + d.Eqctpvusucreacion + "</td>");
                strHtml.Append("<td>" + d.Eqctpvfeccreacion.Value.ToString(ConstantesAppServicio.FormatoFecha) + "</td>");
                strHtml.Append("<td><a href='JavaScript:eliminarReg(" + d.Eqctpvcodi + ",\"" + d.Eqctpvfechadat.Value.ToString(ConstantesAppServicio.FormatoFecha) + "\")'><img src='" + url + "Content/Images/Trash.png' alt='Eliminar registro' title='Delete'></a></td>");
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            #endregion

            return strHtml.ToString();
        }

        #endregion

        #endregion


    }
}
