using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Equipamiento.Helper;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FamiliaController : Controller
    {
        //
        // GET: /Equipamiento/Familia/
        private EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        readonly List<EstadoModel> _lsEstadosFamilia = new List<EstadoModel>();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FamiliaController));

        public FamiliaController()
        {
            _lsEstadosFamilia.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            _lsEstadosFamilia.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            _lsEstadosFamilia.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Pendiente" });
            _lsEstadosFamilia.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("FamiliaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("FamiliaController", ex);
                throw;
            }
        }

        public ActionResult Index()
        {
            var modelo = new FamiliaIndexModel(){ListaEstados = _lsEstadosFamilia};
            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            return View(modelo);
        }
        [HttpPost]
        public PartialViewResult Lista(string sFamEstado)
        {
            var modelo= new FamiliaListaModel();
            modelo.listaFamilia = appEquipamiento.GetByCriteriaEqFamilias(sFamEstado);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            foreach (var fam in modelo.listaFamilia)
            {
                fam.EstadoDescripcion = EquipamientoHelper.EstadoDescripcion(fam.Famestado);
                fam.UsuarioCreacion = EquipamientoHelper.EstiloEstado(fam.Famestado);
            }
            return PartialView(modelo);
        }

        [HttpPost]
        public PartialViewResult DetalleFamilia(int famcodi)
        {
            var oFam= appEquipamiento.GetByIdEqFamilia(famcodi);
            var modelo = new FamiliaDetalleModel
            {
                Famabrev = oFam.Famabrev,
                Famcodi = oFam.Famcodi,
                Famnomb = oFam.Famnomb,
                Famnombgraf = oFam.Famnombgraf,
                Famnumconec = oFam.Famnumconec,
                Tareacodi = oFam.Tareacodi,
                Tipoecodi = oFam.Tipoecodi,
                EstadoDescripcion = EquipamientoHelper.EstadoDescripcion(oFam.Famestado),
                UsuarioCreacion = oFam.UsuarioCreacion,
                UsuarioUpdate = oFam.UsuarioUpdate,
                FechaCreacion = oFam.FechaCreacion.HasValue ? oFam.FechaCreacion.Value.ToString("dd/MM/yyyy") : "",
                FechaUpdate = oFam.FechaUpdate.HasValue ? oFam.FechaUpdate.Value.ToString("dd/MM/yyyy") : ""
            };
            return PartialView(modelo);
        }
        [HttpPost]
        public PartialViewResult NuevaFamilia()
        {
            var modelo = new FamiliaDetalleModel
            {
                ListaTipoArea = appEquipamiento.ListEqTipoareas()
            };
            return PartialView(modelo);
        }
        [HttpPost]
        public JsonResult GuardarFamilia(FamiliaDetalleModel oFamilia)
        {
            try
            {
                var oNuevaFamilia = new EqFamiliaDTO()
                {
                    Famabrev= oFamilia.Famabrev,
                    Famnomb = oFamilia.Famnomb,
                    Famnombgraf = oFamilia.Famnombgraf,
                    Famnumconec = (short?) (oFamilia.Famnumconec.HasValue ? short.Parse(oFamilia.Famnumconec.Value.ToString()) : (int?)null),
                    Tareacodi = (short?)(oFamilia.Tareacodi.HasValue ? short.Parse(oFamilia.Tareacodi.Value.ToString()) : (int?)null),
                    Tipoecodi = (short?)(oFamilia.Tipoecodi.HasValue ? short.Parse(oFamilia.Tipoecodi.Value.ToString()) : (int?)null),
                    UsuarioCreacion = User.Identity.Name
                };
                appEquipamiento.SaveEqFamilia(oNuevaFamilia);

                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public PartialViewResult EditarFamilia(int famcodi)
        {
            var oFam = appEquipamiento.GetByIdEqFamilia(famcodi);
            var modelo = new FamiliaDetalleModel
            {
                Famabrev = oFam.Famabrev,
                Famcodi = oFam.Famcodi,
                Famnomb = oFam.Famnomb,
                Famnombgraf = oFam.Famnombgraf,
                Famnumconec = oFam.Famnumconec,
                Tareacodi = oFam.Tareacodi,
                Tipoecodi = oFam.Tipoecodi,
                ListaTipoArea = appEquipamiento.ListEqTipoareas(),
                EstadoDescripcion = EquipamientoHelper.EstadoDescripcion(oFam.Famestado),
                Famestado = oFam.Famestado,
                ListaEstados = _lsEstadosFamilia
            };
            return PartialView(modelo);
        }

        public JsonResult ActualizarFamilia(FamiliaDetalleModel oFamilia)
        {
            try
            {
                var oNuevaFamilia = new EqFamiliaDTO()
                {
                    Famcodi = oFamilia.Famcodi,
                    Famabrev = oFamilia.Famabrev,
                    Famnomb = oFamilia.Famnomb,
                    Famnombgraf = oFamilia.Famnombgraf,
                    Famnumconec = (short?)(oFamilia.Famnumconec.HasValue ? short.Parse(oFamilia.Famnumconec.Value.ToString()) : (int?)null),
                    Tareacodi = (short?)(oFamilia.Tareacodi.HasValue ? short.Parse(oFamilia.Tareacodi.Value.ToString()) : (int?)null),
                    Tipoecodi = (short?)(oFamilia.Tipoecodi.HasValue ? short.Parse(oFamilia.Tipoecodi.Value.ToString()) : (int?)null),
                    Famestado = oFamilia.Famestado,
                    UsuarioUpdate = User.Identity.Name
                };
                appEquipamiento.UpdateEqFamilia(oNuevaFamilia);

                return Json(1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }
    }
}
