using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.FichaTecnica.ViewModels;
using COES.Servicios.Aplicacion;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using log4net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Publico.Areas.FichaTecnica.Controllers
{
    public class FichaTecnicaController : Controller
    {
        //
        // GET: /FichaTecnica/FichaTecnica/
        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        DespachoAppServicio appDespacho = new DespachoAppServicio();
        GeneralAppServicio appGeneral = new GeneralAppServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FichaTecnicaController));
        public FichaTecnicaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("Error Web", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("Error", ex);
                throw;
            }
        }
        public ActionResult Index()
        {
            var Modelo = new IndexFichaTecnicaViewModel();
            Modelo.TipoCentrales = new List<EqFamiliaDTO>();
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(4));
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(5));
            //Modelo.TipoCentrales.Add(appFamilia.TraerFamilia(8));//Lineas
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(37));//CS
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(39));//CE
            if (Request.QueryString["empcod"] != null)
            {
                Modelo.CodEmpresa = Request.QueryString["empcod"].ToString();
            }
            else
            {
                Modelo.CodEmpresa = "0";
            }
            //var empresas = appGeneral.ListarEmpresasPorTipoEmpresa(3).Where(e=>e.Emprestado=="A").OrderBy(e => e.Emprnomb).ToList();

            #region Filtrado de empresas
            int idEmpresa = 0;
            int totalPages = 0;
            int totalRecords = 0;

            //var lsResultado = appEquipo.BuscarEquiposxFiltro(idEmpresa, "AF", iTipoEquipo, 0, page, ref totalPages, 20);

            var lsResultadoCentrales = appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 4, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords);
            lsResultadoCentrales.AddRange(appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 5, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords));
            lsResultadoCentrales.AddRange(appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 37, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords));
            lsResultadoCentrales.AddRange(appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 39, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords));

            try
            {
                var sCentralesExcluidas = ConfigurationManager.AppSettings["CentralesExcluidas"].ToString();
                var lsCentralesExcluidas = sCentralesExcluidas.Split(',').ToList();
                foreach (var sCentralEx in lsCentralesExcluidas)
                {
                    var central = lsResultadoCentrales.SingleOrDefault(t => t.Equicodi == int.Parse(sCentralEx));
                    lsResultadoCentrales.Remove(central);
                }
            }
            catch (Exception ex)
            {

                log.Error("CentralesExcluidas", ex);
            }

            var lsEmpresas = lsResultadoCentrales.Select(T => T.Emprcodi).Distinct();

            var empresas = appGeneral.ListadoEmpresasCentralesActivas().OrderBy(e => e.Emprnomb).ToList();
            //var empresa1 = empresas.SingleOrDefault(t => t.Emprcodi == 10490);
            //var empresa2 = empresas.SingleOrDefault(t => t.Emprcodi == 12479);
            //empresas.Remove(empresa1);
            //empresas.Remove(empresa2);
            List<SiEmpresaDTO> EmpresasListado = new List<SiEmpresaDTO>();
            foreach (var codigo in lsEmpresas)
            {

                var empresa = empresas.Find(t => t.Emprcodi == codigo);
                EmpresasListado.Add(empresa);

            }
            Modelo.Empresas = EmpresasListado.OrderBy(t => t.Emprnomb).ToList();
            #endregion
            Modelo.NombreEquipo = "";
            Modelo.CodigoTipoEquipo = 4;
            Modelo.CodigoTipoEquipo2 = 4;
            return View(Modelo);
        }
        [ActionName("Index"), HttpPost]
        public ActionResult IndexPost()
        {
            var Modelo = new IndexFichaTecnicaViewModel();
            Modelo.TipoCentrales = new List<EqFamiliaDTO>();
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(4));
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(5));
            //Modelo.TipoCentrales.Add(appFamilia.TraerFamilia(8));//Lineas
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(37));//CS
            Modelo.TipoCentrales.Add(appEquipamiento.GetByIdEqFamilia(39));//CE
            var sCodigoEquipo = Request["CodigoTipoEquipo"];
            var sEmpresaCodi = Request["CodEmpresa"];
            var splitEmpresa = sEmpresaCodi.Split(',');
            string codEmpresa = "";
            if (splitEmpresa.Length > 0)
                codEmpresa = splitEmpresa[0];
            else
            {
                codEmpresa = sEmpresaCodi;
            }
            Modelo.CodigoTipoEquipo = int.Parse(sCodigoEquipo);
            Modelo.CodEmpresa = codEmpresa;

            #region Filtrado de empresas
            int idEmpresa = 0;
            int totalPages = 0;
            int totalRecords = 0;

            //var lsResultado = appEquipo.BuscarEquiposxFiltro(idEmpresa, "AF", iTipoEquipo, 0, page, ref totalPages, 20);

            var lsResultadoCentrales = appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 4, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords);
            lsResultadoCentrales.AddRange(appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 5, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords));
            lsResultadoCentrales.AddRange(appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 37, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords));
            lsResultadoCentrales.AddRange(appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", 39, 0, "", -99, 1, 1000, ref totalPages, ref totalRecords));

            try
            {
                var sCentralesExcluidas = ConfigurationManager.AppSettings["CentralesExcluidas"].ToString();
                var lsCentralesExcluidas = sCentralesExcluidas.Split(',').ToList();
                foreach (var sCentralEx in lsCentralesExcluidas)
                {
                    var central = lsResultadoCentrales.SingleOrDefault(t => t.Equicodi == int.Parse(sCentralEx));
                    lsResultadoCentrales.Remove(central);
                }
            }
            catch (Exception ex)
            {

                log.Error("CentralesExcluidas", ex);
            }

            var lsEmpresas = lsResultadoCentrales.Select(T => T.Emprcodi).Distinct();

            var empresas = appGeneral.ListadoEmpresasCentralesActivas().OrderBy(e => e.Emprnomb).ToList();
            //var empresa1 = empresas.SingleOrDefault(t => t.Emprcodi == 10490);
            //var empresa2 = empresas.SingleOrDefault(t => t.Emprcodi == 12479);
            //empresas.Remove(empresa1);
            //empresas.Remove(empresa2);
            List<SiEmpresaDTO> EmpresasListado = new List<SiEmpresaDTO>();
            foreach (var codigo in lsEmpresas)
            {

                var empresa = empresas.Find(t => t.Emprcodi == codigo);
                EmpresasListado.Add(empresa);

            }
            Modelo.Empresas = EmpresasListado.OrderBy(t => t.Emprnomb).ToList();
            #endregion
            Modelo.CodigoTipoEquipo2 = int.Parse(sCodigoEquipo);
            var sNombre = Request["nombreEquipo"];
            Modelo.NombreEquipo = sNombre;
            return View(Modelo);
        }
        public ActionResult ListarCentrales(string sidx, string sord, int page, int rows, string pTipoEquipo, string pCodEmpresa, string pNombre)
        {
            int idEmpresa = int.Parse(pCodEmpresa);
            var iTipoEquipo = int.Parse(pTipoEquipo);
            int totalPages = 0;
            int totalRecords = 0;

            //var lsResultado = appEquipo.BuscarEquiposxFiltro(idEmpresa, "AF", iTipoEquipo, 0, page, ref totalPages, 20);

            var lsResultado = appEquipamiento.ListarEquiposxFiltroPaginado(idEmpresa, "A", iTipoEquipo, 0, pNombre, -99,
                page, 20, ref totalPages, ref totalRecords);

            try
            {
                var sCentralesExcluidas = ConfigurationManager.AppSettings["CentralesExcluidas"].ToString();
                var lsCentralesExcluidas = sCentralesExcluidas.Split(',').ToList();
                foreach (var sCentralEx in lsCentralesExcluidas)
                {
                    var central = lsResultado.SingleOrDefault(t => t.Equicodi == int.Parse(sCentralEx));
                    lsResultado.Remove(central);
                }
            }
            catch (Exception ex)
            {

                log.Error("CentralesExcluidas", ex);
            }


            //if (lsResultado != null)
            //    totalRecords = lsResultado.Count();
            var jsondata = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                from q in lsResultado
                select new
                {
                    cell = new string[]
                    {
                        q.Equicodi.ToString(),
                        q.Equinomb ?? "",
                        q.Equiabrev ?? "" ,
                        q.EMPRNOMB ?? "",
                        q.AREANOMB ?? "",
                        q.Equicodi.ToString(),
                    }
                }
                ).ToArray()
            };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarGeneradores(string sidx, string sord, int page, int rows, string pCentral, string pFamilia)
        {
            var iCentral = int.Parse(pCentral);
            var ifamilia = int.Parse(pFamilia);
            int totalPages = 0;
            int totalRecords = 0;
            var lsResultado = appEquipamiento.ListarEquiposxFiltroPaginado(0, "A", ifamilia, 0, "", iCentral,
                page, 20, ref totalPages, ref totalRecords);

            //var lsResultado = appEquipo.BuscarEquiposxPadre(iCentral, ifamilia, page, ref totalPages, 20);

            if (lsResultado != null)
                totalRecords = lsResultado.Count();
            var jsondata = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                from q in lsResultado
                select new
                {
                    cell = new string[]
                                                            {
                                                                q.Equicodi.ToString(),
                                                                q.Equinomb ?? "",
                                                                q.Equiabrev ?? "" ,
                                                                q.Equicodi.ToString(),
                                                            }
                }
                ).ToArray()
            };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListaModosOperacion(string sidx, string sord, int page, int rows, string pCentral)
        {
            var iCentral = int.Parse(pCentral);
            int totalPages = 0;

            var lsResultado = appDespacho.ListadoModosOperacionPorCentral(iCentral);
            int totalRecords = 0;
            if (lsResultado != null)
                totalRecords = lsResultado.Count();
            else
                lsResultado = new List<ModoOperacionDTO>(1);

            var jsondata = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                from q in lsResultado.OrderBy(g => g.GRUPONOM)
                select new
                {
                    cell = new string[]
                                                            {
                                                                q.MODONOM,
                                                                q.GRUPONOM.ToString(),
                                                                q.EQUICODI.ToString(),
                                                                q.GRUPOCODI.ToString(),
                                                                "",
                                                            }
                }
                ).ToArray()
            };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        protected string ConvertirEnLink(string Valor)
        {
            string link = string.Empty;
            if (string.IsNullOrEmpty(Valor) || string.IsNullOrWhiteSpace(Valor))
                return link;
            if (Valor.ToUpperInvariant().Trim().StartsWith("HTTP"))
            {
                link = "<a href='" + Valor + "' target='_blank'>Archivo</a>";
                link = link.Replace("'", "\"");
                return link;
            }
            else
            {
                return Valor;
            }
            //HtmlString anchor = new HtmlString(link);
        }
        protected string DeterminarValorPropiedad(PropiedadEquipoHistDTO dtoPropiedadVigente)
        {
            string strValor = string.Empty;

            if (dtoPropiedadVigente.PROPUNIDAD == null && dtoPropiedadVigente.PROPFILE == null)
            {
                return dtoPropiedadVigente.VALOR;
            }
            if (dtoPropiedadVigente.PROPUNIDAD == null)
            {
                if (dtoPropiedadVigente.PROPFILE.Trim().ToUpperInvariant() == "S")
                {
                    strValor = ConvertirEnLink(dtoPropiedadVigente.VALOR);
                    return strValor;
                }
                else
                {
                    strValor = dtoPropiedadVigente.VALOR;
                    return strValor;
                }
            }
            if (dtoPropiedadVigente.PROPFILE == null)
            {
                if (dtoPropiedadVigente.PROPUNIDAD.Trim().ToUpperInvariant() == "FILE")
                {
                    strValor = ConvertirEnLink(dtoPropiedadVigente.VALOR);
                    return strValor;
                }
                else
                {
                    strValor = dtoPropiedadVigente.VALOR;
                    return strValor;
                }
            }

            if (dtoPropiedadVigente.PROPUNIDAD.Trim().ToUpperInvariant() == "FILE" || dtoPropiedadVigente.PROPFILE.Trim().ToUpperInvariant() == "S")
            {
                strValor = ConvertirEnLink(dtoPropiedadVigente.VALOR);
            }
            else
            {
                strValor = dtoPropiedadVigente.VALOR;
            }

            return strValor;
        }
        public ActionResult DatosCentralH(string id, string iFamilia)
        {
            var oCentral = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            //var oCentral = appEquipo.TraerEquipo(int.Parse(id));
            //var Propiedades = appPropEqui.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));


            var centralHidraulica = new DetalleCentralHidraulicaViewModel();
            centralHidraulica.NombreCentral = oCentral.Equinomb;
            centralHidraulica.CódigoCentral = id;//oCentral.EQUIABREV;
            centralHidraulica.NombreEMpresa = oCentral.EMPRNOMB;
            centralHidraulica.IdCentral = oCentral.Equicodi.ToString();
            centralHidraulica.IdFamilia = oCentral.Famcodi.ToString();
            foreach (var propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1487:
                        centralHidraulica.Númerounidades = strValor;
                        break;
                    case 1488:
                        centralHidraulica.Tipo = strValor;
                        break;
                    case 46:
                        centralHidraulica.PotenciaEfectiva = strValor;
                        break;
                    case 941:
                        centralHidraulica.PotenciaMinima = strValor;
                        break;

                    case 330:
                        centralHidraulica.ServiciosAuxiliares = strValor;
                        break;
                    case 932:
                        centralHidraulica.Rendimiento = strValor;
                        break;
                    case 1489:
                        centralHidraulica.Batimetría = strValor;
                        break;
                    case 1490:
                        centralHidraulica.Hidrologíadelmes = strValor;
                        break;
                    case 1491:
                        centralHidraulica.Modosoperación = strValor;
                        break;
                    case 1483:
                        centralHidraulica.PotenciaGarantizada = strValor;
                        break;
                    case 1492:
                        centralHidraulica.Esquemahidráulica = strValor;
                        break;
                    case 1493:
                        centralHidraulica.Reservorioanualvolumenmaximo = strValor;
                        break;
                    case 1494:
                        centralHidraulica.Reservorioanualvolumenminimo = strValor;
                        break;
                    case 1495:
                        centralHidraulica.Reservorioanualcaudaldescarga = strValor;
                        break;
                    case 1496:
                        centralHidraulica.Reservorioanualtiemodesplazamiento = strValor;
                        break;
                    case 1497:
                        centralHidraulica.Reservorioestacionalvolumenmaximo = strValor;
                        break;
                    case 1498:
                        centralHidraulica.Reservorioestacionalvolumenminimo = strValor;
                        break;
                    case 1499:
                        centralHidraulica.Reservorioestacionalcaudaldescarga = strValor;
                        break;
                    case 1500:
                        centralHidraulica.Reservorioestacionaltiemodesplazamie = strValor;
                        break;
                    case 1501:
                        centralHidraulica.Reservoriosemanalvolumenmaximo = strValor;
                        break;
                    case 1502:
                        centralHidraulica.Reservoriosemanalvolumenminimo = strValor;
                        break;
                    case 1503:
                        centralHidraulica.Reservoriosemanalcaudaldescarga = strValor;
                        break;
                    case 1504:
                        centralHidraulica.Reservoriosemanaltiemodesplazamiento = strValor;
                        break;
                    case 1505:
                        centralHidraulica.Reservoriodiahoravolumenmaximo = strValor;
                        break;
                    case 1506:
                        centralHidraulica.Reservoriodiahoravolumenminimo = strValor;
                        break;
                    case 1507:
                        centralHidraulica.Reservoriodiahoracaudaldescarga = strValor;
                        break;
                    case 1508:
                        centralHidraulica.Reservoriodiahoratiemodesplazamiento = strValor;
                        break;
                    case 1509:
                        centralHidraulica.Restriccióncaudalmínimoregadío = strValor;
                        break;
                    case 1510:
                        centralHidraulica.Restriccióncaudalmáximoregadío = strValor;
                        break;
                    case 1511:
                        centralHidraulica.Caudalrequeridomensual = strValor;
                        break;
                    case 1512:
                        centralHidraulica.Caudalrequeridosemanal = strValor;
                        break;
                    case 1513:
                        centralHidraulica.Reservoriocaracterísticastécnicas = strValor;
                        break;
                    case 1514:
                        centralHidraulica.Diagramasunifilares = strValor;
                        break;
                }
            }

            return View(centralHidraulica);
        }
        public ActionResult DatosGeneradorH(string id, string iFamilia)
        {

            var Equipo = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Central = appEquipamiento.GetByIdEqEquipo(Equipo.Equipadre.Value);

            var NombreCentral = Central.Equinomb;
            var NombreEmpresa = Central.EMPRNOMB;

            var GeneradorHidraulico = new DetalleGeneradorHidraulicoViewModel();
            GeneradorHidraulico.CodCentral = Central.Equicodi.ToString(); //Central.EQUINOMB;
            GeneradorHidraulico.CodGrupo = Equipo.Equinomb;
            GeneradorHidraulico.NombreCentral = NombreCentral;
            GeneradorHidraulico.NombreEmpresa = NombreEmpresa;

            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));
            //var Propiedades = appPropEqui.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));

            foreach (var propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1525:
                        GeneradorHidraulico.DatPlaca = strValor;
                        break;
                    case ConstantesAppServicio.PropiedadOperacionComercial:
                        GeneradorHidraulico.FecOperCom = propiedadEquipoHistDto.FechapropequiDesc;
                        break;
                    case 971:
                        GeneradorHidraulico.T_turbina = strValor;
                        break;
                    case 1527:
                        GeneradorHidraulico.Fabr = strValor;
                        break;
                    case 1528:
                        GeneradorHidraulico.Modelo = strValor;
                        break;
                    case 1529:
                        GeneradorHidraulico.Serie = strValor;
                        break;
                    case 164:
                        GeneradorHidraulico.Pefectiva = strValor;
                        break;
                    case 1530:
                        GeneradorHidraulico.Pinst = strValor;
                        break;
                    case 297:
                        GeneradorHidraulico.PotNom = strValor;
                        break;
                    case 165:
                        GeneradorHidraulico.Snominal = strValor;
                        break;
                    case 298:
                        GeneradorHidraulico.Pmáx = strValor;
                        break;
                    case 299:
                        GeneradorHidraulico.Pmin = strValor;
                        break;
                    case 1531:
                        GeneradorHidraulico.Psincr = strValor;
                        break;
                    case 178:
                        GeneradorHidraulico.RPM = strValor;
                        break;
                    case 166:
                        GeneradorHidraulico.univelotom = strValor;
                        break;
                    case 300:
                        GeneradorHidraulico.VelDescar = strValor;
                        break;
                    case 301:
                        GeneradorHidraulico.TieSincro = strValor;
                        break;
                    case 1532:
                        GeneradorHidraulico.T_sincr_PC = strValor;
                        break;
                    case 958:
                        GeneradorHidraulico.T_SFSP = strValor;
                        break;
                    case 1533:
                        GeneradorHidraulico.T_ArrNegr = strValor;
                        break;
                    case 1534:
                        GeneradorHidraulico.T_PC_sinc = strValor;
                        break;
                    case 1535:
                        GeneradorHidraulico.T_sinc_par = strValor;
                        break;
                    case 957:
                        GeneradorHidraulico.Tmin_ARR_S = strValor;
                        break;
                    case 1536:
                        GeneradorHidraulico.Tmin_ARR_S_eme = strValor;
                        break;
                    case 19:
                        GeneradorHidraulico.TmaxOperPotMin = strValor;
                        break;
                    case 959:
                        GeneradorHidraulico.Tmin_op = strValor;
                        break;
                    case 1537:
                        GeneradorHidraulico.Ene_sinc = strValor;
                        break;
                    case 1538:
                        GeneradorHidraulico.Ene_PC_sinc = strValor;
                        break;
                    case 303:
                        GeneradorHidraulico.QMínTur = strValor;
                        break;
                    case 304:
                        GeneradorHidraulico.QmáxTur = strValor;
                        break;
                    case 308:
                        GeneradorHidraulico.Rendimiento = strValor;
                        break;
                    case 1539:
                        GeneradorHidraulico.CoefA = strValor;
                        break;
                    case 1540:
                        GeneradorHidraulico.CoefB = strValor;
                        break;
                    case 1541:
                        GeneradorHidraulico.CoefC = strValor;
                        break;
                    case 1542:
                        GeneradorHidraulico.Cost_sol = strValor;
                        break;
                    case 965:
                        GeneradorHidraulico.ConcentraSolMax = strValor;
                        break;
                    case 966:
                        GeneradorHidraulico.ConcentraSolMin = strValor;
                        break;
                    case 955:
                        GeneradorHidraulico.P_SSAA = strValor;
                        break;
                    case 969:
                        GeneradorHidraulico.Sneto_max = strValor;
                        break;
                    case 970:
                        GeneradorHidraulico.Sneto_min = strValor;
                        break;
                    case 1543:
                        GeneradorHidraulico.DBCF = strValor;
                        break;
                    case 1544:
                        GeneradorHidraulico.PotGener = strValor;
                        break;
                    case 1545:
                        GeneradorHidraulico.VelGener = strValor;
                        break;
                    case 1546:
                        GeneradorHidraulico.VelEmbGener = strValor;
                        break;
                    case 1547:
                        GeneradorHidraulico.Numpolos = strValor;
                        break;
                    case 309:
                        GeneradorHidraulico.CurCap = strValor;
                        break;
                    case 14:
                        GeneradorHidraulico.CapacMIn = strValor;
                        break;
                    case 15:
                        GeneradorHidraulico.GI50 = strValor;
                        break;
                    case 13:
                        GeneradorHidraulico.Capac100 = strValor;
                        break;
                    case 17:
                        GeneradorHidraulico.GIMÍN = strValor;
                        break;
                    case 311:
                        GeneradorHidraulico.Reac50 = strValor;
                        break;
                    case 16:
                        GeneradorHidraulico.GI100 = strValor;
                        break;
                    case 960:
                        GeneradorHidraulico.Vn_GEN = strValor;
                        break;
                    case 8:
                        GeneradorHidraulico.TMinGen = strValor;
                        break;
                    case 310:
                        GeneradorHidraulico.TMáxGen = strValor;
                        break;
                    case 183:
                        GeneradorHidraulico.unitensmin = strValor;
                        break;
                    case 184:
                        GeneradorHidraulico.unitensmax = strValor;
                        break;
                    case 961:
                        GeneradorHidraulico.Vmin_SSAA = strValor;
                        break;
                    case 962:
                        GeneradorHidraulico.Vmax_SSAA = strValor;
                        break;
                    case 170:
                        GeneradorHidraulico.FactPot = strValor;
                        break;
                    case 5:
                        GeneradorHidraulico.ARR_BS = strValor;
                        break;
                    case 332:
                        GeneradorHidraulico.TransEjeDir = strValor;
                        break;
                    case 333:
                        GeneradorHidraulico.SubTransEjeDir = strValor;
                        break;
                    case 334:
                        GeneradorHidraulico.ArmSecNeg = strValor;
                        break;
                    case 335:
                        GeneradorHidraulico.Xo = strValor;
                        break;
                    case 371:
                        GeneradorHidraulico.EjeDirCortoCirc = strValor;
                        break;
                    case 372:
                        GeneradorHidraulico.CuadraCircAbierto = strValor;
                        break;
                    case 373:
                        GeneradorHidraulico.SubTransCuadraCircAbierto = strValor;
                        break;
                    case 374:
                        GeneradorHidraulico.Ta = strValor;
                        break;
                    case 500:
                        GeneradorHidraulico.Xp = strValor;
                        break;
                    case 501:
                        GeneradorHidraulico.XL = strValor;
                        break;
                    case 502:
                        GeneradorHidraulico.MomentoInercia = strValor;
                        break;
                    case 503:
                        GeneradorHidraulico.ResistenciaArm = strValor;
                        break;
                    case 504:
                        GeneradorHidraulico.SCR = strValor;
                        break;
                    case 505:
                        GeneradorHidraulico.S10 = strValor;
                        break;
                    case 506:
                        GeneradorHidraulico.S12 = strValor;
                        break;
                    case 964:
                        GeneradorHidraulico.Prot_sob_f = strValor;
                        break;
                    case 968:
                        GeneradorHidraulico.TiempoTransitoriaCortoC = strValor;
                        break;
                    case 972:
                        GeneradorHidraulico.Iexco_1pu = strValor;
                        break;
                    case 973:
                        GeneradorHidraulico.Iexco_12p = strValor;
                        break;
                    case 7:
                        GeneradorHidraulico.R2 = strValor;
                        break;
                    case 167:
                        GeneradorHidraulico.Rn = strValor;
                        break;
                    case 171:
                        GeneradorHidraulico.Xn = strValor;
                        break;
                    case 173:
                        GeneradorHidraulico.TiempoSubTransEjeDir = strValor;
                        break;
                    case 176:
                        GeneradorHidraulico.H = strValor;
                        break;
                    case 179:
                        GeneradorHidraulico.Xq = strValor;
                        break;
                    case 180:
                        GeneradorHidraulico.ReactanciaTransEjeCua = strValor;
                        break;
                    case 181:
                        GeneradorHidraulico.TiempoTransEjeDirCirAbier = strValor;
                        break;
                    case 182:
                        GeneradorHidraulico.ReactanciaSubTrans = strValor;
                        break;
                    case 185:
                        GeneradorHidraulico.Ro = strValor;
                        break;
                    case 186:
                        GeneradorHidraulico.CteTiempoSubTrans = strValor;
                        break;
                    case 331:
                        GeneradorHidraulico.Xd = strValor;
                        break;
                    case 974:
                        GeneradorHidraulico.DBRTE = strValor;
                        break;
                    case 975:
                        GeneradorHidraulico.DBRVT = strValor;
                        break;
                    case 976:
                        GeneradorHidraulico.DBPSS = strValor;
                        break;
                    case 1548:
                        GeneradorHidraulico.TSF = strValor;
                        break;
                    case 1549:
                        GeneradorHidraulico.ProgMantos = strValor;
                        break;
                    case 1550:
                        GeneradorHidraulico.RegVel = strValor;
                        break;
                    case 1551:
                        GeneradorHidraulico.ModoTrab = strValor;
                        break;
                    case 1552:
                        GeneradorHidraulico.ModoContr = strValor;
                        break;
                    case 1553:
                        GeneradorHidraulico.BandaMuer = strValor;
                        break;
                    case 305:
                        GeneradorHidraulico.EstatisVA = strValor;
                        break;
                    case 1554:
                        GeneradorHidraulico.ValSistAisl = strValor;
                        break;
                    case 306:
                        GeneradorHidraulico.RanVariac = strValor;
                        break;
                }
            }

            return View(GeneradorHidraulico);
        }
        public ActionResult DatosCentralT(string id, string iFamilia)
        {
            var oCentral = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));

            var centraltermica = new DetalleCentralTermicaViewModel();
            centraltermica.CodCentral = oCentral.Equicodi.ToString();// oCentral.EQUIABREV;
            centraltermica.NomCentral = oCentral.Equinomb;
            centraltermica.NombreEmpresa = oCentral.EMPRNOMB;
            centraltermica.IdCentral = oCentral.Equicodi.ToString();
            foreach (var propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1515:
                        centraltermica.NumUnid = strValor;
                        break;
                    case 1516:
                        centraltermica.Tipo = strValor;
                        break;
                    case 1517:
                        centraltermica.Comb = strValor;
                        break;
                    case 1518:
                        //ANTES 1519
                        centraltermica.Modos = strValor;
                        break;
                    case 1845:
                        centraltermica.CapacComb = strValor;
                        break;
                    case 1520:
                        centraltermica.PresMinGN = strValor;
                        break;
                    case 1521:
                        centraltermica.ContCentral = strValor;
                        break;
                    case 1522:
                        centraltermica.Pefect = strValor;
                        break;
                    case 49:
                        centraltermica.Pinst = strValor;
                        break;
                    case 50:
                        centraltermica.PinstAp = strValor;
                        break;
                    case 51:
                        centraltermica.PnomMW = strValor;
                        break;
                    case 52:
                        centraltermica.PnomMVA = strValor;
                        break;
                    case 942:
                        centraltermica.Pmín = strValor;
                        break;
                    case 944:
                        centraltermica.Pmáx = strValor;
                        break;
                    case 284:
                        centraltermica.Vgmin = strValor;
                        break;
                    case 285:
                        centraltermica.Vgmax = strValor;
                        break;
                    case 286:
                        centraltermica.Vemin = strValor;
                        break;
                    case 287:
                        centraltermica.VeMáx = strValor;
                        break;
                    case 1523:
                        centraltermica.SSAA = strValor;
                        break;
                    case 1524:
                        centraltermica.Dunifil = strValor;
                        break;
                    case 1816:
                        centraltermica.PotEfecCont = strValor;
                        break;
                }
            }
            return View(centraltermica);
        }
        public ActionResult DatosGeneradorT(string id, string iFamilia)
        {
            var Equipo = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Central = appEquipamiento.GetByIdEqEquipo(Equipo.Equipadre.Value);
            var NombreCentral = Central.Equinomb;
            var NombreEmpresa = Central.EMPRNOMB;

            var GeneradorTermico = new DetalleGeneradorTermicoViewModel();
            GeneradorTermico.CodCentral = Central.Equicodi.ToString(); //Central.EQUINOMB;
            GeneradorTermico.CodGrupo = Equipo.Equinomb;
            GeneradorTermico.NombreCentral = NombreCentral;
            GeneradorTermico.NombreEmpresa = NombreEmpresa;

            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));

            foreach (var propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1555:
                        GeneradorTermico.DatPlaca = strValor;
                        break;
                    case 1556:
                        GeneradorTermico.Moper = strValor;
                        break;
                    case ConstantesAppServicio.PropiedadOperacionComercial:
                        GeneradorTermico.IngCom = propiedadEquipoHistDto.FechapropequiDesc;
                        break;
                    case 1558:
                        GeneradorTermico.Tecnolog = strValor;
                        break;
                    case 1559:
                        GeneradorTermico.Tipo = strValor;
                        break;
                    case 1560:
                        GeneradorTermico.Fabr = strValor;
                        break;
                    case 1561:
                        GeneradorTermico.Modelo = strValor;
                        break;
                    case 1562:
                        GeneradorTermico.Serie = strValor;
                        break;
                    case 1563:
                        GeneradorTermico.Pinst = strValor;
                        break;
                    case 189:
                        GeneradorTermico.Pnom = strValor;
                        break;
                    case 313:
                        GeneradorTermico.Snom = strValor;
                        break;
                    case 980:
                        GeneradorTermico.Pnom_30ps = strValor;
                        break;
                    case 1564:
                        GeneradorTermico.Pnom_15ps = strValor;
                        break;
                    case 979:
                        GeneradorTermico.Pnom_05ps = strValor;
                        break;
                    case 1565:
                        GeneradorTermico.Psincr = strValor;
                        break;
                    case 354:
                        GeneradorTermico.RPM = strValor;
                        break;
                    case 1566:
                        GeneradorTermico.GNaDB5 = strValor;
                        break;
                    case 1567:
                        GeneradorTermico.DB5aGN = strValor;
                        break;
                    case 1568:
                        GeneradorTermico.DB5aRes = strValor;
                        break;
                    case 1569:
                        GeneradorTermico.ResaDB5 = strValor;
                        break;
                    case 1570:
                        GeneradorTermico.PresVap = strValor;
                        break;
                    case 1571:
                        GeneradorTermico.TempVap = strValor;
                        break;
                    case 1572:
                        GeneradorTermico.CaudVap = strValor;
                        break;
                    case 1573:
                        GeneradorTermico.PresVacCond = strValor;
                        break;
                    case 1574:
                        GeneradorTermico.NumCalent = strValor;
                        break;
                    case 1575:
                        GeneradorTermico.NumExtracc = strValor;
                        break;
                    case 1576:
                        GeneradorTermico.NumetapComp = strValor;
                        break;
                    case 1577:
                        GeneradorTermico.NumetapTurb = strValor;
                        break;
                    case 1578:
                        GeneradorTermico.TempAir = strValor;
                        break;
                    case 1579:
                        GeneradorTermico.PresAir = strValor;
                        break;
                    case 1580:
                        GeneradorTermico.FlujoGases = strValor;
                        break;
                    case 1581:
                        GeneradorTermico.TempGases = strValor;
                        break;
                    case 1582:
                        GeneradorTermico.DBCF = strValor;
                        break;
                    case 1583:
                        GeneradorTermico.PotGener = strValor;
                        break;
                    case 1584:
                        GeneradorTermico.VelGener = strValor;
                        break;
                    case 1585:
                        GeneradorTermico.VelEmbGener = strValor;
                        break;
                    case 1586:
                        GeneradorTermico.Numpolos = strValor;
                        break;
                    case 196:
                        GeneradorTermico.Curva_Cap = strValor;
                        break;
                    case 195:
                        GeneradorTermico.G_capac_min = strValor;
                        break;
                    case 198:
                        GeneradorTermico.G_capac_50 = strValor;
                        break;
                    case 200:
                        GeneradorTermico.G_capac_100 = strValor;
                        break;
                    case 192:
                        GeneradorTermico.G_ind_min = strValor;
                        break;
                    case 199:
                        GeneradorTermico.G_ind_50 = strValor;
                        break;
                    case 201:
                        GeneradorTermico.G_ind_100 = strValor;
                        break;
                    case 1001:
                        GeneradorTermico.Vn_GEN = strValor;
                        break;
                    case 193:
                        GeneradorTermico.Vmin_GEN = strValor;
                        break;
                    case 194:
                        GeneradorTermico.Vmax_GEN = strValor;
                        break;
                    case 208:
                        GeneradorTermico.Vmin_Exc = strValor;
                        break;
                    case 209:
                        GeneradorTermico.Vmax_Exc = strValor;
                        break;
                    case 1002:
                        GeneradorTermico.Vmin_SSAA = strValor;
                        break;
                    case 1003:
                        GeneradorTermico.Vmax_SSAA = strValor;
                        break;
                    case 190:
                        GeneradorTermico.ARR_BS = strValor;
                        break;
                    case 336:
                        GeneradorTermico.ReactanciasincrónicaEjeDirecto = strValor;
                        break;
                    case 337:
                        GeneradorTermico.ReactanciaTransitoriaEjeDirecto = strValor;
                        break;
                    case 338:
                        GeneradorTermico.ReactanciaSubtransitoriaEjeDirecto = strValor;
                        break;
                    case 339:
                        GeneradorTermico.X2 = strValor;
                        break;
                    case 340:
                        GeneradorTermico.Xo = strValor;
                        break;
                    case 341:
                        GeneradorTermico.Ro = strValor;
                        break;
                    case 342:
                        GeneradorTermico.R2 = strValor;
                        break;
                    case 343:
                        GeneradorTermico.Xn = strValor;
                        break;
                    case 344:
                        GeneradorTermico.Rn = strValor;
                        break;
                    case 355:
                        GeneradorTermico.H = strValor;
                        break;
                    case 375:
                        GeneradorTermico.ReactanciaSincrónicaEjeCuadratura = strValor;
                        break;
                    case 376:
                        GeneradorTermico.ReactanciaTransitoriaEjeCuadratura = strValor;
                        break;
                    case 377:
                        GeneradorTermico.ReactanciaSubtransitoriaEjeCuadratura = strValor;
                        break;
                    case 378:
                        GeneradorTermico.CteTtiempoTranEjeDirectCircAbierto = strValor;
                        break;
                    case 379:
                        GeneradorTermico.CteTiempoSubtransitoriaEjeDirCircAbierto = strValor;
                        break;
                    case 380:
                        GeneradorTermico.CteTiempoTransitoriaEjeDirCortocircuito = strValor;
                        break;
                    case 381:
                        GeneradorTermico.CteTiempoSubtransitoriaEjeDirCortocircuito = strValor;
                        break;
                    case 382:
                        GeneradorTermico.CteTiempoTransitoriaEjeCuadraturaCircuitoAbiert = strValor;
                        break;
                    case 383:
                        GeneradorTermico.CteTiempoSubtransitoriaEjeCuadraturaCircuitoAbierto = strValor;
                        break;
                    case 384:
                        GeneradorTermico.Ta = strValor;
                        break;
                    case 507:
                        GeneradorTermico.GD2 = strValor;
                        break;
                    case 508:
                        GeneradorTermico.Xp = strValor;
                        break;
                    case 509:
                        GeneradorTermico.XL = strValor;
                        break;
                    case 510:
                        GeneradorTermico.CteTiempoSubtransitoriaEjeCuadraturaCortocircuito = strValor;
                        break;
                    case 511:
                        GeneradorTermico.Ra20C = strValor;
                        break;
                    case 512:
                        GeneradorTermico.SCR = strValor;
                        break;
                    case 513:
                        GeneradorTermico.S10 = strValor;
                        break;
                    case 514:
                        GeneradorTermico.S12 = strValor;
                        break;
                    case 1005:
                        GeneradorTermico.Prot_sob_f = strValor;
                        break;
                    case 1008:
                        GeneradorTermico.CteTiempoTransitoriaEjeCuadraturaCortocircuito = strValor;
                        break;
                    case 1009:
                        GeneradorTermico.Iexco_1pu = strValor;
                        break;
                    case 1010:
                        GeneradorTermico.Iexco_12p = strValor;
                        break;
                    case 1011:
                        GeneradorTermico.DBRTE = strValor;
                        break;
                    case 1012:
                        GeneradorTermico.DBRVT = strValor;
                        break;
                    case 1013:
                        GeneradorTermico.DBPSS = strValor;
                        break;
                    case 1587:
                        GeneradorTermico.TipCald = strValor;
                        break;
                    case 1588:
                        GeneradorTermico.ProdVapCald = strValor;
                        break;
                    case 1589:
                        GeneradorTermico.PresVapCald = strValor;
                        break;
                    case 1590:
                        GeneradorTermico.TempVapCald = strValor;
                        break;
                    case 1591:
                        GeneradorTermico.CombTC = strValor;
                        break;
                    case 1592:
                        GeneradorTermico.TipQuem = strValor;
                        break;
                    case 1593:
                        GeneradorTermico.NumQuem = strValor;
                        break;
                    case 1594:
                        GeneradorTermico.RendTerm = strValor;
                        break;
                    case 1595:
                        GeneradorTermico.TSF = strValor;
                        break;
                    case 1596:
                        GeneradorTermico.ProgMantos = strValor;
                        break;
                    case 1597:
                        GeneradorTermico.RegVel = strValor;
                        break;
                    case 1598:
                        GeneradorTermico.ModoTrab = strValor;
                        break;
                    case 1599:
                        GeneradorTermico.ModoContr = strValor;
                        break;
                    case 1007:
                        GeneradorTermico.BandaMuer = strValor;
                        break;
                    case 291:
                        GeneradorTermico.EstaAct = strValor;
                        break;
                    case 1600:
                        GeneradorTermico.ValSistAisl = strValor;
                        break;
                    case 292:
                        GeneradorTermico.RanVarEsta = strValor;
                        break;
                    case 1805:
                        GeneradorTermico.V_MinConArr = strValor;
                        break;
                }
            }
            return View(GeneradorTermico);
        }
        public ActionResult DatosModoOperacion(string equicodi, string grupocodi, string central)
        {
            var oCentral = appEquipamiento.GetByIdEqEquipo(int.Parse(central));
            string sCombustible = string.Empty;
            //int iEquiCodiGen = 0;
            //var oGenerador= new EquipoDTO();
            //if (equicodi.Trim() == "-1")
            //{
            //    var oGeneradorHijo = appEquipo.ListarEquipoPorPadre(oCentral.EQUICODI).First();
            //    oGenerador = appEquipo.TraerEquipo(oGeneradorHijo.EQUICODI);
            //}
            //else
            //{
            //    oGenerador = appEquipo.TraerEquipo(int.Parse(equicodi));
            //}


            //var iGrupoCodiGenerador = oGenerador.GRUPOCODI;
            var oModoOperacion = appDespacho.GetByIdPrGrupo(int.Parse(grupocodi));//Modo de Operación

            var iGrupoCodi = appDespacho.ObtenerCodigoModoOperacionPadre(int.Parse(grupocodi));//Obtenemos Código de generador o grupo
            var lsValoresMOGrupo = appDespacho.ListadoValoresModoOperacion(int.Parse(central), iGrupoCodi);//Datos de Grupo logico o Generador

            var iGrupoCodiCentral = appDespacho.ObtenerCodigoModoOperacionPadre(iGrupoCodi);//Obtenemos Código de central
            var lsValoresMOCentral = appDespacho.ListadoValoresModoOperacion(int.Parse(central), iGrupoCodiCentral); //Valores de MO de las central


            var NombreCentral = oCentral.Equinomb;
            var NombreEmpresa = oCentral.EMPRNOMB;

            var ValoresMO = appDespacho.ListadoValoresModoOperacion(int.Parse(equicodi), int.Parse(grupocodi));//Datos de Modo de Operación
            var Modelo = new DetalleModoOperacionViewModel();
            Modelo.TextoCostoTransporte = "Costo de transporte";
            Modelo.TextoCostoTratamientoMecanico = "Costo de tratamiento mecánico de combustibles";
            Modelo.TextoCostoTratamientoQuimico = "Costo de tratamiento químico de combustibles ";

            Modelo.CodCentral = oCentral.Equicodi.ToString();//oCentral.EQUINOMB;

            Modelo.NombreCentral = NombreCentral;
            Modelo.NombreEmpresa = NombreEmpresa;

            #region Caso Modo Operacion ILO2CARB
            Modelo.EsIlo2 = false;
            if (grupocodi == "257")
            {
                Modelo.EsIlo2 = true;
                Modelo.UnidadIlo2 = "[kg]";
                Modelo.CombustibleIlo2 = "";
            }

            #endregion

            PrConceptoDTO conceptoDto = new PrConceptoDTO();

            //conceptoDto = appDespacho.GetByIdPrConcepto(98);
            //Modelo.CombATM_uni = "["+conceptoDto.CONCEPUNID+"]";
            //conceptoDto = appConcepto.TraerConcepto(149);
            //Modelo.Comb_arr_sinc_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(150);
            //Modelo.Comb_arr_sinc_F1_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(152);
            //Modelo.Comb_arr_sinc_int_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(153);
            //Modelo.Comb_arr_sinc_cal_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(154);
            //Modelo.Comb_sinc_PC_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(155);
            //Modelo.Comb_sinc_PC_F1_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(157);
            //Modelo.Comb_sinc_PC_int_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(158);
            //Modelo.Comb_sinc_PC_cal_uni = conceptoDto.CONCEPUNID;
            //conceptoDto = appConcepto.TraerConcepto(79);
            //Modelo.CombPRD_uni = "[" + conceptoDto.CONCEPUNID.Trim() + "]";
            //conceptoDto = appConcepto.TraerConcepto(159);
            //Modelo.Comb_PC_sinc_uni = "[" + conceptoDto.CONCEPUNID + "]";
            //conceptoDto = appConcepto.TraerConcepto(160);
            //Modelo.Comb_sinc_par_uni = "[" + conceptoDto.CONCEPUNID + "]";
            //conceptoDto = appConcepto.TraerConcepto(17);
            //Modelo.EficTerm_uni = conceptoDto.CONCEPUNID;
            conceptoDto = appDespacho.GetByIdPrConcepto(193);
            Modelo.EficBTUKWh_uni = "[" + conceptoDto.Concepunid + "]";

            //var CMarr = lsValoresMOGrupo.SingleOrDefault(mo => mo.CONCEPCODI == 80);
            var CMarr = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 186);
            Modelo.CMarr = CMarr == null ? "" : CMarr.VALOR;
            foreach (var Valor in ValoresMO)
            {

                switch ((int)Valor.CONCEPCODI)
                {
                    case -99:
                        Modelo.CodModo = Valor.VALOR;
                        break;
                    case 14:
                        Modelo.Pe = Valor.VALOR;
                        break;
                    case 16:
                        Modelo.Pmin = Valor.VALOR;
                        break;
                    case 15:
                        Modelo.Pmax = Valor.VALOR;
                        break;
                    case 115:
                        Modelo.V_tomacarga = Valor.VALOR;
                        break;
                    case 116:
                        Modelo.V_TC_frio1 = Valor.VALOR;
                        break;
                    case 117:
                        Modelo.V_TC_frio2 = Valor.VALOR;
                        break;
                    case 118:
                        Modelo.V_TC_intca = Valor.VALOR;
                        break;
                    case 119:
                        Modelo.V_TC_calie = Valor.VALOR;
                        break;
                    case 120:
                        Modelo.V_descarga = Valor.VALOR;
                        break;
                    case 121:
                        Modelo.T_sinc = Valor.VALOR;
                        break;
                    case 122:
                        Modelo.T_sincronizacionFrio1 = Valor.VALOR;
                        break;
                    case 123:
                        Modelo.T_sinc_F2 = Valor.VALOR;
                        break;
                    case 124:
                        Modelo.T_sincronizaciónintermedio = Valor.VALOR;
                        break;
                    case 125:
                        Modelo.T_sincronizacióncaliente = Valor.VALOR;
                        break;
                    case 126:
                        Modelo.T_PC_Sinc = Valor.VALOR;
                        break;
                    case 127:
                        Modelo.T_PC_F1 = Valor.VALOR;
                        break;
                    case 128:
                        Modelo.T_PC_F2 = Valor.VALOR;
                        break;
                    case 129:
                        Modelo.T_CargaIntermedio = Valor.VALOR;
                        break;
                    case 130:
                        Modelo.T_PC_Cal = Valor.VALOR;
                        break;
                    case 131:
                        Modelo.T_SFSP = Valor.VALOR;
                        break;
                    case 132:
                        Modelo.T_PC_pm = Valor.VALOR;
                        break;
                    case 133:
                        Modelo.T_ArrNegr = Valor.VALOR;
                        break;
                    case 134:
                        Modelo.T_fuera_sinc = Valor.VALOR;
                        break;
                    case 135:
                        Modelo.T_sinc_par = Valor.VALOR;
                        break;
                    case 136:
                        Modelo.T_min_Arr = Valor.VALOR;
                        break;
                    case 137:
                        Modelo.T_min_Arr_eme = Valor.VALOR;
                        break;
                    case 138:
                        Modelo.MaxPotMin = Valor.VALOR;
                        break;
                    case 139:
                        Modelo.Tmin_op = Valor.VALOR;
                        break;
                    case 140:
                        Modelo.Ene_sinc = Valor.VALOR;
                        break;
                    case 141:
                        Modelo.Ene_sinc_F1 = Valor.VALOR;
                        break;
                    case 142:
                        Modelo.Ene_sinc_F2 = Valor.VALOR;
                        break;
                    case 143:
                        Modelo.Ene_sinc_int = Valor.VALOR;
                        break;
                    case 144:
                        Modelo.Ene_sinc_cal = Valor.VALOR;
                        break;
                    case 145:
                        Modelo.Ene_PC_sinc = Valor.VALOR;
                        break;
                    case 146:
                        sCombustible = Modelo.TipComb = Valor.VALOR;
                        ConceptoDatoDTO DatoComb;
                        ConceptoDatoDTO conDato = new ConceptoDatoDTO();
                        switch (Valor.VALOR.Trim().ToUpperInvariant())
                        {

                            case "GAS":
                            case "GAS NATURAL":
                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 77);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 194);
                                Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                //Modelo.HHV_uni = "[Btu/pc]";
                                //Modelo.LHV_uni = "[Btu/pc]";
                                Modelo.HHV_uni = "[KJ/m3]";
                                Modelo.LHV_uni = "[KJ/m3]";

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 72);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 192);
                                Modelo.LHV = DatoComb == null ? "" : DatoComb.VALOR;
                                //conceptoDto = appConcepto.TraerConcepto(72);


                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 61);
                                //if (oModoOperacion.Grupocodi == 280)
                                //    DatoComb = lsValoresMOGrupo.SingleOrDefault(mo => mo.CONCEPCODI == 540);
                                //else


                                ////if (int.Parse(grupocodi) != 281)
                                ////{
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 191);
                                Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                //conceptoDto = appDespacho.GetByIdPrConcepto(61);
                                conceptoDto = appDespacho.GetByIdPrConcepto(191);
                                Modelo.PrecioCComb_uni = conceptoDto.Concepunid;
                                ////}
                                ////else
                                ////{
                                ////    DatoComb = lsValoresMOGrupo.SingleOrDefault(mo => mo.CONCEPCODI == 540);
                                ////    Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                ////    conceptoDto = appDespacho.GetByIdPrConcepto(540);
                                ////    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                ////}


                                Modelo.Comb_arr_sinc_uni = "[m3]";
                                Modelo.Comb_arr_sinc_F1_uni = "[m3]";
                                Modelo.Comb_arr_sinc_int_uni = "[m3]";
                                Modelo.Comb_arr_sinc_cal_uni = "[m3]";
                                Modelo.Comb_sinc_PC_uni = "[m3]";
                                Modelo.Comb_sinc_PC_F1_uni = "[m3]";
                                Modelo.Comb_sinc_PC_int_uni = "[m3]";
                                Modelo.Comb_sinc_PC_cal_uni = "[m3]";
                                Modelo.CombPRD_uni = "[m3]";
                                Modelo.Comb_PC_sinc_uni = "[m3]";
                                Modelo.Comb_sinc_par_uni = "[m3]";
                                Modelo.CombATM_uni = "[m3]";
                                Modelo.Consumo_uni = "[m3/h]";

                                Modelo.EficTerm_uni = "[kWh/m3]";// "[kWh/pc]";

                                //Modelo.PrecioCComb_uni = "[S/./MMBtu]";
                                Modelo.CostoTransCComb_uni = "[S/./GJ]";
                                Modelo.CostoTratMecCComb_uni = "[S/./GJ]";
                                Modelo.CostoTratQuiCComb_uni = "[S/./GJ]";

                                Modelo.HHV_abrev = "(HHVg)";
                                Modelo.LHV_abrev = "(LHVg)";

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 191);//En caso Gas se usa Total Costo Combustible GAS 61
                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;

                                break;
                            case "DIESEL B5 S-50":
                            case "DIESEL B5":
                            case "DIESEL":
                            case "D2":

                                #region GENERAL PARA COMBUSTIBLES LIQUIDOS
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 187);
                                //Modelo.a = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 188);
                                //Modelo.b = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 189);
                                //Modelo.c = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 190);
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);
                                Modelo.EficTerm = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);
                                Modelo.EficBTUKWh = conDato.VALOR;
                                #endregion

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 69);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 215);
                                Modelo.LHV = DatoComb == null ? "" : DatoComb.VALOR;
                                //conceptoDto = appDespacho.GetByIdPrConcepto(69);
                                Modelo.HHV_uni = "[kJ/kg]";
                                Modelo.LHV_uni = "[kJ/kg]";
                                //Costo Combustible
                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 207);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 531);
                                Modelo.CostoTotalComb = DatoComb == null ? "" : DatoComb.VALOR;
                                Modelo.CostoTotalComb_uni = "[S/./l]";
                                ///
                                /// 
                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 42);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 521);
                                Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(521);

                                try
                                {
                                    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }


                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 44);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 522);
                                Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(522);
                                try
                                {
                                    Modelo.CostoTransCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTransCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 45);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 523);
                                Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(523);

                                try
                                {
                                    Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 46);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 524);
                                Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(524);

                                try
                                {
                                    Modelo.CostoTratQuiCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTratQuiCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 47);
                                Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;

                                Modelo.Comb_arr_sinc_uni = "[l]";
                                Modelo.Comb_arr_sinc_F1_uni = "[l]";
                                Modelo.Comb_arr_sinc_int_uni = "[l]";
                                Modelo.Comb_arr_sinc_cal_uni = "[l]";
                                Modelo.Comb_sinc_PC_uni = "[l]";
                                Modelo.Comb_sinc_PC_F1_uni = "[l]";
                                Modelo.Comb_sinc_PC_int_uni = "[l]";
                                Modelo.Comb_sinc_PC_cal_uni = "[l]";
                                Modelo.CombPRD_uni = "[l]";
                                Modelo.Comb_PC_sinc_uni = "[l]";
                                Modelo.Comb_sinc_par_uni = "[l]";
                                Modelo.CombATM_uni = "[l]";
                                Modelo.Consumo_uni = "[l/h]";

                                Modelo.EficTerm_uni = "[kWh/l]";

                                //Modelo.PrecioCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTransCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTratMecCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTratQuiCComb_uni = "[S/./Bbl]";

                                Modelo.LHV_abrev = "(PCalD2)";

                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 507);
                                Modelo.Densidad = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(507);
                                try
                                {
                                    Modelo.Densidad_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.Densidad_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                break;

                            case "R500":
                            case "RESIDUAL 500":

                                #region GENERAL PARA COMBUSTIBLES LIQUIDOS
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 187);
                                //Modelo.a = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 188);
                                //Modelo.b = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 189);
                                //Modelo.c = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 190);
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);
                                Modelo.EficTerm = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);
                                Modelo.EficBTUKWh = conDato.VALOR;
                                #endregion

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 71);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 217);
                                Modelo.LHV = DatoComb == null ? "" : DatoComb.VALOR;
                                //conceptoDto = appDespacho.GetByIdPrConcepto(71);
                                Modelo.HHV_uni = "[kJ/kg]";
                                Modelo.LHV_uni = "[kJ/kg]";

                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 509);
                                Modelo.Densidad = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(509);
                                Modelo.Densidad_uni = "[" + conceptoDto.Concepunid + "]";

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 55);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 533);
                                Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(533);

                                try
                                {
                                    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                //Costo Combustible
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 538);
                                Modelo.CostoTotalComb = DatoComb == null ? "" : DatoComb.VALOR;
                                Modelo.CostoTotalComb_uni = "[S/./l]";

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 56);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 534);
                                Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(534);

                                try
                                {
                                    Modelo.CostoTransCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTransCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }


                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 57);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 535);
                                Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(535);

                                try
                                {
                                    Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 58);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 536);
                                Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(536);
                                Modelo.CostoTratQuiCComb_uni = "[" + conceptoDto.Concepunid + "]";

                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 59);
                                Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;

                                Modelo.Comb_arr_sinc_uni = "[l]";
                                Modelo.Comb_arr_sinc_F1_uni = "[l]";
                                Modelo.Comb_arr_sinc_int_uni = "[l]";
                                Modelo.Comb_arr_sinc_cal_uni = "[l]";
                                Modelo.Comb_sinc_PC_uni = "[l]";
                                Modelo.Comb_sinc_PC_F1_uni = "[l]";
                                Modelo.Comb_sinc_PC_int_uni = "[l]";
                                Modelo.Comb_sinc_PC_cal_uni = "[l]";
                                Modelo.CombPRD_uni = "[l]";
                                Modelo.Comb_PC_sinc_uni = "[l]";
                                Modelo.Comb_sinc_par_uni = "[l]";
                                Modelo.CombATM_uni = "[l]";
                                Modelo.Consumo_uni = "[l/h]";

                                Modelo.EficTerm_uni = "[kWh/gal]";

                                //Modelo.PrecioCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTransCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTratMecCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTratQuiCComb_uni = "[S/./Bbl]";

                                Modelo.LHV_abrev = "(PCalR500)";
                                break;
                            //case "GAS NATURAL":
                            //    Modelo.Comb_arr_sinc_uni = "[m3]";
                            //    Modelo.Comb_arr_sinc_F1_uni = "[m3]";
                            //    Modelo.Comb_arr_sinc_int_uni = "[m3]";
                            //    Modelo.Comb_arr_sinc_cal_uni = "[m3]";
                            //    Modelo.Comb_sinc_PC_uni = "[m3]";
                            //    Modelo.Comb_sinc_PC_F1_uni = "[m3]";
                            //    Modelo.Comb_sinc_PC_int_uni = "[m3]";
                            //    Modelo.Comb_sinc_PC_cal_uni = "[m3]";
                            //    Modelo.CombPRD_uni = "[m3]";
                            //    Modelo.Comb_PC_sinc_uni = "[m3]";
                            //    Modelo.Comb_sinc_par_uni = "[m3]";
                            //    Modelo.CombATM_uni = "[m3]";



                            //    Modelo.HHV_uni = "[Btu/pc]";
                            //    Modelo.LHV_uni = "[Btu/pc]";

                            //    Modelo.EficTerm_uni = "[kWh/pc]";

                            //    Modelo.PrecioCComb_uni = "[S/./MMBtu]";
                            //    Modelo.CostoTransCComb_uni = "[S/./MMBtu]";
                            //    Modelo.CostoTratMecCComb_uni = "[S/./MMBtu]";
                            //    Modelo.CostoTratQuiCComb_uni = "[S/./MMBtu]";

                            //    Modelo.HHV_abrev = "(HHVg)";
                            //    Modelo.LHV_abrev = "(LHVg)";

                            //    DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 61);//En caso Gas se usa Total Costo Combustible GAS
                            //    Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;


                            //    break;
                            case "BAGAZO":
                                Modelo.HHV_uni = "[kJ/kg]";
                                Modelo.LHV_uni = "[kJ/kg]";
                                Modelo.EficTerm_uni = "[kWh/kg]";
                                Modelo.EficBTUKWh_uni = "[kJ/kWh]";

                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);
                                Modelo.EficTerm = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);
                                Modelo.EficBTUKWh = conDato.VALOR;

                                Modelo.CostoTotalComb_uni = "[S/./kg]";
                                Modelo.PrecioCComb_uni = "[S/./kg]";
                                Modelo.CostoTransCComb_uni = "[S/./kg]";
                                Modelo.CostoTratMecCComb_uni = "[S/./kg]";
                                Modelo.CostoTratQuiCComb_uni = "[S/./kg]";

                                Modelo.CombPRD_uni = "[kg]";
                                Modelo.CombATM_uni = "[kg]";

                                break;
                            case "BIOGAS":
                                Modelo.HHV_uni = "[kJ/m3]";
                                Modelo.LHV_uni = "[kJ/m3]";

                                Modelo.Comb_arr_sinc_uni = "[m3]";
                                Modelo.Comb_arr_sinc_F1_uni = "[m3]";
                                Modelo.Comb_arr_sinc_int_uni = "[m3]";
                                Modelo.Comb_arr_sinc_cal_uni = "[m3]";
                                Modelo.Comb_sinc_PC_uni = "[m3]";
                                Modelo.Comb_sinc_PC_F1_uni = "[m3]";
                                Modelo.Comb_sinc_PC_int_uni = "[m3]";
                                Modelo.Comb_sinc_PC_cal_uni = "[m3]";
                                Modelo.CombPRD_uni = "[m3]";
                                Modelo.Comb_PC_sinc_uni = "[m3]";
                                Modelo.Comb_sinc_par_uni = "[m3]";
                                Modelo.CombATM_uni = "[m3]";
                                Modelo.Consumo_uni = "[m3/h]";

                                Modelo.EficTerm_uni = "[kWh/m3]";// "[kWh/pc]";
                                Modelo.CostoTotalComb_uni = "[S/./m3]";
                                Modelo.PrecioCComb_uni = "[S/./m3]";
                                Modelo.CostoTransCComb_uni = "[S/./m3]";
                                Modelo.CostoTratMecCComb_uni = "[S/./m3]";
                                Modelo.CostoTratQuiCComb_uni = "[S/./m3]";

                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);
                                Modelo.EficTerm = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);
                                Modelo.EficBTUKWh = conDato.VALOR;
                                break;

                            case "R6":
                            case "RESIDUAL 6":

                                #region GENERAL PARA COMBUSTIBLES LIQUIDOS
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 187);
                                //Modelo.a = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 188);
                                //Modelo.b = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 189);
                                //Modelo.c = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);
                                Modelo.EficTerm = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);
                                Modelo.EficBTUKWh = conDato.VALOR;
                                #endregion

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 70);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 216);
                                Modelo.LHV = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(216);
                                Modelo.HHV_uni = "[" + conceptoDto.Concepunid + "]";
                                Modelo.LHV_uni = "[kJ/kg]";

                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 508);
                                Modelo.Densidad = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(508);
                                Modelo.Densidad_uni = "[" + conceptoDto.Concepunid + "]";

                                //Costo Combustible
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 532);
                                Modelo.CostoTotalComb = DatoComb == null ? "" : DatoComb.VALOR;
                                Modelo.CostoTotalComb_uni = "[S/./l]";

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 48);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 526);
                                Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(526);

                                try
                                {
                                    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 49);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 527);
                                Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(527);
                                Modelo.CostoTransCComb_uni = "[" + conceptoDto.Concepunid + "]";

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 50);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 528);
                                Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(528);

                                try
                                {
                                    Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 51);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 529);
                                Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(529);

                                try
                                {
                                    Modelo.CostoTratQuiCComb_uni = "[" + conceptoDto.Concepunid.Trim() + "]";
                                }
                                catch
                                {
                                    Modelo.CostoTratQuiCComb_uni = "[" + conceptoDto.Concepunid + "]";
                                }

                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 52);
                                Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;

                                Modelo.Comb_arr_sinc_uni = "[l]";
                                Modelo.Comb_arr_sinc_F1_uni = "[l]";
                                Modelo.Comb_arr_sinc_int_uni = "[l]";
                                Modelo.Comb_arr_sinc_cal_uni = "[l]";
                                Modelo.Comb_sinc_PC_uni = "[l]";
                                Modelo.Comb_sinc_PC_F1_uni = "[l]";
                                Modelo.Comb_sinc_PC_int_uni = "[l]";
                                Modelo.Comb_sinc_PC_cal_uni = "[l]";
                                Modelo.CombPRD_uni = "[l]";
                                Modelo.Comb_PC_sinc_uni = "[l]";
                                Modelo.Comb_sinc_par_uni = "[l]";
                                Modelo.CombATM_uni = "[l]";
                                Modelo.Consumo_uni = "[l/h]";

                                //Modelo.EficTerm_uni = "[kWh/gal]";
                                //Modelo.PrecioCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTransCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTratMecCComb_uni = "[S/./Bbl]";
                                //Modelo.CostoTratQuiCComb_uni = "[S/./Bbl]";

                                Modelo.LHV_abrev = "(PCalR6)";
                                break;
                            case "CARBON":
                            case "CARBÓN":

                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 187);
                                //Modelo.a = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 188);
                                //Modelo.b = conDato.VALOR;
                                //conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 189);
                                //Modelo.c = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);
                                Modelo.EficTerm = conDato.VALOR;
                                conDato = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);
                                Modelo.EficBTUKWh = conDato.VALOR;


                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 73);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 222);
                                Modelo.LHV = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(222);
                                Modelo.HHV_uni = "[" + conceptoDto.Concepunid + "]";
                                Modelo.LHV_uni = "[kJ/kg]";

                                //Costo Combustible
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 221);
                                Modelo.CostoTotalComb = DatoComb == null ? "" : DatoComb.VALOR;
                                Modelo.CostoTotalComb_uni = "[S/./kg]";

                                //DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 63);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 218);
                                Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(218);
                                Modelo.PrecioCComb_uni = "[" + conceptoDto.Concepunid + "]";

                                //DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 64);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 219);
                                Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(219);
                                Modelo.CostoTransCComb_uni = "[" + conceptoDto.Concepunid + "]";

                                //DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 65);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 223);
                                Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(223);
                                Modelo.CostoTratMecCComb_uni = "[" + conceptoDto.Concepunid + "]";

                                //DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 66);
                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 224);
                                Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(224);
                                Modelo.CostoTratQuiCComb_uni = "[" + conceptoDto.Concepunid + "]";


                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 67);
                                Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;


                                Modelo.Comb_arr_sinc_uni = "[kg]";
                                Modelo.Comb_arr_sinc_F1_uni = "[kg]";
                                Modelo.Comb_arr_sinc_int_uni = "[kg]";
                                Modelo.Comb_arr_sinc_cal_uni = "[kg]";
                                Modelo.Comb_sinc_PC_uni = "[kg]";
                                Modelo.Comb_sinc_PC_F1_uni = "[kg]";
                                Modelo.Comb_sinc_PC_int_uni = "[kg]";
                                Modelo.Comb_sinc_PC_cal_uni = "[kg]";
                                Modelo.CombPRD_uni = "[kg]";
                                Modelo.Comb_PC_sinc_uni = "[kg]";
                                Modelo.Comb_sinc_par_uni = "[kg]";
                                Modelo.CombATM_uni = "[kg]";
                                Modelo.Consumo_uni = "[kg/h]";


                                Modelo.EficTerm_uni = "[kWh/kg]";

                                //Modelo.PrecioCComb_uni = "[S/./kg]";
                                //Modelo.CostoTransCComb_uni = "[S/./kg]";
                                //Modelo.CostoTratMecCComb_uni = "[S/./kg]";
                                //Modelo.CostoTratQuiCComb_uni = "[S/./kg]";

                                Modelo.LHV_abrev = "(PCalCarb)";

                                Modelo.TextoCostoTransporte = "Costo de Seguros y Fletes Marítimos Carbón";
                                Modelo.TextoCostoTratamientoMecanico = "Costo Aduanas y Desaduanaje Carbón";
                                Modelo.TextoCostoTratamientoQuimico = "Costo Embarques y Fletes Terrestres Carbón";

                                break;
                            default:
                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 97);
                                Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                conceptoDto = appDespacho.GetByIdPrConcepto(97);
                                Modelo.HHV_uni = "[" + conceptoDto.Concepunid + "]";
                                break;

                        }
                        break;
                    //case 147:
                    //    Modelo.ge = Valor.VALOR;
                    //    break;
                    case 148:
                        Modelo.TempComb = Valor.VALOR;
                        break;
                    case 187:
                        Modelo.a = Valor.VALOR;
                        break;
                    case 188:
                        Modelo.b = Valor.VALOR;
                        break;
                    case 189:
                        Modelo.c = Valor.VALOR;
                        break;
                    case 78:
                        Modelo.CombATM = Valor.VALOR;

                        break;
                    /*
                case 149:
                    Modelo.Comb_arr_sinc = Valor.VALOR;

                    break;
                    */
                    /*
                    case 150:
                        Modelo.Comb_arr_sinc_F1 = Valor.VALOR;

                        break;*/
                    /*
                case 151:
                    Modelo.Comb_arr_sinc_F2 = Valor.VALOR;
                    break;*/
                    /*
                case 152:
                    Modelo.Comb_arr_sinc_int = Valor.VALOR;

                    break;
                case 153:
                    Modelo.Comb_arr_sinc_cal = Valor.VALOR;

                    break;*/
                    /*
                case 154:
                    Modelo.Comb_sinc_PC = Valor.VALOR;

                    break;
                    */
                    /*
                    case 155:
                        Modelo.Comb_sinc_PC_F1 = Valor.VALOR;

                        break;
                        */
                    /*
                case 156:
                    Modelo.Comb_sinc_PC_F2 = Valor.VALOR;
                    break;
                case 157:
                    Modelo.Comb_sinc_PC_int = Valor.VALOR;

                    break;
                case 158:
                    Modelo.Comb_sinc_PC_cal = Valor.VALOR;

                    break;*/
                    case 79:
                        Modelo.CombPRD = Valor.VALOR;

                        break;
                    /*
                case 159:
                    Modelo.Comb_PC_sinc = Valor.VALOR;

                    break;
                    */
                    /*
                    case 154:
                        Modelo.Comb_sinc_PC = Valor.VALOR;

                        break; */
                    /*
                case 160:
                    Modelo.Comb_sinc_par = Valor.VALOR;

                    break;
                    */
                    //case 17:
                    //    Modelo.EficTerm = Valor.VALOR;
                    //    break;
                    //case 74:
                    //    Modelo.EficBTUKWh = Valor.VALOR;
                    //    break;
                    case 27:
                        Modelo.CVC = Valor.VALOR;
                        break;
                    case 62:
                        Modelo.CVNC = Valor.VALOR;
                        break;
                    case 161:
                        Modelo.CVONC = Valor.VALOR;
                        break;
                    case 162:
                        Modelo.CVM = Valor.VALOR;
                        break;
                    //case 80:
                    //    Modelo.CMarr = Valor.VALOR;
                    //    break;
                    case 163:
                        Modelo.SSAA = Valor.VALOR;
                        break;
                    case 164:
                        Modelo.FDP = Valor.VALOR;
                        break;
                    case 167:
                        Modelo.C_Arr_Par_f1 = Valor.VALOR;
                        break;
                    case 168:
                        Modelo.C_Arr_Par_f2 = Valor.VALOR;
                        break;
                    case 169:
                        Modelo.C_Arr_Par_int = Valor.VALOR;
                        break;
                    case 170:
                        Modelo.C_Arr_Par_cal = Valor.VALOR;
                        break;
                    case 184:
                        Modelo.Ccbef = Valor.VALOR;
                        break;
                    case 171:
                        Modelo.Ccbef_f1 = Valor.VALOR;
                        break;
                    case 172:
                        Modelo.Ccbef_f2 = Valor.VALOR;
                        break;
                    case 173:
                        Modelo.Ccbef_int = Valor.VALOR;
                        break;
                    case 174:
                        Modelo.Ccbef_cal = Valor.VALOR;
                        break;
                    /*
                case 175:
                    Modelo.CombPE = Valor.VALOR;
                    break;
                    */
                    case 176:
                        Modelo.Pot_punto1 = Valor.VALOR;
                        break;
                    /*
                case 177:
                    Modelo.Comb_punto1 = Valor.VALOR;
                    break;
                    */
                    case 178:
                        Modelo.Pot_punto2 = Valor.VALOR;
                        break;
                    /*
                case 179:
                    Modelo.Comb_punto2 = Valor.VALOR;
                    break;
                    */
                    case 180:
                        Modelo.Pot_punto3 = Valor.VALOR;
                        break;
                    /*
                case 181:
                    Modelo.Comb_punto3 = Valor.VALOR;
                    break;
                    */
                    case 182:
                        Modelo.Pot_punto4 = Valor.VALOR;
                        break;
                    /*
                case 183:
                    Modelo.Comb_punto4 = Valor.VALOR;
                    break;
                    */
                    case 516:
                        Modelo.CombPE = Valor.VALOR;
                        break;
                    case 517:
                        Modelo.Comb_punto1 = Valor.VALOR;
                        break;
                    case 518:
                        Modelo.Comb_punto2 = Valor.VALOR;
                        break;
                    case 519:
                        Modelo.Comb_punto3 = Valor.VALOR;
                        break;
                    case 520:
                        Modelo.Comb_punto4 = Valor.VALOR;
                        break;
                    case 373:
                        Modelo.Comb_arr_sinc = Valor.VALOR;
                        break;
                    case 378:
                        Modelo.Comb_sinc_PC = Valor.VALOR;
                        break;
                    case 383:
                        Modelo.Comb_PC_sinc = Valor.VALOR;
                        break;
                    case 384:
                        Modelo.Comb_sinc_par = Valor.VALOR;
                        break;
                    case 374:
                        Modelo.Comb_arr_sinc_F1 = Valor.VALOR;
                        break;
                    case 375:
                        Modelo.Comb_arr_sinc_F2 = Valor.VALOR;
                        break;
                    case 376:
                        Modelo.Comb_arr_sinc_int = Valor.VALOR;
                        break;
                    case 377:
                        Modelo.Comb_arr_sinc_cal = Valor.VALOR;
                        break;
                    case 379:
                        Modelo.Comb_sinc_PC_F1 = Valor.VALOR;
                        break;
                    case 380:
                        Modelo.Comb_sinc_PC_F2 = Valor.VALOR;
                        break;
                    case 381:
                        Modelo.Comb_sinc_PC_int = Valor.VALOR;
                        break;
                    case 382:
                        Modelo.Comb_sinc_PC_cal = Valor.VALOR;
                        break;
                }
            }

            //foreach (var Valor in lsValoresMOGrupo)
            //{
            //}

            var lstModosEspeciales = ConfigurationManager.AppSettings["ModosOperacionEspeciales"].Split(',');
            Modelo.bEsCasoIlo = false;
            string sAbrevModo = oModoOperacion.Grupoabrev.Trim().ToUpperInvariant();
            Modelo.CodModo = sAbrevModo;
            foreach (var sModoEspecial in lstModosEspeciales)
            {
                if (sAbrevModo == sModoEspecial.ToUpperInvariant().Trim())
                {
                    Modelo.bEsCasoIlo = true;
                    break;
                }
            }
            if (Modelo.bEsCasoIlo)
            {
                Modelo.sRowSpanVelocidad = "6";
                Modelo.sRowSpanTiempo = "19";
                Modelo.sRowSpanEnergia = "6";
                Modelo.sRowSpanCombustible = "19";// "21";
                Modelo.sRowSpanArranque = "11";
                Modelo.sRowSpanCCBEF = "5";
                Modelo.sNumeral1 = "1.9.5";
                Modelo.sNumeral2 = "1.9.6";
            }
            else
            {
                Modelo.sRowSpanVelocidad = "2";
                Modelo.sRowSpanTiempo = "11";
                Modelo.sRowSpanEnergia = "2";
                Modelo.sRowSpanCombustible = "11";// "13";
                Modelo.sRowSpanArranque = "3";
                Modelo.sRowSpanCCBEF = "1";
                Modelo.sNumeral1 = "1.9.1";
                Modelo.sNumeral2 = "1.9.2";
            }

            //if (Modelo.CodCentral == "11571")

            if (grupocodi == "318")
            {
                Modelo.sRowSpanTiempo = "15";
            }

            #region "Calculo Costo Total Combustible"

            //try
            //{
            //    decimal dPrecioCComb = string.IsNullOrEmpty(Modelo.PrecioCComb) ? 0 : Convert.ToDecimal(Modelo.PrecioCComb);
            //    decimal dCostoTransCComb = string.IsNullOrEmpty(Modelo.CostoTransCComb) ? 0 : Convert.ToDecimal(Modelo.CostoTransCComb);
            //    decimal dCostoTratMecCComb = string.IsNullOrEmpty(Modelo.CostoTratMecCComb) ? 0 : Convert.ToDecimal(Modelo.CostoTratMecCComb);
            //    decimal dCostoTratQuiCComb = string.IsNullOrEmpty(Modelo.CostoTratQuiCComb) ? 0 : Convert.ToDecimal(Modelo.CostoTratQuiCComb);
            //    decimal dCostoFinCComb = string.IsNullOrEmpty(Modelo.CostoFinCComb) ? 0 : Convert.ToDecimal(Modelo.CostoFinCComb);
            //    var dCostoTotalComb = dPrecioCComb + dCostoTransCComb + dCostoTratMecCComb + dCostoTratQuiCComb +
            //                          dCostoFinCComb;
            //    Modelo.CostoTotalComb = dCostoTotalComb.ToString();
            //}
            //catch (Exception)
            //{ }

            #endregion
            #region "Cálculo de CCbef"

            string sTipoCombustible = "";
            if (Modelo.TipComb != null)
                sTipoCombustible = Modelo.TipComb;
            try
            {
                if (!string.IsNullOrEmpty(Modelo.TipComb))
                {
                    double dComb_arr_sinc = string.IsNullOrEmpty(Modelo.Comb_arr_sinc) ? 0 : Convert.ToDouble(Modelo.Comb_arr_sinc);
                    double dComb_sinc_PC = string.IsNullOrEmpty(Modelo.Comb_sinc_PC) ? 0 : Convert.ToDouble(Modelo.Comb_sinc_PC);
                    double dComb_PC_sinc = string.IsNullOrEmpty(Modelo.Comb_PC_sinc) ? 0 : Convert.ToDouble(Modelo.Comb_PC_sinc);
                    double dComb_sinc_par = string.IsNullOrEmpty(Modelo.Comb_sinc_par) ? 0 : Convert.ToDouble(Modelo.Comb_sinc_par);

                    double dLHV = string.IsNullOrEmpty(Modelo.CostoTotalComb) ? 0 : Convert.ToDouble(Modelo.CostoTotalComb);
                    double dCostoTotalComb = string.IsNullOrEmpty(Modelo.Comb_sinc_par) ? 0 : Convert.ToDouble(Modelo.Comb_sinc_par);

                    var SumaTotal = dComb_arr_sinc + dComb_sinc_PC + dComb_PC_sinc + dComb_sinc_par;
                    double dCCbef = 0;
                    switch (sTipoCombustible.ToUpperInvariant())
                    {
                        case "GAS"://GAS
                        case "GAS NATURAL":
                        case "BIOGAS":
                            dCCbef = (SumaTotal * dLHV / 28316.846592) * dCostoTotalComb;
                            break;
                        case "CARBON"://SOLIDOS
                        case "CARBÓN":
                            dCCbef = SumaTotal * dCostoTotalComb;
                            break;
                        case "DIESEL"://LIQUIDOS
                        case "D2":
                        case "R500":
                        case "RESIDUAL 500":
                        case "R6":
                        case "RESIDUAL 6":
                            dCCbef = (SumaTotal / 42) * dCostoTotalComb;
                            break;

                    }
                    Modelo.Ccbef = dCCbef.ToString();
                }
            }
            catch (Exception)
            { }
            #endregion
            #region "Costo Total Arranque Parada"

            try
            {
                decimal dCcbef = string.IsNullOrEmpty(Modelo.Ccbef) ? 0 : Convert.ToDecimal(Modelo.Ccbef);
                decimal dCMarr = string.IsNullOrEmpty(Modelo.CMarr) ? 0 : Convert.ToDecimal(Modelo.CMarr);

                decimal dC_Arr_Par = dCcbef + dCMarr;//Costo Total de Arranque Parada
                Modelo.C_Arr_Par = dC_Arr_Par.ToString();
            }
            catch (Exception)
            { }
            #endregion

            #region "En caso el MO Opere a GAS"
            if (sCombustible.ToUpperInvariant().StartsWith("GAS"))
            {
                var oAux = new ConceptoDatoDTO();
                //ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 187);//A
                //Modelo.a = oAux.VALOR;
                //oAux = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 188);//B
                //Modelo.b = oAux.VALOR;
                //oAux = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 189);//C
                //Modelo.c = oAux.VALOR;
                oAux = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 539);//Rendimiento:
                Modelo.EficTerm = oAux.VALOR;
                oAux = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 193);//Consumo especifico de Calor:
                Modelo.EficBTUKWh = oAux.VALOR;
                conceptoDto = appDespacho.GetByIdPrConcepto(193);
                Modelo.EficBTUKWh_uni = "[" + conceptoDto.Concepunid + "]";
            }
            #endregion
            return View(Modelo);
        }
        public ActionResult DatosCentralE(string id, string iFamilia)
        {
            var oCentral = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));
            var CentralEolicaDetalle = new DetalleCentralEolicaViewModel();

            CentralEolicaDetalle.NombreCentral = oCentral.Equinomb;
            CentralEolicaDetalle.CódigoCentral = id;//oCentral.EQUIABREV;
            CentralEolicaDetalle.NombreEmpresa = oCentral.EMPRNOMB;
            CentralEolicaDetalle.IdCentral = oCentral.Equicodi.ToString();
            CentralEolicaDetalle.IdFamilia = oCentral.Famcodi.ToString();

            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1601:
                        CentralEolicaDetalle.PotApaBrut = strValor;
                        break;
                    case 1602:
                        CentralEolicaDetalle.PotInstNom = strValor;
                        break;
                    case 1603:
                        CentralEolicaDetalle.NumAerog = strValor;
                        break;
                    case 1604:
                        CentralEolicaDetalle.HeqPCAno = strValor;
                        break;
                    case 1605:
                        CentralEolicaDetalle.HeqPCMes = strValor;
                        break;
                    case 1606:
                        CentralEolicaDetalle.CurPotReac = strValor;
                        break;
                    case 1607:
                        CentralEolicaDetalle.CurPotPCR = strValor;
                        break;
                    case 1608:
                        CentralEolicaDetalle.SistCont = strValor;
                        break;
                    case 1609:
                        CentralEolicaDetalle.ContTen = strValor;
                        break;
                    case 1610:
                        CentralEolicaDetalle.ContFrec = strValor;
                        break;
                    case 1611:
                        CentralEolicaDetalle.NivMedTen = strValor;
                        break;
                    case 1612:
                        CentralEolicaDetalle.IntCortCirc = strValor;
                        break;
                    case 1795:
                        CentralEolicaDetalle.DiaUnif = strValor;
                        break;
                }
            }

            return View(CentralEolicaDetalle);
        }
        public ActionResult DatosAeroGenerador(string id, string iFamilia)
        {
            var Equipo = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Central = appEquipamiento.GetByIdEqEquipo(Equipo.Equipadre.Value);
            var NombreCentral = Central.Equinomb;
            var NombreEmpresa = Central.EMPRNOMB;

            var aerogenerador = new DetalleAerogeneradorViewModel();
            aerogenerador.NombreCentral = NombreCentral;
            aerogenerador.NombreEmpresa = NombreEmpresa;
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));

            foreach (var propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1613:
                        aerogenerador.AeFab = strValor;
                        break;
                    case 1614:
                        aerogenerador.AeMod = strValor;
                        break;
                    case 1615:
                        aerogenerador.AeTec = strValor;
                        break;
                    case 1616:
                        aerogenerador.AePnom = strValor;
                        break;
                    case 1617:
                        aerogenerador.AeSnom = strValor;
                        break;
                    case 1618:
                        aerogenerador.AeTnom = strValor;
                        break;
                    case 1619:
                        aerogenerador.AeCurvPQ = strValor;
                        break;
                    case 1620:
                        aerogenerador.AeCurvPot = strValor;
                        break;
                    case 1621:
                        aerogenerador.AeVeonex = strValor;
                        break;
                    case 1622:
                        aerogenerador.AeVVient = strValor;
                        break;
                    case 1623:
                        aerogenerador.AeVDesc = strValor;
                        break;
                    case 1624:
                        aerogenerador.AeRotDiam = strValor;
                        break;
                    case 1625:
                        aerogenerador.AeRotArea = strValor;
                        break;
                    case 1626:
                        aerogenerador.AeRotNumPal = strValor;
                        break;
                    case 1627:
                        aerogenerador.AeRotPos = strValor;
                        break;
                    case 1628:
                        aerogenerador.AeRotVelNom = strValor;
                        break;
                    case 1629:
                        aerogenerador.AeRotRang = strValor;
                        break;
                    case 1630:
                        aerogenerador.AeRotLongPal = strValor;
                        break;
                    case 1631:
                        aerogenerador.AeCajTip = strValor;
                        break;
                    case 1632:
                        aerogenerador.AeCajRelMult = strValor;
                        break;
                    case 1633:
                        aerogenerador.AeTorrTip = strValor;
                        break;
                    case 1634:
                        aerogenerador.AeTorrMat = strValor;
                        break;
                    case 1635:
                        aerogenerador.AeTorrLong = strValor;
                        break;
                    case 1636:
                        aerogenerador.AeGenFab = strValor;
                        break;
                    case 1637:
                        aerogenerador.AeGenTip = strValor;
                        break;
                    case 1638:
                        aerogenerador.AeGenPNom = strValor;
                        break;
                    case 1639:
                        aerogenerador.AeGenPApar = strValor;
                        break;
                    case 1640:
                        aerogenerador.AeGenVelNom = strValor;
                        break;
                    case 1641:
                        aerogenerador.AeGenRang = strValor;
                        break;
                    case 1642:
                        aerogenerador.AeGenTenNom = strValor;
                        break;
                    case 1643:
                        aerogenerador.AeGenFrec = strValor;
                        break;
                    case 1644:
                        aerogenerador.AeGenDesNom = strValor;
                        break;
                    case 1645:
                        aerogenerador.AeGenTemp = strValor;
                        break;
                    case 1646:
                        aerogenerador.AeGenCurvPot = strValor;
                        break;
                    case 1647:
                        aerogenerador.AeConvFab = strValor;
                        break;
                    case 1648:
                        aerogenerador.AeConvTip = strValor;
                        break;
                    case 1649:
                        aerogenerador.AeConvSoft = strValor;
                        break;
                    case 1650:
                        aerogenerador.AeConvTen = strValor;
                        break;
                    case 1651:
                        aerogenerador.AeConvSnom = strValor;
                        break;
                    case 1652:
                        aerogenerador.AeSCSoft = strValor;
                        break;
                    case 1653:
                        aerogenerador.AeSCAngPala = strValor;
                        break;
                    case 1654:
                        aerogenerador.AeSCOriBar = strValor;
                        break;
                    case 1655:
                        aerogenerador.AeSCVel = strValor;
                        break;
                    case 1656:
                        aerogenerador.AeSCTen = strValor;
                        break;
                    case 1657:
                        aerogenerador.AeSCFrec = strValor;
                        break;
                    case 1658:
                        aerogenerador.AeNa10 = strValor;
                        break;
                    case 1659:
                        aerogenerador.AeNa120 = strValor;
                        break;
                    case 1660:
                        aerogenerador.AeNn10 = strValor;
                        break;
                    case 1661:
                        aerogenerador.AeNn120 = strValor;
                        break;
                    case 1662:
                        aerogenerador.AeAjPTip = strValor;
                        break;
                    case 1663:
                        aerogenerador.AeAjPSobrT = strValor;
                        break;
                    case 1664:
                        aerogenerador.AeAjPSubTen = strValor;
                        break;
                    case 1665:
                        aerogenerador.AeAjPSobrF = strValor;
                        break;
                    case 1666:
                        aerogenerador.AeAjPSubF = strValor;
                        break;
                    case 1667:
                        aerogenerador.CTTFab = strValor;
                        break;
                    case 1668:
                        aerogenerador.CTTTip = strValor;
                        break;
                    case 1669:
                        aerogenerador.CTTTen = strValor;
                        break;
                    case 1670:
                        aerogenerador.CTTRelTr = strValor;
                        break;
                    case 1671:
                        aerogenerador.CTTGrupC = strValor;
                        break;
                    case 1672:
                        aerogenerador.CTTSnom = strValor;
                        break;
                    case 1673:
                        aerogenerador.CTTTenCC = strValor;
                        break;
                    case 1674:
                        aerogenerador.CTCFab = strValor;
                        break;
                    case 1675:
                        aerogenerador.CTCTip = strValor;
                        break;
                    case 1676:
                        aerogenerador.RMTTen = strValor;
                        break;
                    case 1677:
                        aerogenerador.RMTNumC = strValor;
                        break;
                    case 1678:
                        aerogenerador.RMTLong = strValor;
                        break;
                    case 1679:
                        aerogenerador.RMTCond = strValor;
                        break;
                    case 1680:
                        aerogenerador.RMTSec = strValor;
                        break;
                    case 1681:
                        aerogenerador.RMTAisl = strValor;
                        break;
                    case 1682:
                        aerogenerador.RMTTenAisl = strValor;
                        break;
                    case 1683:
                        aerogenerador.RMTResis = strValor;
                        break;
                    case 1684:
                        aerogenerador.RMTReac = strValor;
                        break;
                    case 1685:
                        aerogenerador.RMTSusc = strValor;
                        break;
                    case 1686:
                        aerogenerador.SETCFab = strValor;
                        break;
                    case 1687:
                        aerogenerador.SETCTip = strValor;
                        break;
                    case 1688:
                        aerogenerador.SETCTen = strValor;
                        break;
                    case 1689:
                        aerogenerador.SETCRelTr = strValor;
                        break;
                    case 1690:
                        aerogenerador.SETCGrupC = strValor;
                        break;
                    case 1691:
                        aerogenerador.SETCSnom = strValor;
                        break;
                    case 1692:
                        aerogenerador.SETCTenCC = strValor;
                        break;
                    case 1693:
                        aerogenerador.SETCRegUbi = strValor;
                        break;
                    case 1694:
                        aerogenerador.SETCRegTip = strValor;
                        break;
                    case 1695:
                        aerogenerador.SETCRegAut = strValor;
                        break;
                    case 1696:
                        aerogenerador.SETCRegNumT = strValor;
                        break;
                    case 1697:
                        aerogenerador.SETCRegRang = strValor;
                        break;
                    case 1698:
                        aerogenerador.SEMTFab = strValor;
                        break;
                    case 1699:
                        aerogenerador.SEMTTip = strValor;
                        break;
                    case 1700:
                        aerogenerador.SECRTip = strValor;
                        break;
                    case 1701:
                        aerogenerador.SECREPot = strValor;
                        break;
                    case 1702:
                        aerogenerador.SECRENum = strValor;
                        break;
                    case 1703:
                        aerogenerador.SECRETip = strValor;
                        break;
                    case 1704:
                        aerogenerador.SECRDTen = strValor;
                        break;
                    case 1705:
                        aerogenerador.SECRDPot = strValor;
                        break;
                    case 1706:
                        aerogenerador.SECRDCont = strValor;
                        break;
                    case 1707:
                        aerogenerador.DS_DiaUni = strValor;
                        break;
                }
            }
            return View(aerogenerador);
        }
        public ActionResult DatosCentralSolar(string id, string iFamilia)
        {
            var oCentral = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));

            var centralSolar = new DetalleCentralSolarViewModel();
            centralSolar.CodCentral = oCentral.Equicodi.ToString();// oCentral.EQUIABREV;
            centralSolar.NombreCentral = oCentral.Equinomb;
            centralSolar.NombreEmpresa = oCentral.EMPRNOMB;
            centralSolar.IdCentral = oCentral.Equicodi.ToString();

            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1709:
                        centralSolar.Sbrut = strValor;
                        break;
                    case 1710:
                        centralSolar.Pinst = strValor;
                        break;
                    case 1711:
                        centralSolar.NumMod = strValor;
                        break;
                    case 1712:
                        centralSolar.TecnSeg = strValor;
                        break;
                    case 1713:
                        centralSolar.AngIncl = strValor;
                        break;
                    case 1714:
                        centralSolar.DistMod = strValor;
                        break;
                    case 1715:
                        centralSolar.HEqPCAn = strValor;
                        break;
                    case 1716:
                        centralSolar.HEqPCMes = strValor;
                        break;
                    case 1717:
                        centralSolar.CurGen = strValor;
                        break;
                    case 1718:
                        centralSolar.PenMax = strValor;
                        break;
                    case 1719:
                        centralSolar.CurPot = strValor;
                        break;
                    case 1720:
                        centralSolar.ConTen = strValor;
                        break;
                    case 1721:
                        centralSolar.ConFrec = strValor;
                        break;
                    case 1722:
                        centralSolar.NivMTen = strValor;
                        break;
                    case 1723:
                        centralSolar.IntCC = strValor;
                        break;
                    case 1724:
                        centralSolar.DiaUni = strValor;
                        break;
                }
            }

            return View(centralSolar);
        }
        public ActionResult DatosModuloSolar(string id, string iFamilia)
        {
            var Equipo = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var Central = appEquipamiento.GetByIdEqEquipo(Equipo.Equipadre.Value);
            var NombreCentral = Central.Equinomb;
            var NombreEmpresa = Central.EMPRNOMB;

            var moduloSolar = new DetalleModuloSolarViewModel();
            moduloSolar.NombreCentral = NombreCentral;
            moduloSolar.NombreEmpresa = NombreEmpresa;
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));
            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
            {
                string strValor = string.Empty;
                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 1725:
                        moduloSolar.MFab = strValor;
                        break;
                    case 1726:
                        moduloSolar.MMod = strValor;
                        break;
                    case 1727:
                        moduloSolar.MTec = strValor;
                        break;
                    case 1728:
                        moduloSolar.MPnom = strValor;
                        break;
                    case 1729:
                        moduloSolar.MSnom = strValor;
                        break;
                    case 1730:
                        moduloSolar.CurPot = strValor;
                        break;
                    case 1731:
                        moduloSolar.TempOp = strValor;
                        break;
                    case 1732:
                        moduloSolar.CoefTemp = strValor;
                        break;
                    case 1733:
                        moduloSolar.InvFab = strValor;
                        break;
                    case 1734:
                        moduloSolar.InvMod = strValor;
                        break;
                    case 1735:
                        moduloSolar.InvTec = strValor;
                        break;
                    case 1736:
                        moduloSolar.InvRend = strValor;
                        break;
                    case 1737:
                        moduloSolar.InvPap = strValor;
                        break;
                    case 1738:
                        moduloSolar.InvRangT = strValor;
                        break;
                    case 1739:
                        moduloSolar.InvTenCMA = strValor;
                        break;
                    case 1740:
                        moduloSolar.InvCorrCMA = strValor;
                        break;
                    case 1741:
                        moduloSolar.InvPMaxP = strValor;
                        break;
                    case 1742:
                        moduloSolar.InvPApaNom = strValor;
                        break;
                    case 1743:
                        moduloSolar.InvPActNom = strValor;
                        break;
                    case 1744:
                        moduloSolar.InvTenOpeAl = strValor;
                        break;
                    case 1745:
                        moduloSolar.InvNumFas = strValor;
                        break;
                    case 1746:
                        moduloSolar.InvFrec = strValor;
                        break;
                    case 1747:
                        moduloSolar.InvFDTens = strValor;
                        break;
                    case 1748:
                        moduloSolar.InvSoft = strValor;
                        break;
                    case 1749:
                        moduloSolar.InvContInv = strValor;
                        break;
                    case 1750:
                        moduloSolar.InvContFP = strValor;
                        break;
                    case 1751:
                        moduloSolar.InvFrecCom = strValor;
                        break;
                    case 1752:
                        moduloSolar.InvMetSPMP = strValor;
                        break;
                    case 1753:
                        moduloSolar.InvRestAut = strValor;
                        break;
                    case 1754:
                        moduloSolar.InvSinc = strValor;
                        break;
                    case 1755:
                        moduloSolar.InvProt = strValor;
                        break;
                    case 1756:
                        moduloSolar.InvAPT = strValor;
                        break;
                    case 1757:
                        moduloSolar.InvAPRSobreT = strValor;
                        break;
                    case 1758:
                        moduloSolar.InvAPRSubT = strValor;
                        break;
                    case 1759:
                        moduloSolar.InvAPRSobreF = strValor;
                        break;
                    case 1760:
                        moduloSolar.InvAPRSubF = strValor;
                        break;
                    case 1761:
                        moduloSolar.CTTCC = strValor;
                        break;
                    case 1762:
                        moduloSolar.CTTFab = strValor;
                        break;
                    case 1763:
                        moduloSolar.CTTTip = strValor;
                        break;
                    case 1764:
                        moduloSolar.CTTTen = strValor;
                        break;
                    case 1765:
                        moduloSolar.CTTRelT = strValor;
                        break;
                    case 1766:
                        moduloSolar.CTTGrupC = strValor;
                        break;
                    case 1767:
                        moduloSolar.CTTSnom = strValor;
                        break;
                    case 1768:
                        moduloSolar.CTTTensCC = strValor;
                        break;
                    case 1769:
                        moduloSolar.CTCFab = strValor;
                        break;
                    case 1770:
                        moduloSolar.CTCTip = strValor;
                        break;
                    case 1771:
                        moduloSolar.MTTen = strValor;
                        break;
                    case 1772:
                        moduloSolar.MTNumC = strValor;
                        break;
                    case 1773:
                        moduloSolar.MTLong = strValor;
                        break;
                    case 1774:
                        moduloSolar.MTCond = strValor;
                        break;
                    case 1775:
                        moduloSolar.MTSecc = strValor;
                        break;
                    case 1776:
                        moduloSolar.MTTipAis = strValor;
                        break;
                    case 1777:
                        moduloSolar.MTTensAis = strValor;
                        break;
                    case 1778:
                        moduloSolar.MTResis = strValor;
                        break;
                    case 1779:
                        moduloSolar.MTReac = strValor;
                        break;
                    case 1780:
                        moduloSolar.MTSusc = strValor;
                        break;
                    case 1781:
                        moduloSolar.SEFab = strValor;
                        break;
                    case 1782:
                        moduloSolar.SETip = strValor;
                        break;
                    case 1783:
                        moduloSolar.SETen = strValor;
                        break;
                    case 1784:
                        moduloSolar.SERelT = strValor;
                        break;
                    case 1785:
                        moduloSolar.SEGrupoC = strValor;
                        break;
                    case 1786:
                        moduloSolar.SESnom = strValor;
                        break;
                    case 1787:
                        moduloSolar.SETensCC = strValor;
                        break;
                    case 1788:
                        moduloSolar.SERUbi = strValor;
                        break;
                    case 1789:
                        moduloSolar.SERTipR = strValor;
                        break;
                    case 1790:
                        moduloSolar.SERAut = strValor;
                        break;
                    case 1791:
                        moduloSolar.DiaUni = strValor;
                        break;
                    case 1792:
                        moduloSolar.PMM = strValor;
                        break;
                }
            }
            return View(moduloSolar);
        }
        public ActionResult DatosLinea(string id, string iFamilia)
        {
            var oLinea = appEquipamiento.GetByIdEqEquipo(int.Parse(id));
            var NombreLinea = oLinea.Equinomb;
            var NombreEmpresa = oLinea.EMPRNOMB;

            var LineaTran = new DetalleLineaViewModel();
            LineaTran.NombreLinea = NombreLinea;
            LineaTran.NombreEmpresa = NombreEmpresa;
            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(int.Parse(id));
            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
            {
                string Valor = string.Empty;
                Valor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                switch ((int)propiedadEquipoHistDto.PROPCODI)
                {
                    case 699:
                        LineaTran.Capacidadcontinua = Valor;
                        break;
                    case 706:
                        LineaTran.G1 = Valor;
                        break;
                    case 263:
                        LineaTran.LongitudLinea = Valor;
                        break;
                    case 67:
                        LineaTran.NivelTension = Valor;
                        break;
                    case 756:
                        LineaTran.R0m = Valor;
                        break;
                    case 704:
                        LineaTran.X1 = Valor;
                        break;
                    case 709:
                        LineaTran.X0 = Valor;
                        break;
                    case 703:
                        LineaTran.R1 = Valor;
                        break;
                    case 708:
                        LineaTran.R0 = Valor;
                        break;
                    case 707:
                        LineaTran.B1 = Valor;
                        break;
                    case 711:
                        LineaTran.B0 = Valor;
                        break;
                    case 757:
                        LineaTran.Xom = Valor;
                        break;
                    case 1793:
                        LineaTran.EstacionInicio = Valor;
                        break;
                    case 1794:
                        LineaTran.EstacionFin = Valor;
                        break;
                }
            }
            return View(LineaTran);
        }
        [HttpGet]
        public virtual ActionResult Exportar(string sTipoCental, string sEmpresa, string sEquipo)
        {
            int iFamilia = Convert.ToInt32(sTipoCental);
            int iEmpresa = string.IsNullOrWhiteSpace(sEmpresa) ? 0 : Convert.ToInt32(sEmpresa);
            string sNombreEquipo = sEquipo ?? string.Empty;
            int iTotalResultados = 0;
            int iTotalPaginas = 0;

            var centrales = appEquipamiento.ListarEquiposxFiltroPaginado(iEmpresa, "AF", iFamilia, 0, sNombreEquipo, -99,
                1, int.MaxValue, ref iTotalPaginas, ref iTotalResultados);
            string sRutaArchivoDatos = string.Empty;
            string sNombreArchivo = string.Empty;
            switch (iFamilia)
            {
                case 4://Hidraulicas
                    sRutaArchivoDatos = GenerarArchivoDataHidro(centrales);
                    sNombreArchivo = "FichaTecnicaHidroelectrica.xlsx";
                    break;
                case 37://Solares
                    sRutaArchivoDatos = GenerarArchivoDataSolar(centrales);
                    sNombreArchivo = "FichaTecnicaSolar.xlsx";
                    break;
                case 39://Eolica
                    sRutaArchivoDatos = GenerarArchivoDataEolica(centrales);
                    sNombreArchivo = "FichaTecnicaEolica.xlsx";
                    break;
                case 5://Termoelectrica
                    sRutaArchivoDatos = GenerarArchivoDataTermica(centrales);
                    sNombreArchivo = "FichaTecnicaTermoelectrica.xlsx";
                    break;
            }

            if (sRutaArchivoDatos != string.Empty)
            {
                return File(sRutaArchivoDatos, "application/vnd.ms-excel", sNombreArchivo);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        protected string GenerarArchivoDataHidro(IList<EqEquipoDTO> lCentrales)
        {
            string sRutaArchivo = string.Empty;

            if (lCentrales.Count > 0)
            {
                try
                {
                    List<EqEquipoDTO> lsGeneradores = new List<EqEquipoDTO>();
                    string ruta = ConfigurationManager.AppSettings["RutaPlantilla"].ToString();
                    string sNombreFile = "FichaTecnicaHidroelectrica.xlsx";
                    FileInfo template = new FileInfo(ruta + "HidroFichaTecnica.xlsx");
                    FileInfo newFile = new FileInfo(ruta + sNombreFile);

                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(ruta + sNombreFile);
                    }
                    int index = 9;
                    int row = 2;
                    int column_central = 9;
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        //Primera Hoja Datos de la Central
                        #region "HOJA CENTRAL HIDRAULICA"
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets["CENTRAL_HIDORELECTRICA"];

                        foreach (var ocentral in lCentrales)
                        {
                            ///// Obtengo los datos de los Generadores H///////
                            int iTotPag = 0;
                            int iTotRec = 0;
                            //var lGenH = appEquipo.BuscarEquiposxPadre(ocentral.EQUICODI, 2, 1, ref iTotPag, int.MaxValue);

                            var lGenH = appEquipamiento.ListarEquiposxFiltroPaginado(0, "AF", 2, 0, "", ocentral.Equicodi, 1, int.MaxValue, ref iTotPag, ref iTotRec);


                            lsGeneradores.AddRange(lGenH);
                            /////////////////////////////////////////////////////
                            /// DATOS DE LAS PROPIEDADES DE LA CENTAL///////////
                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(ocentral.Equicodi);

                            ws.SetValue(2, column_central, ocentral.EMPRNOMB.Trim());
                            ws.SetValue(3, column_central, ocentral.Equinomb.Trim());
                            ws.SetValue(5, column_central, ocentral.Equicodi);

                            foreach (var propiedadEquipoHistDto in Propiedades)//RECORRE CADA PROPIEDAD Y LA PINTA EN SU RESPECTIVA CELDA
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1487:
                                        //centralHidraulica.Númerounidades = strValor;
                                        ws.SetValue(6, column_central, strValor);
                                        break;
                                    case 1488:
                                        //centralHidraulica.Tipo = strValor;
                                        ws.SetValue(7, column_central, strValor);
                                        break;
                                    case 46:
                                        //centralHidraulica.PotenciaEfectiva = strValor;
                                        ws.SetValue(8, column_central, strValor);
                                        break;
                                    case 330:
                                        //centralHidraulica.ServiciosAuxiliares = strValor;
                                        ws.SetValue(9, column_central, strValor);
                                        break;
                                    case 932:
                                        //centralHidraulica.Rendimiento = strValor;
                                        ws.SetValue(10, column_central, strValor);
                                        break;
                                    case 1489:
                                        //centralHidraulica.Batimetría = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        ws.SetValue(11, column_central, strValor);
                                        break;
                                    case 1490:
                                        //centralHidraulica.Hidrologíadelmes = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        ws.SetValue(12, column_central, strValor);
                                        break;
                                    case 1491:
                                        //centralHidraulica.Modosoperación = strValor;
                                        ws.SetValue(13, column_central, strValor);
                                        break;
                                    case 1483:
                                        //centralHidraulica.PotenciaGarantizada = strValor;
                                        ws.SetValue(14, column_central, strValor);
                                        break;
                                    case 1492:
                                        //centralHidraulica.Esquemahidráulica = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        ws.SetValue(16, column_central, strValor);
                                        break;
                                    case 1493:
                                        //centralHidraulica.Reservorioanualvolumenmaximo = strValor;
                                        ws.SetValue(17, column_central, strValor);
                                        break;
                                    case 1494:
                                        //centralHidraulica.Reservorioanualvolumenminimo = strValor;
                                        ws.SetValue(18, column_central, strValor);
                                        break;
                                    case 1495:
                                        //centralHidraulica.Reservorioanualcaudaldescarga = strValor;
                                        ws.SetValue(19, column_central, strValor);
                                        break;
                                    case 1496:
                                        //centralHidraulica.Reservorioanualtiemodesplazamiento = strValor;
                                        ws.SetValue(20, column_central, strValor);
                                        break;
                                    case 1497:
                                        // centralHidraulica.Reservorioestacionalvolumenmaximo = strValor;
                                        ws.SetValue(21, column_central, strValor);
                                        break;
                                    case 1498:
                                        //centralHidraulica.Reservorioestacionalvolumenminimo = strValor;
                                        ws.SetValue(22, column_central, strValor);
                                        break;
                                    case 1499:
                                        //centralHidraulica.Reservorioestacionalcaudaldescarga = strValor;
                                        ws.SetValue(23, column_central, strValor);
                                        break;
                                    case 1500:
                                        // centralHidraulica.Reservorioestacionaltiemodesplazamie = strValor;
                                        ws.SetValue(24, column_central, strValor);
                                        break;
                                    case 1501:
                                        //centralHidraulica.Reservoriosemanalvolumenmaximo = strValor;
                                        ws.SetValue(25, column_central, strValor);
                                        break;
                                    case 1502:
                                        //centralHidraulica.Reservoriosemanalvolumenminimo = strValor;
                                        ws.SetValue(26, column_central, strValor);
                                        break;
                                    case 1503:
                                        //centralHidraulica.Reservoriosemanalcaudaldescarga = strValor;
                                        ws.SetValue(27, column_central, strValor);
                                        break;
                                    case 1504:
                                        //centralHidraulica.Reservoriosemanaltiemodesplazamiento = strValor;
                                        ws.SetValue(28, column_central, strValor);
                                        break;
                                    case 1505:
                                        //centralHidraulica.Reservoriodiahoravolumenmaximo = strValor;
                                        ws.SetValue(29, column_central, strValor);
                                        break;
                                    case 1506:
                                        //centralHidraulica.Reservoriodiahoravolumenminimo = strValor;
                                        ws.SetValue(30, column_central, strValor);
                                        break;
                                    case 1507:
                                        //centralHidraulica.Reservoriodiahoracaudaldescarga = strValor;
                                        ws.SetValue(31, column_central, strValor);
                                        break;
                                    case 1508:
                                        //centralHidraulica.Reservoriodiahoratiemodesplazamiento = strValor;
                                        ws.SetValue(32, column_central, strValor);
                                        break;
                                    case 1509:
                                        //centralHidraulica.Restriccióncaudalmínimoregadío = strValor;
                                        ws.SetValue(33, column_central, strValor);
                                        break;
                                    case 1510:
                                        //centralHidraulica.Restriccióncaudalmáximoregadío = strValor;
                                        ws.SetValue(34, column_central, strValor);
                                        break;
                                    case 1511:
                                        //centralHidraulica.Caudalrequeridomensual = strValor;
                                        ws.SetValue(35, column_central, strValor);
                                        break;
                                    case 1512:
                                        //centralHidraulica.Caudalrequeridosemanal = strValor;
                                        ws.SetValue(36, column_central, strValor);
                                        break;
                                    case 1513:
                                        //centralHidraulica.Reservoriocaracterísticastécnicas = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        ws.SetValue(37, column_central, strValor);
                                        break;
                                    case 1514:
                                        //centralHidraulica.Diagramasunifilares = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        ws.SetValue(39, column_central, strValor);
                                        break;
                                }
                            }
                            var cell = ws.Cells[2, column_central, 39, column_central];
                            foreach (var celda in cell)
                            {
                                var border = celda.Style.Border;
                                border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cell.AutoFitColumns();
                            column_central++;//Aumenta con cada Central H
                        }
                        #endregion
                        //SEGUNDA HOJA DATOS DE GENERADORES
                        #region "HOJA GENERADOR HIDRAULICO"
                        ExcelWorksheet wsgen = xlPackage.Workbook.Worksheets["GENERADOR_HIDROELECTRICO"];

                        int column_gen = 11;
                        foreach (var oGenerador in lsGeneradores)
                        {
                            var Central = appEquipamiento.GetByIdEqEquipo(oGenerador.Equipadre.Value);
                            var NombreCentral = Central.Equinomb;
                            var NombreEmpresa = Central.EMPRNOMB;
                            wsgen.SetValue(2, column_gen, oGenerador.Equinomb.Trim());
                            wsgen.SetValue(3, column_gen, NombreEmpresa.Trim());
                            wsgen.SetValue(4, column_gen, NombreCentral.Trim());
                            wsgen.SetValue(6, column_gen, Central.Equicodi);
                            wsgen.SetValue(7, column_gen, oGenerador.Equicodi);

                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oGenerador.Equicodi);
                            foreach (var propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                strValor = strValor == null ? "" : strValor.Trim();
                                ; switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1525:
                                        //GeneradorHidraulico.DatPlaca = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsgen.SetValue(8, column_gen, strValor);
                                        break;
                                    case 1526:
                                        //GeneradorHidraulico.FecOperCom = strValor;
                                        wsgen.SetValue(9, column_gen, strValor);
                                        break;
                                    case 971:
                                        //GeneradorHidraulico.T_turbina = strValor;
                                        wsgen.SetValue(11, column_gen, strValor);
                                        break;
                                    case 1527:
                                        //GeneradorHidraulico.Fabr = strValor;
                                        wsgen.SetValue(12, column_gen, strValor);
                                        break;
                                    case 1528:
                                        //GeneradorHidraulico.Modelo = strValor;
                                        wsgen.SetValue(13, column_gen, strValor);
                                        break;
                                    case 1529:
                                        //GeneradorHidraulico.Serie = strValor;
                                        wsgen.SetValue(14, column_gen, strValor);
                                        break;
                                    case 164:
                                        //GeneradorHidraulico.Pefectiva = strValor;
                                        wsgen.SetValue(15, column_gen, strValor);
                                        break;
                                    case 1530:
                                        //GeneradorHidraulico.Pinst = strValor;
                                        wsgen.SetValue(16, column_gen, strValor);
                                        break;
                                    case 297:
                                        //GeneradorHidraulico.PotNom = strValor;
                                        wsgen.SetValue(17, column_gen, strValor);
                                        break;
                                    case 165:
                                        //GeneradorHidraulico.Snominal = strValor;
                                        wsgen.SetValue(18, column_gen, strValor);
                                        break;
                                    case 298:
                                        //GeneradorHidraulico.Pmáx = strValor;
                                        wsgen.SetValue(19, column_gen, strValor);
                                        break;
                                    case 299:
                                        //GeneradorHidraulico.Pmin = strValor;
                                        wsgen.SetValue(20, column_gen, strValor);
                                        break;
                                    case 1531:
                                        //GeneradorHidraulico.Psincr = strValor;
                                        wsgen.SetValue(21, column_gen, strValor);
                                        break;
                                    case 178:
                                        //GeneradorHidraulico.RPM = strValor;
                                        wsgen.SetValue(22, column_gen, strValor);
                                        break;
                                    case 166:
                                        //GeneradorHidraulico.univelotom = strValor;
                                        wsgen.SetValue(23, column_gen, strValor);
                                        break;
                                    case 300:
                                        //GeneradorHidraulico.VelDescar = strValor;
                                        wsgen.SetValue(24, column_gen, strValor);
                                        break;
                                    case 301:
                                        //GeneradorHidraulico.TieSincro = strValor;
                                        wsgen.SetValue(25, column_gen, strValor);
                                        break;
                                    case 1532:
                                        //GeneradorHidraulico.T_sincr_PC = strValor;
                                        wsgen.SetValue(26, column_gen, strValor);
                                        break;
                                    case 958:
                                        //GeneradorHidraulico.T_SFSP = strValor;
                                        wsgen.SetValue(27, column_gen, strValor);
                                        break;
                                    case 1533:
                                        //GeneradorHidraulico.T_ArrNegr = strValor;
                                        wsgen.SetValue(28, column_gen, strValor);
                                        break;
                                    case 1534:
                                        //GeneradorHidraulico.T_PC_sinc = strValor;
                                        wsgen.SetValue(29, column_gen, strValor);
                                        break;
                                    case 1535:
                                        //GeneradorHidraulico.T_sinc_par = strValor;
                                        wsgen.SetValue(30, column_gen, strValor);
                                        break;
                                    case 957:
                                        //GeneradorHidraulico.Tmin_ARR_S = strValor;
                                        wsgen.SetValue(31, column_gen, strValor);
                                        break;
                                    case 1536:
                                        //GeneradorHidraulico.Tmin_ARR_S_eme = strValor;
                                        wsgen.SetValue(32, column_gen, strValor);
                                        break;
                                    case 19:
                                        //GeneradorHidraulico.TmaxOperPotMin = strValor;
                                        wsgen.SetValue(33, column_gen, strValor);
                                        break;
                                    case 959:
                                        //GeneradorHidraulico.Tmin_op = strValor;
                                        wsgen.SetValue(34, column_gen, strValor);
                                        break;
                                    case 1537:
                                        //GeneradorHidraulico.Ene_sinc = strValor;
                                        wsgen.SetValue(35, column_gen, strValor);
                                        break;
                                    case 1538:
                                        //GeneradorHidraulico.Ene_PC_sinc = strValor;
                                        wsgen.SetValue(36, column_gen, strValor);
                                        break;
                                    case 303:
                                        //GeneradorHidraulico.QMínTur = strValor;
                                        wsgen.SetValue(37, column_gen, strValor);
                                        break;
                                    case 304:
                                        //GeneradorHidraulico.QmáxTur = strValor;
                                        wsgen.SetValue(38, column_gen, strValor);
                                        break;
                                    case 308:
                                        //GeneradorHidraulico.Rendimiento = strValor;
                                        wsgen.SetValue(39, column_gen, strValor);
                                        break;
                                    case 1539:
                                        //GeneradorHidraulico.CoefA = strValor;
                                        wsgen.SetValue(40, column_gen, strValor);
                                        break;
                                    case 1540:
                                        //GeneradorHidraulico.CoefB = strValor;
                                        wsgen.SetValue(41, column_gen, strValor);
                                        break;
                                    case 1541:
                                        //GeneradorHidraulico.CoefC = strValor;
                                        wsgen.SetValue(42, column_gen, strValor);
                                        break;
                                    case 1542:
                                        //GeneradorHidraulico.Cost_sol = strValor;
                                        wsgen.Cells[43, column_gen, 44, column_gen].Merge = true;
                                        wsgen.SetValue(43, column_gen, strValor);

                                        break;
                                    case 965:
                                        //GeneradorHidraulico.ConcentraSolMax = strValor;
                                        wsgen.SetValue(45, column_gen, strValor);
                                        break;
                                    case 966:
                                        //GeneradorHidraulico.ConcentraSolMin = strValor;
                                        wsgen.SetValue(46, column_gen, strValor);
                                        break;
                                    case 955:
                                        // GeneradorHidraulico.P_SSAA = strValor;
                                        wsgen.SetValue(47, column_gen, strValor);
                                        break;
                                    case 969:
                                        //GeneradorHidraulico.Sneto_max = strValor;
                                        wsgen.SetValue(48, column_gen, strValor);
                                        break;
                                    case 970:
                                        //GeneradorHidraulico.Sneto_min = strValor;
                                        wsgen.SetValue(49, column_gen, strValor);
                                        break;
                                    case 1543:
                                        //GeneradorHidraulico.DBCF = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsgen.SetValue(50, column_gen, strValor);
                                        wsgen.Cells[50, column_gen, 52, column_gen].Merge = true;
                                        break;
                                    case 1544:
                                        //GeneradorHidraulico.PotGener = strValor;
                                        wsgen.SetValue(55, column_gen, strValor);
                                        break;
                                    case 1545:
                                        //GeneradorHidraulico.VelGener = strValor;
                                        wsgen.SetValue(56, column_gen, strValor);
                                        break;
                                    case 1546:
                                        //GeneradorHidraulico.VelEmbGener = strValor;
                                        wsgen.SetValue(57, column_gen, strValor);
                                        break;
                                    case 1547:
                                        //GeneradorHidraulico.Numpolos = strValor;
                                        wsgen.SetValue(58, column_gen, strValor);
                                        break;
                                    case 309:
                                        //GeneradorHidraulico.CurCap = strValor;
                                        wsgen.SetValue(59, column_gen, strValor);
                                        break;
                                    case 14:
                                        //GeneradorHidraulico.CapacMIn = strValor;
                                        wsgen.SetValue(60, column_gen, strValor);
                                        break;
                                    case 15:
                                        //GeneradorHidraulico.GI50 = strValor;
                                        wsgen.SetValue(61, column_gen, strValor);
                                        break;
                                    case 13:
                                        //GeneradorHidraulico.Capac100 = strValor;
                                        wsgen.SetValue(62, column_gen, strValor);
                                        break;
                                    case 17:
                                        //GeneradorHidraulico.GIMÍN = strValor;
                                        wsgen.SetValue(63, column_gen, strValor);
                                        break;
                                    case 311:
                                        //GeneradorHidraulico.Reac50 = strValor;
                                        wsgen.SetValue(64, column_gen, strValor);
                                        break;
                                    case 16:
                                        //GeneradorHidraulico.GI100 = strValor;
                                        wsgen.SetValue(65, column_gen, strValor);
                                        break;
                                    case 960:
                                        //GeneradorHidraulico.Vn_GEN = strValor;
                                        wsgen.SetValue(66, column_gen, strValor);
                                        break;
                                    case 8:
                                        //GeneradorHidraulico.TMinGen = strValor;
                                        wsgen.SetValue(67, column_gen, strValor);
                                        break;
                                    case 310:
                                        //GeneradorHidraulico.TMáxGen = strValor;
                                        wsgen.SetValue(68, column_gen, strValor);
                                        break;
                                    case 183:
                                        //GeneradorHidraulico.unitensmin = strValor;
                                        wsgen.SetValue(69, column_gen, strValor);
                                        break;
                                    case 184:
                                        //GeneradorHidraulico.unitensmax = strValor;
                                        wsgen.SetValue(70, column_gen, strValor);
                                        break;
                                    case 961:
                                        //GeneradorHidraulico.Vmin_SSAA = strValor;
                                        wsgen.SetValue(71, column_gen, strValor);
                                        break;
                                    case 962:
                                        //GeneradorHidraulico.Vmax_SSAA = strValor;
                                        wsgen.SetValue(72, column_gen, strValor);
                                        break;
                                    case 170:
                                        //GeneradorHidraulico.FactPot = strValor;
                                        wsgen.SetValue(73, column_gen, strValor);
                                        break;
                                    case 5:
                                        //GeneradorHidraulico.ARR_BS = strValor;
                                        wsgen.SetValue(74, column_gen, strValor);
                                        break;
                                    case 332:
                                        //GeneradorHidraulico.TransEjeDir = strValor;
                                        wsgen.SetValue(75, column_gen, strValor);
                                        break;
                                    case 333:
                                        //GeneradorHidraulico.SubTransEjeDir = strValor;
                                        wsgen.SetValue(76, column_gen, strValor);
                                        break;
                                    case 334:
                                        //GeneradorHidraulico.ArmSecNeg = strValor;
                                        wsgen.SetValue(77, column_gen, strValor);
                                        break;
                                    case 335:
                                        //GeneradorHidraulico.Xo = strValor;
                                        wsgen.SetValue(78, column_gen, strValor);
                                        break;
                                    case 371:
                                        //GeneradorHidraulico.EjeDirCortoCirc = strValor;
                                        wsgen.SetValue(79, column_gen, strValor);
                                        break;
                                    case 372:
                                        //GeneradorHidraulico.CuadraCircAbierto = strValor;
                                        wsgen.SetValue(80, column_gen, strValor);
                                        break;
                                    case 373:
                                        //GeneradorHidraulico.SubTransCuadraCircAbierto = strValor;
                                        wsgen.SetValue(81, column_gen, strValor);
                                        break;
                                    case 374:
                                        //GeneradorHidraulico.Ta = strValor;
                                        wsgen.SetValue(82, column_gen, strValor);
                                        break;
                                    case 500:
                                        //GeneradorHidraulico.Xp = strValor;
                                        wsgen.SetValue(83, column_gen, strValor);
                                        break;
                                    case 501:
                                        //GeneradorHidraulico.XL = strValor;
                                        wsgen.SetValue(84, column_gen, strValor);
                                        break;
                                    case 502:
                                        //GeneradorHidraulico.MomentoInercia = strValor;
                                        wsgen.SetValue(85, column_gen, strValor);
                                        break;
                                    case 503:
                                        //GeneradorHidraulico.ResistenciaArm = strValor;
                                        wsgen.SetValue(86, column_gen, strValor);
                                        break;
                                    case 504:
                                        //GeneradorHidraulico.SCR = strValor;
                                        wsgen.SetValue(87, column_gen, strValor);
                                        break;
                                    case 505:
                                        //GeneradorHidraulico.S10 = strValor;
                                        wsgen.SetValue(88, column_gen, strValor);
                                        break;
                                    case 506:
                                        //GeneradorHidraulico.S12 = strValor;
                                        wsgen.SetValue(89, column_gen, strValor);
                                        break;
                                    case 964:
                                        //GeneradorHidraulico.Prot_sob_f = strValor;
                                        wsgen.SetValue(91, column_gen, strValor);
                                        break;
                                    case 968:
                                        //GeneradorHidraulico.TiempoTransitoriaCortoC = strValor;
                                        wsgen.SetValue(92, column_gen, strValor);
                                        break;
                                    case 972:
                                        //GeneradorHidraulico.Iexco_1pu = strValor;
                                        wsgen.SetValue(93, column_gen, strValor);
                                        break;
                                    case 973:
                                        //GeneradorHidraulico.Iexco_12p = strValor;
                                        wsgen.SetValue(94, column_gen, strValor);
                                        break;
                                    case 7:
                                        //GeneradorHidraulico.R2 = strValor;
                                        wsgen.SetValue(95, column_gen, strValor);
                                        break;
                                    case 167:
                                        //GeneradorHidraulico.Rn = strValor;
                                        wsgen.SetValue(96, column_gen, strValor);
                                        break;
                                    case 171:
                                        //GeneradorHidraulico.Xn = strValor;
                                        wsgen.SetValue(97, column_gen, strValor);
                                        break;
                                    case 173:
                                        //GeneradorHidraulico.TiempoSubTransEjeDir = strValor;
                                        wsgen.SetValue(98, column_gen, strValor);
                                        break;
                                    case 176:
                                        //GeneradorHidraulico.H = strValor;
                                        wsgen.SetValue(99, column_gen, strValor);
                                        break;
                                    case 179:
                                        //GeneradorHidraulico.Xq = strValor;
                                        wsgen.SetValue(100, column_gen, strValor);
                                        break;
                                    case 180:
                                        //GeneradorHidraulico.ReactanciaTransEjeCua = strValor;
                                        wsgen.SetValue(101, column_gen, strValor);
                                        break;
                                    case 181:
                                        //GeneradorHidraulico.TiempoTransEjeDirCirAbier = strValor;
                                        wsgen.SetValue(102, column_gen, strValor);
                                        break;
                                    case 182:
                                        //GeneradorHidraulico.ReactanciaSubTrans = strValor;
                                        wsgen.SetValue(103, column_gen, strValor);
                                        break;
                                    case 185:
                                        //GeneradorHidraulico.Ro = strValor;
                                        wsgen.SetValue(104, column_gen, strValor);
                                        break;
                                    case 186:
                                        //GeneradorHidraulico.CteTiempoSubTrans = strValor;
                                        wsgen.SetValue(105, column_gen, strValor);
                                        break;
                                    case 331:
                                        //GeneradorHidraulico.Xd = strValor;
                                        wsgen.SetValue(106, column_gen, strValor);
                                        break;
                                    case 974:
                                        //GeneradorHidraulico.DBRTE = strValor;

                                        break;
                                    case 975:
                                        //GeneradorHidraulico.DBRVT = strValor;

                                        break;
                                    case 976:
                                        //GeneradorHidraulico.DBPSS = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsgen.SetValue(107, column_gen, strValor);
                                        wsgen.Cells[107, column_gen, 109, column_gen].Merge = true;
                                        break;
                                    case 1548:
                                        //GeneradorHidraulico.TSF = strValor;
                                        wsgen.SetValue(112, column_gen, strValor);
                                        break;
                                    case 1549:
                                        //GeneradorHidraulico.ProgMantos = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsgen.SetValue(113, column_gen, strValor);
                                        break;
                                    case 1550:
                                        //GeneradorHidraulico.RegVel = strValor;
                                        wsgen.SetValue(114, column_gen, strValor);
                                        break;
                                    case 1551:
                                        //GeneradorHidraulico.ModoTrab = strValor;
                                        wsgen.SetValue(115, column_gen, strValor);
                                        break;
                                    case 1552:
                                        //GeneradorHidraulico.ModoContr = strValor;
                                        wsgen.SetValue(116, column_gen, strValor);
                                        break;
                                    case 1553:
                                        //GeneradorHidraulico.BandaMuer = strValor;
                                        wsgen.SetValue(117, column_gen, strValor);
                                        break;
                                    case 305:
                                        //GeneradorHidraulico.EstatisVA = strValor;
                                        wsgen.SetValue(118, column_gen, strValor);
                                        break;
                                    case 1554:
                                        //GeneradorHidraulico.ValSistAisl = strValor;
                                        wsgen.SetValue(119, column_gen, strValor);
                                        break;
                                    case 306:
                                        //GeneradorHidraulico.RanVariac = strValor;
                                        wsgen.SetValue(120, column_gen, strValor);
                                        break;
                                }

                            }
                            var cellGen = wsgen.Cells[2, column_gen, 120, column_gen];
                            foreach (var oCeldaGen in cellGen)
                            {
                                var borde = oCeldaGen.Style.Border;
                                borde.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            //var border = cellGen.Style.Border;
                            //border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            //foreach (var celda in cellGen)
                            //{
                            //    var border = celda.Style.Border;
                            //    border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            //}
                            cellGen.AutoFitColumns();
                            column_gen++;
                        }
                        #endregion
                        xlPackage.Save();
                    }

                    sRutaArchivo = ruta + sNombreFile;
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }

            return sRutaArchivo;
        }
        protected string GenerarArchivoDataSolar(IList<EqEquipoDTO> lCentrales)
        {
            string sRutaArchivo = string.Empty;
            if (lCentrales.Count > 0)
            {
                try
                {
                    List<EqEquipoDTO> lsGeneradores = new List<EqEquipoDTO>();
                    string ruta = ConfigurationManager.AppSettings["RutaPlantilla"].ToString();
                    string sNombreFile = "FichaTecnicaSolar.xlsx";
                    FileInfo template = new FileInfo(ruta + "SolarFichaTecnica.xlsx");
                    FileInfo newFile = new FileInfo(ruta + sNombreFile);
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(ruta + sNombreFile);
                    }
                    int column_central = 9;
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        #region "CENTRAL SOLAR"
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets["CENTRAL_SOLAR"];
                        foreach (var oCentral in lCentrales)
                        {
                            ///// Obtengo los datos de los Generadores H///////
                            int iTotPag = 0;
                            int iTotRec = 0;
                            //var lGenH = appEquipo.BuscarEquiposxPadre(oCentral.EQUICODI, 36, 1, ref iTotPag, int.MaxValue);
                            var lGenH = appEquipamiento.ListarEquiposxFiltroPaginado(0, "AF", 36, 0, "", oCentral.Equicodi, 1, int.MaxValue, ref iTotPag, ref iTotRec);
                            lsGeneradores.AddRange(lGenH);
                            /////////////////////////////////////////////////////
                            /// DATOS DE LAS PROPIEDADES DE LA CENTAL///////////
                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oCentral.Equicodi);

                            ws.SetValue(2, column_central, "Solar");
                            ws.SetValue(3, column_central, oCentral.EMPRNOMB.Trim());
                            ws.SetValue(4, column_central, oCentral.Equinomb.Trim());
                            ws.SetValue(6, column_central, oCentral.Equicodi);

                            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                strValor = strValor == null ? "" : strValor.Trim();
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1709:
                                        //centralSolar.Sbrut = strValor;
                                        ws.SetValue(7, column_central, strValor);
                                        break;
                                    case 1710:
                                        //centralSolar.Pinst = strValor;
                                        ws.SetValue(8, column_central, strValor);
                                        break;
                                    case 1711:
                                        //centralSolar.NumMod = strValor;
                                        ws.SetValue(9, column_central, strValor);
                                        break;
                                    case 1712:
                                        //centralSolar.TecnSeg = strValor;
                                        ws.SetValue(10, column_central, strValor);
                                        break;
                                    case 1713:
                                        //centralSolar.AngIncl = strValor;
                                        ws.SetValue(11, column_central, strValor);
                                        break;
                                    case 1714:
                                        //centralSolar.DistMod = strValor;
                                        ws.SetValue(12, column_central, strValor);
                                        break;
                                    case 1715:
                                        //centralSolar.HEqPCAn = strValor;
                                        ws.SetValue(13, column_central, strValor);
                                        break;
                                    case 1716:
                                        //centralSolar.HEqPCMes = strValor;
                                        ws.SetValue(14, column_central, strValor);
                                        break;
                                    case 1717:
                                        //centralSolar.CurGen = strValor;
                                        ws.SetValue(15, column_central, strValor);
                                        break;
                                    case 1718:
                                        //centralSolar.PenMax = strValor;
                                        ws.SetValue(16, column_central, strValor);
                                        break;
                                    case 1719:
                                        //centralSolar.CurPot = strValor;
                                        ws.SetValue(17, column_central, strValor);
                                        break;
                                    case 1720:
                                        //centralSolar.ConTen = strValor;
                                        ws.SetValue(18, column_central, strValor);
                                        break;
                                    case 1721:
                                        //centralSolar.ConFrec = strValor;
                                        ws.SetValue(19, column_central, strValor);
                                        break;
                                    case 1722:
                                        //centralSolar.NivMTen = strValor;
                                        ws.SetValue(20, column_central, strValor);
                                        break;
                                    case 1723:
                                        //centralSolar.IntCC = strValor;
                                        ws.SetValue(21, column_central, strValor);
                                        break;
                                    case 1724:
                                        //centralSolar.DiaUni = strValor;
                                        ws.SetValue(22, column_central, strValor);
                                        break;
                                }
                            }

                            var cell = ws.Cells[2, column_central, 22, column_central];
                            foreach (var celda in cell)
                            {
                                var border = celda.Style.Border;
                                border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cell.AutoFitColumns();

                            column_central++;
                        }
                        #endregion
                        #region "GENERADOR SOLAR"
                        ExcelWorksheet wsgen = xlPackage.Workbook.Worksheets["GENERADOR_SOLAR"];
                        int column_gen = 10;
                        foreach (var oGenerador in lsGeneradores)
                        {
                            var Central = appEquipamiento.GetByIdEqEquipo(oGenerador.Equipadre.Value);
                            var NombreCentral = Central.Equinomb.Trim();
                            var NombreEmpresa = Central.EMPRNOMB.Trim();

                            wsgen.SetValue(2, column_gen, oGenerador.Equinomb.Trim());
                            wsgen.SetValue(3, column_gen, NombreEmpresa);
                            wsgen.SetValue(4, column_gen, NombreCentral);

                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oGenerador.Equicodi);
                            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1725:
                                        // moduloSolar.MFab = strValor;
                                        wsgen.SetValue(7, column_gen, strValor);
                                        break;
                                    case 1726:
                                        //moduloSolar.MMod = strValor;
                                        wsgen.SetValue(8, column_gen, strValor);
                                        break;
                                    case 1727:
                                        //moduloSolar.MTec = strValor;
                                        wsgen.SetValue(9, column_gen, strValor);
                                        break;
                                    case 1728:
                                        //moduloSolar.MPnom = strValor;
                                        wsgen.SetValue(10, column_gen, strValor);
                                        break;
                                    case 1729:
                                        //moduloSolar.MSnom = strValor;
                                        wsgen.SetValue(11, column_gen, strValor);
                                        break;
                                    case 1730:
                                        // moduloSolar.CurPot = strValor;
                                        wsgen.SetValue(12, column_gen, strValor);
                                        break;
                                    case 1731:
                                        // moduloSolar.TempOp = strValor;
                                        wsgen.SetValue(13, column_gen, strValor);
                                        break;
                                    case 1732:
                                        //moduloSolar.CoefTemp = strValor;
                                        wsgen.SetValue(14, column_gen, strValor);
                                        break;
                                    case 1733:
                                        //moduloSolar.InvFab = strValor;
                                        wsgen.SetValue(17, column_gen, strValor);
                                        break;
                                    case 1734:
                                        //moduloSolar.InvMod = strValor;
                                        wsgen.SetValue(18, column_gen, strValor);
                                        break;
                                    case 1735:
                                        //moduloSolar.InvTec = strValor;
                                        wsgen.SetValue(19, column_gen, strValor);
                                        break;
                                    case 1736:
                                        //moduloSolar.InvRend = strValor;
                                        wsgen.SetValue(20, column_gen, strValor);
                                        break;
                                    case 1737:
                                        //moduloSolar.InvPap = strValor;
                                        wsgen.SetValue(21, column_gen, strValor);
                                        break;
                                    case 1738:
                                        //moduloSolar.InvRangT = strValor;
                                        wsgen.SetValue(23, column_gen, strValor);
                                        break;
                                    case 1739:
                                        //moduloSolar.InvTenCMA = strValor;
                                        wsgen.SetValue(24, column_gen, strValor);
                                        break;
                                    case 1740:
                                        //moduloSolar.InvCorrCMA = strValor;
                                        wsgen.SetValue(25, column_gen, strValor);
                                        break;
                                    case 1741:
                                        //moduloSolar.InvPMaxP = strValor;
                                        wsgen.SetValue(26, column_gen, strValor);
                                        break;
                                    case 1742:
                                        //moduloSolar.InvPApaNom = strValor;
                                        wsgen.SetValue(28, column_gen, strValor);
                                        break;
                                    case 1743:
                                        //moduloSolar.InvPActNom = strValor;
                                        wsgen.SetValue(29, column_gen, strValor);
                                        break;
                                    case 1744:
                                        //moduloSolar.InvTenOpeAl = strValor;
                                        wsgen.SetValue(30, column_gen, strValor);
                                        break;
                                    case 1745:
                                        //moduloSolar.InvNumFas = strValor;
                                        wsgen.SetValue(31, column_gen, strValor);
                                        break;
                                    case 1746:
                                        //moduloSolar.InvFrec = strValor;
                                        wsgen.SetValue(32, column_gen, strValor);
                                        break;
                                    case 1747:
                                        //moduloSolar.InvFDTens = strValor;
                                        wsgen.SetValue(33, column_gen, strValor);
                                        break;
                                    case 1748:
                                        //moduloSolar.InvSoft = strValor;
                                        wsgen.SetValue(35, column_gen, strValor);
                                        break;
                                    case 1749:
                                        //moduloSolar.InvContInv = strValor;
                                        wsgen.SetValue(36, column_gen, strValor);
                                        break;
                                    case 1750:
                                        //moduloSolar.InvContFP = strValor;
                                        wsgen.SetValue(37, column_gen, strValor);
                                        break;
                                    case 1751:
                                        //moduloSolar.InvFrecCom = strValor;
                                        wsgen.SetValue(38, column_gen, strValor);
                                        break;
                                    case 1752:
                                        //moduloSolar.InvMetSPMP = strValor;
                                        wsgen.SetValue(39, column_gen, strValor);
                                        break;
                                    case 1753:
                                        //moduloSolar.InvRestAut = strValor;
                                        wsgen.SetValue(40, column_gen, strValor);
                                        break;
                                    case 1754:
                                        //moduloSolar.InvSinc = strValor;
                                        wsgen.SetValue(41, column_gen, strValor);
                                        break;
                                    case 1755:
                                        //moduloSolar.InvProt = strValor;
                                        wsgen.SetValue(42, column_gen, strValor);
                                        break;
                                    case 1756:
                                        //moduloSolar.InvAPT = strValor;
                                        wsgen.SetValue(44, column_gen, strValor);
                                        break;
                                    case 1757:
                                        //moduloSolar.InvAPRSobreT = strValor;
                                        wsgen.SetValue(45, column_gen, strValor);
                                        break;
                                    case 1758:
                                        //moduloSolar.InvAPRSubT = strValor;
                                        wsgen.SetValue(46, column_gen, strValor);
                                        break;
                                    case 1759:
                                        //moduloSolar.InvAPRSobreF = strValor;
                                        wsgen.SetValue(47, column_gen, strValor);
                                        break;
                                    case 1760:
                                        //moduloSolar.InvAPRSubF = strValor;
                                        wsgen.SetValue(48, column_gen, strValor);
                                        break;
                                    case 1761:
                                        //moduloSolar.CTTCC = strValor;
                                        wsgen.SetValue(50, column_gen, strValor);
                                        break;
                                    case 1762:
                                        //moduloSolar.CTTFab = strValor;
                                        wsgen.SetValue(51, column_gen, strValor);
                                        break;
                                    case 1763:
                                        //moduloSolar.CTTTip = strValor;
                                        wsgen.SetValue(52, column_gen, strValor);
                                        break;
                                    case 1764:
                                        //moduloSolar.CTTTen = strValor;
                                        wsgen.SetValue(53, column_gen, strValor);
                                        break;
                                    case 1765:
                                        //moduloSolar.CTTRelT = strValor;
                                        wsgen.SetValue(54, column_gen, strValor);
                                        break;
                                    case 1766:
                                        //moduloSolar.CTTGrupC = strValor;
                                        wsgen.SetValue(55, column_gen, strValor);
                                        break;
                                    case 1767:
                                        //moduloSolar.CTTSnom = strValor;
                                        wsgen.SetValue(56, column_gen, strValor);
                                        break;
                                    case 1768:
                                        //moduloSolar.CTTTensCC = strValor;
                                        wsgen.SetValue(57, column_gen, strValor);
                                        break;
                                    case 1769:
                                        //moduloSolar.CTCFab = strValor;
                                        wsgen.SetValue(58, column_gen, strValor);
                                        break;
                                    case 1770:
                                        //moduloSolar.CTCTip = strValor;
                                        wsgen.SetValue(59, column_gen, strValor);
                                        break;
                                    case 1771:
                                        //moduloSolar.MTTen = strValor;
                                        wsgen.SetValue(61, column_gen, strValor);
                                        break;
                                    case 1772:
                                        //moduloSolar.MTNumC = strValor;
                                        wsgen.SetValue(62, column_gen, strValor);
                                        break;
                                    case 1773:
                                        //moduloSolar.MTLong = strValor;
                                        wsgen.SetValue(63, column_gen, strValor);
                                        break;
                                    case 1774:
                                        //moduloSolar.MTCond = strValor;
                                        wsgen.SetValue(64, column_gen, strValor);
                                        break;
                                    case 1775:
                                        //moduloSolar.MTSecc = strValor;
                                        wsgen.SetValue(65, column_gen, strValor);
                                        break;
                                    case 1776:
                                        //moduloSolar.MTTipAis = strValor;
                                        wsgen.SetValue(66, column_gen, strValor);
                                        break;
                                    case 1777:
                                        //moduloSolar.MTTensAis = strValor;
                                        wsgen.SetValue(67, column_gen, strValor);
                                        break;
                                    case 1778:
                                        //moduloSolar.MTResis = strValor;
                                        wsgen.SetValue(68, column_gen, strValor);
                                        break;
                                    case 1779:
                                        //moduloSolar.MTReac = strValor;
                                        wsgen.SetValue(69, column_gen, strValor);
                                        break;
                                    case 1780:
                                        //moduloSolar.MTSusc = strValor;
                                        wsgen.SetValue(70, column_gen, strValor);
                                        break;
                                    case 1781:
                                        // moduloSolar.SEFab = strValor;
                                        wsgen.SetValue(73, column_gen, strValor);
                                        break;
                                    case 1782:
                                        //moduloSolar.SETip = strValor;
                                        wsgen.SetValue(74, column_gen, strValor);
                                        break;
                                    case 1783:
                                        //moduloSolar.SETen = strValor;
                                        wsgen.SetValue(75, column_gen, strValor);
                                        break;
                                    case 1784:
                                        //moduloSolar.SERelT = strValor;
                                        wsgen.SetValue(76, column_gen, strValor);
                                        break;
                                    case 1785:
                                        //moduloSolar.SEGrupoC = strValor;
                                        wsgen.SetValue(77, column_gen, strValor);
                                        break;
                                    case 1786:
                                        //moduloSolar.SESnom = strValor;
                                        wsgen.SetValue(78, column_gen, strValor);
                                        break;
                                    case 1787:
                                        // moduloSolar.SETensCC = strValor;
                                        wsgen.SetValue(79, column_gen, strValor);
                                        break;
                                    case 1788:
                                        //moduloSolar.SERUbi = strValor;
                                        wsgen.SetValue(80, column_gen, strValor);
                                        break;
                                    case 1789:
                                        //moduloSolar.SERTipR = strValor;
                                        wsgen.SetValue(81, column_gen, strValor);
                                        break;
                                    case 1790:
                                        //moduloSolar.SERAut = strValor;
                                        wsgen.SetValue(82, column_gen, strValor);
                                        break;
                                    case 1791:
                                        //moduloSolar.DiaUni = strValor;
                                        wsgen.SetValue(84, column_gen, strValor);
                                        break;
                                    case 1792:
                                        //moduloSolar.PMM = strValor;
                                        wsgen.SetValue(86, column_gen, strValor);
                                        break;
                                }
                            }

                            var cellGen = wsgen.Cells[2, column_gen, 86, column_gen];
                            foreach (var oCeldaGen in cellGen)
                            {
                                var borde = oCeldaGen.Style.Border;
                                borde.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cellGen.AutoFitColumns();
                            column_gen++;
                        }
                        #endregion
                        xlPackage.Save();
                    }
                    sRutaArchivo = ruta + sNombreFile;
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
            return sRutaArchivo;
        }
        protected string GenerarArchivoDataEolica(IList<EqEquipoDTO> lCentrales)
        {
            string sRutaArchivo = string.Empty;
            if (lCentrales.Count > 0)
            {
                try
                {
                    List<EqEquipoDTO> lsGeneradores = new List<EqEquipoDTO>();
                    string ruta = ConfigurationManager.AppSettings["RutaPlantilla"].ToString();
                    string sNombreFile = "FichaTecnicaEolica.xlsx";
                    FileInfo template = new FileInfo(ruta + "EolicoFichaTecnica.xlsx");
                    FileInfo newFile = new FileInfo(ruta + sNombreFile);
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(ruta + sNombreFile);
                    }
                    int column_central = 4;
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        #region "CENTRAL EOLICA"
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets["CENTRAL_EOLICA"];
                        foreach (var oCentral in lCentrales)
                        {
                            ///// Obtengo los datos de los Generadores H///////
                            int iTotPag = 0;
                            int iTotRec = 0;
                            //var lGenH = appEquipo.BuscarEquiposxPadre(oCentral.EQUICODI, 38, 1, ref iTotPag, int.MaxValue);
                            var lGenH = appEquipamiento.ListarEquiposxFiltroPaginado(0, "AF", 38, 0, "", oCentral.Equicodi, 1, int.MaxValue, ref iTotPag, ref iTotRec);
                            lsGeneradores.AddRange(lGenH);
                            /////////////////////////////////////////////////////
                            /// DATOS DE LAS PROPIEDADES DE LA CENTAL///////////
                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oCentral.Equicodi);

                            ws.SetValue(2, column_central, "Eólica");
                            ws.SetValue(3, column_central, oCentral.EMPRNOMB.Trim());
                            ws.SetValue(4, column_central, oCentral.Equinomb.Trim());
                            ws.SetValue(6, column_central, oCentral.Equicodi);
                            foreach (PropiedadEquipoHistDTO propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                strValor = strValor == null ? "" : strValor.Trim();
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1601:
                                        //CentralEolicaDetalle.PotApaBrut = strValor;
                                        ws.SetValue(7, column_central, strValor);
                                        break;
                                    case 1602:
                                        //CentralEolicaDetalle.PotInstNom = strValor;
                                        ws.SetValue(8, column_central, strValor);
                                        break;
                                    case 1603:
                                        //CentralEolicaDetalle.NumAerog = strValor;
                                        ws.SetValue(9, column_central, strValor);
                                        break;
                                    case 1604:
                                        //CentralEolicaDetalle.HeqPCAno = strValor;
                                        ws.SetValue(10, column_central, strValor);
                                        break;
                                    case 1605:
                                        //CentralEolicaDetalle.HeqPCMes = strValor;
                                        ws.SetValue(11, column_central, strValor);
                                        break;
                                    case 1606:
                                        //CentralEolicaDetalle.CurPotReac = strValor;
                                        ws.SetValue(12, column_central, strValor);
                                        break;
                                    case 1607:
                                        //CentralEolicaDetalle.CurPotPCR = strValor;
                                        ws.SetValue(13, column_central, strValor);
                                        break;
                                    case 1608:
                                        //CentralEolicaDetalle.SistCont = strValor;
                                        ws.SetValue(14, column_central, strValor);
                                        break;
                                    case 1609:
                                        //CentralEolicaDetalle.ContTen = strValor;
                                        ws.SetValue(15, column_central, strValor);
                                        break;
                                    case 1610:
                                        //CentralEolicaDetalle.ContFrec = strValor;
                                        ws.SetValue(16, column_central, strValor);
                                        break;
                                    case 1611:
                                        //CentralEolicaDetalle.NivMedTen = strValor;
                                        ws.SetValue(17, column_central, strValor);
                                        break;
                                    case 1612:
                                        //CentralEolicaDetalle.IntCortCirc = strValor;
                                        ws.SetValue(18, column_central, strValor);
                                        break;
                                    case 1795:
                                        //CentralEolicaDetalle.DiaUnif = strValor;
                                        ws.SetValue(19, column_central, strValor);
                                        break;
                                }
                            }
                            var cell = ws.Cells[2, column_central, 19, column_central];
                            foreach (var celda in cell)
                            {
                                var border = celda.Style.Border;
                                border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cell.AutoFitColumns();
                            column_central++;
                        }

                        #endregion
                        #region "GENERADOR EOLICO"
                        ExcelWorksheet wsGen = xlPackage.Workbook.Worksheets["GENERADOR_EOLICO"];
                        int column_gen = 10;
                        foreach (var oGenerador in lsGeneradores)
                        {
                            var Central = appEquipamiento.GetByIdEqEquipo(oGenerador.Equipadre.Value);
                            var NombreCentral = Central.Equinomb;
                            var NombreEmpresa = Central.EMPRNOMB;

                            wsGen.SetValue(2, column_gen, oGenerador.Equinomb.Trim());
                            wsGen.SetValue(3, column_gen, NombreEmpresa.Trim());
                            wsGen.SetValue(4, column_gen, NombreCentral.Trim());

                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oGenerador.Equicodi);

                            foreach (var propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                strValor = strValor == null ? "" : strValor.Trim();
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1613:
                                        //aerogenerador.AeFab = strValor;
                                        wsGen.SetValue(7, column_gen, strValor);
                                        break;
                                    case 1614:
                                        //aerogenerador.AeMod = strValor;
                                        wsGen.SetValue(8, column_gen, strValor);
                                        break;
                                    case 1615:
                                        // aerogenerador.AeTec = strValor;
                                        wsGen.SetValue(9, column_gen, strValor);
                                        break;
                                    case 1616:
                                        //aerogenerador.AePnom = strValor;
                                        wsGen.SetValue(10, column_gen, strValor);
                                        break;
                                    case 1617:
                                        //aerogenerador.AeSnom = strValor;
                                        wsGen.SetValue(11, column_gen, strValor);
                                        break;
                                    case 1618:
                                        //aerogenerador.AeTnom = strValor;
                                        wsGen.SetValue(12, column_gen, strValor);
                                        break;
                                    case 1619:
                                        //aerogenerador.AeCurvPQ = strValor;
                                        wsGen.SetValue(13, column_gen, strValor);
                                        break;
                                    case 1620:
                                        //aerogenerador.AeCurvPot = strValor;
                                        wsGen.SetValue(14, column_gen, strValor);
                                        break;
                                    case 1621:
                                        //aerogenerador.AeVeonex = strValor;
                                        wsGen.SetValue(15, column_gen, strValor);
                                        break;
                                    case 1622:
                                        //aerogenerador.AeVVient = strValor;
                                        wsGen.SetValue(16, column_gen, strValor);
                                        break;
                                    case 1623:
                                        //aerogenerador.AeVDesc = strValor;
                                        wsGen.SetValue(17, column_gen, strValor);
                                        break;
                                    case 1624:
                                        //aerogenerador.AeRotDiam = strValor;
                                        wsGen.SetValue(19, column_gen, strValor);
                                        break;
                                    case 1625:
                                        //aerogenerador.AeRotArea = strValor;
                                        wsGen.SetValue(20, column_gen, strValor);
                                        break;
                                    case 1626:
                                        //aerogenerador.AeRotNumPal = strValor;
                                        wsGen.SetValue(21, column_gen, strValor);
                                        break;
                                    case 1627:
                                        // aerogenerador.AeRotPos = strValor;
                                        wsGen.SetValue(22, column_gen, strValor);
                                        break;
                                    case 1628:
                                        //aerogenerador.AeRotVelNom = strValor;
                                        wsGen.SetValue(23, column_gen, strValor);
                                        break;
                                    case 1629:
                                        //aerogenerador.AeRotRang = strValor;
                                        wsGen.SetValue(24, column_gen, strValor);
                                        break;
                                    case 1630:
                                        //aerogenerador.AeRotLongPal = strValor;
                                        wsGen.SetValue(25, column_gen, strValor);
                                        break;
                                    case 1631:
                                        //aerogenerador.AeCajTip = strValor;
                                        wsGen.SetValue(27, column_gen, strValor);
                                        break;
                                    case 1632:
                                        //aerogenerador.AeCajRelMult = strValor;
                                        wsGen.SetValue(28, column_gen, strValor);
                                        break;
                                    case 1633:
                                        //aerogenerador.AeTorrTip = strValor;
                                        wsGen.SetValue(30, column_gen, strValor);
                                        break;
                                    case 1634:
                                        //aerogenerador.AeTorrMat = strValor;
                                        wsGen.SetValue(31, column_gen, strValor);
                                        break;
                                    case 1635:
                                        //aerogenerador.AeTorrLong = strValor;
                                        wsGen.SetValue(32, column_gen, strValor);
                                        break;
                                    case 1636:
                                        //aerogenerador.AeGenFab = strValor;
                                        wsGen.SetValue(34, column_gen, strValor);
                                        break;
                                    case 1637:
                                        //aerogenerador.AeGenTip = strValor;
                                        wsGen.SetValue(35, column_gen, strValor);
                                        break;
                                    case 1638:
                                        //aerogenerador.AeGenPNom = strValor;
                                        wsGen.SetValue(36, column_gen, strValor);
                                        break;
                                    case 1639:
                                        //aerogenerador.AeGenPApar = strValor;
                                        wsGen.SetValue(37, column_gen, strValor);
                                        break;
                                    case 1640:
                                        //aerogenerador.AeGenVelNom = strValor;
                                        wsGen.SetValue(38, column_gen, strValor);
                                        break;
                                    case 1641:
                                        //aerogenerador.AeGenRang = strValor;
                                        wsGen.SetValue(39, column_gen, strValor);
                                        break;
                                    case 1642:
                                        //aerogenerador.AeGenTenNom = strValor;
                                        wsGen.SetValue(40, column_gen, strValor);
                                        break;
                                    case 1643:
                                        //aerogenerador.AeGenFrec = strValor;
                                        wsGen.SetValue(41, column_gen, strValor);
                                        break;
                                    case 1644:
                                        //aerogenerador.AeGenDesNom = strValor;
                                        wsGen.SetValue(42, column_gen, strValor);
                                        break;
                                    case 1645:
                                        //aerogenerador.AeGenTemp = strValor;
                                        wsGen.SetValue(43, column_gen, strValor);
                                        break;
                                    case 1646:
                                        //aerogenerador.AeGenCurvPot = strValor;
                                        wsGen.SetValue(44, column_gen, strValor);
                                        break;
                                    case 1647:
                                        //aerogenerador.AeConvFab = strValor;
                                        wsGen.SetValue(46, column_gen, strValor);
                                        break;
                                    case 1648:
                                        //aerogenerador.AeConvTip = strValor;
                                        wsGen.SetValue(47, column_gen, strValor);
                                        break;
                                    case 1649:
                                        //aerogenerador.AeConvSoft = strValor;
                                        wsGen.SetValue(48, column_gen, strValor);
                                        break;
                                    case 1650:
                                        //aerogenerador.AeConvTen = strValor;
                                        wsGen.SetValue(49, column_gen, strValor);
                                        break;
                                    case 1651:
                                        //aerogenerador.AeConvSnom = strValor;
                                        wsGen.SetValue(50, column_gen, strValor);
                                        break;
                                    case 1652:
                                        //aerogenerador.AeSCSoft = strValor;
                                        wsGen.SetValue(52, column_gen, strValor);
                                        break;
                                    case 1653:
                                        //aerogenerador.AeSCAngPala = strValor;
                                        wsGen.SetValue(53, column_gen, strValor);
                                        break;
                                    case 1654:
                                        //aerogenerador.AeSCOriBar = strValor;
                                        wsGen.SetValue(54, column_gen, strValor);
                                        break;
                                    case 1655:
                                        //aerogenerador.AeSCVel = strValor;
                                        wsGen.SetValue(55, column_gen, strValor);
                                        break;
                                    case 1656:
                                        //aerogenerador.AeSCTen = strValor;
                                        wsGen.SetValue(56, column_gen, strValor);
                                        break;
                                    case 1657:
                                        //aerogenerador.AeSCFrec = strValor;
                                        wsGen.SetValue(57, column_gen, strValor);
                                        break;
                                    case 1658:
                                        //aerogenerador.AeNa10 = strValor;
                                        wsGen.SetValue(59, column_gen, strValor);
                                        break;
                                    case 1659:
                                        //aerogenerador.AeNa120 = strValor;
                                        wsGen.SetValue(60, column_gen, strValor);
                                        break;
                                    case 1660:
                                        //aerogenerador.AeNn10 = strValor;
                                        wsGen.SetValue(61, column_gen, strValor);
                                        break;
                                    case 1661:
                                        //aerogenerador.AeNn120 = strValor;
                                        wsGen.SetValue(62, column_gen, strValor);
                                        break;
                                    case 1662:
                                        //aerogenerador.AeAjPTip = strValor;
                                        wsGen.SetValue(63, column_gen, strValor);
                                        break;
                                    case 1663:
                                        //aerogenerador.AeAjPSobrT = strValor;
                                        wsGen.SetValue(64, column_gen, strValor);
                                        break;
                                    case 1664:
                                        //aerogenerador.AeAjPSubTen = strValor;
                                        wsGen.SetValue(65, column_gen, strValor);
                                        break;
                                    case 1665:
                                        //aerogenerador.AeAjPSobrF = strValor;
                                        wsGen.SetValue(66, column_gen, strValor);
                                        break;
                                    case 1666:
                                        //aerogenerador.AeAjPSubF = strValor;
                                        wsGen.SetValue(67, column_gen, strValor);
                                        break;
                                    case 1667:
                                        //aerogenerador.CTTFab = strValor;
                                        wsGen.SetValue(69, column_gen, strValor);
                                        break;
                                    case 1668:
                                        //aerogenerador.CTTTip = strValor;
                                        wsGen.SetValue(70, column_gen, strValor);
                                        break;
                                    case 1669:
                                        //aerogenerador.CTTTen = strValor;
                                        wsGen.SetValue(71, column_gen, strValor);
                                        break;
                                    case 1670:
                                        //aerogenerador.CTTRelTr = strValor;
                                        wsGen.SetValue(72, column_gen, strValor);
                                        break;
                                    case 1671:
                                        //aerogenerador.CTTGrupC = strValor;
                                        wsGen.SetValue(73, column_gen, strValor);
                                        break;
                                    case 1672:
                                        //aerogenerador.CTTSnom = strValor;
                                        wsGen.SetValue(74, column_gen, strValor);
                                        break;
                                    case 1673:
                                        //aerogenerador.CTTTenCC = strValor;
                                        wsGen.SetValue(75, column_gen, strValor);
                                        break;
                                    case 1674:
                                        // aerogenerador.CTCFab = strValor;
                                        wsGen.SetValue(76, column_gen, strValor);
                                        break;
                                    case 1675:
                                        //aerogenerador.CTCTip = strValor;
                                        wsGen.SetValue(77, column_gen, strValor);
                                        break;
                                    case 1676:
                                        //aerogenerador.RMTTen = strValor;
                                        wsGen.SetValue(79, column_gen, strValor);
                                        break;
                                    case 1677:
                                        //aerogenerador.RMTNumC = strValor;
                                        wsGen.SetValue(80, column_gen, strValor);
                                        break;
                                    case 1678:
                                        //aerogenerador.RMTLong = strValor;
                                        wsGen.SetValue(81, column_gen, strValor);
                                        break;
                                    case 1679:
                                        //aerogenerador.RMTCond = strValor;
                                        wsGen.SetValue(82, column_gen, strValor);
                                        break;
                                    case 1680:
                                        //aerogenerador.RMTSec = strValor;
                                        wsGen.SetValue(83, column_gen, strValor);
                                        break;
                                    case 1681:
                                        //aerogenerador.RMTAisl = strValor;
                                        wsGen.SetValue(84, column_gen, strValor);
                                        break;
                                    case 1682:
                                        //aerogenerador.RMTTenAisl = strValor;
                                        wsGen.SetValue(85, column_gen, strValor);
                                        break;
                                    case 1683:
                                        //aerogenerador.RMTResis = strValor;
                                        wsGen.SetValue(86, column_gen, strValor);
                                        break;
                                    case 1684:
                                        //aerogenerador.RMTReac = strValor;
                                        wsGen.SetValue(87, column_gen, strValor);
                                        break;
                                    case 1685:
                                        //aerogenerador.RMTSusc = strValor;
                                        wsGen.SetValue(88, column_gen, strValor);
                                        break;
                                    case 1686:
                                        //aerogenerador.SETCFab = strValor;
                                        wsGen.SetValue(91, column_gen, strValor);
                                        break;
                                    case 1687:
                                        //aerogenerador.SETCTip = strValor;
                                        wsGen.SetValue(92, column_gen, strValor);
                                        break;
                                    case 1688:
                                        //aerogenerador.SETCTen = strValor;
                                        wsGen.SetValue(93, column_gen, strValor);
                                        break;
                                    case 1689:
                                        //aerogenerador.SETCRelTr = strValor;
                                        wsGen.SetValue(94, column_gen, strValor);
                                        break;
                                    case 1690:
                                        //aerogenerador.SETCGrupC = strValor;
                                        wsGen.SetValue(95, column_gen, strValor);
                                        break;
                                    case 1691:
                                        //aerogenerador.SETCSnom = strValor;
                                        wsGen.SetValue(96, column_gen, strValor);
                                        break;
                                    case 1692:
                                        //aerogenerador.SETCTenCC = strValor;
                                        wsGen.SetValue(97, column_gen, strValor);
                                        break;
                                    case 1693:
                                        //aerogenerador.SETCRegUbi = strValor;
                                        wsGen.SetValue(98, column_gen, strValor);
                                        break;
                                    case 1694:
                                        //aerogenerador.SETCRegTip = strValor;
                                        wsGen.SetValue(99, column_gen, strValor);
                                        break;
                                    case 1695:
                                        //aerogenerador.SETCRegAut = strValor;
                                        wsGen.SetValue(100, column_gen, strValor);
                                        break;
                                    case 1696:
                                        //aerogenerador.SETCRegNumT = strValor;
                                        wsGen.SetValue(101, column_gen, strValor);
                                        break;
                                    case 1697:
                                        //aerogenerador.SETCRegRang = strValor;
                                        wsGen.SetValue(102, column_gen, strValor);
                                        break;
                                    case 1698:
                                        //aerogenerador.SEMTFab = strValor;
                                        wsGen.SetValue(104, column_gen, strValor);
                                        break;
                                    case 1699:
                                        //aerogenerador.SEMTTip = strValor;
                                        wsGen.SetValue(105, column_gen, strValor);
                                        break;
                                    case 1700:
                                        //aerogenerador.SECRTip = strValor;
                                        wsGen.SetValue(107, column_gen, strValor);
                                        break;
                                    case 1701:
                                        //aerogenerador.SECREPot = strValor;
                                        wsGen.SetValue(108, column_gen, strValor);
                                        break;
                                    case 1702:
                                        //aerogenerador.SECRENum = strValor;
                                        wsGen.SetValue(109, column_gen, strValor);
                                        break;
                                    case 1703:
                                        //aerogenerador.SECRETip = strValor;
                                        wsGen.SetValue(110, column_gen, strValor);
                                        break;
                                    case 1704:
                                        //aerogenerador.SECRDTen = strValor;
                                        wsGen.SetValue(111, column_gen, strValor);
                                        break;
                                    case 1705:
                                        //aerogenerador.SECRDPot = strValor;
                                        wsGen.SetValue(112, column_gen, strValor);
                                        break;
                                    case 1706:
                                        //aerogenerador.SECRDCont = strValor;
                                        wsGen.SetValue(113, column_gen, strValor);
                                        break;
                                    case 1707:
                                        //aerogenerador.DS_DiaUni = strValor;
                                        wsGen.SetValue(115, column_gen, strValor);
                                        break;
                                }
                            }

                            var cellgen = wsGen.Cells[2, column_gen, 115, column_gen];
                            foreach (var celda in cellgen)
                            {
                                var border = celda.Style.Border;
                                border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cellgen.AutoFitColumns();
                            column_gen++;
                        }

                        #endregion
                        xlPackage.Save();
                    }
                    sRutaArchivo = ruta + sNombreFile;

                }
                catch (Exception e)
                {
                    log.Error(e);
                }

            }
            return sRutaArchivo;
        }
        protected string GenerarArchivoDataTermica(IList<EqEquipoDTO> lCentrales)
        {
            string sRutaArchivo = string.Empty;
            if (lCentrales.Count > 0)
            {
                try
                {
                    List<EqEquipoDTO> lsGeneradores = new List<EqEquipoDTO>();
                    List<ModoOperacionDTO> lsModos = new List<ModoOperacionDTO>();
                    //appModoOperacion.ListadoModosOperacion((short)iCentral);
                    string ruta = ConfigurationManager.AppSettings["RutaPlantilla"].ToString();
                    string sNombreFile = "FichaTecnicaTermoElectrica.xlsx";
                    FileInfo template = new FileInfo(ruta + "TermoFichaTecnica.xlsx");
                    FileInfo newFile = new FileInfo(ruta + sNombreFile);

                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(ruta + sNombreFile);
                    }
                    int column_central = 9;
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
                    {
                        #region "CENTRAL TERMICA"
                        ExcelWorksheet ws = xlPackage.Workbook.Worksheets["CENTRAL_TERMOELECTRICA"];
                        foreach (var oCentral in lCentrales)
                        {
                            ///// Obtengo los datos de los Generadores H///////
                            int iTotPag = 0;
                            int iTotRec = 0;
                            //var lGenH = appEquipo.BuscarEquiposxPadre(oCentral.EQUICODI, 3, 1, ref iTotPag, int.MaxValue);
                            var lGenH = appEquipamiento.ListarEquiposxFiltroPaginado(0, "AF", 3, 0, "", oCentral.Equicodi, 1, int.MaxValue, ref iTotPag, ref iTotRec);
                            lsGeneradores.AddRange(lGenH);
                            ///Obtener Listado Modos
                            var lMods = appDespacho.ListadoModosOperacionPorCentral(oCentral.Equicodi);
                            lsModos.AddRange(lMods);
                            /////////////////////////////////////////////////////
                            /// DATOS DE LAS PROPIEDADES DE LA CENTAL///////////
                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oCentral.Equicodi);

                            ws.SetValue(2, column_central, "Termoeléctrica");
                            ws.SetValue(3, column_central, oCentral.EMPRNOMB.Trim());
                            ws.SetValue(4, column_central, oCentral.Equinomb.Trim());
                            ws.SetValue(6, column_central, oCentral.Equicodi);
                            ws.SetValue(7, column_central, oCentral.Equinomb.Trim());

                            foreach (var propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                strValor = strValor == null ? "" : strValor.Trim();
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1515:
                                        //centraltermica.NumUnid = strValor;
                                        ws.SetValue(8, column_central, strValor);
                                        break;
                                    case 1516:
                                        //centraltermica.Tipo = strValor;
                                        ws.SetValue(9, column_central, strValor);
                                        break;
                                    case 1517:
                                        //centraltermica.Comb = strValor;
                                        ws.SetValue(10, column_central, strValor);
                                        break;
                                    case 1518:
                                        //centraltermica.Modos = strValor;
                                        ws.SetValue(11, column_central, strValor);
                                        break;
                                    case 1519:
                                        //centraltermica.CapacComb = strValor;
                                        ws.SetValue(12, column_central, strValor);
                                        break;
                                    case 1520:
                                        //centraltermica.PresMinGN = strValor;
                                        ws.SetValue(13, column_central, strValor);
                                        break;
                                    case 1521:
                                        //centraltermica.ContCentral = strValor;
                                        ws.SetValue(14, column_central, strValor);
                                        break;
                                    case 1522:
                                        //centraltermica.Pefect = strValor;
                                        ws.SetValue(15, column_central, strValor);
                                        break;
                                    case 49:
                                        //centraltermica.Pinst = strValor;
                                        ws.SetValue(16, column_central, strValor);
                                        break;
                                    case 50:
                                        //centraltermica.PinstAp = strValor;
                                        ws.SetValue(17, column_central, strValor);
                                        break;
                                    case 51:
                                        //centraltermica.PnomMW = strValor;
                                        ws.SetValue(18, column_central, strValor);
                                        break;
                                    case 52:
                                        //centraltermica.PnomMVA = strValor;
                                        ws.SetValue(19, column_central, strValor);
                                        break;
                                    case 942:
                                        //centraltermica.Pmín = strValor;
                                        ws.SetValue(20, column_central, strValor);
                                        break;
                                    case 944:
                                        //centraltermica.Pmáx = strValor;
                                        ws.SetValue(21, column_central, strValor);
                                        break;
                                    case 284:
                                        //centraltermica.Vgmin = strValor;
                                        ws.SetValue(22, column_central, strValor);
                                        break;
                                    case 285:
                                        //centraltermica.Vgmax = strValor;
                                        ws.SetValue(23, column_central, strValor);
                                        break;
                                    case 286:
                                        //centraltermica.Vemin = strValor;
                                        ws.SetValue(24, column_central, strValor);
                                        break;
                                    case 287:
                                        //centraltermica.VeMáx = strValor;
                                        ws.SetValue(25, column_central, strValor);
                                        break;
                                    case 1523:
                                        //centraltermica.SSAA = strValor;
                                        ws.SetValue(26, column_central, strValor);
                                        break;
                                    case 1524:
                                        //centraltermica.Dunifil = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        ws.SetValue(27, column_central, strValor);
                                        break;
                                }
                            }
                            var cell = ws.Cells[2, column_central, 27, column_central];
                            foreach (var celda in cell)
                            {
                                var border = celda.Style.Border;
                                border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cell.AutoFitColumns();
                            column_central++;
                        }

                        #endregion
                        #region "GENERADOR TERMICO"
                        ExcelWorksheet wsGen = xlPackage.Workbook.Worksheets["GENERADOR_TERMOELEC"];
                        int column_gen = 11;
                        foreach (var oGenerador in lsGeneradores)
                        {
                            var Central = appEquipamiento.GetByIdEqEquipo(oGenerador.Equipadre.Value);
                            var NombreCentral = Central.Equinomb;
                            var NombreEmpresa = Central.EMPRNOMB;

                            wsGen.SetValue(2, column_gen, oGenerador.Equinomb.Trim());
                            wsGen.SetValue(3, column_gen, NombreEmpresa.Trim());
                            wsGen.SetValue(4, column_gen, NombreCentral.Trim());
                            wsGen.SetValue(6, column_gen, Central.Equicodi);
                            wsGen.SetValue(7, column_gen, oGenerador.Equicodi);
                            var Propiedades = appEquipamiento.ListarDatosPropiedadesFichaTecnicaVigentesxEquipo(oGenerador.Equicodi);
                            foreach (var propiedadEquipoHistDto in Propiedades)
                            {
                                string strValor = string.Empty;
                                strValor = DeterminarValorPropiedad(propiedadEquipoHistDto);
                                strValor = strValor == null ? "" : strValor.Trim();
                                switch ((int)propiedadEquipoHistDto.PROPCODI)
                                {
                                    case 1555:
                                        //GeneradorTermico.DatPlaca = strValor;
                                        wsGen.SetValue(8, column_gen, strValor);
                                        break;
                                    case 1556:
                                        //GeneradorTermico.Moper = strValor;
                                        wsGen.SetValue(9, column_gen, strValor);
                                        break;
                                    case 1557:
                                        //GeneradorTermico.IngCom = strValor;
                                        wsGen.SetValue(10, column_gen, strValor);
                                        break;
                                    case 1558:
                                        //GeneradorTermico.Tecnolog = strValor;
                                        wsGen.SetValue(12, column_gen, strValor);
                                        break;
                                    case 1559:
                                        //GeneradorTermico.Tipo = strValor;
                                        wsGen.SetValue(13, column_gen, strValor);
                                        break;
                                    case 1560:
                                        //GeneradorTermico.Fabr = strValor;
                                        wsGen.SetValue(14, column_gen, strValor);
                                        break;
                                    case 1561:
                                        //GeneradorTermico.Modelo = strValor;
                                        wsGen.SetValue(15, column_gen, strValor);
                                        break;
                                    case 1562:
                                        //GeneradorTermico.Serie = strValor;
                                        wsGen.SetValue(16, column_gen, strValor);
                                        break;
                                    case 1563:
                                        //GeneradorTermico.Pinst = strValor;
                                        wsGen.SetValue(17, column_gen, strValor);
                                        break;
                                    case 189:
                                        //GeneradorTermico.Pnom = strValor;
                                        wsGen.SetValue(18, column_gen, strValor);
                                        break;
                                    case 313:
                                        //GeneradorTermico.Snom = strValor;
                                        wsGen.SetValue(19, column_gen, strValor);
                                        break;
                                    //case 980:
                                    //    //GeneradorTermico.Pnom_30ps = strValor;
                                    //    wsGen.SetValue(20, column_gen, strValor);
                                    //    break;
                                    //case 1564:
                                    //    //GeneradorTermico.Pnom_15ps = strValor;
                                    //    break;
                                    //case 979:
                                    //    //GeneradorTermico.Pnom_05ps = strValor;
                                    //    break;
                                    case 1565:
                                        //GeneradorTermico.Psincr = strValor;
                                        wsGen.SetValue(20, column_gen, strValor);
                                        break;
                                    case 354:
                                        //GeneradorTermico.RPM = strValor;
                                        wsGen.SetValue(21, column_gen, strValor);
                                        break;
                                    case 1566:
                                        //GeneradorTermico.GNaDB5 = strValor;
                                        wsGen.SetValue(22, column_gen, strValor);
                                        break;
                                    case 1567:
                                        //GeneradorTermico.DB5aGN = strValor;
                                        wsGen.SetValue(23, column_gen, strValor);
                                        break;
                                    case 1568:
                                        //GeneradorTermico.DB5aRes = strValor;
                                        wsGen.SetValue(24, column_gen, strValor);
                                        break;
                                    case 1569:
                                        //GeneradorTermico.ResaDB5 = strValor;
                                        wsGen.SetValue(25, column_gen, strValor);
                                        break;
                                    case 1570:
                                        //GeneradorTermico.PresVap = strValor;
                                        wsGen.SetValue(26, column_gen, strValor);
                                        break;
                                    case 1571:
                                        //GeneradorTermico.TempVap = strValor;
                                        wsGen.SetValue(27, column_gen, strValor);
                                        break;
                                    case 1572:
                                        //GeneradorTermico.CaudVap = strValor;
                                        wsGen.SetValue(28, column_gen, strValor);
                                        break;
                                    case 1573:
                                        //GeneradorTermico.PresVacCond = strValor;
                                        wsGen.SetValue(29, column_gen, strValor);
                                        break;
                                    case 1574:
                                        //GeneradorTermico.NumCalent = strValor;
                                        wsGen.SetValue(30, column_gen, strValor);
                                        break;
                                    case 1575:
                                        //GeneradorTermico.NumExtracc = strValor;
                                        wsGen.SetValue(31, column_gen, strValor);
                                        break;
                                    case 1576:
                                        //GeneradorTermico.NumetapComp = strValor;
                                        wsGen.SetValue(32, column_gen, strValor);
                                        break;
                                    case 1577:
                                        //GeneradorTermico.NumetapTurb = strValor;
                                        wsGen.SetValue(33, column_gen, strValor);
                                        break;
                                    case 1578:
                                        //GeneradorTermico.TempAir = strValor;
                                        wsGen.SetValue(34, column_gen, strValor);
                                        break;
                                    case 1579:
                                        //GeneradorTermico.PresAir = strValor;
                                        wsGen.SetValue(35, column_gen, strValor);
                                        break;
                                    case 1580:
                                        //GeneradorTermico.FlujoGases = strValor;
                                        wsGen.SetValue(36, column_gen, strValor);
                                        break;
                                    case 1581:
                                        //GeneradorTermico.TempGases = strValor;
                                        wsGen.SetValue(37, column_gen, strValor);
                                        break;
                                    case 1582:
                                        //GeneradorTermico.DBCF = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsGen.SetValue(38, column_gen, strValor);
                                        break;
                                    case 1583:
                                        //GeneradorTermico.PotGener = strValor;
                                        wsGen.SetValue(41, column_gen, strValor);
                                        break;
                                    case 1584:
                                        //GeneradorTermico.VelGener = strValor;
                                        wsGen.SetValue(42, column_gen, strValor);
                                        break;
                                    case 1585:
                                        //GeneradorTermico.VelEmbGener = strValor;
                                        wsGen.SetValue(43, column_gen, strValor);
                                        break;
                                    case 1586:
                                        //GeneradorTermico.Numpolos = strValor;
                                        wsGen.SetValue(44, column_gen, strValor);
                                        break;
                                    case 196:
                                        //GeneradorTermico.Curva_Cap = strValor;
                                        wsGen.SetValue(45, column_gen, strValor);
                                        break;
                                    case 195:
                                        //GeneradorTermico.G_capac_min = strValor;
                                        wsGen.SetValue(46, column_gen, strValor);
                                        break;
                                    case 198:
                                        //GeneradorTermico.G_capac_50 = strValor;
                                        wsGen.SetValue(47, column_gen, strValor);
                                        break;
                                    case 200:
                                        //GeneradorTermico.G_capac_100 = strValor;
                                        wsGen.SetValue(48, column_gen, strValor);
                                        break;
                                    case 192:
                                        //GeneradorTermico.G_ind_min = strValor;
                                        wsGen.SetValue(49, column_gen, strValor);
                                        break;
                                    case 199:
                                        //GeneradorTermico.G_ind_50 = strValor;
                                        wsGen.SetValue(50, column_gen, strValor);
                                        break;
                                    case 201:
                                        //GeneradorTermico.G_ind_100 = strValor;
                                        wsGen.SetValue(51, column_gen, strValor);
                                        break;
                                    case 1001:
                                        //GeneradorTermico.Vn_GEN = strValor;
                                        wsGen.SetValue(52, column_gen, strValor);
                                        break;
                                    case 193:
                                        //GeneradorTermico.Vmin_GEN = strValor;
                                        wsGen.SetValue(53, column_gen, strValor);
                                        break;
                                    case 194:
                                        //GeneradorTermico.Vmax_GEN = strValor;
                                        wsGen.SetValue(54, column_gen, strValor);
                                        break;
                                    case 208:
                                        //GeneradorTermico.Vmin_Exc = strValor;
                                        wsGen.SetValue(55, column_gen, strValor);
                                        break;
                                    case 209:
                                        //GeneradorTermico.Vmax_Exc = strValor;
                                        wsGen.SetValue(56, column_gen, strValor);
                                        break;
                                    case 1002:
                                        //GeneradorTermico.Vmin_SSAA = strValor;
                                        wsGen.SetValue(57, column_gen, strValor);
                                        break;
                                    case 1003:
                                        //GeneradorTermico.Vmax_SSAA = strValor;
                                        wsGen.SetValue(58, column_gen, strValor);
                                        break;
                                    case 190:
                                        //GeneradorTermico.ARR_BS = strValor;
                                        wsGen.SetValue(59, column_gen, strValor);
                                        break;
                                    case 336:
                                        // GeneradorTermico.ReactanciasincrónicaEjeDirecto = strValor;
                                        wsGen.SetValue(60, column_gen, strValor);
                                        break;
                                    case 337:
                                        //GeneradorTermico.ReactanciaTransitoriaEjeDirecto = strValor;
                                        wsGen.SetValue(61, column_gen, strValor);
                                        break;
                                    case 338:
                                        //GeneradorTermico.ReactanciaSubtransitoriaEjeDirecto = strValor;
                                        wsGen.SetValue(62, column_gen, strValor);
                                        break;
                                    case 339:
                                        //GeneradorTermico.X2 = strValor;
                                        wsGen.SetValue(66, column_gen, strValor);
                                        break;
                                    case 340:
                                        //GeneradorTermico.Xo = strValor;
                                        wsGen.SetValue(64, column_gen, strValor);
                                        break;
                                    case 341:
                                        //GeneradorTermico.Ro = strValor;
                                        wsGen.SetValue(65, column_gen, strValor);
                                        break;
                                    case 342:
                                        //GeneradorTermico.R2 = strValor;
                                        wsGen.SetValue(63, column_gen, strValor);
                                        break;
                                    case 343:
                                        //GeneradorTermico.Xn = strValor;
                                        wsGen.SetValue(67, column_gen, strValor);
                                        break;
                                    case 344:
                                        //GeneradorTermico.Rn = strValor;
                                        wsGen.SetValue(68, column_gen, strValor);
                                        break;
                                    case 355:
                                        //GeneradorTermico.H = strValor;
                                        wsGen.SetValue(69, column_gen, strValor);
                                        break;
                                    case 375:
                                        //GeneradorTermico.ReactanciaSincrónicaEjeCuadratura = strValor;
                                        wsGen.SetValue(70, column_gen, strValor);
                                        break;
                                    case 376:
                                        //GeneradorTermico.ReactanciaTransitoriaEjeCuadratura = strValor;
                                        wsGen.SetValue(71, column_gen, strValor);
                                        break;
                                    case 377:
                                        //GeneradorTermico.ReactanciaSubtransitoriaEjeCuadratura = strValor;
                                        wsGen.SetValue(72, column_gen, strValor);
                                        break;
                                    case 378:
                                        //GeneradorTermico.CteTtiempoTranEjeDirectCircAbierto = strValor;
                                        wsGen.SetValue(73, column_gen, strValor);
                                        break;
                                    case 379:
                                        //GeneradorTermico.CteTiempoSubtransitoriaEjeDirCircAbierto = strValor;
                                        wsGen.SetValue(74, column_gen, strValor);
                                        break;
                                    case 380:
                                        //GeneradorTermico.CteTiempoTransitoriaEjeDirCortocircuito = strValor;
                                        wsGen.SetValue(75, column_gen, strValor);
                                        break;
                                    case 381:
                                        //GeneradorTermico.CteTiempoSubtransitoriaEjeDirCortocircuito = strValor;
                                        wsGen.SetValue(76, column_gen, strValor);
                                        break;
                                    case 382:
                                        //GeneradorTermico.CteTiempoTransitoriaEjeCuadraturaCircuitoAbiert = strValor;
                                        wsGen.SetValue(77, column_gen, strValor);
                                        break;
                                    case 383:
                                        //GeneradorTermico.CteTiempoSubtransitoriaEjeCuadraturaCircuitoAbierto = strValor;
                                        wsGen.SetValue(78, column_gen, strValor);
                                        break;
                                    case 384:
                                        //GeneradorTermico.Ta = strValor;
                                        wsGen.SetValue(79, column_gen, strValor);
                                        break;
                                    case 507:
                                        //GeneradorTermico.GD2 = strValor;
                                        wsGen.SetValue(80, column_gen, strValor);
                                        break;
                                    case 508:
                                        //GeneradorTermico.Xp = strValor;
                                        wsGen.SetValue(81, column_gen, strValor);
                                        break;
                                    case 509:
                                        //GeneradorTermico.XL = strValor;
                                        wsGen.SetValue(82, column_gen, strValor);
                                        break;
                                    case 510:
                                        //GeneradorTermico.CteTiempoSubtransitoriaEjeCuadraturaCortocircuito = strValor;
                                        wsGen.SetValue(83, column_gen, strValor);
                                        break;
                                    case 511:
                                        //GeneradorTermico.Ra20C = strValor;
                                        wsGen.SetValue(84, column_gen, strValor);
                                        break;
                                    case 512:
                                        //GeneradorTermico.SCR = strValor;
                                        wsGen.SetValue(85, column_gen, strValor);
                                        break;
                                    case 513:
                                        //GeneradorTermico.S10 = strValor;
                                        wsGen.SetValue(86, column_gen, strValor);
                                        break;
                                    case 514:
                                        //GeneradorTermico.S12 = strValor;
                                        wsGen.SetValue(87, column_gen, strValor);
                                        break;
                                    case 1005:
                                        //GeneradorTermico.Prot_sob_f = strValor;
                                        wsGen.SetValue(88, column_gen, strValor);
                                        break;
                                    case 1008:
                                        //GeneradorTermico.CteTiempoTransitoriaEjeCuadraturaCortocircuito = strValor;
                                        wsGen.SetValue(89, column_gen, strValor);
                                        break;
                                    case 1009:
                                        //GeneradorTermico.Iexco_1pu = strValor;
                                        wsGen.SetValue(90, column_gen, strValor);
                                        break;
                                    case 1010:
                                        //GeneradorTermico.Iexco_12p = strValor;
                                        wsGen.SetValue(91, column_gen, strValor);
                                        break;
                                    case 1011:
                                        //GeneradorTermico.DBRTE = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsGen.Cells[92, column_gen, 94, column_gen].Merge = true;
                                        wsGen.SetValue(92, column_gen, strValor);
                                        break;
                                    case 1012:
                                        //GeneradorTermico.DBRVT = strValor;
                                        break;
                                    case 1013:
                                        //GeneradorTermico.DBPSS = strValor;
                                        break;
                                    case 1587:
                                        //GeneradorTermico.TipCald = strValor;
                                        wsGen.SetValue(97, column_gen, strValor);
                                        break;
                                    case 1588:
                                        //GeneradorTermico.ProdVapCald = strValor;
                                        wsGen.SetValue(98, column_gen, strValor);
                                        break;
                                    case 1589:
                                        //GeneradorTermico.PresVapCald = strValor;
                                        wsGen.SetValue(99, column_gen, strValor);
                                        break;
                                    case 1590:
                                        //GeneradorTermico.TempVapCald = strValor;
                                        wsGen.SetValue(100, column_gen, strValor);
                                        break;
                                    case 1591:
                                        //GeneradorTermico.CombTC = strValor;
                                        wsGen.SetValue(101, column_gen, strValor);
                                        break;
                                    case 1592:
                                        //GeneradorTermico.TipQuem = strValor;
                                        wsGen.SetValue(102, column_gen, strValor);
                                        break;
                                    case 1593:
                                        //GeneradorTermico.NumQuem = strValor;
                                        wsGen.SetValue(103, column_gen, strValor);
                                        break;
                                    case 1594:
                                        //GeneradorTermico.RendTerm = strValor;
                                        wsGen.SetValue(104, column_gen, strValor);
                                        break;
                                    case 1595:
                                        //GeneradorTermico.TSF = strValor;
                                        wsGen.SetValue(106, column_gen, strValor);
                                        break;
                                    case 1596:
                                        //GeneradorTermico.ProgMantos = strValor;
                                        strValor = strValor.Length > 38 ? "SI" : "NO";
                                        wsGen.SetValue(107, column_gen, strValor);
                                        break;
                                    case 1597:
                                        //GeneradorTermico.RegVel = strValor;
                                        wsGen.SetValue(108, column_gen, strValor);
                                        break;
                                    case 1598:
                                        //GeneradorTermico.ModoTrab = strValor;
                                        wsGen.SetValue(109, column_gen, strValor);
                                        break;
                                    case 1599:
                                        //GeneradorTermico.ModoContr = strValor;
                                        wsGen.SetValue(110, column_gen, strValor);
                                        break;
                                    case 1007:
                                        //GeneradorTermico.BandaMuer = strValor;
                                        wsGen.SetValue(111, column_gen, strValor);
                                        break;
                                    case 291:
                                        //GeneradorTermico.EstaAct = strValor;
                                        wsGen.SetValue(112, column_gen, strValor);
                                        break;
                                    case 1600:
                                        //GeneradorTermico.ValSistAisl = strValor;
                                        wsGen.SetValue(113, column_gen, strValor);
                                        break;
                                    case 292:
                                        //GeneradorTermico.RanVarEsta = strValor;
                                        wsGen.SetValue(114, column_gen, strValor);
                                        break;
                                }
                            }
                            var cellGen = wsGen.Cells[2, column_gen, 114, column_gen];
                            foreach (var oCeldaGen in cellGen)
                            {
                                var borde = oCeldaGen.Style.Border;
                                borde.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cellGen.AutoFitColumns();
                            column_gen++;
                        }

                        #endregion
                        #region "MODOS OPERACION"
                        ExcelWorksheet wsModos = xlPackage.Workbook.Worksheets["MODO_OPERACION"];
                        int column_mods = 10;
                        foreach (var oModo in lsModos)
                        {
                            var oEquipo = appEquipamiento.GetByIdEqEquipo(Convert.ToInt32(oModo.IDCENTRAL));

                            var oModoOperacion = appDespacho.GetByIdPrGrupo(Convert.ToInt32(oModo.GRUPOCODI));//Modo de Operación

                            var iGrupoCodi = appDespacho.ObtenerCodigoModoOperacionPadre(oModoOperacion.Grupocodi);//Obtenemos Código de generador o grupo
                            var lsValoresMOGrupo = appDespacho.ListadoValoresModoOperacion(int.Parse(oModo.IDCENTRAL), iGrupoCodi);//Datos de Grupo logico o Generador

                            var iGrupoCodiCentral = appDespacho.ObtenerCodigoModoOperacionPadre(iGrupoCodi);//Obtenemos Código de central
                            var lsValoresMOCentral = appDespacho.ListadoValoresModoOperacion(int.Parse(oModo.IDCENTRAL), iGrupoCodiCentral); //Valores de MO de las central

                            wsModos.SetValue(2, column_mods, oModo.MODONOM.Trim());
                            wsModos.SetValue(3, column_mods, oEquipo.EMPRNOMB);
                            wsModos.SetValue(4, column_mods, oEquipo.Equinomb);
                            wsModos.SetValue(5, column_mods, oModo.IDCENTRAL);
                            wsModos.SetValue(6, column_mods, oModo.GRUPOCODI);
                            var ValoresMO = appDespacho.ListadoValoresModoOperacion(Convert.ToInt32(oModo.EQUICODI), Convert.ToInt32(oModo.GRUPOCODI));
                            ///Datos de CEntral
                            var CMarr = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 80);
                            wsModos.SetValue(67, column_mods, CMarr == null ? "" : CMarr.VALOR);

                            foreach (var Valor in ValoresMO)
                            {
                                string sValor = Valor.VALOR == null ? "" : Valor.VALOR.Trim();
                                switch ((int)Valor.CONCEPCODI)
                                {
                                    case -99:
                                        //Modelo.CodModo = Valor.VALOR;
                                        wsModos.SetValue(6, column_mods, sValor);
                                        break;
                                    case 14:
                                        //Modelo.Pe = Valor.VALOR;
                                        wsModos.SetValue(7, column_mods, sValor);
                                        break;
                                    case 16:
                                        //Modelo.Pmin = Valor.VALOR;
                                        wsModos.SetValue(8, column_mods, sValor);
                                        break;
                                    case 15:
                                        //Modelo.Pmax = Valor.VALOR;
                                        wsModos.SetValue(9, column_mods, sValor);
                                        break;
                                    case 115:
                                        //Modelo.V_tomacarga = Valor.VALOR;
                                        wsModos.SetValue(10, column_mods, sValor);
                                        break;
                                    case 116:
                                        //Modelo.V_TC_frio1 = Valor.VALOR;
                                        wsModos.SetValue(11, column_mods, sValor);
                                        break;
                                    case 118:
                                        //Modelo.V_TC_intca = Valor.VALOR;
                                        wsModos.SetValue(12, column_mods, sValor);
                                        break;
                                    case 119:
                                        //Modelo.V_TC_calie = Valor.VALOR;
                                        wsModos.SetValue(13, column_mods, sValor);
                                        break;
                                    case 120:
                                        //Modelo.V_descarga = Valor.VALOR;
                                        wsModos.SetValue(14, column_mods, sValor);
                                        break;
                                    case 121:
                                        //Modelo.T_sinc = Valor.VALOR;
                                        wsModos.SetValue(15, column_mods, sValor);
                                        break;
                                    case 122:
                                        //Modelo.T_sincronizacionFrio1 = Valor.VALOR;
                                        wsModos.SetValue(16, column_mods, sValor);
                                        break;
                                    case 124:
                                        //Modelo.T_sincronizaciónintermedio = Valor.VALOR;
                                        wsModos.SetValue(17, column_mods, sValor);
                                        break;
                                    case 125:
                                        //Modelo.T_sincronizacióncaliente = Valor.VALOR;
                                        wsModos.SetValue(18, column_mods, sValor);
                                        break;
                                    case 126:
                                        //Modelo.T_PC_Sinc = Valor.VALOR;
                                        wsModos.SetValue(19, column_mods, sValor);
                                        break;
                                    case 127:
                                        //Modelo.T_PC_F1 = Valor.VALOR;
                                        wsModos.SetValue(20, column_mods, sValor);
                                        break;
                                    case 129:
                                        //Modelo.T_CargaIntermedio = Valor.VALOR;
                                        wsModos.SetValue(21, column_mods, sValor);
                                        break;
                                    case 130:
                                        //Modelo.T_PC_Cal = Valor.VALOR;
                                        wsModos.SetValue(22, column_mods, sValor);
                                        break;
                                    case 131:
                                        //Modelo.T_SFSP = Valor.VALOR;
                                        wsModos.SetValue(23, column_mods, sValor);
                                        break;
                                    case 132:
                                        //Modelo.T_PC_pm = Valor.VALOR;
                                        wsModos.SetValue(24, column_mods, sValor);
                                        break;
                                    case 133:
                                        //Modelo.T_ArrNegr = Valor.VALOR;
                                        wsModos.SetValue(25, column_mods, sValor);
                                        break;
                                    case 134:
                                        //Modelo.T_fuera_sinc = Valor.VALOR;
                                        wsModos.SetValue(26, column_mods, sValor);
                                        break;
                                    case 135:
                                        //Modelo.T_sinc_par = Valor.VALOR;
                                        wsModos.SetValue(27, column_mods, sValor);
                                        break;
                                    case 136:
                                        //Modelo.T_min_Arr = Valor.VALOR;
                                        wsModos.SetValue(28, column_mods, sValor);
                                        break;
                                    case 137:
                                        //Modelo.T_min_Arr_eme = Valor.VALOR;
                                        wsModos.SetValue(29, column_mods, sValor);
                                        break;
                                    case 138:
                                        //Modelo.MaxPotMin = Valor.VALOR;
                                        wsModos.SetValue(30, column_mods, sValor);
                                        break;
                                    case 139:
                                        //Modelo.Tmin_op = Valor.VALOR;
                                        wsModos.SetValue(31, column_mods, sValor);
                                        break;
                                    case 140:
                                        //Modelo.Ene_sinc = Valor.VALOR;
                                        wsModos.SetValue(32, column_mods, sValor);
                                        break;
                                    case 141:
                                        //Modelo.Ene_sinc_F1 = Valor.VALOR;
                                        wsModos.SetValue(33, column_mods, sValor);
                                        break;
                                    case 143:
                                        //Modelo.Ene_sinc_int = Valor.VALOR;
                                        wsModos.SetValue(34, column_mods, sValor);
                                        break;
                                    case 144:
                                        //Modelo.Ene_sinc_cal = Valor.VALOR;
                                        wsModos.SetValue(35, column_mods, sValor);
                                        break;
                                    case 145:
                                        //Modelo.Ene_PC_sinc = Valor.VALOR;
                                        wsModos.SetValue(36, column_mods, sValor);
                                        break;
                                    case 146:
                                        //Modelo.TipComb = Valor.VALOR;
                                        wsModos.SetValue(37, column_mods, sValor);
                                        ConceptoDatoDTO DatoComb;
                                        switch (Valor.VALOR.Trim().ToUpperInvariant())
                                        {

                                            case "GAS":
                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 77);
                                                //Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(38, column_mods, DatoComb == null ? "" : DatoComb.VALOR);


                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 72);
                                                //Modelo.LHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(39, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 61);//En caso Gas se usa Total Costo Combustible GAS
                                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(60, column_mods, DatoComb == null ? "" : DatoComb.VALOR);
                                                break;
                                            case "DIESEL":
                                            case "D2":
                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 69);
                                                //Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(39, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 42);
                                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(60, column_mods, DatoComb == null ? "" : DatoComb.VALOR);


                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 44);
                                                //Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(61, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 45);
                                                //Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(62, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 46);
                                                //Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(63, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 47);
                                                //Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;



                                                break;
                                            case "R500":
                                            case "RESIDUAL 500":
                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 71);
                                                //Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(39, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 55);
                                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(60, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 56);
                                                //Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(61, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 57);
                                                //Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(62, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 58);
                                                //Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(63, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 59);
                                                //Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;


                                                break;
                                            case "GAS NATURAL":

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 61);//En caso Gas se usa Total Costo Combustible GAS
                                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(60, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                break;
                                            case "BAGAZO":
                                                break;
                                            case "BIOGAS":

                                                break;
                                            case "R6":
                                            case "RESIDUAL 6":
                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 70);
                                                //Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(39, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 48);
                                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(60, column_mods, DatoComb == null ? "" : DatoComb.VALOR);


                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 49);
                                                //Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(61, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 50);
                                                //Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(62, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 51);
                                                //Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(63, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                //DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 52);
                                                //Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;


                                                break;
                                            case "CARBON":
                                            case "CARBÓN":
                                                DatoComb = lsValoresMOCentral.SingleOrDefault(mo => mo.CONCEPCODI == 73);
                                                //Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(39, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 63);
                                                //Modelo.PrecioCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(60, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 64);
                                                //Modelo.CostoTransCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(61, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 65);
                                                //Modelo.CostoTratMecCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(62, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 66);
                                                //Modelo.CostoTratQuiCComb = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(63, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                //DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 67);
                                                //Modelo.CostoFinCComb = DatoComb == null ? "" : DatoComb.VALOR;



                                                break;
                                            default:
                                                DatoComb = ValoresMO.SingleOrDefault(mo => mo.CONCEPCODI == 97);
                                                //Modelo.HHV = DatoComb == null ? "" : DatoComb.VALOR;
                                                wsModos.SetValue(39, column_mods, DatoComb == null ? "" : DatoComb.VALOR);

                                                break;

                                        }
                                        break;
                                    case 147:
                                        //Modelo.ge = Valor.VALOR;
                                        wsModos.SetValue(40, column_mods, sValor);
                                        break;
                                    case 148:
                                        //Modelo.TempComb = Valor.VALOR;
                                        wsModos.SetValue(41, column_mods, sValor);
                                        break;
                                    case 11:
                                        //Modelo.a = Valor.VALOR;
                                        wsModos.SetValue(42, column_mods, sValor);
                                        break;
                                    case 12:
                                        //Modelo.b = Valor.VALOR;
                                        wsModos.SetValue(43, column_mods, sValor);
                                        break;
                                    case 13:
                                        //Modelo.c = Valor.VALOR;
                                        wsModos.SetValue(44, column_mods, sValor);
                                        break;
                                    case 78:
                                        //Modelo.CombATM = Valor.VALOR;
                                        wsModos.SetValue(45, column_mods, sValor);
                                        break;
                                    case 149:
                                        //Modelo.Comb_arr_sinc = Valor.VALOR;
                                        wsModos.SetValue(46, column_mods, sValor);
                                        break;
                                    case 150:
                                        // Modelo.Comb_arr_sinc_F1 = Valor.VALOR;
                                        wsModos.SetValue(47, column_mods, sValor);
                                        break;
                                    case 152:
                                        //Modelo.Comb_arr_sinc_int = Valor.VALOR;
                                        wsModos.SetValue(48, column_mods, sValor);
                                        break;
                                    case 153:
                                        //Modelo.Comb_arr_sinc_cal = Valor.VALOR;
                                        wsModos.SetValue(49, column_mods, sValor);
                                        break;
                                    case 154:
                                        //Modelo.Comb_sinc_PC = Valor.VALOR;
                                        wsModos.SetValue(50, column_mods, sValor);
                                        break;
                                    case 155:
                                        //Modelo.Comb_sinc_PC_F1 = Valor.VALOR;
                                        wsModos.SetValue(51, column_mods, sValor);
                                        break;

                                    case 157:
                                        //Modelo.Comb_sinc_PC_int = Valor.VALOR;
                                        wsModos.SetValue(52, column_mods, sValor);
                                        break;
                                    case 158:
                                        //Modelo.Comb_sinc_PC_cal = Valor.VALOR;
                                        wsModos.SetValue(53, column_mods, sValor);
                                        break;
                                    case 79:
                                        //Modelo.CombPRD = Valor.VALOR;
                                        wsModos.SetValue(54, column_mods, sValor);
                                        break;
                                    case 159:
                                        //Modelo.Comb_PC_sinc = Valor.VALOR;
                                        wsModos.SetValue(55, column_mods, sValor);
                                        break;
                                    case 160:
                                        //Modelo.Comb_sinc_par = Valor.VALOR;
                                        wsModos.SetValue(56, column_mods, sValor);
                                        break;
                                    case 17:
                                        //Modelo.EficTerm = Valor.VALOR;
                                        wsModos.SetValue(57, column_mods, sValor);
                                        break;
                                    case 74:
                                        //Modelo.EficBTUKWh = Valor.VALOR;
                                        wsModos.SetValue(58, column_mods, sValor);
                                        break;
                                    case 27:
                                        //Modelo.CVC = Valor.VALOR;
                                        break;
                                    case 62:
                                        //Modelo.CVNC = Valor.VALOR;
                                        wsModos.SetValue(64, column_mods, sValor);
                                        break;
                                    case 161:
                                        //Modelo.CVONC = Valor.VALOR;
                                        wsModos.SetValue(65, column_mods, sValor);
                                        break;
                                    case 162:
                                        //Modelo.CVM = Valor.VALOR;
                                        wsModos.SetValue(66, column_mods, sValor);
                                        break;
                                    case 80:
                                        //Modelo.CMarr = Valor.VALOR;
                                        wsModos.SetValue(67, column_mods, sValor);
                                        break;
                                    case 163:
                                        //Modelo.SSAA = Valor.VALOR;
                                        wsModos.SetValue(68, column_mods, sValor);
                                        break;
                                    case 164:
                                        //Modelo.FDP = Valor.VALOR;
                                        wsModos.SetValue(69, column_mods, sValor);
                                        break;
                                }
                            }

                            var cellGen = wsModos.Cells[2, column_mods, 69, column_mods];
                            foreach (var oCeldaGen in cellGen)
                            {
                                var borde = oCeldaGen.Style.Border;
                                borde.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(141, 180, 226));
                            }
                            cellGen.AutoFitColumns();
                            column_mods++;
                        }
                        #endregion
                        xlPackage.Save();
                    }
                    sRutaArchivo = ruta + sNombreFile;
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
            return sRutaArchivo;
        }
    }
}
