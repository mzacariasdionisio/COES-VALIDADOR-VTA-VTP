using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.Servicios.Aplicacion.Mediciones;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class CalidadProductoController : Controller
    {
        
         //GET: /Mediciones/CalidadProducto/
        LecturaAppServicio appLectura = new LecturaAppServicio();
        IndicadorAppServicio appIndicador = new IndicadorAppServicio();
        public ActionResult Index()
        {
            var oModel = new IndexCalidadProductoModel();
            oModel.FechaInicio = DateTime.Today;
            oModel.FechaFin = DateTime.Today;
            oModel.Intervalo = "15";
            oModel.GpsCodi = 1;
            oModel.Equipos = CrearListadoGps();

            return View(oModel);
        }

        public List<EquipoGps> CrearListadoGps()
        {
            List<EquipoGps> lsEquipos = new List<EquipoGps>();

            lsEquipos.Add(new EquipoGps() { GpsCodi = 1, GpsNombre = "SAN JUAN" });
            lsEquipos.Add(new EquipoGps() { GpsCodi = 2, GpsNombre = "SANTA ROSA" });
            lsEquipos.Add(new EquipoGps() { GpsCodi = 20, GpsNombre = "CHICLAYO" });
            lsEquipos.Add(new EquipoGps() { GpsCodi = 30, GpsNombre = "SOCABAYA" });
            lsEquipos.Add(new EquipoGps() { GpsCodi = 41, GpsNombre = "PIURA" });
            return lsEquipos;
        }

        [HttpPost]
        public PartialViewResult Grafico(string fechaInicial, string fechaFinal, string equipos, string intervalo)
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult CuadrosResumen(string fechaInicial, string fechaFinal, string equipos, string intervalo)
        {
            var modelo = new CuadroResumenModel();
            DateTime dFechaIni = Convert.ToDateTime(fechaInicial);
            DateTime dFechaFin = Convert.ToDateTime(fechaFinal);
            int iIntervalo = Convert.ToInt32(intervalo);
            var lEquipos = equipos.Split(',');
            var iEqupos= Array.ConvertAll(lEquipos, int.Parse).ToList();

            modelo.lsCuadroFrecFrecuenciaInstantaneas = ObtenerVariaciones(dFechaIni, dFechaFin, iEqupos, iIntervalo);
            modelo.lsCuadroIntervalos = ObtenerDatosIntervalo(dFechaIni, dFechaFin, iEqupos, iIntervalo);
            return PartialView(modelo);
        }

        private List<Variacion> ObtenerVariaciones(DateTime dtFechaInicial,DateTime dtFechaFinal,List<int> iEquipo,int iIntervalo)
        {
            var lsResultado = new List<Variacion>();

            for (DateTime fechaini = dtFechaInicial; fechaini <= dtFechaFinal; fechaini = fechaini.AddDays(1))
            {
                foreach (var igps in iEquipo)
                {
                    double ld_frec_min = 0d;
                    double ld_frec_max = 0d;
                    DateTime ldt_fechahora_frec_min = Convert.ToDateTime("2000-01-01");
                    DateTime ldt_fechahora_frec_max = Convert.ToDateTime("2000-01-01");

                    //Matriz para Variación Sostenida
                    int iTamMax = iIntervalo == 15 ? 96 : 48;
                    var arr_VarSostenida = new sVarSostenida[iTamMax];
                    DateTime ldt_fecha = Convert.ToDateTime(fechaini);
                    for (int j = 0; j < iTamMax; j++)
                    {
                        ldt_fecha = ldt_fecha.AddMinutes(iIntervalo);
                        arr_VarSostenida[j].Fechahora = ldt_fecha;
                        arr_VarSostenida[j].Sumnum = 0d;
                        arr_VarSostenida[j].Sumdesv = 0d;
                        arr_VarSostenida[j].Valor = 0d;
                    }
                    //
                    var tDatos = appLectura.GetFechaDesvNumPorGpsFecha(igps, fechaini, fechaini);

                    if (tDatos != null && tDatos.Rows.Count > 0)
                    {
                        
                        var oVariacionSubita = new Variacion();
                        oVariacionSubita.Fecha = fechaini.ToString("dd/MM/yyyy");
                        oVariacionSubita.Rango = "Min.";
                        oVariacionSubita.Indicador = "Variaciones Súbitas de Frecuencia";

                        var oVariacionSostenida = new Variacion();
                        oVariacionSostenida.Fecha = fechaini.ToString("dd/MM/yyyy");
                        oVariacionSostenida.Rango = "Min.";
                        oVariacionSostenida.Indicador = "Variaciones Sostenida de Frecuencia";
                        
                        int i = 0;
                        foreach (DataRow drow in tDatos.Rows)
                        {
                            #region "Obtener Variacion Súbita
                            double ld_frec = Convert.ToDouble(drow["DESV"]);
                            DateTime ldt_fechahora = Convert.ToDateTime(drow["FECHAHORA"]);

                            if (i == 0) // Inicializamos el mínimo
                            {
                                ld_frec_min = ld_frec;
                                ldt_fechahora_frec_min = ldt_fechahora;
                            }

                            if (ld_frec > ld_frec_max) // Hallamos el máximo
                            {
                                ld_frec_max = ld_frec;
                                ldt_fechahora_frec_max = ldt_fechahora;
                            }
                            if (ld_frec < ld_frec_min) // Hallamos el mínimo
                            {
                                ld_frec_min = ld_frec;
                                ldt_fechahora_frec_min = ldt_fechahora;
                            }
                            i++;
                            #endregion
                            #region "Obtener Variacion Sostenida"
                            DateTime ldt_fechahoraSostenida = Convert.ToDateTime(drow["FECHAHORA"]);
                            double ld_desv = 0d, ld_num = 0d;
                            string ls_vacio = Convert.ToString(drow["DESV"]);
                            if (ls_vacio != "") ld_desv = Convert.ToDouble(drow["DESV"]);
                            ls_vacio = Convert.ToString(drow["NUM"]);
                            if (ls_vacio != "") ld_num = Convert.ToDouble(drow["NUM"]);
                            // Hora stamp a minutos
                            double ld_minutos = ldt_fechahoraSostenida.Hour * 60d + ldt_fechahoraSostenida.Minute;
                            int li_indice = Convert.ToInt32(Math.Truncate(ld_minutos / iIntervalo));
                            arr_VarSostenida[li_indice].Sumnum += ld_num;
                            arr_VarSostenida[li_indice].Sumdesv += ld_desv;
                            #endregion
                            
                        }

                        for (int j = 0; j < iTamMax; j++)
                        {
                            if (arr_VarSostenida[j].Sumnum != 0)
                                arr_VarSostenida[j].Valor = Math.Round(arr_VarSostenida[j].Sumdesv / arr_VarSostenida[j].Sumnum * 100d, 4);
                            arr_VarSostenida[j].Sumdesv = Math.Round(arr_VarSostenida[j].Sumdesv, 4);
                        }
                        // Obtenemos max y mín en arr_VarSostenida
                        double ld_frec_minSos = 0d;
                        double ld_frec_maxSos = 0d;
                        DateTime ldt_fechahora_frec_minSos = Convert.ToDateTime("2000-01-01");
                        DateTime ldt_fechahora_frec_maxSos = Convert.ToDateTime("2000-01-01");
                        for (int j = 0; j < 96; j++)
                        {
                            double ld_frecSos = ((arr_VarSostenida[j].Valor / 100d) + 1d) * 60d;
                            DateTime ldt_fechahoraSos = arr_VarSostenida[j].Fechahora;

                            if (j == 0) // Inicializamos el mínimo
                            {
                                ld_frec_minSos = ld_frecSos;
                                ldt_fechahora_frec_minSos = ldt_fechahoraSos;
                            }

                            if (ld_frecSos > ld_frec_maxSos) // Hallamos el máximo
                            {
                                ld_frec_maxSos = ld_frecSos;
                                ldt_fechahora_frec_maxSos = ldt_fechahoraSos;
                            }
                            if (ld_frecSos < ld_frec_minSos) // Hallamos el mínimo
                            {
                                ld_frec_minSos = ld_frecSos;
                                ldt_fechahora_frec_minSos = ldt_fechahoraSos;
                            }
                        }

                        // Mostramos máx y mín Sostenida
                        // Hora Frecuencia mínima
                        oVariacionSostenida.FrecMinHora = ldt_fechahora_frec_minSos.ToString("yyyy-MM-dd HH:mm").Substring(11, 5);
                       // Frecuencia mínima
                        oVariacionSostenida.FrecMinValor = Convert.ToString(Math.Round(ld_frec_minSos, 3));
                        // Hora Frecuencia máxima
                        oVariacionSostenida.FrecMaxHora = ldt_fechahora_frec_maxSos.ToString("yyyy-MM-dd HH:mm").Substring(11, 5);
                        // Frecuencia máxima
                        oVariacionSostenida.FrecMaxValor = Convert.ToString(Math.Round(ld_frec_maxSos, 3));
                        

                        // Mostramos máx y mín Súbita
                        // Hora Frecuencia mínima
                        oVariacionSubita.FrecMinHora = ldt_fechahora_frec_min.ToString("yyyy-MM-dd HH:mm").Substring(11, 5);
                        // Frecuencia mínima
                        oVariacionSubita.FrecMinValor = Convert.ToString(Math.Round(60d + ld_frec_min, 3));
                        // Hora Frecuencia máxima
                        oVariacionSubita.FrecMaxHora = ldt_fechahora_frec_max.ToString("yyyy-MM-dd HH:mm").Substring(11, 5);
                        // Frecuencia máxima
                        oVariacionSubita.FrecMaxValor = Convert.ToString(Math.Round(60d + ld_frec_max, 3));
                        

                        #region Transgresiones
                        oVariacionSostenida.TransgHora = appIndicador.Get_cadena_transgresion(fechaini, "FECHAHORA", igps, "O", "%");
                        oVariacionSostenida.TransgValor = appIndicador.Get_cadena_transgresion(fechaini, "INDICVALOR", igps, "O", "%");
                        oVariacionSostenida.TransgAcum = appIndicador.Get_fallaacumulada(fechaini, igps, "O") + appIndicador.Get_fallaacumuladaSEIN(fechaini,"NUMTRSGSOSTN");

                        oVariacionSubita.TransgHora = appIndicador.Get_cadena_transgresion(fechaini, "FECHAHORA", igps, "U", "Hz");
                        oVariacionSubita.TransgValor = appIndicador.Get_cadena_transgresion(fechaini, "INDICVALOR", igps, "U", "Hz");
                        oVariacionSubita.TransgAcum = appIndicador.Get_fallaacumulada(fechaini, igps, "U") + appIndicador.Get_fallaacumuladaSEIN(fechaini, "NUMTRSGSUBIT");
                        #endregion
                        lsResultado.Add(oVariacionSostenida);
                        lsResultado.Add(oVariacionSubita);
                    }
                }
            }
            

            return lsResultado;
        }

        private List<DatosIntervalo> ObtenerDatosIntervalo(DateTime dtFechaInicial, DateTime dtFechaFinal, List<int> iEquipo,
            int iIntervalo)
        {
            var lsResultado = new List<DatosIntervalo>();

            return lsResultado;
        }
    }
}
