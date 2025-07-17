using System;
using System.Collections.Generic;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using log4net;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class FeriadosController : Controller
    {
        private readonly DemandaPOAppServicio demandaPoServicio = new DemandaPOAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FeriadosController));
        private static string NameController = "FeriadosController";
        public ActionResult Index()
        {
            FeriadoModel model = new FeriadoModel();
            model.Fecha = DateTime.Today.Year.ToString();

            return View(model);
        }

        public JsonResult ListaFeriados(string fechaAnio) 
        {
            FeriadoModel model = new FeriadoModel();

            try{
                int anio = Int32.Parse(fechaAnio);
                model.ListaFeriados = demandaPoServicio.GetByAnioDpoFeriados(anio);
                model.Mensaje = "Todo correcto";
                model.Resultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
                return Json(model);

            }
        }

        public JsonResult AgregarFeriados(string fechaAnio, string fecha, string descripcion, string spl, string sco)
        {
            FeriadoModel model = new FeriadoModel();
            
            try {
                List<DpoFeriadosDTO> listaFeriadosRepetidos;
                listaFeriadosRepetidos = demandaPoServicio.GetByFechaDpoFeriados(fecha);
                int anioLista = Int32.Parse(fechaAnio);
                int anio = Int32.Parse(fecha.Substring(6));

                if (listaFeriadosRepetidos.Count() == 0) {
                    if (spl == "S" || sco == "S")
                    {
                        DateTime fecFeriado = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                        DpoFeriadosDTO dpoFeriadosDTO = new DpoFeriadosDTO { Dpoferanio = anio, Dpoferdescripcion = descripcion.TrimEnd(), Dpoferfecha = fecFeriado, Dpoferspl = spl, Dpofersco = sco, Dpoferfeccreacion = DateTime.Now, Dpoferusucreacion = User.Identity.Name };
                        demandaPoServicio.SaveDpoFeriados(dpoFeriadosDTO);
                        model.Mensaje = "El feriado con fecha " + fecha + " fue agregado correctamente";
                        model.Resultado = "1";
                    }
                    else {
                        model.Mensaje = "Se debe seleccionar por lo menos un usuario";
                        model.Resultado = "-2";
                    }

                }
                else {
                    model.Mensaje = "La fecha seleccionada ya posee un feriado registrado";
                    model.Resultado = "-2";
                }

                model.ListaFeriados = demandaPoServicio.GetByAnioDpoFeriados(anioLista);

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
                return Json(model);
            }
        }

        public JsonResult EliminarFeriados(int dpofercodi, string fechaAnio)
        {
            FeriadoModel model = new FeriadoModel();

            try {
                demandaPoServicio.DeleteDpoFeriados(dpofercodi);

                int anioLista = Int32.Parse(fechaAnio);
                model.ListaFeriados = demandaPoServicio.GetByAnioDpoFeriados(anioLista);
                model.Mensaje = "Feriado eliminado con exito";
                model.Resultado = "1";

                return Json(model);
            }
            catch (Exception ex) {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
                return Json(model);
            }
        }

        public JsonResult EditarFeriados(int dpofercodi, 
            string descripcion, string spl, string sco,
            string fecha, string fechaAnio)
        {
            FeriadoModel model = new FeriadoModel();

            try
            {
                if (spl == "S" || sco == "S")
                {
                    demandaPoServicio.UpdateByIdDpoFeriados(dpofercodi,
                        descripcion, spl, sco, fecha);
                    model.Mensaje = "El feriado fue editado correctamente";
                    model.Resultado = "1";
                }
                else {
                    model.Mensaje = "Se debe seleccionar por lo menos un usuario";
                    model.Resultado = "-2";
                }

                int anioLista = Int32.Parse(fechaAnio);
                model.ListaFeriados = demandaPoServicio.GetByAnioDpoFeriados(anioLista);
                
                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
                return Json(model);
            }
        }

    }
}