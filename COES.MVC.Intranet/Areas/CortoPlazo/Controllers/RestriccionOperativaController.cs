using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.OperacionesVarias;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class RestriccionOperativaController : BaseController
    {

        /// <summary>
        /// Instancias de las clases servicio
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();
        OperacionesVariasAppServicio servOpVarias = new OperacionesVariasAppServicio();

        /// <summary>
        /// Permite listar los registros de los Nodales
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult Index()
        {
            RestriccionOperativaModel model = new RestriccionOperativaModel();
            model.ListaEmpresa = this.servicio.ListarEmpresasRelacion();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Permite obtener datos de operaciones varias Nodales. También considera listado de Generadores nodales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataOperacionNodal(int empresa, string fecha)
        {
            RestriccionOperativaModel model = new RestriccionOperativaModel();
            DateTime fechaInicio = DateTime.Now;

            if (fecha != null)
            {
                fechaInicio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            model.ListGeneradorEquipo = this.servicio.ListarEquiposPorEmpresa(empresa);
            model.ListaIeodcuadro = servOpVarias.BuscarOperaciones(ConstantesOperacionesVarias.EvenclasecodiPdiario, 0,
                fechaInicio, fechaInicio, -1, -1).Where(x => x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaFija ||
                                                             x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaMax ||
                                                             x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaMin ||
                                                             x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPlenacarga).ToList();

            //subcausa
            List<EveSubcausaeventoDTO> ListaEvensubcausacodi = servOpVarias.ListarSubcausaevento(ConstantesOperacionesVarias.EvenSubcausa).
                Where(x => x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaFija ||
                           x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaMax ||
                           x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaMin ||
                           x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPlenacarga).ToList();
            model.ListaEvensubcausa = ListaEvensubcausacodi.Select(x => x.Subcausaabrev).ToList();

            int registrosTotal = 7 + (model.ListaIeodcuadro.Count == 0 ? 1 : model.ListaIeodcuadro.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 7);
            int indice = 0;

            foreach (var item in model.ListaIeodcuadro)
            {

                try
                {
                    string cad = "";
                    cad = (model.ListGeneradorEquipo.Where(x => x.Equicodi == item.Equicodi).ToList())[0].Equinomb;
                    data[indice][1] = cad;

                    data[indice][0] = item.Iccodi.ToString();
                    data[indice][6] = item.Iccodi.ToString();

                    if (item.Equicodi == null)
                        item.Equicodi = -1;

                    data[indice][1] = item.Equicodi.ToString();

                    try
                    {
                        cad = (ListaEvensubcausacodi.Where(x => x.Subcausacodi == item.Subcausacodi).ToList())[0].Subcausaabrev;
                        data[indice][2] = cad;
                    }
                    catch
                    {
                        data[indice][2] = ConstantesOperacionesVarias.SubcausacodiPotenciaMax.ToString();
                    }

                    try
                    {
                        data[indice][3] = item.Icvalor1 == null ? "0" : item.Icvalor1.ToString();
                    }
                    catch
                    {
                        data[indice][3] = "0";
                    }

                    try
                    {
                        data[indice][4] = (item.Ichorini != null) ? ((DateTime)item.Ichorini).ToString(Constantes.FormatoHoraMinuto) :
                                          DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                    }
                    catch
                    {
                        data[indice][4] = "00:00";
                    }

                    try
                    {
                        data[indice][5] = (item.Ichorfin != null) ? ((DateTime)item.Ichorfin).ToString(Constantes.FormatoHoraMinuto) :
                                          DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                    }
                    catch
                    {
                        data[indice][5] = "00:00";
                    }

                    indice++;
                }
                catch
                {

                }
            }

            model.Datos = data;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar las lineas
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarOperacionNodal(string dataExcel, string fecha)
        {
            try
            {
                List<EqEquipoDTO> ListGeneradorEquipo = this.servicio.ListarEquiposPorEmpresa(-1);

                //lista de subcausa nodales                
                List<EveSubcausaeventoDTO> ListaEvensubcausacodi = servOpVarias.ListarSubcausaevento(ConstantesOperacionesVarias.EvenSubcausa).
                    Where(x => x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaFija ||
                    x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaMax ||
                    x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPotenciaMin ||
                    x.Subcausacodi == ConstantesOperacionesVarias.SubcausacodiPlenacarga).ToList();


                int nroColumnas = 6;

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
                        EveIeodcuadroDTO entity = new EveIeodcuadroDTO();

                        string id = matriz[i][0];
                        if (id != "")
                        {
                            entity.Iccodi = Convert.ToInt32(id);
                        }

                        string equipo = matriz[i][1];
                        if (equipo != "")
                        {
                            // es número
                            try
                            {
                                entity.Equicodi = Convert.ToInt32(equipo);
                                entity.Emprcodi = (int)(ListGeneradorEquipo.Where(x => x.Equicodi == Convert.ToInt32(equipo)).ToList())[0].Emprcodi;
                            }
                            catch
                            {
                                // es texto
                                try
                                {
                                    int equicodi = (ListGeneradorEquipo.Where(x => x.Equinomb == equipo).ToList())[0].Equicodi;
                                    entity.Equicodi = equicodi;
                                    int emprcodi = (int)(ListGeneradorEquipo.Where(x => x.Equinomb == equipo).ToList())[0].Emprcodi;
                                    entity.Emprcodi = emprcodi;
                                }
                                catch
                                {
                                    entity.Equicodi = -1;
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }

                        try
                        {
                            string subcausaAbrev = matriz[i][2];
                            int subcausaCodi = (ListaEvensubcausacodi.Where(x => x.Subcausaabrev == subcausaAbrev).ToList())[0].Subcausacodi;
                            entity.Subcausacodi = subcausaCodi;
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(matriz[i][3]))
                            {
                                decimal valor1 = Convert.ToDecimal(matriz[i][3]);

                                if (valor1 < 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    entity.Icvalor1 = valor1;
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            string horaini = matriz[i][4];
                            entity.Ichorini = DateTime.ParseExact(fecha + " " + horaini, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            string horafin = matriz[i][5];

                            if (horafin != "00:00")
                            {
                                entity.Ichorfin = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime fechaVigente = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                fechaVigente = fechaVigente.AddDays(1);

                                entity.Ichorfin = fechaVigente;
                            }
                        }
                        catch
                        {
                            continue;
                        }

                        entity.Evenclasecodi = ConstantesOperacionesVarias.EvenclasecodiPdiario;
                        entity.Lastuser = base.UserName;
                        entity.Lastdate = DateTime.Now;

                        servOpVarias.Save(entity);
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

        /// <summary>
        /// Permite eliminar un registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servOpVarias.DeleteOperacion(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
