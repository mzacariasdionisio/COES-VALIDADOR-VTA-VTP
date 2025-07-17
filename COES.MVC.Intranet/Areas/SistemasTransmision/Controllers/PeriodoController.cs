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
    public class PeriodoController : BaseController
    {
        //
        // GET: /SistemasTransmision/Periodo/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        SistemasTransmisionAppServicio servicioSistemasTransmision = new SistemasTransmisionAppServicio();
        
        public ActionResult Index()
        {
            PeriodoModel model = new PeriodoModel();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName); 
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Lista(string nombre)
        {
            PeriodoModel model = new PeriodoModel();
            model.ListaPeriodos = this.servicioSistemasTransmision.GetByCriteriaStPeriodos(nombre);
            foreach (var item in model.ListaPeriodos)
            {
                item.NombreMes = Tools.ObtenerNombreMes(item.Stpermes);
            }
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }

        public ActionResult View(int id = 0)
        {
            PeriodoModel model = new PeriodoModel();
            model.Entidad = this.servicioSistemasTransmision.GetByIdStPeriodo(id);

            return PartialView(model);
        }

        public ActionResult New() 
        {
            PeriodoModel modelo = new PeriodoModel();
            modelo.Entidad = new StPeriodoDTO();
            if (modelo.Entidad == null)
            {
                return HttpNotFound();
            }
            modelo.Entidad.Stpercodi = 0;
            modelo.Entidad.Stperanio = DateTime.Now.Year;
            modelo.Entidad.Stpermes = DateTime.Now.Month;
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            TempData["Mescodigo"] = new SelectList(ObtenerMes(), "Value", "Text", modelo.Entidad.Stpermes);
            TempData["Aniocodigo"] = new SelectList(ObtenerAnio(), "Value", "Text", modelo.Entidad.Stperanio);

            return PartialView(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PeriodoModel modelo) 
        {
            if (ModelState.IsValid)
            {
                if (modelo.Entidad.Stperaniomes == 0)
                    modelo.Entidad.Stperaniomes = modelo.Entidad.Stperanio * 100 + modelo.Entidad.Stpermes;
                
                modelo.Entidad.Stperusucreacion = User.Identity.Name;
                modelo.Entidad.Stperusumodificacion = User.Identity.Name;
                modelo.Entidad.Stperfeccreacion = DateTime.Now;
                modelo.Entidad.Stperfecmodificacion = DateTime.Now;

                if (modelo.Entidad.Stpercodi == 0)
                {
                    this.servicioSistemasTransmision.SaveStPeriodo(modelo.Entidad);
                }
                else
                {
                    this.servicioSistemasTransmision.UpdateStPeriodo(modelo.Entidad);
                }
                
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return RedirectToAction("Index");
            }

            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            TempData["Mescodigo"] = new SelectList(ObtenerMes(), "Value", "Text", modelo.Entidad.Stpermes);
            TempData["Aniocodigo"] = new SelectList(ObtenerAnio(), "Value", "Text", modelo.Entidad.Stperanio);
            return PartialView(modelo); 
        }

        public ActionResult Edit(int id)
        {
            PeriodoModel model = new PeriodoModel();
            model.Entidad = this.servicioSistemasTransmision.GetByIdStPeriodo(id);
            if(model.Entidad == null)
            {
                return HttpNotFound();
            }
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            TempData["Mescodigo"] = new SelectList(ObtenerMes(), "Value", "Text", model.Entidad.Stpermes);
            TempData["Aniocodigo"] = new SelectList(ObtenerAnio(), "Value", "Text", model.Entidad.Stperanio);

            return PartialView(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int id) 
        {
            if (id > 0) 
            {
                PeriodoModel model = new PeriodoModel();
                this.servicioSistemasTransmision.DeleteStPeriodo(id);
                return "true";
            }
            return "False";
        }

        /// <summary>
        /// Retorna una lista de meses del año
        /// </summary>
        public static IEnumerable<SelectListItem> ObtenerMes()
        {
            var ListaMes = new List<SelectListItem>();
            var list1 = new SelectListItem();
            list1.Text = "Enero"; list1.Value = "1"; ListaMes.Add(list1);
            var list2 = new SelectListItem();
            list2.Text = "Febrero"; list2.Value = "2"; ListaMes.Add(list2);
            var list3 = new SelectListItem();
            list3.Text = "Marzo"; list3.Value = "3"; ListaMes.Add(list3);
            var list4 = new SelectListItem();
            list4.Text = "Abril"; list4.Value = "4"; ListaMes.Add(list4);
            var list5 = new SelectListItem();
            list5.Text = "Mayo"; list5.Value = "5"; ListaMes.Add(list5);
            var list6 = new SelectListItem();
            list6.Text = "Junio"; list6.Value = "6"; ListaMes.Add(list6);
            var list7 = new SelectListItem();
            list7.Text = "Julio"; list7.Value = "7"; ListaMes.Add(list7);
            var list8 = new SelectListItem();
            list8.Text = "Agosto"; list8.Value = "8"; ListaMes.Add(list8);
            var list9 = new SelectListItem();
            list9.Text = "Setiembre"; list9.Value = "9"; ListaMes.Add(list9);
            var list10 = new SelectListItem();
            list10.Text = "Octubre"; list10.Value = "10"; ListaMes.Add(list10);
            var list11 = new SelectListItem();
            list11.Text = "Noviembre"; list11.Value = "11"; ListaMes.Add(list11);
            var list12 = new SelectListItem();
            list12.Text = "Diciembre"; list12.Value = "12"; ListaMes.Add(list12);
            return ListaMes;
        }

        /// <summary>
        /// Retorna una lista de años tomando como inicio 2014 y finalizando 6 años mas al año actual
        /// </summary>
        public static IEnumerable<SelectListItem> ObtenerAnio()
        {
            var ListaAnio = new List<SelectListItem>();
            int iAnioFinal = DateTime.Today.Year + 6;
            for (int i = 2014; i <= iAnioFinal; i++)
            {
                var list = new SelectListItem();
                list.Text = list.Value = i.ToString();
                ListaAnio.Add(list);
            }
            return ListaAnio;
        }
    }
}