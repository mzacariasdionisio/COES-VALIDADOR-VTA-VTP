using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Reflection;
using System.Configuration;
using log4net;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.Transferencias.Models;
using COES.MVC.Intranet.Areas.Transferencias.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.DemandaMaxima;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Factory;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Net;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.Transferencias.Controllers
{
    public class EvaluacionParticipanteController: BaseController
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        private readonly DemandaMaximaAppServicio servicio = new DemandaMaximaAppServicio();
        private readonly FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        private readonly TransferenciasAppServicio servicioTransferencia = new TransferenciasAppServicio();
        private readonly TransferenciaInformacionBaseAppServicio servicioBase = new TransferenciaInformacionBaseAppServicio();
        private readonly EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }
        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BusquedaEVALDPModel model = new BusquedaEVALDPModel();
            try
            {
                model.FechaInicio = DateTime.Now.ToString(ConstantesTransferencia.FormatoFecha);
                model.FechaFin = DateTime.Now.ToString(ConstantesTransferencia.FormatoFecha);
                model.ListaEmpresas = this.ListarEmpresas(base.UserName);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
            }
            
            //List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(ConstantesTransferencia.FormatoPotenciaMax);

            //model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
            //        OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
            //        {
            //            Emprcodi = x.EMPRCODI,
            //            Emprnomb = x.EMPRNOMB,
            //            Emprrazsocial = x.EMPRRAZSOCIAL
            //        }).ToList();

            return View(model);
        }
        /// <summary>
        /// Permite configurar los puntos mme
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ConfigurarPtoMME(int confconcodi)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();

            try
            {
                model.ConfiguracionPto = servicioTransferencia.ObtenerConfPtosMME(confconcodi);
                model.FechaVigencia = DateTime.Now.ToString(ConstantesTransferencia.FormatoFecha);
                model.ListaEmpresas = this.ListarEmpresas(base.UserName);
                //List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(ConstantesTransferencia.FormatoPotenciaMax);

                //model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                //        OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                //        {
                //            Emprcodi = x.EMPRCODI,
                //            Emprnomb = x.EMPRNOMB,
                //            Emprrazsocial = x.EMPRRAZSOCIAL
                //        }).ToList();               
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de puntos de medición por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        /// 
        public JsonResult CargarPtosMedicion(int idEmpresa)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            try
            {
                var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == ConstantesTransferencia.CabeceraPotenciaMax).FirstOrDefault();
                model.FechaVigencia = DateTime.Now.ToString(ConstantesTransferencia.FormatoFecha);              
                model.ListaHojaPto = this.servicio.GetPtoMedicionPR16(idEmpresa, ConstantesTransferencia.FormatoPotenciaMax, model.FechaVigencia, cabecera.Cabquery).OrderBy(x => x.PuntoConexion == null ? 0: Convert.ToInt32(x.PuntoConexion)).ToList();
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Guarda las nuevas configuraciones para los puntos de medición
        /// </summary>
        /// <param name="codEracmf"></param>
        /// <param name="codOsinergmin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarConfPtoMME(string emprcodi, string ptomedicodi, string vigenciacodi, string fechavigencia, int tipo, int confconcodi = 0)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            int trnconfcodi = 0;
            try
            {
                bool existeConfig;
                TrnConfiguracionPmmeDTO Entidad = new TrnConfiguracionPmmeDTO();
                Entidad.Ptomedicodi = Convert.ToInt32(ptomedicodi);
                Entidad.Emprcodi = Convert.ToInt32(emprcodi);
                Entidad.Vigencia = vigenciacodi == "1" ? "S" : "N";
                Entidad.Fechavigencia = DateTime.ParseExact(fechavigencia, ConstantesTransferencia.FormatoFecha, null);
                Entidad.Lastuser = base.User.Identity.Name;
                Entidad.Lastdate = DateTime.Now;
              
                if (Entidad.Vigencia == "N" && tipo == 1)
                {
                    Entidad.Vigencia = "S";
                    this.servicioTransferencia.VerificarExistenciaConfiguracion(Entidad, out existeConfig);
                    if(existeConfig)
                    {
                        TrnConfiguracionPmmeDTO UltVigente = this.servicioTransferencia.ListaTrnConfiguracionPmmexVigencia(Entidad.Emprcodi, Entidad.Ptomedicodi, Entidad.Vigencia).OrderByDescending(x => x.Fechavigencia).FirstOrDefault();
                        if (UltVigente != null && UltVigente.Fechavigencia >= Entidad.Fechavigencia)
                            model.Resultado = "3";
                        else
                        {
                            Entidad.Vigencia = "N";
                            trnconfcodi = this.servicioTransferencia.SaveTrnConfiguracionPmme(Entidad);
                            model.Resultado = "1";
                        }                      
                    }
                    else
                        model.Resultado = "2";                   
                }
                else if(tipo == 1)
                {
                    this.servicioTransferencia.VerificarExistenciaConfiguracion(Entidad, out existeConfig);
                    if (!existeConfig)
                        trnconfcodi = this.servicioTransferencia.SaveTrnConfiguracionPmme(Entidad);
                    else
                    {
                        Entidad.Vigencia = "N";
                        this.servicioTransferencia.VerificarExistenciaConfiguracion(Entidad, out existeConfig);
                        if (existeConfig)
                        {
                            TrnConfiguracionPmmeDTO UltNoVigente = this.servicioTransferencia.ListaTrnConfiguracionPmmexVigencia(Entidad.Emprcodi,Entidad.Ptomedicodi,Entidad.Vigencia).OrderByDescending(x=>x.Fechavigencia).FirstOrDefault();
                            if (UltNoVigente!= null && UltNoVigente.Fechavigencia >= Entidad.Fechavigencia)
                                model.Resultado = "2";
                        }
                        else
                            model.Resultado = "2";
                    }
                 
                    model.Resultado = "1";
                }

                if(tipo == 2)
                {
                    Entidad.Confconcodi = confconcodi;
                    this.servicioTransferencia.UpdateTrnConfiguracionPmme(Entidad);
                    model.Resultado = "1";
                }
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";

                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Permite mostrar la lista de configuraciones de ptos mme
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(BusquedaEVALDPModel modelo)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();

            try
            {
                DateTime fInicio = DateTime.ParseExact(modelo.FechaInicio, ConstantesTransferencia.FormatoFecha, null);
                DateTime fFin = DateTime.ParseExact(modelo.FechaFin, ConstantesTransferencia.FormatoFecha, null);
                model.ListaConfiguracionPtos = this.servicioTransferencia.ListaConfiguracionPtosMME(modelo.IdEmpresa, fInicio, fFin).ToList();
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";

                Log.Error(NameController, ex);
            }

             return PartialView(model);
        }

        /// <summary>
        /// Muestra la pantalla inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult ListadoEvalCna()
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();

            int idAnio = DateTime.Now.Year;
            int numSemana = EPDate.f_numerosemana(DateTime.Now) - 1;
            model.AnioPeriodo = idAnio + string.Empty;

            string aniosemana = DateTime.Now.Year.ToString() + numSemana.ToString();
            DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, aniosemana, string.Empty, string.Empty).AddDays(2);
            DateTime fechaFinSemana = fechaIniSemana.AddDays(6);

            model.FechaInicio = fechaIniSemana.ToString(ConstantesTransferencia.FormatoFecha);
            model.FechaFin = fechaFinSemana.ToString(ConstantesTransferencia.FormatoFecha);
            model.ListaEmpresas = this.ListarEmpresas(base.UserName);
            //List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(ConstantesTransferencia.FormatoPotenciaMax);

            //model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
            //        OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
            //        {
            //            Emprcodi = x.EMPRCODI,
            //            Emprnomb = x.EMPRNOMB,
            //            Emprrazsocial = x.EMPRRAZSOCIAL
            //        }).ToList();

            return View(model);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Despacho diario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrillaPtoMax(string fechaInicio, string fechaFin, int tipoEmpresa, string nombreEmpresa)
        {
            FormatoModel jsModel = BuildHojaExcelPtoMax(fechaInicio, fechaFin, tipoEmpresa, nombreEmpresa);
            return Json(jsModel);
        }

        /// <summary>
        /// Muestra y carga la información de la potencia máxima de los participantes
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        public FormatoModel BuildHojaExcelPtoMax(string fechaInicio, string fechaFin, int tipoEmpresa, string nombreEmpresa)
        {
            FormatoModel model = new FormatoModel();
            try
            {
                var anioMes = fechaInicio.Split('/');
                var fechaFinal = new DateTime(Convert.ToInt32(anioMes[2]), Convert.ToInt32(anioMes[1]), 1);
                var fechaInicial = fechaFinal.AddMonths(-11);
                int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaTR"]);
                string periodo = string.Empty;
                DemandaMercadoLibreModel modelDemandaMercadoLibre = new DemandaMercadoLibreModel();
                EmpresaModel modelEmpresa = new EmpresaModel();
                modelDemandaMercadoLibre.PeriodosEvaluados = new DateTime[12];
                modelDemandaMercadoLibre.ListaInformacionAgentes = new List<DemandaMercadoLibreDTO>();

                List<DemandaMercadoLibreDTO> ListaAgentes = new List<DemandaMercadoLibreDTO>();
                List<TrnDemandaDTO> ListaDemandas = new List<TrnDemandaDTO>();

                DateTime[] PeriodosPtoMax = new DateTime[4];
                var fechaPtosMax = fechaFinal.AddMonths(-3);
                for (int x = 0; x < 4; x++)
                {
                    PeriodosPtoMax[x] = fechaPtosMax.AddMonths(x);
                }

                foreach (var item in PeriodosPtoMax)
                {
                    fechaInicial = item.AddMonths(-11);
                    periodo = item.ToString("dd/MM/yyyy").Split('/')[1] + item.ToString("dd/MM/yyyy").Split('/')[2];
                    for (int i = 0; i < 12; i++)
                    {
                        modelDemandaMercadoLibre.PeriodosEvaluados[i] = fechaInicial.AddMonths(i);
                    }

                    modelDemandaMercadoLibre.ListaInformacionAgentes = servicioBase.ListDemandaMercadoLibre(modelDemandaMercadoLibre.PeriodosEvaluados,
                        item, tipoEmpresa, nombreEmpresa, IdLectura, Funcion.CodigoOrigenLecturaML);

                    foreach (var dem in modelDemandaMercadoLibre.ListaInformacionAgentes)
                    {
                        dem.PotenciaMaximaRetirar = dem.DemandaMaxima * Funcion.dPorcentajePotenciaMaxima;

                        modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByNombreSic(dem.EmprRazSocial);
                        TrnDemandaDTO demanda = servicioTransferencia.TrnDemandaxEmpresa(periodo, modelEmpresa.Entidad.EmprCodi);
                        if (demanda.Demcodi > 0)
                        {
                            dem.PotenciaMaximaRetirar = demanda.Valormaximo;
                            ListaDemandas.Add(demanda);
                        }
                            

                        dem.Periodo = periodo;
                    }

                    ListaAgentes.AddRange(modelDemandaMercadoLibre.ListaInformacionAgentes);
                }

                List<int> ids = ListaAgentes.Select(x => x.EmprCodi).Distinct().ToList();
                List<string> idsperiodos = ListaAgentes.Select(x => x.Periodo).Distinct().ToList();
                model.Handson = new HandsonModel();
                model.Handson.ListaMerge = new List<CeldaMerge>();
                model.Handson.ListaColWidth = new List<int>();
                model.Formato = new MeFormatoDTO();
                model.Formato.Formatcols = idsperiodos.Count;
                model.Formato.Formatrows = ids.Count;
                model.ColumnasCabecera = 1;
                model.FilasCabecera = 1;
                model.Formato.FechaProceso = new DateTime();
                model.ListaEnvios = new List<MeEnvioDTO>();
                model.EnPlazo = true;
                model.Handson.ReadOnly = false;
                model.Dia = model.Formato.FechaInicio.Day.ToString();
                model.Handson.Width = HandsonConstantes.ColWidth * 20;
                model.Handson.ListaFilaReadOnly = new List<bool>();
                model.Handson.ListaCambios = new List<CeldaCambios>();

                for (int w = 0; w <= model.Formato.Formatcols; w++)
                {
                    if(w == 0)
                        model.Handson.ListaColWidth.Add(300);
                    else
                        model.Handson.ListaColWidth.Add(100);
                }

                model.Handson.ListaExcelData = Funcion.InicializaMatrizExcel(model.FilasCabecera, model.Formato.Formatrows, model.ColumnasCabecera, model.Formato.Formatcols);

                int cont = 0;
                bool band = true;
                for (int x = 0; x < model.Handson.ListaExcelData.Length; x++)
                {
                    
                    if (x == 0)
                    {
                        model.Handson.ListaExcelData[x][0] = "PARTICIPANTE";
                        for (int j = 0; j < idsperiodos.Count; j++)
                        {
                            model.Handson.ListaExcelData[x][j + 1] = idsperiodos[j];
                        }
                    }
                    else
                    {
                        band = true;                    
                        List<string> lstempresa = new List<string>();
                        lstempresa = ListaAgentes.Select(r => r.EmprRazSocial).Distinct().ToList();

                        foreach (var empresa in lstempresa)
                        {
                            if (!band)
                                break;

                            cont = 0;
                            for (int z = 1; z < model.Handson.ListaExcelData.Length; z++)
                            {
                                if (model.Handson.ListaExcelData[z][0] == empresa)
                                    cont++;        
                            }

                            foreach (var agente in ListaAgentes)
                            {
                                for (int i = 0; i < idsperiodos.Count; i++)
                                {                                   
                                    if (empresa == agente.EmprRazSocial && agente.Periodo == idsperiodos[i] && model.Handson.ListaExcelData[0][i + 1] == idsperiodos[i] && cont == 0)
                                    {
                                        for (int r=0; r< ListaDemandas.Count;r++)
                                        {
                                            if (agente.EmprCodi == ListaDemandas[r].Emprcodi && agente.Periodo == ListaDemandas[r].Periododemanda)
                                            {
                                                CeldaCambios cambios = new CeldaCambios();
                                                cambios.Row = x;
                                                cambios.Col = i + 1;
                                                model.Handson.ListaCambios.Add(cambios);
                                            }
                                        }
                                        
                                        model.Handson.ListaExcelData[x][0] = agente.EmprRazSocial;
                                        model.Handson.ListaExcelData[x][i + 1] = agente.PotenciaMaximaRetirar.ToString("N3");
                                        band = false;
                                        break;                                     
                                    }                                                                          
                                }
                            }
                        }                       
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("BuildHojaExcelPtoMax", ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return model;           
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Despacho diario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrillaCna(string[][] dataExcel,string fechaInicio, string fechaFin, int tipoEmpresa, string nombreEmpresa)
        {
            FormatoModel jsModel = BuildHojaExcelCna(dataExcel,fechaInicio, fechaFin, tipoEmpresa, nombreEmpresa);
            return Json(jsModel);
        }

        /// <summary>
        /// Muestra y carga la información de la potencia máxima de los participantes
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        public FormatoModel BuildHojaExcelCna(string[][] dataExcel,string fechaInicio, string fechaFin, int tipoEmpresa, string nombreEmpresa)
        {
            FormatoModel model = new FormatoModel();
            List<string> celdas = new List<string>();
            try
            {
                List<TrnConsumoNoAutorizadoDTO> lstCNAs = new List<TrnConsumoNoAutorizadoDTO>();
                DateTime Finicio = DateTime.ParseExact(fechaInicio, ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime Ffin = DateTime.ParseExact(fechaFin, ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture);
                lstCNAs = servicioTransferencia.ListTrnConsumoNoAutorizado(Finicio.ToString(ConstantesTransferencia.FormatoFechaCorto), Ffin.ToString(ConstantesTransferencia.FormatoFechaCorto));
                DateTime primerdia = DateTime.ParseExact(fechaInicio, ConstantesTransferencia.FormatoFecha, null);
                DateTime ultimodia = DateTime.ParseExact(fechaFin, ConstantesTransferencia.FormatoFecha, null);

                var listaFechas = Enumerable.Range(0, 1 + ultimodia.Subtract(primerdia).Days).Select(incremento => primerdia.AddDays(incremento)).ToList();

                List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(ConstantesTransferencia.FormatoPotenciaMax);

                List<SiEmpresaDTO> ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                        OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                        {
                            Emprcodi = x.EMPRCODI,
                            Emprnomb = x.EMPRNOMB,
                            Emprrazsocial = x.EMPRRAZSOCIAL
                        }).ToList();

                List<string> EmpresasPtoMAx = new List<string>();
                for (int z = 1; z < dataExcel.Length; z++)
                {
                    EmpresasPtoMAx.Add(dataExcel[z][0]);
                }

                List<SiEmpresaDTO> lsEmp = new List<SiEmpresaDTO>();

                for (int r=0; r < ListaEmpresas.Count; r++)
                {
                    for(int y= 0; y < EmpresasPtoMAx.Count; y++)
                    {
                        if (ListaEmpresas[r].Emprrazsocial.Trim() == EmpresasPtoMAx[y].Trim())
                            lsEmp.Add(ListaEmpresas[r]);
                    }
                }
                model.ListaEmpresas = lsEmp;

                model.Handson = new HandsonModel();
                model.Handson.ListaMerge = new List<CeldaMerge>();
                model.Handson.ListaColWidth = new List<int>();
                model.Formato = new MeFormatoDTO();
                model.Formato.Formatcols = 2;
                model.Formato.Formatrows = 2;
                model.ColumnasCabecera = 2;
                model.FilasCabecera = 1;
                model.Formato.FechaProceso = new DateTime();
                model.ListaEnvios = new List<MeEnvioDTO>();
                model.EnPlazo = true;
                model.Handson.ReadOnly = false;
                model.Dia = model.Formato.FechaInicio.Day.ToString();
                model.Handson.Width = HandsonConstantes.ColWidth * 20;

                List<int> idsEmpr = lstCNAs.Select(x => x.Emprcodi).Distinct().ToList();

                model.Handson.ListaColWidth.Add(180);
                foreach (var empr in model.ListaEmpresas)
                {
                    model.Handson.ListaColWidth.Add(300);
                }
                model.Handson.ListaFilaReadOnly = new List<bool>();
                model.ListaCambios = new List<CeldaCambios>();
                model.Handson.ListaExcelData = Funcion.InicializaMatrizExcel(1, listaFechas.Count, 1, model.ListaEmpresas.Count);

                List<string> emprnombres = lstCNAs.Select(x => x.Emprnomb).Distinct().ToList();
                int aux = 0;
                for (int x = 0; x < model.Handson.ListaExcelData.Length; x++)
                {
                    bool band = true;
                    if (x == 0)
                    {
                        model.Handson.ListaExcelData[x][0] = "PARTICIPANTE";
                            
                        for (int j = 0; j < model.ListaEmpresas.Count; j++)
                        {
                            model.Handson.ListaExcelData[x][j + 1] = model.ListaEmpresas[j].Emprrazsocial.Trim();
                        }
                    }
                    else
                    {
                        for (int y = 0; y < listaFechas.Count; y++)
                        {
                            if (lstCNAs.Count > 0)
                            {
                                if (!band)
                                {
                                    aux = y;
                                    break;
                                }

                                if (aux > 0)
                                    y = aux;

                                model.Handson.ListaExcelData[x][0] = listaFechas[y].ToString("dd/MM/yyyy");
                                for (int z = 0; z < lstCNAs.Count; z++)
                                {
                                    for (int r = 1; r < model.Handson.ListaExcelData[0].Length; r++)
                                    {
                                        if (model.Handson.ListaExcelData[0][r] == lstCNAs[z].Emprnomb.Trim() && lstCNAs[z].Fechacna == listaFechas[y])
                                        {
                                            model.Handson.ListaExcelData[x][r] = lstCNAs[z].Valorcna.ToString("N3");
                                            band = false;
                                            break;
                                        }
                                        else if (z == lstCNAs.Count - 1)
                                        {
                                            band = false;
                                            //break;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                if(x == y + 1)
                                    model.Handson.ListaExcelData[x][0] = listaFechas[y].ToString("dd/MM/yyyy");
                            }
                                    
                        }                           
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("BuildHojaExcelPtoMax", ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return model;
        }

        /// <summary>
        /// Permite generar el formato en formato excel de los CNA
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        [HttpPost]
        public JsonResult GenerarFormato(string[][] dataExcel, string fechaInicio, string fechaFin, int tipoEmpresa, string nombreEmpresa)
        {
            string ruta = string.Empty;
            int indicador = 0;
            try
            {
                ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesTransferencia.FolderReporte;
                FormatoModel model = BuildHojaExcelCna(dataExcel, fechaInicio, fechaFin, tipoEmpresa, nombreEmpresa);
                model.NombreArchivoExcel = ConstantesTransferencia.NombreExcel;

                GenerarFileExcelFormato(model, ruta);
                indicador = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Genera Archivo excel del formato y devuelve la ruta mas el nombre del archivo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ruta"></param>
        public static void GenerarFileExcelFormato(FormatoModel model, string ruta)
        {
            FileInfo newFile = new FileInfo(ruta + ConstantesTransferencia.NombreExcel + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
            }
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                int row = 5;
                int column = ConstantesTransferencia.ColExcelData;

                int rowIniFormato = row + 3;
                int colIniFormato = column;

                ws.Cells[rowIniFormato, colIniFormato].Value = "Reporte de Consumos no Autorizados - CNA";
                ws.Cells[rowIniFormato, colIniFormato].Style.Font.SetFromFont(new Font("Calibri", 14));
                ws.Cells[rowIniFormato, colIniFormato].Style.Font.Bold = true;

                row = rowIniFormato;
                ws.Column(1).Width = 3;
                ws.Column(colIniFormato).Width = 19;
               
                int totColumnas = model.Handson.ListaExcelData[0].Length;

                for (var i = 0; i < totColumnas; i++)
                {
                    if (i > 0)
                        ws.Column(colIniFormato + i).Width = 25;

                    for (var x = 0; x < model.Handson.ListaExcelData.Length; x++)
                    {
                        if (x == 0)
                        {
                            ws.Cells[rowIniFormato + x, colIniFormato + i].Value = model.Handson.ListaExcelData[x][i];
                            ws.Cells[rowIniFormato + x, colIniFormato + i].Style.Font.SetFromFont(new Font("Calibri", 12));
                            ws.Cells[rowIniFormato + x, colIniFormato + i].Style.Font.Bold = true;
                            ws.Cells[rowIniFormato + x, colIniFormato + i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[rowIniFormato + x, colIniFormato + i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            ws.Cells[rowIniFormato + x, colIniFormato + i].Value = model.Handson.ListaExcelData[x][i];
                        }
                    }
                }

                using (var range = ws.Cells[rowIniFormato, colIniFormato, rowIniFormato, colIniFormato + totColumnas - 1])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.WrapText = true;
                }

                if (model.Handson.ListaExcelData.Length > 0)
                {
                    using (var range = ws.Cells[rowIniFormato + 1, colIniFormato, rowIniFormato + model.Handson.ListaExcelData.Length - 1, colIniFormato])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor( ColorTranslator.FromHtml("#87CEEB"));
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    using (var range = ws.Cells[rowIniFormato + 1, colIniFormato + 1, rowIniFormato + model.Handson.ListaExcelData.Length - 1, colIniFormato + totColumnas - 1])
                    {
                        range.Style.Numberformat.Format = @"0.000";
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                if (img != null)
                {
                    ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                    picture.From.Column = 1;
                    picture.From.Row = 1;
                }
                xlPackage.Save();
            }
                
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesTransferencia.FolderReporte;
            string fullPath = ruta + ConstantesTransferencia.NombreExcel + ".xlsx";
            return File(fullPath, ConstantesTransferencia.AppExcel, ConstantesTransferencia.NombreExcel + ".xlsx");
        }

        /// <summary>
        /// Permite grabar la potencia máxima
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        [HttpPost]
        public JsonResult grabarPotenciaMax(string[][] dataExcel,string[][] dataExcelCambios, string fechaInicio, string fechaFin, int tipoEmpresa, string nombreEmpresa)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            TrnDemandaDTO Entidad = new TrnDemandaDTO();
            EmpresaModel modelEmpresa = new EmpresaModel();
            try
            {
                for (int x = 1; x < dataExcel.Length; x++)    
                {
                    for (int y = 1; y < dataExcel[0].Length; y++)
                    {
                        for (int z=0;z < dataExcelCambios.Length;z++)
                        {
                            if (Convert.ToInt32(dataExcelCambios[z][0]) == x && Convert.ToInt32(dataExcelCambios[z][1]) == y)
                            {
                                modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByNombreSic(dataExcel[x][0].Trim());
                                Entidad.Emprcodi = modelEmpresa.Entidad.EmprCodi;
                                Entidad.Valormaximo = Convert.ToDecimal(dataExcel[x][y]);
                                Entidad.Periododemanda = dataExcel[0][y];
                                Entidad.Lastuser = base.User.Identity.Name;
                                Entidad.Lastdate = DateTime.Now;
                                this.servicioTransferencia.SaveTrnDemanda(Entidad);
                            }
                        }

                        
                    }  
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Genera la lista de puntos de medición por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        /// 
        public PartialViewResult ConfigurarDias()
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();

            try
            {
                int idAnio = DateTime.Now.Year;
                int numSemana = EPDate.f_numerosemana(DateTime.Now);
                model.SemanaPeriodo = EPDate.GetFechaIniPeriodo(2, string.Empty, idAnio + "" + numSemana, string.Empty, string.Empty).AddDays(2).ToString(ConstantesTransferencia.FormatoFecha);
                model.AnioPeriodo = idAnio + string.Empty;
                int nsemanas = EPDate.TotalSemanasEnAnho(idAnio, FirstDayOfWeek.Saturday);
                string aniosemana = DateTime.Now.Year.ToString() + numSemana.ToString();
                DateTime fechaIniSemanaAnio = EPDate.GetFechaIniPeriodo(2, string.Empty, idAnio + "1", string.Empty, string.Empty).AddDays(2);
                DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, aniosemana, string.Empty, string.Empty).AddDays(2);
                DateTime fechaFinSemana = fechaIniSemana.AddDays(6);
                List<GenericoDTO> entitys = new List<GenericoDTO>();
                for (int i = 1; i <= nsemanas; i++)
                {
                    GenericoDTO reg = new GenericoDTO();
                    reg.Entero1 = i;
                    reg.String1 = "Sem" + i + "-" + idAnio;
                    reg.String2 = fechaIniSemanaAnio.ToString(ConstantesTransferencia.FormatoFecha);
                    entitys.Add(reg);
                    fechaIniSemanaAnio = fechaIniSemanaAnio.AddDays(7);
                }
                model.ListaGenSemanas = entitys;

                string semanaAnio = numSemana.ToString() + DateTime.Now.Year.ToString();        
                model.FechaInicio = fechaIniSemana.ToString(ConstantesTransferencia.FormatoFecha);
                model.FechaFin = fechaFinSemana.ToString(ConstantesTransferencia.FormatoFecha);
                model.EntidadConfiguracionDias = this.servicioTransferencia.ObtenerSemana(semanaAnio);
                model.ListaEmpresas = this.ListarEmpresas(base.UserName);
                //List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(ConstantesTransferencia.FormatoPotenciaMax);
                //model.ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                //        OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                //        {
                //            Emprcodi = x.EMPRCODI,
                //            Emprnomb = x.EMPRNOMB,
                //            Emprrazsocial = x.EMPRRAZSOCIAL
                //        }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato en formato excel de Despacho diario
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        [HttpPost]
        public JsonResult grabarConfiguracionDias(string diasemana,string Dd, string Dl, string Dm, string Dmm, string Dj, string Dvr, string Ds)
        {           
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            TrnPeriodoCnaDTO Entidad = new TrnPeriodoCnaDTO();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(diasemana, ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture);
                int numSemana = EPDate.f_numerosemana(fechaInicio);
                string semanaperiodo = numSemana.ToString() + fechaInicio.Year.ToString();

                Entidad.Dd = Dd;
                Entidad.Dl = Dl;
                Entidad.Dm = Dm;
                Entidad.Dmm = Dmm;
                Entidad.Dj = Dj;
                Entidad.Dvr = Dvr;
                Entidad.Ds = Ds;
                Entidad.Semperiodo = semanaperiodo;
                Entidad.Lastuser = base.User.Identity.Name;
                Entidad.Lastdate = DateTime.Now;

                this.servicioTransferencia.SaveTrnPeriodoCna(Entidad);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Permite procesar los Cna de manera manual
        /// </summary>
        [HttpPost]
        public JsonResult ProcesarCna(string fechaInicio, string fechaFin)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            try
            {
                servicioTransferencia.ProcesarCna(fechaInicio, fechaFin);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Permite procesar los Cna de manera manual
        /// </summary>
        [HttpPost]
        public JsonResult NotificacionCNA()
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            try
            {
                servicioTransferencia.NotificacionCna(ConstantesTransferencia.Plantillacorreo, base.User.Identity.Name);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los CNAs por empresa
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="nombreEmpresa"></param>
        [HttpPost]
        public JsonResult grabarCna(string[][] dataExcel)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            EmpresaModel modelEmpresa = new EmpresaModel();

            try
            {
                for (int x = 1; x < dataExcel.Length; x++)
                {
                    TrnConsumoNoAutorizadoDTO CnaDTO = new TrnConsumoNoAutorizadoDTO();
                    for (int y = 1; y < dataExcel[0].Length; y++)
                    {
                        modelEmpresa.Entidad = (new EmpresaAppServicio()).GetByNombreSic(dataExcel[0][y].Trim());
                        CnaDTO.Emprcodi = modelEmpresa.Entidad.EmprCodi;
                        CnaDTO.Emprnomb = dataExcel[0][y].Trim();
                        CnaDTO.Fechacna = DateTime.ParseExact(dataExcel[x][0], ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture);
                        CnaDTO.Valorcna = dataExcel[x][y] == "" ? 0 : dataExcel[x][y] == null ? 0 : Convert.ToDecimal(dataExcel[x][y]);
                        CnaDTO.Lastuser = base.User.Identity.Name;
                        CnaDTO.Lastdate = DateTime.Now;
                        if (CnaDTO.Valorcna > 0)
                            this.servicioTransferencia.SaveTrnConsumoNoAutorizado(CnaDTO);
                    }
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Permite cargar los días configurados de las semananas
        /// </summary>
        /// <param name="fecha"></param>
        [HttpPost]
        public JsonResult CargarSemanas(string fecha)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            try
            {
                model.FechaInicio = fecha;
                DateTime FechaFin = DateTime.ParseExact(fecha, ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture).AddDays(6);
                model.FechaFin = FechaFin.ToString(ConstantesTransferencia.FormatoFecha);

                int numSemana = EPDate.f_numerosemana(DateTime.ParseExact(fecha, ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture));
                int anio = DateTime.ParseExact(fecha, ConstantesTransferencia.FormatoFecha, CultureInfo.InvariantCulture).Year;
                string semanaAnio = numSemana.ToString() + anio.ToString();
                model.EntidadConfiguracionDias = this.servicioTransferencia.ObtenerSemana(semanaAnio);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
            }
            return Json(model);
        }

        /// <summary>
        /// Permite reiniciar el contador de envio de correos de los participantes
        /// </summary>
        /// <param name="fecha"></param>
        [HttpPost]
        public JsonResult ReiniciarContador(string emprcodi)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            try
            {
                this.servicioTransferencia.DeleteTrnContadorCorreosCna(Convert.ToInt32(emprcodi));
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
            }
            return Json(model);
        }

        /// <summary>
        /// Simula el proceso de cálculo de CNA desde ProcesoServicio
        /// </summary>
        [HttpPost]
        public JsonResult ProcesarCnaTest()
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();
            try
            {
                int numSemana = EPDate.f_numerosemana(DateTime.Now) - 1;
                string semanaperiodo = DateTime.Now.Year.ToString() + numSemana.ToString();
                DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, semanaperiodo, string.Empty, string.Empty).AddDays(2);
                DateTime fechaFinSemana = fechaIniSemana.AddDays(6);

                servicioTransferencia.ProcesarCna(fechaIniSemana.ToString(ConstantesTransferencia.FormatoFecha), fechaFinSemana.ToString(ConstantesTransferencia.FormatoFecha));
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Permite mostrar la lista de configuraciones de ptos mme
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoProcesosCNA(string fechadesde, string fechahasta)
        {
            EvaluacionParticipanteModel model = new EvaluacionParticipanteModel();

            try
            {
                if(fechadesde == null || fechahasta == null)
                {
                    fechadesde = DateTime.Now.ToString(ConstantesTransferencia.FormatoFecha);
                    fechahasta = DateTime.Now.ToString(ConstantesTransferencia.FormatoFecha);
                }
                DateTime fInicio = DateTime.ParseExact(fechadesde, ConstantesTransferencia.FormatoFecha, null);
                DateTime fFin = DateTime.ParseExact(fechahasta, ConstantesTransferencia.FormatoFecha, null);
                model.FechaInicio = fechadesde;
                model.FechaFin = fechahasta;
                model.ListaLogCna = this.servicioTransferencia.ListaTrnLogCna(fInicio, fFin).ToList();
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";

                Log.Error(NameController, ex);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Elimina las configuraciones para los puntos de medición
        /// </summary>
        /// <param name="confconcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarConfiguracionPtoMME(int confconcodi = 0)
        {
            string estado = "I";
            try
            {
                this.servicioTransferencia.DeleteTrnConfiguracionPmme(confconcodi,estado);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { result = "-1", responseText = "Se produjo un error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "1", responseText = "" }, JsonRequestBehavior.AllowGet);
        }

        public List<SiEmpresaDTO> ListarEmpresas(string UserName)
        {
            List<SiEmpresaDTO> listEmpresas = (new FormatoMedicionAppServicio()).GetListaEmpresaFormato(ConstantesTransferencia.FormatoPotenciaMax);
            List<SiEmpresaDTO> ListaEmpresas = new List<SiEmpresaDTO>();
            bool permisoEmpresas = base.VerificarAccesoAccion(ConstantesTransferencia.AccesoEmpresa, UserName);

            if (permisoEmpresas)
            {
                ListaEmpresas = this.seguridad.ListarEmpresas().Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).
                    OrderBy(x => x.EMPRNOMB).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }
            else
            {
                ListaEmpresas = this.seguridad.ObtenerEmpresasActivasPorUsuario(User.Identity.Name).
                    Where(x => listEmpresas.Any(y => y.Emprcodi == x.EMPRCODI) && x.EMPRRAZSOCIAL != null).OrderBy(x => x.EMPRRAZSOCIAL).Select(x => new SiEmpresaDTO
                    {
                        Emprcodi = x.EMPRCODI,
                        Emprnomb = x.EMPRNOMB,
                        Emprrazsocial = x.EMPRRAZSOCIAL
                    }).ToList();
            }
            return ListaEmpresas;
        }
    }
}