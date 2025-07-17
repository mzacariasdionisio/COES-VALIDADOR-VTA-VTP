using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using log4net;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Helper;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Migraciones;
using COES.MVC.Intranet.Helper;
using System.IO;
using System.Configuration;


namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class SalaPrensaController : BaseController
    {
        MigracionesAppServicio servicio = new MigracionesAppServicio();
        public ActionResult SalaPrensa()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            SalaPrensaModel model = new SalaPrensaModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite listar todos los comunicados que pertenecen a sala de prensa
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarComunicados(int typ)
        {
            SalaPrensaModel model = new SalaPrensaModel();
            List<WbComunicadosDTO> lista = new List<WbComunicadosDTO>();

                lista = servicio.GetListaComunicados().Where(x => x.Comtipo == "S").OrderByDescending(x => x.Comcodi).ToList();

            var ruta = Url.Content("~/");
            model.Resultado = servicio.ListaComunicadosSalaPrensaHtml(lista, ruta, typ);
            model.nRegistros = lista.Count();

            return Json(model);
        }


        /// <summary>
        /// Permite cargar la imagen temporalmente
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;
         
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + file.FileName;

                    Session["nombre"] = fileName;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Permite cargar la imagen definitiva
        /// </summary>
        /// <returns></returns>
        public void CargarImagen(int IdComunicado)
        {
            
            try
            {
                string Pathorigen = Session["nombre"] as string;
                string path = ConfigurationManager.AppSettings["RutaComunicados"].ToString();
                if (System.IO.File.Exists(Pathorigen))
                {

                string file = IdComunicado + ".jpg";

                string Pathdestino = path + file;
                if (System.IO.File.Exists(Pathdestino))
                    {
                        System.IO.File.Delete(Pathdestino);
                    }
                System.IO.File.Move(Pathorigen, Pathdestino);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// Update Orden Comunicados
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrdenComunicados(int id, int fromPosition, int toPosition, string direction)
        {
            List<WbComunicadosDTO> lista = servicio.GetListaComunicados().Where(x => x.Comestado != "X" && x.Composition == 1).OrderBy(x => x.Comorden).ToList();

            if (direction == "back")
            {
                int orden = toPosition;
                List<WbComunicadosDTO> ltmp = new List<WbComunicadosDTO>();
                ltmp.Add(lista[fromPosition - 1]);
                ltmp.AddRange(lista.GetRange(toPosition - 1, fromPosition - toPosition));
                foreach (var reg in ltmp)
                {
                    reg.Comorden = orden;
                    this.servicio.ActualizarWbComunicados(reg); //Actualizar el orden
                    orden++;
                }
            }
            else
            {
                int orden = fromPosition;
                List<WbComunicadosDTO> ltmp = new List<WbComunicadosDTO>();
                ltmp.AddRange(lista.GetRange(fromPosition, toPosition - fromPosition));
                ltmp.Add(lista[fromPosition - 1]);
                foreach (var reg in ltmp)
                {
                    reg.Comorden = orden;
                    this.servicio.ActualizarWbComunicados(reg); //Actualizar el orden
                    orden++;
                }
            }
        }

        /// <summary>
        /// Permite editar un comunicado
        /// </summary>
        /// <param name="comcodi"></param>
        /// <param name="evnto"></param>
        /// <returns></returns>
        public JsonResult ProcesoEditComunicado(int comcodi, int evnto, string pos)
        {
            SalaPrensaModel model = new SalaPrensaModel();

            try
            {
                var lista = servicio.GetListaComunicados().Where(x => x.Comcodi == comcodi).ToList();

                foreach (var d in lista)
                {
                    if (evnto == 1)
                    {
                        d.Composition = (pos == "" ? 1 : 0);
                        this.servicio.ActualizarWbComunicados(d); //Actualizar el orden
                    }
                    else
                    {
                        model.Wbcomunicados = new WbComunicadosDTO();
                        model.Wbcomunicados.Comcodi = comcodi;
                        model.Wbcomunicados.Comfecha = d.Comfecha;
                        model.Wbcomunicados.Comtitulo = d.Comtitulo;
                        model.Wbcomunicados.Comresumen = d.Comresumen;
                        model.Wbcomunicados.Comdesc = d.Comdesc;
                        model.Wbcomunicados.Comfechaini = d.Comfechaini;
                        model.Wbcomunicados.Comfechafin = d.Comfechafin;
                        model.Wbcomunicados.Comestado = d.Comestado;
                        model.Wbcomunicados.ComfechaDesc = d.Comfecha.Value.ToString(ConstantesAppServicio.FormatoFecha);
                        model.Wbcomunicados.ComfechainiDesc = d.Comfechaini.Value.ToString(ConstantesAppServicio.FormatoFecha);
                        model.Wbcomunicados.ComfechafinDesc = d.Comfechafin.Value.ToString(ConstantesAppServicio.FormatoFecha);

                        string path = ConfigurationManager.AppSettings["RutaComunicados"].ToString();
                        //byte[] imagen = System.IO.File.ReadAllBytes("D:\Fuentes\git\Framework_SalaPrensa\COES.MVC.Publico\Content\Images\Comunicados\" + comcodi + ".jpg");
                        string ruta = path+comcodi+".jpg";
                        byte[] imagen = null;
                        if (System.IO.File.Exists(ruta))
                        {
                            imagen = System.IO.File.ReadAllBytes(path + comcodi + ".jpg");
                        }
                    
                        
                        if (imagen != null)
                        {
                            string mimeType = "image/" + "jpg";
                            string base64 = Convert.ToBase64String(imagen);
                            model.Imagen = string.Format("data:{0};base64,{1}", mimeType, base64);
                        }
                        
                    }
                }

                model.nRegistros = 1;
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Elimina un comunicado
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteComunicado(int comcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                servicio.EliminarWbComunicados(comcodi);

                model.nRegistros = 1;
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Insertar nuevo comunicado
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveComunicado(SalaPrensaModel model)
        //public JsonResult SaveComunicado(string fecha, string titu, string descrip, string fecha1, string fecha2, string est, int evnto, int comcodi, string tipocomu)
        {
            //MigracionesModel model = new MigracionesModel();
            try
            {
                string est_ = model.Estado;
                int posit_ = 0;
                var verif = servicio.GetListaComunicados().Find(x => x.Comcodi == model.Codigo);

                if (verif != null) { est_ = verif.Comestado; posit_ = verif.Composition ?? 0; }

                if (model.Evento == 2)
                {
                    servicio.ActualizarWbComunicados(new WbComunicadosDTO()
                    {
                        Comcodi = model.Codigo,
                        Comfecha = DateTime.ParseExact(model.Fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comtitulo = model.Titulo,
                        Comresumen = model.Resumen,
                        Comdesc = model.Descripcion,
                        Comfechaini = DateTime.ParseExact(model.FechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comfechafin = DateTime.ParseExact(model.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comestado = est_,
                        Composition = posit_
                    });
                    CargarImagen(model.Codigo);
                }
                else
                {
                    int idNuevoComunicado=servicio.InsertarWbComunicados(new WbComunicadosDTO()
                    {
                        Comfecha = DateTime.ParseExact(model.Fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comtitulo = model.Titulo,
                        Comresumen = model.Resumen,
                        Comdesc = model.Descripcion,
                        Comfechaini = DateTime.ParseExact(model.FechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comfechafin = DateTime.ParseExact(model.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comestado = est_,
                        Comtipo = model.Tipo
                        
                    });
                    CargarImagen(idNuevoComunicado);
                }

                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }
    }
}
