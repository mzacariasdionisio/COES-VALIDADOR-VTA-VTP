using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.Servicios.Aplicacion.Hidrologia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.FormatoMedicion;


namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class CambioEnvioController : Controller
    {
        //
        // GET: /Hidrologia/CambioEnvio/

        private GeneralAppServicio logicGeneral;
        private HidrologiaAppServicio logic;
        public CambioEnvioController()
        {
            logic = new HidrologiaAppServicio();
            logicGeneral = new GeneralAppServicio();
        }

        /// <summary>
        /// Index de inicio de controller CambioEnvio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CambioEnvioModel model = new CambioEnvioModel();
            int idOrigen = ConstantesHidrologia.IdOrigenHidro;
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.Fecha = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);           
            model.ListaFormato = logic.ListMeFormatos().Where(x => x.Modcodi == ConstantesHidrologia.IdModulo).ToList(); //lista de todos los formatos para hidrologia
            model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen).ToList();                      

            List<int> listaIdFormato = new List<int>();
            List<int> listaFormatoPeriodo = new List<int>();
            foreach (var reg in model.ListaFormato)
            {
                listaIdFormato.Add(reg.Formatcodi);
                listaFormatoPeriodo.Add((int)reg.Formatperiodo);
            }
            model.StrFormatCodi = String.Join(",", listaIdFormato);
            model.StrFormatPeriodo = String.Join(",", listaFormatoPeriodo);


            model.ListaSemanas = GetListaSemana(DateTime.Now.Year);
            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana= EPDate.f_numerosemana(DateTime.Now);
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            return View(model);
        }

        /// <summary>
        /// Lista los formatos de acuerdo a la lectura seleccionada
        /// </summary>
        /// <param name="idLectura"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFormatosXLectura(string sLectura)
        {
            HidrologiaModel model = new HidrologiaModel();
            string sEmpresa = "-1";
            var entitys = logic.GetByModuloLecturaMeFormatosMultiple(ConstantesHidrologia.IdModulo, sLectura, sEmpresa);
            SelectList lista = new SelectList(entitys, "Formatcodi", "Formatnombre");
            return Json(lista);
        }

        /// <summary>
        /// Lista de Cambios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="fecha"></param>
        /// <param name="mes"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string idsEmpresa, int idFormato,string fecha,string mes,string semana)
        {
            CambioEnvioModel model = new CambioEnvioModel();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.Now;

            //if (fecha != null)
            //{
            //    fechaInicial = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //}

            var formato = logic.GetByIdMeFormato(idFormato);
            fechaInicial = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
            var lista = logic.GetByCriteriaMeCambioenvios(idsEmpresa, fechaInicial, idFormato, 0);
            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    model.Columnas = 96;
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    model.Columnas = 48;
                    break;
                case ParametrosFormato.ResolucionHora:
                    model.Columnas = 24;
                    break;
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                case ParametrosFormato.ResolucionMes:
                    model.Columnas = 1;
                    break;
            }

            model.Resolucion = (int)formato.Formatresolucion;
            model.ListaCambioEnvio = lista;
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
            List<TipoInformacion> entitys = GetListaSemana(Int32.Parse(idAnho));
            SelectList list = new SelectList(entitys, "IdTipoInfo", "NombreTipoInfo");
            return Json(list);
        }

        /// <summary>
        /// Devuelve Lista de semanas
        /// </summary>
        /// <param name="anho"></param>
        /// <returns></returns>
        private List<TipoInformacion> GetListaSemana(int anho)
        {
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(anho, 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);
            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + anho;
                entitys.Add(reg);

            }
            return entitys;
        }

    }
}
