using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Servicios.Aplicacion.Mediciones.Helper
{
    public class MedidoresHelper
    {
        /// <summary>
        /// Obtener el valor según el tipo de periodo (máximo, mínima del día o solo en Hora de punta)
        /// </summary>
        /// <param name="tipoPeriodo"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="lista96"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="valorResultado"></param>
        /// <param name="hResultado"></param>
        /// <param name="fechaHora"></param>
        /// <param name="tipoPeriodoAplicado"></param>
        public static void ObtenerValorHXPeriodoDemandaM96(int tipoPeriodo, DateTime fechaProceso, List<MeMedicion96DTO> lista96,
                                        List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario,
                                        out decimal valorResultado, out int hResultado, out DateTime fechaHora, out int tipoPeriodoAplicado)
        {
            ObtenerValorHXPeriodoDemanda(ParametrosFormato.ResolucionCuartoHora, ConstantesAppServicio.Periodo96, tipoPeriodo, fechaProceso, lista96, listaRangoNormaHP, listaBloqueHorario,
                                        out valorResultado, out hResultado, out fechaHora, out tipoPeriodoAplicado);
        }

        /// <summary>
        /// Obtener el valor según el tipo de periodo (máximo, mínima del día o solo en Hora de punta)
        /// </summary>
        /// <param name="tipoPeriodo"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="lista48"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="valorResultado"></param>
        /// <param name="hResultado"></param>
        /// <param name="fechaHora"></param>
        public static void ObtenerValorHXPeriodoDemandaM48(int tipoPeriodo, DateTime fechaProceso, List<MeMedicion48DTO> lista48,
                                        List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario,
                                        out decimal valorResultado, out int hResultado, out DateTime fechaHora)
        {
            ObtenerValorHXPeriodoDemanda(ParametrosFormato.ResolucionMediaHora, ConstantesAppServicio.Periodo48, tipoPeriodo, fechaProceso, lista48, listaRangoNormaHP, listaBloqueHorario,
                                        out valorResultado, out hResultado, out fechaHora, out int tipoPeriodoAplicado);
        }

        /// <summary>
        /// Obtener el valor según el tipo de periodo (máximo, mínima del día o solo en Hora de punta)
        /// </summary>
        /// <param name="tipoPeriodo"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="lista24"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="valorResultado"></param>
        /// <param name="hResultado"></param>
        /// <param name="fechaHora"></param>
        public static void ObtenerValorHXPeriodoDemandaM24(int tipoPeriodo, DateTime fechaProceso, List<MeMedicion24DTO> lista24,
                                        List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario,
                                        out decimal valorResultado, out int hResultado, out DateTime fechaHora)
        {
            ObtenerValorHXPeriodoDemanda(ParametrosFormato.ResolucionHora, ConstantesAppServicio.Periodo24, tipoPeriodo, fechaProceso, lista24, listaRangoNormaHP, listaBloqueHorario,
                                        out valorResultado, out hResultado, out fechaHora, out int tipoPeriodoAplicado);
        }

        /// <summary>
        /// Obtener el valor según el tipo de periodo (máximo, mínima del día o solo en Hora de punta)
        /// </summary>
        /// <param name="tipoPeriodo"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="lista96"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="valorResultado"></param>
        /// <param name="hResultado"></param>
        /// <param name="fechaHora"></param>
        public static void ObtenerValorHXPeriodoRcaDemandaUsuario96(int tipoPeriodo, DateTime fechaProceso, List<RcaDemandaUsuarioDTO> lista96,
                                        List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario,
                                        out decimal valorResultado, out int hResultado, out DateTime fechaHora)
        {
            ObtenerValorHXPeriodoDemanda(ParametrosFormato.ResolucionCuartoHora, ConstantesAppServicio.Periodo96, tipoPeriodo, fechaProceso, lista96, listaRangoNormaHP, listaBloqueHorario,
                                        out valorResultado, out hResultado, out fechaHora, out int tipoPeriodoAplicado);
        }

        private static void ObtenerValorHXPeriodoDemanda<T>(int resolucion, int filasXDia, int tipoPeriodo, DateTime fechaProceso, List<T> listaMe,
                                        List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario,
                                        out decimal valorResultado, out int hResultado, out DateTime fechaHora, out int tipoPeriodoAplicado)
        {
            
            //norma vigente para la fecha de consulta
            if (tipoPeriodo == ConstantesRepMaxDemanda.TipoMDNormativa)
                tipoPeriodo = ParametroAppServicio.GetPeriodoPorNorma(listaRangoNormaHP, fechaProceso);

            //parámetros de hora punta vigente para la fecha de consulta
            SiParametroValorDTO paramHPyHFP = ParametroAppServicio.GetParametroVigenteHPyHFPXResolucion(listaBloqueHorario, fechaProceso, resolucion);

            //Tipo de periodo aplicado para el cálculo
            tipoPeriodoAplicado = tipoPeriodo;

            hResultado = 0;
            valorResultado = 0;
            for (var j = 1; j <= filasXDia; j++)
            {
                decimal valorH = listaMe.Sum(x => ((decimal?)x.GetType().GetProperty(ConstantesAppServicio.CaracterH + j).GetValue(x, null)) ?? 0);//Sumar las Hx de todos los puntos

                switch (tipoPeriodo)
                {
                    case ConstantesRepMaxDemanda.TipoMaximaTodoDia: //10, obtener maxima de todo el dia
                        //inicializar
                        if (j == 1)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        //cálculo
                        if (valorH > valorResultado)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        break;
                    case ConstantesRepMaxDemanda.TipoMinimaTodoDia: //11, obtener minima de todo el dia
                        //inicializar
                        if (j == 1)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        //cálculo
                        if (valorH < valorResultado)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        break;
                    //3 bloques del día
                    case ConstantesRepMaxDemanda.TipoPeriodoBloqueMedia: //4, Bloque media: despues de minima y antes de maxima
                        //inicializar
                        if (j == paramHPyHFP.HMaxFinMinima + 1)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        //cálculo
                        if (j > paramHPyHFP.HMaxFinMinima && j <= paramHPyHFP.HMaxFinMedia)
                        {
                            if (valorH > valorResultado)
                            {
                                valorResultado = valorH;
                                hResultado = j;
                            }
                        }

                        break;
                    case ConstantesRepMaxDemanda.TipoPeriodoBloqueMinima: //Bloque mínima: despues de máxima y antes de media
                        //inicializar
                        if (j == 1)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        //cálculo
                        if (j <= paramHPyHFP.HMaxFinMinima || j > paramHPyHFP.HMaxFinMaxima)
                        {
                            if (valorH < valorResultado)
                            {
                                valorResultado = valorH;
                                hResultado = j;
                            }
                        }

                        break;
                    case ConstantesRepMaxDemanda.TipoPeriodoBloqueMaxima: //0, HORAS PUNTA - norma (Bloque de Hora Punta)
                    case ConstantesRepMaxDemanda.TipoHoraPunta: //en hora punta
                                                                //inicializar
                        if (j == paramHPyHFP.HFueraHoraPuntaFin + 1)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        //cálculo
                        if (j > paramHPyHFP.HFueraHoraPuntaFin && j < paramHPyHFP.HDespuesHoraPuntaFin) //HP
                        {
                            if (valorH > valorResultado)
                            {
                                valorResultado = valorH;
                                hResultado = j;
                            }
                        }
                        break;

                    case ConstantesRepMaxDemanda.TipoFueraHoraPunta: //HORAS FUERA DE PUNTA
                        //inicializar
                        if (j == 1)
                        {
                            valorResultado = valorH;
                            hResultado = j;
                        }

                        //cálculo
                        if (!(j > paramHPyHFP.HFueraHoraPuntaFin && j < paramHPyHFP.HDespuesHoraPuntaFin)) //HFP
                        {
                            if (valorH > valorResultado)
                            {
                                valorResultado = valorH;
                                hResultado = j;
                            }
                        }
                        break;
                }
            }

            //fecha hora
            fechaHora = fechaProceso.AddMinutes(resolucion * hResultado);
        }

        /// <summary>
        /// total de horas punta en rango de tiempo
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <returns></returns>
        public static int TotalHorasXRango(DateTime fechaIni, DateTime fechaFin, List<SiParametroValorDTO> listaBloqueHorario)
        {
            int totalHoras = 0;
            for (DateTime day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                //parámetros de hora punta vigente para la fecha de consulta
                SiParametroValorDTO paramHPyHFP = ParametroAppServicio.GetParametroVigenteHPyHFPXResolucion(listaBloqueHorario, day, ParametrosFormato.ResolucionCuartoHora);

                totalHoras += paramHPyHFP.TotalHorasHP;
            }

            return totalHoras;
        }


    }

}
