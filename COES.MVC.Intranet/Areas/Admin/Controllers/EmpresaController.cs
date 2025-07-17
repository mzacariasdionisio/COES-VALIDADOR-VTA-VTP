using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using log4net;
using System.Configuration;


namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class EmpresaController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de aplicación
        /// </summary>
        EmpresaAppServicio servicio = new EmpresaAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EmpresaController));

        public EmpresaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("EmpresaController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("EmpresaController", ex);
                throw;
            }
        }

        /// <summary>
        /// Pagina inicial de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string usuario, string opcion)
        {
            int? idOpcion = 0;
            if (string.IsNullOrEmpty(usuario))
            {
                if (!base.IsValidSesion) return base.RedirectToLogin();
                idOpcion = base.IdOpcion;
                if (idOpcion == null) idOpcion = -1;
            }
            else
            {
                SeguridadServicio.UserDTO entity = (new SeguridadServicio.SeguridadServicioClient()).ObtenerUsuarioPorLogin(usuario);
                if (entity != null)
                {
                    Session[DatosSesion.SesionUsuario] = entity;
                }
                else
                {
                    return base.RedirectToLoginDefault();
                }
                idOpcion = ConstantesAdmin.IdOpcionEmpresa;
            }

            EmpresaModel model = new EmpresaModel();
            model.ListaTipoEmpresa = this.servicio.ListarTipoEmpresas();

            model.Indicador = opcion;
            bool grabar = (new SeguridadServicio.SeguridadServicioClient()).ValidarPermisoOpcion(Constantes.IdAplicacion, (int)idOpcion,
                Acciones.Grabar, base.UserName);
            model.IndicadorGrabar = (grabar) ? Constantes.SI : Constantes.NO;

            return View(model);
        }

        /// <summary>
        /// Muesta el paginado
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="ruc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="estado"></param>
        /// <param name="empresaSein"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string nombre, string ruc, int idTipoEmpresa, string estado, string empresaSein)
        {
            Paginacion model = new Paginacion();

            if (string.IsNullOrEmpty(nombre)) nombre = (-1).ToString();
            if (string.IsNullOrEmpty(ruc)) ruc = (-1).ToString();

            int nroRegistros = servicio.ObtenerNroRegistrosBusqueda(nombre, ruc, idTipoEmpresa, estado, empresaSein);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }

        /// <summary>
        /// Muestra el listado de empresas
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="ruc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <param name="estado"></param>
        /// <param name="empresaSein"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string nombre, string ruc, int idTipoEmpresa, string estado, string empresaSein, int nroPagina, string indicadorGrabar, string indicador)
        {
            EmpresaModel model = new EmpresaModel();

            if (string.IsNullOrEmpty(nombre)) nombre = (-1).ToString();
            if (string.IsNullOrEmpty(ruc)) ruc = (-1).ToString();
            model.ListaEmpresa = this.servicio.BuscarEmpresas(nombre, ruc, idTipoEmpresa, empresaSein, estado, nroPagina, Constantes.PageSize);
            model.IndicadorGrabar = indicadorGrabar;
            model.Indicador = indicador;

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de un registro
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult Editar(int idEmpresa, string indicadorGrabar)
        {
            EmpresaModel model = new EmpresaModel();
            model.ListaTipoEmpresa = this.servicio.ListarTipoEmpresas().Where(x=>x.Tipoemprcodi > 0 && x.Tipoemprcodi <= 5).ToList();
            model.IndicadorGrabar = indicadorGrabar;
            model.IndicadorIntegrante = Constantes.NO;

            if (idEmpresa == 0)
            {
                model.Entidad = new SiEmpresaDTO();
                model.Entidad.Emprestado = Constantes.EstadoActivo;
                model.Entidad.Emprcodi = 0;
                model.Entidad.Emprdomiciliada = Constantes.NO;
                model.Entidad.TiposEmpresas = string.Empty;
            }
            else
            {
                model.Entidad = this.servicio.ObtenerEmpresa(idEmpresa);
                List<SiTipoComportamientoDTO> tipos = this.servicio.ObtenerTipoComportamiento(idEmpresa);
                List<String> listaIndicadorIntegrante = ConfigurationManager.AppSettings["ListaIndicadorIntegrante"].ToString().Split(';').ToList();

                //if (base.UserName == "amontalva" || base.UserName == "raul.castro" || base.UserName == "neiver.flores")


                if (listaIndicadorIntegrante.Contains(base.UserEmail))
                {
                    model.IndicadorIntegrante = Constantes.SI;
                }
                 //- Aplicamos lógica
                if (tipos.Count == 0)
                {
                    model.Entidad.TiposEmpresas = model.Entidad.Tipoemprcodi.ToString();
                }
                else
                {
                    model.Entidad.TiposEmpresas = string.Join(",", tipos.Select(x => x.Tipoemprcodi).ToList());

                    SiTipoComportamientoDTO tipoPrincipal = tipos.Where(x => x.Tipoprincipal == Constantes.SI).First();

                    if (tipoPrincipal != null)
                    {
                        model.Entidad.Tipoemprcodi = (int)tipoPrincipal.Tipoemprcodi;
                    }
                }

            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos de la empresa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(EmpresaModel model)
        {
            ResultadoMensaje result = new ResultadoMensaje();

            try
            {
                SiEmpresaDTO entity = new SiEmpresaDTO
                {
                    Emprabrev = model.Emprabrev,
                    Emprnomb = model.Emprnomb,
                    Tipoemprcodi = model.Tipoemprcodi,
                    Emprrazsocial = model.Emprrazsocial,
                    EmprCodOsinergmin = model.EmprCodOsinergmin,
                    Emprruc = model.Emprruc,
                    Emprsein = (model.Emprsein == null || model.Emprsein == "0") ? "N" : model.Emprsein,
                    Emprestado = model.Emprestado,
                    Inddemanda = model.Inddemanda,
                    Emprcodi = model.Emprcodi,
                    Lastuser = base.UserName,
                    Emprcoes = model.Emprcoes,
                    Emprdomiciliada = model.Emprdomiciliada,
                    Emprambito = model.Emprambito,
                    Emprrubro = model.Emprrubro,
                    Empragente = model.Empragente,
                    Lastdate = DateTime.Now,
                    Emprindproveedor = model.Emprindproveedor
                };


                //- Sección de código para almacenamiento de los tipos de comportamiento
                string strEmpresas = model.ListaEmpresas;
                entity.ListaTipoComportamiento = new List<SiTipoComportamientoDTO>();

                if (strEmpresas != null)
                {
                    string[] arrayEmpresas = strEmpresas.Split(ConstantesAppServicio.CaracterComa);

                    foreach(string strEmpresa in arrayEmpresas)
                    {
                        if (!string.IsNullOrEmpty(strEmpresa))
                        {
                            int idTipoComportamiento = int.Parse(strEmpresa);

                            SiTipoComportamientoDTO entityTipo = new SiTipoComportamientoDTO();
                            entityTipo.Tipoprincipal = (entity.Tipoemprcodi == idTipoComportamiento) ? Constantes.SI : Constantes.NO;
                            entityTipo.Tipoemprcodi = idTipoComportamiento;
                            entityTipo.Tipobaja = Constantes.NO;
                            entity.ListaTipoComportamiento.Add(entityTipo);
                        }
                    }
                }

                //- Fin de codigo para almacenamiento de los tipos de comportamiento

                int id = 0;
                string ruc = string.Empty;
                List<string> validaciones = new List<string>();
                bool flag = this.servicio.GrabarEmpresa(entity, out id, out validaciones, out ruc);

                if (flag)
                {
                    result.IdResultado = 1;
                    result.Id = id;
                    result.Ruc = ruc;
                }
                else
                {
                    result.IdResultado = 2;
                    result.Validaciones = validaciones.ToArray();
                    result.Ruc = ruc;
                }
            }
            catch (Exception ex)
            {
                log.Error("Grabar", ex);
                result.IdResultado = -1;
            }

            return Json(result);
        }


        [HttpPost]
        public JsonResult GrabarMME(EmpresaMMEModel model)
        {
            ResultadoMensaje result = new ResultadoMensaje();

            try
            {
                int id = 0;

                SiEmpresaMMEDTO entity = new SiEmpresaMMEDTO
                {
                    Emprmmecodi = model.Emprmmecodi,
                    Emprcodi = model.Emprcodi,
                    TipoEmprcodi = model.TipoEmprcodi,
                    Emprfecparticipacion = model.Emprfecparticipacion,
                    Emprfecretiro = model.Emprfecretiro,
                    Emprcomentario = model.Emprcomentario,
                    Emprusucreacion = base.UserName,
                    Emprfeccreacion = DateTime.Now,
                    Emprusumodificacion = base.UserName,
                    Emprfecmodificacion = DateTime.Now
                };

                bool flag = this.servicio.GrabarEmpresaMME(entity, out id);

                if (flag)
                {
                    result.IdResultado = 1;
                    result.Id = id;
                }
                else
                {
                    result.IdResultado = 2;
                }
            }
            catch (Exception ex)
            {
                log.Error("Grabar", ex);
                result.IdResultado = -1;
            }

            return Json(result);
        }


        /// <summary>
        /// Permite dar de baja a una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DarBaja(int idEmpresa)
        {
            try
            {
                SiEmpresaDTO entity = new SiEmpresaDTO
                {
                    Emprcodi = idEmpresa,
                    Emprestado = Constantes.EstadoBaja,
                    Lastuser = base.UserName,
                    Lastdate = DateTime.Now
                };

                this.servicio.ActualizarEstadoEmpresa(entity);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("DarBaja", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idEmpresa)
        {
            try
            {
                SiEmpresaDTO entity = new SiEmpresaDTO
                {
                    Emprcodi = idEmpresa,
                    Emprestado = Constantes.EstadoEliminado,
                    Lastuser = base.UserName,
                    Lastdate = DateTime.Now
                };

                this.servicio.ActualizarEstadoEmpresa(entity);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("Eliminar", ex);
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
        public JsonResult Exportar(string nombre, string ruc, int idTipoEmpresa, string estado, string empresaSein)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin;
                string file = ConstantesAdmin.ReporteEmpresas;
                if (string.IsNullOrEmpty(nombre)) nombre = (-1).ToString();
                if (string.IsNullOrEmpty(ruc)) ruc = (-1).ToString();
                List<SiEmpresaDTO> list = this.servicio.ExportarEmpresas(nombre, ruc, idTipoEmpresa, empresaSein, estado);
                ReporteHelper.GenerarReporteEmpresa(list, path, file);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("Exportar", ex);
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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionAdmin + ConstantesAdmin.ReporteEmpresas;
            return File(fullPath, Constantes.AppExcel, ConstantesAdmin.ReporteEmpresas);
        }

        /// <summary>
        /// Permite obtener la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEmpresa(int idEmpresa)
        {
            try
            {
                SiEmpresaDTO empr = this.servicio.ObtenerEmpresa(idEmpresa);

                if (idEmpresa == 0)
                {
                    empr.Emprdomiciliada = Constantes.NO;
                }
           

                return Json(empr);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener los datos de SUNAT
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatos(string ruc)
        {
            try
            {
                BeanEmpresa empresa = this.servicio.ConsultarPorRUC(ruc);

                if (empresa == null)
                {
                    return Json(-2); //- RUC no Existe            
                }
                else
                {
                    if (string.IsNullOrEmpty(empresa.RUC))
                    {
                        return Json(-2); //- RUC no Existe            
                    }
                    else
                    {
                        return Json(empresa);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(-1); //- Error en el proceso
            }
        }


        public ActionResult Asociacion()
        {
            return View();
        }

        #region VIGENCIA DE EMPRESAS

        /// <summary>
        /// Index de Vigencia de Empresas
        /// </summary>
        /// <returns></returns>
        public ActionResult Vigencia()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            EmpresaModel model = new EmpresaModel();
            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaEmpresa = this.servicio.ListarIdNameEmpresasActivasBajas();

            return View(model);
        }

        /// <summary>
        /// Listado historico de estados de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaEstadoHistoricoEmpresa(int idEmpresa, string strfecha)
        {
            EmpresaModel model = new EmpresaModel();

            DateTime fecha = DateTime.ParseExact(strfecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            model.ListaSiEmpresadat = this.servicio.ListarEstadoEmpresaHistorico(idEmpresa.ToString(), fecha);
            SiEmpresadatDTO obj = model.ListaSiEmpresadat.FirstOrDefault();
            model.Mensaje = obj != null ? "La empresa se encuentra en estado <span class='estado_empresa_" + obj.Emprestado + "'>" + obj.EmprestadoDesc + "</span> a la fecha de " + strfecha
                : "La empresa no tiene configuración de Vigencia";

            return PartialView(model);
        }

        #endregion

        #region EMPRESA MME

        public ActionResult IndexMME(string usuario, string opcion)
        {
            int? idOpcion = 0;
            if (string.IsNullOrEmpty(usuario))
            {
                if (!base.IsValidSesion) return base.RedirectToLogin();
                idOpcion = base.IdOpcion;
                if (idOpcion == null) idOpcion = -1;
            }
            else
            {
                SeguridadServicio.UserDTO entity = (new SeguridadServicio.SeguridadServicioClient()).ObtenerUsuarioPorLogin(usuario);
                if (entity != null)
                {
                    Session[DatosSesion.SesionUsuario] = entity;
                }
                else
                {
                    return base.RedirectToLoginDefault();
                }
                idOpcion = ConstantesAdmin.IdOpcionEmpresa;
            }

            EmpresaMMEModel model = new EmpresaMMEModel();

            model.Indicador = opcion;
            bool grabar = (new SeguridadServicio.SeguridadServicioClient()).ValidarPermisoOpcion(Constantes.IdAplicacion, (int)idOpcion,
                Acciones.Grabar, base.UserName);
            model.IndicadorGrabar = (grabar) ? Constantes.SI : Constantes.NO;

            return View(model);
        }

        /// <summary>
        /// Muestra el listado de empresas MME
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="ruc"></param>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoMME(string nombre, string ruc, int idTipoEmpresa, int nroPagina, string indicadorGrabar)
        {
            EmpresaMMEModel model = new EmpresaMMEModel();

            if (string.IsNullOrEmpty(nombre)) nombre = (-1).ToString();
            if (string.IsNullOrEmpty(ruc)) ruc = (-1).ToString();
            model.ListaEmpresaMME = this.servicio.BuscarEmpresasMME(idTipoEmpresa);
            model.IndicadorGrabar = indicadorGrabar;
            //model.Indicador = indicador;

            return PartialView(model);
        }
        public PartialViewResult EditarMME(int EmprMME, int Emprcodi, string indicadorGrabar, int tipo)
        {
            EmpresaMMEModel model = new EmpresaMMEModel();
            model.IndicadorGrabar = indicadorGrabar;
            model.ListaEmpresa = this.servicio.BuscarEmpresasporParticipacion(tipo);

            if (EmprMME == 0)
            {
                model.Entidad = new SiEmpresaMMEDTO();
                model.Entidad.Emprestado = Constantes.EstadoActivo;
                model.Entidad.Emprcodi = 0;
            }
            else
            {
                model.Entidad = this.servicio.ObtenerEmpresaMME(Emprcodi);
            }

            return PartialView(model);
        }

        public PartialViewResult HistorialMME(int Emprcodi)
        {
            EmpresaMMEModel model = new EmpresaMMEModel();

            model.ListaHistorialMME = this.servicio.BuscarHistorialEmpresasMME(Emprcodi);

            return PartialView(model);
        }
               
        #endregion

    }
}
