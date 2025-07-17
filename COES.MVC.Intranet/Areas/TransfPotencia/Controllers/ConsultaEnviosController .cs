using System;
using System.Web.Mvc;
using log4net;
using System.Reflection;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.Servicios.Aplicacion.Helper;
using System.Collections.Generic;
using COES.MVC.Intranet.Helper;
using System.Globalization;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using System.Configuration;
using System.Linq;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class ConsultaEnviosController : BaseController
    {
        // GET: /TransfPotencia/ConsultaEnvios/
        public ConsultaEnviosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        ExportarExcelGAppServicio servicioExportarExcel = new ExportarExcelGAppServicio();
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0, string plazo = "", string liquidacion = "")
        {
            base.ValidarSesionUsuario();
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            Log.Info("Lista Periodos - ListaPeriodos");
            model.ListaPeriodos = servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                Log.Info("Entidad Periodo - GetByIdPeriodo");
                model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                Log.Info("Lista Recalculos - ListByPericodiVtpRecalculoPotencia");
                model.ListaRecalculos = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculos.Count > 0 && recpotcodi == 0)
                { recpotcodi = model.ListaRecalculos[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                Log.Info("Entidad Recalculo - GetByIdVtpRecalculoPotencia");
                model.EntidadRecalculo = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
            }
            else
            {
                model.EntidadRecalculo = new VtpRecalculoPotenciaDTO();
            }
            Log.Info("Lista Empresas - ListaEmpresasCombo");
            model.ListaEmpresas = servicioEmpresa.ListEmpresas();

            model.pericodi = pericodi;
            model.recpotcodi = recpotcodi;
            model.emprcodi = emprcodi;
            model.plazo = plazo;
            model.liquidacion = liquidacion;

            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int pericodi, int recpotcodi, int emprcodi, string plazo, string liquidacion)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            Log.Info("Lista Presupuestos - ListVtpPeajeEgresosView");
            model.ListaPeajeEgreso = this.servicioTransfPotencia.ListVtpPeajeEgresosConsulta(pericodi, recpotcodi, emprcodi, plazo, liquidacion);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra un pop up donde se visulizara el detalle del envio
        /// </summary>
        /// <param name="id">Numero de envio</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult View(int id = 0)
        {
            GridExcelModel modelo = new GridExcelModel();
            modelo.pegrcodi = id;
            return PartialView(modelo);
        }

        ///// <summary>
        ///// Muestra la grilla excel con los datos enviados pegrcodi
        ///// </summary>
        ///// <param name="pegrcodi">Numero de envio</param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult MostrarExcelWeb(int pegrcodi)
        //{

        //    GridExcelModel model = new GridExcelModel();
        //    PeajeEgresoModel modelpe = new PeajeEgresoModel();
        //    try
        //    {
        //        //Definimos la cabecera como una matriz
        //        string[] Cabecera1 = { "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
        //        string[] Cabecera2 = { "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };

        //        //Ancho de cada columna
        //        int[] widths = { 140, 100, 55, 65, 80, 60, 70, 70, 80, 100, 60, 60, 60 };
        //        object[] columnas = new object[13];

        //        bool pegrestado = true;
        //        string[][] data;
        //        modelpe.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi);
        //        if (modelpe.EntidadPeajeEgreso != null)
        //        {
        //            modelpe.ListaPeajeEgresoDetalle = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoDetalles(modelpe.EntidadPeajeEgreso.Pegrcodi);
        //            data = new string[modelpe.ListaPeajeEgresoDetalle.Count + 2][];
        //            data[0] = Cabecera1;
        //            data[1] = Cabecera2;
        //            int index = 2;
        //            model.NumRegistros = modelpe.ListaPeajeEgresoDetalle.Count;
        //            foreach (VtpPeajeEgresoDetalleDTO item in modelpe.ListaPeajeEgresoDetalle)
        //            {
        //                string[] itemDato = { item.Emprnomb, item.Barrnombre, item.Pegrdtipousuario, item.Pegrdlicitacion.ToString(), item.Pegrdpreciopote.ToString(), item.Pegrdpoteegreso.ToString(), item.Pegrdpotecalculada.ToString(), item.Pegrdpotedeclarada.ToString(), item.Pegrdpeajeunitario.ToString(), item.Barrnombrefco, item.Pegrdpoteactiva.ToString(), item.Pegrdpotereactiva.ToString(), item.Pegrdcalidad };
        //                data[index] = itemDato;
        //                index++;
        //            }
        //            #region formato la grilla Excel
        //            columnas[0] = new
        //            {   //Emprcodi - Emprnomb
        //                type = GridExcelModel.TipoTexto,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                readOnly = pegrestado,
        //            };
        //            columnas[1] = new
        //            {   //Barrcodi - Barrnombre
        //                type = GridExcelModel.TipoTexto,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                readOnly = pegrestado,
        //            };
        //            columnas[2] = new
        //            {   //Rpsctipousuario
        //                type = GridExcelModel.TipoTexto,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                readOnly = pegrestado,
        //            };
        //            columnas[3] = new
        //            {   //Licitación
        //                type = GridExcelModel.TipoTexto,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                readOnly = pegrestado,
        //            };
        //            columnas[4] = new
        //            {   //Precio Potencia
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[5] = new
        //            {   //Potencia Egreso
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000000000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[6] = new
        //            {   //Potencia Calculada
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000000000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[7] = new
        //            {   //Potencia Declarada
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000000000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[8] = new
        //            {   //Peaje Unitario
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[9] = new
        //            {   //Barrcodifco - Barrnombrefco
        //                type = GridExcelModel.TipoTexto,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                readOnly = pegrestado,
        //            };
        //            columnas[10] = new
        //            {   //Rpscpoteactiva
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000000000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[11] = new
        //            {   //Rpscpotereactiva
        //                type = GridExcelModel.TipoNumerico,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                className = "htRight",
        //                format = "0,0.0000000000",
        //                readOnly = pegrestado,
        //            };
        //            columnas[12] = new
        //            {   //Rpsccalidad
        //                type = GridExcelModel.TipoTexto,
        //                source = (new List<String>()).ToArray(),
        //                strict = false,
        //                correctFormat = true,
        //                readOnly = pegrestado,
        //            };
        //            #endregion
        //            model.Grabar = pegrestado;
        //            model.Data = data;
        //            model.Widths = widths;
        //            model.Columnas = columnas;
        //            model.FixedRowsTop = 2;
        //            model.FixedColumnsLeft = 2;
        //        }
        //        return Json(model);
        //    }
        //    catch (Exception e)
        //    {
        //        model.MensajeError = e.Message + "<br><br>" + e.StackTrace;
        //        return Json(model);
        //    }
        //}



        /// <summary>
        /// Muestra la grilla excel con los datos enviados pegrcodi
        /// </summary>
        /// <param name="pegrcodi">Numero de envio</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarExcelWeb(int pegrcodi, int pericodi, int emprcodi)
        {
            base.ValidarSesionUsuario();
            PeriodoDTO periodo = new PeriodoAppServicio().GetByIdPeriodo(pericodi);
            GridExcelModel model = null;
            if (periodo.PeriFormNuevo == 1)
            {
                model = mpGrillaFormatoNuevo(pegrcodi, emprcodi);
                model.esFormatoNuevo = periodo.PeriFormNuevo;
            }
            else
            {
                model = mpGrillaFormatoAntiguo(pegrcodi);
                model.esFormatoNuevo = periodo.PeriFormNuevo;
            }

            return Json(model);
        }



        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pegrcodi">Id del envío</param>
        /// <param name="formato">Tipo de archivo a exportar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi, int recpotcodi, int pegrcodi, int emprcodi, int formato = 1)
        {
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioTransfPotencia.GenerarFormatoPeajeEgresoEnvio(pericodi, recpotcodi, emprcodi, pegrcodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite exportar a un archivo todas las validaciones
        /// </summary>
        /// <param name="pegrcodi">Código del Envío</param>
        /// <param name="formato">Formato del Archivo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataValidaciones(int pericodi = 0, int recpotcodi = 0, int pegrcodi = 0, int formato = 1, string enterprisename = "")
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioTransfPotencia.GenerarFormatoValidacionesEnvio(pericodi, recpotcodi, pegrcodi, enterprisename, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Descarga el archivo
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Muestra un pop up donde se visulizara la lista de envios seleccionados
        /// </summary>
        /// <param name="pericodi">Ide de periodo</param>
        /// <param name="recpotcodi">Id de recalculo</param>
        /// <param name="items">Lista de Envios</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarSeleccionados(int pericodi, int recpotcodi, string items)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            model.sError = "";
            model.iNumReg = 0;

            try
            {
                if (pericodi > 0 && recpotcodi > 0)
                {
                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                    Log.Info("Entidad Recalculo - GetByIdRecalculo");
                    model.EntidadRecalculo = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);

                    model.ListaPeajeEgreso = new List<VtpPeajeEgresoDTO>();

                    //Actualizamos la información, colocando en SI, solo a la lista de items seleccionados
                    string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
                    foreach (string Id in Ids)
                    {
                        int pegrcodi = Convert.ToInt32(Id);
                        if (pegrcodi > 0)
                        {
                            Log.Info("Actualizamos la información - UpdateVcrUnidadexoneradaVersionSI");
                            VtpPeajeEgresoDTO dto = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi);
                            if (dto != null)
                            {
                                model.ListaPeajeEgreso.Add(dto);
                                model.iNumReg++;
                            }
                        }
                    }
                }
                else
                    model.sError = "Debe seleccionar un periodo y versión correcto";
            }
            catch (Exception e)
            {
                model.sError = e.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Muestra un pop up donde se visulizara la lista de envios seleccionados
        /// </summary>
        /// <param name="pericodi">Ide de periodo</param>
        /// <param name="recpotcodi">Id de recalculo</param>
        /// <param name="items">Lista de Envios</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SiLiquidacion(int pericodi, int recpotcodi, string items)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            model.sError = "";
            model.iNumReg = 0;

            try
            {
                Log.Info("Entidad Periodo - GetByIdPeriodo");
                model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                Log.Info("Entidad Recalculo - GetByIdRecalculo");
                model.EntidadRecalculo = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);

                //Actualizamos la información, colocando en SI, solo a la lista de items seleccionados
                string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
                foreach (string Id in Ids)
                {
                    int pegrcodi = Convert.ToInt32(Id);
                    if (pegrcodi > 0)
                    {
                        Log.Info("Estraemos el envio - GetByIdTrnEnvio");
                        VtpPeajeEgresoDTO dtoPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi);
                        if (dtoPeajeEgreso != null)
                        {
                            int iEmprcodi = (int)dtoPeajeEgreso.Emprcodi;
                            //Todos los envios VtpPeajeEgreso (pericodi, recpotcodi, emprcodi) la dtoPeajeEgreso.Pegrestado <- NO
                            Log.Info("Todos los VtpPeajeEgreso (pericodi, recpotcodi, emprcodi) la dtoPeajeEgreso.Pegrestado <- NO - UpdateByCriteriaVtpPeajeEgreso");
                            this.servicioTransfPotencia.UpdateByCriteriaVtpPeajeEgreso(iEmprcodi, pericodi, recpotcodi);

                            //El actual envio estará en liquidación
                            dtoPeajeEgreso.Pegrestado = "SI";
                            dtoPeajeEgreso.Pegrusumodificacion = User.Identity.Name;
                            dtoPeajeEgreso.Pegrfecmodificacion = DateTime.Now;
                            Log.Info("UpdatePeajeEgreso- UpdateVtpPeajeEgreso");
                            this.servicioTransfPotencia.UpdateVtpPeajeEgreso(dtoPeajeEgreso);
                            model.iNumReg++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                model.sError = e.Message;
                return Json(model);
            }
            return Json(model);
        }

        #region privates
        private GridExcelModel mpGrillaFormatoNuevo(int pegrcodi, int emprcodi)
        {


            GridExcelModel model = new GridExcelModel();
            PeajeEgresoModel modelpe = new PeajeEgresoModel();

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "Codigo", "Cliente", "Barra", "Contrato", "Tipo Usuario", "Precio Potencia S/ /kW-mes", "Potencia Coincidente kW", "Potencia Declarada kW", "Peaje Unitario /KW mes", "Factor pérdida", "Calidad" };

            //Ancho de cada columna
            int[] widths = { 120, 120, 120, 80, 80, 120, 120, 120, 120, 80, 80 };
            object[] columnas = new object[11];

            bool pegrestado = false;
            bool pegrestadocalculo = false;


            //Se arma la matriz de datos
            string[][] data;


            //Es un envio que ya esta INACTIVO - no es el ultimo
            pegrestado = true;  //Deshabilita los botones para que grabe o realice cualquier otra acción

            modelpe.ListaPeajeEgresoDetalle = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoDetalles(pegrcodi);
            data = new string[modelpe.ListaPeajeEgresoDetalle.Count() + 1][];
            data[0] = Cabecera1;
            int index = 1;
            model.NumRegistros = modelpe.ListaPeajeEgresoDetalle.Count;
            foreach (VtpPeajeEgresoDetalleDTO item in modelpe.ListaPeajeEgresoDetalle)
            {
                string[] itemDato = { item.Coregecodvtp ?? "",
                    item.Emprnomb ?? "",
                    item.Barrnombre?.ToString(),//si no es null lo convierte a string
                    item.TipConNombre ??  item.Pegrdlicitacion?.ToString(),
                    item.Pegrdtipousuario,
                    item.Pegrdpreciopote?.ToString("N2"),
                    (item.Pegrdpotecoincidente??0)==0?(item.Pegrdpoteegreso?.ToString("N2")):item.Pegrdpotecoincidente?.ToString("N2"),
                    item.Pegrdpotedeclarada?.ToString("N2"),
                    item.Pegrdpeajeunitario?.ToString("N2"),
                    item.Pegrdfacperdida?.ToString("N2"),
                    item.Pegrdcalidad?.ToString() };
                data[index] = itemDato;
                index++;
            }


            ///////////          
            string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
            string[] aLicitacion = { "Si", "No" };
            string[] aCalidad = { "Final", "Preliminar" };

            columnas[0] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[1] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoLista,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[2] = new
            {   //Barrcodi - Barrnombre
                type = GridExcelModel.TipoLista,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[3] = new
            {   //Licitación
                type = GridExcelModel.TipoLista,
                source = aLicitacion,
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[4] = new
            {   //Rpsctipousuario
                type = GridExcelModel.TipoLista,
                source = aTipoUsuario,
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[5] = new
            {   //Precio Potencia
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = pegrestadocalculo,
            };
            columnas[6] = new
            {   //Potencia Egreso
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = pegrestadocalculo,
            };
            columnas[7] = new
            {   //Potencia Calculada
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = pegrestadocalculo,
            };
            columnas[8] = new
            {   //Potencia Declarada
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = pegrestadocalculo,
            };
            columnas[9] = new
            {   //Barrcodifco - Barrnombrefco
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = pegrestadocalculo,
            };
            columnas[10] = new
            {   //Rpscpoteactiva
                type = GridExcelModel.TipoTexto,
                source = "",
                strict = false,
                correctFormat = true,
                readOnly = pegrestadocalculo,
            };

            #endregion

            model.ListaEmpresas = ListaEmpresas.ToArray();

            model.ListaLicitacion = aLicitacion.ToArray();
            model.ListaCalidad = aCalidad.ToArray();
            model.ListaTipoUsuario = aTipoUsuario.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.FixedRowsTop = 1;
            model.FixedColumnsLeft = 1;
            //ASSETEC 20190219
            return model;

        }

        private GridExcelModel mpGrillaFormatoAntiguo(int pegrcodi)
        {

            GridExcelModel model = new GridExcelModel();
            PeajeEgresoModel modelpe = new PeajeEgresoModel();
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
            string[] Cabecera2 = { "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };

            //Ancho de cada columna
            int[] widths = { 140, 100, 55, 65, 80, 60, 70, 70, 80, 100, 60, 60, 60 };
            object[] columnas = new object[13];

            bool pegrestado = true;
            string[][] data;
            modelpe.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi);
            if (modelpe.EntidadPeajeEgreso != null)
            {
                int emprcodi = modelpe.EntidadPeajeEgreso.Emprcodi == null ? 0 : (Int32)modelpe.EntidadPeajeEgreso.Emprcodi;
                modelpe.ListaPeajeEgresoDetalle = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoDetalles(modelpe.EntidadPeajeEgreso.Pegrcodi);
                data = new string[modelpe.ListaPeajeEgresoDetalle.Count + 2][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                int index = 2;
                model.NumRegistros = modelpe.ListaPeajeEgresoDetalle.Count;
                foreach (VtpPeajeEgresoDetalleDTO item in modelpe.ListaPeajeEgresoDetalle)
                {
                    string[] itemDato = { item.Emprnomb, item.Barrnombre, item.Pegrdtipousuario, item.Pegrdlicitacion.ToString(), item.Pegrdpreciopote.ToString(), item.Pegrdpoteegreso.ToString(), item.Pegrdpotecalculada.ToString(), item.Pegrdpotedeclarada.ToString(), item.Pegrdpeajeunitario.ToString(), item.Barrnombrefco, item.Pegrdpoteactiva.ToString(), item.Pegrdpotereactiva.ToString(), item.Pegrdcalidad };
                    data[index] = itemDato;
                    index++;
                }
                #region formato la grilla Excel
                columnas[0] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[1] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[2] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[3] = new
                {   //Licitación
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[4] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = pegrestado,
                };
                columnas[5] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[6] = new
                {   //Potencia Calculada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[7] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[8] = new
                {   //Peaje Unitario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = pegrestado,
                };
                columnas[9] = new
                {   //Barrcodifco - Barrnombrefco
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[10] = new
                {   //Rpscpoteactiva
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[11] = new
                {   //Rpscpotereactiva
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[12] = new
                {   //Rpsccalidad
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                #endregion
                model.Grabar = pegrestado;
                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 2;
                model.FixedColumnsLeft = 2;
            }
            return model;
        }
        #endregion privates
    }
}