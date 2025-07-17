using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class VolumenController : BaseController
    {
        /// <summary>
        /// Instancias de las clases servicio
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();       

        /// <summary>
        /// Permite listar los registros de los volúmenes de insensibilidad
        /// </summary>       
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult Index()
        {
            VolumenModel model = new VolumenModel();            
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite obtener datos de los volumenes de insensibilidad
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Consultar(string fecha)
        {
            VolumenModel model = new VolumenModel();
            DateTime fechaInicio =  DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);            
            model.ListaEmbalses = this.servicio.ObtenerEmbalsesYUPANA();
            List<CmVolumenInsensibilidadDTO> listaVolumen = this.servicio.GetByCriteriaCmVolumenInsensibilidads(fechaInicio);
            int registrosTotal = 7 + (listaVolumen.Count == 0 ? 1 : listaVolumen.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 6);
            int indice = 0;

            foreach (var item in listaVolumen)
            {
                data[indice][0] = item.Volinscodi.ToString();
                data[indice][1] = item.Recurcodi.ToString();
                data[indice][2] = item.Volinsvolmin.ToString();
                data[indice][3] = item.Volinsvolmax.ToString();
                data[indice][4] = ((DateTime)item.Volinsinicio).ToString(Constantes.FormatoHoraMinuto);
                data[indice][5] = ((DateTime)item.Volinsfin).ToString(Constantes.FormatoHoraMinuto);

                indice++;
            }

            model.Data = data;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar las lineas
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarVolumen(string[][] data, string fecha)
        {
            DateTime fechaDatos = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            return Json(this.servicio.SaveCmVolumenInsensibilidad(data, fechaDatos, base.UserName));
        }


        /// <summary>
        /// Permite eliminar un registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servicio.DeleteCmVolumenInsensibilidad(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
