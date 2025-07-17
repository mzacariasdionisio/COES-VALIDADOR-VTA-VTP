using COES.Dominio.DTO.Transferencias;
using COES.MVC.Extranet.Areas.TransfPotencia.Models;
using COES.MVC.Extranet.Areas.TransfPotencia.Helper;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
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
using COES.MVC.Extranet.Controllers;

namespace COES.MVC.Extranet.Areas.TransfPotencia.Controllers
{
    public class PeajeEgresoController : BaseController
    {
        // GET: /TransfPotencia/PeajeEgreso/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        BarraAppServicio servicioBarra = new BarraAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Index(int pericodi = 0, int recpotcodi = 0, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();

            #region Autentificando Empresa

            PeajeEgresoModel models = new PeajeEgresoModel();
            int iEmprCodi = 0;
            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

            List<SeguridadServicio.EmpresaDTO> listTotal = new List<SeguridadServicio.EmpresaDTO>();//UtilTransfPotencia.ObtenerEmpresasPorUsuario(User.Identity.Name);
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();

            bool accesoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            if (accesoEmpresas)
            {
                listTotal = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13 || x.EMPRCODI == 67).OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                listTotal = base.ListaEmpresas;
            }


            //- aca debemos hacer jugada para escoger la empresa
            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();

            foreach (var item in listTotal)
            {
                list.Add(item);
                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        SeguridadServicio.EmpresaDTO dtoEmpresaConcepto = new SeguridadServicio.EmpresaDTO();
                        dtoEmpresaConcepto.EMPRCODI = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EMPRNOMB = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TIPOEMPRCODI = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        list.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            if(accesoEmpresas)
            {
                list.RemoveAll(x => x.EMPRCODI == 67);
            }
            

            TempData["EMPRNRO"] = list.Count();
            if (list.Count() == 1)
            {
                TempData["EMPRNOMB"] = list[0].EMPRNOMB;
                Session["EmprNomb"] = list[0].EMPRNOMB;
                Session["EmprCodi"] = list[0].EMPRCODI;

                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(list[0].EMPRCODI);

                if (list[0].EMPRCODI > 0)
                {
                    if (empresa.Emprestado != "A")
                    {
                        TempData["EmprNomb"] = list[0].EMPRNOMB + "  <span style='color:red'>(EN BAJA)</span>";
                    }
                }

                iEmprCodi = Convert.ToInt32(list[0].EMPRCODI);
            }
            else if (Session["EmprCodi"] != null)
            {
                iEmprCodi = Convert.ToInt32(Session["EmprCodi"].ToString());
                EmpresaDTO dtoEmpresa = new EmpresaDTO();
                dtoEmpresa = (new EmpresaAppServicio()).GetByIdEmpresa(iEmprCodi);
                TempData["EMPRNOMB"] = dtoEmpresa.EmprNombre;
                Session["EmprNomb"] = dtoEmpresa.EmprNombre;

                Dominio.DTO.Sic.SiEmpresaDTO empresa = (new COES.Servicios.Aplicacion.General.EmpresaAppServicio()).ObtenerEmpresa(iEmprCodi);

                if (iEmprCodi > 0)
                {
                    if (empresa.Emprestado != "A")
                    {
                        TempData["EmprNomb"] = dtoEmpresa.EmprNombre + "  <span style='color:red'>(EN BAJA)</span>";
                    }
                }
            }
            else if (list.Count() > 1)
            {
                TempData["EMPRNOMB"] = "";
                return View();
            }
            else
            {
                //No hay empresa asociada a la cuenta
                TempData["EMPRNOMB"] = "";
                TempData["EMPRNRO"] = -1;
                return View();
            }

            #endregion

            PeajeEgresoModel model = new PeajeEgresoModel();

            if (pericodi > 0)
            {
                model.ListaPeriodos = new List<PeriodoDTO>();
                model.ListaPeriodos.Add(this.servicioPeriodo.GetByIdPeriodo(pericodi));
                if (model.ListaPeriodos[0] != null)
                {
                    if (model.ListaPeriodos[0].PeriFormNuevo == 1)
                    {
                        return Redirect("~/transferencias/ingresoinfopotenciapeaje/index?pericodi=" + pericodi );
                    }
                }
            }

            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            if (model.ListaPeriodos.Count > 0 && pericodi == 0)
            { pericodi = model.ListaPeriodos[0].PeriCodi; }
            if (pericodi > 0)
            {
                model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(pericodi); //Ordenado en descendente
                if (model.ListaRecalculoPotencia.Count > 0 && recpotcodi == 0)
                { recpotcodi = (int)model.ListaRecalculoPotencia[0].Recpotcodi; }
            }

            if (pericodi > 0 && recpotcodi > 0)
            {
                model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
                if (pegrcodi == 0)
                { model.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresos(iEmprCodi, pericodi, recpotcodi); }
                else
                { model.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi); }
                if (model.EntidadPeajeEgreso == null)
                { model.EntidadPeajeEgreso = new VtpPeajeEgresoDTO(); }
            }
            else
            {
                model.EntidadRecalculoPotencia = new VtpRecalculoPotenciaDTO();
                model.EntidadPeajeEgreso = new VtpPeajeEgresoDTO();
            }
            model.Emprcodi = iEmprCodi;
            model.Pericodi = pericodi;
            model.Recpotcodi = recpotcodi;

            // cambiar al deplegar
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            //model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return View(model);
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult EscogerEmpresa()
        {
            base.ValidarSesionUsuario();

            SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
            List<SeguridadServicio.EmpresaDTO> listTotal = new List<SeguridadServicio.EmpresaDTO>();//
            List<SeguridadServicio.EmpresaDTO> list = new List<SeguridadServicio.EmpresaDTO>();//

            bool accesoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            if (accesoEmpresas)
            {
                listTotal = seguridad.ListarEmpresas().ToList().Where(x => x.TIPOEMPRCODI == 3 || x.EMPRCODI == 11772 || x.EMPRCODI == 13 || x.EMPRCODI == 67).OrderBy(x => x.EMPRNOMB).ToList();
            }
            else
            {
                listTotal = base.ListaEmpresas;
            }

            List<TrnInfoadicionalDTO> listaInfoAdicional = (new TransferenciasAppServicio()).ListTrnInfoadicionals();
            List<int> idsEmpresas = listaInfoAdicional.Where(x => x.Emprcodi != null).Select(x => (int)x.Emprcodi).Distinct().ToList();


            foreach (var item in listTotal)
            {
                list.Add(item);

                //ASSETEC 20190111 - comparar contra otros conceptos
                Int16 iEmpcodi = Convert.ToInt16(item.EMPRCODI);
                foreach (TrnInfoadicionalDTO dtoOtroConcepto in listaInfoAdicional)
                {
                    Int16 iOtroConcepto = Convert.ToInt16(dtoOtroConcepto.Emprcodi);
                    if (iEmpcodi == iOtroConcepto)
                    {
                        SeguridadServicio.EmpresaDTO dtoEmpresaConcepto = new SeguridadServicio.EmpresaDTO();
                        dtoEmpresaConcepto.EMPRCODI = Convert.ToInt16(dtoOtroConcepto.Infadicodi);
                        dtoEmpresaConcepto.EMPRNOMB = dtoOtroConcepto.Infadinomb;
                        dtoEmpresaConcepto.TIPOEMPRCODI = Convert.ToInt16(dtoOtroConcepto.Tipoemprcodi);
                        list.Add(dtoEmpresaConcepto);
                    }
                }
                //--------------------------------------------------
            }

            if (accesoEmpresas)
            {
                list.RemoveAll(x => x.EMPRCODI == 67);
            }


            //UtilTransfPotencia.ObtenerEmpresasPorUsuario(User.Identity.Name);
            BaseModel model = new BaseModel();
            model.ListaEmpresas = new List<EmpresaDTO>();
            foreach (var item in list)
            {
                //if (item.EMPRCODI == 12758)
                //{
                //    item.EMPRCODI = 11567;
                //    item.EMPRNOMB = "STATKRAFT";
                //}

                model.ListaEmpresas.Add(new EmpresaDTO { EmprCodi = item.EMPRCODI, EmprNombre = item.EMPRNOMB });
            }
            return PartialView(model);
        }

        /// Permite actualizar o grabar un registro
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmpresaElegida(int EmprCodi)
        {
            if (EmprCodi != 0)
            {
                Session["EmprCodi"] = EmprCodi;
            }
            return RedirectToAction("Index");
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel para el Desarrollo de peajes e ingresos tarifarios
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int pericodi, int recpotcodi, int emprcodi, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();

            GridExcelModel model = new GridExcelModel();
            PeajeEgresoModel modelpe = new PeajeEgresoModel();

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
            string[] Cabecera2 = { "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };

            //Ancho de cada columna
            int[] widths = { 140, 100, 55, 65, 80, 60, 70, 70, 80, 100, 60, 60, 60 };
            object[] columnas = new object[13];

            bool pegrestado = false;
            model.sEstado = "SI"; //PegrPlazo <- Si entra a liquidación
            //Obtener el periodo de recalculo
            modelpe.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
            //Validamos si el periodo / versión ya esta cerrado 
            //(modelpe.EntidadRecalculoPotencia.Recpotfechalimite < DateTime.Now)
            if (modelpe.EntidadRecalculoPotencia.Recpotestado.Equals("Cerrado"))
            {
                pegrestado = true; //Deshabilita los botones para que grabe o realice cualquier otra acción
            }
            else
            {   //Consultamos por la fecha limite para el envio de información
                try
                {   //Si todo el proceso sale bien 
                    IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                    string sHora = modelpe.EntidadRecalculoPotencia.Recpothoralimite;
                    double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                    double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                    DateTime dDiaHoraLimite = Convert.ToDateTime(modelpe.EntidadRecalculoPotencia.Recpotfechalimite);
                    dDiaHoraLimite = dDiaHoraLimite.AddHours(dHora);
                    dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                    if (dDiaHoraLimite < System.DateTime.Now)
                    {
                        model.sEstado = "NO"; //PegrPlazo <- NO entra a liquidación, la Fecha/Hora limite esmenor a la fecha del sistema
                    }
                }
                catch (Exception e)
                {   // Error en la conversión del tipo hora a fecha.
                    string sMensaje = e.ToString();
                }
            }
            //Obtener las empresas para el dropdown
            var ListaEmpresas = this.servicioEmpresa.ListEmpresasSTR().Select(x => x.EmprNombre).ToList();
            //Obtener las barras para el dropdown
            var ListaBarras = this.servicioBarra.ListBarras().Select(x => x.BarrNombre).ToList();
            //Lista de PeajesEgreso por EmprCodi
            if (pegrcodi == 0)
            { modelpe.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresos(emprcodi, pericodi, recpotcodi); }
            else
            { modelpe.EntidadPeajeEgreso = this.servicioTransfPotencia.GetByIdVtpPeajeEgreso(pegrcodi); }
            
            //Se arma la matriz de datos
            string[][] data;
            if (modelpe.EntidadPeajeEgreso != null)
            {
                if (modelpe.EntidadPeajeEgreso.Pegrestado.Equals("NO"))
                {
                    //Es un envio que ya esta INACTIVO - no es el ultimo
                    pegrestado = true;  //Deshabilita los botones para que grabe o realice cualquier otra acción
                }
                modelpe.ListaPeajeEgresoDetalle = this.servicioTransfPotencia.GetByCriteriaVtpPeajeEgresoDetalles(modelpe.EntidadPeajeEgreso.Pegrcodi);
                data = new string[modelpe.ListaPeajeEgresoDetalle.Count() + 2][];
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
            }
            else
            {
                data = new string[3][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                int index = 2;
                string[] itemDato = { "", "", "", "", "", "", "", "", "", "", "", "", "" };
                data[index] = itemDato;
            }

            ///////////          
            string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
            string[] aLicitacion = { "Si", "No" };
            string[] aCalidad = { "Final", "Preliminar" };

            columnas[0] = new
            {   //Emprcodi - Emprnomb
                type = GridExcelModel.TipoLista,
                source = ListaEmpresas.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[1] = new
            {   //Barrcodi - Barrnombre
                type = GridExcelModel.TipoLista,
                source = ListaBarras.ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = pegrestado,
            };
            columnas[2] = new
            {   //Rpsctipousuario
                type = GridExcelModel.TipoLista,
                source = aTipoUsuario,
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
                type = GridExcelModel.TipoLista,
                source = ListaBarras.ToArray(),
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
            model.FixedRowsTop = 2;
            model.FixedColumnsLeft = 2;
            //ASSETEC 20190219
            if (modelpe.EntidadPeajeEgreso != null)
                model.Pegrcodi = modelpe.EntidadPeajeEgreso.Pegrcodi;

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int pericodi, int recpotcodi, int emprcodi, string testado, List<string[]> datos)
        {
            base.ValidarSesionUsuario();
            int genemprcodi = emprcodi;
            int pegrcodi = 0;
            int NumRegistros = 0;
            int iRegError = 0;
            string sMensajeError = "";
            try
            {
                PeajeEgresoModel model = new PeajeEgresoModel();
                if (testado.Equals(""))
                {
                    testado = "SI";
                    model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
                    if (model.EntidadRecalculoPotencia != null)
                    {
                        IFormatProvider culture = new System.Globalization.CultureInfo("es-PE", true); // Specify exactly how to interpret the string.
                        string sHora = model.EntidadRecalculoPotencia.Recpothoralimite;
                        double dHora = Convert.ToDouble(sHora.Substring(0, sHora.IndexOf(":")));
                        double dMinute = Convert.ToDouble(sHora.Substring(sHora.IndexOf(":") + 1));
                        DateTime dDiaHoraLimite = Convert.ToDateTime(model.EntidadRecalculoPotencia.Recpotfechalimite);
                        dDiaHoraLimite = dDiaHoraLimite.AddHours(dHora);
                        dDiaHoraLimite = dDiaHoraLimite.AddMinutes(dMinute);
                        if (dDiaHoraLimite < System.DateTime.Now)
                        {
                            testado = "NO"; //PegrPlazo <- NO entra a liquidación, la Fecha/Hora limite esmenor a la fecha del sistema
                        }
                    }
                }

                //Graba Cabezera
                model.Entidad = new VtpPeajeEgresoDTO();
                model.Entidad.Pericodi = pericodi;
                model.Entidad.Recpotcodi = recpotcodi;
                model.Entidad.Emprcodi = genemprcodi;
                model.Entidad.Pegrestado = "SI"; //entra a liquidación
                model.Entidad.Pegrplazo = "S"; //esta en plazo
                model.Entidad.Pegrusucreacion = User.Identity.Name;
                model.Entidad.Pegrfeccreacion = DateTime.Now;
                if (testado.Equals("NO"))
                {
                    model.Entidad.Pegrestado = "NO"; //Se graba, pero no entra a liquidación
                    model.Entidad.Pegrplazo = "N"; //Se graba, pero no esta en plazo
                }

                if(model.Entidad.Pegrestado.Equals("SI"))
                {
                    //Antes de grabar cabezera actualiza los estados de "SI" a "NO"
                    this.servicioTransfPotencia.UpdateByCriteriaVtpPeajeEgreso(genemprcodi, pericodi, recpotcodi);
                }

                //Graba nuevo, vacio sin detalle y es el ultimo dato reportado por el agente
                pegrcodi = this.servicioTransfPotencia.SaveVtpPeajeEgreso(model.Entidad);

                //Recorrer matriz para grabar detalle
                //Recorremos la matriz que se inicia en la fila 2

                for (int f = 2; f < datos.Count(); f++)
                {   //Por Fila
                    if (datos[f][0] == null)
                        break; //FIN

                    //INSERTAR EL REGISTRO
                    model.EntidadDetalle = new VtpPeajeEgresoDetalleDTO();
                    model.EntidadDetalle.Pegrcodi = pegrcodi;
                    //cliente
                    if (!datos[f][0].Equals(""))
                    {
                        model.EntidadDetalle.Emprnomb = Convert.ToString(datos[f][0]);
                        EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(model.EntidadDetalle.Emprnomb);
                        if (dtoEmpresa != null)
                        { model.EntidadDetalle.Emprcodi = dtoEmpresa.EmprCodi; }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe: " + model.EntidadDetalle.Emprnomb;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - cliente invalido.";
                        iRegError++;
                        break;
                    }
                    //barra
                    if (!datos[f][1].Equals(""))
                    {
                        model.EntidadDetalle.Barrnombre = Convert.ToString(datos[f][1]);
                        BarraDTO dtoBarra = this.servicioBarra.GetByBarra(model.EntidadDetalle.Barrnombre);
                        if (dtoBarra != null)
                        { model.EntidadDetalle.Barrcodi = dtoBarra.BarrCodi; }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe: " + model.EntidadDetalle.Barrnombre;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - barra invalida.";
                        iRegError++;
                        break;
                    }
                    //tipousuario
                    if (!datos[f][2].Equals(""))
                    {
                        string sTipoUsuario = Convert.ToString(datos[f][2]).ToString().ToUpper();
                        if (sTipoUsuario == "REGULADO")
                        { model.EntidadDetalle.Pegrdtipousuario = "Regulado"; }
                        else if (sTipoUsuario == "LIBRE")
                        { model.EntidadDetalle.Pegrdtipousuario = "Libre"; }
                        else if (sTipoUsuario == "GRAN USUARIO")
                        { model.EntidadDetalle.Pegrdtipousuario = "Gran Usuario"; }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe: " + sTipoUsuario;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - tipo de usuario invalido.";
                        iRegError++;
                        break;
                    }
                    //licitación
                    if (!datos[f][3].Equals("") && Convert.ToString(datos[f][3]).ToString().ToUpper() == "SI")
                    {
                        model.EntidadDetalle.Pegrdlicitacion = "Si";
                    }
                    else
                    {
                        model.EntidadDetalle.Pegrdlicitacion = "No";
                    }
                    model.EntidadDetalle.Pegrdpreciopote = UtilTransfPotencia.ValidarNumero(datos[f][4].ToString());
                    model.EntidadDetalle.Pegrdpoteegreso = UtilTransfPotencia.ValidarNumero(datos[f][5].ToString());
                    model.EntidadDetalle.Pegrdpotecalculada = UtilTransfPotencia.ValidarNumero(datos[f][6].ToString());
                    model.EntidadDetalle.Pegrdpotedeclarada = UtilTransfPotencia.ValidarNumero(datos[f][7].ToString());
                    model.EntidadDetalle.Pegrdpeajeunitario = UtilTransfPotencia.ValidarNumero(datos[f][8].ToString());
                    //Barra FCO
                    if (!datos[f][9].Equals(""))
                    {
                        model.EntidadDetalle.Barrnombrefco = Convert.ToString(datos[f][9]);
                        BarraDTO dtoBarrafco = this.servicioBarra.GetByBarra(model.EntidadDetalle.Barrnombrefco);
                        if (dtoBarrafco != null)
                        { model.EntidadDetalle.Barrcodifco = dtoBarrafco.BarrCodi; }
                        else
                        {
                            sMensajeError += "<br>Fila:" + (f + 1) + " - No existe: " + model.EntidadDetalle.Barrnombrefco;
                            iRegError++;
                            break;
                        }
                    }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (f + 1) + " - barra FCO invalido.";
                        iRegError++;
                        break;
                    }
                    model.EntidadDetalle.Pegrdpoteactiva = UtilTransfPotencia.ValidarNumero(datos[f][10].ToString());
                    model.EntidadDetalle.Pegrdpotereactiva = UtilTransfPotencia.ValidarNumero(datos[f][11].ToString());
                    if (!datos[f][12].Equals("") && Convert.ToString(datos[f][12]).ToString().ToUpper() == "FINAL")
                    {
                        model.EntidadDetalle.Pegrdcalidad = "Final";
                    }
                    else
                    {
                        model.EntidadDetalle.Pegrdcalidad = "Preliminar";
                    }

                    model.EntidadDetalle.Pegrdusucreacion = User.Identity.Name;

                    //Insertar registro
                    this.servicioTransfPotencia.SaveVtpPeajeEgresoDetalle(model.EntidadDetalle);
                    NumRegistros++;
                }
                model.Pegrcodi = pegrcodi;
                model.sFecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").ToString();
                if (!sMensajeError.Equals(""))
                {
                    model.sError = sMensajeError;
                    //No se graba ningun detalle para que este vacio
                    this.servicioTransfPotencia.DeleteVtpPeajeEgresoDetalle(pegrcodi);
                }
                else
                {
                    model.sError = "";
                    model.NumRegistros = NumRegistros;
                    model.sPlazo = model.Entidad.Pegrplazo;
                }

                return Json(model);
            }
            catch (Exception e)
            {
                PeajeEgresoModel model = new PeajeEgresoModel();
                model.Pegrcodi = pegrcodi;
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        #endregion

        /// <summary>
        /// Permite eliminar todos los registros del periodo y versión de recalculo
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatos(int pericodi, int recpotcodi, int emprcodi, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";
            try
            {
                PeajeEgresoModel model = new PeajeEgresoModel();

                //Graba Cabezera
                model.Entidad = new VtpPeajeEgresoDTO();
                model.Entidad.Pericodi = pericodi;
                model.Entidad.Recpotcodi = recpotcodi;
                model.Entidad.Emprcodi = emprcodi;
                model.Entidad.Pegrestado = "SI";
                model.Entidad.Pegrusucreacion = User.Identity.Name;

                //Antes de grabar cabezera actualiza los estados de "SI" a "NO"
                this.servicioTransfPotencia.UpdateByCriteriaVtpPeajeEgreso(emprcodi, pericodi, recpotcodi);

                //Graba nuevo
                pegrcodi = this.servicioTransfPotencia.SaveVtpPeajeEgreso(model.Entidad);
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }

        /// <summary>
        /// Permite exportar a un archivo todos los registros en pantalla
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(int pericodi = 0, int recpotcodi = 0, int pegrcodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
                string file = this.servicioTransfPotencia.GenerarFormatoPeajeEgresoAnterior(pericodi, recpotcodi, pegrcodi, formato, pathFile, pathLogo);
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
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);

            return File(bytes, app, sFecha + "_" + file);
        }

        /// Permite seleccionar a la empresa con la que desea trabajar
        [HttpPost]
        public ActionResult ListaEnvios(int pericodi = 0, int recpotcodi = 0, int emprcodi = 0)
        {
            base.ValidarSesionUsuario();
            PeajeEgresoModel model = new PeajeEgresoModel();
            model.ListaPeajeEgreso = this.servicioTransfPotencia.ListVtpPeajeEgresosView(emprcodi, pericodi, recpotcodi);
            model.EntidadRecalculoPotencia = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            return PartialView(model);
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
            catch
            {
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
        public JsonResult ProcesarArchivo(string sarchivo, int pegrcodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            PeajeEgresoModel modelpe = new PeajeEgresoModel();
            string path = ConfigurationManager.AppSettings[ConstantesTransfPotencia.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";
            try
            {
                #region Armando de contenido
                //Definimos la cabecera como una matriz
                string[] Cabecera1 = { "Cliente", "Barra", "Tipo Usuario", "Licitación", "PARA EGRESO DE POTENCIA", "", "PARA PEAJE POR CONEXIÓN", "", "", "PARA FLUJO DE CARGA OPTIMO", "", "", "Calidad" };
                string[] Cabecera2 = { "", "", "", "", "Precio Potencia S/ /kW-mes", "Potencia Egreso kW", "Potencia Calculada kW", "Potencia Declarada kW", "Peaje Unitario S//kW-mes", "Barra", "Potencia Activa kW", "Potencia Reactiva KW", "" };

                //Ancho de cada columna
                int[] widths = { 140, 100, 55, 65, 80, 60, 70, 70, 80, 100, 60, 60, 60 };
                object[] columnas = new object[13];

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
                data[1] = Cabecera2;
                int index = 2;
                int iFila = 0;
                int iNumRegistros = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 7)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;
                    //CLIENTE
                    string sCliente = dtRow[1].ToString();
                    EmpresaDTO dtoEmpresa = this.servicioEmpresa.GetByNombre(sCliente);
                    if (dtoEmpresa == null)
                    {
                        sMensajeError += "<br>Fila:" + (iNumFila - 5) + " - No existe: " + sCliente;
                        sCliente = "";
                        iRegError++;
                        //continue;
                    }
                    //BARRA
                    string sBarra = dtRow[2].ToString();
                    BarraDTO dtoBarra = this.servicioBarra.GetByBarra(sBarra);
                    if (dtoBarra == null)
                    {
                        sMensajeError += "<br>Fila:" + (iNumFila - 5) + " - No existe: " + sBarra;
                        sBarra = "";
                        iRegError++;
                        //continue;
                    }
                    //tipousuario
                    string sTipoUsuario = Convert.ToString(dtRow[3]).ToString().ToUpper();
                    if (sTipoUsuario == "REGULADO")
                    { sTipoUsuario = "Regulado"; }
                    else if (sTipoUsuario == "LIBRE")
                    { sTipoUsuario = "Libre"; }
                    else if (sTipoUsuario == "GRAN USUARIO")
                    { sTipoUsuario = "Gran Usuario"; }
                    else
                    {
                        sMensajeError += "<br>Fila:" + (iNumFila - 5) + " - No existe: " + sTipoUsuario;
                        sTipoUsuario = "";
                        iRegError++;
                        //break;
                    }
                    //LICITACION
                    string sLicitacion = "No";
                    if (Convert.ToString(dtRow[4]).ToString().ToUpper() == "SI")
                    {
                        sLicitacion = "Si";
                    }
                    //
                    decimal dPreciopote = UtilTransfPotencia.ValidarNumero(dtRow[5].ToString());
                    decimal dPoteegreso = UtilTransfPotencia.ValidarNumero(dtRow[6].ToString());
                    decimal dPotecalculada = UtilTransfPotencia.ValidarNumero(dtRow[7].ToString());
                    decimal dPotedeclarada = UtilTransfPotencia.ValidarNumero(dtRow[8].ToString());
                    decimal dPeajeunitario = UtilTransfPotencia.ValidarNumero(dtRow[9].ToString());
                    //Barra FCO
                    string sBarraFCO = dtRow[10].ToString();
                    BarraDTO dtoBarraFCO = this.servicioBarra.GetByBarra(sBarraFCO);
                    if (dtoBarraFCO == null)
                    {
                        sMensajeError += "<br>Fila:" + (iNumFila - 5) + " - No existe: " + sBarraFCO;
                        sBarraFCO = "";
                        iRegError++;
                        //continue;
                    }
                    decimal dPoteactiva = UtilTransfPotencia.ValidarNumero(dtRow[11].ToString());
                    decimal dPotereactiva = UtilTransfPotencia.ValidarNumero(dtRow[12].ToString());
                    string sCalidad = "Preliminar";
                    if (Convert.ToString(dtRow[13]).ToString().ToUpper() == "FINAL")
                    {
                        sCalidad = "Final";
                    }
                    string[] itemDato = { sCliente, sBarra, sTipoUsuario, sLicitacion, dPreciopote.ToString(), dPoteegreso.ToString(), dPotecalculada.ToString(), dPotedeclarada.ToString(), dPeajeunitario.ToString(), sBarraFCO, dPoteactiva.ToString(), dPotereactiva.ToString(), sCalidad };
                    data[index] = itemDato;
                    index++;
                    iNumRegistros++;
                }

                string[] aTipoUsuario = { "Regulado", "Libre", "Gran Usuario" };
                string[] aLicitacion = { "Si", "No" };
                string[] aCalidad = { "Final", "Preliminar" };
                columnas[0] = new
                {   //Emprcodi - Emprnomb
                    type = GridExcelModel.TipoLista,
                    source = ListaEmpresas.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[1] = new
                {   //Barrcodi - Barrnombre
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
                    strict = false,
                    correctFormat = true,
                    readOnly = pegrestado,
                };
                columnas[2] = new
                {   //Rpsctipousuario
                    type = GridExcelModel.TipoLista,
                    source = aTipoUsuario,
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
                {   //Precio Potencia
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[5] = new
                {   //Potencia Egreso
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[6] = new
                {   //Potencia Calculada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[7] = new
                {   //Potencia Declarada
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[8] = new
                {   //Peaje Unitario
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[9] = new
                {   //Barrcodifco - Barrnombrefco
                    type = GridExcelModel.TipoLista,
                    source = ListaBarras.ToArray(),
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
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[11] = new
                {   //Rpscpotereactiva
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.00",
                    readOnly = pegrestado,
                };
                columnas[12] = new
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
                model.FixedRowsTop = 2;
                model.FixedColumnsLeft = 2;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;
                model.NumRegistros = iNumRegistros;
                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }

        }

    }
}
