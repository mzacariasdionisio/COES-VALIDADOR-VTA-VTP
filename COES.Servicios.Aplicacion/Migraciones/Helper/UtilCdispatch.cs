using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Despacho;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Siosein2;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace COES.Servicios.Aplicacion.Migraciones.Helper
{
    /// <summary>
    /// clase utilitario de cálculo CDispatch
    /// </summary>
    public class UtilCdispatch
    {
        #region Cálculo CDisptach (Reserva Fría, Reserva Rotante, MWxMantto)

        /// <summary>
        /// Data de Reserva Fria del Sistema
        /// </summary>
        /// <param name="regCDespacho"></param>
        /// <param name="reporteRFriaOut"></param>
        public static void ListarReporteCDispatch(CDespachoGlobal regCDespacho, out ResultadoCDespacho reporteRFriaOut)
        {
            reporteRFriaOut = new ResultadoCDespacho();

            List<MeMedicion48DTO> listaDataTotal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRFTotal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRFDetalleTotal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRFXFenerg = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRFRapida = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRFMinima = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRFIndisp = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> listaRRTotal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaRRDetalle = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> listaREficiente = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaREfiGasDetalle = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaREfiCarbonDetalle = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> listaMMTotal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaMMDetalle = new List<MeMedicion48DTO>();

            List<ResultadoValidacionAplicativo> listaMensajeValidacion = new List<ResultadoValidacionAplicativo>();

            //
            int lectcodi = regCDespacho.Lectcodi;
            List<EqPropequiDTO> listaPropequi = regCDespacho.ListaPropequi;
            List<MePtomedicionDTO> listaPtoDespacho30min = regCDespacho.ListaAllPtoPlantilla;

            // Generar la data de Reserva Fría
            for (DateTime f1 = regCDespacho.FechaIni; f1 <= regCDespacho.FechaFin; f1 = f1.AddDays(1))
            {
                CDespachoDiario regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == f1);

                if (regCDespachoXDia == null)
                {
                    continue;
                }

                List<DatoCDispatch> listaDatoX30min = regCDespachoXDia.ListaDatoX30min;

                ResultadoCDespachoDiario regDiario = new ResultadoCDespachoDiario();
                List<ResultadoValidacionAplicativo> listaMjsXDia = new List<ResultadoValidacionAplicativo>();

                #region insumos de cada día 

                List<MeMedicion48DTO> listaMW48 = regCDespachoXDia.ListaMe48XDiaMW;
                List<MePtomedicionDTO> listaPtoDespacho30minXDia = regCDespachoXDia.ListaPtoXDia;
                List<MePtomedicionDTO> listaPtoDespachoTermo30min = listaPtoDespacho30minXDia.Where(x => x.Famcodi == 3 || x.Famcodi == 5).ToList();

                List<EqEquipoDTO> listaEquiposOC = regCDespachoXDia.ListaEquipoOC;
                List<PrGrupoDTO> listaGrupoOC = regCDespachoXDia.ListaGrupoXDia;
                List<PrGrupoDTO> listaModoOC = regCDespachoXDia.ListaModoOC;

                List<EqEquipoDTO> listaEquiposAll = regCDespachoXDia.ListaAllEquipo;
                List<PrGrupoDTO> listaGrupoAll = regCDespachoXDia.ListaAllGrupo;

                List<EqEquipoDTO> lPotenciaByEquipo = UtilCdispatch.ListarPropiedadXUnidad(f1, listaEquiposOC, listaPropequi);

                #endregion

                foreach (var regPto in listaPtoDespacho30minXDia) //fria,rotantes,mwmantto
                {
                    regPto.ListaReservaFria = new List<MeMedicion48DTO>(); //para anexo A

                    if (regPto.Ptomedicodi == 219)
                    { }

                    DatoCDispatch regDatoXPto = new DatoCDispatch
                    {
                        Fecha = f1,
                        Ptomedicodi = regPto.Ptomedicodi,
                        Grupocodi = regPto.Grupocodi ?? 0,
                        Equicodi = regPto.Equicodi ?? 0,
                        Famcodi = regPto.Famcodi
                    };

                    DatoCDispatch regTmp = listaDatoX30min.Find(x => x.ListagrupocodiDesp.Contains(regDatoXPto.Grupocodi));

                    #region obtener datos para equipos no termicos

                    //Determinar fuente de energia
                    int fenergcodi = 0;

                    //información del despacho
                    MeMedicion48DTO regM48 = listaMW48.Where(x => x.Ptomedicodi == regPto.Ptomedicodi).FirstOrDefault();

                    //centrales por grupo
                    //List<EqEquipoDTO> listaCentralXGrupo = listaEquipos.Where(x => x.Grupocodi == reg.Grupocodi && (x.Famcodi == 4 || x.Famcodi == 5)).ToList();

                    //generadores por grupo
                    List<EqEquipoDTO> listaGenXGrupoOC = listaEquiposOC.Where(x => x.Grupocodi == regPto.Grupocodi && (x.Famcodi != 4 && x.Famcodi != 5))
                                                                        .OrderBy(x => x.Equipadre).ThenBy(x => x.Equinomb).ToList();

                    //centrales por grupo
                    List<EqEquipoDTO> listaCentralXGrupo = listaEquiposOC.Where(x => listaGenXGrupoOC.Select(y => y.Equipadre).Contains(x.Equicodi)).ToList();

                    //decimal? potEfectiva = GetPotenciaEfectivaDespacho(reg, listaUnidadTermo, listaPotEfectivaXModo, listaPotEfectivaXEquipo);
                    #region Verificación de potencias minimas y máximas. Determinar fuente de energia

                    if (regPto.Ptomedicodi == 147)
                    { }

                    List<int> listaFenergcodiXEq = new List<int>();
                    foreach (var regEq in listaGenXGrupoOC)
                    {
                        UtilCdispatch.GetPropEquipoCdispatch(regEq, lPotenciaByEquipo, out decimal? pmax, out decimal? pmin, out int fenergcodiEq, out string gruponombCS, out bool tieneModoOpDefecto, out bool esUnidadModoEspecial);

                        if (pmax.GetValueOrDefault(0) <= 0 && regEq.Famcodi != ConstantesHorasOperacion.IdGeneradorTemoelectrico)
                        {
                            var val = new ResultadoValidacionAplicativo()
                            {
                                TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                                Ptomedicodi = regPto.Ptomedicodi,
                                Ptomedielenomb = regPto.Ptomedielenomb,
                                Descripcion = string.Format("El equipo [{0}, {1}] no tiene potencia efectiva / máxima. {2}", regEq.Equicodi, regEq.Equinomb, (esUnidadModoEspecial ? "(Modo de operación especial)" : "")),
                                Emprnomb = regPto.Emprnomb
                            };
                            listaMjsXDia.Add(val);
                        }
                        if (regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico && !tieneModoOpDefecto)
                        {
                            var regGrupo = listaGrupoOC.Find(x => x.Grupocodi == regPto.Grupocodi);
                            var grupoDespachoDesc = regGrupo != null ? regGrupo.Gruponomb : regEq.Equinomb;
                            var val = new ResultadoValidacionAplicativo()
                            {
                                TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                                Ptomedicodi = regPto.Ptomedicodi,
                                Ptomedielenomb = regPto.Ptomedielenomb,
                                Descripcion = string.Format("El grupo [{0}, {1}] no tiene un modo de operación por defecto.", regPto.Grupocodi, grupoDespachoDesc),
                                Justificacion = "Es requerido el 'Tiempo mínimo de arranque' del modo de operación por defecto para determinar la media hora cual comienza la ReservaFríaTérmica.",
                                Emprnomb = regPto.Emprnomb
                            };
                            listaMjsXDia.Add(val);
                        }

                        listaFenergcodiXEq.Add(fenergcodiEq);
                    }

                    //Si no hay equipos para el grupo de despacho OC, entonces son equipos en proyecto o fuera de Coes
                    if (!listaGenXGrupoOC.Any())
                    {
                        listaFenergcodiXEq = new List<int>();

                        List<EqEquipoDTO> listaGenXGrupoAll = listaEquiposAll.Where(x => x.Grupocodi == regPto.Grupocodi)
                                                                            .OrderBy(x => x.Equipadre).ThenBy(x => x.Equinomb).ToList();

                        //considerar para los fuentes de energia a los de proyecto o fuera de Coes
                        List<EqEquipoDTO> listaGenXGrupoNoBaja = listaGenXGrupoAll.Where(x => x.Equiestado != "B").ToList();
                        foreach (var reg in listaGenXGrupoNoBaja)
                        {
                            listaFenergcodiXEq.Add(reg.Fenergcodi);
                        }

                        if (listaGenXGrupoAll.Any())
                        {
                            foreach (var regEq in listaGenXGrupoAll)
                            {
                                //verificar si estado del punto y el equipo/grupo son correctos
                                if (regPto.Ptomediestado == ConstantesAppServicio.Activo || regPto.Ptomediestado == ConstantesAppServicio.Proyecto)
                                {
                                    if (regEq.Equiestado != ConstantesAppServicio.Activo && regEq.Equiestado != ConstantesAppServicio.Proyecto && regEq.Equiestado != ConstantesAppServicio.FueraCOES)
                                    {
                                        var val = new ResultadoValidacionAplicativo()
                                        {
                                            TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                                            Ptomedicodi = regPto.Ptomedicodi,
                                            Ptomedielenomb = regPto.Ptomedielenomb ?? regPto.Gruponomb,
                                            Descripcion = string.Format("El punto de medición {2} [{0}, {1}] esta asociado a un equipo [{3}, {4}] que tiene estado {5}."
                                                                                        , regPto.Ptomedicodi, regPto.Ptomedielenomb, Util.EstadoDescripcion(regPto.Ptomediestado), regEq.Equicodi, regEq.Equinomb, Util.EstadoDescripcion(regEq.Equiestado)),
                                            Justificacion = "",
                                            Emprnomb = regPto.Emprnomb
                                        };
                                        listaMjsXDia.Add(val);
                                    }

                                    var regGrupo = listaGrupoAll.Find(x => x.Grupocodi == regPto.Grupocodi);
                                    if (regGrupo != null && regGrupo.GrupoEstado != null && regGrupo.GrupoEstado != ConstantesAppServicio.Activo && regGrupo.GrupoEstado != ConstantesAppServicio.Proyecto)
                                    {
                                        var val = new ResultadoValidacionAplicativo()
                                        {
                                            TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                                            Ptomedicodi = regPto.Ptomedicodi,
                                            Ptomedielenomb = regPto.Ptomedielenomb ?? regPto.Gruponomb,
                                            Descripcion = string.Format("El punto de medición {2} [{0}, {1}] esta asociado a un grupo [{3}, {4}] que tiene estado {5}."
                                                                                        , regPto.Ptomedicodi, regPto.Ptomedielenomb, Util.EstadoDescripcion(regPto.Ptomediestado), regGrupo.Gruponomb, regGrupo.Gruponomb, Util.EstadoDescripcion(regGrupo.GrupoEstado)),
                                            Justificacion = "",
                                            Emprnomb = regPto.Emprnomb
                                        };
                                        listaMjsXDia.Add(val);
                                    }
                                }
                            }
                        }

                        if (!listaFenergcodiXEq.Any())
                        {
                            var val = new ResultadoValidacionAplicativo()
                            {
                                TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                                Ptomedicodi = regPto.Ptomedicodi,
                                Ptomedielenomb = regPto.Ptomedielenomb ?? regPto.Gruponomb,
                                Descripcion = string.Format("El punto de medición [{0}, {1}] no tiene Fuente de energía por defecto por tener datos inconsistentes.", regPto.Ptomedicodi, (regPto.Ptomedielenomb ?? regPto.Gruponomb)),
                                Justificacion = "",
                                Emprnomb = regPto.Emprnomb
                            };
                            listaMjsXDia.Add(val);
                        }
                    }

                    fenergcodi = listaFenergcodiXEq.Any() ? listaFenergcodiXEq.OrderByDescending(x => x).First() : 0;

                    #endregion


                    #endregion

                    #region Anexo A

                    var regEqprop = GetPropiedadTiempoSincronizacion(f1, regPto.Equicodi ?? -1, listaPropequi);
                    decimal? valorSinc = regEqprop?.ValorDecimal;
                    string valorSincDesc = regEqprop != null ? regEqprop.ValorDesc : string.Empty;

                    #endregion

                    #region Cálculo por punto de medicion

                    MeMedicion48DTO mrfTotal = GetConstructorRF(ConstantesPR5ReportesServicio.TipoReservaFriaTotal, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, fenergcodi, 0);
                    MeMedicion48DTO mrfRapida = GetConstructorRF(ConstantesPR5ReportesServicio.TipoReservaFriaRapida, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, 0, 0);
                    mrfRapida.SincronizacionMin = valorSinc;
                    mrfRapida.SincronizacionTiempo = valorSincDesc;
                    MeMedicion48DTO mrfMinima = GetConstructorRF(ConstantesPR5ReportesServicio.TipoReservaFriaMinima, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, 0, 0);
                    MeMedicion48DTO mrfIndisp = GetConstructorRF(ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidad, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, 0, 0);
                    MeMedicion48DTO mMwMantto = GetConstructorRF(0, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, 0, 0);
                    MeMedicion48DTO mrRotante = GetConstructorRF(0, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, fenergcodi, 0);

                    MeMedicion48DTO mEfiGas = GetConstructorRF(0, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, 0, regPto.Ctgdetcodi);
                    MeMedicion48DTO mEfiCarbon = GetConstructorRF(0, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, fenergcodi, 0);

                    List<MeMedicion48DTO> listaRFriaXFenerg = new List<MeMedicion48DTO>();
                    if (regTmp != null)
                    {
                        foreach (var fenerg in regTmp.Unidad.ListaFuenteEnergia)
                        {
                            var objUnidadXFenerg = GetConstructorRF(0, f1, regPto.Grupocodi.GetValueOrDefault(0), regPto.Ptomedicodi, regPto.Emprcodi.Value, fenerg.Fenergcodi, 0);
                            objUnidadXFenerg.Emprnomb = regPto.Emprnomb;
                            objUnidadXFenerg.Fenergcodi = fenerg.Fenergcodi;
                            objUnidadXFenerg.Gruponomb = regPto.Gruponomb; //regTmp.Unidad.Central + " " + regTmp.Unidad.Equiabrev;
                            listaRFriaXFenerg.Add(objUnidadXFenerg);
                        }
                    }

                    for (int h = 1; h <= 48; h++)
                    {
                        DateTime fechaHora = f1.AddMinutes(h * 30);

                        Celda30minCDispatch regCelda30min = new Celda30minCDispatch
                        {
                            FechaHora = fechaHora
                        };

                        //obtener datos de Calculo de Termicas
                        if (regTmp != null)
                        {
                            Celda30minCDispatch regCalculo = regTmp.Array30Min[h - 1];

                            if (regCalculo != null)
                            {
                                var regCalculoDesp = regCalculo.ListaDatoDespachoDefault.Find(x => x.Grupocodi == regDatoXPto.Grupocodi);
                                if (regCalculoDesp != null)
                                {
                                    regCelda30min.TieneMantto = regCalculoDesp.TieneMantto;
                                    regCelda30min.TieneTminarranque = regCalculoDesp.TieneTminarranque;
                                    regCelda30min.TieneRfria = regCalculoDesp.TieneRfria;
                                    regCelda30min.TieneHOnoPotencia = regCalculoDesp.TieneHOnoPotencia;
                                    regCelda30min.Fenergcodi = regCalculoDesp.Fenergcodi;
                                }

                                regCelda30min.ListaMsj = regCalculo.ListaMsj;

                                bool esPintarDatosEnPto = false;
                                //si es ciclo combinado, visualizar el calculo en la TV
                                if (regTmp.Unidad.TieneCicloComb && regTmp.Unidad.EquicodiTVCicloComb > 0 && !regTmp.Unidad.EsUnidadModoEspecial)
                                {
                                    if (regDatoXPto.Equicodi == regTmp.Unidad.EquicodiTVCicloComb)
                                    {
                                        esPintarDatosEnPto = true;
                                    }
                                }
                                else
                                {
                                    //ciclo simple o especiales
                                    esPintarDatosEnPto = true;
                                }

                                if (esPintarDatosEnPto)
                                {
                                    mrRotante.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mrRotante, regCalculo.ValorMWRotante);
                                    mrfTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mrfTotal, regCalculo.ValorMWRfria);
                                    mMwMantto.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mMwMantto, regCalculo.ValorMWMantto);

                                    //Valor eficiente
                                    GetValorEficienteXH(regCalculo, out decimal efiGas, out decimal efiCarbon);
                                    mEfiGas.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mEfiGas, efiGas);
                                    mEfiCarbon.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mEfiCarbon, efiCarbon);

                                    //Valor reserva fria por fuente de energi
                                    GetValorRfriaDespachoXH(regCalculo, h, ref listaRFriaXFenerg);

                                    //Anexo A
                                    if (regTmp.Unidad.Gruporeservafria == ConstantesAppServicio.GrupoReservaFria)
                                    {
                                        mrfMinima.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(mrfMinima, regCalculo.ValorMWRfria);
                                    }

                                    mrfIndisp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(mrfIndisp, regCalculo.ValorMWMantto);

                                    if (valorSinc == null || valorSinc <= ConstantesPR5ReportesServicio.TiempoSincRapidoMin)//6horas
                                    {
                                        mrfRapida.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(mrfRapida, regCalculo.ValorMWRfria);
                                    }
                                }
                            }
                        }
                        else
                        {
                            regCelda30min.Fenergcodi = fenergcodi;
                        }

                        regDatoXPto.Array30Min[h - 1] = regCelda30min;
                    }

                    regDiario.ListaCelda.Add(regDatoXPto);

                    #endregion

                    #region totalizar por m48

                    SetMeditotalXLista(mrfTotal, new List<MeMedicion48DTO>() { });
                    SetMeditotalXLista(mrfRapida, new List<MeMedicion48DTO>() { });
                    SetMeditotalXLista(mrfMinima, new List<MeMedicion48DTO>() { });
                    SetMeditotalXLista(mrfIndisp, new List<MeMedicion48DTO>() { });
                    SetMeditotalXLista(mMwMantto, new List<MeMedicion48DTO>() { });
                    SetMeditotalXLista(mrRotante, new List<MeMedicion48DTO>() { });

                    #endregion

                    regPto.ListaReservaFria.Add(mrfTotal);
                    regPto.ListaReservaFria.Add(mrfRapida);
                    regPto.ListaReservaFria.Add(mrfMinima);
                    regPto.ListaReservaFria.Add(mrfIndisp);

                    listaRFDetalleTotal.Add(mrfTotal);
                    listaRFXFenerg.AddRange(listaRFriaXFenerg);
                    listaRFRapida.Add(mrfRapida);
                    listaRFMinima.Add(mrfMinima);
                    listaRFIndisp.Add(mrfIndisp);

                    listaMMDetalle.Add(mMwMantto);
                    listaRRDetalle.Add(mrRotante);

                    listaREfiGasDetalle.Add(mEfiGas);
                    listaREfiCarbonDetalle.Add(mEfiCarbon);
                }

                #region Totales

                List<int> listaPtomedicodiTermo = listaPtoDespachoTermo30min.Select(x => x.Ptomedicodi).Distinct().ToList();

                listaRFDetalleTotal = listaRFDetalleTotal.Where(x => listaPtomedicodiTermo.Contains(x.Ptomedicodi)).ToList();
                listaRFXFenerg = listaRFXFenerg.Where(x => listaPtomedicodiTermo.Contains(x.Ptomedicodi)).ToList();
                listaRFRapida = listaRFRapida.Where(x => listaPtomedicodiTermo.Contains(x.Ptomedicodi)).ToList();
                listaRFMinima = listaRFMinima.Where(x => listaPtomedicodiTermo.Contains(x.Ptomedicodi)).ToList();
                listaRFIndisp = listaRFIndisp.Where(x => listaPtomedicodiTermo.Contains(x.Ptomedicodi)).ToList();

                listaMMDetalle = listaMMDetalle.Where(x => listaPtomedicodiTermo.Contains(x.Ptomedicodi)).ToList();

                #region Generar Totales por Tipo de Reserva Fria

                MeMedicion48DTO regTotalRF = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Lectcodi = lectcodi,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvFriaTermica,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    TipoReservaFria = ConstantesPR5ReportesServicio.TipoReservaFriaTotal
                };

                MeMedicion48DTO regTotalRFRapida = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Ptomedicodi = ConstantesAppServicio.AnexoAPtomedicodiRsvFriaRapida,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    TipoReservaFria = ConstantesPR5ReportesServicio.TipoReservaFriaRapida
                };

                MeMedicion48DTO regTotalRFMinima = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    TipoReservaFria = ConstantesPR5ReportesServicio.TipoReservaFriaMinima
                };

                MeMedicion48DTO regTotalRFIndisp = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Ptomedicodi = ConstantesAppServicio.AnexoAPtomedicodiRsvFriaxMto,
                    TipoReservaFria = ConstantesPR5ReportesServicio.TipoReservaFriaIndisponibilidad
                };

                MeMedicion48DTO regTotalMM = new MeMedicion48DTO
                {
                    Medifecha = f1
                };

                MeMedicion48DTO regTotalRR = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Lectcodi = lectcodi,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvRotante,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda
                };

                MeMedicion48DTO regTotalEfi = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Lectcodi = lectcodi,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvEficiente,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda
                };

                MeMedicion48DTO regTotalEfiGas = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Lectcodi = lectcodi,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvEficienteGas,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiGas
                };

                MeMedicion48DTO regTotalEfiCarbon = new MeMedicion48DTO
                {
                    Medifecha = f1,
                    Lectcodi = lectcodi,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvEficienteCarbon,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Fenergcodi = ConstantesPR5ReportesServicio.FenergcodiCarbon
                };

                //
                SetMeditotalXLista(regTotalRF, listaRFDetalleTotal.Where(x => x.Medifecha == f1).ToList());
                SetMeditotalXLista(regTotalRFRapida, listaRFRapida.Where(x => x.Medifecha == f1).ToList());
                SetMeditotalXLista(regTotalRFMinima, listaRFMinima.Where(x => x.Medifecha == f1).ToList());
                SetMeditotalXLista(regTotalRFIndisp, listaRFIndisp.Where(x => x.Medifecha == f1).ToList());
                SetMeditotalXLista(regTotalRR, listaRRDetalle.Where(x => x.Medifecha == f1).ToList());

                //Reserva Eficiente Gas
                SetMeditotalXLista(regTotalEfiGas, listaREfiGasDetalle.Where(x => x.Medifecha == f1).ToList());
                //Reserva Eficiente Carbon
                SetMeditotalXLista(regTotalEfiCarbon, listaREfiCarbonDetalle.Where(x => x.Medifecha == f1).ToList());

                //Reserva Eficiente
                SetMeditotalXLista(regTotalEfi, new List<MeMedicion48DTO>() { regTotalEfiGas, regTotalEfiCarbon });

                SetMeditotalXLista(regTotalMM, listaMMDetalle.Where(x => x.Medifecha == f1).ToList());

                #endregion

                listaRFTotal.Add(regTotalRF);

                listaDataTotal.Add(regTotalRF);
                listaDataTotal.Add(regTotalRFRapida);
                listaDataTotal.Add(regTotalRFMinima);
                listaDataTotal.Add(regTotalRFIndisp);

                listaREficiente.Add(regTotalEfi);
                listaREficiente.Add(regTotalEfiGas);
                listaREficiente.Add(regTotalEfiCarbon);

                listaMMTotal.Add(regTotalMM);
                listaRRTotal.Add(regTotalRR);

                //
                regDiario.Fecha = f1;
                regDiario.RFTotalXDia = regTotalRF;
                regDiario.ListaRFDetalleXDia = listaRFDetalleTotal.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaRFXFenergXDia = listaRFXFenerg.Where(x => x.Medifecha == f1).ToList(); ;
                regDiario.ListaRFDataTotalXDia = listaDataTotal.Where(x => x.Medifecha == f1).ToList();
                regDiario.RRTotalXDia = listaRRTotal.Find(x => x.Medifecha == f1);
                regDiario.ListaRRDetalleXDia = listaRRDetalle.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaMMTotalXDia = listaMMTotal.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaMMDetalleXDia = listaMMDetalle.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaREficienteXDia = listaREficiente.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaREfiGasXDia = listaREfiGasDetalle.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaREfiCarbonXDia = listaREfiCarbonDetalle.Where(x => x.Medifecha == f1).ToList();
                regDiario.ListaMensajeValidacionXDia = listaMjsXDia;

                reporteRFriaOut.ListaResultado.Add(regDiario);
                listaMensajeValidacion.AddRange(regDiario.ListaMensajeValidacionXDia);

                #endregion
            }

            #region Resultado

            reporteRFriaOut.ListaPto = listaPtoDespacho30min;

            reporteRFriaOut.ListaRFDataTotal = listaDataTotal;
            reporteRFriaOut.ListaRFTotal = listaRFTotal;
            reporteRFriaOut.ListaRFDetalle = listaRFDetalleTotal;
            reporteRFriaOut.ListaRFXFenerg = listaRFXFenerg;

            reporteRFriaOut.ListaRRTotal = listaRRTotal;
            reporteRFriaOut.ListaRRDetalle = listaRRDetalle;

            reporteRFriaOut.ListaMMTotal = listaMMTotal;
            reporteRFriaOut.ListaMMDetalle = listaMMDetalle;

            reporteRFriaOut.ListaREficiente = listaREficiente;
            reporteRFriaOut.ListaREfiGasDetalle = listaREfiGasDetalle;
            reporteRFriaOut.ListaREfiCarbonDetalle = listaREfiCarbonDetalle;

            reporteRFriaOut.ListaMensajeValidacion = new List<ResultadoValidacionAplicativo>();

            //solo considerar los puntos que no son proyectos
            var listaPtocodiParamensaje = listaPtoDespacho30min.Where(x => x.Ptomediestado != ConstantesAppServicio.Proyecto).Select(x => x.Ptomedicodi).ToList();
            listaMensajeValidacion = listaMensajeValidacion.Where(x => listaPtocodiParamensaje.Contains(x.Ptomedicodi)).ToList();

            foreach (var msjxPto in listaMensajeValidacion.Where(x => x.TipoValidacion == ConstantesPR5ReportesServicio.MensajeError).OrderBy(x => x.Emprnomb).GroupBy(x => new { x.Emprnomb }))
            {
                var listaMsj = msjxPto.Select(x => x.Descripcion + (x.Justificacion ?? "")).Distinct();
                var regVal = new ResultadoValidacionAplicativo()
                {
                    TipoValidacion = msjxPto.First().TipoValidacion,
                    Emprnomb = msjxPto.First().Emprnomb,
                    ListaMensaje = listaMsj.OrderBy(x => x).ToList(),
                };
                reporteRFriaOut.ListaMensajeValidacion.Add(regVal);
            }

            foreach (var msjxPto in listaMensajeValidacion.Where(x => x.TipoValidacion == ConstantesPR5ReportesServicio.MensajeAlerta).GroupBy(x => new { x.Ptomedicodi }))
            {
                var regVal = new ResultadoValidacionAplicativo()
                {
                    Ptomedicodi = msjxPto.Key.Ptomedicodi,
                    TipoValidacion = msjxPto.First().TipoValidacion,
                    Ptomedielenomb = msjxPto.First().Ptomedielenomb,
                    Emprnomb = msjxPto.First().Emprnomb,
                    ListaMensaje = msjxPto.Select(x => x.Descripcion).Distinct().ToList()
                };
                reporteRFriaOut.ListaMensajeValidacion.Add(regVal);
            }

            #endregion
        }

        /// <summary>
        /// Generar Datos cada 30min por unidad de generación (equipo y modo de operación)
        /// </summary>
        /// <param name="regCDespachoXDia"></param>
        /// <returns></returns>
        public static List<DatoCDispatch> GenerarDatos30min(CDespachoDiario regCDespachoXDia)
        {
            //1. Determinar los equipos E/S (Equipos E/S, Modos de operación E/S con HO, Modos de operación E/S sin HO)
            //2. Equipos indisponibles por mantenimientos / intervenciones
            List<DatoCDispatch> listaUnidad = ListarUnidad30min(regCDespachoXDia);

            //2.1 Modos de operación E/S, con arranque, con parada
            GenerarDatos30minCostoOperacion(listaUnidad, regCDespachoXDia);

            //3. Equipos indisponibles por tiempo mínimo de arranque
            GenerarDatos30minTminArranque(listaUnidad, regCDespachoXDia);

            //4. Casos especiales de C.T. AGUAYTIA, C.T. MALACAS 1, C.T. MALACAS 2, C.T. RF DE GENERACION TALARA
            GenerarReservaEficienteGasEstimada(listaUnidad, regCDespachoXDia);

            //5. Reserva fría y MwMantto por fuente de energia
            RealizarCalculoXFenergCDispatch(listaUnidad, regCDespachoXDia);

            //6. Datos informativos para excel y web
            GenerarDatosDefaultReporteWeb(listaUnidad, regCDespachoXDia.ListaDespachoOC, regCDespachoXDia.ListaModoOC);
            GenerarDetalleInformativoXDato30Min(listaUnidad, regCDespachoXDia);

            return listaUnidad;
        }

        private static void GenerarDatosDefaultReporteWeb(List<DatoCDispatch> listaUnidad, List<PrGrupoDTO> listaDespachoOC, List<PrGrupoDTO> listaModoOC)
        {
            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.Unidad.Equicodi == 34)
                { }
                if (regUnidad.ListagrupocodiDesp.Contains(147))
                { }

                for (int h = 1; h <= 48; h++)
                {
                    Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];

                    if (h == 15)
                    { }

                    foreach (var grupocodi in regH.ListaGrupocodiDespachoAll)
                    {
                        bool tieneHoNoPot = false;
                        int fenergcodi = 0; //por defecto

                        var grDesp = listaDespachoOC.Find(x => x.Grupocodi == grupocodi);
                        List<int> lEquicodi = grDesp.ListaEquicodi;

                        //obtener datos de las horas de operacion
                        var regDespOp = regH.ListaDatoDespachoOperativo.Find(x => x.Grupocodi == grupocodi);
                        if (regDespOp != null)
                        {
                            fenergcodi = regUnidad.Unidad.Fenergcodi;

                            //equipos del grupo de despacho
                            var listaEqDespOp = regH.ListaDatoGeneradorOperativo.Where(x => lEquicodi.Contains(x.Equicodi)).OrderBy(x => x.ValorMW).ToList();
                            if (listaEqDespOp.Any())
                            {
                                var grModo = listaModoOC.Find(x => x.Grupocodi == listaEqDespOp.First().Grupocodi);
                                if (grModo != null) fenergcodi = grModo.Fenergcodi ?? 0;
                                if (listaEqDespOp.First().Subcausacodi > 0)
                                    tieneHoNoPot = listaEqDespOp.First().Subcausacodi != ConstantesSubcausaEvento.SubcausaPorPotenciaEnergia;
                            }
                        }
                        else
                        {
                            //obtener datos de reserva fría
                            foreach (var equicodi in lEquicodi)
                            {
                                var lfria = regH.ListaDatoRfriaXFenerg.Where(x => x.LEquipoIndispXFenerg.Contains(equicodi)).ToList();
                                if (lfria.Any())
                                {
                                    fenergcodi = lfria.First().Fenergcodi;
                                }

                                //especiales
                                var regFria = regH.ListaDatoRfria.Find(x => x.Equicodi == equicodi);
                                if (regFria != null)
                                {
                                    var grModo = listaModoOC.Find(x => x.Grupocodi == regFria.Grupocodi);
                                    if (grModo != null) fenergcodi = grModo.Fenergcodi ?? 0;
                                }
                            }

                            //fenerg de los manttos
                            if (fenergcodi <= 0)
                            {
                                foreach (var equicodi in lEquicodi)
                                {
                                    var lfria = regH.ListaDatoMWManttoXFenerg.Where(x => x.LEquipoIndispXFenerg.Contains(equicodi)).ToList();
                                    if (lfria.Any())
                                    {
                                        fenergcodi = lfria.First().Fenergcodi;
                                    }

                                    //especiales
                                    var regFria = regH.ListaDatoMWMantto.Find(x => x.Equicodi == equicodi);
                                    if (regFria != null)
                                    {
                                        var grModo = listaModoOC.Find(x => x.Grupocodi == regFria.Grupocodi);
                                        if (grModo != null) fenergcodi = grModo.Fenergcodi ?? 0;
                                    }
                                }
                            }

                            //fenerg de tiempo minimo de arranque
                            if (fenergcodi <= 0)
                            {
                                //equipos del grupo de despacho
                                var listaEqDisp = regH.ListaDatoIndisponibilidad.Where(x => lEquicodi.Contains(x.Equicodi) && x.TieneTminarranque && x.Grupocodi > 0).ToList();
                                if (listaEqDisp.Any())
                                {
                                    var grModo = listaModoOC.Find(x => x.Grupocodi == listaEqDisp.First().Grupocodi);
                                    if (grModo != null) fenergcodi = grModo.Fenergcodi ?? 0;
                                }
                            }
                        }

                        bool tieneMantto = regH.ListaEquicodiMantto.Any(x => lEquicodi.Contains(x)) || regH.ListaEquicodiManttoGas.Any(x => lEquicodi.Contains(x));
                        bool tieneTmin = regH.ListaEquicodiTmin.Any(x => lEquicodi.Contains(x));
                        bool tieneRfria = regH.ListaEquicodiCandidatoRfria.Any(x => lEquicodi.Contains(x));

                        regH.ListaDatoDespachoDefault.Add(new DatoCalculoHorario()
                        {
                            Grupocodi = grupocodi,
                            TieneHOnoPotencia = tieneHoNoPot,
                            Fenergcodi = fenergcodi,
                            TieneMantto = tieneMantto,
                            TieneTminarranque = tieneTmin,
                            TieneRfria = tieneRfria,
                        });
                    }
                }
            }
        }

        private static List<DatoCDispatch> ListarUnidad30min(CDespachoDiario regCDespachoXDia)
        {
            List<DatoCDispatch> listaUnidad = new List<DatoCDispatch>();

            foreach (var unidad in regCDespachoXDia.ListaUnidadOC)
            {
                DatoCDispatch regUnidad = new DatoCDispatch
                {
                    Unidad = unidad
                };

                PrGrupoDTO regModo = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == unidad.Grupocodi);
                regUnidad.ListagrupocodiDesp = regModo.ListaGrupocodiDespacho;

                if (regUnidad.Unidad.Equicodi == 13298)
                { }
                if (regUnidad.ListagrupocodiDesp.Contains(193))
                { }

                for (int h = 1; h <= 48; h++)
                {
                    Celda30minCDispatch regH = new Celda30minCDispatch
                    {
                        FechaHora = regCDespachoXDia.Fecha.AddMinutes(h * 30),
                        ListaEquicodiAll = unidad.ListaEquicodi,
                        ListaGrupocodiDespachoAll = regModo.ListaGrupocodiDespacho,
                        Fenergcodi = regModo.Fenergcodi ?? 0
                    };

                    //obtener datos operativos
                    ListarDatosOperativo(h, unidad, regH, regCDespachoXDia);

                    List<int> listaEquicodiNoOperativo = regH.ListaEquicodiAll.Where(x => !regH.ListaEquicodiOperativo.Contains(x)).ToList();

                    //obtener equipos indisponibles por mantenimientos
                    ListarEquiposIndispXMantto(h, listaEquicodiNoOperativo, regH, regCDespachoXDia);

                    regUnidad.Array30Min[h - 1] = regH;
                }

                listaUnidad.Add(regUnidad);
            }

            return listaUnidad;
        }

        private static void GenerarDatos30minCostoOperacion(List<DatoCDispatch> listaUnidad, CDespachoDiario regCDespachoXDia)
        {
            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.Unidad.Equicodi == 34)
                { }
                if (regUnidad.ListagrupocodiDesp.Contains(3402))
                { }

                for (int h = 1; h <= 48; h++)
                {
                    if (h == 39)
                    { }

                    //datos de la media hora
                    Celda30minCDispatch regHActual = regUnidad.Array30Min[h - 1];
                    Celda30minCDispatch regHAnterior = h > 1 ? regUnidad.Array30Min[h - 2] : null;

                    if (regHActual != null)
                    {
                        if (h > 1 && regHAnterior != null)
                        {
                            //verificar arranque
                            ListarModoArranqueParadaX30min(regHAnterior, regHActual, regCDespachoXDia.ListaModoOC,
                                            out List<int> listaGrupocodiModoArranque, out List<int> listaGrupocodiModoParada);

                            if (listaGrupocodiModoArranque.Any())
                            {
                                regHActual.TieneArranqueCO = true;
                                regHActual.ListaGrupocodiModoArranque = listaGrupocodiModoArranque;
                            }

                            //verificar parada
                            if (listaGrupocodiModoParada.Any())
                            {
                                regHActual.TieneParadaCO = true;
                                regHActual.ListaGrupocodiModoParada = listaGrupocodiModoParada;
                            }
                        }

                        //Costo de arranque
                        foreach (var codigoModoOp30min in regHActual.ListaGrupocodiModoArranque)
                        {
                            //costo variable
                            PrCvariablesDTO regCV = regCDespachoXDia.ListaCostoVariableDia.Find(x => x.Grupocodi == codigoModoOp30min);
                            if (regCV != null)
                            {
                                regHActual.TCambio = regCV.TCambio;

                                //ccbef
                                regHActual.Ccbef += (regCV.Cbe * regCV.Ccomb.GetValueOrDefault(0)) + (regCV.CbeAlt * regCV.CcombAlt);

                                //CArrUS
                                regHActual.CArrCO += regCV.CMarrPar;

                                //valores temporales
                                regHActual.CbeTmpCO += regCV.Cbe;
                                regHActual.CcombTmpCO += regCV.Ccomb.GetValueOrDefault(0);
                                regHActual.CbeAltTmpCO += regCV.CbeAlt;
                                regHActual.CcombAltTmpCO += regCV.CcombAlt;
                            }
                        }

                        //costo del consumo del combustible
                        if (regHActual.ValorMWActiva > 0)
                        {
                            //modo de operación de la media hora actual (puede haber varios cuando hay ciclo simple y combinado en una central)
                            foreach (var codigoModoOp30min in regHActual.ListaGrupocodiModoOperativo)
                            {
                                var regModoOpero = regHActual.ListaDatoModoOperativo.Find(x => x.Grupocodi == codigoModoOp30min);
                                if (regModoOpero != null && regModoOpero.ValorMWCVC > 0)
                                {
                                    decimal potenciaCVC = regModoOpero.ValorMWCVC;
                                    decimal energiaCNVC = regModoOpero.ValorMWCNVC / 2.0m;

                                    //consumo
                                    ConsumoHorarioCombustible regCurva = regCDespachoXDia.ListaCurvaXDia.Find(x => x.Grupocodi == codigoModoOp30min);
                                    var consumoCombCO = GetConsumoPorMediaHora(regCurva, potenciaCVC);

                                    //costo variable
                                    PrCvariablesDTO regCV = regCDespachoXDia.ListaCostoVariableDia.Find(x => x.Grupocodi == codigoModoOp30min);
                                    if (regCV != null)
                                    {
                                        regHActual.TCambio = regCV.TCambio;

                                        //costo combustible en dolares
                                        regHActual.CostoCombCO += regCV.Ccomb.GetValueOrDefault(0);

                                        //CVNC
                                        regHActual.CvncCO += (energiaCNVC * regCV.CvncUS * 1000.0m);

                                        //CVC
                                        regHActual.CvcCO += (consumoCombCO * regCV.Ccomb.GetValueOrDefault(0));

                                        //valores temporales
                                        regHActual.EnergiaTmpCO += energiaCNVC;
                                        regHActual.ConsumoCombTmpCO += consumoCombCO;
                                    }
                                }
                            }
                        }

                        //Suma de costos
                        decimal costoMediaHora = regHActual.CvncCO + regHActual.CvcCO + regHActual.Ccbef + regHActual.CArrCO;
                        regHActual.CostoTotalDolares = costoMediaHora;
                        regHActual.CostoTotalSoles = costoMediaHora * regHActual.TCambio;
                    }
                }
            }
        }

        private static void ListarModoArranqueParadaX30min(Celda30minCDispatch regHAnterior, Celda30minCDispatch regHActual, List<PrGrupoDTO> listaModoOC,
                                    out List<int> listaGrupocodiModoArranque, out List<int> listaGrupocodiModoParada)
        {
            //1. Determinar los  grupos despacho que arrancan o paran
            List<int> listaGrupocodiDespAnt = regHAnterior.ListaGrupocodiDespachoOperativo;
            List<int> listaGrupocodiDespAct = regHActual.ListaGrupocodiDespachoOperativo;

            List<int> listaArranqueDesp = listaGrupocodiDespAct.Except(listaGrupocodiDespAnt).ToList();
            List<int> listaParadaDesp = listaGrupocodiDespAnt.Except(listaGrupocodiDespAct).ToList();

            //2. de los despacho obtener los modos de operación de la media hora actual
            listaGrupocodiModoArranque = new List<int>();
            foreach (var grupocodi in regHActual.ListaGrupocodiModoOperativo)
            {
                var regModo = listaModoOC.Find(x => x.Grupocodi == grupocodi);
                if (regModo != null)
                {
                    if (regModo.ListaGrupocodiDespacho.Any(x => listaArranqueDesp.Contains(x)))
                        listaGrupocodiModoArranque.Add(grupocodi);
                }
            }

            //2. de los despacho obtener los modos de operación de la media hora anterior
            listaGrupocodiModoParada = new List<int>();
            foreach (var grupocodi in regHAnterior.ListaGrupocodiModoOperativo)
            {
                var regModo = listaModoOC.Find(x => x.Grupocodi == grupocodi);
                if (regModo != null)
                {
                    if (regModo.ListaGrupocodiDespacho.Any(x => listaParadaDesp.Contains(x)))
                        listaGrupocodiModoParada.Add(grupocodi);
                }
            }
        }

        private static void GenerarDatos30minTminArranque(List<DatoCDispatch> listaUnidad, CDespachoDiario regCDespachoXDia)
        {
            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.Unidad.Equicodi == 34)
                { }
                if (regUnidad.ListagrupocodiDesp.Contains(451))
                { }

                for (int h = 1; h <= 48; h++)
                {
                    Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];

                    if (h == 15)
                    { }

                    List<int> listaEquicodiNoOperativo = regH.ListaEquicodiAll.Where(x => !regH.ListaEquicodiOperativo.Contains(x)).ToList();
                    listaEquicodiNoOperativo = listaEquicodiNoOperativo.Where(x => !regH.ListaEquicodiMantto.Contains(x)).ToList();

                    //obtener equipos indisponibles por tminarranque
                    ListarEquipoIndispXTminarranq(h, regH, regCDespachoXDia.Fecha, listaEquicodiNoOperativo, regUnidad.Array30Min, regCDespachoXDia.ListaHOXDiaAyer, regCDespachoXDia.ListaModoOC);

                    //unidades que quedan son de reserva fria
                    regH.ListaEquicodiCandidatoRfria = listaEquicodiNoOperativo.Where(x => !regH.ListaEquicodiTmin.Contains(x)).ToList();
                }
            }
        }

        private static void GenerarReservaEficienteGasEstimada(List<DatoCDispatch> listaUnidad, CDespachoDiario regCDespachoXDia)
        {
            //1. Por cada gaseoducto obtener el consumo utilizado de sus unidades
            foreach (var regGaseoducto in regCDespachoXDia.ListaDispCombustible)
            {
                //Verificar los grupos despacho tengan operación comercial en la fecha de consulta
                foreach (var grupocodiDesp in regGaseoducto.ListaGrupoDespachoOrden)
                {
                    var regUnidad = listaUnidad.Find(x => x.ListagrupocodiDesp.Contains(grupocodiDesp));
                    if (regUnidad != null)
                    {
                        for (int h = 1; h <= 48; h++)
                        {
                            Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];

                            //inicializar caso especial
                            if (regH != null)
                            {
                                regH.TieneCasoEspecialRfria = true;
                                regH.ValorMWRfriaGasEspecial = 0;
                            }

                            if (regH != null && regH.ValorMWActiva > 0)
                            {
                                //normalmente la lista es vacia o tiene 1 modo
                                foreach (var grupocodiModo in regH.ListaGrupocodiModoOperativo)
                                {
                                    //datos de consumo
                                    var regCurva = regCDespachoXDia.ListaCurvaXDia.Find(x => x.Grupocodi == grupocodiModo);
                                    if (regCurva != null && regCurva.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
                                    {
                                        //consumo horario
                                        decimal valorConsumoH = (regH.ValorMWActiva * decimal.Round(regCurva.PendienteM01, 4) + decimal.Round(regCurva.CoeficienteIndependiente, 4));

                                        //convertir consumo horario a 30 minutos
                                        decimal valorConsumo30min = valorConsumoH / (60.0m / 30.0m);

                                        //sumatoria de todas las unidades
                                        regGaseoducto.Consumo30min[h - 1] += valorConsumo30min;

                                        //se asumen que si hay potencia activa entonces se está utilizando el gaseoducto
                                        regGaseoducto.GrupocodiConsumo30min[h - 1] = grupocodiDesp;
                                    }
                                }
                            }
                        }
                    }
                }

                //totalizado
                regGaseoducto.CombustibleGasStockConsumido = regGaseoducto.Consumo30min.Sum(x => x);
                if (regGaseoducto.CombustibleGasStockInicial > regGaseoducto.CombustibleGasStockConsumido)
                    regGaseoducto.CombustibleGasStockDisponible = regGaseoducto.CombustibleGasStockInicial - regGaseoducto.CombustibleGasStockConsumido;
            }

            //2. Distribuir combustible a las unidades en reserva fría
            foreach (var regGaseoducto in regCDespachoXDia.ListaDispCombustible)
            {
                decimal consumoDisponible = regGaseoducto.CombustibleGasStockDisponible;

                //Verificar los grupos despacho tengan operación comercial en la fecha de consulta
                foreach (var grupocodiDesp in regGaseoducto.ListaGrupoDespachoOrden)
                {
                    var regUnidad = listaUnidad.Find(x => x.ListagrupocodiDesp.Contains(grupocodiDesp));
                    if (regUnidad != null)
                    {
                        //si hay dualidad, solo procesar Gas
                        var regFenergGas = regUnidad.Unidad.ListaFuenteEnergia.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas);
                        if (regFenergGas != null)
                        {
                            var regModo = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == regFenergGas.GrupocodiMaxPe);
                            var pe = (regModo?.Potencia) ?? 0;
                            var pmin = (regModo?.PotenciaMinima) ?? 0;

                            var regCurva = regCDespachoXDia.ListaCurvaXDia.Find(x => x.Grupocodi == regFenergGas.GrupocodiMaxPe);
                            if (regCurva != null && pe > 0 && pmin > 0)
                            {
                                //contar medias horas con reserva fría
                                int numMediaHoraRfria = 0;
                                List<int> listaHRfria = new List<int>();

                                for (int h = 1; h <= 48; h++)
                                {
                                    Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];

                                    if (regH != null)
                                    {
                                        //calculo preliminar de reserva fria
                                        DistribuirEquipoIndispXFenerg(regH, regUnidad, out List<DatoCalculoHorario> listaDatoRFriaTmp, out List<DatoCalculoHorario> listaDatoMwManttoTmp);

                                        foreach (var regRfria in listaDatoRFriaTmp)
                                        {
                                            //en reserva fría diesel o gas
                                            if (regRfria.LEquipoIndispXFenerg.Any())
                                            {
                                                numMediaHoraRfria++;
                                                listaHRfria.Add(h);
                                            }
                                        }
                                    }
                                }

                                //determinar la potencia para cada media hora y asignar
                                if (numMediaHoraRfria > 0)
                                {
                                    DistribuirCombustibleEnPeriodos(consumoDisponible, numMediaHoraRfria, regCurva, pe, pmin,
                                            out decimal consumoAUtilizarDespacho, out decimal potenciaAUtilizar, out int numMediaHoraAUtilizar);

                                    if (numMediaHoraAUtilizar > 0)
                                    {
                                        consumoDisponible -= consumoAUtilizarDespacho;

                                        for (int i = 0; i < numMediaHoraAUtilizar; i++)
                                        {
                                            int h = listaHRfria[i];

                                            //solo puede consumir una unidad cada 30min
                                            if (regGaseoducto.GrupocodiConsumo30min[h - 1] == 0)
                                            {
                                                Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];
                                                regH.ValorMWRfriaGasEspecial = potenciaAUtilizar;

                                                //asignar código para que ya no pueda ser usado por otro
                                                regGaseoducto.GrupocodiConsumo30min[h - 1] = grupocodiDesp;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //actualizar consumo para el siguiente grupo despacho
                regGaseoducto.CombustibleGasStockDisponible = consumoDisponible;
            }
        }

        private static void RealizarCalculoXFenergCDispatch(List<DatoCDispatch> listaUnidad, CDespachoDiario regCDespachoXDia)
        {
            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.Unidad.Equipadre == 608)
                { }
                if (regUnidad.ListagrupocodiDesp.Contains(671))
                { }

                for (int h = 1; h <= 48; h++)
                {
                    if (h == 18)
                    { }
                    Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];

                    //Datos E/S
                    RealizarCalculoDatosOperativos(regH, regCDespachoXDia.ListaModoOC);

                    //Datos Rotante E/S
                    RealizarCalculoRRotanteXUnidadYH(regUnidad, regH, regCDespachoXDia.ListaModoOC, regCDespachoXDia.ListaUnidadEspOC);

                    //MWxmantto F/S y Reserva fria F/S
                    RealizarCalculoMWxmanttoYRFriaXUnidadYH(regUnidad, regH, regCDespachoXDia.ListaModoOC, regCDespachoXDia.ListaUnidadEspOC);
                }
            }
        }

        private static void RealizarCalculoDatosOperativos(Celda30minCDispatch regH, List<PrGrupoDTO> listaModoOC)
        {
            List<PrGrupoDTO> listaModoOperativo = listaModoOC.Where(x => regH.ListaGrupocodiModoOperativo.Contains(x.Grupocodi)).ToList();

            //obtener las fuentes segun los modos
            List<int> lFenergOperativo = listaModoOperativo.Select(x => x.Fenergcodi ?? 0).OrderBy(x => x).Distinct().ToList();

            //1. iterar cada fuente para obtener los grupos despacho, modos y equipos operativos
            List<int> lGrupocodiDespUsado = new List<int>();
            List<int> lEquicodiUsado = new List<int>();
            foreach (var fenergcodi in lFenergOperativo)
            {
                var lModoXFenerg = listaModoOperativo.Where(x => x.Fenergcodi == fenergcodi).ToList();

                List<int> lGrupocodiDesp = new List<int>(); //grupos despacho del modo
                List<int> lEquicodiDesp = new List<int>(); //equipos del modo
                foreach (var obj in lModoXFenerg) lGrupocodiDesp.AddRange(obj.ListaGrupocodiDespacho);
                foreach (var obj in lModoXFenerg) lEquicodiDesp.AddRange(obj.ListaEquicodi);

                var lDespachoXFenerg = regH.ListaGrupocodiDespachoOperativo.Where(x => lGrupocodiDesp.Contains(x)).ToList();
                lDespachoXFenerg = lDespachoXFenerg.Where(x => !lGrupocodiDespUsado.Contains(x)).ToList();

                var lEquipoXFenerg = regH.ListaEquicodiOperativo.Where(x => lEquicodiDesp.Contains(x)).ToList();
                lEquipoXFenerg = lEquipoXFenerg.Where(x => !lEquicodiUsado.Contains(x)).ToList();

                //Agregar grupo despacho y fuente de energia
                regH.ListaDatoOperativoXFenerg.Add(new DatoCalculoHorario()
                {
                    Fenergcodi = fenergcodi,
                    LGrupoModoXFenerg = lModoXFenerg.Select(x => x.Grupocodi).ToList(),
                    LGrupoDespXFenerg = lDespachoXFenerg,
                    LEquipoXFenerg = lEquipoXFenerg,
                });

                lGrupocodiDespUsado.AddRange(lGrupocodiDesp);
                lEquicodiUsado.AddRange(lEquicodiDesp);
            }
        }

        private static void RealizarCalculoRRotanteXUnidadYH(DatoCDispatch regUnidad, Celda30minCDispatch regH, List<PrGrupoDTO> listaModoOC, List<EqEquipoDTO> listaUnidadEspOC)
        {
            if (regH.ListaGrupocodiModoOperativo.Any())
            {
                if (regUnidad.Unidad.EsUnidadModoEspecial)
                {
                    var listaEqEspRot = listaUnidadEspOC.Where(x => x.Grupocodi == regUnidad.Unidad.Grupocodi).ToList();

                    foreach (var eqDistri in regH.ListaDatoGeneradorOperativo)
                    {
                        var regEq = listaEqEspRot.Find(x => x.Equicodi == eqDistri.Equicodi);
                        var objRot = new DatoCalculoHorario() { Equicodi = regEq.Equicodi, Pe = regEq.Pe ?? 0, Pmin = regEq.Pmin ?? 0, ValorMW = eqDistri.ValorMW };

                        if (objRot.Pe > objRot.ValorMW && objRot.ValorMW >= objRot.Pmin)
                        {
                            objRot.Valor = objRot.Pe - objRot.ValorMW;
                            objRot.EsOperaMenor100 = true;
                        }
                        else
                        {
                            objRot.EsOperaMenorMin = (objRot.ValorMW < objRot.Pmin);
                            objRot.EsOpera100 = (objRot.Pe <= objRot.ValorMW);
                        }

                        regH.ListaDatoRotante.Add(objRot);
                    }

                    regH.ValorMWRotante += regH.ListaDatoRotante.Sum(x => x.Valor);
                    regH.ListaDatoRotanteXFenerg.Add(new DatoCalculoHorario() { Fenergcodi = regUnidad.Unidad.Fenergcodi, Valor = regH.ListaDatoRotante.Sum(x => x.Valor) });
                }
                else
                {
                    if (regUnidad.Unidad.TieneCicloComb)
                    {
                        foreach (var objFenerg in regH.ListaDatoOperativoXFenerg)
                        {

                            //Determinar los modos de operacion 
                            List<int> listaGrupocodiModoRrotSumar = ListarModoSegunListaEquipos(objFenerg.Fenergcodi, objFenerg.LEquipoXFenerg, objFenerg.LEquipoXFenerg, listaModoOC);
                            List<int> listaGrupocodiModoRotRestar = objFenerg.LGrupoModoXFenerg;

                            if (listaGrupocodiModoRrotSumar.Any())
                            {
                                List<PrGrupoDTO> listaModoOperativo = listaModoOC.Where(x => listaGrupocodiModoRrotSumar.Contains(x.Grupocodi)).ToList();
                                //cuando es la unidad tipo central con tv, entonces obtener el modo con mayor potencia efectiva
                                foreach (var obj in listaModoOperativo)
                                {
                                    RealizarCalculoRotanteXModo(obj, regH);
                                }
                            }
                        }
                    }
                    else
                    {
                        List<PrGrupoDTO> listaModoOperativo = listaModoOC.Where(x => regH.ListaGrupocodiModoOperativo.Contains(x.Grupocodi)).ToList();

                        //ciclo simple (solo se usa el modo de operacion activo, no el que tendría mayor potencia para ese combustible)
                        foreach (var obj in listaModoOperativo)
                        {
                            RealizarCalculoRotanteXModo(obj, regH);
                        }
                    }
                }
            }
        }

        private static void RealizarCalculoRotanteXModo(PrGrupoDTO obj, Celda30minCDispatch regH)
        {
            var lDespXModo = regH.ListaDatoDespachoOperativo.Where(x => obj.ListaGrupocodiDespacho.Contains(x.Grupocodi)).ToList();

            decimal pmax = obj.Potencia ?? 0;
            decimal pmin = obj.PotenciaMinima ?? 0;
            decimal mw = lDespXModo.Sum(x => x.ValorMW);

            var objModoRot = new DatoCalculoHorario() { Grupocodi = obj.Grupocodi, Pe = obj.Potencia ?? 0, Pmin = obj.PotenciaMinima ?? 0 };
            if (pmax > mw && mw >= pmin)
            {
                decimal mwRotante = pmax - mw;

                objModoRot.Valor = mwRotante;
                objModoRot.EsOperaMenor100 = true;
                regH.ValorMWRotante += mwRotante;
            }
            else
            {
                objModoRot.EsOperaMenorMin = (mw < pmin);
                objModoRot.EsOpera100 = (pmax <= mw);
            }

            //detalle
            regH.ListaDatoRotante.Add(objModoRot);

            //para eficiente gas, carbon
            if (regH.ListaDatoRotanteXFenerg.Find(x => x.Fenergcodi == obj.Fenergcodi) == null)
            {
                regH.ListaDatoRotanteXFenerg.Add(new DatoCalculoHorario() { Fenergcodi = obj.Fenergcodi ?? 0 });
            }
            var regFenerg = regH.ListaDatoRotanteXFenerg.Find(x => x.Fenergcodi == obj.Fenergcodi);
            regFenerg.Valor += objModoRot.Valor;
        }

        private static void RealizarCalculoMWxmanttoYRFriaXUnidadYH(DatoCDispatch regUnidad, Celda30minCDispatch regH, List<PrGrupoDTO> listaModoOC, List<EqEquipoDTO> listaUnidadEspOC)
        {
            if (regUnidad.Unidad.EsUnidadModoEspecial)
            {
                DistribuirEquipoIndispXFenergEspecial(ConstantesMigraciones.TipoCalculoMwMantto, regH, regUnidad, listaUnidadEspOC);
                DistribuirEquipoIndispXFenergEspecial(ConstantesMigraciones.TipoCalculoReservaFria, regH, regUnidad, listaUnidadEspOC);
            }
            else
            {
                DistribuirEquipoIndispXFenerg(regH, regUnidad, out List<DatoCalculoHorario> listaDatoRFria, out List<DatoCalculoHorario> listaDatoMwMantto);
                regH.ListaDatoRfriaXFenerg = listaDatoRFria;
                regH.ListaDatoMWManttoXFenerg = listaDatoMwMantto;

                //reserva fría
                AsignarValorXFenerg(ConstantesMigraciones.TipoCalculoReservaFria, regH, listaModoOC, out decimal valorCalculoRfria, out List<DatoCalculoHorario> listaDatoCalculoRFria);
                regH.ValorMWRfria = valorCalculoRfria;
                regH.ListaDatoRfria = listaDatoCalculoRFria;

                //mw x mantto
                AsignarValorXFenerg(ConstantesMigraciones.TipoCalculoMwMantto, regH, listaModoOC, out decimal valorCalculoMwMantto, out List<DatoCalculoHorario> listaDatoCalculoMwMantto);
                regH.ValorMWMantto = valorCalculoMwMantto;
                regH.ListaDatoMWMantto = listaDatoCalculoMwMantto;
            }
        }

        private static void DistribuirEquipoIndispXFenerg(Celda30minCDispatch regH, DatoCDispatch regUnidad,
                            out List<DatoCalculoHorario> listaDatoRFria, out List<DatoCalculoHorario> listaDatoMwMantto)
        {
            //combustibles de la unidad térmica (de mayor potencia efectiva)
            int fenergcodiPrincipal = regUnidad.Unidad.Fenergcodi;
            List<int> lFenergcodiUnidad = regUnidad.Unidad.ListaFuenteEnergia.Select(x => x.Fenergcodi).ToList();

            //Verificar en central con un combustible y centrales duales (gas y diesel)
            listaDatoRFria = new List<DatoCalculoHorario>();
            listaDatoMwMantto = new List<DatoCalculoHorario>();
            if (lFenergcodiUnidad.Any())
            {
                //combustible secundario (comun es que sea diesel)
                int fenergcodiSec = lFenergcodiUnidad.Where(x => x != fenergcodiPrincipal).FirstOrDefault();

                //caso especial C.T. AGUAYTIA, C.T. MALACAS 1, C.T. RF DE GENERACION TALARA 
                if (fenergcodiSec > 0 && regH.ValorMWRfriaGasEspecial > 0)
                {
                    //si hay dualidad, la prioridad es gas en los casos especial
                    fenergcodiSec = ConstantesPR5ReportesServicio.FenergcodiDiesel;
                    fenergcodiPrincipal = ConstantesPR5ReportesServicio.FenergcodiGas;
                }

                //diferenciar la TV de los otros equipos [informativo]
                int equicodiTV = regUnidad.Unidad.EquicodiTVCicloComb;
                List<int> lEqNoTV = regUnidad.Unidad.ListaEquicodi.Where(x => x != equicodiTV).ToList();

                //determinar los equipos de cada combustible
                List<int> listaEqUniversoPrinc = regUnidad.Unidad.ListaFuenteEnergia.Find(x => x.Fenergcodi == fenergcodiPrincipal).ListaEquicodiXFenerg;
                List<int> listaEqUniversoSec = (regUnidad.Unidad.ListaFuenteEnergia.Find(x => x.Fenergcodi == fenergcodiSec)?.ListaEquicodiXFenerg) ?? new List<int>();

                //Determinar
                if (fenergcodiSec > 0)
                {
                    if (fenergcodiPrincipal == ConstantesPR5ReportesServicio.FenergcodiGas)
                    {
                        ObtenerListaEquipoXCombustible(regH, listaEqUniversoPrinc, listaEqUniversoSec, fenergcodiPrincipal, fenergcodiSec, out listaDatoRFria, out listaDatoMwMantto);
                    }
                    else
                    {
                        ObtenerListaEquipoXCombustible(regH, listaEqUniversoSec, listaEqUniversoPrinc, fenergcodiSec, fenergcodiPrincipal, out listaDatoRFria, out listaDatoMwMantto);
                    }
                }
                else
                {
                    ObtenerListaEquipoXCombustible(regH, listaEqUniversoPrinc, listaEqUniversoSec, fenergcodiPrincipal, 0, out listaDatoRFria, out listaDatoMwMantto);
                }

            }
            else
            { }
        }

        private static void AsignarValorXFenerg(int tipoCalculo, Celda30minCDispatch regH, List<PrGrupoDTO> listaModoOC
                                        , out decimal valorCalculo, out List<DatoCalculoHorario> listaDatoCalculo)
        {
            valorCalculo = 0;
            listaDatoCalculo = new List<DatoCalculoHorario>();

            List<DatoCalculoHorario> listaDatoXFenerg = new List<DatoCalculoHorario>();
            if (tipoCalculo == ConstantesMigraciones.TipoCalculoReservaFria) listaDatoXFenerg = regH.ListaDatoRfriaXFenerg;
            if (tipoCalculo == ConstantesMigraciones.TipoCalculoMwMantto) listaDatoXFenerg = regH.ListaDatoMWManttoXFenerg;

            foreach (var objFenerg in listaDatoXFenerg)
            {
                List<int> listaAllEquicodi2 = objFenerg.LAllEquipo;
                List<int> listaEquicodiIndisp2 = objFenerg.LEquipoIndispXFenerg;
                List<int> listaEquicodiDisp2 = listaAllEquicodi2.Where(x => !listaEquicodiIndisp2.Contains(x)).ToList();  //modos de operacion que tiene los equipos disponibles no rfria (operativos y mantto)

                //Determinar los modos de operacion 
                List<int> listaGrupocodiModoRfriaSumar = new List<int>(), listaGrupocodiModoRfriaRestar = new List<int>();
                if (listaEquicodiIndisp2.Any())
                {
                    listaGrupocodiModoRfriaSumar = ListarModoSegunListaEquipos(objFenerg.Fenergcodi, listaAllEquicodi2, listaAllEquicodi2, listaModoOC);
                    listaGrupocodiModoRfriaRestar = ListarModoSegunListaEquipos(objFenerg.Fenergcodi, listaAllEquicodi2, listaEquicodiDisp2, listaModoOC);
                }

                if (listaGrupocodiModoRfriaSumar.Any())
                {
                    List<PrGrupoDTO> listaModoSumar = listaModoOC.Where(x => listaGrupocodiModoRfriaSumar.Contains(x.Grupocodi)).ToList();
                    List<PrGrupoDTO> listaModorestar = listaModoOC.Where(x => listaGrupocodiModoRfriaRestar.Contains(x.Grupocodi)).ToList();

                    decimal pe = listaModoSumar.Select(x => x.Potencia ?? 0).Sum(x => x);
                    decimal valorDisp = listaModorestar.Select(x => x.Potencia ?? 0).Sum(x => x);

                    if (pe > valorDisp)
                    {
                        valorCalculo += pe - valorDisp;

                        //detalle
                        if (listaModoSumar.Any())
                        {
                            foreach (var regDisp in listaModoSumar)
                            {
                                listaDatoCalculo.Add(new DatoCalculoHorario() { Grupocodi = regDisp.Grupocodi, Valor = regDisp.Potencia ?? 0, EsDisponible = true });
                            }
                        }

                        if (listaModorestar.Any())
                        {
                            foreach (var regDisp in listaModorestar)
                            {
                                listaDatoCalculo.Add(new DatoCalculoHorario() { Grupocodi = regDisp.Grupocodi, Valor = regDisp.Potencia ?? 0, EsDisponible = false });
                            }
                        }

                        //obtener el valor por fuente de energia
                        objFenerg.Valor = pe - valorDisp;

                        if (tipoCalculo == ConstantesMigraciones.TipoCalculoReservaFria)
                        {
                            if (regH.TieneCasoEspecialRfria)
                            {
                                valorCalculo = regH.ValorMWRfriaGasEspecial;
                                objFenerg.Valor = regH.ValorMWRfriaGasEspecial; //caso especial

                                listaDatoCalculo = new List<DatoCalculoHorario>();
                                foreach (var regDisp in listaModoSumar)
                                {
                                    listaDatoCalculo.Add(new DatoCalculoHorario() { Grupocodi = regDisp.Grupocodi, Valor = regH.ValorMWRfriaGasEspecial, EsDisponible = false, TieneRfriaEspecial = true });
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void DistribuirEquipoIndispXFenergEspecial(int tipoCalculo, Celda30minCDispatch regH, DatoCDispatch regUnidad, List<EqEquipoDTO> listaUnidadEspOC)
        {
            if (tipoCalculo == ConstantesMigraciones.TipoCalculoMwMantto)
            {
                var listaEqEspMantto = listaUnidadEspOC.Where(x => x.Grupocodi == regUnidad.Unidad.Grupocodi &&
                                                    (regH.ListaEquicodiMantto.Contains(x.Equicodi) || regH.ListaEquicodiManttoGas.Contains(x.Equicodi))).ToList();
                foreach (var regEq in listaEqEspMantto)
                {
                    regH.ListaDatoMWMantto.Add(new DatoCalculoHorario() { Equicodi = regEq.Equicodi, Grupocodi = regUnidad.Unidad.Grupocodi ?? 0, Valor = regEq.Pe ?? 0, EsDisponible = true });
                }

                regH.ValorMWMantto = regH.ListaDatoMWMantto.Sum(x => x.Valor);
                regH.ListaDatoMWManttoXFenerg.Add(new DatoCalculoHorario() { Fenergcodi = regUnidad.Unidad.Fenergcodi, Valor = regH.ListaDatoMWMantto.Sum(x => x.Valor) });
            }

            if (tipoCalculo == ConstantesMigraciones.TipoCalculoReservaFria)
            {
                var listaEqEspRfria = listaUnidadEspOC.Where(x => x.Grupocodi == regUnidad.Unidad.Grupocodi &&
                                                    (regH.ListaEquicodiCandidatoRfria.Contains(x.Equicodi) && !regH.ListaEquicodiManttoGas.Contains(x.Equicodi))).ToList();
                foreach (var regEq in listaEqEspRfria)
                {
                    regH.ListaDatoRfria.Add(new DatoCalculoHorario() { Equicodi = regEq.Equicodi, Valor = regEq.Pe ?? 0, Grupocodi = regUnidad.Unidad.Grupocodi ?? 0 });
                }

                regH.ValorMWRfria += regH.ListaDatoRfria.Sum(x => x.Valor);
                regH.ListaDatoRfriaXFenerg.Add(new DatoCalculoHorario() { Fenergcodi = regUnidad.Unidad.Fenergcodi, Valor = regH.ListaDatoRfria.Sum(x => x.Valor) });

            }
        }

        private static void GenerarDetalleInformativoXDato30Min(List<DatoCDispatch> listaUnidad, CDespachoDiario regCDespachoXDia)
        {
            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.Unidad.Equipadre == 13656)
                { }
                if (regUnidad.ListagrupocodiDesp.Contains(671))
                { }

                for (int h = 1; h <= 48; h++)
                {
                    if (h == 19)
                    { }
                    Celda30minCDispatch regH = regUnidad.Array30Min[h - 1];

                    List<string> listaMsj = new List<string>();

                    //FECHA HORA
                    List<string> lDesc = new List<string>();
                    foreach (var obj in regH.ListaEquicodiAll)
                    {
                        EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == obj);
                        string eqabrev = ((eq?.Equiabrev) ?? "").Trim();
                        lDesc.Add(string.Format("{0}", eqabrev));
                    }

                    string sCabecera = string.Empty;
                    if (regUnidad.Unidad.TieneCicloComb && !regUnidad.Unidad.EsUnidadModoEspecial)
                    {
                        sCabecera = "Los cálculos se realizan a nivel de central: " + string.Join(", ", lDesc);
                    }
                    else
                    {
                        if (regUnidad.Unidad.EsUnidadModoEspecial)
                            sCabecera = "Los cálculos se realizan por cada unidad especial: " + string.Join(", ", lDesc);
                        else
                            sCabecera = "El cálculo se realiza por grupo de despacho: " + string.Join(", ", lDesc);
                    }
                    listaMsj.Add(string.Format("{0} {1}", regH.FechaHora.ToString(ConstantesAppServicio.FormatoHora), sCabecera));

                    //Grupos de despacho operativos
                    if (regH.ListaDatoDespachoOperativo.Any())
                    {
                        listaMsj.Add("Grupos de despacho operativos: ");

                        lDesc = new List<string>();
                        if (regUnidad.Unidad.EsUnidadModoEspecial)
                        {
                            var obj = regH.ListaDatoDespachoOperativo.FirstOrDefault();
                            PrGrupoDTO grDesp = regCDespachoXDia.ListaDespachoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                            string grabrev = ((grDesp?.Grupoabrev) ?? "").Trim();
                            lDesc.Add(string.Format("Pd: {1} [{0}]", grabrev, obj.ValorMW));

                            foreach (var objEq in regH.ListaDatoGeneradorOperativo)
                            {
                                EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == objEq.Equicodi);
                                string eqabrev = ((eq?.Equiabrev) ?? "").Trim();

                                string detalleEq = !string.IsNullOrEmpty(objEq.DetalleHO) ? ". " + objEq.DetalleHO.Trim() : string.Empty;

                                lDesc.Add(string.Format("-Pd: {1} [{0}]{2}", eqabrev, objEq.ValorMW, detalleEq));
                            }
                        }
                        else
                        {
                            foreach (var obj in regH.ListaDatoDespachoOperativo)
                            {
                                PrGrupoDTO grDesp = regCDespachoXDia.ListaDespachoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                                string grabrev = ((grDesp?.Grupoabrev) ?? "").Trim();

                                var objEq = regH.ListaDatoGeneradorOperativo.FirstOrDefault(x => grDesp.ListaEquicodi.Contains(x.Equicodi));
                                string detalleEq = string.Empty;
                                if (objEq != null) detalleEq = !string.IsNullOrEmpty(objEq.DetalleHO) ? ". " + objEq.DetalleHO.Trim() : string.Empty;

                                lDesc.Add(string.Format("Pd: {1} [{0}]{2}", grabrev, obj.ValorMW, detalleEq));
                            }

                        }

                        listaMsj.AddRange(lDesc);
                    }

                    //Manttos
                    if (regH.ListaDatoMantto.Any())
                    {
                        listaMsj.Add(" ");
                        listaMsj.Add("Equipos de generación con mantenimientos: ");

                        lDesc = new List<string>();
                        foreach (var obj in regH.ListaDatoMantto)
                        {
                            EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == obj.Equicodi);
                            string eqabrev = ((eq?.Equiabrev) ?? "").Trim();
                            string strPrefijo = "M)";
                            lDesc.Add(string.Format("{2} {0}: {1}", eqabrev, obj.Detalle, strPrefijo));
                        }

                        listaMsj.AddRange(lDesc);
                    }

                    if (regH.ListaDatoIndisponibilidad.Any())
                    {
                        listaMsj.Add(" ");
                        listaMsj.Add("Equipos no operativos y no están en mantenimiento: ");

                        lDesc = new List<string>();
                        foreach (var obj in regH.ListaDatoIndisponibilidad)
                        {
                            EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == obj.Equicodi);
                            string eqabrev = ((eq?.Equiabrev) ?? "").Trim();

                            //descripcion tminarranque
                            string descDisp = string.Empty;
                            if (obj.TieneTminarranque)
                            {
                                descDisp = string.Format("La unidad todavía NO cumple el tiempo mínimo de arranque ({0}min). Indisponible desde {1} hasta {2}."
                                         , obj.Tminarranque, obj.FechaUltimaHO.Value.ToString(ConstantesAppServicio.FormatoFechaFull), obj.FechaUltimaHO.Value.AddMinutes(obj.Tminarranque).ToString(ConstantesAppServicio.FormatoFechaFull));
                            }
                            else
                            {
                                if (obj.FechaUltimaHO != null) descDisp = string.Format("Disponible desde {1}. La unidad cumplió el tiempo mínimo de arranque ({0}min)."
                                                        , obj.Tminarranque, obj.FechaUltimaHO.Value.AddMinutes(obj.Tminarranque + 1).ToString(ConstantesAppServicio.FormatoFechaFull));
                                else
                                    descDisp = string.Format("La unidad se encuentra disponible desde antes de {0}", obj.FechaDispAntes.Value.ToString(ConstantesAppServicio.FormatoFechaFull));
                            }

                            string strPrefijo = obj.TieneTminarranque ? "A) " : "Rf) ";

                            lDesc.Add(string.Format("{1}{0}: {2}", eqabrev, strPrefijo, descDisp));
                        }

                        listaMsj.AddRange(lDesc);
                    }

                    //Datos rotante
                    if (regH.ListaDatoRotante.Any())
                    {
                        var strValorCalculo = regH.ValorMWRotante > 0 ? ": " + regH.ValorMWRotante : ": No tiene";

                        listaMsj.Add(" ");
                        listaMsj.Add("Cálculo Reserva Rotante" + strValorCalculo);

                        if (regUnidad.Unidad.EsUnidadModoEspecial)
                        {
                            lDesc = new List<string>();
                            int i = 1;
                            foreach (var obj in regH.ListaDatoRotante)
                            {
                                string strPrefijo = "C" + i + ") ";

                                EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == obj.Equicodi);
                                string eqabrev = ((eq?.Equiabrev) ?? "").Trim();
                                string strCumple = GetMensajeRotante(obj);

                                lDesc.Add(string.Format("{1}{0}. Pmax: {2} | Pd: {3} | Pdisp: {4}{5}", eqabrev, strPrefijo, obj.Pe, obj.ValorMW, obj.Valor, strCumple));

                                i++;
                            }

                            listaMsj.AddRange(lDesc);
                        }
                        else
                        {
                            lDesc = new List<string>();
                            int i = 1;
                            foreach (var obj in regH.ListaDatoRotante)
                            {
                                string strPrefijo = "C" + i + ") ";
                                string strPrefijoDet = "-";

                                PrGrupoDTO gr = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                                string grabrev = ((gr?.Gruponomb) ?? "").Trim();
                                string strCumple = GetMensajeRotante(obj);

                                lDesc.Add(string.Format("{1}{0}", grabrev, strPrefijo));
                                lDesc.Add(string.Format("{1}Pmax: {0}", obj.Pe, strPrefijoDet));

                                foreach (var objDesp in regH.ListaDatoDespachoOperativo.Where(x => gr.ListaGrupocodiDespacho.Contains(x.Grupocodi)).ToList())
                                {
                                    PrGrupoDTO grDesp = regCDespachoXDia.ListaAllGrupo.Find(x => x.Grupocodi == objDesp.Grupocodi);
                                    string grabrev2 = ((grDesp?.Grupoabrev) ?? "").Trim();
                                    lDesc.Add(string.Format("{2}Pd: {1} [{0}]", grabrev2, objDesp.ValorMW, strPrefijoDet));
                                }
                                lDesc.Add(string.Format("{2}Pdisp: {0}{1}", obj.Valor, strCumple, strPrefijoDet));
                                i++;
                            }

                            listaMsj.AddRange(lDesc);
                        }
                    }


                    //Manttos y MW mantto
                    if (regH.ListaDatoMantto.Any())
                    {
                        var strValorCalculo = regH.ValorMWMantto > 0 ? ": " + regH.ValorMWMantto : ": No tiene";

                        listaMsj.Add(" ");
                        listaMsj.Add("Cálculo MWxMantto" + strValorCalculo);
                        if (regUnidad.Unidad.EsUnidadModoEspecial)
                        {
                            lDesc = new List<string>();
                            int i = 1;
                            foreach (var obj in regH.ListaDatoMWMantto)
                            {
                                string strPrefijo = "MWxM" + i + ") ";

                                EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == obj.Equicodi);
                                string eqabrev = ((eq?.Equiabrev) ?? "").Trim();

                                PrGrupoDTO gr = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                                string grabrev = ((gr?.Gruponomb) ?? "").Trim();

                                lDesc.Add(string.Format("{1}{0}. Pmax: {2} ({3})", eqabrev, strPrefijo, obj.Valor, grabrev));

                                i++;
                            }

                            listaMsj.AddRange(lDesc);
                        }
                        else
                        {
                            lDesc = new List<string>();
                            int i = 1;
                            foreach (var obj in regH.ListaDatoMWMantto)
                            {
                                string strPrefijo = "MWxM" + i + ") ";

                                PrGrupoDTO gr = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                                string grabrev = ((gr?.Gruponomb) ?? "").Trim();
                                string disp = obj.EsDisponible ? "Pmax" : "Pd";

                                lDesc.Add(string.Format("{1}{0}: {2} ({3})", disp, strPrefijo, obj.Valor, grabrev));

                                i++;
                            }
                            listaMsj.AddRange(lDesc);
                        }
                    }

                    //datos reserva fria
                    if (regH.ListaDatoRfria.Any())
                    {
                        var strValorCalculo = regH.ValorMWRfria > 0 ? ": " + regH.ValorMWRfria : ": No tiene";

                        listaMsj.Add(" ");
                        listaMsj.Add("Cálculo de Reserva fría" + strValorCalculo);

                        if (regUnidad.Unidad.EsUnidadModoEspecial)
                        {
                            lDesc = new List<string>();
                            int i = 1;
                            foreach (var obj in regH.ListaDatoRfria)
                            {
                                string strPrefijo = "Rf" + i + ") ";

                                EqEquipoDTO eq = regCDespachoXDia.ListaAllEquipo.Find(x => x.Equicodi == obj.Equicodi);
                                string eqabrev = ((eq?.Equiabrev) ?? "").Trim();

                                PrGrupoDTO gr = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                                string grabrev = ((gr?.Gruponomb) ?? "").Trim();

                                lDesc.Add(string.Format("{1}{0}. Pmax: {2} ({3})", eqabrev, strPrefijo, obj.Valor, grabrev));

                                i++;
                            }

                            listaMsj.AddRange(lDesc);
                        }
                        else
                        {
                            lDesc = new List<string>();
                            int i = 1;
                            foreach (var obj in regH.ListaDatoRfria)
                            {
                                string strPrefijo = "Rf" + i + ") ";

                                PrGrupoDTO gr = regCDespachoXDia.ListaModoOC.Find(x => x.Grupocodi == obj.Grupocodi);
                                string grabrev = ((gr?.Gruponomb) ?? "").Trim();
                                string disp = obj.EsDisponible ? "Pmax" : "Pd";
                                string disponibilidadCombustible = "";
                                if (regH.TieneCasoEspecialRfria) disponibilidadCombustible = "Según Stock de Combustible Programado";

                                lDesc.Add(string.Format("{1}{0}: {2} ({3}) {4}", disp, strPrefijo, obj.Valor, grabrev, disponibilidadCombustible));

                                i++;
                            }
                            listaMsj.AddRange(lDesc);
                        }
                    }

                    regH.ListaMsj = listaMsj;
                }
            }

        }

        private static void DistribuirCombustibleEnPeriodos(decimal combustibleGasStockDisponible, int numMediaHoraRfria,
                                ConsumoHorarioCombustible regCurva, decimal pe, decimal pmin,
                                out decimal consumoAUtilizar, out decimal potenciaAUtilizar, out int numMediaHoraAUtilizar)
        {
            consumoAUtilizar = 0;
            potenciaAUtilizar = 0;
            numMediaHoraAUtilizar = 0;

            //umbral
            decimal valorConsumo30minPe = ((pe * decimal.Round(regCurva.PendienteM01, 4) + decimal.Round(regCurva.CoeficienteIndependiente, 4))) / 2.0m;
            decimal valorConsumo30minPmin = ((pmin * decimal.Round(regCurva.PendienteM01, 4) + decimal.Round(regCurva.CoeficienteIndependiente, 4))) / 2.0m;

            //si se comsumo todas las medias horas
            decimal totalConsumoPe = valorConsumo30minPe * numMediaHoraRfria;
            decimal totalConsumoPmin = valorConsumo30minPmin * numMediaHoraRfria;

            if (combustibleGasStockDisponible >= totalConsumoPmin)
            {
                //Criterio 1: Si el combustible disponible se podría consumir a potencia efectiva
                if (combustibleGasStockDisponible >= totalConsumoPe)
                {
                    potenciaAUtilizar = pe;
                    consumoAUtilizar = totalConsumoPe;
                    numMediaHoraAUtilizar = numMediaHoraRfria;
                }
                else
                {
                    //Criterio 2: El combustible disponible se podría consumir entre potencia minima y potencia máxima
                    decimal consumoTmp30min = combustibleGasStockDisponible / (numMediaHoraRfria * 1.0M);

                    if (regCurva.PendienteM01 > 0)
                        potenciaAUtilizar = ((consumoTmp30min * 2.0m) - decimal.Round(regCurva.CoeficienteIndependiente, 4)) / decimal.Round(regCurva.PendienteM01, 4);
                    consumoAUtilizar = combustibleGasStockDisponible;
                    numMediaHoraAUtilizar = numMediaHoraRfria;
                }
            }
            else
            {
                //Criterio 3: No alcanza el combustible a potencia minima, distribuir pmin en las medias horas que pueda
                for (int numTmp = numMediaHoraRfria - 1; numTmp > 0; numTmp--)
                {
                    decimal totalConsumoPminTmp = valorConsumo30minPmin * numTmp;
                    if (combustibleGasStockDisponible >= totalConsumoPminTmp)
                    {
                        potenciaAUtilizar = pmin;
                        consumoAUtilizar = totalConsumoPminTmp;
                        numMediaHoraAUtilizar = numTmp;
                        break;
                    }
                }
            }

            //redondear
            potenciaAUtilizar = Math.Round(potenciaAUtilizar, 4);

        }

        private static string GetMensajeRotante(DatoCalculoHorario obj)
        {
            var str = string.Empty;

            if (obj.EsOpera100) str = ". Sin rotante: está operando al 100%.";
            if (obj.EsOperaMenor100) str = ". Tiene rotante.";
            if (obj.EsOperaMenorMin) str = string.Format(". Sin rotante: La potencia de las unidades no alcanza la Pmin {0}.", obj.Pmin);

            return str;
        }

        private static void ObtenerListaEquipoXCombustible(Celda30minCDispatch regH, List<int> listaEqUniversoPrinc, List<int> listaEqUniversoSecun,
                                int fenergcodiPrinc, int fenergcodiSecun,
                                out List<DatoCalculoHorario> listaDatoRFria, out List<DatoCalculoHorario> listaDatoMwMantto)
        {
            listaDatoRFria = new List<DatoCalculoHorario>();
            listaDatoMwMantto = new List<DatoCalculoHorario>();

            List<int> lEqRfriaPrinc = new List<int>();
            List<int> lEqRfriaSec = new List<int>();
            List<int> lEqRfriaIndispPrinc = new List<int>();
            List<int> lEqRfriaIndispSec = new List<int>();

            List<int> lEqMwManttoPrinc = new List<int>();
            List<int> lEqMwManttoSec = new List<int>();
            List<int> lEqMwManttoIndispPrinc = new List<int>();
            List<int> lEqMwManttoIndispSec = new List<int>();

            //todos los equipos (E/S, F/S) excluyendo los que estan en mantto y los que estan en tiempo minimo de arranque
            var listaAllEquipoCalculoRFria = regH.ListaEquicodiAll.Where(x => !regH.ListaEquicodiMantto.Contains(x) && !regH.ListaEquicodiTmin.Contains(x)).ToList();
            var listaEquipoIndispRFria = regH.ListaEquicodiCandidatoRfria;

            //todos los equipos (E/S, F/S)
            var listaAllEquipoCalculoMwMantto = regH.ListaEquicodiAll;
            var listaEquipoIndispMwMantto = regH.ListaEquicodiMantto;

            //distribuir entre los combustibles
            var lEqPrincOperativo = (regH.ListaDatoOperativoXFenerg.Find(x => x.Fenergcodi == fenergcodiPrinc)?.LEquipoXFenerg) ?? new List<int>();
            var lEqSecunOperativo = (regH.ListaDatoOperativoXFenerg.Find(x => x.Fenergcodi == fenergcodiSecun)?.LEquipoXFenerg) ?? new List<int>();

            //ningun equipo puede tener reserva fria de Gas porque Camisea está en mantenimiento
            if (regH.ListaEquicodiManttoGas.Any())
            {
                // a los equipos que estan fuera de servicio, también excluir los equipos que tienen mantenimiento Camisea
                lEqRfriaPrinc = new List<int>();
                lEqRfriaIndispPrinc = new List<int>();

                lEqRfriaSec = listaAllEquipoCalculoRFria.Where(x => listaEqUniversoSecun.Contains(x)).ToList();
                lEqRfriaIndispSec = listaEquipoIndispRFria.Where(x => lEqRfriaSec.Contains(x)).ToList();
            }
            else
            {
                //verificar que el combustible secundario está operando
                lEqRfriaSec = lEqSecunOperativo;
                lEqRfriaPrinc = listaAllEquipoCalculoRFria.Where(x => !lEqRfriaSec.Contains(x)).ToList();

                lEqRfriaIndispPrinc = listaEquipoIndispRFria.Where(x => listaEqUniversoPrinc.Contains(x)).ToList();
                lEqRfriaIndispSec = new List<int>();
            }

            //manttos
            var lEqManttoPrincCand = listaEqUniversoPrinc.Where(x => regH.ListaEquicodiMantto.Contains(x) || regH.ListaEquicodiManttoGas.Contains(x)).ToList();
            var lEqManttoSecunCand = listaEqUniversoSecun.Where(x => regH.ListaEquicodiMantto.Contains(x)).ToList();

            if (lEqSecunOperativo.Any() || lEqRfriaSec.Any())
            {
                //si hay secundario entonces quitar al principal los equipos de camisea
                lEqManttoPrincCand = lEqManttoPrincCand.Where(x => !regH.ListaEquicodiManttoGas.Contains(x)).ToList();
            }

            //universo para mantto
            lEqMwManttoPrinc.AddRange(lEqPrincOperativo);
            lEqMwManttoPrinc.AddRange(lEqRfriaPrinc);
            lEqMwManttoPrinc.AddRange(lEqManttoPrincCand);
            lEqMwManttoPrinc = lEqMwManttoPrinc.Distinct().ToList();
            lEqMwManttoIndispPrinc = lEqManttoPrincCand;

            lEqMwManttoSec = listaEqUniversoSecun.Where(x => !lEqMwManttoPrinc.Contains(x)).ToList();
            lEqMwManttoIndispSec = lEqMwManttoSec.Where(x => lEqManttoSecunCand.Contains(x)).ToList();

            //Combustible 1
            if (lEqRfriaPrinc.Any())
            {
                listaDatoRFria.Add(new DatoCalculoHorario()
                {
                    Fenergcodi = fenergcodiPrinc,
                    LAllEquipo = lEqRfriaPrinc,
                    LEquipoIndispXFenerg = lEqRfriaIndispPrinc
                });
            }

            //Combustible 2
            if (fenergcodiSecun > 0 && lEqRfriaSec.Any())
            {
                listaDatoRFria.Add(new DatoCalculoHorario()
                {
                    Fenergcodi = fenergcodiSecun,
                    LAllEquipo = lEqRfriaSec,
                    LEquipoIndispXFenerg = lEqRfriaIndispSec
                });
            }

            //Combustible 1
            if (lEqMwManttoPrinc.Any())
            {
                listaDatoMwMantto.Add(new DatoCalculoHorario()
                {
                    Fenergcodi = fenergcodiPrinc,
                    LAllEquipo = lEqMwManttoPrinc,
                    LEquipoIndispXFenerg = lEqMwManttoIndispPrinc
                });
            }

            //Combustible 2
            if (fenergcodiSecun > 0 && lEqMwManttoSec.Any())
            {
                listaDatoMwMantto.Add(new DatoCalculoHorario()
                {
                    Fenergcodi = fenergcodiSecun,
                    LAllEquipo = lEqMwManttoSec,
                    LEquipoIndispXFenerg = lEqMwManttoIndispSec
                });
            }
        }

        private static void ListarDatosOperativo(int h, EqEquipoDTO regUnidadOC, Celda30minCDispatch regH, CDespachoDiario regCDespachoXDia)
        {
            List<int> listaAllEquicodiPosibleOperativo = new List<int>();

            //obtener los grupos despachos que operan para la unidad (central, ciclo simple)
            var lista48Tmp = regCDespachoXDia.ListaMe48XDiaMW.Where(x => regH.ListaGrupocodiDespachoAll.Contains(x.Grupocodi)).ToList();
            foreach (var regM48 in lista48Tmp)
            {
                decimal genMW = regM48 != null ? ((decimal?)regM48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regM48, null)).GetValueOrDefault(0) : 0;
                if (genMW > 0)
                {
                    regH.ValorMWActiva += genMW;
                    regH.ListaGrupocodiDespachoOperativo.Add(regM48.Grupocodi);

                    //agregar detalle
                    regH.ListaDatoDespachoOperativo.Add(new DatoCalculoHorario() { Grupocodi = regM48.Grupocodi, ValorMW = genMW });

                    //obtener todos los equipos posibles, luego determinar cuales estuvieron operativos
                    PrGrupoDTO regDesp = regCDespachoXDia.ListaDespachoOC.Find(x => x.Grupocodi == regM48.Grupocodi);
                    if (regDesp != null) listaAllEquicodiPosibleOperativo.AddRange(regDesp.ListaEquicodi);
                }
            }

            //obtener los equipos que tuvieron horas de operacion en el H
            List<EveHoraoperacionDTO> listaHOUnidadXeq = regCDespachoXDia.ListaHOXDiaHoy.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad && listaAllEquicodiPosibleOperativo.Contains(x.Equicodi ?? 0))
                                                        .OrderByDescending(x => x.Hophorfin).ToList();
            var listaHO30min = HorasOperacionAppServicio.ListarHO30minConsumoCombustible(listaHOUnidadXeq, regCDespachoXDia.Fecha);
            listaHO30min = listaHO30min.Where(x => x.HIni48 <= h && h <= x.HFin48).ToList();

            //Lista de equipos operativos segun HO
            List<int> listaEqOperativo = new List<int>();
            List<int> listaGrupoModoOperativo = new List<int>();

            decimal valorMwRestante = regH.ValorMWActiva;
            foreach (var equicodi in listaAllEquicodiPosibleOperativo)
            {
                EveHoraoperacionDTO regHoUn = listaHO30min.Find(x => x.Equicodi == equicodi);
                if (regHoUn != null)
                {
                    EveHoraoperacionDTO regHoModo = regCDespachoXDia.ListaHOXDiaHoy.Find(x => x.Hopcodi == regHoUn.Hopcodipadre);
                    if (regHoModo != null)
                    {
                        regHoUn.Subcausadesc = regHoModo != null ? regHoModo.Subcausadesc : "";

                        listaGrupoModoOperativo.Add(regHoModo.Grupocodi ?? 0);
                        listaEqOperativo.Add(equicodi);

                        //agregar detalle
                        if (regUnidadOC.EsUnidadModoEspecial)
                        {
                            EqEquipoDTO regEqEsp = regCDespachoXDia.ListaUnidadEspOC.Find(x => x.Grupocodi == regUnidadOC.Grupocodi && x.Equicodi == equicodi);
                            if (regEqEsp != null && valorMwRestante > 0)
                            {
                                decimal valorTmp = valorMwRestante > regEqEsp.Pe ? regEqEsp.Pe.GetValueOrDefault(0) : valorMwRestante;
                                regH.ListaDatoGeneradorOperativo.Add(new DatoCalculoHorario() { TieneHO = true, Equicodi = equicodi, Grupocodi = regUnidadOC.Grupocodi ?? 0, ValorMW = valorTmp, Subcausacodi = regHoUn.Subcausacodi ?? 0, DetalleHO = GetDescripcionHo(regHoUn) });

                                valorMwRestante -= regEqEsp.Pe.GetValueOrDefault(0);
                            }
                        }
                        else
                        {
                            //ciclo simple o combinado
                            regH.ListaDatoGeneradorOperativo.Add(new DatoCalculoHorario() { TieneHO = true, Equicodi = equicodi, Grupocodi = regHoModo.Grupocodi ?? 0, Subcausacodi = regHoModo.Subcausacodi ?? 0, DetalleHO = GetDescripcionHo(regHoModo) });
                        }
                    }
                }
            }

            //Lista de equipos restantes sin horas de operacion
            List<int> listaEqRestanteOperativo = listaAllEquicodiPosibleOperativo.Where(x => !listaEqOperativo.Contains(x)).ToList();
            if (listaEqRestanteOperativo.Any())
            {
                if (regUnidadOC.EsUnidadModoEspecial)
                {
                    //distribuir los mw restantes en las otros equipos especiales
                    if (valorMwRestante > 0)
                    {
                        //de las unidades restantes, utilizar la potencia mínima
                        List<EqEquipoDTO> listaEqEspRestante = regCDespachoXDia.ListaUnidadEspOC.Where(x => x.Grupocodi == regUnidadOC.Grupocodi && !listaEqOperativo.Contains(x.Equicodi))
                                                                            .OrderBy(x => x.Pmin).ThenBy(x => x.Pmax).ToList();

                        foreach (var regEqEsp in listaEqEspRestante)
                        {
                            if (valorMwRestante > 0)
                            {
                                if (valorMwRestante > regEqEsp.Pmin && regEqEsp.Pmin > 0)
                                {
                                    decimal valorTmp = valorMwRestante > regEqEsp.Pmin ? regEqEsp.Pmin.GetValueOrDefault(0) : valorMwRestante;
                                    regH.ListaDatoGeneradorOperativo.Add(new DatoCalculoHorario() { Equicodi = regEqEsp.Equicodi, ValorMW = valorTmp, });
                                    listaEqOperativo.Add(regEqEsp.Equicodi);
                                    listaGrupoModoOperativo.Add(regEqEsp.Grupocodi.Value);

                                    valorMwRestante -= regEqEsp.Pmin.GetValueOrDefault(0);
                                }
                            }
                        }

                        foreach (var regEqEsp in listaEqEspRestante)
                        {
                            DatoCalculoHorario eqDistribuido = regH.ListaDatoGeneradorOperativo.Find(x => x.Equicodi == regEqEsp.Equicodi);

                            if (eqDistribuido != null && valorMwRestante > 0)
                            {
                                decimal potDispEq = (regEqEsp.Pe ?? 0) - eqDistribuido.ValorMW;
                                if (potDispEq > 0)
                                {
                                    decimal valorTmp = valorMwRestante > potDispEq ? potDispEq : valorMwRestante;
                                    eqDistribuido.ValorMW += valorTmp;

                                    valorMwRestante -= potDispEq;
                                }
                            }

                        }
                    }
                }
                else
                {
                    //obtener todos los modos que tiene equipos operativos
                    List<PrGrupoDTO> listaModoXUnidad = regCDespachoXDia.ListaModoOC.Where(x => x.ListaEquicodi.Any(y => listaAllEquicodiPosibleOperativo.Contains(y)))
                                                        .OrderByDescending(x => x.Potencia).ToList();

                    //buscar modos de operacion para los equipos restantes
                    foreach (var regModo in listaModoXUnidad)
                    {
                        if (listaEqRestanteOperativo.Any() && regModo.ListaEquicodi.All(y => listaEqRestanteOperativo.Contains(y)))
                        {
                            listaGrupoModoOperativo.Add(regModo.Grupocodi);
                            listaEqRestanteOperativo = listaEqRestanteOperativo.Where(x => !regModo.ListaEquicodi.Contains(x)).ToList();

                            //agregar restantes operativos
                            foreach (var equicodi in regModo.ListaEquicodi)
                            {
                                listaEqOperativo.Add(equicodi);
                                regH.ListaDatoGeneradorOperativo.Add(new DatoCalculoHorario() { Equicodi = equicodi, Grupocodi = regModo.Grupocodi });
                            }
                        }
                    }
                }
            }

            //salidas
            regH.ListaEquicodiOperativo = listaEqOperativo;
            regH.ListaGrupocodiModoOperativo = listaGrupoModoOperativo.Distinct().ToList();

            //2. datos de potencia por cada modo de operación 
            List<PrGrupoDTO> listaModoOperativo = regCDespachoXDia.ListaModoOC.Where(x => regH.ListaGrupocodiModoOperativo.Contains(x.Grupocodi)).ToList();
            foreach (var regModo in listaModoOperativo)
            {
                //para CVC se utiliza todas las unidades
                decimal valorMWCVC = 0;

                //para CVNC si está en ciclo combinado solo considerar la TV caso contrario todas las unidades
                decimal valorMWCNVC = 0;

                foreach (var grupocodiDesp in regModo.ListaGrupocodiDespacho)
                {
                    var regDato = regH.ListaDatoDespachoOperativo.Find(x => x.Grupocodi == grupocodiDesp);
                    if (regDato != null)
                    {
                        //CVC
                        valorMWCVC += regDato.ValorMW;

                        //CVNC
                        if (regModo.TieneModoCicloCombinado)
                        {
                            //la TV es el grupo padre del ciclo combinado
                            if (grupocodiDesp == regModo.Grupopadre)
                            {
                                valorMWCNVC += regDato.ValorMW;
                            }
                        }
                        else
                        {
                            valorMWCNVC += regDato.ValorMW;
                        }
                    }
                }

                regH.ListaDatoModoOperativo.Add(new DatoCalculoHorario()
                {
                    Grupocodi = regModo.Grupocodi,
                    ValorMWCVC = valorMWCVC,
                    ValorMWCNVC = valorMWCNVC
                });
            }
        }

        private static void ListarEquiposIndispXMantto(int h, List<int> listaEquicodiNoOperativo, Celda30minCDispatch regH, CDespachoDiario regCDespachoXDia)
        {
            List<int> listaEquicodiMantto = new List<int>();
            List<int> listaEquicodiManttoGas = new List<int>();

            foreach (var equicodiNoOp in listaEquicodiNoOperativo)
            {
                EqEquipoDTO regEq = regCDespachoXDia.ListaEquipoOC.Find(x => x.Equicodi == equicodiNoOp);
                EqEquipoDTO equipoPadre = regCDespachoXDia.ListaEquipoOC.Find(x => x.Equicodi == regEq.Equipadre);

                List<int> listaEqToMantto = new List<int>() { equicodiNoOp };
                if (equipoPadre != null) listaEqToMantto.Add(equipoPadre.Equicodi);

                //mantenimientos de generación
                VerificaEqbyIndisponibilidadXMantto(h, listaEqToMantto, regCDespachoXDia.ListaManttoTermicoXDia, out bool existeMantto, out List<string> listaManttoDesc);
                if (existeMantto)
                {
                    listaEquicodiMantto.Add(equicodiNoOp);

                    //agregar detalle
                    regH.ListaDatoMantto.Add(new DatoCalculoHorario() { Equicodi = equicodiNoOp, Detalle = string.Join(". ", listaManttoDesc) });
                }

                //mantenimientos de Camisea
                var existeEquipoConexionCamisea = regCDespachoXDia.ListaEquicodiTieneConexionCamisea.Any(x => listaEqToMantto.Any(y => y == x));
                if (existeEquipoConexionCamisea)
                {
                    VerificaEqbyIndisponibilidadXMantto(h, ListaEquicodiCamisea(), regCDespachoXDia.ListaManttoTermicoCamiseaXDia, out bool existeManttoCamisea, out List<string> listaManttoCamiseaDesc);
                    if (existeManttoCamisea)
                    {
                        listaEquicodiManttoGas.Add(equicodiNoOp);

                        //agregar detalle
                        regH.ListaDatoMantto.Add(new DatoCalculoHorario() { Equicodi = equicodiNoOp, Detalle = " [CAMISEA] " + string.Join(". ", listaManttoCamiseaDesc) });
                    }
                }
            }

            regH.ListaEquicodiMantto = listaEquicodiMantto;
            regH.ListaEquicodiManttoGas = listaEquicodiManttoGas;
        }

        private static void ListarEquipoIndispXTminarranq(int h, Celda30minCDispatch regH, DateTime fecha, List<int> listaEquicodiNoOperativo
                                    , Celda30minCDispatch[] array30MinHoy, List<EveHoraoperacionDTO> listaHOAyer, List<PrGrupoDTO> listaGrModo)
        {
            List<int> listaEquicodiTminarranq = new List<int>();

            foreach (var equicodiNoOp in listaEquicodiNoOperativo)
            {
                ObtenerUltimoMOxEquipo(h, equicodiNoOp, array30MinHoy, listaHOAyer, listaGrModo, out DateTime? fechaUltimaHO, out int grupocodiModo);

                bool tieneTmin = false;
                DateTime? fechaDispAntes = null;
                int tMinArranque = 0;
                if (grupocodiModo > 0)
                {
                    PrGrupoDTO grModo = listaGrModo.Find(x => x.Grupocodi == grupocodiModo);
                    if (grModo != null)
                    {
                        tMinArranque = grModo.TminArranque;

                        if (fechaUltimaHO != null)
                        {
                            DateTime fechaActual = fecha.AddMinutes(h * 30);
                            double totalMinutos = ((TimeSpan)(fechaActual - fechaUltimaHO)).TotalMinutes;
                            if (totalMinutos <= tMinArranque)
                            {
                                listaEquicodiTminarranq.Add(equicodiNoOp);
                                tieneTmin = true;
                            }
                        }
                    }
                }

                if (grupocodiModo <= 0 || fechaUltimaHO == null)
                    fechaDispAntes = fecha.Date.AddDays(-1).AddMinutes(30);

                //agregar detalle
                regH.ListaDatoIndisponibilidad.Add(new DatoCalculoHorario()
                {
                    Equicodi = equicodiNoOp,
                    TieneTminarranque = tieneTmin,
                    Grupocodi = grupocodiModo,
                    FechaDispAntes = fechaDispAntes,
                    FechaUltimaHO = fechaUltimaHO,
                    Tminarranque = tMinArranque
                });
            }

            regH.ListaEquicodiTmin = listaEquicodiTminarranq;
        }

        private static void ObtenerUltimoMOxEquipo(int h, int equicodi, Celda30minCDispatch[] array30MinHoy, List<EveHoraoperacionDTO> listaHOAyer, List<PrGrupoDTO> listaGrModo
                                        , out DateTime? fechaUltima, out int grupocodiModo)
        {
            grupocodiModo = 0;
            fechaUltima = null;

            //día de ayer
            int grupocodiModoAyer = 0;
            DateTime? fechaHOAyer = null;
            List<EveHoraoperacionDTO> listaHOUnidadXeq = listaHOAyer.Where(x => x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad && x.Equicodi == equicodi)
                                                        .OrderByDescending(x => x.Hophorfin).ToList();
            if (listaHOUnidadXeq.Any())
            {
                var regHoUn = listaHOUnidadXeq.First();
                var regHoModo = listaHOAyer.Find(x => x.Hopcodi == regHoUn.Hopcodipadre && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo);
                if (regHoModo != null)
                {
                    grupocodiModoAyer = regHoModo.Grupocodi ?? 0;
                    fechaHOAyer = regHoUn.Hophorfin.Value;
                }
            }

            //día de hoy
            int grupocodiModoHoy = 0;
            DateTime? fechaHOHoy = null;
            if (array30MinHoy != null)
            {
                for (int i = h - 1; i >= 0 && (fechaHOHoy == null); i--)
                {
                    var celda = array30MinHoy[i];

                    //verificar si el equipo estuvo operando
                    if (celda != null && celda.ListaEquicodiOperativo.Contains(equicodi))
                    {
                        fechaHOHoy = celda.FechaHora;

                        //si estuvo operando saber que modo
                        List<PrGrupoDTO> listaGrModoXH = listaGrModo.Where(x => celda.ListaGrupocodiModoOperativo.Contains(x.Grupocodi)).ToList();
                        foreach (var reg in listaGrModoXH)
                        {
                            if (reg.ListaEquicodi.Contains(equicodi))
                            {
                                grupocodiModoHoy = reg.Grupocodi;
                            }
                        }
                    }
                }
            }

            //salidas
            fechaUltima = fechaHOHoy != null ? fechaHOHoy : fechaHOAyer;
            grupocodiModo = fechaHOHoy != null ? grupocodiModoHoy : grupocodiModoAyer;
        }

        /// <summary>
        /// ver funcion GenerarIndisponibilidadesParcialesYPrPrevista(
        /// ver funcion ObtenerDatosPrToIndReporteDetDTO  INDAppServicio
        /// </summary>
        /// <param name="fenergcodi"></param>
        /// <param name="listaAllEquicodi"></param>
        /// <param name="listaEquicodiDisp"></param>
        /// <param name="listaModoOC"></param>
        /// <returns></returns>
        private static List<int> ListarModoSegunListaEquipos(int fenergcodi, List<int> listaAllEquicodi, List<int> listaEquicodiDisp, List<PrGrupoDTO> listaModoOC)
        {
            //Lista de modos que se restaran a la central
            List<int> listaModoDisp = new List<int>();

            //obtener todos los modos que tiene equipos para mantto, Rfria
            List<PrGrupoDTO> listaModoXUnidad = listaModoOC.Where(x => (x.Fenergcodi == fenergcodi || fenergcodi == -1)
                                                                && x.ListaEquicodi.Any(y => listaAllEquicodi.Contains(y))).OrderByDescending(x => x.Potencia).ToList();

            foreach (var regModo in listaModoXUnidad)
            {
                if (regModo.TieneModoEspecial)
                {
                    if (listaEquicodiDisp.Any() && regModo.ListaEquicodi.Any(y => listaEquicodiDisp.Contains(y)))
                    {
                        listaModoDisp.Add(regModo.Grupocodi);
                        listaEquicodiDisp = listaEquicodiDisp.Where(x => !regModo.ListaEquicodi.Contains(x)).ToList();
                    }
                }
                else
                {
                    if (listaEquicodiDisp.Any() && regModo.ListaEquicodi.All(y => listaEquicodiDisp.Contains(y)))
                    {
                        listaModoDisp.Add(regModo.Grupocodi);
                        listaEquicodiDisp = listaEquicodiDisp.Where(x => !regModo.ListaEquicodi.Contains(x)).ToList();
                    }
                }
            }

            return listaModoDisp.Distinct().ToList();
        }

        /// <summary>
        /// Obtener descripción de la hora de operación (aparece como comentario en excel)
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        private static string GetDescripcionHo(EveHoraoperacionDTO reg)
        {
            string separador = "> ";
            string desc = (reg.Gruponomb ?? "").Trim() + separador + reg.Hophorini.Value.ToString(ConstantesAppServicio.FormatoHHmmss) + " - " + reg.Hophorfin.Value.ToString(ConstantesAppServicio.FormatoHHmmss);

            desc += string.Format("{0} {1}", separador, (reg.Subcausadesc ?? "").ToString());

            ////
            //if (!string.IsNullOrEmpty(reg.HophorordarranqDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "O. Arranque", reg.HophorordarranqDesc);
            //if (!string.IsNullOrEmpty(reg.HophorparadaDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "O. Parada", reg.HophorparadaDesc);

            ////if (!string.IsNullOrEmpty(reg.Gruponomb))
            ////    desc += string.Format(" {0} {1}: {2}", separador, "Modo de operación", reg.Gruponomb.Trim());

            ////
            //if (!string.IsNullOrEmpty(reg.HopplenacargaDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "Plena carga", reg.HopplenacargaDesc);
            //if (!string.IsNullOrEmpty(reg.HopensayopeDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "Ensayo de Potencia efectiva", reg.HopensayopeDesc);
            //if (!string.IsNullOrEmpty(reg.HopsaisladoDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "Sistema aislado", reg.HopsaisladoDesc);

            ////
            //if (reg.Hopcausacodi > 0)
            //    desc += string.Format("{0} {1}: {2}", separador, " Motivo Operación Forzada", reg.HopcausacodiDesc.ToString());
            //if (!string.IsNullOrEmpty(reg.HoplimtransDesc))
            //    desc += string.Format("{0} {1}", separador, reg.HoplimtransDesc);
            //if (!string.IsNullOrEmpty(reg.HopcompordarrqDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "Compensar O. Arranque", "SÍ");
            //if (!string.IsNullOrEmpty(reg.HopcompordpardDesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "Compensar O. Parada", "SÍ");

            //if (!string.IsNullOrEmpty(reg.Hopdesc))
            //    desc += string.Format("{0} {1}: {2}", separador, "Descripción", reg.Hopdesc.ToString());
            //if (!string.IsNullOrEmpty(reg.Hopobs))
            //    desc += string.Format("{0} {1}: {2}", separador, "Observación del agente", reg.Hopobs.ToString());


            //if (!string.IsNullOrEmpty(reg.LastdateDesc))
            //    desc += string.Format("{0} {1} {2}", separador, "Registrado por ", reg.Lastuser + " " + reg.LastdateDesc);

            return desc;
        }

        private static void GetValorEficienteXH(Celda30minCDispatch regCeldaTmp, out decimal efiGas, out decimal efiCarbon)
        {
            var lRfriaGas = regCeldaTmp.ListaDatoRfriaXFenerg.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas).ToList();
            var lRrotGas = regCeldaTmp.ListaDatoRotanteXFenerg.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas).ToList();

            var lRfriaCarbon = regCeldaTmp.ListaDatoRfriaXFenerg.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiCarbon).ToList();
            var lRrotCarbon = regCeldaTmp.ListaDatoRotanteXFenerg.Where(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiCarbon).ToList();

            efiGas = lRfriaGas.Sum(x => x.Valor); //+ lRrotGas.Sum(x => x.Valor);
            efiCarbon = lRfriaCarbon.Sum(x => x.Valor) + lRrotCarbon.Sum(x => x.Valor);
        }

        private static void GetValorRfriaDespachoXH(Celda30minCDispatch regCeldaTmp, int h, ref List<MeMedicion48DTO> listaRFriaXFenerg)
        {
            foreach (var fenerg in listaRFriaXFenerg)
            {
                var lRfria = regCeldaTmp.ListaDatoRfriaXFenerg.Where(x => x.Fenergcodi == fenerg.Fenergcodi).ToList();
                var rfria = lRfria.Sum(x => x.Valor);

                fenerg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(fenerg, rfria);
            }
        }

        private static MeMedicion48DTO GetConstructorRF(int tipoRF, DateTime f1, int grupocodi, int ptomedicodi, int emprcodi, int fenergcodi, int ctgdetcodi)
        {
            MeMedicion48DTO mrfTotal = new MeMedicion48DTO
            {
                Grupocodi = grupocodi,
                Ptomedicodi = ptomedicodi,
                Emprcodi = emprcodi,
                Fenergcodi = fenergcodi, //fuente de energia
                TipoReservaFria = tipoRF,
                Medifecha = f1,
                Ctgdetcodi = ctgdetcodi //categoria de equipo
            };

            return mrfTotal;
        }

        private static void SetMeditotalXLista(MeMedicion48DTO regTotal, List<MeMedicion48DTO> lista)
        {
            decimal totalReg = 0;
            for (int h = 1; h <= 48; h++)
            {
                decimal total = ((decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regTotal, null)).GetValueOrDefault(0);

                foreach (var m48 in lista)
                {
                    decimal? valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total += valor.GetValueOrDefault(0);
                }

                regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regTotal, total);

                totalReg += total;
            }

            regTotal.Meditotal = totalReg;
        }

        /// <summary>
        /// Verificación de equipo si tiene mantto
        /// </summary>
        /// <param name="h"></param>
        /// <param name="listaEquicodi"></param>
        /// <param name="mantos"></param>
        /// <param name="existeMantto"></param>
        /// <param name="listaManttoDesc"></param>
        private static void VerificaEqbyIndisponibilidadXMantto(int h, List<int> listaEquicodi, List<EveManttoDTO> mantos, out bool existeMantto, out List<string> listaManttoDesc)
        {
            existeMantto = false;
            listaManttoDesc = new List<string>();

            foreach (var eq in listaEquicodi)
            {
                var listaMantto = mantos.Where(x => x.Equicodi == eq).ToList();

                foreach (var veri in listaMantto)
                {
                    var strIniHora = veri.Evenini.Value.ToString(ConstantesAppServicio.FormatoHora);
                    var strFinHora = veri.Evenfin.Value.ToString(ConstantesAppServicio.FormatoHora) != "00:00" ? veri.Evenfin.Value.ToString(ConstantesAppServicio.FormatoHora) : "23:59";
                    string sDescMantto = strIniHora + "-" + strFinHora + "." + veri.Evendescrip;

                    DateTime f1 = veri.Evenini.Value, f2 = veri.Evenfin.Value;
                    int v1 = (int)f1.TimeOfDay.TotalMinutes / 30;
                    int v2 = (f2.TimeOfDay.TotalMinutes == 0 ? 48 : (int)f2.TimeOfDay.TotalMinutes / 30);

                    if (v1 <= (h - 1) && (h - 1) < v2)
                    {
                        existeMantto = true;
                        listaManttoDesc.Add(sDescMantto);
                    }
                }
            }
        }

        /// <summary>
        /// ListarDisponibilidadCombustibleXDia
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaStock"></param>
        /// <returns></returns>
        public static List<CDispatchDisponibilidadGas> ListarDisponibilidadCombustibleXDia(DateTime fechaPeriodo, List<MeMedicion1DTO> listaStock)
        {
            List<CDispatchDisponibilidadGas> lista = new List<CDispatchDisponibilidadGas>
            {
                //C.T. AGUAYTIA (TG1 y TG2)
                new CDispatchDisponibilidadGas()
                {
                    Fecha = fechaPeriodo,
                    ListaGrupoDespachoOrden = new List<int>() { 138, 139 },
                    ListaGrupocodiCentral = new List<int>() { 28 },
                },

                //C.T. MALACAS 1 (TG6), C.T. MALACAS 2 (TG4), C.T. RF DE GENERACION TALARA (TG5)
                new CDispatchDisponibilidadGas()
                {
                    Fecha = fechaPeriodo,
                    ListaGrupoDespachoOrden = new List<int>() { 671, 115, 439 },
                    ListaGrupocodiCentral = new List<int>() { 10 },
                }
            };

            //por cada gaseoducto obtener el combustible
            foreach (var gaseoducto in lista)
            {
                var listaM1Dia = listaStock.Where(x => gaseoducto.ListaGrupocodiCentral.Contains(x.Grupocodi)).ToList();
                if (listaM1Dia.Any())
                    gaseoducto.CombustibleGasStockInicial = listaM1Dia.Sum(x => x.H1 ?? 0);
            }

            return lista;
        }

        /// <summary>
        /// Obtener la potencia minima, maxima y fuente de energia por equipo
        /// </summary>
        /// <param name="regEq"></param>
        /// <param name="lPotenciaByEquipo"></param>
        /// <param name="pmax"></param>
        /// <param name="pmin"></param>
        /// <param name="fenergcodi"></param>
        /// <param name="gruponombCS"></param>
        /// <param name="tieneModoOpDefecto"></param>
        /// <param name="esUnidadModoEspecial"></param>
        public static void GetPropEquipoCdispatch(EqEquipoDTO regEq, List<EqEquipoDTO> lPotenciaByEquipo
                            , out decimal? pmax, out decimal? pmin, out int fenergcodi, out string gruponombCS, out bool tieneModoOpDefecto, out bool esUnidadModoEspecial)
        {
            pmax = 0;
            pmin = 0;
            fenergcodi = 0;
            gruponombCS = "";
            tieneModoOpDefecto = true;
            esUnidadModoEspecial = false;

            if (regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico || regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorSolar || regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorEolica)
            {
                var regEqPot = lPotenciaByEquipo.Find(x => x.Grupocodi == regEq.Grupocodi && x.Equicodi == regEq.Equicodi);
                pmax = regEqPot != null ? regEqPot.Pmax : 0.0m;
                pmin = regEqPot != null ? regEqPot.Pmin : 0.0m;
                fenergcodi = regEqPot != null ? regEqPot.Fenergcodi : 0;
                if (regEq.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico)
                {
                    tieneModoOpDefecto = regEqPot == null || regEqPot.TieneModoOpDefecto;
                    esUnidadModoEspecial = regEqPot != null && regEqPot.EsUnidadModoEspecial;
                    gruponombCS = regEqPot != null ? regEqPot.GruponombCS : "";
                }
            }
            else
            {
                var regEqPot = lPotenciaByEquipo.Find(x => x.Equicodi == regEq.Equicodi);
                pmax = regEqPot != null ? regEqPot.Pmax : 0.0m;
                pmin = regEqPot != null ? regEqPot.Pmin : 0.0m;
                fenergcodi = regEqPot != null ? regEqPot.Fenergcodi : 0;
            }

            if (fenergcodi <= 0)
                fenergcodi = regEq.Fenergcodi;
        }

        /// <summary>
        /// Obtener la lista de propiedad de sincronizacion de las unidades
        /// </summary>
        /// <param name="fechaFinal"></param>
        /// <param name="equicodi"></param>
        /// <param name="listaPropequi"></param>
        /// <returns></returns>
        public static EqPropequiDTO GetPropiedadTiempoSincronizacion(DateTime fechaFinal, int equicodi, List<EqPropequiDTO> listaPropequi)
        {
            List<EqPropequiDTO> listaPropiedad = listaPropequi.Where(x => x.Fechapropequi < fechaFinal.AddDays(1) && (x.Propcodi == 197 || x.Propcodi == 301)).ToList();
            EqPropequiDTO reg = listaPropiedad.Find(x => x.Equicodi == equicodi);

            if (reg != null)
            {
                if (decimal.TryParse((reg.Valor ?? string.Empty), out decimal tiempo))
                {
                    reg.ValorDecimal = tiempo;
                    if (tiempo >= 0)
                    {
                        int hora = Decimal.ToInt32((tiempo / 60.0m));
                        int minuto = Decimal.ToInt32(tiempo - (hora * 60));
                        int segundo = Decimal.ToInt32((tiempo - (hora * 60) - minuto) * 60);
                        reg.ValorDesc = hora.ToString("00") + ":" + minuto.ToString("00") + ":" + segundo.ToString("00");
                    }
                }
            }

            return reg;
        }

        /// <summary>
        /// ListarPropiedadXUnidad
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaEquipo"></param>
        /// <param name="listaPropequi"></param>
        /// <returns></returns>
        public static List<EqEquipoDTO> ListarPropiedadXUnidad(DateTime fechaPeriodo, List<EqEquipoDTO> listaEquipo, List<EqPropequiDTO> listaPropequi)
        {
            List<EqEquipoDTO> lista = new List<EqEquipoDTO>();
            foreach (var reg in listaEquipo)
            {
                EqEquipoDTO eq = new EqEquipoDTO
                {
                    Equicodi = reg.Equicodi,
                    Equinomb = reg.Equinomb,
                    Famcodi = reg.Famcodi,
                    Grupocodi = reg.Grupocodi.GetValueOrDefault(0),
                    Fenergcodi = reg.Fenergcodi,
                    TieneModoOpDefecto = true,
                    EsUnidadModoEspecial = false,
                    Equiestado = reg.Equiestado,
                    Equipadre = (reg.Famcodi == ConstantesHorasOperacion.IdTipoSolar || reg.Famcodi == ConstantesHorasOperacion.IdTipoEolica || reg.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica || reg.Famcodi == ConstantesHorasOperacion.IdTipoTermica) ? reg.Equicodi : reg.Equipadre
                };

                lista.Add(eq);
            }

            foreach (var d in lista)
            {
                if (d.Grupocodi == 138)
                { }
                if (d.Equicodi == 1)
                { }

                decimal valorPE = 0;
                decimal valorPMin = 0;
                int fenergcodi = d.Fenergcodi;

                switch (d.Famcodi)
                {
                    case ConstantesHorasOperacion.IdGeneradorHidroelectrico:

                        List<EqPropequiDTO> listaPEHidroMax = listaPropequi.Where(x => x.Propcodi == ConstantesPR5ReportesServicio.PropPotEfecHidroGen).ToList();
                        valorPE = ReporteMedidoresAppServicio.GetPotInstaladaOrEfectivaByEquipo(listaPEHidroMax, d.Equicodi, fechaPeriodo);

                        List<EqPropequiDTO> listaPEHidroMin = listaPropequi.Where(x => x.Propcodi == ConstantesPR5ReportesServicio.PropPotMinHidroGen).ToList();
                        valorPMin = ReporteMedidoresAppServicio.GetPotInstaladaOrEfectivaByEquipo(listaPEHidroMin, d.Equicodi, fechaPeriodo);
                        break;

                    case ConstantesHorasOperacion.IdGeneradorSolar:
                    case ConstantesHorasOperacion.IdTipoSolar:

                        List<EqPropequiDTO> listaPESolar = listaPropequi.Where(x => x.Propcodi == ConstantesPR5ReportesServicio.PropPotEfectSolar).ToList();
                        int equicodiSolar = d.Equipadre > 0 ? d.Equipadre.Value : d.Equicodi;
                        valorPE = ReporteMedidoresAppServicio.GetPotInstaladaOrEfectivaByEquipo(listaPESolar, equicodiSolar, fechaPeriodo);
                        break;

                    case ConstantesHorasOperacion.IdGeneradorEolica:
                    case ConstantesHorasOperacion.IdTipoEolica:
                        List<EqPropequiDTO> listaPEEolica = listaPropequi.Where(x => x.Propcodi == ConstantesPR5ReportesServicio.PropPotEfecEolica).ToList();
                        int equicodiEolico = d.Equipadre > 0 ? d.Equipadre.Value : d.Equicodi;
                        valorPE = ReporteMedidoresAppServicio.GetPotInstaladaOrEfectivaByEquipo(listaPEEolica, equicodiEolico, fechaPeriodo);

                        break;
                }

                d.Pe = valorPE;
                d.Pmax = valorPE;
                d.Pmin = valorPMin;
                d.Fenergcodi = fenergcodi;
            }

            return lista.Where(x => x.Pe != null || x.Pmax != null).ToList();
        }

        /// <summary>
        /// Obtener el evenclase segun la lectura
        /// </summary>
        /// <param name="lectcodiOrig"></param>
        /// <returns></returns>
        public static int GetEvenclasecodiByLectcodi(int lectcodiOrig)
        {
            int evenclasecodi = 1;

            string lectcodi = lectcodiOrig + string.Empty;
            switch (lectcodi)
            {
                case ConstantesAppServicio.LectcodiEjecutadoHisto: //6
                case ConstantesAppServicio.LectcodiEjecutado: //93
                case ConstantesAppServicio.LectcodiReprogDiario: //5
                    evenclasecodi = ConstantesAppServicio.EvenclasecodiEjecutado;
                    break;
                case ConstantesAppServicio.LectcodiProgDiario: //4
                case ConstantesAppServicio.LectcodiAjusteDiario: //7
                    evenclasecodi = ConstantesAppServicio.EvenclasecodiProgDiario;
                    break;
                case ConstantesAppServicio.LectcodiProgSemanal: //3
                    evenclasecodi = ConstantesAppServicio.EvenclasecodiProgSemanal;
                    break;
            }

            return evenclasecodi;
        }

        /// <summary>
        /// ListaEquicodiCamisea
        /// </summary>
        /// <returns></returns>
        public static List<int> ListaEquicodiCamisea()
        {
            return new List<int>() { 11682, 11681 };
        }

        /// <summary>
        /// ListarObjYupanaHoxM48
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="listaAllGrupo"></param>
        /// <param name="hopcodi"></param>
        /// <returns></returns>
        public static List<EveHoraoperacionDTO> ListarObjYupanaHoxM48(CpMedicion48DTO reg, List<PrGrupoDTO> listaAllGrupo, ref int hopcodi)
        {
            List<EveHoraoperacionDTO> l = new List<EveHoraoperacionDTO>();

            int grupocodiModo = reg.Recurcodisicoes;
            PrGrupoDTO regModo = listaAllGrupo.Find(x => x.Grupocodi == grupocodiModo);
            string gruponomb = regModo != null ? regModo.Gruponomb : string.Empty;

            List<List<DateTime>> agrupSubListaXmodo = new List<List<DateTime>>();
            List<DateTime> sublistaTmp = new List<DateTime>();
            for (int i = 0; i < 48; i++)
            {
                int h = i + 1;
                decimal regActual = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null)).GetValueOrDefault(0);
                decimal regSiguiente = h < 48 ? ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h + 1)).GetValue(reg, null)).GetValueOrDefault(0) : 0;

                DateTime fechaH = reg.Medifecha.AddMinutes(h * 30);

                if (regSiguiente != 0)
                {
                    if (regSiguiente == 0 && regActual > 0)
                    {
                        sublistaTmp.Add(fechaH);
                        agrupSubListaXmodo.Add(sublistaTmp);

                        sublistaTmp = new List<DateTime>();
                    }
                    else
                    {
                        sublistaTmp.Add(fechaH);
                    }
                }

                if (regSiguiente == 0 && regActual > 0)
                {
                    sublistaTmp.Add(fechaH);
                    agrupSubListaXmodo.Add(sublistaTmp);
                }
            }

            foreach (var sublista in agrupSubListaXmodo)
            {
                DateTime f1 = sublista.First().AddMinutes(-29);
                DateTime f2 = sublista.Last();

                EveHoraoperacionDTO ho = new EveHoraoperacionDTO()
                {
                    Hopcodi = hopcodi,
                    FlagTipoHo = ConstantesHorasOperacion.FlagTipoHoModo,
                    Grupocodi = grupocodiModo,
                    Gruponomb = gruponomb,
                    Subcausadesc = "YUPANA",
                    Hophorini = f1,
                    Hophorfin = f2
                };

                l.Add(ho);
                hopcodi++;
            }

            return l;
        }

        #endregion

        #region Cálculo CDispatch (COSTOS DE OPERACION)

        /// <summary>
        /// Consumo de combustible por potencia
        /// </summary>
        /// <param name="param"></param>
        /// <param name="potenciaGenerada"></param>
        /// <returns></returns>
        private static decimal GetConsumoPorMediaHora(ConsumoHorarioCombustible param, decimal potenciaGenerada)
        {
            decimal consumoComb = 0;

            if (param != null)
            {
                int totalPuntos = param.ListaX.Count;
                if (totalPuntos >= 2)
                {
                    decimal potencia = param.ListaX[totalPuntos - 1];
                    decimal consComb = param.ListaY[totalPuntos - 1];

                    decimal potenciaAnt = 0;
                    decimal consCombAnt = 0;

                    bool iter1 = true;
                    for (int i = totalPuntos - 2; i >= 0 && iter1; i--)
                    {
                        potenciaAnt = potencia;
                        consCombAnt = consComb;

                        potencia = param.ListaX[i];
                        consComb = param.ListaY[i];
                        if (potencia > potenciaGenerada)
                        {
                            iter1 = false;
                        }
                    }

                    consumoComb = consComb - (potencia - potenciaGenerada) * (consComb - consCombAnt) / (potencia - potenciaAnt);
                }
            }

            return consumoComb / 2.0m;
        }

        /// <summary>
        /// DiferenciaCostoOperacion
        /// </summary>
        /// <param name="fechaPeriodo"></param>
        /// <param name="listaActual"></param>
        /// <param name="listaNuevo"></param>
        /// <param name="hayDiferencia"></param>
        /// <param name="costoActual"></param>
        /// <param name="costoNuevo"></param>
        public static void DiferenciaCostoOperacion(DateTime fechaPeriodo, List<MeMedicion1DTO> listaActual, List<MeMedicion1DTO> listaNuevo,
                                    out bool hayDiferencia, out decimal costoActual, out decimal costoNuevo)
        {
            var regActual = listaActual.Find(x => x.Medifecha == fechaPeriodo && x.Ptomedicodi == ConstantesAppServicio.PtomedicodiCostoOpeDia) ?? new MeMedicion1DTO();
            var regNuevo = listaNuevo.Find(x => x.Medifecha == fechaPeriodo && x.Ptomedicodi == ConstantesAppServicio.PtomedicodiCostoOpeDia) ?? new MeMedicion1DTO();

            //la base de datos permite 5 decimales
            costoActual = Math.Round(regActual.H1 ?? 0, 5);
            costoNuevo = Math.Round(regNuevo.H1 ?? 0, 5);

            hayDiferencia = costoActual != costoNuevo;
        }

        /// <summary>
        /// ListarCostoVariableEvaluacionFormula
        /// </summary>
        /// <param name="listaGrupoModo"></param>
        /// <param name="lParametrosGenerales"></param>
        /// <param name="lParametrosFuncionales"></param>
        /// <returns></returns>
        public static List<PrCvariablesDTO> ListarCostoVariableEvaluacionFormula(List<PrGrupoDTO> listaGrupoModo, List<PrGrupodatDTO> lParametrosGenerales, List<PrGrupodatDTO> lParametrosFuncionales)
        {
            //por cada modo de operación con operación comercial
            List<PrCvariablesDTO> listaCostoVariableDia = new List<PrCvariablesDTO>();
            foreach (PrGrupoDTO oGrupoFunc in listaGrupoModo)
            {
                if (oGrupoFunc.Grupocodi == 3449)
                { }
                int grupocodi = oGrupoFunc.Grupocodi; //codigo modo
                int grupopadre = oGrupoFunc.Grupopadre.Value; //codigo grupo termico
                int grupocentral = oGrupoFunc.GrupoCentral; //codigo central

                Base.Tools.n_parameter lparamXModo = new Base.Tools.n_parameter();

                //Parámetros generales del sistema
                foreach (PrGrupodatDTO drParam in lParametrosGenerales)
                {
                    lparamXModo.SetData((drParam.Concepabrev ?? "").Trim(), (drParam.Formuladat ?? "").Trim());
                }

                //Parámetros del modo, grupo térmico y de la central
                List<PrGrupodatDTO> lParametrosFuncionalesParaElModo = lParametrosFuncionales.Where(x => x.Grupocodi == grupocodi || x.Grupocodi == grupopadre || x.Grupocodi == grupocentral).ToList();
                foreach (PrGrupodatDTO drLast in lParametrosFuncionalesParaElModo)
                {
                    if (drLast.Concepabrev == null || drLast.Formuladat == null)
                    { }
                    lparamXModo.SetData((drLast.Concepabrev ?? "").Trim(), (drLast.Formuladat ?? "").Trim());
                }

                //variables para el costo de la operación en dolares
                PrCvariablesDTO regCV = new PrCvariablesDTO
                {
                    Grupocodi = oGrupoFunc.Grupocodi
                };

                //TCambio (S/.) Tipo de Cambio	(1)
                ObtenerValorFormula(lparamXModo, "TCambio", out decimal valor1, out string msj1);
                regCV.TCambio = valor1;

                //CVNC_US (US$ /KWh) CVNC US $	(26)
                ObtenerValorFormula(lparamXModo, "CVNC_US", out decimal valor2, out string msj2);
                regCV.CvncUS = valor2;

                //CMarrPar (S/./Arranq) Costo de Mantto por Arranque-Parada (186)
                ObtenerValorFormula(lparamXModo, "CMARRPAR", out decimal valor3, out string msj3);
                regCV.CMarrPar = valor3 / regCV.TCambio;

                //CComb (S/./) Costo Combustible (22)
                ObtenerValorFormula(lparamXModo, "CComb", out decimal valor4, out string msj4);
                regCV.Ccomb = valor4 / regCV.TCambio;

                //CCombAlt (S/./) Costo Combustible Alternativo	(240)
                ObtenerValorFormula(lparamXModo, "CCombAlt", out decimal valor5, out string msj5);
                regCV.CcombAlt = valor5 / regCV.TCambio;

                //PCI_SI (kJ/m3) Poder Calorífico Inferior/central (192)
                ObtenerValorFormula(lparamXModo, "PCI_SI", out decimal valor6, out string msj6);
                regCV.PciSI = valor6 / 1000000.0m;

                if (oGrupoFunc.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas ||
                    oGrupoFunc.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiRef ||
                    oGrupoFunc.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiNR ||
                    oGrupoFunc.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiFLE ||
                    oGrupoFunc.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiFG ||
                    oGrupoFunc.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiNFG)
                {
                    regCV.Ccomb *= regCV.PciSI;
                }

                //CombArSin (uni) Comb arraque hasta sincronismo (149)
                ObtenerValorFormula(lparamXModo, "CombArSin", out decimal valor20, out string msj20);
                //CombSinPC (uni) Comb sincronismo hasta PC	(354)
                ObtenerValorFormula(lparamXModo, "CombSinPC", out decimal valor21, out string msj21);
                //ComPCSinc [concepto No existe en BD]
                ObtenerValorFormula(lparamXModo, "ComPCSinc", out decimal valor22, out string msj22);
                //ComSinPar (uni) Combustible fuera sincronismo parada	(160)
                ObtenerValorFormula(lparamXModo, "ComSinPar", out decimal valor23, out string msj23);
                regCV.Cbe = valor20 + valor21 + valor22 + valor23;

                //CombArSinAlt (gal) Comb arranque hasta sincronismo alternativo (241)
                ObtenerValorFormula(lparamXModo, "CombArSinAlt", out decimal valor30, out string msj30);
                //CombSinPCAlt (gal) Comb sincronismo hasta PM alternativo (244)
                ObtenerValorFormula(lparamXModo, "CombSinPCAlt", out decimal valor31, out string msj31);
                //ComPCSincAlt (gal) Combustible PM hasta fuera de sincronismo alternativo (245)
                ObtenerValorFormula(lparamXModo, "ComPCSincAlt", out decimal valor32, out string msj32);
                //ComSinParAlt (gal) Combustible fuera sincronismo parada alternativo (242)
                ObtenerValorFormula(lparamXModo, "ComSinParAlt", out decimal valor33, out string msj33);
                regCV.CbeAlt = valor30 + valor31 + valor32 + valor33;

                listaCostoVariableDia.Add(regCV);
            }

            return listaCostoVariableDia;
        }

        private static void ObtenerValorFormula(Base.Tools.n_parameter lparamXModo, string concepto, out decimal valor, out string mensaje)
        {
            valor = 0;
            mensaje = "";

            if (lparamXModo.Contains(concepto))
            {
                try
                {
                    var sValor = lparamXModo.GetEvaluate(concepto); //EvaluateFormula
                    valor = Convert.ToDecimal(sValor);

                }
                catch (Exception)
                {
                    //error al evaluar la fórmula
                }
            }
            else
            {
                mensaje = "No existe fórmula para el concepto " + concepto;
            }
        }

        #endregion

        #region Presentación CDispatch - Lógica

        /// <summary>
        /// AsignarVariablesCalculoXUnidadGeneracion
        /// </summary>
        /// <param name="regCDespacho"></param>
        public static void AsignarVariablesCalculoXUnidadGeneracion(ref CDespachoGlobal regCDespacho)
        {
            DateTime fechaIni = regCDespacho.FechaIni;
            DateTime fechaFin = regCDespacho.FechaFin;

            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                var regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == fecha);

                //Obtener datos cada 30 minutos para futuros cálculos de reserva fría y costo de operación
                regCDespachoXDia.ListaDatoX30min = UtilCdispatch.GenerarDatos30min(regCDespachoXDia);
            }
        }

        /// <summary>
        /// AsignarVariablesGlobalADia
        /// </summary>
        /// <param name="regInput"></param>
        /// <param name="regCDespacho"></param>
        public static void AsignarVariablesPlantillaCDispatch(CDespachoInput regInput, ref CDespachoGlobal regCDespacho)
        {
            DateTime fechaIni = regCDespacho.FechaIni;
            DateTime fechaFin = regCDespacho.FechaFin;
            var listaCatecodiConData = regCDespacho.ListaCatecodiConData;
            int lectcodi = regCDespacho.Lectcodi;

            //variables globales del rango de consulta
            List<MePtomedicionDTO> listaAllPtoTmp = new List<MePtomedicionDTO>();
            List<PrGrupoDTO> listaAllGrupoTmp = new List<PrGrupoDTO>();

            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                CDespachoDiario regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == fecha);

                //1. Generación de Potencia Activa del día. Se asume que la data ya tiene la empresa para el día
                regCDespachoXDia.ListaMe48XDiaMW = regCDespachoXDia.ListaMe48XDia.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW).ToList();

                //2. Listar grupos que tienen data en el día. Determinar si es COES o No COES (GRUPOINTEGRANTE)
                var listaM48Tmp = regCDespachoXDia.ListaMe48XDiaMW.Where(x => listaCatecodiConData.Contains(x.Catecodi)).ToList();
                List<int> listaGrXData = listaM48Tmp.Where(x => x.Grupocodi > 0).Select(x => x.Grupocodi).Distinct().ToList();

                foreach (var grupocodi in listaGrXData)
                {
                    if (grupocodi == 3430)
                    { }
                    var reg = regCDespacho.ListaGrupoXGrupoDespachoHistorico.Find(x => x.Grupocodi == grupocodi);
                    var reg48 = listaM48Tmp.Find(x => x.Grupocodi == grupocodi);
                    bool existeObj = regCDespachoXDia.ListaGrupoXDia.Find(x => x.Grupocodi == grupocodi) != null;

                    if (reg == null)  //el grupo se encuentra eliminado o anulado
                    {
                        var reg48Elim = reg48 ?? new MeMedicion48DTO();
                        reg = new PrGrupoDTO()
                        {
                            Grupocodi = grupocodi,
                            Gruponomb = "ELIMINADO " + reg48Elim.Gruponomb,
                            Grupoabrev = "X",
                            Grupointegrante = reg48Elim.Grupointegrante,
                            Grupotipocogen = reg48Elim.Grupotipocogen,
                            TipoGenerRer = reg48Elim.Tipogenerrer,
                            Fenergcodi = reg48Elim.Fenergcodi,
                            Tgenercodi = reg48Elim.Tgenercodi
                        };
                        int ptomedicodi = reg48Elim.Ptomedicodi;

                        var regMsj3 = new ResultadoValidacionAplicativo()
                        {
                            TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                            Descripcion = string.Format("El grupo {0} {1} asociado al punto de medición {2} está eliminado. Se debe cambiar el estado a Baja o cambiar el grupo del punto de medición.", reg.Grupocodi, reg.Gruponomb, ptomedicodi)
                        };

                        regCDespachoXDia.ListaMensajeValidacionXDia.Add(regMsj3);
                    }

                    if (reg != null && reg48 != null && !existeObj)
                    {
                        int emprcodi = reg48.Emprcodi;
                        int ptomedicodi = reg48.Ptomedicodi;
                        List<int> lPtomedicodi = listaM48Tmp.Where(x => x.Grupocodi == grupocodi).Select(x => x.Ptomedicodi).Distinct().ToList();

                        AgregarPtoAReporteTmp(reg, emprcodi, ptomedicodi, lPtomedicodi, fecha, ref regCDespacho, ref regCDespachoXDia);
                    }
                }

                //3. Incluir grupos que no tienen data pero sí estado "A" en PR_GRUPO. Activo significa que forman parte de la plantilla del CDispatch (GRUPOACTIVO)
                List<int> listaGrActivo = regCDespacho.ListaGrupoXGrupoDespachoActivo.Where(x => x.Grupoactivo == ConstantesAppServicio.SI).Select(x => x.Grupocodi).ToList();
                listaGrActivo = listaGrActivo.Where(x => !listaGrXData.Contains(x)).ToList();
                foreach (var grupocodi in listaGrActivo)
                {
                    if (grupocodi == 3430)
                    { }
                    var reg = regCDespacho.ListaGrupoXGrupoDespachoActivo.Find(x => x.Grupocodi == grupocodi);
                    if (reg != null)
                    {
                        MePtomedicionDTO pto = regCDespacho.ListaPtoXGrupoDespachoValido.Find(x => x.Grupocodi == reg.Grupocodi);

                        if (pto != null)
                        {
                            int emprcodi = pto.Emprcodi ?? 0;
                            int ptomedicodi = pto.Ptomedicodi;
                            List<int> lPtomedicodi = regCDespacho.ListaPtoXGrupoDespachoValido.Where(x => x.Grupocodi == reg.Grupocodi).Select(x => x.Ptomedicodi).Distinct().ToList();

                            AgregarPtoAReporteTmp(reg, emprcodi, ptomedicodi, lPtomedicodi, fecha, ref regCDespacho, ref regCDespachoXDia);
                        }
                    }
                }

                //4. Listar Puntos de medición de los grupos del día [Plantilla Generación] 
                if (regInput.Empresas != null && regInput.Empresas != ConstantesAppServicio.ParametroDefecto)
                {
                    List<int> listaEmprcodis = regInput.Empresas.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    regCDespachoXDia.ListaPtoXDia = regCDespachoXDia.ListaPtoXDia.Where(x => listaEmprcodis.Contains(x.Emprcodi ?? 0)).ToList();
                }
                regCDespachoXDia.ListaPtoXDiaOC = regCDespachoXDia.ListaPtoXDia.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();

                //5. Totalidad de puntos de medición Excel [Plantilla Generación, Flujos, Calculados]
                regCDespachoXDia.ListaGrupoXDiaCabeceraExcel = new List<PrGrupoDTO>();
                regCDespachoXDia.ListaGrupoXDiaCabeceraExcel.AddRange(OrdenarListaGrupoCDispatch(regCDespachoXDia.ListaGrupoXDia));
                regCDespachoXDia.ListaGrupoXDiaCabeceraExcel.AddRange(regCDespacho.ListaCabeceraCdispatchActual.Where(x => x.Equiabrev != "").ToList());

                //lista tmp
                listaAllPtoTmp.AddRange(regCDespachoXDia.ListaPtoXDia);
                listaAllGrupoTmp.AddRange(regCDespachoXDia.ListaGrupoXDia);
            }

            regCDespacho.ListaAllGrupoPlantilla = listaAllGrupoTmp.GroupBy(x => x.Grupocodi).Select(x => x.First()).ToList();
            regCDespacho.ListaAllPtoPlantilla = listaAllPtoTmp.GroupBy(x => new { x.Ptomedicodi }).Select(x => x.Last()).ToList(); //para reportes excel de Reserva fria en modulo distinto a migraciones
            regCDespacho.ListaAllPtoPlantillaExcel = listaAllPtoTmp.GroupBy(x => new { x.Ptomedicodi, x.Emprcodi, x.Grupointegrante }).Select(x => x.First()).ToList();

            //cabecera web
            string[] arr = ConstantesMigraciones.CabeceraCDisptachWeb.Split(',');

            regCDespacho.ListaGrupoCabeceraWeb = new List<PrGrupoDTO>();
            foreach (var desc in arr)
                regCDespacho.ListaGrupoCabeceraWeb.Add(new PrGrupoDTO() { GruponombWeb = desc });
            regCDespacho.ListaGrupoCabeceraWeb.AddRange(regCDespacho.ListaAllGrupoPlantilla);

            //Ordenamiento
            regCDespacho.ListaAllPtoPlantilla = OrdenarListaPtoCDispatch(regCDespacho.ListaAllPtoPlantilla);
            regCDespacho.ListaGrupoCabeceraWeb = OrdenarListaGrupoCDispatch(regCDespacho.ListaGrupoCabeceraWeb);

            //ordenamiento TTIE e integrantes COES/ No COES
            regCDespacho.ListaAllPtoPlantillaExcel = OrdenarListaPtoAnexoA(regCDespacho.ListaAllPtoPlantillaExcel);
        }

        private static void AgregarPtoAReporteTmp(PrGrupoDTO reg, int emprcodi, int ptomedicodiGr, List<int> lPtomedicodi, DateTime fecha,
            ref CDespachoGlobal regCDespacho, ref CDespachoDiario regCDespachoXDia)
        {
            if (reg.Grupocodi == 101)
            { }

            if (emprcodi > 0 && ptomedicodiGr > 0)
            {
                int lectcodi = regCDespacho.Lectcodi;

                //agregar grupo
                PrGrupoDTO regTmp = (PrGrupoDTO)reg.Clone();
                regTmp.Emprcodi = emprcodi;
                regTmp.Ptomedicodi = ptomedicodiGr;
                regTmp.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(regTmp.Grupocodi, fecha, regTmp.Grupointegrante, regCDespacho.ListaOperacionCoes);
                regTmp.EsGrupogen = true;
                SetDescripcionEmpresaToGrupo(regTmp, regCDespacho.ListaSiEmpresaBD, out ResultadoValidacionAplicativo regMsj);
                if (regMsj != null) regCDespachoXDia.ListaMensajeValidacionXDia.Add(regMsj);

                regCDespachoXDia.ListaGrupoXDia.Add(regTmp);

                //agregar un punto
                if (lectcodi != ConstantesPR5ReportesServicio.LectDespachoEjecutado)
                {
                    var regPto = regCDespacho.ListaPtoXGrupoDespacho.Find(x => x.Ptomedicodi == ptomedicodiGr);
                    if (regPto != null)
                    {
                        MePtomedicionDTO regPtoTmp = (MePtomedicionDTO)regPto.Clone();
                        regPtoTmp.Emprcodi = regTmp.Emprcodi;
                        regPtoTmp.Emprnomb = regTmp.Emprnomb;
                        regPtoTmp.Grupointegrante = regTmp.Grupointegrante;
                        regPtoTmp.Tipogenerrer = regTmp.TipoGenerRer;
                        regPtoTmp.Emprorden = regTmp.Emprorden;
                        regPtoTmp.Grupoorden = regTmp.Grupoorden;
                        regPtoTmp.Gruponomb = regTmp.Gruponomb;
                        regPtoTmp.Grupotipocogen = regTmp.Grupotipocogen;
                        regPtoTmp.Fenergcodi = regTmp.Fenergcodi ?? 0;
                        regPtoTmp.Tgenernomb = regTmp.Tgenernomb;
                        regCDespachoXDia.ListaPtoXDia.Add(regPtoTmp);
                    }
                    else
                    {
                        //TODO el punto no es grupo termico
                    }
                }
                else
                {
                    var listaPtoXGr = regCDespacho.ListaPtoXGrupoDespacho.Where(x => lPtomedicodi.Contains(x.Ptomedicodi)).ToList();

                    //agregar varios puntos
                    foreach (var ptomedicodiEq in lPtomedicodi)
                    {
                        var regPto = listaPtoXGr.Find(x => x.Ptomedicodi == ptomedicodiEq);
                        if (regPto != null)
                        {
                            MePtomedicionDTO regPtoTmp = (MePtomedicionDTO)regPto.Clone();
                            regPtoTmp.Emprcodi = regTmp.Emprcodi;
                            regPtoTmp.Emprnomb = regTmp.Emprnomb;
                            regPtoTmp.Grupointegrante = regTmp.Grupointegrante;
                            regPtoTmp.Tipogenerrer = regTmp.TipoGenerRer;
                            regPtoTmp.Emprorden = regTmp.Emprorden;
                            regPtoTmp.Grupoorden = regTmp.Grupoorden;
                            regPtoTmp.Gruponomb = regTmp.Gruponomb;
                            regPtoTmp.Grupotipocogen = regTmp.Grupotipocogen;
                            regPtoTmp.Fenergcodi = regTmp.Fenergcodi ?? 0;
                            regPtoTmp.Tgenernomb = regTmp.Tgenernomb;
                            regCDespachoXDia.ListaPtoXDia.Add(regPtoTmp);
                        }
                        else
                        {
                            //TODO el punto no es grupo termico
                        }
                    }

                    //validar que todos los puntos del grupo sean de la misma empresa
                    var listaEmprcodiXGr = listaPtoXGr.Select(x => x.Emprcodi).Distinct().ToList();
                    if (listaEmprcodiXGr.Count > 1)
                    {
                        var regMsj2 = new ResultadoValidacionAplicativo()
                        {
                            TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                            Descripcion = string.Format("El grupo {0} {1} tiene puntos de medición ({2}) que pertenecen a distintas empresas.", reg.Grupocodi, reg.Gruponomb, string.Join(",", lPtomedicodi))
                        };

                        regCDespachoXDia.ListaMensajeValidacionXDia.Add(regMsj2);
                    }
                }
            }
        }

        private static List<MePtomedicionDTO> OrdenarListaPtoAnexoA(List<MePtomedicionDTO> listaPto)
        {
            //separar
            var listaNoRerNoCog = listaPto.Where(x => x.Tipogenerrer != ConstantesAppServicio.SI && x.Grupotipocogen != ConstantesAppServicio.SI).ToList();
            var listaRer = listaPto.Where(x => x.Tipogenerrer == ConstantesAppServicio.SI && x.Grupotipocogen == ConstantesAppServicio.NO).ToList();
            var listaCog = listaPto.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI && x.Grupotipocogen != ConstantesAppServicio.NO).ToList();

            List<MePtomedicionDTO> listaOrdenada = new List<MePtomedicionDTO>();
            listaOrdenada.AddRange(OrdenarListaPtoCDispatch(listaNoRerNoCog));
            listaOrdenada.AddRange(OrdenarListaPtoCDispatch(listaRer));
            listaOrdenada.AddRange(OrdenarListaPtoCDispatch(listaCog));

            return listaOrdenada;
        }

        private static List<MePtomedicionDTO> OrdenarListaPtoCDispatch(List<MePtomedicionDTO> listaPto)
        {
            //primero las empresas que no tienen orden, luego los grupos que no tienen orden
            return listaPto.OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ThenBy(x => x.Grupoorden).ThenBy(x => x.Gruponomb).ThenBy(x => x.Equinomb).ToList();
        }

        private static List<PrGrupoDTO> OrdenarListaGrupoCDispatch(List<PrGrupoDTO> listaGr)
        {
            return listaGr.OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ThenBy(x => x.Grupoorden).ThenBy(x => x.Gruponomb).ToList();
        }

        /// <summary>
        /// SetDescripcionEmpresaToGrupo
        /// </summary>
        /// <param name="regGr"></param>
        /// <param name="listaEmp"></param>
        /// <param name="regMsj"></param>
        public static void SetDescripcionEmpresaToGrupo(PrGrupoDTO regGr, List<SiEmpresaDTO> listaEmp, out ResultadoValidacionAplicativo regMsj)
        {
            regMsj = null;
            regGr.Grupoorden = regGr.Grupoorden != null ? regGr.Grupoorden.Value : 9999999;

            var regEmp = listaEmp.Find(x => x.Emprcodi == regGr.Emprcodi);
            if (regEmp != null)
            {
                regGr.Emprorden = regEmp.Emprorden;
                regGr.Emprnomb = regEmp.Emprnomb;
                regGr.Emprabrev = regEmp.Emprabrev;
            }
            else
            {
                regMsj = new ResultadoValidacionAplicativo() { TipoValidacion = ConstantesPR5ReportesServicio.MensajeError, Descripcion = "La empresa " + regGr.Emprcodi + " no existe o no pertenece al SEIN" };
            }

            regGr.GruponombWeb = (regGr.Grupoabrev ?? "").Trim() + " - " + (regGr.Emprabrev ?? "").Trim().ToLower();
            regGr.GruponombWebCompleto = regGr.Grupocodi + " " + (regGr.Gruponomb ?? "").Trim() + " - " + (regGr.Emprnomb ?? "").Trim();
        }

        /// <summary>
        /// FillCabeceraCdispatchHistorico
        /// </summary>
        /// <param name="listaPto"></param>
        /// <param name="listaPtoMediaHora"></param>
        /// <param name="listaPtosReporteFl"></param>
        /// <param name="listaPtosReporteCmg"></param>
        /// <returns></returns>
        public static List<PrGrupoDTO> FillCabeceraCdispatchHistorico(List<MePtomedicionDTO> listaPto, List<MePtomedicionDTO> listaPtoMediaHora, List<MeReporptomedDTO> listaPtosReporteFl, List<MeReporptomedDTO> listaPtosReporteCmg)
        {
            List<PrGrupoDTO> lista = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaTempFl = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaTempCmg = new List<PrGrupoDTO>();

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Totalcogeneracion));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Totalrenov));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Generacioncoes));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Demandancp));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Gtotal));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Perdidasncp));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Deficitncp));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Demandacentroncp));

            listaTempFl.Add(FillMeptomedicion(1, ConstantesMigraciones.FlCentroSur));
            listaTempFl.Add(FillMeptomedicion(2, ConstantesMigraciones.FlChilcaCarapongo));
            listaTempFl.Add(FillMeptomedicion(3, ConstantesMigraciones.FlCarapongoCarabayllo));
            listaTempFl.Add(FillMeptomedicion(4, ConstantesMigraciones.FlChimboteTrujillo));
            listaTempFl.Add(FillMeptomedicion(5, ConstantesMigraciones.FlTrujilloNinia));
            listaTempFl.Add(FillMeptomedicion(6, ConstantesMigraciones.FlColcabambaPoroma));
            listaTempFl.Add(FillMeptomedicion(7, ConstantesMigraciones.FlChilcaPoroma));
            listaTempFl.Add(FillMeptomedicion(8, ConstantesMigraciones.FlPoromaYarabamba));
            listaTempFl.Add(FillMeptomedicion(9, ConstantesMigraciones.FlPoromaOconia));
            listaTempFl.Add(FillMeptomedicion(10, ConstantesMigraciones.FlYarabambaMontalvo));
            listaTempFl.Add(FillMeptomedicion(11, ConstantesMigraciones.FlHuancavelicaIndependencia));
            listaTempFl.Add(FillMeptomedicion(12, ConstantesMigraciones.FlChilcaSanJuan));
            listaTempFl.Add(FillMeptomedicion(13, ConstantesMigraciones.Flcotasoca));
            listaTempFl.Add(FillMeptomedicion(14, ConstantesMigraciones.Flchimbtruj));
            listaTempFl.Add(FillMeptomedicion(15, ConstantesMigraciones.Flsepanuchimb));
            listaTempFl.Add(FillMeptomedicion(16, ConstantesMigraciones.Flpomacochasjuan));
            listaTempFl.Add(FillMeptomedicion(17, ConstantesMigraciones.Fltrujguadalupe));
            listaTempFl.Add(FillMeptomedicion(18, ConstantesMigraciones.Flarmicota));
            listaTempFl.Add(FillMeptomedicion(19, ConstantesMigraciones.Floconsjose));
            listaTempFl.Add(FillMeptomedicion(20, ConstantesMigraciones.Flsjosemonta));
            listaTempFl.Add(FillMeptomedicion(21, ConstantesMigraciones.Fltinnvasoca));
            listaTempFl.Add(FillMeptomedicion(22, ConstantesMigraciones.Flsocamoque));
            listaTempFl.Add(FillMeptomedicion(23, ConstantesMigraciones.Flcarabchimb));
            listaTempFl.Add(FillMeptomedicion(24, ConstantesMigraciones.Flguadareque));
            listaTempFl.Add(FillMeptomedicion(25, ConstantesMigraciones.Flninapiura));
            listaTempFl.Add(FillMeptomedicion(26, ConstantesMigraciones.Flcarabchilca));
            listaTempFl.Add(FillMeptomedicion(27, ConstantesMigraciones.Flsjuanindus));
            listaTempFl.Add(FillMeptomedicion(28, ConstantesMigraciones.Flchilcaplani));
            listaTempFl.Add(FillMeptomedicion(29, ConstantesMigraciones.Flplanicarab));
            listaTempFl.Add(FillMeptomedicion(30, ConstantesMigraciones.Flventachav));
            listaTempFl.Add(FillMeptomedicion(31, ConstantesMigraciones.Flventazapa));
            listaTempFl.Add(FillMeptomedicion(32, ConstantesMigraciones.Flhuanzcarab));

            //>>>>>>>>>>>>>>>>>>>>Flujos de Potencia>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            var listaPuntosFlujos = listaPtoMediaHora.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiFlujo).ToList();

            //si el punto existe en la lista temporal se saca el equinomb y gruponomb
            AgregarListaEQNombres(listaPuntosFlujos, listaTempFl, ref lista, ConstantesMigraciones.ReporteCdispatchFl);

            //si de la lista de puntos del reporte, alguno no existe en la lista final, entonces se agrega
            foreach (var pto in listaPtosReporteFl)
            {
                if (lista.Find(x => x.Ptomedicodi == pto.Ptomedicodi) == null)
                {
                    string cadena = string.Empty;
                    string[] words = { "9992", pto.Ptomedidesc.Trim(), pto.Ptomedicodi.ToString(), "8", "F0", "FLUJO EN LINEAS", pto.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(FillMeptomedicion(9999999, cadena));
                }
            }

            listaTempCmg.Add(FillMeptomedicion(1, ConstantesMigraciones.Cmgsein_ncp));
            listaTempCmg.Add(FillMeptomedicion(2, ConstantesMigraciones.Cmgstarosancp));
            listaTempCmg.Add(FillMeptomedicion(3, ConstantesMigraciones.Cmgtrujillo_ncp));
            listaTempCmg.Add(FillMeptomedicion(4, ConstantesMigraciones.Cmgsocabaya_ncp));
            //>>>>>>>>>>>>>>>>>>>>>>Costos Marginales>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            var listaPuntosCmg = listaPtoMediaHora.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiDolar).ToList();

            //si el punto existe en la lista temporal se saca el equinomb y gruponomb
            AgregarListaEQNombres(listaPuntosCmg, listaTempCmg, ref lista, ConstantesMigraciones.ReporteCdispatchCmg);

            //si de la lista de puntos del reporte, alguno no existe en la lista final, entonces se agrega
            foreach (var pto in listaPtosReporteCmg)
            {
                if (lista.Find(x => x.Ptomedicodi == pto.Ptomedicodi) == null)
                {
                    string cadena = string.Empty;
                    string desc = pto.Ptomedidesc.Replace("_", " ").Trim();
                    desc += "(US$/MWh)";
                    string[] words = { "9993", desc, pto.Ptomedicodi.ToString(), "21", "F2", "COSTOS MARGINALES", pto.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(FillMeptomedicion(9999999, cadena));
                }
            }

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Niveld));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Escenario));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Cmgxmwh_sr));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Cmgxmwh_srideal));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Unidadmarginal));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Unidadmarginalideal));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Reservarotante));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Reservasec));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Reservafria));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Reservafriatermica));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Reservafriahidraulica));
            lista.Add(FillMeptomedicion(6, ConstantesMigraciones.ReservaEficiente));
            lista.Add(FillMeptomedicion(7, ConstantesMigraciones.ReservaEficienteGas));
            lista.Add(FillMeptomedicion(8, ConstantesMigraciones.ReservaEficienteCarbon));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Ntablachaca));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Demandasein));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Demandacentro));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Demandanorte));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Demandasur));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Demandaela));
            lista.Add(FillMeptomedicion(6, ConstantesMigraciones.Demandaecuador));
            lista.Add(FillMeptomedicion(7, ConstantesMigraciones.Demandacoes));

            //Agregar los puntos de medición de Hidrología, Flujos que no han sido registrados lineas arriba
            //if (ls_grupoabrev != null && li_tipoinfocodi > 0 && !H_Pr.ContainsKey(ls_grupoabrev) && li_tipoinfocodi > 0)
            listaPto = listaPto.Where(x => !lista.Any(y => y.Ptomedicodi == x.Ptomedicodi)).ToList();
            listaPto = listaPto.Where(x => !lista.Any(y => ((y.Gruponomb) ?? string.Empty).Replace(" ", "").ToUpper().Trim() == ((x.Ptomedielenomb) ?? string.Empty).ToUpper().Trim())).ToList();

            foreach (var reg in listaPto)
            {
                PrGrupoDTO obj = new PrGrupoDTO()
                {
                    Gruponomb = reg.Ptomedidesc,
                    Ptomedicodi = reg.Ptomedicodi,
                    Tipoinfocodi = reg.Tipoinfocodi.Value,
                    Osicodi = reg.Osicodi,
                    Equinomb = reg.Equinomb,
                    Equiabrev = reg.Equiabrev,
                    Emprnomb = reg.Emprnomb,
                    Emprcodi = reg.Emprcodi,
                    Emprorden = 9999999 + 1,
                    Grupoorden = 9999999 + 1
                };

                lista.Add(obj);
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<PrGrupoDTO> FillCabeceraCdispatchActual(List<MeReporptomedDTO> listaPtosReporteFl, List<MeReporptomedDTO> listaPtosReporteCmg)
        {
            List<PrGrupoDTO> lista = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaTempFl = new List<PrGrupoDTO>();
            List<PrGrupoDTO> listaTempCmg = new List<PrGrupoDTO>();

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Totalcogeneracion));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Totalrenov));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Generacioncoes));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Demandancp));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Gtotal));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Perdidasncp));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Deficitncp));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Demandacentroncp));

            listaTempFl.Add(FillMeptomedicion(1, ConstantesMigraciones.FlCentroSur));
            listaTempFl.Add(FillMeptomedicion(2, ConstantesMigraciones.FlChilcaCarapongo));
            listaTempFl.Add(FillMeptomedicion(3, ConstantesMigraciones.FlCarapongoCarabayllo));
            listaTempFl.Add(FillMeptomedicion(4, ConstantesMigraciones.FlChimboteTrujillo));
            listaTempFl.Add(FillMeptomedicion(5, ConstantesMigraciones.FlTrujilloNinia));
            listaTempFl.Add(FillMeptomedicion(6, ConstantesMigraciones.FlColcabambaPoroma));
            listaTempFl.Add(FillMeptomedicion(7, ConstantesMigraciones.FlChilcaPoroma));
            listaTempFl.Add(FillMeptomedicion(8, ConstantesMigraciones.FlPoromaYarabamba));
            listaTempFl.Add(FillMeptomedicion(9, ConstantesMigraciones.FlPoromaOconia));
            listaTempFl.Add(FillMeptomedicion(10, ConstantesMigraciones.FlYarabambaMontalvo));
            listaTempFl.Add(FillMeptomedicion(11, ConstantesMigraciones.FlHuancavelicaIndependencia));
            listaTempFl.Add(FillMeptomedicion(12, ConstantesMigraciones.FlChilcaSanJuan));

            //si el punto existe en la lista temporal se saca el equinomb y gruponomb
            foreach (var ptoFlujos in listaPtosReporteFl)
            {
                int orden = 9999999;
                var ptoTemporal = listaTempFl.Find(x => x.Ptomedicodi == ptoFlujos.Ptomedicodi);
                if (ptoTemporal != null)
                {
                    ptoFlujos.Equinomb = ptoTemporal.Equinomb;
                    ptoFlujos.Gruponomb = ptoTemporal.Gruponomb;
                    ptoFlujos.Equiabrev = ptoTemporal.Equiabrev;
                    orden = ptoTemporal.Grupoorden ?? 0;
                }

                string cadena = string.Empty;
                ptoFlujos.Equiabrev = ptoFlujos.Equiabrev == "0" || ptoFlujos.Equiabrev == null ? string.Empty : ptoFlujos.Equiabrev;
                string[] words = { "9992", ptoFlujos.Ptomedidesc.Trim(), ptoFlujos.Ptomedicodi.ToString(), "8", "F0", "FLUJO EN LINEAS", ptoFlujos.Equiabrev };
                cadena = String.Join(",", words);

                lista.Add(FillMeptomedicion(orden, cadena));
            }


            /*lista.Add(FillMeptomedicion(ConstantesMigraciones.Flcotasoca));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flchimbtruj));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flsepanuchimb));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flpomacochasjuan));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Fltrujguadalupe));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flarmicota));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Floconsjose));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flsjosemonta));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Fltinnvasoca));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flsocamoque));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flcarabchimb));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flguadareque));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flninapiura));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flcarabchilca));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flsjuanindus));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flchilcaplani));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flplanicarab));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flventachav));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flventazapa));
            lista.Add(FillMeptomedicion(ConstantesMigraciones.Flhuanzcarab));*/

            listaTempCmg.Add(FillMeptomedicion(1, ConstantesMigraciones.Cmgsein_ncp));
            listaTempCmg.Add(FillMeptomedicion(2, ConstantesMigraciones.Cmgstarosancp));
            listaTempCmg.Add(FillMeptomedicion(3, ConstantesMigraciones.Cmgtrujillo_ncp));
            listaTempCmg.Add(FillMeptomedicion(4, ConstantesMigraciones.Cmgsocabaya_ncp));

            //>>>>>>>>>>>>>>>>>>>>Costo Marginales>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            //si el punto existe en la lista temporal se saca el equinomb y gruponomb
            foreach (var ptoCmg in listaPtosReporteCmg)
            {
                int orden = 9999999;
                var ptoTemporal = listaTempCmg.Find(x => x.Ptomedicodi == ptoCmg.Ptomedicodi);
                if (ptoTemporal != null)
                {
                    ptoCmg.Equinomb = ptoTemporal.Equinomb;
                    ptoCmg.Gruponomb = ptoTemporal.Gruponomb;
                    ptoCmg.Equiabrev = ptoTemporal.Equiabrev;
                    orden = ptoTemporal.Grupoorden ?? 0;
                }

                string cadena = string.Empty;
                ptoCmg.Equiabrev = ptoCmg.Equiabrev == "0" || ptoCmg.Equiabrev == null ? string.Empty : ptoCmg.Equiabrev;
                string desc = ptoCmg.Ptomedidesc.Replace("_", " ").Trim();
                desc += "(US$/MWh)";
                string[] words = { "9993", desc, ptoCmg.Ptomedicodi.ToString(), "21", "F2", "COSTOS MARGINALES", ptoCmg.Equiabrev };
                cadena = String.Join(",", words);

                lista.Add(FillMeptomedicion(orden, cadena));
            }

            lista.Add(FillMeptomedicion(9999999 + 1, ConstantesMigraciones.Niveld));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Escenario));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Cmgxmwh_sr));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Cmgxmwh_srideal));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Unidadmarginal));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Unidadmarginalideal));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Reservarotante));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Reservasec));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Reservafria));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Reservafriatermica));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.ReservaEficiente));
            lista.Add(FillMeptomedicion(6, ConstantesMigraciones.ReservaEficienteGas));
            lista.Add(FillMeptomedicion(7, ConstantesMigraciones.ReservaEficienteCarbon));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Ntablachaca));

            lista.Add(FillMeptomedicion(1, ConstantesMigraciones.Demandasein));
            lista.Add(FillMeptomedicion(2, ConstantesMigraciones.Demandacentro));
            lista.Add(FillMeptomedicion(3, ConstantesMigraciones.Demandanorte));
            lista.Add(FillMeptomedicion(4, ConstantesMigraciones.Demandasur));
            lista.Add(FillMeptomedicion(5, ConstantesMigraciones.Demandaela));
            lista.Add(FillMeptomedicion(6, ConstantesMigraciones.Demandaecuador));
            lista.Add(FillMeptomedicion(7, ConstantesMigraciones.Demandacoes));

            //cambiar texto BD de NCP a Yupana
            var listaPtoNoGen = lista.Where(x => x.Equiabrev != "").ToList();
            foreach (var reg in listaPtoNoGen)
            {
                if (reg.Emprnomb != null && reg.Emprnomb.Contains("NCP"))
                    reg.Emprnomb = reg.Emprnomb.Replace("NCP", "YUPANA");
                if (reg.Gruponomb != null && reg.Gruponomb.Contains("NCP"))
                    reg.Gruponomb = reg.Gruponomb.Replace("NCP", "YUPANA");
            }

            return lista;
        }

        /// <summary>
        /// Lista de puntos medición para Anexo A - Reserva Fria
        /// </summary>
        /// <returns></returns>
        public static List<PrGrupoDTO> FillCabeceraCdispatchActualAnexoA()
        {
            List<PrGrupoDTO> lista = new List<PrGrupoDTO>();

            PrGrupoDTO pg = FillMeptomedicion(1, ConstantesMigraciones.AnexoAReservafriatermica);
            pg.Grupointegrante = ConstantesAppServicio.NO;
            pg.Gruporeservafria = ConstantesMigraciones.TipogrupoReservaFriaFicticio;
            lista.Add(pg);

            pg = FillMeptomedicion(2, ConstantesMigraciones.AnexoAReservafriatermicaRap);
            pg.Grupointegrante = ConstantesAppServicio.NO;
            pg.Gruporeservafria = ConstantesMigraciones.TipogrupoReservaFriaFicticio;
            lista.Add(pg);

            pg = FillMeptomedicion(3, ConstantesMigraciones.AnexoAReservafriatermicaXMantto);
            pg.Grupointegrante = ConstantesAppServicio.NO;
            pg.Gruporeservafria = ConstantesMigraciones.TipogrupoReservaFriaFicticio;
            lista.Add(pg);

            return lista;
        }

        private static PrGrupoDTO FillMeptomedicion(int ordenGrupo, string cadena)
        {
            var arr = cadena.Split(',');
            string equiabrev_ = string.Empty;
            if (arr.Length > 7)
            {
                for (int z = 6; z < arr.Length; z++) { equiabrev_ += arr[z] + ","; }
                equiabrev_ = equiabrev_.Substring(0, equiabrev_.Length - 1);
            }
            else { equiabrev_ = arr[6]; }
            //string equiabrev_ = (arr.Length > 7 ? string.Join(",",)+ arr[6] + "," + arr[7] : arr[6]);

            int emprorden = 9999999 + 1;
            //setear orden
            switch (arr[5] ?? "")
            {
                case "GENERACION":
                    emprorden += 100;
                    break;

                case "SALIDAS NCP":
                case "SALIDAS YUPANA":
                    emprorden += 200;
                    break;

                case "FLUJO EN LINEAS":
                    emprorden += 300;
                    break;

                case "COSTOS MARGINALES":
                    emprorden += 400;
                    break;

                case "RESERVA":
                    emprorden += 500;
                    break;

                case "R":
                    emprorden += 600;
                    break;

                case "DEMANDA POR AREAS":
                    emprorden += 700;
                    break;
            }

            return new PrGrupoDTO()
            {
                Gruponomb = arr[1],
                GruponombWeb = arr[1],
                Ptomedicodi = int.Parse(arr[2]),
                Tipoinfocodi = int.Parse(arr[3]),
                Osicodi = arr[4],
                Equinomb = arr[5],
                Equiabrev = equiabrev_,
                Grupointegrante = ConstantesAppServicio.SI,
                Emprnomb = arr[5],
                Emprcodi = int.Parse(arr[0]),
                Grupoorden = ordenGrupo,
                Emprorden = emprorden
            };
        }

        /// <summary>
        /// método que obtiene el equinombre 
        /// </summary>
        /// <param name="listaPuntos"></param>
        /// <param name="listaTemporal"></param>
        /// <param name="lista"></param>
        /// <param name="tipo"></param>
        private static void AgregarListaEQNombres(List<MePtomedicionDTO> listaPuntos, List<PrGrupoDTO> listaTemporal, ref List<PrGrupoDTO> lista, int tipo)
        {
            if (tipo == ConstantesMigraciones.ReporteCdispatchFl)
            {
                foreach (var ptoFlujos in listaPuntos)
                {
                    var ptoTemporal = listaTemporal.Find(x => x.Ptomedicodi == ptoFlujos.Ptomedicodi);
                    if (ptoTemporal != null)
                    {
                        ptoFlujos.Equinomb = ptoTemporal.Equinomb;
                        ptoFlujos.Gruponomb = ptoTemporal.Gruponomb;
                        ptoFlujos.Equiabrev = ptoTemporal.Equiabrev;
                    }
                    string cadena = string.Empty;
                    ptoFlujos.Equiabrev = ptoFlujos.Equiabrev == "0" || ptoFlujos.Equiabrev == null ? string.Empty : ptoFlujos.Equiabrev;
                    string[] words = { "9992", ptoFlujos.Ptomedidesc.Trim(), ptoFlujos.Ptomedicodi.ToString(), "8", "F0", "FLUJO EN LINEAS", ptoFlujos.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(FillMeptomedicion(9999999, cadena));
                }
            }
            if (tipo == ConstantesMigraciones.ReporteCdispatchCmg)
            {
                foreach (var ptoCmg in listaPuntos)
                {
                    var ptoTemporal = listaTemporal.Find(x => x.Ptomedicodi == ptoCmg.Ptomedicodi);
                    if (ptoTemporal != null)
                    {
                        ptoCmg.Equinomb = ptoTemporal.Equinomb;
                        ptoCmg.Gruponomb = ptoTemporal.Gruponomb;
                        ptoCmg.Equiabrev = ptoTemporal.Equiabrev;
                    }
                    string cadena = string.Empty;
                    ptoCmg.Equiabrev = ptoCmg.Equiabrev == "0" || ptoCmg.Equiabrev == null ? string.Empty : ptoCmg.Equiabrev;
                    string desc = ptoCmg.Ptomedidesc.Replace("_", " ").Trim();
                    desc += "(US$/MWh)";
                    string[] words = { "9993", desc, ptoCmg.Ptomedicodi.ToString(), "21", "F2", "COSTOS MARGINALES", ptoCmg.Equiabrev };
                    cadena = String.Join(",", words);

                    lista.Add(FillMeptomedicion(9999999, cadena));
                }
            }
        }

        private static List<ResultadoValidacionAplicativo> ValidarPtoMedicionDespacho(List<MePtomedicionDTO> entitys)
        {
            List<ResultadoValidacionAplicativo> listaVal = new List<ResultadoValidacionAplicativo>();

            //formatear datos
            foreach (var reg in entitys)
            {
                reg.Emprnomb = reg.Emprcodi > 0 ? (reg.Emprnomb ?? "").Trim() : string.Empty;
                reg.Equinomb = reg.Equicodi.GetValueOrDefault(0) > 0 ? (reg.Equinomb ?? "").Trim() : string.Empty;
                reg.Famabrev = reg.Equicodi.GetValueOrDefault(0) > 0 ? reg.Famabrev : string.Empty;
                reg.Famnomb = reg.Equicodi.GetValueOrDefault(0) > 0 ? reg.Famnomb : string.Empty;
                reg.Areanomb = reg.Equicodi.GetValueOrDefault(0) > 0 ? reg.Areanomb : string.Empty;
                reg.DesUbicacion = reg.Equicodi.GetValueOrDefault(0) > 0 ? (reg.DesUbicacion ?? "").Trim() : string.Empty;
                reg.Gruponomb = reg.Grupocodi.GetValueOrDefault(0) > 0 ? (reg.Gruponomb ?? "").Trim() : string.Empty;
                reg.Catenomb = reg.Grupocodi.GetValueOrDefault(0) > 0 ? (reg.Catenomb ?? "").Trim() : string.Empty;
                reg.Ptomedielenomb = string.IsNullOrEmpty(reg.Ptomedielenomb) ? (reg.Ptomedidesc ?? reg.Ptomedibarranomb) : reg.Ptomedielenomb;
                reg.Ptomedielenomb = (reg.Ptomedielenomb ?? "").Trim();

                reg.Ptomediestado = (reg.Ptomediestado ?? "ND").Trim().ToUpper();
                reg.Grupoestado = reg.Grupoestado ?? "ND";
                reg.Equiestado = reg.Equiestado ?? "ND";

                reg.Tipoptomedinomb = reg.Tipoptomedicodi > 0 ? reg.Tipoptomedinomb : string.Empty;
                reg.Tipoinfoabrev = reg.Tipoinfocodi > 0 ? reg.Tipoinfoabrev : string.Empty;
                reg.Tipoptomedinomb = reg.Tipoinfoabrev + " " + (reg.Tipoinfocodi > 0 && reg.Tipoptomedicodi > 0 ? "(" + reg.Tipoptomedinomb + ")" : reg.Tipoptomedinomb);

                ValidarDataPuntoMedicion(reg, out List<ResultadoValidacionAplicativo> listaMsjXPto);
                listaVal.AddRange(listaMsjXPto);
            }

            return listaVal;
        }

        private static void ValidarDataPuntoMedicion(MePtomedicionDTO regPto, out List<ResultadoValidacionAplicativo> listaMsj)
        {
            listaMsj = new List<ResultadoValidacionAplicativo>();

            //verificar consistencia de los datos
            if (regPto.Ptomediestado == ConstantesAppServicio.Activo)
            {
                List<string> listaEstadoVal = new List<string>() { ConstantesAppServicio.Activo, ConstantesAppServicio.Proyecto, ConstantesAppServicio.FueraCOES };
                if (regPto.Equicodi > 0 && !listaEstadoVal.Contains(regPto.Equiestado))
                {
                    var val = new ResultadoValidacionAplicativo()
                    {
                        TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                        Ptomedicodi = regPto.Ptomedicodi,
                        Ptomedielenomb = regPto.Ptomedielenomb,
                        Descripcion = string.Format("El equipo [{2},{3}] tiene estado {4}. Debe actualizarse el estado del punto de medición [{0}, {1}] de ACTIVO a BAJA.", regPto.Ptomedicodi, regPto.Ptomedielenomb, regPto.Equicodi, regPto.Central + " " + regPto.Equinomb, Util.EstadoDescripcion(regPto.Equiestado)),
                        Justificacion = "",
                        Emprnomb = regPto.Emprnomb
                    };
                    listaMsj.Add(val);
                }

                if (regPto.Grupocodi > 0 && !listaEstadoVal.Contains(regPto.Grupoestado))
                {

                    var val = new ResultadoValidacionAplicativo()
                    {
                        TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                        Ptomedicodi = regPto.Ptomedicodi,
                        Ptomedielenomb = regPto.Ptomedielenomb,
                        Descripcion = string.Format("El grupo [{2},{3}] tiene estado {4}. Debe actualizarse el estado del punto de medición [{0}, {1}] de ACTIVO a BAJA.", regPto.Ptomedicodi, regPto.Ptomedielenomb, regPto.Grupocodi, regPto.Gruponomb, Util.EstadoDescripcion(regPto.Grupoestado)),
                        Justificacion = "",
                        Emprnomb = regPto.Emprnomb
                    };
                    listaMsj.Add(val);
                }

                if (regPto.Emprcodi > 0 && regPto.Emprestado != ConstantesAppServicio.Activo)
                {

                    var val = new ResultadoValidacionAplicativo()
                    {
                        TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                        Ptomedicodi = regPto.Ptomedicodi,
                        Ptomedielenomb = regPto.Ptomedielenomb,
                        Descripcion = string.Format("La empresa [{2},{3}] tiene estado {4}. Debe actualizarse el Estado del punto de medición [{0}, {1}] de ACTIVO a BAJA.", regPto.Ptomedicodi, regPto.Ptomedielenomb, regPto.Emprcodi, regPto.Emprnomb, Util.EstadoDescripcion(regPto.Emprestado)),
                        Justificacion = "",
                        Emprnomb = regPto.Emprnomb
                    };
                    listaMsj.Add(val);
                }
            }

            if (regPto.Ptomediestado == "ND")
            {
                var val = new ResultadoValidacionAplicativo()
                {
                    TipoValidacion = ConstantesPR5ReportesServicio.MensajeError,
                    Ptomedicodi = regPto.Ptomedicodi,
                    Ptomedielenomb = regPto.Ptomedielenomb,
                    Descripcion = string.Format("El punto de medición [{0}, {1}] no tiene estado. Debe actualizarse el Estado del punto de medición a BAJA.", regPto.Ptomedicodi, regPto.Ptomedielenomb),
                    Justificacion = "",
                    Emprnomb = regPto.Emprnomb
                };
                listaMsj.Add(val);
            }

        }

        /// <summary>
        /// Guardar excel en objetos de medicion 48
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lectcodi"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="inputTipoinfocodi"></param>
        /// <param name="filaData"></param>
        /// <param name="columnPtos"></param>
        /// <param name="rowPto"></param>
        /// <param name="numHXDia"></param>
        /// <param name="numDia"></param>
        /// <param name="listaPtos"></param>
        /// <param name="listaPtoNoGeneradores"></param>
        /// <param name="esHojaActiva"></param>
        /// <param name="flagAutomatico"></param>
        /// <param name="lstColoresValidos"></param>
        /// <param name="lstErrorColores"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> LeerDatosMe48FromHoja(ExcelWorksheet ws, int lectcodi, DateTime fechaPeriodo, string inputTipoinfocodi, int filaData, int filaNombPto, int columnPtos
                                                , int rowPto, int numHXDia, int numDia, List<MePtomedicionDTO> listaPtos, List<PrGrupoDTO> listaPtoNoGeneradores
                                                , bool esHojaActiva, bool flagAutomatico, List<PrHtrabajoEstadoDTO> lstColoresValidos, out List<HtError> lstErrorColores)
        {
            lstErrorColores = new List<HtError>();

            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            int col = columnPtos, row = rowPto;

            string nombreHoja = ws.Name;
            int nroFilas = numHXDia * numDia;
            List<int> listaTipoinfocodi = inputTipoinfocodi.Split(',').Select(x => int.Parse(x)).ToList();

            //Leer las primeras 500 columnas del excel
            for (int cont = 0; cont < 500; cont++)
            {
                row = rowPto;
                if (ws.Cells[row, col].Value != null)
                {
                    //no leer puntos vacios ni -1
                    if (int.Parse(ws.Cells[row, col].Value.ToString()) != -1)
                    {
                        int ptomedicodi = int.Parse(ws.Cells[row, col].Value.ToString());

                        MePtomedicionDTO ptoGenerador = listaPtos.Find(x => x.Ptomedicodi == ptomedicodi);
                        PrGrupoDTO ptoNoGenerador = listaPtoNoGeneradores.Find(x => x.Ptomedicodi == ptomedicodi);

                        int emprcodi = (ptoGenerador != null ? (int)ptoGenerador.Emprcodi : -1);
                        int tipoinfocodi = (ptoGenerador != null ? (int)ptoGenerador.Tipoinfocodi : 0);
                        if (listaTipoinfocodi.Count == 1) tipoinfocodi = listaTipoinfocodi[0];

                        if (ptoNoGenerador != null) // si el tipoinfocodi es MW, en los archivos también puede incluir otros como mw demanda
                        {
                            tipoinfocodi = (ptoNoGenerador != null ? (int)ptoNoGenerador.Tipoinfocodi : 0);
                        }

                        if (ptomedicodi == 1316)
                        { }

                        //solo leer los puntos de medición que se encuentran en ME_PTOMEDICION, los puntos negativos se calculan en memoria
                        if ((ptomedicodi != 0 && tipoinfocodi > 0 && (ptoGenerador != null || ptoNoGenerador != null)))
                        {
                            row = filaData;
                            if (nroFilas == 48)
                            {
                                decimal total = 0;
                                var obj48 = new MeMedicion48DTO()
                                {
                                    Lectcodi = lectcodi,
                                    Ptomedicodi = ptomedicodi,
                                    Emprcodi = emprcodi,
                                    Tipoinfocodi = tipoinfocodi,
                                    Medifecha = fechaPeriodo,
                                };

                                string nombrePto = nombreHoja + " " + ptomedicodi.ToString();
                                if (ws.Cells[filaNombPto, col].Value != null) nombrePto = nombreHoja + " " + ptomedicodi + " " + ws.Cells[filaNombPto, col].Value.ToString();

                                //leer las 48 filas de celdas numéricas
                                for (int i = 1; i <= numHXDia; i++)
                                {
                                    string valorCelda = ws.Cells[row, col].Value != null ? ws.Cells[row, col].Value.ToString().Trim() : "";
                                     
                                    decimal val = !string.IsNullOrEmpty(valorCelda) ? decimal.TryParse(valorCelda, out decimal valor) ? valor : 0.0m : 0.0m;
                                    int posH = i;

                                    //validar valor numerico
                                    if (!string.IsNullOrEmpty(valorCelda) && !decimal.TryParse(valorCelda, out decimal valorTmp))
                                    {
                                        var regError = new HtError { Ptomedicion = nombrePto, Posicion = UtilExcel.GetCellAddress(row, col), Descripcion = "tiene valor no numérico" };
                                        
                                        if (flagAutomatico)
                                        {
                                            val = 0.0m; // solo si es automatico se setea con 0 a los no numericos
                                            regError.Descripcion += " y se asignó como 0";
                                        }

                                        lstErrorColores.Add(regError);
                                    }

                                    //validar valor negativo para potencia activa
                                    if (tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW && val < 0.0m)
                                    {
                                        var regError = new HtError { Ptomedicion = nombrePto, Posicion = UtilExcel.GetCellAddress(row, col), Descripcion = "tiene valor negativo" };

                                        if (flagAutomatico)
                                        {
                                            val = 0.0m; // solo si es automatico se setea con 0 a los valores negativos
                                            regError.Descripcion += " y se asignó como 0";
                                        }

                                        lstErrorColores.Add(regError);
                                    }

                                    total += val;
                                    obj48.GetType().GetProperty(ConstantesAppServicio.CaracterH + posH).SetValue(obj48, val);

                                    //validar color
                                    if (esHojaActiva)
                                    {
                                        string color = ws.Cells[row, col].Style.Fill.BackgroundColor.Rgb ?? "FFFFFFFF"; // blanco por defecto
                                        color = color == string.Empty ? "FFFFFFFF" : color;
                                        color = color.Substring(2);

                                        var colorBD = lstColoresValidos.Find(x => x.Htestcolor == color);

                                        if (colorBD != null)
                                        {
                                            int idColor = colorBD.Htestcodi;
                                            obj48.GetType().GetProperty(ConstantesAppServicio.CaracterT + posH).SetValue(obj48, idColor);
                                        }
                                        else
                                        {
                                            lstErrorColores.Add(new HtError { Ptomedicion = nombrePto, Posicion = UtilExcel.GetCellAddress(row, col), Descripcion = "no tiene un color válido " + "(" + color + ")" });
                                        }
                                    }

                                    row++;
                                }
                                obj48.Meditotal = total;

                                //solo considerar a los objetos con total distinto a cero
                                if (total != 0)
                                {
                                    lista.Add(obj48);
                                }
                            }
                        }
                    }
                }

                col++;
            }

            return lista;
        }

        /// <summary>
        /// GetFechaHTrabajo
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="filaFecha"></param>
        /// <param name="columnFecha"></param>
        /// <returns></returns>
        public static DateTime GetFechaHTrabajo(ExcelWorksheet ws, int filaFecha, int columnFecha)
        {
            DateTime fechaPeriodo;
            try
            {
                fechaPeriodo = ((DateTime?)ws.Cells[filaFecha, columnFecha].Value) ?? DateTime.MinValue;
            }
            catch (Exception)
            {
                throw new ArgumentException("La celda de la fecha no tiene el formato correcto.");
            }

            return fechaPeriodo;
        }

        public class HtError
        {
            public string Ptomedicion { get; set; }
            public string Posicion { get; set; }
            public string Descripcion { get; set; }
        }

        public class HtFiltro
        {
            public bool FlagCDispatchCargaActiva { get; set; }
            public bool FlagCDispatchCargaReactiva { get; set; }
            public bool FlagCDispatchCargaHidrologia { get; set; }
            public bool FlagCDispatchCargaReprograma { get; set; }
        }

        #endregion

        #region Presentación CDispatch - Html

        /// <summary>
        /// Reporte Cdispatch Html de todos los filtros
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="regCDespacho"></param>
        /// <returns></returns>
        public static List<string> CdispatchHtml(DateTime fechaInicio, DateTime fechaFin, CDespachoGlobal regCDespacho)
        {
            List<string> listaRes = new List<string>();

            foreach (var regTipo in ListarTipoinfo())
            {
                listaRes.Add(CdispatchHtmlXTipoInfo(regTipo.Tipoinfocodi, regTipo.Tipoinfoabrev, fechaInicio, fechaFin, regCDespacho));
            }

            return listaRes;
        }

        /// <summary>
        /// Reporte web
        /// </summary>
        /// <param name="tipoinfocodi"></param>
        /// <param name="tipoinfoabrev"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="regCDespacho"></param>
        /// <returns></returns>
        private static string CdispatchHtmlXTipoInfo(int tipoinfocodi, string tipoinfoabrev, DateTime fechaInicio, DateTime fechaFin, CDespachoGlobal regCDespacho)
        {
            //
            List<PrGrupoDTO> listaGrupoCabecera = regCDespacho.ListaGrupoCabeceraWeb;

            StringBuilder strHtml = new StringBuilder();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            #region cabecera

            strHtml.Append("<div class='freeze_table' id='resultado' style='height: auto;'>");
            strHtml.Append("<table id='tb_info' class='pretty tabla-icono' style='' >");

            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.AppendFormat("<th style='width: 100px;height: 160px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Día Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br/> ({0})</th>", tipoinfoabrev);
            foreach (var regGrupo in listaGrupoCabecera)
            {
                string claseTh = "th_cdisptach_default";
                if (regGrupo.EsGrupogen && regGrupo.Grupointegrante == ConstantesAppServicio.SI) claseTh = "th_cdisptach_integrante";
                if (regGrupo.EsGrupogen && regGrupo.Grupointegrante != ConstantesAppServicio.SI) claseTh = "th_cdisptach_no_integrante";
                if (regGrupo.EsGrupogen && regGrupo.TipoGenerRer == ConstantesAppServicio.SI) claseTh = "th_cdisptach_rer";
                if (regGrupo.EsGrupogen && regGrupo.Grupotipocogen == ConstantesAppServicio.SI) claseTh = "th_cdisptach_cogeneracion";

                string titleTh = string.Empty;
                if (regGrupo.EsGrupogen) titleTh = string.Format("title='{0}'", regGrupo.GruponombWebCompleto);
                int ancho = ((regGrupo.GruponombWeb ?? "").ToUpper().StartsWith("RESERVA")) ? 40 : 38;
                strHtml.AppendFormat("<th class='{1}' {2}><div style='width: {3}px;height: 160px;' class='rotate {1}'> {0} </div></th>", regGrupo.GruponombWeb, claseTh, titleTh, ancho);
            }
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");

            #endregion

            #region cuerpo

            strHtml.Append("<tbody>");

            for (DateTime f = fechaInicio; f <= fechaFin; f = f.AddDays(1))
            {
                CDespachoDiario regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == f);

                List<DatoCDispatch> listaCelda = regCDespachoXDia.ListaCelda;
                List<MeMedicion48DTO> listaMe48 = regCDespachoXDia.ListaMe48XDia.Where(x => x.Tipoinfocodi == tipoinfocodi).ToList();
                List<MeMedicion48DTO> lCostoOrdenado = regCDespachoXDia.ListaCostoXDia;

                ResultadoCDespachoDiario resultadoDiario = regCDespachoXDia.ReservaFriaXDia;
                MeMedicion48DTO regTotalRFTermo = resultadoDiario.RFTotalXDia;
                MeMedicion48DTO regRsvRotante = resultadoDiario.RRTotalXDia;
                MeMedicion48DTO regREfi = resultadoDiario.ListaREficienteXDia.Find(x => x.Fenergcodi == 0);
                MeMedicion48DTO regREfiGas = resultadoDiario.ListaREficienteXDia.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas);
                MeMedicion48DTO regREfiCarbon = resultadoDiario.ListaREficienteXDia.Find(x => x.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiCarbon);
                List<MeMedicion48DTO> listaMwMantto = resultadoDiario.ListaMMDetalleXDia;

                switch (tipoinfocodi)
                {
                    case ConstantesAppServicio.TipoinfocodiRsvFria: //Reserva Fria
                        listaMe48 = resultadoDiario.ListaRFDetalleXDia;
                        break;
                    case ConstantesAppServicio.TipoinfocodiRsvRotante: //Reserva Rotante
                        listaMe48 = resultadoDiario.ListaRRDetalleXDia;
                        break;
                    case ConstantesAppServicio.TipoinfocodiMwXMantto://MW x Mantto
                        listaMe48 = resultadoDiario.ListaMMDetalleXDia;
                        break;
                    case ConstantesAppServicio.TipoinfocodiEfiGas://MW x Mantto
                        listaMe48 = resultadoDiario.ListaREfiGasXDia;
                        break;
                    case ConstantesAppServicio.TipoinfocodiEfiCarbon://MW x Mantto
                        listaMe48 = resultadoDiario.ListaREfiCarbonXDia;
                        break;
                }

                #region detalle de las medias horas

                string bloque = "par";
                for (int i = 1; i <= 48; i++)
                {
                    strHtml.AppendFormat("<tr>");

                    //columna hora
                    if (i != 48) strHtml.AppendFormat("<td>{0}</td>", f.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoFechaHora));
                    else strHtml.AppendFormat("<td>{0}</td>", f.AddMinutes(i * 30).AddMinutes(-1).ToString(ConstantesAppServicio.FormatoFechaHora));

                    //columna de cada punto/grupo
                    foreach (var regGrupo in listaGrupoCabecera)
                    {
                        if (regGrupo.Grupocodi == 3417)
                        { }

                        //puntos de medición no grupos de despacho
                        if (!regGrupo.EsGrupogen)
                        {
                            decimal value = 0;
                            switch (regGrupo.GruponombWeb)
                            {
                                case "NivelDemanda": strHtml.Append("<td>" + (f.AddMinutes(i * 30).Hour < 8 ? 3 : (f.AddMinutes(i * 30).Hour < 18 ? 2 : (f.AddMinutes(i * 30).Hour < 23 ? 1 : 3))) + "</td>"); break;
                                case "Escenario": strHtml.Append("<td>1</td>"); break;
                                //case "CmgxMWh_SR": strHtml.Append("<td></td>"); break;
                                //case "ReservaFriaHidraulica":
                                //    strHtml.Append("<td></td>"); break;
                                case "ReservaFriaTermica":
                                    value = (regTotalRFTermo != null ? (decimal?)regTotalRFTermo.GetType().GetProperty("H" + i).GetValue(regTotalRFTermo, null) : null).GetValueOrDefault(0);
                                    strHtml.AppendFormat("<td>{0}</td>", value.ToString("N", nfi));
                                    break;
                                case "ReservaRotante":
                                    value = (regRsvRotante != null ? (decimal?)regRsvRotante.GetType().GetProperty("H" + i).GetValue(regRsvRotante, null) : null).GetValueOrDefault(0);
                                    strHtml.AppendFormat("<td>{0}</td>", value.ToString("N", nfi));
                                    break;

                                case "ReservaEficiente":
                                    value = (regREfi != null ? (decimal?)regREfi.GetType().GetProperty("H" + i).GetValue(regREfi, null) : null).GetValueOrDefault(0);
                                    strHtml.AppendFormat("<td>{0}</td>", value.ToString("N", nfi));
                                    break;
                                case "ReservaEficienteGas":
                                    value = (regREfiGas != null ? (decimal?)regREfiGas.GetType().GetProperty("H" + i).GetValue(regREfiGas, null) : null).GetValueOrDefault(0);
                                    strHtml.AppendFormat("<td>{0}</td>", value.ToString("N", nfi));
                                    break;
                                case "ReservaEficienteCarbon":
                                    value = (regREfiCarbon != null ? (decimal?)regREfiCarbon.GetType().GetProperty("H" + i).GetValue(regREfiCarbon, null) : null).GetValueOrDefault(0);
                                    strHtml.AppendFormat("<td>{0}</td>", value.ToString("N", nfi));
                                    break;

                                default: strHtml.Append("<td></td>"); break;
                            }
                        }
                        else
                        {
                            MeMedicion48DTO reg48 = listaMe48.FirstOrDefault(x => x.Grupocodi == regGrupo.Grupocodi) ?? new MeMedicion48DTO();

                            //puntos de medición SÍ grupos de despacho
                            DatoCDispatch datoXGrupo = listaCelda.Find(x => x.Grupocodi == regGrupo.Grupocodi);

                            decimal? val = (decimal?)reg48.GetType().GetProperty("H" + i).GetValue(reg48, null);
                            string valorH = (val != null && val != 0) ? val.Value.ToString("N", nfi) : string.Empty;

                            string claseFenerg = string.Empty;
                            string titleTd = string.Empty;
                            string tdAlerta = string.Empty;
                            if (datoXGrupo != null)
                            {
                                var datoH = datoXGrupo.Array30Min[i - 1];
                                claseFenerg = GetBackgroundColorCeldaH(bloque, datoH.Fenergcodi);
                                titleTd = string.Join("\n", datoH.ListaMsj);
                                if (datoH.TieneMantto) tdAlerta += "<span class='td_mantto'>M</span>";
                                if (datoH.TieneTminarranque) tdAlerta += "<span class='td_arranque'>A</span>";
                                //if (datoH.TieneRfria) tdAlerta += "<span class='td_rfria'>F</span>";
                                if (datoH.TieneHOnoPotencia) valorH = string.Format("<span class='td_noPot'>{0}</span>", valorH);
                            }

                            strHtml.AppendFormat("<td style='background-color: {1} ' title='{2}'>{3} {0}</td>", valorH, claseFenerg, titleTd, tdAlerta);
                        }
                    }
                    strHtml.Append("</tr>");

                    //al terminar la fila multiplo de 12
                    if (i % 12 == 0)
                    {
                        bloque = bloque == "par" ? "impar" : "par";
                    }
                }

                #endregion

                #region ENERGIA MWh

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td_cdispatch_costoOp' style='height: 17px;'>MWh</td>");
                foreach (var d in listaGrupoCabecera)
                {
                    var det = listaMe48.Find(x => x.Grupocodi == d.Grupocodi);
                    if (det != null)
                    {
                        decimal total = 0;
                        for (int i = 1; i <= 48; i++)
                        {
                            var val = (decimal?)det.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(det, null);
                            total += (val ?? 0);
                        }
                        strHtml.Append("<td class='td_cdispatch_costoOp'>" + (total / 2).ToString("N", nfi) + "</td>");
                    }
                    else
                    {
                        strHtml.Append("<td class='td_cdispatch_costoOp'>" + (0).ToString("N", nfi) + "</td>");
                    }
                }
                strHtml.Append("</tr>");

                #endregion

                #region ENERGIA MWh x Mantto

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td_cdispatch_costoOp' style='height: 17px;'>MWhxMto</td>");
                foreach (var d in listaGrupoCabecera)
                {
                    var det = listaMwMantto.Find(x => x.Grupocodi == d.Grupocodi);
                    if (det != null)
                    {
                        decimal total = 0;
                        for (int i = 1; i <= 48; i++)
                        {
                            var val = (decimal?)det.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(det, null);
                            total += (val ?? 0);
                        }
                        strHtml.Append("<td class='td_cdispatch_costoOp'>" + (total / 2).ToString("N", nfi) + "</td>");
                    }
                    else
                    {
                        strHtml.Append("<td class='td_cdispatch_costoOp'>" + (0).ToString("N", nfi) + "</td>");
                    }
                }
                strHtml.Append("</tr>");

                #endregion

                #region Costo Combustible

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td_cdispatch_costoOp' style='height: 17px;'>CVC US$</td>");
                foreach (var d in listaGrupoCabecera)
                {
                    var grupo = lCostoOrdenado.Find(x => x.Grupocodi == d.Grupocodi);
                    if (grupo != null)
                    {
                        decimal total = grupo.CostoConsumoCombustible;
                        strHtml.Append(string.Format("<td class='td_cdispatch_costoOp'>{0}</td>", total.ToString("N", nfi)));
                    }
                    else { strHtml.Append("<td class='td_cdispatch_costoOp'></td>"); }
                }
                strHtml.Append("</tr>");

                #endregion

                #region Costo No Combustible

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td_cdispatch_costoOp' style='height: 17px;'>CVNC US$</td>");
                foreach (var d in listaGrupoCabecera)
                {
                    var grupo = lCostoOrdenado.Find(x => x.Grupocodi == d.Grupocodi);
                    if (grupo != null)
                    {
                        decimal total = (grupo.CostoNoCombustible > 0 ? grupo.CostoNoCombustible : 0);
                        strHtml.Append(string.Format("<td class='td_cdispatch_costoOp'>{0}</td>", total.ToString("N", nfi)));
                    }
                    else { strHtml.Append("<td class='td_cdispatch_costoOp'></td>"); }
                }
                strHtml.Append("</tr>");

                #endregion

                #region Costo Combustible baja Eficiencia

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td_cdispatch_costoOp' style='height: 17px;'>CBEF US$</td>");
                foreach (var d in listaGrupoCabecera)
                {
                    var grupo = lCostoOrdenado.Find(x => x.Grupocodi == d.Grupocodi);
                    if (grupo != null)
                    {
                        decimal total = grupo.CostoCombustibleBajaEficiencia;
                        strHtml.Append(string.Format("<td class='td_cdispatch_costoOp'>{0}</td>", total.ToString("N", nfi)));
                    }
                    else { strHtml.Append("<td class='td_cdispatch_costoOp'></td>"); }
                }
                strHtml.Append("</tr>");
                #endregion

                #region Costo Arranque

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td_cdispatch_costoOp' style='height: 20px;'>CArr US$</td>");
                foreach (var d in listaGrupoCabecera)
                {
                    var grupo = lCostoOrdenado.Find(x => x.Grupocodi == d.Grupocodi);

                    if (grupo != null)
                    {
                        decimal total = grupo.CostoArranque;
                        strHtml.Append(string.Format("<td class='td_cdispatch_costoOp'>{0}</td>", total.ToString("N", nfi)));
                    }
                    else { strHtml.Append("<td class='td_cdispatch_costoOp'></td>"); }
                }
                strHtml.Append("</tr>");

                #endregion

                #region Detalle adicional
                //Código de grupo
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional' style='height: 20px;'>{0}</td>", "Grupocodi");
                foreach (var d in listaGrupoCabecera)
                {
                    string valorTd = (d.Grupocodi > 0) ? d.Grupocodi.ToString() : string.Empty;
                    strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional'>{0}</td>", valorTd);
                }
                strHtml.Append("</tr>");

                //Código de pto
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional' style='height: 20px;'>{0}</td>", "Ptomedicodi");
                foreach (var d in listaGrupoCabecera)
                {
                    string valorTd = (d.Grupocodi > 0) ? d.Ptomedicodi.ToString() : string.Empty;
                    strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional'>{0}</td>", valorTd);
                }
                strHtml.Append("</tr>");

                //Código de empresa
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional' style='height: 20px;'>{0}</td>", "Emprcodi");
                foreach (var d in listaGrupoCabecera)
                {
                    string valorTd = (d.Grupocodi > 0 && d.Emprcodi > 0) ? d.Emprcodi.ToString() : string.Empty;
                    strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional'>{0}</td>", valorTd);
                }
                strHtml.Append("</tr>");

                //orden de empresa
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional' style='height: 20px;'>{0}</td>", "Emprorden");
                foreach (var d in listaGrupoCabecera)
                {
                    string valorTd = (d.Grupocodi > 0 && d.Emprorden > 0) ? d.Emprorden.ToString() : string.Empty;
                    strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional'>{0}</td>", valorTd);
                }
                strHtml.Append("</tr>");

                //orden de grupo
                strHtml.Append("<tr>");
                strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional' style='height: 20px;'>{0}</td>", "Grupoorden");
                foreach (var d in listaGrupoCabecera)
                {
                    string valorTd = (d.Grupocodi > 0 && d.Grupoorden > 0) ? d.Grupoorden.ToString() : string.Empty;
                    strHtml.AppendFormat("<td class='td_cdispatch_detalle_adicional'>{0}</td>", valorTd);
                }
                strHtml.Append("</tr>");
                #endregion
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append("</div>");

            #endregion

            return strHtml.ToString();
        }

        /// <summary>
        /// GenerarCdispatchObservacionesHtml
        /// </summary>
        /// <param name="regCDespacho"></param>
        /// <returns></returns>
        public static string GenerarCdispatchObservacionesHtml(CDespachoGlobal regCDespacho)
        {
            StringBuilder strHtml = new StringBuilder();

            foreach (var reg in regCDespacho.ListaCDespachoDiario)
            {
                var listaMensajeValidacion = reg.ListaMensajeValidacionXDia;

                if (listaMensajeValidacion.Any())
                {
                    List<ResultadoValidacionAplicativo> listaError = new List<ResultadoValidacionAplicativo>();
                    foreach (var msjxPto in listaMensajeValidacion.Where(x => x.TipoValidacion == ConstantesPR5ReportesServicio.MensajeError).OrderBy(x => x.Emprnomb).GroupBy(x => new { x.Emprnomb }))
                    {
                        var listaMsj = msjxPto.Select(x => x.Descripcion + (x.Justificacion ?? "")).Distinct();
                        var regVal = new ResultadoValidacionAplicativo()
                        {
                            TipoValidacion = msjxPto.First().TipoValidacion,
                            Emprnomb = msjxPto.First().Emprnomb,
                            ListaMensaje = listaMsj.OrderBy(x => x).ToList(),
                        };
                        listaError.Add(regVal);
                    }

                    List<ResultadoValidacionAplicativo> listaAlerta = new List<ResultadoValidacionAplicativo>();
                    foreach (var msjxPto in listaMensajeValidacion.Where(x => x.TipoValidacion == ConstantesPR5ReportesServicio.MensajeAlerta).GroupBy(x => new { x.Ptomedicodi }))
                    {
                        var regVal = new ResultadoValidacionAplicativo()
                        {
                            Ptomedicodi = msjxPto.Key.Ptomedicodi,
                            TipoValidacion = msjxPto.First().TipoValidacion,
                            Ptomedielenomb = msjxPto.First().Ptomedielenomb,
                            Emprnomb = msjxPto.First().Emprnomb,
                            ListaMensaje = msjxPto.Select(x => x.Descripcion).Distinct().ToList()
                        };
                        listaAlerta.Add(regVal);
                    }

                    List<ResultadoValidacionAplicativo> listaOtro = new List<ResultadoValidacionAplicativo>();
                    foreach (var msjxPto in listaMensajeValidacion.Where(x => x.TipoValidacion != ConstantesPR5ReportesServicio.MensajeError && x.TipoValidacion != ConstantesPR5ReportesServicio.MensajeAlerta)
                                                                    .GroupBy(x => new { x.Ptomedicodi }))
                    {
                        var regVal = new ResultadoValidacionAplicativo()
                        {
                            Ptomedicodi = msjxPto.Key.Ptomedicodi,
                            ListaMensaje = msjxPto.Select(x => x.Descripcion).Distinct().ToList()
                        };
                        listaOtro.Add(regVal);
                    }

                    strHtml.AppendFormat("<h1>Observaciones {0}</h1>", reg.Fecha.ToString(ConstantesAppServicio.FormatoFecha));

                    if (listaOtro.Any())
                    {
                        strHtml.Append(GenerarListarMensajeHtml(3, listaOtro, "Mensajes", string.Empty, string.Empty));
                    }

                    if (listaError.Any())
                    {
                        strHtml.Append(GenerarListarMensajeHtml(1, listaError, "Mensajes de Error", string.Empty, "<b style='padding-left: 5px; padding-right: 5px; color: white; background-color: red;'>ERROR</b> "));
                    }

                    if (listaAlerta.Any())
                    {
                        strHtml.Append(GenerarListarMensajeHtml(2, listaAlerta, "Mensajes de Alerta (no afectan a los cálculos)", string.Empty, "<b style='padding-left: 5px; padding-right: 5px; color: white; background-color: #ffb021;'>ALERTA</b> "));
                    }
                }
            }

            return strHtml.ToString();
        }

        private static string GenerarListarMensajeHtml(int tipo, List<ResultadoValidacionAplicativo> lista, string titulo, string estilo, string htmlEstiloAdicional)
        {
            StringBuilder strHtml = new StringBuilder();
            strHtml.AppendFormat(@"
                <h2 style='{1}'>{0}</h2>
            ", titulo, estilo);

            foreach (var pto in lista)
            {
                if (tipo == 1)
                    strHtml.AppendFormat("Empresa <b>{0}</b><br/>", pto.Emprnomb);

                if (tipo == 2)
                    strHtml.AppendFormat("Punto de medición {0} <b> {1} </b> de {2} <br/>", pto.Ptomedicodi, pto.Ptomedielenomb, pto.Emprnomb);

                strHtml.Append("<ul>");
                foreach (var msj in pto.ListaMensaje)
                {
                    strHtml.AppendFormat("<li>{0} {1}</li>", htmlEstiloAdicional, msj);
                }
                strHtml.Append("</ul>");
            }

            return strHtml.ToString();
        }

        private static string GetBackgroundColorCeldaH(string bloque, int fenergcodi)
        {
            string backColor = fenergcodi >= 0 ? "#FFFFFF" : "#FF0000";
            switch (fenergcodi)
            {
                case ConstantesPR5ReportesServicio.FenergcodiAgua:
                    backColor = bloque == "par" ? "#F0FFFF" : "#E0FFFF";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiGas:
                    backColor = bloque == "par" ? "#FFFDE6" : "#FFFACD";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiCarbon:
                    backColor = bloque == "par" ? "#FFEDDD" : "#FFDAB9";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiDiesel:
                    backColor = bloque == "par" ? "#E3FBE3" : "#C8F7C8";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiResidual:
                    backColor = bloque == "par" ? "#EBFFCC" : "#D6FF97";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiR500:
                    backColor = bloque == "par" ? "#EBFFCC" : "#D6FF97";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiR6:
                    backColor = bloque == "par" ? "#EBFFCC" : "#D6FF97";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiBagazo:
                    backColor = bloque == "par" ? "#EED38E" : "#DAA520";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiBiogas:
                    backColor = bloque == "par" ? "#EED38E" : "#DAA520";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiSolar:
                    backColor = bloque == "par" ? "#FFFFFF" : "#FFFFFF";
                    break;
                case ConstantesPR5ReportesServicio.FenergcodiEolica:
                    backColor = bloque == "par" ? "#FFFFFF" : "#FFFFFF";
                    break;
            }

            return backColor;
        }

        /// <summary>
        /// ListarTipoinfo
        /// </summary>
        /// <returns></returns>
        public static List<SiTipoinformacionDTO> ListarTipoinfo()
        {
            List<SiTipoinformacionDTO> listaTipoinfo = new List<SiTipoinformacionDTO>
            {
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW, Tipoinfoabrev = "Generación MW" },
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMVAR, Tipoinfoabrev = "Generación MVAR" },
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiRsvFria, Tipoinfoabrev = "Reserva Fría" },
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiRsvRotante, Tipoinfoabrev = "Reserva Rotante" },
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMwXMantto, Tipoinfoabrev = "MWxMantto" },
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiEfiGas, Tipoinfoabrev = "Eficiente Gas" },
                new SiTipoinformacionDTO() { Tipoinfocodi = ConstantesAppServicio.TipoinfocodiEfiCarbon, Tipoinfoabrev = "Eficiente Carbón" }
            };

            return listaTipoinfo;
        }

        #endregion

        #region Presentación CDispatch - Excel

        /// <summary>
        /// Exportacion Excel CDispatch, cada día del rango es una pestaña excel
        /// </summary>
        /// <param name="tieneMostrarDetallaAdicional"></param>
        /// <param name="regCDespacho"></param>
        /// <param name="rutaFile"></param>
        /// <param name="tipoinfocodi"></param>
        public static void GenerarArchivoExcelCdispatch(bool tieneMostrarDetallaAdicional, CDespachoGlobal regCDespacho, string rutaFile, int tipoinfocodi)
        {
            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                foreach (CDespachoDiario regDia in regCDespacho.ListaCDespachoDiario)
                {
                    //
                    string nombreHoja = regDia.Fecha.ToString(ConstantesBase.FormatoFecha);
                    nombreHoja = tipoinfocodi == 100 ? "ReservaFria" + nombreHoja : nombreHoja;

                    //
                    ws = xlPackage.Workbook.Worksheets.Add(nombreHoja);
                    ws = xlPackage.Workbook.Worksheets[nombreHoja];

                    HojaExcelDiaCdispatch(ws, tieneMostrarDetallaAdicional, regDia.Fecha, regCDespacho.Lectnomb, tipoinfocodi, 0, regDia);

                    ws.View.ShowGridLines = false;
                }

                if (regCDespacho.ListaFecha.Count == 0)
                {
                    xlPackage.Workbook.Worksheets.Add("sheet");
                    _ = xlPackage.Workbook.Worksheets["sheet"];
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Hoja individual CDispatch
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="tieneMostrarDetallaAdicional"></param>
        /// <param name="fechaPeriodo"></param>
        /// <param name="lectnomb"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="tipoRsvaFriaAnexoA"></param>
        /// <param name="regCDespachoXDia"></param>
        public static void HojaExcelDiaCdispatch(ExcelWorksheet ws, bool tieneMostrarDetallaAdicional, DateTime fechaPeriodo, string lectnomb, int tipoinfocodi, int tipoRsvaFriaAnexoA
                                            , CDespachoDiario regCDespachoXDia)
        {
            ResultadoCDespachoDiario regReservaFriaXDia = regCDespachoXDia.ReservaFriaXDia;
            List<MeMedicion48DTO> listaDespachoMe48 = regCDespachoXDia.ListaMe48XDia;
            List<PrTipogrupoDTO> listaTipoGrupo = regCDespachoXDia.ListaTipoGrupo;
            List<PrDnotasDTO> notas = regCDespachoXDia.ListaNotas;
            decimal tipoCambio = regCDespachoXDia.TipoCambio;

            List<MeMedicion1DTO> listaMe1 = regCDespachoXDia.ListaMe1XDia;
            List<MeMedicion48DTO> listaCosto = regCDespachoXDia.ListaCostoXDia;
            List<PrGrupoDTO> listaGr = new List<PrGrupoDTO>();
            listaGr.AddRange(regCDespachoXDia.ListaGrupoXDiaCabeceraExcel);
            List<MePtomedicionDTO> listaPtoDespacho30min = regCDespachoXDia.ListaPtoXDia;
            List<MeMedicion48DTO> listaMwMantto = regReservaFriaXDia.ListaMMDetalleXDia;

            List<DatoCDispatch> listaCelda = regReservaFriaXDia.ListaCelda;

            string descripcionFecha = EPDate.f_NombreDiaSemana(fechaPeriodo.DayOfWeek) + " " + EPDate.f_FechaenLetras(fechaPeriodo) + " / SEM N° " + EPDate.f_numerosemana(fechaPeriodo);

            //PR_TIPOGRUPO
            var tipogrupocodis = listaTipoGrupo.Select(x => x.Tipogrupocodi).ToList();

            //Consideración especial para reporte Reserva Fría del Anexo A
            if (tipoRsvaFriaAnexoA > 0)
            {
                listaGr.AddRange(UtilCdispatch.FillCabeceraCdispatchActualAnexoA().Where(x => x.Equiabrev != "").ToList());
                tipogrupocodis.Add(ConstantesMigraciones.TipogrupoReservaFriaFicticio);
            }

            List<MeMedicion48DTO> listaMe48 = new List<MeMedicion48DTO>();
            switch (tipoinfocodi)
            {
                case ConstantesAppServicio.TipoinfocodiMVAR:
                    listaMe48 = listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW).ToList();
                    List<int> lptoExcluir = new List<int>() { ConstantesSiosein2.PtomedicodiGTotal, ConstantesAppServicio.PtomedicodiDemandaCoes,
                        ConstantesAppServicio.PtomedicodiRsvFriaTermica, ConstantesAppServicio.PtomedicodiRsvEficiente, ConstantesAppServicio.PtomedicodiRsvEficienteGas, ConstantesAppServicio.PtomedicodiRsvEficienteCarbon};
                    listaMe48 = listaMe48.Where(x => !lptoExcluir.Contains(x.Ptomedicodi)).ToList();
                    break;
                case ConstantesAppServicio.TipoinfocodiMW:
                    listaMe48 = listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList();
                    break;
                case ConstantesAppServicio.TipoinfocodiRsvFria: //Reserva Fria
                    listaMe48 = regReservaFriaXDia.ListaRFDetalleXDia;
                    listaMe48.AddRange(listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList());

                    if (tipoRsvaFriaAnexoA > 0)
                    {
                        listaMe48 = new List<MeMedicion48DTO>();
                        foreach (var reg in listaPtoDespacho30min)
                        {
                            listaMe48.AddRange(reg.ListaReservaFria.Where(x => x.TipoReservaFria == tipoRsvaFriaAnexoA).ToList());
                        }
                        listaMe48.AddRange(regReservaFriaXDia.ListaRFDataTotalXDia);

                        listaMe48.AddRange(listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList());
                    }

                    break;
                case ConstantesAppServicio.TipoinfocodiRsvRotante: //Reserva Rotante
                    listaMe48 = regReservaFriaXDia.ListaRRDetalleXDia;
                    listaMe48.AddRange(listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList());
                    break;
                case ConstantesAppServicio.TipoinfocodiMwXMantto://MW x Mantto
                    listaMe48 = regReservaFriaXDia.ListaMMDetalleXDia;
                    listaMe48.AddRange(listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList());
                    break;
                case ConstantesAppServicio.TipoinfocodiEfiGas://MW x Mantto
                    listaMe48 = regReservaFriaXDia.ListaREfiGasXDia;
                    listaMe48.AddRange(listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList());
                    break;
                case ConstantesAppServicio.TipoinfocodiEfiCarbon://MW x Mantto
                    listaMe48 = regReservaFriaXDia.ListaREfiCarbonXDia;
                    listaMe48.AddRange(listaDespachoMe48.Where(x => x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMW && x.Tipoinfocodi != ConstantesAppServicio.TipoinfocodiMVAR).ToList());
                    break;
            }

            #region Cabecera y Cuerpo 

            int colIniTabla = 2;
            int rowIniTabla = 4;
            int sizeFont = 9;

            int rowTitulo = rowIniTabla - 1;

            //Primera fila - Fecha
            int colFecha = colIniTabla;
            ws.Cells[rowTitulo, colFecha].Value = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
            ws.Cells[rowTitulo, colFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Primera fila - Lectura
            int colIniLectnomb1 = colFecha + 3;
            int colIniFecha1 = colIniLectnomb1 + 7;
            int colIniLectnomb2 = colIniFecha1 + 10;
            int colIniFecha2 = colIniLectnomb2 + 7;
            ws.Cells[rowTitulo, colIniLectnomb1].Value = lectnomb;
            ws.Cells[rowTitulo, colIniFecha1].Value = descripcionFecha;
            ws.Cells[rowTitulo, colIniLectnomb2].Value = lectnomb;
            ws.Cells[rowTitulo, colIniFecha2].Value = descripcionFecha;

            ws.Cells[rowTitulo, colIniLectnomb1, rowTitulo, colIniFecha2].Style.Font.SetFromFont(new Font("Calibri", 14));
            ws.Cells[rowTitulo, colIniLectnomb1, rowTitulo, colIniFecha2].Style.Font.Bold = true;

            int rowEmp = rowIniTabla;
            int rowPto = rowEmp + 1;
            int rowGr = rowPto + 1;
            int rowHora = rowGr + 1;

            //columna hora

            ws.Row(rowGr).Height = 170;
            ws.Column(colFecha).Width = 8;

            ws.Cells[rowEmp, colFecha].Value = EPDate.f_NombreDiaSemana(fechaPeriodo.DayOfWeek);
            ws.Cells[rowGr, colFecha].Value = "Día Hora";
            ws.Cells[rowGr, colFecha].Style.TextRotation = 90;

            for (int i = 1; i <= 48; i++)
            {
                if (i != 48) { ws.Cells[rowHora, colFecha].Value = fechaPeriodo.AddMinutes(i * 30).ToString(ConstantesAppServicio.FormatoOnlyHora); }
                else { ws.Cells[rowHora, colFecha].Value = fechaPeriodo.AddMinutes(i * 30).AddMinutes(-1).ToString(ConstantesAppServicio.FormatoOnlyHora); }

                rowHora++;
            }

            int rowDataIni = rowGr + 1;
            int rowDataFin = rowDataIni + 47;
            int rowDataTotal = rowDataFin + 1;

            ws.Cells[rowDataTotal, colFecha].Value = "MWh";

            int rowMWhxMto = rowDataTotal;
            int rowCVC = rowDataTotal;
            int rowCVNC = rowDataTotal;
            int rowCBEF = rowDataTotal;
            int rowCArr = rowDataTotal;
            int rowGrupocodi = rowDataTotal;
            int rowPtomedicodi = rowDataTotal;
            int rowEmprcodi = rowDataTotal;
            int rowEmprorden = rowDataTotal;
            int rowGrupoorden = rowDataTotal;

            if (tieneMostrarDetallaAdicional)
            {
                rowMWhxMto = rowDataTotal + 1;
                rowCVC = rowMWhxMto + 1;
                rowCVNC = rowCVC + 1;
                rowCBEF = rowCVNC + 1;
                rowCArr = rowCBEF + 1;

                rowGrupocodi = rowMWhxMto;
                rowGrupoorden = rowCArr;

                ws.Cells[rowMWhxMto, colFecha].Value = "MWhxMto";
                ws.Cells[rowCVC, colFecha].Value = "CVC US$";
                ws.Cells[rowCVNC, colFecha].Value = "CVNC US$";
                ws.Cells[rowCBEF, colFecha].Value = "CBEF US$";
                ws.Cells[rowCArr, colFecha].Value = "CArr US$";

                ws.Column(colFecha).Width = 11;
            }
            int rowFinDetalle = rowGrupoorden;

            UtilCdispatch.SetBorderEmpresaCDispatch(ws, sizeFont, rowEmp, colFecha, colFecha, rowGr, rowDataTotal, rowGrupocodi, rowFinDetalle);

            //grupo de columnas
            int colIniTipogrupo = colFecha + 1;
            foreach (var t in tipogrupocodis)
            {
                List<PrGrupoDTO> listaGrXTipo = new List<PrGrupoDTO>();
                switch (t)
                {
                    case ConstantesMigraciones.TipogrupoIntegrantes:
                        listaGrXTipo = listaGr.Where(x => x.Grupointegrante == ConstantesAppServicio.SI && x.TipoGenerRer != ConstantesAppServicio.SI && x.Grupotipocogen != ConstantesAppServicio.SI).ToList();
                        break;
                    case ConstantesMigraciones.TipogrupoRER:
                        listaGrXTipo = listaGr.Where(x => x.TipoGenerRer == ConstantesAppServicio.SI).ToList();
                        break;
                    case ConstantesMigraciones.TipogrupoCogeneracion:
                        listaGrXTipo = listaGr.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).ToList();
                        break;
                    case ConstantesMigraciones.TipogrupoNoIntegrantes:
                        listaGrXTipo = listaGr.Where(x => x.Grupointegrante != ConstantesAppServicio.SI && x.Gruporeservafria != ConstantesMigraciones.TipogrupoReservaFriaFicticio).ToList();
                        break;
                    case ConstantesMigraciones.TipogrupoReservaFriaFicticio:
                        listaGrXTipo = listaGr.Where(x => x.Gruporeservafria == ConstantesMigraciones.TipogrupoReservaFriaFicticio).ToList();
                        break;
                }

                List<SiEmpresaDTO> listaEmpresa = listaGrXTipo.GroupBy(x => new { x.Emprcodi })
                                                            .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi.Value, Emprnomb = x.First().Emprnomb, Emprabrev = x.First().Emprabrev, Emprorden = x.First().Emprorden })
                                                            .OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ToList();
                int colFinTipoGrupo = colIniTipogrupo + listaGrXTipo.Count - 1;
                int colEmp = colIniTipogrupo;
                int colGr = colIniTipogrupo;

                //Mostrar Tipo de grupo
                if (t != 1)//Integrantes
                {
                    PrTipogrupoDTO tipoGrupo = listaTipoGrupo.Find(x => x.Tipogrupocodi == t);
                    ws.Cells[rowTitulo, colIniTipogrupo].Value = tipoGrupo != null ? tipoGrupo.Tipogruponomb : string.Empty;
                    ws.Cells[rowTitulo, colIniTipogrupo].Style.Font.Size = sizeFont;
                }

                // bordes y lineas
                //Mostrar empresas
                foreach (var regEmp in listaEmpresa)
                {
                    List<PrGrupoDTO> listaGrxEmpresa = listaGrXTipo.Where(x => x.Emprcodi == regEmp.Emprcodi).ToList();
                    int numGrXEmpresa = listaGrxEmpresa.Count;

                    //nombre de empresa
                    ws.Cells[rowEmp, colEmp].Value = (numGrXEmpresa > 2 ? regEmp.Emprnomb : regEmp.Emprabrev);
                    int colEmpFin = colEmp + numGrXEmpresa - 1;
                    ws.Cells[rowEmp, colEmp, rowEmp, colEmpFin].Merge = true;

                    UtilCdispatch.SetBorderEmpresaCDispatch(ws, sizeFont, rowEmp, colEmp, colEmpFin, rowGr, rowDataTotal, rowGrupocodi, rowFinDetalle);

                    //grupos
                    foreach (var regGr in listaGrxEmpresa)
                    {
                        //ptomedicodi
                        //Mostrar código de puntos de medición
                        ws.Cells[rowPto, colGr].Value = regGr.Ptomedicodi;

                        //Mostrar nombre de grupos
                        ws.Cells[rowGr, colGr].Value = regGr.Gruponomb;
                        ws.Cells[rowGr, colGr].Style.TextRotation = 90;

                        //Caso reporte Reserva Fría del Anexo A
                        if (t == ConstantesMigraciones.TipogrupoReservaFriaFicticio && regGr.Ptomedicodi == ConstantesAppServicio.AnexoAPtomedicodiRsvFriaTermica)
                            regGr.Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvFriaTermica;

                        //despacho
                        MeMedicion48DTO regM48 = listaMe48.Find(x => x.Ptomedicodi == regGr.Ptomedicodi);

                        //Datos calculo Rfria
                        DatoCDispatch regDato = listaCelda.Find(x => x.Ptomedicodi == regGr.Ptomedicodi);

                        //Información de cada media hora
                        int rowData = rowDataIni;
                        string bloque = "par";
                        bool despachoDiesel = false;
                        bool despachoGas = false;
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? val = regM48 != null ? (decimal?)regM48.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regM48, null) : null;

                            if (regGr.Ptomedicodi == ConstantesAppServicio.PtomedicodiNivelDemanda)
                            {
                                ws.Cells[rowData, colGr].Value = (fechaPeriodo.AddMinutes(i * 30).Hour < 8 ? "MIN" : (fechaPeriodo.AddMinutes(i * 30).Hour < 18 ? "MED" : (fechaPeriodo.AddMinutes(i * 30).Hour < 23 ? "MAX" : "MIN")));
                            }
                            else
                            {
                                if (val.GetValueOrDefault(0) != 0)
                                {
                                    ws.Cells[rowData, colGr].Value = val;
                                }

                                string strFormato = "0.0";
                                switch (regGr.Ptomedicodi)
                                {
                                    case ConstantesAppServicio.PtomedicodiDemandaCoes: strFormato = "0"; break;
                                    case ConstantesAppServicio.PtomedicodiGeneracionCoes: strFormato = "0"; break;
                                }
                                if (regGr.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiDolares)
                                    strFormato = "0.00";

                                ws.Cells[rowData, colGr].Style.Numberformat.Format = strFormato;
                            }

                            //REQ 2023-003167 -> Fondo color gris para las unidades que operen con diesel
                            List<int> maquinasDuales = new List<int> { 454, 451, 452, 439, 109, 144, 110, 113, 114 };

                            if (regDato != null && regM48 != null && val != null && val != 0)
                            {
                                Celda30minCDispatch tempRegH = regDato.Array30Min[i - 1];

                                if (tempRegH.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiDiesel && maquinasDuales.Contains(regGr.Grupocodi))
                                {
                                    despachoDiesel = true;
                                    ws.Cells[rowData, colGr].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                }
                                else if (tempRegH.Fenergcodi == ConstantesPR5ReportesServicio.FenergcodiGas)
                                {
                                    despachoGas = true;
                                }
                            }

                            //agregar descripción del cálculo
                            if (regGr.EsGrupogen && tieneMostrarDetallaAdicional)
                            {
                                AgregarDescripcionMediaHora(ws, rowData, colGr, bloque, regDato, i);

                            }


                            rowData++;
                            //al terminar la fila multiplo de 12
                            if (i % 12 == 0)
                            {
                                bloque = bloque == "par" ? "impar" : "par";
                            }
                        }
                        if (despachoDiesel == true && despachoGas == false)
                        {
                            ws.Cells[rowGr, colGr].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }

                        //Total
                        if (regGr.EsGrupogen)
                        {
                            decimal val = regM48 != null ? regM48.Meditotal.GetValueOrDefault(0) : 0;

                            ws.Cells[rowDataTotal, colGr].Value = val / 2;
                            ws.Cells[rowDataTotal, colGr].Style.Numberformat.Format = "0";
                        }

                        if (tieneMostrarDetallaAdicional)
                        {
                            if (regGr.EsGrupogen)
                            {
                                decimal val;

                                var reg48Mantto = listaMwMantto.Find(x => x.Grupocodi == regGr.Grupocodi);

                                //
                                val = reg48Mantto != null ? reg48Mantto.Meditotal.GetValueOrDefault(0) / 2 : 0;
                                ws.Cells[rowMWhxMto, colGr].Value = val;

                                //Costo
                                MeMedicion48DTO regCosto = listaCosto.Find(x => x.Grupocodi == regGr.Grupocodi);

                                //
                                val = regCosto != null ? regCosto.CostoConsumoCombustible : 0;
                                ws.Cells[rowCVC, colGr].Value = val;

                                //
                                val = regCosto != null ? regCosto.CostoNoCombustible : 0;
                                ws.Cells[rowCVNC, colGr].Value = val;

                                //
                                val = regCosto != null ? regCosto.CostoCombustibleBajaEficiencia : 0;
                                ws.Cells[rowCBEF, colGr].Value = val;

                                //
                                val = regCosto != null ? regCosto.CostoArranque : 0;
                                ws.Cells[rowCArr, colGr].Value = val;

                                ws.Cells[rowMWhxMto, colGr, rowCArr, colGr].Style.Numberformat.Format = "0.0";
                            }

                        }

                        int width = regGr.EsGrupogen ? 6 : 7;
                        ws.Column(colGr).Width = width;

                        colGr++;
                    }

                    colEmp += numGrXEmpresa;
                }

                colIniTipogrupo = colFinTipoGrupo + 2;
            }

            #endregion

            #region Costo y Nota

            //costo
            int rowCosto = rowFinDetalle + 2;
            string[] arrayCosto = GetDescripcionCostoOperacion(Acciones.Exportar, listaMe1, fechaPeriodo, tipoCambio);
            ws.Cells[rowCosto, colFecha + 1].Value = arrayCosto[0] + "    " + arrayCosto[1];
            ws.Cells[rowCosto, colFecha + 1].Style.Font.Bold = true;

            //notas
            if (!string.IsNullOrEmpty(arrayCosto[2]))
                notas.Add(new PrDnotasDTO() { Notadesc = "- " + ConstantesMigraciones.NotaCostoOperacionDolares });
            notas.Add(new PrDnotasDTO() { Notadesc = "- " + ConstantesMigraciones.NotaReservaEficiente });

            int rowNota = rowCosto + 2;
            ws.Cells[rowNota, colFecha + 1].Value = "NOTAS:";
            ws.Cells[rowNota, colFecha + 1].Style.Font.Bold = true;

            int cnotas = 1;
            foreach (var not in notas)
            {
                ws.Cells[rowNota + cnotas, colFecha + 2].Value = not.Notadesc;
                cnotas++;
            }

            rowNota += 3;
            ws.Cells[rowNota, colFecha + 1].Value = "LEYENDA:";
            ws.Cells[rowNota, colFecha + 1].Style.Font.Bold = true;
            ws.Cells[rowNota + 1, colFecha + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowNota + 1, colFecha + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            ws.Cells[rowNota + 1, colFecha + 2].Value = "- Despacho de máquinas duales que operen en diesel";

            #endregion

            ws.View.FreezePanes(rowGr + 1, colFecha + 1);
        }

        /// <summary>
        /// comentarios y colores por celda
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="bloque"></param>
        /// <param name="regDato"></param>
        /// <param name="h"></param>
        private static void AgregarDescripcionMediaHora(ExcelWorksheet ws, int row, int col, string bloque, DatoCDispatch regDato, int h)
        {
            if (regDato != null)
            {
                Celda30minCDispatch regH = regDato.Array30Min[h - 1];

                if (regDato.Ptomedicodi == 49462 && h == 15)
                { }

                string colorFenerg = GetBackgroundColorCeldaH(bloque, regH.Fenergcodi);
                UtilExcel.CeldasExcelColorFondo(ws, row, col, row, col, colorFenerg);

                string colorTexto = (regH.TieneHOnoPotencia) ? "#0000FF" : "#000000";
                UtilExcel.CeldasExcelColorTexto(ws, row, col, row, col, colorTexto);

                //
                UtilExcel.AgregarComentarioExcel(ws, row, col, string.Join("\n", regH.ListaMsj));
            }
        }

        private static void SetBorderEmpresaCDispatch(ExcelWorksheet ws, int sizeFont, int rowEmp, int colEmp, int colEmpFin, int rowGr, int rowDataTotal, int rowIniDetalle, int rowFinDetalle)
        {
            BorderCeldasSimple(ws, rowEmp, colEmp, rowDataTotal, colEmpFin);

            //formatear las primeras filas (empresa, ptomedicodi, grupo)
            FormatoCabeceraSinColor(ws, rowEmp, colEmp, rowDataTotal, colEmpFin, sizeFont, true);
            //formato 
            ws.Cells[rowEmp, colEmp, rowEmp, colEmpFin].Style.Font.Size = sizeFont - 2;

            //encapsular la primera fila y ultima fila de toda la data dentro de la empresa
            ws.Cells[rowEmp, colEmp, rowEmp, colEmpFin].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowEmp, colEmp, rowEmp, colEmpFin].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowGr, colEmp, rowGr, colEmpFin].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowDataTotal, colEmp, rowDataTotal, colEmpFin].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowDataTotal, colEmp, rowDataTotal, colEmpFin].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            //vertical
            ws.Cells[rowEmp, colEmp, rowDataTotal, colEmp].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            ws.Cells[rowEmp, colEmpFin, rowDataTotal, colEmpFin].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            ws.Cells[rowEmp, colEmp, rowGr, colEmpFin].Style.Font.Bold = true;
            ws.Cells[rowDataTotal, colEmp, rowDataTotal, colEmpFin].Style.Font.Bold = true;

            if (rowFinDetalle > rowIniDetalle)
            {
                BorderCeldasSimple(ws, rowDataTotal + 1, colEmp, rowFinDetalle, colEmpFin);
                FormatoCabeceraSinColor(ws, rowDataTotal + 1, colEmp, rowFinDetalle, colEmpFin, sizeFont, false);

                ws.Cells[rowIniDetalle, colEmp, rowIniDetalle, colEmpFin].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                ws.Cells[rowFinDetalle, colEmp, rowFinDetalle, colEmpFin].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                //vertical
                ws.Cells[rowDataTotal + 1, colEmp, rowFinDetalle, colEmp].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                ws.Cells[rowDataTotal + 1, colEmpFin, rowFinDetalle, colEmpFin].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            }
        }

        /// <summary>
        /// Obtener la descripción para mostrar el costo de la operación
        /// </summary>
        /// <param name="tipoAccion"></param>
        /// <param name="listaM1"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoCambio"></param>
        /// <returns></returns>
        public static string[] GetDescripcionCostoOperacion(int tipoAccion, List<MeMedicion1DTO> listaM1, DateTime fecha, decimal tipoCambio)
        {
            string[] array = new string[3];
            array[0] = "";
            array[1] = "";
            array[2] = ""; //tiene nota (S)

            MeMedicion1DTO regSoles = listaM1.Find(x => x.Ptomedicodi == ConstantesAppServicio.PtomedicodiCostoOpeDia && x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiSoles);
            MeMedicion1DTO regDolares = listaM1.Find(x => x.Ptomedicodi == ConstantesAppServicio.PtomedicodiCostoOperacionNCP && x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiDolares);

            if (regDolares != null)
            {
                regSoles = new MeMedicion1DTO
                {
                    H1 = regDolares.H1.GetValueOrDefault(0) * tipoCambio
                };
                array[2] = "S";
            }

            if (regSoles != null)
            {
                switch (tipoAccion)
                {
                    case Acciones.Consultar:
                        array[0] = "Costo Operación[" + fecha.Day + "]=";
                        array[1] = " S/. " + decimal.Round(regSoles.H1.Value, 0);
                        break;
                    case Acciones.Exportar:
                        array[0] = "COSTO TOTAL:";
                        array[1] = " " + decimal.Round(regSoles.H1.Value, 2) + " Soles";
                        break;
                }
            }

            return array;
        }

        private static void FormatoCabeceraSinColor(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin, int sizeFont, bool tieneWrap)
        {
            using (ExcelRange r1 = ws.Cells[rowIni, colIni, rowFin, colFin])
            {
                r1.Style.Font.Color.SetColor(Color.Black);
                r1.Style.Font.Size = sizeFont;
                r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r1.Style.Fill.BackgroundColor.SetColor(Color.White);
                r1.Style.WrapText = tieneWrap;
            }
        }

        private static void BorderCeldasSimple(ExcelWorksheet ws, int rowIni, int colIni, int rowFin, int colFin)
        {
            var borderTabla = ws.Cells[rowIni, colIni, rowFin, colFin].Style.Border;
            borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Thin;
        }

        #endregion

        #region Presentación CDispatch - Costo Operación

        /// <summary>
        /// ListarReporteCostoOperacionCDispatch
        /// </summary>
        /// <param name="regCDespacho"></param>
        /// <param name="reporteCDispatchOut"></param>
        public static void ListarReporteCostoOperacionCDispatch(CDespachoGlobal regCDespacho, ref ResultadoCDespacho reporteCDispatchOut)
        {
            List<MeMedicion1DTO> listaCostoTotal = new List<MeMedicion1DTO>();
            List<MeMedicion48DTO> listaDataGenCosto = new List<MeMedicion48DTO>();
            List<ResultadoValidacionAplicativo> listaMensajeValidacion = new List<ResultadoValidacionAplicativo>();

            //
            int lectcodi = regCDespacho.Lectcodi;
            List<EqPropequiDTO> listaPropequi = regCDespacho.ListaPropequi;
            List<MePtomedicionDTO> listaPtoDespacho30min = regCDespacho.ListaAllPtoPlantilla;

            // Generar la data de Reserva Fría
            for (DateTime f1 = regCDespacho.FechaIni; f1 <= regCDespacho.FechaFin; f1 = f1.AddDays(1))
            {
                CDespachoDiario regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == f1);
                List<MeMedicion48DTO> listaDataGenCostoXDia = new List<MeMedicion48DTO>();
                MeMedicion1DTO regCostoTotalXDia = new MeMedicion1DTO()
                {
                    Medifecha = f1,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiCostoOpeDia,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiSoles,
                    Lectcodi = regCDespacho.Lectcodi,
                    H1 = 0
                };

                //datos cada 30 minutos
                List<DatoCDispatch> listaDatoX30min = regCDespachoXDia.ListaDatoX30min;

                if (regCDespachoXDia == null)
                {
                    continue;
                }

                ResultadoCDespachoDiario regDiario = new ResultadoCDespachoDiario();
                List<ResultadoValidacionAplicativo> listaMjsXDia = new List<ResultadoValidacionAplicativo>();

                #region insumos de cada día 

                List<MeMedicion48DTO> listaMW48 = regCDespachoXDia.ListaMe48XDiaMW;
                List<MePtomedicionDTO> listaPtoDespacho30minXDia = regCDespachoXDia.ListaPtoXDia;
                List<MePtomedicionDTO> listaPtoDespachoTermo30min = listaPtoDespacho30minXDia.Where(x => x.Famcodi == 3 || x.Famcodi == 5).ToList();

                List<EqEquipoDTO> listaEquiposOC = regCDespachoXDia.ListaEquipoOC;
                List<PrGrupoDTO> listaGrupoOC = regCDespachoXDia.ListaGrupoXDia;
                List<PrGrupoDTO> listaModoOC = regCDespachoXDia.ListaModoOC;

                List<EqEquipoDTO> listaEquiposAll = regCDespachoXDia.ListaAllEquipo;
                List<PrGrupoDTO> listaGrupoAll = regCDespachoXDia.ListaAllGrupo;

                List<ConsumoHorarioCombustible> listaCurvaXDia = regCDespachoXDia.ListaCurvaXDia;
                List<PrCvariablesDTO> listaCostoVariableDia = regCDespachoXDia.ListaCostoVariableDia;

                #endregion

                foreach (var regPto in listaPtoDespachoTermo30min)
                {
                    if (regPto.Ptomedicodi == 236)
                    { }

                    DatoCDispatch regUnidad = listaDatoX30min.Find(x => x.ListagrupocodiDesp.Contains(regPto.Grupocodi ?? 0));

                    MeMedicion48DTO regCostoGenDia = new MeMedicion48DTO
                    {
                        Ptomedicodi = regPto.Ptomedicodi,
                        Medifecha = f1,
                        Grupocodi = regPto.Grupocodi ?? 0,
                        Catecodi = regPto.Catecodi,
                        Famcodi = regPto.Famcodi,
                        Emprnomb = regPto.Emprnomb,
                        Emprorden = regPto.Emprorden
                    };
                    regCostoGenDia.Emprorden = regPto.Emprorden;
                    regCostoGenDia.Gruponomb = regPto.Gruponomb;
                    regCostoGenDia.Grupoorden = regPto.Grupoorden;

                    #region Cálculo por punto de medicion

                    if (regUnidad != null)
                    {
                        for (int h = 1; h <= 48; h++)
                        {
                            DateTime fechaHora = f1.AddMinutes(h * 30);
                            Celda30minCDispatch regCeldaCO30min = regUnidad.Array30Min[h - 1];

                            //obtener datos de Calculo de Termicas
                            if (regCeldaCO30min != null)
                            {
                                bool esPintarDatosEnPto = false;
                                //si es ciclo combinado, visualizar el calculo en la TV
                                if (regUnidad.Unidad.TieneCicloComb && regUnidad.Unidad.EquicodiTVCicloComb > 0 && !regUnidad.Unidad.EsUnidadModoEspecial)
                                {
                                    if (regPto.Equicodi == regUnidad.Unidad.EquicodiTVCicloComb)
                                    {
                                        esPintarDatosEnPto = true;
                                    }
                                }
                                else
                                {
                                    //ciclo simple o especiales
                                    esPintarDatosEnPto = true;
                                }

                                if (esPintarDatosEnPto)
                                {
                                    regCostoGenDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(regCostoGenDia, regCeldaCO30min.CostoTotalDolares);

                                    regCostoGenDia.CostoConsumoCombustible += regCeldaCO30min.CvcCO;
                                    regCostoGenDia.CostoNoCombustible += regCeldaCO30min.CvncCO;
                                    regCostoGenDia.CostoCombustibleBajaEficiencia += regCeldaCO30min.Ccbef;
                                    regCostoGenDia.CostoArranque += regCeldaCO30min.CArrCO;

                                    LogCostoX30min(regCostoGenDia, regCeldaCO30min, h);

                                    //el costo total en soles
                                    regCostoTotalXDia.H1 += regCeldaCO30min.CostoTotalSoles;
                                }
                            }
                        }
                    }

                    #endregion

                    listaDataGenCostoXDia.Add(regCostoGenDia);
                }

                foreach (var regTmp in listaDataGenCostoXDia)
                {
                    LogCostoXDia(regTmp);
                }

                #region Totales

                //salidas
                regDiario.ListaMensajeValidacionXDia.AddRange(listaMjsXDia);
                regDiario.ListaCostoTotalXDia = listaDataGenCostoXDia;

                listaCostoTotal.Add(regCostoTotalXDia);
                listaDataGenCosto.AddRange(listaDataGenCostoXDia);

                #endregion
            }

            //salidas al objeto global
            reporteCDispatchOut.ListaCostoGenTotal = listaDataGenCosto;
            reporteCDispatchOut.ListaCostoOpTotal = listaCostoTotal;
        }

        /// <summary>
        /// Configura Formato de numeros al sistema internacional
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Devuelve el html  de costo de operacion  cada media hora
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="lCosto"></param>
        /// <param name="lPotenciaActiva"></param>
        /// <param name="tipoDatoMostrar"></param>
        /// <returns></returns>
        public static string ListarCostoOperacionDiarioHtml(DateTime fecha, List<MeMedicion48DTO> lCosto, List<MeMedicion48DTO> lPotenciaActiva, string tipoDatoMostrar)
        {
            lCosto = lCosto.Where(x => x.Catecodi == 3 || x.Catecodi == 4).ToList();
            lPotenciaActiva = lPotenciaActiva.Where(x => x.Famcodi == 3 || x.Famcodi == 5).ToList();

            List<MeMedicion48DTO> listaData48 = tipoDatoMostrar == "1" ? lCosto : lPotenciaActiva;

            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 1;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            int totalCol = (lCosto.Count * (100 + 20)) + 200;
            List<MeMedicion48DTO> lCostoOrdenado = new List<MeMedicion48DTO>();

            strHtml.Append("<div id='resultado' style=' overflow: auto; width: auto ;height :auto'>");
            strHtml.AppendFormat("<table id='reporte' class='pretty tabla-icono' style='table-layout: fixed; width: {0}px'>", totalCol);

            strHtml.Append("<thead>");
            #region cabecera

            // Empresas
            strHtml.Append("<tr>");
            strHtml.Append("<th style='width: 70px' rowspan='2'> Día / Hora</th>");
            List<SiEmpresaDTO> listaEmpresa = lCosto.GroupBy(x => new { x.Emprcodi, x.Emprnomb, x.Emprorden })
                .Select(x => new SiEmpresaDTO() { Emprcodi = x.Key.Emprcodi, Emprnomb = x.Key.Emprnomb, Emprorden = (x.Key.Emprorden > 0 ? x.Key.Emprorden : 9999) })
                .OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ToList();
            foreach (var obj in listaEmpresa)
            {
                List<PrGrupoDTO> listaGrupo = lCosto.Where(x => x.Emprcodi == obj.Emprcodi).GroupBy(x => new { x.Grupocodi, x.Gruponomb, x.Grupoorden })
                    .Select(x => new PrGrupoDTO() { Grupocodi = x.Key.Grupocodi, Gruponomb = x.Key.Gruponomb, Grupoorden = x.Key.Grupoorden })
                    .OrderBy(x => x.Grupoorden).ThenBy(x => x.Gruponomb).ToList();
                strHtml.AppendFormat("<th style='word-wrap: break-word;white-space: normal;width:{2}px' colspan='{1}'>{0}</th>", obj.Emprnomb.ToString(), listaGrupo.Count, 100 * listaGrupo.Count);
            }
            strHtml.Append("</tr>");

            // Grupos
            strHtml.Append("<tr>");
            foreach (var obj in listaEmpresa)
            {
                List<PrGrupoDTO> listaGrupo = lCosto.Where(x => x.Emprcodi == obj.Emprcodi).GroupBy(x => new { x.Grupocodi, x.Gruponomb, x.Grupoorden, x.Grupoabrev })
                    .Select(x => new PrGrupoDTO() { Grupocodi = x.Key.Grupocodi, Gruponomb = x.Key.Gruponomb, Grupoabrev = x.Key.Grupoabrev, Grupoorden = (x.Key.Grupoorden > 0 ? x.Key.Grupoorden : 9999) })
                    .OrderBy(x => x.Grupoorden).ThenBy(x => x.Gruponomb).ToList();
                foreach (var grupo in listaGrupo)
                {
                    strHtml.AppendFormat("<th style='word-wrap: break-word;white-space: normal;width:100px'>{0}</th>", grupo.Grupoabrev);
                    MeMedicion48DTO regcosto = lCosto.Find(x => x.Grupocodi == grupo.Grupocodi);
                    lCostoOrdenado.Add(regcosto);
                }
            }
            strHtml.Append("</tr>");

            #endregion
            strHtml.Append("</thead>");

            strHtml.Append("<tbody>");
            #region cuerpo

            // Día - Hora
            DateTime horas = fecha.Date.AddMinutes(30);

            // POTENCIA ACTIVA
            for (int h = 1; h <= 48; h++)
            {
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", horas.ToString(ConstantesAppServicio.FormatoHora)));

                foreach (var grupo in lCostoOrdenado)
                {
                    MeMedicion48DTO regpotActiva = listaData48.Find(x => x.Grupocodi == grupo.Grupocodi);
                    decimal? pot = null;
                    if (regpotActiva != null)
                    {
                        pot = (decimal?)regpotActiva.GetType().GetProperty("H" + h).GetValue(regpotActiva, null);
                    }

                    if (pot != null)
                    {
                        strHtml.Append(string.Format("<td>{0}</td>", pot != 0 ? pot.Value.ToString("N", nfi) : string.Empty));
                    }
                    else { strHtml.Append("<td></td>"); }
                }

                strHtml.Append("</tr>");

                horas = horas.AddMinutes(30);
            }

            // ENERGIA MWh
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte' style='height: 20px;'>MWh</td>");
            foreach (var grupo in lCostoOrdenado)
            {
                MeMedicion48DTO regpotActiva = lPotenciaActiva.Find(x => x.Grupocodi == grupo.Grupocodi);
                decimal total = regpotActiva != null ? regpotActiva.Meditotal.Value / ConstantesSioSein.FactorConversionPot30minToEnergiaHora : 0;
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", total.ToString("N", nfi)));
            }
            strHtml.Append("</tr>");

            // Costo Combustible
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte' style='height: 20px;'>CVC US$</td>");
            foreach (var grupo in lCostoOrdenado)
            {
                decimal total = grupo.CostoConsumoCombustible;
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", total.ToString("N", nfi)));
            }
            strHtml.Append("</tr>");

            // Costo No Combustible
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte' style='height: 20px;'>CVNC US$</td>");
            foreach (var grupo in lCostoOrdenado)
            {
                decimal total = grupo.CostoNoCombustible;
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", total.ToString("N", nfi)));
            }
            strHtml.Append("</tr>");

            // Costo Combustible baja Eficiencia
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte' style='height: 20px;'>CBEF US$</td>");
            foreach (var grupo in lCostoOrdenado)
            {
                decimal total = grupo.CostoCombustibleBajaEficiencia;
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", total.ToString("N", nfi)));
            }
            strHtml.Append("</tr>");

            // Costo Arranque
            strHtml.Append("<tr>");
            strHtml.Append("<td class='tdbody_reporte' style='height: 20px;'>CArr US$</td>");
            foreach (var grupo in lCostoOrdenado)
            {
                decimal total = grupo.CostoArranque;
                strHtml.Append(string.Format("<td class='tdbody_reporte'>{0}</td>", total.ToString("N", nfi)));
            }
            strHtml.Append("</tr>");


            #endregion
            strHtml.Append("</tbody>");

            strHtml.Append("</table>");
            strHtml.Append("</div>");

            return strHtml.ToString();
        }

        #endregion

        #region Oferta diaria

        /// <summary>
        /// ListarOfertaDiariaCDispatch
        /// </summary>
        /// <param name="regCDespacho"></param>
        public static void ListarPuntoMedicionMemoriaCDispatch(ref CDespachoGlobal regCDespacho)
        {
            DateTime fechaIni = regCDespacho.FechaIni;
            DateTime fechaFin = regCDespacho.FechaFin;
            int lectcodi = regCDespacho.Lectcodi;

            List<int> tipogrupocodis = new List<int>() { ConstantesMigraciones.TipogrupoTodos, ConstantesMigraciones.TipogrupoIntegrantes, ConstantesMigraciones.TipogrupoRER
                                                            , ConstantesMigraciones.TipogrupoCogeneracion, ConstantesMigraciones.TipogrupoNoIntegrantes};

            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                var regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == fecha);
                var regReservaFriaXDia = regCDespacho.ReporteRFria.ListaResultado.Find(x => x.Fecha == fecha);
                List<MeMedicion48DTO> lista48XDia = regCDespachoXDia.ListaMe48XDia;
                var listaGrXDia = regCDespachoXDia.ListaGrupoXDia;

                List<MeMedicion48DTO> listaCalculadoXDia = new List<MeMedicion48DTO>();

                //Generar objeto COES
                MeMedicion48DTO objGenCOES = new MeMedicion48DTO()
                {
                    Medifecha = fecha,
                    Ptomedicodi = ConstantesSiosein2.PtomedicodiCoesTotal,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Lectcodi = lectcodi
                };
                SetMeditotalXListaYGrupos(objGenCOES, listaGrXDia.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList(), lista48XDia.Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMW).ToList());
                listaCalculadoXDia.Add(objGenCOES);

                //Generar objeto COES y no COES
                MeMedicion48DTO objGen = new MeMedicion48DTO()
                {
                    Medifecha = fecha,
                    Ptomedicodi = ConstantesSiosein2.PtomedicodiGTotal,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Lectcodi = lectcodi
                };
                SetMeditotalXListaYGrupos(objGen, listaGrXDia, lista48XDia.Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMW).ToList());
                listaCalculadoXDia.Add(objGen);

                //Calculados Generación
                foreach (var t in tipogrupocodis)
                {
                    List<PrGrupoDTO> listaGrXTipo = new List<PrGrupoDTO>();

                    MeMedicion48DTO obj = new MeMedicion48DTO();
                    int ptomedicodiXTipogrupo = 0;

                    switch (t)
                    {
                        case ConstantesMigraciones.TipogrupoIntegrantes:
                            ptomedicodiXTipogrupo = ConstantesAppServicio.PtomedicodiGeneracionCoes;
                            listaGrXTipo = listaGrXDia.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();
                            break;
                        case ConstantesMigraciones.TipogrupoRER:
                            ptomedicodiXTipogrupo = ConstantesAppServicio.PtomedicodiTotalRenovab;
                            listaGrXTipo = listaGrXDia.Where(x => x.TipoGenerRer == ConstantesAppServicio.SI).ToList();
                            break;
                        case ConstantesMigraciones.TipogrupoCogeneracion:
                            ptomedicodiXTipogrupo = ConstantesAppServicio.PtomedicodiTotalCoGener;
                            listaGrXTipo = listaGrXDia.Where(x => x.Grupotipocogen == ConstantesAppServicio.SI).ToList();
                            break;
                        case ConstantesMigraciones.TipogrupoNoIntegrantes:
                            listaGrXTipo = listaGrXDia.Where(x => x.Grupointegrante != ConstantesAppServicio.SI).ToList();
                            break;
                    }

                    if (ptomedicodiXTipogrupo != 0)
                    {
                        MeMedicion48DTO objCalculadoMW = new MeMedicion48DTO()
                        {
                            Medifecha = fecha,
                            Ptomedicodi = ptomedicodiXTipogrupo,
                            Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMW,
                            Lectcodi = lectcodi
                        };
                        SetMeditotalXListaYGrupos(objCalculadoMW, listaGrXTipo, lista48XDia.Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMW).ToList());
                        listaCalculadoXDia.Add(objCalculadoMW);

                        MeMedicion48DTO objCalculadoMVar = new MeMedicion48DTO()
                        {
                            Medifecha = fecha,
                            Ptomedicodi = ptomedicodiXTipogrupo,
                            Tipoinfocodi = ConstantesAppServicio.TipoinfocodiMVAR,
                            Lectcodi = lectcodi
                        };
                        SetMeditotalXListaYGrupos(objCalculadoMVar, listaGrXTipo, lista48XDia.Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMVAR).ToList());
                        listaCalculadoXDia.Add(objCalculadoMVar);
                    }
                }

                //Demanda COES
                MeMedicion48DTO objDemanda = new MeMedicion48DTO()
                {
                    Medifecha = fecha,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiDemandaCoes,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Lectcodi = lectcodi
                };
                SetMeditotalXLista(objDemanda, new List<MeMedicion48DTO>() { objGenCOES });
                var regDemandaECUADOR = lista48XDia.Find(x => x.Ptomedicodi == ConstantesAppServicio.PtomedicodiDemandaECUADOR) ?? new MeMedicion48DTO();
                SetMeditotalXListaResta(objDemanda, new List<MeMedicion48DTO>() { regDemandaECUADOR });
                listaCalculadoXDia.Add(objDemanda);

                //Escenario
                MeMedicion48DTO objEscenario = new MeMedicion48DTO()
                {
                    Medifecha = fecha,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiEscenario,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Lectcodi = lectcodi
                };
                for (int i = 1; i <= 48; i++)
                {
                    objEscenario.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(objEscenario, 1.0m);
                }
                objEscenario.Meditotal = 48;
                listaCalculadoXDia.Add(objEscenario);

                //Reserva fría hidro
                MeMedicion48DTO objRFhidro = new MeMedicion48DTO
                {
                    Medifecha = fecha,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvFriaHidraulica,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Lectcodi = lectcodi,
                    Meditotal = 0.1m //si es 0 no se guarda en BD
                };
                listaCalculadoXDia.Add(objRFhidro);

                //Reserva rotante
                MeMedicion48DTO regRRTotalXDia = regReservaFriaXDia.RRTotalXDia;
                listaCalculadoXDia.Add(regRRTotalXDia);

                //Reserva fría térmica
                MeMedicion48DTO regRFTotalXDia = regReservaFriaXDia.RFTotalXDia;
                listaCalculadoXDia.Add(regRFTotalXDia);

                //Reserva fría COES (hidro + termo)
                MeMedicion48DTO objRFCOES = new MeMedicion48DTO()
                {
                    Medifecha = fecha,
                    Ptomedicodi = ConstantesAppServicio.PtomedicodiRsvFria,
                    Tipoinfocodi = ConstantesAppServicio.TipoinfocodiDemanda,
                    Lectcodi = lectcodi
                };
                SetMeditotalXLista(objRFCOES, new List<MeMedicion48DTO>() { regRFTotalXDia });
                listaCalculadoXDia.Add(objRFCOES);

                //Reserva eficiente
                List<MeMedicion48DTO> listaEfiXDia = regReservaFriaXDia.ListaREficienteXDia;
                listaCalculadoXDia.AddRange(listaEfiXDia);

                //salida
                regCDespachoXDia.ListaCalculadoGenXDia = listaCalculadoXDia;
            }
        }

        private static void SetMeditotalXListaYGrupos(MeMedicion48DTO regTotal, List<PrGrupoDTO> listaCabecera, List<MeMedicion48DTO> lista48XDia)
        {
            decimal totDia = 0;
            for (int h = 1; h <= 48; h++)
            {
                decimal tot = 0;
                foreach (var d in listaCabecera)
                {
                    var det = lista48XDia.Find(x => x.Ptomedicodi == d.Ptomedicodi);
                    if (det != null)
                    {
                        var val = (decimal?)det.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(det, null);
                        tot += (val ?? 0);
                    }
                }

                regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regTotal, tot);

                totDia += tot;
            }

            regTotal.Meditotal = totDia;
        }

        private static void SetMeditotalXListaResta(MeMedicion48DTO regTotal, List<MeMedicion48DTO> lista)
        {
            decimal totalReg = 0;
            for (int h = 1; h <= 48; h++)
            {
                decimal total = ((decimal?)regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(regTotal, null)).GetValueOrDefault(0);

                foreach (var m48 in lista)
                {
                    decimal? valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                    total -= valor.GetValueOrDefault(0);
                }

                regTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(regTotal, total);

                totalReg += total;
            }

            regTotal.Meditotal = totalReg;
        }

        #endregion

        #region Datos memoria

        /// <summary>
        /// ActualizarConDatoCalculado
        /// </summary>
        /// <param name="regInput"></param>
        /// <param name="regCDespacho"></param>
        public static void ActualizarConDatoCalculado(CDespachoInput regInput, ref CDespachoGlobal regCDespacho)
        {
            DateTime fechaIni = regCDespacho.FechaIni;
            DateTime fechaFin = regCDespacho.FechaFin;
            int lectcodi = regCDespacho.Lectcodi;
            bool esCalcularCostos = regInput.EsCalcularCostos;

            //Actualizar CDispatch diario
            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                var regCDespachoXDia = regCDespacho.ListaCDespachoDiario.Find(x => x.Fecha == fecha);

                //Actualizar Reserva fria
                regCDespachoXDia.ReservaFriaXDia = regCDespacho.ReporteRFria.ListaResultado.Find(x => x.Fecha == fecha);
                regCDespachoXDia.ListaMensajeValidacionXDia.AddRange(regCDespachoXDia.ReservaFriaXDia.ListaMensajeValidacionXDia);

                //Actualizar puntos de medición con cálculo en memoria
                List<MeMedicion48DTO> lista48XDia = regCDespachoXDia.ListaMe48XDia;

                List<int> listaPtomedicodi = new List<int>() { ConstantesSiosein2.PtomedicodiCoesTotal, ConstantesSiosein2.PtomedicodiGTotal,
                                            ConstantesAppServicio.PtomedicodiRsvRotante, ConstantesAppServicio.PtomedicodiRsvFria,
                                            ConstantesAppServicio.PtomedicodiRsvFriaTermica,ConstantesAppServicio.PtomedicodiRsvFriaHidraulica,
                                            ConstantesAppServicio.PtomedicodiRsvEficiente,ConstantesAppServicio.PtomedicodiRsvEficienteGas,ConstantesAppServicio.PtomedicodiRsvEficienteCarbon ,
                                            ConstantesAppServicio.PtomedicodiGeneracionCoes,ConstantesAppServicio.PtomedicodiTotalRenovab,ConstantesAppServicio.PtomedicodiTotalCoGener,
                                            ConstantesAppServicio.PtomedicodiEscenario,ConstantesAppServicio.PtomedicodiDemandaCoes};
                lista48XDia = lista48XDia.Where(x => !listaPtomedicodi.Contains(x.Ptomedicodi)).ToList();

                lista48XDia.AddRange(regCDespachoXDia.ListaCalculadoGenXDia);

                regCDespachoXDia.ListaMe48XDia = lista48XDia;

                //Actualizar costo
                regCDespachoXDia.ListaCostoXDia = regCDespacho.ReporteRFria.ListaCostoGenTotal.Where(x => x.Medifecha == fecha).ToList();
                if (esCalcularCostos)
                {
                    regCDespachoXDia.ListaMe1XDia = regCDespacho.ReporteRFria.ListaCostoOpTotal.Where(x => x.Medifecha == fecha).ToList();
                }

                //Actualizar detalle por celda
                regCDespachoXDia.ListaCelda = regCDespachoXDia.ReservaFriaXDia.ListaCelda;
            }

            //Actualizar CDispatch global
            List<MeMedicion48DTO> listaAll48Update = new List<MeMedicion48DTO>();
            List<MeMedicion1DTO> listaAll1Update = new List<MeMedicion1DTO>();
            List<MeMedicion48DTO> listaAllCostoUpdate = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaAllPotenciaUpdate = new List<MeMedicion48DTO>();

            foreach (var reg in regCDespacho.ListaCDespachoDiario)
            {
                listaAll48Update.AddRange(reg.ListaMe48XDia);
                listaAll1Update.AddRange(reg.ListaMe1XDia);
                listaAllCostoUpdate.AddRange(reg.ListaCostoXDia);
                listaAllPotenciaUpdate.AddRange(reg.ListaMe48XDia.Where(x => x.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW));

                var listaValPtoDespacho = UtilCdispatch.ValidarPtoMedicionDespacho(regCDespacho.ListaPtoOrig30min);
                reg.ListaMensajeValidacionXDia.AddRange(listaValPtoDespacho);
                reg.ListaMensajeValidacionXDia = reg.ListaMensajeValidacionXDia.Where(x => x != null).ToList();
            }

            regCDespacho.ListaAllCosto = listaAllCostoUpdate;
            regCDespacho.ListaAllPotencia = listaAllPotenciaUpdate;
            regCDespacho.ListaAllMe48 = listaAll48Update;
            if (esCalcularCostos) regCDespacho.ListaAllMe1 = listaAll1Update;
        }

        /// <summary>
        /// FormatearLista48
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="usuario"></param>
        /// <param name="fechaActualizacion"></param>
        /// <returns></returns>
        public static List<MeMedicion48DTO> FormatearLista48(List<MeMedicion48DTO> lista, string usuario, DateTime fechaActualizacion)
        {
            foreach (var reg in lista)
            {
                if (reg.Ptomedicodi != 0 && reg.Tipoinfocodi > 0)
                {
                    decimal sumaH = 0;

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valH = ((decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(reg, null)).GetValueOrDefault(0);

                        if (reg.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMW || reg.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiMVAR)
                        {
                            valH = Math.Round(valH, 5); //5 decimales máximo en BD
                        }
                        else
                        {
                            //costos
                            if (reg.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiDolar || reg.Tipoinfocodi == ConstantesAppServicio.TipoinfocodiSoles)
                            {
                                //si es de tipo moneda utilizar el redondeo financiero (que se usa en el SGOCOES)
                                MathHelper.Round(valH, 2);
                            }
                            else
                            {
                                valH = Math.Round(valH, 2); //hidrologia y calculos en memoria
                            }
                        }

                        reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).SetValue(reg, valH);

                        sumaH += valH;
                    }

                    reg.Meditotal = sumaH;

                    //se agrega esta condición para registrar la Reserva Hidraulica
                    if (reg.Ptomedicodi == ConstantesAppServicio.PtomedicodiRsvFriaHidraulica && reg.Meditotal.GetValueOrDefault(0) == 0)
                        reg.Meditotal = 0.1m;
                }

                reg.Mediestado = ConstantesAppServicio.Activo;
                reg.Lastuser = usuario;
                reg.Lastdate = fechaActualizacion;
            }

            return lista;
        }

        #endregion

        /// <summary>
        /// LogDiffSegundos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="contadorIni"></param>
        /// <param name="fechaActual"></param>
        /// <param name="contador"></param>
        /// <param name="lineNumber"></param>
        /// <param name="caller"></param>
        public static void LogDiffSegundos(DateTime fechaInicio, int contadorIni, out DateTime fechaActual, out int contador,
                        [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null)
        {
            var diffInSeconds = (DateTime.Now - fechaInicio).TotalSeconds;

            Debug.WriteLine(string.Format("Línea {3} ({4}).De Punto {0} a punto {1}: {2}", contadorIni, contadorIni + 1, diffInSeconds, lineNumber, caller));
            //Logger.Info(string.Format("De Punto {0} a punto {1}: {2}", contadorIni, contadorIni + 1, diffInSeconds));

            //siguiente
            fechaActual = DateTime.Now;
            contador = contadorIni + 1;
        }

        /// <summary>
        /// LogCostoXDia
        /// </summary>
        /// <param name="dato"></param>
        public static void LogCostoXDia(MeMedicion48DTO dato)
        {
            //Debug.WriteLine(string.Format("Código|Grupo|CVNC|CVC|CBEF|CARR"));
            /*
            Debug.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", dato.Grupocodi, dato.Gruponomb,
                       dato.CostoNoCombustible, dato.CostoConsumoCombustible, dato.CostoCombustibleBajaEficiencia, dato.CostoArranque));
            */
        }

        /// <summary>
        /// LogCostoX30min
        /// </summary>
        /// <param name="dato"></param>
        /// <param name="regCeldaCO30min"></param>
        /// <param name="h"></param>
        public static void LogCostoX30min(MeMedicion48DTO dato, Celda30minCDispatch regCeldaCO30min, int h)
        {
            //Debug.WriteLine(string.Format("Código|Grupo|Media hora|Código MO|MO|CVNC|CVC|Consumo comb|Costo comb|nhora|acum arr|acum npar|energía|código default|nombre default|n arr|CBE|costo comb|CBEalt|costo comb alt|costo arra|tcambio"));

            Debug.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}",
                        dato.Grupocodi, dato.Gruponomb, h, string.Join(", ", regCeldaCO30min.ListaGrupocodiModoOperativo), "",
                       regCeldaCO30min.CvncCO, regCeldaCO30min.CvcCO, regCeldaCO30min.ConsumoCombTmpCO, regCeldaCO30min.CostoCombCO, "0.5", regCeldaCO30min.TieneArranqueCO ? 1 : 0, regCeldaCO30min.TieneParadaCO ? 1 : 0,
                       regCeldaCO30min.EnergiaTmpCO, "", "", regCeldaCO30min.TieneArranqueCO ? 1 : 0, regCeldaCO30min.CbeTmpCO, regCeldaCO30min.CcombTmpCO, regCeldaCO30min.CbeAltTmpCO, regCeldaCO30min.CcombAltTmpCO, regCeldaCO30min.CArrCO, regCeldaCO30min.TCambio));

        }
    }

    #region Clases CDispatch

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Clase que almacena todos los datos del Cdispatch (varios días)
    /// </summary>
    public class CDespachoGlobal
    {
        public int Lectcodi { get; set; }
        public string Lectnomb { get; set; }

        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public List<DateTime> ListaFecha { get; set; } = new List<DateTime>();

        public decimal TipoCambioIni { get; set; }

        public ResultadoCDespacho ReporteRFria { get; set; } = new ResultadoCDespacho();
        public List<CDespachoDiario> ListaCDespachoDiario { get; set; } = new List<CDespachoDiario>();

        public List<PrGrupoDTO> ListaGrupoCabeceraWeb { get; set; } = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaCabeceraCdispatchActual { get; set; } = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaCabeceraCdispatchHistorico { get; set; } = new List<PrGrupoDTO>();

        public List<MePtomedicionDTO> ListaAllPtoPlantillaExcel { get; set; } = new List<MePtomedicionDTO>();
        public List<MePtomedicionDTO> ListaAllPtoPlantilla { get; set; } = new List<MePtomedicionDTO>();
        public List<PrGrupoDTO> ListaAllGrupoPlantilla { get; set; } = new List<PrGrupoDTO>();
        public List<SiEmpresaDTO> ListaSiEmpresaBD = new List<SiEmpresaDTO>();
        public List<PrGrupoDTO> ListaAllGrupoBD { get; set; } = new List<PrGrupoDTO>();

        public List<PrTipogrupoDTO> ListaTipoGrupo { get; set; } = new List<PrTipogrupoDTO>();
        public List<int> ListaCatecodiConData { get; set; } = new List<int>();
        public List<int> ListaEquicodiTieneConexionCamisea { get; set; } = new List<int>();
        public List<PrGrupoDTO> ListaGrupoXGrupoDespachoHistorico { get; set; } = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaGrupoXGrupoDespachoActivo { get; set; } = new List<PrGrupoDTO>();
        public List<MePtomedicionDTO> ListaPtoXGrupoDespacho { get; set; } = new List<MePtomedicionDTO>();
        public List<MePtomedicionDTO> ListaPtoXGrupoDespachoValido { get; set; } = new List<MePtomedicionDTO>();
        public List<MePtomedicionDTO> ListaPtoOrig30min { get; set; } = new List<MePtomedicionDTO>();

        public List<EqPropequiDTO> ListaPropequi { get; set; } = new List<EqPropequiDTO>();
        public List<EveManttoDTO> ListaManttoTermico { get; set; } = new List<EveManttoDTO>();
        public List<EveManttoDTO> ListaManttoTermicoCamisea { get; set; } = new List<EveManttoDTO>();
        public List<EveHoraoperacionDTO> ListaHOHoy { get; set; } = new List<EveHoraoperacionDTO>();
        public List<EveHoraoperacionDTO> ListaHOAyer { get; set; } = new List<EveHoraoperacionDTO>();
        public List<MeMedicion1DTO> ListaStockCombustibleProg { get; set; } = new List<MeMedicion1DTO>();
        public List<PrGrupodatDTO> ListaOperacionCoes { get; set; } = new List<PrGrupodatDTO>();

        public List<MeMedicion48DTO> ListaAllMe48 { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion1DTO> ListaAllMe1 { get; set; } = new List<MeMedicion1DTO>();
        public List<MeMedicion1DTO> ListaAllMe1BD { get; set; } = new List<MeMedicion1DTO>();
        public List<MeMedicion48DTO> ListaAllPotencia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaAllCosto { get; set; } = new List<MeMedicion48DTO>();

        public List<ResultadoValidacionAplicativo> ListaMensajeValidacion { get; set; } = new List<ResultadoValidacionAplicativo>();
    }

    /// <summary>
    /// Clase que almacena los insumos y resultados cálculo para el Cdispatch de un día específico
    /// </summary>
    public class CDespachoDiario
    {
        public DateTime Fecha { get; set; }
        public decimal TipoCambio { get; set; }
        public List<DatoCDispatch> ListaDatoX30min { get; set; }

        public List<DatoCDispatch> ListaCelda { get; set; } = new List<DatoCDispatch>();

        public ResultadoCDespachoDiario ReservaFriaXDia { get; set; } = new ResultadoCDespachoDiario();

        public List<MeMedicion48DTO> ListaCalculadoGenXDia { get; set; } = new List<MeMedicion48DTO>();

        public List<MeMedicion48DTO> ListaMe48XDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaMe48XDiaMW { get; set; } = new List<MeMedicion48DTO>();

        public List<MeMedicion1DTO> ListaMe1XDia { get; set; } = new List<MeMedicion1DTO>();
        public List<MeMedicion48DTO> ListaPotenciaXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaCostoXDia { get; set; } = new List<MeMedicion48DTO>();

        public List<PrGrupoDTO> ListaGrupoXDiaCabeceraExcel { get; set; } = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaGrupoXDia { get; set; } = new List<PrGrupoDTO>();
        public List<MePtomedicionDTO> ListaPtoXDia { get; set; } = new List<MePtomedicionDTO>();
        public List<MePtomedicionDTO> ListaPtoXDiaOC { get; set; } = new List<MePtomedicionDTO>();

        public List<EveManttoDTO> ListaManttoTermicoXDia { get; set; } = new List<EveManttoDTO>();
        public List<EveManttoDTO> ListaManttoTermicoCamiseaXDia { get; set; } = new List<EveManttoDTO>();
        public List<EveHoraoperacionDTO> ListaHOXDiaHoy { get; set; } = new List<EveHoraoperacionDTO>();
        public List<EveHoraoperacionDTO> ListaHOXDiaAyer { get; set; } = new List<EveHoraoperacionDTO>();
        public List<CDispatchDisponibilidadGas> ListaDispCombustible { get; set; } = new List<CDispatchDisponibilidadGas>();

        public List<EqEquipoDTO> ListaAllEquipo { get; set; } = new List<EqEquipoDTO>();
        public List<int> ListaEquicodiTieneConexionCamisea { get; set; } = new List<int>();
        public List<PrGrupoDTO> ListaAllGrupo { get; set; } = new List<PrGrupoDTO>();
        public List<PrTipogrupoDTO> ListaTipoGrupo { get; set; } = new List<PrTipogrupoDTO>();

        public List<EqEquipoDTO> ListaUnidadOC { get; set; } = new List<EqEquipoDTO>();
        public List<EqEquipoDTO> ListaUnidadEspOC { get; set; } = new List<EqEquipoDTO>();
        public List<EqEquipoDTO> ListaEquipoOC { get; set; } = new List<EqEquipoDTO>();
        public List<PrGrupoDTO> ListaModoOC { get; set; } = new List<PrGrupoDTO>();
        public List<PrGrupoDTO> ListaDespachoOC { get; set; } = new List<PrGrupoDTO>();

        public List<ConsumoHorarioCombustible> ListaCurvaXDia { get; set; } = new List<ConsumoHorarioCombustible>();
        public List<PrCvariablesDTO> ListaCostoVariableDia { get; set; } = new List<PrCvariablesDTO>();

        public List<PrDnotasDTO> ListaNotas = new List<PrDnotasDTO>();
        public List<ResultadoValidacionAplicativo> ListaMensajeValidacionXDia { get; set; } = new List<ResultadoValidacionAplicativo>();
    }

    public class CDespachoInput
    {
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public int Lectcodi { get; set; }
        public string Empresas { get; set; }
        public List<MeMedicion48DTO> ListaAllMe48 { get; set; }
        public List<MeMedicion1DTO> ListaAllMe1 { get; set; }

        public bool EsCalcularRFria { get; set; }
        public bool EsCalcularCostos { get; set; }
    }

    /// <summary>
    /// Clase temporal de los resultados del CDispatch (Reserva fría, Costo Operación)
    /// </summary>
    public class ResultadoCDespacho
    {
        public List<MePtomedicionDTO> ListaPto { get; set; }

        public List<ResultadoCDespachoDiario> ListaResultado { get; set; } = new List<ResultadoCDespachoDiario>();

        public List<ResultadoValidacionAplicativo> ListaMensajeValidacion { get; set; } = new List<ResultadoValidacionAplicativo>();

        public List<MeMedicion48DTO> ListaRFTotal { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRFDetalle { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRFXFenerg { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRFDataTotal { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRRTotal { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRRDetalle { get; set; } = new List<MeMedicion48DTO>();

        public List<MeMedicion48DTO> ListaMMTotal { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaMMDetalle { get; set; } = new List<MeMedicion48DTO>();

        public List<MeMedicion48DTO> ListaREficiente { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaREfiGasDetalle { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaREfiCarbonDetalle { get; set; } = new List<MeMedicion48DTO>();

        public List<MeMedicion48DTO> ListaCostoGenTotal { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion1DTO> ListaCostoOpTotal { get; set; } = new List<MeMedicion1DTO>();
    }

    public class ResultadoCDespachoDiario
    {
        public DateTime Fecha { get; set; }

        public List<DatoCDispatch> ListaCelda { get; set; } = new List<DatoCDispatch>();

        public List<ResultadoValidacionAplicativo> ListaMensajeValidacionXDia { get; set; } = new List<ResultadoValidacionAplicativo>();

        public MeMedicion48DTO RFTotalXDia { get; set; } = new MeMedicion48DTO();
        public MeMedicion48DTO RRTotalXDia { get; set; } = new MeMedicion48DTO();

        public List<MeMedicion48DTO> ListaRFDataTotalXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRFDetalleXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRFXFenergXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaRRDetalleXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaMMTotalXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaMMDetalleXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaREficienteXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaREfiGasXDia { get; set; } = new List<MeMedicion48DTO>();
        public List<MeMedicion48DTO> ListaREfiCarbonXDia { get; set; } = new List<MeMedicion48DTO>();

        public List<MeMedicion48DTO> ListaCostoTotalXDia { get; set; } = new List<MeMedicion48DTO>();
    }

    /// <summary>
    /// Datos de cada punto o grupo
    /// </summary>
    public class DatoCDispatch
    {
        public DateTime Fecha { get; set; }

        public EqEquipoDTO Unidad { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Famcodi { get; set; }
        public int Emprcodi { get; set; }
        public List<int> ListagrupocodiDesp { get; set; } = new List<int>();

        public MeMedicion48DTO M48 { get; set; }
        public int FenergcodiDefault { get; set; }
        public List<EqEquipoDTO> ListaCentralXGrupo { get; set; } = new List<EqEquipoDTO>();
        public List<EqEquipoDTO> ListaGeneradorXGrupo { get; set; } = new List<EqEquipoDTO>();

        public Celda30minCDispatch[] Array30Min { get; set; } = new Celda30minCDispatch[48];
    }

    /// <summary>
    /// Datos de cada 30 minutos de cada grupo
    /// </summary>
    public class Celda30minCDispatch
    {
        public List<int> ListaEquicodiAll { get; set; } = new List<int>();
        public List<int> ListaGrupocodiDespachoAll { get; set; } = new List<int>();
        public List<int> ListaGrupocodiModoAll { get; set; } = new List<int>();

        public List<int> ListaEquicodiOperativo { get; set; } = new List<int>();
        public List<int> ListaGrupocodiDespachoOperativo { get; set; } = new List<int>();
        public List<int> ListaGrupocodiModoOperativo { get; set; } = new List<int>();
        public List<int> ListaGrupocodiModoArranque { get; set; } = new List<int>();
        public List<int> ListaGrupocodiModoParada { get; set; } = new List<int>();

        public List<int> ListaEquicodiMantto { get; set; } = new List<int>();
        public List<int> ListaEquicodiManttoGas { get; set; } = new List<int>();
        public List<int> ListaGrupocodiModoMWManttoDisp { get; set; } = new List<int>();

        public List<int> ListaEquicodiTmin { get; set; } = new List<int>();

        public List<int> ListaEquicodiCandidatoRfria { get; set; } = new List<int>();

        public List<DatoCalculoHorario> ListaDatoDespachoOperativo { get; set; } = new List<DatoCalculoHorario>();

        public List<DatoCalculoHorario> ListaDatoDespachoDefault { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoModoOperativo { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoGeneradorOperativo { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoOperativoXFenerg { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoMantto { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoIndisponibilidad { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoRotante { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoRotanteXFenerg { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoRfria { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoRfriaXFenerg { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoMWMantto { get; set; } = new List<DatoCalculoHorario>();
        public List<DatoCalculoHorario> ListaDatoMWManttoXFenerg { get; set; } = new List<DatoCalculoHorario>();

        public DateTime FechaHora { get; set; }
        public int Fenergcodi { get; set; }
        public decimal ValorMWActiva { get; set; }
        public decimal ValorMWRotante { get; set; }
        public decimal ValorMWRfria { get; set; }
        public decimal ValorMWMantto { get; set; }

        //Reserva fría caso especial
        public bool TieneCasoEspecialRfria { get; set; }
        public decimal ValorMWRfriaGasEspecial { get; set; }

        public bool TieneGrupoOpera { get; set; }
        public bool TieneRotante { get; set; }
        public bool TieneMantto { get; set; }
        public bool TieneTminarranque { get; set; }
        public bool TieneRfria { get; set; }
        public bool TieneHOnoPotencia { get; set; }

        //costo operación
        public bool TieneArranqueCO { get; set; }
        public bool TieneParadaCO { get; set; }
        public decimal CostoTotalSoles { get; set; }
        public decimal CostoTotalDolares { get; set; }
        public decimal CvcCO { get; set; }
        public decimal CvncCO { get; set; }
        public decimal CostoCombCO { get; set; }
        public decimal CArrCO { get; set; }
        public decimal Ccbef { get; set; }
        public decimal TCambio { get; set; }

        public decimal EnergiaTmpCO { get; set; }
        public decimal ConsumoCombTmpCO { get; set; }
        public decimal CbeTmpCO { get; set; }
        public decimal CcombTmpCO { get; set; }
        public decimal CbeAltTmpCO { get; set; }
        public decimal CcombAltTmpCO { get; set; }

        //mensajes
        public List<string> ListaMsj { get; set; } = new List<string>();

        public List<string> ListaMsjRotanteCalculo { get; set; } = new List<string>();
        public List<string> ListaMsjRotanteValidacion { get; set; } = new List<string>();

        public List<string> ListaMsjRfriaCalculo { get; set; } = new List<string>();
        public List<string> ListaMsjRfriaValidacion { get; set; } = new List<string>();

        public List<string> ListaMsjOtrosValidacion { get; set; } = new List<string>();

    }

    public class DatoCalculoHorario
    {
        //Tipo. 1: Grupo despacho
        public int Tipo { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }

        //datos default
        public int Fenergcodi { get; set; }
        public bool TieneRotante { get; set; }
        public bool TieneMantto { get; set; }
        public bool TieneTminarranque { get; set; }
        public bool TieneRfria { get; set; }
        public bool TieneHOnoPotencia { get; set; }

        public decimal Valor { get; set; }
        public decimal ValorMW { get; set; }
        public decimal ValorMWCVC { get; set; }
        public decimal ValorMWCNVC { get; set; }

        public bool TieneHO { get; set; }
        public int Subcausacodi { get; set; }
        public string DetalleHO { get; set; }
        public string Detalle { get; set; }

        public decimal Pe { get; set; }
        public decimal Pmin { get; set; }

        public bool EsDisponible { get; set; }
        public bool EsOpera100 { get; set; }
        public bool EsOperaMenor100 { get; set; }
        public bool EsOperaMenorMin { get; set; }
        public bool TieneRfriaEspecial { get; set; }

        public DateTime? FechaDispAntes { get; set; }
        public DateTime? FechaUltimaHO { get; set; }
        public DateTime? Fecha { get; set; }
        public int Tminarranque { get; set; }

        public List<int> LAllEquipo { get; set; } = new List<int>();
        public List<int> LEquipoIndispXFenerg { get; set; } = new List<int>();
        public List<int> LEquipoXFenerg { get; set; } = new List<int>();
        public List<int> LGrupoDespXFenerg { get; set; } = new List<int>();
        public List<int> LGrupoModoXFenerg { get; set; } = new List<int>();
    }

    public class CDispatchDisponibilidadGas
    {
        public List<int> ListaGrupoDespachoOrden { get; set; }
        public List<decimal> ListaMWRfriaCandidato { get; set; }
        public List<int> ListaGrupocodiCentral { get; set; }

        public decimal CombustibleGasStockInicial { get; set; }
        public decimal CombustibleGasStockConsumido { get; set; }
        public decimal CombustibleGasStockDisponible { get; set; }

        public decimal[] Consumo30min { get; set; } = new decimal[48];
        public int[] GrupocodiConsumo30min { get; set; } = new int[48];

        public DateTime Fecha { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion
}
