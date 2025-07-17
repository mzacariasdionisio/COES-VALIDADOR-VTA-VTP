using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class RsfAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RsfAppServicio));

        /// <summary>
        /// Permite obtener la configuracion
        /// </summary>
        /// <returns></returns>
        public List<EveRsfdetalleDTO> ObtenerConfiguracion(DateTime fechaPeriodo)
        {
            List<EveRsfdetalleDTO> configuracion = FactorySic.GetEveRsfdetalleRepository().ObtenerConfiguracion(fechaPeriodo);
            List<EveRsfdetalleDTO> resultado = new List<EveRsfdetalleDTO>();

            foreach (EveRsfdetalleDTO item in configuracion)
            {
                //if (item.Grupotipo != ConstantesAppServicio.SI)
                //{
                resultado.Add(item);
                //}
                //else 
                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    List<EqEquipoDTO> equipos = FactorySic.GetEqEquipoRepository().ObtenerPorPadre(item.Equicodi).
                        OrderBy(x => x.Equinomb).ToList();

                    foreach (EqEquipoDTO equipo in equipos)
                    {
                        EveRsfdetalleDTO entity = new EveRsfdetalleDTO
                        {
                            Grupocodi = item.Grupocodi,
                            Emprnomb = item.Emprnomb,
                            Emprcodi = item.Emprcodi,
                            Gruponomb = item.Gruponomb,
                            Ursnomb = item.Ursnomb + " - " + equipo.Equinomb,
                            Equicodi = equipo.Equicodi
                            
                        };
                        resultado.Add(entity);
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener los registros del día
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string[][] ObtenerEstructura(DateTime fecha, out int longitud, out List<int> padres, bool flag, out int columnas, int operacion,
            out int contadorAdd, out List<RsfLimite> listaLimites)
        {
            List<string[]> result = new List<string[]>();
            List<RsfLimite> Limites = new List<RsfLimite>();
            int count = FactorySic.GetEveRsfhoraRepository().ValidarExistencia(fecha);
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();
            List<List<string>> contenido = new List<List<string>>();
            List<int> indices = new List<int>();

            List<string> cabeceras = new List<string>();
            cabeceras.Add("ID");
            cabeceras.Add("");
            cabeceras.Add("CENTRAL");
            cabeceras.Add("URS");
            cabeceras.Add("COD. AGC");

            List<string> headers = new List<string>();
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");

            List<string> totales = new List<string>();
            totales.Add("TOTAL");
            totales.Add("");
            totales.Add("");
            totales.Add("");
            totales.Add("");

            List<string> comentarios = new List<string>();
            comentarios.Add("COMENTARIOS");
            comentarios.Add("");
            comentarios.Add("");
            comentarios.Add("");
            comentarios.Add("");
            columnas = 0;

            int indice = 2;
            int indexPadre = 2;
            string indicadorGrupo = string.Empty;
            foreach (EveRsfdetalleDTO item in configuracion)
            {
                List<string> row = new List<string>();


                //- MODRSF - Mostrar codigos de agc en la grilla

                string codAGC = string.Empty;
                string limiteMin = string.Empty;
                string limiteMax = string.Empty;
                string indicador = string.Empty;

                if (item.Equicodi != null)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        if (!string.IsNullOrEmpty(equiv.Rsfequagccent))
                        {
                            codAGC = equiv.Rsfequagccent;
                        }
                        if (!string.IsNullOrEmpty(equiv.Rsfequagcuni))
                        {
                            codAGC = codAGC + " - " + equiv.Rsfequagcuni;
                        }

                        limiteMin = equiv.Rsfequminimo.ToString();
                        limiteMax = equiv.Rsfequmaximo.ToString();
                        indicador = equiv.Rsfequindicador;


                    }

                    equiv.Grupocodi = (int)item.Grupocodi;
                    equiv.Grupotipo = item.Grupotipo;
                }
                row.Add(item.Grupocodi.ToString() + "-" + item.Equicodi.ToString());
                row.Add(""); //row.Add(item.Emprnomb);
                row.Add(item.Gruponomb);
                row.Add(item.Ursnomb);
                row.Add(codAGC);

                //- Fin MODRSF

                contenido.Add(row);

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    indices.Add(indice);
                    indexPadre = indice;
                    indicadorGrupo = indicador;
                }

                Limites.Add(new RsfLimite
                {
                    Codigo = item.Grupocodi.ToString() + "-" + item.Equicodi.ToString(),
                    LimiteMin = limiteMin,
                    LimiteMax = limiteMax,
                    Indice = indexPadre,
                    Tipo = indicadorGrupo
                });

                indice++;
            }

            //- Vemos la cantidad de elementos del grupo
            foreach (int index in indices)
            {
                int countElement = Limites.Where(x => x.Indice == index).Count();
                List<RsfLimite> subListLimite = Limites.Where(x => x.Indice == index).ToList();
                foreach (RsfLimite itemListLimite in subListLimite)
                {
                    itemListLimite.Contador = countElement;
                }
            }

            listaLimites = Limites;
            padres = indices;
            longitud = configuracion.Count();

            List<RsfEstructura> listaXML = new List<RsfEstructura>();
            int contador = 0;
            int periodoFinal = 0;
            //- Cambio por lectura de XML
            if (operacion > 0)
            {
                listaXML = this.ProcesarArchivoXML(fecha, equivalenciaAgc);
                if (listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).Count() > 0)
                    contador = listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).ToList()[0].MedicionesUp.Count();

                if (operacion == 2)
                {
                    count = 0;
                }
            }

            if (count != 0)
            {
                List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
                List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

                if (operacion > 0)
                {
                    if (operacion != 3)
                    {
                        DateTime fechaInicio = ((DateTime)horas.Last().Rsfhorinicio);
                        int periodo = this.CalcularPeriodo(fechaInicio);

                        periodoFinal = 48 - contador + 1;
                        if (periodoFinal <= periodo)
                        {
                            periodoFinal = periodo + 1;

                            if (fechaInicio.Minute == 0 || fechaInicio.Minute == 30)
                            {
                                periodoFinal = periodoFinal + 1;
                            }
                        }

                        DateTime fechaFinal = this.ObtenerFecha(periodoFinal, fecha);
                        horas.Last().Rsfhorfin = fechaFinal;

                        #region Cambio Movisoft ticket 18853
                        if (operacion == 1)
                        {
                            if (periodoFinal == periodo + 1)
                            {
                                horas.Last().Indicador = 1;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        periodoFinal = 48 - contador + 1;
                        foreach (EveRsfhoraDTO hora in horas)
                        {
                            #region Correccion_RSF_07012021
                            //int perCalculado = this.CalcularPeriodo((DateTime)hora.Rsfhorinicio);
                            int perCalculado = this.CalcularPeriodo((DateTime)hora.Rsfhorinicio) + 1;
                            #endregion

                            if (perCalculado >= periodoFinal)
                            {
                                hora.Indicador = 1;
                            }

                            //acá está el problema
                        }
                    }
                }
                List<EveRsfhoraDTO> listHoras = new List<EveRsfhoraDTO>();
                #region Cambio Movisoft ticket 18853
                /*if (operacion == 3)
                {
                    listHoras = horas.Where(x => x.Indicador != 1).ToList();
                }
                else
                {
                    listHoras = horas;
                }*/
                listHoras = horas.Where(x => x.Indicador != 1).ToList();
                #endregion
                int contadorGrupo = 0;
                foreach (EveRsfhoraDTO hora in listHoras)
                {
                    contadorGrupo = contadorGrupo + 1;
                    List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                    if (contadorGrupo == listHoras.Count)
                    {
                        if (operacion == 3)
                        {
                            //hora.Rsfhorfin
                            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            int per = 48 - contador + 1;
                            DateTime horaFinal = fechaActual.AddMinutes((per - 1) * 30);
                            hora.Rsfhorfin = horaFinal;
                        }
                    }


                    if (flag)
                    {
                        headers.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                            ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));
                        headers.Add("");
                        headers.Add("");
                    }
                    else
                    {
                        headers.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                               ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));
                        headers.Add("");
                        headers.Add("");
                    }

                    cabeceras.Add("Disp");
                    cabeceras.Add("UP");
                    cabeceras.Add("DOWN");

                    comentarios.Add(hora.Rsfhorcomentario);
                    comentarios.Add("");
                    comentarios.Add("");
                    decimal sumUp = items.Where(x => x.Rsfdetindope == null).Sum(x => (x.Rsfdetsub != null) ? (decimal)x.Rsfdetsub : 0);
                    decimal sumDown = items.Where(x => x.Rsfdetindope == null).Sum(x => (x.Rsfdetbaj != null) ? (decimal)x.Rsfdetbaj : 0);

                    totales.Add("");
                    totales.Add(sumUp.ToString());
                    totales.Add(sumDown.ToString());

                    columnas = columnas + 1;
                    int index = 0;
                    foreach (EveRsfdetalleDTO item in configuracion)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string ind = string.Empty;
                        string up = string.Empty;
                        string down = string.Empty;

                        if (registro != null)
                        {
                            ind = registro.Rsfdetindope;

                            up = (registro.Rsfdetsub != null) ? registro.Rsfdetsub.ToString() : string.Empty;
                            down = (registro.Rsfdetbaj != null) ? registro.Rsfdetbaj.ToString() : string.Empty;

                            if (string.IsNullOrEmpty(up) && string.IsNullOrEmpty(down))
                            {
                                up = (registro.Rsfdetvalaut != null) ? registro.Rsfdetvalaut.ToString() : string.Empty;
                                down = up;

                            }
                            if (string.IsNullOrEmpty(ind) && !string.IsNullOrEmpty(up) && !string.IsNullOrEmpty(down)) ind = 1.ToString();
                        }
                        contenido[index].Add(ind);
                        contenido[index].Add(up);
                        contenido[index].Add(down);

                        index++;
                    }
                }
            }
            else
            {
                if (operacion > 0)
                {
                    periodoFinal = 48 - contador + 1;
                }
            }

            int contadorAdicional = 0;
            if (contador > 0)
            {
                for (int i = periodoFinal - 1; i < 48; i++)
                {
                    DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    DateTime horaInicial = fechaActual.AddMinutes(i * 30);
                    DateTime horaFinal = fechaActual.AddMinutes((i + 1) * 30);
                    headers.Add(horaInicial.ToString(ConstantesAppServicio.FormatoHora) + " - " +
                           horaFinal.ToString(ConstantesAppServicio.FormatoHora));
                    headers.Add("");
                    headers.Add("");

                    cabeceras.Add("Disp");
                    cabeceras.Add("UP");
                    cabeceras.Add("DOWN");
                    contadorAdicional += 3;

                    comentarios.Add(string.Empty);
                    comentarios.Add(string.Empty);
                    comentarios.Add(string.Empty);
                }

                int index = 0;

                int countTotal = 0;
                //if (listaXML.Count > 0)
                //    countTotal = listaXML[0].Mediciones.Count;

                if (listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).Count() > 0)
                    countTotal = listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).ToList()[0].MedicionesUp.Count();

                columnas = columnas + countTotal;

                int diferencia = 48 - periodoFinal + 1;

                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    RsfEstructura datosRSF = listaXML.Where(x => x.Equicodi == item.Equicodi).FirstOrDefault();

                    for (int i = countTotal - diferencia; i < countTotal; i++)
                    {
                        if (item.Grupotipo != ConstantesAppServicio.SI)
                        {
                            //- Cambio Movisoft 20012021
                            if (datosRSF != null)
                            {
                                if (datosRSF.MedicionesUp.Count > i)
                                {
                                    contenido[index].Add(1.ToString());
                                    contenido[index].Add(datosRSF.MedicionesUp[i].ToString());
                                    contenido[index].Add(datosRSF.MedicionesDown[i].ToString());
                                }
                                else
                                {
                                    contenido[index].Add(1.ToString());
                                    contenido[index].Add(string.Empty);
                                    contenido[index].Add(string.Empty);
                                }
                            }
                            else
                            {
                                contenido[index].Add(1.ToString());
                                contenido[index].Add(string.Empty);
                                contenido[index].Add(string.Empty);
                            }
                            //- Fin cambio Movisoft 20012021
                        }
                        else
                        {
                            contenido[index].Add(string.Empty);

                            //- Debemos pasar los totales de los datos para los cálculos intermedios
                            List<RsfEstructura> datosSuma = listaXML.Where(x => x.Equipadre == item.Equicodi).ToList();

                            decimal sumRsfUp = 0;
                            decimal sumRsfDown = 0;
                            foreach (RsfEstructura itemSuma in datosSuma)
                            {
                                sumRsfUp = sumRsfUp + itemSuma.MedicionesUp[i];
                                sumRsfDown = sumRsfDown + itemSuma.MedicionesDown[i];
                            }

                            contenido[index].Add(sumRsfUp.ToString());
                            contenido[index].Add(sumRsfDown.ToString());

                            //contenido[index].Add(string.Empty);
                            //contenido[index].Add(string.Empty);
                        }
                    }

                    index++;
                }
            }

            contadorAdd = contadorAdicional;

            result.Add(headers.ToArray());
            result.Add(cabeceras.ToArray());

            foreach (List<string> item in contenido)
            {
                result.Add(item.ToArray());
            }

            result.Add(totales.ToArray());
            result.Add(comentarios.ToArray());
            contadorAdd = result[2].Length - contadorAdd - 1;

            return result.ToArray();
        }

        public string[][] ObtenerEstructuraExcel(DateTime fecha, out int longitud, out List<int> padres, bool flag, out int columnas, int operacion,
            out int contadorAdd, out List<RsfLimite> listaLimites)
        {
            List<string[]> result = new List<string[]>();
            List<RsfLimite> Limites = new List<RsfLimite>();
            int count = FactorySic.GetEveRsfhoraRepository().ValidarExistencia(fecha);
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();
            List<List<string>> contenido = new List<List<string>>();
            List<int> indices = new List<int>();

            List<string> cabeceras = new List<string>();
            cabeceras.Add("ID");
            cabeceras.Add("EMPRESA");
            cabeceras.Add("CENTRAL");
            cabeceras.Add("URS");
            cabeceras.Add("COD. AGC");

            List<string> headers = new List<string>();
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");

            List<string> totales = new List<string>();
            totales.Add("TOTAL");
            totales.Add("");
            totales.Add("");
            totales.Add("");
            totales.Add("");

            List<string> comentarios = new List<string>();
            comentarios.Add("COMENTARIOS");
            comentarios.Add("");
            comentarios.Add("");
            comentarios.Add("");
            comentarios.Add("");
            columnas = 0;

            int indice = 2;
            int indexPadre = 2;
            string indicadorGrupo = string.Empty;
            foreach (EveRsfdetalleDTO item in configuracion)
            {
                List<string> row = new List<string>();


                //- MODRSF - Mostrar codigos de agc en la grilla

                string codAGC = string.Empty;
                string limiteMin = string.Empty;
                string limiteMax = string.Empty;
                string indicador = string.Empty;

                if (item.Equicodi != null)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        if (!string.IsNullOrEmpty(equiv.Rsfequagccent))
                        {
                            codAGC = equiv.Rsfequagccent;
                        }
                        if (!string.IsNullOrEmpty(equiv.Rsfequagcuni))
                        {
                            codAGC = codAGC + " - " + equiv.Rsfequagcuni;
                        }

                        limiteMin = equiv.Rsfequminimo.ToString();
                        limiteMax = equiv.Rsfequmaximo.ToString();
                        indicador = equiv.Rsfequindicador;


                    }

                    equiv.Grupocodi = (int)item.Grupocodi;
                    equiv.Grupotipo = item.Grupotipo;
                }
                row.Add(item.Grupocodi.ToString() + "-" + item.Equicodi.ToString());
                row.Add(item.Emprnomb);
                row.Add(item.Gruponomb);
                row.Add(item.Ursnomb);
                row.Add(codAGC);

                //- Fin MODRSF

                contenido.Add(row);

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    indices.Add(indice);
                    indexPadre = indice;
                    indicadorGrupo = indicador;
                }

                Limites.Add(new RsfLimite
                {
                    Codigo = item.Grupocodi.ToString() + "-" + item.Equicodi.ToString(),
                    LimiteMin = limiteMin,
                    LimiteMax = limiteMax,
                    Indice = indexPadre,
                    Tipo = indicadorGrupo
                });

                indice++;
            }

            //- Vemos la cantidad de elementos del grupo
            foreach (int index in indices)
            {
                int countElement = Limites.Where(x => x.Indice == index).Count();
                List<RsfLimite> subListLimite = Limites.Where(x => x.Indice == index).ToList();
                foreach (RsfLimite itemListLimite in subListLimite)
                {
                    itemListLimite.Contador = countElement;
                }
            }

            listaLimites = Limites;
            padres = indices;
            longitud = configuracion.Count();

            List<RsfEstructura> listaXML = new List<RsfEstructura>();
            int contador = 0;
            int periodoFinal = 0;
            //- Cambio por lectura de XML
            if (operacion > 0)
            {
                listaXML = this.ProcesarArchivoXML(fecha, equivalenciaAgc);
                if (listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).Count() > 0)
                    contador = listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).ToList()[0].MedicionesUp.Count();

                if (operacion == 2)
                {
                    count = 0;
                }
            }

            if (count != 0)
            {
                List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
                List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

                if (operacion > 0)
                {
                    if (operacion != 3)
                    {
                        DateTime fechaInicio = ((DateTime)horas.Last().Rsfhorinicio);
                        int periodo = this.CalcularPeriodo(fechaInicio);

                        periodoFinal = 48 - contador + 1;
                        if (periodoFinal <= periodo)
                        {
                            periodoFinal = periodo + 1;

                            if (fechaInicio.Minute == 0 || fechaInicio.Minute == 30)
                            {
                                periodoFinal = periodoFinal + 1;
                            }
                        }

                        DateTime fechaFinal = this.ObtenerFecha(periodoFinal, fecha);
                        horas.Last().Rsfhorfin = fechaFinal;
                    }
                    else
                    {
                        periodoFinal = 48 - contador + 1;
                        foreach (EveRsfhoraDTO hora in horas)
                        {
                            #region Correccion_RSF_07012021
                            //int perCalculado = this.CalcularPeriodo((DateTime)hora.Rsfhorinicio);
                            int perCalculado = this.CalcularPeriodo((DateTime)hora.Rsfhorinicio) + 1;
                            #endregion

                            if (perCalculado >= periodoFinal)
                            {
                                hora.Indicador = 1;
                            }

                            //acá está el problema
                        }
                    }
                }
                List<EveRsfhoraDTO> listHoras = new List<EveRsfhoraDTO>();
                if (operacion == 3)
                {
                    listHoras = horas.Where(x => x.Indicador != 1).ToList();
                }
                else
                {
                    listHoras = horas;
                }

                int contadorGrupo = 0;
                foreach (EveRsfhoraDTO hora in listHoras)
                {
                    contadorGrupo = contadorGrupo + 1;
                    List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                    if (contadorGrupo == listHoras.Count)
                    {
                        if (operacion == 3)
                        {
                            //hora.Rsfhorfin
                            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            int per = 48 - contador + 1;
                            DateTime horaFinal = fechaActual.AddMinutes((per - 1) * 30);
                            hora.Rsfhorfin = horaFinal;
                        }
                    }


                    if (flag)
                    {
                        headers.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                            ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));
                        headers.Add("");
                        headers.Add("");
                    }
                    else
                    {
                        headers.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                               ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));
                        headers.Add("");
                        headers.Add("");
                    }

                    cabeceras.Add("Disp");
                    cabeceras.Add("UP");
                    cabeceras.Add("DOWN");

                    comentarios.Add(hora.Rsfhorcomentario);
                    comentarios.Add("");
                    comentarios.Add("");
                    decimal sumUp = items.Where(x => x.Rsfdetindope == null).Sum(x => (x.Rsfdetsub != null) ? (decimal)x.Rsfdetsub : 0);
                    decimal sumDown = items.Where(x => x.Rsfdetindope == null).Sum(x => (x.Rsfdetbaj != null) ? (decimal)x.Rsfdetbaj : 0);

                    totales.Add("");
                    totales.Add(sumUp.ToString());
                    totales.Add(sumDown.ToString());

                    columnas = columnas + 1;
                    int index = 0;
                    foreach (EveRsfdetalleDTO item in configuracion)
                    {
                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();
                        string ind = string.Empty;
                        string up = string.Empty;
                        string down = string.Empty;

                        if (registro != null)
                        {
                            ind = registro.Rsfdetindope;

                            up = (registro.Rsfdetsub != null) ? registro.Rsfdetsub.ToString() : string.Empty;
                            down = (registro.Rsfdetbaj != null) ? registro.Rsfdetbaj.ToString() : string.Empty;

                            if (string.IsNullOrEmpty(up) && string.IsNullOrEmpty(down))
                            {
                                up = (registro.Rsfdetvalaut != null) ? registro.Rsfdetvalaut.ToString() : string.Empty;
                                down = up;

                            }
                            if (string.IsNullOrEmpty(ind) && !string.IsNullOrEmpty(up) && !string.IsNullOrEmpty(down)) ind = 1.ToString();
                        }
                        contenido[index].Add(ind);
                        contenido[index].Add(up);
                        contenido[index].Add(down);

                        index++;
                    }
                }
            }
            else
            {
                if (operacion > 0)
                {
                    periodoFinal = 48 - contador + 1;
                }
            }

            int contadorAdicional = 0;
            if (contador > 0)
            {
                for (int i = periodoFinal - 1; i < 48; i++)
                {
                    DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    DateTime horaInicial = fechaActual.AddMinutes(i * 30);
                    DateTime horaFinal = fechaActual.AddMinutes((i + 1) * 30);
                    headers.Add(horaInicial.ToString(ConstantesAppServicio.FormatoHora) + " - " +
                           horaFinal.ToString(ConstantesAppServicio.FormatoHora));
                    headers.Add("");
                    headers.Add("");

                    cabeceras.Add("Disp");
                    cabeceras.Add("UP");
                    cabeceras.Add("DOWN");
                    contadorAdicional += 3;

                    comentarios.Add(string.Empty);
                    comentarios.Add(string.Empty);
                    comentarios.Add(string.Empty);
                }

                int index = 0;

                int countTotal = 0;
                //if (listaXML.Count > 0)
                //    countTotal = listaXML[0].Mediciones.Count;

                if (listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).Count() > 0)
                    countTotal = listaXML.Where(x => x.IndicadorDatos == ConstantesAppServicio.SI).ToList()[0].MedicionesUp.Count();

                columnas = columnas + countTotal;

                int diferencia = 48 - periodoFinal + 1;

                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    RsfEstructura datosRSF = listaXML.Where(x => x.Equicodi == item.Equicodi).FirstOrDefault();

                    for (int i = countTotal - diferencia; i < countTotal; i++)
                    {
                        if (item.Grupotipo != ConstantesAppServicio.SI)
                        {
                            //- Cambio Movisoft 20012021
                            if (datosRSF != null)
                            {
                                if (datosRSF.MedicionesUp.Count > i)
                                {
                                    contenido[index].Add(1.ToString());
                                    contenido[index].Add(datosRSF.MedicionesUp[i].ToString());
                                    contenido[index].Add(datosRSF.MedicionesDown[i].ToString());
                                }
                                else
                                {
                                    contenido[index].Add(1.ToString());
                                    contenido[index].Add(string.Empty);
                                    contenido[index].Add(string.Empty);
                                }
                            }
                            else
                            {
                                contenido[index].Add(1.ToString());
                                contenido[index].Add(string.Empty);
                                contenido[index].Add(string.Empty);
                            }
                            //- Fin cambio Movisoft 20012021
                        }
                        else
                        {
                            contenido[index].Add(string.Empty);

                            //- Debemos pasar los totales de los datos para los cálculos intermedios
                            List<RsfEstructura> datosSuma = listaXML.Where(x => x.Equipadre == item.Equicodi).ToList();

                            decimal sumRsfUp = 0;
                            decimal sumRsfDown = 0;
                            foreach (RsfEstructura itemSuma in datosSuma)
                            {
                                sumRsfUp = sumRsfUp + itemSuma.MedicionesUp[i];
                                sumRsfDown = sumRsfDown + itemSuma.MedicionesDown[i];
                            }

                            contenido[index].Add(sumRsfUp.ToString());
                            contenido[index].Add(sumRsfDown.ToString());

                            //contenido[index].Add(string.Empty);
                            //contenido[index].Add(string.Empty);
                        }
                    }

                    index++;
                }
            }

            contadorAdd = contadorAdicional;

            result.Add(headers.ToArray());
            result.Add(cabeceras.ToArray());

            foreach (List<string> item in contenido)
            {
                result.Add(item.ToArray());
            }

            result.Add(totales.ToArray());
            result.Add(comentarios.ToArray());
            contadorAdd = result[2].Length - contadorAdd - 1;

            return result.ToArray();
        }


        /// <summary>
        /// Calcular el periodo
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int CalcularPeriodo(DateTime fecha)
        {
            int totalMinutes = fecha.Hour * 60 + fecha.Minute;
            return Convert.ToInt32(Math.Ceiling(((decimal)totalMinutes / 30.0M)));
        }

        /// <summary>
        /// Permite obtener la fecha segun el periodo
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public DateTime ObtenerFecha(int periodo, DateTime fecha)
        {
            int minutos = (periodo - 1) * 30;

            return new DateTime(fecha.Year, fecha.Month, fecha.Day).AddMinutes(minutos);
        }

        /// <summary>
        /// Permite obtener la estructura para el idcoes
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string[][] ObtenerEstructura(DateTime fecha)
        {
            int longitud = 0;
            List<int> padres = new List<int>();
            int countAdd = 0;
            List<RsfLimite> limites = new List<RsfLimite>();
            string[][] estructura = this.ObtenerEstructura(fecha, out longitud, out padres, false, out countAdd, 0, out countAdd, out limites);

            List<string[]> list = new List<string[]>();

            for (int i = 0; i < estructura.Length - 1; i++)
            {
                string[] item = new string[estructura[i].Length - 2];
                item[0] = estructura[i][3];

                if (i < estructura.Length - 2)
                {
                    item[1] = estructura[i][2];
                }
                else
                {
                    item[1] = estructura[i][0];
                }


                int indice = 2;
                bool flag = false;
                for (int j = 4; j < estructura[i].Length; j++)
                {
                    item[indice] = estructura[i][j];
                    if (estructura[i][j] != null && estructura[i][j] != "")
                    {
                        flag = true;
                    }
                    indice++;
                }
                if (flag)
                {
                    list.Add(item);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Permite grabar los datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="datos"></param>
        public void Grabar(DateTime fecha, string[][] datos, string usuario)
        {
            try
            {
                //obtener datos actuales en BD
                List<EveRsfhoraDTO> lista30minBD = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha); //48 medias horas
                List<EveRsfdetalleDTO> listaDetalleXUrsBD = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha); //lista de urs por cada media hora

                //Eliminamos todos los datos preexistentes
                FactorySic.GetEveRsfdetalleRepository().Delete(fecha);
                FactorySic.GetEveRsfhoraRepository().Delete(fecha);

                for (int i = 5; i < datos[0].Length; i += 3)
                {
                    EveRsfhoraDTO hora = new EveRsfhoraDTO();
                    hora.Rsfhorindaut = ConstantesAppServicio.SI;
                    hora.Rsfhorindman = ConstantesAppServicio.NO;
                    hora.Lastuser = usuario;
                    hora.Lastdate = DateTime.Now;
                    hora.Rsfhorfecha = fecha;
                    hora.Rsfhorcomentario = datos[datos.Length - 1][i];
                    if (hora.Rsfhorcomentario != null)
                    {
                        try
                        {
                            hora.Rsfhorcomentario = hora.Rsfhorcomentario.Substring(0, 1500);
                        }
                        catch (Exception ex)
                        {
                            hora.Rsfhorcomentario = hora.Rsfhorcomentario;
                        }
                    }

                    DateTime inicio = fecha;
                    DateTime fin = fecha;
                    string itemHora = datos[0][i];
                    this.ObtenerRango(itemHora, ref inicio, ref fin, fecha);
                    hora.Rsfhorinicio = inicio;
                    hora.Rsfhorfin = fin;

                    //verificar si tuvo carga xml previamente
                    string rsfhorindxmlPrevio = null;
                    List<EveRsfdetalleDTO> listaUrs30minBD = new List<EveRsfdetalleDTO>();

                    var reg30minBD = lista30minBD.Find(x => x.Rsfhorinicio == hora.Rsfhorinicio);
                    if (reg30minBD != null)
                    {
                        rsfhorindxmlPrevio = reg30minBD.Rsfhorindxml;
                        listaUrs30minBD = listaDetalleXUrsBD.Where(x => x.Rsfhorcodi == reg30minBD.Rsfhorcodi).ToList();
                    }

                    hora.Rsfhorindxml = rsfhorindxmlPrevio;

                    int idHora = FactorySic.GetEveRsfhoraRepository().Save(hora);

                    for (int j = 2; j < datos.Length - 2; j++)
                    {

                        EveRsfdetalleDTO itemDetalle = new EveRsfdetalleDTO();
                        string[] id = datos[j][0].Split(ConstantesAppServicio.CaracterGuion);
                        itemDetalle.Grupocodi = Convert.ToInt32(id[0]);
                        itemDetalle.Equicodi = Convert.ToInt32(id[1]);
                        itemDetalle.Rsfhorcodi = idHora;

                        if (!string.IsNullOrEmpty(datos[j][i]))
                        {
                            itemDetalle.Rsfdetindope = Convert.ToDecimal(datos[j][i]).ToString();
                        }
                        if (!string.IsNullOrEmpty(datos[j][i + 1]))
                        {
                            itemDetalle.Rsfdetsub = Convert.ToDecimal(datos[j][i + 1]);
                        }
                        if (!string.IsNullOrEmpty(datos[j][i + 2]))
                        {
                            itemDetalle.Rsfdetbaj = Convert.ToDecimal(datos[j][i + 2]);
                        }

                        itemDetalle.Rsfdetvalman = 0;
                        itemDetalle.Lastuser = usuario;
                        itemDetalle.Lastdate = DateTime.Now;

                        //si existe carga previa de xml obtener datos de la urs editada
                        if (rsfhorindxmlPrevio == ConstantesAppServicio.SI)
                        {
                            EveRsfdetalleDTO regDetUrs = listaUrs30minBD.Find(x => x.Grupocodi == itemDetalle.Grupocodi && x.Equicodi == itemDetalle.Equicodi);
                            if (regDetUrs != null)
                            {
                                itemDetalle.Rsfdetdesp = regDetUrs.Rsfdetdesp;
                                itemDetalle.Rsfdetload = regDetUrs.Rsfdetload;
                                itemDetalle.Rsfdetmingen = regDetUrs.Rsfdetmingen;
                                itemDetalle.Rsfdetmaxgen = regDetUrs.Rsfdetmaxgen;
                            }
                        }

                        FactorySic.GetEveRsfdetalleRepository().Save(itemDetalle);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int Grabar2(DateTime fecha, string[][] datos, string usuario, string[][] datos2, int ope)
        {
            int reg = 0;
            try
            {
                if (ope > 0)
                {
                    int i = 5;
                    for (i = 5; i < datos2[0].Length; i += 3)
                    {

                        for (int j = 2; j < datos2.Length; j++)
                        {

                            if ((datos[j][i] != datos2[j][i]) || (datos[j][i + 1] != datos2[j][i + 1]) || (datos[j][i + 2] != datos2[j][i + 2]))
                            {
                                int idHora = 0;
                                if (j < datos.Length - 2 || j == datos.Length - 1)
                                {
                                    EveRsfhoraDTO hora = new EveRsfhoraDTO();
                                    hora.Rsfhorindaut = ConstantesAppServicio.SI;
                                    hora.Rsfhorindman = ConstantesAppServicio.NO;
                                    hora.Lastuser = usuario;
                                    hora.Lastdate = DateTime.Now;
                                    hora.Rsfhorfecha = fecha;
                                    hora.Rsfhorcomentario = datos[datos.Length - 1][i];
                                    if (hora.Rsfhorcomentario != null)
                                    {
                                        try
                                        {
                                            hora.Rsfhorcomentario = hora.Rsfhorcomentario.Substring(0, 1500);
                                        }
                                        catch (Exception ex)
                                        {
                                            hora.Rsfhorcomentario = hora.Rsfhorcomentario;
                                        }
                                    }
                                    DateTime inicio = fecha;
                                    DateTime fin = fecha;
                                    string itemHora = datos[0][i];
                                    this.ObtenerRango(itemHora, ref inicio, ref fin, fecha);
                                    hora.Rsfhorinicio = inicio;
                                    hora.Rsfhorfin = fin;

                                    idHora = FactorySic.GetEveRsfhoraRepository().Update2(hora);
                                }

                                if (j < datos.Length - 2)
                                {
                                    EveRsfdetalleDTO itemDetalle = new EveRsfdetalleDTO();
                                    string[] id = datos[j][0].Split(ConstantesAppServicio.CaracterGuion);
                                    itemDetalle.Grupocodi = Convert.ToInt32(id[0]);
                                    itemDetalle.Equicodi = Convert.ToInt32(id[1]);
                                    itemDetalle.Rsfhorcodi = idHora;

                                    if (!string.IsNullOrEmpty(datos[j][i]))
                                    {
                                        itemDetalle.Rsfdetindope = Convert.ToDecimal(datos[j][i]).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(datos[j][i + 1]))
                                    {
                                        itemDetalle.Rsfdetsub = Convert.ToDecimal(datos[j][i + 1]);
                                    }
                                    if (!string.IsNullOrEmpty(datos[j][i + 2]))
                                    {
                                        itemDetalle.Rsfdetbaj = Convert.ToDecimal(datos[j][i + 2]);
                                    }

                                    //itemDetalle.Rsfdetvalman = 0;
                                    itemDetalle.Lastuser = usuario;
                                    itemDetalle.Lastdate = DateTime.Now;
                                    FactorySic.GetEveRsfdetalleRepository().Update2(itemDetalle);
                                }

                                reg++;
                            }
                        }
                    }

                    if (datos[0].Length > datos2[0].Length)
                    {
                        for (int m = i; m < datos[0].Length; m += 3)
                        {

                            EveRsfhoraDTO hora = new EveRsfhoraDTO();
                            hora.Rsfhorindaut = ConstantesAppServicio.SI;
                            hora.Rsfhorindman = ConstantesAppServicio.NO;
                            hora.Lastuser = usuario;
                            hora.Lastdate = DateTime.Now;
                            hora.Rsfhorfecha = fecha;
                            hora.Rsfhorcomentario = datos[datos.Length - 1][m];
                            if (hora.Rsfhorcomentario != null)
                            {
                                try
                                {
                                    hora.Rsfhorcomentario = hora.Rsfhorcomentario.Substring(0, 1500);
                                }
                                catch (Exception ex)
                                {
                                    hora.Rsfhorcomentario = hora.Rsfhorcomentario;
                                }
                            }
                            DateTime inicio = fecha;
                            DateTime fin = fecha;
                            string itemHora = datos[0][m];
                            this.ObtenerRango(itemHora, ref inicio, ref fin, fecha);
                            hora.Rsfhorinicio = inicio;
                            hora.Rsfhorfin = fin;

                            int idHora = FactorySic.GetEveRsfhoraRepository().Save(hora);

                            for (int k = 2; k < datos.Length - 2; k++)
                            {

                                EveRsfdetalleDTO itemDetalle = new EveRsfdetalleDTO();
                                string[] id = datos[k][0].Split(ConstantesAppServicio.CaracterGuion);
                                itemDetalle.Grupocodi = Convert.ToInt32(id[0]);
                                itemDetalle.Equicodi = Convert.ToInt32(id[1]);
                                itemDetalle.Rsfhorcodi = idHora;

                                if (!string.IsNullOrEmpty(datos[k][m]))
                                {
                                    itemDetalle.Rsfdetindope = Convert.ToDecimal(datos[k][m]).ToString();
                                }
                                if (!string.IsNullOrEmpty(datos[k][m + 1]))
                                {
                                    itemDetalle.Rsfdetsub = Convert.ToDecimal(datos[k][m + 1]);
                                }
                                if (!string.IsNullOrEmpty(datos[k][m + 2]))
                                {
                                    itemDetalle.Rsfdetbaj = Convert.ToDecimal(datos[k][m + 2]);
                                }

                                itemDetalle.Rsfdetvalman = 0;
                                itemDetalle.Lastuser = usuario;
                                itemDetalle.Lastdate = DateTime.Now;
                                FactorySic.GetEveRsfdetalleRepository().Save(itemDetalle);
                                reg++;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 5; i < datos[0].Length; i += 3)
                    {

                        for (int j = 2; j < datos.Length; j++)
                        {

                            if ((datos[j][i] != datos2[j][i]) || (datos[j][i + 1] != datos2[j][i + 1]) || (datos[j][i + 2] != datos2[j][i + 2]))
                            {
                                int idHora = 0;
                                if (j < datos.Length - 2 || j == datos.Length - 1)
                                {
                                    EveRsfhoraDTO hora = new EveRsfhoraDTO();
                                    hora.Rsfhorindaut = ConstantesAppServicio.SI;
                                    hora.Rsfhorindman = ConstantesAppServicio.NO;
                                    hora.Lastuser = usuario;
                                    hora.Lastdate = DateTime.Now;
                                    hora.Rsfhorfecha = fecha;
                                    hora.Rsfhorcomentario = datos[datos.Length - 1][i];
                                    if (hora.Rsfhorcomentario != null)
                                    {
                                        try
                                        {
                                            hora.Rsfhorcomentario = hora.Rsfhorcomentario.Substring(0, 1500);
                                        }
                                        catch (Exception ex)
                                        {
                                            hora.Rsfhorcomentario = hora.Rsfhorcomentario;
                                        }
                                    }
                                    DateTime inicio = fecha;
                                    DateTime fin = fecha;
                                    string itemHora = datos[0][i];
                                    this.ObtenerRango(itemHora, ref inicio, ref fin, fecha);
                                    hora.Rsfhorinicio = inicio;
                                    hora.Rsfhorfin = fin;

                                    idHora = FactorySic.GetEveRsfhoraRepository().Update2(hora);

                                }

                                if (j < datos.Length - 2)
                                {

                                    EveRsfdetalleDTO itemDetalle = new EveRsfdetalleDTO();
                                    string[] id = datos[j][0].Split(ConstantesAppServicio.CaracterGuion);
                                    itemDetalle.Grupocodi = Convert.ToInt32(id[0]);
                                    itemDetalle.Equicodi = Convert.ToInt32(id[1]);
                                    itemDetalle.Rsfhorcodi = idHora;

                                    if (!string.IsNullOrEmpty(datos[j][i]))
                                    {
                                        itemDetalle.Rsfdetindope = Convert.ToDecimal(datos[j][i]).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(datos[j][i + 1]))
                                    {
                                        itemDetalle.Rsfdetsub = Convert.ToDecimal(datos[j][i + 1]);
                                    }
                                    if (!string.IsNullOrEmpty(datos[j][i + 2]))
                                    {
                                        itemDetalle.Rsfdetbaj = Convert.ToDecimal(datos[j][i + 2]);
                                    }

                                    //itemDetalle.Rsfdetvalman = 0;
                                    itemDetalle.Lastuser = usuario;
                                    itemDetalle.Lastdate = DateTime.Now;
                                    FactorySic.GetEveRsfdetalleRepository().Update2(itemDetalle);
                                }
                                reg++;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return reg;
        }

        /// <summary>
        /// Permite generar los archivos XML
        /// </summary>
        /// <param name="fecha"></param>
        public void GeneralXml(DateTime fecha, string path, string file, string ursFile)
        {
            try
            {
                List<RsfXml> ListaLoad = new List<RsfXml>();
                List<RsfXml> ListaLoad1 = new List<RsfXml>();
                List<RsfXml> ListaMax = new List<RsfXml>();
                List<RsfXml> listMaxAGC = new List<RsfXml>();
                List<RsfXml> ListaMin = new List<RsfXml>();

                this.ObtenerDatosXml(fecha, out ListaLoad, out ListaLoad1, out ListaMax, out ListaMin, out listMaxAGC);

                //- Escritura archivo Load
                this.EscribirXml(ListaLoad, path, "UnitScheduledLoad.xml", "UnitScheduledLoad");

                //- Escritura archivo Load
                this.EscribirXml(ListaLoad1, path, "UnitScheduledLoad_Virtual.xml", "UnitScheduledLoad");

                //- Escritura archivo Max
                this.EscribirXml(ListaMax, path, "UnitMaxGeneration.xml", "UnitMaxGeneration");

                //- Escritura archivo Max
                this.EscribirXml(listMaxAGC, path, "UnitMaxGenerationAGC.xml", "UnitMaxGeneration");

                //- Escritura archivo Min
                this.EscribirXml(ListaMin, path, "UnitMinGeneration.xml", "UnitMinGeneration");

                List<string> files = new List<string> { path + "UnitScheduledLoad.xml", path + "UnitScheduledLoad_Virtual.xml", path + "UnitMaxGeneration.xml", path + "UnitMinGeneration.xml", ursFile };

                //- Generamos el archivo comprimido con los tres archivos
                string fileResult = path + file;

                //- Escribir los datos en el archivo comprimido
                GeneracionZip.AddToArchive(fileResult, files,
                     GeneracionZip.ArchiveAction.Replace,
                     GeneracionZip.Overwrite.IfNewer,
                     System.IO.Compression.CompressionLevel.Optimal);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el despacho programado
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hora"></param>
        /// <returns></returns>
        public decimal? ObtenerDespacho(EveRsfequivalenciaDTO item, EveRsfhoraDTO hora, List<CpMedicion48DTO> dataDespacho)
        {
            #region Modificación_RSF_05012021
            //if (((DateTime)hora.Rsfhorfin).Subtract((DateTime)hora.Rsfhorinicio).TotalMinutes == 30)
            if ((((DateTime)hora.Rsfhorfin).Subtract((DateTime)hora.Rsfhorinicio).TotalMinutes == 30) ||
                (((DateTime)hora.Rsfhorfin).Hour == 0 && ((DateTime)hora.Rsfhorfin).Minute == 0 &&
                ((DateTime)hora.Rsfhorinicio).Hour == 23 && ((DateTime)hora.Rsfhorinicio).Minute == 30
                ))
            {
                #endregion
                int periodo = this.CalcularPeriodo((DateTime)hora.Rsfhorinicio) + 1;
                List<int> ids = new List<int>();
                int famcodi = 0;

                if (item.Rsfequindicador == 1.ToString() || item.Rsfequindicador == 2.ToString())
                {
                    ids = item.CodigosGrupos;
                    famcodi = 2;
                }
                else
                {
                    ids.Add(item.Equicodi);
                    famcodi = 1;
                }

                List<CpMedicion48DTO> data = dataDespacho.Where(x => ids.Any(y => y == x.Recurcodisicoes) && x.Famcodi == famcodi).ToList();

                decimal result = 0;

                foreach (CpMedicion48DTO medicion in data)
                {
                    object med = medicion.GetType().GetProperty("H" + periodo).GetValue(medicion, null);
                    if (med != null)
                    {
                        result = result + Convert.ToDecimal(med);
                    }
                }

                return result;
            }

            return 0;
        }


        /// <summary>
        /// Permite obtener la estrucutura de datos para la generación de los archivos XML
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="longitud"></param>
        /// <param name="padres"></param>
        /// <param name="columnas"></param>
        /// <param name="listaLimites"></param>
        /// <returns></returns>
        public string[][] ObtenerEstructuraXML(DateTime fecha, out int longitud, out List<int> padres, out int columnas,
            out List<RsfLimite> listaLimites)
        {
            CpTopologiaDTO entityTopologia = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fecha, ConstantesCortoPlazo.TopologiaDiario);
            List<CpMedicion48DTO> dataDespacho = (new McpAppServicio()).ProcesarDataXML(entityTopologia, fecha, "62,63", true);

            List<string[]> result = new List<string[]>();
            List<RsfLimite> Limites = new List<RsfLimite>();
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();
            List<List<string>> contenido = new List<List<string>>();
            List<int> indices = new List<int>();

            List<string> identificadores = new List<string>();
            identificadores.Add("");
            identificadores.Add("");
            identificadores.Add("");
            identificadores.Add("");
            identificadores.Add("");

            List<string> cabeceras = new List<string>();
            cabeceras.Add("ID");
            cabeceras.Add("");
            cabeceras.Add("CENTRAL");
            cabeceras.Add("URS");
            cabeceras.Add("COD. AGC");

            List<string> headers = new List<string>();
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");

            columnas = 0;

            int indice = 2;
            int indexPadre = 2;
            string indicadorGrupo = string.Empty;
            foreach (EveRsfdetalleDTO item in configuracion)
            {
                List<string> row = new List<string>();

                //- MODRSF - Mostrar codigos de agc en la grilla

                string codAGC = string.Empty;
                string limiteMin = string.Empty;
                string limiteMax = string.Empty;
                string indicador = string.Empty;
                int asignacion = 0;
                int tipoGrupo = 0;

                if (item.Equicodi != null)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        if (!string.IsNullOrEmpty(equiv.Rsfequagccent))
                        {
                            codAGC = equiv.Rsfequagccent;
                        }
                        if (!string.IsNullOrEmpty(equiv.Rsfequagcuni))
                        {
                            codAGC = codAGC + " - " + equiv.Rsfequagcuni;
                        }

                        limiteMin = equiv.Rsfequminimo.ToString();
                        limiteMax = equiv.Rsfequmaximo.ToString();
                        indicador = equiv.Rsfequindicador;
                        asignacion = (equiv.Rsfequasignacion != null) ? (int)equiv.Rsfequasignacion : 0;
                    }

                    equiv.Grupocodi = (int)item.Grupocodi;
                    equiv.Grupotipo = item.Grupotipo;
                }

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    tipoGrupo = 1;
                }

                row.Add(item.Grupocodi.ToString() + "-" + item.Equicodi.ToString());
                row.Add(""); //row.Add(item.Emprnomb);
                row.Add(item.Gruponomb);
                row.Add(item.Ursnomb);
                row.Add(codAGC);

                //- Fin MODRSF

                contenido.Add(row);

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    indices.Add(indice);
                    indexPadre = indice;
                    indicadorGrupo = indicador;
                }

                Limites.Add(new RsfLimite
                {
                    Codigo = item.Grupocodi.ToString() + "-" + item.Equicodi.ToString(),
                    LimiteMin = limiteMin,
                    LimiteMax = limiteMax,
                    Indice = indexPadre,
                    Tipo = indicadorGrupo,
                    Asignacion = asignacion,
                    TipoGrupo = tipoGrupo
                });

                indice++;
            }

            //- Vemos la cantidad de elementos del grupo
            foreach (int index in indices)
            {
                int countElement = Limites.Where(x => x.Indice == index).Count();
                List<RsfLimite> subListLimite = Limites.Where(x => x.Indice == index).ToList();
                foreach (RsfLimite itemListLimite in subListLimite)
                {
                    itemListLimite.Contador = countElement;
                }
            }

            listaLimites = Limites;
            padres = indices;
            longitud = configuracion.Count();

            foreach (EveRsfequivalenciaDTO equiv in equivalenciaAgc)
            {
                if (equiv.Rsfequindicador == 1.ToString() || equiv.Rsfequindicador == 2.ToString())
                {
                    string modosoperacion = string.Join(",", equivalenciaAgc.Where(x => x.Equipadre == equiv.Equicodi).Select(x => x.ModosOperacion));
                    List<int> grupos = modosoperacion.Split(',').Select(int.Parse).ToList();
                    equiv.CodigosGrupos = grupos;
                }
            }

            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
            List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

            int contadorGrupo = 0;
            foreach (EveRsfhoraDTO hora in horas)
            {
                string indicadorDatos = hora.Rsfhorindxml;

                identificadores.Add(hora.Rsfhorcodi.ToString());
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");

                contadorGrupo = contadorGrupo + 1;
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                headers.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                        ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));
                headers.Add("");
                headers.Add("");
                headers.Add("");
                headers.Add("");
                headers.Add("");
                headers.Add("");

                cabeceras.Add("Disp");
                cabeceras.Add("Desp");
                cabeceras.Add("UP");
                cabeceras.Add("DOWN");
                cabeceras.Add("SheLoad");
                cabeceras.Add("MinGen");
                cabeceras.Add("MaxGen");

                columnas = columnas + 1;
                int index = 0;
                string tipoCalculo = string.Empty;
                decimal? despacho = null;
                decimal rsfUp = 0;
                decimal rsfDown = 0;
                int countElement = 0;
                decimal rsfUpTot = 0;
                decimal rsfDownTot = 0;
                int countMax = 0;
                int opeAnt = 0;
                decimal scheduleLoadAnt = 0;

                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        string ind = string.Empty;
                        string up = string.Empty;
                        string down = string.Empty;
                        string desp = string.Empty;
                        string unitLoad = string.Empty;
                        string minGen = string.Empty;
                        string maxGen = string.Empty;

                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();

                        if (registro != null)
                        {
                            ind = registro.Rsfdetindope;
                            up = (registro.Rsfdetsub != null) ? registro.Rsfdetsub.ToString() : string.Empty;
                            down = (registro.Rsfdetbaj != null) ? registro.Rsfdetbaj.ToString() : string.Empty;
                        }

                        if (item.Grupotipo == ConstantesAppServicio.SI)
                        {
                            tipoCalculo = equiv.Rsfequindicador;
                        }

                        if (indicadorDatos != ConstantesAppServicio.SI)
                        {
                            if (tipoCalculo == 1.ToString())
                            {
                                if (item.Grupotipo == ConstantesAppServicio.SI)
                                {
                                    despacho = this.ObtenerDespacho(equiv, hora, dataDespacho);
                                    desp = (despacho != null) ? ((decimal)despacho).ToString() : string.Empty;
                                    unitLoad = desp;
                                    minGen = string.Empty;
                                    maxGen = string.Empty;
                                }
                                else
                                {
                                    desp = string.Empty;
                                    unitLoad = string.Empty;

                                    if (equiv.Rsfequasignacion == 1)
                                    {
                                        try
                                        {
                                            minGen = (((despacho != null) ? ((decimal)despacho) : 0) - ((registro.Rsfdetbaj != null) ? (decimal)registro.Rsfdetbaj : 0)).ToString();
                                            maxGen = (((despacho != null) ? ((decimal)despacho) : 0) + ((registro.Rsfdetsub != null) ? (decimal)registro.Rsfdetsub : 0)).ToString();

                                        }
                                        catch(Exception ex)
                                        {
                                            minGen = string.Empty;
                                            maxGen = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        minGen = 0.ToString();
                                        maxGen = 0.ToString();
                                    }
                                }
                            }
                            else if (tipoCalculo == 2.ToString())
                            {
                                if (item.Grupotipo != ConstantesAppServicio.SI && equiv.Rsfequasignacion == 1)
                                {
                                    EveRsfequivalenciaDTO equivPadre = equivalenciaAgc.Where(x => x.Equicodi == equiv.Equipadre).First();

                                    despacho = this.ObtenerDespacho(equivPadre, hora, dataDespacho);
                                    desp = (despacho != null) ? ((decimal)despacho).ToString() : string.Empty;
                                    unitLoad = desp;

                                    try
                                    {
                                        minGen = (((despacho != null) ? ((decimal)despacho) : 0) - ((registro.Rsfdetbaj != null) ? (decimal)registro.Rsfdetbaj : 0)).ToString();
                                        maxGen = (((despacho != null) ? ((decimal)despacho) : 0) + ((registro.Rsfdetsub != null) ? (decimal)registro.Rsfdetsub : 0)).ToString();
                                    }catch(Exception ex)
                                    {

                                        minGen = string.Empty;
                                        maxGen = string.Empty;
                                    }

                                    if (despacho == 0)
                                    {
                                        minGen = (equiv.Rsfequmaximo != null) ? ((decimal)equiv.Rsfequmaximo).ToString() : 0.ToString();
                                        maxGen = (equiv.Rsfequmaximo != null) ? ((decimal)equiv.Rsfequmaximo).ToString() : 0.ToString();
                                    }
                                }
                            }
                            else if (tipoCalculo == 3.ToString())
                            {
                                if (item.Grupotipo != ConstantesAppServicio.SI)
                                {
                                    despacho = this.ObtenerDespacho(equiv, hora, dataDespacho);
                                    desp = (despacho != null) ? ((decimal)despacho).ToString() : string.Empty;
                                    unitLoad = desp;
                                    try
                                    {
                                        minGen = (((despacho != null) ? ((decimal)despacho) : 0) - ((registro.Rsfdetbaj != null) ? (decimal)registro.Rsfdetbaj : 0)).ToString();
                                        maxGen = (((despacho != null) ? ((decimal)despacho) : 0) + ((registro.Rsfdetsub != null) ? (decimal)registro.Rsfdetsub : 0)).ToString();
                                    }
                                    catch(Exception ex)
                                    {
                                        minGen = string.Empty;
                                        maxGen = string.Empty;
                                    }

                                    if (despacho == 0)
                                    {
                                        minGen = (equiv.Rsfequmaximo != null) ? ((decimal)equiv.Rsfequmaximo).ToString() : 0.ToString();
                                        maxGen = (equiv.Rsfequmaximo != null) ? ((decimal)equiv.Rsfequmaximo).ToString() : 0.ToString();
                                    }
                                }
                            }
                            else if (tipoCalculo == 4.ToString())
                            {
                                if (item.Grupotipo == ConstantesAppServicio.SI)
                                {
                                    despacho = this.ObtenerDespacho(equiv, hora, dataDespacho);

                                    List<int> ids = configuracion.Where(x => x.Grupocodi == item.Grupocodi && x.Grupotipo != ConstantesAppServicio.SI).
                                        Select(x => (int)x.Equicodi).ToList();

                                    rsfUp = items.Where(x => ids.Any(y => y == (int)x.Equicodi)).Sum(x => (x.Rsfdetsub != null) ? (decimal)x.Rsfdetsub : 0);
                                    rsfDown = items.Where(x => ids.Any(y => y == (int)x.Equicodi)).Sum(x => (x.Rsfdetbaj != null) ? (decimal)x.Rsfdetbaj : 0);
                                    countElement = items.Where(x => ids.Any(y => y == (int)x.Equicodi)).Sum(x => (!string.IsNullOrEmpty(x.Rsfdetindope)) ? int.Parse(x.Rsfdetindope) : 0);
                                    desp = (despacho != null) ? ((decimal)despacho).ToString() : string.Empty;
                                    unitLoad = desp;
                                }
                                else
                                {
                                    //- Cambio Movisoft 20012021
                                    decimal limmax = (equiv.Rsfequmaximo != null) ? (decimal)equiv.Rsfequmaximo : 0;

                                    minGen = string.Empty;
                                    maxGen = string.Empty;
                                    desp = string.Empty;
                                    unitLoad = string.Empty;
                                    int ope = (!string.IsNullOrEmpty(ind)) ? int.Parse(ind) : 0;

                                    if (despacho != 0)
                                    {
                                        if (countElement != 0)
                                        {
                                            minGen = (ope * (despacho - rsfDown) / countElement).ToString();
                                            maxGen = (ope * (despacho + rsfUp) / countElement).ToString();
                                        }
                                    }
                                    else
                                    {
                                        minGen = (limmax * ope).ToString();
                                        maxGen = (limmax * ope).ToString();
                                    }

                                    //- Fin cambio movisoft 20012021
                                }
                            }
                            else if (tipoCalculo == 5.ToString())
                            {
                                if (item.Grupotipo == ConstantesAppServicio.SI)
                                {
                                    despacho = this.ObtenerDespacho(equiv, hora, dataDespacho);
                                    desp = (despacho != null) ? ((decimal)despacho).ToString() : string.Empty;
                                    countMax = 0;

                                    List<int> ids = configuracion.Where(x => x.Grupocodi == item.Grupocodi && x.Grupotipo != ConstantesAppServicio.SI).
                                        Select(x => (int)x.Equicodi).ToList();

                                    rsfUpTot = items.Where(x => ids.Any(y => y == (int)x.Equicodi)).Sum(x => (x.Rsfdetsub != null) ? (decimal)x.Rsfdetsub : 0);
                                    rsfDownTot = items.Where(x => ids.Any(y => y == (int)x.Equicodi)).Sum(x => (x.Rsfdetbaj != null) ? (decimal)x.Rsfdetbaj : 0);
                                }
                                else
                                {
                                    int ope = (!string.IsNullOrEmpty(ind)) ? int.Parse(ind) : 0;
                                    decimal limmin = (equiv.Rsfequminimo != null) ? (decimal)equiv.Rsfequminimo : 0;
                                    decimal limmax = (equiv.Rsfequmaximo != null) ? (decimal)equiv.Rsfequmaximo : 0;
                                    try
                                    {

                                        rsfDown = (registro.Rsfdetbaj != null) ? (decimal)registro.Rsfdetbaj : 0;
                                        rsfUp = (registro.Rsfdetsub != null) ? (decimal)registro.Rsfdetsub : 0;
                                    }
                                    catch(Exception ex)
                                    {
                                        rsfDown = 0;
                                        rsfUp = 0;
                                    }

                                    decimal scheduledLoad = 0;
                                    decimal maxGeneration = 0;
                                    decimal minGeneration = 0;

                                    if (equiv.Rsfequasignacion != 1)
                                    {
                                        if (rsfDown + rsfUp == 0)
                                        {
                                            scheduledLoad = (limmin + limmax) / 2;

                                            //- Cambio movisoft 20012021
                                            //minGeneration = (limmin + limmax) / 2;
                                            //maxGeneration = (limmin + limmax) / 2;
                                            minGeneration = limmax * ope;
                                            maxGeneration = limmax * ope;
                                            //- Fin cambio movisoft 20012021                                                                                  
                                        }
                                        else
                                        {
                                            scheduledLoad = limmax - rsfUp;
                                            minGeneration = limmax * ope - rsfUp - rsfDown;
                                            maxGeneration = limmax * ope;
                                        }

                                        unitLoad = scheduledLoad.ToString();
                                        minGen = minGeneration.ToString();
                                        maxGen = maxGeneration.ToString();
                                    }
                                    else
                                    {
                                        countMax++;

                                        if (countMax == 1)
                                        {
                                            if (ope == 0)
                                            {
                                                //- Cambio movisoft 25012021
                                                //scheduledLoad = (limmin + limmax) / 2;
                                                scheduledLoad = limmax;
                                                //- Fin cambio movisoft 25012021
                                            }
                                            else
                                            {
                                                if (ope * limmax - despacho >= rsfUp && despacho - ope * limmin >= rsfDown)
                                                {
                                                    scheduledLoad = (decimal)despacho;
                                                }
                                                else
                                                {
                                                    if (rsfDownTot + rsfUpTot == 0)
                                                    {
                                                        if (despacho > ope * limmax)
                                                        {
                                                            scheduledLoad = ope * limmax;
                                                        }
                                                        else
                                                        {
                                                            scheduledLoad = (decimal)despacho;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        scheduledLoad = ope * limmax - rsfUp;
                                                    }
                                                }
                                            }

                                            opeAnt = ope;
                                            scheduleLoadAnt = scheduledLoad;
                                        }
                                        else if (countMax == 2)
                                        {
                                            if (ope == 0)
                                            {
                                                //scheduledLoad = (limmin + limmax) / 2;

                                                //- Cambio movisoft 25012021
                                                //scheduledLoad = (limmin + limmax) / 2;
                                                scheduledLoad = limmax;
                                                //- Fin cambio movisoft 25012021
                                            }
                                            else
                                            {
                                                if (opeAnt == 0)
                                                {
                                                    if (ope * limmax - despacho >= rsfUp && despacho - ope * limmin >= rsfDown)
                                                    {
                                                        scheduledLoad = (decimal)despacho;
                                                    }
                                                    else
                                                    {
                                                        if (rsfDownTot + rsfUpTot == 0)
                                                        {
                                                            if (despacho > ope * limmax)
                                                            {
                                                                scheduledLoad = ope * limmax;
                                                            }
                                                            else
                                                            {
                                                                scheduledLoad = (decimal)despacho;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            scheduledLoad = ope * limmax - rsfUp;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    decimal cal = (despacho - scheduleLoadAnt <= ope * limmin) ? ope * limmin + rsfDown : (decimal)despacho - scheduleLoadAnt;

                                                    if (cal > ope * limmax)
                                                    {
                                                        scheduledLoad = ope * limmax;
                                                    }
                                                    else
                                                    {
                                                        if (despacho - scheduleLoadAnt <= ope * limmin)
                                                        {
                                                            scheduledLoad = ope * limmin + rsfDown;
                                                        }
                                                        else
                                                        {
                                                            scheduledLoad = (decimal)despacho - scheduleLoadAnt;
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                        //- Correción Movisoft 25012021
                                        if (despacho != 0)
                                        {
                                            minGeneration = scheduledLoad - rsfDown;
                                            maxGeneration = scheduledLoad + rsfUp;
                                        }
                                        else
                                        {
                                            minGeneration = limmax;
                                            maxGeneration = limmax;
                                        }

                                        //- Fin corrección Movisoft 25012021

                                        unitLoad = scheduledLoad.ToString();
                                        minGen = minGeneration.ToString();
                                        maxGen = maxGeneration.ToString();

                                    }

                                    if (despacho == 0)
                                    {
                                        minGen = (equiv.Rsfequmaximo != null) ? ((decimal)equiv.Rsfequmaximo).ToString() : 0.ToString();
                                        maxGen = (equiv.Rsfequmaximo != null) ? ((decimal)equiv.Rsfequmaximo).ToString() : 0.ToString();
                                    }
                                }
                            }
                        }
                        else if (registro != null)
                        {
                            desp = (registro.Rsfdetdesp != null) ? registro.Rsfdetdesp.ToString() : string.Empty;
                            unitLoad = (registro.Rsfdetload != null) ? registro.Rsfdetload.ToString() : string.Empty;
                            minGen = (registro.Rsfdetmingen != null) ? registro.Rsfdetmingen.ToString() : string.Empty;
                            maxGen = (registro.Rsfdetmaxgen != null) ? registro.Rsfdetmaxgen.ToString() : string.Empty;
                        }

                        //- Debemos colocar las demás casuísticas

                        contenido[index].Add(ind);
                        contenido[index].Add(desp);
                        contenido[index].Add(up);
                        contenido[index].Add(down);
                        contenido[index].Add(unitLoad);
                        contenido[index].Add(minGen);
                        contenido[index].Add(maxGen);
                    }


                    index++;
                }
            }


            result.Add(headers.ToArray());
            result.Add(cabeceras.ToArray());

            foreach (List<string> item in contenido)
            {
                result.Add(item.ToArray());
            }

            result.Add(identificadores.ToArray());

            return result.ToArray();
        }


        /// <summary>
        /// Permite obtener la estrucutura de datos para la generación de los archivos XML
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string[][] ObtenerEstructuraXML2(DateTime fecha)
        {
            CpTopologiaDTO entityTopologia = FactorySic.GetCpTopologiaRepository().GetByFechaTopfinal(fecha, ConstantesCortoPlazo.TopologiaDiario);
            List<CpMedicion48DTO> dataDespacho = (new McpAppServicio()).ProcesarDataXML(entityTopologia, fecha, "62,63", true);

            List<string[]> result = new List<string[]>();
            List<RsfLimite> Limites = new List<RsfLimite>();
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();
            List<List<string>> contenido = new List<List<string>>();
            List<int> indices = new List<int>();

            List<string> identificadores = new List<string>();
            identificadores.Add("");
            identificadores.Add("");
            identificadores.Add("");
            identificadores.Add("");
            identificadores.Add("");

            List<string> cabeceras = new List<string>();
            cabeceras.Add("ID");
            cabeceras.Add("");
            cabeceras.Add("CENTRAL");
            cabeceras.Add("URS");
            cabeceras.Add("COD. AGC");

            List<string> headers = new List<string>();
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");
            headers.Add("");

            int indice = 2;
            int indexPadre = 2;
            string indicadorGrupo = string.Empty;
            foreach (EveRsfdetalleDTO item in configuracion)
            {
                List<string> row = new List<string>();

                //- MODRSF - Mostrar codigos de agc en la grilla

                string codAGC = string.Empty;
                string limiteMin = string.Empty;
                string limiteMax = string.Empty;
                string indicador = string.Empty;
                int asignacion = 0;
                int tipoGrupo = 0;

                if (item.Equicodi != null)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        if (!string.IsNullOrEmpty(equiv.Rsfequagccent))
                        {
                            codAGC = equiv.Rsfequagccent;
                        }
                        if (!string.IsNullOrEmpty(equiv.Rsfequagcuni))
                        {
                            codAGC = codAGC + " - " + equiv.Rsfequagcuni;
                        }

                        limiteMin = equiv.Rsfequminimo.ToString();
                        limiteMax = equiv.Rsfequmaximo.ToString();
                        indicador = equiv.Rsfequindicador;
                        asignacion = (equiv.Rsfequasignacion != null) ? (int)equiv.Rsfequasignacion : 0;
                    }

                    equiv.Grupocodi = (int)item.Grupocodi;
                    equiv.Grupotipo = item.Grupotipo;
                }

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    tipoGrupo = 1;
                }

                row.Add(item.Grupocodi.ToString() + "-" + item.Equicodi.ToString());
                row.Add(""); //row.Add(item.Emprnomb);
                row.Add(item.Gruponomb);
                row.Add(item.Ursnomb);
                row.Add(codAGC);

                //- Fin MODRSF

                contenido.Add(row);

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    indices.Add(indice);
                    indexPadre = indice;
                    indicadorGrupo = indicador;
                }

                indice++;
            }


            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
            List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

            int contadorGrupo = 0;
            foreach (EveRsfhoraDTO hora in horas)
            {
                string indicadorDatos = hora.Rsfhorindxml;

                identificadores.Add(hora.Rsfhorcodi.ToString());
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");
                identificadores.Add("");

                contadorGrupo = contadorGrupo + 1;
                List<EveRsfdetalleDTO> items = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi).ToList();

                headers.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                        ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));
                headers.Add("");
                headers.Add("");
                headers.Add("");
                headers.Add("");
                headers.Add("");
                headers.Add("");

                cabeceras.Add("Disp");
                cabeceras.Add("Desp");
                cabeceras.Add("UP");
                cabeceras.Add("DOWN");
                cabeceras.Add("SheLoad");
                cabeceras.Add("MinGen");
                cabeceras.Add("MaxGen");

                int index = 0;
                string tipoCalculo = string.Empty;

                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        string ind = string.Empty;
                        string up = string.Empty;
                        string down = string.Empty;
                        string desp = string.Empty;
                        string unitLoad = string.Empty;
                        string minGen = string.Empty;
                        string maxGen = string.Empty;

                        EveRsfdetalleDTO registro = items.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).FirstOrDefault();

                        if (registro != null)
                        {
                            ind = registro.Rsfdetindope;
                            desp = (registro.Rsfdetdesp != null) ? registro.Rsfdetdesp.ToString() : string.Empty;
                            up = (registro.Rsfdetsub != null) ? registro.Rsfdetsub.ToString() : string.Empty;
                            down = (registro.Rsfdetbaj != null) ? registro.Rsfdetbaj.ToString() : string.Empty;
                            unitLoad = (registro.Rsfdetload != null) ? registro.Rsfdetload.ToString() : string.Empty;
                            minGen = (registro.Rsfdetmingen != null) ? registro.Rsfdetmingen.ToString() : string.Empty;
                            maxGen = (registro.Rsfdetmaxgen != null) ? registro.Rsfdetmaxgen.ToString() : string.Empty;
                        }

                        contenido[index].Add(ind);
                        contenido[index].Add(desp);
                        contenido[index].Add(up);
                        contenido[index].Add(down);
                        contenido[index].Add(unitLoad);
                        contenido[index].Add(minGen);
                        contenido[index].Add(maxGen);
                    }

                    index++;
                }
            }

            result.Add(headers.ToArray());
            result.Add(cabeceras.ToArray());

            foreach (List<string> item in contenido)
            {
                result.Add(item.ToArray());
            }

            result.Add(identificadores.ToArray());

            return result.ToArray();
        }


        /// <summary>
        /// Permite grabar los datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="datos"></param>
        public void GrabarXML(DateTime fecha, string[][] datos, string usuario)
        {
            try
            {
                //- Aca debemos obtener los datos de la grilla para ser almacenados

                for (int i = 5; i < datos[0].Length; i += 7)
                {
                    string idRegistro = datos[datos.Length - 1][i];
                    int idHora = int.Parse(idRegistro);

                    for (int j = 2; j < datos.Length - 1; j++)
                    {
                        EveRsfdetalleDTO itemDetalle = new EveRsfdetalleDTO();
                        string[] id = datos[j][0].Split(ConstantesAppServicio.CaracterGuion);
                        itemDetalle.Grupocodi = Convert.ToInt32(id[0]);
                        itemDetalle.Equicodi = Convert.ToInt32(id[1]);
                        itemDetalle.Rsfhorcodi = idHora;

                        if (!string.IsNullOrEmpty(datos[j][i + 1]))
                        {
                            itemDetalle.Rsfdetdesp = Convert.ToDecimal(datos[j][i + 1]);
                        }
                        if (!string.IsNullOrEmpty(datos[j][i + 4]))
                        {
                            itemDetalle.Rsfdetload = Convert.ToDecimal(datos[j][i + 4]);
                        }
                        if (!string.IsNullOrEmpty(datos[j][i + 5]))
                        {
                            itemDetalle.Rsfdetmingen = Convert.ToDecimal(datos[j][i + 5]);
                        }
                        if (!string.IsNullOrEmpty(datos[j][i + 6]))
                        {
                            itemDetalle.Rsfdetmaxgen = Convert.ToDecimal(datos[j][i + 6]);
                        }

                        itemDetalle.Lastuser = usuario;
                        itemDetalle.Lastdate = DateTime.Now;
                        FactorySic.GetEveRsfdetalleRepository().Update(itemDetalle);
                    }

                    EveRsfhoraDTO entidad = new EveRsfhoraDTO();
                    entidad.Rsfhorcodi = idHora;
                    entidad.Rsfhorindxml = ConstantesAppServicio.SI;
                    FactorySic.GetEveRsfhoraRepository().ActualizarXML(entidad);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int GrabarXML2(DateTime fecha, string[][] datos, string usuario, string[][] datosBkp)
        {
            var exceptions = new ConcurrentQueue<Exception>();
            int reg = 0;
            bool UpdtXml = false;
            try
            {
                //- Aca debemos obtener los datos de la grilla para ser almacenados

                List<int> listIntIten = new List<int>();

                for (int i = 5; i < datos[0].Length; i += 7)
                {
                    listIntIten.Add(i);
                }

                Parallel.ForEach(listIntIten, new ParallelOptions { MaxDegreeOfParallelism = 500 }, (posIndex, state) =>
                {
                    try
                    {
                        int i = posIndex;
                        UpdtXml = false;
                        string idRegistro = datos[datos.Length - 1][i];
                        int idHora = int.Parse(idRegistro);

                        for (int j = 2; j < datos.Length - 1; j++)
                        {
                            if ((datos[j][i + 1] != datosBkp[j][i + 1]) || (datos[j][i + 4] != datosBkp[j][i + 4]) ||
                                (datos[j][i + 5] != datosBkp[j][i + 5]) || (datos[j][i + 6] != datosBkp[j][i + 6]))
                            {

                                UpdtXml = true;
                                EveRsfdetalleDTO itemDetalle = new EveRsfdetalleDTO();
                                string[] id = datos[j][0].Split(ConstantesAppServicio.CaracterGuion);
                                itemDetalle.Grupocodi = Convert.ToInt32(id[0]);
                                itemDetalle.Equicodi = Convert.ToInt32(id[1]);
                                itemDetalle.Rsfhorcodi = idHora;

                                if (!string.IsNullOrEmpty(datos[j][i + 1]))
                                {
                                    itemDetalle.Rsfdetdesp = Convert.ToDecimal(datos[j][i + 1]);
                                }
                                if (!string.IsNullOrEmpty(datos[j][i + 4]))
                                {
                                    itemDetalle.Rsfdetload = Convert.ToDecimal(datos[j][i + 4]);
                                }
                                if (!string.IsNullOrEmpty(datos[j][i + 5]))
                                {
                                    itemDetalle.Rsfdetmingen = Convert.ToDecimal(datos[j][i + 5]);
                                }
                                if (!string.IsNullOrEmpty(datos[j][i + 6]))
                                {
                                    itemDetalle.Rsfdetmaxgen = Convert.ToDecimal(datos[j][i + 6]);
                                }

                                itemDetalle.Lastuser = usuario;
                                itemDetalle.Lastdate = DateTime.Now;
                                FactorySic.GetEveRsfdetalleRepository().Update(itemDetalle);
                                Interlocked.Add(ref reg, 1);
                            }
                        }

                        if (UpdtXml)
                        {
                            EveRsfhoraDTO entidad = new EveRsfhoraDTO();
                            entidad.Rsfhorcodi = idHora;
                            entidad.Rsfhorindxml = ConstantesAppServicio.SI;
                            FactorySic.GetEveRsfhoraRepository().ActualizarXML(entidad);
                        }

                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }

                });

                if (!exceptions.IsEmpty)
                {
                    throw new AggregateException(exceptions);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return reg;
        }


        /// <summary>
        /// Permite grabar los datos en archivos XML
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public void EscribirXml(List<RsfXml> list, string path, string filename, string identificador)
        {
            try
            {
                StringBuilder str = new StringBuilder();

                str.Append("<?xml version=\"1.0\"?>");
                str.Append("<ns0:Data xmlns:ns0=\"http://www.siemens.com/cop/SegmentTypeSchedule.xsd\">");
                str.Append("  <ns0:DataAnalog>");
                str.AppendFormat("    <ns0:ScheduleType>{0}</ns0:ScheduleType>", identificador);
                str.AppendFormat("    <ns0:Msg_Time_Stamp>{0}</ns0:Msg_Time_Stamp>", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                str.AppendFormat("    <ns0:Msg_Identifier>{0}</ns0:Msg_Identifier>", identificador);
                str.Append("    <ns0:Msg_Version>1</ns0:Msg_Version>");

                foreach (RsfXml item in list)
                {
                    str.Append("    <ns0:Object>");
                    str.Append("      <ns0:B1>Network/.Generation</ns0:B1>");
                    str.AppendFormat("      <ns0:B2>{0}</ns0:B2>", item.CentralAgc.Trim());
                    if (!string.IsNullOrEmpty(item.GrupoAgc))
                    {
                        str.AppendFormat("      <ns0:B3>{0}</ns0:B3>", item.GrupoAgc);
                    }
                    else
                    {
                        str.AppendFormat("      <ns0:B3 />");
                    }
                    str.Append("      <ns0:Element/>");
                    foreach (RsfXml subItem in item.ListaValores)
                    {
                        str.Append("      <ns0:DataInfo>");
                        str.AppendFormat("        <ns0:Start_Time>{0}</ns0:Start_Time>", subItem.FechaHora.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                        str.AppendFormat("        <ns0:Value>{0}</ns0:Value>", subItem.Valor);
                        str.Append("      </ns0:DataInfo>");
                    }
                    str.Append("    </ns0:Object>");
                }

                str.Append("  </ns0:DataAnalog>");
                str.Append("</ns0:Data>");

                XmlDocument xdoc = new XmlDocument();

                try
                {
                    if (File.Exists(path + filename))
                    {
                        File.Delete(path + filename);
                    }
                }
                catch
                {
                }

                xdoc.LoadXml(str.ToString());
                xdoc.Save(path + filename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los datos de RSF
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listLoad"></param>
        /// <param name="listMax"></param>
        /// <param name="listMin"></param>
        public void ObtenerDatosXml(DateTime fecha, out List<RsfXml> listLoad, out List<RsfXml> listLoad1, out List<RsfXml> listMax, out List<RsfXml> listMin, out List<RsfXml> listMaxAGC)
        {
            //- Obtención de los datos almacenados en base de datos
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<EveRsfequivalenciaDTO> equivalencias = FactorySic.GetEveRsfequivalenciaRepository().List();
            equivalencias = equivalencias.OrderBy(x => x.Rsfequagccent).ToList();

            #region Modificación_RSF_05012021
            //List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
            //List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().ObtenerDatosXML(fecha);
            List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleXML(fecha);
            #endregion

            List<RsfXml> resultLoad = new List<RsfXml>();
            List<RsfXml> resultLoad1 = new List<RsfXml>();
            List<RsfXml> resultMin = new List<RsfXml>();
            List<RsfXml> resultMax = new List<RsfXml>();
            List<RsfXml> resultMaxAGC = new List<RsfXml>();

            //- Realizarmos un recorrido por cada elemento de la tabla de equivalencias
            string tipo = string.Empty;

            //equivalencias = equivalencias.Where(x => x.Equicodi == 12601 || x.Equicodi == 12602).ToList();

            foreach (EveRsfequivalenciaDTO equivalencia in equivalencias)
            {
                EveRsfdetalleDTO config = configuracion.Where(x => x.Equicodi == equivalencia.Equicodi).FirstOrDefault();
                if (equivalencia.Rsfequagccent == "CHMalpas")
                {

                }

                RsfXml itemLoad = new RsfXml();
                RsfXml itemMin = new RsfXml();
                RsfXml itemMax = new RsfXml();
                RsfXml itemMaxAGC = new RsfXml();

                itemLoad.CentralAgc = equivalencia.Rsfequagccent;
                itemLoad.GrupoAgc = equivalencia.Rsfequagcuni;
                itemLoad.ListaValores = new List<RsfXml>();

                itemMin.CentralAgc = equivalencia.Rsfequagccent;
                itemMin.GrupoAgc = equivalencia.Rsfequagcuni;
                itemMin.ListaValores = new List<RsfXml>();

                itemMax.CentralAgc = equivalencia.Rsfequagccent;
                itemMax.GrupoAgc = equivalencia.Rsfequagcuni;

                itemMaxAGC.CentralAgc = equivalencia.Rsfequagccent;
                itemMaxAGC.GrupoAgc = equivalencia.Rsfequagcuni;

                if (equivalencia.Rsfequindicador == 4.ToString())
                {
                    itemLoad.CentralAgc = equivalencia.Rsfequagcuni;
                    itemLoad.GrupoAgc = string.Empty;
                }

                itemMax.ListaValores = new List<RsfXml>();
                itemMaxAGC.ListaValores = new List<RsfXml>();

                decimal? valorMaximoLimiteConfigurado = equivalencia.Rsfequmaximo == null ? 0 : equivalencia.Rsfequmaximo;


                foreach (EveRsfhoraDTO hora in horas)
                {
                    EveRsfdetalleDTO registro = detalle.Where(x => x.Rsfhorcodi == hora.Rsfhorcodi &&
                    x.Equicodi == equivalencia.Equicodi).FirstOrDefault();

                    if (registro != null)
                    {
                        itemLoad.ListaValores.Add(new RsfXml
                        {
                            FechaHora = ((DateTime)hora.Rsfhorinicio).ToUniversalTime(),
                            Valor = (registro.Rsfdetload != null) ? (decimal)registro.Rsfdetload : 0
                        });

                        itemMin.ListaValores.Add(new RsfXml
                        {
                            FechaHora = ((DateTime)hora.Rsfhorinicio).ToUniversalTime(),
                            Valor = (registro.Rsfdetmingen != null) ? (decimal)registro.Rsfdetmingen : 0
                        });

                        itemMax.ListaValores.Add(new RsfXml
                        {
                            FechaHora = ((DateTime)hora.Rsfhorinicio).ToUniversalTime(),
                            Valor = (registro.Rsfdetmaxgen != null) ? (decimal)registro.Rsfdetmaxgen : 0
                        });

                        itemMaxAGC.ListaValores.Add(new RsfXml
                        {
                            FechaHora = ((DateTime)hora.Rsfhorinicio).ToUniversalTime(),
                            Valor = (decimal)valorMaximoLimiteConfigurado
                        });
                    }
                }

                if (!string.IsNullOrEmpty(equivalencia.Rsfequindicador) && config.Grupotipo == ConstantesAppServicio.SI)
                {
                    tipo = equivalencia.Rsfequindicador;
                }

                //--Aqui debemos hacer el filtrado

                if (tipo == 1.ToString())
                {
                    if (config.Grupotipo == ConstantesAppServicio.SI)
                    {
                        resultLoad.Add(itemLoad);
                    }
                    else
                    {
                        resultMin.Add(itemMin);
                        resultMax.Add(itemMax);
                        resultMaxAGC.Add(itemMaxAGC);
                    }
                }
                else if (tipo == 2.ToString())
                {
                    if (equivalencia.Rsfequasignacion == 1)
                    {
                        resultLoad.Add(itemLoad);
                        resultMin.Add(itemMin);
                        resultMax.Add(itemMax);
                        resultMaxAGC.Add(itemMaxAGC);
                    }
                }
                else if (tipo == 3.ToString())
                {
                    if (config.Grupotipo != ConstantesAppServicio.SI)
                    {
                        resultLoad.Add(itemLoad);
                        resultMin.Add(itemMin);
                        resultMax.Add(itemMax);
                        resultMaxAGC.Add(itemMaxAGC);
                    }
                }
                else if (tipo == 4.ToString())
                {
                    if (config.Grupotipo == ConstantesAppServicio.SI)
                    {
                        resultLoad1.Add(itemLoad);
                        //resultLoad.Add(itemLoad);
                    }
                    else
                    {
                        resultMin.Add(itemMin);
                        resultMax.Add(itemMax);
                        resultMaxAGC.Add(itemMaxAGC);
                    }
                }
                else if (tipo == 5.ToString())
                {
                    if (config.Grupotipo != ConstantesAppServicio.SI)
                    {
                        resultLoad.Add(itemLoad);
                        resultMin.Add(itemMin);
                        resultMax.Add(itemMax);
                        resultMaxAGC.Add(itemMaxAGC);
                    }
                }
            }

            listLoad = resultLoad;
            listMax = resultMax;
            listMaxAGC = resultMaxAGC;
            listMin = resultMin;
            listLoad1 = resultLoad1;
        }


        /// <summary>
        /// Permite obtener el rango de horas
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        public void ObtenerRango(string texto, ref DateTime inicio, ref DateTime fin, DateTime fecha)
        {
            string[] horas = texto.Split(ConstantesAppServicio.CaracterGuion);
            if (horas.Length == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    string[] componentes = horas[i].Split(ConstantesAppServicio.CaracterDosPuntos);

                    if (componentes.Length == 2)
                    {
                        DateTime operacion = fecha.AddHours(Convert.ToInt32(componentes[0])).
                            AddMinutes(Convert.ToInt32(componentes[1]));
                        //.AddSeconds(Convert.ToInt32(componentes[2]));
                        if (i == 0) inicio = operacion;
                        else fin = operacion;
                    }
                }
            }

            //- Corrección Movisoft 03022021
            if ((inicio.Hour == 23 && inicio.Minute == 30) && (fin.Hour == 0 && fin.Minute == 0))
            {
                fin = fin.AddDays(1);
            }
            //- Fin corrección Movisoft 03022021
        }

        /// <summary>
        /// Estilo del excel 
        /// 0: Celdas
        /// 1: Titulos        
        /// </summary>
        /// <param name="rango"></param>
        /// <param name="seccion"></param>
        /// <returns></returns>
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            string colorborder = "#919191";

            rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
            rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rango.Style.Font.Size = 10;

            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#245C86"));
                rango.Style.Font.Bold = false;
                rango.Style.WrapText = true;
                rango.Style.Numberformat.Format = "0.00";
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#4C97C3"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Bold = true;
            }

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D7EFEF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
            }

            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#70AD47"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Bold = true;
            }

            if (seccion == 4)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFEB9C"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#AD6500"));
                rango.Style.Font.Bold = true;
            }

            if (seccion == 5)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#EBEBEB"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
            }

            if (seccion == 6)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#70AD47"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Bold = true;
                rango.Style.Numberformat.Format = "0.00";
            }

            if (seccion == 7)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));
                rango.Style.Font.Bold = true;
                rango.Style.Numberformat.Format = "0.00";
            }
            if (seccion == 8)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFDBA4"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#1C91AE"));
            }

            return rango;
        }

        /// <summary>
        /// Permite exportar los datos de la matriz
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="longitud"></param>
        public void ExportarDatos(string[][] datos, int longitud, DateTime fecha, string path, string fileName, decimal limite)
        {
            try
            {
                fileName = path + fileName;
                FileInfo newFile = new FileInfo(fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        int index = 5;

                        ExcelRange rg;
                        ws.Cells[2, 3].Value = "UNIDADES DE REGULACIÓN SECUNDARIA";
                        ws.Cells[index, 1].Value = "FECHA:";
                        rg = ws.Cells[index, 1, index, 1];
                        rg = ObtenerEstiloCelda(rg, 2);
                        ws.Cells[index, 2].Value = fecha.ToString(ConstantesAppServicio.FormatoFecha);
                        index = 7;

                        GenerarTablaDatosRsf(ws, datos, longitud, index, 1, limite);

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 10);
                        picture.SetSize(120, 40);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Método para generar la tabla del excel, reutilizable también en el PR5 AnexoA
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="datos"></param>
        /// <param name="longitud"></param>
        /// <param name="index"></param>
        /// <param name="colIni"></param>
        /// <param name="limite"></param>
        public void GenerarTablaDatos(ExcelWorksheet ws, string[][] datos, int longitud, int index, int colIni, decimal limite)
        {
            ExcelRange rg;

            for (int i = 0; i < datos.Length; i++)
            {
                if (i <= longitud + 1)
                {
                    for (int j = 0; j < datos[0].Length - 1; j++)
                    {
                        if (j < 1)
                            ws.Cells[index, colIni + j].Value = datos[i][j];
                        else
                            ws.Cells[index, colIni + j].Value = datos[i][j + 1];
                    }
                }
                else
                {
                    ws.Cells[index, colIni].Value = datos[i][0];
                    for (int j = 4; j < datos[0].Length - 1; j++)
                    {
                        if (j < 1)
                            ws.Cells[index, colIni + j].Value = datos[i][j];
                        else
                            ws.Cells[index, colIni + j].Value = datos[i][j + 1];
                    }
                }

                if (i == 0 || i == 1)
                {
                    rg = ws.Cells[index, colIni, index, datos[0].Length + colIni - 2];
                    rg = ObtenerEstiloCelda(rg, 1);
                }
                else if (i > 1 && i <= longitud)
                {

                    if (datos[i][2] != datos[i - 1][2])
                    {
                        rg = ws.Cells[index, colIni, index, datos[0].Length];
                        rg = ObtenerEstiloCelda(rg, 8);
                    }
                    else
                    {
                        rg = ws.Cells[index, colIni, index, colIni + 4 - 1];
                        rg = ObtenerEstiloCelda(rg, 2);

                        if (datos[0].Length >= 5)
                        {
                            rg = ws.Cells[index, colIni - 1 + 5, index, datos[0].Length];
                            rg = ObtenerEstiloCelda(rg, 0);
                        }
                    }
                }
                else if (i == longitud + 1)
                {
                    rg = ws.Cells[index, colIni, index, colIni + 4 - 2];
                    rg = ObtenerEstiloCelda(rg, 3);
                    decimal resultado;
                    for (int j = 5; j < datos[0].Length; j++)
                    {
                        try
                        {
                            //string valor = datos[i][j];
                            string valor = datos[i][j]; // Cambio
                            if (!string.IsNullOrEmpty(valor))
                            {
                                if (decimal.TryParse(valor, out resultado))
                                {
                                    //if (Convert.ToDecimal(valor) != limite)
                                    if (resultado != limite)
                                    {
                                        rg = ws.Cells[index, colIni + j - 1, index, colIni + j - 1];
                                        rg = ObtenerEstiloCelda(rg, 7);
                                    }
                                    else
                                    {
                                        rg = ws.Cells[index, colIni + j - 1, index, colIni + j - 1];
                                        rg = ObtenerEstiloCelda(rg, 6);
                                    }
                                }
                                else
                                {
                                    rg = ws.Cells[index, colIni + j - 1, index, colIni + j - 1];
                                    rg = ObtenerEstiloCelda(rg, 6);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var salida = ex.Message;
                        }

                    }

                }
                else if (i == longitud + 2)
                {
                    rg = ws.Cells[index, colIni, index, colIni + 4 - 2];
                    rg = ObtenerEstiloCelda(rg, 4);

                    if (datos[0].Length >= 5)
                    {
                        rg = ws.Cells[index, colIni - 1 + 5, index, datos[0].Length + colIni - 2];
                        rg = ObtenerEstiloCelda(rg, 5);
                    }
                    rg.Style.WrapText = true;
                }

                index++;
            }

            rg = ws.Cells[index - 2, colIni, index - 2, colIni - 1 + 4];
            rg.Merge = true;
            rg = ws.Cells[index - 1, colIni, index - 1, colIni - 1 + 4];
            rg.Merge = true;

            ws.Column(colIni).Width = 20;
            ws.Column(colIni + 1).Width = 20;
            ws.Column(colIni + 2).Width = 40;
            ws.Column(colIni + 3).Width = 20;

            for (int i = colIni + 1; i <= datos[0].Length + colIni - 1; i++)
            {
                ws.Column(i).Width = 20;
            }
        }

        /// <summary>
        /// Método para generar la tabla del excel, solo para RSF
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="datos"></param>
        /// <param name="longitud"></param>
        /// <param name="index"></param>
        /// <param name="colIni"></param>
        /// <param name="limite"></param>
        public static void GenerarTablaDatosRsf(ExcelWorksheet ws, string[][] datos, int longitud, int index, int colIni, decimal limite)
        {
            ExcelRange rg;

            int ind = index;

            for (int i = 0; i < datos.Length; i++)
            {
                if (i <= longitud + 3)
                {
                    for (int j = 0; j < datos[0].Length; j++)
                    {
                        ws.Cells[index, colIni + j].Value = datos[i][j];
                    }
                }
                /*else
                {
                    ws.Cells[index, colIni].Value = datos[i][0];
                    for (int j = 4; j < datos[0].Length - 1; j++)
                    {
                        if (j < 1)
                            ws.Cells[index, colIni + j].Value = datos[i][j];                        
                        else
                            ws.Cells[index, colIni + j].Value = datos[i][j + 1];
                    }
                }*/

                if (i > 1 && i < (longitud + 2) && datos[i][2] != datos[i - 1][2])
                {
                    for (int j = 6; j < datos[0].Length; j += 3)
                    {
                        string ij = datos[i][j] == "" ? "0" : datos[i][j];
                        string ijplus1 = datos[i][j + 1] == "" ? "0" : datos[i][j + 1];
                        ws.Cells[longitud + 2 + ind, j + 1].Value = Convert.ToDecimal(ws.Cells[longitud + 2 + ind, j + 1].Value) + Convert.ToDecimal(ij);
                        ws.Cells[longitud + 2 + ind, j + 2].Value = Convert.ToDecimal(ws.Cells[longitud + 2 + ind, j + 2].Value) + Convert.ToDecimal(ijplus1);
                    }
                }

                if (i <= 1)
                {
                    rg = ws.Cells[index, colIni, index, datos[0].Length + colIni - 1];
                    rg = ObtenerEstiloCelda(rg, 1);
                }
                else if (i > 1 && i <= longitud + 1)
                {
                    if (datos[i][2] != datos[i - 1][2])
                    {
                        rg = ws.Cells[index, colIni, index, datos[0].Length + colIni - 1];
                        rg = ObtenerEstiloCelda(rg, 8);
                    }
                    else
                    {
                        rg = ws.Cells[index, colIni, index, colIni + 5 - 1];
                        rg = ObtenerEstiloCelda(rg, 2);

                        if (datos[0].Length > 5)
                        {
                            rg = ws.Cells[index, colIni + 5, index, datos[0].Length + colIni - 1];
                            rg = ObtenerEstiloCelda(rg, 0);
                        }
                    }


                }
                else if (i == longitud + 2)
                {
                    rg = ws.Cells[index, colIni, index, colIni + 5 - 1];
                    rg = ObtenerEstiloCelda(rg, 3);
                    decimal resultado;
                    for (int j = 5; j <= datos[0].Length - 1; j++)
                    {
                        try
                        {
                            //string valor = datos[i][j];
                            string valor = datos[i][j]; // Cambio
                            if (!string.IsNullOrEmpty(valor))
                            {
                                if (decimal.TryParse(valor, out resultado))
                                {
                                    //if (Convert.ToDecimal(valor) != limite)
                                    if (resultado != limite)
                                    {
                                        rg = ws.Cells[index, colIni + j, index, colIni + j];
                                        rg = ObtenerEstiloCelda(rg, 7);
                                    }
                                    else
                                    {
                                        rg = ws.Cells[index, colIni + j, index, colIni + j];
                                        rg = ObtenerEstiloCelda(rg, 6);
                                    }
                                }
                                else
                                {
                                    rg = ws.Cells[index, colIni + j, index, colIni + j];
                                    rg = ObtenerEstiloCelda(rg, 6);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var salida = ex.Message;
                        }

                    }

                }
                else if (i == longitud + 3)
                {
                    rg = ws.Cells[index, colIni, index, colIni + 5 - 1];
                    rg = ObtenerEstiloCelda(rg, 4);

                    if (datos[0].Length > 5)
                    {
                        rg = ws.Cells[index, colIni + 5, index, datos[0].Length + colIni - 1];
                        rg = ObtenerEstiloCelda(rg, 5);
                    }
                    rg.Style.WrapText = true;
                }

                index++;
            }

            rg = ws.Cells[index - 2, colIni, index - 2, colIni + 4];
            rg.Merge = true;
            rg = ws.Cells[index - 1, colIni, index - 1, colIni + 4];
            rg.Merge = true;

            ws.Column(colIni).Width = 20;
            ws.Column(colIni + 1).Width = 20;
            ws.Column(colIni + 2).Width = 40;
            ws.Column(colIni + 3).Width = 20;

            for (int i = colIni + 1; i <= datos[0].Length; i++)
            {
                ws.Column(i + 1).Width = 20;
            }
        }

        /// <summary>
        /// Permite exportar los datos cada 30 minutos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void ExportarDatos30(DateTime fecha, string path, string fileName)
        {
            try
            {
                #region Obtencion de datos

                List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
                List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
                List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

                foreach (EveRsfdetalleDTO item in detalle)
                {
                    EveRsfhoraDTO hora = horas.Where(x => x.Rsfhorcodi == item.Rsfhorcodi).First();
                    item.HorInicio = hora.Rsfhorinicio;
                    item.HorFin = hora.Rsfhorfin;
                }

                List<HoraExcel> resultadoAutomatico = new List<HoraExcel>();
                List<HoraExcel> horaExcel = new List<HoraExcel>();

                foreach (EveRsfdetalleDTO item in configuracion)
                {
                    List<EveRsfdetalleDTO> list = detalle.Where(x => x.Grupocodi == item.Grupocodi && x.Equicodi == item.Equicodi).ToList();

                    foreach (EveRsfdetalleDTO child in list)
                    {
                        int horaInicio = ((DateTime)child.HorInicio).Hour;
                        int minutoInicio = ((DateTime)child.HorInicio).Minute;
                        int horaFin = ((DateTime)child.HorFin).Hour;
                        int minutoFin = ((DateTime)child.HorFin).Minute;

                        decimal? valor = child.Rsfdetvalaut;

                        for (int i = horaInicio; i <= horaFin; i++)
                        {
                            if (i == horaInicio || i == horaFin)
                            {
                                if (i == horaInicio)
                                {
                                    if (i != 0 || minutoInicio != 0)
                                        resultadoAutomatico.Add(
                                            new HoraExcel
                                            {
                                                Hora = i,
                                                Minuto = minutoInicio,
                                                Automatico = valor,
                                                IdGrupo = (int)item.Grupocodi,
                                                IdEquipo = (int)item.Equicodi
                                            });

                                    if (minutoInicio == 0 && horaInicio != horaFin)
                                    {
                                        resultadoAutomatico.Add(
                                            new HoraExcel
                                            {
                                                Hora = i,
                                                Minuto = 30,
                                                Automatico = valor,
                                                IdGrupo = (int)item.Grupocodi,
                                                IdEquipo = (int)item.Equicodi
                                            });
                                    }
                                }
                                if (i == horaFin)
                                {
                                    if (horaInicio != horaFin)
                                    {
                                        resultadoAutomatico.Add(
                                            new HoraExcel
                                            {
                                                Hora = i,
                                                Minuto = 0,
                                                Automatico = valor,
                                                IdGrupo = (int)item.Grupocodi,
                                                IdEquipo = (int)item.Equicodi
                                            });
                                    }

                                    if (minutoFin != 0)
                                    {
                                        if (minutoFin > 30 && horaInicio != horaFin)
                                        {
                                            resultadoAutomatico.Add(
                                                new HoraExcel
                                                {
                                                    Hora = i,
                                                    Minuto = 30,
                                                    Automatico = valor,
                                                    IdGrupo = (int)item.Grupocodi,
                                                    IdEquipo = (int)item.Equicodi
                                                });
                                        }

                                        resultadoAutomatico.Add(
                                            new HoraExcel
                                            {
                                                Hora = i,
                                                Minuto = minutoFin,
                                                Automatico = valor,
                                                IdGrupo = (int)item.Grupocodi,
                                                IdEquipo = (int)item.Equicodi
                                            });
                                    }
                                }
                            }
                            else
                            {
                                resultadoAutomatico.Add(
                                    new HoraExcel
                                    {
                                        Hora = i,
                                        Minuto = 0,
                                        Automatico = valor,
                                        IdGrupo = (int)item.Grupocodi,
                                        IdEquipo = (int)item.Equicodi
                                    });

                                resultadoAutomatico.Add(
                                    new HoraExcel
                                    {
                                        Hora = i,
                                        Minuto = 30,
                                        Automatico = valor,
                                        IdGrupo = (int)item.Grupocodi,
                                        IdEquipo = (int)item.Equicodi
                                    });
                            }
                        }
                    }
                }

                List<HoraExcel> listAutomatico = (from itemAuto in resultadoAutomatico
                                                  orderby itemAuto.Hora, itemAuto.Minuto
                                                  select new HoraExcel { Hora = itemAuto.Hora, Minuto = itemAuto.Minuto }).Distinct().ToList();

                foreach (HoraExcel item in listAutomatico)
                {
                    if (horaExcel.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto).Count() == 0)
                    {
                        horaExcel.Add(new HoraExcel { Hora = item.Hora, Minuto = item.Minuto });
                    }
                }

                List<HoraExcel> listHora = (from hora in horaExcel orderby hora.Hora, hora.Minuto select hora).ToList();

                #endregion

                fileName = path + fileName;
                FileInfo newFile = new FileInfo(fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        int indice = 0;
                        int column = 2;
                        int row = 8;
                        ExcelRange rg = null;

                        ws.Cells[2, 3].Value = "FECHA:";
                        rg = ws.Cells[2, 3, 2, 3];
                        rg = ObtenerEstiloCelda(rg, 2);
                        ws.Cells[2, 4].Value = fecha.ToString(ConstantesAppServicio.FormatoFecha);

                        foreach (HoraExcel item in listHora)
                        {
                            ws.Cells[row, column].Value = item.Hora.ToString().PadLeft(2, '0') + ":" + item.Minuto.ToString().PadLeft(2, '0');
                            row++;
                        }

                        rg = ws.Cells[7, 2, row - 1, 2];
                        rg = ObtenerEstiloCelda(rg, 2);

                        ws.Cells[4, column].Value = "TIPO";
                        ws.Cells[5, column].Value = "URS";
                        ws.Cells[6, column].Value = "EMPRESA";
                        ws.Cells[7, column].Value = "CENTRAL";

                        rg = ws.Cells[4, column, 7, column];
                        rg = ObtenerEstiloCelda(rg, 1);

                        column++;

                        foreach (EveRsfdetalleDTO config in configuracion)
                        {
                            ws.Cells[4, column].Value = "AUT";
                            ws.Cells[5, column].Value = config.Ursnomb.Trim();
                            ws.Cells[6, column].Value = config.Emprnomb.Trim();
                            ws.Cells[7, column].Value = config.Gruponomb.Trim();

                            rg = ws.Cells[4, column, 7, column];
                            rg = ObtenerEstiloCelda(rg, 1);

                            row = 8;
                            foreach (HoraExcel item in listHora)
                            {
                                HoraExcel child = resultadoAutomatico.Where(x => x.Hora == item.Hora && x.Minuto == item.Minuto &&
                                    x.IdGrupo == config.Grupocodi && x.IdEquipo == config.Equicodi && x.Automatico != null).FirstOrDefault();
                                if (child != null)
                                {
                                    if (child.Automatico != null)
                                        ws.Cells[row, column].Value = child.Automatico;
                                }

                                rg = ws.Cells[row, column, row, column];
                                rg = ObtenerEstiloCelda(rg, 0);

                                row++;
                            }

                            indice++;

                            column++;
                        }

                        ws.Column(2).Width = 20;


                        for (int i = 3; i <= configuracion.Count + 2; i++)
                        {
                            ws.Column(i).Width = 20;
                        }

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 60);
                        picture.SetSize(120, 40);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite exportar los datos segun rango de fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void ExportarReporte(DateTime fechaInicio, DateTime fechaFin, string path, string fileName)
        {
            try
            {
                List<EveRsfhoraDTO> list = FactorySic.GetEveRsfhoraRepository().ObtenerReporte(fechaInicio, fechaFin);

                fileName = path + fileName;
                FileInfo newFile = new FileInfo(fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                    if (ws != null)
                    {
                        int index = 5;
                        ExcelRange rg;

                        ws.Cells[2, 3].Value = "UNIDADES DE REGULACIÓN SECUNDARIA";
                        ws.Cells[index, 2].Value = "FECHA DESDE:";
                        rg = ws.Cells[index, 2, index, 2];
                        rg = ObtenerEstiloCelda(rg, 2);

                        ws.Cells[index, 3].Value = fechaInicio.ToString(ConstantesAppServicio.FormatoFecha);
                        ws.Cells[index, 4].Value = "FECHA HASTA:";
                        rg = ws.Cells[index, 4, index, 4];
                        rg = ObtenerEstiloCelda(rg, 2);
                        ws.Cells[index, 5].Value = fechaFin.ToString(ConstantesAppServicio.FormatoFecha);

                        index += 2;
                        ws.Cells[index, 2].Value = "FECHA";
                        ws.Cells[index, 3].Value = "INICIO";
                        ws.Cells[index, 4].Value = "FINAL";
                        ws.Cells[index, 5].Value = "URS";
                        ws.Cells[index, 6].Value = "RESERVA ASIGNADA (MW)";
                        ws.Cells[index, 7].Value = "TIPO";

                        rg = ws.Cells[index, 2, index, 7];
                        rg = ObtenerEstiloCelda(rg, 1);

                        index++;

                        foreach (EveRsfhoraDTO item in list)
                        {
                            ws.Cells[index, 2].Value = ((DateTime)item.Rsfhorfecha).ToString("dd/MM/yyyy");
                            ws.Cells[index, 3].Value = ((DateTime)item.Rsfhorinicio).ToString("HH:mm");
                            ws.Cells[index, 4].Value = ((DateTime)item.Rsfhorfin).ToString("HH:mm");
                            ws.Cells[index, 5].Value = item.Ursnomb;
                            ws.Cells[index, 6].Value = item.Valorautomatico;
                            ws.Cells[index, 7].Value = "AUTOMATICO";
                            index++;
                        }

                        if (list.Count > 0)
                        {
                            rg = ws.Cells[8, 2, index - 1, 7];
                            rg = ObtenerEstiloCelda(rg, 0);
                        }

                        ws.Column(2).Width = 20;
                        ws.Column(3).Width = 20;
                        ws.Column(4).Width = 20;
                        ws.Column(5).Width = 20;
                        ws.Column(6).Width = 20;
                        ws.Column(7).Width = 20;

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture(ConstantesAppServicio.NombreLogo, img);
                        picture.SetPosition(10, 60);
                        picture.SetSize(120, 40);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #region Métodos Tabla EVE_RSFEQUIVALENCIA

        /// <summary>
        /// Inserta un registro de la tabla EVE_RSFEQUIVALENCIA
        /// </summary>
        public void SaveEveRsfequivalencia(EveRsfequivalenciaDTO entity)
        {
            try
            {
                FactorySic.GetEveRsfequivalenciaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_RSFEQUIVALENCIA
        /// </summary>
        public void UpdateEveRsfequivalencia(EveRsfequivalenciaDTO entity)
        {
            try
            {
                FactorySic.GetEveRsfequivalenciaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_RSFEQUIVALENCIA
        /// </summary>
        public void DeleteEveRsfequivalencia(int rsfequcodi)
        {
            try
            {
                FactorySic.GetEveRsfequivalenciaRepository().Delete(rsfequcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_RSFEQUIVALENCIA
        /// </summary>
        public EveRsfequivalenciaDTO GetByIdEveRsfequivalencia(int rsfequcodi)
        {
            return FactorySic.GetEveRsfequivalenciaRepository().GetById(rsfequcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_RSFEQUIVALENCIA
        /// </summary>
        public List<EveRsfequivalenciaDTO> ListEveRsfequivalencias()
        {
            return FactorySic.GetEveRsfequivalenciaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveRsfequivalencia
        /// </summary>
        public List<EveRsfequivalenciaDTO> GetByCriteriaEveRsfequivalencias()
        {
            return FactorySic.GetEveRsfequivalenciaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener los datos del yupana para el manejo del XML
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="equivalencia"></param>
        /// <returns></returns>
        public List<RsfEstructura> ProcesarArchivoXML(DateTime fecha, List<EveRsfequivalenciaDTO> equivalencia)
        {
            try
            {
                #region Obtención de Datos de RSF

                string restcodi = "100,101,102,103";
                CpTopologiaDTO topologia = (new McpAppServicio()).ObtenerTopologiaFinalPorFecha(fecha, ConstantesCortoPlazo.TopologiaDiario, 48);
                List<CpMedicion48DTO> listReserva = FactorySic.GetCpMedicion48Repository().GetByCriteria(topologia.Topcodi.ToString(), fecha, restcodi);
                List<CpMedicion48DTO> listReservaUp = listReserva.Where(x => x.Srestcodi == 100 || x.Srestcodi == 102).ToList();
                List<CpMedicion48DTO> listReservaDown = listReserva.Where(x => x.Srestcodi == 101 || x.Srestcodi == 103).ToList();

                List<RsfEstructura> result = new List<RsfEstructura>();

                //- Empezamos con la modificación de los valores, esta es la parte central del codigo

                foreach (CpMedicion48DTO itemUp in listReservaUp)
                {
                    RsfEstructura entity = new RsfEstructura();
                    entity.Grupocodi = itemUp.Recurcodisicoes;
                    entity.Famcodi = itemUp.Famcodi;
                    entity.ListaValores = new List<RsfEstructura>();
                    int contador = 1;

                    CpMedicion48DTO itemDown = listReservaDown.Where(x => x.Recurcodisicoes == entity.Grupocodi && x.Famcodi == entity.Famcodi).FirstOrDefault();
                    try
                    {


                        for (int i = topologia.HoraReprograma; i <= 48; i++)
                        {
                            RsfEstructura itemEntity = new RsfEstructura();
                            itemEntity.Periodo = contador;
                            object oUp = itemUp.GetType().GetProperty("H" + i).GetValue(itemUp, null);
                            itemEntity.RsfUp = (oUp != null) ? (decimal?)Convert.ToDecimal(oUp) : null;

                            if (itemDown != null)
                            {
                                object oDown = itemDown.GetType().GetProperty("H" + i).GetValue(itemDown, null);
                                itemEntity.RsfDown = (oDown != null) ? (decimal?)Convert.ToDecimal(oDown) : null;
                            }

                            entity.ListaValores.Add(itemEntity);
                            contador++;
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                    result.Add(entity);
                }

                #endregion

                #region Procesamiento de datos

                List<RsfEstructura> resultFinal = new List<RsfEstructura>();
                #region Caso 1 y 2

                List<int> listCaso1 = equivalencia.Where(x => x.Rsfequindicador == 1.ToString() || x.Rsfequindicador == 2.ToString()).Select(x => x.Equicodi).ToList();

                foreach (int equipadre in listCaso1)
                {
                    //- Modos operacion que debemos tomar
                    string modosoperacion = string.Join(",", equivalencia.Where(x => x.Equipadre == equipadre).Select(x => x.ModosOperacion));
                    List<int> grupos = modosoperacion.Split(',').Select(int.Parse).ToList();
                    List<RsfEstructura> rsfData = result.Where(x => grupos.Any(y => y == x.Grupocodi)).ToList();
                    int equitv = equivalencia.Where(x => x.Equipadre == equipadre && x.IndCC == 1).Select(x => x.Equicodi).FirstOrDefault();

                    RsfEstructura entity = new RsfEstructura();
                    entity.Equicodi = equitv;
                    entity.Equipadre = equipadre;
                    entity.IndicadorDatos = ConstantesAppServicio.SI;
                    entity.MedicionesUp = new List<decimal>();
                    entity.MedicionesDown = new List<decimal>();

                    int count = (rsfData.Count > 0) ? rsfData[0].ListaValores.Count : 0;

                    for (int i = 0; i < count; i++)
                    {
                        decimal rsfup = 0;
                        decimal rsfdown = 0;
                        foreach (RsfEstructura item in rsfData)
                        {
                            rsfup = rsfup + (decimal)item.ListaValores[i].RsfUp;
                            rsfdown = rsfdown + (decimal)item.ListaValores[i].RsfDown;
                        }
                        entity.MedicionesUp.Add(rsfup);
                        entity.MedicionesDown.Add(rsfdown);
                    }

                    resultFinal.Add(entity);
                }

                #endregion Caso 1 y 2

                #region Caso 3

                List<int> listCaso3 = equivalencia.Where(x => x.Rsfequindicador == 3.ToString()).Select(x => x.Equicodi).ToList();

                foreach (int equipadre in listCaso3)
                {
                    //- Modos operacion que debemos tomar                    
                    List<int> equipos = equivalencia.Where(x => x.Equipadre == equipadre).Select(x => x.Equicodi).ToList();

                    foreach (int equipo in equipos)
                    {
                        RsfEstructura rsfData = result.Where(x => x.Grupocodi == equipo).FirstOrDefault();

                        if (rsfData != null)
                        {
                            RsfEstructura entity = new RsfEstructura();
                            entity.Equicodi = rsfData.Grupocodi;
                            entity.Equipadre = equipadre;
                            entity.IndicadorDatos = ConstantesAppServicio.SI;
                            entity.MedicionesUp = new List<decimal>();
                            entity.MedicionesDown = new List<decimal>();

                            foreach (RsfEstructura subItem in rsfData.ListaValores)
                            {
                                entity.MedicionesUp.Add((decimal)subItem.RsfUp);
                                entity.MedicionesDown.Add((decimal)subItem.RsfDown);
                            }

                            resultFinal.Add(entity);
                        }
                    }

                }

                #endregion

                #region Caso 4

                // List<int> listCaso4 = equivalencia.Where(x => x.Rsfequindicador == 4.ToString()).Select(x => x.Equicodi).ToList();
                var listCaso4 = equivalencia.Where(x => x.Rsfequindicador == 4.ToString()).Select(x => new { Equicodi = x.Equicodi, Famcodi = x.Famcodi }).ToList();
                foreach (var equipadre in listCaso4)
                {
                    RsfEstructura rsfData = result.Where(x => x.Grupocodi == equipadre.Equicodi && x.Famcodi == equipadre.Famcodi).FirstOrDefault();

                    if (rsfData != null)
                    {
                        //- Modos operacion que debemos tomar                    
                        List<int> equipos = equivalencia.Where(x => x.Equipadre == equipadre.Equicodi).Select(x => x.Equicodi).ToList();

                        foreach (int equipo in equipos)
                        {
                            RsfEstructura entity = new RsfEstructura();
                            entity.Equicodi = equipo;
                            entity.Equipadre = equipadre.Equicodi;
                            entity.IndicadorDatos = ConstantesAppServicio.SI;
                            entity.MedicionesUp = new List<decimal>();
                            entity.MedicionesDown = new List<decimal>();

                            foreach (RsfEstructura subItem in rsfData.ListaValores)
                            {
                                entity.MedicionesUp.Add((decimal)subItem.RsfUp / equipos.Count);
                                entity.MedicionesDown.Add((decimal)subItem.RsfDown / equipos.Count);
                            }

                            resultFinal.Add(entity);
                        }
                    }

                }

                #endregion Caso 4

                #region Caso 5

                var listCaso5 = equivalencia.Where(x => x.Rsfequindicador == 5.ToString()).Select(x => new { Equicodi = x.Equicodi, Famcodi = x.Famcodi }).ToList();

                foreach (var equipadre in listCaso5)
                {
                    RsfEstructura rsfData = result.Where(x => x.Grupocodi == equipadre.Equicodi && x.Famcodi == equipadre.Famcodi).FirstOrDefault();

                    if (rsfData != null)
                    {
                        // if ((rsfData.ListaValores.Sum(x => x.RsfDown) + rsfData.ListaValores.Sum(x => x.RsfUp)) > 0)
                        // Correccion el dia 01/01/2021 a solicitud de D. callupe por problemas de CH Matuca
                        if (rsfData.ListaValores.Sum(x => x.RsfDown) > 0 || rsfData.ListaValores.Sum(x => x.RsfUp) > 0)
                        {
                            List<RsfEstructura> resultTemporal = new List<RsfEstructura>();
                            List<EveRsfequivalenciaDTO> subEquivalencia = equivalencia.Where(x => x.Equipadre == equipadre.Equicodi).ToList();

                            foreach (EveRsfequivalenciaDTO item in subEquivalencia)
                            {
                                item.Diferencia = (decimal)(item.Rsfequmaximo - item.Rsfequminimo);
                                RsfEstructura entity = new RsfEstructura();
                                entity.Equicodi = item.Equicodi;
                                entity.Equipadre = equipadre.Equicodi;
                                //entity.IndicadorDatos = ConstantesAppServicio.SI;
                                entity.MedicionesUp = new List<decimal>();
                                entity.MedicionesDown = new List<decimal>();
                                resultTemporal.Add(entity);
                            }

                            foreach (RsfEstructura subItem in rsfData.ListaValores)
                            {
                                int index = 0;

                                if (subItem.RsfDown + subItem.RsfUp > 0)
                                {
                                    foreach (EveRsfequivalenciaDTO item in subEquivalencia)
                                    {
                                        if (index == 0)
                                        {
                                            item.Acumulado = (decimal)(subItem.RsfUp + subItem.RsfDown);
                                        }
                                        else
                                        {
                                            if (subEquivalencia[index - 1].Acumulado - subEquivalencia[index - 1].Diferencia < 0)
                                            {
                                                item.Acumulado = 0;
                                            }
                                            else
                                            {
                                                item.Acumulado = subEquivalencia[index - 1].Acumulado - subEquivalencia[index - 1].Diferencia;
                                            }
                                        }
                                        decimal rsfUp = 0;
                                        decimal rsfDown = 0;

                                        if (item.Diferencia < item.Acumulado)
                                        {
                                            rsfDown = (decimal)(item.Diferencia * subItem.RsfDown / (subItem.RsfUp + subItem.RsfDown));
                                            rsfUp = (decimal)(item.Diferencia * subItem.RsfUp / (subItem.RsfUp + subItem.RsfDown));
                                        }
                                        else
                                        {
                                            rsfDown = (decimal)(item.Acumulado * subItem.RsfDown / (subItem.RsfUp + subItem.RsfDown));
                                            rsfUp = (decimal)(item.Acumulado * subItem.RsfUp / (subItem.RsfUp + subItem.RsfDown));
                                        }

                                        resultTemporal[index].MedicionesUp.Add(rsfUp);
                                        resultTemporal[index].MedicionesDown.Add(rsfDown);
                                        resultTemporal[index].IndicadorDatos = ConstantesAppServicio.SI;

                                        index++;
                                    }
                                }
                                else
                                {
                                    foreach (EveRsfequivalenciaDTO item in subEquivalencia)
                                    {
                                        resultTemporal[index].MedicionesUp.Add(0);
                                        resultTemporal[index].MedicionesDown.Add(0);
                                        index++;
                                    }

                                }
                            }

                            resultFinal.AddRange(resultTemporal);
                        }
                    }
                }

                #endregion                

                #endregion

                return resultFinal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Horas

        /// <summary>
        /// Lista las horas de una fecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EveRsfhoraDTO> ObtenerHorasPorFecha(DateTime fecha)
        {
            return FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
        }

        /// <summary>
        /// Permite grabar los datos de una hora
        /// </summary>
        /// <param name="entity"></param>
        public void GrabarHora(EveRsfhoraDTO entity)
        {
            try
            {
                if (entity.Rsfhorcodi == 0)
                {
                    FactorySic.GetEveRsfhoraRepository().Save(entity);
                }
                else
                {
                    FactorySic.GetEveRsfhoraRepository().Update(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los datos de una hora
        /// </summary>
        /// <param name="id"></param>
        public void EliminarHora(int id)
        {
            try
            {
                FactorySic.GetEveRsfdetalleRepository().DeletePorId(id);
                FactorySic.GetEveRsfhoraRepository().DeletePorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Configuracion

        public string[][] ObtenerEstructuraConfiguracion(DateTime fecha, out int longitud, out List<int> padres)
        {
            List<string[]> result = new List<string[]>();

            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<List<string>> contenido = new List<List<string>>();
            List<int> indices = new List<int>();

            //- MODRSF - Listado de equivalencias AGC
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();

            List<string> cabeceras = new List<string>();
            cabeceras.Add("ID");
            cabeceras.Add("CENTRAL");
            cabeceras.Add("URS");
            cabeceras.Add("CENTRAL. AGC");
            cabeceras.Add("GENE. AGC");
            cabeceras.Add("TIPO CÁLCULO");
            cabeceras.Add("LÍMITE MÍNIMO");
            cabeceras.Add("LÍMITE MÁXIMO");
            //cabeceras.Add("EQUIVALENCIA");
            //cabeceras.Add("INDICADOR");

            int indice = 1;
            foreach (EveRsfdetalleDTO item in configuracion)
            {
                List<string> row = new List<string>();
                row.Add(item.Grupocodi.ToString() + "-" + item.Equicodi.ToString());
                row.Add(item.Gruponomb);
                row.Add(item.Ursnomb);
                string centAGC = string.Empty;
                string uniAGC = string.Empty;
                decimal? minimo = null;
                decimal? maximo = null;
                string tipo = string.Empty;
                string codigos = string.Empty;
                string indicador = string.Empty;

                if (item.Equicodi != null)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        centAGC = equiv.Rsfequagccent;
                        uniAGC = equiv.Rsfequagcuni;
                        minimo = equiv.Rsfequminimo;
                        maximo = equiv.Rsfequmaximo;
                        tipo = equiv.Rsfequindicador;
                        codigos = equiv.Rsfequrecurcodi;
                        //indicador = equiv.Rsfequasignacion;
                    }
                }
                row.Add(centAGC);
                row.Add(uniAGC);
                row.Add(tipo);
                row.Add((minimo == null) ? string.Empty : ((decimal)minimo).ToString());
                row.Add((maximo == null) ? string.Empty : ((decimal)maximo).ToString());
                //row.Add(codigos);
                //row.Add(indicador);
                //- Fin MODRSF

                contenido.Add(row);

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    indices.Add(indice);
                }

                indice++;
            }
            padres = indices;
            longitud = configuracion.Count();

            //- Fin de cambio

            result.Add(cabeceras.ToArray());

            foreach (List<string> item in contenido)
            {
                result.Add(item.ToArray());
            }

            return result.ToArray();
        }


        /// <summary>
        /// Permite grabar los datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="datos"></param>
        public void GrabarConfiguracion(string[][] datos, string usuario)
        {
            try
            {

                for (int i = 1; i < datos.Length; i++)
                {
                    string[] id = datos[i][0].Split(ConstantesAppServicio.CaracterGuion);
                    int Grupocodi = Convert.ToInt32(id[0]);
                    int Equicodi = Convert.ToInt32(id[1]);

                    EveRsfequivalenciaDTO entity = new EveRsfequivalenciaDTO();
                    entity.Equicodi = Equicodi;
                    entity.Rsfequagccent = datos[i][3];
                    entity.Rsfequagcuni = datos[i][4];
                    entity.Rsfequindicador = datos[i][5];
                    entity.Rsfequminimo = (string.IsNullOrEmpty(datos[i][6])) ? null : (decimal?)decimal.Parse(datos[i][6]);
                    entity.Rsfequmaximo = (string.IsNullOrEmpty(datos[i][7])) ? null : (decimal?)decimal.Parse(datos[i][7]);
                    //entity.Rsfequrecurcodi = datos[i][8];
                    //entity.Rsfequasignacion = datos[i][9];
                    entity.Rsfequlastuser = usuario;
                    entity.Rsfequlastdate = DateTime.Now;

                    EveRsfequivalenciaDTO verificar = FactorySic.GetEveRsfequivalenciaRepository().GetById(entity.Equicodi);

                    if (verificar != null)
                    {
                        FactorySic.GetEveRsfequivalenciaRepository().Update(entity);
                    }
                    else
                    {
                        FactorySic.GetEveRsfequivalenciaRepository().Save(entity);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Gráfico RSF

        /// <summary>
        /// Permite obtener los datos para el armando del gráfico
        /// </summary>
        /// <param name="fecha"></param>
        public List<RsfGrafico> ObtenerGraficoRSF(DateTime fecha)
        {

            List<RsfGrafico> categorias = new List<RsfGrafico>();
            List<EveRsfdetalleDTO> configuracion = this.ObtenerConfiguracion(fecha);
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();
            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fecha);
            List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fecha);

            List<int> idsHoras = new List<int>();

            int periodo = this.CalcularPeriodo(DateTime.Now) - 1;
            int countHora = 0;
            RsfGrafico cabecera = new RsfGrafico();
            cabecera.ListaHoras = new List<string>();
            foreach (EveRsfhoraDTO hora in horas)
            {
                int periodoComparar = this.CalcularPeriodo((DateTime)hora.Rsfhorinicio);

                if (periodoComparar >= periodo)
                {
                    idsHoras.Add(hora.Rsfhorcodi);
                    cabecera.ListaHoras.Add(((DateTime)hora.Rsfhorinicio).ToString(ConstantesAppServicio.FormatoHora) + " - " +
                               ((DateTime)hora.Rsfhorfin).ToString(ConstantesAppServicio.FormatoHora));

                    countHora++;

                    if (countHora == 3) break;
                }
            }



            List<RsfLimite> Limites = new List<RsfLimite>();

            #region Obteniendo estructura

            List<int> indices = new List<int>();
            int indice = 2;
            int indexPadre = 2;
            string indicadorGrupo = string.Empty;
            foreach (EveRsfdetalleDTO item in configuracion)
            {
                string codAGC = string.Empty;
                string limiteMin = string.Empty;
                string limiteMax = string.Empty;
                string indicador = string.Empty;
                int asignacion = 0;
                int tipoGrupo = 0;

                if (item.Equicodi != null)
                {
                    EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == (int)item.Equicodi).FirstOrDefault();

                    if (equiv != null)
                    {
                        if (!string.IsNullOrEmpty(equiv.Rsfequagccent))
                        {
                            codAGC = equiv.Rsfequagccent;
                        }
                        if (!string.IsNullOrEmpty(equiv.Rsfequagcuni))
                        {
                            codAGC = codAGC + " - " + equiv.Rsfequagcuni;
                        }

                        limiteMin = equiv.Rsfequminimo.ToString();
                        limiteMax = equiv.Rsfequmaximo.ToString();
                        indicador = equiv.Rsfequindicador;
                        asignacion = (equiv.Rsfequasignacion != null) ? (int)equiv.Rsfequasignacion : 0;
                    }

                    equiv.Grupocodi = (int)item.Grupocodi;
                    equiv.Grupotipo = item.Grupotipo;
                }

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    tipoGrupo = 1;
                }

                //- Fin MODRSF                             

                if (item.Grupotipo == ConstantesAppServicio.SI)
                {
                    indices.Add(indice);
                    indexPadre = indice;
                    indicadorGrupo = indicador;
                }

                Limites.Add(new RsfLimite
                {
                    Codigo = item.Grupocodi.ToString() + "-" + item.Equicodi.ToString(),
                    Nombre = codAGC,
                    LimiteMin = limiteMin,
                    LimiteMax = limiteMax,
                    Indice = indexPadre,
                    Tipo = indicadorGrupo,
                    Asignacion = asignacion,
                    TipoGrupo = tipoGrupo,
                    Equicodi = (int)item.Equicodi
                });

                indice++;
            }

            //- Vemos la cantidad de elementos del grupo
            foreach (int index in indices)
            {
                int countElement = Limites.Where(x => x.Indice == index).Count();
                List<RsfLimite> subListLimite = Limites.Where(x => x.Indice == index).ToList();
                foreach (RsfLimite itemListLimite in subListLimite)
                {
                    itemListLimite.Contador = countElement;
                }
            }

            #endregion


            #region Definicion de categorias de datos

            for (int i = 0; i < Limites.Count; i++)
            {
                if (Limites[i].Tipo == 1.ToString())
                {
                    int despachoid = Limites[i].Equicodi;
                    int reservaid = Limites[i + Limites[i].Contador - 1].Equicodi;
                    int idDato = i + Limites[i].Contador - 1;
                    i = i + Limites[i].Contador - 1;

                    List<int> ids = new List<int>();
                    ids.Add(reservaid);

                    categorias.Add(new RsfGrafico
                    {
                        Equicodi = despachoid,
                        Nombre = Limites[idDato].Nombre,
                        Elementos = ids
                    });
                }
                else if (Limites[i].Tipo == 2.ToString())
                {
                    if (Limites[i].Asignacion == 1)
                    {
                        int despachoid = Limites[i].Equicodi;
                        int reservaid = Limites[i].Equicodi;

                        List<int> ids = new List<int>();
                        ids.Add(reservaid);

                        categorias.Add(new RsfGrafico
                        {
                            Equicodi = despachoid,
                            Nombre = Limites[i].Nombre,
                            Elementos = ids
                        });

                    }
                }
                else if (Limites[i].Tipo == 3.ToString())
                {
                    if (Limites[i].TipoGrupo != 1)
                    {
                        int despachoid = Limites[i].Equicodi;
                        int reservaid = Limites[i].Equicodi;

                        List<int> ids = new List<int>();
                        ids.Add(reservaid);

                        categorias.Add(new RsfGrafico
                        {
                            Equicodi = despachoid,
                            Nombre = Limites[i].Nombre,
                            Elementos = ids
                        });
                    }
                }
                else if (Limites[i].Tipo == 4.ToString())
                {
                    int despachoid = Limites[i].Equicodi;
                    List<int> ids = new List<int>();

                    for (int j = i + 1; j <= i + Limites[i].Contador - 1; j++)
                    {
                        int reservaid = Limites[j].Equicodi;
                        ids.Add(reservaid);
                    }

                    categorias.Add(new RsfGrafico
                    {
                        Equicodi = despachoid,
                        Nombre = Limites[i].Nombre,
                        Elementos = ids
                    });

                    i = i + Limites[i].Contador - 1;
                }
                else if (Limites[i].Tipo == 5.ToString())
                {
                    if (Limites[i].TipoGrupo != 1)
                    {
                        int despachoid = Limites[i].Equicodi;
                        int reservaid = Limites[i].Equicodi;

                        List<int> ids = new List<int>();
                        ids.Add(reservaid);

                        categorias.Add(new RsfGrafico
                        {
                            Equicodi = despachoid,
                            Nombre = Limites[i].Nombre,
                            Elementos = ids
                        });
                    }
                }
            }


            #endregion

            #region Armado de los datos del gráfico



            foreach (RsfGrafico categoria in categorias)
            {
                categoria.ListaValores = new List<RsfGrafico>();
                foreach (int idHora in idsHoras)
                {
                    RsfGrafico itemGrafico = new RsfGrafico();
                    //- Elemento para la obtención de despacho
                    EveRsfdetalleDTO registro = detalle.Where(x => x.Rsfhorcodi == idHora && x.Equicodi == categoria.Equicodi).FirstOrDefault();
                    if (registro != null)
                    {
                        itemGrafico.Load = (registro.Rsfdetload != null) ? (decimal)registro.Rsfdetload : 0;
                    }
                    //- Elementos para el cálculo de la reserva UP y DOWN
                    List<EveRsfdetalleDTO> registros = detalle.Where(x => x.Rsfhorcodi == idHora && categoria.Elementos.Any(y => y == x.Equicodi)).ToList();
                    if (registros.Count > 0)
                    {
                        itemGrafico.RsfUp = registros.Sum(x => (x.Rsfdetsub != null) ? (decimal)x.Rsfdetsub : 0);
                        itemGrafico.RsfDown = registros.Sum(x => (x.Rsfdetbaj != null) ? (decimal)x.Rsfdetbaj : 0);
                    }
                    categoria.ListaValores.Add(itemGrafico);
                }
            }

            categorias.Insert(0, cabecera);

            #endregion

            return categorias;
        }

        /// <summary>
        /// Permite obtener los datos para el gráfico de potencia
        /// </summary>
        /// <param name="fecha"></param>
        public List<PrecioGrafico> ObtenerDatosGraficoPrecio(DateTime fecha)
        {
            #region Obtención de datos
            List<SmaOfertaDetalleDTO> ofertaDetalleDTOs = FactorySic.GetSmaOfertaDetalleRepository().ListByDate(fecha, fecha.AddDays(1),
                Subastas.ConstantesSubasta.OfertipoDiaria, Subastas.ConstantesSubasta.EstadoActivo);

            foreach (var ofertaDetalle in ofertaDetalleDTOs)
            {
                if (ofertaDetalle.Ofdeprecio != null)
                    if ((new Subastas.SubastasAppServicio()).AnalizarNumerico(ofertaDetalle.Ofdeprecio) == false)
                        ofertaDetalle.Ofdeprecio = (new Subastas.SubastasAppServicio()).DecryptData(ofertaDetalle.Ofdeprecio);

                string txtFecInicio = ofertaDetalle.Ofdehorainicio;
                string txtFecFin = ofertaDetalle.Ofdehorafin;
                int horaInicioCalc = 0;
                int horaFinCalc = 0;

                if (!string.IsNullOrEmpty(txtFecInicio))
                {
                    string[] arrFecInicio = txtFecInicio.Split(':');

                    if (arrFecInicio.Length == 2)
                    {
                        horaInicioCalc = int.Parse(arrFecInicio[0]) * 30 + int.Parse(arrFecInicio[1]);
                    }
                }

                if (!string.IsNullOrEmpty(txtFecFin))
                {
                    string[] arrFecFin = txtFecFin.Split(':');

                    if (arrFecFin.Length == 2)
                    {
                        horaFinCalc = int.Parse(arrFecFin[0]) * 30 + int.Parse(arrFecFin[1]);
                    }
                }
                ofertaDetalle.Horinicio = horaInicioCalc;
                ofertaDetalle.Horfin = horaFinCalc;
            }

            #endregion

            var list = ofertaDetalleDTOs.Select(x => new { Usrcodi = x.Urscodi, Usrnomb = x.Gruponomb }).Distinct().ToList();

            List<PrecioGrafico> result = new List<PrecioGrafico>();

            foreach (var item in list)
            {
                PrecioGrafico itemResult = new PrecioGrafico();
                itemResult.Nombre = item.Usrnomb;
                itemResult.ListaValores = new List<decimal>();
                List<SmaOfertaDetalleDTO> subList = ofertaDetalleDTOs.Where(x => x.Urscodi == item.Usrcodi).ToList();

                for (int i = 0; i < 48; i++)
                {
                    decimal valor = 0;
                    int horaInicio = i * 30;
                    int horaFin = (i + 1) * 30;
                    if (i == 47) horaFin = 47 * 30 + 29;

                    var itemDato = subList.Where(x => x.Horinicio <= horaInicio && x.Horfin >= horaFin).FirstOrDefault();

                    if (itemDato != null)
                    {
                        valor = Convert.ToDecimal(itemDato.Ofdeprecio);
                    }

                    itemResult.ListaValores.Add(valor);
                }

                result.Add(itemResult);
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Listado de elementos del XML
    /// </summary>
    public class RsfEstructura
    {
        public int Equicodi { get; set; }
        public string IdCentral { get; set; }
        public string IdUnidad { get; set; }
        public int Periodo { get; set; }
        public decimal Valor { get; set; }
        public List<RsfEstructura> ListaValores { get; set; }
        public List<decimal> Mediciones { get; set; }
        public List<decimal> MedicionesUp { get; set; }
        public List<decimal> MedicionesDown { get; set; }
        public string IndicadorDatos { get; set; }
        public int Grupocodi { get; set; }
        public decimal? RsfUp { get; set; }
        public decimal? RsfDown { get; set; }
        public int Equipadre { get; set; }
        public int Famcodi { get; set; }
    }

    public class RsfLimite
    {
        public string Codigo { get; set; }
        public string LimiteMin { get; set; }
        public string LimiteMax { get; set; }
        public int Indice { get; set; }
        public string Tipo { get; set; }
        public int Contador { get; set; }
        public int Asignacion { get; set; }
        public int TipoGrupo { get; set; }
        public int Equicodi { get; set; }
        public string Nombre { get; set; }
    }

    public class RsfXml
    {
        public string CentralAgc { get; set; }
        public string GrupoAgc { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Valor { get; set; }
        public List<RsfXml> ListaValores { get; set; }
    }

    public class RsfGrafico
    {
        public int Equicodi { get; set; }
        public string Nombre { get; set; }
        public List<int> Elementos { get; set; }
        public decimal Load { get; set; }
        public decimal RsfUp { get; set; }
        public decimal RsfDown { get; set; }
        public List<RsfGrafico> ListaValores { get; set; }
        public List<string> ListaHoras { get; set; }

    }

    public class PrecioGrafico
    {
        public string Nombre { get; set; }
        public List<decimal> ListaValores { get; set; }
    }


}
