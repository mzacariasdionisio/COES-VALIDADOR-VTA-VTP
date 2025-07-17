using System;
using System.Web.Mvc;
using log4net;
using System.Reflection;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Helper;
using System.Collections.Generic;
using COES.MVC.Intranet.Helper;
using System.Globalization;
using COES.MVC.Intranet.Areas.Transferencias.Helper;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class ConsultaEnviosController : BaseController
    {
        // GET: /Transferencias/ConsultaEnvios/
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
        TransferenciasAppServicio servicioTransferencias = new TransferenciasAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        ExportarExcelGAppServicio servicioExportarExcel = new ExportarExcelGAppServicio();
        public ActionResult Index(int pericodi = 0, int recacodi = 0, int emprcodi = 0, int trnenvtipinf = 0, string trnenvplazo = "", string trnenvliqvt = "")
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

                Log.Info("Lista Recalculos - ListaRecalculos");
                model.ListaRecalculos = this.servicioRecalculo.ListRecalculos(pericodi); //Ordenado en descendente
                if (model.ListaRecalculos.Count > 0 && recacodi == 0)
                { recacodi = model.ListaRecalculos[0].RecaCodi; }
            }

            if (pericodi > 0 && recacodi > 0)
            {
                Log.Info("Entidad Recalculo - GetByIdRecalculo");
                model.EntidadRecalculo = this.servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
            }
            else
            {
                model.EntidadRecalculo = new RecalculoDTO();
            }
            Log.Info("Lista Empresas - ListaEmpresasCombo");
            model.ListaEmpresas = servicioEmpresa.ListEmpresas();
            model.ListaTipoInfo = ListTipoInfo();

            model.pericodi = pericodi;
            model.recacodi = recacodi;
            model.emprcodi = emprcodi;
            model.trnenvtipinf = trnenvtipinf;
            model.trnenvplazo = trnenvplazo;
            model.trnenvliqvt = trnenvliqvt;

            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista(int pericodi, int recacodi, int emprcodi, int trnenvtipinf, string trnenvplazo, string trnenvliqvt)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            Log.Info("Lista Presupuestos - ListCaiPresupuestos");
            string sTipoInfoCodigo = "X";
            if (trnenvtipinf >= 0) sTipoInfoCodigo = TipoInformacion(trnenvtipinf);
            model.ListaTrnEnvios = this.servicioTransferencias.ListarTrnEnvioIntranet(pericodi, recacodi, emprcodi, sTipoInfoCodigo, trnenvplazo, trnenvliqvt);
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
            GrillaExcelModel modelo = new GrillaExcelModel();
            modelo.Trnenvcodi = id;
            return PartialView(modelo);
        }

        /// <summary>
        /// Muestra la grilla excel con los datos enviados trnenvcodi
        /// </summary>
        /// <param name="trnenvcodi">Numero de envio</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarExcelWeb(int trnenvcodi)
        {

            GrillaExcelModel model = new GrillaExcelModel();

            string testing = "";
            try
            {               
                EnvioInformacionModel modelEI = new EnvioInformacionModel();
                modelEI.EntidadEnvio = servicioTransferencias.GetByIdTrnEnvio(trnenvcodi);
                int trnmodcodi = modelEI.EntidadEnvio.TrnModCodi;
                int pericodi = modelEI.EntidadEnvio.PeriCodi;
                int recacodi = modelEI.EntidadEnvio.RecaCodi;
                int emprcodi = modelEI.EntidadEnvio.EmprCodi;
                model.TrnEnvFecIns = modelEI.EntidadEnvio.TrnEnvFecIns.ToString("dd/MM/yyyy HH:mm:ss").ToString();

                modelEI.EntidadPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                modelEI.EntidadRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                modelEI.EntidadEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);
                //Buacamos todos los codigos de información base que estan validos para el periodo seleccionado
                string sTipoInformacion = modelEI.EntidadEnvio.TrnEnvTipInf;
                if (sTipoInformacion == "ER")
                    modelEI.ListaEntregReti = servicioExportarExcel.BuscarCodigoRetiroVistaTodo(pericodi, emprcodi, 0);
                else if (sTipoInformacion == "IB")
                    modelEI.ListaEntregReti = servicioExportarExcel.GetByListCodigoInfoBase(pericodi, emprcodi);
                else if (sTipoInformacion == "DM")
                    modelEI.ListaEntregReti = servicioExportarExcel.GetByListCodigoModeloVTA(pericodi, emprcodi, trnmodcodi);
                else
                {
                    model.MensajeError = "No se ha relacionado correctamente con el tipo de información";
                    return Json(model);
                }

                int iNroColumnas = modelEI.ListaEntregReti.Count;

                #region Armando la grilla Excel
                //Se arma la matriz de datos
                string[][] data;
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "EMPRESA" }; //Titulos de cada columna
                string[] Cabecera2 = { "BARRA" };
                string[] Cabecera3 = { "CÓDIGO" };
                string[] Cabecera4 = { "FECHA / UNIDAD" };
                int[] widths = { 120 }; //Ancho de cada columna
                string[] itemDato = { "" };
                int iFila = 4;
                //La lista de barras es dinamica
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                    Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                    Array.Resize(ref Cabecera3, Cabecera3.Length + iNroColumnas);
                    Array.Resize(ref Cabecera4, Cabecera4.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                    //Formato de columnas
                    object[] columnas = new object[iNroColumnas + 1];
                    int iColumna = 0;
                    //Formateamos la primera columna
                    columnas[iColumna++] = new
                    {   //INTERVALO DE 15 MINUTOS
                        type = GrillaExcelModel.TipoTexto,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        readOnly = true,
                    };
                    //Columna de fechas
                    string sMes = modelEI.EntidadPeriodo.MesCodi.ToString();
                    if (sMes.Length == 1) sMes = "0" + sMes;
                    var Fecha = "01/" + sMes + "/" + modelEI.EntidadPeriodo.AnioCodi;
                    var dates = new List<DateTime>();
                    var dateIni = DateTime.ParseExact(Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);  //Convert.ToDateTime(Fecha);
                    var dateFin = dateIni.AddMonths(1);
                    dateIni = dateIni.AddMinutes(15);
                    for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                    {
                        dates.Add(dt);
                    }                    
                    data = new string[dates.Count + iFila][]; //Numero de filas de la hoja de datos modelEI.ListaEntregReti.Count()
                    
                    if (modelEI.ListaEntregReti.Count > 0)
                    {
                        foreach (var day in dates)
                        {
                            //Para cada intervalo de 15 minutos en todos los dias del mes
                            string[] itemFila = { day.ToString("dd/MM/yyyy HH:mm") };
                            Array.Resize(ref itemFila, itemFila.Length + iNroColumnas);
                            for (int i = 1; i < iNroColumnas; i++)
                            {
                                itemFila[i] = "";
                            }
                            data[iFila] = itemFila;
                            iFila++;
                        }
                    }

                    //Registramos las cabeceras de las siguientes columnas
                    foreach (ExportExcelDTO dto in modelEI.ListaEntregReti)
                    {
                        Cabecera1[iColumna] = dto.EmprNomb;
                        Cabecera2[iColumna] = dto.BarrNombBarrTran;
                        if (sTipoInformacion == "ER" || sTipoInformacion == "DM")
                        {
                            Cabecera3[iColumna] = dto.CodiEntreRetiCodigo;
                        }
                        else if (sTipoInformacion == "IB")
                        {
                            Cabecera3[iColumna] = dto.CoInfbCodigo;
                        }
                        Cabecera4[iColumna] = "MWh";
                        widths[iColumna] = 100;
                        itemDato[iColumna] = "";
                        columnas[iColumna] = new
                        {   //R(pu)
                            type = GrillaExcelModel.TipoNumerico,
                            source = (new List<String>()).ToArray(),
                            strict = false,
                            correctFormat = true,
                            className = "htRight",
                            format = "0,0.000000000000",
                            readOnly = true,
                        };
                        //Traemos la data de los codigos, en caso existiese para almacenarlo en una matriz de codigos
                        if (sTipoInformacion == "ER" || sTipoInformacion == "DM")
                        {
                            if (dto.Tipo.Equals("Entrega"))
                            {
                                modelEI.EntidadEntrega = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (modelEI.EntidadEntrega != null)
                                {
                                    Cabecera4[iColumna] += " [" + modelEI.EntidadEntrega.TranEntrEstado + "]";
                                    int iFilCodigo = 4;
                                    int iColCodigo = iColumna;
                                    //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                    modelEI.ListaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaEntrega(modelEI.EntidadEntrega.TranEntrCodi, modelEI.EntidadEntrega.TranEntrVersion);
                                    foreach (TransferenciaEntregaDetalleDTO dtoDetalle in modelEI.ListaEntregaDetalle)
                                    {
                                        if (data.Length <= iFilCodigo)
                                            break;

                                        for (int k = 1; k <= 96; k++)
                                        {
                                            data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TranEntrDetah" + k).GetValue(dtoDetalle, null).ToString();
                                        }
                                    }
                                }
                            }
                            else if (dto.Tipo.Equals("Retiro"))
                            {
                                modelEI.EntidadRetiro = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (modelEI.EntidadRetiro != null) //&& iColumna < 55
                                {
                                    Cabecera4[iColumna] += " [" + modelEI.EntidadRetiro.TranRetiEstado + "]";
                                    int iFilCodigo = 4;
                                    int iColCodigo = iColumna;
                                    
                                    //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                    modelEI.ListaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(modelEI.EntidadRetiro.TranRetiCodi, modelEI.EntidadRetiro.TranRetiVersion);
                                    foreach (TransferenciaRetiroDetalleDTO dtoDetalle in modelEI.ListaRetiroDetalle)
                                    {
                                        try
                                        {
                                            if (data.Length <= iFilCodigo) 
                                                break;

                                            for (int k = 1; k <= 96; k++)
                                            {
                                                testing = "TranRetiDetah" + k;
                                                data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TranRetiDetah" + k).GetValue(dtoDetalle, null).ToString();
                                            }
                                        }
                                        catch (Exception)
                                        {

                                            throw new Exception(string.Format("{0}-{1}-{2}-{3}", dtoDetalle.Emprnomb, dtoDetalle.SoliCodiRetiCodigo, testing, iFilCodigo));
                                        }

                                    }
                                }
                            }
                            else if (dto.Tipo.Equals("Modelo"))
                            {
                                //El generador, a quien pertenece el código
                                int genemprcodi = dto.EmprCodi;
                                modelEI.EntidadRetiro = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigoEnvio(genemprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (modelEI.EntidadRetiro != null) //&& iColumna < 55
                                {
                                    Cabecera4[iColumna] += " [" + modelEI.EntidadRetiro.TranRetiEstado + "]";
                                    int iFilCodigo = 4;
                                    int iColCodigo = iColumna;
                                    //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                    List<TransferenciaRetiroDetalleDTO> ListaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(modelEI.EntidadRetiro.TranRetiCodi, modelEI.EntidadRetiro.TranRetiVersion);
                                    foreach (TransferenciaRetiroDetalleDTO dtoDetalle in ListaRetiroDetalle)
                                    {
                                        if (data.Length <= iFilCodigo)
                                            break;

                                        for (int k = 1; k <= 96; k++)
                                        {
                                            testing = "iFilCodigo:" + iFilCodigo + " iColCodigo:" + iColCodigo + "TranRetiDetah" + k;
                                            data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TranRetiDetah" + k).GetValue(dtoDetalle, null).ToString();                                                                               
                                        }
                                    }
                                }
                            }

                        }
                        else if (sTipoInformacion == "IB")
                        {
                            modelEI.EntidadInformacionBase = (new TransferenciaInformacionBaseAppServicio()).GetTransferenciaInfoBaseByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CoInfbCodigo);
                            if (modelEI.EntidadInformacionBase != null)
                            {
                                int iFilCodigo = 4;
                                int iColCodigo = iColumna;
                                //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                modelEI.ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).ListByTransferenciaInformacionBase(modelEI.EntidadInformacionBase.TinfbCodi);
                                foreach (TransferenciaInformacionBaseDetalleDTO dtoDetalle in modelEI.ListaInformacionBaseDetalle)
                                {
                                    if (data.Length <= iFilCodigo)
                                        break;

                                    for (int k = 1; k <= 96; k++)
                                    {
                                        data[iFilCodigo++][iColCodigo] = dtoDetalle.GetType().GetProperty("TinfbDe" + k).GetValue(dtoDetalle, null).ToString();
                                    }
                                }
                            }
                        }
                        iColumna++;
                    }
                    if (modelEI.ListaEntregReti.Count > 0)
                    {
                        data[0] = Cabecera1;
                        data[1] = Cabecera2;
                        data[2] = Cabecera3;
                        data[3] = Cabecera4;
                    }
                    else
                    {
                        data = new string[5][];
                        data[0] = Cabecera1;
                        data[1] = Cabecera2;
                        data[2] = Cabecera3;
                        data[3] = Cabecera4;
                        data[iFila] = itemDato;
                    }
                    model.Data = data;
                    model.Widths = widths;
                    model.Columnas = columnas;
                    model.FixedRowsTop = 4;
                    model.FixedColumnsLeft = 1;
                    model.NroColumnas = iNroColumnas;
                    model.Trnenvcodi = trnenvcodi;
                    model.Trnmodcodi = trnmodcodi;
                    model.LimiteMaxEnergia = (decimal)Funcion.dLimiteMaxEnergia;
                }
                #endregion
                
                var JsonResult = Json(model);
                JsonResult.MaxJsonLength = Int32.MaxValue;
                return JsonResult;
            }
            catch (Exception e)
            {
                model.MensajeError = "testing:" + testing  + "/" + e.Message + "<br><br>" + e.StackTrace;
                return Json(model);
            }
        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="trnenvcodi">Id del envío</param>
        /// <param name="formato">Tipo de archivo a exportar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int trnenvcodi, int formato = 1)
        {
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte;
                string file = "";

                EnvioInformacionModel modelEI = new EnvioInformacionModel();
                modelEI.EntidadEnvio = servicioTransferencias.GetByIdTrnEnvio(trnenvcodi);
                int trnmodcodi = modelEI.EntidadEnvio.TrnModCodi;
                int pericodi = modelEI.EntidadEnvio.PeriCodi;
                int recacodi = modelEI.EntidadEnvio.RecaCodi;
                int emprcodi = modelEI.EntidadEnvio.EmprCodi;
                string sTipoInformacion = modelEI.EntidadEnvio.TrnEnvTipInf;
                if (sTipoInformacion == "ER")
                {
                    file = this.servicioTransferencias.GenerarFormatoEntregaRetiro(pericodi, recacodi, emprcodi, trnenvcodi, formato, pathFile, pathLogo, 1);
                }
                else if (sTipoInformacion == "IB")
                {
                    file = this.servicioTransferencias.GenerarFormatoInfoBase(pericodi, recacodi, emprcodi, trnenvcodi, formato, pathFile, pathLogo);
                }
                else if (sTipoInformacion == "DM")
                {
                    file = this.servicioTransferencias.GenerarFormatoModelo(pericodi, recacodi, emprcodi, trnenvcodi, trnmodcodi, formato, pathFile, pathLogo);
                }
                return Json(file);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = AppDomain.CurrentDomain.BaseDirectory + Funcion.RutaReporte + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;
            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Muestra un pop up donde se visulizara la lista de envios seleccionados
        /// </summary>
        /// <param name="pericodi">Ide de periodo</param>
        /// <param name="recacodi">Id de recalculo</param>
        /// <param name="items">Lista de Envios</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarSeleccionados(int pericodi, int recacodi, string items)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            model.sError = "";
            model.iNumReg = 0;

            try
            {
                if (pericodi > 0 && recacodi > 0)
                {
                    Log.Info("Entidad Periodo - GetByIdPeriodo");
                    model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                    Log.Info("Entidad Recalculo - GetByIdRecalculo");
                    model.EntidadRecalculo = this.servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);

                    model.ListaTrnEnvios = new List<TrnEnvioDTO>();

                    //Actualizamos la información, colocando en SI, solo a la lista de items seleccionados
                    string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
                    foreach (string Id in Ids)
                    {
                        int trnenvcodi = Convert.ToInt32(Id);
                        if (trnenvcodi > 0)
                        {
                            Log.Info("Actualizamos la información - UpdateVcrUnidadexoneradaVersionSI");
                            TrnEnvioDTO dto = this.servicioTransferencias.GetByIdTrnEnvio(trnenvcodi);
                            if (dto != null)
                            {
                                model.ListaTrnEnvios.Add(dto);
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
        /// <param name="recacodi">Id de recalculo</param>
        /// <param name="items">Lista de Envios</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SiLiquidacion(int pericodi, int recacodi, string items)
        {
            ConsultaEnviosModel model = new ConsultaEnviosModel();
            model.sError = "";
            model.iNumReg = 0;

            try
            {
                Log.Info("Entidad Periodo - GetByIdPeriodo");
                model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                Log.Info("Entidad Recalculo - GetByIdRecalculo");
                model.EntidadRecalculo = this.servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);

                //Actualizamos la información, colocando en SI, solo a la lista de items seleccionados
                string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
                foreach (string Id in Ids)
                {
                    int trnenvcodi = Convert.ToInt32(Id);
                    if (trnenvcodi != 0)
                    {
                        Log.Info("Estraemos el envio - GetByIdTrnEnvio");
                        TrnEnvioDTO dtoTrnEnvio = this.servicioTransferencias.GetByIdTrnEnvio(trnenvcodi);
                        if (dtoTrnEnvio != null)
                        {
                            //Todos los envios TrnEnvTipInf:ER/DM (pericodi, recacodi, emprcodi, trnmodcodi) la dtoTrnEnvio.TrnEnvLiqVt <- N
                            Log.Info("Todos los envios TrnEnvTipInf:ER/DM (pericodi, recacodi, emprcodi) la dtoTrnEnvio.TrnEnvLiqVt <- N - UpdateByCriteriaTrnEnvio");
                            this.servicioTransferencias.UpdateByCriteriaTrnEnvio(pericodi, recacodi, dtoTrnEnvio.EmprCodi, dtoTrnEnvio.TrnModCodi, dtoTrnEnvio.TrnEnvTipInf, User.Identity.Name);

                            //El actual envio esta en liquidación
                            dtoTrnEnvio.TrnEnvLiqVt = "S";
                            dtoTrnEnvio.TrnEnvUseAct = User.Identity.Name;
                            dtoTrnEnvio.TrnEnvFecAct = DateTime.Now;
                            Log.Info("UpdateTrnEnvio - UpdateTrnEnvio");
                            int iResultado = this.servicioTransferencias.UpdateTrnEnvio(dtoTrnEnvio);

                            //Procedemos a grabar el detalle del envio [ER / DM]
                            if (dtoTrnEnvio.TrnEnvTipInf == "ER")
                            {
                                //En Entregas y Retiros el envio relacionado a la empresa, es la misma empresa en los TRN_TRANS_ENTREGA y TRN_TRANS_RETIRO 
                                Log.Info("En Entregas y Retiros el envio relacionado a la empresa - UpdateTrnEnvioLiquidacion");
                                servicioTransferencias.UpdateTrnEnvioLiquidacion(dtoTrnEnvio);
                            }
                            else if (dtoTrnEnvio.TrnEnvTipInf == "DM")
                            {
                                //Para el caso de DM, el envio le pertenece a una empresa, pero la info de TRN_TRANS_ENTREGA y TRN_TRANS_RETIRO le pertenece a otra empresa
                                //Lista de entregas y retiros
                                Log.Info("Para el caso de DM, el envio le pertenece a una empresa - GetByListCodigoModelo");
                                model.ListaEntregReti = servicioExportarExcel.GetByListCodigoModeloVTA(pericodi, dtoTrnEnvio.EmprCodi, dtoTrnEnvio.TrnModCodi);
                                foreach (ExportExcelDTO dtoRetiro in model.ListaEntregReti)
                                {
                                    dtoRetiro.PeriCodi = pericodi;
                                    dtoRetiro.VtranVersion = recacodi;
                                    //dtoRetiro.codentcodi -> tiene el ID del codigo de retiro
                                    //Todas los retiros relacionadas a pericodi, recacodi, empresa y TrnModCodi del retiro debe cambiar el estado tretestado = 'INA'
                                    //Para este retiro el estado tretestado = 'ACT'
                                    Log.Info("Todas los retiros relacionadas a pericodi, recacodi y la empresa del retiro - UpdateRetiroLiquidacion");
                                    servicioTransferencias.UpdateRetiroLiquidacion(dtoRetiro, dtoTrnEnvio.TrnEnvCodi, dtoTrnEnvio.TrnModCodi, User.Identity.Name);
                                }
                            }
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

        /// Permite listar los tipos de información
        public List<TipoInformacionDTO> ListTipoInfo()
        {
            List<TipoInformacionDTO> Lista = new List<TipoInformacionDTO>();
            TipoInformacionDTO dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 0;
            dtoTipoInfo.TipoInfoCodigo = "ER";
            dtoTipoInfo.TipoInfoNombre = "ENTREGAS Y RETIROS";
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 1;
            dtoTipoInfo.TipoInfoCodigo = "IB";
            dtoTipoInfo.TipoInfoNombre = "INFORMACIÓN BASE";
            Lista.Add(dtoTipoInfo);
            dtoTipoInfo = new TipoInformacionDTO();
            dtoTipoInfo.TipoInfoCodi = 2;
            dtoTipoInfo.TipoInfoCodigo = "DM";
            dtoTipoInfo.TipoInfoNombre = "DATOS DE MODELOS";
            Lista.Add(dtoTipoInfo);
            return Lista;
        }

        /// Muestra el codigo del Tipo de Información
        public string TipoInformacion(int tipoinfocodi)
        {
            List<TipoInformacionDTO> ListaTipoInfo = ListTipoInfo();
            return ListaTipoInfo[tipoinfocodi].TipoInfoCodigo;
        }
    }
}