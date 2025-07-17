using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Eventos.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.OperacionesVarias;
using COES.Framework.Base.Core;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class SubcausaFamiliaController : BaseController
    {

        
        OperacionesVariasAppServicio servOpVarias = new OperacionesVariasAppServicio();
        EquipamientoAppServicio servEquipamiento = new EquipamientoAppServicio();
        

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            BusquedaEveSubcausaFamiliaModel model = new BusquedaEveSubcausaFamiliaModel();
            model.ListaEveSubcausaevento = servOpVarias.ObtenerSubCausaEvento();
            model.ListaEqFamilia = servEquipamiento.ListEqFamilias();
            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }

        public ActionResult Editar(int id, int accion)
        {

            EveSubcausaFamiliaModel model = new EveSubcausaFamiliaModel();
            model.ListaEveSubcausaevento = servOpVarias.ObtenerSubCausaEvento();
            model.ListaEqFamilia = servEquipamiento.ListEqFamilias();
            EveSubcausaFamiliaDTO eveSubcausaFamilia =null;

            if (id != 0)
                eveSubcausaFamilia = servOpVarias.GetByIdEveSubcausaFamilia(id);

            if (eveSubcausaFamilia != null)
            {
                model.EveSubcausaFamilia = eveSubcausaFamilia;
            }
            else
            {
                eveSubcausaFamilia = new EveSubcausaFamiliaDTO();

                eveSubcausaFamilia.Subcausacodi = Convert.ToInt32(214);
                eveSubcausaFamilia.Famcodi = Convert.ToInt32(Constantes.ParametroDefecto);

                model.EveSubcausaFamilia = eveSubcausaFamilia;

            }

            model.Accion = accion;
            

            return View(model);
            
        }


        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servOpVarias.DeleteEveSubcausaFamilia(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite deshabilitar una relación configurada
        /// </summary>
        /// <param name="id">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Desactivar(int id)
        {
            try
            {

                EveSubcausaFamiliaDTO entity = null;

                if (id != 0)
                {
                    entity = servOpVarias.GetByIdEveSubcausaFamilia(id);

                    entity.Scaufausumodificacion = base.UserName;
                    entity.Scaufafecmodificacion = DateTime.Now;

                    entity.Scaufaeliminado = "S";

                    servOpVarias.UpdateEveSubcausaFamilia(entity);
                    return Json(1);
                }
                return Json(-1);
            }
            catch
            {
                return Json(-1);
            }

        }


        [HttpPost]
        public JsonResult Grabar(EveSubcausaFamiliaModel model)
        {
            try
            {

                EveSubcausaFamiliaDTO entity = new EveSubcausaFamiliaDTO();

                entity.Scaufacodi = model.ScaufaCodi;
                entity.Subcausacodi = model.SubcausaCodi;
                entity.Famcodi = model.FamCodi;
                entity.Scaufaeliminado = model.ScaufaEliminado;


                if (entity.Scaufacodi == 0)
                {
                    entity.Scaufausucreacion = base.UserName;
                    entity.Scaufafeccreacion = DateTime.Now;
                }
                else
                {

                    if (model.ScaufaUsuCreacion != null)
                    {

                        entity.Scaufausucreacion = model.ScaufaUsuCreacion;
                    }

                    if (model.ScaufaFecCreacion != null)
                    {
                        entity.Scaufafeccreacion = DateTime.ParseExact(model.ScaufaFecCreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Scaufausumodificacion = base.UserName;
                    entity.Scaufafecmodificacion = DateTime.Now;
                }


                if (entity.Scaufacodi == 0)
                {

                    if (!this.servOpVarias.ExisteRelacion((int)entity.Subcausacodi, entity.Famcodi))
                    {
                        int id = this.servOpVarias.SaveEveSubcausaFamiliaId(entity);
                        return Json(id);
                    }
                    else
                    {
                        return Json(-2);
                    }
                }

                else
                {
                    int id = this.servOpVarias.SaveEveSubcausaFamiliaId(entity);
                    return Json(id);

                }
                
            }
            catch
            {
                return Json(-1);
            }
        }


        [HttpPost]
        public PartialViewResult Lista(string estado,int subcausaCodi,int famCodi, int nroPage)
        {
            BusquedaEveSubcausaFamiliaModel model = new BusquedaEveSubcausaFamiliaModel();

            model.ListaEveSubcausaFamilia = servOpVarias.BuscarOperaciones(estado, subcausaCodi, famCodi, nroPage, Constantes.PageSizeEvento).ToList();
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        public string ListaEveSubcausaFamilias(int subcausacodi)
        {
            return servOpVarias.ListEveSubcausaFamilias(subcausacodi);
        }


        [HttpPost]
        public PartialViewResult Paginado(string estado,int subcausaCodi,int famCodi)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = servOpVarias.ObtenerNroFilas(estado, subcausaCodi, famCodi);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }
    }
}
