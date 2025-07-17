using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Hidrologia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class AmpliacionController : Controller
    {
        //
        // GET: /Hidrologia/Ampliacion/
        private HidrologiaAppServicio logic;
        private GeneralAppServicio logicGeneral;
        public AmpliacionController()
        {
            logic = new HidrologiaAppServicio();
            logicGeneral = new GeneralAppServicio();
        }

        /// <summary>
        /// Index de inicio de controller Ampliacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HidrologiaModel model = new HidrologiaModel();

            int idModulo = ConstantesHidrologia.IdModulo;
            int idOrigen = ConstantesHidrologia.IdOrigenHidro;

            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.ListaFormato = logic.ListMeFormatos().Where(x => x.Modcodi == idModulo).ToList(); //lista de todos los formatos para hidrologia
            model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen).ToList();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            foreach (var reg in model.ListaFormato)
            {
                listaFormatCodi.Add(reg.Formatcodi);
                listaFormatPeriodo.Add((int)reg.Formatperiodo);
            }
            model.Anho = DateTime.Now.Year.ToString();
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);

            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de todas las ampliaciones de plazo.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresa, string fecha, string sFormato)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fecha != null)
            {
                fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaIni.AddDays(1);

            }

            var lista = logic.ObtenerListaMultipleMeAmpliacionfechas(fechaIni, fechaFin, sEmpresa, sFormato);
            model.ListaAmpliacion = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el model para pintar el popup de ingreso de la nueva ampliacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarAmpliacion()
        {

            BusquedaModel model = new BusquedaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            int idOrigen = ConstantesHidrologia.IdOrigenHidro;
            int idModulo = ConstantesHidrologia.IdModulo;
            model.ListaFormato = logic.GetByModuloLecturaMeFormatos(idModulo, -1, -1);
            model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen).ToList();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaPlazo = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.HoraPlazo = DateTime.Now.Hour * 2 + 1;
            model.AnhoMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaSemanas = HelperHidrologia.GetListaSemana(DateTime.Now.Year);
            model.NroSemana = EPDate.f_numerosemana(DateTime.Now);
            return PartialView(model);
        }

        /// <summary>
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarValidacion(string fecha, int hora, int empresa, int idFormato, string semana, string mes)
        {
            int resultado = 1;
            DateTime fechaEnvio = DateTime.Now;

            //if (fecha != null)
            //{
            //    fechaEnvio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //}
            MeFormatoDTO formato = logic.GetByIdMeFormato(idFormato);
            fechaEnvio = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);

            MeAmpliacionfechaDTO ampliacion = new MeAmpliacionfechaDTO();
            ampliacion.Lastuser = User.Identity.Name;
            ampliacion.Lastdate = DateTime.Now;
            ampliacion.Amplifecha = fechaEnvio;
            ampliacion.Formatcodi = idFormato;
            ampliacion.Emprcodi = empresa;
            ampliacion.Amplifechaplazo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMinutes(hora * 30);
            if (hora == 48)
                ampliacion.Amplifechaplazo = ampliacion.Amplifechaplazo.AddSeconds(-1);
            try
            {
                var reg = logic.GetByIdMeAmpliacionfecha(fechaEnvio, empresa, idFormato);
                if (reg == null)
                {
                    ampliacion.Formatcodi = idFormato;
                    logic.SaveMeAmpliacionfecha(ampliacion);
                }
                else
                {
                    logic.UpdateMeAmpliacionfecha(ampliacion);
                }

            }
            catch
            {
                resultado = 0;
            }
            return Json(resultado);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarFormatosXLectura(string sLectura, string sEmpresa)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaFormato = logic.GetByModuloLecturaMeFormatosMultiple(ConstantesHidrologia.IdModulo, sLectura, sEmpresa);
            return PartialView(model);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarFormatosXLectura2(int idLectura, int idEmpresa)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaFormato = logic.GetByModuloLecturaMeFormatos(ConstantesHidrologia.IdModulo, idLectura, idEmpresa);
            return PartialView(model);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarSemanas(string idAnho)
        {
            BusquedaModel model = new BusquedaModel();
            List<TipoInformacion> entitys = HelperHidrologia.GetListaSemana(Int32.Parse(idAnho));
            SelectList list = new SelectList(entitys, "IdTipoInfo", "NombreTipoInfo");
            return Json(list);
        }

    }
}
