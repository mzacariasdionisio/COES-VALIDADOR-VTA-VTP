using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class ContactoController : Controller
    {
        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        ContactoAppServicio servicio = new ContactoAppServicio();

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ContactoModel model = new ContactoModel();
            model.ListaTipoEmpresas = this.servicio.ObtenerTipoEmpresas();
            model.ListaEmpresas = this.servicio.ObtenerEmpresas(null);
            model.ListaComite = this.servicio.ListWbComites();
            //model.ListaCorreos = this.servicio.ListWbComites();
            model.ListaProceso = this.servicio.ListWbProcesos();
            return View(model);
        }

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Consulta()
        {
            ContactoModel model = new ContactoModel();
            model.ListaTipoEmpresas = this.servicio.ObtenerTipoEmpresas();
            model.ListaEmpresas = this.servicio.ObtenerEmpresas(null);
            return View(model);
        }

        /// <summary>
        /// Permite cargar las empresas por tipo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int? idTipoEmpresa)
        {
            List<SiEmpresaDTO> entitys = this.servicio.ObtenerEmpresas(idTipoEmpresa);
            SelectList list = new SelectList(entitys, EntidadPropiedad.Emprcodi, EntidadPropiedad.Emprnomb);
            return Json(list);
        }

        /// <summary>
        /// Permite mostrar la consulta en base a los filtros
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fuente"></param>
        /// <returns></returns>
        public PartialViewResult Listar(int? idTipoEmpresa, int? idEmpresa, string fuente, string publico, int? idComite, int? idComiteLista, int? idProceso)
        {
            ContactoModel model = new ContactoModel();
            model.ListaContactos = this.servicio.GetByCriteriaWbContactos(idTipoEmpresa, idEmpresa, fuente, idComite, idComiteLista, idProceso);
            model.IndPublico = publico;
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos del contacto
        /// </summary>
        /// <param name="idContacto"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int idContacto, string fuente, string tipo)
        {
            ContactoModel model = new ContactoModel();
            model.ListaTipoEmpresas = this.servicio.ObtenerTipoEmpresas();

            model.ListaComitecontacto = this.servicio.GetByCriteriaWbComiteContactos(idContacto);
            List<int> ids = model.ListaComitecontacto.Where(x => x.Indicador > 0).Select(x => x.Comitecodi).ToList();
            model.Comites = string.Join<int>(Constantes.CaracterComa.ToString(), ids);

            model.ListaProcesocontacto = this.servicio.GetByCriteriaWbProcesoContactos(idContacto);
            List<int> idsp = model.ListaProcesocontacto.Where(x => x.Indicador > 0).Select(x => x.Procesocodi).ToList();
            model.Procesos = string.Join<int>(Constantes.CaracterComa.ToString(), idsp);

            model.ListaComiteListaContacto = this.servicio.GetByCriteriaWbComiteListaContacto(idContacto);
            List<int> idsc = model.ListaComiteListaContacto.Where(x => x.Indicador > 0).Select(x => x.ComiteListacodi).ToList();
            model.Correos = string.Join<int>(Constantes.CaracterComa.ToString(), idsc);

            model.IndicadorEdicion = tipo;
            model.Fuente = fuente;

            if (idContacto == 0)
            {
                model.Entidad = new WbContactoDTO();
                model.Entidad.Tipoemprcodi = -2;
                model.Entidad.Emprcodi = -1;
                model.ListaEmpresas = this.servicio.ObtenerEmpresas(null);
                model.Entidad.Contacestado = Constantes.EstadoActivo;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbContacto(idContacto, fuente);
                SiEmpresaDTO empresa = this.servicio.ObtenerEmpresa(model.Entidad.Emprcodi);
                model.Entidad.Tipoemprcodi = empresa.Tipoemprcodi;
                model.ListaEmpresas = this.servicio.ObtenerEmpresas(empresa.Tipoemprcodi);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del nuevo contacto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(ContactoModel model)
        {
            try
            {
                string fechaRegistroCadena = Request.Form["FechaRegistro"];
                model.FechaRegistro = string.IsNullOrWhiteSpace(fechaRegistroCadena) ? DateTime.ParseExact("01/01/1930", "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.ParseExact(fechaRegistroCadena, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                WbContactoDTO entity = new WbContactoDTO
                {
                    Contaccodi = model.Codigo,
                    Contacnombre = model.Nombre,
                    Contacapellido = model.Apellido,
                    Contacemail = model.Email,
                    Contaccargo = model.Cargo,
                    Contactelefono = model.Telefono,
                    Contacmovil = model.Movil,
                    Contaccomentario = model.Comentario,
                    Contacarea = model.Area,
                    Contacestado = model.Estado,
                    Emprcodi = model.IdEmpresa,
                    Contacdoc = model.Documento,
                    ContacFecRegistro = model.FechaRegistro
                };

                string items = string.Empty;

                if (!string.IsNullOrEmpty(model.Comites))
                {
                    items = model.Comites.Remove(model.Comites.Length - 1, 1);
                }

                string correos = string.Empty;

                if (!string.IsNullOrEmpty(model.Correos))
                {
                    correos = model.Correos.Remove(model.Correos.Length - 1, 1);
                }

                int id = this.servicio.SaveWbContacto(entity, items, correos);
                return Json(id);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Permite grabar los datos del nuevo contacto de proceso
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarProc(ContactoModel model)
        {
            try
            {
                string fechaRegistroCadena = Request.Form["FechaRegistro"];
                model.FechaRegistro = string.IsNullOrWhiteSpace(fechaRegistroCadena) ? DateTime.ParseExact("01/01/1930", "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.ParseExact(fechaRegistroCadena, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                WbContactoDTO entity = new WbContactoDTO
                {
                    Contaccodi = model.Codigo,
                    Contacnombre = model.Nombre,
                    Contacapellido = model.Apellido,
                    Contacemail = model.Email,
                    Contaccargo = model.Cargo,
                    Contactelefono = model.Telefono,
                    Contacmovil = model.Movil,
                    Contaccomentario = model.Comentario,
                    Contacarea = model.Area,
                    Contacestado = model.Estado,
                    Emprcodi = model.IdEmpresa,
                    Contacdoc = model.Documento,
                    ContacFecRegistro = model.FechaRegistro
                };

                string items = string.Empty;

                if (!string.IsNullOrEmpty(model.Procesos))
                {
                    items = model.Procesos.Remove(model.Procesos.Length - 1, 1);
                }

                int id = this.servicio.SaveWbContactoProc(entity, items);
                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite eliminar los datos de un contacto
        /// </summary>
        /// <param name="idContacto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idContacto)
        {
            try
            {
                this.servicio.DeleteWbContacto(idContacto);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int? idTipoEmpresa, int? idEmpresa, string fuente, string publico, int? idComite, int? idProceso, int? idComiteLista)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin;
                string file = ConstantesAdmin.ReporteContactos;
                List<WbContactoDTO> list = this.servicio.GetByCriteriaWbContactos(idTipoEmpresa, idEmpresa, fuente, idComite, idProceso, idComiteLista);
                ReporteHelper.GenerarReporteContactoExcel(list, path, file, publico);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteContactos;
            return File(fullPath, Constantes.AppExcel, ConstantesAdmin.ReporteContactos);
        }

        /// <summary>
        /// Muestra la vista de comités
        /// </summary>
        /// <returns></returns>        
        public ViewResult Comite()
        {
            return View();
        }

        /// <summary>
        /// Muestra la lista de comités
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ComiteList()
        {
            ComiteModel model = new ComiteModel();
            model.ListaComite = this.servicio.ListWbComites();
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar el comité
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ComiteEdit(int idComite)
        {
            ComiteModel model = new ComiteModel();

            if (idComite == 0)
            {
                model.Entidad = new WbComiteDTO();
                model.Entidad.Comitecodi = 0;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbComite(idComite);
            }

            return PartialView(model);
        }

        public PartialViewResult ComiteLista(int idComiteLista)
        {
            ComiteListaModel model = new ComiteListaModel();
            model.Entidad = new WbComiteListaDTO();

            if (idComiteLista == 0)
            {
                model.Entidad.Comitelistacodi = 0;
                model.Lista = new List<WbComiteListaDTO>();
            }
            else
            {
                model.Entidad.Comitecodi = idComiteLista;
                model.Lista = this.servicio.GetByIdWbComiteLista(idComiteLista);
            }

            return PartialView(model);
        }

        public JsonResult ListByComite(int idComiteLista)
        {
            List<WbComiteListaDTO> lista = this.servicio.GetByIdWbComiteLista(idComiteLista);
            SelectList lista_ = new SelectList(lista, EntidadComite.ComiteListaCodi, EntidadComite.ComiteListaName);
            return Json(lista);

        }

        /// <summary>
        /// Permite grabar los datos del comité
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ComiteSave(ComiteModel model)
        {
            try
            {
                if (model.Codigo == 0)
                {
                    WbComiteDTO entity = new WbComiteDTO
                    {
                        Comitecodi = model.Codigo,
                        Comitename = model.Nombre,
                        Comiteestado = "A",
                        Comiteusucreacion = User.Identity.Name,
                        Comiteusumodificacion = "",
                        Comitefeccreacion = DateTime.Now,
                        Comitefecmodificacion = null
                    };
                    this.servicio.SaveWbComite(entity);
                }
                else
                {
                    WbComiteDTO entity = new WbComiteDTO
                    {
                        Comitecodi = model.Codigo,
                        Comitename = model.Nombre,
                        Comiteestado = "A",
                        Comiteusucreacion = User.Identity.Name,
                        Comiteusumodificacion = User.Identity.Name,
                        Comitefeccreacion = DateTime.Now,
                        Comitefecmodificacion = DateTime.Now
                    };
                    this.servicio.UpdateWbComite(entity);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar el comité
        /// </summary>
        /// <param name="idComite"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ComiteDelete(int idComite)
        {
            try
            {
                this.servicio.DeleteWbComite(idComite);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Muestra la vista de procesos
        /// </summary>
        /// <returns></returns>        
        public ViewResult Proceso()
        {
            return View();
        }

        /// <summary>
        /// Muestra la lista de procesos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ProcesoList()
        {
            ProcesoModel model = new ProcesoModel();
            model.ListaProceso = this.servicio.ListWbProcesos();
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar el proceso
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ProcesoEdit(int idProceso)
        {
            ProcesoModel model = new ProcesoModel();

            if (idProceso == 0)
            {
                model.Entidad = new WbProcesoDTO();
                model.Entidad.Procesocodi = 0;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdWbProceso(idProceso);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del comité
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesoSave(ProcesoModel model)
        {
            try
            {
                if (model.Codigo == 0)
                {
                    WbProcesoDTO entity = new WbProcesoDTO
                    {
                        Procesocodi = model.Codigo,
                        Procesoname = model.Nombre,
                        Procesoestado = "A",
                        Procesousucreacion = User.Identity.Name,
                        Procesousumodificacion = "",
                        Procesofeccreacion = DateTime.Now,
                        Procesofecmodificacion = null
                    };
                    this.servicio.SaveWbProceso(entity);
                }
                else
                {
                    WbProcesoDTO entity = new WbProcesoDTO
                    {
                        Procesocodi = model.Codigo,
                        Procesoname = model.Nombre,
                        Procesoestado = "A",
                        Procesousucreacion = User.Identity.Name,
                        Procesousumodificacion = User.Identity.Name,
                        Procesofeccreacion = DateTime.Now,
                        Procesofecmodificacion = DateTime.Now
                    };
                    this.servicio.UpdateWbProceso(entity);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar el comité
        /// </summary>
        /// <param name="idProceso"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesoDelete(int idProceso)
        {
            try
            {
                this.servicio.DeleteWbProceso(idProceso);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        [HttpPost]
        public JsonResult ComiteListaSave(ComiteListaModel model)
        {
            try
            {
                WbComiteListaDTO entity = new WbComiteListaDTO
                {
                    Comitecodi = model.ComiteCodigo,
                    Comitelistaname = model.Nombre,
                    Comitelistaestado = "A",
                    Comitelistausucreacion = User.Identity.Name,
                    Comitelistafeccreacion = DateTime.Now
                };
                this.servicio.SaveWbComiteLista(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult ComiteListaDelete(int idComiteLista)
        {
            try
            {
                this.servicio.DeleteWbComiteLista(idComiteLista);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
