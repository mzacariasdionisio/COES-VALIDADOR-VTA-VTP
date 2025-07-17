using COES.Base.Core;
using System.Globalization;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PrimasRER.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using System.Configuration;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Framework.Base.Tools;
using System.IO;
using System.Text.RegularExpressions;
using COES.Servicios.Aplicacion.TransfPotencia;
using System.Drawing;
using COES.Servicios.Aplicacion.PMPO.Helper;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenSeparatorHandlers;
using ExcelLibrary.BinaryFileFormat;
using Org.BouncyCastle.Crypto;

namespace COES.Servicios.Aplicacion.PrimasRER
{
    /// <summary>
    /// Clase Servicio de PrimasRER
    /// </summary>
    public class PrimasRERAppServicio : AppServicioBase
    {
        #region Variables Generales
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PrimasRERAppServicio));

        //Servicios
        readonly INDAppServicio indAppServicio = new INDAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        ValorTransferenciaAppServicio servicioValorTransferencia = new ValorTransferenciaAppServicio();
        CostoMarginalAppServicio servicioCostoMarginal = new CostoMarginalAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();

        #endregion

        #region Extranet - Carga de la información de EDI

        #region Métodos de la tabla RERSOLICITUDEDI
        /// <summary>
        /// Permite listar solicitudes EDI por empresa y periodo.
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="ipericodi">Código de periodo</param>
        /// <returns>Lista de RerSolicitudEdiDTO</returns>
        public List<RerSolicitudEdiDTO> ListarSolicitudesEDIPorEmpresaYPeriodo(int emprcodi, int ipericodi)
        {
            return FactoryTransferencia.GetRerSolicitudEdiRepository().GetByCriteria(emprcodi, ipericodi).OrderBy(x => x.Rersedcodi).ToList();
        }

        /// <summary>
        /// Valida un registro de la tabla RER_SOLICITUDEDI, agrega el estado del envío de la solicitud EDI (FP o EP)
        /// </summary>
        /// <param name="entity">Objeto RerSolicitudEdiDTO</param>
        /// <param name="enviaArchivo">Indica si se envía un archivo adjunto como sustento</param>
        /// <param name="listaErrores">Lista de errores</param>
        /// <param name="listaInfo">Lista de advertencias</param>
        /// <returns>Boleano que indica si la solicitud EDI es válida</returns>
        public bool ValidarSolicitudEDI(RerSolicitudEdiDTO entity, bool enviaArchivo, out List<string> listaErrores, out List<string> listaInfo)
        {
            #region Definiendo variables globales
            listaErrores = new List<string>();
            listaInfo = new List<string>();
            bool esValido = true;
            INDAppServicio servicioIndisponibilidad = new INDAppServicio();
            DateTime fechaSistema = DateTime.Now;
            #endregion
            try
            {
                IndPeriodoDTO indPeriodo = servicioIndisponibilidad.GetByIdIndPeriodo(entity.Ipericodi);
                int ultimoDiaMes = DateTime.DaysInMonth(indPeriodo.Iperianio, indPeriodo.Iperimes);
                DateTime finPeriodo = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, ultimoDiaMes, 23, 59, 59);
                DateTime inicioPeriodoSig = new DateTime(indPeriodo.Iperianio, indPeriodo.Iperimes, 1, 0, 0, 0).AddMonths(1);
                DateTime inicioMinimo = new DateTime(entity.Rersedfechahorainicio.Year, entity.Rersedfechahorainicio.Month, 1, 0, 0, 0);
                DateTime finMaximo = new DateTime(entity.Rersedfechahorainicio.Year, entity.Rersedfechahorainicio.Month, 1, 0, 0, 0).AddMonths(1);

                #region Valida Central RER
                if (entity.Rercencodi == 0)
                {
                    listaErrores.Add("Debe seleccionar una central.");
                }
                #endregion

                #region Valida fecha hora de inicio y fecha hora de fin
                if (entity.Rersedfechahorainicio >= entity.Rersedfechahorafin)
                {
                    listaErrores.Add("Error de fecha, la fecha y hora de inicio debe ser menor a la fecha y hora de fin.");
                }
                if (entity.Rersedfechahorainicio > fechaSistema || entity.Rersedfechahorafin > fechaSistema)
                {
                    listaErrores.Add("Error de fecha, la Solicitud EDI debe tener una fecha anterior a la fecha de hoy.");
                }
                if (entity.Rersedfechahorainicio > inicioPeriodoSig || entity.Rersedfechahorafin > inicioPeriodoSig)
                {
                    listaErrores.Add("Error de fecha, la Solicitud EDI no puede tener fecha posterior al Periodo de Reporte EDI.");
                }
                if (finMaximo < entity.Rersedfechahorafin)
                {
                    string inicioMin = inicioMinimo.ToString(ConstantesAppServicio.FormatoFechaFull);
                    string finMax = finMaximo.ToString(ConstantesAppServicio.FormatoFechaFull);
                    listaErrores.Add("Error de fecha, el rango de fecha y hora de la Solicitud EDI debe estar dentro de un mismo mes. Ej: Fecha y hora inicio mínimo: " + inicioMin + ", Fecha y hora fin máximo: " + finMax);
                }
                #endregion

                #region Valida detalle
                if (string.IsNullOrEmpty(entity.Rerseddesc))
                {
                    listaErrores.Add("El detalle no puede estar vacío.");
                }
                if (!string.IsNullOrEmpty(entity.Rerseddesc) && entity.Rerseddesc.Length > 300)
                {
                    listaErrores.Add("El detalle debe tener como máximo 300 caracteres.");
                }

                #endregion

                #region Valida sustento
                if (entity.Rersedcodi == 0 && !enviaArchivo)
                {
                    listaErrores.Add("Debe importar un archivo de sustento.");
                }
                #endregion

                #region Valida fecha de envío

                #region Valida plazo de cierre Extranet
                DateTime? plazoCierreExtranet = ObtenerPlazoCierreExtranet(entity.Ipericodi, out string mensajeError);
                if (plazoCierreExtranet == null)
                {
                    listaErrores.Add(mensajeError);
                }
                else
                {
                    if (fechaSistema > plazoCierreExtranet)
                    {
                        listaErrores.Add("La solicitud enviada, su fecha de envío es posterior al Plazo de Cierre.");
                    }
                }
                #endregion

                #region Valida plazo de entrega EDI
                DateTime? plazoEntregaEDI = ObtenerPlazoEntregaEDI(indPeriodo);
                if (plazoEntregaEDI != null)
                {
                    if (entity.Rersedcodi == 0)
                    {
                        #region Nueva Solicitud EDI
                        if (fechaSistema > plazoEntregaEDI)
                        {
                            entity.Rersedestadodeenvio = "FP";
                            listaInfo.Add("La solicitud enviada, su fecha de envío es posterior a su Plazo de Entrega EDI respectivo. Por lo tanto, la solicitud está fuera de plazo.");
                        }
                        else
                        {
                            entity.Rersedestadodeenvio = "EP";
                            listaInfo.Add("La solicitud se envió en plazo.");
                        }
                        #endregion
                    }
                    else
                    {
                        #region Editar Solicitud EDI
                        if (fechaSistema > plazoEntregaEDI)
                        {
                            entity.Rersedestadodeenvio = "FP";
                            listaErrores.Add("La solicitud no se puede enviar, porque su fecha de envío es posterior a su Plazo de Entrega EDI respectivo. Por lo tanto, la solicitud está fuera de plazo.");
                        }
                        else
                        {
                            entity.Rersedestadodeenvio = "EP";
                            listaInfo.Add("La solicitud se envió en plazo.");
                        }
                        #endregion
                    }
                }
                else
                {
                    listaErrores.Add("No se encontró un valor para el Plazo de Entrega EDI para el Periodo de Reporte EDI seleccionado.");
                }
                #endregion

                #region Validar si la solicitud EDI fue importada hacia la Intranet
                bool existeEsteCaso = (entity.Rersedcodi != 0 && !(listaErrores != null && listaErrores.Count > 0));
                if (existeEsteCaso)
                {
                    List<RerEvaluacionSolicitudEdiDTO> listESE = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetByCriteria(-1, entity.Rersedcodi);
                    bool existeListESE = (listESE != null && listESE.Count > 0);
                    if (existeListESE)
                    {
                        listaErrores.Add("La solicitud EDI no puede ser actualizada porque ha sido procesada por el COES. Ponerse en contacto con SME.");
                    }
                }
                #endregion

                #endregion

                if (listaErrores != null && listaErrores.Count > 0)
                {
                    esValido = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                listaErrores.Add(ex.Message);
                esValido = false;
            }

            return esValido;
        }

        /// <summary>
        /// Inserta o actualiza un registro de la tabla RER_SOLICITUDEDI y su tabla detalle RER_ENERGIAUNIDAD
        /// </summary>
        /// <param name="solicitudEdi">Objeto RerSolicitudEdiDTO</param>
        /// <param name="listaEnergiaUnidad">Lista de objeto RerEnergiaUnidadDTO</param>
        /// <param name="usuario">Nombre del usuario</param>
        /// <param name="rutaSustentoAnterior">Ruta del sustento antes de reemplazarlo</param>      
        public void GuardarSolicitudEDI(RerSolicitudEdiDTO solicitudEdi, List<RerEnergiaUnidadDTO> listaEnergiaUnidad, string usuario, string rutaSustentoAnterior)
        {
            if (solicitudEdi.Rersedcodi == 0)
            {
                NuevaSolicitudEDI(solicitudEdi, listaEnergiaUnidad, usuario);
            }
            else
            {
                ActualizarSolicitudEDI(solicitudEdi, listaEnergiaUnidad, usuario, rutaSustentoAnterior);
            }
        }

        /// <summary>
        /// Transacción que inserta un registro de la tabla RER_SOLICITUDEDI y uno o más de un registro de la tabla RER_ENERGIAUNIDAD
        /// </summary>
        /// <param name="solicitudEdi">Objeto RerSolicitudEdiDTO</param>
        /// <param name="listaEnergiaUnidad">Lista de objeto RerEnergiaUnidadDTO</param>
        /// <param name="usuario">Nombre del usuario</param>
        public void NuevaSolicitudEDI(RerSolicitudEdiDTO solicitudEdi, List<RerEnergiaUnidadDTO> listaEnergiaUnidad, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion
            try
            {
                DateTime fechaActual = DateTime.Now;

                #region Crear Solicitud EDI
                conn = FactoryTransferencia.GetRerSolicitudEdiRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerSolicitudEdiRepository().StartTransaction(conn);
                int rersedcodi = FactoryTransferencia.GetRerSolicitudEdiRepository().GetMaxId();

                solicitudEdi.Rersedcodi = rersedcodi;
                solicitudEdi.Rersedusucreacion = usuario;
                solicitudEdi.Rersedusumodificacion = usuario;
                solicitudEdi.Rersedfeccreacion = fechaActual;
                solicitudEdi.Rersedfecmodificacion = fechaActual;
                solicitudEdi.Rersedtotenergia = Math.Round(listaEnergiaUnidad.Sum(eu => eu.Rereutotenergia), 4);
                FactoryTransferencia.GetRerSolicitudEdiRepository().Save(solicitudEdi, conn, tran);
                #endregion

                #region Crear Energía por Unidad de Generación de una Central de la Solicitud EDI
                int rereucodi = FactoryTransferencia.GetRerEnergiaUnidadRepository().GetMaxId();
                int rereudcodi = FactoryTransferencia.GetRerEnergiaUnidadDetRepository().GetMaxId();
                foreach (RerEnergiaUnidadDTO rerEnergiaUnidad in listaEnergiaUnidad)
                {
                    List<string> listEnergiaUnidadRestante = new List<string>();
                    if (rerEnergiaUnidad.Rereuenergiaunidad != null && rerEnergiaUnidad.Rereuenergiaunidad.Length > ConstantesPrimasRER.numero4000)
                    {
                        string energiaUnidadRestante = rerEnergiaUnidad.Rereuenergiaunidad;
                        rerEnergiaUnidad.Rereuenergiaunidad = energiaUnidadRestante.Substring(0, ConstantesPrimasRER.numero4000);
                        energiaUnidadRestante = energiaUnidadRestante.Substring(ConstantesPrimasRER.numero4000);

                        do
                        {
                            if (energiaUnidadRestante.Length > ConstantesPrimasRER.numero4000)
                            {
                                listEnergiaUnidadRestante.Add(energiaUnidadRestante.Substring(0, ConstantesPrimasRER.numero4000));
                                energiaUnidadRestante = energiaUnidadRestante.Substring(ConstantesPrimasRER.numero4000);
                            }
                            else
                            {
                                listEnergiaUnidadRestante.Add(energiaUnidadRestante);
                                energiaUnidadRestante = "";
                            }
                        }
                        while (energiaUnidadRestante.Length > 0);
                    }

                    rerEnergiaUnidad.Rereucodi = rereucodi;
                    rerEnergiaUnidad.Rersedcodi = rersedcodi;
                    rerEnergiaUnidad.Rereuusucreacion = usuario;
                    rerEnergiaUnidad.Rereufeccreacion = fechaActual;
                    FactoryTransferencia.GetRerEnergiaUnidadRepository().Save(rerEnergiaUnidad, conn, tran);
                    rereucodi++;

                    foreach (string energiaunidadRestante in listEnergiaUnidadRestante)
                    {
                        RerEnergiaUnidadDetDTO rerEnergiaUnidadDet = new RerEnergiaUnidadDetDTO
                        { 
                            Rereudcodi = rereudcodi,
                            Rereucodi = rerEnergiaUnidad.Rereucodi,
                            Rereudenergiaunidad = energiaunidadRestante
                        };
                        FactoryTransferencia.GetRerEnergiaUnidadDetRepository().Save(rerEnergiaUnidadDet, conn, tran);
                        rereudcodi++;
                    }
                }
                #endregion

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                solicitudEdi.Rersedcodi = 0;
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Transacción que actualiza un registro de la tabla RER_SOLICITUDEDI y uno o más de un registro de la tabla RER_ENERGIAUNIDAD
        /// </summary>
        /// <param name="solicitudEdi">Objeto RerSolicitudEdiDTO</param>
        /// <param name="listaEnergiaUnidad">Lista de objeto RerEnergiaUnidadDTO</param>
        /// <param name="usuario">Nombre del usuario</param>
        /// <param name="rutaSustentoAnterior">Ruta del sustento antes de reemplazarlo</param>
        public void ActualizarSolicitudEDI(RerSolicitudEdiDTO solicitudEdi, List<RerEnergiaUnidadDTO> listaEnergiaUnidad, string usuario, string rutaSustentoAnterior)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion
            try
            {
                bool existeListaEU = (listaEnergiaUnidad != null && listaEnergiaUnidad.Count > 0);
                DateTime fechaActual = DateTime.Now;

                #region Actualizar Solicitud EDI
                conn = FactoryTransferencia.GetRerSolicitudEdiRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerSolicitudEdiRepository().StartTransaction(conn);

                solicitudEdi.Rersedusumodificacion = usuario;
                solicitudEdi.Rersedfecmodificacion = fechaActual;
                if (existeListaEU) solicitudEdi.Rersedtotenergia = listaEnergiaUnidad.Sum(eu => eu.Rereutotenergia);
                FactoryTransferencia.GetRerSolicitudEdiRepository().Update(solicitudEdi, conn, tran);
                #endregion

                #region Actualizar Energía por Unidad de Generación de una Central de la Solicitud EDI
                if (existeListaEU)
                {
                    #region Eliminar Energía por Unidad de Generación
                    List<RerEnergiaUnidadDTO> listRerEnergiaUnidad = FactoryTransferencia.GetRerEnergiaUnidadRepository().List(solicitudEdi.Rersedcodi);
                    foreach (RerEnergiaUnidadDTO rerEnergiaUnidad in listRerEnergiaUnidad)
                    {
                        FactoryTransferencia.GetRerEnergiaUnidadDetRepository().Delete(rerEnergiaUnidad.Rereucodi, conn, tran);
                    }

                    FactoryTransferencia.GetRerEnergiaUnidadRepository().Delete(solicitudEdi.Rersedcodi, conn, tran);
                    #endregion

                    #region Crear Energía por Unidad de Generación
                    int rereucodi = FactoryTransferencia.GetRerEnergiaUnidadRepository().GetMaxId();
                    int rereudcodi = FactoryTransferencia.GetRerEnergiaUnidadDetRepository().GetMaxId();
                    foreach (RerEnergiaUnidadDTO rerEnergiaUnidad in listaEnergiaUnidad)
                    {
                        List<string> listEnergiaUnidadRestante = new List<string>();
                        if (rerEnergiaUnidad.Rereuenergiaunidad != null && rerEnergiaUnidad.Rereuenergiaunidad.Length > ConstantesPrimasRER.numero4000)
                        {
                            string energiaUnidadRestante = rerEnergiaUnidad.Rereuenergiaunidad;
                            rerEnergiaUnidad.Rereuenergiaunidad = energiaUnidadRestante.Substring(0, ConstantesPrimasRER.numero4000);
                            energiaUnidadRestante = energiaUnidadRestante.Substring(ConstantesPrimasRER.numero4000);

                            do
                            {
                                if (energiaUnidadRestante.Length > ConstantesPrimasRER.numero4000)
                                {
                                    listEnergiaUnidadRestante.Add(energiaUnidadRestante.Substring(0, ConstantesPrimasRER.numero4000));
                                    energiaUnidadRestante = energiaUnidadRestante.Substring(ConstantesPrimasRER.numero4000);
                                }
                                else
                                {
                                    listEnergiaUnidadRestante.Add(energiaUnidadRestante);
                                    energiaUnidadRestante = "";
                                }
                            }
                            while (energiaUnidadRestante.Length > 0);
                        }

                        rerEnergiaUnidad.Rereucodi = rereucodi;
                        rerEnergiaUnidad.Rersedcodi = solicitudEdi.Rersedcodi;
                        rerEnergiaUnidad.Rereuusucreacion = usuario;
                        rerEnergiaUnidad.Rereufeccreacion = fechaActual;
                        FactoryTransferencia.GetRerEnergiaUnidadRepository().Save(rerEnergiaUnidad, conn, tran);
                        rereucodi++;

                        foreach (string energiaunidadRestante in listEnergiaUnidadRestante)
                        {
                            RerEnergiaUnidadDetDTO rerEnergiaUnidadDet = new RerEnergiaUnidadDetDTO
                            {
                                Rereudcodi = rereudcodi,
                                Rereucodi = rerEnergiaUnidad.Rereucodi,
                                Rereudenergiaunidad = energiaunidadRestante
                            };
                            FactoryTransferencia.GetRerEnergiaUnidadDetRepository().Save(rerEnergiaUnidadDet, conn, tran);
                            rereudcodi++;
                        }
                    }
                    #endregion
                }
                #endregion

                #region Actualizar archivo sustento
                if (rutaSustentoAnterior != String.Empty && System.IO.File.Exists(rutaSustentoAnterior))
                {
                    System.IO.File.Delete(rutaSustentoAnterior);
                }
                #endregion

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RER_SOLICITUDEDI
        /// </summary>
        /// <param name="rersedcodi">Código de la solicitud EDI</param>
        /// <param name="rersedusumodificacion">Nombre de usuario</param>
        public void EliminarSolicitudEDI(int rersedcodi, string rersedusumodificacion)
        {
            try
            {
                FactoryTransferencia.GetRerSolicitudEdiRepository().LogicalDelete(rersedcodi, rersedusumodificacion);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RER_SOLICITUDEDI
        /// </summary>
        /// <param name="rersedcodi">Código de la solicitud EDI</param>
        /// <returns>RerSolicitudEdiDTO</returns>
        public RerSolicitudEdiDTO BuscarSolicitudEDI(int rersedcodi)
        {
            return FactoryTransferencia.GetRerSolicitudEdiRepository().GetById(rersedcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RER_SOLICITUDEDI para mostrarlo en pop-up
        /// </summary>
        /// <param name="rersedcodi">Código de la solicitud EDI</param>
        /// <returns>RerSolicitudEdiDTO</returns>
        public RerSolicitudEdiDTO BuscarSolicitudEDIView(int rersedcodi)
        {
            return FactoryTransferencia.GetRerSolicitudEdiRepository().GetByIdView(rersedcodi);
        }

        /// <summary>
        /// Obtiene la fecha del plazo de entrega EDI
        /// </summary>
        /// <param name="indPeriodo">Objeto IndPeriodoDTO</param>
        /// <returns>Plazo de entrega EDI</returns>
        public DateTime? ObtenerPlazoEntregaEDI(IndPeriodoDTO indPeriodo)
        {
            ParametroAppServicio servicio = new ParametroAppServicio();
            int idParametro = int.Parse(ConfigurationManager.AppSettings[ConstantesPrimasRER.IdPlazoEntregaEDI]);
            decimal valorParametro = servicio.ObtenerValorParametro(idParametro, DateTime.Now);
            DateTime? plazoEntregaEDI = null;
            if (valorParametro > 0)
            {
                int anio = indPeriodo.Iperianio;
                int mes = indPeriodo.Iperimes + 1;
                if (mes > 12)
                {
                    mes = mes - 12;
                    anio++;
                }
                plazoEntregaEDI = new DateTime(anio, mes, (int)valorParametro, 23, 59, 59);
            }
            return plazoEntregaEDI;
        }

        /// <summary>
        /// Obtiene la fecha del plazo de cierre de Extranet
        /// </summary>
        /// <param name="ipericodi">Código del periodo</param>
        /// <param name="mensajeError">Errores al obtener la fecha</param>
        /// <returns>Plazo de Cierre de Extranet</returns>
        public DateTime? ObtenerPlazoCierreExtranet(int ipericodi, out string mensajeError)
        {
            mensajeError = string.Empty;
            List<RerRevisionDTO> listaRevisiones = ListarRevisiones(ipericodi).Where(rev => rev.Rerrevtipo == "M").ToList();
            DateTime? plazoCierreExtranet = null;
            if (listaRevisiones != null && listaRevisiones.Count > 0)
            {
                plazoCierreExtranet = listaRevisiones.First().Rerrevfecha;
                if (plazoCierreExtranet == null)
                {
                    mensajeError = "Lo sentimos, no se ha creado el Plazo de Cierre Extranet para el periodo";
                }
            }
            else
            {
                mensajeError = "Si desea crear nuevas solicitudes EDI para el 'Periodo Reporte EDI' seleccionado. Debe existir, una revisión de tipo 'Mensual' para dicho periodo.";
            }
            return plazoCierreExtranet;
        }

        #endregion

        #region Métodos de la tabla RERORIGEN
        /// <summary>
        /// Permite listar información del procedimiento PR-38.
        /// </summary>
        /// <returns>Lista de RerOrigenDTO</returns>
        public List<RerOrigenDTO> ListarOrigen()
        {
            return FactoryTransferencia.GetRerOrigenRepository().List();
        }


        #endregion

        #region Métodos de la tabla RER_ENERGIAUNIDAD
        /// <summary>
        /// Permite listar las energías por unidad de la solicitud EDI.
        /// </summary>
        /// <param name="rersedcodi">Código de la solicitud EDI</param>
        /// <param name="recuperarFechaIntervalos">Indica si se debe completar las fechas de intervalos</param>
        /// <returns>Lista de RerEnergiaUnidadDTO</returns>
        public List<RerEnergiaUnidadDTO> BuscarEnergiaUnidadPorSolicitud(int rersedcodi, bool recuperarFechaIntervalos = false)
        {
            List<RerEnergiaUnidadDTO> listaEnergiaUnidad = FactoryTransferencia.GetRerEnergiaUnidadRepository().List(rersedcodi);

            foreach(RerEnergiaUnidadDTO rerEnergiaUnidad in listaEnergiaUnidad)
            {
                List<RerEnergiaUnidadDetDTO> listEnergiaUnidadDet = FactoryTransferencia.GetRerEnergiaUnidadDetRepository().GetByCriteria(rerEnergiaUnidad.Rereucodi.ToString());
                if (listEnergiaUnidadDet != null && listEnergiaUnidadDet.Count > 0)
                {
                    listEnergiaUnidadDet = listEnergiaUnidadDet.OrderBy(x => x.Rereudcodi).ToList();
                    rerEnergiaUnidad.Rereuenergiaunidad += String.Join("", listEnergiaUnidadDet.Select(x => x.Rereudenergiaunidad).ToArray());
                }
            }

            if (recuperarFechaIntervalos)
            {
                RerSolicitudEdiDTO solicitudEDI = BuscarSolicitudEDI(rersedcodi);
                foreach (var objeto in listaEnergiaUnidad)
                {
                    objeto.Intervaloinicio = solicitudEDI.Rersedfechahorainicio;
                    objeto.Intervalofin = solicitudEDI.Rersedfechahorafin;
                }
            }
            return listaEnergiaUnidad;
        }
        #endregion

        #region Métodos para exportar archivo Excel (Energía Unidad)
        /// <summary>
        /// Permite obtener un listado con las fechas en intervalos de x minutos
        /// </summary>
        /// <param name="inicio">Fecha de inicio</param>
        /// <param name="fin">Fecha de fin</param>
        /// <param name="intervaloMin">Minutos por intervalo</param>
        /// <returns>Lista de DateTime</returns>
        private List<DateTime> ObtenerIntervalosMinuto(DateTime inicio, DateTime fin, int intervaloMin = 15)
        {
            List<DateTime> listaIntervalos = new List<DateTime>();

            int numInicio = inicio.Minute;
            int residuoInicio = numInicio % intervaloMin;
            if (residuoInicio > 0)
            {
                inicio = inicio.AddMinutes(-residuoInicio);
            }

            int numFin = fin.Minute;
            int residuoFin = numFin % intervaloMin;
            if (residuoFin > 0)
            {
                int faltante = intervaloMin - residuoFin;
                if (faltante > 0)
                {
                    fin = fin.AddMinutes(faltante);
                }
            }

            while (inicio <= fin)
            {
                listaIntervalos.Add(inicio);
                inicio = inicio.AddMinutes(intervaloMin);
            }

            return listaIntervalos;
        }

        /// <summary>
        /// Permite generar un archivo excel para ingresar la energia (MWh) de cada unidad de una central
        /// </summary>
        /// <param name="rersedcodi">Código de la solicitud EDI</param>
        /// <param name="rercencodi">Código de la central RER</param>
        /// <param name="rersedfechahorainicio">Fecha y hora de inicio</param>
        /// <param name="rersedfechahorafin">Fecha y hora de fin</param>
        /// <returns>Lista de RerExcelHoja</returns>
        public List<RerExcelHoja> GenerarExcelEnergiaUnidad(int rersedcodi, int rercencodi, DateTime rersedfechahorainicio, DateTime rersedfechahorafin)
        {
            if (rersedfechahorainicio >= rersedfechahorafin)
            {
                throw new ArgumentException("No se pudo generar el archivo excel, la fecha y hora de inicio debe ser menor a la fecha y hora de fin");
            }

            #region Cabecera
            CentralGeneracionAppServicio centralServicio = new CentralGeneracionAppServicio();
            RerCentralDTO rerCentral = this.GetByIdRerCentral(rercencodi);
            List<int> numerosFiltro = new List<int> { 2, 3, 36, 38 };
            List<CentralGeneracionDTO> listaUnidadesCentral = centralServicio.ListUnidadCentral(rerCentral.Equicodi).Where(n => numerosFiltro.Contains(n.FamCodi)).ToList();
            if (listaUnidadesCentral.Count < 1)
            {
                throw new ArgumentException("No se pudo generar el archivo excel, la central no tiene unidades");
            }
            List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[1];
            List<int> listaAnchoColumna = new List<int>();
            listaCabeceras[0] = new List<RerExcelModelo>
            {
                CrearExcelModelo("Periodo de inicio"),
                CrearExcelModelo("Periodo final")
            };
            listaAnchoColumna.Add(30);
            listaAnchoColumna.Add(30);
            for (int i = 0; i < listaUnidadesCentral.Count; i++)
            {
                listaCabeceras[0].Add(CrearExcelModelo("Energía MWh " + listaUnidadesCentral[i].CentGeneNombre));
                listaAnchoColumna.Add(16);
            }
            #endregion
            #region Cuerpo
            RerExcelCuerpo cuerpo = null;
            List<string>[] tablaCuerpo;
            List<string> listaAlineaHorizontal = new List<string>();
            List<string> listaTipo = new List<string>();
            if (rersedcodi == 0)
            {
                #region Solicitud nueva
                List<DateTime> listaIntervalos = ObtenerIntervalosMinuto(rersedfechahorainicio, rersedfechahorafin);
                if (listaIntervalos != null && listaIntervalos.Count > 0)
                {
                    tablaCuerpo = new List<string>[listaIntervalos.Count - 1];
                    for (int i = 0; i < listaIntervalos.Count - 1; i++)
                    {
                        DateTime intervalo = listaIntervalos[i];
                        List<string> tFila = new List<string>();
                        if (i == 0)
                        {
                            listaAlineaHorizontal.Add("left");
                            listaAlineaHorizontal.Add("left");
                            listaTipo.Add("string");
                            listaTipo.Add("string");
                        }
                        tFila.Add(intervalo.ToString(ConstantesPrimasRER.FormatoFechaHora));
                        tFila.Add(intervalo.AddMinutes(15).ToString(ConstantesPrimasRER.FormatoFechaHora));
                        //agregar valor en blanco para la energía MWh para cada unidad
                        for (int j = 0; j < listaUnidadesCentral.Count; j++)
                        {
                            if (i == 0)
                            {
                                listaAlineaHorizontal.Add("right");
                                listaTipo.Add("double");
                            }
                            tFila.Add("");
                        }
                        tablaCuerpo[i] = tFila;
                    }
                    cuerpo = CrearExcelCuerpo(tablaCuerpo, listaAlineaHorizontal, listaTipo);
                }
                #endregion
            }
            else
            {
                #region Solicitud existente
                RerSolicitudEdiDTO solicitudEDI = BuscarSolicitudEDI(rersedcodi);
                if (rersedfechahorainicio == solicitudEDI.Rersedfechahorainicio && rersedfechahorafin == solicitudEDI.Rersedfechahorafin)
                {
                    //Se crea un diccionario donde se guardará el código de la unidad y sus energías en intervalos de 15 min
                    Dictionary<int, string[]> listaEnergiaPorUnidad = new Dictionary<int, string[]>();

                    //Se recupera las energías por unidad en intervalos de 15 min de la solicitud en BD
                    List<RerEnergiaUnidadDTO> listaEnergiaUnidades = BuscarEnergiaUnidadPorSolicitud(rersedcodi);

                    if (listaEnergiaUnidades != null && listaEnergiaUnidades.Count > 0)
                    {
                        foreach (RerEnergiaUnidadDTO energia in listaEnergiaUnidades)
                        {
                            //Se llena el diccionario con el código de la unidad y sus energías en intervalos de 15 min
                            listaEnergiaPorUnidad.Add(energia.Equicodi, energia.Rereuenergiaunidad.Split(ConstantesPrimasRER.EnergiaUnidadDelimitador.ToCharArray()));
                        }
                    }

                    List<DateTime> listaIntervalos = ObtenerIntervalosMinuto(rersedfechahorainicio, rersedfechahorafin);
                    if (listaIntervalos != null && listaIntervalos.Count > 0)
                    {
                        tablaCuerpo = new List<string>[listaIntervalos.Count - 1];
                        for (int i = 0; i < listaIntervalos.Count - 1; i++)
                        {
                            DateTime intervalo = listaIntervalos[i];
                            List<string> tFila = new List<string>();
                            if (i == 0)
                            {
                                listaAlineaHorizontal.Add("left");
                                listaAlineaHorizontal.Add("left");
                                listaTipo.Add("string");
                                listaTipo.Add("string");
                            }
                            tFila.Add(intervalo.ToString(ConstantesPrimasRER.FormatoFechaHora));
                            tFila.Add(intervalo.AddMinutes(15).ToString(ConstantesPrimasRER.FormatoFechaHora));
                            //agregar valor en blanco para la energía MWh para cada unidad
                            for (int j = 0; j < listaUnidadesCentral.Count; j++)
                            {
                                if (i == 0)
                                {
                                    listaAlineaHorizontal.Add("right");
                                    listaTipo.Add("double");
                                }
                                //obtengo el código de la unidad según la columna
                                int codUnidad = listaUnidadesCentral[j].CentGeneCodi; //EQUICODI

                                //Si el diccionario tiene el código de la unidad
                                if (listaEnergiaPorUnidad.ContainsKey(codUnidad))
                                {
                                    //recupero la lista de energias en intervalo de 15 min
                                    string[] energiaIntervalos = listaEnergiaPorUnidad[codUnidad];
                                    if (energiaIntervalos.Length > i)
                                    {
                                        string valorEnergia = energiaIntervalos[i];
                                        tFila.Add(valorEnergia);
                                    }
                                    else
                                    {
                                        tFila.Add("0");
                                    }
                                }
                                else
                                {
                                    tFila.Add("");
                                }
                            }
                            tablaCuerpo[i] = tFila;
                        }
                        cuerpo = CrearExcelCuerpo(tablaCuerpo, listaAlineaHorizontal, listaTipo);
                    }
                }
                #endregion
            }
            #endregion

            List<RerExcelHoja> listaHojas = new List<RerExcelHoja>();
            RerExcelHoja excelHoja = new RerExcelHoja
            {
                NombreHoja = "Valor de EDI",
                ListaAnchoColumna = listaAnchoColumna,
                ListaCabeceras = listaCabeceras,
                Cuerpo = cuerpo
            };
            listaHojas.Add(excelHoja);

            return listaHojas;
        }
        #endregion

        #region Métodos para la lectura de archivo Excel
        /// <summary>
        /// Valida que los intervalos del archivo excel sean correctos
        /// </summary>
        /// <param name="dataSet">DataSet con datos del excel</param>
        /// <param name="fechaHoraInicio">Fecha y hora de inicio</param>
        /// <param name="fechaHoraFin">Fecha y hora de fin</param>
        /// <param name="cantIntervalos">Cantidad de intervalos</param>
        /// <returns>Boleano que indica si los intervalos del excel son correctos</returns>
        public bool ValidarIntervalosEnergiaUnidadExcel(DataSet dataSet, DateTime fechaHoraInicio, DateTime fechaHoraFin, out int cantIntervalos)
        {
            //Lista de intervalos que debe tener el excel
            List<DateTime> listaIntervalos = ObtenerIntervalosMinuto(fechaHoraInicio, fechaHoraFin);
            cantIntervalos = listaIntervalos.Count - 1;

            //Lista donde se guarda todas las fechas de los intervalos del excel,
            //luego se compara con listaIntervalos para validar que esten todos los intervalos.
            List<DateTime> listaIntervalosExcel = new List<DateTime>();

            //fecha de inicio que debería tener el primer intervalo del excel
            DateTime intervaloInicio = listaIntervalos.First();

            int i = 1;
            foreach (DataRow dtRow in dataSet.Tables[0].Rows)
            {
                string sIntervaloInicioExcel = dtRow[0].ToString();
                string sIntervaloFinalExcel = dtRow[1].ToString();
                if (sIntervaloInicioExcel != "null" && sIntervaloFinalExcel != "null")
                {
                    i++;
                    DateTime intervaloInicioExcel;
                    DateTime intervaloFinalExcel;

                    try
                    {
                        intervaloInicioExcel = UtilPrimasRER.ConstruirDateTime(sIntervaloInicioExcel, ConstantesPrimasRER.FormatoFechaHora);
                    }
                    catch (Exception)
                    {
                        string errorMessage = "El dato de la fila " + i + " de la columna 'Periodo de inicio' del archivo Excel, no tiene el formato correcto. Descargue el archivo Excel nuevamente y vuelva a intentarlo.";
                        throw new ArgumentException(errorMessage);
                    }

                    try
                    {
                        intervaloFinalExcel = UtilPrimasRER.ConstruirDateTime(sIntervaloFinalExcel, ConstantesPrimasRER.FormatoFechaHora);
                    }
                    catch (Exception)
                    {
                        string errorMessage = "El dato de la fila " + i + " de la columna 'Periodo final' del archivo Excel, no tiene el formato correcto. Descargue el archivo Excel nuevamente y vuelva a intentarlo.";
                        throw new ArgumentException(errorMessage);
                    }

                    //Agrego las fechas de los intervalos a la lista
                    listaIntervalosExcel.Add(intervaloInicioExcel);
                    listaIntervalosExcel.Add(intervaloFinalExcel);

                    //valida que los intervalos de fechas sean correctos
                    if (intervaloInicioExcel == intervaloInicio && intervaloFinalExcel == intervaloInicio.AddMinutes(15))
                    {
                        intervaloInicio = intervaloInicio.AddMinutes(15);
                    }
                    else
                    {
                        //Si los intervalos no son correctos (repiten intervalo en excel)
                        //se agrega una fecha errada para que falle al comparar los valores
                        intervaloInicio = intervaloInicio.AddMinutes(1);
                        listaIntervalosExcel.Add(intervaloInicio);
                        break;
                    }
                }
            }

            // Verificar que los intervalos sean correctos
            // No se considera el orden ni los repetidos, solo valida que las 2 listas tengan los mismos valores
            var list1 = listaIntervalos.Except(listaIntervalosExcel).ToList();
            var list2 = listaIntervalosExcel.Except(listaIntervalos).ToList();
            bool intervalosCorrectos = (list1.Count == 0 && list2.Count == 0);

            return intervalosCorrectos;
        }

        /// <summary>
        /// Genera Lista del objeto RerEnergiaUnidadDTO según los datos del excel
        /// </summary>
        /// <param name="dataSet">DataSet con datos del excel</param>
        /// <param name="rercencodi">Código de la central RER</param>
        /// <param name="fechaHoraInicio">Fecha y hora de inicio</param>
        /// <param name="fechaHoraFin">Fecha y hora de fin</param>
        /// <param name="cantIntervalos">Cantidad de intervalos</param>
        /// <returns>Lista de RerEnergiaUnidadDTO</returns>
        public List<RerEnergiaUnidadDTO> GenerarListaEnergiaUnidadExcel(DataSet dataSet, int rercencodi, DateTime fechaHoraInicio, DateTime fechaHoraFin, int cantIntervalos)
        {
            Dictionary<int, string[]> listaEnergiaPorUnidad = new Dictionary<int, string[]>();
            //Obtener las unidades de la central
            RerCentralDTO rerCentral = GetByIdRerCentral(rercencodi);
            CentralGeneracionAppServicio centralServicio = new CentralGeneracionAppServicio();
            List<int> numerosFiltro = new List<int> { 2, 3, 36, 38 };
            List<CentralGeneracionDTO> listaUnidadesCentral = centralServicio.ListUnidadCentral(rerCentral.Equicodi).Where(n => numerosFiltro.Contains(n.FamCodi)).ToList();
            List<int> codigoUnidades = new List<int>();

            if (listaUnidadesCentral != null && listaUnidadesCentral.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    if (i >= 2)
                    {
                        //Obtengo el nombre de la unidad del excel y busco si la central tiene esa unidad
                        string nombreUnidad = dataSet.Tables[0].Columns[i].ToString().Replace("Energía MWh ", "");
                        List<CentralGeneracionDTO> listaUC = listaUnidadesCentral.Where(c => c.CentGeneNombre == nombreUnidad).ToList();

                        if (listaUC != null && listaUC.Count > 0)
                        {
                            CentralGeneracionDTO unidad = listaUC.First();
                            codigoUnidades.Add(unidad.CentGeneCodi);
                        }
                        else
                        {
                            codigoUnidades.Add(0);
                        }
                    }
                }

                int contFila = 1;
                foreach (DataRow dtRow in dataSet.Tables[0].Rows)
                {
                    //valor inicial se coloca 2, para no considerar las 2 primeras columnas del excel
                    for (int c = 2; c < dtRow.ItemArray.Length; c++)
                    {
                        string sEnergiaMWh = dtRow[c].ToString();
                        string realEnergiaMWh = string.Empty;
                        realEnergiaMWh = sEnergiaMWh.Replace(",", "");
                        int sUnidadCodi = codigoUnidades[c - 2];

                        // Verificar si el codigo de la unidad existe en el diccionario
                        if (listaEnergiaPorUnidad.ContainsKey(sUnidadCodi))
                        {
                            // Obtener el array asociado a la unidad
                            string[] array = listaEnergiaPorUnidad[sUnidadCodi];
                            // Redimensionar el array para tener espacio para un elemento adicional
                            Array.Resize(ref array, array.Length + 1);
                            // Agregar el nuevo valor al final del array
                            array[array.Length - 1] = realEnergiaMWh;
                            // Actualizar el array asociado al código de la unidad en el diccionario
                            listaEnergiaPorUnidad[sUnidadCodi] = array;
                        }
                        else
                        {
                            // Si el codigo de la unidad no existe, crear un nuevo array con un solo elemento
                            string[] nuevoArray = { realEnergiaMWh };
                            // Agregar el código de la unidad y el nuevo array al diccionario
                            listaEnergiaPorUnidad.Add(sUnidadCodi, nuevoArray);
                        }
                    }
                    if (contFila == cantIntervalos) { break; }
                    contFila++;
                }
            }

            List<RerEnergiaUnidadDTO> listaEnergiaUnidad = new List<RerEnergiaUnidadDTO>();
            foreach (KeyValuePair<int, string[]> pair in listaEnergiaPorUnidad)
            {
                int unidadCodi = pair.Key;
                string[] aEnergiaUnidad = pair.Value;
                decimal sumaEnergiaUnidad = 0;

                foreach (string valor in aEnergiaUnidad)
                {
                    if (UtilPrimasRER.ValidarCandenaNumero(valor))
                    {
                        sumaEnergiaUnidad += UtilPrimasRER.ValidarNumero(valor);
                    }
                }

                RerEnergiaUnidadDTO energiaUnidadDTO = new RerEnergiaUnidadDTO();
                energiaUnidadDTO.Rereuenergiaunidad = string.Join(", ", aEnergiaUnidad);
                energiaUnidadDTO.Rereutotenergia = sumaEnergiaUnidad;
                energiaUnidadDTO.Equicodi = unidadCodi;
                if (unidadCodi != 0) energiaUnidadDTO.Equinombre = listaUnidadesCentral.Where(c => c.CentGeneCodi == unidadCodi).First().CentGeneNombre;

                //Fecha y hora de inicio y fecha de hora fin con la que se procesó el excel
                energiaUnidadDTO.Intervaloinicio = fechaHoraInicio;
                energiaUnidadDTO.Intervalofin = fechaHoraFin;

                energiaUnidadDTO.Centralcodi = rercencodi;
                listaEnergiaUnidad.Add(energiaUnidadDTO);
            }
            return listaEnergiaUnidad;
        }

        /// <summary>
        /// Valida si las unidades le pertenecen a la central
        /// </summary>
        /// <param name="listaUnidadCodi">Lista con ids de unidades </param>
        /// <param name="centralCodi">Código de la central</param>
        /// <returns>Boleano que indica si las unidades le pertenecen a la central</returns>
        public bool ValidaListaUnidadCodi(List<int> listaUnidadCodi, int centralCodi)
        {
            bool contieneCero = listaUnidadCodi.Any(x => x == 0);
            bool esValido = false;

            if (!contieneCero)
            {
                //Obtener las unidades de la central
                CentralGeneracionAppServicio centralServicio = new CentralGeneracionAppServicio();
                List<int> numerosFiltro = new List<int> { 2, 3, 36, 38 };
                List<CentralGeneracionDTO> listaUnidadesCentral = centralServicio.ListUnidadCentral(centralCodi).Where(n => numerosFiltro.Contains(n.FamCodi)).ToList();
                //Obtener los codigos de las unidades
                List<int> listaUnidadCodiCentral = listaUnidadesCentral.Select(e => e.CentGeneCodi).ToList();
                //Valida que las unidades sean iguales
                esValido = UtilPrimasRER.SonListasIguales(listaUnidadCodi, listaUnidadCodiCentral);
            }

            return esValido;
        }

        /// <summary>
        /// Valida los valores de la energía por unidad
        /// </summary>
        /// <param name="listaEnergiaUnidad">Lista RerEnergiaUnidadDTO</param>
        /// <param name="ipericodi">Código del periodo</param>
        /// <param name="rercencodi">Código de la central</param>
        /// <param name="fechaHoraInicioFormulario">Fecha y hora de inicio</param>
        /// <param name="fechaHoraFinFormulario">Fecha y hora de fin</param>
        /// <param name="RegError">Cantidad de filas con error</param>
        /// <param name="MensajeError">Mensaje con errores</param>
        /// <returns>Boleano que indica si la Lista RerEnergiaUnidadDTO es válido</returns>
        public bool ValidaListaEnergiaUnidad(List<RerEnergiaUnidadDTO> listaEnergiaUnidad, int ipericodi, int rercencodi, DateTime fechaHoraInicioFormulario, DateTime fechaHoraFinFormulario, out int RegError, out StringBuilder MensajeError)
        {
            #region Variables globales
            int iInicioFila = 2; //fila donde comienzan los datos del excel
            int maxErrores = 20;
            RegError = 0;
            //Lista el numero de filas con error
            List<int> listaRegError = new List<int>();
            //Filas con error
            Dictionary<int, string[]> erroresFila = new Dictionary<int, string[]>();
            MensajeError = new StringBuilder();
            #endregion

            #region Validaciones adicionales
            //Valida listaEnergiaUnidad diferente a null
            if (listaEnergiaUnidad.Count < 1)
            {
                RegError = 1;
                MensajeError.Append("<br> Debe importar un archivo Excel con el valor de EDI.");
                return false;
            }

            //Valida unidades de la central
            List<int> listaUnidadCodi = listaEnergiaUnidad.Select(e => e.Equicodi).ToList();
            RerCentralDTO rerCentral = GetByIdRerCentral(rercencodi);
            if (!ValidaListaUnidadCodi(listaUnidadCodi, rerCentral.Equicodi))
            {
                RegError = 1;
                MensajeError.Append("<br> Los nombres y/o la cantidad de las unidades de generación del archivo Excel, no coinciden con las unidades de generación de la Central RER.");
                return false;
            }
            #endregion

            EqEquipoDTO oEquipoCentral = FactorySic.GetEqEquipoRepository().GetById(rerCentral.Equicodi);

            foreach (RerEnergiaUnidadDTO energiaUnidad in listaEnergiaUnidad)
            {
                EqEquipoDTO oEquipoUnidadGeneradora = FactorySic.GetEqEquipoRepository().GetById(energiaUnidad.Equicodi);
                decimal potenciaEfectivaCentral = CalcularPotenciaEfectivaCentral(oEquipoCentral, oEquipoUnidadGeneradora, out string mensajeError, ipericodi);
                if (potenciaEfectivaCentral == -1)
                {
                    RegError = -1;
                    MensajeError.Append(mensajeError);
                    return false;
                }

                //Valida fechas, deben coincidir con las fechas ingresadas en formulario
                if (energiaUnidad.Intervaloinicio != fechaHoraInicioFormulario || energiaUnidad.Intervalofin != fechaHoraFinFormulario)
                {
                    RegError = 1;
                    MensajeError.Append("<br> Los intervalos de fecha ingresados en el formulario no coinciden con el archivo Excel procesado. Descargue el archivo Excel nuevamente y vuelva a intentarlo.");
                    return false;
                }
                else
                {
                    string[] listaEnergia = energiaUnidad.Rereuenergiaunidad.Split(ConstantesPrimasRER.EnergiaUnidadDelimitador.ToCharArray());
                    decimal valorLimite = (potenciaEfectivaCentral * 2) / 4;

                    for (int i = 0; i < listaEnergia.Length; i++)
                    {
                        string energia = listaEnergia[i].Trim();
                        decimal energiaMWh;
                        string mensajeErrorCelda = "";

                        if (energia != "" && energia != "null")
                        {
                            if (UtilPrimasRER.ValidarCandenaNumero(energia))
                            {
                                energiaMWh = UtilPrimasRER.ValidarNumero(energia);
                                if (energiaMWh > valorLimite)
                                {
                                    mensajeErrorCelda = "Error en la magnitud, no puede superar el valor de " + valorLimite;
                                }
                            }
                            else
                            {
                                mensajeErrorCelda = "Error en el formato, se espera un valor numérico, mayor o igual a 0.";
                            }
                        }
                        else
                        {
                            mensajeErrorCelda = "El valor es inválido, no puede estar en blanco.";
                        }

                        if (mensajeErrorCelda != "")
                        {
                            //Verificar si el código de la unidad existe en el diccionario
                            if (erroresFila.ContainsKey(i))
                            {
                                // Obtener el array asociado a la unidad
                                string[] array = erroresFila[i];
                                // Redimensionar el array para tener espacio para un elemento adicional
                                Array.Resize(ref array, array.Length + 1);
                                // Agregar el nuevo valor al final del array
                                array[array.Length - 1] = "<br>(Fila:" + (i + iInicioFila) + "-Col:\"" + energiaUnidad.Equinombre + "\"): " + mensajeErrorCelda;
                                // Actualizar el array asociado al código de la unidad en el diccionario
                                erroresFila[i] = array;
                            }
                            else
                            {
                                // Si el codigo de la unidad no existe, crear un nuevo array con un solo elemento
                                string[] nuevoArray = { "<br>(Fila:" + (i + iInicioFila) + "-Col:\"" + energiaUnidad.Equinombre + "\"): " + mensajeErrorCelda };
                                // Agregar el código de la unidad y el nuevo array al diccionario
                                erroresFila.Add(i, nuevoArray);
                            }
                            listaRegError.Add(i);
                        }

                        if (listaRegError.Distinct().ToList().Count > maxErrores)
                        {
                            RegError = -2;
                            SortedDictionary<int, string[]> erroresFilaOrdenado2 = new SortedDictionary<int, string[]>(erroresFila);
                            foreach (var kvp in erroresFilaOrdenado2)
                            {
                                string errores = string.Join("", kvp.Value);
                                MensajeError.Append(errores);
                            }
                            return false;
                        }

                    }
                }
            }
            RegError = listaRegError.Distinct().ToList().Count;

            SortedDictionary<int, string[]> erroresFilaOrdenado = new SortedDictionary<int, string[]>(erroresFila);
            foreach (var kvp in erroresFilaOrdenado)
            {
                string errores = string.Join("", kvp.Value);
                MensajeError.Append(errores);
            }

            return (RegError == 0);
        }

        /// <summary>
        /// Calcula la potencia efectiva de la Central y su Unidad Generadora respectiva
        /// </summary>
        /// <param name="oEquipoCentral">Objeto EqEquipoDTO</param>
        /// <param name="oEquipoUnidadGeneradora">Objeto EqEquipoDTO</param>
        /// <param name="mensajeError">Mensaje de error</param>
        /// <param name="ipericodi">Código del periodo</param>
        /// <returns>Potencia efectiva de la central</returns>
        private decimal CalcularPotenciaEfectivaCentral(EqEquipoDTO oEquipoCentral, EqEquipoDTO oEquipoUnidadGeneradora, out string mensajeError, int ipericodi = 0)
        {
            decimal potenciaEfectiva;
            mensajeError = "";

            switch (oEquipoCentral.Famcodi.Value)
            {
                case 5: //Termoeléctrica
                    INDAppServicio servicioIndisponibilidad = new INDAppServicio();
                    List<IndRecalculoDTO> listaRecalculo = servicioIndisponibilidad.GetByCriteriaIndRecalculos(ipericodi);
                    listaRecalculo = listaRecalculo.OrderBy(reca => reca.Irecafeccreacion).ToList();
                    if (listaRecalculo != null && listaRecalculo.Count > 0)
                    {
                        int cuadrocodi = 3;
                        //Obtener el último recálculo
                        int recalculocodi = listaRecalculo.Last().Irecacodi;
                        //Lista de versiones del último recálculo
                        List<IndReporteDTO> listaIndReportes = servicioIndisponibilidad.GetByCriteriaIndReportes(cuadrocodi, recalculocodi);
                        //Obtener la versión validada
                        listaIndReportes = listaIndReportes.Where(x => x.Irptesfinal == 1).ToList();

                        if (listaIndReportes != null && listaIndReportes.Count > 0)
                        {
                            IndReporteDTO regVersion = listaIndReportes.First();
                            IndCuadroDTO regCuadro = servicioIndisponibilidad.GetByIdIndCuadro(regVersion.Icuacodi);
                            List<IndReporteTotalDTO> listaReptot = servicioIndisponibilidad.GetByCriteriaIndReporteTotals(regVersion.Irptcodi).Where(r => r.Emprcodi == oEquipoCentral.Emprcodi && r.Equipadre == oEquipoCentral.Equicodi && (r.Equicodi == oEquipoUnidadGeneradora.Equicodi || r.Equicodi == oEquipoCentral.Equicodi)).ToList();
                            listaReptot = listaReptot.Where(x => regCuadro.ListaFamcodi.Contains(x.Famcodi) && x.Itotopcom == "S").ToList();

                            if (listaReptot != null && listaReptot.Count > 0)
                            {
                                potenciaEfectiva = UtilPrimasRER.ValidarNumero(listaReptot.First().ItotpeDesc);
                            }
                            else
                            {
                                potenciaEfectiva = -1;
                                mensajeError = "<br> No se encontró la potencia efectiva de la central " + oEquipoCentral.Equinomb + " y su unidad generadora " + oEquipoUnidadGeneradora.Equinomb;
                            }
                        }
                        else
                        {
                            potenciaEfectiva = -1;
                            mensajeError = "<br> El reporte de indisponibilidad no tiene una versión validada para el periodo seleccionado";
                        }
                    }
                    else
                    {
                        potenciaEfectiva = -1;
                        mensajeError = "<br> El reporte de indisponibilidad no tiene recálculo para el periodo seleccionado";
                    }
                    break;
                case 4: //Hidráulica
                case 37: //Solar
                case 39: //Eólica
                    int codPropiedad = (oEquipoCentral.Famcodi == 4) ? 46 : (oEquipoCentral.Famcodi == 37) ? 1710 : 1602;  // 46: Potencia efectiva, 1710: Potencia nominal
                    EquipamientoAppServicio equipamientoServicio = new EquipamientoAppServicio();
                    string valor = equipamientoServicio.ObtenerValorPropiedadEquipoFecha(codPropiedad, oEquipoCentral.Equicodi, DateTime.Now.ToString("dd/MM/yyyy"));

                    if (!string.IsNullOrEmpty(valor))
                    {
                        potenciaEfectiva = decimal.Parse(valor);
                    }
                    else
                    {
                        potenciaEfectiva = -1;
                        mensajeError = "<br> No se encontró el valor de la propiedad";
                    }
                    break;
                default:
                    potenciaEfectiva = -1;
                    mensajeError = "<br> La central debe ser HIDROELÉCTRICA, TERMOELÉCTRICA, SOLAR o EOLICA";
                    break;
            }
            return potenciaEfectiva;
        }

        #endregion

        #endregion

        #region Intranet

        #region Datos Generales

        #region CentralRER
        public int SaveRerCentral(RerCentralDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerCentralRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerCentral(RerCentralDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerCentral(int rercodicodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralRepository().Delete(rercodicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerCentralDTO GetByIdRerCentral(int rercodicodi)
        {
            return FactoryTransferencia.GetRerCentralRepository().GetById(rercodicodi);
        }

        public List<RerCentralDTO> ListRerCentrales()
        {
            return FactoryTransferencia.GetRerCentralRepository().List();
        }
        public List<RerCentralDTO> ListNombreCentralEmpresaBarra()
        {
            return FactoryTransferencia.GetRerCentralRepository().ListNombreCentralEmpresaBarra();
        }
        public List<RerCentralDTO> ListByFiltros(int equicodi, int emprcodi, int ptomedicodi, string fechaini, string fechafin, string estado, string codEntrega, int barrcodi)
        {
            return FactoryTransferencia.GetRerCentralRepository().ListByFiltros(equicodi, emprcodi, ptomedicodi, fechaini, fechafin, estado, codEntrega, barrcodi);
        }
        public List<RerCentralDTO> ListCentralREREmpresas()
        {
            return FactoryTransferencia.GetRerCentralRepository().ListCentralREREmpresas();
        }

        /// <summary>
        /// Permite listar centrales RER por empresa y rango de fechas del contrato.
        /// </summary>
        /// <param name="emprcodi">Código de empresa</param>
        /// <param name="periodo">periodo</param>
        /// <returns>Lista de RerCentralDTO</returns>
        public List<RerCentralDTO> ListarRerCentralPorEmpresaYContrato(int emprcodi, DateTime periodo)
        {
            //Centrales hidroeléctricas, termoeléctricas, solares, eólicas
            List<RerCentralDTO> listByEmprcodi = FactoryTransferencia.GetRerCentralRepository().ListByEmprcodi(emprcodi);
            listByEmprcodi = listByEmprcodi.Where(cen => cen.Rercenfechainicio <= periodo
                                                         && cen.Rercenfechafin >= periodo).ToList();
            return listByEmprcodi;
        }

        public List<RerCentralDTO> ListByEquiEmprFecha(int rercencodi, int equicodi, int emprcodi, string fechaini, string fechafin)
        {
            return FactoryTransferencia.GetRerCentralRepository().ListByEquiEmprFecha(rercencodi, equicodi, emprcodi, fechaini, fechafin);
        }

        //CU21
        /// <summary>
        /// Proceso que se encarga de Listar las Centrales [EQUICODI] que estan vigentes en un mes del Año Tarifario
        /// </summary>
        /// <param name="dRerCenFecha">Fecha de consulta</param>
        /// <returns>Solo Centrales: [EQUICODI y CODENTRCODI]</returns>
        public List<RerCentralDTO> ListCentralByFecha(DateTime dRerCenFecha)
        {
            return FactoryTransferencia.GetRerCentralRepository().ListCentralByFecha(dRerCenFecha);
        }

        /// <summary>
        /// Proceso que se encarga de Listar las Centrales LVTP [EQUICODI] que estan vigentes en un mes del Año Tarifario
        /// </summary>
        /// <param name="dRerCenFecha">Fecha de consulta</param>
        /// <returns>List<RerCentralDTO>: Solo Centrales: [EQUICODI y CODENTRCODI]</returns>
        public List<RerCentralDTO> ListCentralByFechaLVTP(DateTime dRerCenFecha)
        {
            return FactoryTransferencia.GetRerCentralRepository().ListCentralByFechaLVTP(dRerCenFecha);
        }

        #region RerCentralLvtp
        public void SaveRerCentralLvtp(RerCentralLvtpDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralLvtpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerCentralLvtp(RerCentralLvtpDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralLvtpRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerCentralLvtp(int rerclvtpcodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralLvtpRepository().Delete(rerclvtpcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerCentralLvtpDTO GetByIdRerCentralLvtp(int rerclvtpcodi)
        {
            return FactoryTransferencia.GetRerCentralLvtpRepository().GetById(rerclvtpcodi);
        }

        public List<RerCentralLvtpDTO> ListRerCentralLvtps()
        {
            return FactoryTransferencia.GetRerCentralLvtpRepository().List();
        }
        public List<RerCentralLvtpDTO> ListRerCentralLvtpsByRercencodi(int Rercencodi)
        {
            return FactoryTransferencia.GetRerCentralLvtpRepository().ListByRercencodi(Rercencodi);
        }
        public void DeleteAllLvtpByRercencodi(int Rercencodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralLvtpRepository().DeleteAllByRercencodi(Rercencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region RerCentralPmpoDTO
        public void SaveRerCentralPmpo(RerCentralPmpoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralPmpoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerCentralPmpo(RerCentralPmpoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralPmpoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerCentralPmpo(int rercpmpocodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralPmpoRepository().Delete(rercpmpocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerCentralPmpoDTO GetByIdRerCentralPmpo(int rercpmpocodi)
        {
            return FactoryTransferencia.GetRerCentralPmpoRepository().GetById(rercpmpocodi);
        }

        public List<RerCentralPmpoDTO> ListRerCentralPmpos()
        {
            return FactoryTransferencia.GetRerCentralPmpoRepository().List();
        }
        public List<RerCentralPmpoDTO> ListRerCentralPmposByRercencodi(int Rercencodi)
        {
            return FactoryTransferencia.GetRerCentralPmpoRepository().ListByRercencodi(Rercencodi);
        }
        public void DeleteAllPmpoByRercencodi(int Rercencodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralPmpoRepository().DeleteAllByRercencodi(Rercencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region ParametrosRER
        public void SaveRerParametroPrima(RerParametroPrimaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerParametroPrimaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerParametroPrima(RerParametroPrimaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerParametroPrimaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerParametroPrima(int rerpprimid)
        {
            try
            {
                FactoryTransferencia.GetRerParametroPrimaRepository().Delete(rerpprimid);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerParametroPrimaDTO GetByIdRerParametroPrima(int rerpprcodi)
        {
            return FactoryTransferencia.GetRerParametroPrimaRepository().GetById(rerpprcodi);
        }

        public List<RerParametroPrimaDTO> ListRerParametroPrimas()
        {
            return FactoryTransferencia.GetRerParametroPrimaRepository().List();
        }

        /// <summary>
        /// Proporciona la lista de meses de una Año/Versión
        /// </summary>
        /// <param name="sAnio">Año dentro de la descripción</param>
        /// <param name="iMes">Mes de año tarifrario</param>
        /// <returns></returns>
        public List<RerParametroPrimaDTO> GetByCriteriaRerParametroPrima(string sAnio, int iMes)
        {
            return FactoryTransferencia.GetRerParametroPrimaRepository().GetByCriteria(sAnio, iMes);
        }

        /// <summary>
        /// Proporciona la lista de meses de una Año/Versión
        /// </summary>
        /// <param name="reravcodi">Identificador de Año Versión</param>
        /// <returns></returns>
        public List<RerParametroPrimaDTO> GetParametroPrimaRerByAnioVersion(int reravcodi)
        {
            return FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersion(reravcodi);
        }

        /// <summary>
        /// Valida un registro de la tabla RER_SOLICITUDEDI, agrega el estado del envío de la solicitud EDI (FP o EP)
        /// </summary>
        /// <param name="anio">Anio del mes de un Anio Tarifario</param>
        /// <returns>Boleano que indica si la solicitud EDI es válida</returns>
        public List<string> obtenerMesesDescripcion(int anio)
        {

            #region Genero la descripción de los meses del Anio Tarifario
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;

            // Obtiene los nombres de los meses comenzando desde enero
            List<string> mesesAnioDescripcion = new List<string>();
            string mesDescripcion = "";
            string mesnombre = "";
            for (int i = 0; i < 12; i++)
            {
                mesnombre = ConstantesPrimasRER.mesesAnioTarifarioDesc[i];
                if (i >= 8)
                {
                    mesDescripcion = mesnombre + "." + Convert.ToString(anio + 1);
                }
                else {
                    mesDescripcion = mesnombre + "." + Convert.ToString(anio);
                }
                mesesAnioDescripcion.Add(mesDescripcion);
            }

            return mesesAnioDescripcion;
            #endregion
        }

        /// <summary>
        /// Proporciona la lista de meses de una Año/Versión a partir de Reravaniotarif
        /// </summary>
        /// <param name="Reravaniotarif">Anio del mes de un Anio Tarifario</param>
        /// <returns>List<RerParametroPrimaDTO></returns>
        public List<RerParametroPrimaDTO> listaParametroPrimaRerByAnio(int Reravaniotarif)
        {
            return FactoryTransferencia.GetRerParametroPrimaRepository().listaParametroPrimaRerByAnio(Reravaniotarif);
        }
        #endregion

        #region Anio Version (Anio Tarifario)

        /// <summary>
        /// Valida un registro de la tabla RER_SOLICITUDEDI, agrega el estado del envío de la solicitud EDI (FP o EP)
        /// </summary>
        /// <param name="ListaAniosVersion">lista de las versiones de un Anio Tarifario</param>
        /// <returns>Boleano que indica si la solicitud EDI es válida</returns>
        public bool ValidarInflaciones(List<RerAnioVersionDTO> ListaAniosVersion)
        {
            #region Definiendo variables globales
            List<string> listaErrores = new List<string>();
            bool esValido = true;
            string patron = @"^(\d{1,3})(\.\d{1,3})?$";
            #endregion

            try
            {
                #region Valida las infaciones de las versiones del Anio Tarifario
                foreach (RerAnioVersionDTO AnioVersion in ListaAniosVersion)
                {
                    if (AnioVersion.Reravinflacion == 0)
                    {
                        listaErrores.Add("El valor de la inflacion es un campo obligatorio");
                    }
                    if (!Regex.IsMatch(AnioVersion.Reravinflacion.ToString(), patron)) {
                        listaErrores.Add("El valor de la inflacion debe tener máximo 3 números enteros y 3 números decimales");
                    }
                }
                #endregion

                if (listaErrores != null && listaErrores.Count > 0)
                {
                    esValido = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                listaErrores.Add(ex.Message);
                esValido = false;
            }

            return esValido;
        }

        #endregion

        #region RerCentralCodRetiro
        public void SaveRerCentralCodRetiro(RerCentralCodRetiroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralCodRetiroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerCentralCodRetiro(RerCentralCodRetiroDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerCentralCodRetiroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerCentralCodRetiro(int rerccrcodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralCodRetiroRepository().Delete(rerccrcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteAllRerCentralCodRetiroByRerpprcodiRercencodi(int Rerpprcodi, int Rercencodi)
        {
            try
            {
                FactoryTransferencia.GetRerCentralCodRetiroRepository().DeleteAllByRerpprcodiRercencodi(Rerpprcodi, Rercencodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        public RerCentralCodRetiroDTO GetByIdRerCentralCodRetiro(int rerccrcodi)
        {
            return FactoryTransferencia.GetRerCentralCodRetiroRepository().GetById(rerccrcodi);
        }

        public List<RerCentralCodRetiroDTO> ListRerCentralCodRetiros()
        {
            return FactoryTransferencia.GetRerCentralCodRetiroRepository().List();
        }

        public List<RerCentralCodRetiroDTO> ListCantidadByRerpprcodi(int rerpprcodi)
        {
            return FactoryTransferencia.GetRerCentralCodRetiroRepository().ListCantidadByRerpprcodi(rerpprcodi);
        }

        //CU21
        /// <summary>
        /// Función que me da un string que contiene a todos los codigos de retiro
        /// </summary>
        /// <param name="iRerPPrCodi">Identificador del Parametro Prima</param>
        /// <param name="iEquiCodi">Identificador de una CENTRAL</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ListaCodigoRetiroByEquipo(int iRerPPrCodi, int iEquiCodi)
        {
            return FactoryTransferencia.GetRerCentralCodRetiroRepository().ListaCodigoRetiroByEquipo(iRerPPrCodi, iEquiCodi);
        }
        #endregion

        #region FactorPerdidas
        public int SaveRerFacPerMed(RerFacPerMedDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetRerFacPerMedRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        public void UpdateRerFacPerMed(RerFacPerMedDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerFacPerMedRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerFacPerMed(int rerfpmedid)
        {
            try
            {
                FactoryTransferencia.GetRerFacPerMedRepository().Delete(rerfpmedid);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerFacPerMedDTO GetByIdRerFacPerMed(int rerfpmedid)
        {
            return FactoryTransferencia.GetRerFacPerMedRepository().GetById(rerfpmedid);
        }

        public List<RerFacPerMedDTO> ListRerFacPerMeds()
        {
            return FactoryTransferencia.GetRerFacPerMedRepository().List();
        }

        #endregion

        #region Factor de perdidas medias detalle
        public void SaveRerFacPerMedDet(RerFacPerMedDetDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerFacPerMedDetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerFacPerMedDet(RerFacPerMedDetDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerFacPerMedDetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerFacPerMedDet(int rerfpmdetid)
        {
            try
            {
                FactoryTransferencia.GetRerFacPerMedDetRepository().Delete(rerfpmdetid);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerFacPerMedDetDTO GetByIdRerFacPerMedDet(int rerfpmdetid)
        {
            return FactoryTransferencia.GetRerFacPerMedDetRepository().GetById(rerfpmdetid);
        }

        public List<RerFacPerMedDetDTO> ListRerFacPerMedDets()
        {
            return FactoryTransferencia.GetRerFacPerMedDetRepository().List();
        }

        public List<RerFacPerMedDetDTO> ListRerFacPerMedDetsByFPM(int Rerfpmcodi)
        {
            return FactoryTransferencia.GetRerFacPerMedDetRepository().ListByFPM(Rerfpmcodi);
        }

        #endregion

        #endregion

        #region Cálculos EDI

        #region Periodos y Revisiones

        /// <summary>
        /// Obtener una revisión
        /// </summary>
        public RerRevisionDTO ObtenerRevision(int rerrevcodi)
        {
            try
            {
                RerRevisionDTO rerRevision = FactoryTransferencia.GetRerRevisionRepository().GetById(rerrevcodi);
                bool existeRevision = (rerRevision != null);
                if (existeRevision)
                {
                    FormatearRerRevision(rerRevision);
                }

                return rerRevision;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Crear una nueva revisión
        /// </summary>
        /// <param name="rerRevision"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public void CrearRevision(RerRevisionDTO rerRevision, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validar datos
                string mensajeError = ValidarRevision(ConstantesPrimasRER.operacionCrear, rerRevision, out RerRevisionDTO rerRevisionBD);
                bool existeError = !string.IsNullOrEmpty(mensajeError);
                if (existeError)
                {
                    throw new Exception(mensajeError);
                }
                #endregion

                #region Crear revision
                conn = FactoryTransferencia.GetRerRevisionRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerRevisionRepository().StartTransaction(conn);

                int rerrevcodi = FactoryTransferencia.GetRerRevisionRepository().GetMaxId();
                DateTime fecha = DateTime.Now;
                rerRevision.Rerrevcodi = rerrevcodi;    /**/
                rerRevision.Rerrevusucreacion = usuario;
                rerRevision.Rerrevfeccreacion = fecha;
                rerRevision.Rerrevusumodificacion = usuario;
                rerRevision.Rerrevfecmodificacion = fecha;
                FactoryTransferencia.GetRerRevisionRepository().Save(rerRevision, conn, tran);

                #region Copiar "Cuadro de Evaluación" de la Revisión anterior, en caso lo tenga 
                List<RerRevisionDTO> listRevision = FactoryTransferencia.GetRerRevisionRepository().GetByCriteria(rerRevision.Ipericodi).OrderByDescending(x => x.Rerrevcodi).ToList();
                bool existeListRevision = (listRevision != null && listRevision.Count > 0);
                if (existeListRevision)
                {
                    RerRevisionDTO lastRevision = listRevision[0];   //Obtener última revisión
                    List<RerEvaluacionDTO> listEvaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetByCriteria(lastRevision.Rerrevcodi).OrderByDescending(x => x.Rerevanumversion).ToList();
                    bool existeListEvaluacion = (listEvaluacion != null && listEvaluacion.Count > 0);
                    if (existeListEvaluacion)
                    {
                        #region Obtener datos con respecto a la última versión de la última revisión
                        RerEvaluacionDTO lastEvaluacion = listEvaluacion[0];  //Obtener última versión de la última revisión
                        List<RerEvaluacionSolicitudEdiDTO> listEvaluacionSolicitudEdi = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetByCriteria(lastEvaluacion.Rerevacodi, -1).OrderBy(x => x.Reresecodi).ToList();
                        List<RerEvaluacionEnergiaUnidadDTO> listEvaluacionEnergiaUnidad = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetByCriteria(ConstantesPrimasRER.numeroMenosUno, lastEvaluacion.Rerevacodi).OrderBy(x => x.Rereeucodi).ToList();
                        List<RerEvaluacionEnergiaUnidDetDTO> listEvaluacionEnergiaUnidadDet = FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().GetByCriteria(string.Join(",", listEvaluacionEnergiaUnidad.Select(x => x.Rereeucodi).ToArray())).ToList();
                        List<RerComparativoCabDTO> listComparativoCab = FactoryTransferencia.GetRerComparativoCabRepository().GetByCriteria(lastEvaluacion.Rerevacodi).OrderBy(x => x.Rerccbcodi).ToList();
                        List<RerComparativoDetDTO> listComparativoDet = FactoryTransferencia.GetRerComparativoDetRepository().GetByCriteria(lastEvaluacion.Rerevacodi, ConstantesPrimasRER.numeroMenosUno, ConstantesPrimasRER.numeroMenosUno).OrderBy(x => x.Rercdtcodi).ToList();
                        bool existeListEvaluacionSolicitudEdi = (listEvaluacionSolicitudEdi != null && listEvaluacionSolicitudEdi.Count > 0);
                        bool existeListEvaluacionEnergiaUnidad = (listEvaluacionEnergiaUnidad != null && listEvaluacionEnergiaUnidad.Count > 0);
                        bool existeListEvaluacionEnergiaUnidadDet = (listEvaluacionEnergiaUnidadDet != null && listEvaluacionEnergiaUnidadDet.Count > 0);
                        bool existeListComparativoCab = (listComparativoCab != null && listComparativoCab.Count > 0);
                        bool existeListComparativoDet = (listComparativoDet != null && listComparativoDet.Count > 0);
                        #endregion

                        #region Obtener Ids
                        int rerevacodi = FactoryTransferencia.GetRerEvaluacionRepository().GetMaxId();
                        int reresecodi = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetMaxId();
                        int rereeucodi = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetMaxId();
                        int rereedcodi = FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().GetMaxId();
                        int rerccbcodi = FactoryTransferencia.GetRerComparativoCabRepository().GetMaxId();
                        int rercdtcodi = FactoryTransferencia.GetRerComparativoDetRepository().GetMaxId();
                        #endregion

                        #region Crear "Cuadro de Evaluación"
                        DateTime fechaeva = DateTime.Now;
                        RerEvaluacionDTO evaluacion = new RerEvaluacionDTO()
                        {
                            Rerevacodi = rerevacodi,  /**/
                            Rerrevcodi = rerRevision.Rerrevcodi,  /**/
                            Rerevanumversion = ConstantesPrimasRER.numero1,
                            Rerevaestado = ConstantesPrimasRER.estadoGenerado,
                            Rerevausucreacion = usuario,
                            Rerevafeccreacion = fechaeva,
                            Rerevausumodificacion = usuario,
                            Rerevafecmodificacion = fechaeva
                        };
                        FactoryTransferencia.GetRerEvaluacionRepository().Save(evaluacion, conn, tran);
                        rerevacodi++;

                        #region Crear "Evaluación y Evaluación Energía Unidad de la Solicitud EDI" y "Comparativo Cabecera y Detalle de la Solicitud EDI" del "Cuadro de Evaluación" de la Revisión anterior
                        if (existeListEvaluacionSolicitudEdi)
                        {
                            foreach (var regESE in listEvaluacionSolicitudEdi)
                            {
                                DateTime fecharese = DateTime.Now;
                                RerEvaluacionSolicitudEdiDTO evaluacionSolicitudEdi = new RerEvaluacionSolicitudEdiDTO
                                {
                                    Reresecodi = reresecodi,  /**/
                                    Rerevacodi = evaluacion.Rerevacodi,    /**/
                                    Rersedcodi = regESE.Rersedcodi,
                                    Rercencodi = regESE.Rercencodi,
                                    Emprcodi = regESE.Emprcodi,
                                    Ipericodi = regESE.Ipericodi,
                                    Reroricodi = regESE.Reroricodi,
                                    Reresefechahorainicio = regESE.Reresefechahorainicio,
                                    Reresefechahorafin = regESE.Reresefechahorafin,
                                    Reresedesc = regESE.Reresedesc,
                                    Reresetotenergia = regESE.Reresetotenergia,
                                    Reresesustento = regESE.Reresesustento,
                                    Rereseestadodeenvio = regESE.Rereseestadodeenvio,
                                    Rereseeliminado = regESE.Rereseeliminado,
                                    Rereseusucreacionext = regESE.Rereseusucreacionext,
                                    Reresefeccreacionext = regESE.Reresefeccreacionext,
                                    Rereseusumodificacionext = regESE.Rereseusumodificacionext,
                                    Reresefecmodificacionext = regESE.Reresefecmodificacionext,
                                    Rereseusucreacion = usuario,
                                    Reresefeccreacion = fecharese,
                                    Rereseusumodificacion = usuario,
                                    Reresefecmodificacion = fecharese,
                                    Reresetotenergiaestimada = regESE.Reresetotenergiaestimada,
                                    Rereseediaprobada = regESE.Rereseediaprobada,
                                    Rereserfpmc = regESE.Rereserfpmc,
                                    Rereseresdesc = regESE.Rereseresdesc,
                                    Rereseresestado = regESE.Rereseresestado
                                };
                                FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().Save(evaluacionSolicitudEdi, conn, tran);
                                reresecodi++;

                                #region Crear "Evaluación Energía Unidad de la Solicitud EDI" del "Cuadro de Evaluación" de la Revisión anterior
                                if (existeListEvaluacionEnergiaUnidad)
                                {
                                    List<RerEvaluacionEnergiaUnidadDTO> listREEU = listEvaluacionEnergiaUnidad.Where(x => x.Reresecodi == regESE.Reresecodi && x.Rerevacodi == regESE.Rerevacodi).ToList();    /**/
                                    bool existeListREEUU = (listREEU != null && listREEU.Count > 0);
                                    if (existeListREEUU)
                                    {
                                        foreach (var regEEU in listREEU)
                                        {
                                            DateTime fechareeu = DateTime.Now;
                                            RerEvaluacionEnergiaUnidadDTO evaluacionEnergiaUnidad = new RerEvaluacionEnergiaUnidadDTO
                                            {
                                                Rereeucodi = rereeucodi, /**/
                                                Reresecodi = evaluacionSolicitudEdi.Reresecodi,   /**/
                                                Rerevacodi = evaluacionSolicitudEdi.Rerevacodi,   /**/
                                                Rereucodi = regEEU.Rereucodi,
                                                Rersedcodi = regEEU.Rersedcodi,
                                                Equicodi = regEEU.Equicodi,
                                                Rereeuenergiaunidad = regEEU.Rereeuenergiaunidad,
                                                Rereeutotenergia = regEEU.Rereeutotenergia,
                                                Rereeuusucreacionext = regEEU.Rereeuusucreacionext,
                                                Rereeufeccreacionext = regEEU.Rereeufeccreacionext,
                                                Rereeuusucreacion = usuario,
                                                Rereeufeccreacion = fechareeu
                                            };
                                            FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().Save(evaluacionEnergiaUnidad, conn, tran);
                                            rereeucodi++;

                                            if(existeListEvaluacionEnergiaUnidadDet)
                                            {
                                                List<RerEvaluacionEnergiaUnidDetDTO> listEEUDet = listEvaluacionEnergiaUnidadDet.Where(x => x.Rereeucodi == regEEU.Rereeucodi).OrderBy(x => x.Rereedcodi).ToList();
                                                if (listEEUDet != null && listEEUDet.Count > 0)
                                                {
                                                    foreach(var regEEUDet in listEEUDet)
                                                    {
                                                        RerEvaluacionEnergiaUnidDetDTO evaluacionEnergiaUnidadDet = new RerEvaluacionEnergiaUnidDetDTO
                                                        {
                                                            Rereedcodi = rereedcodi,
                                                            Rereeucodi = evaluacionEnergiaUnidad.Rereeucodi,
                                                            Rereedenergiaunidad = regEEUDet.Rereedenergiaunidad
                                                        };
                                                        FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().Save(evaluacionEnergiaUnidadDet, conn, tran);
                                                        rereedcodi++;
                                                    }
                                                }
                                            }

                                            #region Crear "Comparativo Cabecera y Detalle de la Solicitud EDI" del "Cuadro de Evaluación" de la Revisión anterior
                                            if (existeListComparativoCab)
                                            {
                                                List<RerComparativoCabDTO> listCC = listComparativoCab.Where(x => x.Rerevacodi == regEEU.Rerevacodi && x.Reresecodi == regEEU.Reresecodi && x.Rereeucodi == regEEU.Rereeucodi).ToList();    /**/
                                                bool existeListCC = (listCC != null && listCC.Count > 0);
                                                if (existeListCC)
                                                {
                                                    foreach (var regCC in listCC)
                                                    {
                                                        DateTime fecharcc = DateTime.Now;
                                                        RerComparativoCabDTO comparativoCab = new RerComparativoCabDTO
                                                        {
                                                            Rerccbcodi = rerccbcodi,    /**/
                                                            Rerevacodi = evaluacionEnergiaUnidad.Rerevacodi,    /**/
                                                            Reresecodi = evaluacionEnergiaUnidad.Reresecodi,  /**/
                                                            Rereeucodi = evaluacionEnergiaUnidad.Rereeucodi, /**/
                                                            Rerccboridatos = regCC.Rerccboridatos,
                                                            Rerccbtotenesolicitada = regCC.Rerccbtotenesolicitada,
                                                            Rerccbtoteneestimada = regCC.Rerccbtoteneestimada,
                                                            Rerccbusucreacion = usuario,
                                                            Rerccbfeccreacion = fecharcc,
                                                            Rerccbusumodificacion = usuario,
                                                            Rerccbfecmodificacion = fecharcc
                                                        };
                                                        FactoryTransferencia.GetRerComparativoCabRepository().Save(comparativoCab, conn, tran);
                                                        rerccbcodi++;

                                                        #region Crear "Comparativo Detalle de la Solicitud EDI" del "Cuadro de Evaluación" de la Revisión anterior
                                                        if (existeListComparativoDet)
                                                        {
                                                            List<RerComparativoDetDTO> listCD = listComparativoDet.Where(x => x.Rerccbcodi == regCC.Rerccbcodi && x.Rerevacodi == regCC.Rerevacodi && x.Reresecodi == regCC.Reresecodi).ToList();    /**/
                                                            bool existeListCD = (listCD != null && listCD.Count > 0);
                                                            if (existeListCD)
                                                            {
                                                                foreach (var regCD in listCD)
                                                                {
                                                                    DateTime fecharcd = DateTime.Now;
                                                                    RerComparativoDetDTO comparativoDet = new RerComparativoDetDTO
                                                                    {
                                                                        Rercdtcodi = rercdtcodi,
                                                                        Rerccbcodi = comparativoCab.Rerccbcodi,
                                                                        Rerevacodi = comparativoCab.Rerevacodi,
                                                                        Reresecodi = comparativoCab.Reresecodi,
                                                                        Rereeucodi = comparativoCab.Rereeucodi,
                                                                        Rercdtfecha = regCD.Rercdtfecha,
                                                                        Rercdthora = regCD.Rercdthora,
                                                                        Rercdtmedfpm = regCD.Rercdtmedfpm,
                                                                        Rercdtenesolicitada = regCD.Rercdtenesolicitada,
                                                                        Rercdteneestimada = regCD.Rercdteneestimada,
                                                                        Rercdtpordesviacion = regCD.Rercdtpordesviacion,
                                                                        Rercdtflag = regCD.Rercdtflag,
                                                                        Rercdtusucreacion = usuario,
                                                                        Rercdtfeccreacion = fecharcd,
                                                                        Rercdtusumodificacion = usuario,
                                                                        Rercdtfecmodificacion = fecharcd
                                                                    };
                                                                    FactoryTransferencia.GetRerComparativoDetRepository().Save(comparativoDet, conn, tran);
                                                                    rercdtcodi++;
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #endregion
                    }
                }
                #endregion

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Actualizar una nueva revisión
        /// </summary>
        public void ActualizarRevision(RerRevisionDTO rerRevision, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validar datos
                string mensajeError = ValidarRevision(ConstantesPrimasRER.operacionActualizar, rerRevision, out RerRevisionDTO rerRevisionBD);
                bool existeError = !string.IsNullOrEmpty(mensajeError);
                if (existeError)
                {
                    throw new Exception(mensajeError);
                }
                #endregion

                #region Actualización de datos

                #region Actualizar Revisión
                conn = FactoryTransferencia.GetRerRevisionRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerRevisionRepository().StartTransaction(conn);

                rerRevisionBD.Ipericodi = rerRevision.Ipericodi;
                rerRevisionBD.Rerrevtipo = rerRevision.Rerrevtipo;
                rerRevisionBD.Rerrevnombre = rerRevision.Rerrevnombre;
                rerRevisionBD.Rerrevestado = rerRevision.Rerrevestado;
                rerRevisionBD.Rerrevfecha = rerRevision.Rerrevfecha;
                rerRevisionBD.Rerrevusumodificacion = usuario;
                rerRevisionBD.Rerrevfecmodificacion = DateTime.Now;
                FactoryTransferencia.GetRerRevisionRepository().Update(rerRevisionBD, conn, tran);
                #endregion

                #region Actualizar Evaluación
                bool esEstadoAbierto = (rerRevision.Rerrevestado == ConstantesPrimasRER.estadoAbierto);
                if (esEstadoAbierto)
                {
                    FactoryTransferencia.GetRerEvaluacionRepository().UpdateEstadoAGenerado(rerRevisionBD.Rerrevcodi, usuario, conn, tran);
                }
                #endregion

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Valida los datos de una Revisión, según la operación que se desea realizar con esta.
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="rerRevision"></param>        
        /// <param name="rerRevisionBD">Revisión obtenida de la BD, para el caso que la operación sea Actualizar, es decir, buscará en BD si existe una revisión con el id de la revisión enviada. En caso, operación es igua a Crear, devolverá nulo</param>
        /// <returns>Retorna un mensaje de error, en caso no pase la validación</returns>
        private string ValidarRevision(string operacion, RerRevisionDTO rerRevision, out RerRevisionDTO rerRevisionBD)
        {
            try
            {
                rerRevisionBD = null;
                StringBuilder sb = new StringBuilder();

                #region Validar datos ingresados
                if (operacion == ConstantesPrimasRER.operacionActualizar && rerRevision.Rerrevcodi < 1)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Id Revisión es requerido");
                }
                if (rerRevision.Ipericodi < 1)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Periodo es requerido");
                }
                if (string.IsNullOrEmpty(rerRevision.Rerrevtipo))
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Tipo de revisión es requerido");
                }
                else
                {
                    if (!(rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoMensual || rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoRevision))
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("Tipo de revisión es inválido");
                    }
                }
                if (string.IsNullOrEmpty(rerRevision.Rerrevnombre))
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Nombre es requerido");
                }
                rerRevision.Rerrevnombre = rerRevision.Rerrevnombre.Trim();
                if (string.IsNullOrEmpty(rerRevision.Rerrevestado))
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Estado es requerido");
                }
                else
                {
                    if (operacion == ConstantesPrimasRER.operacionCrear && rerRevision.Rerrevestado != ConstantesPrimasRER.estadoAbierto)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("Estado debe ser abierto.");
                    }
                    else if (operacion == ConstantesPrimasRER.operacionActualizar && !(rerRevision.Rerrevestado == ConstantesPrimasRER.estadoAbierto || rerRevision.Rerrevestado == ConstantesPrimasRER.estadoCerrado))
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("Estado es inválido");
                    }
                }
                if (rerRevision.Rerrevfecha.Value == null)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Fecha es requerida");
                }
                #endregion

                #region Validar datos ingresados con lógica de negocio
                List<RerRevisionDTO> list = FactoryTransferencia.GetRerRevisionRepository().GetByCriteria(rerRevision.Ipericodi);
                if (operacion == ConstantesPrimasRER.operacionCrear)
                {
                    #region Validar que no exista otra revisión de tipo Mensual
                    bool validarTipoMensual = (list != null && list.Count > 0 && rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoMensual);
                    if (validarTipoMensual)
                    {
                        List<RerRevisionDTO> listMensual = list.Where(x => x.Rerrevtipo == ConstantesPrimasRER.tipoMensual).ToList();
                        bool existeListMensual = (listMensual != null && listMensual.Count > 0);
                        if (existeListMensual)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.Append("Ya existe una revisión de tipo 'Mensual'");
                        }
                    }
                    #endregion

                    #region Si se va a crear una revisión de tipo revisión = "Revisión", verificar que exista una revisión de tipo revisión = "Mensual" para dicho periodo
                    bool esTipoRevisionIgualRevision = (rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoRevision);
                    if (esTipoRevisionIgualRevision)
                    {
                        bool existeList = (list != null && list.Count > 0);
                        if (!existeList)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.Append("Debe crearse primero una revisión de tipo 'Mensual'");
                        }
                        else
                        {
                            List<RerRevisionDTO> listMensual = list.Where(x => x.Rerrevtipo == ConstantesPrimasRER.tipoMensual).ToList();
                            bool existeListMensual = (listMensual != null && listMensual.Count > 0);
                            if (!existeListMensual)
                            {
                                if (sb.ToString().Length > 0) { sb.Append(", "); }
                                sb.Append("Debió crearse primero una revisión de tipo 'Mensual'");
                            }
                        }
                    }
                    #endregion

                    #region Validar nombre para las revisiones del mismo tipo
                    bool validarNombre = (list != null && list.Count > 0);
                    if (validarNombre)
                    {
                        List<RerRevisionDTO> listNombre = list.Where(x => x.Rerrevtipo == rerRevision.Rerrevtipo && x.Rerrevnombre.ToUpper().Trim() == rerRevision.Rerrevnombre.ToUpper().Trim()).ToList();
                        bool existeListNombre = (listNombre != null && listNombre.Count > 0);
                        if (existeListNombre)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.Append("Ya existe una revisión con el mismo nombre para tipo revisión '");
                            sb.Append((rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoMensual) ? ConstantesPrimasRER.descTipoMensual : ConstantesPrimasRER.descTipoRevision);
                            sb.Append("'");
                        }
                    }
                    #endregion
                }
                else if (operacion == ConstantesPrimasRER.operacionActualizar)
                {
                    #region Validar que no exista otra revisión de tipo Mensual
                    bool validarTipoMensual = (list != null && list.Count > 0 && rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoMensual);
                    if (validarTipoMensual)
                    {
                        List<RerRevisionDTO> listMensual = list.Where(x => x.Rerrevcodi != rerRevision.Rerrevcodi && x.Rerrevtipo == ConstantesPrimasRER.tipoMensual).ToList();
                        bool existeListMensual = (listMensual != null && listMensual.Count > 0);
                        if (existeListMensual)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.Append("Ya existe una revisión de tipo 'Mensual'");
                        }
                    }
                    #endregion

                    #region Validar nombre para las revisiones del mismo tipo
                    bool validarNombre = (list != null && list.Count > 0);
                    if (validarNombre)
                    {
                        List<RerRevisionDTO> listNombre = list.Where(x => x.Rerrevcodi != rerRevision.Rerrevcodi && x.Rerrevtipo == rerRevision.Rerrevtipo && x.Rerrevnombre.ToUpper().Trim() == rerRevision.Rerrevnombre.ToUpper().Trim()).ToList();
                        bool existeListNombre = (listNombre != null && listNombre.Count > 0);
                        if (existeListNombre)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.Append("Ya existe una revisión con el mismo nombre para tipo de revisión '");
                            sb.Append((rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoMensual) ? ConstantesPrimasRER.descTipoMensual : ConstantesPrimasRER.descTipoRevision);
                            sb.Append("'");
                        }
                    }
                    #endregion

                    #region Verificar que exista la revisión en la BD
                    rerRevisionBD = ObtenerRevision(rerRevision.Rerrevcodi);
                    bool notExisteRerRevisionBD = (rerRevisionBD == null);
                    if (notExisteRerRevisionBD)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("No existe la revisión en la base de datos");
                    }
                    #endregion

                    #region Verificar que no cierre una revisión que esté abierta a través del Formulario de Revisión
                    bool esRevisionBDEstadoAbiertoYrerRevisionEstadoCerrado = (rerRevisionBD.Rerrevestado == ConstantesPrimasRER.estadoAbierto && rerRevision.Rerrevestado == ConstantesPrimasRER.estadoCerrado);
                    if (esRevisionBDEstadoAbiertoYrerRevisionEstadoCerrado)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.Append("Para guardar esta revisión con estado 'Cerrado' debe consultar esta revisión a través de la opción 'Resultados' y luego ejecutar el botón 'Validar y Cerrar'");
                    }
                    #endregion
                }
                #endregion

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las revisiones del periodo
        /// </summary>
        public List<RerRevisionDTO> ListarRevisiones(int ipericodi)
        {
            try
            {
                List<RerRevisionDTO> list = FactoryTransferencia.GetRerRevisionRepository().GetByCriteria(ipericodi);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    list = list.OrderByDescending(x => x.Rerrevcodi).ToList();
                    foreach (var reg in list)
                    {
                        FormatearRerRevision(reg);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la ultima revisión con estado cerrado del peridodo con id ipericodi
        /// </summary>
        public List<RerRevisionDTO> ObtenerUltimaRevisionCerrada(int ipericodi)
        {
            try
            {
                List<RerRevisionDTO> list = FactoryTransferencia.GetRerRevisionRepository().GetByCriteria(ipericodi);
                List<RerRevisionDTO> listCerrada = list.Where(item => item.Rerrevestado == "C").ToList();
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    list = list.OrderByDescending(x => x.Rerrevcodi).ToList();
                    foreach (var reg in list)
                    {
                        FormatearRerRevision(reg);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera Reporte html con el Listado de Periodos y sus últimas revisiones
        /// </summary>
        /// <param name="idPlazoEntregaEdi"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoPeriodos(int idPlazoEntregaEdi, string url)
        {
            try
            {
                List<RerRevisionDTO> list = FactoryTransferencia.GetRerRevisionRepository().ListPeriodosConUltimaRevision(idPlazoEntregaEdi);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    list = list.OrderByDescending(x => x.Ipericodi).ThenByDescending(x => x.Rerrevcodi).ToList();
                    foreach (var reg in list)
                    {
                        FormatearRerRevision(reg);
                    }
                }

                StringBuilder str = new StringBuilder();
                #region tabla
                str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_periodo'>");
                #region cabecera
                str.Append("<thead>");
                str.Append("<tr>");
                str.Append("<th style='width: 30px'>Listado de<br/>Revisión</th>");
                str.Append("<th style=''>Periodo</th>");
                str.Append("<th style=''>Año</th>");
                str.Append("<th style=''>Mes</th>");
                str.Append("<th style='background: #9370DB;'>Estado</th>");
                str.Append("<th style='background: #9370DB;'>Revisión</th>");
                str.Append("<th style='background: #9370DB;'>Plazo Cierre Extranet</th>");
                str.Append("<th style='background: #9370DB;'>Plazo Entrega EDI</th>");
                str.Append("</tr>");
                str.Append("</thead>");
                #endregion
                #region cuerpo
                str.Append("<tbody>");
                foreach (var reg in list)
                {
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.AppendFormat("<a class='' href='JavaScript:listarRevisiones(" + reg.Ipericodi + ");' style='margin-right: 4px;'><img style='margin-top: 3px; margin-bottom: 3px;' src='" + url + "Content/Images/btn-properties.png' alt='Ver listado de revisión' title='Ver listado de revisión' /></a>");
                    str.Append("</td>");

                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Iperinombre);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Iperianio);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Iperimes);

                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.RerrevestadoDesc);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rerrevnombre);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.RerrevfechaDesc);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.RerrevfechaentregaEDIDesc);

                    str.Append("</tr>");
                }
                str.Append("</tbody>");
                #endregion
                str.Append("</table>");
                #endregion

                return str.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera reporte html del Listado de Revisiones
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoRevisiones(string url, bool tienePermisoEditar, int ipericodi)
        {
            try
            {
                List<RerRevisionDTO> list = FactoryTransferencia.GetRerRevisionRepository().GetByCriteria(ipericodi);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    list = list.OrderByDescending(x => x.Rerrevcodi).ToList();
                    foreach (var reg in list)
                    {
                        FormatearRerRevision(reg);
                    }
                }

                StringBuilder str = new StringBuilder();
                #region tabla
                str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_revision'>");
                #region cabecera
                str.Append("<thead>");
                str.Append("<tr>");
                str.Append("<th style='background: #9370DB; width: 30px'>Opciones</th>");
                str.Append("<th style='background: #9370DB;'>Estado</th>");
                str.Append("<th style='background: #9370DB;'>Nombre</th>");
                str.Append("<th style='background: #9370DB;'>Usuario modificación</th>");
                str.Append("<th style='background: #9370DB;'>Fecha modificación</th>");
                str.Append("</tr>");
                str.Append("</thead>");
                #endregion
                #region cuerpo
                str.Append("<tbody>");
                foreach (var reg in list)
                {
                    str.Append("<tr>");
                    str.Append("<td>");
                    str.AppendFormat("<a class='' href='JavaScript:verRevision({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-open.png' alt='Ver revisión' title='Ver revisión' /></a>", reg.Rerrevcodi, url);
                    if (tienePermisoEditar)
                    {
                        str.AppendFormat("<a class='' href='JavaScript:editarRevision({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-edit.png' alt='Editar revisión' title='Editar revisión' /></a>", reg.Rerrevcodi, url);
                    }
                    str.Append("</td>");
                    str.AppendFormat("<td class='' style='text-align: center;'>{0}</td>", reg.RerrevestadoDesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.Rerrevnombre);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rerrevusumodificacion);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.RerrevfecmodificacionDesc);
                    str.Append("</tr>");
                }
                str.Append("</tbody>");
                #endregion
                str.Append("</table>");
                #endregion

                return str.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Formatea RerRevision
        /// </summary>
        /// <param name="reg"></param>
        private static void FormatearRerRevision(RerRevisionDTO reg)
        {
            reg.RerrevtipoDesc = (string.IsNullOrEmpty(reg.Rerrevtipo)) ? "" : ((reg.Rerrevtipo == ConstantesPrimasRER.tipoMensual) ? ConstantesPrimasRER.descTipoMensual : ConstantesPrimasRER.descTipoRevision);
            reg.RerrevfechaDesc = (reg.Rerrevfecha != null) ? reg.Rerrevfecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "";
            reg.RerrevfechaentregaEDIDesc = (reg.RerrevfechaentregaEDI != null) ? reg.RerrevfechaentregaEDI.Value.ToString(ConstantesAppServicio.FormatoFecha) : "";
            reg.RerrevestadoDesc = (string.IsNullOrEmpty(reg.Rerrevestado)) ? "" : ((reg.Rerrevestado == ConstantesPrimasRER.estadoAbierto) ? ConstantesPrimasRER.descEstadoAbierto : ConstantesPrimasRER.descEstadoCerrado);
            reg.RerrevfeccreacionDesc = (reg.Rerrevfeccreacion != null) ? reg.Rerrevfeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
            reg.RerrevfecmodificacionDesc = (reg.Rerrevfecmodificacion != null) ? reg.Rerrevfecmodificacion.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
        }


        #endregion

        #region Cuadros

        #region Evaluaciones

        /// <summary>
        /// Genera reporte html del Listado de Evaluaciones
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tienePermisoEditar"></param>
        /// <param name="rerrevcodi"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoEvaluaciones(string url, bool tienePermisoEditar, int rerrevcodi)
        {
            try
            {
                List<RerEvaluacionDTO> list = FactoryTransferencia.GetRerEvaluacionRepository().GetByCriteria(rerrevcodi);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    list = list.OrderBy(x => x.Rerevanumversion).ToList();
                    foreach (var reg in list)
                    {
                        FormatearRerEvaluacion(reg);
                    }
                }

                StringBuilder str = new StringBuilder();
                #region tabla
                str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_version_x_revision'>");
                #region cabecera
                str.Append("<thead>");
                str.Append("<tr>");
                str.Append("<th style='background: #9370DB;'>N° Versión</th>");
                str.Append("<th style='background: #9370DB;'>Revisión</th>");
                str.Append("<th style='background: #9370DB;'>Usuario registro</th>");
                str.Append("<th style='background: #9370DB;'>Fecha registro</th>");
                str.Append("<th style='background: #9370DB;'>Estado</th>");
                if (tienePermisoEditar)
                {
                    str.AppendFormat("<th style='width: 30px'>{0}</th>", "Ver/Editar");
                }
                else
                {
                    str.AppendFormat("<th style='width: 30px'>{0}</th>", "Ver");
                }
                str.Append("<th style='width: 30px'>Excel</th>");
                str.Append("</tr>");
                str.Append("</thead>");
                #endregion
                #region cuerpo
                str.Append("<tbody>");
                foreach (var reg in list)
                {
                    str.Append("<tr>");
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rerevanumversion);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rerrevnombre);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rerevausucreacion);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.RerevafeccreacionDesc);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.RerevaestadoDesc);
                    str.Append("<td style='text-align: center;'>");
                    if (tienePermisoEditar)
                    {
                        str.AppendFormat("<a class='' href='JavaScript:editarEvaluacion({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-edit.png' alt='Editar reporte' title='Editar versión' /></a>", reg.Rerevacodi, url);
                    }
                    else
                    {
                        str.AppendFormat("<a class='' href='JavaScript:verEvaluacion({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/btn-open.png' alt='Visualizar reporte' title='Visualizar versión' /></a>", reg.Rerevacodi, url);
                    }
                    str.Append("</td>");
                    str.Append("<td style='text-align: center;'>");
                    str.AppendFormat("<a class='' href='JavaScript:descargarEvaluacion({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/ExportExcel.png' title='Descargar versión' /></a>", reg.Rerevacodi, url);
                    str.Append("</td>");
                    str.Append("</tr>");
                }
                str.Append("</tbody>");
                #endregion
                str.Append("</table>");
                #endregion

                return str.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la cantidad de evaluaciones validadas de una revisión específica
        /// </summary>
        /// <param name="rerrevcodi"></param>
        /// <returns></returns>
        public int CantidadEvaluacionValidado(int rerrevcodi)
        {
            try
            {
                return FactoryTransferencia.GetRerEvaluacionRepository().GetCantidadEvaluacionValidado(rerrevcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la cantidad de revisiones de tipo revisión de un periodo específico
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        public int CantidadRevisionesTipoRevision(int ipericodi)
        {
            try
            {
                return FactoryTransferencia.GetRerRevisionRepository().GetCantidadRevisionesTipoRevision(ipericodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera una nueva evaluación para la revisión especificada 
        /// </summary>
        /// <param name="rerrevcodi"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public void GenerarNuevaEvaluacion(int rerrevcodi, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validar datos
                string mensajeError = ValidarGenerarNuevaEvaluacion(rerrevcodi, out RerRevisionDTO rerRevision,
                    out List<RerSolicitudEdiDTO> listSolicitudEdi, out List<RerEnergiaUnidadDTO> listEnergiaUnidad,
                    out List<RerEnergiaUnidadDetDTO> listEnergiaUnidadDet, out List<RerFacPerMedDetDTO> listFacPerMedDet);
                bool existeError = !string.IsNullOrEmpty(mensajeError);
                if (existeError)
                {
                    throw new Exception(mensajeError);
                }
                #endregion

                #region Crear Evaluación con sus Solicitudes EDI

                #region Variables
                conn = FactoryTransferencia.GetRerEvaluacionRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerEvaluacionRepository().StartTransaction(conn);
                int rerevacodi = FactoryTransferencia.GetRerEvaluacionRepository().GetMaxId();
                int rerevanumversion = FactoryTransferencia.GetRerEvaluacionRepository().GetNextNumVersion(rerrevcodi);
                #endregion

                #region Actualizar Estado de Evaluaciones a Generado
                int cantidadEvaluacionValidado = FactoryTransferencia.GetRerEvaluacionRepository().GetCantidadEvaluacionValidado(rerrevcodi);
                bool esMayorACero = (cantidadEvaluacionValidado > 0);
                if (esMayorACero)
                {
                    FactoryTransferencia.GetRerEvaluacionRepository().UpdateEstadoAGenerado(rerrevcodi, usuario, conn, tran);

                    int idPlazoEntregaEDI = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesPrimasRER.IdPlazoEntregaEDI]);
                    List<RerRevisionDTO> listRevision = FactoryTransferencia.GetRerRevisionRepository().ListPeriodosConUltimaRevision(idPlazoEntregaEDI);
                    bool existeListRevision = (listRevision != null && listRevision.Count > 0);
                    if (existeListRevision)
                    {
                        RerRevisionDTO rev = listRevision.First(x => x.Rerrevcodi == rerrevcodi);
                        bool existeRev = (rev != null);
                        if (existeRev)
                        {   //Como es la última revisión, entonces actualizamos su estado a abierto
                            FactoryTransferencia.GetRerRevisionRepository().UpdateEstado(rerrevcodi, ConstantesPrimasRER.estadoAbierto, usuario, conn, tran);
                        }
                    }
                }
                #endregion

                #region Crear Evaluación
                DateTime fecha = DateTime.Now;
                RerEvaluacionDTO rerEvaluacion = new RerEvaluacionDTO()
                {
                    Rerevacodi = rerevacodi,
                    Rerrevcodi = rerRevision.Rerrevcodi,
                    Rerevanumversion = rerevanumversion,
                    Rerevaestado = ConstantesPrimasRER.estadoGenerado,
                    Rerevausucreacion = usuario,
                    Rerevafeccreacion = fecha,
                    Rerevausumodificacion = usuario,
                    Rerevafecmodificacion = fecha
                };
                FactoryTransferencia.GetRerEvaluacionRepository().Save(rerEvaluacion, conn, tran);
                #endregion

                #region Crear Solicitudes EDI de la Evaluación
                int reresecodi = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetMaxId();
                int rereeucodi = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetMaxId();
                int rereedcodi = FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().GetMaxId();

                foreach (var regSE in listSolicitudEdi)
                {
                    decimal? fpmc = null;
                    bool existeListFacPerMedDet = (listFacPerMedDet != null && listFacPerMedDet.Count > 0);
                    if (existeListFacPerMedDet)
                    {
                        List<RerFacPerMedDetDTO> listFpmd = listFacPerMedDet.Where(x => x.Emprcodi == regSE.Emprcodi && x.Equicodi == regSE.Equicodi).ToList();
                        bool existeListFpmd = (listFpmd != null && listFpmd.Count > 0);
                        if (existeListFpmd)
                        {
                            fpmc = listFpmd.OrderBy(x => x.Rerfpmdesde).ThenBy(x => x.Rerfpmhasta).First().Rerfpdfactperdida;
                        }
                    }

                    DateTime fechaese = DateTime.Now;
                    RerEvaluacionSolicitudEdiDTO rerEvaluacionSolicitudEdi = new RerEvaluacionSolicitudEdiDTO()
                    {
                        Reresecodi = reresecodi,
                        Rerevacodi = rerEvaluacion.Rerevacodi,
                        Rersedcodi = regSE.Rersedcodi,
                        Rercencodi = regSE.Rercencodi,
                        Emprcodi = regSE.Emprcodi,
                        Ipericodi = regSE.Ipericodi,
                        Reroricodi = regSE.Reroricodi,
                        Reresefechahorainicio = regSE.Rersedfechahorainicio,
                        Reresefechahorafin = regSE.Rersedfechahorafin,
                        Reresedesc = regSE.Rerseddesc,
                        Reresetotenergia = regSE.Rersedtotenergia,
                        Reresesustento = regSE.Rersedsustento,
                        Rereseestadodeenvio = regSE.Rersedestadodeenvio,
                        Rereseeliminado = regSE.Rersedeliminado,
                        Rereseusucreacionext = regSE.Rersedusucreacion,
                        Reresefeccreacionext = regSE.Rersedfeccreacion.Value,
                        Rereseusumodificacionext = regSE.Rersedusumodificacion,
                        Reresefecmodificacionext = regSE.Rersedfecmodificacion.Value,
                        Rereserfpmc = fpmc,
                        Rereseusucreacion = usuario,
                        Reresefeccreacion = fechaese,
                        Rereseusumodificacion = usuario,
                        Reresefecmodificacion = fechaese
                    };
                    FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().Save(rerEvaluacionSolicitudEdi, conn, tran);
                    reresecodi++;

                    #region Crear Energía por Unidad de Generación de una Central de las Solicitudes EDI de la Evaluación
                    List<RerEnergiaUnidadDTO> listEU = listEnergiaUnidad.Where(x => x.Rersedcodi == regSE.Rersedcodi).OrderBy(x => x.Rereucodi).ToList();
                    bool existeListEU = (listEU != null && listEU.Count > 0);
                    if (existeListEU)
                    {
                        foreach (var regEU in listEU)
                        {
                            RerEvaluacionEnergiaUnidadDTO rerEvaluacionEnergiaUnidad = new RerEvaluacionEnergiaUnidadDTO()
                            {
                                Rereeucodi = rereeucodi,
                                Reresecodi = rerEvaluacionSolicitudEdi.Reresecodi,
                                Rerevacodi = rerEvaluacionSolicitudEdi.Rerevacodi,
                                Rereucodi = regEU.Rereucodi,
                                Rersedcodi = regEU.Rersedcodi,
                                Equicodi = regEU.Equicodi,
                                Rereeuenergiaunidad = regEU.Rereuenergiaunidad,
                                Rereeutotenergia = regEU.Rereutotenergia,
                                Rereeuusucreacionext = regEU.Rereuusucreacion,
                                Rereeufeccreacionext = regEU.Rereufeccreacion,
                                Rereeuusucreacion = usuario,
                                Rereeufeccreacion = DateTime.Now
                            };
                            FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().Save(rerEvaluacionEnergiaUnidad, conn, tran);
                            rereeucodi++;

                            if (listEnergiaUnidadDet != null && listEnergiaUnidadDet.Count > 0)
                            {
                                List<RerEnergiaUnidadDetDTO> listEUDet = listEnergiaUnidadDet.Where(x => x.Rereucodi == regEU.Rereucodi).OrderBy(x => x.Rereudcodi).ToList();
                                bool existeListEUDet = (listEUDet != null && listEUDet.Count > 0);
                                if (existeListEUDet)
                                {
                                    foreach (var regEUDet in listEUDet)
                                    {
                                        RerEvaluacionEnergiaUnidDetDTO rerEvaluacionEnergiaUnidadDet = new RerEvaluacionEnergiaUnidDetDTO()
                                        {
                                            Rereedcodi = rereedcodi,
                                            Rereeucodi = rerEvaluacionEnergiaUnidad.Rereeucodi,
                                            Rereedenergiaunidad = regEUDet.Rereudenergiaunidad
                                        };
                                        FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().Save(rerEvaluacionEnergiaUnidadDet, conn, tran);
                                        rereedcodi++;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Obtener una evaluación
        /// </summary>
        public RerEvaluacionDTO ObtenerEvaluacion(int rerevacodi)
        {
            try
            {
                return FactoryTransferencia.GetRerEvaluacionRepository().GetById(rerevacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }


        /// <summary>
        /// Obtener las ultimas evaluaciones segun su estado de un Año Tarifario
        /// </summary>
        public List<RerEvaluacionDTO> ObtenerUltimaEvaluacionByEstadoByAnioTarifario(string rerevaestado, int anio)
        {
            try
            {
                return FactoryTransferencia.GetRerEvaluacionRepository().GetUltimaByEstadoEvaluacionByAnioTarifario(rerevaestado, anio);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtener una evaluación solicitud edi
        /// </summary>
        public RerEvaluacionSolicitudEdiDTO ObtenerEvaluacionSolicitudEdi(int reresecodi)
        {
            try
            {
                return FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtener una evaluación energía unidad
        /// </summary>
        public RerEvaluacionEnergiaUnidadDTO ObtenerEvaluacionEnergiaUnidad(int rereeucodi)
        {
            try
            {
                return FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetById(rereeucodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera un handson table desde un listado de EvalucionSolicitudEdi filtrado por rerevacodi (Id de Evaluación)
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public HandsonModel GenerarHandsonTableEvaluacionSolicitudEdi(int rerevacodi, string url)
        {
            try
            {
                #region Validación Básica
                bool esRerevacodiValido = (rerevacodi > 0);
                if (!esRerevacodiValido)
                {
                    throw new Exception("El id de la evaluación no es válido");
                }

                RerEvaluacionDTO evaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetById(rerevacodi);
                bool existeEvaluacion = (evaluacion != null);
                if (!existeEvaluacion)
                {
                    throw new Exception("No existe una evaluación con id = " + rerevacodi);
                }
                #endregion

                #region Definir alcance
                int numColFijas = 17;
                int numColMoviles = 0;
                string[] fuenteDatoVacio = new string[] { };
                string[] resultadoDesc = new string[] { "", "Posee coherencia con datos históricos", "Se redujo la magnitud EDI solicitada descontando la generación ejecutada en el periodo solicitado", "Otro" };
                string[] resultadoEstado = new string[] { "", ConstantesPrimasRER.resultadoEstadoAprobada, ConstantesPrimasRER.resultadoEstadoNoAprobada, ConstantesPrimasRER.resultadoEstadoSolicitudDeFuerzaMayor };
                List<RerEvaluacionSolicitudEdiDTO> list = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetByCriteria(evaluacion.Rerevacodi, -1);
                bool existeList = (list != null && list.Count > 0);
                if (existeList)
                {
                    list = list.Where(x => x.Rereseeliminado.ToUpper() == ConstantesPrimasRER.eliminadoNo).ToList();
                    existeList = (list != null && list.Count > 0);
                    if (existeList)
                    {
                        list = list.OrderBy(x => x.Equinomb).ThenBy(x => x.Emprnomb).ThenBy(x => x.Reresefechahorainicio).ThenBy(x => x.Reresefechahorafin).ToList();
                    }
                }

                List<CabeceraRow> listaCabecera = new List<CabeceraRow>
                {
                    new CabeceraRow() { TituloRow = "", IsMerge = 0, Ancho = 1, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false }, //oculto - reresecodi
                    new CabeceraRow() { TituloRow = "Id <br/> Solicitud <br/> EDI", IsMerge = 0, Ancho = 60, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Central", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Empresa", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Fecha y hora <br/> de inicio", IsMerge = 0, Ancho = 105, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = true },
                    new CabeceraRow() { TituloRow = "Fecha y hora <br/> de término", IsMerge = 0, Ancho = 105, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = true },
                    new CabeceraRow() { TituloRow = "Origen", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Detalle", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = true },
                    new CabeceraRow() { TituloRow = "Solicitud <br/> de EDI <br/> (MWh)", IsMerge = 0, Ancho = 50, AlineacionHorizontal = "Derecha", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Sustento", IsMerge = 0, Ancho = 50, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Fecha <br/> Registro", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Fecha <br/> Última <br/> Modif.", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Estado", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Factor de <br/> Pérdida", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Derecha", TipoDato = GridExcel.TipoNumerico, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "EDI <br/> Aprobada", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Derecha", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Resultado <br/> Descripción", IsMerge = 1, Ancho = 200, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoLista, FuenteDato = resultadoDesc, EsEditable = true },
                    new CabeceraRow() { TituloRow = "Resultado <br/> Estado", IsMerge = 1, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoLista, FuenteDato = resultadoEstado, EsEditable = true },
                };
                #endregion

                #region Cargar configuraciones
                string[] headers = listaCabecera.Select(x => x.TituloRow).ToArray();
                List<int> widths = listaCabecera.Select(x => x.Ancho).ToList();
                object[] columnas = new object[headers.Length];

                for (int m = 0; m < headers.Length; m++)
                {
                    var cabecera = listaCabecera[m];
                    columnas[m] = new
                    {
                        type = cabecera.TipoDato,
                        source = cabecera.FuenteDato,
                        strict = false,
                        dateFormat = string.Empty,
                        correctFormat = false,
                        defaultDate = string.Empty,
                        format = string.Empty,
                        className = cabecera.AlineacionHorizontal == "Derecha" ? "htRight" : (cabecera.AlineacionHorizontal == "Izquierda" ? "htLeft" : "htCenter"),
                        readOnly = !cabecera.EsEditable,
                    };
                }
                #endregion

                #region Cargar datos 
                int numFilaActual = 0;
                int numCol = numColFijas + numColMoviles;
                List<string[]> listaDataHandson = new List<string[]>();

                foreach (var reg in list)
                {
                    string[] matriz = new string[numCol];
                    if (reg != null)
                    {
                        matriz[0] = reg.Reresecodi.ToString(); //oculto - reresecodi
                        matriz[1] = reg.Rersedcodi.ToString(); //rersedcodi
                        matriz[2] = reg.Equinomb; //Central
                        matriz[3] = reg.Emprnomb; //Empresa
                        matriz[4] = reg.Reresefechahorainicio.ToString(ConstantesAppServicio.FormatoFechaFull); //Fecha hora inicio //editable
                        matriz[5] = reg.Reresefechahorafin.ToString(ConstantesAppServicio.FormatoFechaFull); //Fecha hora fin //editable
                        matriz[6] = reg.Reroridesc; //Origen
                        matriz[7] = reg.Reresedesc; //Detalle  //editable
                        matriz[8] = reg.Reresecodi.ToString(); //energía Solicitud de EDI (MWh) 
                        matriz[9] = reg.Reresesustento; // archivo sustento
                        matriz[10] = reg.Reresefeccreacionext.ToString(ConstantesAppServicio.FormatoFechaFull); //Fecha Registro
                        matriz[11] = reg.Reresefecmodificacionext.ToString(ConstantesAppServicio.FormatoFechaFull); //Fecha Última Modif.
                        matriz[12] = (reg.Rereseestadodeenvio == ConstantesPrimasRER.estadoFueraPlazo) ? ConstantesPrimasRER.descEstadoFueraPlazo : ConstantesPrimasRER.descEstadoEnPlazo; //Estado
                        matriz[13] = (reg.Rereserfpmc != null) ? reg.Rereserfpmc.Value.ToString() : ""; //Factor de Pérdida
                        matriz[14] = (reg.Rereseediaprobada != null) ? reg.Rereseediaprobada.Value.ToString() : ""; //EDI Aprobada  
                        matriz[15] = reg.Rereseresdesc; //Resultado - Descripción  //editable
                        matriz[16] = reg.Rereseresestado; //Resultado - Estado  //editable
                    }

                    listaDataHandson.Add(matriz);
                    numFilaActual++;
                }

                List<CeldaMerge> listaMerge = new List<CeldaMerge>();
                #endregion

                #region Return
                HandsonModel handson = new HandsonModel();
                handson.ListaExcelData = listaDataHandson.ToArray();
                handson.Headers = headers;
                handson.ListaColWidth = widths;
                handson.Columnas = columnas;
                handson.MaxCols = numCol;
                handson.MaxRows = listaDataHandson.Count;
                handson.ListaMerge = listaMerge;
                handson.ListaCambios = new List<CeldaCambios>(); //arrCambioCells;

                return handson;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza las solicitudes EDI de una evaluación específica
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="dataht"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string ActualizarEvaluacionSolicitudEdi(int rerevacodi, string[][] dataht, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            #region Actualizar EvaluacionSolicitudEdi
            try
            {
                #region Validación
                string mensajeError = ValidarActualizarEvaluacionSolicitudEdi(rerevacodi, dataht, usuario);
                bool existeError = !string.IsNullOrEmpty(mensajeError);
                if (existeError)
                {
                    throw new Exception(mensajeError);
                }
                #endregion

                #region Actualizar EvaluacionSolicitudEDI
                conn = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().StartTransaction(conn);
                foreach (string[] data in dataht)
                {
                    RerEvaluacionSolicitudEdiDTO entity = SetEvaluacionSolicitudEdiByUpdateFields(rerevacodi, data, usuario);
                    FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().UpdateFields(entity, conn, tran);
                }

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }

            return "1";
            #endregion
        }

        /// <summary>
        /// Genera un archivo Excel con las solicitudes EDI de una evaluación específica
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="tipoReporte">
        /// tipoReporte = 0 : Si es invocado desde Cuadros - Evaluación
        /// tipoReporte = 1 : Si es invocado desde Cuadros - Resultados - Aprobados
        /// tipoReporte = 2 : Si es invocado desde Cuadros - Resultados - No Aprobados
        /// tipoReporte = 3 : Si es invocado desde Cuadros - Resultados - Fuerza Mayor
        /// </param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelEvaluacion(int rerevacodi, int tipoReporte)
        {
            try
            {
                #region Titulo
                RerEvaluacionDTO rerEvaluacion = ObtenerEvaluacion(rerevacodi);
                string titulo = "";
                if (tipoReporte == ConstantesPrimasRER.tipoReporte)
                {
                    titulo = "Evaluación - " + rerEvaluacion.Iperinombre + " - " + rerEvaluacion.Rerrevnombre + " - Versión N° " + rerEvaluacion.Rerevanumversion;
                }
                else
                {
                    //Tipo de Reporte: Aprobados, No Aprobados, Fuerza Mayor
                    string descTipoReporte = (tipoReporte == ConstantesPrimasRER.tipoReporteAprobados) ? ConstantesPrimasRER.descReporteAprobados :
                        (tipoReporte == ConstantesPrimasRER.tipoReporteNoAprobados ? ConstantesPrimasRER.descReporteNoAprobados :
                        (tipoReporte == ConstantesPrimasRER.tipoReporteFuerzaMayor ? ConstantesPrimasRER.descReporteFuerzaMayor : ""));

                    titulo = "Resultados - " + rerEvaluacion.Iperinombre + " - " + rerEvaluacion.Rerrevnombre + " - Versión N° " + rerEvaluacion.Rerevanumversion + " - EDIS " + descTipoReporte;
                }
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[1];
                if (tipoReporte == ConstantesPrimasRER.tipoReporte)
                {
                    listaCabeceras[0] = new List<RerExcelModelo>
                    {
                        CrearExcelModelo("Id \n Solicitud \n EDI"),
                        CrearExcelModelo("Central"),
                        CrearExcelModelo("Empresa"),
                        CrearExcelModelo("Fecha y hora \n de inicio"),
                        CrearExcelModelo("Fecha y hora \n de término"),
                        CrearExcelModelo("Origen"),
                        CrearExcelModelo("Detalle"),
                        CrearExcelModelo("Fecha \n Registro"),
                        CrearExcelModelo("Fecha \n Última Modif."),
                        CrearExcelModelo("Estado"),
                        CrearExcelModelo("Factor de \n Pérdida"),
                        CrearExcelModelo("EDI \n Aprobada"),
                        CrearExcelModelo("Resultado \n Descripción"),
                        CrearExcelModelo("Resultado \n Estado")
                    };
                }
                else
                {
                    //Tipo de Reporte: Aprobados, No Aprobados, Fuerza Mayor
                    string titulo1 = (tipoReporte == ConstantesPrimasRER.tipoReporteAprobados) ? "Magnitud \n EDI \n Aprobada" : "EDI \n Solicitada \n (MWh)";
                    string titulo2 = (tipoReporte == ConstantesPrimasRER.tipoReporteNoAprobados) ? "Motivo" : "Resultado \n Descripción";
                    listaCabeceras[0] = new List<RerExcelModelo>
                    {
                        CrearExcelModelo("Fecha y hora \n de inicio"),
                        CrearExcelModelo("Fecha y hora \n de término"),
                        CrearExcelModelo("Empresa"),
                        CrearExcelModelo("Equipo"),
                        CrearExcelModelo("Origen"),
                        CrearExcelModelo("Descripción"),
                        CrearExcelModelo(titulo1),
                        CrearExcelModelo(titulo2),
                        CrearExcelModelo("Resultado \n Estado")
                    };
                }

                List<int> listaAnchoColumna = new List<int> { };
                if (tipoReporte == ConstantesPrimasRER.tipoReporte)
                {
                    listaAnchoColumna = new List<int>
                    {
                        20, //Id Solicitud EDI
                        40, //Central
                        40, //Empresa
                        20, //Fecha y hora de inicio
                        20, //Fecha y hora de término
                        60, //Origen
                        60, //Detalle
                        20, //Fecha Registro
                        20, //Fecha Última Modif.
                        20, //Estado
                        20, //Factor de Pérdida
                        20, //EDI Aprobada
                        40, //Resultado Descripción
                        20  //Resultado Estado
                    };
                }
                else
                {
                    //Tipo de Reporte: Aprobados, No Aprobados, Fuerza Mayor
                    listaAnchoColumna = new List<int>
                    {
                        20, //Fecha y hora de inicio
                        20, //Fecha y hora de término
                        40, //Empresa
                        40, //Equipo
                        60, //Origen
                        60, //Descripción
                        20, //EDI Aprobada // EDI Solicitada (MWh)
                        40, //Resultado Descripción // Motivo
                        20  //Resultado Estado
                    };
                }
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { };
                if (tipoReporte == ConstantesPrimasRER.tipoReporte)
                {
                    listaAlineaHorizontal = new List<string>
                    {
                        "left", //Id Solicitud EDI
                        "left", //Central
                        "left", //Empresa
                        "left", //Fecha hora inicio 
                        "left", //Fecha hora fin 
                        "left", //Origen
                        "left", //Detalle 
                        "left", //Fecha Registro
                        "left", //Fecha Última Modif.
                        "left", //Estado
                        "right", //Factor de Pérdida
                        "right", //EDI Aprobada 
                        "left", //Resultado - Descripción 
                        "left" //Resultado - Estado 
                    };
                }
                else
                {
                    //Tipo de Reporte: Aprobados, No Aprobados, Fuerza Mayor
                    listaAlineaHorizontal = new List<string>
                    {
                        "left", //Fecha hora inicio 
                        "left", //Fecha hora fin 
                        "left", //Empresa
                        "left", //Equipo
                        "left", //Origen
                        "left", //Descripción 
                        "right", //Magnitud EDI Aprobada //EDI Solicitada (MWh)
                        "left", //Resultado - Descripción // Motivo
                        "left" //Resultado - Estado 
                    };
                }

                List<string> listaTipo = new List<string> { };
                if (tipoReporte == ConstantesPrimasRER.tipoReporte)
                {
                    listaTipo = new List<string>
                    {
                        "integer", //Id Solicitud EDI
                        "string", //Central
                        "string", //Empresa
                        "string", //Fecha hora inicio 
                        "string", //Fecha hora fin
                        "string", //Origen
                        "string", //Detalle
                        "string", //Fecha Registro
                        "string", //Fecha Última Modif.
                        "string", //Estado
                        "double", //Factor de Pérdida
                        "double", //EDI Aprobada
                        "string", //Resultado - Descripción
                        "string" //Resultado - Estado
                    };
                }
                else
                {
                    //Tipo de Reporte: Aprobados, No Aprobados, Fuerza Mayor
                    listaTipo = new List<string>
                    {
                        "string", //Fecha hora inicio 
                        "string", //Fecha hora fin
                        "string", //Empresa
                        "string", //Equipo
                        "string", //Origen
                        "string", //Descripción
                        "double", //Magnitud EDI Aprobada //EDI Solicitada (MWh)
                        "string", //Resultado - Descripción //Motivo
                        "string" //Resultado - Estado
                    };
                }

                List<string>[] listaRegistros = null;
                List<RerEvaluacionSolicitudEdiDTO> listESE = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetByCriteria(rerevacodi, -1);
                bool existeListESE = (listESE != null && listESE.Count > 0);
                if (existeListESE)
                {
                    listESE = listESE.Where(x => x.Rereseeliminado.ToUpper() == ConstantesPrimasRER.eliminadoNo).ToList();
                    existeListESE = (listESE != null && listESE.Count > 0);
                    if (existeListESE)
                    {
                        listESE = (tipoReporte == ConstantesPrimasRER.tipoReporteAprobados) ?
                            listESE.Where(x => x.Rereseresestado == ConstantesPrimasRER.resultadoEstadoAprobada).ToList() :
                            ((tipoReporte == ConstantesPrimasRER.tipoReporteNoAprobados) ?
                            listESE.Where(x => x.Rereseresestado == ConstantesPrimasRER.resultadoEstadoNoAprobada).ToList() :
                            ((tipoReporte == ConstantesPrimasRER.tipoReporteFuerzaMayor) ?
                            listESE.Where(x => x.Reroricodi == ConstantesPrimasRER.IdCausasDeFuerzaMayorCalificadasPorOsinergmin &&
                                              (x.Rereseresestado != ConstantesPrimasRER.resultadoEstadoAprobada &&
                                               x.Rereseresestado != ConstantesPrimasRER.resultadoEstadoNoAprobada
                                              )).ToList() :
                            listESE.ToList()
                            ));

                        existeListESE = (listESE != null && listESE.Count > 0);
                        if (existeListESE)
                        {
                            listESE = (tipoReporte == ConstantesPrimasRER.tipoReporte) ?
                                listESE.OrderBy(x => x.Equinomb).ThenBy(x => x.Emprnomb).ThenBy(x => x.Reresefechahorainicio).ThenBy(x => x.Reresefechahorafin).ToList() :
                                listESE.OrderBy(x => x.Reresefechahorainicio).ThenBy(x => x.Reresefechahorafin).ThenBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ToList();

                            int i = 0;
                            listaRegistros = new List<string>[listESE.Count];
                            foreach (var reg in listESE)
                            {
                                List<string> registro = new List<string> { };
                                if (tipoReporte == ConstantesPrimasRER.tipoReporte)
                                {
                                    registro = new List<string>
                                    {
                                        reg.Rersedcodi.ToString(), //Id Solicitud EDI
                                        reg.Equinomb, //Central
                                        reg.Emprnomb, //Empresa
                                        reg.Reresefechahorainicio.ToString(ConstantesAppServicio.FormatoFechaFull), //Fecha hora inicio 
                                        reg.Reresefechahorafin.ToString(ConstantesAppServicio.FormatoFechaFull), //Fecha hora fin 
                                        reg.Reroridesc, //Origen
                                        reg.Reresedesc, //Detalle  
                                        reg.Reresefeccreacionext.ToString(ConstantesAppServicio.FormatoFechaFull), //Fecha Registro
                                        reg.Reresefecmodificacionext.ToString(ConstantesAppServicio.FormatoFechaFull), //Fecha Última Modif.
                                        (reg.Rereseestadodeenvio == ConstantesPrimasRER.estadoFueraPlazo) ? ConstantesPrimasRER.descEstadoFueraPlazo : ConstantesPrimasRER.descEstadoEnPlazo, //Estado
                                        (reg.Rereserfpmc != null) ? reg.Rereserfpmc.Value.ToString() : "", //Factor de Pérdida
                                        (reg.Rereseediaprobada != null) ? reg.Rereseediaprobada.Value.ToString() : "", //EDI Aprobada  
                                        reg.Rereseresdesc, //Resultado - Descripción 
                                        reg.Rereseresestado //Resultado - Estado  
                                    };
                                }
                                else
                                {
                                    //Tipo de Reporte: Aprobados, No Aprobados, Fuerza Mayor

                                    string datoEdi = (tipoReporte == ConstantesPrimasRER.tipoReporteAprobados) ?
                                        ((reg.Rereseediaprobada != null) ? reg.Rereseediaprobada.Value.ToString() : "") : //Tipo de Reporte: Aprobados - Magnitud EDI Aprobada  
                                        reg.Reresetotenergia.ToString(); //Tipo de Reporte: No Aprobados, Fuerza Mayor - EDI Solicitada (MWh)

                                    registro = new List<string>
                                    {
                                        reg.Reresefechahorainicio.ToString(ConstantesAppServicio.FormatoFechaFull), //Fecha hora inicio 
                                        reg.Reresefechahorafin.ToString(ConstantesAppServicio.FormatoFechaFull), //Fecha hora fin 
                                        reg.Emprnomb, //Empresa
                                        reg.Equinomb, //Equippo
                                        reg.Reroridesc, //Origen
                                        reg.Reresedesc, //Descripción
                                        datoEdi, //Magnitud EDI Aprobada //EDI Solicitada (MWh)
                                        reg.Rereseresdesc, //Resultado - Descripción // Motivo
                                        reg.Rereseresestado //Resultado - Estado  
                                    };
                                }

                                listaRegistros[i] = registro;
                                i++;
                            }
                        }
                    }
                }

                RerExcelCuerpo cuerpo = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo
                };
                #endregion

                #region Return
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Solicitudes EDI",
                    Titulo = titulo,
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);

                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera un archivo Excel con las energías de unidad de generación de una central de una solicitudes EDI de una evaluación específica
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <param name="rerevacodi"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelEvaluacionEnergiaUnidad(int reresecodi, int rerevacodi)
        {
            try
            {
                #region Validación básica y obtener datos
                RerEvaluacionSolicitudEdiDTO ese = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
                bool esNuloESE = (ese == null);
                if (esNuloESE)
                {
                    throw new Exception("No existe un registro en RerEvaluacionSolicitudEdi con reresecodi = " + reresecodi);
                }

                List<RerEvaluacionEnergiaUnidadDTO> listEEU = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetByCriteria(reresecodi, rerevacodi);
                bool existeListEEU = (listEEU != null && listEEU.Count > 0);
                if (!existeListEEU)
                {
                    throw new Exception("No existe ningún registro en RerEvaluacionEnergiaUnidad con reresecodi = " + reresecodi + " y rerevacodi = " + rerevacodi);
                }
                listEEU = listEEU.OrderBy(x => x.Equinomb).ToList();

                //Se crea un diccionario donde se guardará el código de la unidad y sus energias en intervalos de 15 min
                Dictionary<int, string[]> listaEnergiaPorUnidad = new Dictionary<int, string[]>();
                if (listEEU.Count > 0)
                {
                    foreach (RerEvaluacionEnergiaUnidadDTO eeu in listEEU)
                    {
                        List<RerEvaluacionEnergiaUnidDetDTO> listEEUDet = FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().GetByCriteria(eeu.Rereeucodi.ToString());
                        if (listEEUDet != null && listEEUDet.Count > 0)
                        {
                            listEEUDet = listEEUDet.OrderBy(x => x.Rereedcodi).ToList();
                            foreach(var eeudet in listEEUDet)
                            {
                                eeu.Rereeuenergiaunidad += eeudet.Rereedenergiaunidad; 
                            }
                        }

                        //Se llena el diccionario con el código de la unidad y sus energias en intervalos de 15 min
                        listaEnergiaPorUnidad.Add(eeu.Equicodi, eeu.Rereeuenergiaunidad.Split(','));
                    }
                }

                List<DateTime> listaIntervalos = ObtenerIntervalosMinuto(ese.Reresefechahorainicio, ese.Reresefechahorafin);
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[1];
                listaCabeceras[0] = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Periodo de inicio"),
                    CrearExcelModelo("Periodo final")
                };
                List<int> listaAnchoColumna = new List<int>
                {
                    30,
                    30
                };
                for (int i = 0; i < listEEU.Count; i++)
                {
                    listaCabeceras[0].Add(CrearExcelModelo("Energía MWh " + listEEU[i].Equinomb));
                    listaAnchoColumna.Add(16);
                }
                #endregion

                #region Cuerpo
                RerExcelCuerpo cuerpo = null;
                List<string>[] tablaCuerpo;
                List<string> listaAlineaHorizontal = new List<string>();
                List<string> listaTipo = new List<string>();

                if (listaIntervalos != null && listaIntervalos.Count > 0)
                {
                    tablaCuerpo = new List<string>[listaIntervalos.Count - 1];
                    for (int i = 0; i < listaIntervalos.Count - 1; i++)
                    {
                        #region Configurando Alineamiento Horizontal y Tipo de Columna para Fecha Inicio y Fecha Fin
                        if (i == 0)
                        {
                            listaAlineaHorizontal.Add("left");
                            listaAlineaHorizontal.Add("left");
                            listaTipo.Add("string");
                            listaTipo.Add("string");
                        }
                        #endregion

                        #region Añadiendo Fecha Inicio y Fecha Fin
                        DateTime intervalo = listaIntervalos[i];
                        string fechaInicio = intervalo.ToString(ConstantesAppServicio.FormatoFechaFull);
                        string fechaFin = intervalo.AddMinutes(ConstantesPrimasRER.numero15).ToString(ConstantesAppServicio.FormatoFechaFull); ;
                        List<string> tFila = new List<string>
                        {
                            fechaInicio,
                            fechaFin
                        };
                        #endregion

                        #region Añadiendo EEU según cada Unidad Generadora de una Central de una Solicitid EDI
                        for (int j = 0; j < listEEU.Count; j++)
                        {
                            #region Configurando Alineamiento Horizontal y Tipo de Columna para EEU
                            if (i == 0)
                            {
                                listaAlineaHorizontal.Add("right");
                                listaTipo.Add("double");
                            }
                            #endregion

                            //obtengo el código de la unidad según la columna
                            int codUnidad = listEEU[j].Equicodi;

                            //Si el diccionario tiene el código de la unidad
                            if (listaEnergiaPorUnidad.ContainsKey(codUnidad))
                            {
                                //recupero la lista de energias en intervalo de 15 min
                                string[] energiaIntervalos = listaEnergiaPorUnidad[codUnidad];
                                if (energiaIntervalos.Length > i)
                                {
                                    string valorEnergia = energiaIntervalos[i];
                                    tFila.Add(valorEnergia);
                                }
                                else
                                {
                                    tFila.Add("0");
                                }
                            }
                            else
                            {
                                tFila.Add("");
                            }
                        }
                        #endregion

                        #region Añadiendo una fila a la tabla
                        tablaCuerpo[i] = tFila;
                        #endregion
                    }
                    cuerpo = CrearExcelCuerpo(tablaCuerpo, listaAlineaHorizontal, listaTipo);
                }
                #endregion

                #region Return
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Energías de Unidades de Generación",
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);

                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Setea los campos que se van a actualizar para EvaluacionSolicitudEdi
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="aData"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private RerEvaluacionSolicitudEdiDTO SetEvaluacionSolicitudEdiByUpdateFields(int rerevacodi, string[] aData, string usuario)
        {
            try
            {
                RerEvaluacionSolicitudEdiDTO entity = new RerEvaluacionSolicitudEdiDTO
                {
                    Reresecodi = Convert.ToInt32(aData[0]),
                    Rerevacodi = rerevacodi,
                    Reresefechahorainicio = DateTime.ParseExact(aData[4], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Reresefechahorafin = DateTime.ParseExact(aData[5], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Reresedesc = Convert.ToString(aData[7]),
                    Rereseresdesc = Convert.ToString(aData[15]),
                    Rereseresestado = Convert.ToString(aData[16]),
                    Rereseusumodificacion = usuario,
                    Reresefecmodificacion = DateTime.Now
                };
                if (!string.IsNullOrEmpty(aData[14]))
                {
                    entity.Rereseediaprobada = Convert.ToDecimal(aData[14]);
                }
                else
                {
                    entity.Rereseediaprobada = null;
                }
                return entity;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que exista una revisión y que esta sea de tipo Mensual
        /// </summary>
        /// <param name="rerrevcodi"></param>
        /// <param name="rerRevision">Revisión obtenida de la BD, según rerrevcodi</param>
        /// <param name="listSolicitudEdi">Lista de Solicitudes EDI obtenida de la BD, según el ipericodi de la revisión obtenida de la BD</param>
        /// <param name="listEnergiaUnidad">Lista de Energía Unidad de las Solicitudes EDI obtenida de la BD, según el ipericodi de la revisión obtenida de la BD</param>
        /// <param name="listEnergiaUnidadDet">Lista de Energía Unidad Detalle de las Solicitudes EDI obtenida de la BD, según el ipericodi de la revisión obtenida de la BD</param>
        /// <param name="listFacPerMedDet">Lista de Factor de Perdida Media Detalle obtenida de la BD, según el ipericodi de la revisión obtenida de la BD</param>
        /// <returns>Retorna un mensaje de error, en caso no pase la validación</returns>
        private string ValidarGenerarNuevaEvaluacion(int rerrevcodi, out RerRevisionDTO rerRevision, out List<RerSolicitudEdiDTO> listSolicitudEdi, out List<RerEnergiaUnidadDTO> listEnergiaUnidad, out List<RerEnergiaUnidadDetDTO> listEnergiaUnidadDet, out List<RerFacPerMedDetDTO> listFacPerMedDet)
        {
            try
            {
                rerRevision = null;
                listSolicitudEdi = null;
                listEnergiaUnidad = null;
                listEnergiaUnidadDet = null;
                listFacPerMedDet = null;
                StringBuilder sb = new StringBuilder();

                bool esRequeridoRerrevcodi = (rerrevcodi < 1);
                if (esRequeridoRerrevcodi)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("Id de Revisión es requerido");
                    return sb.ToString();
                }

                rerRevision = FactoryTransferencia.GetRerRevisionRepository().GetById(rerrevcodi);
                bool existeRerRevision = (rerRevision != null);
                if (!existeRerRevision)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("La revisión no existe");
                    return sb.ToString();
                }

                INDAppServicio indAppServicio = new INDAppServicio();
                IndPeriodoDTO periodo = indAppServicio.GetByIdIndPeriodo(rerRevision.Ipericodi);
                bool existePeriodo = (periodo != null);
                if (!existePeriodo)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.AppendFormat("No existe un Periodo con id = {0}", rerRevision.Ipericodi);
                    return sb.ToString();
                }

                bool esTipoMensual = (rerRevision.Rerrevtipo == ConstantesPrimasRER.tipoMensual);
                if (!esTipoMensual)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("La revisión debe ser de tipo 'Mensual'");
                    return sb.ToString();
                }

                bool existeFecha = (rerRevision.Rerrevfecha != null);
                if (!existeFecha)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("La revisión de tipo 'Mensual' debe tener una fecha de creación del periodo del reporte");
                    return sb.ToString();
                }

                string sFechaRevision = rerRevision.Rerrevfecha.Value.ToString(ConstantesAppServicio.FormatoFecha);
                string sFechaHoy = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
                DateTime fechaRevision = DateTime.ParseExact(sFechaRevision, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaHoy = DateTime.ParseExact(sFechaHoy, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                bool fechaHoyEsMenorOIgualFechaRevision = (DateTime.Compare(fechaHoy, fechaRevision) <= 0);
                if (fechaHoyEsMenorOIgualFechaRevision)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.AppendFormat("La fecha de hoy día '{0}' debe ser mayor a la fecha de creación del periodo del reporte '{1}' de la revisión de tipo 'Mensual'", sFechaHoy, sFechaRevision);
                    return sb.ToString();
                }

                int cantidadRevisionesTipoRevision = FactoryTransferencia.GetRerRevisionRepository().GetCantidadRevisionesTipoRevision(rerRevision.Ipericodi);
                bool esMayorACero = (cantidadRevisionesTipoRevision > 0);
                if (esMayorACero)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("El periodo especificado no debe tener revisiones de tipo 'Revisión'");
                    return sb.ToString();
                }

                listSolicitudEdi = FactoryTransferencia.GetRerSolicitudEdiRepository().ListByPeriodo(rerRevision.Ipericodi);
                bool existeListSolicitudEdi = (listSolicitudEdi != null && listSolicitudEdi.Count > 0);
                if (!existeListSolicitudEdi)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("No existen solicitudes EDI para el periodo especificado");
                }

                listEnergiaUnidad = FactoryTransferencia.GetRerEnergiaUnidadRepository().ListByPeriodo(rerRevision.Ipericodi);
                bool existeListEnergiaUnidad = (listEnergiaUnidad != null && listEnergiaUnidad.Count > 0);
                if (!existeListEnergiaUnidad)
                {
                    if (sb.ToString().Length > 0) { sb.Append(", "); }
                    sb.Append("No existen energías por unidad de generación de las solicitudes EDI para el periodo especificado");
                }

                string rereucodi = string.Join(",",listEnergiaUnidad.Select(x => x.Rereucodi).ToArray());
                listEnergiaUnidadDet = FactoryTransferencia.GetRerEnergiaUnidadDetRepository().GetByCriteria(rereucodi);
                if (listEnergiaUnidadDet != null && listEnergiaUnidadDet.Count > 0)
                {
                    listEnergiaUnidadDet = listEnergiaUnidadDet.OrderBy(x => x.Rereudcodi).ToList();
                }

                listFacPerMedDet = FactoryTransferencia.GetRerFacPerMedDetRepository().GetByRangeDate(periodo.FechaIni, periodo.FechaFin);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Valida los datos de EvaluacionSolicitudEdi
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="dataht"></param>
        /// <param name="usuario"></param>
        /// <returns>Retorna un mensaje de error, en caso no pase la validación</returns>
        private string ValidarActualizarEvaluacionSolicitudEdi(int rerevacodi, string[][] dataht, string usuario)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                bool esRequeridoRerevacodi = (rerevacodi < 1);
                if (esRequeridoRerevacodi)
                {
                    sb.Append("Id de Evaluación es requerido");
                    return sb.ToString();
                }

                RerEvaluacionDTO evaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetById(rerevacodi);
                bool existeEvaluacion = (evaluacion != null);
                if (!existeEvaluacion)
                {
                    sb.Append("No existe una evaluación con id = " + rerevacodi);
                    return sb.ToString();
                }

                bool noExisteUsuario = string.IsNullOrWhiteSpace(usuario);
                if (noExisteUsuario)
                {
                    sb.Append("El usuario no existe");
                    return sb.ToString();
                }

                bool existeDataht = (dataht != null);
                if (!existeDataht)
                {
                    sb.Append("Los datos del handsontable son requeridos.");
                    return sb.ToString();
                }

                int fila = 1;
                foreach (string[] data in dataht)
                {
                    DateTime? dtFechaHoraInicio = null;
                    DateTime? dtFechaHoraFin = null;

                    bool noExisteFechaHoraInicio = string.IsNullOrWhiteSpace(data[4]);
                    if (noExisteFechaHoraInicio)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe una fecha y hora de inicio para la fila {0}", fila);
                    }
                    else
                    {
                        try
                        {
                            dtFechaHoraInicio = DateTime.ParseExact(data[4], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("No es válida la fecha y hora de inicio para la fila {0}", fila);
                        }
                    }

                    bool noExisteFechaHoraFin = string.IsNullOrWhiteSpace(data[5]);
                    if (noExisteFechaHoraFin)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe una fecha y hora de fin para la fila {0}", fila);
                    }
                    else
                    {
                        try
                        {
                            dtFechaHoraFin = DateTime.ParseExact(data[5], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("No es válida la fecha y hora de fin para la fila {0}", fila);
                        }
                    }

                    bool procederCompararFechas = (dtFechaHoraInicio != null && dtFechaHoraFin != null);
                    if (procederCompararFechas)
                    {
                        int compararFechas = DateTime.Compare(dtFechaHoraInicio.Value, dtFechaHoraFin.Value);
                        bool esFechaHoraInicioMayorOIgualFechaHoraFin = (compararFechas >= 0);
                        if (esFechaHoraInicioMayorOIgualFechaHoraFin)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("La 'fecha y hora de inicio' debe ser menor a la 'fecha y hora de fin' para la fila {0}", fila);
                        }
                    }

                    bool noExisteDesc = string.IsNullOrWhiteSpace(data[7]);
                    if (noExisteDesc)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe el detalle para la fila {0}", fila);
                    }

                    fila++;
                }

                if (sb.ToString().Length > 0) { sb.Append("."); }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Formatea RerEvaluacion
        /// </summary>
        /// <param name="reg"></param>
        private static void FormatearRerEvaluacion(RerEvaluacionDTO reg)
        {
            reg.RerevaestadoDesc = (string.IsNullOrEmpty(reg.Rerevaestado)) ? "" : ((reg.Rerevaestado == ConstantesPrimasRER.estadoGenerado) ? ConstantesPrimasRER.descEstadoGenerado : ConstantesPrimasRER.descEstadoValidado);
            reg.RerevafeccreacionDesc = (reg.Rerevafeccreacion != null) ? reg.Rerevafeccreacion.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
            reg.RerevafecmodificacionDesc = (reg.Rerevafecmodificacion != null) ? reg.Rerevafecmodificacion.ToString(ConstantesAppServicio.FormatoFechaFull2) : "";
        }

        #endregion

        #region Comparativos

        /// <summary>
        /// Carga una lista con los siguientes datos de las solicitudes EDI: 
        /// id = Id de la evaluación de la solicitud EDI (reresecodi)
        /// value = id de la solicitud EDI y nombre de la central de la solicitud EDI.
        /// Con respecto a la última versión de una revisión
        /// </summary>
        /// <param name="rerrevcodi"></param>
        /// <param name="rerEvaluacion"></param>
        /// <returns></returns>
        public List<GenericoDTO> CargarSolicitudesEdi(int rerrevcodi, out RerEvaluacionDTO rerEvaluacion)
        {
            try
            {
                List<GenericoDTO> list = new List<GenericoDTO>();

                rerEvaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetByRevisionAndLastNumVersion(rerrevcodi);
                bool esNuloRerEvaluacion = (rerEvaluacion == null);
                if (esNuloRerEvaluacion)
                {
                    return list;
                }

                List<RerEvaluacionSolicitudEdiDTO> listESE = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetByCriteria(rerEvaluacion.Rerevacodi, -1);
                bool existeListESE = (listESE != null && listESE.Count > 0);
                if (!existeListESE)
                {
                    return list;
                }

                listESE = listESE.Where(x => x.Rereseeliminado.ToUpper() == ConstantesPrimasRER.eliminadoNo).ToList();
                existeListESE = (listESE != null && listESE.Count > 0);
                if (!existeListESE)
                {
                    return list;
                }

                listESE = listESE.OrderBy(x => x.Equinomb).ThenBy(x => x.Emprnomb).ThenBy(x => x.Reresefechahorainicio).ThenBy(x => x.Reresefechahorafin).ToList();
                foreach (var ese in listESE)
                {
                    list.Add(new GenericoDTO() { Entero1 = ese.Reresecodi, String1 = ese.Rersedcodi + " - " + ese.Equinomb });
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtener listado de evalución de energía de unidad para una evaluación y una solicitud EDI
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <param name="rerevacodi"></param>
        /// <returns></returns>
        public List<RerEvaluacionEnergiaUnidadDTO> ObtenerListadoEvaluacionEnergiaUnidad(int reresecodi, int rerevacodi)
        {
            try
            {
                List<RerEvaluacionEnergiaUnidadDTO> listEEU = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetByCriteria(reresecodi, rerevacodi);
                bool existeListEEU = (listEEU != null && listEEU.Count > 0);
                if (existeListEEU)
                {
                    listEEU = listEEU.OrderBy(x => x.Equinomb).ToList();
                }

                return listEEU;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Generar Html del listado de evalución de energía de unidad para una evaluación y una solicitud EDI
        /// y los datos de su gráfico lineal web
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="rerccbcodi"></param>
        /// <param name="energia_estimada_unidad"></param>
        /// <param name="energia_solicitada_unidad"></param>
        /// <param name="tablaHtml"></param>
        /// <param name="graficoWeb"></param> 
        /// <returns></returns>
        public void GenerarHtmlEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi,
            out int rerccbcodi, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad, out string tablaHtml, out GraficoWeb graficoWeb)
        {
            try
            {
                #region Obtener datos de la tabla del cuadro comparativo para una Evaluación Energía Unidad
                ObtenerDatosComparativoEvaluacionEnergiaUnidad(
                    rerevacodi, reresecodi, rereeucodi, ConstantesPrimasRER.numeroMenosTres, ConstantesPrimasRER.numero3, null,
                    out rerccbcodi, out decimal energia_estimada_15_min, out energia_estimada_unidad, out energia_solicitada_unidad,
                    out RerEvaluacionSolicitudEdiDTO ese, out RerEvaluacionEnergiaUnidadDTO eeu, out List<string> xAxisCategories, out List<RerComparativoDetDTO> listCDT);
                #endregion

                #region Generar html 
                tablaHtml = GenerarTableHtmlEvaluacionEnergiaUnidad(listCDT);
                #endregion

                #region Generar gráfico web lineal
                graficoWeb = GenerarGraficoWebEvaluacionEnergiaUnidad(ese, eeu, xAxisCategories, listCDT);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Generar un archivo Excel con la información de la tabla de energía de una unidad de generación de una central de una solicitud de una evaluación
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelTablaEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi)
        {
            try
            {
                #region Obtener datos de la tabla del cuadro comparativo para una Evaluación Energía Unidad
                ObtenerDatosComparativoEvaluacionEnergiaUnidad(
                    rerevacodi, reresecodi, rereeucodi, ConstantesPrimasRER.numero0, ConstantesPrimasRER.numero0, null,
                    out int rerccbcodi, out decimal energia_estimada_15_min, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad,
                    out RerEvaluacionSolicitudEdiDTO ese, out RerEvaluacionEnergiaUnidadDTO eeu, out List<string> xAxisCategories, out List<RerComparativoDetDTO> listCDT);
                #endregion

                #region Obtener fechas y horas de inicio y fin ajustadas
                ObtenerFechasAjustadas(ese.Reresefechahorainicio, ese.Reresefechahorafin, out DateTime eseFechaHoraInicioAjustado, out DateTime eseFechaHoraFinAjustado);
                #endregion

                #region Armar data del archivo Excel
                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[1];
                listaCabeceras[0] = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Fecha"),
                    CrearExcelModelo("Hora"),
                    CrearExcelModelo("Medidores \n con FP MWh"),
                    CrearExcelModelo("Energía \n Solicitada"),
                    CrearExcelModelo("Energía \n Estimada"),
                    CrearExcelModelo("% \n Desviación"),
                };
                List<int> listaAnchoColumna = new List<int>
                {
                    15, //Fecha
                    15, //Hora
                    15, //Medidores \n con FP MWh
                    15, //Energía \n Solicitada
                    15, //Energía \n Estimada
                    15  //% \n Desviación
                };
                #endregion

                #region Cuerpo
                RerExcelCuerpo cuerpo = null;
                List<string>[] tablaCuerpo = new List<string>[] { };
                List<string> listaAlineaHorizontal = new List<string>
                {
                    "center", //Fecha
                    "center", //Hora
                    "center", //Medidores \n con FP MWh
                    "center", //Energía \n Solicitada
                    "center", //Energía \n Estimada
                    "center"  //% \n Desviación
                };
                List<string> listaTipo = new List<string>
                {
                    "string", //Fecha
                    "string", //Hora
                    "double", //Medidores \n con FP MWh
                    "double", //Energía \n Solicitada
                    "double", //Energía \n Estimada
                    "double"  //% \n Desviación
                };

                bool existeListCDT = (listCDT != null && listCDT.Count > 0);
                if (existeListCDT)
                {
                    int i = 0;
                    foreach (var cdt in listCDT)
                    {
                        DateTime cdtFechaHora = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdt.Rercdtfecha.Day.ToString("D2"), cdt.Rercdtfecha.Month.ToString("D2"), cdt.Rercdtfecha.Year, cdt.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        int compararDtInicio = DateTime.Compare(cdtFechaHora, eseFechaHoraInicioAjustado);
                        int compararDtFin = DateTime.Compare(cdtFechaHora, eseFechaHoraFinAjustado);
                        //bool perteneceAlRango = (compararDtInicio >= 0 && compararDtFin < 0); //Hay q evitar tomar un registro demás. Ejemplo: si termina en 12:45, entonces el último es 12:30, pq representa a 12:30-12:45
                        bool perteneceAlRango = (compararDtInicio > 0 && compararDtFin <= 0); //Hay q evitar tomar un registro demás. Ejemplo: si empieza en 00:01, entonces el primero es 00:15, pq representa a 00:01-00:15
                        if (perteneceAlRango)
                        {
                            List<string> tFila = new List<string>
                            {
                                cdt.Rercdtfecha.ToString(ConstantesAppServicio.FormatoFechaWS),
                                cdt.Rercdthora,
                                (cdt.Rercdtmedfpm != null ? cdt.Rercdtmedfpm.Value.ToString() : ""),
                                (cdt.Rercdtenesolicitada != null ? cdt.Rercdtenesolicitada.Value.ToString() : ""),
                                (cdt.Rercdteneestimada != null ? cdt.Rercdteneestimada.Value.ToString() : ""),
                                (cdt.Rercdtpordesviacion != null ? cdt.Rercdtpordesviacion.Value.ToString() : "")
                            };
                            Array.Resize(ref tablaCuerpo, tablaCuerpo.Length + 1);
                            tablaCuerpo[i] = tFila;
                            i++;
                        }
                    }
                    cuerpo = CrearExcelCuerpo(tablaCuerpo, listaAlineaHorizontal, listaTipo);
                }
                #endregion
                #endregion

                #region Return
                StringBuilder sbNombreHoja = new StringBuilder();
                sbNombreHoja.Append(ese.Rersedcodi);
                sbNombreHoja.Append(" - ");
                sbNombreHoja.Append(ese.Equinomb);
                sbNombreHoja.Append(" - ");
                sbNombreHoja.Append(eeu.Equinomb);

                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = sbNombreHoja.ToString(),
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);

                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Procesar los datos del archivo Excel con la información de la tabla de energía de una unidad de generación de una central de una solicitud de una evaluación
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="rutayNombreArchivo"></param>
        /// <param name="numErrores"></param>
        /// <param name="mensajeError"></param>
        /// <param name="energia_estimada_15_min"></param>
        /// <param name="energia_estimada_unidad"></param>
        /// <param name="energia_solicitada_unidad"></param>
        /// <param name="tablaHtml"></param>
        /// <param name="graficoWeb"></param>
        public void ProcesarArchivoExcelParaTablaEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi, string rutayNombreArchivo,
            out int numErrores, out string mensajeError, out decimal energia_estimada_15_min, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad,
            out string tablaHtml, out GraficoWeb graficoWeb)
        {
            try
            {
                #region Inicializando variables
                numErrores = 0;
                mensajeError = "";
                energia_estimada_15_min = 0;
                energia_estimada_unidad = 0;
                energia_solicitada_unidad = 0;
                tablaHtml = "";
                graficoWeb = null;
                #endregion

                #region Validar y leer datos del archivo Excel
                StringBuilder sbMensajeError = new StringBuilder();
                List<RerComparativoDetDTO> listCDTExcel = new List<RerComparativoDetDTO>();

                DataSet ds = new DataSet();
                ds = GeneraDataset(rutayNombreArchivo, 1); //Leer la primera hoja del archivo Excel
                int fila = 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dtRow = ds.Tables[0].Rows[i];
                    int numFila = fila + 1;
                    fila++;

                    #region Validar Fecha
                    DateTime dtFecha = new DateTime();
                    string fecha = dtRow[0].ToString();
                    bool esNuloOVacioFecha = (string.IsNullOrWhiteSpace(fecha));
                    if (esNuloOVacioFecha)
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La fecha no existe.", numFila);
                        numErrores++;
                        continue;
                    }

                    try
                    {
                        dtFecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La fecha no es válida.", numFila);
                        numErrores++;
                        continue;
                    }
                    #endregion

                    #region Validar Hora
                    string hhmm = dtRow[1].ToString();
                    bool esNuloOVacioHhmm = (string.IsNullOrWhiteSpace(hhmm));
                    if (esNuloOVacioHhmm)
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La hora no existe.", numFila);
                        numErrores++;
                        continue;
                    }

                    string[] aHora = hhmm.Split(':');
                    bool esLonguitudValida = (aHora.Length == 2);
                    if (!esLonguitudValida)
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La hora no es válida.", numFila);
                        numErrores++;
                        continue;
                    }

                    bool esLonguitudIguala2_0 = (aHora[0].Length == 2);
                    bool esLonguitudIguala2_1 = (aHora[1].Length == 2);
                    if (!esLonguitudIguala2_0 || !esLonguitudIguala2_1)
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La hora no es válida.", numFila);
                        numErrores++;
                        continue;
                    }

                    try
                    {
                        Convert.ToInt32(aHora[0]);
                        Convert.ToInt32(aHora[1]);
                    }
                    catch
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La hora no es válida.", numFila);
                        numErrores++;
                        continue;
                    }
                    #endregion

                    #region Energía Estimada
                    decimal dEnegiaEstimada = 0;
                    string valorEnergiaEstimada = dtRow[4].ToString();
                    bool esNuloOVacioEnergiaEstimada = (string.IsNullOrWhiteSpace(valorEnergiaEstimada));
                    if (esNuloOVacioEnergiaEstimada)
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La energía estimada no existe.", numFila);
                        numErrores++;
                        continue;
                    }

                    try
                    {
                        dEnegiaEstimada = Convert.ToDecimal(valorEnergiaEstimada);
                    }
                    catch
                    {
                        sbMensajeError.AppendFormat("<br> Fila: {0} - La energía estimada no es válida.", numFila);
                        numErrores++;
                        continue;
                    }
                    #endregion

                    #region Añadir fila a listCDTExcel
                    RerComparativoDetDTO cdtExcel = new RerComparativoDetDTO()
                    {
                        Rercdtfecha = dtFecha.Date,
                        Rercdthora = hhmm,
                        Rercdteneestimada = dEnegiaEstimada,
                        Rercdtpordesviacion = null,
                    };

                    listCDTExcel.Add(cdtExcel);
                    #endregion
                }
                mensajeError = sbMensajeError.ToString();
                bool existeMensajeError = (mensajeError.Length > 0);
                if (existeMensajeError)
                {
                    return;
                }

                RerEnergiaEstimada energiaEstimada = new RerEnergiaEstimada()
                {
                    OrigenDatos = ConstantesPrimasRER.origenDatosArchivoExcel,
                    ListCDTExcel = listCDTExcel
                };
                #endregion

                #region Obtener datos de la tabla del cuadro comparativo para una Evaluación Energía Unidad
                ObtenerDatosComparativoEvaluacionEnergiaUnidad(
                    rerevacodi, reresecodi, rereeucodi, ConstantesPrimasRER.numeroMenosTres, ConstantesPrimasRER.numero3, energiaEstimada,
                    out int rerccbcodi, out energia_estimada_15_min, out energia_estimada_unidad, out energia_solicitada_unidad,
                    out RerEvaluacionSolicitudEdiDTO ese, out RerEvaluacionEnergiaUnidadDTO eeu, out List<string> xAxisCategories, out List<RerComparativoDetDTO> listCDT);
                #endregion

                #region Generar html 
                tablaHtml = GenerarTableHtmlEvaluacionEnergiaUnidad(listCDT);
                #endregion

                #region Generar gráfico web lineal
                graficoWeb = GenerarGraficoWebEvaluacionEnergiaUnidad(ese, eeu, xAxisCategories, listCDT);
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Procesar los datos del valor típico con la información de la tabla de energía de una unidad de generación de una central de una solicitud de una evaluación
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="fecha_inicio">Formato: dd/MM/yyyy</param>
        /// <param name="hora_inicio"></param>
        /// <param name="fecha_fin">Formato: dd/MM/yyyy</param>
        /// <param name="hora_fin"></param>
        /// <param name="energia_estimada_15_min"></param>
        /// <param name="energia_estimada_unidad"></param>
        /// <param name="energia_solicitada_unidad"></param>
        /// <param name="tablaHtml"></param>
        /// <param name="graficoWeb"></param>
        public void ProcesarValorTipicoParaTablaEvaluacionEnergiaUnidad(int rerevacodi, int reresecodi, int rereeucodi,
            string fecha_inicio, string hora_inicio, string fecha_fin, string hora_fin,
            out decimal energia_estimada_15_min, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad,
            out string tablaHtml, out GraficoWeb graficoWeb)
        {
            try
            {
                #region Inicializando variables
                energia_estimada_15_min = 0;
                energia_estimada_unidad = 0;
                energia_solicitada_unidad = 0;
                tablaHtml = "";
                graficoWeb = null;
                #endregion

                #region Validar y armar datos del Valor Típico
                ValidarDatosValorTipico(fecha_inicio, hora_inicio, fecha_fin, hora_fin);

                RerValorTipico valorTipico = new RerValorTipico()
                {
                    FechaInicio = DateTime.ParseExact(fecha_inicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                    HoraInicio = hora_inicio,
                    FechaFin = DateTime.ParseExact(fecha_fin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                    HoraFin = hora_fin,
                };

                RerEnergiaEstimada energiaEstimada = new RerEnergiaEstimada()
                {
                    OrigenDatos = ConstantesPrimasRER.origenDatosValorTipico,
                    ValorTipico = valorTipico
                };
                #endregion

                #region Obtener datos de la tabla del cuadro comparativo para una Evaluación Energía Unidad
                ObtenerDatosComparativoEvaluacionEnergiaUnidad(
                    rerevacodi, reresecodi, rereeucodi, ConstantesPrimasRER.numeroMenosTres, ConstantesPrimasRER.numero3, energiaEstimada,
                    out int rerccbcodi, out energia_estimada_15_min, out energia_estimada_unidad, out energia_solicitada_unidad,
                    out RerEvaluacionSolicitudEdiDTO ese, out RerEvaluacionEnergiaUnidadDTO eeu, out List<string> xAxisCategories, out List<RerComparativoDetDTO> listCDT);
                #endregion

                #region Generar html 
                tablaHtml = GenerarTableHtmlEvaluacionEnergiaUnidad(listCDT);
                #endregion

                #region Generar gráfico web lineal
                graficoWeb = GenerarGraficoWebEvaluacionEnergiaUnidad(ese, eeu, xAxisCategories, listCDT);
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtener total de la energía estimada de una central RER de una evaluación de solicitud EDI 
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <returns></returns>
        public decimal ObtenerTotalEnergiaEstimdaCentral(int reresecodi)
        {
            try
            {
                #region Validación Básica
                bool esInvalido = (reresecodi < 1);
                if (esInvalido)
                {
                    throw new Exception("Reresecodi debe ser mayor a cero");
                }
                #endregion

                #region Obtener total de energía estimada de una central RER
                RerEvaluacionSolicitudEdiDTO ese = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
                bool existeESE = (ese != null);
                if (!existeESE)
                {
                    throw new Exception("No existe un registro en rer_evaluacion_solicitudedi con id = " + reresecodi);
                }

                decimal value = (ese.Reresetotenergiaestimada != null) ? ese.Reresetotenergiaestimada.Value : 0;
                return value;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Calcular el nuevo total de la energía estimada de una central RER de una evaluación de solicitud EDI
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <param name="rerccbcodi"></param>
        /// <param name="nueva_energia_estimada_unidad"></param>
        /// <returns></returns>
        public decimal CalcularNuevoTotalEnergiaEstimdaCentral(int reresecodi, int rerccbcodi, decimal nueva_energia_estimada_unidad)
        {
            try
            {
                #region Validación Básica
                bool esInvalidoReresecodi = (reresecodi < 1);
                if (esInvalidoReresecodi)
                {
                    throw new Exception("Reresecodi debe ser mayor a cero");
                }

                bool esInvalidoRerccbcodi = (rerccbcodi < 0);
                if (esInvalidoRerccbcodi)
                {
                    throw new Exception("Rerccbcodi debe ser mayor o igual a cero");
                }
                #endregion

                #region Obtener total de energía estimada de una central RER
                RerEvaluacionSolicitudEdiDTO ese = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
                bool existeESE = (ese != null);
                if (!existeESE)
                {
                    throw new Exception("No existe un registro en rer_evaluacion_solicitudedi con id = " + reresecodi);
                }
                #endregion

                #region Obtener total de energía estimada de una unidad generadora de una central RER ubicada en rer_comparativo_cab 
                decimal rerccbtoteneestimada = 0;
                bool esMayoraCero = (rerccbcodi > 0);
                if (esMayoraCero)
                {
                    RerComparativoCabDTO ccb = FactoryTransferencia.GetRerComparativoCabRepository().GetById(rerccbcodi);
                    bool existeCCB = (ccb != null);
                    if (!existeCCB)
                    {
                        throw new Exception("No existe un registro en rer_comparativo_cab con id = " + rerccbcodi);
                    }
                    rerccbtoteneestimada = ccb.Rerccbtoteneestimada;
                }
                #endregion

                #region Calcular nuevo total de energía estimada de una central RER
                decimal reresetotenergiaestimada = (ese.Reresetotenergiaestimada != null) ? ese.Reresetotenergiaestimada.Value : 0;
                decimal nuevaReresetotenergiaestimada = Math.Round(reresetotenergiaestimada - rerccbtoteneestimada + nueva_energia_estimada_unidad, ConstantesPrimasRER.numero4);
                return nuevaReresetotenergiaestimada;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Guardar comparativo para una unidad generadora de una central de una solicitud EDI de una evaluación
        /// Retorna el rerccbcodi al cual se realizó la actualización, es decir, el insert o update
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="rerccbcodi"></param>
        /// <param name="reresetotenergiaestimada"></param>
        /// <param name="rerccbtoteneestimada"></param>
        /// <param name="rerccbtotenesolicitada"></param>
        /// <param name="datosTabla"></param>
        /// <param name="rerccboridatos"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GuardarComparativo(int rerevacodi, int reresecodi, int rereeucodi, int rerccbcodi, decimal reresetotenergiaestimada, decimal rerccbtoteneestimada, decimal rerccbtotenesolicitada, string[][] datosTabla, string rerccboridatos, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validación
                bool esValidoRerccbcodi = rerccbcodi > -1;
                if (!esValidoRerccbcodi)
                {
                    throw new Exception("No es válido el Id de Comparativo.");
                }

                bool existeDatosTabla = (datosTabla != null && datosTabla.Length > 0);
                if (!existeDatosTabla)
                {
                    throw new Exception("Los datos de tabla son requeridos.");
                }

                bool noExisteRerccboridatos = (string.IsNullOrEmpty(rerccboridatos) || rerccboridatos.Trim() == "");
                if (noExisteRerccboridatos)
                {
                    throw new Exception("Origen de datos es requerido.");
                }

                rerccboridatos = rerccboridatos.Trim().ToUpper();
                bool esValidoRerccboridatos = (rerccboridatos == ConstantesPrimasRER.origenDatosValorTipico || rerccboridatos == ConstantesPrimasRER.origenDatosArchivoExcel);
                if (!esValidoRerccboridatos)
                {
                    throw new Exception("Origen de datos no es válido.");
                }

                RerEvaluacionSolicitudEdiDTO ese = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
                bool existeEse = (ese != null);
                if (!existeEse)
                {
                    throw new Exception("No existe un registro en rer_evaluacion_solicitud_edi para el id = " + reresecodi);
                }

                RerComparativoCabDTO comparativoCab = new RerComparativoCabDTO();
                bool existeComparativoCab = false;
                bool esMayoraCeroRerccbcodi = (rerccbcodi > 0);
                if (esMayoraCeroRerccbcodi)
                {
                    comparativoCab = FactoryTransferencia.GetRerComparativoCabRepository().GetById(rerccbcodi);
                    existeComparativoCab = (comparativoCab != null);
                    if (!existeComparativoCab)
                    {
                        throw new Exception("No existe un registro en rer_comparativo_cab para el id = " + rerccbcodi);
                    }

                    bool esValidoComparativoCab = (comparativoCab.Rerevacodi == rerevacodi && comparativoCab.Reresecodi == reresecodi && comparativoCab.Rereeucodi == rereeucodi);
                    if (!esValidoComparativoCab)
                    {
                        throw new Exception("No es válido los valores relacionados a rer_comparativo_cab para el id = " + rerccbcodi);
                    }
                }
                #endregion

                #region Guardar datos de RerEvaluacionSolicitudEdi, ComparativoCab y ComparativoDet
                DateTime fechaHoy = DateTime.Now;
                conn = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().StartTransaction(conn);

                ese.Reresetotenergiaestimada = reresetotenergiaestimada;
                ese.Rereseediaprobada = reresetotenergiaestimada;
                ese.Rereseusumodificacion = usuario;
                ese.Reresefecmodificacion = fechaHoy;
                FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().UpdateEnergias(ese, conn, tran);

                DateTime fechaCabHoy = DateTime.Now;
                if (existeComparativoCab)
                {
                    comparativoCab.Rerccboridatos = rerccboridatos;
                    comparativoCab.Rerccbtotenesolicitada = rerccbtotenesolicitada;
                    comparativoCab.Rerccbtoteneestimada = rerccbtoteneestimada;
                    comparativoCab.Rerccbusumodificacion = usuario;
                    comparativoCab.Rerccbfecmodificacion = fechaCabHoy;
                    FactoryTransferencia.GetRerComparativoCabRepository().Update(comparativoCab, conn, tran);

                    FactoryTransferencia.GetRerComparativoDetRepository().DeleteByRerccbcodi(rerccbcodi, conn, tran);
                }
                else
                {
                    rerccbcodi = FactoryTransferencia.GetRerComparativoCabRepository().GetMaxId();
                    comparativoCab = new RerComparativoCabDTO
                    {
                        Rerccbcodi = rerccbcodi,
                        Rerevacodi = rerevacodi,
                        Reresecodi = reresecodi,
                        Rereeucodi = rereeucodi,
                        Rerccboridatos = rerccboridatos,
                        Rerccbtotenesolicitada = rerccbtotenesolicitada,
                        Rerccbtoteneestimada = rerccbtoteneestimada,
                        Rerccbusucreacion = usuario,
                        Rerccbfeccreacion = fechaCabHoy,
                        Rerccbusumodificacion = usuario,
                        Rerccbfecmodificacion = fechaCabHoy
                    };
                    FactoryTransferencia.GetRerComparativoCabRepository().Save(comparativoCab, conn, tran);
                }

                int rercdtcodi = FactoryTransferencia.GetRerComparativoDetRepository().GetMaxId();
                foreach (var reg in datosTabla)
                {
                    string Rercdtflag = "";
                    decimal? Rercdtmedfpm = null;
                    decimal? Rercdtenesolicitada = null;
                    decimal? Rercdteneestimada = null;
                    decimal? Rercdtpordesviacion = null;

                    if (!string.IsNullOrEmpty(reg[2]) && reg[2].Trim() != "") { Rercdtmedfpm = Convert.ToDecimal(reg[2]); }
                    if (!string.IsNullOrEmpty(reg[3]) && reg[3].Trim() != "") { Rercdtenesolicitada = Convert.ToDecimal(reg[3]); }
                    if (!string.IsNullOrEmpty(reg[4]) && reg[4].Trim() != "")
                    {
                        Rercdteneestimada = Convert.ToDecimal(reg[4]);
                        Rercdtflag = ConstantesPrimasRER.flagDentroRango;
                    }
                    else
                    {
                        Rercdtflag = ConstantesPrimasRER.flagFueraRango;
                    }
                    if (!string.IsNullOrEmpty(reg[5]) && reg[5].Trim() != "") { Rercdtpordesviacion = Convert.ToDecimal(reg[5]); }

                    DateTime fechaDetHoy = DateTime.Now;
                    RerComparativoDetDTO comparativoDet = new RerComparativoDetDTO()
                    {
                        Rercdtcodi = rercdtcodi,
                        Rerccbcodi = comparativoCab.Rerccbcodi,
                        Rerevacodi = comparativoCab.Rerevacodi,
                        Reresecodi = comparativoCab.Reresecodi,
                        Rereeucodi = comparativoCab.Rereeucodi,
                        Rercdtfecha = DateTime.ParseExact(reg[0], ConstantesAppServicio.FormatoFechaWS, CultureInfo.InvariantCulture),
                        Rercdthora = reg[1],
                        Rercdtmedfpm = Rercdtmedfpm,
                        Rercdtenesolicitada = Rercdtenesolicitada,
                        Rercdteneestimada = Rercdteneestimada,
                        Rercdtpordesviacion = Rercdtpordesviacion,
                        Rercdtflag = Rercdtflag,
                        Rercdtusucreacion = usuario,
                        Rercdtfeccreacion = fechaDetHoy,
                        Rercdtusumodificacion = usuario,
                        Rercdtfecmodificacion = fechaDetHoy
                    };

                    FactoryTransferencia.GetRerComparativoDetRepository().Save(comparativoDet, conn, tran);
                    rercdtcodi++;
                }

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                bool isConnectionOpen = (conn != null && conn.State == ConnectionState.Open);
                if (isConnectionOpen)
                {
                    conn.Close();
                }
            }

            return rerccbcodi;
        }

        /// <summary>
        /// Obtener el valor de la Energia de Medidores de Generacion de la Central para una fecha y hora específica
        /// NOTA: el parámetro rercdtfecha de tener sólo la fecha, es decir, su hora y minuto debe ser 00:00
        /// </summary>
        /// <param name="rercdtfecha"></param>
        /// <param name="rercdthora"></param>
        /// <param name="listEMGC"></param>
        /// <returns></returns>
        private decimal GetValorEnergiaDeMedidoresDeGeneracionDeLaCentral(DateTime rercdtfecha, string rercdthora, List<MeMedicion96DTO> listEMGC)
        {
            bool existeListEMGC = (listEMGC != null && listEMGC.Count > 0);
            if (!existeListEMGC)
            {
                return 0;
            }

            string hhmm = rercdtfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
            bool esCero = (hhmm == "00:00");
            if (!esCero)
            {
                throw new Exception("La hora y los minutos de la fecha debe ser 00:00. La fecha enviada es " + rercdtfecha);
            }

            DateTime mediFechaHora = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", 
                rercdtfecha.Day.ToString("D2"), rercdtfecha.Month.ToString("D2"), rercdtfecha.Year, rercdthora), 
                ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture).AddMinutes(ConstantesPrimasRER.numeroMenosQuince);
            
            DateTime mediFecha = mediFechaHora.Date;
            string mediHhmm = mediFechaHora.ToString(ConstantesAppServicio.FormatoOnlyHora);
            string[] mediHhmmArray = mediHhmm.Split(':');
            int mediHh = int.Parse(mediHhmmArray[0]);
            int mediMm = int.Parse(mediHhmmArray[1]);
            int mediIndex = ((mediHh * ConstantesPrimasRER.numero4 * ConstantesPrimasRER.numero15 + mediMm) / ConstantesPrimasRER.numero15) + ConstantesPrimasRER.numero1;

            MeMedicion96DTO meMedicion96;
            try
            {
                meMedicion96 = listEMGC.Where(x => x.Medifecha.Value.Date == mediFecha.Date).First();
            }
            catch
            {   //En caso no exista listEMGC o no exista un registro para el filtro de fecha. Entonces, se devolverá 0
                return 0;
            }

            object value = meMedicion96.GetType().GetProperty("H" + mediIndex).GetValue(meMedicion96);
            decimal? valor = (value != null) ? (decimal)value : 0;

            //Dictionary<string, decimal?> dValoresMeMedicion96 = new Dictionary<string, decimal?>();
            //for (int i = 1; i <= 96; i++)
            //{
            //    hhmm = mediFecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
            //    object value = meMedicion96.GetType().GetProperty("H" + i).GetValue(meMedicion96);
            //    dValoresMeMedicion96.Add(hhmm, (decimal)value);
            //    mediFecha = mediFecha.AddMinutes(ConstantesPrimasRER.numero15);
            //}

            //bool existeHoraMinutos = dValoresMeMedicion96.ContainsKey(mediHhmm);
            //if (!existeHoraMinutos)
            //{
            //    return 0;
            //}

            //decimal? valor = dValoresMeMedicion96[mediHhmm];
            //bool esNuloValor = (valor == null);
            //if (esNuloValor)
            //{
            //    return 0;
            //}

            return valor.Value;
        }

        /// <summary>
        /// Obtener el valor del Factor de Pérdidas Medias de la Central para una central y una fecha específica
        /// </summary>
        /// <param name="EquicodiCentral"></param>
        /// <param name="soloFecha"></param>
        /// <param name="listFPMD"></param>
        /// <returns></returns>
        private decimal GetValorFactorDePerdidasMediasDeLaCentral(int EquicodiCentral, DateTime soloFecha, List<RerFacPerMedDetDTO> listFPMD)
        {
            List<RerFacPerMedDetDTO> list;
            try
            {
                list = listFPMD.Where(x => x.Equicodi == EquicodiCentral && x.Rerfpmdesde <= soloFecha.Date && x.Rerfpmhasta >= soloFecha.Date).ToList();
                bool existeList = (list != null && list.Count > 0);
                if (!existeList)
                {
                    return 0;
                }
            }
            catch
            {   //En caso no exista listFPMF. Entonces, se devolverá 0
                return 0;
            }

            bool existeMasDeUno = (list.Count > 1);
            if (existeMasDeUno)
            {
                throw new Exception("Existe más de un Factor de Pérdida Media para la central de id = " + EquicodiCentral + " y para la fecha = " + soloFecha.Date.ToString(ConstantesBase.FormatoFechaBase));
            }

            return list.First().Rerfpdfactperdida;
        }

        /// <summary>
        /// Generar tabla Html del listado de evalución de energía de unidad para una evaluación y una solicitud EDI
        /// </summary>
        /// <param name="listCDT"></param>
        /// <returns></returns>
        private string GenerarTableHtmlEvaluacionEnergiaUnidad(List<RerComparativoDetDTO> listCDT)
        {
            #region tabla
            StringBuilder sbTablaHtml = new StringBuilder();
            sbTablaHtml.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_evaluacion_energia_unidad'>");
            #region cabecera
            sbTablaHtml.Append("<thead>");
            sbTablaHtml.Append("<tr>");
            sbTablaHtml.Append("<th style=''>Fecha</th>");
            sbTablaHtml.Append("<th style=''>Hora</th>");
            sbTablaHtml.Append("<th style=''>Medidores<br/>con FP MWh</th>");
            sbTablaHtml.Append("<th style=''>Energía<br/>Solicitada</th>");
            sbTablaHtml.Append("<th style=''>Energía<br/>Estimada</th>");
            sbTablaHtml.Append("<th style=''>%<br/>Desviación</th>");
            sbTablaHtml.Append("</tr>");
            sbTablaHtml.Append("</thead>");
            #endregion
            #region cuerpo
            sbTablaHtml.Append("<tbody>");
            foreach (var reg in listCDT)
            {
                sbTablaHtml.Append("<tr>");
                sbTablaHtml.AppendFormat("<td style=''>{0}</td>", reg.Rercdtfecha.ToString(ConstantesAppServicio.FormatoFechaWS));
                sbTablaHtml.AppendFormat("<td style=''>{0}</td>", reg.Rercdthora);
                sbTablaHtml.AppendFormat("<td style=''>{0}</td>", (reg.Rercdtmedfpm != null ? reg.Rercdtmedfpm.Value.ToString("0.###0") : ""));
                sbTablaHtml.AppendFormat("<td style=''>{0}</td>", (reg.Rercdtenesolicitada != null ? reg.Rercdtenesolicitada.Value.ToString("0.###0") : ""));
                sbTablaHtml.AppendFormat("<td style=''>{0}</td>", (reg.Rercdteneestimada != null ? reg.Rercdteneestimada.Value.ToString("0.###0") : ""));
                sbTablaHtml.AppendFormat("<td style=''>{0}</td>", (reg.Rercdtpordesviacion != null ? reg.Rercdtpordesviacion.Value.ToString("0.###0") : ""));
                sbTablaHtml.Append("</tr>");
            }
            sbTablaHtml.Append("</tbody>");
            #endregion
            sbTablaHtml.Append("</table>");
            #endregion
            return sbTablaHtml.ToString();
        }

        /// <summary>
        /// Generar data para el gráfico líneal del listado de evalución de energía de unidad para una evaluación y una solicitud EDI
        /// </summary>
        /// <param name="ese"></param>
        /// <param name="eeu"></param>
        /// <param name="xAxisCategories"></param>
        /// <param name="listCDT"></param>
        /// <returns></returns>
        private GraficoWeb GenerarGraficoWebEvaluacionEnergiaUnidad(RerEvaluacionSolicitudEdiDTO ese, RerEvaluacionEnergiaUnidadDTO eeu,
            List<string> xAxisCategories, List<RerComparativoDetDTO> listCDT)
        {
            StringBuilder sbTitulo = new StringBuilder();
            sbTitulo.Append(ese.Rersedcodi);
            sbTitulo.Append(" - ");
            sbTitulo.Append(ese.Equinomb);
            sbTitulo.Append(" - ");
            sbTitulo.Append(eeu.Equinomb);

            GraficoWeb graficoWeb = new GraficoWeb()
            {
                Type = "column",
                XAxisCategories = xAxisCategories, //new List<string> { "01.01.23 01:00", "01.01.23 01:15", "01.01.23 01:30" }, 
                TitleText = sbTitulo.ToString(),
                YAxixTitle = new List<string>(), //{ "MWh" },
                TooltipValuePrefix = "",
                YaxixLabelsFormat = "",
                TooltipValueDecimals = 4,
                LegendLayout = "horizontal",
                LegendAlign = "center",
                LegendVerticalAlign = "bottom",
                XAxisTitle = "",
                YAxisStackLabels = false,
                PlotOptionsDataLabels = false
            };

            graficoWeb.SerieData = new DatosSerie[3];
            graficoWeb.SerieData[0] = new DatosSerie { Name = "Medidores con FP MWh", Data = new decimal?[listCDT.Count] };
            graficoWeb.SerieData[1] = new DatosSerie { Name = "Energía Solicitada", Data = new decimal?[listCDT.Count] };
            graficoWeb.SerieData[2] = new DatosSerie { Name = "Energía Estimada", Data = new decimal?[listCDT.Count] };

            int index = 0;
            foreach (var reg in listCDT)
            {
                graficoWeb.SerieData[0].Data[index] = reg.Rercdtmedfpm;
                graficoWeb.SerieData[1].Data[index] = reg.Rercdtenesolicitada;
                graficoWeb.SerieData[2].Data[index] = reg.Rercdteneestimada;
                index++;
            }

            return graficoWeb;
        }

        /// <summary>
        /// Obtener los datos de la tabla de la interfaz del cuadro comparativo de una evaluación de energía de unidad de generación 
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="reresecodi"></param>
        /// <param name="rereeucodi"></param>
        /// <param name="menosHoras">Debe ser un valor negativo o cero</param>
        /// <param name="masHoras">Debe ser un valor positivo o cero</param>
        /// <param name="energiaEstimada">Si es diferente de nulo, entonces incorpora la energía estimada a la listaCDT</param>
        /// <param name="rerccbcodi"></param>
        /// <param name="energia_estimada_15_min">Si listCDTExcel es difente de nulo, entonces devuelve la energía estimada promedio para un intervalo de 15 min</param>
        /// <param name="energia_estimada_unidad"></param>
        /// <param name="energia_solicitada_unidad"></param>
        /// <param name="ese"></param>
        /// <param name="eeu"></param>
        /// <param name="xAxisCategories"></param>
        /// <param name="listCDT"></param>
        private void ObtenerDatosComparativoEvaluacionEnergiaUnidad(
            int rerevacodi, int reresecodi, int rereeucodi, int menosHoras, int masHoras, RerEnergiaEstimada energiaEstimada,
            out int rerccbcodi, out decimal energia_estimada_15_min, out decimal energia_estimada_unidad, out decimal energia_solicitada_unidad,
            out RerEvaluacionSolicitudEdiDTO ese, out RerEvaluacionEnergiaUnidadDTO eeu, out List<string> xAxisCategories, out List<RerComparativoDetDTO> listCDT)
        {
            try
            {
                #region Validacion básica
                ese = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
                bool esNuloESE = (ese == null);
                if (esNuloESE)
                {
                    throw new Exception("No existe un registro en RerEvaluacionSolicitudEdi con reresecodi = " + reresecodi);
                }

                eeu = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetById(rereeucodi);
                bool esNuloEEU = (eeu == null);
                if (esNuloEEU)
                {
                    throw new Exception("No existe un registro en RerEvaluacionEnergiaUnidad con rereeucodi = " + rereeucodi);
                }

                List<RerEvaluacionEnergiaUnidDetDTO> listEEUDet = FactoryTransferencia.GetRerEvaluacionEnergiaUnidDetRepository().GetByCriteria(eeu.Rereeucodi.ToString());
                if (listEEUDet != null && listEEUDet.Count > 0)
                {
                    listEEUDet = listEEUDet.OrderBy(x => x.Rereedcodi).ToList();
                    foreach (var euudet in listEEUDet)
                    {
                        eeu.Rereeuenergiaunidad += euudet.Rereedenergiaunidad; 
                    }
                }
                #endregion

                #region Obtener datos
                rerccbcodi = 0;
                energia_estimada_15_min = 0;
                energia_estimada_unidad = 0;
                energia_solicitada_unidad = 0;
                xAxisCategories = new List<string>();
                string empresas = ese.Emprcodi.ToString();
                DateTime dtFechahorainicioMenosHoras = ese.Reresefechahorainicio.AddHours(menosHoras);
                DateTime dtFechahorafinMasHoras = ese.Reresefechahorafin.AddHours(masHoras);
                ObtenerFechasAjustadas(ese.Reresefechahorainicio, ese.Reresefechahorafin, out DateTime eseFechaHoraInicioAjustado, out DateTime eseFechaHoraFinAjustado);

                listCDT = FactoryTransferencia.GetRerComparativoDetRepository().GetByCriteria(rerevacodi, reresecodi, rereeucodi);
                bool existeListCDT = (listCDT != null && listCDT.Count > 0);
                if (existeListCDT)
                {
                    #region Obtener datos desde RerComparativoCab y RerComparativoDet
                    #region Verificar cabecera y setear Categorias X
                    listCDT = listCDT.OrderBy(x => x.Rercdtcodi).ToList();
                    rerccbcodi = listCDT[0].Rerccbcodi;
                    foreach (var cdt in listCDT)
                    {
                        #region Verificar que pertenecen a la misma cabecera
                        bool esDiferenteCabecera = (rerccbcodi != cdt.Rerccbcodi);
                        if (esDiferenteCabecera)
                        {
                            throw new Exception("Los registros de rer_comparativo_det pertenecen a diferentes registros en rer_comparativo_cab para rerevacodi = " + rerevacodi + " , reresecodi = " + reresecodi + " , rereeucodi = " + rereeucodi);
                        }
                        #endregion

                        #region Setear Categorias X
                        string fechaHora = cdt.Rercdtfecha.ToString(ConstantesAppServicio.FormatoFechaEjecutivo) + " " + cdt.Rercdthora; 
                        xAxisCategories.Add(fechaHora);
                        #endregion
                    }
                    #endregion

                    #region Setear Energía Estimada Unidad y Energía Solicitada Unidad
                    RerComparativoCabDTO comparativoCab = FactoryTransferencia.GetRerComparativoCabRepository().GetById(rerccbcodi);
                    bool existeComparativoCab = (comparativoCab != null);
                    if (!existeComparativoCab)
                    {
                        throw new Exception("No existe un registro en rer_comparativo_cab con id = " + rerccbcodi);
                    }

                    energia_estimada_unidad = comparativoCab.Rerccbtoteneestimada;
                    energia_solicitada_unidad = comparativoCab.Rerccbtotenesolicitada;
                    #endregion
                    #endregion
                }
                else
                {
                    #region Obtener datos desde RerEvaluacionEnergiaUnidad    
                    #region Obtener "Intervalos de 15 minutos"
                    List<DateTime> listaIntervalos = ObtenerIntervalosMinuto(dtFechahorainicioMenosHoras, dtFechahorafinMasHoras);
                    bool existeListaIntervalos = (listaIntervalos != null && listaIntervalos.Count > 0);
                    if (!existeListaIntervalos)
                    {
                        throw new Exception("No hay una lista de intervalos por minuto");
                    }
                    listaIntervalos.RemoveAt(0); //Hay q evitar tomar un registro demás. Ejemplo: si empieza en 00:01, entonces el primer intervalo es 00:15, pq representa a 00:01-00:15
                    #endregion

                    #region Obtener "Energía de la Unidad Generadora"
                    bool esNuloOVacioEnergiaUnidad = string.IsNullOrEmpty(eeu.Rereeuenergiaunidad);
                    if (esNuloOVacioEnergiaUnidad)
                    {
                        throw new Exception("No existen datos de la energía de unidad de generación de la evaluación a consultar");
                    }

                    string[] listValorEEU = eeu.Rereeuenergiaunidad.Split(',');
                    bool existeValorEEU = (listValorEEU != null && listValorEEU.Length > 0);
                    if (!existeValorEEU)
                    {
                        throw new Exception("No existen datos de la energía de unidad de generación con el separador de coma");
                    }
                    #endregion

                    #region Obtener "Energía de Medidores de Generación" y "Factor de Pérdidas Medias" de la Central
                    List<MeMedicion96DTO> listEMGC = ObtenerListaDeEnergiaDeMedidoresDeGeneracionDeLaCentral(empresas, ese.Equicodi, eeu.Equicodi, dtFechahorainicioMenosHoras, dtFechahorafinMasHoras);
                    List<RerFacPerMedDetDTO> listFPMD = FactoryTransferencia.GetRerFacPerMedDetRepository().GetByRangeDate(dtFechahorainicioMenosHoras, dtFechahorafinMasHoras);
                    #endregion

                    #region Crear los datos de la tabla "Energia Estimada vs Energía Solicitada" en base a la lista de Intervalos
                    #region Definir o setear variables
                    int i = 0;
                    listCDT = new List<RerComparativoDetDTO>();
                    #endregion

                    #region Crear datos de la tabla
                    foreach (var fechaHoraFinalIntervalo in listaIntervalos)
                    {
                        #region Setear CDT Fecha y hora
                        RerComparativoDetDTO cdt = new RerComparativoDetDTO
                        {
                            Rercdtfecha = fechaHoraFinalIntervalo.Date,
                            Rercdthora = fechaHoraFinalIntervalo.ToString(ConstantesAppServicio.FormatoOnlyHora),
                        };
                        #endregion

                        #region Setear Categorias X
                        string fechaHora = cdt.Rercdtfecha.ToString(ConstantesAppServicio.FormatoFechaEjecutivo) + " " + cdt.Rercdthora;
                        xAxisCategories.Add(fechaHora);
                        #endregion

                        #region Setear Medidor con Factor de Pérdida MWh
                        decimal valueEMGC = GetValorEnergiaDeMedidoresDeGeneracionDeLaCentral(cdt.Rercdtfecha, cdt.Rercdthora, listEMGC);
                        decimal valueFPMC = GetValorFactorDePerdidasMediasDeLaCentral(ese.Equicodi, cdt.Rercdtfecha, listFPMD);
                        cdt.Rercdtmedfpm = Math.Round(valueEMGC * valueFPMC / 4, ConstantesPrimasRER.numero4);
                        #endregion

                        #region Setear Energia Solicitada
                        int compararDtInicio = DateTime.Compare(fechaHoraFinalIntervalo, eseFechaHoraInicioAjustado);
                        int compararDtFin = DateTime.Compare(fechaHoraFinalIntervalo, eseFechaHoraFinAjustado);
                        bool perteneceAlRango = (compararDtInicio > 0 && compararDtFin <= 0);
                        if (perteneceAlRango)
                        {
                            #region Setear Energia Solicitada
                            string valorEEU;
                            try { valorEEU = listValorEEU[i].Trim(); }
                            catch { valorEEU = "0"; }

                            decimal? energiaSolicitada;
                            try { energiaSolicitada = Decimal.Parse(valorEEU, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint); }
                            catch { throw new Exception("El valor de energía '" + valorEEU  + "' de la unidad de generación '" + eeu.Equinomb + "', ubicado en el índice " + i + ", es inválido."); }

                            decimal valorEnergiaSolicitada = Math.Round(energiaSolicitada.Value, ConstantesPrimasRER.numero4);
                            cdt.Rercdtenesolicitada = valorEnergiaSolicitada;

                            i++;
                            #endregion
                        }
                        #endregion

                        #region Agregar registro a la tabla 
                        listCDT.Add(cdt);
                        #endregion
                    }
                    #endregion
                    #endregion
                    #endregion
                }
                #endregion

                #region En caso exista RerEnergiaEstimada, setear "Energia Estimada" sobre listCDT (Comparativo Detalle)
                #region Desde el Archivo Excel: Setear Energía Estimada, Sumar Energía Estimada Unidad, Sumar Energía Solicitada Unidad
                bool aplicarEnergiaEstimadaDesdeArchivoExcel = (energiaEstimada != null && energiaEstimada.OrigenDatos == ConstantesPrimasRER.origenDatosArchivoExcel);
                if (aplicarEnergiaEstimadaDesdeArchivoExcel)
                {
                    bool existenDatos = (energiaEstimada.ListCDTExcel != null && energiaEstimada.ListCDTExcel.Count > 0 && listCDT != null && listCDT.Count > 0);
                    if (existenDatos)
                    {
                        #region Validar que las fechas del Archivo Excel, estén dentro del rango de fechas de la tabla
                        RerComparativoDetDTO cdtFirst = listCDT[0];
                        RerComparativoDetDTO cdtLast = listCDT[listCDT.Count - 1];
                        DateTime dtFechaHoraInicioListCDT = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdtFirst.Rercdtfecha.Day.ToString("D2"), cdtFirst.Rercdtfecha.Month.ToString("D2"), cdtFirst.Rercdtfecha.Year, cdtFirst.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        DateTime dtFechaHoraFinListCDT = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdtLast.Rercdtfecha.Day.ToString("D2"), cdtLast.Rercdtfecha.Month.ToString("D2"), cdtLast.Rercdtfecha.Year, cdtLast.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                        RerComparativoDetDTO cdtExcelFirst = energiaEstimada.ListCDTExcel[0];
                        RerComparativoDetDTO cdtExcelLast = energiaEstimada.ListCDTExcel[energiaEstimada.ListCDTExcel.Count - 1];
                        DateTime dtFechaHoraInicioArchivoExcel = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdtExcelFirst.Rercdtfecha.Day.ToString("D2"), cdtExcelFirst.Rercdtfecha.Month.ToString("D2"), cdtExcelFirst.Rercdtfecha.Year, cdtExcelFirst.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        DateTime dtFechaHoraFinArchivoExcel = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdtExcelLast.Rercdtfecha.Day.ToString("D2"), cdtExcelLast.Rercdtfecha.Month.ToString("D2"), cdtExcelLast.Rercdtfecha.Year, cdtExcelLast.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                        int compararFechaHoraInicio = DateTime.Compare(dtFechaHoraInicioArchivoExcel, dtFechaHoraInicioListCDT);
                        bool perteneceAlInicioDelRango = (compararFechaHoraInicio >= 0);
                        if (!perteneceAlInicioDelRango)
                        {
                            throw new Exception("La 'fecha y hora de inicio' del archivo Excel no debe ser menor a la 'fecha y hora de inicio de la tabla' de la unidad de generación");
                        }

                        int compararFechaHoraFin = DateTime.Compare(dtFechaHoraFinArchivoExcel, dtFechaHoraFinListCDT);
                        bool perteneceAlFinDelRango = (compararFechaHoraFin <= 0);
                        if (!perteneceAlFinDelRango)
                        {
                            throw new Exception("La 'fecha y hora de fin' del archivo Excel no debe ser mayor a la 'fecha y hora de fin de la tabla' de la unidad de generación");
                        }
                        #endregion

                        #region Setear variables
                        int z = 0;
                        energia_estimada_unidad = 0;
                        energia_solicitada_unidad = 0;
                        #endregion

                        #region Setear Energía Estimada y Porcentaje de Desviación
                        foreach (var cdt in listCDT)
                        {
                            DateTime dtCdtFechaHora = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdt.Rercdtfecha.Day.ToString("D2"), cdt.Rercdtfecha.Month.ToString("D2"), cdt.Rercdtfecha.Year.ToString("D2"), cdt.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                            int compararDtInicio = DateTime.Compare(dtCdtFechaHora, dtFechaHoraInicioArchivoExcel);
                            int compararDtFin = DateTime.Compare(dtCdtFechaHora, dtFechaHoraFinArchivoExcel);
                            bool perteneceAlRango = (compararDtInicio >= 0 && compararDtFin <= 0);
                            if (!perteneceAlRango)
                            {
                                cdt.Rercdteneestimada = null;
                                cdt.Rercdtpordesviacion = null;
                            }
                            else
                            {
                                foreach (var cdtExcel in energiaEstimada.ListCDTExcel)
                                {
                                    bool existeRegistro = (DateTime.Compare(cdtExcel.Rercdtfecha, cdt.Rercdtfecha) == 0 && cdtExcel.Rercdthora == cdt.Rercdthora);
                                    if (existeRegistro)
                                    {
                                        #region Setear Energía Estimada y Porcentaje de Desviación
                                        cdt.Rercdteneestimada = Math.Round(cdtExcel.Rercdteneestimada.Value, ConstantesPrimasRER.numero4);
                                        bool energiasValidas = (cdt.Rercdtenesolicitada != null && cdt.Rercdteneestimada != null && cdt.Rercdteneestimada.Value != 0);
                                        if (energiasValidas)
                                        {
                                            cdt.Rercdtpordesviacion = Math.Round((cdt.Rercdtenesolicitada.Value - cdt.Rercdteneestimada.Value) / cdt.Rercdteneestimada.Value, ConstantesPrimasRER.numero4);
                                        }
                                        else
                                        {
                                            cdt.Rercdtpordesviacion = 0;
                                        }
                                        #endregion

                                        #region Sumar Energía Estimada, Sumar Energía Solicitada
                                        energia_estimada_unidad += (cdt.Rercdteneestimada != null ? cdt.Rercdteneestimada.Value : 0);
                                        energia_solicitada_unidad += (cdt.Rercdtenesolicitada != null ? cdt.Rercdtenesolicitada.Value : 0);
                                        z++;
                                        #endregion
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Setear Energia Estimada 15 min
                        if (z > 0)
                        {
                            energia_estimada_15_min = Math.Round((energia_estimada_unidad / z), ConstantesPrimasRER.numero4);
                        }
                        #endregion
                    }
                }
                #endregion

                #region Desde el Valor Típico 
                bool aplicarEnergiaEstimadaDesdeValorTipico = (energiaEstimada != null && energiaEstimada.OrigenDatos == ConstantesPrimasRER.origenDatosValorTipico);
                if (aplicarEnergiaEstimadaDesdeValorTipico)
                {
                    bool existenDatos = (energiaEstimada.ValorTipico != null && listCDT != null && listCDT.Count > 0);
                    if (existenDatos)
                    {
                        #region Validar que las fechas del Valor Típico, estén dentro del rango de fechas de la tabla
                        RerComparativoDetDTO cdtFirst = listCDT[0];
                        RerComparativoDetDTO cdtLast = listCDT[listCDT.Count - 1];
                        DateTime dtFechaHoraInicioListCDT = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdtFirst.Rercdtfecha.Day.ToString("D2"), cdtFirst.Rercdtfecha.Month.ToString("D2"), cdtFirst.Rercdtfecha.Year, cdtFirst.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        DateTime dtFechaHoraFinListCDT = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdtLast.Rercdtfecha.Day.ToString("D2"), cdtLast.Rercdtfecha.Month.ToString("D2"), cdtLast.Rercdtfecha.Year, cdtLast.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        dtFechaHoraFinListCDT = dtFechaHoraFinListCDT.AddMinutes(ConstantesPrimasRER.numero15);
                        DateTime dtFechaHoraInicioValorTipico = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", energiaEstimada.ValorTipico.FechaInicio.Day.ToString("D2"), energiaEstimada.ValorTipico.FechaInicio.Month.ToString("D2"), energiaEstimada.ValorTipico.FechaInicio.Year, energiaEstimada.ValorTipico.HoraInicio), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        DateTime dtFechaHoraFinValorTipico = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", energiaEstimada.ValorTipico.FechaFin.Day.ToString("D2"), energiaEstimada.ValorTipico.FechaFin.Month.ToString("D2"), energiaEstimada.ValorTipico.FechaFin.Year, energiaEstimada.ValorTipico.HoraFin), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

                        int compararFechaHoraInicio = DateTime.Compare(dtFechaHoraInicioValorTipico, dtFechaHoraInicioListCDT);
                        bool perteneceAlInicioRango = (compararFechaHoraInicio >= 0);
                        if (!perteneceAlInicioRango)
                        {
                            throw new Exception("La 'fecha y hora de inicio' no debe ser menor a la 'fecha y hora de inicio de la tabla' de la unidad de generación");
                        }

                        int compararFechaHoraFin = DateTime.Compare(dtFechaHoraFinValorTipico, dtFechaHoraFinListCDT);
                        bool perteneceAlFinRango = (compararFechaHoraFin <= 0);
                        if (!perteneceAlFinRango)
                        {
                            throw new Exception("La 'fecha y hora de fin' no debe ser mayor a la 'fecha y hora de fin de la tabla' de la unidad de generación");
                        }
                        #endregion

                        #region Setear Variables
                        energia_estimada_unidad = 0;
                        energia_solicitada_unidad = 0;
                        #endregion

                        #region Obtener Energía Estimada 15 minutos
                        int countMedidorFactorPerdida = 0;
                        decimal sumaMedidorFactorPerdida = 0;
                        foreach (var cdt in listCDT)
                        {
                            DateTime dtCdtFechaHora = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdt.Rercdtfecha.Day.ToString("D2"), cdt.Rercdtfecha.Month.ToString("D2"), cdt.Rercdtfecha.Year, cdt.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                            int compararDtInicioVsEseFechaHoraInicioAjustado = DateTime.Compare(dtCdtFechaHora, eseFechaHoraInicioAjustado);
                            bool dtInicioEsMenorEseFechaHoraInicioAjustado = (compararDtInicioVsEseFechaHoraInicioAjustado <= 0);
                            if (dtInicioEsMenorEseFechaHoraInicioAjustado) //Donde se consideran los intervalos antes de la fecha inicio de la solicitud EDI
                            {
                                sumaMedidorFactorPerdida += ((cdt.Rercdtmedfpm != null) ? cdt.Rercdtmedfpm.Value : 0);
                                countMedidorFactorPerdida++;
                            }
                        }
                        energia_estimada_15_min = Math.Round((sumaMedidorFactorPerdida / countMedidorFactorPerdida), ConstantesPrimasRER.numero4);
                        #endregion

                        #region Setear Energía Estimada y Porcentaje de Desviación
                        foreach (var cdt in listCDT)
                        {
                            DateTime dtCdtFechaHora = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", cdt.Rercdtfecha.Day.ToString("D2"), cdt.Rercdtfecha.Month.ToString("D2"), cdt.Rercdtfecha.Year, cdt.Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                            int compararDtInicio = DateTime.Compare(dtCdtFechaHora, dtFechaHoraInicioValorTipico);
                            int compararDtFin = DateTime.Compare(dtCdtFechaHora, dtFechaHoraFinValorTipico);
                            bool perteneceAlRango = (compararDtInicio >= 0 && compararDtFin <= 0);
                            if (!perteneceAlRango)
                            {
                                cdt.Rercdteneestimada = null;
                                cdt.Rercdtpordesviacion = null;
                            }
                            else
                            {
                                cdt.Rercdteneestimada = CalcularCdtEnergiaEstimada(cdt, ese, eseFechaHoraInicioAjustado, eseFechaHoraFinAjustado, energia_estimada_15_min);
                                bool energiasValidas = (cdt.Rercdtenesolicitada != null && cdt.Rercdteneestimada != null && cdt.Rercdteneestimada.Value != 0);
                                if (energiasValidas)
                                {
                                    cdt.Rercdtpordesviacion = Math.Round((cdt.Rercdtenesolicitada.Value - cdt.Rercdteneestimada.Value) / cdt.Rercdteneestimada.Value, ConstantesPrimasRER.numero4);
                                }
                                else
                                {
                                    cdt.Rercdtpordesviacion = 0;
                                }

                                #region Sumar Energía Estimada, Sumar Energía Solicitada
                                energia_estimada_unidad += (cdt.Rercdteneestimada != null ? cdt.Rercdteneestimada.Value : 0);
                                energia_solicitada_unidad += (cdt.Rercdtenesolicitada != null ? cdt.Rercdtenesolicitada.Value : 0);
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Valida la fecha y hora de inicio, y la fecha y hora fin, correspondientes a los datos de Valor Típico
        /// </summary>
        /// <param name="fecha_inicio">Formato: dd/MM/yyyy</param>
        /// <param name="hora_inicio"></param>
        /// <param name="fecha_fin">Formato: dd/MM/yyyy</param>
        /// <param name="hora_fin"></param>
        private void ValidarDatosValorTipico(string fecha_inicio, string hora_inicio, string fecha_fin, string hora_fin)
        {
            #region Validar que fechas y horas no sean nulos ni vacíos
            bool esNuloOVacioFechaInicio = (string.IsNullOrEmpty(fecha_inicio) || fecha_inicio.Trim() == "");
            if (esNuloOVacioFechaInicio)
            {
                throw new Exception("Una fecha de inicio es requerida");
            }
            bool esNuloOVacioFechaFin = (string.IsNullOrEmpty(fecha_fin) || fecha_fin.Trim() == "");
            if (esNuloOVacioFechaFin)
            {
                throw new Exception("Una fecha de fin es requerida");
            }
            bool esNuloOVacioHoraInicio = (string.IsNullOrEmpty(hora_inicio) || hora_inicio.Trim() == "");
            if (esNuloOVacioHoraInicio)
            {
                throw new Exception("Una hora de inicio es requerida");
            }
            bool esNuloOVacioHoraFin = (string.IsNullOrEmpty(hora_fin) || hora_fin.Trim() == "");
            if (esNuloOVacioHoraFin)
            {
                throw new Exception("Una hora de fin es requerida");
            }
            #endregion

            #region Setear fechas
            DateTime dtFechaHoraInicio;
            try
            {
                dtFechaHoraInicio = DateTime.ParseExact(string.Format("{0} {1}", fecha_inicio.Substring(0, 10), hora_inicio), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("La fecha y hora de inicio no es válida");
            }

            DateTime dtFechaHoraFin;
            try
            {
                dtFechaHoraFin = DateTime.ParseExact(string.Format("{0} {1}", fecha_fin.Substring(0, 10), hora_fin), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("La fecha y hora de fin no es válida");
            }
            #endregion

            #region Comparar fechas
            int compararFechas = DateTime.Compare(dtFechaHoraInicio, dtFechaHoraFin);
            bool esFechaHoraInicioMayorFechaHoraFin = (compararFechas >= 0);
            if (esFechaHoraInicioMayorFechaHoraFin)
            {
                throw new Exception("La 'fecha y hora de inicio' debe ser menor a la 'fecha y hora de fin'");
            }
            #endregion

            #region Validar que horas sean múltiplo de 15
            bool esHoraInicioMultiplo15 = (dtFechaHoraInicio.Minute % ConstantesPrimasRER.numero15 == 0);
            if (!esHoraInicioMultiplo15)
            {
                throw new Exception("La hora de inicio no es múltiplo de 15");
            }

            bool esHoraFinMultiplo15 = (dtFechaHoraFin.Minute % ConstantesPrimasRER.numero15 == 0);
            if (!esHoraFinMultiplo15)
            {
                throw new Exception("La hora de fin no es múltiplo de 15");
            }
            #endregion
        }

        /// <summary>
        /// Verifica que los minutos de las fechas de inicio y fin (de una solicitud EDI) sean múltiplo de quince. 
        /// De no ser así, entonces realiza los ajustes necesarios para que lo sean.
        /// Finalmente, devuelve las nuevas fechas en los parámetros de salida respectivos
        /// </summary>
        /// <param name="dtFechaHoraInicio"></param>
        /// <param name="dtFechaHoraFin"></param>
        /// <param name="dtFechaHoraInicioAjustado"></param>
        /// <param name="dtFechaHoraFinAjustado"></param>
        private void ObtenerFechasAjustadas(DateTime dtFechaHoraInicio, DateTime dtFechaHoraFin, out DateTime dtFechaHoraInicioAjustado, out DateTime dtFechaHoraFinAjustado)
        {
            try
            {
                int residuoMinutosInicio = dtFechaHoraInicio.Minute % ConstantesPrimasRER.numero15;
                dtFechaHoraInicioAjustado = (residuoMinutosInicio > 0) ? dtFechaHoraInicio.AddMinutes(-residuoMinutosInicio) : dtFechaHoraInicio;

                int residuoMinutosFin = dtFechaHoraFin.Minute % ConstantesPrimasRER.numero15;
                dtFechaHoraFinAjustado = (residuoMinutosFin > 0) ? dtFechaHoraFin.AddMinutes(ConstantesPrimasRER.numero15 - residuoMinutosFin) : dtFechaHoraFin;
            }
            catch (Exception e1)
            {
                throw new Exception("Existe un error con las fechas de la solicitud EDI.", e1);
            }
        }

        /// <summary>
        /// Calcula la energía estimada con respecto a un intervalo de 15 minutos. 
        /// En caso, el intervalo de las fechas horas de inicio y fin no sean múltiplo 15 minutos, obtiene su energía estimada proporcional.
        /// </summary>
        /// <param name="cdt"></param>
        /// <param name="ese"></param>
        /// <param name="eseFechaHoraInicioAjustado"></param>
        /// <param name="eseFechaHoraFinAjustado"></param>
        /// <param name="energia_estimada_15_min"></param>
        /// <returns></returns>
        private decimal CalcularCdtEnergiaEstimada(RerComparativoDetDTO cdt, RerEvaluacionSolicitudEdiDTO ese, DateTime eseFechaHoraInicioAjustado, DateTime eseFechaHoraFinAjustado, decimal energia_estimada_15_min)
        {
            try
            {
                decimal cdt_energia_estimada = energia_estimada_15_min;

                DateTime eseFechaHoraInicioAjustado_FechaHoraFinalIntervalo = eseFechaHoraInicioAjustado.AddMinutes(ConstantesPrimasRER.numero15);
                DateTime eseFechaHoraFinAjustado_FechaHoraInicialIntervalo = eseFechaHoraFinAjustado.AddMinutes(ConstantesPrimasRER.numeroMenosQuince);

                DateTime fechaInicio_FechaFinalIntervalo = eseFechaHoraInicioAjustado_FechaHoraFinalIntervalo.Date;
                string horaInicio_HoraFinalIntervalo = eseFechaHoraInicioAjustado_FechaHoraFinalIntervalo.Hour.ToString("D2") + ":" + eseFechaHoraInicioAjustado_FechaHoraFinalIntervalo.Minute.ToString("D2");
                bool esIgualAFechaHoraInicioAjustado_FechaHoraFinalIntervalo = (cdt.Rercdtfecha == fechaInicio_FechaFinalIntervalo && cdt.Rercdthora == horaInicio_HoraFinalIntervalo);

                DateTime fechaFin = eseFechaHoraFinAjustado.Date;
                string horaFin = eseFechaHoraFinAjustado.Hour.ToString("D2") + ":" + eseFechaHoraFinAjustado.Minute.ToString("D2");
                bool esIgualAFechaHoraFinAjustado = (cdt.Rercdtfecha == fechaFin && cdt.Rercdthora == horaFin);

                if (esIgualAFechaHoraInicioAjustado_FechaHoraFinalIntervalo)
                {
                    bool esHoraInicioMultiplo15 = (ese.Reresefechahorainicio.Minute % ConstantesPrimasRER.numero15 == 0);
                    if (!esHoraInicioMultiplo15)
                    {
                        int intervaloEnMinutos = (eseFechaHoraInicioAjustado_FechaHoraFinalIntervalo - ese.Reresefechahorainicio).Minutes;
                        cdt_energia_estimada = Math.Round((energia_estimada_15_min * intervaloEnMinutos / ConstantesPrimasRER.numero15), ConstantesPrimasRER.numero4);
                    }
                }
                else if (esIgualAFechaHoraFinAjustado)
                {
                    bool esHoraFinMultiplo15 = (ese.Reresefechahorafin.Minute % ConstantesPrimasRER.numero15 == 0);
                    if (!esHoraFinMultiplo15)
                    {
                        int intervaloEnMinutos = (ese.Reresefechahorafin - eseFechaHoraFinAjustado_FechaHoraInicialIntervalo).Minutes;
                        cdt_energia_estimada = Math.Round((energia_estimada_15_min * intervaloEnMinutos / ConstantesPrimasRER.numero15), ConstantesPrimasRER.numero4);
                    }
                }

                return cdt_energia_estimada;
            }
            catch (Exception e1)
            {
                throw new Exception("Existe un error al calcular una energía estimada.", e1);
            }
        }

        /// <summary>
        /// Obtiene la lista de Energía de Medidores de Generación de la Central, con respecto al filtro especificado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ese_equicodi"></param>
        /// <param name="eeu_equicodi"></param>
        /// <param name="dtFechahorainicioMenosHoras"></param>
        /// <param name="dtFechahorafinMasHoras"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ObtenerListaDeEnergiaDeMedidoresDeGeneracionDeLaCentral(string empresas, int ese_equicodi, int eeu_equicodi, DateTime dtFechahorainicioMenosHoras, DateTime dtFechahorafinMasHoras)
        {
            int nroPagina = 1;
            int tipoCentral = 1;
            string tipoEmpresa = "1,2,3,4,5";
            string tipoGeneracion = "1,2,3,4";
            int tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaActiva;
            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            ConsultaMedidoresAppServicio medidoresAppServicio = new ConsultaMedidoresAppServicio();
            List<MeMedicion96DTO> listEMGC = null;

            int nroRegistros = medidoresAppServicio.ObtenerNroRegistroConsultaMedidores(dtFechahorainicioMenosHoras, dtFechahorafinMasHoras,
                tipoEmpresa, empresas, tipoGeneracion, tipoCentral, 1);

            List<MeMedicion96DTO> listMedicion96 = FactorySic.GetMeMedicion96Repository().ObtenerConsultaMedidores(dtFechahorainicioMenosHoras, dtFechahorafinMasHoras,
                tipoCentral, tipoGeneracion, empresas, ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante,
                lectcodi, tipoInformacion, ConstantesMedicion.TipoGenerrer, nroPagina, nroRegistros, ConstantesMedidores.TptoMedicodiTodos);

            bool existeListMedicion96 = (listMedicion96 != null && listMedicion96.Count > 0);
            if (existeListMedicion96)
            {
                listEMGC = listMedicion96.Where(x => x.Equipadre == ese_equicodi && x.Equicodi == eeu_equicodi).ToList();
                bool existeListEMGC = (listEMGC != null && listEMGC.Count > 0);
                if (existeListEMGC)
                {
                    listEMGC = listEMGC.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
                }
            }

            return listEMGC;
        }

        #endregion

        #region Resultados

        /// <summary>
        /// Obtiene la última Evaluación de una Revisión específica
        /// </summary>
        /// <param name="rerrevcodi"></param>
        /// <returns></returns>
        public RerEvaluacionDTO ObtenerUltimaEvaluacion(int rerrevcodi)
        {
            #region Validación Básica
            bool esRerrevcodiValido = (rerrevcodi > 0);
            if (!esRerrevcodiValido)
            {
                throw new Exception("Debe seleccionar una revisión para realizar la consulta");
            }

            RerRevisionDTO revision = FactoryTransferencia.GetRerRevisionRepository().GetById(rerrevcodi);
            bool existeRevision = (revision != null);
            if (!existeRevision)
            {
                throw new Exception("No existe la revisión con id = " + rerrevcodi);
            }
            #endregion

            #region Obtener última evaluación de la revisión
            RerEvaluacionDTO lastEvaluacion = null;
            List<RerEvaluacionDTO> listEvaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetByCriteria(revision.Rerrevcodi).OrderByDescending(x => x.Rerevanumversion).ToList();
            bool existeListEvaluacion = (listEvaluacion != null && listEvaluacion.Count > 0);
            if (existeListEvaluacion)
            {
                lastEvaluacion = listEvaluacion[0];  //Obtener última versión de la revisión
            }

            return lastEvaluacion;
            #endregion
        }

        /// <summary>
        /// Genera un handsontable con el listado de Evaluación de Solicitudes EDI (Intranet) correspondiente a la última versión para un periodo y una revisión específica
        /// Donde determina si alguno de sus campos son editables
        /// Esto es esclusivo para el sub módulos "Cuadro - Resultados"
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="url"></param>
        /// <param name="esEditable"></param>
        /// <returns></returns>
        public HandsonModel GenerarHandsonTableEvaluacionSolicitudEdiParaResultados(int rerevacodi, string url, out bool esEditable)
        {
            try
            {
                #region Validación Básica
                bool esRerevacodiValido = (rerevacodi > 0);
                if (!esRerevacodiValido)
                {
                    return GenerarHandsonTableVacioParaResultados(out esEditable);
                }

                RerEvaluacionDTO evaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetById(rerevacodi);
                bool existeEvaluacion = (evaluacion != null);
                if (!existeEvaluacion)
                {
                    throw new Exception("No existe una evaluación con id = " + rerevacodi);
                }

                RerRevisionDTO revision = FactoryTransferencia.GetRerRevisionRepository().GetById(evaluacion.Rerrevcodi);
                bool existeRevision = (revision != null);
                if (!existeRevision)
                {
                    throw new Exception("No existe una revisión con id = " + evaluacion.Rerrevcodi);
                }
                #endregion

                #region Determinar si se deben editar algunos campos del handsontable
                esEditable = (revision.Rerrevestado == ConstantesPrimasRER.estadoAbierto);
                #endregion

                #region Obtener Listado Evaluación de Solicitud EDI (Intranet) con respecto a la última versión de una revisión
                List<RerEvaluacionSolicitudEdiDTO> listESE = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetByCriteria(evaluacion.Rerevacodi, -1);
                bool existeListESE = (listESE != null && listESE.Count > 0);
                if (existeListESE)
                {
                    listESE = listESE.Where(x => x.Rereseeliminado.ToUpper() == ConstantesPrimasRER.eliminadoNo).ToList();
                    existeListESE = (listESE != null && listESE.Count > 0);
                    if (existeListESE)
                    {
                        listESE = listESE.OrderBy(x => x.Emprcodi).ThenBy(x => x.Equinomb).ThenBy(x => x.Reresefechahorainicio).ThenBy(x => x.Reresefechahorafin).ToList();
                    }
                }
                #endregion

                #region Generar HandsonTable
                #region Definir alcance
                int numColFijas = 12;
                int numColMoviles = 0;
                string[] fuenteDatoVacio = new string[] { };
                string[] resultadoDesc = new string[] { "", "Posee coherencia con datos históricos", "Se redujo la magnitud EDI solicitada descontando la generación ejecutada en el periodo solicitado", "Otro" };
                string[] resultadoEstado = new string[] { "", ConstantesPrimasRER.resultadoEstadoAprobada, ConstantesPrimasRER.resultadoEstadoNoAprobada, ConstantesPrimasRER.resultadoEstadoSolicitudDeFuerzaMayor };

                List<CabeceraRow> listaCabecera = new List<CabeceraRow>
                {
                    new CabeceraRow() { TituloRow = "", IsMerge = 0, Ancho = 1, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false }, //oculto - reresecodi
                    new CabeceraRow() { TituloRow = "Id <br/> Solicitud <br/> EDI", IsMerge = 0, Ancho = 60, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Fecha y hora<br/>de inicio", IsMerge = 0, Ancho = 105, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = esEditable },  //EsEditable?
                    new CabeceraRow() { TituloRow = "Fecha y hora<br/>de término", IsMerge = 0, Ancho = 105, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = esEditable }, //EsEditable?
                    new CabeceraRow() { TituloRow = "Empresa", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Equipo", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Origen", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Descripción", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = esEditable }, //EsEditable?
                    new CabeceraRow() { TituloRow = "Magnitud<br/>EDI<br/>Aprobada", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Derecha", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Resultado<br/>Descripción", IsMerge = 1, Ancho = 200, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoLista, FuenteDato = resultadoDesc, EsEditable = esEditable }, //EsEditable?
                    new CabeceraRow() { TituloRow = "Resultado<br/>Estado", IsMerge = 1, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoLista, FuenteDato = resultadoEstado, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Energía<br/>Estimada<br/>Central", IsMerge = 0, Ancho = 50, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                };
                #endregion

                #region Cargar configuraciones
                string[] headers = listaCabecera.Select(x => x.TituloRow).ToArray();
                List<int> widths = listaCabecera.Select(x => x.Ancho).ToList();
                object[] columnas = new object[headers.Length];

                for (int m = 0; m < headers.Length; m++)
                {
                    var cabecera = listaCabecera[m];
                    columnas[m] = new
                    {
                        type = cabecera.TipoDato,
                        source = cabecera.FuenteDato,
                        strict = false,
                        dateFormat = string.Empty,
                        correctFormat = false,
                        defaultDate = string.Empty,
                        format = string.Empty,
                        className = cabecera.AlineacionHorizontal == "Derecha" ? "htRight" : (cabecera.AlineacionHorizontal == "Izquierda" ? "htLeft" : "htCenter"),
                        readOnly = !cabecera.EsEditable,
                    };
                }
                #endregion

                #region Cargar datos 
                int numFilaActual = 0;
                int numCol = numColFijas + numColMoviles;
                List<string[]> listaDataHandson = new List<string[]>();

                foreach (var reg in listESE)
                {
                    string[] matriz = new string[numCol];
                    if (reg != null)
                    {
                        matriz[0] = reg.Reresecodi.ToString(); //oculto - reresecodi
                        matriz[1] = reg.Rersedcodi.ToString(); //rersedcodi
                        matriz[2] = reg.Reresefechahorainicio.ToString(ConstantesAppServicio.FormatoFechaFull); //Fecha hora inicio //editable
                        matriz[3] = reg.Reresefechahorafin.ToString(ConstantesAppServicio.FormatoFechaFull); //Fecha hora fin //editable
                        matriz[4] = reg.Emprnomb; //Empresa
                        matriz[5] = reg.Equinomb; //Equipo
                        matriz[6] = reg.Reroridesc; //Origen
                        matriz[7] = reg.Reresedesc; //Descripción  //editable
                        matriz[8] = (reg.Rereseediaprobada != null) ? reg.Rereseediaprobada.Value.ToString() : ""; //Magnitud EDI Aprobada 
                        matriz[9] = reg.Rereseresdesc; //Resultado - Descripción  //editable
                        matriz[10] = reg.Rereseresestado; //Resultado - Estado  
                        matriz[11] = reg.Reresecodi.ToString(); //reresecodi  
                    }

                    listaDataHandson.Add(matriz);
                    numFilaActual++;
                }

                List<CeldaMerge> listaMerge = new List<CeldaMerge>();
                #endregion
                #endregion

                #region Return
                HandsonModel handson = new HandsonModel
                {
                    ListaExcelData = listaDataHandson.ToArray(),
                    Headers = headers,
                    ListaColWidth = widths,
                    Columnas = columnas,
                    MaxCols = numCol,
                    MaxRows = listaDataHandson.Count,
                    ListaMerge = listaMerge,
                    ListaCambios = new List<CeldaCambios>() //arrCambioCells;
                };

                return handson;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera un handsontable vacío.
        /// Donde determina si alguno de sus campos son editables.
        /// Esto es esclusivo para el sub módulos "Cuadro - Resultados"
        /// </summary>
        /// <param name="esEditable"></param>
        /// <returns></returns>
        public HandsonModel GenerarHandsonTableVacioParaResultados(out bool esEditable)
        {
            try
            {
                #region Determinar si se deben editar algunos campos del handsontable
                esEditable = false;
                #endregion

                #region Generar HandsonTable
                #region Definir alcance
                int numColFijas = 12;
                int numColMoviles = 0;
                string[] fuenteDatoVacio = new string[] { };
                string[] resultadoDesc = new string[] { "", "Posee coherencia con datos históricos", "Se redujo la magnitud EDI solicitada descontando la generación ejecutada en el periodo solicitado", "Otro" };
                string[] resultadoEstado = new string[] { "", ConstantesPrimasRER.resultadoEstadoAprobada, ConstantesPrimasRER.resultadoEstadoNoAprobada, ConstantesPrimasRER.resultadoEstadoSolicitudDeFuerzaMayor };

                List<CabeceraRow> listaCabecera = new List<CabeceraRow>
                {
                    new CabeceraRow() { TituloRow = "", IsMerge = 0, Ancho = 1, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false }, //oculto - reresecodi
                    new CabeceraRow() { TituloRow = "Id <br/> Solicitud <br/> EDI", IsMerge = 0, Ancho = 60, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Fecha y hora<br/>de inicio", IsMerge = 0, Ancho = 105, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = esEditable },  //EsEditable?
                    new CabeceraRow() { TituloRow = "Fecha y hora<br/>de término", IsMerge = 0, Ancho = 105, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = esEditable }, //EsEditable?
                    new CabeceraRow() { TituloRow = "Empresa", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Equipo", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Origen", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Descripción", IsMerge = 0, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = esEditable }, //EsEditable?
                    new CabeceraRow() { TituloRow = "Magnitud<br/>EDI<br/>Aprobada", IsMerge = 0, Ancho = 100, AlineacionHorizontal = "Derecha", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Resultado<br/>Descripción", IsMerge = 1, Ancho = 200, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoLista, FuenteDato = resultadoDesc, EsEditable = esEditable }, //EsEditable?
                    new CabeceraRow() { TituloRow = "Resultado<br/>Estado", IsMerge = 1, Ancho = 180, AlineacionHorizontal = "Izquierda", TipoDato = GridExcel.TipoLista, FuenteDato = resultadoEstado, EsEditable = false },
                    new CabeceraRow() { TituloRow = "Energía<br/>Estimada<br/>Central", IsMerge = 0, Ancho = 50, AlineacionHorizontal = "Centro", TipoDato = GridExcel.TipoTexto, FuenteDato = fuenteDatoVacio, EsEditable = false },
                };
                #endregion

                #region Cargar configuraciones
                string[] headers = listaCabecera.Select(x => x.TituloRow).ToArray();
                List<int> widths = listaCabecera.Select(x => x.Ancho).ToList();
                object[] columnas = new object[headers.Length];

                for (int m = 0; m < headers.Length; m++)
                {
                    var cabecera = listaCabecera[m];
                    columnas[m] = new
                    {
                        type = cabecera.TipoDato,
                        source = cabecera.FuenteDato,
                        strict = false,
                        dateFormat = string.Empty,
                        correctFormat = false,
                        defaultDate = string.Empty,
                        format = string.Empty,
                        className = cabecera.AlineacionHorizontal == "Derecha" ? "htRight" : (cabecera.AlineacionHorizontal == "Izquierda" ? "htLeft" : "htCenter"),
                        readOnly = !cabecera.EsEditable,
                    };
                }
                #endregion

                #region Cargar datos 
                int numCol = numColFijas + numColMoviles;
                List<string[]> listaDataHandson = new List<string[]>();

                string[] matriz = new string[numCol];
                matriz[0] = "0"; //oculto - reresecodi
                matriz[1] = "No se recibieron solicitudes. Recuerde que debe importar los datos desde la opción 'Cuadros - Evaluación'"; //rersedcodi
                matriz[2] = ""; //Fecha hora inicio //editable
                matriz[3] = ""; //Fecha hora fin //editable
                matriz[4] = ""; //Empresa
                matriz[5] = ""; //Equipo
                matriz[6] = ""; //Origen
                matriz[7] = ""; //Descripción  //editable
                matriz[8] = ""; //Magnitud EDI Aprobada 
                matriz[9] = ""; //Resultado - Descripción  //editable
                matriz[10] = ""; //Resultado - Estado  
                matriz[11] = ""; //reresecodi  
                listaDataHandson.Add(matriz);

                List<CeldaMerge> listaMerge = new List<CeldaMerge>();
                listaMerge.Add(new CeldaMerge { col = 1, row = 0, rowspan = 1, colspan = 11 });
                #endregion
                #endregion

                #region Return
                HandsonModel handson = new HandsonModel
                {
                    ListaExcelData = listaDataHandson.ToArray(),
                    Headers = headers,
                    ListaColWidth = widths,
                    Columnas = columnas,
                    MaxCols = numCol,
                    MaxRows = listaDataHandson.Count,
                    ListaMerge = listaMerge,
                    ListaCambios = new List<CeldaCambios>()
                };

                return handson;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza las solicitudes EDI de una evaluación específica 
        /// Para el caso de "Cuadros - Resultados"
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="dataht"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string ActualizarEvaluacionSolicitudEdiParaResultados(int rerevacodi, string[][] dataht, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            #region Actualizar EvaluacionSolicitudEdi
            try
            {
                #region Validación
                string mensajeError = ValidarActualizarEvaluacionSolicitudEdiParaResultados(rerevacodi, dataht, usuario, out RerEvaluacionDTO evaluacion);
                bool existeError = !string.IsNullOrWhiteSpace(mensajeError);
                if (existeError)
                {
                    throw new Exception(mensajeError);
                }
                #endregion

                #region Actualizar EvaluacionSolicitudEDI para el caso de Resultados
                conn = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().StartTransaction(conn);
                foreach (string[] data in dataht)
                {
                    RerEvaluacionSolicitudEdiDTO entity = SetEvaluacionSolicitudEdiByUpdateFieldsForResults(rerevacodi, data, usuario);
                    FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().UpdateFieldsForResults(entity, conn, tran);
                }
                #endregion

                #region Actualizar Evaluación a estado "validado"
                FactoryTransferencia.GetRerEvaluacionRepository().UpdateEstadoAGenerado(evaluacion.Rerrevcodi, usuario, conn, tran);
                FactoryTransferencia.GetRerEvaluacionRepository().UpdateEstado(evaluacion.Rerevacodi, ConstantesPrimasRER.estadoValidado, usuario, conn, tran);
                #endregion

                #region Cerrar Revisión
                FactoryTransferencia.GetRerRevisionRepository().UpdateEstado(evaluacion.Rerrevcodi, ConstantesPrimasRER.estadoCerrado, usuario, conn, tran);
                #endregion

                #region commit
                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }

            return "1";
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reresecodi"></param>
        /// <param name="rerevacodi"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelEnergiaEstimadaEvaluacionSolicitudEdi(int reresecodi, int rerevacodi)
        {
            try
            {
                #region Validación: EvaluacionSolicitudEdi y EvaluacionEnergiaUnidad
                RerEvaluacionSolicitudEdiDTO ese = FactoryTransferencia.GetRerEvaluacionSolicitudEdiRepository().GetById(reresecodi);
                bool existeEse = (ese != null);
                if (!existeEse)
                {
                    throw new Exception("No existe un registro en 'Evaluación Solicitud EDI' con id = " + reresecodi + ".");
                }

                List<RerEvaluacionEnergiaUnidadDTO> listEEU = FactoryTransferencia.GetRerEvaluacionEnergiaUnidadRepository().GetByCriteria(reresecodi, rerevacodi);
                bool existeListEEU = (listEEU != null && listEEU.Count > 0);
                if (!existeListEEU)
                {
                    throw new Exception("No existen registros en 'Evaluación Energia Unidad' con reresecodi = " + reresecodi + " y rerevacodi = " + rerevacodi + ".");
                }
                listEEU = listEEU.OrderBy(x => x.Rereeucodi).ToList(); //Importante que se ordene
                #endregion

                #region Obtener datos de las unidades de generación de la tabla comparativo detalle
                List<RerComparativoDetDTO> listComparativoDet = FactoryTransferencia.GetRerComparativoDetRepository().GetByCriteria(rerevacodi, reresecodi, ConstantesPrimasRER.numeroMenosUno);
                bool existeListComparativoDet = (listComparativoDet != null && listComparativoDet.Count > 0);
                if (!existeListComparativoDet)
                {
                    throw new Exception("No se han registrado ninguna 'Energía Estimada' para las unidades generadoras de la centrarl RER de la solicitud EDI.");
                }

                listComparativoDet = listComparativoDet.Where(x => x.Rercdtflag == ConstantesPrimasRER.flagDentroRango).ToList();
                existeListComparativoDet = (listComparativoDet != null && listComparativoDet.Count > 0);
                if (!existeListComparativoDet)
                {
                    throw new Exception("No existen registros de 'Energía Estimada' con flag 'dentro del rango' para las unidades generadoras de la centrarl RER de la solicitud EDI.");
                }
                listComparativoDet = listComparativoDet.OrderBy(x => x.Rercdtcodi).ToList(); //Importante que se ordene
                #endregion

                #region Obtener lista de intervalos por minuto
                List<DateTime> listFechaInicio = new List<DateTime>();
                List<DateTime> listFechaFinal = new List<DateTime>();
                foreach (var eeu in listEEU)
                {
                    List<RerComparativoDetDTO> list = listComparativoDet.Where(x =>
                        x.Rerevacodi == eeu.Rerevacodi && x.Reresecodi == eeu.Reresecodi && x.Rereeucodi == eeu.Rereeucodi &&
                        x.Rercdtflag == ConstantesPrimasRER.flagDentroRango
                        ).ToList();
                    bool existeList = (list != null && list.Count > 0);
                    if (!existeList)
                    {
                        continue;
                    }
                    list = list.OrderBy(x => x.Rercdtcodi).ToList(); //Importante que se ordene

                    DateTime fechaInicio = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", list[0].Rercdtfecha.Day.ToString("D2"), list[0].Rercdtfecha.Month.ToString("D2"), list[0].Rercdtfecha.Year, list[0].Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                    DateTime fechaFinal = DateTime.ParseExact(string.Format("{0}/{1}/{2} {3}", list[list.Count - 1].Rercdtfecha.Day.ToString("D2"), list[list.Count - 1].Rercdtfecha.Month.ToString("D2"), list[list.Count - 1].Rercdtfecha.Year, list[list.Count - 1].Rercdthora), ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                    listFechaInicio.Add(fechaInicio);
                    listFechaFinal.Add(fechaFinal);
                }

                bool existeListFechaInicio = (listFechaInicio != null && listFechaInicio.Count > 0);
                bool existeListFechaFinal = (listFechaFinal != null && listFechaFinal.Count > 0);
                if (!existeListFechaInicio || !existeListFechaFinal)
                {
                    throw new Exception("No existen registros dentro del rango para rer_comparativo_det para obtener el intervalo.");
                }

                DateTime minFechaInicio = listFechaInicio.Min();
                DateTime maxFechaFinal = listFechaFinal.Max();
                int comparacion = DateTime.Compare(minFechaInicio, maxFechaFinal);
                if (comparacion > 0)
                {
                    throw new Exception("La fecha de inicio no debe ser mayor que la fecha final.");
                }

                List<DateTime> listIntervalosMinutos = ObtenerIntervalosMinuto(minFechaInicio, maxFechaFinal);
                bool existeListIntervalosMinutos = (listIntervalosMinutos != null && listIntervalosMinutos.Count > 0);
                if (!existeListIntervalosMinutos)
                {
                    throw new Exception("No se pudo crear una lista de intervalos.");
                }
                #endregion

                #region Armar archivo Excel

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[1];
                listaCabeceras[0] = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Fecha"),
                    CrearExcelModelo("Hora"),
                };

                List<int> listaAnchoColumna = new List<int>
                {
                    15, //Fecha
                    15, //Hora
                };

                foreach (var eeu in listEEU)
                {
                    listaCabeceras[0].Add(CrearExcelModelo(eeu.Equinomb));
                    listaAnchoColumna.Add(15);
                }
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string>
                {
                    "center", //Fecha
                    "center", //Hora
                };

                List<string> listaTipo = new List<string>
                {
                    "string", //Fecha
                    "string", //Hora
                };

                foreach (var eeu in listEEU)
                {
                    listaAlineaHorizontal.Add("center");
                    listaTipo.Add("double");
                }

                int i = 0;
                List<string>[] listaRegistros = new List<string>[] { };
                foreach (var intervaloMinutos in listIntervalosMinutos)
                {
                    DateTime fecha = DateTime.ParseExact(string.Format("{0}/{1}/{2}", intervaloMinutos.Day.ToString("D2"), intervaloMinutos.Month.ToString("D2"), intervaloMinutos.Year), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    string hora = string.Format("{0}:{1}", intervaloMinutos.Hour.ToString("D2"), intervaloMinutos.Minute.ToString("D2"));
                    List<string> registro = new List<string>
                    {
                        fecha.ToString(ConstantesAppServicio.FormatoFechaWS),
                        hora
                    };

                    List<RerComparativoDetDTO> listCdt = listComparativoDet.Where(x => x.Rercdtfecha == fecha && x.Rercdthora == hora).ToList();
                    bool existeListCdt = (listCdt != null && listCdt.Count > 0);
                    if (!existeListCdt)
                    {
                        foreach (var euu in listEEU)
                        {
                            registro.Add("");
                        }
                    }
                    else
                    {
                        foreach (var eeu in listEEU)
                        {
                            List<RerComparativoDetDTO> list = listCdt.Where(x =>
                                x.Rerevacodi == eeu.Rerevacodi && x.Reresecodi == eeu.Reresecodi && x.Rereeucodi == eeu.Rereeucodi &&
                                x.Rercdtflag == ConstantesPrimasRER.flagDentroRango
                                ).ToList();

                            bool existeList = (list != null && list.Count > 0);
                            if (!existeList)
                            {
                                registro.Add("");
                            }
                            else
                            {
                                bool masDeUno = (list.Count > 1);
                                if (masDeUno)
                                {
                                    throw new Exception("Existe más de un registro en 'Comparativo Detalle' para " + eeu.Equinomb + " , fecha = " + fecha + " y hora = " + hora);
                                }
                                registro.Add((list[0].Rercdteneestimada != null ? list[0].Rercdteneestimada.Value.ToString() : ""));
                            }
                        }
                    }

                    Array.Resize(ref listaRegistros, listaRegistros.Length + 1);
                    listaRegistros[i] = registro;
                    i++;
                }
                RerExcelCuerpo cuerpo = CrearExcelCuerpo(listaRegistros, listaAlineaHorizontal, listaTipo);

                #endregion

                #endregion

                #region Return
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Energía Estimada",
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);

                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Setea los campos que se van a actualizar para EvaluacionSolicitudEdi
        /// Para el caso "Cuadro - Resultados"
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="aData"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private RerEvaluacionSolicitudEdiDTO SetEvaluacionSolicitudEdiByUpdateFieldsForResults(int rerevacodi, string[] aData, string usuario)
        {
            try
            {
                RerEvaluacionSolicitudEdiDTO entity = new RerEvaluacionSolicitudEdiDTO
                {
                    Reresecodi = Convert.ToInt32(aData[0]),
                    Rerevacodi = rerevacodi,
                    Reresefechahorainicio = DateTime.ParseExact(aData[2], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Reresefechahorafin = DateTime.ParseExact(aData[3], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Reresedesc = Convert.ToString(aData[7]),
                    Rereseresdesc = Convert.ToString(aData[9]),
                    Rereseusumodificacion = usuario,
                    Reresefecmodificacion = DateTime.Now
                };
                if (!string.IsNullOrWhiteSpace(aData[7]))
                {
                    entity.Rereseediaprobada = Convert.ToDecimal(aData[8]);
                }
                else
                {
                    entity.Rereseediaprobada = null;
                }
                return entity;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Valida los datos de EvaluacionSolicitudEdi
        /// </summary>
        /// <param name="rerevacodi"></param>
        /// <param name="dataht"></param>
        /// <param name="usuario"></param>
        /// <param name="evaluacion"></param>
        /// <returns>Retorna un mensaje de error, en caso no pase la validación</returns>
        private string ValidarActualizarEvaluacionSolicitudEdiParaResultados(int rerevacodi, string[][] dataht, string usuario, out RerEvaluacionDTO evaluacion)
        {
            try
            {
                evaluacion = null;
                StringBuilder sb = new StringBuilder();

                bool esRequeridoRerevacodi = (rerevacodi < 1);
                if (esRequeridoRerevacodi)
                {
                    sb.Append("Id de Evaluación es requerido");
                    return sb.ToString();
                }

                evaluacion = FactoryTransferencia.GetRerEvaluacionRepository().GetById(rerevacodi);
                bool existeEvaluacion = (evaluacion != null);
                if (!existeEvaluacion)
                {
                    sb.Append("No existe una evaluación con id = " + rerevacodi);
                    return sb.ToString();
                }

                bool noExisteUsuario = string.IsNullOrWhiteSpace(usuario);
                if (noExisteUsuario)
                {
                    sb.Append("El usuario no existe");
                    return sb.ToString();
                }

                bool existeDataht = (dataht != null);
                if (!existeDataht)
                {
                    sb.Append("Los datos del handsontable son requeridos.");
                    return sb.ToString();
                }

                int fila = 1;
                foreach (string[] data in dataht)
                {
                    DateTime? dtFechaHoraInicio = null;
                    DateTime? dtFechaHoraFin = null;

                    bool noExisteFechaHoraInicio = string.IsNullOrWhiteSpace(data[2]);
                    if (noExisteFechaHoraInicio)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe una fecha y hora de inicio para la fila {0}", fila);
                    }
                    else
                    {
                        try
                        {
                            dtFechaHoraInicio = DateTime.ParseExact(data[2], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("No es válida la fecha y hora de inicio para la fila {0}", fila);
                        }
                    }

                    bool noExisteFechaHoraFin = string.IsNullOrWhiteSpace(data[3]);
                    if (noExisteFechaHoraFin)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe una fecha y hora de fin para la fila {0}", fila);
                    }
                    else
                    {
                        try
                        {
                            dtFechaHoraFin = DateTime.ParseExact(data[3], ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("No es válida la fecha y hora de fin para la fila {0}", fila);
                        }
                    }

                    bool procederCompararFechas = (dtFechaHoraInicio != null && dtFechaHoraFin != null);
                    if (procederCompararFechas)
                    {
                        int compararFechas = DateTime.Compare(dtFechaHoraInicio.Value, dtFechaHoraFin.Value);
                        bool esFechaHoraInicioMayorOIgualFechaHoraFin = (compararFechas >= 0);
                        if (esFechaHoraInicioMayorOIgualFechaHoraFin)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("La 'fecha y hora de inicio' debe ser menor a la 'fecha y hora de fin' para la fila {0}", fila);
                        }
                    }

                    bool noExisteDesc = string.IsNullOrWhiteSpace(data[7]);
                    if (noExisteDesc)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe una descripción para la fila {0}", fila);
                    }

                    bool noExisteEDI = string.IsNullOrWhiteSpace(data[8]);
                    if (noExisteEDI)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe una magnitud EDI aprobada para la fila {0}", fila);
                    }
                    else
                    {
                        try { Convert.ToDecimal(data[8]); }
                        catch
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.AppendFormat("No es válida la magnitud EDI aprobada para la fila {0}", fila);
                        }
                    }

                    bool noExisteResDesc = string.IsNullOrWhiteSpace(data[9]);
                    if (noExisteResDesc)
                    {
                        if (sb.ToString().Length > 0) { sb.Append(", "); }
                        sb.AppendFormat("No existe el 'Resultado - Descripción' para la fila {0}", fila);
                    }

                    fila++;
                }

                if (sb.ToString().Length > 0) { sb.Append("."); }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        #endregion

        #endregion

        #region Insumos

        /// <summary>
        /// Genera reporte html del Listado de Solicitudes EDI de la Extranet para un periodo específico
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        public string GenerarHtmlListadoSolicitudesEdi(string url, int ipericodi)
        {
            try
            {
                List<RerSolicitudEdiDTO> listSE = FactoryTransferencia.GetRerSolicitudEdiRepository().GetByCriteria(ConstantesPrimasRER.numeroMenosUno, ipericodi);
                bool existeListSE = (listSE != null && listSE.Count > 0);
                if (existeListSE)
                {
                    listSE = listSE.Where(x => x.Rersedeliminado == ConstantesPrimasRER.eliminadoNo).ToList();
                    existeListSE = (listSE != null && listSE.Count > 0);
                    if (existeListSE)
                    {
                        listSE = listSE.OrderBy(x => x.Emprcodi).ThenBy(x => x.Equinomb).ThenBy(x => x.Rersedfechahorainicio).ThenBy(x => x.Rersedfechahorafin).ToList();
                    }
                }

                int i = 1;
                StringBuilder str = new StringBuilder();
                #region tabla
                str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla_solicitudes_edi'>");
                #region cabecera
                str.Append("<thead>");
                str.Append("<tr>");
                str.Append("<th style=''>N°</th>");
                str.Append("<th style=''>Id<br/>Solicitud<br/>EDI</th>");
                str.Append("<th style=''>Empresa</th>");
                str.Append("<th style=''>Central</th>");
                str.Append("<th style=''>Fecha y hora<br/>de inicio</th>");
                str.Append("<th style=''>Fecha y hora<br/>fin</th>");
                str.Append("<th style=''>Origen</th>");
                str.Append("<th style=''>Detalle</th>");
                str.Append("<th style=''>Solicitud EDI<br/>Total (MWh)</th>");
                str.Append("<th style='width: 30px;'>Registros<br/>MWh</th>");
                str.Append("<th style='width: 30px;'>Sustento</th>");
                str.Append("</tr>");
                str.Append("</thead>");
                #endregion
                #region cuerpo
                str.Append("<tbody>");
                foreach (var reg in listSE)
                {
                    str.Append("<tr>");
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", i);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rersedcodi);
                    str.AppendFormat("<td class='' style='text-align: left'>{0}</td>", reg.Emprnomb);
                    str.AppendFormat("<td class='' style='text-align: left'>{0}</td>", reg.Equinomb);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rersedfechahorainicio.ToString(ConstantesAppServicio.FormatoFechaFull));
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Rersedfechahorafin.ToString(ConstantesAppServicio.FormatoFechaFull));
                    str.AppendFormat("<td class='' style='text-align: left'>{0}</td>", reg.Reroridesc);
                    str.AppendFormat("<td class='' style='text-align: left'>{0}</td>", reg.Rerseddesc);
                    str.AppendFormat("<td class='' style='text-align: right'>{0}</td>", reg.Rersedtotenergia);
                    str.Append("<td style='text-align: center;'>");
                    str.AppendFormat("<a class='' href='JavaScript:descargarArchivoExcel({0});' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' width='25' height='25' src='{1}Content/Images/ExportExcel.png' alt='Descargar archivo Excel' title='Descargar archivo Excel' /></a>", reg.Rersedcodi, url);
                    str.Append("</td>");
                    str.Append("<td style='text-align: center;'>");
                    str.AppendFormat("<a class='' href='JavaScript:descargarArchivo(\"{0}\");' style='margin-right: 4px;'><img style='margin-top: 4px; margin-bottom: 4px;' src='{1}Content/Images/bajar.png' alt='Descargar archivo' title='Descargar archivo' /></a>", reg.Rersedsustento, url);
                    str.Append("</td>");
                    str.Append("</tr>");
                    i++;
                }
                str.Append("</tbody>");
                #endregion
                str.Append("</table>");
                #endregion

                return str.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera un archivo Excel con las energías de las unidades generadoras de una solicitud EDI de la Extranet
        /// </summary>
        /// <param name="rersedcodi"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelEnergiaUnidad(int rersedcodi)
        {
            try
            {
                #region Validación
                RerSolicitudEdiDTO solicitudEDI = BuscarSolicitudEDI(rersedcodi);
                bool existeSolicitudEDI = (solicitudEDI != null);
                if (!existeSolicitudEDI)
                {
                    throw new Exception("No existe una solicitud EDI con id = " + rersedcodi);
                }
                #endregion

                #region Generar hoja excel con los datos de las energias de las unidades de generación de una solicitud EDI de la extranet
                List<RerExcelHoja> listRerExcelHoja = GenerarExcelEnergiaUnidad(solicitudEDI.Rersedcodi, solicitudEDI.Rercencodi, solicitudEDI.Rersedfechahorainicio, solicitudEDI.Rersedfechahorafin);
                #endregion

                #region Return
                return listRerExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera un archivo Excel con los Eventos y Causas
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelEventosCausas(int ipericodi)
        {
            try
            {
                #region Validación
                INDAppServicio indAppServicio = new INDAppServicio();
                IndPeriodoDTO indPeriodo = indAppServicio.GetByIdIndPeriodo(ipericodi);
                bool existeIndPeriodo = (indPeriodo != null);
                if (!existeIndPeriodo)
                {
                    throw new Exception("No existe un periodo con id = " + ipericodi);
                }
                #endregion

                #region Variables
                DateTime fechaInicio = indPeriodo.FechaIni;
                DateTime fechaFin = indPeriodo.FechaFin.AddDays(1);
                #endregion

                #region Obtener datos de Eventos y Mantenimiento
                List<EventoDTO> listEvento = FactorySic.ObtenerEventoDao().BuscarEventos(0, fechaInicio, fechaFin, "-1", "", 0, 0, 0, "-1", -1, -1, "EVENINI", "desc", "-1", 1);

                int filasMantto = FactorySic.GetEveManttoRepository().ObtenerNroRegistros(ConstantesPrimasRER.IdEjecutadosEJ, fechaInicio, fechaFin, "-1", "-1", "-1", "-1", "-1", "-1");
                List<EveManttoDTO> listMantto = FactorySic.GetEveManttoRepository().BuscarMantenimientos(ConstantesPrimasRER.IdEjecutadosEJ, fechaInicio, fechaFin, "-1", "-1", "-1", "-1", "-1", "-1", 1, filasMantto);
                #endregion

                #region Armar archivo Excel

                #region Eventos

                #region Cabecera
                List<RerExcelModelo>[] eventosListaCabeceras = new List<RerExcelModelo>[1];
                eventosListaCabeceras[0] = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Empresa"),
                    CrearExcelModelo("Ubicación"),
                    CrearExcelModelo("Familia"),
                    CrearExcelModelo("Equipo"),
                    CrearExcelModelo("Inicio"),
                    CrearExcelModelo("CIER"),
                    CrearExcelModelo("Causa"),
                    CrearExcelModelo("Interrup."),
                    CrearExcelModelo("R"),
                    CrearExcelModelo("Descripción"),
                    CrearExcelModelo("Final"),
                    CrearExcelModelo("Usuario"),
                    CrearExcelModelo("Fecha")
                };

                List<int> eventosListaAnchoColumna = new List<int>
                {
                    50, //Empresa
                    50, //Ubicación
                    35, //Familia
                    20, //Equipo
                    25, //Inicio
                    15, //CIER
                    15, //Causa
                    15, //Interrup.
                    10, //R
                    80, //Descripción
                    25, //Final
                    15, //Usuario
                    25 //Fecha
                };
                #endregion

                #region Cuerpo
                List<string> eventosListaAlineaHorizontal = new List<string>
                {
                    "left", //Empresa
                    "left", //Ubicación
                    "center", //Familia
                    "center", //Equipo
                    "center", //Inicio
                    "center", //CIER
                    "center", //Causa
                    "center", //Interrup.
                    "center", //R
                    "left", //Descripción
                    "center", //Final
                    "center", //Usuario
                    "center" //Fecha
                };

                List<string> eventosListaTipo = new List<string>
                {
                    "string", //Empresa
                    "string", //Ubicación
                    "string", //Familia
                    "string", //Equipo
                    "string", //Inicio
                    "string", //CIER
                    "string", //Causa
                    "string", //Interrup.
                    "integer", //R
                    "string", //Descripción
                    "string", //Final
                    "string", //Usuario
                    "string" //Fecha
                };

                List<string>[] eventosListaRegistros = new List<string>[] { };
                bool existeListEvento = (listEvento != null && listEvento.Count > 0);
                if (existeListEvento)
                {
                    int i = 0;
                    eventosListaRegistros = new List<string>[listEvento.Count];
                    foreach (var evento in listEvento)
                    {
                        List<string> eventoRegistro = new List<string>
                        {
                            evento.EMPRNOMB,
                            evento.AREADESC,
                            evento.FAMNOMB,
                            evento.EQUIABREV,
                            ((evento.EVENINI != null) ? evento.EVENINI.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : ""),
                            evento.CAUSAEVENABREV,
                            evento.SUBCAUSAABREV,
                            evento.EVENINTERRUP,
                            ((evento.EVENRELEVANTE != null) ? evento.EVENRELEVANTE.ToString() : ""),
                            evento.EVENASUNTO,
                            ((evento.EVENFIN != null) ? evento.EVENFIN.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : ""),
                            evento.LASTUSER,
                            ((evento.LASTDATE != null) ? evento.LASTDATE.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : "")
                        };

                        eventosListaRegistros[i] = eventoRegistro;
                        i++;
                    }
                }

                RerExcelCuerpo eventosCuerpo = CrearExcelCuerpo(eventosListaRegistros, eventosListaAlineaHorizontal, eventosListaTipo);
                #endregion

                #endregion

                #region Mantenimiento

                #region Cabecera
                List<RerExcelModelo>[] mantenimientoListaCabeceras = new List<RerExcelModelo>[1];
                mantenimientoListaCabeceras[0] = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Tipo"),
                    CrearExcelModelo("Empresa"),
                    CrearExcelModelo("Ubicación"),
                    CrearExcelModelo("Tipo Equipo"),
                    CrearExcelModelo("Equipo"),
                    CrearExcelModelo("Inicio"),
                    CrearExcelModelo("Final"),
                    CrearExcelModelo("Descripción"),
                    CrearExcelModelo("PROG"),
                    CrearExcelModelo("Interrup."),
                    CrearExcelModelo("Indisp."),
                    CrearExcelModelo("Tensión"),
                    CrearExcelModelo("Tipo Mantto"),
                    CrearExcelModelo("CodEq"),
                    CrearExcelModelo("TipoEq Osinerg")
                };

                List<int> mantenimientoListaAnchoColumna = new List<int>
                {
                    20, //Tipo,
                    50, //Empresa,
                    30, //Ubicación,
                    30, //Tipo Equipo,
                    15, //Equipo,
                    20, //Inicio,
                    20, //Final,
                    120, //Descripción,
                    10, //PROG,
                    10, //Interrup.,
                    10, //Indisp.,
                    10, //Tensión,
                    10, //Tipo Mantto,
                    10, //CodEq,
                    10 //TipoEq_Osinerg
                };
                #endregion

                #region Cuerpo
                List<string> mantenimientoListaAlineaHorizontal = new List<string>
                {
                    "center", //Tipo,
                    "left", //Empresa,
                    "left", //Ubicación,
                    "left", //Tipo Equipo,
                    "center", //Equipo,
                    "center", //Inicio,
                    "center", //Final,
                    "left", //Descripción,
                    "center", //PROG,
                    "center", //Interrup.,
                    "center", //Indisp.,
                    "center", //Tensión,
                    "center", //Tipo Mantto,
                    "center", //CodEq,
                    "center" //TipoEq_Osinerg
                };

                List<string> mantenimientoListaTipo = new List<string>
                {
                    "string", //Tipo,
                    "string", //Empresa,
                    "string", //Ubicación,
                    "string", //Tipo Equipo,
                    "string", //Equipo,
                    "string", //Inicio,
                    "string", //Final,
                    "string", //Descripción,
                    "string", //PROG,
                    "string", //Interrup.,
                    "string", //Indisp.,
                    "double", //Tensión,
                    "string", //Tipo Mantto,
                    "integer", //CodEq,
                    "string" //TipoEq_Osinerg
                };

                List<string>[] mantenimientoListaRegistros = new List<string>[] { };
                bool existeListMantto = (listMantto != null && listMantto.Count > 0);
                if (existeListMantto)
                {
                    int i = 0;
                    mantenimientoListaRegistros = new List<string>[listMantto.Count];
                    foreach (var mantto in listMantto)
                    {
                        List<string> mantenimientoRegistro = new List<string>
                        {
                            mantto.Evenclasedesc, //Tipo,
                            mantto.Emprnomb, //Empresa,
                            mantto.Areanomb, //Ubicación,
                            mantto.Famnomb, //Tipo Equipo,
                            mantto.Equiabrev, //Equipo,
                            ((mantto.Evenini != null) ? mantto.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : ""), //Inicio,
                            ((mantto.Evenfin != null) ? mantto.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : ""), //Final,
                            mantto.Evendescrip, //Descripción,
                            mantto.Eventipoprog, //PROG,
                            mantto.Eveninterrup, //Interrup.,
                            mantto.Evenindispo, //Indisp.,
                            ((mantto.Equitension != null) ? mantto.Equitension.Value.ToString() : ""), //Tensión, 
                            mantto.Tipoevenabrev, //Tipo Mantto,
                            ((mantto.Equicodi != null) ? mantto.Equicodi.Value.ToString(): ""), //CodEq,
                            mantto.Osigrupocodi //TipoEq_Osinerg
                        };

                        mantenimientoListaRegistros[i] = mantenimientoRegistro;
                        i++;
                    }
                }

                RerExcelCuerpo mantenimientoCuerpo = CrearExcelCuerpo(mantenimientoListaRegistros, mantenimientoListaAlineaHorizontal, mantenimientoListaTipo);
                #endregion

                #endregion

                #endregion

                #region Return
                StringBuilder sbSubTitulo1 = new StringBuilder();
                sbSubTitulo1.AppendFormat("Desde: {0}   Hasta: {1}", indPeriodo.FechaIni.ToString(ConstantesAppServicio.FormatoFecha), indPeriodo.FechaFin.ToString(ConstantesAppServicio.FormatoFecha));

                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();

                RerExcelHoja eventosExcelHoja = new RerExcelHoja
                {
                    NombreHoja = "Eventos",
                    Titulo = "Consulta de Eventos",
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = eventosListaAnchoColumna,
                    ListaCabeceras = eventosListaCabeceras,
                    Cuerpo = eventosCuerpo
                };
                listExcelHoja.Add(eventosExcelHoja);

                RerExcelHoja mantenimientoExcelHoja = new RerExcelHoja
                {
                    NombreHoja = "Mantenimiento",
                    Titulo = "Consulta de Mantenimientos",
                    Subtitulo1 = sbSubTitulo1.ToString(),
                    ListaAnchoColumna = mantenimientoListaAnchoColumna,
                    ListaCabeceras = mantenimientoListaCabeceras,
                    Cuerpo = mantenimientoCuerpo
                };
                listExcelHoja.Add(mantenimientoExcelHoja);

                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        #endregion

        #endregion

        #region Exportación archivo Excel
        /// <summary>
        /// Genera un objeto RerExcelModelo (puede ser cabecera o pie)
        /// </summary>
        /// <param name="nombre">Valor de la celda</param>
        /// <param name="alineaHoriz">Alineación de la celda</param>
        /// <param name="numColumnas">Número de columnas de la celda</param>
        /// <param name="numFilas">Número de filas de la cenda</param>
        /// <returns>RerExcelModelo</returns>
        public RerExcelModelo CrearExcelModelo(string nombre, string alineaHoriz = "center", int numColumnas = 1, int numFilas = 1)
        {
            RerExcelModelo modelo = new RerExcelModelo
            {
                Nombre = nombre,
                AlineaHorizontal = alineaHoriz,
                NumColumnas = numColumnas,
                NumFilas = numFilas
            };
            return modelo;
        }

        /// <summary>
        /// Genera un objeto RerExcelEstilo
        /// Ejemplo: 
        /// List<RerExcelEstilo> listaEstilo = new List<RerExcelEstilo> {
        ///     CrearExcelEstilo(null, null, null, null, "#DADAD9", null,
        ///         new List<RerExcelEstilo> {
        ///             CrearExcelEstilo(null, null, null, "#ffff14", "#DADAD9", new List<string> { "0,0", "2,3", }, null),
        ///             CrearExcelEstilo(null, null, null, "#05faf6", "#DADAD9", new List<string> { "5,6", "8,10", }, null)
        ///         } ),
        ///     CrearExcelEstilo(null, null, null, null, "#DADAD9", null,
        ///         new List<RerExcelEstilo> {
        ///             CrearExcelEstilo(null, null, null, "#05faf6", "#DADAD9", new List<string> { "0,1", "5,6", }, null),
        ///             CrearExcelEstilo(null, null, null, "#05faf6", "#DADAD9", new List<string> { "8,8", "10,10", }, null)
        ///         } ),
        ///     CrearExcelEstilo("#,##0.0000"),
        ///     CrearExcelEstilo("#,##0.0000"),
        ///     CrearExcelEstilo("#,##0.0000")
        /// };
        /// </summary>
        /// <param name="numberformatFormat">Formato numérico. Ej: #,##0.0000</param>
        /// <param name="fontBold">Aplicar negrita al texto. Ej: true</param>
        /// <param name="fontColor">Color del texto. Ej: Color.White</param>
        /// <param name="fillBackgroundColor">Color del fondo. Ej: #2980B9</param>
        /// <param name="borderColor">Color del borde. Ej: #DADAD9</param>
        /// <param name="listRangoFilas">Lista de rango de filas en donde se aplicará el estilo. Si es nulo se aplica a toda la columna. Ej: new List<string> { "0,0", "1,3" } . Donde <filaInicial, filaFinal></param>
        /// <param name="listEstilo">Lista de estilos para filas específicas. Ej: new List<RerExcelEstilo> { CrearExcelEstilo(null, null, null, "#ffff14", "#DADAD9", new List<string> { "0,0" }, null) })</param>
        /// <returns>RerExcelModelo</returns>
        public RerExcelEstilo CrearExcelEstilo(string numberformatFormat = null, bool? fontBold = null, Color? fontColor = null, string fillBackgroundColor = null, string borderColor = null, List<string> listRangoFilas = null, List<RerExcelEstilo> listEstilo = null)
        {
            RerExcelEstilo estilo = new RerExcelEstilo
            {
                NumberformatFormat = numberformatFormat,
                FontBold = fontBold,
                FontColor = fontColor,
                FillBackgroundColor = fillBackgroundColor,
                BorderColor = borderColor,
                ListaRangoFilas = listRangoFilas,
                ListaEstilo = listEstilo
            };
            return estilo;
        }

        /// <summary>
        /// Genera un objeto RerExcelCuerpo
        /// </summary>
        /// <param name="listaRegistros">Matriz con el contenido del cuerpo del reporte</param>
        /// <param name="listaAlineaHorizontal">Lista con la alineación de las celdas</param>
        /// <param name="listaTipo">Lista con el tipo de dato de las celdas</param>
        /// <returns>RerExcelCuerpo</returns>
        public RerExcelCuerpo CrearExcelCuerpo(List<string>[] listaRegistros, List<string> listaAlineaHorizontal, List<string> listaTipo)
        {
            RerExcelCuerpo modelo = new RerExcelCuerpo
            {
                ListaRegistros = listaRegistros,
                ListaAlineaHorizontal = listaAlineaHorizontal,
                ListaTipo = listaTipo
            };
            return modelo;
        }

        /// <summary>
        /// Genera un reporte a excel(.xlsx)
        /// </summary>
        /// <param name="listaExcelHoja">Contenido del archivo a exportar, definido por hojas excel</param>
        /// <param name="rutaArchivo">Ruta donde se generará el reporte</param>
        /// <param name="nombreArchivo">Nombre del reporte. La aplicación colocará al final la extension: .xlsx</param>
        /// <param name="mostrarLogoTitulo">Bool que indica si el excel tendrá logo y titulo</param>
        /// <returns>Retorna el nombre del reporte generado. Nota: En caso de haber error devuelve -1</returns>
        public string ExportarReporteaExcel(List<RerExcelHoja> listaExcelHoja, string rutaArchivo, string nombreArchivo, bool mostrarLogoTitulo)
        {
            StringBuilder metodo = new StringBuilder();
            metodo.Append("PrimasRERAppServicio.ExportarReporteaExcel(List<RerExcelHoja> listaExcelHoja, string rutaArchivo, string nombreArchivo) - listaExcelHoja = ");
            metodo.Append(listaExcelHoja);
            metodo.Append(", nombreArchivo = ");
            metodo.Append(nombreArchivo);
            metodo.Append(", rutaArchivo = ");
            metodo.Append(rutaArchivo);

            string Reporte = nombreArchivo + ".xlsx";
            try
            {
                ExcelDocumentPrimasRER.ExportarReporte(listaExcelHoja, rutaArchivo + Reporte, mostrarLogoTitulo);
            }
            catch (Exception e)
            {
                metodo.Append(" , e.Message: ");
                metodo.Append(e.Message);
                Logger.Error(metodo.ToString());
                throw;
            }
            return Reporte;
        }
        #endregion

        #region Métodos Genelares
        /// <summary>
        /// Almacena un archivo en excel en un data set
        /// </summary>
        /// <param name="RutaArchivo"></param>
        /// <param name="Hoja"></param>
        public DataSet GeneraDataset(string RutaArchivo, int Hoja)
        {
            return UtilPrimasRER.GeneraDataset(RutaArchivo, Hoja);
        }
        #endregion

        #region Fuente de datos o insumos
        /// <summary>
        /// Lista de Insumos
        /// </summary>
        /// <returns></returns>
        public List<InsumoDTO> ListarInsumos(string sAnioTarifario, string sVersion)
        {
            List<InsumoDTO> entitys = new List<InsumoDTO>();

            List<RerInsumoDTO> listRerInsumoDto = new List<RerInsumoDTO>();

            // Lista de nombres de insumos
            string[] nombresInsumos = {
                "Inyección Neta 15 min.",
                "Costo Marginal 15 min.",
                "Ingreso por Potencia",
                "Ingreso por Cargo Prima RER",
                "Energía Dejada de Inyectar 15 min.",
                "Saldos VTEA 15 min.",
                "Saldo VTP"
            };

            if (sAnioTarifario != null && sAnioTarifario != "-1" && sVersion != null && sVersion != "-1") {
                int iRerAVAnioTarif = int.Parse(sAnioTarifario);
                int iRerAVVersion = int.Parse(sVersion);
                RerAnioVersionDTO rerAnioVersionDto = GetRerAnioVersionByAnioVersion(iRerAVAnioTarif, iRerAVVersion);

                for (int i = 1; i < 8; i++) {
                    RerInsumoDTO RerInsumoDTO = GetByReravcodiByRerinstipinsumoRerInsumo(rerAnioVersionDto.Reravcodi, i.ToString());
                    listRerInsumoDto.Add(RerInsumoDTO);
                }
            }

            // Crear y agregar instancias de InsumoDTO a la lista entitys
            int j = 0;
            foreach (string nombreInsumo in nombresInsumos)
            {
                InsumoDTO insumo = new InsumoDTO
                {
                    NomInsumo = nombreInsumo,
                    FecUltImportacion = ""
                };
                if (listRerInsumoDto != null && listRerInsumoDto.Count != 0)
                {
                    if (listRerInsumoDto[j] != null)
                    {
                        insumo.FecUltImportacion = listRerInsumoDto[j].Rerinsusucreacion + " [" + listRerInsumoDto[j].Rerinsfeccreacion.ToString("yyyy-MM-dd HH:mm:ss") + "]";
                    }
                    else {
                        insumo.FecUltImportacion = "-";
                    }

                }
                else {
                    insumo.FecUltImportacion = "-";
                }

                entitys.Add(insumo);
                j++;
            }

            return entitys;
        }

        /// <summary>
        /// Lista los Años Tarifario. 
        /// Considera los años tarifarios mayores a 2021
        /// </summary>
        /// <returns></returns>
        public List<RerAnioTarifarioDTO> ListarAniosTarifario()
        {
            List<RerAnioTarifarioDTO> listAnioTarifario = new List<RerAnioTarifarioDTO>();

            try
            {
                List<RerAnioVersionDTO> listAnioVersion = ListRerAnioVersionesFactorizado();
                foreach (RerAnioVersionDTO anioVersion in listAnioVersion)
                {
                    RerAnioTarifarioDTO anioTarifario = new RerAnioTarifarioDTO
                    {
                        Id = anioVersion.Reravcodi,
                        Anio = anioVersion.Reravaniotarif,
                        NomAnio = anioVersion.Reravaniotarifdesc
                    };
                    listAnioTarifario.Add(anioTarifario);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }

            return listAnioTarifario;
        }

        /// <summary>
        /// Lista de Versiones
        /// </summary>
        /// <returns></returns>
        public List<RerVersionDTO> ListarVersiones()
        {
            List<RerVersionDTO> entitys = new List<RerVersionDTO>();

            RerVersionDTO version = new RerVersionDTO { Numero = "0", Nombre = "Anual" };
            entitys.Add(version);

            version = new RerVersionDTO { Numero = "1", Nombre = "1er Ajuste Trimestral" };
            entitys.Add(version);

            version = new RerVersionDTO { Numero = "2", Nombre = "2do Ajuste Trimestral" };
            entitys.Add(version);

            version = new RerVersionDTO { Numero = "3", Nombre = "3er Ajuste Trimestral" };
            entitys.Add(version);

            version = new RerVersionDTO { Numero = "4", Nombre = "4to Ajuste Trimestral" };
            entitys.Add(version);

            version = new RerVersionDTO { Numero = "5", Nombre = "Liquidación" };
            entitys.Add(version);

            return entitys;
        }

        /// <summary>
        /// Lista los meses de un Año Tarifario con datos adicionales
        /// </summary>
        /// <returns></returns>
        public List<MesAnioTarifarioDTO> ListarMesesAnioTarifario(int Reravcodi, string tipoInsumo)
        {
            List<MesAnioTarifarioDTO> entitys = new List<MesAnioTarifarioDTO>();

            List<RerParametroPrimaDTO> ListaParametroPrima = GetParametroPrimaRerByAnioVersion(Reravcodi);

            foreach (var ParametroPrima in ListaParametroPrima)
            {
                MesAnioTarifarioDTO mesAnioTarifario = new MesAnioTarifarioDTO
                {
                    Rerpprcodi = ParametroPrima.Rerpprcodi,
                    NomMesAnio = ParametroPrima.Rerpprmesaniodesc,
                    NomMes = ConstantesPrimasRER.mesesDesc[ParametroPrima.Rerpprmes - 1],
                    Rerpprcodi_tipoInsumo = ParametroPrima.Rerpprcodi.ToString() + "_" + tipoInsumo,
                    TipoInsumo = tipoInsumo
                };
                entitys.Add(mesAnioTarifario);
            }

            return entitys;
        }

        /// <summary>
        /// Lista de Exportaciones
        /// </summary>
        /// <returns></returns>
        public List<ReporteDTO> ListarReportes()
        {
            List<ReporteDTO> entitys = new List<ReporteDTO>();

            ReporteDTO exportar = new ReporteDTO();
            exportar.Id = 1;
            exportar.NomReporte = "Ingreso por Potencia";
            entitys.Add(exportar);

            exportar = new ReporteDTO();
            exportar.Id = 2;
            exportar.NomReporte = "Ingreso por Energia";
            entitys.Add(exportar);

            exportar = new ReporteDTO();
            exportar.Id = 3;
            exportar.NomReporte = "Ingreso por Prima RER";
            entitys.Add(exportar);

            exportar = new ReporteDTO();
            exportar.Id = 4;
            exportar.NomReporte = "Saldos VTP";
            entitys.Add(exportar);

            exportar = new ReporteDTO();
            exportar.Id = 5;
            exportar.NomReporte = "Saldos VTA";
            entitys.Add(exportar);

            exportar = new ReporteDTO();
            exportar.Id = 6;
            exportar.NomReporte = "Saldo Mensual a Compensar";
            entitys.Add(exportar);


            return entitys;
        }

        /// <summary>
        /// Procesar los “gergnd.csv”, “gerhid.csv” y “sddp.dat”
        /// CUS20-Importar insumos del SDDP
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <param name="idVersion"></param>
        /// <param name="pathDirectorio"></param>
        /// <param name="usuario"></param>
        /// <returns>string</returns>
        public string ProcesarArchivosSddp(string idPeriodo, string idVersion, string pathDirectorio, string usuario)
        {
            RerSddpDTO entitySddp;
            RerGerCsvDTO entityGerCsv = null;
            List<string> arrayDataSddpDat = new List<string>();
            List<RerLecCsvTemp> entitysRerLeccsvTemp = new List<RerLecCsvTemp>();
            List<RerGerCsvDetDTO> entitysGerCsvDet = new List<RerGerCsvDetDTO>();
            string sResultado = "1";
            try {
                /* Valida el directorio de los archivos generados */
                bool existe = FileServer.VerificarLaExistenciaDirectorio(pathDirectorio);
                if (!existe)
                {
                    sResultado = "Verificar la existencia del directorio: La ruta '" + pathDirectorio + "' no es una ruta válida o el usuario no tiene acceso a dicha ruta.";
                    Logger.Error(sResultado);
                    return sResultado;
                }

                /* Valida la existencia de los archivos */
                StringBuilder sb = new StringBuilder();
                bool exiteGergnd = FileServer.VerificarExistenciaFile(null, "gergnd.csv", pathDirectorio);
                bool exiteGerhid = FileServer.VerificarExistenciaFile(null, "gerhid.csv", pathDirectorio);
                bool exiteSddp = FileServer.VerificarExistenciaFile(null, "sddp.dat", pathDirectorio);
                //Control de Cambios 2024-02-01
                bool exiteGerter = FileServer.VerificarExistenciaFile(null, "gerter.csv", pathDirectorio);
                bool exiteDuraci = FileServer.VerificarExistenciaFile(null, "duraci.csv", pathDirectorio);

                if (!exiteGergnd)
                {
                    sb.Append("gergnd.csv");
                }
                if (!exiteGerhid)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("gerhid.csv");
                }
                if (!exiteSddp)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("sddp.dat");
                }
                //Control de Cambios 2024-02-01
                if (!exiteGerter)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("gerter.csv");
                }
                if (!exiteDuraci)
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append("duraci.csv");
                }

                if (sb.Length > 0)
                {
                    sResultado = "No se encontraron los archivos SDDP: " + sb.ToString();
                    Logger.Error(sResultado);
                    return sResultado;
                }

                /* Lee archivo .DAT de acuerdo a la logica del ECUS20 */
                using (StreamReader sr = FileServer.OpenReaderFile("sddp.dat", pathDirectorio))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null) arrayDataSddpDat.Add(s);
                }
                int semIni = Convert.ToInt32(arrayDataSddpDat[12].Substring(28, 2).Trim());
                int anioIni = Convert.ToInt32(arrayDataSddpDat[13].Substring(26, 4).Trim());
                int serie = Convert.ToInt32(arrayDataSddpDat[18].Substring(28, 2).Trim());
                DateTime diaInicio = EPDate.f_fechainiciosemana(anioIni, semIni);

                entitySddp = new RerSddpDTO
                {
                    Reravcodi = Convert.ToInt32(idPeriodo),
                    Resddpnomarchivo = "sddp.dat_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".dat",
                    Resddpsemanaini = semIni,
                    Resddpanioini = anioIni,
                    Resddpnroseries = serie,
                    Resddpdiainicio = diaInicio,
                    Resddpusucreacion = usuario,
                    Resddpfeccreacion = DateTime.Now
                };
                int idSddp = SaveRerSddp(entitySddp);

                /* Llena tabla cabecera archivos gergnd.csv y gerhid.csv de acuerdo a la logica del ECUS20 */
                entityGerCsv = new RerGerCsvDTO
                {
                    Resddpcodi = idSddp,
                    Regergndarchivo = "gergnd_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                    Regerhidarchivo = "gerhid_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                    Regerusucreacion = usuario,
                    Regerfeccreacion = DateTime.Now
                };
                int idGerCsv = SaveRerGerCsv(entityGerCsv);

                /* Elimina los datos de las tablas temporales */
                FactoryTransferencia.GetRerGerCsvRepository().TruncateTablaTemporal(ConstantesPrimasRER.tablaRerLeccsvTemp);
                FactoryTransferencia.GetRerGerCsvRepository().TruncateTablaTemporal(ConstantesPrimasRER.tablaGerCsvDet);

                /* Lee detalle archivo gergnd.csv de acuerdo a la logica del ECUS20 */
                entitysRerLeccsvTemp = LeerArchivoGerCsv("gergnd.csv", pathDirectorio, "G", semIni, anioIni, serie);

                /*-----------------------------------------------------------------------------------------------------------------------------------*/

                /* Lee detalle archivo gerhid.csv de acuerdo a la logica del ECUS20 */
                entitysRerLeccsvTemp = LeerArchivoGerCsv("gerhid.csv", pathDirectorio, "H", semIni, anioIni, serie);

                //Control de Cambios 2024-02-01
                /*-----------------------------------------------------------------------------------------------------------------------------------*/

                /* Lee detalle archivo gergnd.csv de acuerdo a la logica del ECUS20 */
                entitysRerLeccsvTemp = LeerArchivoGerCsv("gerter.csv", pathDirectorio, "T", semIni, anioIni, serie);

                /*-----------------------------------------------------------------------------------------------------------------------------------*/

                /* Lee detalle archivo gergnd.csv de acuerdo a la logica del ECUS20 */
                entitysRerLeccsvTemp = LeerArchivoGerCsv("duraci.csv", pathDirectorio, "D", semIni, anioIni, serie);

                //fin de Control de Cambios

                /* Trae los datos filtrados y ordenados por etapa y bloque de la tabla temporal RER_LECCSV_TEMP para almacenarlos en RER_GERCSV_DET */
                entitysGerCsvDet = ObtenerCentralesPMPO(idGerCsv, usuario, serie);

                if (entitysGerCsvDet.Count > 0)
                {
                    /* Inserta los registros Detalle*/
                    FactoryTransferencia.GetRerGerCsvDetRepository().BulkInsertRerGerCsvDet(entitysGerCsvDet, ConstantesPrimasRER.tablaGerCsvDet);
                }
            }
            catch (Exception ex)
            {
                sResultado = ex.StackTrace;
                sResultado = ex.Message;
                return sResultado;
            }

            return sResultado;
        }

        /// <summary>
        /// Procesar archivos .csv de acuerdo a la logica del ECUS20
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="tipo"></param>
        /// <param name="semIni"></param>
        /// <param name="anioIni"></param>
        /// <param name="serie"></param>
        /// <returns>List RerLeccsvTemp</returns>
        public List<RerLecCsvTemp> LeerArchivoGerCsv(string fileName, string path, string tipo, int semIni, int anioIni, int serie)
        {
            RerLecCsvTemp entityRerLeccsvTemp;
            List<RerLecCsvTemp> entitysRerLeccsvTemp = new List<RerLecCsvTemp>();

            List<string> arrayDataGerGndCsv = new List<string>();
            using (StreamReader sr = FileServer.OpenReaderFile(fileName, path)) // Abre y lee el archivo
            {
                string s = "";
                while ((s = sr.ReadLine()) != null) arrayDataGerGndCsv.Add(s); // Llena el array con los datos del archivo
            }
            arrayDataGerGndCsv.RemoveRange(0, 3); // Elimina las 3 primeras filas

            List<string[]> cleanArrayData = new List<string[]>(); // Limpia los datos del array y lo combierte a matriz
            for (int i = 0; i <= arrayDataGerGndCsv.Count - 1; i++)
            {
                string[] arry = arrayDataGerGndCsv[i].Split(',');
                cleanArrayData.Add(arry);
            }

            // Trabajo las cabeceras de la matriz de la lectura
            for (int cabecera = 3; cabecera < cleanArrayData[0].Length; cabecera++) // Recorro las cabeceras (columnas) apartir 3ra (Centrales)
            {
                for (int fila = 1; fila < cleanArrayData.Count - 1; fila++) // Recorro los datos (filas) apartir 3ra cabecera (Centrales)
                {   //Control de cambios 2024-02-01 "<= serie"
                    int iEtapa = Convert.ToInt32(cleanArrayData[fila][0]);
                    if (iEtapa > 80)
                        break; // De la semana que inicia la lectura, 80 semanas mas. Para evitar quedarnos sin memoria
                    if (Convert.ToInt32(cleanArrayData[fila][1]) <= serie)
                    {
                        // Armo la lista con la data de las centrales (cabeceras)
                        entityRerLeccsvTemp = new RerLecCsvTemp();
                        entityRerLeccsvTemp.Rerfecinicio = EPDate.f_fechainiciosemana(anioIni, (Convert.ToInt32(cleanArrayData[fila][0]) + semIni - 1)); // Mayo.2022-Abril.2023 Semana Inicio = 27
                        entityRerLeccsvTemp.Reretapa = iEtapa;
                        entityRerLeccsvTemp.Rerserie = Convert.ToInt32(cleanArrayData[fila][1]);
                        entityRerLeccsvTemp.Rerbloque = Convert.ToInt32(cleanArrayData[fila][2]);
                        entityRerLeccsvTemp.Rercentrsddp = cleanArrayData[0][cabecera].Trim().ToUpper();
                        if (ValidarString(entityRerLeccsvTemp.Rercentrsddp)) // || (entityRerLeccsvTemp.Rercentrsddp.IndexOf("CARMEN") > 0)
                            entityRerLeccsvTemp.Rercentrsddp = RemplazarN(entityRerLeccsvTemp.Rercentrsddp); 
                        entityRerLeccsvTemp.Rervalor = (decimal)double.Parse(cleanArrayData[fila][cabecera]);
                        entityRerLeccsvTemp.Rertipcsv = tipo;
                        entitysRerLeccsvTemp.Add(entityRerLeccsvTemp);
                    }
                }
                if (entitysRerLeccsvTemp.Count > 20000)
                {
                    /* Inserta los datos leidos del archivo xxx.csv en la tabla temporal, liberando memoria */
                    FactoryTransferencia.GetRerGerCsvRepository().BulkInsertTablaTemporal(entitysRerLeccsvTemp, ConstantesPrimasRER.tablaRerLeccsvTemp);
                    entitysRerLeccsvTemp = new List<RerLecCsvTemp>();
                }
            }

            if (entitysRerLeccsvTemp.Count > 0)
            {
                /* Inserta los datos leidos restantes del archivo xxx.csv en la tabla temporal */
                FactoryTransferencia.GetRerGerCsvRepository().BulkInsertTablaTemporal(entitysRerLeccsvTemp, ConstantesPrimasRER.tablaRerLeccsvTemp);
                entitysRerLeccsvTemp = new List<RerLecCsvTemp>();
            }
            return entitysRerLeccsvTemp;
        }

        /// <summary>
        /// Procesar archivos .csv de acuerdo a la logica del ECUS20
        /// </summary>
        /// <param name="idGerCsv"></param>
        /// <param name="usuario"></param>
        /// <param name="serie"></param>
        /// <returns>List<RerGerCsvDetDTO></returns>
        public List<RerGerCsvDetDTO> ObtenerCentralesPMPO(int idGerCsv, string usuario, int serie)
        {
            List<RerGerCsvDetDTO> entitysRerGerCsvDetDTO = new List<RerGerCsvDetDTO>();
            int iRegedcodi = 1;
            //Definimos los dias de la semana
            char[] diasSemana = { 'S', 'D', 'L', 'M', 'X', 'J', 'V' };
            //Traemos la lista de feriados: listaferiado.Pmfrdofecha: Almacena las fechas que son Feriado PMPO
            List<PmoFeriadoDTO> listaferiado = FactorySic.GetPmoFeriadoRepository().List();
            //Lista de DURACIÓN
            List<RerLecCsvTemp> entitysRerLeccsvDuracion = FactoryTransferencia.GetRerGerCsvRepository().ListTablaTemporal("PERU");
            //Traemos la lista de CentralesRER Activos
            List<RerCentralDTO> entitysRerCentral = FactoryTransferencia.GetRerGerCsvRepository().ListEquiposEmpresasCentralesRer();
            foreach (RerCentralDTO central in entitysRerCentral)
            {
                //Para cada Centrarl RER, traemos la lista de Centrales PMPO
                List<RerCentralPmpoDTO> entitysCentralPmpo = FactoryTransferencia.GetRerGerCsvRepository().ListPtosMedicionCentralesPmpo(central.Rercencodi);
                //Recorremos la lista de Centrales PMPO
                foreach (RerCentralPmpoDTO centralPmpo in entitysCentralPmpo)
                {
                    //Por cada Central PMPO, lo relacionamos con la Central SDDP y lo asignamos a CentralRER
                    PmoSddpCodigoDTO entidad = FactoryTransferencia.GetRerGerCsvRepository().GetByCentralesSddp(centralPmpo.Ptomedicodi);
                    //entidad.Sddpnomb -> Tiene el Nombre de la CentralSDDP
                    //entidad.Emprcodi deberia ser igual a central.Emprcodi, pero para asegurarnos mantengo el central.Emprcodi
                    entidad.Emprcodi = central.Emprcodi; //Empresa
                    entidad.Equicodi = central.Equicodi; //CentralRER

                    //Vamos a poblar la tabla RER_GERCSV_DET
                    List<RerLecCsvTemp> entitysRerLeccsvTemp = FactoryTransferencia.GetRerGerCsvRepository().ListTablaTemporal(entidad.Sddpnomb.Trim().ToUpper());
                    //Lista de RerLeccsvTemp por una central SDDP
                    int iLimiteEtapa = 80;
                    if (entitysRerLeccsvTemp.Count < 80) iLimiteEtapa = entitysRerLeccsvTemp.Count;
                    for (int iContadorEtapa = 0; iContadorEtapa < iLimiteEtapa; iContadorEtapa++)
                    {
                        //Control de Cambios 2024-02-01: /serie, con ello se obtiene el promedio
                        //Energia = energia(j, k) /  (duraci(j, k) * 4 * nseries) * 1000 
                        decimal dDenom1 = entitysRerLeccsvDuracion[5 * iContadorEtapa].Rervalor * 4 * serie;
                        decimal dDenom2 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 1].Rervalor * 4 * serie;
                        decimal dDenom3 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 2].Rervalor * 4 * serie;
                        decimal dDenom4 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 3].Rervalor * 4 * serie;
                        decimal dDenom5 = entitysRerLeccsvDuracion[5 * iContadorEtapa + 4].Rervalor * 4 * serie;
                        //Tomamos los bloques de 5 en 5
                        decimal dBloque1 = 1000 * (entitysRerLeccsvTemp[5 * iContadorEtapa].Rervalor/dDenom1);
                        decimal dBloque2 = 1000 * (entitysRerLeccsvTemp[5 * iContadorEtapa + 1].Rervalor / dDenom2);
                        decimal dBloque3 = 1000 * (entitysRerLeccsvTemp[5 * iContadorEtapa + 2].Rervalor / dDenom3);
                        decimal dBloque4 = 1000 * (entitysRerLeccsvTemp[5 * iContadorEtapa + 3].Rervalor / dDenom4);
                        decimal dBloque5 = 1000 * (entitysRerLeccsvTemp[5 * iContadorEtapa + 4].Rervalor / dDenom5);
                        //Las 7 fechas de la semana 

                        //iniciando en Sabado:
                        DateTime dFecha = entitysRerLeccsvTemp[5 * iContadorEtapa].Rerfecinicio;
                        string sTipoCSV = entitysRerLeccsvTemp[5 * iContadorEtapa].Rertipcsv;

                        for (int i = 0; i < diasSemana.Length; i++)
                        {
                            char sFeriado = 'N';
                            var feriadoEncontrado = listaferiado.Where(x => x.Pmfrdofecha == dFecha).FirstOrDefault();
                            if (feriadoEncontrado != null)
                                sFeriado = 'S';
                            entitysRerGerCsvDetDTO.Add(addRerGerCsvDetalle(iRegedcodi++, dFecha, idGerCsv, entidad.Emprcodi, entidad.Equicodi, sTipoCSV, diasSemana[i], dBloque1, dBloque2, dBloque3, dBloque4, dBloque5, sFeriado, usuario));
                            dFecha = dFecha.AddDays(1);
                        }
                    }

                }
            }
            return entitysRerGerCsvDetDTO;
        }

        /// <summary>
        /// Completar un dia de insumos de acuerdo a la logica del ECUS20
        /// </summary>
        /// <param name="iRegedcodi"></param>
        /// <param name="dFecha"></param>
        /// <param name="idGerCsv"></param>
        /// <param name="iEmprcodi"></param>
        /// <param name="iEquicodi"></param>
        /// <param name="sTipoCSV"></param>
        /// <param name="sDia"></param>
        /// <param name="dBloque1"></param>
        /// <param name="dBloque2"></param>
        /// <param name="dBloque3"></param>
        /// <param name="dBloque4"></param>
        /// <param name="dBloque5"></param>
        /// <param name="sFeriado"></param>
        /// <param name="usuario"></param>
        /// <returns>RerGerCsvDetDTO</returns>
        public RerGerCsvDetDTO addRerGerCsvDetalle(int iRegedcodi, DateTime dFecha, int idGerCsv, int? iEmprcodi, int? iEquicodi, string sTipoCSV,
            char sDia, decimal dBloque1, decimal dBloque2, decimal dBloque3, decimal dBloque4, decimal dBloque5, char sFeriado, string usuario)
        {
            RerGerCsvDetDTO dtoRerGerCsvDet = new RerGerCsvDetDTO();
            dtoRerGerCsvDet.Regedcodi = iRegedcodi;
            dtoRerGerCsvDet.Regercodi = idGerCsv;
            dtoRerGerCsvDet.Emprcodi = iEmprcodi;
            dtoRerGerCsvDet.Equicodi = iEquicodi;
            dtoRerGerCsvDet.Regedtipcsv = sTipoCSV;
            dtoRerGerCsvDet.Regedfecha = dFecha;
            dtoRerGerCsvDet.Regedusucreacion = usuario;
            dtoRerGerCsvDet.Regedfeccreacion = DateTime.Now;

            //De Sabado a Viernes, el Bloque 5 se asigna sin restrigción
            //h1 -> h32 // 00:15 A 08:00
            int j = 1;
            while (j <= 32)
            {
                dtoRerGerCsvDet.GetType().GetProperty($"Regedh{(j)}").SetValue(dtoRerGerCsvDet, dBloque5);
                j++;
            }
            //h93 -> h96 // 23:15 A 00:00 DEL DIA SIGUIENTE
            j = 93;
            while (j <= 96)
            {
                dtoRerGerCsvDet.GetType().GetProperty($"Regedh{(j)}").SetValue(dtoRerGerCsvDet, dBloque5);
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 4 se asigna en el horario del h33 -> h72,
            //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 2
            j = 33;
            while (j <= 72)
            {
                if ((j >= 45 && j <= 48) && (sDia != 'S' && sDia != 'D' && sFeriado != 'S'))
                {
                    //Cumple para L, M. X, J y V en el intervalo h45-> h48
                    dtoRerGerCsvDet.GetType().GetProperty($"Regedh{(j)}").SetValue(dtoRerGerCsvDet, dBloque2);
                }
                else
                {
                    dtoRerGerCsvDet.GetType().GetProperty($"Regedh{(j)}").SetValue(dtoRerGerCsvDet, dBloque4);
                }
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 3 se asigna en el horario del h73 -> h92,
            //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 1
            j = 73;
            while (j <= 92)
            {
                if ((j >= 77 && j <= 78) && (sDia != 'S' && sDia != 'D' && sFeriado != 'S'))
                {
                    //Cumple para L, M. X, J y V en el intervalo h45-> h48
                    dtoRerGerCsvDet.GetType().GetProperty($"Regedh{(j)}").SetValue(dtoRerGerCsvDet, dBloque1);
                }
                else
                {
                    dtoRerGerCsvDet.GetType().GetProperty($"Regedh{(j)}").SetValue(dtoRerGerCsvDet, dBloque3);
                }
                j++;
            }
            return dtoRerGerCsvDet;
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto a los archivos SDDP
        /// </summary>
        /// <param name="sAnioTarifario"></param>
        /// <param name="sVersion"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelSDDP(string sAnioTarifario, string sVersion, string rutaArchivo, out string nombreArchivo, out List<RerExcelHoja> listExcelHoja)
        {
            try
            {
                listExcelHoja = new List<RerExcelHoja>();
                nombreArchivo = "ResumenSDDP_" + sAnioTarifario + "_v" + sVersion;

                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(Int32.Parse(sAnioTarifario), sVersion);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", sAnioTarifario, sVersion));
                }
                #endregion

                #region Obtener datos
                RerAnioVersionDTO ReranioversionDTO = GetRerAnioVersionByAnioVersion(Int32.Parse(sAnioTarifario), Int32.Parse(sVersion));
                RerSddpDTO RerSdd = FactoryTransferencia.GetRerSddpRepository().List().Where(item => item.Reravcodi == ReranioversionDTO.Reravcodi).LastOrDefault();
                if (RerSdd == null)
                {
                    throw new Exception("Aún no se realizó la importación para el Año Tarifario y versión seleccionados. Por favor, realizar la acción de 'Procesar'.");
                }

                RerGerCsvDTO RerGerCsv = FactoryTransferencia.GetRerGerCsvRepository().List().Where(item => item.Resddpcodi == RerSdd.Resddpcodi).LastOrDefault();

                if (RerGerCsv == null)
                {
                    throw new Exception("No se encontro un archivo para el id = " + RerSdd.Resddpcodi.ToString());
                }

                string sSDDP = RerSdd.Resddpnomarchivo;
                int iMes_semanaInicial = RerSdd.Resddpsemanaini;
                int iAnio = RerSdd.Resddpanioini;
                int iNroSerie = RerSdd.Resddpnroseries;

                string sGergnd = RerGerCsv.Regergndarchivo;
                string sGerhid = RerGerCsv.Regerhidarchivo;
                string sGerter = RerGerCsv.Regerterarchivo;


                List<RerGerCsvDetDTO> listGerGnd = FactoryTransferencia.GetRerGerCsvDetRepository().GetByEmprcodiEquicodiTipo(-1, -1, "G", ReranioversionDTO.Reravaniotarif);
                List<RerGerCsvDetDTO> listGerHid = FactoryTransferencia.GetRerGerCsvDetRepository().GetByEmprcodiEquicodiTipo(-1, -1, "H", ReranioversionDTO.Reravaniotarif);
                List<RerGerCsvDetDTO> listGerTer = FactoryTransferencia.GetRerGerCsvDetRepository().GetByEmprcodiEquicodiTipo(-1, -1, "T", ReranioversionDTO.Reravaniotarif);

                var listEmprcodisEquicodisGerGnd = listGerGnd
                .Select(item => new { Emprcodi = item.Emprcodi, Equicodi = item.Equicodi, Equinomb = item.Equinomb, Emprnomb = item.Emprnomb })
                .Distinct()
                .ToList();

                var listEmprcodisEquicodisGerHid = listGerHid
                .Select(item => new { Emprcodi = item.Emprcodi, Equicodi = item.Equicodi, Equinomb = item.Equinomb, Emprnomb = item.Emprnomb })
                .Distinct()
                .ToList();

                var listEmprcodisEquicodisGerTer = listGerTer
                .Select(item => new { Emprcodi = item.Emprcodi, Equicodi = item.Equicodi, Equinomb = item.Equinomb, Emprnomb = item.Emprnomb })
                .Distinct()
                .ToList();
                #endregion

                #region Variables
                string subtitulo1 = ReranioversionDTO.Reravaniotarifdesc;
                string subtitulo2 = ConstantesPrimasRER.versionesDesc[Int32.Parse(sVersion)];
                #endregion

                #region Hoja SDDP

                #region Titulo
                string tituloSDDP = "Resumen del archivo SDDP";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasSDDP = new List<RerExcelModelo>[1];
                List<RerExcelModelo> listaCabecera1SDDP = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Datos SDDP"),
                    CrearExcelModelo("Valor")
                };
                List<int> listaAnchoColumnaSDDP = new List<int>
                {
                    30,
                    30
                };

                listaCabecerasSDDP[0] = listaCabecera1SDDP;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalSDDP = new List<string> { "left", "left" };
                List<string> listaTipoSDDP = new List<string> { "string", "string" };
                List<RerExcelEstilo> listaEstiloSDDP = new List<RerExcelEstilo> {
                    CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                    null
                };

                List<string>[] listaRegistrosSDDP = new List<string>[6];
                listaRegistrosSDDP[0] = new List<string> { "SDDP", sSDDP };
                listaRegistrosSDDP[1] = new List<string> { "Mes/Semana Inicial", iMes_semanaInicial.ToString() };
                listaRegistrosSDDP[2] = new List<string> { "Año", iAnio.ToString() };
                listaRegistrosSDDP[3] = new List<string> { "Nro. serie", iNroSerie.ToString() };
                listaRegistrosSDDP[4] = new List<string> { "gergnd", sGergnd };
                listaRegistrosSDDP[5] = new List<string> { "gerhid", sGerhid };

                RerExcelCuerpo cuerpoSDDP = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosSDDP,
                    ListaAlineaHorizontal = listaAlineaHorizontalSDDP,
                    ListaTipo = listaTipoSDDP,
                    ListaEstilo = listaEstiloSDDP
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaSDDP = new RerExcelHoja
                {
                    NombreHoja = "SDDP",
                    Titulo = tituloSDDP,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaSDDP,
                    ListaCabeceras = listaCabecerasSDDP,
                    Cuerpo = cuerpoSDDP
                };
                listExcelHoja.Add(excelHojaSDDP);
                #endregion

                #endregion

                #region Hoja gergnd

                #region Titulo
                string tituloGergnd = "Resumen del archivo Gergnd";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasGergnd = new List<RerExcelModelo>[2];
                List<RerExcelModelo> listaCabecera1Gergnd = new List<RerExcelModelo>
                {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 2)
                };
                List<RerExcelModelo> listaCabecera2Gergnd = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaGergnd = new List<int> { 30 };
                foreach (var item in listEmprcodisEquicodisGerGnd)
                {
                    listaCabecera1Gergnd.Add(CrearExcelModelo(item.Equinomb.ToString()));
                    listaCabecera2Gergnd.Add(CrearExcelModelo(item.Equicodi.ToString()));
                    listaAnchoColumnaGergnd.Add(30);
                }

                listaCabecerasGergnd[0] = listaCabecera1Gergnd;
                listaCabecerasGergnd[1] = listaCabecera2Gergnd;
                #endregion

                #region Cuerpo
                var registrosPorFechaGerGnd = listGerGnd.GroupBy(item => item.Regedfecha.Date);
                int cantidadFechasDiferentesGerGnd = registrosPorFechaGerGnd.Count();

                List<string> listaAlineaHorizontalGergnd = new List<string> { "center" };
                List<string> listaTipoGergnd = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloGergnd = new List<RerExcelEstilo>();

                if (cantidadFechasDiferentesGerGnd > 0)
                {
                    listaEstiloGergnd.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                }

                foreach (var item in listEmprcodisEquicodisGerGnd)
                {
                    listaAlineaHorizontalGergnd.Add("right");
                    listaTipoGergnd.Add("double");
                    listaEstiloGergnd.Add(CrearExcelEstilo("#,##0.0000000"));
                }

                List<string>[] listaRegistrosGergnd;
                int posicionFecha = 0;

                if (cantidadFechasDiferentesGerGnd == 0)
                {
                    listaRegistrosGergnd = new List<string>[1];
                    listaRegistrosGergnd[0] = new List<string> { "No existen registros a exportar en Gergnd." };
                }
                else
                {
                    listaRegistrosGergnd = new List<string>[96 * cantidadFechasDiferentesGerGnd];
                }

                foreach (var grupo in registrosPorFechaGerGnd)
                {
                    var fecha = grupo.Key;
                    List<string> intervalosFormateados = ObtenerIntervalosConFormato(fecha);

                    List<RerGerCsvDetDTO> listEmprCent = grupo.ToList();

                    for (int indice = 1; indice <= 96; indice++)
                    {
                        int posicionRegistro = posicionFecha * 96 + indice - 1;
                        listaRegistrosGergnd[posicionRegistro] = new List<string> { intervalosFormateados[indice - 1] };
                        foreach (var emprCentr in listEmprcodisEquicodisGerGnd)
                        {
                            RerGerCsvDetDTO emprCent = listEmprCent.Where(item => item.Equicodi == emprCentr.Equicodi && item.Emprcodi == emprCentr.Emprcodi).FirstOrDefault();
                            string valorH = "0";
                            if (emprCent != null)
                            {
                                var propertyName = $"Regedh{indice}";
                                valorH = emprCent.GetType().GetProperty(propertyName).GetValue(emprCent).ToString();
                            }

                            listaRegistrosGergnd[posicionRegistro].Add(valorH);
                        }
                    }

                    posicionFecha++;
                }

                RerExcelCuerpo cuerpoGergnd = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosGergnd,
                    ListaAlineaHorizontal = listaAlineaHorizontalGergnd,
                    ListaTipo = listaTipoGergnd,
                    ListaEstilo = listaEstiloGergnd
                };

                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaGergnd = new RerExcelHoja
                {
                    NombreHoja = "Gergnd",
                    Titulo = tituloGergnd,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaGergnd,
                    ListaCabeceras = listaCabecerasGergnd,
                    Cuerpo = cuerpoGergnd
                };
                listExcelHoja.Add(excelHojaGergnd);
                #endregion

                #endregion

                #region hoja gerhid

                #region Titulo
                string tituloGerhid = "Resumen del archivo Gerhid";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasGerhid = new List<RerExcelModelo>[2];
                List<RerExcelModelo> listaCabecera1Gerhid = new List<RerExcelModelo>
                {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 2)
                };
                List<RerExcelModelo> listaCabecera2Gerhid = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaGerhid = new List<int> { 30 };
                foreach (var item in listEmprcodisEquicodisGerHid)
                {
                    listaCabecera1Gerhid.Add(CrearExcelModelo(item.Equinomb.ToString()));
                    listaCabecera2Gerhid.Add(CrearExcelModelo(item.Equicodi.ToString()));
                    listaAnchoColumnaGerhid.Add(30);
                }

                listaCabecerasGerhid[0] = listaCabecera1Gerhid;
                listaCabecerasGerhid[1] = listaCabecera2Gerhid;
                #endregion

                #region Cuerpo
                var registrosPorFechaGerHid = listGerHid.GroupBy(item => item.Regedfecha.Date);
                int cantidadFechasDiferentesGerHid = registrosPorFechaGerHid.Count();

                List<string> listaAlineaHorizontalGerhid = new List<string> { "center" };
                List<string> listaTipoGerhid = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloGerhid = new List<RerExcelEstilo>();

                if (cantidadFechasDiferentesGerHid > 0)
                {
                    listaEstiloGerhid.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                }

                foreach (var item in listEmprcodisEquicodisGerHid)
                {
                    listaAlineaHorizontalGerhid.Add("right");
                    listaTipoGerhid.Add("double");
                    listaEstiloGerhid.Add(CrearExcelEstilo("#,##0.0000000"));
                }

                List<string>[] listaRegistrosGerhid;
                posicionFecha = 0;
                if (cantidadFechasDiferentesGerHid == 0)
                {
                    listaRegistrosGerhid = new List<string>[1];
                    listaRegistrosGerhid[0] = new List<string> { "No existen registros a exportar en Gerhid." };
                }
                else {
                    listaRegistrosGerhid = new List<string>[96 * cantidadFechasDiferentesGerHid];
                }

                foreach (var grupo in registrosPorFechaGerHid)
                {
                    var fecha = grupo.Key;
                    List<string> intervalosFormateados = ObtenerIntervalosConFormato(fecha);

                    List<RerGerCsvDetDTO> listEmprCent = grupo.ToList();

                    for (int indice = 1; indice <= 96; indice++)
                    {
                        int posicionRegistro = posicionFecha * 96 + indice - 1;
                        listaRegistrosGerhid[posicionRegistro] = new List<string> { intervalosFormateados[indice - 1] };
                        foreach (var emprCentr in listEmprcodisEquicodisGerHid)
                        {
                            RerGerCsvDetDTO emprCent = listEmprCent.Where(item => item.Equicodi == emprCentr.Equicodi && item.Emprcodi == emprCentr.Emprcodi).FirstOrDefault();
                            string valorH = "0";
                            if (emprCent != null)
                            {
                                var propertyName = $"Regedh{indice}";
                                valorH = emprCent.GetType().GetProperty(propertyName).GetValue(emprCent).ToString();
                            }

                            listaRegistrosGerhid[posicionRegistro].Add(valorH);
                        }
                    }

                    posicionFecha++;
                }

                RerExcelCuerpo cuerpoGerhid = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosGerhid,
                    ListaAlineaHorizontal = listaAlineaHorizontalGerhid,
                    ListaTipo = listaTipoGerhid,
                    ListaEstilo = listaEstiloGerhid
                };

                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaGerhid = new RerExcelHoja
                {
                    NombreHoja = "Gerhid",
                    Titulo = tituloGerhid,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaGerhid,
                    ListaCabeceras = listaCabecerasGerhid,
                    Cuerpo = cuerpoGerhid
                };
                listExcelHoja.Add(excelHojaGerhid);
                #endregion

                #endregion

                #region hoja gerter

                #region Titulo
                string tituloGerter = "Resumen del archivo Gerter";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasGerter = new List<RerExcelModelo>[2];
                List<RerExcelModelo> listaCabecera1Gerter = new List<RerExcelModelo>
                {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 2)
                };
                List<RerExcelModelo> listaCabecera2Gerter = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaGerter = new List<int> { 30 };
                foreach (var item in listEmprcodisEquicodisGerTer)
                {
                    listaCabecera1Gerter.Add(CrearExcelModelo(item.Equinomb.ToString()));
                    listaCabecera2Gerter.Add(CrearExcelModelo(item.Equicodi.ToString()));
                    listaAnchoColumnaGerter.Add(30);
                }

                listaCabecerasGerter[0] = listaCabecera1Gerter;
                listaCabecerasGerter[1] = listaCabecera2Gerter;
                #endregion

                #region Cuerpo
                var registrosPorFechaGerTer = listGerTer.GroupBy(item => item.Regedfecha.Date);
                int cantidadFechasDiferentesGerTer = registrosPorFechaGerTer.Count();

                List<string> listaAlineaHorizontalGerTer = new List<string> { "center" };
                List<string> listaTipoGerter = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloGerter = new List<RerExcelEstilo>();

                if (cantidadFechasDiferentesGerTer > 0)
                {
                    listaEstiloGerter.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                }

                foreach (var item in listEmprcodisEquicodisGerHid)
                {
                    listaAlineaHorizontalGerTer.Add("right");
                    listaTipoGerter.Add("double");
                    listaEstiloGerter.Add(CrearExcelEstilo("#,##0.0000000"));
                }

                List<string>[] listaRegistrosGerter;
                posicionFecha = 0;
                if (cantidadFechasDiferentesGerTer == 0)
                {
                    listaRegistrosGerter = new List<string>[1];
                    listaRegistrosGerter[0] = new List<string> { "No existen registros a exportar en Gerhid." };
                }
                else
                {
                    listaRegistrosGerter = new List<string>[96 * cantidadFechasDiferentesGerTer];
                }

                foreach (var grupo in registrosPorFechaGerTer)
                {
                    var fecha = grupo.Key;
                    List<string> intervalosFormateados = ObtenerIntervalosConFormato(fecha);

                    List<RerGerCsvDetDTO> listEmprCent = grupo.ToList();

                    for (int indice = 1; indice <= 96; indice++)
                    {
                        int posicionRegistro = posicionFecha * 96 + indice - 1;
                        listaRegistrosGerter[posicionRegistro] = new List<string> { intervalosFormateados[indice - 1] };
                        foreach (var emprCentr in listEmprcodisEquicodisGerTer)
                        {
                            RerGerCsvDetDTO emprCent = listEmprCent.Where(item => item.Equicodi == emprCentr.Equicodi && item.Emprcodi == emprCentr.Emprcodi).FirstOrDefault();
                            string valorH = "0";
                            if (emprCent != null)
                            {
                                var propertyName = $"Regedh{indice}";
                                valorH = emprCent.GetType().GetProperty(propertyName).GetValue(emprCent).ToString();
                            }

                            listaRegistrosGerter[posicionRegistro].Add(valorH);
                        }
                    }

                    posicionFecha++;
                }

                RerExcelCuerpo cuerpoGerter = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosGerter,
                    ListaAlineaHorizontal = listaAlineaHorizontalGerTer,
                    ListaTipo = listaTipoGerter,
                    ListaEstilo = listaEstiloGerter
                };

                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaGerter = new RerExcelHoja
                {
                    NombreHoja = "Gerter",
                    Titulo = tituloGerter,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaGerter,
                    ListaCabeceras = listaCabecerasGerter,
                    Cuerpo = cuerpoGerter
                };
                listExcelHoja.Add(excelHojaGerter);
                #endregion
                
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        public static List<string> ObtenerIntervalosConFormato(DateTime dia)
        {
            List<string> intervalosFormateados = new List<string>();

            DateTime inicio = new DateTime(dia.Year, dia.Month, dia.Day, 0, 15, 0);

            while (inicio <= dia.Date.AddDays(1))
            {
                intervalosFormateados.Add(inicio.ToString("d/MM/yyyy HH:mm"));
                inicio = inicio.AddMinutes(15);
            }

            return intervalosFormateados;
        }

        public static List<string> ObtenerIntervalosXMesConFormato(DateTime dia)
        {
            List<string> intervalosFormateados = new List<string>();

            DateTime inicio = new DateTime(dia.Year, dia.Month, dia.Day, 0, 15, 0);

            while (inicio <= dia.Date.AddMonths(1))
            {
                intervalosFormateados.Add(inicio.ToString("dd/MM/yyyy HH:mm"));
                inicio = inicio.AddMinutes(15);
            }

            return intervalosFormateados;
        }

        public static List<string> ObtenerIntervalosMinutosXDiaConFormato(DateTime dia)
        {
            List<string> intervalosFormateados = new List<string>();

            DateTime inicio = new DateTime(dia.Year, dia.Month, dia.Day, 0, 15, 0);

            while (inicio < dia.Date.AddDays(1))
            {
                intervalosFormateados.Add(inicio.ToString("HH:mm"));
                inicio = inicio.AddMinutes(15);
            }
            intervalosFormateados.Add("24:00");
            return intervalosFormateados;
        }

        #endregion

        #region Procesar Cálculo de la Prima RER
        /// <summary>
        /// Procesa el cálculo RER para un Año Tarifario y un número de versión
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="numVersion"></param>
        /// <param name="usuario"></param>
        public void ProcesarCalculo(int anio, string numVersion, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validación
                bool noEsMayorAnioBase = (anio <= ConstantesPrimasRER.anioBase);
                if (noEsMayorAnioBase)
                {
                    throw new Exception("El año tarifario debe ser mayor a " + ConstantesPrimasRER.anioBase);
                }

                bool noEsValidoNumeroVersion = (!ConstantesPrimasRER.numeroVersiones.Contains(numVersion));
                if (noEsValidoNumeroVersion)
                {
                    throw new Exception("La versión del Año Tarifario no es válida");
                }

                RerAnioVersionDTO anioTarifario = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, numVersion);
                bool esNuloAnioTarifario = (anioTarifario == null);
                if (esNuloAnioTarifario)
                {
                    throw new Exception(string.Format("No existe un Año Tarifario con año tarifario = {0} y versión = {1}", anio, numVersion));
                }

                bool esEstadoAnioVersionAbierto = (anioTarifario.Reravestado != null && anioTarifario.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (!esEstadoAnioVersionAbierto)
                {
                    throw new Exception(string.Format("La versión '{0}' del Año Tarifario '{1}' no tiene su estado ‘abierto’. Realice la actualización de dicho estado en 'Parámetros de Prima RER', para continuar con el cálculo de la Prima RER.", anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc));
                }

                RerAnioVersionDTO anioTarifarioAnterior = ObtenerAnioTarifarioAnterior(anio);
                bool esNuloAnioTarifarioAnterior = (anioTarifarioAnterior == null);
                if (esNuloAnioTarifarioAnterior)
                {
                    throw new Exception(string.Format("No existe un Año Tarifario anterior para el Año Tarifario '{0}'", anioTarifario.Reravaniotarifdesc));
                }
                #endregion

                #region Obtener datos

                #region Declarar variables
                DateTime fechaInicioAnioTarifario = DateTime.ParseExact(string.Format("01/05/{0}", anioTarifario.Reravaniotarif), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinAnioTarifario = DateTime.ParseExact(string.Format("30/04/{0}", anioTarifario.Reravaniotarif + 1), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string paraLaVersionDelAnioTarifario = string.Format("para la versión '{0}' del Año Tarifario '{1}'", anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc);
                #endregion

                #region Obtener Insumos

                #region Obtener Insumo Mes
                List<RerInsumoMesDTO> listInsumoMesByAnioTarifario = FactoryTransferencia.GetRerInsumoMesRepository().GetByAnioTarifario(anioTarifario.Reravcodi, "-1");
                bool existeListInsumoMesByAnioTarifario = (listInsumoMesByAnioTarifario != null && listInsumoMesByAnioTarifario.Count > 0);
                if (!existeListInsumoMesByAnioTarifario)
                {
                    throw new Exception(string.Format("No se ha importado ningún insumo {0}", paraLaVersionDelAnioTarifario));
                }
                #endregion

                #region Obtener Insumo Mes - Inyección Neta Última Revisión, de todo el Año Tarifario
                List<RerInsumoMesDTO> listInyeccionNetaByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoInyeccionNeta).ToList();
                bool existeListInyeccionNetaByAnioTarifario = (listInyeccionNetaByAnioTarifario != null && listInyeccionNetaByAnioTarifario.Count > 0);
                if (!existeListInyeccionNetaByAnioTarifario)
                {
                    throw new Exception(string.Format("No se importó el insumo 'Inyección Neta' {0}", paraLaVersionDelAnioTarifario));
                }
                #endregion

                #region Obtener Insumo Mes - Inyección Neta Mensual, de todo el Año Tarifario
                List<RerInsumoMesDTO> listInyeccionNetaMensualByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoInyeccionNetaMensual).ToList();
                bool existeListInyeccionNetaMensualByAnioTarifario = (listInyeccionNetaMensualByAnioTarifario != null && listInyeccionNetaMensualByAnioTarifario.Count > 0);
                if (!existeListInyeccionNetaMensualByAnioTarifario)
                {
                    throw new Exception(string.Format("No se importó el insumo 'Inyección Neta' de tipo mensual {0}", paraLaVersionDelAnioTarifario));
                }
                #endregion

                #region Obtener Insumo Mes - Costo Marginal, de todo el Año Tarifario
                List<RerInsumoMesDTO> listCostoMarginalByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoCostoMarginal).ToList();
                bool existeListCostoMarginalByAnioTarifario = (listCostoMarginalByAnioTarifario != null && listCostoMarginalByAnioTarifario.Count > 0);
                if (!existeListCostoMarginalByAnioTarifario)
                {
                    throw new Exception(string.Format("No se importó el insumo 'Costo Marginal' {0}", paraLaVersionDelAnioTarifario));
                }
                #endregion

                #region Obtenes Insumo Mes - Ingresos por Potencia, de todo el Año Tarifario
                List<RerInsumoMesDTO> listIngresosPotenciaByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoIngresosPotencia).ToList();
                bool existeListIngresosPotenciaByAnioTarifario = (listIngresosPotenciaByAnioTarifario != null && listIngresosPotenciaByAnioTarifario.Count > 0);
                if (!existeListIngresosPotenciaByAnioTarifario)
                {
                    throw new Exception(string.Format("No se importó el insumo 'Ingresos por Potencia' {0}", paraLaVersionDelAnioTarifario));
                }
                #endregion

                #region Obtenes Insumo Mes - Ingresos por Cargo Prima RER, de todo el Año Tarifario
                List<RerInsumoMesDTO> listIngresosCargoPrimaRERByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER).ToList();
                bool existeListIngresosCargoPrimaRERByAnioTarifario = (listIngresosCargoPrimaRERByAnioTarifario != null && listIngresosCargoPrimaRERByAnioTarifario.Count > 0);
                if (!existeListIngresosCargoPrimaRERByAnioTarifario)
                {
                    throw new Exception(string.Format("No se importó el insumo 'Ingresos por Cargo Prima RER' {0}", paraLaVersionDelAnioTarifario));
                }
                #endregion

                #region Obtener Insumo Mes - Energía Dejada Inyectar, de todo el Año Tarifario
                List<RerInsumoMesDTO> listEDIByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar).ToList();
                bool existeListEDIByAnioTarifario = (listEDIByAnioTarifario != null && listEDIByAnioTarifario.Count > 0);
                #endregion

                #region Obtenes Insumo Mes - Saldo VTEA, de todo el Año Tarifario
                List<RerInsumoMesDTO> listSaldoVTEAByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoSaldosVTEA).ToList();
                bool existeListSaldoVTEAByAnioTarifario = (listSaldoVTEAByAnioTarifario != null && listSaldoVTEAByAnioTarifario.Count > 0);
                #endregion

                #region Obtenes Insumo Mes - Saldos VTP, de todo el Año Tarifario
                List<RerInsumoMesDTO> listSaldoVTPByAnioTarifario = listInsumoMesByAnioTarifario.Where(x => x.Rerinmtipresultado == ConstantesPrimasRER.tipoResultadoSaldosVTP).ToList();
                bool existeListSaldoVTPByAnioTarifario = (listSaldoVTPByAnioTarifario != null && listSaldoVTPByAnioTarifario.Count > 0);
                #endregion

                #endregion

                #region Obtener Centrales RER Únicas
                List<RerCentralUnicaDTO> listCentralUnica = ObtenerCentralesRERUnicas(anioTarifario.Reravaniotarif, out string rercacomment);
                bool existeListCentralUnica = (listCentralUnica != null && listCentralUnica.Count > 0);
                if (!existeListCentralUnica)
                {
                    throw new Exception(string.Format("No existe el listado de Empresas y Centrales Únicas"));
                }
                #endregion

                #endregion

                #region Obtener Cálculo de la Prima RER para cada Central RER Única

                foreach (RerCentralUnicaDTO centralUnica in listCentralUnica)
                {
                    #region Declarar variables
                    bool isFound = false;
                    decimal IN_nt_t_anterior = 0;
                    decimal suma_IN = 0;
                    decimal suma_IN_anterior = 0;
                    decimal eadj_dv_da = Obtener_EAdj_DV_DA(fechaInicioAnioTarifario, fechaFinAnioTarifario, centralUnica.Rercenfechainicio, centralUnica.Rercenenergadj);
                    centralUnica.ListCalculoMensual = new List<RerCalculoMensualDTO>();
                    #endregion

                    #region 1. Calcular "Tarifa Adjudicada"

                    #region 1.1 Cálculo “Factor de Actualización”
                    decimal ipp_i = anioTarifario.Reravinflacion;
                    decimal ipp_0 = centralUnica.Rerceninflabase;
                    decimal fa_i = Math.Round(ipp_i / ipp_0, ConstantesPrimasRER.numero4);
                    decimal fa_i_1 = centralUnica.FactorActualizacionAnterior;
                    decimal factor_actualizacion = (fa_i > (fa_i_1 * 1.05M)) ? fa_i : fa_i_1;
                    #endregion

                    #region 1.2 Cálculo “Factor de Corrección”
                    decimal factor_correccion = 0M;
                    decimal sum_E_EDI = SumarInyeccionNetaMasEDI(centralUnica.Emprcodi, centralUnica.Equicodi, listInyeccionNetaByAnioTarifario, listEDIByAnioTarifario);
                    if (sum_E_EDI >= centralUnica.Rercenenergadj)
                    {
                        factor_correccion = 1M;
                    }
                    else
                    {
                        decimal div__sum_E_EDI__EAdj = sum_E_EDI / centralUnica.Rercenenergadj;
                        factor_correccion = (div__sum_E_EDI__EAdj < 0) ? 0M : div__sum_E_EDI__EAdj;
                    }
                    #endregion

                    #region 1.3 Cálculo “Tarifa Adjudicada”
                    decimal tarifaadjudicada = Math.Round(factor_actualizacion * factor_correccion * centralUnica.Rercenprecbase, ConstantesPrimasRER.numero1);
                    centralUnica.CalculoAnual = new RerCalculoAnualDTO()
                    {
                        Reravcodi = anioTarifario.Reravcodi,
                        Emprcodi = centralUnica.Emprcodi,
                        Equicodi = centralUnica.Equicodi,
                        Rercaippi = anioTarifario.Reravinflacion,
                        Rercaippo = centralUnica.Rerceninflabase,
                        Rercataradjbase = centralUnica.Rercenprecbase,
                        Rercafaccorreccion = factor_correccion,
                        Rercafacactanterior = centralUnica.FactorActualizacionAnterior,
                        Rercafacactualizacion = factor_actualizacion,
                        Rercataradj = tarifaadjudicada,
                        Rercacomment = rercacomment
                    };
                    #endregion

                    #endregion

                    foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                    {
                        #region Obtener Datos

                        #region Seleccionar el año de acuerdo al mes del Año Tarifario
                        int anioSelected = ConstantesPrimasRER.mesesSiguientesAnioTarifario.Contains(mes) ? (anio + 1) : anio;
                        string paraElPeriodoVersionAnioTarifarioCentralEmpresa = string.Format("para el periodo '{0}-{1}' de la versión '{2}' del Año Tarifario '{3}' para la central '{4}' de la empresa '{5}'",
                                anioSelected, mes.ToString("D2"), anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc, centralUnica.Equinomb, centralUnica.Emprnomb);
                        #endregion

                        #region Obtener Insumos y Tipo de Cambio

                        #region Obtener Insumo Mes - Inyección Neta Última Revisión, para el año, mes, empresa y central especificados
                        List<RerInsumoMesDTO> listInyeccionNetaMes = listInyeccionNetaByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                        bool existeListInyeccionNetaMes = (listInyeccionNetaMes != null && listInyeccionNetaMes.Count > 0);
                        if (!existeListInyeccionNetaMes)
                        {
                            throw new Exception(string.Format("No se importó el insumo 'Inyección Neta' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                        }
                        RerInsumoMesDTO inyeccionNetaMes = listInyeccionNetaMes[0];
                        #endregion

                        #region Obtener Insumo Mes - Inyección Neta Mensual, para el año, mes, empresa y central especificados
                        List<RerInsumoMesDTO> listInyeccionNetaMensualMes = listInyeccionNetaMensualByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                        bool existeListInyeccionNetaMensualMes = (listInyeccionNetaMensualMes != null && listInyeccionNetaMensualMes.Count > 0);
                        if (!existeListInyeccionNetaMensualMes)
                        {
                            throw new Exception(string.Format("No se importó el insumo 'Inyección Neta' de tipo mensual {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                        }
                        RerInsumoMesDTO inyeccionNetaMensualMes = listInyeccionNetaMensualMes[0];
                        #endregion

                        #region Obtener Insumo Mes - Costo Marginal, para el año, mes, empresa y central especificados
                        List<RerInsumoMesDTO> listCostoMarginalMes = listCostoMarginalByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                        bool existeListCostoMarginalMes = (listCostoMarginalMes != null && listCostoMarginalMes.Count > 0);
                        if (!existeListCostoMarginalMes)
                        {
                            throw new Exception(string.Format("No se importó el insumo 'Costo Marginal' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                        }
                        RerInsumoMesDTO costoMarginalMes = listCostoMarginalMes[0];
                        #endregion

                        #region Obtener Insumo Mes - Ingresos por Potencia, para el año, mes, empresa y central especificados
                        List<RerInsumoMesDTO> listIngresosPotenciaMes = listIngresosPotenciaByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                        bool existeListIngresosPotenciaMes = (listIngresosPotenciaMes != null && listIngresosPotenciaMes.Count > 0);
                        if (!existeListIngresosPotenciaMes)
                        {
                            throw new Exception(string.Format("No se importó el insumo 'Ingresos por Potencia' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                        }
                        RerInsumoMesDTO ingresosPotenciaMes = listIngresosPotenciaMes[0];
                        #endregion

                        #region Obtener Insumo Mes - Ingresos por Cargo Prima RER, para el año, mes, empresa y central especificados
                        List<RerInsumoMesDTO> listIngresosCargoPrimaRERMes = listIngresosCargoPrimaRERByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                        bool existeListIngresosCargoPrimaRERMes = (listIngresosCargoPrimaRERMes != null && listIngresosCargoPrimaRERMes.Count > 0);
                        if (!existeListIngresosCargoPrimaRERMes)
                        {
                            throw new Exception(string.Format("No se importó el insumo 'Ingresos por Cargo Prima RER' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                        }
                        RerInsumoMesDTO ingresosCargoPrimaRERMes = listIngresosCargoPrimaRERMes[0];
                        #endregion

                        #region Obtener Insumo Mes - Energía Dejada Inyectar, para el año, mes, empresa y central especificados
                        RerInsumoMesDTO ediMes = null;
                        bool existeEDIMes = false;
                        if (existeListEDIByAnioTarifario)
                        {
                            List<RerInsumoMesDTO> listEdiMes = listEDIByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                            bool existeListEDIMes = (listEdiMes != null && listEdiMes.Count > 0);
                            if (existeListEDIMes)
                            {
                                ediMes = listEdiMes[0];
                                existeEDIMes = true;
                            }
                        }
                        #endregion

                        #region Obtener Insumo Mes - Saldos VTEA, para el año, mes, empresa y central especificados
                        RerInsumoMesDTO saldoVTEAMes = null;
                        if (existeListSaldoVTEAByAnioTarifario)
                        {
                            List<RerInsumoMesDTO> listSaldoVTEAMes = listSaldoVTEAByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                            bool existeListSaldoVTEAMes = (listSaldoVTEAMes != null && listSaldoVTEAMes.Count > 0);
                            if (existeListSaldoVTEAMes)
                            {
                                saldoVTEAMes = listSaldoVTEAMes[0];
                            }
                        }
                        #endregion

                        #region Obtener Insumo Mes - Saldos VTP, para el año, mes, empresa y central especificados
                        RerInsumoMesDTO saldoVTPMes = null;
                        if (existeListSaldoVTPByAnioTarifario)
                        {
                            List<RerInsumoMesDTO> listSaldoVTPMes = listSaldoVTPByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                            bool existeListSaldoVTPMEs = (listSaldoVTPMes != null && listSaldoVTPMes.Count > 0);
                            if (existeListSaldoVTPMEs)
                            {
                                saldoVTPMes = listSaldoVTPMes[0];
                            }
                        }
                        #endregion

                        #region Obtener Insumo Dia y Tipo de Cambio
                        List<RerInsumoDiaDTO> listInyeccionNetaDia = null;
                        List<RerInsumoDiaDTO> listInyeccionNetaMensualDia = null;
                        List<RerInsumoDiaDTO> listCostoMarginalDia = null;
                        List<RerInsumoDiaDTO> listEDIDia = null;
                        List<RerParametroPrimaDTO> listParametroPrima = null;
                        RerParametroPrimaDTO parametroPrima = null;
                        ConcurrentBag<string> cbErrores = new ConcurrentBag<string>();
                        List<Task> tasks = new List<Task>{
                            #region Obtener Insumo Día - Inyección Neta Última Revisión, para el año, mes, empresa y central especificados
                            Task.Run(() =>
                            {
                                listInyeccionNetaDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByMesByEmpresaByCentral(inyeccionNetaMes.Rerinmmescodi, centralUnica.Emprcodi, centralUnica.Equicodi);
                                bool existeListInyeccionNetaDia = (listInyeccionNetaDia != null && listInyeccionNetaDia.Count > 0);
                                if (!existeListInyeccionNetaDia)
                                {
                                    cbErrores.Add(string.Format("No se importó el insumo 'Inyección Neta' en la tabla 'rer_insumo_dia' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                                }
                            }),
                            #endregion

                            #region Obtener Insumo Día - Inyección Neta Mensual, para el año, mes, empresa y central especificados
                            Task.Run(() =>
                            {
                                listInyeccionNetaMensualDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByMesByEmpresaByCentral(inyeccionNetaMensualMes.Rerinmmescodi, centralUnica.Emprcodi, centralUnica.Equicodi);
                                bool existeListInyeccionNetaMensualDia = (listInyeccionNetaMensualDia != null && listInyeccionNetaMensualDia.Count > 0);
                                if (!existeListInyeccionNetaMensualDia)
                                {
                                    cbErrores.Add(string.Format("No se importó el insumo 'Inyección Neta' de tipo mensual en la tabla 'rer_insumo_dia' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                                }
                            }),
                            #endregion

                            #region Obtener Insumo Día - Costo Marginal, para el año, mes, empresa y central especificados
                            Task.Run(() =>
                            {
                                listCostoMarginalDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByMesByEmpresaByCentral(costoMarginalMes.Rerinmmescodi, centralUnica.Emprcodi, centralUnica.Equicodi);
                                bool existeListCostoMarginalDia = (listCostoMarginalDia != null && listCostoMarginalDia.Count > 0);
                                if (!existeListCostoMarginalDia)
                                {
                                    cbErrores.Add(string.Format("No se importó el insumo 'CostoMarginal' en la tabla 'rer_insumo_dia' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                                }
                            }),
                            #endregion

                            #region Obtener Insumo Día - Energía Dejada de Inyectar, para el año, mes, empresa y central especificados 
                            Task.Run(() =>
                            {
                                if (existeEDIMes)
                                {
                                    listEDIDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByMesByEmpresaByCentral(ediMes.Rerinmmescodi, centralUnica.Emprcodi, centralUnica.Equicodi);
                                }
                            }),
                            #endregion

                            #region Obtener Tipo de Cambio
                            Task.Run(() =>
                            {
                                listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioTarifario.Reravcodi, mes.ToString());
                                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                                if (!existeListParametroPrima)
                                {
                                    cbErrores.Add(string.Format("No existe un registro en la tabla rer_parametro_prima para reravcodi = {0} y rerpprmes = {1}", anioTarifario.Reravcodi, mes));
                                }
                                else
                                {
                                    parametroPrima = listParametroPrima[0];
                                    bool noExisteTipoCambio = (parametroPrima.Rerpprtipocambio == null);
                                    if (noExisteTipoCambio)
                                    {
                                        cbErrores.Add(string.Format("No existe un tipo de cambio para el periodo {0}-{1} para la versión '{2}' del Año Tarifario '{3}'", anioTarifario.Reravaniotarif, mes.ToString("D2"), anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc));
                                    }
                                }
                            }),
                            #endregion
                        };
                        Task.WaitAll(tasks.ToArray());
                        if (cbErrores.Count > 0)
                        {
                            throw new Exception(cbErrores.First<string>());
                        }
                        #endregion

                        #endregion

                        #endregion

                        #region 2. Calcular "Factor de Ajuste al Cumplimiento de Energía Adjudicada"
                        ObtenerDatosTipoIntervalo(anioSelected, mes, eadj_dv_da, listInyeccionNetaDia,
                                                ref isFound, ref suma_IN, ref suma_IN_anterior, ref IN_nt_t_anterior,
                                                out string tipo_intervalo, out DateTime? fechahora_intervalo, out decimal? valor_intervalo);
                        #endregion

                        #region 3. Calcular "Saldos Mensuales por Compensar"

                        #region 3.1 Obtener la suma de la multiplicación de Inyección Neta por Factor de Ajuste
                        decimal suma__multiplicacion_in_fa = SumarMultiplicacionInyeccionNetaPorFactorAjuste(anioSelected, mes, listInyeccionNetaDia,
                            tipo_intervalo, fechahora_intervalo, valor_intervalo);
                        #endregion

                        #region 3.2 Obtener total de insumos por periodo
                        decimal multiplicacion_tadj_sminfa = centralUnica.CalculoAnual.Rercataradj * suma__multiplicacion_in_fa;
                        decimal ingresoPorPotencia = ObtenerIngresoPorPotencia(anioSelected, mes, ingresosPotenciaMes.Rerinmmestotal, tipo_intervalo, fechahora_intervalo, valor_intervalo, out decimal suma_fa_div_n);
                        decimal ingresoPorPrimaRER = ingresosCargoPrimaRERMes.Rerinmmestotal;
                        decimal ingresoPorEnergia = ObtenerIngresoPorEnergia(anioSelected, mes, tipo_intervalo, fechahora_intervalo, valor_intervalo, listInyeccionNetaMensualDia, listCostoMarginalDia);
                        decimal saldoVTEA = (saldoVTEAMes != null ? saldoVTEAMes.Rerinmmestotal : 0M); //ObtenerSaldoVTEA(anioSelected, mes, tipo_intervalo, fechahora_intervalo, valor_intervalo, listSaldoVTEADia);
                        decimal saldoVTP = (saldoVTPMes != null ? saldoVTPMes.Rerinmmestotal : 0M); //ObtenerSaldoVTP(anioSelected, mes, tipo_intervalo, fechahora_intervalo, valor_intervalo, saldoVTPMes);
                        decimal imcp = ObtenerIMCP(ingresoPorPotencia, ingresoPorPrimaRER, ingresoPorEnergia, saldoVTEA, saldoVTP, parametroPrima.Rerpprtipocambio.Value);
                        decimal saldoMensualCompensar = ObtenerSaldoMensualPorCompensar(centralUnica.CalculoAnual.Rercataradj, suma__multiplicacion_in_fa, imcp);

                        RerCalculoMensualDTO calculoMensual = new RerCalculoMensualDTO()
                        {
                            Rerpprcodi = parametroPrima.Rerpprcodi,
                            Emprcodi = centralUnica.Emprcodi,
                            Equicodi = centralUnica.Equicodi,
                            Rercmfatipintervalo = tipo_intervalo,
                            Rercmfafecintervalo = fechahora_intervalo,
                            Rercmfavalintervalo = valor_intervalo,
                            Rercmtaradj = centralUnica.CalculoAnual.Rercataradj,
                            Rercmsummulinfa = suma__multiplicacion_in_fa,
                            Rercminggarantizado = multiplicacion_tadj_sminfa,
                            Rercminsingpotencia = ingresosPotenciaMes.Rerinmmestotal,
                            Rercmsumfadivn = suma_fa_div_n,
                            Rercmingpotencia = ingresoPorPotencia,
                            Rercmingprimarer = ingresoPorPrimaRER,
                            Rercmingenergia = ingresoPorEnergia,
                            Rercmsaldovtea = saldoVTEA,
                            Rercmsaldovtp = saldoVTP,
                            Rercmtipocambio = parametroPrima.Rerpprtipocambio.Value,
                            Rercmimcp = imcp,
                            Rercmsalmencompensar = saldoMensualCompensar
                        };
                        centralUnica.ListCalculoMensual.Add(calculoMensual);
                        #endregion

                        #endregion
                    }
                }

                #endregion

                #region Guardar datos del Cálculo de la Prima RER para cada Central RER Única

                #region Inicializar variables de conección y transacción
                conn = FactoryTransferencia.GetRerCalculoMensualRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerCalculoMensualRepository().StartTransaction(conn);
                #endregion

                #region Eliminar Cálculo de la Prima RER
                FactoryTransferencia.GetRerCalculoMensualRepository().DeleteByAnioVersion(anioTarifario.Reravcodi, conn, tran);
                FactoryTransferencia.GetRerCalculoAnualRepository().DeleteByAnioVersion(anioTarifario.Reravcodi, conn, tran);
                tran.Commit();
                tran = FactoryTransferencia.GetRerCalculoMensualRepository().StartTransaction(conn);
                #endregion

                #region Guardar "Tarifa Adjudicada"
                int rercacodi = FactoryTransferencia.GetRerCalculoAnualRepository().GetMaxId();
                foreach (RerCalculoAnualDTO calculoAnual in listCentralUnica.Select(x => x.CalculoAnual))
                {
                    calculoAnual.Rercacodi = rercacodi;
                    calculoAnual.Rercausucreacion = usuario;
                    calculoAnual.Rercafeccreacion = DateTime.Now;
                    FactoryTransferencia.GetRerCalculoAnualRepository().Save(calculoAnual, conn, tran);
                    rercacodi++;
                }
                #endregion

                #region Guardar "Factor de Ajuste al Cumplimiento de Energía Adjudicada" y "Saldos Mensuales por Compensar"
                int rercmcodi = FactoryTransferencia.GetRerCalculoMensualRepository().GetMaxId();
                foreach (List<RerCalculoMensualDTO> listCalculoMensual in listCentralUnica.Select(x => x.ListCalculoMensual))
                {
                    foreach (RerCalculoMensualDTO calculoMensual in listCalculoMensual)
                    {
                        calculoMensual.Rercmcodi = rercmcodi;
                        calculoMensual.Rercmusucreacion = usuario;
                        calculoMensual.Rercmfeccreacion = DateTime.Now;
                        FactoryTransferencia.GetRerCalculoMensualRepository().Save(calculoMensual, conn, tran);
                        rercmcodi++;
                    }
                }
                #endregion

                tran.Commit();

                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                bool conexionEstaAbierta = (conn != null && conn.State == ConnectionState.Open);
                if (conexionEstaAbierta)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Elimina el cálculo RER para un Año Tarifario y un número de versión
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="numVersion"></param>
        /// <param name="usuario"></param>
        public void EliminarCalculo(int anio, string numVersion, string usuario)
        {
            #region Definiendo variables globales
            IDbConnection conn = null;
            DbTransaction tran = null;
            #endregion

            try
            {
                #region Validación
                bool noEsMayorAnioBase = (anio <= ConstantesPrimasRER.anioBase);
                if (noEsMayorAnioBase)
                {
                    throw new Exception("El año tarifario debe ser mayor a " + ConstantesPrimasRER.anioBase);
                }

                bool noEsValidoNumeroVersion = (!ConstantesPrimasRER.numeroVersiones.Contains(numVersion));
                if (noEsValidoNumeroVersion)
                {
                    throw new Exception("La versión no es válido");
                }

                RerAnioVersionDTO anioTarifario = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, numVersion);
                bool esNuloAnioTarifario = (anioTarifario == null);
                if (esNuloAnioTarifario)
                {
                    throw new Exception(string.Format("No existe un registro en la tabla rer_annioversion con año tarifario = {0} y versión = {1}", anio, numVersion));
                }

                bool esEstadoAnioVersionAbierto = (anioTarifario.Reravestado != null && anioTarifario.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (!esEstadoAnioVersionAbierto)
                {
                    throw new Exception(string.Format("La versión '{0}' del Año Tarifario '{1}' no tiene su estado ‘abierto’. Realice la actualización de dicho estado en 'Parámetros de Prima RER', para continuar con el cálculo de la Prima RER.", anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc));
                }
                #endregion

                #region Eliminar Cálculo de la Prima RER
                conn = FactoryTransferencia.GetRerCalculoMensualRepository().BeginConnection();
                tran = FactoryTransferencia.GetRerCalculoMensualRepository().StartTransaction(conn);

                FactoryTransferencia.GetRerCalculoMensualRepository().DeleteByAnioVersion(anioTarifario.Reravcodi, conn, tran);
                FactoryTransferencia.GetRerCalculoAnualRepository().DeleteByAnioVersion(anioTarifario.Reravcodi, conn, tran);

                tran.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
            finally
            {
                bool conexionEstaAbierta = (conn != null && conn.State == ConnectionState.Open);
                if (conexionEstaAbierta)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Obtener Factor de Ajuste para un Año Tarifario y un número de versión
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="numVersion"></param>
        public List<RerCentralUnicaDTO> ObtenerFAJ(int anio, string numVersion)
        {
            try
            {
                #region Validación
                bool noEsMayorAnioBase = (anio <= ConstantesPrimasRER.anioBase);
                if (noEsMayorAnioBase)
                {
                    throw new Exception("El año tarifario debe ser mayor a " + ConstantesPrimasRER.anioBase);
                }

                bool noEsValidoNumeroVersion = (!ConstantesPrimasRER.numeroVersiones.Contains(numVersion));
                if (noEsValidoNumeroVersion)
                {
                    throw new Exception("La versión no es válido");
                }

                RerAnioVersionDTO anioTarifario = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, numVersion);
                bool esNuloAnioTarifario = (anioTarifario == null);
                if (esNuloAnioTarifario)
                {
                    throw new Exception(string.Format("No existe un Año Tarifario con año tarifario = {0} y versión = {1}", anio, numVersion));
                }
                #endregion

                #region Obtener datos

                #region Declarar variables
                DateTime fechaInicioAnioTarifario = DateTime.ParseExact(string.Format("01/05/{0}", anioTarifario.Reravaniotarif), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinAnioTarifario = DateTime.ParseExact(string.Format("30/04/{0}", anioTarifario.Reravaniotarif + 1), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                string paraLaVersionDelAnioTarifario = string.Format("para la versión '{0}' del Año Tarifario '{1}'", anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc);
                #endregion

                #region Obtener Centrales RER Únicas e Insumo Año - Inyección Neta Última Revisión
                List<RerCentralUnicaDTO> listCentralUnica = null;
                List<RerInsumoMesDTO> listInyeccionNetaByAnioTarifario = null;
                ConcurrentBag<string> cbErrores = new ConcurrentBag<string>();
                List<Task> tasks = new List<Task>{
                    #region Obtener Centrales RER Únicas
                    Task.Run(() =>
                    {
                        listCentralUnica = ObtenerCentralesRERUnicas(anioTarifario.Reravaniotarif, out _);
                        bool existeListCentralUnica = (listCentralUnica != null && listCentralUnica.Count > 0);
                        if (!existeListCentralUnica)
                        {
                            cbErrores.Add(string.Format("No existe el listado de Empresas y Centrales Únicas"));
                        }
                    }),
                    #endregion

                    #region Obtener Insumo Año - Inyección Neta Última Revisión, de todo el Año Tarifario
                    Task.Run(() =>
                    {
                        listInyeccionNetaByAnioTarifario = FactoryTransferencia.GetRerInsumoMesRepository().GetByAnioTarifario(anioTarifario.Reravcodi, ConstantesPrimasRER.tipoResultadoInyeccionNeta);
                        bool existeListInyeccionNetaByAnioTarifario = (listInyeccionNetaByAnioTarifario != null && listInyeccionNetaByAnioTarifario.Count > 0);
                        if (!existeListInyeccionNetaByAnioTarifario)
                        {
                            cbErrores.Add(string.Format("No se importó el insumo 'Inyección Neta' {0}", paraLaVersionDelAnioTarifario));
                        }
                    }),
                    #endregion
                };
                Task.WaitAll(tasks.ToArray());
                if (cbErrores.Count > 0)
                {
                    throw new Exception(cbErrores.First<string>());
                }
                #endregion

                #endregion

                #region Obtener FAJ para cada Central RER Única

                foreach (RerCentralUnicaDTO centralUnica in listCentralUnica)
                {
                    #region Declarar variables
                    bool isFound = false;
                    decimal IN_nt_t_anterior = 0;
                    decimal suma_IN = 0;
                    decimal suma_IN_anterior = 0;
                    decimal eadj_dv_da = Obtener_EAdj_DV_DA(fechaInicioAnioTarifario, fechaFinAnioTarifario, centralUnica.Rercenfechainicio, centralUnica.Rercenenergadj);
                    centralUnica.ListCalculoMensual = new List<RerCalculoMensualDTO>();
                    #endregion

                    foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                    {
                        #region Obtener Datos

                        #region Seleccionar el año de acuerdo al mes del Año Tarifario
                        int anioSelected = ConstantesPrimasRER.mesesSiguientesAnioTarifario.Contains(mes) ? (anio + 1) : anio;
                        string paraElPeriodoVersionAnioTarifarioCentralEmpresa = string.Format("para el periodo '{0}-{1}' de la versión '{2}' del Año Tarifario '{3}' para la central '{4}' de la empresa '{5}'",
                                anioSelected, mes.ToString("D2"), anioTarifario.Reravversiondesc, anioTarifario.Reravaniotarifdesc, centralUnica.Equinomb, centralUnica.Emprnomb);
                        #endregion

                        #region Obtener Insumo Día - Inyección Neta Última Revisión y Parametro Prima

                        #region Obtener Insumo Mes - Inyección Neta Última Revisión
                        List<RerInsumoMesDTO> listInyeccionNetaMes = listInyeccionNetaByAnioTarifario.Where(x => x.Rerinmanio == anioSelected && x.Rerinmmes == mes && x.Emprcodi == centralUnica.Emprcodi && x.Equicodi == centralUnica.Equicodi).ToList();
                        bool existeListInyeccionNetaMes = (listInyeccionNetaMes != null && listInyeccionNetaMes.Count > 0);
                        if (!existeListInyeccionNetaMes)
                        {
                            throw new Exception(string.Format("No se importó el insumo 'Inyección Neta' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                        }
                        RerInsumoMesDTO inyeccionNetaMes = listInyeccionNetaMes[0];
                        #endregion

                        #region Obtener Insumo Día - Inyección Neta Última Revisión y Parametro Prima
                        List<RerInsumoDiaDTO> listInyeccionNetaDia = null;
                        List<RerParametroPrimaDTO> listParametroPrima = null;
                        RerParametroPrimaDTO parametroPrima = null;
                        cbErrores = new ConcurrentBag<string>();
                        tasks = new List<Task>{
                            #region Obtener Insumo Día - Inyección Neta Última Revisión, para el año, mes, empresa y central especificados
                            Task.Run(() =>
                            {
                                listInyeccionNetaDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByMesByEmpresaByCentral(inyeccionNetaMes.Rerinmmescodi, centralUnica.Emprcodi, centralUnica.Equicodi);
                                bool existeListInyeccionNetaDia = (listInyeccionNetaDia != null && listInyeccionNetaDia.Count > 0);
                                if (!existeListInyeccionNetaDia)
                                {
                                    cbErrores.Add(string.Format("No se importó el insumo 'Inyección Neta' en la tabla 'rer_insumo_dia' {0}", paraElPeriodoVersionAnioTarifarioCentralEmpresa));
                                }
                            }),
                            #endregion

                            #region Obtener Parametro Prima 
                            Task.Run(() =>
                            {
                                listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioTarifario.Reravcodi, mes.ToString());
                                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                                if (!existeListParametroPrima)
                                {
                                    cbErrores.Add(string.Format("No existe un registro en la tabla rer_parametro_prima para reravcodi = {0} y rerpprmes = {1}", anioTarifario.Reravcodi, mes));
                                }
                                else
                                {
                                    parametroPrima = listParametroPrima[0];
                                }
                            }),
                            #endregion
                        };
                        Task.WaitAll(tasks.ToArray());
                        if (cbErrores.Count > 0)
                        {
                            throw new Exception(cbErrores.First<string>());
                        }
                        #endregion

                        #endregion

                        #endregion

                        #region 2. Calcular "Factor de Ajuste al Cumplimiento de Energía Adjudicada"
                        ObtenerDatosTipoIntervalo(anioSelected, mes, eadj_dv_da, listInyeccionNetaDia,
                                                ref isFound, ref suma_IN, ref suma_IN_anterior, ref IN_nt_t_anterior,
                                                out string tipo_intervalo, out DateTime? fechahora_intervalo, out decimal? valor_intervalo);

                        RerCalculoMensualDTO calculoMensual = new RerCalculoMensualDTO()
                        {
                            Rerpprcodi = parametroPrima.Rerpprcodi,
                            Emprcodi = centralUnica.Emprcodi,
                            Equicodi = centralUnica.Equicodi,
                            Rercmfatipintervalo = tipo_intervalo,
                            Rercmfafecintervalo = fechahora_intervalo,
                            Rercmfavalintervalo = valor_intervalo,
                            Rerpprmes = mes,
                        };
                        centralUnica.ListCalculoMensual.Add(calculoMensual);
                        #endregion
                    }
                }

                return listCentralUnica;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera el comentario para el campo rercacomment de la tabla rer_calculo_anual
        /// </summary>
        /// <param name="calculoAnual"></param>
        /// <returns></returns>
        private string getRercacomment(RerCalculoAnualDTO calculoAnual)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("rercacodi=");
            sb.Append(calculoAnual.Rercacodi);
            sb.Append(";reravcodi=");
            sb.Append(calculoAnual.Reravcodi);
            sb.Append(";rercausucreacion=");
            sb.Append(calculoAnual.Rercausucreacion);
            sb.Append(";rercafeccreacion=");
            sb.Append(calculoAnual.Rercafeccreacion);
            return sb.ToString();
        }

        /// <summary>
        /// Obtiene la última versión del Año Tarifario Anterior con respecto al año ingresado (el cual representa al Año Tarifario actual con el que se está trabajando)
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        private RerAnioVersionDTO ObtenerAnioTarifarioAnterior(int anio)
        {
            //int anioAnterior = anio - 1;
            int anioAnterior = anio;
            RerAnioVersionDTO anioTarifarioAnterior = null;
            for (int i = ConstantesPrimasRER.numeroVersiones.Length - 1; i >= 0; i--)
            {
                anioTarifarioAnterior = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anioAnterior, ConstantesPrimasRER.numeroVersiones[i]);
                bool existeAnioTarifarioAnterior = (anioTarifarioAnterior != null);
                if (existeAnioTarifarioAnterior)
                {
                    break;
                }
            }

            return anioTarifarioAnterior;
        }

        /// <summary>
        /// Obtiene la última versión del Cálculo Anual Anterior con respecto al año ingresado
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        private List<RerCalculoAnualDTO> ObtenerCalculoAnualAnterior(int anio)
        {
            int anioAnterior = anio - 1;
            List<RerCalculoAnualDTO> listCalculoAnualAnterior = null;
            for (int i = ConstantesPrimasRER.numeroVersiones.Length - 1; i >= 0; i--)
            {
                listCalculoAnualAnterior = FactoryTransferencia.GetRerCalculoAnualRepository().GetByAnnioAndVersion(anioAnterior, ConstantesPrimasRER.numeroVersiones[i]);
                bool existeListCalculoAnualAnterior = (listCalculoAnualAnterior != null && listCalculoAnualAnterior.Count > 0);
                if (existeListCalculoAnualAnterior)
                {
                    break;
                }
            }

            return listCalculoAnualAnterior;
        }

        /// <summary>
        /// Devuelve Centrales RER únicas. Donde su fecha de contrato esté dentro del rango de fecha del Año Tarifario, y su estado sea activo
        /// </summary>
        /// <remarks>
        /// Las propiedades seteadas con un valor son: 
        /// Rercencodi: en caso de ser 0, significa que tuvo más de una Central RER para el mismo par Emprcodi-Equicodi,
        /// Emprcodi,
        /// Equicodi,
        /// Emprnomb,
        /// Equinomb,
        /// Rercenfechainicio, 
        /// Rercenenergadj,  
        /// Rercenprecbase, 
        /// Rerceninflabase,
        /// FactorActualizacionAnterior
        /// </remarks>
        /// <param name="reravaniotarif">Año Tarifario a trabajar</param>
        /// <param name="rercacomment">Contiene los siguientes datos: Rercacodi, Reravcodi, Rercausucreacion, Rercafeccreacion del primer registro del cálculo anual anterior</param>
        /// <returns></returns>
        private List<RerCentralUnicaDTO> ObtenerCentralesRERUnicas(int reravaniotarif, out string rercacomment)
        {
            DateTime fechaInicioAnioTarifario = DateTime.ParseExact(string.Format("01/05/{0}", reravaniotarif), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinAnioTarifario = DateTime.ParseExact(string.Format("30/04/{0}", reravaniotarif + 1), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            List<RerCalculoAnualDTO> listCalculoAnualAnterior = null;
            List<RerCentralDTO> listCentralRER = null;
            ConcurrentBag<string> cbErrores = new ConcurrentBag<string>();
            List<Task> tasks = new List<Task>{
                Task.Run(() =>
                {
                    listCalculoAnualAnterior = ObtenerCalculoAnualAnterior(reravaniotarif);
                    bool existeListCalculoAnualAnterior = (listCalculoAnualAnterior != null && listCalculoAnualAnterior.Count > 0);
                    if (!existeListCalculoAnualAnterior)
                    {
                        cbErrores.Add(string.Format("Debe ejecutar 'Procesar Cálculo de la Prima RER' para un Año Tarifario anterior al Año Tarifario Mayo.{0}-Abril.{1}", reravaniotarif, reravaniotarif + 1));
                    }
                }),
                Task.Run(() =>
                {
                    listCentralRER = FactoryTransferencia.GetRerCentralRepository().ListByFechasEstado(fechaInicioAnioTarifario.ToString(ConstantesAppServicio.FormatoFecha), fechaFinAnioTarifario.ToString(ConstantesAppServicio.FormatoFecha), ConstantesPrimasRER.estadoActivo);
                    bool existeListCentralRER = (listCentralRER != null && listCentralRER.Count > 0);
                    if (!existeListCentralRER)
                    {
                        cbErrores.Add(string.Format("No existen centrales RER con fechas de contrato entre el rango de {0} hasta {1} ", fechaInicioAnioTarifario, fechaFinAnioTarifario));
                    }
                })
            };
            Task.WaitAll(tasks.ToArray());
            if (cbErrores.Count > 0)
            {
                throw new Exception(cbErrores.First<string>());
            }

            rercacomment = getRercacomment(listCalculoAnualAnterior[0]);
            List<RerCentralUnicaDTO> listCentralRERUnica = new List<RerCentralUnicaDTO>();
            var listCentralRERGroupBy = listCentralRER.GroupBy(x => new { x.Emprcodi, x.Equicodi }).Select(x => x.Key);

            foreach (var g in listCentralRERGroupBy)
            {
                List<RerCentralDTO> listCentralRERWhere = listCentralRER.Where(x => x.Emprcodi == g.Emprcodi && x.Equicodi == g.Equicodi).ToList();
                bool existeListCentralRERWhere = (listCentralRERWhere != null && listCentralRERWhere.Count > 0);
                if (!existeListCentralRERWhere)
                {
                    throw new Exception(string.Format("Dentro de la lista de Centrales RER no existe una central RER con Emprcodi = {0} y Equicodi = {1}", g.Emprcodi, g.Equicodi));
                }

                int totalDias = 0;
                decimal energiaAdjudicada = 0;
                decimal precioBase = 0;
                decimal inflacionBase = 0;
                decimal fa_i_1 = ObtenerFactorActualizacionAnterior(listCalculoAnualAnterior, listCentralRERWhere[0].Emprcodi, listCentralRERWhere[0].Equicodi);
                bool existeMasDeUno = (listCentralRERWhere.Count > ConstantesPrimasRER.numero1);

                if (existeMasDeUno)
                {
                    foreach (var item in listCentralRERWhere)
                    {
                        int dias = ObtenerDiasRangoFechaContrato(fechaInicioAnioTarifario, fechaFinAnioTarifario, item.Rercenfechainicio, item.Rercenfechafin);
                        totalDias += dias;
                        energiaAdjudicada += (item.Rercenenergadj * dias);
                        precioBase += item.Rercenprecbase;
                        inflacionBase += item.Rerceninflabase;
                    }
                    energiaAdjudicada /= totalDias;
                    precioBase /= listCentralRERWhere.Count;
                    inflacionBase /= listCentralRERWhere.Count;

                }
                else
                {
                    energiaAdjudicada = listCentralRERWhere[0].Rercenenergadj;
                    precioBase = listCentralRERWhere[0].Rercenprecbase;
                    inflacionBase = listCentralRERWhere[0].Rerceninflabase;
                }

                RerCentralUnicaDTO centralUnica = new RerCentralUnicaDTO()
                {
                    Emprcodi = listCentralRERWhere[0].Emprcodi,
                    Equicodi = listCentralRERWhere[0].Equicodi,
                    Emprnomb = (!string.IsNullOrWhiteSpace(listCentralRERWhere[0].Emprnomb)) ? listCentralRERWhere[0].Emprnomb.Trim() : "",
                    Equinomb = (!string.IsNullOrWhiteSpace(listCentralRERWhere[0].Equinomb)) ? listCentralRERWhere[0].Equinomb.Trim() : "",
                    Rercenfechainicio = listCentralRERWhere[0].Rercenfechainicio,
                    Rercenenergadj = energiaAdjudicada,
                    Rercenprecbase = precioBase,
                    Rerceninflabase = inflacionBase,
                    FactorActualizacionAnterior = fa_i_1
                };
                listCentralRERUnica.Add(centralUnica);
            }

            return listCentralRERUnica;
        }

        /// <summary>
        /// Obtiene el "factor de actualización anterior" de una lista de "Cálculo Anual Anterior" para una Central RER (empresa-central) 
        /// </summary>
        /// <param name="listCalculoAnualAnterior"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <returns></returns>
        private decimal ObtenerFactorActualizacionAnterior(List<RerCalculoAnualDTO> listCalculoAnualAnterior, int emprcodi, int equicodi)
        {
            decimal fa = 0M;

            try
            {
                bool existeListCalculoAnualAnterior = (listCalculoAnualAnterior != null && listCalculoAnualAnterior.Count > 0);
                if (existeListCalculoAnualAnterior)
                {
                    List<RerCalculoAnualDTO> list = listCalculoAnualAnterior.Where(x => x.Emprcodi == emprcodi && x.Equicodi == equicodi).ToList();
                    bool existeList = (list != null && list.Count > 0);
                    if (existeList)
                    {
                        fa = list[0].Rercafacactualizacion;
                    }
                }
            }
            catch
            {
                //La consulta arrojó null. Por lo tanto, no existe un registro para dicha consulta. En este caso, se va a retornar 0
            }

            return fa;
        }

        /// <summary>
        /// Retorna el número de días que participa una CentralRER dentro del rango de fechas de su contrato y para un Año Tarifario
        /// </summary>
        /// <param name="fechaInicioAnioTarifario"></param>
        /// <param name="fechaFinAnioTarifario"></param>
        /// <param name="rercenfechainicio"></param>
        /// <param name="rercenfechafin"></param>
        /// <returns></returns>
        private int ObtenerDiasRangoFechaContrato(DateTime fechaInicioAnioTarifario, DateTime fechaFinAnioTarifario, DateTime rercenfechainicio, DateTime rercenfechafin)
        {
            fechaInicioAnioTarifario = fechaInicioAnioTarifario.Date;
            fechaFinAnioTarifario = fechaFinAnioTarifario.Date;
            rercenfechainicio = rercenfechainicio.Date;
            rercenfechafin = rercenfechafin.Date;

            int result = DateTime.Compare(rercenfechainicio, fechaInicioAnioTarifario);
            if (result < 0)
            {
                rercenfechainicio = fechaInicioAnioTarifario;
            }

            result = DateTime.Compare(rercenfechafin, fechaFinAnioTarifario);
            if (result > 0)
            {
                rercenfechafin = fechaFinAnioTarifario;
            }

            int dias = (rercenfechafin - rercenfechainicio).Days + 1;
            return dias;
        }

        /// <summary>
        /// Realiza la sumatoria del "Insumo - Inyección Neta" más el "Insumo - Energía Dejada Inyectar" 
        /// con respecto al Año Tarifario para una empresa y una central
        /// Nota: 
        /// Esta suma se realiza de manera directa. Es decir, primero se suma el insumo de Inyección Neta, segundo se suma el insumo de Energia Dejada de Inyectar, 
        /// y finalmente se suman estos 2 resultados
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="equicodi"></param>
        /// <param name="listInyeccionNetaByAnioTarifario">Insumos - Inyeccion Neta por un Año Tarifario</param>
        /// <param name="listEDIByAnioTarifario">Insumos - Energía Dejada de Inyectar por un Año Tarifario</param>
        /// <returns></returns>
        private decimal SumarInyeccionNetaMasEDI(int emprcodi, int equicodi, List<RerInsumoMesDTO> listInyeccionNetaByAnioTarifario, List<RerInsumoMesDTO> listEDIByAnioTarifario)
        {
            decimal sumaIN = 0M;
            decimal sumaEDI = 0M;
            decimal sumaTotal = 0M;

            if (listInyeccionNetaByAnioTarifario != null && listInyeccionNetaByAnioTarifario.Count > 0)
            {
                List<RerInsumoMesDTO> listIN = listInyeccionNetaByAnioTarifario.Where(x => x.Emprcodi == emprcodi && x.Equicodi == equicodi).ToList();
                sumaIN = (listIN != null && listIN.Count > 0) ? listIN.Sum(x => x.Rerinmmestotal) : 0M;
            }

            if (listEDIByAnioTarifario != null && listEDIByAnioTarifario.Count > 0)
            {
                List<RerInsumoMesDTO> listEDI = listEDIByAnioTarifario.Where(x => x.Emprcodi == emprcodi && x.Equicodi == equicodi).ToList();
                sumaEDI = (listEDI != null && listEDI.Count > 0) ? listEDI.Sum(x => x.Rerinmmestotal) : 0M;
            }

            sumaTotal = sumaIN + sumaEDI;
            return sumaTotal;
        }

        /// <summary>
        /// Obtener Energía Adjudicada * (dv / da);
        /// </summary>
        /// <param name="fechaInicioAnioTarifario"></param>
        /// <param name="fechaFinAnioTarifario"></param>
        /// <param name="rercenfechainicio"></param>
        /// <param name="rercenenergadj"></param>
        /// <returns></returns>
        private decimal Obtener_EAdj_DV_DA(DateTime fechaInicioAnioTarifario, DateTime fechaFinAnioTarifario, DateTime rercenfechainicio, decimal rercenenergadj)
        {
            decimal division_dv_da = 1M;
            if (DateTime.Compare(rercenfechainicio, fechaInicioAnioTarifario) > 0)
            {
                int dv = (fechaFinAnioTarifario - rercenfechainicio).Days + 1;
                int da = (fechaFinAnioTarifario - fechaInicioAnioTarifario).Days + 1;
                division_dv_da = Convert.ToDecimal(dv / da);
            }

            decimal eadj_dv_da = rercenenergadj * division_dv_da;
            return eadj_dv_da;
        }

        /// <summary>
        /// Obtiene los datos del tipo de intervalo para el periodo especificado.
        /// Nota:
        /// listINDia debe ser del mismo: mes, empresa y central 
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="eadj_dv_da"></param>
        /// <param name="listINDia"></param>
        /// <param name="isFound"></param>
        /// <param name="suma_IN"></param>
        /// <param name="suma_IN_anterior"></param>
        /// <param name="IN_nt_t_anterior"></param>
        /// <param name="tipo_intervalo"></param>
        /// <param name="fechahora_intervalo"></param>
        /// <param name="valor_intervalo"></param>
        private void ObtenerDatosTipoIntervalo(int anio, int mes, decimal eadj_dv_da, List<RerInsumoDiaDTO> listINDia, 
            ref bool isFound, ref decimal suma_IN, ref decimal suma_IN_anterior, ref decimal IN_nt_t_anterior,
            out string tipo_intervalo, out DateTime? fechahora_intervalo, out decimal? valor_intervalo)
        {
            try
            {
                tipo_intervalo = null;
                fechahora_intervalo = null;
                valor_intervalo = null;

                if (isFound)
                {   //Ya se encontró el intervalo de quiebre. Por lo tanto, todos sus valores siguientes son 0
                    tipo_intervalo = ConstantesPrimasRER.tipoIntervalo0;
                    return;
                }

                int intervalosPorDia = ConstantesPrimasRER.numero96;
                DateTime fechaInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), anio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = fechaInicio.AddMonths(1).AddDays(-1);
                DateTime fecha = fechaInicio;

                while (!isFound && fecha <= fechaFinal)
                {
                    int i = 1;
                    int minutos = ConstantesPrimasRER.numero15;

                    RerInsumoDiaDTO inDia = ObtenerInsumoDiaPorFecha(listINDia, fecha);
                    if (inDia != null)
                    {
                        while (!isFound && i <= intervalosPorDia)
                        {
                            decimal IN_nt_t = 0M;
                            if (inDia != null)
                            {
                                var value = inDia.GetType().GetProperty("Rerinddiah" + i).GetValue(inDia, null); 
                                IN_nt_t = (value != null) ? (decimal)value : 0M;
                            }

                            suma_IN += IN_nt_t;

                            if (suma_IN < eadj_dv_da) {
                                tipo_intervalo = ConstantesPrimasRER.tipoIntervalo1;
                            }
                            else {
                                //Se encontró el intervalo de quiebre
                                tipo_intervalo = ConstantesPrimasRER.tipoIntervalo2;
                                fechahora_intervalo = fecha.AddMinutes(minutos); //Fecha cuando finaliza el intervalo
                                bool esVerdad = (suma_IN > eadj_dv_da && suma_IN_anterior < eadj_dv_da);
                                valor_intervalo = (esVerdad && IN_nt_t_anterior != 0) ? ((eadj_dv_da - suma_IN_anterior) / IN_nt_t_anterior) : 0M;
                                isFound = true;
                            }

                            i++;
                            minutos += ConstantesPrimasRER.numero15;

                            IN_nt_t_anterior = IN_nt_t;
                            suma_IN_anterior += IN_nt_t;
                        }
                    }
                    else
                    {
                        IN_nt_t_anterior = 0M;
                    }

                    fecha = fecha.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene insumoDia correspondiente a una fecha específica
        /// </summary>
        /// <param name="listInsumoDia">Todos los registros de insumo_dia con respecto a un mes y a un insumo</param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private RerInsumoDiaDTO ObtenerInsumoDiaPorFecha(List<RerInsumoDiaDTO> listInsumoDia, DateTime fecha)
        {
            try
            {
                RerInsumoDiaDTO insumoDia = null;
                if (listInsumoDia != null && listInsumoDia.Count > 0)
                {
                    List<RerInsumoDiaDTO> list = listInsumoDia.Where(x => x.Rerinddiafecdia == fecha).ToList();
                    if (list != null && list.Count > 0)
                    {
                        insumoDia = list[0];
                    }
                }
                return insumoDia;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene la suma de la multiplicación de Inyección Neta por Factor de Ajuste, 
        /// de los intervalos de 15 minutos con respecto a un periodo de un Año Tarifario
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="listInyeccionNetaDia"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <returns></returns>
        private decimal SumarMultiplicacionInyeccionNetaPorFactorAjuste(int anio, int mes, List<RerInsumoDiaDTO> listInyeccionNetaDia,
            string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo)
        {
            decimal suma__multiplicar_IN_FA = 0M;

            switch (rercmfatipintervalo)
            {
                case ConstantesPrimasRER.tipoIntervalo0:
                    //Como FA_nt_t es 0 para el periodo completo, entonces la suma de la multiplicación de IN por FA siempre será 0
                    suma__multiplicar_IN_FA = 0M;
                    break;

                case ConstantesPrimasRER.tipoIntervalo1:
                case ConstantesPrimasRER.tipoIntervalo2:

                    int intervalosPorDia = ConstantesPrimasRER.numero96;
                    DateTime fechaInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), anio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFinal = fechaInicio.AddMonths(1).AddDays(-1);
                    DateTime fecha = fechaInicio;

                    while (fecha <= fechaFinal)
                    {
                        int i = 1;
                        int minutos = 15;

                        RerInsumoDiaDTO inDia = ObtenerInsumoDiaPorFecha(listInyeccionNetaDia, fecha);
                        if (inDia != null)
                        {
                            while (i <= intervalosPorDia)
                            {
                                DateTime dtFechaHora = fecha.AddMinutes(minutos);
                                var value = inDia.GetType().GetProperty("Rerinddiah" + i).GetValue(inDia, null);
                                decimal IN_nt_t = (value != null) ? (decimal)value : 0M;
                                decimal FA_nt_t = ObtenerFA_nt_t(rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo, dtFechaHora);
                                suma__multiplicar_IN_FA += (IN_nt_t * FA_nt_t);

                                i++;
                                minutos += ConstantesPrimasRER.numero15;
                            }
                        }

                        fecha = fecha.AddDays(1);
                    }
                    break;
            }

            return suma__multiplicar_IN_FA;
        }

        /// <summary>
        /// Devuelve el valor de Factor de Ajuste para un intervalo de una fecha hora correspondiente a un periodo 
        /// </summary>
        /// <param name="tipoIntervalo"></param>
        /// <param name="fechahoraIntervalo"></param>
        /// <param name="valorIntervalo"></param>
        /// <param name="fechahora"></param>
        /// <returns></returns>
        private decimal ObtenerFA_nt_t(string tipoIntervalo, DateTime? fechahoraIntervalo, decimal? valorIntervalo, DateTime fechahora)
        {
            decimal FA_nt_t = 0M;

            switch (tipoIntervalo)
            {
                case ConstantesPrimasRER.tipoIntervalo0:
                    FA_nt_t = 0M;
                    break;

                case ConstantesPrimasRER.tipoIntervalo1:
                    FA_nt_t = 1M;
                    break;

                case ConstantesPrimasRER.tipoIntervalo2:
                    if (fechahoraIntervalo == null || valorIntervalo == null)
                    {
                        throw new Exception("Revisar los valores del Factor de Ajuste para la fecha de intervalo de quiebre, debido a que no existe");
                    }

                    int result = DateTime.Compare(fechahora, fechahoraIntervalo.Value);
                    decimal v = (result > 0) ? 0M : valorIntervalo.Value;
                    FA_nt_t = (result < 0) ? 1M : v;
                    break;
            }

            return FA_nt_t;
        }

        /// <summary>
        /// Obtiene el ingreso por potencia para un periodo específico
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="ip"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <param name="suma_fa_div_n"></param>
        /// <returns></returns>
        private decimal ObtenerIngresoPorPotencia(int anio, int mes, decimal ip, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo, out decimal suma_fa_div_n)
        {
            suma_fa_div_n = 0M;

            if (rercmfatipintervalo == ConstantesPrimasRER.tipoIntervalo0)
            {
                //Como FA_nt_t es 0 para el periodo completo, entonces suma_fa siempre será 0. 
                //Por lo tanto: suma_fa_div_n = (suma_fa / Nt) = 0 
                suma_fa_div_n = 0M;
            }
            else if (rercmfatipintervalo == ConstantesPrimasRER.tipoIntervalo1)
            {
                //Como FA_nt_t es 1 para el periodo completo, entonces suma_fa siempre será igual a Nt. 
                //Por lo tanto: suma_fa_div_n = (suma_fa / Nt) = 1 
                suma_fa_div_n = 1M;
            }
            else if (rercmfatipintervalo == ConstantesPrimasRER.tipoIntervalo2)
            {
                int Nt = 0;
                decimal suma_fa = 0M;
                int minutosPorDia = ConstantesPrimasRER.numero96;
                DateTime fechaInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), anio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = fechaInicio.AddMonths(1).AddDays(-1);

                DateTime fecha = fechaInicio;
                while (fecha <= fechaFinal)
                {
                    int i = 1;
                    int minutos = 15;
                    while (i <= minutosPorDia)
                    {
                        DateTime dtFechaHora = fecha.AddMinutes(minutos);
                        decimal FA_nt_t = ObtenerFA_nt_t(rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo, dtFechaHora);
                        suma_fa += FA_nt_t;

                        i++;
                        minutos += ConstantesPrimasRER.numero15;
                        Nt++;
                    }
                    fecha = fecha.AddDays(1);
                }

                suma_fa_div_n = (suma_fa / Nt);
            }

            decimal ingresoPorPotencia = ip * suma_fa_div_n;
            return ingresoPorPotencia;
        }

        /// <summary>
        /// Obtiene el Ingreso Por Energía para un periodo específico
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <param name="listInyeccionNetaMensualDia"></param>
        /// <param name="listCostoMarginalDia"></param>
        /// <returns></returns>
        private decimal ObtenerIngresoPorEnergia(int anio, int mes, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo, List<RerInsumoDiaDTO> listInyeccionNetaMensualDia, List<RerInsumoDiaDTO> listCostoMarginalDia)
        {
            decimal suma__multiplicacion_nt_t = 0M;

            switch (rercmfatipintervalo)
            {
                case ConstantesPrimasRER.tipoIntervalo0:
                    //Como FA_nt_t es 0 para el periodo completo, entonces toda la multiplicación será 0.
                    //Por lo tanto, la sumatoria siempre será 0. suma__multiplicacion_nt_t += (FA_nt_t * IN_nt_t * CM_nt_t)
                    suma__multiplicacion_nt_t = 0M;
                    break;

                case ConstantesPrimasRER.tipoIntervalo1:
                case ConstantesPrimasRER.tipoIntervalo2:

                    int minutosPorDia = ConstantesPrimasRER.numero96;
                    DateTime fechaInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), anio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFinal = fechaInicio.AddMonths(1).AddDays(-1);

                    DateTime fecha = fechaInicio;
                    while (fecha <= fechaFinal)
                    {
                        int i = 1;
                        int minutos = 15;

                        RerInsumoDiaDTO inDia = ObtenerInsumoDiaPorFecha(listInyeccionNetaMensualDia, fecha);
                        RerInsumoDiaDTO cmDia = ObtenerInsumoDiaPorFecha(listCostoMarginalDia, fecha);
                        if (inDia != null && cmDia != null)
                        {
                            while (i <= minutosPorDia)
                            {
                                DateTime dtFechaHora = fecha.AddMinutes(minutos);
                                decimal FA_nt_t = ObtenerFA_nt_t(rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo, dtFechaHora);
                                var valueIN_nt_t = inDia.GetType().GetProperty("Rerinddiah" + i).GetValue(inDia, null);
                                decimal IN_nt_t = (valueIN_nt_t != null) ? (decimal)valueIN_nt_t : 0M; //EN-ΣR
                                var valueCM_nt_t = cmDia.GetType().GetProperty("Rerinddiah" + i).GetValue(cmDia, null); 
                                decimal CM_nt_t = (valueCM_nt_t != null) ? (decimal)valueCM_nt_t : 0M; //CM
                                suma__multiplicacion_nt_t += (FA_nt_t * IN_nt_t * CM_nt_t); //FA*(EN-ΣR)*CM

                                i++;
                                minutos += ConstantesPrimasRER.numero15;
                            }
                        }

                        fecha = fecha.AddDays(1);
                    }
                    break;
            }

            return suma__multiplicacion_nt_t;
        }

        /// <summary>
        /// Obtener Saldo VTEA para un periodo específico
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <param name="listSaldoVTEADia"></param>
        /// <returns></returns>
        private decimal ObtenerSaldoVTEA(int anio, int mes, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo, List<RerInsumoDiaDTO> listSaldoVTEADia)
        {
            decimal multiplicar_FA_nt_t_SaldoVTEA_nt_t = 0M;

            switch (rercmfatipintervalo)
            {
                case ConstantesPrimasRER.tipoIntervalo0:
                    //Como FA_nt_t es 0 para el periodo completo, entonces toda la multiplicación siempre será 0.
                    //Por lo tanto, multiplicar_FA_nt_t_SaldoVTEA_nt_t += (FA_nt_t * SaldoVTEA_nt_t) = 0
                    multiplicar_FA_nt_t_SaldoVTEA_nt_t = 0M;
                    break;

                case ConstantesPrimasRER.tipoIntervalo1:
                case ConstantesPrimasRER.tipoIntervalo2:

                    int intervalosPorDia = ConstantesPrimasRER.numero96;
                    DateTime fechaInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), anio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFinal = fechaInicio.AddMonths(1).AddDays(-1);

                    DateTime fecha = fechaInicio;
                    while (fecha <= fechaFinal)
                    {
                        int i = 1;
                        int minutos = 15;

                        RerInsumoDiaDTO saldoVTEADia = ObtenerInsumoDiaPorFecha(listSaldoVTEADia, fecha);
                        if (saldoVTEADia != null)
                        {   
                            while (i <= intervalosPorDia)
                            {
                                DateTime dtFechaHora = fecha.AddMinutes(minutos);
                                decimal FA_nt_t = ObtenerFA_nt_t(rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo, dtFechaHora);
                                var value = saldoVTEADia.GetType().GetProperty("Rerinddiah" + i).GetValue(saldoVTEADia, null);
                                decimal SaldoVTEA_nt_t = (value != null) ? (decimal)value : 0M;
                                multiplicar_FA_nt_t_SaldoVTEA_nt_t += (FA_nt_t * SaldoVTEA_nt_t);

                                i++;
                                minutos += ConstantesPrimasRER.numero15;
                            }
                        }

                        fecha = fecha.AddDays(1);
                    }
                    break;
            }

            return multiplicar_FA_nt_t_SaldoVTEA_nt_t;
        }

        /// <summary>
        /// Obtener Saldo VTP para un periodo específico
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <param name="saldoVTPMes">Contiene: IP_t_rp - IP_t_rp_1</param>
        /// <returns></returns>
        private decimal ObtenerSaldoVTP(int anio, int mes, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo, RerInsumoMesDTO saldoVTPMes)
        {
            decimal suma_fa__division_Nt = ObtenerSumaFaDivisionNt(anio, mes, rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo);
            decimal resta_IP_t_rp_IP_t_rp_1 = (saldoVTPMes != null ? saldoVTPMes.Rerinmmestotal : 0M);
            decimal saldoVTP = (suma_fa__division_Nt * resta_IP_t_rp_IP_t_rp_1);
            return saldoVTP;
        }

        /// <summary>
        /// Obtener Saldo VTEA de una Revisión para un periodo, día y hora específicos
        /// </summary>
        /// <param name="indexHora"></param>
        /// <param name="dtFechaHora"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <param name="insumoVtea">Contiene: multiplicacion_EN_nt_r_CMG_nt_r__resta__suma_multiplicacion_R_nt_r_CMG_nt_r</param>
        /// <returns></returns>
        private decimal ObtenerSaldoVTEARevision(int indexHora, DateTime dtFechaHora, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo, RerInsumoVteaDTO insumoVtea)
        {
            decimal FA_nt_t = ObtenerFA_nt_t(rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo, dtFechaHora);
            var valor_nt_r = insumoVtea.GetType().GetProperty("Rerinediah" + indexHora).GetValue(insumoVtea, null);
            decimal multip_EN_nt_r_CMG_nt_r__resta__suma_multip_R_nt_r_CMG_nt_r = (valor_nt_r != null) ? (decimal)valor_nt_r : 0M;
            decimal saldoVTEA_nt_r = FA_nt_t * multip_EN_nt_r_CMG_nt_r__resta__suma_multip_R_nt_r_CMG_nt_r;
            return saldoVTEA_nt_r;
        }

        /// <summary>
        /// Obtener Saldo VTP de una Revisión para un periodo específico
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <param name="insumoVtp">Contiene: IP_t_rp</param>
        /// <returns></returns>
        private decimal ObtenerSaldoVTPRevision(int anio, int mes, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo, RerInsumoVtpDTO insumoVtp)
        {
            decimal suma_fa__division_Nt = ObtenerSumaFaDivisionNt(anio, mes, rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo);
            decimal ip_t_rp = (insumoVtp != null ? insumoVtp.Rerinpmestotal : 0M);
            decimal saldoVTP = (suma_fa__division_Nt * ip_t_rp);
            return saldoVTP;
        }

        /// <summary>
        /// Obtiene la sumatoria del Factor de Ajuste de cada intervalo de 15 de un periodo, dividido entre el número total de intervalo de 15 de dicho periodo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="rercmfatipintervalo"></param>
        /// <param name="rercmfafecintervalo"></param>
        /// <param name="rercmfavalintervalo"></param>
        /// <returns></returns>
        private decimal ObtenerSumaFaDivisionNt(int anio, int mes, string rercmfatipintervalo, DateTime? rercmfafecintervalo, decimal? rercmfavalintervalo)
        {
            decimal suma_fa__division_Nt = 0M;

            switch (rercmfatipintervalo)
            {
                case ConstantesPrimasRER.tipoIntervalo0:
                    //Como FA_nt_t es 0 para el periodo completo, entonces suma_fa siempre será 0
                    //Por lo tanto, suma_fa__division_Nt = (suma_fa / Nt) = 0
                    suma_fa__division_Nt = 0M;
                    break;

                case ConstantesPrimasRER.tipoIntervalo1:
                case ConstantesPrimasRER.tipoIntervalo2:

                    int Nt = 0;
                    decimal suma_fa = 0M;

                    int intervalosPorDia = ConstantesPrimasRER.numero96;
                    DateTime fechaInicio = DateTime.ParseExact(string.Format("01/{0}/{1}", mes.ToString("D2"), anio), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaFinal = fechaInicio.AddMonths(1).AddDays(-1);

                    DateTime fecha = fechaInicio;
                    while (fecha <= fechaFinal)
                    {
                        int i = 1;
                        int minutos = 15;

                        while (i <= intervalosPorDia)
                        {
                            DateTime dtFechaHora = fecha.AddMinutes(minutos);
                            decimal FA_nt_t = ObtenerFA_nt_t(rercmfatipintervalo, rercmfafecintervalo, rercmfavalintervalo, dtFechaHora);
                            suma_fa += FA_nt_t;

                            i++;
                            minutos += ConstantesPrimasRER.numero15;
                            Nt++;
                        }

                        fecha = fecha.AddDays(1);
                    }
                    suma_fa__division_Nt = (suma_fa / Nt);
                    break;
            }

            return suma_fa__division_Nt;
        }

        /// <summary>
        /// Es el resultado de sumar los insumos y dividirlo por el tipo de cambio, 
        /// correspondientes a una central para un periodo específico
        /// </summary>
        /// <param name="ingresoPorPotencia"></param>
        /// <param name="ingresoPorPrimaRER"></param>
        /// <param name="ingresoPorEnergia"></param>
        /// <param name="saldoVTEA"></param>
        /// <param name="saldoVTP"></param>
        /// <param name="tipoCambio"></param>
        /// <returns></returns>
        private decimal ObtenerIMCP(decimal ingresoPorPotencia, decimal ingresoPorPrimaRER, decimal ingresoPorEnergia,
                                    decimal saldoVTEA, decimal saldoVTP, decimal tipoCambio)
        {
            decimal imcp = ((ingresoPorPotencia + ingresoPorPrimaRER + ingresoPorEnergia + saldoVTEA + saldoVTP) / tipoCambio);
            return imcp;
        }

        /// <summary>
        /// Obtiene el saldo mensual por compensar con respecto a los valores entregados,
        /// los cuales deben corresponder a una Central RER para un periodo específico
        /// </summary>
        /// <param name="tarifaAdjudicada"></param>
        /// <param name="suma__multiplicacion_in_fa"></param>
        /// <param name="imcp"></param>
        /// <returns></returns>
        private decimal ObtenerSaldoMensualPorCompensar(decimal tarifaAdjudicada, decimal suma__multiplicacion_in_fa, decimal imcp)
        {
            decimal smc = (tarifaAdjudicada * suma__multiplicacion_in_fa) - imcp;
            return smc;
        }

        #endregion

        #region Reportes Prima RER

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto a un tipo de Reporte Prima RER para el año y versión especificados
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="invocadoPor"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="listExcelHoja"></param>
        public void GenerarArchivoExcelReporte(int anio, string version, int tipoReporte, string invocadoPor, out string nombreArchivo, out List<RerExcelHoja> listExcelHoja)
        {
            try
            {
                nombreArchivo = null;
                listExcelHoja = null;
                switch (tipoReporte)
                {
                    case ConstantesPrimasRER.tipoReportePrimaRERIngresoPorPotencia:
                        nombreArchivo = ("ReporteIngresoPotencia_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteIngresoPorPotencia(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERIngresoPorEnergia:
                        nombreArchivo = ("ReporteIngresoEnergia_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteIngresoPorEnergia(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaREREnergiaNeta:
                        nombreArchivo = ("ReporteEnergiaNeta_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteEnergiaNeta(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERCostoMarginal:
                        nombreArchivo = ("ReporteCostoMarginal_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteCostoMarginal(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERFactorAjuste:
                        nombreArchivo = ("ReporteFactorAjuste_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteFA(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERIngresoPorPrimaRER:
                        nombreArchivo = ("ReporteIngresoPrimaRER_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteIngresoPrimaRER(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldosVTEAResumen:
                        nombreArchivo = ("ReporteSaldoVTEA_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSaldosVTEAResumen(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldosVTEA1Trimestre:
                        nombreArchivo = ("ReporteSaldoVTEA_" + anio + "_" + version + "_" + ConstantesPrimasRER.numero1 + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSaldosVTEATrimestral(anio, version, ConstantesPrimasRER.numero1, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldosVTEA2Trimestre:
                        nombreArchivo = ("ReporteSaldoVTEA_" + anio + "_" + version + "_" + ConstantesPrimasRER.numero2 + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSaldosVTEATrimestral(anio, version, ConstantesPrimasRER.numero2, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldosVTEA3Trimestre:
                        nombreArchivo = ("ReporteSaldoVTEA_" + anio + "_" + version + "_" + ConstantesPrimasRER.numero3 + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSaldosVTEATrimestral(anio, version, ConstantesPrimasRER.numero3, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldosVTEA4Trimestre:
                        nombreArchivo = ("ReporteSaldoVTEA_" + anio + "_" + version + "_" + ConstantesPrimasRER.numero4 + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSaldosVTEATrimestral(anio, version, ConstantesPrimasRER.numero4, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldosVTP:
                        nombreArchivo = ("ReporteSaldoVTP_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSaldosVTP(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERTarifaAdjudicada:
                        nombreArchivo = ("ReporteTarifaAdjudicada_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteTA(anio, version, invocadoPor);
                        break;
                    case ConstantesPrimasRER.tipoReportePrimaRERSaldoMensualPorCompensar:
                        nombreArchivo = ("ReporteSaldoMensualCompensar_" + anio + "_" + version + "_" + DateTime.Now.ToString(ConstantesPrimasRER.FormatoFechaHoraFull2));
                        listExcelHoja = GenerarArchivoExcelReporteSMC(anio, version, invocadoPor);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Ingreso por Potencia
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteIngresoPorPotencia(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerParametroPrimaDTO> listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                if (!existeListParametroPrima)
                {
                    throw new Exception(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                int i = 0;
                #endregion

                #region Hoja Resumen

                #region Titulo
                string tituloResumen = "Ingresos por Potencia (S/)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasResumen = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1Resumen = new List<RerExcelModelo> {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloResumen, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2Resumen = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3Resumen = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaResumen = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2Resumen.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3Resumen.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaResumen.Add(25);
                }

                listaCabecerasResumen[0] = listaCabecera1Resumen;
                listaCabecerasResumen[1] = listaCabecera2Resumen;
                listaCabecerasResumen[2] = listaCabecera3Resumen;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalResumen = new List<string> { "center" };
                List<string> listaTipoResumen = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloResumen = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalResumen.Add("right");
                    listaTipoResumen.Add("double");
                    listaEstiloResumen.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosResumen = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosResumen[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string>{ "Rercmingpotencia" });
                    i++;
                }

                RerExcelCuerpo cuerpoResumen = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosResumen,
                    ListaAlineaHorizontal = listaAlineaHorizontalResumen,
                    ListaTipo = listaTipoResumen,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaResumen = new RerExcelHoja
                {
                    NombreHoja = "Resumen",
                    Titulo = tituloResumen,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaResumen,
                    ListaCabeceras = listaCabecerasResumen,
                    Cuerpo = cuerpoResumen
                };
                listExcelHoja.Add(excelHojaResumen);
                #endregion

                #endregion

                #region Hoja Ingreso por Potencia Total

                #region Titulo
                string tituloIPT = "Ingreso por Potencia Total (S/)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasIPT = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1IPT = new List<RerExcelModelo> {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloIPT, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2IPT = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3IPT = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaIPT = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2IPT.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3IPT.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaIPT.Add(25);
                }

                listaCabecerasIPT[0] = listaCabecera1IPT;
                listaCabecerasIPT[1] = listaCabecera2IPT;
                listaCabecerasIPT[2] = listaCabecera3IPT;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalIPT = new List<string> { "center" };
                List<string> listaTipoIPT = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloIPT = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalIPT.Add("right");
                    listaTipoIPT.Add("double");
                    listaEstiloIPT.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosIPT = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosIPT[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string>{ "Rercminsingpotencia" });
                    i++;
                }

                RerExcelCuerpo cuerpoIPT = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosIPT,
                    ListaAlineaHorizontal = listaAlineaHorizontalIPT,
                    ListaTipo = listaTipoIPT,
                    ListaEstilo = listaEstiloIPT
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaIPT = new RerExcelHoja
                {
                    NombreHoja = "Ingreso Potencia",
                    Titulo = tituloIPT,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaIPT,
                    ListaCabeceras = listaCabecerasIPT,
                    Cuerpo = cuerpoIPT
                };
                listExcelHoja.Add(excelHojaIPT);
                #endregion

                #endregion

                #region Factor de Ajuste

                #region Titulo
                string tituloFA = "Factor de Ajuste";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasFA = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1FA = new List<RerExcelModelo> {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloFA, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2FA = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3FA = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaFA = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2FA.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3FA.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaFA.Add(25);
                }

                listaCabecerasFA[0] = listaCabecera1FA;
                listaCabecerasFA[1] = listaCabecera2FA;
                listaCabecerasFA[2] = listaCabecera3FA;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalFA = new List<string> { "center" };
                List<string> listaTipoFA = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloFA = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalFA.Add("right");
                    listaTipoFA.Add("double");
                    listaEstiloFA.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosFA = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosFA[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string>{ "Rercmsumfadivn" });
                    i++;
                }

                RerExcelCuerpo cuerpoFA = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosFA,
                    ListaAlineaHorizontal = listaAlineaHorizontalFA,
                    ListaTipo = listaTipoFA,
                    ListaEstilo = listaEstiloFA
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaFA = new RerExcelHoja
                {
                    NombreHoja = "FAJ",
                    Titulo = tituloFA,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaFA,
                    ListaCabeceras = listaCabecerasFA,
                    Cuerpo = cuerpoFA
                };
                listExcelHoja.Add(excelHojaFA);
                #endregion

                #endregion

                return listExcelHoja;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Ingreso por Energía
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteIngresoPorEnergia(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerParametroPrimaDTO> listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                if (!existeListParametroPrima)
                {
                    throw new Exception(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                int i = 0;
                #endregion

                #region Hoja Resumen

                #region Titulo
                string tituloResumen = "Ingresos por Energía (S/)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasResumen = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1Resumen = new List<RerExcelModelo> {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloResumen, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2Resumen = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3Resumen = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaResumen = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2Resumen.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3Resumen.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaResumen.Add(25);
                }

                listaCabecerasResumen[0] = listaCabecera1Resumen;
                listaCabecerasResumen[1] = listaCabecera2Resumen;
                listaCabecerasResumen[2] = listaCabecera3Resumen;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalResumen = new List<string> { "center" };
                List<string> listaTipoResumen = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloResumen = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalResumen.Add("right");
                    listaTipoResumen.Add("double");
                    listaEstiloResumen.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosResumen = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosResumen[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string> { "Rercmingenergia" });
                    i++;
                }

                RerExcelCuerpo cuerpoResumen = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosResumen,
                    ListaAlineaHorizontal = listaAlineaHorizontalResumen,
                    ListaTipo = listaTipoResumen,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaResumen = new RerExcelHoja
                {
                    NombreHoja = "Resumen",
                    Titulo = tituloResumen,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaResumen,
                    ListaCabeceras = listaCabecerasResumen,
                    Cuerpo = cuerpoResumen
                };
                listExcelHoja.Add(excelHojaResumen);
                #endregion

                #endregion

                #region return
                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Energía Neta
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteEnergiaNeta(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener Datos Generales
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerInsumoDiaDTO> listInyeccionNetaMensualDia = listInyeccionNetaMensualDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByTipoResultadoByPeriodo(ConstantesPrimasRER.tipoResultadoInyeccionNetaMensual, anioVersion.Reravcodi, "-1");
                bool existeListInyeccionNetaMensualDia = (listInyeccionNetaMensualDia != null && listInyeccionNetaMensualDia.Count > 0);
                if (!existeListInyeccionNetaMensualDia)
                {
                    throw new Exception(string.Format("No existen registros de Inyección Neta de tipo mensual en rer_insumo_dia para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                string mesInicioAT = string.Format("05-{0}", anioVersion.Reravaniotarif);
                DateTime dtFechaInicioAT = DateTime.ParseExact(mesInicioAT, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime dtFechaFinAT = dtFechaInicioAT.AddYears(1).AddDays(-1);
                TimeSpan difFechasAT = (dtFechaFinAT - dtFechaInicioAT);
                int cantidadRegistros = (difFechasAT.Days + 1) * ConstantesPrimasRER.numero96;
                #endregion

                #region Obtener Datos: Inyección Neta Mensual (Energía Neta Mensual)
                ConcurrentDictionary<int, List<string>[]> cdListaRegistrosINM = new ConcurrentDictionary<int, List<string>[]>();
                Parallel.ForEach(ConstantesPrimasRER.mesesAnioTarifario, new ParallelOptions { MaxDegreeOfParallelism = 2 }, mes =>
                {
                    int j = 0;
                    var annio = (ConstantesPrimasRER.mesesActualesAnioTarifario.Contains(mes)) ? anioVersion.Reravaniotarif : anioVersion.Reravaniotarif + 1;
                    string mesAnnio = string.Format("{0}-{1}", mes.ToString("D2"), annio);
                    DateTime fechaInicio = DateTime.ParseExact(mesAnnio, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                    DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                    TimeSpan difFechas = (fechaFin - fechaInicio);
                    int cantidadRegistrosByMes = (difFechas.Days + 1) * ConstantesPrimasRER.numero96;
                    List<string>[] listaRegistrosINMByMes = new List<string>[cantidadRegistrosByMes];
                    List<RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Rerpprmes == mes).ToList();

                    for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        List<RerInsumoDiaDTO> listINMD = listInyeccionNetaMensualDia.Where(x => x.Rerinddiafecdia == fecha).ToList();
                        for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                        {
                            listaRegistrosINMByMes[j] = ObtenerRegistroReporteEnergiaNeta(mes, fecha, indexHora, listCentral, listCM, listINMD);
                            j++;
                        }
                    }

                    cdListaRegistrosINM.TryAdd(mes, listaRegistrosINMByMes);
                });
                #endregion

                #region Hoja Inyección Neta Mensual (Energía Neta Mensual)

                #region Titulo
                string tituloINM = "Energía Neta (MWh)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasINM = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1INM = new List<RerExcelModelo> {
                                CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                                CrearExcelModelo(tituloINM, "center", cantidadCentrales, 1)
                            };
                List<RerExcelModelo> listaCabecera2INM = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3INM = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaINM = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2INM.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3INM.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaINM.Add(25);
                }

                listaCabecerasINM[0] = listaCabecera1INM;
                listaCabecerasINM[1] = listaCabecera2INM;
                listaCabecerasINM[2] = listaCabecera3INM;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalINM = new List<string> { "center" };
                List<string> listaTipoINM = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloINM = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalINM.Add("right");
                    listaTipoINM.Add("double");
                    listaEstiloINM.Add(CrearExcelEstilo("#,##0.0000"));
                }

                int i2 = 0;
                List<string>[] listaRegistrosINM = new List<string>[cantidadRegistros];
                foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    if (cdListaRegistrosINM.ContainsKey(mes))
                    {
                        for (int j = 0; j < cdListaRegistrosINM[mes].Length; j++)
                        {
                            listaRegistrosINM[i2] = cdListaRegistrosINM[mes][j];
                            i2++;
                        }
                    }
                }

                RerExcelCuerpo cuerpoINM = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosINM,
                    ListaAlineaHorizontal = listaAlineaHorizontalINM,
                    ListaTipo = listaTipoINM,
                    ListaEstilo = listaEstiloINM
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaINM = new RerExcelHoja
                {
                    NombreHoja = "Energía Neta",
                    Titulo = tituloINM,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaINM,
                    ListaCabeceras = listaCabecerasINM,
                    Cuerpo = cuerpoINM
                };

                listExcelHoja.Add(excelHojaINM);
                #endregion

                #endregion

                #region return
                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Costo Marginal
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteCostoMarginal(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener Datos Generales
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerInsumoDiaDTO> listCostoMarginalDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByTipoResultadoByPeriodo(ConstantesPrimasRER.tipoResultadoCostoMarginal, anioVersion.Reravcodi, "-1");
                bool existeListCostoMarginalDia = (listCostoMarginalDia != null && listCostoMarginalDia.Count > 0);
                if (!existeListCostoMarginalDia)
                {
                    throw new Exception(string.Format("No existen registros de Costos Marginales en rer_insumo_dia para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                string mesInicioAT = string.Format("05-{0}", anioVersion.Reravaniotarif);
                DateTime dtFechaInicioAT = DateTime.ParseExact(mesInicioAT, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime dtFechaFinAT = dtFechaInicioAT.AddYears(1).AddDays(-1);
                TimeSpan difFechasAT = (dtFechaFinAT - dtFechaInicioAT);
                int cantidadRegistros = (difFechasAT.Days + 1) * ConstantesPrimasRER.numero96;
                #endregion

                #region Obtener Datos: Costo Marginal
                ConcurrentDictionary<int, List<string>[]> cdListaRegistrosCM = new ConcurrentDictionary<int, List<string>[]>();
                Parallel.ForEach(ConstantesPrimasRER.mesesAnioTarifario, new ParallelOptions { MaxDegreeOfParallelism = 2 }, mes =>
                {
                    int j = 0;
                    var annio = (ConstantesPrimasRER.mesesActualesAnioTarifario.Contains(mes)) ? anioVersion.Reravaniotarif : anioVersion.Reravaniotarif + 1;
                    string mesAnnio = string.Format("{0}-{1}", mes.ToString("D2"), annio);
                    DateTime fechaInicio = DateTime.ParseExact(mesAnnio, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                    DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                    TimeSpan difFechas = (fechaFin - fechaInicio);
                    int cantidadRegistrosByMes = (difFechas.Days + 1) * ConstantesPrimasRER.numero96;
                    List<string>[] listaRegistrosCMByMes = new List<string>[cantidadRegistrosByMes];

                    for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        List<RerInsumoDiaDTO> listCMDia = listCostoMarginalDia.Where(x => x.Rerinddiafecdia == fecha).ToList();
                        for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                        {
                            listaRegistrosCMByMes[j] = ObtenerRegistroReporteCMgCP(fecha, indexHora, listCentral, listCMDia);
                            j++;
                        }
                    }

                    cdListaRegistrosCM.TryAdd(mes, listaRegistrosCMByMes);
                });
                #endregion

                #region Hoja CMgCP

                #region Titulo
                string tituloCM = "Costo Marginal (S/MWh)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasCM = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1CM = new List<RerExcelModelo> {
                                CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                                CrearExcelModelo(tituloCM, "center", cantidadCentrales, 1),
                            };
                List<RerExcelModelo> listaCabecera2CM = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3CM = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaCM = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2CM.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3CM.Add(CrearExcelModelo(central.Barrbarratransferencia));
                    listaAnchoColumnaCM.Add(25);
                }

                listaCabecerasCM[0] = listaCabecera1CM;
                listaCabecerasCM[1] = listaCabecera2CM;
                listaCabecerasCM[2] = listaCabecera3CM;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalCM = new List<string> { "center" };
                List<string> listaTipoCM = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloCM = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalCM.Add("right");
                    listaTipoCM.Add("double");
                    listaEstiloCM.Add(CrearExcelEstilo("#,##0.0000"));
                }

                int i3 = 0;
                List<string>[] listaRegistrosCM = new List<string>[cantidadRegistros];
                foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    if (cdListaRegistrosCM.ContainsKey(mes))
                    {
                        for (int j = 0; j < cdListaRegistrosCM[mes].Length; j++)
                        {
                            listaRegistrosCM[i3] = cdListaRegistrosCM[mes][j];
                            i3++;
                        }
                    }
                }

                RerExcelCuerpo cuerpoCM = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosCM,
                    ListaAlineaHorizontal = listaAlineaHorizontalCM,
                    ListaTipo = listaTipoCM,
                    ListaEstilo = listaEstiloCM
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaCM = new RerExcelHoja
                {
                    NombreHoja = "CMgCP",
                    Titulo = tituloCM,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaCM,
                    ListaCabeceras = listaCabecerasCM,
                    Cuerpo = cuerpoCM
                };
                listExcelHoja.Add(excelHojaCM);
                #endregion

                #endregion

                #region return
                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Factor de Ajuste
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteFA(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener Datos Generales
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                string mesInicioAT = string.Format("05-{0}", anioVersion.Reravaniotarif);
                DateTime dtFechaInicioAT = DateTime.ParseExact(mesInicioAT, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime dtFechaFinAT = dtFechaInicioAT.AddYears(1).AddDays(-1);
                TimeSpan difFechasAT = (dtFechaFinAT - dtFechaInicioAT);
                int cantidadRegistros = (difFechasAT.Days + 1) * ConstantesPrimasRER.numero96;
                #endregion

                #region Obtener Datos: Factor de Ajuste
                ConcurrentDictionary<int, List<string>[]> cdListaRegistrosFA = new ConcurrentDictionary<int, List<string>[]>();
                Parallel.ForEach(ConstantesPrimasRER.mesesAnioTarifario, new ParallelOptions { MaxDegreeOfParallelism = 2 }, mes =>
                {
                    int j = 0;
                    var annio = (ConstantesPrimasRER.mesesActualesAnioTarifario.Contains(mes)) ? anioVersion.Reravaniotarif : anioVersion.Reravaniotarif + 1;
                    string mesAnnio = string.Format("{0}-{1}", mes.ToString("D2"), annio);
                    DateTime fechaInicio = DateTime.ParseExact(mesAnnio, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                    DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                    TimeSpan difFechas = (fechaFin - fechaInicio);
                    int cantidadRegistrosByMes = (difFechas.Days + 1) * ConstantesPrimasRER.numero96;
                    List<string>[] listaRegistrosFAByMes = new List<string>[cantidadRegistrosByMes];
                    List<RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Rerpprmes == mes).ToList();

                    for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                        {
                            listaRegistrosFAByMes[j] = ObtenerRegistroReporteFA(mes, fecha, indexHora, listCentral, listCM);
                            j++;
                        }
                    }

                    cdListaRegistrosFA.TryAdd(mes, listaRegistrosFAByMes);
                });
                #endregion

                #region Hoja Factor de Ajuste

                #region Titulo
                string tituloFA = "Factor de Ajuste";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasFA = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1FA = new List<RerExcelModelo> {
                                CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                                CrearExcelModelo(tituloFA, "center", cantidadCentrales, 1)
                            };
                List<RerExcelModelo> listaCabecera2FA = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3FA = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaFA = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2FA.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3FA.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaFA.Add(25);
                }

                listaCabecerasFA[0] = listaCabecera1FA;
                listaCabecerasFA[1] = listaCabecera2FA;
                listaCabecerasFA[2] = listaCabecera3FA;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalFA = new List<string> { "center" };
                List<string> listaTipoFA = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloFA = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalFA.Add("right");
                    listaTipoFA.Add("double");
                    listaEstiloFA.Add(CrearExcelEstilo("#,##0.0000"));
                }

                int i4 = 0;
                List<string>[] listaRegistrosFA = new List<string>[cantidadRegistros];
                foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    if (cdListaRegistrosFA.ContainsKey(mes))
                    {
                        for (int j = 0; j < cdListaRegistrosFA[mes].Length; j++)
                        {
                            listaRegistrosFA[i4] = cdListaRegistrosFA[mes][j];
                            i4++;
                        }
                    }
                }

                RerExcelCuerpo cuerpoFA = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosFA,
                    ListaAlineaHorizontal = listaAlineaHorizontalFA,
                    ListaTipo = listaTipoFA,
                    ListaEstilo = listaEstiloFA
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaFA = new RerExcelHoja
                {
                    NombreHoja = "FAJ",
                    Titulo = tituloFA,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaFA,
                    ListaCabeceras = listaCabecerasFA,
                    Cuerpo = cuerpoFA
                };
                listExcelHoja.Add(excelHojaFA);
                #endregion

                #endregion

                #region return
                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Ingreso Prima RER
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteIngresoPrimaRER(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerParametroPrimaDTO> listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                if (!existeListParametroPrima)
                {
                    throw new Exception(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int i = 0;
                int cantidadCentrales = listCentral.Count;
                #endregion

                #region Hoja Ingresos por Prima RER

                #region Titulo
                string titulo = "Ingresos por Prima RER (S/)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1 = new List<RerExcelModelo> { 
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(titulo, "center", cantidadCentrales, 1),
                };
                List<RerExcelModelo> listaCabecera2 = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3 = new List<RerExcelModelo> { };
                List<int> listaAnchoColumna = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumna.Add(25);
                }

                listaCabeceras[0] = listaCabecera1;
                listaCabeceras[1] = listaCabecera2;
                listaCabeceras[2] = listaCabecera3;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { "center" };
                List<string> listaTipo = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloResumen = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontal.Add("right");
                    listaTipo.Add("double");
                    listaEstiloResumen.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistros = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistros[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string>{ "Rercmingprimarer" }); 
                    i++;
                }

                RerExcelCuerpo cuerpo = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Ingreso Prima RER",
                    Titulo = titulo,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);
                #endregion

                #endregion

                return listExcelHoja;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Saldos VTEA Resumen
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteSaldosVTEAResumen(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerParametroPrimaDTO> listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                if (!existeListParametroPrima)
                {
                    throw new Exception(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                int i = 0;
                #endregion

                #region Hoja Resumen

                #region Titulo
                string tituloResumen = "Saldo Total VTEA (S/)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasResumen = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1Resumen = new List<RerExcelModelo> {
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloResumen, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2Resumen = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3Resumen = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaResumen = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2Resumen.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3Resumen.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaResumen.Add(25);
                }

                listaCabecerasResumen[0] = listaCabecera1Resumen;
                listaCabecerasResumen[1] = listaCabecera2Resumen;
                listaCabecerasResumen[2] = listaCabecera3Resumen;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalResumen = new List<string> { "center" };
                List<string> listaTipoResumen = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloResumen = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalResumen.Add("right");
                    listaTipoResumen.Add("double");
                    listaEstiloResumen.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosResumen = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosResumen[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string> { "Rercmsaldovtea" });
                    i++;
                }

                RerExcelCuerpo cuerpoResumen = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosResumen,
                    ListaAlineaHorizontal = listaAlineaHorizontalResumen,
                    ListaTipo = listaTipoResumen,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaResumen = new RerExcelHoja
                {
                    NombreHoja = "Resumen",
                    Titulo = tituloResumen,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaResumen,
                    ListaCabeceras = listaCabecerasResumen,
                    Cuerpo = cuerpoResumen
                };
                listExcelHoja.Add(excelHojaResumen);
                #endregion

                #endregion

                #region Return
                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Saldos VTEA
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="trimestre"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteSaldosVTEATrimestral(int anio, string version, int trimestre, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool existeTrimestre = (ConstantesPrimasRER.numeroTrimestres.Contains(trimestre));
                if (!existeTrimestre)
                {
                    throw new Exception(string.Format("No existe un trimestre con valor = {0}", trimestre));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                string rerpprmes = "";
                int[] mesesTrimestre = new int[3];
                switch (trimestre)
                {
                    case 1:
                        rerpprmes = "5,6,7";
                        mesesTrimestre[0] = 5;
                        mesesTrimestre[1] = 6;
                        mesesTrimestre[2] = 7; 
                        break;
                    case 2:
                        rerpprmes = "8,9,10";
                        mesesTrimestre[0] = 8;
                        mesesTrimestre[1] = 9;
                        mesesTrimestre[2] = 10;
                        break;
                    case 3:
                        rerpprmes = "11,12,1";
                        mesesTrimestre[0] = 11;
                        mesesTrimestre[1] = 12;
                        mesesTrimestre[2] = 1;
                        break;
                    case 4:
                        rerpprmes = "2,3,4";
                        mesesTrimestre[0] = 2;
                        mesesTrimestre[1] = 3;
                        mesesTrimestre[2] = 4;
                        break;
                }

                List<RerParametroPrimaDTO> listParametroPrima = null;
                List<RerInsumoVteaDTO> listInsumoVtea = null;
                ConcurrentBag<string> cbErrores = new ConcurrentBag<string>();
                List<Task> tasks = new List<Task>{
                    Task.Run(() =>
                    {
                        listInsumoVtea = FactoryTransferencia.GetRerInsumoVteaRepository().GetByPeriodo(anioVersion.Reravcodi, rerpprmes);
                        bool existeListInsumoVtea = (listInsumoVtea != null && listInsumoVtea.Count > 0);
                        if (!existeListInsumoVtea)
                        {
                            cbErrores.Add("No existen registros a exportar. No hay registros de insumos de Saldo VTEA (2)");
                        }
                    }),
                    Task.Run(() =>
                    {
                        listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, rerpprmes);
                        bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                        if (!existeListParametroPrima)
                        {
                            cbErrores.Add(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                        }
                    })
                };
                Task.WaitAll(tasks.ToArray());
                if (cbErrores.Count > 0)
                {
                    throw new Exception(cbErrores.First<string>());
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                int i = 0;
                #endregion

                #region Hojas Meses del Año Tarifario
                ConcurrentDictionary<int, RerExcelHoja> cdExcelHoja = new ConcurrentDictionary<int, RerExcelHoja>();
                Parallel.ForEach(mesesTrimestre, new ParallelOptions { MaxDegreeOfParallelism = 3 }, mes =>
                {
                    #region Hoja Mes del Año Tarifario

                    #region Validacion
                    List<RerParametroPrimaDTO> listPP = listParametroPrima.Where(x => x.Rerpprmes == mes).ToList();
                    if (listPP.Count <= 0)
                    {
                        return;
                    }
                    RerParametroPrimaDTO parametroPrima = listPP[0];

                    List<RerInsumoVteaDTO> listInsVteaMes = listInsumoVtea.Where(x => x.Rerpprcodi == parametroPrima.Rerpprcodi).ToList();
                    if (listInsVteaMes.Count <= 0)
                    {
                        return;
                    }
                    listInsVteaMes = listInsVteaMes.OrderByDescending(x => x.Pericodi).ThenByDescending(x => x.Recacodi).ToList();
                    #endregion

                    #region Obtener datos
                    int z = 0;
                    var listRevision = listInsVteaMes.GroupBy(x => new { x.Pericodi, x.Perinombre, x.Recacodi, x.Recanombre }).Select(x => new RerInsumoVteaDTO { Pericodi = x.Key.Pericodi, Perinombre = x.Key.Perinombre, Recacodi = x.Key.Recacodi, Recanombre = x.Key.Recanombre }).OrderByDescending(x => x.Pericodi).ThenByDescending(x => x.Recacodi).ToList();
                    Dictionary<int, RerInsumoVteaDTO> dRevisiones = new Dictionary<int, RerInsumoVteaDTO>();
                    foreach (var revision in listRevision)
                    {
                        dRevisiones.Add(z, revision);
                        z++;
                    }
                    #endregion

                    #region Titulo
                    string tituloMes = "Saldo VTEA (S/)";
                    #endregion

                    #region Cabecera
                    List<RerExcelModelo>[] listaCabecerasMes = new List<RerExcelModelo>[3];
                    List<RerExcelModelo> listaCabecera1Mes = new List<RerExcelModelo> { CrearExcelModelo("Día", "center", 1, 3), CrearExcelModelo("Hora", "center", 1, 3) };
                    List<RerExcelModelo> listaCabecera2Mes = new List<RerExcelModelo> { };
                    List<RerExcelModelo> listaCabecera3Mes = new List<RerExcelModelo> { };
                    List<int> listaAnchoColumnaMes = new List<int> { 20, 20 };

                    foreach (var revision in dRevisiones)
                    {
                        string nombre = string.Format("{0} {1}", revision.Value.Perinombre, revision.Value.Recanombre);
                        listaCabecera1Mes.Add(CrearExcelModelo(nombre, "center", cantidadCentrales, 1));

                        foreach (RerCentralDTO central in listCentral)
                        {
                            listaCabecera2Mes.Add(CrearExcelModelo(central.Codentcodigo));
                            listaCabecera3Mes.Add(CrearExcelModelo(central.Equinomb));
                            listaAnchoColumnaMes.Add(25);
                        }
                    }

                    listaCabecerasMes[0] = listaCabecera1Mes;
                    listaCabecerasMes[1] = listaCabecera2Mes;
                    listaCabecerasMes[2] = listaCabecera3Mes;
                    #endregion

                    #region Cuerpo
                    List<string> listaAlineaHorizontalMes = new List<string> { "center", "center" };
                    List<string> listaTipoMes = new List<string> { "string", "string" };
                    List<RerExcelEstilo> listaEstiloMes = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"), CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };

                    foreach (var revision in dRevisiones)
                    {
                        foreach (RerCentralDTO central in listCentral)
                        {
                            listaAlineaHorizontalMes.Add("right");
                            listaTipoMes.Add("double");
                            listaEstiloMes.Add(CrearExcelEstilo("#,##0.0000"));
                        }
                    }

                    int j = 0;
                    int cantidadRegistroByMes = ConstantesPrimasRER.numero31 * ConstantesPrimasRER.numero96;
                    List<string>[] listaRegistrosByMes = new List<string>[cantidadRegistroByMes];
                    for (int dia = ConstantesPrimasRER.numero1; dia <= ConstantesPrimasRER.numero31; dia++)
                    {
                        List<RerInsumoVteaDTO> listIVMes = listInsVteaMes.Where(x => x.Rerpprcodi == parametroPrima.Rerpprcodi && x.Rerinefecdia.Day == dia).ToList();
                        for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                        {
                            listaRegistrosByMes[j] = ObtenerRegistroReporteSaldosVTEAPorRevision(dia, indexHora, parametroPrima, listCentral, dRevisiones, listIVMes);
                            j++;
                        }
                    }

                    RerExcelCuerpo cuerpoMes = new RerExcelCuerpo
                    {
                        ListaRegistros = listaRegistrosByMes,
                        ListaAlineaHorizontal = listaAlineaHorizontalMes,
                        ListaTipo = listaTipoMes,
                        ListaEstilo = listaEstiloMes
                    };
                    #endregion

                    #region Definir hoja
                    RerExcelHoja excelHojaMes = new RerExcelHoja
                    {
                        NombreHoja = parametroPrima.Rerpprmesaniodesc,
                        Titulo = tituloMes,
                        Subtitulo1 = subtitulo1,
                        Subtitulo2 = subtitulo2,
                        ListaAnchoColumna = listaAnchoColumnaMes,
                        ListaCabeceras = listaCabecerasMes,
                        Cuerpo = cuerpoMes
                    };
                    cdExcelHoja.TryAdd(mes, excelHojaMes);
                    #endregion

                    #endregion
                });

                foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    if (cdExcelHoja.ContainsKey(mes))
                    {
                        listExcelHoja.Add(cdExcelHoja[mes]);
                    }
                }
                #endregion

                #region Return
                return listExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Saldos VTP
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteSaldosVTP(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerParametroPrimaDTO> listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                if (!existeListParametroPrima)
                {
                    throw new Exception(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerInsumoVtpDTO> listInsumoVtp = FactoryTransferencia.GetRerInsumoVtpRepository().GetByPeriodo(anioVersion.Reravcodi, "-1");
                bool existeListInsumoVtp = (listInsumoVtp != null && listInsumoVtp.Count > 0);
                if (!existeListInsumoVtp)
                {
                    throw new Exception("No existen registros de insumos VTP");
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                int i = 0;
                #endregion

                #region Hoja Resumen

                #region Titulo
                string tituloResumen = "Saldo Total VTP (S/)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasResumen = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1Resumen = new List<RerExcelModelo> { 
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo("Ingreso por Potencia (S/)", "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2Resumen = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3Resumen = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaResumen = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2Resumen.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3Resumen.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaResumen.Add(25);
                }

                listaCabecerasResumen[0] = listaCabecera1Resumen;
                listaCabecerasResumen[1] = listaCabecera2Resumen;
                listaCabecerasResumen[2] = listaCabecera3Resumen;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalResumen = new List<string> { "center" };
                List<string> listaTipoResumen = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloResumen = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalResumen.Add("right");
                    listaTipoResumen.Add("double");
                    listaEstiloResumen.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosResumen = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosResumen[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string> { "Rercmsaldovtp" });
                    i++;
                }

                RerExcelCuerpo cuerpoResumen = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosResumen,
                    ListaAlineaHorizontal = listaAlineaHorizontalResumen,
                    ListaTipo = listaTipoResumen,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaResumen = new RerExcelHoja
                {
                    NombreHoja = "Resumen",
                    Titulo = tituloResumen,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaResumen,
                    ListaCabeceras = listaCabecerasResumen,
                    Cuerpo = cuerpoResumen
                };
                listExcelHoja.Add(excelHojaResumen);
                #endregion

                #endregion

                foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    #region Hoja Mes del Año Tarifario

                    #region Validacion
                    List<RerParametroPrimaDTO> listPP = listParametroPrima.Where(x => x.Rerpprmes == mes).ToList();
                    if (listPP.Count <= 0)
                    {
                        throw new Exception(string.Format("No existe un mes con valor '{0}' en listParametroPrima.", mes));
                    }
                    RerParametroPrimaDTO parametroPrima = listPP[0];

                    List<RerInsumoVtpDTO> listInsVtpMes = listInsumoVtp.Where(x => x.Rerpprcodi == parametroPrima.Rerpprcodi).ToList(); 
                    if (listInsVtpMes.Count <= 0)
                    {
                        continue;
                    }
                    listInsVtpMes = listInsVtpMes.OrderByDescending(x => x.Pericodi).ThenByDescending(x => x.Recpotcodi).ToList();
                    #endregion

                    #region Obtener datos
                    int z = 0;
                    var listRevision = listInsVtpMes.GroupBy(x => new { x.Pericodi, x.Perinombre, x.Recpotcodi, x.Recpotnombre }).Select(x => new RerInsumoVtpDTO { Pericodi = x.Key.Pericodi, Perinombre = x.Key.Perinombre, Recpotcodi = x.Key.Recpotcodi, Recpotnombre = x.Key.Recpotnombre } ).OrderByDescending(x => x.Pericodi).ThenByDescending(x => x.Recpotcodi).ToList();
                    Dictionary<int, RerInsumoVtpDTO> dRevisiones = new Dictionary<int, RerInsumoVtpDTO>();
                    foreach (var revision in listRevision)
                    {
                        dRevisiones.Add(z, revision);
                        z++;
                    }
                    #endregion

                    #region Titulo
                    string tituloMes = "Saldo Total VTP (S/)";
                    #endregion

                    #region Cabecera
                    List<RerExcelModelo>[] listaCabecerasMes = new List<RerExcelModelo>[3];
                    List<RerExcelModelo> listaCabecera1Mes = new List<RerExcelModelo> { CrearExcelModelo("", "center", 1, 3) };
                    List<RerExcelModelo> listaCabecera2Mes = new List<RerExcelModelo> { };
                    List<RerExcelModelo> listaCabecera3Mes = new List<RerExcelModelo> { };
                    List<int> listaAnchoColumnaMes = new List<int> { 35 };

                    foreach (var revision in dRevisiones)
                    {
                        string nombre = string.Format("{0} {1}", revision.Value.Perinombre, revision.Value.Recpotnombre);
                        listaCabecera1Mes.Add(CrearExcelModelo(nombre, "center", cantidadCentrales, 1));

                        foreach (RerCentralDTO central in listCentral)
                        {
                            listaCabecera2Mes.Add(CrearExcelModelo(central.Codentcodigo));
                            listaCabecera3Mes.Add(CrearExcelModelo(central.Equinomb));
                            listaAnchoColumnaMes.Add(25);
                        }
                    }

                    listaCabecerasMes[0] = listaCabecera1Mes;
                    listaCabecerasMes[1] = listaCabecera2Mes;
                    listaCabecerasMes[2] = listaCabecera3Mes;
                    #endregion

                    #region Cuerpo
                    List<string> listaAlineaHorizontalMes = new List<string> { "center" };
                    List<string> listaTipoMes = new List<string> { "string" };
                    List<RerExcelEstilo> listaEstiloMes = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                    foreach (var revision in dRevisiones)
                    {
                        foreach (RerCentralDTO central in listCentral)
                        {
                            listaAlineaHorizontalMes.Add("right");
                            listaTipoMes.Add("double");
                            listaEstiloMes.Add(CrearExcelEstilo("#,##0.0000"));
                        }
                    }

                    List<string>[] listaRegistrosMes = new List<string>[ConstantesPrimasRER.numero1];
                    listaRegistrosMes[0] = ObtenerRegistroReporteSaldosVTPPorRevision(parametroPrima, listCentral, dRevisiones, listInsVtpMes);

                    RerExcelCuerpo cuerpoMes = new RerExcelCuerpo
                    {
                        ListaRegistros = listaRegistrosMes,
                        ListaAlineaHorizontal = listaAlineaHorizontalMes,
                        ListaTipo = listaTipoMes,
                        ListaEstilo = listaEstiloMes
                    };
                    #endregion

                    #region Definir hoja
                    RerExcelHoja excelHojaMes = new RerExcelHoja
                    {
                        NombreHoja = parametroPrima.Rerpprmesaniodesc,
                        Titulo = tituloMes,
                        Subtitulo1 = subtitulo1,
                        Subtitulo2 = subtitulo2,
                        ListaAnchoColumna = listaAnchoColumnaMes,
                        ListaCabeceras = listaCabecerasMes,
                        Cuerpo = cuerpoMes
                    };
                    listExcelHoja.Add(excelHojaMes);
                    #endregion

                    #endregion
                }

                return listExcelHoja;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Tarifa Adjudicada
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteTA(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerCalculoAnualDTO> listCalculoAnual = null;
                List<RerParametroPrimaDTO> listParametroPrima = null;
                List<RerInsumoDiaDTO> listEDIDia = null;
                List<RerInsumoDiaDTO> listINURDia = null;
                ConcurrentBag<string> cbErrores = new ConcurrentBag<string>();
                List<Task> tasks = new List<Task>{
                    Task.Run(() =>
                    {
                        listINURDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByTipoResultadoByPeriodo(ConstantesPrimasRER.tipoResultadoInyeccionNeta, anioVersion.Reravcodi, "-1");
                        bool existeListINURDia = (listINURDia != null && listINURDia.Count > 0);
                        if (!existeListINURDia)
                        {
                            cbErrores.Add(string.Format("No existen registros de Inyección Neta de tipo última revisión en rer_insumo_dia para reravcodi = {0}", anioVersion.Reravcodi));
                        }
                    }),
                    Task.Run(() =>
                    {
                        listEDIDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByTipoResultadoByPeriodo(ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar, anioVersion.Reravcodi, "-1");
                        bool existeListEDIDia = (listEDIDia != null && listEDIDia.Count > 0);
                        if (!existeListEDIDia)
                        {
                            cbErrores.Add(string.Format("No existen registros de Energia Dejada de Inyectar en rer_insumo_dia para reravcodi = {0}", anioVersion.Reravcodi));
                        }
                    }),
                    Task.Run(() =>
                    {
                        listCalculoAnual = FactoryTransferencia.GetRerCalculoAnualRepository().GetByAnnioAndVersion(anioVersion.Reravaniotarif, anioVersion.Reravversion);
                        bool existeListCalculoAnual = (listCalculoAnual != null && listCalculoAnual.Count > 0);
                        if (!existeListCalculoAnual)
                        {
                            cbErrores.Add(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                        }
                    }),
                    Task.Run(() =>
                    {
                        listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                        bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                        if (!existeListParametroPrima)
                        {
                            cbErrores.Add(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                        }
                    })
                };
                Task.WaitAll(tasks.ToArray());
                if (cbErrores.Count > 0)
                {
                    throw new Exception(cbErrores.First<string>());
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int cantidadCentrales = listCentral.Count;
                string mesInicioAT = string.Format("05-{0}", anioVersion.Reravaniotarif);
                DateTime dtFechaInicioAT = DateTime.ParseExact(mesInicioAT, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime dtFechaFinAT = dtFechaInicioAT.AddYears(1).AddDays(-1);
                TimeSpan difFechasAT = (dtFechaFinAT - dtFechaInicioAT);
                int cantidadRegistros = (difFechasAT.Days + 1) * ConstantesPrimasRER.numero96;
                var listNumeros = Enumerable.Range(1, 5).ToList();
                string descAnioTarifarioAnterior = string.Format("Mayo.{0}-Abril.{1}", anioVersion.Reravaniotarif - 1, anioVersion.Reravaniotarif);
                #endregion

                #region Obtener Datos: Inyección Neta Última Revisión y Energía Dejada de Inyectar
                ConcurrentDictionary<int, List<string>[]> cdListaRegistrosINUR = new ConcurrentDictionary<int, List<string>[]>();
                ConcurrentDictionary<int, List<string>[]> cdListaRegistrosEDI = new ConcurrentDictionary<int, List<string>[]>();
                Parallel.ForEach(ConstantesPrimasRER.mesesAnioTarifario, new ParallelOptions { MaxDegreeOfParallelism = 2 }, mes =>
                {
                    int j = 0;
                    var annio = (ConstantesPrimasRER.mesesActualesAnioTarifario.Contains(mes)) ? anioVersion.Reravaniotarif : anioVersion.Reravaniotarif + 1;
                    string mesAnnio = string.Format("{0}-{1}", mes.ToString("D2"), annio);
                    DateTime fechaInicio = DateTime.ParseExact(mesAnnio, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                    DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                    TimeSpan difFechas = (fechaFin - fechaInicio);
                    int cantidadRegistrosByMes = (difFechas.Days + 1) * ConstantesPrimasRER.numero96;
                    List<string>[] listaRegistrosINURByMes = new List<string>[cantidadRegistrosByMes];
                    List<string>[] listaRegistrosEDIByMes = new List<string>[cantidadRegistrosByMes];

                    for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        List<RerInsumoDiaDTO> listINUR_Dia = listINURDia.Where(x => x.Rerinddiafecdia == fecha).ToList();
                        List<RerInsumoDiaDTO> listEDI_Dia = listEDIDia.Where(x => x.Rerinddiafecdia == fecha).ToList();
                        for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                        {
                            ObtenerRegistroReporteTA_INUR_EDI(fecha, indexHora, listCentral,
                               listINUR_Dia, listEDI_Dia,
                               out listaRegistrosINURByMes[j], out listaRegistrosEDIByMes[j]);
                            j++;
                        }
                    }

                    cdListaRegistrosINUR.TryAdd(mes, listaRegistrosINURByMes);
                    cdListaRegistrosEDI.TryAdd(mes, listaRegistrosEDIByMes);
                });
                #endregion

                #region Hojas Archivo Excel
                ConcurrentDictionary<int, RerExcelHoja> cdListaExcelHoja = new ConcurrentDictionary<int, RerExcelHoja>();
                Parallel.ForEach(listNumeros, numero =>
                {
                    switch (numero)
                    {
                        case 1:
                            #region Tarifa Adjudicada

                            #region Titulo
                            string tituloTAdj = "Tarifa Adjudicada (US$/MW.h)";
                            #endregion

                            #region Cabecera
                            List<RerExcelModelo>[] listaCabecerasTAdj = new List<RerExcelModelo>[3];
                            List<RerExcelModelo> listaCabecera1TAdj = new List<RerExcelModelo> {
                                CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                                CrearExcelModelo("Tipo de Cambio", "center", 1, 3),
                                CrearExcelModelo(tituloTAdj, "center", cantidadCentrales, 1)
                            };
                            List<RerExcelModelo> listaCabecera2TAdj = new List<RerExcelModelo> { };
                            List<RerExcelModelo> listaCabecera3TAdj = new List<RerExcelModelo> { };
                            List<int> listaAnchoColumnaTAdj = new List<int> { 25, 20 };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaCabecera2TAdj.Add(CrearExcelModelo(central.Codentcodigo));
                                listaCabecera3TAdj.Add(CrearExcelModelo(central.Equinomb));
                                listaAnchoColumnaTAdj.Add(25);
                            }

                            listaCabecerasTAdj[0] = listaCabecera1TAdj;
                            listaCabecerasTAdj[1] = listaCabecera2TAdj;
                            listaCabecerasTAdj[2] = listaCabecera3TAdj;
                            #endregion

                            #region Cuerpo
                            List<string> listaAlineaHorizontalTAdj = new List<string> { "center", "center" };
                            List<string> listaTipoTAdj = new List<string> { "string", "double" };
                            List<RerExcelEstilo> listaEstiloTAdj = new List<RerExcelEstilo> {
                                CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"),
                                CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9")
                            };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaAlineaHorizontalTAdj.Add("right");
                                listaTipoTAdj.Add("double");
                                listaEstiloTAdj.Add(CrearExcelEstilo("#,##0.00"));
                            }

                            int i1 = 0;
                            List<string>[] listaRegistrosTAdj = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                            foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                            {
                                listaRegistrosTAdj[i1] = ObtenerRegistroReporteTATAdj(mes, listParametroPrima, listCentral, listCalculoMensual);
                                i1++;
                            }

                            RerExcelCuerpo cuerpoTAdj = new RerExcelCuerpo
                            {
                                ListaRegistros = listaRegistrosTAdj,
                                ListaAlineaHorizontal = listaAlineaHorizontalTAdj,
                                ListaTipo = listaTipoTAdj,
                                ListaEstilo = listaEstiloTAdj
                            };
                            #endregion

                            #region Definir hoja
                            RerExcelHoja excelHojaTAdj = new RerExcelHoja
                            {
                                NombreHoja = "Tarifa Adjudicada",
                                Titulo = tituloTAdj,
                                Subtitulo1 = subtitulo1,
                                Subtitulo2 = subtitulo2,
                                ListaAnchoColumna = listaAnchoColumnaTAdj,
                                ListaCabeceras = listaCabecerasTAdj,
                                Cuerpo = cuerpoTAdj
                            };
                            cdListaExcelHoja.TryAdd(numero, excelHojaTAdj);
                            #endregion

                            #endregion
                            break;

                        case 2:
                            #region Factor de Actualización

                            #region Titulo
                            string tituloFA = "Factor de Actualización";
                            #endregion

                            #region Cabecera
                            List<RerExcelModelo>[] listaCabecerasFA = new List<RerExcelModelo>[3];
                            List<RerExcelModelo> listaCabecera1FA = new List<RerExcelModelo> {
                                CrearExcelModelo("", "center", 1, 3),
                                CrearExcelModelo("Factor de Ajuste", "center", cantidadCentrales, 1)
                            };
                            List<RerExcelModelo> listaCabecera2FA = new List<RerExcelModelo> { };
                            List<RerExcelModelo> listaCabecera3FA = new List<RerExcelModelo> { };
                            List<int> listaAnchoColumnaFA = new List<int> { 25, 20 };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaCabecera2FA.Add(CrearExcelModelo(central.Codentcodigo));
                                listaCabecera3FA.Add(CrearExcelModelo(central.Equinomb));
                                listaAnchoColumnaFA.Add(25);
                            }

                            listaCabecerasFA[0] = listaCabecera1FA;
                            listaCabecerasFA[1] = listaCabecera2FA;
                            listaCabecerasFA[2] = listaCabecera3FA;
                            #endregion

                            #region Cuerpo
                            List<string> listaAlineaHorizontalFA = new List<string> { "center" };
                            List<string> listaTipoFA = new List<string> { "string" };
                            List<RerExcelEstilo> listaEstiloFA = new List<RerExcelEstilo> {
                                CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9")
                            };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaAlineaHorizontalFA.Add("right");
                                listaTipoFA.Add("double");
                                listaEstiloFA.Add(CrearExcelEstilo("#,##0.0000"));
                            }

                            List<string>[] listaRegistrosFA = new List<string>[ConstantesPrimasRER.numero5];
                            listaRegistrosFA[0] = ObtenerRegistroReporte("Precio Base ($/MWh)", listCentral, listCalculoAnual, new List<string> { "Rercataradjbase" });
                            listaRegistrosFA[1] = ObtenerRegistroReporte("IPPo", listCentral, listCalculoAnual, new List<string> { "Rercaippo" });
                            listaRegistrosFA[2] = ObtenerRegistroReporte("IPPi", listCentral, listCalculoAnual, new List<string> { "Rercaippi" });
                            listaRegistrosFA[3] = ObtenerRegistroReporte(descAnioTarifarioAnterior, listCentral, listCalculoAnual, new List<string> { "Rercafacactanterior" });
                            listaRegistrosFA[4] = ObtenerRegistroReporte(anioVersion.Reravaniotarifdesc, listCentral, listCalculoAnual, new List<string> { "Rercafacactualizacion" });

                            RerExcelCuerpo cuerpoFA = new RerExcelCuerpo
                            {
                                ListaRegistros = listaRegistrosFA,
                                ListaAlineaHorizontal = listaAlineaHorizontalFA,
                                ListaTipo = listaTipoFA,
                                ListaEstilo = listaEstiloFA
                            };
                            #endregion

                            #region Definir hoja
                            RerExcelHoja excelHojaFA = new RerExcelHoja
                            {
                                NombreHoja = "Factor de Actualización",
                                Titulo = tituloFA,
                                Subtitulo1 = subtitulo1,
                                Subtitulo2 = subtitulo2,
                                ListaAnchoColumna = listaAnchoColumnaFA,
                                ListaCabeceras = listaCabecerasFA,
                                Cuerpo = cuerpoFA
                            };
                            cdListaExcelHoja.TryAdd(numero, excelHojaFA);
                            #endregion

                            #endregion
                            break;

                        case 3:
                            #region Factor de Corrección

                            #region Titulo
                            string tituloFC = "Factor de Corrección";
                            #endregion

                            #region Cabecera
                            List<RerExcelModelo>[] listaCabecerasFC = new List<RerExcelModelo>[3];
                            List<RerExcelModelo> listaCabecera1FC = new List<RerExcelModelo> {
                                CrearExcelModelo("", "center", 1, 3),
                                CrearExcelModelo("Factor de Ajuste", "center", cantidadCentrales, 1)
                            };
                            List<RerExcelModelo> listaCabecera2FC = new List<RerExcelModelo> { };
                            List<RerExcelModelo> listaCabecera3FC = new List<RerExcelModelo> { };
                            List<int> listaAnchoColumnaFC = new List<int> { 25 };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaCabecera2FC.Add(CrearExcelModelo(central.Codentcodigo));
                                listaCabecera3FC.Add(CrearExcelModelo(central.Equinomb));
                                listaAnchoColumnaFC.Add(25);
                            }

                            listaCabecerasFC[0] = listaCabecera1FC;
                            listaCabecerasFC[1] = listaCabecera2FC;
                            listaCabecerasFC[2] = listaCabecera3FC;
                            #endregion

                            #region Cuerpo
                            List<string> listaAlineaHorizontalFC = new List<string> { "center" };
                            List<string> listaTipoFC = new List<string> { "string" };
                            List<RerExcelEstilo> listaEstiloFC = new List<RerExcelEstilo> {
                                CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9")
                            };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaAlineaHorizontalFC.Add("right");
                                listaTipoFC.Add("double");
                                listaEstiloFC.Add(CrearExcelEstilo("#,##0.0000"));
                            }

                            List<string>[] listaRegistrosFC = new List<string>[ConstantesPrimasRER.numero1];
                            listaRegistrosFC[0] = ObtenerRegistroReporte(anioVersion.Reravaniotarifdesc, listCentral, listCalculoAnual, new List<string> { "Rercafaccorreccion" });

                            RerExcelCuerpo cuerpoFC = new RerExcelCuerpo
                            {
                                ListaRegistros = listaRegistrosFC,
                                ListaAlineaHorizontal = listaAlineaHorizontalFC,
                                ListaTipo = listaTipoFC,
                                ListaEstilo = listaEstiloFC
                            };
                            #endregion

                            #region Definir hoja
                            RerExcelHoja excelHojaFC = new RerExcelHoja
                            {
                                NombreHoja = "Factor de Corrección",
                                Titulo = tituloFC,
                                Subtitulo1 = subtitulo1,
                                Subtitulo2 = subtitulo2,
                                ListaAnchoColumna = listaAnchoColumnaFC,
                                ListaCabeceras = listaCabecerasFC,
                                Cuerpo = cuerpoFC
                            };
                            cdListaExcelHoja.TryAdd(numero, excelHojaFC);
                            #endregion

                            #endregion
                            break;

                        case 4:
                            #region EDI

                            #region Titulo
                            string tituloEDI = "Energia Dejada de Inyectar (MWh)";
                            #endregion

                            #region Cabecera
                            List<RerExcelModelo>[] listaCabecerasEDI = new List<RerExcelModelo>[3];
                            List<RerExcelModelo> listaCabecera1EDI = new List<RerExcelModelo> {
                                CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                                CrearExcelModelo("Energía Dejada de Inyectar (MWh)", "center", cantidadCentrales, 1)
                            };
                            List<RerExcelModelo> listaCabecera2EDI = new List<RerExcelModelo> { };
                            List<RerExcelModelo> listaCabecera3EDI = new List<RerExcelModelo> { };
                            List<int> listaAnchoColumnaEDI = new List<int> { 25 };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaCabecera2EDI.Add(CrearExcelModelo(central.Codentcodigo));
                                listaCabecera3EDI.Add(CrearExcelModelo(central.Equinomb));
                                listaAnchoColumnaEDI.Add(25);
                            }

                            listaCabecerasEDI[0] = listaCabecera1EDI;
                            listaCabecerasEDI[1] = listaCabecera2EDI;
                            listaCabecerasEDI[2] = listaCabecera3EDI;
                            #endregion

                            #region Cuerpo
                            List<string> listaAlineaHorizontalEDI = new List<string> { "center" };
                            List<string> listaTipoEDI = new List<string> { "string" };
                            List<RerExcelEstilo> listaEstiloEDI = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaAlineaHorizontalEDI.Add("right");
                                listaTipoEDI.Add("double");
                                listaEstiloEDI.Add(CrearExcelEstilo("#,##0.0000"));
                            }

                            int i4 = 0;
                            List<string>[] listaRegistrosEDI = new List<string>[cantidadRegistros];
                            foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                            {
                                if (cdListaRegistrosEDI.ContainsKey(mes))
                                {
                                    for (int j = 0; j < cdListaRegistrosEDI[mes].Length; j++)
                                    {
                                        listaRegistrosEDI[i4] = cdListaRegistrosEDI[mes][j];
                                        i4++;
                                    }
                                }
                            }

                            RerExcelCuerpo cuerpoEDI = new RerExcelCuerpo
                            {
                                ListaRegistros = listaRegistrosEDI,
                                ListaAlineaHorizontal = listaAlineaHorizontalEDI,
                                ListaTipo = listaTipoEDI,
                                ListaEstilo = listaEstiloEDI
                            };
                            #endregion

                            #region Definir hoja
                            RerExcelHoja excelHojaEDI = new RerExcelHoja
                            {
                                NombreHoja = "EDI",
                                Titulo = tituloEDI,
                                Subtitulo1 = subtitulo1,
                                Subtitulo2 = subtitulo2,
                                ListaAnchoColumna = listaAnchoColumnaEDI,
                                ListaCabeceras = listaCabecerasEDI,
                                Cuerpo = cuerpoEDI
                            };
                            cdListaExcelHoja.TryAdd(numero, excelHojaEDI);
                            #endregion

                            #endregion
                            break;

                        case 5:
                            #region Inyección Neta - Última Revisión

                            #region Titulo
                            string tituloINUR = "Energia Neta de la Última Revisión (MWh)";
                            #endregion

                            #region Cabecera
                            List<RerExcelModelo>[] listaCabecerasINUR = new List<RerExcelModelo>[3];
                            List<RerExcelModelo> listaCabecera1INUR = new List<RerExcelModelo> {
                                CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                                CrearExcelModelo("Energía Neta de la Última Revisión (MWh)", "center", cantidadCentrales, 1)
                            };
                            List<RerExcelModelo> listaCabecera2INUR = new List<RerExcelModelo> { };
                            List<RerExcelModelo> listaCabecera3INUR = new List<RerExcelModelo> { };
                            List<int> listaAnchoColumnaINUR = new List<int> { 25 };

                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaCabecera2INUR.Add(CrearExcelModelo(central.Codentcodigo));
                                listaCabecera3INUR.Add(CrearExcelModelo(central.Equinomb));
                                listaAnchoColumnaINUR.Add(25);
                            }

                            listaCabecerasINUR[0] = listaCabecera1INUR;
                            listaCabecerasINUR[1] = listaCabecera2INUR;
                            listaCabecerasINUR[2] = listaCabecera3INUR;
                            #endregion

                            #region Cuerpo
                            List<string> listaAlineaHorizontalINUR = new List<string> { "center" };
                            List<string> listaTipoINUR = new List<string> { "string" };
                            List<RerExcelEstilo> listaEstiloINUR = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                            foreach (RerCentralDTO central in listCentral)
                            {
                                listaAlineaHorizontalINUR.Add("right");
                                listaTipoINUR.Add("double");
                                listaEstiloINUR.Add(CrearExcelEstilo("#,##0.0000"));
                            }

                            int i5 = 0;
                            List<string>[] listaRegistrosINUR = new List<string>[cantidadRegistros];
                            foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                            {
                                if (cdListaRegistrosINUR.ContainsKey(mes))
                                {
                                    for (int j = 0; j < cdListaRegistrosINUR[mes].Length; j++)
                                    {
                                        listaRegistrosINUR[i5] = cdListaRegistrosINUR[mes][j];
                                        i5++;
                                    }
                                }
                            }

                            RerExcelCuerpo cuerpoINUR = new RerExcelCuerpo
                            {
                                ListaRegistros = listaRegistrosINUR,
                                ListaAlineaHorizontal = listaAlineaHorizontalINUR,
                                ListaTipo = listaTipoINUR,
                                ListaEstilo = listaEstiloINUR
                            };
                            #endregion

                            #region Definir hoja
                            RerExcelHoja excelHojaINUR = new RerExcelHoja
                            {
                                NombreHoja = "Energía Neta Revisión",
                                Titulo = tituloINUR,
                                Subtitulo1 = subtitulo1,
                                Subtitulo2 = subtitulo2,
                                ListaAnchoColumna = listaAnchoColumnaINUR,
                                ListaCabeceras = listaCabecerasINUR,
                                Cuerpo = cuerpoINUR
                            };
                            cdListaExcelHoja.TryAdd(numero, excelHojaINUR);
                            #endregion

                            #endregion
                            break;
                    }
                });

                foreach (int numero in listNumeros)
                {
                    if (cdListaExcelHoja.ContainsKey(numero))
                    {
                        listExcelHoja.Add(cdListaExcelHoja[numero]);
                    }
                }
                #endregion

                return listExcelHoja;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto al Reporte Saldo Mensual Por Compensar
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="invocadoPor"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelReporteSMC(int anio, string version, string invocadoPor)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro en rer_anioversion con año = {0} y versión = {1}", anio, version));
                }

                bool esInvocadoPorExtranetYEstaAbierto = (invocadoPor == ConstantesPrimasRER.invocadoPorExtranet && anioVersion.Reravestado == ConstantesPrimasRER.estadoAnioVersionAbierto);
                if (esInvocadoPorExtranetYEstaAbierto)
                {
                    throw new Exception("El reporte para la versión y el Año Tarifario seleccionados aún no se encuentra disponible.");
                }
                #endregion

                #region Obtener datos
                List<RerCalculoMensualDTO> listCalculoMensual = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(anioVersion.Reravcodi);
                bool existeListCalculoMensual = (listCalculoMensual != null && listCalculoMensual.Count > 0);
                if (!existeListCalculoMensual)
                {
                    throw new Exception(string.Format("No existen registros a exportar. Por favor, ejecute el proceso del cálculo de la Prima RER para La versión '{0}' del Año Tarifario '{1}'.", anioVersion.Reravversiondesc, anioVersion.Reravaniotarifdesc));
                }
                listCalculoMensual = listCalculoMensual.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ThenBy(x => x.Rerpprmes).ToList();

                List<RerParametroPrimaDTO> listParametroPrima = FactoryTransferencia.GetRerParametroPrimaRepository().GetByAnioVersionByMes(anioVersion.Reravcodi, "-1");
                bool existeListParametroPrima = (listParametroPrima != null && listParametroPrima.Count > 0);
                if (!existeListParametroPrima)
                {
                    throw new Exception(string.Format("No existen registros en la tabla rer_parametro_prima para reravcodi = {0}", anioVersion.Reravcodi));
                }

                List<RerCentralDTO> listCentralGroupBy = listCalculoMensual.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Emprnomb, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Emprnomb = x.Key.Emprnomb, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                #endregion

                #region Variables
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                int i = 0;
                int cantidadCentrales = listCentral.Count;
                #endregion

                #region Hoja Resumen

                #region Titulo
                string tituloResumen = "Saldo Mensual a Compensar ($)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasResumen = new List<RerExcelModelo>[1];
                List<RerExcelModelo> listaCabecera1Resumen = new List<RerExcelModelo>
                {
                    CrearExcelModelo("Empresa"),
                    CrearExcelModelo("Central"),
                    CrearExcelModelo("Ingreso Garantizado"),
                    CrearExcelModelo("Ingreso por MCP"),
                    CrearExcelModelo("SMC")
                };
                List<int> listaAnchoColumnaResumen = new List<int>
                {
                    30,
                    30,
                    30,
                    30,
                    30
                };

                listaCabecerasResumen[0] = listaCabecera1Resumen;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalResumen = new List<string> { "left", "left", "right", "right", "right" };
                List<string> listaTipoResumen = new List<string> { "string", "string", "double", "double", "double" };
                List<RerExcelEstilo> listaEstiloResumen = new List<RerExcelEstilo> {
                    null,
                    null,
                    CrearExcelEstilo("#,##0.0000"),
                    CrearExcelEstilo("#,##0.0000"),
                    CrearExcelEstilo("#,##0.0000")
                };

                i = 0;
                List<string>[] listaRegistrosResumen = new List<string>[listCentral.Count];
                foreach (RerCentralDTO central in listCentral)
                {
                    listaRegistrosResumen[i] = ObtenerRegistroReporteSMCResumen(central, listCalculoMensual);
                    i++;
                }

                RerExcelCuerpo cuerpoResumen = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosResumen,
                    ListaAlineaHorizontal = listaAlineaHorizontalResumen,
                    ListaTipo = listaTipoResumen,
                    ListaEstilo = listaEstiloResumen
                };
                #endregion

                #region Definir hoja Resumen
                RerExcelHoja excelHojaResumen = new RerExcelHoja
                {
                    NombreHoja = "Resumen",
                    Titulo = tituloResumen,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaResumen,
                    ListaCabeceras = listaCabecerasResumen,
                    Cuerpo = cuerpoResumen
                };
                listExcelHoja.Add(excelHojaResumen);
                #endregion

                #endregion

                #region Hoja SMC

                #region Titulo
                string tituloSMC = "Saldo Mensual a Compensar ($)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasSMC = new List<RerExcelModelo>[4];
                List<RerExcelModelo> listaCabecera1SMC = new List<RerExcelModelo> { 
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 4),
                    CrearExcelModelo(tituloSMC, "center", cantidadCentrales * 3, 1)
                };
                List<RerExcelModelo> listaCabecera2SMC = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3SMC = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera4SMC = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaSMC = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2SMC.Add(CrearExcelModelo(central.Codentcodigo, "center", 3, 1));
                    listaCabecera3SMC.Add(CrearExcelModelo(central.Equinomb, "center", 3, 1));
                    listaCabecera4SMC.Add(CrearExcelModelo("Ingreso Garantizado"));
                    listaCabecera4SMC.Add(CrearExcelModelo("IMCP"));
                    listaCabecera4SMC.Add(CrearExcelModelo("SMC"));
                    listaAnchoColumnaSMC.Add(25);
                    listaAnchoColumnaSMC.Add(25);
                    listaAnchoColumnaSMC.Add(25);
                }

                listaCabecerasSMC[0] = listaCabecera1SMC;
                listaCabecerasSMC[1] = listaCabecera2SMC;
                listaCabecerasSMC[2] = listaCabecera3SMC;
                listaCabecerasSMC[3] = listaCabecera4SMC;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalSMC = new List<string> { "center" };
                List<string> listaTipoSMC = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloSMC = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalSMC.Add("right");
                    listaAlineaHorizontalSMC.Add("right");
                    listaAlineaHorizontalSMC.Add("right");
                    listaTipoSMC.Add("double");
                    listaTipoSMC.Add("double");
                    listaTipoSMC.Add("double");
                    listaEstiloSMC.Add(CrearExcelEstilo("#,##0.0000"));
                    listaEstiloSMC.Add(CrearExcelEstilo("#,##0.0000"));
                    listaEstiloSMC.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosSMC = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosSMC[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string> { "Rercminggarantizado", "Rercmimcp", "Rercmsalmencompensar" });
                    i++;
                }

                RerExcelCuerpo cuerpoSMC = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosSMC,
                    ListaAlineaHorizontal = listaAlineaHorizontalSMC,
                    ListaTipo = listaTipoSMC,
                    ListaEstilo = listaEstiloSMC
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaSMC = new RerExcelHoja
                {
                    NombreHoja = "SMC",
                    Titulo = tituloSMC,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaSMC,
                    ListaCabeceras = listaCabecerasSMC,
                    Cuerpo = cuerpoSMC
                };
                listExcelHoja.Add(excelHojaSMC);
                #endregion

                #endregion

                #region Hoja Tarifa Adjudicada

                #region Titulo
                string tituloTAdj = "Tarifa Adjudicada ($/MWh)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasTAdj = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1TAdj = new List<RerExcelModelo> { 
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloTAdj, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2TAdj = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3TAdj = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaTAdj = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2TAdj.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3TAdj.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaTAdj.Add(25);
                }

                listaCabecerasTAdj[0] = listaCabecera1TAdj;
                listaCabecerasTAdj[1] = listaCabecera2TAdj;
                listaCabecerasTAdj[2] = listaCabecera3TAdj;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalTAdj = new List<string> { "center" };
                List<string> listaTipoTAdj = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloTAdj = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalTAdj.Add("right");
                    listaTipoTAdj.Add("double");
                    listaEstiloTAdj.Add(CrearExcelEstilo("#,##0.00"));
                }

                i = 0;
                List<string>[] listaRegistrosTAdj = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosTAdj[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string> { "Rercmtaradj" }); 
                    i++;
                }

                RerExcelCuerpo cuerpoTAdj = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosTAdj,
                    ListaAlineaHorizontal = listaAlineaHorizontalTAdj,
                    ListaTipo = listaTipoTAdj,
                    ListaEstilo = listaEstiloTAdj
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaTAdj = new RerExcelHoja
                {
                    NombreHoja = "Tarifa Adjudicada",
                    Titulo = tituloTAdj,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaTAdj,
                    ListaCabeceras = listaCabecerasTAdj,
                    Cuerpo = cuerpoTAdj
                };
                listExcelHoja.Add(excelHojaTAdj);
                #endregion

                #endregion

                #region Hoja Energía Neta

                #region Titulo
                string tituloEN = "Energía Neta hasta Energía Adjudicada (MWh)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasEN = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1EN = new List<RerExcelModelo> { 
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloEN, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2EN = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3EN = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaEN = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2EN.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3EN.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaEN.Add(25);
                }

                listaCabecerasEN[0] = listaCabecera1EN;
                listaCabecerasEN[1] = listaCabecera2EN;
                listaCabecerasEN[2] = listaCabecera3EN;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalEN = new List<string> { "center" };
                List<string> listaTipoEN = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloEN = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalEN.Add("right");
                    listaTipoEN.Add("double");
                    listaEstiloEN.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosEN = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosEN[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string> { "Rercmsummulinfa" }); 
                    i++;
                }

                RerExcelCuerpo cuerpoEN = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosEN,
                    ListaAlineaHorizontal = listaAlineaHorizontalEN,
                    ListaTipo = listaTipoEN,
                    ListaEstilo = listaEstiloEN
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaEN = new RerExcelHoja
                {
                    NombreHoja = "Energía Neta",
                    Titulo = tituloEN,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaEN,
                    ListaCabeceras = listaCabecerasEN,
                    Cuerpo = cuerpoEN
                };
                listExcelHoja.Add(excelHojaEN);
                #endregion

                #endregion

                #region Hoja IMCP

                #region Titulo
                string tituloIMCP = "Ingresos por MCP ($)";
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabecerasIMCP = new List<RerExcelModelo>[3];
                List<RerExcelModelo> listaCabecera1IMCP = new List<RerExcelModelo> { 
                    CrearExcelModelo(anioVersion.Reravaniotarifdesc, "center", 1, 3),
                    CrearExcelModelo(tituloIMCP, "center", cantidadCentrales, 1)
                };
                List<RerExcelModelo> listaCabecera2IMCP = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera3IMCP = new List<RerExcelModelo> { };
                List<int> listaAnchoColumnaIMCP = new List<int> { 35 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera2IMCP.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera3IMCP.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumnaIMCP.Add(25);
                }

                listaCabecerasIMCP[0] = listaCabecera1IMCP;
                listaCabecerasIMCP[1] = listaCabecera2IMCP;
                listaCabecerasIMCP[2] = listaCabecera3IMCP;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontalIMCP = new List<string> { "center" };
                List<string> listaTipoIMCP = new List<string> { "string" };
                List<RerExcelEstilo> listaEstiloIMCP = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontalIMCP.Add("right");
                    listaTipoIMCP.Add("double");
                    listaEstiloIMCP.Add(CrearExcelEstilo("#,##0.0000"));
                }

                i = 0;
                List<string>[] listaRegistrosIMCP = new List<string>[ConstantesPrimasRER.mesesAnioTarifario.Count()];
                foreach (var mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    listaRegistrosIMCP[i] = ObtenerRegistroReporte(mes, listParametroPrima, listCentral, listCalculoMensual, new List<string>{ "Rercmimcp" }); 
                    i++;
                }

                RerExcelCuerpo cuerpoIMCP = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosIMCP,
                    ListaAlineaHorizontal = listaAlineaHorizontalIMCP,
                    ListaTipo = listaTipoIMCP,
                    ListaEstilo = listaEstiloIMCP
                };
                #endregion

                #region Definir hoja
                RerExcelHoja excelHojaIMCP = new RerExcelHoja
                {
                    NombreHoja = "IMCP",
                    Titulo = tituloIMCP,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumnaIMCP,
                    ListaCabeceras = listaCabecerasIMCP,
                    Cuerpo = cuerpoIMCP
                };
                listExcelHoja.Add(excelHojaIMCP);
                #endregion

                #endregion

                return listExcelHoja;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los datos para un registro con respecto al Reporte SMC - hoja Resumen
        /// </summary>
        /// <param name="central"></param>
        /// <param name="listCalculoMensual"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteSMCResumen(RerCentralDTO central, List<RerCalculoMensualDTO> listCalculoMensual)
        {
            List <RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
            string ingresoGarantizado = (listCM.Count > 0) ? listCM.Sum(x => x.Rercminggarantizado).ToString() : "";
            string sumaIMCP = (listCM.Count > 0) ? listCM.Sum(x => x.Rercmimcp).ToString() : "";
            string sumaSMC = (listCM.Count > 0) ? listCM.Sum(x => x.Rercmsalmencompensar).ToString() : "";

            List<string> list = new List<string>
            {
                central.Emprnomb,
                central.Equinomb,
                ingresoGarantizado,
                sumaIMCP,
                sumaSMC
            };

            return list;
        }

        /// <summary>
        /// Obtiene los datos de un registro para el Reporte Energía Neta
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="fecha"></param>
        /// <param name="indexHora"></param>
        /// <param name="listCentral"></param>
        /// <param name="listCalculoMensual"></param>
        /// <param name="listInyeccionNetaMensualDia"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteEnergiaNeta(int mes, DateTime fecha, int indexHora, List<RerCentralDTO> listCentral,
            List<RerCalculoMensualDTO> listCalculoMensual, List<RerInsumoDiaDTO> listInyeccionNetaMensualDia)
        {
            string fechahora = ObtenerFechaHora(fecha, indexHora);
            DateTime dtFechaHora = DateTime.ParseExact(fechahora, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
            List<string> listINM = new List<string> { fechahora };

            foreach (RerCentralDTO central in listCentral)
            {
                List<RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Rerpprmes == mes && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                List<RerInsumoDiaDTO> listINMDia = listInyeccionNetaMensualDia.Where(x => x.Rerinddiafecdia == fecha && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();

                decimal? fa_n_t = null;
                if (listCM.Count > 0)
                {
                    fa_n_t = ObtenerFA_nt_t(listCM[0].Rercmfatipintervalo, listCM[0].Rercmfafecintervalo, listCM[0].Rercmfavalintervalo, dtFechaHora);
                }

                #region Inyección Neta Mensual
                decimal? inm_n_t = null;
                if (fa_n_t != null && listINMDia.Count > 0)
                {
                    var value = listINMDia[0].GetType().GetProperty($"Rerinddiah{indexHora}").GetValue(listINMDia[0], null);
                    if (value != null)
                    {
                        inm_n_t = (decimal)value; //EN-ΣR
                        listINM.Add((inm_n_t.Value * fa_n_t.Value).ToString());
                    }
                    else
                    {
                        listINM.Add("");
                    }
                }
                else
                {
                    listINM.Add("");
                }
                #endregion
            }

            return listINM;
        }

        /// <summary>
        /// Obtiene los datos de un registro para el Reporte Costo Marginal
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indexHora"></param>
        /// <param name="listCentral"></param>
        /// <param name="listCostoMarginalDia"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteCMgCP(DateTime fecha, int indexHora, List<RerCentralDTO> listCentral, List<RerInsumoDiaDTO> listCostoMarginalDia)
        {
            string fechahora = ObtenerFechaHora(fecha, indexHora);
            List<string> listCMgCP = new List<string> { fechahora };

            foreach (RerCentralDTO central in listCentral)
            {
                List<RerInsumoDiaDTO> listCMDia = listCostoMarginalDia.Where(x => x.Rerinddiafecdia == fecha && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();

                #region CMgCP
                decimal? cmgcp_n_t = null;
                if (listCMDia.Count > 0)
                {
                    var value = listCMDia[0].GetType().GetProperty($"Rerinddiah{indexHora}").GetValue(listCMDia[0], null);
                    if (value != null)
                    {
                        cmgcp_n_t = (decimal)value;
                        listCMgCP.Add(cmgcp_n_t.Value.ToString());
                    }
                    else
                    {
                        listCMgCP.Add("");
                    }
                }
                else
                {
                    listCMgCP.Add("");
                }
                #endregion                
            }

            return listCMgCP;
        }

        /// <summary>
        /// Obtiene los datos de un registro para el Reporte Factor de Ajuste
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="fecha"></param>
        /// <param name="indexHora"></param>
        /// <param name="listCentral"></param>
        /// <param name="listCalculoMensual"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteFA(int mes, DateTime fecha, int indexHora, List<RerCentralDTO> listCentral, List<RerCalculoMensualDTO> listCalculoMensual)
        {
            string fechahora = ObtenerFechaHora(fecha, indexHora);
            DateTime dtFechaHora = DateTime.ParseExact(fechahora, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
            List<string> listFA = new List<string> { fechahora };

            foreach (RerCentralDTO central in listCentral)
            {
                List<RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Rerpprmes == mes && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();

                decimal? fa_n_t = null;
                if (listCM.Count > 0)
                {
                    fa_n_t = ObtenerFA_nt_t(listCM[0].Rercmfatipintervalo, listCM[0].Rercmfafecintervalo, listCM[0].Rercmfavalintervalo, dtFechaHora);
                }

                #region Factor Ajuste
                listFA.Add((fa_n_t != null) ? fa_n_t.Value.ToString() : "");
                #endregion
            }

            return listFA;
        }

        /// <summary>
        /// Obtiene los datos de un registro para el Reporte Saldo VTEA
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="indexHora"></param>
        /// <param name="parametroPrima"></param>
        /// <param name="listCentral"></param>
        /// <param name="dRevisiones"></param>
        /// <param name="listInsVteaMes"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteSaldosVTEAPorRevision(int dia, int indexHora, RerParametroPrimaDTO parametroPrima, List<RerCentralDTO> listCentral, Dictionary<int, RerInsumoVteaDTO> dRevisiones, List<RerInsumoVteaDTO> listInsVteaMes)
        {
            ConcurrentDictionary<int, List<string>> cdListaRevision = new ConcurrentDictionary<int, List<string>>();
            Parallel.ForEach(dRevisiones, revision =>
            {
                List<string> listByRevision = new List<string>();
                foreach (RerCentralDTO central in listCentral)
                {
                    List<RerInsumoVteaDTO> listInsVteaDia = listInsVteaMes.Where(x => x.Rerpprcodi == parametroPrima.Rerpprcodi && x.Rerinefecdia.Day == dia && x.Pericodi == revision.Value.Pericodi && x.Recacodi == revision.Value.Recacodi && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                    if (listInsVteaDia.Count > 0)
                    {
                        var valor_nt_r = listInsVteaDia[0].GetType().GetProperty("Rerinediah" + indexHora).GetValue(listInsVteaDia[0], null);
                        decimal saldoVTEA = (valor_nt_r != null) ? (decimal)valor_nt_r : 0M; 
                        listByRevision.Add(saldoVTEA.ToString());
                    }
                    else
                    {
                        listByRevision.Add("");
                    }
                }

                cdListaRevision.TryAdd(revision.Key, listByRevision);
            });

            string hhmm = ObtenerHoraMinutos(indexHora);
            string sdia = (hhmm == "00:00") ? (dia + 1).ToString("D2") : dia.ToString("D2");
            if (sdia == "32") { sdia = "01"; }
            List<string> list = new List<string> { sdia, hhmm };
            foreach (var revision in dRevisiones)
            {
                if (cdListaRevision.ContainsKey(revision.Key))
                {
                    foreach (string data in cdListaRevision[revision.Key])
                    {
                        list.Add(data);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Obtiene los datos de un registro para el Reporte Saldo VTP
        /// </summary>
        /// <param name="parametroPrima"></param>
        /// <param name="listCentral"></param>
        /// <param name="dRevisiones"></param>
        /// <param name="listInsVtpMes"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteSaldosVTPPorRevision(RerParametroPrimaDTO parametroPrima, List<RerCentralDTO> listCentral, Dictionary<int, RerInsumoVtpDTO> dRevisiones, List<RerInsumoVtpDTO> listInsVtpMes)
        {
            List<string> list = new List<string> { "Saldo VTP" };
            foreach(var revision in dRevisiones)
            {
                foreach (RerCentralDTO central in listCentral)
                {
                    List<RerInsumoVtpDTO> listIVMes = listInsVtpMes.Where(x => x.Rerpprcodi == parametroPrima.Rerpprcodi && x.Pericodi == revision.Value.Pericodi && x.Recpotcodi == revision.Value.Recpotcodi && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                    if (listIVMes.Count > 0)
                    {
                        list.Add(listIVMes[0].Rerinpmestotal.ToString());
                    }
                    else
                    {
                        list.Add("");
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Obtiene los datos de un registro para la hoja "Tarifa Adjudicada" del Reporte "Tarifa Adjudicada". En donde, se visualiza el tipo de cambio
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="listParametroPrima"></param>
        /// <param name="listCentral"></param>
        /// <param name="listCalculoMensual"></param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporteTATAdj(int mes, List<RerParametroPrimaDTO> listParametroPrima, List<RerCentralDTO> listCentral, List<RerCalculoMensualDTO> listCalculoMensual)
        {
            List<RerParametroPrimaDTO> listPP = listParametroPrima.Where(x => x.Rerpprmes == mes).ToList();
            List<string> list = new List<string> { (listPP.Count > 0) ? listPP[0].Rerpprmesaniodesc : "" };

            bool setTipoCambio = false;
            List<string> listPropertyName = new List<string> { "Rercmtaradj" };
            foreach (RerCentralDTO central in listCentral)
            {
                List<RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Rerpprmes == mes && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                if (listCM.Count > 0)
                {
                    if (!setTipoCambio)
                    {
                        var valor = listCM[0].GetType().GetProperty("Rercmtipocambio").GetValue(listCM[0], null);
                        list.Add((valor != null) ? valor.ToString() : "");
                        setTipoCambio = true;
                    }

                    foreach (string propertyName in listPropertyName)
                    {
                        var valor = listCM[0].GetType().GetProperty(propertyName).GetValue(listCM[0], null);
                        list.Add((valor != null) ? valor.ToString() : "");
                    }
                }
                else
                {
                    if (!setTipoCambio)
                    {
                        list.Add("");
                        setTipoCambio = true;
                    }

                    foreach (string propertyName in listPropertyName)
                    {
                        list.Add("");
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Obtiene los datos de un registro de la tabla "Cálculo Central" con respecto a las propiedades especificadas
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="listParametroPrima"></param>
        /// <param name="listCentral"></param>
        /// <param name="listCalculoMensual"></param>
        /// <param name="listPropertyName">Lista de propiedades de tipo decimal</param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporte(int mes, List<RerParametroPrimaDTO> listParametroPrima, List<RerCentralDTO> listCentral, List<RerCalculoMensualDTO> listCalculoMensual, List<string> listPropertyName)
        {
            List<RerParametroPrimaDTO> listPP = listParametroPrima.Where(x => x.Rerpprmes == mes).ToList();
            List<string> list = new List<string> { (listPP.Count > 0) ? listPP[0].Rerpprmesaniodesc : "" };
            foreach (RerCentralDTO central in listCentral)
            {
                List<RerCalculoMensualDTO> listCM = listCalculoMensual.Where(x => x.Rerpprmes == mes && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                if (listCM.Count > 0)
                {
                    foreach (string propertyName in listPropertyName)
                    {
                        var valor = listCM[0].GetType().GetProperty(propertyName).GetValue(listCM[0], null);
                        list.Add((valor != null) ? valor.ToString() : "");
                    }
                }
                else
                {
                    foreach (string propertyName in listPropertyName)
                    {
                        list.Add("");
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Obtiene los datos de un registro de la tabla "Calculo Anual" con respecto a las propiedades especificadas
        /// </summary>
        /// <param name="rowDesc"></param>
        /// <param name="listCentral"></param>
        /// <param name="listCalculoAnual"></param>
        /// <param name="listPropertyName">Lista de propiedades de tipo decimal</param>
        /// <returns></returns>
        private List<string> ObtenerRegistroReporte(string rowDesc, List<RerCentralDTO> listCentral, List<RerCalculoAnualDTO> listCalculoAnual, List<string> listPropertyName)
        {
            List<string> list = new List<string> { rowDesc };
            foreach (RerCentralDTO central in listCentral)
            {
                List<RerCalculoAnualDTO> listCA = listCalculoAnual.Where(x => x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                if (listCA.Count > 0)
                {
                    foreach (string propertyName in listPropertyName)
                    {
                        var valor = listCA[0].GetType().GetProperty(propertyName).GetValue(listCA[0], null);
                        list.Add((valor != null) ? valor.ToString() : "");
                    }
                }
                else
                {
                    foreach (string propertyName in listPropertyName)
                    {
                        list.Add("");
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// Obtiene los datos de un registro para el Reporte Tarifa Adjudicada con respecto a la Energía Neta Ultima Revisión y Energía Dejada de Inyectar
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indexHora"></param>
        /// <param name="listCentral"></param>
        /// <param name="listInyeccionNetaUltimaRevisionDia"></param>
        /// <param name="listEnergiaDejadaInyectarDia"></param>
        /// <param name="listINUR"></param>
        /// <param name="listEDI"></param>
        private void ObtenerRegistroReporteTA_INUR_EDI(DateTime fecha, int indexHora, List<RerCentralDTO> listCentral,
            List<RerInsumoDiaDTO> listInyeccionNetaUltimaRevisionDia, List<RerInsumoDiaDTO> listEnergiaDejadaInyectarDia,
            out List<string> listINUR, out List<string> listEDI)
        {
            string fechahora = ObtenerFechaHora(fecha, indexHora);
            DateTime dtFechaHora = DateTime.ParseExact(fechahora, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
            listINUR = new List<string> { fechahora };
            listEDI = new List<string> { fechahora };

            foreach (RerCentralDTO central in listCentral)
            {
                List<RerInsumoDiaDTO> listINURDia = listInyeccionNetaUltimaRevisionDia.Where(x => x.Rerinddiafecdia == fecha && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                List<RerInsumoDiaDTO> listEDIDia = listEnergiaDejadaInyectarDia.Where(x => x.Rerinddiafecdia == fecha && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();

                #region Inyección Neta Última Revisión
                decimal? inur_n_t = null;
                if (listINURDia.Count > 0)
                {
                    var value = listINURDia[0].GetType().GetProperty($"Rerinddiah{indexHora}").GetValue(listINURDia[0], null);
                    if (value != null)
                    {
                        inur_n_t = (decimal)value; //EN-ΣR
                        listINUR.Add((inur_n_t.Value).ToString());
                    }
                    else
                    {
                        listINUR.Add("");
                    }
                }
                else
                {
                    listINUR.Add("");
                }
                #endregion

                #region Energía Dejada de Inyectar
                decimal? edi_n_t = null;
                if (listEDIDia.Count > 0)
                {
                    var value = listEDIDia[0].GetType().GetProperty($"Rerinddiah{indexHora}").GetValue(listEDIDia[0], null);
                    if (value != null)
                    {
                        edi_n_t = (decimal)value;
                        listEDI.Add(edi_n_t.Value.ToString());
                    }
                    else
                    {
                        listEDI.Add("");
                    }
                }
                else
                {
                    listEDI.Add("");
                }
                #endregion                
            }
        }


        /// <summary>
        /// Obtiene la hora y minutos según indexHora
        /// </summary>
        /// <param name="indexHora"></param>
        /// <returns></returns>
        private string ObtenerHoraMinutos(int indexHora)
        {
            DateTime dt1 = DateTime.Now.Date;
            DateTime dt2 = dt1.AddMinutes(indexHora * ConstantesPrimasRER.numero15);
            string hhmm = string.Format("{0}:{1}", dt2.Hour.ToString("D2"), dt2.Minute.ToString("D2"));
            return hhmm;
        }

        /// <summary>
        /// Obtiene el calculo mensual segun su Reravcodi
        /// </summary>
        /// <param name="iReravcodi">Id de un Año tarifario y una versión</param>
        /// <returns>RerAnioVersionDTO</returns>
        public List<RerCalculoMensualDTO> GetRerCalculoMensualByReravcodi(int iReravcodi)
        {
            return FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(iReravcodi);
        }

        #endregion

        #region Métodos de la tabla RER_INSUMO
        public int SaveRerInsumo(RerInsumoDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetRerInsumoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        public void UpdateRerInsumo(RerInsumoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerInsumoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerInsumoDTO GetByIdRerInsumo(int rerInsumoId)
        {
            return FactoryTransferencia.GetRerInsumoRepository().GetById(rerInsumoId);
        }

        public RerInsumoDTO GetByReravcodiByRerinstipinsumoRerInsumo(int Reravcodi, string Rerinstipinsumo)
        {
            return FactoryTransferencia.GetRerInsumoRepository().GetByReravcodiByRerinstipinsumo(Reravcodi, Rerinstipinsumo);
        }

        #region CU21
        /// <summary>
        /// Proceso que se encarga de Insertar un Insumo
        /// </summary>
        /// <param name="RerAVCodi">Identificador de la tabla RER_ANIOVERSION</param>
        /// <param name="sTipoInsumo">1 = Inyección Neta c/15 minutos; 2 = Costo Marginal c/15 minutos; 3 = Ingresos por Potencia; 4 = Ingresos por Cargo Prima RER; 5 = Energía Dejada de Inyectar (EDI) c/15 minutos; 6 = Saldos VTEA c/15 minutos; 7 = Saldos VTP</param>
        /// <param name="sTipoProceso">M: Manual, A: Automatico</param>
        /// <param name="sLog">Mensaje para el usuario</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public int InsertarInsumo(int RerAVCodi, string sTipoInsumo, string sTipoProceso, string sLog, string sUser)
        {
            RerInsumoDTO dtoInsumo = new RerInsumoDTO();
            //dtoInsumo.RerInsCodi -- PK
            dtoInsumo.Reravcodi = RerAVCodi;
            dtoInsumo.Rerinstipinsumo = sTipoInsumo;
            dtoInsumo.Rerinslog = sLog;
            dtoInsumo.Rerinstipproceso = sTipoProceso;
            dtoInsumo.Rerinsusucreacion = sUser;
            dtoInsumo.Rerinsfeccreacion = DateTime.Now;
            return SaveRerInsumo(dtoInsumo);
        }

        /// <summary>
        /// Proceso que se encarga de Insertar un InsumoMes
        /// </summary>
        /// <param name="iRerInsCodi">Código de insumo Identificador de la tabla RER_INSUMO</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="dtoCentral">Entidad de una Central</param>
        /// <param name="iAnio">Año calendario del mes iRerpprmes</param>
        /// <param name="iRerpprmes">Mes Tarifario</param>
        /// <param name="sTipoInsumo">1 = Inyección Neta c/15 minutos; 2 = Costo Marginal c/15 minutos; 3 = Ingresos por Potencia; 4 = Ingresos por Cargo Prima RER; 5 = Energía Dejada de Inyectar (EDI) c/15 minutos; 6 = Saldos VTEA c/15 minutos; 7 = Saldos VTP</param>
        /// <param name="sRerInmTipInformacion">E: EJECUTADO, M: MEJOR INFORMACION</param>
        /// <param name="sRerInmDetalle">S: Tiene detalle en Insumo dia, N: No tiene detalle en insumo dia </param>
        /// <param name="sUser">Usuario</param>
        /// <param name="dRerinmmestotal">Total del Insumo/Central en el mes, por defecto cero.</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public int InsertarInsumoMes(int iRerInsCodi, int iRerPPrCodi, RerCentralDTO dtoCentral, int iAnio, int iRerpprmes, string sTipoInsumo,
            string sRerInmTipInformacion, string sRerInmDetalle, string sUser, decimal dRerinmmestotal = 0)
        {
            RerInsumoMesDTO dtoInsumoMes = new RerInsumoMesDTO();
            //dtoInsumoMes.RerInmMesCodi            --PK
            dtoInsumoMes.Rerinscodi = iRerInsCodi;
            dtoInsumoMes.Rerpprcodi = iRerPPrCodi;
            dtoInsumoMes.Emprcodi = dtoCentral.Emprcodi;
            dtoInsumoMes.Equicodi = dtoCentral.Equicodi;
            dtoInsumoMes.Rerinmanio = iAnio;
            dtoInsumoMes.Rerinmmes = iRerpprmes;
            dtoInsumoMes.Rerinmtipresultado = sTipoInsumo;
            dtoInsumoMes.Rerinmtipinformacion = sRerInmTipInformacion;
            dtoInsumoMes.Rerinmdetalle = sRerInmDetalle;
            dtoInsumoMes.Rerinmmestotal = dRerinmmestotal; //Por defecto Cero
            dtoInsumoMes.Rerinmmesusucreacion = sUser;
            dtoInsumoMes.Rerinmmesfeccreacion = DateTime.Now;
            return SaveRerInsumoMes(dtoInsumoMes);
        }

        /// <summary>
        /// Proceso que se encarga de ImportarCostoMarginal, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarCostoMarginal(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, int iRerAVAnioTarif, string sUser)
        {
            string sResultado = "";
            string sTipoInsumo = "2"; //Costo Marginal
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;

            try
            {
                
                string sTipoProceso = "A"; //Automatico
                string sRerInmTipInformacion = "E";
                string sRerInmDetalle = "S"; //Si tiene detalle
                string sMensaje = "Se completó la importación automática del Costo Marginal 15 min, para ver los detalles, pulse sobre el botón 'Log'.";
                bool bCargarPronosticado = true;

                //Borrando la data de todo el año
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    //Para cada mes Eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes
                    sResultado = DeleteRerInsumoDiaByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                }

                //Insertamos el procedimiento en la tabla RER_INSUMO mes or mes
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);

                //Asignamos el correlativo de la tabla RER_INSUMO_DIA
                int iRerIndDiaCodi = FactoryTransferencia.GetRerInsumoDiaRepository().GetMaxId();
                List<RerInsumoDiaDTO> listaRerInsumoDia = new List<RerInsumoDiaDTO>();

                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    int iPeriCodi = 0;
                    int iRecaCodi = 0;
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; 
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes

                    bool bEjecutado = true;
                    //Consultamos por el periodo correspondiente
                    int iAnio = iRerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente
                    int iPeriAnioMes = iAnio * 100 + iRerpprmes;
                    PeriodoDTO dtoPeriodo = servicioPeriodo.GetByAnioMes(iPeriAnioMes);

                    //Si en "Parametros Prima RER" -> Origen esta definido como VTEA, es ejecutado
                    if (dtoParametroPrima.Pericodi != null && dtoParametroPrima.Pericodi > 0)
                    {
                        iPeriCodi = (int)dtoParametroPrima.Pericodi;
                    }
                    else
                    {
                        bEjecutado = false;
                    }

                    //Consultamos por la ultima revisión cerrada del periodo, según "Parametros Prima RER" -> Origen
                    if (dtoParametroPrima.Recacodi != null && dtoParametroPrima.Recacodi > 0 && bEjecutado)
                    {
                        //Inyección Neta Última revisión
                        iRecaCodi = (int)dtoParametroPrima.Recacodi;
                    }
                    else
                    {
                        bEjecutado = false;
                    }

                    //Si existe el periodo y la Revisión "Cerrado"
                    if (bEjecutado)
                    {
                        sRerInmTipInformacion = "E"; //Información ejecutada
                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        int iUltimoDia = dRerCenFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Para cada Central - EQUICODI
                            //Insertamos el registro RER_INSUMO_MES de la Central
                            decimal dTotalMes = 0;
                            int iRerInmMesCodi = InsertarInsumoMes(iRerInsCodi, iRerPPrCodi, dtoCentral, iAnio, iRerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                            //Traemos la lista de los Costos Marginales [El numero de registros es el numero de dias en el mes]
                            List<CostoMarginalDTO> listaCostoMarginal = servicioCostoMarginal.ListCostoMarginalByCodigoEntrega(iPeriCodi, iRecaCodi, dtoCentral.Codentcodi);
                            if(listaCostoMarginal == null || listaCostoMarginal.Count <= 0)
                            {
                                throw new Exception(string.Format("Para el periodo {0} en LVTEA no existe información del costo marginal para la central {1}", dtoPeriodo.PeriNombre, dtoCentral.Equinomb));
                            }
                            for (int iDia = 0; iDia < iUltimoDia; iDia++)
                            {
                                //Para cada dia del mes
                                RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO {
                                    Rerinmmescodi = iRerInmMesCodi,
                                    Rerpprcodi = iRerPPrCodi,
                                    Emprcodi = dtoCentral.Emprcodi,
                                    Equicodi = dtoCentral.Equicodi,
                                    Rerinddiafecdia = dRerCenFecha.AddDays(iDia),
                                    Rerindtipresultado = sTipoInsumo,
                                    Rerindtipinformacion = sRerInmTipInformacion,
                                    Rerinddiausucreacion = sUser,
                                    Rerinddiafeccreacion = DateTime.Now
                                };
                                //Las unidades de los Costos Marginales ya estan en S/MWh (en ListCostoMarginalByCodigoEntrega ya se multiplico por 1000)
                                int j = 0;
                                while (j < 96)
                                {
                                    decimal dCostoMarginal = 0;
                                    if (listaCostoMarginal != null && listaCostoMarginal.Count > 0 && listaCostoMarginal.ElementAtOrDefault(iDia) != null)
                                    {
                                        dCostoMarginal = (decimal)listaCostoMarginal[iDia].GetType().GetProperty($"CosMar{j + 1}").GetValue(listaCostoMarginal[iDia]);
                                    }
                                    dtoInsumoDia.GetType().GetProperty($"Rerinddiah{(j + 1)}").SetValue(dtoInsumoDia, dCostoMarginal);
                                    j++;
                                }
                                decimal Rerinddiatotal = 0;
                                if (listaCostoMarginal != null && listaCostoMarginal.Count > 0 && listaCostoMarginal.ElementAtOrDefault(iDia) != null)
                                {
                                    Rerinddiatotal = listaCostoMarginal[iDia].CosMarTotalDia;
                                }
                                dtoInsumoDia.Rerinddiatotal = Rerinddiatotal;
                                
                                //APILAMOS PARA EL BULKINSERT
                                dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                                listaRerInsumoDia.Add(dtoInsumoDia);

                                //Llevamos la cuenta para calcular el total del mes.
                                dTotalMes += dtoInsumoDia.Rerinddiatotal;
                            }
                            #region Actualizar el valor del total del mes en RER_INSUMO_MES
                            RerInsumoMesDTO dtoInsumoMes = GetByIdRerInsumoMes(iRerInmMesCodi);
                            dtoInsumoMes.Rerinmmestotal = dTotalMes;
                            UpdateRerInsumoMes(dtoInsumoMes);
                            #endregion

                        }
                        sLog = sLog + " Ejecutado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                    }
                    else
                    {
                        //Validamos la primera ejecucón de los CM Pronosticados:
                        if (bCargarPronosticado)
                        {
                            //Es la primera vez
                            bCargarPronosticado = false; // Ya no va a volver a ingresar pues ya cargo la información pronosticada de los costos marginales

                            /* Elimina los datos de la tabla temporal la tabla temporal RER_INSUMO_CM_TEMP */
                            FactoryTransferencia.GetRerInsumoRepository().TruncateTablaTemporal(ConstantesPrimasRER.tablaRerInsumoCmTemp);

                            /* Elimina los datos de la tabla temporal la tabla temporal RER_INSUMO_DIA_TEMP */
                            FactoryTransferencia.GetRerInsumoDiaRepository().TruncateTablaTemporal(ConstantesPrimasRER.tablaRerInsumoDiaTemp);
                            //ASSETEC 20250317 - Atención al ticket INC 2025-000924 
                            int codigoenvio = 0;
                            int cont = 0;
                            while (codigoenvio == 0)
                            {
                                // Obtener código de envío pero un mes anterior a: iPeriAnioMes 202301
                                int iPeriAnioMesAnterior = iPeriAnioMes - cont;
                                //Incorporando el caso de cambio de año: 202301
                                if (iPeriAnioMes % 100 == 1)
                                {
                                    iPeriAnioMesAnterior = iPeriAnioMes - 100 + 11;
                                }
                                cont++;
                                codigoenvio = ObtenerCodigoEnvio(iPeriAnioMesAnterior);
                            }
                            // Fin INC 2025-000924

                            // Obtener los datos del PMPO para este mes
                            List<MeMedicionxintervaloDTO> listaData = this.GetDatosPMPOReporteEnvio(codigoenvio, "I");

                            List<RerInsumoTemporalDTO> lstTemporal = new List<RerInsumoTemporalDTO>();
                            RerInsumoTemporalDTO entityInsumoTemporal;
                            foreach (MeMedicionxintervaloDTO dataPmpo in listaData)
                            {
                                entityInsumoTemporal = new RerInsumoTemporalDTO();
                                entityInsumoTemporal.Rerfecinicio = dataPmpo.Medintfechaini;
                                entityInsumoTemporal.Reretapa = Convert.ToInt32(dataPmpo.Semana.Split('/')[0]);
                                entityInsumoTemporal.Rerbloque = dataPmpo.Medintblqnumero;
                                entityInsumoTemporal.Ptomedicodi = dataPmpo.Ptomedicodi;
                                entityInsumoTemporal.Ptomedidesc = dataPmpo.Ptomedidesc;
                                entityInsumoTemporal.Rervalor = dataPmpo.Medinth1;

                                lstTemporal.Add(entityInsumoTemporal);
                            }
                            //Se inserta en la tabla RER_INSUMO_CM_TEMP
                            FactoryTransferencia.GetRerInsumoRepository().BulkInsertTablaTemporal(lstTemporal, ConstantesPrimasRER.tablaRerInsumoCmTemp);

                            /* Obtiene la lista procesada de insumos de los costos marginales pronosticada de acuerdo a la logica del CUS20 */
                            List<RerInsumoDiaTemporalDTO> entitysInsumoDiaTemporal = ObtenerMatrizRerInsumoDia();

                            /* Inserta los registros en la tabla RER_INSUMO_DIA_TEMP; */
                            FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDiaTemporal(entitysInsumoDiaTemporal, ConstantesPrimasRER.tablaRerInsumoDiaTemp);
                            /* El valor de los Costos Marginales importados a este punto estan en dolares */

                        }

                        // Información Pronosticada
                        sRerInmTipInformacion = "P";
                        //Tipo de cambio del mes
                        decimal dTipoCambio = (decimal)dtoParametroPrima.Rerpprtipocambio;

                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        int iUltimoDia = dRerCenFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Para cada Central - EQUICODI
                            //Insertamos el registro RER_INSUMO_MES de la Central
                            decimal dTotalMes = 0;
                            int iRerInmMesCodi = InsertarInsumoMes(iRerInsCodi, iRerPPrCodi, dtoCentral, iAnio, iRerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                            //Traemos la lista de los Costos Marginales Pronosticado [El numero de registros es el numero de dias en el mes]
                            List<RerInsumoDiaTemporalDTO> listaCostoMarginalPronosticado = FactoryTransferencia.GetRerGerCsvRepository().ListTablaCMTemporalDia((int)dtoCentral.Ptomedicodi, dRerCenFecha, dRerCenFecha.AddMonths(1).AddDays(-1), dTipoCambio);
                            for (int iDia = 0; iDia < iUltimoDia; iDia++)
                            {
                                //Para cada dia del mes
                                RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO();
                                dtoInsumoDia.Rerinmmescodi = iRerInmMesCodi;
                                dtoInsumoDia.Rerpprcodi = iRerPPrCodi;
                                dtoInsumoDia.Emprcodi = dtoCentral.Emprcodi;
                                dtoInsumoDia.Equicodi = dtoCentral.Equicodi;
                                dtoInsumoDia.Rerinddiafecdia = dRerCenFecha.AddDays(iDia);
                                dtoInsumoDia.Rerindtipresultado = sTipoInsumo;
                                dtoInsumoDia.Rerindtipinformacion = sRerInmTipInformacion;
                                //Las unidades de los Costos Marginales ya estan en S/MWh (en ListCostoMarginalByCodigoEntrega ya se multiplico por 1000)
                                int j = 0;
                                while (j < 96)
                                {
                                    decimal dCostoMarginal = 0;
                                    if (listaCostoMarginalPronosticado != null && listaCostoMarginalPronosticado.Count > 0 && listaCostoMarginalPronosticado.ElementAtOrDefault(iDia) != null) 
                                    {
                                        dCostoMarginal = (decimal)listaCostoMarginalPronosticado[iDia].GetType().GetProperty($"Rerinddiah{j + 1}").GetValue(listaCostoMarginalPronosticado[iDia]);
                                    }
                                    dtoInsumoDia.GetType().GetProperty($"Rerinddiah{(j + 1)}").SetValue(dtoInsumoDia, dCostoMarginal);
                                    j++;
                                }
                                decimal rerinddiatotal = 0;
                                if (listaCostoMarginalPronosticado != null && listaCostoMarginalPronosticado.Count > 0 && listaCostoMarginalPronosticado.ElementAtOrDefault(iDia) != null)
                                {
                                    rerinddiatotal = (decimal)listaCostoMarginalPronosticado[iDia].Rerinddiatotal;
                                }
                                dtoInsumoDia.Rerinddiatotal = rerinddiatotal;
                                dtoInsumoDia.Rerinddiausucreacion = sUser;
                                dtoInsumoDia.Rerinddiafeccreacion = DateTime.Now;

                                //APILAMOS PARA EL BULKINSERT
                                dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                                listaRerInsumoDia.Add(dtoInsumoDia);

                                //Llevamos la cuenta para calcular el total del mes.
                                dTotalMes += dtoInsumoDia.Rerinddiatotal;
                            }
                            #region Actualizar el valor del total del mes en RER_INSUMO_MES
                            RerInsumoMesDTO dtoInsumoMes = GetByIdRerInsumoMes(iRerInmMesCodi);
                            dtoInsumoMes.Rerinmmestotal = dTotalMes;
                            UpdateRerInsumoMes(dtoInsumoMes);
                            #endregion

                        }

                        sLog = sLog + " Pronosticado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                    }
                    //Actualizamos totales en el registro RER_INSUMO_MES mediante un Update
                }

                //Subimos todos los registros de RER_INSUMO_DIA
                if (listaRerInsumoDia.Count > 0)
                {
                    FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDia(listaRerInsumoDia);
                }

                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally {
                try 
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        /// <summary>
        /// Proceso que se encarga de ImportarCostoMarginal, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarIngresosPorPotencia(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, int iRerAVAnioTarif, string sUser)
        {
            string sResultado = "";
            string sTipoInsumo = "3"; //Ingreso por Potencia
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;
            bool sNoDisponible = false;

            try
            {
                string sTipoProceso = "A"; //Automatico
                string sRerInmTipInformacion;
                string sRerInmDetalle = "N"; //No tiene detalle
                string sMensaje = "Se completó la importación automática del insumo 'Ingreso por Potencia'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";

                //Traemos el ultimo periodo vigente que este Publicado o Cerrado
                int iPeriCodiDisponible = 0;
                PeriodoDTO dtoPericodiDisponible = servicioPeriodo.ListarByEstadoPublicarCerrado().FirstOrDefault();
                if (dtoPericodiDisponible != null)
                {
                    iPeriCodiDisponible = dtoPericodiDisponible.PeriCodi;
                }
                int iRecaPotCodi = 1; //Constante dentro del procedmiento, pues siempre se consulta por la versión mensual en cualquier periodo de VTP

                //Insertamos el procedimiento en la tabla RER_INSUMO 
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);

                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    //Para cada mes
                    int iPeriCodi = 0;
                    //Eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;

                    //Consultamos por el periodo correspondiente
                    int iAnio = iRerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente
                    int iPeriAnioMes = iAnio * 100 + iRerpprmes;
                    PeriodoDTO dtoPeriodo = servicioPeriodo.GetByAnioMes(iPeriAnioMes);
                    if (dtoPeriodo == null)
                    {
                        iPeriCodi = iPeriCodiDisponible; //Asignamos el ultimo periodo que si esta publicado y cerrado
                    }
                    else
                    {
                        iPeriCodi = dtoPeriodo.PeriCodi;

                    }
                    //Consultamos por la ultima revisión cerrada del periodo
                    if (iPeriCodi > 0)
                    {
                        VtpRecalculoPotenciaDTO dtoRecalculoPotencia = servicioTransfPotencia.GetByIdVtpRecalculoPotenciaCerrado(iPeriCodi, iRecaPotCodi);
                        if (dtoRecalculoPotencia == null)
                        {
                            //Como no existe la revisión Mensual, nos traemos el recalculo del Periodo Mensual que si esta Disponible
                            dtoRecalculoPotencia = servicioTransfPotencia.GetByIdVtpRecalculoPotencia(iPeriCodiDisponible, iRecaPotCodi);
                            iPeriCodi = iPeriCodiDisponible;
                            sNoDisponible = true;
                        }
                    }
                    
                    //Si existe el Periodo y la Revisión de LVTP "Cerrado"
                    if (iPeriCodi > 0)
                    {
                        sRerInmTipInformacion = "E"; //Información ejecutada
                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Para cada Central - EQUICODI
                            decimal dRerinmmestotal = 0;
                            //Calculamos su ingreso por potencia del mes
                            VtpIngresoPotUnidPromdDTO dtoIngresoPotUnidPromd = servicioTransfPotencia.GetByVtpIngresoPotUnidPromdCentral(iPeriCodi, iRecaPotCodi, dtoCentral.Equicodi);
                            if (dtoIngresoPotUnidPromd.Inpuprimportepromd != 0)
                            {
                                dRerinmmestotal = dtoIngresoPotUnidPromd.Inpuprimportepromd;
                            }
                            else
                            {
                                //Al no encontrar el IP por la central, consultamos por sus unidades, donde la Central es el padre
                                dtoIngresoPotUnidPromd = servicioTransfPotencia.GetByVtpIngresoPotUnidPromdCentralSumUnidades(iPeriCodi, iRecaPotCodi, dtoCentral.Equicodi);
                                if (dtoIngresoPotUnidPromd.Inpuprimportepromd != 0)
                                {
                                    dRerinmmestotal = dtoIngresoPotUnidPromd.Inpuprimportepromd;
                                }
                            }

                            //Insertamos el registro RER_INSUMO_MES de la Central
                            RerInsumoMesDTO dtoInsumoMes = new RerInsumoMesDTO();
                            //dtoInsumoMes.RerInmMesCodi            --PK
                            dtoInsumoMes.Rerinscodi = iRerInsCodi;
                            dtoInsumoMes.Rerpprcodi = iRerPPrCodi;
                            dtoInsumoMes.Emprcodi = dtoCentral.Emprcodi;
                            dtoInsumoMes.Equicodi = dtoCentral.Equicodi;
                            dtoInsumoMes.Rerinmanio = iAnio;
                            dtoInsumoMes.Rerinmmes = iRerpprmes;
                            dtoInsumoMes.Rerinmtipresultado = sTipoInsumo;
                            dtoInsumoMes.Rerinmtipinformacion = sRerInmTipInformacion;
                            dtoInsumoMes.Rerinmdetalle = sRerInmDetalle;
                            dtoInsumoMes.Rerinmmestotal = dRerinmmestotal;
                            dtoInsumoMes.Rerinmmesusucreacion = sUser;
                            dtoInsumoMes.Rerinmmesfeccreacion = DateTime.Now;
                            SaveRerInsumoMes(dtoInsumoMes);
                        }
                        string valorProvicional = "";
                        if (sNoDisponible || dtoPeriodo == null)
                        {
                            sLog = sLog + "Proyectado: [" + iAnio.ToString() + "." + ConstantesPrimasRER.mesesDesc[iRerpprmes - 1] + "] <br>";
                        }
                        else
                        {
                            sLog = sLog + " Ejecutado: [" + dtoPeriodo.PeriNombre + "] <br>";
                        }
                    }
                }

                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        /// <summary>
        /// Proceso que se encarga de ImportarInyeccionNeta, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarInyeccionNeta(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, int iRerAVAnioTarif, string sUser, string sInsumoVersión)
        {
            string sResultado = "";
            string sTipoInsumo = sInsumoVersión; //Inyección Neta [1] Ultima versión / [8] Mensual 
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;

            try
            {
                string sTipoProceso = "A"; //Automatico
                string sRerInmTipInformacion;
                string sRerInmDetalle = "S"; //Si tiene detalle
                string sMensaje = "Se completó la importación automática del insumo 'Inyección Neta 15 min.'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";
                
                //Eliminamos ña información del año
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    //Para cada mes Eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes
                    sResultado = DeleteRerInsumoDiaByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                }

                //Insertamos el procedimiento en la tabla RER_INSUMO mes or mes
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);
                
                //Asignamos el correlativo de la tabla RER_INSUMO_DIA
                int iRerIndDiaCodi = FactoryTransferencia.GetRerInsumoDiaRepository().GetMaxId();
                List<RerInsumoDiaDTO> listaRerInsumoDia = new List<RerInsumoDiaDTO>();

                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    int iPeriCodi = 0;
                    int iRecaCodi = 0;
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; 
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes

                    bool bEjecutado = true;
                    //Consultamos por el periodo correspondiente
                    int iAnio = iRerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente

                    //Si en "Parametros Prima RER" -> Origen esta definido como VTEA, es ejecutado
                    if (dtoParametroPrima.Pericodi != null && dtoParametroPrima.Pericodi > 0)
                    {
                        iPeriCodi = (int)dtoParametroPrima.Pericodi;
                    }
                    else 
                    {
                        bEjecutado = false;
                    }

                    //Consultamos por la ultima revisión cerrada del periodo, según "Parametros Prima RER" -> Origen
                    if (dtoParametroPrima.Recacodi != null && dtoParametroPrima.Recacodi > 0 && bEjecutado)
                    {
                        if (sTipoInsumo == ConstantesPrimasRER.tipoResultadoInyeccionNetaMensual)
                        {
                            //Para la Inyección Neta Mensual, seteamos:
                            iRecaCodi = 1;
                        }
                        else
                        {
                            //Inyección Neta Última revisión
                            iRecaCodi = (int)dtoParametroPrima.Recacodi;
                        }
                    }
                    else
                    {
                        bEjecutado = false;
                    }

                    //Si existe el periodo y la Revisión "Cerrado"
                    if (bEjecutado)
                    {
                        sRerInmTipInformacion = "E"; //Información ejecutada
                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        int iUltimoDia = dRerCenFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Para cada Central - EQUICODI
                            //Insertamos el registro RER_INSUMO_MES de la Central
                            decimal dTotalMes = 0;
                            int iRerInmMesCodi = InsertarInsumoMes(iRerInsCodi, iRerPPrCodi, dtoCentral, iAnio, iRerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                            //Traemos la lista de Energia de Entrega [El numero de registros es el numero de dias en el mes]
                            List<ValorTransferenciaDTO> listaEntrega = servicioValorTransferencia.ListarEnergiaEntregaDetalle(iPeriCodi, iRecaCodi, dtoCentral.Codentcodi);
                            //Traemos la lista de Codigos de Retiro de la Central, sumarizado por dia
                            List<ValorTransferenciaDTO> listaRetiro = new List<ValorTransferenciaDTO>();
                            string listaCodigosRetiro = ListaCodigoRetiroByEquipo(iRerPPrCodi, dtoCentral.Equicodi);
                            if(listaCodigosRetiro != "")
                                listaRetiro = servicioValorTransferencia.ListarEnergiaRetiroDetalle(iPeriCodi, iRecaCodi, listaCodigosRetiro);
                            //Aplicamos la resta en cada día y almacenamos en la Tabla Insumo
                            for (int iDia = 0; iDia < iUltimoDia; iDia++)
                            {
                                //Para cada dia del mes
                                RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO {
                                    Rerinmmescodi = iRerInmMesCodi,
                                    Rerpprcodi = iRerPPrCodi,
                                    Emprcodi = dtoCentral.Emprcodi,
                                    Equicodi = dtoCentral.Equicodi,
                                    Rerinddiafecdia = dRerCenFecha.AddDays(iDia),
                                    Rerindtipresultado = sTipoInsumo,
                                    Rerindtipinformacion = sRerInmTipInformacion,
                                    Rerinddiausucreacion = sUser,
                                    Rerinddiafeccreacion = DateTime.Now
                                };
                                dtoInsumoDia.Rerinddiatotal = 0;
                                int j = 0;
                                while (j < 96)
                                {
                                    decimal dEntrega = 0;
                                    if (listaEntrega != null && listaEntrega.Count > 0 && listaEntrega.ElementAtOrDefault(iDia) != null)
                                    {
                                        dEntrega = (decimal)listaEntrega[iDia].GetType().GetProperty($"VT{j + 1}").GetValue(listaEntrega[iDia]);
                                    }
                                    decimal dRetiro = 0;
                                    if(listaRetiro != null && listaRetiro.Count > 0 && listaRetiro.ElementAtOrDefault(iDia) != null)
                                    {
                                        dRetiro = (decimal)listaRetiro[iDia].GetType().GetProperty($"VT{j + 1}").GetValue(listaRetiro[iDia]);
                                    }
                                    decimal Rerinddiah = dEntrega - dRetiro;
                                    //ASSETEC 20240902
                                    if (Rerinddiah < 0) Rerinddiah = 0;
                                    dtoInsumoDia.Rerinddiatotal += Rerinddiah; // se totaliza cada intervalo de 15 minutos.
                                    //FIN
                                    dtoInsumoDia.GetType().GetProperty($"Rerinddiah{(j + 1)}").SetValue(dtoInsumoDia, Rerinddiah);
                                    j++;
                                }

                                //ASSETEC 20240902 - Ya no va, pues se tiene que totalizar cada intervalo de 15 minutos.
                                /*
                                dtoInsumoDia.Rerinddiatotal = 0;
                                if (listaEntrega != null && listaEntrega.Count > 0 && listaEntrega.ElementAtOrDefault(iDia) != null)
                                {
                                    dtoInsumoDia.Rerinddiatotal = listaEntrega[iDia].VTTotalEnergia;
                                }
                                if (listaRetiro != null && listaRetiro.Count > 0 && listaRetiro.ElementAtOrDefault(iDia) != null)
                                {
                                    dtoInsumoDia.Rerinddiatotal -= listaRetiro[iDia].VTTotalEnergia;
                                }*/
                                //FIN

                                //APILAMOS PARA EL BULKINSERT
                                dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                                listaRerInsumoDia.Add(dtoInsumoDia);

                                //Llevamos la cuenta para calcular el total del mes.
                                dTotalMes += dtoInsumoDia.Rerinddiatotal;
                            }
                            #region Actualizar el valor del total del mes en RER_INSUMO_MES
                            RerInsumoMesDTO dtoInsumoMes = GetByIdRerInsumoMes(iRerInmMesCodi);
                            dtoInsumoMes.Rerinmmestotal = dTotalMes;
                            UpdateRerInsumoMes(dtoInsumoMes);
                            #endregion
                        }
                        sLog = sLog + " Ejecutado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                    }
                    else
                    {
                        sRerInmTipInformacion = "P"; //Información pronosticada
                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        DateTime dRerCenFechaFin = dRerCenFecha.AddMonths(1).AddDays(-1);
                        int iUltimoDia = dRerCenFechaFin.Day; //Devuelve 28/29/30/31
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Para cada Central - EQUICODI
                            //Insertamos el registro RER_INSUMO_MES de la Central
                            decimal dTotalMes = 0;
                            int iRerInmMesCodi = InsertarInsumoMes(iRerInsCodi, iRerPPrCodi, dtoCentral, iAnio, iRerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                            //Traemos la lista de Energia pronosticada [El numero de registros es el numero de dias en el mes]
                            List<RerGerCsvDetDTO> listaEnergia = ListRerGerCsvDetsByEquipo(dtoCentral.Equicodi, dRerCenFecha, dRerCenFechaFin);
                            //ASSETEC 20250310: Se completa los dias iniciales con el primer registro valido
                            int iDiasDiferencia = iUltimoDia - listaEnergia.Count;
                            int iRegistro = 0;
                            //end 20250310
                            //Completamos el registro para todos los dias del mes
                            for (int iDia = 0; iDia < iUltimoDia; iDia++)
                            {
                                //ASSETEC 20250310
                                if (iDiasDiferencia >= 0)
                                    iDiasDiferencia--;
                                else
                                    iRegistro++;

                                //Para cada dia del mes
                                RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO();
                                dtoInsumoDia.Rerinmmescodi = iRerInmMesCodi;
                                dtoInsumoDia.Rerpprcodi = iRerPPrCodi;
                                dtoInsumoDia.Emprcodi = dtoCentral.Emprcodi;
                                dtoInsumoDia.Equicodi = dtoCentral.Equicodi;
                                dtoInsumoDia.Rerinddiafecdia = dRerCenFecha.AddDays(iDia); 
                                dtoInsumoDia.Rerindtipresultado = sTipoInsumo;
                                dtoInsumoDia.Rerindtipinformacion = sRerInmTipInformacion;
                                int j = 0;
                                while (j < 96)
                                {
                                    decimal dEnergia = 0;
                                    if (listaEnergia != null && listaEnergia.Count > 0 && listaEnergia.ElementAtOrDefault(iRegistro) != null)
                                    {
                                        dEnergia = (decimal)listaEnergia[iRegistro].GetType().GetProperty($"Regedh{j + 1}").GetValue(listaEnergia[iRegistro]);
                                    }
                                    dtoInsumoDia.GetType().GetProperty($"Rerinddiah{(j + 1)}").SetValue(dtoInsumoDia, dEnergia);
                                    dtoInsumoDia.Rerinddiatotal += dEnergia;
                                    j++;
                                }
                                dtoInsumoDia.Rerinddiausucreacion = sUser;
                                dtoInsumoDia.Rerinddiafeccreacion = DateTime.Now;

                                //APILAMOS PARA EL BULKINSERT
                                dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                                listaRerInsumoDia.Add(dtoInsumoDia);

                                //Llevamos la cuenta para calcular el total del mes.
                                dTotalMes += dtoInsumoDia.Rerinddiatotal;
                            }
                            #region Actualizar el valor del total del mes en RER_INSUMO_MES
                            RerInsumoMesDTO dtoInsumoMes = GetByIdRerInsumoMes(iRerInmMesCodi);
                            dtoInsumoMes.Rerinmmestotal = dTotalMes;
                            UpdateRerInsumoMes(dtoInsumoMes);
                            #endregion
                        }

                        sLog = sLog + " Pronosticado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                    }
                }
                //Subimos todos los registros de RER_INSUMO_DIA
                if (listaRerInsumoDia.Count > 0)
                {
                    FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDia(listaRerInsumoDia);
                }

                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }
            return sResultado;
        }

        /// <summary>
        /// Proceso que se encarga de ImportarIngresoPorCargoPrimaRER, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarIngresoPorCargoPrimaRER(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, int iRerAVAnioTarif, string sUser)
        {
            string sResultado = "";
            string sTipoInsumo = "4"; //Ingresos por Cargo Prima RER
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;

            try
            {
                string sTipoProceso = "A"; //Automatico
                string sRerInmTipInformacion;
                string sRerInmDetalle = "N"; //No tiene detalle
                string sMensaje = "Se completó la importación automática del insumo 'Ingreso por Cargo Prima RER'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";

                int iRecaPotCodi = 1; //Constante dentro del procedimiento, pues siempre se consulta por la versión mensual en cualquier periodo de VTP

                //Insertamos el procedimiento en la tabla RER_INSUMO mes or mes
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);

                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    int iPeriCodi = 0;
                    int iRecaCodi = 0;
                    decimal dTotalMes = 0;
                    //Para cada mes Eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;

                    //Consultamos por el periodo correspondiente
                    int iAnio = iRerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente
                    int iPeriAnioMes = iAnio * 100 + iRerpprmes;
                    PeriodoDTO dtoPeriodo = servicioPeriodo.GetByAnioMes(iPeriAnioMes);
                    if (dtoPeriodo == null)
                    {
                        iPeriCodi = 0;
                        //sLog = sLog + " 'No se encontró información en " + iAnio.ToString() + "." + ConstantesPrimasRER.mesesDesc[iRerpprmes - 1] + "' <br>";
                        //continue; //vamos al siguiente mes del año tarifario
                    }
                    else
                    {
                        iPeriCodi = dtoPeriodo.PeriCodi;

                    }
                    //Consultamos por la revisión mensual cerrada del periodo
                    if (iPeriCodi > 0)
                    {
                        VtpRecalculoPotenciaDTO dtoRecalculoPotencia = servicioTransfPotencia.GetByIdVtpRecalculoPotencia(iPeriCodi, iRecaPotCodi);
                        if (dtoRecalculoPotencia == null)
                        {
                            iPeriCodi = 0;
                            //continue; //vamos al siguiente mes del año tarifario
                        }
                    }

                    //Si existe el Periodo y la Revisión Mensual de LVTP "Cerrado"
                    if (iPeriCodi > 0)
                    {
                        sRerInmTipInformacion = "E"; //Información ejecutada
                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Para cada Central - EQUICODI
                            decimal dRerinmmestotal = 0;
                            //Calculamos su ingreso por potencia del mes
                            VtpPeajeEmpresaPagoDTO dtoPeajeEmpresaPago = servicioTransfPotencia.GetByIdVtpPeajeEmpresaPagoByCargoPrima(iPeriCodi, iRecaPotCodi, dtoCentral.Pingnombre);
                            if (dtoPeajeEmpresaPago != null)
                            {
                                dRerinmmestotal = dtoPeajeEmpresaPago.Pempagsaldo;
                            }

                            //Insertamos el registro RER_INSUMO_MES de la Central
                            RerInsumoMesDTO dtoInsumoMes = new RerInsumoMesDTO();
                            //dtoInsumoMes.RerInmMesCodi            --PK
                            dtoInsumoMes.Rerinscodi = iRerInsCodi;
                            dtoInsumoMes.Rerpprcodi = iRerPPrCodi;
                            dtoInsumoMes.Emprcodi = dtoCentral.Emprcodi;
                            dtoInsumoMes.Equicodi = dtoCentral.Equicodi;
                            dtoInsumoMes.Rerinmanio = iAnio;
                            dtoInsumoMes.Rerinmmes = iRerpprmes;
                            dtoInsumoMes.Rerinmtipresultado = sTipoInsumo;
                            dtoInsumoMes.Rerinmtipinformacion = sRerInmTipInformacion;
                            dtoInsumoMes.Rerinmdetalle = sRerInmDetalle;
                            dtoInsumoMes.Rerinmmestotal = dRerinmmestotal;
                            dtoInsumoMes.Rerinmmesusucreacion = sUser;
                            dtoInsumoMes.Rerinmmesfeccreacion = DateTime.Now;
                            SaveRerInsumoMes(dtoInsumoMes);
                        }
                        sLog = sLog + " Ejecutado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                    }
                    else
                    {
                        sRerInmTipInformacion = "P"; //Información pronosticada

                        DateTime dRerCenFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            RerInsumoMesDTO dtoInsumoMes = new RerInsumoMesDTO();
                            //dtoInsumoMes.RerInmMesCodi            --PK
                            dtoInsumoMes.Rerinscodi = iRerInsCodi;
                            dtoInsumoMes.Rerpprcodi = iRerPPrCodi;
                            dtoInsumoMes.Emprcodi = dtoCentral.Emprcodi;
                            dtoInsumoMes.Equicodi = dtoCentral.Equicodi;
                            dtoInsumoMes.Rerinmanio = iAnio;
                            dtoInsumoMes.Rerinmmes = iRerpprmes;
                            dtoInsumoMes.Rerinmtipresultado = sTipoInsumo;
                            dtoInsumoMes.Rerinmtipinformacion = sRerInmTipInformacion;
                            dtoInsumoMes.Rerinmdetalle = sRerInmDetalle;
                            dtoInsumoMes.Rerinmmestotal = 0;
                            dtoInsumoMes.Rerinmmesusucreacion = sUser;
                            dtoInsumoMes.Rerinmmesfeccreacion = DateTime.Now;
                            SaveRerInsumoMes(dtoInsumoMes);
                        }
                        sLog = sLog + " Información completado con '0': [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                    }
                }

                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        /// <summary>
        /// Proceso que se encarga de ImportarEnergiaDejadaInyectar, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarEnergiaDejadaInyectar(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, int iRerAVAnioTarif, string sUser)
        {
            string sResultado = "";
            string sTipoInsumo = "5";           // Energia dejada de inyectar (EDI)
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;

            try
            {

                #region Variables del método
                //Para rer_insumo
                string sTipoProceso = "A";          // Automatico
                string sMensaje = "Se completó la importación automática del insumo 'Energía Dejada de Inyectar 15 min.'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";

                // Para rer_insumo_mes 
                int iRerpprmes = 0;
                string sRerInmTipInformacion = "M"; // Mejor información
                string sRerInmDetalle = "N";        // No tiene detalle
                decimal dTotalMes = 0;               // representa al valor del total del mes

                // Para rer_insumo_dia
                string sRerIndTipInformacion = "M";
                #endregion

                #region Creamos un RER_INSUMO
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);
                #endregion

                #region Procedimiento para obtencion de datos
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    #region Para cada mes eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    sResultado = DeleteRerInsumoDiaByParametroPrimaAndTipo(dtoParametroPrima.Rerpprcodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(dtoParametroPrima.Rerpprcodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                    #endregion
                    #region Obtenemos todos los comparativos con solicitud aprobada y esten en su ultima evaluación para un mes
                    int iAnioCalendario = dtoParametroPrima.Rerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;
                    DateTime dDia1Mes = new DateTime(iAnioCalendario, dtoParametroPrima.Rerpprmes, 1, 0, 0, 0); // Primer dia del mes

                    List<RerComparativoDetDTO> listComparativoDet = FactoryTransferencia.GetRerComparativoDetRepository().GetComparativoAprobadaValidadByMes(dDia1Mes);
                    #endregion

                    #region Eliminando el 00:00 del dia 1 del mes
                    if (listComparativoDet.Count == 0)
                    {
                        sLog += "El mes [" + iAnioCalendario.ToString("D4") + "." + ConstantesPrimasRER.mesesDesc[dtoParametroPrima.Rerpprmes - 1] + "] no posee solicitudes EDI con estado = Aprobada y validada.<br>";
                        continue;
                    }
                    #endregion

                    #region Rectificando el valor de "00:00" a "24:00" del dia anterior
                    foreach (var elemento in listComparativoDet)
                    {
                        if (elemento.Rercdthora == "00:00") {
                            elemento.Rercdthora = "24:00";
                            elemento.Rercdtfecha = elemento.Rercdtfecha.AddDays(-1);
                        }
                    }
                    #endregion

                    #region Agrupamos listComparativoDet por emprcodi, equicodi y rercdthora para sumar su Rercdteneestimada
                    List<RerComparativoDetDTO> nuevaListComparativoDet = listComparativoDet
                        .GroupBy(p => new { p.Emprcodi, p.Equicodi, p.Rercdtfecha, p.Rercdthora })
                        .Select(g => new RerComparativoDetDTO
                        {
                            Emprcodi = g.Key.Emprcodi,
                            Equicodi = g.Key.Equicodi,
                            Rercdtfecha = g.Key.Rercdtfecha,
                            Rercdthora = g.Key.Rercdthora,
                            Rercdteneestimada = g.Sum(p => p.Rercdteneestimada)
                        }).ToList();
                    #endregion

                    #region Recorremos todos los {emprcodi, equicodi}
                    var gruposPorEmprcodiEquicodi = nuevaListComparativoDet
                        .GroupBy(p => new { p.Emprcodi, p.Equicodi });

                    foreach (var grupoPorEmprcodiEquicodi in gruposPorEmprcodiEquicodi)
                    {
                        //Asignamos el correlativo de la tabla RER_INSUMO_DIA
                        int iRerIndDiaCodi = FactoryTransferencia.GetRerInsumoDiaRepository().GetMaxId();
                        List<RerInsumoDiaDTO> listaRerInsumoDia = new List<RerInsumoDiaDTO>();

                        int emprcodi = grupoPorEmprcodiEquicodi.Key.Emprcodi;
                        int equicodi = grupoPorEmprcodiEquicodi.Key.Equicodi;
                        dTotalMes = 0;
                        RerInsumoMesDTO dtoInsumoMes = new RerInsumoMesDTO
                        {
                            Rerinscodi = iRerInsCodi,
                            Rerpprcodi = dtoParametroPrima.Rerpprcodi,
                            Emprcodi = emprcodi,
                            Equicodi = equicodi,
                            Rerinmanio = iAnioCalendario,
                            Rerinmmes = dtoParametroPrima.Rerpprmes,
                            Rerinmtipresultado = sTipoInsumo,
                            Rerinmtipinformacion = sRerInmTipInformacion,
                            Rerinmdetalle = sRerInmDetalle,
                            Rerinmmestotal = 0, //Por defecto Cero
                            Rerinmmesusucreacion = sUser,
                            Rerinmmesfeccreacion = DateTime.Now
                        };
                        int iRerInsMesCodi = SaveRerInsumoMes(dtoInsumoMes);

                        var gruposPorDia = grupoPorEmprcodiEquicodi
                            .GroupBy(p => p.Rercdtfecha).OrderBy(g => g.Key);
                        #region Recorremos todos los dias del mes
                        foreach (var grupoDia in gruposPorDia)
                        {
                            DateTime fecha = grupoDia.Key;

                            #region Creamos el RerinsumoDia
                            RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO
                            {
                                Rerinmmescodi = iRerInsMesCodi,
                                Rerpprcodi = dtoParametroPrima.Rerpprcodi,
                                Emprcodi = emprcodi,
                                Equicodi = equicodi,
                                Rerinddiafecdia = fecha,
                                Rerindtipresultado = sTipoInsumo,
                                Rerindtipinformacion = sRerIndTipInformacion,
                                Rerinddiatotal = 0,
                                Rerinddiausucreacion = sUser,
                                Rerinddiafeccreacion = DateTime.Now
                            };
                            ResetearHaCero(dtoInsumoDia);
                            #endregion

                            foreach (var dato15min in grupoDia)
                            {
                                string rercdthora = dato15min.Rercdthora;
                                decimal? Rercdteneestimada = dato15min.Rercdteneestimada;

                                if (rercdthora == "24:00"){
                                    dtoInsumoDia.Rerinddiah96 = Rercdteneestimada;
                                }
                                else
                                {
                                    TimeSpan tiempoAgregado = TimeSpan.Parse(dato15min.Rercdthora);
                                    int indice = (tiempoAgregado.Hours * 4) + (tiempoAgregado.Minutes / 15);
                                    var propiedad = typeof(RerInsumoDiaDTO).GetProperty($"Rerinddiah{indice}");
                                    if (propiedad != null)
                                    {
                                        propiedad.SetValue(dtoInsumoDia, Rercdteneestimada);
                                    }
                                }
                                dtoInsumoDia.Rerinddiatotal += Rercdteneestimada ?? 0.0m;
                                dTotalMes += Rercdteneestimada ?? 0.0m;
                            }

                            //APILAMOS PARA EL BULKINSERT
                            dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                            listaRerInsumoDia.Add(dtoInsumoDia);

                            //Código para agregar elemento por elemento
                            #region Guardamos la data de un dia
                            //SaveRerInsumoDia(dtoInsumoDia);
                            #endregion
                        }
                        //Subimos todos los registros de RER_INSUMO_DIA
                        if (listaRerInsumoDia.Count > 0)
                        {
                            FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDia(listaRerInsumoDia);
                        }
                        #region Actualizar el valor del total del mes en RER_INSUMO_MES
                        RerInsumoMesDTO dtoInsumoMesPorActualizar = GetByIdRerInsumoMes(iRerInsMesCodi);
                        dtoInsumoMesPorActualizar.Rerinmmestotal = dTotalMes;
                        UpdateRerInsumoMes(dtoInsumoMesPorActualizar);
                        #endregion

                        #endregion
                    }
                    #endregion

                    sLog += "El mes [" + iAnioCalendario.ToString("D4") + "." + ConstantesPrimasRER.mesesDesc[iRerpprmes - 1] + "] se cargó correctamente.<br>";

                }
                #endregion
                
                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        public RerInsumoDiaDTO SumarValoresH(RerInsumoDiaDTO previo, RerInsumoDiaDTO actual)
        {
            RerInsumoDiaDTO resultado = new RerInsumoDiaDTO();

            for (int i = 1; i <= 96; i++)
            {
                var propertyName = $"Rerinddiah{i}";
                var property = typeof(RerInsumoDiaDTO).GetProperty(propertyName);

                if (property != null && property.PropertyType == typeof(double))
                {
                    double valorPrevio = (double)property.GetValue(previo);
                    double valorActual = (double)property.GetValue(actual);

                    property.SetValue(resultado, valorPrevio + valorActual);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Proceso que se encarga de ImportarSaldosVTEA, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="sVersion">Versión del año tarifario: [0]Anual; [1]Trimestre 1; [2]Trimestre 2; [3]Trimestre 3, [4]Trimestre 4; [5]iquidacion</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarSaldosVTEA(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, string sVersion, int iRerAVAnioTarif, string sUser)
        {
            string sResultado = "";
            string sTipoInsumo = "6"; //Saldos VTEA c/15 minutos;
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;

            try
            {
                string sTipoProceso = "A"; //Automatico
                string sRerInmTipInformacion = "E"; //Información ejecutada
                string sRerInmDetalle = "S"; //Si tiene detalle
                string sMensaje = "Se completó la importación automática del insumo 'Saldos VTEA 15 min.'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";

                //Traemos la lista de todos los FAJ's del AñoTarifarioAnterior
                List<RerCalculoMensualDTO> listaCalculoMensualAnterior = new List<RerCalculoMensualDTO>();
                int iReravaniotarifAnterior = iRerAVAnioTarif - 1;
                //Traemos la lista de todas las versiones de ese año, son 6(versiones) x 12(meses) = 72 registros
                List<RerAnioVersionDTO> listaAnioVersionAnterior = ListRerAnioVersionesByAnio(iReravaniotarifAnterior);
                foreach (RerAnioVersionDTO AnioVersionAnterior in listaAnioVersionAnterior)
                {
                    //Para cada grupo de 12 meses, dentro de una versión, analizamos quien esta completaen RER_CALCULO_MENSUAL
                    listaCalculoMensualAnterior = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(AnioVersionAnterior.Reravcodi);
                    if (listaCalculoMensualAnterior != null && listaCalculoMensualAnterior.Count > 0)
                    {
                        //Lista con los FAJ de cada Central RER en la versión Reravcodi
                        break;
                    }
                }

                //Para obtener el FAJ del presente Año Tarifario
                List<RerCentralUnicaDTO> listaFAJAnioActual = ObtenerFAJ(iRerAVAnioTarif, sVersion);
                //Seteamos la Fecha (Mes) de donde proviene la información
                DateTime dFechaCorte = new DateTime(iRerAVAnioTarif, 5, 1, 0, 0, 0); // Retorna el primer dia de Mayo del Año Tariario

                //Eliminamos toda la data del año tarifario
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    //Para cada mes Eliminamos la data en la tabla RER_INSUMO_MES, RER_INSUMO_DIA
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes

                    //Eliminamos Insumo VTEA -> Todo lo que esta relacionado con el Año/Mes de ParametroPrima
                    sResultado = DeleteRerInsumoVTEAyParametroPrimaAndMes(iRerPPrCodi);
                    if (sResultado != "")
                        return sResultado;

                    //Elimanos Insumo Mes
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                }

                //Insertamos el procedimiento en la tabla RER_INSUMO mes or mes
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);

                //Para cada Mes del año tariario
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    //Asignamos el correlativo de la tabla RER_INSUMO_VTEA
                    int iRerInECodi = FactoryTransferencia.GetRerInsumoVteaRepository().GetMaxId();
                    List<RerInsumoVteaDTO> listaRerInsumoVtea = new List<RerInsumoVteaDTO>();

                    int iRerpprmes = dtoParametroPrima.Rerpprmes;
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes

                    //Consultamos por el periodo correspondiente relacionado al año tarifario
                    int iAnio = iRerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente

                    //Traemos la lista de Periodos destino que se aplican en este mes: iRerPPrCodi
                    List<RerParametroRevisionDTO> listRerParametroRevision = ListRerParametroRevisionesByRerpprcodiByTipo(iRerPPrCodi, "VTEA");

                    //Por cada elemento de la lista, vamos a aplicar la diferencia de la Revisión[N+1] - Revisión[N] (este ultimo a lo mas es la mensual)
                    //El Resultado = Saldo, lo almacenamos en Insumo VTEA
                    foreach (RerParametroRevisionDTO dtoRerParametroRevision in listRerParametroRevision)
                    {
                        int iPeriCodi = dtoRerParametroRevision.Pericodi;
                        int iRecaCodi = dtoRerParametroRevision.Recacodi;
                        int iRecaCodiAnterior = iRecaCodi - 1; // De la operación, a lo menos podria salir 1: Mensual

                        //Buscamos la información relacionada al periodo
                        PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(iPeriCodi);
                        //Seteamos la Fecha (Mes) de donde proviene la información
                        DateTime dRerCenFecha = new DateTime(dtoPeriodo.AnioCodi, dtoPeriodo.MesCodi, 1, 0, 0, 0); // Primer dia del mes
                        int iUltimoDia = dRerCenFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31

                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            /*if (dtoCentral.Equicodi != 14777)
                                continue; //Para realizar seguimientos    */
                            //Traemos el valor del FAJ para la Central en el ParametroPrima(MES)
                            int iRercmfatipintervalo = 1; //Para el periodo, todos sus intervalos de su Factor de Ajuste es 1.
                            DateTime dRercmfafecintervalo = DateTime.MinValue; //Intervalo de quiebre (dd/mm/yyyy hh:mm).
                            decimal dRercmfavalintervalo = 0; //Valor del Factor de Ajuste al Cumplimiento de Energía Adjudicada para el intervalo quiebre (dd/mm/yyyy hh:mm)
                            if (dRerCenFecha < dFechaCorte)
                            {
                                //El valor esta en la data historica
                                if (listaCalculoMensualAnterior != null && listaCalculoMensualAnterior.Count > 0)
                                {
                                    RerCalculoMensualDTO dtoRerCalculoMensualDTOAnterior = listaCalculoMensualAnterior.Where(x => x.Rerpprmes == dtoPeriodo.MesCodi &&
                                x.Equicodi == dtoCentral.Equicodi).FirstOrDefault();
                                    if (dtoRerCalculoMensualDTOAnterior != null && dtoRerCalculoMensualDTOAnterior.Rercmfatipintervalo == "0")
                                    {
                                        iRercmfatipintervalo = 0; //Para el periodo, todos sus intervalos de su Factor de Ajuste es 0.
                                    }
                                    else if (dtoRerCalculoMensualDTOAnterior != null && dtoRerCalculoMensualDTOAnterior.Rercmfatipintervalo == "2")
                                    {
                                        iRercmfatipintervalo = 2;
                                        /* Para el periodo, existe un intervalo de quiebre. Por lo tanto:
                                         * Dicho intervalo de quiebre es RERCMFAFECINTERVALO y su Factor de Ajuste respectivo es RERCMFAVALINTERVALO.  
                                         * Para los intervalos anteriores al intervalo de quiebre su de Factor de Ajuste es 1.
                                         * Para los intervalos posteriores al intervalo de quiebre su de Factor de Ajuste es 0." */
                                        dRercmfafecintervalo = (DateTime)dtoRerCalculoMensualDTOAnterior.Rercmfafecintervalo;
                                        dRercmfavalintervalo = (decimal)dtoRerCalculoMensualDTOAnterior.Rercmfavalintervalo;
                                    }
                                }
                            }
                            else
                            {
                                //El valor esta en el presente Año tarifario, cuando se calcule la Prima RER
                                RerCentralUnicaDTO dtoFAJCentral = listaFAJAnioActual.Where(x => x.Equicodi == dtoCentral.Equicodi).FirstOrDefault();
                                RerCalculoMensualDTO dtoFAJCentralMensual = dtoFAJCentral.ListCalculoMensual.Where(y => y.Rerpprmes == dtoPeriodo.MesCodi).FirstOrDefault();
                                if (dtoFAJCentralMensual == null || dtoFAJCentralMensual.Rercmfatipintervalo == "0")
                                {
                                    iRercmfatipintervalo = 0; //Para el periodo, todos sus intervalos de su Factor de Ajuste es 0.
                                }
                                else if (dtoFAJCentralMensual == null || dtoFAJCentralMensual.Rercmfatipintervalo == "2")
                                {
                                    iRercmfatipintervalo = 2;
                                    /* Para el periodo, existe un intervalo de quiebre. Por lo tanto:
                                     * Dicho intervalo de quiebre es RERCMFAFECINTERVALO y su Factor de Ajuste respectivo es RERCMFAVALINTERVALO.  
                                     * Para los intervalos anteriores al intervalo de quiebre su de Factor de Ajuste es 1.
                                     * Para los intervalos posteriores al intervalo de quiebre su de Factor de Ajuste es 0." */
                                    dRercmfafecintervalo = (DateTime)dtoFAJCentralMensual.Rercmfafecintervalo;
                                    dRercmfavalintervalo = (decimal)dtoFAJCentralMensual.Rercmfavalintervalo;
                                }
                            }

                            //Traemos la lista del Valor de Energia de Entrega [El numero de registros es el numero de dias en el mes].
                            //ASSETEC 20240902: Ajustes a la formula de importación de Saldos VTEA, por lo tanto se cambio a ListarEnergiaEntregaDetalle en vez de ListarValorEnergiaEntregaDetalle que contenia la aplicación del CMg
                            List<ValorTransferenciaDTO> listaEntrega = servicioValorTransferencia.ListarEnergiaEntregaDetalle(iPeriCodi, iRecaCodi, dtoCentral.Codentcodi);
                            List<ValorTransferenciaDTO> listaEntregaAnterior = servicioValorTransferencia.ListarEnergiaEntregaDetalle(iPeriCodi, iRecaCodiAnterior, dtoCentral.Codentcodi);

                            //Traemos la lista de Valor de Energia de Retiros [sumando los retiros y el numero de registros es el numero de dias en el mes]. 
                            //ASSETEC 20240902: Ajustes a la formula de importación de Saldos VTEA, por lo tanto se cambio a ListarEnergiaRetiroDetalle en vez de ListarValoRetiroEntregaDetalle que contenia la aplicación del CMg
                            List<ValorTransferenciaDTO> listaRetiro = new List<ValorTransferenciaDTO>();
                            List<ValorTransferenciaDTO> listaRetiroAnterior = new List<ValorTransferenciaDTO>();
                            string listaCodigosRetiro = "";

                            //Para el dtoPeriodo: perianio / perimes hay una lista de versiones que contienen la relación con la lista de codigos de retiro.
                            List<RerParametroPrimaDTO> listaVersiones = GetByCriteriaRerParametroPrima(dtoPeriodo.AnioCodi.ToString(), dtoPeriodo.MesCodi);
                            foreach (RerParametroPrimaDTO dtoVersion in listaVersiones)
                            {
                                if (dtoVersion.Rerpprcodi > iRerPPrCodi)
                                    continue; //No debe coger parametro prima posteriores al actual
                                //Recorremos hasta encontrar una versión con info de relaciones de cdigos de retiro
                                listaCodigosRetiro = ListaCodigoRetiroByEquipo(dtoVersion.Rerpprcodi, dtoCentral.Equicodi);
                                if (listaCodigosRetiro != "")
                                    break; //Ya lo encontre, salgo del foreach
                            }
                            if (listaCodigosRetiro != "")
                            {
                                listaRetiro = servicioValorTransferencia.ListarEnergiaRetiroDetalle(iPeriCodi, iRecaCodi, listaCodigosRetiro);
                                listaRetiroAnterior = servicioValorTransferencia.ListarEnergiaRetiroDetalle(iPeriCodi, iRecaCodiAnterior, listaCodigosRetiro);
                            }

                            //ASSETEC 20240902: Traemos los Costos Marginales de esta revisión y del anterior
                            List<CostoMarginalDTO> listaCostoMarginal = servicioCostoMarginal.ListCostoMarginalByCodigoEntrega(iPeriCodi, iRecaCodi, dtoCentral.Codentcodi);
                            List<CostoMarginalDTO> listaCostoMarginalAnterior = servicioCostoMarginal.ListCostoMarginalByCodigoEntrega(iPeriCodi, iRecaCodiAnterior, dtoCentral.Codentcodi);

                            //Aplicamos la resta en cada día y almacenamos en la Tabla Insumo
                            for (int iDia = 0; iDia < iUltimoDia; iDia++)
                            {
                                //Para cada dia del mes
                                DateTime dRerinefecdia = dRerCenFecha.AddDays(iDia);
                                RerInsumoVteaDTO dtoInsumoVtea = new RerInsumoVteaDTO
                                {
                                    Rerinscodi = iRerInsCodi,
                                    Rerpprcodi = iRerPPrCodi,
                                    Emprcodi = dtoCentral.Emprcodi,
                                    Equicodi = dtoCentral.Equicodi,
                                    Rerinefecdia = dRerinefecdia,
                                    Pericodi = iPeriCodi,
                                    Recacodi = iRecaCodi,
                                    Rerinediausucreacion = sUser,
                                    Rerinediafeccreacion = DateTime.Now
                                };
                                dtoInsumoVtea.Rerinediatotal = 0;
                                int j = 0;
                                while (j < 96)
                                {
                                    decimal dFAJ = 1;
                                    if (iRercmfatipintervalo == 0)
                                        dFAJ = 0;
                                    if (iRercmfatipintervalo == 2)
                                    {
                                        if (dRercmfafecintervalo != DateTime.MinValue && dRerinefecdia.AddMinutes(j * 15) > dRercmfafecintervalo)
                                        {
                                            dFAJ = 0;
                                        }
                                        else if (dRercmfafecintervalo != DateTime.MinValue && dRerinefecdia.AddMinutes(j * 15) == dRercmfafecintervalo)
                                        {
                                            dFAJ = dRercmfavalintervalo;
                                        }
                                    }

                                    //Revisión actual
                                    decimal dEntrega = 0;
                                    if (listaEntrega != null && listaEntrega.Count > 0 && listaEntrega.ElementAtOrDefault(iDia) != null)
                                    {
                                        dEntrega = (decimal)listaEntrega[iDia].GetType().GetProperty($"VT{j + 1}").GetValue(listaEntrega[iDia]);
                                    }
                                    decimal dRetiro = 0;
                                    if (listaRetiro != null && listaRetiro.Count > 0 && listaRetiro.ElementAtOrDefault(iDia) != null)
                                    {
                                        dRetiro = (decimal)listaRetiro[iDia].GetType().GetProperty($"VT{j + 1}").GetValue(listaRetiro[iDia]);
                                    }
                                    decimal Rerinddiah = dEntrega - dRetiro;
                                    //ASSETEC 20240902
                                    if (Rerinddiah < 0)
                                        Rerinddiah = 0;
                                    else
                                    {
                                        decimal dCostoMarginal = 0;
                                        if (listaCostoMarginal != null && listaCostoMarginal.Count > 0 && listaCostoMarginal.ElementAtOrDefault(iDia) != null)
                                        {
                                            dCostoMarginal = (decimal)listaCostoMarginal[iDia].GetType().GetProperty($"CosMar{j + 1}").GetValue(listaCostoMarginal[iDia]);
                                        }
                                        Rerinddiah = Rerinddiah * dCostoMarginal;
                                    }
                                    //FIN
                                    /**************************************************************************************************************************/
                                    //Revisión anterior
                                    dEntrega = 0;
                                    if (listaEntregaAnterior != null && listaEntregaAnterior.Count > 0 && listaEntregaAnterior.ElementAtOrDefault(iDia) != null)
                                    {
                                        dEntrega = (decimal)listaEntregaAnterior[iDia].GetType().GetProperty($"VT{j + 1}").GetValue(listaEntregaAnterior[iDia]);
                                    }
                                    dRetiro = 0;
                                    if (listaRetiroAnterior != null && listaRetiroAnterior.Count > 0 && listaRetiroAnterior.ElementAtOrDefault(iDia) != null)
                                    {
                                        dRetiro = (decimal)listaRetiroAnterior[iDia].GetType().GetProperty($"VT{j + 1}").GetValue(listaRetiroAnterior[iDia]);
                                    }
                                    //Diferencia de Energia
                                    decimal RerinddiahAnterior = dEntrega - dRetiro;
                                    //ASSETEC 20240902
                                    if (RerinddiahAnterior < 0) 
                                        RerinddiahAnterior = 0;
                                    else
                                    {
                                        decimal dCostoMarginal = 0;
                                        if (listaCostoMarginalAnterior != null && listaCostoMarginalAnterior.Count > 0 && listaCostoMarginalAnterior.ElementAtOrDefault(iDia) != null)
                                        {
                                            dCostoMarginal = (decimal)listaCostoMarginalAnterior[iDia].GetType().GetProperty($"CosMar{j + 1}").GetValue(listaCostoMarginalAnterior[iDia]);
                                        }
                                        RerinddiahAnterior = RerinddiahAnterior * dCostoMarginal;
                                    }
                                    //FIN

                                    //Aplicamos la formula dfe 15 minutos
                                    decimal dSaldo15 = dFAJ * (Rerinddiah - RerinddiahAnterior);
                                    //Acumulamos el saldo del dia
                                    dtoInsumoVtea.Rerinediatotal += dSaldo15;
                                    //Seteamos el valor
                                    dtoInsumoVtea.GetType().GetProperty($"Rerinediah{(j + 1)}").SetValue(dtoInsumoVtea, dSaldo15);
                                    j++;
                                }

                                //ITERATIVA
                                //int iId = SaveRerInsumoVtea(dtoInsumoVtea);

                                //APILAMOS PARA EL BULKINSERT
                                dtoInsumoVtea.Rerinecodi = iRerInECodi++;
                                listaRerInsumoVtea.Add(dtoInsumoVtea);
                            }
                        }

                    }
                    //Subimos todos los registros de RER_INSUMO_VTEA
                    if (listaRerInsumoVtea.Count > 0)
                    {
                        FactoryTransferencia.GetRerInsumoVteaRepository().BulkInsertRerInsumoVtea(listaRerInsumoVtea);
                    }

                    //Tomando como referencia lo almacenado en Insumo VTEA, procedemos a guardar la información en Insumo Mes
                    //Variables para Insertar en Insumo Dia
                    DateTime dRerParametroPrimaFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes del Parametro Prima
                    int iUltimoDiaParametroPrima = dRerParametroPrimaFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31

                    //Listamos las Centrales vigentes para este mes de Parametro Prima 
                    List<RerCentralDTO> listaCentralMes = ListCentralByFecha(dRerParametroPrimaFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                    foreach (RerCentralDTO dtoCentral in listaCentralMes)
                    {
                        //Por cada Central - EQUICODI, traemos la suma de los saldos entre las revisiones de otros periodos que se aplican en el mes
                        decimal dTotalMes = ObtenerSaldoVteaByInsumoVTEA(iRerPPrCodi, dtoCentral.Emprcodi, dtoCentral.Equicodi);

                        //Insertamos el registro RER_INSUMO_MES de la Central
                        int iRerInmMesCodi = InsertarInsumoMes(iRerInsCodi, iRerPPrCodi, dtoCentral, iAnio, iRerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser, dTotalMes);
                    }
                    sLog = sLog + " Ejecutado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                }
                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                string innerExceptionMessage = (e.InnerException != null) ? (" " + e.InnerException.Message) : "";
                sResultado = e.Message + innerExceptionMessage;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }

        /// <summary>
        /// Proceso que se encarga de ImportarSaldosVTP, por un año tarifario y una versión
        /// </summary>
        /// <param name="listaParametroPrima">Lista de Parametros Prima (meses) de un Año Tarifario y Versión</param>
        /// <param name="iRerAVCodi">Identificador de la tabla RER_ANIOVERSION. Autoincremental</param>
        /// <param name="iRerPPrCodi">identificador de la tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="sVersion">Versión del año tarifario: [0]Anual; [1]Trimestre 1; [2]Trimestre 2; [3]Trimestre 3, [4]Trimestre 4; [5]iquidacion</param>
        /// <param name="iRerAVAnioTarif">Año del parámetro de prima RER (2022: significa Año Tarifario Mayo 2022 - Abril 2023)</param>
        /// <param name="sUser">Usuario</param>
        /// <returns>Mensaje con el resultado del Proceso</returns>
        public string ImportarSaldosVTP(List<RerParametroPrimaDTO> listaParametroPrima, int iRerAVCodi, int iRerPPrCodi, string sVersion, int iRerAVAnioTarif, string sUser)
        {
            string sResultado = "";
            string sTipoInsumo = "7"; //Saldos VTP
            string sLog = "Se inició la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'. <br>";
            int iRerInsCodi = 0;

            try
            {
                string sTipoProceso = "A"; //Automatico
                string sRerInmTipInformacion = "E"; //Información ejecutada
                string sRerInmDetalle = "N"; //No tiene detalle
                string sMensaje = "Se completó la importación automática del insumo 'Saldo VTP'. Para ver el resultado de esta importación, ejecute su ícono respectivo de la columna 'Log'.";

                //Traemos la lista de todos los FAJ's del AñoTarifarioAnterior
                List<RerCalculoMensualDTO> listaCalculoMensualAnterior = new List<RerCalculoMensualDTO>();
                int iReravaniotarifAnterior = iRerAVAnioTarif - 1;
                //Traemos la lista de todas las versiones de ese año, son 6(versiones) x 12(meses) = 72 registros
                List<RerAnioVersionDTO> listaAnioVersionAnterior = ListRerAnioVersionesByAnio(iReravaniotarifAnterior);
                foreach (RerAnioVersionDTO AnioVersionAnterior in listaAnioVersionAnterior)
                {
                    //Para cada grupo de 12 meses, dentro de una versión, analizamos quien esta completaen RER_CALCULO_MENSUAL
                    listaCalculoMensualAnterior = FactoryTransferencia.GetRerCalculoMensualRepository().GetByAnioTarifario(AnioVersionAnterior.Reravcodi);
                    if (listaCalculoMensualAnterior != null && listaCalculoMensualAnterior.Count > 0)
                    {
                        //Lista con los FAJ de cada Central RER en la versión Reravcodi
                        break;
                    }
                }

                //Para obtener el FAJ del presente Año Tarifario
                List<RerCentralUnicaDTO> listaFAJAnioActual = ObtenerFAJ(iRerAVAnioTarif, sVersion);
                //Seteamos la Fecha (Mes) de donde proviene la información
                DateTime dFechaCorte = new DateTime(iRerAVAnioTarif, 5, 1, 0, 0, 0); // Retorna el primer dia de Mayo del Año Tariario

                //Eliminamos toda la data del año tarifario
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    //Para cada mes Eliminamos la data en la tabla RER_INSUMO_MES, RER_INSUMO_DIA
                    int iRerpprmes = dtoParametroPrima.Rerpprmes; //Mes a eliminar
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes

                    //Eliminamos Insumo VTP -> Todo lo que esta relacionado con el Año/Mes de ParametroPrima
                    sResultado = DeleteRerInsumoVTPByParametroPrimaAndMes(iRerPPrCodi, iRerpprmes);
                    if (sResultado != "")
                        return sResultado;

                    //Elimanos Insumo Mes
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(iRerPPrCodi, iRerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        return sResultado;
                }

                //Insertamos el procedimiento en la tabla RER_INSUMO mes or mes
                iRerInsCodi = InsertarInsumo(iRerAVCodi, sTipoInsumo, sTipoProceso, sLog, sUser);

                //Para cada Mes del año tariario
                foreach (RerParametroPrimaDTO dtoParametroPrima in listaParametroPrima)
                {
                    int iRerpprmes = dtoParametroPrima.Rerpprmes;
                    iRerPPrCodi = dtoParametroPrima.Rerpprcodi; //Identificador que permite cambiar de mes

                    //Consultamos por el periodo correspondiente relacionado al año tarifario
                    int iAnio = iRerpprmes < 5 ? iRerAVAnioTarif + 1 : iRerAVAnioTarif;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente

                    //Traemos la lista de Periodos destino que se aplican en este mes: iRerPPrCodi
                    List<RerParametroRevisionDTO> listRerParametroRevision = ListRerParametroRevisionesByRerpprcodiByTipo(iRerPPrCodi, "VTP");

                    //Por cada elemento de la lista, vamos a aplicar la diferencia de la Revisión[N+1] - Revisión[N] (este ultimo a lo mas es la mensual)
                    //El Resultado = Saldo, lo almacenamos en Insumo VTEA
                    foreach (RerParametroRevisionDTO dtoRerParametroRevision in listRerParametroRevision)
                    {
                        int iPeriCodi = dtoRerParametroRevision.Pericodi;
                        int iRecaCodi = dtoRerParametroRevision.Recacodi;
                        int iRecaCodiAnterior = iRecaCodi - 1; // De la operación, a lo menos podria salir 1: Mensual

                        //Buscamos la información relacionada al periodo
                        PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(iPeriCodi);
                        //Seteamos la Fecha (Mes) de donde proviene la información
                        DateTime dRerCenFecha = new DateTime(dtoPeriodo.AnioCodi, dtoPeriodo.MesCodi, 1, 0, 0, 0); // Primer dia del mes
                        int iUltimoDia = dRerCenFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31

                        //Listamos las Centrales - EQUICODI, que estan Vigentes en el Mes
                        List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                        foreach (RerCentralDTO dtoCentral in listaCentral)
                        {
                            //Traemos el valor del FAJ para la Central en el ParametroPrima(MES)
                            int iRercmfatipintervalo = 1; //Para el periodo, todos sus intervalos de su Factor de Ajuste es 1.
                            DateTime dRercmfafecintervalo = DateTime.MinValue; //Intervalo de quiebre (dd/mm/yyyy hh:mm).
                            decimal dRercmfavalintervalo = 0; //Valor del Factor de Ajuste al Cumplimiento de Energía Adjudicada para el intervalo quiebre (dd/mm/yyyy hh:mm)
                            if (dRerCenFecha < dFechaCorte)
                            {
                                //El valor esta en la data historica
                                if (listaCalculoMensualAnterior != null && listaCalculoMensualAnterior.Count > 0)
                                {
                                    RerCalculoMensualDTO dtoRerCalculoMensualDTOAnterior = listaCalculoMensualAnterior.Where(x => x.Rerpprmes == dtoPeriodo.MesCodi &&
                                    x.Equicodi == dtoCentral.Equicodi).FirstOrDefault();
                                    if (dtoRerCalculoMensualDTOAnterior != null && dtoRerCalculoMensualDTOAnterior.Rercmfatipintervalo == "0")
                                    {
                                        iRercmfatipintervalo = 0; //Para el periodo, todos sus intervalos de su Factor de Ajuste es 0.
                                    }
                                    else if (dtoRerCalculoMensualDTOAnterior != null && dtoRerCalculoMensualDTOAnterior.Rercmfatipintervalo == "2")
                                    {
                                        iRercmfatipintervalo = 2;
                                        /* Para el periodo, existe un intervalo de quiebre. Por lo tanto:
                                         * Dicho intervalo de quiebre es RERCMFAFECINTERVALO y su Factor de Ajuste respectivo es RERCMFAVALINTERVALO.  
                                         * Para los intervalos anteriores al intervalo de quiebre su de Factor de Ajuste es 1.
                                         * Para los intervalos posteriores al intervalo de quiebre su de Factor de Ajuste es 0." */
                                        dRercmfafecintervalo = (DateTime)dtoRerCalculoMensualDTOAnterior.Rercmfafecintervalo;
                                        dRercmfavalintervalo = (decimal)dtoRerCalculoMensualDTOAnterior.Rercmfavalintervalo;
                                    }
                                }
                            }
                            else
                            {
                                //El valor esta en el presente Año tarifario, cuando se calcule la Prima RER
                                RerCentralUnicaDTO dtoFAJCentral = listaFAJAnioActual.Where(x => x.Equicodi == dtoCentral.Equicodi).FirstOrDefault();
                                RerCalculoMensualDTO dtoFAJCentralMensual = dtoFAJCentral.ListCalculoMensual.Where(y => y.Rerpprmes == dtoPeriodo.MesCodi).FirstOrDefault();
                                if (dtoFAJCentralMensual == null || dtoFAJCentralMensual.Rercmfatipintervalo == "0")
                                {
                                    iRercmfatipintervalo = 0; //Para el periodo, todos sus intervalos de su Factor de Ajuste es 0.
                                }
                                else if (dtoFAJCentralMensual.Rercmfatipintervalo == "2")
                                {
                                    iRercmfatipintervalo = 2;
                                    /* Para el periodo, existe un intervalo de quiebre. Por lo tanto:
                                     * Dicho intervalo de quiebre es RERCMFAFECINTERVALO y su Factor de Ajuste respectivo es RERCMFAVALINTERVALO.  
                                     * Para los intervalos anteriores al intervalo de quiebre su de Factor de Ajuste es 1.
                                     * Para los intervalos posteriores al intervalo de quiebre su de Factor de Ajuste es 0." */
                                    dRercmfafecintervalo = (DateTime)dtoFAJCentralMensual.Rercmfafecintervalo;
                                    dRercmfavalintervalo = (decimal)dtoFAJCentralMensual.Rercmfavalintervalo;
                                }
                            }

                            //Obtenemos los Ingresos por Potencia x Central
                            VtpIngresoPotUnidPromdDTO dtoIngresoPotUnidPromd = servicioTransfPotencia.GetByVtpIngresoPotUnidPromdCentral(iPeriCodi, iRecaCodi, dtoCentral.Equicodi);
                            //Devuelve el Objeto con el atributo Inpuprimportepromd
                            if (dtoIngresoPotUnidPromd.Inpuprimportepromd == 0)
                            {
                                dtoIngresoPotUnidPromd = servicioTransfPotencia.GetByVtpIngresoPotUnidPromdCentralSumUnidades(iPeriCodi, iRecaCodi, dtoCentral.Equicodi);
                            }
                            //Revisión anterior
                            VtpIngresoPotUnidPromdDTO dtoIngresoPotUnidPromdAnterior = servicioTransfPotencia.GetByVtpIngresoPotUnidPromdCentral(iPeriCodi, iRecaCodiAnterior, dtoCentral.Equicodi);
                            if (dtoIngresoPotUnidPromdAnterior.Inpuprimportepromd == 0)
                            {
                                dtoIngresoPotUnidPromdAnterior = servicioTransfPotencia.GetByVtpIngresoPotUnidPromdCentralSumUnidades(iPeriCodi, iRecaCodiAnterior, dtoCentral.Equicodi);
                            }

                            decimal Nt = iUltimoDia * 96; //# de dias * 96 intervalos dentro de un dia
                            decimal dSUM_FAJ_entre_Nt = 1;
                            if (iRercmfatipintervalo == 0)
                                dSUM_FAJ_entre_Nt = 0;
                            if (iRercmfatipintervalo == 2)
                            {
                                decimal dFAJ = 0;
                                for (int iDia = 0; iDia < iUltimoDia; iDia++)
                                {
                                    DateTime dRerinefecdia = dRerCenFecha.AddDays(iDia);
                                    int j = 0;
                                    while (j < 96)
                                    {
                                        if (dRercmfafecintervalo != DateTime.MinValue && dRerinefecdia.AddMinutes(j * 15) < dRercmfafecintervalo)
                                        {
                                            dFAJ += 1;
                                        }
                                        else if (dRercmfafecintervalo != DateTime.MinValue && dRerinefecdia.AddMinutes(j * 15) == dRercmfafecintervalo)
                                        {
                                            dFAJ += dRercmfavalintervalo;
                                        }
                                        else
                                            break;
                                        j++;
                                    }
                                }
                                dSUM_FAJ_entre_Nt = dFAJ / Nt;
                            };

                            decimal dSaldo = dSUM_FAJ_entre_Nt * (dtoIngresoPotUnidPromd.Inpuprimportepromd - dtoIngresoPotUnidPromdAnterior.Inpuprimportepromd);

                            //Insertamos la revisión en RER_INSUMO_VTP dIP_rp
                            RerInsumoVtpDTO dtoRerInsumoVTP = new RerInsumoVtpDTO();
                            //dtoRerInsumoVTP.Rerinpcodi PK
                            dtoRerInsumoVTP.Rerinscodi = iRerInsCodi;
                            dtoRerInsumoVTP.Rerpprcodi = iRerPPrCodi;
                            dtoRerInsumoVTP.Emprcodi = dtoCentral.Emprcodi;
                            dtoRerInsumoVTP.Equicodi = dtoCentral.Equicodi;
                            dtoRerInsumoVTP.Rerinpanio = iAnio;
                            dtoRerInsumoVTP.Rerinpmes = iRerpprmes;
                            dtoRerInsumoVTP.Pericodi = iPeriCodi;
                            dtoRerInsumoVTP.Recpotcodi = iRecaCodi;
                            dtoRerInsumoVTP.Rerinpmestotal = dSaldo;
                            dtoRerInsumoVTP.Rerinpmesusucreacion = sUser;
                            dtoRerInsumoVTP.Rerinpmesfeccreacion = DateTime.Now;
                            SaveRerInsumoVtp(dtoRerInsumoVTP);
                        }
                    }

                    //Tomando como referencia lo almacenado en Insumo VTEA, procedemos a guardar la información en Insumo Mes
                    //Variables para Insertar en Insumo Dia
                    DateTime dRerParametroPrimaFecha = new DateTime(iAnio, iRerpprmes, 1, 0, 0, 0); // Primer dia del mes del Parametro Prima
                    int iUltimoDiaParametroPrima = dRerParametroPrimaFecha.AddMonths(1).AddDays(-1).Day; //Devuelve 28/29/30/31

                    //Listamos las Centrales vigentes para este mes de Parametro Prima 
                    List<RerCentralDTO> listaCentralMes = ListCentralByFecha(dRerParametroPrimaFecha); //Solo Centrales: [EQUICODI y CODENTRCODI]
                    foreach (RerCentralDTO dtoCentral in listaCentralMes)
                    {
                        //Por cada Central - EQUICODI, traemos la suma de los saldos entre las revisiones de otros periodos que se aplican en el mes
                        decimal dTotalMes = ObtenerSaldoVtpByInsumoVTP(iRerPPrCodi, dtoCentral.Emprcodi, dtoCentral.Equicodi);

                        //Insertamos el registro RER_INSUMO_MES de la Central
                        int iRerInmMesCodi = InsertarInsumoMes(iRerInsCodi, iRerPPrCodi, dtoCentral, iAnio, iRerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser, dTotalMes);
                    }

                    sLog = sLog + " Ejecutado: [" + dtoParametroPrima.Rerpprmesaniodesc + "] <br>";
                }
                sResultado = sMensaje;
            }
            catch (Exception e)
            {
                string innerExceptionMessage = (e.InnerException != null) ? (" " + e.InnerException.Message) : "";
                sResultado = e.Message + innerExceptionMessage;
                sLog = sLog + "<br>Ocurrio el siguiente error:<br>" + e.Message + "<br>";
                sLog = sLog + "Intentelo más tarde. En caso persista el error, comunicarse con su administrador. <br> <br>";
            }
            finally
            {
                try
                {
                    #region Actualizar un LOG en RER_INSUMO
                    if (iRerInsCodi > 0)
                    {
                        RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                        sLog = sLog + "Se finalizó la importación automática del insumo '" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "'.";
                        dtoInsumo.Rerinslog = sLog;
                        UpdateRerInsumo(dtoInsumo);
                    }
                    #endregion
                }
                catch (Exception e2)
                {
                    sResultado = e2.Message;
                }
            }

            return sResultado;
        }
        #endregion

        #region Exportar Plantilla de insumos

        #endregion

        #endregion

        #region Métodos de la tabla RER_INSUMO_DIA
        public int SaveRerInsumoDia(RerInsumoDiaDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetRerInsumoDiaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        public void UpdateRerInsumoDia(RerInsumoDiaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerInsumoDiaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Proceso que se encarga de Eliminar los registros de RER_INSUMO_DIA
        /// </summary>
        /// <param name="iRerpprcodi">Identificador de la Tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerpprmes">Mes a eliminar</param>
        /// <param name="sRerinmtipresultado">Tipo de insumo donde: 
        /// 1 = Inyección Neta c/15 minutos; 
        /// 2 = Costo Marginal c/15 minutos;  
        /// 3 = Ingresos por Potencia; 
        /// 4 = Ingresos por Cargo Prima RER;  
        /// 5 = Energía Dejada de Inyectar (EDI) c/15 minutos; 
        /// 6 = Saldos VTEA c/15 minutos;
        /// 7 = Saldos VTP</param>
        /// <returns></returns>
        public string DeleteRerInsumoDiaByParametroPrimaAndTipo(int iRerpprcodi, int iRerpprmes, string sRerinmtipresultado)
        {
            string sResultado = "";
            try
            {
                FactoryTransferencia.GetRerInsumoDiaRepository().DeleteByParametroPrimaAndTipo(iRerpprcodi, iRerpprmes, sRerinmtipresultado);
            }
            catch (Exception ex)
            {
                sResultado = ex.Message;
            }
            return sResultado;
        }

        public void ResetearHaCero(RerInsumoDiaDTO dtoInsumoDia)
        {
            for (int i = 1; i <= 96; i++)
            {
                var propertyName = $"Rerinddiah{i}";
                var property = dtoInsumoDia.GetType().GetProperty(propertyName);

                if (property != null && (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?)))
                {
                    property.SetValue(dtoInsumoDia, 0m);
                }
            }
        }
        #endregion

        #region Métodos de la tabla RER_INSUMO_MES
        public int SaveRerInsumoMes(RerInsumoMesDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetRerInsumoMesRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        public void UpdateRerInsumoMes(RerInsumoMesDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerInsumoMesRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerInsumoMesDTO GetByIdRerInsumoMes(int Rerinmmescodi)
        {
            return FactoryTransferencia.GetRerInsumoMesRepository().GetById(Rerinmmescodi);
        }

        public List<RerInsumoMesDTO> ListRerInsumoMeses()
        {
            return FactoryTransferencia.GetRerInsumoMesRepository().List();
        }

        /// <summary>
        /// Proceso que se encarga de Eliminar los registros de RER_INSUMO_MES
        /// </summary>
        /// <param name="iRerpprcodi">Identificador de la Tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerpprmes">Mes a eliminar</param>
        /// <param name="sRerindtipresultado">Tipo de insumo donde: 
        /// 1 = Inyección Neta c/15 minutos; 
        /// 2 = Costo Marginal c/15 minutos;  
        /// 3 = Ingresos por Potencia; 
        /// 4 = Ingresos por Cargo Prima RER;  
        /// 5 = Energía Dejada de Inyectar (EDI) c/15 minutos; 
        /// 6 = Saldos VTEA c/15 minutos;
        /// 7 = Saldos VTP</param>
        /// <returns></returns>
        public string DeleteRerInsumoMesByParametroPrimaAndTipo(int iRerpprcodi, int iRerpprmes, string sRerindtipresultado)
        {
            string sResultado = "";
            try
            {
                FactoryTransferencia.GetRerInsumoMesRepository().DeleteByParametroPrimaAndTipo(iRerpprcodi, iRerpprmes, sRerindtipresultado);
            }
            catch (Exception ex)
            {
                sResultado = ex.Message;
            }
            return sResultado;
        }
        #endregion

        #region Métodos de la tabla RER_ANIOVERSION
        public int SaveRerAnioVersion(RerAnioVersionDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerAnioVersionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRerAnioVersion(RerAnioVersionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRerAnioVersionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public RerAnioVersionDTO GetByIdRerAnioVersion(int rerAnioVersionId)
        {
            return FactoryTransferencia.GetRerAnioVersionRepository().GetById(rerAnioVersionId);
        }

        public List<RerAnioVersionDTO> ListRerAnioVersiones()
        {
            return FactoryTransferencia.GetRerAnioVersionRepository().List();
        }

        /// <summary>
        /// Obtiene los Años Tarifarios Factorizados, es decir:
        /// Considera los Años Tarifarios mayores al Año Base (2021)
        /// Realizar un GroupBy por AñoVersion, y escoge el primer registro de cada GroupBy
        /// </summary>
        /// <returns></returns>
        public List<RerAnioVersionDTO> ListRerAnioVersionesFactorizado()
        {
            return FactoryTransferencia.GetRerAnioVersionRepository().List().Where(x => x.Reravaniotarif > ConstantesPrimasRER.anioBase).GroupBy(x => x.Reravaniotarif).Select(g => g.First()).OrderByDescending(x => x.Reravaniotarif).ToList();
        }

        /// <summary>
        /// Obtiene el registro AñoVersión, a partir de un año tarifario y una versión
        /// </summary>
        /// <param name="iRerAVAnioTarif">Año tarifario</param>
        /// <param name="iRerAVVersion">Versión</param>
        /// <returns>RerAnioVersionDTO</returns>
        public RerAnioVersionDTO GetRerAnioVersionByAnioVersion(int iRerAVAnioTarif, int iRerAVVersion)
        {
            return FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(iRerAVAnioTarif, iRerAVVersion.ToString());
        }

        /// <summary>
        /// Lista los registro AñoVersión, a partir de un año tarifario
        /// </summary>
        /// <param name="iRerAVAnioTarif">Año tarifario</param>
        /// <returns>List<RerAnioVersionDTO></returns>
        public List<RerAnioVersionDTO> ListRerAnioVersionesByAnio(int iRerAVAnioTarif)
        {
            return FactoryTransferencia.GetRerAnioVersionRepository().ListRerAnioVersionesByAnio(iRerAVAnioTarif);
        }
        #endregion

        #region CU21 - Importar Total Insumos

        /// <summary>
        /// Almacena en la DB los registros de los archivos excel importados segun el insumo y los meses seleccionados
        /// </summary>
        /// <param name="iAnioTarifario">Año tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses que se seleccionaron para realizar la importación</param>
        /// <param name="sUser">Usuario que realiza la importación</param>
        /// <returns></returns>
        public void procesarArchivosInsumo(int iAnioTarifario, string sVersion, string sTipoInsumo, int[] iMeses, string sUser)
        {
            string sMensajeError = "";
            try {
                #region Validar que se seleccionaron meses
                if (iMeses == null) {
                    sMensajeError = "Por favor, seleccione un mes para guardar registros en la Base de Datos";
                    throw new Exception(sMensajeError);
                }
                #endregion

                #region Procesar archivos seleccionados segun su sTipoInsumo
                switch (sTipoInsumo)
                {
                    case ConstantesPrimasRER.tipoResultadoInyeccionNeta:
                        //Inyección Neta 15 min
                        procesarArchivosInsumoX15Min(iAnioTarifario, sVersion, sTipoInsumo, iMeses, sUser);
                        break;
                    case ConstantesPrimasRER.tipoResultadoCostoMarginal:
                        //Costo Marginal c/15 minutos
                        procesarArchivosInsumoX15Min(iAnioTarifario, sVersion, sTipoInsumo, iMeses, sUser);
                        break;
                    case ConstantesPrimasRER.tipoResultadoIngresosPotencia:
                        //Ingresos por Potencia
                        procesarArchivosInsumoXMes(iAnioTarifario, sVersion, sTipoInsumo, iMeses, sUser);
                        break;
                    case ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER:
                        //Ingresos por Cargo Prima RER
                        procesarArchivosInsumoXMes(iAnioTarifario, sVersion, sTipoInsumo, iMeses, sUser);
                        break;
                    case ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar:
                        //Energía Dejada de Inyectar (EDI) c/15 minutos
                        procesarArchivosInsumoX15Min(iAnioTarifario, sVersion, sTipoInsumo, iMeses, sUser);
                        break;
                    default:
                        sMensajeError = "Ocurrio un error no previsto";
                        throw new Exception(sMensajeError);
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Almacena en la DB los registros (registros de un mes en intervalos de 15 minutos) de los archivos excel importados del insumo 1, 2 y 5.
        /// </summary>
        /// <param name="iAnioTarifario">Año tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses que se seleccionaron para realizar la importación</param>
        /// <param name="sUser">Usuario que realiza la importación</param>
        /// <returns></returns>
        public void procesarArchivosInsumoX15Min(int iAnioTarifario, string sVersion, string sTipoInsumo, int[] iMeses, string sUser)
        {
            try
            {
                string sError = "";
                #region Valido si existe el año tarifario
                RerAnioVersionDTO anioVersionDTO = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(iAnioTarifario, sVersion);
                bool existeAnioTarifario = (anioVersionDTO != null);
                if (!existeAnioTarifario)
                {
                    sError = "No existe el año tarifario seleccionado";
                    throw new Exception(sError);
                }
                #endregion

                #region Variables del método
                string sResultado = "";
                List<string> sCeldaErrores = new List<string>();
                List<string> sMesesNoImportados = new List<string>();

                //Para rer_insumo
                int iRerInsCodi = 0;
                string sTipoProceso = "M";
                string sLog = "Se inició la importación manual del insumo [" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "]. <br>";

                // Para rer_insumo_mes 
                int iRerpprmes = 0;
                string sRerInmTipInformacion = "M"; // Mejor información
                string sRerInmDetalle = "S";        // Si tiene detalle

                // Para rer_insumo_dia
                string sRerIndTipInformacion = "M";
                #endregion

                #region Valido que los meses seleccionado tengan un archivo excel importado
                string sNombreArchivo = "";
                string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

                foreach (int iMesAValidar in iMeses)
                {
                    int iAnio = iMesAValidar < 5 ? iAnioTarifario + 1 : iAnioTarifario;     // Si es Enero, Febrero, Marzo o Abril: Anio=AnioTarifario+1

                    sNombreArchivo = iAnio.ToString() + iMesAValidar.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;
                    if (!System.IO.File.Exists(path + sNombreArchivo))
                    {
                        sMesesNoImportados.Add("'" + ConstantesPrimasRER.mesesDesc[iMesAValidar - 1] + "'");
                    }
                }
                if (sMesesNoImportados.Count > 0)
                {
                    throw new Exception("El mes de [" + string.Join(", ", sMesesNoImportados) + "] no se importo previamente.");
                }
                #endregion

                #region Crea el RER_INSUMO
                //Asignamos el correlativo de la tabla RER_INSUMO_DIA
                int iRerIndDiaCodi = FactoryTransferencia.GetRerInsumoDiaRepository().GetMaxId();
                List<RerInsumoDiaDTO> listaRerInsumoDia = new List<RerInsumoDiaDTO>();

                iRerInsCodi = InsertarInsumo(anioVersionDTO.Reravcodi, sTipoInsumo, sTipoProceso, sLog, sUser);
                #endregion

                #region De los meses seleccionados, obtenemos sus respectivos archivos Excels
                foreach (int iMes in iMeses)
                {
                    int iAnio = iMes < 5 ? iAnioTarifario + 1 : iAnioTarifario;     // Si es Enero, Febrero, Marzo o Abril: Anio=AnioTarifario+1
                    decimal dTotalMes = 0;
                    decimal dTotalDia = 0;

                    #region Valida que exista el mes seleccionado
                    RerParametroPrimaDTO rerParametroPrimaDTO = ListRerParametroPrimas().Where(item => (item.Reravcodi == anioVersionDTO.Reravcodi) && (item.Rerpprmes == iMes)).FirstOrDefault();
                    bool existeParametroPrima = (rerParametroPrimaDTO != null);
                    if (!existeParametroPrima)
                    {
                        sError = "No existe el mes seleccionado";
                        throw new Exception(sError);
                    }
                    #endregion

                    #region Para cada mes, eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    sResultado = DeleteRerInsumoDiaByParametroPrimaAndTipo(rerParametroPrimaDTO.Rerpprcodi, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        throw new Exception(sResultado);
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(rerParametroPrimaDTO.Rerpprcodi, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        throw new Exception(sResultado);
                    #endregion

                    #region Obtenemos la cantidad de intervalos de 15 minutos que existen en el mes
                    DateTime dRerCenFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", iMes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    DateTime inicioMesSiguiente = dRerCenFecha.AddMonths(1);
                    TimeSpan intervalo = TimeSpan.FromMinutes(15);
                    int cantidadIntervalos15Min = (int)((inicioMesSiguiente - dRerCenFecha).TotalMinutes / intervalo.TotalMinutes);
                    #endregion

                    int cantidadDias = DateTime.DaysInMonth(iAnio, iMes);
                    int cantidadIntervalos = 0;
                    // Obtenemos las centrales
                    List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha);

                    //Traemos la primera hoja del archivo
                    sNombreArchivo = iAnio.ToString() + iMes.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;

                    DataSet ds = new DataSet();
                    ds = GeneraDataset(path + sNombreArchivo, 1);

                    int numColumnasXAnalizar = listaCentral.Count;
                    int numFilasXAnalizar = cantidadIntervalos15Min;

                    int numRows = ds.Tables[0].Rows.Count;
                    int numCols = ds.Tables[0].Columns.Count;

                    #region Validamos las fechas 
                    for (int f = 8; f < numFilasXAnalizar; f++)
                    {
                        string fechaExcel = ds.Tables[0].Rows[f][1].ToString();
                        string fechaEsperada = dRerCenFecha.AddMinutes(15 * (f - 7)).ToString("dd/MM/yyyy HH:mm");
                        if (fechaExcel != fechaEsperada)
                        {
                            throw new Exception("La fecha esperada no es la correcta. Por favor, no manipular las fechas de la columna B");
                        }
                    }
                    #endregion

                    #region Obtenemos el numero de columnas a analizar
                    numColumnasXAnalizar = 0;
                    for (int c = 2; c < numCols; c++)
                    {
                        string filaOculta = ds.Tables[0].Rows[6][c].ToString();
                        if (filaOculta != null && filaOculta != "" && filaOculta != "null")
                        {
                            numColumnasXAnalizar++;
                        }
                    }
                    #endregion

                    #region Validacion de los datos para rer_insumo_dia ingresados
                    for (int j = 2; j < numColumnasXAnalizar + 2; j++)        // Recorro las centrales (columnas)
                    {
                        dTotalMes = 0;

                        string nombreCodigoEntrega = ds.Tables[0].Rows[4][j].ToString().Trim();
                        string equinomb = ds.Tables[0].Rows[5][j].ToString().Trim();
                        int codiCodigoEntrega = int.Parse(ds.Tables[0].Rows[6][j].ToString().Trim());
                        int equicodi = int.Parse(ds.Tables[0].Rows[7][j].ToString().Trim());

                        #region Validamos que el equicodi y el codigo de entrega se encuentren en listaCentral
                        RerCentralDTO centralRERValida = listaCentral.Where(item => (item.Equicodi == equicodi) && (item.Codentcodi == codiCodigoEntrega)).ToList().FirstOrDefault();
                        bool central_is_valid = (centralRERValida != null);
                        if (!central_is_valid)
                        {
                            throw new Exception("El equipo o código de entrega no se encuentran vigentes en el mes seleleccionado.");
                        }
                        #endregion

                        #region Crea el rer_insumo_mes para una empresa-central
                        int iRerInsMesCodi = InsertarInsumoMes(iRerInsCodi, rerParametroPrimaDTO.Rerpprcodi, centralRERValida, iAnio, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                        #endregion

                        for (int iDia = 1; iDia <= cantidadDias; iDia++)
                        {
                            dTotalDia = 0;

                            //Crea un RER_INSUMO_DIA_DTO
                            RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO
                            {
                                Rerinmmescodi = iRerInsMesCodi,
                                Rerpprcodi = rerParametroPrimaDTO.Rerpprcodi,
                                Emprcodi = centralRERValida.Emprcodi,
                                Equicodi = centralRERValida.Equicodi,
                                Rerindtipresultado = sTipoInsumo,
                                Rerindtipinformacion = sRerIndTipInformacion,
                                Rerinddiatotal = 0,
                                Rerinddiafecdia = DateTime.ParseExact(string.Format("{0}/{1}/{2}", iDia.ToString("D2"), iMes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                                Rerinddiausucreacion = sUser,
                                Rerinddiafeccreacion = DateTime.Now
                            };

                            #region Almacena los valores de H de dtoInsumoDia
                            for (int i = 1; i <= 96; i++)
                            {
                                string sValorIntervalo = ds.Tables[0].Rows[(i + 7) + (iDia - 1)*96][j].ToString().Trim();//data[iCol][(diasMes - 1) * 96 + i + 7].Trim();
                                decimal dValorIntervalo = 0;

                                #region Validamos que el Valor total sea un valor decimal
                                if (sValorIntervalo == "null")
                                {
                                    dValorIntervalo = 0;
                                }
                                else if (!decimal.TryParse(sValorIntervalo, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out dValorIntervalo))
                                {
                                    sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 7) + ((iDia - 1) * 96).ToString());
                                }
                                else
                                {
                                    dValorIntervalo = decimal.Parse(dValorIntervalo.ToString("0.################"));
                                }
                                #endregion

                                #region Guardamos el valor en el Rerinddiah{i} correspondiente en RER_INSUMO_DIA_dto
                                string propertyName = $"Rerinddiah{i}";
                                var propertyInfo = typeof(RerInsumoDiaDTO).GetProperty(propertyName);

                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(dtoInsumoDia, dValorIntervalo);
                                }
                                #endregion

                                dTotalDia += dValorIntervalo;
                            }

                            dtoInsumoDia.Rerinddiatotal = dTotalDia;
                            #endregion

                            #region Almacena el rer_insumo_dia para una empresa-central
                            //APILAMOS PARA EL BULKINSERT
                            dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                            listaRerInsumoDia.Add(dtoInsumoDia);
                            #endregion

                            dTotalMes += dTotalDia;
                        }

                        #region Actualizamos el Rerinsmtotal en rer_insumo_mes
                        RerInsumoMesDTO insumoMes = GetByIdRerInsumoMes(iRerInsMesCodi);
                        insumoMes.Rerinmmestotal = dTotalMes;
                        UpdateRerInsumoMes(insumoMes);
                        #endregion
                        
                    }
                    #endregion

                    sLog = sLog + " El mes [" + iAnio.ToString("D2") + "." + ConstantesPrimasRER.mesesDesc[iMes - 1] + "] se cargo correctamente. <br>";
                }
                #endregion

                #region Subimos todos los registros de RER_INSUMO_DIA por BulkInsert
                if (listaRerInsumoDia.Count > 0)
                {
                    FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDia(listaRerInsumoDia);
                }
                #endregion

                #region Actualizar el log de RER_INSUMO
                RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                sLog = sLog + "Se finalizó la importación manual del insumo [" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "].";
                dtoInsumo.Rerinslog = sLog;
                UpdateRerInsumo(dtoInsumo);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Almacena en la DB los registros (registro equivalente al mes) de los archivos excel importados del insumo 3 y 4.
        /// </summary>
        /// <param name="iAnioTarifario">Año tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses que se seleccionaron para realizar la importación</param>
        /// <param name="sUser">Usuario que realiza la importación</param>
        /// <returns></returns>
        public void procesarArchivosInsumoXMes(int iAnioTarifario, string sVersion, string sTipoInsumo, int[] iMeses, string sUser)
        {
            try
            {
                string sError = "";

                #region Valido si existe el año tarifario
                RerAnioVersionDTO anioVersionDTO = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(iAnioTarifario, sVersion);
                bool existeAnioTarifario = (anioVersionDTO != null);
                if (!existeAnioTarifario)
                {
                    sError = "No existe el año tarifario seleccionado";
                    throw new Exception(sError);
                }
                #endregion

                #region Variables del método
                string sResultado = "";

                //Para rer_insumo
                int iRerInsCodi = 0;
                string sTipoProceso = "M";
                string sLog = "Se inició la importación manual del insumo [" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "]. <br>";

                // Para rer_insumo_mes 
                string sRerInmTipInformacion = "M"; // Mejor información
                string sRerInmDetalle = "N";        // No tiene detalle
                List<string> sCeldaErrores = new List<string>();
                #endregion

                #region Valido que los meses seleccionado tengan un archivo excel importado
                string sNombreArchivo = "";
                List<string> sMesesNoImportados = new List<string>();

                string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

                foreach (int iMesAValidar in iMeses)
                {
                    string sAnio = iMesAValidar < 5 ? (iAnioTarifario + 1).ToString() : iAnioTarifario.ToString();
                    sNombreArchivo = sAnio.ToString() + iMesAValidar.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;
                    if (!System.IO.File.Exists(path + sNombreArchivo))
                    {
                        sMesesNoImportados.Add("'" + ConstantesPrimasRER.mesesDesc[iMesAValidar - 1] + "'");
                    }
                }
                if (sMesesNoImportados.Count > 0)
                {
                    sError = "El mes de [" + string.Join(", ", sMesesNoImportados) + "] no se importo previamente";
                    throw new Exception(sError);
                }

                #endregion

                #region Crea el RER_INSUMO
                iRerInsCodi = InsertarInsumo(anioVersionDTO.Reravcodi, sTipoInsumo, sTipoProceso, sLog, sUser);
                #endregion

                #region De los meses seleccionados, obtenemos sus respectivos archivos Excels
                foreach (int iMes in iMeses)
                {
                    int iAnio = iMes < 5 ? iAnioTarifario + 1 : iAnioTarifario;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente

                    #region Valida que exista el mes seleccionado
                    RerParametroPrimaDTO rerParametroPrimaDTO = ListRerParametroPrimas().Where(item => (item.Reravcodi == anioVersionDTO.Reravcodi) && (item.Rerpprmes == iMes)).FirstOrDefault();
                    bool existeParametroPrima = (rerParametroPrimaDTO != null);
                    if (!existeParametroPrima)
                    {
                        sError = "No existe el mes seleccionado";
                        throw new Exception(sError);
                    }
                    #endregion

                    #region Para cada mes, eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    sResultado = DeleteRerInsumoDiaByParametroPrimaAndTipo(rerParametroPrimaDTO.Rerpprcodi, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        throw new Exception(sResultado);
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(rerParametroPrimaDTO.Rerpprcodi, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        throw new Exception(sResultado);
                    #endregion

                    //Traemos la primera hoja del archivo
                    sNombreArchivo = iAnio.ToString() + iMes.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;
                    DataSet ds = new DataSet();
                    ds = GeneraDataset(path + sNombreArchivo, 1);

                    // Obtenemos las centrales
                    DateTime dRerCenFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", iMes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha);

                    int numColumnasXAnalizar = listaCentral.Count;
                    int numFilasXAnalizar = 1;

                    int numRows = ds.Tables[0].Rows.Count;
                    int numCols = ds.Tables[0].Columns.Count;

                    string[][] data = new string[numCols][];

                    int iRegError = 0;

                    #region Valido que el mes seleccionado coincida con lo que se encuentra en el Excel
                    string fechaExcel = ds.Tables[0].Rows[8][1].ToString().Trim();
                    string fechaEsperada = ConstantesPrimasRER.mesesDesc[dRerCenFecha.Month - 1] + "." + dRerCenFecha.Year.ToString();
                    if (fechaExcel != fechaEsperada)
                    {
                        throw new Exception("El mes esperado en la celda B10 es: " + fechaEsperada + " y se obtuvo" + fechaExcel + ". Por favor, no manipular las fechas de la columna B");
                    }
                    #endregion

                    #region Obtenemos el numero de columnas a analizar
                    numColumnasXAnalizar = 0;
                    for (int c = 2 ; c < numCols; c++)
                    {
                        string filaOculta = ds.Tables[0].Rows[6][c].ToString();
                        if (filaOculta != null && filaOculta != "" && filaOculta != "null")
                        {
                            numColumnasXAnalizar++;
                        }
                    }
                    #endregion

                    #region Validacion de los datos ingresados
                    for (int j = 2 ; j < numColumnasXAnalizar + 2 ; j++)        // Recorro las centrales (columnas)
                    {
                        string nombreCodigoEntrega = ds.Tables[0].Rows[4][j].ToString().Trim();
                        string equinomb = ds.Tables[0].Rows[5][j].ToString().Trim();
                        int codiCodigoEntrega = int.Parse(ds.Tables[0].Rows[6][j].ToString().Trim());
                        int equicodi = int.Parse(ds.Tables[0].Rows[7][j].ToString().Trim());
                        string sValorTotal = ds.Tables[0].Rows[8][j].ToString().Trim();

                        #region Validamos que el equicodi y el codigo de entrega se encuentren en listaCentral
                        RerCentralDTO centralRERValida = listaCentral.Where(item => (item.Equicodi == equicodi) && (item.Codentcodi == codiCodigoEntrega)).ToList().FirstOrDefault();
                        bool central_is_valid = (centralRERValida != null);
                        if (!central_is_valid)
                        {
                            throw new Exception("El equipo o código de entrega no se encuentran vigentes en el mes seleleccionado.");
                        }
                        #endregion

                        #region Validamos que el Valor total sea un valor decimal
                        decimal dValorTotal = 0;

                        if (sValorTotal == "null" || sValorTotal == "")
                        {
                            dValorTotal = 0;
                        }
                        else if (!decimal.TryParse(sValorTotal, out dValorTotal))
                        {
                            sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 2) + "8");
                        }
                        else
                        {
                            dValorTotal = decimal.Parse(sValorTotal);
                        }
                        #endregion

                        #region Crea el rer_insumo_mes para una central RER
                        int iRerInsMesCodi = InsertarInsumoMes(iRerInsCodi, rerParametroPrimaDTO.Rerpprcodi, centralRERValida, iAnio, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                        #endregion

                        #region Actualizo el valor de Htotal en rer_insumo_mes
                        RerInsumoMesDTO insumoMes = GetByIdRerInsumoMes(iRerInsMesCodi);
                        insumoMes.Rerinmmestotal = dValorTotal;
                        UpdateRerInsumoMes(insumoMes);
                        #endregion

                    }
                    #endregion

                    sLog = sLog + " El mes [" + iAnio.ToString("D2") + "." + ConstantesPrimasRER.mesesDesc[iMes - 1] + "] se cargo correctamente. <br>";
                }
                #endregion

                #region Actualizar el log de RER_INSUMO
                RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                sLog = sLog + "Se finalizó la importación de la carga manual del insumo [" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "].";
                dtoInsumo.Rerinslog = sLog;
                UpdateRerInsumo(dtoInsumo);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Almacena en la DB los registros (registros de un dia en intervalos de 15 minutos) de los archivos excel importados del insumo 5
        /// </summary>
        /// <param name="iAnioTarifario">Año tarifario seleccionado</param>
        /// <param name="sVersion">Versión del Año Tarifario seleccionado</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <param name="iMeses">Meses que se seleccionaron para realizar la importación</param>
        /// <param name="sUser">Usuario que realiza la importación</param>
        /// <returns></returns>
        public void procesarArchivosInsumoXDia_5(int iAnioTarifario, string sVersion, string sTipoInsumo, int[] iMeses, string sUser)
        {
            try
            {
                string sError = "";
                #region Valido si existe el año tarifario
                RerAnioVersionDTO anioVersionDTO = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(iAnioTarifario, sVersion);
                bool existeAnioTarifario = (anioVersionDTO != null);
                if (!existeAnioTarifario)
                {
                    sError = "No existe el año tarifario seleccionado";
                    throw new Exception(sError);
                }
                #endregion

                #region Variables del método
                string sResultado = "";
                List<string> sCeldaErrores = new List<string>();
                List<string> sMesesNoImportados = new List<string>();

                //Para rer_insumo
                int iRerInsCodi = 0;
                string sTipoProceso = "M";
                string sLog = "Se inició la importación manual del insumo [" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "]. <br>";

                // Para rer_insumo_mes 
                int iRerpprmes = 0;
                string sRerInmTipInformacion = "M"; // Mejor información
                string sRerInmDetalle = "S";        // Si tiene detalle

                // Para rer_insumo_dia
                string sRerIndTipInformacion = "M";
                #endregion

                #region Valido que los meses seleccionado tengan un archivo excel importado
                string sNombreArchivo = "";
                string path = ConfigurationManager.AppSettings[ConstantesPrimasRER.ReporteDirectorio].ToString();

                foreach (int iMesAValidar in iMeses)
                {
                    int iAnio = iMesAValidar < 5 ? iAnioTarifario + 1 : iAnioTarifario;     // Si es Enero, Febrero, Marzo o Abril: Anio=AnioTarifario+1

                    sNombreArchivo = iAnio.ToString() + iMesAValidar.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;
                    if (!System.IO.File.Exists(path + sNombreArchivo))
                    {
                        sMesesNoImportados.Add("'" + ConstantesPrimasRER.mesesDesc[iMesAValidar - 1] + "'");
                    }
                }
                if (sMesesNoImportados.Count > 0)
                {
                    throw new Exception("El mes de [" + string.Join(", ", sMesesNoImportados) + "] no se importo previamente.");
                }
                #endregion

                #region Crea el RER_INSUMO
                //Asignamos el correlativo de la tabla RER_INSUMO_DIA
                int iRerIndDiaCodi = FactoryTransferencia.GetRerInsumoDiaRepository().GetMaxId();
                List<RerInsumoDiaDTO> listaRerInsumoDia = new List<RerInsumoDiaDTO>();

                iRerInsCodi = InsertarInsumo(anioVersionDTO.Reravcodi, sTipoInsumo, sTipoProceso, sLog, sUser);
                #endregion

                #region De los meses seleccionados, obtenemos sus respectivos archivos Excels
                foreach (int iMes in iMeses)
                {
                    int iAnio = iMes < 5 ? iAnioTarifario + 1 : iAnioTarifario;     // Si es Enero, Febrero, Marzo o Abril: Anio=AnioTarifario+1
                    decimal dTotalMes = 0;
                    decimal dTotalDia = 0;

                    #region Valida que exista el mes seleccionado
                    RerParametroPrimaDTO rerParametroPrimaDTO = ListRerParametroPrimas().Where(item => (item.Reravcodi == anioVersionDTO.Reravcodi) && (item.Rerpprmes == iMes)).FirstOrDefault();
                    bool existeParametroPrima = (rerParametroPrimaDTO != null);
                    if (!existeParametroPrima)
                    {
                        sError = "No existe el mes seleccionado";
                        throw new Exception(sError);
                    }
                    #endregion

                    #region Para cada mes, eliminamos la data en la tabla RER_INSUMO_MES e RER_INSUMO_DIA
                    sResultado = DeleteRerInsumoDiaByParametroPrimaAndTipo(rerParametroPrimaDTO.Rerpprcodi, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        throw new Exception(sResultado);
                    sResultado = DeleteRerInsumoMesByParametroPrimaAndTipo(rerParametroPrimaDTO.Rerpprcodi, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo);
                    if (sResultado != "")
                        throw new Exception(sResultado);
                    #endregion

                    #region Obtenemos la cantidad de intervalos de 15 minutos que existen en el mes
                    DateTime dRerCenFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", iMes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    DateTime inicioMesSiguiente = dRerCenFecha.AddMonths(1);
                    TimeSpan intervalo = TimeSpan.FromMinutes(15);
                    int cantidadIntervalos15Min = (int)((inicioMesSiguiente - dRerCenFecha).TotalMinutes / intervalo.TotalMinutes);
                    #endregion

                    int cantidadDias = DateTime.DaysInMonth(iAnio, iMes);
                    int cantidadIntervalos = 0;
                    // Obtenemos las centrales
                    List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha);

                    //Traemos la primera hoja del archivo
                    sNombreArchivo = iAnio.ToString() + iMes.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;

                    DataSet ds = new DataSet();
                    ds = GeneraDataset(path + sNombreArchivo, 1);

                    int numColumnasXAnalizar = listaCentral.Count;
                    int numFilasXAnalizar = cantidadIntervalos15Min;

                    int numRows = ds.Tables[0].Rows.Count;
                    int numCols = ds.Tables[0].Columns.Count;

                    #region Validamos las fechas 
                    List<string> intervalos15Min = ObtenerIntervalosMinutosXDiaConFormato(dRerCenFecha);

                    for (int i = 0; i < 96; i++)
                    {
                        string fechaExcel = ds.Tables[0].Rows[i + 8][2].ToString().Trim();
                        string fechaEsperada = intervalos15Min[i];
                        if (fechaExcel != fechaEsperada)
                        {
                            throw new Exception("El horario esperado en la celda 'B" + (i + 8).ToString() + "' es: " + fechaEsperada + " y se obtuvo" + fechaExcel + ". Por favor, no manipular las fechas de la columna B");
                        }
                    }
                    #endregion

                    #region Obtenemos el numero de columnas a analizar
                    numColumnasXAnalizar = 0;
                    for (int c = 2; c < numCols; c++)
                    {
                        string filaOculta = ds.Tables[0].Rows[6][c].ToString();
                        if (filaOculta != null && filaOculta != "" && filaOculta != "null")
                        {
                            numColumnasXAnalizar++;
                        }
                    }
                    #endregion

                    #region Valida que el dia seleccionado se encuentre entre los dias del mes
                    string sDiaSeleccionado = ds.Tables[0].Rows[8][1].ToString().Trim();
                    int iDiaSeleccionado = 0;
                    int diasEnElMes = DateTime.DaysInMonth(dRerCenFecha.Year, dRerCenFecha.Month);

                    if (int.TryParse(sDiaSeleccionado, out iDiaSeleccionado))
                    {
                        iDiaSeleccionado = int.Parse(sDiaSeleccionado);
                        if (iDiaSeleccionado >= diasEnElMes)
                        {
                            string sErrorDia = "El día colocado en la celda 'A9' no es un día del mes de " + ConstantesPrimasRER.mesesDesc[dRerCenFecha.Month - 1];
                            throw new Exception(sErrorDia);
                        }
                    }
                    else
                    {
                        string sErrorDia = "El valor colocado en la celda 'A9' no es un día del mes de " + ConstantesPrimasRER.mesesDesc[dRerCenFecha.Month - 1];
                        throw new Exception(sErrorDia);
                    }
                    #endregion


                    #region Validacion de los datos para rer_insumo_dia ingresados
                    for (int j = 3; j < numColumnasXAnalizar + 2; j++)        // Recorro las centrales (columnas)
                    {
                        dTotalMes = 0;

                        string nombreCodigoEntrega = ds.Tables[0].Rows[4][j].ToString().Trim();
                        string equinomb = ds.Tables[0].Rows[5][j].ToString().Trim();
                        int codiCodigoEntrega = int.Parse(ds.Tables[0].Rows[6][j].ToString().Trim());
                        int equicodi = int.Parse(ds.Tables[0].Rows[7][j].ToString().Trim());

                        #region Validamos que el equicodi y el codigo de entrega se encuentren en listaCentral
                        RerCentralDTO centralRERValida = listaCentral.Where(item => (item.Equicodi == equicodi) && (item.Codentcodi == codiCodigoEntrega)).ToList().FirstOrDefault();
                        bool central_is_valid = (centralRERValida != null);
                        if (!central_is_valid)
                        {
                            throw new Exception("El equipo o código de entrega no se encuentran vigentes en el mes seleleccionado.");
                        }
                        #endregion

                        #region Crea el rer_insumo_mes para una empresa-central
                        int iRerInsMesCodi = InsertarInsumoMes(iRerInsCodi, rerParametroPrimaDTO.Rerpprcodi, centralRERValida, iAnio, rerParametroPrimaDTO.Rerpprmes, sTipoInsumo, sRerInmTipInformacion, sRerInmDetalle, sUser);
                        #endregion

                        dTotalDia = 0;

                        //Crea un RER_INSUMO_DIA_DTO
                        RerInsumoDiaDTO dtoInsumoDia = new RerInsumoDiaDTO
                        {
                            Rerinmmescodi = iRerInsMesCodi,
                            Rerpprcodi = rerParametroPrimaDTO.Rerpprcodi,
                            Emprcodi = centralRERValida.Emprcodi,
                            Equicodi = centralRERValida.Equicodi,
                            Rerindtipresultado = sTipoInsumo,
                            Rerindtipinformacion = sRerIndTipInformacion,
                            Rerinddiatotal = 0,
                            Rerinddiafecdia = DateTime.ParseExact(string.Format("{0}/{1}/{2}", iDiaSeleccionado.ToString("D2"), iMes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                            Rerinddiausucreacion = sUser,
                            Rerinddiafeccreacion = DateTime.Now
                        };

                        #region Almacena los valores de H de dtoInsumoDia
                        for (int i = 1; i <= 96; i++)
                        {
                            string sValorIntervalo = ds.Tables[0].Rows[(i + 7)][j].ToString().Trim();
                            decimal dValorIntervalo = 0;

                            #region Validamos que el Valor total sea un valor decimal
                            if (sValorIntervalo == "null")
                            {
                                dValorIntervalo = 0;
                            }
                            else if (!decimal.TryParse(sValorIntervalo, out dValorIntervalo))
                            {
                                sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 7).ToString());
                            }
                            else
                            {
                                dValorIntervalo = decimal.Parse(sValorIntervalo);
                            }
                            #endregion

                            #region Guardamos el valor en el Rerinddiah{i} correspondiente en RER_INSUMO_DIA_dto
                            string propertyName = $"Rerinddiah{i}";
                            var propertyInfo = typeof(RerInsumoDiaDTO).GetProperty(propertyName);

                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(dtoInsumoDia, dValorIntervalo);
                            }
                            #endregion

                            dTotalDia += dValorIntervalo;
                        }

                        dtoInsumoDia.Rerinddiatotal = dTotalDia;
                        #endregion

                        #region Almacena el rer_insumo_dia para una empresa-central
                        //APILAMOS PARA EL BULKINSERT
                        dtoInsumoDia.Rerinddiacodi = iRerIndDiaCodi++;
                        listaRerInsumoDia.Add(dtoInsumoDia);
                        #endregion

                        dTotalMes = dTotalDia;

                        #region Actualizamos el Rerinsmtotal en rer_insumo_mes
                        RerInsumoMesDTO insumoMes = GetByIdRerInsumoMes(iRerInsMesCodi);
                        insumoMes.Rerinmmestotal = dTotalMes;
                        UpdateRerInsumoMes(insumoMes);
                        #endregion

                    }
                    #endregion

                    sLog = sLog + " El mes [" + iAnio.ToString("D2") + "." + ConstantesPrimasRER.mesesDesc[iMes - 1] + "] se cargo correctamente. <br>";
                }
                #endregion

                #region Subimos todos los registros de RER_INSUMO_DIA por BulkInsert
                if (listaRerInsumoDia.Count > 0)
                {
                    FactoryTransferencia.GetRerInsumoDiaRepository().BulkInsertRerInsumoDia(listaRerInsumoDia);
                }
                #endregion

                #region Actualizar el log de RER_INSUMO
                RerInsumoDTO dtoInsumo = GetByIdRerInsumo(iRerInsCodi);
                sLog = sLog + "Se finalizó la importación manual del insumo [" + ConstantesPrimasRER.insumosDesc[int.Parse(sTipoInsumo) - 1] + "].";
                dtoInsumo.Rerinslog = sLog;
                UpdateRerInsumo(dtoInsumo);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Valida el archivo Excel importado según el mes, año y tipo de insumo escogidos.
        /// </summary>
        /// <param name="sAnioTarifario">Año tarifario seleccionado</param>
        /// <param name="sVersion">Versión del año tarifario seleccionado</param>
        /// <param name="sNumMes">Mes del año tarifario seleccionado</param>
        /// <param name="sRutaArchivo">Ruta donde se descargará temporalmente el archivo</param>
        /// <param name="sTipoInsumo">Tipo de insumo seleccionado</param>
        /// <returns></returns>
        public void ValidarExcelImportado(string sAnioTarifario, string sVersion, string sNumMes, string sRutaArchivo, string sTipoInsumo) {

            #region Variables
            string sMensajeError = "";
            #endregion

            try
            {
                int iAnioTarifario = int.Parse(sAnioTarifario);
                int iVersion = int.Parse(sVersion);
                int iMes = int.Parse(sNumMes);
                int iAnio = iMes < 5 ? iAnioTarifario + 1 : iAnioTarifario;  // Si el mes es 1, 2, 3 o 4, se considera el año siguiente
                
                #region Validamos que exista el Anio Tarifario
                RerAnioVersionDTO rerAnioVersionDTO = GetRerAnioVersionByAnioVersion(iAnioTarifario, iVersion);
                bool noExisteAnioVersion = (rerAnioVersionDTO == null);
                if (noExisteAnioVersion)
                {
                    sMensajeError = "No existe el Año Tarifario: [Mayo." + iAnioTarifario.ToString() + "-Abril]" + (iAnioTarifario+1).ToString();
                    throw new Exception(sMensajeError);
                }
                #endregion

                DateTime dRerCenFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", iMes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime inicioMesSiguiente = dRerCenFecha.AddMonths(1);
                TimeSpan intervalo = TimeSpan.FromMinutes(15);
                int cantidadIntervalos15Min = (int)((inicioMesSiguiente - dRerCenFecha).TotalMinutes / intervalo.TotalMinutes);

                string sNombreArchivo = iAnio.ToString("D4") + iMes.ToString("D2") + "_v" + sVersion + "_i" + sTipoInsumo;

                #region Validar Excel segun su sTipoInsumo
                switch (sTipoInsumo)
                {
                    case ConstantesPrimasRER.tipoResultadoInyeccionNeta:            // Insumo 1
                    case ConstantesPrimasRER.tipoResultadoCostoMarginal:            // Insumo 2
                    case ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar:    // Insumo 5
                        ValidarImportacionInsumo_1_2_3_4_5(sRutaArchivo + sNombreArchivo, dRerCenFecha, cantidadIntervalos15Min);
                        break;
                    case ConstantesPrimasRER.tipoResultadoIngresosPotencia:         // Insumo 3
                    case ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER:    // Insumo 4
                        ValidarImportacionInsumo_1_2_3_4_5(sRutaArchivo + sNombreArchivo, dRerCenFecha, 1);
                        break;
                    default:
                        sMensajeError = "Ocurrio un error no previsto";
                        throw new Exception(sMensajeError);
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Convierte de un numero de columna, a una letra de columna en formato Excel
        /// </summary>
        /// <param name="numeroColumna">Numero de columna al que se desea obtener el valor en Letra</param>
        public string ConvertirNumeroColumnaALetra(int numeroColumna)
        {
            string letraColumna = "";
            while (numeroColumna > 0)
            {
                int modulo = (numeroColumna - 1) % 26;
                char letra = (char)('A' + modulo);
                letraColumna = letra + letraColumna;
                numeroColumna = (numeroColumna - 1) / 26;
            }
            return letraColumna;
        }

        /// <summary>
        /// Genera los datos para el archivo Excel con respecto a la exportación total de insumos
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="meses"></param>
        /// <param name="tipoInsumo"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoExcelExportarTotalInsumos(int anio, string version, int[] meses, string tipoInsumo)
        {
            try
            {
                #region Obtener datos para exportar 
                List<RerExcelHoja> listRerExcelHoja = null;
                switch (tipoInsumo)
                {
                    case ConstantesPrimasRER.tipoResultadoInyeccionNeta:
                    case ConstantesPrimasRER.tipoResultadoCostoMarginal:
                    case ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar:
                        listRerExcelHoja = GenerarArchivoExcelInsumoDia(anio, version, meses, tipoInsumo);
                        break;

                    case ConstantesPrimasRER.tipoResultadoIngresosPotencia:
                    case ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER:
                        listRerExcelHoja = GenerarArchivoExcelInsumoMes(anio, version, meses, tipoInsumo);
                        break;
                }

                return listRerExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto a la tabla rer_insumo_dia
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="meses"></param>
        /// <param name="tipoResultado"></param>
        /// <returns></returns>
        private List<RerExcelHoja> GenerarArchivoExcelInsumoDia(int anio, string version, int[] meses, string tipoResultado)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe una versión del Año Tarifario con año = {0} y versión = {1}", anio, version));
                }

                foreach (int mes in meses)
                {
                    if (!ConstantesPrimasRER.mesesAnioTarifario.Contains(mes))
                    {
                        throw new Exception(string.Format("No existe un mes con número = {0}", mes));
                    }
                }

                bool pertenece = (tipoResultado == ConstantesPrimasRER.tipoResultadoInyeccionNeta ||
                    tipoResultado == ConstantesPrimasRER.tipoResultadoCostoMarginal ||
                    tipoResultado == ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar);
                if (!pertenece)
                {
                    throw new Exception("El valor de la variable tipoResultado no corresponde a este método");
                }
                #endregion

                #region Obtener datos
                List<RerInsumoDiaDTO> listInsumoDia = FactoryTransferencia.GetRerInsumoDiaRepository().GetByTipoResultadoByPeriodo(tipoResultado, anioVersion.Reravcodi, string.Join(",", meses));
                bool existeListInsumoDia = (listInsumoDia != null && listInsumoDia.Count > 0);
                if (!existeListInsumoDia)
                {
                    throw new Exception("No existen registros a exportar");
                }
                listInsumoDia = listInsumoDia.OrderBy(x => x.Rerinddiafecdia).ToList();

                List<RerCentralDTO> listCentralGroupBy = listInsumoDia.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb }).ToList();
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy);
                List<DateTime> listRerinddiafecdia = listInsumoDia.GroupBy(x => x.Rerinddiafecdia).Select(x => x.Key).ToList();
                int cantidadRegistros = listRerinddiafecdia.Count * ConstantesPrimasRER.numero96;
                #endregion

                #region Variables
                int numeroCabeceras = (tipoResultado == ConstantesPrimasRER.tipoResultadoCostoMarginal) ? 3: 2;
                #endregion

                #region Titulo
                string titulo = "";
                switch (tipoResultado)
                {
                    case ConstantesPrimasRER.tipoResultadoInyeccionNeta:
                        titulo = "Inyección Neta (MWh)";
                        break;

                    case ConstantesPrimasRER.tipoResultadoCostoMarginal:
                        titulo = "Costo Marginal (S/./MWh)";
                        break;

                    case ConstantesPrimasRER.tipoResultadoEnergiaDejadaInyectar:
                        titulo = "Energía Dejada de Inyectar (MWh)";
                        break;
                }

                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[numeroCabeceras];
                List<RerExcelModelo> listaCabecera1 = new List<RerExcelModelo> { CrearExcelModelo("Fecha", "center", 1, numeroCabeceras) };
                List<RerExcelModelo> listaCabecera2 = new List<RerExcelModelo> { };
                List<int> listaAnchoColumna = new List<int> { 20 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera1.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera2.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumna.Add(25);
                }

                listaCabeceras[0] = listaCabecera1;
                listaCabeceras[1] = listaCabecera2;

                if (tipoResultado == ConstantesPrimasRER.tipoResultadoCostoMarginal)
                {
                    listaCabeceras[2] = listCentral.Select(central => CrearExcelModelo(central.Barrbarratransferencia)).ToList();
                }
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { "center" };
                List<string> listaTipo = new List<string> { "string" };
                List<RerExcelEstilo> listaEstilo = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontal.Add("right");
                    listaTipo.Add("double");
                    listaEstilo.Add(CrearExcelEstilo("#,##0.0000"));
                }

                ConcurrentDictionary<int, List<string>[]> cdListaRegistros = new ConcurrentDictionary<int, List<string>[]>();
                Parallel.ForEach(meses, mes =>
                {
                    List<DateTime> listRerinddiafecdiaByMes = listRerinddiafecdia.Where(x => x.Month == mes).ToList();
                    if (listRerinddiafecdiaByMes.Count > 0)
                    {
                        int j = 0;
                        int countByMes = listRerinddiafecdiaByMes.Count * ConstantesPrimasRER.numero96;
                        List<string>[] listaRegistrosByMes = new List<string>[countByMes];
                        foreach (DateTime fecha in listRerinddiafecdiaByMes)
                        {
                            for (int indexHora = 1; indexHora <= ConstantesPrimasRER.numero96; indexHora++)
                            {
                                listaRegistrosByMes[j] = ObtenerInsumoDiaPorFechaHora(fecha, indexHora, listCentral, listInsumoDia);
                                j++;
                            }
                        }
                        cdListaRegistros.TryAdd(mes, listaRegistrosByMes);
                    }
                });

                int i = 0;
                List<string>[] listaRegistros = new List<string>[cantidadRegistros];
                foreach (int mes in ConstantesPrimasRER.mesesAnioTarifario)
                {
                    if (cdListaRegistros.ContainsKey(mes))
                    {
                        for (int j = 0; j < cdListaRegistros[mes].Length; j++)
                        {
                            listaRegistros[i] = cdListaRegistros[mes][j];
                            i++;
                        }
                    }
                }

                RerExcelCuerpo cuerpo = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo,
                    ListaEstilo = listaEstilo
                };
                #endregion

                #region Return
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Hoja1",
                    Titulo = titulo,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);

                return listExcelHoja;
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Genera los datos para un archivo Excel con respecto a la tabla rer_insumo_mes
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="version"></param>
        /// <param name="meses"></param>
        /// <param name="tipoResultado"></param>
        /// <returns></returns>
        private List<RerExcelHoja> GenerarArchivoExcelInsumoMes(int anio, string version, int[] meses, string tipoResultado)
        {
            try
            {
                #region Validación
                RerAnioVersionDTO anioVersion = FactoryTransferencia.GetRerAnioVersionRepository().GetByAnnioAndVersion(anio, version);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe una versión del Año Tarifario con año = {0} y versión = {1}", anio, version));
                }

                foreach (int mes in meses)
                {
                    if (!ConstantesPrimasRER.mesesAnioTarifario.Contains(mes))
                    {
                        throw new Exception(string.Format("No existe un mes con número = {0}", mes));
                    }
                }

                bool pertenece = (tipoResultado == ConstantesPrimasRER.tipoResultadoIngresosPotencia ||
                    tipoResultado == ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER);
                if (!pertenece)
                {
                    throw new Exception("El valor de la variable tipoResultado no corresponde a este método");
                }
                #endregion

                #region Obtener datos
                List<RerInsumoMesDTO> listInsumoMes = FactoryTransferencia.GetRerInsumoMesRepository().GetByTipoResultadoByPeriodo(tipoResultado, anioVersion.Reravcodi, string.Join(",", meses));
                bool existeListInsumoMes = (listInsumoMes != null && listInsumoMes.Count > 0);
                if (!existeListInsumoMes)
                {
                    throw new Exception("No existen registros a exportar");
                }
                listInsumoMes = listInsumoMes.OrderBy(x => x.Rerinmanio).ThenBy(x => x.Rerinmmes).ToList();

                List<RerCentralDTO> listCentralGroupBy = listInsumoMes.GroupBy(x => new { x.Emprcodi, x.Equicodi, x.Equinomb }).Select(x => new RerCentralDTO { Emprcodi = x.Key.Emprcodi, Equicodi = x.Key.Equicodi, Equinomb = x.Key.Equinomb }).ToList(); 
                List<RerCentralDTO> listCentral = AsignarCodigoEntregaYBarraTransferencia(listCentralGroupBy); 
                var listAniomes = listInsumoMes.GroupBy(x => new { x.Rerinmanio, x.Rerinmmes }).Select(x => x.Key);
                #endregion

                #region Variables
                int numeroCabeceras = 2;
                #endregion

                #region Titulo
                string titulo = "";
                switch (tipoResultado)
                {
                    case ConstantesPrimasRER.tipoResultadoIngresosPotencia:
                        titulo = "Ingresos por Potencia RER (S/)";
                        break;

                    case ConstantesPrimasRER.tipoResultadoIngresosCargoPrimaRER:
                        titulo = "Ingresos por Cargo Prima RER (S/)";
                        break;
                }

                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = anioVersion.Reravversiondesc;
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[numeroCabeceras];
                List<RerExcelModelo> listaCabecera1 = new List<RerExcelModelo> { CrearExcelModelo("Fecha", "center", 1, numeroCabeceras) };
                List<RerExcelModelo> listaCabecera2 = new List<RerExcelModelo> { };
                List<int> listaAnchoColumna = new List<int> { 20 };

                foreach (RerCentralDTO central in listCentral)
                {
                    listaCabecera1.Add(CrearExcelModelo(central.Codentcodigo));
                    listaCabecera2.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumna.Add(25);
                }

                listaCabeceras[0] = listaCabecera1;
                listaCabeceras[1] = listaCabecera2;
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { "center" };
                List<string> listaTipo = new List<string> { "string" };
                List<RerExcelEstilo> listaEstilo = new List<RerExcelEstilo> { CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9") };
                foreach (RerCentralDTO central in listCentral)
                {
                    listaAlineaHorizontal.Add("right");
                    listaTipo.Add("double");
                    listaEstilo.Add(CrearExcelEstilo("#,##0.0000"));
                }

                int i = 0;
                List<string>[] listaRegistros = new List<string>[listAniomes.Count()];
                foreach (var aniomes in listAniomes)
                {
                    listaRegistros[i] = ObtenerInsumoMesPorAnioMes(aniomes.Rerinmanio, aniomes.Rerinmmes, listCentral, listInsumoMes);
                    i++;
                }

                RerExcelCuerpo cuerpo = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo,
                    ListaEstilo = listaEstilo
                };
                #endregion

                #region Return
                List<RerExcelHoja> listExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Hoja1",
                    Titulo = titulo,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    Cuerpo = cuerpo
                };
                listExcelHoja.Add(excelHoja);

                return listExcelHoja;
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Devuelve una lista de centrales con los siguientes datos: 
        /// Emprcodi,
        /// Equicodi,
        /// Emprnomb,
        /// Equinomb,
        /// Codentcodigo y
        /// Barrbarratransferencia.
        /// Ordenada alfabéticamente por el nombre de la empresa y de la central. 
        /// </summary>
        /// <param name="listCentralBasica"></param>
        /// <returns></returns>
        private List<RerCentralDTO> AsignarCodigoEntregaYBarraTransferencia(List<RerCentralDTO> listCentralBasica) 
        {
            try
            {
                List<RerCentralDTO> listCodigoEntregaYBarraTransferencia = FactoryTransferencia.GetRerCentralRepository().ListCodigoEntregaYBarraTransferencia();
                bool existeListCodigoEntregaYBarraTransferencia = (listCodigoEntregaYBarraTransferencia != null && listCodigoEntregaYBarraTransferencia.Count > 0);
                if (!existeListCodigoEntregaYBarraTransferencia)
                {
                    throw new Exception("No existen centrales RER con código de entrega y barra de transferencia");
                }

                List<RerCentralDTO> listCentral = new List<RerCentralDTO>();
                foreach (var g in listCentralBasica)
                {
                    List<RerCentralDTO> listCEYBT = listCodigoEntregaYBarraTransferencia.Where(x => x.Emprcodi == g.Emprcodi && x.Equicodi == g.Equicodi).ToList();
                    bool existeListCEYBT = (listCEYBT.Count > 0);
                    if (!existeListCEYBT)
                    {
                        throw new Exception(string.Format("No existe un código de entrega y barra de transferencia para la central RER '{0}'", g.Equinomb));
                    }
                    RerCentralDTO codigoEntregaYBarraTransferencia = listCEYBT[0];

                    RerCentralDTO central = new RerCentralDTO
                    {
                        Emprcodi = g.Emprcodi,
                        Equicodi = g.Equicodi,
                        Emprnomb = !string.IsNullOrWhiteSpace(g.Emprnomb) ? g.Emprnomb.Trim() : "",
                        Equinomb = !string.IsNullOrWhiteSpace(g.Equinomb) ? g.Equinomb.Trim() : "",
                        Codentcodigo = codigoEntregaYBarraTransferencia.Codentcodigo,
                        Barrbarratransferencia = codigoEntregaYBarraTransferencia.Barrbarratransferencia
                    };
                    listCentral.Add(central);
                }

                listCentral = listCentral.OrderBy(x => x.Emprnomb).ThenBy(x => x.Equinomb).ToList();
                return listCentral;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los datos de los insumos dia de las Centrales RER, para una fecha y hora especifica,
        /// en el orden especificado de la lista de centrales 'listCentral'
        /// </summary>
        /// <param name="rerinddiafecdia"></param>
        /// <param name="indexHora"></param>
        /// <param name="listCentral"></param>
        /// <param name="listInsumoDia">Todos los registros deben corresponder a un mismo rerindtipresultado</param>
        /// <returns></returns>
        private List<string> ObtenerInsumoDiaPorFechaHora(DateTime rerinddiafecdia, int indexHora, List<RerCentralDTO> listCentral, List<RerInsumoDiaDTO> listInsumoDia) 
        {
            try 
            {
                List<string> listRegistro = new List<string> { ObtenerFechaHora(rerinddiafecdia, indexHora) };
                foreach (RerCentralDTO central in listCentral) 
                {
                    List<RerInsumoDiaDTO> listIDia = listInsumoDia.Where(x => x.Rerinddiafecdia == rerinddiafecdia && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                    if (listIDia.Count > 0) 
                    {
                        var value = listIDia[0].GetType().GetProperty($"Rerinddiah{indexHora}").GetValue(listIDia[0], null);
                        listRegistro.Add((value != null) ? value.ToString() : "");
                    }
                    else 
                    {
                        listRegistro.Add("");
                    }
                }
                return listRegistro;
            }
            catch (Exception ex) 
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los datos de los insumos mes de las Centrales RER, para un año y mes especifico, 
        /// en el orden especificado de la lista de centrales 'listCentral'
        /// </summary>
        /// <param name="rerinmanio"></param>
        /// <param name="rerinmmes"></param>
        /// <param name="listCentral"></param>
        /// <param name="listInsumoMes">Todos los registros deben corresponder a un mismo rerindtipresultado</param>
        /// <returns></returns>
        private List<string> ObtenerInsumoMesPorAnioMes(int rerinmanio, int rerinmmes, List<RerCentralDTO> listCentral, List<RerInsumoMesDTO> listInsumoMes)
        {
            try
            {
                List<string> listRegistro = new List<string> { string.Format("{0}.{1}", ConstantesPrimasRER.mesesDesc[rerinmmes - 1], rerinmanio) };
                foreach (RerCentralDTO central in listCentral)
                {
                    List<RerInsumoMesDTO> listIMes = listInsumoMes.Where(x => x.Rerinmanio == rerinmanio && x.Rerinmmes == rerinmmes && x.Emprcodi == central.Emprcodi && x.Equicodi == central.Equicodi).ToList();
                    if (listIMes.Count > 0)
                    {
                        listRegistro.Add(listIMes[0].Rerinmmestotal.ToString());
                    }
                    else
                    {
                        listRegistro.Add("");
                    }
                }
                return listRegistro;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Arma una fecha y hora en base a los valores entregados
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="indexHora"></param>
        /// <returns></returns>
        private string ObtenerFechaHora(DateTime fecha, int indexHora)
        {
            int hora = 0;
            int minutos = indexHora * ConstantesPrimasRER.numero15;
            while (minutos >= ConstantesPrimasRER.numero60)
            {
                hora++;
                minutos -= ConstantesPrimasRER.numero60;
            }

            if (indexHora == ConstantesPrimasRER.numero96)
            {
                fecha = fecha.AddDays(1);
                hora = 0;
            }
                
            return string.Format("{0}/{1}/{2} {3}:{4}", fecha.Day.ToString("D2"), fecha.Month.ToString("D2"), fecha.Year, hora.ToString("D2"), minutos.ToString("D2"));
        }

        #endregion

        #region Métodos de la tabla RER_PARAMETRO_REVISION
        public int SaveRerParametroRevision(RerParametroRevisionDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerParametroRevisionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRerParametroRevision(int rerParametroRevisionId)
        {
            try
            {
                FactoryTransferencia.GetRerParametroRevisionRepository().Delete(rerParametroRevisionId);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<RerParametroRevisionDTO> ListRerParametroRevisiones()
        {
            return FactoryTransferencia.GetRerParametroRevisionRepository().List();
        }

        public List<RerParametroRevisionDTO> ListRerParametroRevisionesByRerpprcodiByTipo(int Rerpprcodi, string Rerpretipo)
        {
            return FactoryTransferencia.GetRerParametroRevisionRepository().ListByRerpprcodiByTipo(Rerpprcodi, Rerpretipo);
        }

        public void DeleteAllRerParametroRevisionByRerpprcodi(int Rerpprcodi)
        {
            try
            {
                FactoryTransferencia.GetRerParametroRevisionRepository().DeleteAllByRerpprcodi(Rerpprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos de la tabla RER_SDDP
        /// <summary>
        /// CUS20-Importar insumos del SDDP
        /// Registra información en la tabla RER_SDDP
        /// </summary>
        /// <param name="entity">Objeto entidad RerSddpDTO</param>
        /// <returns>int</returns>
        public int SaveRerSddp(RerSddpDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerSddpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos de la tabla RER_GERCSV
        /// <summary>
        /// CUS20-Importar insumos del SDDP
        /// Registra información en la tabla RER_GERCSV
        /// </summary>
        /// <param name="entity">Objeto entidad RerGerCsvDTO</param>
        /// <returns>int</returns>
        public int SaveRerGerCsv(RerGerCsvDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerGerCsvRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion

        #region Métodos de la tabla RER_GERCSV_DET
        /// <summary>
        /// CUS20-Importar insumos del SDDP
        /// Listar información en la tabla RER_GERCSV_DET
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <returns>RerGerCsvDetDTO</returns>
        public List<RerGerCsvDetDTO> ListRerGerCsvDetsByEquipo(int equicodi, DateTime fechainicio, DateTime fechafin)
        {
            return FactoryTransferencia.GetRerGerCsvDetRepository().ListByEquipo(equicodi, fechainicio, fechafin);
        }
        #endregion

        #region Métodos de la tabla RER_INSUMO_VTP
        public int SaveRerInsumoVtp(RerInsumoVtpDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerInsumoVtpRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Proceso que se encarga de Eliminar los registros de RER_INSUMO_VTP
        /// </summary>
        /// <param name="iRerpprcodi">Identificador de la Tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iRerpprmes">Mes a eliminar</param>
        /// <returns></returns>
        public string DeleteRerInsumoVTPByParametroPrimaAndMes(int iRerpprcodi, int iRerpprmes)
        {
            string sResultado = "";
            try
            {
                FactoryTransferencia.GetRerInsumoVtpRepository().DeleteByParametroPrimaAndMes(iRerpprcodi, iRerpprmes);
            }
            catch (Exception ex)
            {
                sResultado = ex.Message;
            }
            return sResultado;
        }

        /// <summary>
        /// Proceso que se encarga de obtener el SaldoVTP por ParametroPrima, Empresa y Central RER
        /// </summary>
        /// <param name="iRerpprcodi">Identificador de la Tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iEmprcodi">Identificador de la Tabla SI_EMPRESA</param>
        /// <param name="iEquicodi">Identificador de la Tabla EQ_EQUIPO</param>
        /// <returns></returns>
        public decimal ObtenerSaldoVtpByInsumoVTP(int iRerpprcodi, int iEmprcodi, int iEquicodi)
        {
            decimal dResultado = 0;
            try
            {
                dResultado = FactoryTransferencia.GetRerInsumoVtpRepository().ObtenerSaldoVtpByInsumoVTP(iRerpprcodi, iEmprcodi, iEquicodi);
            }
            catch (Exception ex)
            {
                string sMensaje = ex.Message;
            }
            return dResultado;
        }
        #endregion

        #region Métodos de la tabla RER_INSUMO_VTEA
        public int SaveRerInsumoVtea(RerInsumoVteaDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetRerInsumoVteaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Proceso que se encarga de Eliminar los registros de RER_INSUMO_VTEA para un Mes relacionado con el Parametro Prima
        /// </summary>
        /// <param name="iRerpprcodi">Identificador de la Tabla RER_PARAMETRO_PRIMA</param>
        /// <returns></returns>
        public string DeleteRerInsumoVTEAyParametroPrimaAndMes(int iRerpprcodi)
        {
            string sResultado = "";
            try
            {
                FactoryTransferencia.GetRerInsumoVteaRepository().DeleteByParametroPrimaAndMes(iRerpprcodi);
            }
            catch (Exception ex)
            {
                sResultado = ex.Message;
            }
            return sResultado;
        }
        /// <summary>
        /// Proceso que se encarga de obtener el SaldoVTEA por ParametroPrima, Empresa y Central RER
        /// </summary>
        /// <param name="iRerpprcodi">Identificador de la Tabla RER_PARAMETRO_PRIMA</param>
        /// <param name="iEmprcodi">Identificador de la Tabla SI_EMPRESA</param>
        /// <param name="iEquicodi">Identificador de la Tabla EQ_EQUIPO</param>
        /// <returns></returns>
        public decimal ObtenerSaldoVteaByInsumoVTEA(int iRerpprcodi, int iEmprcodi, int iEquicodi)
        {
            decimal dResultado = 0;
            try
            {
                dResultado = FactoryTransferencia.GetRerInsumoVteaRepository().ObtenerSaldoVteaByInsumoVTEA(iRerpprcodi, iEmprcodi, iEquicodi);
            }
            catch (Exception ex)
            {
                string sMensaje = ex.Message;
            }
            return dResultado;
        }
        #endregion

        #region Métodos Importacion Costos Marginales
        /// <summary>
        /// Obtener código de envio
        /// </summary>
        /// <param name="iPeriAnioMes">Año mes</param>
        /// <returns>Codigo de envio</returns>
        public int ObtenerCodigoEnvio(int iPeriAnioMes)
        {
            int codigoenvio = 0;

            // Obtengo el id del periodo Pmpo por año y mes
            int idPeriodoPmpo = FactoryTransferencia.GetRerInsumoRepository().GetIdPeriodoPmpoByAnioMes(iPeriAnioMes);
            var regPeriodo = this.GetByIdPmoPeriodo(idPeriodoPmpo);
            DateTime fecha1Mes = regPeriodo.Pmperifecinimes;
            var regEnvio = this.GetUltimoEnvioSddp(fecha1Mes);
            if (regEnvio != null)
            {
                codigoenvio = regEnvio.Enviocodi;
            }

            return codigoenvio;
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PMO_PERIODO
        /// </summary>
        public PmoPeriodoDTO GetByIdPmoPeriodo(int pmpericodi)
        {
            var reg = FactorySic.GetPmoPeriodoRepository().GetById(pmpericodi);

            if (reg != null)
            {
                FormatearPmoPeriodo(reg);
            }

            return reg;
        }

        /// <summary>
        /// Metodo obtener datos para generar reporte PMPO de Centrales
        /// </summary>
        /// <param name="codigoenvio"></param>
        /// <param name="tipoSalida"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GetDatosPMPOReporteEnvio(int codigoenvio, string tipoSalida)
        {
            // Cuadro N° 5.4
            string param11 = (tipoSalida == "F") ? "25000,25047,25052,25086" : "-1";
            List<MeMedicionxintervaloDTO> entitys = FactorySic.GetMeMedicionxintervaloRepository().ListarReporteSDDP(codigoenvio, "80", param11);
            foreach (var regPto in entitys)
            {
                if (regPto.Ptomedicodi == 25000) regPto.Ptomedidesc = "SEIN";
                regPto.Orden = regPto.Ptomedicodi;
            }


            return entitys;
        }

        private void FormatearPmoPeriodo(PmoPeriodoDTO reg)
        {
            reg.SPmPeriFecIniMantAnual = reg.PmPeriFecIniMantAnual.ToString(ConstantesAppServicio.FormatoFecha);
            reg.SPmPeriFecFinMantAnual = reg.PmPeriFecFinMantAnual.ToString(ConstantesAppServicio.FormatoFecha);
            reg.SPmPeriFecIniMantMensual = reg.PmPeriFecIniMantMensual.ToString(ConstantesAppServicio.FormatoFecha);
            reg.SPmPeriFecFinMantMensual = reg.PmPeriFecFinMantMensual.ToString(ConstantesAppServicio.FormatoFecha);
        }

        /// <summary>
        /// Obtener Ultimo Envio Sddp
        /// </summary>
        public MeEnvioDTO GetUltimoEnvioSddp(DateTime? fecha1Mes)
        {
            DateTime f1 = fecha1Mes != null ? fecha1Mes.Value : DateTime.Today.AddYears(-3);
            DateTime f2 = fecha1Mes != null ? fecha1Mes.Value : DateTime.Today.AddYears(1);

            MeEnvioDTO version = this.GetListaMultipleMeEnviosXLS(ConstantesPMPO.EmprcodiCoes.ToString(), ConstantesAppServicio.ParametroDefecto, ConstantesPMPO.FormatoPMPO.ToString()
                                                                                , ConstantesAppServicio.ParametroDefecto, f1, f2)
                                                                            .OrderByDescending(x => x.Enviocodi).FirstOrDefault();
            if (version != null)
            {
                version.EnviofechaDesc = version.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            }
            return version;
        }

        /// <summary>
        /// Obtener Lista Multiple Envios XLS
        /// </summary>
        public List<MeEnvioDTO> GetListaMultipleMeEnviosXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Llenar matriz de insumos de costos marginales pronosticado de acuerdo a la logica del ECUS20
        /// </summary>
        /// <returns>List RerGerCsvDetDTO</returns>
        public List<RerInsumoDiaTemporalDTO> ObtenerMatrizRerInsumoDia()
        {
            List<RerInsumoDiaTemporalDTO> entitysRerInsumoDiaTemporalDTO = new List<RerInsumoDiaTemporalDTO>();
            //Definimos los dias de la semana
            char[] diasSemana = { 'S', 'D', 'L', 'M', 'X', 'J', 'V' };
            //Traemos la lista de feriados: listaferiado.Pmfrdofecha: Almacena las fechas que son Feriado PMPO
            List<PmoFeriadoDTO> listaferiado = FactorySic.GetPmoFeriadoRepository().List();
            //Traemos la lista de CentralesRER Activos
            List<RerCentralDTO> entitysPtoMedicion = FactoryTransferencia.GetRerGerCsvRepository().ListPtoMedicionCentralesRer();
            foreach (RerCentralDTO entidad in entitysPtoMedicion)
            {
                //Para cada PtoMedición en Centrarl RER
                if (entidad.Ptomedicodi == null)
                {
                    throw new Exception("Verificar que todas las centrales RER tengan asociado una barra PMPO.");
                }
                int iPtoMediCodi = (int)entidad.Ptomedicodi;
                //Traemos la lista RerInsumoTemporalDTO de un punto de medición: 
                List<RerInsumoTemporalDTO> entitysRerInsumoTemporalDTO = FactoryTransferencia.GetRerGerCsvRepository().ListTablaCMTemporal(iPtoMediCodi);
                //Vamos a poblar la tabla RER_INSUMO_DIA_TEMP
                int iLimiteEtapa = 80;
                if (entitysRerInsumoTemporalDTO.Count < 80) iLimiteEtapa = entitysRerInsumoTemporalDTO.Count;
                for (int iContadorEtapa = 0; iContadorEtapa < iLimiteEtapa; iContadorEtapa++)
                {
                    //Tomamos los bloques de 5 en 5
                    decimal dBloque1 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa].Rervalor;
                    decimal dBloque2 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 1].Rervalor;
                    decimal dBloque3 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 2].Rervalor;
                    decimal dBloque4 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 3].Rervalor;
                    decimal dBloque5 = (decimal)entitysRerInsumoTemporalDTO[5 * iContadorEtapa + 4].Rervalor;
                    //Las 7 fechas de la semana 

                    //iniciando en Sabado:
                    DateTime dFecha = entitysRerInsumoTemporalDTO[5 * iContadorEtapa].Rerfecinicio;
                    string sPtomediDesc = entitysRerInsumoTemporalDTO[5 * iContadorEtapa].Ptomedidesc;

                    for (int i = 0; i < diasSemana.Length; i++)
                    {
                        char sFeriado = 'N';
                        var feriadoEncontrado = listaferiado.Where(x => x.Pmfrdofecha == dFecha).FirstOrDefault();
                        if (feriadoEncontrado != null)
                            sFeriado = 'S';
                        entitysRerInsumoDiaTemporalDTO.Add(addRerInsumoDiaTemporal(iPtoMediCodi, sPtomediDesc, dFecha, diasSemana[i], dBloque1, dBloque2, dBloque3, dBloque4, dBloque5, sFeriado));
                        dFecha = dFecha.AddDays(1);
                    }
                }
            }

            return entitysRerInsumoDiaTemporalDTO;
        }

        /// <summary>
        /// Completar un dia de insumos de los costos marginales pronosticados de acuerdo a la logica del ECUS20
        /// </summary>
        /// <param name="iPtoMediCodi"></param>
        /// <param name="sPtomediDesc"></param>
        /// <param name="dFecha"></param>
        /// <param name="sDia"></param>
        /// <param name="dBloque1"></param>
        /// <param name="dBloque2"></param>
        /// <param name="dBloque3"></param>
        /// <param name="dBloque4"></param>
        /// <param name="dBloque5"></param>
        /// <param name="sFeriado"></param>
        /// <returns>RerInsumoDiaTemporalDTO</returns>
        public RerInsumoDiaTemporalDTO addRerInsumoDiaTemporal(int iPtoMediCodi, string sPtomediDesc, DateTime dFecha, 
            char sDia, decimal dBloque1, decimal dBloque2, decimal dBloque3, decimal dBloque4, decimal dBloque5, char sFeriado)
        {
            RerInsumoDiaTemporalDTO dtoRerInsumoDiaTemporal = new RerInsumoDiaTemporalDTO();
            dtoRerInsumoDiaTemporal.Ptomedicodi = iPtoMediCodi;
            dtoRerInsumoDiaTemporal.Ptomedidesc = sPtomediDesc;
            dtoRerInsumoDiaTemporal.Rerinddiafecdia = dFecha;
            decimal dTotal = 0;
            //De Sabado a Viernes, el Bloque 5 se asigna sin restrigción
            //h1 -> h32
            int j = 1;
            while (j <= 32)
            {
                dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque5);
                dTotal += dBloque5;
                j++;
            }
            //h93 -> h96
            j = 93;
            while (j <= 96)
            {
                dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque5);
                dTotal += dBloque5;
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 4 se asigna en el horario del h33 -> h72,
            //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 2
            j = 33;
            while (j <= 72)
            {
                if ((j >= 45 && j <= 48) && ((sDia != 'S' && sDia != 'D') || sFeriado == 'S'))
                {
                    //Cumple para L, M. X, J y V en el intervalo h45-> h48
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque2);
                    dTotal += dBloque2;
                }
                else
                {
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque4);
                    dTotal += dBloque4;
                }
                j++;
            }
            /**************************************************************************************************/
            //De Sabado a Viernes, el Bloque 3 se asigna en el horario del h73 -> h92,
            //con Excepciones los L, M. X, J y V en el intervalo h45-> h48 donde se asigna el Bloque 1
            j = 73;
            while (j <= 92)
            {
                if ((j >= 77 && j <= 78) && ((sDia != 'S' && sDia != 'D') || sFeriado == 'S'))
                {
                    //Cumple para L, M. X, J y V en el intervalo h45-> h48
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque1);
                    dTotal += dBloque1;
                }
                else
                {
                    dtoRerInsumoDiaTemporal.GetType().GetProperty($"Rerinddiah{(j)}").SetValue(dtoRerInsumoDiaTemporal, dBloque3);
                    dTotal += dBloque3;
                }
                j++;
            }
            dtoRerInsumoDiaTemporal.Rerinddiatotal = dTotal;
            return dtoRerInsumoDiaTemporal;
        }
        #endregion

        #region Plantilla Insumos de Carga Manual

        /// <summary>
        /// Genera los datos para el archivo Excel con respecto a la exportación total de insumos
        /// </summary>
        /// <param name="iRerpprcodi"></param>
        /// <param name="sVersion"></param>
        /// <param name="sTipoInsumo"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public List<RerExcelHoja> GenerarArchivoPlantillaExcelCargaManual(int iRerpprcodi, string sVersion, string sTipoInsumo, out string nombreArchivo)
        {
            try
            {
                #region Validación

                RerParametroPrimaDTO parametroPrima = GetByIdRerParametroPrima(iRerpprcodi);
                bool noExisteParametroPrima = (parametroPrima == null);
                if (noExisteParametroPrima)
                {
                    throw new Exception(string.Format("No existe un registro de la tabla rer_parametro_prima con id = ", iRerpprcodi.ToString()));
                }

                RerAnioVersionDTO anioVersion = GetByIdRerAnioVersion(parametroPrima.Reravcodi);
                bool noExisteAnioVersion = (anioVersion == null);
                if (noExisteAnioVersion)
                {
                    throw new Exception(string.Format("No existe un registro de la tabla rer_anioversion con id = ", parametroPrima.Reravcodi));
                }
                #endregion

                #region Obtener datos para exportar 
                int iAnio = parametroPrima.Rerpprmes < 5 ? anioVersion.Reravaniotarif + 1 : anioVersion.Reravaniotarif;     // Si es Enero, Febrero, Marzo o Abril: Anio=AnioTarifario+1

                DateTime dRerCenFecha = DateTime.ParseExact(string.Format("01/{0}/{1}", parametroPrima.Rerpprmes.ToString("D2"), iAnio.ToString("D4")), ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                // Obtener datos: listado de centrales
                List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha).OrderBy(item => item.Equinomb).ToList();

                // Obtengo el nombre del archivo a exportar
                string descTipoInsumo = UtilPrimasRER.ObtenerNombreTipoResultado(sTipoInsumo, true);
                nombreArchivo = ("Plantilla_" + descTipoInsumo + "_" + iAnio.ToString() + "_" + ConstantesPrimasRER.mesesDesc[parametroPrima.Rerpprmes - 1] + "_v" + sVersion);
              
                #region Armando el archivo Excel

                #region Variables
                int numeroCabeceras = 2;
                string nombreAtributo = sTipoInsumo == "2" ? "Barrbarratransferencia" : "Codentcodigo";
                #endregion

                #region Titulo
                string titulo = ConstantesPrimasRER.cabeceraInsumosDesc[Int32.Parse(sTipoInsumo) - 1];
                string subtitulo1 = anioVersion.Reravaniotarifdesc;
                string subtitulo2 = ConstantesPrimasRER.versionesDesc[Int32.Parse(anioVersion.Reravversion)];
                #endregion

                #region Cabecera
                List<RerExcelModelo>[] listaCabeceras = new List<RerExcelModelo>[numeroCabeceras];
                List<RerExcelModelo> listaCabecera1 = new List<RerExcelModelo> { };
                List<RerExcelModelo> listaCabecera2 = new List<RerExcelModelo> { };
                List<int> listaAnchoColumna = new List<int> { };
                listaCabecera1.Add(CrearExcelModelo("Fecha", "center", 1, numeroCabeceras));
                listaAnchoColumna.Add(20);

                foreach (RerCentralDTO central in listaCentral)
                {
                    listaCabecera1.Add(CrearExcelModelo((central.GetType().GetProperty(nombreAtributo)?.GetValue(central))?.ToString()));
                    listaCabecera2.Add(CrearExcelModelo(central.Equinomb));
                    listaAnchoColumna.Add(25);
                }

                listaCabeceras[0] = listaCabecera1;
                listaCabeceras[1] = listaCabecera2;
                #endregion

                #region Cuerpo Oculto
                List<string>[] listaRegistrosOcultos = new List<string>[numeroCabeceras];
                List<string> listaCuerpoOculto1 = new List<string> { };
                List<string> listaCuerpoOculto2 = new List<string> { };
                listaCuerpoOculto1.Add("Codentcodi");
                listaCuerpoOculto2.Add("Equicodi");
                foreach (RerCentralDTO central in listaCentral)
                {
                    listaCuerpoOculto1.Add(central.Codentcodi.ToString());
                    listaCuerpoOculto2.Add(central.Equicodi.ToString());
                }
                listaRegistrosOcultos[0] = listaCuerpoOculto1;
                listaRegistrosOcultos[1] = listaCuerpoOculto2;

                RerExcelCuerpo cuerpoOculto = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistrosOcultos,
                };
                #endregion

                #region Cuerpo
                List<string> listaAlineaHorizontal = new List<string> { };
                List<string> listaTipo = new List<string> { };
                List<RerExcelEstilo> listaEstilo = new List<RerExcelEstilo> {  };
                listaAlineaHorizontal.Add("center");
                listaTipo.Add("string");
                listaEstilo.Add(CrearExcelEstilo(null, true, Color.White, "#2980B9", "#DADAD9"));
                foreach (RerCentralDTO central in listaCentral)
                {
                    listaAlineaHorizontal.Add("right");
                    listaTipo.Add("double");
                }

                List<string> intervalosFormateados = new List<string>();

                if (sTipoInsumo == "3" || sTipoInsumo == "4") {
                    intervalosFormateados = new List<string> { parametroPrima.Rerpprmesaniodesc };
                }
                else {
                    intervalosFormateados = ObtenerIntervalosXMesConFormato(dRerCenFecha);
                }

                List<string>[] listaRegistros = new List<string>[intervalosFormateados.Count];

                for (int i = 0; i < intervalosFormateados.Count; i++)
                {
                    listaRegistros[i] = new List<string> { };
                    listaRegistros[i].Add(intervalosFormateados[i]);
                }

                RerExcelCuerpo cuerpo = new RerExcelCuerpo
                {
                    ListaRegistros = listaRegistros,
                    ListaAlineaHorizontal = listaAlineaHorizontal,
                    ListaTipo = listaTipo,
                    ListaEstilo = listaEstilo
                };
                #endregion

                #region Definir hoja Resumen
                List<RerExcelHoja> listRerExcelHoja = new List<RerExcelHoja>();
                RerExcelHoja excelHoja = new RerExcelHoja
                {
                    NombreHoja = "Plantilla",
                    Titulo = titulo,
                    Subtitulo1 = subtitulo1,
                    Subtitulo2 = subtitulo2,
                    ListaAnchoColumna = listaAnchoColumna,
                    ListaCabeceras = listaCabeceras,
                    CuerpoOculto = cuerpoOculto,
                    Cuerpo = cuerpo
                };
                listRerExcelHoja.Add(excelHoja);
                #endregion

                #endregion

                return listRerExcelHoja;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        /// <summary>
        /// Valida el archivo Excel importado en el insumo 1 (Inyección Neta 15 min) y 2 (Costo Marginal 15 min)
        /// </summary>
        /// <param name="sRutaNombreArchivo"></param>
        /// <param name="dRerCenFecha"></param>
        /// <param name="cantidadIntervalos"></param>
        public void ValidarImportacionInsumo_1_2_3_4_5(string sRutaNombreArchivo, DateTime dRerCenFecha, int cantidadIntervalos)
        {
            try
            {
                #region Variables
                DateTime inicioMesSiguiente = dRerCenFecha.AddMonths(1);
                TimeSpan intervalo = TimeSpan.FromMinutes(15);
                List<string> intervalosFormateados = new List<string>();

                int desplazamiento = 0;
                #endregion
                int cantidadIntervalos15Min = (int)((inicioMesSiguiente - dRerCenFecha).TotalMinutes / intervalo.TotalMinutes);
                
                //Obtenemos la lista de centrales
                List<RerCentralDTO> listaCentral = ListCentralByFecha(dRerCenFecha);

                intervalosFormateados = ObtenerIntervalosXMesConFormato(dRerCenFecha);
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = GeneraDataset(sRutaNombreArchivo, 1);

                int numColumnasXAnalizar = listaCentral.Count;
                int numFilasXAnalizar = cantidadIntervalos;

                int numRows = ds.Tables[0].Rows.Count;
                int numCols = ds.Tables[0].Columns.Count;

                List<string> sCeldaErrores = new List<string>();

                if (cantidadIntervalos > 96)
                {
                    #region Validamos las fechas con formato dd/MM/yyyy HH:mm
                    for (int f = 8; f < numFilasXAnalizar + 8; f++)
                    {
                        string fechaExcel = ds.Tables[0].Rows[f][1].ToString();
                        string fechaEsperada = dRerCenFecha.AddMinutes(15 * (f - 7)).ToString("dd/MM/yyyy HH:mm");
                        if (fechaExcel != fechaEsperada)
                        {
                            throw new Exception("La fecha esperada en la celda 'B" + (f + 2).ToString() + "' es " + fechaEsperada + " y se obtuvo " + fechaExcel + ". Por favor, no manipular las fechas de la columna B.");
                        }
                    }
                    #endregion
                }
                else if (cantidadIntervalos == 1)
                {
                    string fechaExcel = ds.Tables[0].Rows[8][1].ToString().Trim();
                    string fechaEsperada = ConstantesPrimasRER.mesesDesc[dRerCenFecha.Month - 1] + "." + dRerCenFecha.Year.ToString();
                    if (fechaExcel != fechaEsperada)
                    {
                        throw new Exception("El mes esperado en la celda 'B10' es " + fechaEsperada + " y se obtuvo " + fechaExcel + ". Por favor, no manipular las fechas de la columna B.");
                    }
                }
                else if (cantidadIntervalos == 96) 
                {
                    desplazamiento = 1;
                    #region Valida que el dia seleccionado se encuentre entre los dias del mes
                    string sDiaSeleccionado = ds.Tables[0].Rows[8][1].ToString().Trim();
                    int iDiaSeleccionado = 0;
                    int diasEnElMes = DateTime.DaysInMonth(dRerCenFecha.Year, dRerCenFecha.Month);

                    if (int.TryParse(sDiaSeleccionado, out iDiaSeleccionado))
                    {
                        iDiaSeleccionado = int.Parse(sDiaSeleccionado);
                        if (iDiaSeleccionado >= diasEnElMes)
                        {
                            string sErrorDia = "El día colocado en la celda 'A9' no es un día del mes de " + ConstantesPrimasRER.mesesDesc[dRerCenFecha.Month - 1];
                            throw new Exception(sErrorDia);
                        }
                    }
                    else
                    {
                        string sErrorDia = "El valor colocado en la celda 'A9' no es un día del mes de " + ConstantesPrimasRER.mesesDesc[dRerCenFecha.Month - 1];
                        throw new Exception(sErrorDia);
                    }
                    #endregion

                    #region Valida que los intervalos de 15 minutos se encuentren completos
                    List<string> intervalos15Min = ObtenerIntervalosMinutosXDiaConFormato(dRerCenFecha);

                    for (int i = 0; i < 96; i++) {
                        string fechaExcel = ds.Tables[0].Rows[i + 8][2].ToString().Trim();
                        string fechaEsperada = intervalos15Min[i];
                        if (fechaExcel != fechaEsperada)
                        {
                            throw new Exception("El horario esperado en la celda 'B" + (i + 8).ToString() + "' es " + fechaEsperada + " y se obtuvo" + fechaExcel + ". Por favor, no manipular las fechas de la columna B.");
                        }
                    }

                    #endregion
                }
                else {
                    throw new Exception("Se agregó un archivo Excel incorrecto. Vuelva a intentarlo.");
                }

                List<RerCentralDTO> listaRerCentral = new List<RerCentralDTO>();
                #region Obtenemos el numero de columnas a analizar
                numColumnasXAnalizar = 0;
                for (int c = 2 + desplazamiento; c < numCols ; c++)
                {
                    string filaOculta = ds.Tables[0].Rows[6][c].ToString();
                    RerCentralDTO rerCentralDto = new RerCentralDTO();
                    rerCentralDto.Codentcodigo = filaOculta;
                    listaRerCentral.Add(rerCentralDto);
                    if (filaOculta != null && filaOculta != "" && filaOculta != "null")
                    {
                        numColumnasXAnalizar++;
                    }
                }
                #endregion

                #region Validando que no se dupliquen los codigos de entrega en la fila oculta 
                bool existeCodEntregaDuplicados = listaRerCentral.GroupBy(e => e.Codentcodigo).Any(group => group.Count() > 1);
                if (existeCodEntregaDuplicados)
                {
                    throw new Exception("Por favor, no duplicar columnas en el archivo Excel. Descargue nuevamente la plantilla.");
                }
                #endregion

                #region Validacion de los datos ingresados
                for (int j = 2 + desplazamiento; j < numColumnasXAnalizar + 2 + desplazamiento; j++)        // Recorro las centrales (columnas)
                {
                    string nombreCodigoEntrega = ds.Tables[0].Rows[4][j].ToString().Trim();
                    string equinomb = ds.Tables[0].Rows[5][j].ToString().Trim();
                    string sCodiCodigoEntrega = ds.Tables[0].Rows[6][j].ToString().Trim();
                    string sEquicodi = ds.Tables[0].Rows[7][j].ToString().Trim();

                    #region Validamos que no este vacio ni contenga letras en las filas ocultas
                    if (string.IsNullOrEmpty(sCodiCodigoEntrega) || !int.TryParse(sCodiCodigoEntrega, out int codiCodigoEntrega))
                    {
                        // Lanzar una excepción con el mensaje adecuado
                        throw new Exception("Por favor, no manipular las filas 8 y 9. Descargar nuevamente la plantilla excel.");
                    }

                    if (string.IsNullOrEmpty(sEquicodi) || !int.TryParse(sEquicodi, out int equicodi))
                    {
                        // Lanzar una excepción con el mensaje adecuado
                        throw new Exception("Por favor, no manipular las filas 8 y 9. Descargar nuevamente la plantilla excel.");
                    }
                    #endregion

                    #region Validamos que el equicodi y el codigo de entrega se encuentren en listaCentral
                    RerCentralDTO centralValida = listaCentral.Where(item => (item.Equicodi == equicodi) && (item.Codentcodi == codiCodigoEntrega)).ToList().FirstOrDefault();
                    bool central_is_valid = (centralValida != null);
                    if (!central_is_valid)
                    {
                        throw new Exception("El equipo o código de entrega en la columna '" + ConvertirNumeroColumnaALetra(j + 1) + "' no se encuentran vigentes en el mes seleleccionado. Por favor, descargar nuevamente la plantilla excel.");
                    }
                    #endregion

                    for (int i = 8; i < numFilasXAnalizar + 8; i++)        // Recorro los intervalos (filas)
                    {
                        string sValorIntervalo = ds.Tables[0].Rows[i][j].ToString().Trim();
                        decimal dValorIntervalo = 0;

                        #region Validamos que el Valor total sea un valor decimal
                        if (sValorIntervalo == "null" || sValorIntervalo == "")
                        {
                            dValorIntervalo = 0;
                        }
                        else if (!decimal.TryParse(sValorIntervalo, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out dValorIntervalo))
                        {
                            sCeldaErrores.Add(ConvertirNumeroColumnaALetra(j + 1) + (i + 2).ToString());
                        }
                        else
                        {
                            dValorIntervalo = decimal.Parse(dValorIntervalo.ToString("0.########"));
                        }
                        #endregion
                    }
                }
                #endregion

                #region En caso que existan errores, lo muestro en pantalla
                if (sCeldaErrores.Count > 0)
                {
                    List<string> celdaErrores = sCeldaErrores.Select(numero => "'" + (numero) + "'").ToList();

                    string sCeldasError = string.Join(", ", celdaErrores);
                    throw new Exception("La(s) celdas(s) [" + sCeldasError + "] ingresada(s) no posee(n) un valor en decimal.");
                }
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw;
            }
        }

        #endregion

        #endregion

        static bool ValidarString(string input)
        {
            // Expresión regular para verificar si la cadena contiene solo letras de la A a la Z (mayúsculas o minúsculas)
            Regex regex = new Regex("[^a-zA-Z0-9 _.-]");

            // Verificar si el input coincide con la expresión regular
            return regex.IsMatch(input);
        }
        static string RemplazarN(string input)
        {
            // Expresión regular para verificar si la cadena contiene solo letras de la A a la Z (mayúsculas o minúsculas)
            Regex regex = new Regex("[^a-zA-Z0-9 _.-]");

            // Reemplazar cualquier caracter que no sea una letra A-Z, un espacio en blanco, un ".", o un número del 0 al 9 con 'Ñ'
            string resultado = regex.Replace(input, "Ñ");

            return resultado;
        }
    }

    #region Clases DTO Adicionales

    /// <summary>
    /// Entidad InsumoDTO
    /// </summary>
    public class InsumoDTO
    {
        public string NomInsumo { get; set; }
        public string FecUltImportacion { get; set; }
        public string LogInsumos { get; set; }
    }

    /// <summary>
    /// Entidad CargaManualDTO
    /// </summary>
    public class MesAnioTarifarioDTO
    {
        public int Id { get; set; }
        public int Rerpprcodi { get; set; }
        public string NomMesAnio { get; set; }
        public string Rerpprcodi_tipoInsumo { get; set; }
        public string TipoInsumo { get; set; }
        
        public string NomMes { get; set; }
        public string NomArchivo { get; set; }
        public string NomPlantilla { get; set; }
        public string Id_Insumo { get; set; }
    }

    /// <summary>
    /// Entidad ReporteDTO
    /// </summary>
    public class ReporteDTO
    {
        public int Id { get; set; }
        public string NomReporte { get; set; }
    }

    #endregion
}