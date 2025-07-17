using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COES.Servicios.Aplicacion.General
{
    public class ParametroAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ContactoAppServicio));

        #region Métodos Tabla SI_PARAMETRO

        /// <summary>
        /// Inserta un registro de la tabla SI_PARAMETRO
        /// </summary>
        public void SaveSiParametro(SiParametroDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PARAMETRO
        /// </summary>
        public void UpdateSiParametro(SiParametroDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PARAMETRO
        /// </summary>
        public void DeleteSiParametro(int siparcodi)
        {
            try
            {
                FactorySic.GetSiParametroRepository().Delete(siparcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PARAMETRO
        /// </summary>
        public SiParametroDTO GetByIdSiParametro(int siparcodi)
        {
            return FactorySic.GetSiParametroRepository().GetById(siparcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PARAMETRO
        /// </summary>
        public List<SiParametroDTO> ListSiParametros()
        {
            return FactorySic.GetSiParametroRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiParametro
        /// </summary>
        public List<SiParametroDTO> GetByCriteriaSiParametros()
        {
            return FactorySic.GetSiParametroRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SI_PARAMETRO
        /// </summary>
        public int SaveSiParametroId(SiParametroDTO entity)
        {
            return FactorySic.GetSiParametroRepository().SaveSiParametroId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SI_PARAMETRO
        /// </summary>
        public List<SiParametroDTO> BuscarOperaciones(string abreviatura, string descripcion, int nroPage, int pageSize)
        {
            return FactorySic.GetSiParametroRepository().BuscarOperaciones(abreviatura, descripcion, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SI_PARAMETRO
        /// </summary>
        public int ObtenerNroFilas(string abreviatura, string descripcion)
        {
            return FactorySic.GetSiParametroRepository().ObtenerNroFilas(abreviatura, descripcion);
        }

        #endregion

        #region Métodos Tabla SI_PARAMETRO_VALOR

        /// <summary>
        /// Inserta un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public void SaveSiParametroValor(SiParametroValorDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroValorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public void UpdateSiParametroValor(SiParametroValorDTO entity)
        {
            try
            {
                FactorySic.GetSiParametroValorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public void DeleteSiParametroValor(int siparvcodi)
        {
            try
            {
                FactorySic.GetSiParametroValorRepository().Delete(siparvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public SiParametroValorDTO GetByIdSiParametroValor(int siparvcodi)
        {
            return FactorySic.GetSiParametroValorRepository().GetById(siparvcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public List<SiParametroValorDTO> ListSiParametroValors()
        {
            return FactorySic.GetSiParametroValorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiParametroValor
        /// </summary>
        public List<SiParametroValorDTO> GetByCriteriaSiParametroValors()
        {
            return FactorySic.GetSiParametroValorRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public int SaveSiParametroValorId(SiParametroValorDTO entity)
        {
            return FactorySic.GetSiParametroValorRepository().SaveSiParametroValorId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public List<SiParametroValorDTO> BuscarOperaciones(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, int nroPage, int pageSize, string estado)
        {
            return FactorySic.GetSiParametroValorRepository().BuscarOperaciones(siparCodi, siparvFechaInicial, siparvFechaFinal, nroPage, pageSize, estado);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla SI_PARAMETRO_VALOR
        /// </summary>
        public int ObtenerNroFilas(int siparCodi, DateTime siparvFechaInicial, DateTime siparvFechaFinal, string estado)
        {
            return FactorySic.GetSiParametroValorRepository().ObtenerNroFilas(siparCodi, siparvFechaInicial, siparvFechaFinal, estado);
        }

        /// <summary>
        /// Obtiene los valores de una propiedad
        /// </summary>
        /// <param name="parametro"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal ObtenerValorParametro(int parametro, DateTime fecha)
        {
            decimal valor = FactorySic.GetSiParametroValorRepository().ObtenerValorParametro(parametro, fecha);

            return valor;
        }

        /// <summary>
        /// valor vigente del parámetro numérico
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public decimal ObtenerValorVigente(int parametro)
        {
            var listaParametroValor = ListSiParametroValorByIdParametro(parametro).OrderByDescending(x => x.Siparvfeccreacion).ToList();

            if (listaParametroValor.Any())
            {
                var regParam1 = listaParametroValor.First(); // toma el último elemento guardado
                return regParam1.Siparvvalor ?? 0;
            }

            return 0;
        }

        /// <summary>
        /// Lista de parmetro valor segun parametro
        /// </summary>
        /// <param name="siparcodi"></param>
        /// <returns></returns>
        public List<SiParametroValorDTO> ListSiParametroValorByIdParametro(int siparcodi)
        {
            return FactorySic.GetSiParametroValorRepository().ListByIdParametro(siparcodi).OrderBy(x => x.Siparvcodi).ToList();
        }

        /// <summary>
        /// Lista de parmetro valor segun parametro y fecha
        /// </summary>
        /// <param name="siparcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<SiParametroValorDTO> ListSiParametroValorByIdParametroAndFechaInicial(int siparcodi, DateTime fecha)
        {
            return FactorySic.GetSiParametroValorRepository().ListByIdParametroAndFechaInicial(siparcodi, fecha);
        }
        #endregion

        /// <summary>
        /// Convertir cantidad de minutos a HH:mm
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ConvertirMinutosFormatoCadena(SiParametroValorDTO param)
        {
            if (param != null)
            {
                int valor = Decimal.ToInt32(param.Siparvvalor.GetValueOrDefault(0));//la hora guardado en cantidad de minutos
                if (valor >= 0)
                {
                    int hora = valor / (60);
                    int minuto = valor - (hora * 60);

                    return hora.ToString("00") + ":" + minuto.ToString("00");
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Convertir HH:mm a cantidad de minutos
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static int ConvertirMinutosFormatoNumero(string hora)
        {
            try
            {
                if (hora != null)
                {
                    hora = hora.Trim();
                    if (hora.Length == 5)
                    {
                        string parteHora = hora.Substring(0, 2);
                        string parteMin = hora.Substring(3, 2);
                        int minhora = Int32.Parse(parteHora) * 60;
                        int min = Int32.Parse(parteMin);

                        return minhora + min;
                    }
                }

                return -1;
            }
            catch (FormatException e)
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtener el H segun la resolucion
        /// </summary>
        /// <param name="min"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public static int GetHFromMinutosYResolucion(int min, int resolucion)
        {
            if (min > 0)
            {
                switch (resolucion)
                {
                    case ParametrosFormato.ResolucionCuartoHora:

                        if (min % 15 == 0)
                        {
                            return min / 15;
                        }
                        else
                        {
                            return (min / 15) + 1;
                        }

                    case ParametrosFormato.ResolucionMediaHora:

                        if (min % 30 == 0)
                        {
                            return min / 30;
                        }
                        else
                        {
                            return (min / 30) + 1;
                        }
                }
            }
            return 1;
        }

        #region Rango de Operación de Centrales Solares

        /// <summary>
        /// Lista de ParametroRangoSolar desde los 2 parametros (HoraIni, HoraFin) 
        /// </summary>
        /// <param name="listaParam"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public List<ParametroRangoSolar> GetListaParametroSolar(List<SiParametroValorDTO> listaParam, List<EstadoParametro> listaEstado, int resolucion)
        {
            List<ParametroRangoSolar> lista = new List<ParametroRangoSolar>();

            List<DateTime> listaFecha = listaParam.Select(x => x.Siparvfechainicial.Value.Date).Distinct().ToList();
            foreach (var fecha in listaFecha)
            {
                var listaHora = listaParam.Where(x => x.Siparvfechainicial.Value.Date == fecha).ToList();
                var paramHoraIni = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorHoraInicioSolar).First();
                var paramHoraFin = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorHoraFinSolar).First();

                ParametroRangoSolar rangoSolar = GetParametroSolar(paramHoraIni, paramHoraFin, listaEstado, resolucion);

                lista.Add(rangoSolar);
            }

            return lista;
        }

        /// <summary>
        /// ParametroRangoSolar desde los 2 parametros (HoraIni, HoraFin) 
        /// </summary>
        /// <param name="paramHoraIni"></param>
        /// <param name="paramHoraFin"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public ParametroRangoSolar GetParametroSolar(SiParametroValorDTO paramHoraIni, SiParametroValorDTO paramHoraFin, List<EstadoParametro> listaEstado, int resolucion)
        {
            ParametroRangoSolar rangoSolar = new ParametroRangoSolar();
            rangoSolar.Fecha = paramHoraIni.Siparvfechainicial.Value.Date;
            rangoSolar.FechaFormato = rangoSolar.Fecha.ToString(ConstantesBase.FormatoFechaBase);

            rangoSolar.SiParvcodiHoraInicio = paramHoraIni.Siparvcodi;
            rangoSolar.HoraInicio = ConvertirMinutosFormatoCadena(paramHoraIni);
            rangoSolar.HInicio = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rangoSolar.HoraInicio), resolucion);

            rangoSolar.SiParvcodiHoraFin = paramHoraFin.Siparvcodi;
            rangoSolar.HoraFin = ConvertirMinutosFormatoCadena(paramHoraFin);
            rangoSolar.HFin = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rangoSolar.HoraFin), resolucion);

            rangoSolar.Estado = paramHoraIni.Siparveliminado;
            if (listaEstado != null)
            {
                rangoSolar.EstadoValor = listaEstado.Where(x => x.EstadoCodigo == rangoSolar.Estado).FirstOrDefault().EstadoDescripcion;
            }

            rangoSolar.Siparvusucreacion = paramHoraIni.Siparvusucreacion;
            rangoSolar.Siparvfeccreacion = paramHoraIni.Siparvfeccreacion != null ? paramHoraIni.Siparvfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            rangoSolar.Siparvusumodificacion = paramHoraIni.Siparvusumodificacion;
            rangoSolar.Siparvfecmodificacion = paramHoraIni.Siparvfecmodificacion != null ? paramHoraIni.Siparvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            return rangoSolar;
        }

        /// <summary>
        /// Obtener Parametro RangoCentralSolar
        /// </summary>
        /// <param name="listaRangoSolar"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public SiParametroValorDTO GetParametroRangoCentralSolar(List<SiParametroValorDTO> listaRangoSolar, DateTime fechaPeriodo, int resolucion)
        {
            List<SiParametroValorDTO> listaRango = listaRangoSolar.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList().OrderByDescending(x => x.Siparvfechainicial).ToList();
            var lista = this.GetListaParametroSolar(listaRango, null, resolucion);

            DateTime fechaIni = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            DateTime fechaFin = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            fechaFin = fechaFin.AddMonths(1).AddDays(-1);

            //completar rango
            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Siparvfechainicial = lista[i].Fecha;
                    lista[i].Siparvfechainicial = new DateTime(lista[i].Siparvfechainicial.Year, lista[i].Siparvfechainicial.Month, 1);

                    if (i == 0)
                    {
                        lista[i].Siparvfechafinal = fechaFin;
                    }
                    else
                    {
                        lista[i].Siparvfechafinal = lista[i - 1].Siparvfechainicial.AddDays(-1);
                    }
                    lista[i].Siparvfechafinal = new DateTime(lista[i].Siparvfechafinal.Year, lista[i].Siparvfechafinal.Month, 1);
                    lista[i].Siparvfechafinal = lista[i].Siparvfechafinal.AddMonths(1).AddDays(-1);
                }
            }

            foreach (var param in lista)
            {
                SiParametroValorDTO paramHora = new SiParametroValorDTO();

                if (param.Siparvfechainicial.Date <= fechaIni && fechaFin <= param.Siparvfechafinal.Date)
                {
                    paramHora.HInicio = param.HInicio;
                    paramHora.HFin = param.HFin;
                    paramHora.HoraInicio = param.HoraInicio;
                    paramHora.HoraFin = param.HoraFin;

                    return paramHora;
                }
            }

            return null;
        }

        #endregion

        #region Hora Punta para Potencia Activa

        /// <summary>
        /// Lista ParametroHPPotenciaActiva desde (HoraMinima, HoraMedia, HoraMaxima)
        /// </summary>
        /// <param name="listaParam"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="tipMedicion"></param>
        /// <returns></returns>
        public static List<ParametroHPPotenciaActiva> GetListaParametroHPPotenciaActiva(List<SiParametroValorDTO> listaParam, DateTime fechaConsulta, int tipMedicion)
        {
            listaParam = listaParam ?? new List<SiParametroValorDTO>();

            //fechas
            List<DateTime> listaFecha = listaParam.Select(x => x.Siparvfechainicial.Value.Date).Distinct().OrderBy(x => x).ToList();

            //agrupar 
            List<ParametroHPPotenciaActiva> lista = new List<ParametroHPPotenciaActiva>();
            foreach (var fecha in listaFecha)
            {
                var listaHora = listaParam.Where(x => x.Siparvfechainicial.Value.Date == fecha).ToList();
                var paramHoraMinima = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorHoraMinimaHP).First();
                var paramHoraMedia = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorHoraMediaHP).First();
                var paramHoraMaxima = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorHoraMaximaHP).First();

                ParametroHPPotenciaActiva rango = GetParametroHPPotenciaActiva(paramHoraMinima, paramHoraMedia, paramHoraMaxima, tipMedicion);

                lista.Add(rango);
            }

            //vigencia
            var regActivo = lista.Where(x => x.Estado == ConstantesParametro.EstadoActivo).OrderBy(x => x.Fecha).Where(x => x.Fecha <= fechaConsulta.Date).LastOrDefault();
            if (regActivo != null)
            {
                regActivo.EsVigente = true;
                regActivo.VigenteDesc = "Vigente";
            }

            return lista;
        }

        /// <summary>
        /// ParametroHPPotenciaActiva desde (HoraMinima, HoraMedia, HoraMaxima)
        /// </summary>
        /// <param name="paramHoraMinima"></param>
        /// <param name="paramHoraMedia"></param>
        /// <param name="paramHoraMaxima"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public static ParametroHPPotenciaActiva GetParametroHPPotenciaActiva(SiParametroValorDTO paramHoraMinima, SiParametroValorDTO paramHoraMedia, SiParametroValorDTO paramHoraMaxima, int resolucion)
        {
            ParametroHPPotenciaActiva rango = new ParametroHPPotenciaActiva();
            rango.Fecha = paramHoraMinima.Siparvfechainicial.Value.Date;
            rango.FechaFormato = rango.Fecha.ToString(ConstantesBase.FormatoFechaBase);

            rango.SiParvcodiHoraMinima = paramHoraMinima.Siparvcodi;
            rango.HoraMinima = ConvertirMinutosFormatoCadena(paramHoraMinima);
            rango.HMaxFinMinima = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.HoraMinima), resolucion);

            rango.SiParvcodiHoraMedia = paramHoraMedia.Siparvcodi;
            rango.HoraMedia = ConvertirMinutosFormatoCadena(paramHoraMedia);
            rango.HMaxFinMedia = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.HoraMedia), resolucion);

            rango.SiParvcodiHoraMaxima = paramHoraMaxima.Siparvcodi;
            rango.HoraMaxima = ConvertirMinutosFormatoCadena(paramHoraMaxima);
            rango.HMaxFinMaxima = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.HoraMaxima), resolucion);

            rango.Estado = paramHoraMinima.Siparveliminado;
            if (rango.Estado == ConstantesParametro.EstadoActivo)
            {
                rango.VigenteDesc = "No vigente";
            }
            else
            {
                rango.VigenteDesc = "Eliminado";
            }

            switch (rango.Estado)
            {
                case ConstantesParametro.EstadoBaja:
                    rango.ClaseFila = "fila_anulado";
                    break;
                default:
                    rango.ClaseFila = string.Empty;
                    break;
            }

            rango.Siparvusucreacion = paramHoraMinima.Siparvusucreacion;
            rango.Siparvfeccreacion = paramHoraMinima.Siparvfeccreacion != null ? paramHoraMinima.Siparvfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            rango.Siparvusumodificacion = paramHoraMinima.Siparvusumodificacion;
            rango.Siparvfecmodificacion = paramHoraMinima.Siparvfecmodificacion != null ? paramHoraMinima.Siparvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            return rango;
        }

        /// <summary>
        /// Lista ParametroRangoPeriodoHP
        /// </summary>
        /// <param name="listaParam"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public List<ParametroRangoPeriodoHP> GetListaParametroRangoPeriodoHP(List<SiParametroValorDTO> listaParam, List<EstadoParametro> listaEstado)
        {
            List<ParametroRangoPeriodoHP> lista = new List<ParametroRangoPeriodoHP>();

            foreach (var param in listaParam)
            {
                ParametroRangoPeriodoHP rango = GetParametroRangoPeriodoHP(param, listaEstado);
                lista.Add(rango);
            }

            return lista;
        }

        /// <summary>
        /// ParametroRangoPeriodoHP
        /// </summary>
        /// <param name="param"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public ParametroRangoPeriodoHP GetParametroRangoPeriodoHP(SiParametroValorDTO param, List<EstadoParametro> listaEstado)
        {
            ParametroRangoPeriodoHP rango = new ParametroRangoPeriodoHP();
            rango.FechaInicio = param.Siparvfechainicial.Value.Date;
            rango.FechaFin = param.Siparvfechafinal.Value.Date;
            rango.FechaFormatoInicio = rango.FechaInicio.ToString(ConstantesBase.FormatoFechaBase);
            rango.FechaFormatoFin = rango.FechaFin.ToString(ConstantesBase.FormatoFechaBase);
            rango.SiParvcodi = param.Siparvcodi;
            rango.Estado = param.Siparveliminado;
            if (listaEstado != null)
            {
                rango.EstadoValor = listaEstado.Where(x => x.EstadoCodigo == rango.Estado).FirstOrDefault().EstadoDescripcion;
            }
            rango.Normativa = param.Siparvnota != null ? param.Siparvnota.Trim() : string.Empty;

            rango.Siparvusucreacion = param.Siparvusucreacion;
            rango.Siparvfeccreacion = param.Siparvfeccreacion != null ? param.Siparvfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            rango.Siparvusumodificacion = param.Siparvusumodificacion;
            rango.Siparvfecmodificacion = param.Siparvfecmodificacion != null ? param.Siparvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            switch (rango.Estado)
            {
                case ConstantesParametro.EstadoAnulado:
                    rango.ClaseFila = "fila_anulado";
                    break;
                case ConstantesParametro.EstadoBaja:
                    rango.ClaseFila = "fila_baja";
                    break;
                case ConstantesParametro.EstadoActivo:
                    rango.Editable = true;
                    break;
                case ConstantesParametro.EstadoPendiente:
                    rango.Editable = true;
                    break;
                default:
                    rango.Editable = false;
                    rango.ClaseFila = string.Empty;
                    break;
            }

            return rango;
        }

        /// <summary>
        /// Obtener Periodo por norma
        /// </summary>
        /// <param name="listaFecha"></param>
        /// <param name="fechaAhora"></param>
        /// <returns></returns>
        public static int GetPeriodoPorNorma(List<SiParametroValorDTO> listaFecha, DateTime fechaAhora)
        {
            //Valor por defecto. Considerar todo el día para obtener el máximo valor
            int periodo = ConstantesRepMaxDemanda.TipoMaximaTodoDia;

            //Solo considerar registros activos, no lo anulados
            listaFecha = listaFecha ?? new List<SiParametroValorDTO>();
            var listaFechaHP = listaFecha.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();

            //verificar que la fecha de consulta esta dentro de la norma
            foreach (var param in listaFechaHP)
            {
                DateTime fechaIni = param.Siparvfechainicial.Value.Date;
                DateTime fechaFin = param.Siparvfechafinal.Value.Date;

                if (fechaAhora.Date >= fechaIni && fechaAhora.Date <= fechaFin)
                {
                    periodo = ConstantesRepMaxDemanda.TipoHoraPunta; //con norma, solo hora punta
                }
            }

            return periodo;
        }

        /// <summary>
        /// GetParametroVigenteHPyHFPXResolucion
        /// </summary>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public static SiParametroValorDTO GetParametroVigenteHPyHFPXResolucion(List<SiParametroValorDTO> listaBloqueHorario, DateTime fechaConsulta, int resolucion)
        {
            var lista = GetListaParametroHPPotenciaActiva(listaBloqueHorario, fechaConsulta, resolucion);
            var regVigente = lista.Find(x => x.EsVigente);

            SiParametroValorDTO paramHP = new SiParametroValorDTO();

            if (regVigente != null)
            {
                paramHP.HMaxFinMinima = regVigente.HMaxFinMinima;
                paramHP.HMaxFinMedia = regVigente.HMaxFinMedia;
                paramHP.HMaxFinMaxima = regVigente.HMaxFinMaxima; //si es 30 min restar 1
            }
            else
            {
                //valores default
                paramHP.HMaxFinMinima = 1;
                paramHP.HMaxFinMedia = 24;
                paramHP.HMaxFinMaxima = 48;
            }

            paramHP.HFueraHoraPuntaFin = paramHP.HMaxFinMedia;
            paramHP.HDespuesHoraPuntaFin = paramHP.HMaxFinMaxima + 1;

            //Indisponibilidades
            paramHP.HIniHP = paramHP.HMaxFinMedia + 1; //18:15 o 18:30
            paramHP.HFinHP = paramHP.HMaxFinMaxima;
            paramHP.SegIniHP = paramHP.HMaxFinMedia * resolucion * 60; //18:00 en segundos
            paramHP.SegFinHP = paramHP.HFinHP * resolucion * 60;
            paramHP.TotalHorasHP = (paramHP.SegFinHP - paramHP.SegIniHP) / (60 * 60);

            return paramHP;
        }

        /// <summary>
        /// Obtener Parametro HPPotenciaActiva
        /// </summary>
        /// <param name="listaHP"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        public SiParametroValorDTO GetParametroHPPotenciaActiva(List<SiParametroValorDTO> listaHP, DateTime fechaPeriodo, int resolucion)
        {
            List<SiParametroValorDTO> listaRangoHP = listaHP.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo || x.Siparveliminado == ConstantesParametro.EstadoBaja)
                                                        .ToList().OrderByDescending(x => x.Siparvfechainicial).ToList();

            var lista = GetListaParametroHPPotenciaActiva(listaRangoHP, fechaPeriodo, resolucion);

            DateTime fechaIni = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            DateTime fechaFin = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            fechaFin = fechaFin.AddMonths(1).AddDays(-1);

            //completar rango
            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Siparvfechainicial = lista[i].Fecha;
                    lista[i].Siparvfechainicial = new DateTime(lista[i].Siparvfechainicial.Year, lista[i].Siparvfechainicial.Month, 1);

                    if (i == 0)
                    {
                        lista[i].Siparvfechafinal = fechaFin;
                    }
                    else
                    {
                        lista[i].Siparvfechafinal = lista[i - 1].Siparvfechainicial.AddDays(-1);
                    }
                    lista[i].Siparvfechafinal = new DateTime(lista[i].Siparvfechafinal.Year, lista[i].Siparvfechafinal.Month, 1);
                    lista[i].Siparvfechafinal = lista[i].Siparvfechafinal.AddMonths(1).AddDays(-1);
                }
            }

            foreach (var param in lista)
            {
                SiParametroValorDTO paramHora = new SiParametroValorDTO();

                if (param.Siparvfechainicial.Date <= fechaIni && fechaFin <= param.Siparvfechafinal.Date)
                {
                    paramHora.HMaxFinMinima = param.HMaxFinMinima;
                    paramHora.HMaxFinMedia = param.HMaxFinMedia;
                    paramHora.HMaxFinMaxima = param.HMaxFinMaxima;

                    return paramHora;
                }
            }

            return null;
        }

        /// <summary>
        /// Eliminar Parametros HP Potencia Activa
        /// </summary>
        /// <param name="idHoraMinima"></param>
        /// <param name="idHoraMedia"></param>
        /// <param name="idHoraMaxima"></param>
        /// <param name="usuario"></param>
        public void EliminarParametrosHPPotenciaActiva(int idHoraMinima, int idHoraMedia, int idHoraMaxima, string usuario)
        {
            SiParametroValorDTO paramHoraMinima = GetByIdSiParametroValor(idHoraMinima);
            paramHoraMinima.Siparveliminado = ConstantesParametro.EstadoBaja;
            paramHoraMinima.Siparvusumodificacion = usuario;
            paramHoraMinima.Siparvfecmodificacion = DateTime.Now;

            SiParametroValorDTO paramHoraMedia = GetByIdSiParametroValor(idHoraMedia);
            paramHoraMedia.Siparveliminado = ConstantesParametro.EstadoBaja;
            paramHoraMedia.Siparvusumodificacion = usuario;
            paramHoraMedia.Siparvfecmodificacion = DateTime.Now;

            SiParametroValorDTO paramHoraMaxima = GetByIdSiParametroValor(idHoraMaxima);
            paramHoraMaxima.Siparveliminado = ConstantesParametro.EstadoBaja;
            paramHoraMaxima.Siparvusumodificacion = usuario;
            paramHoraMaxima.Siparvfecmodificacion = DateTime.Now;

            //update BD
            UpdateSiParametroValor(paramHoraMinima);
            UpdateSiParametroValor(paramHoraMedia);
            UpdateSiParametroValor(paramHoraMaxima);
        }

        #endregion

        #region Rango de Análisis de Potencia Inductiva

        /// <summary>
        /// Lista ParametroRangoPotenciaInductiva desde (H1Ini, H1Fin, H2Ini, H2Fin)
        /// </summary>
        /// <param name="listaParam"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public List<ParametroRangoPotenciaInductiva> GetListaParametroRangoPotenciaInductiva(List<SiParametroValorDTO> listaParam, List<EstadoParametro> listaEstado, int resolucion)
        {
            List<ParametroRangoPotenciaInductiva> lista = new List<ParametroRangoPotenciaInductiva>();

            List<DateTime> listaFecha = listaParam.Select(x => x.Siparvfechainicial.Value.Date).Distinct().ToList();
            foreach (var fecha in listaFecha)
            {
                var listaHora = listaParam.Where(x => x.Siparvfechainicial.Value.Date == fecha).ToList();
                var paramH1Ini = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorH1Ini).First();
                var paramH1Fin = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorH1Fin).First();
                var paramH2Ini = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorH2Ini).First();
                var paramH2Fin = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorH2Fin).First();

                ParametroRangoPotenciaInductiva rango = GetParametroRangoPotenciaInductiva(paramH1Ini, paramH1Fin, paramH2Ini, paramH2Fin, listaEstado, resolucion);

                lista.Add(rango);
            }

            return lista;
        }

        /// <summary>
        /// ParametroRangoPotenciaInductiva desde (H1Ini, H1Fin, H2Ini, H2Fin)
        /// </summary>
        /// <param name="paramH1Ini"></param>
        /// <param name="paramH1Fin"></param>
        /// <param name="paramH2Ini"></param>
        /// <param name="paramH2Fin"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public ParametroRangoPotenciaInductiva GetParametroRangoPotenciaInductiva(SiParametroValorDTO paramH1Ini, SiParametroValorDTO paramH1Fin, SiParametroValorDTO paramH2Ini, SiParametroValorDTO paramH2Fin, List<EstadoParametro> listaEstado, int resolucion)
        {
            ParametroRangoPotenciaInductiva rango = new ParametroRangoPotenciaInductiva();
            rango.Fecha = paramH1Ini.Siparvfechainicial.Value.Date;
            rango.FechaFormato = rango.Fecha.ToString(ConstantesBase.FormatoFechaBase);

            rango.SiParvcodiH1Ini = paramH1Ini.Siparvcodi;
            rango.H1Ini = ConvertirMinutosFormatoCadena(paramH1Ini);
            rango.HRango1Ini = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.H1Ini), resolucion);

            rango.SiParvcodiH1Fin = paramH1Fin.Siparvcodi;
            rango.H1Fin = ConvertirMinutosFormatoCadena(paramH1Fin);
            rango.HRango1Fin = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.H1Fin), resolucion);

            rango.SiParvcodiH2Ini = paramH2Ini.Siparvcodi;
            rango.H2Ini = ConvertirMinutosFormatoCadena(paramH2Ini);
            rango.HRango2Ini = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.H2Ini), resolucion);

            rango.SiParvcodiH2Fin = paramH2Fin.Siparvcodi;
            rango.H2Fin = ConvertirMinutosFormatoCadena(paramH2Fin);
            rango.HRango2Fin = GetHFromMinutosYResolucion(ConvertirMinutosFormatoNumero(rango.H2Fin), resolucion);

            rango.Estado = paramH1Ini.Siparveliminado;
            if (listaEstado != null)
            {
                rango.EstadoValor = listaEstado.Where(x => x.EstadoCodigo == rango.Estado).FirstOrDefault().EstadoDescripcion;
            }

            rango.Siparvusucreacion = paramH1Ini.Siparvusucreacion;
            rango.Siparvfeccreacion = paramH1Ini.Siparvfeccreacion != null ? paramH1Ini.Siparvfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            rango.Siparvusumodificacion = paramH1Ini.Siparvusumodificacion;
            rango.Siparvfecmodificacion = paramH1Ini.Siparvfecmodificacion != null ? paramH1Ini.Siparvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            switch (rango.Estado)
            {
                case ConstantesParametro.EstadoAnulado:
                    rango.ClaseFila = "fila_anulado";
                    break;
                case ConstantesParametro.EstadoBaja:
                    rango.ClaseFila = "fila_baja";
                    break;
                case ConstantesParametro.EstadoActivo:
                    rango.Editable = true;
                    break;
                case ConstantesParametro.EstadoPendiente:
                    rango.Editable = true;
                    break;
                default:
                    rango.Editable = false;
                    rango.ClaseFila = string.Empty;
                    break;
            }

            return rango;
        }

        /// <summary>
        /// Obtener Parametro HPPotenciaInductiva
        /// </summary>
        /// <param name="listaHP"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public SiParametroValorDTO GetParametroHPPotenciaInductiva(List<SiParametroValorDTO> listaHP, DateTime fechaPeriodo, int resolucion)
        {

            List<SiParametroValorDTO> listaRangoHP = listaHP.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo || x.Siparveliminado == ConstantesParametro.EstadoBaja).ToList().OrderByDescending(x => x.Siparvfechainicial).ToList();
            var lista = this.GetListaParametroRangoPotenciaInductiva(listaRangoHP, null, resolucion);

            DateTime fechaIni = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            DateTime fechaFin = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            fechaFin = fechaFin.AddMonths(1).AddDays(-1);

            //completar rango
            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Siparvfechainicial = lista[i].Fecha;
                    lista[i].Siparvfechainicial = new DateTime(lista[i].Siparvfechainicial.Year, lista[i].Siparvfechainicial.Month, 1);

                    if (i == 0)
                    {
                        lista[i].Siparvfechafinal = fechaFin;
                    }
                    else
                    {
                        lista[i].Siparvfechafinal = lista[i - 1].Siparvfechainicial.AddDays(-1);
                    }
                    lista[i].Siparvfechafinal = new DateTime(lista[i].Siparvfechafinal.Year, lista[i].Siparvfechafinal.Month, 1);
                    lista[i].Siparvfechafinal = lista[i].Siparvfechafinal.AddMonths(1).AddDays(-1);
                }
            }

            foreach (var param in lista)
            {
                SiParametroValorDTO paramHora = new SiParametroValorDTO();

                if (param.Siparvfechainicial.Date <= fechaIni && fechaFin <= param.Siparvfechafinal.Date)
                {
                    paramHora.HRango1Ini = param.HRango1Ini;
                    paramHora.HRango1Fin = param.HRango1Fin;
                    paramHora.HRango2Ini = param.HRango2Ini;
                    paramHora.HRango2Fin = param.HRango2Fin;


                    return paramHora;
                }
            }

            return null;
        }

        #endregion

        #region Magnitud de la Reserva Rotante para la RPF
        /// <summary>
        /// Lista ParametroRangoPeriodoHP
        /// </summary>
        /// <param name="listaParam"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public List<ParametroMagnitudRPF> GetListaParametroMagnitudRPF(List<SiParametroValorDTO> listaParam, List<EstadoParametro> listaEstado)
        {
            List<ParametroMagnitudRPF> lista = new List<ParametroMagnitudRPF>();

            foreach (var param in listaParam)
            {
                ParametroMagnitudRPF rango = GetParametroMagnitudRPF(param, listaEstado);
                lista.Add(rango);
            }

            return lista;
        }

        /// <summary>
        /// ParametroRangoPeriodoHP
        /// </summary>
        /// <param name="param"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public ParametroMagnitudRPF GetParametroMagnitudRPF(SiParametroValorDTO param, List<EstadoParametro> listaEstado)
        {
            ParametroMagnitudRPF rango = new ParametroMagnitudRPF();
            rango.FechaInicio = param.Siparvfechainicial.Value.Date;
            rango.FechaFin = param.Siparvfechafinal.Value.Date;
            rango.FechaFormatoInicio = rango.FechaInicio.ToString(ConstantesBase.FormatoFechaBase);
            rango.FechaFormatoFin = rango.FechaFin.ToString(ConstantesBase.FormatoFechaBase);
            rango.SiParvcodi = param.Siparvcodi;
            rango.Estado = param.Siparveliminado;
            if (listaEstado != null)
            {
                rango.EstadoValor = listaEstado.Where(x => x.EstadoCodigo == rango.Estado).FirstOrDefault().EstadoDescripcion;
            }

            rango.Magnitud = param.Siparvvalor.GetValueOrDefault(0);
            rango.MagnitudTexto = String.Format("{0:0.00}", rango.Magnitud) + "%";
            rango.Periodo = param.Siparvnota;
            rango.PeriodoDesc = ConstantesParametro.ValorPeriodoAvenida == rango.Periodo ? ConstantesParametro.DescPeriodoAvenida : ConstantesParametro.DescPeriodoEstiaje;

            rango.Siparvusucreacion = param.Siparvusucreacion;
            rango.Siparvfeccreacion = param.Siparvfeccreacion != null ? param.Siparvfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            rango.Siparvusumodificacion = param.Siparvusumodificacion;
            rango.Siparvfecmodificacion = param.Siparvfecmodificacion != null ? param.Siparvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            return rango;
        }

        /// <summary>
        /// Obtener la magnitud de RPF para el rango disponible
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal? GetMagnitudRPF(DateTime fecha)
        {
            var listaFecha = this.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroMagnitudRPF);
            listaFecha = listaFecha.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo).ToList();
            listaFecha = listaFecha.Where(x => x.Siparvfechainicial.Value.Date <= fecha.Date && fecha.Date <= x.Siparvfechafinal.Value.Date).ToList();
            SiParametroValorDTO m = listaFecha.Count() > 0 ? listaFecha.First() : null;
            return m != null ? m.Siparvvalor : null;
        }

        #endregion

        #region Tendencia de Indicador HHI

        /// <summary>
        /// Lista Tendencia de Indicador HHI (A cero, A uno)
        /// </summary>
        /// <param name="listaParam"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public List<ParametroTendenciaHHI> GetListaParametroTendenciaHHI(List<SiParametroValorDTO> listaParam, List<EstadoParametro> listaEstado)
        {
            List<ParametroTendenciaHHI> lista = new List<ParametroTendenciaHHI>();

            List<DateTime> listaFecha = listaParam.Select(x => x.Siparvfechainicial.Value.Date).Distinct().ToList();
            foreach (var fecha in listaFecha)
            {
                var listaHora = listaParam.Where(x => x.Siparvfechainicial.Value.Date == fecha).ToList();
                var paramCero = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorMonitoreoTendenciaCero).First();
                var paramUno = listaHora.Where(x => x.Siparvnota == ConstantesParametro.ValorMonitoreoTendenciaUno).First();

                ParametroTendenciaHHI rango = this.GetParametroTendenciaHHIFromLista(paramCero, paramUno, listaEstado);

                lista.Add(rango);
            }

            return lista;
        }

        /// <summary>
        /// Tendencia de Indicador HHI (A cero, A uno)
        /// </summary>
        /// <param name="paramCero"></param>
        /// <param name="paramUno"></param>
        /// <param name="listaEstado"></param>
        /// <returns></returns>
        public ParametroTendenciaHHI GetParametroTendenciaHHIFromLista(SiParametroValorDTO paramCero, SiParametroValorDTO paramUno, List<EstadoParametro> listaEstado)
        {
            ParametroTendenciaHHI rango = new ParametroTendenciaHHI();
            rango.Fecha = paramCero.Siparvfechainicial.Value.Date;
            rango.FechaFormato = rango.Fecha.ToString(ConstantesAppServicio.FormatoFecha);

            rango.SiParvcodiTendenciaCero = paramCero.Siparvcodi;
            rango.HHITendenciaCero = paramCero.Siparvvalor.GetValueOrDefault(0);

            rango.SiParvcodiTendenciaUno = paramUno.Siparvcodi;
            rango.HHITendenciaUno = paramUno.Siparvvalor.GetValueOrDefault(0);

            rango.Estado = paramCero.Siparveliminado;
            if (listaEstado != null)
            {
                rango.EstadoValor = listaEstado.Where(x => x.EstadoCodigo == rango.Estado).FirstOrDefault().EstadoDescripcion;
            }
            switch (rango.Estado)
            {
                case ConstantesParametro.EstadoAnulado:
                    rango.ClaseFila = "fila_anulado";
                    break;
                case ConstantesParametro.EstadoBaja:
                    rango.ClaseFila = "fila_baja";
                    break;
                case ConstantesParametro.EstadoActivo:
                    rango.Editable = true;
                    break;
                case ConstantesParametro.EstadoPendiente:
                    rango.Editable = true;
                    break;
                default:
                    rango.Editable = false;
                    rango.ClaseFila = string.Empty;
                    break;
            }

            rango.Siparvusucreacion = paramCero.Siparvusucreacion;
            rango.Siparvfeccreacion = paramCero.Siparvfeccreacion != null ? paramCero.Siparvfeccreacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            rango.Siparvusumodificacion = paramCero.Siparvusumodificacion;
            rango.Siparvfecmodificacion = paramCero.Siparvfecmodificacion != null ? paramCero.Siparvfecmodificacion.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            return rango;
        }

        /// <summary>
        /// Obtener Parametro HPPotenciaActiva
        /// </summary>
        /// <param name="listaHP"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        public SiParametroValorDTO GetParametroTendenciaHHI(DateTime fechaPeriodo)
        {
            List<SiParametroValorDTO> listaParam = this.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroTendenciaHHI);

            List<SiParametroValorDTO> listaRango = listaParam.Where(x => x.Siparveliminado == ConstantesParametro.EstadoActivo || x.Siparveliminado == ConstantesParametro.EstadoBaja).ToList().OrderByDescending(x => x.Siparvfechainicial).ToList();
            List<ParametroTendenciaHHI> lista = this.GetListaParametroTendenciaHHI(listaRango, null);

            DateTime fechaIni = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            DateTime fechaFin = new DateTime(fechaPeriodo.Year, fechaPeriodo.Month, 1);
            fechaFin = fechaFin.AddMonths(1).AddDays(-1);

            //completar rango
            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    lista[i].Siparvfechainicial = lista[i].Fecha;
                    lista[i].Siparvfechainicial = new DateTime(lista[i].Siparvfechainicial.Year, lista[i].Siparvfechainicial.Month, 1);

                    if (i == 0)
                    {
                        lista[i].Siparvfechafinal = fechaFin;
                    }
                    else
                    {
                        lista[i].Siparvfechafinal = lista[i - 1].Siparvfechainicial.AddDays(-1);
                    }
                    lista[i].Siparvfechafinal = new DateTime(lista[i].Siparvfechafinal.Year, lista[i].Siparvfechafinal.Month, 1);
                    lista[i].Siparvfechafinal = lista[i].Siparvfechafinal.AddMonths(1).AddDays(-1);
                }
            }

            foreach (var param in lista)
            {
                SiParametroValorDTO paramHora = new SiParametroValorDTO();

                if (param.Siparvfechainicial.Date <= fechaIni && fechaFin <= param.Siparvfechafinal.Date)
                {
                    paramHora.HHITendenciaCero = param.HHITendenciaCero;
                    paramHora.HHITendenciaUno = param.HHITendenciaUno;

                    return paramHora;
                }
            }

            return null;
        }

        #endregion
    }
}
