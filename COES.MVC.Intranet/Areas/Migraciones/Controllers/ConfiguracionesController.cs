using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Migraciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Migraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Helper;
using System.Globalization;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using System.IO;
using OfficeOpenXml;
using System.Reflection;
using COES.Servicios.Aplicacion.FormatoMedicion;

namespace COES.MVC.Intranet.Areas.Migraciones.Controllers
{
    public class ConfiguracionesController : BaseController
    {
        MigracionesAppServicio servicio = new MigracionesAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ConfiguracionesController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Protected de log de errores page
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
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionMigraciones.SesionNombreArchivo] != null) ?
                    Session[DatosSesionMigraciones.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionMigraciones.SesionNombreArchivo] = value; }
        }

        #endregion

        /// <summary>
        /// //Descarga el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppXML, nombreArchivo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            string sNombreArchivo = "";

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

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
                    this.NombreFile = sNombreArchivo;
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Actividades del personal

        public ActionResult ActividadesPersonal()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();
            model.ListaAreas = servicio.ListSiAreas().Where(x => x.Emprcodi == 1).ToList();
            model.Fecha = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <returns></returns>
        public JsonResult CargarActividadesPersonal(string areacodi)
        {
            MigracionesModel model = new MigracionesModel();

            var lista = servicio.GetListaActividadesPersonal(areacodi);

            var ruta = Url.Content("~/");
            model.Resultado = servicio.ListaActividadesPersonalHtml(lista, ruta);
            model.nRegistros = lista.Count();

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <param name="acti"></param>
        /// <param name="descrip"></param>
        /// <returns></returns>
        public JsonResult SaveActividad(string areacodi, string acti, string descrip, int evnto, int actcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                if (evnto == 1)
                {
                    servicio.SaveSiActividad(new SiActividadDTO()
                    {
                        Areacodi = int.Parse(areacodi),
                        Actabrev = acti,
                        Actnomb = descrip,
                        Lastuser = User.Identity.Name,
                        Lastdate = DateTime.Now
                    });
                }
                else
                {
                    servicio.UpdateSiActividad(new SiActividadDTO()
                    {
                        Areacodi = int.Parse(areacodi),
                        Actcodi = actcodi,
                        Actabrev = acti,
                        Actnomb = descrip,
                        Lastuser = User.Identity.Name,
                        Lastdate = DateTime.Now
                    });
                }

                model.nRegistros = 1;
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <returns></returns>
        public JsonResult DeleteActividad(int actcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                var actividad = servicio.GetByCriteriaSiRolTurnos().Find(x => x.Actcodi == actcodi);
                if (actividad == null)
                {
                    servicio.DeleteSiActividad(actcodi);

                    model.nRegistros = 1;
                }
                else { model.nRegistros = -2; }
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actcodi"></param>
        /// <param name="evnto"></param>
        /// <returns></returns>
        public JsonResult ProcesoEditActividad(int actcodi, int evnto)
        {
            MigracionesModel model = new MigracionesModel();

            var obj = servicio.GetByIdSiActividad(actcodi);
            if (obj != null)
            {
                model.Actcodi = (int)actcodi;
                model.Areacodi = (int)obj.Areacodi;
                model.Actabrev = obj.Actabrev;
                model.Actnomb = obj.Actnomb;
                model.Readonly = (evnto == 1 ? "Readonly" : "");
                model.Disabled = (evnto == 1 ? "Disabled" : "");
            }

            return Json(model);
        }

        #endregion

        #region Rol de turnos

        public ActionResult Rolturnos()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();
            model.TienePermisoGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.ListaAreas = servicio.ListSiAreas().Where(x => x.Emprcodi == 1).ToList();
            model.MesAnio = DateTime.Now.ToString(ConstantesBase.FormatoMesAnio);

            List<UserModel> list_ = new List<UserModel>();
            list_.Add(new UserModel() { IdArea = 1, Roles = "Rol de Turnos" });
            list_.Add(new UserModel() { IdArea = 2, Roles = "Movimientos" });
            model.ListaSelect = list_;

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <param name="tipo"></param>
        /// <param name="mesanio"></param>
        /// <returns></returns>
        public JsonResult CargarRolturnos(string areacodi, string mesanio)
        {
            base.ValidarSesionJsonResult();

            MigracionesModel model = new MigracionesModel();
            model.TienePermisoGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            model.Handson = new HandsonModel();
            model.Handson.ReadOnly = !model.TienePermisoGrabar;

            List<SiRolTurnoDTO> Rols = new List<SiRolTurnoDTO>();

            DateTime fecIni = DateTime.MinValue, fecFin = DateTime.MinValue;
            if (mesanio != null)
            {
                mesanio = ConstantesAppServicio.IniDiaFecha + mesanio.Replace("-", "/");
                fecIni = DateTime.ParseExact(mesanio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fecFin = fecIni.AddMonths(1).AddDays(-1);
            }

            var listaAct = servicio.GetListaActividadesPersonal(areacodi);

            var listaPer = servicio.GetListaPersonalRol(areacodi, fecIni, fecFin);


            if (listaPer.Count > 0)
            {
                Rols = servicio.ListaRols(fecIni, fecFin, string.Join(",", listaPer.Select(x => x.Percodi).Distinct().ToList()))
                    .Where(x => x.Roltestado == ConstantesAppServicio.SI).ToList();
                servicio.FillListaPer(listaPer, ref Rols);
            }

            TimeSpan ts = fecFin - fecIni;
            int colum_ = ts.Days, columAdi = 4;
            model.Handson.ListaExcelData = servicio.InicializacionMatriz(2, listaPer.Count, colum_ + columAdi);
            model.Handson.ListaExcelComment = servicio.InicializacionMatriz(2, listaPer.Count, colum_ + columAdi);

            var listaMovi = servicio.ListaMovimientos(fecIni, fecFin).Where(x => listaPer.Select(e => e.Percodi).Contains(x.Percodi)).ToList();
            if (!model.TienePermisoGrabar)
                listaMovi = new List<SiRolTurnoDTO>();
            servicio.GeneraExcelWebRolTurnos(model.Handson.ListaExcelData, model.Handson.ListaExcelComment, colum_ + columAdi, fecIni, fecFin, listaPer, Rols, listaMovi);

            model.ListaString = new List<string>();
            model.ListaStringNoRepet = new List<string>();
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(70);
            model.Handson.ListaColWidth.Add(160);
            for (int x = 2; x <= colum_ + columAdi; x++)
            {
                model.Handson.ListaColWidth.Add(45);
                model.ListaString.Add(string.Join(",", listaAct.Select(d => "&" + d.Actabrev.ToUpper() + "&").ToList()));
                model.ListaStringNoRepet.Add(string.Empty);
            }

            model.Administrador = 1;
            /*if (fecFin.Month == 1)
            {
                model.Administrador = (accesoAprobar ? 1 : 0);
            }
            else { model.Administrador = 0; }*/
            model.nRegistros = Rols.Count();
            model.Comentario = servicio.ListaLeyendaAct(listaAct);

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <param name="tipo"></param>
        /// <param name="mesanio"></param>
        /// <returns></returns>
        public JsonResult SaveRolturnos(string areacodi, string mesanio, string[][] data)
        {
            base.ValidarSesionJsonResult();
            if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

            MigracionesModel model = new MigracionesModel();
            List<SiRolTurnoDTO> Rols = new List<SiRolTurnoDTO>();
            bool accesoAprobar = base.VerificarAccesoAccion(Acciones.Aprobar, User.Identity.Name);
            accesoAprobar = true;
            try
            {
                DateTime fecIni = DateTime.MinValue, fecFin = DateTime.MinValue;
                if (mesanio != null)
                {
                    mesanio = ConstantesAppServicio.IniDiaFecha + mesanio.Replace("-", "/");
                    fecIni = DateTime.ParseExact(mesanio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    fecFin = fecIni.AddMonths(1).AddDays(-1);
                }

                if (this.VerificarDatos(data, fecIni, fecFin))
                {
                    var listaAct = servicio.GetListaActividadesPersonal(areacodi);

                    var listaPer = servicio.GetListaPersonalRol(areacodi, fecIni, fecFin);
                    var rolsConsulta = servicio.ListaRols(fecIni, fecFin, string.Join(",", listaPer.Select(x => x.Percodi).Distinct().ToList()));

                    Rols = this.servicio.GetDataRols(data, rolsConsulta, listaAct, fecIni, fecFin, User.Identity.Name).Where(x => x.Actcodi > 0).ToList();

                    if (Rols.Count > 0)
                    {
                        List<string> ListaPercodi = new List<string>();
                        for (int x = 2; x < data.Length; x++)
                        {
                            ListaPercodi.Add(data[x][1]);
                        }

                        var rolsInsertar = Rols.Where(x => x.Tipoproceso == 1).ToList();
                        var rolsActualizar = Rols.Where(x => x.Tipoproceso == 2).ToList();

                        if (rolsActualizar.Any())
                        {
                            foreach (var rolTurno in rolsActualizar)
                            {
                                servicio.UpdateSiRolTurno(rolTurno);
                            }
                        }

                        if (rolsInsertar.Any())
                        {
                            servicio.SaveSiRolTurnoMasivo(rolsInsertar);
                        }

                    }
                    model.Administrador = 1;
                    /*if (fecFin.Month == 1)
                    {
                        model.Administrador = (accesoAprobar ? 1 : 0);
                    }
                    else { model.Administrador = 0; }*/

                    model.nRegistros = 1;
                }
                else { model.nRegistros = -2; }
            }
            catch (Exception e)
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool VerificarDatos(string[][] data, DateTime fecIni, DateTime fecFin)
        {
            bool val = true;

            for (int x = 2; x < data.Length - 2; x++)
            {
                DateTime f_ = fecIni.AddDays(-1);
                for (int y = fecIni.Day; y < fecFin.Day; y++)
                {
                    //esta validacion se ha deshabilitado 26/09/2018
                    //if (data[x][y + 1] == "") { val = false; break; }
                }
            }

            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <param name="tipo"></param>
        /// <param name="mesanio"></param>
        /// <returns></returns>
        public JsonResult VerificarDatosMeses(string areacodi, string mesanio, string[][] data)
        {
            MigracionesModel model = new MigracionesModel();
            List<SiRolTurnoDTO> Rols = new List<SiRolTurnoDTO>();
            List<SiRolTurnoDTO> RolsNew = new List<SiRolTurnoDTO>();

            try
            {
                DateTime fecIni = DateTime.MinValue, fecFin = DateTime.MinValue;
                if (mesanio != null)
                {
                    mesanio = ConstantesAppServicio.IniDiaFecha + mesanio.Replace("-", "/");
                    fecIni = DateTime.ParseExact(mesanio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    fecFin = fecIni.AddMonths(1).AddDays(-1);
                }

                if (this.VerificarDatos(data, fecIni, fecFin))
                {
                    List<string> ListaPercodi = new List<string>();
                    var listaAct = servicio.GetListaActividadesPersonal(areacodi);

                    for (int x = 2; x < data.Length - 2; x++)
                    {
                        ListaPercodi.Add(data[x][1]);
                    }

                    Rols = servicio.ListaRols(fecFin.AddDays(1), new DateTime(fecFin.Year, 12, 31), string.Join(",", ListaPercodi));

                    model.nRegistros = (Rols.Count > 0 ? 1 : 0);
                }
                else { model.nRegistros = -2; }
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areacodi"></param>
        /// <param name="mesanio"></param>
        /// <returns></returns>
        public JsonResult CargarMovimientos(string areacodi, string mesanio)
        {
            MigracionesModel model = new MigracionesModel();

            DateTime fecIni = DateTime.MinValue, fecFin = DateTime.MinValue;
            if (mesanio != null)
            {
                mesanio = ConstantesAppServicio.IniDiaFecha + mesanio.Replace("-", "/");
                fecIni = DateTime.ParseExact(mesanio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fecFin = fecIni.AddMonths(1).AddDays(-1);
            }

            var ListaMovi = servicio.ListaMovimientos(fecIni, fecFin);
            model.Resultado = servicio.ListaMovimientosHtml(ListaMovi);
            model.nRegistros = ListaMovi.Count;

            return Json(model);
        }

        /// <summary>
        /// Exportacion del Anexo A a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult GenerarReporteXls(int areacodi, string mesanio)
        {
            MigracionesModel model = new MigracionesModel();
            List<SiRolTurnoDTO> Rols = new List<SiRolTurnoDTO>();
            DateTime fecIni = DateTime.MinValue, fecFin = DateTime.MinValue;

            try
            {
                if (mesanio != null)
                {
                    mesanio = ConstantesAppServicio.IniDiaFecha + mesanio.Replace("-", "/");
                    fecIni = DateTime.ParseExact(mesanio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    fecFin = fecIni.AddMonths(1).AddDays(-1);
                }

                //var listaAct = servicio.GetListaActividadesPersonal(areacodi);
                var listaPer = servicio.GetListaPersonalRol(areacodi.ToString(), fecIni, fecFin);

                if (listaPer.Count > 0)
                {
                    Rols = servicio.ListaRols(fecIni, fecFin, string.Join(",", listaPer.Select(x => x.Percodi).Distinct().ToList()))
                        .Where(x => x.Roltestado == ConstantesAppServicio.SI).ToList(); ;
                    servicio.FillListaPer(listaPer, ref Rols);
                }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = ConstantesMigraciones.RptRols + "_" + fecFin.ToString("yyyyMM") + ConstantesAppServicio.ExtensionExcel;
                this.servicio.GenerarArchivoExcelRols(fecIni, fecFin, listaPer, Rols, areacodi, ruta + nameFile);

                model.Resultado = nameFile;
                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Exportacion del Anexo A a archivo Excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult ImportarDatosRolTurnos(string areacodi, string mesanio)
        {
            base.ValidarSesionJsonResult();
            if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

            MigracionesModel model = new MigracionesModel();
            model.Handson = new HandsonModel();
            List<SiRolTurnoDTO> Rols = new List<SiRolTurnoDTO>();

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            DateTime fecIni = DateTime.MinValue, fecFin = DateTime.MinValue;
            if (mesanio != null)
            {
                var arrF = mesanio.Split('-');
                fecIni = new DateTime(int.Parse(arrF[1]), int.Parse(arrF[0]), 1);
                fecFin = fecIni.AddMonths(1).AddDays(-1);
            }

            var listaAct = servicio.GetListaActividadesPersonal(areacodi);
            var listaPer = servicio.GetListaPersonalRol(areacodi, fecIni, fecFin);

            int nFil = listaPer.Count, nCol = fecFin.Day;

            string path_ = path + NombreFile;
            FileInfo fileInfo = new FileInfo(path_);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                //Verificar Formato
                int pp = 0;
                for (int i = 8; i < nFil + 8; i++)
                {
                    DateTime f_ = fecIni.AddDays(-1);
                    for (int j = 4; j < nCol + 4; j++)
                    {
                        var obj = new SiRolTurnoDTO();
                        obj.Percodi = listaPer[pp].Percodi;
                        obj.Pernomb = listaPer[pp].Pernomb;
                        obj.Lastuser = User.Identity.Name;
                        obj.Lastdate = f_.AddDays(j - 3);
                        obj.Roltfecha = f_.AddDays(j - 3);
                        obj.Roltestado = "S";
                        var actcodi = listaAct.Find(c => c.Actabrev.Trim().ToUpper() == (ws.Cells[i, j].Value != null ? ws.Cells[i, j].Value.ToString().Trim().ToUpper() : string.Empty));
                        obj.Actcodi = (actcodi != null ? actcodi.Actcodi : 0);
                        obj.Actabrev = (actcodi != null ? actcodi.Actabrev : string.Empty);
                        Rols.Add(obj);
                    }
                    pp++;
                }

            }

            if (listaPer.Count > 0)
            {
                servicio.FillListaPer(listaPer, ref Rols);
            }

            TimeSpan ts = fecFin - fecIni;
            int colum_ = ts.Days, columAdi = 4;
            model.Handson.ListaExcelData = servicio.InicializacionMatriz(2, listaPer.Count, colum_ + columAdi);
            model.Handson.ListaExcelComment = servicio.InicializacionMatriz(2, listaPer.Count, colum_ + columAdi);


            var ListaMovi = servicio.ListaMovimientos(fecIni, fecFin).Where(x => listaPer.Select(e => e.Percodi).Contains(x.Percodi)).ToList();
            servicio.GeneraExcelWebRolTurnos(model.Handson.ListaExcelData, model.Handson.ListaExcelComment, colum_ + columAdi, fecIni, fecFin, listaPer, Rols, ListaMovi);

            model.ListaString = new List<string>();
            model.ListaStringNoRepet = new List<string>();
            model.Handson.ListaColWidth = new List<int>();
            model.Handson.ListaColWidth.Add(70);
            model.Handson.ListaColWidth.Add(160);
            for (int x = 1; x <= colum_ + columAdi; x++)
            {
                model.Handson.ListaColWidth.Add(45);
                model.ListaString.Add(string.Join(",", listaAct.Select(d => "&" + d.Actabrev.ToUpper() + "&").ToList()));
                model.ListaStringNoRepet.Add(string.Empty);
            }

            model.Administrador = 1;
            /*if (fecFin.Month == 1)
            {
                model.Administrador = (accesoAprobar ? 1 : 0);
            }
            else { model.Administrador = 0; }*/
            model.nRegistros = Rols.Count();
            model.Comentario = servicio.ListaLeyendaAct(listaAct);

            //Eliminamos archivo xlsm
            System.IO.File.Delete(path_);

            return Json(model);
        }
        #endregion

        #region Comunicados

        public ActionResult Comunicados()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            MigracionesModel model = new MigracionesModel();

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);
            model.FechaIni = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult CargarComunicados(int typ)
        {
            MigracionesModel model = new MigracionesModel();
            List<WbComunicadosDTO> lista = new List<WbComunicadosDTO>();

            if (typ == 1)
            {
                lista = servicio.GetListaComunicados().Where(x => x.Comestado != "X" && x.Composition == 1).OrderBy(x => x.Comorden).ToList();
                int i = 1;
                foreach (var d in lista)
                {
                    d.Comorden = i;
                    servicio.ActualizarWbComunicados(d);
                    i++;
                }
            }
            else
            {
                lista = servicio.GetListaComunicados().Where(x => x.Comestado != "X" && x.Composition != 1 && x.Comtipo != "S").OrderByDescending(x => x.Comfecha).ToList();
            }

            var ruta = Url.Content("~/");
            model.Resultado = servicio.ListaComunicadosHtml(lista, ruta, typ);
            model.nRegistros = lista.Count();

            return Json(model);
        }

        /// <summary>
        /// Update Orden Comunicados
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrdenComunicados(int id, int fromPosition, int toPosition, string direction)
        {
            List<WbComunicadosDTO> lista = servicio.GetListaComunicados().Where(x => x.Comestado != "X" && x.Composition == 1).OrderBy(x => x.Comorden).ToList();

            if (direction == "back")
            {
                int orden = toPosition;
                List<WbComunicadosDTO> ltmp = new List<WbComunicadosDTO>();
                ltmp.Add(lista[fromPosition - 1]);
                ltmp.AddRange(lista.GetRange(toPosition - 1, fromPosition - toPosition));
                foreach (var reg in ltmp)
                {
                    reg.Comorden = orden;
                    this.servicio.ActualizarWbComunicados(reg); //Actualizar el orden
                    orden++;
                }
            }
            else
            {
                int orden = fromPosition;
                List<WbComunicadosDTO> ltmp = new List<WbComunicadosDTO>();
                ltmp.AddRange(lista.GetRange(fromPosition, toPosition - fromPosition));
                ltmp.Add(lista[fromPosition - 1]);
                foreach (var reg in ltmp)
                {
                    reg.Comorden = orden;
                    this.servicio.ActualizarWbComunicados(reg); //Actualizar el orden
                    orden++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comcodi"></param>
        /// <param name="evnto"></param>
        /// <returns></returns>
        public JsonResult ProcesoEditComunicado(int comcodi, int evnto, string pos)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                var lista = servicio.GetListaComunicados().Where(x => x.Comcodi == comcodi).ToList();

                foreach (var d in lista)
                {
                    if (evnto == 1)
                    {
                        d.Composition = (pos == "" ? 1 : 0);
                        this.servicio.ActualizarWbComunicados(d); //Actualizar el orden
                    }
                    else
                    {
                        model.Wbcomunicados = new WbComunicadosDTO();
                        model.Wbcomunicados.Comcodi = comcodi;
                        model.Wbcomunicados.Comfecha = d.Comfecha;
                        model.Wbcomunicados.Comtitulo = d.Comtitulo;
                        model.Wbcomunicados.Comresumen = null;
                        model.Wbcomunicados.Comdesc = d.Comdesc;
                        model.Wbcomunicados.Comlink = d.Comlink;
                        model.Wbcomunicados.Comfechaini = d.Comfechaini;
                        model.Wbcomunicados.Comfechafin = d.Comfechafin;
                        model.Wbcomunicados.Comestado = d.Comestado;
                        model.Wbcomunicados.ComfechaDesc = d.Comfecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
                        model.Wbcomunicados.ComfechainiDesc = d.Comfechaini.Value.ToString(ConstantesAppServicio.FormatoFecha);
                        model.Wbcomunicados.ComfechafinDesc = d.Comfechafin.Value.ToString(ConstantesAppServicio.FormatoFecha);
                    }
                }

                model.nRegistros = 1;
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteComunicado(int comcodi)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                servicio.EliminarWbComunicados(comcodi);

                model.nRegistros = 1;
            }
            catch
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        /// <summary>
        /// Insertar nuevo comunicado
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveComunicado(string fecha, string titu, string descrip, string lin, string fecha1, string fecha2, string est, int evnto, int comcodi, string tipocomu)
        {
            MigracionesModel model = new MigracionesModel();

            try
            {
                string est_ = est;
                int posit_ = 0;
                var verif = servicio.GetListaComunicados().Find(x => x.Comcodi == comcodi);

                if (verif != null) { est_ = verif.Comestado; posit_ = verif.Composition ?? 0; }

                if (evnto == 2)
                {
                    servicio.ActualizarWbComunicados(new WbComunicadosDTO()
                    {
                        Comcodi = comcodi,
                        //Comfecha = DateTime.Now, // DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comfecha = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.CurrentCulture),
                        Comtitulo = titu,
                        Comresumen = null,
                        Comdesc = descrip,
                        Comlink = lin,
                        Comfechaini = DateTime.ParseExact(fecha1, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comfechafin = DateTime.ParseExact(fecha2, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comestado = "A",
                        Composition = posit_
                    });
                }
                else
                {
                    servicio.InsertarWbComunicados(new WbComunicadosDTO()
                    {
                        Comfecha = DateTime.Now, // DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comtitulo = titu,
                        Comresumen = null,
                        Comdesc = descrip,
                        Comlink = lin,
                        Comfechaini = DateTime.ParseExact(fecha1, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comfechafin = DateTime.ParseExact(fecha2, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture),
                        Comestado = "A",
                        Comtipo = tipocomu
                    });
                }

                model.nRegistros = 1;
            }
            catch (Exception ex)
            {
                model.nRegistros = -1;
            }

            return Json(model);
        }

        #endregion
    }
}