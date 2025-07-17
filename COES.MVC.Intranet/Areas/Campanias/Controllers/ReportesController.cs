using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Campanias.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Campanias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Areas.Campanias.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using COES.Servicios.Aplicacion.PMPO.Helper;
using COES.Framework.Base.Tools;
using System.IO;
using Microsoft.Office.Interop.Excel;
using DevExpress.Office.Utils;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Threading;

namespace COES.MVC.Intranet.Areas.Campanias.Controllers
{
    public class ReportesController : BaseController
    {
        CampaniasAppService campaniasAppService = new CampaniasAppService();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CampaniasModel model = new CampaniasModel();
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult DescargarReporte(RequestReporte requestReporte)
        {
            int indicador = 1;
            string idreportes = requestReporte.idreportes;
            List<int> tipoReporte = new List<int>();
            var nombreFolderTemp = "";
            var nombreTemp = "";
            try
            {
                if (TipoReporte.DeEmpresas.Equals(requestReporte.tipoReporte))
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory;
                    nombreFolderTemp = Guid.NewGuid().ToString();
                    nombreTemp = ConstantesCampanias.FolderTempReporte + nombreFolderTemp + "\\";
                    string path = Path.Combine(ruta, ConstantesCampanias.FolderFichas);
                    string pathFichasTemp = Path.Combine(path, nombreTemp);
                    FileServer.CreateFolder("", nombreTemp, path);
                    if (FileServer.VerificarExistenciaDirectorio(nombreTemp + "/", path))
                    {
                        FileServer.DeleteFolderAlter(nombreTemp, path);
                        FileServer.CreateFolder("", nombreTemp, path);
                    }
                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderEmpresa, pathFichasTemp))
                    {
                        FileServer.DeleteFolderAlter(ConstantesCampanias.FolderEmpresa, pathFichasTemp);
                        FileServer.CreateFolder("", ConstantesCampanias.FolderEmpresa, pathFichasTemp);
                    }
                    descargarReporteEmpresa(requestReporte, nombreTemp);
                    tipoReporte.Add(1);
                }
                else
                {
                    if (idreportes.Length > 0)
                    {
                        string[] arrayIdReportes = idreportes.Split(',');
                        if (arrayIdReportes.Length > 0)
                        {
                            string ruta = AppDomain.CurrentDomain.BaseDirectory;
                            nombreFolderTemp = Guid.NewGuid().ToString();
                            nombreTemp = ConstantesCampanias.FolderTempReporte + nombreFolderTemp + "\\";                            string path = Path.Combine(ruta, ConstantesCampanias.FolderFichas);
                            string pathFichasTemp = Path.Combine(path, nombreTemp);

                            //borrar carpeta temporal cuando existan registros
                            FileServer.CreateFolder("", nombreTemp, path);
                            if (FileServer.VerificarExistenciaDirectorio(nombreTemp + "/", path))
                            {
                                FileServer.DeleteFolderAlter(nombreTemp, path);
                                FileServer.CreateFolder("", nombreTemp, path);
                            }
                            bool existeCronograma = arrayIdReportes.Contains(TipoReporte.ReporteCronogramaEjecucion.ToString());

                            foreach (string strReporte in arrayIdReportes)
                            {

                                int idReporte = Convert.ToInt16(strReporte);
                                string pathFichaTipo = "";
                                string pathFichaSubTipo = "";
                                string subtipo = "";

                                TransmisionProyectoDTO tranmsProyecto = campaniasAppService.GetTransmisionProyectoById(idReporte);

                                PeriodoDTO Periodo = campaniasAppService.GetPeriodoDTOById(int.Parse(requestReporte.periodo));

                                tipoReporte.Add(idReporte);
                                if (idReporte == TipoReporte.ReporteItcDemanda)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteItc);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteItc, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteItc, pathFichasTemp);
                                    }
                                    descargarReporteItcDemanda(requestReporte, Periodo, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteItcSistema)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteItc);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteItc, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteItc, pathFichasTemp);
                                    }
                                    descargarReporteItcSist(requestReporte, nombreTemp);
                                }

                                else if (idReporte == TipoReporte.ReporteCentralesTermicas)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteCentralesTermicas(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteCentralesHidroelectricas)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteCentralesHidroelectricas(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteCentralesSolares)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteCentralesSolares(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteCentralesEolicas)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteCentralesEolicas(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteCentralesBiomasa)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteCentralesBiomasa(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteLineasTransmision)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteLineasTransmision(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteTransformadores)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteTransformadores(requestReporte, Periodo, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteGeneracionDistribuida)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteGeneracionDistribuida(requestReporte, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteHidrogenoVerde)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    }
                                    descargarReporteHidrogenoVerde(requestReporte, existeCronograma, nombreTemp);
                                }
                                else if (idReporte == TipoReporte.ReporteCronogramaEjecucion)
                                {
                                    // pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReporteProyectos);
                                    // if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReporteProyectos, pathFichasTemp))
                                    // {
                                    //     FileServer.CreateFolder("", ConstantesCampanias.FolderReporteProyectos, pathFichasTemp);
                                    // }
                                    // descargarReporteCronogramaEjecucion(tranmsProyecto);
                                }

                                else if (idReporte == TipoReporte.ReporteProyeccionDemanda)
                                {
                                    pathFichaTipo = Path.Combine(pathFichasTemp, ConstantesCampanias.FolderReportePronosticos);
                                    if (!FileServer.VerificarExistenciaDirectorio(ConstantesCampanias.FolderReportePronosticos, pathFichasTemp))
                                    {
                                        FileServer.CreateFolder("", ConstantesCampanias.FolderReportePronosticos, pathFichasTemp);
                                    }
                                    descargarReporteProyeccionDemanda(requestReporte, Periodo, nombreTemp);
                                }
                                else
                                {
                                    throw new Exception($"Reporte con ID {idReporte} no reconocido.");
                                }
                            }

                        }
                    }
                }




            }
            catch (Exception ex)
            {
                indicador = -1;
                throw ex;
            }
            
            return Json(new { tipoReporte, nombreFolderTemp }, JsonRequestBehavior.AllowGet);
        }

        public void descargarReporteItcDemanda(RequestReporte requestReporte, PeriodoDTO Periodo, string nombreTemp)
        {
            ItcDemandaReporteDTO itcDemanda = CargarDetalleReporteItcDemanda(requestReporte);

            ExcelReport.GenerarReporteItcDemanda(itcDemanda, Periodo, nombreTemp);
        }
        public void descargarReporteItcSist(RequestReporte requestReporte, string nombreTemp)
        {
            try
            {
                List<ItcPrm1Dto> listaprm1 = campaniasAppService.GetItcprm1ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
                List<ItcPrm2Dto> listaprm2 = campaniasAppService.GetItcprm2ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
                List<ItcRed1Dto> listared1 = campaniasAppService.GetItcred1ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
                List<ItcRed2Dto> listared2 = campaniasAppService.GetItcred2ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
                List<ItcRed3Dto> listared3 = campaniasAppService.GetItcred3ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
                List<ItcRed4Dto> listared4 = campaniasAppService.GetItcred4ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
                List<ItcRed5Dto> listared5 = campaniasAppService.GetItcred5ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

                ItcSistemaReporteDTO itcSistema = new ItcSistemaReporteDTO();
                itcSistema.listaprm1 = listaprm1;
                itcSistema.listaprm2 = listaprm2;
                itcSistema.listared1 = listared1;
                itcSistema.listared2 = listared2;
                itcSistema.listared3 = listared3;
                itcSistema.listared4 = listared4;
                itcSistema.listared5 = listared5;

                ExcelReport.GenerarReporteItcSist(itcSistema, nombreTemp);
            } catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al procesar el reporte.", ex);
            }
        }
        public void descargarReporteCentralesTermicas(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.CentralTermoHojaC);

            List<RegHojaCCTTADTO> dataTermicaA = descargarProyectoTipoGeneracionCentralTermoA(requestReporte);
            List<RegHojaCCTTBDTO> dataTermicaB = descargarProyectoTipoGeneracionCentralTermoB(requestReporte);

            CentralTermicaReporteDTO centralTermica = new CentralTermicaReporteDTO();
            centralTermica.listaTermicaA = dataTermicaA;
            centralTermica.listaTermicaB = dataTermicaB;

            if(existeCronograma){
                List<Det1RegHojaCCTTCDTO> dataTermicaC = descargarProyectoTipoGeneracionCentralTermoC(requestReporte);
                List<Det2RegHojaCCTTCDTO> dataTermicaC2 = descargarProyectoTipoGeneracionCentralTermoC2(requestReporte);
                centralTermica.listaTermicaC = dataTermicaC;
                centralTermica.listaTermicaC2 = dataTermicaC2;
            }

            ReporteModel model = new ReporteModel();

            ExcelReport.GenerarReporteCentralesTermicas(centralTermica, periodo, ListCatalogo, nombreTemp);
        }

        public void descargarReporteCentralesHidroelectricas(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.CentralHidroHojaC);

            List<RegHojaADTO> dataHidroA = cargarProyectoTipoGeneracionCentralHidroA(requestReporte);
            List<RegHojaBDTO> dataHidroB = cargarProyectoTipoGeneracionCentralHidrob(requestReporte);
            List<RegHojaDDTO> dataFichaD = cargarProyectoTipoGeneracionCentralHidroD(requestReporte);

            CentralHidroReporteDTO centralHidro = new CentralHidroReporteDTO();
            centralHidro.listaHidroA = dataHidroA;
            centralHidro.listaHidroB = dataHidroB;
            centralHidro.listaFichaD = dataFichaD;

            if(existeCronograma){
                List<DetRegHojaCDTO> dataHidroC = cargarProyectoTipoGeneracionCentralHidroC(requestReporte);
                centralHidro.listaHidroC = dataHidroC;
            }

            ExcelReport.GenerarReporteCentralesHidroelectricas(centralHidro, periodo, ListCatalogo, nombreTemp);
        }

        public void descargarReporteCentralesSolares(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.CentralSolarHojaC);

            List<SolHojaADTO> dataSolarA = cargarProyectoTipoGeneracionCentralSolarA(requestReporte);
            List<SolHojaBDTO> dataSolarB = cargarProyectoTipoGeneracionCentralSolarB(requestReporte);

            CentralSolarReporteDTO centralSolar = new CentralSolarReporteDTO();
            centralSolar.listaSolarA = dataSolarA;
            centralSolar.listaSolarB = dataSolarB;

            if(existeCronograma){
                List<DetSolHojaCDTO> dataSolarC = cargarProyectoTipoGeneracionCentralSolarC(requestReporte);
                centralSolar.listaSolarC = dataSolarC;
            }

            ExcelReport.GenerarReporteCentralesSolares(centralSolar, periodo, ListCatalogo, nombreTemp);
        }

        public void descargarReporteCentralesEolicas(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.CentralEolHojaC);

            List<RegHojaEolADTO> dataEolicaA = cargarProyectoTipoGeneracionCentralEolicaA(requestReporte);
            List<RegHojaEolBDTO> dataEolicaB = cargarProyectoTipoGeneracionCentralEolicaB(requestReporte);


            CentralEolicaReporteDTO centralEolica = new CentralEolicaReporteDTO();
            centralEolica.listaEolicaA = dataEolicaA;
            centralEolica.listaEolicaB = dataEolicaB;

            if (existeCronograma)
            {
                List<DetRegHojaEolCDTO> dataEolicaC = cargarProyectoTipoGeneracionCentralEolicaC(requestReporte);
                centralEolica.listaEolicaC = dataEolicaC;
            }

            ExcelReport.GenerarReporteCentralesEolicas(centralEolica, periodo, ListCatalogo, nombreTemp);
        }

        public void descargarReporteCentralesBiomasa(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.CentralBioHojaC);

            List<BioHojaADTO> dataBioA = descargarProyectoTipoGeneracionCentralBiomA(requestReporte);
            List<BioHojaBDTO> dataBioB = descargarProyectoTipoGeneracionCentralBiomB(requestReporte);

            CentralBiomasaReporteDTO centralBiomasa = new CentralBiomasaReporteDTO();
            centralBiomasa.listaBioA = dataBioA;
            centralBiomasa.listaBioB = dataBioB;

            if(existeCronograma){
                List<DetBioHojaCDTO> dataBioC = descargarProyectoTipoGeneracionCentralBiomC(requestReporte);
                centralBiomasa.listaBioC = dataBioC;
            }

            ExcelReport.GenerarReporteCentralesBiomasa(centralBiomasa, periodo, ListCatalogo, nombreTemp);
        }

        public void descargarReporteLineasTransmision(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.LineasHojaB);

            List<LineasFichaADTO> dataLineaA = descargarProyectoTipoLineaA(requestReporte);
            List<LineasFichaATramoDTO> dataLineaATramo = descargarProyectoTipoLineaATramo(requestReporte);
            List<LineasFichaBDetDTO> dataLineaB = descargarProyectoTipoLineaB(requestReporte);
            LineasReporteDTO LineaTrans = new LineasReporteDTO();
            LineaTrans.ListaLineaA = dataLineaA;
            LineaTrans.ListaLineaATramo = dataLineaATramo;
            LineaTrans.ListaLineaB = dataLineaB;
            ExcelReport.GenerarReporteLineasTransmision(LineaTrans, periodo, ListCatalogo, nombreTemp);
        }

        public void descargarReporteTransformadores(RequestReporte requestReporte, PeriodoDTO periodo, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.TransmisionCrono);
            List<DataCatalogoDTO> ListCatalogoTransC = campaniasAppService.ListParametria(CategoriaRequisito.SubesTrasnformPot);
            List<DataCatalogoDTO> ListCatalogoTransP = campaniasAppService.ListParametria(CategoriaRequisito.SubesTrasnformPotPrueba);
            List<DataCatalogoDTO> ListCatalogoTrans = ListCatalogoTransC.Concat(ListCatalogoTransP).ToList();
            List<DataCatalogoDTO> ListCatalogoEqui = campaniasAppService.ListParametria(CategoriaRequisito.SubesCompenReactPrueba);
            List<T2SubestFicha1DTO> dataT2SubA = descargarProyectoTipoT2SubA(requestReporte);
            List<T2SubestFicha1TransDTO> dataT2SubATrans = descargarProyectoTipoT2SubATrans(requestReporte);
            List<T2SubestFicha1EquiDTO> dataT2SubAEqui = descargarProyectoTipoT2SubAEqui(requestReporte);
            List<CroFicha1DetDTO> dataT3Crono = descargarProyectoTipoT3Crono(requestReporte);
            TransformReporteDTO Transform = new TransformReporteDTO();
            Transform.ListaT2Subest1 = dataT2SubA;
            Transform.ListaT2Subest1Trans = dataT2SubATrans;
            Transform.ListaT2Subest1Equi = dataT2SubAEqui;
            Transform.ListaT3Crono = dataT3Crono;
            ExcelReport.GenerarReporteTransformadores(Transform, periodo, ListCatalogo, ListCatalogoTrans, ListCatalogoEqui, nombreTemp);
        }

        public void descargarReporteGeneracionDistribuida(RequestReporte requestReporte, bool existeCronograma, string nombreTemp)
        {
            List<CCGDADTO> dataDistA = descargarProyectoTipoGeneracionDistribuidaA(requestReporte);
            List<CCGDBDTO> dataDistB = descargarProyectoTipoGeneracionDistribuidaB(requestReporte);
            //List<CCGDCOptDTO> dataDistC1 = descargarProyectoTipoGeneracionDistribuidaCOpt(requestReporte);
            //List<CCGDCPesDTO> dataDistC2 = descargarProyectoTipoGeneracionDistribuidaCPes(requestReporte);
            List<CCGDCDTO> dataDistC = descargarProyectoTipoGeneracionDistribuidaC(requestReporte);
            List<CCGDDDTO> dataDistD = descargarProyectoTipoGeneracionDistribuidaD(requestReporte);
            List<CCGDEDTO> dataDistE = descargarProyectoTipoGeneracionDistribuidaE(requestReporte);

            GenDistribuidaReporteDTO GenDistribuida = new GenDistribuidaReporteDTO();
            GenDistribuida.ListaDistA = dataDistA;
            GenDistribuida.ListaDistB = dataDistB;
            //GenDistribuida.ListaDistC1 = dataDistC1;
            //GenDistribuida.ListaDistC2 = dataDistC2;
            GenDistribuida.ListaDistC = dataDistC;
            GenDistribuida.ListaDistD = dataDistD;
            GenDistribuida.ListaDistE = dataDistE;

            ExcelReport.GenerarReporteGeneracionDistribuida(GenDistribuida, nombreTemp);
        }

        public void descargarReporteHidrogenoVerde(RequestReporte proyecto, bool existeCronograma, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametria(CategoriaRequisito.H2VHojaA);
            List<CuestionarioH2VADTO> dataH2VA = descargarProyectoTipoH2VA(proyecto);
            List<CuestionarioH2VBDTO> dataH2VB = descargarProyectoTipoH2VB(proyecto);
            List<CuestionarioH2VCDTO> dataH2VC = descargarProyectoTipoH2VC(proyecto);
        
            List<CuestionarioH2VEDTO> dataH2VE= descargarProyectoTipoH2VE(proyecto);
            List<CuestionarioH2VFDTO> dataH2VF = descargarProyectoTipoH2VF(proyecto);


            CuestionarioH2VReporteDTO dataH2V = new CuestionarioH2VReporteDTO();
            dataH2V.dataH2VA = dataH2VA;
            dataH2V.dataH2VB = dataH2VB;
            dataH2V.dataH2VC = dataH2VC;
            dataH2V.dataH2VE = dataH2VE;
           dataH2V.dataH2VF = dataH2VF;
    

            ExcelReport.GenerarReporteHidrogenoVerde(dataH2V, ListCatalogo, nombreTemp);
        }

        public void descargarReporteCronogramaEjecucion(TransmisionProyectoDTO proyecto, string nombreTemp)
        {
            ReporteModel model = new ReporteModel();
            ExcelReport.GenerarReporteCronogramaEjecucion(model, nombreTemp);
        }

        public void descargarReporteProyeccionDemanda(RequestReporte requestReporte, PeriodoDTO Periodo, string nombreTemp)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll().Where(x => x.CatCodi == 32).ToList();
            List<FormatoD1ADTO> dataProDemandaA = cargarProyectoTipoDemandaA(requestReporte);
            List<FormatoD1BDTO> dataProDemandaB = cargarProyectoTipoDemandaB(requestReporte);
            List<FormatoD1CDTO> dataProDemandaC = cargarProyectoTipoDemandaC(requestReporte);
            List<FormatoD1DDTO> dataProDemandaD = cargarProyectoTipoDemandaD(requestReporte);
            

            ProDemandaReporteDTO proDemanda = new ProDemandaReporteDTO();
            proDemanda.listaProDemandaA = dataProDemandaA;
            proDemanda.listaProDemandaB = dataProDemandaB;
            proDemanda.listaProDemandaC = dataProDemandaC;
            proDemanda.listaProDemandaD = dataProDemandaD;
            ExcelReport.GenerarReporteProyeccionDemanda(proDemanda, Periodo, ListCatalogo, nombreTemp);
        }

        public virtual FileResult GenerarZipReporte(string nameZip, string nameFolderTemp)
        {
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory;
                var nombreFile = nameFolderTemp;
                var nombreZip = nameFolderTemp + ".zip";
                var nombreTemp = ConstantesCampanias.FolderTempReporte;
                string path = ruta + ConstantesCampanias.FolderFichas + nombreTemp;
                var directorio = path + nombreFile;
                var rutaZip = path + nombreZip;

                if (FileServer.VerificarExistenciaFile("", nombreZip, path)) FileServer.DeleteBlob(nombreZip, path);

                FileServer.CreateZipFromDirectory("", rutaZip, directorio);
                if (System.IO.File.Exists(rutaZip))
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            Thread.Sleep(20000);

                            if (Directory.Exists(directorio))
                                Directory.Delete(directorio, true);

                            if (System.IO.File.Exists(rutaZip))
                                FileServer.DeleteBlob(nombreZip, path);
                        }
                        catch (Exception ex)
                        {
                        }
                    });
                    return File(rutaZip, System.Net.Mime.MediaTypeNames.Application.Octet, nameZip);
                }
                else
                {
                    throw new FileNotFoundException(rutaZip);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"ERROR: {ex.Message}", ex);
            }

        }

        public void descargarReporteEmpresa(RequestReporte requestReporte, string nombreTemp)
        {
            List<TransmisionProyectoDTO> listaReporte = campaniasAppService.GetTransmisionProyectoFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            ExcelReport.GenerarReporteEmpresa(listaReporte, nombreTemp);
        }

        public ItcDemandaReporteDTO CargarDetalleReporteItcDemanda(RequestReporte requestReporte)
        {
            ItcDemandaReporteDTO itcDemanda = new ItcDemandaReporteDTO();

            List<Itcdf104DTO> lista104 = campaniasAppService.GetItcdf104ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            List<Itcdf108DTO> lista108 = campaniasAppService.GetItcdf108ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            List<Itcdfp011DTO> listaP011 = campaniasAppService.GetItcdfp011ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            List<Itcdfp012DTO> listaP012 = campaniasAppService.GetItcdfp012ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            
            List<Itcdfp013DTO> listaP013 = campaniasAppService.GetItcdfp013ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            List<Itcdf110DTO> lista110 = campaniasAppService.GetItcdf110ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            List<Itcdf116DTO> lista116 = campaniasAppService.GetItcdf116ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            List<Itcdf121DTO> lista121 = campaniasAppService.GetItcdf121ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            List<Itcdf123DTO> lista123 = campaniasAppService.GetItcdf123ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            // F-P01.3
            for (int i = 0; i < listaP013.Count; i++)
            {
                listaP013[i].ListItcdf013Det = campaniasAppService.GetItcdfp013DetCodi(listaP013[i].Itcdfp013Codi);
            }

            // F-110
            for (int i = 0; i < lista110.Count; i++)
            {
                lista110[i].ListItcdf110Det = campaniasAppService.GetItcdf110DetCodi(lista110[i].Itcdf110Codi);
            }

            // F-116
            for (int i = 0; i < lista116.Count; i++)
            {
                lista116[i].ListItcdf116Det = campaniasAppService.GetItcdf116DetCodi(lista116[i].Itcdf116Codi);
            }

            // F-121
            for (int i = 0; i < lista121.Count; i++)
            {
                lista121[i].ListItcdf121Det = campaniasAppService.GetItcdf121DetCodi(lista121[i].Itcdf121Codi);
            }

            itcDemanda.lista104 = lista104;
            itcDemanda.lista108 = lista108;
            itcDemanda.listaP011 = listaP011;
            itcDemanda.listaP012 = listaP012;
            itcDemanda.listaP013 = listaP013;
            itcDemanda.lista110 = lista110;
            itcDemanda.lista116 = lista116;
            itcDemanda.lista121 = lista121;
            itcDemanda.lista123 = lista123;

            return itcDemanda;
        }

        public List<RegHojaADTO> cargarProyectoTipoGeneracionCentralHidroA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            DataCatalogoDTO catalogo;
            // FICHA A

            List<RegHojaADTO> listaregHojaADTO = campaniasAppService.GetRegHojaAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);


            foreach (var regHojaADTO in listaregHojaADTO)
            {
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == regHojaADTO.Propietario);
                if (catalogo != null)
                {
                    regHojaADTO.Propietario = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == regHojaADTO.Concesiontemporal);
                if (catalogo != null)
                {
                    regHojaADTO.Concesiontemporal = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == regHojaADTO.Tipoconcesionactual);
                if (catalogo != null)
                {
                    regHojaADTO.Tipoconcesionactual = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipoPPL && c.Valor == regHojaADTO.Tuneltipo);
                if (catalogo != null)
                {
                    regHojaADTO.Tuneltipo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSE && c.Valor == regHojaADTO.Tuberiatipo);
                if (catalogo != null)
                {
                    regHojaADTO.Tuberiatipo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSE && c.Valor == regHojaADTO.Maquinatipo);
                if (catalogo != null)
                {
                    regHojaADTO.Maquinatipo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == regHojaADTO.Perfil);
                if (catalogo != null)
                {
                    regHojaADTO.Perfil = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == regHojaADTO.Prefactibilidad);
                if (catalogo != null)
                {
                    regHojaADTO.Prefactibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == regHojaADTO.Factibilidad);
                if (catalogo != null)
                {
                    regHojaADTO.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == regHojaADTO.Estudiodefinitivo);
                if (catalogo != null)
                {
                    regHojaADTO.Estudiodefinitivo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == regHojaADTO.Eia);
                if (catalogo != null)
                {
                    regHojaADTO.Eia = catalogo.DescortaDatacat;
                }
                if (regHojaADTO.Distrito != null && regHojaADTO.Distrito != "")
                {
                    regHojaADTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaADTO.Distrito);
                }
            }

            return listaregHojaADTO;
        }


        public List<RegHojaBDTO> cargarProyectoTipoGeneracionCentralHidrob(RequestReporte requestReporte)
        {
            List<RegHojaBDTO> listaregHojabDTO = campaniasAppService.GetRegHojaBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<DetRegHojaCDTO> cargarProyectoTipoGeneracionCentralHidroC(RequestReporte requestReporte)
        {
            List<DetRegHojaCDTO> listaregHojaCDTO = campaniasAppService.GetRegHojaCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<RegHojaDDTO> cargarProyectoTipoGeneracionCentralHidroD(RequestReporte requestReporte)
        {
            List<RegHojaDDTO> regHojaDDTO = campaniasAppService.GetRegHojaDByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            if (regHojaDDTO != null)
            {
                foreach (var item in regHojaDDTO)
                {
                    item.ListDetRegHojaD = campaniasAppService.GetDetRegHojaDFichaCCodi(item.Hojadcodi);
                }
            }
            return regHojaDDTO;
        }

        public List<RegHojaEolADTO> cargarProyectoTipoGeneracionCentralEolicaA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // EOL-A
            List<RegHojaEolADTO> regHojaEolADTOLista = campaniasAppService.GetRegHojaEolAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            foreach (var regHojaEolADTO in regHojaEolADTOLista)
            {
                List<RegHojaEolADetDTO> regHojaEolADetDTOs = campaniasAppService.GetRegHojaEolADetCodi(regHojaEolADTO.CentralACodi);
                regHojaEolADTO.RegHojaEolADetDTOs = regHojaEolADetDTOs;
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == regHojaEolADTO.Propietario);
                if (catalogo != null)
                {
                    regHojaEolADTO.Propietario = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == regHojaEolADTO.ConcesionTemporal);
                if (catalogo != null)
                {
                    regHojaEolADTO.ConcesionTemporal = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == regHojaEolADTO.TipoConcesionActual);
                if (catalogo != null)
                {
                    regHojaEolADTO.TipoConcesionActual = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSerieVelocidad && c.Valor == regHojaEolADTO.SerieVelViento);
                if (catalogo != null)
                {
                    regHojaEolADTO.SerieVelViento = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudioGeol && c.Valor == regHojaEolADTO.EstudioGeologico);
                if (catalogo != null)
                {
                    regHojaEolADTO.EstudioGeologico = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudioTopo && c.Valor == regHojaEolADTO.EstudioTopografico);
                if (catalogo != null)
                {
                    regHojaEolADTO.EstudioTopografico = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipTurbina && c.Valor == regHojaEolADTO.TipoTurbina);
                if (catalogo != null)
                {
                    regHojaEolADTO.TipoTurbina = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipParqueEol && c.Valor == regHojaEolADTO.TipoParqEolico);
                if (catalogo != null)
                {
                    regHojaEolADTO.TipoParqEolico = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipGenerador && c.Valor == regHojaEolADTO.TipoTecGenerador);
                if (catalogo != null)
                {
                    regHojaEolADTO.TipoTecGenerador = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == regHojaEolADTO.Perfil);
                if (catalogo != null)
                {
                    regHojaEolADTO.Perfil = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == regHojaEolADTO.Prefactibilidad);
                if (catalogo != null)
                {
                    regHojaEolADTO.Prefactibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == regHojaEolADTO.Factibilidad);
                if (catalogo != null)
                {
                    regHojaEolADTO.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == regHojaEolADTO.EstudioDefinitivo);
                if (catalogo != null)
                {
                    regHojaEolADTO.EstudioDefinitivo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == regHojaEolADTO.Eia);
                if (catalogo != null)
                {
                    regHojaEolADTO.Eia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catBacterias && c.Valor == regHojaEolADTO.Bess);
                if (catalogo != null)
                {
                    regHojaEolADTO.Bess = catalogo.DescortaDatacat;
                }
                if (!string.IsNullOrEmpty(regHojaEolADTO.NombreSubestacion))
                {
                    var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == regHojaEolADTO.NombreSubestacion);
                    if (subestacion != null)
                    {
                        regHojaEolADTO.NombreSubestacion = subestacion.Equinomb;
                    }

                }

                if (regHojaEolADTO.Distrito != null && regHojaEolADTO.Distrito != "")
                {
                    regHojaEolADTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaEolADTO.Distrito);
                }
            }
            return regHojaEolADTOLista;
        }

        public List<RegHojaEolBDTO> cargarProyectoTipoGeneracionCentralEolicaB(RequestReporte requestReporte)
        {
            List<RegHojaEolBDTO> listaregHojabDTO = campaniasAppService.GetRegHojaEolBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<DetRegHojaEolCDTO> cargarProyectoTipoGeneracionCentralEolicaC(RequestReporte requestReporte)
        {
            List<DetRegHojaEolCDTO> listaregHojaCDTO = campaniasAppService.GetRegHojaEolCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<SolHojaADTO> cargarProyectoTipoGeneracionCentralSolarA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<SolHojaADTO> solHojaADTOList = campaniasAppService.GetSolHojaAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            DataCatalogoDTO catalogo;

            foreach (var solHojaADTO in solHojaADTOList)
            {
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == solHojaADTO.Propietario);
                if (catalogo != null)
                {
                    solHojaADTO.Propietario = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == solHojaADTO.Concesiontemporal);
                if (catalogo != null)
                {
                    solHojaADTO.Concesiontemporal = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == solHojaADTO.Tipoconcesionact);
                if (catalogo != null)
                {
                    solHojaADTO.Tipoconcesionact = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == solHojaADTO.Perfil);
                if (catalogo != null)
                {
                    solHojaADTO.Perfil = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == solHojaADTO.Prefact);
                if (catalogo != null)
                {
                    solHojaADTO.Prefact = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == solHojaADTO.Factibilidad);
                if (catalogo != null)
                {
                    solHojaADTO.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == solHojaADTO.Estdefinitivo);
                if (catalogo != null)
                {
                    solHojaADTO.Estdefinitivo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == solHojaADTO.Eia);
                if (catalogo != null)
                {
                    solHojaADTO.Eia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSegSolar && c.Valor == solHojaADTO.Seguidorsol);
                if (catalogo != null)
                {
                    solHojaADTO.Seguidorsol = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRadSolar && c.Valor == solHojaADTO.Serieradiacion);
                if (catalogo != null)
                {
                    solHojaADTO.Serieradiacion = catalogo.DescortaDatacat;
                }

                //UBICACION
                if (solHojaADTO.Distrito != null && solHojaADTO.Distrito != "")
                {
                    solHojaADTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(solHojaADTO.Distrito);
                }
            }

            return solHojaADTOList;

        }

        public List<SolHojaBDTO> cargarProyectoTipoGeneracionCentralSolarB(RequestReporte requestReporte)
        {
            List<SolHojaBDTO> listaregHojabDTO = campaniasAppService.GetSolHojaBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<DetSolHojaCDTO> cargarProyectoTipoGeneracionCentralSolarC(RequestReporte requestReporte)
        {
            List<DetSolHojaCDTO> listaregHojaCDTO = campaniasAppService.GetSolHojaCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<FormatoD1ADTO> cargarProyectoTipoDemandaA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // FormatoD1-A
            List<FormatoD1ADTO> formatoD1ADTOList = campaniasAppService.GetFormatoD1AByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            foreach (var formatoD1ADTO in formatoD1ADTOList)
            {
                formatoD1ADTO.ListaFormatoDet1A = campaniasAppService.GetFormatoD1ADET1ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                formatoD1ADTO.ListaFormatoDet2A = campaniasAppService.GetFormatoD1ADET2ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                formatoD1ADTO.ListaFormatoDet3A = campaniasAppService.GetFormatoD1ADET3ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                formatoD1ADTO.ListaFormatoDet4A = campaniasAppService.GetFormatoD1ADET4ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                formatoD1ADTO.ListaFormatoDet5A = campaniasAppService.GetFormatoD1ADET5ByProyCodi(formatoD1ADTO.FormatoD1ACodi);
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTipoCarga && c.Valor == formatoD1ADTO.TipoCarga);
                if (catalogo != null)
                {
                    formatoD1ADTO.TipoCarga = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSubEmpElectro && c.Valor == formatoD1ADTO.EmpresaSuminicodi);
                if (catalogo != null)
                {
                    formatoD1ADTO.EmpresaSuminicodi = catalogo.DescortaDatacat;
                }
                //UBICACION
                if (formatoD1ADTO.Distrito != null && formatoD1ADTO.Distrito != "")
                {
                    formatoD1ADTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(formatoD1ADTO.Distrito);
                }
                // Asignar el nombre de la subestación correspondiente
                if (!string.IsNullOrEmpty(formatoD1ADTO.SubestacionCodi))
                {
                    var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == formatoD1ADTO.SubestacionCodi);
                    if (subestacion != null)
                    {
                        formatoD1ADTO.SubestacionCodi = subestacion.Equinomb;
                    }

                }
            }
            return formatoD1ADTOList;

        }

        public List<FormatoD1BDTO> cargarProyectoTipoDemandaB(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // FormatoD1-B
            List<FormatoD1BDTO> formatoD1BDTOlist = campaniasAppService.GetFormatoD1BByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            foreach (var formatoD1BDTO in formatoD1BDTOlist)
            {
                formatoD1BDTO.ListaFormatoDet1B = campaniasAppService.GetFormatoD1BDetByCodi(formatoD1BDTO.FormatoD1BCodi);
            }
            return formatoD1BDTOlist;
        }
        public List<FormatoD1CDTO> cargarProyectoTipoDemandaC(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // FormatoD1-C
            List<FormatoD1CDTO> formatoD1CDTOlist = campaniasAppService.GetFormatoD1CByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            foreach (var formatoD1CDTO in formatoD1CDTOlist)
            {
                formatoD1CDTO.ListaFormatoDe1CDet = campaniasAppService.GetFormatoD1CDetCCodi(formatoD1CDTO.FormatoD1CCodi);
            }
            return formatoD1CDTOlist;
        }
        public List<FormatoD1DDTO> cargarProyectoTipoDemandaD(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            DataCatalogoDTO catalogo;
            // FormatoD1-d
            List<FormatoD1DDTO> formatoD1DDTOlist = campaniasAppService.GetFormatoD1DByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            //foreach (var formatoD1DDTO in formatoD1DDTOlist)
            //{
            //    formatoD1DDTO.ListaFormatoDe1DDet = campaniasAppService.GetFormatoD1DDetCCodi(formatoD1DDTO.FormatoD1DCodi);
            //}
            return formatoD1DDTOlist;
        }
        public List<RegHojaCCTTADTO> descargarProyectoTipoGeneracionCentralTermoA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // CCTTA
            List<RegHojaCCTTADTO> regHojaCCTTADTOlist = campaniasAppService.GetRegHojaCCTTAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            foreach (var regHojaCCTTADTO in regHojaCCTTADTOlist)
            {
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == regHojaCCTTADTO.Propietario);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Propietario = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == regHojaCCTTADTO.Tipoconcesionactual);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Tipoconcesionactual = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCombustible && c.Valor == regHojaCCTTADTO.Combustibletipo);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Combustibletipo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == regHojaCCTTADTO.Perfil);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Perfil = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == regHojaCCTTADTO.Prefactibilidad);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Prefactibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == regHojaCCTTADTO.Factibilidad);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == regHojaCCTTADTO.Estudiodefinitivo);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Estudiodefinitivo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == regHojaCCTTADTO.Eia);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Eia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalInf && c.Valor == regHojaCCTTADTO.Undpci);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undpci = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalSup && c.Valor == regHojaCCTTADTO.Undpcs);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undpcs = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCComb && c.Valor == regHojaCCTTADTO.Undcomb);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undcomb = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTratComb && c.Valor == regHojaCCTTADTO.Undtrtcomb);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undtrtcomb = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTranspComb && c.Valor == regHojaCCTTADTO.Undtrnspcomb);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undtrnspcomb = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCVarNComb && c.Valor == regHojaCCTTADTO.Undvarncmb);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undvarncmb = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCInvIni && c.Valor == regHojaCCTTADTO.Undinvinic);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undinvinic = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRendPl && c.Valor == regHojaCCTTADTO.Undrendcnd);
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undrendcnd = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConsEspCon && c.Valor == regHojaCCTTADTO.Undconscp);
                // Asignar el nombre de la subestación correspondiente
                if (!string.IsNullOrEmpty(regHojaCCTTADTO.Nombresubestacion))
                {
                    var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == regHojaCCTTADTO.Nombresubestacion);
                    if (subestacion != null)
                    {
                        regHojaCCTTADTO.Nombresubestacion = subestacion.Equinomb;
                    }

                }
                if (catalogo != null)
                {
                    regHojaCCTTADTO.Undconscp = catalogo.DescortaDatacat;
                }
                if (regHojaCCTTADTO.Distrito != null && regHojaCCTTADTO.Distrito != "")
                {
                    regHojaCCTTADTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(regHojaCCTTADTO.Distrito);
                }
            }

            return regHojaCCTTADTOlist;
        }

        public List<RegHojaCCTTBDTO> descargarProyectoTipoGeneracionCentralTermoB(RequestReporte requestReporte)
        {
            List<RegHojaCCTTBDTO> listaregHojabDTO = campaniasAppService.GetRegHojaCCTTBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<Det1RegHojaCCTTCDTO> descargarProyectoTipoGeneracionCentralTermoC(RequestReporte requestReporte)
        {
            List<Det1RegHojaCCTTCDTO> listaregHojaCDTO = campaniasAppService.GetRegHojaCCTTCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<Det2RegHojaCCTTCDTO> descargarProyectoTipoGeneracionCentralTermoC2(RequestReporte requestReporte)
        {
            List<Det2RegHojaCCTTCDTO> listaregHojaCDTO = campaniasAppService.GetRegHojaCCTTC2ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<BioHojaADTO> descargarProyectoTipoGeneracionCentralBiomA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // BIO-A
            List<BioHojaADTO> bioHojaADTOlist = campaniasAppService.GetBioHojaAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            foreach (var bioHojaADTO in bioHojaADTOlist)
            {

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPropietario && c.Valor == bioHojaADTO.Propietario);
                if (catalogo != null)
                {
                    bioHojaADTO.Propietario = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionTemporal && c.Valor == bioHojaADTO.ConTemporal);
                if (catalogo != null)
                {
                    bioHojaADTO.ConTemporal = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConcesionActual && c.Valor == bioHojaADTO.TipoConActual);
                if (catalogo != null)
                {
                    bioHojaADTO.TipoConActual = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == bioHojaADTO.Perfil);
                if (catalogo != null)
                {
                    bioHojaADTO.Perfil = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == bioHojaADTO.Prefactibilidad);
                if (catalogo != null)
                {
                    bioHojaADTO.Prefactibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == bioHojaADTO.Factibilidad);
                if (catalogo != null)
                {
                    bioHojaADTO.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == bioHojaADTO.EstDefinitivo);
                if (catalogo != null)
                {
                    bioHojaADTO.EstDefinitivo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == bioHojaADTO.Eia);
                if (catalogo != null)
                {
                    bioHojaADTO.Eia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCombustible && c.Valor == bioHojaADTO.TipoNomComb);
                if (catalogo != null)
                {
                    bioHojaADTO.TipoNomComb = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalInf && c.Valor == bioHojaADTO.CombPoderCalorInf);
                if (catalogo != null)
                {
                    bioHojaADTO.CombPoderCalorInf = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPodCalSup && c.Valor == bioHojaADTO.CombPoderCalorSup);
                if (catalogo != null)
                {
                    bioHojaADTO.CombPoderCalorSup = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCComb && c.Valor == bioHojaADTO.CombCostoCombustible);
                if (catalogo != null)
                {
                    bioHojaADTO.CombCostoCombustible = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTratComb && c.Valor == bioHojaADTO.CombCostTratamiento);
                if (catalogo != null)
                {
                    bioHojaADTO.CombCostTratamiento = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCTranspComb && c.Valor == bioHojaADTO.CombCostTransporte);
                if (catalogo != null)
                {
                    bioHojaADTO.CombCostTransporte = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCVarNComb && c.Valor == bioHojaADTO.CombCostoVariableNoComb);
                if (catalogo != null)
                {
                    bioHojaADTO.CombCostoVariableNoComb = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catCInvIni && c.Valor == bioHojaADTO.CombCostoInversion);
                if (catalogo != null)
                {
                    bioHojaADTO.CombCostoInversion = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRendPl && c.Valor == bioHojaADTO.CombRendPlanta);
                if (catalogo != null)
                {
                    bioHojaADTO.CombRendPlanta = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catConsEspCon && c.Valor == bioHojaADTO.CombConsEspec);
                if (catalogo != null)
                {
                    bioHojaADTO.CombConsEspec = catalogo.DescortaDatacat;
                }
                // Asignar el nombre de la subestación correspondiente
                if (!string.IsNullOrEmpty(bioHojaADTO.NomSubEstacion))
                {
                    var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == bioHojaADTO.NomSubEstacion);
                    if (subestacion != null)
                    {
                        bioHojaADTO.NomSubEstacion = subestacion.Equinomb;
                    }

                }
                if (bioHojaADTO.Distrito != null && bioHojaADTO.Distrito != "")
                {
                    bioHojaADTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(bioHojaADTO.Distrito);
                }
            }

            return bioHojaADTOlist;
        }

        public List<BioHojaBDTO> descargarProyectoTipoGeneracionCentralBiomB(RequestReporte requestReporte)
        {
            List<BioHojaBDTO> listaregHojabDTO = campaniasAppService.GetBioHojaBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<DetBioHojaCDTO> descargarProyectoTipoGeneracionCentralBiomC(RequestReporte requestReporte)
        {
            List<DetBioHojaCDTO> listaregHojaCDTO = campaniasAppService.GetBioHojaCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<CCGDADTO> descargarProyectoTipoGeneracionDistribuidaA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // CC.GD-A

            List<CCGDADTO> ccgdaDTOlist = campaniasAppService.GetCamCCGDAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            foreach (var ccgdaDTO in ccgdaDTOlist)
            {
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catObjProyecto && c.Valor == ccgdaDTO.ObjetivoProyecto);
                if (catalogo != null)
                {
                    ccgdaDTO.ObjetivoProyecto = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTituloHab && c.Valor == ccgdaDTO.TipoTecnologia);
                if (catalogo != null)
                {
                    ccgdaDTO.TipoTecnologia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == ccgdaDTO.EstadoOperacion);
                if (catalogo != null)
                {
                    ccgdaDTO.EstadoOperacion = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == ccgdaDTO.Perfil);
                if (catalogo != null)
                {
                    ccgdaDTO.Perfil = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == ccgdaDTO.Prefactibilidad);
                if (catalogo != null)
                {
                    ccgdaDTO.Prefactibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == ccgdaDTO.Factibilidad);
                if (catalogo != null)
                {
                    ccgdaDTO.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == ccgdaDTO.EstDefinitivo);
                if (catalogo != null)
                {
                    ccgdaDTO.EstDefinitivo = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == ccgdaDTO.Eia);
                if (catalogo != null)
                {
                    ccgdaDTO.Eia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRecursoUsado && c.Valor == ccgdaDTO.RecursoUsada);
                if (catalogo != null)
                {
                    ccgdaDTO.RecursoUsada = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTecnologia && c.Valor == ccgdaDTO.Tecnologia);
                if (catalogo != null)
                {
                    ccgdaDTO.Tecnologia = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == ccgdaDTO.EstadoOperacionGD);
                if (catalogo != null)
                {
                    ccgdaDTO.EstadoOperacionGD = catalogo.DescortaDatacat;
                }
                if (ccgdaDTO.DistritoCodi != null && ccgdaDTO.DistritoCodi != "")
                {
                    ccgdaDTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(ccgdaDTO.DistritoCodi);
                }
                if (ccgdaDTO.DistritoGDCodi != null && ccgdaDTO.DistritoGDCodi != "")
                {
                    ccgdaDTO.ubicacionDTO2 = campaniasAppService.GetUbicacionByDistrito(ccgdaDTO.DistritoGDCodi);
                }
            }
            return ccgdaDTOlist;
        }


        public List<CCGDBDTO> descargarProyectoTipoGeneracionDistribuidaB(RequestReporte requestReporte)
        {
            List<CCGDBDTO> listaregHojabDTO = campaniasAppService.GetCcgdbByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<CCGDCOptDTO> descargarProyectoTipoGeneracionDistribuidaCOpt(RequestReporte requestReporte)
        {
            List<CCGDCOptDTO> listaregHojabDTO = campaniasAppService.GetCamCCGDCOptByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<CCGDCPesDTO> descargarProyectoTipoGeneracionDistribuidaCPes(RequestReporte requestReporte)
        {
            List<CCGDCPesDTO> listaregHojabDTO = campaniasAppService.GetCamCCGDCPesByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<CCGDCDTO> descargarProyectoTipoGeneracionDistribuidaC(RequestReporte requestReporte)
        {
            List<CCGDCDTO> listaregHojaCDTO = campaniasAppService.GetCamCCGDCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaCDTO;
        }

        public List<CCGDDDTO> descargarProyectoTipoGeneracionDistribuidaD(RequestReporte requestReporte)
        {
            List<CCGDDDTO> listaregHojabDTO = campaniasAppService.GetCamCCGDDByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<CCGDEDTO> descargarProyectoTipoGeneracionDistribuidaE(RequestReporte requestReporte)
        {
            List<CCGDEDTO> listaregHojabDTO = campaniasAppService.GetCamCCGDEByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojabDTO;
        }

        public List<LineasFichaADTO> descargarProyectoTipoLineaA(RequestReporte requestReporte)
        {
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            List<LineasFichaADTO> listaregHojaADTO = campaniasAppService.GetLineasFichaAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            foreach (var linHojaADTO in listaregHojaADTO)
            {
                if (!string.IsNullOrEmpty(linHojaADTO.SubInicio))
                {
                    var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == linHojaADTO.SubInicio);
                    if (subestacion != null)
                    {
                        linHojaADTO.SubInicio = subestacion.Equinomb;
                    }

                }
                if (!string.IsNullOrEmpty(linHojaADTO.SubFin))
                {
                    var subestacion = subestaciones.FirstOrDefault(s => s.Equicodi.ToString() == linHojaADTO.SubFin);
                    if (subestacion != null)
                    {
                        linHojaADTO.SubFin = subestacion.Equinomb;
                    }

                }
            }
                
            return listaregHojaADTO;
        }

        public List<LineasFichaATramoDTO> descargarProyectoTipoLineaATramo(RequestReporte requestReporte)
        {
            List<LineasFichaATramoDTO> listaregHojaADTO = campaniasAppService.GetLineasFichaATramoByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaADTO;
        }

        public List<LineasFichaBDetDTO> descargarProyectoTipoLineaB(RequestReporte requestReporte)
        {
            List<LineasFichaBDetDTO> listaregHojaBDTO = campaniasAppService.GetLineasFichaBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHojaBDTO;
        }

        public List<T2SubestFicha1DTO> descargarProyectoTipoT2SubA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<T2SubestFicha1DTO> listaregHoja1DTO = campaniasAppService.GetT2SubFicha1ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            DataCatalogoDTO catalogo;
            foreach (var reg in listaregHoja1DTO)
            {
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSistBarras && c.Valor == reg.SistemaBarras);
                if (catalogo != null)
                {
                    reg.SistemaBarras = catalogo.DescortaDatacat;
                }
            }
            return listaregHoja1DTO;
        }

        public List<T2SubestFicha1TransDTO> descargarProyectoTipoT2SubATrans(RequestReporte requestReporte)
        {
            List<T2SubestFicha1TransDTO> listaregHoja1Det1DTO = campaniasAppService.GetT2SubFicha1TransByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHoja1Det1DTO;
        }

        public List<T2SubestFicha1EquiDTO> descargarProyectoTipoT2SubAEqui(RequestReporte requestReporte)
        {
            List<T2SubestFicha1EquiDTO> listaregHoja1Det3DTO = campaniasAppService.GetT2SubFicha1EquiByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHoja1Det3DTO;
        }

        public List<CroFicha1DetDTO> descargarProyectoTipoT3Crono(RequestReporte requestReporte)
        {
            List<CroFicha1DetDTO> listaregHoja1DTO = campaniasAppService.GetT3CronoFicha1ByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            return listaregHoja1DTO;
        }


        public List<CuestionarioH2VADTO> descargarProyectoTipoH2VA(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<DataSubestacionDTO> subestaciones = campaniasAppService.ListParamSubestacion();
            DataCatalogoDTO catalogo;
            // CC.GD-A

            List<CuestionarioH2VADTO> h2vaDTOlist = campaniasAppService.GetFormatoH2VAByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            foreach (var h2vaDTO in h2vaDTOlist)
            {
                h2vaDTO.ListCH2VADet1DTOs = campaniasAppService.GetCuestionarioH2VADet1ById(h2vaDTO.H2vaCodi);
                h2vaDTO.ListCH2VADet2DTOs = campaniasAppService.GetCuestionarioH2VADet2ById(h2vaDTO.H2vaCodi);

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == h2vaDTO.SituacionAct);
                if (catalogo != null)
                {
                    h2vaDTO.SituacionAct = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catElectrolizador && c.Valor == h2vaDTO.TipoElectrolizador);
                if (catalogo != null)
                {
                    h2vaDTO.TipoElectrolizador = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catObjProyec && c.Valor == h2vaDTO.ObjetivoProyecto);
                if (catalogo != null)
                {
                    h2vaDTO.ObjetivoProyecto = catalogo.DescortaDatacat;
                }


                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catHidrogeno && c.Valor == h2vaDTO.UsoEsperadoHidro);
                if (catalogo != null)
                {
                    h2vaDTO.UsoEsperadoHidro = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTransporteH2 && c.Valor == h2vaDTO.MetodoTransH2);
                if (catalogo != null)
                {
                    h2vaDTO.MetodoTransH2 = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catSuministro && c.Valor == h2vaDTO.TipoSuministro);
                if (catalogo != null)
                {
                    h2vaDTO.TipoSuministro = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstadoOperacion && c.Valor == h2vaDTO.SituacionAct);
                if (catalogo != null)
                {
                    h2vaDTO.SituacionAct = catalogo.DescortaDatacat;
                }

                if (h2vaDTO.Distrito != null && h2vaDTO.Distrito != "")
                {
                    h2vaDTO.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(h2vaDTO.Distrito);
                }
                //if (h2vaDTO.Distrito != null && h2vaDTO.Distrito != "")
                //{
                //    h2vaDTO.ubicacionDTO2 = campaniasAppService.GetUbicacionByDistrito(h2vaDTO.Distrito);
                //}
            }
            return h2vaDTOlist;
        }

        public List<CuestionarioH2VBDTO> descargarProyectoTipoH2VB(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<CuestionarioH2VBDTO> list = campaniasAppService.GetFormatoH2VBByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);
            DataCatalogoDTO catalogo;

            foreach (var dto in list)
            {
  
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPerfil && c.Valor == dto.Perfil);
                if (catalogo != null)
                {
                    dto.Perfil = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catPreFactibilidad && c.Valor == dto.Prefactibilidad);
                if (catalogo != null)
                {
                    dto.Prefactibilidad = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catFactibilidad && c.Valor == dto.Factibilidad);
                if (catalogo != null)
                {
                    dto.Factibilidad = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catElectrolizador && c.Valor == dto.TipoElectrolizador);
                if (catalogo != null)
                {
                    dto.TipoElectrolizador = catalogo.DescortaDatacat;
                }
                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEstudDef && c.Valor == dto.EstudioDefinitivo);
                if (catalogo != null)
                {
                    dto.EstudioDefinitivo = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catEia && c.Valor == dto.EIA);
                if (catalogo != null)
                {
                    dto.EIA = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catRecursoUsado && c.Valor == dto.RecursoUsado);
                if (catalogo != null)
                {
                    dto.RecursoUsado = catalogo.DescortaDatacat;
                }

                catalogo = ListCatalogo.FirstOrDefault(c => c.CatCodi == CatalogoValor.catTecnologia && c.Valor == dto.Tecnologia);
                if (catalogo != null)
                {
                    dto.Tecnologia = catalogo.DescortaDatacat;
                }
                if (dto.Distrito != null && dto.Distrito != "")
                {
                    dto.ubicacionDTO = campaniasAppService.GetUbicacionByDistrito(dto.Distrito);
                }

            }

            return list;
        }

        public List<CuestionarioH2VCDTO> descargarProyectoTipoH2VC(RequestReporte requestReporte)
        {
            // Tampoco hay campos de catálogo en el DTO E, pero sí 'Distrito' implícito (si quieres agregarlo).
            List<CuestionarioH2VCDTO> list = campaniasAppService.GetFormatoH2VCByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            return list;
        }
        public List<CuestionarioH2VEDTO> descargarProyectoTipoH2VE(RequestReporte requestReporte)
        {
            // Tampoco hay campos de catálogo en el DTO E, pero sí 'Distrito' implícito (si quieres agregarlo).
            List<CuestionarioH2VEDTO> list = campaniasAppService.GetFormatoH2VEByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            return list;
        }
        public List<CuestionarioH2VFDTO> descargarProyectoTipoH2VF(RequestReporte requestReporte)
        {
            List<DataCatalogoDTO> ListCatalogo = campaniasAppService.ListParametriaAll();
            List<CuestionarioH2VFDTO> list = campaniasAppService.GetFormatoH2VFByFilter(requestReporte.periodo, requestReporte.empresas, requestReporte.estado);

            return list;
        }


    }
}
