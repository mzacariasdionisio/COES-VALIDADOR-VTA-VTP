
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.StockCombustibles.Helper;
using COES.MVC.Extranet.Areas.StockCombustibles.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ConsumoCombustible;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.StockCombustibles.Controllers
{
    public class EnvioController : BaseController
    {
        //
        // GET: /StockCombustibles/Envio/
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        StockCombustiblesAppServicio servicio = new StockCombustiblesAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();
        IEODAppServicio servIeod = new IEODAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesCombustible.SesionNombreArchivo] != null) ?
                    Session[ConstantesCombustible.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesCombustible.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[ConstantesCombustible.SesionMatrizExcel] != null) ?
                    (string[][])Session[ConstantesCombustible.SesionMatrizExcel] : new string[1][];
            }
            set { Session[ConstantesCombustible.SesionMatrizExcel] = value; }
        }

        public EnvioController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region METODOS ENVIO DE CONSUMO COMBUSTIBLE
        /// <summary>
        /// Index para envio de Consumo de Combustibles de las centrales hidroeléctricas
        /// </summary>
        /// <returns></returns>
        public ActionResult Consumo()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            try
            {

                bool accesoEmpresa = seguridad.ValidarPermisoOpcion(this.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
                model.OpAccesoEmpresa = accesoEmpresa;
                var empresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);
                if (accesoEmpresa)
                {
                    model.ListaEmpresas = empresas;
                }
                else
                {
                    var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                        Select(x => new SiEmpresaDTO()
                        {
                            Emprcodi = x.EMPRCODI,
                            Emprnomb = x.EMPRNOMB
                        });
                    if (emprUsuario.Count() > 0)
                    {
                        model.ListaEmpresas = emprUsuario.ToList();
                    }
                    else
                    {
                        model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                    }
                }

                model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View(model);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Consumo y Stock
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarHojaExcelWebConsumo(int idEmpresa, int idEnvio, string fecha)
        {
            base.ValidarSesionUsuario();
            List<MeFormatoDTO> entitys = servFormato.GetByModuloLecturaMeFormatos((int)base.IdModulo, ConstantesStockCombustibles.LectCodiConsumo, idEmpresa);
            if (entitys.Count > 0)
            {
                StockCombustiblesModel jsModel = new StockCombustiblesModel();
                jsModel.IsExcelWeb = true;
                BuildHojaExcelConsumo(jsModel, idEmpresa, idEnvio, fecha);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }

        }

        public StockCombustiblesModel BuildHojaExcelConsumo(StockCombustiblesModel model, int idEmpresa, int idEnvio, string fecha)
        {
            List<MeMedicionxintervaloDTO> listaMedicion = new List<MeMedicionxintervaloDTO>();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            DateTime fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa, fechaIni);
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
            model.ListaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoConsumo, model.Formato.FechaProceso);
            if (model.ListaEnvios.Count > 0)
            {
                model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                if (idEnvio == -1)
                {
                    idEnvio = model.IdEnvioLast;
                }
            }

            int idCfgFormato = 0;
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = servFormato.ValidarPlazo(model.Formato);
                model.TipoPlazo = servFormato.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo;//!ValidarFecha(model.Formato, idEmpresa);
                model.Formato.ListaHoja[0].ListaPtos = model.Formato.ListaHoja[0].ListaPtos.Where(x => x.Hojaptoactivo == 1).ToList();
                model.Formato.ListaHoja[1].ListaPtos = model.Formato.ListaHoja[1].ListaPtos.Where(x => x.Hojaptoactivo == 1).ToList();
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;
                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.EnvioActual = envioAnt;
                    idCfgFormato = envioAnt.Cfgenvcodi.GetValueOrDefault(0);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);

                    foreach (var hoja in model.Formato.ListaHoja)
                    {
                        hoja.ListaPtos = this.servFormato.GetListaPtosByHoja(hoja.ListaPtos, idCfgFormato, hoja.Hojacodi);
                        hoja.ListaPtos = hoja.ListaPtos.OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
                    }
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                    model.TipoPlazo = envioAnt.Envioplazo;
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                listaCambios = servFormato.GetAllCambioEnvio(ConstantesStockCombustibles.IdFormatoConsumo, model.Formato.FechaProceso.AddDays(-1), model.Formato.FechaProceso, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
            }
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Dia = model.Formato.FechaProceso.Day.ToString();

            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }
            model.IdEmpresa = idEmpresa;
            listaMedicion = servFormato.GetEnvioMedicionXIntervalo(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa, fechaIni.AddDays(-1), fechaIni);
            this.ActualizarValoresDiaAnterior(model.Formato.FechaProceso, model.EnvioActual, idEmpresa, listaMedicion);

            // Almacenamos los valores para anteriores para usar las observaciones del dia anterior

            Session["VALORES_ANTERIORES"] = listaMedicion;


            // Si solo se tiene datos de Stock entonces no se muestra informacion el el formato
            if (listaMedicion.Where(x => x.Lectcodi == ConstantesStockCombustibles.LectCodiConsumo).Count() == 0)
            {
                listaMedicion = new List<MeMedicionxintervaloDTO>();
            }
            model.ListaHojaPto = model.Formato.ListaHoja[0].ListaPtos;
            int ncol = ConstantesStockCombustibles.ColLen; // numero de columnas para la matriz de datos
            int nfil = model.ListaHojaPto.Count; // número de filas del listado consumo de combustibles
            model.Handson.Width = nfil;
            // Lista de envios
            model.ListaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoConsumo, model.Formato.FechaProceso);
            ///// Datos Stock ///////////////////////////////////////////////////////////////
            var listaPtosStock = model.Formato.ListaHoja[1].ListaPtos;
            int nFilStock = listaPtosStock.GroupBy(x => x.Ptomedicodi).Select(y => y.First()).Count() / 2;
            /////////////////////////////////////////////////////////////////////////////////
            model.IdEnvio = idEnvio;
            model.ListaCambios = new List<CeldaCambios>();

            //definimos el tamaño de la matriz de datos
            model.Handson.ListaExcelData = Tools.ObtenerMatrizListaExcelData(nfil + nFilStock, ncol, (nFilStock == 0) ? ConstantesCombustible.NroFilHeadConsumo : 2 * ConstantesCombustible.NroFilHeadConsumo);
            model.Handson.ListaMerge = new List<CeldaMerge>();
            CargaConsumoCombustiblesEnMatriz(model, listaMedicion, listaCambios, fechaIni, out List<string> listaMensajaValidacion);
            model.ListaMensajeValidacion = listaMensajaValidacion;
            //model.Handson.ListaDropDown = Tools.ObtenerListaDropDown();

            var validaciones = servFormato.GetByCriteriaMeValidacions(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa).Count;
            if (validaciones == 0)
                model.EnabledStockInicio = true;
            else
                model.EnabledStockInicio = false;

            return model;
        }

        /// <summary>
        /// Permite habilitar el stock inicial
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HabilitarStockInicio(int idEmpresa)
        {
            try
            {
                servicio.BorrarValidacionEmpresa(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }

        }

        private void ActualizarValoresDiaAnterior(DateTime fechaProceso, MeEnvioDTO regEnvioActual, int emprcodi, List<MeMedicionxintervaloDTO> listaMxInt)
        {
            if (regEnvioActual != null)
            {
                var listaEnvios = servFormato.GetByCriteriaMeEnvios(emprcodi, ConstantesStockCombustibles.IdFormatoConsumo, fechaProceso.AddDays(-1)).OrderByDescending(x => x.Enviofecha).ToList();
                var regEnvioDiaAnterior = listaEnvios.Find(x => x.Enviofecha < regEnvioActual.Enviofecha);

                if (regEnvioDiaAnterior != null)
                {
                    var listaCambios = servFormato.GetAllCambioEnvio(ConstantesStockCombustibles.IdFormatoConsumo, fechaProceso.AddDays(-1), fechaProceso.AddDays(-1)
                                                  , regEnvioDiaAnterior.Enviocodi, emprcodi).Where(x => x.Enviocodi == regEnvioDiaAnterior.Enviocodi).ToList();

                    if (listaCambios.Count > 0)
                    {
                        foreach (var reg in listaCambios)
                        {
                            var regMxIntCambio = listaMxInt.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == reg.Cambenvfecha);
                            if (regMxIntCambio != null)
                            {
                                decimal valorCambio = 0;
                                if (decimal.TryParse(Tools.StrNumeroFormato(reg.Cambenvdatos), out valorCambio))
                                    regMxIntCambio.Medinth1 = valorCambio;

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Carga Informacion de Consumo de Combustible en el model para visualizacion de la pagina web
        /// </summary>
        /// <param name="ListaHojaPto"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        /// <param name="cordX"></param>
        /// <param name="codrY"></param>
        /// <returns></returns>
        public void CargaConsumoCombustiblesEnMatriz(StockCombustiblesModel model,
           List<MeMedicionxintervaloDTO> listaMedicion, List<MeCambioenvioDTO> listaCambios, DateTime fechaIni, out List<string> listaMensajaValidacion)
        {
            //
            listaMensajaValidacion = new List<string>();

            int cordX = 0;
            int cordY = 0;
            int fil = 0;
            var listaHojaPtoConsumo = model.Formato.ListaHoja[0].ListaPtos;
            var listaPtosStock = model.Formato.ListaHoja[1].ListaPtos;
            var listaMerge = model.Handson.ListaMerge;
            var listaMedicionConsumo = listaMedicion.Where(x => x.Medintfechaini == fechaIni && x.Lectcodi == ConstantesStockCombustibles.LectCodiConsumo).ToList();
            //encabezado para consumo de combustible
            model.Handson.ListaExcelData[cordX][cordY] = "CONSUMO DE COMBUSTIBLE";
            var elemento = new CeldaMerge() { col = cordX, row = cordY, colspan = 6, rowspan = 1 };
            listaMerge.Add(elemento);
            model.Handson.ListaExcelData[cordX + 1][cordY] = "CENTRAL";
            model.Handson.ListaExcelData[cordX + 1][cordY + 1] = "TIPO";
            model.Handson.ListaExcelData[cordX + 1][cordY + 2] = "UNIDAD";
            model.Handson.ListaExcelData[cordX + 1][cordY + 3] = "MEDIDOR";
            model.Handson.ListaExcelData[cordX + 1][cordY + 4] = "CONSUMO";
            model.Handson.ListaExcelData[cordX + 1][cordY + 5] = "TOTAL";
            model.Handson.ListaExcelData[cordX + 1][cordY + 6] = "";
            model.Handson.ListaExcelData[cordX + 1][cordY + 7] = "";
            model.Handson.ListaExcelData[cordX + 1][cordY + 8] = "";
            //Agrupar centrales en Handson
            var listaGrupoCentral = listaHojaPtoConsumo.GroupBy(x => x.Equipadre).Select(
                grp => new
                {
                    Central = grp.Key,
                    Total = grp.Count()
                }
                ).OrderBy(x => x.Central);

            fil = cordX + 2;
            foreach (var reg in listaGrupoCentral)
            {
                elemento = new CeldaMerge()
                {
                    col = cordY,
                    row = cordX + fil,
                    colspan = 1,
                    rowspan = reg.Total
                };
                listaMerge.Add(elemento);
                fil += reg.Total;
            }
            //Agrupar Combustibles
            var listaGrupoCombustible = listaHojaPtoConsumo.GroupBy(x => new { x.Equipadre, x.Tptomedicodi }).Select(
                grp => new
                {
                    Central = grp.Key.Equipadre,
                    Tipoptomed = grp.Key.Tptomedicodi,
                    Total = grp.Count()
                }
                ).OrderBy(x => x.Central).ThenByDescending(x => x.Tipoptomed);
            fil = cordX + 2;
            int fila = 0;
            if (model.IsExcelWeb)
            {
                fila = cordX + 2;
            }
            else
            {
                fila = ConstantesCombustible.FilaExcelData + 1;
            }

            foreach (var reg in listaGrupoCombustible)
            {
                elemento = new CeldaMerge()
                {
                    col = cordY + 1,
                    row = cordX + fil,
                    colspan = 1,
                    rowspan = reg.Total
                };
                listaMerge.Add(elemento);
                elemento = new CeldaMerge()
                {
                    col = cordY + 2,
                    row = cordX + fil,
                    colspan = 1,
                    rowspan = reg.Total
                };
                listaMerge.Add(elemento);
                elemento = new CeldaMerge()
                {
                    col = cordY + 5,
                    row = cordX + fil,
                    colspan = 1,
                    rowspan = reg.Total
                };
                listaMerge.Add(elemento);
                //Columna Final

                string formula = "=Sum(E" + (fila + 1).ToString() + ")";

                for (int j = 1; j < reg.Total; j++)
                {
                    formula += " + Sum(E" + (fila + j + 1).ToString() + ")";
                }
                model.Handson.ListaExcelData[cordX + fil][cordY + 5] = formula;
                fil += reg.Total;
                fila += reg.Total;
            }
            elemento = new CeldaMerge()
            {
                col = cordY + 6,
                row = cordX,
                colspan = 3,
                rowspan = listaHojaPtoConsumo.Count + 2
            };
            listaMerge.Add(elemento);

            elemento = new CeldaMerge()
            {
                col = cordY,
                row = cordX + listaHojaPtoConsumo.Count + 2,
                colspan = 8,
                rowspan = 1
            };
            listaMerge.Add(elemento);
            List<MeHojaptomedDTO> listaEquipos = listaHojaPtoConsumo.GroupBy(x => new { x.Ptomedicodi })
                                .Select(y => y.First()).OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();


            int i = ConstantesCombustible.NroFilHeadConsumo;

            foreach (var reg in listaEquipos)
            {
                if (reg.Equipadre > 0)
                {
                    model.Handson.ListaExcelData[cordX + i][cordY] = reg.Equipopadre.Trim();
                }
                else
                {
                    model.Handson.ListaExcelData[cordX + i][cordY] = reg.Equinomb.Trim();

                }
                model.Handson.ListaExcelData[cordX + i][cordY + 1] = reg.Tipoptomedinomb.Trim().Substring(23, reg.Tipoptomedinomb.Trim().Length - 23);
                model.Handson.ListaExcelData[cordX + i][cordY + 2] = reg.Tipoinfoabrev.Trim();
                if (reg.Ptomedicodi == ConstantesCombustible.PtoMedUti5)
                {
                    model.Handson.ListaExcelData[cordX + i][cordY + 3] = reg.Ptomedidesc.Trim();
                }
                else
                {
                    model.Handson.ListaExcelData[cordX + i][cordY + 3] = reg.Equiabrev.Trim();
                }

                var entitys = listaMedicionConsumo.Where(x => x.Ptomedicodi == reg.Ptomedicodi && reg.Tipoinfocodi == x.Tipoinfocodi).ToList();

                //====Imprime valores del dia seleccionado
                if (entitys.Count > 0)
                {
                    var find = entitys.Find(x => x.Medestcodi == 1);
                    if (find != null)
                    {
                        var cambio = listaCambios.Find(y => y.Ptomedicodi == find.Ptomedicodi && y.Cambenvfecha == find.Medintfechaini);

                        if (cambio != null)
                        {
                            model.Handson.ListaExcelData[cordX + i][cordY + 4] = Tools.StrNumeroFormatoWithLength(cambio.Cambenvdatos, 4);
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = cordX + i,
                                Col = cordY + 4
                            });
                        }
                        else
                        {
                            model.Handson.ListaExcelData[cordX + i][cordY + 4] = ((decimal)find.Medinth1).ToString();
                        }
                    }

                }
                i++;

            }
            i++;
            var listaMedicionStock = listaMedicion.Where(x => x.Lectcodi == ConstantesStockCombustibles.LectCodiStock).ToList();
            if (listaPtosStock.Count > 0)
                CargaStockCombustiblesEnMatriz(model, listaMedicionStock, listaMedicionConsumo, listaCambios, cordX + i, cordY, listaEquipos, fechaIni, out listaMensajaValidacion);
        }

        /// <summary>
        /// Carga Informacion de Stock de Combustible en el model para visualizacion de la pagina web
        /// </summary>
        /// <param name="ListaHojaPto"></param>
        /// <param name="nfil"></param>
        /// <param name="ncol"></param>
        /// <param name="cordX"></param>
        /// <param name="codrY"></param>
        /// <returns></returns>
        public void CargaStockCombustiblesEnMatriz(StockCombustiblesModel model, List<MeMedicionxintervaloDTO> listaMedicion, List<MeMedicionxintervaloDTO> listaConsumo, List<MeCambioenvioDTO> listaCambios, int xCoor, int yCoor
                                                    , List<MeHojaptomedDTO> listaPtoConsumo, DateTime fecha, out List<string> listaMensajaValidacion)
        {
            //
            listaMensajaValidacion = new List<string>();

            int cordX = xCoor;
            int cordY = yCoor;
            int i = 1;
            var lista = model.Handson.ListaExcelData;
            var listaPto = model.Formato.ListaHoja[1].ListaPtos;
            var listaMerge = model.Handson.ListaMerge;
            List<int> codRecepcion = new List<int>();
            List<int> codStock = new List<int>();
            codRecepcion = ConstantesStockCombustibles.StrTptoRecepcion.Split(',').Select(Int32.Parse).ToList();
            codStock = ConstantesStockCombustibles.StrTptoStock.Split(',').Select(Int32.Parse).ToList();
            //encabezado para listado de stock de combustible
            lista[cordX][cordY] = "STOCK DE COMBUSTIBLES";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColCentral] = "CENTRAL";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColTipo] = "TIPO";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColUnidad] = "UNIDAD";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColInicial] = "INICIAL";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColRecepcion] = "RECEPCIÓN";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColConsumo] = "CONSUMO TOTAL";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColFinal] = "FINAL";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColDeclarado] = "FINAL DECLARADO";
            lista[cordX + i][cordY + ConstantesStockCombustibles.ColObservacion] = "OBSERVACIÓN";
            var elemento = new CeldaMerge() { col = cordY, row = cordX, colspan = ConstantesStockCombustibles.ColLen - 1, rowspan = 1 };
            listaMerge.Add(elemento);
            //Agrupar centrales en Handson
            var listaGrupoCentral = listaPto.Where(z => ConstantesStockCombustibles.IdsTptoStock.Contains(z.Tptomedicodi))
                .GroupBy(x => new { x.Equicodi }).Select(
                grp => new
                {
                    Central = grp.Key,
                    Total = grp.Count()
                }
                );
            var fil = cordX + 2;
            int filaExcel = 0;
            if (!model.IsExcelWeb)
            {
                filaExcel = ConstantesCombustible.FilaExcelData - 1;
            }
            foreach (var reg in listaGrupoCentral)
            {
                elemento = new CeldaMerge()
                {
                    col = cordY,
                    row = fil,
                    colspan = 1,
                    rowspan = reg.Total
                };
                listaMerge.Add(elemento);
                fil += reg.Total;
            }

            i++;
            List<MeHojaptomedDTO> listaEquipos = listaPto.Where(z => ConstantesStockCombustibles.IdsTptoStock.Contains(z.Tptomedicodi))
                .GroupBy(x => new { x.Equicodi, x.Tptomedicodi })
                                .Select(y => y.First()).OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
            List<MeMedicionxintervaloDTO> listaStockIni = listaMedicion.Where(x => x.Medintfechaini == fecha.AddDays(-1)).ToList();
            List<MeMedicionxintervaloDTO> listaActual = listaMedicion.Where(x => x.Medintfechaini == fecha).ToList();
            foreach (var reg in listaEquipos)
            {
                string tipoComb = reg.Tipoptomedinomb.Substring(19, reg.Tipoptomedinomb.Length - 19);
                lista[cordX + i][cordY] = reg.Equinomb;
                lista[cordX + i][cordY + ConstantesStockCombustibles.ColTipo] = tipoComb;
                lista[cordX + i][cordY + ConstantesStockCombustibles.ColUnidad] = reg.Tipoinfoabrev;
                //var findStockIni = listaStockIni.Find(x => x.Equicodi == reg.Equicodi && ConstantesStockCombustibles.IdsTptoStock.Contains(x.Tipoptomedicodi) && x.Medintfechaini == fecha.AddDays(-1));
                var findStockIni = listaStockIni.Find(x => x.Equicodi == reg.Equicodi && x.Tptomedicodi == reg.Tptomedicodi && x.Medintfechaini == fecha.AddDays(-1));
                if (findStockIni != null)
                {
                    if (findStockIni.Medinth1 != null)
                        lista[cordX + i][cordY + ConstantesStockCombustibles.ColInicial] = ((decimal)findStockIni.Medinth1).ToString("0.00");
                }
                //Stock Fin
                //var findStockFin = listaMedicion.Find(x => x.Equicodi == reg.Equicodi && ConstantesStockCombustibles.IdsTptoStock.Contains(x.Tipoptomedicodi) && x.Medintfechaini == fecha);
                var findStockFin = listaMedicion.Find(x => x.Equicodi == reg.Equicodi && x.Tptomedicodi == reg.Tptomedicodi && x.Medintfechaini == fecha);
                if (findStockFin != null)
                {
                    var cambio = listaCambios.Find(y => y.Ptomedicodi == findStockFin.Ptomedicodi && y.Cambenvfecha == findStockFin.Medintfechaini);
                    if (cambio != null)
                    {
                        var lstValor = cambio.Cambenvdatos.Split('#');
                        int totCampos = lstValor.Count();
                        switch (totCampos)
                        {
                            case 1:
                            case 2:
                                lista[cordX + i][cordY + ConstantesStockCombustibles.ColDeclarado] = Tools.StrNumeroFormatoWithLength(lstValor[0], 4);
                                if (lstValor[0] != "")
                                {
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = cordX + i,
                                        Col = cordY + ConstantesStockCombustibles.ColDeclarado
                                    });
                                }
                                if (totCampos == 2)
                                {
                                    lista[cordX + i][cordY + ConstantesStockCombustibles.ColObservacion] = lstValor[1];
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = cordX + i,
                                        Col = cordY + ConstantesStockCombustibles.ColObservacion
                                    });
                                }
                                break;
                            default:
                                lista[cordX + i][cordY + ConstantesStockCombustibles.ColDeclarado] = Tools.StrNumeroFormatoWithLength(lstValor[0], 4);
                                if (lstValor[0] != "")
                                {
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = cordX + i,
                                        Col = cordY + ConstantesStockCombustibles.ColDeclarado
                                    });
                                }
                                for (var z = 1; z < totCampos; z++)
                                {
                                    lista[cordX + i][cordY + ConstantesStockCombustibles.ColObservacion] += lstValor[z];
                                }
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = cordX + i,
                                    Col = cordY + ConstantesStockCombustibles.ColObservacion
                                });

                                break;
                        }
                    }
                    else
                    {
                        if (findStockFin.Medinth1 != null)
                            lista[cordX + i][cordY + ConstantesStockCombustibles.ColDeclarado] = ((decimal)findStockFin.Medinth1).ToString("0.0000");
                        lista[cordX + i][cordY + ConstantesStockCombustibles.ColObservacion] = findStockFin.Medintdescrip;
                    }


                }
                int filaTotal = 0;
                lista[cordX + i][cordY + ConstantesStockCombustibles.ColConsumo] = ObtieneFormulaConsumo(reg.Equicodi, reg.Tptomedicodi, listaPtoConsumo, listaConsumo, fecha, ref filaTotal, model.IsExcelWeb, out decimal valorConsumo);
                lista[cordX + i][cordY + ConstantesStockCombustibles.ColFinal] = "= Sum(" + Tools.GetExcelColumnName(ConstantesStockCombustibles.ColInicial + 1) +
                    (cordX + filaExcel + i + 1).ToString() + ") + Sum(" + Tools.GetExcelColumnName(ConstantesStockCombustibles.ColRecepcion + 1) +
                    (cordX + filaExcel + i + 1).ToString() + ") - Sum(" + Tools.GetExcelColumnName(ConstantesStockCombustibles.ColConsumo + 1) +
                    (cordX + filaExcel + i + 1).ToString() + ")";
                //if(filaTotal > 0)
                //    lista[filaTotal - 1][cordY + 5] = lista[cordX + i][cordY + 4];
                int index = codStock.FindIndex(x => x == reg.Tptomedicodi);
                //var findRepecion = listaActual.Find(x => x.Equicodi == reg.Equicodi && ConstantesStockCombustibles.IdsTptoRecepcion.Contains(x.Tipoptomedicodi));
                var findRepecion = listaActual.Find(x => x.Equicodi == reg.Equicodi && codRecepcion[index] == x.Tptomedicodi);
                if (findRepecion != null)
                {
                    var cambio = listaCambios.Find(y => y.Ptomedicodi == findRepecion.Ptomedicodi && y.Cambenvfecha == findRepecion.Medintfechaini);
                    if (cambio != null)
                    {
                        lista[cordX + i][cordY + ConstantesStockCombustibles.ColRecepcion] = Tools.StrNumeroFormatoWithLength(cambio.Cambenvdatos, 4);
                        model.ListaCambios.Add(new CeldaCambios()
                        {
                            Row = cordX + i,
                            Col = cordY + ConstantesStockCombustibles.ColRecepcion
                        });
                    }
                    else
                    {
                        lista[cordX + i][cordY + ConstantesStockCombustibles.ColRecepcion] = ((decimal)findRepecion.Medinth1).ToString("0.0000");
                    }
                }

                decimal valorInicial = findStockIni != null ? findStockIni.Medinth1.GetValueOrDefault(0) : 0;
                decimal valorRecepcion = findRepecion != null ? findRepecion.Medinth1.GetValueOrDefault(0) : 0;
                decimal valorFinal = valorInicial + valorRecepcion - valorConsumo;

                if (valorFinal < 0)
                    listaMensajaValidacion.Add(string.Format("{0} {1} tiene STOCK DE COMBUSTIBLE FINAL negativo para el día {2}. Se solicita corregir la carga de datos de aquel día."
                                                            , reg.Equinomb, tipoComb, fecha.ToString(ConstantesAppServicio.FormatoFecha)));

                i++;
            }
        }

        /// <summary>
        /// Graba los datos enviados del formato consumo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWebConsumo(string[][] data, int idEmpresa, string fecha)
        {
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            DateTime fechaPeriodo = DateTime.ParseExact(fecha,
                        Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int idEnvio = 0;

            try
            {
                base.ValidarSesionJsonResult();

                var formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa, fechaPeriodo);
                formato.ListaHoja[0].ListaPtos = formato.ListaHoja[0].ListaPtos.Where(x => x.Hojaptoactivo == 1).ToList();
                formato.ListaHoja[1].ListaPtos = formato.ListaHoja[1].ListaPtos.Where(x => x.Hojaptoactivo == 1).ToList();

                List<MeMedicionxintervaloDTO> entitys = this.ObtenerDatosConsumo(data, fechaPeriodo, formato);
                formato.FechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                formato.Emprcodi = idEmpresa;
                string empresa = string.Empty;
                var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa);
                //Validación de vigencia de empresa
                if (!this.servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now))
                {
                    return Json(model);
                }
                if (regEmp != null)
                    empresa = regEmp.Emprnomb;

                string tipoPlazo = servFormato.EnvioValidarPlazo(formato, idEmpresa);
                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

                /////////////// Grabar Config Formato Envio //////////////////
                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = ConstantesStockCombustibles.IdFormatoConsumo;
                config.Emprcodi = idEmpresa;
                config.FechaInicio = formato.FechaFin;
                int idConfig = servFormato.GrabarConfigFormatEnvio(config);
                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                Boolean enPlazo = servFormato.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);
                DateTime fechaEnvio = DateTime.Now;
                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Enviofecha = fechaEnvio;
                envio.Enviofechaperiodo = fechaPeriodo;
                envio.Enviofechaini = formato.FechaInicio;
                envio.Enviofechafin = formato.FechaFin;
                envio.Envioplazo = tipoPlazo;//(enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = ConstantesStockCombustibles.IdFormatoConsumo;
                idEnvio = servFormato.SaveMeEnvio(envio);
                model.IdEnvio = idEnvio;

                servicio.GrabarValoresCargadosMedxIntervalo(entitys, User.Identity.Name, idEnvio, idEmpresa, formato);
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = idConfig;
                servFormato.UpdateMeEnvio(envio);
                model.Resultado = 1;
                EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
                 (DateTime)envio.Enviofecha, ConstantesStockCombustibles.IdFormatoConsumo);

                EnviarCorreoRecepcion(entitys, idEmpresa, empresa, formato.FechaProceso, idEnvio);

                this.ValidarDatosDiaSiguiente(fechaPeriodo, idEmpresa, idEnvio, out List<string> listaMensajeDiaSig);
                model.Mensaje = string.Join("\n", listaMensajeDiaSig);

                EnviarCorreoValidacionTransgresion(idEmpresa, formato.FechaProceso, idEnvio);
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return Json(model);
        }

        /// <summary>
        /// Obtiene MAtriz de datos del formato web
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<MeMedicionxintervaloDTO> ObtenerDatosConsumo(string[][] datos, DateTime fecha, MeFormatoDTO formato)
        //int idLectConsumo, int idLectStock, List<MeHojaptomedDTO> listaPtos,List<MeHojaptomedDTO> listaPtosStock)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO entity;
            var listaPtos = formato.ListaHoja[0].ListaPtos.OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
            int idLectConsumo = (int)formato.ListaHoja[0].Lectcodi;
            var listaPtosStock = formato.ListaHoja[1].ListaPtos.OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ThenBy(x => x.Equinomb).ToList();
            int idLectStock = (int)formato.ListaHoja[1].Lectcodi;
            int lenStock = formato.ListaHoja[1].ListaPtos.Count;//listaPtosStock.Count;
            int lenCons = formato.ListaHoja[0].ListaPtos.Count; //listaPtos.Count;
            if (datos.Length > 1)
            {
                ///Lee Consumo de Combustible
                for (int i = 3; i < lenCons + 4; i++)
                {
                    if (!string.IsNullOrEmpty(datos[i - 1][4]))
                    {
                        entity = new MeMedicionxintervaloDTO();
                        entity.Medintfechaini = fecha; //DateTime.ParseExact(datos[i][0] + " " + datos[i][2],
                        // Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                        entity.Ptomedicodi = listaPtos[i - 3].Ptomedicodi;
                        entity.Medintusumodificacion = User.Identity.Name;
                        entity.Medintfecmodificacion = DateTime.Now;
                        entity.Tipoinfocodi = listaPtos[i - 3].Tipoinfocodi;
                        entity.Tptomedicodi = listaPtos[i - 3].Tptomedicodi;
                        entity.Emprcodi = listaPtos[i - 3].Emprcodi;
                        entity.Lectcodi = idLectConsumo;
                        entity.Medinth1 = decimal.Parse(datos[i - 1][4]);
                        entity.Medestcodi = 1;
                        //entity.Medintdescrip = datos[i - 1][6];
                        lista.Add(entity);
                    }
                    // Para futura implementacion
                    //if (!string.IsNullOrEmpty(datos[i-1][5]))
                    //{
                    //    entity = new MeMedicionxintervaloDTO();
                    //    entity.Medintfechaini = fecha; //DateTime.ParseExact(datos[i][0] + " " + datos[i][2],
                    //    // Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    //    entity.Ptomedicodi = listaPtos[i - 3].Ptomedicodi;
                    //    entity.Medintusumodificacion = User.Identity.Name;
                    //    entity.Medintfecmodificacion = DateTime.Now;
                    //    entity.Tipoinfocodi = listaPtos[i - 3].Tipoinfocodi;
                    //    entity.Lectcodi = idLectura;
                    //    entity.Medinth1 = decimal.Parse(datos[i-1][5]);
                    //    entity.Medestcodi = 2;
                    //    lista.Add(entity);
                    //}

                }
                /// Lee Stock de Combustible
                List<MeHojaptomedDTO> listaFilaStock = listaPtosStock.Where(z => ConstantesStockCombustibles.IdsTptoStock.Contains(z.Tptomedicodi))
    .GroupBy(x => new { x.Equicodi, x.Tptomedicodi, x.Emprcodi })
                    .Select(y => y.First()).ToList();
                List<MeHojaptomedDTO> listaFilaRecepcion = listaPtosStock.Where(z => ConstantesStockCombustibles.IdsTptoRecepcion.Contains(z.Tptomedicodi))
.GroupBy(x => new { x.Equicodi, x.Tptomedicodi, x.Emprcodi })
    .Select(y => y.First()).ToList();

                int iniIndice = lenCons + 5;
                for (int i = iniIndice; i < datos.Length; i++)
                {
                    entity = new MeMedicionxintervaloDTO();
                    entity.Medintfechaini = fecha.AddDays(-1);
                    entity.Emprcodi = listaFilaStock[i - iniIndice].Emprcodi;
                    entity.Ptomedicodi = listaFilaStock[i - iniIndice].Ptomedicodi; //listaPtosStock[(i - iniIndice) * 2].Ptomedicodi;
                    //entity.Tipoptomedicodi = ConstantesStockCombustibles.IdTipoPtoStock;
                    entity.Medintusumodificacion = User.Identity.Name;
                    entity.Medintfecmodificacion = DateTime.Now;
                    entity.Tipoinfocodi = listaFilaStock[i - iniIndice].Tipoinfocodi; //listaPtosStock[(i - iniIndice) * 2].Tipoinfocodi;
                    entity.Lectcodi = idLectStock;
                    if (!string.IsNullOrEmpty(datos[i][ConstantesStockCombustibles.ColInicial]))
                    {
                        entity.Medinth1 = decimal.Parse(datos[i][ConstantesStockCombustibles.ColInicial]);
                    }

                    /*********Agregamos las Observaciones del dia anterior *********/

                    if (Session["VALORES_ANTERIORES"] != null)
                    {
                        var DatosOld = (List<MeMedicionxintervaloDTO>)Session["VALORES_ANTERIORES"];
                        foreach (MeMedicionxintervaloDTO dat in DatosOld)
                        {
                            if (dat.Medintfechaini == fecha.AddDays(-1))
                            {
                                if (dat.Ptomedicodi == listaFilaStock[i - iniIndice].Ptomedicodi)
                                {
                                    if (dat.Tipoinfocodi == listaFilaStock[i - iniIndice].Tipoinfocodi)
                                    {
                                        entity.Medintdescrip = dat.Medintdescrip;
                                    }
                                }
                            }
                        }

                    }
                    /**************************************************************/

                    lista.Add(entity);
                    // Stock FIn
                    entity = new MeMedicionxintervaloDTO();
                    entity.Medintfechaini = fecha;
                    entity.Emprcodi = listaFilaStock[i - iniIndice].Emprcodi;
                    entity.Ptomedicodi = listaFilaStock[i - iniIndice].Ptomedicodi;//listaPtosStock[(i - iniIndice) * 2].Ptomedicodi;
                    //entity.Tipoptomedicodi = ConstantesStockCombustibles.IdTipoPtoStock;
                    entity.Medintusumodificacion = User.Identity.Name;
                    entity.Medintfecmodificacion = DateTime.Now;
                    entity.Tipoinfocodi = listaFilaStock[i - iniIndice].Tipoinfocodi; //listaPtosStock[(i - iniIndice) * 2].Tipoinfocodi;
                    entity.Tptomedicodi = listaFilaStock[i - iniIndice].Tptomedicodi;
                    entity.Emprcodi = listaFilaStock[i - iniIndice].Emprcodi;
                    entity.Lectcodi = idLectStock;
                    if (!string.IsNullOrEmpty(datos[i][ConstantesStockCombustibles.ColDeclarado]))
                    {
                        entity.Medinth1 = decimal.Parse(datos[i][ConstantesStockCombustibles.ColDeclarado]);
                    }
                    entity.Medintdescrip = datos[i][ConstantesStockCombustibles.ColObservacion];
                    lista.Add(entity);
                    // Recepción
                    entity = new MeMedicionxintervaloDTO();
                    entity.Medintfechaini = fecha;
                    entity.Emprcodi = listaFilaRecepcion[i - iniIndice].Emprcodi;
                    entity.Ptomedicodi = listaFilaRecepcion[i - iniIndice].Ptomedicodi;//listaPtosStock[(i - iniIndice) * 2 + 1].Ptomedicodi;
                    //entity.Tipoptomedicodi = ConstantesStockCombustibles.IdTipoPtoRecepcion;
                    entity.Medintusumodificacion = User.Identity.Name;
                    entity.Medintfecmodificacion = DateTime.Now;
                    entity.Tipoinfocodi = listaFilaRecepcion[i - iniIndice].Tipoinfocodi;//listaPtosStock[(i - iniIndice) * 2 + 1].Tipoinfocodi;
                    entity.Lectcodi = idLectStock;
                    if (!string.IsNullOrEmpty(datos[i][ConstantesStockCombustibles.ColRecepcion]))
                    {
                        entity.Medinth1 = decimal.Parse(datos[i][ConstantesStockCombustibles.ColRecepcion]);
                        entity.Equinomb = datos[i][0];
                        entity.H1Recep = entity.Medinth1;
                        entity.Tipoinfoabrev = datos[i][2];
                        entity.Fenergnomb = datos[i][1];
                    }
                    lista.Add(entity);
                }
            }

            return lista;
        }

        /// <summary>
        /// Obtiene formula suma para los combustible de una central
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="tipoptomedicodi"></param>
        /// <param name="listaPto"></param>
        /// <param name="iniFil"></param>
        /// <param name="isExcelWeb"></param>
        /// <returns></returns>
        private string ObtieneFormulaConsumo(int equicodi, int tipoptomedicodi, List<MeHojaptomedDTO> listaPto, List<MeMedicionxintervaloDTO> listaMxInt, DateTime fecha, ref int iniFil, Boolean isExcelWeb, out decimal valorBD)
        {
            valorBD = 0;

            string formulaCons = string.Empty;
            int i = 0;
            if (isExcelWeb)
            {
                i = 3;
            }
            else
            {
                i = ConstantesCombustible.FilaExcelData + 2;
            }
            bool inicio = true;
            int indice = Array.IndexOf(ConstantesStockCombustibles.IdsTptoStock.ToArray(), tipoptomedicodi);
            foreach (var reg in listaPto)
            {
                if (equicodi == reg.Equipadre && ConstantesStockCombustibles.IdsTptoConsumo[indice] == reg.Tptomedicodi)
                {
                    if (inicio)
                    {
                        formulaCons += "=Sum(E" + i.ToString() + ")";// +" + F" + i.ToString();
                        iniFil = i;
                        inicio = false;
                    }
                    else
                        formulaCons += " + Sum(E" + i.ToString() + ")";// +" + F" + i.ToString();

                    var regDato = listaMxInt.Find(x => x.Ptomedicodi == reg.Ptomedicodi && x.Medintfechaini == fecha);
                    if (regDato != null)
                        valorBD += regDato.Medinth1.GetValueOrDefault(0);
                }
                i++;
            }

            return formulaCons;
        }

        /// <summary>
        /// Genera litsa de objetos Memedicionxintervalo con los valores de los stock iniciales de cada pto de medición
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="listaPtos"></param>
        /// <param name="formato"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<MeMedicionxintervaloDTO> GenerarListaStockIniCambios(string[][] dataExcel, List<MeHojaptomedDTO> listaPtos, MeFormatoDTO formato, string userName)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            for (var i = ConstantesCombustible.FilasHead; i < dataExcel.Length; i++)
            {
                MeMedicionxintervaloDTO entity = new MeMedicionxintervaloDTO();
                entity.Medintfechaini = formato.FechaProceso.AddDays(-1);
                entity.Medintfechafin = formato.FechaProceso.AddDays(-1);
                entity.Medintusumodificacion = userName;
                entity.Medintfecmodificacion = DateTime.Now;
                entity.Lectcodi = formato.Lectcodi;

                for (var k = 0; k <= 1; k++)
                {
                    if (listaPtos[2 * (i - 2) + k].Tptomedicodi == ConstantesStockCombustibles.TipotomedicodiStock) //Stock
                    {
                        entity.Ptomedicodi = listaPtos[2 * (i - 2) + k].Ptomedicodi;
                        entity.Tipoinfocodi = listaPtos[2 * (i - 2) + k].Tipoinfocodi;
                        entity.Tptomedicodi = listaPtos[2 * (i - 2) + k].Tptomedicodi;
                    }

                }
                stValor = dataExcel[i][3]; // Stock Fin

                if (COES.Base.Tools.Util.EsNumero(stValor))
                {
                    valor = decimal.Parse(stValor);
                    entity.Medinth1 = valor;
                }
                else
                {
                    if (formato.Formatcheckblanco == 0)
                        entity.Medinth1 = null;
                    else
                        entity.Medinth1 = 0;
                }

                lista.Add(entity);
            }
            return lista;
        }

        /// <summary>
        /// Validar si la fecha siguiente tiene datos erróneos.
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="listaMensajeDiaSig"></param>
        private void ValidarDatosDiaSiguiente(DateTime fechaProceso, int idEmpresa, int idEnvio, out List<string> listaMensajeDiaSig)
        {
            listaMensajeDiaSig = new List<string>();

            try
            {
                StockCombustiblesModel jsModel = new StockCombustiblesModel();
                jsModel.IsExcelWeb = true;

                BuildHojaExcelConsumo(jsModel, idEmpresa, -1, fechaProceso.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha));
                listaMensajeDiaSig = jsModel.ListaMensajeValidacion;

                SiEmpresaDTO emp = servFormato.GetByIdSiEmpresa(idEmpresa);
                EnviarCorreoValidacionStockCombustible(jsModel.ListaMensajeValidacion, idEmpresa, emp.Emprnomb, fechaProceso, idEnvio);
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormatoConsumo(int idEmpresa, string dia)
        {
            //int indicador = 0;
            int idEnvio = 0;
            string ruta = string.Empty;
            try
            {
                StockCombustiblesModel model = new StockCombustiblesModel();
                model.IsExcelWeb = false;
                BuildHojaExcelConsumo(model, idEmpresa, idEnvio, dia);
                ruta = Tools.GenerarFileExcelConsumo(model);
            }
            catch
            {
                ruta = "-1";
            }
            return ruta;
        }

        #endregion

        #region METODOS ENVIO PRESION GAS

        /// <summary>
        /// Index de presion de gas
        /// </summary>
        /// <returns></returns>
        public ActionResult PresionGas()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            StockCombustiblesModel model = new StockCombustiblesModel();
            try
            {


                bool accesoEmpresa = seguridad.ValidarPermisoOpcion(this.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
                var empresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoPGas);
                if (accesoEmpresa)
                {
                    model.ListaEmpresas = empresas;
                }
                else
                {
                    var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                        Select(x => new SiEmpresaDTO()
                        {
                            Emprcodi = x.EMPRCODI,
                            Emprnomb = x.EMPRNOMB
                        });
                    if (emprUsuario.Count() > 0)
                    {
                        model.ListaEmpresas = emprUsuario.ToList();
                    }
                    else
                    {
                        model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                    }
                }
                model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            return View(model);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Presion de Gas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarPresionGas(int idEmpresa, int idEnvio, string fecha)
        {
            base.ValidarSesionUsuario();
            List<MeFormatoDTO> entitys = servFormato.GetByModuloLecturaMeFormatos((int)base.IdModulo, ConstantesStockCombustibles.LectCodiPresionGas, idEmpresa);
            if (entitys.Count > 0)
            {
                StockCombustiblesModel jsModel = BuildHojaExcelPresion(idEmpresa, idEnvio, fecha);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }

        }

        /// <summary>
        /// Devuelve el model con informacion de Presion de Gas
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public StockCombustiblesModel BuildHojaExcelPresion(int idEmpresa, int idEnvio, string fecha)
        {
            int idEnvioExcel = idEnvio;
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.Handson = new HandsonModel();

            //handson.ListaMerge = Tools.ObtenerListaMergePG();
            //model.ListaCentrales = Tools.ObtenerListaCentrales();
            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoPGas);
            //this.Formato = model.Formato;
            var cabercera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            /// DEFINICION DEL FORMATO //////
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
            model.ListaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoPGas, model.Formato.FechaProceso);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (model.ListaEnvios.Count > 0)
            {
                idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                model.IdEnvioLast = idUltEnvio;
                if (idEnvio == -1)
                {
                    idEnvio = model.IdEnvioLast;
                }

                var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                if (reg != null)
                    model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
            }

            idEnvio = idEnvio <= 0 ? 0 : idEnvio;

            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }

            int idCfgFormato = 0;
            model.IdEnvio = idEnvio;
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = servFormato.ValidarPlazo(model.Formato);
                model.TipoPlazo = servFormato.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo;
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;

                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                listaCambios = servFormato.GetAllCambioEnvio(ConstantesStockCombustibles.IdFormatoPGas, model.Formato.FechaProceso, model.Formato.FechaProceso, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
            }
            model.Anho = model.Formato.FechaInicio.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
            model.Dia = model.Formato.FechaInicio.Day.ToString();

            model.ListaHojaPto = servFormato.GetListaPtos(model.Formato.FechaProceso, idCfgFormato, idEmpresa, ConstantesStockCombustibles.IdFormatoPGas, cabercera.Cabquery);
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(150);
            for (var i = 0; i < model.ListaHojaPto.Count; i++)
            {
                model.Handson.ListaColWidth.Add(100);
            }


            var cabecerasRow = cabercera.Cabcampodef.Split(ConstantesCombustible.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(ConstantesCombustible.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }
            idEnvio = idEnvioExcel != -2 ? idEnvio : idEnvioExcel;
            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                var lista = servFormato.GetDataFormato24(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                model.Handson.ListaExcelData = Tools.ObtenerListaExcelDataPG(model, lista, listaCambios, fecha, idEnvio);
            }
            else
            {
                model.Handson.ListaExcelData = this.MatrizExcel;
            }
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia;

            model.Handson.ListaFilaReadOnly = Tools.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques);
            //model.Handson.ListaFilaReadOnly = CargarListaFilaReadOnly(model.Formato.FechaProceso, idEmpresa, ConstantesStockCombustibles.IdFormatoPGas, model.Formato.Formatrows, nBloques, model.EnPlazo);


            return model;
        }

        /// <summary>
        /// Carga lista de Filas que indican si esta bloqueadas o no, valido para formatos en tiempo real.
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatcodi"></param>
        /// <param name="filHead"></param>
        /// <param name="filData"></param>
        /// <param name="plazo"></param>
        /// <returns></returns>
        protected List<bool> CargarListaFilaReadOnly(DateTime fechaProceso, int idEmpresa, int formatcodi, int filHead, int filData, bool plazo)
        {
            List<bool> lista = new List<bool>();
            int horaini = 0;
            int horaAmplIni = 0;
            int horaAmplFin = 0;
            DateTime fechaActual = DateTime.Now;
            int hora = fechaActual.Hour;
            List<int> listaHoras = new List<int>();
            if (hora < ConstantesStockCombustibles.BandaTR)
            {
                horaini = 0;
            }
            else
            {
                horaini = hora - 1 * ConstantesStockCombustibles.BandaTR;
            }
            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }


            listaHoras.Add(horaini);
            var regfechaPlazo = servFormato.GetByIdMeAmpliacionfecha(fechaProceso, idEmpresa, formatcodi);
            if (regfechaPlazo != null)
            {
                horaAmplFin = regfechaPlazo.Amplifechaplazo.Hour;
                if (horaAmplFin < ConstantesStockCombustibles.BandaTR)
                {
                    horaAmplIni = 0;
                }
                else
                {
                    horaAmplIni = horaAmplFin - 1 * ConstantesStockCombustibles.BandaTR;
                }
            }

            for (int i = 0; i < filData; i++)
            {
                if (plazo)
                {
                    if (i >= horaini)
                        lista.Add(false);
                    else
                    {
                        if ((i >= horaAmplIni) && (i < horaAmplFin))
                        {
                            lista.Add(false);
                        }
                        else
                            lista.Add(true);
                    }
                }
                else
                    lista.Add(false);

            }
            return lista;
        }


        /// <summary>
        /// Graba los datos enviados por el agente del formato presion de Gas
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWebPresion(string[][] data, int idEmpresa, string fecha)
        {
            base.ValidarSesionUsuario();
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int idFormato = ConstantesStockCombustibles.IdFormatoPGas;
            int exito = 0;
            string empresa = string.Empty;
            var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa); ;
            //////////////////////////////////////////////////
            //Validación de vigencia de empresa
            if (!this.servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now))
            {
                return Json(model);
            }
            if (regEmp != null)
                empresa = regEmp.Emprnomb;

            MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
            var cabercera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            formato.Formatcols = cabercera.Cabcolumnas;
            formato.Formatrows = cabercera.Cabfilas;
            formato.Formatheaderrow = cabercera.Cabcampodef;
            int filaHead = formato.Formatrows;
            int colHead = formato.Formatcols;

            formato.Emprcodi = idEmpresa;
            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
            FormatoMedicionAppServicio.GetSizeFormato(formato);

            var listaPto = servFormato.GetByCriteriaMeHojaptomeds(idEmpresa, idFormato, formato.FechaInicio, formato.FechaFin);
            int nPtos = listaPto.Count();

            string tipoPlazo = servFormato.EnvioValidarPlazo(formato, idEmpresa);
            if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

            /////////////// Grabar Config Formato Envio //////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = idFormato;
            config.Emprcodi = idEmpresa;
            config.FechaInicio = formato.FechaInicio;
            config.FechaFin = formato.FechaFin;
            int idConfig = servFormato.GrabarConfigFormatEnvio(config);
            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = servFormato.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);
            MeEnvioDTO envio = new MeEnvioDTO();
            envio.Archcodi = 0;
            envio.Emprcodi = idEmpresa;
            envio.Enviofecha = DateTime.Now;
            envio.Enviofechaperiodo = formato.FechaProceso;
            envio.Enviofechaini = formato.FechaInicio;
            envio.Enviofechafin = formato.FechaFin;
            envio.Envioplazo = (enPlazo) ? "P" : "F";
            envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
            envio.Lastdate = DateTime.Now;
            envio.Lastuser = User.Identity.Name;
            envio.Userlogin = User.Identity.Name;
            envio.Formatcodi = idFormato;
            envio.Cfgenvcodi = idConfig;
            int idEnvio = servFormato.SaveMeEnvio(envio);
            model.IdEnvio = idEnvio;
            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            try
            {
                var lista24 = ObtenerDatosPresion(data, listaPto, formato.Formatcheckblanco);
                servicio.GrabarValoresCargados24(lista24, User.Identity.Name, idEnvio, idEmpresa, formato);
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                servFormato.UpdateMeEnvio(envio);
                EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
    (DateTime)envio.Enviofecha, ConstantesStockCombustibles.IdFormatoPGas);
                exito = 1;
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                exito = -1;
                model.Resultado = -1;
            }

            model.Resultado = exito;
            return Json(model);
        }

        /// <summary>
        /// Lee los datos del  formato web presion de gas y los almacena en una lista de DTO Medicion24
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<MeMedicion24DTO> ObtenerDatosPresion(string[][] datos, List<MeHojaptomedDTO> ptos, int checkBlanco)
        {
            int nFil = 27;
            int nCol = ptos.Count + 1;
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            MeMedicion24DTO reg;
            string stValor = string.Empty;
            decimal valor = decimal.MinValue;
            DateTime fecha = DateTime.MinValue;
            for (var i = 1; i < nCol; i++)
            {
                reg = new MeMedicion24DTO();
                for (var j = 3; j < nFil; j++)
                {
                    int indice = j - 2;
                    reg.Ptomedicodi = ptos[i - 1].Ptomedicodi;
                    reg.Tipoinfocodi = ptos[i - 1].Tipoinfocodi;
                    reg.Emprcodi = ptos[i - 1].Emprcodi;
                    reg.Meditotal = 0;
                    reg.Lectcodi = ConstantesStockCombustibles.LectCodiPresionGas;
                    fecha = DateTime.ParseExact(datos[j][0], Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                    reg.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
                    stValor = datos[j][i];
                    if (COES.Base.Tools.Util.EsNumero(stValor))
                    {
                        valor = decimal.Parse(stValor);
                        reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, valor);
                    }
                    else
                    {
                        if (checkBlanco == 0)
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, null);
                        else
                            reg.GetType().GetProperty("H" + indice.ToString()).SetValue(reg, 0);
                    }
                }
                lista.Add(reg);
            }
            return lista;
        }

        /// <summary>
        /// Permite generar el formato en formato excel de presion de gas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormatoPresion(string[][] data, int idEmpresa, string dia)
        {
            int idEnvio = -2;
            string ruta = string.Empty;
            try
            {
                this.MatrizExcel = data;
                StockCombustiblesModel model = BuildHojaExcelPresion(idEmpresa, idEnvio, dia);
                ruta = Tools.GenerarFileExcelPresion(model);
            }
            catch
            {
                ruta = "-1";
            }
            return ruta;
        }

        #endregion

        #region METODOS ENVIO DISPONIBILIDAD DE GAS

        /// <summary>
        /// Index Disponibilidad de Gas
        /// </summary>
        /// <returns></returns>
        public ActionResult DisponibilidadGas()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            StockCombustiblesModel model = new StockCombustiblesModel();
            try
            {


                bool accesoEmpresa = seguridad.ValidarPermisoOpcion(this.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
                var empresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoDisponibilidadGas);
                if (accesoEmpresa)
                {
                    model.ListaEmpresas = empresas;
                }
                else
                {
                    var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                        Select(x => new SiEmpresaDTO()
                        {
                            Emprcodi = x.EMPRCODI,
                            Emprnomb = x.EMPRNOMB
                        });
                    if (emprUsuario.Count() > 0)
                    {
                        model.ListaEmpresas = emprUsuario.ToList();
                    }
                    else
                    {
                        model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                    }
                }
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }

            return View(model);

        }


        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGridExcelWebDisponibilidad(int idEmpresa, int idEnvio, string fecha)
        {
            base.ValidarSesionUsuario();
            List<MeFormatoDTO> entitys = servFormato.GetByModuloLecturaMeFormatos((int)base.IdModulo, ConstantesStockCombustibles.LectCodiDisponibilidad, idEmpresa);
            if (entitys.Count > 0)
            {
                StockCombustiblesModel jsModel = GetModelFormatoDisponibilidad(idEmpresa, idEnvio, fecha);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }

        }

        /// <summary>
        /// Devuelve el model para mostrar en la pagina  web de envio de Disponibilidad de Gas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public StockCombustiblesModel GetModelFormatoDisponibilidad(int idEmpresa, int idEnvio, string fecha)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.Formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoDisponibilidadGas);
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);

            int idEnvioUltimo = 0;
            model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoDisponibilidadGas, idEmpresa, fechaProceso);
            model.Handson = new HandsonModel();
            model.Handson.ReadOnly = false;
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.ListaHojaPto = model.Formato.ListaHoja[0].ListaPtos;

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
            model.Fecha = model.Formato.FechaProceso.ToString(Constantes.FormatoFecha);
            model.FechaNext = model.Formato.FechaProceso.AddDays(1).ToString(Constantes.FormatoFecha);
            var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoDisponibilidadGas, model.Formato.FechaProceso);
            if (listaEnvios.Count() > 0)
            {
                idEnvioUltimo = listaEnvios.Max(x => x.Enviocodi);
                model.IdEnvioLast = idEnvioUltimo;
                model.ListaEnvios = listaEnvios;
                if (idEnvio == -1)
                {
                    idEnvio = model.IdEnvioLast;
                }
            }
            else
                model.ListaEnvios = listaEnvios;

            ///
            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }

            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            var fechaIni = model.Formato.FechaProceso;//.AddHours(6);
            var fechaFin = fechaIni.AddDays(1).AddSeconds(-1);
            var listaData = new List<MeMedicionxintervaloDTO>();

            int idCfgFormato = 0;
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = servFormato.ValidarPlazo(model.Formato);
                model.TipoPlazo = servFormato.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo;
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                var envioAnt = listaEnvios.Where(x => x.Enviocodi == idEnvio).FirstOrDefault(); //servFormato.GetByIdMeEnvio(idEnvio);
                model.Handson.ReadOnly = true;
                if (envioAnt != null)
                {
                    idCfgFormato = envioAnt.Cfgenvcodi.GetValueOrDefault(0);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);

                    if (idEnvio == idEnvioUltimo)
                        listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                    else
                        listaData = servFormato.GetEnvioCambioMedicionXIntervalo(idEnvio);

                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
            }
            model.ListaHojaPto = this.servFormato.GetListaPtosByFormato(model.ListaHojaPto, idCfgFormato);
            model.ListaPtoMedicion = model.ListaHojaPto.Select(x => new ListaSelect
            {
                id = x.Ptomedicodi,
                text = x.Equinomb
            }).ToList();
            List<int> listaptomedicodi = model.ListaHojaPto.Select(x => x.Ptomedicodi).ToList();
            listaData = listaData.Where(x => listaptomedicodi.Contains(x.Ptomedicodi)).ToList();

            int nBloques = listaData.Count;
            model.Handson.ListaExcelData = new string[nBloques + 1][];
            model.Handson.ListaSourceDropDown = new string[1][];
            model.Handson.ListaSourceDropDown[0] = new string[2];
            model.Handson.ListaSourceDropDown[0][0] = model.Formato.FechaProceso.ToString(ConstantesBase.FormatoFecha);
            model.Handson.ListaSourceDropDown[0][1] = model.Formato.FechaProceso.AddDays(1).ToString(ConstantesBase.FormatoFecha);
            Tools.LoadMatrizExcelDisponibilidad(model.Handson.ListaExcelData, listaData, ConstantesCombustible.NColumnaDisp);
            model.IdEnvio = idEnvio;

            return model;
        }


        /// <summary>
        /// Graba los datos enviados por el agente del formato presion de Gas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDisponibilidad(string[][] data, int idEmpresa, string fecha)
        {
            // base.ValidarSesionUsuario();
            FormatoResultado model = new FormatoResultado();
            try
            {
                int idEnvio = 0;
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                MeFormatoDTO formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoDisponibilidadGas, idEmpresa, fechaProceso);
                formato.FechaProceso = fechaProceso;
                List<MeMedicionxintervaloDTO> entitys = this.GetDatosExcelDisponibilidad(data, formato.FechaProceso);
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                string empresa = string.Empty;
                var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa);
                //Validación de vigencia de empresa
                if (!this.servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now))
                {
                    return Json(model);
                }
                if (regEmp != null)
                    empresa = regEmp.Emprnomb;

                formato.Emprcodi = idEmpresa;
                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                //
                var listaPto = servFormato.GetByCriteriaMeHojaptomeds(idEmpresa, ConstantesStockCombustibles.IdFormatoDisponibilidadGas, formato.FechaInicio, formato.FechaFin);
                int nPtos = listaPto.Count();

                string tipoPlazo = servFormato.EnvioValidarPlazo(formato, idEmpresa);
                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

                /////////////// Grabar Config Formato Envio //////////////////
                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = ConstantesStockCombustibles.IdFormatoDisponibilidadGas;
                config.Emprcodi = idEmpresa;
                config.FechaInicio = formato.FechaInicio;
                config.FechaFin = formato.FechaFin;
                int idConfig = servFormato.GrabarConfigFormatEnvio(config);
                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                Boolean enPlazo = servFormato.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                envio.Enviofechaini = formato.FechaInicio;
                envio.Enviofechafin = formato.FechaFin;
                envio.Envioplazo = (enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = ConstantesStockCombustibles.IdFormatoDisponibilidadGas;
                envio.Cfgenvcodi = idConfig;
                idEnvio = servFormato.SaveMeEnvio(envio);
                model.IdEnvio = idEnvio;

                servicio.GrabarDisponibilidadGas(entitys, User.Identity.Name, idEnvio, idEmpresa, formato);
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                servFormato.UpdateMeEnvio(envio);
                EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
    (DateTime)envio.Enviofecha, ConstantesStockCombustibles.IdFormatoDisponibilidadGas);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                model.Resultado = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// Lee los datos del  formato web disponibilidad de gas y los almacena en una lista de DTO MedicionxIntervalo
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<MeMedicionxintervaloDTO> GetDatosExcelDisponibilidad(string[][] datos, DateTime fecha)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO entity;
            if (datos.Length > 1)
            {
                for (int i = 1; i < datos.Length; i++)
                {
                    entity = new MeMedicionxintervaloDTO();
                    entity.Medintfechaini = fecha;

                    if (datos[i][1].Length < 10)
                        entity.Medintfechafin = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " " + datos[i][1], ConstantesBase.FormatoFechaExtendido, CultureInfo.InvariantCulture);//DateTime.ParseExact(datos[i][1] + " " + datos[i][2],ConstantesBase.FormatoFechaExtendido, CultureInfo.InvariantCulture);
                    else
                        entity.Medintfechafin = DateTime.ParseExact(datos[i][1], ConstantesBase.FormatoFechaExtendido, CultureInfo.InvariantCulture);

                    entity.Ptomedicodi = int.Parse(datos[i][0]);
                    entity.Medintusumodificacion = User.Identity.Name;
                    entity.Medintfecmodificacion = DateTime.Now;
                    entity.Tipoinfocodi = ConstantesCombustible.UnidadDisponibilidad;
                    entity.Lectcodi = ConstantesStockCombustibles.LectCodiDisponibilidad;
                    entity.Medinth1 = decimal.Parse(datos[i][2]);
                    entity.Medintdescrip = datos[i][4];
                    entity.Medestcodi = int.Parse(datos[i][3]);
                    entity.Emprcodi = int.Parse(datos[i][6]);
                    lista.Add(entity);
                }
            }

            return lista;
        }

        #endregion

        #region METODOS QUEMA Y VENTEO DE GAS

        /// <summary>
        /// Index de Quema y Venteo de Gas
        /// </summary>
        /// <returns></returns>
        public ActionResult QuemaGas()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();
            StockCombustiblesModel model = new StockCombustiblesModel();
            try
            {
                bool accesoEmpresa = seguridad.ValidarPermisoOpcion(this.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
                var empresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoQuemaGas);
                if (accesoEmpresa)
                {
                    model.ListaEmpresas = empresas;
                }
                else
                {
                    var emprUsuario = base.ListaEmpresas.Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi)).
                        Select(x => new SiEmpresaDTO()
                        {
                            Emprcodi = x.EMPRCODI,
                            Emprnomb = x.EMPRNOMB
                        });
                    if (emprUsuario.Count() > 0)
                    {
                        model.ListaEmpresas = emprUsuario.ToList();
                    }
                    else
                    {
                        model.ListaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                    }
                }
                model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            }
            catch (Exception ex)
            {
                log.Error("Error Quema Gas:", ex);
            }
            return View(model);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Quema y Venteo de Gas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGridExcelWebQuemaGas(int idEmpresa, int idEnvio, string fecha)
        {
            base.ValidarSesionUsuario();
            List<MeFormatoDTO> entitys = servFormato.GetByModuloLecturaMeFormatos((int)base.IdModulo, ConstantesStockCombustibles.LectCodiQuemaGas, idEmpresa);
            if (entitys.Count > 0)
            {
                StockCombustiblesModel jsModel = GetModelFormatoQuemaGas(idEmpresa, idEnvio, fecha);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Devuelve el model para mostrar en la pagina  web de envio de quema de gas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public StockCombustiblesModel GetModelFormatoQuemaGas(int idEmpresa, int idEnvio, string fecha)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.Formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoQuemaGas);
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);

            int idEnvioUltimo = 0;
            model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoQuemaGas, idEmpresa, fechaProceso);
            model.Handson = new HandsonModel();
            model.Handson.ReadOnly = false;
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(250);
            model.Handson.ListaColWidth.Add(130);
            model.Handson.ListaColWidth.Add(110);
            model.Handson.ListaColWidth.Add(150);
            model.Handson.ListaColWidth.Add(280);
            model.Handson.ListaColWidth.Add(50);
            model.ListaHojaPto = model.Formato.ListaHoja[0].ListaPtos;

            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
            model.Fecha = model.Formato.FechaProceso.ToString(Constantes.FormatoFecha);

            var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoQuemaGas, model.Formato.FechaProceso);
            if (listaEnvios.Count() > 0)
            {
                idEnvioUltimo = listaEnvios.Max(x => x.Enviocodi);
                model.ListaEnvios = listaEnvios;
                model.IdEnvioLast = idEnvioUltimo;
                if (idEnvio == -1)
                {
                    idEnvio = model.IdEnvioLast;
                }
            }
            else
                model.ListaEnvios = listaEnvios;
            idEnvio = idEnvio <= 0 ? 0 : idEnvio;

            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }

            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            var fechaIni = model.Formato.FechaProceso;
            var fechaFin = fechaIni.AddDays(1);
            var listaData = new List<MeMedicionxintervaloDTO>();

            int idCfgFormato = 0;
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = servFormato.ValidarPlazo(model.Formato);
                model.TipoPlazo = servFormato.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == model.TipoPlazo;
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                var envioAnt = listaEnvios.Where(x => x.Enviocodi == idEnvio).FirstOrDefault(); //servFormato.GetByIdMeEnvio(idEnvio);
                model.Handson.ReadOnly = true;
                if (envioAnt != null)
                {
                    idCfgFormato = envioAnt.Cfgenvcodi.GetValueOrDefault(0);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    if (idEnvio == idEnvioUltimo)
                        listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                    else
                        listaData = servFormato.GetEnvioCambioMedicionXIntervalo(idEnvio);
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
            }
            model.ListaHojaPto = this.servFormato.GetListaPtosByFormato(model.ListaHojaPto, idCfgFormato);
            model.ListaPtoMedicion = model.ListaHojaPto.Select(x => new ListaSelect
            {
                id = x.Ptomedicodi,
                text = x.Equinomb
            }).ToList();
            List<int> listaptomedicodi = model.ListaHojaPto.Select(x => x.Ptomedicodi).ToList();
            listaData = listaData.Where(x => listaptomedicodi.Contains(x.Ptomedicodi)).ToList();

            int nBloques = listaData.Count;
            model.Handson.ListaExcelData = new string[nBloques + 1][];
            model.Handson.ListaSourceDropDown = new string[1][];
            model.Handson.ListaSourceDropDown[0] = new string[2];
            model.Handson.ListaSourceDropDown[0][0] = "Quema de Gas";
            model.Handson.ListaSourceDropDown[0][1] = "Venteo de Gas";
            Tools.LoadMatrizExcelQuemaGas(model.Handson.ListaExcelData, listaData, ConstantesCombustible.NColumnaQuema);
            model.IdEnvio = idEnvio;
            return model;
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato Quema y Venteo de Gas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarQuemaGas(string[][] data, int idEmpresa, string fecha)
        {
            // base.ValidarSesionUsuario();
            int idEnvio = 0;
            FormatoResultado model = new FormatoResultado();
            try
            {
                List<MeMedicionxintervaloDTO> entitys = this.GetDatosExcelQuemaGas(data, fecha);
                DateTime fechaPeriodo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                MeFormatoDTO formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoQuemaGas, idEmpresa, fechaPeriodo);
                formato.FechaProceso = fechaPeriodo;
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                string empresa = string.Empty;
                var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa);
                //Validación de vigencia de empresa
                if (!this.servFormato.EsEmpresaVigente(idEmpresa, DateTime.Now))
                {
                    return Json(model);
                }
                if (regEmp != null)
                    empresa = regEmp.Emprnomb;

                formato.Emprcodi = idEmpresa;
                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                //
                var listaPto = servFormato.GetByCriteriaMeHojaptomeds(idEmpresa, ConstantesStockCombustibles.IdFormatoQuemaGas, formato.FechaInicio, formato.FechaFin);
                int nPtos = listaPto.Count();

                string tipoPlazo = servFormato.EnvioValidarPlazo(formato, idEmpresa);
                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

                /////////////// Grabar Config Formato Envio //////////////////
                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = ConstantesStockCombustibles.IdFormatoQuemaGas;
                config.Emprcodi = idEmpresa;
                config.FechaInicio = formato.FechaInicio;
                config.FechaFin = formato.FechaFin;
                int idConfig = servFormato.GrabarConfigFormatEnvio(config);
                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                Boolean enPlazo = servFormato.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                envio.Enviofechaini = formato.FechaInicio;
                envio.Enviofechafin = formato.FechaFin;
                envio.Envioplazo = (enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = ConstantesStockCombustibles.IdFormatoQuemaGas;
                idEnvio = servFormato.SaveMeEnvio(envio);
                model.IdEnvio = idEnvio;

                servicio.GrabarQuemaGas(entitys, User.Identity.Name, idEnvio, idEmpresa, formato);
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = idConfig;
                servFormato.UpdateMeEnvio(envio);
                model.Resultado = 1;
                EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
    (DateTime)envio.Enviofecha, ConstantesStockCombustibles.IdFormatoQuemaGas);

            }
            catch (Exception ex)
            {
                log.Error("Error Save Quema Gas:", ex);
                model.Resultado = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato Quema y Venteo de Gas
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<MeMedicionxintervaloDTO> GetDatosExcelQuemaGas(string[][] datos, string fecha)
        {
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            MeMedicionxintervaloDTO entity;
            if (datos.Length > 1)
            {
                for (int i = 1; i < datos.Length; i++)
                {
                    entity = new MeMedicionxintervaloDTO();
                    entity.Medintfechaini = DateTime.ParseExact(fecha + " " + datos[i][2],
                        Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    entity.Ptomedicodi = int.Parse(datos[i][0]);
                    entity.Medintusumodificacion = User.Identity.Name;
                    entity.Medintfecmodificacion = DateTime.Now;
                    entity.Tipoinfocodi = ConstantesCombustible.UnidadQuemaGas;
                    entity.Lectcodi = ConstantesStockCombustibles.LectCodiQuemaGas;
                    entity.Medinth1 = decimal.Parse(datos[i][3]);
                    entity.Medintdescrip = datos[i][4];
                    entity.Medestcodi = int.Parse(datos[i][1]);
                    entity.Emprcodi = int.Parse(datos[i][6]);
                    lista.Add(entity);
                }
            }

            return lista;
        }

        #endregion

        #region UTIL

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {

            string strArchivoTemporal = Request["archivo"];

            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);


                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("Archivo_{0:yyyymmdd_HHmmss}.xlsx", DateTime.Now);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustible.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesCombustible.ExtensionFile;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Mensaje = ex.ToString(), Detalle = ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Lee datos desde excel
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult LeerFileUpExcel(int idEmpresa, string dia)
        {
            int retorno = Tools.VerificarIdsFormato(this.NombreFile, idEmpresa, ConstantesStockCombustibles.IdFormatoPGas);

            if (retorno > 0)
            {
                MeFormatoDTO formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoPGas);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, dia, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabercera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var listaPtos = servFormato.GetByCriteria2MeHojaptomeds(idEmpresa, ConstantesStockCombustibles.IdFormatoPGas, cabercera.Cabquery, fechaProceso, fechaProceso);
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                this.MatrizExcel = Tools.LeerExcelFile(this.NombreFile, listaPtos, dia);
            }
            Tools.BorrarArchivo(this.NombreFile);
            return Json(retorno);
        }

        /// <summary>
        /// Envia correo segun plantilla y graba el correo en base de datos
        /// </summary>
        /// <param name="enPlazo"></param>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatoNombre"></param>
        /// <param name="empresaNombre"></param>
        /// <param name="areaNombre"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="fechaEnvio"></param>
        protected void EnviarCorreo(bool enPlazo, int idEnvio, int idEmpresa, string formatoNombre, string empresaNombre,
            string areaNombre, DateTime fechaProceso, DateTime fechaEnvio, int formatcodi)
        {
            var usuario = User.Identity.Name;
            var plantilla = servCorreo.ObtenerPlantillaPorModulo(TipoPlantillaCorreo.NotificacionEnvioExtranet, (int)base.IdModulo);

            if (ConstantesStockCombustibles.IdFormatoConsumo == formatcodi && plantilla != null)
            {
                List<string> ccEmails = seguridad.ObtenerModulo((int)base.IdModulo).ListaAdministradores.ToList().Select(x => x.UserEmail).ToList();
                string ccMail = string.Empty;
                string cumplimiento = enPlazo ? "En Plazo" : "En fuera de plazo";
                string asunto = string.Format(plantilla.Plantasunto, formatoNombre);
                List<string> toMail = new List<string>();
                usuario = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;

                toMail.Add(usuario);
                string contenido = string.Format(plantilla.Plantcontenido, User.Identity.Name, cumplimiento, empresaNombre, formatoNombre
                    , fechaProceso.ToString(Constantes.FormatoFecha), ((DateTime)fechaEnvio).ToString(Constantes.FormatoFechaFull), idEnvio, areaNombre);
                COES.Base.Tools.Util.SendEmail(toMail, ccEmails, asunto, contenido);
                var correo = new SiCorreoDTO();
                correo.Corrasunto = asunto;
                correo.Corrcontenido = contenido;
                correo.Corrfechaenvio = fechaEnvio;
                correo.Corrfechaperiodo = fechaProceso;
                correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
                correo.Corrto = usuario;
                correo.Emprcodi = idEmpresa;
                correo.Enviocodi = idEnvio;
                correo.Plantcodi = plantilla.Plantcodi;
                servCorreo.SaveSiCorreo(correo);
            }
        }

        protected void EnviarCorreoRecepcion(List<MeMedicionxintervaloDTO> entitys, int idEmpresa, string empresaNombre, DateTime fechaProceso, int idEnvio)
        {
            string listaRecepcion = "<TABLE class='mail' style='border-spacing:0;' border=1><TR><TD style='background: #417AA7;color:#ffffff;' width=250>Central</td>";
            listaRecepcion += "<td style='background: #417AA7;color:#ffffff;' width=150>Tipo</td>";
            listaRecepcion += "<td style='background: #417AA7;color:#ffffff;' width=100>Recepci&oacute;n</td>";
            listaRecepcion += "<td style='background: #417AA7;color:#ffffff;' width=100>Unidades</td></tr>";
            var usuario = User.Identity.Name;
            var plantilla = servCorreo.ObtenerPlantillaPorModulo(TipoPlantillaCorreo.NotificacionAdministradorModulo, (int)base.IdModulo);
            bool enviaCorreo = false;
            if (plantilla != null)
            {
                foreach (var reg in entitys)
                {
                    if (reg.H1Recep != null)
                        if (reg.H1Recep > 0)
                        {
                            listaRecepcion += "<tr><td>" + reg.Equinomb + "</td><td>" + reg.Fenergnomb + "</td><td>" + reg.H1Recep.ToString() + "</td><td> " + reg.Tipoinfoabrev + " </td></tr>";
                            enviaCorreo = true;
                        }
                }

                listaRecepcion += "</table>";
                if (enviaCorreo)
                {
                    List<string> ccEmails = seguridad.ObtenerModulo((int)base.IdModulo).ListaAdministradores.ToList().Select(x => x.UserEmail).ToList();
                    string ccMail = string.Empty;
                    string asunto = plantilla.Plantasunto;
                    List<string> toMail = new List<string>();
                    usuario = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;
                    toMail.Add(usuario);
                    string contenido = string.Format(plantilla.Plantcontenido, empresaNombre,
                         fechaProceso.ToString(Constantes.FormatoFecha), listaRecepcion);
                    COES.Base.Tools.Util.SendEmail(toMail, ccEmails, asunto, contenido);
                    var correo = new SiCorreoDTO();
                    correo.Corrasunto = asunto;
                    correo.Corrcontenido = contenido;
                    correo.Corrfechaenvio = DateTime.Now;
                    correo.Corrfechaperiodo = fechaProceso;
                    correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
                    correo.Corrto = usuario;
                    correo.Emprcodi = idEmpresa;
                    correo.Enviocodi = idEnvio;
                    correo.Plantcodi = plantilla.Plantcodi;
                    servCorreo.SaveSiCorreo(correo);
                }
            }
        }

        protected void EnviarCorreoValidacionStockCombustible(List<string> listaMensaje, int idEmpresa, string empresaNombre, DateTime fechaProceso, int idEnvio)
        {
            var usuario = User.Identity.Name;
            if (listaMensaje != null && listaMensaje.Any())
            {
                List<string> ccEmails = seguridad.ObtenerModulo((int)base.IdModulo).ListaAdministradores.Select(x => x.UserEmail).ToList();
                string ccMail = string.Empty;
                string asunto = string.Format("Notificación de Validación Envío de Información - {0} {1}", fechaProceso.ToString(ConstantesAppServicio.FormatoFecha), empresaNombre);
                string contenido = GetContenidoCorreoValidacionStock(listaMensaje, empresaNombre, fechaProceso.ToString(ConstantesAppServicio.FormatoFecha), fechaProceso.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha));
                COES.Base.Tools.Util.SendEmail(ccEmails, new List<string>(), asunto, contenido);
                var correo = new SiCorreoDTO();
                correo.Corrasunto = asunto;
                correo.Corrcontenido = contenido;
                correo.Corrfechaenvio = DateTime.Now;
                correo.Corrfechaperiodo = fechaProceso;
                correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
                correo.Corrto = usuario;
                correo.Emprcodi = idEmpresa;
                correo.Enviocodi = idEnvio;
                correo.Plantcodi = 6;
                servCorreo.SaveSiCorreo(correo);
            }
        }

        /// <summary>
        /// Envio correo de trangresion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaProceso"></param>
        /// <param name="idEnvio"></param>
        protected void EnviarCorreoValidacionTransgresion(int idEmpresa, DateTime fechaProceso, int idEnvio)
        {
            var usuario = User.Identity.Name;
            var plantilla = servCorreo.GetByIdSiPlantillacorreo(ConstantesConsumoCombustible.PlantcodiTransgresion);

            var servComb = new ConsumoCombustibleAppServicio();
            List<CccReporteDTO> listaTransg = servComb.ListarTransgresionXEmpresa(fechaProceso, idEmpresa);
            var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa);

            if (listaTransg.Any())
            {
                List<string> ccEmails = seguridad.ObtenerModulo((int)base.IdModulo).ListaAdministradores.Select(x => x.UserEmail).ToList();

                string strTabla = servComb.GenerarTablaHtmlListadoRptDiario(listaTransg);

                string ccMail = string.Empty;
                string asunto = string.Format(plantilla.Plantasunto, fechaProceso.ToString(ConstantesAppServicio.FormatoFecha), regEmp.Emprnomb);
                string contenido = string.Format(plantilla.Plantcontenido, strTabla, CorreoAppServicio.GetFooterCorreo());

                COES.Base.Tools.Util.SendEmail(ccEmails, new List<string>(), asunto, contenido);
                var correo = new SiCorreoDTO();
                correo.Corrasunto = asunto;
                correo.Corrcontenido = contenido;
                correo.Corrfechaenvio = DateTime.Now;
                correo.Corrfechaperiodo = fechaProceso;
                correo.Corrfrom = HelperApp.ObtenerEmailRemitente();
                correo.Corrto = usuario;
                correo.Emprcodi = idEmpresa;
                correo.Enviocodi = idEnvio;
                correo.Plantcodi = ConstantesConsumoCombustible.PlantcodiTransgresion;
                servCorreo.SaveSiCorreo(correo);
            }
        }

        /// <summary>
        /// Generación del contenido del correo
        /// </summary>
        /// <param name="listaDespues"></param>
        /// <param name="listaAntes"></param>
        /// <returns></returns>
        private string GetContenidoCorreoValidacionStock(List<string> listaMensaje, string empresa, string strfecha, string strfechaSig)
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
                        Se ha encontrado las siguientes observaciones. La actualización de los datos del día {fechaCarga} de la empresa {nombreEmpresa} afecta la carga de datos del día {fechaCargaSig}:
                        <br/><br/>
                        {cuerpoObs}
                        <br/><br/>
    
                        {footer}
                    </body>
                </html>
            ";
            html = html.Replace("{fechaCarga}", strfecha);
            html = html.Replace("{fechaCargaSig}", strfechaSig);
            html = html.Replace("{nombreEmpresa}", empresa);
            html = html.Replace("{cuerpoObs}", this.GetHtmlTablaValidacionEmail(listaMensaje));

            html = html.Replace("{footer}", CorreoAppServicio.GetFooterCorreo());

            return html;
        }

        /// <summary>
        /// Html tabla para email
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string GetHtmlTablaValidacionEmail(List<string> lista)
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
                                    <td style='font-weight: bold;'>Observación:</td>
                                    <td style=''>{0}</td>
                                </tr>
                            </tbody>
                        </table>
                    ";

                    htmlTr = string.Format(htmlTr, reg);
                    str.Append(htmlTr);
                }
            }

            #endregion

            return str.ToString();
        }

        /// <summary>
        /// carga los datos del model para el panel IEOD
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GetInformacionPanelIEOD(int idEmpresa, string fecha)
        {
            try
            {
                if (!base.IsValidSesion) throw new Exception("La sesión del usuario ha expirado");
                if (this.IdModulo == null) throw new Exception("No tiene Acceso a está opción");

                DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                FormatoModel model = new FormatoModel();
                model.ListaPanelIEOD = servIeod.ObtenerPanelIEOD((int)this.IdModulo, idEmpresa, dfecha, null);

                return Json(model);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message, Detalle = ex.StackTrace });
            }
        }

        #endregion

    }
}
