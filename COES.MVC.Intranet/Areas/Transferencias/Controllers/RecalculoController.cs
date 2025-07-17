using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class RecalculoController : Controller
    {
        // GET: /Transferencias/Recalculo/
        //[CustomAuthorize]
        //Declaración de servicios:
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        FactorPerdidaAppServicio servicioFactorPerdida = new FactorPerdidaAppServicio();
        CostoMarginalAppServicio servicioCostoMarginal = new CostoMarginalAppServicio();
        CodigoEntregaAppServicio servicioTrnCodigoEntrega = new CodigoEntregaAppServicio();
        TransferenciaEntregaRetiroAppServicio servicioTransEntregaRetiro = new TransferenciaEntregaRetiroAppServicio();
        ValorTransferenciaAppServicio servicioValorTransferencia = new ValorTransferenciaAppServicio();
        TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        ValorTransferenciaController controllerTransferencia = new ValorTransferenciaController();
        Funcion archivoFuncion = new Funcion();

        public ActionResult Index(int id = 0)
        {
            if (id != 0)
            {
                Session["sPericodi"] = id;
                PeriodoModel modelPeriodo = new PeriodoModel();
                modelPeriodo.Entidad = this.servicioPeriodo.GetByIdPeriodo(id);
                TempData["Perinombre"] = modelPeriodo.Entidad.PeriNombre;
                Session["Perinombre"] = modelPeriodo.Entidad.PeriNombre;
            }
            RecalculoModel model = new RecalculoModel();
            int iNroRecalculosAbiertos = 0;
            model.ListaRecalculo = this.servicioRecalculo.ListRecalculos(id);
            foreach (var entidad in model.ListaRecalculo)
            {
                if (entidad.RecaEstado.Equals("Abierto") || entidad.RecaEstado.Equals("Publicar"))
                    iNroRecalculosAbiertos++;
            }
            if (iNroRecalculosAbiertos == 0)
                model.bNuevo = this.archivoFuncion.ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            else
                model.bNuevo = false;

            model.bNuevo = true;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            int id = Convert.ToInt32(Session["sPericodi"].ToString());
            RecalculoModel model = new RecalculoModel();
            model.ListaRecalculo = this.servicioRecalculo.ListRecalculos(id);
            model.bEditar = this.archivoFuncion.ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = this.archivoFuncion.ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult New()
        {
            RecalculoModel modelo = new RecalculoModel();
            modelo.Entidad = new RecalculoDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.RecaCodi = 0;
            modelo.Entidad.RecaPeriCodi = Convert.ToInt32(Session["sPericodi"].ToString());
            modelo.Entidad.RecaHoraLimite = "23:59";
            modelo.Recafechavalorizacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Recafechalimite = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Recafechaobservacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Entidad.RecaEstado = "Abierto";
            modelo.bGrabar = this.archivoFuncion.ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = this.servicioPeriodo.ListarPeriodosFuturos(modelo.Entidad.RecaPeriCodi);
            TempData["PeriodoDestino"] = new SelectList(modelPeriodo.ListaPeriodos, "PeriCodi", "PeriNombre");
            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RecalculoModel modelo)
        {
            bool bDuplicarData = false;
            if (ModelState.IsValid)
            {
                var EmpresasTTIE = servicioTransferencia.ListTrnMigracionDTI().OrderByDescending(x => x.Migracodi).ToList();

                int iPericodi = Convert.ToInt32(Session["sPericodi"].ToString());

                if (modelo.Entidad.RecaCodi == 0)
                {   //Acción Nuevo: Hay que realizar una nueva versión de la información en las tablas:
                    bDuplicarData = true;
                }
                if (modelo.pericodidestino > 0)
                    modelo.Entidad.PeriCodiDestino = (int)modelo.pericodidestino;
                if (modelo.Recafechavalorizacion != "" && modelo.Recafechavalorizacion != null)
                    modelo.Entidad.RecaFechaValorizacion = DateTime.ParseExact(modelo.Recafechavalorizacion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Recafechalimite != "" && modelo.Recafechalimite != null)
                    modelo.Entidad.RecaFechaLimite = DateTime.ParseExact(modelo.Recafechalimite, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Recafechaobservacion != "" && modelo.Recafechaobservacion != null)
                    modelo.Entidad.RecaFechaObservacion = DateTime.ParseExact(modelo.Recafechaobservacion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                //Actualizamos la información de consulta del Periodo
                PeriodoModel modeloperiodo = new PeriodoModel();
                modeloperiodo.Entidad = this.servicioPeriodo.GetByIdPeriodo(iPericodi);
                modeloperiodo.Entidad.PeriFechaValorizacion = modelo.Entidad.RecaFechaValorizacion;
                modeloperiodo.Entidad.PeriFechaLimite = modelo.Entidad.RecaFechaLimite;
                modeloperiodo.Entidad.PeriHoraLimite = modelo.Entidad.RecaHoraLimite;
                modeloperiodo.Entidad.PeriFechaObservacion = modelo.Entidad.RecaFechaObservacion;
                modeloperiodo.Entidad.PeriEstado = modelo.Entidad.RecaEstado;
                modeloperiodo.Entidad.RecaNombre = modelo.Entidad.RecaNombre;
                modeloperiodo.IdPeriodo = this.servicioPeriodo.SaveOrUpdatePeriodo(modeloperiodo.Entidad);
                //Información del Recalculo
                modelo.Entidad.RecaUserName = User.Identity.Name;
                modelo.IdRecalculo = this.servicioRecalculo.SaveOrUpdateRecalculo(modelo.Entidad); //2; // 
                if (bDuplicarData && modelo.IdRecalculo > 1)
                {
                    //Ojo (IdRecalculo > 1), replicamos la data de la versión anterior, sólo se da en la opción nueva versión
                    
                    int iPeriCodi = modelo.Entidad.RecaPeriCodi;
                    int iVersionNew = modelo.IdRecalculo;
                    int iVersionOld = modelo.IdRecalculo - 1; //Versión anterior a duplicar
                    //---------------------------------------------------------------------------------------------------------
                    //Duplicamos los Factores de Perdida, Costos Marginales, Entregas y Retiros:
                    this.servicioRecalculo.CopiarRecalculo(iPeriCodi, iVersionNew, iVersionOld);
                    //ASSETEC 202209 - Duplicamos los Ajustes de Costos Marginales, Tabla TRN_COSTO_MARGINAL_AJUSTE
                    this.servicioFactorPerdida.CopiarAjustesCostosMarginales(iPeriCodi, iVersionNew, iVersionOld);
                    //---------------------------------------------------------------------------------------------------------
                    //Duplicamos la información de la tabla TRN_ING_COMPENSACION(PERICODI, INGCOMVERSION) a la nueva versión
                    IngresoCompensacionModel modelIngCompen = new IngresoCompensacionModel();
                    modelIngCompen.ListaIngresoCompensacion = (new IngresoCompensacionAppServicio()).ListByPeriodoVersion(iPeriCodi, iVersionOld);
                    foreach (var dtoIC in modelIngCompen.ListaIngresoCompensacion)
                    {
                        dtoIC.IngrCompCodi = 0;  //Se insertara un nuevo registro
                        dtoIC.IngrCompVersion = iVersionNew; //Se agrega la nueva versión
                        // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                        var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoIC.EmprCodi || x.Emprcodidestino == dtoIC.EmprCodi).ToList();
                        if (empresaTTIE.Count > 0) dtoIC.EmprCodi = empresaTTIE.First().Emprcodidestino;
                        // FIN
                    }
                    var duplicadosIng = modelIngCompen.ListaIngresoCompensacion.GroupBy(x => x.EmprCodi).Where(emp => emp.Count() > 1).Select(x => x.Key).ToList();
                    if (duplicadosIng.Any())
                    {
                        foreach (var codigoEmpresaDuplicado in duplicadosIng)
                        {
                            decimal sum = modelIngCompen.ListaIngresoCompensacion.Where(x => x.EmprCodi == codigoEmpresaDuplicado).Sum(x => x.IngrCompImporte);
                            var dtoICd = modelIngCompen.ListaIngresoCompensacion.Where(x => x.EmprCodi == codigoEmpresaDuplicado).FirstOrDefault();
                            dtoICd.IngrCompImporte = sum;
                            dtoICd.IngrCompCodi = (new IngresoCompensacionAppServicio()).SaveIngresoCompensacion(dtoICd);
                            modelIngCompen.ListaIngresoCompensacion.RemoveAll(x => x.EmprCodi == codigoEmpresaDuplicado);
                        }
                    }
                    foreach (var dtoIC in modelIngCompen.ListaIngresoCompensacion)
                    {
                        dtoIC.IngrCompCodi = (new IngresoCompensacionAppServicio()).SaveIngresoCompensacion(dtoIC);
                    }
                    //---------------------------------------------------------------------------------------------------------
                    //Duplicamos la información de la tabla TRN_ING_POTENCIA(PERICODI, INGPOTVERSION) a la nueva versión
                    IngresoPotenciaModel modelIngPoten = new IngresoPotenciaModel();
                    modelIngPoten.ListaIngresoPotencia = (new IngresoPotenciaAppServicio()).ListByPeriodoVersion(iPeriCodi, iVersionOld);
                    foreach (var dtoIP in modelIngPoten.ListaIngresoPotencia)
                    {
                        dtoIP.IngrPoteCodi = 0;  //Se insertara un nuevo registro
                        dtoIP.IngrPoteVersion = iVersionNew; //Se agrega la nueva versión
                        // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                        var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoIP.EmprCodi || x.Emprcodidestino == dtoIP.EmprCodi).ToList();
                        if (empresaTTIE.Count > 0) dtoIP.EmprCodi = empresaTTIE.First().Emprcodidestino;
                        // FIN
                    }
                    var duplicadosPot = modelIngPoten.ListaIngresoPotencia.GroupBy(x => x.EmprCodi).Where(emp => emp.Count() > 1).Select(x => x.Key).ToList();
                    if (duplicadosPot.Any())
                    {
                        foreach (var codigoEmpresaDuplicado in duplicadosPot)
                        {
                            decimal sum = modelIngPoten.ListaIngresoPotencia.Where(x => x.EmprCodi == codigoEmpresaDuplicado).Sum(x => x.IngrPoteImporte);
                            var dtoIPd = modelIngPoten.ListaIngresoPotencia.Where(x => x.EmprCodi == codigoEmpresaDuplicado).FirstOrDefault();
                            dtoIPd.IngrPoteImporte = sum;
                            dtoIPd.IngrPoteCodi = (new IngresoPotenciaAppServicio()).SaveIngresoPotencia(dtoIPd);
                            modelIngPoten.ListaIngresoPotencia.RemoveAll(x => x.EmprCodi == codigoEmpresaDuplicado);
                        }
                    }
                    foreach (var dtoIP in modelIngPoten.ListaIngresoPotencia)
                    {
                        dtoIP.IngrPoteCodi = (new IngresoPotenciaAppServicio()).SaveIngresoPotencia(dtoIP);
                    }
                    //---------------------------------------------------------------------------------------------------------
                    //Duplicamos la información de la tabla TRN_ING_RETIROSC(PERICODI, INGRSCVERSION) a la nueva versión
                    IngresoRetiroSCModel modelIngRSC = new IngresoRetiroSCModel();
                    modelIngRSC.ListaIngresoRetiroSC = (new IngresoRetiroSCAppServicio()).ListByPeriodoVersion(iPeriCodi, iVersionOld);
                    foreach (var dtoIRSC in modelIngRSC.ListaIngresoRetiroSC)
                    {
                        dtoIRSC.IngrscCodi = 0;  //Se insertara un nuevo registro
                        dtoIRSC.IngrscVersion = iVersionNew; //Se agrega la nueva versión
                        // ACTUALIZA LOS CÓDIGOS DE EMPRESA QUE TIENEN TTIE POR LOS NUEVOS
                        var empresaTTIE = EmpresasTTIE.Where(x => x.Emprcodiorigen == dtoIRSC.EmprCodi || x.Emprcodidestino == dtoIRSC.EmprCodi).ToList();
                        if (empresaTTIE.Count > 0) dtoIRSC.EmprCodi = empresaTTIE.First().Emprcodidestino;
                        // FIN
                    }
                    var duplicadosRet = modelIngRSC.ListaIngresoRetiroSC.GroupBy(x => x.EmprCodi).Where(emp => emp.Count() > 1).Select(x => x.Key).ToList();
                    if (duplicadosRet.Any())
                    {
                        foreach (var codigoEmpresaDuplicado in duplicadosRet)
                        {
                            decimal sum = modelIngRSC.ListaIngresoRetiroSC.Where(x => x.EmprCodi == codigoEmpresaDuplicado).Sum(x => x.IngrscImporte);
                            var dtoIRSCd = modelIngRSC.ListaIngresoRetiroSC.Where(x => x.EmprCodi == codigoEmpresaDuplicado).FirstOrDefault();
                            dtoIRSCd.IngrscImporte = sum;
                            dtoIRSCd.IngrscCodi = (new IngresoRetiroSCAppServicio()).SaveIngresoRetiroSC(dtoIRSCd);
                            modelIngRSC.ListaIngresoRetiroSC.RemoveAll(x => x.EmprCodi == codigoEmpresaDuplicado);
                        }
                    }
                    foreach (var dtoIRSC in modelIngRSC.ListaIngresoRetiroSC)
                    {
                        dtoIRSC.IngrscCodi = (new IngresoRetiroSCAppServicio()).SaveIngresoRetiroSC(dtoIRSC);
                    }
                }
                else
                {   //Es una opción de editar
                    //Para el caso en que se cambie el PeriCodiDestino
                    //1.-Validamos si existen recalculos ya asignados a un periodo destino en la tabla TRN_SALDO_RECALCULO
                    int iPeriCodiDestino = this.servicioValorTransferencia.GetByPericodiDestino(iPericodi, modelo.Entidad.RecaCodi);
                    if (iPeriCodiDestino != modelo.Entidad.PeriCodiDestino)
                    {   //2.-Colocamos el nuevo pericodidestino:  
                        iPeriCodiDestino = this.servicioValorTransferencia.UpdatePericodiDestino(iPericodi, modelo.Entidad.RecaCodi, modelo.Entidad.PeriCodiDestino);
                    }
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = this.archivoFuncion.ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = this.servicioPeriodo.ListarPeriodosFuturos(modelo.Entidad.RecaPeriCodi);
            TempData["PeriodoDestino"] = new SelectList(modelPeriodo.ListaPeriodos, "PeriCodi", "PeriNombre");
            return PartialView(modelo);
        }

        public ActionResult Edit(int id)
        {
            int iPeriCodi = Convert.ToInt32(Session["sPericodi"].ToString());
            RecalculoModel modelo = new RecalculoModel();
            modelo.Entidad = this.servicioRecalculo.GetByIdRecalculo(iPeriCodi, id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Recafechavalorizacion = modelo.Entidad.RecaFechaValorizacion.ToString("dd/MM/yyyy");
            modelo.Recafechalimite = modelo.Entidad.RecaFechaLimite.ToString("dd/MM/yyyy");
            modelo.Recafechaobservacion = modelo.Entidad.RecaFechaObservacion.ToString("dd/MM/yyyy");
            modelo.bGrabar = this.archivoFuncion.ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            modelo.pericodidestino = modelo.Entidad.PeriCodiDestino;
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            PeriodoModel modelPeriodo = new PeriodoModel();
            modelPeriodo.ListaPeriodos = this.servicioPeriodo.ListarPeriodosFuturos(modelo.Entidad.RecaPeriCodi);
            TempData["PeriodoDestino"] = new SelectList(modelPeriodo.ListaPeriodos, "PeriCodi", "PeriNombre");
            return PartialView(modelo);
        }

        public ActionResult CopiarRetiros(int PeriCodi, int VersionNew, int VersionOld, int GenEmprCodi)
        {
            int iEmprCodi = -1; //Para salir del bucle

            //Duplicamos la información de la tabla TRN_TRANS_RETIRO(TRETCODI, PERICODI, TRETVERSION) a la nueva versión por empresa
            TransferenciaRetiroModel modelTraRet = new TransferenciaRetiroModel();
            modelTraRet.ListaTransferenciaRetiro = (new TransferenciaEntregaRetiroAppServicio()).ListByPeriodoVersionREmpresa(PeriCodi, VersionOld, GenEmprCodi); //Para la empresa que sigue > GenEmprCodi
            foreach (var dtoTR in modelTraRet.ListaTransferenciaRetiro)
            {
                if (iEmprCodi == -1)
                {
                    iEmprCodi = dtoTR.EmprCodi;
                    //Eliminamos información de la tabla TRN_TRANS_RETIRO_DETALLE ASOCIADA a la empresa
                    int iIdDetalle = (new TransferenciaEntregaRetiroAppServicio()).DeleteListaTransferenciaRetiroDetalleEmpresa(PeriCodi, VersionNew, iEmprCodi);
                    //Eliminamos información de la tabla TRN_TRANS_RETIRO ASOCIADA a la empresa
                    int iId = (new TransferenciaEntregaRetiroAppServicio()).DeleteListaTransferenciaRetiroEmpresa(PeriCodi, VersionNew, iEmprCodi);
                }
                if (iEmprCodi == dtoTR.EmprCodi)
                {
                    //Insertamos el nuevo registro
                    int TranRetiCodiOld = dtoTR.TranRetiCodi;  //Código antiguo
                    dtoTR.TranRetiCodi = 0;  //Se insertara un nuevo registro
                    dtoTR.TranRetiVersion = VersionNew; //Se insertara una nueva versión
                    int iTRetCodiNew = (new TransferenciaEntregaRetiroAppServicio()).SaveOrUpdateTransferenciaRetiro(dtoTR);//Devuelve el numevo TRetCodi
                    //Duplicamos la información de la tabla TRN_TRANS_RETIRO_DETALLE(TRETCODI, TRETDEVERSION) a la nueva versión
                    TransferenciaRetiroDetalleModel modelTraRetDet = new TransferenciaRetiroDetalleModel();
                    modelTraRetDet.ListaTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(TranRetiCodiOld, VersionNew);
                    foreach (var dtoTRD in modelTraRetDet.ListaTransferenciaRetiroDetalle)
                    {
                        dtoTRD.TranRetiDetaCodi = 0;
                        dtoTRD.TranRetiDetaVersion = VersionNew; //Se agrega la nueva versión
                        dtoTRD.TranRetiCodi = iTRetCodiNew; //Se agrega el nuevo código de la Transferencia de  Retiro
                        modelTraRetDet.idTransferenciaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).SaveOrUpdateTransferenciaRetiroDetalle(dtoTRD);
                    }
                }
                else
                    break;
            }
            if (iEmprCodi == -1)
            {
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                Session["sPericodi"] = PeriCodi;
                return RedirectToAction("Index");
            }
            return RedirectToAction("CopiarRetiros", new { PeriCodi = PeriCodi, VersionNew = VersionNew, VersionOld = VersionOld, GenEmprCodi = iEmprCodi });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    int iIdBorrar;
                    int iPeriCodi = Convert.ToInt32(Session["sPericodi"].ToString());
                    string valorizacion = "";

                    //Eliminamos los registros del calculo de la valorizacion solo para revisiones mayores a 1
                    if (id > 2)
                    {
                        valorizacion = (new ValorTransferenciaAppServicio()).DeleteValorizacion(iPeriCodi, id, User.Identity.Name);
                    }

                    //Eliminamos información de la tabla TRN_COSTO_MARGINAL ASOCIADA
                    iIdBorrar = (new CostoMarginalAppServicio()).DeleteListaCostoMarginal(iPeriCodi, id);
                    if (iIdBorrar > 0)
                    {
                        //Eliminamos información de la tabla TRN_COSTO_MARGINAL_AJUSTE ASOCIADA
                        iIdBorrar = (new FactorPerdidaAppServicio()).DeleteListaAjusteCostoMarginal(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_FACTOR_PERDIDA ASOCIADA
                        iIdBorrar = (new FactorPerdidaAppServicio()).DeleteListaFactorPerdida(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_ING_COMPENSACION ASOCIADA
                        iIdBorrar = (new IngresoCompensacionAppServicio()).DeleteListaIngresoCompensacion(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_ING_POTENCIA ASOCIADA
                        iIdBorrar = (new IngresoPotenciaAppServicio()).DeleteListaIngresoPotencia(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_ING_RETIROSC ASOCIADA
                        iIdBorrar = (new IngresoRetiroSCAppServicio()).DeleteListaIngresoRetiroSC(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_TRANS_ENTREGA_DETALLE ASOCIADA
                        iIdBorrar = (new TransferenciaEntregaRetiroAppServicio()).DeleteListaTransferenciaEntregaDetalle(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_TRANS_ENTREGA ASOCIADA
                        iIdBorrar = (new TransferenciaEntregaRetiroAppServicio()).DeleteListaTransferenciaEntrega(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_TRANS_RETIRO_DETALLE ASOCIADA
                        iIdBorrar = (new TransferenciaEntregaRetiroAppServicio()).DeleteListaTransferenciaRetiroDetalle(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_TRANS_RETIRO ASOCIADA
                        iIdBorrar = (new TransferenciaEntregaRetiroAppServicio()).DeleteListaTransferenciaRetiro(iPeriCodi, id);
                        //Eliminamos información de la tabla TRN_TRAMITE ASOCIADA
                        iIdBorrar = (new TramiteAppServicio()).DeleteListaTramite(iPeriCodi, id);
                        //Eliminamos información de la tabla RCG_REPARTO_RND
                        iIdBorrar = this.servicioRecalculo.DeleteRepartoRnd(iPeriCodi, id);
                        //Eliminamos información de la tabla RCG_COSTOMARGINAL_CAB
                        var listRcgCostoMarginalCab = (new TransferenciaInformacionBaseAppServicio()).ListRcgCostoMarginalCab(iPeriCodi, id).ToList();
                        if (listRcgCostoMarginalCab.Count > 0)
                        {
                            iIdBorrar = (new TransferenciaInformacionBaseAppServicio()).DeleteDatosRcgCostoMarginalDet(listRcgCostoMarginalCab[0].Rccmgccodi);
                            //Eliminamos información de la tabla RCG_COSTOMARGINAL_DET
                            iIdBorrar = (new TransferenciaInformacionBaseAppServicio()).DeleteDatosRcgCostoMarginalCab(iPeriCodi, id);
                        }
                        //Eliminamos información de la tabla rcg_renta_periodo
                        iIdBorrar = (new TransferenciaInformacionBaseAppServicio()).DeleteDatosPeriodo(iPeriCodi, id);

                        RecalculoModel model = new RecalculoModel();
                        model.IdRecalculo = this.servicioRecalculo.DeleteRecalculo(iPeriCodi, id);

                        //Actualizamos el encabezado del periodo
                        int ultVersion = (new RecalculoAppServicio()).GetUltimaVersion(iPeriCodi);
                        if(ultVersion > 0)
                        {
                            //Actualizamos la información de consulta del Periodo
                            RecalculoDTO recalculoUlt = new RecalculoDTO();
                            recalculoUlt = (new RecalculoAppServicio()).GetByIdRecalculo(iPeriCodi, ultVersion);
                            PeriodoModel modeloperiodo = new PeriodoModel();
                            modeloperiodo.Entidad = this.servicioPeriodo.GetByIdPeriodo(iPeriCodi);
                            modeloperiodo.Entidad.PeriFechaValorizacion = recalculoUlt.RecaFechaValorizacion;
                            modeloperiodo.Entidad.PeriFechaLimite = recalculoUlt.RecaFechaLimite;
                            modeloperiodo.Entidad.PeriHoraLimite = recalculoUlt.RecaHoraLimite;
                            modeloperiodo.Entidad.PeriFechaObservacion = recalculoUlt.RecaFechaObservacion;
                            modeloperiodo.Entidad.PeriEstado = recalculoUlt.RecaEstado;
                            modeloperiodo.Entidad.RecaNombre = recalculoUlt.RecaNombre;
                            modeloperiodo.IdPeriodo = this.servicioPeriodo.SaveOrUpdatePeriodo(modeloperiodo.Entidad);
                        }

                        return "true";
                    }
                }
            }catch(Exception ex)
            {
                return ex.Message;
            }
            
            return "false";
        }

    }
}
