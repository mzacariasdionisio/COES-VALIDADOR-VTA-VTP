using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.ViewModels;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class BarraModeladaController : Controller
    {
        //
        // GET: /Despacho/BarraModelada/
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(BarraModeladaController));
        DespachoAppServicio DespachoServicio = new DespachoAppServicio();
        public const int CategoriaBarraModelada = 10;

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("BarraModeladaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("BarraModeladaController", ex);
                throw;
            }
        }

        public BarraModeladaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ListaBarrasModeladas()
        {
            ListaBarrasModeladasViewModel model = new ListaBarrasModeladasViewModel()
            {
                ListaBarras=DespachoServicio.ListadoBarrasModeladasPaginado(1, int.MaxValue)
            };

            return View(model);
        }

        [HttpPost]
        public PartialViewResult NuevaBarra()
        {
            var barrasEquipamiento = DespachoServicio.ListadoBarrasEquipoDisponibles();
            DetalleBarraModelado modelo = new DetalleBarraModelado
            {
                ListabarrasEquipo = barrasEquipamiento,
                BarraModelada = new PrGrupoDTO()
            };
            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult GrabarBarra(string sNombre,string sAbrevitura,string sTension,string equipos)
        {
            try
            {
                EquipamientoAppServicio equipoServicio = new EquipamientoAppServicio();
                PrGrupoDTO nuevaBarra = new PrGrupoDTO();
                nuevaBarra.Gruponomb = sNombre;
                nuevaBarra.Grupoabrev = sAbrevitura;
                nuevaBarra.Grupotension = string.IsNullOrEmpty(sTension) ? (decimal?)null : decimal.Parse(sTension);
                nuevaBarra.Lastuser = User.Identity.Name;
                nuevaBarra.Lastdate = DateTime.Now;
                nuevaBarra.Catecodi = CategoriaBarraModelada;
                nuevaBarra.Barracodi = -1;
                nuevaBarra.Barracodi2 = -1;
                nuevaBarra.Barramw1 = 0;
                nuevaBarra.Barramw2 = 0;
                nuevaBarra.Tipogrupocodi = -1;
                nuevaBarra.Grupoactivo = "S";
                nuevaBarra.GrupoEstado = "A";
                nuevaBarra.Grupopadre = null;
                nuevaBarra.Grupotipoc = null;
                nuevaBarra.Grupousucreacion = nuevaBarra.Lastuser;
                nuevaBarra.Grupofeccreacion = nuevaBarra.Lastdate;

                int codigoNuevaBarra = DespachoServicio.SavePrGrupo(nuevaBarra);

                if (!string.IsNullOrEmpty(equipos))
                {
                    var codigoEquipo= equipos.Split('&');
                    foreach (var codigo in codigoEquipo)
                    {
                        var equipoBarra = equipoServicio.GetByIdEqEquipo(int.Parse(codigo));
                        equipoBarra.Grupocodi = codigoNuevaBarra;
                        equipoBarra.UsuarioUpdate = User.Identity.Name;
                        equipoBarra.FechaUpdate=DateTime.Today;
                        equipoServicio.UpdateEqEquipo(equipoBarra);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("GrabarBarra",e);
                return Json(-1);
            }
            return Json(1);
        }

        [HttpPost]
        public PartialViewResult EditarBarra(int grupoCodi)
        {
            var barraModelada = DespachoServicio.GetByIdPrGrupo(grupoCodi);
            var barrasEquipamientoAsignadas = DespachoServicio.ListadoBarrasEquipoPorBarraModelada(grupoCodi);
            var barrasEquipamientoDisponibles = DespachoServicio.ListadoBarrasEquipoDisponibles();
            DetalleBarraModelado modelo = new DetalleBarraModelado
            {
                BarraModelada = barraModelada,
                ListabarrasEquipo = barrasEquipamientoAsignadas
            };
            modelo.ListabarrasEquipo.AddRange(barrasEquipamientoDisponibles);

            if (barrasEquipamientoAsignadas != null && barrasEquipamientoAsignadas.Count > 0)
            {
                List<int> listaCodEquipos = (from p in barrasEquipamientoAsignadas select p.Equicodi).ToList();
                Session["BarrasEquipo"] = listaCodEquipos;
            }
            else
            {
                Session["BarrasEquipo"] = null;
            }
            return PartialView(modelo);
        }
        [HttpPost]
        public JsonResult ActualizarBarra(int iGrupocodi,string sNombre, string sAbrevitura, string sTension, string equipos)
        {
            try
            {
                EquipamientoAppServicio equipoServicio = new EquipamientoAppServicio();
                var BarraModOriginal = DespachoServicio.GetByIdPrGrupo(iGrupocodi);
                bool bHayOriginales = false;
                bool bHayNuevos = false;

                BarraModOriginal.Gruponomb = sNombre.Trim();
                BarraModOriginal.Grupoabrev = sAbrevitura.Trim();
                BarraModOriginal.Grupotension = string.IsNullOrEmpty(sTension) ? (decimal?)null : decimal.Parse(sTension);
                BarraModOriginal.Grupousumodificacion = User.Identity.Name;
                BarraModOriginal.Grupofecmodificacion = DateTime.Now;
                BarraModOriginal.Lastuser = BarraModOriginal.Grupousumodificacion;
                BarraModOriginal.Lastdate = BarraModOriginal.Grupofecmodificacion;
                BarraModOriginal.Barracodi = -1;
                BarraModOriginal.Barracodi2 = -1;
                BarraModOriginal.Barramw1 = 0;
                BarraModOriginal.Barramw2 = 0;
                BarraModOriginal.Tipogrupocodi = -1;
                DespachoServicio.UpdatePrGrupo(BarraModOriginal);

                #region Comentado
                //List<int> listaCodigosOriginales = (List<int>)Session["BarrasEquipo"];
                //if (listaCodigosOriginales != null && listaCodigosOriginales.Count > 0) bHayOriginales = true;
                //if (!string.IsNullOrEmpty(equipos)) bHayNuevos = true;

                //if (!bHayOriginales && bHayNuevos)
                //{
                //    var codigoEquipo = equipos.Split('&');
                //    foreach (var codigo in codigoEquipo)
                //    {
                //        var equipoBarra = equipoServicio.GetByIdEqEquipo(int.Parse(codigo));
                //        equipoBarra.Grupocodi = iGrupocodi;
                //        equipoBarra.UsuarioUpdate = User.Identity.Name;
                //        equipoBarra.FechaUpdate = DateTime.Today;
                //        equipoServicio.UpdateEqEquipo(equipoBarra);
                //    }
                //}

                //if (bHayOriginales && !bHayNuevos)
                //{
                //    foreach (var codigo in listaCodigosOriginales)
                //    {
                //        var equipoBarra = equipoServicio.GetByIdEqEquipo(codigo);
                //        equipoBarra.Grupocodi = null;
                //        equipoBarra.UsuarioUpdate = User.Identity.Name;
                //        equipoBarra.FechaUpdate = DateTime.Today;
                //        equipoServicio.UpdateEqEquipo(equipoBarra);
                //    }
                //}
                //if (bHayOriginales && bHayNuevos)
                //{

                //}
                #endregion
                string sCodigosNuevos = string.Empty;
                if (!string.IsNullOrEmpty(equipos))
                {
                    var codigoEquipo = equipos.Split('&').ToList();
                    sCodigosNuevos = string.Join(",", codigoEquipo.Select(n => n.ToString()).ToArray());
                }

                DespachoServicio.ActualizarBarrasEquipoPorBarraModelada(sCodigosNuevos, iGrupocodi, User.Identity.Name);
            }
            catch (Exception e)
            {
                log.Error("EditarBarra", e);
                return Json(-1);
            }
            return Json(1);
        }

        [HttpPost]
        public PartialViewResult DetalleBarra(int grupoCodi)
        {
            var barraModelada = DespachoServicio.GetByIdPrGrupo(grupoCodi);
            var barrasEquipamiento = DespachoServicio.ListadoBarrasEquipoPorBarraModelada(grupoCodi);
            DetalleBarraModelado modelo = new DetalleBarraModelado
            {
                ListabarrasEquipo = barrasEquipamiento,
                BarraModelada = barraModelada
            };
            return PartialView(modelo);
        }
    }
}
