using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Models;
using COES.Servicios.Aplicacion.Recomendacion;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Auditoria;
using COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Helper;
using COES.Framework.Base.Tools;
using System.Collections;
using COES.Servicios.Aplicacion.Evento;
using System.IO;
using log4net;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Controllers
{
    public class ComentarioController : BaseController
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ComentarioController));
        public ComentarioController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ComentarioController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ComentarioController", ex);
                throw;
            }
        }

                
        SeguimientoRecomendacionAppServicio servSegRecomendacion = new SeguimientoRecomendacionAppServicio();
        AuditoriaAppServicio servAuditoria = new AuditoriaAppServicio();
        EventosAppServicio servEmpresa = new EventosAppServicio();


        /// <summary>
        /// Maneja el codigo de recomendacion
        /// </summary>
        public string SesionIdRecCom
        {
            get
            {
                return (Session[ConstanteSeguimientoRecomendacion.SesionIdRecom] != null) ?
                   Session[ConstanteSeguimientoRecomendacion.SesionIdRecom].ToString() : "";
            }
            set { Session[ConstanteSeguimientoRecomendacion.SesionIdRecom] = value; }
        }


        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            BusquedaSrmComentarioModel model = new BusquedaSrmComentarioModel();
            model.ListaSrmRecomendacion = new List<SrmRecomendacionDTO>();
            model.ListaFwUser = servAuditoria.ListUserRol(ConstanteSeguimientoRecomendacion.RolAseguramiento);
            model.ListaSiEmpresa = new List<SiEmpresaDTO>(); 
            model.FechaIni = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.AccionNuevo = true;// base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);

            return View(model);
        }


        /// <summary>
        /// Permite a edición del comentario
        /// </summary>
        /// <param name="id">id de comentario</param>
        /// <param name="accion">accion</param>
        /// <param name="srmreccodi">código de recomendación</param>
        /// <returns></returns>
        public ActionResult Editar(int id, int accion, int srmreccodi)
        {
            
            SrmComentarioModel model = new SrmComentarioModel();
            model.ListaSrmRecomendacion = new List<SrmRecomendacionDTO>();
            model.ListaFwUser = servAuditoria.ListUserRol(ConstanteSeguimientoRecomendacion.RolAseguramiento);
            model.ListaSiEmpresa = servEmpresa.ListarEmpresas();
            SrmComentarioDTO srmComentario =null;

            if (id != 0)
                srmComentario = servSegRecomendacion.GetByIdSrmComentario(id);

            if (srmComentario != null)
            {
                srmComentario.Fechacomentario = (srmComentario.Srmcomfechacoment != null) ? ((DateTime)srmComentario.Srmcomfechacoment).ToString(Constantes.FormatoFecha) : "";
                srmComentario.Fechacreacion = (srmComentario.Srmcomfeccreacion != null) ? ((DateTime)srmComentario.Srmcomfeccreacion).ToString(Constantes.FormatoFechaFull) : "";
                srmComentario.Fechamodificacion = (srmComentario.Srmcomfecmodificacion != null) ? ((DateTime)srmComentario.Srmcomfecmodificacion).ToString(Constantes.FormatoFechaFull) : "";
                model.SrmComentario = srmComentario;
            }
            else
            {
                srmComentario = new SrmComentarioDTO();

                srmComentario.Srmreccodi = srmreccodi;
                srmComentario.Usercode = Convert.ToInt32(Constantes.ParametroDefecto);
                srmComentario.Emprcodi = Convert.ToInt32(Constantes.ParametroDefecto);
                //srmComentario.Srmcomfechacoment = Convert.ToDateTime(DateTime.Now.ToString(Constantes.FormatoFecha));
                //srmComentario.Srmcomfeccreacion = Convert.ToDateTime(DateTime.Now.ToString(Constantes.FormatoFechaFull));
                srmComentario.Fechacomentario = DateTime.Now.ToString(Constantes.FormatoFecha);
                srmComentario.Fechacreacion = DateTime.Now.ToString(Constantes.FormatoFecha);


                model.SrmComentario = srmComentario;

            }

            model.Accion = accion;
            model.IndicadorGrabar = true;// base.VerificarAccesoAccion(Acciones.Nuevo, base.UserName);


            return View(model);
            
        }


        /// <summary>
        /// Permite eliminar un registro
        /// </summary>
        /// <param name="id">id de comentario</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                servSegRecomendacion.DeleteSrmComentario(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar un registro
        /// </summary>
        /// <param name="model">model tipo comentario</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(SrmComentarioModel model)
        {
            try
            {

                SrmComentarioDTO entity = new SrmComentarioDTO();

                entity.Srmcomcodi = model.SrmcomCodi;
                entity.Srmreccodi = model.Srmreccodi;

                if (model.SrmcomGruporespons == "C")
                {
                    entity.Usercode = model.Usercode;
                }
                else
                {
                    entity.Emprcodi = model.EmprCodi;
                }

                entity.Srmcomfechacoment = DateTime.ParseExact(model.SrmcomFechacoment, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                entity.Srmcomgruporespons = model.SrmcomGruporespons;
                entity.Srmcomcomentario = model.SrmcomComentario;
                

                if (model.SrmcomActivo == null)
                {
                    entity.Srmcomactivo = "S";
                }
                else
                {
                    entity.Srmcomactivo = model.SrmcomActivo;
                }


                //---
                if (entity.Srmcomcodi == 0)
                {
                    entity.Srmcomusucreacion = User.Identity.Name;
                    entity.Srmcomfeccreacion = DateTime.Now;
                }
                else
                {

                    if (model.SrmcomUsucreacion != null)
                    {
                        entity.Srmcomusucreacion = model.SrmcomUsucreacion;
                    }

                    if (model.SrmcomFeccreacion != null)
                    {
                        entity.Srmcomfeccreacion = DateTime.ParseExact(model.SrmcomFeccreacion, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    }

                    entity.Srmcomusumodificacion = User.Identity.Name;
                    entity.Srmcomfecmodificacion = DateTime.Now; 

                }
                //---


                int id = this.servSegRecomendacion.SaveSrmComentarioId(entity);

                SesionIdRecCom = ((int)entity.Srmreccodi).ToString() + '\\' + id;
                return Json(id);
                
            }
            catch
            {
                SesionIdRecCom = "-1";
                return Json(-1);
            }
        }


        //manejo de archivos
        /// <summary>
        /// Permite cargar los documentos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(int chunks, int chunk, string name)
        {
            try
            {

                string srmreccodi = SesionIdRecCom;
                string pathBaseSeguimiento = RutaBaseArchivos();

                FileServer.CreateFolder(pathBaseSeguimiento, srmreccodi.ToString(), null);

                pathBaseSeguimiento += srmreccodi;

                if (Request.Files.Count > 0)
                {
                    if (chunks > 1)
                    {
                        var file = Request.Files[0];
                        string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
                        using (var fs = new FileStream(Path.Combine(path, name), chunk == 0 ? FileMode.Create : FileMode.Append))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        if (chunk == chunks - 1)
                        {
                            System.IO.FileInfo info = new FileInfo(path + name);
                            FileServer.UploadFromFileDirectory(path + name, base.PathFiles, name, string.Empty);
                        }
                    }
                    else
                    {
                        var file = Request.Files[0];
                        FileServer.UploadFromStream(file.InputStream, pathBaseSeguimiento + "\\", file.FileName, string.Empty);
                    }
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Permite listar los documentos
        /// </summary>
        /// <param name="srmreccodi">códio de recomendacion</param>
        /// <param name="activo">activo</param>
        /// <param name="nroPage">nro. página</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int srmreccodi, string activo, int nroPage)
        {
            BusquedaSrmComentarioModel model = new BusquedaSrmComentarioModel();


            model.ListaSrmComentario = servSegRecomendacion.BuscarOperaciones(srmreccodi, activo, nroPage, ConstanteSeguimientoRecomendacion.PageSizeComentario).ToList();

            Hashtable htFiles = new Hashtable();

            foreach(SrmComentarioDTO item in  model.ListaSrmComentario)
            {
                int srmcomcodi = item.Srmcomcodi;

                List<FileData> listaFileCom = ListaArchivos(srmreccodi, srmcomcodi);

                htFiles.Add(srmcomcodi, listaFileCom);


            }

            model.htFiles = htFiles;


            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        /// <summary>
        /// Permite obtener la ruta base de los archivos
        /// </summary>
        /// <returns></returns>
        public string RutaBaseArchivos()
        {

            string LocalDirectory = base.PathFiles;//ConstanteSeguimientoRecomendacion.PathSeguimientoRecomendaciones;
            //string LocalDirectory = ConstanteSeguimientoRecomendacion.PathSeguimientoRecomendaciones;

            return LocalDirectory;
        }


        /// <summary>
        /// Permite obtener el listado de archivos
        /// </summary>
        /// <param name="srmreccodi">código de recomendación</param>
        /// <param name="srmcomcodi">código de comentario</param>
        /// <returns></returns>
        public List<FileData> ListaArchivos(int srmreccodi,int srmcomcodi)
        {
            //FileModel model = new FileModel();
            List<FileData> lista;
            if (srmreccodi != 0)
            {

                string pathBaseSeguimiento = RutaBaseArchivos() + srmreccodi + "\\" + srmcomcodi + "\\";

                //if (FileServer.VerificarExistenciaDirectorio(pathBaseSeguimiento))
                //{
                    lista = FileServer.ListarArhivos(pathBaseSeguimiento, string.Empty);
                //}
                //else
                //{
                //    lista = new List<FileData>();
                //}
            }
            else
            {
                lista = new List<FileData>();
            }

            return lista;
        }


        /// <summary>
        /// Permite realizar el paginado
        /// </summary>
        /// <param name="srmreccodi">código de recomendación</param>
        /// <param name="activo">activo</param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoCom(int srmreccodi, string activo)
        {
            Paginacion model = new Paginacion();


            int nroRegistros = servSegRecomendacion.ObtenerNroFilas(srmreccodi, activo);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = ConstanteSeguimientoRecomendacion.PageSizeComentario;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }

        /// <summary>
        /// Permite activar/desactivar un comentario
        /// </summary>
        /// <param name="id">id de comentario</param>
        /// <param name="activo">activo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Activar(int id, string activo)
        {
            try
            {
                SrmComentarioDTO coment = servSegRecomendacion.GetByIdSrmComentario(id);
                coment.Srmcomactivo = activo;
                coment.Srmcomfecmodificacion = DateTime.Now;
                coment.Srmcomusumodificacion = User.Identity.Name;
                servSegRecomendacion.SaveSrmComentarioId(coment);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
