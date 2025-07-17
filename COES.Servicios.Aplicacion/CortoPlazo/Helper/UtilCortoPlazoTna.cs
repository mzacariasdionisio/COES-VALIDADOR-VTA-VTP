using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo.Helper
{
    public class UtilCortoPlazoTna
    {
        /// <summary>
        /// Permite obtener la fuente de la congestión
        /// </summary>
        /// <param name="caso"></param>
        /// <returns></returns>
        public static string ObtenerFuenteCongestion(int caso)
        {
            string texto = string.Empty;
            switch (caso)
            {
                case ConstantesCortoPlazo.IdConjuntoLinea:
                    {
                        texto = ConstantesCortoPlazo.TxtConjuntoLinea;
                        break;
                    }
                case ConstantesCortoPlazo.IdLineaTransmision:
                    {
                        texto = ConstantesCortoPlazo.TxtLineaTransmision;
                        break;
                    }
                case ConstantesCortoPlazo.IdTrafo2D:
                    {
                        texto = ConstantesCortoPlazo.TxtTrafo2D;
                        break;
                    }
                case ConstantesCortoPlazo.IdTrafo3D:
                    {
                        texto = ConstantesCortoPlazo.TxtTrafo3D;
                        break;
                    }
            }

            return texto;
        }

        /// <summary>
        /// Calcula el periodo en el que se está ejecutando el proceso
        /// </summary>
        /// <returns></returns>
        public static int CalcularPeriodo(DateTime fecha)
        {
            int totalMinutes = fecha.Hour * 60 + fecha.Minute;
            return Convert.ToInt32(Math.Ceiling(((decimal)totalMinutes / 30.0M)));
        }

        /// <summary>
        /// Permite obtener el rango de horas de congestion programada
        /// </summary>
        /// <param name="fecha">Fecha de consulta de datos</param>
        /// <param name="periodo">Nro de media hora en el dia seleccionado</param>
        /// <param name="tipo">1. Extremo izquierdo del rango, 0. Extremo derecho del rango</param>
        /// <returns></returns>
        public static DateTime ObtenerRangoFecha(DateTime fecha, int periodo, int tipo)
        {
            DateTime fechaHora = new DateTime(fecha.Year, fecha.Month, fecha.Day);

            if (periodo < 48)
                return fechaHora.AddMinutes((periodo - tipo) * 30);
            else
                return fechaHora.AddMinutes((periodo - tipo) * 30 - 1);
        }

        /// <summary>
        /// Obtiene la curva de consumo
        /// </summary>
        /// <param name="puntos"></param>
        /// <returns></returns>
        public static List<CoordenadaConsumo> ObtenerCurvaConsumo(List<PrGrupodatDTO> puntos)
        {
            CoordenadaConsumo coordenada1 = ObtenerCoordenada(puntos, ConstantesCortoPlazo.PX1, ConstantesCortoPlazo.PY1);
            CoordenadaConsumo coordenada2 = ObtenerCoordenada(puntos, ConstantesCortoPlazo.PX2, ConstantesCortoPlazo.PY2);
            CoordenadaConsumo coordenada3 = ObtenerCoordenada(puntos, ConstantesCortoPlazo.PX3, ConstantesCortoPlazo.PY3);
            CoordenadaConsumo coordenada4 = ObtenerCoordenada(puntos, ConstantesCortoPlazo.PX4, ConstantesCortoPlazo.PY4);
            CoordenadaConsumo coordenada5 = ObtenerCoordenada(puntos, ConstantesCortoPlazo.PX5, ConstantesCortoPlazo.PY5);

            List<CoordenadaConsumo> result = new List<CoordenadaConsumo>();
            if (coordenada1 != null) result.Add(coordenada1);
            if (coordenada2 != null) result.Add(coordenada2);
            if (coordenada3 != null) result.Add(coordenada3);
            if (coordenada4 != null) result.Add(coordenada4);
            if (coordenada5 != null) result.Add(coordenada5);

            if (result.Count == 1)
            {
                result.Add(new CoordenadaConsumo { Consumo = 0, Potencia = 0 });
            }

            return result.OrderBy(x => x.Potencia).ToList();
        }

        /// <summary>
        /// Permite obtener las coordenadas de la curva de consumo
        /// </summary>
        /// <param name="puntos"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <returns></returns>
        public static CoordenadaConsumo ObtenerCoordenada(List<PrGrupodatDTO> puntos, int coorX, int coorY)
        {
            bool flag = true;
            decimal puntoX = 0;
            decimal puntoY = 0;

            string posX = puntos.Where(x => x.Concepcodi == coorX).Select(x => x.Formuladat).FirstOrDefault();
            string posY = puntos.Where(x => x.Concepcodi == coorY).Select(x => x.Formuladat).FirstOrDefault();

            if (!string.IsNullOrEmpty(posX) && !string.IsNullOrEmpty(posY))
            {
                if (decimal.TryParse(posX, out puntoX) && decimal.TryParse(posY, out puntoY))
                {
                    flag = true;
                }
            }

            if (flag)
            {
                return new CoordenadaConsumo { Potencia = puntoX, Consumo = puntoY };
            }

            return null;
        }

        /// <summary>
        /// Permite obtener el código asociado a un nombre de barra
        /// </summary>
        /// <param name="nomBarra"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ObtenerCodigoBarra(string codBarra, List<NombreCodigoBarra> list, out string tension)
        {            
            tension = string.Empty;
            string nombreBarra = string.Empty;

            if (codBarra != null)
            {
               
                foreach (NombreCodigoBarra item in list)
                {
                    if (item.CodBarra.Trim().ToUpper() == codBarra.Trim().ToUpper())
                    {                        
                        tension = item.Tension;
                        nombreBarra = item.NombBarra;
                        break;
                    }
                }
            }

            return nombreBarra;
        }

        /// <summary>
        /// Permite obtener valores del archivo de entrada .raw 0: Potencia Máxima 1: Indicador de Operación
        /// </summary>
        /// <param name="barra"></param>
        /// <param name="id"></param>        
        /// <param name="matriz"></param>
        /// <returns></returns>
        public static string[] ObtenerValorPSSE(string nombretna, List<string[]> matriz)
        {
            string[] resultado = new string[3];

            foreach (string[] item in matriz)
            {
                if (item[item.Length - 1].Trim() == nombretna.Trim())
                {
                    resultado[0] = item[2].Trim();
                    resultado[1] = item[14].Trim();
                    resultado[2] = item[0].Trim(); //-Codigo de barra                    
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el valor del consumo para el caso de las centrales hidraúlicas
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="periodo">Entre 1 y 48</param>
        /// <returns></returns>
        public static string ObtenerValorAgua(string nombre, int periodo, List<string[]> matriz, double tipoCambio, DateTime fechaProceso)
        {
            string resultado = string.Empty;
            int indice = matriz.Count - (48 - (periodo - 1));

            if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
            {
                indice = matriz.Count - 1;
            }

            if (indice >= 0)
            {
                if (matriz[indice].Length > 0)
                {
                    int posicion = 0;
                    foreach (string item in matriz[0])
                    {
                        if (item.Trim().ToLower() == nombre.Trim().ToLower()) break;
                        posicion++;
                    }

                    if (posicion < matriz[indice].Length)
                    {
                        resultado = matriz[indice][posicion].Trim();
                    }
                }
            }

            decimal valor = 0;
            if (decimal.TryParse(resultado, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
            {
                if (!(valor != 0))
                {
                    resultado = string.Empty;
                }
                else
                {
                    if (valor < 0)
                    {
                        resultado = 0.ToString();
                    }
                    else
                    {
                        resultado = (valor * (decimal)tipoCambio).ToString("0.000");
                    }
                }
            }

            return resultado;
        }


        /// <summary>
        /// Permite obtener el volumen programado de una central
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="periodo">Entre 1 y 48</param>
        /// <returns></returns>
        public static decimal ObtenerVolumenProgramado(string nombre, int periodo, List<string[]> matriz, DateTime fechaProceso)
        {
            string resultado = string.Empty;
            int indice = matriz.Count - (48 - (periodo - 1));

            if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
            {
                indice = matriz.Count - 1;
            }

            if (indice >= 0)
            {
                if (matriz[indice].Length > 0)
                {
                    int posicion = 0;
                    foreach (string item in matriz[0])
                    {
                        if (item.Trim().ToLower() == nombre.Trim().ToLower()) break;
                        posicion++;
                    }

                    if (posicion < matriz[indice].Length)
                    {
                        resultado = matriz[indice][posicion].Trim();
                    }
                }
            }

            decimal valor = 0;
            if (decimal.TryParse(resultado, NumberStyles.Any, CultureInfo.InvariantCulture, out valor)) { }

            return valor;
        }

        /// <summary>
        /// Permite obtener el listado de objetos Linea
        /// </summary>
        /// <param name="linea">Líneas</param>
        /// <returns></returns>
        public static List<LineaEms> ObtenerObjetoLinea(List<NombreCodigoLinea> linea)
        {
            List<LineaEms> objetoLinea = new List<LineaEms>();

            foreach (NombreCodigoLinea item in linea)
            {
                LineaEms lineaEms = new LineaEms(item.CodBarra1, item.CodBarra2, item.BitEstado, 0, 1, item.NombLinea, item.Rps, item.Xps, item.Bsh, item.GshP, item.BshP, item.GshS,
                    item.BshS, item.BitEstado, item.VoltajePU1, item.Angulo1, item.VoltajePU2, item.Angulo2, item.Pot, 1, 1, item.Nombretna);

                objetoLinea.Add(lineaEms);
            }

            return objetoLinea;
        }

        /// <summary>
        /// Permite obtener las cargas del archivo ODMS
        /// </summary>
        /// <param name="fileName">NOmbre de archivo</param>
        /// <param name="path">Ruta de archivo</param>
        /// <param name="relacionBarra">Listado de Barras (código tem y Nombre Barra)</param>
        /// <returns>Listado de cargas</returns>
        public static List<Carga> ObtenerCargas(string fileName, string path, List<NombreCodigoBarra> relacionBarra)
        {
            List<Carga> list = new List<Carga>();

            List<string[]> datosDemanda = FileHelperTna.ObtenerDatosDemanda(fileName, path);

            foreach (string[] demanda in datosDemanda)
            {
                var oBarra = relacionBarra.FirstOrDefault(x => x.CodBarra.Trim() == demanda[0].Trim()); //Se busca el código de barra para obtener nombre de barra
                Carga carga = new Carga();
                carga.CodCarga = demanda[0].Trim();
                carga.Carga1 = Convert.ToDouble(demanda[1]);
                carga.Carga2 = Convert.ToDouble(demanda[2]);
                carga.Conn = Convert.ToInt32(demanda[3]);
                if (oBarra != null) carga.NomBarra = oBarra.NombBarra;
                list.Add(carga);
            }


            return list;
        }

        /// <summary>
        /// Permite obtener los shunt del archivo ODMS
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>        
        /// <returns></returns>
        public static List<Shunt> ObtenerShunts(string fileName, string path)
        {
            List<Shunt> list = new List<Shunt>();

            List<string[]> datosShunt = FileHelperTna.ObtenerDatosShunt(fileName, path);

            foreach (string[] dato in datosShunt)
            {
                Shunt shunt = new Shunt();
                shunt.CodShunt = dato[0].Trim();
                shunt.Carga = Convert.ToDouble(dato[1]); // I
                shunt.CargaAdicional = Convert.ToDouble(dato[2]); //R
                shunt.Conn = Convert.ToInt32(dato[3]); // Conn

                list.Add(shunt);
            }


            return list;
        }

        /// <summary>
        /// Permite leer la estructura de shunts dinámicos
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<SwitchedShunt> ObtenerShuntsDinamicos(string fileName, string path)
        {
            List<SwitchedShunt> list = new List<SwitchedShunt>();

            List<string[]> datosShunt = FileHelper.ObtenerDatosShuntDinamicos(fileName, path);

            foreach (string[] dato in datosShunt)
            {
                SwitchedShunt shunt = new SwitchedShunt();
                shunt.Codigo = dato[0].Trim();
                shunt.Val = Convert.ToDouble(dato[1]);
                shunt.Min = Convert.ToDouble(dato[2]);
                shunt.Max = Convert.ToDouble(dato[3]);
                shunt.Conn = Convert.ToInt32(dato[4]);
                double vv = Convert.ToInt32(dato[5]);

                if (vv != 2)
                {
                    shunt.Min = shunt.Val;
                    shunt.Max = shunt.Val;
                }
                else
                {
                    if (shunt.Max < shunt.Min)
                    {
                        shunt.Max = shunt.Min;
                        shunt.Min = 0;
                    }
                }

                int count = 0;
                foreach (SwitchedShunt item in list)
                {
                    if (shunt.Codigo == item.Codigo)
                    {
                        count++;
                    }
                }

                shunt.N = count;
                list.Add(shunt);
            }


            return list;
        }

        /// <summary>
        /// Permite obtener la generación del archivo ODMS
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public static List<Generator> ObtenerGeneracion(List<string[]> datos)
        {
            List<Generator> result = new List<Generator>();

            foreach (string[] dato in datos)
            {
                Generator genen = new Generator();
                genen.Codbarra = dato[0].Trim();
                genen.ID = dato[1].Replace("'", "").Trim();
                genen.Qmax = Convert.ToDouble(dato[4]);
                genen.Qmin = Convert.ToDouble(dato[5]);
                genen.Qg = Convert.ToDouble(dato[3]);
                genen.Pg = Convert.ToDouble(dato[2]);
                genen.Ope = Convert.ToInt32(dato[14]);                

                result.Add(genen);
            }

            return result;
        }

        #region Region_seguridad

        /// <summary>
        /// Migrado desde VBA
        /// </summary>
        /// <summary>    
        public static string GenerarEntradaGams(List<Carga> listaCarga, List<NombreCodigoBarra> listaBarra, List<Shunt> listaShunt,
            List<LineaEms> listaLinea, List<TrafoEms> listaTrafo, List<RegistroGenerado> listaGenerador,
            List<PrGenforzadaDTO> listaGenFor, List<PrCongestionDTO> listaCongestion, List<PrCongestionDTO> listaCongestionGrupo,
            List<PrCongestionDTO> listaCongestionRegionSeguridad, List<NombreCodigoBarra> relacionBarra)
        {
            double pot;//, g, b;
            double r12, x12, r23, x23, r13, x13;
            string aux1;

            foreach (Carga item in listaCarga)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.CodCarga).FirstOrDefault();

                if (barra != null)
                {
                    barra.barraPc += item.Carga1;
                }
            }

            string logCarga = "";
            foreach (Carga item in listaCarga)
            {
                logCarga += item.CodCarga + "," + item.Carga1 + "\r\n";
            }

            foreach (Shunt item in listaShunt)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.CodShunt).FirstOrDefault();

                if (barra != null)
                {
                    barra.barraPc += item.CargaAdicional;
                    barra.barraShunt += item.Carga;
                }
            }

            string logShunt = "";
            foreach (Shunt item in listaShunt)
            {
                logShunt += item.CodShunt + "," + item.Carga + "\r\n";
            }


            int lineaTrafo2d = 0;
            int lineaTrafo3d = 0;

            //List<TrafoEms> listaTrafo = new List<TrafoEms>();
            foreach (TrafoEms item in listaTrafo)
            {
                NombreCodigoBarra barra1 = listaBarra.Where(x => x.CodBarra == item.IdBarra1).FirstOrDefault();
                NombreCodigoBarra barra2 = listaBarra.Where(x => x.CodBarra == item.IdBarra2).FirstOrDefault();
                NombreCodigoBarra barra3 = listaBarra.Where(x => x.CodBarra == item.IdBarra3).FirstOrDefault();

                item.Barra1 = (barra1 != null) ? barra1 : null;
                item.Barra2 = (barra2 != null) ? barra2 : null;
                item.Barra3 = (barra3 != null) ? barra3 : null;

                double g = item.DatoTrafo[0, 7];
                double b = item.DatoTrafo[0, 8];

                if (item.IdBarra3 == "0")
                {
                    LineaEms linea = new LineaEms(item.IdBarra1, item.IdBarra2, Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                    item.DatoTrafo[1, 0], item.DatoTrafo[1, 1], 0, item.DatoTrafo[0, 7], item.DatoTrafo[0, 8], 0, 0, 0, 0, 0, 0, 0, item.DatoTrafo[1, 2], 0, 0, item.NombreTna);
                    linea.Barra1 = item.Barra1;
                    linea.Barra2 = item.Barra2;

                    listaLinea.Add(linea);
                    lineaTrafo2d++;
                }
                else
                {
                    //pos = pos + 1
                    r12 = item.DatoTrafo[1, 0]; //Worksheets(nombre).Cells(pos, 1)
                    x12 = item.DatoTrafo[1, 1]; //Worksheets(nombre).Cells(pos, 2)
                    r23 = item.DatoTrafo[1, 3]; //Worksheets(nombre).Cells(pos, 4)
                    x23 = item.DatoTrafo[1, 4]; //Worksheets(nombre).Cells(pos, 5)
                    r13 = item.DatoTrafo[1, 6]; //Worksheets(nombre).Cells(pos, 7)
                    x13 = item.DatoTrafo[1, 7]; //Worksheets(nombre).Cells(pos, 8)

                    LineaEms linea1 = new LineaEms(item.IdBarra1, item.IdBarra2, Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                        r12, x12, 0, g / 3, b / 3, 0, 0, 0,
                        0, 0, 0, 0, item.DatoTrafo[1, 2], 0, 0, item.NombreTna);

                    linea1.Barra1 = item.Barra1;
                    linea1.Barra2 = item.Barra2;

                    listaLinea.Add(linea1);
                    lineaTrafo3d++;

                    LineaEms linea2 = new LineaEms(item.IdBarra1, item.IdBarra3, Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                        r13, x13, 0, g / 3, b / 3, 0, 0, 0,
                        0, 0, 0, 0, item.DatoTrafo[1, 5], 0, 0, item.NombreTna);

                    linea2.Barra1 = item.Barra1;
                    linea2.Barra2 = item.Barra3;

                    listaLinea.Add(linea2);
                    lineaTrafo3d++;

                    LineaEms linea3 = new LineaEms(item.IdBarra2, item.IdBarra3, Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                       r23, x23, 0, g / 3, b / 3, 0, 0, 0,
                       0, 0, 0, 0, item.DatoTrafo[1, 8], 0, 0, item.NombreTna);

                    linea3.Barra1 = item.Barra2;
                    linea3.Barra2 = item.Barra3;

                    listaLinea.Add(linea3);
                    lineaTrafo3d++;
                }
            }


            string logLinea2 = "";
            foreach (LineaEms item in listaLinea)
            {
                logLinea2 += item.IdBarra1 + "," + item.IdBarra2 + "," + item.Orden + "," +
                    (item.Barra1 == null ? "NULL" : item.Barra1.CodBarra) + "," +
                        (item.Barra2 == null ? "NULL" : item.Barra2.CodBarra) +
                    "\r\n";
            }

            //'***************************************************
            //' Inicio de lectura archivo GEN
            //'***************************************************
            List<RegistroGenerado> cicloComb = ObtenerCicloCombinado(listaGenerador);

            //'Detectar las centrales en ciclo combinado
            string txtcc = "";
            for (int i = 1; i < listaGenerador.Count; i++)
            {
                RegistroGenerado regGen = listaGenerador[i];

                for (int j = 0; j <= i - 1; j++)
                {
                    RegistroGenerado regGen2 = listaGenerador[j];

                    if (regGen.Cod == regGen2.Cod)
                    {
                        regGen.NroCicloCombinado = regGen2.Cod;
                        regGen2.NroCicloCombinado = regGen2.Cod;
                        txtcc += regGen2.Cod + "\r\n";
                    }
                }
            }

            int nciclocomb = -1;
            int[] tempo = new int[listaGenerador.Count];

            int MAXCICLOCOMB = 100;
            int[,] ciclocomb = new int[MAXCICLOCOMB, 20];
            int[] ciclocombn = new int[MAXCICLOCOMB];


            for (int i = 0; i < listaGenerador.Count; i++)
            {
                tempo[i] = 1;
            }

            string txtccomb = "";
            for (int i = 0; i < listaGenerador.Count; i++)
            {
                RegistroGenerado item = listaGenerador[i];

                if ((item.NroCicloCombinado != 0) && (tempo[i] == 1))
                {
                    tempo[i] = 0;
                    nciclocomb = nciclocomb + 1;
                    ciclocombn[nciclocomb] = 0;
                    ciclocomb[nciclocomb, ciclocombn[nciclocomb]] = i;

                    txtccomb = txtccomb + "ciclocomb[" + nciclocomb + "," + ciclocombn[nciclocomb] + "]=" + i + "~";

                    for (int j = i + 1; j < listaGenerador.Count; j++)
                    {
                        RegistroGenerado item2 = listaGenerador[j];

                        if ((item.NroCicloCombinado != 0) && (tempo[j] == 1))
                        {
                            if (item2.NroCicloCombinado == item.NroCicloCombinado)
                            {
                                tempo[j] = 0;
                                ciclocombn[nciclocomb] = ciclocombn[nciclocomb] + 1;
                                txtccomb = txtccomb + "ciclocombn[" + nciclocomb + "]=" + (ciclocombn[nciclocomb] + 1) + "~";

                                ciclocomb[nciclocomb, ciclocombn[nciclocomb]] = j;
                                txtccomb = txtccomb + "ciclocombn[" + nciclocomb + "," + ciclocombn[nciclocomb] + "]=" + j + "\r\n";

                            }
                        }
                    }
                }
            }

            //- Pendiente de analizar esta parte por cambio de tna
            for (int i = 0; i <= nciclocomb; i++)
            {
                for (int j = 0; j <= ciclocombn[i] - 1; j++)
                {
                    int idx1 = ciclocomb[i, j];
                    int idx2 = ciclocomb[i, j + 1];

                    RegistroGenerado itemGenerador1 = listaGenerador[idx1];
                    RegistroGenerado itemGenerador2 = listaGenerador[idx2];

                    LineaEms lineaCc = new LineaEms(
                                            itemGenerador1.BarraID.ToString(),
                                            itemGenerador2.BarraID.ToString(),
                                            1,
                                            1,
                                            1,
                                            "CC",
                                            0, 0.01, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 100, 0, 0,
                                            "Creado por CC");

                    listaLinea.Add(lineaCc);
                }
            }

            foreach (LineaEms item in listaLinea)
            {
                NombreCodigoBarra barra1 = listaBarra.Where(x => x.CodBarra == item.IdBarra1).FirstOrDefault();
                NombreCodigoBarra barra2 = listaBarra.Where(x => x.CodBarra == item.IdBarra2).FirstOrDefault();


                item.Barra1 = (barra1 != null) ? barra1 : null;
                item.Barra2 = (barra2 != null) ? barra2 : null;
            }

            string logLinea = "";
            foreach (LineaEms item in listaLinea)
            {
                logLinea += item.IdBarra1 + "," + item.IdBarra2 + "," + item.Orden + "," +
                    (item.Barra1 == null ? "NULL" : item.Barra1.CodBarra) + "," +
                        (item.Barra2 == null ? "NULL" : item.Barra2.CodBarra) +
                    "\r\n";
            }

            string listaLin = "";

            try
            {
                foreach (LineaEms item in listaLinea)
                {
                    listaLin += item.Barra1.CodBarra + "," + item.Barra1.NombBarra + "," +
                        item.Barra2.CodBarra + "," + item.Barra2.NombBarra + "\r\n";
                }
            }
            catch (Exception e1)
            {
                string e12 = e1.ToString();
            }

            //'Detectar que barras estan sin conexion alguna, esto trae errores la determinacion de Shift Factors (inversion de matriz singular)
            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.conect = false;
            }

            string logBarra21 = "";
            foreach (NombreCodigoBarra item in listaBarra)
            {
                logBarra21 += item.CodBarra + "," + item.NombBarra + "," + item.Id + "," + item.conect + "\r\n";
            }

            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    item.Barra1.conect = true;
                    item.Barra2.conect = true;

                }
            }

            string logBarra2 = "";
            foreach (NombreCodigoBarra item in listaBarra)
            {
                logBarra2 += item.CodBarra + "," + item.NombBarra + "," + item.Id + "," + item.conect + "\r\n";
            }
            //'******************************************
            //' identificacion de sistemas aislados
            //'******************************************
            int nsis, nsisaux1, nsisaux2;
            int nsistema;
            int[] sistema = new int[1000];
            nsis = 0;//numero de sistema

            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.sis = 0;
            }

            nsis = 0;
            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    if (item.Barra1.sis == 0 && item.Barra2.sis == 0)
                    {
                        nsis = nsis + 1;
                        item.Barra1.sis = nsis;
                        item.Barra2.sis = nsis;
                    }
                    else
                    {
                        if (item.Barra1.sis != 0 && item.Barra2.sis != 0)
                        {
                            nsisaux1 = item.Barra1.sis;
                            nsisaux2 = item.Barra2.sis;
                            if (item.Barra2.sis < nsisaux1)
                            {
                                nsisaux1 = item.Barra2.sis;
                                nsisaux2 = item.Barra1.sis;
                            }
                            foreach (NombreCodigoBarra item2 in listaBarra)
                            {
                                if (item2.sis == nsisaux2)
                                {
                                    item2.sis = nsisaux1;
                                }
                            }
                        }
                        else
                        {
                            if (item.Barra1.sis != 0 && item.Barra2.sis == 0)
                            {
                                item.Barra2.sis = item.Barra1.sis;
                            }
                            else
                            {
                                if (item.Barra2.sis != 0 && item.Barra1.sis == 0)
                                {
                                    item.Barra1.sis = item.Barra2.sis;
                                }
                            }
                        }
                    }
                }
            }

            //'normalizar numero de sistema e identificar cuantos sistemas hay
            nsistema = 0;// -1;
            for (int i = 1; i < 1000; i++)//for (int i = 0; i < 50; i++)
            {
                sistema[i] = 0;
            }

            int esta;

            foreach (NombreCodigoBarra item in listaBarra)
            {
                nsisaux1 = item.sis;
                esta = 0;
                for (int j = 1; j <= nsistema; j++)//for (int j = 0; j <= nsistema; j++)
                {
                    if (esta == 0)
                    {
                        if (sistema[j] == nsisaux1)
                        {
                            esta = 1;
                        }
                    }
                }
                if (esta == 0)
                {
                    nsistema = nsistema + 1;
                    sistema[nsistema] = nsisaux1;
                }
            }

            for (int i = 1; i <= nsistema; i++)//for (int i = 0; i <= nsistema; i++)
            {
                if (sistema[i] != 0)
                {
                    foreach (NombreCodigoBarra item in listaBarra)
                    {
                        if (item.sis == sistema[i])
                        {
                            item.sis = i;
                        }
                    }
                    sistema[i] = i;
                }
            }


            if (sistema[nsistema] == 0)
            {
                nsistema = nsistema - 1;
            }


            //'Todas las barras que no estan conectadas a nada, estan figurando en un sistema 0.  cambio2
            //'Se deben unir en un solo sistema 0, unirla a la primera que encuentre              cambio2

            //int esta = 0;                              //'cambio2
            NombreCodigoBarra barraAislado = null;

            foreach (NombreCodigoBarra barra in listaBarra)                   //'cambio2
            {
                if (barra.sis == 0)
                {              //'cambio2
                    if (barraAislado == null)
                    {            //'cambio2
                        barraAislado = barra;                  //'cambio2
                    }
                    else
                    {                               //'cambio2
                        //nlin = nlin + 1           //'cambio2

                        LineaEms linea = new LineaEms("01", "", 1, 1, 3, "", 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 0, 0, "Linea por SA");

                        linea.Barra1 = barraAislado;        //'cambio2
                        linea.Barra2 = barra;           //'cambio2
                        listaLinea.Add(linea);



                    }                        //'cambio2
                }                            //'cambio2
            }                                  //'cambio2



            //'crear un enlace entre sistemas aislados con X muy grande.
            //'De esta forma el optimizador solucionara un solo problema pero con resultados independientes

            if (nsistema > 1)//if (nsistema > 0)
            {
                for (int i = 2; i <= nsistema; i++)//for (int i = 1; i <= nsistema; i++)
                {
                    LineaEms linea = new LineaEms("01", "", 1, 1, 3, "", 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 0, 0, "Linea por SA1");
                    listaLinea.Add(linea);

                    esta = 0;

                    foreach (NombreCodigoBarra item in listaBarra)
                    {
                        if (esta == 0 && item.sis == sistema[i])
                        {
                            linea.Barra1 = item;
                            esta = 1;
                            break;
                        }
                    }

                    esta = 0;
                    foreach (NombreCodigoBarra item in listaBarra)
                    {
                        if (esta == 0 && item.sis == sistema[i - 1])
                        {
                            linea.Barra2 = item;
                            esta = 1;
                            break;
                        }
                    }
                }
            }

            //'*******************************************
            //'   ESCRITURA DE ARCHIVO PARA GAMS
            //'*******************************************
            //nombre2 = nombre & sistema(s) & ".DAT"
            string log = "";
            bool esCicloComb;
            //Open nombre2 For Output As #1

            AgregaLinea(ref log, "*********************************************");

            AgregaLinea(ref log, "* Barras");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET NB /");

            int idLinea = 0;
            string logGamLin = "";
            foreach (LineaEms item in listaLinea)
            {
                try
                {
                    item.Id = ++idLinea;
                    logGamLin += item.Id + "," + item.IdBarra1 + "," + item.IdBarra2 + "," + item.Orden + "," + item.LinIslin + "\r\n";
                }
                catch (Exception e)
                {
                }
            }

            int idBarra = 0;
            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.Id = ++idBarra;
            }

            foreach (NombreCodigoBarra item in listaBarra)
            {
                //if (item.barraPc != 0)
                //if (item.conect && item.barraPc != 0)
                //if (item.conect)
                //if (!(item.VoltajePU == 1 && item.Angulo == 0)) //- Linea modificada 05022019
                //{
                AgregaLinea(ref log, "        B" + String.Format("{0:000}", item.Id) + " " + item.NombBarra);
                //}
            }
            AgregaLinea(ref log, "/;");

            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Demandas");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "TABLE Demanda(NB,*)");
            AgregaLinea(ref log, "                Pc");
            foreach (NombreCodigoBarra item in listaBarra)
            {
                //if (!(item.VoltajePU == 1 && item.Angulo == 0)) //- Linea modificada 05022019
                //{
                if (item.barraPc != 0)
                {
                    AgregaLinea(ref log, "        B" + String.Format("{0:000}", item.Id) + " " + String.Format("{0:0.000}", item.barraPc));
                }
                //}
            }
            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Termica");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET  GT  /");

            //*** PENDIENTE

            string logGen = "";

            foreach (RegistroGenerado item in listaGenerador)
            {
                esCicloComb = listaGenerador.Where(x => x.Cod == item.Cod).Count() > 1;
                item.EsCicloCombinado = esCicloComb;

                logGen += item.BarraID + "," + item.BarraNombre + "\r\n";
            }

            string logBarra = "";
            foreach (NombreCodigoBarra item in listaBarra)
            {
                logBarra += item.CodBarra + "," + item.NombBarra + "," + item.Id + "," + item.conect + "\r\n";
            }

            string logTermica = "";

            int cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                item.Correlativo = ++cod;
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                string nombreBarra = (barra != null) ? barra.NombBarra : "";
                //bool condicion = ((item.Tipo == "T") && (item.IndOpe == 1) && conectado && (item.Ccombcodi == null ? 0 : item.Ccombcodi) == 0);
                bool condicion = ((item.Tipo == "T") && (item.IndOpe == 1) && conectado && (!item.EsCicloCombinado));
                //bool condicion = ((item.Tipo == "T") && (item.IndOpe == 1) && conectado);
                //if ((item.Tipo == "T") && (item.IndOpe == 1) && conectado)

                //if ((item.Tipo == "T") && (item.IndOpe == 1) && conectado && (item.Ccombcodi == null ? 0 : item.Ccombcodi) == 0)                

                //if ((item.Tipo == "T") && (item.IndOpe == 1) && conectado && (!item.EsCicloCombinado))
                if ((item.Tipo == "T") && (item.IndOpe == 1) && conectado && (!item.EsCicloCombinado) && item.Potencia != 0)
                {
                    AgregaLinea(ref log, "        T" + String.Format("{0:000}", item.Correlativo) + " " + item.Nombretna); // nombreBarra + " " + item.GenerID);
                }

                logTermica += item.Correlativo + "," + nombreBarra + ",GenerId," + item.GenerID + ",Tipo," +
                item.Tipo + ",Indope," + item.IndOpe + ",conectado," + conectado +
                ",condicion," + condicion + ",combcodi," +
                 (item.Ccombcodi == null ? -1000 : item.Ccombcodi) + ",escicloComb," + item.EsCicloCombinado +
                 ",Potencia," + item.Potencia + ",Minima," + item.PotenciaMinima + ",Maxima," + item.PotenciaMaxima +
                 "\r\n";
            }

            //List<RegistroGenerado> cicloComb = ObtenerCicloCombinado(listaGenerador);

            string logCicloC = "";
            int corrcc = 1;
            //' ciclo combinados, agarra la primera unidad como referencia
            foreach (RegistroGenerado item in cicloComb)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                string nombreBarra = (barra != null) ? barra.NombBarra : "";
                bool condicion = item.Tipo == "T" && item.IndOpe == 1 && conectado;

                if (item.Tipo == "T" && item.IndOpe == 1 && conectado)
                {
                    AgregaLinea(ref log, "        C" + String.Format("{0:000}", corrcc) + " " + item.Nombretna);// nombreBarra + " " + item.GenerID);
                }
                logCicloC += corrcc + "," + nombreBarra + ",GenerId," + item.GenerID + ",Tipo," +
                item.Tipo + ",Indope," + item.IndOpe + ",conectado," + conectado + ",condicion," + condicion + "\r\n";
                corrcc++;
            }


            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "SET  PtBus(GT,NB) /");

            string logPtBus = "";
            foreach (RegistroGenerado item in listaGenerador)
            {
                //bool esCicloComb = cicloComb.Where(x => x.Cod == item.Cod).Count() > 0;
                //item.EsCicloCombinado = esCicloComb;
                esCicloComb = item.EsCicloCombinado;

                string temp1 = "";
                string temp2 = "";
                if (!esCicloComb)
                {
                    NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                    bool conectado = (barra != null) ? barra.conect : false;
                    string nombreBarra = (barra != null) ? barra.NombBarra : "";
                    int barraID = (barra != null) ? barra.Id : 0;
                    temp1 = barraID.ToString();
                    //if (item.Tipo == "T" && item.IndOpe == 1 && conectado)
                    temp2 = ",Tipo," + item.Tipo + " " +
                        "Indope," + item.IndOpe + " " +
                            "conectado," + conectado + " " +
                    "cccodi," + (item.Ccombcodi == null ? 0 : item.Ccombcodi);
                    //if (item.Tipo == "T" && item.IndOpe == 1 && conectado && (item.Ccombcodi == null ? 0 : item.Ccombcodi) == 0)
                    if (item.Tipo == "T" && item.IndOpe == 1 && conectado && (!item.EsCicloCombinado))
                    {
                        AgregaLinea(ref log, "        T" + String.Format("{0:000}", item.Correlativo) + ".B" + String.Format("{0:000}", barraID));
                    }
                }

                logPtBus += "T" + item.Correlativo + ",Barra," + temp1 + ",esCicloComb" + esCicloComb + temp2 + "\r\n";
            }

            corrcc = 1;

            //' ciclo combinados, agarra la primera unidad como referencia
            foreach (RegistroGenerado item in cicloComb)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                string nombreBarra = (barra != null) ? barra.NombBarra : "";
                int barraID = (barra != null) ? barra.Id : 0;

                if (item.Tipo == "T" && item.IndOpe == 1 && conectado)
                {
                    AgregaLinea(ref log, "        C" + String.Format("{0:000}", corrcc) + ".B" + String.Format("{0:000}", barraID));
                }

                corrcc++;
            }

            List<PrGenforzadaDTO> listaForzada = new List<PrGenforzadaDTO>();

            foreach (PrGenforzadaDTO itemForzada in listaGenFor)
            {
                foreach (PrgenforzadaitemDTO subItemForzada in itemForzada.ListaItems)
                {
                    if (!string.IsNullOrEmpty(itemForzada.Subcausacmg))
                        listaForzada.Add(new PrGenforzadaDTO { Codbarra = subItemForzada.Codbarra, Idgener = subItemForzada.Idgenerador, Nombretna = subItemForzada.Nombretna });
                }
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "TABLE PtData(GT,*)");
            AgregaLinea(ref log, "                Pgen        Pmin      Pmax       Costo1     CI1        CI2       Pmax1       Pmax2   Forzada     Sist   Calif");

            foreach (RegistroGenerado item in listaGenerador)
            {
                //indexcount++;
                // Ajustado segun macro
                if (item.IndNcv == 1.ToString())
                {
                    item.Ci2 = item.Ci1;
                    item.Pmax2 = item.Pmax1;
                }

                if (true) //indexcount > 1)
                {

                    //bool esCicloComb = cicloComb.Where(x => x.Cod == item.Cod).Count() > 0;
                    //item.EsCicloCombinado = esCicloComb;
                    esCicloComb = item.EsCicloCombinado;

                    if (!esCicloComb)
                    {
                        NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                        bool conectado = (barra != null) ? barra.conect : false;
                        string nombreBarra = (barra != null) ? barra.NombBarra : "";
                        int barraID = (barra != null) ? barra.Id : 0;
                        int barraSis = (barra != null) ? barra.sis : 0;
                        string genFor = "no ";
                        string genCalif = item.IdCalificacion.ToString();



                        PrGenforzadaDTO genfor = listaForzada.Where(x => x.Nombretna == item.Nombretna).FirstOrDefault();
                        genFor = (genfor != null) ? "yes" : "no ";

                        //if (item.Tipo == "T" && item.IndOpe == 1 && conectado)
                        //if (item.Tipo == "T" && item.IndOpe == 1 && conectado && (item.Ccombcodi == null ? 0 : item.Ccombcodi) == 0)
                        if (item.Tipo == "T" && item.IndOpe == 1 && conectado)
                        {
                            item.IndRegistro = 1;
                            AgregaLinea(ref log, "        T" + String.Format("{0:000}", item.Correlativo) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.Potencia.ToString(), 5, 3) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.PotenciaMinima.ToString(), 5, 3) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.PotenciaMaxima.ToString(), 5, 3) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.Costo1.ToString(), 6, 2) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci1, 4, 4) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci2, 4, 4) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.Pmax1, 5, 3) +
                            "  " + UtilCortoPlazo.FormatearValorAdicional(item.Pmax2, 5, 3) +
                            "     " + UtilCortoPlazo.FormatearCadena(genFor, 3) +
                            "        " + UtilCortoPlazo.FormatearCadena((barraSis).ToString(), 1) +
                            "     " + UtilCortoPlazo.FormatearCadena(genCalif, 1));
                        }
                        // }
                    }
                }
            }

            //}
            //catch (Exception ex)
            //{
            //    int s = indexcount;
            //}

            try
            {
                string logCicloComb = "";
                corrcc = 1;
                //' ciclo combinados, agarra la primera unidad como referencia
                foreach (RegistroGenerado item in cicloComb)
                {
                    NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                    bool conectado = (barra != null) ? barra.conect : false;
                    string nombreBarra = (barra != null) ? barra.NombBarra : "";
                    int barraID = (barra != null) ? barra.Id : 0;
                    bool condicion = (item.Tipo == "T" && item.IndOpe == 1 && conectado && item.Potencia != 0);
                    logCicloComb += "C" + item.Correlativo + "," + item.Potencia + "," + item.PotenciaMinima + ","
                        + item.PotenciaMaxima + "," + item.Cod + ","
                        + item.Tipo + "," + item.IndOpe + "," + conectado + "," + item.Potencia +
                        "\r\n";

                    decimal pg = 0;
                    decimal pmin = 0;
                    decimal pmax = 0;
                    string genFor = "no ";
                    PrGenforzadaDTO genfor = listaForzada.Where(x => x.Codbarra == item.BarraID.ToString()).FirstOrDefault();
                    genFor = (genfor != null) ? "yes" : "no ";

                    NombreCodigoBarra barra2 = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                    int barraSis = (barra2 != null) ? barra2.sis : 0;
                    string genCalif = item.IdCalificacion.ToString();

                    item.IndRegistro = 1;

                    foreach (RegistroGenerado item2 in listaGenerador)
                    {
                        if (item.Cod == item2.Cod)
                        {
                            pg = item2.Potencia;//pg += item2.Potencia;
                            pmin = item2.PotenciaMinima;//pmin += item2.PotenciaMinima;

                            decimal pmax1 = item2.PotenciaMaxima;

                            if (item2.PGen > pmax1)
                            {
                                pmax1 = item2.PGen;
                            }

                            if (pmax1 < item2.PotenciaMinima)
                            {
                                pmax1 = item2.PotenciaMinima;
                            }

                            pmax = pmax1;//pmax += pmax1;
                        }
                    }

                    AgregaLinea(ref log, "        C" + String.Format("{0:000}", corrcc) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(pg.ToString(), 5, 3) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(pmin.ToString(), 5, 3) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(pmax.ToString(), 5, 3) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(item.Costo1.ToString(), 6, 2) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci1, 4, 4) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci2, 4, 4) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(item.Pmax1, 5, 3) +
                                  "  " + UtilCortoPlazo.FormatearValorAdicional(item.Pmax2, 5, 3) +
                                  "     " + UtilCortoPlazo.FormatearCadena(genFor, 3) +
                                  "        " + UtilCortoPlazo.FormatearCadena(barraSis.ToString(), 1) +
                                  "     " + UtilCortoPlazo.FormatearCadena(genCalif, 1));

                    corrcc++;
                }
            }
            catch (Exception e11)
            {
                e11.ToString();
            }

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");

            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Hidraulica");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET  GH  /");

            foreach (RegistroGenerado item in listaGenerador)
            {

                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                string nombreBarra = (barra != null) ? barra.NombBarra : "";

                if ((item.Tipo == "H") && (item.IndOpe == 1) && (conectado) && (item.Potencia > 0))
                {

                    AgregaLinea(ref log, "        H" + String.Format("{0:000}", item.Correlativo) + " " + item.Nombretna); // nombreBarra + " " + item.GenerID);
                }
            }
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "SET  PhBus(GH,NB) /");

            foreach (RegistroGenerado item in listaGenerador)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                if ((item.Tipo == "H") && (item.IndOpe == 1) && (conectado) && (item.Potencia > 0))
                {
                    AgregaLinea(ref log, "        H" + String.Format("{0:000}", item.Correlativo) + ".B" + String.Format("{0:000}", barra.Id));
                }
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "TABLE PhData(GH,*)");
            AgregaLinea(ref log, "                Pgen       Pmin      Pmax        CI         Forzada      Sist");

            foreach (RegistroGenerado item in listaGenerador)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                int sistema2 = (barra != null) ? barra.sis : 0;

                string genFor = "no ";

                PrGenforzadaDTO genfor = listaForzada.Where(x => x.Nombretna == item.Nombretna).FirstOrDefault();
                genFor = (genfor != null) ? "yes" : "no ";

                if ((item.Tipo == "H") && (item.IndOpe == 1) && (conectado) && (item.Potencia > 0))
                {
                    AgregaLinea(ref log, "        H" + String.Format("{0:000}", item.Correlativo) +
                        "  " + UtilCortoPlazo.FormatearValorAdicional(item.Potencia.ToString(), 5, 3) +
                        "  " + UtilCortoPlazo.FormatearValorAdicional(item.PotenciaMinima.ToString(), 5, 3) +
                        "  " + UtilCortoPlazo.FormatearValorAdicional(item.PotenciaMaxima.ToString(), 5, 3) +
                        "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci1, 4, 4) +
                        "        " + UtilCortoPlazo.FormatearCadena(genFor, 3) +
                        "        " + UtilCortoPlazo.FormatearCadena((sistema2).ToString(), 1));
                }
            }

            // *** FIN PENDIENTE ***

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Enlaces");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET  ENL  /");

            string logLinea3 = "";

            foreach (LineaEms item in listaLinea)
            {

                if (item.LinOp == "yes")
                {
                    if (item.LinIslin != 3)
                    {
                        //AgregaLinea(ref log, "        E" + String.Format("{0:0000}", item.Id) + "    " + item.Barra1.NombBarra + "-" + item.Barra2.NombBarra);
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) + "    " + item.Barra1.NombBarra + "-" + item.Barra2.NombBarra + " *" + item.NombreTna);
                        logLinea3 += "(1)," + item.Id + "," + item.Barra1.NombBarra + "," + item.Barra2.NombBarra + "\r\n";
                    }
                    else
                    {
                        //AgregaLinea(ref log, "        E" + String.Format("{0:0000}", item.Id) + "    Ficticio para sistemas aislados");
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) + "    Ficticio para sistemas aislados" + " *" + item.NombreTna);
                        logLinea3 += "(2)," + item.Id + "," + "    Ficticio para sistemas aislados" + "\r\n";
                    }
                }
                else
                {
                    logLinea3 += "(3)," + item.Id + "," + item.Barra1.NombBarra + "," + item.Barra2.NombBarra + "," + item.LinOp + "," + item.BitEstado + ",NO EXISTE\r\n";
                }
            }

            string logLineaEnlace = "";
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "SET  FBus(ENL,NB,NB) /");

            int countd = 0;
            try
            {
                foreach (LineaEms item in listaLinea)
                {
                    countd++;

                    //if (countd == 1209)
                    //{
                    //if (item.LinOp == "yes")
                    if (item.LinOp == "yes")// && item.LinIslin != 3)
                    {
                        //AgregaLinea(ref log, "        E" + String.Format("{0:0000}", item.Id) + ".B" + String.Format("{0:000}", item.Barra1.Id) + ".B" + String.Format("{0:000}", item.Barra2.Id));

                        if (item.Barra2 == null)
                        {
                            item.Barra2 = new NombreCodigoBarra();
                            item.Barra2.Id = 0;
                        }
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) + ".B" + String.Format("{0:000}", item.Barra1.Id) + ".B" + String.Format("{0:000}", item.Barra2.Id) );
                    }
                    logLineaEnlace += item.Id + "," + item.Barra1.Id + "," + item.Barra2.Id + "," + item.LinOp + "\r\n";
                    //}
                }
            }
            catch
            {
                string s = countd.ToString();
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "TABLE  FData(ENL,*)");
            AgregaLinea(ref log, "                   R0       X0         G0      Pmax     Cong  Sist");

            List<EqCongestionConfigDTO> congestion = new List<EqCongestionConfigDTO>();
            congestion = FactorySic.GetEqCongestionConfigRepository().List();

            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    aux1 = "no ";
                    pot = item.LinPot;

                    foreach (PrCongestionDTO item2 in listaCongestion)
                    {
                        //NombreCodigoBarra barra1 = relacionBarra.Where(x => x.NombBarra == item2.Nodobarra1 && item2.Nodobarra1 != null).FirstOrDefault();
                        //NombreCodigoBarra barra2 = relacionBarra.Where(x => x.NombBarra == item2.Nodobarra2 && item2.Nodobarra2 != null).FirstOrDefault();

                        if (
                            item.NombreTna == item2.Nombretna
                            //((item.IdBarra1 == barra1.CodBarra && item.IdBarra2 == barra2.CodBarra) ||
                            //(item.IdBarra1 == barra2.CodBarra && item.IdBarra2 == barra1.CodBarra))
                            //&& item.Orden == item2.NombLinea
                            )

                        {
                            aux1 = "yes";
                            pot = item2.Flujo;
                        }
                    }

                    if (item.LinIslin != 3)
                    {
                        string r = (item.Rps == null ? 0 : item.Rps).ToString();
                                               
                        AgregaLinea(ref log, "        E" +
                           String.Format("{0:000}", item.Id) +
                        "    " + UtilCortoPlazoTna.FormatearValorAdicionalLinea(r.ToString(), 1, 6) +
                        "  " + UtilCortoPlazoTna.FormatearValorAdicionalLinea(item.Xps.ToString(), 1, 6) +
                        "  " + UtilCortoPlazoTna.FormatearValorAdicionalLinea((item.Gshp + item.Gshs).ToString(), 1, 5) +
                        "  " + UtilCortoPlazoTna.FormatearValorAdicional(pot.ToString(), 4, 3) +
                        "    " + UtilCortoPlazoTna.FormatearCadena(aux1, 3) +
                        "    " + (item.Barra1.sis).ToString());
                        
                    }
                    else
                    {
                        string r = (item.Rps == null ? 0 : item.Rps).ToString();

                        //'enlace ficticio entres sistemas aislados
                        AgregaLinea(ref log, "        E" +
                             String.Format("{0:000}", item.Id) +
                         "    " + UtilCortoPlazoTna.FormatearValorAdicional(r.ToString(), 1, 6) +
                         "  " + UtilCortoPlazoTna.FormatearValorAdicional(item.Xps.ToString(), 8, 0) +
                         "  " + UtilCortoPlazoTna.FormatearValorAdicional((item.Gshp + item.Gshs).ToString(), 1, 5) +
                         "  " + UtilCortoPlazoTna.FormatearValorAdicional(pot.ToString(), 4, 3) +
                         "    " + UtilCortoPlazo.FormatearCadena(aux1, 3) +
                         "    " + (item.Barra1.sis).ToString());
                    }
                }
            }
            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");

            // *** FIN PENDIENTE ***

            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Congestion de varios enlaces en simultaneo");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET CONG /");

            int numgrupoLinea = 0;

            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                numgrupoLinea++;
                string lineanomb = "       C" + String.Format("{0:000}", numgrupoLinea) + "    ";
                string nombreconjunto = string.Empty;

                foreach (PrCongestionitemDTO itemCongestion in item.ListaItems)
                {
                    if (itemCongestion.Nombarra1 != null && itemCongestion.Nombarra1.Trim().Length > 0)
                    {
                        lineanomb += itemCongestion.Nombarra1.Trim() + "-";
                        nombreconjunto += itemCongestion.Nombarra1.Trim() + "-";
                    }
                }

                #region Mejoras CMgN
                item.NombreResultado = nombreconjunto;
                #endregion

                AgregaLinea(ref log, lineanomb);
            }

            //ncongc
            if (numgrupoLinea == 0)
            {
                AgregaLinea(ref log, "C1 CONGESTION L1");
            }
            AgregaLinea(ref log, "        /;");

            AgregaLinea(ref log, "SET CONGRuta(CONG,ENL)");
            AgregaLinea(ref log, "      /");

            numgrupoLinea = 0;
            int esta1 = 0;
            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                numgrupoLinea++;
                string lincjt = "       C" + String.Format("{0:000}", numgrupoLinea) + ".(";

                int j = 1;

                foreach (PrCongestionitemDTO itemCongestion in item.ListaItems)
                {
                    esta1 = 0;
                    int k = 1;
                    foreach (LineaEms lin in listaLinea)
                    {
                        //if (itemCongestion.Nombarra1 != null && itemCongestion.Nombarra2 != null)
                        //{
                        //    if ((lin.Barra1.NombBarra == itemCongestion.Nombarra1 && lin.Barra2.NombBarra == itemCongestion.Nombarra2)
                        //        ||
                        //        (lin.Barra2.NombBarra == itemCongestion.Nombarra1 && lin.Barra1.NombBarra == itemCongestion.Nombarra2))
                        //    {
                        //        esta1 = lin.Id;
                        //    }
                        //}
                        if (itemCongestion.Nombretna == lin.NombreTna)
                        {
                            esta1 = lin.Id;
                        }
                        k++;
                    }

                    lincjt += "E" + String.Format("{0:000}", esta1);

                    if (j != item.ListaItems.Count)
                    {
                        lincjt += ",";
                    }
                    else
                    {
                        lincjt += ")";
                    }
                    j++;
                }
                AgregaLinea(ref log, lincjt);
            }

            if (numgrupoLinea == 0)
            {
                //AgregaLinea(ref log, "C1.(E001)");
                //'cambio3   buscar la primera linea conectada
                esta = 0;                                            //'cambio3
                foreach (LineaEms lineaN in listaLinea)
                {                                 //'cambio3
                    if ((lineaN.LinOp == "yes") && (esta == 0))
                    {       //'cambio3
                        esta = lineaN.Id;                                    //'cambio3
                    }                                          //'cambio3
                }                                                //'cambio3

                AgregaLinea(ref log, "C1.(E00" + esta + ")");                    //'cambio3

            }

            AgregaLinea(ref log, "      /;");

            AgregaLinea(ref log, "TABLE CONGmax(CONG,*)");
            AgregaLinea(ref log, "                 Pmax     Cong");


            numgrupoLinea = 0;
            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                numgrupoLinea++;
                string lincjt = "       C" + String.Format("{0:000}", numgrupoLinea) + "    " + String.Format("{0:0000.000}", item.Flujo) + "   yes";
                AgregaLinea(ref log, lincjt);
            }

            if (numgrupoLinea == 0)
            {
                AgregaLinea(ref log, "              C1  130     no  ");
            }

            AgregaLinea(ref log, ";");


            #region CambioRS29122020

            AgregaLinea(ref log, " ");

            List<PrCongestionDTO> regionArriba = listaCongestionRegionSeguridad.Where(x => x.Regsegdirec == 1.ToString()).ToList();

            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Congestion por área de seguridad hacia arriba");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET CONG_AS_ARRIBA /");

            if (regionArriba.Count > 0)
            {

                int numRegionSeguridad = 0;

                bool flagGH = false;
                bool flagGT = false;

                foreach (PrCongestionDTO item in regionArriba)
                {
                    numRegionSeguridad++;
                    string lineanomb = "       C_AS_ARRIBA" + String.Format("{0:000}", numRegionSeguridad) + "    " + item.Regsegnombre;

                    #region Mejoras CMgN
                    item.NombreResultado = item.Regsegnombre;
                    #endregion

                    AgregaLinea(ref log, lineanomb);


                    List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                  x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorHidraulico &&
                  listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                  ).ToList();

                    if (listSubItem.Count > 0) flagGH = true;

                    listSubItem = item.ListaItems.Where(x =>
                   x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorTermico &&
                   listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                   ).ToList();

                    if (listSubItem.Count > 0) flagGT = true;

                }
                AgregaLinea(ref log, "      /;");


                AgregaLinea(ref log, "SET CONGRuta_AS_ARRIBA_ENL(CONG_AS_ARRIBA,ENL) /");

                numRegionSeguridad = 0;

                foreach (PrCongestionDTO item in regionArriba)
                {
                    numRegionSeguridad++;

                    List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                    x.TipoEquipo == ConstantesCortoPlazo.IdLineaTransmision &&
                    listaLinea.Any(y => x.Nombretna == y.NombreTna && y.LinOp == "yes")).ToList();

                    if (listSubItem.Count > 0)
                    {
                        string lineanomb = "       C_AS_ARRIBA" + String.Format("{0:000}", numRegionSeguridad) + ".(";


                        int indexEquipo = 0;
                        foreach (PrCongestionitemDTO itemEquipo in listSubItem)
                        {
                            LineaEms itemLinea = listaLinea.Where(x => x.NombreTna == itemEquipo.Nombretna).FirstOrDefault();

                            if (itemLinea != null)
                            {
                                lineanomb = lineanomb + "E" + String.Format("{0:000}", itemLinea.Id) + ((indexEquipo < listSubItem.Count - 1) ? "," : ")");
                            }
                            indexEquipo++;
                        }

                        AgregaLinea(ref log, lineanomb);
                    }
                }

                AgregaLinea(ref log, "      /;");


                if (flagGH)
                {
                    AgregaLinea(ref log, "SET CONGRuta_AS_ARRIBA_GH(CONG_AS_ARRIBA,GH) /");


                    numRegionSeguridad = 0;

                    foreach (PrCongestionDTO item in regionArriba)
                    {
                        numRegionSeguridad++;

                        List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                        x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorHidraulico &&
                        listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                        ).ToList();

                        if (listSubItem.Count > 0)
                        {

                            string lineanomb = "       C_AS_ARRIBA" + String.Format("{0:000}", numRegionSeguridad) + ".(";

                            int indexEquipo = 0;
                            foreach (PrCongestionitemDTO itemEquipo in listSubItem)
                            {
                                RegistroGenerado itemGenerador = listaGenerador.Where(x => x.Nombretna == itemEquipo.Nombretna).FirstOrDefault();

                                if (itemGenerador != null)
                                {
                                    lineanomb = lineanomb + "H" + String.Format("{0:000}", itemGenerador.Correlativo) + ((indexEquipo < listSubItem.Count - 1) ? "," : ")");
                                }
                                indexEquipo++;
                            }

                            AgregaLinea(ref log, lineanomb);
                        }
                    }


                    AgregaLinea(ref log, "      /;");
                }

                if (flagGT)
                {

                    AgregaLinea(ref log, "SET CONGRuta_AS_ARRIBA_GT(CONG_AS_ARRIBA,GT) /");

                    numRegionSeguridad = 0;

                    foreach (PrCongestionDTO item in regionArriba)
                    {
                        numRegionSeguridad++;

                        List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                      x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorTermico &&
                      listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                      ).ToList();

                        if (listSubItem.Count > 0)
                        {

                            string lineanomb = "       C_AS_ARRIBA" + String.Format("{0:000}", numRegionSeguridad) + ".(";


                            int indexEquipo = 0;
                            foreach (PrCongestionitemDTO itemEquipo in listSubItem)
                            {
                                RegistroGenerado itemGenerador = listaGenerador.Where(x => x.Nombretna == itemEquipo.Nombretna).FirstOrDefault();

                                if (itemGenerador != null)
                                {
                                    lineanomb = lineanomb + "T" + String.Format("{0:000}", itemGenerador.Correlativo) + ((indexEquipo < listSubItem.Count - 1) ? "," : ")");
                                }
                                indexEquipo++;
                            }

                            AgregaLinea(ref log, lineanomb);
                        }
                    }

                    AgregaLinea(ref log, "      /;");

                }
                AgregaLinea(ref log, "TABLE CONGmax_AS_ARRIBA(CONG_AS_ARRIBA,*)");
                AgregaLinea(ref log, "                           Pmax     	Cong       	m            b      	  GenT          FlujoT");


                numRegionSeguridad = 0;

                foreach (PrCongestionDTO item in regionArriba)
                {
                    numRegionSeguridad++;
                    string lineanomb = "       C_AS_ARRIBA" + String.Format("{0:000}", numRegionSeguridad) + "      ";
                    lineanomb = lineanomb + String.Format("{0:0000.000}", item.Pmax) + "      " + "yes" + "      " +
                         String.Format("{0:0000.000}", item.Regsegvalorm) + "      " + String.Format("{0:0000.000}", item.ParamB)
                          +"      " + String.Format("{0:0000.000}", item.GenT) + "      " + String.Format("{0:0000.000}", item.FlujoT);
                    AgregaLinea(ref log, lineanomb);
                }

                AgregaLinea(ref log, ";");

            }
            else
            {
                LineaEms lineaEms = listaLinea.Where(y => y.LinOp == "yes").First();
                RegistroGenerado generadorEms = listaGenerador.Where(x => x.IndOpe == 1 && x.Tipo == "H").First();

                AgregaLinea(ref log, "       C_AS_ARRIBA001    L1_AR");
                AgregaLinea(ref log, "      /;");
                AgregaLinea(ref log, "SET CONGRuta_AS_ARRIBA_ENL(CONG_AS_ARRIBA,ENL) /");
                AgregaLinea(ref log, "       C_AS_ARRIBA001.(E" + String.Format("{0:000}", lineaEms.Id)+ ")");
                AgregaLinea(ref log, "      /;");
                AgregaLinea(ref log, "SET CONGRuta_AS_ARRIBA_GH(CONG_AS_ARRIBA,GH) /");
                AgregaLinea(ref log, "       C_AS_ARRIBA001.(H" + String.Format("{0:000}", generadorEms.Correlativo) + ")");
                AgregaLinea(ref log, "      /;");
                AgregaLinea(ref log, "TABLE CONGmax_AS_ARRIBA(CONG_AS_ARRIBA,*)");
                AgregaLinea(ref log, "                          Pmax     Cong       m          b      GenT   FlujoT");
                AgregaLinea(ref log, "       C_AS_ARRIBA001      1000     no        30      -30000    1000   1000");
                AgregaLinea(ref log, ";");

                // regiones hacia arriba
            }

            AgregaLinea(ref log, " ");

            List<PrCongestionDTO> regionAbajo = listaCongestionRegionSeguridad.Where(x => x.Regsegdirec == 2.ToString()).ToList();

            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Congestion por área de seguridad hacia abajo");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET CONG_AS_ABAJO /");

            if (regionAbajo.Count > 0)
            {

                int numRegionSeguridad = 0;
                bool flagGH = false;
                bool flagGT = false; // ajuste 23122020
                foreach (PrCongestionDTO item in regionAbajo)
                {
                    numRegionSeguridad++;
                    string lineanomb = "       C_AS_ABAJO" + String.Format("{0:000}", numRegionSeguridad) + "    " + item.Regsegnombre;
                    #region Mejoras CMgN
                    item.NombreResultado = item.Regsegnombre;
                    #endregion
                    AgregaLinea(ref log, lineanomb);

                    List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                 x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorHidraulico &&
                 listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                 ).ToList();

                    if (listSubItem.Count > 0) flagGH = true;

                    listSubItem = item.ListaItems.Where(x =>
                   x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorTermico &&
                   listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                   ).ToList();

                    if (listSubItem.Count > 0) flagGT = true;
                }
                AgregaLinea(ref log, "      /;");


                AgregaLinea(ref log, "SET CONGRuta_AS_ABAJO_ENL(CONG_AS_ABAJO,ENL) /");

                numRegionSeguridad = 0;

                foreach (PrCongestionDTO item in regionAbajo)
                {
                    numRegionSeguridad++;

                    List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                   x.TipoEquipo == ConstantesCortoPlazo.IdLineaTransmision &&
                   listaLinea.Any(y => x.Nombretna == y.NombreTna && y.LinOp == "yes")).ToList();

                    if (listSubItem.Count > 0)
                    {
                        string lineanomb = "       C_AS_ABAJO" + String.Format("{0:000}", numRegionSeguridad) + ".(";

                        int indexEquipo = 0;
                        foreach (PrCongestionitemDTO itemEquipo in listSubItem)
                        {
                            LineaEms itemLinea = listaLinea.Where(x => x.NombreTna == itemEquipo.Nombretna).FirstOrDefault();

                            if (itemLinea != null)
                            {
                                lineanomb = lineanomb + "E" + String.Format("{0:000}", itemLinea.Id) + ((indexEquipo < listSubItem.Count - 1) ? "," : ")");
                            }
                            indexEquipo++;
                        }

                        AgregaLinea(ref log, lineanomb);
                    }
                }

                AgregaLinea(ref log, "      /;");


                if (flagGH)
                {

                    AgregaLinea(ref log, "SET CONGRuta_AS_ABAJO_GH(CONG_AS_ABAJO,GH) /");


                    numRegionSeguridad = 0;

                    foreach (PrCongestionDTO item in regionAbajo)
                    {
                        numRegionSeguridad++;

                        List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
                 x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorHidraulico &&
                 listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
                 ).ToList();

                        if (listSubItem.Count > 0)
                        {

                            string lineanomb = "       C_AS_ABAJO" + String.Format("{0:000}", numRegionSeguridad) + ".(";


                            int indexEquipo = 0;
                            foreach (PrCongestionitemDTO itemEquipo in listSubItem)
                            {
                                RegistroGenerado itemGenerador = listaGenerador.Where(x => x.Nombretna == itemEquipo.Nombretna).FirstOrDefault();

                                if (itemGenerador != null)
                                {
                                    lineanomb = lineanomb + "H" + String.Format("{0:000}", itemGenerador.Correlativo) + ((indexEquipo < listSubItem.Count - 1) ? "," : ")");
                                }
                                indexEquipo++;
                            }

                            AgregaLinea(ref log, lineanomb);
                        }
                    }


                    AgregaLinea(ref log, "      /;");
                }

                if (flagGT)
                {

                    AgregaLinea(ref log, "SET CONGRuta_AS_ABAJO_GT(CONG_AS_ABAJO,GT) /");

                    numRegionSeguridad = 0;

                    foreach (PrCongestionDTO item in regionAbajo)
                    {
                        numRegionSeguridad++;

                        List<PrCongestionitemDTO> listSubItem = item.ListaItems.Where(x =>
               x.TipoEquipo == ConstantesCortoPlazo.TipoGeneradorTermico &&
               listaGenerador.Any(y => y.Nombretna == x.Nombretna && y.IndOpe == 1)
               ).ToList();

                        if (listSubItem.Count > 0)
                        {
                            string lineanomb = "       C_AS_ABAJO" + String.Format("{0:000}", numRegionSeguridad) + ".(";



                            int indexEquipo = 0;
                            foreach (PrCongestionitemDTO itemEquipo in listSubItem)
                            {
                                RegistroGenerado itemGenerador = listaGenerador.Where(x => x.Nombretna == itemEquipo.Nombretna).FirstOrDefault();

                                if (itemGenerador != null)
                                {
                                    lineanomb = lineanomb + "T" + String.Format("{0:000}", itemGenerador.Correlativo) + ((indexEquipo < listSubItem.Count - 1) ? "," : ")");
                                }
                                indexEquipo++;
                            }

                            AgregaLinea(ref log, lineanomb);
                        }

                    }

                    AgregaLinea(ref log, "      /;");
                }

                AgregaLinea(ref log, "TABLE CONGmax_AS_ABAJO(CONG_AS_ABAJO,*)");
                AgregaLinea(ref log, "                           Pmax     	Cong       	m            b      	  GenT          FlujoT");


                numRegionSeguridad = 0;

                foreach (PrCongestionDTO item in regionAbajo)
                {
                    numRegionSeguridad++;
                    string lineanomb = "       C_AS_ABAJO" + String.Format("{0:000}", numRegionSeguridad) + "      ";
                    lineanomb = lineanomb + String.Format("{0:0000.000}", item.Pmax) + "      " + "yes" + "      " +
                         String.Format("{0:0000.000}", item.Regsegvalorm) + "      " + String.Format("{0:0000.000}", item.ParamB)
                          + "      " + String.Format("{0:0000.000}", item.GenT) + "      " + String.Format("{0:0000.000}", item.FlujoT);
                    AgregaLinea(ref log, lineanomb);
                }

                AgregaLinea(ref log, ";");
            }
            else
            {
                LineaEms lineaEms = listaLinea.Where(y => y.LinOp == "yes").First();
                RegistroGenerado generadorEms = listaGenerador.Where(x => x.IndOpe == 1 && x.Tipo == "H").First();

                AgregaLinea(ref log, "       C_AS_ABAJO001    L1_AB");
                AgregaLinea(ref log, "      /;");
                AgregaLinea(ref log, "SET CONGRuta_AS_ABAJO_ENL(CONG_AS_ABAJO,ENL) /");
                AgregaLinea(ref log, "       C_AS_ABAJO001.(E" + String.Format("{0:000}", lineaEms.Id) + ")");
                AgregaLinea(ref log, "      /;");
                AgregaLinea(ref log, "SET CONGRuta_AS_ABAJO_GH(CONG_AS_ABAJO,GH) /");
                AgregaLinea(ref log, "       C_AS_ABAJO001.(H" + String.Format("{0:000}", generadorEms.Correlativo) + ")");
                AgregaLinea(ref log, "      /;");
                AgregaLinea(ref log, "TABLE CONGmax_AS_ABAJO(CONG_AS_ABAJO,*)");
                AgregaLinea(ref log, "                          Pmax     Cong       m          b      GenT   FlujoT");
                AgregaLinea(ref log, "       C_AS_ABAJO001      1000     no        30      -30000    1000   1000");
                //- Regiones hacia abajo
            }

            #endregion

            return log;
        }

        #endregion

        /// <summary>
        /// Permite generar el archivo de entrada GAMS para AC
        /// </summary>
        /// <param name="listaCarga"></param>
        /// <param name="listaBarra"></param>
        /// <param name="listaShunt"></param>
        /// <param name="listaLinea"></param>
        /// <param name="listaTrafo"></param>
        /// <param name="listaGenerador"></param>
        /// <param name="listaGenFor"></param>
        /// <param name="listaCongestion"></param>
        /// <param name="listaCongestionGrupo"></param>
        /// <param name="relacionBarra"></param>
        /// <param name="listaGeneracionEMS"></param>
        /// <param name="listShuntDinamico"></param>
        /// <returns></returns>
        public static string GenerarEntradaGamsAC(List<Carga> listaCarga, List<NombreCodigoBarra> listaBarraTotal, List<Shunt> listaShunt,
           List<LineaEms> listaLineaTotal, List<TrafoEms> listaTrafoTotal, List<RegistroGenerado> listaGeneradorTotal,
           List<PrGenforzadaDTO> listaGenFor, List<PrCongestionDTO> listaCongestion, List<PrCongestionDTO> listaCongestionGrupo, List<NombreCodigoBarra> relacionBarra,
           List<Generator> listaGeneracionEMSTotal, List<SwitchedShunt> listShuntDinamicoTotal)
        {

            #region Preparación de datos
            List<RegistroGenerado> listaGenerador = listaGeneradorTotal.Where(x => x.IndOpe == 1).ToList();
            List<NombreCodigoBarra> listaBarra = listaBarraTotal.Where(x => !(x.VoltajePU == 1 && x.Angulo == 0)).ToList();
            List<Generator> listaGeneracionEMS = listaGeneracionEMSTotal.Where(x => x.Ope == 1).ToList();
            List<LineaEms> listaLinea = listaLineaTotal.Where(x => x.LinOp == "yes").ToList();
            List<TrafoEms> listaTrafo = listaTrafoTotal.Where(x => x.Op == "yes").ToList();
            List<SwitchedShunt> listShuntDinamico = listShuntDinamicoTotal.Where(x => x.Conn == 1).ToList();

            double pot;//, g, b;            
            string aux1;

            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.barraPc = 0;
                item.barraQc = 0;
                item.barraShunt = 0;
                item.barraShunt = 0;
                item.barraShuntI = 0;
                item.barraShuntR = 0;
            }

            foreach (Carga item in listaCarga)
            {
                if (item.Conn == 1)
                {
                    NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.CodCarga).FirstOrDefault();

                    if (barra != null)
                    {
                        barra.barraPc += item.Carga1;
                        barra.barraQc += item.Carga2;
                    }
                }
            }

            foreach (Shunt item in listaShunt)
            {
                if (item.Conn == 1)
                {
                    NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.CodShunt).FirstOrDefault();

                    if (barra != null)
                    {
                        barra.barraPc += item.CargaAdicional; //R
                        barra.barraShunt += item.Carga; // I

                        barra.barraShuntI += item.Carga;
                        barra.barraShuntR += item.CargaAdicional;
                    }
                }
            }

            int lineaTrafo2d = 0;
            int lineaTrafo3d = 0;

            //List<TrafoEms> listaTrafo = new List<TrafoEms>();

            int codigoBarra = -1000;
            foreach (TrafoEms item in listaTrafo)
            {
                NombreCodigoBarra barra1 = listaBarra.Where(x => x.CodBarra == item.IdBarra1).FirstOrDefault();
                NombreCodigoBarra barra2 = listaBarra.Where(x => x.CodBarra == item.IdBarra2).FirstOrDefault();
                NombreCodigoBarra barra3 = listaBarra.Where(x => x.CodBarra == item.IdBarra3).FirstOrDefault();


                item.Barra1 = (barra1 != null) ? barra1 : null;
                item.Barra2 = (barra2 != null) ? barra2 : null;
                item.Barra3 = (barra3 != null) ? barra3 : null;


                if (item.IdBarra3 == "0")
                {
                    LineaEms linea = new LineaEms(item.IdBarra1, item.IdBarra2, Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                        item.R1, item.X1, 0, item.G1, item.B1, 0, 0, 0, 0, 0, 0, 0, item.Pot1, item.Tap1, item.Tap2, item.NombreTna);
                    linea.Barra1 = item.Barra1;
                    linea.Barra2 = item.Barra2;
                    listaLinea.Add(linea);
                    lineaTrafo2d++;
                }
                else
                {
                    codigoBarra++;
                    NombreCodigoBarra barraAdicional = new NombreCodigoBarra();
                    barraAdicional.CodBarra = codigoBarra.ToString();
                    barraAdicional.NombBarra = barra1.NombBarra + "-F";
                    barraAdicional.Tension = barra1.Tension;
                    barraAdicional.VoltajePU = item.Vf1;
                    barraAdicional.Angulo = item.Angf1;
                    barraAdicional.barraPc = 0;
                    barraAdicional.barraQc = 0;

                    listaBarra.Add(barraAdicional);

                    LineaEms linea1 = new LineaEms(item.IdBarra1, codigoBarra.ToString(), Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                         item.R1, item.X1, 0, item.G1, item.B1, 0, 0, 0, 0, 0, 0, 0, item.Pot1, item.Tap1, 1, item.NombreTna);

                    linea1.Barra1 = item.Barra1;
                    linea1.Barra2 = barraAdicional;

                    listaLinea.Add(linea1);
                    lineaTrafo3d++;

                    LineaEms linea2 = new LineaEms(item.IdBarra2, codigoBarra.ToString(), Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                         item.R2, item.X2, 0, item.G1, item.B1, 0, 0, 0, 0, 0, 0, 0, item.Pot2, item.Tap2, 1, item.NombreTna);

                    linea2.Barra1 = item.Barra2;
                    linea2.Barra2 = barraAdicional;

                    listaLinea.Add(linea2);
                    lineaTrafo3d++;

                    LineaEms linea3 = new LineaEms(item.IdBarra3, codigoBarra.ToString(), Convert.ToInt32(item.DatoTrafo[0, 13]), 1, 0, item.Orden,
                        item.R3, item.X3, 0, item.G1, item.B1, 0, 0, 0, 0, 0, 0, 0, item.Pot3, item.Tap3, 1, item.NombreTna);

                    linea3.Barra1 = item.Barra3;
                    linea3.Barra2 = barraAdicional;

                    listaLinea.Add(linea3);
                    lineaTrafo3d++;

                }
            }

            foreach (RegistroGenerado item in listaGenerador)
            {
                bool esCicloComb1 = listaGenerador.Where(x => x.Cod == item.Cod).Count() > 1;
                item.EsCicloCombinado = esCicloComb1;

            }


            foreach (Generator generator in listaGeneracionEMS)
            {
                generator.Traslado = 0;

                List<RegistroGenerado> subList = listaGenerador.Where(x => (x.BarraID.ToString() == generator.Codbarra.Trim() ||
                                                                            x.BarraID2.ToString() == generator.Codbarra.Trim()) &&
                                                                            x.GenerID == generator.ID).ToList();

                //if (subList.Count() > 0)
                //{
                //    generator.Traslado = 1;
                //}

                foreach (RegistroGenerado item in listaGenerador)
                {
                    if ((item.BarraID.ToString() == generator.Codbarra.Trim() && item.GenerID == generator.ID) ||
                        (item.BarraID2.ToString() == generator.Codbarra.Trim() && item.GenerID == generator.ID))
                    {
                        item.Qmin = generator.Qmin;
                        item.Qmax = generator.Qmax;
                        item.Qg = generator.Qg;
                        item.Potencia = (decimal)generator.Pg;
                        generator.Traslado = 1;
                    }
                }
            }

            List<RegistroGenerado> listGenAdicional = new List<RegistroGenerado>();
            foreach (Generator generator in listaGeneracionEMS)
            {
                if (generator.Traslado == 0)
                {
                    RegistroGenerado itemGenerado = new RegistroGenerado();
                    itemGenerado.BarraID = Convert.ToInt32(generator.Codbarra.Trim());//esta faltando el nombre
                    itemGenerado.GenerID = generator.ID;
                    itemGenerado.Qmax = generator.Qmax;
                    itemGenerado.Qmin = generator.Qmin;
                    itemGenerado.Qg = generator.Qg;
                    itemGenerado.Potencia = (decimal)generator.Pg;
                    itemGenerado.Pmax = generator.Pg.ToString();
                    itemGenerado.Pmin = generator.Pg.ToString();
                    itemGenerado.PGen = (decimal)generator.Pg;
                    itemGenerado.IndOpe = generator.Ope;
                    itemGenerado.IndNcv = 1.ToString();
                    itemGenerado.Ci1 = 0.ToString();
                    itemGenerado.Ci2 = 0.ToString();
                    itemGenerado.Pmax1 = generator.Pg.ToString();
                    itemGenerado.Pmax2 = generator.Pg.ToString();
                    itemGenerado.Tipo = "N";

                    if (generator.Qg > generator.Qmax)
                    {
                        itemGenerado.Qmax = generator.Qg;
                    }
                    if (generator.Qg < generator.Qmin)
                    {
                        itemGenerado.Qmin = generator.Qg;
                    }

                    listGenAdicional.Add(itemGenerado);
                }
            }

            listaGenerador.AddRange(listGenAdicional);


            //'***************************************************
            //' Inicio de lectura archivo GEN
            //'***************************************************
            List<RegistroGenerado> cicloComb = ObtenerCicloCombinado(listaGenerador);

            //'Detectar las centrales en ciclo combinado

            for (int i = 1; i < listaGenerador.Count; i++)
            {
                RegistroGenerado regGen = listaGenerador[i];

                for (int j = 0; j <= i - 1; j++)
                {
                    RegistroGenerado regGen2 = listaGenerador[j];

                    if (regGen.Cod == regGen2.Cod)
                    {
                        regGen.NroCicloCombinado = regGen2.Cod;
                        regGen2.NroCicloCombinado = regGen2.Cod;
                    }
                }
            }

            int nciclocomb = -1;
            int[] tempo = new int[listaGenerador.Count];

            int MAXCICLOCOMB = 100;
            int[,] ciclocomb = new int[MAXCICLOCOMB, 20];
            int[] ciclocombn = new int[MAXCICLOCOMB];

            for (int i = 0; i < listaGenerador.Count; i++)
            {
                tempo[i] = 1;
            }

            for (int i = 0; i < listaGenerador.Count; i++)
            {
                RegistroGenerado item = listaGenerador[i];

                if ((item.NroCicloCombinado != 0) && (tempo[i] == 1))
                {
                    tempo[i] = 0;
                    nciclocomb = nciclocomb + 1;
                    ciclocombn[nciclocomb] = 0;
                    ciclocomb[nciclocomb, ciclocombn[nciclocomb]] = i;

                    for (int j = i + 1; j < listaGenerador.Count; j++)
                    {
                        RegistroGenerado item2 = listaGenerador[j];

                        if ((item.NroCicloCombinado != 0) && (tempo[j] == 1))
                        {
                            if (item2.NroCicloCombinado == item.NroCicloCombinado)
                            {
                                tempo[j] = 0;
                                ciclocombn[nciclocomb] = ciclocombn[nciclocomb] + 1;
                                ciclocomb[nciclocomb, ciclocombn[nciclocomb]] = j;
                            }
                        }
                    }
                }
            }

            foreach (LineaEms item in listaLinea)
            {
                NombreCodigoBarra barra1 = listaBarra.Where(x => x.CodBarra == item.IdBarra1).FirstOrDefault();
                NombreCodigoBarra barra2 = listaBarra.Where(x => x.CodBarra == item.IdBarra2).FirstOrDefault();

                item.Barra1 = (barra1 != null) ? barra1 : null;
                item.Barra2 = (barra2 != null) ? barra2 : null;
            }

            foreach (NombreCodigoBarra barra in listaBarra)
            {
                double valor = 0;
                foreach (SwitchedShunt shunt in listShuntDinamico)
                {
                    if (barra.CodBarra.Trim() == shunt.Codigo.Trim())
                    {
                        valor = valor + shunt.Val;
                    }
                }
                int orden = 0;
                int contador = 0;

                if (valor != 0)
                {
                    double b = 0;
                    double Vk = 0;
                    double Vj = 0;
                    double Vi = 0;
                    NombreCodigoBarra barraSel = null;

                    foreach (LineaEms linea in listaLinea)
                    {
                        if ((linea.Barra1.CodBarra == barra.CodBarra || linea.Barra2.CodBarra == barra.CodBarra) && linea.LinOp == "yes")
                        {
                            contador++;

                            if (linea.Rps == 0 && linea.Xps == 0.0001 && linea.Tap1 == 1 && linea.Tap2 == 1 && linea.Barra1.Angulo == linea.Barra2.Angulo)
                            {
                                //-aqui nos quedamos
                                orden++;

                                b = linea.Xps;

                                if (linea.Barra1.CodBarra == barra.CodBarra)
                                {
                                    Vk = linea.Barra1.VoltajePU;
                                    Vj = linea.Barra2.VoltajePU;
                                    barraSel = linea.Barra1;
                                }
                                if (linea.Barra2.CodBarra == barra.CodBarra)
                                {
                                    Vk = linea.Barra2.VoltajePU;
                                    Vj = linea.Barra1.VoltajePU;
                                    barraSel = linea.Barra2;
                                }
                            }
                        }
                    }

                    if (orden == 1 && contador == 1)
                    {
                        Vi = (1 / b) * Vj / (1 / b - valor / 100);
                        if (Math.Abs(Vi - Vk) < 0.0001)
                        {
                            if (barraSel != null)
                            {
                                barraSel.VoltajePU = Vi;
                            }
                        }
                    }
                }
            }

            //'Detectar que barras estan sin conexion alguna, esto trae errores la determinacion de Shift Factors (inversion de matriz singular)
            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.conect = false;
            }

            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    item.Barra1.conect = true;
                    item.Barra2.conect = true;
                }
            }

            //'******************************************
            //' identificacion de sistemas aislados
            //'******************************************
            int nsis, nsisaux1, nsisaux2;
            int nsistema;
            int[] sistema = new int[1000];
            nsis = 0;//numero de sistema

            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.sis = 0;
            }

            nsis = 0;
            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    if (item.Barra1.sis == 0 && item.Barra2.sis == 0)
                    {
                        nsis = nsis + 1;
                        item.Barra1.sis = nsis;
                        item.Barra2.sis = nsis;
                    }
                    else
                    {
                        if (item.Barra1.sis != 0 && item.Barra2.sis != 0)
                        {
                            nsisaux1 = item.Barra1.sis;
                            nsisaux2 = item.Barra2.sis;
                            if (item.Barra2.sis < nsisaux1)
                            {
                                nsisaux1 = item.Barra2.sis;
                                nsisaux2 = item.Barra1.sis;
                            }
                            foreach (NombreCodigoBarra item2 in listaBarra)
                            {
                                if (item2.sis == nsisaux2)
                                {
                                    item2.sis = nsisaux1;
                                }
                            }
                        }
                        else
                        {
                            if (item.Barra1.sis != 0 && item.Barra2.sis == 0)
                            {
                                item.Barra2.sis = item.Barra1.sis;
                            }
                            else
                            {
                                if (item.Barra2.sis != 0 && item.Barra1.sis == 0)
                                {
                                    item.Barra1.sis = item.Barra2.sis;
                                }
                            }
                        }
                    }
                }
            }

            //'normalizar numero de sistema e identificar cuantos sistemas hay
            nsistema = 0;// -1;
            for (int i = 1; i < 1000; i++)//for (int i = 0; i < 50; i++)
            {
                sistema[i] = 0;
            }

            int esta;

            foreach (NombreCodigoBarra item in listaBarra)
            {
                nsisaux1 = item.sis;
                esta = 0;
                for (int j = 1; j <= nsistema; j++)//for (int j = 0; j <= nsistema; j++)
                {
                    if (esta == 0)
                    {
                        if (sistema[j] == nsisaux1)
                        {
                            esta = 1;
                        }
                    }
                }
                if (esta == 0)
                {
                    nsistema = nsistema + 1;
                    sistema[nsistema] = nsisaux1;
                }
            }

            for (int i = 1; i <= nsistema; i++)//for (int i = 0; i <= nsistema; i++)
            {
                if (sistema[i] != 0)
                {
                    foreach (NombreCodigoBarra item in listaBarra)
                    {
                        if (item.sis == sistema[i])
                        {
                            item.sis = i;
                        }
                    }
                    sistema[i] = i;
                }
            }


            if (sistema[nsistema] == 0)
            {
                nsistema = nsistema - 1;
            }


            //'Todas las barras que no estan conectadas a nada, estan figurando en un sistema 0.  
            //'Se deben unir en un solo sistema 0, unirla a la primera que encuentre

            //int esta = 0;                              
            NombreCodigoBarra barraAislado = null;

            foreach (NombreCodigoBarra barra in listaBarra)
            {
                if (barra.sis == 0)
                {
                    if (barraAislado == null)
                    {
                        barraAislado = barra;
                    }
                    else
                    {
                        LineaEms linea = new LineaEms("01", "", 1, 1, 3, "", 0, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 0, 0, string.Empty);
                        linea.Barra1 = barraAislado;
                        linea.Barra2 = barra;
                        listaLinea.Add(linea);
                    }
                }
            }

            //'crear un enlace entre sistemas aislados con X muy grande.
            //'De esta forma el optimizador solucionara un solo problema pero con resultados independientes

            if (nsistema > 1)//if (nsistema > 0)
            {
                for (int i = 2; i <= nsistema; i++)//for (int i = 1; i <= nsistema; i++)
                {
                    LineaEms linea = new LineaEms("01", "", 1, 1, 3, "", 0, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 0, 0, string.Empty);
                    listaLinea.Add(linea);

                    esta = 0;

                    foreach (NombreCodigoBarra item in listaBarra)
                    {
                        if (esta == 0 && item.sis == sistema[i])
                        {
                            linea.Barra1 = item;
                            esta = 1;
                            break;
                        }
                    }

                    esta = 0;
                    foreach (NombreCodigoBarra item in listaBarra)
                    {
                        if (esta == 0 && item.sis == sistema[i - 1])
                        {
                            linea.Barra2 = item;
                            esta = 1;
                            break;
                        }
                    }
                }
            }




            for (int i = 0; i <= nciclocomb; i++)
            {
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == listaGenerador[ciclocomb[i, 0]].BarraID.ToString()).FirstOrDefault();
                bool conectado = (barra != null) ? barra.conect : false;
                if (listaGenerador[ciclocomb[i, 0]].Tipo == "T" && listaGenerador[ciclocomb[i, 0]].IndOpe == 1 && conectado && listaGenerador[ciclocomb[i, 0]].EsCicloCombinado)
                {
                    double pmin = (double)listaGenerador[ciclocomb[i, 0]].PotenciaMinima;
                    double pmax = (double)listaGenerador[ciclocomb[i, 0]].PotenciaMaxima;
                    double pg = 0;

                    for (int j = 0; j <= ciclocombn[i]; j++)
                    {
                        pg = pg + (double)listaGenerador[ciclocomb[i, j]].Potencia;
                    }

                    double deltapmax = pmax - pg;
                    double deltapmin = pg - pmin;

                    for (int j = 0; j <= ciclocombn[i]; j++)
                    {
                        listaGenerador[ciclocomb[i, j]].PotenciaMaxima = (decimal)((double)listaGenerador[ciclocomb[i, j]].Potencia + deltapmax * ((double)listaGenerador[ciclocomb[i, j]].Potencia) / pg);
                        listaGenerador[ciclocomb[i, j]].PotenciaMinima = (decimal)((double)listaGenerador[ciclocomb[i, j]].Potencia - deltapmin * ((double)listaGenerador[ciclocomb[i, j]].Potencia) / pg);
                    }
                }
            }

            int idLinea = 0;
            foreach (LineaEms item in listaLinea)
            {
                try
                {
                    item.Id = ++idLinea;
                }
                catch (Exception e)
                {
                }
            }

            int idBarra = 0;
            foreach (NombreCodigoBarra item in listaBarra)
            {
                item.Id = ++idBarra;
            }

            #endregion

            #region Escritura de archivo

            //'*******************************************
            //'   ESCRITURA DE ARCHIVO PARA GAMS
            //'*******************************************
            //nombre2 = nombre & sistema(s) & ".DAT"
            string log = "";
            bool esCicloComb;

            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Barras");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET NB /");

            foreach (NombreCodigoBarra item in listaBarra)
            {
                AgregaLinea(ref log, "        B" + String.Format("{0:000}", item.Id) + " " + item.NombBarra);
            }

            AgregaLinea(ref log, "/;");

            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Demandas");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "TABLE Demanda(NB,*)");
            AgregaLinea(ref log, "                 V           Angulo        Pc      Qc        ShuntR  ShuntI");
            foreach (NombreCodigoBarra item in listaBarra)
            {
                AgregaLinea(ref log, "        B" + String.Format("{0:000}", item.Id) + "   " +
                                                   String.Format("{0:0.000000}", item.VoltajePU) + "   " +
                                                   String.Format("{0:000.00000}", item.Angulo) + "   " +
                                                   String.Format("{0:000.000}", item.barraPc) + "   " +
                                                   String.Format("{0:000.000}", item.barraQc) + "   " +
                                                   String.Format("{0:000.000}", -1 * item.barraShuntR) + "   " +
                                                   String.Format("{0:000.000}", item.barraShuntI));
            }

            List<PrGenforzadaDTO> listaForzada = new List<PrGenforzadaDTO>();

            foreach (PrGenforzadaDTO itemForzada in listaGenFor)
            {
                foreach (PrgenforzadaitemDTO subItemForzada in itemForzada.ListaItems)
                {
                    if (!string.IsNullOrEmpty(itemForzada.Subcausacmg))
                        listaForzada.Add(new PrGenforzadaDTO { Codbarra = subItemForzada.Codbarra, Idgener = subItemForzada.Idgenerador });
                }
            }

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "* Unidades generacion /");
            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "SET  GT  /");

            foreach (RegistroGenerado item in listaGenerador)
            {
                esCicloComb = listaGenerador.Where(x => x.Cod == item.Cod).Count() > 1;
                item.EsCicloCombinado = esCicloComb;
            }

            int cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {

                cod++;

                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.BarraID.ToString()).FirstOrDefault();

                if (barra != null)
                {
                    bool conectado = (barra != null) ? barra.conect : false;
                    string nombreBarra = (barra != null) ? barra.NombBarra : "";
                    int barraID = (barra != null) ? barra.Id : 0;
                    int barraSis = (barra != null) ? barra.sis : 0;

                    string genFor = "no ";
                    string genCalif = "0"; // item.IdCalificacion.ToString();
                    string referencia = "no";
                    if (barra.TipoTension == 3) referencia = "yes";
                    PrGenforzadaDTO genfor = listaForzada.Where(x => x.Codbarra == item.BarraID.ToString() && x.Idgener == item.GenerID).FirstOrDefault();
                    genFor = (genfor != null) ? "yes" : "no ";

                    if (item.Tipo == "N")
                    {
                        genFor = "yes";
                        item.PotenciaMaxima = item.Potencia;
                        item.PotenciaMinima = item.Potencia;
                    }

                    item.NombreBarra = nombreBarra;
                    item.Conectado = conectado;
                    item.IdBarra = barraID;
                    item.Referencia = referencia;
                    item.GenFor = genFor;
                    item.BarraSis = barraSis;
                    item.GenCalificacion = genCalif;

                    if ((item.Tipo == "T") && (item.IndOpe == 1) && conectado)
                    {
                        AgregaLinea(ref log, "        T" + String.Format("{0:000}", cod) + " " + item.NombreBarra + " " + item.GenerID);
                    }
                }
            }

            //*Codigo agregado de generación forzada
            for (int i = 0; i <= nciclocomb; i++)
            {
                string fin = "no";
                for (int j = 0; j <= ciclocombn[i]; j++)
                {
                    if (listaGenerador[ciclocomb[i, j]].GenFor == "yes")
                    {
                        fin = "yes";
                    }
                }
                if (fin == "yes")
                {
                    for (int j = 0; j <= ciclocombn[i]; j++)
                    {
                        listaGenerador[ciclocomb[i, j]].GenFor = "yes";
                    }
                }
            }
            //*Fin codigo agregado de generación forzada

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;
                if ((item.Tipo == "H") && (item.IndOpe == 1) && item.Conectado)
                {
                    AgregaLinea(ref log, "        H" + String.Format("{0:000}", cod) + " " + item.NombreBarra + " " + item.GenerID);
                }
            }

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;
                if ((item.Tipo == "N") && (item.IndOpe == 1) && item.Conectado)
                {
                    AgregaLinea(ref log, "        N" + String.Format("{0:000}", cod) + " " + item.NombreBarra + " " + item.GenerID);
                }
            }


            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "SET  PgBus(GT,NB) /");

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;
                if (item.Tipo == "T" && item.IndOpe == 1 && item.Conectado)
                {
                    AgregaLinea(ref log, "        T" + String.Format("{0:000}", cod) + ".B" + String.Format("{0:000}", item.IdBarra));
                }
            }

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;
                if (item.Tipo == "H" && item.IndOpe == 1 && item.Conectado)
                {
                    AgregaLinea(ref log, "        H" + String.Format("{0:000}", cod) + ".B" + String.Format("{0:000}", item.IdBarra));
                }
            }

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;
                if (item.Tipo == "N" && item.IndOpe == 1 && item.Conectado)
                {
                    AgregaLinea(ref log, "        N" + String.Format("{0:000}", cod) + ".B" + String.Format("{0:000}", item.IdBarra));
                }
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "TABLE PgData(GT,*)");
            AgregaLinea(ref log, "                 Pgen        Qgen        Pmin        Pmax        Qmin       Qmax        CI1      Forzada     Sist  Calif  Tipo Conec  Ref");

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;

                string auxpotencia = "";
                string auxqg = "";
                string auxpotmin = "";
                string auxpotmax = "";
                string auxqmin = "";
                string auxqmax = "";

                if (item.Potencia >= 0) auxpotencia = " " + String.Format("{0:0.000E+00}", item.Potencia);
                else auxpotencia = String.Format("{0:0.000E+00}", item.Potencia);
                if (item.Qg >= 0) auxqg = " " + String.Format("{0:0.000E+00}", item.Qg);
                else auxqg = String.Format("{0:0.000E+00}", item.Qg);
                if (item.PotenciaMinima >= 0) auxpotmin = " " + String.Format("{0:0.000E+00}", item.PotenciaMinima);
                else auxpotmin = String.Format("{0:0.000E+00}", item.PotenciaMinima);
                if (item.PotenciaMaxima >= 0) auxpotmax = " " + String.Format("{0:0.000E+00}", item.PotenciaMaxima);
                else auxpotmax = String.Format("{0:0.000E+00}", item.PotenciaMaxima);
                if (item.Qmin >= 0) auxqmin = " " + String.Format("{0:0.000E+00}", item.Qmin);
                else auxqmin = String.Format("{0:0.000E+00}", item.Qmin);
                if (item.Qmax >= 0) auxqmax = " " + String.Format("{0:0.000E+00}", item.Qmax);
                else auxqmax = String.Format("{0:0.000E+00}", item.Qmax);


                if (item.Tipo == "T" && item.IndOpe == 1 && item.Conectado)
                {
                    AgregaLinea(ref log, "        T" + String.Format("{0:000}", cod) +

                    "  " + auxpotencia +
                    "  " + auxqg +
                    "  " + auxpotmin +
                    "  " + auxpotmax +
                    "  " + auxqmin +
                    "  " + auxqmax +
                    "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci1, 4, 4) +
                    "     " + UtilCortoPlazo.FormatearCadena(item.GenFor, 3) +
                    "        " + UtilCortoPlazo.FormatearCadena((item.BarraSis).ToString(), 1) +
                    "     " + UtilCortoPlazo.FormatearCadena(item.GenCalificacion, 1) +
                    "       1" +
                    "   " + "yes" +
                    "    " + item.Referencia);
                }
            }

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;

                string auxpotencia = "";
                string auxqg = "";
                string auxpotmin = "";
                string auxpotmax = "";
                string auxqmin = "";
                string auxqmax = "";

                if (item.Potencia >= 0) auxpotencia = " " + String.Format("{0:0.000E+00}", item.Potencia);
                else auxpotencia = String.Format("{0:0.000E+00}", item.Potencia);
                if (item.Qg >= 0) auxqg = " " + String.Format("{0:0.000E+00}", item.Qg);
                else auxqg = String.Format("{0:0.000E+00}", item.Qg);
                if (item.PotenciaMinima >= 0) auxpotmin = " " + String.Format("{0:0.000E+00}", item.PotenciaMinima);
                else auxpotmin = String.Format("{0:0.000E+00}", item.PotenciaMinima);
                if (item.PotenciaMaxima >= 0) auxpotmax = " " + String.Format("{0:0.000E+00}", item.PotenciaMaxima);
                else auxpotmax = String.Format("{0:0.000E+00}", item.PotenciaMaxima);
                if (item.Qmin >= 0) auxqmin = " " + String.Format("{0:0.000E+00}", item.Qmin);
                else auxqmin = String.Format("{0:0.000E+00}", item.Qmin);
                if (item.Qmax >= 0) auxqmax = " " + String.Format("{0:0.000E+00}", item.Qmax);
                else auxqmax = String.Format("{0:0.000E+00}", item.Qmax);


                if (item.Tipo == "H" && item.IndOpe == 1 && item.Conectado)
                {
                    AgregaLinea(ref log, "        H" + String.Format("{0:000}", cod) +

                    "  " + auxpotencia +
                    "  " + auxqg +
                    "  " + auxpotmin +
                    "  " + auxpotmax +
                    "  " + auxqmin +
                    "  " + auxqmax +
                    "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci1, 4, 4) +
                    "     " + UtilCortoPlazo.FormatearCadena(item.GenFor, 3) +
                    "        " + UtilCortoPlazo.FormatearCadena((item.BarraSis).ToString(), 1) +
                    "     " + UtilCortoPlazo.FormatearCadena(item.GenCalificacion, 1) +
                    "       1" +
                    "   " + "yes" +
                    "    " + item.Referencia);
                }
            }

            cod = 0;
            foreach (RegistroGenerado item in listaGenerador)
            {
                cod++;
                string auxpotencia = "";
                string auxqg = "";
                string auxpotmin = "";
                string auxpotmax = "";
                string auxqmin = "";
                string auxqmax = "";

                if (item.Potencia >= 0) auxpotencia = " " + String.Format("{0:0.000E+00}", item.Potencia);
                else auxpotencia = String.Format("{0:0.000E+00}", item.Potencia);
                if (item.Qg >= 0) auxqg = " " + String.Format("{0:0.000E+00}", item.Qg);
                else auxqg = String.Format("{0:0.000E+00}", item.Qg);
                if (item.PotenciaMinima >= 0) auxpotmin = " " + String.Format("{0:0.000E+00}", item.PotenciaMinima);
                else auxpotmin = String.Format("{0:0.000E+00}", item.PotenciaMinima);
                if (item.PotenciaMaxima >= 0) auxpotmax = " " + String.Format("{0:0.000E+00}", item.PotenciaMaxima);
                else auxpotmax = String.Format("{0:0.000E+00}", item.PotenciaMaxima);
                if (item.Qmin >= 0) auxqmin = " " + String.Format("{0:0.000E+00}", item.Qmin);
                else auxqmin = String.Format("{0:0.000E+00}", item.Qmin);
                if (item.Qmax >= 0) auxqmax = " " + String.Format("{0:0.000E+00}", item.Qmax);
                else auxqmax = String.Format("{0:0.000E+00}", item.Qmax);

                if (item.Tipo == "N" && item.IndOpe == 1 && item.Conectado)
                {
                    AgregaLinea(ref log, "        N" + String.Format("{0:000}", cod) +

                    "  " + auxpotencia +
                    "  " + auxqg +
                    "  " + auxpotmin +
                    "  " + auxpotmax +
                    "  " + auxqmin +
                    "  " + auxqmax +
                    "  " + UtilCortoPlazo.FormatearValorAdicional(item.Ci1, 4, 4) +
                    "     " + UtilCortoPlazo.FormatearCadena(item.GenFor, 3) +
                    "        " + UtilCortoPlazo.FormatearCadena((item.BarraSis).ToString(), 1) +
                    "     " + UtilCortoPlazo.FormatearCadena(item.GenCalificacion, 1) +
                    "       1" +
                    "   " + "yes" +
                    "    " + item.Referencia);
                }
            }


            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");

            AgregaLinea(ref log, "TABLE Tension(NB,*)");
            AgregaLinea(ref log, "                Vmin     Vmax   Tipo");
            AgregaLinea(ref log, "        B001   0.500    1.500    3");

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");

            AgregaLinea(ref log, "********************************************* /");
            AgregaLinea(ref log, "* Compensacion Reactiva dinámica /");
            AgregaLinea(ref log, "********************************************* /");

            AgregaLinea(ref log, "SET  CR  /");

            int correlativoShunt = 0;
            foreach (SwitchedShunt item in listShuntDinamico)
            {
                correlativoShunt++;
                item.Correlativo = correlativoShunt;
                NombreCodigoBarra barra = listaBarra.Where(x => x.CodBarra == item.Codigo).FirstOrDefault();
                item.BarraID = barra.Id;
                AgregaLinea(ref log, "        S" + String.Format("{0:000}", item.Correlativo) + " " + barra.NombBarra + " " + (item.N + 1));
            }
            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");

            AgregaLinea(ref log, "SET  CRBus(CR,NB) /");

            foreach (SwitchedShunt item in listShuntDinamico)
            {
                AgregaLinea(ref log, "        S" + String.Format("{0:000}", item.Correlativo) + ".B" + String.Format("{0:000}", item.BarraID));
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");

            AgregaLinea(ref log, "TABLE CRData(CR,*)");
            AgregaLinea(ref log, "                   Q            Qmin        Qmax       Conec");

            foreach (SwitchedShunt item in listShuntDinamico)
            {
                string auxval = "";
                string auxmin = "";
                string auxmax = "";

                if (item.Val >= 0) auxval = " " + String.Format("{0:0.000E+00}", item.Val);
                else auxval = String.Format("{0:0.000E+00}", item.Val);

                if (item.Min >= 0) auxmin = " " + String.Format("{0:0.000E+00}", item.Min);
                else auxmin = String.Format("{0:0.000E+00}", item.Min);

                if (item.Max >= 0) auxmax = " " + String.Format("{0:0.000E+00}", item.Max);
                else auxmax = String.Format("{0:0.000E+00}", item.Max);

                AgregaLinea(ref log, "        S" + String.Format("{0:000}", item.Correlativo) +
                                              "   " + auxval +
                                              "   " + auxmin +
                                              "   " + auxmax +
                                              "    " + ((item.Conn == 1) ? "yes" : "no"));
            }

            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Enlaces");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET  ENL  /");

            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    if (item.LinIslin != 3)
                    {
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) + "    " + item.Barra1.NombBarra + "-" + item.Barra2.NombBarra);

                    }
                    else
                    {
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) + "    Ficticio para sistemas aislados");
                    }
                }
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "SET  FBus(ENL,NB,NB) /");

            int countd = 0;
            try
            {
                foreach (LineaEms item in listaLinea)
                {
                    countd++;

                    if (item.LinOp == "yes")// && item.LinIslin != 3)
                    {
                        if (item.Barra2 == null)
                        {
                            item.Barra2 = new NombreCodigoBarra();
                            item.Barra2.Id = 0;
                        }
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) + ".B" + String.Format("{0:000}", item.Barra1.Id) + ".B" + String.Format("{0:000}", item.Barra2.Id));
                    }
                }
            }
            catch
            {
                string s = countd.ToString();
            }

            AgregaLinea(ref log, "/;");
            AgregaLinea(ref log, " ");
            AgregaLinea(ref log, "TABLE  FData(ENL,*)");
            AgregaLinea(ref log, "                     R0          X0            G0           B0      Tap1    Tap2    Pmax     Cong  Envio Tipo Conec   Sis");

            List<EqCongestionConfigDTO> congestion = new List<EqCongestionConfigDTO>();
            congestion = FactorySic.GetEqCongestionConfigRepository().List();

            foreach (LineaEms item in listaLinea)
            {
                if (item.LinOp == "yes")
                {
                    aux1 = "no ";
                    pot = item.LinPot;

                    foreach (PrCongestionDTO item2 in listaCongestion)
                    {
                        NombreCodigoBarra barra1 = relacionBarra.Where(x => x.NombBarra == item2.Nodobarra1 && item2.Nodobarra1 != null).FirstOrDefault();
                        NombreCodigoBarra barra2 = relacionBarra.Where(x => x.NombBarra == item2.Nodobarra2 && item2.Nodobarra2 != null).FirstOrDefault();

                        if (((item.IdBarra1 == barra1.CodBarra && item.IdBarra2 == barra2.CodBarra) ||
                            (item.IdBarra1 == barra2.CodBarra && item.IdBarra2 == barra1.CodBarra))
                            && item.Orden == item2.NombLinea)
                        {
                            aux1 = "yes";
                            pot = item2.Flujo;
                        }
                    }

                    if (item.LinIslin != 3)
                    {
                        string auxr = "";
                        string auxx = "";
                        string auxg = "";
                        string auxb = "";

                        double xb = item.B;

                        if (item.Rps >= 0) auxr = " " + String.Format("{0:0.00000E+00}", item.Rps);
                        else auxr = String.Format("{0:0.00000E+00}", item.Rps);

                        if (item.Xps >= 0) auxx = " " + String.Format("{0:0.00000E+00}", item.Xps);
                        else auxx = String.Format("{0:0.00000E+00}", item.Xps);

                        if (item.Gshp + item.Gshs >= 0) auxg = " " + String.Format("{0:0.00000E+00}", item.Gshp + item.Gshs);
                        else auxg = String.Format("{0:0.00000E+00}", item.Gshp + item.Gshs);

                        if (xb >= 0) auxb = " " + String.Format("{0:0.00000E+00}", xb);
                        else auxb = String.Format("{0:0.00000E+00}", xb);

                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) +
                         "   " + auxr +
                         " " + auxx +
                         " " + auxg +
                         " " + auxb +
                         " " + String.Format("{0:0.00000}", item.Tap1) +
                         " " + String.Format("{0:0.00000}", item.Tap2) +
                         " " + String.Format("{0:0000.00}", pot) +
                         "    " + aux1 +
                         "   yes   2 " +
                         "    " + String.Format("{0:00}", item.LinOp) +
                         "    " + (item.Barra1.sis).ToString());
                    }
                    else
                    {
                        string r = (item.Rps == null ? 0 : item.Rps).ToString();
                        //'enlace ficticio entres sistemas aislados
                        AgregaLinea(ref log, "        E" + String.Format("{0:000}", item.Id) +
                        "    0.0000E+00  1.0000E+04    0.0000E+00   0.0000E+00 1.00000 1.00000 0100.00    no    yes   2     yes    " + (item.Barra1.sis).ToString());
                    }
                }
            }
            AgregaLinea(ref log, ";");
            AgregaLinea(ref log, " ");

            // *** FIN PENDIENTE ***


            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                if (item.ListaItems.Count == 2)
                {
                    if (item.ListaItems[0].Nombarra1 == item.ListaItems[1].Nombarra1)
                    {
                        foreach (NombreCodigoBarra barra in listaBarra)
                        {
                            if (int.Parse(barra.CodBarra) < 0)
                            {
                                int esta0 = 0;
                                int esta2 = 0;
                                int esta3 = 0;

                                foreach (LineaEms linea in listaLinea)
                                {
                                    if (linea.Barra1.NombBarra == item.ListaItems[0].Nombarra1 && linea.Barra2.CodBarra == barra.CodBarra)
                                    {
                                        esta0 = 1;
                                    }
                                    if (linea.Barra1.NombBarra == item.ListaItems[1].Nombarra1 && linea.Barra2.CodBarra == barra.CodBarra)
                                    {
                                        esta2 = 1;
                                    }
                                    if (linea.Barra1.NombBarra == item.ListaItems[1].Nombarra2 && linea.Barra2.CodBarra == barra.CodBarra)
                                    {
                                        esta3 = 1;
                                    }
                                }
                                if (esta0 == 1 && esta2 == 1 && esta3 == 1)
                                {
                                    item.ListaItems.RemoveAt(1);
                                    item.ListaItems[0].Nombarra2 = barra.NombBarra;
                                    break;
                                }

                            }
                        }
                    }
                }
            }


            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "* Congestion de varios enlaces en simultaneo");
            AgregaLinea(ref log, "*********************************************");
            AgregaLinea(ref log, "SET CONG /");

            int numgrupoLinea = 0;

            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                numgrupoLinea++;
                string lineanomb = "       C" + String.Format("{0:000}", numgrupoLinea) + "    ";

                foreach (PrCongestionitemDTO itemCongestion in item.ListaItems)
                {
                    if (itemCongestion.Nombarra1 != null && itemCongestion.Nombarra1.Trim().Length > 0)
                    {
                        lineanomb += itemCongestion.Nombarra1.Trim() + "-";
                    }
                }

                AgregaLinea(ref log, lineanomb);
            }

            //ncongc
            if (numgrupoLinea == 0)
            {
                AgregaLinea(ref log, "C1 CONGESTION L1");
            }
            AgregaLinea(ref log, "        /;");

            AgregaLinea(ref log, "SET CONGRuta(CONG,ENL)");
            AgregaLinea(ref log, "      /");

            numgrupoLinea = 0;
            int esta1 = 0;
            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                numgrupoLinea++;
                string lincjt = "       C" + String.Format("{0:000}", numgrupoLinea) + ".(";

                int j = 1;

                foreach (PrCongestionitemDTO itemCongestion in item.ListaItems)
                {
                    esta1 = 0;
                    int k = 1;
                    foreach (LineaEms lin in listaLinea)
                    {
                        if (itemCongestion.Nombarra1 != null && itemCongestion.Nombarra2 != null)
                        {
                            if ((lin.Barra1.NombBarra == itemCongestion.Nombarra1 && lin.Barra2.NombBarra == itemCongestion.Nombarra2)
                                ||
                                (lin.Barra2.NombBarra == itemCongestion.Nombarra1 && lin.Barra1.NombBarra == itemCongestion.Nombarra2))
                            {
                                esta1 = lin.Id;
                            }
                        }
                        k++;
                    }

                    lincjt += "E" + String.Format("{0:000}", esta1);

                    if (j != item.ListaItems.Count)
                    {
                        lincjt += ",";
                    }
                    else
                    {
                        lincjt += ")";
                    }
                    j++;
                }
                AgregaLinea(ref log, lincjt);
            }

            if (numgrupoLinea == 0)
            {
                AgregaLinea(ref log, "C1.(E001)");
            }

            AgregaLinea(ref log, "      /;");

            AgregaLinea(ref log, "TABLE CONGmax(CONG,*)");
            AgregaLinea(ref log, "                 Pmax     Cong");


            numgrupoLinea = 0;
            foreach (PrCongestionDTO item in listaCongestionGrupo)
            {
                numgrupoLinea++;
                string lincjt = "       C" + String.Format("{0:000}", numgrupoLinea) + "    " + String.Format("{0:0000.000}", item.Flujo) + "   yes";
                AgregaLinea(ref log, lincjt);
            }

            if (numgrupoLinea == 0)
            {
                AgregaLinea(ref log, "              C1  130     no  ");
            }

            AgregaLinea(ref log, ";");

            return log;

            #endregion
        }


        /// <summary>
        /// Permite contar los registros correspondientes a un ciclo combinado
        /// </summary>
        /// <param name="listaGenerador"></param>
        /// <returns></returns>
        private static List<RegistroGenerado> ObtenerCicloCombinado(List<RegistroGenerado> listaGenerador)
        {
            string logGenerador = "";
            List<RegistroGenerado> cicloComb = new List<RegistroGenerado>();

            foreach (RegistroGenerado item in listaGenerador)
            {
                int existeEnLista = cicloComb.Where(x => x.Cod == item.Cod).Count();

                logGenerador += item.BarraID + "," + item.BarraNombre + "," + item.Ccombcodi + "," + item.Cod + "," + existeEnLista + "\r\n";
                if (existeEnLista == 0)
                {
                    int repeticiones = listaGenerador.Where(x => x.Cod == item.Cod).Count();
                    if (repeticiones > 1)
                    {
                        cicloComb.Add(item);
                    }
                }
            }
            return cicloComb;
        }

        /// <summary>
        /// Agregar una nueva linea al formato de salida
        /// </summary>
        /// <param name="log"></param>
        /// <param name="linea"></param>
        private static void AgregaLinea(ref string log, string linea)
        {
            log += linea + "\r\n";
        }

        /// <summary>
        /// Permite obtener el nombre del archivo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string ObtenerFileName(DateTime fecha, string extension)
        {
            #region Ticket 19127 - Movisoft
            /*string fileName = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') +
                DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                extension;*/

            string fileName = fecha.Year + fecha.Month.ToString().PadLeft(2, '0') +
                fecha.Day.ToString().PadLeft(2, '0') +
                fecha.Hour.ToString().PadLeft(2, '0') +
                fecha.Minute.ToString().PadLeft(2, '0') +
                extension;

            #endregion

            return fileName;
        }

        /// <summary>
        /// Permite formatear los valores ingresados
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatearValor(string str)
        {
            decimal valor = 0;
            if (str == null) str = string.Empty;
            string resultado = string.Empty;
            if (decimal.TryParse(str, out valor))
            {
                resultado = valor.ToString("0.000");
            }
            string cad = resultado.PadLeft(8, ' ');

            return cad;
        }


        /// <summary>
        /// Permite formatear los valores ingresados
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatearValorAdicional(string str, int enteros, int decimales)
        {
            decimal valor = 0;
            if (str == null) str = string.Empty;
            string resultado = string.Empty;

            if (str.Contains("-"))
            {
                if (enteros > 1)
                    enteros = enteros - 1;
                else
                {
                    decimales = decimales - 1;
                    enteros = enteros + 1;
                }
            }


            string formato = (decimales > 0) ? "00000000".Substring(0, enteros) + "." + "0000000".Substring(0, decimales) : "00000000".Substring(0, enteros);

            if (decimal.TryParse(str, out valor))
            {
                resultado = valor.ToString(formato);
            }

            if (string.IsNullOrEmpty(resultado)) resultado = formato;

            return resultado;
        }

        public static string FormatearValorAdicionalLinea(string str, int enteros, int decimales)
        {
            decimal valor = 0;
            if (str == null) str = string.Empty;
            string resultado = string.Empty;


           
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
            {
                if (valor < 0)
                {
                    if (enteros == 1)
                    {
                        decimales = decimales - 1;
                    }
                }
            }


            string formato = (decimales > 0) ? "0000000".Substring(0, enteros) + "." + "000000".Substring(0, decimales) : "0000000".Substring(0, enteros);


            
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
            {
                resultado = valor.ToString(formato);
            }

            if (string.IsNullOrEmpty(resultado)) resultado = formato;

            return resultado;
        }


        /// <summary>
        /// Permite formatear una cadena
        /// </summary>
        /// <param name="str"></param>
        /// <param name="espacios"></param>
        /// <returns></returns>
        public static string FormatearCadena(string str, int espacios)
        {
            decimal valor = 0;
            if (str == null) str = string.Empty;
            string resultado = str;

            string cad = resultado.PadLeft(espacios, ' ');

            return cad;
        }



        /// <summary>
        /// Permite obtener el mensaje del error
        /// </summary>
        /// <param name="tipoError"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public static string ObtenerMensajeCorreo(int tipoError, string mensaje, DateTime fechaProceso, bool validacionDatos, bool flagWeb, bool flagNegativos, string mensajeNoModeladas)
        {
            string texto = "<strong>Informe de Proceso Costos Marginales Nodales</strong><br /><br />";
            texto += "Fecha: ";
            texto += fechaProceso.ToString("dd/MM/yyyy");
            texto += " Hora: ";
            texto += fechaProceso.ToString("HH:mm");
            texto += "<br /><br />";

            switch (tipoError)
            {
                case ConstantesCortoPlazo.OperacionCorrecta:
                    {
                        if (validacionDatos)
                        {
                            texto += "La ejecución del proceso se realizó correctamente.";

                            if (flagNegativos)
                            {
                                texto += "<br /><br />";
                                texto += "<strong>Existen valores negativos en los Costos Marginales, por favor revise.</strong>";
                            }
                        }
                        else
                        {
                            if (flagWeb)
                            {
                                texto += "Se ejecutó el proceso, pero los resultados no se mostrarán en el Portal debido a que existen valores grandes, por favor revise.";
                            }
                            else
                            {
                                texto += "La ejecución del proceso se realizó correctamente.";

                                if (flagNegativos)
                                {
                                    texto += "<br /><br />";
                                    texto += "<strong>Existen valores negativos en los Costos Marginales, por favor revise.</strong>";
                                }
                            }
                        }

                        break;
                    }
                case ConstantesCortoPlazo.ErrorGamsNoEjecuto:
                    {
                        texto += "No se ha encontrado resultado tras ejecutar GAMS por favor revise el archivo de entrada GAMS .DAT";
                        break;
                    }
                case ConstantesCortoPlazo.ErrorNoExistePSSE:
                    {
                        texto += "No se encontrado archivo PSSE. Por favor revisar que se estén exportando correctamente.";
                        break;
                    }
                case ConstantesCortoPlazo.ErrorEnOperacion:
                    {
                        texto += "Ha ocurrido un error en el proceso.";
                        texto += "<br /><br />";
                        texto += "<strong>Detalle del error</strong><br /><br/>";
                        texto += mensaje;
                        break;
                    }
                case ConstantesCortoPlazo.ErrorInconsistenciaModoOperacionOperacionEMS:
                    {
                        texto += "Se encontraron inconsistencias en los cálculos de Potencia Mínima y Máxima, además no se encontraron Modos de Operación para algunas unidades y algunas unidades tienen modo de operación pero aparecen como no operativas en el EMS.";
                        texto += "<br /><br />";
                        texto += "<strong>Unidades con inconsistencias:</strong><br/><br/>";
                        texto += mensaje;
                        break;
                    }
                case ConstantesCortoPlazo.ErrorInconsistencias:
                    {
                        texto += "Se encontraron inconsistencias en los cálculos de Potencia Mínima y Máxima, por favor revise y corrija.";
                        texto += "<br /><br />";
                        texto += "<strong>Unidades con inconsistencias:</strong><br/><br/>";
                        texto += mensaje;
                        break;
                    }
                case ConstantesCortoPlazo.ErrorModosOperacion:
                    {
                        texto += "No se encontraron los modos de operación para las siguientes unidades.";
                        texto += "<br /><br />";
                        texto += "<strong>Unidades:</strong><br/><br/>";
                        texto += mensaje;
                        break;
                    }
                case ConstantesCortoPlazo.ErrorOperacionEMS:
                    {
                        texto += "El proceso se ejecutó correctamente, sin embargo algunas ";
                        texto += "unidades tienen modo de operación pero aparecen no operativos en el EMS";
                        texto += "<br /><br />";
                        texto += "<strong>Unidades:</strong><br/><br/>";
                        texto += mensaje;


                        if (flagNegativos)
                        {
                            texto += "<br /><br />";
                            texto += "<strong>Existen valores negativos en los Costos Marginales, por favor revise.</strong>";
                        }

                        break; ;
                    }
                case ConstantesCortoPlazo.ErrorInconsistenciaModoOperacion:
                    {
                        texto += "Se encontraron inconsistencias en los cálculos de Potencia Mínima y Máxima, además no se encontraron Modos de Operación para algunas unidades.";
                        texto += "<br /><br />";
                        texto += "<strong>Unidades con inconsistencias:</strong><br/><br/>";
                        texto += mensaje;
                        break;
                    }
                case ConstantesCortoPlazo.ErrorModosOperacionOperacionEMS:
                    {
                        texto += "No se encontraron los modos de operación para algunas unidades y otras unidades tienen modo de operación pero aparecen como no operativos en el EMS.";
                        texto += "<br /><br />";
                        texto += mensaje;
                        break;
                    }
                case ConstantesCortoPlazo.ErrorInconsistenciaOperacionEMS:
                    {
                        texto += "Se encontraron inconsistencias en los cálculos de Potencia Mínima y Máxima, además algunas unidades que tienen modo de operación aparecen como no operativas en el EMS.";
                        texto += "<br /><br />";
                        texto += "<strong>Unidades con inconsistencias:</strong><br/><br/>";
                        texto += mensaje;
                        break;
                    }

                case ConstantesCortoPlazo.ErrorNoExisteArchivosNCP:
                    {
                        texto += "No se ha encontrados archivos NCP. Por favor revise que el archivo comprimnido se encuentre en la ruta correcta y con el nombre correcto.";
                        break;
                    }
                case ConstantesCortoPlazo.ErrorNoExisteTopologiaMD:
                    {
                        texto += "No se encontró escenario en el aplicativo YUPANA para la fecha del proceso.";
                        break;
                    }
            }

            #region Ticket 2022-004345

            if (!string.IsNullOrEmpty(mensajeNoModeladas))
            {
                texto += "<br /><br />";
                texto += "<strong>Unidades no modeladas en TNA</strong>";
                texto += "<br /><br />";
                texto += "Unidades no modeladas en TNA que no se incluyeron en el .DAT dado que ninguna de las barras relacionadas no se encuentran en el .raw.<br/><br/>";
                texto += mensajeNoModeladas;
            }

            #endregion

            return texto;
        }

        /// <summary>
        /// Permite obtener el listado de inconsistencias
        /// </summary>
        /// <param name="logInconsistencia"></param>
        /// <returns></returns>
        public static string ObtenerMensajeInconsistencia(string logInconsistencia)
        {
            StringBuilder html = new StringBuilder();

            string[] lines = logInconsistencia.Split('\n');

            html.Append("<table border='1' cellspacing = '0' cellpadding='0'>");

            if (lines.Length > 0)
            {
                string[] cabecera = lines[0].Split(',');

                html.Append("   <thead>");
                html.Append("       <tr style='background-color:#2980B9; color:#fff'>");

                for (int k = 0; k < cabecera.Length; k++)
                {
                    html.AppendFormat("           <th style='padding: 0 4px'>{0}</th>", cabecera[k]);
                }

                html.Append("       </tr>");
                html.Append("   </thead>");
                html.Append("   <tbody>");

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] items = lines[i].Split(',');

                    html.Append("       <tr>");

                    for (int k = 0; k < items.Length; k++)
                    {
                        html.AppendFormat("           <td>{0}</td>", items[k]);
                    }

                    html.Append("       </tr>");
                }

                html.Append("   </tbody>");
            }

            html.Append("</table>");
            html.Append("</br>");
            html.Append("</br>");
            return html.ToString();
        }

        /// <summary>
        /// Permite armar el mensaje de las unidades sin modos de operacion
        /// </summary>
        /// <param name="logModoOperacion"></param>
        /// <returns></returns>
        public static string ObtenerMensajeModoOperacion(string logModoOperacion)
        {
            StringBuilder html = new StringBuilder();

            string[] lines = logModoOperacion.Split('\n');

            html.Append("<table border='1' cellspacing = '0' cellpadding='0'>");

            if (lines.Length > 0)
            {
                html.Append("   <thead>");
                html.Append("       <tr style='background-color:#2980B9; color:#fff'>");
                html.Append("           <th>Barra</th>");
                html.Append("           <th>ID</th>");
                html.Append("           <th>Tensión</th>");
                html.Append("           <th>Pot. Generada</th>");
                html.Append("           <th>Ind. Operación</th>");

                html.Append("       </tr>");
                html.Append("   </thead>");
                html.Append("   <tbody>");

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] items = lines[i].Split(',');

                    html.Append("       <tr>");

                    for (int k = 0; k < items.Length; k++)
                    {
                        html.AppendFormat("           <td>{0}</td>", items[k]);
                    }

                    html.Append("       </tr>");
                }

                html.Append("   </tbody>");
            }

            html.Append("</table>");
            html.Append("<br /><br />");
            return html.ToString();
        }

        /// <summary>
        /// Permite armar el mensaje de las unidades sin modos de operacion
        /// </summary>
        /// <param name="logModoOperacion"></param>
        /// <returns></returns>
        public static string ObtenerMensajeOperacionEMS(string logOperacionEMS)
        {
            StringBuilder html = new StringBuilder();

            string[] lines = logOperacionEMS.Split('\n');

            html.Append("<table border='1' cellspacing = '0' cellpadding='0'>");

            if (lines.Length > 0)
            {
                html.Append("   <thead>");
                html.Append("       <tr style='background-color:#2980B9; color:#fff'>");
                html.Append("           <th>Barra</th>");
                html.Append("           <th>ID</th>");
                html.Append("           <th>Tensión</th>");
                html.Append("           <th>Pot. Generada</th>");
                html.Append("           <th>Ind. Operación</th>");
                html.Append("       </tr>");
                html.Append("   </thead>");
                html.Append("   <tbody>");

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] items = lines[i].Split(',');

                    html.Append("       <tr>");

                    for (int k = 0; k < items.Length; k++)
                    {
                        html.AppendFormat("           <td>{0}</td>", items[k]);
                    }

                    html.Append("       </tr>");
                }

                html.Append("   </tbody>");
            }

            html.Append("</table>");
            html.Append("<br /><br />");
            return html.ToString();
        }

        #region Ticket 2022-004345
        /// <summary>
        /// Lista las unidades que no tiene barra asignada
        /// </summary>      
        /// <returns></returns>
        public static string ObtenerMensajeUnidadesNoMoedeladasTNA(List<EqRelacionDTO> listEquipos)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<table border='1' cellspacing = '0' cellpadding='0'>");
            html.Append("   <thead>");
            html.Append("       <tr style='background-color:#2980B9; color:#fff'>");
            html.Append("           <th>Equipo</th>");
            html.Append("           <th>Nombre TNA</th>");            
            html.Append("       </tr>");
            html.Append("   </thead>");
            html.Append("   <tbody>");

            foreach(EqRelacionDTO item in listEquipos)
            {
                html.Append("       <tr>");
                html.Append("           <td>" + item.Equinomb + "</td>");
                html.Append("           <td>" + item.Nombretna + "</td>");
                html.Append("       </tr>");
            }

            html.Append("   </tbody>");

            html.Append("</table>");
            html.Append("<br /><br />");
            return html.ToString();
        }
        #endregion

        /// <summary>
        /// Permite armar el cuerpo del correo
        /// </summary>
        /// <param name="validacion"></param>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static string ObtenerCuerpoCorreoValidacion(string validacion, DateTime fechaProceso)
        {
            string texto = "<strong>Validación de datos requeridos para el cálculo de Costos Marginales Nodales a ejecutarse en la siguiente media hora.</strong><br /><br />";
            texto += "Fecha: ";
            texto += fechaProceso.ToString("dd/MM/yyyy");
            texto += " Hora: ";
            texto += fechaProceso.ToString("HH:mm");
            texto += "<br /><br />";

            texto += validacion;

            return texto;
        }

        /// <summary>
        /// Permite armar el cuerpo de correo de informe de reproceso masivo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static string ObtenerCorreoReprocesoMasivo(List<ResultadoProcesoMasivo> list, DateTime fechaInicio, DateTime fechaFin)
        {
            string texto = "";

            texto += "<html>";
            texto += "    <head><STYLE TYPE='text/css'>";
            texto += "    body {font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}";
            texto += "    .mail {width:500px;border-spacing:0;border-collapse:collapse;}";
            texto += "    .mail thead th {text-align: center;background: #417AA7;color:#ffffff}";
            texto += "    .mail tr td {border:1px solid #dddddd;}";
            texto += "    </style></head>";
            texto += "    <body>";

            texto += "<strong>Por favor revise el resultado de la ejecución de reprocesos masivos.</strong><br /><br />";
            texto += "Desde el ";
            texto += fechaInicio.ToString("dd/MM/yyyy");
            texto += " al ";
            texto += fechaFin.ToString("dd/MM/yyyy");
            texto += "<br /><br />";

            texto += "<table cellspacing = '0' cellpadding = '0' class='mail'>";
            texto += "   <thead>";
            texto += "      <tr>";
            texto += "          <th style='width:200px'>Fecha y hora</th>";
            texto += "          <th style='width:150px'>Resultado</th>";
            texto += "      </tr>";
            texto += "   </thead>";
            texto += "   <tbody>";

            foreach (ResultadoProcesoMasivo item in list)
            {
                texto += "      <tr>";
                texto += string.Format("          <td>{0}</td>", item.FechaProceso);
                texto += string.Format("          <td><strong style='color:{1}'>{0}</strong></td>", (item.Resultado) ? "Ok" : "Error", (item.Resultado) ? "#009900" : "#FF0000");
                texto += "      </tr>";
            }

            texto += "   </tbody>";
            texto += "</table>";
            texto += "    </body>";
            texto += "</html>";

            return texto;
        }

        /// <summary>
        /// Permite generar el correo
        /// </summary>
        /// <param name="indicadorPSSE">0: No Existe, 1: Existe</param>
        /// <param name="indicadorNCP">-1: Error, 1: Existe programa, 2: No existe programa, 3: Existe reprograma, 4: Reprograma no tiene formato correcto</param>
        /// <param name="modosOperacion"></param>
        /// <param name="listRSF"></param>
        /// <returns></returns>
        public static string ObtenerCorreoValidacion(int indicadorPSSE, int indicadorNCP, List<RegistroGenerado> modosOperacion,
            List<EveRsfdetalleDTO> listRSF, List<RegistroGenerado> operacionEMS)
        {
            bool flagValidacion = false;

            if (indicadorPSSE == 0 ||
                (indicadorNCP == -1 || indicadorNCP == 2 || indicadorNCP == 4) ||
                modosOperacion.Count() > 0 ||
                listRSF.Count() == 0)
            {
                flagValidacion = true;
            }

            if (!flagValidacion)
            {
                return string.Empty;
            }
            else
            {
                //- Generamos formato html
                StringBuilder str = new StringBuilder();

                str.Append("<ul>");

                if (indicadorPSSE == 0)
                {
                    str.Append("<li>No existe archivo de salida PSS/ODMS (.raw)</li>");
                }

                if (indicadorNCP == -1)
                {
                    str.Append("<li>No se encontró archivos del NCP.</li>");
                }
                else if (indicadorNCP == 2)
                {
                    str.Append("<li>No se encontró datos de la programación NCP</li>");
                }
                else if (indicadorNCP == 4)
                {
                    str.Append("<li>Los archivos del Reprograma no tiene la estructura correcta.</li>");
                }

                if (listRSF.Count() == 0)
                {
                    str.Append("<li>No existe datos en el módulo de RSF.</li>");
                }

                if (modosOperacion.Count() > 0)
                {
                    str.Append("<li>Las siguiente unidades no tienen modos de operación.</li>");
                }

                str.Append("</ul>");
                str.Append("<br />");

                if (modosOperacion.Count() > 0)
                {
                    str.Append("<table border='1' cellspacing = '0' cellpadding='0'>");
                    str.Append("   <thead>");
                    str.Append("       <tr style='background-color:#2980B9; color:#fff'>");
                    str.Append("           <th>Barra</th>");
                    str.Append("           <th>ID</th>");
                    str.Append("           <th>Tensión</th>");
                    str.Append("           <th>Pot. Generada</th>");
                    str.Append("           <th>Ind. Operación</th>");
                    str.Append("       </tr>");
                    str.Append("   </thead>");
                    str.Append("   <tbody>");

                    foreach (RegistroGenerado item in modosOperacion)
                    {
                        str.Append("       <tr>");
                        str.AppendFormat("           <td>{0}</td>", item.BarraNombre);
                        str.AppendFormat("           <td>{0}</td>", item.GenerID);
                        str.AppendFormat("           <td>{0}</td>", item.Tension);
                        str.AppendFormat("           <td>{0}</td>", item.Potencia);
                        str.AppendFormat("           <td>{0}</t>", item.IndOpe);
                        str.Append("       </tr>");
                    }

                    str.Append("   </tbody>");
                    str.Append("</table>");
                }

                str.Append("<br />");

                if (operacionEMS.Count() > 0)
                {
                    str.Append("<ul>");
                    str.Append("<li>Las siguiente unidades tienen modo de operación pero aparecen como no activos en el EMS.</li>");
                    str.Append("</ul>");
                    str.Append("<br />");

                    str.Append("<table border='1' cellspacing = '0' cellpadding='0'>");
                    str.Append("   <thead>");
                    str.Append("       <tr style='background-color:#2980B9; color:#fff'>");
                    str.Append("           <th>Barra</th>");
                    str.Append("           <th>ID</th>");
                    str.Append("           <th>Tensión</th>");
                    str.Append("           <th>Pot. Generada</th>");
                    str.Append("           <th>Ind. Operación</th>");
                    str.Append("       </tr>");
                    str.Append("   </thead>");
                    str.Append("   <tbody>");

                    foreach (RegistroGenerado item in operacionEMS)
                    {
                        str.Append("       <tr>");
                        str.AppendFormat("           <td>{0}</td>", item.BarraNombre);
                        str.AppendFormat("           <td>{0}</td>", item.GenerID);
                        str.AppendFormat("           <td>{0}</td>", item.Tension);
                        str.AppendFormat("           <td>{0}</td>", item.Potencia);
                        str.AppendFormat("           <td>{0}</t>", item.IndOpe);
                        str.Append("       </tr>");
                    }

                    str.Append("   </tbody>");
                    str.Append("</table>");
                    str.Append("<br />");
                }



                return str.ToString();
            }
        }


        /// <summary>
        /// Permite generar el correo
        /// </summary>
        /// <param name="indicadorPSSE">0: No Existe, 1: Existe</param>
        /// <param name="indicadorNCP">-1: Error, 1: Existe programa, 2: No existe programa, 3: Existe reprograma, 4: Reprograma no tiene formato correcto</param>
        /// <param name="modosOperacion"></param>
        /// <param name="listRSF"></param>
        /// <returns></returns>
        public static ResultadoValidacion ObtenerDetalleValidacion(int indicadorPSSE, int indicadorNCP, List<RegistroGenerado> modosOperacion,
            List<EveRsfdetalleDTO> listRSF, List<RegistroGenerado> resultadoEMS, string indicadorNegativo, bool flagPotenciaNegativa,
            bool comparacionRAW, bool indicadorMaximoCMSinCongestion, bool indicadorMaximoConCongestion)
        {
            ResultadoValidacion resultado = new ResultadoValidacion();
            bool flagValidacion = false;
            resultado.IndicadorPSSE = true;
            resultado.IndicadorNCP = true;
            resultado.IndicadorRSF = true;
            resultado.IndicadorMO = true;
            resultado.IndicadorEMS = true;
            resultado.IndicadorNegativo = false;
            resultado.IndicadorGeneracionNegativa = false;
            resultado.IndicadorComparativoRAW = true;

            if (indicadorPSSE == 0 ||
                (indicadorNCP == -1 || indicadorNCP == 2 || indicadorNCP == 4) ||
                modosOperacion.Count() > 0 || resultadoEMS.Count() > 0 ||
                listRSF.Count() == 0 || indicadorNegativo == ConstantesAppServicio.SI
                || flagPotenciaNegativa == true || comparacionRAW == false || indicadorMaximoCMSinCongestion == true || indicadorMaximoConCongestion == true)
            {
                flagValidacion = true;
            }

            if (!flagValidacion)
            {
                resultado.Indicador = true;
            }
            else
            {
                resultado.Indicador = false;

                if (indicadorPSSE == 0)
                {
                    resultado.IndicadorPSSE = false;
                    resultado.ValidacionPSSE = "No existe archivo de salida del TNA (.raw)";
                }

                if (indicadorNCP == -1)
                {
                    resultado.IndicadorNCP = false;
                    resultado.ValidacionNCP = "No se encontró archivos del NCP.";
                }
                else if (indicadorNCP == 2)
                {
                    resultado.IndicadorNCP = false;
                    resultado.ValidacionNCP = "No se encontró datos de la programación NCP.";
                }
                else if (indicadorNCP == 4)
                {
                    resultado.IndicadorNCP = false;
                    resultado.ValidacionNCP = "Los archivos del Reprograma no tiene la estructura correcta.";
                }

                if (listRSF.Count() == 0)
                {
                    resultado.IndicadorRSF = false;
                    resultado.ValidacionRSF = "No existe datos en el módulo de RSF.";
                }

                if (indicadorNegativo == ConstantesAppServicio.SI)
                {
                    resultado.IndicadorNegativo = true;
                    resultado.ValidacionNegativo = "Existen valores negativos en los CM de la última ejecución. Por favor revise.";
                }

                if (modosOperacion.Count() > 0)
                {
                    resultado.IndicadorMO = false;
                    resultado.ValidacionMO = "Las siguiente unidades no tienen modos de operación.";
                }

                if (modosOperacion.Count() > 0)
                {
                    resultado.ListaModosOperacion = new List<ResultadoValidacionItem>();

                    foreach (RegistroGenerado item in modosOperacion)
                    {
                        resultado.ListaModosOperacion.Add
                            (
                                new ResultadoValidacionItem
                                {
                                    BarraNombre = item.BarraNombre,
                                    GenerID = item.GenerID,
                                    Tension = item.Tension,
                                    Potencia = item.Potencia,
                                    IndOpe = item.IndOpe,
                                }
                            );
                    }
                }

                if (resultadoEMS.Count() > 0)
                {
                    resultado.IndicadorEMS = false;
                    resultado.ValidacionEMS = "Las siguientes unidades tienen modo de operación pero aparecen como no operativos en el EMS.";
                }

                if (resultadoEMS.Count() > 0)
                {
                    resultado.ListaOperacionEMS = new List<ResultadoValidacionItem>();

                    foreach (RegistroGenerado item in resultadoEMS)
                    {
                        resultado.ListaOperacionEMS.Add
                            (
                                new ResultadoValidacionItem
                                {
                                    BarraNombre = item.BarraNombre,
                                    GenerID = item.GenerID,
                                    Tension = item.Tension,
                                    Potencia = item.Potencia,
                                    IndOpe = item.IndOpe,
                                }
                            );
                    }
                }

                if (flagPotenciaNegativa == true)
                {
                    resultado.IndicadorGeneracionNegativa = true;
                    resultado.ValidacionGeneracionNegativa = "Existen unidades que tienen generación negativa menor a -5 MW. Favor de sintonizar el EMS";
                }

                if (comparacionRAW == false)
                {
                    resultado.IndicadorComparativoRAW = false;
                    resultado.ValidacionCompartivoRAW = "Los datos de generación en el archivo .raw actual y el archivo .raw de hace media hora son idénticos.";
                }

                if (indicadorMaximoCMSinCongestion == true || indicadorMaximoConCongestion == true)
                {

                    resultado.IndicadorMaximoCM = true;

                    if (indicadorMaximoCMSinCongestion == true && indicadorMaximoConCongestion == false)
                    {
                        resultado.ValidacionMaximoCM = "El máximo valor de CM sin congestión supera en más de 4 al máximo valor de CI de las unidades sin límite de transmisión.";
                    }

                    if (indicadorMaximoCMSinCongestion == false && indicadorMaximoConCongestion == true)
                    {
                        resultado.ValidacionMaximoCM = "El máximo valor de CM con congestión supera en más de 4 al máximo valor de CI de las unidades con límite de transmisión.";
                    }

                    if (indicadorMaximoCMSinCongestion == true && indicadorMaximoConCongestion == true)
                    {
                        resultado.ValidacionMaximoCM = @"El máximo valor de CM sin congestión supera en más de 4 al máximo valor de CI de las unidades sin límite de transmisión. \n
                                                         El máximo valor de CM con congestión supera en más de 4 al máximo valor de CI de las unidades con límite de transmisión.";
                    }

                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite enviar el correo de notificacion del proceso CMgN
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="mensaje"></param>
        public static void EnviarCorreo(DateTime fechaProceso, string mensaje, bool reproceso)
        {
            //string subject = (reproceso) ? ConstantesCortoPlazo.AsuntoReproceso + " - TNA" : ConstantesCortoPlazo.AsuntoCorreo  + " - TNA"  ;
            string subject = ConstantesCortoPlazo.AsuntoCorreo + " - TNA";
            string asunto = string.Format(subject, fechaProceso.ToString("dd/MM/yyyy"), fechaProceso.ToString("HH:mm"));
            string email = ConfigurationManager.AppSettings[ConstantesCortoPlazo.EmailNotificacionCMgN];
            string[] split = email.Split(ConstantesCortoPlazo.CaracterComa);
            List<String> emails = split.ToList();

            COES.Base.Tools.Util.SendEmail(emails, new List<string>(), asunto, mensaje);
        }

        /// <summary>
        /// Permite enviar el mensaje de correo de validacion previa al proceso
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="mensaje"></param>
        public static void EnviarCorreoValidacion(DateTime fechaProceso, string mensaje)
        {
            string asunto = string.Format(ConstantesCortoPlazo.AsuntoValidacionCorreo, fechaProceso.ToString("dd/MM/yyyy"), fechaProceso.ToString("HH:mm"));
            string email = ConfigurationManager.AppSettings[ConstantesCortoPlazo.EmailNotificacionCMgN];
            string[] split = email.Split(ConstantesCortoPlazo.CaracterComa);
            List<String> emails = split.ToList();

            COES.Base.Tools.Util.SendEmail(emails, new List<string>(), asunto, mensaje);
        }

        /// <summary>
        /// Permite enviar el resultado de la ejecución del proceso masivo
        /// </summary>
        /// <param name="list"></param>
        public static void EnviarCorreoEjecucionReprocesoMasivo(List<ResultadoProcesoMasivo> list, DateTime fechaInicio, DateTime fechaFin)
        {
            string subject = string.Format(ConstantesCortoPlazo.AsuntoReprocesoMasivo, fechaInicio.ToString("dd/MM/yyyy"),
                fechaFin.ToString("dd/MM/yyyy"));
            string email = ConfigurationManager.AppSettings[ConstantesCortoPlazo.EmailNotificacionCMgN];
            string[] split = email.Split(ConstantesCortoPlazo.CaracterComa);
            List<String> emails = split.ToList();

            string mensaje = ObtenerCorreoReprocesoMasivo(list, fechaInicio, fechaFin);

            COES.Base.Tools.Util.SendEmail(emails, new List<string>(), subject, mensaje);
        }

        #region Ticket 2022-004345

        public static string ObtenerMensajeValidacionArchivoResultado(List<string> validaciones)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Se han identificado errores al momento de generar el archivo del preprocesador .gen, por lo que no se generó el archivo .dat, los motivos son los siguientes: <br/> <br />");
            str.Append("<ul>");

            foreach(string item in validaciones)
            {
                str.Append("<li>" + item + "</li>");
            }

            str.Append("</ul>");

            return str.ToString();
        }

        #endregion
    }
}
