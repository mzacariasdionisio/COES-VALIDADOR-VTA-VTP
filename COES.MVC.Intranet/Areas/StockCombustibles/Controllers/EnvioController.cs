using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Areas.StockCombustibles.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.StockCombustibles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.MVC.Intranet.Models;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.StockCombustibles.Controllers
{
    public class EnvioController : BaseController
    {
        //
        // GET: /StockCombustibles/Envio/
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        StockCombustiblesAppServicio logic = new StockCombustiblesAppServicio();
        StockCombustiblesAppServicio servicio = new StockCombustiblesAppServicio();
        public ActionResult Index()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaEstadoEnvio = servFormato.ListMeEstadoenvios();
            model.ListaCombo = servicio.ListaFormatos().OrderBy(x => x.Formatnombre).ToList();
            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de envío
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsEstado"></param>
        /// <param name="nPaginas"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string idsEmpresa, string fechaIni, string fechaFin, string idsFormato, string idsEstado, int nPaginas)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            var lista = servFormato.GetListaMultipleMeEnvios(idsEmpresa, string.Empty, idsFormato, idsEstado, fechaInicial, fechaFinal, nPaginas, Constantes.PageSize);
            model.ListaEnvio = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string idsEmpresa, string fechaIni, string fechaFin, string idsFormato, string idsEstado)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.IndicadorPagina = false;
            //var formato = logic.GetByIdMeFormato(idTipoInformacion);
            //formato.ListaHoja = logic.GetByCriteriaMeFormatohojas(idTipoInformacion);
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (fechaIni != null)
            {
                fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFin != null)
            {
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            int nroRegistros = servFormato.TotalListaMultipleMeEnvios(idsEmpresa, string.Empty, idsFormato, idsEstado, fechaInicial, fechaFinal);

            if (nroRegistros > 0)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        // exporta el reporte general consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarArchivoReporteXLS(string idsEmpresa, string fechaIni, string fechaFin, string idsFormato, string idsLectura, string idsEstado)
        {
            int indicador = 1;

            StockCombustiblesModel model = new StockCombustiblesModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
            DateTime fechaInicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            try
            {
                if (fechaIni != null)
                {
                    fechaInicial = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFin != null)
                {
                    fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                this.logic.GeneraExcelEnvio(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaInicial, fechaFinal, ruta + ConstantesIntranet.NombreArchivoEnvio,
                    ruta + Constantes.NombreLogoCoes);
                indicador = 1;

            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesIntranet.NombreArchivoEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Genera Archivo del envio solicitado
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoEnvio(int idEnvio)
        {
            int indicador = 1;

            StockCombustiblesModel model = new StockCombustiblesModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
            try
            {
                // indicador = this.logic.GenerarArchivoEnvio(idEnvio, ruta);
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarEnvio()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesIntranet.NombreArchivoEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Obtiene las empresas segun formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresas(int idFormato)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return PartialView(model);
        }

        #region METODOS ENVIO DISPONIBILIDAD DE GAS

        /// <summary>
        /// Index Disponibilidad de Gas
        /// </summary>
        /// <returns></returns>
        public ActionResult DisponibilidadGas()
        {
            base.ValidarSesionUsuario();
            StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
            bool accesoEmpresa = true;//seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
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
            return View(model);

        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Disponibilidad de Gas
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGridExcelWebDisponibilidad(int idEmpresa, int idEnvio, string fecha)
        {
            base.ValidarSesionUsuario();
            //(int)base.IdModulo
            List<MeFormatoDTO> entitys = servFormato.GetByModuloLecturaMeFormatos((int)base.IdModulo, ConstantesStockCombustibles.LectCodiDisponibilidad, idEmpresa);
            if (entitys.Count > 0)
            {
                StockCombustibleFormatoModel jsModel = GetModelFormatoDisponibilidad(idEmpresa, idEnvio, fecha);
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
        public StockCombustibleFormatoModel GetModelFormatoDisponibilidad(int idEmpresa, int idEnvio, string fecha)
        {
            StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
            int idEnvioUltimo = 0;
            if (idEnvio > 0)
            {
                var envio = servFormato.GetByIdMeEnvio(idEnvio);
                idEmpresa = (int)envio.Emprcodi;
                DateTime fechaPeriodo = (DateTime)envio.Enviofechaperiodo;
                model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoDisponibilidadGas, idEmpresa, fechaPeriodo);
                model.Formato.FechaProceso = fechaPeriodo;
            }
            else
            {
                model.Formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoDisponibilidadGas);
                DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
                model = new StockCombustibleFormatoModel();
                model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoDisponibilidadGas, idEmpresa, fechaPeriodo);
                model.Formato.FechaProceso = fechaPeriodo;
            }

            model.Handson = new HandsonModel();
            model.Handson.ReadOnly = false;
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.ListaHojaPto = model.Formato.ListaHoja[0].ListaPtos;
            model.ListaPtoMedicion = model.ListaHojaPto.Select(x => new ListaSelect
            {
                id = x.Ptomedicodi,
                text = x.Equinomb
            }).ToList();

            model.Fecha = model.Formato.FechaProceso.ToString(Constantes.FormatoFecha);
            model.FechaNext = model.Formato.FechaProceso.AddDays(1).ToString(Constantes.FormatoFecha);
            var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoDisponibilidadGas, model.Formato.FechaProceso);
            if (listaEnvios.Count() > 0)
            {
                idEnvioUltimo = listaEnvios.Max(x => x.Enviocodi);
                model.ListaEnvios = listaEnvios.Where(x => x.Enviocodi != idEnvioUltimo).ToList();
            }
            else
                model.ListaEnvios = listaEnvios;
            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            var fechaIni = model.Formato.FechaProceso;//.AddHours(6);
            var fechaFin = fechaIni.AddDays(1);
            var listaData = new List<MeMedicionxintervaloDTO>();
            if (idEnvio <= 0)
            {
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
                model.Handson.ReadOnly = !ValidarFecha(model.Formato, idEmpresa);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                model.Handson.ReadOnly = true;
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    //listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                    if (idEnvio == idEnvioUltimo)
                        listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                    else
                        listaData = servFormato.GetEnvioCambioMedicionXIntervalo(idEnvio);
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
            }
            int nBloques = listaData.Count;
            model.Handson.ListaExcelData = new string[nBloques + 1][];
            model.Handson.ListaSourceDropDown = new string[1][];
            model.Handson.ListaSourceDropDown[0] = new string[2];
            model.Handson.ListaSourceDropDown[0][0] = model.Formato.FechaProceso.ToString(ConstantesBase.FormatoFecha);
            model.Handson.ListaSourceDropDown[0][1] = model.Formato.FechaProceso.AddDays(1).ToString(ConstantesBase.FormatoFecha);
            Util.LoadMatrizExcelDisponibilidad(model.Handson.ListaExcelData, listaData, ConstantesIntranet.NColumnaDisp);
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
            int idEnvio = 0;
            DateTime fechaPeriodo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            MeFormatoDTO formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoDisponibilidadGas, idEmpresa, fechaPeriodo);
            formato.FechaProceso = fechaPeriodo;
            FormatoResultado model = new FormatoResultado();
            List<MeMedicionxintervaloDTO> entitys = this.GetDatosExcelDisponibilidad(data, formato.FechaProceso);
            Util.GetSizeFormato(formato);
            string empresa = string.Empty;
            var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa); ;
            if (regEmp != null)
                empresa = regEmp.Emprnomb;
            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

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
            idEnvio = servFormato.SaveMeEnvio(envio);
            model.IdEnvio = idEnvio;
            try
            {

                logic.GrabarDisponibilidadGas(entitys, User.Identity.Name, idEnvio, idEmpresa, formato);
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = 0;
                servFormato.UpdateMeEnvio(envio);
                //            EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
                //(DateTime)envio.Enviofecha);
                model.Resultado = 1;
            }
            catch
            {
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
                    entity.Tipoinfocodi = ConstantesIntranet.UnidadDisponibilidad;
                    entity.Lectcodi = ConstantesStockCombustibles.LectCodiDisponibilidad;
                    entity.Medinth1 = decimal.Parse(datos[i][2]);
                    entity.Medintdescrip = datos[i][4];
                    entity.Medestcodi = int.Parse(datos[i][3]);
                    lista.Add(entity);
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormatoDisponibilidad(int idEnvio)
        {
            int indicador = 0;
            string ruta = string.Empty;
            ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
            try
            {
                StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
                model.IsExcelWeb = false;
                model = GetModelFormatoDisponibilidad(0, idEnvio, string.Empty);
                Util.GenerarFileExcelDisponibilidad(model, ruta + StockConsumoArchivo.ArchivoEnvio);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }
        #endregion

        #region METODOS QUEMA Y VENTEO DE GAS

        /// <summary>
        /// Index de Quema y Venteo de Gas
        /// </summary>
        /// <returns></returns>
        public ActionResult QuemaGas()
        {
            //base.ValidarSesionUsuario();
            StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
            bool accesoEmpresa = true;//seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, User.Identity.Name);
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
            //(int)base.IdModulo
            List<MeFormatoDTO> entitys = servFormato.GetByModuloLecturaMeFormatos((int)base.IdModulo, ConstantesStockCombustibles.LectCodiQuemaGas, idEmpresa);
            if (entitys.Count > 0)
            {
                StockCombustibleFormatoModel jsModel = GetModelFormatoQuemaGas(idEmpresa, idEnvio, fecha);
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
        public StockCombustibleFormatoModel GetModelFormatoQuemaGas(int idEmpresa, int idEnvio, string fecha)
        {
            StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
            if (idEnvio > 0)
            {
                var envio = servFormato.GetByIdMeEnvio(idEnvio);
                idEmpresa = (int)envio.Emprcodi;
                DateTime fechaPeriodo = (DateTime)envio.Enviofechaperiodo;
                model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoQuemaGas, idEmpresa, fechaPeriodo);
                model.Formato.FechaProceso = fechaPeriodo;
            }
            else
            {
                model.Formato = servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoQuemaGas);
                DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
                model = new StockCombustibleFormatoModel();
                model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoQuemaGas, idEmpresa, fechaPeriodo);
                model.Formato.FechaProceso = fechaPeriodo;
            }

            //model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoQuemaGas, idEmpresa);
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
            model.ListaPtoMedicion = model.ListaHojaPto.Select(x => new ListaSelect
            {
                id = x.Ptomedicodi,
                text = x.Equinomb
            }).ToList();
            // model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
            model.Fecha = model.Formato.FechaProceso.ToString(Constantes.FormatoFecha);

            var listaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoQuemaGas, model.Formato.FechaProceso);
            if (listaEnvios.Count() > 0)
            {
                var idEnvioUltimo = listaEnvios.Max(x => x.Enviocodi);
                model.ListaEnvios = listaEnvios.Where(x => x.Enviocodi != idEnvioUltimo).ToList();
            }
            else
                model.ListaEnvios = listaEnvios;
            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            var fechaIni = model.Formato.FechaProceso;
            var fechaFin = fechaIni.AddDays(1);
            var listaData = new List<MeMedicionxintervaloDTO>();
            if (idEnvio <= 0)
            {
                model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
                model.Handson.ReadOnly = !ValidarFecha(model.Formato, idEmpresa);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                model.Handson.ReadOnly = true;
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    listaData = servFormato.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, fechaIni, fechaFin);
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
            }
            int nBloques = listaData.Count;
            model.Handson.ListaExcelData = new string[nBloques + 1][];
            model.Handson.ListaSourceDropDown = new string[1][];
            model.Handson.ListaSourceDropDown[0] = new string[2];
            model.Handson.ListaSourceDropDown[0][0] = "Quema de Gas";
            model.Handson.ListaSourceDropDown[0][1] = "Venteo de Gas";
            Util.LoadMatrizExcelQuemaGas(model.Handson.ListaExcelData, listaData, ConstantesIntranet.NColumnaQuema);
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
            List<MeMedicionxintervaloDTO> entitys = this.GetDatosExcelQuemaGas(data, fecha);
            DateTime fechaPeriodo = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            MeFormatoDTO formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoQuemaGas, idEmpresa, fechaPeriodo);
            formato.FechaProceso = fechaPeriodo;
            Util.GetSizeFormato(formato);
            string empresa = string.Empty;
            var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa); ;
            if (regEmp != null)
                empresa = regEmp.Emprnomb;
            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

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
            try
            {

                logic.GrabarQuemaGas(entitys, User.Identity.Name, idEnvio, idEmpresa, formato);
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = 0;
                servFormato.UpdateMeEnvio(envio);
                model.Resultado = 1;
                //            EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
                //(DateTime)envio.Enviofecha);

            }
            catch
            {
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
                    entity.Tipoinfocodi = ConstantesIntranet.UnidadQuemaGas;
                    entity.Lectcodi = ConstantesStockCombustibles.LectCodiQuemaGas;
                    entity.Medinth1 = decimal.Parse(datos[i][3]);
                    entity.Medintdescrip = datos[i][4];
                    entity.Medestcodi = int.Parse(datos[i][1]);
                    lista.Add(entity);
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormatoQuema(int idEnvio)
        {
            int indicador = 0;
            string ruta = string.Empty;
            ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
            try
            {
                StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
                model.IsExcelWeb = false;
                model = GetModelFormatoQuemaGas(0, idEnvio, string.Empty);
                Util.GenerarFileExcelQuemaGas(model, ruta + StockConsumoArchivo.ArchivoEnvio);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        #endregion

        #region Consumo

        /// <summary>
        ///Devuelve el model necesario para mostrar en la web
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public StockCombustibleFormatoModel BuildHojaExcelConsumo(StockCombustibleFormatoModel model, int idEnvio)
        {
            List<MeMedicionxintervaloDTO> listaMedicion = new List<MeMedicionxintervaloDTO>();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>() { 150, 100, 70, 100, 100, 70, 80, 290 };
            var envio = servFormato.GetByIdMeEnvio(idEnvio);
            // var formato = servFormato.GetByIdMeFormato((int)envio.Formatcodi);
            int idEmpresa = (int)envio.Emprcodi;
            model.Formato = servFormato.GetByIdMeFormatoDetalle(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa, DateTime.Now);


            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
            if (idEnvio <= 0)
            {

            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;
                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora); ;
                    //if (envioAnt.Cfgenvcodi != null)
                    //{
                    //    idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    //}
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
                listaCambios = servFormato.GetAllCambioEnvio(ConstantesStockCombustibles.IdFormatoConsumo, model.Formato.FechaProceso.AddDays(-1), model.Formato.FechaProceso, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
            }
            DateTime fechaIni = model.Formato.FechaProceso;//DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            listaMedicion = servFormato.GetEnvioMedicionXIntervalo(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa, fechaIni.AddDays(-1), fechaIni);
            //this.Formato = model.Formato;
            model.ListaHojaPto = model.Formato.ListaHoja[0].ListaPtos;
            int ncol = ConstantesIntranet.NroColumnasComnsumo; // numero de columnas para la matriz de datos
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
            model.Handson.ListaExcelData = Util.ObtenerMatrizListaExcelData(nfil + nFilStock, ncol, (nFilStock == 0) ? ConstantesIntranet.NroFilHeadConsumo : 2 * ConstantesIntranet.NroFilHeadConsumo + 2);
            model.Handson.ListaMerge = new List<CeldaMerge>();
            CargaConsumoCombustiblesEnMatriz(model, listaMedicion, listaCambios, fechaIni);
            //model.Handson.ListaDropDown = Tools.ObtenerListaDropDown();

            var validaciones = servFormato.GetByCriteriaMeValidacions(ConstantesStockCombustibles.IdFormatoConsumo, idEmpresa).Count;
            if (validaciones == 0)
                model.EnabledStockInicio = true;
            else
                model.EnabledStockInicio = false;

            return model;
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
        public void CargaConsumoCombustiblesEnMatriz(StockCombustibleFormatoModel model,
            List<MeMedicionxintervaloDTO> listaMedicion, List<MeCambioenvioDTO> listaCambios, DateTime fechaIni)
        {
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
                fila = ConstantesIntranet.FilaExcelData + 1;
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

                string formula = "=E" + (fila + 1).ToString(); ;

                for (int j = 1; j < reg.Total; j++)
                {
                    formula += " +E" + (fila + j + 1).ToString();
                }
                model.Handson.ListaExcelData[cordX + fil][cordY + 5] = formula;
                fil += reg.Total;
                fila += reg.Total;
            }
            elemento = new CeldaMerge()
            {
                col = cordY + 6,
                row = cordX,
                colspan = 2,
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
                                .Select(y => y.First()).OrderBy(x => x.Equipadre).ThenByDescending(x => x.Tptomedicodi).ToList();


            int i = ConstantesIntranet.NroFilHeadConsumo;

            foreach (var reg in listaEquipos)
            {
                if (reg.Equipadre > 0)
                    model.Handson.ListaExcelData[cordX + i][cordY] = reg.Equipopadre.Trim();
                else
                    model.Handson.ListaExcelData[cordX + i][cordY] = reg.Equinomb.Trim();
                model.Handson.ListaExcelData[cordX + i][cordY + 1] = reg.Tipoptomedinomb.Trim().Substring(23, reg.Tipoptomedinomb.Trim().Length - 23);
                model.Handson.ListaExcelData[cordX + i][cordY + 2] = reg.Tipoinfoabrev.Trim();
                model.Handson.ListaExcelData[cordX + i][cordY + 3] = reg.Equiabrev.Trim();

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
                            model.Handson.ListaExcelData[cordX + i][cordY + 4] = Util.StrNumeroFormato(cambio.Cambenvdatos);
                            model.ListaCambios.Add(new CeldaCambios()
                            {
                                Row = cordX + i,
                                Col = cordY + 4
                            });
                        }
                        else
                        {
                            model.Handson.ListaExcelData[cordX + i][cordY + 4] = ((decimal)find.Medinth1).ToString("0.00");
                        }
                    }

                }
                i++;

            }
            i++;
            var listaMedicionStock = listaMedicion.Where(x => x.Lectcodi == ConstantesStockCombustibles.LectCodiStock).ToList();
            if (listaPtosStock.Count > 0)
                CargaStockCombustiblesEnMatriz(model, listaMedicionStock, listaCambios, cordX + i, cordY, listaEquipos, fechaIni);
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
        public void CargaStockCombustiblesEnMatriz(StockCombustibleFormatoModel model, List<MeMedicionxintervaloDTO> listaMedicion, List<MeCambioenvioDTO> listaCambios, int xCoor, int yCoor, List<MeHojaptomedDTO> listaPtoConsumo, DateTime fecha)
        {
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
            lista[cordX + i][cordY] = "CENTRAL";
            lista[cordX + i][cordY + 1] = "TIPO";
            lista[cordX + i][cordY + 2] = "INICIAL";
            lista[cordX + i][cordY + 3] = "RECEPCIÓN";
            lista[cordX + i][cordY + 4] = "CONSUMO TOTAL";
            lista[cordX + i][cordY + 5] = "FINAL";
            lista[cordX + i][cordY + 6] = "FINAL DECLARADO";
            lista[cordX + i][cordY + 7] = "OBSERVACIÓN";
            var elemento = new CeldaMerge() { col = cordY, row = cordX, colspan = 7, rowspan = 1 };
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
                filaExcel = ConstantesIntranet.FilaExcelData - 1;
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
                                .Select(y => y.First()).ToList();
            List<MeMedicionxintervaloDTO> listaStockIni = listaMedicion.Where(x => x.Medintfechaini == fecha.AddDays(-1)).ToList();
            List<MeMedicionxintervaloDTO> listaActual = listaMedicion.Where(x => x.Medintfechaini == fecha).ToList();
            foreach (var reg in listaEquipos)
            {
                lista[cordX + i][cordY] = reg.Equinomb;
                lista[cordX + i][cordY + 1] = reg.Tipoptomedinomb.Substring(19, reg.Tipoptomedinomb.Length - 19) + " - " + reg.Tipoinfoabrev;

                //var findStockIni = listaStockIni.Find(x => x.Equicodi == reg.Equicodi && ConstantesStockCombustibles.IdsTptoStock.Contains(x.Tptomedicodi) && x.Medintfechaini == fecha.AddDays(-1));
                var findStockIni = listaStockIni.Find(x => x.Equicodi == reg.Equicodi && x.Tptomedicodi == reg.Tptomedicodi && x.Medintfechaini == fecha.AddDays(-1));
                if (findStockIni != null)
                {
                    if (findStockIni.Medinth1 != null)
                        lista[cordX + i][cordY + 2] = ((decimal)findStockIni.Medinth1).ToString("0.00");
                }
                //Stock Fin
                //var findStockFin = listaMedicion.Find(x => x.Equicodi == reg.Equicodi && ConstantesStockCombustibles.IdsTptoStock.Contains(x.Tptomedicodi) && x.Medintfechaini == fecha);
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
                                lista[cordX + i][cordY + 6] = Util.StrNumeroFormato(lstValor[0]);
                                if (lstValor[0] != "")
                                {
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = cordX + i,
                                        Col = cordY + 6
                                    });
                                }
                                if (totCampos == 2)
                                {
                                    lista[cordX + i][cordY + 7] = lstValor[1];
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = cordX + i,
                                        Col = cordY + 7
                                    });
                                }
                                break;
                            default:
                                lista[cordX + i][cordY + 6] = Util.StrNumeroFormato(lstValor[0]);
                                if (lstValor[0] != "")
                                {
                                    model.ListaCambios.Add(new CeldaCambios()
                                    {
                                        Row = cordX + i,
                                        Col = cordY + 6
                                    });
                                }
                                for (var z = 1; z < totCampos; z++)
                                {
                                    lista[cordX + i][cordY + 7] += lstValor[z];
                                }
                                model.ListaCambios.Add(new CeldaCambios()
                                {
                                    Row = cordX + i,
                                    Col = cordY + 7
                                });

                                break;
                        }
                    }
                    else
                    {
                        if (findStockFin.Medinth1 != null)
                            lista[cordX + i][cordY + 6] = ((decimal)findStockFin.Medinth1).ToString("0.00");
                        lista[cordX + i][cordY + 7] = findStockFin.Medintdescrip;
                    }


                }
                int filaTotal = 0;
                lista[cordX + i][cordY + 4] = ObtieneFormulaConsumo(reg.Equicodi, reg.Tptomedicodi, listaPtoConsumo, ref filaTotal, model.IsExcelWeb);
                lista[cordX + i][cordY + 5] = "=C" + (cordX + filaExcel + i + 1).ToString() + " + D" + (cordX + filaExcel + i + 1).ToString() + " - E" + (cordX + filaExcel + i + 1).ToString();
                //if(filaTotal > 0)
                //    lista[filaTotal - 1][cordY + 5] = lista[cordX + i][cordY + 4];
                int index = codStock.FindIndex(x => x == reg.Tptomedicodi);
                //var findRepecion = listaActual.Find(x => x.Equicodi == reg.Equicodi && ConstantesStockCombustibles.IdsTptoRecepcion.Contains(x.Tptomedicodi));
                var findRepecion = listaActual.Find(x => x.Equicodi == reg.Equicodi && codRecepcion[index] == x.Tptomedicodi);
                if (findRepecion != null)
                {
                    var cambio = listaCambios.Find(y => y.Ptomedicodi == findRepecion.Ptomedicodi && y.Cambenvfecha == findRepecion.Medintfechaini);
                    if (cambio != null)
                    {
                        lista[cordX + i][cordY + 3] = Util.StrNumeroFormato(cambio.Cambenvdatos);
                        model.ListaCambios.Add(new CeldaCambios()
                        {
                            Row = cordX + i,
                            Col = cordY + 3
                        });
                    }
                    else
                    {
                        lista[cordX + i][cordY + 3] = ((decimal)findRepecion.Medinth1).ToString("0.00");
                    }
                }
                i++;
            }
        }

        /// <summary>
        /// Obtiene formula suma para los combustible de una central
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="Tptomedicodi"></param>
        /// <param name="listaPto"></param>
        /// <param name="iniFil"></param>
        /// <param name="isExcelWeb"></param>
        /// <returns></returns>
        private string ObtieneFormulaConsumo(int equicodi, int Tptomedicodi, List<MeHojaptomedDTO> listaPto, ref int iniFil, Boolean isExcelWeb)
        {
            string formulaCons = string.Empty;
            int i = 0;
            if (isExcelWeb)
            {
                i = 3;
            }
            else
            {
                i = ConstantesIntranet.FilaExcelData + 2;
            }
            bool inicio = true;
            int indice = Array.IndexOf(ConstantesStockCombustibles.IdsTptoStock.ToArray(), Tptomedicodi);
            foreach (var reg in listaPto)
            {
                if (equicodi == reg.Equipadre && ConstantesStockCombustibles.IdsTptoConsumo[indice] == reg.Tptomedicodi)
                {
                    if (inicio)
                    {
                        formulaCons += "=E" + i.ToString();// +" + F" + i.ToString();
                        iniFil = i;
                        inicio = false;
                    }
                    else
                        formulaCons += " + E" + i.ToString();// +" + F" + i.ToString();
                }
                i++;
            }

            return formulaCons;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormatoConsumo(int idEnvio)
        {
            int indicador = 0;
            string ruta = string.Empty;
            ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
            try
            {
                StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
                model.IsExcelWeb = false;
                BuildHojaExcelConsumo(model, idEnvio);
                Util.GenerarFileExcelConsumo(model, ruta + StockConsumoArchivo.ArchivoEnvio);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }


        #endregion

        #region Presion de Gas

        /// <summary>
        /// Devuelve el model con informacion de Presion de Gas
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public StockCombustibleFormatoModel BuildHojaExcelPresion(int idEnvio)
        {
            StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
            model.Handson = new HandsonModel();

            var envio = servFormato.GetByIdMeEnvio(idEnvio);
            int idEmpresa = (int)envio.Emprcodi;
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

            var entEmpresa = servFormato.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;

            int idCfgFormato = 0;
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
            if (idEnvio <= 0)
            {
                //model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, Constantes.FormatoFecha);
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
                model.Handson.ReadOnly = !ValidarFecha(model.Formato, idEmpresa);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = true;

                var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                Util.GetSizeFormato(model.Formato);
                model.EnPlazo = ValidarPlazo(model.Formato);
                listaCambios = servFormato.GetAllCambioEnvio(ConstantesStockCombustibles.IdFormatoPGas, model.Formato.FechaProceso, model.Formato.FechaProceso, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
            }
            model.Anho = model.Formato.FechaInicio.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
            model.Dia = model.Formato.FechaInicio.Day.ToString();
            model.ListaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa, ConstantesStockCombustibles.IdFormatoPGas, model.Formato.FechaInicio);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (model.ListaEnvios.Count > 0)
            {
                idUltEnvio = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
                if (reg != null)
                    model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(Constantes.FormatoFechaHora);
            }

            model.ListaHojaPto = servFormato.GetListaPtos(model.Formato.FechaProceso, idCfgFormato, idEmpresa, ConstantesStockCombustibles.IdFormatoPGas, cabercera.Cabquery);
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(150);
            for (var i = 0; i < model.ListaHojaPto.Count; i++)
            {
                model.Handson.ListaColWidth.Add(100);
            }


            var cabecerasRow = cabercera.Cabcampodef.Split(ConstantesIntranet.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(ConstantesIntranet.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }
            string fecha = model.Formato.FechaProceso.ToString(ConstantesBase.FormatoFecha);
            if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            {
                var lista = servFormato.GetDataFormato24(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                model.Handson.ListaExcelData = Util.ObtenerListaExcelDataPG(model, lista, listaCambios, fecha, idEnvio);
            }

            return model;
        }

        /// <summary>
        /// Permite generar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarFormatoPresion(int idEnvio)
        {
            int indicador = 0;
            string ruta = string.Empty;
            ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
            try
            {
                StockCombustibleFormatoModel model = new StockCombustibleFormatoModel();
                model.IsExcelWeb = false;
                model = BuildHojaExcelPresion(idEnvio);
                Util.GenerarFileExcelPresion(model, ruta + StockConsumoArchivo.ArchivoEnvio);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        #endregion

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuera de plazo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        protected bool ValidarPlazo(MeFormatoDTO formato)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            //resultado = true;
            return resultado;
        }

        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected bool ValidarFecha(MeFormatoDTO formato, int idEmpresa)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = servFormato.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
                if (regfechaPlazo != null) // si existe registro de ampliacion
                {

                    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= regfechaPlazo.Amplifechaplazo))
                    {
                        resultado = true;
                    }
                }
            }
            if ((formato.Formatdiaplazo == 0) && (resultado)) //Formato Tiempo Real
            {

            }
            return true;
            //return resultado;
        }

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {

            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
            string fullPath = ruta + StockConsumoArchivo.ArchivoEnvio;
            return File(fullPath, ConstantesIntranet.AppExcel, StockConsumoArchivo.ArchivoEnvio);
        }


    }
}
