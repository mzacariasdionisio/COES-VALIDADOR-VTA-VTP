using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.Servicios.Aplicacion.Transferencias;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Transferencias.Helper;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class PeriodoController : Controller
    {
        // GET: /Transferencias/Periodo/
        //[CustomAuthorize]
        public ActionResult Index()
        {
            PeriodoModel model = new PeriodoModel();
            model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Lista(string nombre)
        {
            PeriodoModel model = new PeriodoModel();
            model.ListaPeriodos = (new PeriodoAppServicio()).BuscarPeriodo(nombre);
            model.bEditar = (new Funcion()).ValidarPermisoEditar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bEliminar = (new Funcion()).ValidarPermisoEliminar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            model.bNuevo = true;
            model.bEditar = true;
            model.bGrabar = true;
            return PartialView(model);
        }
        //
        // GET: /Transferencias/Periodo/Details/5

        public ActionResult View(int id = 0)
        {
            PeriodoModel model = new PeriodoModel();
            model.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(id);

            return PartialView(model);
        }

        //
        // GET: /Transferencias/Periodo/Create
        public ActionResult New()
        {
            PeriodoModel modelo = new PeriodoModel();
            modelo.Entidad = new PeriodoDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.PeriCodi = 0;
            modelo.Entidad.RecaNombre = "Mensual";
            modelo.Entidad.PeriHoraLimite = "20:00";
            modelo.Perifechavalorizacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Perifechalimite = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Perifechaobservacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            modelo.Entidad.AnioCodi = DateTime.Today.Year;
            modelo.Entidad.MesCodi = DateTime.Today.Month;
            modelo.Entidad.PeriEstado = "Abierto";
            modelo.Entidad.PeriFormNuevo = 1;
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Mescodigo"] = new SelectList((new Funcion()).ObtenerMes(), "Value", "Text", modelo.Entidad.MesCodi);
            TempData["Aniocodigo"] = new SelectList((new Funcion()).ObtenerAnio(), "Value", "Text", modelo.Entidad.AnioCodi);
            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PeriodoModel modelo)
        {
            bool bRecalculo = false;
            if (ModelState.IsValid)
            {
                if (modelo.Entidad.PeriCodi == 0)
                {   //Nuevo periodo
                    bRecalculo = true;
                    modelo.Entidad.PeriFormNuevo = 1;
                } else
                {
                    modelo.Entidad.PeriFormNuevo = (new PeriodoAppServicio()).GetByIdPeriodo(modelo.Entidad.PeriCodi).PeriFormNuevo;
                }
                if (modelo.Entidad.PeriAnioMes == 0)
                    modelo.Entidad.PeriAnioMes = modelo.Entidad.AnioCodi * 100 + modelo.Entidad.MesCodi;
                if (modelo.Entidad.PeriEstado.Equals(""))
                    modelo.Entidad.PeriEstado = "Abierto";
                if (modelo.Perifechalimite != "" && modelo.Perifechalimite != null)
                    modelo.Entidad.PeriFechaLimite = DateTime.ParseExact(modelo.Perifechalimite, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Perifechavalorizacion != "" && modelo.Perifechavalorizacion != null)
                    modelo.Entidad.PeriFechaValorizacion = DateTime.ParseExact(modelo.Perifechavalorizacion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (modelo.Perifechaobservacion != "" && modelo.Perifechaobservacion != null)
                    modelo.Entidad.PeriFechaObservacion = DateTime.ParseExact(modelo.Perifechaobservacion, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                modelo.Entidad.PeriUserName = User.Identity.Name;

                modelo.IdPeriodo = (new PeriodoAppServicio()).SaveOrUpdatePeriodo(modelo.Entidad);
                if (bRecalculo)
                {   //Insertamos los conceptos de compensación del periodo anterior en caso existiera.
                    int iMesAnterior = modelo.Entidad.MesCodi - 1;
                    int iAnioAnterior = modelo.Entidad.AnioCodi;
                    if (iMesAnterior == 0)
                    {
                        iMesAnterior = 12;
                        iAnioAnterior = modelo.Entidad.AnioCodi - 1;
                    }
                    int Perianiomes = iAnioAnterior * 100 + iMesAnterior;
                    PeriodoDTO PeriAnterior = (new PeriodoAppServicio()).GetByAnioMes(Perianiomes);
                    if (PeriAnterior != null)
                    {   //Traemos la lista de Compensaciones del Periodo anterior
                        CompensacionModel modelCompesacion = new CompensacionModel();
                        modelCompesacion.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(PeriAnterior.PeriCodi);
                        foreach (var item in modelCompesacion.ListaCompensacion)
                        {
                            item.CabeCompCodi = 0;
                            item.CabeCompPeriCodi = modelo.IdPeriodo;
                            item.CabeCompUserName = User.Identity.Name;
                            item.CabeCompCodi = (new CompensacionAppServicio()).SaveOrUpdateCompensacion(item);
                        }
                        //assetec 20200604 - COPIAMOS POTENCIAS CONTRATADAS
                        (new TransferenciasAppServicio()).CopiarPotenciasContratadasByPeriodo(modelo.IdPeriodo, PeriAnterior.PeriCodi, User.Identity.Name);
                    }
                    //Insertamos la version 1 en el recalculo 
                    RecalculoDTO dto = new RecalculoDTO();
                    dto.RecaPeriCodi = modelo.IdPeriodo;
                    dto.RecaFechaValorizacion = modelo.Entidad.PeriFechaValorizacion;
                    dto.RecaFechaLimite = modelo.Entidad.PeriFechaLimite;
                    dto.RecaHoraLimite = modelo.Entidad.PeriHoraLimite;
                    dto.RecaFechaObservacion = modelo.Entidad.PeriFechaObservacion;
                    dto.RecaEstado = modelo.Entidad.PeriEstado;
                    dto.RecaNombre = "Mensual";
                    dto.RecaDescripcion = "Apertura del mes de valorización";
                    dto.RecaNroInforme = "COES/D/DO/STR-INF-";
                    dto.RecaUserName = User.Identity.Name;
                    RecalculoModel modelRecalculo = new RecalculoModel();
                    modelRecalculo.IdRecalculo = (new RecalculoAppServicio()).SaveOrUpdateRecalculo(dto);
                }
                else
                {   //Actualizamos la información del Recalculo
                    RecalculoModel modelorecalculo = new RecalculoModel();
                    modelorecalculo.Entidad = (new RecalculoAppServicio()).GetByIdRecalculo(modelo.IdPeriodo, 1);
                    modelorecalculo.Entidad.RecaFechaValorizacion = modelo.Entidad.PeriFechaValorizacion;
                    modelorecalculo.Entidad.RecaFechaLimite = modelo.Entidad.PeriFechaLimite;
                    modelorecalculo.Entidad.RecaHoraLimite = modelo.Entidad.PeriHoraLimite;
                    modelorecalculo.Entidad.RecaFechaObservacion = modelo.Entidad.PeriFechaObservacion;
                    modelorecalculo.Entidad.RecaEstado = modelo.Entidad.PeriEstado;
                    modelorecalculo.IdRecalculo = (new RecalculoAppServicio()).SaveOrUpdateRecalculo(modelorecalculo.Entidad);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }
            //Error
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            TempData["Mescodigo"] = new SelectList((new Funcion()).ObtenerMes(), "Value", "Text", modelo.Entidad.MesCodi);
            TempData["Aniocodigo"] = new SelectList((new Funcion()).ObtenerAnio(), "Value", "Text", modelo.Entidad.AnioCodi);
            return PartialView(modelo);
        }

        //
        // GET: /Transferencias/Periodo/Edit/id
        public ActionResult Edit(int id)
        {
            PeriodoModel modelo = new PeriodoModel();
            modelo.Entidad = (new PeriodoAppServicio()).GetByIdPeriodo(id);
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Perifechavalorizacion = modelo.Entidad.PeriFechaValorizacion.ToString("dd/MM/yyyy");
            modelo.Perifechalimite = modelo.Entidad.PeriFechaLimite.ToString("dd/MM/yyyy");
            modelo.Perifechaobservacion = modelo.Entidad.PeriFechaObservacion.ToString("dd/MM/yyyy");
            modelo.bGrabar = (new Funcion()).ValidarPermisoGrabar(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);
            modelo.bGrabar = true;
            TempData["Mescodigo"] = new SelectList((new Funcion()).ObtenerMes(), "Value", "Text", modelo.Entidad.MesCodi);
            TempData["Aniocodigo"] = new SelectList((new Funcion()).ObtenerAnio(), "Value", "Text", modelo.Entidad.AnioCodi);
            return PartialView(modelo);
        }

        // POST: /Transferencias/Periodo/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id = 0)
        {
            if (id > 0)
            {   //Preguntamos si existen registros asociados a este periodo
                PeriodoModel model = new PeriodoModel();
                model.IdPeriodo = (new PeriodoAppServicio()).GetNumRegistros(id);

                if (model.IdPeriodo == 0)
                {   //No hay información, proceder a borrar
                    //Eliminando el recalculo 1
                    RecalculoModel modelRecalculo = new RecalculoModel();
                    modelRecalculo.IdRecalculo = (new RecalculoAppServicio()).DeleteRecalculo(id, 1);
                    //Eliminando las compensaciones
                    CompensacionModel modelCompesacion = new CompensacionModel();
                    modelCompesacion.ListaCompensacion = (new CompensacionAppServicio()).ListCompensaciones(id);
                    foreach (var item in modelCompesacion.ListaCompensacion)
                    {
                        item.CabeCompCodi = (new CompensacionAppServicio()).DeleteCompensacion(item.CabeCompCodi);
                    }
                    //assetec 20200604 - ELIMINAMOS POTENCIAS CONTRATADAS
                    (new TransferenciasAppServicio()).DeleteTrnPotenciaContratadaByPeriodo(id);
                    //Eliminamos el perido
                    model.IdPeriodo = (new PeriodoAppServicio()).DeletePeriodo(id);
                    return "true";
                }
                return "false";
            }
            else
                return "false";
        }
    }
}