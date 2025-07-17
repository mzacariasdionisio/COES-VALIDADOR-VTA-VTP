using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using log4net;
using COES.Dominio.DTO.Enum;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class MejorInformacionController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MejorInformacionController));

        // GET: /TransfPotencia/PeajeEgresoMinfo/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();
        CodigoRetiroGeneradoAppServicio servicioCodigoRetiroGenerado = new CodigoRetiroGeneradoAppServicio();
        CodigoConsolidadoAppServicio servicioCodigoConsolidado = new CodigoConsolidadoAppServicio();
        TipoContratoAppServicio servicioTipoContrato = new TipoContratoAppServicio();
        private static string NombreControlador = "MejorInformacionController";
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();

        public ActionResult Index(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoMinfoModel model = new PeajeEgresoMinfoModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
            }

            //evaluar formato del periodo
            PeriodoDTO oPeriodo = new PeriodoDTO();
            oPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);

            if (oPeriodo.PeriFormNuevo == 0)
            {
                return RedirectToAction("Index", "PeajeEgresoMinfo", new { pericodi = pericodi });
            }
            else
            {
                List<TipoContratoDTO> lstTipoContrato = servicioTipoContrato.ListTipoContrato();
                model.ListaLicitacion = new string[lstTipoContrato.Count];
                for (int i = 0; i < lstTipoContrato.Count; i++)
                {
                    model.ListaLicitacion[i] = lstTipoContrato[i].TipoContNombre;
                }
            }

            if (pericodi > 0)
            {

                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                {
                    recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi;
                }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
            }
            //model.ListaEmpresas = this.servicioEmpresa.ListaEmpresasCombo();
            model.ListaEmpresas = this.servicioEmpresa.ListaInterCoReSoGen();
            model.ListaBarras = this.servicioBarra.ListBarras();
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;
            model.Emprcodi = emprcodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel con los registros de Egresos y Peajes - Mejor información
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GrillaExcel(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            PeajeEgresoMinfoModel modelpe = new PeajeEgresoMinfoModel();
            try
            {
                modelpe.Pericodi = pericodi;
                modelpe.Recpotcodi = recpotcodi;

                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "Codigo", "Empresa", "Cliente", "Barra", "Contrato", "Tipo Usuario", "Precio Potencia S/ /kW-mes", "Potencia Coincidente kW", "Potencia Declarada kW", "Peaje Unitario /KW mes", "Factor pérdida", "Calidad" };
                //string[] Cabecera1 = { "Empresa", "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };

                //Ancho de cada columna
                int[] widths = { 120, 120, 120, 120, 80, 80, 120, 120, 120, 120, 80, 80 };
                object[] columnas = new object[12];

                //Obtener las empresas para el dropdown
                var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
                //Obtener las barras para el dropdown
                var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
                //Lista de PeajesEgreso por EmprCodi
                modelpe.ListaPeajeEgresoMinfo = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoMinfo(modelpe.Pericodi, modelpe.Recpotcodi);
                bool pegrestado = false;
                //Se arma la matriz de datos
                string[][] data;

                decimal totalPotenciaEgreso = 0;
                decimal totalPotenciaCalculada = 0;
                decimal totalPotenciaDeclarada = 0;


                if (modelpe.ListaPeajeEgresoMinfo.Count() != 0)
                {
                    data = new string[modelpe.ListaPeajeEgresoMinfo.Count() + 1][];
                    data[0] = Cabecera1;
                    int index = 1;
                    foreach (VtpPeajeEgresoMinfoDTO item in modelpe.ListaPeajeEgresoMinfo)
                    {
                        string[] itemDato = { item.Coregecodvtp, item.Genemprnombre, item.Cliemprnombre, item.Barrnombre,
                            item.Tipconnombre == null ? "" : item.Tipconnombre.ToString(),
                            item.Pegrmitipousuario,
                            item.Pegrmipreciopote.ToString(), item.Pegrmipotecoincidente == null ? item.Pegrmipoteegreso.ToString() : item.Pegrmipotecoincidente.ToString(),
                            item.Pegrmipotedeclarada.ToString(), item.Pegrmipeajeunitario.ToString(),
                            item.Pegrmifacperdida == null ? item.Pegrdfacperdida.ToString():item.Pegrmifacperdida.ToString() , //100221
                            item.Pegrmicalidad };
                        data[index] = itemDato;

                        totalPotenciaEgreso += (item.Pegrmipoteegreso != null) ? (decimal)item.Pegrmipoteegreso : 0;
                        //totalPotenciaCalculada += (item.Pegrmipotecalculada != null) ? (decimal)item.Pegrmipotecalculada : 0;
                        totalPotenciaDeclarada += (item.Pegrmipotedeclarada != null) ? (decimal)item.Pegrmipotedeclarada : 0;

                        index++;
                    }

                    //string[] itemDatoSuma = { "TOTAL", "", "", "", "", "", totalPotenciaEgreso.ToString(), totalPotenciaDeclarada.ToString(), "", "", "", "" };
                    //data[index] = itemDatoSuma;

                }
                else
                {
                    data = new string[3][];
                    data[0] = Cabecera1;
                    int index = 1;
                    string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "" };
                    data[index] = itemDato;


                }
                string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
                string[] aLicitacion = { "Si", "No" };
                string[] aCalidad = { "Final", "Preliminar", "Mejor información" };

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
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[2] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[3] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[4] = new
                {   //Licitación
                    type = GridExcelModel.TipoLista,
                    source = aLicitacion,
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[5] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoLista,
                    source = aTipoUsuario,
                    strict = false,
                    correctFormat = true,
                    readOnly = false,
                };
                columnas[6] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = pegrestado,
                };
                columnas[7] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[8] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[9] = new
                {   //Peaje Unitario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000",
                    readOnly = pegrestado,
                };
                columnas[10] = new
                {   //factor perdida
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.0000000000",
                    readOnly = pegrestado,
                };
                columnas[11] = new
                {   //Rpsccalidad
                    type = GridExcelModel.TipoLista,
                    source = aCalidad,
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };

                #endregion
                model.Grabar = pegrestado;
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaBarras = ListaBarras.ToArray();
                model.ListaLicitacion = aLicitacion.ToArray();
                model.ListaCalidad = aCalidad.ToArray();
                model.ListaTipoUsuario = aTipoUsuario.ToArray();

                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 1;
                model.FixedColumnsLeft = 1;


                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(model),
                    ContentType = "application/json"
                };

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - GrillaExcel", ex);
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(model),
                    ContentType = "application/json"
                };
                return result;
            }
        }


        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int pericodi, int recpotcodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoMinfoModel model = new PeajeEgresoMinfoModel();
            PeajeEgresoModel pemodel = new PeajeEgresoModel();
            int idTipoContrato = 0;

            if (pericodi == 0 || recpotcodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes de valorización y una versión de recalculo";
                return Json(model);
            }
            try
            {
                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                //Eliminamos la información procesada en el periodo / versión
                string sBorrar = this.servicioTransfPotencia.EliminarProceso(pericodi, recpotcodi);
                if (!sBorrar.Equals("1"))
                {
                    model.sError = sBorrar;
                    return Json(model);
                }

                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpPeajeEgresoMinfo(pericodi, recpotcodi);
                ///////////////////////////////////

                //Graba Cabezera
                model.Entidad = new VtpPeajeEgresoMinfoDTO();
                model.Entidad.Pericodi = pericodi;
                model.Entidad.Recpotcodi = recpotcodi;
                model.Entidad.Pegrmiusucreacion = User.Identity.Name;

                //Recorrer matriz para grabar detalle
                //se inicia en la fila 2

                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.Entidad = new VtpPeajeEgresoMinfoDTO();
                    model.Entidad.Pericodi = pericodi;
                    model.Entidad.Recpotcodi = recpotcodi;
                    model.Entidad.Pegrmiusucreacion = User.Identity.Name;

                    if (!datos[f][0].Equals(""))
                    {
                        model.Entidad.Coregecodvtp = Convert.ToString(datos[f][0]);
                        VtpCodigoConsolidadoDTO dtoCodigoConsolidado = this.servicioCodigoConsolidado.GetByCodigoVTP(model.Entidad.Coregecodvtp);
                        if (dtoCodigoConsolidado != null)
                        {
                            model.Entidad.Coregecodvtp = dtoCodigoConsolidado.Codcncodivtp;
                        }
                        else
                        { continue; }
                    }

                    if (!datos[f][1].Equals(""))
                    {
                        model.Entidad.Genemprnombre = Convert.ToString(datos[f][1]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.Entidad.Genemprnombre);
                        if (dtoEmpresa != null)
                        {
                            model.Entidad.Genemprcodi = dtoEmpresa.EmprCodi;
                            ///Para obtener el Pegrcodi
                            pemodel.Entidad = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresos(Convert.ToInt32(model.Entidad.Genemprcodi), pericodi, recpotcodi);
                            if (pemodel.Entidad != null)
                            {
                                model.Entidad.Pegrcodi = pemodel.Entidad.Pegrcodi;
                            }
                            else
                            {
                                model.Entidad.Pegrcodi = 0;
                            }
                        }
                        else
                        { continue; }
                    }

                    if (!datos[f][2].Equals(""))
                    {
                        model.Entidad.Cliemprnombre = Convert.ToString(datos[f][2]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.Entidad.Cliemprnombre);
                        if (dtoEmpresa != null)
                        {
                            model.Entidad.Cliemprcodi = dtoEmpresa.EmprCodi;
                        }
                        else
                        { continue; }
                    }
                    if (!datos[f][3].Equals(""))
                    {
                        model.Entidad.Barrnombre = Convert.ToString(datos[f][3]);
                        BarraDTO dtoBarra = this.servicioBarra.GetByBarra(model.Entidad.Barrnombre);
                        if (dtoBarra != null)
                        {
                            model.Entidad.Barrcodi = dtoBarra.BarrCodi;
                            model.Entidad.Barrcodifco = dtoBarra.BarrCodi;
                            pemodel.EntidadDetalle = this.servicioTransfPotencia.GetByIdVtpPeajeEgresoMinfo(model.Entidad.Pegrcodi, Convert.ToInt32(model.Entidad.Cliemprcodi), Convert.ToInt32(model.Entidad.Barrcodi), model.Entidad.Pegrmitipousuario);
                            if (pemodel.EntidadDetalle != null)
                            {
                                model.Entidad.Pegrdcodi = pemodel.EntidadDetalle.Pegrdcodi;
                            }
                            else
                            {
                                model.Entidad.Pegrdcodi = 0;
                            }
                        }
                        else
                        { continue; }
                    }
                    //Para otbtener el tipousuario primero
                    if (!datos[f][4].Equals(""))
                    {
                        string sTipoContratoV = Convert.ToString(datos[f][4]).ToString().ToUpper();
                        TipoContratoDTO dtoTipoContrato = this.servicioTipoContrato.GetByNombre(sTipoContratoV);
                        if (dtoTipoContrato == null)
                        {
                            model.sError += "<br>Fila:" + (f + 1) + " - No existe tipo de contrato: " + sTipoContratoV;
                            continue;
                        }
                        else
                        {
                            idTipoContrato = dtoTipoContrato.TipoContCodi;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    string sLicitacionAnt = datos[f][4].ToString().ToUpper();
                    if (sLicitacionAnt == "LICITACION")
                    {
                        sLicitacionAnt = "Si";
                    }
                    else
                    {
                        sLicitacionAnt = "No";
                    }

                    if (!datos[f][5].Equals(""))
                    {
                        string sTipoUsuario = Convert.ToString(datos[f][5]).ToString().ToUpper();
                        if (sTipoUsuario == "REGULADO")
                        { model.Entidad.Pegrmitipousuario = "Regulado"; }
                        else if (sTipoUsuario == "LIBRE")
                        { model.Entidad.Pegrmitipousuario = "Libre"; }
                        else if (sTipoUsuario == "GRAN USUARIO")
                        { model.Entidad.Pegrmitipousuario = "Gran Usuario"; }
                        else
                        {
                            model.sError = "Lo sentimos, hay un error en la columna Tipo de usuario";
                            return Json(model);
                        }
                    }
                    string sTipoContrato = Convert.ToString(datos[f][4]).ToString().ToUpper();
                    model.Entidad.Pegrmilicitacion = sLicitacionAnt;
                    model.Entidad.Pegrmipreciopote = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                    model.Entidad.Pegrmipoteegreso = UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                    model.Entidad.Pegrmipotecalculada = sTipoContrato == "AUTOCONSUMO" ? 0 : UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                    model.Entidad.Pegrmipotecoincidente = UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                    model.Entidad.Pegrmipotedeclarada = UtilTransfPotencia.ValidarNumero(datos[f][8].ToString());
                    model.Entidad.Pegrmipeajeunitario = UtilTransfPotencia.ValidarNumero(datos[f][9].ToString());
                    model.Entidad.Pegrmifacperdida = UtilTransfPotencia.ValidarNumero(datos[f][10].ToString());
                    model.Entidad.Tipconcodi = idTipoContrato;
                    //if (!datos[f][10].Equals(""))
                    //{
                    //    model.Entidad.Barrnombrefco = Convert.ToString(datos[f][10]);
                    //    BarraDTO dtoBarrafco = this.servicioBarra.GetByBarra(model.Entidad.Barrnombrefco);
                    //    if (dtoBarrafco != null)
                    //    { model.Entidad.Barrcodifco = dtoBarrafco.BarrCodi; }
                    //    else
                    //    { continue; }
                    //}
                    //model.Entidad.Pegrmipoteactiva = UtilTransfPotencia.ValidarNumero(datos[f][11].ToString());
                    //model.Entidad.Pegrmipotereactiva = UtilTransfPotencia.ValidarNumero(datos[f][12].ToString());
                    if (!datos[f][11].Equals(""))
                    {
                        string sCalidad = Convert.ToString(datos[f][11]).ToString().ToUpper();
                        if (sCalidad == "MEJOR INFORMACIÓN")
                        { model.Entidad.Pegrmicalidad = "Mejor información"; }
                        else if (sCalidad == "FINAL")
                        { model.Entidad.Pegrmicalidad = "Final"; }
                        else if (sCalidad == "PRELIMINAR")
                        { model.Entidad.Pegrmicalidad = "Preliminar"; }
                    }

                    model.Entidad.Pegrmiusucreacion = User.Identity.Name;

                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpPeajeEgresoMinfo(model.Entidad);
                }

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPIntranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.EnviarDatos;
                objAuditoria.Audproproceso = "Se realizó el envío de información desde intranet - VTP";
                objAuditoria.Audprodescripcion = "Se realizó el envío con el código " + periodoDTO.PeriNombre + " - cantidad de errores - 0" + " - usuario " + User.Identity.Name;
                objAuditoria.Audprousucreacion = User.Identity.Name;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Grabar Data Intranet VTP");
                }

                #endregion

                model.sError = (string.IsNullOrEmpty(model.sError)) ? "" : model.sError;

                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sError = ex.Message; //"-1"
                Logger.Error(NombreControlador + " - GrabarGrillaExcel", ex);
                return Json(model);
            }
        }

        /// <summary>
        /// Muestra la grilla excel con los registros de Egresos y Peajes - vista VW_
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelConsulta(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0, int cliemprcodi = 0, int barrcodi = 0, int barrcodifco = 0, string pegrmitipousuario = "*", string pegrmilicitacion = "*", string pegrmicalidad = "*")
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            PeajeEgresoMinfoModel modelpe = new PeajeEgresoMinfoModel();
            try
            {
                modelpe.Pericodi = pericodi;
                modelpe.Recpotcodi = recpotcodi;

                #region Armando de contenido
                //Definimos la cabecera como una matriz
                //string[] Cabecera1 = { "Empresa", "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
                //string[] Cabecera2 = { "", "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };
                string[] Cabecera1 = { "Codigo", "Empresa", "Cliente", "Barra", "Contrato", "Tipo Usuario", "Precio Potencia S/ /kW-mes", "Potencia Coincidente kW", "Potencia Declarada kW", "Peaje Unitario /KW mes", "Factor pérdida", "Calidad" };

                //Anvho de cada columna
                //int[] widths = { 150, 150, 120, 60, 60, 80, 60, 80, 80, 80, 120, 80, 80, 110 };
                //object[] columnas = new object[14];
                int[] widths = { 120, 120, 120, 120, 100, 100, 100, 100, 100, 100, 80, 80 };
                object[] columnas = new object[12];

                //Lista de PeajesEgreso por EmprCodi
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
                modelpe.ListaPeajeEgresoMinfo = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoMinfoVistaNuevo(pericodi, recpotcodi, emprcodi, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, pegrmicalidad2);
                bool bEstado = true;
                //Se arma la matriz de datos
                string[][] data;

                decimal totalPotenciaEgreso = 0;
                decimal totalPotenciaCalculada = 0;
                decimal totalPotenciaDeclarada = 0;

                if (modelpe.ListaPeajeEgresoMinfo.Count() != 0)
                {
                    data = new string[modelpe.ListaPeajeEgresoMinfo.Count() + 3][];
                    data[0] = Cabecera1;
                    //data[1] = Cabecera2;
                    int index = 1;
                    foreach (VtpPeajeEgresoMinfoDTO item in modelpe.ListaPeajeEgresoMinfo)
                    {
                        //string[] itemDato = { item.Genemprnombre, item.Cliemprnombre, item.Barrnombre, item.Pegrmitipousuario, item.Pegrmilicitacion.ToString(), item.Pegrmipreciopote.ToString(), item.Pegrmipoteegreso.ToString(), item.Pegrmipotecalculada.ToString(), item.Pegrmipotedeclarada.ToString(), item.Pegrmipeajeunitario.ToString(), item.Barrnombrefco, item.Pegrmipoteactiva.ToString(), item.Pegrmipotereactiva.ToString(), item.Pegrmicalidad };
                        decimal? potencia = (item.Pegrdpotecoincidente == null) ? item.Pegrmipoteegreso : item.Pegrdpotecoincidente;
                        string[] itemDato = {
                        item.Coregecodvtp,
                        item.Genemprnombre,
                        item.Cliemprnombre,
                        item.Barrnombre,
                        item.Tipconnombre == null ? "" : item.Tipconnombre.ToString(),
                        item.Pegrmitipousuario,
                        item.Pegrmipreciopote.ToString(),
                        potencia.ToString(),
                        item.Pegrmipotedeclarada.ToString(),
                        item.Pegrmipeajeunitario.ToString(),
                        item.Pegrmifacperdida.ToString(),
                        item.Pegrmicalidad
                    };
                        data[index] = itemDato;

                        totalPotenciaEgreso += potencia ?? 0;
                        //totalPotenciaCalculada += (item.Pegrmipotecalculada != null) ? (decimal)item.Pegrmipotecalculada : 0;
                        totalPotenciaDeclarada += (item.Pegrmipotedeclarada != null) ? (decimal)item.Pegrmipotedeclarada : 0;

                        index++;
                    }

                    string[] itemDatoSuma = { "TOTAL", "", "", "", "", "", "", totalPotenciaEgreso.ToString(), totalPotenciaDeclarada.ToString(), "", "", "" };
                    data[index] = itemDatoSuma;
                }
                else
                {
                    data = new string[3][];
                    data[0] = Cabecera1;
                    //data[1] = Cabecera2;
                    int index = 2;
                    //string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                    string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "" };
                    data[index] = itemDato;
                }
                ///////////  
                columnas[0] = new
                {   //Coregecodvtp - Coregecodvtp
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[1] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[2] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[3] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[4] = new
                {   //Licitación
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[5] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[6] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = bEstado,
                };
                columnas[7] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = bEstado,
                };
                columnas[8] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = bEstado,
                };
                columnas[9] = new
                {   //Peaje Unitario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = bEstado,
                };
                columnas[10] = new
                {   //Factor Perdida
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = bEstado,
                };
                columnas[11] = new
                {   //Rpsccalidad
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };

                #endregion
                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 1;
                model.FixedColumnsLeft = 3;

                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - GrillaExcelConsulta", ex);
                return Json(ex);
            }
        }



        /// <summary>
        /// Muestra la grilla excel con los registros de Egresos y Peajes faltantes en el periodo versión
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelInfoFaltante(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            PeajeEgresoMinfoModel modelpe = new PeajeEgresoMinfoModel();
            try
            {
                modelpe.Pericodi = pericodi;
                modelpe.Recpotcodi = recpotcodi;

                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] Cabecera = { "Empresa", "Cliente", "Barra", "Tipo Usuario", "Licitación" };

                //Anvho de cada columna
                int[] widths = { 250, 250, 250, 100, 100 };
                object[] columnas = new object[5];
                //Información del periodo anterior
                int periodoanterior = pericodi;
                int recpotcodianterior = recpotcodi - 1;
                if (recpotcodianterior == 0)
                {
                    modelpe.EntidadPeriodo = this.servicioPeriodo.BuscarPeriodoAnterior(pericodi);
                    if (modelpe.EntidadPeriodo == null)
                    {
                        model.sMensaje = "No existe un periodo anterior";
                    }
                    periodoanterior = modelpe.EntidadPeriodo.PeriCodi;
                    modelpe.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(periodoanterior);
                    if (modelpe.ListaRecalculoPotencia.Count == 0)
                    {
                        model.sMensaje = "No existe un recalculo anterior";
                    }
                    recpotcodianterior = modelpe.ListaRecalculoPotencia[0].Recpotcodi;
                }

                modelpe.ListaPeajeEgresoMinfo = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoMinfoFaltante(pericodi, recpotcodi, periodoanterior, recpotcodianterior);
                bool bEstado = true;
                //Se arma la matriz de datos
                string[][] data;

                if (modelpe.ListaPeajeEgresoMinfo.Count() != 0)
                {
                    data = new string[modelpe.ListaPeajeEgresoMinfo.Count() + 1][];
                    data[0] = Cabecera;
                    int index = 1;
                    foreach (VtpPeajeEgresoMinfoDTO item in modelpe.ListaPeajeEgresoMinfo)
                    {
                        string[] itemDato = { item.Genemprnombre, item.Cliemprnombre, item.Barrnombre, item.Pegrmitipousuario, item.Pegrmilicitacion.ToString(), item.Pegrmipreciopote.ToString(), item.Pegrmipoteegreso.ToString(), item.Pegrmipotecalculada.ToString(), item.Pegrmipotedeclarada.ToString(), item.Pegrmipeajeunitario.ToString(), item.Barrnombrefco, item.Pegrmipoteactiva.ToString(), item.Pegrmipotereactiva.ToString(), item.Pegrmicalidad };
                        data[index] = itemDato;
                        index++;
                    }
                }
                else
                {
                    data = new string[3][];
                    data[0] = Cabecera;
                    int index = 1;
                    string[] itemDato = { "No existe información faltante", "", "", "", "" };
                    data[index] = itemDato;
                }
                ///////////          
                columnas[0] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[1] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[2] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[3] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };
                columnas[4] = new
                {   //Licitación
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = bEstado,
                };

                #endregion
                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                //model.FixedRowsTop = 2;
                //model.FixedColumnsLeft = 3;

                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - GrillaExcelInfoFaltante", ex);
                return Json(ex);
            }

        }

        #endregion

        /// <summary>
        /// Permite eliminar todos los registros del periodo y versión de recalculo para mejor información
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatos(int pericodi, int recpotcodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            PeajeEgresoMinfoModel model = new PeajeEgresoMinfoModel();
            try
            {
                //Eliminamos la información procesada en el periodo / versión
                string sBorrar = this.servicioTransfPotencia.EliminarProceso(pericodi, recpotcodi);
                if (!sBorrar.Equals("1"))
                {
                    model.sError = sBorrar;
                    return Json(model);
                }

                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpPeajeEgresoMinfo(pericodi, recpotcodi);
                ///////////////////////////////////
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - EliminarDatos", ex);
                sResultado = ex.Message; //"-1";
            }

            return Json(sResultado);

        }

        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0, int formato = 1, int cliemprcodi = 0, int barrcodi = 0, int barrcodifco = 0, string pegrmitipousuario = "", string pegrmilicitacion = "", string pegrmicalidad = "")
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                ExcelWorksheet hoja = null;
                string file = this.servicioTransfPotencia.GenerarFormatoPeajeEgresoMinfoNuevo(pericodi, recpotcodi, emprcodi, formato,
                    pathFile, pathLogo, cliemprcodi, barrcodi, barrcodifco, pegrmitipousuario, pegrmilicitacion, pegrmicalidad, out hoja);

                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                VtpRecalculoPotenciaDTO EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPIntranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.BajarFormato;
                objAuditoria.Audproproceso = "Exportación de data en excel intranet - VTP";
                objAuditoria.Audprodescripcion = "Se exporta data en excel del periodo " + periodoDTO.PeriNombre + " y revisión " + EntidadRecalculoPotencia.Recpotnombre + " - usuario " + base.UserName;
                objAuditoria.Audprousucreacion = base.UserName;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Exportar Data Intranet - VTP");
                }

                #endregion

                return Json(file);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ExportarData", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Descarga el archivo excel
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
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - UploadExcel", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivo(string sarchivo, int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                //string[] Cabecera1 = { "Empresa", "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
                //string[] Cabecera2 = { "", "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };
                string[] Cabecera1 = { "Codigo", "Empresa", "Cliente", "Barra", "Contrato", "Tipo Usuario", "Precio Potencia S/ /kW-mes", "Potencia Coincidente kW", "Potencia Demanda kW", "Peaje Unitario /KW mes", "Factor pérdida", "Calidad" };
                //Ancho de cada columna
                int[] widths = { 120, 120, 120, 120, 100, 100, 100, 100, 100, 100, 80, 80 };
                object[] columnas = new object[12];

                //Obtener las empresas para el dropdown
                var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
                //Obtener las barras para el dropdown
                var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
                bool pegrestado = false;
                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioTransfPotencia.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 4][]; // -6 por las primeras filas del encabezado + 2 por las dos cabeceras
                data[0] = Cabecera1;
                int index = 1;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 6)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    //CODIVO VTP
                    string sCodigoVtp = dtRow[1].ToString();
                    VtpCodigoConsolidadoDTO dtoCodigoConsolidado = this.servicioCodigoConsolidado.GetByCodigoVTP(sCodigoVtp);
                    if (dtoCodigoConsolidado == null)
                    {
                        sMensajeError += "<br>No existe el codigo VTP: " + sCodigoVtp;
                        sCodigoVtp = "";
                        iRegError++;
                        continue;
                    }
                    string sEmpresa = dtRow[2].ToString();
                    EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(sEmpresa);
                    if (dtoEmpresa == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe: " + sEmpresa;
                        iRegError++;
                        continue;
                    }
                    if (sEmpresa != dtoCodigoConsolidado.Empresa.Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide la empresa: " + sEmpresa + ", debe ser la siguiente empresa: " + dtoCodigoConsolidado.Empresa;
                        sEmpresa = "";
                        iRegError++;
                        continue;
                    }
                    string sCliente = dtRow[3].ToString();
                    EmpresaDTO dtoCliente = this.servicioEmpresa.GetByNombre(sCliente);
                    if (dtoCliente == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe: " + sCliente;
                        iRegError++;
                        continue;
                    }
                    if (sCliente != dtoCodigoConsolidado.Cliente.Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide el cliente: " + sCliente + ", debe ser el siguiente cliente: " + dtoCodigoConsolidado.Cliente;
                        sCliente = "";
                        iRegError++;
                        continue;
                    }
                    string sBarra = dtRow[4].ToString();
                    BarraDTO dtoBarra = this.servicioBarra.GetByBarra(sBarra);
                    if (dtoBarra == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe: " + sBarra;
                        iRegError++;
                        continue;
                    }
                    if (sBarra.ToUpper().Trim() != dtoCodigoConsolidado.Barra.ToUpper().Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide la barra: " + sBarra + ", debe ser la siguiente barra: " + dtoCodigoConsolidado.Barra;
                        sCliente = "";
                        iRegError++;
                        continue;
                    }

                    string sLicitacion = dtRow[5].ToString().ToUpper();
                    TipoContratoDTO dtoTipoContrato = this.servicioTipoContrato.GetByNombre(sLicitacion);
                    if (dtoTipoContrato == null)
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe el tipo de contrato: " + sLicitacion;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }
                    if (sLicitacion.ToUpper().Trim() != dtoCodigoConsolidado.TipConNombre.ToUpper().Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide el tipo de contrato: " + sLicitacion + ", debe ser el tipo de contrato: " + dtoCodigoConsolidado.TipConNombre;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }

                    string sLicitacionAnt = dtRow[5].ToString().ToUpper();

                    if (sLicitacionAnt == "LICITACION")
                    {
                        sLicitacionAnt = "Si";
                    }
                    else
                    {
                        sLicitacionAnt = "No";
                    }

                    string sTipoUsuario = Convert.ToString(dtRow[6]).ToString().ToUpper();
                    if (sTipoUsuario == "REGULADO")
                    { sTipoUsuario = "Regulado"; }
                    else if (sTipoUsuario == "LIBRE")
                    { sTipoUsuario = "Libre"; }
                    else if (sTipoUsuario == "GRAN USUARIO")
                    { sTipoUsuario = "Gran Usuario"; }
                    else
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No existe el tipo de usuario: " + sTipoUsuario;
                        sTipoUsuario = "";
                        iRegError++;
                        continue;
                    }
                    if (sTipoUsuario.ToUpper().Trim() != dtoCodigoConsolidado.TipUsuNombre.ToUpper().Trim())
                    {
                        sMensajeError += "<br>Codigo VTP:" + sCodigoVtp + " - No coincide el tipo de usuario: " + sTipoUsuario.ToUpper() + ", debe ser el siguiente tipo de usuario: " + dtoCodigoConsolidado.TipUsuNombre;
                        sLicitacion = "";
                        iRegError++;
                        continue;
                    }

                    string[] itemDato = {
                        sCodigoVtp,
                        sEmpresa,
                        sCliente,
                        sBarra,
                        sLicitacion,
                        dtRow[6].ToString(),
                        dtRow[7].ToString(),
                        dtRow[8].ToString(),
                        sLicitacion == "AUTOCONSUMO" ? "0" : dtRow[9].ToString(),
                        sLicitacion == "AUTOCONSUMO" ? "0" : dtRow[10].ToString(),
                        dtRow[11].ToString(),
                        "Mejor Información"
                    };
                    data[index] = itemDato;
                    index++;
                }

                PeriodoDTO periodoDTO = this.servicioPeriodo.GetByIdPeriodo(pericodi);

                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.CargaInformacionVTPIntranet;
                objAuditoria.Estdcodi = (int)EVtpEstados.SubirFormato;
                objAuditoria.Audproproceso = "Importación de data en excel intranet - VTP";
                objAuditoria.Audprodescripcion = "Se importa la data del periodo " + periodoDTO.PeriNombre + " - cantidad de errores - 0" + " - usuario " + base.UserName;
                objAuditoria.Audprousucreacion = base.UserName;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                int auditoria = this.servicioAuditoria.save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(NombreControlador + " - Error Save Auditoria - Importar Data Intranet VTP");
                }

                #endregion

                string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
                string[] aLicitacion = { "Licitacion", "Bilateral", "Autoconsumo" };
                string[] aCalidad = { "Final", "Preliminar", "Mejor información" };
                columnas[0] = new
                {   //CodigoVTP
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[1] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[2] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[3] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[4] = new
                {   //Licitación
                    type = GridExcelModel.TipoLista,
                    source = aLicitacion,
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[5] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoLista,
                    source = aTipoUsuario,
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[6] = new
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[7] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[8] = new
                {   //Potencia Calculada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[9] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[10] = new
                {   //Peaje Unitario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[11] = new
                {   //Rpsccalidad
                    type = GridExcelModel.TipoLista,
                    source = aCalidad,
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };

                #endregion
                model.Grabar = pegrestado;
                model.ListaEmpresas = ListaEmpresas.ToArray();
                model.ListaBarras = ListaBarras.ToArray();
                model.ListaLicitacion = aLicitacion.ToArray();
                model.ListaCalidad = aCalidad.ToArray();
                model.ListaTipoUsuario = aTipoUsuario.ToArray();

                model.Data = data;
                model.Widths = widths;
                model.Columnas = columnas;
                model.FixedRowsTop = 1;
                model.FixedColumnsLeft = 1;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception ex)
            {
                Logger.Error(NombreControlador + " - ProcesarArchivo", ex);
                sResultado = ex.Message;
                return Json(sResultado);
            }
        }
    }
}
