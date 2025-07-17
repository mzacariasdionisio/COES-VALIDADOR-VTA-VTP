using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ForzadaController : BaseController
    {

        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();

        /// <summary>
        /// Muestra la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ForzadaModel model = new ForzadaModel();
            model.FechaHidroInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaHidroFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        #region Maestro de Generación Forzada

        /// <summary>
        /// Muestra el listado de maestro
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MaestroList() 
        {
            ForzadaMaestroModel model = new ForzadaMaestroModel();
            model.ListaMaestro = this.servicio.GetByCriteriaPrGenforzadaMaestros();
            return PartialView(model);        
        }

        /// <summary>
        /// Permite mostrar la venta de nuevo o edicion
        /// </summary>
        /// <param name="idMaestro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MaestroEdit(int idMaestro)
        {
            ForzadaMaestroModel model = new ForzadaMaestroModel();
            model.ListaRelacion = this.servicio.ListEqRelacions();
            model.ListaSubCausa = this.servicio.ListaSubCausaEvento();
            if (idMaestro == 0)
            {
                model.Entidad = new PrGenforzadaMaestroDTO();
                model.Entidad.Genformaecodi = 0;
                model.Entidad.Indestado = Constantes.EstadoActivo;
                model.Entidad.Genformaesimbolo = Constantes.CaracterIgual.ToString();
                model.Entidad.Relacioncodi = -1;
                model.Entidad.Genfortipo = 2.ToString();
            }
            else 
            {
                model.Entidad = this.servicio.GetByIdPrGenforzadaMaestro(idMaestro);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar los datos del maestro
        /// </summary>
        /// <param name="idMaestro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MaestroDelete(int idMaestro)
        {
            try
            {
                this.servicio.DeletePrGenforzadaMaestro(idMaestro);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar el registro de la generación forzada
        /// </summary>
        /// <param name="idCongestion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenForzadaDelete(int idGenforcodi)
        {
            try
            {
                this.servicio.DeletePrGenforzada(idGenforcodi);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la configuracion del maestro
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MaestroSave(ForzadaMaestroModel model)
        {
            try
            {
                PrGenforzadaMaestroDTO entity = new PrGenforzadaMaestroDTO
                {
                    Genformaecodi = model.Codigo,
                    Relacioncodi = model.CodigoRelacion,
                    Indestado = model.Estado,
                    Genformaesimbolo = model.Simbolo,
                    Genfortipo = model.Tipo,
                    Subcausacodi = model.Subcausa,
                    Lastdate = DateTime.Now,
                    Lastuser = base.UserName                     
                };

                int idResultado = this.servicio.SavePrGenforzadaMaestro(entity);

                return Json(idResultado);
            }
            catch 
            {
                return Json(-1);
            }
        }

        #endregion

        #region Generación Hidraulico

        /// <summary>
        /// Permite mostrar el listado de hidraulicos
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HidraulicoList(string fechaInicio, string fechaFin)
        {
            ForzadaModel model = new ForzadaModel();
            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);            

            model.ListaGeneracionForzada = this.servicio.GetByCriteriaPrGenforzadas(fecInicio, fecFin.AddDays(1));

            return PartialView(model);
        }

        /// <summary>
        /// Muestra la ventana de edicion o creacion de generacion forzada
        /// </summary>
        /// <param name="idForzada"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult HidraulicoEdit(int idHidraulico)
        {
            ForzadaModel model = new ForzadaModel();
            model.ListaRelacion = this.servicio.ListEqRelacionsHidraulico();
            model.ListaSubCausa = this.servicio.ListaSubCausaEvento();

            if (idHidraulico == 0)
            {
                model.Entidad = new PrGenforzadaDTO();
                model.Entidad.Relacioncodi = -1;
                model.FechaHidroInicio = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.FechaHidroFin = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.Entidad.Genforsimbolo = Constantes.CaracterIgual.ToString();
            }
            else
            {
                model.Entidad = this.servicio.GetByIdPrGenforzada(idHidraulico);
                model.FechaHidroInicio = ((DateTime)model.Entidad.Genforinicio).ToString(Constantes.FormatoFechaFull);
                model.FechaHidroFin = ((DateTime)model.Entidad.Genforfin).ToString(Constantes.FormatoFechaFull);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar los datos del registro
        /// </summary>
        /// <param name="idHidraulico"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HidraulicoDelete(int idHidraulico)
        {
            try
            {
                this.servicio.DeletePrGenforzada(idHidraulico);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar los datos de generación forzada Hidraulico
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HidraulicoSave(ForzadaModel model)
        {
            try
            {
                PrGenforzadaDTO entity = new PrGenforzadaDTO
                {
                    Genforcodi = model.Codigo,
                    Relacioncodi = model.CodigoRelacion,
                    Genforinicio = DateTime.ParseExact(model.FechaHidroInicio, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Genforfin = DateTime.ParseExact(model.FechaHidroFin, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Genforsimbolo = model.Simbolo,
                    Subcausacodi = model.Subcausa,
                    Lastuser = base.UserName,
                    Lastdate = DateTime.Now
                };

                int idResultado = this.servicio.SavePrGenforzada(entity);

                return Json(idResultado);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite listar los registros de Geenración Forzada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult Lista()
        {

            ForzadaModel model = new ForzadaModel();

            model.FechaHidroInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            
            return View(model);

        }

        /// <summary>
        /// Permite obtener un listado de configuración con listado de equipos (ejemplo: linea, trafo)
        /// </summary>
        /// <param name="ListCongestionLineaTrafo"></param>
        /// <param name="ListEquipo"></param>
        /// <returns></returns>
        public List<EqRelacionDTO> ObtenerConfiguracionFamilia(List<EqRelacionDTO> ListCongestionLineaTrafo, List<EqEquipoDTO> ListEquipo)
        {

            ListCongestionLineaTrafo.RemoveAll(x => x.Equicodi <= 0);

            var queryLinea = from x in ListCongestionLineaTrafo
                             join y in ListEquipo
                             on x.Equicodi equals y.Equicodi
                             select new EqRelacionDTO
                             {                                 
                                 Relacioncodi = x.Relacioncodi,
                                 Equicodi = y.Equicodi,
                                 Equinomb = y.Equinomb
                             };

            return queryLinea.ToList();
        }

        [HttpPost]
        public JsonResult ObtenerGeneracionForzada(string fechaini)
        {
            DateTime fecInicio = DateTime.ParseExact(fechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = fecInicio;
            
            ForzadaModel model = new ForzadaModel();

            List<PrGenforzadaDTO> ListaGeneracionForzada = this.servicio.GetByCriteriaPrGenforzadas(fecInicio, fecFin.AddDays(1));
            model.ListaGeneracionForzada = ListaGeneracionForzada;

            model.ListaSubCausa = this.servicio.ListaSubCausaEvento();

            //generadores
            List<EqEquipoDTO> listaEquipoGenHidro = servEquipo.ListadoEquipoNombre(ConstantesAppServicio.ParametroDefecto).Where(
                x => x.Famcodi == ConstantesCortoPlazo.IdGeneradorHidro
                    || x.Famcodi == ConstantesCortoPlazo.IdCentralHidro
                    || x.Famcodi == ConstantesCortoPlazo.IdGeneradorTermico
                    || x.Famcodi == ConstantesCortoPlazo.IdCentralTermico
                    || x.Equicodi == -1).ToList();


            model.ListaRelacion = this.servicio.GetByCriteriaEqRelacions(-1, ConstantesCortoPlazo.EstadoActivo);                                

            //congestion de lineas configuradas
            var queryLinea = ObtenerConfiguracionFamilia(model.ListaRelacion, listaEquipoGenHidro);

            List<EqRelacionDTO> ListGeneradorEquipo = queryLinea;

            model.ListEquipo = ListGeneradorEquipo.ToList();
                        


            //poblando la matriz a partir listaCongestionFinal

            int registrosTotal = 7 + (ListaGeneracionForzada.Count == 0 ? 1 : ListaGeneracionForzada.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = PintarCeldas(data, 6);
            int indice = 0;

            foreach (var item in ListaGeneracionForzada)
            {
                data[indice][0] = item.Genforcodi.ToString();

                data[indice][1] = item.Genforsimbolo.ToString();
                
                data[indice][2] = item.Relacioncodi.ToString();
                data[indice][3] = item.Subcausacodi.ToString();
                
                try
                {
                    data[indice][4] = (item.Genforinicio != null) ? ((DateTime)item.Genforinicio).ToString(Constantes.FormatoHoraMinuto) :
                                      DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                }
                catch
                {
                    
                }

                try
                {
                    data[indice][5] = (item.Genforfin != null) ? ((DateTime)item.Genforfin).ToString(Constantes.FormatoHoraMinuto) :
                                      DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                }
                catch
                {
                   
                }
                
                indice++;
            }

            model.Datos = data;
            //model.Registro = indice;
            
            return Json(model);

        }

        /// <summary>
        /// Permite pintar la data por defecto
        /// </summary>
        /// <param name="data">Matriz de datos</param>
        /// <param name="columna">Número de columnas</param>
        /// <returns></returns>
        private string[][] PintarCeldas(string[][] data, int columna)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new string[columna];

                for (int j = 0; j < columna; j++)
                {
                    data[i][j] = string.Empty;
                }
            }

            return data;
        }


        /// <summary>
        /// Permite grabar las lineas
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarForzada(string dataExcel, string fecha)
        {

            try
            {

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
                        PrGenforzadaDTO entity = new PrGenforzadaDTO();

                        string id = matriz[i][0];
                        if (id != "")
                        {
                            entity.Genforcodi = Convert.ToInt32(id);
                        }

                        entity.Genforsimbolo = matriz[i][1].ToString();

                        string relacion = matriz[i][2].ToString();

                        if (relacion != "")
                        {
                            entity.Relacioncodi = Convert.ToInt32(relacion);
                        }
                        else
                        {
                            break;
                        }

                        string subcausa = matriz[i][3].ToString();

                        if (subcausa != "")
                        {
                            entity.Subcausacodi = Convert.ToInt32(subcausa);
                        }
                        else
                        {
                            continue;
                        }

                                                

                        try
                        {
                            string horaini = matriz[i][4];
                            entity.Genforinicio = DateTime.ParseExact(fecha + " " + horaini, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
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
                                entity.Genforfin = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime fechaVigente = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                fechaVigente = fechaVigente.AddDays(1);

                                entity.Genforfin = fechaVigente;
                            }


                        }
                        catch
                        {
                            continue;
                        }


                        entity.Lastuser = base.UserName;
                        entity.Lastdate = DateTime.Now;

                        servicio.SavePrGenforzada(entity);
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



        #endregion
    }
}
