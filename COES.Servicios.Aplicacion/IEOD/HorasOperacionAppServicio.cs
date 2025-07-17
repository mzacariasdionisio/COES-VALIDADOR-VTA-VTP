using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Medidores;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.OperacionesVarias;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace COES.Servicios.Aplicacion.IEOD
{
    /// <summary>
    /// Lógica del aplicativo Horas de Operación
    /// </summary>
    public class HorasOperacionAppServicio : AppServicioBase
    {
        readonly IEODAppServicio servIeod = new IEODAppServicio();
        readonly OperacionesVariasAppServicio servOpVarias = new OperacionesVariasAppServicio();
        readonly CorreoAppServicio servCorreo = new CorreoAppServicio();
        readonly EventosAppServicio servEventos = new EventosAppServicio();
        readonly GeneralAppServicio servGeneral = new GeneralAppServicio();
        readonly GrupoDespachoAppServicio servGrDespach = new GrupoDespachoAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HorasOperacionAppServicio));

        #region METODOS DE LA TABLA EVE_HORAOPERACION

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_HORAOPERACION
        /// </summary>
        public EveHoraoperacionDTO GetByIdEveHoraoperacion(int hopcodi)
        {
            return FactorySic.GetEveHoraoperacionRepository().GetById(hopcodi);
        }

        /// <summary>
        /// Metodo que extrae los registros de horas de operacion para una fecha indicada
        /// </summary>
        /// <param name="sFecha"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetEveHoraoperacionCriteria(DateTime sFecha)
        {
            return FactorySic.GetEveHoraoperacionRepository().GetByCriteria(sFecha);
        }

        /// <summary>
        /// Metodo que devuelve el listado de horas de operacion para un envio determinado
        /// </summary>
        /// <param name="pkCodis"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetEveHoraoperacionCriteriaxPKCodis(string pkCodis)
        {
            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().GetCriteriaxPKCodis(pkCodis);
            foreach (var reg in lista)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
            }
            List<EveHoraoperacionDTO> listaUnidades = FactorySic.GetEveHoraoperacionRepository().GetCriteriaUnidadesxPKCodis(pkCodis);
            foreach (var reg in listaUnidades)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad;
                EveHoraoperacionDTO hopPadre = lista.Find(x => x.Hopcodi == reg.Hopcodipadre);
                reg.Hopfalla = hopPadre?.Hopfalla;
            }

            lista.AddRange(listaUnidades);
            return lista;
        }

        /// <summary>
        /// Metodo que devuelve los registros de horas de operacion para una empresa y de una fecha determinada
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="sFecha"></param>
        /// <param name="sfechaFinal"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetEveHoraoperacionCriteriaxEmpresaxFecha(int emprcodi, DateTime sFecha, DateTime sfechaFinal, int idCentral)
        {
            List<int> listaGrupocodiUnidEsp = ListarAllUnidadTermoelectricaModoEspecial();

            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().GetByCriteriaXEmpresaxFecha(emprcodi, sFecha, sfechaFinal, idCentral);
            foreach (var reg in lista)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
                reg.FlagModoEspecial = listaGrupocodiUnidEsp.Contains(reg.Grupocodi ?? -1) ? ConstantesHorasOperacion.FlagModoEspecial : null;
            }

            List<EveHoraoperacionDTO> listaUnidades = FactorySic.GetEveHoraoperacionRepository().GetByCriteriaUnidadesXEmpresaxFecha(emprcodi, sFecha, sfechaFinal, idCentral);
            foreach (var reg in listaUnidades)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad;
                EveHoraoperacionDTO hopPadre = lista.Find(x => x.Hopcodi == reg.Hopcodipadre);
                reg.Hopfalla = hopPadre?.Hopfalla;
            }

            lista.AddRange(listaUnidades);
            return lista;
        }

        /// <summary>
        /// Listar las horas de Operación Termoelectricas por empresa y/o centrales
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="centrales"></param>
        /// <param name="tipoListado"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHorasOperacionByCriteria(DateTime dfechaIni, DateTime dfechaFin, string empresas, string centrales, int tipoListado)
        {
            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacionByCriteria(dfechaIni, dfechaFin, empresas, centrales, tipoListado).ToList();

            foreach (var reg in lista)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
                reg.FlagModoEspecial = reg.Grupotipomodo == "E" ? ConstantesHorasOperacion.FlagModoEspecial : null;
            }

            //EVE_HO_UNIDAD
            List<EveHoraoperacionDTO> listaUnidades = FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacionByCriteriaUnidades(dfechaIni, dfechaFin, empresas, centrales).ToList();
            foreach (var reg in listaUnidades)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad;
                EveHoraoperacionDTO hopPadre = lista.Find(x => x.Hopcodi == reg.Hopcodipadre);
                reg.Hopfalla = hopPadre?.Hopfalla;
            }

            lista.AddRange(listaUnidades);
            return lista;
        }

        /// <summary>
        /// Listar las horas de Operación Termoelectricas por empresa y/o centrales
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="centrales"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHorasOperacionMejoras2023(DateTime dfechaIni, DateTime dfechaFin, string empresas, string centrales)
        {
            //EVE_HORAOPERACION
            List<EveHoraoperacionDTO> lista = FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacionByCriteria(dfechaIni, dfechaFin, empresas, centrales, ConstantesHorasOperacion.TipoListadoSoloTermico)
                                                .Where(x => x.Grupocodi > 0).ToList();

            foreach (var reg in lista)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
                reg.FlagModoEspecial = reg.Grupotipomodo == "E" ? ConstantesHorasOperacion.FlagModoEspecial : null;
                reg.BitacoraIdEvento = reg.Evencodi ?? 0;
            }

            //EVE_HO_UNIDAD
            List<EveHoUnidadDTO> listaUnidadesXHo = new List<EveHoUnidadDTO>();

            //pasar eve_horaoperacion a eve_hounidad
            List<EveHoraoperacionDTO> listaUnidades = FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacionByCriteriaUnidades(dfechaIni, dfechaFin, empresas, centrales).ToList();
            foreach (var reg in listaUnidades)
            {
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad;
                EveHoraoperacionDTO hopPadre = lista.Find(x => x.Hopcodi == reg.Hopcodipadre);
                reg.Hopfalla = hopPadre?.Hopfalla;

                listaUnidadesXHo.Add(new EveHoUnidadDTO()
                {
                    Hopuniactivo = 1,
                    Hopunicodi = reg.Hopcodi,
                    Emprcodi = reg.Emprcodi,
                    Hopcodi = reg.Hopcodipadre ?? 0,
                    Equicodi = reg.Equicodi ?? 0,

                    Hophorordarranq = reg.Hophorordarranq,
                    Hophorini = reg.Hophorini,
                    Hophorfin = reg.Hophorfin,
                    Hophorparada = reg.Hophorparada,

                    Hopunihorordarranq = reg.Hophorordarranq,
                    Hopunihorini = reg.Hophorini,
                    Hopunihorfin = reg.Hophorfin,
                    Hopunihorparada = reg.Hophorparada,
                    Equiabrev = reg.Equiabrev.Trim()
                });
            }

            //asignar a cada ho su detalle
            foreach (var reg in lista)
            {
                reg.ListaHoUnidad = listaUnidadesXHo.Where(x => x.Hopcodi == reg.Hopcodi).ToList();
            }

            //ordenamiento listado: Central > grupo > paralelo
            lista = lista.OrderBy(x => x.Emprnomb)
                                .ThenBy(x => x.Central)
                                .ThenBy(x => x.Gruponomb)
                                .ThenBy(x => x.HophoriniDesc).ToList();

            return lista;
        }

        /// <summary>
        /// AsignarDatosAHoraOperacion
        /// </summary>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaModosOperacion"></param>
        /// <param name="listaDesgloseRango"></param>
        /// <param name="listaHOHoyValidacion"></param>
        public void AsignarDatosAHoraOperacion(List<EveHoraoperacionDTO> listaHorasOperacion,
                                            List<PrGrupoDTO> listaModosOperacion, List<EveHoEquiporelDTO> listaDesgloseRango, List<EveHoraoperacionDTO> listaHOHoyValidacion)
        {
            //formatear celdas
            foreach (var reg in listaHorasOperacion)
            {
                reg.UnidadesExtra = new List<UnidadesExtra>();
                reg.HophoriniDesc = reg.Hophorini != null ? reg.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                reg.HophorfinDesc = reg.Hophorfin != null ? reg.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;

                reg.HophorordarranqDesc = reg.Hophorordarranq != null ? reg.Hophorordarranq.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                reg.HophorparadaDesc = reg.Hophorparada != null ? reg.Hophorparada.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                reg.HopcompordarrqDesc = reg.Hopcompordarrq == ConstantesAppServicio.SI ? ConstantesAppServicio.SI : string.Empty;
                reg.HopcompordpardDesc = reg.Hopcompordpard == ConstantesAppServicio.SI ? ConstantesAppServicio.SI : string.Empty;

                reg.HopensayopeDesc = this.GetDescripcionEnsayoPe(reg.Hopensayope);
                reg.HopensayopminDesc = this.GetDescripcionEnsayoPmin(reg.Hopensayopmin);
                reg.HopsaisladoDesc = this.GetDescripcionSistemaAislado(reg.Hopsaislado);
                reg.HoplimtransDesc = this.GetDescripcionLimTransm(reg.Hoplimtrans);
                reg.FlagCalificado = reg.Subcausacodi != ConstantesSubcausaEvento.SubcausaNoIdentificado ? ConstantesHorasOperacion.FlagCalificadoSI : ConstantesHorasOperacion.FlagCalificadoNO;

                reg.LastdateDesc = reg.Lastdate != null ? reg.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
                reg.Hopdesc = reg.Hopdesc ?? "";

                //desglose
                reg.ListaDesglose = listaDesgloseRango.Where(x => x.Hopcodi == reg.Hopcodi).ToList();

                //color y datos de ficha técnica
                var regModo = listaModosOperacion.Find(x => x.Grupocodi == reg.Grupocodi);
                if (regModo != null)
                {
                    reg.ColorTermica = regModo.ColorTermica;
                    reg.PMin = regModo.PMin;
                    reg.PEfe = regModo.PEfe;
                    reg.TMinO = regModo.TMinO;
                    reg.TMinA = regModo.TMinA;

                    reg.TArranque = reg.TArranque ?? "";
                    reg.TParada = reg.TParada ?? "";

                    //mostrar empresa para el día de consulta
                    regModo.Emprcodi = reg.Emprcodi;
                    regModo.Emprnomb = reg.Emprnomb;
                    regModo.EmprNomb = reg.Emprnomb;
                }
                else
                {
                    reg.ColorTermica = ConstantesIEOD.PropiedadColorDefault;
                    reg.PMin = "";
                    reg.PEfe = "";
                    reg.TMinO = "";
                    reg.TMinA = "";

                    reg.TArranque = "";
                    reg.TParada = "";
                }

                //alertas
                var regConAlerta = listaHOHoyValidacion.Where(x => x.Hopcodi == reg.Hopcodi).FirstOrDefault();
                reg.TieneAlertaEms = regConAlerta != null ? regConAlerta.TieneAlertaEms : 0;
                reg.TieneAlertaScada = regConAlerta != null ? regConAlerta.TieneAlertaScada : 0;
                reg.TieneAlertaIntervencion = regConAlerta != null ? regConAlerta.TieneAlertaIntervencion : 0;
                reg.TieneAlertaCostoIncremental = regConAlerta != null ? regConAlerta.TieneAlertaCostoIncremental : 0;
            }

            //ordenamiento de pestaña Listado
            listaHorasOperacion = listaHorasOperacion
                                .OrderBy(x => x.Emprnomb)
                                .ThenBy(x => x.Central)
                                .ThenBy(x => x.Gruponomb)
                                .ThenBy(x => x.HophoriniDesc).ToList();
        }

        /// <summary>
        /// SetearPuedeOffPuedeOnXModo
        /// </summary>
        /// <param name="fechaCI"></param>
        /// <param name="listaModosOperacion"></param>
        /// <param name="listaHorasOperacionHoy"></param>
        /// <param name="listaHorasOperacionAyer"></param>
        public void SetearPuedeOffPuedeOnXModo(DateTime fechaCI, List<PrGrupoDTO> listaModosOperacion,
                                        List<EveHoraoperacionDTO> listaHorasOperacionHoy, List<EveHoraoperacionDTO> listaHorasOperacionAyer)
        {
            //agrupar listar de hoy y ayer
            var listaDataHoyAyer = new List<EveHoraoperacionDTO>();
            listaDataHoyAyer.AddRange(listaHorasOperacionAyer);
            listaDataHoyAyer.AddRange(listaHorasOperacionHoy);

            //for para asignar los pmin y lo demas => listaDataModoHoy , buscamos en la jsModel.ListaModosOperacion =>find == grupocodi , asignamos el pmin y lo resto
            foreach (var hop in listaDataHoyAyer)
            {
                var modoOperacion = listaModosOperacion.Find(x => x.Grupocodi == hop.Grupocodi);
                if (modoOperacion != null)
                {
                    hop.PMin = modoOperacion.PMin;
                    hop.PEfe = modoOperacion.PEfe;
                    hop.TMinO = modoOperacion.TMinO;
                    hop.TMinA = modoOperacion.TMinA;
                }
            }

            //lista de los que cruzan con la linea verde PUEDE OFF //
            var listaDataCruzaVerde = new List<EveHoraoperacionDTO>();
            var listaDataNOCruzaVerde = new List<EveHoraoperacionDTO>();

            //ocultar puede off y puede on
            //(reg.Grupotipomodo != "CS" || reg.Equipadre == 11883 || reg.Subcausacodi == -1)
            //Grupotipomodo: "CS" => ciclo simple y EQUIPADRE = 11883 y calificacion = "no identificado" //grupocodi
            var listaDataHoyFiltrada = listaHorasOperacionHoy
                            .Where(dataModo => listaModosOperacion.Any(modoOperacion => modoOperacion.Grupocodi == dataModo.Grupocodi)
                               && dataModo.Grupotipomodo == "CS"
                               && dataModo.Equipadre != 11883
                               && dataModo.Subcausacodi > 0).ToList();

            foreach (var hop in listaDataHoyFiltrada)
            {
                //validar si pasa por la fecha
                if (hop.Hophorini <= fechaCI && fechaCI <= hop.Hophorfin) // la HoraTR esta dentroi del ultimo barra_hop
                {
                    listaDataCruzaVerde.Add(hop);
                }
                else
                {
                    if (hop.Hophorfin <= fechaCI)
                    {
                        listaDataNOCruzaVerde.Add(hop);
                    }
                }
            }

            //listaDataNOCruzaVerde => excluir aquellos grupo codi no esten en listaDataCruzaVerde
            listaDataNOCruzaVerde = listaDataNOCruzaVerde.Where(x => !listaDataCruzaVerde.Select(y => y.Grupocodi).Contains(x.Grupocodi)).ToList();

            //agrupamos por grupocodi y buscamos todos los HO ordenados por hora inicio => puede OFF
            foreach (var hopActual in listaDataCruzaVerde)
            {
                var lstData = listaDataHoyAyer
               .Where(x => x.Grupocodi == hopActual.Grupocodi && x.Hophorini <= hopActual.Hophorini)
               .OrderByDescending(x => x.Hophorini)
               .ToList();

                //por defecto el HO seleccionado sera el primero
                var InicioActualHOP = hopActual;

                if (lstData.Count > 0)
                {
                    for (int i = 0; i < lstData.Count - 1; i++)
                    {
                        var actualHOP = lstData[i];
                        var siguienteIndex = i + 1;

                        var actualHoraInicial = actualHOP.Hophorini?.ToString(ConstantesHorasOperacion.FormatoOnlyHoraFull);
                        var nextExists = siguienteIndex < lstData.Count;

                        if (nextExists)
                        {
                            var siguienteHOP = lstData[siguienteIndex];
                            var siguienteHoraFinal = siguienteHOP.Hophorfin?.ToString(ConstantesHorasOperacion.FormatoOnlyHoraFull);

                            // Comparar la hora final actual con la hora inicial del siguiente
                            if (actualHoraInicial == siguienteHoraFinal)
                            {
                                // Las horas coinciden, establecemos el HO el siguiente, continúa con el siguiente elemento
                                InicioActualHOP = siguienteHOP;
                                continue;
                            }
                            else
                            {
                                // Se encontró el elemento que rompe la cadena
                                InicioActualHOP = actualHOP;
                                break;
                            }
                        }
                    }
                }

                //asignamos el TParada(PUEDEOFF) al que rompe la continuidad o en su defecto al actual
                //si no coinciden con el dia actual(es menor) estabelcemo toda la fecha entera, dia y hora
                if (InicioActualHOP != null)
                {
                    DateTime fechaAux = InicioActualHOP.Hophorini.Value.AddHours(Convert.ToDouble(InicioActualHOP.TMinO));

                    if (fechaAux.Date < fechaCI.Date)
                    {
                        //la fecha auxiliar resulta a un dia anterior al de la consulta
                        hopActual.TParada = "00:00";
                    }
                    else
                    {
                        //la Hophorini resulta el mismo dia o superior
                        if (fechaAux.Date > fechaCI.Date)
                        {
                            hopActual.TParada = "23:58";
                        }
                        else
                        {
                            hopActual.TParada = fechaAux.ToString("HH:mm");
                        }
                    }
                }

                //actualizar modo
                var regModo = listaModosOperacion.Find(x => x.Grupocodi == hopActual.Grupocodi);
                if (regModo != null) regModo.TParada = hopActual.TParada;
            }

            //Horas de operación que no están activos y estan antes de la linea verde
            var listaHOPPuedeON = listaDataNOCruzaVerde.GroupBy(x => x.Grupocodi).Select(x => x.OrderByDescending(y => y.Hophorini).First()).ToList();

            //al PUEDEON se le suman las horas del TMinA
            foreach (var hopActual in listaHOPPuedeON) //actualizar hora de operación
            {
                DateTime fechaAux = hopActual.Hophorfin.Value.AddHours(Convert.ToDouble(hopActual.TMinA));
                hopActual.TArranque = (fechaAux.Date == hopActual.Hophorfin.Value.Date && fechaAux.Date == fechaCI.Date) ? fechaAux.ToString("HH:mm") : "23:58";

                //actualizar modo
                var regModo = listaModosOperacion.Find(x => x.Grupocodi == hopActual.Grupocodi);
                if (regModo != null) regModo.TArranque = hopActual.TArranque;
            }
        }

        /// <summary>
        /// Metodo que devuelve los registros de horas de operacion para una empresa, rango de fechas y tipo de operación
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="FechaIni"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idTipoOperacion"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetListarHorasOperacxEmpresaxFechaxTipoOPxFam(int emprcodi, DateTime FechaIni, DateTime fechaFinal, string idTipoOperacion, int famcodi)
        {
            return FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacxEmpresaxFechaxTipoOPxFam(emprcodi, FechaIni, fechaFinal, idTipoOperacion, famcodi);
        }

        /// <summary>
        /// Genera listado de las horas de operación de todos los equipos para una empresa, rango de fechas, tipo de operación y tipo de central
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="FechaIni"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idTipoOperacion"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHorasOperacxEquiposXEmpXTipoOPxFam(int emprcodi, DateTime FechaIni, DateTime fechaFinal, string idTipoOperacion, int famcodi)
        {
            return FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacxEquiposXEmpXTipoOPxFam(emprcodi, FechaIni, fechaFinal, idTipoOperacion, famcodi);
        }

        /// <summary>
        /// Lista horas de operacion por tipo de operacion
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="FechaIni"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idTipoOperacion"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHorasOperacxEquiposXEmpXTipoOPxFam2(int emprcodi, DateTime FechaIni, DateTime fechaFinal, string idTipoOperacion, int idCentral)
        {
            return FactorySic.GetEveHoraoperacionRepository().ListarHorasOperacxEquiposXEmpXTipoOPxFam2(emprcodi, FechaIni, fechaFinal, idTipoOperacion, idCentral);
        }

        /// <summary>
        /// metodo que lista todos los registros de la tabla Ev_horaoperacion para un formato, una empresa y una fecha determinada
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarEquiposxFormatoXHorasOperacion(int formatcodi, int emprcodi, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetEveHoraoperacionRepository().ListEquiposHorasOperacionxFormato(formatcodi, emprcodi, fechaIni, fechaFin);
        }

        /// <summary>
        /// Guarda horas de operación agente Extranet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GuardarHoraOP(EveHoraoperacionDTO entity)
        {
            int codigo;
            try
            {
                var hopcodiTmp = entity.Hopcodi;
                int idHoraOperacion = this.servGeneral.ObtenerNextIdTabla(ConstantesEvento.TablaHoraOperacion);
                if (entity.Hopcodipadre.GetValueOrDefault(0) == 0)
                {
                    entity.Hopcodi = idHoraOperacion;
                    FactorySic.GetEveHoraoperacionRepository().Save(entity);
                }
                else
                {
                    EveHoUnidadDTO unidad = this.ConvertirObjHoraoperacionToHoUnidad(entity, ConstantesHorasOperacion.OpcionSave);

                    unidad.Hopunicodi = idHoraOperacion;
                    FactorySic.GetEveHoUnidadRepository().Save(unidad);
                }
                this.servGeneral.ActualizarIdTabla(ConstantesEvento.TablaHoraOperacion, idHoraOperacion);

                codigo = idHoraOperacion;
                entity.Hopcodi = hopcodiTmp;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }

        /// <summary>
        /// Actualiza horas de operación Extranet
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateHorasOperacion(EveHoraoperacionDTO entity)
        {
            try
            {
                if (entity.Hopcodipadre.GetValueOrDefault(0) == 0)
                {
                    FactorySic.GetEveHoraoperacionRepository().Update(entity);
                }
                else
                {
                    EveHoUnidadDTO unidad = this.ConvertirObjHoraoperacionToHoUnidad(entity, ConstantesHorasOperacion.OpcionUpdate);
                    FactorySic.GetEveHoUnidadRepository().Update(unidad);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda horas de operación Intranet
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GuardarHoraOPAdministrador(EveHoraoperacionDTO entity)
        {
            int codigo;
            try
            {
                codigo = this.servGeneral.ObtenerNextIdTabla(ConstantesEvento.TablaHoraOperacion);
                entity.Hopcodi = codigo;
                FactorySic.GetEveHoraoperacionRepository().Save(entity);

                this.servGeneral.ActualizarIdTabla(ConstantesEvento.TablaHoraOperacion, codigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return codigo;
        }

        /// <summary>
        /// Actualiza horas de operación Intranet
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateHorasOperacionAdministrador(EveHoraoperacionDTO entity)
        {
            try
            {
                FactorySic.GetEveHoraoperacionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla EVE_HORAOPERACION_EQUIPO

        /// <summary>
        /// Inserta un registro de la tabla EVE_HORAOPERACION_EQUIPO
        /// </summary>
        public void SaveEveHoraoperacionEquipo(EveHoraoperacionEquipoDTO entity)
        {
            try
            {
                FactorySic.GetEveHoraoperacionEquipoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_HORAOPERACION_EQUIPO
        /// </summary>
        public void UpdateEveHoraoperacionEquipo(EveHoraoperacionEquipoDTO entity)
        {
            try
            {
                FactorySic.GetEveHoraoperacionEquipoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_HORAOPERACION_EQUIPO
        /// </summary>
        public void DeleteEveHoraoperacionEquipo(int hopequcodi)
        {
            try
            {
                FactorySic.GetEveHoraoperacionEquipoRepository().Delete(hopequcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_HORAOPERACION_EQUIPO
        /// </summary>
        public EveHoraoperacionEquipoDTO GetByIdEveHoraoperacionEquipo(int hopequcodi)
        {
            return FactorySic.GetEveHoraoperacionEquipoRepository().GetById(hopequcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_HORAOPERACION_EQUIPO
        /// </summary>
        public List<EveHoraoperacionEquipoDTO> ListEveHoraoperacionEquipos()
        {
            return FactorySic.GetEveHoraoperacionEquipoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveHoraoperacionEquipo
        /// </summary>
        public List<EveHoraoperacionEquipoDTO> GetByCriteriaEveHoraoperacionEquipos()
        {
            return FactorySic.GetEveHoraoperacionEquipoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla EVE_HO_UNIDAD

        /// <summary>
        /// Actualiza un registro de la tabla EVE_HO_UNIDAD
        /// </summary>
        public void UpdateEveHoUnidad(EveHoUnidadDTO entity)
        {
            try
            {
                FactorySic.GetEveHoUnidadRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_HO_UNIDAD
        /// </summary>
        public void DeleteEveHoUnidad(int hopunicodi)
        {
            try
            {
                FactorySic.GetEveHoUnidadRepository().Delete(hopunicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_HO_UNIDAD
        /// </summary>
        public EveHoUnidadDTO GetByIdEveHoUnidad(int hopunicodi)
        {
            return FactorySic.GetEveHoUnidadRepository().GetById(hopunicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_HO_UNIDAD
        /// </summary>
        public List<EveHoUnidadDTO> ListEveHoUnidads()
        {
            return FactorySic.GetEveHoUnidadRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveHoUnidad
        /// </summary>
        public List<EveHoUnidadDTO> GetByCriteriaEveHoUnidads()
        {
            return FactorySic.GetEveHoUnidadRepository().GetByCriteria();
        }

        #endregion

        #region METODOS DE LA TABLA EQ_FAMILIA

        /// <summary>
        /// Listar todos los tipos de centrales de Horas de Operación
        /// </summary>
        /// <returns></returns>
        public List<EqFamiliaDTO> ListarTipoCentralHOP()
        {
            return this.servIeod.ListarFamilia().Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica
                || x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica
                || x.Famcodi == ConstantesHorasOperacion.IdTipoSolar
                || x.Famcodi == ConstantesHorasOperacion.IdTipoEolica).ToList();
        }

        #endregion

        #region METODOS DE LA TABLA EVE_SUBCAUSAEVENTO

        /// <summary>
        /// Obtiene sub causa para Horas de Operación
        /// </summary>
        /// <returns></returns>
        public List<EveSubcausaeventoDTO> ListarTipoOperacionHO()
        {
            List<EveSubcausaeventoDTO> lista = FactorySic.GetEveSubcausaeventoRepository().ListarTipoOperacionHO();

            int[] codigosNoValidos = new int[] { 104, 113, 118, 316, 117 };

            /*
            104: POR NECESIDAD DE RPF
            113: POR REQUERIMIENTO PROPIO SUSTENTADO
            118: OPERACION FORZADA CHR
            316: ERROR DE MANIOBRA U OPERACIÓN
            117: POR TENSION SS
            */

            lista = lista.Where(p => !codigosNoValidos.Contains(p.Subcausacodi)).ToList();

            foreach (var c in lista)
            {
                c.Subcausadesc = c.Subcausadesc.Trim();
                c.Subcausacolor = ConstantesSubcausaEvento.SubcausaDefaultColor;
                switch (c.Subcausacodi)
                {
                    case ConstantesSubcausaEvento.SubcausaNoIdentificado:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaNoIdentificadoColor;
                        c.Orden = 0;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorPotenciaEnergiaColor;
                        c.Orden = 1;
                        break;
                    case ConstantesSubcausaEvento.SubcausaAMinimaCarga:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaAMinimaCargaColor;
                        c.Orden = 2;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorPruebas:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorPruebasColor;
                        c.Orden = 3;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorRsf:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorNecesidadRPFColor;
                        c.Orden = 4;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorPruebasAleatoriasPR25:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorPruebasAleatoriasColor;
                        c.Orden = 5;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorSeguridad:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorSeguridadColor;
                        c.Orden = 6;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorCongeneracion:
                        c.Orden = 7;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorTension:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorTensionColor;
                        c.Orden = 8;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorManiobras:
                        c.Orden = 9;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorReservaEspecial:
                        c.Orden = 10;
                        break;
                    case ConstantesSubcausaEvento.SubcausaPorRestricOpTemporal:
                        c.Subcausacolor = ConstantesSubcausaEvento.SubcausaPorRestricOpTemporalColor;
                        c.Orden = 11;
                        break;
                }
            }

            return lista.OrderBy(p => p.Orden).ToList();
        }

        #endregion

        #region METODOS DE LA TABLA PR_GRUPO

        /// <summary>
        /// Listar Todos los grupos de generacion GR.CATECODI in (3,5) and grupoactivo = 'S', y si es Integrante o no segun la fecha
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllGrupoGeneracion(DateTime fechaConsulta, string estado)
        {
            return FactorySic.GetPrGrupoRepository().ListarAllGrupoGeneracion(fechaConsulta, estado, ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// Listar todas las unidades indicando el grupocodi de su modo principal y si es una unidad especial
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllUnidadTermoelectrica()
        {
            return FactorySic.GetPrGrupoRepository().ListarAllUnidadTermoelectrica();
        }

        /// <summary>
        /// Listar todas las unidades indicando el grupocodi de su modo principal y si es una unidad especial y modo Especial S
        /// </summary>
        /// <returns></returns>
        public List<int> ListarAllUnidadTermoelectricaModoEspecial()
        {
            return ListarModoOperacionXCentralYEmpresa(-2, -2).Where(x => x.FlagModoEspecial == "S").Select(x => x.Grupocodi).ToList();
        }

        /// <summary>
        /// Lista de Modos de operacion por Central y Empresa
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarModoOperacionXCentralYEmpresa(int idCentral, int idEmpresa)
        {
            //lista modos Activos, En proyecto, fuera de COES
            var lista = FactorySic.GetPrGrupoRepository().ListaModosOperacion().
                                Where(x => x.GrupoEstado != ConstantesAppServicio.Anulado && x.GrupoEstado != ConstantesAppServicio.Baja).ToList();

            //relacion equipo y sus modos
            var listaRelEqModo = FactorySic.GetPrGrupoeqRepository().GetByCriteria(-1, -1).Where(x => x.Geqactivo == 1).ToList();

            //datos adicionales al modo
            foreach (var reg in lista)
            {
                var listaEqXmodo = listaRelEqModo.Where(x => x.Grupocodi == reg.Grupocodi).ToList();

                if (listaEqXmodo.Any())
                {
                    reg.EmprNomb = listaEqXmodo[0].Emprnomb;
                    //reg.Emprnomb = listaEqXmodo[0].Emprnomb;
                    reg.Central = listaEqXmodo[0].Central;
                    reg.Equipadre = listaEqXmodo[0].Equipadre;
                    reg.ListaEquicodi = listaEqXmodo.Select(x => x.Equicodi).ToList();
                    reg.ListaEquiabrev = listaEqXmodo.Select(x => x.Equiabrev).ToList();
                }
                reg.FlagModoEspecial = reg.Grupotipomodo == ConstantesHorasOperacion.TipoModoOpEspecial ? "S" : "N";
                reg.Gruponomb = reg.Gruponomb != null ? reg.Gruponomb.Trim() : string.Empty;
                reg.Grupoabrev = reg.Grupoabrev != null ? reg.Grupoabrev.Trim() : string.Empty;
            }

            if (idCentral > 0) //-2 es todos
            {
                lista = lista.Where(x => x.Equipadre == idCentral).ToList();
            }

            if (idEmpresa > 0) //-2 es todos
            {
                lista = lista.Where(x => x.Emprcodi == idEmpresa).ToList();
            }

            lista = lista.OrderBy(x => x.EmprNomb).ThenBy(x => x.Gruponomb).ToList();

            return lista;
        }

        /// <summary>
        /// Método que devuelve una lista de unidades por modo de operacion para una determinada central termoeléctrica
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarUnidadesWithModoOperacionXCentralYEmpresa(int idCentral, string idEmpresa)
        {
            var lista = FactorySic.GetPrGrupoRepository().ListarUnidadesWithModoOperacionXCentralYEmpresa(idCentral, idEmpresa)
                .Where(x => x.Equipadre > 0).ToList().OrderBy(x => x.Gruponomb).ThenBy(x => x.Equinomb).ToList();

            //Caso especial de CT Caña Brava
            //CANA BRAVA TV1 - BAGAZO, el método anterior agrega el G2 incorrectamente
            lista = lista.Where(x => !(x.Grupocodi == 573 && x.Equicodi == 20523)).ToList();

            //CANA BRAVA TV2 - BAGAZO, el método anterior agrega el G1 incorrectamente
            lista = lista.Where(x => !(x.Grupocodi == 574 && x.Equicodi == 20524)).ToList();

            return lista;
        }

        #endregion

        #region Métodos Tabla EQ_EQUIPO

        /// <summary>
        /// Lista grupos por central de generacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="idGenerador"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarGruposxCentralGEN(int idEmpresa, int idCentral, int idGenerador)
        {
            List<EqEquipoDTO> lista = new List<EqEquipoDTO>();
            if (idGenerador == ConstantesHorasOperacion.IdGeneradorTemoelectrico)
            {
                lista = FactorySic.GetEqEquipoRepository().GetByEmprFamCentral(idEmpresa, ConstantesHorasOperacion.IdGeneradorTemoelectrico, idCentral);
            }
            if (idGenerador == ConstantesHorasOperacion.IdGeneradorHidroelectrico)
            {
                lista = FactorySic.GetEqEquipoRepository().GetByEmprFamCentral(idEmpresa, ConstantesHorasOperacion.IdGeneradorHidroelectrico, idCentral);
            }

            return lista;
        }

        /// <summary>
        /// Lista de grupos de generacion por central de generación
        /// </summary>
        /// <param name="grupoCodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarGruposxCentralGENEspecial(int grupoCodi, int famcodi)
        {
            List<EqEquipoDTO> listaUnidades = FactorySic.GetEqEquipoRepository().GetByEmprFam2(grupoCodi, famcodi);

            if (grupoCodi != 355) //Cuando el modo de operación es CHILINA TG - D2, mostrará sus unidades activas y en baja, para los demás equipos activos y en proyecto
            {
                listaUnidades = listaUnidades.Where(x => x.Equiestado == ConstantesAppServicio.Activo || x.Equiestado == ConstantesAppServicio.Proyecto).ToList();
            }

            return listaUnidades;
        }

        /// <summary>
        /// Lista de centrales filtrados por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener(int idEmpresa)
        {
            return FactorySic.GetEqEquipoRepository().CentralesXEmpresaHorasOperacion(idEmpresa);
        }

        /// <summary>
        /// ListarCentralesXEmpresaGener
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesXEmpresaGener(int idEmpresa, string fecha)
        {
            return FactorySic.GetEqEquipoRepository().CentralesXEmpresaHorasOperacion(idEmpresa, fecha);
        }

        #endregion

        #region METODOS DE LA TABLA EVE_HO_EQUIPOREL

        /// <summary>
        /// Listar EveHoEquiporel por hora de operación
        /// </summary>
        /// <param name="hopcodi"></param>
        /// <returns></returns>
        public List<EveHoEquiporelDTO> ListEveHoEquiporelByHopcodi(int hopcodi)
        {
            var listaTipoDesg = HorasOperacionAppServicio.ListarTipoDesglose();

            var lista = FactorySic.GetEveHoEquiporelRepository().ListaByHopcodi(hopcodi);

            foreach (var reg in lista)
            {
                reg.TipoDesglose = listaTipoDesg.Find(x => x.Subcausacodi == reg.Subcausacodi).TipoDesglose;
            }

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveHoEquiporel
        /// </summary>
        public List<EveHoEquiporelDTO> GetByCriteriaEveHoEquiporel(DateTime fechaIni, DateTime fechaFin)
        {
            var listaTipoDesg = HorasOperacionAppServicio.ListarTipoDesglose();

            var lista = FactorySic.GetEveHoEquiporelRepository().GetByCriteria(fechaIni, fechaFin);

            foreach (var reg in lista)
            {
                reg.TipoDesglose = listaTipoDesg.Find(x => x.Subcausacodi == reg.Subcausacodi).TipoDesglose;
                reg.IchoriniDesc = reg.Ichorini.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.IchorfinDesc = reg.Ichorfin.ToString(ConstantesAppServicio.FormatoFechaFull2);
            }

            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EveHoEquiporel
        /// </summary>
        public List<EveHoEquiporelDTO> GetByCriteriaEveHoEquiporelGroupByHoPadre(DateTime fechaIni, DateTime fechaFin)
        {
            var listaTipoDesg = HorasOperacionAppServicio.ListarTipoDesglose();

            var lista = FactorySic.GetEveHoEquiporelRepository().GetByCriteria(fechaIni, fechaFin);

            foreach (var reg in lista)
            {
                reg.TipoDesglose = listaTipoDesg.Find(x => x.Subcausacodi == reg.Subcausacodi).TipoDesglose;
                reg.IchoriniDesc = reg.Ichorini.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.IchorfinDesc = reg.Ichorfin.ToString(ConstantesAppServicio.FormatoFechaFull2);
                reg.FechaIni = reg.Ichorini.ToString(ConstantesAppServicio.FormatoFecha);
                reg.HoraIni = reg.Ichorini.ToString(ConstantesAppServicio.FormatoHHmmss);
                reg.FechaFin = reg.Ichorfin.ToString(ConstantesAppServicio.FormatoFecha);
                reg.HoraFin = reg.Ichorfin.ToString(ConstantesAppServicio.FormatoHHmmss);
            }

            //Se usa group by porque puede haber duplicados que se diferencian por el campo VALOR (si se usa en simultaneo el aplicativo Horas de Operación y Operaciones Varias, Restricciones Operativas)
            //TODO cambiar lógica para Modos de operación especiales
            lista = lista.GroupBy(x => new
            {
                x.Hopcodi,
                x.Ichorini,
                x.IchoriniDesc,
                x.Ichorfin,
                x.IchorfinDesc,
                x.Icvalor1,
                x.Subcausacodi,
                x.Subcausadesc,
                x.TipoDesglose
            })
                .Select(x => new EveHoEquiporelDTO()
                {
                    Hopcodi = x.Key.Hopcodi,
                    Ichorini = x.Key.Ichorini,
                    IchoriniDesc = x.Key.IchoriniDesc,
                    Ichorfin = x.Key.Ichorfin,
                    IchorfinDesc = x.Key.IchorfinDesc,
                    Icvalor1 = x.Key.Icvalor1,
                    Subcausacodi = x.Key.Subcausacodi,
                    Subcausadesc = x.Key.Subcausadesc,
                    TipoDesglose = x.Key.TipoDesglose,
                    FechaIni = x.First().FechaIni,
                    HoraIni = x.First().HoraIni,
                    FechaFin = x.First().FechaFin,
                    HoraFin = x.First().HoraFin,
                })
                .ToList();

            return lista;
        }

        #endregion

        #region Plazo, Ampliación, Envio

        /// <summary>
        /// Listar las fuentes de datos por familia
        /// </summary>
        /// <returns></returns>
        public List<FuenteDatosXFamilia> ListarAllFuenteDatosXFamilia()
        {
            List<FuenteDatosXFamilia> l = new List<FuenteDatosXFamilia>
            {
                new FuenteDatosXFamilia() { Fdatcodi = ConstantesIEOD.FdatcodiHOPTermoelectrica, Famcodi = ConstantesHorasOperacion.IdTipoTermica },
                new FuenteDatosXFamilia() { Fdatcodi = ConstantesIEOD.FdatcodiHOPHidroelectrica, Famcodi = ConstantesHorasOperacion.IdTipoHidraulica },
                new FuenteDatosXFamilia() { Fdatcodi = ConstantesIEOD.FdatcodiHOPSolar, Famcodi = ConstantesHorasOperacion.IdTipoSolar },
                new FuenteDatosXFamilia() { Fdatcodi = ConstantesIEOD.FdatcodiHOPEolica, Famcodi = ConstantesHorasOperacion.IdTipoEolica },
                new FuenteDatosXFamilia() { Fdatcodi = ConstantesIEOD.FdatcodiHOPTermoelectricaBiogasBagazo, Famcodi = ConstantesHorasOperacion.IdTipoTermica }
            };

            return l;
        }

        /// <summary>
        /// Obtener fdatcodi segun Famcodi y fuente de energia
        /// </summary>
        /// <param name="famcodi"></param>
        /// <param name="listaFenergcodi"></param>
        /// <returns></returns>
        public int GetFdatcodiByFamcodi(int famcodi, List<int> listaFenergcodi)
        {
            if (ConstantesHorasOperacion.IdTipoTermica == famcodi
                && (listaFenergcodi.Contains(ConstantesPR5ReportesServicio.FenergcodiBiogas) || listaFenergcodi.Contains(ConstantesPR5ReportesServicio.FenergcodiBagazo)))
                return ConstantesIEOD.FdatcodiHOPTermoelectricaBiogasBagazo;

            return this.ListarAllFuenteDatosXFamilia().Find(x => x.Famcodi == famcodi).Fdatcodi;
        }

        /// <summary>
        /// Obtener las empresas segun las fuente de energia
        /// </summary>
        /// <param name="fenergcodi"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresaXFenergcodi(string fenergcodi)
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasxCombustible(fenergcodi);
        }

        #endregion

        #region Registro / Edición de Horas de Operación

        /// <summary>
        /// Listar empresas de Horas de operacion (Empresas de GENERACION)
        /// </summary>
        /// <param name="accesoEmpresa"></param>
        /// <param name="usuarioEmpresas"></param>
        /// <param name="tipoCentrales"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListarEmpresasHorasOperacionByTipoCentral(bool accesoEmpresa, List<SiEmpresaDTO> usuarioEmpresas, string tipoCentrales)
        {
            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();
            List<SiEmpresaDTO> empresas = this.servIeod.ListarEmpresasxTipoEquipos(tipoCentrales);
            if (accesoEmpresa)
            {
                listaEmpresas = empresas;
            }
            else
            {
                var emprUsuario = usuarioEmpresas.Where(x => empresas.Any(y => x.Emprcodi == y.Emprcodi)).
                    Select(x => new SiEmpresaDTO()
                    {
                        Emprcodi = x.Emprcodi,
                        Emprnomb = x.Emprnomb
                    });
                if (emprUsuario.Count() > 0)
                {
                    listaEmpresas = emprUsuario.ToList();
                }
            }

            return listaEmpresas.OrderBy(x => x.Emprnomb).ToList();
        }

        /// <summary>
        /// Aplica para las HO antigüas, si tienen un modo se crean sus hop hijas con unidades
        /// </summary>
        /// <param name="ho"></param>
        /// <param name="listaUnidadXModo"></param>
        /// <param name="tipoHO"></param>
        /// <returns></returns>
        private List<EveHoraoperacionDTO> GenerarListaTemporalReporteHoByModoDesktop(EveHoraoperacionDTO ho, List<PrGrupoDTO> listaUnidadXModo, int tipoHO)
        {
            List<EveHoraoperacionDTO> lista = new List<EveHoraoperacionDTO>();
            ho.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
            lista.Add(ho);

            List<PrGrupoDTO> listaUnidad = new List<PrGrupoDTO>();
            switch (tipoHO)
            {
                case ConstantesHorasOperacion.TipoHONormal:
                    listaUnidad = listaUnidadXModo.Where(x => x.Grupocodi == ho.Grupocodi).ToList();
                    break;
                case ConstantesHorasOperacion.TipoHOUnidadEspecial:

                    if (ho.Equicodi == null)
                    {
                        listaUnidad = listaUnidadXModo.Where(x => x.Grupocodi == ho.Grupocodi).ToList();
                    }
                    else
                    {
                        listaUnidad = listaUnidadXModo.Where(x => x.Grupocodi == ho.Grupocodi && (x.Equicodi == ho.Equicodi || x.Equipadre == ho.Equicodi)).ToList();
                    }

                    break;
            }

            //Genera objetos para validaciones. Estos no se usan en el registro / edición de horas de operación
            foreach (var unid in listaUnidad)
            {
                EveHoraoperacionDTO reg = new EveHoraoperacionDTO
                {
                    Hopcodipadre = ho.Hopcodi,
                    Equicodi = unid.Equicodi,
                    Hophorini = ho.Hophorini,
                    Hophorfin = ho.Hophorfin,
                    Hophorordarranq = ho.Hophorordarranq,
                    Hophorparada = ho.Hophorparada,
                    Subcausacodi = ho.Subcausacodi,
                    Hopdesc = ho.Hopdesc,
                    Hopfalla = ho.Hopfalla,
                    Hopcompordarrq = ho.Hopcompordarrq,
                    Hopcompordpard = ho.Hopcompordpard,
                    Hopsaislado = ho.Hopsaislado,
                    FlagTipoTemporal = ConstantesHorasOperacion.TipoTemporal,
                    FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad,
                    Fenergcodi = ho.Fenergcodi,
                    Fenergnomb = ho.Fenergnomb,
                    Fenercolor = ho.Fenercolor,
                    Emprcodi = ho.Emprcodi,
                    Grupocodi = unid.Grupocodi,
                    Grupopadre = unid.Grupopadre ?? 0,
                    Lastdate = ho.Lastdate,
                    Lastuser = ho.Lastuser
                };

                lista.Add(reg);
            }

            return lista;
        }

        /// <summary>
        /// completar HO del aplicativo web
        /// </summary>
        /// <param name="listaHoWeb"></param>
        /// <param name="listaModoYeq"></param>
        /// <returns></returns>
        private List<EveHoraoperacionDTO> GenerarListaTemporalFaltanteReporteHoByModoWeb(List<EveHoraoperacionDTO> listaHoWeb, List<PrGrupoDTO> listaModoYeq)
        {
            List<EveHoraoperacionDTO> listaFaltante = new List<EveHoraoperacionDTO>();

            //Genera objetos para validaciones. Estos no se usan en el registro / edición de horas de operación
            foreach (var ho in listaHoWeb.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList())
            {
                var regModo = listaModoYeq.Find(x => x.Grupocodi == ho.Grupocodi);
                var listaHoUnidad = listaHoWeb.Where(x => x.Hopcodipadre == ho.Hopcodi).ToList();

                //Solo completar para modos de operación no especiales
                if (regModo != null && regModo.FlagModoEspecial != "S")
                {
                    for (var i = 0; i < regModo.ListaEquicodi.Count(); i++)
                    {
                        var equicodi = regModo.ListaEquicodi[i];
                        var equiabrev = regModo.ListaEquiabrev[i];

                        var regHoUnidad = listaHoUnidad.Find(x => x.Equicodi == equicodi);
                        if (regHoUnidad == null)
                        {
                            EveHoraoperacionDTO reg = new EveHoraoperacionDTO
                            {
                                Hopcodipadre = ho.Hopcodi,
                                Equicodi = equicodi,
                                Equiabrev = equiabrev,
                                Hophorini = ho.Hophorini,
                                Hophorfin = ho.Hophorfin,
                                Hophorordarranq = ho.Hophorordarranq,
                                Hophorparada = ho.Hophorparada,
                                Subcausacodi = ho.Subcausacodi,
                                Hopdesc = ho.Hopdesc,
                                Hopfalla = ho.Hopfalla,
                                Hopcompordarrq = ho.Hopcompordarrq,
                                Hopcompordpard = ho.Hopcompordpard,
                                Hopsaislado = ho.Hopsaislado,
                                FlagTipoTemporal = ConstantesHorasOperacion.TipoTemporal,
                                FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad,
                                Fenergcodi = ho.Fenergcodi,
                                Fenergnomb = ho.Fenergnomb,
                                Fenercolor = ho.Fenercolor,
                                Emprcodi = ho.Emprcodi,
                                //reg.Grupocodi = unid.Grupocodi;
                                //reg.Grupopadre = unid.Grupopadre ?? 0;
                                Lastdate = ho.Lastdate,
                                Lastuser = ho.Lastuser
                            };

                            listaFaltante.Add(reg);
                        }
                    }
                }
            }

            return listaFaltante;
        }

        private bool ExisteHoraOperacionSinUnidad(List<EveHoraoperacionDTO> listaHoWeb)
        {
            //Genera objetos para validaciones. Estos no se usan en el registro / edición de horas de operación
            foreach (var ho in listaHoWeb.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList())
            {
                var listaHoUnidad = listaHoWeb.Where(x => x.Hopcodipadre == ho.Hopcodi).ToList();

                //Solo completar para modos de operación no especiales
                if (!listaHoUnidad.Any())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Convertir objeto EveHoraoperacionDTO a EveHoUnidadDTO
        /// </summary>
        /// <param name="hop"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        private EveHoUnidadDTO ConvertirObjHoraoperacionToHoUnidad(EveHoraoperacionDTO hop, int accion)
        {
            if (hop.Hopcodipadre.GetValueOrDefault(0) == 0) { return null; }

            EveHoUnidadDTO entity = new EveHoUnidadDTO
            {
                Hopcodi = hop.Hopcodipadre.Value,
                Equicodi = hop.Equicodi.Value,
                Emprcodi = hop.Emprcodi,
                Hopuniactivo = hop.Hopestado == ConstantesAppServicio.Activo ? ConstantesHorasOperacion.HOUnidadActivo : ConstantesHorasOperacion.HOUnidadInactivo,
                Hopunihorordarranq = hop.Hophorordarranq,
                Hopunihorini = hop.Hophorini,
                Hopunihorparada = hop.Hophorparada,
                Hopunihorfin = hop.Hophorfin,
                Hopuniusucreacion = hop.Lastuser,
                Hopunifeccreacion = hop.Lastdate
            };

            if (ConstantesHorasOperacion.OpcionUpdate == accion)
            {
                entity.Hopunicodi = hop.Hopcodi;
                entity.Emprcodi = hop.Emprcodi;
                entity.Hopunifecmodificacion = hop.Lastdate;
                entity.Hopuniusumodificacion = hop.Lastuser;
            }

            return entity;
        }

        /// <summary>
        /// Obtener equicodi que debe tener la Hora de Operación
        /// </summary>
        /// <param name="grupocodiModo"></param>
        /// <param name="listaHoHijo"></param>
        /// <param name="listaAllModosOperacion"></param>
        /// <returns></returns>
        private int? GetEquicodiHoraOperacionAgente(int grupocodiModo, List<EveHoraoperacionDTO> listaHoHijo, List<PrGrupoDTO> listaAllModosOperacion)
        {
            int? equicodi = null;

            PrGrupoDTO regModo = listaAllModosOperacion.Find(x => x.Grupocodi == grupocodiModo);
            if (regModo != null)
            {
                int equicodiCentral = regModo.Equipadre;

                //Caso Modos de operación Simples y Combinados
                if (regModo.FlagModoEspecial != ConstantesHorasOperacion.FlagModoEspecial)
                {
                    List<int> listaUnidadXModoOP = regModo.ListaEquicodi;

                    if (listaUnidadXModoOP.Count > 0)
                    {
                        List<int> listaUnidadXCentral = new List<int>();
                        foreach (var regModoTmp in listaAllModosOperacion.Where(x => x.Equipadre == equicodiCentral).ToList())
                        {
                            listaUnidadXCentral.AddRange(regModoTmp.ListaEquicodi);
                        }
                        listaUnidadXCentral = listaUnidadXCentral.Distinct().ToList();

                        if (listaUnidadXCentral.Count > 1)
                        {
                            if (listaUnidadXModoOP.Count == 1)
                            {
                                return listaUnidadXModoOP.First();
                            }
                            else
                            {
                                return equicodiCentral;
                            }
                        }
                        else
                        {
                            return equicodiCentral;
                        }
                    }
                }
                else
                {
                    //Modo de operación con unidades especiales
                    int totalUnidadesUsadas = 0;
                    List<int> listaUnidadXModoOP = regModo.ListaEquicodi;
                    foreach (var unidad in listaUnidadXModoOP)
                    {
                        totalUnidadesUsadas += (listaHoHijo.Where(x => x.Equicodi == unidad).Count() > 0 ? 1 : 0);
                    }

                    if (totalUnidadesUsadas > 0)
                    {
                        if (totalUnidadesUsadas == 1)
                        {
                            return listaHoHijo.First().Equicodi;
                        }
                        else
                        {
                            return equicodiCentral;
                        }
                    }
                }
            }

            return equicodi;
        }

        /// <summary>
        /// Obtener equicodi que debe tener la Hora de Operación
        /// </summary>
        /// <param name="grupocodiModo"></param>
        /// <param name="listaHoHijo"></param>
        /// <param name="listaAllModosOperacion"></param>
        /// <param name="equicodiRepresentativo"></param>
        /// <param name="nombreCentral"></param>
        private void GetEquicodiHoraOperacionAdministrador(int grupocodiModo, List<EveHoUnidadDTO> listaHoHijo, List<PrGrupoDTO> listaAllModosOperacion, out int? equicodiRepresentativo, out string nombreCentral)
        {
            equicodiRepresentativo = null;
            nombreCentral = "";

            PrGrupoDTO regModo = listaAllModosOperacion.Find(x => x.Grupocodi == grupocodiModo);
            if (regModo != null)
            {
                int equicodiCentral = regModo.Equipadre;
                equicodiRepresentativo = equicodiCentral;
                nombreCentral = regModo.Central;

                //Caso Modos de operación de ciclo Simple y Combinado
                if (regModo.FlagModoEspecial != ConstantesHorasOperacion.FlagModoEspecial)
                {
                    List<int> listaUnidadXModoOP = regModo.ListaEquicodi;

                    if (listaUnidadXModoOP.Count > 0)
                    {
                        List<int> listaUnidadXCentral = new List<int>();
                        foreach (var regModoTmp in listaAllModosOperacion.Where(x => x.Equipadre == equicodiCentral).ToList())
                        {
                            listaUnidadXCentral.AddRange(regModoTmp.ListaEquicodi);
                        }
                        listaUnidadXCentral = listaUnidadXCentral.Distinct().ToList();

                        //si la central tiene varios equipos
                        if (listaUnidadXCentral.Count > 1)
                        {
                            if (listaUnidadXModoOP.Count == 1)
                            {
                                equicodiRepresentativo = listaUnidadXModoOP.First();
                            }
                            else
                            {
                                equicodiRepresentativo = equicodiCentral;
                            }
                        }
                        else
                        {
                            equicodiRepresentativo = equicodiCentral;
                        }
                    }
                }
                else
                {
                    //Modo de operación con unidades especiales
                    int totalUnidadesUsadas = 0;
                    List<int> listaUnidadXModoOP = regModo.ListaEquicodi;
                    foreach (var unidad in listaUnidadXModoOP)
                    {
                        totalUnidadesUsadas += (listaHoHijo.Where(x => x.Equicodi == unidad).Count() > 0 ? 1 : 0);
                    }

                    if (totalUnidadesUsadas > 0)
                    {
                        if (totalUnidadesUsadas == 1)
                        {
                            equicodiRepresentativo = listaHoHijo.First().Equicodi;
                        }
                        else
                        {
                            equicodiRepresentativo = equicodiCentral;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Guarda registro de horas de operación y el detalle de envio
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="listaHorasOperacionBD"></param>
        /// <param name="listCodHop"></param>
        /// <param name="listCodHopElim"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaModificacion"></param>
        public void GuardarHorasdeOperacionAdministrador(List<EveHoraoperacionDTO> lista, int idEmpresa, 
                List<EveHoraoperacionDTO> listaHorasOperacionBD, ref List<int> listCodHop, ref List<int> listCodHopElim, string usuario, DateTime fechaModificacion)
        {
            lista = lista.OrderByDescending(x => x.Hopcodi).ToList();

            int idEmpresaFiltro = idEmpresa > 0 ? idEmpresa : -2;
            int codigo = 0;

            List<PrGrupoDTO> listaModosOperacion = this.ListarModoOperacionXCentralYEmpresa(-2, idEmpresaFiltro);

            //Guardar por hora de operación padre
            foreach (var entity in lista)
            {
                GetEquicodiHoraOperacionAdministrador(entity.Grupocodi ?? 0, entity.ListaHoUnidad.Where(x => x.OpcionCrud != -1).ToList(), listaModosOperacion, 
                                                    out int? equicodiRepresentativo, out string nombreCentral);

                entity.Equicodi = equicodiRepresentativo;
                entity.PadreNombre = nombreCentral;
                entity.Lastdate = fechaModificacion;
                entity.Lastuser = usuario;
                entity.Hopestado = ConstantesAppServicio.Activo;
                entity.Hoparrqblackstart = entity.Hoparrqblackstart ?? ConstantesAppServicio.NO;
                entity.Hopensayope = ConstantesSubcausaEvento.SubcausaPorPruebas == entity.Subcausacodi && entity.Hopensayope != null ? entity.Hopensayope : ConstantesAppServicio.NO;
                entity.Hopensayopmin = ConstantesSubcausaEvento.SubcausaPorPruebas == entity.Subcausacodi && entity.Hopensayopmin != null ? entity.Hopensayopmin : ConstantesAppServicio.NO;
                entity.HopPruebaExitosa = entity.HopPruebaExitosa == null ? 0 : entity.HopPruebaExitosa;

                //1. CRUD sobre la hora de operación (modo de operación). Guardar el padre (EVE_HORAOPERACION)
                if (entity.OpcionCrud == -1) // Eliminado lógico
                {
                    entity.Hopestado = ConstantesAppServicio.Baja;
                    this.UpdateHorasOperacion(entity);
                    if (entity.ListaHoUnidad != null)
                    {
                        foreach (var itemHoUnidad in entity.ListaHoUnidad)
                        {
                            this.CreateOrUpdateRestriccionOperativa(entity, entity.Hopcodi, itemHoUnidad.Equicodi, usuario, fechaModificacion, true);
                        }
                    }

                    listCodHop.Add(entity.Hopcodi); //para notificación
                    listCodHopElim.Add(entity.Hopcodi); //para notificación
                }

                if (entity.OpcionCrud == 2) // Actualizacion
                {
                    entity.Hopnotifuniesp = ConstantesHorasOperacion.FlagUnidadEspecialAdminCreacion;
                    var regEnBD = listaHorasOperacionBD.Find(x => x.Hopcodi == entity.Hopcodi);
                    if (regEnBD != null)
                    {
                        entity.Hopnotifuniesp = regEnBD.Hopnotifuniesp >= ConstantesHorasOperacion.FlagUnidadEspecialAdminModificacionFromAgente
                            ? ConstantesHorasOperacion.FlagUnidadEspecialAdminModificacionFromAgente : ConstantesHorasOperacion.FlagUnidadEspecialAdminModificacionPropio;
                    }

                    if (entity.Hopfalla == ConstantesHorasOperacion.HopFalla)
                    {
                        //solo se guardar bitacora modificadas por el usuario
                        if (entity.BitacoraHophoriniFecha != null && entity.BitacoraHophoriniFecha != string.Empty && entity.BitacoraHophoriniHora != null && entity.BitacoraHophoriniHora != string.Empty
                            && entity.BitacoraHophorfinFecha != null && entity.BitacoraHophorfinFecha != string.Empty && entity.BitacoraHophorfinHora != null && entity.BitacoraHophorfinHora != string.Empty)
                        {
                            entity.BitacoraHophorini = DateTime.ParseExact(entity.BitacoraHophoriniFecha + " " + entity.BitacoraHophoriniHora, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                            entity.BitacoraHophorfin = DateTime.ParseExact(entity.BitacoraHophorfinFecha + " " + entity.BitacoraHophorfinHora, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);

                            entity.Evencodi = this.GrabarBitacora(entity.BitacoraHophorini.Value, entity.BitacoraHophorfin.Value, entity.BitacoraIdTipoEvento, entity.BitacoraDescripcion, entity.BitacoraDetalle, entity.BitacoraComentario
                               , entity.BitacoraIdSubCausaEvento, entity.BitacoraIdEvento, entity.BitacoraIdEquipo, entity.BitacoraIdEmpresa, entity.BitacoraIdTipoOperacion, usuario);
                        }
                    }

                    this.UpdateHorasOperacion(entity);

                    listCodHop.Add(entity.Hopcodi);
                }

                if (entity.OpcionCrud == 0) // sin cambios
                {
                    //listCodHop.Add(entity.Hopcodi);
                }

                if (entity.OpcionCrud == 1)  // Nuevo
                {
                    entity.Hopnotifuniesp = ConstantesHorasOperacion.FlagUnidadEspecialAdminCreacion;
                    // si se trata de un modo de operacion termoelectrico, entonces registramos su nuevo Hopcodi que será el codigo padre de las unidades asociadas

                    if (entity.Grupocodi != null) //Modo de Operación
                    {
                        if (entity.Hopfalla == ConstantesHorasOperacion.HopFalla)
                        {
                            //solo se guardar bitacora modificadas por el usuario
                            if (entity.BitacoraHophoriniFecha != null && entity.BitacoraHophoriniFecha != string.Empty && entity.BitacoraHophoriniHora != null && entity.BitacoraHophoriniHora != string.Empty
                                && entity.BitacoraHophorfinFecha != null && entity.BitacoraHophorfinFecha != string.Empty && entity.BitacoraHophorfinHora != null && entity.BitacoraHophorfinHora != string.Empty)
                            {
                                entity.BitacoraHophorini = DateTime.ParseExact(entity.BitacoraHophoriniFecha + " " + entity.BitacoraHophoriniHora, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                                entity.BitacoraHophorfin = DateTime.ParseExact(entity.BitacoraHophorfinFecha + " " + entity.BitacoraHophorfinHora, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);

                                entity.Evencodi = this.GrabarBitacora(entity.BitacoraHophorini.Value, entity.BitacoraHophorfin.Value, entity.BitacoraIdTipoEvento, entity.BitacoraDescripcion, entity.BitacoraDetalle, entity.BitacoraComentario
                                   , entity.BitacoraIdSubCausaEvento, entity.BitacoraIdEvento, entity.BitacoraIdEquipo, entity.BitacoraIdEmpresa, entity.BitacoraIdTipoOperacion, usuario);
                            }
                        }

                        entity.Hopcodipadre = 0;
                        codigo = this.GuardarHoraOP(entity);
                        entity.Hopcodi = codigo;
                    }
                }

                //guardar detalle
                if (entity.OpcionCrud == 1 || entity.OpcionCrud == 2)  // Nuevo
                {
                    //2. CRUD sobre el detalle de horas de operación (equipos o unidades). Guardar los hijos (EVE_HO_UNIDAD)
                    foreach (var itemHoUnidad in entity.ListaHoUnidad)
                    {
                        itemHoUnidad.Hopcodi = entity.OpcionCrud == 1 ? codigo : entity.Hopcodi;
                        itemHoUnidad.Emprcodi = entity.Emprcodi;
                        //itemHoUnidad.Hopuniactivo = hop.Hopestado == ConstantesAppServicio.Activo ? ConstantesHorasOperacion.HOUnidadActivo : ConstantesHorasOperacion.HOUnidadInactivo,

                        if (itemHoUnidad.Hopunicodi == 0)
                        {
                            itemHoUnidad.Hopuniusucreacion = usuario;
                            itemHoUnidad.Hopunifeccreacion = fechaModificacion;
                            itemHoUnidad.Hopuniactivo = 1;
                            FactorySic.GetEveHoUnidadRepository().Save(itemHoUnidad);

                            //guardar desglose
                            this.CreateOrUpdateRestriccionOperativa(entity, entity.Hopcodi, itemHoUnidad.Equicodi, usuario, fechaModificacion, false);
                        }
                        else
                        {
                            itemHoUnidad.Hopuniusumodificacion = usuario;
                            itemHoUnidad.Hopunifecmodificacion = fechaModificacion;
                            FactorySic.GetEveHoUnidadRepository().Update(itemHoUnidad);

                            //guardar desglose
                            this.CreateOrUpdateRestriccionOperativa(entity, entity.Hopcodi, itemHoUnidad.Equicodi, usuario, fechaModificacion, itemHoUnidad.Hopuniactivo != 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Guarda registro de horas de operación y el detalle de envio
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="listaHorasOperacionBD"></param>
        /// <param name="listCodHop"></param>
        /// <param name="listCodHopElim"></param>
        /// <param name="usuario"></param>
        public void GuardarHorasdeOperacionAgente(List<EveHoraoperacionDTO> lista, int idEmpresa, List<EveHoraoperacionDTO> listaHorasOperacionBD, ref List<int> listCodHop, ref List<int> listCodHopElim, string usuario)
        {
            int codiHoraOPNew = 0;
            List<CodigosPadresHO> ListaCodPadre = new List<CodigosPadresHO>();
            lista = lista.OrderByDescending(x => x.Hopcodi).ToList();
            DateTime fechaModificacion = DateTime.Now;

            List<PrGrupoDTO> listaModosOperacion = this.ListarModoOperacionXCentralYEmpresa(-2, idEmpresa);

            foreach (var entity in lista)
            {
                //si no es termica entonces el equipo es el mismo de la hora de operación
                entity.Equicodi = (entity.Hopcodipadre ?? 0) != 0 || (entity.Grupocodi ?? 0) <= 0 ? entity.Equicodi : GetEquicodiHoraOperacionAgente(entity.Grupocodi ?? 0, lista.Where(x => x.Hopcodipadre == entity.Hopcodi).ToList(), listaModosOperacion);
                entity.Lastdate = DateTime.Now;
                entity.Lastuser = usuario;
                entity.Hopestado = ConstantesAppServicio.Activo;
                entity.Hoparrqblackstart = entity.Hoparrqblackstart ?? ConstantesAppServicio.NO;
                entity.Hopensayope = ConstantesSubcausaEvento.SubcausaPorPruebas == entity.Subcausacodi && entity.Hopensayope != null ? entity.Hopensayope : ConstantesAppServicio.NO;
                entity.Hopensayopmin = ConstantesSubcausaEvento.SubcausaPorPruebas == entity.Subcausacodi && entity.Hopensayopmin != null ? entity.Hopensayopmin : ConstantesAppServicio.NO;
                entity.Emprcodi = idEmpresa;

                // si la info llego de BD
                if (entity.Hopcodi > -1)
                {
                    #region Actualización
                    if (entity.OpcionCrud == -1) // Eliminado lógico
                    {
                        entity.Hopestado = ConstantesAppServicio.Baja;
                        this.UpdateHorasOperacion(entity);
                        listCodHop.Add(entity.Hopcodi);
                        listCodHopElim.Add(entity.Hopcodi);
                    }
                    if (entity.OpcionCrud == 2) // Actualizacion
                    {
                        var regEnBD = listaHorasOperacionBD.Find(x => x.Hopcodi == entity.Hopcodi);
                        if (regEnBD != null)
                        {
                            entity.Hopnotifuniesp = regEnBD.Hopnotifuniesp < ConstantesHorasOperacion.FlagUnidadEspecialAgenteCreacion
                            ? ConstantesHorasOperacion.FlagUnidadEspecialAgenteModificacionFromAdmin : ConstantesHorasOperacion.FlagUnidadEspecialAgenteModificacionPropio;
                        }
                        this.UpdateHorasOperacion(entity);
                        listCodHop.Add(entity.Hopcodi);

                        if (entity.Grupocodi != null) // si se esta editando un modo de operaciòn, rescatamos el codigo padre para las unidades
                        {
                            CodigosPadresHO Obj = new CodigosPadresHO
                            {
                                Hopcodipadre = (int)entity.Hopcodi,
                                Hopcodi = (int)entity.Hopcodi
                            };
                            ListaCodPadre.Add(Obj);

                        }
                    }
                    if (entity.OpcionCrud == 0) // sin cambios
                    {
                        listCodHop.Add(entity.Hopcodi);
                    }

                    #endregion
                }
                // si es nuevo por ser codigo negativo entonces guardar
                else
                {
                    #region Registro
                    entity.Hopnotifuniesp = ConstantesHorasOperacion.FlagUnidadEspecialAgenteCreacion;

                    // si se trata de un modo de operacion termoelectrico, entonnces registramos su nuevo Hopcodi que será el codigo padre de las unidades asociadas
                    if (entity.Grupocodi != null)
                    {
                        entity.Hopcodipadre = 0;
                        codiHoraOPNew = this.GuardarHoraOP(entity);

                        //***guardar el codiHoraOPNew para almacenarlo en las unidades relacionadas
                        CodigosPadresHO Obj = new CodigosPadresHO
                        {
                            Hopcodipadre = codiHoraOPNew,
                            Hopcodi = entity.Hopcodi // viene de memoria
                        };
                        listCodHop.Add(codiHoraOPNew);
                        ListaCodPadre.Add(Obj);
                    }
                    else // si es equipo o unidad
                    {
                        if (entity.CodiPadre != 0) // si se trata de una unidad
                        {
                            var find = ListaCodPadre.Find(x => x.Hopcodi == entity.Hopcodipadre);
                            if (find != null)
                            {

                                entity.Hopcodipadre = find.Hopcodipadre;
                                codiHoraOPNew = this.GuardarHoraOP(entity);
                                listCodHop.Add(codiHoraOPNew);
                            }
                        }
                        else
                        {
                            codiHoraOPNew = this.GuardarHoraOP(entity);
                            listCodHop.Add(codiHoraOPNew);
                        }
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Grabar bitacora asociado a la desconexión de algún equipo por falla y permitir ingresar descripción de dicho evento
        /// Este evento esta asociado al check fuera de servicio y hora de fuera de servicio (inicio).
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTipoEvento"></param>
        /// <param name="descripcion"></param>
        /// <param name="detalle"></param>
        /// <param name="comentarios"></param>
        /// <param name="idSubCausaEvento"></param>
        /// <param name="idEvento"></param>
        /// <param name="idEquipo"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoOperacion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GrabarBitacora(DateTime fechaInicio, DateTime fechaFin, int? idTipoEvento, string descripcion, string detalle, string comentarios
            , int? idSubCausaEvento, int? idEvento, int? idEquipo, int? idEmpresa, int? idTipoOperacion, string usuario)
        {
            TimeSpan diferencia = fechaFin.Subtract(fechaInicio);
            int id = 0;
            if (diferencia.TotalSeconds >= 0)
            {
                EveEventoDTO entity = new EveEventoDTO
                {
                    Tipoevencodi = idTipoEvento,
                    Evenini = fechaInicio,
                    Evenfin = fechaFin,
                    Evenasunto = descripcion,
                    Evendesc = detalle,
                    Evencomentarios = comentarios,
                    Subcausacodi = idSubCausaEvento,
                    Eventipofalla = ConstantesAppServicio.NO,
                    Eventipofallafase = ConstantesAppServicio.NO,
                    Smsenviar = ConstantesAppServicio.NO,
                    Evenrelevante = 0,
                    Evenctaf = ConstantesAppServicio.NO,
                    Eveninffalla = ConstantesAppServicio.NO,
                    Eveninffallan2 = ConstantesAppServicio.NO,
                    Smsenviado = ConstantesAppServicio.NO,
                    Twitterenviado = ConstantesAppServicio.NO,
                    Evenclasecodi = 1,
                    Deleted = ConstantesAppServicio.NO,
                    Evencodi = idEvento.GetValueOrDefault(0),
                    Equicodi = idEquipo,
                    Emprcodi = idEmpresa,
                    Emprcodirespon = idEmpresa,
                    Evenpreliminar = ConstantesAppServicio.SI,
                    Subcausacodiop = idTipoOperacion
                };

                id = this.servEventos.GrabarBitacora(entity, usuario);
            }

            return id;
        }

        /// <summary>
        /// Listar los Motivos de Operacion Forzada
        /// </summary>
        /// <returns></returns>
        public static List<MotivoOperacionForzada> ListarMotivoOperacionForzada()
        {
            List<MotivoOperacionForzada> lista = new List<MotivoOperacionForzada>
            {
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoNoDefinido, Motdesc = ConstantesMotivoOperacionForzada.DescNoDefinido },
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoReqPropio, Motdesc = ConstantesMotivoOperacionForzada.DescReqPropio },
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoSeguridad, Motdesc = ConstantesMotivoOperacionForzada.DescSeguridad },
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoTension, Motdesc = ConstantesMotivoOperacionForzada.DescTension },
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoEvitarArranqueParada, Motdesc = ConstantesMotivoOperacionForzada.DescEvitarArranqueParada },
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoEcuador, Motdesc = ConstantesMotivoOperacionForzada.DescEcuador },
                new MotivoOperacionForzada() { Motcodi = ConstantesMotivoOperacionForzada.CodigoOtros, Motdesc = ConstantesMotivoOperacionForzada.Descotros }
            };

            return lista;
        }

        /// <summary>
        /// Get Descripcion Motivo Operacion Forzada
        /// </summary>
        /// <param name="motcodi"></param>
        /// <returns></returns>
        public string GetDescripcionMotivoOperacionForzada(int motcodi)
        {
            var obj = HorasOperacionAppServicio.ListarMotivoOperacionForzada().Find(x => x.Motcodi == motcodi);
            return obj != null ? obj.Motdesc : ConstantesMotivoOperacionForzada.DescNoDefinido;
        }

        /// <summary>
        /// Devuelve la descripcion del sistema aislado
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string GetDescripcionSistemaAislado(int? valor)
        {
            string text;
            if (valor == -1) text = "NODEF";
            else
            {
                if (valor == 0 || valor == null) text = "";
                else text = "AISLADO" + valor.ToString().PadLeft(2, '0');
            }

            return text;
        }

        /// <summary>
        /// Devuelve la descripcion del limite de transmision
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string GetDescripcionLimTransm(string valor)
        {
            return valor == ConstantesAppServicio.SI ? ConstantesHorasOperacion.LimTransm : string.Empty;
        }

        /// <summary>
        /// Devuelve la descripcion de Realizó ensayo de potencia efectiva
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string GetDescripcionEnsayoPe(string valor)
        {
            return valor == ConstantesAppServicio.SI ? ConstantesAppServicio.SIDesc : string.Empty;
        }

        /// <summary>
        /// Devuelve la descripcion de Realizó ensayo de potencia efectiva
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string GetDescripcionEnsayoPmin(string valor)
        {
            return valor == ConstantesAppServicio.SI ? ConstantesAppServicio.SIDesc : string.Empty;
        }

        /// <summary>
        /// Metodo para enviar correo sobre modificaciones a las horas de operacion
        /// </summary>
        /// <param name="listCodHopElim"></param>
        /// <param name="lista"></param>
        /// <param name="listaHorasOperacionBD"></param>
        /// <param name="fechaModificacion"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        public void EnviarCorreoNotificacionHayCambioHorasOperacion(List<int> listCodHopElim, List<EveHoraoperacionDTO> lista, List<EveHoraoperacionDTO> listaHorasOperacionBD
            , DateTime fechaModificacion, DateTime fechaPeriodo, int idEmpresa, int idEnvio)
        {
            //Horas de operación modificadas
            DateTime fechaNotificables = fechaModificacion.Date.AddDays(-1);
            List<EveHoraoperacionDTO> listaHopNotificables = lista.Where(x => x.Hophorini.Value.Date <= fechaNotificables).ToList();

            DateTime fechaValidacion = fechaModificacion.Date.AddHours(5);
            listaHopNotificables = listaHopNotificables.Where(x => (x.Hophorini.Value.Date == fechaNotificables && x.Lastdate > fechaValidacion) //Despues de las 5am de hoy
                                                              || x.Hophorini.Value.Date <= fechaNotificables.AddDays(-1)).ToList();

            //Horas de operación eliminadas
            List<EveHoraoperacionDTO> listaHopNotificablesElim = listaHorasOperacionBD.Where(x => x.Hophorini.Value.Date <= fechaNotificables).ToList();
            listaHopNotificablesElim = listaHopNotificablesElim.Where(x => (x.Hophorini.Value.Date == fechaNotificables && x.Lastdate > fechaValidacion) //Despues de las 5am de hoy
                                                              || x.Hophorini.Value.Date <= fechaNotificables.AddDays(-1)).ToList();
            listaHopNotificablesElim = listaHopNotificablesElim.Where(x => listCodHopElim.Contains(x.Hopcodi)).ToList();

            //Enviar correo
            if (listaHopNotificables.Count > 0 || listaHopNotificablesElim.Count > 0)
            {
                try
                {
                    SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesHorasOperacion.PlantcodiNotifHoModif);//modificar el valor

                    string asunto = string.Format(plantilla.Plantasunto, fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha));
                    string contenido = this.GetContenidoCorreo(listaHopNotificables, listaHorasOperacionBD);
                    string from = TipoPlantillaCorreo.MailFrom;
                    string to = plantilla.Planticorreos;
                    string cc = plantilla.PlanticorreosCc;
                    string bcc = plantilla.PlanticorreosBcc;

                    if (!string.IsNullOrEmpty(contenido))
                    {
                        List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to);
                        List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc);
                        List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true, true);
                        asunto = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                        to = string.Join(";", listaTo);
                        cc = string.Join(";", listaCC);
                        bcc = string.Join(";", listaBCC);

                        //Enviar correo
                        COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asunto, contenido, null);

                        SiCorreoDTO correo = new SiCorreoDTO
                        {
                            Corrasunto = asunto,
                            Corrcontenido = contenido,
                            Corrfechaenvio = fechaModificacion,
                            Corrfechaperiodo = fechaPeriodo,
                            Corrfrom = from,
                            Corrto = to,
                            Corrcc = cc,
                            Corrbcc = bcc,
                            Emprcodi = idEmpresa <= 0 ? 1 : idEmpresa,
                            Enviocodi = idEnvio,
                            Plantcodi = plantilla.Plantcodi
                        };

                        this.servCorreo.SaveSiCorreo(correo);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                }
            }
        }

        /// <summary>
        /// Generación del contenido del correo
        /// </summary>
        /// <param name="listaDespues"></param>
        /// <param name="listaAntes"></param>
        /// <returns></returns>
        private string GetContenidoCorreo(List<EveHoraoperacionDTO> listaDespues, List<EveHoraoperacionDTO> listaAntes)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<html>");

            str.Append(" <head><STYLE TYPE='text/css'>");
            str.Append(" body {{font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}}");
            str.Append("    .mail {{width:500px;border-spacing:0;border-collapse:collapse;}}");
            str.Append("    .mail thead th {{text-align: center;background: #417AA7;color:#ffffff}}");
            str.Append("    .mail tr td {{border:1px solid #dddddd;}}");
            str.Append(" table.tabla_hop thead th {text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;}");
            str.Append("</style>");
            str.Append("</head>");

            str.Append("<body>");
            str.Append("<b>Estimados Señores.</b><br></br>");
            str.Append("Se le informa que se ha encontrado cambios en las Horas de Operación.<br/>");
            str.Append("<br/>Horas de Operación antes de ser modificadas:<br/>");
            str.Append(this.GetHtmlTablaHopEmail(listaAntes));
            str.Append("<br/>Horas de Operación modificadas:<br/>");
            str.Append(this.GetHtmlTablaHopEmail(listaDespues));

            str.Append("<br/><br/>");
            str.Append("Atentamente,<br><br>");
            str.Append("<b>COES SINAC</b><br>");
            str.Append("Calle Manuel Roaud y Paz Soldan 364. San Isidro, Lima - Perú. Teléfono: (511) 611-8585");
            str.Append("</body>");

            str.Append("</html>");

            return str.ToString();
        }

        /// <summary>
        /// Html tabla para email
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string GetHtmlTablaHopEmail(List<EveHoraoperacionDTO> lista)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<table class='tabla_hop'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Código</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Empresa</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Central</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Modos de Operación/Grupos</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Orden de Arranque</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>EN PARALELO</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Orden de Parada</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>FIN DE REGISTRO</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Calificación</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Motivo Operación Forzada</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Lím Transm.</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px; width: 106px !important'>Observación</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px; width: 106px !important'>Observación del agente</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Usuario modificación</th>");
            str.Append("<th style='text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;padding: 5px; white-space: nowrap; overflow: hidden; font-size: 11px;'>Fecha modificación</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    str.AppendFormat("<tr id='hop{0}' class=''>", reg.Hopcodi);

                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.Hopcodi);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: left !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.Emprnomb);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: left !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.PadreNombre);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: left !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.EquipoNombre);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.HophorordarranqDesc);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.HophoriniDesc); //paralelo
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.HophorparadaDesc);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.HophorfinDesc); // fin de paralelo
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.Subcausadesc);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.HopcausacodiDesc);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.HoplimtransDesc);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: left !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.Hopdesc);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: left !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.Hopobs);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.Lastuser);
                    str.AppendFormat("<td style='vertical-align: middle; padding: 1px; border: 1px solid #dddddd; font-size: 11px; color: #335873;; white-space: nowrap; overflow: hidden; text-align: center !important; padding-left: 7px !important;  padding-right: 7px !important;'>{0}</td>", reg.LastdateDesc);

                    str.Append("</tr>");
                }
            }
            else
            {
                str.Append("<tr>");
                str.Append("<td colspan='12' style='text-align: left'></td>");
                str.Append("</tr>");
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        #endregion

        #region Desglose

        /// <summary>
        /// REgistrar, Actualizar o Eliminar Restricciones Operativas de Plena Carga, Potencia Fija, Potencia Mínima, Potencia Máxima
        /// </summary>
        /// <param name="hopModo"></param>
        /// <param name="hopcodiModo"></param>
        /// <param name="equicodi"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaRegistro"></param>
        /// <param name="eliminar"></param>
        public void CreateOrUpdateRestriccionOperativa(EveHoraoperacionDTO hopModo, int hopcodiModo, int? equicodi, string usuario, DateTime fechaRegistro, bool eliminar)
        {
            if (hopModo != null && hopcodiModo > 0 && hopModo.ListaDesglose != null && equicodi > 0)
            {
                //bd
                List<EveHoEquiporelDTO> listaDesgloseRangoBD = this.ListEveHoEquiporelByHopcodi(hopcodiModo).Where(x => x.Equicodi == equicodi).ToList();

                foreach (var reg in hopModo.ListaDesglose)
                {
                    //
                    reg.Subcausacodi = HorasOperacionAppServicio.ListarTipoDesglose().Find(x => x.TipoDesglose == reg.TipoDesglose).Subcausacodi;

                    //asociar Iccodi a lista que viene de la interfaz web
                    var regCand = listaDesgloseRangoBD.Find(x => x.Subcausacodi == reg.Subcausacodi && x.Ichorini == reg.Ichorini && x.Ichorfin == reg.Ichorfin);
                    if (regCand != null)
                    {
                        int iccodiCand = regCand.Iccodi;
                        var regSinIccodi = hopModo.ListaDesglose.Find(x => x.Iccodi == iccodiCand);
                        if (regSinIccodi == null)
                            reg.Iccodi = iccodiCand;
                    }
                }

                foreach (var reg in hopModo.ListaDesglose)
                {
                    //asociar Iccodi a lista que viene de la interfaz web
                    var regCand = listaDesgloseRangoBD.Find(x => x.Subcausacodi == reg.Subcausacodi);
                    if (regCand != null)
                    {
                        int iccodiCand = regCand.Iccodi;
                        var regSinIccodi = hopModo.ListaDesglose.Find(x => x.Iccodi == iccodiCand);
                        if (regSinIccodi == null)
                            reg.Iccodi = iccodiCand;
                    }
                }

                List<int> inListaIccodi = hopModo.ListaDesglose.Select(x => x.Iccodi).ToList();

                //////////////////////////////////////////////////////////////////////////////////////
                var listaDesgloseUserSelect = listaDesgloseRangoBD.Where(x => inListaIccodi.Contains(x.Iccodi)).ToList();
                var listaDesgloseUserNoSelect = listaDesgloseRangoBD.Where(x => !inListaIccodi.Contains(x.Iccodi)).ToList();

                if (eliminar)
                {
                    listaDesgloseUserSelect = new List<EveHoEquiporelDTO>();
                    listaDesgloseUserNoSelect = listaDesgloseRangoBD;
                }

                //Actualizar
                foreach (var regBd in listaDesgloseUserSelect)
                {
                    var regWeb = hopModo.ListaDesglose.Find(x => x.Iccodi == regBd.Iccodi);

                    //IEODCUADRO
                    EveIeodcuadroDTO entity = this.servOpVarias.ObtenerIeodCuadro(regBd.Iccodi);
                    entity.Iccodi = regBd.Iccodi;
                    entity.Equicodi = equicodi;
                    entity.Emprcodi = hopModo.Emprcodi;
                    entity.Subcausacodi = regBd.Subcausacodi;
                    entity.Ichorini = regWeb.Ichorini;
                    entity.Ichorfin = regWeb.Ichorfin;
                    entity.Icvalor1 = regWeb.Icvalor1;
                    entity.Evenclasecodi = ConstantesOperacionesVarias.EvenclasecodiPdiario;
                    entity.Lastuser = usuario;
                    entity.Lastdate = fechaRegistro;

                    int corrIeod = this.servOpVarias.Save(entity);
                }

                //Eliminar
                foreach (var regNoselect in listaDesgloseUserNoSelect)
                {
                    //Eliminar registros de HO_EQUIREL
                    FactorySic.GetEveHoEquiporelRepository().Delete(regNoselect.Hoequicodi);

                    //IEODCUADRO
                    this.servOpVarias.DeleteOperacion(regNoselect.Iccodi);
                }

                //////////////////////////////////////////////////////////////////////////////////////
                //Nuevo
                var listaDesgloseRegistrados = listaDesgloseRangoBD.Select(x => x.Iccodi).ToList();
                var listaDesgloseNuevo = hopModo.ListaDesglose.Where(x => !listaDesgloseRegistrados.Contains(x.Iccodi)).ToList();
                foreach (var regNuevo in listaDesgloseNuevo)
                {
                    //Registrar en la IEODCUADRO
                    EveIeodcuadroDTO entity = new EveIeodcuadroDTO
                    {
                        Equicodi = equicodi,
                        Subcausacodi = regNuevo.Subcausacodi,
                        Ichorini = regNuevo.Ichorini,
                        Ichorfin = regNuevo.Ichorfin,
                        Icvalor1 = regNuevo.Icvalor1,
                        Evenclasecodi = ConstantesOperacionesVarias.EvenclasecodiPdiario,
                        Lastuser = usuario,
                        Lastdate = fechaRegistro
                    };

                    int corrIeod = this.servOpVarias.Save(entity);

                    //Registrar en HO_EQUIREL
                    EveHoEquiporelDTO reg = new EveHoEquiporelDTO
                    {
                        Hopcodi = hopcodiModo,
                        Iccodi = corrIeod,
                        Equicodi = equicodi.Value,
                        Hoequitipo = regNuevo.TipoDesglose
                    };

                    FactorySic.GetEveHoEquiporelRepository().Save(reg);
                }
            }
        }

        /// <summary>
        /// Listar los tipos de desglose para la hora de operación
        /// </summary>
        /// <returns></returns>
        public static List<TipoDesgloseHoraOperacion> ListarTipoDesglose()
        {
            List<TipoDesgloseHoraOperacion> l = new List<TipoDesgloseHoraOperacion>
            {
                new TipoDesgloseHoraOperacion() { Subcausacodi = ConstantesOperacionesVarias.SubcausacodiPlenacarga, Subcausadesc = ConstantesHorasOperacion.HoequitipoEventoPlenacargaDesc, TipoDesglose = ConstantesHorasOperacion.HoequitipoEventoPlenacarga },
                new TipoDesgloseHoraOperacion() { Subcausacodi = ConstantesOperacionesVarias.SubcausacodiPotenciaFija, Subcausadesc = ConstantesHorasOperacion.HoequitipoEventoPotenciaFijaDesc, TipoDesglose = ConstantesHorasOperacion.HoequitipoEventoPotenciaFija },
                new TipoDesgloseHoraOperacion() { Subcausacodi = ConstantesOperacionesVarias.SubcausacodiPotenciaMax, Subcausadesc = ConstantesHorasOperacion.HoequitipoEventoPotenciaMaxDesc, TipoDesglose = ConstantesHorasOperacion.HoequitipoEventoPotenciaMax },
                new TipoDesgloseHoraOperacion() { Subcausacodi = ConstantesOperacionesVarias.SubcausacodiPotenciaMin, Subcausadesc = ConstantesHorasOperacion.HoequitipoEventoPotenciaMinDesc, TipoDesglose = ConstantesHorasOperacion.HoequitipoEventoPotenciaMin }
            };

            return l;
        }

        #endregion

        #region Validación de Cruces de Horas Operación

        /// <summary>
        /// ListarValidacionCruce
        /// </summary>
        /// <param name="listaHOweb"></param>
        /// <param name="listaHObd"></param>
        /// <returns></returns>
        public List<ValidacionHoraOperacion> ListarValidacionCruce(List<EveHoraoperacionDTO> listaHOweb
                                                        , List<EveHoraoperacionDTO> listaHObd)
        {
            List<ValidacionHoraOperacion> listaVal = new List<ValidacionHoraOperacion>();

            //0. Solo quedarse con horas de operación que pueden estar en el sistema
            List<EveHoraoperacionDTO> listaHoFinal = ListarHoWebAValidar(listaHOweb, listaHObd);
            listaHoFinal = listaHoFinal.Where(x => x.Subcausacodi > 0).ToList(); //no considerar los "NO IDENTIFICADO"

            //generar lista ho
            var listaHoUnidad = new List<EveHoUnidadDTO>();
            foreach (var item in listaHoFinal) listaHoUnidad.AddRange(item.ListaHoUnidad);
            List<int> listaEquicodiBD = listaHoUnidad.Select(x => x.Equicodi).Distinct().ToList();
            if (listaEquicodiBD.Any())
            {
                var listaEqBD = FactorySic.GetEqEquipoRepository().ListByIdEquipo(string.Join(",", listaEquicodiBD));

                //actualizar nombre de equipo
                foreach (var hoUnidad in listaHoUnidad)
                {
                    var regEq = listaEqBD.Find(x => x.Equicodi == hoUnidad.Equicodi);
                    if (regEq != null)
                    {
                        hoUnidad.Equiabrev = (regEq.Equinomb ?? "").Trim();
                    }
                }
            }

            //1. verifica si hay cruces de horas de operacion a nivel de modos de operación
            foreach (var agrupXModo in listaHoFinal.Where(x => x.FlagModoEspecial != "S").GroupBy(x => x.Grupocodi))
            {
                int grupocodiModo = agrupXModo.Key.Value;
                var listaHoXmodo = agrupXModo.ToList();
                var total = listaHoXmodo.Count;

                //comparar ho con las otras ho del modo
                foreach (var hoActual in listaHoXmodo)
                {
                    var listaHoModoNoActual = listaHoXmodo.Where(x => x.Hopcodi != hoActual.Hopcodi);

                    foreach (var hoOtro in listaHoModoNoActual)
                    {
                        if (HayCruceHorario(hoActual.Hophorini.Value, hoActual.Hophorfin.Value, hoOtro.Hophorini.Value, hoOtro.Hophorfin.Value))
                        {
                            Ordenar2EveHoraoperacion(hoActual, hoOtro, out EveHoraoperacionDTO ho1, out EveHoraoperacionDTO ho2);
                            string hoDesc1 = string.Format("[{0} de {1} a {2}]", ho1.Gruponomb, ho1.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho1.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                            string hoDesc2 = string.Format("[{0} de {1} a {2}]", ho2.Gruponomb, ho2.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho2.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                            listaVal.Add(new ValidacionHoraOperacion() { Mensaje = string.Format("Existen cruces en horas de operación {0} y {1}. Debe eliminar y/o actualizar horas de operación. ", hoDesc1, hoDesc2) });
                        }
                    }
                }
            }

            //2. Verificar a nivel de unidades
            if (!listaVal.Any())
            {
                var listaEquicodi = listaHoUnidad.GroupBy(x => x.Equicodi).Select(x => x.Key).ToList();
                foreach (var equicodi in listaEquicodi)
                {
                    var listaHoUnidadXEqui = listaHoUnidad.Where(x => x.Equicodi == equicodi).ToList(); //filtrar sobre todos los equipos de todas las horas de operación

                    //comparar ho con las otras ho del modo
                    foreach (var hoActual in listaHoUnidadXEqui)
                    {
                        var listaHoModoNoActual = listaHoUnidadXEqui.Where(x => x.Hopunicodi != hoActual.Hopunicodi).ToList();

                        foreach (var hoOtro in listaHoModoNoActual)
                        {
                            if (HayCruceHorario(hoActual.Hophorini.Value, hoActual.Hophorfin.Value, hoOtro.Hophorini.Value, hoOtro.Hophorfin.Value))
                            {
                                Ordenar2EveHoUnidad(hoActual, hoOtro, out EveHoUnidadDTO ho1, out EveHoUnidadDTO ho2);
                                string hoDesc1 = string.Format("[{0} de {1} a {2}]", ho1.Equiabrev, ho1.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho1.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                                string hoDesc2 = string.Format("[{0} de {1} a {2}]", ho2.Equiabrev, ho2.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho2.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                                listaVal.Add(new ValidacionHoraOperacion() { Mensaje = string.Format("Existe Cruce de Horas en las unidades {0} y {1}. ", hoDesc1, hoDesc2) });
                            }
                        }
                    }
                }
            }

            //3. Validación adicional en modos de operación especial
            if (!listaVal.Any())
            {
                foreach (var agrupXModo in listaHoFinal.Where(x => x.FlagModoEspecial == "S").GroupBy(x => x.Grupocodi))
                {
                    int grupocodiModo = agrupXModo.Key.Value;
                    var listaHoXmodo = agrupXModo.ToList();
                    var listaHopcodi = listaHoXmodo.Select(x => x.Hopcodi).ToList();

                    //4. Verificar que dentro de los modos de operación Especial haya paralelo de las unidades (no debe haber huecos entre sus unidades)
                    foreach (var hoActual in listaHoXmodo)
                    {
                        var objFictInterv = new InIntervencionDTO() { Equicodi = 0, Interfechaini = hoActual.Hophorini.Value, Interfechafin = hoActual.Hophorfin.Value };
                        var listaHoXEqXDia = hoActual.ListaHoUnidad.Select(x => new EveHoraoperacionDTO() { Equicodi = 0, Hophorini = x.Hophorini, Hophorfin = x.Hophorfin, Equiabrev = x.Equiabrev }).ToList();
                        var listaHoCortadoXEqXDia = HorasOperacionAppServicio.GetListaEveHoraOperacionFragmentada(listaHoXEqXDia, new List<InIntervencionDTO>() { objFictInterv }, false)
                                                                .OrderBy(x => x.Hophorini).ToList();

                        listaVal.AddRange(VerificarContinuidadHOModoEspecial(listaHoCortadoXEqXDia, hoActual));
                    }

                    //5. Validar que no haya continuidad / cruce entre las horas de cada equipo
                    foreach (var hoActual in listaHoXmodo)
                    {
                        var listaEquicodiXHo = hoActual.ListaHoUnidad.GroupBy(x => x.Equicodi).Select(x => x.Key).ToList();

                        foreach (var equicodi in listaEquicodiXHo)
                        {
                            var listaHoUnidadXEqui = hoActual.ListaHoUnidad.Where(x => x.Equicodi == equicodi).ToList(); //filtrar sobre todos los equipos de todas las horas de operación

                            //comparar ho con las otras ho del modo
                            foreach (var hoEqActual in listaHoUnidadXEqui)
                            {
                                var listaHoModoNoActual = listaHoUnidadXEqui.Where(x => x.Hopunicodi != hoEqActual.Hopunicodi);

                                foreach (var hoOtro in listaHoModoNoActual)
                                {
                                    if (HayCruceHorario(hoEqActual.Hophorini.Value, hoEqActual.Hophorfin.Value, hoOtro.Hophorini.Value, hoOtro.Hophorfin.Value)
                                        || HayContinuidadHorario(hoEqActual.Hophorfin.Value, hoOtro.Hophorini.Value))
                                    {
                                        Ordenar2EveHoUnidad(hoEqActual, hoOtro, out EveHoUnidadDTO ho1, out EveHoUnidadDTO ho2);
                                        string hoDesc1 = string.Format("[{0} de {1} a {2}]", ho1.Equiabrev, ho1.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho1.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                                        string hoDesc2 = string.Format("[{0} de {1} a {2}]", ho2.Equiabrev, ho2.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho2.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                                        listaVal.Add(new ValidacionHoraOperacion() { Mensaje = string.Format("Existe continuidad en las unidades de {0} y {1}, elimine o actualice. ", hoDesc1, hoDesc2) });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //quitar duplicados de mensajes
            listaVal = listaVal.GroupBy(x => x.Mensaje).Select(x => x.First()).OrderBy(x => x.Mensaje).ToList();

            return listaVal;
        }

        private List<EveHoraoperacionDTO> ListarHoWebAValidar(List<EveHoraoperacionDTO> listaHOweb, List<EveHoraoperacionDTO> listaHObd)
        {
            List<int> listaHopcodi = listaHOweb.Where(x => x.Hopcodi > 0 && x.OpcionCrud != 1).Select(x => x.Hopcodi).ToList();

            List<EveHoraoperacionDTO> listaHoFinal = new List<EveHoraoperacionDTO>();

            //omitir los eliminados y modificados que se encuentran actualmente en BD
            listaHoFinal.AddRange(listaHObd.Where(x => !listaHopcodi.Contains(x.Hopcodi)).ToList());

            //agregar los nuevos registros
            listaHoFinal.AddRange(listaHOweb.Where(x => x.OpcionCrud == 1));

            //agregar los registros modificados o no
            listaHoFinal.AddRange(listaHOweb.Where(x => x.OpcionCrud == 2 || x.OpcionCrud == 0));

            //asignar códigos Ficticios (estos códigos no se guardan, solo sirven para validar cruce)
            int hopcodiMax = 99999999;
            int hopunicodiMax = 99999999;
            foreach (var hoModo in listaHoFinal)
            {
                if (hoModo.Hopcodi <= 0)
                {
                    hoModo.Hopcodi = hopcodiMax;

                    hoModo.ListaHoUnidad = hoModo.ListaHoUnidad ?? new List<EveHoUnidadDTO>();
                    hoModo.ListaHoUnidad = hoModo.ListaHoUnidad.Where(x => x.OpcionCrud != -1).ToList(); //no considerar los eliminados
                    foreach (var item in hoModo.ListaHoUnidad)
                    {
                        item.Hopcodi = hopcodiMax;
                        item.Hopunicodi = hopunicodiMax;
                        hopunicodiMax++;
                    }

                    hopcodiMax++;
                }
                else
                {
                    hoModo.ListaHoUnidad = hoModo.ListaHoUnidad ?? new List<EveHoUnidadDTO>();
                    hoModo.ListaHoUnidad = hoModo.ListaHoUnidad.Where(x => x.OpcionCrud != -1).ToList(); //no considerar los eliminados
                    foreach (var item in hoModo.ListaHoUnidad)
                    {
                        if (item.Hopunicodi <= 0)
                        {
                            item.Hopunicodi = hopunicodiMax;
                            hopunicodiMax++;
                        }
                    }
                }
            }

            return listaHoFinal;
        }

        private List<ValidacionHoraOperacion> VerificarContinuidadHOModoEspecial(List<EveHoraoperacionDTO> listaHoCortadoXEqXDia, EveHoraoperacionDTO hoActual)
        {
            string hoDesc0 = string.Format("[{0} de {1} a {2}]", hoActual.Gruponomb, hoActual.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), hoActual.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));

            List<ValidacionHoraOperacion> listaVal = new List<ValidacionHoraOperacion>();

            if (listaHoCortadoXEqXDia.Count > 0)
            {
                var dateIni = listaHoCortadoXEqXDia.First().Hophorini;
                var dateFin = listaHoCortadoXEqXDia.Last().Hophorfin;

                //el inicio y fin de la hora a nivel de modo debe coincidir con el menor inicio y mayor fin de los unidades
                if (hoActual.Hophorini == dateIni && dateFin == hoActual.Hophorfin)
                {
                    if (listaHoCortadoXEqXDia.Count >= 2)
                    {
                        for (var j = 0; j < listaHoCortadoXEqXDia.Count - 1; j++)
                        {
                            Ordenar2EveHoraoperacion(listaHoCortadoXEqXDia[j], listaHoCortadoXEqXDia[j + 1], out EveHoraoperacionDTO ho1, out EveHoraoperacionDTO ho2);

                            if (!(HayCruceHorario(ho1.Hophorini.Value, ho1.Hophorfin.Value, ho2.Hophorini.Value, ho2.Hophorfin.Value)
                                    || HayContinuidadHorario(ho1.Hophorfin.Value, ho2.Hophorini.Value)))
                            {
                                string hoDesc1 = string.Format("[{0} de {1} a {2}]", ho1.Equiabrev, ho1.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho1.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                                string hoDesc2 = string.Format("[{0} de {1} a {2}]", ho2.Equiabrev, ho2.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora), ho2.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora));
                                return new List<ValidacionHoraOperacion>() { new ValidacionHoraOperacion() { Mensaje = string.Format("No existe continuidad de la operación {0} y {1}. ", hoDesc1, hoDesc2) } };
                            }
                        }

                        return listaVal;
                    }
                }
                else
                {
                    return new List<ValidacionHoraOperacion>() { new ValidacionHoraOperacion() { Mensaje = string.Format("Existe Hora de operación {0} no válida. ", hoDesc0) } };
                }
            }
            else
            {
                return new List<ValidacionHoraOperacion>() { new ValidacionHoraOperacion() { Mensaje = string.Format("Existe Hora de operación {0} no válida. ", hoDesc0) } };
            }

            return listaVal;
        }

        private void Ordenar2EveHoraoperacion(EveHoraoperacionDTO hoActual, EveHoraoperacionDTO hoOtro, out EveHoraoperacionDTO ho1, out EveHoraoperacionDTO ho2)
        {
            if (hoActual.Hophorini < hoOtro.Hophorini)
            {
                ho1 = hoActual;
                ho2 = hoOtro;
            }
            else
            {
                if (hoActual.Hophorini == hoOtro.Hophorini)
                {
                    if (hoActual.Hophorfin < hoOtro.Hophorfin)
                    {
                        ho1 = hoActual;
                        ho2 = hoOtro;
                    }
                    else
                    {
                        ho1 = hoOtro;
                        ho2 = hoActual;
                    }
                }
                else
                {
                    ho1 = hoOtro;
                    ho2 = hoActual;
                }
            }
        }

        private void Ordenar2EveHoUnidad(EveHoUnidadDTO hoActual, EveHoUnidadDTO hoOtro, out EveHoUnidadDTO ho1, out EveHoUnidadDTO ho2)
        {
            if (hoActual.Hophorini < hoOtro.Hophorini)
            {
                ho1 = hoActual;
                ho2 = hoOtro;
            }
            else
            {
                if (hoActual.Hophorini == hoOtro.Hophorini)
                {
                    if (hoActual.Hophorfin < hoOtro.Hophorfin)
                    {
                        ho1 = hoActual;
                        ho2 = hoOtro;
                    }
                    else
                    {
                        ho1 = hoOtro;
                        ho2 = hoActual;
                    }
                }
                else
                {
                    ho1 = hoOtro;
                    ho2 = hoActual;
                }
            }
        }

        private bool IsBetweenMoment(DateTime fecha, DateTime fechaIni, DateTime fechaFin)
        {
            //https://momentjscom.readthedocs.io/en/latest/moment/05-query/06-is-between/
            return fechaIni < fecha && fecha < fechaFin;
        }

        private bool HayCruceHorario(DateTime horaIni, DateTime horaFin, DateTime dateFrom, DateTime dateTo)
        {
            //Funcion para indicar si existe cruce de horas
            if (this.IsBetweenMoment(horaIni, dateFrom, dateTo))
                return true;
            if (this.IsBetweenMoment(horaFin, dateFrom, dateTo))
                return true;
            if (this.IsBetweenMoment(dateFrom, horaIni, horaFin))
                return true;
            if (this.IsBetweenMoment(dateTo, horaIni, horaFin))
                return true;
            if (dateFrom == horaIni && dateTo == horaFin)
                return true;

            return false;
        }

        private bool HayContinuidadHorario(DateTime horaFinIzquierda, DateTime dateFinDerecha)
        {
            return horaFinIzquierda == dateFinDerecha;
        }

        private ValidacionHoraOperacion VerificarCrucesHorasOperacionGlobal(List<EveHoraoperacionDTO> listaHorasOperacion, int idTipoCentral
            , int? equicodi, int? grupocodi, int tipoHO, List<PrGrupoDTO> listaModosOperacion, List<EqEquipoDTO> codUnidadesExtras, DateTime horaIni, DateTime horaFin, int pos)
        {
            // Funciones que analizan cruces de horas de operacion, unidades
            // 1: si no hay cruces de horas de operación
            // 2: si hay cruces de horas de operacion 
            //function verificaCrucesHorasOperacion(tipoCentral, equicodi, fecha, fechaFin, enParalelo, fueraParalelo, pos)

            ValidacionHoraOperacion val = new ValidacionHoraOperacion() { HoraOperacion = null, TipoCruce = ConstantesHorasOperacion.CruceHoNoExiste };

            if (listaHorasOperacion.Count > 0)
            {
                //verificar que el registro no tenga eliminado lógico si vino de BD propiedad opCdrud = -1            
                switch (idTipoCentral)
                {
                    case ConstantesHorasOperacion.IdTipoHidraulica:
                    case ConstantesHorasOperacion.IdTipoSolar:
                    case ConstantesHorasOperacion.IdTipoEolica:
                        val = this.HayCruceEquicodiHo(listaHorasOperacion, equicodi.Value, horaIni, horaFin, pos);
                        break;
                    case ConstantesHorasOperacion.IdTipoTermica:

                        //Validación por Modo de Operación
                        if (ConstantesHorasOperacion.TipoHOUnidadEspecial == tipoHO)
                        {
                            val.TipoCruce = this.HayCruceUnidadEspecial(listaHorasOperacion, codUnidadesExtras, horaIni, horaFin)
                                ? ConstantesHorasOperacion.CruceHoUnidadEspecialSiExisteRepartirHo : ConstantesHorasOperacion.CruceHoNoExiste;
                        }
                        else
                        {
                            val = this.HayCruceGrupocodiHo(listaHorasOperacion, grupocodi.Value, horaIni, horaFin, pos);
                        }

                        //Valida por Unidades del Modo de Operación
                        if (ConstantesHorasOperacion.CruceHoNoExiste == val.TipoCruce)
                        {
                            if (ConstantesHorasOperacion.PosicionNuevo != pos)
                            {
                                val = this.HayCruceUnidadesXModoHoUpdate(listaHorasOperacion, listaModosOperacion, grupocodi.Value, pos, horaIni, horaFin);
                            }
                            else
                            {
                                val = this.HayCruceUnidadesXModoHoNew(listaHorasOperacion, listaModosOperacion, grupocodi.Value, horaIni, horaFin, tipoHO);
                            }
                        }
                        break;
                }
            }

            if (ConstantesHorasOperacion.CruceHoNoExiste != val.TipoCruce)
            {
                val.Mensaje = val.HoraOperacion != null ? "Existe cruces entre HO actual y HO[Código=" + val.HoraOperacion.Hopcodi + "].\nActualizar horas de operación." : "Existe cruces de horas de Operación.\nActualizar horas de operación.";
            }

            return val;
        }

        private ValidacionHoraOperacion HayCruceEquicodiHo(List<EveHoraoperacionDTO> listaHorasOperacion
            , int equicodi, DateTime horaIni, DateTime horaFin, int pos)
        {
            //horasoperacionValidacion.js?v=4.3.3
            //=> function verificaCrucesHorasOperacion(tipoCentral, equicodi, fecha, fechaFin, enParalelo, fueraParalelo, pos) {
            for (var i = 0; i < listaHorasOperacion.Count; i++)
            {
                if (listaHorasOperacion[i].Equicodi == equicodi && listaHorasOperacion[i].OpcionCrud != -1 && i != pos)
                { // si hay cruce de horas con el rango de horas ingresadas
                    var dateFrom = listaHorasOperacion[i].Hophorini;
                    var dateTo = listaHorasOperacion[i].Hophorfin;
                    if (horaIni >= dateFrom && horaIni <= dateTo)
                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[i], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                    if (horaFin >= dateFrom && horaFin <= dateTo)
                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[i], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                    if (dateFrom >= horaIni && dateFrom <= horaFin)
                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[i], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                    if (dateTo >= horaIni && dateTo <= horaFin)
                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[i], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                    if (dateFrom == horaIni && dateTo == horaFin)
                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[i], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                }
            }

            return new ValidacionHoraOperacion() { HoraOperacion = null, TipoCruce = ConstantesHorasOperacion.CruceHoNoExiste };
        }

        private ValidacionHoraOperacion HayCruceGrupocodiHo(List<EveHoraoperacionDTO> listaHorasOperacion
            , int grupocodi, DateTime horaIni, DateTime horaFin, int pos)
        {
            //horasoperacionValidacion.js?v=4.3.3
            //=> function verificarCruceModoHO(listaHo, objHoDTO) {

            for (var i = 0; i < listaHorasOperacion.Count; i++)
            {
                if (listaHorasOperacion[i].FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo
                    && listaHorasOperacion[i].Grupocodi == grupocodi && listaHorasOperacion[i].OpcionCrud != -1 && i != pos)
                { // si hay cruce de horas con el rango de horas ingresadas 
                    var dateFrom = listaHorasOperacion[i].Hophorini.Value;
                    var dateTo = listaHorasOperacion[i].Hophorfin.Value;

                    if (this.HayCruceHorario(horaIni, horaFin, dateFrom, dateTo))
                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[i], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                }
            }

            return new ValidacionHoraOperacion() { HoraOperacion = null, TipoCruce = ConstantesHorasOperacion.CruceHoNoExiste };
        }

        /// <summary>
        /// Verifica si hay cruce de ho de modo con sus unidades cuando es actualización
        /// </summary>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaModosOperacion"></param>
        /// <param name="grupocodi"></param>
        /// <param name="pos"></param>
        /// <param name="horaIni"></param>
        /// <param name="horaFin"></param>
        /// <returns></returns>
        //function hayCruceUnidadesHO_UPDATE(grupocodi, pos, horaIni, horaFin)
        private ValidacionHoraOperacion HayCruceUnidadesXModoHoUpdate(List<EveHoraoperacionDTO> listaHorasOperacion, List<PrGrupoDTO> listaModosOperacion
            , int grupocodi, int pos, DateTime horaIni, DateTime horaFin)
        {
            var checkdateFrom = listaHorasOperacion[pos].Hophorini;
            var checkdateTo = listaHorasOperacion[pos].Hophorfin;

            PrGrupoDTO regModo = listaModosOperacion.Find(x => x.Grupocodi == grupocodi);

            if (regModo != null && regModo.ListaEquicodi.Count > 0 && listaHorasOperacion.Count > 0)
            {
                for (var j = 0; j < regModo.ListaEquicodi.Count; j++)
                {
                    for (var zz = 0; zz < listaHorasOperacion.Count; zz++)
                    {
                        //verificar que el registro no tenga eliminado lógico si vino de BD prOpiedad opCdrud != -1  y no sea una actualizacion                               
                        if (listaHorasOperacion[zz].Equicodi == regModo.ListaEquicodi[j] && listaHorasOperacion[zz].OpcionCrud != -1)
                        {
                            var dateFrom = listaHorasOperacion[zz].Hophorini;
                            var dateTo = listaHorasOperacion[zz].Hophorfin;

                            if (!(dateFrom == checkdateFrom) && !(dateTo == checkdateTo))
                            {
                                var hopPadre = listaHorasOperacion.Find(x => x.Hopcodi == listaHorasOperacion[zz].Hopcodipadre);
                                if (horaIni >= dateFrom && horaIni < dateTo)
                                    return new ValidacionHoraOperacion() { HoraOperacion = hopPadre, HoraOperacionUnidadEspecial = listaHorasOperacion[zz], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                                if (horaFin >= dateFrom && horaFin <= dateTo)
                                    return new ValidacionHoraOperacion() { HoraOperacion = hopPadre, HoraOperacionUnidadEspecial = listaHorasOperacion[zz], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                            }
                        }
                    }
                }
            }
            return new ValidacionHoraOperacion() { HoraOperacion = null, TipoCruce = ConstantesHorasOperacion.CruceHoNoExiste };
        }

        /// <summary>
        /// Verifica si hay cruce de ho de modo con sus unidades cuando es nuevo
        /// </summary>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaModosOperacion"></param>
        /// <param name="grupocodi"></param>
        /// <param name="horaIni"></param>
        /// <param name="horaFin"></param>
        /// <param name="tipoHO"></param>
        /// <returns></returns>
        //function hayCruceUnidadesHO(grupocodi, horaIni, horaFin, tipoHO, listUnid) 
        public ValidacionHoraOperacion HayCruceUnidadesXModoHoNew(List<EveHoraoperacionDTO> listaHorasOperacion, List<PrGrupoDTO> listaModosOperacion,
            int grupocodi, DateTime horaIni, DateTime horaFin, int tipoHO)
        {
            PrGrupoDTO regModo = listaModosOperacion.Find(x => x.Grupocodi == grupocodi);
            List<int> listUnid = (regModo?.ListaEquicodi) ?? new List<int>();

            if (tipoHO == ConstantesHorasOperacion.TipoHOUnidadEspecial)
            {
                //tipo de hora de operacion especial
                for (var j = 0; j < listUnid.Count; j++)
                {
                    if (listaHorasOperacion.Count > 0)
                    {
                        for (var zz = 0; zz < listaHorasOperacion.Count; zz++)
                        {
                            //verificar que el registro no tenga eliminado lógico si vino de BD porpiedad opCdrud != -1  y no sea una actualizacion                               
                            if (listaHorasOperacion[zz].Equicodi == listUnid[j] && listaHorasOperacion[zz].OpcionCrud != -1)
                            {
                                var dateFrom = listaHorasOperacion[zz].Hophorini.Value;
                                var dateTo = listaHorasOperacion[zz].Hophorfin.Value;

                                if (this.HayCruceHorario(horaIni, horaFin, dateFrom, dateTo))
                                    return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[zz], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                            }
                        }
                    }
                }
            }

            if (tipoHO == ConstantesHorasOperacion.TipoHONormal)
            {
                // tipo de hora de operacion normal
                if (listUnid.Count > 0)
                {
                    for (var m = 0; m < listUnid.Count; m++)
                    {
                        if (listaHorasOperacion.Count > 0)
                        {
                            for (var nn = 0; nn < listaHorasOperacion.Count; nn++)
                            {
                                //verificar que el registro no tenga eliminado lógico si vino de BD propiedad opCdrud != -1  y no sea una actualizacion                               
                                if (listaHorasOperacion[nn].Equicodi == listUnid[m] && listaHorasOperacion[nn].OpcionCrud != -1)
                                {//dudas?? en == listUnid[m].Equicodi, en el .js es sin equicodi
                                    var dateFrom = listaHorasOperacion[nn].Hophorini.Value;
                                    var dateTo = listaHorasOperacion[nn].Hophorfin.Value;

                                    if (this.HayCruceHorario(horaIni, horaFin, dateFrom, dateTo))
                                        return new ValidacionHoraOperacion() { HoraOperacion = listaHorasOperacion[nn], TipoCruce = ConstantesHorasOperacion.CruceHoSiExiste };
                                }
                            }
                        }
                    }
                }

            }

            return new ValidacionHoraOperacion() { HoraOperacion = null, TipoCruce = ConstantesHorasOperacion.CruceHoNoExiste };
        }

        /// <summary>
        /// Verificar si cruza Hop con unidades especiales
        /// </summary>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="horaIni"></param>
        /// <param name="horaFin"></param>
        /// <param name="codUnidadesExtras"></param>
        /// <returns></returns>
        public bool HayCruceUnidadEspecial(List<EveHoraoperacionDTO> listaHorasOperacion
            , List<EqEquipoDTO> codUnidadesExtras, DateTime horaIni, DateTime horaFin)
        {
            if (listaHorasOperacion.Count > 0)
            {
                for (var i = 0; i < listaHorasOperacion.Count; i++)
                {
                    if (listaHorasOperacion[i].OpcionCrud != -1)
                    { // si hay cruce de horas con el rango de horas ingresadas                        
                        var dateFrom = listaHorasOperacion[i].Hophorini;
                        var dateTo = listaHorasOperacion[i].Hophorfin;

                        if (horaIni >= dateFrom && horaIni <= dateTo)
                        {
                            if (dateTo >= horaIni && dateTo <= horaFin)
                            {// si esta parcialmente saliendo a la derecha ->
                                for (int n = 0; n < codUnidadesExtras.Count; n++)
                                {
                                    if (listaHorasOperacion[i].Equicodi == codUnidadesExtras[n].Equicodi)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            { // si nueva hora esta incluida en otra hora de operación
                                for (int n = 0; n < codUnidadesExtras.Count; n++)
                                {
                                    if (listaHorasOperacion[i].Equicodi == codUnidadesExtras[n].Equicodi)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        if (horaFin >= dateFrom && horaFin <= dateTo)
                        {
                            if (dateFrom >= horaIni && dateFrom <= horaFin)
                            {// <- si esta parcialemnte saliendo a la izquierda 
                                for (int n = 0; n < codUnidadesExtras.Count; n++)
                                {
                                    if (listaHorasOperacion[i].Equicodi == codUnidadesExtras[n].Equicodi)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        if ((dateFrom >= horaIni && dateFrom <= horaFin)
                            && (dateTo >= horaIni && dateTo <= horaFin))
                        { // si la nueva hora de operacion cubre toda una hora de operacion previa
                            for (int n = 0; n < codUnidadesExtras.Count; n++)
                            {
                                if (listaHorasOperacion[i].Equicodi == codUnidadesExtras[n].Equicodi)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region Útil - Validación con Intervenciones

        /// <summary>
        /// Lista Hora de Operación Fragmentada
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaIntervenciones"></param>
        /// <param name="flagFicticios"></param>
        /// <returns></returns>
        public static List<EveHoraoperacionDTO> GetListaEveHoraOperacionFragmentada(List<EveHoraoperacionDTO> data, List<InIntervencionDTO> listaIntervenciones, bool flagFicticios)
        {
            //Generar fragmentacion de la data
            List<EveHoraoperacionDTO> resultFinalTmp = HorasOperacionAppServicio.GetListaHoraOperacionDividida(data, listaIntervenciones);

            //Validar con HP
            List<EveHoraoperacionDTO> resultFinal = new List<EveHoraoperacionDTO>();

            foreach (var reg in listaIntervenciones)
            {
                List<EveHoraoperacionDTO> listaTmp = resultFinalTmp.Where(x => reg.Interfechaini <= x.Hophorini && x.Hophorfin <= reg.Interfechafin).ToList();
                bool existeFicticioEnSublista = listaTmp.Find(x => x.EventoGenerado == ConstantesIndisponibilidades.EventoGeneradoFicticio) != null;
                bool existeRealEnSublista = listaTmp.Find(x => x.EventoGenerado == ConstantesIndisponibilidades.EventoGeneradoFicticio) != null;

                if (flagFicticios)
                {
                    listaTmp = resultFinalTmp.Where(x => x.EventoGenerado == ConstantesIndisponibilidades.EventoGeneradoFicticio).ToList();
                    resultFinalTmp.RemoveAll(x => listaTmp.Contains(x));

                    if (existeRealEnSublista) listaTmp = new List<EveHoraoperacionDTO>();
                }
                else
                {
                    listaTmp = resultFinalTmp.Where(x => x.EventoGenerado != ConstantesIndisponibilidades.EventoGeneradoFicticio).ToList();
                    resultFinalTmp.RemoveAll(x => listaTmp.Contains(x));
                }

                resultFinal.AddRange(listaTmp);
            }

            return resultFinal;
        }

        /// <summary>
        /// Ver: COES.Servicios.Aplicacion.Indisponibilidades / IndisponibilidadesAppServicio / GetListaManttoDividida(
        /// Dividir todos las Horas de operacion por fechas (fechas de eventos, inicio del dia, fin del dia, hora punta)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaIntervenciones"></param>
        /// <returns></returns>
        private static List<EveHoraoperacionDTO> GetListaHoraOperacionDividida(List<EveHoraoperacionDTO> data, List<InIntervencionDTO> listaIntervenciones)
        {
            List<EveHoraoperacionDTO> resultFinalTmp = new List<EveHoraoperacionDTO>();

            List<DateTime> listaFecha = data.Select(x => x.Hophorini.Value.Date).Distinct().OrderBy(x => x).ToList();
            List<int> listaEquicodi = data.Select(x => x.Equicodi.GetValueOrDefault(0)).Distinct().ToList();

            foreach (var fecha in listaFecha)
            {
                List<InIntervencionDTO> listaIntXDia = listaIntervenciones.Where(x => x.Interfechaini.Date == fecha).ToList();

                foreach (var equicodi in listaEquicodi)
                {
                    List<EveHoraoperacionDTO> dataXEq = data.Where(x => x.Equicodi == equicodi && x.Hophorini.Value.Date == fecha).ToList();
                    List<InIntervencionDTO> hoXEq = listaIntervenciones.Where(x => x.Equicodi == equicodi && x.Interfechaini.Date == fecha).ToList();

                    List<DateTime> listaFechaHoIniXEq = hoXEq.Select(x => x.Interfechaini).Distinct().OrderBy(x => x).ToList();
                    List<DateTime> listaFechaHoFinXEq = hoXEq.Select(x => x.Interfechafin).Distinct().ToList();

                    List<DateTime> listaFechaHoXEq = new List<DateTime>();
                    listaFechaHoXEq.AddRange(listaFechaHoIniXEq);
                    listaFechaHoXEq.AddRange(listaFechaHoFinXEq);

                    List<EveHoraoperacionDTO> resultXEqXFechaTotal = ListarHorasDivididoXHoras(fecha, dataXEq, listaFechaHoXEq);
                    resultFinalTmp.AddRange(resultXEqXFechaTotal);
                }
            }

            return resultFinalTmp;
        }

        /// <summary>
        /// Dividir horas de operación
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dataXEq"></param>
        /// <param name="listaFechaHoXEq"></param>
        /// <returns></returns>
        public static List<EveHoraoperacionDTO> ListarHorasDivididoXHoras(DateTime fecha, List<EveHoraoperacionDTO> dataXEq, List<DateTime> listaFechaHoXEq)
        {
            List<EveHoraoperacionDTO> resultXEqXFechaTotal = new List<EveHoraoperacionDTO>();

            // Lista de Fechas por equipo
            List<DateTime> listaFechaIniXEq = dataXEq.Select(x => x.Hophorini.Value).Distinct().OrderBy(x => x).ToList();
            List<DateTime> listaFechaFinXEq = dataXEq.Select(x => x.Hophorfin.Value).Distinct().OrderBy(x => x).ToList();

            List<DateTime> listaFechaXEq = new List<DateTime>
            {
                fecha, //inicio del día
                fecha.AddDays(1) //fin del día
            };

            listaFechaXEq.AddRange(listaFechaIniXEq);
            listaFechaXEq.AddRange(listaFechaFinXEq);
            listaFechaXEq.AddRange(listaFechaHoXEq);
            listaFechaXEq = listaFechaXEq.Distinct().OrderBy(x => x).ToList();

            foreach (var reg in dataXEq)
            {
                List<EveHoraoperacionDTO> resultXEqXFecha = new List<EveHoraoperacionDTO>();
                for (int fi = 0; fi < listaFechaXEq.Count - 1; fi++)
                {
                    DateTime factual = listaFechaXEq[fi];
                    DateTime fsiguiente = listaFechaXEq[fi + 1];

                    #region Generación de corte

                    //El evento intersecta completo
                    if (reg.Hophorini <= factual && fsiguiente <= reg.Hophorfin)
                    {
                        //Las fechas estan incluidas en el rango
                        EveHoraoperacionDTO e1 = new EveHoraoperacionDTO
                        {
                            Hopcodi = reg.Hopcodi,
                            Hophorini = factual,
                            Hophorfin = fsiguiente
                        };

                        resultXEqXFecha.Add(e1);
                    }
                    else
                    {
                        //El ficticio insertecta completo
                        if (reg.Hophorfin <= factual || fsiguiente <= reg.Hophorini)
                        {
                            EveHoraoperacionDTO e1 = new EveHoraoperacionDTO
                            {
                                Hopcodi = reg.Hopcodi,
                                EventoGenerado = ConstantesIndisponibilidades.EventoGeneradoFicticio,
                                Hophorini = factual,
                                Hophorfin = fsiguiente
                            };

                            resultXEqXFecha.Add(e1);
                        }
                        else
                        {
                            //Incluido abierdo dentro de las 2 fechas
                            if (factual < reg.Hophorini && reg.Hophorfin < fsiguiente)
                            {
                                EveHoraoperacionDTO e1 = new EveHoraoperacionDTO
                                {
                                    Hophorini = reg.Hophorini,
                                    Hophorfin = reg.Hophorfin
                                };

                                EveHoraoperacionDTO e2 = new EveHoraoperacionDTO
                                {
                                    EventoGenerado = ConstantesIndisponibilidades.EventoGeneradoFicticio,
                                    Hophorini = factual,
                                    Hophorfin = reg.Hophorini
                                };

                                EveHoraoperacionDTO e3 = new EveHoraoperacionDTO
                                {
                                    EventoGenerado = ConstantesIndisponibilidades.EventoGeneradoFicticio,
                                    Hophorini = reg.Hophorfin,
                                    Hophorfin = fsiguiente
                                };

                                resultXEqXFecha.Add(e1);
                                resultXEqXFecha.Add(e2);
                                resultXEqXFecha.Add(e3);
                            }
                            else
                            {
                                if (reg.Hophorini <= factual)
                                {
                                    //Incluye total izq y parcial der
                                    EveHoraoperacionDTO e1 = new EveHoraoperacionDTO
                                    {
                                        Hophorini = factual,
                                        Hophorfin = reg.Hophorfin
                                    };

                                    resultXEqXFecha.Add(e1);

                                    EveHoraoperacionDTO e2 = new EveHoraoperacionDTO
                                    {
                                        EventoGenerado = ConstantesIndisponibilidades.EventoGeneradoFicticio,
                                        Hophorini = reg.Hophorfin,
                                        Hophorfin = fsiguiente
                                    };

                                    resultXEqXFecha.Add(e2);
                                }

                                if (reg.Hophorfin >= fsiguiente)
                                {
                                    //Incluye par izq y total der
                                    EveHoraoperacionDTO e1 = new EveHoraoperacionDTO
                                    {
                                        EventoGenerado = ConstantesIndisponibilidades.EventoGeneradoFicticio,
                                        Hophorini = factual,
                                        Hophorfin = reg.Hophorini
                                    };

                                    resultXEqXFecha.Add(e1);

                                    EveHoraoperacionDTO e2 = new EveHoraoperacionDTO
                                    {
                                        Hophorini = reg.Hophorini,
                                        Hophorfin = fsiguiente
                                    };

                                    resultXEqXFecha.Add(e2);
                                }
                            }
                        }
                    }
                    #endregion
                }

                //Agregar valores del mantto
                foreach (var rtmp in resultXEqXFecha)
                {
                    rtmp.Hopcodi = reg.Hopcodi;
                    rtmp.Hopcodipadre = reg.Hopcodipadre;
                    rtmp.Equicodi = reg.Equicodi;
                    rtmp.Emprcodi = reg.Emprcodi;
                    rtmp.Hopdesc = reg.Hopdesc;
                    rtmp.Lastuser = reg.Lastuser;
                    rtmp.Lastdate = reg.Lastdate;
                    rtmp.Subcausadesc = reg.Subcausadesc;
                }

                resultXEqXFechaTotal.AddRange(resultXEqXFecha);
            }
            resultXEqXFechaTotal = resultXEqXFechaTotal.OrderBy(x => x.Hopcodi).ThenBy(x => x.Hophorini).ToList();

            return resultXEqXFechaTotal;
        }

        /// <summary>
        /// Ver: COES.Servicios.Aplicacion.Indisponibilidades / IndisponibilidadesAppServicio / GetListaManttoDividida(
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<EveHoraoperacionDTO> GetListaHoraOperacionUnificada(List<EveHoraoperacionDTO> data)
        {
            List<EveHoraoperacionDTO> listaIndNuevo = new List<EveHoraoperacionDTO>();
            for (int i = 0; i < data.Count; i++)
            {
                var indActual = data[i];

                var indNuevo = new EveHoraoperacionDTO
                {
                    Hopcodi = indActual.Hopcodi,
                    Equicodi = indActual.Equicodi,
                    Emprcodi = indActual.Emprcodi,
                    Hophorini = indActual.Hophorini,
                    Hophorfin = indActual.Hophorfin,
                    Hopdesc = indActual.Hopdesc,
                    Subcausadesc = indActual.Subcausadesc,
                    Lastdate = indActual.Lastdate,
                    Lastuser = indActual.Lastuser,
                    Hopcodipadre = indActual.Hopcodipadre
                };
                listaIndNuevo.Add(indNuevo);

                //buscar por interseccion
                bool terminoBusqueda = false;
                for (int j = i + 1; j < data.Count && !terminoBusqueda; j++)
                {
                    if (indNuevo.Hophorini <= data[j].Hophorini && data[j].Hophorini <= indNuevo.Hophorfin
                        && indNuevo.Hophorfin <= data[j].Hophorfin)
                    {
                        indNuevo.Hophorfin = data[j].Hophorfin;
                    }
                    else
                    {
                        terminoBusqueda = true;
                        i = j - 1;
                    }
                }
                if (!terminoBusqueda)
                {
                    i = data.Count;
                }
            }

            return listaIndNuevo;
        }

        #endregion

        #region Validación de Intervenciones

        /// <summary>
        /// Verificar las horas de operación antes de registrarse en base de datos 
        /// </summary>
        /// <param name="listaHo"></param>
        /// <param name="fechaReg"></param>
        /// <param name="consultarOtros"></param>
        /// <param name="listaHoOut"></param>
        /// <param name="listaValInterv"></param>
        public void VerificarIntervencionFS(List<EveHoraoperacionDTO> listaHo, DateTime fechaReg, bool consultarOtros, out List<EveHoraoperacionDTO> listaHoOut, out List<ResultadoValidacionAplicativo> listaValInterv)
        {
            DateTime fechaIni = fechaReg.Date;
            DateTime fechaFin = fechaIni.AddDays(1);

            //Tratar horas de operacion de la interfaz grafica
            List<EveHoraoperacionDTO> listaHoValidos = listaHo.Where(x => x.OpcionCrud != -1).ToList(); // Eliminado lógico
            List<EveHoraoperacionDTO> listaHoModo = listaHoValidos.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();

            List<InIntervencionDTO> listaIntervencion = (new IntervencionesAppServicio()).ListarIntervencionesEquiposGen(fechaIni, fechaIni, ConstantesHorasOperacion.IdGeneradorTemoelectrico, ConstantesHorasOperacion.IdTipoTermica)
                .Where(x => x.Interindispo == ConstantesIntervencionesAppServicio.sFS).ToList();

            List<EveHoraoperacionDTO> listaHoData = new List<EveHoraoperacionDTO>();
            foreach (var regHo in listaHoModo)
            {
                regHo.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
                List<EveHoraoperacionDTO> listaHoUnidades = listaHoValidos.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();
                foreach (var reg in listaHoUnidades) { reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoUnidad; }

                listaHoData.Add(regHo);
                listaHoData.AddRange(listaHoUnidades);
            }

            //Obtener las mantto de esas horas de operacion
            List<EqEquipoDTO> listaEq = listaHoData.Where(x => x.Equicodi > 0).GroupBy(x => new { x.Equicodi, x.Equiabrev, x.Emprcodi })
                .Select(x => new EqEquipoDTO() { Equicodi = x.Key.Equicodi.Value, Equinomb = x.Key.Equiabrev, Emprcodi = x.Key.Emprcodi }).ToList();
            this.ListarHorasOperacionUnidadesRegistrados(fechaIni, listaHoData, listaEq, new List<int> { ConstantesHorasOperacion.ValidarIntervenciones }, new List<MeMedicion48DTO>(), new List<MeMedicion96DTO>(), listaIntervencion
                , consultarOtros
                , out List<ResultadoValidacionAplicativo> listaValEmsUniReg, out List<ResultadoValidacionAplicativo> listaValScadaUniReg, out listaValInterv);

            ////////////////////////////////////////////////////////////////////////////////////////////////
            listaHoOut = new List<EveHoraoperacionDTO>();
            List<int> listaHopcodiOut = listaValInterv.Select(x => x.HoraOperacion.Hopcodi).Distinct().ToList();
            foreach (var hopcodi in listaHopcodiOut)
            {
                listaHoOut.Add(listaValInterv.Where(x => x.HoraOperacion.Hopcodi == hopcodi).ToList().First().HoraOperacion);
            }
        }

        /// <summary>
        /// Metodo para enviar correo sobre modificaciones a las horas de operacion
        /// </summary>
        /// <param name="listaHo"></param>
        /// <param name="listaValInterv"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaModificacion"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        public void EnviarCorreoNotificacionIntervencionesFS(List<EveHoraoperacionDTO> listaHo, List<ResultadoValidacionAplicativo> listaValInterv
            , string usuario, DateTime fechaModificacion, DateTime fechaPeriodo, int idEmpresa, int idEnvio)
        {
            //Enviar correo
            if (listaHo.Count > 0 || listaValInterv.Count > 0)
            {
                try
                {
                    SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesHorasOperacion.PlantcodiNotifIntervencionesFS);

                    if (plantilla != null)
                    {
                        string asunto = string.Format(plantilla.Plantasunto, fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha));
                        string contenido = this.GetContenidoCorreoIntervencionesFS(listaHo, listaValInterv);

                        List<string> listaToTmp = plantilla.Planticorreos.Split(';').Select(x => x).ToList();
                        List<string> listaToInterv = listaValInterv.Where(x => !string.IsNullOrEmpty(x.UltimaModificacionUsuarioCorreo)).Select(x => x.UltimaModificacionUsuarioCorreo).Distinct().ToList();
                        listaToTmp.AddRange(listaToInterv);

                        string from = TipoPlantillaCorreo.MailFrom;
                        string to = string.Join(" ", listaToTmp);
                        string cc = plantilla.PlanticorreosCc;
                        string bcc = plantilla.PlanticorreosBcc;

                        if (!string.IsNullOrEmpty(contenido))
                        {
                            List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to);
                            List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc);
                            List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true, true);
                            asunto = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                            to = string.Join(";", listaTo);
                            cc = string.Join(";", listaCC);
                            bcc = string.Join(";", listaBCC);

                            COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asunto, contenido, null);

                            SiCorreoDTO correo = new SiCorreoDTO
                            {
                                Corrasunto = asunto,
                                Corrcontenido = contenido,
                                Corrfechaenvio = fechaModificacion,
                                Corrfechaperiodo = fechaPeriodo,
                                Corrfrom = from,
                                Corrto = to,
                                Corrcc = cc,
                                Corrbcc = bcc,
                                Emprcodi = idEmpresa <= 1 ? 1 : idEmpresa,
                                Enviocodi = idEnvio,
                                Plantcodi = plantilla.Plantcodi
                            };

                            this.servCorreo.SaveSiCorreo(correo);

                            SiMensajeDTO entity = new SiMensajeDTO
                            {
                                Msgfecha = fechaModificacion,
                                Msgfechaperiodo = fechaPeriodo,
                                Msgcontenido = contenido + "\n\n",
                                Emprcodi = idEmpresa <= 1 ? 1 : idEmpresa,
                                Msgasunto = asunto,
                                Msgusucreacion = usuario,
                                Msgfeccreacion = fechaModificacion,
                                Msgto = to,
                                Msgfrom = from
                            };

                            (new IntervencionesAppServicio()).EnviarMensaje(ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado, entity, ConstantesIntervencionesAppServicio.TMsgcodiAlertaHo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                }
            }
        }

        private string GetContenidoCorreoIntervencionesFS(List<EveHoraoperacionDTO> listaHoOut, List<ResultadoValidacionAplicativo> listaValInterv)
        {
            string html = @"
                <html>

                    <head>
	                    <STYLE TYPE='text/css'>
	                    body {{font-size: .80em;font-family: 'Helvetica Neue', 'Lucida Grande', 'Segoe UI', Arial, Helvetica, Verdana, sans-serif;}}
	                    .mail {{width:500px;border-spacing:0;border-collapse:collapse;}}
	                    .mail thead th {{text-align: center;background: #417AA7;color:#ffffff}}
	                    .mail tr td {{border:1px solid #dddddd;}}
	                    table.tabla_hop thead th {text-align: center; background: #2980B9; color: #ffffff; cursor: pointer; border: 1px solid #9AC9E9;}
	                    </style>
                    </head>

                    <body>
	                    Estimados Señores,
                        <br/> <br>
                        Se ha registrado las siguientes horas de operación con intervenciones F/S:<br/>
                        {cuerpoHo}
                        <br/>
                        Unidades que presentan intervenciones F/S:<br/>
                        {cuerpoInterv}
                        <br/><br/>
    
                        {footer}
                    </body>
                </html>
            ";
            html = html.Replace("{cuerpoHo}", this.GetHtmlTablaValidacionIntervencionesFSEmail(listaHoOut));
            html = html.Replace("{cuerpoInterv}", this.GetHtmlTablaIntervencionesEmail(listaValInterv));

            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreo());

            return html;
        }

        /// <summary>
        /// Html tabla para email
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string GetHtmlTablaIntervencionesEmail(List<ResultadoValidacionAplicativo> lista)
        {
            StringBuilder str = new StringBuilder();

            #region cuerpo

            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    string htmlTr = @"
                        <table class='tabla_hop' style='margin-left: 20px;'>
                            <tbody>
                                <tr>
                                    <td style='font-weight: bold;'>Tipo de intervención:</td>
                                    <td style=''>{0}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Equipo:</td>
                                    <td style=''>{1}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Fecha Inicio:</td>
                                    <td style=''>{2}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Fecha Fin:</td>
                                    <td style=''>{3}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Disponibilidad:</td>
                                    <td style=''>{4}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Descripción:</td>
                                    <td style=''>{5}</td>
                                </tr>
                            </tbody>
                        </table>
                    ";

                    htmlTr = string.Format(htmlTr, reg.Tipoevenabrev, reg.Equinomb, reg.FechaIniDesc, reg.FechaFinDesc, reg.Interindispo, reg.Interdescrip);

                    str.Append(htmlTr);
                }
            }

            #endregion

            return str.ToString();
        }

        /// <summary>
        /// Html tabla para email
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string GetHtmlTablaValidacionIntervencionesFSEmail(List<EveHoraoperacionDTO> lista)
        {
            StringBuilder str = new StringBuilder();

            #region cuerpo

            if (lista.Count > 0)
            {
                foreach (var reg in lista)
                {
                    string htmlTr = @"
                        <table class='tabla_hop' style='margin-left: 20px;'>
                            <tbody>
                                <tr>
                                    <td style='font-weight: bold;'>Empresa:</td>
                                    <td style=''>{0}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Modo de Operación:</td>
                                    <td style=''>{1}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>EN PARALELO:</td>
                                    <td style=''>{2}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>FIN DE REGISTRO:</td>
                                    <td style=''>{3}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight: bold;'>Observación:</td>
                                    <td style=''>{4}</td>
                                </tr>
                            </tbody>
                        </table>
                    ";

                    htmlTr = string.Format(htmlTr, reg.Emprnomb, reg.Gruponomb, reg.HophoriniDesc, reg.HophorfinDesc, reg.Hopdesc);
                    str.Append(htmlTr);
                }
            }

            #endregion

            return str.ToString();
        }

        #endregion

        #region Grafico Recursos energeticos

        /// <summary>
        /// Listar horas de operacion 30min
        /// </summary>
        /// <param name="l"></param>
        /// <param name="infecha"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHO30min(List<EveHoraoperacionDTO> l, DateTime infecha)
        {
            DateTime fecha = infecha.Date;
            List<EveHoraoperacionDTO> lhopFinal = new List<EveHoraoperacionDTO>();

            //listar las HO con modo
            var listaHOModo = l;

            foreach (var reg in listaHOModo)
            {
                DateTime dt1 = reg.Hophorini.Value;
                DateTime dt2 = reg.Hophorfin.Value;

                GetHoraIniFin48Despacho(fecha, dt1, dt2, out int hi, out int hf);

                if (hi <= hf)
                {
                    DateTime di = fecha.AddMinutes(30 * hi);
                    DateTime df = fecha.AddMinutes(30 * hf);
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>
                    {
                        reg
                    };

                    foreach (var h in lhop)
                    {
                        h.HoraIni48 = di;
                        h.HoraFin48 = df;
                        h.HIni48 = (hi == 0 ? 1 : hi);
                        h.HFin48 = hf;
                    }

                    lhopFinal.AddRange(lhop);
                }
            }

            return lhopFinal;
        }

        /// <summary>
        /// Listar horas de operacion 15min
        /// </summary>
        /// <param name="l"></param>
        /// <param name="infecha"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHO15min(List<EveHoraoperacionDTO> l, DateTime infecha)
        {
            DateTime fecha = infecha.Date;
            List<EveHoraoperacionDTO> lhopFinal = new List<EveHoraoperacionDTO>();

            //listar las HO con modo
            var listaHOModo = l;

            foreach (var reg in listaHOModo)
            {
                DateTime dt1 = reg.Hophorini.Value;
                DateTime dt2 = reg.Hophorfin.Value;

                GetHoraIniFin96Medidores(fecha, dt1, dt2, out int hi, out int hf);

                if (hi <= hf)
                {
                    DateTime di = fecha.AddMinutes(15 * hi);
                    DateTime df = fecha.AddMinutes(15 * hf);
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>
                    {
                        reg
                    };

                    foreach (var h in lhop)
                    {
                        h.HoraIni96 = di;
                        h.HoraFin96 = df;
                        h.HIni96 = (hi == 0 ? 1 : hi);
                        h.HFin96 = hf;
                    }

                    lhopFinal.AddRange(lhop);
                }
            }

            return lhopFinal;
        }

        /// <summary>
        /// Listar horas de operacion 15min
        /// </summary>
        /// <param name="listaHo"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHO15minDivididoXh(List<EveHoraoperacionDTO> listaHo)
        {
            List<EveHoraoperacionDTO> lhopFinal = new List<EveHoraoperacionDTO>();

            //listar las HO 
            foreach (var regOriginal in listaHo)
            {
                DateTime fechaProceso = regOriginal.Hophorini.Value.Date; //fecha a las 00:00 de la hora de operación

                if (regOriginal.Hopcodi == 721696)
                { }
                DateTime fechaIniTotal = regOriginal.Hophorini.Value;
                fechaIniTotal = fechaIniTotal.AddSeconds(-fechaIniTotal.Second);//solo deben ser minutos sin segundos
                DateTime fechaFinTotal = regOriginal.Hophorfin.Value;

                //Lo ideal es agregar 1min a la hora de inicio y quitar 1min hora fin para determinar exactamente el cruce
                //si las horas ini y fin siguen igual se obtiene rangos aproximados para los combustibles. el cuarto de hora anterior y posterior 'también' forman parte del combustible de la hora de operación
                GetHoraIniFin96Medidores(fechaProceso, fechaIniTotal, fechaFinTotal, out int hi, out int hf);
                hi = (hi == 0 ? 1 : hi);
                if (hi <= hf)
                {
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>();

                    //por cada hora de operacion original generar "n" horas replicas. cada uno de duración 15 minutos
                    for (int i = hi; i <= hf; i++)
                    {
                        DateTime fechaIniH = fechaProceso.AddMinutes(15 * (i - 1));
                        DateTime fechaFinH = fechaProceso.AddMinutes(15 * i);

                        if (fechaIniTotal > fechaIniH) fechaIniH = fechaIniTotal;
                        if (fechaFinTotal < fechaFinH) fechaFinH = fechaFinTotal;

                        lhop.Add(AgregarHoraOpH15(i, fechaProceso, fechaIniH, fechaFinH, regOriginal, false));
                    }

                    //agregar 1 ficticio anterior a la hora de inicio
                    if (hi == 1)
                    {
                        lhop.Add(AgregarHoraOpH15(96, fechaProceso.AddDays(-1), fechaProceso.AddDays(-1).AddMinutes(-15), fechaProceso.AddDays(-1), regOriginal, true));
                    }
                    else
                    {
                        lhop.Add(AgregarHoraOpH15(hi - 1, fechaProceso, fechaProceso.AddMinutes(15 * (hi - 2)), fechaProceso.AddMinutes(15 * (hi - 1)), regOriginal, true));
                    }

                    //agregar 1 ficticio posterior a la hora de fin
                    if (hf == 96)
                    {
                        lhop.Add(AgregarHoraOpH15(1, fechaProceso.AddDays(1), fechaProceso.AddDays(1), fechaProceso.AddDays(1).AddMinutes(15), regOriginal, true));
                    }
                    else
                    {
                        lhop.Add(AgregarHoraOpH15(hf + 1, fechaProceso, fechaProceso.AddMinutes(15 * (hf)), fechaProceso.AddMinutes(15 * (hf + 1)), regOriginal, true));
                    }

                    //agregar horas a la variable global
                    lhopFinal.AddRange(lhop);
                }
            }

            return lhopFinal;
        }

        private EveHoraoperacionDTO AgregarHoraOpH15(int h, DateTime fechaProceso, DateTime fechaIniH, DateTime fechaFinH, EveHoraoperacionDTO regOriginal, bool esFicticio)
        {
            //puede darse el caso de duraciones igual a cero porque la hora de operación puede iniciar / terminar en múltiplo de 15 y usan rango aproximados
            int totalMinuto = (int)(fechaFinH - fechaIniH).TotalMinutes;
            totalMinuto = totalMinuto > 0 ? totalMinuto : 0;

            if (esFicticio) totalMinuto = 0;

            var regH = (EveHoraoperacionDTO)regOriginal.Clone();
            regH.FechaProceso = fechaProceso;
            regH.HoraIni96 = fechaIniH;
            regH.HoraFin96 = fechaFinH;
            regH.HIni96 = h;
            regH.HFin96 = h;
            regH.TotalMinuto = totalMinuto;
            regH.Es15MinTieneHo = !esFicticio;

            return regH;
        }

        /// <summary>
        /// Listar horas de operacion 15min
        /// </summary>
        /// <param name="l"></param>
        /// <param name="infecha"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHO15minExtranetMedidores(List<EveHoraoperacionDTO> l, DateTime infecha)
        {
            DateTime fecha = infecha.Date;
            List<EveHoraoperacionDTO> lhopFinal = new List<EveHoraoperacionDTO>();

            //listar las HO con modo
            var listaHOModo = l;

            foreach (var reg in listaHOModo)
            {
                DateTime dt1 = reg.Hophorini.Value;
                DateTime dt2 = reg.Hophorfin.Value;

                GetHoraIniFin96ExtranetMedidores(fecha, dt1, dt2, out int hi, out int hf);

                if (hi <= hf)
                {
                    DateTime di = fecha.AddMinutes(15 * hi);
                    DateTime df = fecha.AddMinutes(15 * hf);
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>
                    {
                        reg
                    };

                    foreach (var h in lhop)
                    {
                        h.HoraIni96 = di;
                        h.HoraFin96 = df;
                        h.HIni96 = (hi == 0 ? 1 : hi);
                        h.HFin96 = hf;
                    }

                    lhopFinal.AddRange(lhop);
                }
            }

            return lhopFinal;
        }

        /// <summary>
        /// Listar horas de operacion 30min
        /// </summary>
        /// <param name="l"></param>
        /// <param name="infecha"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHO30minEms(List<EveHoraoperacionDTO> l, DateTime infecha)
        {
            DateTime fecha = infecha.Date;
            List<EveHoraoperacionDTO> lhopFinal = new List<EveHoraoperacionDTO>();

            //listar las HO con modo
            var listaHOModo = l;

            foreach (var reg in listaHOModo)
            {
                DateTime dt1 = reg.Hophorini.Value;
                DateTime dt2 = reg.Hophorfin.Value;

                GetHoraIniFin48Ems(fecha, dt1, dt2, out int hi, out int hf);

                if (hi <= hf)
                {
                    DateTime di = fecha.AddMinutes(30 * hi);
                    DateTime df = fecha.AddMinutes(30 * hf);
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>
                    {
                        reg
                    };

                    foreach (var h in lhop)
                    {
                        h.HoraIni48 = di;
                        h.HoraFin48 = df;
                    }

                    lhopFinal.AddRange(lhop);
                }
            }

            return lhopFinal;
        }

        /// <summary>
        /// Listar horas de operacion 30min
        /// </summary>
        /// <param name="l"></param>
        /// <param name="infecha"></param>
        /// <returns></returns>
        public static List<EveHoraoperacionDTO> ListarHO30minConsumoCombustible(List<EveHoraoperacionDTO> l, DateTime infecha)
        {
            DateTime fecha = infecha.Date;
            List<EveHoraoperacionDTO> lhopFinal = new List<EveHoraoperacionDTO>();

            //listar las HO con modo
            var listaHOModo = l;

            foreach (var reg in listaHOModo)
            {
                DateTime dt1 = reg.Hophorini.Value;
                DateTime dt2 = reg.Hophorfin.Value;

                GetHoraIniFin48CCC(fecha, dt1, dt2, out int hi, out int hf);

                if (hi <= hf)
                {
                    DateTime di = fecha.AddMinutes(30 * hi);
                    DateTime df = fecha.AddMinutes(30 * hf);
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>
                    {
                        reg
                    };

                    foreach (var h in lhop)
                    {
                        h.HoraIni48 = di;
                        h.HoraFin48 = df;
                        h.HIni48 = (hi == 0 ? 1 : hi);
                        h.HFin48 = hf;
                    }

                    lhopFinal.AddRange(lhop);
                }
            }

            return lhopFinal;
        }

        /// <summary>
        /// ListarHO30minConsumoCombustibleDivididoX
        /// </summary>
        /// <param name="listaHO"></param>
        /// <param name="infecha"></param>
        /// <param name="listaHoConCruce"></param>
        /// <param name="listaHoSinCruce"></param>
        public void ListarHO30minConsumoCombustibleDivididoX(List<EveHoraoperacionDTO> listaHO, DateTime infecha,
                                    out List<EveHoraoperacionDTO> listaHoConCruce, out List<EveHoraoperacionDTO> listaHoSinCruce)
        {
            DateTime fechaProceso = infecha.Date;
            listaHoConCruce = new List<EveHoraoperacionDTO>();
            listaHoSinCruce = new List<EveHoraoperacionDTO>();

            foreach (var regOriginal in listaHO)
            {
                DateTime fechaIniTotal = regOriginal.Hophorini.Value;
                fechaIniTotal = fechaIniTotal.AddSeconds(-fechaIniTotal.Second);//solo deben ser minutos sin segundos
                DateTime fechaFinTotal = regOriginal.Hophorfin.Value;

                if (fechaIniTotal.Hour == 10 && fechaIniTotal.Minute == 30)
                { }
                if (fechaFinTotal.Hour == 22 && fechaFinTotal.Minute == 53)
                { }
                GetHoraIniFin48CCC(fechaProceso, fechaIniTotal, fechaFinTotal, out int hi, out int hf);

                if (hi <= hf) //la hora final al menos termina en multiplo de 30 en el mismo h del inicial o es un h posterior al inicial
                {
                    List<EveHoraoperacionDTO> lhop = new List<EveHoraoperacionDTO>();

                    //por cada hora de operacion original generar "n" horas replicas. cada uno de duración 30 minutos
                    for (int i = hi; i <= hf; i++)
                    {
                        DateTime fechaIniH = fechaProceso.AddMinutes(30 * (i - 1));
                        DateTime fechaFinH = fechaProceso.AddMinutes(30 * i);

                        if (fechaIniTotal > fechaIniH) fechaIniH = fechaIniTotal;
                        if (fechaFinTotal < fechaFinH) fechaFinH = fechaFinTotal;

                        lhop.Add(AgregarHoraOpH30(i, fechaProceso, fechaIniH, fechaFinH, regOriginal, false));
                    }

                    //no se toma en cuenta los minutos que sobran (hf + 1) que no terminan en multiplo de 30. Para despacho siempre debe terminar en multiplo de 30
                    if (fechaFinTotal != fechaProceso.AddMinutes(30 * (hf + 1)))
                    {
                        listaHoSinCruce.Add(AgregarHoraOpH30(hf + 1, fechaProceso, fechaProceso.AddMinutes(30 * hf), fechaFinTotal, regOriginal, false));
                    }

                    //agregar horas a la variable global
                    listaHoConCruce.AddRange(lhop);
                }
                else
                {
                    //cuando la hora dura menos a 30min y la hora fin no es multiplo de 30
                    listaHoSinCruce.Add(AgregarHoraOpH30(hi, fechaProceso, fechaIniTotal, fechaFinTotal, regOriginal, false));
                }
            }
        }

        private EveHoraoperacionDTO AgregarHoraOpH30(int h, DateTime fechaProceso, DateTime fechaIniH, DateTime fechaFinH, EveHoraoperacionDTO regOriginal, bool esFicticio)
        {
            //puede darse el caso de duraciones igual a cero porque la hora de operación puede iniciar / terminar en múltiplo de 15 y usan rango aproximados
            int totalMinuto = (int)(fechaFinH - fechaIniH).TotalMinutes;
            totalMinuto = totalMinuto > 0 ? totalMinuto : 0;
            if (fechaFinH == fechaIniH) totalMinuto = 1; //minuto simbolico para porrateo

            if (esFicticio) totalMinuto = 0;

            var regH = (EveHoraoperacionDTO)regOriginal.Clone();
            regH.FechaProceso = fechaProceso;
            regH.HoraIni48 = fechaIniH;
            regH.HoraFin48 = fechaFinH;
            regH.HIni48 = h;
            regH.HFin48 = h;
            regH.TotalMinuto = totalMinuto;
            regH.Es30MinTieneHo = !esFicticio;

            return regH;
        }

        /// <summary>
        /// Obtener posición de la H inicial y la H final para información EMS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="hIni48"></param>
        /// <param name="hFin48"></param>
        public static void GetHoraIniFin48Despacho(DateTime fecha, DateTime dt1, DateTime dt2, out int hIni48, out int hFin48)
        {
            if (dt1 < fecha) dt1 = fecha;
            if (dt2 > fecha.AddDays(1)) dt2 = fecha.AddDays(1);

            int hi = (int)dt1.TimeOfDay.TotalMinutes / 30;
            int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
            int minhi = (int)dt1.TimeOfDay.TotalMinutes % 30;

            hi = hi < 48 ? hi + (minhi == 0 ? 0 : 1) : hi; //si la hora inicia a las 10:04 entonces se tiene informacion a las 10:30
            hf = hf < 48 ? hf : 48; //Para Despacho es distinto a Medidores, debe tener hora fin multipo de 30min sino no se le suma 1. Por ejemplo si la hora de operacion termina a las 10:15 solo se tiene información de las 10:00

            if (hi > hf && hi == hf + 1) //si la hora de operacion esta dentro de una media hora y dura menos de 30min entonces deben tener el mismo h
                hf = hi;

            hIni48 = hi;
            hFin48 = hf;
        }

        /// <summary>
        /// GetHoraIniFin96Medidores
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="hIni96"></param>
        /// <param name="hFin96"></param>
        public static void GetHoraIniFin96Medidores(DateTime fecha, DateTime dt1, DateTime dt2, out int hIni96, out int hFin96)
        {
            if (dt1 < fecha) dt1 = fecha;
            if (dt2 > fecha.AddDays(1)) dt2 = fecha.AddDays(1);

            int hi = (int)dt1.TimeOfDay.TotalMinutes / 15;
            int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 96 : (int)dt2.TimeOfDay.TotalMinutes / 15;
            int minhi = (int)dt1.TimeOfDay.TotalMinutes % 15;

            hi = hi < 96 ? hi + (minhi == 0 ? 0 : 1) : hi;
            hf = hf < 96 ? hf + 1 : 96;

            if (hi > hf && hi == hf + 1) //si la hora de operacion esta dentro de un cuarto de hora y dura menos de 15min entonces deben tener el mismo h
                hf = hi;

            hIni96 = hi;
            hFin96 = hf;
        }

        private static void GetHoraIniFin96ExtranetMedidores(DateTime fecha, DateTime dt1, DateTime dt2, out int hIni96, out int hFin96)
        {
            if (dt1 < fecha) dt1 = fecha;
            if (dt2 > fecha.AddDays(1)) dt2 = fecha.AddDays(1);

            int hi = (int)dt1.TimeOfDay.TotalMinutes / 15;
            int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 96 : (int)dt2.TimeOfDay.TotalMinutes / 15;
            int minhf = (int)dt2.TimeOfDay.TotalMinutes % 15;

            hi = hi < 96 ? hi + (1) : hi;
            hf = hf < 96 ? hf + (minhf == 0 ? 0 : 1) : 96;

            if (hi > hf && hi == hf + 1) //si la hora de operacion esta dentro de un cuarto de hora y dura menos de 15min entonces deben tener el mismo h
                hf = hi;

            hIni96 = hi;
            hFin96 = hf;
        }

        /// <summary>
        /// Obtener posición de la H inicial y la H final para información EMS
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="hIni48"></param>
        /// <param name="hFin48"></param>
        public static void GetHoraIniFin48Ems(DateTime fecha, DateTime dt1, DateTime dt2, out int hIni48, out int hFin48)
        {
            if (dt1 < fecha) dt1 = fecha;
            if (dt2 > fecha.AddDays(1)) dt2 = fecha.AddDays(1);

            int hi = (int)dt1.TimeOfDay.TotalMinutes / 30;
            int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
            int minhi = (int)dt1.TimeOfDay.TotalMinutes % 30;

            hi = hi < 48 ? hi + (minhi == 0 ? 0 : 1) : hi;
            hf = hf < 48 ? hf + 1 : 48; //Para Despacho es distinto a Medidores, debe tener hora fin multipo de 30min sino no se le suma 1

            hIni48 = hi;
            hFin48 = hf;
        }

        /// <summary>
        /// Obtener posición de la H inicial y la H final para información Consumo Combustible
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="hIni48"></param>
        /// <param name="hFin48"></param>
        public static void GetHoraIniFin48CCC(DateTime fecha, DateTime dt1, DateTime dt2, out int hIni48, out int hFin48)
        {
            if (dt1 < fecha) dt1 = fecha;
            if (dt2 > fecha.AddDays(1)) dt2 = fecha.AddDays(1);

            int hi = (int)dt1.TimeOfDay.TotalMinutes / 30;
            int hf = (dt2.TimeOfDay.TotalMinutes == 0) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
            int minhi = (int)dt1.TimeOfDay.TotalMinutes % 30;

            hi = hi < 48 ? hi + (minhi == 0 ? 0 : 1) : hi;
            if (hi <= 0) hi = 1;
            hf = hf < 48 ? hf : 48;

            hIni48 = hi;
            hFin48 = hf;
        }

        /// <summary>
        /// Obtener posición de la H inicial y la H final para cruce de Hora de operación con programado yupana
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="hIni48"></param>
        /// <param name="hFin48"></param>
        public static void GetHoraIniFin48HoVsYupana(DateTime fecha, DateTime dt1, DateTime dt2, out int hIni48, out int hFin48)
        {
            if (dt1 < fecha) dt1 = fecha;
            if (dt2 > fecha.AddDays(1)) dt2 = fecha.AddDays(1);

            int hi = (int)dt1.TimeOfDay.TotalMinutes / 30;
            int minhi = (int)dt1.TimeOfDay.TotalMinutes % 30;
            int hf = (dt2 == dt1.Date.AddDays(1)) ? 48 : (int)dt2.TimeOfDay.TotalMinutes / 30;
            int minhf = (int)dt2.TimeOfDay.TotalMinutes % 30;

            hi = hi < 48 ? hi + (minhi == 0 ? 0 : 1) : hi; //si la hora inicia a las 10:04 entonces se tiene informacion a las 10:30
            hf = hf < 48 ? hf + (minhf == 0 ? 0 : 1) : 48; //s la hora fin no completa los 30 min, para la validación se asume que completa los 30min

            //dentro de los rangos
            if (hi <= 0) hi = 1;
            if (hf <= 0) hf = 1;
            if (hi > 48) hi = 48;
            if (hf > 48) hf = 48;

            hIni48 = hi;
            hFin48 = hf;
        }

        #endregion

        #region Gráficos de Costos Incrementales

        /// <summary>
        /// Ordenar los Modos de Operación en función de los costos incrementales
        /// </summary>
        /// <param name="fechaCI"></param>
        /// <param name="listaModosBD"></param>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaReporteCostosIncr"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> OrdenarListXCostoIncremental(DateTime fechaCI, List<PrGrupoDTO> listaModosBD, List<EveHoraoperacionDTO> listaHorasOperacion,
                                                 List<ReporteCostoIncrementalDTO> listaReporteCostosIncr)
        {
            //solo cargar los modos que tienen Horas de Operación y tiene centrales activas (esto es necesario para reutilizar la lista en el momento de registrar una nueva hora de operacion)
            List<PrGrupoDTO> listaModoFiltro = new List<PrGrupoDTO>();
            List<int> listaEquipadreHop = listaHorasOperacion.Where(x => x.Grupocodi != null && x.Grupocodi > 0).Select(x => x.Equipadre).Distinct().ToList();
            foreach (var reg in listaModosBD)
            {
                var hopWithModo = listaHorasOperacion.Where(x => x.Grupocodi == reg.Grupocodi && listaEquipadreHop.Contains(reg.Equipadre)).FirstOrDefault();
                if (hopWithModo != null)
                {
                    listaModoFiltro.Add(reg);
                }
            }

            //filtrar
            List<EveHoraoperacionDTO> listaHoModo = listaHorasOperacion.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
            List<int> idsGrupos = listaHoModo.Where(x => x.Grupocodi > 0).Select(x => x.Grupocodi.Value).Distinct().ToList();

            var listaModosOperacionCI = this.OrdenarListXCostoIncrementalByFiltro(listaModoFiltro, listaHorasOperacion, fechaCI, listaReporteCostosIncr);

            return listaModosOperacionCI;
        }

        /// <summary>
        /// Método para ordenar los modos de operacion
        /// </summary>
        /// <param name="listaModos"></param>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="fechaCI"></param>
        /// <param name="listCostosIncr"></param>
        /// <returns></returns>
        private List<PrGrupoDTO> OrdenarListXCostoIncrementalByFiltro(List<PrGrupoDTO> listaModos, List<EveHoraoperacionDTO> listaHorasOperacion,
                                                                DateTime fechaCI, List<ReporteCostoIncrementalDTO> listCostosIncr)
        {
            DateTime fechaCIEms = fechaCI.AddMinutes(-ConstantesHorasOperacion.DelayNumMinDataEms);

            List<EveHoraoperacionDTO> listHoraOperacionTemp = new List<EveHoraoperacionDTO>
            {
                new EveHoraoperacionDTO() { Hophorini = fechaCIEms, Hophorfin = fechaCIEms.Date.AddDays(1) }
            };

            //obtener el H inicio de la data ems
            var listaHO30min = this.ListarHO30min(listHoraOperacionTemp, fechaCIEms);
            var numH = listaHO30min.First().HIni48;
            string horaMin = listaHO30min.First().HoraIni48.Value.AddMinutes(-30).ToString(ConstantesAppServicio.FormatoOnlyHora) + "-" + listaHO30min.First().HoraIni48.Value.ToString(ConstantesAppServicio.FormatoOnlyHora);

            //filtrar horas de operacion
            List<EveHoraoperacionDTO> listaHOModo = listaHorasOperacion.Where(x => x.Hophorini.Value.AddSeconds(-x.Hophorini.Value.Second) <= fechaCI.AddSeconds(1) && fechaCI.AddSeconds(59) <= x.Hophorfin.Value && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList().OrderByDescending(x => x.Hophorini).ToList();

            //separa modos que tienen horas de operación para la fechaCI
            var listaGrupocodiHOCI = listaHOModo.Select(x => x.Grupocodi).ToList();
            var lisModosHO = listaModos.Where(x => listaGrupocodiHOCI.Contains(x.Grupocodi)).ToList();

            if (listaHorasOperacion.Find(x => x.Hopcodi == 416425) != null)
            { }

            //iterar modos
            List<PrGrupoDTO> listaModosOrdenados = new List<PrGrupoDTO>();
            foreach (var modo in lisModosHO)
            {
                if (modo.Grupocodi == 296)
                { }
                //saca la hora operación del modo
                EveHoraoperacionDTO hoModo = listaHOModo.Find(x => x.Grupocodi == modo.Grupocodi);
                modo.FlagEncendido = ConstantesHorasOperacion.FlagModoOperacionEncendido;
                modo.Hopcodi = hoModo.Hopcodi;
                modo.HoraMin = horaMin;
                modo.EmprNomb = hoModo.Emprnomb;

                var listaHOUnidades = hoModo.ListaHoUnidad ?? new List<EveHoUnidadDTO>();

                decimal? valorAcum = null;
                modo.Potencia = valorAcum;

                //comprara con costo incremental
                if (modo.Potencia != null && modo.Potencia > 0)
                {
                    var costoInc = listCostosIncr.Find(x => x.Grupocodi == modo.Grupocodi);

                    if (costoInc != null)
                        this.ConsultarTramoPotencia(costoInc, modo);
                    else
                    {
                        modo.CIncremental = 1000000;
                        modo.Comentario = " No tiene Costo Incremental registrado";
                    }
                }
                else
                {
                    //no tiene potencia
                    modo.Potencia = null;

                    var costoInc = listCostosIncr.Find(x => x.Grupocodi == modo.Grupocodi);

                    if (costoInc != null)
                    {
                        //a la funcion se le pasa un costo ficticio para obtener el tramo con mayor potencia 
                        modo.Potencia = 1000000;
                        this.ConsultarTramoPotencia(costoInc, modo);
                        modo.Potencia = null;
                    }
                    else
                    {
                        modo.CIncremental = 1000000;
                        modo.Comentario = " No tiene Costo Incremental registrado";
                    }
                }
            }

            lisModosHO = lisModosHO.OrderBy(x => x.CIncremental).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Gruponomb).ToList();
            listaModosOrdenados.AddRange(lisModosHO);

            //iterar los restantes
            var lisModosRestantes = listaModos.Where(x => !listaGrupocodiHOCI.Contains(x.Grupocodi)).ToList();
            foreach (var modo in lisModosRestantes)
            {
                var costoInc = listCostosIncr.Find(x => x.Grupocodi == modo.Grupocodi);

                if (costoInc != null)
                {
                    //a la funcion se le pasa un costo ficticio para obtener el tramo con mayor potencia 
                    modo.Potencia = 1000000;
                    this.ConsultarTramoPotencia(costoInc, modo);
                    modo.Potencia = null;
                }
                else
                {
                    modo.CIncremental = 1000000;
                    modo.Comentario = " No tiene Costo Incremental registrado";
                }
            }

            lisModosRestantes = lisModosRestantes.OrderBy(x => x.CIncremental).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Gruponomb).ToList();

            //agregar los modos restantes
            listaModosOrdenados.AddRange(lisModosRestantes);

            //Formatear decimales
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            nfi2.NumberDecimalSeparator = ".";
            NumberFormatInfo nfi1 = UtilAnexoAPR5.GenerarNumberFormatInfo1();
            nfi1.NumberDecimalSeparator = ".";

            foreach (var reg in listaModosOrdenados)
            {
                if (reg.Potencia >= 0) reg.PotenciaFormateado = MathHelper.TruncateDecimal(reg.Potencia.Value, 2).ToString("N", nfi2);
                if (reg.CIncremental >= 0) reg.CIncrementalFormateado = MathHelper.TruncateDouble(reg.CIncremental, 2).ToString("N", nfi2);
                if (reg.CVariable >= 0) reg.CVariableFormateado = MathHelper.TruncateDouble(reg.CVariable, 2).ToString("N", nfi2);

                // Para el TAB Costo CV
                if (reg.CIncremental1 >= 0) reg.CIncremental1Formateado = MathHelper.TruncateDouble(reg.CIncremental1, 2).ToString("N", nfi2);
                if (reg.CIncremental2 >= 0) reg.CIncremental2Formateado = MathHelper.TruncateDouble(reg.CIncremental2, 2).ToString("N", nfi2);
                if (reg.CVariable1 >= 0) reg.CVariable1Formateado = MathHelper.TruncateDouble(reg.CVariable1, 2).ToString("N", nfi2);
            }

            return listaModosOrdenados;
        }

        /// <summary>
        /// OrdenarModoXData15min
        /// </summary>
        /// <param name="listaModos"></param>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaDespachoXFecha"></param>
        /// <param name="fechaCI"></param>
        /// <param name="listCostosIncr"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> OrdenarModoXData15min(List<PrGrupoDTO> listaModos, List<EveHoraoperacionDTO> listaHorasOperacion, List<MeMedicion96DTO> listaDespachoXFecha
            , DateTime fechaCI, List<ReporteCostoIncrementalDTO> listCostosIncr)
        {
            List<EveHoraoperacionDTO> listHoraOperacionTemp = new List<EveHoraoperacionDTO>
            {
                new EveHoraoperacionDTO() { Hophorini = fechaCI, Hophorfin = fechaCI.Date.AddDays(1) }
            };

            //obtener el H inicio de la data ems
            var listaHO15min = this.ListarHO15min(listHoraOperacionTemp, fechaCI);
            var numH = listaHO15min.First().HIni96;
            string horaMin = listaHO15min.First().HoraIni96.Value.AddMinutes(-15).ToString(ConstantesAppServicio.FormatoOnlyHora) + "-" + listaHO15min.First().HoraIni96.Value.ToString(ConstantesAppServicio.FormatoOnlyHora);

            //filtrar horas de operacion
            List<EveHoraoperacionDTO> listaHOModo = listaHorasOperacion.Where(x => x.Hophorini.Value.AddSeconds(-x.Hophorini.Value.Second) <= fechaCI.AddSeconds(1) && fechaCI.AddSeconds(59) <= x.Hophorfin.Value && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList().OrderByDescending(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHorasOperacion.Where(x => x.Hophorini.Value.AddSeconds(-x.Hophorini.Value.Second) <= fechaCI.AddSeconds(1) && fechaCI.AddSeconds(59) <= x.Hophorfin.Value && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList().OrderByDescending(x => x.Hophorini).ToList();

            //separa modos que tienen horas de operación para la fechaCI
            var listaGrupocodiHOCI = listaHOModo.Select(x => x.Grupocodi).ToList();
            var lisModosHO = listaModos.Where(x => listaGrupocodiHOCI.Contains(x.Grupocodi)).ToList();

            //iterar modos
            List<PrGrupoDTO> listaModosOrdenados = new List<PrGrupoDTO>();
            foreach (var modo in lisModosHO)
            {
                if (modo.Grupocodi == 291)
                { }
                //saca la hora operación del modo
                EveHoraoperacionDTO hoModo = listaHOModo.Find(x => x.Grupocodi == modo.Grupocodi);
                modo.FlagEncendido = ConstantesHorasOperacion.FlagModoOperacionEncendido;
                modo.Hopcodi = hoModo.Hopcodi;
                modo.HoraMin = horaMin;

                List<EveHoraoperacionDTO> listaHOUnidades = listaHOUnidad.Where(x => x.Hopcodipadre == hoModo.Hopcodi).ToList();

                decimal? valorAcum = null;

                foreach (var Unidad in listaHOUnidades)
                {
                    var unidadPotencia = listaDespachoXFecha.Find(x => x.Equicodi == Unidad.Equicodi);
                    if (unidadPotencia != null)
                    {
                        decimal? valor = numH != 0 ? (decimal?)unidadPotencia.GetType().GetProperty(ConstantesAppServicio.CaracterH + numH.ToString()).GetValue(unidadPotencia, null) : null;
                        valorAcum = valor != null ? valorAcum.GetValueOrDefault(0) + valor : valorAcum;
                    }
                }
                modo.Potencia = valorAcum;

                //comprara con costo incremental
                if (modo.Potencia != null && modo.Potencia > 0)
                {
                    var costoInc = listCostosIncr.Find(x => x.Grupocodi == modo.Grupocodi);

                    if (costoInc != null)
                        this.ConsultarTramoPotencia(costoInc, modo);
                    else
                    {
                        modo.CIncremental = 1000000;
                        modo.Comentario = " No tiene Costo Incremental registrado";
                    }
                }
                else
                {
                    //no tiene potencia
                    modo.Potencia = null;

                    var costoInc = listCostosIncr.Find(x => x.Grupocodi == modo.Grupocodi);

                    if (costoInc != null)
                    {
                        //a la funcion se le pasa un costo ficticio para obtener el tramo con mayor potencia 
                        modo.Potencia = 1000000;
                        this.ConsultarTramoPotencia(costoInc, modo);
                        modo.Potencia = null;
                    }
                    else
                    {
                        modo.CIncremental = 1000000;
                        modo.Comentario = " No tiene Costo Incremental registrado";
                    }
                }
            }

            lisModosHO = lisModosHO.OrderBy(x => x.CIncremental).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Gruponomb).ToList();
            listaModosOrdenados.AddRange(lisModosHO);

            //iterar los restantes
            var lisModosRestantes = listaModos.Where(x => !listaGrupocodiHOCI.Contains(x.Grupocodi)).ToList();
            foreach (var modo in lisModosRestantes)
            {
                modo.FlagEncendido = 0;
                var costoInc = listCostosIncr.Find(x => x.Grupocodi == modo.Grupocodi);

                if (costoInc != null)
                {
                    //a la funcion se le pasa un costo ficticio para obtener el tramo con mayor potencia 
                    modo.Potencia = 1000000;
                    this.ConsultarTramoPotencia(costoInc, modo);
                    modo.Potencia = null;
                }
                else
                {
                    modo.CIncremental = 1000000;
                    modo.Comentario = " No tiene Costo Incremental registrado";
                }
            }

            lisModosRestantes = lisModosRestantes.OrderBy(x => x.CIncremental).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Gruponomb).ToList();

            //agregar los modos restantes
            listaModosOrdenados.AddRange(lisModosRestantes);

            return listaModosOrdenados;
        }

        /// <summary>
        /// Calcular tramo de cosotos incrementales según la potencia
        /// </summary>
        /// <param name="costoInc"></param>
        /// <param name="modo"></param>
        public void ConsultarTramoPotencia(ReporteCostoIncrementalDTO costoInc, PrGrupoDTO modo)
        {
            modo.CIncremental = costoInc.Cincrem1;
            modo.CVariable = string.IsNullOrEmpty(costoInc.CV) ? 0 : (Convert.ToDouble(costoInc.CV) * 1000);
            modo.NumTramo = 1;
            modo.Tramo = costoInc.Tramo1;

            if (modo.Potencia >= costoInc.Pe1 && costoInc.Pe2 > 0)
            {
                modo.CIncremental = costoInc.Cincrem1;
                modo.NumTramo = 1;
                modo.Tramo = costoInc.Tramo1;
                modo.PeIni = costoInc.Pe1;
                modo.PeFin = costoInc.Pe2;
            }
            if (modo.Potencia >= costoInc.Pe2 && costoInc.Pe3 > 0)
            {
                modo.CIncremental = costoInc.Cincrem2;
                modo.NumTramo = 2;
                modo.Tramo = costoInc.Tramo2;
                modo.PeIni = costoInc.Pe2;
                modo.PeFin = costoInc.Pe3;
            }
            if (modo.Potencia >= costoInc.Pe3 && costoInc.Pe4 > 0)
            {
                modo.CIncremental = costoInc.Cincrem3;
                modo.NumTramo = 3;
                modo.Tramo = costoInc.Tramo3;
                modo.PeIni = costoInc.Pe3;
                modo.PeFin = costoInc.Pe4;
            }

            // Para el TAB Costo CV
            modo.CVariable1 = string.IsNullOrEmpty(costoInc.CV) ? 0 : (Convert.ToDouble(costoInc.CV) * 1000);
            modo.Tramo1 = costoInc.Tramo1;
            modo.Tramo2 = costoInc.Tramo2;
            modo.CIncremental1 = costoInc.Cincrem1;
            modo.CIncremental2 = costoInc.Cincrem2;
        }

        /// <summary>
        /// Método para Verificar la existencia de centrales mas caras que aún no se bajaron a mínima Carga
        /// </summary>
        /// <param name="listaHo"></param>
        /// <param name="fechaReg"></param>
        /// <param name="fechaCI"></param>
        /// <param name="listaHoOut"></param>
        /// <param name="listaValCentralCaras"></param>
        public void VerificarCentralesMasCaras(List<EveHoraoperacionDTO> listaHo, DateTime fechaReg, DateTime fechaCI
            , out List<EveHoraoperacionDTO> listaHoOut, out List<ResultadoValidacionAplicativo> listaValCentralCaras)
        {
            listaHoOut = new List<EveHoraoperacionDTO>();
            listaValCentralCaras = new List<ResultadoValidacionAplicativo>();

            if (fechaReg.Date == DateTime.Now.Date) //solo para tiempo real
            {
                //Tratar horas de operacion de la interfaz grafica  -1: eliminar, 0:lectura, 1:crear, 2:actualizar  
                List<EveHoraoperacionDTO> listaHoValidos = listaHo.Where(x => x.OpcionCrud != -1).ToList(); // Eliminado lógico
                List<EveHoraoperacionDTO> listaHoModo = listaHoValidos.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo
                                            && (x.Subcausacodi == ConstantesSubcausaEvento.SubcausaAMinimaCarga || x.Subcausacodi == ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia)).ToList();

                //verificar si la hora de operacion (a registrar o modificar) es de Minima Carga
                List<EveHoraoperacionDTO> listaHoModoMinCarga = listaHoModo.Where(x => x.Subcausacodi == ConstantesSubcausaEvento.SubcausaAMinimaCarga).ToList();
                if (listaHoModoMinCarga.Count() > 0)
                {
                    List<EveHoraoperacionDTO> listaHoData = new List<EveHoraoperacionDTO>();
                    foreach (var regHo in listaHoModo)
                    {
                        regHo.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
                        listaHoData.Add(regHo);
                    }

                    //Obtener Costos Incrementales
                    List<PrGrupoDTO> listaModos = new List<PrGrupoDTO>();
                    List<int> idsGrupos = listaHoModo.Where(x => x.Grupocodi > 0).Select(x => x.Grupocodi.Value).Distinct().ToList();
                    foreach (var grupocodi in idsGrupos)
                        listaModos.Add(new PrGrupoDTO() { Grupocodi = grupocodi });

                    List<ReporteCostoIncrementalDTO> listCostosIncr = servGrDespach.ListarTodosCI(idsGrupos, fechaReg);
                    //List<ReporteCostoIncrementalDTO> listCostosIncr = servGrDespach.ListarTodosCI(fechaReg);
                    listaModos = this.OrdenarListXCostoIncrementalByFiltro(listaModos, listaHoData, fechaCI, listCostosIncr);

                    this.ListarValidacionCostoIncremental(listaModos, listaHoData, out listaHoOut, out listaValCentralCaras);
                }
            }
        }

        /// <summary>
        /// Validación para determinar horas de operación caras
        /// </summary>
        /// <param name="listaModos"></param>
        /// <param name="listaHoData"></param>
        /// <param name="listaHoOut"></param>
        /// <param name="listaValCentralCaras"></param>
        private void ListarValidacionCostoIncremental(List<PrGrupoDTO> listaModos, List<EveHoraoperacionDTO> listaHoData
            , out List<EveHoraoperacionDTO> listaHoOut, out List<ResultadoValidacionAplicativo> listaValCentralCaras)
        {
            listaModos = listaModos.Where(x => x.FlagEncendido == ConstantesHorasOperacion.FlagModoOperacionEncendido).ToList();

            //asignar y ordenar costos incrementales
            List<int> listaHopcodi = new List<int>();
            foreach (var regModo in listaModos)
            {
                var hop = listaHoData.Find(x => x.Hopcodi == regModo.Hopcodi); //hora de operacion del modo en la fecha ci
                if (hop.Subcausacodi == ConstantesSubcausaEvento.SubcausaAMinimaCarga)
                {
                    var subListaModoMasCaras = listaModos.Where(x => x.CIncremental > regModo.CIncremental).ToList();
                    foreach (var regModoCara in subListaModoMasCaras)
                    {
                        var hopCara = listaHoData.Find(x => x.Hopcodi == regModoCara.Hopcodi);

                        //La validación es sobre horas de operación calificadas como Potencia y no son Sistemas aislados
                        if (hopCara.Subcausacodi == ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia && hopCara.Hopsaislado != ConstantesHorasOperacion.CheckSistemaAislado)
                            listaHopcodi.Add(hopCara.Hopcodi);
                    }
                }
            }
            listaHopcodi = listaHopcodi.Distinct().ToList();

            ////////////////////////////////////////////////////////////////////////////////////////////////
            listaValCentralCaras = new List<ResultadoValidacionAplicativo>();
            foreach (var hopcodi in listaHopcodi)
            {
                var hop = listaHoData.Find(x => x.Hopcodi == hopcodi);

                hop.HophoriniDesc = hop.Hophorini != null ? hop.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                hop.HophorfinDesc = hop.Hophorfin != null ? hop.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                hop.HophorordarranqDesc = hop.Hophorordarranq != null ? hop.Hophorordarranq.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                hop.HophorparadaDesc = hop.Hophorparada != null ? hop.Hophorparada.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;

                ResultadoValidacionAplicativo regNuevo = new ResultadoValidacionAplicativo
                {
                    FechaIni = hop.Hophorini.Value,
                    FechaFin = hop.Hophorfin.Value,
                    FechaIniDesc = hop.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHHmmss),
                    FechaFinDesc = hop.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHHmmss),
                    HoraOperacion = hop,
                    //regNuevo.Hopcodi = hop.Hopcodi;
                    Hopcodipadre = hop.Hopcodi,

                    Emprnomb = hop.Emprnomb,
                    Central = hop.Central,
                    ModoOp = hop.Gruponomb,
                    Subcausadesc = hop.Subcausadesc,
                    Descripcion = !string.IsNullOrEmpty(hop.Hopdesc) ? hop.Hopdesc : string.Empty
                };

                listaValCentralCaras.Add(regNuevo);
            }

            listaHoOut = new List<EveHoraoperacionDTO>();
            List<int> listaHopcodiOut = listaValCentralCaras.Select(x => x.HoraOperacion.Hopcodi).Distinct().ToList();
            foreach (var hopcodi in listaHopcodiOut)
            {
                listaHoOut.Add(listaValCentralCaras.Where(x => x.HoraOperacion.Hopcodi == hopcodi).ToList().First().HoraOperacion);
            }
        }

        #endregion

        #region Reporte Horas Operacion IEOD

        /// <summary>
        /// Reporte de HOP por empresa y mes para PR15
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<ReporteHoraoperacionDTO> ListarReporteHOP(int idEmpresa, DateTime fechaIni, DateTime fechaFin)
        {
            List<ReporteHoraoperacionDTO> listaHOP = new List<ReporteHoraoperacionDTO>();
            List<EveHoraoperacionDTO> listaHorasOperacion = new List<EveHoraoperacionDTO>();
            List<EqEquipoDTO> listaUnidades = new List<EqEquipoDTO>();

            //tipo de centrales
            var listaTC = this.ListarCentralesXEmpresaGener(idEmpresa);
            List<EqEquipoDTO> listaTCentral = listaTC.GroupBy(x => new { x.Famcodipadre, x.Famnomb })
                                .Select(y => new EqEquipoDTO() { Famcodi = y.Key.Famcodipadre, Famnomb = y.Key.Famnomb })
                                .Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica).ToList();

            foreach (var tc in listaTCentral)
            {
                //centrales
                var listaC = this.ListarCentralesXEmpresaGener(idEmpresa).Where(x => x.Famcodipadre == tc.Famcodi).ToList();
                List<EqEquipoDTO> listaCentrales = listaC.GroupBy(x => new { x.Codipadre, x.Nombrecentral })
                                .Select(y => new EqEquipoDTO() { Equicodi = y.Key.Codipadre, Equinomb = y.Key.Nombrecentral }).ToList();

                switch (tc.Famcodi)
                {
                    case ConstantesHorasOperacion.IdTipoHidraulica:
                        List<EqEquipoDTO> listaGrupo = new List<EqEquipoDTO>();
                        if (listaCentrales.Count > 0)
                        {
                            //agrupamos los grupos de todas las centrales en una sola lista
                            foreach (var entity in listaCentrales)
                            {
                                var listaAux = this.ListarGruposxCentralGEN(idEmpresa, entity.Equicodi, ConstantesHorasOperacion.IdGeneradorHidroelectrico);
                                if (listaAux.Count > 0)
                                {
                                    foreach (var obj in listaAux)
                                    {
                                        listaGrupo.Add(obj);
                                    }
                                }
                            }
                        }

                        foreach (var central in listaCentrales)
                        {
                            listaHorasOperacion = this.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaIni, fechaFin.AddDays(1), central.Equicodi);

                            foreach (var grupo in listaGrupo)
                            {
                                var listaHOPxGrupo = listaHorasOperacion.Where(x => x.Equicodi == grupo.Equicodi).ToList();

                                //generar reporte HOP
                                for (int i = 0; i < listaHOPxGrupo.Count; i++)
                                {
                                    var regAct = listaHOPxGrupo[i];

                                    ReporteHoraoperacionDTO r = new ReporteHoraoperacionDTO
                                    {
                                        IdCentral = central.Equicodi,
                                        Central = central.Equinomb,
                                        IdModoOpGrupo = grupo.Equicodi,
                                        ModoOpGrupo = grupo.Equinomb,
                                        Hophorini = regAct.Hophorini.Value,
                                        Hophorfin = regAct.Hophorfin.Value
                                    };

                                    if (i + 1 < listaHOPxGrupo.Count)//tiene siguiente
                                    {
                                        //buscar fin
                                        bool fin = false;
                                        for (int j = i + 1; j < listaHOPxGrupo.Count && !fin; j++)
                                        {
                                            var regSig = listaHOPxGrupo[j];

                                            if (regAct.Hophorfin.Value == regSig.Hophorini.Value)
                                            {
                                                r.Hophorfin = regSig.Hophorfin.Value;
                                                i++;
                                            }
                                            else
                                            {
                                                fin = true;
                                            }

                                            regAct = regSig;
                                        }
                                    }

                                    listaHOP.Add(r);
                                }
                            }
                        }

                        break;
                    case ConstantesHorasOperacion.IdTipoTermica:
                        List<PrGrupoDTO> listaModosOperacion = new List<PrGrupoDTO>();
                        foreach (var central in listaCentrales)
                        {
                            listaHorasOperacion = this.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaIni, fechaFin.AddDays(1), central.Equicodi).OrderBy(x => x.Hophorini).ToList();
                            listaModosOperacion = this.ListarModoOperacionXCentralYEmpresa(central.Equicodi, idEmpresa);
                            listaUnidades = this.ListarGruposxCentralGEN(idEmpresa, central.Equicodi, ConstantesHorasOperacion.IdGeneradorTemoelectrico);

                            foreach (var modo in listaModosOperacion)
                            {
                                var listaHOPxModo = listaHorasOperacion.Where(x => x.Grupocodi == modo.Grupocodi).ToList();
                                //generar reporte HOP                                
                                for (int i = 0; i < listaHOPxModo.Count; i++)
                                {
                                    var regAct = listaHOPxModo[i];
                                    //var unidad = listaHorasOperacion.Where(x => x.Hopcodipadre == regAct.Hopcodi).OrderBy(x => x.Equicodi).FirstOrDefault();

                                    ReporteHoraoperacionDTO r = new ReporteHoraoperacionDTO
                                    {
                                        IdCentral = central.Equicodi,
                                        Central = central.Equinomb,

                                        IdUnidad = regAct.Equicodi ?? 0,
                                        Unidad = regAct.Equiabrev,

                                        IdModoOpGrupo = modo.Grupocodi,
                                        ModoOpGrupo = modo.Gruponomb,
                                        Hophorini = regAct.Hophorini.Value,
                                        Hophorfin = regAct.Hophorfin.Value
                                    };

                                    if (i + 1 < listaHOPxModo.Count)//tiene siguiente
                                    {
                                        //buscar fin
                                        bool fin = false;
                                        for (int j = i + 1; j < listaHOPxModo.Count && !fin; j++)
                                        {
                                            var regSig = listaHOPxModo[j];

                                            if (regAct.Hophorfin.Value == regSig.Hophorini.Value)
                                            {
                                                r.Hophorfin = regSig.Hophorfin.Value;
                                                i++;
                                            }
                                            else
                                            {
                                                fin = true;
                                            }

                                            regAct = regSig;
                                        }
                                    }

                                    listaHOP.Add(r);
                                }
                            }
                        }

                        break;
                    case ConstantesHorasOperacion.IdTipoSolar:
                    case ConstantesHorasOperacion.IdTipoEolica:
                        listaHorasOperacion = this.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaIni, fechaFin.AddDays(1), -1);
                        foreach (var central in listaCentrales)
                        {
                            listaHorasOperacion.AddRange(this.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaIni, fechaFin.AddDays(1), central.Equicodi));
                            listaHorasOperacion = listaHorasOperacion.Where(x => x.Equicodi == central.Equicodi).OrderBy(x => x.Hophorini).ToList();
                            for (int i = 0; i < listaHorasOperacion.Count; i++)
                            {
                                var regAct = listaHorasOperacion[i];

                                ReporteHoraoperacionDTO r = new ReporteHoraoperacionDTO
                                {
                                    IdCentral = central.Equicodi,
                                    Central = central.Equinomb,
                                    Hophorini = regAct.Hophorini.Value,
                                    Hophorfin = regAct.Hophorfin.Value
                                };

                                if (i + 1 < listaHorasOperacion.Count)//tiene siguiente
                                {
                                    //buscar fin
                                    bool fin = false;
                                    for (int j = i + 1; j < listaHorasOperacion.Count && !fin; j++)
                                    {
                                        var regSig = listaHorasOperacion[j];

                                        if (regAct.Hophorfin.Value == regSig.Hophorini.Value)
                                        {
                                            r.Hophorfin = regSig.Hophorfin.Value;
                                            i++;
                                        }
                                        else
                                        {
                                            fin = true;
                                        }

                                        regAct = regSig;
                                    }
                                }

                                listaHOP.Add(r);
                            }
                        }

                        break;
                }

                foreach (var h in listaHOP)
                {
                    h.Central = h.Central.Trim();
                    h.ModoOpGrupo = h.ModoOpGrupo != null ? h.ModoOpGrupo.Trim() : string.Empty;
                    h.Unidad = h.Unidad != null ? h.Unidad.Trim() : string.Empty;
                    h.FechaIni = h.Hophorini.ToString(ConstantesBase.FormatFechaFull);
                    h.FechaFin = h.Hophorfin.ToString(ConstantesBase.FormatFechaFull);
                }

                listaHOP = listaHOP.OrderBy(x => x.Central).ThenBy(x => x.Unidad).ThenBy(x => x.ModoOpGrupo).ThenBy(x => x.FechaIni).ToList();
            }

            return listaHOP;
        }

        /// <summary>
        /// Reporte de Estado Operativo para Excel
        /// </summary>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarReporteEstadoOperativoExcel(List<EveHoraoperacionDTO> listaData)
        {
            List<EveHoraoperacionDTO> listaHorasOperacion = new List<EveHoraoperacionDTO>();

            List<SiEmpresaDTO> listaEmpresa = listaData.GroupBy(x => new { x.Emprcodi, x.Emprnomb })
                .Select(y => new SiEmpresaDTO() { Emprcodi = y.Key.Emprcodi, Emprnomb = y.Key.Emprnomb }).ToList();

            foreach (var empresa in listaEmpresa)
            {
                List<EqAreaDTO> listaArea = listaData.Where(x => x.Emprcodi == empresa.Emprcodi).GroupBy(x => new { x.Areacodi, x.Areanomb })
                                .Select(y => new EqAreaDTO() { Areacodi = y.Key.Areacodi, Areanomb = y.Key.Areanomb }).ToList();

                foreach (var area in listaArea)
                {
                    List<EqEquipoDTO> listaEquipo = listaData.Where(x => x.Emprcodi == empresa.Emprcodi && x.Areacodi == area.Areacodi).GroupBy(x => new { x.Equicodi, x.Equiabrev })
                                    .Select(y => new EqEquipoDTO() { Equicodi = y.Key.Equicodi.Value, Equiabrev = y.Key.Equiabrev }).ToList();

                    foreach (var equipo in listaEquipo)
                    {
                        List<SubCausaEventoDTO> listaCausa = listaData.Where(x => x.Emprcodi == empresa.Emprcodi && x.Areacodi == area.Areacodi && x.Equicodi == equipo.Equicodi).GroupBy(x => new { x.Subcausacodi, x.Subcausadesc })
                                    .Select(y => new SubCausaEventoDTO() { SUBCAUSACODI_ = y.Key.Subcausacodi.Value, SUBCAUSADESC = y.Key.Subcausadesc }).ToList();

                        foreach (var causa in listaCausa)
                        {
                            List<EveHoraoperacionDTO> listaHOPxEquipo = listaData.Where(x => x.Equicodi == equipo.Equicodi && x.Subcausacodi == causa.SUBCAUSACODI_).OrderBy(x => x.Hophorini).ThenBy(x => x.Hophorfin).ToList();

                            for (int i = 0; i < listaHOPxEquipo.Count; i++)
                            {
                                var regAct = listaHOPxEquipo[i];

                                EveHoraoperacionDTO r = new EveHoraoperacionDTO
                                {
                                    Emprcodi = empresa.Emprcodi,
                                    Emprnomb = empresa.Emprnomb,
                                    Areacodi = area.Areacodi,
                                    Areanomb = area.Areanomb,
                                    Equicodi = equipo.Equicodi,
                                    Equiabrev = equipo.Equiabrev,
                                    Hophorini = regAct.Hophorini.Value,
                                    Hophorfin = regAct.Hophorfin.Value,
                                    Subcausacodi = regAct.Subcausacodi,
                                    Subcausadesc = regAct.Subcausadesc
                                };

                                if (i + 1 < listaHOPxEquipo.Count)//tiene siguiente
                                {
                                    //buscar fin
                                    bool fin = false;
                                    for (int j = i + 1; j < listaHOPxEquipo.Count && !fin; j++)
                                    {
                                        var regSig = listaHOPxEquipo[j];

                                        if (regSig.Hophorini.Value <= regAct.Hophorfin.Value && regAct.Hophorfin.Value <= regSig.Hophorfin.Value && regAct.Subcausacodi == regSig.Subcausacodi)
                                        {
                                            r.Hophorfin = regSig.Hophorfin.Value;
                                            i++;
                                        }
                                        else
                                        {
                                            fin = true;
                                        }

                                        regAct = regSig;
                                    }
                                }

                                listaHorasOperacion.Add(r);
                            }
                        }
                    }
                }
            }

            listaHorasOperacion = listaHorasOperacion.OrderBy(x => x.Emprnomb).ThenBy(x => x.Areanomb).ThenBy(x => x.Equiabrev).ThenBy(x => x.Hophorini).ToList();

            return listaHorasOperacion;
        }

        /// <summary>
        /// Listar Horas de Operación por criterios de búsqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="sIdTipoOperacion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <param name="idFiltroEnsayoPe"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetListaHorasOperacionByCriteria(int idEmpresa, string sIdTipoOperacion, DateTime fechaInicial, DateTime fechaFinal, int idTipoCentral, int idCentral, string idFiltroEnsayoPe = "-1")
        {
            idEmpresa = idEmpresa <= 0 ? Int32.Parse(ConstantesHorasOperacion.ParamEmpresaTodos) : idEmpresa;

            List<EveHoraoperacionDTO> listaReporte = new List<EveHoraoperacionDTO>();
            List<EveHoraoperacionDTO> lista = new List<EveHoraoperacionDTO>();

            List<EqFamiliaDTO> listaTipoCentral = this.ListarTipoCentralHOP();

            switch (idTipoCentral)
            {
                case ConstantesHorasOperacion.IdTipoHidraulica:
                    lista = this.ListarHorasOperacxEquiposXEmpXTipoOPxFam(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, ConstantesHorasOperacion.IdTipoHidraulica);
                    listaReporte = this.FormatearListaReporteHOP(idTipoCentral, listaTipoCentral, lista);
                    break;
                case ConstantesHorasOperacion.IdTipoSolar:
                case ConstantesHorasOperacion.IdTipoEolica:
                    lista = this.GetListarHorasOperacxEmpresaxFechaxTipoOPxFam(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, idTipoCentral);
                    listaReporte = this.FormatearListaReporteHOP(idTipoCentral, listaTipoCentral, lista);
                    break;
                case ConstantesHorasOperacion.IdTipoTermica:
                    lista = this.ListarHorasOperacxEquiposXEmpXTipoOPxFam2(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, idCentral);
                    if (idFiltroEnsayoPe != ConstantesAppServicio.ParametroDefecto) lista = lista.Where(x => x.Hopensayope == idFiltroEnsayoPe).ToList();
                    listaReporte = this.FormatearListaReporteHOP(idTipoCentral, listaTipoCentral, lista);
                    break;
                case ConstantesHorasOperacion.IdTipoCentralTodos:

                    //hidro
                    lista = this.ListarHorasOperacxEquiposXEmpXTipoOPxFam(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, ConstantesHorasOperacion.IdTipoHidraulica);
                    lista = this.FormatearListaReporteHOP(ConstantesHorasOperacion.IdTipoHidraulica, listaTipoCentral, lista);
                    listaReporte.AddRange(lista);

                    //solar
                    lista = this.GetListarHorasOperacxEmpresaxFechaxTipoOPxFam(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, ConstantesHorasOperacion.IdTipoSolar);
                    lista = this.FormatearListaReporteHOP(ConstantesHorasOperacion.IdTipoSolar, listaTipoCentral, lista);
                    listaReporte.AddRange(lista);

                    //eolica
                    lista = this.GetListarHorasOperacxEmpresaxFechaxTipoOPxFam(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, ConstantesHorasOperacion.IdTipoEolica);
                    lista = this.FormatearListaReporteHOP(ConstantesHorasOperacion.IdTipoEolica, listaTipoCentral, lista);
                    listaReporte.AddRange(lista);

                    //termo
                    lista = this.ListarHorasOperacxEquiposXEmpXTipoOPxFam2(idEmpresa, fechaInicial, fechaFinal, sIdTipoOperacion, idCentral);
                    if (idFiltroEnsayoPe != ConstantesAppServicio.ParametroDefecto) lista = lista.Where(x => x.Hopensayope == idFiltroEnsayoPe).ToList();
                    lista = this.FormatearListaReporteHOP(ConstantesHorasOperacion.IdTipoTermica, listaTipoCentral, lista);
                    listaReporte.AddRange(lista);

                    break;
            }

            return listaReporte.OrderBy(x => x.Emprnomb).ThenBy(x => x.PadreNombre).ThenBy(x => x.Equiabrev).ThenBy(x => x.Hophorini).ToList();
        }

        /// <summary>
        /// Set desglose a objeto Hora de Operación
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="listaReporte"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> GetListaHorasOperacionByCriteriaWithDesglose(DateTime fechaInicial, DateTime fechaFinal, List<EveHoraoperacionDTO> listaReporte)
        {
            List<EveHoEquiporelDTO> listaDesgloseRango = this.GetByCriteriaEveHoEquiporelGroupByHoPadre(fechaInicial, fechaFinal);

            foreach (var regHo in listaReporte)
            {
                regHo.ListaDesglose = listaDesgloseRango.Where(x => x.Hopcodi == regHo.Hopcodi).ToList();
            }

            return listaReporte;
        }

        /// <summary>
        /// Formatear la lista de hop
        /// </summary>
        /// <param name="idTipoCentral"></param>
        /// <param name="listaTipoCentral"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> FormatearListaReporteHOP(int idTipoCentral, List<EqFamiliaDTO> listaTipoCentral, List<EveHoraoperacionDTO> lista)
        {
            foreach (var reg in lista)
            {
                reg.PadreNombre = reg.Famcodi == ConstantesHorasOperacion.IdTipoSolar || reg.Famcodi == ConstantesHorasOperacion.IdTipoEolica ? reg.EquipoNombre : reg.PadreNombre;
                reg.Central = reg.Famcodi == ConstantesHorasOperacion.IdTipoSolar || reg.Famcodi == ConstantesHorasOperacion.IdTipoEolica ? "CENTRAL" : string.Empty;

                reg.Equiabrev = (reg.Equiabrev ?? "").Trim();
                reg.Emprabrev = (reg.Emprabrev ?? "").Trim();
                reg.Areanomb = (reg.Areanomb ?? "").Trim();

                reg.Famcodi = idTipoCentral;
                reg.Famnomb = listaTipoCentral.Find(x => x.Famcodi == idTipoCentral).Famnomb;

                FormatearDescripcionesHop(reg);
            }

            return lista;
        }

        /// <summary>
        /// FormatearDescripcionesHop
        /// </summary>
        /// <param name="reg"></param>
        public void FormatearDescripcionesHop(EveHoraoperacionDTO reg)
        {
            reg.HophoriniDesc = reg.Hophorini != null ? reg.Hophorini.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            reg.HophorfinDesc = reg.Hophorfin != null ? reg.Hophorfin.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            reg.HophorordarranqDesc = reg.Hophorordarranq != null ? reg.Hophorordarranq.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;
            reg.HophorparadaDesc = reg.Hophorparada != null ? reg.Hophorparada.Value.ToString(ConstantesBase.FormatoFechaFullBase) : string.Empty;

            reg.HopensayopeDesc = GetDescripcionEnsayoPe(reg.Hopensayope);
            reg.HopensayopminDesc = GetDescripcionEnsayoPmin(reg.Hopensayopmin);
            reg.HopsaisladoDesc = GetDescripcionSistemaAislado(reg.Hopsaislado);

            reg.LastdateDesc = reg.Lastdate != null ? reg.Lastdate.Value.ToString(ConstantesAppServicio.FormatoFechaHora) : string.Empty;
            reg.Hopcausacodi = reg.Hopcausacodi.GetValueOrDefault(ConstantesMotivoOperacionForzada.CodigoNoDefinido);
            reg.HopcausacodiDesc = GetDescripcionMotivoOperacionForzada(reg.Hopcausacodi.Value);
            reg.HoplimtransDesc = GetDescripcionLimTransm(reg.Hoplimtrans);
            reg.HopcompordarrqDesc = reg.Hopcompordarrq == ConstantesAppServicio.SI ? ConstantesAppServicio.SI : string.Empty;
            reg.HopcompordpardDesc = reg.Hopcompordpard == ConstantesAppServicio.SI ? ConstantesAppServicio.SI : string.Empty;

            reg.Subcausadesc = reg.Subcausadesc != null ? reg.Subcausadesc.Trim() : string.Empty;
        }

        /// <summary>
        /// Reporte HTML de HORAS DE OPERACIÓN
        /// </summary>
        /// <param name="url"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="sIdTipoOperacion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <param name="checkCompensar"></param>
        /// <param name="idFiltroEnsayoPe"></param>
        /// <param name="idFiltroEnsayoPMin"></param>
        /// <returns></returns>
        public string ListarReporteHOPHtml(string url, int idEmpresa, string sIdTipoOperacion, DateTime fechaInicial, DateTime fechaFinal, int idTipoCentral, int idCentral, bool checkCompensar, string idFiltroEnsayoPe, string idFiltroEnsayoPMin)
        {
            StringBuilder str = new StringBuilder();

            //
            List<EveHoraoperacionDTO> listaData = this.GetListaHorasOperacionByCriteria(idEmpresa, sIdTipoOperacion, fechaInicial, fechaFinal, idTipoCentral, idCentral);
            if (idFiltroEnsayoPe != ConstantesAppServicio.ParametroDefecto) listaData = listaData.Where(x => x.Hopensayope == idFiltroEnsayoPe).ToList();
            if (idFiltroEnsayoPMin != ConstantesAppServicio.ParametroDefecto) listaData = listaData.Where(x => x.Hopensayopmin == idFiltroEnsayoPMin).ToList();
            List<EqFamiliaDTO> listaTipoCentral = listaData.GroupBy(x => new { FamCodi = x.Famcodi, FamNomb = x.Famnomb }).Select(x => new EqFamiliaDTO() { Famcodi = x.Key.FamCodi, Famnomb = x.Key.FamNomb }).ToList();
            bool tieneVarioTC = listaTipoCentral.Count >= 2;
            string descModoGrupo = listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null && listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación - Grupo" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                ? "Modo de Operación" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null
                ? "Grupo" : string.Empty;

            //
            str.Append("<table class='pretty tabla-adicional' border='0' cellspacing='0' width='100%' id='tabla'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style=''>Código</th>");
            str.Append("<th style=''>Empresa</th>");
            if (tieneVarioTC)
            {
                str.Append("<th style=''>Tipo Central</th>");
            }
            str.Append("<th style=''>Central</th>");
            str.Append("<th style=''>Grupo</th>");
            if (descModoGrupo != string.Empty)
            {
                str.AppendFormat("<th style=''>{0}</th>", descModoGrupo);
            }
            str.Append("<th style=''>Inicio</th>");
            str.Append("<th style=''>Final</th>");
            str.Append("<th style=''>O. Arranque</th>");
            str.Append("<th style=''>O. Parada</th>");
            str.Append("<th style=''>Tipo de Operación</th>");
            str.Append("<th style=''>Ensayo de Pe</th>");
            str.Append("<th style=''>Ensayo de PMin</th>");
            str.Append("<th style=''>Sistema</th>");
            str.Append("<th style=''>Lim. Transm.</th>");
            str.Append("<th style=''>Causa</th>");
            str.Append("<th style=''>Observación</th>");
            if (checkCompensar)
            {
                str.Append("<th style=''>Comp. Arrq</th>");
                str.Append("<th style=''>Comp. Pard</th>");
            }
            str.Append("<th style=''>Usuario modificación</th>");
            str.Append("<th style=''>Fecha modificación</th>");
            str.Append("<th style=''></th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo

            if (listaData.Count > 0)
            {
                foreach (var reg in listaData)
                {
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Hopcodi);
                    str.AppendFormat("<td class=''>{0}</td>", reg.Emprnomb);
                    if (tieneVarioTC)
                    {
                        str.AppendFormat("<td class=''>{0}</td>", reg.Famnomb);
                    }
                    str.AppendFormat("<td class=''>{0}</td>", reg.PadreNombre);
                    str.AppendFormat("<td class=''>{0}</td>", reg.Equiabrev);
                    if (descModoGrupo != string.Empty)
                    {
                        str.AppendFormat("<td class=''>{0}</td>", reg.EquipoNombre);
                    }
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.HophoriniDesc); //paralelo
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.HophorfinDesc); // fin de paralelo
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.HophorordarranqDesc); // orden de arranque
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.HophorparadaDesc); // orden de parada
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.Subcausadesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.HopensayopeDesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.HopensayopminDesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.HopsaisladoDesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.HoplimtransDesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.HopcausacodiDesc);
                    str.AppendFormat("<td class='' style=''>{0}</td>", reg.Hopdesc);
                    if (checkCompensar)
                    {
                        str.AppendFormat("<td class='' style='text-align: center'>{0}</th>", reg.HopcompordarrqDesc);
                        str.AppendFormat("<td class='' style='text-align: center'>{0}</th>", reg.HopcompordpardDesc);
                    }
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.Lastuser);
                    str.AppendFormat("<td class='' style='text-align: center'>{0}</td>", reg.LastdateDesc);
                    str.AppendFormat("<td>");
                    str.AppendFormat("<a class='ver_hop' href='JavaScript:verHoraOperacion(" + reg.Hopcodi + "," + reg.Famcodi + "," + reg.Emprcodi + "," + reg.Equipadre + ");' style='margin-right: 4px;'><img src='" + url + "Content/Images/btn-open.png' alt='Ver registro' title='Ver registro' /></a>");
                    str.AppendFormat("</td>");

                    str.Append("</tr>");
                }
            }

            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        /// <summary>
        /// Genera Archivo excel  de HOP y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="checkCompensar"></param>
        /// <param name="checkEnsayoPe"></param>
        /// <param name="checkEnsayoPMin"></param>
        /// <returns></returns>
        public string GenerarFileExcelReporteHOP(List<EveHoraoperacionDTO> lista, DateTime fechaInicial, DateTime fechaFinal, bool checkCompensar, bool checkEnsayoPe, bool checkEnsayoPMin)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet wsHist = xlPackage.Workbook.Worksheets.Add(ConstantesHorasOperacion.NombreHojaHOP);
                wsHist.View.ShowGridLines = false;

                var data = lista;
                string fechaIniFiltro = fechaInicial.ToString(ConstantesAppServicio.FormatoFecha);
                string fechaFinFiltro = fechaFinal.ToString(ConstantesAppServicio.FormatoFecha);

                #region Hoja histórico

                int row = 7;
                int column = 2;

                int rowTitulo = 2;
                wsHist.Cells[rowTitulo, column + 2].Value = ConstantesHorasOperacion.TituloHojaHOP;
                wsHist.Cells[rowTitulo, column + 2].Style.Font.SetFromFont(new Font("Calibri", 14));
                wsHist.Cells[rowTitulo, column + 2].Style.Font.Bold = true;

                #region filtros
                int rowIniFiltro = row + 1;
                /*wsHist.Cells[row, column].Value = "Empresa:";
                wsHist.Cells[row, column + 1].Value = empresaFiltro;*/

                row++;
                wsHist.Cells[row, column].Value = "Fecha Inicio:";
                wsHist.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, column + 1].Value = fechaIniFiltro;

                row++;
                wsHist.Cells[row, column].Value = "Fecha Fin:";
                wsHist.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, column + 1].Value = fechaFinFiltro;

                using (var range = wsHist.Cells[rowIniFiltro, column, row, column])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Font.Bold = true;
                }

                #endregion

                row += 2;
                #region cabecera

                List<EqFamiliaDTO> listaTipoCentral = lista.GroupBy(x => new { FamCodi = x.Famcodi, FamNomb = x.Famnomb }).Select(x => new EqFamiliaDTO() { Famcodi = x.Key.FamCodi, Famnomb = x.Key.FamNomb }).ToList();
                bool tieneVarioTC = listaTipoCentral.Count >= 2;
                string descModoGrupo = listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null && listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                    ? "Modo de Operación - Grupo" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica) != null
                    ? "Modo de Operación" : listaTipoCentral.Find(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica) != null
                    ? "Grupo" : string.Empty;

                int colIniCodigo = 2;
                int colIniEmpresa = colIniCodigo + 1;
                int colIniTipoCentral = colIniEmpresa + 1;
                if (!tieneVarioTC)
                {
                    colIniTipoCentral = colIniEmpresa;
                }
                int colIniCentral = colIniTipoCentral + 1;
                int colIniEquipo = colIniCentral + 1;
                int colIniModo = colIniEquipo + 1;
                if (descModoGrupo == string.Empty)
                {
                    colIniModo = colIniCentral;
                }
                int colIniEnParalelo = colIniModo + 1;
                int colIniFueraParalelo = colIniEnParalelo + 1;
                int colIniOArranque = colIniFueraParalelo + 1;
                int colIniOParada = colIniOArranque + 1;
                int colIniTipoOp = colIniOParada + 1;

                int colIniEnsayoPe = colIniTipoOp;
                if (checkEnsayoPe)
                {
                    colIniEnsayoPe = colIniTipoOp + 1;
                }
                int colIniEnsayoPMin = colIniEnsayoPe;
                if (checkEnsayoPMin)
                {
                    colIniEnsayoPMin = colIniEnsayoPe + 1;
                }

                int colIniSistema = colIniEnsayoPMin + 1;
                int colIniLimTransm = colIniSistema + 1;
                int colIniCausa = colIniLimTransm + 1;
                int colIniObs = colIniCausa + 1;

                int colIniCompArr = colIniObs;
                int colIniCompPar = colIniObs;
                if (checkCompensar)
                {
                    colIniCompArr = colIniObs + 1;
                    colIniCompPar = colIniCompArr + 1;
                }

                int colIniDesglose = colIniCompPar + 1;
                int colFinDesglose = colIniDesglose + 1;
                int colIniDesgloseTipo = colFinDesglose + 1;
                int colIniDesgloseValor = colIniDesgloseTipo + 1;

                int colIniUsuariomodif = colIniDesgloseValor + 1;
                int colIniFechamodif = colIniUsuariomodif + 1;

                wsHist.Cells[row, colIniCodigo].Value = "Código";
                if (tieneVarioTC)
                {
                    wsHist.Cells[row, colIniTipoCentral].Value = "Tipo Central";
                }
                wsHist.Cells[row, colIniEmpresa].Value = "Empresa";
                wsHist.Cells[row, colIniCentral].Value = "Central";
                wsHist.Cells[row, colIniEquipo].Value = "Grupo";
                if (descModoGrupo != string.Empty)
                {
                    wsHist.Cells[row, colIniModo].Value = descModoGrupo;
                }
                wsHist.Cells[row, colIniEnParalelo].Value = "Inicio";
                wsHist.Cells[row, colIniFueraParalelo].Value = "Final";
                wsHist.Cells[row, colIniOArranque].Value = "O. Arranque";
                wsHist.Cells[row, colIniOParada].Value = "O. Parada";
                wsHist.Cells[row, colIniTipoOp].Value = "Tipo Operación";

                if (checkEnsayoPe)
                    wsHist.Cells[row, colIniEnsayoPe].Value = "Ensayo de Potencia Efectiva";

                if (checkEnsayoPMin)
                    wsHist.Cells[row, colIniEnsayoPMin].Value = "Ensayo de Potencia Mínima";

                wsHist.Cells[row, colIniSistema].Value = "Sistema";
                wsHist.Cells[row, colIniLimTransm].Value = "Lim. Transm.";
                wsHist.Cells[row, colIniCausa].Value = "Causa";
                wsHist.Cells[row, colIniObs].Value = "Observación";

                if (checkCompensar)
                {
                    wsHist.Cells[row, colIniCompArr].Value = "Comp. Arrq";
                    wsHist.Cells[row, colIniCompPar].Value = "Comp. Pard";
                }

                wsHist.Cells[row, colIniDesglose].Value = "Hora Inicio";
                wsHist.Cells[row, colFinDesglose].Value = "Hora Fin";
                wsHist.Cells[row, colIniDesgloseTipo].Value = "Tipo";
                wsHist.Cells[row, colIniDesgloseValor].Value = "Valor";

                wsHist.Cells[row, colIniUsuariomodif].Value = "Usuario modificación";
                wsHist.Cells[row, colIniFechamodif].Value = "Fecha modificación";

                wsHist.Cells[row, colIniCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                if (descModoGrupo != string.Empty)
                {
                    wsHist.Cells[row, colIniModo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                wsHist.Cells[row, colIniEnParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniFueraParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniOArranque].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniOParada].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                if (checkEnsayoPe)
                    wsHist.Cells[row, colIniEnsayoPe].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                if (checkEnsayoPMin)
                    wsHist.Cells[row, colIniEnsayoPMin].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, colIniSistema].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniLimTransm].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCausa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniObs].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                if (checkCompensar)
                {
                    wsHist.Cells[row, colIniCompArr].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCompPar].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                wsHist.Cells[row, colIniDesglose].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colFinDesglose].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDesgloseTipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniDesgloseValor].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = wsHist.Cells[row, colIniCodigo, row, colIniFechamodif])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Font.Bold = true;
                }

                //Desglose
                int rowDesgloseTitulo = row - 1;
                AgregarCeldaTipoDesgloseExcel(wsHist, rowDesgloseTitulo, colIniDesglose, colIniDesgloseValor, "Restricción Operativa", 1);

                #endregion

                #region cuerpo
                row++;
                foreach (var reg in data)
                {
                    wsHist.Cells[row, colIniCodigo].Value = reg.Hopcodi;
                    wsHist.Cells[row, colIniCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCodigo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniCodigo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    if (tieneVarioTC)
                    {
                        wsHist.Cells[row, colIniTipoCentral].Value = reg.Famnomb.Trim();
                        wsHist.Cells[row, colIniTipoCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniTipoCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }

                    wsHist.Cells[row, colIniEmpresa].Value = reg.Emprnomb.Trim();
                    wsHist.Cells[row, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniEmpresa].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniCentral].Value = reg.PadreNombre.Trim();
                    wsHist.Cells[row, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniEquipo].Value = reg.Equiabrev;
                    wsHist.Cells[row, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniEquipo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    if (descModoGrupo != string.Empty)
                    {
                        wsHist.Cells[row, colIniModo].Value = reg.EquipoNombre.Trim();
                        wsHist.Cells[row, colIniModo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniCodigo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }

                    wsHist.Cells[row, colIniEnParalelo].Value = reg.HophoriniDesc;
                    wsHist.Cells[row, colIniEnParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniEnParalelo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniEnParalelo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniFueraParalelo].Value = reg.HophorfinDesc;
                    wsHist.Cells[row, colIniFueraParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniFueraParalelo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniFueraParalelo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniOArranque].Value = reg.HophorordarranqDesc;
                    wsHist.Cells[row, colIniOArranque].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniOArranque].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniOArranque].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniOParada].Value = reg.HophorparadaDesc;
                    wsHist.Cells[row, colIniOParada].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniOParada].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniOParada].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniTipoOp].Value = reg.Subcausadesc;
                    wsHist.Cells[row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniTipoOp].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    if (checkEnsayoPe)
                    {
                        wsHist.Cells[row, colIniEnsayoPe].Value = reg.HopensayopeDesc;
                        wsHist.Cells[row, colIniEnsayoPe].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniEnsayoPe].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        wsHist.Cells[row, colIniEnsayoPe].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    if (checkEnsayoPMin)
                    {
                        wsHist.Cells[row, colIniEnsayoPMin].Value = reg.HopensayopminDesc;
                        wsHist.Cells[row, colIniEnsayoPMin].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniEnsayoPMin].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        wsHist.Cells[row, colIniEnsayoPMin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    wsHist.Cells[row, colIniSistema].Value = reg.HopsaisladoDesc;
                    wsHist.Cells[row, colIniSistema].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniSistema].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniLimTransm].Value = reg.HoplimtransDesc;
                    wsHist.Cells[row, colIniLimTransm].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniLimTransm].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniLimTransm].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniCausa].Value = reg.HopcausacodiDesc;
                    wsHist.Cells[row, colIniCausa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCausa].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniObs].Value = reg.Hopdesc;
                    wsHist.Cells[row, colIniObs].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniObs].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    wsHist.Cells[row, colIniObs].Style.WrapText = true;

                    if (checkCompensar)
                    {
                        wsHist.Cells[row, colIniCompArr].Value = reg.HopcompordarrqDesc;
                        wsHist.Cells[row, colIniCompArr].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniCompArr].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsHist.Cells[row, colIniCompArr].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[row, colIniCompPar].Value = reg.HopcompordpardDesc;
                        wsHist.Cells[row, colIniCompPar].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniCompPar].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsHist.Cells[row, colIniCompPar].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }

                    wsHist.Cells[row, colIniUsuariomodif].Value = reg.Lastuser;
                    wsHist.Cells[row, colIniUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniUsuariomodif].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniUsuariomodif].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    wsHist.Cells[row, colIniFechamodif].Value = reg.LastdateDesc;
                    wsHist.Cells[row, colIniFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniFechamodif].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colIniFechamodif].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    List<EveHoEquiporelDTO> listaDesglose = reg.ListaDesglose ?? new List<EveHoEquiporelDTO>();
                    if (listaDesglose.Count > 0)
                    {
                        int rowIni = row;
                        foreach (var regDesglose in listaDesglose)
                        {
                            AgregarCeldaFechaReporteExcel(wsHist, row, colIniDesglose, regDesglose.IchoriniDesc);
                            AgregarCeldaFechaReporteExcel(wsHist, row, colFinDesglose, regDesglose.IchorfinDesc);
                            AgregarCeldaFechaReporteExcel(wsHist, row, colIniDesgloseTipo, regDesglose.Subcausadesc);
                            AgregarCeldaFechaReporteExcel(wsHist, row, colIniDesgloseValor, regDesglose.Icvalor1.ToString());
                            row++;
                        }
                        row--;
                        wsHist.Cells[rowIni, colIniCodigo, row, colIniCodigo].Merge = true;
                        wsHist.Cells[rowIni, colIniCodigo, row, colIniCodigo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniCodigo, row, colIniCodigo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        if (tieneVarioTC)
                        {
                            wsHist.Cells[rowIni, colIniTipoCentral, row, colIniTipoCentral].Merge = true;
                            wsHist.Cells[rowIni, colIniTipoCentral, row, colIniTipoCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            wsHist.Cells[rowIni, colIniTipoCentral, row, colIniTipoCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        }
                        wsHist.Cells[rowIni, colIniEmpresa, row, colIniEmpresa].Merge = true;
                        wsHist.Cells[rowIni, colIniEmpresa, row, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniEmpresa, row, colIniEmpresa].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[rowIni, colIniCentral, row, colIniCentral].Merge = true;
                        wsHist.Cells[rowIni, colIniCentral, row, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniCentral, row, colIniCentral].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        if (descModoGrupo != string.Empty)
                        {
                            wsHist.Cells[rowIni, colIniModo, row, colIniModo].Merge = true;
                            wsHist.Cells[rowIni, colIniModo, row, colIniModo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            wsHist.Cells[rowIni, colIniModo, row, colIniModo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        }
                        wsHist.Cells[rowIni, colIniEquipo, row, colIniEquipo].Merge = true;
                        wsHist.Cells[rowIni, colIniEquipo, row, colIniEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniEquipo, row, colIniEquipo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[rowIni, colIniEnParalelo, row, colIniEnParalelo].Merge = true;
                        wsHist.Cells[rowIni, colIniEnParalelo, row, colIniEnParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniEnParalelo, row, colIniEnParalelo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[rowIni, colIniFueraParalelo, row, colIniFueraParalelo].Merge = true;
                        wsHist.Cells[rowIni, colIniFueraParalelo, row, colIniFueraParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniFueraParalelo, row, colIniFueraParalelo].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[rowIni, colIniTipoOp, row, colIniTipoOp].Merge = true;
                        wsHist.Cells[rowIni, colIniTipoOp, row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniTipoOp, row, colIniTipoOp].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[rowIni, colIniUsuariomodif, row, colIniUsuariomodif].Merge = true;
                        wsHist.Cells[rowIni, colIniUsuariomodif, row, colIniUsuariomodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniUsuariomodif, row, colIniUsuariomodif].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        wsHist.Cells[rowIni, colIniFechamodif, row, colIniFechamodif].Merge = true;
                        wsHist.Cells[rowIni, colIniFechamodif, row, colIniFechamodif].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[rowIni, colIniFechamodif, row, colIniFechamodif].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    }
                    else
                    {
                        AgregarCeldaFechaReporteExcel(wsHist, row, colIniDesglose, string.Empty);
                        AgregarCeldaFechaReporteExcel(wsHist, row, colFinDesglose, string.Empty);
                        AgregarCeldaFechaReporteExcel(wsHist, row, colIniDesgloseTipo, string.Empty);
                        AgregarCeldaFechaReporteExcel(wsHist, row, colIniDesgloseValor, string.Empty);
                    }
                    row++;
                }

                wsHist.Column(1).Width = 3;
                wsHist.Column(colIniCodigo).Width = 13;
                if (tieneVarioTC)
                {
                    wsHist.Column(colIniTipoCentral).Width = 30;
                }
                wsHist.Column(colIniEmpresa).Width = 35;
                wsHist.Column(colIniCentral).Width = 25;
                wsHist.Column(colIniEquipo).Width = 13;
                if (descModoGrupo != string.Empty)
                {
                    wsHist.Column(colIniModo).Width = 45;
                }
                wsHist.Column(colIniEnParalelo).Width = 20;
                wsHist.Column(colIniFueraParalelo).Width = 20;
                wsHist.Column(colIniOArranque).Width = 20;
                wsHist.Column(colIniOParada).Width = 20;
                wsHist.Column(colIniTipoOp).Width = 30;

                if (checkEnsayoPe)
                    wsHist.Column(colIniEnsayoPe).Width = 28;

                if (checkEnsayoPMin)
                    wsHist.Column(colIniEnsayoPMin).Width = 28;

                wsHist.Column(colIniSistema).Width = 12;
                wsHist.Column(colIniLimTransm).Width = 12;
                wsHist.Column(colIniCausa).Width = 30;
                wsHist.Column(colIniObs).Width = 75;

                if (checkCompensar)
                {
                    wsHist.Column(colIniCompArr).Width = 12;
                    wsHist.Column(colIniCompPar).Width = 12;
                }

                wsHist.Column(colIniDesglose).Width = 20;
                wsHist.Column(colFinDesglose).Width = 20;
                wsHist.Column(colIniDesgloseTipo).Width = 25;
                wsHist.Column(colIniDesgloseValor).Width = 9;

                wsHist.Column(colIniUsuariomodif).Width = 20;
                wsHist.Column(colIniFechamodif).Width = 20;

                #endregion

                #region logo

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = wsHist.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;

                #endregion

                #endregion

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        /// <summary>
        /// Setear elementos para celda de tipo fecha en el reporte de Horas de Operación
        /// </summary>
        /// <param name="wsHist"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="fechaDesc"></param>
        private void AgregarCeldaFechaReporteExcel(ExcelWorksheet wsHist, int row, int col, string fechaDesc)
        {
            wsHist.Cells[row, col].Value = fechaDesc ?? string.Empty;
            wsHist.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            wsHist.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void AgregarCeldaTipoDesgloseExcel(ExcelWorksheet ws, int row, int colIni, int colFin, string tipoDesglose, int turnoColor)
        {
            ws.Cells[row, colIni].Value = tipoDesglose;
            ws.Cells[row, colIni, row, colFin].Merge = true;
            ws.Cells[row, colIni, row, colFin].Style.WrapText = true;
            ws.Cells[row, colIni, row, colFin].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[row, colIni, row, colFin].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[row, colIni, row, colFin].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            using (var range = ws.Cells[row, colIni, row, colFin])
            {
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(turnoColor % 2 == 1 ? ColorTranslator.FromHtml("#FFC000") : ColorTranslator.FromHtml("#2980B9"));
                range.Style.Font.Color.SetColor(Color.White);
            }
        }

        #endregion

        #region Horas Operacion EMS

        /// <summary>
        /// Reporte de HORAS DE OPERACIÓN INTRANET - EMS
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="empresa"></param>
        /// <param name="fechaCI"></param>
        /// <param name="idEnvio"></param>
        /// <param name="consultarOtros"></param>
        /// <param name="listaHOHoyValidacion"></param>
        /// <param name="listaValEmsUniNoReg"></param>
        /// <param name="listaHopcodiValInterFS"></param>
        /// <param name="listaEms"></param>
        /// <param name="listaValScadaUniNoReg"></param>
        public void ListarReporteHopEms(DateTime fechaIni, string empresa, DateTime fechaCI, int? idEnvio
            , bool consultarOtros, out List<EveHoraoperacionDTO> listaHOHoyValidacion
            , out List<ResultadoValidacionAplicativo> listaValEmsUniNoReg, out List<int> listaHopcodiValInterFS
            , out List<MeMedicion48DTO> listaEms
            , out List<ResultadoValidacionAplicativo> listaValScadaUniNoReg)
        {
            //Validación para la fecha de consulta
            List<int> listaValidacion = new List<int> { ConstantesHorasOperacion.ValidarIntervenciones, ConstantesHorasOperacion.ValidarScada,
                                                ConstantesHorasOperacion.ValidarEMS };

            this.ListarValidacionHorasOperacionYAplicativos(fechaIni, idEnvio, empresa, string.Empty, fechaCI, listaValidacion
                , consultarOtros
                , out List<EveHoraoperacionDTO> listaDataHoy
                , out List<ResultadoValidacionAplicativo> listaValEmsUniReg, out List<ResultadoValidacionAplicativo> listaValScadaUniReg, out List<ResultadoValidacionAplicativo> listaValInterv, out List<ResultadoValidacionAplicativo> listaValCostoIncremental
                , out listaValEmsUniNoReg, out listaEms, out List<ReporteCostoIncrementalDTO> listaReporteCostosIncr, out listaValScadaUniNoReg);

            //asignar validacion a cada hora operacion
            foreach (var reg in listaDataHoy)
            {
                if (consultarOtros)
                {
                    reg.TieneAlertaEms = this.VerificaAlarmaHo(reg, listaValEmsUniReg) ? ConstantesHorasOperacion.AlertaEmsSI : ConstantesHorasOperacion.AlertaEmsNO;
                    reg.TieneAlertaScada = this.VerificaAlarmaHo(reg, listaValScadaUniReg) ? ConstantesHorasOperacion.AlertaScadaSI : ConstantesHorasOperacion.AlertaScadaNO;
                    reg.TieneAlertaIntervencion = this.VerificaAlarmaHo(reg, listaValInterv) ? ConstantesHorasOperacion.AlertaIntervencionSI : ConstantesHorasOperacion.AlertaIntervencionNO;
                    reg.TieneAlertaCostoIncremental = this.VerificaAlarmaHo(reg, listaValCostoIncremental) ? ConstantesHorasOperacion.AlertaCostoIncrementalSI : ConstantesHorasOperacion.AlertaCostoIncrementalNO;
                }
            }

            //salidas
            listaHOHoyValidacion = listaDataHoy;
            listaHopcodiValInterFS = listaValInterv.Select(x => x.HoraOperacion.Hopcodi).Distinct().ToList();
        }

        /// <summary>
        /// Llenamos los datos de pmin, etc.
        /// </summary>
        /// <param name="listaModosOperacion"></param>
        /// <param name="dfechaIni"></param>
        /// <returns></returns>
        public void AsignarDatosModoOperacion(List<PrGrupoDTO> listaModosOperacion, DateTime dfechaIni)
        {
            //Formatear decimales
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();
            nfi2.NumberDecimalSeparator = ".";
            NumberFormatInfo nfi1 = UtilAnexoAPR5.GenerarNumberFormatInfo1();
            nfi1.NumberDecimalSeparator = ".";

            //datos de ficha técnica
            List<int> grupos = listaModosOperacion.Select(x => x.Grupocodi).Distinct().ToList();
            string conceptos = "14,16,136,139";
            List<PrGrupodatDTO> listaGrupoDat = FactorySic.GetPrGrupodatRepository().ParametrosConfiguracionPorFecha(dfechaIni, string.Join(",", grupos), string.Join(",", conceptos));

            //colores de las centrales vigentes hoy
            List<int> equipadres = listaModosOperacion.Where(x => x.Equipadre > 0).Select(x => x.Equipadre).Distinct().ToList();
            List<EqPropequiDTO> listaValProiedad = FactorySic.GetEqPropequiRepository()
                .ListarValoresVigentesPropiedades(DateTime.Today, string.Join(",", equipadres), "-1", "-1", ConstantesIEOD.IDPropiedadColor + "", string.Empty, "-1");

            //asignar datos a modo
            foreach (var reg in listaModosOperacion)
            {
                //ficha técnica
                reg.PMin = listaGrupoDat.FirstOrDefault(p => p.Grupocodi == reg.Grupocodi && p.Concepcodi == ConstantesHorasOperacion.ConcepcodiPotenciaMinima)?.Formuladat;
                reg.PEfe = listaGrupoDat.FirstOrDefault(p => p.Grupocodi == reg.Grupocodi && p.Concepcodi == ConstantesHorasOperacion.ConcepcodiPotenciaEfectiva)?.Formuladat;
                reg.TMinO = listaGrupoDat.FirstOrDefault(p => p.Grupocodi == reg.Grupocodi && p.Concepcodi == ConstantesHorasOperacion.ConcepcodiTiempoMinOperacion)?.Formuladat;
                reg.TMinA = listaGrupoDat.FirstOrDefault(p => p.Grupocodi == reg.Grupocodi && p.Concepcodi == ConstantesHorasOperacion.ConcepcodiTiempoEntreArranques)?.Formuladat;

                if (!EsNumero(reg.PMin)) { reg.PMin = string.Empty; } else { reg.PMin = MathHelper.TruncateDouble(Convert.ToDouble(reg.PMin), 2).ToString("N", nfi1); }
                if (!EsNumero(reg.PEfe)) { reg.PEfe = string.Empty; } else { reg.PEfe = MathHelper.TruncateDouble(Convert.ToDouble(reg.PEfe), 2).ToString("N", nfi1); }

                if (!EsNumero(reg.TMinO)) { reg.TMinO = string.Empty; } else { reg.TMinO = MathHelper.TruncateDouble(Convert.ToDouble(reg.TMinO), 2).ToString("N", nfi1); }

                if (!EsNumero(reg.TMinA)) { reg.TMinA = string.Empty; }
                else
                {
                    reg.TMinA = MathHelper.TruncateDouble(Convert.ToDouble(reg.TMinA) / 60, 2).ToString("N", nfi1);
                }

                //color central
                reg.ColorTermica = ConstantesIEOD.PropiedadColorDefault;
                var config = listaValProiedad.FirstOrDefault(x => x.Equicodi == reg.Equipadre);
                if (config != null) reg.ColorTermica = config.Valor ?? ConstantesIEOD.PropiedadColorDefault;
            }
        }

        /// <summary>
        /// Completar las lista de horas de operacion para correcta visualizacion de las hop antigüas
        /// </summary>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> CompletarListaHoraOperacionTermo(List<EveHoraoperacionDTO> listaData)
        {
            List<EveHoraoperacionDTO> listaFinal = new List<EveHoraoperacionDTO>();

            //Horas de operación registradas en aplicativo web 
            List<EveHoraoperacionDTO> listaNewIeod = listaData.Where(x => x.Hopcodipadre >= 0 && x.Hopcodi > 0).ToList(); //1. codigos de la tabla eve_horaoperacion y eve_ho_unidad
            List<int> listaHopcodiNew = listaNewIeod.Select(x => x.Hopcodi).Distinct().ToList();

            //Horas de operación registradas en aplicativo desktop
            listaHopcodiNew = listaNewIeod.Select(x => x.Hopcodi).Distinct().ToList();
            List<EveHoraoperacionDTO> listaOldIeod = listaData.Where(x => !listaHopcodiNew.Contains(x.Hopcodi)
                && x.Hopcodipadre == null && x.Grupocodi != null && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();

            //Data maestra
            List<PrGrupoDTO> listaModoYeq = new List<PrGrupoDTO>();

            //1. Completar horas de operación faltantes EVE_HO_UNIDAD (a nivel de equipos) del aplicativo desktop. Anteriores al 2019
            if (listaOldIeod.Any())
            {
                List<PrGrupoDTO> listaUnidadXModo = ListarUnidadesWithModoOperacionXCentralYEmpresa(-2, "-2");
                listaModoYeq = ListarModoOperacionXCentralYEmpresa(-2, -2);
                List<int> listaGrupocodiUnidEsp = listaModoYeq.Where(x => x.FlagModoEspecial == "S").Select(x => x.Grupocodi).Distinct().ToList();

                foreach (var reg in listaOldIeod)
                {
                    var tipoHO = listaGrupocodiUnidEsp.Contains(reg.Grupocodi.Value) ? ConstantesHorasOperacion.TipoHOUnidadEspecial : ConstantesHorasOperacion.TipoHONormal;
                    listaFinal.AddRange(this.GenerarListaTemporalReporteHoByModoDesktop(reg, listaUnidadXModo, tipoHO));
                }
            }

            listaFinal.AddRange(listaNewIeod);

            //2. Completar horas de operación faltantes EVE_HO_UNIDAD (a nivel de equipos) del aplicativo web. Esto aplica cuando no configuran correctamente el modo de operación
            if (ExisteHoraOperacionSinUnidad(listaNewIeod))
            {
                listaModoYeq = ListarModoOperacionXCentralYEmpresa(-2, -2);
                listaFinal.AddRange(this.GenerarListaTemporalFaltanteReporteHoByModoWeb(listaNewIeod, listaModoYeq));
            }

            return listaFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Hophorini).ThenBy(x => x.Grupocodi).ThenBy(x => x.Equicodi).ToList();
        }

        /// <summary>
        /// Listar hop segun envio
        /// </summary>
        /// <param name="listaData"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHopByEnvio(List<EveHoraoperacionDTO> listaData, int? idEnvio)
        {
            //Envio
            if (idEnvio != null && idEnvio > 0)
            {
                var listaMeenviodet = this.servIeod.GetByCriteriaMeEnviodets(idEnvio.Value);
                List<int> listaDetalleEnvio = listaMeenviodet.Select(x => x.Envdetfpkcodi).ToList();

                return listaData.Where(x => listaDetalleEnvio.Contains(x.Hopcodi)).ToList();
            }

            return listaData;
        }

        /// <summary>
        /// Data Ems de los equipos que no operaron por cada hora de operacion del modo
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="idEnvio"></param>
        /// <param name="empresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <param name="fechaCI"></param>
        /// <param name="listaValidacionPermitida"></param>
        /// <param name="consultarOtros"></param>
        /// <param name="listaHopFinal"></param>
        /// <param name="listaValEmsUniReg"></param>
        /// <param name="listaValScadaUniReg"></param>
        /// <param name="listaValInterv"></param>
        /// <param name="listaValCostoIncremental"></param>
        /// <param name="listaValEmsUniNoReg"></param>
        /// <param name="listaEms"></param>
        /// <param name="listaReporteCostosIncr"></param>
        /// <param name="listaValScadaUniNoReg"></param>
        public void ListarValidacionHorasOperacionYAplicativos(DateTime fechaIni, int? idEnvio, string empresa, string hopcodipadre, DateTime fechaCI, List<int> listaValidacionPermitida
            , bool consultarOtros
            , out List<EveHoraoperacionDTO> listaHopFinal
            , out List<ResultadoValidacionAplicativo> listaValEmsUniReg, out List<ResultadoValidacionAplicativo> listaValScadaUniReg, out List<ResultadoValidacionAplicativo> listaValInterv, out List<ResultadoValidacionAplicativo> listaValCostoIncremental
            , out List<ResultadoValidacionAplicativo> listaValEmsUniNoReg
            , out List<MeMedicion48DTO> listaEms
            , out List<ReporteCostoIncrementalDTO> listaReporteCostosIncr
            , out List<ResultadoValidacionAplicativo> listaValScadaUniNoReg)
        {
            DateTime fechaFin = fechaIni.AddDays(1);

            //variables debug
            DateTime fechaHoraIni = DateTime.Now;
            int contadorIni = 1;

            // Datos

            //Lista de HOP de las unidades de la HOP de el modo para el día de consulta
            List<EveHoraoperacionDTO> listaHOP = this.ListarHorasOperacionByCriteria(fechaIni, fechaFin, empresa, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico).ToList();

            listaHOP = this.ListarHopByEnvio(listaHOP, idEnvio);
            listaHOP = this.CompletarListaHoraOperacionTermo(listaHOP);

            listaValEmsUniReg = new List<ResultadoValidacionAplicativo>();
            listaValCostoIncremental = new List<ResultadoValidacionAplicativo>();
            listaReporteCostosIncr = new List<ReporteCostoIncrementalDTO>();
            listaEms = new List<MeMedicion48DTO>();
            listaValScadaUniReg = new List<ResultadoValidacionAplicativo>();
            listaValInterv = new List<ResultadoValidacionAplicativo>();
            listaValEmsUniNoReg = new List<ResultadoValidacionAplicativo>();
            listaValScadaUniNoReg = new List<ResultadoValidacionAplicativo>();

            // Para Forzar la vista de Costos CT
            bool consultarOtrosOriginal = consultarOtros;

            consultarOtros = true;

            if (consultarOtros)
            {
                //Horas de Operación
                if (!string.IsNullOrEmpty(hopcodipadre)) //si es una o varias horas de operación en específico
                {
                    List<int> listaHopcodi = hopcodipadre.Split(',').Select(x => int.Parse(x)).ToList();
                    listaHOP = listaHOP.Where(x => listaHopcodi.Contains(x.Hopcodipadre.GetValueOrDefault(0)) || listaHopcodi.Contains(x.Hopcodi)).ToList();
                }

                List<EveHoraoperacionDTO> listaDataHopByModo = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
                List<int> listaGrupocodiModo = listaDataHopByModo.Where(x => x.Grupocodi > 0).Select(x => x.Grupocodi.Value).Distinct().ToList();

                List<EqEquipoDTO> listaEquipoHop = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad)
                                                    .GroupBy(x => x.Equicodi.GetValueOrDefault(0)).Select(x =>
                                                    new EqEquipoDTO() { Equicodi = x.Key, Equinomb = x.First().Equiabrev, Emprcodi = x.First().Emprcodi }).ToList();

                //Umbral MW mínimo EMS / Scada
                decimal valorUmbral = (new ParametroAppServicio()).ObtenerValorVigente(ConstantesHorasOperacion.IdParametroUmbral);

                //Lista de EMS que no estuvieron operativo
                if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarEMS))
                {
                    listaEms = this.GetListaGeneracionEmsCompleta2(fechaIni.AddDays(-1), fechaIni, empresa, "5,3", valorUmbral).ToList();
                }

                //Lista de Reporte Costos Incrementales
                if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarCostoIncremental))
                {
                    List<PrGrupoDTO> listaModos = new List<PrGrupoDTO>();
                    foreach (var grupocodi in listaGrupocodiModo)
                        listaModos.Add(new PrGrupoDTO() { Grupocodi = grupocodi });

                    listaReporteCostosIncr = this.servGrDespach.ListarTodosCI(listaGrupocodiModo, fechaCI);
                    //listaReporteCostosIncr = this.servGrDespach.ListarTodosCI(fechaCI);
                    listaModos = this.OrdenarListXCostoIncrementalByFiltro(listaModos, listaHOP, fechaCI, listaReporteCostosIncr);
                    this.ListarValidacionCostoIncremental(listaModos, listaDataHopByModo, out List<EveHoraoperacionDTO> listaHoOut, out listaValCostoIncremental);
                }

                if (consultarOtrosOriginal)
                {
                    //Lista de Scada
                    List<MeMedicion96DTO> listaScada = new List<MeMedicion96DTO>();
                    if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarScada))
                    {
                        listaScada = this.GetListaGeneracionScadaCompletaByDespacho(fechaIni, empresa, valorUmbral);
                    }

                    //Lista de Intervenciones
                    List<InIntervencionDTO> listaIntervencion = new List<InIntervencionDTO>();
                    if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarIntervenciones))
                        listaIntervencion = (new IntervencionesAppServicio()).ListarIntervencionesEquiposGen(fechaIni, fechaIni, ConstantesHorasOperacion.IdGeneradorTemoelectrico, ConstantesHorasOperacion.IdTipoTermica)
                        .Where(x => x.Interindispo == ConstantesIntervencionesAppServicio.sFS).ToList();

                    // Proceso de validación (Scada, Ems, Costo Incremental, Intervenciones)

                    this.ListarHorasOperacionUnidadesRegistrados(fechaIni, listaHOP, listaEquipoHop, listaValidacionPermitida, listaEms.Where(x => x.Medifecha == fechaIni).ToList(), listaScada, listaIntervencion, consultarOtros, out listaValEmsUniReg, out listaValScadaUniReg, out listaValInterv);
                    this.ListarHorasOperacionUnidadesNoRegistrados(fechaIni, listaHOP, listaEms.Where(x => x.Medifecha == fechaIni).ToList(), out listaValEmsUniNoReg);
                    this.ListarHorasOperacionUnidadesNoRegistradosScada(fechaIni, listaHOP, listaScada.Where(x => x.Medifecha == fechaIni).ToList(), out listaValScadaUniNoReg);

                    if (!string.IsNullOrEmpty(hopcodipadre)) //si es una o varias horas de operación en específico
                    {
                        List<int> listaHopcodi = hopcodipadre.Split(',').Select(x => int.Parse(x)).ToList();
                        listaValEmsUniReg = listaValEmsUniReg.Where(x => listaHopcodi.Contains(x.Hopcodipadre.GetValueOrDefault(0))).ToList();
                        listaValScadaUniReg = listaValScadaUniReg.Where(x => listaHopcodi.Contains(x.Hopcodipadre.GetValueOrDefault(0))).ToList();
                    }
                }
                else
                {
                    listaEms = new List<MeMedicion48DTO>();
                    listaValEmsUniReg = new List<ResultadoValidacionAplicativo>();
                    listaValScadaUniReg = new List<ResultadoValidacionAplicativo>();
                }
            }

            listaHopFinal = listaHOP;
        }

        /// <summary>
        /// Data Ems de los equipos que no operaron por hora de operacion
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="idEnvio"></param>
        /// <param name="empresa"></param>
        /// <param name="listaHopcodipadre"></param>
        /// <param name="consultarOtros"></param>
        /// <returns></returns>
        public List<ResultadoValidacionAplicativo> ListarAlertaEmsEquipoNoOperaronByListaHo(DateTime fechaIni, int? idEnvio, string empresa, string listaHopcodipadre, bool consultarOtros)
        {
            this.ListarValidacionHorasOperacionYAplicativos(fechaIni, idEnvio, empresa, listaHopcodipadre, DateTime.MinValue, new List<int> { ConstantesHorasOperacion.ValidarEMS }
            , consultarOtros
            , out _
            , out List<ResultadoValidacionAplicativo> listaValEmsUniReg, out _, out _, out _
            , out _
            , out _, out _
            , out _);

            return listaValEmsUniReg;
        }

        /// <summary>
        /// Data Scada de los equipos que no operaron por hora de operacion
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="idEnvio"></param>
        /// <param name="empresa"></param>
        /// <param name="listaHopcodipadre"></param>
        /// <param name="consultarOtros"></param>
        /// <returns></returns>
        public List<ResultadoValidacionAplicativo> ListarAlertaScadaEquipoNoOperaronByListaHo(DateTime fechaIni, int? idEnvio, string empresa, string listaHopcodipadre, bool consultarOtros)
        {
            this.ListarValidacionHorasOperacionYAplicativos(fechaIni, idEnvio, empresa, listaHopcodipadre, DateTime.MinValue, new List<int> { ConstantesHorasOperacion.ValidarScada }
            , consultarOtros
            , out _
            , out _, out List<ResultadoValidacionAplicativo> listaValScadaUniReg, out _, out _
            , out _
            , out _, out _
            , out _);

            return listaValScadaUniReg;
        }

        /// <summary>
        /// Data Scada de los equipos que no operaron por hora de operacion
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="idEnvio"></param>
        /// <param name="empresa"></param>
        /// <param name="listaHopcodipadre"></param>
        /// <param name="consultarOtros"></param>
        /// <returns></returns>
        public List<ResultadoValidacionAplicativo> ListarAlertaIntervencionEquipoNoOperaronByListaHo(DateTime fechaIni, int? idEnvio, string empresa, string listaHopcodipadre, bool consultarOtros)
        {
            this.ListarValidacionHorasOperacionYAplicativos(fechaIni, idEnvio, empresa, listaHopcodipadre, DateTime.MinValue, new List<int> { ConstantesHorasOperacion.ValidarIntervenciones }
            , consultarOtros
            , out _
            , out _, out _, out List<ResultadoValidacionAplicativo> listaValInterv, out _
            , out _
            , out _, out _
            , out _);

            return listaValInterv;
        }

        /// <summary>
        /// Listar las Unidades de las Horas de operación cuyas unidades no tienen correspondencia en EMS, Scada
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="listaHOP"></param>
        /// <param name="listaEquipoHop"></param>
        /// <param name="listaValidacionPermitida"></param>
        /// <param name="listaEmsCompleta"></param>
        /// <param name="listaScada"></param>
        /// <param name="listaIntervencion"></param>
        /// <param name="consultarOtros"></param>
        /// <param name="listaEmsFinal"></param>
        /// <param name="listaScadaFinal"></param>
        /// <param name="listaIntervFinal"></param>
        /// <returns></returns>
        public void ListarHorasOperacionUnidadesRegistrados(DateTime fechaIni, List<EveHoraoperacionDTO> listaHOP, List<EqEquipoDTO> listaEquipoHop
            , List<int> listaValidacionPermitida, List<MeMedicion48DTO> listaEmsCompleta, List<MeMedicion96DTO> listaScada, List<InIntervencionDTO> listaIntervencion
            , bool consultarOtros
            , out List<ResultadoValidacionAplicativo> listaEmsFinal, out List<ResultadoValidacionAplicativo> listaScadaFinal, out List<ResultadoValidacionAplicativo> listaIntervFinal)
        {
            listaScadaFinal = new List<ResultadoValidacionAplicativo>();
            listaEmsFinal = new List<ResultadoValidacionAplicativo>();
            listaIntervFinal = new List<ResultadoValidacionAplicativo>();

            DateTime fechaActual96 = DateTime.Now.AddMinutes(-ConstantesHorasOperacion.DelayNumMinDataScada);
            int indiceH96Actual = Util.GetPosicionHoraInicial96(fechaActual96)[0];
            DateTime fechaActual48 = DateTime.Now.AddMinutes(-ConstantesHorasOperacion.DelayNumMinDataEms);
            int indiceH48Actual = Util.GetPosicionHoraInicial48(fechaActual48)[0];

            List<int> listaHopcodiPadre = listaHOP.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).Select(x => x.Hopcodi).Distinct().ToList();

            if (listaHopcodiPadre.Count > 0)
            {
                EqEquipoDTO equipoHop;
                List<EveHoraoperacionDTO> listaHOPDia;

                DateTime f = fechaIni.Date;
                foreach (var hopcodipadre in listaHopcodiPadre)
                {
                    var hop = listaHOP.Where(x => x.Hopcodi == hopcodipadre).First();
                    hop.HophoriniDesc = hop.Hophorini != null ? hop.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                    hop.HophorfinDesc = hop.Hophorfin != null ? hop.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                    hop.HophorordarranqDesc = hop.Hophorordarranq != null ? hop.Hophorordarranq.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;
                    hop.HophorparadaDesc = hop.Hophorparada != null ? hop.Hophorparada.Value.ToString(ConstantesAppServicio.FormatoHHmmss) : string.Empty;

                    if (consultarOtros)
                    {
                        var listaEquicodi = listaHOP.Where(x => x.Hopcodipadre == hopcodipadre && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).Select(x => x.Equicodi).Distinct().ToList();

                        foreach (var equicodi in listaEquicodi)
                        {
                            equipoHop = listaEquipoHop.Find(x => x.Equicodi == equicodi);

                            listaHOPDia = listaHOP.Where(x => x.Hopcodipadre == hopcodipadre && x.Equicodi == equicodi && x.Hophorini.Value.Date == f).OrderBy(x => x.Hophorini.Value).ToList();

                            #region Validación Alerta Ems

                            if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarEMS))
                            {
                                MeMedicion48DTO regEmsXEqXDia = listaEmsCompleta.Where(x => x.Equicodi == equicodi.Value && x.Medifecha == f).FirstOrDefault();

                                if (equicodi == 12779)
                                { }
                                if (regEmsXEqXDia != null)
                                {
                                    foreach (var regHop in listaHOPDia)
                                    {
                                        int indiceIni = HorasOperacionAppServicio.GetPosicionHoraInicial48Validaciones(regHop.Hophorini.Value);
                                        int indiceFin = HorasOperacionAppServicio.GetPosicionHoraFinal48Validaciones(regHop.Hophorfin.Value);
                                        indiceFin = fechaActual48 < regHop.Hophorfin ? indiceH48Actual : indiceFin;

                                        //si existe hora
                                        if (indiceFin >= indiceIni)
                                        {
                                            bool validacionNoOpero = false;
                                            int contadorEmsData0 = 0;
                                            ResultadoValidacionAplicativo regNuevo = null;
                                            DateTime? fini = null, ffin = null;
                                            for (var z = indiceIni; z <= indiceFin; z++)
                                            {
                                                if (1 <= z && z <= 48)
                                                {
                                                    decimal? valor = regEmsXEqXDia != null ? (decimal?)regEmsXEqXDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString())
                                                    .GetValue(regEmsXEqXDia, null) : null;
                                                    validacionNoOpero = valor.GetValueOrDefault(0) == 0;

                                                    if (validacionNoOpero)
                                                    {
                                                        if (contadorEmsData0 == 0)
                                                        {
                                                            regNuevo = new ResultadoValidacionAplicativo();
                                                            fini = f.Date.AddMinutes((z - 1) * 30);
                                                            ffin = f.Date.AddMinutes((z) * 30);
                                                        }
                                                        else
                                                        {
                                                            ffin = f.Date.AddMinutes((z) * 30);
                                                        }
                                                        regNuevo.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString()).SetValue(regNuevo, valor);
                                                        contadorEmsData0++;
                                                    }

                                                    if (!validacionNoOpero && regNuevo != null) //no hay scada pero si hop
                                                    {
                                                        regNuevo.Equicodi = equicodi.Value;
                                                        regNuevo.Equinomb = equipoHop != null ? equipoHop.Equinomb : string.Empty;
                                                        regNuevo.Medifecha = f;
                                                        regNuevo.FechaIni = fini.Value;
                                                        regNuevo.FechaFin = ffin.Value;
                                                        regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                        regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                        regNuevo.HoraOperacion = hop;
                                                        regNuevo.Hopcodi = regHop.Hopcodi;
                                                        regNuevo.Hopcodipadre = hopcodipadre;

                                                        listaEmsFinal.Add(regNuevo);

                                                        fini = null;
                                                        ffin = null;
                                                        contadorEmsData0 = 0;
                                                        regNuevo = null;
                                                    }
                                                }
                                            }

                                            if (validacionNoOpero && regNuevo != null) //no hay scada pero si hop, cuando no se cierra las fechas
                                            {
                                                regNuevo.Equicodi = equicodi.Value;
                                                regNuevo.Equinomb = equipoHop != null ? equipoHop.Equinomb : string.Empty;
                                                regNuevo.Medifecha = f;
                                                regNuevo.FechaIni = fini.Value;
                                                regNuevo.FechaFin = ffin.Value;
                                                regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                regNuevo.HoraOperacion = hop;
                                                regNuevo.Hopcodi = regHop.Hopcodi;
                                                regNuevo.Hopcodipadre = hopcodipadre;

                                                listaEmsFinal.Add(regNuevo);
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Validación Alerta Scada

                            if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarScada))
                            {
                                MeMedicion96DTO regScadaXEqXDia = listaScada.Where(x => x.Equicodi == equicodi.Value && x.Medifecha == f).FirstOrDefault();

                                if (regScadaXEqXDia != null)
                                {
                                    foreach (var regHop in listaHOPDia)
                                    {
                                        int indiceIni = HorasOperacionAppServicio.GetPosicionHoraInicial96Validaciones(regHop.Hophorini.Value);
                                        int indiceFin = HorasOperacionAppServicio.GetPosicionHoraFinal96Validaciones(regHop.Hophorfin.Value);
                                        indiceFin = fechaActual96 < regHop.Hophorfin ? indiceH96Actual : indiceFin;

                                        //si existe hora
                                        if (indiceFin >= indiceIni)
                                        {
                                            bool validacionNoOpero = false;
                                            int contadorScadaData0 = 0;
                                            ResultadoValidacionAplicativo regNuevo = null;
                                            DateTime? fini = null, ffin = null;
                                            for (var z = indiceIni; z <= indiceFin; z++)
                                            {
                                                if (1 <= z && z <= 96)
                                                {
                                                    decimal? valor = regScadaXEqXDia != null ? (decimal?)regScadaXEqXDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString())
                                                        .GetValue(regScadaXEqXDia, null) : null;
                                                    validacionNoOpero = valor.GetValueOrDefault(0) == 0;

                                                    if (validacionNoOpero)
                                                    {
                                                        if (contadorScadaData0 == 0)
                                                        {
                                                            regNuevo = new ResultadoValidacionAplicativo();
                                                            fini = f.Date.AddMinutes((z - 1) * 15);
                                                            ffin = f.Date.AddMinutes((z) * 15);
                                                        }
                                                        else
                                                        {
                                                            ffin = f.Date.AddMinutes((z) * 15);
                                                        }
                                                        regNuevo.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString()).SetValue(regNuevo, valor);
                                                        contadorScadaData0++;
                                                    }

                                                    if (!validacionNoOpero && regNuevo != null) //no hay scada pero si hop
                                                    {
                                                        regNuevo.Equicodi = equicodi.Value;
                                                        regNuevo.Equinomb = equipoHop != null ? equipoHop.Equinomb : string.Empty;
                                                        regNuevo.Canalcodi = regScadaXEqXDia != null ? regScadaXEqXDia.Canalcodi : -1;
                                                        regNuevo.Medifecha = f;
                                                        regNuevo.FechaIni = fini.Value;
                                                        regNuevo.FechaFin = ffin.Value;
                                                        regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                        regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                        regNuevo.HoraOperacion = hop;
                                                        regNuevo.Hopcodi = regHop.Hopcodi;
                                                        regNuevo.Hopcodipadre = hopcodipadre;

                                                        listaScadaFinal.Add(regNuevo);

                                                        fini = null;
                                                        ffin = null;
                                                        contadorScadaData0 = 0;
                                                        regNuevo = null;
                                                    }
                                                }
                                            }

                                            if (validacionNoOpero && regNuevo != null) //no hay scada pero si hop, cuando no se cierra las fechas
                                            {
                                                regNuevo.Equicodi = equicodi.Value;
                                                regNuevo.Equinomb = equipoHop != null ? equipoHop.Equinomb : string.Empty;
                                                regNuevo.Canalcodi = regScadaXEqXDia != null ? regScadaXEqXDia.Canalcodi : -1;
                                                regNuevo.Medifecha = f;
                                                regNuevo.FechaIni = fini.Value;
                                                regNuevo.FechaFin = ffin.Value;
                                                regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);
                                                regNuevo.HoraOperacion = hop;
                                                regNuevo.Hopcodi = regHop.Hopcodi;
                                                regNuevo.Hopcodipadre = hopcodipadre;

                                                listaScadaFinal.Add(regNuevo);
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Validación Alerta Intervenciones

                            if (listaValidacionPermitida.Contains(ConstantesHorasOperacion.ValidarIntervenciones))
                            {
                                List<InIntervencionDTO> listaIntervXEqXDia = listaIntervencion.Where(x => x.Equicodi == equicodi.Value && x.Interfechaini.Date == f).ToList();
                                listaIntervXEqXDia = IntervencionesAppServicio.GetListaEveManttoFragmentada(listaIntervXEqXDia, listaHOPDia);

                                InIntervencionDTO regIntervXEqXDia = listaIntervXEqXDia.FirstOrDefault();

                                foreach (var regHop in listaHOPDia)
                                {
                                    List<InIntervencionDTO> listaInterv = listaIntervXEqXDia.Where(x => x.Interfechaini >= regHop.Hophorini && x.Interfechafin <= regHop.Hophorfin).ToList();
                                    List<int> listarIntercodi = listaInterv.Select(x => x.Intercodi).Distinct().ToList();

                                    foreach (var intercodi in listarIntercodi)
                                    {
                                        List<InIntervencionDTO> listaByIntercodi = listaInterv.Where(x => x.Intercodi == intercodi).OrderBy(x => x.Interfechaini).ToList();
                                        var listTmpInt = IntervencionesAppServicio.GetListaEveManttoUnificada(listaByIntercodi);

                                        foreach (var regInt in listTmpInt)
                                        {
                                            ResultadoValidacionAplicativo regNuevo = new ResultadoValidacionAplicativo
                                            {
                                                Equicodi = equicodi.Value,
                                                Equinomb = equipoHop != null ? equipoHop.Equinomb : string.Empty,
                                                Medifecha = f,
                                                FechaIni = regInt.Interfechaini,
                                                FechaFin = regInt.Interfechafin,
                                                FechaIniDesc = regInt.Interfechaini.ToString(ConstantesAppServicio.FormatoHHmmss),
                                                FechaFinDesc = regInt.Interfechafin.ToString(ConstantesAppServicio.FormatoHHmmss),
                                                HoraOperacion = hop,
                                                Hopcodi = regHop.Hopcodi,
                                                Hopcodipadre = hopcodipadre,
                                                Tipoevenabrev = regInt.Tipoevenabrev,
                                                Interdescrip = regInt.Interdescrip,
                                                Interindispo = regInt.Interindispo,
                                                UltimaModificacionUsuarioDesc = regInt.UltimaModificacionUsuarioDesc,
                                                UltimaModificacionFechaDesc = regInt.UltimaModificacionFechaDesc
                                            };

                                            listaIntervFinal.Add(regNuevo);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
            }

            listaEmsFinal = listaEmsFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equiabrev).ThenBy(x => x.FechaIni).ToList();
            listaScadaFinal = listaScadaFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equiabrev).ThenBy(x => x.FechaIni).ToList();
            listaIntervFinal = listaIntervFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equiabrev).ThenBy(x => x.FechaIni).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="listaHOP"></param>
        /// <param name="listaScadaCompleta"></param>
        /// <param name="listaScadaFinal"></param>
        private void ListarHorasOperacionUnidadesNoRegistradosScada(DateTime fechaIni, List<EveHoraoperacionDTO> listaHOP, List<MeMedicion96DTO> listaScadaCompleta
            , out List<ResultadoValidacionAplicativo> listaScadaFinal)
        {
            listaScadaFinal = new List<ResultadoValidacionAplicativo>();

            DateTime fechaActual96 = DateTime.Now.AddMinutes(-15);
            int indiceH96Actual = Util.GetPosicionHoraInicial96(fechaActual96)[0];

            List<MeMedicion96DTO> listaScada = listaScadaCompleta.Where(x => x.Meditotal > 0).ToList();
            //List<MeMedicion96DTO> listaScada = listaScadaCompleta.ToList();
            List<int> listaEquicodi = listaScada.Select(x => x.Equicodi).Distinct().ToList();

            //listaScadaCompleta = listaScadaCompleta.Where(p => p.Central == null).ToList();

            foreach (var equicodi in listaEquicodi)
            {
                if (equicodi == 12713)
                { }
                for (var f = fechaIni.Date; f <= fechaIni; f = f.AddDays(1))
                {
                    List<EveHoraoperacionDTO> listaHOPDia = listaHOP.Where(x => x.Equicodi == equicodi && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad && x.Hophorini.Value.Date == f).OrderBy(x => x.Hophorini.Value).ToList();
                    //listaHOPDia = this.ListarHO15minEms(listaHOPDia, fechaIni);
                    listaHOPDia = this.ListarHO15min(listaHOPDia, fechaIni);
                    MeMedicion96DTO regEmsXEqXDia = listaScadaCompleta.Where(x => x.Equicodi == equicodi && x.Medifecha == f).FirstOrDefault();

                    bool validacionOperoSinHo = false;
                    int contadorEmsData0 = 0;
                    ResultadoValidacionAplicativo regNuevo = null;
                    DateTime? fini = null, ffin = null;

                    int hMax = fechaActual96 < fechaIni ? indiceH96Actual : 96;
                    for (var z = 1; z <= hMax; z++)
                    {
                        if (z == 96 && equicodi == 13604)
                        { }

                        DateTime fi = f.AddMinutes(z * 15);
                        bool tieneHop = listaHOPDia.Find(x => x.Equicodi == equicodi && x.HoraIni96 <= fi && fi <= x.HoraFin96) != null;

                        decimal? valor = regEmsXEqXDia != null ? (decimal?)regEmsXEqXDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString())
                            .GetValue(regEmsXEqXDia, null) : null;
                        validacionOperoSinHo = !tieneHop && (valor.GetValueOrDefault(0) > 0 && valor.GetValueOrDefault(0) != 2);//no tiene hora de operacion y tiene generación no ficticia

                        if (validacionOperoSinHo)
                        {
                            if (contadorEmsData0 == 0)
                            {
                                regNuevo = new ResultadoValidacionAplicativo();
                                fini = f.Date.AddMinutes((z - 1) * 15);
                                ffin = f.Date.AddMinutes((z) * 15);
                            }
                            else
                            {
                                ffin = f.Date.AddMinutes((z) * 15);
                            }
                            regNuevo.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString()).SetValue(regNuevo, 1.0m);
                            contadorEmsData0++;
                        }

                        if (!validacionOperoSinHo && regNuevo != null) //no hay scada pero si hop
                        {
                            regNuevo.FechaIni = fini.Value;
                            regNuevo.FechaFin = ffin.Value;
                            regNuevo.Central = regEmsXEqXDia.Central;
                            regNuevo.Emprnomb = regEmsXEqXDia.Emprnomb;
                            regNuevo.Equicodi = regEmsXEqXDia.Equicodi;
                            regNuevo.Equipadre = regEmsXEqXDia.Equipadre;
                            regNuevo.Equiabrev = regEmsXEqXDia.Equiabrev;
                            regNuevo.Equinomb = regEmsXEqXDia.Equinomb;
                            regNuevo.Famcodipadre = regEmsXEqXDia.Famcodi;
                            regNuevo.Emprcodi = regEmsXEqXDia.Emprcodi;
                            regNuevo.Equipadre = regEmsXEqXDia.Equipadre;
                            regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                            regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);

                            if (regNuevo.Equiabrev == "CHILCA1_TG11")
                            {
                                regNuevo.Equiabrev = regNuevo.Equiabrev;
                            }

                            listaScadaFinal.Add(regNuevo);

                            fini = null;
                            ffin = null;
                            contadorEmsData0 = 0;
                            regNuevo = null;
                        }
                    }

                    if (validacionOperoSinHo && regNuevo != null) //no hay scada pero si hop, cuando no se cierra las fechas
                    {
                        regNuevo.FechaIni = fini.Value;
                        regNuevo.FechaFin = ffin.Value;
                        regNuevo.Central = regEmsXEqXDia.Central;
                        regNuevo.Emprnomb = regEmsXEqXDia.Emprnomb;
                        regNuevo.Equicodi = regEmsXEqXDia.Equicodi;
                        regNuevo.Equipadre = regEmsXEqXDia.Equipadre;
                        regNuevo.Equiabrev = regEmsXEqXDia.Equiabrev;
                        regNuevo.Equinomb = regEmsXEqXDia.Equinomb;
                        regNuevo.Famcodipadre = regEmsXEqXDia.Famcodi;
                        regNuevo.Emprcodi = regEmsXEqXDia.Emprcodi;
                        regNuevo.Equipadre = regEmsXEqXDia.Equipadre;
                        regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                        regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);

                        if (regNuevo.Equiabrev == "CHILCA1_TG11")
                        {
                            regNuevo.Equiabrev = regNuevo.Equiabrev;
                        }

                        listaScadaFinal.Add(regNuevo);
                    }
                }
            }

            listaScadaFinal = listaScadaFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equiabrev).ThenBy(x => x.FechaIni).ToList();
        }

        /// <summary>
        /// Listar las Unidades reportadas por el EMS que no tienen Horas de Operación
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="listaHOP"></param>
        /// <param name="listaEmsCompleta"></param>
        /// <param name="listaEmsFinal"></param>
        private void ListarHorasOperacionUnidadesNoRegistrados(DateTime fechaIni, List<EveHoraoperacionDTO> listaHOP, List<MeMedicion48DTO> listaEmsCompleta
            , out List<ResultadoValidacionAplicativo> listaEmsFinal)
        {
            listaEmsFinal = new List<ResultadoValidacionAplicativo>();

            DateTime fechaActual48 = DateTime.Now.AddMinutes(-30);
            int indiceH48Actual = Util.GetPosicionHoraInicial48(fechaActual48)[0];

            List<MeMedicion48DTO> listaEms = listaEmsCompleta.Where(x => x.Meditotal > 0).ToList();

            List<int> listaEquicodi = listaEms.Select(x => x.Equicodi).Distinct().ToList();

            foreach (var equicodi in listaEquicodi)
            {
                if (equicodi == 12713)
                { }
                for (var f = fechaIni.Date; f <= fechaIni; f = f.AddDays(1))
                {
                    List<EveHoraoperacionDTO> listaHOPDia = listaHOP.Where(x => x.Equicodi == equicodi && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad && x.Hophorini.Value.Date == f).OrderBy(x => x.Hophorini.Value).ToList();
                    listaHOPDia = this.ListarHO30minEms(listaHOPDia, fechaIni);
                    MeMedicion48DTO regEmsXEqXDia = listaEmsCompleta.Where(x => x.Equicodi == equicodi && x.Medifecha == f).FirstOrDefault();

                    bool validacionOperoSinHo = false;
                    int contadorEmsData0 = 0;
                    ResultadoValidacionAplicativo regNuevo = null;
                    DateTime? fini = null, ffin = null;

                    int hMax = fechaActual48 < fechaIni ? indiceH48Actual : 48;
                    for (var z = 1; z <= hMax; z++)
                    {
                        if (z == 48 && equicodi == 13604)
                        { }

                        DateTime fi = f.AddMinutes(z * 30);
                        bool tieneHop = listaHOPDia.Find(x => x.Equicodi == equicodi && x.HoraIni48 <= fi && fi <= x.HoraFin48) != null;

                        decimal? valor = regEmsXEqXDia != null ? (decimal?)regEmsXEqXDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString())
                            .GetValue(regEmsXEqXDia, null) : null;
                        validacionOperoSinHo = !tieneHop && (valor.GetValueOrDefault(0) > 0 && valor.GetValueOrDefault(0) != 2);//no tiene hora de operacion y tiene generación no ficticia

                        if (validacionOperoSinHo)
                        {
                            if (contadorEmsData0 == 0)
                            {
                                regNuevo = new ResultadoValidacionAplicativo();
                                fini = f.Date.AddMinutes((z - 1) * 30);
                                ffin = f.Date.AddMinutes((z) * 30);
                            }
                            else
                            {
                                ffin = f.Date.AddMinutes((z) * 30);
                            }
                            regNuevo.GetType().GetProperty(ConstantesAppServicio.CaracterH + z.ToString()).SetValue(regNuevo, 1.0m);
                            contadorEmsData0++;
                        }

                        if (!validacionOperoSinHo && regNuevo != null) //no hay scada pero si hop
                        {
                            regNuevo.FechaIni = fini.Value;
                            regNuevo.FechaFin = ffin.Value;
                            regNuevo.Central = regEmsXEqXDia.Central;
                            regNuevo.Emprnomb = regEmsXEqXDia.Emprnomb;
                            regNuevo.Equicodi = regEmsXEqXDia.Equicodi;
                            regNuevo.Equiabrev = regEmsXEqXDia.Equiabrev;
                            regNuevo.Equinomb = regEmsXEqXDia.Equinomb;
                            regNuevo.Famcodipadre = regEmsXEqXDia.Famcodi;
                            regNuevo.Emprcodi = regEmsXEqXDia.Emprcodi;
                            regNuevo.Equipadre = regEmsXEqXDia.Equipadre;
                            regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                            regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);

                            listaEmsFinal.Add(regNuevo);

                            fini = null;
                            ffin = null;
                            contadorEmsData0 = 0;
                            regNuevo = null;
                        }
                    }

                    if (validacionOperoSinHo && regNuevo != null) //no hay scada pero si hop, cuando no se cierra las fechas
                    {
                        regNuevo.FechaIni = fini.Value;
                        regNuevo.FechaFin = ffin.Value;
                        regNuevo.Central = regEmsXEqXDia.Central;
                        regNuevo.Emprnomb = regEmsXEqXDia.Emprnomb;
                        regNuevo.Equicodi = regEmsXEqXDia.Equicodi;
                        regNuevo.Equiabrev = regEmsXEqXDia.Equiabrev;
                        regNuevo.Equinomb = regEmsXEqXDia.Equinomb;
                        regNuevo.Famcodipadre = regEmsXEqXDia.Famcodi;
                        regNuevo.FechaIniDesc = regNuevo.FechaIni.ToString(ConstantesAppServicio.FormatoHHmmss);
                        regNuevo.FechaFinDesc = regNuevo.FechaFin.ToString(ConstantesAppServicio.FormatoHHmmss);

                        listaEmsFinal.Add(regNuevo);
                    }
                }
            }

            listaEmsFinal = listaEmsFinal.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equiabrev).ThenBy(x => x.FechaIni).ToList();
        }

        /// <summary>
        /// Lista de Unidades No Registradas
        /// </summary>
        /// <param name="listaData"></param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarUnidadesNoRegistradas(List<ResultadoValidacionAplicativo> listaData)
        {
            List<EqEquipoDTO> lista = listaData.GroupBy(x => x.Equicodi)
                .Select(grp => new EqEquipoDTO
                {
                    Emprnomb = grp.First().Emprnomb,
                    Emprcodi = grp.First().Emprcodi,
                    Central = grp.First().Central,
                    Equipadre = grp.First().Equipadre,
                    Equicodi = grp.Key,
                    Equinomb = grp.First().Equinomb,
                    Equiabrev = grp.First().Equiabrev
                })
                .ToList();

            return lista;
        }

        /// <summary>
        /// Verifica si la Hora de Operación tiene alarma EMS
        /// </summary>
        /// <param name="horaOp"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public bool VerificaAlarmaHo(EveHoraoperacionDTO horaOp, List<ResultadoValidacionAplicativo> lista)
        {
            return lista.Where(x => x.Hopcodipadre == horaOp.Hopcodi).Count() > 0;
        }

        /// <summary>
        /// Data Ems incluyendo las unidades de los equipos de los modos
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaGeneracionEmsCompleta(DateTime fechaIni, DateTime fechaFin, string empresa)
        {
            List<MeMedicion48DTO> listaEmsFinal = new List<MeMedicion48DTO>();

            List<int> listaEmprcodi = empresa.Split(',').Select(x => int.Parse(x)).ToList();

            string famcodis = "2,3,4,5,36,37,38,39";
            List<CmGeneracionEmsDTO> listaEmsTotal = FactorySic.GetCmGeneracionEmsRepository().GetListaGeneracionByEquipoFecha(fechaIni, fechaFin, "-2", famcodis);

            List<EqRelacionDTO> listaEqRelacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion);
            listaEqRelacion = listaEqRelacion.Where(x => ConstantesHorasOperacion.ParamEmpresaTodos == empresa || listaEmprcodi.Contains(x.Emprcodi)).ToList();

            for (var day = fechaIni.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                List<CmGeneracionEmsDTO> listaEmsXDia = listaEmsTotal.Where(x => x.Genemsfecha.Date == day && (ConstantesHorasOperacion.ParamEmpresaTodos == empresa || listaEmprcodi.Contains(x.Emprcodi))).ToList();
                var listaEquicodi = listaEmsXDia.Select(x => x.Equicodi).Distinct().ToList();

                //Los equipos que no tienen ems se genera data
                foreach (var regEq in listaEqRelacion)
                {
                    if (listaEmsXDia.Find(x => x.Equicodi == regEq.Equicodi) == null)
                    {
                        MeMedicion48DTO c = new MeMedicion48DTO
                        {
                            Medifecha = day,
                            Equicodi = regEq.Equicodi.GetValueOrDefault(-1),
                            Equinomb = regEq.Equinomb,
                            Equiabrev = regEq.Equinomb,
                            //c.Central = regEq.Central;
                            Emprnomb = regEq.Emprnomb,
                            Famcodi = regEq.Famcodi,
                            Tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva
                        };

                        listaEmsFinal.Add(c);
                    }
                }

                //La data existente completar las medias horas restantes
                foreach (var equicodi in listaEquicodi)
                {
                    var regEq = listaEmsXDia.Find(x => x.Equicodi == equicodi);
                    MeMedicion48DTO c = new MeMedicion48DTO
                    {
                        Medifecha = day,
                        Equicodi = regEq.Equicodi,
                        Equinomb = regEq.Equinomb,
                        Equiabrev = regEq.Equiabrev,
                        Central = regEq.Central,
                        Emprnomb = regEq.Emprnomb,
                        Famcodi = regEq.Famcodipadre,
                        Tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva
                    };

                    List<CmGeneracionEmsDTO> listaEmsDia = listaEmsXDia.Where(x => x.Equicodi == equicodi).OrderBy(x => x.Genemsfecha).ToList();

                    var regPrimerEms = listaEmsDia[0];

                    decimal? valorAnt = 0.0m;
                    decimal? valorAct = 0.0m;
                    for (int h = 1; h <= 48; h++)
                    {
                        var ems30 = listaEmsDia.Where(x => x.Genemsfecha == day.AddMinutes(h * 30)).FirstOrDefault();
                        var ems2359 = listaEmsDia.Where(x => x.Genemsfecha == day.AddMinutes(h * 30).AddMinutes(-1)).FirstOrDefault();

                        valorAct = 0.0m;
                        if (ems30 != null)
                        {
                            valorAct = (ems30.Genemsoperativo > 0 && ems30.Genemsgeneracion > 0) ? ems30.Genemsgeneracion : 0.0m;
                        }
                        if (ems2359 != null)
                        {
                            valorAct = (ems2359.Genemsoperativo > 0 && ems2359.Genemsgeneracion > 0) ? ems2359.Genemsgeneracion : 0.0m;
                        }
                        decimal? valorH = valorAct > 0 ? valorAct : (valorAnt > 0 ? 2 : 0); //si la anterior media hora estuvo el equipo Operativo y con generación, posiblemente estuvo generanado entre 0 y29 minutos de la H actual
                        c.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(c, valorH);

                        valorAnt = valorAct;
                    }

                    listaEmsFinal.Add(c);
                }

                //Generar total para verificar si algun h tiene estado operativo
                foreach (var m48 in listaEmsFinal)
                {
                    decimal? valor = null;
                    decimal total = 0;
                    List<decimal> listaH = new List<decimal>();

                    for (int h = 1; h <= 48; h++)
                    {
                        valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        if (valor != null)
                        {
                            listaH.Add(valor.Value);
                        }
                    }

                    if (listaH.Count > 0)
                    {
                        total = listaH.Sum(x => x);
                    }

                    m48.Meditotal = total;
                }
            }

            return listaEmsFinal;
        }

        /// <summary>
        /// Data Ems incluyendo las unidades de los equipos de los modos
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresa"></param>
        /// <param name="famcodi"></param>
        /// <param name="umbralMinimo"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaGeneracionEmsCompleta2(DateTime fechaIni, DateTime fechaFin, string empresa, string famcodi, decimal umbralMinimo)
        {
            List<MeMedicion48DTO> listaEmsFinal = new List<MeMedicion48DTO>();

            List<int> listaEmprcodi = empresa.Split(',').Select(x => int.Parse(x)).ToList();

            List<CmGeneracionEmsDTO> listaEmsTotal = FactorySic.GetCmGeneracionEmsRepository().GetListaGeneracionByEquipoFecha(fechaIni, fechaFin, empresa, famcodi);

            List<EqRelacionDTO> listaEqRelacion = FactorySic.GetEqRelacionRepository().ObtenerConfiguracionProceso(ConstantesCortoPlazo.FuenteGeneracion, famcodi);
            listaEqRelacion = listaEqRelacion.Where(x => ConstantesHorasOperacion.ParamEmpresaTodos == empresa || listaEmprcodi.Contains(x.Emprcodi)).ToList();

            for (var day = fechaIni.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                List<CmGeneracionEmsDTO> listaEmsXDia = listaEmsTotal.Where(x => x.Genemsfecha.Date == day && (ConstantesHorasOperacion.ParamEmpresaTodos == empresa || listaEmprcodi.Contains(x.Emprcodi))).ToList();
                var listaEquicodi = listaEmsXDia.Select(x => x.Equicodi).Distinct().ToList();

                //Los equipos que no tienen ems se genera data
                foreach (var regEq in listaEqRelacion)
                {
                    if (listaEmsXDia.Find(x => x.Equicodi == regEq.Equicodi) == null)
                    {
                        MeMedicion48DTO c = new MeMedicion48DTO
                        {
                            Medifecha = day,
                            Equicodi = regEq.Equicodi.GetValueOrDefault(-1),
                            Equinomb = regEq.Equinomb,
                            Equiabrev = regEq.Equinomb,
                            //c.Central = regEq.Central;
                            Emprnomb = regEq.Emprnomb,
                            Famcodi = regEq.Famcodi,
                            Emprcodi = regEq.Emprcodi,
                            Equipadre = regEq.Equipadre ?? 0,
                            Tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva
                        };

                        listaEmsFinal.Add(c);
                    }
                }

                //La data existente completar las medias horas restantes
                foreach (var equicodi in listaEquicodi)
                {
                    var regEq = listaEmsXDia.Find(x => x.Equicodi == equicodi);
                    MeMedicion48DTO c = new MeMedicion48DTO
                    {
                        Medifecha = day,
                        Equicodi = regEq.Equicodi,
                        Equinomb = regEq.Equinomb,
                        Equiabrev = regEq.Equiabrev,
                        Central = regEq.Central,
                        Emprnomb = regEq.Emprnomb,
                        Famcodi = regEq.Famcodipadre,
                        Emprcodi = regEq.Emprcodi,
                        Equipadre = regEq.Equipadre,
                        Tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva
                    };

                    List<CmGeneracionEmsDTO> listaEmsDia = listaEmsXDia.Where(x => x.Equicodi == equicodi).OrderBy(x => x.Genemsfecha).ToList();

                    var regPrimerEms = listaEmsDia[0];

                    decimal? valorAnt = 0.0m;
                    decimal? valorAct = 0.0m;
                    for (int h = 1; h <= 48; h++)
                    {
                        var ems30 = listaEmsDia.Where(x => x.Genemsfecha == day.AddMinutes(h * 30)).FirstOrDefault();
                        var ems2359 = listaEmsDia.Where(x => x.Genemsfecha == day.AddMinutes(h * 30).AddMinutes(-1)).FirstOrDefault();

                        valorAct = 0.0m;
                        if (ems30 != null)
                        {
                            valorAct = (ems30.Genemsoperativo > 0 && ems30.Genemsgeneracion > 0) ? ems30.Genemsgeneracion : 0.0m;
                        }
                        if (ems2359 != null)
                        {
                            valorAct = (ems2359.Genemsoperativo > 0 && ems2359.Genemsgeneracion > 0) ? ems2359.Genemsgeneracion : 0.0m;
                        }
                        decimal? valorH = valorAct > 0 ? valorAct : (valorAnt > 0 ? 2 : 0); //si la anterior media hora estuvo el equipo Operativo y con generación, posiblemente estuvo generanado entre 0 y29 minutos de la H actual
                        c.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(c, valorH);

                        valorAnt = valorAct;
                    }

                    listaEmsFinal.Add(c);
                }

                //Generar total para verificar si algun h tiene estado operativo
                foreach (var m48 in listaEmsFinal)
                {
                    decimal? valor = null;
                    decimal total = 0;
                    List<decimal> listaH = new List<decimal>();

                    for (int h = 1; h <= 48; h++)
                    {
                        //valor BD
                        valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);

                        //solo considerar valores numéricos mayor o igual al umbral
                        if (valor.GetValueOrDefault(0) < umbralMinimo) valor = null;
                        m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(m48, valor);

                        if (valor != null)
                        {
                            listaH.Add(valor.Value);
                        }
                    }

                    if (listaH.Count > 0)
                    {
                        total = listaH.Sum(x => x);
                    }

                    m48.Meditotal = total;
                }
            }

            return listaEmsFinal;
        }

        /// <summary>
        /// Data Ems incluyendo las unidades de los equipos de los modos
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> GetListaFlujoPotenciaEmsCompleta(DateTime fechaIni, string empresa)
        {
            List<MeMedicion48DTO> listaEmsFinal = new List<MeMedicion48DTO>();

            DateTime f = fechaIni.Date;
            DateTime fahora = DateTime.Now;
            List<CmFlujoPotenciaDTO> listaEms = FactorySic.GetCmFlujoPotenciaRepository().ObtenerReporteFlujoPotencia(fechaIni, fechaIni);
            var listaEquicodi = listaEms.Select(x => x.Equicodi).Distinct().ToList();

            List<EqCongestionConfigDTO> listaLineas = FactorySic.GetEqCongestionConfigRepository().ObtenerListadoEquipos();

            //Los equipos que no tienen ems se genera data
            foreach (var equiv in listaLineas)
            {
                if (listaEms.Find(x => x.Equicodi == equiv.Equicodi) == null)
                {
                    MeMedicion48DTO reg = new MeMedicion48DTO
                    {
                        Medifecha = fechaIni.Date,
                        Equicodi = equiv.Equicodi,
                        Equinomb = equiv.Equinomb,
                        Equiabrev = equiv.Equinomb,
                        Emprnomb = equiv.Emprnomb,
                        Famcodi = equiv.Famcodi.GetValueOrDefault(-1),
                        Tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva
                    };

                    listaEmsFinal.Add(reg);
                }
            }

            //La data existente completar las medias horas restantes
            foreach (var equicodi in listaEquicodi)
            {
                var regEq = listaEms.Find(x => x.Equicodi == equicodi);
                MeMedicion48DTO c = new MeMedicion48DTO
                {
                    Medifecha = fechaIni.Date,
                    Equicodi = regEq.Equicodi,
                    Equinomb = regEq.Equinomb,
                    Equiabrev = regEq.Equiabrev,
                    Emprnomb = regEq.Emprnomb,
                    Famcodi = regEq.Famcodi,
                    Tipoinfocodi = ConstantesMedicion.IdTipoInfoPotenciaActiva
                };

                List<CmFlujoPotenciaDTO> listaEmsDia = listaEms.Where(x => x.Equicodi == equicodi).OrderBy(x => x.Flupotfecha).ToList();

                var regPrimerEms = listaEmsDia[0];

                decimal? valorAnt = 0.0m;
                decimal? valorAct = 0.0m;
                for (int h = 1; h <= 48; h++)
                {
                    var ems30 = listaEmsDia.Where(x => x.Flupotfecha == f.AddMinutes(h * 30)).FirstOrDefault();
                    var ems2359 = listaEmsDia.Where(x => x.Flupotfecha == f.AddMinutes(h * 30).AddMinutes(-1)).FirstOrDefault();

                    valorAct = 0.0m;
                    if (ems30 != null)
                    {
                        valorAct = (ems30.Flupotvalor > 0) ? 1.0m : 0.0m; //el campo FLUPOTOPERATIVO no se toma en cuenta porque tiene valores en NULL
                    }
                    if (ems2359 != null)
                    {
                        valorAct = (ems2359.Flupotvalor > 0 && ems2359.Flupotvalor > 0) ? 1.0m : 0.0m;
                    }
                    decimal? valorH = valorAct > 0 ? valorAct : (valorAnt > 0 ? 2 : 0); //si la anterior media hora estuvo el equipo Operativo y con potencia, posiblemente estuvo generanado entre 0 y 29 minutos de la H actual
                    c.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(c, valorH);

                    valorAnt = valorAct;
                }

                listaEmsFinal.Add(c);
            }

            //Generar total para verificar si algun h tiene estado operativo
            foreach (var m48 in listaEmsFinal)
            {
                decimal? valor = null;
                decimal total = 0;
                List<decimal> listaH = new List<decimal>();

                for (int h = 1; h <= 48; h++)
                {
                    valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    if (valor != null)
                    {
                        listaH.Add(valor.Value);
                    }
                }

                if (listaH.Count > 0)
                {
                    total = listaH.Sum(x => x);
                }

                m48.Meditotal = total;
            }

            return listaEmsFinal;
        }

        /// <summary>
        /// Data Scada para Termo
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="empresa"></param>
        /// <param name="umbralMinimo"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetListaGeneracionScadaCompletaByDespacho(DateTime fechaIni, string empresa, decimal umbralMinimo)
        {
            //equivalencia de Extranet IEOD despacho ejecutado
            List<MePtomedcanalDTO> listaPtoCanal = (new MedidoresAppServicio()).ListarEquivalenciaPtomedicionCanal((empresa == "-2" ? "-1" : empresa), 
                                    Int32.Parse(ConstantesPR5ReportesServicio.OrigLecturaPR05IEOD), 1, "3,5");

            List<MeMedicion96DTO> listaData = this.ConvertirMeScadaToMeMedicion96(FactoryScada.GetMeScadaSp7Repository().GetDatosScadaAFormato(ConstantesHard.IdFormatoDespacho, Convert.ToInt32(empresa), 
                                    fechaIni, fechaIni, "3,5", ConstantesTipoInformacion.TipoinfoMW.ToString()));

            return this.CompletarDataScadaSp7(fechaIni, listaPtoCanal, listaData, umbralMinimo);
        }

        /// <summary>
        /// Obtener data scada por equipos y tipo de información
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="listaEquicodiInput"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> GetListaGeneracionScadaCompletaByEquipos(DateTime fechaIni, List<int> listaEquicodiInput)
        {
            List<MePtomedcanalDTO> listaPtoCanal = (new MedidoresAppServicio()).ListarEquivalenciaPtomedicionCanal("-1", -1, -1, ConstantesAppServicio.ParametroDefecto)
                .Where(x => listaEquicodiInput.Contains(x.Equicodi)).ToList();

            List<MeMedicion96DTO> listaData = this.ConvertirMeScadaToMeMedicion96(FactoryScada.GetMeScadaSp7Repository().GetDatosScadaAFormato(-1, -2, fechaIni, fechaIni, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto)
                .Where(x => listaEquicodiInput.Contains(x.Equicodi)).ToList());

            return this.CompletarDataScadaSp7(fechaIni, listaPtoCanal, listaData, 0);
        }

        private List<MeMedicion96DTO> CompletarDataScadaSp7(DateTime fechaIni, List<MePtomedcanalDTO> listaPtoCanal, List<MeMedicion96DTO> listaData, decimal umbralMinimo)
        {
            //Los equipos que no tienen scada se genera data
            foreach (var equiv in listaPtoCanal)
            {
                if (listaData.Find(x => x.Equicodi == equiv.Equicodi && x.Tipoinfocodi == equiv.Tipoinfocodi) == null)
                {
                    MeMedicion96DTO reg = new MeMedicion96DTO
                    {
                        Medifecha = fechaIni.Date,
                        Equicodi = equiv.Equicodi,
                        Tipoinfocodi = equiv.Tipoinfocodi,
                        Canalcodi = equiv.Canalcodi,
                        Emprnomb = equiv.Emprnomb,
                        Central = equiv.Central,
                        Emprcodi = equiv.Emprcodi,
                        Equipadre = equiv.Equipadre,

                        Equinomb = equiv.Equinomb,
                        Equiabrev = equiv.Equiabrev
                    };

                    listaData.Add(reg);
                }
            }

            //Algunos equipos(puntos de medición) tienen doble señal scada como las lineas (lado A y lado B)
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();
            List<MeScadaSp7DTO> listaConf = listaPtoCanal.GroupBy(x => new { x.Equicodi, x.Tipoinfocodi })
                .Select(x => new MeScadaSp7DTO()
                {
                    Equicodi = x.Key.Equicodi,
                    Tipoinfocodi = x.Key.Tipoinfocodi,
                    Equinomb = x.First().Equinomb,
                    Emprcodi = x.First().Emprcodi,
                    Emprnomb = x.First().Emprnomb,
                    Central = x.First().Central,
                    Equipadre = x.First().Equipadre
                }).ToList();
            foreach (var regConf in listaConf)
            {
                List<MeMedicion96DTO> listaByEqAndTipoinfo = listaData.Where(x => x.Equicodi == regConf.Equicodi && x.Tipoinfocodi == regConf.Tipoinfocodi).ToList();
                if (listaByEqAndTipoinfo.Count() > 1)
                {
                    MeMedicion96DTO reg = new MeMedicion96DTO
                    {
                        Medifecha = fechaIni.Date,
                        Equicodi = regConf.Equicodi,
                        Tipoinfocodi = regConf.Tipoinfocodi,
                        Canalcodi = regConf.Canalcodi,
                        Emprnomb = regConf.Emprnomb,
                        Central = regConf.Central,
                        Emprcodi = regConf.Emprcodi,
                        Equipadre = regConf.Equipadre
                    };

                    for (int h = 1; h <= 96; h++)
                    {
                        decimal? valorHFinal = 0;
                        foreach (var regTmp in listaByEqAndTipoinfo)
                        {
                            decimal? valorHTmp = (decimal?)regTmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regTmp, null);
                            valorHTmp = Math.Abs(valorHTmp.GetValueOrDefault(0));
                            if (valorHTmp > valorHFinal)
                                valorHFinal = valorHTmp;
                        }

                        reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(reg, valorHFinal);
                    }
                }
                else
                {
                    listaFinal.AddRange(listaByEqAndTipoinfo);
                }
            }

            //Generar total para verificar si algun h tiene estado operativo
            foreach (var m96 in listaFinal)
            {
                decimal? valor = null;
                decimal total = 0;
                List<decimal> listaH = new List<decimal>();

                for (int h = 1; h <= 96; h++)
                {
                    //valor BD
                    valor = (decimal?)m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m96, null);

                    //solo considerar valores numéricos mayor o igual al umbral
                    if (valor.GetValueOrDefault(0) < umbralMinimo) valor = null;
                    m96.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(m96, valor);

                    if (valor != null)
                    {
                        listaH.Add(valor.Value);
                    }
                }

                if (listaH.Count > 0)
                {
                    total = listaH.Sum(x => x);
                }

                m96.Meditotal = total;
            }

            return listaFinal;
        }

        /// <summary>
        /// Convertir lista de scada lista de m96
        /// </summary>
        /// <param name="listaScada"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ConvertirMeScadaToMeMedicion96(List<MeScadaSp7DTO> listaScada)
        {
            List<MeMedicion96DTO> l = new List<MeMedicion96DTO>();
            foreach (var reg in listaScada)
            {
                l.Add(this.ConvertirMeScadaToMeMedicion96(reg));
            }

            return l;
        }

        /// <summary>
        /// convertir obj scada a m96
        /// </summary>
        /// <param name="regScada"></param>
        /// <returns></returns>
        private MeMedicion96DTO ConvertirMeScadaToMeMedicion96(MeScadaSp7DTO regScada)
        {
            MeMedicion96DTO reg = new MeMedicion96DTO
            {
                Medifecha = regScada.Medifecha,
                Meditotal = regScada.Meditotal,
                Ptomedicodi = regScada.Ptomedicodi,
                Equicodi = regScada.Equicodi,
                Equinomb = regScada.Equinomb,
                Tipoinfocodi = regScada.Tipoinfocodi,
                Canalcodi = regScada.Canalcodi,
                Emprnomb = regScada.Emprnomb,
                Central = regScada.Central,
                Equipadre = regScada.Equipadre,
                Emprcodi = regScada.Emprcodi,
                Equiabrev = regScada.Equiabrev,
                Famcodi = regScada.Famcodi
            };

            for (int h = 1; h <= 96; h++)
            {
                decimal? valorHTmp = (decimal?)regScada.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regScada, null);
                reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(reg, valorHTmp);
            }

            return reg;
        }

        /// <summary>
        /// Posicion H para validar información de Data 48
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private static int GetPosicionHoraInicial48Validaciones(DateTime fecha)
        {
            if (fecha.Hour == 0 && fecha.Minute == 0 && fecha.Second == 0)
                return 1;

            int indice = Util.GetPosicionHoraInicial48(fecha)[0];
            indice++;

            return indice;
        }

        /// <summary>
        /// Posicion H para validar información de Data 48
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private static int GetPosicionHoraFinal48Validaciones(DateTime fecha)
        {
            if (fecha.Hour == 0 && fecha.Minute == 0 && fecha.Second == 0)
                return 48;

            int indice = Util.GetPosicionHoraFinal48(fecha)[0];
            indice--;

            return indice;
        }

        /// <summary>
        /// Posicion H para validar información de Data 96
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private static int GetPosicionHoraInicial96Validaciones(DateTime fecha)
        {
            if (fecha.Hour == 0 && fecha.Minute == 0 && fecha.Second == 0)
                return 1;

            int indice = Util.GetPosicionHoraInicial96(fecha)[0];
            indice++;

            return indice;
        }

        /// <summary>
        /// Posicion H para validar información de Data 96
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private static int GetPosicionHoraFinal96Validaciones(DateTime fecha)
        {
            if (fecha.Hour == 0 && fecha.Minute == 0 && fecha.Second == 0)
                return 96;

            int indice = Util.GetPosicionHoraFinal96(fecha)[0];
            indice--;

            return indice;
        }

        /// <summary>
        /// ListarAlertaHoXDia
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaHorasProgramadas"></param>
        /// <param name="listaModosOperacion"></param>
        /// <returns></returns>
        public List<HOPAlerta> ListarAlertaHoXDia(DateTime fecha, List<EveHoraoperacionDTO> listaHorasOperacion,
                                            List<HorasProgramadasDTO> listaHorasProgramadas, List<PrGrupoDTO> listaModosOperacion)
        {
            var alertas = new List<HOPAlerta>();

            bool esTesting = true;
            bool sumarTminA = true;
            bool tieneHoraEjecucion = false;
            DateTime horaActual = DateTime.Now;

            if (esTesting)
            {
                horaActual = fecha;
                sumarTminA = false;
            }

            fecha = fecha.Date;

            // Contrastar HProgramadas con HEjecutadas
            List<EveHoraoperacionDTO> ho;
            PrGrupoDTO grupo;
            EveHoraoperacionDTO hoItem;
            DateTime horaInicio;
            DateTime horaFin;
            DateTime horaAlerta;
            DateTime horaPermitidaMin;
            DateTime horaPermitidaMax;

            string TminA = string.Empty;
            int? subCausaCodi = null;
            string descripcion = string.Empty;
            string hophorfinDesc = string.Empty;

            foreach (HorasProgramadasDTO hp in listaHorasProgramadas)
            {
                grupo = listaModosOperacion.SingleOrDefault(p => p.Grupocodi == hp.Grupocodi);

                if (grupo != null)
                {
                    hp.Empresa = grupo.EmprNomb;
                    hp.Central = grupo.Central;

                    ho = listaHorasOperacion.Where(p => p.Grupocodi == hp.Grupocodi).ToList();

                    tieneHoraEjecucion = ho.Any();

                    if (tieneHoraEjecucion)
                    {
                        hp.Hopcodi = ho.First().Hopcodi; //asignarle una hora de operación ejecutada para la edición múltiple

                        var fechaInicioEjec = ho.Max(p => p.Hophorfin).Value;

                        hp.EsValido = (fechaInicioEjec.Date == fecha && fechaInicioEjec < hp.FechaFin);
                        hp.EsEdicion = true;

                        TminA = ho.First().TMinA;
                        subCausaCodi = ho.First().Subcausacodi;
                        descripcion = ho.First().Hopdesc ?? string.Empty;
                        hophorfinDesc = ho.First().HophorfinDesc;
                    }
                    else
                    {
                        hp.EsValido = true;
                        TminA = string.Empty;
                        subCausaCodi = null;
                        descripcion = string.Empty;
                    }

                    hoItem = new EveHoraoperacionDTO()
                    {
                        Emprnomb = hp.Empresa,
                        Central = hp.Central,
                        Grupoabrev = hp.Gruponom,
                        TMinA = TminA,
                        Subcausacodi = subCausaCodi,
                        Hopdesc = descripcion,
                        HophorfinDesc = hophorfinDesc
                    };

                    horaInicio = hp.FechaInicio.AddSeconds(0);
                    horaFin = hp.FechaFin.AddSeconds(0);

                    if (!sumarTminA) { hoItem.TMinA = "0"; }

                    if (string.IsNullOrEmpty(hoItem.TMinA)) { hoItem.TMinA = "0"; }

                    // SF1
                    if (hp.EsValido)
                    {
                        horaAlerta = horaInicio.AddMinutes(-Convert.ToDouble(hoItem.TMinA));
                        horaPermitidaMin = horaAlerta.AddMinutes(-15);
                        horaPermitidaMax = horaAlerta;

                        if ((horaActual >= horaPermitidaMin && (horaActual <= horaPermitidaMax))
                                && (horaAlerta >= horaPermitidaMin && (horaAlerta <= horaPermitidaMax)))
                        {
                            alertas.Add(new HOPAlerta()
                            {
                                MsjAlerta = string.Format("Dar orden de arranque: {0} / {1}"
                                                                , hoItem.Central, hoItem.Grupoabrev),
                                FechaAlerta = horaAlerta,
                                Prioridad = 2,
                                CodAlerta = string.Format("F1-", hp.Grupocodi, horaAlerta)
                            });
                        }
                    }

                    // SF2 (alerta 15 minutos antes de que termine la hora de operación programada hasta que termine la hora programada)
                    if (tieneHoraEjecucion)
                    {
                        horaAlerta = horaFin.AddMinutes(-15);
                        horaPermitidaMin = horaAlerta;
                        horaPermitidaMax = horaFin;

                        if ((horaFin != horaFin.Date)
                                && (hoItem.HophorfinDesc != "00:00:00")
                                && (horaActual >= horaPermitidaMin && (horaActual <= horaPermitidaMax))
                                && (horaAlerta >= horaPermitidaMin && (horaAlerta <= horaPermitidaMax)))
                        {
                            alertas.Add(new HOPAlerta()
                            {
                                MsjAlerta = string.Format("Dar orden de parada: {0} / {1}"
                                                                , hoItem.Central, hoItem.Grupoabrev),
                                FechaAlerta = horaAlerta,
                                Prioridad = 2,
                                CodAlerta = string.Format("F2-", hp.Grupocodi, horaAlerta)
                            });
                        }
                    }
                }
            }

            // SF4
            foreach (EveHoraoperacionDTO item in listaHorasOperacion)
            {
                if (item.Subcausacodi == ConstantesSubcausaEvento.SubcausaPorPruebasAleatoriasPR25)
                {
                    try
                    {
                        // A las 20:00 h, alcanzó su máxima generación
                        string horaMin = item.Hopdesc.Replace("A las ", string.Empty).Replace("h, alcanzó su máxima generación", string.Empty);

                        int horaDesc = Convert.ToInt32(horaMin.Split(':')[0].Trim());
                        int minDesc = Convert.ToInt32(horaMin.Split(':')[1].Trim());
                        horaFin = horaActual.Date.AddHours(horaDesc).AddMinutes(minDesc);
                    }
                    catch
                    {
                        horaFin = DateTime.MinValue;
                    }

                    if (horaFin != DateTime.MinValue)
                    {
                        horaAlerta = horaFin.AddMinutes(120); // 2h + 15
                        horaPermitidaMin = horaFin.AddMinutes(105);
                        horaPermitidaMax = horaFin.AddMinutes(120);

                        if ((horaFin != horaFin.Date)
                            && (horaActual >= horaPermitidaMin && (horaActual <= horaPermitidaMax))
                            && (horaAlerta >= horaPermitidaMin && (horaAlerta <= horaPermitidaMax)))
                        {
                            alertas.Add(new HOPAlerta()
                            {
                                MsjAlerta = string.Format("Fin de prueba PR-25: Parar la unidad o bajar a potencia mínima: {0} / {1} / {2}"
                                                            , item.Emprnomb, item.Central, item.Grupoabrev),

                                FechaAlerta = horaAlerta,
                                Prioridad = 1,
                                CodAlerta = string.Format("F4-", item.Grupocodi, horaAlerta)
                            });
                        }
                    }
                }
            }

            /*
            //bloques válidos
            listaHorasProgramadas = listaHorasProgramadas.Where(p => p.EsValido).ToList();

            //modos de operación de los bloques válidos
            List<int> lgrupocodiValido = listaHorasProgramadas.Select(x => x.Grupocodi).ToList();
            listaModoProgramada = listaModoProgramada.Where(x => lgrupocodiValido.Contains(x.Grupocodi))
                    .OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Gruponomb).ToList();
            */

            alertas = alertas.OrderBy(p => p.Prioridad).ThenBy(p => p.FechaAlerta).ToList();

            foreach (var item in alertas)
            {
                item.FechaHoraDesc = item.FechaAlerta.ToString("yyyy-MM-dd HH:mm");
                item.HoraDesc = item.FechaAlerta.ToString("HH:mm");
            }

            return alertas;
        }

        /// <summary>
        /// Genera Archivo excel  de HOP y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="checkCompensar"></param>
        /// <returns></returns>
        public string GenerarFileExcelReporteHOPOsinergmin(DateTime fechaInicial, DateTime fechaFinal, bool checkCompensar)
        {
            //Data
            List<EveHoraoperacionDTO> data = this.ListarHorasOperacionByCriteria(fechaInicial, fechaFinal, ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico).ToList();
            data = this.CompletarListaHoraOperacionTermo(data);
            data = data.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();

            foreach (var reg in data)
            {
                reg.HophoriniDesc = reg.Hophorini != null ? reg.Hophorini.Value.ToString(ConstantesBase.FormatoFechaHora) : string.Empty;
                reg.HophorfinDesc = reg.Hophorfin != null ? reg.Hophorfin.Value.ToString(ConstantesBase.FormatoFechaHora) : string.Empty;
            }

            //Exportación a Excel
            string fileExcel = string.Empty;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet wsHist = xlPackage.Workbook.Worksheets.Add(ConstantesHorasOperacion.NombreHojaHOPOsinergmin);
                wsHist.View.ShowGridLines = false;

                #region Hoja Osinergmin

                int row = 6;
                int column = 2;

                int rowTitulo = 2;
                wsHist.Cells[rowTitulo, column + 2].Value = ConstantesHorasOperacion.TituloHojaHOPOsinergmin;
                wsHist.Cells[rowTitulo, column + 2].Style.Font.SetFromFont(new Font("Calibri", 14));
                wsHist.Cells[rowTitulo, column + 2].Style.Font.Bold = true;

                #region filtros
                int columnIniFiltro = column;
                string fechaIniFiltro = fechaInicial.ToString(ConstantesAppServicio.FormatoFecha);
                string fechaFinFiltro = fechaFinal.ToString(ConstantesAppServicio.FormatoFecha);

                int rowIniFiltro = row;
                wsHist.Cells[row, columnIniFiltro].Value = "Fecha Inicio:";
                wsHist.Cells[row, columnIniFiltro].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, columnIniFiltro + 1].Value = fechaIniFiltro;

                row++;
                wsHist.Cells[row, columnIniFiltro].Value = "Fecha Fin:";
                wsHist.Cells[row, columnIniFiltro].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, columnIniFiltro + 1].Value = fechaFinFiltro;

                using (var range = wsHist.Cells[rowIniFiltro, columnIniFiltro, row, columnIniFiltro])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Font.Bold = true;
                }

                #endregion

                row += 2;
                #region cabecera

                int colIniEmpresa = 3;
                int colIniCentral = colIniEmpresa + 1;
                int colIniModo = colIniCentral + 1;
                int colIniEnParalelo = colIniModo + 1;
                int colIniFueraParalelo = colIniEnParalelo + 1;
                int colIniTipoOp = colIniFueraParalelo + 1;

                int colIniCompArr = colIniTipoOp;
                int colIniCompPar = colIniTipoOp;
                if (checkCompensar)
                {
                    colIniCompArr = colIniTipoOp + 1;
                    colIniCompPar = colIniCompArr + 1;
                }

                int colIniCodEq = colIniCompPar + 1;

                wsHist.Cells[row, colIniEmpresa].Value = "Empresa";
                wsHist.Cells[row, colIniCentral].Value = "Central";
                wsHist.Cells[row, colIniModo].Value = "Grupo";
                wsHist.Cells[row, colIniEnParalelo].Value = "Inicio";
                wsHist.Cells[row, colIniFueraParalelo].Value = "Final";
                wsHist.Cells[row, colIniTipoOp].Value = "Situación";

                if (checkCompensar)
                {
                    wsHist.Cells[row, colIniCompArr].Value = "Comp. Arrq";
                    wsHist.Cells[row, colIniCompPar].Value = "Comp. Pard";
                }

                wsHist.Cells[row, colIniCodEq].Value = "CodEq";

                wsHist.Cells[row, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniModo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniEnParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniFueraParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                if (checkCompensar)
                {
                    wsHist.Cells[row, colIniCompArr].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniCompPar].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                wsHist.Cells[row, colIniCodEq].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                using (var range = wsHist.Cells[row, colIniEmpresa, row, colIniCodEq])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Font.Bold = true;
                }

                #endregion

                #region cuerpo
                int rowIni = row;
                foreach (var reg in data)
                {
                    row++;

                    wsHist.Cells[row, colIniEmpresa].Value = reg.Emprnomb.Trim();
                    wsHist.Cells[row, colIniEmpresa].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row, colIniCentral].Value = reg.Central.Trim();
                    wsHist.Cells[row, colIniCentral].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row, colIniModo].Value = reg.Equiabrev;
                    wsHist.Cells[row, colIniModo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row, colIniEnParalelo].Value = reg.HophoriniDesc;
                    wsHist.Cells[row, colIniEnParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniEnParalelo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    wsHist.Cells[row, colIniFueraParalelo].Value = reg.HophorfinDesc;
                    wsHist.Cells[row, colIniFueraParalelo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colIniFueraParalelo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    wsHist.Cells[row, colIniTipoOp].Value = reg.Subcausadesc;
                    wsHist.Cells[row, colIniTipoOp].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    if (checkCompensar)
                    {
                        wsHist.Cells[row, colIniCompArr].Value = reg.HopcompordarrqDesc;
                        wsHist.Cells[row, colIniCompArr].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniCompArr].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        wsHist.Cells[row, colIniCompPar].Value = reg.HopcompordpardDesc;
                        wsHist.Cells[row, colIniCompPar].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row, colIniCompPar].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    wsHist.Cells[row, colIniCodEq].Value = reg.Equicodi;
                    wsHist.Cells[row, colIniCodEq].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                wsHist.Column(1).Width = 3;
                wsHist.Column(columnIniFiltro).Width = 13;
                wsHist.Column(colIniEmpresa).Width = 35;
                wsHist.Column(colIniCentral).Width = 20;
                wsHist.Column(colIniModo).Width = 30;
                wsHist.Column(colIniEnParalelo).Width = 25;
                wsHist.Column(colIniFueraParalelo).Width = 25;
                wsHist.Column(colIniTipoOp).Width = 30;
                if (checkCompensar)
                {
                    wsHist.Column(colIniCompArr).Width = 12;
                    wsHist.Column(colIniCompPar).Width = 12;
                }
                wsHist.Column(colIniCodEq).Width = 10;

                #endregion

                #region logo

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = wsHist.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 1;
                picture.From.Row = 1;

                #endregion

                #endregion

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        #region Reporte Costos CT

        /// <summary>
        /// GenerarExcelReporteCostosCT
        /// </summary>
        /// <param name="listaModosOperacionCT"></param>
        /// <param name="path"></param>
        /// <param name="fecha"></param>
        /// <param name="fileName"></param>
        public void GenerarExcelReporteCostosCT(List<ReporteExcelCT> listaModosOperacionCT, string path, string fecha, out string fileName)
        {

            listaModosOperacionCT = listaModosOperacionCT.OrderBy(x => x.CIncremental1).ToList();
            fileName = "ReporteCostosCT_" + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaYMD);
            fileName += ".xlsx";
            string file = path + fileName;
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");

                #region Cabecera
                int rowIniHeader = 1;
                int colIniHeader = 2;
                int filTitulo = rowIniHeader;
                int colTitulo = colIniHeader + 1;

                //titulo
                ws.Cells[filTitulo, colTitulo].Value = "REPORTE DE COSTOS CENTRALES TÉRMICAS";
                var font = ws.Cells[filTitulo, colTitulo].Style.Font;
                font.Size = 16;
                font.Bold = true;
                font.Name = "Calibri";

                ws.Cells[rowIniHeader + 1, colIniHeader].Value = "FECHA:";
                ws.Cells[rowIniHeader + 1, colIniHeader + 1].Value = fecha;

                int row = rowIniHeader + 3;
                int col = colIniHeader;
                int filaIniCab = row;
                int coluIniCab = col;

                int colEmpresa = coluIniCab;
                int colCentral = colEmpresa + 1;
                int colModo = colCentral + 1;
                int colCV = colModo + 1;
                int colTramo1 = colCV + 1;
                int colCI1 = colTramo1 + 1;
                int colTramo2 = colCI1 + 1;
                int colCI2 = colTramo2 + 1;

                ws.Cells[filaIniCab, colEmpresa].Value = "EMPRESA";
                ws.Cells[filaIniCab, colCentral].Value = "CENTRAL";
                ws.Cells[filaIniCab, colModo].Value = "Modo de Operación";
                ws.Cells[filaIniCab, colCV].Value = "CV (S/. / MWh)";
                ws.Cells[filaIniCab, colTramo1].Value = "Tramo 1";
                ws.Cells[filaIniCab, colCI1].Value = "CI 1 (S/. / MWh)";
                ws.Cells[filaIniCab, colTramo2].Value = "Tramo 2";
                ws.Cells[filaIniCab, colCI2].Value = "CI 2 (S/. / MWh)";

                ExcelRange rg = ws.Cells[filaIniCab, colEmpresa, filaIniCab, colCI2];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                #endregion

                int filaIniData = filaIniCab + 1;
                int coluIniData = coluIniCab;
                foreach (var item in listaModosOperacionCT)
                {
                    var color = item.FlagActivo ? "#FFFFFF" : "#C6F5FB";

                    ws.Cells[filaIniData, colEmpresa].Value = item.Emprnomb;
                    ws.Cells[filaIniData, colCentral].Value = item.Central;
                    ws.Cells[filaIniData, colModo].Value = item.Gruponomb;
                    ws.Cells[filaIniData, colCV].Value = item.CVariable1;
                    ws.Cells[filaIniData, colCV].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[filaIniData, colTramo1].Value = item.Tramo1;
                    ws.Cells[filaIniData, colTramo1].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[filaIniData, colCI1].Value = item.CIncremental1;
                    ws.Cells[filaIniData, colCI1].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[filaIniData, colTramo2].Value = item.Tramo2;
                    ws.Cells[filaIniData, colTramo2].Style.Numberformat.Format = "#,##0.00";
                    ws.Cells[filaIniData, colCI2].Value = item.CIncremental2;
                    ws.Cells[filaIniData, colCI2].Style.Numberformat.Format = "#,##0.00";

                    //UtilExcel.CeldasExcelWrapText(ws, filaIniData, colEmpresa, filaIniData, colCI2);
                    UtilExcel.CeldasExcelColorFondo(ws, filaIniData, colEmpresa, filaIniData, colCI2, color);

                    rg = ws.Cells[filaIniData, 2, filaIniData, 12];
                    rg.Style.Font.Size = 10;
                    rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));

                    if (item.FlagActivo)
                        rg.Style.Font.Bold = true;

                    filaIniData++;
                }

                rg = ws.Cells[filaIniCab + 1, colEmpresa, filaIniData - 1, colCI2];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                ws.Cells[filaIniCab, colEmpresa, filaIniData - 1, colCI2].AutoFilter = true;

                rg = ws.Cells[filaIniCab + 1, colEmpresa, filaIniData - 1, colModo];
                rg.AutoFitColumns();
                ws.Column(colCV).Width = 20; //CVariable1
                ws.Column(colTramo1).Width = 20; //Tramo1
                ws.Column(colCI1).Width = 20; //CIncremental1
                ws.Column(colTramo2).Width = 20; //Tramo2
                ws.Column(colCI2).Width = 20; //CIncremental2

                //rg = ws.Cells[filaIniCab + 1, colEmpresa + 1, filaIniData, colCI2];
                //rg.AutoFitColumns();
                ws.View.FreezePanes(filaIniCab + 1, colEmpresa + 1);

                xlPackage.Save();
            }
        }

        /// <summary>
        /// ListarReporteHopCT
        /// </summary>
        /// <param name="listaReporteCostosIncr"></param>
        /// <param name="listaDataHopModo"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="listaModoProgramada"></param>
        /// <param name="listaModosOperacion"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarReporteHopCT(List<ReporteCostoIncrementalDTO> listaReporteCostosIncr,
                                            List<EveHoraoperacionDTO> listaDataHopModo, DateTime fechaConsulta
                                        , List<PrGrupoDTO> listaModoProgramada, List<PrGrupoDTO> listaModosOperacion)
        {
            PrGrupoDTO grupo;
            NumberFormatInfo nfi2 = UtilAnexoAPR5.GenerarNumberFormatInfo2();

            PrGrupoDTO lmp;

            foreach (ReporteCostoIncrementalDTO item in listaReporteCostosIncr)
            {
                    //al modo programado asignarle CV y CI
                    lmp = listaModoProgramada.FirstOrDefault(p => p.Grupocodi == item.Grupocodi);
                    if (lmp != null)
                    {
                        lmp.NumTramo = 1;
                        lmp.CVariableFormateado = MathHelper.TruncateDouble(Convert.ToDouble(item.CV) * 1000, 2).ToString("N", nfi2);
                        lmp.CIncrementalFormateado = MathHelper.TruncateDouble(item.Cincrem1, 2).ToString("N", nfi2);
                        lmp.CVariable = Convert.ToDouble(item.CV) * 1000;
                        lmp.CIncremental = item.Cincrem1;
                    }

                    grupo = listaModosOperacion.SingleOrDefault(p => p.Grupocodi == item.Grupocodi);

                    if (grupo == null)
                    {
                        listaModosOperacion.Add(new PrGrupoDTO()
                        {
                            Grupocodi = item.Grupocodi,
                            Emprnomb = item.Empresa ?? "",
                            Gruponomb = item.GrupoModoOperacion ?? "",
                            Central = "",
                            CVariable1Formateado = MathHelper.TruncateDouble(Convert.ToDouble(item.CV) * 1000, 2).ToString("N", nfi2),
                            CVariable1 = Convert.ToDouble(item.CV) * 1000,

                            Tramo1 = item.Tramo1 ?? "",
                            CIncremental1Formateado = MathHelper.TruncateDouble(item.Cincrem1, 2).ToString("N", nfi2),

                            Tramo2 = item.Tramo2 ?? "",
                            CIncremental2Formateado = MathHelper.TruncateDouble(item.Cincrem2, 2).ToString("N", nfi2),

                            FlagEncendido = 1,
                            ColorTermica = ConstantesIEOD.PropiedadColorDefault
                        });
                    }
                    else
                    {
                        grupo.FlagActivo = listaDataHopModo.Any(p => p.Grupocodi == item.Grupocodi && p.Subcausacodi > -1 && p.Hophorini <= fechaConsulta && p.Hophorfin >= fechaConsulta);

                        if (string.IsNullOrEmpty(grupo.Emprnomb)) grupo.Emprnomb = item.Empresa ?? "";

                        grupo.CVariable1 = Convert.ToDouble(item.CV) * 1000;
                        grupo.CVariable1Formateado = MathHelper.TruncateDouble(Convert.ToDouble(item.CV) * 1000, 2).ToString("N", nfi2);

                        grupo.Tramo1 = item.Tramo1 ?? "";
                        grupo.CIncremental1 = item.Cincrem1;
                        grupo.CIncremental1Formateado = MathHelper.TruncateDouble(item.Cincrem1, 2).ToString("N", nfi2);

                        grupo.Tramo2 = item.Tramo2 ?? "";
                        grupo.CIncremental2 = item.Cincrem2;
                        grupo.CIncremental2Formateado = MathHelper.TruncateDouble(item.Cincrem2, 2).ToString("N", nfi2);

                    }
            }

            var grp = listaReporteCostosIncr.Select(p => p.Grupocodi).ToList();

            List<PrGrupoDTO> listaGrupoCT = listaModosOperacion.Where(p => grp.Contains(p.Grupocodi)).ToList();

            return listaGrupoCT;
        }

        #endregion

        #endregion

        #region Continuar Día (Finalizar día actual e iniciar día siguiente)

        /// <summary>
        /// Registrar continuacion de horas de operacion, desde el día anterior cuyas Horas de Operación que terminan a las 24:00 
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        /// <param name="listaHopcodiPermitido"></param>
        /// <param name="listCodHop"></param>
        /// <returns></returns>
        public int RegistrarIniciarDia(DateTime fechaIni, DateTime fechaFin, string usuario, List<int> listaHopcodiPermitido, ref List<int> listCodHop)
        {
            //
            List<EveHoraoperacionDTO> listaValida = new List<EveHoraoperacionDTO>();

            //
            DateTime fechaRegistro = DateTime.Now;

            var listaHorasOperacion = this.ListarHorasOperacionByCriteria(fechaIni, fechaFin, ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
            var listaHorasOperacionAnt = this.ListarHorasOperacionByCriteria(fechaIni.AddDays(-1), fechaFin.AddDays(-1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
            listaHorasOperacionAnt = listaHorasOperacionAnt.Where(x => x.Subcausacodi >= 0).ToList(); //solo iniciar día para las horas de operación que tienen calificación
            //Lista Desglose
            List<EveHoEquiporelDTO> listaDesgloseRango = this.GetByCriteriaEveHoEquiporelGroupByHoPadre(fechaIni.AddDays(-1), fechaFin.AddDays(-1));
            var listaHorasOperacionAntCond = listaHorasOperacionAnt.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo && x.Hophorfin.Value.Date == fechaIni.Date && (x.Hophorparada == null || listaHopcodiPermitido.Contains(x.Hopcodi))).ToList();

            //Verificacion de las Horas de operacion que cumplen las condiciones y no tengan cruces
            List<EveHoraoperacionDTO> lista = this.ListarHoraOperacionCandidatoNuevoDia(fechaIni, usuario, fechaRegistro, listaHorasOperacion, listaHorasOperacionAnt, listaHorasOperacionAntCond, listaDesgloseRango);

            //Registrar las Horas de Operacion
            List<CodigosPadresHO> ListaCodPadre = new List<CodigosPadresHO>();
            int codiHoraOPNew = 0;
            List<int> listaHopcodipadreFicticio = lista.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).Select(x => x.Hopcodi).Distinct().ToList();

            foreach (var hcodip in listaHopcodipadreFicticio)
            {
                var regHop = lista.Find(x => x.Hopcodi == hcodip);

                //Verificar cruces de las nuevas horas de operación 
                var val = this.VerificarCruceHoRegistroNuevaDiaEms(regHop, ConstantesHorasOperacion.IdTipoTermica, regHop.Emprcodi, regHop.Equipadre);

                if (val.TipoCruce == ConstantesHorasOperacion.CruceHoNoExiste)
                {
                    var listaTmp = lista.Where(x => x.Hopcodi == hcodip || x.Hopcodipadre == hcodip).ToList();

                    foreach (var entity in listaTmp)
                    {
                        if (entity.Grupocodi != null)
                        {
                            codiHoraOPNew = this.GuardarHoraOP(entity);
                            listaValida.Add(entity);

                            CodigosPadresHO obj = new CodigosPadresHO
                            {
                                Hopcodipadre = codiHoraOPNew,
                                Hopcodi = entity.Hopcodi
                            };
                            ListaCodPadre.Add(obj);

                            listCodHop.Add(codiHoraOPNew);
                        }
                        else // si es equipo o unidad
                        {
                            if (entity.Hopcodipadre != 0) // si se trata de una unidad
                            {
                                var find = ListaCodPadre.Find(x => x.Hopcodi == entity.Hopcodipadre);
                                if (find != null)
                                {
                                    entity.Hopcodipadre = find.Hopcodipadre;
                                    codiHoraOPNew = this.GuardarHoraOP(entity);
                                    listaValida.Add(entity);

                                    listCodHop.Add(codiHoraOPNew);

                                    this.CreateOrUpdateRestriccionOperativa(listaTmp.Find(x => x.Hopcodi == find.Hopcodi), entity.Hopcodipadre ?? -1, entity.Equicodi, usuario, fechaRegistro, false);
                                }
                            }
                        }
                    }
                }
            }

            //Generar la lista de Horas de operacion para el envio
            listCodHop.AddRange(listaHorasOperacion.Select(x => x.Hopcodi).Distinct().ToList());

            //Mostrar el tipo de resultado obtenido
            if (listaHorasOperacionAntCond.Count > 0)
            {
                return listaValida.Count > 0 ? ConstantesHorasOperacion.TipoNuevoDiaExitoso : ConstantesHorasOperacion.TipoNuevoDiaRegistroPrevioExistente;
            }
            else
            {
                return ConstantesHorasOperacion.TipoNuevoDiaNoHayDataAnterior;
            }
        }

        /// <summary>
        /// Guardar las Horas de Operación
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="usuario"></param>
        /// <param name="listCodHop"></param>
        /// <returns></returns>
        public int GuardarFinalizarDia(DateTime fechaIni, DateTime fechaFin, string usuario, ref List<int> listCodHop)
        {
            int numHorFin = Convert.ToInt32(ConstantesHorasOperacion.HoraFinDefecto.Substring(0, 2));
            int numMinFin = Convert.ToInt32(ConstantesHorasOperacion.HoraFinDefecto.Substring(3, 2));
            DateTime horaFinNuevoDia = fechaIni.Date.AddHours(numHorFin).AddMinutes(numMinFin);
            DateTime horaFinFinalizarDia = fechaFin.Date;
            DateTime fechaRegistro = DateTime.Now;

            //Lista Ho BD
            List<EveHoraoperacionDTO> listaHorasOperacionData = this.ListarHorasOperacionByCriteria(fechaIni, fechaFin, ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
            listaHorasOperacionData = listaHorasOperacionData.Where(x => x.Subcausacodi >= 0).ToList(); //solo finalizar día para las horas de operación que tienen calificación
            //Lista Desglose
            List<EveHoEquiporelDTO> listaDesgloseRango = this.GetByCriteriaEveHoEquiporelGroupByHoPadre(fechaIni, fechaIni);

            var listaModoOp = listaHorasOperacionData.Select(x => x.Grupocodi).Distinct().ToList();
            List<EveHoraoperacionDTO> listaHorasOperacion = new List<EveHoraoperacionDTO>();
            foreach (var modoOp in listaModoOp)
            {
                var hopXModo = listaHorasOperacionData.Where(x => x.Grupocodi == modoOp).OrderByDescending(x => x.Hophorfin).First();
                listaHorasOperacion.Add(hopXModo);
            }
            var listaHorasOperacionCond = listaHorasOperacion.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo && x.Hophorparada == null && x.Hophorfin.Value == horaFinNuevoDia).ToList();

            //Horas de operacion que son del nuevo día
            List<EveHoraoperacionDTO> lista = new List<EveHoraoperacionDTO>();
            foreach (var hop in listaHorasOperacionCond)
            {
                var listaHopUnid = listaHorasOperacionData.Where(x => x.Hopcodipadre == hop.Hopcodi && x.Hophorfin.Value == horaFinNuevoDia).ToList();

                if (listaHopUnid.Count > 0) //hay hop del modo y sus hop unidades
                {
                    hop.Hophorfin = horaFinFinalizarDia;
                    hop.Hophorparada = null;
                    hop.Lastuser = usuario;
                    hop.Lastdate = fechaRegistro;

                    lista.Add(hop);
                    foreach (var hopuni in listaHopUnid)
                    {
                        hopuni.Hophorfin = horaFinFinalizarDia;
                        hopuni.Hophorparada = null;
                        hopuni.Lastuser = usuario;
                        hopuni.Lastdate = fechaRegistro;

                        lista.Add(hopuni);
                    }
                }
            }

            //Actualizar las Horas de Operacion y el desplose
            List<EveHoraoperacionDTO> listaValida = new List<EveHoraoperacionDTO>();

            List<int> listaHopcodipadre = lista.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).Select(x => x.Hopcodi).Distinct().ToList();

            foreach (var hcodip in listaHopcodipadre)
            {
                var regHop = lista.Find(x => x.Hopcodi == hcodip);
                regHop.ListaDesglose = listaDesgloseRango.Where(x => x.Hopcodi == hcodip).ToList();
                foreach (var reg in regHop.ListaDesglose)
                {
                    if (reg.Ichorfin == horaFinNuevoDia)
                    {
                        reg.Ichorfin = horaFinFinalizarDia;
                    }
                }

                //Verificar cruces de las nuevas horas de operación 
                var val = this.VerificarCruceHoFinalizarDiaEms(regHop, ConstantesHorasOperacion.IdTipoTermica, regHop.Emprcodi, regHop.Equipadre);

                if (val.TipoCruce == ConstantesHorasOperacion.CruceHoNoExiste)
                {
                    var listaTmp = lista.Where(x => x.Hopcodi == hcodip || x.Hopcodipadre == hcodip).ToList();

                    foreach (var entity in listaTmp)
                    {
                        //Actualización del desglose
                        this.CreateOrUpdateRestriccionOperativa(listaTmp.Find(x => x.Hopcodi == entity.Hopcodipadre), entity.Hopcodipadre ?? -1, entity.Equicodi, usuario, fechaRegistro, false);

                        //Actualización de la HO
                        this.UpdateHorasOperacion(entity);
                        listaValida.Add(entity);
                    }
                }
            }

            //Generar la lista de Horas de operacion para el envio
            listCodHop = listaHorasOperacionData.Select(x => x.Hopcodi).Distinct().ToList();

            //Mostrar el tipo de resultado obtenido
            if (listaHorasOperacionCond.Count > 0)
            {
                return listaValida.Count > 0 ? ConstantesHorasOperacion.TipoFinalizarDiaExitoso : ConstantesHorasOperacion.TipoFinalizarDiaNoExisteRegCierre;
            }
            else
            {
                return ConstantesHorasOperacion.TipoFinalizarDiaNoHayData;
            }
        }

        /// <summary>
        /// Lista de Hora de operación candidatas a continuar el nuevo día
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaRegistro"></param>
        /// <param name="listaHorasOperacion"></param>
        /// <param name="listaHorasOperacionAnt"></param>
        /// <param name="listaHorasOperacionAntCond"></param>
        /// <param name="listaDesgloseRango"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHoraOperacionCandidatoNuevoDia(DateTime fechaIni, string usuario, DateTime fechaRegistro
                        , List<EveHoraoperacionDTO> listaHorasOperacion, List<EveHoraoperacionDTO> listaHorasOperacionAnt, List<EveHoraoperacionDTO> listaHorasOperacionAntCond
                        , List<EveHoEquiporelDTO> listaDesgloseRango)
        {
            int numHorFin = Convert.ToInt32(ConstantesHorasOperacion.HoraFinDefecto.Substring(0, 2));
            int numMinFin = Convert.ToInt32(ConstantesHorasOperacion.HoraFinDefecto.Substring(3, 2));
            DateTime horaIni = fechaIni.Date;
            DateTime horaFinNuevoDia = horaIni.AddHours(numHorFin).AddMinutes(numMinFin);
            DateTime? horparada = null; //horaIni.AddHours(23);

            int contador = 1, hopcodipadreFicticio = 1;
            List<EveHoraoperacionDTO> lista = new List<EveHoraoperacionDTO>();

            foreach (var hopant in listaHorasOperacionAntCond)
            {
                var listaHopactual = listaHorasOperacion.Where(x => x.Grupocodi == hopant.Grupocodi).ToList();

                if (listaHopactual.Count == 0)  //el día actual no tiene la hora operacion del día anterior
                {
                    var listaHopAntUnid = listaHorasOperacionAnt.Where(x => x.Hopcodipadre == hopant.Hopcodi && x.Hophorfin.Value.Date == horaIni).ToList();
                    var regDesgByHopadre = listaDesgloseRango.Find(x => x.Hopcodi == hopant.Hopcodi && x.Ichorfin == horaIni);

                    if (listaHopAntUnid.Count > 0) //hay hop del modo y sus hop unidades
                    {
                        hopcodipadreFicticio = contador * -100;
                        contador++;

                        EveHoraoperacionDTO nhoppadre = new EveHoraoperacionDTO
                        {
                            Hopcodi = hopcodipadreFicticio,
                            Hophorini = horaIni,
                            Subcausacodi = hopant.Subcausacodi, //consultar
                            Hophorfin = horaFinNuevoDia,
                            Equicodi = hopant.Equicodi,
                            Hopdesc = string.Empty,
                            Hophorordarranq = null,
                            Hophorparada = horparada,
                            Lastuser = usuario,
                            Lastdate = fechaRegistro,
                            Grupocodi = hopant.Grupocodi,
                            Hopsaislado = hopant.Hopsaislado, //consultar
                            Hoplimtrans = hopant.Hoplimtrans, //consultar
                            Hopfalla = hopant.Hopfalla, //consultar
                            Hopcompordarrq = null, //consultar
                            Hopcompordpard = null, //consultar
                            Hopcausacodi = hopant.Hopcausacodi, //consultar
                            Hoparrqblackstart = hopant.Hoparrqblackstart,
                            Hopensayope = hopant.Hopensayope,
                            Hopensayopmin = hopant.Hopensayopmin,
                            Hopcodipadre = 0,
                            FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo,
                            Hopestado = ConstantesHorasOperacion.EstadoActivo,

                            Emprcodi = hopant.Emprcodi,
                            Equipadre = hopant.Equipadre,

                            //agregar desglose a la Hora de Operación
                            ListaDesglose = new List<EveHoEquiporelDTO>()
                        };
                        if (regDesgByHopadre != null)
                        {
                            EveHoEquiporelDTO regDesg = new EveHoEquiporelDTO
                            {
                                Ichorini = nhoppadre.Hophorini.Value,
                                Ichorfin = nhoppadre.Hophorfin.Value,
                                Icvalor1 = regDesgByHopadre.Icvalor1,
                                Subcausacodi = regDesgByHopadre.Subcausacodi,
                                Hoequitipo = regDesgByHopadre.Hoequitipo,
                                TipoDesglose = regDesgByHopadre.TipoDesglose
                            };

                            nhoppadre.ListaDesglose.Add(regDesg);
                        }

                        lista.Add(nhoppadre);
                        foreach (var hopantuni in listaHopAntUnid)
                        {
                            EveHoraoperacionDTO nhopunid = new EveHoraoperacionDTO
                            {
                                Hopcodi = -2 + hopcodipadreFicticio,
                                Hophorini = horaIni,
                                Subcausacodi = hopantuni.Subcausacodi, //consultar
                                Hophorfin = horaFinNuevoDia,
                                Equicodi = hopantuni.Equicodi,
                                Hopdesc = string.Empty,
                                Hophorordarranq = null,
                                Hophorparada = horparada,
                                Lastuser = usuario,
                                Lastdate = fechaRegistro,
                                Grupocodi = null,
                                Hopsaislado = null, //consultar
                                Hoplimtrans = null, //consultar
                                Hopfalla = null, //consultar
                                Hopcompordarrq = null, //consultar
                                Hopcompordpard = null, //consultar
                                Hopcausacodi = null, //consultar
                                Hopcodipadre = hopcodipadreFicticio,
                                Hoparrqblackstart = hopantuni.Hoparrqblackstart,
                                Hopensayope = hopantuni.Hopensayope,
                                Hopensayopmin = hopantuni.Hopensayopmin,
                                Emprcodi = nhoppadre.Emprcodi,
                                Hopestado = ConstantesHorasOperacion.EstadoActivo
                            };

                            lista.Add(nhopunid);
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Listar las horas de operación del día anterior que tienen valor de orden de parada
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EveHoraoperacionDTO> ListarHoOrdParCondContinuarNuevoDia(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> lista = new List<EveHoraoperacionDTO>();

            //
            var listaHorasOperacion = this.ListarHorasOperacionByCriteria(fechaIni, fechaFin, ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
            var listaHorasOperacionAnt = this.ListarHorasOperacionByCriteria(fechaIni.AddDays(-1), fechaFin.AddDays(-1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoSoloTermico);
            var listaHorasOperacionAntCond = listaHorasOperacionAnt.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo && x.Hophorfin.Value.Date == fechaIni.Date && x.Hophorparada != null).ToList();

            foreach (var hopant in listaHorasOperacionAntCond)
            {
                var listaHopactual = listaHorasOperacion.Where(x => x.Grupocodi == hopant.Grupocodi).ToList();
                if (listaHopactual.Count == 0)  //el día actual no tiene la hora operacion del día anterior
                {
                    lista.Add(hopant);
                }
            }

            return lista.OrderBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Grupoabrev).ToList();
        }

        /// <summary>
        /// Verificación si existe cruce de Horas Operacion en Registro de Nuevo Día Ems
        /// </summary>
        /// <param name="iho"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public ValidacionHoraOperacion VerificarCruceHoRegistroNuevaDiaEms(EveHoraoperacionDTO iho, int idTipoCentral, int idEmpresa, int idCentral)
        {
            List<EveHoraoperacionDTO> listaHorasOperacion = this.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, iho.Hophorini.Value.Date, iho.Hophorini.Value.Date.AddDays(1), idCentral);

            int tipoHO = ConstantesHorasOperacion.TipoHONormal;
            List<PrGrupoDTO> listaModosOperacion = this.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa);
            List<EqEquipoDTO> codUnidadesExtras = new List<EqEquipoDTO>();

            var regModo = listaModosOperacion.Find(x => x.Grupocodi == iho.Grupocodi);
            if (regModo != null)
            {
                tipoHO = ConstantesHorasOperacion.FlagModoEspecial != regModo.FlagModoEspecial ? ConstantesHorasOperacion.TipoHONormal : ConstantesHorasOperacion.TipoHOUnidadEspecial;
                var listaUnidadesEsp = this.ListarGruposxCentralGENEspecial(regModo.Grupocodi, 3);
                if (listaUnidadesEsp.Count > 1)
                {
                    var listaHoUnidEsp = listaHorasOperacion.Where(x => x.Grupocodi == iho.Grupocodi && x.Hopcodipadre == iho.Hopcodi);
                    foreach (var reg in listaHoUnidEsp)
                    {
                        codUnidadesExtras.Add(new EqEquipoDTO() { Equicodi = reg.Equicodi.Value });
                    }
                }
            }

            //
            var val = this.VerificarCrucesHorasOperacionGlobal(listaHorasOperacion, idTipoCentral, iho.Equicodi, iho.Grupocodi, tipoHO
                , listaModosOperacion, codUnidadesExtras, iho.Hophorini.Value, iho.Hophorfin.Value, -1);

            return val;
        }

        /// <summary>
        /// Verificación si existe cruce de Horas Operacion en Finalizar Día Ems
        /// </summary>
        /// <param name="iho"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public ValidacionHoraOperacion VerificarCruceHoFinalizarDiaEms(EveHoraoperacionDTO iho, int idTipoCentral, int idEmpresa, int idCentral)
        {
            List<EveHoraoperacionDTO> listaHorasOperacion = this.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, iho.Hophorini.Value.Date, iho.Hophorini.Value.Date.AddDays(1), idCentral);
            var hopSelected = listaHorasOperacion.Find(x => x.Hopcodi == iho.Hopcodi);
            int pos = listaHorasOperacion.FindIndex(x => x.Hopcodi == iho.Hopcodi);
            if (pos == -1)
            {
                throw new Exception("No existe la Hora de Operación con código " + iho.Hopcodi);
            }

            //
            int tipoHO = ConstantesHorasOperacion.TipoHONormal;
            List<PrGrupoDTO> listaModosOperacion = this.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa);
            List<EqEquipoDTO> codUnidadesExtras = new List<EqEquipoDTO>();

            var regModo = listaModosOperacion.Find(x => x.Grupocodi == iho.Grupocodi);
            if (regModo != null)
            {
                tipoHO = ConstantesHorasOperacion.FlagModoEspecial != regModo.FlagModoEspecial ? ConstantesHorasOperacion.TipoHONormal : ConstantesHorasOperacion.TipoHOUnidadEspecial;
                var listaUnidadesEsp = this.ListarGruposxCentralGENEspecial(regModo.Grupocodi, 3);
                if (listaUnidadesEsp.Count > 1)
                {
                    var listaHoUnidEsp = listaHorasOperacion.Where(x => x.Hopcodipadre == iho.Hopcodi);
                    foreach (var reg in listaHoUnidEsp)
                    {
                        codUnidadesExtras.Add(new EqEquipoDTO() { Equicodi = reg.Equicodi.Value });
                    }
                }
            }

            //
            var val = this.VerificarCrucesHorasOperacionGlobal(listaHorasOperacion, idTipoCentral, iho.Equicodi, iho.Grupocodi, tipoHO
                , listaModosOperacion, codUnidadesExtras, iho.Hophorini.Value, iho.Hophorfin.Value, pos);

            return val;
        }

        #endregion

        #region Listado de Correos

        /// <summary>
        /// Permite listar los logs de correos enviados por el aplicativo
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<SiCorreoDTO> ListarLogCorreo(DateTime fechaIni, DateTime fechaFin)
        {
            List<int> ids = new List<int>
            {
                ConstantesHorasOperacion.PlantcodiNotifHoModif,
                ConstantesIntervencionesAppServicio.PlantcodiAlertaHoraOperacion
            };

            return FactorySic.GetSiCorreoRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto, fechaIni, fechaFin, string.Join<int>(
                ConstantesAppServicio.CaracterComa.ToString(), ids));
        }

        /// <summary>
        /// Reporte html de los correos enviados por el aplicativo
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ListarReporteLogCorreoHtml(DateTime fechaIni, DateTime fechaFin, string url)
        {
            List<SiCorreoDTO> listaData = this.ListarLogCorreo(fechaIni, fechaFin).OrderByDescending(x => x.Corrfechaenvio).ToList();

            if (!listaData.Any()) return string.Empty;

            StringBuilder str = new StringBuilder();

            //
            str.Append("<table class='pretty tabla-icono tabla-ems' style='width: 95%'>");

            str.Append("<thead>");
            #region cabecera
            str.Append("<tr>");
            str.Append("<th style=''>Detalle</th>");
            str.Append("<th style=''>Asunto</th>");
            str.Append("<th style=''>Destinatarios</th>");
            str.Append("<th style=''>Remitente</th>");
            str.Append("<th style=''>Empresa</th>");
            str.Append("<th style=''>Fecha Envío</th>");
            str.Append("</tr>");
            #endregion
            str.Append("</thead>");

            str.Append("<tbody>");
            #region cuerpo
            if (listaData.Count > 0)
            {
                foreach (var reg in listaData)
                {
                    str.Append("<tr>");
                    str.AppendFormat("<td>");
                    str.AppendFormat("<a href='JavaScript: verContenido({0}); '><img src='" + url + "Content/Images/btn-open.png' alt='Ver registro' title='Ver contenido' /></a>", reg.Corrcodi);
                    str.AppendFormat("</td>");
                    str.AppendFormat("<td>{0}</td>", reg.Corrasunto);
                    str.AppendFormat("<td>{0}</td>", reg.Corrto);
                    str.AppendFormat("<td>{0}</td>", reg.Corrfrom);
                    str.AppendFormat("<td>{0}</td>", reg.Emprnomb);
                    str.AppendFormat("<td>{0}</td>", reg.Corrfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));
                    str.Append("</tr>");
                }
            }
            else
            {
                str.Append("<tr>");
                str.Append("<td colspan='6'>&nbsp;</td>");
                str.Append("</tr>");
            }
            #endregion
            str.Append("</tbody>");

            str.Append("</table>");

            return str.ToString();
        }

        #endregion

        #region Utiles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private bool EsNumero(string valor)
        {
            return decimal.TryParse(valor, out _);
        }
        #endregion
    }

    #region Clases Horas de Operación

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class HOPAlerta
    {
        public string CodAlerta { get; set; }
        public string MsjAlerta { get; set; }
        public DateTime FechaAlerta { get; set; }
        public Int16 Prioridad { get; set; }
        public bool Visible { get; set; }

        public string FechaHoraDesc { get; set; }
        public string HoraDesc { get; set; }
    }

    public class CodigosPadresHO
    {
        public int Hopcodipadre { get; set; }
        public int Hopcodi { get; set; }
    }

    public class ValidacionHoraOperacion
    {
        public EveHoraoperacionDTO HoraOperacion { get; set; }
        public EveHoraoperacionDTO HoraOperacionUnidadEspecial { get; set; }
        public int TipoCruce { get; set; }
        public string Mensaje { get; set; }
    }

    public class ReporteHoraoperacionDTO
    {
        public string Unidad { get; set; }
        public int IdUnidad { get; set; }
        public string ModoOpGrupo { get; set; }
        public int IdModoOpGrupo { get; set; }
        public string Central { get; set; }
        public int IdCentral { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public DateTime Hophorini { get; set; }
        public DateTime Hophorfin { get; set; }
    }

    /// <summary>
    /// Clase para almacenar los valores de validación de Horas de operación con otros aplicativos (EMS, Scada, Intervenciones)
    /// </summary>
    public class ResultadoValidacionAplicativo
    {
        public int TipoResultado { get; set; }
        public int TipoValidacion { get; set; }

        public int TipoFuenteDato { get; set; }
        public string TipoFuenteDatoDesc { get; set; }

        public int Hopcodi { get; set; }
        public int? Hopcodipadre { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }

        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        public int Ptomedicodi { get; set; }
        public string Ptomedielenomb { get; set; }

        public int Fenergcodi { get; set; }

        public int Equipadre { get; set; }
        public int Famcodipadre { get; set; }
        public string Central { get; set; }
        public string Hopdesc { get; set; }
        public string ModoOp { get; set; }
        public string Subcausadesc { get; set; }
        public string Descripcion { get; set; }
        public string Justificacion { get; set; }
        public string Accion { get; set; }
        public List<string> ListaMensaje { get; set; }

        public string FechaIniDesc { get; set; }
        public string FechaFinDesc { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }

        public int Canalcodi { get; set; }
        public DateTime Medifecha { get; set; }
        public int Tipoinfocodi { get; set; }
        public string Tipoinfoabrev { get; set; }

        public int Intercodi { get; set; }
        public string Tipoevenabrev { get; set; }
        public string Interdescrip { get; set; }
        public string Interindispo { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionUsuarioCorreo { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public EveHoraoperacionDTO HoraOperacion { get; set; }
        public InIntervencionDTO Intervencion { get; set; }

        #region H

        public decimal? H1 { get; set; }
        public decimal? H2 { get; set; }
        public decimal? H3 { get; set; }
        public decimal? H4 { get; set; }
        public decimal? H5 { get; set; }
        public decimal? H6 { get; set; }
        public decimal? H7 { get; set; }
        public decimal? H8 { get; set; }
        public decimal? H9 { get; set; }
        public decimal? H10 { get; set; }
        public decimal? H11 { get; set; }
        public decimal? H12 { get; set; }
        public decimal? H13 { get; set; }
        public decimal? H14 { get; set; }
        public decimal? H15 { get; set; }
        public decimal? H16 { get; set; }
        public decimal? H17 { get; set; }
        public decimal? H18 { get; set; }
        public decimal? H19 { get; set; }
        public decimal? H20 { get; set; }
        public decimal? H21 { get; set; }
        public decimal? H22 { get; set; }
        public decimal? H23 { get; set; }
        public decimal? H24 { get; set; }
        public decimal? H25 { get; set; }
        public decimal? H26 { get; set; }
        public decimal? H27 { get; set; }
        public decimal? H28 { get; set; }
        public decimal? H29 { get; set; }
        public decimal? H30 { get; set; }
        public decimal? H31 { get; set; }
        public decimal? H32 { get; set; }
        public decimal? H33 { get; set; }
        public decimal? H34 { get; set; }
        public decimal? H35 { get; set; }
        public decimal? H36 { get; set; }
        public decimal? H37 { get; set; }
        public decimal? H38 { get; set; }
        public decimal? H39 { get; set; }
        public decimal? H40 { get; set; }
        public decimal? H41 { get; set; }
        public decimal? H42 { get; set; }
        public decimal? H43 { get; set; }
        public decimal? H44 { get; set; }
        public decimal? H45 { get; set; }
        public decimal? H46 { get; set; }
        public decimal? H47 { get; set; }
        public decimal? H48 { get; set; }
        public decimal? H49 { get; set; }
        public decimal? H50 { get; set; }
        public decimal? H51 { get; set; }
        public decimal? H52 { get; set; }
        public decimal? H53 { get; set; }
        public decimal? H54 { get; set; }
        public decimal? H55 { get; set; }
        public decimal? H56 { get; set; }
        public decimal? H57 { get; set; }
        public decimal? H58 { get; set; }
        public decimal? H59 { get; set; }
        public decimal? H60 { get; set; }
        public decimal? H61 { get; set; }
        public decimal? H62 { get; set; }
        public decimal? H63 { get; set; }
        public decimal? H64 { get; set; }
        public decimal? H65 { get; set; }
        public decimal? H66 { get; set; }
        public decimal? H67 { get; set; }
        public decimal? H68 { get; set; }
        public decimal? H69 { get; set; }
        public decimal? H70 { get; set; }
        public decimal? H71 { get; set; }
        public decimal? H72 { get; set; }
        public decimal? H73 { get; set; }
        public decimal? H74 { get; set; }
        public decimal? H75 { get; set; }
        public decimal? H76 { get; set; }
        public decimal? H77 { get; set; }
        public decimal? H78 { get; set; }
        public decimal? H79 { get; set; }
        public decimal? H80 { get; set; }
        public decimal? H81 { get; set; }
        public decimal? H82 { get; set; }
        public decimal? H83 { get; set; }
        public decimal? H84 { get; set; }
        public decimal? H85 { get; set; }
        public decimal? H86 { get; set; }
        public decimal? H87 { get; set; }
        public decimal? H88 { get; set; }
        public decimal? H89 { get; set; }
        public decimal? H90 { get; set; }
        public decimal? H91 { get; set; }
        public decimal? H92 { get; set; }
        public decimal? H93 { get; set; }
        public decimal? H94 { get; set; }
        public decimal? H95 { get; set; }
        public decimal? H96 { get; set; }

        #endregion
    }

    public class HorasProgramadasDTO
    {
        public int Grupocodi { get; set; }
        public string Gruponom { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public int Hopcodi { get; set; }
        public double Potencia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime HoraInicioReal { get; set; }
        public DateTime HoraFinReal { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public bool EsValido { get; set; }
        public bool EsEdicion { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion

}
