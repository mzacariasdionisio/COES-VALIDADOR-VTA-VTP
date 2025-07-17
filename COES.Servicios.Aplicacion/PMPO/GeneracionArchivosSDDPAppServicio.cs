using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PMPO
{
    /// <summary>
    /// Clase de la capa de servicio del aplicativo PMPO para Integración SDDP
    /// </summary>
    public class GeneracionArchivosSDDPAppServicio
    {
        readonly ProgramacionAppServicio servPmpo = new ProgramacionAppServicio();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeneracionArchivosSDDPAppServicio));

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GeneracionArchivosSDDPAppServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region Tablas PMPO_*

        #region Métodos Tabla PMO_DAT_PMHI_TR

        public int SaveDatPmhiTr(PmoDatPmhiTrDTO entity)
        {
            try
            {
                return FactorySic.GetPmoDatPmhiTrRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int UpdateDatPmhiTr(PmoDatPmhiTrDTO entity)
        {
            try
            {
                return FactorySic.GetPmoDatPmhiTrRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<PmoDatPmhiTrDTO> ListDatPmhiTr(int codigoPeriodo, string tipo)
        {
            return FactorySic.GetPmoDatPmhiTrRepository().ListDatPmhiTr(codigoPeriodo, tipo);
        }

        public int CountDatPmhiTr(int periCodi, string tipo)
        {
            return FactorySic.GetPmoDatPmhiTrRepository().CountDatPmhiTr(periCodi, tipo);
        }

        #endregion

        #endregion

        public string GetDirectorioDat()
        {
            return ConfigurationManager.AppSettings[ConstantesPMPO.KeyFlagPmpoDirectorioDat];
        }

        #region Pantalla inicial .dat Resultados

        public List<PmoFormatoDTO> ListarFormatoXMes(int pmpericodi, string horizonte)
        {
            List<PmoFormatoDTO> listaFormatos;

            if ("S" == horizonte)
            {
                listaFormatos = servPmpo.ListFormatoByArchivo("DAT");

                var cantCgnd = CountDatCgnd(pmpericodi);
                var cantDbf = CountDatDbf(pmpericodi);
                var cantDbus = CountDatDbus(pmpericodi);
                var cantGndse = CountDatGndse(pmpericodi);
                var cantMgnd = CountDatMgnd(pmpericodi);
                var cantpmhisepe = CountDatPmhiTr(pmpericodi, ConstantesPMPO.FormatoHidraulicoSemanal);
                var cantpmtrsepe = CountDatPmhiTr(pmpericodi, ConstantesPMPO.FormatoTermicoSemanal);

                foreach (var reg in listaFormatos)
                {
                    switch (reg.PmFTabNombArchivo)
                    {
                        case "pmhisepe.dat":
                            reg.PmFTabQueryCount = cantpmhisepe.ToString();
                            reg.IndexWeb = "Disponibilidad";
                            reg.TipoFormato = ConstantesPMPO.FormatoHidraulicoSemanal;
                            break;
                        case "pmtrsepe.dat":
                            reg.PmFTabQueryCount = cantpmtrsepe.ToString();
                            reg.IndexWeb = "Disponibilidad";
                            reg.TipoFormato = ConstantesPMPO.FormatoTermicoSemanal;
                            break;
                        case "dbus.dat":
                            reg.PmFTabQueryCount = cantDbus.ToString();
                            reg.IndexWeb = "Dbus";
                            break;
                        case "dbf005pe.dat":
                            reg.PmFTabQueryCount = cantDbf.ToString();
                            reg.IndexWeb = "Dbf";
                            break;
                        case "cgndpe.dat":
                            reg.PmFTabQueryCount = cantCgnd.ToString();
                            reg.IndexWeb = "Cgndpe";
                            break;
                        case "mgndpe.dat":
                            reg.PmFTabQueryCount = cantMgnd.ToString();
                            reg.IndexWeb = "Mgndpe";
                            break;
                        case "gndse05pe.dat":
                            reg.PmFTabQueryCount = cantGndse.ToString();
                            reg.IndexWeb = "Gndse";
                            break;
                    }
                }
            }
            else
            {
                listaFormatos = servPmpo.ListFormatoByArchivo("DM");

                var cantpmhisepe = CountDatPmhiTr(pmpericodi, ConstantesPMPO.FormatoHidraulicoMensual);
                var cantpmtrsepe = CountDatPmhiTr(pmpericodi, ConstantesPMPO.FormatoTermicoMensual);

                foreach (var reg in listaFormatos)
                {
                    switch (reg.PmFTabNombArchivo)
                    {
                        case "pmhimepe.dat":
                            reg.PmFTabQueryCount = cantpmhisepe.ToString();
                            reg.IndexWeb = "Disponibilidad";
                            reg.TipoFormato = ConstantesPMPO.FormatoHidraulicoMensual;
                            break;
                        case "pmtrmepe.dat":
                            reg.PmFTabQueryCount = cantpmtrsepe.ToString();
                            reg.IndexWeb = "Disponibilidad";
                            reg.TipoFormato = ConstantesPMPO.FormatoTermicoMensual;
                            break;
                    }
                }
            }

            return listaFormatos;
        }

        public int CountDatGndse(int periCodi)
        {
            return FactorySic.GetPmoDatGndseRepository().CountDatGndse(periCodi);
        }

        public int CountDatCgnd(int periCodi)
        {
            return FactorySic.GetPmoDatCgndRepository().CountDatCgnd(periCodi);
        }
        public int CountDatDbf(int periCodi)
        {
            return FactorySic.GetPmoDatDbfRepository().CountDatDbf(periCodi);
        }
        public int CountDatDbus(int periCodi)
        {
            return FactorySic.GetPmoDatDbusRepository().CountDatDbus(periCodi);
        }

        public int CountDatMgnd(int periCodi)
        {
            return FactorySic.GetPmoDatMgndRepository().CountDatMgnd(periCodi);
        }

        #endregion

        #region Descargar archivos .dat

        /// <summary>
        /// Descargar archivos semanal
        /// </summary>
        /// <param name="archivos"></param>
        /// <param name="periodo"></param>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public async Task<bool[]> GenerarArchivosSemanal(string[] archivos, int periodo, string carpeta)
        {
            List<PmoDatPmhiTrDTO> pmhisepe = null;
            List<PmoDatPmhiTrDTO> pmtrsepe = null;
            List<PmoDatDbusDTO> dbus = null;
            List<PmoDatDbfDTO> dbf = null;
            List<PmoDatCgndDTO> cgnd = null;
            List<PmoDatMgndDTO> mgnd = null;
            List<PmoDatGndseDTO> gndse = null;
            List<PrGrupoDTO> gndseCabeceras = null;

            foreach (var dat in archivos)
            {
                switch (dat)
                {
                    case "pmhisepe.dat":
                        pmhisepe = ListDatPmhiTr(periodo, ConstantesPMPO.FormatoHidraulicoSemanal);
                        break;
                    case "pmtrsepe.dat":
                        pmtrsepe = ListDatPmhiTr(periodo, ConstantesPMPO.FormatoTermicoSemanal);
                        break;
                    case "dbus.dat":
                        dbus = ListDatDbus();
                        break;
                    case "dbf005pe.dat":
                        dbf = ListDatDbf(periodo);
                        break;
                    case "cgndpe.dat":
                        cgnd = ListDatCgnd();
                        break;
                    case "mgndpe.dat":
                        mgnd = ListDatMgnd();
                        break;
                    case "gndse05pe.dat":
                        gndse = ListDatGndse(periodo);
                        gndseCabeceras = ListDatGndseCabeceras(periodo);
                        break;
                }
            }

            return await ArchivosDat.GenerarDatSemanal(pmhisepe, pmtrsepe, dbus, dbf, cgnd, mgnd, gndse, gndseCabeceras, carpeta);
        }

        /// <summary>
        /// Descargar archivos mensual
        /// </summary>
        /// <param name="archivos"></param>
        /// <param name="periodo"></param>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public async Task<bool[]> GenerarArchivosMensual(string[] archivos, int periodo, string carpeta)
        {
            List<PmoDatPmhiTrDTO> pmhisepe = null;
            List<PmoDatPmhiTrDTO> pmtrsepe = null;

            foreach (var dat in archivos)
            {
                switch (dat)
                {
                    case "pmhimepe.dat":
                        pmhisepe = ListDatPmhiTr(periodo, ConstantesPMPO.FormatoHidraulicoMensual);
                        break;
                    case "pmtrmepe.dat":
                        pmtrsepe = ListDatPmhiTr(periodo, ConstantesPMPO.FormatoTermicoMensual);
                        break;
                }
            }

            return await ArchivosDat.GenerarDatMensual(pmhisepe, pmtrsepe, carpeta);
        }

        #endregion

        #region Procesar archivos

        public void ProcesarDat(int PeriCodi, string TableName, string Usuario)
        {
            FactorySic.GetPmoDatGenerateRepository().GenerateDat(PeriCodi, TableName, Usuario);

            if (TableName.Equals("mgndpe.dat"))
            {
                List<PmoDatMgndDTO> listMgnd = this.ListMgnd();
                var fechas = listMgnd.OrderBy(p => p.PmMgndFecha).Select(p => p.PmMgndFecha).Distinct().ToList();

                foreach (var fecha in fechas)
                {
                    FactorySic.GetPmoDatGenerateRepository().GenerateDatMgndPtoInstFactOpe((DateTime)fecha);
                }
            }
        }

        public void ProcesarDatNew(int periCodi, string TableName, string tipoReporteMantto)
        {
            if (TableName == "pmhisepe.dat")
                ProcesarDisponibilidadSemanal(periCodi, "H", tipoReporteMantto);// Cálculo de Disponibilidad de Centrales Hidráulicas		 

            if (TableName == "pmtrsepe.dat")
                ProcesarDisponibilidadSemanal(periCodi, "T", tipoReporteMantto);// Cálculo de Disponibilidad de Centrales Térmicas

            if (TableName == "dbf005.dat")
                generarDataProyeccionDemanadaBarras(periCodi);// Generar data Proyeccion en Demanda Barras SDDP

            if (TableName == "gndse05.dat")
                generarDataProyeccionRER(periCodi);// Generar data RER
        }

        public void ProcesarDatMensual(int periCodi, string TableName, string tipoReporteMantto)
        {
            if (TableName == "pmhimepe.dat")
                ProcesarDisponibilidadMensual(periCodi, "HM", tipoReporteMantto);// Cálculo de Disponibilidad de Centrales Hidráulicas		 

            if (TableName == "pmtrmepe.dat")
                ProcesarDisponibilidadMensual(periCodi, "TM", tipoReporteMantto);// Cálculo de Disponibilidad de Centrales Térmicas
        }

        #endregion

        #region Mantenimiento (1. Hidráulico, 2. Térmico)

        #region Disponibilidad de las unidades de generación

        public PmoPeriodoDTO GetRangoPeriodoManttos(int periocodi, string tipoFormato)
        {
            PmoPeriodoDTO periodoAct = servPmpo.GetByIdPmoPeriodo(periocodi);
            int anioActual = periodoAct.Pmperifecinimes.Year;

            if (ConstantesPMPO.FormatoHidraulicoSemanal == tipoFormato || ConstantesPMPO.FormatoTermicoSemanal == tipoFormato)
            {
                //Obtenemos los años
                List<int> lAnios = new List<int>() { anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };
                List<int> lAniosMantto = new List<int>() { anioActual - 1, anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };

                List<PmpoSemana> listaAllAnio = new List<PmpoSemana>();
                foreach (var anio in lAniosMantto)
                {
                    List<PmpoSemana> listaXAnio = servPmpo.ListarSemanaXAnio(anio, out List<string> listMsjVal);
                    listaAllAnio.AddRange(listaXAnio);
                }

                var regSemFinalData = listaAllAnio.Where(x => x.Anio == periodoAct.PmPeriFecFinMantAnual.Year && x.Mes == periodoAct.PmPeriFecFinMantAnual.Month).OrderByDescending(x => x.NroSemana).FirstOrDefault();
                DateTime fechaIniMantto = listaAllAnio.Find(x => x.Anio == anioActual && x.NroSemana == 1).FechaIni;
                DateTime fechaFinMantto = regSemFinalData.FechaFin.AddDays(1);

                periodoAct.PmPeriFecIniMantMensual = fechaIniMantto;
                periodoAct.PmPeriFecFinMantMensual = periodoAct.Pmperifecinimes.AddMonths(1).AddDays(-1);
                periodoAct.PmPeriFecIniMantAnual = periodoAct.Pmperifecinimes.AddMonths(1);
            }
            else
            {
                //Obtenemos los años
                List<int> lAnios = new List<int>() { anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };
                List<int> lAniosMantto = new List<int>() { anioActual - 1, anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };

                List<PmoMesDTO> listaAllAnio = new List<PmoMesDTO>();
                foreach (var anio in lAniosMantto)
                {
                    List<PmoMesDTO> listaXAnio = servPmpo.ListarSemanaMesDeAnho(anio, ConstantesPMPO.AccionEditar, null);
                    listaAllAnio.AddRange(listaXAnio);
                }

                var regMesFinalData = listaAllAnio.Where(x => x.Pmmesfecinimes.Year == periodoAct.PmPeriFecFinMantAnual.Year && x.Pmmesfecinimes.Month == periodoAct.PmPeriFecFinMantAnual.Month).OrderByDescending(x => x.Pmmesfecinimes).FirstOrDefault();
                DateTime fechaIniMantto = listaAllAnio.Find(x => x.Pmmesfecinimes.Year == anioActual && x.Pmmesfecinimes.Month == 1).Pmmesfecinimes;
                DateTime fechaFinMantto = regMesFinalData.Pmmesfecinimes.AddMonths(1);

                periodoAct.PmPeriFecIniMantMensual = fechaIniMantto;
                periodoAct.PmPeriFecFinMantMensual = periodoAct.Pmperifecinimes.AddMonths(1).AddDays(-1);
                periodoAct.PmPeriFecIniMantAnual = periodoAct.Pmperifecinimes.AddMonths(1);
            }

            //formatear string
            periodoAct.SPmPeriFecIniMantMensual = periodoAct.PmPeriFecIniMantMensual.ToString(ConstantesAppServicio.FormatoFecha);
            periodoAct.SPmPeriFecFinMantMensual = periodoAct.PmPeriFecFinMantMensual.ToString(ConstantesAppServicio.FormatoFecha);
            periodoAct.SPmPeriFecIniMantAnual = periodoAct.PmPeriFecIniMantAnual.ToString(ConstantesAppServicio.FormatoFecha);
            periodoAct.SPmPeriFecFinMantAnual = periodoAct.PmPeriFecFinMantAnual.ToString(ConstantesAppServicio.FormatoFecha);

            return periodoAct;
        }

        /// <summary>
        /// pmhisepe.dat, pmtrsepe.dat
        /// </summary>
        /// <param name="periCodi"></param>
        /// <param name="tipoCentral"></param>
        public void ProcesarDisponibilidadSemanal(int periCodi, string tipoCentral, string tipoReporteMantto)
        {
            //Eliminamos la data existente en la tabla PMO_DAT_PMHI_TR, para el periodo de trabajo
            FactorySic.GetPmoDatGenerateRepository().DeleteDataPorPeriodoYtipo(periCodi, tipoCentral);

            PmpoProcesamientoDat obj = ObtenerProcesamientoDisponibilidad(periCodi, tipoCentral, tipoReporteMantto, true);

            //guardar en bd
            foreach (var reg in obj.ListaResultado)
            {
                SaveDatPmhiTr(reg);
            }

        }

        public PmpoProcesamientoDat ObtenerProcesamientoDisponibilidad(int periCodi, string tipoFormato, string tipoReporteMantto, bool incluirReplica)
        {
            int tsddpcodi = GetTsddpcodiXCadena(tipoFormato);

            PmpoProcesamientoDat obj = new PmpoProcesamientoDat();
            obj.Tsddpcodi = tsddpcodi;
            obj.TipoFormato = tipoFormato;

            //Actualizamos las fechas de mantenimiento anual
            PmoPeriodoDTO periodoAct = servPmpo.GetByIdPmoPeriodo(periCodi);

            //obtener lista de entidades
            ListarEntidadesCalculoDisp(tsddpcodi, out List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                        , out List<PmoSddpCodigoDTO> listaSddp, out List<EqEquipoDTO> listaEq, out List<EqEquipoDTO> listaCentralComb, out List<PrGrupoDTO> listaGrupoModo);
            obj.ListaCodigo = listaSddp;

            //Obtenemos los años
            int anioActual = periodoAct.Pmperifecinimes.Year;
            List<int> lAnios = new List<int>() { anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };
            List<int> lAniosMantto = new List<int>() { anioActual - 1, anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };

            List<PmpoSemana> listaAllAnio = new List<PmpoSemana>();
            foreach (var anio in lAniosMantto)
            {
                List<PmpoSemana> listaXAnio = servPmpo.ListarSemanaXAnio(anio, out List<string> listMsjVal);
                listaAllAnio.AddRange(listaXAnio);
            }

            var regSemInicioData = listaAllAnio.Where(x => x.Anio == anioActual && x.Mes == 1).OrderBy(x => x.NroSemana).FirstOrDefault();
            var regSemFinalData = listaAllAnio.Where(x => x.Anio == periodoAct.PmPeriFecFinMantAnual.Year && x.Mes == periodoAct.PmPeriFecFinMantAnual.Month).OrderByDescending(x => x.NroSemana).FirstOrDefault();
            DateTime fechaIniMantto = listaAllAnio.Find(x => x.Anio == anioActual && x.NroSemana == 1).FechaIni;
            DateTime fechaFinMantto = regSemFinalData.FechaFin.AddDays(1);

            //Obtenemos la data de los eventos de mantenimiento para el periodo de procesamiento
            List<EveManttoDTO> listaManttos = ListarMantto(periodoAct.Pmperifecinimes, periodoAct.PmPeriFecFinMantAnual.AddDays(1), tipoReporteMantto, true);
            listaManttos = listaManttos.Where(x => x.Evenini >= fechaIniMantto && x.Evenini < fechaFinMantto).ToList();
            foreach (var reg in listaManttos)
            {
                var regSem = listaAllAnio.Find(x => x.FechaIni <= reg.Evenini && reg.Evenini < x.FechaFin.AddDays(1));
                if (regSem != null)
                {
                    reg.Anio = regSem.Anio;
                    reg.Mes = regSem.Mes;
                    reg.NroSemana = regSem.NroSemana;
                }
                else
                { }
            }
            obj.ListaManttos = listaManttos;

            List<PmpoHITotal> listaDisp = new List<PmpoHITotal>();
            //Obtener calculo por semana
            var listaPeriodoProcesar = listaAllAnio.Where(x => x.FechaIni >= regSemInicioData.FechaIni && x.FechaIni <= regSemFinalData.FechaIni).ToList();
            foreach (var reg in listaPeriodoProcesar)
            {
                reg.TotalHoras = 168;
            }
            bool esProcesarTodoComoHidro = false;
            if (esProcesarTodoComoHidro)
            {
                //utilizado para comparar el resultado de los termicos.
                listaDisp = CalcularIndisponibilidadPmpo(tsddpcodi, listaPeriodoProcesar, listaSddp, listaEq, listaCorrelaciones, listaManttos);
            }
            else
            {
                List<PmpoHITotal> listaDispNoCicloComb = CalcularIndisponibilidadPmpo(tsddpcodi, listaPeriodoProcesar
                                                                        , listaSddp.Where(x => !x.TieneCentralCicloComb).ToList(), listaEq, listaCorrelaciones, listaManttos);

                List<PmpoHITotal> listaDispCicloComb = CalcularIndisponibilidadTermoPmpo(tsddpcodi, listaPeriodoProcesar
                                                                        , listaSddp.Where(x => x.TieneCentralCicloComb).ToList()
                                                                        , listaCentralComb, listaEq, listaGrupoModo, listaCorrelaciones, listaManttos);
                listaDisp.AddRange(listaDispNoCicloComb);
                listaDisp.AddRange(listaDispCicloComb);
            }

            if (incluirReplica)
            {
                var listaReplica = ListarReplicaIndisponibilidad(regSemFinalData, listaDisp);
                listaDisp.AddRange(listaReplica);
            }
            obj.ListaDisp = listaDisp.OrderBy(x => x.Sddpnum).ThenBy(x => x.Anio).ThenBy(x => x.NroSemana).ToList();

            //iterar por cada grupocodisddp
            List<PmoDatPmhiTrDTO> listaResultado = new List<PmoDatPmhiTrDTO>();
            foreach (var regSddp in listaSddp)
            {
                List<PmpoHITotal> listaDispXgru = listaDisp.Where(x => x.Sddpcodi == regSddp.Sddpcodi).ToList();

                //Bucle de Años
                foreach (var anio in lAnios)
                {
                    var listaDispAnio = listaDispXgru.Where(x => x.Anio == anio).ToList();

                    //Insertamos el registro en la tabla 
                    PmoDatPmhiTrDTO pmoDatPmhiTrDTO = new PmoDatPmhiTrDTO();
                    pmoDatPmhiTrDTO.PmPeriCodi = periCodi;
                    pmoDatPmhiTrDTO.Sddpcodi = regSddp.Sddpcodi;
                    pmoDatPmhiTrDTO.PmPmhtAnhio = anio;
                    pmoDatPmhiTrDTO.PmPmhtTipo = tipoFormato;

                    for (int i = 1; i <= 52; i++)
                    {
                        if (i == 17)
                        { }
                        var regDispXSem = listaDispAnio.Find(x => x.NroSemana == i);
                        if (regDispXSem != null)
                        {
                            pmoDatPmhiTrDTO.GetType().GetProperty("PmPmhtDisp" + i.ToString("D2")).SetValue(pmoDatPmhiTrDTO, regDispXSem.PorcentajeDisp);
                        }
                        else
                        { }
                    }

                    listaResultado.Add(pmoDatPmhiTrDTO);
                }
            }

            obj.ListaResultado = listaResultado;
            obj.ListaManttos = obj.ListaManttos.OrderBy(x => x.Anio).ThenBy(x => x.NroSemana).ThenBy(x => x.Evenini)
                                                .ThenBy(x => x.Areanomb).ThenBy(x => x.Equiabrev).ToList();

            return obj;
        }

        /// <summary>
        /// pmhisepe.dat, pmtrsepe.dat
        /// </summary>
        /// <param name="periCodi"></param>
        /// <param name="tipoCentral"></param>
        public void ProcesarDisponibilidadMensual(int periCodi, string tipoCentral, string tipoReporteMantto)
        {
            //Eliminamos la data existente en la tabla PMO_DAT_PMHI_TR, para el periodo de trabajo
            FactorySic.GetPmoDatGenerateRepository().DeleteDataPorPeriodoYtipo(periCodi, tipoCentral);

            PmpoProcesamientoDat obj = ObtenerProcesamientoDisponibilidadMensual(periCodi, tipoCentral, tipoReporteMantto, true);

            //guardar en bd
            foreach (var reg in obj.ListaResultado)
            {
                SaveDatPmhiTr(reg);
            }
        }

        public PmpoProcesamientoDat ObtenerProcesamientoDisponibilidadMensual(int periCodi, string tipoFormato, string tipoReporteMantto, bool incluirReplica)//tipoCentral: HM, TM
        {
            int tsddpcodi = GetTsddpcodiXCadena(tipoFormato);

            PmpoProcesamientoDat obj = new PmpoProcesamientoDat();
            obj.Tsddpcodi = tsddpcodi;
            obj.TipoFormato = tipoFormato;

            //Actualizamos las fechas de mantenimiento anual
            PmoPeriodoDTO periodoAct = servPmpo.GetByIdPmoPeriodo(periCodi);

            //obtener lista de entidades
            ListarEntidadesCalculoDisp(tsddpcodi, out List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                        , out List<PmoSddpCodigoDTO> listaSddp, out List<EqEquipoDTO> listaEq, out List<EqEquipoDTO> listaCentralComb, out List<PrGrupoDTO> listaGrupoModo);
            obj.ListaCodigo = listaSddp;

            //Obtenemos los años
            int anioActual = periodoAct.Pmperifecinimes.Year;
            List<int> lAnios = new List<int>() { anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };
            List<int> lAniosMantto = new List<int>() { anioActual - 1, anioActual, anioActual + 1, anioActual + 2, anioActual + 3 };

            List<PmoMesDTO> listaAllAnio = new List<PmoMesDTO>();
            foreach (var anio in lAniosMantto)
            {
                List<PmoMesDTO> listaXAnio = servPmpo.ListarSemanaMesDeAnho(anio, ConstantesPMPO.AccionEditar, null);
                listaAllAnio.AddRange(listaXAnio);
            }

            var regSemInicioData = listaAllAnio.Where(x => x.Pmmesfecinimes.Year == anioActual && x.Pmmesfecinimes.Month == 1).FirstOrDefault();
            var regMesFinalData = listaAllAnio.Where(x => x.Pmmesfecinimes.Year == periodoAct.PmPeriFecFinMantAnual.Year && x.Pmmesfecinimes.Month == periodoAct.PmPeriFecFinMantAnual.Month).OrderByDescending(x => x.Pmmesfecinimes).FirstOrDefault();
            DateTime fechaIniMantto = listaAllAnio.Find(x => x.Pmmesfecinimes.Year == anioActual && x.Pmmesfecinimes.Month == 1).Pmmesfecinimes;
            DateTime fechaFinMantto = regMesFinalData.Pmmesfecinimes.AddMonths(1);

            //Obtenemos la data de los eventos de mantenimiento para el periodo de procesamiento
            List<EveManttoDTO> listaManttos = ListarMantto(periodoAct.Pmperifecinimes, periodoAct.PmPeriFecFinMantAnual.AddDays(1), tipoReporteMantto, true); //periodoAct.PmPeriFecIniMantMensual, periodoAct.PmPeriFecFinMantAnual, true);
            listaManttos = listaManttos.Where(x => x.Evenini >= fechaIniMantto && x.Evenini < fechaFinMantto).ToList();
            foreach (var reg in listaManttos)
            {
                var regMes = listaAllAnio.Find(x => x.Pmmesfecinimes <= reg.Evenini && reg.Evenini < x.Pmmesfecinimes.AddMonths(1));
                if (regMes != null)
                {
                    reg.Anio = regMes.Pmmesfecinimes.Year;
                    reg.Mes = regMes.Pmmesfecinimes.Month;
                }
                else
                { }
            }
            obj.ListaManttos = listaManttos;

            List<PmpoHITotal> listaDisp = new List<PmpoHITotal>();
            //Obtener calculo por mes
            var listaPeriodoProcesarMensual = listaAllAnio.Where(x => x.Pmmesfecinimes >= regSemInicioData.Pmmesfecinimes && x.Pmmesfecinimes <= regMesFinalData.Pmmesfecinimes).ToList();
            var listaPeriodoProcesar = new List<PmpoSemana>();
            foreach (var reg in listaPeriodoProcesarMensual)
            {
                listaPeriodoProcesar.Add(new PmpoSemana()
                {
                    FechaIni = reg.Pmmesfecinimes,
                    FechaFin = reg.Pmmesfecinimes.AddMonths(1).AddDays(-1),
                    Anio = reg.Pmmesfecinimes.Year,
                    Mes = reg.Pmmesfecinimes.Month,
                    TotalHoras = (reg.Pmmesfecinimes.AddMonths(1) - reg.Pmmesfecinimes).Days * 24
                });
            }
            List<PmpoHITotal> listaDispNoCicloComb = CalcularIndisponibilidadPmpo(tsddpcodi, listaPeriodoProcesar
                                                                   , listaSddp.Where(x => !x.TieneCentralCicloComb).ToList(), listaEq, listaCorrelaciones, listaManttos);

            List<PmpoHITotal> listaDispCicloComb = CalcularIndisponibilidadTermoPmpo(tsddpcodi, listaPeriodoProcesar
                                                                    , listaSddp.Where(x => x.TieneCentralCicloComb).ToList()
                                                                    , listaCentralComb, listaEq, listaGrupoModo, listaCorrelaciones, listaManttos);
            listaDisp.AddRange(listaDispNoCicloComb);
            listaDisp.AddRange(listaDispCicloComb);
            if (incluirReplica)
            {
                var listaReplica = ListarReplicaIndisponibilidadMensual(regMesFinalData, listaDisp);
                listaDisp.AddRange(listaReplica);
            }
            obj.ListaDisp = listaDisp.OrderBy(x => x.Sddpnum).ThenBy(x => x.Anio).ThenBy(x => x.Mes).ToList();

            //iterar por cada grupocodisddp
            List<PmoDatPmhiTrDTO> listaResultado = new List<PmoDatPmhiTrDTO>();
            foreach (var regGrupo in listaSddp)
            {
                List<PmpoHITotal> listaDispXgru = listaDisp.Where(x => x.Sddpcodi == regGrupo.Sddpcodi).ToList();

                //Bucle de Años
                foreach (var anio in lAnios)
                {
                    var listaDispAnio = listaDispXgru.Where(x => x.Anio == anio).ToList();

                    //Insertamos el registro en la tabla 
                    PmoDatPmhiTrDTO pmoDatPmhiTrDTO = new PmoDatPmhiTrDTO();
                    pmoDatPmhiTrDTO.PmPeriCodi = periCodi;
                    pmoDatPmhiTrDTO.Sddpcodi = regGrupo.Sddpcodi;
                    pmoDatPmhiTrDTO.PmPmhtAnhio = anio;
                    pmoDatPmhiTrDTO.PmPmhtTipo = tipoFormato;

                    for (int i = 1; i <= 12; i++)
                    {
                        DateTime fecha1Mes = new DateTime(anio, i, 1);

                        var regDispXSem = listaDispAnio.Find(x => x.Mes == i);

                        if (regDispXSem != null)
                        {
                            pmoDatPmhiTrDTO.GetType().GetProperty("PmPmhtDisp" + i.ToString("D2")).SetValue(pmoDatPmhiTrDTO, regDispXSem.PorcentajeDisp);
                        }
                        else
                        { }
                    }
                    for (int i = 13; i <= 52; i++)
                    {
                        pmoDatPmhiTrDTO.GetType().GetProperty("PmPmhtDisp" + i.ToString("D2")).SetValue(pmoDatPmhiTrDTO, 0.0m);
                    }
                    listaResultado.Add(pmoDatPmhiTrDTO);
                }
            }

            obj.ListaResultado = listaResultado;
            obj.ListaManttos = obj.ListaManttos.OrderBy(x => x.Anio).ThenBy(x => x.NroSemana).ThenBy(x => x.Evenini)
                                                .ThenBy(x => x.Areanomb).ThenBy(x => x.Equiabrev).ToList();

            return obj;
        }

        public int GetTsddpcodiXCadena(string tipoCentral)
        {
            return (tipoCentral == "H" || tipoCentral == "HM") ? ConstantesPMPO.TsddpPlantaHidraulica : ConstantesPMPO.TsddpPlantaTermica;
        }

        private List<PmpoHITotal> ListarReplicaIndisponibilidad(PmpoSemana regSemFinalData, List<PmpoHITotal> listaDisp)
        {
            List<PmpoHITotal> lRep = new List<PmpoHITotal>();

            if (regSemFinalData != null)
            {
                int anioIni = regSemFinalData.Anio;
                int semIni = 1;
                int anioFin = regSemFinalData.Anio;
                int semFin = regSemFinalData.NroSemana;

                if (regSemFinalData.NroSemana != 52)
                {
                    anioIni = regSemFinalData.Anio - 1;
                    semIni = regSemFinalData.NroSemana + 1;
                }

                var sublista1 = listaDisp.Where(x => x.Anio == anioIni && x.NroSemana >= semIni).ToList();
                var sublista2 = listaDisp.Where(x => x.Anio == anioFin && x.NroSemana <= semFin).ToList();

                List<PmpoHITotal> lARepl = new List<PmpoHITotal>();
                lARepl.AddRange(sublista1);
                lARepl.AddRange(sublista2);
                lARepl = lARepl.OrderBy(x => x.Anio).ThenBy(x => x.NroSemana).ToList();

                foreach (var reg in lARepl)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        var regClone = (PmpoHITotal)reg.Clone();
                        regClone.Anio += i;
                        lRep.Add(regClone);
                    }
                }
            }

            return lRep;
        }

        private List<PmpoHITotal> ListarReplicaIndisponibilidadMensual(PmoMesDTO regMesFinalData, List<PmpoHITotal> listaDisp)
        {
            List<PmpoHITotal> lRep = new List<PmpoHITotal>();

            if (regMesFinalData != null)
            {
                int anioIni = regMesFinalData.Pmmesfecinimes.Year;
                int mesIni = 1;
                int anioFin = regMesFinalData.Pmmesfecinimes.Year;
                int mesFin = regMesFinalData.Pmmesfecinimes.Month;

                if (regMesFinalData.Pmmesfecinimes.Month != 12)
                {
                    anioIni = regMesFinalData.Pmmesfecinimes.Year - 1;
                    mesIni = regMesFinalData.Pmmesfecinimes.Month + 1;
                }

                var sublista1 = listaDisp.Where(x => x.Anio == anioIni && x.Mes >= mesIni).ToList();
                var sublista2 = listaDisp.Where(x => x.Anio == anioFin && x.Mes <= mesFin).ToList();

                List<PmpoHITotal> lARepl = new List<PmpoHITotal>();
                lARepl.AddRange(sublista1);
                lARepl.AddRange(sublista2);
                lARepl = lARepl.OrderBy(x => x.Anio).ThenBy(x => x.Mes).ToList();

                foreach (var reg in lARepl)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        var regClone = (PmpoHITotal)reg.Clone();
                        regClone.Anio += i;
                        lRep.Add(regClone);
                    }
                }
            }

            return lRep;
        }

        private List<PmpoHITotal> CalcularIndisponibilidadPmpo(int tsddpcodi, List<PmpoSemana> listaAllAnio
                                                    , List<PmoSddpCodigoDTO> listaGrupo, List<EqEquipoDTO> listaEq, List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                                    , List<EveManttoDTO> listaManttos)
        {
            List<PmpoHITotal> listaDisp = new List<PmpoHITotal>();

            //iterar por cada grupocodisddp
            foreach (var regGr in listaGrupo)
            {
                if (regGr.Sddpnum == 9019)
                { }

                List<EveManttoDTO> listaManttoXGr = listaManttos.Where(x => regGr.ListaEquicodi.Contains(x.Equicodi ?? 0)).ToList();

                ProcesarListaMantto(tsddpcodi, listaManttoXGr, listaEq, new List<PmoSddpCodigoDTO>() { regGr }, out List<EveManttoDTO> listaFinalOut);
                List<PmpoHIDetalle> listaDetXGr = GenerarDetalleCuadro(listaFinalOut);

                //obtener los equipos relacionados al grupo
                var listaCorrelxGr = listaCorrelaciones.Where(x => x.Sddpcodi == regGr.Sddpcodi).ToList();

                foreach (var regPeriodo in listaAllAnio)
                {
                    int totalHxSem = regPeriodo.TotalHoras;

                    if (regPeriodo.NroSemana == 3)
                    { }
                    List<PmpoHIDetalle> listaDetXSem = listaDetXGr.Where(x => x.Horaini >= regPeriodo.FechaIni && x.Horafin <= regPeriodo.FechaFin.AddDays(1))
                                                                    .OrderBy(x => x.Horaini).ToList();
                    List<PmpoHIDetalle> listaDetXSemSinMantto = ListarRangoSinMantto(regPeriodo.FechaIni, regPeriodo.FechaFin, listaDetXSem);

                    List<EveManttoDTO> listaXSem = listaManttoXGr.Where(x => x.Evenini >= regPeriodo.FechaIni && x.Evenfin <= regPeriodo.FechaFin.AddDays(1)).ToList();

                    if (listaDetXSem.Any())
                    {
                        decimal horasIndisp = 0;
                        PmpoHITotal regTotXSem = new PmpoHITotal()
                        {
                            Sddpnomb = regGr.Sddpnomb,
                            Sddpnum = regGr.Sddpnum,
                            ListaEquicodi = regGr.ListaEquicodi,
                            Sddpcodi = regGr.Sddpcodi,
                            Anio = regPeriodo.Anio,
                            NroSemana = regPeriodo.NroSemana,
                            Mes = regPeriodo.Mes,
                            FechaIni = regPeriodo.FechaIni,
                            FechaFin = regPeriodo.FechaFin,
                            ListaDetXSem = new List<PmpoHIDetalle>()
                        };

                        foreach (var regDet in listaDetXSem)
                        {
                            decimal totalHours = (decimal)(regDet.Horafin - regDet.Horaini).TotalHours;

                            List<PmoConfIndispEquipoDTO> listaEqcorrel = new List<PmoConfIndispEquipoDTO>();
                            if (ConstantesPMPO.TsddpPlantaTermica == tsddpcodi && regGr.Grupocodimodo > 0)
                            {

                                if (!regGr.TieneModoEspecial)
                                {
                                    foreach (var equicodi in regGr.ListaEquicodi)
                                    {
                                        var regCorrel = listaCorrelxGr.Find(x => x.EquiCodi == equicodi);
                                        if (regDet.ListaEquicodi.Contains(equicodi))
                                        {
                                            var regEq = listaEq.Find(x => x.Equicodi == equicodi);
                                            var equiabrevTmp = regEq != null ? regEq.Equiabrev : "";

                                            if (regCorrel != null) listaEqcorrel.Add(regCorrel);
                                            else
                                            {
                                                listaEqcorrel.Add(new PmoConfIndispEquipoDTO()
                                                {
                                                    EquiCodi = equicodi,
                                                    EquiAbrev = equiabrevTmp,
                                                    PmCindPorcentaje = 100.0m,
                                                    AreaNomb = regGr.Central,
                                                    Central = regGr.Central,
                                                    Equipadre = regGr.Equipadre
                                                });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var equicodi in regGr.ListaEquicodiModo)
                                    {
                                        decimal porcentaje = MathHelper.Round(100.0m / regGr.ListaEquicodiModo.Count, 4);
                                        var regCorrel = listaCorrelxGr.Find(x => x.EquiCodi == equicodi);
                                        if (regDet.ListaEquicodi.Contains(equicodi))
                                        {
                                            var regEq = listaEq.Find(x => x.Equicodi == equicodi);
                                            var equiabrevTmp = regEq != null ? regEq.Equiabrev : "";

                                            if (regCorrel != null) listaEqcorrel.Add(regCorrel);
                                            else
                                            {
                                                listaEqcorrel.Add(new PmoConfIndispEquipoDTO()
                                                {
                                                    EquiCodi = equicodi,
                                                    EquiAbrev = equiabrevTmp,
                                                    PmCindPorcentaje = porcentaje,
                                                    AreaNomb = regGr.Central,
                                                    Central = regGr.Central,
                                                    Equipadre = regGr.Equipadre
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                listaEqcorrel = listaCorrelxGr.Where(x => regDet.ListaEquicodi.Contains(x.EquiCodi)).OrderByDescending(x => x.PmCindPorcentaje).ToList();
                            }

                            List<PmoConfIndispEquipoDTO> listaDetXRango = new List<PmoConfIndispEquipoDTO>();
                            foreach (var regEq in listaEqcorrel)
                            {
                                var regClon = (PmoConfIndispEquipoDTO)regEq.Clone();

                                var listaManttoXeq = listaXSem.Where(x => x.Equicodi == regEq.EquiCodi && (x.Evenini <= regDet.Horaini && regDet.Horaini < x.Evenfin)).ToList();
                                if (!listaManttoXeq.Any())
                                { }
                                regClon.ListaEvendescrip = listaManttoXeq.Select(x => GetDescripcionMantto(x, false)).ToList();
                                listaDetXRango.Add(regClon);
                            }
                            regDet.ListaCorrelaciones = listaDetXRango;
                            regDet.ListaEquiabrevStr = string.Join(", ", regDet.ListaCorrelaciones.Select(x => (x.EquiAbrev ?? "").Trim()));

                            decimal porIndispXRango = regDet.ListaCorrelaciones.Sum(x => x.PmCindPorcentaje);
                            if (porIndispXRango > 100) porIndispXRango = 100;

                            regDet.HorasTotal = totalHours;
                            regDet.HorasIndisp = (totalHours * porIndispXRango / 100.0m);
                            horasIndisp += regDet.HorasIndisp;
                        }

                        if (horasIndisp > totalHxSem)
                        {
                            horasIndisp = totalHxSem;
                        }

                        regTotXSem.HoraIndisp = horasIndisp;
                        regTotXSem.ListaDetXSem = listaDetXSem;
                        listaDisp.Add(regTotXSem);
                    }

                    if (listaDetXSemSinMantto.Any())
                    {
                        decimal horasIndisp = 0;
                        PmpoHITotal regTotXSem = new PmpoHITotal()
                        {
                            Sddpnomb = regGr.Sddpnomb,
                            Sddpnum = regGr.Sddpnum,
                            ListaEquicodi = regGr.ListaEquicodi,
                            Sddpcodi = regGr.Sddpcodi,
                            Anio = regPeriodo.Anio,
                            NroSemana = regPeriodo.NroSemana,
                            Mes = regPeriodo.Mes,
                            FechaIni = regPeriodo.FechaIni,
                            FechaFin = regPeriodo.FechaFin,
                            ListaDetXSem = new List<PmpoHIDetalle>()
                        };

                        foreach (var regDet in listaDetXSemSinMantto)
                        {
                            decimal totalHours = (decimal)(regDet.Horafin - regDet.Horaini).TotalHours;
                            regDet.HorasIndisp = 0;
                            horasIndisp += regDet.HorasIndisp;
                        }

                        regTotXSem.HoraIndisp = horasIndisp;
                        regTotXSem.ListaDetXSem = listaDetXSemSinMantto;
                        listaDisp.Add(regTotXSem);
                    }
                }
            }

            //generar porcentaje de disponibilidad por cada Sddp
            foreach (var regDisp in listaDisp)
            {
                if (regDisp.Sddpnum == 9102 && regDisp.NroSemana == 18)
                { }

                int totalHxMes = ((regDisp.FechaFin - regDisp.FechaIni).Days + 1) * 24;
                decimal horasIndisp = regDisp.HoraIndisp;

                decimal valorDispon = ((totalHxMes - horasIndisp) * 100.0m) / (totalHxMes * 1.0m);

                regDisp.HoraTotal = totalHxMes;
                regDisp.PorcentajeDisp = valorDispon;
            }

            return listaDisp;
        }

        private List<PmpoHITotal> CalcularIndisponibilidadTermoPmpo(int tsddpcodi, List<PmpoSemana> listaAllAnio
                                                    , List<PmoSddpCodigoDTO> listaSddp, List<EqEquipoDTO> listaCentral, List<EqEquipoDTO> listaEq, List<PrGrupoDTO> listaGrupoModo
                                                    , List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                                    , List<EveManttoDTO> listaManttos)
        {
            List<PmpoHITotal> listaDisp = new List<PmpoHITotal>();

            foreach (var regCentral in listaCentral)
            {
                List<PmoSddpCodigoDTO> listaSddpXCentral = listaSddp.Where(x => x.Equipadre == regCentral.Equicodi).OrderByDescending(x => x.Pe).ToList();

                List<int> listaEquicodiXCentral = new List<int>();
                foreach (var regSddp in listaSddpXCentral)
                {
                    var regCorrelxGr = listaCorrelaciones.Find(x => x.Sddpcodi == regSddp.Sddpcodi);
                    if (regCorrelxGr != null)
                        listaEquicodiXCentral.AddRange(regCorrelxGr.ListaEquicodi);
                }
                listaEquicodiXCentral = listaEquicodiXCentral.Distinct().ToList();

                List<EveManttoDTO> listaManttoXCentral = listaManttos.Where(x => listaEquicodiXCentral.Contains(x.Equicodi ?? 0)).ToList();

                PmoSddpCodigoDTO regSddpCentral = new PmoSddpCodigoDTO()
                {
                    Sddpcodi = regCentral.Equicodi,
                    ListaEquicodi = listaEquicodiXCentral,
                    Emprcodi = 0,
                };
                ProcesarListaMantto(tsddpcodi, listaManttoXCentral, listaEq, new List<PmoSddpCodigoDTO>() { regSddpCentral }, out List<EveManttoDTO> listaFinalOut);
                List<PmpoHIDetalle> listaDetXCentral = GenerarDetalleCuadro(listaFinalOut);

                foreach (var regPeriodo in listaAllAnio)
                {
                    int totalHxSem = regPeriodo.TotalHoras;

                    if (regPeriodo.NroSemana == 27)
                    { }
                    if (regPeriodo.Mes == 2)
                    { }
                    List<PmpoHIDetalle> listaDetXSem = listaDetXCentral.Where(x => x.Horaini >= regPeriodo.FechaIni && x.Horafin <= regPeriodo.FechaFin.AddDays(1))
                                                                    .OrderBy(x => x.Horaini).ToList();
                    List<PmpoHIDetalle> listaDetXSemSinMantto = ListarRangoSinMantto(regPeriodo.FechaIni, regPeriodo.FechaFin, listaDetXSem);

                    List<EveManttoDTO> listaManttoXSem = listaManttoXCentral.Where(x => x.Evenini >= regPeriodo.FechaIni && x.Evenfin <= regPeriodo.FechaFin.AddDays(1)).ToList();

                    List<PmpoHITotal> listaDispXSem = new List<PmpoHITotal>();
                    decimal horasIndisp = 0;
                    foreach (var regGr in listaSddpXCentral)
                    {
                        PmpoHITotal regTotXSem = new PmpoHITotal()
                        {
                            Sddpnomb = regGr.Sddpnomb,
                            Sddpnum = regGr.Sddpnum,
                            ListaEquicodi = regGr.ListaEquicodi,
                            Grupocodimodo = regGr.Grupocodimodo,
                            Sddpcodi = regGr.Sddpcodi,
                            Anio = regPeriodo.Anio,
                            NroSemana = regPeriodo.NroSemana,
                            Mes = regPeriodo.Mes,
                            FechaIni = regPeriodo.FechaIni,
                            FechaFin = regPeriodo.FechaFin,
                            ListaDetXSem = new List<PmpoHIDetalle>()
                        };

                        listaDispXSem.Add(regTotXSem);
                    }

                    if (listaDetXSem.Any())
                    {
                        foreach (var regDet in listaDetXSem)
                        {
                            decimal totalHours = (decimal)(regDet.Horafin - regDet.Horaini).TotalHours;

                            ListarModoDisp(listaSddpXCentral.Select(x => x.Grupocodimodo).Where(x => x > 0).ToList(), listaEquicodiXCentral, regDet.ListaEquicodi, regCentral.EquicodiTVCicloComb, listaGrupoModo
                                           , out List<int> listaDisp1, out List<int> listaIndisp1);

                            AsignarDetalleTermico(listaDispXSem, regDet, listaCorrelaciones, listaDisp1, listaIndisp1, listaManttoXSem);
                        }

                        if (horasIndisp > totalHxSem)
                        {
                            horasIndisp = totalHxSem;
                        }

                    }

                    //si no existe mantto entonces el primer modo está disponible y los demás no
                    if (listaDetXSemSinMantto.Any())
                    {
                        //asignar disp por sddp
                        foreach (var regDet in listaDetXSemSinMantto)
                        {
                            ListarModoDisp(listaSddpXCentral.Select(x => x.Grupocodimodo).Where(x => x > 0).ToList(), listaEquicodiXCentral, new List<int>(), regCentral.EquicodiTVCicloComb, listaGrupoModo
                                           , out List<int> listaDisp1, out List<int> listaIndisp1);

                            AsignarDetalleTermico(listaDispXSem, regDet, listaCorrelaciones, listaDisp1, listaIndisp1, listaManttoXSem);
                        }
                    }

                    //
                    listaDisp.AddRange(listaDispXSem);
                }
            }

            //generar porcentaje de disponibilidad por cada Sddp
            foreach (var regDisp in listaDisp)
            {
                int totalHxMes = ((regDisp.FechaFin - regDisp.FechaIni).Days + 1) * 24;
                decimal horasIndisp = regDisp.HoraIndisp;

                decimal valorDispon = ((totalHxMes - horasIndisp) * 100.0m) / (totalHxMes * 1.0m);

                regDisp.HoraTotal = totalHxMes;
                regDisp.PorcentajeDisp = valorDispon;
            }

            return listaDisp;
        }

        private void AsignarDetalleTermico(List<PmpoHITotal> listaDispXSem, PmpoHIDetalle regDet, List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                        , List<int> listaDisp1, List<int> listaIndisp1, List<EveManttoDTO> listaMantto)
        {
            decimal totalHours = (decimal)(regDet.Horafin - regDet.Horaini).TotalHours;

            //copiar detalle a cada sdpp de la central
            foreach (var regTot in listaDispXSem)
            {
                var regClone = (PmpoHIDetalle)regDet.Clone();
                regClone.ListaCorrelaciones = new List<PmoConfIndispEquipoDTO>();
                regTot.ListaDetXSem.Add(regClone);
            }

            //al primer no involucrado se le considera disponible, los demás están indisponibles al 100
            decimal horasIndisp = 0;
            foreach (var grupocodi in listaDisp1)
            {
                var regTmp = listaDispXSem.Find(x => x.Grupocodimodo == grupocodi);
                var regCorrelTmp = listaCorrelaciones.Find(x => x.Grupocodimodo == grupocodi) ?? new PmoConfIndispEquipoDTO();

                if (regTmp != null)
                {
                    var regDetTmp = regTmp.ListaDetXSem.Find(x => x.Horaini == regDet.Horaini);
                    regDetTmp.HorasTotal = totalHours;
                    regDetTmp.HorasIndisp = horasIndisp;

                    var regClon = (PmoConfIndispEquipoDTO)regCorrelTmp.Clone();
                    regClon.PmCindPorcentaje = 0.0m;
                    regDetTmp.ListaCorrelaciones.Add(regClon);
                    regDetTmp.ListaEquiabrevStr = "";

                    regTmp.HoraIndisp += horasIndisp;
                }
            }

            horasIndisp = totalHours;
            foreach (var grupocodi in listaIndisp1)
            {
                var regTmp = listaDispXSem.Find(x => x.Grupocodimodo == grupocodi);
                var regCorrelTmp = listaCorrelaciones.Find(x => x.Grupocodimodo == grupocodi) ?? new PmoConfIndispEquipoDTO();

                if (regTmp != null)
                {
                    var regDetTmp = regTmp.ListaDetXSem.Find(x => x.Horaini == regDet.Horaini);
                    regDetTmp.HorasTotal = totalHours;
                    regDetTmp.HorasIndisp = horasIndisp;

                    var regClon = (PmoConfIndispEquipoDTO)regCorrelTmp.Clone();
                    var listaManttoXeq = listaMantto.Where(x => regTmp.ListaEquicodi.Contains(x.Equicodi ?? 0) && (x.Evenini <= regDet.Horaini && regDet.Horaini < x.Evenfin)).ToList();
                    if (!listaManttoXeq.Any())
                    { }
                    regClon.ListaEvendescrip = listaManttoXeq.Select(x => GetDescripcionMantto(x, false)).ToList();
                    regClon.PmCindPorcentaje = 100.0m;
                    regClon.EquiAbrev = "";
                    regDetTmp.ListaCorrelaciones.Add(regClon);
                    regDetTmp.ListaEquiabrevStr = (regClon.EquiAbrev ?? "").Trim();

                    regTmp.HoraIndisp += horasIndisp;
                }
            }
        }

        /// <summary>
        /// 
        /// ver funcion private void AsignarValorRFria(
        /// </summary>
        /// <param name="listaSddpXCentral"></param>
        /// <param name="listaAllEquicodi"></param>
        /// <param name="listaEquicodiIndisp"></param>
        private void ListarModoDisp(List<int> listaGrupoXCentral, List<int> listaAllEquicodi, List<int> listaEquicodiIndisp, int equicodiTv, List<PrGrupoDTO> listaGrupoModo
                                    , out List<int> listaGrupocodiModoDisp, out List<int> listaGrupocodiModoIndisp)
        {
            //obtener los modos de operación de la central
            var listaModoOC = listaGrupoModo.Where(x => listaGrupoXCentral.Contains(x.Grupocodi)).ToList();

            List<int> listaAllEquicodi2 = listaAllEquicodi;
            List<int> listaEquicodiIndisp2 = listaEquicodiIndisp;
            List<int> listaEquicodiDisp2 = listaAllEquicodi2.Where(x => !listaEquicodiIndisp2.Contains(x)).ToList();  //modos de operacion que tiene los equipos disponibles no rfria (operativos y mantto)

            //Determinar los modos de operacion 
            var listaGIndisp = new List<int>();
            var listaGDisp = new List<int>();
            listaGDisp = ListarModoSegunListaEquipos(-1, listaAllEquicodi2, new List<int>(), listaEquicodiDisp2, equicodiTv, listaModoOC);
            //listaGrupocodiModoIndisp = ListarModoSegunListaEquipos(-1, listaAllEquicodi2, listaEquicodiIndisp2, listaEquicodiDisp2, listaModoOC);

            listaGrupocodiModoDisp = listaGDisp;
            listaGrupocodiModoIndisp = listaGrupoXCentral.Where(x => !listaGDisp.Contains(x)).ToList();
        }


        /// <summary>
        /// ver funcion public List<IndReporteDetDTO> GenerarIndisponibilidadesParcialesYPrPrevista(
        /// ver funcion private void ObtenerDatosPrToIndReporteDetDTO(decimal? pe, List<PrGrupoDTO> listaGrupoModo, List<int> listaAllEquicodi, List<int> listaEquicodiIndisp, List<int> listaEquicodiDisp, out decimal? prPrevista, out List<PrGrupoDTO> listaMcc)
        /// INDAppServicio
        /// </summary>
        /// <param name="regH"></param>
        /// <param name="regCDespachoXDia"></param>
        private List<int> ListarModoSegunListaEquipos(int fenergcodi, List<int> listaAllEquicodi, List<int> listaEquicodiIndisp, List<int> listaEquicodiDisp, int equicodiTvDisp, List<PrGrupoDTO> listaModoOC)
        {
            //Lista de modos que se restaran a la central
            List<int> listaModoDisp = new List<int>();

            //obtener todos los modos que tiene equipos para mantto, Rfria
            List<PrGrupoDTO> listaModoXUnidad = listaModoOC.Where(x => (x.Fenergcodi == fenergcodi || fenergcodi == -1)
                                                                && x.ListaEquicodi.Any(y => listaAllEquicodi.Contains(y))).OrderByDescending(x => x.Potencia).ToList();

            //if (listaEquicodiIndisp.Any())
            //{
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

                        //si estan en ciclo simple, mantener la tv para que el siguiente modo pueda utilizarlo
                        if (regModo.ListaEquicodi.Count == 2 && regModo.ListaEquicodi.Contains(equicodiTvDisp))
                            listaEquicodiDisp.Add(equicodiTvDisp);
                    }
                }
            }
            //}

            return listaModoDisp.Distinct().ToList();
        }

        #region Cálculo Indisponibilidad

        private void ProcesarListaMantto(int tsddpcodi, List<EveManttoDTO> data, List<EqEquipoDTO> listaEquiposInput, List<PmoSddpCodigoDTO> listaUnidad
                                        , out List<EveManttoDTO> listaFinalOut)
        {

            #region Preparar data

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///Convertir los mantenimientos mensuales o anuales a diarios
            List<EveManttoDTO> resultDiario = new List<EveManttoDTO>();
            int totalSecDia = 60 * 60 * 24;
            foreach (var reg in data)
            {
                int diffInSeconds = (int)((reg.Evenfin.Value - reg.Evenini.Value).TotalSeconds);
                if (diffInSeconds > totalSecDia)
                {
                    for (var day = reg.Evenini.Value.Date; day < reg.Evenfin.Value.AddDays(1); day = day.AddDays(1))
                    {
                        if (day == reg.Evenini.Value.Date)
                        {
                            EveManttoDTO regDia = (EveManttoDTO)reg.Clone();
                            regDia.Evenini = reg.Evenini;
                            regDia.Evenfin = day.AddDays(1);
                            resultDiario.Add(regDia);
                        }
                        if (reg.Evenini.Value.Date < day && day < reg.Evenfin.Value.Date)
                        {
                            EveManttoDTO regDia = (EveManttoDTO)reg.Clone();
                            regDia.Evenini = day;
                            regDia.Evenfin = day.AddDays(1);
                            resultDiario.Add(regDia);
                        }

                        if (day == reg.Evenfin.Value.Date)
                        {
                            EveManttoDTO regDia = (EveManttoDTO)reg.Clone();
                            regDia.Evenini = day;
                            regDia.Evenfin = reg.Evenfin;
                            resultDiario.Add(regDia);
                        }
                    }
                }
                else
                {
                    resultDiario.Add(reg);
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Lista de Centrales y unidades de generación
            List<EqEquipoDTO> listaEqCentral = new List<EqEquipoDTO>();
            List<EqEquipoDTO> listaEqGenerador = listaEquiposInput; //new List<EqEquipoDTO>();

            //if (5 == catecodi)
            //{
            //    listaEqCentral = listaEquiposInput.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoHidraulica).ToList();
            //    listaEqGenerador = listaEquiposInput.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorHidroelectrico).ToList();
            //}
            //else
            //{
            //    listaEqCentral = listaEquiposInput.Where(x => x.Famcodi == ConstantesHorasOperacion.IdTipoTermica).ToList();
            //    listaEqGenerador = listaEquiposInput.Where(x => x.Famcodi == ConstantesHorasOperacion.IdGeneradorTemoelectrico).ToList();
            //}

            List<int> listaFamcodiCentral = ConstantesHorasOperacion.CodFamilias.Split(',').Select(x => int.Parse(x)).ToList();


            //////////////////////////////////////////////////////////////////////////////////
            List<EveManttoDTO> listaManttoEnGeneradores = new List<EveManttoDTO>();

            // CONVERTIR centrales a GENERADORES. las centrales se tienen que desagregar
            if (ConstantesPMPO.TsddpPlantaTermica == tsddpcodi)
            {
                foreach (var regMantto in resultDiario)
                {
                    //si es central solo agregar sus unidades
                    if (listaFamcodiCentral.Contains(regMantto.Famcodi))
                    {
                        if (regMantto.Equicodi <= 0 || regMantto.Equipadre <= 0)
                        { }

                        List<EqEquipoDTO> listaEqXCentral = listaEqGenerador.Where(x => x.Equipadre == regMantto.Equicodi).ToList();

                        foreach (var regUnidad in listaEqXCentral)
                        {
                            EveManttoDTO regManttoUni = (EveManttoDTO)regMantto.Clone();
                            regManttoUni.Equicodi = regUnidad.Equicodi;
                            regManttoUni.Equinomb = regUnidad.Equinomb;
                            regManttoUni.Equiabrev = regUnidad.Equiabrev;
                            regManttoUni.Equipadre = regMantto.Equicodi;
                            regManttoUni.Central = regMantto.Equinomb;
                            regManttoUni.Famcodi = INDAppServicio.GetFamcodiHijo(regMantto.Famcodi);

                            //agregar las unidades de la central
                            listaManttoEnGeneradores.Add(regManttoUni);
                        }
                    }
                    else
                    {
                        //agregar unidad
                        listaManttoEnGeneradores.Add(regMantto);
                    }
                }
            }
            else
            {
                listaManttoEnGeneradores = resultDiario;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            listaManttoEnGeneradores = GetListaEveManttoDivididaXGrupo(listaManttoEnGeneradores);
            listaManttoEnGeneradores = listaManttoEnGeneradores.Where(x => x.EventoGenerado != ConstantesIndisponibilidades.EventoGeneradoFicticio).ToList();

            #endregion

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            List<EveManttoDTO> listaFinal = new List<EveManttoDTO>();
            listaFinal = this.GetListaEveManttoUnificadaXUnidad(listaManttoEnGeneradores, listaEqGenerador, listaUnidad);


            //salidas
            listaFinalOut = listaFinal;
        }

        /// <summary>
        /// Ver: COES.Servicios.Aplicacion.Indisponibilidades / IndisponibilidadesAppServicio / private List<EveManttoDTO> GetListaManttoDividida(List<EveManttoDTO> data)
        /// Dividir todos los Mantto por fechas (fechas de eventos, inicio del dia, fin del dia, hora punta)
        /// Version actualiazada del metodo del corte
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<EveManttoDTO> GetListaEveManttoDivididaXGrupo(List<EveManttoDTO> data)
        {
            List<EveManttoDTO> resultFinalTmp = new List<EveManttoDTO>();

            //recorrer por central
            List<EveManttoDTO> listaManttoXCentral = data.OrderBy(x => x.Evenini).ToList();
            List<int> listaEquicodi = listaManttoXCentral.Select(x => x.Equicodi.Value).Distinct().ToList();

            List<DateTime> listaFecha = listaManttoXCentral.Select(x => x.Evenini.Value.Date).Distinct().OrderBy(x => x).ToList();
            foreach (var fecha in listaFecha)
            {
                List<EveManttoDTO> listaManttoXCentralXDia = listaManttoXCentral.Where(x => x.Evenini.Value.Date == fecha).ToList();

                foreach (var equicodi in listaEquicodi)
                {
                    List<EveManttoDTO> resultXEqXFechaTotal = new List<EveManttoDTO>();

                    // Lista de Fechas por equipo
                    List<DateTime> listaFechaIniXEq = listaManttoXCentralXDia.Select(x => x.Evenini.Value).Distinct().OrderBy(x => x).ToList();
                    List<DateTime> listaFechaFinXEq = listaManttoXCentralXDia.Select(x => x.Evenfin.Value).Distinct().OrderBy(x => x).ToList();

                    List<DateTime> listaFechaXEq = new List<DateTime>();
                    listaFechaXEq.Add(fecha); //inicio del día
                    listaFechaXEq.Add(fecha.AddDays(1)); //fin del día

                    listaFechaXEq.AddRange(listaFechaIniXEq);
                    listaFechaXEq.AddRange(listaFechaFinXEq);
                    listaFechaXEq = listaFechaXEq.Distinct().OrderBy(x => x).ToList();

                    List<EveManttoDTO> dataXEq = listaManttoXCentralXDia.Where(x => x.Equicodi == equicodi && x.Evenini.Value.Date == fecha).ToList(); //se filtra con equicodi, para los generadores (que tienen una lista mezclada de equicodis), las otras veces que se usa esta funciona no hay unidades mezcladas

                    //por cada mantenimiento dividirlo en secciones de fechas horas para que no haya cruces
                    foreach (var reg in dataXEq)
                    {
                        List<EveManttoDTO> listaTmpXEqXFecha = new List<EveManttoDTO>();
                        for (int fi = 0; fi < listaFechaXEq.Count - 1; fi++)
                        {
                            DateTime factual = listaFechaXEq[fi];
                            DateTime fsiguiente = listaFechaXEq[fi + 1];

                            listaTmpXEqXFecha.AddRange(INDAppServicio.GenerarCorteMantto(reg, factual, fsiguiente));
                        }

                        //Agregar valores del mantto
                        List<EveManttoDTO> resultXEqXFecha = new List<EveManttoDTO>();
                        foreach (var rtmp in listaTmpXEqXFecha)
                        {
                            var regCorte = (EveManttoDTO)reg.Clone();
                            regCorte.Evenini = rtmp.Evenini;
                            regCorte.Evenfin = rtmp.Evenfin;
                            regCorte.EventoGenerado = rtmp.EventoGenerado;

                            resultXEqXFecha.Add(regCorte);
                        }

                        resultXEqXFechaTotal.AddRange(resultXEqXFecha);
                    }
                    resultXEqXFechaTotal = resultXEqXFechaTotal.OrderBy(x => x.Manttocodi).ThenBy(x => x.Evenini).ToList();
                    resultFinalTmp.AddRange(resultXEqXFechaTotal);
                }
            }

            return resultFinalTmp;
        }

        /// <summary>
        /// Guarda detalle en la base datos 
        /// </summary>
        /// <param name="listaIndisp"></param>
        /// <returns></returns>
        public List<PmpoHIDetalle> GenerarDetalleCuadro(List<EveManttoDTO> listaIndisp)
        {
            List<PmpoHIDetalle> listaReporteDet = new List<PmpoHIDetalle>();

            List<PmoSddpCodigoDTO> listaUnidad = listaIndisp.DistinctBy(x => new { x.ListaEquicodiStr })
                                        .Select(x => new PmoSddpCodigoDTO() { ListaEquicodi = x.ListaEquicodi }).ToList();

            List<DateTime> listaFecha = listaIndisp.Select(x => x.Evenini.Value.Date).Distinct().OrderBy(x => x).ToList();

            foreach (var fecha in listaFecha)
            {
                var listaXFecha = listaIndisp.Where(x => x.Evenini.Value.Date == fecha).ToList();
                foreach (var regUnidad in listaUnidad)
                {
                    List<EveManttoDTO> listaEjecXEqTmp = new List<EveManttoDTO>(), listaProgXEqTmp = new List<EveManttoDTO>(), listaFortXEqTmp = new List<EveManttoDTO>();

                    // Lista de Fechas por equipo
                    List<EveManttoDTO> dataXEq = listaXFecha.Where(x => x.ListaEquicodiStr == string.Join(",", regUnidad.ListaEquicodi.OrderBy(y => y))).ToList();
                    List<DateTime> listaFechaXEq = dataXEq.Select(x => x.Evenini.Value).Distinct().OrderBy(x => x).ToList();

                    for (int fi = 0; fi < listaFechaXEq.Count; fi++)
                    {
                        DateTime fechaPrueba = new DateTime(2020, 5, 27);
                        if (fecha.Date == fechaPrueba.Date)
                        { }

                        var prog = dataXEq.Find(y => y.Evenini == listaFechaXEq[fi]);

                        //Caso estandar
                        if (prog != null)
                            listaProgXEqTmp.Add(prog);
                    }

                    //Unir registros contiguos
                    List<EveManttoDTO> listaProgXEq = UnificarEveManttoXListaEquicodi(listaProgXEqTmp);

                    //Guardar las Indisponibilidades programados
                    foreach (var regEve in listaProgXEq)
                    {
                        int min = Convert.ToInt32((regEve.Evenfin.Value - regEve.Evenini.Value).TotalMinutes);

                        listaReporteDet.Add(new PmpoHIDetalle()
                        {
                            Sddpcodi = regUnidad.Sddpcodi,
                            ListaEquicodi = regEve.ListaEquicodi,
                            ListaEquicodiStr = string.Join(",", regEve.ListaEquicodi.OrderBy(x => x)),
                            Horaini = regEve.Evenini.Value,
                            Horafin = regEve.Evenfin.Value,
                            Min = min,
                        });
                    }
                }
            }

            return listaReporteDet;
        }

        /// <summary>
        /// Lista Eve. Mantto unificada por unidad
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaEqCentral"></param>
        /// <param name="listaEqGenerador"></param>
        /// <param name="listaUnidad"></param>
        /// <returns></returns>
        private List<EveManttoDTO> GetListaEveManttoUnificadaXUnidad(List<EveManttoDTO> data, List<EqEquipoDTO> listaEqGenerador
                                                                    , List<PmoSddpCodigoDTO> listaUnidad)
        {
            List<EveManttoDTO> listaAgrupadoXCentral = new List<EveManttoDTO>();

            //si en un rango existe todas las unidades de la central, esos n objetos se reemplazan por uno
            foreach (var regUnidad in listaUnidad)
            {
                if (regUnidad.Equicodi == 13656)
                { }
                if (regUnidad.Grupocodi == 292)
                { }

                List<EqEquipoDTO> listaEqGenXUnidad = listaEqGenerador.Where(x => regUnidad.ListaEquicodi.Contains(x.Equicodi)).ToList();

                List<int> listaEquicodiValido = listaEqGenXUnidad.Select(x => x.Equicodi).ToList();

                List<int> listaEquicodiSinTV = regUnidad.EquicodiTVCicloComb > 0 ? listaEquicodiValido.Where(x => x != regUnidad.EquicodiTVCicloComb).ToList() : new List<int>();

                //mantenimientos de la unidad para indisponibilidad total
                List<EveManttoDTO> listaManttoXUnidad = data.Where(x => listaEquicodiValido.Contains(x.Equicodi.Value)).OrderBy(x => x.Evenini).ToList();

                //
                List<DateTime> listaFecha = listaManttoXUnidad.Select(x => x.Evenini.Value.Date).Distinct().OrderBy(x => x).ToList();

                List<EveManttoDTO> listaXrangoAnterior = new List<EveManttoDTO>(); //saber las unidades parciales del rango anterior (tambien del día anterior)
                DateTime? fechafinrangoAnterior = null; //saber la fecha en que terminó el último rango
                foreach (var day in listaFecha)
                {
                    List<EveManttoDTO> listaByDia = listaManttoXUnidad.Where(x => x.Evenini.Value.Date == day).ToList();

                    // Lista de Fechas por equipo
                    List<DateTime> listaFechaIniXEq = listaByDia.Select(x => x.Evenini.Value).Distinct().OrderBy(x => x).ToList();
                    List<DateTime> listaFechaFinXEq = listaByDia.Select(x => x.Evenfin.Value).Distinct().OrderBy(x => x).ToList();

                    List<DateTime> listaFechaXEq = new List<DateTime>();
                    listaFechaXEq.Add(day); //inicio del día
                    listaFechaXEq.Add(day.AddDays(1)); //fin del día

                    listaFechaXEq.AddRange(listaFechaIniXEq);
                    listaFechaXEq.AddRange(listaFechaFinXEq);
                    listaFechaXEq = listaFechaXEq.Distinct().OrderBy(x => x).ToList();

                    //dividir manttos
                    List<EveManttoDTO> listaByDiaDividido = new List<EveManttoDTO>();
                    foreach (var reg in listaByDia)
                    {
                        if (reg.Manttocodi == 2219358)
                        { }
                        List<EveManttoDTO> resultXEqXFecha = new List<EveManttoDTO>();
                        for (int fi = 0; fi < listaFechaXEq.Count - 1; fi++)
                        {
                            DateTime factual = listaFechaXEq[fi];
                            DateTime fsiguiente = listaFechaXEq[fi + 1];

                            resultXEqXFecha.AddRange(INDAppServicio.GenerarCorteMantto(reg, factual, fsiguiente).Where(x => x.EventoGenerado != ConstantesIndisponibilidades.EventoGeneradoFicticio));
                        }

                        listaByDiaDividido.AddRange(resultXEqXFecha);
                    }

                    for (int fi = 0; fi < listaFechaXEq.Count - 1; fi++)
                    {
                        DateTime factual = listaFechaXEq[fi];
                        DateTime fsiguiente = listaFechaXEq[fi + 1];

                        List<EveManttoDTO> listaAllXrango = listaByDiaDividido.Where(x => x.Evenini >= factual && x.Evenfin <= fsiguiente).ToList();

                        //si las fechas no son continuas, reiniciar el rango anterior. Verificar el rango anterior solo aplica a las centrales con ciclo combinados
                        if (fechafinrangoAnterior != factual || regUnidad.EquicodiTVCicloComb <= 0) { listaXrangoAnterior = new List<EveManttoDTO>(); fechafinrangoAnterior = null; }

                        //obtener unidades parciales, si ya existen del rango anterior hay que continuar con ellas
                        List<EveManttoDTO> listaXrango = GenerarListaUnidadesManttoParcial(listaAllXrango, ref listaXrangoAnterior);

                        bool tieneIndispParcial = false;

                        #region Verificar si existe alguna posible indisponibilidad PARCIAL, si pasa con alguna unidad ya no se puede determinar que sea TOTAL

                        if (listaXrango.Any())
                        {
                            foreach (var regEjecProg in listaXrango)
                            {
                                List<int> listaEquicodiXRango = regEjecProg.ListaEquicodi;
                                List<int> listaEquicodiXRangoSinTV = regUnidad.EquicodiTVCicloComb > 0 ? listaEquicodiXRango.Where(x => x != regUnidad.EquicodiTVCicloComb).ToList() : new List<int>();

                                //bool existePr = listaXrango.Find(x => x.Evenpr > 0) != null;
                                bool existeIndispParcial = listaAllXrango.Find(x => regEjecProg.ListaEquicodi.Contains(x.Equicodi.Value)) != null;
                                bool existeIndisTodoUnidad = regUnidad.ListaEquicodi.Count == listaEquicodiXRango.Count;

                                bool tieneIndipCentral = regUnidad.EquicodiTVCicloComb > 0 ? listaEquicodiSinTV.Count == listaEquicodiXRangoSinTV.Count : false;

                                if ((!tieneIndipCentral && !existeIndisTodoUnidad) || existeIndispParcial)
                                {
                                    regEjecProg.Emprcodi = regUnidad.Emprcodi ?? 0;
                                    regEjecProg.Evenini = factual;
                                    regEjecProg.Evenfin = fsiguiente;
                                    regEjecProg.ListaEquicodiStr = string.Join(",", regEjecProg.ListaEquicodi.OrderBy(x => x));

                                    tieneIndispParcial = true;
                                }
                            }
                        }
                        else
                        { }

                        listaXrangoAnterior = listaXrango;
                        fechafinrangoAnterior = fsiguiente;

                        #endregion

                        if (listaAllXrango.Any())
                        {
                            foreach (var regEjecProg in listaXrango)
                            {
                                List<int> listaEquicodiXRango = regEjecProg.ListaEquicodi;

                                if (regUnidad.TieneCicloComb)
                                {
                                    if (listaEquicodiSinTV.All(x => listaEquicodiXRango.Contains(x)))
                                    {
                                        regEjecProg.Emprcodi = regUnidad.Emprcodi ?? 0;
                                        regEjecProg.Equicodi = regUnidad.Equicodi;
                                        regEjecProg.Evenini = factual;
                                        regEjecProg.Evenfin = fsiguiente;
                                        regEjecProg.Equiabrev = "CENTRAL";
                                        listaAgrupadoXCentral.Add(regEjecProg);
                                    }
                                }
                                else
                                {
                                    regEjecProg.Emprcodi = regUnidad.Emprcodi ?? 0;
                                    regEjecProg.Equicodi = regUnidad.Equicodi;
                                    regEjecProg.Evenini = factual;
                                    regEjecProg.Evenfin = fsiguiente;
                                    listaAgrupadoXCentral.Add(regEjecProg);
                                }
                            }
                        }
                    }
                }
            }

            List<EveManttoDTO> listaIndNuevo = UnificarEveManttoXListaEquicodi(listaAgrupadoXCentral);

            return listaIndNuevo;
        }

        /// <summary>
        /// Lista de unidades, quita los programados
        /// </summary>
        /// <param name="listaXrango"></param>
        /// <param name="listaXrangoAnterior"></param>
        /// <returns></returns>
        public List<EveManttoDTO> GenerarListaUnidadesManttoParcial(List<EveManttoDTO> listaXrango, ref List<EveManttoDTO> listaXrangoAnterior)
        {
            List<EveManttoDTO> listaAllXrango = new List<EveManttoDTO>();
            listaAllXrango.AddRange(listaXrango);

            List<EveManttoDTO> lista1 = new List<EveManttoDTO>();

            //verificar que la lista del rango anterior tenga Indisponibilidades parciales prog y fort, si tiene ambos continuan, caso contrario se reinicia la lista
            bool tieneParcialProg = listaXrangoAnterior.Find(x => x.TieneAmbosTipoMantto) != null;
            //if (tieneParcialProg)
            //{
            //    listaXrangoAnterior = new List<EveManttoDTO>();
            //}

            //primero obtener la lista de unidades parciales que se ejecutaron en el anterior rango
            //cuando se llama por primera vez a este metodo, no se ejecuta este foreach
            foreach (var regAnt in listaXrangoAnterior)
            {
                //equipos que tienen mantenimientos tambien en el rango actual
                int numEquipos = 0;
                var l2 = new List<EveManttoDTO>();

                foreach (int equicodi in regAnt.ListaEquicodi)
                {
                    var lTmp1 = listaXrango.Where(x => x.Equicodi == equicodi && x.Evenclasecodi == regAnt.Evenclasecodi).ToList();

                    if (lTmp1.Any())
                    {
                        numEquipos++;
                        l2.AddRange(lTmp1);
                    }
                }

                if (numEquipos == regAnt.ListaEquicodi.Count)
                {
                    var listaTmpP = GenerarListaUnidadesManttoParcialXLista(l2);
                    lista1.AddRange(listaTmpP);

                    foreach (var regt in l2) listaAllXrango.Remove(regt);
                }
            }

            //obtener unidades parciales con los manttos restantes
            var lista2 = GenerarListaUnidadesManttoParcialXLista(listaAllXrango);

            List<EveManttoDTO> lFinal = new List<EveManttoDTO>();
            lFinal.AddRange(lista1);
            lFinal.AddRange(lista2);

            return lFinal;
        }

        /// <summary>
        /// obtener las unidades  parciales
        /// </summary>
        /// <param name="listaXrango"></param>
        /// <param name="listaXrangoAnterior"></param>
        /// <returns></returns>
        public List<EveManttoDTO> GenerarListaUnidadesManttoParcialXLista(List<EveManttoDTO> listaXrango)
        {
            List<EveManttoDTO> lFinal = new List<EveManttoDTO>();

            //agrupar
            List<EveManttoDTO> listaEqxMantto = listaXrango.GroupBy(x => new { x.Equicodi })
                                                        .Select(x => new EveManttoDTO() { Equicodi = x.Key.Equicodi }).ToList();

            List<int> listaEquicodi = listaEqxMantto.Select(x => x.Equicodi.Value).Distinct().ToList();

            List<int> lAmbos = new List<int>();
            List<int> lSoloProg = new List<int>();

            foreach (int equicodi in listaEquicodi)
            {
                var eqProg = listaEqxMantto.Find(x => x.Equicodi == equicodi);

                if (eqProg != null)
                {
                    lSoloProg.Add(equicodi);
                }
            }

            //generar objetos
            if (lSoloProg.Any())
            {
                EveManttoDTO prog = new EveManttoDTO() { ListaEquicodi = lSoloProg };
                lFinal.Add(prog);
            }

            return lFinal;
        }

        /// <summary>
        /// Permite unificar Eve. Manttos por modo de operación
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<EveManttoDTO> UnificarEveManttoXListaEquicodi(List<EveManttoDTO> data)
        {
            data = data.OrderBy(x => x.Grupocodi).ThenBy(x => x.ListaEquicodiStr).ThenBy(x => x.Evenini).ThenBy(x => x.Evenfin).ToList();

            List<EveManttoDTO> listaIndNuevo = new List<EveManttoDTO>();
            EveManttoDTO indNuevo;

            //Unificar por rango
            for (int i = 0; i < data.Count; i++)
            {
                //indActual = data[i];

                indNuevo = (EveManttoDTO)data[i].Clone();
                listaIndNuevo.Add(indNuevo);

                //buscar por interseccion
                bool terminoBusqueda = false;
                for (int j = i + 1; j < data.Count && !terminoBusqueda; j++)
                {
                    if (data[j].ListaEquicodiStr == indNuevo.ListaEquicodiStr
                        && data[j].Grupocodi == indNuevo.Grupocodi
                        && indNuevo.Evenfin == data[j].Evenini   //el siguiente registro, su inicio debe ser el fin del rango actual
                        && data[j].Evenini < indNuevo.Evenini.Value.Date.AddDays(1) //solo rangos que son del mismo día
                        )
                    {
                        indNuevo.Evenfin = data[j].Evenfin.Value;
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

        /// <summary>
        /// Listar manttos 
        /// </summary>
        /// <param name="fecha1Mes"></param>
        /// <param name="fechaFinAnual"></param>
        /// <param name="tipoReporteMantto"></param>
        /// <param name="particionar"></param>
        /// <returns></returns>
        private List<EveManttoDTO> ListarMantto(DateTime fecha1Mes, DateTime fechaFinAnual, string tipoReporteMantto, bool particionar)
        {
            DateTime fIniMensual = new DateTime(fecha1Mes.Year - 1, 12, 1); //desde diciembre del año anterior
            DateTime fFinMensual = fecha1Mes.AddMonths(1);

            string tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            string empresas = ConstantesAppServicio.ParametroDefecto;
            string idsTipoEquipo = ConstantesAppServicio.ParametroDefecto;
            string idstipoMantto = ConstantesAppServicio.ParametroDefecto;
            string indInterrupcion = ConstantesAppServicio.ParametroDefecto;
            string indispo = "F";

            List<EveManttoDTO> listaFinal = new List<EveManttoDTO>();

            //4: mensual
            List<EveManttoDTO> listaMensual = FactorySic.GetEveManttoRepository().ObtenerReporteMantenimientos("4", fIniMensual, fFinMensual,
                            indispo, tiposEmpresa, empresas, idsTipoEquipo, indInterrupcion, idstipoMantto);

            List<EveManttoDTO> listaAnual = FactorySic.GetEveManttoRepository().ObtenerReporteMantenimientos("5", fFinMensual, fechaFinAnual.AddDays(1),
                            indispo, tiposEmpresa, empresas, idsTipoEquipo, indInterrupcion, idstipoMantto);

            listaFinal.AddRange(listaMensual);
            listaFinal.AddRange(listaAnual);

            //filtrar los manttos
            if (tipoReporteMantto != "-1")
            {
                List<int> listaFamcodi = ListarFamcodiFiltroMantto(tipoReporteMantto);
                listaFinal = listaFinal.Where(x => listaFamcodi.Contains(x.Famcodi)).ToList();
            }

            if (particionar) listaFinal = ObtenerManttosPartidasPorDias(listaFinal);

            //listaManttos = listaManttos.Where(x => x.Emprcodi == 12634).ToList(); //INLAND ENERGY SAC //C.H. SANTA TERESA
            //listaManttos = listaManttos.Where(x => x.Emprcodi == 11258).ToList(); //HIDROELECTRICA HUANCHOR S.A.C.
            //listaManttos = listaManttos.Where(x => x.Emprcodi == 48).ToList(); //ENGIE
            //listaManttos = listaManttos.Where(x => x.Evenini>= new DateTime(2022,1,29) && x.Evenfin<= new DateTime(2022, 2, 4)).ToList();
            //listaFinal = listaFinal.Where(x => x.Emprcodi == 17).ToList(); //Egasa

            return listaFinal;
        }

        /// <summary>
        /// Obtiene las intervenciones partidas por dia
        /// Ver función private List<InIntervencionDTO> ObtenerIntervencionesPartidasPorDias(List<InIntervencionDTO> listaIntervenciones) de IntervencionesAppServicio
        /// </summary>
        /// <param name="listaMantto"></param>
        /// <returns></returns>
        private List<EveManttoDTO> ObtenerManttosPartidasPorDias(List<EveManttoDTO> listaMantto)
        {
            List<EveManttoDTO> listaManttoDia = new List<EveManttoDTO>();

            foreach (var mantto in listaMantto)
            {
                var intervaloFechaXDia = Aplicacion.Helper.Util.SplitDateRange(mantto.Evenini.Value, mantto.Evenfin.Value, 1);

                bool esFraccionado = intervaloFechaXDia.Count() > 1;
                foreach (var intervalFecha in intervaloFechaXDia)
                {
                    var intervencionClone = (EveManttoDTO)mantto.Clone();
                    intervencionClone.Evenini = intervalFecha.Item1;
                    intervencionClone.Evenfin = intervalFecha.Item2;

                    listaManttoDia.Add(intervencionClone);
                }
            }

            return listaManttoDia;
        }

        /// <summary>
        /// Obtener los rangos de tiempo sin mantto
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaDetXSem"></param>
        /// <returns></returns>
        private List<PmpoHIDetalle> ListarRangoSinMantto(DateTime fechaIni, DateTime fechaFin, List<PmpoHIDetalle> listaDetXSem)
        {
            List<PmpoHIDetalle> l = new List<PmpoHIDetalle>();

            for (var day = fechaIni; day <= fechaFin; day = day.AddDays(1))
            {
                var listaXDia = listaDetXSem.Where(x => x.Horaini.Date == day).ToList();

                List<PmpoHIDetalle> lTmpVacioXDia = new List<PmpoHIDetalle>();
                if (listaXDia.Any())
                {
                    PmpoHIDetalle regAnterior, regActual, regSiguiente;
                    for (int i = 0; i < listaXDia.Count; i++)
                    {
                        regAnterior = i > 0 ? listaXDia[i - 1] : null;
                        regActual = listaXDia[i];
                        regSiguiente = i < listaXDia.Count - 1 ? listaXDia[i + 1] : null;

                        if (!(regActual.Horaini == day && regActual.Horafin == day.AddDays(1)))
                        {
                            //caso 3: el primer rango no inicia a las 00:00
                            if (regAnterior == null && regActual.Horaini != day)
                            {
                                lTmpVacioXDia.Add(new PmpoHIDetalle()
                                {
                                    Horaini = day,
                                    Horafin = regActual.Horaini
                                });
                            }

                            //
                            if (regSiguiente == null && regActual.Horafin != day.AddDays(1))
                            {
                                //caso 4: el ultimo rango no termina es a las 24:00
                                lTmpVacioXDia.Add(new PmpoHIDetalle()
                                {
                                    Horaini = regActual.Horafin,
                                    Horafin = day.AddDays(1)
                                });
                            }

                            //
                            if (regActual != null && regSiguiente != null)
                            {
                                if ((regSiguiente.Horafin - regActual.Horaini).TotalMinutes > 0)
                                {
                                    lTmpVacioXDia.Add(new PmpoHIDetalle()
                                    {
                                        Horaini = regActual.Horafin,
                                        Horafin = regSiguiente.Horaini
                                    });
                                }
                            }
                            if (regAnterior != null && regActual != null)
                            {
                                if ((regActual.Horaini - regAnterior.Horafin).TotalMinutes > 0)
                                {
                                    lTmpVacioXDia.Add(new PmpoHIDetalle()
                                    {
                                        Horaini = regAnterior.Horafin,
                                        Horafin = regActual.Horaini
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    //si no existe ningun mantto entonces se crea registro para todo el dia
                    lTmpVacioXDia.Add(new PmpoHIDetalle()
                    {
                        Horaini = day,
                        Horafin = day.AddDays(1)
                    });
                }

                //obtener elementos unicos
                lTmpVacioXDia = lTmpVacioXDia.GroupBy(x => new { x.Horaini, x.Horafin }).Select(x => new PmpoHIDetalle() { Horaini = x.Key.Horaini, Horafin = x.Key.Horafin }).ToList();

                l.AddRange(lTmpVacioXDia);
            }

            return l;
        }

        #endregion

        #region Excel web Disponibilidad

        public void ArmarHandsonDisponibilidad(string tipo, int pericodi, out string[][] data, out object[] columnas, out string[][] descripcionHandson)
        {
            List<PmoDatPmhiTrDTO> lista = ListDatPmhiTr(pericodi, tipo);
            int tsddpcodi = GetTsddpcodiXCadena(tipo);

            //Actualizamos las fechas de mantenimiento anual
            PmoPeriodoDTO periodoAct = servPmpo.GetByIdPmoPeriodo(pericodi);

            //Obtenemos los nombres de Grupo (Centrales), Configuradas en la tabla PMO_CONF_INDISP_EQUIPO (Correlaciones)
            ListarEntidadesCalculoDisp(tsddpcodi, out List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                        , out List<PmoSddpCodigoDTO> listaSddp, out List<EqEquipoDTO> listaEq, out List<EqEquipoDTO> listaCentralComb, out List<PrGrupoDTO> listaGrupoModo);

            if (tipo == "HM" || tipo == "TM")
            {
                //lista Anio
                List<int> listaAnio = lista.GroupBy(x => x.PmPmhtAnhio).Select(x => x.Key).OrderBy(x => x).ToList();

                List<PmoMesDTO> listaAllAnio = new List<PmoMesDTO>();
                foreach (var anio in listaAnio)
                {
                    List<PmoMesDTO> listaXAnio = servPmpo.ListarSemanaMesDeAnho(anio, ConstantesPMPO.AccionEditar, null);
                    listaAllAnio.AddRange(listaXAnio);
                }

                ArmarGrillaMensual(lista, listaSddp, listaEq, listaAllAnio, out data, out columnas, out descripcionHandson);
            }
            else
            {
                //lista Anio
                List<int> listaAnio = lista.GroupBy(x => x.PmPmhtAnhio).Select(x => x.Key).OrderBy(x => x).ToList();

                List<PmpoSemana> listaAllAnio = new List<PmpoSemana>();
                foreach (var anio in listaAnio)
                {
                    List<PmpoSemana> listaXAnio = servPmpo.ListarSemanaXAnio(anio, out List<string> listMsjVal);
                    listaAllAnio.AddRange(listaXAnio);
                }

                ArmarGrilla(lista, listaSddp, listaEq, listaAllAnio, out data, out columnas, out descripcionHandson);
            }
        }

        public void ArmarHandsonImportarDisponibilidad(List<PmoDatPmhiTrDTO> lista, out string[][] data, out object[] columnas, out string[][] descripcionHandson)
        {
            List<PmoSddpCodigoDTO> listaGrupo = lista.GroupBy(x => x.Sddpcodi).Select(x => new PmoSddpCodigoDTO()
            {
                Sddpcodi = x.Key,
                Sddpnomb = x.First().Sddpnomb,
            }).ToList();

            ArmarGrilla(lista, listaGrupo, new List<EqEquipoDTO>(), new List<PmpoSemana>(), out data, out columnas, out descripcionHandson);
        }

        /// <summary>
        /// Obtener descripción de Mantto
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        private string GetDescripcionMantto(EveManttoDTO reg, bool mostrarTodoDetalle = true)
        {
            var fuenteDato = reg.Evencodi > 0 ? ">> EVENTO \n" : (reg.Iccodi > 0 ? ">>RESTRICCIÓN OPERATIVA \n" : "");
            var strIniHora = reg.Evenini.Value.ToString(ConstantesAppServicio.FormatoHora);
            var strFinHora = reg.Evenfin.Value.ToString(ConstantesAppServicio.FormatoHora) != "00:00" ? reg.Evenfin.Value.ToString(ConstantesAppServicio.FormatoHora) : "23:59";
            var indispoDesc = reg.Evenindispo == ConstantesIndisponibilidades.ES ? " - " + "E/S" : (reg.Evenindispo == ConstantesIndisponibilidades.FS ? " - " + "F/S" : string.Empty);
            var tipoevenabrev = !string.IsNullOrEmpty(reg.Tipoevenabrev) ? " - " + reg.Tipoevenabrev.Trim() : string.Empty;
            var equinomb = " - (" + (reg.Areanomb ?? "").Trim() + " " + (!string.IsNullOrEmpty(reg.Equiabrev) ? reg.Equiabrev.Trim() : string.Empty) + ")";

            if (mostrarTodoDetalle) strIniHora = reg.Evenini.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
            if (mostrarTodoDetalle) strFinHora = reg.Evenfin.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
            if (mostrarTodoDetalle) equinomb = " - " + reg.Emprnomb + " " + reg.Areadesc + " " + equinomb;

            var resultado = fuenteDato
                    + strIniHora + " - " + strFinHora + " - " + reg.Evenclaseabrev + " " + indispoDesc + " " + tipoevenabrev + equinomb
                    + (mostrarTodoDetalle ? "\n" + reg.Evendescrip : " " + reg.Evendescrip);

            return resultado;
        }

        public void ArmarGrilla(List<PmoDatPmhiTrDTO> listaData, List<PmoSddpCodigoDTO> listaGrupo, List<EqEquipoDTO> listaEq, List<PmpoSemana> listaSemAnio
                                   , out string[][] dataHandson, out object[] columnasHandson, out string[][] descripcionHandson)
        {
            string[] Cabecera = { "", "", "", "", "", "Semana" };
            string[] Cabecera2 = { "PMPMHTCODI","GRUPOCODI","Codigo SDDP", "Grupo", "Año",
                                    "1","2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12","13","14","15",
                                    "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                                    "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45",
                                    "46", "47", "48", "49", "50", "51", "52" };

            int maxCol = 5 + 52;

            Array.Resize(ref Cabecera, Cabecera.Length + maxCol);

            #region columnas

            object[] columnas = new object[maxCol];
            columnas[0] = new
            {   //Codigo sddp
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[1] = new
            {   //grupo
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[2] = new
            {   //año
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[3] = new
            {   //año
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[4] = new
            {   //año
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            for (var x = 5; x < columnas.Count(); x++)
            {
                columnas[x] = new
                {   //año
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = false,
                };
            }

            #endregion

            string[][] data = new string[listaData.Count + 2][];

            List<string[]> listaDescripcionHandson = new List<string[]>();
            listaDescripcionHandson.Add(new string[maxCol]);
            listaDescripcionHandson.Add(new string[maxCol]);

            data[0] = Cabecera;
            data[1] = Cabecera2;
            int index = 2;

            foreach (var regGr in listaGrupo)
            {
                var listaDataXGr = listaData.Where(x => x.Sddpcodi == regGr.Sddpcodi).ToList();

                //obtener los equipos relacionados al grupo
                List<EqEquipoDTO> listaEqxGr = listaEq.Where(x => x.Grupocodi == regGr.Grupocodi).ToList();
                List<int> lEquicodi = listaEqxGr.Select(x => x.Equicodi).ToList();

                foreach (var item in listaDataXGr)
                {
                    string[] itemDato = new string[57];
                    #region mapeo
                    itemDato[0] = item.PmPmhtCodi.ToString();
                    itemDato[1] = item.Sddpcodi.ToString();
                    itemDato[2] = item.Sddpnum.ToString();
                    itemDato[3] = item.Sddpnomb.ToString();
                    itemDato[4] = item.PmPmhtAnhio.ToString();
                    itemDato[5] = item.PmPmhtDisp01.ToString();
                    itemDato[6] = item.PmPmhtDisp02.ToString();
                    itemDato[7] = item.PmPmhtDisp03.ToString();
                    itemDato[8] = item.PmPmhtDisp04.ToString();
                    itemDato[9] = item.PmPmhtDisp05.ToString();
                    itemDato[10] = item.PmPmhtDisp06.ToString();
                    itemDato[11] = item.PmPmhtDisp07.ToString();
                    itemDato[12] = item.PmPmhtDisp08.ToString();
                    itemDato[13] = item.PmPmhtDisp09.ToString();
                    itemDato[14] = item.PmPmhtDisp10.ToString();
                    itemDato[15] = item.PmPmhtDisp11.ToString();
                    itemDato[16] = item.PmPmhtDisp12.ToString();
                    itemDato[17] = item.PmPmhtDisp13.ToString();
                    itemDato[18] = item.PmPmhtDisp14.ToString();
                    itemDato[19] = item.PmPmhtDisp15.ToString();
                    itemDato[20] = item.PmPmhtDisp16.ToString();
                    itemDato[21] = item.PmPmhtDisp17.ToString();
                    itemDato[22] = item.PmPmhtDisp18.ToString();
                    itemDato[23] = item.PmPmhtDisp19.ToString();
                    itemDato[24] = item.PmPmhtDisp20.ToString();
                    itemDato[25] = item.PmPmhtDisp21.ToString();
                    itemDato[26] = item.PmPmhtDisp22.ToString();
                    itemDato[27] = item.PmPmhtDisp23.ToString();
                    itemDato[28] = item.PmPmhtDisp24.ToString();
                    itemDato[29] = item.PmPmhtDisp25.ToString();
                    itemDato[30] = item.PmPmhtDisp26.ToString();
                    itemDato[31] = item.PmPmhtDisp27.ToString();
                    itemDato[32] = item.PmPmhtDisp28.ToString();
                    itemDato[33] = item.PmPmhtDisp29.ToString();
                    itemDato[34] = item.PmPmhtDisp30.ToString();
                    itemDato[35] = item.PmPmhtDisp31.ToString();
                    itemDato[36] = item.PmPmhtDisp32.ToString();
                    itemDato[37] = item.PmPmhtDisp33.ToString();
                    itemDato[38] = item.PmPmhtDisp34.ToString();
                    itemDato[39] = item.PmPmhtDisp35.ToString();
                    itemDato[40] = item.PmPmhtDisp36.ToString();
                    itemDato[41] = item.PmPmhtDisp37.ToString();
                    itemDato[42] = item.PmPmhtDisp38.ToString();
                    itemDato[43] = item.PmPmhtDisp39.ToString();
                    itemDato[44] = item.PmPmhtDisp40.ToString();
                    itemDato[45] = item.PmPmhtDisp41.ToString();
                    itemDato[46] = item.PmPmhtDisp42.ToString();
                    itemDato[47] = item.PmPmhtDisp43.ToString();
                    itemDato[48] = item.PmPmhtDisp44.ToString();
                    itemDato[49] = item.PmPmhtDisp45.ToString();
                    itemDato[50] = item.PmPmhtDisp46.ToString();
                    itemDato[51] = item.PmPmhtDisp47.ToString();
                    itemDato[52] = item.PmPmhtDisp48.ToString();
                    itemDato[53] = item.PmPmhtDisp49.ToString();
                    itemDato[54] = item.PmPmhtDisp50.ToString();
                    itemDato[55] = item.PmPmhtDisp51.ToString();
                    itemDato[56] = item.PmPmhtDisp52.ToString();
                    #endregion
                    data[index] = itemDato;

                    //title
                    string[] matrizDesc = new string[57];

                    var listaSemXAnio = listaSemAnio.Where(x => x.Anio == item.PmPmhtAnhio).OrderBy(x => x.NroSemana).ToList();
                    int i = 5;
                    foreach (var regSem in listaSemXAnio)
                    {
                        matrizDesc[i] = "";
                        i++;
                    }
                    listaDescripcionHandson.Add(matrizDesc);

                    index++;
                }
            }

            dataHandson = data;
            columnasHandson = columnas;
            descripcionHandson = listaDescripcionHandson.ToArray();

        }

        public void ArmarGrillaMensual(List<PmoDatPmhiTrDTO> listaData, List<PmoSddpCodigoDTO> listaGrupo, List<EqEquipoDTO> listaEq, List<PmoMesDTO> listaMesAnio
                                   , out string[][] dataHandson, out object[] columnasHandson, out string[][] descripcionHandson)
        {
            string[] Cabecera = { "", "", "", "", "", "Mes" };
            string[] Cabecera2 = { "PMPMHTCODI","GRUPOCODI","Codigo SDDP", "Grupo", "Año",
                                    "1","2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"};

            int maxCol = 5 + 12;

            Array.Resize(ref Cabecera, Cabecera.Length + maxCol);

            #region columnas

            object[] columnas = new object[maxCol];
            columnas[0] = new
            {   //Codigo sddp
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[1] = new
            {   //grupo
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[2] = new
            {   //año
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[3] = new
            {   //año
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            columnas[4] = new
            {   //año
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = true,
            };
            for (var x = 5; x < columnas.Count(); x++)
            {
                columnas[x] = new
                {   //año
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = false,
                };
            }

            #endregion

            string[][] data = new string[listaData.Count + 2][];

            List<string[]> listaDescripcionHandson = new List<string[]>();
            listaDescripcionHandson.Add(new string[maxCol]);
            listaDescripcionHandson.Add(new string[maxCol]);

            data[0] = Cabecera;
            data[1] = Cabecera2;
            int index = 2;

            foreach (var regGr in listaGrupo)
            {
                var listaDataXGr = listaData.Where(x => x.Sddpcodi == regGr.Sddpcodi).ToList();

                //obtener los equipos relacionados al grupo
                List<EqEquipoDTO> listaEqxGr = listaEq.Where(x => x.Grupocodi == regGr.Grupocodi).ToList();
                List<int> lEquicodi = listaEqxGr.Select(x => x.Equicodi).ToList();

                foreach (var item in listaDataXGr)
                {
                    string[] itemDato = new string[maxCol];
                    #region mapeo
                    itemDato[0] = item.PmPmhtCodi.ToString();
                    itemDato[1] = item.Sddpcodi.ToString();
                    itemDato[2] = item.Sddpnum.ToString();
                    itemDato[3] = item.Sddpnomb.ToString();
                    itemDato[4] = item.PmPmhtAnhio.ToString();
                    itemDato[5] = item.PmPmhtDisp01.ToString();
                    itemDato[6] = item.PmPmhtDisp02.ToString();
                    itemDato[7] = item.PmPmhtDisp03.ToString();
                    itemDato[8] = item.PmPmhtDisp04.ToString();
                    itemDato[9] = item.PmPmhtDisp05.ToString();
                    itemDato[10] = item.PmPmhtDisp06.ToString();
                    itemDato[11] = item.PmPmhtDisp07.ToString();
                    itemDato[12] = item.PmPmhtDisp08.ToString();
                    itemDato[13] = item.PmPmhtDisp09.ToString();
                    itemDato[14] = item.PmPmhtDisp10.ToString();
                    itemDato[15] = item.PmPmhtDisp11.ToString();
                    itemDato[16] = item.PmPmhtDisp12.ToString();
                    #endregion
                    data[index] = itemDato;

                    //title
                    string[] matrizDesc = new string[maxCol];

                    var listaSemXAnio = listaMesAnio.Where(x => x.Pmmesfecinimes.Year == item.PmPmhtAnhio).OrderBy(x => x.Pmmesfecinimes).ToList();
                    int i = 5;
                    foreach (var regSem in listaSemXAnio)
                    {
                        matrizDesc[i] = "";
                        i++;
                    }
                    listaDescripcionHandson.Add(matrizDesc);

                    index++;
                }
            }

            dataHandson = data;
            columnasHandson = columnas;
            descripcionHandson = listaDescripcionHandson.ToArray();

        }

        #endregion

        #region Importar / Exportar

        public void ActualizarListaDataDisponibilidad(List<PmoDatPmhiTrDTO> listaUpdate)
        {
            foreach (var reg in listaUpdate)
            {
                UpdateDatPmhiTr(reg);
            }
        }

        public byte[] ExportarDataDisponibilidad(int codigoPeriodo, string tipoFormato, out string nombreFile)
        {
            nombreFile = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + tipoFormato + ".xlsx";

            List<PmoDatPmhiTrDTO> lista = FactorySic.GetPmoDatPmhiTrRepository().ListDatPmhiTr(codigoPeriodo, tipoFormato);

            return GenerarExcel.Exportar(lista, tipoFormato);
        }

        public List<PmoDatPmhiTrDTO> ImportarDataDisponibilidad(Stream file)
        {
            return GenerarExcel.Importar(file);
        }

        #endregion

        #endregion

        /// <summary>
        /// Metodo para generar el reporte de Mantenimientos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GenerarReporteMantenimientos(int periCodi, string tipoFormato, string tipoReporteMantto, string path)
        {
            int tsddpcodi = GetTsddpcodiXCadena(tipoFormato);
            string fileName = tsddpcodi == ConstantesPMPO.TsddpPlantaHidraulica ? "Reporte de Mantenimientos - Unidades de Generación Hidráulicas.xlsx"
                                                                            : "Reporte de Mantenimientos - Unidades de Generación Térmicas.xlsx";

            PmpoProcesamientoDat obj;
            if (tipoFormato == ConstantesPMPO.FormatoHidraulicoSemanal || tipoFormato == ConstantesPMPO.FormatoTermicoSemanal)
                obj = ObtenerProcesamientoDisponibilidad(periCodi, tipoFormato, tipoReporteMantto, false);
            else
                obj = ObtenerProcesamientoDisponibilidadMensual(periCodi, tipoFormato, tipoReporteMantto, false);

            ExcelDocumentPMPO.GenerarReporteMantenimientos(path + fileName, obj);

            return fileName;
        }

        public string TituloDisponibilidad(string tipo)
        {
            string titulo = "";
            switch (tipo)
            {
                case ConstantesPMPO.FormatoHidraulicoSemanal:
                    titulo = "Disponibilidad Semanal de las unidades de generación - Hidráulico";
                    break;
                case ConstantesPMPO.FormatoHidraulicoMensual:
                    titulo = "Disponibilidad Mensual de las unidades de generación - Hidráulico";
                    break;
                case ConstantesPMPO.FormatoTermicoSemanal:
                    titulo = "Disponibilidad Semanal de las unidades de generación - Térmico";
                    break;
                case ConstantesPMPO.FormatoTermicoMensual:
                    titulo = "Disponibilidad Mensual de las unidades de generación - Térmico";
                    break;
            }

            return titulo;
        }

        #endregion

        #region Demanda (3. Definición de Barras, 4. Demanda en barras)

        public List<PmoDatDbusDTO> ListDbus()
        {
            return FactorySic.GetPmoDatDbusRepository().ListDbus().OrderBy(x => x.GrupoCodiSddp).ToList();
        }

        public List<PmoDatDbusDTO> ListDatDbus()
        {
            return FactorySic.GetPmoDatDbusRepository().ListDatDbus().OrderBy(x => x.Num).ToList();
        }

        public int UpdateDbf(PmoDatDbfDTO entity)
        {
            return FactorySic.GetPmoDatDbfRepository().Update(entity);
        }

        public List<PrGrupoDTO> ListGrupoDbf(int cateCodi)
        {
            return FactorySic.GetPmoDatDbfRepository().ListGrupoDbf(cateCodi);
        }

        public byte[] ExportarDataDbf(int pericodi, string nombarra)
        {
            List<PmoDatDbfDTO> lista = FactorySic.GetPmoDatDbfRepository().ListDbf(pericodi, nombarra);
            return GenerarExcel.ExportarDbf(lista);
        }

        public int DeleteDbf(int pericodi)
        {
            return FactorySic.GetPmoDatDbfRepository().Delete(pericodi);
        }
        public int SaveDbf(PmoDatDbfDTO entity)
        {
            return FactorySic.GetPmoDatDbfRepository().Save(entity);
        }
        public void ArmarGrillaDbf(List<PmoDatDbfDTO> lista, List<PrGrupoDTO> grupos, out string[][] dataHandson, out object[] columnasHandson)
        {
            var nombregrupo = grupos.Select(x => x.Sddpcodi.ToString() + "-" + x.Gruponomb);

            string[] Cabecera = { "BCod - ..Bus.Name..  ", "  LCod  ", "  dd/mm/yyyy  ", "  Llev  ", "  ..Load..  ", "  Icca  ", "  Nro. Semana  ", "GRUPOCODI", "PMDBF5CODI" };//NET 2019-03-07 - Corrección del guardado de la grilla

            object[] columnas = new object[9];//NET 2019-03-07 - Corrección del guardado de la grilla

            columnas[0] = new
            {   //Codigo sddp
                type = GridExcelModel.TipoLista,
                source = nombregrupo.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = true,//NET 2019-03-07 - Corrección del guardado de la grilla
            };

            for (var x = 1; x < columnas.Count() - 2; x++) //NET 2019-03-06 - Corrección del guardado de la grilla
            {
                columnas[x] = new
                {
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = false,
                };
            }

            //NET 2019-03-07 - Corrección del guardado de la grilla
            columnas[7] = new
            {   //grupocodi
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = true,
            };
            //NET 2019-03-07 - Corrección del guardado de la grilla
            columnas[8] = new
            {   //PMDBF5CODI
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                readOnly = true,
            };

            string[][] data = new string[lista.Count + 1][];

            data[0] = Cabecera;
            int index = 1;

            foreach (var item in lista)
            {
                string[] itemDato = new string[9];//NET 2019-03-07 - Corrección del guardado de la grilla
                itemDato[0] = item.CodBarra.ToString() + "-" + item.NomBarra;
                itemDato[1] = (item.PmDbf5LCod ?? "").ToString();
                itemDato[2] = item.PmDbf5FecIni.Value.ToString("dd/MM/yyyy");
                itemDato[3] = item.PmBloqCodi.ToString();
                itemDato[4] = item.PmDbf5Carga.ToString();
                itemDato[5] = item.PmDbf5ICCO.ToString();
                itemDato[6] = item.NroSemana;
                itemDato[7] = item.GrupoCodi.ToString();//NET 2019-03-07 - Corrección del guardado de la grilla
                itemDato[8] = item.PmDbf5Codi.ToString();//NET 2019-03-07 - Corrección del guardado de la grilla

                data[index] = itemDato;
                index++;
            }

            dataHandson = data;
            columnasHandson = columnas;

        }

        public void ArmarHandsonDbf(List<PmoDatDbfDTO> lista, List<PrGrupoDTO> grupos, out string[][] data, out object[] columnas)
        {
            ArmarGrillaDbf(lista, grupos, out data, out columnas);
        }

        public PrGrupoDTO GetGrupoCodiDbf(int grupoCodiSddp)
        {
            return FactorySic.GetPmoDatDbfRepository().GetGrupoCodi(grupoCodiSddp);
        }

        public List<PmoDatDbfDTO> ImportarDataDbf(Stream file)
        {
            return GenerarExcel.ImportarDbf(file);
        }
        public List<PmoDatDbfDTO> ListDbf(int pericodi, string nombarra)
        {
            return FactorySic.GetPmoDatDbfRepository().ListDbf(pericodi, nombarra);
        }

        public List<PmoDatDbfDTO> ListDatDbf(int periCodi)
        {
            return FactorySic.GetPmoDatDbfRepository().ListDatDbf(periCodi);
        }

        private void generarDataProyeccionDemanadaBarras(int PeriCodi)
        {
            //Obtenemos los rango de fechas, para el periodo de trabajo
            Dictionary<string, object> rangosFechas = FactorySic.GetPmoDatGenerateRepository().GetFechasProcesamientoDisponibilidad(PeriCodi);

            if (rangosFechas.Count() == 0)
                throw new Exception("No se encontró fechas para el periodo seleccionado");

            DateTime Fechaperiodo = (DateTime)rangosFechas["Fechaperiodo"];
            var siSemanaIni = servPmpo.UltimaSemana(Fechaperiodo);
            DateTime fechaInicio = siSemanaIni.FechaIni;

            //Lógica Temporal para calcular la fecha fin
            var siSemanaFin = servPmpo.UltimaSemana(Fechaperiodo.AddYears(1));
            DateTime fechaFin = siSemanaFin.FechaIni;

            //Eliminamos la data existente en la tabla PMO_DAT_DBF, para el periodo de trabajo
            FactorySic.GetPmoDatDbfRepository().Delete(PeriCodi);
            FactorySic.GetPmoDatDbfRepository().DeleteDataTmp(PeriCodi);

            //Obtenemos la data base para el periodo de procesamiento            
            List<PmoDatDbfDTO> list = FactorySic.GetPmoDatDbfRepository().GetDataBaseDbf(fechaInicio, fechaFin);

            //Obtenemos la lista de grupos 
            var grupos = list.Select(p => p.GrupoCodi).Distinct().ToList();
            grupos.Sort();

            //Obtenemos lista de envios
            var envios = list.Select(p => p.EnvioFecha).Distinct().ToList();
            envios.Sort();
            envios.Reverse();//invertimos los envios desde el último hasta primero

            foreach (var grupo in grupos)
            {
                foreach (var envio in envios)
                {
                    var intervalosBarraSein = list.Where(p => p.GrupoCodi == grupo && p.EnvioFecha == envio).OrderBy(p => p.PmDbf5FecIni);
                    foreach (var intervalo in intervalosBarraSein)
                    {
                        //Guardamos el intervalo en la tabla temporal PMO_DAT_DBF_TMP, si aún no existe para la barra SEIN
                        List<PmoDatDbfDTO> listValidacion = FactorySic.GetPmoDatDbfRepository().GetDataTmpByFilter(PeriCodi, (int)grupo, (DateTime)intervalo.PmDbf5FecIni, (int)intervalo.PmBloqCodi, intervalo.PmDbf5LCod);
                        if (listValidacion.Count() == 0)
                        {
                            PmoDatDbfDTO pmoDatDbfIntervalo = intervalo;
                            pmoDatDbfIntervalo.PeriCodi = PeriCodi;
                            FactorySic.GetPmoDatDbfRepository().SqlSaveTmp(intervalo);
                        }

                    }

                }
            }

            List<PmoDatDbfDTO> listFinal = FactorySic.GetPmoDatDbfRepository().GetDataFinalProcesamiento(PeriCodi);
            foreach (var pmoDatDbf in listFinal)
            {
                //Guardamos los registros finales
                PmoDatDbfDTO pmoDatDbfFinal = new PmoDatDbfDTO();
                pmoDatDbfFinal.PeriCodi = PeriCodi;
                pmoDatDbfFinal.GrupoCodi = pmoDatDbf.GrupoCodi;
                pmoDatDbfFinal.PmDbf5LCod = pmoDatDbf.PmDbf5LCod;
                pmoDatDbfFinal.PmDbf5FecIni = pmoDatDbf.PmDbf5FecIni;
                pmoDatDbfFinal.PmBloqCodi = pmoDatDbf.PmBloqCodi;
                pmoDatDbfFinal.PmDbf5Carga = pmoDatDbf.PmDbf5Carga;
                pmoDatDbfFinal.PmDbf5ICCO = pmoDatDbf.PmDbf5ICCO;
                FactorySic.GetPmoDatDbfRepository().Save(pmoDatDbfFinal);
            }


            //Completar Barras del Modelo para el perido de trabajo
            FactorySic.GetPmoDatDbfRepository().CompletarBarrasModelo(PeriCodi);

        }


        #endregion

        #region Unidades tipo renovable (5. Configuración, 6. Modificación, 7. Escenario)

        public List<PmoDatCgndDTO> ListCgnd()
        {
            return FactorySic.GetPmoDatCgndRepository().List();
        }

        public List<PmoDatCgndDTO> ListGrupoCodiCgnd()
        {
            return FactorySic.GetPmoDatCgndRepository().ListGrupoCodig();
        }

        //SELECT GRUPOCODI FROM PMO_DAT_CGND

        public List<PmoDatCgndDTO> ListDatCgnd()
        {
            return FactorySic.GetPmoDatCgndRepository().ListDatCgnd();
        }
        public PmoDatCgndDTO GetByIdCgnd(int id)
        {
            return FactorySic.GetPmoDatCgndRepository().GetById(id);
        }
        public List<PrGrupoDTO> ListBarra()
        {
            return FactorySic.GetPmoDatCgndRepository().ListBarra();
        }
        public int UpdateCgnd(PmoDatCgndDTO entity)
        {
            return FactorySic.GetPmoDatCgndRepository().Update(entity);
        }

        public List<PrGrupoDTO> UpdateCgnd()
        {
            return FactorySic.GetPmoDatCgndRepository().ListBarra();
        }

        public string GenerarReporteExportarCGND(string path)
        {
            string fileName = string.Empty;
            fileName = "Configuración de Fuentes Renovables.xlsx";

            //Obtenemos la data 
            List<PmoDatCgndDTO> listCgnd = this.ListCgnd();
            ExcelDocumentPMPO.GenerarReporteCGND(path + fileName, listCgnd);

            return fileName;
        }


        public string GenerarReporteExportarMGND(string path)
        {
            string fileName = string.Empty;
            fileName = "Modificación de Fuentes Renovables.xlsx";

            //Obtenemos la data 
            List<PmoDatMgndDTO> listMgnd = this.ListMgnd();
            ExcelDocumentPMPO.GenerarReporteMGND(path + fileName, listMgnd);

            return fileName;
        }

        public List<PmoDatMgndDTO> ListMgnd()
        {
            return FactorySic.GetPmoDatMgndRepository().List();
        }

        public List<PmoDatMgndDTO> ListDatMgnd()
        {
            return FactorySic.GetPmoDatMgndRepository().ListDatMgnd();
        }

        public List<PrGrupoDTO> ListBarraMgnd()
        {
            return FactorySic.GetPmoDatMgndRepository().ListBarraMgnd();
        }

        public PmoDatMgndDTO GetByIdMgnd(int id)
        {
            return FactorySic.GetPmoDatMgndRepository().GetById(id);
        }

        public int UpdateMgnd(PmoDatMgndDTO entity)
        {
            return FactorySic.GetPmoDatMgndRepository().Update(entity);
        }

        public byte[] ExportarDataGnde(int pericodi, string central)
        {
            List<PmoDatGndseDTO> lista = FactorySic.GetPmoDatGndseRepository().ListGndse(pericodi, central);
            return GenerarExcel.ExportarGnde(lista);
        }

        public int SaveGnde(PmoDatGndseDTO entity)
        {
            return FactorySic.GetPmoDatGndseRepository().Save(entity);
        }
        public int DeleteGnde(int pericodi, string central)
        {
            return FactorySic.GetPmoDatGndseRepository().Delete(pericodi, central);
        }

        public List<PmoDatGndseDTO> ImportarDataGnde(Stream file)
        {
            return GenerarExcel.ImportarGnde(file);
        }

        public void ArmarGrillaGnde(List<PmoDatGndseDTO> lista, out string[][] dataHandson, out object[] columnasHandson)
        {
            string[] Cabecera = { "Cod. Tabla", "Cod. COES", "Cod. SDDP", "Central", "Stg", "NScn", "LBlk", "PU" };

            object[] columnas = new object[8];

            //columnas[0] = new
            //{   //Codigo sddp
            //    type = GridExcelModel.TipoLista,
            //    source = nombregrupo.ToArray(),
            //    strict = false,
            //    correctFormat = true,
            //    className = "htRight",
            //    readOnly = false,
            //};

            for (var x = 0; x < columnas.Count(); x++)
            {
                columnas[x] = new
                {
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    readOnly = false,
                };
            }

            string[][] data = new string[lista.Count + 1][];

            data[0] = Cabecera;
            int index = 1;

            foreach (var item in lista)
            {
                string[] itemDato = new string[8];
                itemDato[0] = item.PmGnd5Codi.Equals(0) ? "" : item.PmGnd5Codi.ToString();
                itemDato[1] = item.GrupoCodi.Equals(0) ? "" : item.GrupoCodi.ToString();
                itemDato[2] = item.GrupoCodiSDDP.Equals(0) ? "" : item.GrupoCodiSDDP.ToString();
                itemDato[3] = string.IsNullOrEmpty(item.GrupoNomb) ? "" : item.GrupoNomb.ToString();
                itemDato[4] = string.IsNullOrEmpty(item.PmGnd5STG) ? "" : item.PmGnd5STG.ToString();
                itemDato[5] = string.IsNullOrEmpty(item.PmGnd5SCN) ? "" : item.PmGnd5SCN.ToString();
                itemDato[6] = item.PmBloqCodi.Equals(0) ? "" : item.PmBloqCodi.ToString();
                itemDato[7] = item.PmGnd5PU.Equals(0) ? "" : item.PmGnd5PU.ToString();

                data[index] = itemDato;
                index++;
            }

            dataHandson = data;
            columnasHandson = columnas;

        }

        public void ArmarHandsonGnde(List<PmoDatGndseDTO> lista, out string[][] data, out object[] columnas)
        {
            ArmarGrillaGnde(lista, out data, out columnas);
        }

        public List<PrGrupoDTO> ListCentralesGndseByPeriodo(int codigoPeriodo)
        {
            return FactorySic.GetPmoDatGndseRepository().GetCentralesByPeriodo(codigoPeriodo);
        }

        public string GenerarRepDemandaPorBloque(int periodo, string path)
        {
            string fileName = "Reporte Demanda Por Bloque.xlsx";

            List<PmoDatDbfDTO> lista = FactorySic.GetPmoDatDbfRepository().ListDatDbf(periodo);

            ExcelDocumentPMPO.GenerarRepDemandaPorBloque(path + fileName, lista);

            return fileName;
        }

        public string GenerarReporteExcelTotalGndse(int periodo, string path)
        {
            string fileName = "Reporte Generación RER.xlsx";

            List<PrGrupoDTO> gndseCabeceras = FactorySic.GetPmoDatGndseRepository().ListDatGndseCabeceras(Convert.ToInt32(periodo));
            List<PmoDatGndseDTO> gndse = FactorySic.GetPmoDatGndseRepository().ListDatGndse(Convert.ToInt32(periodo));

            ExcelDocumentPMPO.GenerarReporteExcelTotalGndse(path + fileName, gndseCabeceras, gndse);

            return fileName;
        }

        //Método modificado - NET 2019-03-06
        public List<PmoDatGndseDTO> ListDatGndse(int periCodi)
        {
            return FactorySic.GetPmoDatGndseRepository().ListDatGndse(periCodi);
        }

        public List<PmoDatGndseDTO> ListGndse(int periCodi, string central)
        {
            return FactorySic.GetPmoDatGndseRepository().ListGndse(periCodi, central);
        }

        public List<PrGrupoDTO> ListDatGndseCabeceras(int periCodi)
        {
            return FactorySic.GetPmoDatGndseRepository().ListDatGndseCabeceras(periCodi);
        }

        private void generarDataProyeccionRER(int PeriCodi)
        {
            //Obtenemos los rango de fechas, para el periodo de trabajo
            Dictionary<string, object> rangosFechas = FactorySic.GetPmoDatGenerateRepository().GetFechasProcesamientoDisponibilidad(PeriCodi);

            if (rangosFechas.Count() == 0)
                throw new Exception("No se encontró fechas para el periodo seleccionado");

            DateTime Fechaperiodo = (DateTime)rangosFechas["Fechaperiodo"];
            string anioPeriodo = (string)rangosFechas["anio"];


            //string sFechaIniAnio = "01/01/" + Fechaperiodo.Year.ToString() + " 00:00";
            string sFechaIniAnio = "01/01/" + anioPeriodo + " 00:00";
            DateTime FechaIniAnio = DateTime.ParseExact(sFechaIniAnio, "dd/MM/yyyy HH:mm", null);
            var siSemanaIni = servPmpo.UltimaSemana(FechaIniAnio);
            DateTime fechaInicio = siSemanaIni.FechaIni;


            //string sFechaIniAnio2 = "01/01/" + (Fechaperiodo.Year + 1).ToString() + " 00:00";            
            string sFechaIniAnio2 = "01/01/" + (Int32.Parse(anioPeriodo) + 1).ToString() + " 00:00";
            DateTime FechaIniAnio2 = DateTime.ParseExact(sFechaIniAnio2, "dd/MM/yyyy HH:mm", null);
            var siSemanaIniAnio2 = servPmpo.UltimaSemana(FechaIniAnio2);
            DateTime fechaFin = siSemanaIniAnio2.FechaIni; // El rango de trabajo deberá ser menor a esta fecha

            //Eliminamos la data existente en la tabla PMO_DAT_GND, para el periodo de trabajo
            FactorySic.GetPmoDatGndseRepository().Delete(PeriCodi, "0");

            //Obtenemos la data a procesar
            List<PmoDatGndseDTO> list = FactorySic.GetPmoDatGndseRepository().GetDataProcesamiento(fechaInicio, fechaFin, Fechaperiodo);

            //Obtenemos la lista de grupos 
            var grupos = list.Select(p => p.GrupoCodi).Distinct().ToList();
            grupos.Sort();

            //Obtenemos lista de envios
            var envios = list.Select(p => p.EnvioFecha).Distinct().ToList();
            envios.Sort();
            envios.Reverse();//invertimos los envios desde el último hasta primero

            foreach (var grupo in grupos)
            {
                foreach (var envio in envios)
                {
                    var intervalosRER = list.Where(p => p.GrupoCodi == grupo && p.EnvioFecha == envio).OrderBy(p => p.PmGnd5STG);
                    foreach (var intervalo in intervalosRER)
                    {
                        //Guardamos el intervalo en la tabla temporal PMO_DAT_GND, si aún no existe para el grupo RER
                        List<PmoDatGndseDTO> listValidacion = FactorySic.GetPmoDatGndseRepository().GetDataByFilter(PeriCodi, grupo, intervalo.PmGnd5STG, intervalo.PmBloqCodi);
                        if (listValidacion.Count() == 0)
                        {
                            PmoDatGndseDTO pmoDatGndseDTO = intervalo;
                            pmoDatGndseDTO.PmPeriCodi = PeriCodi;
                            FactorySic.GetPmoDatGndseRepository().Save(intervalo);
                        }

                    }

                }
            }


            //Completar Unidades de GEneración del Modelo para el perido de trabajo
            FactorySic.GetPmoDatGndseRepository().CompletarUnidadesGenModelo(PeriCodi);

        }

        #endregion

        public string GenerarRepGrupoRelaso(string strGrrdefcodi, string path)
        {
            string fileName = "Reporte de Relaciones de Grupos SDDP-SIC.xlsx";

            List<PrGrupoRelasoDTO> lista = FactorySic.GetPmoDatGenerateRepository().GetDataGrupoRelaso(strGrrdefcodi);

            ExcelDocumentPMPO.GenerarRepGrupoRelaso(path + fileName, lista);

            return fileName;
        }

        #region Correlaciones 

        public List<PrCategoriaDTO> ListCategoria()
        {
            return FactorySic.GetPrCategoriaRepository().List();
        }

        public List<SiEmpresaDTO> ListEmpresaCorrelacion()
        {
            string catecodis = "2,5"; //modo de operacion termico y grupo hidraulico
            return FactorySic.GetSiEmpresaRepository().ListarEmpresasxCategoria(catecodis);
        }

        /// <summary>
        /// Lista de correlaciones
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="tsddpcodi"></param>
        /// <param name="famcodi"></param>
        /// <param name="tipoReporteMantto"></param>
        /// <returns></returns>
        public List<PmoConfIndispEquipoDTO> ListCorrelaciones(int emprcodi, int tsddpcodi, int famcodi, string tipoReporteMantto
                                                            , out List<EqEquipoDTO> listaEquiposTermicos, out List<PrGrupoDTO> listaGrupoModo, out List<EqEquipoDTO> listaUnidadesTermo)
        {
            List<PmoConfIndispEquipoDTO> listaTmp = FactorySic.GetPmoConfIndispEquipoRepository().List(emprcodi, tsddpcodi, famcodi);
            var listaTmpHidro = listaTmp.Where(x => x.Tsddpcodi == ConstantesPMPO.TsddpPlantaHidraulica).ToList();
            var listaTmpTermo = listaTmp.Where(x => x.Tsddpcodi == ConstantesPMPO.TsddpPlantaTermica).ToList();

            var servInd = new INDAppServicio();
            servInd.ListarUnidadTermicoOpComercialCDispatch(DateTime.Today, "4, 6, 16, 18, 3, 5, 15, 17"
                                                , out listaUnidadesTermo, out List<EqEquipoDTO> listaUnidadesTermoEspecial, out listaEquiposTermicos
                                                , out listaGrupoModo, out List<PrGrupoDTO> listaGrupoDespacho
                                                , out List<ResultadoValidacionAplicativo> listaMsj44);

            List<int> lsddpcodiTermo = listaTmpTermo.Select(x => x.Sddpcodi).Distinct().ToList();

            List<int> listaFamcodiTermo = ListarFamcodiFiltroMantto("G");

            var listaPrueba = new List<int>();
            var listaTermo = new List<PmoConfIndispEquipoDTO>();
            foreach (var sddpcodi in lsddpcodiTermo)
            {
                if (sddpcodi == 113)
                { }
                var listaRelXSddp = listaTmpTermo.Where(x => x.Sddpcodi == sddpcodi).ToList();

                //modos de operación
                var listaRelXSddp0 = listaRelXSddp.Where(x => x.Grupocodimodo > 0).ToList();
                foreach (var reg in listaRelXSddp0)
                {
                    var regModo = listaGrupoModo.Find(x => x.Grupocodi == reg.Grupocodimodo);
                    if (regModo != null)
                    {
                        reg.AreaNomb = regModo.Central;
                        reg.Central = regModo.Central;
                        reg.Equipadre = regModo.Equipadre;
                        reg.TieneCicloComb = regModo.TieneModoCicloCombinado;
                        reg.TieneModoEspecial = regModo.TieneModoEspecial;
                        reg.TieneModoCicloSimple = regModo.TieneModoCicloSimple;
                        reg.ListaEquicodiModo = regModo.ListaEquicodi;
                        reg.ListaEquicodi = regModo.ListaEquicodi;
                        reg.ListaEquiabrev = regModo.ListaEquiabrev;
                        reg.SListaEquiabrev = "CENTRAL, " + string.Join(", ", reg.ListaEquiabrev);
                        reg.SListaEquicodi = regModo.Equipadre + ", " + string.Join(", ", reg.ListaEquicodi);
                        reg.Pe = regModo.Potencia ?? 0;
                    }

                    reg.Famabrev = "Modo op.";
                    reg.FamCodi = 5;
                }

                var listaRelXSddp1 = listaRelXSddp.Where(x => listaFamcodiTermo.Contains(x.FamCodi)).ToList();
                if (listaRelXSddp0.Any())
                {
                    listaTermo.AddRange(listaRelXSddp0);
                    listaPrueba.AddRange(listaRelXSddp1.Select(x => x.PmCindCodi).ToList());
                }
                else
                {
                    //equipos que no tienen modo de operación
                    listaTermo.AddRange(listaRelXSddp1);
                }

                //equipos que nos son centrales ni generadores
                var listaRelXSddp2 = listaRelXSddp.Where(x => !listaFamcodiTermo.Contains(x.FamCodi)).ToList();
                listaTermo.AddRange(listaRelXSddp2);
            }

            List<PmoConfIndispEquipoDTO> lista = new List<PmoConfIndispEquipoDTO>();
            lista.AddRange(listaTmpHidro);
            lista.AddRange(listaTermo);

            if (tipoReporteMantto != "-1")
            {
                List<int> listaFamcodi = ListarFamcodiFiltroMantto(tipoReporteMantto);
                lista = lista.Where(x => listaFamcodi.Contains(x.FamCodi)).ToList();
            }

            string tsddpcodis = tsddpcodi == -1 ? "2,3" : (tsddpcodi.ToString());
            List<PmoSddpCodigoDTO> listaCodigo = servPmpo.ListarCodigoSDDP(tsddpcodi.ToString()).Where(x => x.Sddpestado == ConstantesAppServicio.Activo).ToList();

            foreach (var regRel in lista)
            {
                if (regRel.Sddpcodi == 220)
                { }

                PmoSddpCodigoDTO regCodigo = listaCodigo.Find(x => x.Sddpcodi == regRel.Sddpcodi);
                if (regCodigo != null)
                {
                    regRel.Grupotipo = regCodigo.Tsddpcodi == ConstantesPMPO.TsddpPlantaTermica ? "T" : "H";
                    regRel.Ptomedicodi = regCodigo.Ptomedicodi ?? 0;
                    regRel.Sddpnomb = regCodigo.Sddpnomb;
                    regRel.Sddpnum = regCodigo.Sddpnum;
                    regRel.MensajeAlerta = regCodigo.MensajeValidacion ?? "";
                }
                else
                {
                    regRel.MensajeAlerta = "No existe código SDDP activo.";
                }

                regRel.EquiAbrev = (regRel.EquiAbrev ?? "").Trim();
                regRel.AreaNomb = (regRel.AreaNomb ?? "").Trim();
                regRel.TieneAlerta = !string.IsNullOrEmpty(regRel.MensajeAlerta);
                regRel.FechaModStr = regRel.FechaMod != null ? regRel.FechaMod.Value.ToString(ConstantesAppServicio.FormatoFechaFull) : string.Empty;
            }

            //
            lista = lista.OrderBy(x => x.TieneAlerta ? 1 : 2).ThenBy(x => x.Grupotipo).ThenBy(x => x.Sddpnum).ThenBy(x => x.AreaNomb).ThenBy(x => x.Famabrev).ThenBy(x => x.EquiAbrev).ToList();

            return lista;
        }

        public void ListarEntidadesCalculoDisp(int tsddpcodi, out List<PmoConfIndispEquipoDTO> listaCorrelaciones
                                        , out List<PmoSddpCodigoDTO> listaSddp, out List<EqEquipoDTO> listaEq
                                        , out List<EqEquipoDTO> listaCentralComb, out List<PrGrupoDTO> listaGrupoModo)
        {
            //Obtenemos los nombres de Grupo (Centrales), Configuradas en la tabla PMO_CONF_INDISP_EQUIPO (Correlaciones)
            listaCorrelaciones = ListCorrelaciones(-1, -1, -1, "-1", out List<EqEquipoDTO> listaEquiposTermicos
                                                , out listaGrupoModo, out List<EqEquipoDTO> listaUnidadesTermo)
                                                .Where(x => x.Tsddpcodi == tsddpcodi).ToList();

            //List<int> lSddpcodi = new List<int>() { 9051, 9056, 9059, 9086, 9087, 9088 }; //empresa Kallpa . Las Flores: 9064, 9108
            //List<int> lSddpcodi = new List<int>() { 9047, 9048, 9052, 9053, 9084, 9085 }; //empresa Enel . CT Ventanilla
            //List<int> lSddpcodi = new List<int>() { 9001, 9002 }; //empresa Termoselva . CT Aguaytia
            //List<int> lSddpcodi = new List<int>() { 9023 }; //empresa Egasa . CT Mollendo
            //List<int> lSddpcodi = new List<int>() { 9036, 9038, 9050, 9054, 9057, 9089, 9090, 9091, 9099, 9100 }; //empresa Engie . CT Chilca 1 y 2
            //List<int> lSddpcodi = new List<int>() {  9067, 9104}; //empresa Fenix
            //listaCorrelaciones = listaCorrelaciones.Where(x => lSddpcodi.Contains(x.Sddpnum)).ToList();

            listaSddp = new List<PmoSddpCodigoDTO>();
            foreach (var x in listaCorrelaciones.GroupBy(x => x.Sddpcodi))
            {
                if (x.First().Sddpnum == 9023) 
                { }

                var reg = new PmoSddpCodigoDTO();
                reg.Sddpcodi = x.Key;
                reg.Sddpnum = x.First().Sddpnum;
                reg.Sddpnomb = x.First().Sddpnomb;
                reg.Emprcodi = x.First().EmprCodi;
                var listaEquicodi = x.Select(y => y.EquiCodi).ToList();
                foreach (var tmp in x.ToList())
                    listaEquicodi.AddRange(tmp.ListaEquicodi.ToList());
                reg.ListaEquicodi = listaEquicodi.Where(z => z > 0).Distinct().ToList();
                reg.ListaGrupocodi = x.Select(y => y.Grupocodimodo).Where(z => z > 0).Distinct().ToList();

                var regTmpCorrel = x.ToList().Find(y => y.Grupocodimodo > 0);
                if (regTmpCorrel != null)
                {
                    reg.Grupocodimodo = regTmpCorrel.Grupocodimodo;
                    reg.Equipadre = regTmpCorrel.Equipadre;
                    reg.Central = regTmpCorrel.Central;
                    reg.TieneCicloComb = regTmpCorrel.TieneCicloComb;
                    reg.TieneModoCicloSimple = regTmpCorrel.TieneModoCicloSimple;
                    reg.TieneModoEspecial = regTmpCorrel.TieneModoEspecial;
                    reg.ListaEquicodiModo = regTmpCorrel.ListaEquicodiModo;
                    reg.Pe = regTmpCorrel.Pe;

                    if (regTmpCorrel.TieneModoEspecial)
                    {
                        listaEquicodi.Add(regTmpCorrel.Equipadre);
                        reg.ListaEquicodi = listaEquicodi.Where(z => z > 0).Distinct().ToList();
                    }
                }

                listaSddp.Add(reg);
            }

            listaEq = listaCorrelaciones.GroupBy(x => x.EquiCodi).Select(x => new EqEquipoDTO()
            {
                Equicodi = x.Key,
                Equipadre = x.First().Equipadre,
                Emprcodi = x.First().EmprCodi,
                Grupocodi = x.First().GrupoCodi,
                Gruponomb = x.First().GrupoNomb,
                Areanomb = x.First().AreaNomb,
                Famcodi = x.First().FamCodi
            }).ToList();
            listaEq.AddRange(listaEquiposTermicos);
            listaEq = listaEq.GroupBy(x => x.Equicodi).Select(x => x.First()).ToList();

            listaCentralComb = listaCorrelaciones.Where(x => x.Grupocodimodo > 0 && x.TieneCicloComb).GroupBy(x => x.Equipadre).Select(x => new EqEquipoDTO()
            {
                Equicodi = x.Key,
                Equinomb = x.First().Central,
            }).ToList();

            foreach (var reg in listaSddp)
            {
                var regCentral = listaCentralComb.Find(x => x.Equicodi == reg.Equipadre);
                var listaUnidadXCentral = listaUnidadesTermo.Where(x => x.Equicodi == reg.Equipadre && x.EquicodiTVCicloComb > 0).ToList();

                if (regCentral != null)
                {
                    reg.TieneCentralCicloComb = true;
                    if (listaUnidadXCentral.Any())
                        regCentral.EquicodiTVCicloComb = listaUnidadXCentral.First().EquicodiTVCicloComb;
                }
            }
            //listaGrupo = listaGrupo.Where(x => x.Emprcodi == 12634).ToList(); //INLAND ENERGY SAC //C.H. SANTA TERESA
            //listaGrupo = listaGrupo.Where(x => x.Emprcodi == 11258).ToList(); //HIDROELECTRICA HUANCHOR S.A.C.
            //listaGrupo = listaGrupo.Where(x => x.Emprcodi == 48).ToList(); //ENGIE
            //listaEq = listaEq.Where(x => x.Emprcodi == 12634).ToList(); //INLAND ENERGY SAC //C.H. SANTA TERESA
            //listaEq = listaEq.Where(x => x.Emprcodi == 11258).ToList(); //HIDROELECTRICA HUANCHOR S.A.C.
            //listaEq = listaEq.Where(x => x.Emprcodi == 48).ToList(); //ENGIE

        }

        public List<int> ListarFamcodiFiltroMantto(string tipoReporteMantto)
        {
            string tareaCodi = string.Empty;
            List<int> listaFamcodi = new List<int>();
            string tipoGeneracion = string.Empty;
            string prefijoArchivo = string.Empty;

            if (tipoReporteMantto == "G")
            { //Generación
                tipoGeneracion = "EQUIPOS DE GENERACIÓN";
                tareaCodi = "3,4,10,11,6";
                //listaFamcodi = FactorySic.GetEqFamiliaRepository().ListarByTareaIds(tareaCodi).Select(x => x.Famcodi).ToList();
                listaFamcodi = new List<int>() { 39, 4, 37, 5, 38, 2, 36, 3 };
            }
            if (tipoReporteMantto == "T")
            { //SSEE/Líneas
                tipoGeneracion = "LÍNEAS DE TRANSMISIÓN Y SUBESTACIONES";
                tareaCodi = "1,2";
                listaFamcodi = FactorySic.GetEqFamiliaRepository().ListarByTareaIds(tareaCodi).Select(x => x.Famcodi).ToList();
            }

            return listaFamcodi;
        }

        public int SaveCorrelacion(PmoConfIndispEquipoDTO entity)
        {
            return FactorySic.GetPmoConfIndispEquipoRepository().Save(entity);
        }
        public int UpdateCorrelacion(PmoConfIndispEquipoDTO entity)
        {
            return FactorySic.GetPmoConfIndispEquipoRepository().Update(entity);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PMO_SDDP_TIPO
        /// </summary>
        public PmoConfIndispEquipoDTO GetByIdPmoConfIndispEquipo(int pmCindCodi)
        {
            return FactorySic.GetPmoConfIndispEquipoRepository().GetById(pmCindCodi);
        }

        public void EliminarCorrelacion(int pmCindCodi, string usuario)
        {
            PmoConfIndispEquipoDTO obj = new PmoConfIndispEquipoDTO()
            {
                PmCindCodi = pmCindCodi,
                PmCindUsuModificacion = usuario,
                PmCindFecModificacion = DateTime.Now
            };

            FactorySic.GetPmoConfIndispEquipoRepository().EliminarCorrelacion(obj);
        }

        public List<EqFamiliaDTO> ListTipoEquipo()
        {
            string tareaCodi = "1,2,3,4,10,11,6";
            return FactorySic.GetEqFamiliaRepository().ListarByTareaIds(tareaCodi);
        }

        public List<EqEquipoDTO> ListEquipo(int famcodi)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquiposPorFamilia(famcodi.ToString())
                                .Where(x => x.Equiestado != ConstantesAppServicio.Baja && x.Equiestado != ConstantesAppServicio.Anulado)
                                .OrderBy(x => x.Tareaabrev).ThenBy(x => x.Areanomb).ThenBy(x => x.Famabrev).ThenBy(x => x.Equiabrev).ToList();
        }

        /// <summary>
        /// Listar modos de operación del módulo de indisponibilidades
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListaModoOperacion()
        {
            var servInd = new INDAppServicio();
            servInd.ListarModoOperacionPe(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(2), out List<PrGrupoDTO> listaFinal, out List<ResultadoValidacionAplicativo> listaMsj);

            return listaFinal.OrderBy(x => x.Central).ThenBy(x => x.Gruponomb).ToList();
        }

        #endregion

    }
}
