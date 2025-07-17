using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Scada;
using COES.Framework.Base.Core;
using COES.Base.Tools;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using OfficeOpenXml.Drawing;
using System.Configuration;

namespace COES.Servicios.Aplicacion.CortoPlazo
{
    public class ComparativoAppServicio : AppServicioBase
    {
        INDAppServicio indServ = new INDAppServicio();
        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        EjecutadoAppServicio servEjec = new EjecutadoAppServicio();
        RsfAppServicio servRsf = new RsfAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ComparativoAppServicio));

        #region Comparar datos HOP vs Despacho

        #region Filtro

        /// <summary>
        /// Listar filtros de empresas, central y modos
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <param name="listaEmpresa"></param>
        /// <param name="listaCentral"></param>
        /// <param name="listaModo"></param>
        public void ListarFiltroHOvsDespacho(DateTime fechaPeriodo, string emprcodis, string equipadres, out List<SiEmpresaDTO> listaEmpresa
                                                        , out List<EqEquipoDTO> listaCentral, out List<PrGrupoDTO> listaModo)
        {
            List<EveHoraoperacionDTO> listaHoXDia = ListarHoFiltro(fechaPeriodo, emprcodis, equipadres);

            listaEmpresa = listaHoXDia.GroupBy(x => x.Emprcodi).Select(x => new SiEmpresaDTO() { Emprcodi = x.Key, Emprnomb = x.First().Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            listaCentral = listaHoXDia.GroupBy(x => x.Equipadre).Select(x => new EqEquipoDTO() { Equicodi = x.Key, Equinomb = x.First().Central }).OrderBy(x => x.Equinomb).ToList();
            listaModo = listaHoXDia.GroupBy(x => x.Grupocodi).Select(x => new PrGrupoDTO() { Grupocodi = x.Key ?? 0, Gruponomb = x.First().Gruponomb }).OrderBy(x => x.Gruponomb).ToList();
        }

        /// <summary>
        /// Horas de Operación
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <returns></returns>
        private List<EveHoraoperacionDTO> ListarHoFiltro(DateTime fechaPeriodo, string emprcodis, string equipadres)
        {
            string sParamEmpresa = emprcodis != "-1" ? emprcodis : ConstantesHorasOperacion.ParamEmpresaTodos;
            string sParamCentral = equipadres != "-1" ? equipadres : ConstantesHorasOperacion.ParamCentralTodos;
            string sParamCalif = ConstantesHorasOperacion.ParamTipoOperacionTodos;

            List<EveHoraoperacionDTO> listaHOP = servHO.ListarHorasOperacxEquiposXEmpXTipoOPxFam2(Convert.ToInt32(sParamEmpresa), fechaPeriodo, fechaPeriodo.AddDays(1), sParamCalif, Convert.ToInt32(sParamCentral));

            foreach (var reg in listaHOP)
            {
                servHO.FormatearDescripcionesHop(reg);
                reg.Central = reg.PadreNombre;
                reg.Gruponomb = reg.EquipoNombre;
                reg.FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo;
            }

            return listaHOP;
        }

        #endregion

        #region Cálculo

        /// <summary>
        /// Realizar cálculo
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<ReporteComparativoHOvsDespacho> CalcularComparativoHOvsDespacho(DateTime fechaPeriodo, int equipadre, int grupocodi)
        {
            string emprcodis = ConstantesAppServicio.ParametroDefecto;
            string equipadres = equipadre.ToString();

            //mensajes
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            #region Insumos

            // empresas, equipos, modos de operación
            (new INDAppServicio()).ListarUnidadTermicoXEmpresaXCentral(fechaPeriodo, emprcodis, equipadres, out List<SiEmpresaDTO> listaEmpresa
                            , out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaGenerador
                            , out List<PrGrupoDTO> listaGrupoModo, out List<PrGrupoDTO> listaGrupoDespacho);
            List<EqEquipoDTO> listaAllEq = new List<EqEquipoDTO>();
            listaAllEq.AddRange(listaCentral);
            listaAllEq.AddRange(listaGenerador);

            //el ciclo combinado de CT caña brava no debe considerarse como especial
            foreach (var reg in listaGrupoModo)
                reg.TieneModoEspecial = reg.TieneModoEspecial && !reg.TieneModoCicloCombinado;

            //unidades de los modos de operación especiales
            (new INDAppServicio()).ListarUnidadTermicoEspecial(fechaPeriodo, fechaPeriodo, "4, 6, 16, 18, 3, 5, 15, 17", false, out List<EqEquipoDTO> listaUnidadesEsp, out List<EqEquipoDTO> listaCentralEspTermoOut);

            // Horas de Operacion de los modos y unidades especiales
            List<EveHoraoperacionDTO> listaHO = ListarHoXModoyUnidades(fechaPeriodo, emprcodis, equipadres);

            // Despacho CDdispatch 
            List<MeMedicion48DTO> lista48xGrDesp = Listar48DespachoEjecutadoXCentral(fechaPeriodo, ConstantesMedicion.IdTipoGeneracionTermoelectrica, emprcodis, equipadre.ToString(), listaGrupoDespacho, out List<ResultadoValidacionAplicativo> listaMsj6);
            listaMsj.AddRange(listaMsj6);

            // Reserva Secundario
            List<MeMedicion48DTO> lista48RsvSecXGrDesp = Listar48ReservaSecundaria(fechaPeriodo, listaAllEq, out List<PrGrupoDTO> listaUrs);

            //Porcentaje RPF	
            List<PrGrupodatDTO> listaHistoricoRpf = INDAppServicio.ListarPrGrupodatHistoricoDecimalValido(ConstantesIndisponibilidades.ConcepcodiRpf.ToString());
            INDAppServicio.GetValorDecimalFromListaGrupoDat(fechaPeriodo, 0, listaHistoricoRpf, out decimal? valorRpf, out DateTime? fechaVigencia);

            //umbral
            decimal umbral = 0.0m;
            CmUmbralComparacionDTO umbralDTO = FactorySic.GetCmUmbralComparacionRepository().GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);
            if (umbralDTO != null && umbralDTO.Cmumcohopdesp != null) umbral = umbralDTO.Cmumcohopdesp.Value / 100.0m;

            #endregion

            #region Cálculo

            List<ReporteComparativoHOvsDespacho> listaRpt = CalcularComparativoHOvsDespachoXModo(fechaPeriodo, lista48xGrDesp, lista48RsvSecXGrDesp
                                                            , listaHO, valorRpf.GetValueOrDefault(0), umbral
                                                            , listaGrupoModo, listaGrupoDespacho, listaUnidadesEsp, out List<ResultadoValidacionAplicativo> listaMsj1);
            listaMsj.AddRange(listaMsj1);

            #endregion

            if (grupocodi > 0)
                listaRpt = listaRpt.Where(x => x.Grupocodi == grupocodi).ToList();

            return listaRpt;
        }

        /// <summary>
        /// Actualizar datos del modo de operación
        /// </summary>
        /// <param name="f"></param>
        /// <param name="lista48xGrDesp"></param>
        /// <param name="lista48RsvSecXGrDesp"></param>
        /// <param name="listaHO"></param>
        /// <param name="porcentajeRpf"></param>
        /// <param name="umbralMax"></param>
        /// <param name="listaGrupoModo"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaUnidadesEsp"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<ReporteComparativoHOvsDespacho> CalcularComparativoHOvsDespachoXModo(DateTime f, List<MeMedicion48DTO> lista48xGrDesp, List<MeMedicion48DTO> lista48RsvSecXGrDesp
                                    , List<EveHoraoperacionDTO> listaHO, decimal porcentajeRpf, decimal umbralMax
                                    , List<PrGrupoDTO> listaGrupoModo, List<PrGrupoDTO> listaGrupoDespacho, List<EqEquipoDTO> listaUnidadesEsp
                                    , out List<ResultadoValidacionAplicativo> listaMsj)
        {
            List<ReporteComparativoHOvsDespacho> listaRpt = new List<ReporteComparativoHOvsDespacho>();
            listaMsj = new List<ResultadoValidacionAplicativo>();

            List<EveSubcausaeventoDTO> listaSubcausa = servHO.ListarTipoOperacionHO();

            var listaHO30min = HorasOperacionAppServicio.ListarHO30minConsumoCombustible(listaHO, f);
            List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo)
                                                    .OrderBy(x => x.Equipadre).ThenBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

            foreach (var regHo in listaHOModo)
            {
                if (regHo.Hopcodi == 1)
                { }

                PrGrupoDTO regGrupoModo = listaGrupoModo.Find(x => x.Grupocodi == regHo.Grupocodi);
                if (regGrupoModo != null) //validar si la hora de operación tiene un modo con operación comercial para esa fecha
                {
                    List<PrGrupoDTO> listaGrupoDespachoXModo = listaGrupoDespacho.Where(x => regGrupoModo.ListaGrupocodiDespacho.Contains(x.Grupocodi)).ToList();
                    List<EqEquipoDTO> listaUnidadEspXModo = new List<EqEquipoDTO>();
                    if (regGrupoModo.TieneModoEspecial) listaUnidadEspXModo = listaUnidadesEsp.Where(x => x.Grupocodi == regGrupoModo.Grupocodi).ToList();
                    List<EveHoraoperacionDTO> listaHoXModo = listaHOUnidad.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();

                    if (listaGrupoDespachoXModo.Any())
                    {
                        //Obtener el valor de Despacho, reserva secundaria del modo, obtener los datos de Ho para los modos no especiales
                        foreach (var regGrupoDespacho in listaGrupoDespachoXModo)
                        {
                            MeMedicion48DTO reg48Desp = lista48xGrDesp.Find(x => x.Grupocodi == regGrupoDespacho.Grupocodi);
                            MeMedicion48DTO reg48RsvSecUp = lista48RsvSecXGrDesp.Find(x => x.Item == ConstantesCortoPlazo.TipoRsvSecUp && x.Equipadre == regGrupoDespacho.Equipadre);
                            MeMedicion48DTO reg48RsvSecDown = lista48RsvSecXGrDesp.Find(x => x.Item == ConstantesCortoPlazo.TipoRsvSecDown && x.Equipadre == regGrupoDespacho.Equipadre);

                            for (int h = regHo.HIni48; h <= regHo.HFin48; h++)
                            {
                                if (h >= 1 && h <= 48)
                                {
                                    DateTime fi = f.Date.AddMinutes(h * 30);

                                    decimal? valorHDesp = null, valorHRsvUp = null, valorHRsvDown = null;
                                    if (reg48Desp != null) valorHDesp = (decimal?)reg48Desp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48Desp, null);
                                    if (reg48RsvSecUp != null) valorHRsvUp = (decimal?)reg48RsvSecUp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48RsvSecUp, null);
                                    if (reg48RsvSecDown != null) valorHRsvDown = (decimal?)reg48RsvSecDown.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48RsvSecDown, null);

                                    //a cada modo de operacion que tiene hora de operación se le asigna el ejecutado, up, down de cada grupo despacho
                                    AsignarDatosX30minAModo(h, valorHDesp ?? 0, valorHRsvUp, valorHRsvDown, porcentajeRpf
                                                        , f, regGrupoModo, regHo.Grupocodi.Value, regHo.Subcausacodi.Value, listaSubcausa, ref listaRpt);

                                    //luego de asignar, se quita esos valores para que no puedan ser usados en otros modos de operación
                                    if (reg48Desp != null) reg48Desp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg48Desp, 0.0m);
                                    if (reg48RsvSecUp != null) reg48RsvSecUp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg48RsvSecUp, 0.0m);
                                    if (reg48RsvSecDown != null) reg48RsvSecDown.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg48RsvSecDown, 0.0m);

                                    //si es cero el ejecutado lanzar alerta
                                    if (valorHDesp.GetValueOrDefault(0) <= 0)
                                    {
                                        var regCelda = listaRpt.Find(x => x.Grupocodi == regHo.Grupocodi.Value);
                                        if (regCelda.DatoXModo.ListaCalifHo[h - 1] != 0 && regCelda.DatoXModo.ListaMWDespacho[h - 1].GetValueOrDefault(0) == 0)
                                        {
                                            listaMsj.Add(new ResultadoValidacionAplicativo()
                                            {
                                                Equipadre = regGrupoDespacho.Equipadre,
                                                Descripcion = string.Format("La unidad de generación {0} no tiene datos de despacho para la media hora {1}."
                                                                                                                , (regGrupoDespacho.Gruponomb ?? "").Trim(), fi.ToString(ConstantesAppServicio.FormatoFechaFull))
                                            });
                                        }
                                    }
                                }
                            }

                            //validación
                            if (reg48Desp == null)
                                listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regGrupoModo.Equipadre, Descripcion = string.Format("La unidad {0} tiene registros en Horas de Operación pero no registros en Despacho.", (regGrupoDespacho.Gruponomb ?? "").Trim()) });

                        }

                        //Obtener los datos de Ho de cada unidad especial
                        foreach (var regHoUni in listaHoXModo)
                        {
                            EqEquipoDTO regUnidadEsp = listaUnidadEspXModo.Find(x => x.Equicodi == regHoUni.Equicodi);
                            if (regUnidadEsp != null)
                            {
                                for (int h = regHoUni.HIni48; h <= regHoUni.HFin48; h++)
                                {
                                    if (h >= 1 && h <= 48)
                                    {
                                        AsignarDatosX30minEsp(h, porcentajeRpf, regHo.Grupocodi.Value, regUnidadEsp, regHoUni.Equicodi.Value, regHoUni.Subcausacodi.Value, listaSubcausa, ref listaRpt);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regGrupoModo.Equipadre, Descripcion = string.Format("El modo de operación {0} no tienen asociado a ninguna grupo de despacho.", regGrupoModo.Gruponomb) });
                    }
                }
            }

            //buscar si existe MW sin horas de operación
            foreach (var reg48Gen in lista48xGrDesp)
            {
                for (int h = 1; h <= 48; h++)
                {
                    decimal? valorH = (decimal?)reg48Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48Gen, null);
                    DateTime fi = f.Date.AddMinutes(h * 30);

                    if (valorH > 0)
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo()
                        {
                            Equipadre = reg48Gen.Equipadre,
                            Descripcion = string.Format("La unidad de generación {0} no tiene horas de operación para la media hora {1}."
                                                                                                , (reg48Gen.Gruponomb ?? "").Trim(), fi.ToString(ConstantesAppServicio.FormatoFechaFull))
                        });
                    }
                }
            }

            //si no existe horas de operación para la unidad, agregar detalle sin datos de Potencia (solo para visualización)
            foreach (var regModo in listaGrupoModo)
            {
                //obtener los modos de operación de la unidad y la potencia asociada
                var regRptTmp = listaRpt.Find(x => x.Grupocodi == regModo.Grupocodi);

                if (regRptTmp == null)
                {
                    AsignarDatosX30minAModo(1, 0, 0, 0, porcentajeRpf, f, regModo, regModo.Grupocodi, 0, listaSubcausa, ref listaRpt);
                }
            }

            //formatear resultado
            var listaSubc = ConstantesCortoPlazo.ListaCalificacionAlertaHO;

            foreach (var reg in listaRpt)
            {
                bool tieneAlertaCalifNoValida = false;
                for (int h = 1; h <= 48; h++)
                {
                    int pos = h - 1;
                    //generar Tabla Resultado
                    decimal hop = reg.DatoXModo.ListaMWHo[pos].GetValueOrDefault(0);
                    decimal desp = reg.DatoXModo.ListaMWDespacho[pos].GetValueOrDefault(0);

                    reg.DatoXModo.ListaHora[pos] = DateTime.Today.AddMinutes(h * 30).ToString(ConstantesAppServicio.FormatoHora);
                    if (h == 48) reg.DatoXModo.ListaHora[pos] = "23:59";

                    bool tieneCalifXh = listaSubc.Contains(reg.DatoXModo.ListaCalifHo[h - 1]);
                    if (tieneCalifXh)
                    {
                        if (desp != 0 || hop != 0)
                            reg.DatoXModo.ListaDiferencia[pos] = Math.Abs(hop - desp);

                        if (desp != 0)
                            reg.DatoXModo.ListaDesviacion[pos] = reg.DatoXModo.ListaDiferencia[pos] / desp;
                    }

                    reg.DatoXModo.ListaAlerta[pos] = reg.DatoXModo.ListaDesviacion[pos] > umbralMax;

                    //alerta
                    if (reg.DatoXModo.ListaCalifHo[h - 1] != 0)
                    {
                        if (!tieneCalifXh)
                            tieneAlertaCalifNoValida = true;
                    }
                }

                reg.TieneAlertaCalif = tieneAlertaCalifNoValida;
            }

            return listaRpt;
        }

        /// <summary>
        /// Actualizar datos de la Unidad de generación
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorH"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="regUnidad"></param>
        /// <param name="regModo"></param>
        /// <param name="grupocodimodo"></param>
        /// <param name="listaCurva"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaCelda"></param>
        private void AsignarDatosX30minAModo(int h, decimal valorHDesp, decimal? valorHRsvUp, decimal? valorHRsvDown, decimal valorRpf
                                                , DateTime fechaPeriodo, PrGrupoDTO regModo, int grupocodiModo, int subcausacodi
                                                , List<EveSubcausaeventoDTO> listaSubcausa, ref List<ReporteComparativoHOvsDespacho> listaRpt)
        {
            var regReporte = listaRpt.Find(x => x.Grupocodi == grupocodiModo);
            var regSubcausa = listaSubcausa.Find(x => x.Subcausacodi == subcausacodi);

            if (regReporte == null)
            {
                regReporte = new ReporteComparativoHOvsDespacho()
                {
                    Fecha = fechaPeriodo,
                    Grupocodi = grupocodiModo,
                    EsUnidadEspecial = regModo.TieneModoEspecial,
                    Equipadre = regModo.Equipadre,
                };

                regReporte.DatoXModo = new DatoComparativoHOvsDespacho()
                {
                    Grupocodi = grupocodiModo,
                    Modo = regModo,
                    Rpf = valorRpf,
                    Pe = regModo.Potencia ?? 0,
                    Pmin = regModo.PotenciaMinima ?? 0,
                };
                //setear valor
                listaRpt.Add(regReporte);
            }

            if (regModo.TieneModoEspecial)  //si el modo es especial, el valor de HOP se calcula a partir de sus unidades especiales
            {
                subcausacodi = 0;
                regSubcausa = null;
            }

            SetearValorHorasOperacion(regReporte.DatoXModo, h - 1, regReporte.EsUnidadEspecial, valorHDesp, valorHRsvUp, valorHRsvDown, subcausacodi, regSubcausa);
        }

        /// <summary>
        /// Actualizar datos de la Unidad de generación de generación especial
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorRpf"></param>
        /// <param name="grupocodiModo"></param>
        /// <param name="regUnidadEsp"></param>
        /// <param name="equicodi"></param>
        /// <param name="subcausacodi"></param>
        /// <param name="listaSubcausa"></param>
        /// <param name="listaRpt"></param>
        private void AsignarDatosX30minEsp(int h, decimal valorRpf, int grupocodiModo, EqEquipoDTO regUnidadEsp, int equicodi, int subcausacodi
                                            , List<EveSubcausaeventoDTO> listaSubcausa, ref List<ReporteComparativoHOvsDespacho> listaRpt)
        {
            var regReporte = listaRpt.Find(x => x.Grupocodi == grupocodiModo); //el objeto ya fue creado antes 
            var regSubcausa = listaSubcausa.Find(x => x.Subcausacodi == subcausacodi);

            if (regReporte != null)
            {
                var regDatoEspHo = regReporte.ListaDatoXEq.Find(x => x.Equicodi == equicodi);
                if (regDatoEspHo == null)
                {
                    regDatoEspHo = new DatoComparativoHOvsDespacho()
                    {
                        Grupocodi = grupocodiModo,
                        Equicodi = equicodi,
                        EquipoEsp = regUnidadEsp,
                        Rpf = valorRpf,
                        Pe = regUnidadEsp.Pe ?? 0,
                        Pmin = regUnidadEsp.Pmin ?? 0,
                    };

                    regReporte.ListaDatoXEq.Add(regDatoEspHo);
                }

                SetearValorHorasOperacion(regDatoEspHo, h - 1, true, 0, 0, 0, subcausacodi, regSubcausa);

                decimal valorXModo = 0;
                List<string> listaDesc = new List<string>();
                int califHo = -1;
                foreach (var reg in regReporte.ListaDatoXEq.OrderBy(x => x.EquipoEsp.Equiabrev).ToList())
                {
                    valorXModo += reg.ListaMWHo[h - 1] ?? 0;

                    if (!string.IsNullOrEmpty(reg.ListaDescripcionHo[h - 1]))
                    {
                        listaDesc.Add(reg.EquipoEsp.Equiabrev + "\n" + reg.ListaDescripcionHo[h - 1]);
                    }

                    if (reg.ListaCalifHo[h - 1] > 0)
                        califHo = reg.ListaCalifHo[h - 1];
                }
                regReporte.DatoXModo.ListaMWHo[h - 1] = valorXModo;
                regReporte.DatoXModo.ListaDescripcionHo[h - 1] = string.Join("\n - \n", listaDesc);
                regReporte.DatoXModo.ListaCalifHo[h - 1] = califHo;
            }
            else
            {
            }
        }

        /// <summary>
        /// Setear datos cada media hora y obtener valor de HO
        /// </summary>
        /// <param name="regDato"></param>
        /// <param name="pos"></param>
        /// <param name="esEspecial"></param>
        /// <param name="valorHDesp"></param>
        /// <param name="valorHRsvUp"></param>
        /// <param name="valorHRsvDown"></param>
        /// <param name="subcausacodi"></param>
        /// <param name="regSubcausa"></param>
        private void SetearValorHorasOperacion(DatoComparativoHOvsDespacho regDato, int pos, bool esEspecial, decimal valorHDesp, decimal? valorHRsvUp, decimal? valorHRsvDown
                                                , int subcausacodi, EveSubcausaeventoDTO regSubcausa)
        {
            //setear valores por cada H 30min
            regDato.ListaMWDespacho[pos] = (regDato.ListaMWDespacho[pos] ?? 0) + valorHDesp;
            if (valorHRsvUp >= 0)
                regDato.ListaMWRUp[pos] = (regDato.ListaMWRUp[pos] ?? 0) + valorHRsvUp;
            if (valorHRsvDown >= 0)
                regDato.ListaMWRDown[pos] = (regDato.ListaMWRDown[pos] ?? 0) + valorHRsvDown;

            if (regDato.ListaCalifHo[pos] == 0)
            {
                regDato.ListaCalifHo[pos] = subcausacodi;
                regDato.ListaSubcausadesc[pos] = regSubcausa != null ? regSubcausa.Subcausadesc : "";
            }

            //Calcular valor de Horas de operación
            List<string> listaDesc = new List<string>();

            if (!string.IsNullOrEmpty(regDato.ListaSubcausadesc[pos]))
                listaDesc.Add(string.Format("Calificación: {0}", regDato.ListaSubcausadesc[pos]));

            if (regDato.ListaCalifHo[pos] != 0)
            {
                decimal valorHxCalif = 0;

                switch (regDato.ListaCalifHo[pos])
                {
                    case ConstantesSubcausaEvento.SubcausaAMinimaCarga:
                    case ConstantesSubcausaEvento.SubcausaPorRestricOpTemporal:
                        valorHxCalif = regDato.Pmin * (1 + regDato.Rpf);

                        listaDesc.Add(string.Format("Pmin: {0}MW", regDato.Pmin));
                        listaDesc.Add(string.Format("Rpf: {0}%", regDato.Rpf * 100));

                        break;

                    case ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia:
                        valorHxCalif = regDato.Pe * (1 - regDato.Rpf) - regDato.ListaMWRUp[pos].GetValueOrDefault(0) - regDato.ListaMWRDown[pos].GetValueOrDefault(0);

                        listaDesc.Add(string.Format("Pe: {0}MW", regDato.Pe));
                        listaDesc.Add(string.Format("Rpf: {0}%", regDato.Rpf * 100));
                        if (!esEspecial && regDato.ListaMWRUp[pos] >= 0) listaDesc.Add(string.Format("RUp: {0}MW", regDato.ListaMWRUp[pos].GetValueOrDefault(0)));
                        if (!esEspecial && regDato.ListaMWRDown[pos] >= 0) listaDesc.Add(string.Format("RDown: {0}MW", regDato.ListaMWRDown[pos].GetValueOrDefault(0)));

                        break;
                    case ConstantesSubcausaEvento.SubcausaPorPruebas:
                    case ConstantesSubcausaEvento.SubcausaPorPruebasAleatoriasPR25:
                        valorHxCalif = (regDato.Pmin + regDato.Pe) / 2.0m;

                        listaDesc.Add(string.Format("Pmin: {0}MW", regDato.Pmin));
                        listaDesc.Add(string.Format("Pe: {0}MW", regDato.Pe));

                        break;
                    case ConstantesSubcausaEvento.SubcausaPorRsf:
                        valorHxCalif = regDato.Pmin * (1 + regDato.Rpf) + regDato.ListaMWRUp[pos].GetValueOrDefault(0) + regDato.ListaMWRDown[pos].GetValueOrDefault(0);

                        listaDesc.Add(string.Format("Pmin: {0}MW", regDato.Pmin));
                        listaDesc.Add(string.Format("Rpf: {0}%", regDato.Rpf * 100));
                        if (!esEspecial && regDato.ListaMWRUp[pos] >= 0) listaDesc.Add(string.Format("RUp: {0}MW", regDato.ListaMWRUp[pos].GetValueOrDefault(0)));
                        if (!esEspecial && regDato.ListaMWRDown[pos] >= 0) listaDesc.Add(string.Format("RDown: {0}MW", regDato.ListaMWRDown[pos].GetValueOrDefault(0)));

                        break;
                }

                regDato.ListaMWHo[pos] = valorHxCalif;
            }

            regDato.ListaDescripcionHo[pos] = string.Join("\n", listaDesc);
        }

        /// <summary>
        /// INSUMO: Horas de Operación
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <returns></returns>
        private List<EveHoraoperacionDTO> ListarHoXModoyUnidades(DateTime fechaPeriodo, string emprcodis, string equipadres)
        {
            string sParamEmpresa = emprcodis != "-1" ? emprcodis : ConstantesHorasOperacion.ParamEmpresaTodos;
            string sParamCentral = equipadres != "-1" ? equipadres : ConstantesHorasOperacion.ParamCentralTodos;
            List<EveHoraoperacionDTO> listaHOP = servHO.ListarHorasOperacionByCriteria(fechaPeriodo, fechaPeriodo.AddDays(1), sParamEmpresa, sParamCentral, ConstantesHorasOperacion.TipoListadoTodo);
            listaHOP = servHO.CompletarListaHoraOperacionTermo(listaHOP);

            foreach (var reg in listaHOP)
                servHO.FormatearDescripcionesHop(reg);

            return listaHOP;
        }

        /// <summary>
        /// INSUMO: Despacho Ejecutado CDispatch
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> Listar48DespachoEjecutadoXCentral(DateTime fechaPeriodo, int tipoGen, string emprcodis, string equipadres, List<PrGrupoDTO> listaGrupoDespacho, out List<ResultadoValidacionAplicativo> listaMsj)
        {
            listaMsj = new List<ResultadoValidacionAplicativo>();

            List<MeMedicion48DTO> lista48xGrupo = servEjec.ListaDataMDGeneracionConsolidado48(fechaPeriodo, fechaPeriodo, ConstantesMedicion.IdTipogrupoCOES
                                                      , tipoGen.ToString(), emprcodis, ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString()
                                                      , false, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);

            foreach (var reg in lista48xGrupo)
            {
                reg.Equinomb = (reg.Gruponomb ?? "").Trim();
                reg.Gruponomb = (reg.Gruponomb ?? "").Trim();
            }

            //incluir registros para grupos que no tienen data
            foreach (var reg in listaGrupoDespacho)
            {
                if (lista48xGrupo.Find(x => x.Grupocodi == reg.Grupocodi) == null)
                {
                    MeMedicion48DTO reg48 = new MeMedicion48DTO();
                    reg48.Emprcodi = reg.Emprcodi ?? 0;
                    reg48.Emprnomb = reg.Emprnomb;
                    reg48.Equipadre = reg.Equipadre;
                    reg48.Central = (reg.Central ?? "").Trim();
                    reg48.Equicodi = reg.Equicodi;
                    reg48.Equinomb = reg.Equinomb;
                    reg48.Grupocodi = reg.Grupocodi;
                    reg48.Gruponomb = reg.Gruponomb;
                    reg48.Medifecha = fechaPeriodo;

                    lista48xGrupo.Add(reg48);

                    listaMsj.Add(new ResultadoValidacionAplicativo()
                    {
                        Equipadre = reg48.Equipadre,
                        Descripcion = string.Format("El Grupo {0} no tiene información de Despacho Ejecutado.", reg48.Gruponomb)
                    });
                }
            }

            if (ConstantesAppServicio.ParametroDefecto != equipadres)
            {
                int[] listaequipadres = equipadres.Split(',').Select(x => int.Parse(x)).ToArray();
                lista48xGrupo = lista48xGrupo.Where(x => listaequipadres.Contains(x.Equipadre)).ToList();
            }

            return lista48xGrupo;
        }

        /// <summary>
        /// INSUMO: Listar información de reserva secundaria
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaAllEquipos"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> Listar48ReservaSecundaria(DateTime fechaPeriodo, List<EqEquipoDTO> listaAllEquipos, out List<PrGrupoDTO> listaUrs)
        {
            List<EveRsfdetalleDTO> listaConfiguracionURSEquipos = servRsf.ObtenerConfiguracion(fechaPeriodo)
                                                                    .Where(x => x.Grupotipo == "S").ToList(); //solo tomar a los grupos generadores de tipo central
            List<EveRsfequivalenciaDTO> equivalenciaAgc = FactorySic.GetEveRsfequivalenciaRepository().List();

            //obtener las urs
            listaUrs = listaConfiguracionURSEquipos.GroupBy(x => x.Grupocodi)
                                    .Select(x => new PrGrupoDTO()
                                    {
                                        Grupocodi = x.Key ?? 0,
                                        Gruponomb = x.First().Ursnomb,
                                        Emprnomb = x.First().Emprnomb,
                                        Central = x.First().Gruponomb,
                                        ListaEquicodi = x.Select(y => y.Equicodi ?? 0).Distinct().ToList()
                                    }).ToList();
            foreach (var regUrs in listaUrs)
            {
                string codAGC = string.Empty;
                foreach (var equicodi in regUrs.ListaEquicodi)
                {
                    //obtener el primer agc
                    if (string.IsNullOrEmpty(codAGC))
                    {
                        EveRsfequivalenciaDTO equiv = equivalenciaAgc.Where(x => x.Equicodi == equicodi).FirstOrDefault();

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
                        }
                    }
                }

                regUrs.CodAgc = codAGC;
            }

            //convertir la información de eve_rsf a medicion48
            List<EveRsfhoraDTO> horas = FactorySic.GetEveRsfhoraRepository().GetByCriteria(fechaPeriodo);
            List<EveRsfdetalleDTO> detalle = FactorySic.GetEveRsfdetalleRepository().ObtenerDetalleReserva(fechaPeriodo);

            List<MeMedicion48DTO> listaResultado = new List<MeMedicion48DTO>();
            foreach (var regUrsEq in listaConfiguracionURSEquipos)
            {
                EqEquipoDTO regEq = listaAllEquipos.Find(x => x.Equicodi == regUrsEq.Equicodi);

                //si es central entonces poner el grupo despacho del generador
                if (regEq != null && regEq.Famcodi == ConstantesHorasOperacion.IdTipoTermica)
                {
                    regEq = listaAllEquipos.Find(x => x.Equipadre == regUrsEq.Equicodi && x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                }

                if (regEq != null) //si es un equipo termico
                {
                    MeMedicion48DTO m48Up = new MeMedicion48DTO();
                    m48Up.Equicodi = regEq.Equicodi;
                    m48Up.Equipadre = regEq.Equipadre ?? 0;
                    m48Up.Grupocodi = regEq.Grupocodi ?? 0; //grupo despacho del generador
                    m48Up.Gruponomb = regUrsEq.Gruponomb;
                    m48Up.Equipadre = regEq.Equipadre ?? 0;
                    m48Up.Grupourspadre = regUrsEq.Grupocodi ?? 0;
                    m48Up.Item = ConstantesCortoPlazo.TipoRsvSecUp;

                    MeMedicion48DTO m48Down = new MeMedicion48DTO();
                    m48Down.Equicodi = regEq.Equicodi;
                    m48Down.Equipadre = regEq.Equipadre ?? 0;
                    m48Down.Grupocodi = regEq.Grupocodi ?? 0; //grupo despacho del generador
                    m48Down.Gruponomb = regUrsEq.Gruponomb;
                    m48Down.Equipadre = regEq.Equipadre ?? 0;
                    m48Down.Grupourspadre = regUrsEq.Grupocodi ?? 0;
                    m48Down.Item = ConstantesCortoPlazo.TipoRsvSecDown;

                    for (int h = 1; h <= 48; h++)
                    {
                        m48Up.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(m48Up, 0.0m);
                        m48Down.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(m48Down, 0.0m);

                        EveRsfhoraDTO hora = horas.Find(x => x.Rsfhorfin == fechaPeriodo.AddMinutes(h * 30));
                        if (hora != null)
                        {
                            EveRsfdetalleDTO registroDetUrs = detalle.Find(x => hora.Rsfhorcodi == x.Rsfhorcodi && x.Grupocodi == regUrsEq.Grupocodi && x.Equicodi == regUrsEq.Equicodi);
                            if (registroDetUrs != null)
                            {
                                m48Up.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(m48Up, registroDetUrs.Rsfdetsub);
                                m48Down.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(m48Down, registroDetUrs.Rsfdetbaj);
                            }
                        }
                    }

                    listaResultado.Add(m48Up);
                    listaResultado.Add(m48Down);
                }
            }

            return listaResultado;
        }

        #endregion

        #region Web y excel

        /// <summary>
        /// Generar Reporte web (tabla y gráfico)
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<ReporteComparativoHOvsDespacho> GenerarReporteWebHOvsDespacho(int tipoReporte, DateTime fechaPeriodo, int emprcodi, int equipadre, int grupocodi)
        {
            List<ReporteComparativoHOvsDespacho> listaRpt = new List<ReporteComparativoHOvsDespacho>();

            if (ConstantesCortoPlazo.TipoCompHOvsDesp == tipoReporte)
                listaRpt = CalcularComparativoHOvsDespacho(fechaPeriodo, equipadre, grupocodi);
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte)
                listaRpt = CalcularComparativoEMSvsDespacho(fechaPeriodo, emprcodi, equipadre, grupocodi);

            foreach (var obj in listaRpt)
            {
                obj.ReporteHtml = GenerarReporteHtml(tipoReporte, obj);
                obj.Grafico = ObtenerGraficoXModo(tipoReporte, obj);
            }

            return listaRpt;
        }

        /// <summary>
        /// Generar reporte html
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GenerarReporteHtml(int tipoReporte, ReporteComparativoHOvsDespacho obj)
        {
            NumberFormatInfo nfi2 = new CultureInfo("en-US", false).NumberFormat;
            nfi2.NumberGroupSeparator = " ";
            nfi2.NumberDecimalDigits = 2;
            nfi2.NumberDecimalSeparator = ",";

            string descComp = "HOP <br>(MW)";
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) descComp = "Generación <br/> EMS (MW)";
            DatoComparativoHOvsDespacho objGr = obj.DatoXModo;
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) objGr = obj.DatoXGrupo;

            StringBuilder str = new StringBuilder();

            if (ConstantesCortoPlazo.TipoCompHOvsDesp == tipoReporte)
            {
                if (obj.TieneAlertaCalif)
                {
                    str.AppendFormat(@"
                        <div class='action-alert' style='margin:0; margin-bottom:10px'>El modo de operación seleccionado no tiene una calificación válida.</div>
                    ");
                }
            }

            str.AppendFormat(@"
                   <table class='pretty tabla-adicional tbl_comparativo' border='0' cellspacing='0' style='width: 400px;'>
                    <thead>
                        <tr>
                            <th style='width: 40px;'>Hora</th>
                            <th style='width: 90px;'>{0}</th>
                            <th style='width: 90px;'>Despacho <br> ejecutado (MW)</th>
                            <th style='width: 90px;'>Diferencia <br>(MW)</th>
                            <th style='width: 90px;'>Desviación <br> (%)</th>
                        </tr>
                    </thead>
                    <tbody>
            ", descComp);

            for (int i = 0; i < 48; i++)
            {
                string claseAlerta = objGr.ListaAlerta[i] ? "tr_alerta" : string.Empty;

                string hop = string.Empty;
                if (ConstantesCortoPlazo.TipoCompHOvsDesp == tipoReporte) hop = objGr.ListaMWHo[i].GetValueOrDefault(0) > 0 ? (objGr.ListaMWHo[i].Value).ToString("N", nfi2) : string.Empty;
                if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) hop = objGr.ListaMWEms[i].GetValueOrDefault(0) > 0 ? (objGr.ListaMWEms[i].Value).ToString("N", nfi2) : string.Empty;

                string desp = objGr.ListaMWDespacho[i].GetValueOrDefault(0) > 0 ? (objGr.ListaMWDespacho[i].Value).ToString("N", nfi2) : string.Empty;
                string dif = objGr.ListaDiferencia[i] != null ? (objGr.ListaDiferencia[i].Value).ToString("N", nfi2) : string.Empty;
                string desv = objGr.ListaDesviacion[i] != null ? (objGr.ListaDesviacion[i].Value * 100).ToString("N", nfi2) : string.Empty;

                str.AppendFormat(@"
                        <tr class='{5}'>
                            <td style='text-align: center;'>{0}</td>
                            <td style='text-align: center;'>{1}</td>
                            <td style='text-align: center;'>{2}</td>
                            <td style='text-align: center;'>{3}</td>
                            <td style='text-align: center;'>{4}</td>
                        </tr>
                ", objGr.ListaHora[i]
                , hop
                , desp
                , dif
                , desv
                , claseAlerta);
            }

            str.Append(@"
                    </tbody>
                </table>
            ");

            return str.ToString();
        }

        /// <summary>
        /// Generar objeto grafico web
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private GraficoWeb ObtenerGraficoXModo(int tipoReporte, ReporteComparativoHOvsDespacho obj)
        {
            GraficoWeb grafico = new GraficoWeb();

            if (ConstantesCortoPlazo.TipoCompHOvsDesp == tipoReporte)
                grafico.TitleText = string.Format("Comparativo Horas de Operación vs Despacho Ejecutado - {0}", obj.DatoXModo.Modo.Gruponomb);
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte)
                grafico.TitleText = string.Format("Comparativo Generación EMS vs Despacho Ejecutado - {0}", obj.DatoXGrupo.GrupoDesp.Gruponomb);

            string descComp = "Horas de Operación";
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) descComp = "Generación EMS";

            grafico.Series = new List<RegistroSerie>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesName = new List<string>();
            grafico.YAxixTitle = new List<string>();

            grafico.SerieDataS = new DatosSerie[2][];
            grafico.Series.Add(new RegistroSerie());
            grafico.Series.Add(new RegistroSerie());

            grafico.YAxixTitle.Add("Potencia (MW)");
            grafico.XAxisTitle = "Dia:Horas";

            grafico.Series[0].Name = descComp;
            grafico.Series[0].Type = "line";
            grafico.Series[0].Color = "#7CB5EC";
            grafico.Series[0].YAxisTitle = "MW";

            grafico.Series[1].Name = "Despacho Ejecutado";
            grafico.Series[1].Type = "line";
            grafico.Series[1].Color = "#ED7D31";
            grafico.Series[1].YAxisTitle = "MW";

            int numDia = 1;
            grafico.SerieDataS[0] = new DatosSerie[48 * numDia];
            grafico.SerieDataS[1] = new DatosSerie[48 * numDia];

            // titulo el reporte
            grafico.XAxisCategories = new List<string>();
            grafico.SeriesType = new List<string>();
            grafico.SeriesYAxis = new List<int>();
            grafico.SeriesYAxis.Add(0);

            DatoComparativoHOvsDespacho objGr = obj.DatoXModo;
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) objGr = obj.DatoXGrupo;

            decimal?[] listaHo = objGr.ListaMWHo;
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) listaHo = objGr.ListaMWEms;
            string[] listaDesc = objGr.ListaDescripcionHo;
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) listaDesc = objGr.ListaDescripcionEMS;

            for (var j = 0; j < 48; j++)
            {
                decimal? valor1 = listaHo[j];
                decimal? valor2 = objGr.ListaMWDespacho[j];

                grafico.XAxisCategories.Add(objGr.ListaHora[j]);

                var serieHo = new DatosSerie();
                serieHo.X = obj.Fecha.AddMinutes((j + 1) * 30);
                serieHo.Y = valor1;
                serieHo.Type = (listaDesc[j] ?? "").Trim();

                var serieDespacho = new DatosSerie();
                serieDespacho.X = obj.Fecha.AddMinutes((j + 1) * 30);
                serieDespacho.Y = valor2;

                grafico.SerieDataS[0][j] = serieHo;
                grafico.SerieDataS[1][j] = serieDespacho;
            }

            return grafico;
        }

        /// <summary>
        /// Generar Reporte Excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <param name="nameFile"></param>
        public void GenerarExcelComparativoHOvsDespacho(string ruta, int tipoReporte, DateTime fechaPeriodo, int emprcodi, int equipadre, int grupocodi, out string nameFile)
        {
            List<ReporteComparativoHOvsDespacho> listaRpt = new List<ReporteComparativoHOvsDespacho>();

            if (ConstantesCortoPlazo.TipoCompHOvsDesp == tipoReporte)
                listaRpt = CalcularComparativoHOvsDespacho(fechaPeriodo, equipadre, grupocodi);
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte)
                listaRpt = CalcularComparativoEMSvsDespacho(fechaPeriodo, emprcodi, equipadre, grupocodi);

            foreach (var obj in listaRpt)
            {
                obj.Grafico = ObtenerGraficoXModo(tipoReporte, obj);
            }

            //Nombre de archivo
            nameFile = string.Format("ComparativoHOPvsDespachoEjecutado_{0}.xlsx", fechaPeriodo.ToString(ConstantesAppServicio.FormatoFechaDMY));
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte)
                nameFile = string.Format("ComparativoGeneracionEMSvsDespachoEjecutado_{0}.xlsx", fechaPeriodo.ToString(ConstantesAppServicio.FormatoFechaDMY));

            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                var i = 0;
                foreach (var reg in listaRpt)
                {
                    string nameWs = "Comparativo";
                    if (i > 0) nameWs = "Comparativo" + i;

                    GenerarHojaExcelDetalle(xlPackage, nameWs, 9, 2, tipoReporte, reg);
                    xlPackage.Save();

                    i++;
                }
            }
        }

        /// <summary>
        /// Generar detalle por modo de operación
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="obj"></param>
        /// <param name="fechaConsulta"></param>
        private void GenerarHojaExcelDetalle(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, int tipoReporte, ReporteComparativoHOvsDespacho obj)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string font = "Calibri";
            string colorCeldaCs = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int rowTitulo = 6;
            int rowFecha = rowTitulo + 1;

            ws.Cells[rowTitulo, colIniTabla].Value = "Comparativo Horas de Operación vs Despacho Ejecutado";
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte)
                ws.Cells[rowTitulo, colIniTabla].Value = "Comparativo Generación EMS vs Despacho Ejecutado";
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colIniTabla, rowTitulo, colIniTabla, "Centro", "Izquierda", "#000000", "#FFFFFF", font, 14, true);

            ws.Cells[rowFecha, colIniTabla].Value = "Fecha: ";
            ws.Cells[rowFecha, colIniTabla + 1].Value = obj.Fecha.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.SetFormatoCelda(ws, rowFecha, colIniTabla, rowFecha, colIniTabla, "Centro", "Derecha", "#000000", "#FFFFFF", font, 12, true);

            //
            int colHora = colIniTabla;
            int colHop = colHora + 1;
            int colEjec = colHop + 1;
            int colDif = colEjec + 1;
            int colDesv = colDif + 1;

            int rowIniCabecera = rowIniTabla;

            ws.Cells[rowIniCabecera, colHora].Value = "HORA";
            ws.Cells[rowIniCabecera, colHop].Value = "HOP \n (MW)";
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte)
                ws.Cells[rowIniCabecera, colHop].Value = "Generación \n EMS (MW)";
            ws.Cells[rowIniCabecera, colEjec].Value = "Despacho \n ejecutado (MW)";
            ws.Cells[rowIniCabecera, colDif].Value = "Diferencia \n (MW)";
            ws.Cells[rowIniCabecera, colDesv].Value = "Desviación \n (%)";

            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colHora, rowIniCabecera, colDesv, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniCabecera, colHora, rowIniCabecera, colDesv, colorLinea, true);

            ws.Column(1).Width = 3;
            ws.Column(colHora).Width = 10;
            ws.Column(colHop).Width = 18;
            ws.Column(colEjec).Width = 18;
            ws.Column(colDif).Width = 18;
            ws.Column(colDesv).Width = 18;

            #endregion

            #region Cuerpo

            int numDecimalMw = 2;

            int rowData = rowIniCabecera + 1;
            int rowIniData = rowData;

            DatoComparativoHOvsDespacho objGr = obj.DatoXModo;
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) objGr = obj.DatoXGrupo;
            decimal?[] listaHo = objGr.ListaMWHo;
            if (ConstantesCortoPlazo.TipoCompEMSvsDesp == tipoReporte) listaHo = objGr.ListaMWEms;

            for (int i = 0; i < 48; i++)
            {
                ws.Cells[rowData, colHora].Value = objGr.ListaHora[i];
                UtilExcel.SetFormatoCelda(ws, rowData, colHora, rowData, colHora, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true);

                //
                if (listaHo[i] > 0)
                    ws.Cells[rowData, colHop].Value = listaHo[i];
                if (objGr.ListaMWDespacho[i] > 0)
                    ws.Cells[rowData, colEjec].Value = objGr.ListaMWDespacho[i];
                ws.Cells[rowData, colDif].Value = objGr.ListaDiferencia[i];
                ws.Cells[rowData, colDesv].Value = objGr.ListaDesviacion[i];

                UtilExcel.CeldasExcelFormatoNumero(ws, rowData, colHop, rowData, colDif, numDecimalMw);
                UtilExcel.CeldasExcelFormatoPorcentaje(ws, rowData, colDesv, rowData, colDesv, numDecimalMw);

                string colorAlerta = objGr.ListaAlerta[i] ? "#FFB4B4" : "#FFFFFF";
                UtilExcel.SetFormatoCelda(ws, rowData, colHop, rowData, colDesv, "Centro", "Centro", "#000000", colorAlerta, font, 12, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colHora, rowData, colDesv, colorLinea, true);

                rowData++;
            }

            #endregion

            #region Gráfico

            int rowIniGr = rowIniData;
            int colIniGr = colDesv + 1;

            var lineaChart = ws.Drawings.AddChart("grafico", eChartType.LineMarkers) as ExcelLineChart;
            lineaChart.SetPosition(rowIniGr, 0, colIniGr, 0);
            lineaChart.SetSize(1100, 540);

            lineaChart.DisplayBlanksAs = eDisplayBlanksAs.Gap;
            lineaChart.Title.Text = obj.Grafico.TitleText;
            lineaChart.DataLabel.ShowLeaderLines = true;
            lineaChart.YAxis.Title.Text = obj.Grafico.YAxixTitle.First();
            lineaChart.YAxis.MinValue = 0.0;
            lineaChart.Legend.Position = eLegendPosition.Bottom;
            //lineaChart.XAxis.Orientation = eAxisOrientation.MaxMin;

            var rangoHora = ws.Cells[rowIniData, colHora, rowIniData + 47, colHora];
            var rangoHo = ws.Cells[rowIniData, colHop, rowIniData + 47, colHop];
            var rangoDesp = ws.Cells[rowIniData, colEjec, rowIniData + 47, colEjec];

            var serieHo = (ExcelChartSerie)lineaChart.Series.Add(rangoHo, rangoHora);
            serieHo.Header = obj.Grafico.Series[0].Name;

            var serieDesp = (ExcelChartSerie)lineaChart.Series.Add(rangoDesp, rangoHora);
            serieDesp.Header = obj.Grafico.Series[1].Name;

            #endregion

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());

            UtilExcel.AddImage(ws, img, 1, 2);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.FreezePanes(rowIniCabecera + 1, colHora + 1);

            ws.View.ZoomScale = 80;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;

        }

        #endregion

        #endregion

        #region Comparar datos Generación EMS vs Despacho Ejecutado

        #region Filtro

        /// <summary>
        /// Listar filtros de empresas, central y grupos
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <param name="listaEmpresa"></param>
        /// <param name="listaCentral"></param>
        /// <param name="listaGrupo"></param>
        public void ListarFiltroEMSvsDespacho(DateTime fechaPeriodo, string emprcodis, string equipadres, out List<SiEmpresaDTO> listaEmpresa
                                                        , out List<PrGrupoDTO> listaCentral, out List<PrGrupoDTO> listaGrupo, out List<string> listaVal)
        {
            ListarM48Filtro(fechaPeriodo, emprcodis, equipadres, out List<MeMedicion48DTO> listaM48XDia, out List<PrGrupoDTO> listaGrupoActivo, out listaVal);

            listaEmpresa = listaM48XDia.GroupBy(x => x.Emprcodi).Select(x => new SiEmpresaDTO() { Emprcodi = x.Key, Emprnomb = x.First().Emprnomb }).OrderBy(x => x.Emprnomb).ToList();
            listaCentral = listaM48XDia.GroupBy(x => x.Grupopadre).Select(x => new PrGrupoDTO() { Grupocodi = x.Key, Gruponomb = x.First().Grupocentral }).OrderBy(x => x.Gruponomb).ToList();
            listaGrupo = listaM48XDia.GroupBy(x => x.Grupocodi).Select(x => new PrGrupoDTO() { Grupocodi = x.Key, Gruponomb = x.First().Gruponomb }).OrderBy(x => x.Gruponomb).ToList();
        }

        /// <summary>
        /// Obtener la data de Medición48 necesario para los filtros 
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <returns></returns>
        private void ListarM48Filtro(DateTime fechaPeriodo, string emprcodis, string grupopadres
                                    , out List<MeMedicion48DTO> lista48xGrupo, out List<PrGrupoDTO> listaGrupoActivo, out List<string> listaVal)
        {
            lista48xGrupo = servEjec.ListaDataMDGeneracionConsolidado48(fechaPeriodo, fechaPeriodo, ConstantesMedicion.IdTipogrupoCOES
                                                      , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), emprcodis, ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString()
                                                      , false, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);

            //Lista de grupos validos (no eliminados)
            List<PrGrupoDTO> listaGrupo = (new INDAppServicio()).ListarGrupoValido().ToList();

            SetearGrupoCentralAM48(lista48xGrupo, listaGrupo, out listaGrupoActivo, out listaVal);

            if (ConstantesAppServicio.ParametroDefecto != grupopadres)
            {
                int[] listaequipadres = grupopadres.Split(',').Select(x => int.Parse(x)).ToArray();

                lista48xGrupo = lista48xGrupo.Where(x => listaequipadres.Contains(x.Grupopadre)).ToList();
            }
        }

        #endregion

        #region Cálculo

        /// <summary>
        /// Calcular diferencias de un rango de dias
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="lFinal"></param>
        /// <param name="listaGrupoDespacho"></param>
        public void CalcularDiferenciaEMSvsDespacho(DateTime fechaIni, DateTime fechaFin, out List<ReporteComparativoHOvsDespacho> lFinal, out List<PrGrupoDTO> listaGrupoDespacho)
        {
            //Listar data de los dias seleccionados
            lFinal = new List<ReporteComparativoHOvsDespacho>();

            for (DateTime day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                var listaRptXDia = CalcularComparativoEMSvsDespacho(day, -1, -1, -1);
                lFinal.AddRange(listaRptXDia);
            }

            //obtener grupos de despacho que será la cabecera de las hojas
            listaGrupoDespacho = new List<PrGrupoDTO>();

            List<int> listaGrupocodi = lFinal.Select(x => x.Grupocodi).Distinct().ToList();
            foreach (var grupocodi in listaGrupocodi)
            {
                //se obtiene el ultimo para obtener la empresa más reciente en caso de TTIE
                var regGrupo = lFinal.LastOrDefault(x => x.DatoXGrupo.GrupoDesp.Grupocodi == grupocodi);
                if (regGrupo != null)
                {
                    listaGrupoDespacho.Add(regGrupo.DatoXGrupo.GrupoDesp);
                }
            }

            listaGrupoDespacho = listaGrupoDespacho.OrderBy(x => x.Emprnomb).ThenBy(x => x.Gruponomb).ToList();
        }

        /// <summary>
        /// Realizar cálculo
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodi"></param>
        /// <param name="grupopadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public List<ReporteComparativoHOvsDespacho> CalcularComparativoEMSvsDespacho(DateTime fechaPeriodo, int emprcodi, int grupopadre, int grupocodi)
        {
            string emprcodis = ConstantesAppServicio.ParametroDefecto;

            //mensajes
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            #region Insumos

            // empresas, equipos, modos de operación
            ListarGrupoDespacho(fechaPeriodo, emprcodi.ToString(), grupopadre.ToString(), out List<PrGrupoDTO> listaAllGrupo, out List<PrGrupoDTO> listaGrupoDespacho, out List<EqEquipoDTO> listaAllEq);

            // Despacho CDdispatch 
            List<MeMedicion48DTO> lista48xGrDesp = Listar48DespachoEjecutadoXGrupoCentral(fechaPeriodo, ConstantesMedicion.IdTipoGeneracionTodos, emprcodis, grupopadre.ToString()
                                                    , listaGrupoDespacho, listaAllGrupo, out List<string> listaMsj6);

            // EMS
            string equipadres = listaAllEq.Any() ? string.Join(",", listaAllEq.Select(x => x.Equipadre).Distinct()) : ConstantesAppServicio.ParametroDefecto;
            ListarGeneracionEMS48(fechaPeriodo, emprcodis, equipadres, out List<MeMedicion48DTO> lista48EmsXEq, out List<MeMedicion48DTO> listaEmsOperativo);

            //umbral
            decimal umbralMW = 0.0m;
            CmUmbralComparacionDTO umbralDTO = FactorySic.GetCmUmbralComparacionRepository().GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);
            if (umbralDTO != null && umbralDTO.Cmumcoemsdesp != null) umbralMW = umbralDTO.Cmumcoemsdesp.Value;

            #endregion

            #region Cálculo

            List<ReporteComparativoHOvsDespacho> listaRpt = CalcularComparativoEMSvsDespachoXGrupo(fechaPeriodo, lista48xGrDesp, lista48EmsXEq, listaEmsOperativo
                                                            , umbralMW
                                                            , listaGrupoDespacho, listaAllEq, out List<ResultadoValidacionAplicativo> listaMsj1);
            listaMsj.AddRange(listaMsj1);

            #endregion

            if (grupocodi > 0)
                listaRpt = listaRpt.Where(x => x.Grupocodi == grupocodi).ToList();

            return listaRpt.OrderBy(x => x.DatoXGrupo.GrupoDesp.Emprnomb).ThenBy(x => x.DatoXGrupo.GrupoDesp.Central).ThenBy(x => x.DatoXGrupo.GrupoDesp.Gruponomb).ToList();
        }

        /// <summary>
        /// REalizar calculo por grupo de despacho
        /// </summary>
        /// <param name="f"></param>
        /// <param name="lista48xGrDesp"></param>
        /// <param name="lista48EmsMWEq"></param>
        /// <param name="lista48EmsFlagEq"></param>
        /// <param name="umbralMWMax"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaEquipo"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<ReporteComparativoHOvsDespacho> CalcularComparativoEMSvsDespachoXGrupo(DateTime f, List<MeMedicion48DTO> lista48xGrDesp
                                    , List<MeMedicion48DTO> lista48EmsMWEq, List<MeMedicion48DTO> lista48EmsFlagEq
                                    , decimal umbralMWMax
                                    , List<PrGrupoDTO> listaGrupoDespacho, List<EqEquipoDTO> listaEquipo
                                    , out List<ResultadoValidacionAplicativo> listaMsj)
        {
            List<ReporteComparativoHOvsDespacho> listaRpt = new List<ReporteComparativoHOvsDespacho>();
            listaMsj = new List<ResultadoValidacionAplicativo>();

            foreach (var reg48Desp in lista48xGrDesp)
            {
                PrGrupoDTO regGrupoDesp = listaGrupoDespacho.Find(x => x.Grupocodi == reg48Desp.Grupocodi);
                if (regGrupoDesp != null)
                {
                    //setear datos de despacho
                    for (int h = 1; h <= 48; h++)
                    {
                        DateTime fi = f.Date.AddMinutes(h * 30);

                        decimal? valorHDesp = (decimal?)reg48Desp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48Desp, null);

                        AsignarDatosX30minAGrupoDespacho(h, valorHDesp, f, regGrupoDesp, regGrupoDesp.Grupocodi, ref listaRpt);
                    }

                    //Obtener la información de cada equipo EMS
                    List<EqEquipoDTO> listaEqXDesp = listaEquipo.Where(x => regGrupoDesp.ListaEquicodi.Contains(x.Equicodi)).ToList();
                    if (listaEqXDesp.Any())
                    {
                        foreach (var regEq in listaEqXDesp)
                        {
                            MeMedicion48DTO reg48EmsEq = lista48EmsMWEq.Find(x => x.Equicodi == regEq.Equicodi);
                            MeMedicion48DTO reg48EmsFlagEq = lista48EmsFlagEq.Find(x => x.Equicodi == regEq.Equicodi);
                            for (int h = 1; h <= 48; h++)
                            {
                                DateTime fi = f.Date.AddMinutes(h * 30);

                                decimal? valorHEq = null;
                                if (reg48EmsEq != null) valorHEq = (decimal?)reg48EmsEq.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48EmsEq, null);

                                int? valorFlagEq = null;
                                if (reg48EmsFlagEq != null) valorFlagEq = (int?)((decimal?)reg48EmsFlagEq.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48EmsFlagEq, null));

                                AsignarDatosX30minAEqEms(h, valorHEq, valorFlagEq, reg48EmsEq != null, regGrupoDesp.Grupocodi, regEq, regEq.Equicodi, ref listaRpt);
                            }
                        }
                    }
                    else
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regGrupoDesp.Equipadre, Descripcion = string.Format("El grupo de despacho {0} no tienen asociado a ningun equipo.", reg48Desp.Gruponomb) });
                    }
                }
            }

            ////buscar si existe MW sin horas de operación
            //foreach (var reg48Gen in lista48xGrDesp)
            //{
            //    for (int h = 1; h <= 48; h++)
            //    {
            //        decimal? valorH = (decimal?)reg48Gen.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48Gen, null);
            //        DateTime fi = f.Date.AddMinutes(h * 30);

            //        if (valorH > 0)
            //        {
            //            listaMsj.Add(new ResultadoValidacionAplicativo()
            //            {
            //                Equipadre = reg48Gen.Equipadre,
            //                Descripcion = string.Format("La unidad de generación {0} no tiene horas de operación para la media hora {1}."
            //                                                                                    , (reg48Gen.Gruponomb ?? "").Trim(), fi.ToString(ConstantesAppServicio.FormatoFechaFull))
            //            });
            //        }
            //    }
            //}

            //si no existe horas de operación para la unidad, agregar detalle sin datos de Potencia (solo para visualización)
            foreach (var regGrupoDesp in listaGrupoDespacho)
            {
                //obtener los modos de operación de la unidad y la potencia asociada
                var regRptTmp = listaRpt.Find(x => x.Grupocodi == regGrupoDesp.Grupocodi);

                if (regRptTmp == null)
                {
                    AsignarDatosX30minAGrupoDespacho(1, 0, f, regGrupoDesp, regGrupoDesp.Grupocodi, ref listaRpt);
                }
            }

            //formatear resultado
            foreach (var reg in listaRpt)
            {
                for (int h = 1; h <= 48; h++)
                {
                    int pos = h - 1;
                    //generar Tabla Resultado
                    decimal ems = reg.DatoXGrupo.ListaMWEms[pos].GetValueOrDefault(0);
                    decimal desp = reg.DatoXGrupo.ListaMWDespacho[pos].GetValueOrDefault(0);

                    reg.DatoXGrupo.ListaHora[pos] = DateTime.Today.AddMinutes(h * 30).ToString(ConstantesAppServicio.FormatoHora);
                    if (h == 48) reg.DatoXGrupo.ListaHora[pos] = "23:59";

                    if (reg.DatoXGrupo.TieneEqConfEms)
                    {
                        if (desp != 0 || ems != 0)
                            reg.DatoXGrupo.ListaDiferencia[pos] = Math.Abs(ems - desp);

                        if (desp != 0)
                            reg.DatoXGrupo.ListaDesviacion[pos] = reg.DatoXGrupo.ListaDiferencia[pos] / desp;

                        reg.DatoXGrupo.ListaAlerta[pos] = reg.DatoXGrupo.ListaDiferencia[pos] > umbralMWMax; // || (desp != 0 && ems == 0) || (desp == 0 && ems != 0);
                    }
                }

                //mostrar mensajes                
                foreach (var regEq in reg.ListaDatoXEq)
                {
                    if (!regEq.TieneEqConfEms)
                    {
                        reg.DatoXGrupo.ListaMensaje.Add(string.Format("El generador {0} - {1} no tiene configuración EMS.", regEq.EquipoEsp.Central, regEq.EquipoEsp.Equiabrev));
                    }
                }
            }

            return listaRpt;
        }

        /// <summary>
        /// Actualizar datos del grupo de despacho
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorHDesp"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="regDesp"></param>
        /// <param name="grupocodiDesp"></param>
        /// <param name="listaRpt"></param>
        private void AsignarDatosX30minAGrupoDespacho(int h, decimal? valorHDesp, DateTime fechaPeriodo
                                        , PrGrupoDTO regDesp, int grupocodiDesp, ref List<ReporteComparativoHOvsDespacho> listaRpt)
        {
            var regReporte = listaRpt.Find(x => x.Grupocodi == grupocodiDesp);

            if (regReporte == null)
            {
                regReporte = new ReporteComparativoHOvsDespacho()
                {
                    Fecha = fechaPeriodo,
                    Grupocodi = grupocodiDesp,
                    Equipadre = regDesp.Equipadre,
                };

                regReporte.DatoXGrupo = new DatoComparativoHOvsDespacho()
                {
                    Grupocodi = grupocodiDesp,
                    GrupoDesp = regDesp,
                };
                //setear valor
                listaRpt.Add(regReporte);
            }

            //if (valorHDesp > 0)
            regReporte.DatoXGrupo.ListaMWDespacho[h - 1] = (regReporte.DatoXGrupo.ListaMWDespacho[h - 1] ?? 0) + valorHDesp;
        }

        /// <summary>
        /// Actualizar datos de la Unidad de generación perteneciente al despacho
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorEms"></param>
        /// <param name="flagEmsOperativo"></param>
        /// <param name="existeConfEms"></param>
        /// <param name="grupocodiDesp"></param>
        /// <param name="regUnidad"></param>
        /// <param name="equicodi"></param>
        /// <param name="listaRpt"></param>
        private void AsignarDatosX30minAEqEms(int h, decimal? valorEms, int? flagEmsOperativo, bool existeConfEms
                                            , int grupocodiDesp, EqEquipoDTO regUnidad, int equicodi, ref List<ReporteComparativoHOvsDespacho> listaRpt)
        {
            var regReporte = listaRpt.Find(x => x.Grupocodi == grupocodiDesp); //el objeto ya fue creado antes 

            if (regReporte != null)
            {
                //si alguna equipo tiene ems entonces el grupo despacho tiene configuraion de ems
                if (existeConfEms)
                    regReporte.DatoXGrupo.TieneEqConfEms = true;

                //setear datos al equipo del grupo despacho
                var regDatoEspHo = regReporte.ListaDatoXEq.Find(x => x.Equicodi == equicodi);
                if (regDatoEspHo == null)
                {
                    regDatoEspHo = new DatoComparativoHOvsDespacho()
                    {
                        Grupocodi = grupocodiDesp,
                        Equicodi = equicodi,
                        EquipoEsp = regUnidad,
                        TieneEqConfEms = existeConfEms
                    };

                    regReporte.ListaDatoXEq.Add(regDatoEspHo);
                    regReporte.ListaDatoXEq = regReporte.ListaDatoXEq.OrderBy(x => x.EquipoEsp.Central).ThenBy(x => x.EquipoEsp.Equiabrev).ToList();
                }

                if (regUnidad.Equicodi == 98 && h == 46)
                { }

                #region Actualizar equipo

                //asignar datos del h al equipo
                if (valorEms > 0)
                    regDatoEspHo.ListaMWEms[h - 1] = (regDatoEspHo.ListaMWEms[h - 1] ?? 0) + valorEms;
                if (flagEmsOperativo >= 0)
                    regDatoEspHo.ListaFlagEms[h - 1] = flagEmsOperativo;

                //detalle para grafico web
                List<string> listaDesc = new List<string>();

                if (regDatoEspHo.TieneEqConfEms)
                {
                    if (regDatoEspHo.ListaFlagEms[h - 1] > 0)
                        listaDesc.Add("Estuvo operativo según el EMS.");
                    else
                        listaDesc.Add("NO OPERÓ según el EMS.");

                    if (regDatoEspHo.ListaMWEms[h - 1] > 0)
                        listaDesc.Add(string.Format("Generación: {0}MW", regDatoEspHo.ListaMWEms[h - 1]));
                }
                else
                {
                    listaDesc.Add("El generador no tiene configuración de EMS.");
                }
                regDatoEspHo.ListaDescripcionEMS[h - 1] = string.Join("\n", listaDesc);

                #endregion

                #region Actualizar grupo

                //sumar los equipos y colocarlos al grupo despacho
                decimal valorEmsXDesp = regReporte.ListaDatoXEq.Select(x => x.ListaMWEms[h - 1] ?? 0).Sum();
                //if (valorXDesp > 0)
                //if (regReporte.DatoXGrupo.TieneEqConfEms)
                regReporte.DatoXGrupo.ListaMWEms[h - 1] = valorEmsXDesp;

                decimal valorDesp = regReporte.DatoXGrupo.ListaMWDespacho[h - 1].GetValueOrDefault(0);

                regReporte.DatoXGrupo.ListaAlertaEms[h - 1] = 0;
                if (valorEmsXDesp > 0)//conectado
                {
                    //En caso la unidad se encuentre conectado en el EMS, pero tenga valores iguales a cero en el despacho ejecutado 
                    if (valorDesp == 0)
                    {
                        regReporte.DatoXGrupo.ListaAlertaEms[h - 1] = ConstantesCortoPlazo.AlertaEmsConectadoSinDesp;
                    }
                }
                else
                {
                    //desconectado

                    //En caso la unidad se encuentre desconectada en el EMS y tenga valores 0 en el despacho ejecutado 
                    if (valorDesp == 0)
                    {
                        regReporte.DatoXGrupo.ListaAlertaEms[h - 1] = ConstantesCortoPlazo.AlertaEmsDesconectadoSinDesp;
                    }

                    //En caso la unidad se encuentre desconectado en el EMS, pero tenga valores mayores a cero en el despacho ejecutado 
                    if (valorDesp > 0)
                    {
                        regReporte.DatoXGrupo.ListaAlertaEms[h - 1] = ConstantesCortoPlazo.AlertaEmsDesconectadoConDesp;
                    }
                }

                //descripcion
                List<string> listaDescGr = new List<string>();
                foreach (var reg in regReporte.ListaDatoXEq.OrderBy(x => x.EquipoEsp.Equiabrev).ToList())
                {
                    if (!string.IsNullOrEmpty(reg.ListaDescripcionEMS[h - 1]))
                    {
                        listaDescGr.Add(reg.EquipoEsp.Equiabrev + "\n" + reg.ListaDescripcionEMS[h - 1]);
                    }
                }
                regReporte.DatoXGrupo.ListaDescripcionEMS[h - 1] = string.Join("\n - \n", listaDescGr);

                #endregion

            }
            else
            {
            }
        }

        /// INSUMO: Despacho Ejecutado CDispatch
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> Listar48DespachoEjecutadoXGrupoCentral(DateTime fechaPeriodo, int tipoGen, string emprcodis, string equipadres
                                                                , List<PrGrupoDTO> listaGrupoDespacho, List<PrGrupoDTO> listaAllGrupo
                                                                , out List<string> listaVal)
        {
            List<MeMedicion48DTO> lista48xGrupo = servEjec.ListaDataMDGeneracionConsolidado48(fechaPeriodo, fechaPeriodo, ConstantesMedicion.IdTipogrupoCOES
                                                      , tipoGen.ToString(), emprcodis, ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString()
                                                      , false, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);

            foreach (var reg in lista48xGrupo)
            {
                reg.Equinomb = (reg.Gruponomb ?? "").Trim();
                reg.Gruponomb = (reg.Gruponomb ?? "").Trim();
            }

            SetearGrupoCentralAM48(lista48xGrupo, listaAllGrupo, out List<PrGrupoDTO> listaGrActivo, out listaVal);

            //incluir registros para grupos que no tienen data
            foreach (var reg in listaGrupoDespacho)
            {
                if (lista48xGrupo.Find(x => x.Grupocodi == reg.Grupocodi) == null)
                {
                    MeMedicion48DTO reg48 = new MeMedicion48DTO();
                    reg48.Emprcodi = reg.Emprcodi ?? 0;
                    reg48.Emprnomb = reg.Emprnomb;
                    reg48.Equipadre = reg.Equipadre;
                    reg48.Central = (reg.Central ?? "").Trim();
                    reg48.Equicodi = reg.Equicodi;
                    reg48.Equinomb = reg.Equinomb;
                    reg48.Grupocodi = reg.Grupocodi;
                    reg48.Gruponomb = reg.Gruponomb;
                    reg48.Medifecha = fechaPeriodo;

                    lista48xGrupo.Add(reg48);

                }
            }

            if (ConstantesAppServicio.ParametroDefecto != equipadres)
            {
                int[] listaequipadres = equipadres.Split(',').Select(x => int.Parse(x)).ToArray();
                lista48xGrupo = lista48xGrupo.Where(x => listaequipadres.Contains(x.Grupopadre)).ToList();
            }

            return lista48xGrupo;
        }

        /// <summary>
        /// INSUMO: Generación EMS
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="emprcodis"></param>
        /// <param name="equipadres"></param>
        /// <param name="listaEmsMW"></param>
        /// <param name="listaEmsOperativo"></param>
        private void ListarGeneracionEMS48(DateTime fechaIni, string emprcodis, string equipadres
                                                            , out List<MeMedicion48DTO> listaEmsMW, out List<MeMedicion48DTO> listaEmsOperativo)
        {
            string sParamEmpresa = emprcodis != "-1" ? emprcodis : ConstantesHorasOperacion.ParamEmpresaTodos;

            //data de costos marginales
            string famcodis = "2,3,4,5,36,37,38,39";
            List<CmGeneracionEmsDTO> listaEmsXDia = FactorySic.GetCmGeneracionEmsRepository().GetListaGeneracionByEquipoFecha(fechaIni, fechaIni, sParamEmpresa, famcodis);
            List<int> listaEquicodi = listaEmsXDia.Select(x => x.Equicodi).Distinct().ToList();

            //iterar por cada equipo y generar un objeto m48
            listaEmsMW = new List<MeMedicion48DTO>();
            listaEmsOperativo = new List<MeMedicion48DTO>();
            foreach (var equicodi in listaEquicodi)
            {
                var regEq = listaEmsXDia.Find(x => x.Equicodi == equicodi);

                //generar objetos
                MeMedicion48DTO regMW = new MeMedicion48DTO();
                regMW.Medifecha = fechaIni;
                regMW.Equicodi = regEq.Equicodi;
                regMW.Equinomb = regEq.Equinomb;
                regMW.Equiabrev = regEq.Equiabrev;
                regMW.Central = regEq.Central;
                regMW.Equipadre = regEq.Equipadre;
                regMW.Emprnomb = regEq.Emprnomb;

                MeMedicion48DTO regFlag = new MeMedicion48DTO();
                regFlag.Medifecha = fechaIni;
                regFlag.Equicodi = regEq.Equicodi;
                regFlag.Equinomb = regEq.Equinomb;
                regFlag.Equiabrev = regEq.Equiabrev;
                regFlag.Central = regEq.Central;
                regFlag.Equipadre = regEq.Equipadre;
                regFlag.Emprnomb = regEq.Emprnomb;

                //setear data
                List<CmGeneracionEmsDTO> listaEmsXDiaXEq = listaEmsXDia.Where(x => x.Equicodi == equicodi).OrderBy(x => x.Genemsfecha).ToList();
                var regPrimerEms = listaEmsXDiaXEq[0];

                for (int h = 1; h <= 48; h++)
                {
                    var ems30 = listaEmsXDiaXEq.Where(x => x.Genemsfecha == fechaIni.AddMinutes(h * 30)).FirstOrDefault();
                    var ems2359 = listaEmsXDiaXEq.Where(x => x.Genemsfecha == fechaIni.AddMinutes(h * 30).AddMinutes(-1)).FirstOrDefault();

                    //valor MW
                    decimal? valorH = null;
                    if (ems30 != null) valorH = ems30.Genemsgeneracion;
                    if (ems2359 != null) valorH = ems2359.Genemsgeneracion;

                    regMW.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(regMW, valorH);

                    //flag operativo
                    decimal? valorF = null;
                    if (ems30 != null) valorF = ems30.Genemsoperativo;
                    if (ems2359 != null) valorF = ems2359.Genemsoperativo;

                    regFlag.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(regFlag, valorF);
                }

                listaEmsMW.Add(regMW);
                listaEmsOperativo.Add(regFlag);
            }

            if (ConstantesAppServicio.ParametroDefecto != equipadres)
            {
                int[] listaequipadres = equipadres.Split(',').Select(x => int.Parse(x)).ToArray();
                listaEmsMW = listaEmsMW.Where(x => listaequipadres.Contains(x.Equipadre)).ToList();
                listaEmsOperativo = listaEmsOperativo.Where(x => listaequipadres.Contains(x.Equipadre)).ToList();
            }

        }

        /// <summary>
        /// INSUMO: Grupos de despacho para el día seleccionado
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="emprcodis"></param>
        /// <param name="grupopadres"></param>
        /// <param name="listaAllGrupo"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaAllEq"></param>
        private void ListarGrupoDespacho(DateTime fechaPeriodo, string emprcodis, string grupopadres, out List<PrGrupoDTO> listaAllGrupo, out List<PrGrupoDTO> listaGrupoDespacho, out List<EqEquipoDTO> listaAllEq)
        {
            //equipos
            listaAllEq = FactorySic.GetEqEquipoRepository().ListaEqEmpresaFamilia(-1, -1);
            List<EqEquipoDTO> listaEqCentral = listaAllEq.Where(x => (new List<int>() { 4, 5, 37, 39 }).Contains(x.Famcodi ?? 0)).ToList();
            List<EqEquipoDTO> listaEqGen = listaAllEq.Where(x => (new List<int>() { 3, 2, 36, 38 }).Contains(x.Famcodi ?? 0)).ToList();

            foreach (var regEq in listaEqCentral)
            {
                regEq.Equipadre = regEq.Equicodi;
                regEq.Central = regEq.Equinomb;
            }

            //asignar nombre de central
            foreach (var regEq in listaEqGen)
            {
                var regCentral = listaEqCentral.Find(x => x.Equicodi == regEq.Equipadre);
                if (regCentral != null)
                {
                    regEq.Central = regCentral.Equinomb;
                }
            }

            //Lista de grupos y Modos de operación con operación comercial
            ListarM48Filtro(fechaPeriodo, ConstantesAppServicio.ParametroDefecto, grupopadres, out List<MeMedicion48DTO> listaM48XDia, out listaAllGrupo, out List<string> listaVal);

            if (ConstantesAppServicio.ParametroDefecto != emprcodis)
            {
                int[] listaemprcodis = emprcodis.Split(',').Select(x => int.Parse(x)).ToArray();
                listaAllGrupo = listaAllGrupo.Where(x => listaemprcodis.Contains(x.Emprcodi ?? 0)).ToList();
            }

            if (ConstantesAppServicio.ParametroDefecto != grupopadres)
            {
                int[] listagrupopadres = grupopadres.Split(',').Select(x => int.Parse(x)).ToArray();
                listaAllGrupo = listaAllGrupo.Where(x => listagrupopadres.Contains(x.Grupopadre ?? 0)).ToList();
            }

            listaGrupoDespacho = listaAllGrupo.Where(x => x.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoTermico || x.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoHidraulico || x.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoSolar || x.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoEolico).ToList();

            //asignar central al grupo de despacho
            List<int> listaEquicodiXDesp = new List<int>();
            foreach (var regGrupo in listaGrupoDespacho)
            {
                if (regGrupo.Grupocodi == 151)
                { }
                regGrupo.ListaEquicodi = listaEqGen.Where(x => x.Grupocodi == regGrupo.Grupocodi).Select(x => x.Equicodi).Distinct().ToList();
                listaEquicodiXDesp.AddRange(listaAllEq.Where(x => x.Grupocodi == regGrupo.Grupocodi).Select(x => x.Equicodi).Distinct().ToList());

                var regCentral = listaEqCentral.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                if (regCentral != null)
                {
                    regGrupo.Equipadre = regCentral.Equicodi;
                    regGrupo.Central = regCentral.Equinomb;
                }
                else
                {
                    var regEq = listaEqGen.Find(x => x.Grupocodi == regGrupo.Grupocodi);
                    if (regEq != null)
                    {
                        regGrupo.Equipadre = regEq.Equipadre ?? 0;
                        regGrupo.Central = (listaEqCentral.Find(x => x.Equicodi == regEq.Equipadre)).Equinomb;
                    }
                    else
                    { }
                }
            }

            listaEquicodiXDesp = listaEquicodiXDesp.Distinct().ToList();
            List<int> lEquipadre = listaAllEq.Where(x => listaEquicodiXDesp.Contains(x.Equicodi)).Select(x => x.Equipadre ?? 0).Distinct().ToList();
            listaAllEq = listaAllEq.Where(x => lEquipadre.Contains(x.Equipadre ?? 0)).ToList();
        }

        /// <summary>
        /// Setear datos adicionales a la data de medicion48
        /// </summary>
        /// <param name="lista48xGrupo"></param>
        /// <param name="listaAllGrupo"></param>
        /// <param name="listaGrupoActivo"></param>
        /// <param name="listaVal"></param>
        private void SetearGrupoCentralAM48(List<MeMedicion48DTO> lista48xGrupo, List<PrGrupoDTO> listaAllGrupo, out List<PrGrupoDTO> listaGrupoActivo, out List<string> listaVal)
        {
            listaGrupoActivo = new List<PrGrupoDTO>();

            listaVal = new List<string>();
            foreach (var reg in lista48xGrupo)
            {
                if (reg.Grupocodi == 417)
                { }

                PrGrupoDTO regDesp = listaAllGrupo.Find(x => x.Grupocodi == reg.Grupocodi);
                if (regDesp != null)
                {
                    if (regDesp.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoTermico || regDesp.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoHidraulico || regDesp.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoSolar || regDesp.Catecodi == (int)ConstantesMigraciones.Catecodi.GrupoEolico)
                    {
                        PrGrupoDTO regCentral = listaAllGrupo.Find(x => x.Grupocodi == regDesp.Grupopadre);
                        if (regCentral != null)
                        {
                            reg.Grupopadre = regCentral.Grupocodi;
                            reg.Grupocentral = regCentral.Gruponomb;

                            //agregar grupo central
                            regCentral.Emprcodi = reg.Emprcodi;
                            listaGrupoActivo.Add(regCentral);
                        }
                    }
                    else
                    {
                        reg.Grupopadre = regDesp.Grupocodi;
                        reg.Grupocentral = regDesp.Gruponomb;
                        listaVal.Add(string.Format("{0} {1} no es un grupo de despacho.", regDesp.Grupocodi, regDesp.Gruponomb));
                    }

                    //agregar grupo que esta en m48 para ese día
                    regDesp.Emprcodi = reg.Emprcodi;
                    listaGrupoActivo.Add(regDesp);
                }
                else
                {
                    listaVal.Add(string.Format("No existe el grupo {0}.", reg.Grupocodi));
                }
            }
        }

        #endregion

        #region Excel

        /// <summary>
        /// Generar Reporte Excel
        /// </summary>
        public void GenerarExcelDiferenciaEMSvsDespacho(string ruta, DateTime fechaIni, DateTime fechaFin, out string nameFile)
        {
            CalcularDiferenciaEMSvsDespacho(fechaIni, fechaFin, out List<ReporteComparativoHOvsDespacho> lFinal, out List<PrGrupoDTO> listaGrupoDespacho);

            //Nombre de archivo
            nameFile = string.Format("DiferenciasEMSvsEjecutado_{0}_{1}.xlsx", fechaIni.ToString(ConstantesAppServicio.FormatoFechaDMY), fechaFin.ToString(ConstantesAppServicio.FormatoFechaDMY));
            string rutaFile = ruta + nameFile;
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelDetalleEMSvsDespacho(xlPackage, "Generación EMS", 1, fechaIni, fechaFin, listaGrupoDespacho, lFinal);
                xlPackage.Save();

                GenerarHojaExcelDetalleEMSvsDespacho(xlPackage, "Despacho Ejecutado", 2, fechaIni, fechaFin, listaGrupoDespacho, lFinal);
                xlPackage.Save();

                GenerarHojaExcelDetalleEMSvsDespacho(xlPackage, "Comparativo", 3, fechaIni, fechaFin, listaGrupoDespacho, lFinal);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Generar detalle por modo de operación
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="obj"></param>
        /// <param name="fechaConsulta"></param>
        private void GenerarHojaExcelDetalleEMSvsDespacho(ExcelPackage xlPackage, string nameWS, int tipoReporte, DateTime fechaIni, DateTime fechaFin
                                                , List<PrGrupoDTO> listaGrupoDespacho, List<ReporteComparativoHOvsDespacho> lFinal)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string font = "Calibri";
            string colorCeldaCs = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera
            //
            int colHora = 1;
            int colIniData = colHora + 1;
            int colFinData = colIniData + 1;
            if (listaGrupoDespacho.Any()) colFinData = colHora + listaGrupoDespacho.Count();
            int rowEmp = 1;
            int rowGrupo = rowEmp + 1;

            ws.Cells[rowEmp, colHora].Value = "HORA";
            UtilExcel.CeldasExcelAgrupar(ws, rowEmp, colHora, rowGrupo, colHora);

            UtilExcel.SetFormatoCelda(ws, rowEmp, colHora, rowGrupo, colFinData, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEmp, colHora, rowEmp, colFinData, colorLinea, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowGrupo, colHora, rowGrupo, colFinData, colorLinea, true);

            int rowIniData = rowGrupo + 1;
            int rowData = rowIniData;

            //Columna hora de todos los días
            ws.Column(colHora).Width = 17;
            for (DateTime day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                for (int h = 1; h <= 48; h++)
                {
                    int addSec = (h == 48) ? -1 : 0;
                    ws.Cells[rowData, colHora].Value = day.AddMinutes(h * 30).AddSeconds(addSec).ToString(ConstantesAppServicio.FormatoFechaFull);

                    rowData++;
                }
            }

            //formatear lineas
            int numDia = (int)(fechaFin - fechaIni).TotalDays + 1;
            int numRowsXCuadro = 8;
            int rowFinData = rowIniData + numDia * 48 - 1;
            for (int c = colHora; c <= colFinData; c++)
            {
                for (int f = rowIniData; f <= rowFinData; f += numRowsXCuadro)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f + numRowsXCuadro - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + numRowsXCuadro - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                }
            }

            int numDecimalMw = 2;
            UtilExcel.SetFormatoCelda(ws, rowIniData, colIniData, rowFinData, colFinData, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
            UtilExcel.CeldasExcelFormatoNumero(ws, rowIniData, colIniData, rowFinData, colFinData, numDecimalMw);

            //setear data
            int colData = colIniData;
            foreach (var regGrupo in listaGrupoDespacho)
            {
                //cabecera
                ws.Cells[rowEmp, colData].Value = regGrupo.Emprnomb;
                ws.Cells[rowGrupo, colData].Value = regGrupo.Gruponomb;

                //data
                var listaRptXGrupo = lFinal.Where(x => x.Grupocodi == regGrupo.Grupocodi).ToList();

                rowData = rowIniData;
                for (DateTime day = fechaIni; day <= fechaFin; day = day.AddDays(1))
                {
                    var regRptXGrupo = listaRptXGrupo.Find(x => x.Fecha == day);
                    if (regRptXGrupo != null)
                    {
                        decimal?[] listaMW = null;
                        if (tipoReporte == 1) listaMW = regRptXGrupo.DatoXGrupo.ListaMWEms;
                        if (tipoReporte == 2) listaMW = regRptXGrupo.DatoXGrupo.ListaMWDespacho;
                        if (tipoReporte == 3) listaMW = regRptXGrupo.DatoXGrupo.ListaDiferencia;
                        int[] listaAlertaEms = regRptXGrupo.DatoXGrupo.ListaAlertaEms;

                        for (int h = 0; h < 48; h++)
                        {
                            if (tipoReporte != 3)
                            {
                                if (listaMW[h] > 0)
                                    ws.Cells[rowData, colData].Value = listaMW[h];
                                if (regRptXGrupo.DatoXGrupo.ListaAlerta[h])
                                    UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#000000", "#FFB4B4", font, 12, false);
                            }
                            else
                            {
                                //si tiene alerta ems
                                if (listaAlertaEms[h] > 0)
                                {
                                    switch (listaAlertaEms[h])
                                    {
                                        case ConstantesCortoPlazo.AlertaEmsDesconectadoSinDesp:
                                            ws.Cells[rowData, colData].Value = "DESCONECTADO";
                                            UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#FFFFFF", "#0000FF", font, 12, false);
                                            break;
                                        case ConstantesCortoPlazo.AlertaEmsDesconectadoConDesp:
                                            ws.Cells[rowData, colData].Value = "CONECTAR";
                                            UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#000000", "#C6E0B4", font, 12, false);
                                            break;
                                        case ConstantesCortoPlazo.AlertaEmsConectadoSinDesp:
                                            ws.Cells[rowData, colData].Value = "REVISAR";
                                            UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#000000", "#0DC043", font, 12, false);
                                            break;
                                    }
                                }
                                else
                                {
                                    //si no tiene alerta, mostrar la diferencia
                                    if (listaMW[h] > 0)
                                        ws.Cells[rowData, colData].Value = listaMW[h];
                                    if (regRptXGrupo.DatoXGrupo.ListaAlerta[h])
                                        UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#000000", "#FFB4B4", font, 12, false);
                                }
                            }

                            rowData++;
                        }
                    }
                    else
                    {
                        if (tipoReporte == 3)
                        {
                            for (int h = 0; h < 48; h++)
                            {
                                ws.Cells[rowData, colData].Value = "DESCONECTADO";
                                UtilExcel.SetFormatoCelda(ws, rowData, colData, rowData, colData, "Centro", "Centro", "#FFFFFF", "#0000FF", font, 12, false);
                                rowData++;
                            }
                        }
                        else
                        {
                            rowData += 48;
                        }
                    }
                }

                ws.Column(colData).Width = 18;
                colData++;
            }

            #endregion

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            ws.View.FreezePanes(rowGrupo + 1, colHora + 1);

            ws.View.ZoomScale = 80;

            //excel con Font
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        #endregion

        #endregion

        #region Comparar Demanda EMS vs Demanda Ejecutada

        public List<CmConfigbarraDTO> ObtenerBarrasEMS()
        {
            return FactorySic.GetCmConfigbarraRepository().GetByCriteria(ConstantesAppServicio.Activo, ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// Permite obtener el comparativo de datos de demanda
        /// </summary>
        /// <param name="cnfbarcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public ComparativoResultado ObtenerComparativoDemanda(int cnfbarcodi, DateTime fecha,
            bool flagExportar, string path, string file)
        {
            ComparativoResultado model = new ComparativoResultado();

            try
            {
                CmConfigbarraDTO barraDto = FactorySic.GetCmConfigbarraRepository().GetById(cnfbarcodi);
                CmUmbralComparacionDTO umbralDTO = FactorySic.GetCmUmbralComparacionRepository().
                    GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);
                model.Descripcion = barraDto.Cnfbarnombre;
                model.Fecha = fecha.ToString(ConstantesAppServicio.FormatoFechaDMY);

                decimal umbral = 0;
                if (umbralDTO != null) umbral = (umbralDTO.Cmuncodemanda != null) ? (decimal)umbralDTO.Cmuncodemanda : 0;


                List<CmCostomarginalDTO> demandaEMS = this.ObtenerCostosMarginalesComparativo(
                    cnfbarcodi, fecha, fecha);

                MeScadaSp7DTO demandaEjecutada = FactoryScada.GetMeScadaSp7Repository().ObtenerComparativoDemanda(
                    cnfbarcodi, fecha, fecha).FirstOrDefault();

                model.ListaDatos = new List<ComparativoItemResult>();

                if (demandaEMS.Count > 0 && demandaEjecutada != null)
                {
                    if (demandaEMS.Count == 48)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            ComparativoItemResult item = new ComparativoItemResult();

                            item.Hora = fecha.AddMinutes((i) * 30).ToString(ConstantesAppServicio.FormatoHora);

                            item.DemandaEMS = demandaEMS[i - 1].Cmgndemanda;
                            item.DemandaEjecutada = Convert.ToDecimal(demandaEjecutada.GetType().
                                GetProperty(ConstantesAppServicio.CaracterH + i * 2).GetValue(demandaEjecutada, null));

                            if (item.DemandaEMS != null && item.DemandaEjecutada != null)
                            {
                                item.Desviacion = (item.DemandaEjecutada != 0) ? Math.Abs((decimal)item.DemandaEjecutada - (decimal)item.DemandaEMS) / item.DemandaEjecutada : 0;
                                item.Diferencia = Math.Abs((decimal)item.DemandaEjecutada - (decimal)item.DemandaEMS);
                                if (umbral > 0 && item.Diferencia > umbral) item.Indicador = ConstantesAppServicio.SI;
                            }

                            model.ListaDatos.Add(item);
                        }
                        model.Resultado = 1;

                        if (flagExportar)
                        {
                            this.ExportarComparativoDemanda(model.ListaDatos, path, file, fecha.ToString(ConstantesAppServicio.FormatoFecha),
                                model.Descripcion);
                        }
                    }
                    else
                    {
                        model.Resultado = 3;
                    }
                }
                else
                {
                    model.Resultado = 4;
                    if (demandaEMS.Count > 0 && demandaEjecutada == null)
                    {
                        model.Resultado = 5;
                    }
                }

            }
            catch (Exception)
            {
                model.Resultado = -1;
            }

            return model;
        }

        /// <summary>
        /// Permite generar el reporte masivo de comparativo demanda ems vs demanda ejecutada
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public object GenerarMasivoComparativoDemanda(DateTime fechaDesde, DateTime fechaHasta, string path, string file)
        {
            try
            {
                List<CmCostomarginalDTO> listDemandaEMS = this.ObtenerCostosMarginalesComparativo(
                            -1, fechaDesde, fechaHasta);

                List<int> idsBarras = listDemandaEMS.Select(x => x.Cnfbarcodi).Distinct().ToList();

                List<CmConfigbarraDTO> barras = this.ObtenerBarrasEMS().Where(x => idsBarras.Any(y => y == x.Cnfbarcodi)).OrderBy(x => x.Cnfbarnombre).ToList();

                List<MeScadaSp7DTO> listDemandaEjecutada = FactoryScada.GetMeScadaSp7Repository().ObtenerComparativoDemanda(
                            -1, fechaDesde, fechaHasta);

                int dias = (int)fechaHasta.Subtract(fechaDesde).TotalDays;

                List<ComparativoItemResult> result = new List<ComparativoItemResult>();

                List<CmConfigbarraDTO> headers = new List<CmConfigbarraDTO>();

                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = fechaDesde.AddDays(i);

                    foreach (CmConfigbarraDTO barra in barras)
                    {
                        if (i == 0) headers.Add(barra);

                        List<CmCostomarginalDTO> demandaEMS = listDemandaEMS.Where(x => x.Cnfbarcodi == barra.Cnfbarcodi &&
                        fecha.Year == x.Cmgnfecha.Year && fecha.Month == x.Cmgnfecha.Month && fecha.Day == x.Cmgnfecha.Day).ToList();

                        if (demandaEMS.Count == 48)
                        {
                            MeScadaSp7DTO demandaEjecutada = listDemandaEjecutada.Where(x => x.Canalcodi == barra.Cnfbarcodi &&
                                x.Medifecha == fecha).FirstOrDefault();

                            if (demandaEjecutada != null)
                            {
                                for (int j = 1; j <= 48; j++)
                                {
                                    ComparativoItemResult item = new ComparativoItemResult();
                                    item.Hora = fecha.AddMinutes((j) * 30).ToString(ConstantesAppServicio.FormatoFechaFull);
                                    item.DemandaEMS = (decimal)demandaEMS[j - 1].Cmgndemanda;
                                    item.DemandaEjecutada = Convert.ToDecimal(demandaEjecutada.GetType().
                                        GetProperty(ConstantesAppServicio.CaracterH + j * 2).GetValue(demandaEjecutada, null));
                                    if (item.DemandaEMS != null && item.DemandaEjecutada != null)
                                        item.Diferencia = Math.Abs((decimal)item.DemandaEjecutada - (decimal)item.DemandaEMS);

                                    item.Barra = barra.Cnfbarcodi;
                                    result.Add(item);
                                }
                            }
                            else
                            {
                                for (int j = 1; j <= 48; j++)
                                {
                                    ComparativoItemResult item = new ComparativoItemResult();
                                    item.Hora = fecha.AddMinutes((j) * 30).ToString(ConstantesAppServicio.FormatoFechaFull);
                                    item.DemandaEMS = 0;
                                    item.DemandaEjecutada = 0;
                                    item.Diferencia = 0;
                                    item.Barra = barra.Cnfbarcodi;
                                    result.Add(item);
                                }
                            }
                        }
                        else
                        {
                            for (int j = 1; j <= 48; j++)
                            {
                                ComparativoItemResult item = new ComparativoItemResult();
                                item.Hora = fecha.AddMinutes((j) * 30).ToString(ConstantesAppServicio.FormatoFechaFull);
                                item.DemandaEMS = 0;
                                item.DemandaEjecutada = 0;
                                item.Diferencia = 0;
                                item.Barra = barra.Cnfbarcodi;
                                result.Add(item);
                            }
                        }
                    }
                }

                this.ExportarMasivoComparativoDemanda(headers, result, path, file);

                return new
                {
                    result = 1,
                    fecha = fechaDesde.ToString(ConstantesAppServicio.FormatoFechaDMY) + "_" +
                    fechaHasta.ToString(ConstantesAppServicio.FormatoFechaDMY)
                };
            }
            catch (Exception)
            {
                return new { result = -1, fecha = string.Empty };
            }
        }

        /// <summary>
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void ExportarComparativoDemanda(List<ComparativoItemResult> list, string path, string filename,
            string fecha, string barra)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COMPARATIVO");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "Comparativo Demanda EMS vs Demanda Ejecutada";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;

                        ws.Cells[index, 2].Value = "HORA";
                        ws.Cells[index, 3].Value = "DEMANDA EMS (MW)";
                        ws.Cells[index, 4].Value = "DEMANDA EJECUTADA (MW)";
                        ws.Cells[index, 5].Value = "DIFERENCIA (MW)";
                        ws.Cells[index, 6].Value = "DESVIACIÓN (%)";

                        rg = ws.Cells[index, 2, index, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 6;
                        foreach (ComparativoItemResult item in list)
                        {
                            ws.Cells[index, 2].Value = item.Hora;
                            ws.Cells[index, 3].Value = item.DemandaEMS;
                            ws.Cells[index, 4].Value = item.DemandaEjecutada;
                            ws.Cells[index, 5].Value = item.Diferencia;
                            ws.Cells[index, 6].Value = item.Desviacion * 100;

                            rg = ws.Cells[index, 2, index, 6];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            if (item.Indicador == ConstantesAppServicio.SI)
                            {
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffb4b4"));
                            }

                            rg = ws.Cells[index, 6, index, 6];
                            rg.Style.Numberformat.Format = "#0\\.00%";

                            index++;
                        }

                        rg = ws.Cells[5, 2, index - 1, 6];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        var lineChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.LineMarkers) as ExcelLineChart;
                        lineChart.SetPosition(5, 0, 7, 0);
                        lineChart.SetSize(650, 400);
                        lineChart.Series.Add(ExcelRange.GetAddress(6, 3, index - 1, 3), ExcelRange.GetAddress(6, 2, index - 1, 2));
                        lineChart.Series.Add(ExcelRange.GetAddress(6, 4, index - 1, 4), ExcelRange.GetAddress(6, 2, index - 1, 2));
                        lineChart.Series[0].Header = "Demanda EMS";
                        lineChart.Series[1].Header = "Demanda Ejecutado";

                        lineChart.Title.Text = "Comparativo Demanda EMS vs Demanda Ejecutada - " + barra;
                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 6];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
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
        /// Permite generar el reporte excel
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="result"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void ExportarMasivoComparativoDemanda(List<CmConfigbarraDTO> headers, List<ComparativoItemResult> result,
            string path, string filename)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    this.GenerarHojaComparativoDemanda(xlPackage, "Demanda EMS", headers, result, 1);
                    this.GenerarHojaComparativoDemanda(xlPackage, "Demanda Ejecutada", headers, result, 2);
                    this.GenerarHojaComparativoDemanda(xlPackage, "Comparativo", headers, result, 3);

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generar hojas del comparativo de demanda
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="hoja"></param>
        /// <param name="headers"></param>
        /// <param name="result"></param>
        /// <param name="indicador"></param>
        public void GenerarHojaComparativoDemanda(ExcelPackage xlPackage, string hoja,
            List<CmConfigbarraDTO> headers, List<ComparativoItemResult> result, int indicador)
        {

            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(hoja);

            if (ws != null)
            {
                int index = 1;
                int column = 1;

                ws.Cells[index, column].Value = "FECHA / HORA";
                foreach (CmConfigbarraDTO header in headers)
                {
                    column++;
                    ws.Cells[index, column].Value = header.Cnfbarnombre;
                }

                ExcelRange rg = ws.Cells[index, 1, index, column];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                bool flag = true;
                column = 2;
                index = 2;
                int indexFinal = 0;
                foreach (CmConfigbarraDTO header in headers)
                {
                    List<ComparativoItemResult> subList = result.Where(x => x.Barra == header.Cnfbarcodi).ToList();

                    for (int k = 0; k < subList.Count; k++)
                    {
                        if (flag)
                            ws.Cells[index + k, column - 1].Value = subList[k].Hora;

                        if (indicador == 1)
                            ws.Cells[index + k, column].Value = subList[k].DemandaEMS;
                        else if (indicador == 2)
                            ws.Cells[index + k, column].Value = subList[k].DemandaEjecutada;
                        else if (indicador == 3)
                            ws.Cells[index + k, column].Value = subList[k].Diferencia;
                    }
                    column++;
                    indexFinal = index + subList.Count;
                    flag = false;
                }

                rg = ws.Cells[index, 2, indexFinal, column];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));


                rg = ws.Cells[index, 1, indexFinal - 1, 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D6E9F0"));
                rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                rg = ws.Cells[index - 1, 1, indexFinal - 1, column - 1];
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                rg = ws.Cells[index, 2, indexFinal - 1, column - 1];
                rg.Style.Numberformat.Format = "#,##0.00";
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                rg = ws.Cells[index - 1, 1, index - 1, column];
                rg.AutoFitColumns();
                ws.Column(1).Width = 20;
            }
        }

        #endregion

        #region Comparar CM Aplicativo vs CM Programado vs CI Tiempo Real

        /// <summary>
        /// Analiza los datos por cada barra y por cada dia
        /// </summary>
        /// <param name="cnfbarcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<CmCostomarginalDTO> ObtenerCostosMarginalesComparativo(int cnfbarcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmCostomarginalDTO> result = new List<CmCostomarginalDTO>();
            List<CmCostomarginalDTO> data = FactorySic.GetCmCostomarginalRepository().ObtenerComparativoCM(
                        cnfbarcodi, fechaInicio, fechaFin);

            List<int> horas = new List<int>();
            for (int i = 1; i <= 48; i++) horas.Add(i);

            if (cnfbarcodi != -1)
            {
                List<int> faltantes = horas.Where(x => !data.Any(y => y.Cmgncorrelativo == x)).ToList();
                result.AddRange(data);

                foreach (int faltante in faltantes)
                {
                    result.Add(new CmCostomarginalDTO { Cmgncorrelativo = faltante });
                }

                result = result.OrderBy(x => x.Cmgncorrelativo).ToList();
            }
            else
            {
                int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;
                List<int> barras = data.Select(x => x.Cnfbarcodi).Distinct().ToList();

                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = fechaInicio.AddDays(i);

                    foreach (int barracodi in barras)
                    {
                        List<CmCostomarginalDTO> registros = data.Where(x => x.Cnfbarcodi == barracodi &&
                            fecha.Year == x.Cmgnfecha.Year && fecha.Month == x.Cmgnfecha.Month && fecha.Day == x.Cmgnfecha.Day).ToList();

                        result.AddRange(registros);

                        List<int> faltantes = horas.Where(x => !registros.Any(y => y.Cmgncorrelativo == x)).ToList();

                        foreach (int faltante in faltantes)
                        {
                            result.Add(new CmCostomarginalDTO { Cmgncorrelativo = faltante });
                        }
                    }
                }

            }

            return result;
        }


        /// <summary>
        /// Permite obtener el comparativo de datos de demanda
        /// </summary>
        /// <param name="cnfbarcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public ComparativoResultado ObtenerComparativoCostosMarginales(int cnfbarcodi, DateTime fecha,
            bool flagExportar, string path, string file)
        {
            ComparativoResultado model = new ComparativoResultado();

            try
            {
                CmConfigbarraDTO barraDto = FactorySic.GetCmConfigbarraRepository().GetById(cnfbarcodi);
                model.Descripcion = barraDto.Cnfbarnombre;
                model.Fecha = fecha.ToString(ConstantesAppServicio.FormatoFechaDMY);
                decimal tipoCambio = this.ObtenerTipoDeCambio(fecha);

                List<CmCostomarginalDTO> costoMarginalEMS = this.ObtenerCostosMarginalesComparativo(
                    cnfbarcodi, fecha, fecha);

                CpMedicion48DTO costoMarginalProgramado = this.ObtenerCostoMarginalProgamado(fecha, barraDto);

                List<decimal?> listCI = this.ObtenerCostoIncremental(fecha);

                model.ListaDatos = new List<ComparativoItemResult>();

                if (costoMarginalEMS.Count > 0)
                {
                    if (costoMarginalEMS.Count == 48)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            ComparativoItemResult item = new ComparativoItemResult();

                            item.Hora = fecha.AddMinutes((i) * 30).ToString(ConstantesAppServicio.FormatoHora);
                            item.CmAplicativo = costoMarginalEMS[i - 1].Cmgntotal;

                            if (costoMarginalProgramado != null)
                            {
                                item.CmProgramado = Convert.ToDecimal(costoMarginalProgramado.GetType().
                                    GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(costoMarginalProgramado, null));

                                if (item.CmProgramado != null) item.CmProgramado = item.CmProgramado * tipoCambio;

                                if (item.CmAplicativo != null)
                                    item.DiferenciaAB = Math.Abs((decimal)item.CmAplicativo - (decimal)item.CmProgramado);
                            }

                            if (listCI[i - 1] != null)
                            {
                                item.CostoIncremental = listCI[i - 1];

                                if (item.CmAplicativo != null)
                                    item.DiferenciaAC = Math.Abs((decimal)item.CmAplicativo - (decimal)item.CostoIncremental);
                            }

                            model.ListaDatos.Add(item);
                        }
                        model.Resultado = 1;

                        if (barraDto.Recurcodi == null)
                        {
                            model.Resultado = 5;
                        }

                        if (flagExportar)
                        {
                            this.ExportarComparativoCostosMarginales(model.ListaDatos, path, file, fecha.ToString(ConstantesAppServicio.FormatoFecha),
                                model.Descripcion);
                        }
                    }
                    else
                    {
                        model.Resultado = 3;
                    }
                }
                else
                {
                    model.Resultado = 4;
                }

            }
            catch (Exception)
            {
                model.Resultado = -1;
            }

            return model;
        }

        /// <summary>
        /// Permite obtener el costo marginal programado por barra
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="recurcodi"></param>
        /// <returns></returns>
        public CpMedicion48DTO ObtenerCostoMarginalProgamado(DateTime fecha, CmConfigbarraDTO barraDTO)
        {
            if (barraDTO.Recurcodi != null)
            {
                List<CoMedicion48DTO> topologias = FactorySic.GetCoMedicion48Repository().ObtenerTopologias(fecha, fecha);

                if (topologias.Where(x => x.Topfinal == 1).Count() == 1)
                {
                    CoMedicion48DTO topologia = topologias.Where(x => x.Topfinal == 1).First();
                    List<CoMedicion48DTO> reprogramas = topologias.Where(x => x.Topcodi1 == topologia.Topcodi && x.Topfinal != 1).
                        OrderByDescending(y => y.Topcodi).ToList();

                    //foreach (CoMedicion48DTO itemReprog in reprogramas)
                    //{
                    //    if (itemReprog.Topiniciohora > 1) itemReprog.Topiniciohora = itemReprog.Topiniciohora + 1;
                    //}

                    string propiedadCM = ConstantesCortoPlazo.PropiedadCostoMarginal;

                    CpMedicion48DTO costomarginal = this.ObtenerDatosYupana(topologia, reprogramas, propiedadCM, (int)barraDTO.Recurcodi);

                    return costomarginal;
                }
            }

            return null;
        }

        /// <summary>
        /// Permite obtener los datos desde YUPANA
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reprogramas"></param>
        /// <param name="propiedad"></param>
        /// <returns></returns>
        public CpMedicion48DTO ObtenerDatosYupana(CoMedicion48DTO item, List<CoMedicion48DTO> reprogramas, string propiedad,
            int recurcodi)
        {
            CpMedicion48DTO resultado = new CpMedicion48DTO();
            List<CoMedicion48DTO> entitys = new List<CoMedicion48DTO>();
            bool flag = false;
            foreach (CoMedicion48DTO subList in reprogramas)
            {
                if (subList.Topiniciohora > 0)
                {
                    entitys.Add(new CoMedicion48DTO { Topcodi = subList.Topcodi, Topiniciohora = subList.Topiniciohora });
                    if (subList.Topiniciohora == 1)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                entitys.Add(new CoMedicion48DTO { Topcodi = item.Topcodi, Topiniciohora = 1 });
            }

            List<int> listTopcodi = entitys.Select(x => x.Topcodi).ToList();

            List<CpMedicion48DTO> listDatos = FactorySic.GetCpMedicion48Repository().ObtenerProgramaPorRecurso(
                string.Join<int>(",", listTopcodi), recurcodi, propiedad);

            List<CoMedicion48DTO> orden = entitys.OrderBy(x => x.Topiniciohora).ToList();

            foreach (CoMedicion48DTO itemOrden in orden)
            {
                CpMedicion48DTO reemplazo = listDatos.Where(x => x.Topcodi == itemOrden.Topcodi).FirstOrDefault();

                if (reemplazo != null)
                {
                    for (int i = itemOrden.Topiniciohora; i <= 48; i++)
                    {
                        resultado.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(resultado,
                            reemplazo.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reemplazo));
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el tipo de cambio
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal ObtenerTipoDeCambio(DateTime fecha)
        {
            List<PrGrupodatDTO> formulasGeneral = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

            n_parameter parameterGeneral = new n_parameter();
            foreach (PrGrupodatDTO itemConcepto in formulasGeneral)
            {
                if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                    parameterGeneral.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
            }

            double tipoCambio = parameterGeneral.GetEvaluate(ConstantesCortoPlazo.PropTipoCambio);

            return (decimal)tipoCambio;
        }

        /// <summary>
        /// Permite obtemer los costos incrementales
        /// </summary>
        /// <param name="fecha"></param>
        public List<decimal?> ObtenerCostoIncremental(DateTime fecha)
        {
            //- Listado de costos incrementales
            List<decimal?> result = new List<decimal?>();

            //- Listado de horas de operacion por PyE y sin LT
            List<EveHoraoperacionDTO> horasOperacion = FactorySic.GetEveHoraoperacionRepository().ObtenerHorasOperacionCompartivoCM(fecha);

            //- Obtener los costos incrementales
            List<ReporteCostoIncrementalDTO> listCI = this.CalcularCostoIncremental(horasOperacion, fecha);

            //- Listado de unidades con ciclos combinados
            List<EqRelacionDTO> relacion = FactorySic.GetEqRelacionRepository().ObtenerUnidadComparativoCM();

            //- Obtenemos la generación ems para determinación del rango de CI
            List<CmGeneracionEmsDTO> generacion = this.ObtenerGeneracionComparativoCM(listCI, relacion, fecha);

            for (int i = 1; i <= 48; i++)
            {
                decimal? ci = null;
                decimal maxValue = decimal.MinValue;
                bool flag = false;
                DateTime hora = fecha.AddMinutes(i * 30);
                List<int> subListHO = horasOperacion.Where(x =>
                hora.Subtract((DateTime)x.Hophorini).TotalSeconds >= 0 && ((DateTime)x.Hophorfin).Subtract(hora).TotalSeconds >= 0).
                Select(x => (int)x.Grupocodi).Distinct().ToList();

                foreach (int grupocodi in subListHO)
                {
                    //- Analizamos la curva de consumo
                    ReporteCostoIncrementalDTO ciitem = listCI.Where(x => x.Grupocodi == grupocodi).FirstOrDefault();

                    if (ciitem != null)
                    {
                        if (ciitem.Cincrem2 != 0 || ciitem.Cincrem3 != 0)
                        {
                            decimal cim = this.ObtenerCIMultiplesTramos(i, ciitem, relacion, subListHO,
                                grupocodi, generacion);

                            if (cim > maxValue)
                            {
                                maxValue = cim;
                                flag = true;
                            }
                        }
                        else
                        {
                            if ((decimal)ciitem.Cincrem1 > maxValue)
                            {
                                maxValue = (decimal)ciitem.Cincrem1;
                                flag = true;
                            }
                        }
                    }
                }

                if (flag) ci = maxValue;
                result.Add(ci);
            }

            return result;
        }

        /// <summary>
        /// Permite obtener la generacion de las unidades ems
        /// </summary>
        /// <param name="listCI"></param>
        /// <param name="relacion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CmGeneracionEmsDTO> ObtenerGeneracionComparativoCM(List<ReporteCostoIncrementalDTO> listCI, List<EqRelacionDTO> relacion,
            DateTime fecha)
        {
            List<int> equipos = new List<int>();

            //- Obtener los equipos que tienen modo de operacion con tramos múltiples
            List<int> modosMultitramos = listCI.Where(x => x.Cincrem2 > 0 || x.Cincrem3 > 0).Select(x => x.Grupocodi).ToList();

            foreach (int modo in modosMultitramos)
            {
                EqRelacionDTO itemRelacion = relacion.Where(x => x.Grupocodi == modo).FirstOrDefault();

                if (itemRelacion != null)
                {
                    if (itemRelacion.Ccombcodi != null)
                    {
                        equipos.AddRange(relacion.Where(x => x.Ccombcodi == itemRelacion.Ccombcodi).Select(x => (int)x.Equicodi).Distinct());
                    }
                }
            }

            if (equipos.Count > 0)
            {
                string codigos = string.Join<int>(",", equipos);
                return FactorySic.GetCmGeneracionEmsRepository().ObtenerGeneracionCostoIncremental(codigos, fecha);
            }
            else
            {
                return new List<CmGeneracionEmsDTO>();
            }
        }


        /// <summary>
        /// Permite determinar el costo incremental en caso de múltiples tramos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="ci"></param>
        /// <param name="relacion"></param>
        /// <param name="modos"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public decimal ObtenerCIMultiplesTramos(int hora, ReporteCostoIncrementalDTO ci,
            List<EqRelacionDTO> relacion, List<int> modos, int grupocodi, List<CmGeneracionEmsDTO> generacion)
        {
            decimal result = 0;
            EqRelacionDTO item = relacion.Where(x => x.Grupocodi == grupocodi).FirstOrDefault();

            if (item != null)
            {
                if (item.Ccombcodi != null)
                {
                    List<EqRelacionDTO> subResult = relacion.Where(x => x.Ccombcodi == item.Ccombcodi).ToList();
                    List<EqRelacionDTO> subList = new List<EqRelacionDTO>();

                    foreach (EqRelacionDTO subItem in subResult)
                    {
                        if (subList.Where(x => x.Equicodi == subItem.Equicodi).Count() == 0)
                            subList.Add(subItem);
                    }

                    foreach (EqRelacionDTO equipo in subList)
                    {
                        CmGeneracionEmsDTO gen = generacion.Where(x => x.Equicodi == equipo.Equicodi && x.Cmgncorrelativo == hora).FirstOrDefault();

                        if (gen != null)
                        {
                            equipo.IndOperacion = gen.Genemsoperativo.ToString();
                            equipo.PotGenerada = (double)gen.Genemsgeneracion;
                        }

                        //- Determinaos si el modo de operación de la unidad se encuentra en el listado
                        List<int> modosOperacion = relacion.Where(x => x.Equicodi == equipo.Equicodi).Select(x => x.Grupocodi).Distinct().ToList();

                        if (modos.Where(x => modosOperacion.Any(y => x == y)).Count() != 0)
                        {
                            equipo.IndModoOperacion = ConstantesAppServicio.SI;
                        }
                    }

                    //- Analizamos los datos de los ciclos combinados
                    EqRelacionDTO tvcc = subList.Where(x => x.Indtvcc == ConstantesAppServicio.SI).FirstOrDefault();
                    List<EqRelacionDTO> tgcc = subList.Where(x => x.Indtvcc != ConstantesAppServicio.SI).ToList();
                    bool flagTV = false;
                    if (tvcc.IndOperacion == 1.ToString()) flagTV = true;
                    decimal potencia = (decimal)tvcc.PotGenerada;

                    foreach (EqRelacionDTO itemTG in tgcc)
                    {
                        if (flagTV && itemTG.IndOperacion == 1.ToString())
                        {
                            if (itemTG.IndModoOperacion != ConstantesAppServicio.SI)
                            {
                                potencia = potencia + (decimal)itemTG.PotGenerada;
                            }
                        }
                    }

                    if (flagTV)
                    {
                        if (ci.Pe1 <= potencia && potencia <= ci.Pe2)
                        {
                            result = (decimal)ci.Cincrem1;
                        }
                        else if (ci.Pe2 <= potencia)
                        {
                            result = (decimal)ci.Cincrem2;
                        }
                        //else if (ci.Pe3 <= potencia && potencia <= ci.Pe4)
                        //{
                        //    result = (decimal)ci.Cincrem3;
                        //}
                    }
                    else
                    {
                        result = (decimal)ci.Cincrem1;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Permite calcular los costos incrementales
        /// </summary>
        /// <param name="horasOperacion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<ReporteCostoIncrementalDTO> CalcularCostoIncremental(List<EveHoraoperacionDTO> horasOperacion, DateTime fecha)
        {
            List<ReporteCostoIncrementalDTO> result = new List<ReporteCostoIncrementalDTO>();
            var listaFormulasGenerales = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

            List<PrGrupoDTO> list = horasOperacion.Select(x => new PrGrupoDTO { Grupocodi = (int)x.Grupocodi, Grupopadre = x.Grupopadre }).Distinct().ToList();

            foreach (var item in list)
            {
                ReporteCostoIncrementalDTO entity = (new GrupoDespachoAppServicio()).ObtenerReporteCostoIncremental(fecha, listaFormulasGenerales, item);
                result.Add(entity);
            }

            return result;
        }


        /// <summary>
        /// Permite generar el documento excel con los datos de los usuarios
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void ExportarComparativoCostosMarginales(List<ComparativoItemResult> list, string path, string filename,
            string fecha, string barra)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COMPARATIVO");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "Comparativo CM Aplicativo vs CM Programado vs Costo Incremental";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;

                        ws.Cells[index, 2].Value = "HORA";
                        ws.Cells[index, 3].Value = "CM APLICATIVO (S/.)";
                        ws.Cells[index, 4].Value = "CM PROGRAMADO (S/.)";
                        ws.Cells[index, 5].Value = "COSTO INCREMENTAL (S/.)";
                        ws.Cells[index, 6].Value = "CM APP - CM PROG";
                        ws.Cells[index, 7].Value = "CM APP - CI";


                        rg = ws.Cells[index, 2, index, 7];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        bool flagCMProg = false;

                        index = 6;
                        foreach (ComparativoItemResult item in list)
                        {
                            ws.Cells[index, 2].Value = item.Hora;
                            ws.Cells[index, 3].Value = item.CmAplicativo;
                            ws.Cells[index, 4].Value = item.CmProgramado;
                            ws.Cells[index, 5].Value = item.CostoIncremental;
                            ws.Cells[index, 6].Value = item.DiferenciaAB;
                            ws.Cells[index, 7].Value = item.DiferenciaAC;

                            rg = ws.Cells[index, 2, index, 7];
                            rg.Style.Font.Size = 10;
                            rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                            rg = ws.Cells[index, 3, index, 7];
                            rg.Style.Numberformat.Format = "#,##0.00";

                            index++;

                            if (item.CmProgramado != null) flagCMProg = true;
                        }

                        rg = ws.Cells[5, 2, index - 1, 7];
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        var lineChart = ws.Drawings.AddChart("crtExtensionsSize", eChartType.LineMarkers) as ExcelLineChart;
                        lineChart.SetPosition(5, 0, 8, 0);
                        lineChart.SetSize(700, 430);

                        lineChart.Series.Add(ExcelRange.GetAddress(6, 3, index - 1, 3), ExcelRange.GetAddress(6, 2, index - 1, 2));

                        if (flagCMProg)
                            lineChart.Series.Add(ExcelRange.GetAddress(6, 4, index - 1, 4), ExcelRange.GetAddress(6, 2, index - 1, 2));


                        lineChart.Series.Add(ExcelRange.GetAddress(6, 5, index - 1, 5), ExcelRange.GetAddress(6, 2, index - 1, 2));
                        lineChart.Series[0].Header = "CM Aplicativo";

                        int indexSerie = 1;

                        if (flagCMProg)
                        {
                            lineChart.Series[1].Header = "CM Programado";
                            indexSerie = 2;
                        }

                        lineChart.Series[indexSerie].Header = "Costo Incremental";

                        lineChart.Title.Text = "Comparativo CM Aplicativo vs CM Programado - vs Costo Incremental - " + barra;
                        ws.Column(2).Width = 20;
                        rg = ws.Cells[5, 3, index, 7];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion



        #region Comparar registro de congestiones 

        /// <summary>
        /// Permite obtener el comparativo de diferencias de congestiones
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="flagExportar"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public ComparativoResultado ObtenerComparativoCongestiones(DateTime fecha, bool flagExportar,
            string path, string file)
        {
            ComparativoResultado entity = new ComparativoResultado();

            try
            {
                List<ComparativoCongestion> result = new List<ComparativoCongestion>();
                List<CmCongestionCalculoDTO> registroCongestion = FactorySic.GetCmCongestionCalculoRepository().ObtenerRegistroCongestion(fecha);
                List<CmCongestionCalculoDTO> congestionProceso = FactorySic.GetCmCongestionCalculoRepository().ObtenerCongestionProceso(fecha);
                List<CmCostomarginalDTO> listProcesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCM(fecha);

                if (listProcesos.Count > 0)
                {

                    for (int periodo = 1; periodo <= 48; periodo++)
                    {
                        DateTime fechaPeriodo = fecha.AddMinutes(periodo * 30);

                        List<CmCongestionCalculoDTO> subListRegistro = registroCongestion.Where(x =>
                            fechaPeriodo.Subtract(x.Congesfecinicio).TotalSeconds >= 0 &&
                            fechaPeriodo.Subtract(x.Congesfecfin).TotalSeconds <= 0).ToList();

                        List<CmCongestionCalculoDTO> subListProceso = congestionProceso.Where(x => x.Cmcongperiodo == periodo).ToList();

                        List<CmCongestionCalculoDTO> sistema = subListRegistro.Where(x =>
                            !subListProceso.Any(y => y.Configcodi == x.Configcodi && y.Grulincodi == x.Grulincodi && y.Regsegcodi == x.Regsegcodi)).ToList();

                        List<CmCongestionCalculoDTO> proceso = subListProceso.Where(x =>
                           !subListRegistro.Any(y => y.Configcodi == x.Configcodi && y.Grulincodi == x.Grulincodi && y.Regsegcodi == x.Regsegcodi)).ToList();

                        if (sistema.Count > 0 || proceso.Count > 0)
                        {
                            CmCostomarginalDTO cm = listProcesos.Where(x => x.Cmgncodi == periodo).FirstOrDefault();

                            if (cm != null)
                            {
                                ComparativoCongestion itemComparativo = new ComparativoCongestion
                                {
                                    Correlativo = (int)cm.Cmgncorrelativo,
                                    FechaEjecucion = ((DateTime)cm.Cmgnfeccreacion).ToString(ConstantesAppServicio.FormatoFechaHora),
                                    Hora = cm.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora),
                                    Estimador = cm.TipoEstimador,
                                    Programa = cm.VersionPDO
                                };

                                itemComparativo.ListaDetalle = new List<ComparativoCongestionDetalle>();

                                foreach (CmCongestionCalculoDTO itemSistema in sistema)
                                {
                                    ComparativoCongestionDetalle detSistema = new ComparativoCongestionDetalle
                                    {
                                        EquipoSistema = itemSistema.Equinomb,
                                        HoraInicio = itemSistema.Congesfecinicio.ToString(ConstantesAppServicio.FormatoOnlyHora),
                                        HoraFin = itemSistema.Congesfecfin.ToString(ConstantesAppServicio.FormatoOnlyHora)
                                    };
                                    itemComparativo.ListaDetalle.Add(detSistema);
                                }
                                foreach (CmCongestionCalculoDTO itemProceso in proceso)
                                {
                                    ComparativoCongestionDetalle detProceso = new ComparativoCongestionDetalle
                                    {
                                        EquipoProceso = itemProceso.Equinomb,
                                        Limite = itemProceso.Cmconglimite,
                                        Envio = itemProceso.Cmcongenvio,
                                        Recepcion = itemProceso.Cmcongrecepcion
                                    };
                                    itemComparativo.ListaDetalle.Add(detProceso);
                                }

                                result.Add(itemComparativo);
                            }
                        }
                    }

                    if (result.Count > 0)
                    {
                        entity.Resultado = 1;
                        entity.ListaCongestion = result;

                        if (flagExportar)
                        {
                            this.ExportarComparativoCongestiones(entity.ListaCongestion, path, file, fecha.ToString(ConstantesAppServicio.FormatoFecha));
                        }
                    }
                    else
                    {
                        entity.Resultado = 3;
                    }
                }
                else
                {
                    entity.Resultado = 2;
                }
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return entity;
        }

        /// <summary>
        /// Permite generar el comparativo de congestiones
        /// </summary>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="fecha"></param>
        public void ExportarComparativoCongestiones(List<ComparativoCongestion> list, string path, string filename, string fecha)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COMPARATIVO");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "Comparativo Congestiones";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;

                        ws.Cells[index, 2].Value = "Periodo";
                        rg = ws.Cells[index, 2, index, 5];
                        rg.Merge = true;
                        ws.Cells[index, 6].Value = "Congestiones en SGOCOES y no en resultados GAMS";
                        rg = ws.Cells[index, 6, index, 8];
                        rg.Merge = true;
                        ws.Cells[index, 9].Value = "Congestiones en resultados GAMS y no en SGOCOES";
                        rg = ws.Cells[index, 9, index, 12];
                        rg.Merge = true;

                        ws.Cells[index + 1, 2].Value = "Hora";
                        ws.Cells[index + 1, 3].Value = "Fecha Ejec.";
                        ws.Cells[index + 1, 4].Value = "Est.";
                        ws.Cells[index + 1, 5].Value = "PDO / RDO";
                        ws.Cells[index + 1, 6].Value = "Equipo";
                        ws.Cells[index + 1, 7].Value = "Desde";
                        ws.Cells[index + 1, 8].Value = "Hasta";
                        ws.Cells[index + 1, 9].Value = "Enlace";
                        ws.Cells[index + 1, 10].Value = "Límite";
                        ws.Cells[index + 1, 11].Value = "Envío";
                        ws.Cells[index + 1, 12].Value = "Recepción";

                        rg = ws.Cells[index, 2, index + 1, 12];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 7;
                        foreach (ComparativoCongestion item in list)
                        {
                            ws.Cells[index, 2].Value = item.Hora;
                            rg = ws.Cells[index, 2, index + item.ListaDetalle.Count - 1, 2];
                            rg.Merge = true;
                            ws.Cells[index, 3].Value = item.FechaEjecucion;
                            rg = ws.Cells[index, 3, index + item.ListaDetalle.Count - 1, 3];
                            rg.Merge = true;
                            ws.Cells[index, 4].Value = item.Estimador;
                            rg = ws.Cells[index, 4, index + item.ListaDetalle.Count - 1, 4];
                            rg.Merge = true;
                            ws.Cells[index, 5].Value = item.Programa;
                            rg = ws.Cells[index, 5, index + item.ListaDetalle.Count - 1, 5];
                            rg.Merge = true;

                            foreach (ComparativoCongestionDetalle detalle in item.ListaDetalle)
                            {
                                ws.Cells[index, 6].Value = detalle.EquipoSistema;
                                ws.Cells[index, 7].Value = detalle.HoraInicio;
                                ws.Cells[index, 8].Value = detalle.HoraFin;
                                ws.Cells[index, 9].Value = detalle.EquipoProceso;
                                ws.Cells[index, 10].Value = detalle.Limite;
                                ws.Cells[index, 11].Value = detalle.Envio;
                                ws.Cells[index, 12].Value = detalle.Recepcion;

                                rg = ws.Cells[index, 10, index, 12];
                                rg.Style.Numberformat.Format = "#0\\.00";

                                index++;
                            }
                        }

                        rg = ws.Cells[7, 2, index - 1, 12];

                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 12];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Comparar costos incrementales

        /// <summary>
        /// Permite obtener los filtros del comparativo
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public ComparativoCIFiltro ObtenerFiltrosCI(DateTime fecha)
        {
            ComparativoCIFiltro entity = new ComparativoCIFiltro();
            entity.ListaPeriodos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCM(fecha);

            foreach (CmCostomarginalDTO item in entity.ListaPeriodos)
            {
                item.FechaProceso = item.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
            }

            List<CmCostoIncrementalDTO> result = new List<CmCostoIncrementalDTO>();
            List<CmCostoIncrementalDTO> listaCI = FactorySic.GetCmCostoIncrementalRepository().GetByCriteria(fecha);

            var subList = listaCI.Select(x => new { x.Equicodi, x.Equinomb }).Distinct().ToList();

            foreach (var item in subList)
            {
                CmCostoIncrementalDTO itemResult = new CmCostoIncrementalDTO();
                itemResult.Equicodi = item.Equicodi;
                itemResult.Equinomb = item.Equinomb;
                result.Add(itemResult);
            }

            entity.ListaEquipos = result;

            return entity;
        }

        /// <summary>
        /// Permite obtener el reporte de costos incrementales
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="umbral"></param>
        /// <param name="idEquipo"></param>
        /// <param name="idHora"></param>
        /// <param name="flagExportar"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public ComparativoResultado ObtenerComparativoCostoIncremental(DateTime fecha, decimal umbral, int idEquipo, int idHora,
            bool flagExportar, string path, string file)
        {
            ComparativoResultado response = new ComparativoResultado();

            try
            {
                //- Lista de procesos del medio dia
                List<CmCostomarginalDTO> listProcesos = FactorySic.GetCmCostomarginalRepository().ObtenerUltimosProcesosCM(fecha).
                    Where(x => x.Cmgncodi == idHora || idHora == -1).ToList();


                //- Lista de costos incrementales del proceso
                List<CmCostoIncrementalDTO> listaCI = FactorySic.GetCmCostoIncrementalRepository().GetByCriteria(fecha).
                    Where(x => x.Equicodi == idEquipo || idEquipo == -1).ToList();

                //- Lista de costos incrementales del dia
                List<ReporteCostoIncrementalDTO> result = new List<ReporteCostoIncrementalDTO>();
                var listaFormulasGenerales = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

                var list = listaCI.Select(x => new { Grupocodi = (int)x.Grupocodi, Grupopadre = x.Grupopadre }).Distinct().ToList();

                foreach (var item in list)
                {
                    PrGrupoDTO grupo = new PrGrupoDTO { Grupocodi = item.Grupocodi, Grupopadre = item.Grupopadre };
                    ReporteCostoIncrementalDTO entity = (new GrupoDespachoAppServicio()).ObtenerReporteCostoIncremental(fecha, listaFormulasGenerales, grupo);
                    result.Add(entity);
                }

                List<ComparativoCostoIncremental> entitys = new List<ComparativoCostoIncremental>();

                //- Analizamos cada periodo
                foreach (CmCostomarginalDTO item in listProcesos)
                {
                    List<CmCostoIncrementalDTO> subListCI = listaCI.Where(x => x.Cmciperiodo == item.Cmgncodi).ToList();
                    string hora = item.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);

                    foreach (CmCostoIncrementalDTO itemCI in subListCI)
                    {
                        ComparativoCostoIncremental itemResult = new ComparativoCostoIncremental();
                        itemResult.Hora = item.Cmgnfecha.ToString(ConstantesAppServicio.FormatoOnlyHora);
                        itemResult.Unidad = itemCI.Equinomb;
                        itemResult.Ci1Gams = itemCI.Cmcitramo1;
                        itemResult.Ci2Gams = itemCI.Cmcitramo2;

                        ReporteCostoIncrementalDTO reporteCI = result.Where(x => x.Grupocodi == itemCI.Grupocodi).FirstOrDefault();

                        if (reporteCI != null)
                        {
                            itemResult.Ci1Sicoes = (decimal)reporteCI.Cincrem1;
                            itemResult.Ci2Sicoes = (decimal)reporteCI.Cincrem2;

                            if (itemResult.Ci2Sicoes == 0) itemResult.Ci2Sicoes = itemResult.Ci1Sicoes;
                        }

                        if (itemResult.Ci1Gams != null && itemResult.Ci1Sicoes != null)
                        {
                            itemResult.Diferencia1 = (decimal)Math.Abs((decimal)itemResult.Ci1Gams - (decimal)itemResult.Ci1Sicoes);

                            if (itemResult.Diferencia1 > umbral)
                            {
                                itemResult.IndDiferencia1 = ConstantesAppServicio.SI;
                            }
                        }

                        if (itemResult.Ci2Gams != null && itemResult.Ci2Sicoes != null)
                        {
                            itemResult.Diferencia2 = (decimal)Math.Abs((decimal)itemResult.Ci2Gams - (decimal)itemResult.Ci2Sicoes);

                            if (itemResult.Diferencia2 > umbral)
                            {
                                itemResult.IndDiferencia2 = ConstantesAppServicio.SI;
                            }
                        }

                        entitys.Add(itemResult);
                    }
                }

                response.Resultado = 1;
                response.ListaCostoIncremental = entitys;

                if (flagExportar)
                {
                    this.ExportarComparativoCostosIncrementales(response.ListaCostoIncremental, path, file,
                        fecha.ToString(ConstantesAppServicio.FormatoFecha));
                }

            }
            catch (Exception ex)
            {
                response.Resultado = -1;
                throw new Exception(ex.Message, ex);
            }

            return response;
        }

        public void ExportarComparativoCostosIncrementales(List<ComparativoCostoIncremental> list, string path, string filename, string fecha)
        {
            try
            {
                string file = path + filename;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("COMPARATIVO");

                    if (ws != null)
                    {
                        ws.Cells[2, 3].Value = "Comparativo Costos Incrementales";

                        ExcelRange rg = ws.Cells[2, 3, 3, 3];
                        rg.Style.Font.Size = 13;
                        rg.Style.Font.Bold = true;

                        int index = 5;

                        ws.Cells[3, 3].Value = "FECHA: " + fecha;

                        ws.Cells[index, 2].Value = "Periodo";
                        rg = ws.Cells[index, 2, index + 1, 2];
                        rg.Merge = true;

                        ws.Cells[index, 3].Value = "Unidad de Generación";
                        rg = ws.Cells[index, 3, index + 1, 3];
                        rg.Merge = true;

                        ws.Cells[index, 4].Value = "Tramo 01";
                        rg = ws.Cells[index, 4, index, 6];
                        rg.Merge = true;

                        ws.Cells[index, 7].Value = "Tramo 02";
                        rg = ws.Cells[index, 7, index, 9];
                        rg.Merge = true;

                        ws.Cells[index + 1, 4].Value = "CI GAMS";
                        ws.Cells[index + 1, 5].Value = "CI SGOCOES";
                        ws.Cells[index + 1, 6].Value = "Diferencia";
                        ws.Cells[index + 1, 7].Value = "CI GAMS";
                        ws.Cells[index + 1, 8].Value = "CI SGOCOES";
                        ws.Cells[index + 1, 9].Value = "Diferencia";

                        rg = ws.Cells[index, 2, index + 1, 9];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        index = 7;
                        foreach (ComparativoCostoIncremental item in list)
                        {
                            ws.Cells[index, 2].Value = item.Hora;
                            ws.Cells[index, 3].Value = item.Unidad;
                            ws.Cells[index, 4].Value = item.Ci1Gams;
                            ws.Cells[index, 5].Value = item.Ci1Sicoes;
                            ws.Cells[index, 6].Value = item.Diferencia1;
                            ws.Cells[index, 7].Value = item.Ci2Gams;
                            ws.Cells[index, 8].Value = item.Ci2Sicoes;
                            ws.Cells[index, 9].Value = item.Diferencia2;

                            if (item.IndDiferencia1 == ConstantesAppServicio.SI)
                            {
                                rg = ws.Cells[index, 6, index, 6];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffb4b4"));
                            }

                            if (item.IndDiferencia2 == ConstantesAppServicio.SI)
                            {
                                rg = ws.Cells[index, 9, index, 9];
                                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffb4b4"));
                            }


                            rg = ws.Cells[index, 4, index, 9];
                            rg.Style.Numberformat.Format = "#0\\.000";

                            index++;
                        }

                        rg = ws.Cells[7, 2, index - 1, 9];

                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));

                        ws.Column(2).Width = 30;
                        rg = ws.Cells[5, 3, index, 9];
                        rg.AutoFitColumns();

                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("Logo", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public CmUmbralComparacionDTO ObtenerParametros()
        {
            return FactorySic.GetCmUmbralComparacionRepository().
                   GetById(ConstantesCortoPlazo.IdConfiguracionUmbral);
        }

        #endregion

        #region Comparar Horas de Operación vs Reserva Secundaria

        /// <summary>
        /// Realizar cálculo
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        public void CalcularComparativoHOvsRsvaSec(DateTime fechaPeriodo, out ReporteComparativoHOvsRsvaSec objTab1, out ReporteComparativoHOvsRsvaSec objTab2)
        {
            string emprcodis = ConstantesAppServicio.ParametroDefecto;
            string equipadres = ConstantesAppServicio.ParametroDefecto;

            //mensajes
            List<ResultadoValidacionAplicativo> listaMsj = new List<ResultadoValidacionAplicativo>();

            #region Insumos

            // empresas, equipos, modos de operación
            (new INDAppServicio()).ListarUnidadTermicoXEmpresaXCentral(fechaPeriodo, emprcodis, equipadres, out List<SiEmpresaDTO> listaEmpresa
                            , out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaGenerador
                            , out List<PrGrupoDTO> listaGrupoModo, out List<PrGrupoDTO> listaGrupoDespacho);
            List<EqEquipoDTO> listaAllEq = new List<EqEquipoDTO>();
            listaAllEq.AddRange(listaCentral);
            listaAllEq.AddRange(listaGenerador);

            //el ciclo combinado de CT caña brava no debe considerarse como especial
            foreach (var reg in listaGrupoModo)
                reg.TieneModoEspecial = reg.TieneModoEspecial && !reg.TieneModoCicloCombinado;

            // Horas de Operacion de los modos y unidades especiales
            List<EveHoraoperacionDTO> listaHO = ListarHoXModoyUnidades(fechaPeriodo, emprcodis, equipadres);

            // Reserva Secundario
            List<MeMedicion48DTO> lista48RsvSecXGrDesp = Listar48ReservaSecundaria(fechaPeriodo, listaAllEq, out List<PrGrupoDTO> listaUrs);

            #endregion

            #region Cálculo RSF Asignada - Horas de Operación

            List<DatoComparativoHOvsRasig> listaRpt1 = CalcularComparativoRasignadavHO(fechaPeriodo, lista48RsvSecXGrDesp
                                                            , listaHO, listaGrupoModo, listaUrs, listaGrupoDespacho, out List<ResultadoValidacionAplicativo> listaMsj1);
            listaMsj.AddRange(listaMsj1);

            objTab1 = new ReporteComparativoHOvsRsvaSec();
            objTab1.Titulo = "RSF Asignada - Horas de Operación";
            objTab1.Fecha = fechaPeriodo;

            foreach (var reg in listaRpt1)
            {
                for (int h = 1; h <= 48; h++)
                {
                    var bloqueIni = DateTime.Today.AddMinutes((h - 1) * 30).ToString(ConstantesAppServicio.FormatoHora);
                    var bloqueFin = DateTime.Today.AddMinutes(h * 30).ToString(ConstantesAppServicio.FormatoHora);
                    var periodo = string.Format("{0} - {1}", bloqueIni, bloqueFin);

                    int pos = h - 1;
                    if (reg.ListaAlerta[pos])
                    {
                        var regFila = new DatoComparativoHOvsRsvaSec();
                        regFila.Urs = (reg.Urs.Gruponomb ?? "").Trim();
                        regFila.Central = (reg.Urs.Central ?? "").Trim();
                        regFila.Agc = (reg.Urs.CodAgc ?? "").Trim();
                        regFila.Periodo = periodo;
                        regFila.Mensaje = reg.ListaAlertadesc[pos];

                        objTab1.ListaDato.Add(regFila);
                    }
                }
            }

            #endregion

            #region Cálculo Horas de Operación - RSF Asignada

            List<DatoComparativoHOvsRasig> listaRpt2 = CalcularComparativoHOvsRasignada(fechaPeriodo, lista48RsvSecXGrDesp
                                                            , listaHO, listaGrupoModo, listaGrupoDespacho, out List<ResultadoValidacionAplicativo> listaMsj2);
            listaMsj.AddRange(listaMsj2);

            objTab2 = new ReporteComparativoHOvsRsvaSec();
            objTab2.Titulo = "Horas de Operación - RSF Asignada";
            objTab2.Fecha = fechaPeriodo;

            foreach (var reg in listaRpt2)
            {
                var regFila = new DatoComparativoHOvsRsvaSec();
                regFila.Central = (reg.Ho.Central ?? "").Trim();
                regFila.Gruponomb = (reg.Ho.Gruponomb ?? "").Trim();
                regFila.HoraIni = reg.Ho.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHora);
                regFila.HoraFin = reg.Ho.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHora);

                string bloques = string.Join(",", reg.ListaMensaje);
                regFila.Mensaje = "No se registró la correspondiente RSF asignada en los bloques horarios " + bloques;

                objTab2.ListaDato.Add(regFila);
            }

            #endregion
        }

        #region Cálculo

        /// <summary>
        /// Actualizar datos del modo de operación
        /// </summary>
        /// <param name="f"></param>
        /// <param name="lista48xGrDesp"></param>
        /// <param name="lista48RsvSecXGrDesp"></param>
        /// <param name="listaHO"></param>
        /// <param name="porcentajeRpf"></param>
        /// <param name="umbralMax"></param>
        /// <param name="listaGrupoModo"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaUnidadesEsp"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<DatoComparativoHOvsRasig> CalcularComparativoRasignadavHO(DateTime f, List<MeMedicion48DTO> lista48RsvSecXGrDesp
                                    , List<EveHoraoperacionDTO> listaHO, List<PrGrupoDTO> listaAllGrupoModo, List<PrGrupoDTO> listaUrs, List<PrGrupoDTO> listaGrupoDespacho
                                    , out List<ResultadoValidacionAplicativo> listaMsj)
        {
            List<DatoComparativoHOvsRasig> listaRpt = new List<DatoComparativoHOvsRasig>();
            listaMsj = new List<ResultadoValidacionAplicativo>();

            List<EveSubcausaeventoDTO> listaSubcausa = servHO.ListarTipoOperacionHO();

            var listaHO30min = HorasOperacionAppServicio.ListarHO30minConsumoCombustible(listaHO, f);

            //solo considerar horas de operación "POR RSF"
            List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo)
                                                    .OrderBy(x => x.Equipadre).ThenBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

            //lista de modos de operacion del dia
            List<int> lgrupocodiModo = listaHOModo.Select(x => x.Grupocodi ?? 0).Distinct().ToList();
            List<PrGrupoDTO> listaModo = listaAllGrupoModo.Where(x => lgrupocodiModo.Contains(x.Grupocodi)).ToList();

            //lista de urs del dia
            List<int> listaUrscodo = lista48RsvSecXGrDesp.Select(x => x.Grupourspadre).Distinct().ToList();

            foreach (var grupocodiUrs in listaUrscodo)
            {
                if (grupocodiUrs == 914)
                { }

                PrGrupoDTO regUrs = listaUrs.Find(x => x.Grupocodi == grupocodiUrs);
                List<MeMedicion48DTO> listaUrs48 = lista48RsvSecXGrDesp.Where(x => x.Grupourspadre == regUrs.Grupocodi).ToList();

                //asignar el valor de urs por cada media hora
                foreach (var m48 in listaUrs48)
                {
                    for (int h = 1; h <= 48; h++)
                    {
                        decimal? valorHRsvUp = null, valorHRsvDown = null;
                        if (m48.Item == ConstantesCortoPlazo.TipoRsvSecUp)
                            valorHRsvUp = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        if (m48.Item == ConstantesCortoPlazo.TipoRsvSecDown)
                            valorHRsvDown = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);

                        AsignarDatosX30minAUrs(h, valorHRsvUp, valorHRsvDown, 0, f, regUrs, listaSubcausa, ref listaRpt);
                    }
                }

                List<int> lgrupocodiDespXUrs = listaUrs48.Select(x => x.Grupocodi).Distinct().ToList();
                var listaGrXurs = listaGrupoDespacho.Where(x => lgrupocodiDespXUrs.Contains(x.Grupocodi)).ToList();
                if (listaGrXurs.Any())
                {
                    //obtener todos los modos de la central
                    int equipadre = listaGrXurs.First().Equipadre;
                    List<PrGrupoDTO> listaModoPosibleXUrs = listaModo.Where(x => x.Equipadre == equipadre).ToList();

                    //asignar calificacion
                    List<EveHoraoperacionDTO> listaHOxUrs = listaHOModo.Where(x => listaModoPosibleXUrs.Select(y => y.Grupocodi).Contains(x.Grupocodi ?? 0)).ToList();
                    foreach (var regHo in listaHOxUrs)
                    {
                        for (int h = regHo.HIni48; h <= regHo.HFin48; h++)
                        {
                            if (h >= 1 && h <= 48)
                            {
                                AsignarDatosX30minAUrs(h, 0, 0, regHo.Subcausacodi ?? 0, f, regUrs, listaSubcausa, ref listaRpt);
                            }

                        }
                    }
                }
            }

            //formatear resultado
            var listaSubcausaValido = ConstantesCortoPlazo.ListaCalificacionAlertaUrs;
            foreach (var reg in listaRpt)
            {
                bool tieneAlerta = false;

                for (int h = 1; h <= 48; h++)
                {
                    int pos = h - 1;

                    reg.ListaHora[pos] = DateTime.Today.AddMinutes(h * 30).ToString(ConstantesAppServicio.FormatoHora);
                    if (h == 48) reg.ListaHora[pos] = "23:59";

                    //generar Tabla Resultado
                    decimal rDown = reg.ListaMWRDown[pos].GetValueOrDefault(0);
                    decimal rUp = reg.ListaMWRUp[pos].GetValueOrDefault(0);
                    int subcausacodi = reg.ListaCalifHo[pos];
                    if (rDown > 0 || rUp > 0)
                    {
                        if (!listaSubcausaValido.Contains(subcausacodi))
                        {
                            if (subcausacodi == 0)
                                reg.ListaAlertadesc[pos] = "No existe registro en horas de operación";
                            else
                                reg.ListaAlertadesc[pos] = "La calificación en Horas de Operación no es \"POR RSF\" o \"POTENCIA Y ENERGIA\"";

                            reg.ListaAlerta[pos] = true;
                        }
                    }
                    tieneAlerta = tieneAlerta || reg.ListaAlerta[pos];
                }

                reg.TieneAlerta = tieneAlerta;
            }

            //solo retornar las horas de operación que tienen alerta
            listaRpt = listaRpt.Where(x => x.TieneAlerta).ToList();

            return listaRpt;
        }

        /// <summary>
        /// Actualizar datos del modo de operación
        /// </summary>
        /// <param name="f"></param>
        /// <param name="lista48xGrDesp"></param>
        /// <param name="lista48RsvSecXGrDesp"></param>
        /// <param name="listaHO"></param>
        /// <param name="porcentajeRpf"></param>
        /// <param name="umbralMax"></param>
        /// <param name="listaGrupoModo"></param>
        /// <param name="listaGrupoDespacho"></param>
        /// <param name="listaUnidadesEsp"></param>
        /// <param name="listaMsj"></param>
        /// <returns></returns>
        private List<DatoComparativoHOvsRasig> CalcularComparativoHOvsRasignada(DateTime f, List<MeMedicion48DTO> lista48RsvSecXGrDesp
                                    , List<EveHoraoperacionDTO> listaHO, List<PrGrupoDTO> listaGrupoModo, List<PrGrupoDTO> listaGrupoDespacho
                                    , out List<ResultadoValidacionAplicativo> listaMsj)
        {
            List<DatoComparativoHOvsRasig> listaRpt = new List<DatoComparativoHOvsRasig>();
            listaMsj = new List<ResultadoValidacionAplicativo>();

            List<EveSubcausaeventoDTO> listaSubcausa = servHO.ListarTipoOperacionHO();

            var listaHO30min = HorasOperacionAppServicio.ListarHO30minConsumoCombustible(listaHO, f);

            //solo considerar horas de operación "POR RSF"
            List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo
                                                                        && x.Subcausacodi == ConstantesSubcausaEvento.SubcausaPorRsf)
                                                    .OrderBy(x => x.Equipadre).ThenBy(x => x.Hophorini).ToList();
            List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

            foreach (var regHo in listaHOModo)
            {
                if (regHo.Hopcodi == 1)
                { }

                PrGrupoDTO regGrupoModo = listaGrupoModo.Find(x => x.Grupocodi == regHo.Grupocodi);
                if (regGrupoModo != null) //validar si la hora de operación tiene un modo con operación comercial para esa fecha
                {
                    List<PrGrupoDTO> listaGrupoDespachoXModo = listaGrupoDespacho.Where(x => regGrupoModo.ListaGrupocodiDespacho.Contains(x.Grupocodi)).ToList();
                    List<EveHoraoperacionDTO> listaHoXModo = listaHOUnidad.Where(x => x.Hopcodipadre == regHo.Hopcodi).ToList();

                    if (listaGrupoDespachoXModo.Any())
                    {
                        //Obtener el valor de Despacho, reserva secundaria del modo, obtener los datos de Ho para los modos no especiales
                        foreach (var regGrupoDespacho in listaGrupoDespachoXModo)
                        {
                            MeMedicion48DTO reg48RsvSecUp = lista48RsvSecXGrDesp.Find(x => x.Item == ConstantesCortoPlazo.TipoRsvSecUp && x.Grupocodi == regGrupoDespacho.Grupocodi);
                            MeMedicion48DTO reg48RsvSecDown = lista48RsvSecXGrDesp.Find(x => x.Item == ConstantesCortoPlazo.TipoRsvSecDown && x.Grupocodi == regGrupoDespacho.Grupocodi);

                            for (int h = regHo.HIni48; h <= regHo.HFin48; h++)
                            {
                                if (h >= 1 && h <= 48)
                                {
                                    DateTime fi = f.Date.AddMinutes(h * 30);

                                    decimal? valorHRsvUp = null, valorHRsvDown = null;
                                    if (reg48RsvSecUp != null) valorHRsvUp = (decimal?)reg48RsvSecUp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48RsvSecUp, null);
                                    if (reg48RsvSecDown != null) valorHRsvDown = (decimal?)reg48RsvSecDown.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg48RsvSecDown, null);

                                    //a cada modo de operacion que tiene hora de operación se le asigna el ejecutado, up, down de cada grupo despacho
                                    AsignarDatosX30minAHo(h, valorHRsvUp, valorHRsvDown, f, regGrupoModo, regHo, regHo.Hopcodi, regHo.Grupocodi.Value, ref listaRpt);

                                    //luego de asignar, se quita esos valores para que no puedan ser usados en otros modos de operación
                                    if (reg48RsvSecUp != null) reg48RsvSecUp.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg48RsvSecUp, 0.0m);
                                    if (reg48RsvSecDown != null) reg48RsvSecDown.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg48RsvSecDown, 0.0m);
                                }
                            }
                        }
                    }
                    else
                    {
                        listaMsj.Add(new ResultadoValidacionAplicativo() { Equipadre = regGrupoModo.Equipadre, Descripcion = string.Format("El modo de operación {0} no tienen asociado a ninguna grupo de despacho.", regGrupoModo.Gruponomb) });
                    }
                }
            }

            //formatear resultado
            foreach (var reg in listaRpt)
            {
                bool tieneAlerta = false;

                for (int h = reg.HIni; h <= reg.HFin; h++)
                {
                    int pos = h - 1;
                    //generar Tabla Resultado
                    decimal rDown = reg.ListaMWRDown[pos].GetValueOrDefault(0);
                    decimal rUp = reg.ListaMWRUp[pos].GetValueOrDefault(0);

                    reg.ListaHora[pos] = DateTime.Today.AddMinutes(h * 30).ToString(ConstantesAppServicio.FormatoHora);
                    if (h == 48) reg.ListaHora[pos] = "23:59";

                    reg.ListaAlerta[pos] = rDown <= 0 && rUp <= 0;
                    if (reg.ListaAlerta[pos])
                    {
                        string bloqueIni = DateTime.Today.AddMinutes((h - 1) * 30).ToString(ConstantesAppServicio.FormatoHora);
                        string bloqueFin = DateTime.Today.AddMinutes((h) * 30).ToString(ConstantesAppServicio.FormatoHora);
                        reg.ListaMensaje.Add(string.Format("[{0} - {1}]", bloqueIni, bloqueFin));
                    }
                    tieneAlerta = tieneAlerta || reg.ListaAlerta[pos];
                }

                reg.TieneAlerta = tieneAlerta;
            }

            //solo retornar las horas de operación que tienen alerta
            listaRpt = listaRpt.Where(x => x.TieneAlerta).ToList();

            return listaRpt;
        }

        /// <summary>
        /// Actualizar datos de la Unidad de generación
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorH"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="regUnidad"></param>
        /// <param name="regModo"></param>
        /// <param name="grupocodimodo"></param>
        /// <param name="listaCurva"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaCelda"></param>
        private void AsignarDatosX30minAHo(int h, decimal? valorHRsvUp, decimal? valorHRsvDown
                                                , DateTime fechaPeriodo, PrGrupoDTO regModo, EveHoraoperacionDTO ho, int hopcodi, int grupocodiModo
                                                , ref List<DatoComparativoHOvsRasig> listaRpt)
        {
            var regReporte = listaRpt.Find(x => x.Hopcodi == hopcodi);

            if (regReporte == null)
            {
                regReporte = new DatoComparativoHOvsRasig()
                {
                    Hopcodi = hopcodi,
                    Fecha = fechaPeriodo,
                    Grupocodi = grupocodiModo,
                    Modo = regModo,
                    Ho = ho,
                    HIni = ho.HIni48,
                    HFin = ho.HFin48
                };

                //setear valor
                listaRpt.Add(regReporte);
            }

            int pos = h - 1;
            if (valorHRsvUp >= 0)
                regReporte.ListaMWRUp[pos] = (regReporte.ListaMWRUp[pos] ?? 0) + valorHRsvUp;
            if (valorHRsvDown >= 0)
                regReporte.ListaMWRDown[pos] = (regReporte.ListaMWRDown[pos] ?? 0) + valorHRsvDown;
        }

        /// <summary>
        /// Actualizar datos de la Unidad de generación
        /// </summary>
        /// <param name="h"></param>
        /// <param name="valorH"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="regUnidad"></param>
        /// <param name="regModo"></param>
        /// <param name="grupocodimodo"></param>
        /// <param name="listaCurva"></param>
        /// <param name="listaDetHo"></param>
        /// <param name="listaCelda"></param>
        private void AsignarDatosX30minAUrs(int h, decimal? valorHRsvUp, decimal? valorHRsvDown, int subcausacodi
                                                , DateTime fechaPeriodo, PrGrupoDTO regUrs, List<EveSubcausaeventoDTO> listaSubcausa, ref List<DatoComparativoHOvsRasig> listaRpt)
        {
            var regReporte = listaRpt.Find(x => x.GrupocodiUrs == regUrs.Grupocodi);
            var regSubcausa = listaSubcausa.Find(x => x.Subcausacodi == subcausacodi);

            if (regReporte == null)
            {
                regReporte = new DatoComparativoHOvsRasig()
                {
                    GrupocodiUrs = regUrs.Grupocodi,
                    Fecha = fechaPeriodo,
                    Urs = regUrs,
                };

                //setear valor
                listaRpt.Add(regReporte);
            }

            int pos = h - 1;
            if (valorHRsvUp >= 0)
                regReporte.ListaMWRUp[pos] = (regReporte.ListaMWRUp[pos] ?? 0) + valorHRsvUp;
            if (valorHRsvDown >= 0)
                regReporte.ListaMWRDown[pos] = (regReporte.ListaMWRDown[pos] ?? 0) + valorHRsvDown;

            var listaSubcausaValido = ConstantesCortoPlazo.ListaCalificacionAlertaUrs;
            if (regReporte.ListaCalifHo[pos] == 0 || listaSubcausaValido.Contains(subcausacodi))
            {
                regReporte.ListaCalifHo[pos] = subcausacodi;
                regReporte.ListaSubcausadesc[pos] = regSubcausa != null ? regSubcausa.Subcausadesc : "";
            }
        }

        #endregion

        /// <summary>
        /// Obtener reporte web
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        public List<ReporteComparativoHOvsRsvaSec> GenerarReporteWebHOvsRsvaSec(DateTime fechaConsulta, out int numInconsistencia)
        {
            numInconsistencia = 0;

            CalcularComparativoHOvsRsvaSec(fechaConsulta, out ReporteComparativoHOvsRsvaSec objTab1, out ReporteComparativoHOvsRsvaSec objTab2);
            numInconsistencia += objTab1.ListaDato.Count;
            numInconsistencia += objTab2.ListaDato.Count;

            List<ReporteComparativoHOvsRsvaSec> l = new List<ReporteComparativoHOvsRsvaSec>();

            objTab1.ReporteHtml = GenerarHtmlTab1RptHOvsRsvaSec(objTab1.ListaDato);
            l.Add(objTab1);

            objTab2.ReporteHtml = GenerarHtmlTab2RptHOvsRasig(objTab2.ListaDato);
            l.Add(objTab2);

            return l;
        }

        private string GenerarHtmlTab1RptHOvsRsvaSec(List<DatoComparativoHOvsRsvaSec> listaDato)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"
                   <table class='pretty tabla-adicional tbl_comparativo' border='0' cellspacing='0' style='table-layout: fixed;'>
                    <thead>
                        <tr>
                            <th style='width: 40px;'>URS</th>
                            <th style='width: 60px;'>Central</th>
                            <th style='width: 40px;'>Cod. AGC</th>
                            <th style='width: 20px;'>Periodo</th>
                            <th style='width: 150px;'>Mensaje</th>
                        </tr>
                    </thead>
                    <tbody>
            ");

            foreach (var reg in listaDato)
            {
                str.AppendFormat(@"
                        <tr>
                            <td style='text-align: center;'>{0}</td>
                            <td style='text-align: center;'>{1}</td>
                            <td style='text-align: center;'>{2}</td>
                            <td style='text-align: center;'>{3}</td>
                            <td style='text-align: left;white-space: break-spaces;'>{4}</td>
                        </tr>
                ", reg.Urs
                , reg.Central
                , reg.Agc
                , reg.Periodo
                , reg.Mensaje
                );
            }

            str.Append(@"
                    </tbody>
                </table>
            ");

            return str.ToString();
        }

        private string GenerarHtmlTab2RptHOvsRasig(List<DatoComparativoHOvsRsvaSec> listaDato)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"
                   <table class='pretty tabla-adicional tbl_comparativo' border='0' cellspacing='0' style='table-layout: fixed;'>
                    <thead>
                        <tr>
                            <th style='width: 40px;'>Central</th>
                            <th style='width: 60px;'>Modo de Operación</th>
                            <th style='width: 20px;'>En Paralelo</th>
                            <th style='width: 20px;'>Fin Registro</th>
                            <th style='width: 120px;'>Mensaje</th>
                        </tr>
                    </thead>
                    <tbody>
            ");

            foreach (var reg in listaDato)
            {
                str.AppendFormat(@"
                        <tr>
                            <td style='text-align: center;'>{0}</td>
                            <td style='text-align: center;'>{1}</td>
                            <td style='text-align: center;'>{2}</td>
                            <td style='text-align: center;'>{3}</td>
                            <td style='text-align: left;white-space: break-spaces;'>{4}</td>
                        </tr>
                ", reg.Central
                , reg.Gruponomb
                , reg.HoraIni
                , reg.HoraFin
                , reg.Mensaje
                );
            }

            str.Append(@"
                    </tbody>
                </table>
            ");

            return str.ToString();
        }

        /// <summary>
        /// Generar Reporte Excel
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="nameFile"></param>
        public void GenerarExcelComparativoHOvsRsvaSec(string ruta, DateTime fechaConsulta, out string nameFile)
        {
            CalcularComparativoHOvsRsvaSec(fechaConsulta, out ReporteComparativoHOvsRsvaSec objTab1, out ReporteComparativoHOvsRsvaSec objTab2);

            //Nombre de archivo
            nameFile = string.Format("ComparativoRSF_HO_{0}.xlsx", fechaConsulta.ToString(ConstantesAppServicio.FormatoFechaDMY));

            string rutaFile = ruta + nameFile;

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                GenerarHojaExcelHOvsRsvaSecTab01(xlPackage, "RSFvsHO", 9, 2, objTab1);
                xlPackage.Save();

                GenerarHojaExcelHOvsRsvaSecTab02(xlPackage, "HOvsRSF", 9, 2, objTab2);
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Generar hoja excel de Tab1
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="objtab"></param>
        private void GenerarHojaExcelHOvsRsvaSecTab01(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, ReporteComparativoHOvsRsvaSec objtab)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string font = "Calibri";
            string colorCeldaCs = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int rowTitulo = 6;
            int rowFecha = rowTitulo + 1;

            ws.Cells[rowTitulo, colIniTabla].Value = objtab.Titulo;
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colIniTabla, rowTitulo, colIniTabla, "Centro", "Izquierda", "#000000", "#FFFFFF", font, 14, true);

            ws.Cells[rowFecha, colIniTabla].Value = "Fecha: ";
            ws.Cells[rowFecha, colIniTabla + 1].Value = objtab.Fecha.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.SetFormatoCelda(ws, rowFecha, colIniTabla, rowFecha, colIniTabla, "Centro", "Derecha", "#000000", "#FFFFFF", font, 12, true);

            //
            int colUrs = colIniTabla;
            int colCentral = colUrs + 1;
            int colAgc = colCentral + 1;
            int colPeriodo = colAgc + 1;
            int colMensaje = colPeriodo + 1;

            int rowIniCabecera = rowIniTabla;

            ws.Cells[rowIniCabecera, colUrs].Value = "URS";
            ws.Cells[rowIniCabecera, colCentral].Value = "Central";
            ws.Cells[rowIniCabecera, colAgc].Value = "Cod. AGC";
            ws.Cells[rowIniCabecera, colPeriodo].Value = "Periodo";
            ws.Cells[rowIniCabecera, colMensaje].Value = "Mensaje";

            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colUrs, rowIniCabecera, colMensaje, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniCabecera, colUrs, rowIniCabecera, colMensaje, colorLinea, true);

            ws.Column(1).Width = 3;
            ws.Column(colUrs).Width = 16;
            ws.Column(colCentral).Width = 37;
            ws.Column(colAgc).Width = 18;
            ws.Column(colPeriodo).Width = 18;
            ws.Column(colMensaje).Width = 82;

            #endregion

            #region Cuerpo

            int rowData = rowIniCabecera + 1;

            foreach (var regDato in objtab.ListaDato)
            {
                ws.Cells[rowData, colUrs].Value = regDato.Urs;
                ws.Cells[rowData, colCentral].Value = regDato.Central;
                ws.Cells[rowData, colAgc].Value = regDato.Agc;
                ws.Cells[rowData, colPeriodo].Value = regDato.Periodo;
                ws.Cells[rowData, colMensaje].Value = regDato.Mensaje;

                UtilExcel.SetFormatoCelda(ws, rowData, colUrs, rowData, colMensaje, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colUrs, rowData, colMensaje, colorLinea, true);

                rowData++;
            }

            #endregion

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());

            UtilExcel.AddImage(ws, img, 1, 2);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //ws.View.FreezePanes(rowIniCabecera + 1, colUrs + 1);

            ws.View.ZoomScale = 80;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        /// <summary>
        /// Generar hoja excel de Tab2
        /// </summary>
        /// <param name="xlPackage"></param>
        /// <param name="nameWS"></param>
        /// <param name="rowIniTabla"></param>
        /// <param name="colIniTabla"></param>
        /// <param name="objtab"></param>
        private void GenerarHojaExcelHOvsRsvaSecTab02(ExcelPackage xlPackage, string nameWS, int rowIniTabla, int colIniTabla, ReporteComparativoHOvsRsvaSec objtab)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            string font = "Calibri";
            string colorCeldaCs = "#0070C0";
            string colorTextoFijo = "#ffffff";

            string colorLinea = "#000000";

            #region  Filtros y Cabecera

            int rowTitulo = 6;
            int rowFecha = rowTitulo + 1;

            ws.Cells[rowTitulo, colIniTabla].Value = objtab.Titulo;
            UtilExcel.SetFormatoCelda(ws, rowTitulo, colIniTabla, rowTitulo, colIniTabla, "Centro", "Izquierda", "#000000", "#FFFFFF", font, 14, true);

            ws.Cells[rowFecha, colIniTabla].Value = "Fecha: ";
            ws.Cells[rowFecha, colIniTabla + 1].Value = objtab.Fecha.ToString(ConstantesAppServicio.FormatoFecha);
            UtilExcel.SetFormatoCelda(ws, rowFecha, colIniTabla, rowFecha, colIniTabla, "Centro", "Derecha", "#000000", "#FFFFFF", font, 12, true);

            //
            int colCentral = colIniTabla;
            int colModo = colCentral + 1;
            int colInicio = colModo + 1;
            int colFinal = colInicio + 1;
            int colMensaje = colFinal + 1;

            int rowIniCabecera = rowIniTabla;

            ws.Cells[rowIniCabecera, colCentral].Value = "Central";
            ws.Cells[rowIniCabecera, colModo].Value = "Modo de Operación";
            ws.Cells[rowIniCabecera, colInicio].Value = "En Paralelo";
            ws.Cells[rowIniCabecera, colFinal].Value = "Fin Registro";
            ws.Cells[rowIniCabecera, colMensaje].Value = "Mensaje";

            UtilExcel.SetFormatoCelda(ws, rowIniCabecera, colCentral, rowIniCabecera, colMensaje, "Centro", "Centro", colorTextoFijo, colorCeldaCs, font, 12, true, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniCabecera, colCentral, rowIniCabecera, colMensaje, colorLinea, true);

            ws.Column(1).Width = 3;
            ws.Column(colCentral).Width = 37;
            ws.Column(colModo).Width = 37;
            ws.Column(colInicio).Width = 18;
            ws.Column(colFinal).Width = 18;
            ws.Column(colMensaje).Width = 82;

            #endregion

            #region Cuerpo

            int rowData = rowIniCabecera + 1;

            foreach (var regDato in objtab.ListaDato)
            {
                ws.Cells[rowData, colCentral].Value = regDato.Central;
                ws.Cells[rowData, colModo].Value = regDato.Gruponomb;
                ws.Cells[rowData, colInicio].Value = regDato.HoraIni;
                ws.Cells[rowData, colFinal].Value = regDato.HoraFin;
                ws.Cells[rowData, colMensaje].Value = regDato.Mensaje;

                UtilExcel.SetFormatoCelda(ws, rowData, colCentral, rowData, colMensaje, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false);
                UtilExcel.SetFormatoCelda(ws, rowData, colMensaje, rowData, colMensaje, "Centro", "Centro", "#000000", "#FFFFFF", font, 12, false, true);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowData, colCentral, rowData, colMensaje, colorLinea, true);

                rowData++;
            }

            #endregion

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConstantesAppServicio.EnlaceLogoCoes);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());

            UtilExcel.AddImage(ws, img, 1, 2);

            //No mostrar lineas
            ws.View.ShowGridLines = false;

            //ws.View.FreezePanes(rowIniCabecera + 1, colUrs + 1);

            ws.View.ZoomScale = 80;

            //excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = font;
        }

        #endregion
    }

    /// <summary>
    /// Estructura para los datos por cada media hora
    /// </summary>
    public class ComparativoItemResult
    {
        public string Hora { get; set; }
        public decimal? DemandaEMS { get; set; }
        public decimal? DemandaEjecutada { get; set; }
        public decimal? Desviacion { get; set; }
        public decimal? Diferencia { get; set; }
        public string Indicador { get; set; }
        public int Barra { get; set; }
        public decimal? CmAplicativo { get; set; }
        public decimal? CmProgramado { get; set; }
        public decimal? CostoIncremental { get; set; }
        public decimal? DiferenciaAB { get; set; }
        public decimal? DiferenciaAC { get; set; }
    }

    public class ComparativoResultado
    {
        public int Resultado { get; set; }
        public List<ComparativoItemResult> ListaDatos { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public List<ComparativoCongestion> ListaCongestion { get; set; }
        public List<ComparativoCostoIncremental> ListaCostoIncremental { get; set; }

    }

    public class ComparativoCongestion
    {
        public int Correlativo { get; set; }
        public string Hora { get; set; }
        public string FechaEjecucion { get; set; }
        public string Estimador { get; set; }
        public string Programa { get; set; }

        public List<ComparativoCongestionDetalle> ListaDetalle { get; set; }
    }

    public class ComparativoCongestionDetalle
    {
        public string EquipoSistema { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string EquipoProceso { get; set; }
        public decimal? Limite { get; set; }
        public decimal? Envio { get; set; }
        public decimal? Recepcion { get; set; }

    }

    public class ComparativoCostoIncremental
    {
        public string Hora { get; set; }
        public string Unidad { get; set; }
        public decimal? Ci1Gams { get; set; }
        public decimal? Ci1Sicoes { get; set; }
        public decimal? Diferencia1 { get; set; }
        public decimal? Ci2Gams { get; set; }
        public decimal? Ci2Sicoes { get; set; }
        public decimal? Diferencia2 { get; set; }
        public string IndDiferencia1 { get; set; }
        public string IndDiferencia2 { get; set; }
    }

    public class ComparativoCIFiltro
    {
        public List<CmCostomarginalDTO> ListaPeriodos { get; set; }
        public List<CmCostoIncrementalDTO> ListaEquipos { get; set; }
    }
}
