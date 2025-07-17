using System;
using System.Linq;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using log4net;
using System.Globalization;
using System.Text;
using COES.Servicios.Aplicacion.Helper;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using COES.Framework.Base.Core;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.OperacionesVarias;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Aplicacion.Interconexiones
{
    /// <summary>
    /// Clases con métodos del módulo Interconexiones
    /// </summary>
    public class InterconexionesAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs 
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InterconexionesAppServicio));


        #region Tabla Mantenimientos

        public List<EveManttoDTO> ObtenerManttoEquipo(string idsEquipo, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetEveManttoRepository().ObtenerManttoEquipo(idsEquipo, 1, fechaInicio, fechaFin);
        }

        #endregion

        #region Métodos Tabla ME_LECTURA

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_LECTURA
        /// </summary>
        public List<MeLecturaDTO> ListMeLecturas()
        {
            return FactorySic.GetMeLecturaRepository().List();
        }

        #endregion

        #region Metodows Tabla IN_INTERCONEXION

        /// <summary>
        /// Lista de ptos de medicion que tienen interconexion
        /// </summary>
        /// <returns></returns>
        public List<InInterconexionDTO> ListInInterconexions()
        {
            return FactorySic.GetInInterconexionRepository().List();
        }

        #endregion

        #region Metodos Tabla EQ_PROPEQUI

        /// <summary>
        /// Devuelve el valor de la propiedad del equipo
        /// </summary>
        /// <param name="idPropiedad"></param>
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public string GetValorPropiedad(int idPropiedad, int idEquipo)
        {
            string valor = string.Empty;
            valor = FactorySic.GetEqPropequiRepository().GetValorPropiedad(idPropiedad, idEquipo);
            return valor;
        }

        #endregion

        #region Métodos Tabla ME_MEDICION96

        /// <summary>
        /// Obtiene el cuarto de hora de maxima intercambio del dia
        /// </summary>
        /// <param name="idPto"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public int GetMaximaDemandaDiaInterconexion(int idPto, DateTime fecha)
        {
            var registro = ObtenerHistoricoInterconexion(idPto, ConstantesInterconexiones.IdMedidorConsolidado, fecha, fecha).Where(x => x.Tipoinfocodi == 3).FirstOrDefault();
            decimal maximoValor = 0;
            int indiceMaximo = 1;
            for (int i = 1; i <= 96; i++)
            {
                var valor = (decimal?)registro.GetType().GetProperty("H" + i).GetValue(registro, null);
                if (valor != null)
                {
                    if (valor > maximoValor)
                    {
                        maximoValor = (decimal)valor;
                        indiceMaximo = i;
                    }
                }
            }
            return indiceMaximo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPtoInter"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ObtenerListaIntercambiosElectricidad(int idPtoInter, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idPtoInter);
            

            //Lista total por dia de interconexiones
            decimal? totalImpor = 0;
            decimal? totalExpor = 0;
            var listaHistorico = this.ObtenerDataHistoricaInterconexionByMedidor(idPtoInter, ConstantesInterconexiones.IdMedidorConsolidado, fechaInicio, fechaFin, false);
            var listaExport = listaHistorico.Where(x => x.Tipoinfocodi == 3 && x.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MWh);
            var listaImport = listaHistorico.Where(x => x.Tipoinfocodi == 3 && x.Ptomedicodi == ConstantesInterconexiones.IdImportacionL2280MWh);
            //Sumar total por dia


            foreach (var reg in listaExport)
            {
                totalImpor = 0M;
                totalExpor = 0M;
                decimal? maxExport = 0M;
                decimal? maxImport = 0M;
                var find = listaImport.ToList().Find(x => x.Medifecha == reg.Medifecha);
                for (var i = 1; i <= 96; i++)
                {
                    var valor = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                    if (valor != null)
                    {
                        totalExpor += valor;
                        if (valor > maxExport)
                            maxExport = valor;
                    }
                    if (find != null)
                    {
                        var valorImp = (decimal?)find.GetType().GetProperty("H" + i).GetValue(find, null);
                        if (valorImp != null)
                        {
                            totalImpor += valorImp;
                            if (valorImp > maxImport)
                                maxImport = valorImp;
                        }
                    }
                }
                MeMedicion24DTO registro = new MeMedicion24DTO();
                registro.Ptomedicodi = (int)interconexion.Ptomedicodi;
                registro.H1 = totalExpor;
                registro.H2 = maxExport * 4;
                registro.Medifecha = (DateTime)reg.Medifecha;
                registro.H3 = totalImpor;
                registro.H4 = maxImport * 4;
                lista.Add(registro);

            }
            //Lista total exportada en maxima demanda
            //Unirla y retornar la lista
            return lista;
        }

        /// <summary>
        /// Permite obtener el reporte de evolucion de energia
        /// </summary>
        /// <param name="idPtoInter"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<EstructuraEvolucionEnergia> ObtenerDataEvolucionEnergia(int idPtoInter, DateTime fechaInicio, DateTime fechaFin) 
        {
            List<EstructuraEvolucionEnergia> result = new List<EstructuraEvolucionEnergia>();
            List<EveIeodcuadroDTO> operaciones = (new OperacionesVariasAppServicio()).BuscarOperaciones(1, 219, fechaInicio, fechaFin, -1, -1);
            List<MeMedicion24DTO> data = this.ObtenerListaIntercambiosElectricidad(idPtoInter, fechaInicio, fechaFin);
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idPtoInter);
            MePtomedicionDTO ptoMedicion = FactorySic.GetMePtomedicionRepository().GetById((int)interconexion.Ptomedicodi);
            int idEquipo = (int)ptoMedicion.Equicodi;

            foreach(MeMedicion24DTO item in data)
            {
                
                List<EveIeodcuadroDTO> subOperaciones = operaciones.Where(x => x.Equicodi == idEquipo &&
                (((DateTime)x.Ichorini).Year == item.Medifecha.Year && ((DateTime)x.Ichorini).Month == item.Medifecha.Month && ((DateTime)x.Ichorini).Day == item.Medifecha.Day)).ToList();

                if (subOperaciones.Count > 0)
                {
                    bool flag = true;
                    foreach (EveIeodcuadroDTO itemOperacion in subOperaciones)
                    {
                        EstructuraEvolucionEnergia entity = new EstructuraEvolucionEnergia();
                        entity.Rowspan = (flag) ? subOperaciones.Count : 1;
                        entity.FlagAgrupacion = (flag) ? true : false;
                        entity.Fecha = item.Medifecha.ToString(ConstantesAppServicio.FormatoFecha);
                        entity.EnergiaExportada = (decimal)item.H1;
                        entity.MaximaEnergiaExportada = (decimal)item.H2;
                        entity.EnergiaImportada = (decimal)item.H3;
                        entity.MaximaEnergiaImportada = (decimal)item.H4;
                        entity.Inicio = ((DateTime)itemOperacion.Ichorini).ToString(ConstantesAppServicio.FormatoFechaFullSeg);
                        entity.Fin = ((DateTime)itemOperacion.Ichorfin).ToString(ConstantesAppServicio.FormatoFechaFullSeg);
                        flag = false;
                        result.Add(entity);
                    }
                }
                else 
                {
                    EstructuraEvolucionEnergia entity = new EstructuraEvolucionEnergia();
                    entity.Fecha = item.Medifecha.ToString(ConstantesAppServicio.FormatoFecha);
                    entity.EnergiaExportada = (decimal)item.H1;
                    entity.MaximaEnergiaExportada = (decimal)item.H2;
                    entity.EnergiaImportada = (decimal)item.H3;
                    entity.MaximaEnergiaImportada = (decimal)item.H4;
                    entity.Rowspan = 1;
                    entity.Inicio = string.Empty;
                    entity.FlagAgrupacion = true;
                    entity.Fin = string.Empty;
                    result.Add(entity);
                }

            }

            return result;
        }

        /// <summary>
        /// Obtiene Lista Historico Interconexion
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerHistoricoInterconexion(int idPtomedicion, int medidor, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ObtenerDataHistoricaInterconexionByMedidor(idPtomedicion, medidor, fechaInicio, fechaFin, false);
        }

        public List<MeMedicion96DTO> ObtenerDataHistoricaInterconexionByMedidor(int idPtomedicion, int medidor, DateTime fechaInicio, DateTime fechaFin, bool nuevoFormato)
        {
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> l = new List<MeMedicion96DTO>();            
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idPtomedicion);
            List<MeMedicion96DTO> listaData = ObtenerDataHistoricaInterconexion(idPtomedicion, fechaInicio, fechaFin);

            //fuente
            switch (medidor)
            {
                case ConstantesInterconexiones.IdMedidorConsolidado:
                    for (DateTime f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
                    {
                        var list = listaData.Where(x => x.Medifecha == f && (x.Ptomedicodi == interconexion.Ptomedicodi || x.Ptomedicodi == interconexion.Ptomedicodisecun )).ToList();
                        if (list.Count > 0)
                        {
                            l.AddRange(GetListaMedicion96MedidorConsolidado(list, (int)interconexion.Ptomedicodi, (int)interconexion.Ptomedicodisecun, f));
                        }
                    }
                    break;
                case ConstantesInterconexiones.IdMedidorPrincipal:
                    l = listaData.Where(x => x.Ptomedicodi == interconexion.Ptomedicodi).ToList();
                    break;
                case ConstantesInterconexiones.IdMedidorSecundario:
                    l = listaData.Where(x => x.Ptomedicodi == interconexion.Ptomedicodisecun).ToList();
                    break;
            }

            //final
            #region asignar puntos de medicion
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            ///Exportacion MWh
            lista = l.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).ToList();
            foreach (var m in lista)
            {
                if (!nuevoFormato)
                {
                    m.Ptomedicodi = ConstantesInterconexiones.IdExportacionL2280MWh;
                }
                else
                {
                    m.Ptomedicodi = medidor == ConstantesInterconexiones.IdMedidorPrincipal ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                }
            }

            ///Importacion MWh
            lista = l.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).ToList();
            foreach (var m in lista)
            {
                if (!nuevoFormato)
                {
                    m.Ptomedicodi = ConstantesInterconexiones.IdImportacionL2280MWh;
                }
                else
                {
                    m.Ptomedicodi = medidor == ConstantesInterconexiones.IdMedidorPrincipal ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                }
            }

            //Exportacion MVARh
            lista = l.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh).ToList();
            foreach (var m in lista)
            {
                if (!nuevoFormato)
                {
                    m.Ptomedicodi = ConstantesInterconexiones.IdExportacionL2280MVARr;
                }
                else
                {
                    m.Ptomedicodi = medidor == ConstantesInterconexiones.IdMedidorPrincipal ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                }
            }

            //Importacion MVARh
            lista = l.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh).ToList();
            foreach (var m in lista)
            {
                if (!nuevoFormato)
                {
                    m.Ptomedicodi = ConstantesInterconexiones.IdImportacionL2280MVARr;
                }
                else
                {
                    m.Ptomedicodi = medidor == ConstantesInterconexiones.IdMedidorPrincipal ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                }
            }

            //kV
            lista = l.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica).ToList();
            foreach (var m in lista)
            {
                if (!nuevoFormato)
                {
                    m.Ptomedicodi = ConstantesInterconexiones.IdPtoMedicionPrincipal;
                }
                else
                {
                    m.Ptomedicodi = medidor == ConstantesInterconexiones.IdMedidorPrincipal ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                }
            }

            //A
            lista = l.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica).ToList();
            foreach (var m in lista)
            {
                if (!nuevoFormato)
                {
                    m.Ptomedicodi = ConstantesInterconexiones.IdPtoMedicionPrincipal;
                }
                else
                {
                    m.Ptomedicodi = medidor == ConstantesInterconexiones.IdMedidorPrincipal ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                }
            }
            #endregion

            return l;
        }

        /// <summary>
        /// Lista de datos de interconexion para mostrar en la Extranet
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerDataHistoricaInterconexion(int idInterconexion, DateTime fechaInicio, DateTime fechaFin)
        {
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idInterconexion);
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaFinalTmp = new List<MeMedicion96DTO>();

            List<MeMedicion96DTO> listaOld = FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(ConstantesInterconexiones.IdReporteInterconexion, fechaInicio, fechaFin);
            List<MeMedicion96DTO> listaNew = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(ConstantesInterconexiones.IdFormatoInterconexion, ConstantesInterconexiones.IdEmpresaInterconexion, fechaInicio, fechaFin);

            List<MeEnvioDTO> listaEnvios = FactorySic.GetMeEnvioRepository().GetByCriteriaRango(ConstantesInterconexiones.IdEmpresaInterconexion, ConstantesInterconexiones.IdFormatoInterconexion, fechaInicio, fechaFin);
            List<MePeriodomedidorDTO> listaPeriodoMedidor = FactorySic.GetMePeriodomedidorRepository().GetByCriteriaRango(fechaInicio, fechaFin);

            List<MeMedicion96DTO> listaOldDay = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaNewDay = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listDay = new List<MeMedicion96DTO>();
            for (DateTime f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                listaOldDay = listaOld.Where(x => x.Medifecha == f).ToList();
                listaNewDay = listaNew.Where(x => x.Medifecha == f).ToList();

                //Principal y secundario en uno
                int numOld = listaOldDay.Where(x => x.Tipoptomedicodi == -1).Count();
                if (numOld == 6) //todos los tipoptomedicodi son -1 y los tipoinfocodi son 3,4,5,9 y los puntos 41238, 41239, 41240, 41241, 5020
                {
                    listaFinalTmp = new List<MeMedicion96DTO>();
                    #region trabajar la data antigua
                    MeMedicion96DTO m = null;
                    listDay = listaOldDay.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva).ToList();
                    foreach (var pto in listDay)
                    {
                        switch (pto.Ptomedicodi)
                        {
                            case ConstantesInterconexiones.IdExportacionL2280MWh:
                                pto.Ptomedicodi = (int)interconexion.Ptomedicodi;
                                pto.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh;
                                //agregar uno para secundario
                                m = new MeMedicion96DTO();
                                m.Medifecha = f;
                                m.Ptomedicodi = (int)interconexion.Ptomedicodisecun;
                                m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva;
                                m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh;
                                //listaFinal.Add(m);
                                break;
                            case ConstantesInterconexiones.IdImportacionL2280MWh:
                                pto.Ptomedicodi = (int)interconexion.Ptomedicodi;
                                pto.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh;
                                //agregar uno para secundario
                                m = new MeMedicion96DTO();
                                m.Medifecha = f;
                                m.Ptomedicodi = (int)interconexion.Ptomedicodisecun;
                                m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva;
                                m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh;
                                //listaFinal.Add(m);
                                break;
                        }
                    }

                    listaFinalTmp.AddRange(listDay);

                    listDay = listaOldDay.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva).ToList();
                    foreach (var pto in listDay)
                    {
                        switch (pto.Ptomedicodi)
                        {
                            case ConstantesInterconexiones.IdExportacionL2280MVARr:
                                pto.Ptomedicodi = (int)interconexion.Ptomedicodi;
                                pto.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh;
                                //agregar uno para secundario
                                m = new MeMedicion96DTO();
                                m.Medifecha = f;
                                m.Ptomedicodi = (int)interconexion.Ptomedicodisecun;
                                m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva;
                                m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh;
                                //listaFinal.Add(m);
                                break;
                            case ConstantesInterconexiones.IdImportacionL2280MVARr:
                                pto.Ptomedicodi = (int)interconexion.Ptomedicodi;
                                pto.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh;
                                //agregar uno para secundario
                                m = new MeMedicion96DTO();
                                m.Medifecha = f;
                                m.Ptomedicodi = (int)interconexion.Ptomedicodisecun;
                                m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva;
                                m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh;
                                //listaFinal.Add(m);
                                break;
                        }
                    }
                    listaFinalTmp.AddRange(listDay);

                    listDay = listaOldDay.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV || x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA).ToList();
                    foreach (var pto in listDay)
                    {
                        pto.Ptomedicodi = (int)interconexion.Ptomedicodi;
                        pto.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica;
                        //agregar uno para secundario
                        m = new MeMedicion96DTO();
                        m.Medifecha = f;
                        m.Ptomedicodi = (int)interconexion.Ptomedicodisecun;
                        m.Tipoinfocodi = pto.Tipoinfocodi;
                        m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica;
                        //listaFinal.Add(m);
                    }
                    listaFinalTmp.AddRange(listDay);

                    //traer data me_medidor
                    var listaEnviosDia = listaEnvios.Where(x => x.Enviofechaperiodo == f).ToList();
                    int idEnvio = listaEnviosDia.Count > 0 ? listaEnviosDia[listaEnviosDia.Count - 1].Enviocodi : 0;

                    if (idEnvio > 0)
                    {
                        var listaPeriodoMedidorXDia = listaPeriodoMedidor.Where(x => x.Earcodi == idEnvio).ToList();
                        if (listaPeriodoMedidorXDia.Count > 0)
                        {
                            List<MeMedicion96DTO> listaDataHistorica = listaFinalTmp.Where(x => x.Ptomedicodi == (int)interconexion.Ptomedicodi).ToList();
                            List<MeMedicion96DTO> listaTmp = new List<MeMedicion96DTO>();
                            foreach (var m96 in listaFinalTmp)
                            {
                                MeMedicion96DTO mtmp = new MeMedicion96DTO();
                                mtmp.Medifecha = m96.Medifecha;
                                mtmp.Ptomedicodi = m96.Ptomedicodi;
                                mtmp.Tipoinfocodi = m96.Tipoinfocodi;
                                mtmp.Tipoptomedicodi = m96.Tipoptomedicodi;
                                listaTmp.Add(mtmp);
                            }

                            foreach (var reg in listaPeriodoMedidorXDia)
                            {
                                int idMedidor = (int)reg.Medicodi;
                                int ptomedicodi = ConstantesInterconexiones.IdMedidorPrincipal == idMedidor ? (int)interconexion.Ptomedicodi : (int)interconexion.Ptomedicodisecun;
                                int hIni = (reg.Permedifechaini.Value.Hour * 60 + reg.Permedifechaini.Value.Minute) / 15;
                                int hFin = (reg.Permedifechafin.Value.Hour * 60 + reg.Permedifechafin.Value.Minute) / 15;
                                hFin = hFin == 0 ? 96 : hFin;

                                foreach (var reg96 in listaTmp)
                                {
                                    if (reg96.Ptomedicodi == ptomedicodi)
                                    {
                                        for (int i = hIni; i <= hFin; i++)
                                        {
                                            var regHist = listaDataHistorica.Find(x => x.Medifecha.Value == reg96.Medifecha.Value && x.Tipoinfocodi == reg96.Tipoinfocodi && x.Tipoptomedicodi == reg96.Tipoptomedicodi);
                                            if (regHist != null)
                                            {
                                                decimal? valor = (decimal?)regHist.GetType().GetProperty("H" + i).GetValue(regHist, null);
                                                reg96.GetType().GetProperty("H" + i).SetValue(reg96, valor);
                                            }
                                        }
                                    }
                                }
                            }

                            //listaFinal = listaFinalTmp;
                            listaFinal.AddRange(listaFinalTmp);
                        }
                    }
                    else
                    {
                        listaFinal.AddRange(listaFinalTmp);
                    }

                    #endregion
                }
                else
                {
                    //Principal y secundario separados
                    listaFinal.AddRange(listaNewDay);
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// Lista de medidores de la aplicación
        /// </summary>
        /// <returns></returns>
        public List<MeMedidorDTO> GetListaMedidorApp()
        {
            List<MeMedidorDTO> l = new List<MeMedidorDTO>();
            l.Add(new MeMedidorDTO() { Medicodi = ConstantesInterconexiones.IdMedidorConsolidado, Medinombre = ConstantesInterconexiones.NombMedidorConsolidado });
            l.Add(new MeMedidorDTO() { Medicodi = ConstantesInterconexiones.IdMedidorPrincipal, Medinombre = ConstantesInterconexiones.NombMedidorPrincipal });
            l.Add(new MeMedidorDTO() { Medicodi = ConstantesInterconexiones.IdMedidorSecundario, Medinombre = ConstantesInterconexiones.NombMedidorSecundario });
            return l;
        }

        /// <summary>
        /// Permite obtener un registro de interxonexion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InInterconexionDTO ObtenerInterconexion(int id)
        {
            return FactorySic.GetInInterconexionRepository().GetById(id);
        }

        /// <summary>
        /// Obtiene Listado Historico de interconexiones por parametro (MW,MVAr,KV,Amp)
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="idParametro"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerInterconexionParametro(int idPtomedicion, int idParametro, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion96DTO> list = this.ObtenerDataHistoricaInterconexionByMedidor(idPtomedicion, ConstantesInterconexiones.IdMedidorConsolidado, fechaInicio, fechaFin, false);
            List<MeMedicion96DTO> listParam = new List<MeMedicion96DTO>();
            switch (idParametro)
            {
                case 1:
                case 3:
                    listParam = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva).ToList();
                    break;
                case 2:
                case 4:
                    listParam = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva).ToList();
                    break;
                case 5:
                    listParam = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV || x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA).ToList();
                    break;
            }

            return listParam;
        }

        /// <summary>
        /// Obtiene el reporte historico por pagina
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public string ObtenerConsultaHistoricaPagInterconexion(int idPtomedicion, DateTime fechaInicio, DateTime fechaFin, int pagina)
        {
            var paginas = FactorySic.GetMeMedicion96Repository().PaginacionInterconexiones(3, fechaInicio, fechaFin);
            List<MeMedicion96DTO> list = new List<MeMedicion96DTO>();
            if (paginas.Count() > 0)
            {
                list = FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(3, paginas[pagina - 1], paginas[pagina - 1]);
            }
            return ObtenerHtmlReporteInterconexion(idPtomedicion, list, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite obtener el log de errores
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<LogErrorInterconexion> ValidarPeriodo(int idPtomedicion, DateTime fechaInicio, DateTime fechaFin)
        {
            List<LogErrorInterconexion> result = new List<LogErrorInterconexion>();

            List<EveIeodcuadroDTO> operaciones = (new OperacionesVariasAppServicio()).BuscarOperaciones(1, 219, fechaInicio, fechaFin, -1, -1);
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idPtomedicion);
            MePtomedicionDTO ptoMedicion = FactorySic.GetMePtomedicionRepository().GetById((int)interconexion.Ptomedicodi);
            int equiCodi = (int)ptoMedicion.Equicodi;
            List<MeMedidorDTO> medidores = this.GetListaMedidorApp();

            foreach (MeMedidorDTO medidor in medidores)
            {
                List<MeMedicion96DTO> entitys = this.ObtenerHistoricoInterconexion(idPtomedicion, medidor.Medicodi, fechaInicio, fechaFin);

                for (DateTime fecha = fechaInicio.Date; fecha <= fechaFin.Date; fecha = fecha.AddDays(1))
                {
                    List<EveIeodcuadroDTO> subOperaciones = operaciones.Where(x => x.Equicodi == equiCodi &&
                    ((DateTime)x.Ichorini).Subtract(fecha).TotalSeconds >=0 &&
                            ((DateTime)x.Ichorfin).Subtract(fecha.AddDays(1)).TotalSeconds <= 0).ToList();
                    
                    //Exportacion MWh
                    MeMedicion96DTO exportacionMwh = entitys.Where(x => x.Medifecha == fecha && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault();
                    //Importacion MWh
                    MeMedicion96DTO importacionMwh = entitys.Where(x => x.Medifecha == fecha && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault();
                    result.AddRange(this.ValidarData(exportacionMwh, importacionMwh, subOperaciones, fecha, interconexion.Interenlace, "Energía Activa", medidor.Medinombre));

                    //Exportacion MVARh
                    MeMedicion96DTO exportacionMvarh = entitys.Where(x => x.Medifecha == fecha && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh).FirstOrDefault();
                    //Importacion MVARh
                    MeMedicion96DTO importacionMvarh = entitys.Where(x => x.Medifecha == fecha && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh).FirstOrDefault();
                    result.AddRange(this.ValidarData(exportacionMvarh, importacionMvarh, subOperaciones, fecha, interconexion.Interenlace, "Energía Reactiva", medidor.Medinombre));

                    //kV
                    MeMedicion96DTO kv = entitys.Where(x => x.Medifecha == fecha && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica).FirstOrDefault();
                    result.AddRange(this.ValidarData(kv, null, subOperaciones, fecha, interconexion.Interenlace, "Tensión", medidor.Medinombre));

                    //A  
                    MeMedicion96DTO A = entitys.Where(x => x.Medifecha == fecha && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica).FirstOrDefault();
                    result.AddRange(this.ValidarData(A, null, subOperaciones, fecha, interconexion.Interenlace, "Amperior", medidor.Medinombre));
                }
            }

            return result;
        }

        /// <summary>
        /// Permite validar los registros de mediciones y operaciones varias
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <param name="operaciones"></param>
        /// <param name="fecha"></param>
        /// <param name="equipo"></param>
        /// <param name="parametro"></param>
        /// <param name="medidor"></param>
        /// <returns></returns>
        private List<LogErrorInterconexion> ValidarData(MeMedicion96DTO entity1, MeMedicion96DTO entity2, 
            List<EveIeodcuadroDTO> operaciones, DateTime fecha, string equipo, string parametro, string medidor)
        {
            List<LogErrorInterconexion> result = new List<LogErrorInterconexion>();

            if (entity1 != null || entity2 != null)
            {
                decimal suma = 0;
                int[] datos = new int[96];
                for (int i = 1; i <= 96; i++)
                {
                    object val1 = (entity1 != null) ? entity1.GetType().GetProperty("H" + i).GetValue(entity1) : null;
                    object val2 = (entity2 != null) ? entity2.GetType().GetProperty("H" + i).GetValue(entity2) : null;
                    decimal valor1 = (val1 != null) ? (decimal)val1 : 0;
                    decimal valor2 = (val2 != null) ? (decimal)val2 : 0;
                    decimal valor = valor1 + valor2;
                    datos[i - 1] = 0;
                    if (valor != 0) datos[i - 1] = 1;
                    suma = suma + valor;
                }

                if (suma != 0 && operaciones.Count == 0)
                {
                    result.Add(new LogErrorInterconexion { Linea = equipo, Fecha = fecha.ToString(ConstantesAppServicio.FormatoFecha), Comentario = "Existen mediciones pero no se encuentra registro en Operaciones Varias", Parametro = parametro, Medidor = medidor });
                }

                if(operaciones.Count > 0)
                {                    
                    int[] indicador = new int[96];

                    for (int i = 0; i < 96; i++) indicador[i] = 0;

                    foreach(EveIeodcuadroDTO itemOperacion in operaciones)
                    {
                        int minutosInicio = ((DateTime)itemOperacion.Ichorini).Hour * 60 + ((DateTime)itemOperacion.Ichorini).Minute;
                        int minutosFin = ((DateTime)itemOperacion.Ichorfin).Hour * 60 + ((DateTime)itemOperacion.Ichorfin).Minute;
                        int inicio = (int)(minutosInicio / 15) + 1;
                        int fin = (int)(minutosFin / 15) + 1;

                        if(((DateTime)itemOperacion.Ichorfin).Hour == 0 && ((DateTime)itemOperacion.Ichorfin).Minute == 0)
                        {
                            fin = 96;
                        }

                        for(int k = inicio; k<= fin; k++)
                        {
                            indicador[k - 1] = 1;
                        }
                    }

                    
                    for(int i = 1; i<= 96; i++)
                    {
                        //DateTime fechaConsulta = fecha.AddMinutes(15 * (i-1));
                        //EveIeodcuadroDTO itemOperacion = operaciones.Where(x => ((DateTime)x.Ichorini).Subtract(fechaConsulta).TotalSeconds <= 0 &&
                        //    ((DateTime)x.Ichorfin).Subtract(fechaConsulta).TotalSeconds >= 0).FirstOrDefault();

                        DateTime fechaPeriodo = fecha.AddMinutes(15 * i);

                        if ((datos[i - 1] == 0 && indicador[i-1] == 1) || (datos[i - 1] != 0 && indicador[i - 1] == 0))
                        {
                            result.Add(new LogErrorInterconexion { Linea = equipo, Fecha = fecha.ToString(ConstantesAppServicio.FormatoFecha), Comentario = "Los rangos de fechas de las mediciones no coinciden con lo registrado en Operaciones Varias en el periodo " +
                                fechaPeriodo.ToString(ConstantesAppServicio.FormatoHora), Parametro = parametro, Medidor = medidor });
                            //break;
                        }

                    }
                }
            }
            else 
            {
                if(operaciones.Count > 0)
                {
                    result.Add(new LogErrorInterconexion { Linea = equipo, Fecha = fecha.ToString(ConstantesAppServicio.FormatoFecha), Comentario = "Existen registros en Operaciones Varias pero no se encuentra mediciones", Parametro = parametro, Medidor = medidor });
                }
            }


            return result;
        }

        /// <summary>
        /// Genera Reporte Historico de datos de interconexion
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fini"></param>
        /// <param name="ffin"></param>
        /// <returns></returns>
        public string ObtenerHtmlReporteInterconexion(int idInterconexion, List<MeMedicion96DTO> lista, DateTime fini, DateTime ffin)
        {
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idInterconexion);
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th colspan='7'>LINEA DE TRANSMISIÓN " + interconexion.Interenlace + "</th>");

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MWh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MWh</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MVARh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MVARh</th>");
            strHtml.Append("<th>" + interconexion.Internomlinea + " kV</th>");
            strHtml.Append("<th>" + interconexion.Internomlinea + " Amp.</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;
            var ptos = GetByCriteriaMeReporptomeds(3);
            for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
            {
                var list = lista.Where(x => x.Medifecha == f).ToList();
                if (list.Count() != 0)

                    for (int k = 1; k <= 96; k++)
                    {
                        minuto = minuto + 15;
                        for (int j = 0; j < ptos.Count(); j++)
                        {
                            if (j == 0)
                            {
                                strHtml.Append("<tr>");
                                strHtml.Append(string.Format("<td>{0}</td>", list[0].Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                            }

                            decimal valor = 0M;
                            MeMedicion96DTO entity = list.Where(x => x.Ptomedicodi == ptos[j].Ptomedicodi && x.Tipoinfocodi ==
                                ptos[j].Tipoinfocodi).ToList().FirstOrDefault();
                            if (entity != null)
                            {
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                string kVvalor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", kVvalor));
                            }
                            else
                            {
                                strHtml.Append(string.Format("<td>--</td>"));
                            }
                            if (j == ptos.Count() - 1)
                            {
                                strHtml.Append("</tr>");
                            }
                        }


                    }

            }
            if (lista.Count == 0)
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fecha"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="tipoptomedicodi"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> GetListaMedicion96MedidorConsolidado(List<MeMedicion96DTO> lista, int ptoMedicionPrincipal, int ptoMedicionSecundario, DateTime fecha)
        {
            List<MeMedicion96DTO> listaFinal = new List<MeMedicion96DTO>();
            MeMedicion96DTO m = null;

            m = new MeMedicion96DTO();
            m.Medifecha = fecha;
            m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva;
            m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh;
            listaFinal.Add(m);

            m = new MeMedicion96DTO();
            m.Medifecha = fecha;
            m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva;
            m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh;
            listaFinal.Add(m);

            m = new MeMedicion96DTO();
            m.Medifecha = fecha;
            m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva;
            m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh;
            listaFinal.Add(m);

            m = new MeMedicion96DTO();
            m.Medifecha = fecha;
            m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva;
            m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh;
            listaFinal.Add(m);

            m = new MeMedicion96DTO();
            m.Medifecha = fecha;
            m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiKV;
            m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica;
            listaFinal.Add(m);

            m = new MeMedicion96DTO();
            m.Medifecha = fecha;
            m.Tipoinfocodi = ConstantesInterconexiones.IdTipoInfocodiA;
            m.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica;
            listaFinal.Add(m);

            var mPriExportacionMwh = lista.Find(x => x.Ptomedicodi == ptoMedicionPrincipal && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh);
            var mPriImportacionMwh = lista.Find(x => x.Ptomedicodi == ptoMedicionPrincipal && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh);
            var mPriExportacionMVarh = lista.Find(x => x.Ptomedicodi == ptoMedicionPrincipal && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh);
            var mPriImportacionMVarh = lista.Find(x => x.Ptomedicodi == ptoMedicionPrincipal && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh);
            var mPriKV = lista.Find(x => x.Ptomedicodi == ptoMedicionPrincipal && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica);
            var mPriA = lista.Find(x => x.Ptomedicodi == ptoMedicionPrincipal && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica);

            var mSecExportacionMwh = lista.Find(x => x.Ptomedicodi == ptoMedicionSecundario && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh);
            var mSecImportacionMwh = lista.Find(x => x.Ptomedicodi == ptoMedicionSecundario && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh);
            var mSecExportacionMVarh = lista.Find(x => x.Ptomedicodi == ptoMedicionSecundario && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh);
            var mSecImportacionMVarh = lista.Find(x => x.Ptomedicodi == ptoMedicionSecundario && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh);
            var mSecKV = lista.Find(x => x.Ptomedicodi == ptoMedicionSecundario && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica);
            var mSecA = lista.Find(x => x.Ptomedicodi == ptoMedicionSecundario && x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica);

            for (int i = 1; i <= 96; i++)
            {
                decimal? valorPriExportacionMwh = null;
                decimal? valorPriImportacionMwh = null;
                decimal? valorSecExportacionMwh = null;
                decimal? valorSecImportacionMwh = null;
                if (mPriExportacionMwh != null)
                {
                    valorPriExportacionMwh = (decimal?)mPriExportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mPriExportacionMwh, null);
                }
                if (mPriImportacionMwh != null)
                {
                    valorPriImportacionMwh = (decimal?)mPriImportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mPriImportacionMwh, null);
                }
                if (mSecExportacionMwh != null)
                {
                    valorSecExportacionMwh = (decimal?)mSecExportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mSecExportacionMwh, null);
                }
                if (mSecImportacionMwh != null)
                {
                    valorSecImportacionMwh = (decimal?)mSecImportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mSecImportacionMwh, null);
                }

                bool hayDataPrincipal = valorPriExportacionMwh.GetValueOrDefault(0) != 0 || valorPriImportacionMwh.GetValueOrDefault(0) != 0;
                bool hayDataSecundario = valorSecExportacionMwh.GetValueOrDefault(0) != 0 || valorSecImportacionMwh.GetValueOrDefault(0) != 0;

                var mExportacionMwh = listaFinal.Find(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh);
                var mImportacionMwh = listaFinal.Find(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh);
                var mExportacionMVarh = listaFinal.Find(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMVarh);
                var mImportacionMVarh = listaFinal.Find(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMVarh);
                var mKV = listaFinal.Find(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica);
                var mA = listaFinal.Find(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiMedidaElectrica);

                decimal? newValExportacionMwh = null;
                decimal? newValImportacionMwh = null;
                decimal? newValExportacionMVarh = null;
                decimal? newValImportacionMVarh = null;
                decimal? newValKV = null;
                decimal? newValA = null;
                int tipoMedidor = 0;
                if (hayDataPrincipal || !hayDataSecundario)
                {
                    if (mPriExportacionMwh != null)
                    {
                        newValExportacionMwh = (decimal?)mPriExportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mPriExportacionMwh, null);
                    }
                    if (mPriImportacionMwh != null)
                    {
                        newValImportacionMwh = (decimal?)mPriImportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mPriImportacionMwh, null);
                    }
                    if (mPriExportacionMVarh != null)
                    {
                        newValExportacionMVarh = (decimal?)mPriExportacionMVarh.GetType().GetProperty("H" + i.ToString()).GetValue(mPriExportacionMVarh, null);
                    }
                    if (mPriImportacionMVarh != null)
                    {
                        newValImportacionMVarh = (decimal?)mPriImportacionMVarh.GetType().GetProperty("H" + i.ToString()).GetValue(mPriImportacionMVarh, null);
                    }
                    if (mPriKV != null)
                    {
                        newValKV = (decimal?)mPriKV.GetType().GetProperty("H" + i.ToString()).GetValue(mPriKV, null);
                    }
                    if (mPriA != null)
                    {
                        newValA = (decimal?)mPriA.GetType().GetProperty("H" + i.ToString()).GetValue(mPriA, null);
                    }
                    tipoMedidor = ConstantesInterconexiones.IdMedidorPrincipal;
                }
                else
                {
                    newValExportacionMwh = (decimal?)mSecExportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mSecExportacionMwh, null);
                    newValImportacionMwh = (decimal?)mSecImportacionMwh.GetType().GetProperty("H" + i.ToString()).GetValue(mSecImportacionMwh, null);
                    newValExportacionMVarh = (decimal?)mSecExportacionMVarh.GetType().GetProperty("H" + i.ToString()).GetValue(mSecExportacionMVarh, null);
                    newValImportacionMVarh = (decimal?)mSecImportacionMVarh.GetType().GetProperty("H" + i.ToString()).GetValue(mSecImportacionMVarh, null);
                    newValKV = (decimal?)mSecKV.GetType().GetProperty("H" + i.ToString()).GetValue(mSecKV, null);
                    newValA = (decimal?)mSecA.GetType().GetProperty("H" + i.ToString()).GetValue(mSecA, null);
                    tipoMedidor = ConstantesInterconexiones.IdMedidorSecundario;
                }

                mExportacionMwh.GetType().GetProperty("H" + i.ToString()).SetValue(mExportacionMwh, newValExportacionMwh);
                mExportacionMwh.GetType().GetProperty("OrigenMedidorH" + i.ToString()).SetValue(mExportacionMwh, tipoMedidor);
                mImportacionMwh.GetType().GetProperty("H" + i.ToString()).SetValue(mImportacionMwh, newValImportacionMwh);
                mImportacionMwh.GetType().GetProperty("OrigenMedidorH" + i.ToString()).SetValue(mImportacionMwh, tipoMedidor);
                mExportacionMVarh.GetType().GetProperty("H" + i.ToString()).SetValue(mExportacionMVarh, newValExportacionMVarh);
                mExportacionMVarh.GetType().GetProperty("OrigenMedidorH" + i.ToString()).SetValue(mExportacionMVarh, tipoMedidor);
                mImportacionMVarh.GetType().GetProperty("H" + i.ToString()).SetValue(mImportacionMVarh, newValImportacionMVarh);
                mImportacionMVarh.GetType().GetProperty("OrigenMedidorH" + i.ToString()).SetValue(mImportacionMVarh, tipoMedidor);
                mKV.GetType().GetProperty("H" + i.ToString()).SetValue(mKV, newValKV);
                mKV.GetType().GetProperty("OrigenMedidorH" + i.ToString()).SetValue(mKV, tipoMedidor);
                mA.GetType().GetProperty("H" + i.ToString()).SetValue(mA, newValA);
                mA.GetType().GetProperty("OrigenMedidorH" + i.ToString()).SetValue(mA, tipoMedidor);
            }

            return listaFinal;
        }

        /// <summary>
        /// Obtiene el reporte historico por pagina
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public string ObtenerConsultaHistoricaPagInterconexion(int idPtomedicion, int medidor, DateTime fechaInicio, DateTime fechaFin, int pagina)
        {
            var paginas = FactorySic.GetMeMedicion96Repository().PaginacionInterconexiones(ConstantesInterconexiones.IdReporteInterconexion, fechaInicio, fechaFin);
            List<MeMedicion96DTO> list = new List<MeMedicion96DTO>();
            if (paginas.Count() > 0)
            {
                list = this.ObtenerDataHistoricaInterconexionByMedidor(idPtomedicion, medidor, paginas[pagina - 1], paginas[pagina - 1], false);
            }
            return ObtenerHtmlReporteInterconexion(idPtomedicion, list, medidor, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Genera Reporte Historico de datos de interconexion
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="fini"></param>
        /// <param name="ffin"></param>
        /// <returns></returns>
        public string ObtenerHtmlReporteInterconexion(int idInterconexion, List<MeMedicion96DTO> lista, int medidor, DateTime fini, DateTime ffin)
        {
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idInterconexion);
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th colspan='7'>LINEA DE TRANSMISIÓN " + interconexion.Interenlace + "</th>");

            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MWh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MWh</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MVARh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> " + interconexion.Internomlinea + " <br> MVARh</th>");
            strHtml.Append("<th>" + interconexion.Internomlinea + " kV</th>");
            strHtml.Append("<th>" + interconexion.Internomlinea + " Amp.</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;
            var ptos = GetByCriteriaMeReporptomeds(3);
            for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
            {
                var list = lista.Where(x => x.Medifecha == f).ToList();
                if (list.Count() != 0)
                {
                    for (int k = 1; k <= 96; k++)
                    {
                        minuto = minuto + 15;
                        for (int j = 0; j < ptos.Count(); j++)
                        {
                            if (j == 0)
                            {
                                strHtml.Append("<tr>");
                                strHtml.Append(string.Format("<td>{0}</td>", list[0].Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                            }

                            decimal? valor = null;
                            int origenMedidor = 0;
                            MeMedicion96DTO entity = list.Where(x => x.Ptomedicodi == ptos[j].Ptomedicodi && x.Tipoinfocodi ==
                                ptos[j].Tipoinfocodi).ToList().FirstOrDefault();
                            string claseCelda = string.Empty;
                            if (medidor == ConstantesInterconexiones.IdMedidorConsolidado)
                            {
                                origenMedidor = (int)entity.GetType().GetProperty("OrigenMedidorH" + k).GetValue(entity, null);
                                claseCelda = origenMedidor == ConstantesInterconexiones.IdMedidorPrincipal ? "medidor_principal" : "medidor_secundario";
                            }
                            if (entity != null)
                            {
                                valor = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (valor != null)
                                {
                                    strHtml.Append(string.Format("<td class='{1}'>{0}</td>", valor.Value.ToString("N", nfi), claseCelda));
                                }
                                else
                                {
                                    strHtml.Append("<td></td>");
                                }
                            }
                            else
                            {
                                strHtml.Append(string.Format("<td>--</td>"));
                            }
                            if (j == ptos.Count() - 1)
                            {
                                strHtml.Append("</tr>");
                            }
                        }
                    }
                }
            }

            if (lista.Count == 0)
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            if (lista.Count > 0 && medidor == ConstantesInterconexiones.IdMedidorConsolidado) 
            {
                strHtml.Append("<p>LEYENDA:</p>");
                strHtml.Append("<table border='0' style='width: 126px;'> <tbody>");
                strHtml.Append("<tr><td class='medidor_principal' style='width: 124px;border: 1px solid black;'>PRINCIPAL</td></tr>");
                strHtml.Append("<tr><td class='medidor_secundario' style='border: 1px solid black;'>SECUNDARIO</td></tr>");
                strHtml.Append("</tbody></table>");
            }

            return strHtml.ToString();
        }


        public string GetHtmlInterconexionXParametro(int idPtomedicion, int idParametro, DateTime fini, DateTime ffin)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<MeMedicion96DTO> lista = this.ObtenerInterconexionParametro(idPtomedicion, idParametro, fini, ffin);
            StringBuilder strHtml = new StringBuilder();
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idPtomedicion);

            strHtml.Append("<table border='1' class='pretty tabla-adicional' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");

            switch (idParametro)
            {
                case 1:
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> Exportación <br> MW</th>");
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> Importación <br> MW</th>");
                    break;
                case 2:
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> Exportación <br> MVAR</th>");
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> Importación <br> MVAR</th>");
                    break;
                case 3:
                    strHtml.Append("<th>" + interconexion.Internomlinea + "<br> Exportación <br> MWh</th>");
                    strHtml.Append("<th>" + interconexion.Internomlinea + "<br> Importación <br> MWh</th>");
                    break;
                case 4:
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> Exportación <br> MVARh</th>");
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> Importación <br> MVARh</th>");
                    break;
                case 5:
                    strHtml.Append("<th>" + interconexion.Internomlinea + " <br> kV</th>");
                    strHtml.Append("<th>" + interconexion.Internomlinea + "<br> Amp</th>");
                    break;
            }

            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;
            MeMedicion96DTO entity = new MeMedicion96DTO();
            for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
            {
                var list = lista.Where(x => x.Medifecha == f).ToList();
                minuto = 0;
                if (list.Count() != 0)
                    for (int k = 1; k <= 96; k++)
                    {
                        minuto = minuto + 15;

                        switch (idParametro)
                        {
                            case 1:
                            case 3:
                                ///Exportacion MWh
                                entity = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MWh).FirstOrDefault();
                                
                                strHtml.Append("<tr>");
                                strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                                decimal valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 1)
                                    valor = valor * 4;
                                string mwExport = valor.ToString("N", nfi);

                                ///Importacion MWh
                                entity = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Ptomedicodi == ConstantesInterconexiones.IdImportacionL2280MWh).FirstOrDefault();
                                
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 1)
                                    valor = valor * 4;
                                string mwImport = valor.ToString("N", nfi); ;

                                strHtml.Append(string.Format("<td>{0}</td>", mwExport));
                                strHtml.Append(string.Format("<td>{0}</td>", mwImport));
                                strHtml.Append("</tr>");
                                break;
                            case 2:
                            case 4:
                                //Exportacion MVARh
                                entity = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MVARr).FirstOrDefault();
                                
                                strHtml.Append("<tr>");
                                strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    valor = valor * 4;
                                string mvarExport = valor.ToString("N", nfi);

                                //Importacion MVARh
                                entity = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaReactiva && x.Ptomedicodi == ConstantesInterconexiones.IdImportacionL2280MVARr).FirstOrDefault();

                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    valor = valor * 4;
                                string mvarImport = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", mvarExport));
                                strHtml.Append(string.Format("<td>{0}</td>", mvarImport));
                                strHtml.Append("</tr>");
                                break;
                            case 5:
                                //kV
                                entity = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiKV).FirstOrDefault();                                
                                
                                strHtml.Append(string.Format("<td>{0}</td>", entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                string kVvalor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", kVvalor));

                                //A
                                entity = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiA).FirstOrDefault();
                                
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                string AmpValor = valor.ToString("N", nfi);
                                strHtml.Append(string.Format("<td>{0}</td>", AmpValor));
                                strHtml.Append("</tr>");
                                break;
                        }
                    }
            }

            if (lista.Count == 0)
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td><td></td><td></td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }




        public int ObtenerTotalHistoricoInterconexion(int ptomedicodi, DateTime fechaini, DateTime fechafin)
        {
            var paginas = FactorySic.GetMeMedicion96Repository().PaginacionInterconexiones(3, fechaini, fechafin);
            return paginas.Count();
        }

        /// <summary>
        /// Permite eliminar los valores previos cargados
        /// </summary>
        /// <param name="horizonte"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        public void EliminarValoresCargados(int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                FactorySic.GetMeMedicion96Repository().DeleteEnvioInterconexion(fechaInicio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos cargados
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarInterconexion(List<MeMedicion96DTO> entitys)
        {
            try
            {
                foreach (MeMedicion96DTO entity in entitys)
                {

                    entity.Lectcodi = 1;
                    FactorySic.GetMeMedicion96Repository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la consulta de los datos de Interconexion cargados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string ObtenerConsultaInterconexion(int idEmpresa, DateTime fechaInicio, DateTime fechaFin)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerEnvioInterconexion(idEmpresa, fechaInicio, fechaFin);

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<table border='1' class='pretty tabla-adicional cell-border' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr>");
            strHtml.Append("<th colspan='7'>LINEA DE TRANSMISIÓN L-2280 (ZORRITOS - MACHALA)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<th>FECHA HORA</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> L-2280 (ZORRITOS) <br> MWh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> L-2280 (ZORRITOS) <br> MWh</th>");
            strHtml.Append("<th>EXPORTACIÓN <br> L-2280 (ZORRITOS) <br> MVARh</th>");
            strHtml.Append("<th>IMPORTACIÓN <br> L-2280 (ZORRITOS) <br> MVARh</th>");
            strHtml.Append("<th>L-2280 (ZORRITOS) kV</th>");
            strHtml.Append("<th>L-2280 (ZORRITOS) Amp.</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int minuto = 0;

            if (list.Count > 0)
            {
                for (int k = 1; k <= 96; k++)
                {
                    minuto = minuto + 15;
                    strHtml.Append("<tr>");
                    strHtml.Append(string.Format("<td>{0}</td>", fechaInicio.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm")));
                    MeMedicion96DTO entity = list.Where(x => x.Tipoinfocodi == 3).ToList().FirstOrDefault();
                    decimal valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    string mwExport = "";
                    string mwImport = "";
                    if (valor > 0)
                    {
                        mwExport = valor.ToString("N", nfi);
                        mwImport = 0.ToString("N", nfi);
                    }
                    else
                    {
                        valor = valor * (-1);
                        mwImport = valor.ToString("N", nfi);
                        mwExport = 0.ToString("N", nfi);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", mwExport));
                    strHtml.Append(string.Format("<td>{0}</td>", mwImport));

                    string mvarExport = "";
                    string mvarImport = "";
                    entity = list.Where(x => x.Tipoinfocodi == 4).ToList().FirstOrDefault();
                    valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    if (valor > 0)
                    {
                        mvarExport = valor.ToString("N", nfi);
                        mvarImport = 0.ToString("N", nfi);
                    }
                    else
                    {
                        valor = valor * (-1);
                        mvarImport = valor.ToString("N", nfi);
                        mvarExport = 0.ToString("N", nfi);
                    }
                    strHtml.Append(string.Format("<td>{0}</td>", mvarExport));
                    strHtml.Append(string.Format("<td>{0}</td>", mvarImport));

                    entity = list.Where(x => x.Tipoinfocodi == 5).ToList().FirstOrDefault();
                    valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    string kVvalor = valor.ToString("N", nfi);
                    strHtml.Append(string.Format("<td>{0}</td>", kVvalor));

                    entity = list.Where(x => x.Tipoinfocodi == 9).ToList().FirstOrDefault();
                    valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                    string AmpValor = valor.ToString("N", nfi);
                    strHtml.Append(string.Format("<td>{0}</td>", AmpValor));
                    strHtml.Append("</tr>");
                }

            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        public List<MeMedicion96DTO> ObtenerHistoricoMedicion(int idPtomedicion, DateTime fini, DateTime ffin)
        {
            return FactorySic.GetMeMedicion96Repository().ObtenerHistoricoInterconexion(idPtomedicion, fini, ffin);
        }

        public List<MeMedicion96DTO> ObtenerResumenInterconexion(List<MeMedicion96DTO> lista, DateTime fini, DateTime ffin, int idParametro)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            List<MeMedicion96DTO> ListaContenido = new List<MeMedicion96DTO>();

            int minuto = 0;
            MeMedicion96DTO entity;
            for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
            {
                var list = lista.Where(x => x.Medifecha == f);
                minuto = 0;
                if (list.Count() != 0)
                {
                    for (int k = 1; k <= 96; k++)
                    {
                        MeMedicion96DTO katsu = new MeMedicion96DTO();
                        minuto = minuto + 15;

                        if (idParametro == 1 || idParametro == 3)
                        {
                            decimal valor = 0;
                            //----->Inicio - Exportacion
                            entity = list.Where(x => x.Tipoinfocodi == 3 && x.Ptomedicodi ==
                                ConstantesInterconexiones.IdExportacionL2280MWh).ToList().FirstOrDefault();

                            if (entity != null)
                            {
                                katsu.FechaFila = entity.Medifecha.Value.AddMinutes(minuto);
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 1)
                                    katsu.TotalEnergiaExportada = valor * 4;
                                else
                                    katsu.TotalEnergiaExportada = valor;
                            }
                            //<-----Fin - Exportacion
                            //----->Inicio - Importacion
                            entity = list.Where(x => x.Tipoinfocodi == 3 && x.Ptomedicodi ==
                                ConstantesInterconexiones.IdImportacionL2280MWh).ToList().FirstOrDefault();
                            if (entity != null)
                            {
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 1)
                                    katsu.TotalEnergiaImportada = valor * 4;
                                else
                                    katsu.TotalEnergiaImportada = valor;
                            }
                            //<-----Fin - Importacion
                        }
                        if (idParametro == 2 || idParametro == 4)
                        {
                            decimal valor = 0;
                            //----->Inicio - Exportacion
                            entity = list.Where(x => x.Tipoinfocodi == 4 && x.Ptomedicodi ==
                                ConstantesInterconexiones.IdExportacionL2280MVARr).ToList().FirstOrDefault();

                            if (entity != null)
                            {
                                katsu.FechaFila = entity.Medifecha.Value.AddMinutes(minuto);
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    katsu.TotalEnergiaExportada = valor * 4;
                            }
                            //<-----Fin - Exportacion
                            //----->Inicio - Importacion
                            entity = list.Where(x => x.Tipoinfocodi == 4 && x.Ptomedicodi ==
                                ConstantesInterconexiones.IdImportacionL2280MVARr).ToList().FirstOrDefault();
                            if (entity != null)
                            {
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    katsu.TotalEnergiaImportada = valor * 4;
                            }
                            //<-----Fin - Importacion
                        }
                        if (idParametro == 5)
                        {
                            decimal valor = 0;
                            //----->Inicio - Exportacion
                            entity = list.Where(x => x.Tipoinfocodi == 5).ToList().FirstOrDefault();
                            if (entity != null)
                            {
                                katsu.FechaFila = entity.Medifecha.Value.AddMinutes(minuto);
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    katsu.TotalEnergiaExportada = valor * 4;
                            }
                            //<-----Fin - Exportacion
                            //----->Inicio - Importacion
                            entity = list.Where(x => x.Tipoinfocodi == 9).ToList().FirstOrDefault();
                            if (entity != null)
                            {
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (idParametro == 2)
                                    katsu.TotalEnergiaImportada = valor * 4;
                            }
                            //<-----Fin - Importacion
                        }
                        ListaContenido.Add(katsu);
                    }
                }
            }
            return ListaContenido;
        }

        #endregion

        #region Metodos Tabla ME_MEDICION48

        /// <summary>
        /// Obtiene la potencia disponible en CT Tumbes para los datos de excedentees de exportacion
        /// </summary>
        /// <param name="listaManto"></param>
        /// <param name="listaCTTumbes"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal ObtenerPotDisponibleCTTumbes(List<EveManttoDTO> listaManto, DateTime fecha,
            decimal potMK1, decimal potMK2)
        {
            decimal valor = 0;
            bool disponibleMK1 = true;
            bool disponibleMK2 = true;
            bool disponibleCTTumbes = true;
            //verifica Central

            foreach (var reg in listaManto)
            {
                if ((reg.Evenfin > fecha) && (reg.Evenini < fecha.AddMinutes(30)))
                    switch (reg.Equicodi)
                    {
                        case 1068: //Grupo 1
                            disponibleMK1 = false;
                            break;
                        case 1069://Grupo 2
                            disponibleMK2 = false;
                            break;
                        case 1498://Grupo 3
                            disponibleCTTumbes = false;
                            break;
                    }
            }
            if (disponibleCTTumbes)
            {
                if (disponibleMK1)
                    valor += potMK1;
                if (disponibleMK2)
                    valor += potMK2;
            }

            return valor;
        }

        public List<MeMedicion48DTO> ListaMed48Interconexiones(int tipoCapExc, int lectocodi, int origlect, string ptomedicodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            switch (lectocodi)
            {
                case ConstantesInterconexiones.IdLectCPDiaPre:
                case ConstantesInterconexiones.IdLectCPDiaFin:
                    lista48 = FactorySic.GetMeMedicion48Repository().GetInterconexiones(lectocodi, origlect, ptomedicodi,
                fechaInicio, fechaFin);

                    break;
                case ConstantesInterconexiones.IdLectCPSemPre:
                case ConstantesInterconexiones.IdLectCPSemFin:
                    lista24 = FactorySic.GetMeMedicion24Repository().GetInterconexiones(lectocodi, origlect, ptomedicodi,
                fechaInicio, fechaFin);
                    lista48 = DuplicarLista24(lista24);
                    break;
            }

            if (tipoCapExc == ConstantesInterconexiones.Excedente) //Excedente
            {
                /////////////////////////// Lee parametros de equipamiento /////////////////
                var listProp = FactorySic.GetPrGrupoEquipoValRepository().GetValorPropiedadDetalle(269, 14);
                string stpotInstMK1 = listProp.Find(x => x.Equicodi == ConstantesInterconexiones.IdMK1).Greqvaformuladat;//ConstantesInterconexiones.PotIntMK1.ToString();//GetValorPropiedad(1466, 1068);
                string stpotInstMK2 = listProp.Find(x => x.Equicodi == ConstantesInterconexiones.IdMK1).Greqvaformuladat; //ConstantesInterconexiones.PotIntMK2.ToString();//GetValorPropiedad(1466, 1069);
                string stcapL2249 = GetValorPropiedad(ConstantesInterconexiones.IdPropCapL2249, ConstantesInterconexiones.IdL2249);
                string stfkL2249 = GetValorPropiedad(ConstantesInterconexiones.IdPropFacCapL2249, ConstantesInterconexiones.IdL2249);
                decimal dpotIntMK1 = 0;
                decimal dpotpIntMK2 = 0;
                decimal dcapL229 = 0;
                decimal dfkL2249 = 0;
                if (!decimal.TryParse(stpotInstMK1, out dpotIntMK1))
                    dpotIntMK1 = 0;
                if (!decimal.TryParse(stpotInstMK2, out dpotpIntMK2))
                    dpotpIntMK2 = 0;
                if (!decimal.TryParse(stcapL2249, out dcapL229))
                    dcapL229 = 0;
                if (!decimal.TryParse(stfkL2249, out dfkL2249))
                    dfkL2249 = 0;
                decimal capL2249 = dcapL229 * dfkL2249;
                /////////////////////////////////////////////////////////////////////////
                string IDsTumbes = ConstantesInterconexiones.IdCtTumbes.ToString() + "," + ConstantesInterconexiones.IdMK1.ToString() +
                    "," + ConstantesInterconexiones.IdMK2.ToString();
                var listaManto = ObtenerManttoEquipo(IDsTumbes, fechaInicio, fechaFin);
                foreach (var reg in lista48)
                {
                    for (var i = 1; i <= 48; i++)
                    {
                        //Verificar Disponibilidad  (listaManto,dpotIntMK1,dpotpIntMK2,fecha)
                        var porDiponibleCTTumbes = ObtenerPotDisponibleCTTumbes(listaManto, reg.Medifecha.AddMinutes((i - 1) * 30), dpotIntMK1, dpotpIntMK2);
                        decimal? valor = (decimal?)reg.GetType().GetProperty("H" + i).GetValue(reg, null);
                        if (valor != null)
                        {
                            //Verificar FS de los grupos de CT Tumbes

                            valor = capL2249 - valor + porDiponibleCTTumbes;
                            // Cap L-2249 - Dem Zorritos + Generacion Tumbes.
                        }
                        else
                        {
                            valor = 0;
                        }

                        reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                    }
                }
            }
            return lista48;
        }

        public string ObtenerReporteExcedente(int idVersion, int idHorizonte, int Origlectcodi, string ptomedicodi, DateTime fechaIni, DateTime fechaFin)
        {
            string strHtml = string.Empty;
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            lista48 = ListaMed48Interconexiones(ConstantesInterconexiones.Excedente, idHorizonte, Origlectcodi, ptomedicodi, fechaIni, fechaFin);
            strHtml = GeneraViewExcedentes(lista48);
            return strHtml;
        }

        /// <summary>
        /// Genera la Vista de la tabla de Capacidad de Importacion
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="Origlectcodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public string GeneraVistaCapacImport(int idVersion, int idHorizonte, int Origlectcodi, string ptomedicodi, DateTime fechaIni, DateTime fechaFin)
        {

            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            DateTime fechaInicio = DateTime.MinValue;
            List<MeMedicion48DTO> lista = ListaMed48Interconexiones(0, idHorizonte, Origlectcodi, ptomedicodi, fechaIni, fechaFin);
            var idsPto = lista.GroupBy(x => x.Ptomedicodi).Select(group => group.First()).ToList();
            //var listFecha = lista.GroupBy(x => x.Medifecha).Select(group => group.First()).ToList();
            var listFecha = lista.Select(x => x.Medifecha).Distinct().ToList();

            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-adicional ' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");


            strHtml.Append("<tr><th style='width:160px;'>FECHA/HORA</th>");
            string[][] matriz;

            foreach (var reg in idsPto)
            {
                strHtml.Append("<th style='background-color:#87CEFA;border:1px solid #9AC9E9;text-align: center' >" + "<div >" + reg.Equinomb + "</div>" + "</th>");
            }
            strHtml.Append("<th style='width:160px;'>TOTAL (MW)</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int resolucion = 30;
            int nBloques = 48;

            matriz = new string[listFecha.Count() * nBloques][];
            for (int i = 0; i < listFecha.Count(); i++)
            {
                fechaInicio = listFecha[i];
                for (int j = 0; j < idsPto.Count(); j++)
                {
                    var find = lista.Find(x => x.Medifecha == fechaInicio && x.Ptomedicodi == idsPto[j].Ptomedicodi);
                    for (int k = 1; k <= nBloques; k++)
                    {
                        if (j == 0)
                        {
                            matriz[(i * nBloques) + k - 1] = new string[idsPto.Count() + 1];
                            string fechaMin = ((fechaInicio.AddMinutes(k * resolucion))).ToString(ConstantesBase.FormatoFechaHora);
                            matriz[(i * nBloques) + k - 1][0] = fechaMin;
                        }
                        if (find != null)
                        {
                            decimal? valor;
                            valor = (decimal?)find.GetType().GetProperty("H" + k).GetValue(find, null);
                            if (valor != null)
                            {
                                matriz[(i * nBloques) + k - 1][j + 1] = ((decimal)valor).ToString();
                            }
                            else
                            {
                                matriz[(i * nBloques) + k - 1][j + 1] = "--";
                            }
                        }
                        else
                        {
                            matriz[(i * nBloques) + k - 1][j + 1] = "--";
                        }
                    }
                }

            }

            for (int i = 0; i < listFecha.Count() * nBloques; i++)
            {
                strHtml.Append("<tr>");
                decimal total = 0;
                for (int j = 0; j < idsPto.Count() + 1; j++)
                {
                    if ((j == 0) || (matriz[i][j] == "--"))
                        strHtml.Append(string.Format("<td style='text-align: center;'>{0}</td>", matriz[i][j]));
                    else
                    {
                        var valor = decimal.Parse(matriz[i][j]);
                        strHtml.Append(string.Format("<td style='text-align: center;'>{0}</td>", valor.ToString("N", nfi)));
                        total += valor;
                    }
                }
                strHtml.Append(string.Format("<td style='text-align: center;'>{0}</td></tr>", total.ToString("N", nfi)));
            }

            if (lista.Count == 0)
            {
                strHtml.Append("<tr><td  style='text-align:center'>No existen registros.</td><td></td></tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            return strHtml.ToString();

        }

        /// <summary>
        /// imprime view para la tabla medicion48
        /// </summary>
        /// <param name="listaGenerica"></param>
        /// <param name="listaCabeceraM24"></param>
        /// <param name="nroLectura"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public string GeneraViewExcedentes(List<MeMedicion48DTO> lista)
        {
            NumberFormatInfo nfi = GenerarNumberFormatInfo();
            StringBuilder strHtml = new StringBuilder();
            DateTime fechaInicio = DateTime.MinValue;
            strHtml.Append("<table border='1' style='width:auto' class='pretty tabla-adicional ' cellspacing='0' width='100%' id='tabla'>");
            strHtml.Append("<thead>");
            strHtml.Append("<tr><th style='width:160px;'>FECHA/HORA</th>");
            strHtml.Append("<th style='background-color:#87CEFA;border:1px solid #9AC9E9;text-align: center' >" + "<div >SE ZORRITOS (MW)</div>" + "</th>");
            strHtml.Append("</tr>");
            strHtml.Append("</thead>");
            strHtml.Append("<tbody>");

            int resolucion = 30;
            int nBloques = 48;
            if (lista.Count > 0)
            {
                foreach (var p in lista)
                {
                    fechaInicio = p.Medifecha;
                    for (int k = 1; k <= nBloques; k++)
                    {
                        var fechaMin = ((fechaInicio.AddMinutes(k * resolucion))).ToString(ConstantesBase.FormatoFechaHora);
                        strHtml.Append("<tr>");
                        strHtml.Append(string.Format("<td>{0}</td>", fechaMin));
                        decimal? valor;
                        valor = (decimal?)p.GetType().GetProperty("H" + k).GetValue(p, null);
                        if (valor != null)
                            strHtml.Append(string.Format("<td style='text-align: center;'>{0}</td>", ((decimal)valor).ToString("N", nfi)));
                        else
                            strHtml.Append(string.Format("<td>--</td>"));
                        strHtml.Append("</tr>");
                    }
                }

            }
            else
            {
                strHtml.Append("<td  style='text-align:center'>No existen registros.</td><td></td>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");


            return strHtml.ToString();
        }

        #endregion

        #region Métodos Tabla ME_ENVIO

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ENVIO
        /// </summary>
        public void UpdateMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIO
        /// </summary>
        public MeEnvioDTO GetByIdMeEnvio(int idEnvio)
        {
            return FactorySic.GetMeEnvioRepository().GetById(idEnvio);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_ENVIO
        /// </summary>
        public List<MeEnvioDTO> ListMeEnvios()
        {
            return FactorySic.GetMeEnvioRepository().List();
        }
        /// <summary>
        /// Lista de envios por pagina
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin, nroPaginas, pageSize);
        }

        /// <summary>
        /// Lista de envios envios sin paginacion
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> GetListaMultipleMeEnviosXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().GetListaMultipleXLS(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        /// <summary>
        /// Total de envios
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsEstado"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int TotalListaMultipleMeEnvios(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsLectura)) idsLectura = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsFormato)) idsFormato = ConstantesAppServicio.ParametroDefecto;
            if (string.IsNullOrEmpty(idsEstado)) idsEstado = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetMeEnvioRepository().TotalListaMultiple(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin);
        }

        #endregion

        #region Métodos Tabla ME_MEDIDOR

        /// <summary>
        /// Inserta un registro de la tabla ME_MEDIDOR
        /// </summary>
        public void SaveMeMedidor(MeMedidorDTO entity)
        {
            try
            {
                FactorySic.GetMeMedidorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_MEDIDOR
        /// </summary>
        public void UpdateMeMedidor(MeMedidorDTO entity)
        {
            try
            {
                FactorySic.GetMeMedidorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_MEDIDOR
        /// </summary>
        public void DeleteMeMedidor(int medicodi)
        {
            try
            {
                FactorySic.GetMeMedidorRepository().Delete(medicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_MEDIDOR
        /// </summary>
        public MeMedidorDTO GetByIdMeMedidor(int medicodi)
        {
            return FactorySic.GetMeMedidorRepository().GetById(medicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_MEDIDOR
        /// </summary>
        public List<MeMedidorDTO> ListMeMedidors()
        {
            return FactorySic.GetMeMedidorRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeMedidor
        /// </summary>
        public List<MeMedidorDTO> GetByCriteriaMeMedidors()
        {
            return FactorySic.GetMeMedidorRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ME_ARCHIVO

        /// <summary>
        /// Inserta un registro de la tabla ME_ARCHIVO
        /// </summary>
        public int SaveMeArchivo(MeArchivoDTO entity)
        {
            try
            {
                return FactorySic.GetMeArchivoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_ARCHIVO
        /// </summary>
        public void UpdateMeArchivo(MeArchivoDTO entity)
        {
            try
            {
                FactorySic.GetMeArchivoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Tabla ME_PERIODOMEDIDOR

        /// <summary>
        /// Inserta un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public void SaveMePeriodomedidor(MePeriodomedidorDTO entity)
        {
            try
            {
                FactorySic.GetMePeriodomedidorRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Grabba una lista de periodos
        /// </summary>
        /// <param name="entitys"></param>
        public void SaveListaMePeriodomedidor(List<MePeriodomedidorDTO> entitys)
        {
            foreach (var reg in entitys)
            {
                SaveMePeriodomedidor(reg);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public void UpdateMePeriodomedidor(MePeriodomedidorDTO entity)
        {
            try
            {
                FactorySic.GetMePeriodomedidorRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_PERIODOMEDIDOR
        /// </summary>
        public void DeleteMePeriodomedidor()
        {
            try
            {
                FactorySic.GetMePeriodomedidorRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MePeriodomedidor
        /// </summary>
        public List<MePeriodomedidorDTO> GetByCriteriaMePeriodomedidors(int idEnvio)
        {
            return FactorySic.GetMePeriodomedidorRepository().GetByCriteria(idEnvio);
        }

        #endregion

        #region Métodos Tabla ME_AMPLIACIONFECHA

        /// <summary>
        /// Inserta un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void SaveMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public void UpdateMeAmpliacionfecha(MeAmpliacionfechaDTO entity)
        {
            try
            {
                FactorySic.GetMeAmpliacionfechaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
               
        /// <summary>
        /// Permite obtener un registro de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public MeAmpliacionfechaDTO GetByIdMeAmpliacionfecha(DateTime fecha, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetById(fecha, empresa, formato);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_AMPLIACIONFECHA
        /// </summary>
        public List<MeAmpliacionfechaDTO> ListMeAmpliacionfechas()
        {
            return FactorySic.GetMeAmpliacionfechaRepository().List();
        }

        public List<MeAmpliacionfechaDTO> ObtenerListaMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, int empresa, int formato)
        {
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaAmpliacion(fechaIni, fechaFin, empresa, formato);
        }

        public List<MeAmpliacionfechaDTO> ObtenerListaMultipleMeAmpliacionfechas(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string sFormato)
        {
            if (string.IsNullOrEmpty(sEmpresa)) sEmpresa = ConstantesAppServicio.ParametroNulo;
            if (string.IsNullOrEmpty(sFormato)) sFormato = ConstantesAppServicio.ParametroNulo;
            return FactorySic.GetMeAmpliacionfechaRepository().GetListaMultiple(fechaIni, fechaFin, sEmpresa, sFormato);
        }

        #endregion

        #region Métodos Tabla ME_REPORPTOMED

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeReporptomed
        /// </summary>
        public List<MeReporptomedDTO> GetByCriteriaMeReporptomeds(int reporcodi)
        {
            return FactorySic.GetMeReporptomedRepository().GetByCriteria(reporcodi, -1).OrderBy(x => x.Repptoorden).ToList();
        }

        #endregion

        #region ExtArchivo
        /// <summary>
        /// Devuelve Lista de archivos paginados
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <returns></returns>
        public List<ExtArchivoDTO> GetListaEnvioPagInterconexion(int empresa, int estado, DateTime fechaInicial, DateTime fechaFinal, int nroPagina, int nroFilas)
        {
            return FactorySic.GetExtArchivoRepository().ListaEnvioPagInterconexion(empresa, estado, fechaInicial, fechaFinal, nroPagina, nroFilas);
        }

        /// <summary>
        /// Devuelve TOtal de registros de lista de archuivo
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public int TotalEnvio(DateTime fechaini, DateTime fechafin, int empresa)
        {
            return FactorySic.GetExtArchivoRepository().TotalEnvioInterconexion(fechaini, fechafin, empresa);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla ExtArchivo
        /// </summary>
        public List<ExtArchivoDTO> GetByCriteriaExtArchivos(int empresa, int estado, DateTime fechaInicial, DateTime fechaFinal)
        {
            return FactorySic.GetExtArchivoRepository().GetByCriteria(empresa, estado, fechaInicial, fechaFinal);
        }

        #endregion

        #region Métodos adicionales
        //***************************** METODOS ADICIONALES ***********************************************************
        /// <summary>
        /// Formato de presentacion para numeros decimales, separador de miles:" " y cantidad de decimales:3
        /// </summary>
        /// <returns></returns>
        public NumberFormatInfo GenerarNumberFormatInfo()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = " ";
            nfi.NumberDecimalDigits = 3;
            nfi.NumberDecimalSeparator = ",";
            return nfi;
        }

        /// <summary>
        /// Genera Lista de 48 datos a partir de una lista de 24
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> DuplicarLista24(List<MeMedicion24DTO> lista)
        {
            List<MeMedicion48DTO> lista48 = new List<MeMedicion48DTO>();
            int indice = 1;
            decimal? valor;
            MeMedicion48DTO registro;
            foreach (var p in lista)
            {
                registro = new MeMedicion48DTO();
                registro.Medifecha = p.Medifecha;
                registro.Ptomedicodi = p.Ptomedicodi;
                registro.Lectcodi = p.Lectcodi;
                registro.Equinomb = p.Equinomb;
                registro.Equicodi = p.Equicodi;
                for (int j = 1; j <= 24; j++)
                {

                    valor = (decimal?)p.GetType().GetProperty("H" + j.ToString()).GetValue(p, null);
                    indice = (j - 1) * 2 + 1;
                    registro.GetType().GetProperty("H" + indice.ToString()).SetValue(registro, valor);
                    indice++;
                    registro.GetType().GetProperty("H" + indice.ToString()).SetValue(registro, valor);
                }
                lista48.Add(registro);
            }
            return lista48;

        }

        /// <summary>
        /// Permite exportar historico de intercambios internacionales
        /// </summary>
        /// <param name="list"></param>
        public void GenerarArchivoReporte(int idInterconexion, List<MeMedicion96DTO> lista, int medidor, DateTime fini, DateTime ffin, string rutaPlantilla,
            string rutaNombreArchivo)
        {
            FileInfo template = new FileInfo(rutaPlantilla);
            FileInfo newFile = new FileInfo(rutaNombreArchivo);
            InInterconexionDTO interconexion = FactorySic.GetInInterconexionRepository().GetById(idInterconexion);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaNombreArchivo);
            }
            int row = 8;
            int column = 2;
            int minuto = 0;
            string nombreMedidor = "Medidor " + this.GetListaMedidorApp().Where(x => x.Medicodi == medidor).Select(x => x.Medinombre).First();

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["REPORTE"];
                ws.Name = "Reporte " + nombreMedidor;
                ws.View.FreezePanes(row, 1);
                ws.Cells[2, 3].Value = fini.ToString("dd/MM/yyyy");
                ws.Cells[2, 5].Value = ffin.ToString("dd/MM/yyyy");
                ws.Cells[1, 3].Value = "Consulta Interconexión " + interconexion.Interdecrip + " - " + nombreMedidor;
                ws.Cells[4, 2].Value = "LINEA DE TRANSMISIÓN " + interconexion.Interenlace;
                ws.Cells[6, 3].Value = interconexion.Internomlinea;
                ws.Cells[6, 4].Value = interconexion.Internomlinea;
                ws.Cells[6, 5].Value = interconexion.Internomlinea;
                ws.Cells[6, 6].Value = interconexion.Internomlinea;
                ws.Cells[6, 7].Value = interconexion.Internomlinea;
                ws.Cells[6, 8].Value = interconexion.Internomlinea;


                if (lista.Count > 0)
                {
                    var ptos = GetByCriteriaMeReporptomeds(3);
                    for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
                    {
                        var list = lista.Where(x => x.Medifecha == f).ToList();
                        if (list.Count() != 0)
                        {
                            minuto = 0;
                            for (int k = 1; k <= 96; k++)
                            {
                                minuto = minuto + 15;
                                for (int j = 0; j < ptos.Count(); j++)
                                {
                                    if (j == 0)
                                    {
                                        ws.Cells[row, column].Value = list[0].Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm");
                                        ws.Cells[row, column].StyleID = ws.Cells[8, column].StyleID;
                                    }

                                    int origenMedidor = 0;
                                    decimal? valor = null;
                                    MeMedicion96DTO entity = list.Where(x => x.Ptomedicodi == ptos[j].Ptomedicodi && x.Tipoinfocodi ==
                                        ptos[j].Tipoinfocodi).ToList().FirstOrDefault();
                                    string claseCelda = string.Empty;
                                    if (medidor == ConstantesInterconexiones.IdMedidorConsolidado)
                                    {
                                        origenMedidor = (int)entity.GetType().GetProperty("OrigenMedidorH" + k).GetValue(entity, null);
                                        claseCelda = origenMedidor == ConstantesInterconexiones.IdMedidorPrincipal ? "#fff" : "#B8E2FB";
                                    }

                                    if (entity != null)
                                    {
                                        valor = (decimal?)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                        ws.Cells[row, column + j + 1].Value = valor;
                                        ws.Cells[row, column + j + 1].StyleID = ws.Cells[8, column + j + 1].StyleID;
                                        if (origenMedidor != 0)
                                        {
                                            ws.Cells[row, column + j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            ws.Cells[row, column + j + 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(claseCelda));
                                        }
                                    }
                                }
                                row++;
                            }
                        }
                    }

                    row += 2;
                    //Leyenda
                    if (medidor == ConstantesInterconexiones.IdMedidorConsolidado)
                    {
                        ws.Cells[row, column].Value = "LEYENDA:";
                        row += 2;

                        ws.Cells[row, column].Value = "PRINCIPAL";
                        ws.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        row++;

                        ws.Cells[row, column].Style.Numberformat.Format = "@";
                        ws.Cells[row, column].Value = "SECUNDARIO";
                        ws.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[row, column].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#B8E2FB"));
                        ws.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                }
                xlPackage.Save();
            }

        }

        /// <summary>
        /// Permite publicar los archivos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int PublicarPortalWeb(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                int medidor = ConstantesInterconexiones.IdMedidorConsolidado;
                List<InInterconexionDTO> interconexiones = this.ListInInterconexions();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
                string pathPortalBase = ConfigurationManager.AppSettings[ConstantesInterconexiones.PathPortalWeb];
                string pathInterconexiones = ConstantesInterconexiones.RutaPortalInterconexion;
                string plantilla = ruta + ConstantesInterconexiones.PlantillaReporteHistorico;
                string fileName = ruta + ConstantesInterconexiones.NombreReporteHistorico;

                foreach (InInterconexionDTO entity in interconexiones)
                {
                    for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        List<MeMedicion96DTO> list = this.ObtenerHistoricoInterconexion(entity.Intercodi, medidor, fecha, fecha);
                        if (list.Count > 0)
                        {
                            this.GenerarArchivoReporte(entity.Intercodi, list, medidor, fecha, fecha, plantilla, fileName);

                            var listaExportacion = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault();
                            var listaImportacion = list.Where(x => x.Tipoinfocodi == ConstantesInterconexiones.IdTipoInfocodiEnergiaActiva && x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault();

                            bool tieneExportacion = this.ContieneData(listaExportacion);
                            bool tieneImportacion = this.ContieneData(listaImportacion);

                            string filePortalWeb = string.Empty;
                            if (tieneExportacion && !tieneImportacion)
                            {
                                filePortalWeb = "Medidas_{0}_{1}_{2}_Exportación.xlsx";
                            }

                            if (!tieneExportacion && tieneImportacion)
                            {
                                filePortalWeb = "Medidas_{0}_{1}_{2}_Importación.xlsx";
                            }

                            if (tieneExportacion && tieneImportacion)
                            {
                                filePortalWeb = "Medidas_{0}_{1}_{2}_Importación_Exportación.xlsx";
                            }

                            if (!string.IsNullOrEmpty(filePortalWeb))
                            {
                                filePortalWeb = string.Format(filePortalWeb, entity.Interdecrip, entity.Internomlinea, fecha.ToString("dd_MM_yyyy"));

                                string folder = fecha.Year.ToString() + "/" + fecha.Month.ToString().PadLeft(2, '0') + "_" +
                                    COES.Base.Tools.Util.ObtenerNombreMes(fecha.Month) + "/";

                                if (!FileServer.VerificarExistenciaDirectorio(pathInterconexiones + folder, pathPortalBase))
                                {
                                    FileServer.CreateFolder(pathInterconexiones, folder, pathPortalBase);
                                }

                                if (FileServer.VerificarExistenciaFile(pathInterconexiones + folder, filePortalWeb, pathPortalBase))
                                {
                                    FileServer.DeleteBlob(pathInterconexiones + folder + filePortalWeb, pathPortalBase);
                                }
                                FileServer.UploadFromFileDirectory(fileName, pathInterconexiones + folder, filePortalWeb, pathPortalBase);
                            }
                        }
                    }
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite analizar si hay data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool ContieneData(MeMedicion96DTO item)
        {
            if (item != null)
            {
                for (int i = 1; i <= 96; i++)
                {
                    object val = item.GetType().GetProperty("H" + i).GetValue(item);
                    if (val != null)
                    {
                        decimal valor = (decimal)val;
                        if (valor != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //genera archivo de grafico flujo de potencia
        public void GenerarArchivoGrafFlujoPotencia(GraficoWeb grafico, string fechaInicio, string fechaFin, int idParametro,
            string rutaArchivo, string rutaLogo)
        {
            var listSeries = grafico.Series; //Lista de nombres de las series del grafico
            var listaData = grafico.SerieDataS;
            //string ruta = ConfigurationManager.AppSettings[ConstantesInterconexiones.ReporteInterconexiones].ToString();
            FileInfo newFile = new FileInfo(rutaArchivo);//)ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones3);
            int nfil = listaData[0].Length;
            int ncol = listSeries.Count;
            int row = 7;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);//ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones3);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("GRAFICO");
                ws = xlPackage.Workbook.Worksheets["GRAFICO"];

                string titulo = grafico.TitleText;

                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaInicio, fechaFin, rutaLogo);
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 20;
                ws.Cells[row - 1, 2].Value = "FECHA";

                for (int i = 0; i < ncol; i++)
                {
                    ws.Cells[row - 1, i + 3].Value = listSeries[i].Name;
                }
                for (int i = 0; i < nfil; i++)
                {
                    ws.Cells[row + i, 2].Value = listaData[0][i].X.ToString(ConstantesAppServicio.FormatoFechaHora);
                }

                //Borde cabecera de Tabla Listado
                var border = ws.Cells[row - 1, 2, row - 1, ncol + 2].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, ncol + 2])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    r.Style.WrapText = true;
                    r.Style.Font.Size = 8;
                    r.AutoFitColumns();
                }

                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                for (int i = 0; i < 2; i++) //inserta los datos 
                    for (int j = 0; j < listaData[i].Length; j++)
                    {
                        ws.Cells[j + row, i + 3].Value = listaData[i][j].Y;
                    }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + 0, 3, row + nfil - 1, 4])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
                string xAxisTitle = grafico.XAxisTitle;
                string yAxisTitle = grafico.YAxixTitle[0];

                // Genera Grafico
                ExcelChart chart;
                ExcelChart chart2;

                if (idParametro == 5)
                {
                    chart = ws.Drawings.AddChart("chartFlujoPot", eChartType.Line);
                    chart2 = chart.PlotArea.ChartTypes.Add(eChartType.Line);
                }
                else
                {
                    chart = ws.Drawings.AddChart("chartFlujoPot", eChartType.Area);
                    chart2 = chart.PlotArea.ChartTypes.Add(eChartType.Area);
                }
                chart.YAxis.Title.Font.Color = Color.SteelBlue;
                chart.YAxis.Font.Color = Color.SteelBlue;
                chart.YAxis.Title.Text = grafico.Series[0].YAxisTitle;
                chart2.UseSecondaryAxis = true;
                chart2.YAxis.Deleted = false;
                chart2.YAxis.TickLabelPosition = eTickLabelPosition.High;
                chart2.YAxis.Title.Text = grafico.Series[1].YAxisTitle;
                chart2.YAxis.Title.Font.Color = Color.Red;
                chart2.YAxis.Font.Color = Color.Red;
                chart.SetSize(1200, 700);
                chart2.SetPosition(5, 0, ncol + 3, 0);
                chart2.SetSize(1200, 700);
                for (int i = 0; i < ncol; i++)
                {

                    var ran1 = ws.Cells[row, 3 + i, row + nfil - 1, 3 + i];
                    var ran2 = ws.Cells[row, 2, row + nfil - 1, 2];
                    if (i == 0)
                    {
                        chart.Series.Add(ran1, ran2);
                        chart.Series[0].Header = ws.Cells[6, 3 + i].Value.ToString();
                    }
                    else
                    {
                        chart2.Series.Add(ran1, ran2);
                        chart2.Series[0].Header = ws.Cells[6, 3 + i].Value.ToString();

                    }
                }
                chart.Legend.Position = eLegendPosition.Bottom;
                //chart.Legend.Add();
                chart.Title.Text = titulo;
                //chart.XAxis.Title.Text = xAxisTitle;
                xlPackage.Save();
            }
        }

        //genera archivo de grafico intercambio de electricidad
        public void GenerarArchivoGrafInterElect(GraficoWeb grafico, string fechaInicio, string fechaFin,
            string rutaArchivo, string rutaLogo, List<EstructuraEvolucionEnergia> list)
        {
            var listSeries = grafico.Series;
            List<String> listCategoriaGrafico = grafico.XAxisCategories; // Lista de DIAS ordenados para la categoria del grafico
            List<String> listSerieName = grafico.SeriesName; //Lista de nombres de las series del grafico
            decimal?[][] listSerieData = grafico.SeriesData; // lista de valores para las series del grafico
            FileInfo newFile = new FileInfo(rutaArchivo);
            int nfil = listCategoriaGrafico.Count;
            int ncol = listSeries.Count;
            int row = 7;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets.Add("GRAFICO");
                ws = xlPackage.Workbook.Worksheets["GRAFICO"];

                string titulo = grafico.TitleText;

                ConfiguraEncabezadoHojaExcel(ws, titulo, fechaInicio, fechaFin, rutaLogo);

                ws.Cells[row - 1, 2].Value = "FECHA";
                ws.Cells[row - 1, 3].Value = "INICIO";
                ws.Cells[row - 1, 4].Value = "FIN";

                for (int i = 0; i < ncol; i++)
                {
                    ws.Cells[row - 1, i + 5].Value = listSeries[i].Name;
                }
                for (int i = 0; i < nfil; i++)
                {
                    ws.Cells[row + i, 2].Value = listCategoriaGrafico[i];
                }

                //Borde cabecera de Tabla Listado
                var border = ws.Cells[row - 1, 2, row - 1, ncol + 4].Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                using (ExcelRange r = ws.Cells[row - 1, 2, row - 1, ncol + 4])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    r.Style.WrapText = true;
                    r.Style.Font.Size = 8;
                    r.AutoFitColumns();

                }

                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 4].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 4].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                for (int i = 0; i < ncol; i++) //inserta los datos 
                {                    
                    for (int j = 0; j < nfil; j++)
                    {
                        ws.Cells[j + row, i + 5].Value = listSerieData[i][j];
                    }
                }

                for(int i = 0; i< list.Count; i++)
                {
                    ws.Cells[i + row, 3].Value = list[i].Inicio;
                    ws.Cells[i + row, 4].Value = list[i].Fin;
                }

                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + 0, 3, row + nfil - 1, ncol + 5])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
                string xAxisTitle = grafico.XAxisTitle;
                string yAxisTitle = grafico.YaxixTitle;
                //AddGraficoLineas(ws, nfil, ncol, xAxisTitle, yAxisTitle, titulo);
                var chart = ws.Drawings.AddChart("chart1", eChartType.ColumnClustered);
                var chart2 = chart.PlotArea.ChartTypes.Add(eChartType.Line) as ExcelLineChart;
                var chart3 = chart.PlotArea.ChartTypes.Add(eChartType.ColumnClustered);
                var chart4 = chart.PlotArea.ChartTypes.Add(eChartType.Line) as ExcelLineChart;
                chart2.UseSecondaryAxis = true;
                chart4.UseSecondaryAxis = true;
                chart2.DataLabel.ShowValue = true;
                chart4.DataLabel.ShowValue = true;
                


                chart.SetSize(800, 600);
                chart2.SetPosition(5, 0, ncol + 5, 0);
                chart2.SetSize(800, 600);
                chart3.SetPosition(5, 0, ncol + 5, 0);
                chart3.SetSize(800, 600);
                chart4.SetPosition(5, 0, ncol + 5, 0);
                chart4.SetSize(800, 600);
                chart2.YAxis.Deleted = false;
                chart2.YAxis.TickLabelPosition = eTickLabelPosition.High;
                chart2.YAxis.Title.Text = grafico.Series[1].YAxisTitle;
                chart2.YAxis.Title.Font.Color = Color.Red;
                chart2.YAxis.Font.Color = Color.Red;                
                

                for (int i = 0; i < ncol; i++)
                {
                    var ran1 = ws.Cells[row, 5 + i, row + nfil - 1, 5 + i];
                    var ran2 = ws.Cells[row, 2, row + nfil - 1, 2];
                    switch (i)
                    {
                        case 0:
                            chart.Series.Add(ran1, ran2);
                            chart.Series[0].Header = ws.Cells[6, 5 + i].Value.ToString();  
                            
                            break;
                        case 1:
                            chart2.Series.Add(ran1, ran2);
                            chart2.Series[0].Header = ws.Cells[6, 5 + i].Value.ToString();
                            
                            break;
                        case 2:
                            chart3.Series.Add(ran1, ran2);
                            chart3.Series[0].Header = ws.Cells[6, 5 + i].Value.ToString();
                            break;
                        case 3:
                            chart4.Series.Add(ran1, ran2);
                            chart4.Series[0].Header = ws.Cells[6, 5 + i].Value.ToString();
                            break;
                    }
                }

                chart.Legend.Position = eLegendPosition.Bottom;
                chart.Legend.Add();
                chart.Title.Text = titulo;
                chart.Title.Font.Size = 8;
                chart.YAxis.Font.Size = 11;
                chart.XAxis.Title.Text = xAxisTitle;
                chart.YAxis.Title.Text = grafico.Series[0].YAxisTitle;
                chart.YAxis.Title.Font.Color = Color.SteelBlue;
                chart.YAxis.Font.Color = Color.SteelBlue;
                
                xlPackage.Save();
            }
        }

        //genera archivo de grafico capacidad de importacion

        public void GenerarArchivoCapacInter(GraficoWeb grafico, string fechaInicio, string fechaFin,
            string rutaArchivo, string rutaPlantilla)
        {
            var listCategoriaGrafico = grafico.SerieDataS[0]; // Lista de DIAS/HORAS ordenados para la categoria del grafico
            List<String> listSerieName = grafico.SeriesName; //Lista de nombres de las series del grafico(Ptos de medicion)
            //decimal?[][] listSerieData = model.Grafico.seriesData; // lista de valores para las series del grafico
            var listaData = grafico.SerieDataS;
            //string ruta = ConfigurationManager.AppSettings[ConstantesInterconexiones.ReporteInterconexiones].ToString();
            FileInfo newFile = new FileInfo(rutaArchivo);//ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones);
            FileInfo template = new FileInfo(rutaPlantilla);//ruta + ConstantesInterconexiones.PlantillaCapacidad);
            int nfil = listCategoriaGrafico.Length;
            int ncol = grafico.Series.Count;
            int row = 7;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);//ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                //ws = xlPackage.Workbook.Worksheets.Add(model.SheetName);
                ws = xlPackage.Workbook.Worksheets["GRAFICO"];

                //string titulo = model.TituloReporteXLS;
                string titulo = grafico.TitleText;

                //ConfiguraEncabezadoHojaExcel(ws, titulo, model.FechaInicio, model.FechaFin);
                ws.Cells[3, 3].Value = fechaInicio;
                ws.Cells[4, 3].Value = fechaFin;

                for (int i = 0; i < ncol; i++)
                {
                    ws.Cells[row - 1, i + 3].Value = listSerieName[i];
                }
                for (int i = 0; i < nfil; i++)
                {
                    ws.Cells[row + i, 2].Value = listCategoriaGrafico[i].X.ToString(ConstantesAppServicio.FormatoFechaHora);
                }



                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 3].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 3].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                for (int j = 0; j < nfil; j++)  //inserta los datos 
                {
                    decimal suma = 0;
                    for (int i = 0; i < ncol; i++)
                    {
                        ws.Cells[j + row, i + 3].Value = listaData[i][j].Y;//listSerieData[i][j];
                        if (listaData[i][j].Y != null)
                            suma += (decimal)listaData[i][j].Y;
                    }
                    ws.Cells[j + row, ncol + 3].Value = suma;
                }
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + 0, 3, row + nfil - 1, 3 + ncol])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
                string xAxisTitle = grafico.XAxisTitle;
                string yAxisTitle = grafico.YaxixTitle;
                // Genera Grafico
                var chart = ws.Drawings.AddChart("chartCapcExec", eChartType.Line);
                chart.SetPosition(5, 0, ncol + 4, 0);
                chart.SetSize(1200, 700);
                for (int i = 0; i < ncol; i++)
                {

                    var ran1 = ws.Cells[row, 3 + i, row + nfil - 1, 3 + i];
                    var ran2 = ws.Cells[row, 2, row + nfil - 1, 2];

                    chart.Series.Add(ran1, ran2);
                    chart.Series[i].Header = ws.Cells[6, 3 + i].Value.ToString();
                }
                chart.Legend.Position = eLegendPosition.Bottom;
                chart.Legend.Add();
                chart.Title.Text = titulo;
                chart.YAxis.Font.Size = 11;
                chart.XAxis.Title.Text = xAxisTitle;
                chart.YAxis.Title.Text = yAxisTitle;
                xlPackage.Save();
            }

        }


        //genera archivo de grafico capacidad de importacion, excedentes de exportacion
        public void GenerarArchivoExcedente(GraficoWeb grafico, string fechaInicio, string fechaFin,
            string rutaArchivo, string rutaPlantilla)
        {
            var listCategoriaGrafico = grafico.SerieDataS[0]; // Lista de DIAS/HORAS ordenados para la categoria del grafico
            List<String> listSerieName = grafico.SeriesName; //Lista de nombres de las series del grafico(Ptos de medicion)
            //decimal?[][] listSerieData = model.Grafico.seriesData; // lista de valores para las series del grafico
            var listaData = grafico.SerieDataS;
            //string ruta = ConfigurationManager.AppSettings[ConstantesInterconexiones.ReporteInterconexiones].ToString();
            FileInfo newFile = new FileInfo(rutaArchivo);//ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones);
            FileInfo template = new FileInfo(rutaPlantilla);//ruta + ConstantesInterconexiones.PlantillaCapacidad);
            int nfil = listCategoriaGrafico.Length;
            int ncol = listSerieName.Count;
            int row = 7;

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);//ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = null;
                ws = xlPackage.Workbook.Worksheets["GRAFICO"];
                string titulo = "Excedentes de Exportación";
                ws.Cells[3, 3].Value = fechaInicio;
                ws.Cells[4, 3].Value = fechaFin;
                ws.Cells[row - 1, 3].Value = "S.E. Zorritos (MW)";

                for (int i = 0; i < nfil; i++)
                {
                    ws.Cells[row + i, 2].Value = listCategoriaGrafico[i].X.ToString(ConstantesAppServicio.FormatoFechaHora);
                }

                // borde de region de datos
                var borderReg = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Border;
                borderReg.Bottom.Style = borderReg.Top.Style = borderReg.Left.Style = borderReg.Right.Style = ExcelBorderStyle.Thin;
                var fontTabla = ws.Cells[row, 2, row + nfil - 1, ncol + 2].Style.Font;
                fontTabla.Size = 8;
                fontTabla.Name = "Calibri";

                for (int i = 0; i < ncol; i++) //inserta los datos 
                    for (int j = 0; j < nfil; j++)
                    {
                        ws.Cells[j + row, i + 3].Value = listaData[i][j].Y;//listSerieData[i][j];
                    }
                ws.Cells[nfil + 1 + row, 2].Value = "(*) Se considera la capacidad de la línea L-2249 más la diferencia entre la generación disponible y la demanda de la SE Zorritos.";
                ////////////// Formato de Celdas Valores
                using (var range = ws.Cells[row + 0, 3, row + nfil - 1, 3])
                {
                    range.Style.Numberformat.Format = @"0.000";
                }
                string xAxisTitle = grafico.XAxisTitle;
                string yAxisTitle = grafico.YaxixTitle;
                // Genera Grafico
                var chart = ws.Drawings.AddChart("chartCapcExec", eChartType.Line);
                chart.SetPosition(5, 0, ncol + 3, 0);
                chart.SetSize(1200, 700);
                for (int i = 0; i < ncol; i++)
                {

                    var ran1 = ws.Cells[row, 3 + i, row + nfil - 1, 3 + i];
                    var ran2 = ws.Cells[row, 2, row + nfil - 1, 2];

                    chart.Series.Add(ran1, ran2);
                    chart.Series[i].Header = listSerieName[0];
                }
                chart.Legend.Position = eLegendPosition.Bottom;
                chart.Legend.Add();
                chart.Title.Text = titulo;
                chart.YAxis.Font.Size = 11;
                chart.XAxis.Title.Text = xAxisTitle;
                chart.YAxis.Title.Text = yAxisTitle;
                xlPackage.Save();
            }

        }


        /// <summary>
        /// Genera encabezado de reporte excel
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="titulo"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="rutaLogo"></param>
        public void ConfiguraEncabezadoHojaExcel(ExcelWorksheet ws, string titulo, string fechaIni, string fechaFin,
            string rutaLogo)
        {
            //string ruta = ConfigurationManager.AppSettings[rutaReporte].ToString();
            //ConstantesInterconexiones.ReporteInterconexiones
            AddImage(ws, 1, 0, rutaLogo);//ruta + Constantes.NombreLogoCoes);
            ws.Cells[1, 4].Value = titulo;
            var font = ws.Cells[1, 4].Style.Font;
            font.Size = 16;
            font.Bold = true;
            font.Name = "Calibri";
            var borderFecha = ws.Cells[3, 2, 4, 3].Style.Border;
            borderFecha.Bottom.Style = borderFecha.Top.Style = borderFecha.Left.Style = borderFecha.Right.Style = ExcelBorderStyle.Thin;
            var fontTabla = ws.Cells[3, 2, 4, 3].Style.Font;
            fontTabla.Size = 8;
            fontTabla.Name = "Calibri";
            fontTabla.Bold = true;
            ws.Cells[3, 2].Value = "FECHA INICIO:";
            ws.Cells[3, 3].Value = fechaIni;
            ws.Cells[4, 2].Value = "FECHA FIN:";
            ws.Cells[4, 3].Value = fechaFin;

        }

        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        ///         
        private void AddImage(ExcelWorksheet ws, int columnIndex, int rowIndex, string filePath)
        {
            //How to Add a Image using EP Plus
            Bitmap image = new Bitmap(filePath);

            ExcelPicture picture = null;
            if (image != null)
            {
                picture = ws.Drawings.AddPicture("pic" + rowIndex.ToString() + columnIndex.ToString(), image);
                picture.From.Column = columnIndex;
                picture.From.Row = rowIndex;
                picture.From.ColumnOff = Pixel2MTU(1); //Two pixel space for better alignment
                picture.From.RowOff = Pixel2MTU(1);//Two pixel space for better alignment
                picture.SetSize(120, 40);

            }
        }

        /// <summary>
        /// Deterina ancho en pixeles para el logo
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public int Pixel2MTU(int pixels)
        {
            //convert pixel to MTU
            int MTU_PER_PIXEL = 9525;
            int mtus = pixels * MTU_PER_PIXEL;
            return mtus;
        }

        #endregion

        #region Metodos para Exportar a Excel

        #endregion
    }

    public class LogErrorInterconexion
    { 
        public string Linea { get; set; }
        public string Fecha { get; set; }
        public string Comentario { get; set; }
        public string Parametro { get; set; }
        public string Medidor { get; set; }
    }

    public class EstructuraEvolucionEnergia
    { 
        public int Rowspan { get; set; }
        public string Fecha { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public decimal EnergiaExportada { get; set; }
        public decimal MaximaEnergiaExportada { get; set; }
        public decimal EnergiaImportada { get; set; }
        public decimal MaximaEnergiaImportada { get; set; }
        public bool FlagAgrupacion { get; set; }
    }


}
