using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Medidores.Helpers;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class DuracionCargaController : Controller
    {
        // <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        DuracionCargaAppServicio servicio = new DuracionCargaAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DuracionCargaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Almacena la lista de datos
        /// </summary>
        public List<MeMedicion96DTO> ListaDatos
        {
            get
            {
                return (Session[DatosSesion.ListaDiagramaCarga] != null) ?
                    (List<MeMedicion96DTO>)Session[DatosSesion.ListaDiagramaCarga] : new List<MeMedicion96DTO>();
            }
            set { Session[DatosSesion.ListaDiagramaCarga] = value; }
        }

        /// <summary>
        /// Permite almacenar los datos mensuales
        /// </summary>
        public List<SerieDuracionCarga> ListaSerie
        {
            get
            {
                return (Session[DatosSesion.ListaSerieDiagramaCarga] != null) ?
                    (List<SerieDuracionCarga>)Session[DatosSesion.ListaSerieDiagramaCarga] : new List<SerieDuracionCarga>();
            }
            set { Session[DatosSesion.ListaSerieDiagramaCarga] = value; }
        }

        /// <summary>
        /// Almacena el año en que está realizándose la consulta
        /// </summary>
        public int NroDias
        {
            get
            {
                return (Session[DatosSesion.FechaProceso] != null) ?
                    (int)Session[DatosSesion.FechaProceso] : DateTime.Now.Year;
            }
            set { Session[DatosSesion.FechaProceso] = value; }
        }

        /// <summary>
        /// Muestra la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DuracionCargaModel model = new DuracionCargaModel();
            model.FechaDesde = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaHasta = DateTime.Now.ToString(Constantes.FormatoFecha);
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
            listaRangoPeriodoHP = listaRangoPeriodoHP.OrderBy(x => x.FechaInicio).ToList();

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
            DuracionCargaModel model = new DuracionCargaModel();
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
        public JsonResult Consulta(string fechaDesde, string fechaHasta, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaDesde))
                {
                    fechaInicio = DateTime.ParseExact(fechaDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaHasta))
                {
                    fechaFin = DateTime.ParseExact(fechaHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                this.NroDias = ((int)fechaFin.Subtract(fechaInicio).TotalDays + 1);

                if (this.NroDias < 93)
                {

                    List<MeMedicion96DTO> list = this.servicio.ObtenerDiagramaCarga(fechaInicio, fechaFin, tiposEmpresa, empresas,
                        tiposGeneracion, central);
                    this.ListaDatos = list;
                    List<ListaMesesRE> meses = new List<ListaMesesRE>();
                    meses.Add(new ListaMesesRE { Nombre = TextosPantalla.Todos, Valor = string.Empty });
                    int nroMeses = this.ObtenerNumeroMeses(fechaInicio, fechaFin);

                    for (int i = 0; i <= nroMeses; i++)
                    {
                        DateTime fecha = fechaInicio.AddMonths(i);

                        if (i == 0)
                        {
                            if (fecha.Day == 1)
                            {
                                meses.Add(new ListaMesesRE
                                {
                                    Nombre = Tools.ObtenerNombreMes(fecha.Month) + " " + fecha.Year,
                                    Valor = fecha.Year + "_" + fecha.Month
                                });
                            }
                        }
                        else if (i == nroMeses)
                        {
                            int nroDias = Tools.ObtenerNroDias(fecha.Year, fecha.Month);

                            if (fecha.Year == fechaFin.Year && fecha.Month == fechaFin.Month && fechaFin.Day == nroDias)
                            {
                                meses.Add(new ListaMesesRE
                                {
                                    Nombre = Tools.ObtenerNombreMes(fecha.Month) + " " + fecha.Year,
                                    Valor = fecha.Year + "_" + fecha.Month
                                });
                            }
                        }
                        else
                        {
                            meses.Add(new ListaMesesRE
                            {
                                Nombre = Tools.ObtenerNombreMes(fecha.Month) + " " + fecha.Year,
                                Valor = fecha.Year + "_" + fecha.Month
                            });
                        }
                    }

                    if (list.Count > 0)
                    {
                        return Json(meses);
                    }
                    else
                    {
                        return Json((-3).ToString());
                    }

                }
                else
                {
                    return Json((-2).ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json((-1).ToString());
            }
        }

        /// <summary>
        /// Permite obtener el numero de mes
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public int ObtenerNumeroMeses(DateTime date1, DateTime date2)
        {
            if (date1.Month < date2.Month)
            {
                return (date2.Year - date1.Year) * 12 + date2.Month - date1.Month;
            }
            else
            {
                return (date2.Year - date1.Year - 1) * 12 + date2.Month - date1.Month + 12;
            }
        }

        /// <summary>
        /// Permite obtener el listado paginado del mes seleccionado
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosPorMes(string indicador, string fechaInicio, string fechaFin, string texto)
        {
            try
            {
                int mes = -1;
                int anio = 0;
                string titulo = string.Empty;

                if (!string.IsNullOrEmpty(indicador))
                {
                    string[] cadena = indicador.Split(Constantes.CaracterRaya);
                    anio = int.Parse(cadena[0]);
                    mes = int.Parse(cadena[1]);
                }

                DuracionCargaModel model = new DuracionCargaModel();
                List<MeMedicion96DTO> list = new List<MeMedicion96DTO>();

                int dias = 0;
                if (mes == -1)
                {
                    dias = this.NroDias;
                    list = this.ListaDatos;
                    titulo = " " + fechaInicio + " - " + fechaFin;
                }
                else
                {
                    dias = Tools.ObtenerNroDias(anio, mes);
                    list = this.ListaDatos.Where(x => ((DateTime)x.Medifecha).Year == anio && ((DateTime)x.Medifecha).Month == mes).ToList();
                    titulo = " " + texto.ToUpper();
                }

                #region Obtenemos los datos estructurados

                List<DuracionCargaSerie> resultado = new List<DuracionCargaSerie>();

                for (int i = 0; i < dias; i++)
                {
                    DateTime fecha = ((DateTime)list[0].Medifecha).AddDays(i);
                    List<MeMedicion96DTO> subList = this.ListaDatos.Where(x => x.Medifecha == fecha).ToList();

                    List<DuracionCargaSerie> listaDia = this.ObtieneEstructuraSerieDuracion();

                    foreach (MeMedicion96DTO item in subList)
                    {
                        for (int j = 0; j <= 95; j++)
                        {
                            var valor = item.GetType().GetProperty(Constantes.CaracterH + (j + 1)).GetValue(item, null);
                            #region Categoriacion del valor

                            switch (item.Fenergcodi)
                            {
                                case DuracionCargaSerie.IdAgua:
                                    {
                                        listaDia[j].Agua = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdBagazo:
                                    {
                                        listaDia[j].Bagazo = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdBiogas:
                                    {
                                        listaDia[j].Biogas = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdCarbon:
                                    {
                                        listaDia[j].Carbon = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdDiesel:
                                    {
                                        listaDia[j].Diesel = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdEolica:
                                    {
                                        listaDia[j].Eolica = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdGas:
                                    {
                                        listaDia[j].Gas = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdR500:
                                    {
                                        listaDia[j].R500 = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdR6:
                                    {
                                        listaDia[j].R6 = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdResidual:
                                    {
                                        listaDia[j].Residual = (decimal)valor;
                                        break;
                                    }
                                case DuracionCargaSerie.IdSolar:
                                    {
                                        listaDia[j].Solar = (decimal)valor;
                                        break;
                                    }
                            }
                            #endregion
                        }
                    }

                    resultado.AddRange(listaDia);
                }

                #endregion

                decimal total = 0;
                foreach (DuracionCargaSerie item in resultado)
                {
                    item.Total = item.Agua + item.Bagazo + item.Biogas + item.Carbon + item.Diesel + item.Eolica +
                        item.Gas + item.R500 + item.R6 + item.Residual + item.Solar;

                    total = total + item.Total;
                }

                List<DuracionCargaSerie> ordenado = resultado.OrderByDescending(x => x.Total).ToList();

                decimal minimo = ordenado.Last().Total;
                decimal maximo = ordenado.First().Total;
                decimal gwh = 0;
                decimal fc = 0;

                gwh = total / 4000;

                if (maximo != 0)
                {
                    fc = total / (4 * maximo * 24 * dias);
                }

                List<SerieDuracionCarga> series = new List<SerieDuracionCarga>();

                #region Armado de la serie               

                SerieDuracionCarga itemAgua = new SerieDuracionCarga();
                itemAgua.SerieColor = DuracionCargaSerie.ColorAgua;
                itemAgua.SerieName = DuracionCargaSerie.NombreAgua;
                itemAgua.ListaValores = ordenado.Select(x => x.Agua).ToList();
                series.Add(itemAgua);

                SerieDuracionCarga itemSolar = new SerieDuracionCarga();
                itemSolar.SerieColor = DuracionCargaSerie.ColorSolar;
                itemSolar.SerieName = DuracionCargaSerie.NombreSolar;
                itemSolar.ListaValores = ordenado.Select(x => x.Solar).ToList();
                series.Add(itemSolar);

                SerieDuracionCarga itemBagazo = new SerieDuracionCarga();
                itemBagazo.SerieColor = DuracionCargaSerie.ColorBagazo;
                itemBagazo.SerieName = DuracionCargaSerie.NombreBagazo;
                itemBagazo.ListaValores = ordenado.Select(x => x.Bagazo).ToList();
                series.Add(itemBagazo);

                SerieDuracionCarga itemBiogas = new SerieDuracionCarga();
                itemBiogas.SerieColor = DuracionCargaSerie.ColorBiogas;
                itemBiogas.SerieName = DuracionCargaSerie.NombreBiogas;
                itemBiogas.ListaValores = ordenado.Select(x => x.Biogas).ToList();
                series.Add(itemBiogas);

                SerieDuracionCarga serieEolica = new SerieDuracionCarga();
                serieEolica.SerieColor = DuracionCargaSerie.ColorEolica;
                serieEolica.SerieName = DuracionCargaSerie.NombreEolica;
                serieEolica.ListaValores = ordenado.Select(x => x.Eolica).ToList();
                series.Add(serieEolica);

                SerieDuracionCarga serieGas = new SerieDuracionCarga();
                serieGas.SerieColor = DuracionCargaSerie.ColorGas;
                serieGas.SerieName = DuracionCargaSerie.NombreGas;
                serieGas.ListaValores = ordenado.Select(x => x.Gas).ToList();
                series.Add(serieGas);

                SerieDuracionCarga serieCarbon = new SerieDuracionCarga();
                serieCarbon.SerieColor = DuracionCargaSerie.ColorCarbon;
                serieCarbon.SerieName = DuracionCargaSerie.NombreCarbon;
                serieCarbon.ListaValores = ordenado.Select(x => x.Carbon).ToList();
                series.Add(serieCarbon);

                SerieDuracionCarga sereResidual = new SerieDuracionCarga();
                sereResidual.SerieColor = DuracionCargaSerie.ColorResidual;
                sereResidual.SerieName = DuracionCargaSerie.NombreResidual;
                sereResidual.ListaValores = ordenado.Select(x => x.Residual).ToList();
                series.Add(sereResidual);

                SerieDuracionCarga serieDiesel = new SerieDuracionCarga();
                serieDiesel.SerieColor = DuracionCargaSerie.ColorDiesel;
                serieDiesel.SerieName = DuracionCargaSerie.NombreDiesel;
                serieDiesel.ListaValores = ordenado.Select(x => x.Diesel).ToList();
                series.Add(serieDiesel);

                #endregion

                model.ListaGrafico = series;
                this.ListaSerie = series;
                model.Maximo = maximo;
                model.Minimo = minimo;
                model.Gwh = gwh;
                model.Fc = fc;
                model.Titulo = titulo;

                return Json(model);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener la estructura de la serie
        /// </summary>
        /// <returns></returns>
        private List<DuracionCargaSerie> ObtieneEstructuraSerieDuracion()
        {
            List<DuracionCargaSerie> list = new List<DuracionCargaSerie>();
            for (int i = 1; i <= 96; i++)
            {
                DuracionCargaSerie item = new DuracionCargaSerie();
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Obtiene el número de paginas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPaginas()
        {
            StringBuilder html = new StringBuilder();
            int pageSize = 300;

            if (this.ListaSerie.Count > 0)
            {
                int nroRegistros = this.ListaSerie[0].ListaValores.Count;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                html.Append("<ul>");
                for (int i = 1; i <= nroPaginas; i++)
                {
                    html.Append(string.Format("                    <li><a href='JavaScript:pintarPaginado({0});' class='page-item' id='page-item{0}'>{0}</a></li>", i));
                }
                html.Append("</ul>");
            }
            return Json(html.ToString());
        }


        /// <summary>
        /// Permite mostrar los datos paginados
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Paginado(int index)
        {
            try
            {
                List<SerieDuracionCarga> list = this.ListaSerie;
                List<SerieDuracionCarga> entitys = new List<SerieDuracionCarga>();
                int lenPagina = 300;

                foreach (SerieDuracionCarga serie in list)
                {
                    SerieDuracionCarga entity = new SerieDuracionCarga();
                    entity.SerieColor = serie.SerieColor;
                    entity.SerieName = serie.SerieName;
                    entity.ListaValores = serie.ListaValores.Skip((index - 1) * lenPagina).Take(lenPagina).ToList();
                    entitys.Add(entity);
                }

                #region Armando la tabla

                StringBuilder str = new StringBuilder();
                str.Append("<table class='tabla-formulario' style='width:100%' id='tablaOrdenamiento'>");
                str.Append("  <thead>");
                str.Append("     <tr>");
                str.Append("        <th>N° de Registos/MES</th>");

                foreach (SerieDuracionCarga serie in entitys)
                {
                    str.Append(String.Format("        <th>{0}</th>", serie.SerieName));
                }

                str.Append("     </tr>");
                str.Append("  </thead>");
                str.Append("  <tbody>");

                if (entitys.Count > 0)
                {
                    int contador = 300;
                    var count = (index - 1) * 300 + 1;
                    for (int i = 0; i < contador; i++)
                    {
                        str.Append("     <tr>");
                        str.Append(String.Format("        <td>{0}</td>", count));

                        foreach (SerieDuracionCarga serie in entitys)
                        {
                            if (serie.ListaValores.Count >= (i + 1))
                            {
                                str.Append(String.Format("        <td>{0}</td>", serie.ListaValores[i]));
                            }
                            else
                            {
                                str.Append("<td></td>");
                            }
                        }

                        str.Append("     </tr>");
                        count++;
                    }
                }

                str.Append("  </tbody>");
                str.Append("</table>");

                #endregion

                return Json(str.ToString());
            }
            catch
            {
                return Json(-1);
            }
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
        public JsonResult Exportar(string fechaDesde, string fechaHasta, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaDesde))
                {
                    fechaInicio = DateTime.ParseExact(fechaDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaHasta))
                {
                    fechaFin = DateTime.ParseExact(fechaHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                int dias = ((int)fechaFin.Subtract(fechaInicio).TotalDays + 1);

                if (dias <= 365)
                {
                    List<MeMedicion96DTO> list = this.servicio.ObtenerDiagramaCarga(fechaInicio, fechaFin, tiposEmpresa, empresas,
                   tiposGeneracion, central);

                    if (list.Count > 0)
                    {

                        #region Obtenemos los datos estructurados

                        List<DuracionCargaSerie> resultado = new List<DuracionCargaSerie>();

                        for (int i = 0; i < dias; i++)
                        {
                            DateTime fecha = ((DateTime)list[0].Medifecha).AddDays(i);
                            List<MeMedicion96DTO> subList = list.Where(x => x.Medifecha == fecha).ToList();

                            List<DuracionCargaSerie> listaDia = this.ObtieneEstructuraSerieDuracion();

                            foreach (MeMedicion96DTO item in subList)
                            {
                                for (int j = 0; j <= 95; j++)
                                {
                                    var valor = item.GetType().GetProperty(Constantes.CaracterH + (j + 1)).GetValue(item, null);
                                    #region Categoriacion del valor

                                    switch (item.Fenergcodi)
                                    {
                                        case DuracionCargaSerie.IdAgua:
                                            {
                                                listaDia[j].Agua = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdBagazo:
                                            {
                                                listaDia[j].Bagazo = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdBiogas:
                                            {
                                                listaDia[j].Biogas = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdCarbon:
                                            {
                                                listaDia[j].Carbon = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdDiesel:
                                            {
                                                listaDia[j].Diesel = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdEolica:
                                            {
                                                listaDia[j].Eolica = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdGas:
                                            {
                                                listaDia[j].Gas = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdR500:
                                            {
                                                listaDia[j].R500 = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdR6:
                                            {
                                                listaDia[j].R6 = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdResidual:
                                            {
                                                listaDia[j].Residual = (decimal)valor;
                                                break;
                                            }
                                        case DuracionCargaSerie.IdSolar:
                                            {
                                                listaDia[j].Solar = (decimal)valor;
                                                break;
                                            }
                                    }
                                    #endregion
                                }
                            }

                            resultado.AddRange(listaDia);
                        }

                        #endregion


                        decimal total = 0;
                        foreach (DuracionCargaSerie item in resultado)
                        {
                            item.Total = item.Agua + item.Bagazo + item.Biogas + item.Carbon + item.Diesel + item.Eolica +
                                item.Gas + item.R500 + item.R6 + item.Residual + item.Solar;

                            total = total + item.Total;
                        }

                        List<DuracionCargaSerie> ordenado = resultado.OrderByDescending(x => x.Total).ToList();

                        decimal minimo = ordenado.Last().Total;
                        decimal maximo = ordenado.First().Total;
                        decimal gwh = 0;
                        decimal fc = 0;

                        gwh = total / 4000;

                        if (maximo != 0)
                        {
                            fc = total / (4 * maximo * 24 * dias);
                        }

                        List<SerieDuracionCarga> series = new List<SerieDuracionCarga>();

                        #region Armado de la serie

                        SerieDuracionCarga itemAgua = new SerieDuracionCarga();
                        itemAgua.SerieColor = DuracionCargaSerie.ColorAgua;
                        itemAgua.SerieName = DuracionCargaSerie.NombreAgua;
                        itemAgua.ListaValores = ordenado.Select(x => x.Agua).ToList();
                        series.Add(itemAgua);

                        SerieDuracionCarga itemSolar = new SerieDuracionCarga();
                        itemSolar.SerieColor = DuracionCargaSerie.ColorSolar;
                        itemSolar.SerieName = DuracionCargaSerie.NombreSolar;
                        itemSolar.ListaValores = ordenado.Select(x => x.Solar).ToList();
                        series.Add(itemSolar);

                        SerieDuracionCarga itemBagazo = new SerieDuracionCarga();
                        itemBagazo.SerieColor = DuracionCargaSerie.ColorBagazo;
                        itemBagazo.SerieName = DuracionCargaSerie.NombreBagazo;
                        itemBagazo.ListaValores = ordenado.Select(x => x.Bagazo).ToList();
                        series.Add(itemBagazo);

                        SerieDuracionCarga itemBiogas = new SerieDuracionCarga();
                        itemBiogas.SerieColor = DuracionCargaSerie.ColorBiogas;
                        itemBiogas.SerieName = DuracionCargaSerie.NombreBiogas;
                        itemBiogas.ListaValores = ordenado.Select(x => x.Biogas).ToList();
                        series.Add(itemBiogas);

                        SerieDuracionCarga serieEolica = new SerieDuracionCarga();
                        serieEolica.SerieColor = DuracionCargaSerie.ColorEolica;
                        serieEolica.SerieName = DuracionCargaSerie.NombreEolica;
                        serieEolica.ListaValores = ordenado.Select(x => x.Eolica).ToList();
                        series.Add(serieEolica);

                        SerieDuracionCarga serieGas = new SerieDuracionCarga();
                        serieGas.SerieColor = DuracionCargaSerie.ColorGas;
                        serieGas.SerieName = DuracionCargaSerie.NombreGas;
                        serieGas.ListaValores = ordenado.Select(x => x.Gas).ToList();
                        series.Add(serieGas);

                        SerieDuracionCarga serieCarbon = new SerieDuracionCarga();
                        serieCarbon.SerieColor = DuracionCargaSerie.ColorCarbon;
                        serieCarbon.SerieName = DuracionCargaSerie.NombreCarbon;
                        serieCarbon.ListaValores = ordenado.Select(x => x.Carbon).ToList();
                        series.Add(serieCarbon);

                        SerieDuracionCarga sereResidual = new SerieDuracionCarga();
                        sereResidual.SerieColor = DuracionCargaSerie.ColorResidual;
                        sereResidual.SerieName = DuracionCargaSerie.NombreResidual;
                        sereResidual.ListaValores = ordenado.Select(x => x.Residual).ToList();
                        series.Add(sereResidual);

                        SerieDuracionCarga serieDiesel = new SerieDuracionCarga();
                        serieDiesel.SerieColor = DuracionCargaSerie.ColorDiesel;
                        serieDiesel.SerieName = DuracionCargaSerie.NombreDiesel;
                        serieDiesel.ListaValores = ordenado.Select(x => x.Diesel).ToList();
                        series.Add(serieDiesel);

                        #endregion



                        MedidorHelper.GenerarReporteDuracionCarga(series, fechaDesde, fechaHasta);

                        return Json(1);
                    }
                    else
                    {
                        return Json(-3);
                    }
                }
                else
                {
                    return Json(-2);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = NombreArchivo.ReporteDuracionCarga;
            string app = Constantes.AppExcel;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }
    }
}
