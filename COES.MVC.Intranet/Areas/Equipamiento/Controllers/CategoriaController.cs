using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Equipamiento.Helper;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class CategoriaController : BaseController
    {
        GeneralAppServicio appGeneral = new GeneralAppServicio();
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CategoriaController));

        private readonly List<EstadoModel> _lsEstadosCategoria = new List<EstadoModel>();
        private readonly List<EstadoModel> _lsEstadosFlag = new List<EstadoModel>();
        private readonly List<EstadoModel> _lsEstadosEquipo = new List<EstadoModel>();

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("CategoriaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("CategoriaController", ex);
                throw;
            }
        }

        public CategoriaController()
        {
            _lsEstadosCategoria.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            _lsEstadosCategoria.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            _lsEstadosCategoria.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Pendiente" });
            _lsEstadosCategoria.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });

            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "S", EstadoDescripcion = "Sí" });
            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "N", EstadoDescripcion = "No" });

            _lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            _lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
            _lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "F", EstadoDescripcion = "Fuera de COES" });
            _lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "P", EstadoDescripcion = "Proyecto" });
            _lsEstadosEquipo.Add(new EstadoModel { EstadoCodigo = "X", EstadoDescripcion = "Anulado" });

            log4net.Config.XmlConfigurator.Configure();
        }

        #region Categoria
        //
        // GET: /Equipamiento/Categoria/
        public ActionResult Index()
        {
            CategoriaIndexModel modelo = new CategoriaIndexModel() { ListaEstados = _lsEstadosCategoria };

            //validación
            if (!base.IsValidSesion) return base.RedirectToLogin();

            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            modelo.AccesoNuevo = AccesoNuevo;
            modelo.AccesoEditar = AccesoEditar;

            //inicializar valores
            modelo.ListaTipoEquipo = appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();

            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult ListaCategoriaByEstado(int idFamilia, string sEstado)
        {
            CategoriaListaModel modelo = new CategoriaListaModel();
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";

            modelo.listaCategoria = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(idFamilia, sEstado);
            foreach (var ctg in modelo.listaCategoria)
            {
                ctg.CtgestadoDescripcion = EquipamientoHelper.EstadoDescripcion(ctg.Ctgestado);
                ctg.CtgFlagExcluyenteDescripcion = EquipamientoHelper.SiNoDescripcion(ctg.CtgFlagExcluyente);
            }

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ListaCategoriaByTipoEquipo(int idFamilia)
        {
            try
            {
                //var listaCategoria = appEquipamiento.ListEqCategoriaClasificacion(idFamilia);
                var listaCategoriaValida = new List<EqCategoriaDTO>();
                var listaCategoria = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(idFamilia, Constantes.EstadoActivo);
                var listaCategoriaTodo = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(0, Constantes.EstadoActivo); //Incluir las categorias (Todos los tipos de equipos)
                listaCategoria.AddRange(listaCategoriaTodo);

                foreach (var ctg in listaCategoria)
                {
                    if (ctg.TotalDetalle > 0)
                    {
                        if (ctg.Ctgpadre != null)
                        {
                            ctg.Ctgnomb = ctg.Ctgpadrenomb + " - " + ctg.Ctgnomb;
                        }
                        listaCategoriaValida.Add(ctg);
                    }
                }


                SelectList list = new SelectList(listaCategoriaValida, "Ctgcodi", "Ctgnomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ListaCategoriaByTipoEquipoExistentes(int idFamilia)
        {
            try
            {
                var listaCategoriaPermitida = new List<EqCategoriaDTO>();
                var listaCategoria = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(idFamilia, "-2");
                foreach (var ctg in listaCategoria)
                {
                    if (ctg.Ctgpadre == null)
                    {
                        listaCategoriaPermitida.Add(ctg);
                    }
                }
                SelectList list = new SelectList(listaCategoriaPermitida, "Ctgcodi", "Ctgnomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ListaEquipoByEmpresaAndFamilia(int idEmpresa, int idFamilia)
        {
            try
            {
                var listaEquipo = appEquipamiento.GetByCriteriaEqequipo(idEmpresa, idFamilia);
                SelectList list = new SelectList(listaEquipo, "Equicodi", "Equinomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Registrar nueva categoria
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevaCategoria()
        {
            CategoriaViewModel modelo = new CategoriaViewModel();
            modelo.ListaTipoEquipo = this.appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            int famcodi = modelo.ListaTipoEquipo.Count > 0 ? modelo.ListaTipoEquipo[0].Famcodi : 0;
            modelo.listaCategoria = appEquipamiento.ListEqCategoriaPadre(famcodi, 0);
            modelo.ListaEstadoFlag = _lsEstadosFlag;

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult RegistrarCategoria(CategoriaViewModel oCategoria)
        {
            try
            {
                var ctg = new EqCategoriaDTO();
                ctg.Famcodi = oCategoria.Famcodi;
                ctg.Ctgnomb = oCategoria.Ctgnomb != null ? oCategoria.Ctgnomb.Trim() : string.Empty;
                ctg.Ctgpadre = oCategoria.Ctgpadre <= 0 ? null : oCategoria.Ctgpadre;
                ctg.CtgFlagExcluyente = oCategoria.CtgFlagExcluyente;
                ctg.Ctgestado = Constantes.EstadoActivo;
                ctg.UsuarioCreacion = User.Identity.Name;

                if (ctg.Ctgnomb == null || ctg.Ctgnomb.Trim().Equals(""))
                {
                    throw new Exception("Debe ingresar un nombre para la categoría");
                }

                var listaHijo = appEquipamiento.ListCategoriaHijoByIdPadre(ctg.Famcodi, ctg.Ctgpadre.GetValueOrDefault(-1));
                var listaIguales = listaHijo.Where(x => x.Ctgnomb.Trim().ToUpper() == ctg.Ctgnomb.Trim().ToUpper()).ToList();
                if (listaIguales.Count > 0)
                {
                    throw new Exception("Ya existela categoría con el nombre " + ctg.Ctgnomb);
                }

                if (ctg.Ctgpadre != null)
                {
                    var octgpadre = appEquipamiento.GetByIdEqCategoria(ctg.Ctgpadre.Value);
                    if (octgpadre.Ctgpadre != null) //el padre debe tener su padre null
                    {
                        return Json(-1);
                    }
                }

                appEquipamiento.SaveEqCategoria(ctg);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Ver informacion de categoria
        /// </summary>
        [HttpPost]
        public PartialViewResult VerCategoria(int idCategoria)
        {
            var octg = appEquipamiento.GetByIdEqCategoria(idCategoria);
            var modelo = new CategoriaViewModel
            {
                Ctgcodi = octg.Ctgcodi,
                Ctgnomb = octg.Ctgnomb,
                Ctgpadre = octg.Ctgpadre,
                Ctgpadrenomb = octg.Ctgpadrenomb,
                Famcodi = octg.Famcodi,
                Famnomb = octg.Famnomb,
                CtgestadoDescripcion = EquipamientoHelper.EstadoDescripcion(octg.Ctgestado),
                CtgFlagExcluyenteDescripcion = EquipamientoHelper.SiNoDescripcion(octg.CtgFlagExcluyente),
                UsuarioCreacion = octg.UsuarioCreacion,
                UsuarioUpdate = octg.UsuarioUpdate,
                FechaCreacion = octg.FechaCreacion.HasValue ? octg.FechaCreacion.Value.ToString(Constantes.FormatoFecha) : "",
                FechaUpdate = octg.FechaUpdate.HasValue ? octg.FechaUpdate.Value.ToString(Constantes.FormatoFecha) : ""
            };
            return PartialView(modelo);
        }

        /// <summary>
        /// Editar categoria
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarCategoria(int idCategoria)
        {
            var octg = appEquipamiento.GetByIdEqCategoria(idCategoria);
            var modelo = new CategoriaViewModel
            {
                Ctgcodi = octg.Ctgcodi,
                Ctgnomb = octg.Ctgnomb,
                Ctgpadre = octg.Ctgpadre,
                Ctgpadrenomb = octg.Ctgpadrenomb,
                Famcodi = octg.Famcodi,
                Famnomb = octg.Famnomb,
                CtgFlagExcluyente = octg.CtgFlagExcluyente,
                Ctgestado = octg.Ctgestado
            };
            modelo.ListaTipoEquipo = this.appEquipamiento.ListEqFamilias().Where(t => t.Famcodi == octg.Famcodi).ToList();
            modelo.listaCategoria = appEquipamiento.ListEqCategoriaPadre(octg.Famcodi, octg.Ctgcodi);
            modelo.ListaEstados = _lsEstadosCategoria;
            modelo.ListaEstadoFlag = _lsEstadosFlag;

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ActualizarCategoria(CategoriaViewModel oCategoria)
        {
            try
            {
                var ctg = new EqCategoriaDTO();
                ctg.Ctgcodi = oCategoria.Ctgcodi;
                ctg.Ctgnomb = oCategoria.Ctgnomb.Trim();
                ctg.CtgFlagExcluyente = oCategoria.CtgFlagExcluyente;
                ctg.Ctgestado = oCategoria.Ctgestado;
                ctg.UsuarioUpdate = User.Identity.Name;

                if (ctg.Ctgnomb == null || ctg.Ctgnomb.Trim().Equals(""))
                {
                    return Json("Debe ingresar un nombre para la categoría");
                }

                var octgActual = appEquipamiento.GetByIdEqCategoria(ctg.Ctgcodi);
                if (!octgActual.Ctgnomb.Trim().ToUpper().Equals(ctg.Ctgnomb.ToUpper()))
                {
                    var listaHijo = appEquipamiento.ListCategoriaHijoByIdPadre(octgActual.Famcodi, octgActual.Ctgpadre.GetValueOrDefault(-1));
                    var listaIguales = listaHijo.Where(x => x.Ctgnomb.ToUpper() == ctg.Ctgnomb.ToUpper()).ToList();
                    if (listaIguales.Count > 0)
                    {
                        return Json("Ya existe la categoría con el nombre " + ctg.Ctgnomb);
                    }
                }

                appEquipamiento.UpdateEqCategoria(ctg);

                //si cambia el estado de la categoria, cambiar tambien de sus hijas y de sus subcategorias, y clasificaciones
                actualizarCategoriaHijos(octgActual.Famcodi, ctg.Ctgcodi, ctg.Ctgestado);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        private void actualizarCategoriaHijos(int famcodi, int ctgcodi, string estado)
        {
            var listaHijo = appEquipamiento.ListCategoriaHijoByIdPadre(famcodi, ctgcodi);
            foreach (var h in listaHijo)
            {
                //actualizar categoria hijo
                EqCategoriaDTO eqctg = this.appEquipamiento.GetByIdEqCategoria(h.Ctgcodi);
                eqctg.Ctgestado = estado;
                eqctg.UsuarioUpdate = User.Identity.Name;

                appEquipamiento.UpdateEqCategoria(eqctg);

                //actualizar subcategorias
                actualizarSubcategorias(h.Ctgcodi, estado);
            }

            //actualizar subcategorias
            actualizarSubcategorias(ctgcodi, estado);

        }

        private void actualizarSubcategorias(int ctgcodi, string estado)
        {
            var listaCategoriaDetalle = appEquipamiento.ListEqCategoriaDetalleByCategoriaAndEstado(ctgcodi, "-2");
            foreach (var s in listaCategoriaDetalle)
            {
                //actualizar subcategoria
                var octgdet = appEquipamiento.GetByIdEqCategoriaDetalle(s.Ctgdetcodi);
                octgdet.Ctgdetestado = estado;
                octgdet.UsuarioUpdate = User.Identity.Name;

                appEquipamiento.UpdateEqCategoriaDetalle(octgdet);

                //actualizar clasificacion de equipos
                actualizarClasificacion(s.Ctgdetcodi, estado);
            }
        }

        private void actualizarClasificacion(int ctgdetcodi, string estado)
        {
            var listaCtgEqui = appEquipamiento.ListaClasificacionByCategoriaDetalle(ctgdetcodi);
            foreach (var e in listaCtgEqui)
            {
                var obj = appEquipamiento.GetByIdEqCategoriaEquipo(e.Ctgdetcodi, e.Equicodi);
                if (obj != null)
                {
                    obj.EquicodiOld = obj.Equicodi;
                    obj.CtgdetcodiOld = obj.Ctgdetcodi;
                    obj.Ctgequiestado = estado;
                    obj.UsuarioUpdate = User.Identity.Name;

                    appEquipamiento.UpdateEqCategoriaEquipo(obj);
                }
            }
        }

        /// <summary>
        /// Eliminar categoria
        /// </summary>
        [HttpPost]
        public JsonResult EliminarCategoria(int idCategoria)
        {
            try
            {
                EqCategoriaDTO eqctg = this.appEquipamiento.GetByIdEqCategoria(idCategoria);
                if (eqctg != null)
                {
                    if (eqctg.TotalDetalle != null && eqctg.TotalDetalle > 0)
                    {
                        return Json("La categoria tiene 1 o más subcategorias");
                    }
                    if (eqctg.TotalHijo != null && eqctg.TotalHijo > 0)
                    {
                        return Json("La categoria tiene 1 o más categorias hijas");
                    }

                    appEquipamiento.DeleteEqCategoria(idCategoria);
                    return Json(1);
                }
                return Json("No existe la categoria");
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }
        #endregion

        #region Categoria Detalle
        //
        // GET: /Equipamiento/Categoria/IndexCategoriaDetalle/
        public ActionResult IndexCategoriaDetalle(int? ctg)
        {
            CategoriaDetIndexModel modelo = new CategoriaDetIndexModel();

            //validación
            if (!base.IsValidSesion) return base.RedirectToLogin();

            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            modelo.AccesoNuevo = AccesoNuevo;
            modelo.AccesoEditar = AccesoEditar;

            //inicializar valores
            if (ctg.HasValue)
            {
                var octg = appEquipamiento.GetByIdEqCategoria(ctg.Value);
                if (octg != null)
                {
                    modelo.Ctgcodi = octg.Ctgcodi;
                    modelo.Ctgnomb = octg.Ctgnomb;
                }
            }


            return View(modelo);
        }

        [HttpPost]
        public PartialViewResult ListaCategoriaDetalle(int ctgcodi)
        {
            CategoriaDetListaModel modelo = new CategoriaDetListaModel();
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";

            modelo.listaCategoriaDetalle = appEquipamiento.ListEqCategoriaDetalleByCategoriaAndEstado(ctgcodi, "-2");
            foreach (var ctgdet in modelo.listaCategoriaDetalle)
            {
                ctgdet.CtgdetestadoDescripcion = EquipamientoHelper.EstadoDescripcion(ctgdet.Ctgdetestado);
            }

            return PartialView(modelo);
        }

        /// <summary>
        /// Registrar nuevo categoria detalle
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoCategoriaDetalle(int ctgcodi)
        {
            CategoriaDetViewModel modelo = new CategoriaDetViewModel();
            var octg = appEquipamiento.GetByIdEqCategoria(ctgcodi);
            modelo.Ctgcodi = octg.Ctgcodi;
            modelo.Ctgnomb = octg.Ctgnomb;

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult RegistrarCategoriaDetalle(CategoriaDetViewModel oCtgdet)
        {
            try
            {
                var ctgdet = new EqCategoriaDetDTO();
                ctgdet.Ctgdetcodi = oCtgdet.Ctgdetcodi;
                ctgdet.Ctgdetnomb = oCtgdet.Ctgnomb.Trim();
                ctgdet.Ctgcodi = oCtgdet.Ctgcodi;
                ctgdet.Ctgdetestado = Constantes.EstadoActivo;
                ctgdet.UsuarioCreacion = User.Identity.Name;

                if (ctgdet.Ctgdetnomb == null || ctgdet.Ctgdetnomb.Trim().Equals(""))
                {
                    return Json("Debe ingresar un nombre para la subcategoría");
                }

                var listaSubcategoria = appEquipamiento.ListEqCategoriaDetalleByCategoriaAndEstado(ctgdet.Ctgcodi, "-2");
                var listaIguales = listaSubcategoria.Where(x => x.Ctgdetnomb.Trim().ToUpper() == ctgdet.Ctgdetnomb.Trim().ToUpper()).ToList();
                if (listaIguales.Count > 0)
                {
                    return Json("Ya existe la subcategoría con el nombre " + ctgdet.Ctgdetnomb);
                }

                appEquipamiento.SaveEqCategoriaDetalle(ctgdet);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver informacion de categoria
        /// </summary>
        [HttpPost]
        public PartialViewResult VerCategoriaDetalle(int idCategoriaDet)
        {
            var octgdet = appEquipamiento.GetByIdEqCategoriaDetalle(idCategoriaDet);
            var modelo = new CategoriaDetViewModel
            {
                Ctgdetcodi = octgdet.Ctgdetcodi,
                Ctgdetnomb = octgdet.Ctgdetnomb,
                Ctgcodi = octgdet.Ctgcodi,
                CtgdetestadoDescripcion = EquipamientoHelper.EstadoDescripcion(octgdet.Ctgdetestado),
                UsuarioCreacion = octgdet.UsuarioCreacion,
                UsuarioUpdate = octgdet.UsuarioUpdate,
                FechaCreacion = octgdet.FechaCreacion.HasValue ? octgdet.FechaCreacion.Value.ToString(Constantes.FormatoFecha) : String.Empty,
                FechaUpdate = octgdet.FechaUpdate.HasValue ? octgdet.FechaUpdate.Value.ToString(Constantes.FormatoFecha) : String.Empty,

                Famnomb = octgdet.Famnomb,
                Ctgpadrenomb = octgdet.Ctgpadrenomb,
                Ctgnomb = octgdet.Ctgnomb
            };
            return PartialView(modelo);
        }

        /// <summary>
        /// Editar categoria detalle
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarCategoriaDetalle(int idCtgdet)
        {
            var octgdet = appEquipamiento.GetByIdEqCategoriaDetalle(idCtgdet);
            var modelo = new CategoriaDetViewModel
            {
                Ctgdetcodi = octgdet.Ctgdetcodi,
                Ctgdetnomb = octgdet.Ctgdetnomb,
                Ctgdetestado = octgdet.Ctgdetestado,
                Ctgnomb = octgdet.Ctgnomb
            };
            modelo.ListaEstados = _lsEstadosCategoria;

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ActualizarCategoriaDetalle(CategoriaDetViewModel oCtgdet)
        {
            try
            {
                var ctgdet = new EqCategoriaDetDTO();
                ctgdet.Ctgdetcodi = oCtgdet.Ctgdetcodi;
                ctgdet.Ctgdetnomb = oCtgdet.Ctgdetnomb.Trim();
                ctgdet.Ctgdetestado = oCtgdet.Ctgdetestado;
                ctgdet.UsuarioUpdate = User.Identity.Name;

                if (ctgdet.Ctgdetnomb == null || ctgdet.Ctgdetnomb.Trim().Equals(""))
                {
                    return Json("Debe ingresar un nombre para la subcategoría");
                }

                var octgActual = appEquipamiento.GetByIdEqCategoriaDetalle(ctgdet.Ctgdetcodi);
                if (!octgActual.Ctgdetnomb.Trim().ToUpper().Equals(ctgdet.Ctgdetnomb.ToUpper()))
                {
                    var listaSubcategoria = appEquipamiento.ListEqCategoriaDetalleByCategoriaAndEstado(octgActual.Ctgcodi, "-2");
                    var listaIguales = listaSubcategoria.Where(x => x.Ctgdetnomb.Trim().ToUpper() == ctgdet.Ctgdetnomb.Trim().ToUpper()).ToList();
                    if (listaIguales.Count > 0)
                    {
                        return Json("Ya existe la subcategoría con el nombre " + ctgdet.Ctgdetnomb);
                    }

                }

                appEquipamiento.UpdateEqCategoriaDetalle(ctgdet);

                //actualizar clasificacion de equipos
                actualizarClasificacion(ctgdet.Ctgdetcodi, ctgdet.Ctgdetestado);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Eliminar categoria detalle
        /// </summary>
        [HttpPost]
        public JsonResult EliminarCategoriaDetalle(int idCtgdet)
        {
            try
            {
                EqCategoriaDetDTO eqDet = this.appEquipamiento.GetByIdEqCategoriaDetalle(idCtgdet);
                if (eqDet != null)
                {
                    if (eqDet.TotalEquipo > 0)
                    {
                        return Json("La subcategoria esta asignado a 1 o más equipos");
                    }

                    appEquipamiento.DeleteEqCategoriaDetalle(idCtgdet);
                    return Json(1);
                }
                return Json("No existe la subcategoria");
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }
        #endregion

        #region Categoria Equipo
        //
        // GET: /Equipamiento/Categoria/IndexClasificacionEquipo/
        public ActionResult IndexClasificacionEquipo()
        {
            CategoriaEquipoIndexModel modelo = new CategoriaEquipoIndexModel();

            //validación
            if (!base.IsValidSesion) return base.RedirectToLogin();

            bool AccesoNuevo = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Nuevo);
            bool AccesoEditar = Tools.VerificarAcceso(Session[DatosSesion.SesionIdOpcion], User.Identity.Name, Acciones.Editar);
            modelo.EnableNuevo = AccesoNuevo ? "" : "disabled=disabled";
            modelo.EnableEditar = AccesoEditar ? "" : "disableClick";
            modelo.AccesoNuevo = AccesoNuevo;
            modelo.AccesoEditar = AccesoEditar;

            //inicializar valores
            modelo.ListaTipoEmpresa =
                this.appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();
            modelo.ListaTipoEquipo =
                this.appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            modelo.ListaEquipo = new List<EqEquipoDTO>();
            modelo.ListaCategoria = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(-2, ConstantesAppServicio.Activo);
            modelo.ListaSubclasificacion = new List<EqCategoriaDetDTO>();

            modelo.ListaEstados = _lsEstadosEquipo;
            modelo.iEmpresa = 0;
            modelo.iCategoria = 0;
            modelo.iTipoEmpresa = 0;
            modelo.iTipoEquipo = 0;
            modelo.sEstadoCodi = "A";
            modelo.ListaEmpresa = new List<SiEmpresaDTO>();

            return View(modelo);
        }

        [HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            List<SiEmpresaDTO> entitys = this.appGeneral.ListadoComboEmpresasPorTipo(idTipoEmpresa);
            var list = new SelectList(entitys, "Emprcodi", "Emprnomb");
            return Json(list);
        }

        [HttpPost]
        public PartialViewResult PaginadoClasificacion(CategoriaEquipoIndexModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;
            string sNombre = model.NombreEquipo;
            int iCategoria = model.iCategoria;
            int iSubclasificacion = model.iSubclasificacion;

            int nroRegistros = this.appEquipamiento.TotalClasificacion(iEmpresa, iFamilia, iTipoEmpresa, iEquipo, iCategoria, iSubclasificacion, sNombre);

            model.IndicadorPagina = false;
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListaClasificacion(CategoriaEquipoIndexModel model)
        {
            int iEmpresa = model.iEmpresa;
            int iFamilia = model.iTipoEquipo;
            int iTipoEmpresa = model.iTipoEmpresa;
            int iEquipo = string.IsNullOrEmpty(model.CodigoEquipo) ? -2 : Convert.ToInt32(model.CodigoEquipo.Trim());
            string sEstado = model.sEstadoCodi;
            string sNombre = model.NombreEquipo;
            int iCategoria = model.iCategoria;
            int iSubclasificacion = model.iSubclasificacion;
            var lsResultado = this.appEquipamiento.ListaClasificacionPaginado(iEmpresa, iFamilia, iTipoEmpresa, iEquipo,
                iCategoria, iSubclasificacion, sNombre, model.NroPagina, Constantes.PageSizeEvento);
            foreach (var obj in lsResultado)
            {
                obj.Ctgequiestadodescripcion = EquipamientoHelper.EstadoDescripcion(obj.Ctgequiestado);
            }

            model.ListaCategoriaEquipo = lsResultado;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ListaCategoria(int idFamilia)
        {
            try
            {
                var listaCategoria = appEquipamiento.ListEqCategoriaByFamilia(idFamilia);
                SelectList list = new SelectList(listaCategoria, "Ctgcodi", "Ctgnomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ListaSubclasificacion(int idCtg)
        {
            try
            {
                var listaSub = appEquipamiento.ListEqCategoriaDetalleByCategoria(idCtg);
                SelectList list = new SelectList(listaSub, "Ctgdetcodi", "Ctgdetnomb");
                return Json(list);
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver informacion de categoria equipo
        /// </summary>
        [HttpPost]
        public PartialViewResult VerClasificacionEquipo(int idCategoriaDet, int idEquipo)
        {
            var obj = appEquipamiento.GetByIdEqCategoriaEquipo(idCategoriaDet, idEquipo);
            var modelo = new CategoriaEquipoViewModel
            {
                Equicodi = obj.Equicodi,
                Ctgdetcodi = obj.Ctgdetcodi,
                Ctgdetnomb = obj.Ctgdetnomb,
                Ctgpadrenomb = obj.Ctgpadrenomb,

                Ctgequiestadodescripcion = EquipamientoHelper.EstadoDescripcion(obj.Ctgequiestado),
                UsuarioCreacion = obj.UsuarioCreacion,
                UsuarioUpdate = obj.UsuarioUpdate,
                FechaCreacion = obj.FechaCreacion.HasValue ? obj.FechaCreacion.Value.ToString(Constantes.FormatoFecha) : String.Empty,
                FechaUpdate = obj.FechaUpdate.HasValue ? obj.FechaUpdate.Value.ToString(Constantes.FormatoFecha) : String.Empty,

                Equinomb = obj.Equinomb,
                Famnomb = obj.Famnomb,
                Ctgnomb = obj.Ctgnomb,
                Emprnomb = obj.Emprnomb
            };
            return PartialView(modelo);
        }

        /// <summary>
        /// Registrar nueva categoria detalle
        /// </summary>
        [HttpPost]
        public PartialViewResult NuevoClasificacionEquipo()
        {
            CategoriaEquipoViewModel modelo = new CategoriaEquipoViewModel();
            modelo.ListaTipoEmpresa = this.appGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList();
            modelo.ListaEmpresa = this.appGeneral.ListadoComboEmpresasPorTipo(-2);
            modelo.ListaTipoEquipo = this.appEquipamiento.ListEqFamilias().Where(t => t.Famcodi > 0).OrderBy(t => t.Famnomb).ToList();
            modelo.ListaEquipo = new List<EqEquipoDTO>();
            modelo.ListaCategoria = this.appEquipamiento.ListEqCategoriaByFamiliaAndEstado(0, Constantes.EstadoActivo); //Incluir las categorias (Todos los tipos de equipos)
            modelo.ListaSubclasificacion = new List<EqCategoriaDetDTO>();

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult RegistrarCategoriaEquipo(CategoriaEquipoViewModel oClasif)
        {
            try
            {
                var clasif = new EqCategoriaEquipoDTO();
                clasif.Equicodi = oClasif.Equicodi;
                clasif.Ctgdetcodi = oClasif.Ctgdetcodi;
                clasif.Ctgequiestado = Constantes.EstadoActivo;
                clasif.UsuarioCreacion = User.Identity.Name;

                //validar si existe
                var objExistente = appEquipamiento.GetByIdEqCategoriaEquipo(clasif.Ctgdetcodi, clasif.Equicodi);
                if (objExistente != null)
                {
                    return Json("Ya existe la clasificación del equipo");
                }

                //validar que el equipo solo asociado 1 detalle de la categoria
                List<EqCategoriaEquipoDTO> listaClas = this.appEquipamiento.ListaClasificacionByCategoriaAndEquipo(clasif.Ctgdetcodi, clasif.Equicodi);
                listaClas = listaClas.Where(x => x.Ctgequiestado == Constantes.EstadoActivo).ToList();
                if (listaClas.Count > 0)
                {
                    return Json("Solo puede existir una clasificación de equipo perteneciente a la categoria " + listaClas[0].Ctgnomb);
                }
                else
                {
                    //validar si la categoria es excluyente
                    EqCategoriaDetDTO objdet = this.appEquipamiento.GetByIdEqCategoriaDetalle(clasif.Ctgdetcodi);
                    EqCategoriaDTO objCtg = this.appEquipamiento.GetByIdEqCategoria(objdet.Ctgcodi);

                    List<EqCategoriaEquipoDTO> listaClasxPadre = this.appEquipamiento.ListaClasificacionByCategoriaPadreAndEquipo(objdet.Ctgpadrecodi, clasif.Equicodi);
                    listaClasxPadre = listaClasxPadre.Where(x => x.Ctgequiestado == Constantes.EstadoActivo).ToList();
                    var listaExcluyente = listaClasxPadre.Where(x => x.CtgFlagExcluyente == "S").ToList();
                    if (objCtg.CtgFlagExcluyente == "N")
                    {
                        //guardar categoria equipo
                        appEquipamiento.SaveEqCategoriaEquipo(clasif);
                        return Json(1);
                    }
                    else
                    {
                        if (listaExcluyente != null && listaExcluyente.Count > 0)
                        {
                            return Json("Categoria " + objdet.Ctgnomb + " es excluyente de otras categorias, por tal motivo no se pueden registrar la clasificación");
                        }
                        else
                        {
                            //guardar categoria equipo
                            appEquipamiento.SaveEqCategoriaEquipo(clasif);
                            return Json(1);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Editar categoria
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarClasificacionEquipo(int idCtgdet, int idEquipo)
        {
            var obj = appEquipamiento.GetByIdEqCategoriaEquipo(idCtgdet, idEquipo);
            var modelo = new CategoriaEquipoViewModel
            {
                Equicodi = obj.Equicodi,
                Ctgdetcodi = obj.Ctgdetcodi,
                EquicodiOld = obj.Equicodi,
                CtgdetcodiOld = obj.Ctgdetcodi,
                Ctgequiestado = obj.Ctgequiestado,
                Ctgequiestadodescripcion = EquipamientoHelper.EstadoDescripcion(obj.Ctgequiestado),

                Famcodi = obj.Famcodi,
                Ctgcodi = obj.Ctgcodi,
                Ctgdetnomb = obj.Ctgdetnomb,
                Ctgpadrenomb = obj.Ctgpadrenomb,
                Equinomb = obj.Equinomb,
                Famnomb = obj.Famnomb,
                Ctgnomb = obj.Ctgnomb,
                Emprnomb = obj.Emprnomb
            };
            modelo.ListaEstados = _lsEstadosCategoria;

            var listaCategoriaValida = new List<EqCategoriaDTO>();
            var listaCategoria = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(obj.Famcodi, Constantes.EstadoActivo);
            var listaCategoriaTodo = appEquipamiento.ListEqCategoriaByFamiliaAndEstado(0, Constantes.EstadoActivo); //Incluir las categorias (Todos los tipos de equipos)
            listaCategoria.AddRange(listaCategoriaTodo);

            foreach (var ctg in listaCategoria)
            {
                if (ctg.TotalDetalle > 0)
                {
                    if (ctg.Ctgpadre != null)
                    {
                        ctg.Ctgnomb = ctg.Ctgpadrenomb + " - " + ctg.Ctgnomb;
                    }
                    listaCategoriaValida.Add(ctg);
                }
            }

            modelo.ListaCategoria = listaCategoriaValida;

            modelo.ListaSubclasificacion = appEquipamiento.ListEqCategoriaDetalleByCategoria(obj.Ctgcodi);

            return PartialView(modelo);
        }

        [HttpPost]
        public JsonResult ActualizarCategoriaEquipo(CategoriaEquipoViewModel oCtgequi)
        {
            try
            {
                var ctgequi = new EqCategoriaEquipoDTO();
                ctgequi.Ctgdetcodi = oCtgequi.Ctgdetcodi;
                ctgequi.Equicodi = oCtgequi.Equicodi;
                ctgequi.Ctgequiestado = oCtgequi.Ctgequiestado;
                ctgequi.UsuarioUpdate = User.Identity.Name;
                ctgequi.CtgdetcodiOld = oCtgequi.CtgdetcodiOld;
                ctgequi.EquicodiOld = oCtgequi.EquicodiOld;

                var obj = this.appEquipamiento.GetByIdEqCategoriaEquipo(oCtgequi.CtgdetcodiOld, oCtgequi.EquicodiOld);
                if (obj.Equicodi == ctgequi.Equicodi && ctgequi.Ctgdetcodi == obj.Ctgdetcodi) // si no se cambio la categoria detalle
                { }
                else
                {
                    //validar si existe categoria detalle para tal equipo
                    var objExistente = appEquipamiento.GetByIdEqCategoriaEquipo(ctgequi.Ctgdetcodi, ctgequi.Equicodi);
                    if (objExistente != null)
                    {
                        return Json("Ya existe la clasificación para el equipo");
                    }

                    //validar que el equipo solo esta asociado a una subcategoria
                    List<EqCategoriaEquipoDTO> listaClas = this.appEquipamiento.ListaClasificacionByCategoriaAndEquipo(ctgequi.Ctgdetcodi, ctgequi.Equicodi);
                    listaClas = listaClas.Where(x => x.Ctgequiestado == Constantes.EstadoActivo).ToList();
                    if (listaClas.Count > 0)
                    {
                        return Json("Solo puede existir una clasificación de equipo perteneciente a la categoria " + listaClas[0].Ctgnomb);
                    }
                }

                // si el estado no es activo, guardarlo sin validar
                if (ctgequi.Ctgequiestado != Constantes.EstadoActivo)
                {
                    appEquipamiento.UpdateEqCategoriaEquipo(ctgequi);
                    return Json(1);
                }

                //validar si la categoria es excluyente
                EqCategoriaDetDTO objdet = this.appEquipamiento.GetByIdEqCategoriaDetalle(ctgequi.Ctgdetcodi);
                EqCategoriaDTO objCtg = this.appEquipamiento.GetByIdEqCategoria(objdet.Ctgcodi);

                List<EqCategoriaEquipoDTO> listaClasxPadre = this.appEquipamiento.ListaClasificacionByCategoriaPadreAndEquipo(objdet.Ctgpadrecodi, ctgequi.Equicodi);
                listaClasxPadre = listaClasxPadre.Where(x => x.Ctgequiestado == Constantes.EstadoActivo).ToList();

                var listaExcluyente = listaClasxPadre.Where(x => x.Equicodi != ctgequi.EquicodiOld || x.Ctgdetcodi != ctgequi.CtgdetcodiOld).ToList();
                listaExcluyente = listaExcluyente.Where(x => x.CtgFlagExcluyente == "S").ToList();
                if (objCtg.CtgFlagExcluyente == "N")
                { }
                else
                {
                    if (listaExcluyente != null && listaExcluyente.Count > 0)
                    {
                        return Json("Categoria " + objdet.Ctgnomb + " es excluyente de otras categorias, por tal motivo no se pueden actualizar la clasificación");
                    }
                }

                appEquipamiento.UpdateEqCategoriaEquipo(ctgequi);
                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error("CategoriaController", ex);
                return Json(-1);
            }
        }
        #endregion
    }
}
