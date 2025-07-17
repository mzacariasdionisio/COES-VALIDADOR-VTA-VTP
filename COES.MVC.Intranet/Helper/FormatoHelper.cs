using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using COES.Servicios.Aplicacion.General;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Helper
{
    public class FormatoHelper
    {
        public static List<SiEmpresaDTO> ObtenerEmpresasModulo(int idModulo)
        {
            List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();
            GeneralAppServicio logic = new GeneralAppServicio();
            switch (idModulo)
            {
                case 3://Hidrologia
                    lista = logic.ObtenerEmpresasHidro();
                    break;
                case 5:
                    lista = new List<SiEmpresaDTO>()
                    {
                        new SiEmpresaDTO(){ Emprcodi = -1,Emprnomb ="TODOS"}
                    };
                    break;
                default:
                    lista = logic.ListarEmpresasPorTipo("2");
                    break;
            }
            return lista;
        }

        /// <summary>
        /// Obtiene el nombre de la celda fecha a mostrarse en los formatos excel.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="indice"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public static string ObtenerCeldaFecha(int periodo, int resolucion, int tipoLectura, int horizonte, int indice, int tipoAgregarTiempo, DateTime fechaInicio)
        {
            int minutosAdicionales = tipoAgregarTiempo * resolucion;
            string resultado = string.Empty;
            switch (periodo)
            {
                case ParametrosFormato.PeriodoMensual:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionMes:
                            if (tipoLectura == ParametrosFormato.Ejecutado)
                                resultado = fechaInicio.Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(horizonte + 1);
                            else
                            {
                                resultado = fechaInicio.AddMonths(horizonte).Year.ToString() + " " + COES.Base.Tools.Util.ObtenerNombreMesAbrev(fechaInicio.AddMonths(horizonte).Month);
                            }
                            break;
                        default:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * indice + minutosAdicionales).ToString(Constantes.FormatoFechaHora);
                            break;
                    }
                    break;
                case ParametrosFormato.PeriodoMensualSemana:
                    int semana = COES.Framework.Base.Tools.EPDate.f_numerosemana(fechaInicio.AddDays(horizonte * 7));
                    string stSemana = (semana > 9) ? semana.ToString() : "0" + semana.ToString();
                    if (tipoLectura == ParametrosFormato.Ejecutado)
                    {

                        resultado = fechaInicio.AddDays((horizonte + 1) * 7).Year.ToString() + " Sem:" + stSemana;
                    }
                    else
                    {
                        var fresultado = fechaInicio.AddDays((horizonte + 1) * 7);
                        resultado = fresultado.Year.ToString() + " Sem:" + stSemana;
                    }
                    break;
                case ParametrosFormato.PeriodoDiario:
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionHora:
                        case ParametrosFormato.ResolucionMediaHora:
                        case ParametrosFormato.ResolucionCuartoHora:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice) + minutosAdicionales).ToString(Constantes.FormatoFechaHora);
                            break;
                        default:
                            resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice) + minutosAdicionales).ToString(Constantes.FormatoFecha);
                            break;
                    }

                    break;
                case ParametrosFormato.PeriodoSemanal:
                    if (resolucion == 30)
                        indice += 1;
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice - 1) + minutosAdicionales).ToString(Constantes.FormatoFechaHora);
                    break;
                default:
                    resultado = fechaInicio.AddMinutes(horizonte * ParametrosFormato.ResolucionDia + resolucion * (indice) + minutosAdicionales).ToString(Constantes.FormatoFechaHora);
                    break;
            }

            return resultado;
        }

    }
}