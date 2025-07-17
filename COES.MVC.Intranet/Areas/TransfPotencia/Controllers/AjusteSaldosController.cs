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

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class AjusteSaldosController : BaseController
    {
        // GET: /TransfPotencia/AjusteSaldos/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();

        public ActionResult Index(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            AjusteSaldosModel model = new AjusteSaldosModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            {
                pericodi = model.ListaPeriodos[0].PeriCodi;
                model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            }
            else
            {
                model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            }
            model.Pericodi = pericodi;
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        #region Grilla Excel Peajes

        /// <summary>
        /// Muestra la grilla excel con los registros de PeajeEmpresaAjuste
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelPeajeAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            AjusteSaldosModel modelAS = new AjusteSaldosModel();
            modelAS.Pericodi = pericodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera = { "Empresa generadora", "Empresa transmisora", "Sistema de Transmisión", "Saldo Peaje" };

            //Ancho de cada columna
            int[] widths = { 400, 400, 400, 100 };
            object[] columnas = new object[4];

            //Obtener las empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            //Obtener los cargos para el dropdown
            var ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, 1).Select(x => x.Pingnombre).ToList();
            //Lista de CargoAjuste
            modelAS.ListaPeajeEmpresaAjuste = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEmpresaAjuste(modelAS.Pericodi);
            //Se arma la matriz de datos
            string[][] data;

            if (modelAS.ListaPeajeEmpresaAjuste.Count() != 0)
            {
                data = new string[modelAS.ListaPeajeEmpresaAjuste.Count() + 1][];
                data[0] = Cabecera;
                int index = 1;
                foreach (VtpPeajeEmpresaAjusteDTO item in modelAS.ListaPeajeEmpresaAjuste)
                {
                    string[] itemDato = { item.Emprnombpeaje, item.Emprnombcargo, item.Pingnombre, item.Pempajajuste.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[2][];
                data[0] = Cabecera;
                int index = 1;
                string[] itemDato = { "", "", "", "" };
                data[index] = itemDato;
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------------
            columnas[0] = new
            {   //Emprcodipeaje - Emprnomb - Generador
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                readOnly = false,
            };
            columnas[1] = new
            {   //Emprcodicargo - Emprnomb - Transmisor
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                readOnly = true,
            };
            columnas[2] = new
            {   //Pingnombre
                type = GridExcelModel.TipoLista,
                source = ListaPeajeIngreso.ToArray(),
                strict = false,
                readOnly = false,
            };
            columnas[3] = new
            {   //Pempajajuste
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                className = "htRight",
                format = "0,0.00",
                readOnly = false,
            };

            #endregion
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.ListaPeajeIngreso = ListaPeajeIngreso.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelPeajeAjuste(int pericodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            AjusteSaldosModel model = new AjusteSaldosModel();

            if (pericodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes de valorización";
                return Json(model);
            }
            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpPeajeEmpresaAjuste(pericodi);
                //Recorrer matriz para grabar la información, se inicia en la fila 1
                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadPeajeEmpresaAjuste = new VtpPeajeEmpresaAjusteDTO();
                    model.EntidadPeajeEmpresaAjuste.Pericodi = pericodi;

                    if (!datos[f][0].Equals(""))
                    {
                        model.EntidadPeajeEmpresaAjuste.Emprnombpeaje = Convert.ToString(datos[f][0]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadPeajeEmpresaAjuste.Emprnombpeaje);
                        if (dtoEmpresa != null)
                        {
                            model.EntidadPeajeEmpresaAjuste.Emprcodipeaje = dtoEmpresa.EmprCodi;
                        }
                        else
                        { continue; }
                    }

                    if (!datos[f][2].Equals(""))
                    {
                        model.EntidadPeajeEmpresaAjuste.Pingnombre = Convert.ToString(datos[f][2]);
                        VtpPeajeIngresoDTO dtoPeajeIngreso = this.servicioTransfPotencia.GetByNomIngTarVtpPeajeIngreso(pericodi, 1, model.EntidadPeajeEmpresaAjuste.Pingnombre);
                        if (dtoPeajeIngreso != null)
                        {
                            model.EntidadPeajeEmpresaAjuste.Pingcodi = dtoPeajeIngreso.Pingcodi;
                            model.EntidadPeajeEmpresaAjuste.Emprcodicargo = (int)dtoPeajeIngreso.Emprcodi;
                        }
                        else
                        { continue; }
                    }

                    //if (!datos[f][2].Equals(""))
                    //{
                    //    model.EntidadPeajeEmpresaAjuste.Emprnombcargo = Convert.ToString(datos[f][2]);
                    //    EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadPeajeEmpresaAjuste.Emprnombcargo);
                    //    if (dtoEmpresa != null)
                    //    {
                    //        model.EntidadPeajeEmpresaAjuste.Emprcodicargo = dtoEmpresa.EmprCodi;
                    //    }
                    //    else
                    //    { continue; }
                    //}
                    model.EntidadPeajeEmpresaAjuste.Pempajajuste = UtilTransfPotencia.ValidarNumero(datos[f][3].ToString());
                    model.EntidadPeajeEmpresaAjuste.Pempajusucreacion = User.Identity.Name;

                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpPeajeEmpresaAjuste(model.EntidadPeajeEmpresaAjuste);
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        /// <summary>
        /// Permite eliminar todos los registros del periodo 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarPeajeAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpPeajeEmpresaAjuste(pericodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        #endregion

        #region Grilla Excel Ingreso tarifario

        /// <summary>
        /// Muestra la grilla excel con los registros de IngresoTarifarioAjuste
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelIngresoAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            AjusteSaldosModel modelAS = new AjusteSaldosModel();
            modelAS.Pericodi = pericodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera = { "Empresa generadora", "Empresa transmisora", "Sistema de transmisión", "Saldo IT" };

            //Ancho de cada columna
            int[] widths = { 400, 400, 400, 100 };
            object[] columnas = new object[4];

            //Obtener las empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            //Obtener los cargos para el dropdown
            var ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, 1).Select(x => x.Pingnombre).ToList();
            //Lista de CargoAjuste
            modelAS.ListaIngresoTarifarioAjuste = this.servicioTransfPotencia.GetByCriteriaVtpIngresoTarifarioAjuste(modelAS.Pericodi);
            //Se arma la matriz de datos
            string[][] data;

            if (modelAS.ListaIngresoTarifarioAjuste.Count() != 0)
            {
                data = new string[modelAS.ListaIngresoTarifarioAjuste.Count() + 1][];
                data[0] = Cabecera;
                int index = 1;
                foreach (VtpIngresoTarifarioAjusteDTO item in modelAS.ListaIngresoTarifarioAjuste)
                {
                    string[] itemDato = { item.Emprnombping, item.Emprnombingpot, item.Pingnombre, item.Ingtajajuste.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[2][];
                data[0] = Cabecera;
                int index = 1;
                string[] itemDato = { "", "", "", "" };
                data[index] = itemDato;
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------------
            columnas[0] = new
            {   //Emprcodiping - Emprnomb - generadora
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                readOnly = false,
            };
            columnas[1] = new
            {   //Emprcodingpot - Emprnomb - transmisora
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                readOnly = true,
            };
            columnas[2] = new
            {   //Pingnombre
                type = GridExcelModel.TipoLista,
                source = ListaPeajeIngreso.ToArray(),
                strict = false,
                readOnly = false,
            };
            columnas[3] = new
            {   //Ingtajajuste
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false,
            };

            #endregion
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.ListaPeajeIngreso = ListaPeajeIngreso.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelIngresoAjuste(int pericodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            AjusteSaldosModel model = new AjusteSaldosModel();

            if (pericodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes de valorización";
                return Json(model);
            }
            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpIngresoTarifarioAjuste(pericodi);
                //Recorrer matriz para grabar la información, se inicia en la fila 1
                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadIngresoTarifarioAjuste = new VtpIngresoTarifarioAjusteDTO();
                    model.EntidadIngresoTarifarioAjuste.Pericodi = pericodi;

                    if (!datos[f][0].Equals(""))
                    {
                        model.EntidadIngresoTarifarioAjuste.Emprnombping = Convert.ToString(datos[f][0]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadIngresoTarifarioAjuste.Emprnombping);
                        if (dtoEmpresa != null)
                        {
                            model.EntidadIngresoTarifarioAjuste.Emprcodiping = dtoEmpresa.EmprCodi;
                        }
                        else
                        { continue; }
                    }

                    if (!datos[f][2].Equals(""))
                    {
                        model.EntidadIngresoTarifarioAjuste.Pingnombre = Convert.ToString(datos[f][2]);
                        VtpPeajeIngresoDTO dtoPeajeIngreso = this.servicioTransfPotencia.GetByNomIngTarVtpPeajeIngreso(pericodi, 1, model.EntidadIngresoTarifarioAjuste.Pingnombre);
                        if (dtoPeajeIngreso != null)
                        {
                            model.EntidadIngresoTarifarioAjuste.Pingcodi = dtoPeajeIngreso.Pingcodi;
                            model.EntidadIngresoTarifarioAjuste.Emprcodingpot = (int)dtoPeajeIngreso.Emprcodi;
                        }
                        else
                        { continue; }
                    }
                    //if (!datos[f][2].Equals(""))
                    //{
                    //    model.EntidadIngresoTarifarioAjuste.Emprnombingpot = Convert.ToString(datos[f][2]);
                    //    EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadIngresoTarifarioAjuste.Emprnombingpot);
                    //    if (dtoEmpresa != null)
                    //    {
                    //        model.EntidadIngresoTarifarioAjuste.Emprcodingpot = dtoEmpresa.EmprCodi;
                    //    }
                    //    else
                    //    { continue; }
                    //}
                    model.EntidadIngresoTarifarioAjuste.Ingtajajuste = UtilTransfPotencia.ValidarNumero(datos[f][3].ToString());
                    model.EntidadIngresoTarifarioAjuste.Ingtajusucreacion = User.Identity.Name;

                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpIngresoTarifarioAjuste(model.EntidadIngresoTarifarioAjuste);
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        /// <summary>
        /// Permite eliminar todos los registros del periodo 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarIngresoAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpIngresoTarifarioAjuste(pericodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        #endregion

        #region Grilla Excel Cargos incluidos en el Peaje
        //REVIZAR ESTE PROCEDIMIENTO DE CARGA DE SALDOS, PUES SE ESTA PIDIENDO 2 EMPRESAS, 
        //GENERADOR Y DESTINATARIO.... QUE EMPRESA ESTA EN ESTA TABLA... YO CREO QUE ES LA DESTINATARIO... EN LAS PRUEBAS SE VA A DESCARTAR ELLO

        /// <summary>
        /// Muestra la grilla excel con los registros de CargoAjuste
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelCargoAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            AjusteSaldosModel modelAS = new AjusteSaldosModel();
            modelAS.Pericodi = pericodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera = { "Empresa", "Cargo", "Saldo Cargo" };

            //Ancho de cada columna
            int[] widths = { 400, 500, 150 };
            object[] columnas = new object[3];

            //Obtener las empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            //Obtener los cargos para el dropdown
            var ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(pericodi, 1).Select(x => x.Pingnombre).ToList();
            //Lista de CargoAjuste
            modelAS.ListaPeajeCargoAjuste = this.servicioTransfPotencia.GetByCriteriaVtpPeajeCargoAjuste(modelAS.Pericodi);
            //Se arma la matriz de datos
            string[][] data;

            if (modelAS.ListaPeajeCargoAjuste.Count() != 0)
            {
                data = new string[modelAS.ListaPeajeCargoAjuste.Count() + 1][];
                data[0] = Cabecera;
                int index = 1;
                foreach (VtpPeajeCargoAjusteDTO item in modelAS.ListaPeajeCargoAjuste)
                {
                    string[] itemDato = { item.Emprnomb, item.Pingnombre, item.Pecajajuste.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[2][];
                data[0] = Cabecera;
                int index = 1;
                string[] itemDato = { "", "", "" };
                data[index] = itemDato;
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------------
            columnas[0] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[1] = new
            {   //Pingnombre
                type = GridExcelModel.TipoLista,
                source = ListaPeajeIngreso.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[2] = new
            {   //Pecajajuste
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false,
            };

            #endregion
            model.ListaEmpresas = ListaEmpresas.ToArray();
            model.ListaPeajeIngreso = ListaPeajeIngreso.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            //model.FixedRowsTop = 2;
            //model.FixedColumnsLeft = 3;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelCargoAjuste(int pericodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            AjusteSaldosModel model = new AjusteSaldosModel();

            if (pericodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes de valorización";
                return Json(model);
            }
            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpPeajeCargoAjuste(pericodi);
                //Recorrer matriz para grabar la información, se inicia en la fila 1
                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadPeajeCargoAjuste = new VtpPeajeCargoAjusteDTO();
                    model.EntidadPeajeCargoAjuste.Pericodi = pericodi;

                    if (!datos[f][0].Equals(""))
                    {
                        model.EntidadPeajeCargoAjuste.Emprnomb = Convert.ToString(datos[f][0]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadPeajeCargoAjuste.Emprnomb);
                        if (dtoEmpresa != null)
                        {
                            model.EntidadPeajeCargoAjuste.Emprcodi = dtoEmpresa.EmprCodi;
                        }
                        else
                        { continue; }
                    }

                    if (!datos[f][1].Equals(""))
                    {
                        model.EntidadPeajeCargoAjuste.Pingnombre = Convert.ToString(datos[f][1]);
                        VtpPeajeIngresoDTO dtoPeajeIngreso = this.servicioTransfPotencia.GetByNomIngTarVtpPeajeIngreso(pericodi, 1, model.EntidadPeajeCargoAjuste.Pingnombre);
                        if (dtoPeajeIngreso != null)
                        {
                            model.EntidadPeajeCargoAjuste.Pingcodi = dtoPeajeIngreso.Pingcodi;
                        }
                        else
                        { continue; }
                    }
                    model.EntidadPeajeCargoAjuste.Pecajajuste = UtilTransfPotencia.ValidarNumero(datos[f][2].ToString());
                    model.EntidadPeajeCargoAjuste.Pecajusucreacion = User.Identity.Name;

                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpPeajeCargoAjuste(model.EntidadPeajeCargoAjuste);
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        /// <summary>
        /// Permite eliminar todos los registros del periodo 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarCargoAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpPeajeCargoAjuste(pericodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        #endregion

        #region Grilla Excel Saldo VTP

        /// <summary>
        /// Muestra la grilla excel con los registros de SaldoEmpresaAjuste
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelSaldoAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            AjusteSaldosModel modelAS = new AjusteSaldosModel();
            modelAS.Pericodi = pericodi;

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera = { "Empresa generadora", "Saldo" };

            //Ancho de cada columna
            int[] widths = { 600, 200 };
            object[] columnas = new object[2];

            //Obtener las empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            //Lista de SaldoEmpresaAjuste
            modelAS.ListaSaldoEmpresaAjuste = this.servicioTransfPotencia.GetByCriteriaVtpSaldoEmpresaAjuste(modelAS.Pericodi);
            //Se arma la matriz de datos
            string[][] data;

            if (modelAS.ListaSaldoEmpresaAjuste.Count() != 0)
            {
                data = new string[modelAS.ListaSaldoEmpresaAjuste.Count() + 1][];
                data[0] = Cabecera;
                int index = 1;
                foreach (VtpSaldoEmpresaAjusteDTO item in modelAS.ListaSaldoEmpresaAjuste)
                {
                    string[] itemDato = { item.Emprnomb, item.Potseaajuste.ToString() };
                    data[index] = itemDato;
                    index++;
                }
            }
            else
            {
                data = new string[2][];
                data[0] = Cabecera;
                int index = 1;
                string[] itemDato = { "", "", "", "" };
                data[index] = itemDato;
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------------
            columnas[0] = new
            {   //Emprcodipeaje - Emprnomb
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = false,
            };
            columnas[1] = new
            {   //Potseaajuste
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false,
            };

            #endregion
            model.ListaEmpresas = ListaEmpresas.ToArray();

            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            //model.FixedRowsTop = 2;
            //model.FixedColumnsLeft = 3;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelSaldoAjuste(int pericodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            AjusteSaldosModel model = new AjusteSaldosModel();

            if (pericodi == 0)
            {
                model.sError = "Lo sentimos, debe seleccionar un mes de valorización";
                return Json(model);
            }
            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpSaldoEmpresaAjuste(pericodi);
                //Recorrer matriz para grabar la información, se inicia en la fila 1
                for (int f = 1; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadSaldoEmpresaAjuste = new VtpSaldoEmpresaAjusteDTO();
                    model.EntidadSaldoEmpresaAjuste.Pericodi = pericodi;

                    if (!datos[f][0].Equals(""))
                    {
                        model.EntidadSaldoEmpresaAjuste.Emprnomb = Convert.ToString(datos[f][0]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadSaldoEmpresaAjuste.Emprnomb);
                        if (dtoEmpresa != null)
                        {
                            model.EntidadSaldoEmpresaAjuste.Emprcodi = dtoEmpresa.EmprCodi;
                        }
                        else
                        { continue; }
                    }

                    model.EntidadSaldoEmpresaAjuste.Potseaajuste = UtilTransfPotencia.ValidarNumero(datos[f][1].ToString());
                    model.EntidadSaldoEmpresaAjuste.Potseausucreacion = User.Identity.Name;

                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpSaldoEmpresaAjuste(model.EntidadSaldoEmpresaAjuste);
                }
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }

        }

        /// <summary>
        /// Permite eliminar todos los registros del periodo 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarSaldoAjuste(int pericodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                this.servicioTransfPotencia.DeleteByCriteriaVtpSaldoEmpresaAjuste(pericodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        #endregion


    }
}
