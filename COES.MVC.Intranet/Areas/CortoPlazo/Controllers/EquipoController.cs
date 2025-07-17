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
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.General;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class EquipoController : BaseController
    {
        EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();
        GeneralAppServicio appGeneral = new GeneralAppServicio();

        public ActionResult Index()
        {

            BusquedaEqEquipoModel model = new BusquedaEqEquipoModel();
            model.ListaSiEmpresa = appGeneral.ObtenerEmpresasGeneracion();
            model.ListaEqArea = servEquipo.ListEqAreas().OrderBy(x => x.Areanomb).ToList();
            model.ListaEqFamilia = servEquipo.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            
            model.FechaIni = DateTime.Now.ToString(Constantes.FormatoFecha);            
            model.AccionNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);

            return View(model);
        }

        public ActionResult Editar(int equipo, int propcodi, string fecha, int accion)
        {

            ValorPropiedadModel model = new ValorPropiedadModel();

            model.Propcodi = propcodi;
            model.Equicodi = equipo;
            if (fecha != "")
            {
                fecha = fecha.Substring(0, 2) + "/" + fecha.Substring(2, 2) + "/" + fecha.Substring(4, 4);
                model.Fecha = fecha;
            }
            else
            {
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            }

            DateTime fechaPropEqui = (DateTime)DateTime.ParseExact(model.Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);


            EqPropequiDTO eqPropEqui = null;

            if (fecha != "")
                eqPropEqui = servEquipo.GetByIdEqPropequi(propcodi, equipo, fechaPropEqui);

            if (eqPropEqui != null)
            {
                model.Valor = eqPropEqui.Valor;
            }
            else
            {                
                model.Valor = "";
            }

            model.Accion = accion;


            return View(model);
            
        }


        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servEquipo.DeleteEqEquipo(id, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        [HttpPost]
        public JsonResult Grabar(int propcodi, int equicodi,string fecha,string valor)
        {
            try
            {
                EqPropequiDTO entity = new EqPropequiDTO();

                entity.Propcodi = propcodi;
                entity.Equicodi = equicodi;
                entity.Fechapropequi = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Valor = valor;
                entity.Propequiusucreacion = base.UserName;

                EqPropequiDTO eqPropEqui = servEquipo.GetByIdEqPropequi(propcodi, equicodi, (DateTime)entity.Fechapropequi);

                if (eqPropEqui != null)
                {
                    servEquipo.UpdateEqPropequi(entity);
                }
                else
                {
                    this.servEquipo.SaveEqPropequi(entity);
                }

                return Json(1);

            }
            catch
            {
                return Json(-1);
            }
        }


        [HttpPost]
        public PartialViewResult Lista(int emprCodi,int areacodi,int famCodi,string lastdate,string equiFechiniopcom, int nroPage)
        {
            BusquedaEqEquipoModel model = new BusquedaEqEquipoModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (lastdate != null)
            {
                fechaInicio = DateTime.ParseExact(lastdate, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            model.ListaEqEquipo = servEquipo.ListarEquiposPropiedades(ConstantesCortoPlazo.PropcodiCapacidadRegulacion, fechaInicio, emprCodi, areacodi,famCodi, nroPage, Constantes.PageSizeEvento).ToList();

            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        [HttpPost]
        public PartialViewResult Paginado(int emprCodi, int areaCodi,int famCodi)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = servEquipo.TotalEquiposPropiedades(emprCodi, areaCodi, famCodi);

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
