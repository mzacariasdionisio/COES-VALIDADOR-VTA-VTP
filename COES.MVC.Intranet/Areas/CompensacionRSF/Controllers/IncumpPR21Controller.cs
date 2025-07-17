using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.CompensacionRSF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CompensacionRSF;
using COES.Servicios.Aplicacion.CompensacionRSF.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.SistemasTransmision.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF.Controllers
{
    public class IncumpPR21Controller : BaseController
    {
        // GET: /CompensacionRSF/IncumpPR21/

        public IncumpPR21Controller()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        CompensacionRSFAppServicio servicioCompensacionRSF = new CompensacionRSFAppServicio();
        CentralGeneracionAppServicio servicioEquipo = new CentralGeneracionAppServicio();

        #region Metodos Generales

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            IncumpPR21Model model = new IncumpPR21Model();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult Lista()
        {
            IncumpPR21Model model = new IncumpPR21Model();
            Log.Info("ListaIncumpPR21 - ListVcrVersionincplsIndex");
            model.ListaIncumpPR21 = this.servicioCompensacionRSF.ListVcrVersionincplsIndex();
            model.bEditar = base.VerificarAccesoAccion(Acciones.Editar, User.Identity.Name);
            model.bEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, User.Identity.Name);
            return PartialView(model);
        }

        public ActionResult New()
        {
            base.ValidarSesionUsuario();
            IncumpPR21Model model = new IncumpPR21Model();
            model.EntidadPeriodo = new PeriodoDTO();
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            model.EntidadIncumpPR21 = new VcrVersionincplDTO();

            if (model.EntidadIncumpPR21 == null)
            {
                return HttpNotFound();
            }
            model.EntidadIncumpPR21.Vcrinccodi = 0;
            model.EntidadIncumpPR21.Pericodi = 0;
            model.Vcrincfeccreacion = System.DateTime.Now.ToString("dd/MM/yyyy");
            model.EntidadIncumpPR21.Vcrincestado = "Abierto";
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="pericodi">Código del periodo</param>
        /// <param name="Vcrinccodi">Código de la Versión Incpl PR21</param>
        /// <returns></returns>
        public ActionResult Edit(int pericodi = 0, int vcrinccodi = 0)
        {
            base.ValidarSesionUsuario();
            IncumpPR21Model model = new IncumpPR21Model();
            model.EntidadIncumpPR21 = this.servicioCompensacionRSF.GetByIdVcrVersionincplEdit(vcrinccodi, pericodi);
            if (model.EntidadIncumpPR21 == null)
            {
                return HttpNotFound();
            }
            Log.Info("Lista de Periodos - ListPeriodo");
            model.ListaPeriodos = this.servicioPeriodo.ListPeriodo();
            Log.Info("ListaIncumpPR21 - ListVcrIncpl");
            model.ListaIncumpPR21 = this.servicioCompensacionRSF.ListVcrIncpl(pericodi);
            model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(IncumpPR21Model modelo)
        {
            base.ValidarSesionUsuario();
            Log.Info("ListaIncumpPR21 - ListVcrIncpl");
            modelo.ListaIncumpPR21 = this.servicioCompensacionRSF.ListVcrIncpl(modelo.EntidadIncumpPR21.Vcrinccodi);
            if (modelo.EntidadIncumpPR21.Vcrinccodi == 0)
            {
                foreach (var item in modelo.ListaIncumpPR21)
                {
                    if (modelo.EntidadIncumpPR21.Vcrincnombre == item.Vcrincnombre)
                    {
                        modelo.ListaIncumpPR21 = (new CompensacionRSFAppServicio()).ListVcrVersionincplsIndex();
                        modelo.sError = "El nombre de la version seleccionada ya se encuentra registrada";
                        modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                        return PartialView(modelo);

                    }
                }
            }
            if (ModelState.IsValid)
            {
                modelo.EntidadIncumpPR21.Vcrincusucreacion = User.Identity.Name;
                modelo.EntidadIncumpPR21.Vcrincfeccreacion = DateTime.Now;
                modelo.EntidadIncumpPR21.Vcrincusumodificacion = User.Identity.Name;
                modelo.EntidadIncumpPR21.Vcrincfecmodificacion = DateTime.Now;

                if (modelo.EntidadIncumpPR21.Vcrinccodi == 0)
                {
                    Log.Info("Insertar registro - SaveVcrVersionincpl");
                    this.servicioCompensacionRSF.SaveVcrVersionincpl(modelo.EntidadIncumpPR21);
                }
                else
                {
                    Log.Info("Actualizar registro - UpdateVcrVersionincpl");
                    this.servicioCompensacionRSF.UpdateVcrVersionincpl(modelo.EntidadIncumpPR21);
                }
                TempData["sMensajeExito"] = "La información ha sido correctamente registrada";
                return new RedirectResult(Url.Action("Index", "IncumpPR21"));
            }
            modelo.ListaIncumpPR21 = (new CompensacionRSFAppServicio()).ListVcrVersionincplsIndex();
            modelo.sError = "Se ha producido un error al insertar la información";
            modelo.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            return PartialView(modelo);
        }

        /// <summary>
        /// Muestra un registro 
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        public ActionResult View(int vcrinccodi = 0, int pericodi = 0)
        {
            IncumpPR21Model model = new IncumpPR21Model();
            Log.Info("EntidadIncumpPR21 - GetByIdVcrIncplView");
            model.EntidadIncumpPR21 = this.servicioCompensacionRSF.GetByIdVcrIncplView(vcrinccodi, pericodi);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar un registro de forma definitiva en la base de datos
        /// </summary>
        /// <param name="pericodi">Código del Mes de valorización</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public string Delete(int vcrinccodi = 0)
        {
            base.ValidarSesionUsuario();
            Log.Info("Eliminar registro - DeleteVcrVersionincpl");
            this.servicioCompensacionRSF.DeleteVcrVersionincpl(vcrinccodi);
            return "true";
        }

        public ActionResult Incumplimiento(int pericodi = 0, int vcrinccodi = 0)
        {
            IncumpPR21Model model = new IncumpPR21Model();
            Log.Info("EntidadIncumpPR21 - GetByIdVcrVersionincpl");
            model.EntidadIncumpPR21 = this.servicioCompensacionRSF.GetByIdVcrVersionincpl(vcrinccodi);
            Log.Info("EntidadPeriodo - GetByIdPeriodo");
            model.EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
            model.vcrinccodi = vcrinccodi;
            model.Pericodi = pericodi;
            return View(model);
        }

        #endregion

        #region GRILLA INCUMPLIMIENTOPR21

        /// <summary>
        /// Muestra la grilla excel con los registros de IncumplimientoPR-21
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelPR21(int pericodi = 0, int vcrinccodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            IncumpPR21Model modelin = new IncumpPR21Model();

            //obtener los valores de la tabla detalle de PR-21
            Log.Info("ListaDetallePR-21 - GetByCriteriaVcrVerincumplims");
            modelin.ListaDetalle = this.servicioCompensacionRSF.GetByCriteriaVcrVerincumplims(vcrinccodi);
            //obtenre la lista para Central
            Log.Info("ListaCentral - ListCentralGeneracion");
            var ListaCentral = this.servicioEquipo.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
            //obtenre la lista para Unidad
            Log.Info("ListaUnidad - ListUnidad");
            var ListaUnidad = this.servicioEquipo.ListUnidad().Select(x => x.CentGeneNombre).ToList();

            #region Armando de contenido

            PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            string sMes = EntidadPeriodo.MesCodi.ToString();
            if (sMes.Length == 1) sMes = "0" + sMes;
            var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

            DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
            DateTime dFecFin = dFecInicio.AddMonths(1);

            var listaFechas = new List<DateTime>();

            for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
            {
                listaFechas.Add(dt);
            }
            int iNroColumnas = listaFechas.Count() * 2; //2 columnas por Numero de dias

            //Definimos la cabecera como una metriz
            string[] Cabecera1 = { "CENTRAL", "UNIDAD", "CÓDIGO RPF" }; //Titulos de cada columna
            string[] Cabecera2 = { "", "", "" };
            int[] widths = { 150, 150, 150 }; //Ancho de cada columna
            string[] itemDato = { "", "", "" };

            //Lista Dinámica
            if (iNroColumnas > 0)
            {
                Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                Array.Resize(ref widths, widths.Length + iNroColumnas);
                Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
            }

            //Formato de colummnas
            object[] columnas = new object[iNroColumnas + 3];
            columnas[0] = new
            {   //CENTRAL
                type = GridExcelModel.TipoLista,
                source = ListaCentral.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[1] = new
            {   //UNIDAD
                type = GridExcelModel.TipoLista,
                source = ListaUnidad.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[2] = new
            {   //CÓDIGO RPF
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            int iAux = 3;
            foreach (DateTime item in listaFechas)
            {
                Cabecera1[iAux] = item.ToString("dd/MM/yyyy");
                Cabecera1[iAux + 1] = "";
                Cabecera2[iAux] = "Incumplimiento";
                Cabecera2[iAux + 1] = "Observación";
                widths[iAux] = widths[iAux + 1] = 100;
                itemDato[iAux] = itemDato[iAux + 1] = "";
                columnas[iAux] = new
                {   //Incumplimiento
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false,
                };
                columnas[iAux + 1] = new
                {   //Observacion
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                iAux = iAux + 2;
            }

            //Se arma la matriz de datos
            string[][] data;
            int index = 2;
            if (modelin.ListaDetalle.Count() != 0)
            {
                data = new string[modelin.ListaDetalle.Count() + 2][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                foreach (var item in modelin.ListaDetalle)
                {
                    string[] itemDato2 = { };
                    Array.Resize(ref itemDato2, 3 + iNroColumnas);
                    int iAux2 = 3;
                    itemDato2[0] = item.CentralNombre.ToString();
                    itemDato2[1] = item.UniNombre.ToString();
                    itemDato2[2] = item.Vcrvincodrpf.ToString();
                    List<VcrVerincumplimDTO> listVerincumplim = this.servicioCompensacionRSF.ListVcrVerincumplims(vcrinccodi, item.Equicodicen, item.Equicodiuni);
                    foreach (var dto in listVerincumplim)
                    {
                        if (iAux2 > (iNroColumnas + 2))
                            break;
                        itemDato2[iAux2++] = dto.Vcrvincumpli.ToString();
                        itemDato2[iAux2] = "";
                        if (dto.Vcrvinobserv != null)
                            itemDato2[iAux2] = dto.Vcrvinobserv.ToString();
                        iAux2++;
                    }
                    data[index] = itemDato2;
                    index++;
                }
            }
            else
            {
                data = new string[3][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                data[index] = itemDato;
            }


            #endregion

            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
            model.FixedColumnsLeft = 1;
            model.FixedRowsTop = 2;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelPR21(int pericodi, int vcrinccodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            IncumpPR21Model model = new IncumpPR21Model();
            VcrRecalculoDTO objIncumplimiento = this.servicioCompensacionRSF.GetByIncumplimiento(vcrinccodi);
            int svcrinccodi = (int)objIncumplimiento.Vcrecacodi;// (int)this.servicioCompensacionRSF.GetByIncumplimiento(vcrinccodi).Vcrinccodi;vcrecacodi
            //int svcrinccodi = vcrinccodi - 1;
            try
            {
                //////////////Eliminando datos////////
                Log.Info("Eliminando datos - DeleteVcrVerincumplim");
                this.servicioCompensacionRSF.DeleteVcrVerincumplim(vcrinccodi);
                Log.Info("EntidadPeriodo - GetByIdPeriodo");
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1);

                var listaFechas = new List<DateTime>();

                for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
                {
                    listaFechas.Add(dt);
                }
                int iNumFechas = listaFechas.Count; //Numero de dias
                //INSERTAR EL REGISTRO
                model.EntidadDetalle = new VcrVerincumplimDTO();
                model.EntidadDetalle.Vcrvincodi = 0;
                model.EntidadDetalle.Vcrinccodi = vcrinccodi; //mes actual
                model.EntidadDetalle.Vcrvinusucreacion = User.Identity.Name;
                model.EntidadDetalle.Vcrvinfeccreacion = DateTime.Now;
                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 2; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN  
                    if (!datos[f][0].Equals(""))
                    {
                        string Central = Convert.ToString(datos[f][0]);
                        string Unidad = Convert.ToString(datos[f][1]);
                        int CodigoRPF = 0;
                        if (datos[f][2] != null && datos[f][2].ToString() != "")
                        {
                            CodigoRPF = Convert.ToInt32(UtilCompensacionRSF.ValidarNumero(datos[f][2]));
                        }
                        if (Central.Equals(Unidad))
                        {
                            EqEquipoDTO dtoCentral = this.servicioEquipo.GetByCentGeneNombreEquipoCenUni(Central, svcrinccodi);
                            if (dtoCentral == null)
                            {
                                model.sError = "No existe la Central/Unidad " + Central;
                                return Json(model);
                            }
                            model.idCentral = dtoCentral.Equicodi;
                            model.idUnidad = model.idCentral;
                        }
                        else
                        {
                            Log.Info("EntidadCentral - GetByCentGeneNomb");
                            //CentralGeneracionDTO dtoCentral = this.servicioEquipo.GetByCentGeneNomb(Central);
                            EqEquipoDTO dtoCentral = this.servicioEquipo.GetByCentGeneNombreEquipo(Central, svcrinccodi);
                            if (dtoCentral == null)
                            {
                                model.sError = "No existe la Central " + Central;
                                return Json(model);
                            }
                            model.idCentral = dtoCentral.Equicodi;
                            if (model.idCentral == -1)
                            {
                                continue;
                            }
                            Log.Info("EntidadUnidad - GetByCentGeneNomb");
                            EqEquipoDTO dtoUnidad = this.servicioEquipo.GetByCentGeneUniNombreEquipo(Unidad, (int)dtoCentral.Equicodi, svcrinccodi);
                            if (dtoUnidad == null)
                            {
                                model.sError = "No existe la Unidad " + Unidad + " de la Central " + Central;
                                return Json(model);
                            }
                            model.idUnidad = dtoUnidad.Equicodi;
                        }

                        int aux = 0;
                        //Preparamos la data para registrar los detalles
                        for (int c = 3; c < (iNumFechas * 2) + 2; c = c + 2)
                        {
                            model.EntidadDetalle.Vcrvinfecha = listaFechas[aux++];
                            model.EntidadDetalle.Equicodicen = model.idCentral;
                            model.EntidadDetalle.Equicodiuni = model.idUnidad;
                            model.EntidadDetalle.Vcrvincodrpf = CodigoRPF;
                            //Incumplimiento
                            model.EntidadDetalle.Vcrvincumpli = 0;
                            if (datos[f][c] != null || datos[f][c].ToString() != "")
                            {
                                model.EntidadDetalle.Vcrvincumpli = UtilSistemasTransmision.ValidarNumero(datos[f][c].ToString());
                            }

                            //observacion
                            model.EntidadDetalle.Vcrvinobserv = "-";
                            if (datos[f][c + 1] != null || datos[f][c + 1].ToString() != "")
                            {
                                model.EntidadDetalle.Vcrvinobserv = Convert.ToString(datos[f][c + 1].ToString());
                            }
                            Log.Info("Insertar registro - SaveVcrVerincumplim");
                            this.servicioCompensacionRSF.SaveVcrVerincumplim(model.EntidadDetalle);
                        }
                    }

                    else
                    {
                        continue;
                    }
                }
                //INSERTAMOS vcr_verincumplim; PARA EQ_EQUIPO = -1
                model.EntidadDetalle.Equicodicen = -1;
                model.EntidadDetalle.Equicodiuni = -1;
                model.EntidadDetalle.Vcrvincodrpf = 0;
                model.EntidadDetalle.Vcrvincumpli = 0;
                model.EntidadDetalle.Vcrvinobserv = "-";
                for (int c = 0; c < iNumFechas; c++)
                {
                    model.EntidadDetalle.Vcrvinfecha = listaFechas[c];
                    this.servicioCompensacionRSF.SaveVcrVerincumplim(model.EntidadDetalle);
                }
                //-----------------------------------------
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros de la versión 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosPR21(int pericodi, int vcrinccodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                Log.Info("Eliminando datos - DeleteVcrVerincumplim");
                this.servicioCompensacionRSF.DeleteVcrVerincumplim(vcrinccodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }


        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataPR21(int pericodi, int vcrinccodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportando información - GenerarFormatoVcrPr21");
                string file = this.servicioCompensacionRSF.GenerarFormatoVcrPr21(vcrinccodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoPR21(string sarchivo, int pericodi, int vcrinccodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {
                //obtenre la lista para Central
                Log.Info("ListaCentral - ListCentralGeneracion");
                var ListaCentral = this.servicioEquipo.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
                //obtenre la lista para Unidad
                Log.Info("ListaUnidad - ListUnidad");
                var ListaUnidad = this.servicioEquipo.ListUnidad().Select(x => x.CentGeneNombre).ToList();

                #region Armando de contenido
                Log.Info("EntidadPeriodo - GetByIdPeriodo");
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1);

                var listaFechas = new List<DateTime>();

                for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
                {
                    listaFechas.Add(dt);
                }
                int iNroColumnas = listaFechas.Count() * 2;

                //Definimos la cabecera como una metriz
                string[] Cabecera1 = { "CENTRAL", "UNIDAD", "CÓDIGO RPF" }; //Titulos de cada columna
                string[] Cabecera2 = { "", "", "" };
                int[] widths = { 150, 150, 150 }; //Ancho de cada columna
                string[] itemDato = { "", "", "" };

                //Lista Dinámica
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                    Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                }

                //Formato de colummnas
                object[] columnas = new object[iNroColumnas + 3];
                columnas[0] = new
                {   //CENTRAL
                    type = GridExcelModel.TipoLista,
                    source = ListaCentral.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[1] = new
                {   //UNIDAD
                    type = GridExcelModel.TipoLista,
                    source = ListaUnidad.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[2] = new
                {   //CÓDIGO RPF
                    type = GridExcelModel.TipoTexto,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                int iAux = 3;
                foreach (DateTime item in listaFechas)
                {
                    Cabecera1[iAux] = item.ToString("dd/MM/yyyy");
                    Cabecera1[iAux + 1] = "";
                    Cabecera2[iAux] = "Incumplimiento";
                    Cabecera2[iAux + 1] = "Observacion";
                    widths[iAux] = widths[iAux + 1] = 100;
                    itemDato[iAux] = itemDato[iAux + 1] = "";
                    columnas[iAux] = new
                    {   //Incumplimiento
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htRight",
                        format = "0,0.000000000000",
                        readOnly = false,
                    };
                    columnas[iAux + 1] = new
                    {   //Observacion
                        type = GridExcelModel.TipoTexto,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htLeft",
                        readOnly = false,
                    };
                    iAux = iAux + 2;

                }

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRSF.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 3][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                int index = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 4)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;

                    string[] itemDato2 = { };
                    Array.Resize(ref itemDato2, 3 + iNroColumnas);
                    for (int iAux2 = 0; iAux2 <= (iNroColumnas + 2); iAux2++)
                    {
                        if (dtRow[iAux2 + 1] == null || dtRow[iAux2 + 1].ToString() == "")
                        {
                            itemDato2[iAux2] = "";
                        }
                        else
                        {
                            itemDato2[iAux2] = dtRow[iAux2 + 1].ToString();
                        }

                    }
                    data[index] = itemDato2;
                    index++;
                }

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true

                model.Data = data;
                //model.Headers = headers;
                model.Widths = widths;
                model.Columnas = columnas;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }
        }

        #endregion

        #region GRILLA PORCENTAJE DE LA RESERVA PRIMARIA NO SUMINISTRADA (RPNSdj,g):

        /// <summary>
        /// Muestra la grilla excel con los registros de IncumplimientoPR-21
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcelRPNS(int pericodi = 0, int vcrinccodi = 0)
        {
            GridExcelModel model = new GridExcelModel();
            IncumpPR21Model modelin = new IncumpPR21Model();

            //obtener los valores de la tabla detalle de RPNS
            Log.Info("ListaDetalleRPNS - GetByCriteriaVcrVerporctreservs");
            modelin.ListaDetalleRPNS = this.servicioCompensacionRSF.GetByCriteriaVcrVerporctreservs(vcrinccodi);
            //obtenre la lista para Central
            Log.Info("ListaCentral - ListCentralGeneracion");
            var ListaCentral = this.servicioEquipo.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
            //obtenre la lista para Unidad
            Log.Info("ListaUnidad - ListUnidad");
            var ListaUnidad = this.servicioEquipo.ListUnidad().Select(x => x.CentGeneNombre).ToList();

            #region Armando de contenido

            PeriodoDTO EntidadPeriodo = (new PeriodoAppServicio()).GetByIdPeriodo(pericodi);
            string sMes = EntidadPeriodo.MesCodi.ToString();
            if (sMes.Length == 1) sMes = "0" + sMes;
            var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

            DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
            DateTime dFecFin = dFecInicio.AddMonths(1);

            var listaFechas = new List<DateTime>();

            for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
            {
                listaFechas.Add(dt);
            }
            int iNroColumnas = listaFechas.Count(); 

            //Definimos la cabecera como una metriz
            string[] Cabecera1 = { "CENTRAL", "UNIDAD"}; //Titulos de cada columna
            string[] Cabecera2 = { "", ""};
            int[] widths = { 150, 150}; //Ancho de cada columna
            string[] itemDato = { "", ""};

            //Lista Dinámica
            if (iNroColumnas > 0)
            {
                Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                Array.Resize(ref widths, widths.Length + iNroColumnas);
                Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
            }

            //Formato de colummnas
            object[] columnas = new object[iNroColumnas + 2]; //+ central y unidad
            columnas[0] = new
            {   //CENTRAL
                type = GridExcelModel.TipoLista,
                source = ListaCentral.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            columnas[1] = new
            {   //UNIDAD
                type = GridExcelModel.TipoLista,
                source = ListaUnidad.ToArray(),
                strict = false,
                correctFormat = true,
                className = "htLeft",
                readOnly = false,
            };
            int iAux = 2;
            foreach (DateTime item in listaFechas)
            {
                Cabecera1[iAux] = item.ToString("dd/MM/yyyy");
                Cabecera2[iAux] = "RPNS";
                widths[iAux] = 100;
                itemDato[iAux] = "";
                columnas[iAux] = new
                {   //porcentaje de la reserva primaria no suministrada por el grupo “g” correspondiente al día “j”
                    type = GridExcelModel.TipoNumerico,
                    source = (new List<String>()).ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htRight",
                    format = "0,0.000000000000",
                    readOnly = false,
                };
                iAux = iAux + 1;
            }

            //Se arma la matriz de datos
            string[][] data;
            int index = 2;
            if (modelin.ListaDetalleRPNS.Count() != 0)
            {
                data = new string[modelin.ListaDetalleRPNS.Count() + 2][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                foreach (var item in modelin.ListaDetalleRPNS)
                {
                    string[] itemDato2 = { };
                    Array.Resize(ref itemDato2, iNroColumnas + 2); //+ central y unidad
                    int iAux2 = 2;
                    itemDato2[0] = item.CentralNombre.ToString();
                    itemDato2[1] = item.UnidadNombre.ToString();
                    List<VcrVerporctreservDTO> listVerporctreservs = this.servicioCompensacionRSF.ListVcrVerporctreservs(vcrinccodi, item.Equicodicen, item.Equicodiuni);
                    foreach (var dto in listVerporctreservs)
                    {
                        if (iAux2 > (iNroColumnas + 1))
                            break;
                        itemDato2[iAux2] = dto.Vcrvprrpns.ToString();
                        iAux2++;
                    }
                    data[index] = itemDato2;
                    index++;
                }
            }
            else
            {
                data = new string[3][];
                data[0] = Cabecera1;
                data[1] = Cabecera2;
                data[index] = itemDato;
            }


            #endregion

            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.NumeroColumnas = iNroColumnas; //Es el numero de columnas dobles por barra
            model.FixedColumnsLeft = 1;
            model.FixedRowsTop = 2;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcelRPNS(int pericodi, int vcrinccodi, string[][] datos)
        {
            base.ValidarSesionUsuario();
            IncumpPR21Model model = new IncumpPR21Model();
            VcrRecalculoDTO objIncumplimiento = this.servicioCompensacionRSF.GetByIncumplimiento(vcrinccodi);
            if (objIncumplimiento == null)
            {
                model.sError = "La revisión no esta enlazada con una valorización!";
                return Json(model);
            }
            int svcrinccodi = (int)objIncumplimiento.Vcrecacodi;
            //int svcrinccodi = vcrinccodi - 1;
            try
            {
                //////////////Eliminando datos////////
                Log.Info("Eliminando datos - DeleteVcrVerporctreserv");
                this.servicioCompensacionRSF.DeleteVcrVerporctreserv(vcrinccodi);
                Log.Info("EntidadPeriodo - GetByIdPeriodo");
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1);

                var listaFechas = new List<DateTime>();

                for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
                {
                    listaFechas.Add(dt);
                }
                int iNumFechas = listaFechas.Count; //Numero de dias
                //INSERTAR EL REGISTRO
                model.EntidadDetalleRPNS = new VcrVerporctreservDTO();
                model.EntidadDetalleRPNS.Vcrvprcodi = 0;
                model.EntidadDetalleRPNS.Vcrinccodi = vcrinccodi; //mes actual
                model.EntidadDetalleRPNS.Vcrvprusucreacion = User.Identity.Name;
                model.EntidadDetalleRPNS.Vcrvprfeccreacion = DateTime.Now;
                //Recorrer matriz para grabar detalle: se inicia en la fila 1
                for (int f = 2; f < datos.GetLength(0); f++)
                {   //Por Fila
                    if (datos[f] == null || datos[f][0] == null)
                        break; //FIN  
                    if (!datos[f][0].Equals(""))
                    {
                        string Central = Convert.ToString(datos[f][0]);
                        string Unidad = Convert.ToString(datos[f][1]);

                        if (Central.Equals(Unidad))
                        {
                            EqEquipoDTO dtoCentral = this.servicioEquipo.GetByCentGeneNombreEquipoCenUni(Central, svcrinccodi);
                            if (dtoCentral == null)
                            {
                                model.sError = "No existe la Central/Unidad " + Central;
                                return Json(model);
                            }
                            model.idCentral = dtoCentral.Equicodi;
                            model.idUnidad = model.idCentral;
                        }
                        else
                        {
                            Log.Info("EntidadCentral - GetByCentGeneNomb");
                            //CentralGeneracionDTO dtoCentral = this.servicioEquipo.GetByCentGeneNomb(Central);
                            EqEquipoDTO dtoCentral = this.servicioEquipo.GetByCentGeneNombreEquipo(Central, svcrinccodi);
                            if (dtoCentral == null)
                            {
                                model.sError = "No existe la Central " + Central;
                                return Json(model);
                            }
                            model.idCentral = dtoCentral.Equicodi;
                            if (model.idCentral == -1)
                            {
                                continue;
                            }
                            Log.Info("EntidadUnidad - GetByCentGeneNomb");
                            EqEquipoDTO dtoUnidad = this.servicioEquipo.GetByCentGeneUniNombreEquipo(Unidad, (int)dtoCentral.Equicodi, svcrinccodi);
                            if (dtoUnidad == null)
                            {
                                model.sError = "No existe la Unidad " + Unidad + " de la Central " + Central;
                                return Json(model);
                            }
                            model.idUnidad = dtoUnidad.Equicodi;
                        }

                        int aux = 0;
                        //Preparamos la data para registrar los detalles
                        for (int c = 2; c < iNumFechas + 2; c++)
                        {
                            model.EntidadDetalleRPNS.Vcrvprfecha = listaFechas[aux++];
                            model.EntidadDetalleRPNS.Equicodicen = model.idCentral;
                            model.EntidadDetalleRPNS.Equicodiuni = model.idUnidad;
                            //Porcentaje de la reserva primaria no suministrada por el grupo “g” correspondiente al día “j”
                            model.EntidadDetalleRPNS.Vcrvprrpns = 0;
                            if (datos[f][c] != null || datos[f][c].ToString() != "")
                            {
                                model.EntidadDetalleRPNS.Vcrvprrpns = UtilSistemasTransmision.ValidarNumero(datos[f][c].ToString());
                            }
                            Log.Info("Insertar registro - SaveVcrVerporctreserv");
                            this.servicioCompensacionRSF.SaveVcrVerporctreserv(model.EntidadDetalleRPNS);
                        }
                    }

                    else
                    {
                        continue;
                    }
                }
                //INSERTAMOS vcr_verincumplim; PARA EQ_EQUIPO = -1
                model.EntidadDetalleRPNS.Equicodicen = -1;
                model.EntidadDetalleRPNS.Equicodiuni = -1;
                model.EntidadDetalleRPNS.Vcrvprrpns = 0;
                for (int c = 0; c < iNumFechas; c++)
                {
                    model.EntidadDetalleRPNS.Vcrvprfecha = listaFechas[c];
                    this.servicioCompensacionRSF.SaveVcrVerporctreserv(model.EntidadDetalleRPNS);
                }
                //-----------------------------------------
                model.sError = "";
                model.sMensaje = "Felicidades, la carga de información fue exitosa, Fecha de procesamiento: <b>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</b>";
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }

        /// <summary>
        /// Permite eliminar todos los registros de la versión 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarDatosRPNS(int pericodi, int vcrinccodi)
        {
            base.ValidarSesionUsuario();
            string sResultado = "1";

            try
            {
                ////////////Eliminando datos////////
                Log.Info("Eliminando datos - DeleteVcrVerporctreserv");
                this.servicioCompensacionRSF.DeleteVcrVerporctreserv(vcrinccodi);
                ///////////////////////////////////
            }
            catch (Exception e)
            {
                sResultado = e.Message; //"-1";
            }

            return Json(sResultado);

        }


        /// <summary>
        /// Permite exportar a un archivo excel todos los registros en pantalla de consulta
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDataRPNS(int pericodi, int vcrinccodi = 0, int formato = 1)
        {
            base.ValidarSesionUsuario();
            try
            {
                string PathLogo = @"Content\Images\logocoes.png";
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + PathLogo; //RutaDirectorio.PathLogo;
                string pathFile = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
                Log.Info("Exportando información - GenerarFormatoVcrPr21");
                string file = this.servicioCompensacionRSF.GenerarFormatoVcrRPNS(vcrinccodi, formato, pathFile, pathLogo);
                return Json(file);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        /// <summary>
        /// Lee datos desde el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarArchivoRPNS(string sarchivo, int pericodi, int vcrinccodi = 0)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();
            string sResultado = "1";
            int iRegError = 0;
            string sMensajeError = "";

            try
            {
                //obtenre la lista para Central
                Log.Info("ListaCentral - ListCentralGeneracion");
                var ListaCentral = this.servicioEquipo.ListCentralGeneracion().Select(x => x.CentGeneNombre).ToList();
                //obtenre la lista para Unidad
                Log.Info("ListaUnidad - ListUnidad");
                var ListaUnidad = this.servicioEquipo.ListUnidad().Select(x => x.CentGeneNombre).ToList();

                #region Armando de contenido
                Log.Info("EntidadPeriodo - GetByIdPeriodo");
                PeriodoDTO EntidadPeriodo = this.servicioPeriodo.GetByIdPeriodo(pericodi);
                string sMes = EntidadPeriodo.MesCodi.ToString();
                if (sMes.Length == 1) sMes = "0" + sMes;
                var sFechaInicio = "01/" + sMes + "/" + EntidadPeriodo.AnioCodi;

                DateTime dFecInicio = DateTime.ParseExact(sFechaInicio, ConstantesBase.FormatoFechaPE, CultureInfo.InvariantCulture);
                DateTime dFecFin = dFecInicio.AddMonths(1);

                var listaFechas = new List<DateTime>();

                for (var dt = dFecInicio; dt < dFecFin; dt = dt.AddDays(1))
                {
                    listaFechas.Add(dt);
                }
                int iNroColumnas = listaFechas.Count();

                //Definimos la cabecera como una metriz
                string[] Cabecera1 = { "CENTRAL", "UNIDAD"}; //Titulos de cada columna
                string[] Cabecera2 = { "", ""};
                int[] widths = { 150, 150}; //Ancho de cada columna
                string[] itemDato = { "", ""};

                //Lista Dinámica
                if (iNroColumnas > 0)
                {
                    Array.Resize(ref Cabecera1, Cabecera1.Length + iNroColumnas);
                    Array.Resize(ref Cabecera2, Cabecera2.Length + iNroColumnas);
                    Array.Resize(ref widths, widths.Length + iNroColumnas);
                    Array.Resize(ref itemDato, itemDato.Length + iNroColumnas);
                }

                //Formato de colummnas
                object[] columnas = new object[iNroColumnas + 2];
                columnas[0] = new
                {   //CENTRAL
                    type = GridExcelModel.TipoLista,
                    source = ListaCentral.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                columnas[1] = new
                {   //UNIDAD
                    type = GridExcelModel.TipoLista,
                    source = ListaUnidad.ToArray(),
                    strict = false,
                    correctFormat = true,
                    className = "htLeft",
                    readOnly = false,
                };
                int iAux = 2;
                foreach (DateTime item in listaFechas)
                {
                    Cabecera1[iAux] = item.ToString("dd/MM/yyyy");
                    Cabecera2[iAux] = "RPNS";
                    widths[iAux] = 100;
                    itemDato[iAux] = "";
                    columnas[iAux] = new
                    {   //Incumplimiento
                        type = GridExcelModel.TipoNumerico,
                        source = (new List<String>()).ToArray(),
                        strict = false,
                        correctFormat = true,
                        className = "htRight",
                        format = "0,0.000000000000",
                        readOnly = false,
                    };
                    iAux = iAux + 1;

                }

                //Traemos la primera hoja del archivo
                DataSet ds = new DataSet();
                ds = this.servicioCompensacionRSF.GeneraDataset(path + sarchivo, 1);

                string[][] data = new string[ds.Tables[0].Rows.Count - 3][]; // Lee todo el contenido del excel y le descontamos 4 filas hasta donde empieza la data
                int index = 0;
                int iFila = 0;
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    iFila++;
                    if (iFila < 4)
                    {
                        continue;
                    }
                    int iNumFila = iFila + 1;

                    string[] itemDato2 = { };
                    Array.Resize(ref itemDato2, iNroColumnas + 2);
                    for (int iAux2 = 0; iAux2 <= iNroColumnas + 1; iAux2++)
                    {
                        if (dtRow[iAux2 + 1] == null || dtRow[iAux2 + 1].ToString() == "")
                        {
                            itemDato2[iAux2] = "";
                        }
                        else
                        {
                            itemDato2[iAux2] = dtRow[iAux2 + 1].ToString();
                        }

                    }
                    data[index] = itemDato2;
                    index++;
                }

                #endregion
                model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true

                model.Data = data;
                //model.Headers = headers;
                model.Widths = widths;
                model.Columnas = columnas;
                model.RegError = iRegError;
                model.MensajeError = sMensajeError;

                return Json(model);
            }
            catch (Exception e)
            {
                sResultado = e.Message;
                return Json(sResultado);
            }
        }

        #endregion

        #region FUNCIONES GENERALES PARA TODOS LOS TABS

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString() + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, sFecha + "_" + file);
        }

        /// <summary>
        /// Permite cargar un archivo Excel
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadExcel()
        {
            base.ValidarSesionUsuario();
            string sNombreArchivo = "";
            string path = ConfigurationManager.AppSettings[ConstantesSistemasTransmision.ReporteDirectorio].ToString();

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreArchivo = file.FileName;
                    if (System.IO.File.Exists(path + sNombreArchivo))
                    {
                        System.IO.File.Delete(path + sNombreArchivo);
                    }
                    file.SaveAs(path + sNombreArchivo);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}
