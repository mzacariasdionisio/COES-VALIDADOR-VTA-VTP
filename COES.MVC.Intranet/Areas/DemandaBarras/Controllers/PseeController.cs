using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaBarras.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.DemandaBarras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaBarras.Controllers
{
    public class PseeController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio correspondiente
        /// </summary>
        PseeAppServicio servicio = new PseeAppServicio();
        
        /// <summary>
        /// Muestra la pantalla de relaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Relacion()
        {
            PseeModel model = new PseeModel();
            model.ListaEmpresa = this.servicio.ListarEmpresasRelacion();
            return View(model);
        }

        /// <summary>
        /// Lista los equipos por empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEquipos(int emprcodi, int famcodi)
        {
            PseeModel model = new PseeModel();
            List<EqEquipoDTO> entitys = this.servicio.ListarEquiposPorEmpresa(emprcodi, famcodi);
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
            PseeModel model = new PseeModel();
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
            PseeModel model = new PseeModel();
            model.ListaEmpresa = this.servicio.ListarEmpresasSein();
            model.ListaFamilia = this.servicio.ListarFamilias();

            if (idRelacion != 0)
            {
                model.Entidad = this.servicio.GetByIdEqRelacion(idRelacion);
                model.ListaEquipo = this.servicio.ListarEquiposPorEmpresa(model.Entidad.Emprcodi, model.Entidad.Famcodi);
            }
            else
            {
                model.Entidad = new EqRelacionDTO();
                model.ListaEquipo = new List<EqEquipoDTO>();
                model.Entidad.Emprcodi = -1;
                model.Entidad.Famcodi = -1;
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
        public JsonResult RelacionSave(PseeModel model)
        {
            try
            {
                EqRelacionDTO entity = new EqRelacionDTO
                {
                    Relacioncodi = model.Relacioncodi,
                    Equicodi = model.Equicodi,
                    Nombarra = model.Nombarra,                    
                    Estado = model.Estado
                };

                int result = this.servicio.SaveEqRelacion(entity);

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }    

    }
}
