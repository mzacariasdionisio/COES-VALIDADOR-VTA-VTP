using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using System.Globalization;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Compensacion;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Helper;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class RecalculoPotenciaController : BaseController
    {
        // GET: /Transfpotencia/RecalculoPotencia/
        //[CustomAuthorize]

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        ConstantesTransfPotencia libFuncion = new ConstantesTransfPotencia();
        ReporteMedidoresAppServicio servReporte = new ReporteMedidoresAppServicio();
        Servicios.Aplicacion.Compensacion.CompensacionAppServicio servicio = new Servicios.Aplicacion.Compensacion.CompensacionAppServicio();
        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            RecalculoPotenciaModel model = new RecalculoPotenciaModel();
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de datos de la RecalculoPotencia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista()
        {
            RecalculoPotenciaModel model = new RecalculoPotenciaModel();
            model.ListaRecalculoPotencia = this.servicioTransfPotencia.ListVtpRecalculoPotenciasView(); //Lista todas la lista de la tabla RecalculoPotencia incluido el atributo Nombre area
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para ingresar un nuevo registro
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            RecalculoPotenciaModel model = new RecalculoPotenciaModel();
            model.Entidad = new VtpRecalculoPotenciaDTO();
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.Entidad.Pericodi = 0;
            model.Entidad.Recpotcodi = 0;
            model.Recpotinterpuntames = "";// System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Recpotfechalimite = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.Entidad.Recpothoralimite = "20:00";
            model.Entidad.Recpotestado = "Abierto";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo de Potencia</param>
        /// <returns></returns>
        public ActionResult Edit(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            RecalculoPotenciaModel model = new RecalculoPotenciaModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(pericodi, recpotcodi);
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.Recpotinterpuntames = model.Entidad.Recpotinterpuntames.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
            model.Recpotfechalimite = model.Entidad.Recpotfechalimite.GetValueOrDefault().ToString("dd/MM/yyyy");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.ListaRecalculo = this.servicioRecalculo.ListRecalculos(pericodi);
            var cargaPfr = model.Entidad.Recpotcargapfr;
            model.Entidad.Recpotcargapfr = cargaPfr == null? 0 : cargaPfr;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name); 
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del formulario
        /// </summary>
        /// <param name="model">Contiene los datos del regitsro a grabar</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RecalculoPotenciaModel model)
        {
            base.ValidarSesionUsuario();
            if (ModelState.IsValid)
            {
                var EmpresasTTIE = servicioTransferencia.ListTrnMigracionDTI().OrderByDescending(x => x.Migracodi).ToList();
                model.Entidad.Recpotusumodificacion = User.Identity.Name;
                if (model.Recpotinterpuntames != "" && model.Recpotinterpuntames != null)
                    model.Entidad.Recpotinterpuntames = DateTime.ParseExact(model.Recpotinterpuntames, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                if (model.Recpotfechalimite != "" && model.Recpotfechalimite != null)
                    model.Entidad.Recpotfechalimite = DateTime.ParseExact(model.Recpotfechalimite, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (model.Entidad.Recpotpreciopeajeppm == null)
                    model.Entidad.Recpotpreciopeajeppm = 0;

                if (model.Entidad.Recpotcodi == 0)
                {   //Crear registro
                    model.Entidad.Recpotusucreacion = User.Identity.Name;
                    model.Entidad.Recpotcodi = this.servicioTransfPotencia.SaveVtpRecalculoPotencia(model.Entidad);
                    if (model.Entidad.Recpotcodi > 1)
                    {   //ID > 1: Son nuevos recalculos, hay que realizar una nueva versión de la información en las tablas:
                        int iPeriCodi = model.Entidad.Pericodi;
                        int iVersionNew = model.Entidad.Recpotcodi;
                        int iVersionOld = model.Entidad.Recpotcodi - 1; //Versión anterior a duplicar
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Paso 1 - Duplicamos la información de la tabla VTP_REPA_RECA_PEA = JE a la nueva versión
                        RepaRecaPeajeModel modelRecaPeaje = new RepaRecaPeajeModel();
                        modelRecaPeaje.ListaRepaRecaPeaje = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(iPeriCodi, iVersionOld); //Lista todos los registros de la tabla VTP_REPA_RECA_PEAJE
                        foreach (var dtoRepaRecaPeaje in modelRecaPeaje.ListaRepaRecaPeaje)
                        {
                            int RrpecodiOld = dtoRepaRecaPeaje.Rrpecodi; //Código antiguo
                            dtoRepaRecaPeaje.Pericodi = iPeriCodi;
                            dtoRepaRecaPeaje.Recpotcodi = iVersionNew;
                            int RrpecodiNew = this.servicioTransfPotencia.SaveVtpRepaRecaPeaje(dtoRepaRecaPeaje); //Devuelve el id RepaRecaPeaje
                            //Duplicamos la información de la tabla VTP_REPA_RECA_PEAJE_DETALLE a la nueva versión
                            RepaRecaPeajeDetalleModel modelRepaRecaPeajeDetalle = new RepaRecaPeajeDetalleModel();
                            modelRepaRecaPeajeDetalle.ListaRepaRecaPeajeDetalle = this.servicioTransfPotencia.ListVtpRepaRecaPeajeDetalles(iPeriCodi, iVersionOld, RrpecodiOld); //Consultamos el detalle de los registros Old
                            foreach (var dtoRepaRecaPeajeDetalle in modelRepaRecaPeajeDetalle.ListaRepaRecaPeajeDetalle)
                            {
                                dtoRepaRecaPeajeDetalle.Rrpdcodi = 0;  //Se insertara un nuevo registro
                                dtoRepaRecaPeajeDetalle.Pericodi = iPeriCodi;
                                dtoRepaRecaPeajeDetalle.Recpotcodi = iVersionNew; //Se agrega la nueva versión
                                dtoRepaRecaPeajeDetalle.Rrpecodi = RrpecodiNew; //Se agrega el nuevo código de VTP_REPA_RECA_PEAJE
                                this.servicioTransfPotencia.SaveVtpRepaRecaPeajeDetalle(dtoRepaRecaPeajeDetalle); //Insertamos detalle
                            }
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Paso 2 - Duplicamos la información de la tabla VTP_PEAJE_INGRESO a la nueva versión
                        PeajeIngresoModel modelPeajeIngreso = new PeajeIngresoModel();
                        modelPeajeIngreso.ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(iPeriCodi, iVersionOld);//Lista todos los registros
                        foreach (var dtoPeajeIngreso in modelPeajeIngreso.ListaPeajeIngreso)
                        {
                            int PingcodiOld = dtoPeajeIngreso.Pingcodi; //Código antiguo
                            dtoPeajeIngreso.Pericodi = iPeriCodi;
                            dtoPeajeIngreso.Recpotcodi = iVersionNew;
                            // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                            var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoPeajeIngreso.Emprcodi || x.Emprcodidestino == dtoPeajeIngreso.Emprcodi).ToList();
                            if (empresaTTIE.Count > 0) dtoPeajeIngreso.Emprcodi = empresaTTIE.First().Emprcodidestino;
                            // FIN
                            this.servicioTransfPotencia.SaveVtpPeajeIngreso(dtoPeajeIngreso);
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Paso 3 - Duplicamos la información de la tabla VTP_RETIRO_POTESC a la nueva versión
                        RetiroPotenciaSCModel modelRetiroPotenciaSC = new RetiroPotenciaSCModel();
                        modelRetiroPotenciaSC.ListaRetiroPotenciaSC = this.servicioTransfPotencia.ListVtpRetiroPotenciaSCView(iPeriCodi, iVersionOld);//Lista todos los registros
                        foreach (var dtoRetiroPotenciaSC in modelRetiroPotenciaSC.ListaRetiroPotenciaSC)
                        {
                            int RpsccodiOld = dtoRetiroPotenciaSC.Rpsccodi; //Código antiguo
                            dtoRetiroPotenciaSC.Rpsccodi = 0;
                            dtoRetiroPotenciaSC.Pericodi = iPeriCodi;
                            dtoRetiroPotenciaSC.Recpotcodi = iVersionNew;
                            // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                            var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoRetiroPotenciaSC.Emprcodi || x.Emprcodidestino == dtoRetiroPotenciaSC.Emprcodi).ToList();
                            if (empresaTTIE.Count > 0) dtoRetiroPotenciaSC.Emprcodi = empresaTTIE.First().Emprcodidestino;
                            //FIN
                            this.servicioTransfPotencia.SaveVtpRetiroPotesc(dtoRetiroPotenciaSC);
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Paso 4 - Duplicamos la información de la tabla VTP_INGRESO_POTEFR a la nueva versión
                        IngresoPotefrModel modelIngresoPotefr = new IngresoPotefrModel();
                        modelIngresoPotefr.ListaIngresoPotefr = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrs(iPeriCodi, iVersionOld); //Lista todos los registros de la tabla VTP_INGRESO_POTEFR
                        foreach (var dtoIngresoPotefr in modelIngresoPotefr.ListaIngresoPotefr)
                        {
                            int IpefrcodiOld = dtoIngresoPotefr.Ipefrcodi; //Código antiguo
                            dtoIngresoPotefr.Pericodi = iPeriCodi;
                            dtoIngresoPotefr.Ipefrcodi = 0;
                            dtoIngresoPotefr.Recpotcodi = iVersionNew;
                            int IpefrcodiNew = this.servicioTransfPotencia.SaveVtpIngresoPotefr(dtoIngresoPotefr); //Devuelve el id IngresoPotefr
                            //Duplicamos la información de la tabla VTP_INGRESO_POTEFR_DETALLE a la nueva versión
                            IngresoPotefrDetalleModel modelIngresoPotefrDetalle = new IngresoPotefrDetalleModel();
                            modelIngresoPotefrDetalle.ListaIngresoPotefrDetalle = this.servicioTransfPotencia.GetByCriteriaVtpIngresoPotefrDetalles(IpefrcodiOld, iPeriCodi, iVersionOld); //Consultamos el detalle de los registros Old
                            foreach (var dtoIngresoPotefrDetalle in modelIngresoPotefrDetalle.ListaIngresoPotefrDetalle)
                            {
                                dtoIngresoPotefrDetalle.Ipefrdcodi = 0;  //Se insertara un nuevo registro
                                dtoIngresoPotefrDetalle.Pericodi = iPeriCodi;
                                dtoIngresoPotefrDetalle.Recpotcodi = iVersionNew; //Se agrega la nueva versión
                                dtoIngresoPotefrDetalle.Ipefrcodi = IpefrcodiNew; //Se agrega el nuevo código de VTP_INGRESO_POTEFR_DETALLE
                                // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                                var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoIngresoPotefrDetalle.Emprcodi || x.Emprcodidestino == dtoIngresoPotefrDetalle.Emprcodi).ToList();
                                if (empresaTTIE.Count > 0) dtoIngresoPotefrDetalle.Emprcodi = empresaTTIE.First().Emprcodidestino;
                                // FIN
                                this.servicioTransfPotencia.SaveVtpIngresoPotefrDetalle(dtoIngresoPotefrDetalle); //Insertamos detalle
                            }
                        }
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Paso 5 - Duplicamos la información de la tabla VTP_PEAJE_EGRESO a la nueva versión
                        PeajeEgresoModel modelPeajeEgreso = new PeajeEgresoModel();

                        modelPeajeEgreso.ListaPeajeEgresoEmpresa = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoCabeceraRecalculo(iPeriCodi, iVersionOld);
                        foreach (VtpPeajeEgresoMinfoDTO dtoPeajeEgresoEmpresa in modelPeajeEgreso.ListaPeajeEgresoEmpresa) //var ListaPeajeEgreso
                        {
                            modelPeajeEgreso.Entidad = new VtpPeajeEgresoDTO();
                            modelPeajeEgreso.Entidad.Pericodi = iPeriCodi;
                            modelPeajeEgreso.Entidad.Recpotcodi = iVersionNew;
                            // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                            int nuevoCodEmp = (int)dtoPeajeEgresoEmpresa.Genemprcodi;
                            var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoPeajeEgresoEmpresa.Genemprcodi || x.Emprcodidestino == dtoPeajeEgresoEmpresa.Genemprcodi).ToList();
                            if (empresaTTIE.Count > 0) nuevoCodEmp = empresaTTIE.First().Emprcodidestino;
                            modelPeajeEgreso.Entidad.Emprcodi = nuevoCodEmp;
                            //FIN
                            modelPeajeEgreso.Entidad.Pegrestado = "SI"; //entra a liquidación
                            modelPeajeEgreso.Entidad.Pegrplazo = "S"; //esta en plazo
                            modelPeajeEgreso.Entidad.Pegrusucreacion = User.Identity.Name;
                            modelPeajeEgreso.Entidad.Pegrfeccreacion = DateTime.Now;
                            int pegrcodi = this.servicioTransfPotencia.SaveVtpPeajeEgreso(modelPeajeEgreso.Entidad);
                            //Duplicamos la información de la tabla VTP_PEAJE_EGRESO_DETALLE a la nueva versión
                            //modelPeajeEgreso.ListaPeajeEgresoMinfo = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoEmpresaNuevo(iPeriCodi, iVersionOld, (int)dtoPeajeEgresoEmpresa.Genemprcodi);
                            modelPeajeEgreso.ListaPeajeEgresoMinfo = this.servicioTransfPotencia.ListVtpPeajeEgresoMinfoEmpresa(iPeriCodi, iVersionOld, (int)dtoPeajeEgresoEmpresa.Genemprcodi);
                            foreach (VtpPeajeEgresoMinfoDTO dtoPeajeEgreso in modelPeajeEgreso.ListaPeajeEgresoMinfo)
                            {
                                modelPeajeEgreso.EntidadDetalle = new VtpPeajeEgresoDetalleDTO();
                                modelPeajeEgreso.EntidadDetalle.Pegrcodi = pegrcodi;
                                // ACTUALIZA LOS CÓDIGOS DE CLIENTE(EMP) QUE TIENEN TTIE POR LOS NUEVOS
                                int nuevoCodCli = (int)dtoPeajeEgreso.Cliemprcodi;
                                var clienteTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoPeajeEgreso.Cliemprcodi || x.Emprcodidestino == dtoPeajeEgreso.Cliemprcodi).ToList();
                                if (clienteTTIE.Count > 0) nuevoCodEmp = clienteTTIE.First().Emprcodidestino;
                                modelPeajeEgreso.EntidadDetalle.Emprcodi = nuevoCodCli;
                                //FIN
                                modelPeajeEgreso.EntidadDetalle.Barrcodi = dtoPeajeEgreso.Barrcodi;
                                modelPeajeEgreso.EntidadDetalle.Pegrdtipousuario = dtoPeajeEgreso.Pegrmitipousuario;
                                modelPeajeEgreso.EntidadDetalle.Pegrdlicitacion = dtoPeajeEgreso.Pegrmilicitacion;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpreciopote = dtoPeajeEgreso.Pegrmipreciopote;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpoteegreso = dtoPeajeEgreso.Pegrmipoteegreso;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpotecalculada = dtoPeajeEgreso.Pegrmipotecalculada;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpotedeclarada = dtoPeajeEgreso.Pegrmipotedeclarada;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpeajeunitario = dtoPeajeEgreso.Pegrmipeajeunitario;
                                modelPeajeEgreso.EntidadDetalle.Barrcodifco = dtoPeajeEgreso.Barrcodifco;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpoteactiva = dtoPeajeEgreso.Pegrmipoteactiva;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpotereactiva = dtoPeajeEgreso.Pegrmipotereactiva;
                                modelPeajeEgreso.EntidadDetalle.Pegrdcalidad = dtoPeajeEgreso.Pegrmicalidad;
                                modelPeajeEgreso.EntidadDetalle.Pegrdusucreacion = User.Identity.Name;
                                modelPeajeEgreso.EntidadDetalle.Coregecodvtp = dtoPeajeEgreso.Coregecodvtp;
                                modelPeajeEgreso.EntidadDetalle.Pegrdpotecoincidente = dtoPeajeEgreso.Pegrdpotecoincidente;
                                modelPeajeEgreso.EntidadDetalle.Pegrdfacperdida = dtoPeajeEgreso.Pegrdfacperdida;
                                if (dtoPeajeEgreso.Tipconcodi == null)
                                {
                                    if (dtoPeajeEgreso.Pegrmilicitacion.ToUpper() == "SI")
                                    {
                                        dtoPeajeEgreso.Tipconcodi = Funcion.TipoContratoLicitacion;
                                    }
                                    else if (dtoPeajeEgreso.Tipconcodi == null)
                                    {
                                        if (dtoPeajeEgreso.Pegrmipotedeclarada == 0)
                                        {
                                            dtoPeajeEgreso.Tipconcodi = Funcion.TipoContratoAutoconsumo;
                                        }
                                        else
                                        {
                                            dtoPeajeEgreso.Tipconcodi = Funcion.TipoContratoBilateral;
                                        }
                                    }
                                }
                                modelPeajeEgreso.EntidadDetalle.TipConCondi = (int)dtoPeajeEgreso.Tipconcodi;
                                //tipo licitacion

                                this.servicioTransfPotencia.SaveVtpPeajeEgresoDetalle(modelPeajeEgreso.EntidadDetalle);
                            }
                        }

                    }
                    else if (model.Entidad.Recpotcodi == 1)
                    {   //ID=1: Mes Inicial, hay que copiar las listas del periodo anterior versión 1
                        PeriodoDTO dtoPeriodoAnterior = this.servicioPeriodo.BuscarPeriodoAnterior(model.Entidad.Pericodi);
                        if (dtoPeriodoAnterior != null)
                        {
                            int iPeriCodiNew = model.Entidad.Pericodi;
                            int iPeriCodiOld = dtoPeriodoAnterior.PeriCodi;
                            int iVersion = this.servicioTransfPotencia.GetByMaxIdRecPotCodi(iPeriCodiOld); //model.Entidad.Recpotcodi;
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //Paso 1 - Duplicamos la información de la tabla VTP_REPA_RECA_PEAJE a la nueva versión
                            RepaRecaPeajeModel modelRecaPeaje = new RepaRecaPeajeModel();
                            modelRecaPeaje.ListaRepaRecaPeaje = this.servicioTransfPotencia.GetByCriteriaVtpRepaRecaPeajes(iPeriCodiOld, iVersion); //Lista todos los registros de la tabla VTP_REPA_RECA_PEAJE
                            foreach (var dtoRepaRecaPeaje in modelRecaPeaje.ListaRepaRecaPeaje)
                            {
                                int RrpecodiOld = dtoRepaRecaPeaje.Rrpecodi; //Código antiguo
                                dtoRepaRecaPeaje.Pericodi = iPeriCodiNew;
                                dtoRepaRecaPeaje.Recpotcodi = model.Entidad.Recpotcodi;
                                int RrpecodiNew = this.servicioTransfPotencia.SaveVtpRepaRecaPeaje(dtoRepaRecaPeaje); //Devuelve el id RepaRecaPeaje
                            }
                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //Paso 2 - Duplicamos la información de la tabla VTP_PEAJE_INGRESO a la nueva versión
                            /*PeajeIngresoModel modelPeajeIngreso = new PeajeIngresoModel();
                            modelPeajeIngreso.ListaPeajeIngreso = this.servicioTransfPotencia.ListVtpPeajeIngresoView(iPeriCodiOld, iVersion);//Lista todos los registros
                            foreach (var dtoPeajeIngreso in modelPeajeIngreso.ListaPeajeIngreso)
                            {
                                int PingcodiOld = dtoPeajeIngreso.Pingcodi; //Código antiguo
                                dtoPeajeIngreso.Pericodi = iPeriCodiNew;
                                dtoPeajeIngreso.Recpotcodi = model.Entidad.Recpotcodi;
                                // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                                var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoPeajeIngreso.Emprcodi).ToList();
                                if (empresaTTIE.Count > 0) dtoPeajeIngreso.Emprcodi = empresaTTIE.First().Emprcodidestino;
                                // FIN
                                this.servicioTransfPotencia.SaveVtpPeajeIngreso(dtoPeajeIngreso);
                            }*/
                        }
                    }
                    TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkInsertarReistro;
                }
                else
                {
                    if (model.Entidad.Recpotcodi > 1)
                    {
                        //Para el caso en que se cambie el PeriCodiDestino
                        int iPeriCodi = model.Entidad.Pericodi;
                        int iVersion = model.Entidad.Recpotcodi;
                        VtpRecalculoPotenciaDTO dtoActual = this.servicioTransfPotencia.GetByIdVtpRecalculoPotencia(iPeriCodi, iVersion);
                        if (dtoActual.Pericodidestino != model.Entidad.Pericodidestino)
                        {
                            //Mover los saldos a otro periodo
                            int iPeriCodiDestino = (int)model.Entidad.Pericodidestino;
                            //VTP_PEAJE_CARGO->PECARPERICODIDEST
                            this.servicioTransfPotencia.UpdateVtpPeajeCargoPeriodoDestino(iPeriCodi, iVersion, (int)model.Entidad.Pericodidestino);
                            //VTP_INGRESO_TARIFARIO->INGTARPERICODIDEST
                            this.servicioTransfPotencia.UpdateVtpIngresoTarifarioPeriodoDestino(iPeriCodi, iVersion, (int)model.Entidad.Pericodidestino);
                            //VTP_SALDO_EMPRESA->POTSEPERICODIDEST
                            this.servicioTransfPotencia.UpdateVtpSaldoEmpresaPeriodoDestino(iPeriCodi, iVersion, (int)model.Entidad.Pericodidestino);
                            //VTP_PEAJE_EMPRESA_PAGO->PEMPAGPERICODIDEST
                            this.servicioTransfPotencia.UpdateVtpPeajeEmpresaPagoPeriodoDestino(iPeriCodi, iVersion, (int)model.Entidad.Pericodidestino);
                        }
                    }
                    //Editar registro actual
                    this.servicioTransfPotencia.UpdateVtpRecalculoPotencia(model.Entidad);
                    TempData["sMensajeExito"] = ConstantesTransfPotencia.MensajeOkEditarReistro;
                }

                return RedirectToAction("Index");
            }
            //Error
            model.sError = ConstantesTransfPotencia.MensajeErrorGrabarReistro;
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int pericodi = 0, int recpotcodi = 0)
        {
            base.ValidarSesionUsuario();
            //Eliminamos la información duplicada en el periodo / versión
            string sBorrar = this.servicioTransfPotencia.EliminarVersion(pericodi, recpotcodi);
            if (!sBorrar.Equals("1"))
            {
                return sBorrar;
            }

            this.servicioTransfPotencia.DeleteVtpRecalculoPotencia(pericodi, recpotcodi);
            return "true";
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <param name="recpotcodi">Código de la Versión de Recálculo</param>
        /// <returns></returns>
        public ActionResult View(int pericodi = 0, int recpotcodi = 0)
        {
            RecalculoPotenciaModel model = new RecalculoPotenciaModel();
            model.Entidad = this.servicioTransfPotencia.GetByIdVtpRecalculoPotenciaView(pericodi, recpotcodi);
            return PartialView(model);
        }


        [HttpPost]
        public ActionResult ObtenerMaximaDemanda(int periodo)
        {
            string resultado = "";
            //string[] periodo = mes.Split('.');
            string fechaMaximaDemanda = "";


            try
            {
                var periodoEntidad = (new PeriodoAppServicio()).GetByIdPeriodo(periodo); 
                var oMes = periodoEntidad.MesCodi.ToString();
                var oAnio = periodoEntidad.AnioCodi.ToString();
                var mes = string.Format("{0} {1}", oMes.PadLeft(2, '0'), oAnio);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, string.Empty, string.Empty, string.Empty);
                int estadoValidacion = 0;
                int tipoCentral = 1;
                int tipoGeneracion = -1;
                var idEmpresa = -1;
                DateTime fechaIni = fechaProceso;
                DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
                DateTime diaMaximaDemanda = this.servReporte.GetDiaPeriodoDemanda96XFiltro(fechaIni, fechaFin,
                    ConstantesRepMaxDemanda.TipoMDNormativa, tipoCentral, tipoGeneracion, ConstantesMedicion.IdEmpresaTodos, estadoValidacion);

                var maximaDemandaDTO = this.servReporte.GetResumenDiaMaximaDemanda96(diaMaximaDemanda, tipoCentral, tipoGeneracion,
                idEmpresa, estadoValidacion);
                fechaMaximaDemanda = maximaDemandaDTO.FechaOnlyDia + " " + maximaDemandaDTO.FechaOnlyHora;

                resultado = fechaMaximaDemanda; //diaMaximaDemanda.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                resultado = DateTime.Now.ToString("dd/M/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }

            return Json(resultado);
        }
    }
}
