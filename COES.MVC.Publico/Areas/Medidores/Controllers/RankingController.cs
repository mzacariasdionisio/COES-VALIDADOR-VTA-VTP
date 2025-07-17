using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.Medidores.Helpers;
using COES.MVC.Publico.Areas.Medidores.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Medidores.Controllers
{
    public class RankingController : Controller
    {
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        RankingConsolidadoAppServicio servicio = new RankingConsolidadoAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();

        /// <summary>
        /// Valor de la máxima demanda mensual
        /// </summary>
        public decimal MaximaDemandaMensual
        {
            get
            {
                return (Session[DatosSesion.ValorMaximaDemanda] != null) ?
                    (decimal)Session[DatosSesion.ValorMaximaDemanda] : 0;
            }
            set { Session[DatosSesion.ValorMaximaDemanda] = value; }
        }

        /// <summary>
        /// Fecha de la máxima demanda
        /// </summary>
        public string FechaMaximaDemanda
        {
            get
            {
                return (Session[DatosSesion.FechaMaximaDemanda] != null) ?
                    Session[DatosSesion.FechaMaximaDemanda].ToString() : string.Empty;
            }
            set { Session[DatosSesion.FechaMaximaDemanda] = value; }
        }

        /// <summary>
        /// Lista ordenada tras ejecutar consulta
        /// </summary>
        public List<DemandadiaDTO> ListaOrdenamiento
        {
            get
            {
                return (Session[DatosSesion.ListaOrdenadaDemanda] != null) ?
                    (List<DemandadiaDTO>)Session[DatosSesion.ListaOrdenadaDemanda] : new List<DemandadiaDTO>();
            }
            set
            {
                Session[DatosSesion.ListaOrdenadaDemanda] = value;
            }
        }

        /// <summary>
        /// Almacena el indice de la máxima demanda
        /// </summary>
        public int HoraMaximaDemanda
        {
            get
            {
                return (Session[DatosSesion.HoraMaximaDemanda] != null) ?
                     (int)Session[DatosSesion.HoraMaximaDemanda] : 0;
            }
            set { Session[DatosSesion.HoraMaximaDemanda] = value; }
        }

        /// <summary>
        /// Muestra la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RankingModel model = new RankingModel();
            model.FechaConsulta = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
            model.FechaActual = DateTime.Now.ToString("yyyy-MM");
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();

            return View(model);
        }

        private List<Normativa> ListarNormativaMaximaDemanda()
        {
            List<Normativa> lista = new List<Normativa>();

            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            var listaRangoPeriodoHP = this.servParametro.GetListaParametroRangoPeriodoHP(listaParam, null).Where(x => x.Estado == ConstantesParametro.EstadoActivo || x.Estado == ConstantesParametro.EstadoBaja).ToList();
            //listaRangoPeriodoHP = listaRangoPeriodoHP.OrderBy(x => x.FechaInicio).ToList();

            foreach (var reg in listaRangoPeriodoHP)
            {
                if (reg.Normativa.Length > 0)
                {
                    int pos = reg.Normativa.IndexOf(":");

                    Normativa n = new Normativa();
                    n.DescripcionFull = reg.Normativa;
                    if (pos != -1)
                    {
                        n.Nombre = (reg.Normativa.Substring(0, pos + 1)).Trim();
                        n.Descripcion = (reg.Normativa.Substring(pos + 1, reg.Normativa.Length - pos - 1)).Trim();
                    }
                    else
                    {
                        n.Nombre = string.Empty;
                        n.Descripcion = reg.Normativa;
                    }

                    lista.Add(n);
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite cargar las empresas pora los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            RankingModel model = new RankingModel();
            model.ListaEmpresas = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);

            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la consulta en base a los criterios de búsqueda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Consulta(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            RankingModel model = new RankingModel();

            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, fecha, string.Empty, string.Empty, string.Empty);
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            if (string.IsNullOrEmpty(empresas)) empresas = ConstantesMedicion.IdEmpresaTodos.ToString();
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = ConstantesMedicion.IdTipoGeneracionTodos.ToString();

            ///
            bool esPortal = false; //User.Identity.Name.Length == 0;
            int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

            DemandadiaDTO resultado = new DemandadiaDTO();
            List<DemandadiaDTO> listOrdenado = new List<DemandadiaDTO>();

            List<DemandadiaDTO> lista = this.servicio.ObtenerDemandaDiariaHFPHP(fechaIni, fechaFin, tiposEmpresa, empresas,
                tiposGeneracion, central, out resultado, out listOrdenado, estadoValidacion, fechaProceso).OrderBy(x => x.Medifecha).ToList();

            model.ListaDemandaDia = lista;
            model.MaximaDemanda = resultado.ValorMD;
            model.FechaMD = resultado.FechaMD;
            model.HoraMD = resultado.HoraMD;
            this.MaximaDemandaMensual = resultado.ValorMD;
            this.ListaOrdenamiento = listOrdenado;
            this.FechaMaximaDemanda = resultado.FechaMD;
            this.HoraMaximaDemanda = resultado.IndexHoraMD;

            List<decimal> listHFP = lista.Select(x => x.ValorHFP).ToList();
            List<decimal> listHP = lista.Select(x => x.ValorHP).ToList();


            if (listHFP.Count > 0 && listHP.Count > 0)
            {
                var indexHFP = listHFP.IndexOf(listHFP.Max());
                var indexHP = listHP.IndexOf(listHP.Max());
                model.IndexHFP = indexHFP;
                model.IndexHP = indexHP;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener la lista ordenada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Ordenamiento(string fecha)
        {
            RankingModel model = new RankingModel();
            string[] subs = fecha.Split('-');
            int mes = Int32.Parse(subs[1]);
            int anio = Int32.Parse(subs[0]);
            DateTime fechaInicio = new DateTime(anio, mes, 1);
            DateTime fechaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1).AddDays(-1);
            List<DemandadiaDTO> lista = this.ListaOrdenamiento;
            int nroDias = DateTime.DaysInMonth(anio, mes);
            decimal valor = (decimal)(lista.Sum(x => x.ValorGeneracion) / 4);
            decimal fc = (this.MaximaDemandaMensual != 0) ? valor / (this.MaximaDemandaMensual * 24 * nroDias) : 0;

            model.ListaDemandaDia = lista;
            model.ListaDemandaDiaGeneracion = lista.OrderByDescending(x => x.ValorGeneracion).ToList();
            model.ProduccionEnergia = valor;
            model.FactorCarga = fc;
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite obtener los datos para los gráficos
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Evolucion(string tiposEmpresa, string empresas, string tiposGeneracion, int central)
        {
            DateTime fecha = DateTime.ParseExact(this.FechaMaximaDemanda, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            // Linea para obtener las validaciones
            this.ConsultaValidacion(fecha, ref tiposEmpresa, ref empresas);

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            List<MeMedicion96DTO> list = this.servicio.ObtenerDatosEvolucion(fecha, fecha, tiposEmpresa, empresas, tiposGeneracion, central);

            List<SerieMedicionEvolucion> series = new List<SerieMedicionEvolucion>();

            foreach (MeMedicion96DTO item in list)
            {
                SerieMedicionEvolucion serie = new SerieMedicionEvolucion();
                List<decimal> valores = new List<decimal>();
                for (int i = 1; i <= 96; i++)
                {
                    var valor = item.GetType().GetProperty(Constantes.CaracterH + i.ToString()).GetValue(item, null);
                    valores.Add((decimal)valor);
                }

                serie.SerieName = item.Fenergnomb;
                serie.ListaValores = valores;
                serie.SerieColor = item.Fenercolor;
                series.Add(serie);
            }

            EntidadSerieMedicionEvolucion entidad = new EntidadSerieMedicionEvolucion();
            entidad.ListaSerie = series;
            entidad.IndiceMaximaDemanda = this.HoraMaximaDemanda;
            entidad.ValorMaximaDemanda = this.MaximaDemandaMensual;
            entidad.Titulo = Util.ObtenerNombreMes(fecha.Month) + " " + fecha.Year.ToString();
            return Json(entidad);
        }

        /// <summary>
        /// Permite obtener el grafico de digrama de carga de máximos y mínimos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        public JsonResult DiagramaCarga(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion, int central)
        {
            int mes;
            int anho;

            DateTime fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            if (fecha != null)
            {
                string[] subs = fecha.Split('-');
                mes = Int32.Parse(subs[1]);
                anho = Int32.Parse(subs[0]);
                fechaInicio = new DateTime(anho, mes, 1);
                fechaFin = new DateTime(fechaInicio.Year, fechaInicio.Month, 1).AddMonths(1).AddDays(-1);
            }

            // Linea para obtener las validaciones
            this.ConsultaValidacion(fechaInicio, ref tiposEmpresa, ref empresas);

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            List<MeMedicion96DTO> list = this.servicio.ObtenerDatosMaximaMinimaAcumuladada(fechaFin, fechaFin, tiposEmpresa, empresas,
                tiposGeneracion, central);

            MeMedicion96DTO maximo = list.Where(x => x.Indicador == ConstantesMedicion.IndicadorMaximo).FirstOrDefault();
            MeMedicion96DTO minimo = list.Where(x => x.Indicador == ConstantesMedicion.IndicadorMinimo).FirstOrDefault();
            EntidadSerieMedicionEvolucion series = new EntidadSerieMedicionEvolucion();

            if (maximo != null && minimo != null)
            {
                var valorMaximo = 0M;
                var valorMinimo = 0M;

                List<decimal> listMaximos = new List<decimal>();
                List<decimal> listMinimos = new List<decimal>();

                decimal maxValue = decimal.MinValue;
                decimal minValue = decimal.MaxValue;
                int horaMax = 0;
                int horaMin = 0;

                for (int i = 1; i <= 96; i++)
                {
                    valorMaximo = (decimal)maximo.GetType().GetProperty(Constantes.CaracterH + i.ToString()).GetValue(maximo, null);
                    valorMinimo = (decimal)minimo.GetType().GetProperty(Constantes.CaracterH + i.ToString()).GetValue(minimo, null);

                    listMaximos.Add(valorMaximo);
                    listMinimos.Add(valorMinimo);

                    if (valorMaximo > maxValue)
                    {
                        maxValue = valorMaximo;
                        horaMax = i;
                    }
                    if (valorMinimo < minValue)
                    {
                        minValue = valorMinimo;
                        horaMin = i;
                    }
                }

                SerieMedicionEvolucion serieMaximo = new SerieMedicionEvolucion { ListaValores = listMaximos, SerieColor = "#1F497D", SerieName = "MÁXIMA DEMANDA - " + ((DateTime)maximo.Medifecha).ToString(Constantes.FormatoFecha) };
                SerieMedicionEvolucion serieMinimo = new SerieMedicionEvolucion { ListaValores = listMinimos, SerieColor = "#9BBB59", SerieName = "MÍNIMA DEMANDA - " + ((DateTime)minimo.Medifecha).ToString(Constantes.FormatoFecha) };

                List<SerieMedicionEvolucion> listaSeries = new List<SerieMedicionEvolucion>();
                listaSeries.Add(serieMaximo);
                listaSeries.Add(serieMinimo);
                series.ListaSerie = listaSeries;

                series.IndiceMaximaDemanda = horaMax - 1;
                series.IndiceMinimaDemanda = horaMin - 1;
                series.ListaSerie = listaSeries;
                series.ValorMaximaDemanda = maxValue;
                series.ValorMinimaDemanda = minValue;
                series.Titulo = Util.ObtenerNombreMes(fechaInicio.Month) + " " + fechaInicio.Year.ToString();
            }

            return Json(series);
        }

        /// <summary>
        /// Muestra el listado ordenado pero paginado
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Paginado(int index)
        {
            List<DemandadiaDTO> lista = this.ListaOrdenamiento;
            int lenPagina = 300;
            List<DemandadiaDTO> list = lista.Skip((index - 1) * lenPagina).Take(lenPagina).ToList();
            return Json(list);
        }

        /// <summary>
        /// Generar el formato excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fecha, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                #region Maxima demanda diaria
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, fecha, string.Empty, string.Empty, string.Empty);
                DateTime fechaInicio = fechaProceso;
                DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
                if (string.IsNullOrEmpty(empresas)) empresas = ConstantesMedicion.IdEmpresaTodos.ToString();
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = ConstantesMedicion.IdTipoGeneracionTodos.ToString();

                ///
                bool esPortal = false; //User.Identity.Name.Length == 0;
                int estadoValidacion = esPortal ? ConstanteValidacion.EstadoValidado : ConstanteValidacion.EstadoTodos;

                DemandadiaDTO resultado = new DemandadiaDTO();
                List<DemandadiaDTO> listOrdenado = new List<DemandadiaDTO>();

                List<DemandadiaDTO> lista = this.servicio.ObtenerDemandaDiariaHFPHP(fechaInicio, fechaFin, tiposEmpresa, empresas,
                    tiposGeneracion, central, out resultado, out listOrdenado, estadoValidacion, fechaProceso).OrderBy(x => x.Medifecha).ToList();

                List<decimal> listHFP = lista.Select(x => x.ValorHFP).ToList();
                List<decimal> listHP = lista.Select(x => x.ValorHP).ToList();

                if (listHFP.Count > 0 && listHP.Count > 0)
                {
                    var indexHFP = listHFP.IndexOf(listHFP.Max());
                    var indexHP = listHP.IndexOf(listHP.Max());
                    resultado.IndiceMDHFP = indexHFP;
                    resultado.IndiceMDHP = indexHP;
                }

                #endregion

                #region Valores ordenados

                int nroDias = DateTime.DaysInMonth(fechaProceso.Year, fechaProceso.Month);
                decimal produccionEnergia = (decimal)(listOrdenado.Sum(x => x.ValorGeneracion) / 4);
                decimal fc = (resultado.ValorMD != 0) ? produccionEnergia / (resultado.ValorMD * 24 * nroDias) : 0;

                #endregion

                #region Evolucion

                DateTime fechaConsulta = DateTime.ParseExact(resultado.FechaMD, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<MeMedicion96DTO> list = this.servicio.ObtenerDatosEvolucion(fechaConsulta, fechaConsulta, tiposEmpresa, empresas, tiposGeneracion, central);
                List<SerieMedicionEvolucion> series = new List<SerieMedicionEvolucion>();

                foreach (MeMedicion96DTO item in list)
                {
                    SerieMedicionEvolucion serie = new SerieMedicionEvolucion();
                    List<decimal> valores = new List<decimal>();
                    for (int i = 1; i <= 96; i++)
                    {
                        var valorEvolucion = item.GetType().GetProperty(Constantes.CaracterH + i.ToString()).GetValue(item, null);
                        valores.Add((decimal)valorEvolucion);
                    }

                    serie.SerieName = item.Fenergnomb;
                    serie.ListaValores = valores;
                    serie.SerieColor = item.Fenercolor;
                    series.Add(serie);
                }

                EntidadSerieMedicionEvolucion entidadEvolucion = new EntidadSerieMedicionEvolucion();
                entidadEvolucion.ListaSerie = series;
                entidadEvolucion.Titulo = Util.ObtenerNombreMes(fechaInicio.Month) + " " + fechaInicio.Year.ToString();
                entidadEvolucion.IndiceMaximaDemanda = resultado.IndexHoraMD;

                #endregion

                #region Diagrama de carga

                List<MeMedicion96DTO> listDiagrama = this.servicio.ObtenerDatosMaximaMinimaAcumuladada(fechaFin, fechaFin, tiposEmpresa, empresas,
                tiposGeneracion, central);

                MeMedicion96DTO maximo = listDiagrama.Where(x => x.Indicador == ConstantesMedicion.IndicadorMaximo).FirstOrDefault();
                MeMedicion96DTO minimo = listDiagrama.Where(x => x.Indicador == ConstantesMedicion.IndicadorMinimo).FirstOrDefault();

                var valorMaximo = 0M;
                var valorMinimo = 0M;

                List<decimal> listMaximos = new List<decimal>();
                List<decimal> listMinimos = new List<decimal>();

                decimal maxValue = decimal.MinValue;
                decimal minValue = decimal.MaxValue;
                int horaMax = 0;
                int horaMin = 0;

                for (int i = 1; i <= 96; i++)
                {
                    valorMaximo = (decimal)maximo.GetType().GetProperty(Constantes.CaracterH + i.ToString()).GetValue(maximo, null);
                    valorMinimo = (decimal)minimo.GetType().GetProperty(Constantes.CaracterH + i.ToString()).GetValue(minimo, null);

                    listMaximos.Add(valorMaximo);
                    listMinimos.Add(valorMinimo);

                    if (valorMaximo > maxValue)
                    {
                        maxValue = valorMaximo;
                        horaMax = i;
                    }
                    if (valorMinimo < minValue)
                    {
                        minValue = valorMinimo;
                        horaMin = i;
                    }
                }

                EntidadSerieMedicionEvolucion seriesDiagrama = new EntidadSerieMedicionEvolucion();
                SerieMedicionEvolucion serieMaximo = new SerieMedicionEvolucion
                {
                    ListaValores = listMaximos,
                    SerieColor = "#1F497D",
                    SerieName = "MÁXIMA DEMANDA - " +
                        ((DateTime)maximo.Medifecha).ToString(Constantes.FormatoFecha)
                };
                SerieMedicionEvolucion serieMinimo = new SerieMedicionEvolucion
                {
                    ListaValores = listMinimos,
                    SerieColor = "#9BBB59",
                    SerieName = "MÍNIMA DEMANDA - " +
                        ((DateTime)minimo.Medifecha).ToString(Constantes.FormatoFecha)
                };

                List<SerieMedicionEvolucion> listaSeries = new List<SerieMedicionEvolucion>();
                listaSeries.Add(serieMaximo);
                listaSeries.Add(serieMinimo);
                seriesDiagrama.ListaSerie = listaSeries;

                seriesDiagrama.IndiceMaximaDemanda = horaMax;
                seriesDiagrama.IndiceMinimaDemanda = horaMin;
                seriesDiagrama.ListaSerie = listaSeries;
                seriesDiagrama.ValorMaximaDemanda = maxValue;
                seriesDiagrama.ValorMinimaDemanda = minValue;
                seriesDiagrama.FechaMaximaDemanda = ((DateTime)maximo.Medifecha).ToString(Constantes.FormatoFecha);
                seriesDiagrama.FechaMinimaDemanda = ((DateTime)minimo.Medifecha).ToString(Constantes.FormatoFecha);
                seriesDiagrama.Titulo = Util.ObtenerNombreMes(fechaInicio.Month) + " " + fechaInicio.Year.ToString();


                #endregion

                MedidorHelper.GenerarReporteRankingDemanda(lista, resultado, listOrdenado, produccionEnergia, fc, entidadEvolucion, seriesDiagrama);

                return Json(1.ToString());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = NombreArchivo.ReporteRankingDemanda;
            string app = Constantes.AppExcel;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, app, file);
        }

        /// <summary>
        /// Valida los empresas que ya han pasado la validacion de datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        private void ConsultaValidacion(DateTime fecha, ref string tiposEmpresa, ref string empresas)
        {
            /// Codigo modificado
            #region Verificando plazos de validacion

            if (DateTime.Now.Year == fecha.Year && DateTime.Now.Month - 1 == fecha.Month)
            {
                List<int> idsTipoEmpresas = new List<int>();
                List<int> idsEmpresas = new List<int>();
                List<MdValidacionDTO> validacion = this.servicio.ObtenerValidacionMes(fecha);

                if (!string.IsNullOrEmpty(tiposEmpresa))
                {
                    idsTipoEmpresas = tiposEmpresa.Split(Constantes.CaracterComa).Select(int.Parse).ToList();
                }
                if (!string.IsNullOrEmpty(empresas))
                {
                    idsEmpresas = empresas.Split(Constantes.CaracterComa).Select(int.Parse).ToList();
                }

                if (idsTipoEmpresas.Count == 0)
                {
                    if (idsEmpresas.Count == 0)
                    {
                        empresas = string.Join<int>(Constantes.CaracterComa.ToString(), validacion.Select(x => x.Emprcodi));
                    }
                    else
                    {
                        List<int> subIdsEmpresas = idsEmpresas.Where(x => validacion.Any(y => y.Emprcodi == x)).Distinct().ToList();
                        if (subIdsEmpresas.Count == 0)
                        {
                            empresas = (-2).ToString();
                        }
                        else
                        {
                            empresas = string.Join<int>(Constantes.CaracterComa.ToString(), subIdsEmpresas);
                        }
                    }
                }
                else
                {
                    List<int> subIdsEmpresas = validacion.Where(x => idsTipoEmpresas.Any(y => y == x.Tipoemprcodi)).Select(x => x.Emprcodi).Distinct().ToList();

                    if (idsEmpresas.Count == 0)
                    {
                        if (subIdsEmpresas.Count == 0)
                        {
                            empresas = (-2).ToString();
                        }
                        else
                        {
                            empresas = string.Join<int>(Constantes.CaracterComa.ToString(), subIdsEmpresas);
                        }
                    }
                    else
                    {
                        List<int> subCodigosEmpresas = idsEmpresas.Where(x => subIdsEmpresas.Any(y => y == x)).Distinct().ToList();

                        if (subCodigosEmpresas.Count == 0)
                        {
                            empresas = (-2).ToString();
                        }
                        else
                        {
                            empresas = string.Join<int>(Constantes.CaracterComa.ToString(), subCodigosEmpresas);
                        }
                    }
                }
            }

            #endregion
        }
    }

}
