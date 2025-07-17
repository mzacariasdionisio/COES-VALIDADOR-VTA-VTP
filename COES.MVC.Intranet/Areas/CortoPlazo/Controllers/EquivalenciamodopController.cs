using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using COES.Servicios.Aplicacion.Helper;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.Servicios.Aplicacion.Despacho;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class EquivalenciamodopController : BaseController
    {
        CortoPlazoAppServicio servCortoPlazo = new CortoPlazoAppServicio();
        DespachoAppServicio servGrupo = new DespachoAppServicio();

        public ActionResult Index()
        {

            BusquedaCmEquivalenciamodopModel model = new BusquedaCmEquivalenciamodopModel();
            model.ListaPrGrupo = servGrupo.ListaModosOperacionActivos();
            model.ListaPrGrupo.Sort((m1, m2) => string.Compare(m1.Gruponomb, m2.Gruponomb));

            return View(model);
        }


        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult ListaGlobal()
        {
            CmEquivalenciamodopModel model = new CmEquivalenciamodopModel();            
            return View(model);
        }


        /// <summary>
        /// Permite obtener datos de linea-trafo 2D y 3D. También considera listaod de Lineas y grupos de líneas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataEquivalencia()
        {
            CmEquivalenciamodopModel model = new CmEquivalenciamodopModel();

            List<CmEquivalenciamodopDTO> listEquivalencia = this.servCortoPlazo.GetByCriteriaCmEquivalenciamodops();// ivalenciamodops GetByCriteriaEqGrupoLineas();

            int registrosTotal = (listEquivalencia.Count == 0 ? 1 : listEquivalencia.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 4);


            int indice = 0;

            foreach (var item in listEquivalencia)
            {
                data[indice][0] = (item.Equimocodi == 0) ? "" : item.Equimocodi.ToString();
                data[indice][1] = item.Grupocodi.ToString();
                data[indice][2] = item.Gruponomb;
                data[indice][3] = (item.Equimonombrencp == null) ? "" : item.Equimonombrencp.ToString();
                                
                indice++;
            }

            model.Datos = data;
            
            return new JsonResult { Data = model, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GrabarListGlobal(string dataExcel)
        {
            try
            {
                int nroColumnas = 4;
                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();
                string[][] matriz = CalculoHelper.GetMatrizExcel(celdas, 0, nroColumnas, 0, celdas.Count / nroColumnas);
                int filaFinal = 0;
                int filas = (celdas.Count / nroColumnas);
                for (int i = 0; i < filas; i++)
                {
                    int conteoBlanco = 0;
                    for (int j = 0; j < nroColumnas; j++)
                    {
                        conteoBlanco += (matriz[i][j] == "" ? 1 : 0);
                    }

                    if (conteoBlanco == nroColumnas)
                        return Json(1);
                    else
                    {
                        CmEquivalenciamodopDTO entity = new CmEquivalenciamodopDTO();

                        if (matriz[i][0]!="")
                        {
                            entity.Equimocodi = Convert.ToInt32(matriz[i][0]);
                        }

                        
                            entity.Grupocodi= Convert.ToInt32( matriz[i][1]);

                            if (matriz[i][3] == "")
                                continue;

                            entity.Equimonombrencp = matriz[i][3];
                 

                        
                            entity.Equimousumodificacion = base.UserName;
                            entity.Equimofecmodificacion = DateTime.Now;
                        

                        if (entity.Equimocodi == 0)
                        {
                            servCortoPlazo.SaveCmEquivalenciamodop(entity);
                        }
                        else
                        {
                            servCortoPlazo.UpdateCmEquivalenciamodop(entity);
                        }
                    }

                    filaFinal++;
                }
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        [HttpPost]
        public PartialViewResult Lista(int grupocodi, int nroPage)
        {
            BusquedaCmEquivalenciamodopModel model = new BusquedaCmEquivalenciamodopModel();

            model.ListaCmEquivalenciamodop = servCortoPlazo.BuscarOperaciones(grupocodi, nroPage, Constantes.PageSizeEvento).ToList();
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }


        [HttpPost]
        public PartialViewResult Paginado(int grupocodi)
        {
            Paginacion model = new Paginacion();

            int nroRegistros = servCortoPlazo.ObtenerNroFilas(grupocodi);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return base.Paginado(model);
        }
    }
}
