using COES.MVC.Intranet.Areas.Sorteo.Models;
using COES.MVC.Intranet.Helper;
using System.Web.Mvc;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System;
using System.Globalization;
using log4net;
using System.Configuration;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Sorteo;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Linq;
using COES.MVC.Intranet.Areas.Eventos.Models;

namespace COES.MVC.Intranet.Areas.Sorteo.Controllers
{
    public class SorteoController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(SorteoController));

        public SorteoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("SorteoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("SorteoController", ex);
                throw;
            }
        }
        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        private SorteoAppServicio appSorteo = new SorteoAppServicio();
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        //
        // GET: /Sorteo/Listar/

        public ActionResult Index()
        {
            BusquedaPAleatoriasModel model = new BusquedaPAleatoriasModel();
            model.isBotonNuevoHabilitado = DateTime.Now.Hour >= 14 ? 1 : 0;
            return View(model);
        }

        /// <summary>
        /// Permite tomar un nuemero aleatorio de manera encriptada
        /// </summary>
        /// <returns></returns>
        public int Random(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                rngCsp.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            var rpta = (int)(min + (max - min) *
                        (scale / (double)uint.MaxValue));
            return rpta;
        }

        /// <summary>
        /// Permite pintar la lista de areas
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Listado()
        {
            ListarAreasModel ListarArea = new ListarAreasModel();
            ListarArea.ListarAreas = new List<SiListarAreasDTO>();
            var eq_equipo = this.appSorteo.GetByeq_equipo();
            var eq_central = this.appSorteo.GetByeq_central();
            var eve_mantto = this.appSorteo.GetByeve_mantto(); //new List<PrLogsorteoDTO> ()
            //var eve_indisponibilidad = this.appSorteo.GetByeve_indisponibilidad();
            var eve_horaoperacion = this.appSorteo.GetByeve_horaoperacion(); //new List<PrLogsorteoDTO> (); //this.appSorteo.GetByeve_horaoperacion();
            var eve_pruebaunidad = this.appSorteo.GetByeve_pruebaunidad();
            //var conteo = TotalConteoTipoLocal("X%", DateTime.Today.AddDays(1 - DateTime.Now.Day));
            int i_codigo = 0;
            int i_codigopadre = 0;

            int test = 0;

            foreach (var r in eve_pruebaunidad)
            {
                DateTime fechaPU = (DateTime)r.PRUNDFECHA;
                int li_grupocodiPrueba = Convert.ToInt32(r.GRUPOCODI);

                //eve_horaoperacion.RemoveAll(x => x.Subcausacodi == 114 && li_grupocodiPrueba == x.GRUPOCODI && (x.Hophorini.Day == fechaPU.Day || x.Hophorini.Day == fechaPU.AddDays(1).Day));

                var eve_horaoperacion_temp = new List<PrLogsorteoDTO>();
                eve_horaoperacion_temp.AddRange(eve_horaoperacion);

                foreach (var hora_ope in eve_horaoperacion_temp)
                {
                    /*
                    if (r.Emprnomb.Contains("SAMAY"))
                        test = 0;*/


                    DateTime fechaHO = (DateTime)hora_ope.Hophorini;
                    int li_grupocodiHO = Convert.ToInt32(hora_ope.GRUPOCODI);
                    int li_subcausacodi = Convert.ToInt32(hora_ope.Subcausacodi);  //114
                    /*
                    if (li_subcausacodi == 114 && li_grupocodiPrueba == li_grupocodiHO && (fechaHO.Day == fechaPU.Day || fechaHO.Day == fechaPU.AddDays(1).Day))
                    {
                        eve_horaoperacion.Remove(hora_ope);
                    }else if (li_subcausacodi == 0)
                    {
                        //eve_horaoperacion.Remove(hora_ope);
                    }*/
                }
            }

            foreach (var r in eq_equipo)
            {
                //if (r.Equicodi == 15215 || r.Equicodi == 15216 || r.Equicodi == 15217 || r.Equicodi == 15218)
                if (r.Equicodi == 15218)
                {

                }

                if (r.Equipadre != null)
                {
                    i_codigo = Convert.ToInt32(r.Equicodi);
                    i_codigopadre = Convert.ToInt32(r.Equipadre);
                    var foundRows1 = eve_horaoperacion.Where(d => d.Equicodi == i_codigo || d.Equicodi == i_codigopadre);
                    var foundRows2 = eve_mantto.Where(d => d.Equicodi == i_codigo || d.Equicodi == i_codigopadre);
                    //var foundRows3 = eve_indisponibilidad.Where(d => d.Equicodi == i_codigo || d.Equicodi == i_codigopadre);
                    var lb_valor = nf_get_bool_total_calderos_indisponibles(i_codigo);
                    if (foundRows1.Count() > 0 || foundRows2.Count() > 0 || lb_valor) //|| foundRows3.Count() > 0
                    {
                        if(r.Emprnomb.Contains("SHOUGESA"))
                            test = 0;
                    }
                    else
                    {

                        if (r.Emprnomb.Contains("SHOUGESA"))
                            test = 0;

                        var modelArea = new SiListarAreasDTO();
                        modelArea.equicodi = r.Equicodi;
                        modelArea.emprnomb = r.Emprnomb;
                        modelArea.areanomb = r.Areanomb;
                        modelArea.equiabrev = r.Equiabrev;
                        modelArea.equipadre = r.Equipadre;
                        ListarArea.ListarAreas.Add(modelArea);
                        /*
                        var modelArea = new SiListarAreasDTO();
                        modelArea.equicodi = r.Equicodi;
                        modelArea.emprnomb = r.Emprnomb;
                        modelArea.areanomb = r.Areanomb;
                        modelArea.equiabrev = r.Equiabrev;
                        modelArea.equipadre = r.Equipadre;
                        ListarArea.ListarAreas.Add(modelArea);
                        */
                    }
                }
            }
            return PartialView(ListarArea);
        }

        private bool nf_get_bool_total_calderos_indisponibles(int i_codigo)
        {
            bool lb_valor = true;

            var equipos_validos = this.appSorteo.GetByequipos_validos(i_codigo);
            if (equipos_validos == null || equipos_validos.Count == 0)
            {
                lb_valor = false;
                return lb_valor;
            }
            var eve_mantto_calderos = this.appSorteo.GetByeve_mantto_calderos(i_codigo, DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddHours(12));
            if (eve_mantto_calderos == null || eve_mantto_calderos.Count == 0)
            {
                lb_valor = false;
                return lb_valor;
            }
            return lb_valor;
        }

        public JsonResult sorteoUnidad(int contador)
        {
            var li_sorteo = 0;
            var result = "";
            //var descripcion = "";

            //var eq_equipo = this.appSorteo.GetByeq_equipo();           
            if (contador > 0)
            {
                li_sorteo = Random(1, contador);
                //var r = eq_equipo[li_sorteo];
                //var r = ListarArea.ListarAreas[li_sorteo];

                //codigo = Convert.ToInt32(r.equicodi);
                //descripcion = r.emprnomb + " - " + r.areanomb + " -> " + r.equiabrev;
                //result = descripcion;

            }
            else
            {
                result = "Equipos no disponibles para el sorteo!";
            }

            return Json(new { nroequiposorteado = li_sorteo, responseText = result }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Permite pintar la lista de historicos
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Historico()
        {

            DateTime fechaConsulta = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var DatosHistorico = this.appSorteo.GetByIdPrLogsorteo(fechaConsulta);

            HistoricoSorteoModel modelHistorico = new HistoricoSorteoModel();
            modelHistorico.ListarHistorico = DatosHistorico;
            return PartialView(modelHistorico);
        }

        [HttpPost]
        public JsonResult wf_log(string logdescrip, string logtipo, string coordinador, string docoes)
        {
            try
            {
                //se espera porque la pk de la tabla es date y se duplicaba
                System.Threading.Thread.Sleep(1000);

                UserDTO userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]);

                PrLogsorteoDTO LogSorteo = new PrLogsorteoDTO();
                LogSorteo.Logusuario = userLogin.UsernName;
                LogSorteo.Logfecha = DateTime.Now;
                LogSorteo.Logdescrip = logdescrip.Trim();
                LogSorteo.Logtipo = logtipo;
                LogSorteo.Logcoordinador = coordinador;
                LogSorteo.Logdocoes = docoes;
                appSorteo.InsertPrLogSorteo(LogSorteo);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        public int TotalConteoTipo(string tipo, string logfecha)
        {
            DateTime fecha = DateTime.Now;
            if (logfecha != null)
            {
                fecha = DateTime.ParseExact(logfecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            var TotalConteoTipo = this.appSorteo.TotalConteoTipo(tipo, fecha);

            return TotalConteoTipo;
        }

        public int TotalConteoTipoXEQ()
        {
            DateTime fecha = DateTime.Today.AddDays(1 - DateTime.Now.Day);
            var TotalConteoTipo = this.appSorteo.TotalConteoTipoXEQ(fecha);

            return TotalConteoTipo;
        }

        public int DiasFaltantes()
        {
            DateTime fecha = DateTime.Today.AddDays(1 - DateTime.Now.Day);
            var TotalConteoTipo = this.appSorteo.DiasFaltantes(fecha);

            return TotalConteoTipo;
        }

        public bool eliminarLogSorteo()
        {
            var eliminarLogSorteo = true;
            DateTime logfecha = DateTime.Now;
            eliminarLogSorteo = this.appSorteo.eliminarLogSorteo(logfecha);
            return eliminarLogSorteo;
        }

        [HttpPost]
        public JsonResult insertarSorteo(int equicodi, int codigo)
        {
            string prueba = "N";
            try
            {
                if (equicodi == codigo)
                {
                    prueba = "S";
                }
                else
                {
                    prueba = "N";
                }
                var fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var result = this.appSorteo.InsertPrSorteo(equicodi, fecha, prueba);

                return Json(1);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }
    }
}
