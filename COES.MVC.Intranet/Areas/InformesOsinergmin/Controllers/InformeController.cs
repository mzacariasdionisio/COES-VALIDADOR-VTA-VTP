using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.InformesOsinergmin.Helper;
using COES.MVC.Intranet.Areas.InformesOsinergmin.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Controllers
{
    public class InformeController : Controller
    {
        //
        // GET: /InformesOsinergmin/Informe/

        public ActionResult Index()
        {
            InformeModel model = new InformeModel();
            model.Fecha = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio);
            return View(model);
        }

        /// <summary>
        /// Permite obtener el reporte
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Detalle(string fecha)
        {
            InformeModel model = new InformeModel();
            DateTime fechaConsulta = new DateTime(int.Parse(fecha.Substring(3, 4)), int.Parse(fecha.Substring(0, 2)), 1);
            ResultadoIndSup resultado = (new IndiceSupervision()).ObtenerCuadros(fechaConsulta);
            model.Cuadro1 = resultado.Cuadro1;
            model.Cuadro2 = resultado.Cuadro2;
            model.Cuadro3 = resultado.Cuadro3;
            int nroDias = (int)fechaConsulta.AddMonths(1).Subtract(fechaConsulta).TotalDays;
            model.NroDias = nroDias;
            Session[ConstantesInformes.SesionDatos] = model;

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el comparativo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="diaInicio"></param>
        /// <param name="diaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Comparativo(string fecha, int diaInicio, int diaFin)
        {
            InformeModel resultado = new InformeModel();
            InformeModel model = (InformeModel)Session[ConstantesInformes.SesionDatos];
            DateTime fechaConsulta = new DateTime(int.Parse(fecha.Substring(3, 4)), int.Parse(fecha.Substring(0, 2)), 1);
            DateTime fechaInicio = fechaConsulta.AddDays(diaInicio - 1);
            DateTime fechaFin = fechaConsulta.AddDays(diaFin - 1);

            #region Comparativo

            List<MedicionReporteDTO> listCuadrosFE = new List<MedicionReporteDTO>();
            List<MedicionReporteDTO> listCuadrosTG = new List<MedicionReporteDTO>();
            List<MedicionReporteDTO> listCuadrosUnidades = new List<MedicionReporteDTO>();
            MedicionReporteDTO umbrales = new MedicionReporteDTO();
            List<MedicionReporteDTO> listFuenteEnergia = new List<MedicionReporteDTO>();
            List<MeMedicion96DTO> reporteEmpresas = new List<MeMedicion96DTO>();
            List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();

            (new ReporteMedidoresAppServicio()).ObtenerReporteMedidores(fechaInicio, fechaFin, Constantes.ParametroDefecto, Constantes.ParametroDefecto,
                 Constantes.ParametroDefecto, Constantes.ParametroDefecto, 1,
                   out listCuadrosFE, out listCuadrosTG, out listCuadrosUnidades, out umbrales, out listFuenteEnergia, out reporteEmpresas, out resultadoTG, out List<LogErrorHOPvsMedidores> listaValidacion);

            decimal agua = 0, gas = 0, carbon = 0, residual = 0, diesel = 0, bagazo = 0, biogas = 0, solar = 0, eolica = 0;
            for (int i = diaInicio - 1; i <= diaFin - 1; i++)
            {
                agua = agua + (decimal)model.Cuadro1[i][0];
                gas = gas + (decimal)model.Cuadro2[i][0];
                carbon = carbon + (decimal)model.Cuadro2[i][1];
                residual = residual + (decimal)model.Cuadro2[i][2];
                diesel = diesel + (decimal)model.Cuadro2[i][3];
                bagazo = bagazo + (decimal)model.Cuadro3[i][0];
                biogas = biogas + (decimal)model.Cuadro3[i][1];
                solar = solar + (decimal)model.Cuadro3[i][2];
                eolica = eolica + (decimal)model.Cuadro3[i][3];
            }

            foreach (MedicionReporteDTO item in listFuenteEnergia)
            {
                switch (item.Fenergcodi)
                {
                    case 1:// AGUA
                        {
                            item.EnergiaFuenteEnergiaComparacion = agua * 1000;
                            break;
                        }
                    case 2:	// GAS
                        {
                            item.EnergiaFuenteEnergiaComparacion = gas;
                            break;
                        }
                    case 3: // DIESEL B5                                         
                        {
                            item.EnergiaFuenteEnergiaComparacion = diesel;
                            break;
                        }
                    case 4:	// RESIDUAL
                        {
                            item.EnergiaFuenteEnergiaComparacion = residual;
                            break;
                        }
                    case 5:	// CARBON
                        {
                            item.EnergiaFuenteEnergiaComparacion = carbon;
                            break;
                        }
                    case 6:	// BAGAZO
                        {
                            item.EnergiaFuenteEnergiaComparacion = bagazo;
                            break;
                        }
                    case 7:	// BIOGAS
                        {
                            item.EnergiaFuenteEnergiaComparacion = biogas;
                            break;
                        }
                    case 8:	// SOLAR
                        {
                            item.EnergiaFuenteEnergiaComparacion = solar;
                            break;
                        }
                    case 9:	// EOLICA
                        {
                            item.EnergiaFuenteEnergiaComparacion = eolica;
                            break;
                        }
                    default:
                        {
                            item.EnergiaFuenteEnergiaComparacion = 0;
                            break;
                        }
                }
            }

            #endregion

            resultado.ListaComprativo = listFuenteEnergia.Where(x => x.IndicadorTotal != true).ToList();
            return PartialView(resultado);
        }

    }
}
