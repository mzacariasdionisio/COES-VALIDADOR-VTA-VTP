using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class ParametrosTecnicosController : Controller
    {
        //
        // GET: /Equipamiento/ParametrosTecnicos/
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ParametrosTecnicosController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        NotificacionCambioEquipos _servicio = new NotificacionCambioEquipos();
        EquipamientoAppServicio servEq = new EquipamientoAppServicio();
        GeneralAppServicio ServGeneral = new GeneralAppServicio();

        public ParametrosTecnicosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ParametrosTecnicosController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ParametrosTecnicosController", ex);
                throw;
            }
        }

        /// <summary>
        /// Descargar archivo excel
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteXls(string nameFile)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nameFile;
            return File(fullPath, ConstantesAppServicio.AppExcel, nameFile);
        }

        public ActionResult Index()
        {
            var model = new EquipamientoModel() { Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha) };

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetParametrosTecnicos(string fechaini, string fechafin, int tip)
        {
            var model = new EquipamientoModel();
            var lPropEq = new List<int>() { 1831, 1832, ConstantesAppServicio.PropiedadOperacionComercial, 1833, 1834, 1557, 1526 };//empresa, estado, inicio y fin operacion comercial
            var lPropEqOC = new List<int>() { ConstantesAppServicio.PropiedadOperacionComercial };// inicio y fin operacion comercial

            try
            {
                DateTime f1 = DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime f2 = DateTime.ParseExact(fechafin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                var listaEquipo = _servicio.ListadoEquiposModificados(f1, f2.AddDays(1));
                var equiposmod = listaEquipo.Where(x => x.FechaUpdate != null).ToList();

                var equiposnew = listaEquipo.Where(x => x.Lastdate != null).ToList();
                equiposnew = equiposnew.Where(x => x.Equiestado == ConstantesAppServicio.Activo).ToList();

                var listaPropiedad = _servicio.ListadoPropiedadesValoresModificados(-1, -1, f1, f2.AddDays(1));

                var equiposbaj = listaPropiedad.Where(x => x.Propnomb.ToUpper().Contains("ESTADO")).ToList();
                equiposbaj = equiposbaj.Where(x => x.Equiestado == ConstantesAppServicio.Baja).ToList();

                var propiedadesEqui = listaPropiedad.Where(x => !lPropEq.Contains(x.Propcodi)).ToList();

                var listaGrupo = _servicio.ListadoConceptosActualizados(f1, f2.AddDays(1));
                listaGrupo = listaGrupo.Where(x => x.Concepcodi != ConstantesAppServicio.ConcepcodiActivo).ToList();
                var operacionesCome = listaPropiedad.Where(x => lPropEqOC.Contains(x.Propcodi)).ToList();

                equiposmod = equiposmod.OrderBy(x => x.FechaUpdate).ToList();
                equiposnew = equiposnew.OrderBy(x => x.Lastdate).ToList();
                equiposbaj = equiposbaj.OrderBy(x => x.Fechapropequi).ToList();
                propiedadesEqui = propiedadesEqui.OrderBy(x => x.Fechapropequi).ToList();
                listaGrupo = listaGrupo.OrderBy(x => x.Fechaact).ToList();
                operacionesCome = operacionesCome.OrderBy(x => x.Fechapropequi).ToList();

                var listacount = new List<int>();
                listacount.Add(equiposmod.Count);
                listacount.Add(equiposnew.Count);
                listacount.Add(equiposbaj.Count);
                listacount.Add(propiedadesEqui.Count);
                listacount.Add(listaGrupo.Count);
                listacount.Add(operacionesCome.Count);

                switch (tip)
                {
                    case 1: model.ListaResultado = _servicio.ReporteParametros(equiposmod, equiposnew, equiposbaj, propiedadesEqui, listaGrupo, operacionesCome); break;
                    case 2:
                        string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                        string nameFile = "Rpt_" + f1.ToString("yyyyMMdd")+"_" + f2.ToString("yyyyMMdd") + ConstantesAppServicio.ExtensionExcel;
                        _servicio.GenerarExcelReporteParametros(equiposmod, equiposnew, equiposbaj, propiedadesEqui, listaGrupo, operacionesCome, ruta + nameFile
                            , AppDomain.CurrentDomain.BaseDirectory + "/", f1, f2);
                        model.Resultado = nameFile; break;
                }

                model.ListaCount = listacount;
                foreach(var c in listacount)
                {
                    model.Count += c;
                }
            }
            catch (Exception ex) { model.Count = -1; }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Equipos sin datos de ficha técnica
        /// <summary>
        /// Index Equipos sin Valor en sus Propiedades
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexEquiposProp()
        {
            var model = new DetalleEquipoModel()
            {
                ListaTipoEmpresa = ServGeneral.ListarTiposEmpresa().Where(t => t.Tipoemprcodi > 0).OrderBy(t => t.Tipoemprdesc).ToList(),
                ListaEmpresa = servEq.ListarEmpresasSeinFT(),
                ListaTipoEquipo = servEq.ListarFamiliaXEmp(-1)
            };

            return View(model);
        }

        /// <summary>
        /// Listado de equipos con propiedades sin valor
        /// </summary>
        /// <param name="miDataM"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoEqPropSinValor(DetalleEquipoModel miDataM)
        {
            DetalleEquipoModel model = new DetalleEquipoModel();
            try
            {
                EqPropequiDTO EqPropVaciaDTO = new EqPropequiDTO();
                EqPropVaciaDTO.Emprcodi = miDataM.Emprcodi.Value;
                EqPropVaciaDTO.Famcodi = miDataM.Famcodi.Value;

                model.ListadoValoresVacios = servEq.ListarEquiposPropiedadesSinValor(miDataM.TipoEmpresa, miDataM.Emprcodi.Value, miDataM.Famcodi.Value, miDataM.Equiestado, DateTime.Now, DateTime.Now);

                model.PropVaciasCount = model.ListadoValoresVacios.Sum(x => x.Propsinvacio) + " / " + model.ListadoValoresVacios.Sum(x => x.Proptotal);
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                log.Error(NameController, ex);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Visualizar Propiedades Validas
        /// </summary>
        /// <param name="codEquipo"></param>
        /// <returns></returns>
        public PartialViewResult VerPropiedadesValidas(int codEmpresa, int codEquipo, int codFamilia)
        {
            DetalleEquipoModel model = new DetalleEquipoModel();
            try
            {
                string valoresVacios1 = String.Join(",", ConstantesAppServicio.ListValPropDecimal);
                string valoresVacios2 = String.Join(",", ConstantesAppServicio.ListValPropFile);
                string valoresVacios3 = String.Join(",", ConstantesAppServicio.ListValPropString);

                var listDecimal = new List<string>() { ConstantesAppServicio.TipoDecimal, valoresVacios1 };
                var listNumeric = new List<string>() { ConstantesAppServicio.TipoNumerico, valoresVacios1 };
                var listNumber = new List<string>() { ConstantesAppServicio.TipoEntero, valoresVacios1 };
                var listFile = new List<string>() { ConstantesAppServicio.TipoArchivo, valoresVacios2 };
                var listN = new List<string>() { ConstantesAppServicio.TipoN, valoresVacios3 };
                var listString = new List<string>() { ConstantesAppServicio.TipoString, valoresVacios3 };

                model.ListaConstantes = new List<List<string>>() { listDecimal, listNumeric, listNumber, listFile, listN, listString };

                var ListPropiedadesValidas = servEq.ListaPropiedadesValidas(codEmpresa, codEquipo, codFamilia);

                model.ListaPropValidas = ListPropiedadesValidas;
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// devuelve listado de familia para filtro empresa
        /// </summary>
        /// <param name="idEmpresa"></param>             
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFamilias(int idEmpresa)
        {
            List<EqFamiliaDTO> entitys = servEq.ListarFamiliaXEmp(idEmpresa);

            SelectList list = new SelectList(entitys, "FamCodi", "FamNomb");

            return Json(list);
        }

        /// <summary>
        /// devuelve listado de Empresas para filtro tipo Empresa
        /// </summary>
        /// <param name="idTipoEmpresa"></param>             
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            var entitys = this.servEq.ListarEmpresasSeinFT().Where(x => idTipoEmpresa == -1 || x.Tipoemprcodi == idTipoEmpresa).ToList();
            entitys = entitys.Where(t => t.Emprestado.Trim() != "E").ToList();

            var list = new SelectList(entitys, "Emprcodi", "Emprnomb");
            return Json(list);
        }

        #endregion
    }
}
