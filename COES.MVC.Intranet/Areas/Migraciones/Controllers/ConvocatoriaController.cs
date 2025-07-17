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
    public class ConvocatoriaController : BaseController
    {
        MigracionesAppServicio servicio = new MigracionesAppServicio();
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            ConvocatoriaModel model = new ConvocatoriaModel();   
            
            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite listar todos los Convocatorias que pertenecen a sala de prensa
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarConvocatorias(int typ)
        {
            ConvocatoriaModel model = new ConvocatoriaModel();
            List<WbConvocatoriasDTO> lista = new List<WbConvocatoriasDTO>();

            lista = servicio.GetListaConvocatorias().OrderByDescending(x => x.Convcodi).ToList();

            var ruta = Url.Content("~/");
            model.Resultado = servicio.ListaConvocatoriasHtml(lista, ruta, typ);
            model.nRegistros = lista.Count();

            return Json(model);
        }

        /// <summary>
        /// Update Orden Convocatorias
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrdenConvocatorias(int id, int fromPosition, int toPosition, string direction)
        {
            List<WbConvocatoriasDTO> lista = servicio.GetListaConvocatorias().Where(x => x.Convestado != "X").ToList();

            if (direction == "back")
            {
                int orden = toPosition;
                List<WbConvocatoriasDTO> ltmp = new List<WbConvocatoriasDTO>();
                ltmp.Add(lista[fromPosition - 1]);
                ltmp.AddRange(lista.GetRange(toPosition - 1, fromPosition - toPosition));
                foreach (var reg in ltmp)
                {
                    this.servicio.ActualizarWbConvocatorias(reg); //Actualizar el orden
                    orden++;
                }
            }
            else
            {
                int orden = fromPosition;
                List<WbConvocatoriasDTO> ltmp = new List<WbConvocatoriasDTO>();
                ltmp.AddRange(lista.GetRange(fromPosition, toPosition - fromPosition));
                ltmp.Add(lista[fromPosition - 1]);
                foreach (var reg in ltmp)
                {
                    this.servicio.ActualizarWbConvocatorias(reg); //Actualizar el orden
                    orden++;
                }
            }
        }

        /// <summary>
        /// Permite editar un Convocatoria
        /// </summary>
        /// <param name="comcodi"></param>
        /// <param name="evnto"></param>
        /// <returns></returns>
        public JsonResult ProcesoEditConvocatoria(int convcodi)
        {
            ConvocatoriaModel model = new ConvocatoriaModel();

            try
            {
                var lista = servicio.GetListaConvocatorias().Where(x => x.Convcodi == convcodi).ToList();

                foreach (var d in lista)
                {

                        model.Wbconvocatorias = new WbConvocatoriasDTO();
                        model.Wbconvocatorias.Convcodi = convcodi;
                        model.Wbconvocatorias.Convabrev = d.Convabrev;
                        model.Wbconvocatorias.Convnomb = d.Convnomb;
                        model.Wbconvocatorias.Convdesc = d.Convdesc;
                        model.Wbconvocatorias.Convfechaini = d.Convfechaini;
                        model.Wbconvocatorias.Convfechafin = d.Convfechafin;
                        model.Wbconvocatorias.Convestado = d.Convestado;
                        model.Wbconvocatorias.ConvfechainiDesc = d.Convfechaini.Value.ToString(ConstantesAppServicio.FormatoFecha);
                        model.Wbconvocatorias.ConvfechafinDesc = d.Convfechafin.Value.ToString(ConstantesAppServicio.FormatoFecha);
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
        /// Elimina un Convocatoria
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteConvocatoria(int convcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                servicio.EliminarWbConvocatorias(convcodi);
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Insertar nuevo Convocatoria
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveConvocatoria(ConvocatoriaModel model)
        {
            try
            {
                string est_ = model.Estado;
                var verif = servicio.GetListaConvocatorias().Find(x => x.Convcodi == model.Codigo);

                if (verif != null) { est_ = verif.Convestado; }

                if (model.Codigo == 0)
                {
                    int idNuevoConvocatoria = servicio.InsertarWbConvocatorias(new WbConvocatoriasDTO()
                    {
                        Convabrev = model.Abreviatura,
                        Convnomb = model.Nombre,
                        Convdesc = model.Descripcion,
                        Convlink = null,
                        Convfechaini = DateTime.ParseExact(model.FechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Convfechafin = DateTime.ParseExact(model.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Convestado = est_,
                        Usercreacion = User.Identity.Name,
                    });
                }
                else
                {                    

                    servicio.ActualizarWbConvocatorias(new WbConvocatoriasDTO()
                    {
                        Convabrev = model.Abreviatura,
                        Convnomb = model.Nombre,
                        Convdesc = model.Descripcion,
                        Convlink = null,
                        Convfechaini = DateTime.ParseExact(model.FechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Convfechafin = DateTime.ParseExact(model.FechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Convestado = est_,
                        Lastuser = User.Identity.Name,
                        Convcodi = model.Codigo,
                    });
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
