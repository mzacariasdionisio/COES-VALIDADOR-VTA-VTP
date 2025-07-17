using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.SistemasTransmision.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision.Controllers
{
    public class RecalculoController : BaseController
    {
        //
        // GET: /SistemasTransmision/Recalculo/
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();

        public ActionResult Index(int id = 0)
        {
            PeriodoModel modelPeriodo = new PeriodoModel();
            Session["sPericodi"] = id;
            modelPeriodo.Entidad = this.servicioSistemasTransmision.GetByIdStPeriodo(id);
            modelPeriodo.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            TempData["Perinombre"] = modelPeriodo.Entidad.Stpernombre;
            Session["Perinombre"] = modelPeriodo.Entidad.Stpernombre;
            return PartialView(modelPeriodo);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            int id = Convert.ToInt32(Session["sPericodi"].ToString());
            RecalculoModel model = new RecalculoModel();
            model.ListaRecalculo = (new SistemasTransmisionAppServicio().ListStRecalculos(id));
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return PartialView(model);
        }

        public ActionResult New(int id)
        {
            RecalculoModel model = new RecalculoModel();
            model.Entidad = new StRecalculoDTO();
            if (model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.IdPeriodo = id;
            model.Entidad.Stpercodi = model.IdPeriodo;
            model.Entidad.Strecacodi = 0;
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RecalculoModel modelo)
        {
            bool bDuplicarData = false;
            int iVersionOld = 0;
            if (ModelState.IsValid)
            {
                //Información del Recalculo
                modelo.Entidad.Strecausumodificacion = User.Identity.Name;
                modelo.Entidad.Strecafecmodificacion = DateTime.Now;
                if (modelo.Entidad.Strecacodi == 0)
                {
                    //Acción Nuevo: Hay que realizar una nueva versión de la información en las tablas:
                    bDuplicarData = true;
                    modelo.Entidad.Strecausucreacion = User.Identity.Name;
                    modelo.Entidad.Strecafeccreacion = DateTime.Now;
                    RecalculoModel modelAux = new RecalculoModel();
                    modelAux.ListaRecalculo = this.servicioSistemasTransmision.ListStRecalculos(modelo.Entidad.Stpercodi);
                    modelo.Entidad.Sstversion = modelAux.ListaRecalculo.Count + 1;
                    if (modelAux.ListaRecalculo.Count > 0)
                    {
                        iVersionOld = modelAux.ListaRecalculo[0].Strecacodi; //almacenamos la versión anterior si esta dentro del mismo periodo
                    }
                    //Grabando la versión de recalculo
                    modelo.IdRecalculo = this.servicioSistemasTransmision.SaveStRecalculo(modelo.Entidad);
                }
                else
                {
                    //Acción Edit
                    this.servicioSistemasTransmision.UpdateStRecalculo(modelo.Entidad);
                }

                if (bDuplicarData)
                {
                    //Replicamos la data de la versión anterior, sólo se da en la opción nueva versión
                    int iPeriCodi = modelo.Entidad.Stpercodi;
                    int iVersionNew = modelo.IdRecalculo;

                    if (iVersionOld == 0)
                    {
                        //Traemos el recalculo anterior del periodo anterior
                        StPeriodoDTO dtoPeriodoAnterior = this.servicioSistemasTransmision.BuscarPeriodoAnterior(iPeriCodi);
                        if (dtoPeriodoAnterior != null)
                        {
                            RecalculoModel modelAux = new RecalculoModel();
                            modelAux.ListaRecalculo = this.servicioSistemasTransmision.ListStRecalculos(dtoPeriodoAnterior.Stpercodi);
                            if (modelAux.ListaRecalculo.Count > 0)
                            {
                                iVersionOld = modelAux.ListaRecalculo[0].Strecacodi; //almacenamos la versión anterior del periodo anterior
                            }

                        }
                    }

                    if (iVersionOld > 0)
                    {
                        //Duplicamos la información de la tabla st_Barra a la nueva versión
                        BarraModel modelBarra = new BarraModel();
                        modelBarra.ListaSTBarra = this.servicioSistemasTransmision.ListByStBarraVersion(iVersionOld);
                        foreach (var dtoBarra in modelBarra.ListaSTBarra)
                        {
                            int StbarrcodiOld = dtoBarra.Stbarrcodi; //Código antiguo
                            dtoBarra.Stbarrcodi = 0;
                            dtoBarra.Strecacodi = iVersionNew;
                            dtoBarra.Stbarrusucreacion = User.Identity.Name;
                            dtoBarra.Stbarrfeccreacion = DateTime.Now;
                            this.servicioSistemasTransmision.SaveStBarra(dtoBarra);
                        }

                        //Duplicamos la información de la tabla st_Transmisor a la nueva versión
                        TransmisorModel modelTransmisor = new TransmisorModel();
                        modelTransmisor.ListaEmpresaTransmisor = this.servicioSistemasTransmision.ListByStTransmisorVersion(iVersionOld);
                        foreach (var dtoTransmisor in modelTransmisor.ListaEmpresaTransmisor)
                        {
                            int StbarrcodiOld = dtoTransmisor.Stranscodi; //Código antiguo
                            dtoTransmisor.Stranscodi = 0;
                            dtoTransmisor.Strecacodi = iVersionNew;
                            dtoTransmisor.Stransusucreacion = User.Identity.Name;
                            dtoTransmisor.Stransfeccreacion = DateTime.Now;
                            this.servicioSistemasTransmision.SaveStTransmisor(dtoTransmisor);
                        }

                        //Duplicamos la información de la tabla ST_GENERADOR a la nueva versión
                        GeneradorModel modelGenerador = new GeneradorModel();
                        modelGenerador.ListaEmpresaGeneradora = this.servicioSistemasTransmision.ListByStGeneradorVersion(iVersionOld);
                        foreach (var dtoGen in modelGenerador.ListaEmpresaGeneradora)
                        {
                            int StgenrCodiOld = dtoGen.Stgenrcodi; //Código antiguo
                            dtoGen.Stgenrcodi = 0;
                            dtoGen.Strecacodi = iVersionNew;
                            int iStgenrCodiNew = this.servicioSistemasTransmision.SaveStGenerador(dtoGen);
                            //Duplicamos la información de la tabla ST_CENTRALGEN a la nueva versión
                            CentralGeneracionModel modelCentralGen = new CentralGeneracionModel();
                            modelCentralGen.ListaSTCentralGen = this.servicioSistemasTransmision.ListStCentralgens(StgenrCodiOld);
                            foreach (var dtoCentralGen in modelCentralGen.ListaSTCentralGen)
                            {
                                dtoCentralGen.Stcntgcodi = 0;
                                dtoCentralGen.Strecacodi = iVersionNew;
                                dtoCentralGen.Stgenrcodi = iStgenrCodiNew;
                                dtoCentralGen.Stcntgusucreacion = User.Identity.Name;
                                dtoCentralGen.Stcntgusumodificacion = User.Identity.Name;
                                dtoCentralGen.Stcntgfeccreacion = DateTime.Now;
                                dtoCentralGen.Stcntgfecmodificacion = DateTime.Now;
                                modelCentralGen.IdRecalculo = this.servicioSistemasTransmision.SaveStCentralgen(dtoCentralGen);
                            }
                        }

                        //Duplicamos la información de la tabla ST_SISTEMATRANS a la nueva versión
                        ElementoStModel modelStElemento = new ElementoStModel();
                        modelStElemento.ListaSistema = this.servicioSistemasTransmision.ListByStSistemaTransVersion(iVersionOld);
                        foreach (var dtoSisTrans in modelStElemento.ListaSistema)
                        {
                            int StSisTrnCodiOld = dtoSisTrans.Sistrncodi; //Código antiguo
                            dtoSisTrans.Sistrncodi = 0;
                            dtoSisTrans.Strecacodi = iVersionNew;
                            dtoSisTrans.Sistrnsucreacion = User.Identity.Name;
                            dtoSisTrans.Sistrnsumodificacion = User.Identity.Name;
                            dtoSisTrans.Sistrnfeccreacion = DateTime.Now;
                            dtoSisTrans.Sistrnfecmodificacion = DateTime.Now;
                            int iStSisTrnCodiNew = this.servicioSistemasTransmision.SaveStSistematrans(dtoSisTrans);
                            //Duplicamos la información de la tabla ST_COMPENSACION a la nueva versión
                            CompensacionModel modelCompensacion = new CompensacionModel();
                            modelCompensacion.ListaCompensacion = this.servicioSistemasTransmision.ListStCompensacionsPorID(StSisTrnCodiOld);
                            foreach (var dtoCompensacion in modelCompensacion.ListaCompensacion)
                            {
                                dtoCompensacion.Stcompcodi = 0;
                                dtoCompensacion.Sistrncodi = iStSisTrnCodiNew;
                                dtoCompensacion.Sstcmpusucreacion = User.Identity.Name;
                                dtoCompensacion.Sstcmpusumodificacion = User.Identity.Name;
                                dtoCompensacion.Sstcmpfeccreacion = DateTime.Now;
                                dtoCompensacion.Sstcmpfecmodificacion = DateTime.Now;
                                modelCompensacion.IdRecalculo = this.servicioSistemasTransmision.SaveStCompensacion(dtoCompensacion);
                            }
                            //Insertamos el Factor de Pago correspondiente de la tabla ST_FACTOR a la nueva versión y sistema
                            StFactorDTO dtoFactor = this.servicioSistemasTransmision.GetByCriteriaStFactorsSistema(StSisTrnCodiOld);
                            int StFactCodiOld = dtoFactor.Stfactcodi; //Código antiguo
                            dtoFactor.Stfactcodi = 0;
                            dtoFactor.Sistrncodi = iStSisTrnCodiNew;
                            dtoFactor.Strecacodi = iVersionNew;
                            dtoFactor.Stfactusucreacion = User.Identity.Name;
                            dtoFactor.Stfactfeccreacion = DateTime.Now;
                            this.servicioSistemasTransmision.SaveStFactor(dtoFactor);
                        }
                    }
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index", new { id = modelo.Entidad.Stpercodi });
            }
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();
            return PartialView(modelo);
        }

        public ActionResult Edit(int id)
        {
            RecalculoModel model = new RecalculoModel();
            model.Entidad = this.servicioSistemasTransmision.GetByIdStRecalculo(id);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            TempData["Periodonombre"] = Session["Perinombre"].ToString();

            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        public string Delete(int id)
        {
            if (id > 0)
            {
                RecalculoModel model = new RecalculoModel();

                //Se elimina todas las tablas relacionadas a la version que se esta eliminando
                //Se elimina el detalle (st_compmensualele) y luego la tabla maestra (st_compmensual) en el orden respectivo
                this.servicioSistemasTransmision.DeleteStCompmensualEleVersion(id);
                this.servicioSistemasTransmision.DeleteStCompmensual(id);
                //Se elimina el detalle (st_respagoele) y luego la tabla maestra (st_respago) en el orden respectivo
                this.servicioSistemasTransmision.DeleteStRespagoEleVersion(id);
                this.servicioSistemasTransmision.DeleteStRespago(id);
                //tabla st_factor
                this.servicioSistemasTransmision.DeleteStFactorVersion(id);
                //tabla st_energia
                this.servicioSistemasTransmision.DeleteStEnergia(id);
                //Se elimina el detalle (st_dstele_barra) y luego la tabla maestra (st_distelectrica) en el orden respectivo
                this.servicioSistemasTransmision.DeleteStDsteleBarra(id);
                this.servicioSistemasTransmision.DeleteStDistelectrica(id);
                //Se elimina las tablas que almacenan los datos del calculo de la version a eliminar
                this.servicioSistemasTransmision.DeleteStDistelectricaGenele(id);
                this.servicioSistemasTransmision.DeleteStElementoCompensado(id);
                this.servicioSistemasTransmision.DeleteStFactorpago(id);
                //Se elimina el detalle (st_compensacion) y luego la tabla maestra (st_sistematrans) en el orden respectivo
                this.servicioSistemasTransmision.DeleteStCompensacionVersion(id);
                this.servicioSistemasTransmision.DeleteStSistematransVersion(id);
                //Se elimina el detalle (st_centralgen) y luego la tabla maestra (st_generador) en el orden respectivo
                this.servicioSistemasTransmision.DeleteStCentralgenVersion(id);
                this.servicioSistemasTransmision.DeleteStGeneradorVersion(id);
                //tabla st_recabarra
                this.servicioSistemasTransmision.DeleteStBarraVersion(id);
                //tabla st_transmisor
                this.servicioSistemasTransmision.DeleteStTransmisorVersion(id);

                //elimina version
                this.servicioSistemasTransmision.DeleteStRecalculo(id);

                return "true";
            }
            return "false";
        }
    }
}
