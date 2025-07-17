using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.ServicioRPF.Helper;
using System.Globalization;
using System.Configuration;
using OfficeOpenXml;
using System.IO;

namespace COES.Servicios.Aplicacion.ServicioRPF
{
    public class RpfAppServicio : AppServicioBase
    {
        public List<int> CodigosDisponibles = new List<int>();
        List<RegistrorpfDTO> ListaRegistro = new List<RegistrorpfDTO>();

        /// <summary>
        /// Permite obtener el listado de unidades que deben cargar
        /// </summary>
        /// <returns></returns>
        public List<ServicioRpfDTO> ObtenerUnidadesCarga(DateTime fechaPeriodo)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().GetByCriteria(fechaPeriodo);
        }

        /// <summary>
        /// Permite obtener la reserva primaria en un instante de tiempo
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal? ObtenerReservaPrimaria(DateTime fecha)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ObtenerReservaPrimaria(fecha);
        }

        /// <summary>
        /// Permite obtener la frecuencia de los últimos 10 segundos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<decimal> ObtenerFrecuenciasSanJuan(DateTime fecha)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ObtenerFrecuenciasSanJuan(fecha);
        }

        /// <summary>
        /// Permite obtener las frecuencias de las 600 segundos de SE San Juan
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<decimal> ObtenerFrecuenciaSanJuanTotal(DateTime fecha)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ObtenerFrecuenciaSanJuanTotal(fecha);
        }

        /// <summary>
        /// Permite obtener las frecuencias para la evaluación de consistencia
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<decimal> ObtenerFrecuenciasComparacion(DateTime fecha)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ObtenerFrecuenciasComparacion(fecha);
        }

        /// <summary>
        /// Permite determinar el estado operativo de una unidad
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int ObtenerEstadoOperativo(int equicodi, DateTime fecha)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ValidarExistenciaHoraOperacion(equicodi, fecha);
        }

        /// <summary>
        /// Permite listar las gps del coes
        /// </summary>
        /// <returns></returns>
        public List<ServicioGps> ObtenerGPS(DateTime fecha)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ObtenerGPS(fecha);
        }

        /// <summary>
        /// Permite obtener la consulta de GPS
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<ServicioGps> ObtenerConsultaGPS(DateTime fecha)
        {
            List<ServicioGps> entitys = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerGPS(fecha);

            if (entitys.Where(x => x.GpsCodi == 1).Count() == 0)
            {
                ServicioGps entity = new ServicioGps();
                entity.Cantidad = 0;
                entity.Fecha = fecha;
                entity.GpsCodi = 1;
                entity.GpsNombre = ConstantesServicioRPF.GpsSanJuan;
                entity.IndicadorCompletado = ConstantesAppServicio.SI;
                entitys.Add(entity);
            }
            else
            {
                ServicioGps entity = entitys.Where(x => x.GpsCodi == 1).FirstOrDefault();

                if (entity.Cantidad == 1440)
                {
                    entity.IndicadorCompletado = ConstantesAppServicio.NO;

                    List<ServicioGps> list = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerConsultaFrecuencia(fecha, 1);
                    List<ServicioGps> listCero = list.Where(x => x.Frecuencia <= 0).ToList();

                    if (listCero.Count > 0)
                    {
                        entity.IndicadorCompletado = ConstantesAppServicio.SI;
                    }
                }
                else
                {
                    entity.IndicadorCompletado = ConstantesAppServicio.SI;
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite listar las frecuencias de un gps
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="gpsCodi"></param>
        /// <returns></returns>
        public List<ServicioGps> ObtenerConsultaFrecuencia(DateTime fecha, int gpsCodi)
        {
            return Factory.FactorySic.ObtenerServicioRpfDao().ObtenerConsultaFrecuencia(fecha, gpsCodi);
        }

        /// <summary>
        /// Permite reemplazar los valoes de las frecuencias entre dos gps
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="gpsOrigen"></param>
        /// <param name="gpsDestino"></param>
        public void ReemplazarFrecuencias(DateTime fecha, int gpsOrigen, int gpsDestino)
        {
            try
            {
                Factory.FactorySic.ObtenerServicioRpfDao().ReemplazarFrecuencias(fecha, gpsOrigen, gpsDestino);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar las frecuencias de san juan
        /// </summary>
        /// <param name="fecha"></param>
        public int CompletarFrecuenciaSanJuan(DateTime fecha)
        {
            try
            {
                int count = Factory.FactorySic.ObtenerServicioRpfDao().VerificarFrecuenciaSanJuan(fecha);

                if (count < 1440)
                {
                    Factory.FactorySic.ObtenerServicioRpfDao().CompletarFrecuenciaSanJuan(fecha, 46);
                    count = Factory.FactorySic.ObtenerServicioRpfDao().VerificarFrecuenciaSanJuan(fecha);

                    if (count < 1440)
                    {
                        Factory.FactorySic.ObtenerServicioRpfDao().CompletarFrecuenciaSanJuan(fecha, 2);
                        count = Factory.FactorySic.ObtenerServicioRpfDao().VerificarFrecuenciaSanJuan(fecha);

                        if (count < 1440)
                        {
                            Factory.FactorySic.ObtenerServicioRpfDao().CompletarFrecuenciaSanJuan(fecha, 30);
                            count = Factory.FactorySic.ObtenerServicioRpfDao().VerificarFrecuenciaSanJuan(fecha);

                            if (count < 1440)
                            {
                                Factory.FactorySic.ObtenerServicioRpfDao().CompletarFrecuenciaSanJuan(fecha, 20);
                            }
                        }
                    }
                }

                if (count == 1440)
                {
                    bool flagCero = true;
                    List<ServicioGps> list = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerConsultaFrecuencia(fecha, 1);
                    List<ServicioGps> listCero = list.Where(x => x.Frecuencia <= 0).ToList();

                    foreach (ServicioGps item in listCero)
                    {
                        decimal? valor = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerValorActualizar(item.Fecha, 46);

                        if (valor != null)
                        {
                            Factory.FactorySic.ObtenerServicioRpfDao().ActualizarValorFrecuencia(item.Fecha, (decimal)valor);
                        }
                        else
                        {
                            valor = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerValorActualizar(item.Fecha, 2);

                            if (valor != null)
                            {
                                Factory.FactorySic.ObtenerServicioRpfDao().ActualizarValorFrecuencia(item.Fecha, (decimal)valor);
                            }
                            else
                            {
                                valor = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerValorActualizar(item.Fecha, 30);

                                if (valor != null)
                                {
                                    Factory.FactorySic.ObtenerServicioRpfDao().ActualizarValorFrecuencia(item.Fecha, (decimal)valor);
                                }
                                else
                                {
                                    valor = Factory.FactorySic.ObtenerServicioRpfDao().ObtenerValorActualizar(item.Fecha, 20);

                                    if (valor != null)
                                    {
                                        Factory.FactorySic.ObtenerServicioRpfDao().ActualizarValorFrecuencia(item.Fecha, (decimal)valor);
                                    }
                                    else
                                    {
                                        flagCero = false;
                                    }
                                }

                            }
                        }
                    }

                    if (!flagCero) return 2;

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        #region MigracionSGOCOES-GrupoB RPF

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptosMedicion"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<ResultadoverificacionDTO> VerificarEnvio(int[] ptosMedicion, DateTime fechaConsulta)
        {
            List<int> resultado = FactorySic.GetMeMedicion60Repository().VerificarCarga(fechaConsulta);
            List<ResultadoverificacionDTO> entitys = new List<ResultadoverificacionDTO>();

            foreach (int punto in ptosMedicion)
            {
                ResultadoverificacionDTO entity = new ResultadoverificacionDTO();
                entity.PtoMediCodi = punto;

                if (resultado.Contains(punto)) { entity.IndicadorEnvio = "S"; }
                else { entity.IndicadorEnvio = "N"; }

                entitys.Add(entity);
            }

            return entitys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptosMedicion"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<MeMedicion60DTO> DescargarEnvio(List<int> ptosMedicion, DateTime fechaConsulta)
        {
            return FactorySic.GetMeMedicion60Repository().DescargarEnvio(ptosMedicion, fechaConsulta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="ajuste"></param>
        /// <param name="intentos"></param>
        /// <param name="cantidad"></param>
        /// <param name="nroDatos"></param>
        /// <param name="idsCargaron"></param>
        /// <param name="noEncontrados"></param>
        /// <param name="potenciaCero"></param>
        /// <param name="frecuenciaCero"></param>
        /// <returns></returns>
        public List<RegistrorpfDTO> ObtenerDatosAnalisis(DateTime fecha, decimal ajuste, int intentos, int cantidad, int nroDatos, out List<int> idsCargaron, out List<int> noEncontrados, out List<int> potenciaCero, out List<int> frecuenciaCero)
        {
            List<int> CodigosDisponibles = new List<int>();
            List<RegistrorpfDTO> ListaRegistro = new List<RegistrorpfDTO>();

            List<int> ids = FactorySic.GetMeMedicion60Repository().VerificarCarga(fecha);
            List<int> idsPotenciaCero = FactorySic.GetMeMedicion60Repository().ObtenerValorencero(fecha, ConstantesAppServicio.TipoinfocodiMW);
            List<int> idsFrecuenciaCero = FactorySic.GetMeMedicion60Repository().ObtenerValorencero(fecha, ConstantesAppServicio.TipoinfocodiHZ);

            idsCargaron = new List<int>(ids);
            CodigosDisponibles = ids;

            foreach (int idPotencia in idsPotenciaCero)
            {
                CodigosDisponibles.Remove(idPotencia);
            }

            foreach (int idFrecuencia in idsFrecuenciaCero)
            {
                CodigosDisponibles.Remove(idFrecuencia);
            }

            this.Procesar(fecha, ajuste, nroDatos);

            for (int i = 0; i < intentos; i++)
            {
                if (CodigosDisponibles.Count >= cantidad)
                {
                    this.Procesar(fecha, ajuste, nroDatos);
                }
                else
                {
                    break;
                }
            }

            noEncontrados = this.CodigosDisponibles;
            potenciaCero = idsPotenciaCero;
            frecuenciaCero = idsFrecuenciaCero;

            return this.ListaRegistro;
        }

        /// <summary>
        /// Permite obtener los periodos de evaluación
        /// </summary>
        /// <param name="fecha"></param>
        private void Procesar(DateTime fecha, decimal ajuste, int nroDatos)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            this.GenerarFecha(fecha, out fechaInicio, out fechaFin, nroDatos);
            List<RegistrorpfDTO> list = FactorySic.GetMeMedicion60Repository().ObtenerRango(string.Join(",", this.CodigosDisponibles), fechaInicio, fechaFin);

            this.AnalizarDatos(new List<int>(this.CodigosDisponibles), list, fechaInicio.Second - 1, ajuste, fechaInicio, fechaFin, nroDatos);
        }

        /// <summary>
        /// Permite obtener el rango de datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="fechaInico"></param>
        /// <param name="fechaFin"></param>
        private void GenerarFecha(DateTime fecha, out DateTime fechaInico, out DateTime fechaFin, int nroDatos)
        {
            Random random = new Random();
            int secondInicio = random.Next(0, 86400 - 1 - nroDatos);
            int secondFin = secondInicio + nroDatos;

            fechaInico = fecha.AddSeconds(secondInicio);
            DateTime fecFin = fecha.AddSeconds(secondFin);

            if (fecFin.Second > 0) { fechaFin = fecFin.AddMinutes(1); }
            else { fechaFin = fecFin; }
        }

        /// <summary>
        /// Analizamos los datos para cada punto de medición
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="list"></param>
        private void AnalizarDatos(List<int> ids, List<RegistrorpfDTO> list, int segundoInicial, decimal ajuste,
            DateTime fechaInicio, DateTime fechaFin, int nroDatos)
        {
            List<RegistrorpfDTO> listPotencia = list.Where(x => x.TIPOINFOCODI == 1).ToList();
            List<RegistrorpfDTO> listFrecuencia = list.Where(x => x.TIPOINFOCODI == 6).ToList();

            foreach (int id in ids)
            {
                bool flag = true;
                List<RegistrorpfDTO> listFrecuenciaAnalisis = listFrecuencia.Where(x => x.PTOMEDICODI == id)
                    .Skip(segundoInicial).Take(nroDatos).Where(x => x.VALOR >= 59.85m && x.VALOR <= 60.15m).ToList();

                if (listFrecuenciaAnalisis.Count == nroDatos)
                {
                    List<RegistrorpfDTO> listPotenciaAnalisis = listPotencia.Where(x => x.PTOMEDICODI == id).Skip(segundoInicial).Take(nroDatos).ToList();

                    decimal valor = listPotenciaAnalisis[0].VALOR;
                    if (valor > 0)
                    {
                        for (int i = 1; i < listPotenciaAnalisis.Count; i++)
                        {
                            if (id != 53) //quitar condicional
                            {
                                if (!(Math.Abs((listPotenciaAnalisis[i].VALOR - valor) * 100 / valor) <= ajuste) || listPotenciaAnalisis[i].VALOR == 0)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                    }

                    if (flag)
                    {
                        List<RegistrorpfDTO> resultado = new List<RegistrorpfDTO>();
                        for (int i = 0; i < nroDatos; i++)
                        {
                            RegistrorpfDTO item = new RegistrorpfDTO();
                            item.PTOMEDICODI = id;
                            item.FECHAHORA = listFrecuenciaAnalisis[i].FECHAHORA;
                            item.SEGUNDO = listFrecuenciaAnalisis[i].SEGUNDO;
                            item.FRECUENCIA = listFrecuenciaAnalisis[i].VALOR;
                            item.POTENCIA = listPotenciaAnalisis[i].VALOR;
                            item.HORAINICIO = fechaInicio;
                            item.HORAFIN = fechaInicio.AddSeconds(nroDatos);
                            resultado.Add(item);
                        }

                        this.ListaRegistro.AddRange(resultado);
                        this.CodigosDisponibles.Remove(id);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public List<MeMedicion60DTO> ObtenerDatosComparacionRangoResolucion(DateTime fechaConsulta, string ptomedicodi, int resolucion)
        {
            return FactorySic.GetMeMedicion60Repository().ObtenerDatosComparacionRango(fechaConsulta, ptomedicodi, resolucion, this.VerificarTipoConsultaRpf(fechaConsulta));
        }

        /// <summary>
        /// Verificacion de tiempo estimado para consultar data historica o 12 tablas
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        private string VerificarTipoConsultaRpf(DateTime fechaConsulta)
        {
            string mes = string.Empty, tiempoConsulta = ConfigurationManager.AppSettings["TiempoConsultaRPF"].ToString();
            if (!tiempoConsulta.Equals(string.Empty))
            {
                DateTime f_ = DateTime.Now.AddMonths(int.Parse(tiempoConsulta) * -1);
                if (fechaConsulta.Month >= f_.Month) { mes = "_" + fechaConsulta.Month; }
            }
            return mes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<RegistrorpfDTO> ObtenerPotenciasMaximas(DateTime fechaConsulta)
        {
            return FactorySic.GetMeMedicion60Repository().ObtenerPotenciasMaximas(fechaConsulta);
        }

        public List<RegistrorpfDTO> ObtenerRangoSegundos(DateTime fecha, decimal ajuste, int intentos, int cantidad, int nroDatos, decimal fmaxgen, decimal fmingen, decimal flimmax, decimal flimmin, decimal balance, out List<int> idsCargaron,
            out List<int> noEncontrados, out List<int> potenciaCero, out List<int> frecuenciaCero)
        {
            #region Declaración de variables

            List<int> ids = FactorySic.GetMeMedicion60Repository().VerificarCarga(fecha);
            List<int> idsPotenciaCero = FactorySic.GetMeMedicion60Repository().ObtenerValorencero(fecha, ConstantesAppServicio.TipoinfocodiMW);
            List<int> idsFrecuenciaCero = FactorySic.GetMeMedicion60Repository().ObtenerValorencero(fecha, ConstantesAppServicio.TipoinfocodiHZ);

            idsCargaron = new List<int>(ids);
            this.CodigosDisponibles = ids;

            foreach (int idPotencia in idsPotenciaCero)
            {
                this.CodigosDisponibles.Remove(idPotencia);
            }

            foreach (int idFrecuencia in idsFrecuenciaCero)
            {
                this.CodigosDisponibles.Remove(idFrecuencia);
            }

            #endregion

            this.IterarCalculo(fecha, ajuste, intentos, cantidad, nroDatos, fmaxgen, fmingen, flimmax, flimmin, balance);

            if (this.CodigosDisponibles.Count > cantidad)
            {
                nroDatos = nroDatos - 60;
                this.IterarCalculo(fecha, ajuste, intentos, cantidad, nroDatos, fmaxgen, fmingen, flimmax, flimmin, balance);

                if (this.CodigosDisponibles.Count > cantidad)
                {
                    balance = balance * 0.75M;
                    this.IterarCalculo(fecha, ajuste, intentos, cantidad, nroDatos, fmaxgen, fmingen, flimmax, flimmin, balance);
                }
            }

            noEncontrados = this.CodigosDisponibles;
            potenciaCero = idsPotenciaCero;
            frecuenciaCero = idsFrecuenciaCero;

            return this.ListaRegistro;
        }

        /// <summary>
        /// Permite realizar el llamado a las iteraciones de calculo para la obtención del rango
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="ajuste"></param>
        /// <param name="nroDatos"></param>
        /// <param name="fmaxgen"></param>
        /// <param name="fmingen"></param>
        /// <param name="flimmax"></param>
        /// <param name="flimmin"></param>
        /// <param name="balance"></param>
        private void IterarCalculo(DateTime fecha, decimal ajuste, int intentos, int cantidad, int nroDatos, decimal fmaxgen,
            decimal fmingen, decimal flimmax, decimal flimmin, decimal balance)
        {
            this.ProcesarRango(fecha, ajuste, nroDatos, fmaxgen, fmingen, flimmax, flimmin, balance);

            for (int i = 0; i < intentos; i++)
            {
                if (this.CodigosDisponibles.Count >= cantidad)
                {
                    this.ProcesarRango(fecha, ajuste, nroDatos, fmaxgen, fmingen, flimmax, flimmin, balance);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Permite obtener los periodos de evaluación
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="ajuste"></param>
        /// <param name="nroDatos"></param>
        private void ProcesarRango(DateTime fecha, decimal ajuste, int nroDatos, decimal fmaxgen, decimal fmingen,
            decimal flimmax, decimal flimmin, decimal balance)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            this.GenerarFecha(fecha, out fechaInicio, out fechaFin, nroDatos);
            List<RegistrorpfDTO> list = FactorySic.GetMeMedicion60Repository().ObtenerRango(string.Join(",", this.CodigosDisponibles), fechaInicio, fechaFin);

            this.AnalizarDatosRango(new List<int>(this.CodigosDisponibles), list, fechaInicio.Second - 1, ajuste, fechaInicio, fechaFin, nroDatos,
                fmaxgen, fmingen, flimmax, flimmin, balance);
        }

        /// <summary>
        /// Permite analizar datos para cada punto de medicion
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="list"></param>
        /// <param name="segundoInicial"></param>
        /// <param name="ajuste"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroDatos"></param>
        public void AnalizarDatosRango(List<int> ids, List<RegistrorpfDTO> list, int segundoInicial, decimal ajuste, DateTime fechaInicio,
            DateTime fechaFin, int nroDatos, decimal fmaxgen, decimal fmingen, decimal flimmax, decimal flimmin, decimal balance)
        {
            List<RegistrorpfDTO> listPotencia = list.Where(x => x.TIPOINFOCODI == 1).ToList();
            List<RegistrorpfDTO> listFrecuencia = list.Where(x => x.TIPOINFOCODI == 6).ToList();

            foreach (int id in ids)
            {
                bool flag = true;
                List<RegistrorpfDTO> listPotenciaAnalisis = listPotencia.Where(x => x.PTOMEDICODI == id).Skip(segundoInicial).Take(nroDatos).ToList();

                decimal valor = listPotenciaAnalisis[0].VALOR;
                if (valor > 0)
                {
                    for (int i = 1; i < listPotenciaAnalisis.Count; i++)
                    {
                        if (!(Math.Abs((listPotenciaAnalisis[i].VALOR - valor) * 100 / valor) <= ajuste) || listPotenciaAnalisis[i].VALOR == 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                else
                {
                    flag = false;
                }

                if (flag)
                {
                    List<RegistrorpfDTO> listFrecuenciaAnalisis = listFrecuencia.Where(x => x.PTOMEDICODI == id)
                    .Skip(segundoInicial).Take(nroDatos).Where(x => x.VALOR >= fmingen && x.VALOR <= fmaxgen).ToList();

                    if (listFrecuenciaAnalisis.Count == nroDatos)
                    {
                        int countMax = listFrecuenciaAnalisis.Where(x => x.VALOR >= flimmax).Count();
                        int countMin = listFrecuenciaAnalisis.Where(x => x.VALOR <= flimmin).Count();

                        if (countMax * 100 / nroDatos >= balance && countMin * 100 / nroDatos >= balance)
                        {
                            List<RegistrorpfDTO> resultado = new List<RegistrorpfDTO>();
                            for (int i = 0; i < nroDatos; i++)
                            {
                                RegistrorpfDTO item = new RegistrorpfDTO();
                                item.PTOMEDICODI = id;
                                item.FECHAHORA = listFrecuenciaAnalisis[i].FECHAHORA;
                                item.SEGUNDO = listFrecuenciaAnalisis[i].SEGUNDO;
                                item.FRECUENCIA = listFrecuenciaAnalisis[i].VALOR;
                                item.POTENCIA = listPotenciaAnalisis[i].VALOR;
                                item.HORAINICIO = fechaInicio;
                                item.HORAFIN = fechaInicio.AddSeconds(nroDatos);
                                item.BALANCE = balance;
                                item.NRODATOS = nroDatos;
                                resultado.Add(item);
                            }

                            this.ListaRegistro.AddRange(resultado);
                            this.CodigosDisponibles.Remove(id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<RegistrorpfDTO> ObtenerDatosFallas(DateTime fecha)
        {
            DateTime fechaInicial = fecha.AddSeconds(-10);
            DateTime fechaFinal = fecha.AddMinutes(11); //modificamos tambien esto

            int segundoInicial = fecha.Second;
            List<int> ids = FactorySic.GetMeMedicion60Repository().VerificarCarga(fecha);

            List<RegistrorpfDTO> list = FactorySic.GetMeMedicion60Repository().ObtenerRango(ConstantesAppServicio.ParametroDefecto, fechaInicial, fechaFinal);

            List<RegistrorpfDTO> resultado = new List<RegistrorpfDTO>();

            int nroSegundos = 611; // tambien modificamos esta linea

            foreach (int id in ids)
            {
                int skip = segundoInicial - 10;
                if (fechaInicial.Minute != fecha.Minute)
                {
                    skip = 50 + fecha.Second;
                }

                List<RegistrorpfDTO> listItem = list.Where(x => x.PTOMEDICODI == id).ToList();
                List<RegistrorpfDTO> listFrecuencia = listItem.Where(x => x.TIPOINFOCODI == ConstantesAppServicio.TipoinfocodiHZ).OrderBy
                    (x => x.FECHAHORA).Skip(skip).Take(nroSegundos).ToList();
                List<RegistrorpfDTO> listPotencia = listItem.Where(x => x.TIPOINFOCODI == ConstantesAppServicio.TipoinfocodiMW).OrderBy
                    (x => x.FECHAHORA).Skip(skip).Take(nroSegundos).ToList();

                if (listFrecuencia.Count == listPotencia.Count)
                {
                    int i = 0;

                    foreach (RegistrorpfDTO item in listFrecuencia)
                    {
                        RegistrorpfDTO entity = new RegistrorpfDTO();

                        entity.PTOMEDICODI = item.PTOMEDICODI;
                        entity.FECHAHORA = item.FECHAHORA;
                        entity.SEGUNDO = item.SEGUNDO;
                        entity.FRECUENCIA = item.VALOR;
                        entity.POTENCIA = listPotencia[i].VALOR;

                        resultado.Add(entity);
                        i++;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<ReporteEnvioDTO> ObtenerReporteEnvio(DateTime fechaConsulta)
        {
            return FactorySic.GetMeMedicion60Repository().ObtenerReporte(fechaConsulta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="frecuencias"></param>
        /// <param name="porcentaje"></param>
        /// <param name="potencia"></param>
        /// <param name="valorPercentil"></param>
        /// <returns></returns>
        public int ConsultaEnvioDatos(DateTime fecha, List<decimal> frecuencias, decimal porcentaje, decimal potencia, decimal valorPercentil)
        {
            try
            {
                FactorySic.GetMeMedicion60Repository().EliminarReporte(fecha);

                List<ResultadoverificacionDTO> entitys = new List<ResultadoverificacionDTO>();
                List<int> ids = FactorySic.GetMeMedicion60Repository().VerificarCarga(fecha);

                foreach (int idPto in ids)
                {
                    List<RegistrorpfDTO> list = this.ObtenerDatosEvaluacionConsistencia(fecha, ids, frecuencias, potencia, idPto);

                    //foreach (int id in ids)
                    //{
                    ResultadoverificacionDTO entity = new ResultadoverificacionDTO();
                    entity.IndicadorConsistencia = ConstantesServicioRPF.NO;
                    entity.PtoMediCodi = idPto;

                    double[] datos = list.Where(x => x.INDICADORPOT == ConstantesServicioRPF.SI).Select(x => (double)x.DIFERENCIA).ToArray();

                    if (datos.Length > 0)
                    {
                        double percentil = this.ObtienePercentil(datos, (double)porcentaje);
                        entity.ValorConsistencia = (decimal)percentil * 1000;

                        if (percentil * 1000 <= (double)valorPercentil)
                        {
                            entity.IndicadorConsistencia = ConstantesServicioRPF.SI;
                            entity.EstadoInformacion = ConstantesServicioRPF.OK;
                        }
                        else
                        {
                            entity.EstadoInformacion = ConstantesServicioRPF.Inconsistente;
                        }

                        entity.EstadoOperativo = ConstantesServicioRPF.OPERO;
                    }
                    else
                    {
                        entity.EstadoInformacion = ConstantesServicioRPF.CERO;
                        entity.EstadoOperativo = ConstantesServicioRPF.NO;
                    }

                    LogEnvioMedicionDTO log = FactorySic.GetMeMedicion60Repository().ObtenerLogPorPuntoFecha(idPto, fecha);

                    if (log != null)
                    {
                        entity.FechaCarga = log.LastDate;
                    }

                    ReporteEnvioDTO item = new ReporteEnvioDTO();

                    item.Fecha = fecha;
                    item.EstadoInformacion = entity.EstadoInformacion;
                    item.EstadoOperativo = entity.EstadoOperativo;
                    item.FechaCarga = entity.FechaCarga;
                    item.IndConsistencia = entity.IndicadorConsistencia;
                    item.PtoMediCodi = entity.PtoMediCodi;
                    item.ValConsistencia = entity.ValorConsistencia;

                    FactorySic.GetMeMedicion60Repository().GrabarReporte(item);
                }

                return 1;
            }
            catch (Exception ex)
            {
                FactorySic.GetMeMedicion60Repository().GrabarLogReporte(ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el percentil de un conjunto de datos
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="percentile"></param>
        /// <returns></returns>
        private double ObtienePercentil(double[] seq, double percentile)
        {
            var elements = seq;
            Array.Sort(elements);
            double realIndex = percentile * (elements.Length - 1);
            int index = (int)realIndex;
            double frac = realIndex - index;
            if (index + 1 < elements.Length)
                return elements[index] * (1 - frac) + elements[index + 1] * frac;
            else
                return elements[index];
        }

        /// <summary>
        /// Permite obtener los datos para la evaluación de consistencia
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private List<RegistrorpfDTO> ObtenerDatosEvaluacionConsistencia(DateTime fecha, List<int> ids, List<decimal> frecuencias, decimal potencia, int idPto)
        {
            DateTime fechaInicial = fecha;
            DateTime fechaFinal = fecha.AddDays(1).AddSeconds(-1);

            List<RegistrorpfDTO> list = this.ObtenerDatosFallaConsistencia(fechaInicial, fechaFinal, idPto);

            List<RegistrorpfDTO> resultado = new List<RegistrorpfDTO>();

            if (frecuencias.Count == list.Count)
            {
                int i = 0;
                foreach (RegistrorpfDTO item in list)
                {
                    item.DIFERENCIA = Math.Abs(frecuencias[i] - item.FRECUENCIA);
                    item.INDICADORPOT = ConstantesServicioRPF.NO;
                    if (item.POTENCIA > potencia)
                    {
                        item.INDICADORPOT = ConstantesServicioRPF.SI;
                    }

                    i++;
                }

                resultado = list;
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idPto"></param>
        /// <returns></returns>
        private List<RegistrorpfDTO> ObtenerDatosFallaConsistencia(DateTime fechaInicial, DateTime fechaFinal, int idPto)
        {
            var Listatmp1 = FactorySic.GetMeMedicion60Repository().ListaMedicionesTmp(fechaInicial, fechaFinal, idPto, ConstantesAppServicio.TipoinfocodiMW);
            var Listatmp2 = FactorySic.GetMeMedicion60Repository().ListaMedicionesTmp(fechaInicial, fechaFinal, idPto, ConstantesAppServicio.TipoinfocodiHZ);

            var lista = (from f in Listatmp2
                         join p in Listatmp1 on new MeMedicion60DTO()
                         {
                             Ptomedicodi = f.Ptomedicodi,
                             Fechahora = f.Fechahora,
                             SEGUNDO = f.SEGUNDO
                         }
                         equals
                         new MeMedicion60DTO()
                         {
                             Ptomedicodi = p.Ptomedicodi,
                             Fechahora = p.Fechahora,
                             SEGUNDO = p.SEGUNDO
                         }
                         orderby new { f.Ptomedicodi, f.Fechahora }
                         select new RegistrorpfDTO()
                         {
                             PTOMEDICODI = f.Ptomedicodi,
                             TIPOINFOCODI = f.Tipoinfocodi,
                             FECHAHORA = f.Fechahora,
                             SEGUNDO = f.SEGUNDO,
                             FRECUENCIA = f.VALOR,
                             POTENCIA = p.VALOR
                         }).ToList();

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerPuntosMedicion(int idEmpresa)
        {
            return FactorySic.GetMePtomedicionRepository().ListarPtosMedicionXEmpresa(idEmpresa);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="codigos"></param>
        /// <param name="validaciones"></param>
        /// <param name="indicador"></param>
        /// <param name="fechaSeleccion"></param>
        /// <param name="indicadorValidacionHora"></param>
        /// <returns></returns>
        public List<MeMedicion60DTO> ProcesarArchivoRpf(string file, out List<string> validaciones, out bool indicador, DateTime fechaSeleccion, string indicadorValidacionHora
            , MeCabeceraDTO cabecera, List<MeHojaptomedDTO> ListaHojaPto, int nFil)
        {
            List<String> errors = new List<String>();
            List<MeMedicion60DTO> list = new List<MeMedicion60DTO>();

            bool flag = true;

            #region Lectura completa del archivo

            List<string> arreglo = new List<string>();
            List<string> ptoMedicion = new List<string>();
            List<string> tipoInfo = new List<string>();
            //string[] fecha = null;

            char[] separer = new char[1];
            separer[0] = ',';

            string line = null;

            FileInfo fileInfo = new FileInfo(file);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                /// Verificar Formato
                for (int i = 1; i <= cabecera.Cabfilas - 4; i++)
                {
                    for (int j = 2; j <= ListaHojaPto.Count + 1; j++)
                    {
                        switch (i)
                        {
                            case 1: if (ws.Cells[i, j].Value != null) { ptoMedicion.Add(ws.Cells[i, j].Value.ToString()); } break;
                            case 2: if (ws.Cells[i, j].Value != null) { tipoInfo.Add(ws.Cells[i, j].Value.ToString()); } break;
                        }
                    }
                }
                for (int i = 6; i <= nFil + 6; i++)
                {
                    List<string> arr = new List<string>();
                    for (int j = 2; j <= ListaHojaPto.Count + 1; j++)
                    {
                        if (ws.Cells[i, j].Value != null)
                        {
                            arr.Add(ws.Cells[i, j].Value.ToString());
                        }
                    }
                    arreglo.Add(string.Join(",", arr));
                }
            }
            //using (System.IO.StreamReader sr = System.IO.File.OpenText(file))
            //{
            //    ptoMedicion = sr.ReadLine().Split(';');
            //    tipoInfo = sr.ReadLine().Split(';');
            //    //fecha = sr.ReadLine().Split(separer, StringSplitOptions.RemoveEmptyEntries);
            //    ptoMedicion = ptoMedicion.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //    for (int t = 1; t <= 4; t++) sr.ReadLine();
            //    int i = 0;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        if (i == 0)
            //        {
            //            string[] verificacion = line.Split(separer, StringSplitOptions.RemoveEmptyEntries);
            //            bool flagInicio = false;
            //            if (verificacion.Length > 0)
            //            {
            //                if (verificacion[0].Trim() == ConstantesRpf.HoraCero || verificacion[0].Trim() == ConstantesRpf.HoraCeroAlter)
            //                    flagInicio = true;
            //            }
            //            if (!flagInicio)
            //                errors.Add(ValidacionArchivoRpf.DatosInicio);
            //        }
            //        arreglo.Add(line.Split(separer, StringSplitOptions.RemoveEmptyEntries));
            //        i++;
            //    }
            //}

            #endregion

            #region Validacion 2 primeras líneas

            //if (ptoMedicion.Count == tipoInfo.Count)
            //{
            //    if (ptoMedicion.Count > 2 && (ptoMedicion.Count - 1) % 2 == 0)
            //    {
            //        for (int i = 1; i <= ptoMedicion.Count - 1; i++)
            //        {
            //            int codigoPunto = 0;
            //            int codigoInfo = 0;

            //            if (!int.TryParse(ptoMedicion[i], out codigoPunto))
            //                errors.Add(String.Format(ValidacionArchivoRpf.FormatoIncorrectoPtoMedicion, (i + 1).ToString()));
            //            if (!int.TryParse(tipoInfo[i], out codigoInfo))
            //                errors.Add(String.Format(ValidacionArchivoRpf.FormatoIncorrectoTipoInfo, (i + 1).ToString()));

            //            if (i % 2 == 0)
            //            {
            //                if (ptoMedicion[i] != ptoMedicion[i - 1])
            //                    errors.Add(String.Format(ValidacionArchivoRpf.CodigosPuntosIguales, (i + 1).ToString()));
            //                if (tipoInfo[i] != ConstantesAppServicio.TipoinfocodiHZ.ToString())
            //                    errors.Add(String.Format(ValidacionArchivoRpf.CodigosValorFrecuencia, (i + 1).ToString()));
            //            }
            //            else
            //            {
            //                if (codigoPunto > 0)
            //                {
            //                    if (!codigos.Contains(codigoPunto))
            //                        errors.Add(String.Format(ValidacionArchivoRpf.CodigoInvalido, (i + 1).ToString()));
            //                }
            //                if (tipoInfo[i] != ConstantesAppServicio.TipoinfocodiMW.ToString())
            //                    errors.Add(String.Format(ValidacionArchivoRpf.CodigosValorPotencia, (i + 1).ToString()));
            //            }
            //        }
            //    }
            //    else
            //    {
            //        errors.Add(ValidacionArchivoRpf.VerificarCantidadCodigos);
            //    }
            //}
            //else
            //{
            //    errors.Add(ValidacionArchivoRpf.CantidadPtoMedicionVSTipoInfo);
            //}

            //if (errors.Count > 0) flag = false;

            #endregion

            #region Validacion de fechas

            //DateTime today = DateTime.Now;
            //bool validacionFecha = true;

            //if (fecha.Length == 4)
            //{
            //    int anio = 0; int.Parse(fecha[1]);
            //    int mes = 0; int.Parse(fecha[2]);
            //    int dia = 0; int.Parse(fecha[3]);

            //    if (!int.TryParse(fecha[1], out anio))
            //    {
            //        errors.Add(ValidacionArchivoRpf.AnioIncorrecto);
            //        validacionFecha = false;
            //    }
            //    if (!int.TryParse(fecha[2], out mes))
            //    {
            //        errors.Add(ValidacionArchivoRpf.MesIncorrecto);
            //        validacionFecha = false;
            //    }
            //    if (!int.TryParse(fecha[3], out dia))
            //    {
            //        errors.Add(ValidacionArchivoRpf.DiaIncorrecto);
            //        validacionFecha = false;
            //    }

            //    today = DateTime.ParseExact(dia.ToString().PadLeft(2, ConstantesRpf.CaracterCero) + ConstantesRpf.CaracterSlash +
            //        mes.ToString().PadLeft(2, ConstantesRpf.CaracterCero) + ConstantesRpf.CaracterSlash + anio, ConstantesRpf.FormatoFecha, CultureInfo.InvariantCulture);

            //    if (!(fechaSeleccion.Year == today.Year && fechaSeleccion.Month == today.Month && fechaSeleccion.Day == today.Day))
            //    {
            //        errors.Add(ValidacionArchivoRpf.FechasNoCoinciden);
            //        validacionFecha = false;
            //    }

            //    if (validacionFecha)
            //    {
            //        if (indicadorValidacionHora == ConstantesRpf.SI)
            //        {
            //            //DateTime fechaActual = DateTime.Now.AddHours(-5);
            //            DateTime fechaActual = DateTime.Now;
            //            string horaPermitida = ConstantesRpf.HoraPermitida;

            //            string[] strHoraPermitida = horaPermitida.Split(ConstantesRpf.SeparadorPuntos);
            //            int hora = int.Parse(strHoraPermitida[0]);
            //            int minuto = int.Parse(strHoraPermitida[1]);

            //            //debemos corregir esta línea.

            //            int dias = (int)fechaActual.Subtract(today).TotalDays;

            //            if (dias == 1)
            //            {
            //                if (fechaActual.Hour >= hora)
            //                {
            //                    errors.Add(String.Format(ValidacionArchivoRpf.HoraNoPermitida, horaPermitida));
            //                }
            //            }
            //            else
            //            {
            //                errors.Add(ValidacionArchivoRpf.FechaNoPermitidaDiaAnterior);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    errors.Add(ValidacionArchivoRpf.FechaNoValida);
            //}

            //if (errors.Count > 0) flag = false;


            #endregion

            #region Valicion de secuencia de datos

            //int index = 0;
            //bool flagFila = true;
            //for (int i = 1; i <= 24; i++)
            //{
            //    for (int j = 1; j <= 60; j++)
            //    {
            //        for (int k = 1; k <= 60; k++)
            //        {
            //            if (arreglo[index].Split(separer).Length == ptoMedicion.Count)
            //            {
            //                //if (arreglo[index][0].Split(':').Count == 3)
            //                //{
            //                //    string cad = arreglo[index][0].Split(':')[2];

            //                //    if ((k - 1) != int.Parse(cad))
            //                //    {
            //                //        errors.Add(String.Format(ValidacionArchivoRpf.NoExisteRegistro, (i - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero) +
            //                //            ConstantesRpf.CaracterPuntos + (j - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero) + ConstantesRpf.CaracterPuntos +
            //                //            (k - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero)));
            //                //        flagFila = false;
            //                //        break;
            //                //    }
            //                //}
            //                //else
            //                //{
            //                //    errors.Add(String.Format(ValidacionArchivoRpf.FechaNoTieneFormato, (i - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero) +
            //                //            ConstantesRpf.CaracterPuntos + (j - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero) + ConstantesRpf.CaracterPuntos +
            //                //            (k - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero)));
            //                //    flagFila = false;
            //                //    break;
            //                //}
            //            }
            //            else
            //            {
            //                errors.Add(String.Format(ValidacionArchivoRpf.FechaNoDatosCompleto, (i - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero) +
            //                           ConstantesRpf.CaracterPuntos + (j - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero) + ConstantesRpf.CaracterPuntos +
            //                           (k - 1).ToString().PadLeft(2, ConstantesRpf.CaracterCero)));
            //                flagFila = false;
            //                break;
            //            }
            //            index++;
            //        }
            //        if (!flagFila) break;
            //    }
            //    if (!flagFila) break;
            //}

            //if (!flagFila) flag = false;

            #endregion

            #region Cargado de datos

            bool flagNegativo = true;

            if (flag)
            {
                int ptoMediCodi = 0;
                int tipoInfoCodi = 0;
                decimal valor = 0;
                int indice = 0;

                for (int i = 0; i < ptoMedicion.Count; i++)
                {
                    ptoMediCodi = int.Parse(ptoMedicion[i]);
                    tipoInfoCodi = int.Parse(tipoInfo[i]);
                    for (int j = 1; j <= 24; j++)
                    {
                        for (int k = 1; k <= 60; k++)
                        {
                            MeMedicion60DTO entity = new MeMedicion60DTO();
                            entity.Ptomedicodi = ptoMediCodi;
                            entity.Tipoinfocodi = tipoInfoCodi;
                            entity.Fechahora = fechaSeleccion.AddHours(j - 1).AddMinutes(k - 1);

                            for (int t = 1; t <= 60; t++)
                            {
                                indice = (j - 1) * 60 * 60 + (k - 1) * 60 + t;
                                valor = 0;
                                if (decimal.TryParse(arreglo[indice].Split(separer)[i], out valor))
                                {
                                    if (valor >= 0)
                                        entity.GetType().GetProperty("H" + (t - 1)).SetValue(entity, valor);
                                    else
                                    {
                                        if (!flagNegativo) continue;
                                        errors.Add(ValidacionArchivoRpf.ValoresNegativosNoPermitidos);
                                        flagNegativo = false;
                                        flag = false;
                                    }
                                }
                                else
                                {
                                    errors.Add(String.Format(ValidacionArchivoRpf.ValidacionFormatoNumero, (indice + 8).ToString(), (i + 1).ToString()));
                                    flag = false;
                                }
                            }

                            list.Add(entity);
                        }
                    }
                }
            }

            #endregion

            validaciones = errors;
            indicador = flag;

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fechaCarga"></param>
        public void GrabarDatosRpf(List<MeMedicion60DTO> list, DateTime fechaCarga, int mes)
        {
            try
            {
                FactorySic.GetMeMedicion60Repository().GrabarDatosRpf(list, fechaCarga, mes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GrabarLogEnvio(LogEnvioMedicionDTO entity)
        {
            try
            {
                return FactorySic.GetMeMedicion60Repository().GrabarLogEnvio(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminacion de carga RPF
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <param name="fecha1"></param>
        /// <param name="fecha2"></param>
        /// <param name="mes"></param>
        /// <param name="join"></param>
        /// <param name="fecha"></param>
        /// <returns></returns> 
        public int EliminarCargaRpf(string ptomedicodi, DateTime fecha1, DateTime fecha2, int mes, string tipoinfocodi)
        {
            return FactorySic.GetMeMedicion60Repository().EliminarCargaRpf(ptomedicodi, fecha1, fecha2, mes, tipoinfocodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="ptomedicion"></param>
        /// <param name="idtipodato"></param>
        /// <returns></returns>
        public List<MeMedicion60DTO> BuscarDatosRpf(DateTime fechaini, DateTime fechafin, int ptomedicion, int idtipodato)
        {
            return FactorySic.GetMeMedicion60Repository().BuscarDatosRpf(fechaini, fechafin, ptomedicion, idtipodato);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lista"></param>
        /// <returns></returns>
        public string DatosRpfHtml(List<MeMedicion60DTO> Lista)
        {
            StringBuilder strHtml = new StringBuilder();

            #region cabecera
            //***************************      CABECERA DE LA TABLA         ***********************************//

            strHtml.Append("<table class='pretty tabla-icono' id='tb_info'>");

            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th>Hora</th>");
            for (int i = 1; i <= 60; i++)
            {
                strHtml.Append("<th>" + i + "''</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            //***************************      CUERPO DE LA TABLA         ***********************************//
            strHtml.Append("<tbody>");

            if (Lista.Count > 0)
            {
                foreach (var d in Lista)
                {
                    strHtml.Append("<tr>");
                    strHtml.Append("<td>" + d.Fechahora.ToString("HH:mm") + "</td>");
                    for (int i = 0; i < 60; i++)
                    {
                        strHtml.Append("<td>" + d.GetType().GetProperty("H" + i).GetValue(d, null) + "</td>");
                    }
                    strHtml.Append("</tr>");
                }
            }
            else
            {
                strHtml.Append("<tr><td colspan=61 style='text-align:center'>No existen datos.</td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<MeMedicion60DTO> ProcesarArchivoZipRpf(string file)
        {
            List<MeMedicion60DTO> Lista = new List<MeMedicion60DTO>();

            string[] lineasCSV = System.IO.File.ReadAllLines(file, System.Text.Encoding.Default);
            List<string> lista = new List<string>(lineasCSV);
            lista.RemoveAt(0);

            foreach (var d in lista)
            {
                var arr = d.Replace("\"", "").Split(',');
                var obj = new MeMedicion60DTO();
                obj.Ptomedicodi = int.Parse(arr[0]);
                obj.Tipoinfocodi = int.Parse(arr[1]);
                var arrfec = arr[2].Split(' ');
                int anio = int.Parse(arrfec[0].Split('/')[2]);
                int mes = int.Parse(arrfec[0].Split('/')[0]);
                int dia = int.Parse(arrfec[0].Split('/')[1]);
                string mes_ = "0" + mes;
                obj.Fechahora = DateTime.Parse(dia + "/" + mes_.Substring(mes_.Length - 2) + "/" + anio + " " + arrfec[1] + " " + arrfec[2].ToLower());

                for (int i = 0; i <= 59; i++)
                {
                    obj.GetType().GetProperty("H" + i).SetValue(obj, decimal.Parse(arr[i + 3]));
                }

                Lista.Add(obj);
            }

            return Lista;
        }

        #endregion
    }
}
