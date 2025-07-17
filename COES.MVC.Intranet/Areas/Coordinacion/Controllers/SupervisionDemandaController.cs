using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Coordinacion.Helper;
using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Coordinacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Coordinacion.Controllers
{
    public class SupervisionDemandaController : BaseController
    {
        #region Metodos para configurar relaciones

        /// <summary>
        /// Instancia de la clase de servicio correspondiente
        /// </summary>
        SupervisionDemandaAppServicio servicio = new SupervisionDemandaAppServicio();

        /// <summary>
        /// Muestra la pantalla de relaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Relacion()
        {
            RelacionModel model = new RelacionModel();
            model.ListaEmpresa = this.servicio.ListarEmpresasRelacion();
            return View(model);
        }

        /// <summary>
        /// Lista los equipos por empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEquipos(int emprcodi)
        {
            RelacionModel model = new RelacionModel();
            List<EqEquipoDTO> entitys = this.servicio.ListarEquiposPorEmpresa(emprcodi);
            SelectList list = new SelectList(entitys, EntidadPropiedad.Equicodi, EntidadPropiedad.Equinomb);

            return Json(list);
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RelacionList(int idEmpresa, string estado)
        {
            RelacionModel model = new RelacionModel();
            model.ListaRelacion = this.servicio.GetByCriteriaEqRelacions(idEmpresa, estado);

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la relacion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RelacionEdit(int idRelacion)
        {
            RelacionModel model = new RelacionModel();
            model.ListaEmpresa = this.servicio.ListarEmpresasGeneradoras();

            if (idRelacion != 0)
            {
                model.Entidad = this.servicio.GetByIdEqRelacion(idRelacion);
                model.ListaEquipo = this.servicio.ListarEquiposPorEmpresa(model.Entidad.Emprcodi);
            }
            else
            {
                model.Entidad = new EqRelacionDTO();
                model.ListaEquipo = new List<EqEquipoDTO>();
                model.Entidad.Emprcodi = -1;
                model.Entidad.Estadorvarte = Constantes.EstadoActivo;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RelacionDelete(int idRelacion)
        {
            try
            {
                this.servicio.DeleteEqRelacion(idRelacion);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la relación de equivalencia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RelacionSave(RelacionModel model)
        {
            try
            {
                EqRelacionDTO entity = new EqRelacionDTO
                {
                    Relacioncodi = model.Relacioncodi,
                    Equicodi = model.Equicodi,
                    Nombarra = model.Nombarra,
                    Idgener = model.Idgener,
                    Estadorvarte = model.Estado,
                    Canalcodi = model.Canalcodi,
                    Canaliccp = model.Canaliccp,
                    Indrvarte = Constantes.SI,
                    Canaliccpint = model.Canaliccpint,
                    Canalsigno = model.Canalsigno,
                    Canaluso = model.Canaluso,
                    Lastuser = base.UserName                    
                };

                int result = this.servicio.SaveEqRelacion(entity);

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        #endregion

        #region Metodos para mostrar resultados

        /// <summary>
        /// Muestra la ventana de consulta
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SupervisionDemandaModel model = new SupervisionDemandaModel();
            List<EqRelacionDTO> entitys = this.servicio.ObtenerGruposSupervisionDemanda();
            model.ListaGrupos = entitys;
            return View(model);
        }

        /// <summary>
        /// Mustra en listado con los datos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Grilla(int tipo, int idGrupo, int tipoGeneracion)
        {
            SupervisionDemandaModel model = new SupervisionDemandaModel();
            int[] indices = null;
            string[][] datos = this.servicio.ObtenerGrillaDatosSupDemanda(tipo, idGrupo, out indices, tipoGeneracion);
            model.Datos = datos;
            model.Indices = indices;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.ContentType = "application/json";
            result.Content = serializer.Serialize(model);
            return result;
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int tipo, int idGrupo, int tipoGeneracion)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + COES.MVC.Intranet.Areas.Coordinacion.Helper.ConstantesCoordinacion.RutaReporte;
                string file = COES.MVC.Intranet.Areas.Coordinacion.Helper.ConstantesCoordinacion.ReporteSupervisionDemanda;                
                string[][] datos = this.servicio.ObtenerGrillaDatosSupDemandaExportado(tipo, idGrupo, tipoGeneracion);
                HelperCoordinacion.GenerarReporteSupervisionDemandaExcel(datos, path, file);                

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + COES.MVC.Intranet.Areas.Coordinacion.Helper.ConstantesCoordinacion.RutaReporte +
                COES.MVC.Intranet.Areas.Coordinacion.Helper.ConstantesCoordinacion.ReporteSupervisionDemanda;
            return File(fullPath, Constantes.AppExcel, COES.MVC.Intranet.Areas.Coordinacion.Helper.ConstantesCoordinacion.ReporteSupervisionDemanda);
        }


        #endregion
    }
}
