using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.ValorizacionDiaria;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.ValorizacionDiaria.Helper;
using COES.Servicios.Aplicacion.ValorizacionDiaria.Helper;
using COES.Servicios.Aplicacion.General;

namespace COES.MVC.Intranet.Areas.ValorizacionDiaria.Controllers
{
    public class EnvioController : BaseController
    {
        #region Propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionNombreArchivo] != null) ?
                    Session[DatosSesionDemanda.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionDemanda.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionFileName] != null) ?
                    Session[DatosSesionDemanda.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesionDemanda.SesionFileName] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesionDemanda.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesionDemanda.SesionIdEnvio] = value; }
        }

        /// <summary>
        /// Nombre del formato
        /// </summary>
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[DatosSesionDemanda.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[DatosSesionDemanda.SesionFormato] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[DatosSesionDemanda.SesionMatrizExcel] != null) ?
                    (string[][])Session[DatosSesionDemanda.SesionMatrizExcel] : new string[1][];
            }
            set { Session[DatosSesionDemanda.SesionMatrizExcel] = value; }
        }

        /// <summary>
        /// Codigo de formato
        /// </summary>
        public int IdFormato = 4;
        public int IdLectura = 51;

        #endregion

        int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);

        public bool[] ListaPermisos
        {
            get
            {
                return (Session["ListaPermisos"] != null) ?
                    (bool[])Session["ListaPermisos"] : new bool[14];
            }
            set { Session["ListaPermisos"] = value; }
        }

        ValorizacionDiariaAppServicio servicio = new ValorizacionDiariaAppServicio();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        ContactoAppServicio servicioEmpr = new ContactoAppServicio();

        //
        // GET: /ValorizacionDiaria/Envio/

        public ActionResult Index()
        {
            FormatoModel model = new FormatoModel();
            model.IdModulo = 0;
            CargarFiltrosFormato(model);
            //Escoger fechas de acuerdo a vencimiento.
            DateTime fechaActual = DateTime.Now;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Editable = 1;
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            return View(model);
        }
        public ActionResult IndexExcelWeb(int idformato)
        {
            if (idformato == 101 || idformato == 102)
            {
                //if (!base.IsValidSesion) return base.RedirectToLogin();
                //if (this.IdModulo == null) return base.RedirectToHomeDefault();

                ///Llamar a funcion que carga informacion para los filtros de formato    
                FormatoModel model = new FormatoModel();
                string codigo = string.Empty;
                //this.IdModulo = Modulos.AppHidrologia;
                //model.IdModulo = this.IdModulo;
                model.IdArea = 0;
                CargarFiltrosFormato(model);
                //Escoger fechas de acuerdo a vencimiento.
                DateTime fechaActual = DateTime.Now;
                int nroSemana = EPDate.f_numerosemana(DateTime.Now);
                model.Editable = 1;
                model.NroSemana = nroSemana;
                model.Dia = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
                model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
                model.IdFormato = idformato;
                return View(model);
            }
            return View();
        }


        /// <summary>
        /// Actualiza la informacion necesaria para cargar los filtros para la eleccion de los formatos
        /// </summary>
        /// <param name="model"></param>
        private void CargarFiltrosFormato(FormatoModel model)
        {

            int idOrigen = 16; // ConstantesHidrologia.IdOrigenHidro;
            int idOpcion = (Session[DatosSesion.SesionIdOpcion] == null) ? 0 : (int)Session[DatosSesion.SesionIdOpcion];
            //Cargar Permisos
            this.ListaPermisos = new bool[14];
            //this.ListaPermisos[Permisos.Grabar] = this.seguridad.ValidarPermisoOpcion(Constantes.IdAplicacion, idOpcion, Permisos.Grabar, User.Identity.Name);
            this.ListaPermisos[Acciones.Grabar] = true;
            model.OpGrabar = this.ListaPermisos[Acciones.Grabar];
            model.OpEditar = this.ListaPermisos[Acciones.Editar];

            model.ListaEmpresas = servicio.ObtenerEmpresasMME();


            model.ListaAreas = logic.ListAreaXFormato(idOrigen);
            if ((model.IdArea == 0) && (model.ListaAreas.Count > 0))
                model.IdArea = model.ListaAreas[0].Areacode;
            model.ListaFormato = logic.ListMeFormatos().Where(x => x.Modcodi == 16).ToList(); //lista de todos los formatos para hidrologia
            //if (model.ListaFormato.Count > 0)
            //    model.IdFormato = model.ListaFormato[0].Formatcodi;
            model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen && x.Areacode == model.IdArea).ToList(); // lista de lectura

            if (model.ListaLectura.Count > 0 && (model.IdLectura == 0))
                model.IdLectura = model.ListaLectura[0].Lectcodi;
            if ((model.ListaEmpresas.Count > 0) && (model.IdEmpresa == 0))
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            model.ListaSemanas = semanas;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            string validacion = string.Empty;
            List<int> listaFormatCodi = new List<int>();
            List<int> listaFormatPeriodo = new List<int>();
            List<string> listastrFormatDescrip = new List<string>();
            foreach (var reg in model.ListaFormato)
            {
                listaFormatCodi.Add(reg.Formatcodi);
                listaFormatPeriodo.Add((int)reg.Formatperiodo);
                listastrFormatDescrip.Add(reg.Formatdescrip);
            }
            model.StrFormatCodi = String.Join(",", listaFormatCodi);
            model.StrFormatPeriodo = String.Join(",", listaFormatPeriodo);
            model.StrFormatDescrip = String.Join(",", listastrFormatDescrip);
            model.ListaFormato = logic.GetByModuloLecturaMeFormatos(16, model.IdLectura, model.IdEmpresa);
        }

        [HttpPost]
        public JsonResult ObtenerUltimoEnvio(int idEmpresa, string mes)
        {
            MeFormatoDTO formato = logic.GetByIdMeFormato(IdFormato);
            DateTime fecha = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, string.Empty, string.Empty, Constantes.FormatoFecha);
            List<MeEnvioDTO> list = this.logic.GetByCriteriaMeEnvios(idEmpresa, this.IdFormato, fecha);

            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            if (list.Count > 0)
            {
                idUltEnvio = list[list.Count - 1].Enviocodi;
            }
            return Json(idUltEnvio);
        }
        [HttpPost]
        public JsonResult MostrarHojaExcelWeb(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes, int verUltimoEnvio)
        {
            base.ValidarSesionUsuario();
            FormatoModel jsModel = new FormatoModel();
            switch (idFormato)
            {
                case 41:
                case 42:
                    jsModel = FormatoVariado(idEmpresa, idFormato, idEnvio, fecha, semana, mes);
                    break;
                default:
                    jsModel = BuildHojaExcel(idEmpresa, idFormato, idEnvio, fecha, semana, mes, verUltimoEnvio);
                    break;

            }

            return Json(jsModel);
        }
        public FormatoModel FormatoVariado(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes)
        {
            FormatoModel model = new FormatoModel();

            model.Formato = logic.GetByIdMeFormato(idFormato);
            model.Handson = new HandsonModel();
            //model.Handson.ListaFilaReadOnly = true;
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
            var cabecerasRow = cabercera.Cabcampodef.Split(QueryParametros.SeparadorFila);
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;
            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
            model.ListaHojaPto = logic.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabercera.Cabquery, model.Formato.FechaProceso, model.Formato.FechaProceso);
            model.ListaPtoMedicion = model.ListaHojaPto.Select(x => new ListaSelect
            {
                id = x.Ptomedicodi,
                text = x.Ptomedibarranomb
            }).ToList();
            model.Fecha = model.Formato.FechaProceso.ToString(Constantes.FormatoFecha);
            model.ListaEnvios = logic.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaProceso);

            model.EnPlazo = false;
            model.TipoPlazo = ConstantesEnvio.ENVIO_FUERA_PLAZO;

            var entEmpresa = logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
                model.Empresa = entEmpresa.Emprnomb;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Semana = semana;
            model.Dia = model.Formato.FechaProceso.Day.ToString();

            int nCol = cabercera.Cabcolumnas;
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(70);
            model.Handson.ListaColWidth.Add(260);
            model.Handson.ListaColWidth.Add(100);
            model.Handson.ListaColWidth.Add(100);
            model.Handson.ListaColWidth.Add(90);
            model.Handson.ListaColWidth.Add(320);
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }
            var listaData = logic.GetEnvioMedicionXIntervalo(model.Formato.Formatcodi, model.IdEmpresa, model.Formato.FechaProceso,
               model.Formato.FechaProceso.AddDays(1).AddSeconds(-1));
            int nBloques = listaData.Count;
            model.Handson.ListaExcelData = ValorizacionDiariaHelper.InicializaMatrizExcel2(listaCabeceraRow, model.Formato.Formatrows, nBloques, 0, nCol, model.Fecha);
            ValorizacionDiariaHelper.LoadMatrizExcel2(model.Handson.ListaExcelData, listaData, nCol);

            return model;
        }

        public FormatoModel BuildHojaExcel(int idEmpresa, int idFormato, int idEnvio, string fecha, string semana, string mes, int verUltimoEnvio)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaMerge = new List<CeldaMerge>();
            model.Handson.ListaColWidth = new List<int>();

            ////////// Obtiene el Fotmato ////////////////////////
            model.Formato = logic.GetByIdMeFormato(idFormato);
            this.Formato = model.Formato;
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();

            /// DEFINICION DEL FORMATO //////
            model.Formato.Formatcols = cabercera.Cabcolumnas;
            model.Formato.Formatrows = cabercera.Cabfilas;
            model.Formato.Formatheaderrow = cabercera.Cabcampodef;
            model.ColumnasCabecera = model.Formato.Formatcols;
            model.FilasCabecera = model.Formato.Formatrows;

            ///
            var entEmpresa = logic.GetByIdSiEmpresa(idEmpresa);
            if (entEmpresa != null)
            {
                model.Empresa = entEmpresa.Emprnomb;
                model.EsEmpresaVigente = true; // logic.EsEmpresaVigente(idEmpresa, DateTime.Now);
            }

            ///
            model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);

            //Mostrar último envio cuando se muestra la interfaz de Carga de datos de un formato
            int idUltEnvio = 0; //Si se esta consultando el ultimo envio se podra activar el boton editar
            model.ListaEnvios = logic.GetByCriteriaMeEnvios(idEmpresa, idFormato, model.Formato.FechaProceso);
            if (model.ListaEnvios.Count > 0)
            {
                model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
                idUltEnvio = model.IdEnvioLast;
                if (ConstantesFormato.VerUltimoEnvio == verUltimoEnvio)
                {
                    idEnvio = model.IdEnvioLast;
                }
            }

            int idCfgFormato = 0;
            model.Formato.IdEnvio = idEnvio;
            /// Verifica si Formato esta en Plaz0
            string mensaje = string.Empty;
            int horaini = 0;//Para Formato Tiempo Real
            int horafin = 0;//Para Formato Tiempo Real
            if (idEnvio <= 0)
            {
                model.Formato.Emprcodi = idEmpresa;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
                model.EnPlazo = true; // logic.ValidarPlazo(model.Formato);
                model.TipoPlazo = ConstantesEnvio.ENVIO_EN_PLAZO; // logic.EnvioValidarPlazo(model.Formato, idEmpresa);
                model.Handson.ReadOnly = false; // ConstantesEnvio.ENVIO_EN_PLAZO == model.TipoPlazo;

                ObtenerH24IniFinTR(model.Formato, !model.Handson.ReadOnly, out horaini, out horafin);
            }
            else  // Fecha proceso es obtenida del registro envio
            {
                model.Handson.ReadOnly = false;

                var envioAnt = logic.GetByIdMeEnvio(idEnvio);
                if (envioAnt != null)
                {
                    model.Formato.FechaEnvio = envioAnt.Enviofecha;
                    model.FechaEnvio = ((DateTime)envioAnt.Enviofecha).ToString(Constantes.FormatoFechaHora);
                    model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    if (envioAnt.Cfgenvcodi != null)
                    {
                        idCfgFormato = (int)envioAnt.Cfgenvcodi;
                    }
                    model.EnPlazo = envioAnt.Envioplazo == "P";
                }
                else
                    model.Formato.FechaProceso = DateTime.MinValue;
                FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
            }

            //
            model.ListaHojaPto = logic.GetListaPtos(model.Formato.FechaProceso, idCfgFormato, idEmpresa, idFormato, cabercera.Cabquery);
            //servHidro.ListarConfigPlazoXFormatoYFechaPeriodo(idFormato, model.ListaHojaPto, model.Formato.FechaProceso);

            model.OpGrabar = this.ListaPermisos[Acciones.Grabar];
            model.OpAccesoEmpresa = this.ListaPermisos[Acciones.AccesoEmpresa];
            model.OpEditar = this.ListaPermisos[Acciones.Editar];
            var cabecerasRow = cabercera.Cabcampodef.Split(QueryParametros.SeparadorFila);
            List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
            for (var x = 0; x < cabecerasRow.Length; x++)
            {
                var reg = new CabeceraRow();
                var fila = cabecerasRow[x].Split(QueryParametros.SeparadorCol);
                reg.NombreRow = fila[0];
                reg.TituloRow = fila[1];
                reg.IsMerge = int.Parse(fila[2]);
                listaCabeceraRow.Add(reg);
            }

            model.Editable = model.Handson.ReadOnly ? 1 : 0;
            model.IdEmpresa = idEmpresa;
            model.Anho = model.Formato.FechaProceso.Year.ToString();
            model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaProceso.Month);
            model.Semana = semana;
            model.Dia = model.Formato.FechaProceso.Day.ToString();
            model.Handson.Width = HandsonConstantes.ColWidth * ((model.ListaHojaPto.Count > HandsonConstantes.ColPorHoja) ? HandsonConstantes.ColPorHoja :
                (model.ListaHojaPto.Count + model.ColumnasCabecera));
            //Genera La vista html complementaria a la grilla Handson, nombre de formato, area coes, fecha de formato, etc.
            model.ViewHtml = ValorizacionDiariaHelper.GenerarFormatoHtml(model, idEnvio, model.EnPlazo);

            List<object> lista = new List<object>(); /// Contiene los valores traidos de de BD del envio seleccionado.
            List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>(); /// contiene los cambios de que ha habido en el envio que se esta consultando.
            int nCol = model.ListaHojaPto.Count;
            int nBloques = model.Formato.Formathorizonte * model.Formato.RowPorDia; // SE DEFINE HORIZONTE (NRO DE DIAS)
            model.Handson.ListaFilaReadOnly = ValorizacionDiariaHelper.InicializaListaFilaReadOnly(model.Formato.Formatrows, nBloques);
            if (model.Formato.Formatresolucion == ParametrosFormato.ResolucionHora && model.Formato.Formatperiodo == ParametrosFormato.PeriodoDiario && model.Formato.Formatdiaplazo == 0)
                model.Handson.ListaFilaReadOnly = CargarListaFilaReadOnly(model.Formato.FechaProceso, idEmpresa, idFormato, model.Formato.Formatrows, nBloques, model.EnPlazo);


            //model.Handson.ListaFilaReadOnly = CargarListaFilaReadOnly(model.Formato.FechaProceso, idEmpresa, idFormato, model.Formato.Formatrows, nBloques, model.EnPlazo);

            model.ListaCambios = new List<CeldaCambios>();
            model.Handson.ListaExcelData = ValorizacionDiariaHelper.InicializaMatrizExcel(model.Formato.Formatrows, nBloques, model.Formato.Formatcols, nCol);
            if (idEnvio == 0)
            {
                lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                ValorizacionDiariaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            if (idEnvio > 0)
            {
                lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                listaCambios = logic.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
                ValorizacionDiariaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }
            if (idEnvio < 0)
            {
                model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
                ValorizacionDiariaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            }

            //if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
            //{
            //    lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
            //    if (idEnvio > 0) //Si se esta consultando un envio anterior se obtienen los cambios de ese envio.
            //        listaCambios = logic.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
            //    //if (idEnvio == 0)
            //    //    listaCambios = this.logic.GetAllOrigenEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, model.Formato.FechaProceso, idEmpresa);

            //    /// Cargar Datos en Arreglo para Web
            //    HidrologiaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            //}
            //else
            //{ // los datos para visualizar en el excel web provienen de un archivo excel cargado por el usuario
            //    //Carga de archivo Excel
            //    model.Handson.ListaExcelData = this.MatrizExcel; /// Data del excel cargado previamente se ha guardado en una variable session
            //    HidrologiaHelper.ObtieneMatrizWebExcel(model, lista, listaCambios, idEnvio);
            //}

            #region Filas Cabeceras

            for (var ind = 0; ind < model.ColumnasCabecera; ind++)
            {
                model.Handson.ListaColWidth.Add(120);
            }
            string sTitulo = string.Empty;
            string sTituloAnt = string.Empty;
            int column = model.ColumnasCabecera;
            var cellMerge = new CeldaMerge();
            foreach (var reg in model.ListaHojaPto)
            {
                model.Handson.ListaColWidth.Add(100);
                for (var w = 0; w < model.FilasCabecera; w++)
                {
                    if (column == model.ColumnasCabecera)
                    {
                        model.Handson.ListaExcelData[w] = new string[model.ListaHojaPto.Count + model.ColumnasCabecera];
                        model.Handson.ListaExcelData[w][0] = listaCabeceraRow[w].TituloRow;
                    }
                    model.Handson.ListaExcelData[w][column] = (string)reg.GetType().GetProperty(listaCabeceraRow[w].NombreRow).GetValue(reg, null);
                    if (listaCabeceraRow[w].IsMerge == 1)
                    {
                        if (listaCabeceraRow[w].TituloRowAnt != model.Handson.ListaExcelData[w][column])
                        {
                            if (column != model.ColumnasCabecera)
                            {
                                if ((column - listaCabeceraRow[w].ColumnIni) > 1)
                                {
                                    cellMerge = new CeldaMerge();
                                    cellMerge.col = listaCabeceraRow[w].ColumnIni;
                                    cellMerge.row = w;
                                    cellMerge.colspan = (column - listaCabeceraRow[w].ColumnIni);
                                    cellMerge.rowspan = 1;
                                    model.Handson.ListaMerge.Add(cellMerge);
                                }
                            }
                            listaCabeceraRow[w].TituloRowAnt = model.Handson.ListaExcelData[w][column];
                            listaCabeceraRow[w].ColumnIni = column;
                        }
                    }
                }
                column++;

            }
            if ((column - 1) != model.ColumnasCabecera)
            {
                for (var i = 0; i < listaCabeceraRow.Count; i++)
                {
                    if ((listaCabeceraRow[i].TituloRowAnt == model.Handson.ListaExcelData[i][column - 1]))
                    {
                        if ((column - listaCabeceraRow[i].ColumnIni) > 1)
                        {
                            cellMerge = new CeldaMerge();
                            cellMerge.col = listaCabeceraRow[i].ColumnIni;
                            cellMerge.row = i;
                            cellMerge.colspan = (column - listaCabeceraRow[i].ColumnIni);
                            cellMerge.rowspan = 1;
                            model.Handson.ListaMerge.Add(cellMerge);
                        }
                    }
                }
            }

            #endregion

            model.IdEnvio = idEnvio;
            return model;
        }

        protected void ObtenerH24IniFinTR(MeFormatoDTO formato, bool enPlazo, out int horaini, out int horafin)
        {
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;

            if ((formato.Formatdiaplazo == 0) && (enPlazo)) //Formato Tiempo Real
            {
                int hora = fechaActual.Hour;

                if (hora < 2)
                {
                    horaini = 0;
                }
                else
                {
                    horaini = hora - 2;
                }

                horafin = 24;
            }
        }

        protected List<bool> CargarListaFilaReadOnly(DateTime fechaProceso, int idEmpresa, int formatcodi, int filHead, int filData, bool plazo)
        {
            List<bool> lista = new List<bool>();
            int horaini = 0;
            int horafin = -1;

            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fechaActual2 = DateTime.Now;
            int dif = (fechaActual - fechaProceso).Days;
            int hora = fechaActual2.Hour;
            if (dif == 0)
            {

                int periodo = hora / ConstantesValorizacionDiarias.BandaTR;
                horaini = periodo * ConstantesValorizacionDiarias.BandaTR;
                horafin = horaini + ConstantesValorizacionDiarias.BandaTR - 1;
            }
            if (dif == 1)
            {
                if (hora < ConstantesValorizacionDiarias.BandaTR - 1)
                {
                    horafin = ConstantesValorizacionDiarias.BandaTR - 1;
                }
            }

            for (int i = 0; i < filHead; i++)
            {
                lista.Add(true);
            }


            for (int i = 0; i < filData; i++)
            {
                if (i >= horaini && i <= horafin)
                    lista.Add(false);
                else
                    lista.Add(true);

            }
            return lista;
        }

        /// <summary>
        /// Verifica si un formato enviado esta en plazo o fuyera de plazo
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
            return resultado;
        }
        [HttpPost]
        public JsonResult GrabarExcelWeb(string dataExcel, int idFormato, int idEmpresa, string fecha, string semana, string mes)
        {
            base.ValidarSesionUsuario();
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            int exito = 0;
            List<string> celdas = new List<string>();
            celdas = dataExcel.Split(',').ToList();
            string empresa = string.Empty;
            var regEmp = logic.GetByIdSiEmpresa(idEmpresa); ;
            //////////////////////////////////////////////////
            if (regEmp != null)
            {
                empresa = regEmp.Emprnomb;
                //if (!this.logic.EsEmpresaVigente(idEmpresa, DateTime.Now))
                //{
                //    return Json(model);
                //}
            }

            MeFormatoDTO formato = logic.GetByIdMeFormato(idFormato);
            var cabercera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            formato.Formatcols = cabercera.Cabcolumnas;
            formato.Formatrows = cabercera.Cabfilas;
            formato.Formatheaderrow = cabercera.Cabcampodef;

            /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
            formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
            FormatoMedicionAppServicio.GetSizeFormato(formato);

            int filaHead = formato.Formatrows;
            int colHead = formato.Formatcols;
            var listaPto = logic.GetByCriteriaMeHojaptomeds(idEmpresa, idFormato, formato.FechaInicio, formato.FechaFin);
            int nPtos = listaPto.Count();

            //servHidro.ListarConfigPlazoXFormatoYFechaPeriodo(idFormato, listaPto, formato.FechaProceso);

            formato.Emprcodi = idEmpresa;
            string tipoPlazo = ConstantesEnvio.ENVIO_EN_PLAZO; // logic.EnvioValidarPlazo(formato, idEmpresa);
            if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == tipoPlazo)
                throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + formato.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + formato.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

            /////////////// Grabar Config Formato Envio //////////////////
            MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
            config.Formatcodi = idFormato;
            config.Emprcodi = idEmpresa;
            int idConfig = logic.GrabarConfigFormatEnvio(config);
            ///////////////Grabar Envio//////////////////////////
            string mensajePlazo = string.Empty;
            Boolean enPlazo = logic.ValidarPlazo(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);
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
            this.IdEnvio = logic.SaveMeEnvio(envio);
            model.IdEnvio = this.IdEnvio;
            ///////////////////////////////////////////////////////
            int horizonte = formato.Formathorizonte;
            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    int total = (nPtos + formato.Formatcols) * (filaHead + 96 * formato.Formathorizonte);
                    int totalRecibido = celdas.Count;

                    var lista96 = ValorizacionDiariaHelper.LeerExcelWeb96(celdas, listaPto, formato.Lectcodi, colHead, nPtos, filaHead, 24 * 4 * formato.Formathorizonte, formato.Formatcheckblanco);
                    if (lista96.Count > 0)
                    {
                        try
                        {
                            logic.GrabarValoresCargados96(lista96, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                            envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                            envio.Enviocodi = this.IdEnvio;
                            logic.UpdateMeEnvio(envio);
                            exito = 1;
                            model.Mensaje = MensajesValorizacionDiaria.MensajeEnvioExito;
                        }
                        catch (Exception ex)
                        {
                            exito = -1;
                            model.Resultado = -1;
                        }
                    }
                    else
                    {
                        exito = -2;
                        model.Resultado = -2;
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    try
                    {
                        var lista48 = ValorizacionDiariaHelper.LeerExcelWeb48(celdas, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * 2 * formato.Formathorizonte, formato.Formatcheckblanco);
                        logic.GrabarValoresCargados48(lista48, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logic.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesValorizacionDiaria.MensajeEnvioExito;
                    }
                    catch (Exception ex)
                    {
                        exito = -1;
                        model.Resultado = -1;
                    }
                    break;
                case ParametrosFormato.ResolucionHora:
                    try
                    {
                        var lista24 = ValorizacionDiariaHelper.LeerExcelWeb24(celdas, listaPto, formato.Lectcodi, colHead, nPtos, filaHead, 24 * formato.Formathorizonte);
                        logic.GrabarValoresCargados24(lista24, User.Identity.Name, this.IdEnvio, idEmpresa, formato);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logic.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesValorizacionDiaria.MensajeEnvioExito;
                    }
                    catch (Exception ex)
                    {
                        exito = -1;
                        model.Resultado = -1;
                    }
                    break;
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionMes:
                case ParametrosFormato.ResolucionSemana:
                    try
                    {
                        var lista1 = ValorizacionDiariaHelper.LeerExcelWeb1(celdas, listaPto, formato.Lectcodi, (int)formato.Formatperiodo, colHead, nPtos, filaHead, formato.Formathorizonte);
                        logic.GrabarValoresCargados1(lista1, User.Identity.Name, this.IdEnvio, idEmpresa, formato, formato.Lectcodi);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = this.IdEnvio;
                        logic.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                    }
                    catch (Exception ex)
                    {
                        exito = -1;
                        model.Resultado = -1;
                    }
                    break;
            }

            model.Resultado = exito;
            //Enviar Correo de exito de envio
            var lectura = logic.GetByIdMeLectura(formato.Lectcodi);
            string stLectura = string.Empty;
            if (lectura != null)
                stLectura = lectura.Lectnomb;
            ValorizacionDiariaHelper.EnviarCorreo(stLectura, formato.Formatnombre, enPlazo, empresa, formato.FechaInicio, formato.FechaFin, formato.Areaname, User.Identity.Name, (DateTime)envio.Enviofecha, envio.Enviocodi);
            return Json(model);
        }
        [HttpPost]
        public JsonResult GenerarFormato(int idEmpresa, string desEmpresa, int idFormato, string fecha, string semana, string mes)
        {
            int indicador = 0;
            int idEnvio = 0;

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesValorizacionDiaria.FolderReporte;
            try
            {
                FormatoModel model = BuildHojaExcel(idEmpresa, idFormato, idEnvio, fecha, semana, mes, ConstantesFormato.VerUltimoEnvio);
                ValorizacionDiariaHelper.GenerarFileExcel(model, ruta);
                indicador = 1;

            }

            catch (Exception ex)
            {
                indicador = -1;
            }
            return Json(indicador);

        }

        public JsonResult LeerFileUpExcel(int idEmpresa, string fecha, string semana, string mes, int idFormato)
        {
            //Leer Lista de Puntos
            // Leer  Formato
            //Definir Matriz de Puntos 
            //Recorrer Excel y llenar Matriz de Puntos
            //Verificar Formato

            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();

            int retorno = ValorizacionDiariaHelper.VerificarIdsFormato(this.NombreFile, idEmpresa, idFormato);

            if (retorno > 0)
            {
                MeFormatoDTO formato = logic.GetByIdMeFormato(idFormato);
                DateTime fechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, semana, fecha, Constantes.FormatoFecha);
                formato.FechaProceso = fechaProceso;
                var cabecera = logic.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                var cabecerasRow = cabecera.Cabcampodef.Split(QueryParametros.SeparadorFila);
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                FormatoMedicionAppServicio.GetSizeFormato(formato);
                var listaPtos = logic.GetByCriteria2MeHojaptomeds(idEmpresa, idFormato, cabecera.Cabquery, formato.FechaInicio, formato.FechaFin);
                int nCol = listaPtos.Count;
                int horizonte = formato.Formathorizonte;
                int nBloques = formato.RowPorDia * formato.Formathorizonte;


                if (formato.Formatresolucion == ParametrosFormato.ResolucionHora && formato.Formatperiodo == ParametrosFormato.PeriodoDiario && formato.Formatdiaplazo == 0)
                {
                    lista24 = logic.GetDataAnt(formato.Formatcodi, idEmpresa, formato.FechaInicio, formato.FechaFin);


                    //   lista = logic.GetDataFormato(idEmpresa, model.Formato, idEnvio, idUltEnvio);
                    List<bool> listaRead = CargarListaFilaReadOnly(formato.FechaProceso, idEmpresa, idFormato, formato.Formatrows, nBloques, true);
                    this.MatrizExcel = ValorizacionDiariaHelper.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                    Boolean isValido = ValorizacionDiariaHelper.LeerExcelFile2(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol, listaRead, lista24, listaPtos);
                }
                else
                {
                    this.MatrizExcel = ValorizacionDiariaHelper.InicializaMatrizExcel(formato.Formatrows, nBloques, formato.Formatcols, nCol);
                    Boolean isValido = ValorizacionDiariaHelper.LeerExcelFile(this.MatrizExcel, this.NombreFile, formato.Formatrows, nBloques, formato.Formatcols, nCol);
                }
            }
            //Borrar Archivo
            ValorizacionDiariaHelper.BorrarArchivo(this.NombreFile);
            return Json(retorno);



        }
        public ActionResult Upload()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + "." + NombreArchivoValorizacionDiaria.ExtensionFileUploadValorizacionDiaria;
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesValorizacionDiarias.FolderUpload;
                    string fileName = ruta + this.FileName;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesValorizacionDiaria.FolderReporte;
            string fullPath = ruta + this.Formato.Formatnombre + ".xlsx";
            return File(fullPath, Constantes.AppExcel, this.Formato.Formatnombre + ".xlsx");
        }


        /// <summary>
        /// Graba datos para formato tipo 2
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        /// <summary>
        /// Valida la fecha
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="horaini"></param>
        /// <param name="horafin"></param>
        /// <returns></returns>
        protected bool ValidarFecha(MeFormatoDTO formato, int idEmpresa, out int horaini, out int horafin)
        {
            bool resultado = false;
            DateTime fechaActual = DateTime.Now;
            horaini = 0;
            horafin = 0;
            if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
            {
                resultado = true;
            }
            else
            {
                var regfechaPlazo = this.logic.GetByIdMeAmpliacionfecha(formato.FechaProceso, idEmpresa, formato.Formatcodi);
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
                int hora = fechaActual.Hour;
                if (((hora - 1) % 3) == 0)
                {
                    horaini = hora - 1 - 1 * 3;
                    horafin = hora - 1;
                }
                else
                {
                    horafin = -1;//indica que formato tiempo real no tiene filas habilitadas
                    resultado = false;
                }
            }
            return true;
            //return resultado;
        }




    }
}
