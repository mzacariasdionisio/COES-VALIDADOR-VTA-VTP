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
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class OperacionregistroController : BaseController
    {
        DespachoAppServicio servGrupo = new DespachoAppServicio();
        CortoPlazoAppServicio servCortoPlazo = new CortoPlazoAppServicio();

        /// <summary>
        /// Permite listar los registros de Operación registro
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult Lista()
        {

            CmOperacionregistroModel model = new CmOperacionregistroModel();
            model.OperegFecinicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);

        }


        /// <summary>
        /// Permite eliminar el registro de operación registro
        /// </summary>
        /// <param name="idCongestion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OperacionRegistroDelete(int id)
        {
            try
            {
                servCortoPlazo.DeleteCmOperacionregistro(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite grabar las lineas
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(string dataExcel, string fecha)
        {

            try
            {

                int nroColumnas = 5;

                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();

                string[][] matriz = CalculoHelper.GetMatrizExcel(celdas, 0, nroColumnas, 0, celdas.Count / nroColumnas);


                int filaFinal = 0;
                int filas = (celdas.Count / nroColumnas);

                for (int i = 0; i < filas; i++)
                {
                    //todas son blanco?
                    int conteoBlanco = 0;
                    for (int j = 0; j < nroColumnas; j++)
                    {
                        conteoBlanco += (matriz[i][j] == "" ? 1 : 0);
                    }

                    if (conteoBlanco == nroColumnas)
                    {
                        return Json(1);
                    }
                    else
                    {
                        //insertar fila
                        CmOperacionregistroDTO entity = new CmOperacionregistroDTO();

                        string id = matriz[i][0];
                        if (id != "")
                        {
                            entity.Operegcodi = Convert.ToInt32(id);
                        }

                        string grupocodi = matriz[i][1].ToString();

                        if (grupocodi != "")
                        {
                            entity.Grupocodi = Convert.ToInt32(grupocodi);
                        }
                        else
                        {
                            break;
                        }

                        string subcausa = matriz[i][2].ToString();

                        if (subcausa != "")
                        {
                            entity.Subcausacodi = Convert.ToInt32(subcausa);
                        }
                        else
                        {
                            entity.Subcausacodi = null;
                        }

                        try
                        {
                            string horaini = matriz[i][3];
                            entity.Operegfecinicio = DateTime.ParseExact(fecha + " " + horaini, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            string horafin = matriz[i][4];

                            if (horafin != "00:00")
                            {
                                entity.Operegfecfin = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime fechaVigente = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                fechaVigente = fechaVigente.AddDays(1);

                                entity.Operegfecfin = fechaVigente;
                            }


                        }
                        catch
                        {
                            continue;
                        }


                        entity.Operegusumodificacion = base.UserName;
                        entity.Operegfecmodificacion = DateTime.Now;

                        if (entity.Operegcodi == 0)
                        {
                            servCortoPlazo.SaveCmOperacionregistro(entity);
                        }
                        else
                        {
                            servCortoPlazo.UpdateCmOperacionregistro(entity);
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
        public JsonResult ObtenerOperacionRegistro(string fechaini)
        {
            DateTime fecInicio = DateTime.ParseExact(fechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = fecInicio;

            CmOperacionregistroModel model = new CmOperacionregistroModel();

            List<CmOperacionregistroDTO> ListaOperacionRegistro = this.servCortoPlazo.BuscarOperaciones(0, 0, fecInicio, fecFin.AddDays(1), -1, -1);
            model.ListaOperacionRegistro = ListaOperacionRegistro;


            model.ListaPrGrupo = servGrupo.ListaModosOperacionActivos().OrderBy(x => x.Gruponomb).ToList();



            model.ListaOperacionRegistro = ListaOperacionRegistro;
            model.ListaEveSubcausaevento = this.servCortoPlazo.ListaSubCausaEvento();

            int registrosTotal = 7 + (ListaOperacionRegistro.Count == 0 ? 1 : ListaOperacionRegistro.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 5);
            int indice = 0;

            foreach (var item in ListaOperacionRegistro)
            {
                data[indice][0] = item.Operegcodi.ToString();

                data[indice][1] = item.Grupocodi.ToString();

                data[indice][2] = item.Subcausacodi.ToString();


                try
                {
                    data[indice][3] = (item.Operegfecinicio != null) ? ((DateTime)item.Operegfecinicio).ToString(Constantes.FormatoHoraMinuto) :
                                      DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                }
                catch
                {

                }

                try
                {
                    data[indice][4] = (item.Operegfecfin != null) ? ((DateTime)item.Operegfecfin).ToString(Constantes.FormatoHoraMinuto) :
                                      DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                }
                catch
                {

                }

                indice++;
            }

            model.Datos = data;

            return Json(model);

        }


        [HttpPost]
        public PartialViewResult Lista(int grupocodi, int subcausaCodi, string operegFecinicio, string operegFecfin, int nroPage)
        {
            BusquedaCmOperacionregistroModel model = new BusquedaCmOperacionregistroModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (operegFecinicio != null)
            {
                fechaInicio = DateTime.ParseExact(operegFecinicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (operegFecfin != null)
            {
                fechaFinal = DateTime.ParseExact(operegFecfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFinal = fechaFinal.AddDays(1);

            model.ListaCmOperacionregistro = servCortoPlazo.BuscarOperaciones(grupocodi, subcausaCodi, fechaInicio, fechaFinal, nroPage, Constantes.PageSizeEvento).ToList();
            model.AccionEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.AccionEliminar = base.VerificarAccesoAccion(Acciones.Eliminar, base.UserName);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el paginado
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="subcausaCodi"></param>
        /// <param name="operegFecinicio"></param>
        /// <param name="operegFecfin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int grupocodi, int subcausaCodi, string operegFecinicio, string operegFecfin)
        {
            Paginacion model = new Paginacion();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;

            if (operegFecinicio != null)
            {
                fechaInicio = DateTime.ParseExact(operegFecinicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (operegFecfin != null)
            {
                fechaFinal = DateTime.ParseExact(operegFecfin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            fechaFinal = fechaFinal.AddDays(1);

            int nroRegistros = servCortoPlazo.ObtenerNroFilas(grupocodi, subcausaCodi, fechaInicio, fechaFinal);

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

        /// <summary>
        /// Permite obtener la congestión programada en el NCP
        /// </summary>
        /// <param name="fechaini"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerRegistroProgramado(string fechaini)
        {
            DateTime fechaProceso = DateTime.ParseExact(fechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<CmOperacionregistroDTO> listaProgramado = servCortoPlazo.ObtenerOperacionRegistroProgramado(fechaProceso);

            List<string[]> list = new List<string[]>();

            foreach (CmOperacionregistroDTO item in listaProgramado)
            {
                string[] itemArray = new string[5];
                itemArray[0] = string.Empty;
                itemArray[1] = item.Grupocodi.ToString();
                itemArray[3] = (item.Operegfecinicio != null) ? ((DateTime)item.Operegfecinicio).ToString(Constantes.FormatoHoraMinuto) : string.Empty;
                itemArray[4] = (item.Operegfecfin != null) ? ((DateTime)item.Operegfecfin).ToString(Constantes.FormatoHoraMinuto) : string.Empty;
                list.Add(itemArray);
            }

            return Json(list);
        }
    }
}
