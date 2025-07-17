using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class CongestionController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio respectiva
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();

        /// <summary>
        /// Muestra la pagina de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CongestionModel model = new CongestionModel();
            model.FechaEjecutadoInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaEjecutadoFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el listado de congestiones registradas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EjecutadoList(string fechaInicio, string fechaFin)
        {
            CongestionModel model = new CongestionModel();

            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaCongestionSimple = this.servicio.ObtenerCongestionSimple(fecInicio, fecFin);
            model.ListaCongestionConjunto = this.servicio.ObtenerCongestionConjunto(fecInicio, fecFin);

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar el registro de la congestion
        /// </summary>
        /// <param name="idCongestion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutadoDelete(int idCongestion)
        {
            try
            {
                this.servicio.DeletePrCongestion(idCongestion);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite editar los datos de congestion o agregar uno nuevo
        /// </summary>
        /// <param name="idCongestion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EjecutadoEdit(int idCongestion)
        {
            CongestionModel model = new CongestionModel();
            model.ListaGrupo = new List<EqGrupoLineaDTO>();
            model.ListaLinea = this.servicio.ListEqCongestionConfigs();
            model.ListaBarra1 = new List<string>();
            model.ListaBarra2 = new List<string>();

            if (idCongestion == 0)
            {
                model.Entidad = new PrCongestionDTO();
                model.Entidad.Indtipo = Constantes.SI;
                model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFechaFull);
                model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaFull);
            }
            else
            {
                model.Entidad = this.servicio.GetByIdPrCongestion(idCongestion);

                EqCongestionConfigDTO entity = this.servicio.GetByIdEqCongestionConfig((int)model.Entidad.Configcodi);
                List<string> barrasA = new List<string>();
                List<string> barrasB = new List<string>();
                model.ListaBarra1 = barrasA;
                model.ListaBarra2 = barrasB;

                model.FechaInicio = ((DateTime)model.Entidad.Congesfecinicio).ToString(Constantes.FormatoFechaFull);
                model.FechaFin = ((DateTime)model.Entidad.Congesfecfin).ToString(Constantes.FormatoFechaFull);

            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos de la congestion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutadoSave(CongestionModel model)
        {
            try
            {
                PrCongestionDTO entity = new PrCongestionDTO
                {
                    Congescodi = model.Codigo,
                    Congesfecinicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Congesfecfin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture),
                    Configcodi = model.CodigoLinea,
                    Grulincodi = model.CodigoGrupo,
                    Indtipo = model.IndTipo,
                    Lastdate = DateTime.Now,
                    Lastuser = base.UserName
                };

                int idResultado = this.servicio.SavePrCongestion(entity);
                return Json(idResultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite cargar las barras
        /// </summary>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarBarras(int idLinea)
        {
            CongestionModel model = new CongestionModel();
            EqCongestionConfigDTO entity = this.servicio.GetByIdEqCongestionConfig(idLinea);
            List<string> barrasA = new List<string>();
            List<string> barrasB = new List<string>();
            model.ListaBarra1 = barrasA;
            model.ListaBarra2 = barrasB;

            return Json(model);
        }

        /// <summary>
        /// Permite obtener un listado de configuración con listado de equipos (ejemplo: linea, trafo)
        /// </summary>
        /// <param name="ListCongestionLineaTrafo"></param>
        /// <param name="ListEquipo"></param>
        /// <returns></returns>
        public List<EqCongestionConfigDTO> ObtenerConfiguracionFamilia(List<EqCongestionConfigDTO> ListCongestionLineaTrafo, List<EqEquipoDTO> ListEquipo)
        {
            ListCongestionLineaTrafo.RemoveAll(x => x.Equicodi <= 0);

            var queryLinea = from x in ListCongestionLineaTrafo
                             join y in ListEquipo
                             on x.Equicodi equals y.Equicodi
                             select new EqCongestionConfigDTO
                             {
                                 //Configcodi = (int)x.Grulincodi,
                                 Configcodi = x.Configcodi,
                                 Equicodi = y.Equicodi,
                                 Equinomb = y.Equinomb
                             };

            return queryLinea.ToList();
        }

        /// <summary>
        /// Permite obtener datos de linea-trafo 2D y 3D y grupos de línea que se encuentran configuradas.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerLineaTrafoGrupoConfigurado(string fechaini)
        {
            DateTime fecInicio = DateTime.ParseExact(fechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = fecInicio;// DateTime.ParseExact(fechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            CongestionModel model = new CongestionModel();

            //lista de línea trafo2D y trafo 3D
            List<EqCongestionConfigDTO> ListCongestionLineaTrafo = this.servicio.GetByCriteriaEqCongestionConfigs(-1, "ACTIVO", -1, -1).ToList().OrderBy(x => x.Configcodi).ToList();

            // lista de líneas (equipamiento)
            List<EqEquipoDTO> listaEqBD = servEquipo.ListadoEquipoNombre("8,9,10");
            listaEqBD.Add(new EqEquipoDTO() { Equicodi = -1, Famcodi = -1, Equinomb = "_N (NO-DEF) (_NODEF - _NO DEFINIDO)" });
            List<EqEquipoDTO> listaEquipoLin = listaEqBD.Where(
                x => x.Famcodi == ConstantesCortoPlazo.IdLineaTransmision || x.Equicodi == -1).ToList();

            // Trafo2D
            List<EqEquipoDTO> listaEquipoTrafo2d = listaEqBD.Where(
                x => x.Famcodi == ConstantesCortoPlazo.IdTrafo2D || x.Equicodi == -1).ToList();

            // Trafo 3D
            List<EqEquipoDTO> listaEquipoTrafo3d = listaEqBD.Where(
                x => x.Famcodi == ConstantesCortoPlazo.IdTrafo3D || x.Equicodi == -1).ToList();

            //congestion de lineas configuradas
            var queryLinea = ObtenerConfiguracionFamilia(ListCongestionLineaTrafo, listaEquipoLin);
            var querytrafo2d = ObtenerConfiguracionFamilia(ListCongestionLineaTrafo, listaEquipoTrafo2d);
            var querytrafo3d = ObtenerConfiguracionFamilia(ListCongestionLineaTrafo, listaEquipoTrafo3d);

            List<EqCongestionConfigDTO> ListLineaEquipo = queryLinea;
            List<EqCongestionConfigDTO> ListTrafo2dEquipo = querytrafo2d;
            List<EqCongestionConfigDTO> ListTrafo3dEquipo = querytrafo3d;

            model.ListLineaEquipo = ListLineaEquipo;
            model.ListTrafo2dEquipo = ListTrafo2dEquipo;
            model.ListTrafo3dEquipo = ListTrafo3dEquipo;

            //lista de grupo de línea
            List<EqGrupoLineaDTO> listaGrupoLinea = this.servicio.GetByCriteriaEqGrupoLineas("1");
            foreach (EqGrupoLineaDTO item in listaGrupoLinea)
            {
                item.Grulincodi += ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea;
            }

            model.ListGrupoLinea = listaGrupoLinea;


            //lista de regiones de seguridad
            List<CmRegionseguridadDTO> listaRegiones = this.servicio.GetByCriteriaCmRegionseguridads();

            foreach (CmRegionseguridadDTO item in listaRegiones)
            {
                item.Regsegcodi += ConstantesCortoPlazo.ConstanteDesfaseRegionSeguridad;
            }

            model.ListaRegionSeguridad = listaRegiones;

            //lista de grupo de líneas de flujo mínimo
            List<EqGrupoLineaDTO> listaGrupoLineaMinimo = this.servicio.GetByCriteriaEqGrupoLineas("2");
            foreach (EqGrupoLineaDTO item in listaGrupoLineaMinimo)
            {
                item.Grulincodi += ConstantesCortoPlazo.DesfaceGrupoLineaFlujoMuinimo;
            }

            model.ListaGrupoLineaMinimo = listaGrupoLineaMinimo;


            //servicio
            List<PrCongestionDTO> listaCongestion = servicio.ObtenerCongestion(fecInicio, fecFin);
            List<PrCongestionDTO> listaCongestionFinal = new List<PrCongestionDTO>();

            //creando la lista de congestión completa
            foreach (PrCongestionDTO item in listaCongestion)
            {
                string equinomb = "";

                PrCongestionDTO entity = new PrCongestionDTO();

                entity.Congescodi = item.Congescodi;
                entity.Congesfecinicio = item.Congesfecinicio;
                entity.Congesfecfin = item.Congesfecfin;
                entity.Congesmotivo = item.Congesmotivo;
                entity.Iccodi = item.Iccodi;
                entity.ListaGrupoDespacho = item.ListaGrupoDespacho;
                //es un grupo de linea?
                if (item.Grulincodi != null && item.Indtipo == ConstantesCortoPlazo.CongestionCompuesta)
                {
                    if (item.Grulintipo == "1")
                    {
                        entity.Grulincodi = ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea + (int)item.Grulincodi;
                        entity.Fuente = ConstantesCortoPlazo.TxtFuenteGrupoLinea;
                        entity.Configcodi = ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea + (int)item.Grulincodi;
                        entity.Regsegcodi = ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea + (int)item.Grulincodi;
                    }
                    else if(item.Grulintipo == "2")
                    {
                        entity.Grulincodi = ConstantesCortoPlazo.DesfaceGrupoLineaFlujoMuinimo + (int)item.Grulincodi;
                        entity.Fuente = ConstantesCortoPlazo.TxtFuenteGrupoLineaMinimo;
                        entity.Configcodi = ConstantesCortoPlazo.DesfaceGrupoLineaFlujoMuinimo + (int)item.Grulincodi;
                        entity.Regsegcodi = ConstantesCortoPlazo.DesfaceGrupoLineaFlujoMuinimo + (int)item.Grulincodi;
                    }
                    listaCongestionFinal.Add(entity);
                }
                else if (item.Regsegcodi != null && item.Indtipo == ConstantesCortoPlazo.CongestionRegionSeguridad)
                {
                    entity.Grulincodi = ConstantesCortoPlazo.ConstanteDesfaseRegionSeguridad + (int)item.Regsegcodi;                   
                    entity.Fuente = ConstantesCortoPlazo.TxtFuenteRegionSeguridad;                   
                    entity.Configcodi = ConstantesCortoPlazo.ConstanteDesfaseRegionSeguridad + (int)item.Regsegcodi;
                    entity.Regsegcodi = ConstantesCortoPlazo.ConstanteDesfaseRegionSeguridad + (int)item.Regsegcodi;

                    listaCongestionFinal.Add(entity);
                }
                else if(item.Indtipo == ConstantesCortoPlazo.CongestionSimple){
                    //es un equipo?
                    if (item.Configcodi != null)
                    {
                        entity.Configcodi = (int)item.Configcodi;

                        try
                        {
                            EqCongestionConfigDTO congestion = (ListCongestionLineaTrafo.Where(x => x.Configcodi == item.Configcodi).ToList())[0];

                            if (congestion.Equicodi != null && congestion.Equicodi > 0)
                            {
                                int equicodi = congestion.Equicodi;

                                //es linea?                                
                                int linea = ListLineaEquipo.Where(x => x.Equicodi == equicodi).Count();

                                if (linea > 0)
                                {
                                    entity.Equicodi = item.Equicodi; //entity.Equinomb = equipoLin.Equinomb;
                                    entity.Fuente = ConstantesCortoPlazo.TxtFuenteLinea;//ConstantesCortoPlazo.IdFuenteLinea.ToString(); //"LINEA";                                  
                                    entity.Configcodi = item.Configcodi;
                                    listaCongestionFinal.Add(entity);
                                }
                                else
                                {
                                    //es trafo 2d?
                                    int trafo2d = ListTrafo2dEquipo.Where(x => x.Equicodi == equicodi).Count();

                                    if (trafo2d > 0)
                                    {
                                        entity.Equicodi = item.Equicodi; //string equinomb = equipoTr2d.Equinomb;
                                        entity.Fuente = ConstantesCortoPlazo.TxtFuenteTrafo2D;// //ConstantesCortoPlazo.IdFuenteTrafo2D.ToString(); //"TRAFO2D";                                        
                                        entity.Configcodi = item.Configcodi;
                                        listaCongestionFinal.Add(entity);
                                    }
                                    else
                                    {
                                        //es trafo 3d?
                                        int trafo3d = ListTrafo3dEquipo.Where(x => x.Equicodi == equicodi).Count();

                                        if (trafo3d > 0)
                                        {
                                            EqCongestionConfigDTO equipoTr3d = ListTrafo3dEquipo.Where(x => x.Equicodi == equicodi).ToList()[0];
                                            entity.Equicodi = item.Equicodi; //string equinomb = equipoTr3d.Equinomb;
                                            entity.Fuente = ConstantesCortoPlazo.TxtFuenteTrafo3D; //ConstantesCortoPlazo.IdFuenteTrafo3D.ToString();//"TRAFO3D";                                         
                                            entity.Configcodi = item.Configcodi;
                                            listaCongestionFinal.Add(entity);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        { }
                    }
                }
            }

            //poblando la matriz a partir listaCongestionFinal

            int registrosTotal = 7 + (listaCongestionFinal.Count == 0 ? 1 : listaCongestionFinal.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = PintarCeldas(data, 7);
            int indice = 0;

            foreach (var item in listaCongestionFinal)
            {
                data[indice][0] = item.Congescodi.ToString();

                if (item.Equicodi == null)
                    item.Equicodi = -1;
                data[indice][1] = ((int)item.Configcodi).ToString();

                try
                {
                    data[indice][2] = (item.Congesfecinicio != null) ? ((DateTime)item.Congesfecinicio).ToString(Constantes.FormatoHoraMinuto) :
                                      DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                }
                catch
                {
                }

                try
                {
                    data[indice][3] = (item.Congesfecfin != null) ? ((DateTime)item.Congesfecfin).ToString(Constantes.FormatoHoraMinuto) :
                                      DateTime.Now.ToString(Constantes.FormatoHoraMinuto);
                }
                catch
                {
                }

                data[indice][4] = item.Congesmotivo;
                data[indice][5] = (item.Iccodi != null) ? item.Iccodi.ToString() : string.Empty;

                if (item.ListaGrupoDespacho != null)
                {
                    if (item.ListaGrupoDespacho.Count > 0)
                    {
                        data[indice][6] = string.Join("#", item.ListaGrupoDespacho);
                    }
                }

                indice++;
            }

            model.Datos = data;
            model.Registro = indice;

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

        public ActionResult ListaManualPorFecha(string fecha, int flagHoraTR, string horaTR)
        {
            CongestionModel model = new CongestionModel();
            model.FechaInicio = fecha;
            model.FechaFin = fecha;
            model.FlagHoraTR = flagHoraTR;
            model.HoraTR = horaTR;

            model.ListaGrupoDespacho = (new Servicios.Aplicacion.OperacionesVarias.OperacionesVariasAppServicio()).
                ListarGrupo().Where(x => x.Grupoactivo == Constantes.SI).ToList();

            return View(model);
        }

        /// <summary>
        /// Permite listar los registros de los Nodales
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult ListaManual()
        {
            CongestionModel model = new CongestionModel();

            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaGrupoDespacho = (new Servicios.Aplicacion.OperacionesVarias.OperacionesVariasAppServicio()).
                ListarGrupo().Where(x => x.Grupoactivo == Constantes.SI).ToList();

            return View(model);
        }

        /// <summary>
        /// Permite grabar las lineas
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCongestionManual(string dataExcel, string fecha)
        {
            try
            {
                int nroColumnas = 7;
                List<string> celdas = new List<string>();
                celdas = dataExcel.Split(',').ToList();

                string[][] matriz = GetMatrizExcel(celdas, 0, nroColumnas, 0, celdas.Count / nroColumnas);


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
                        PrCongestionDTO entity = new PrCongestionDTO();

                        string id = matriz[i][0];
                        if (id != "")
                        {
                            entity.Congescodi = Convert.ToInt32(id);
                        }


                        string equipo = matriz[i][1];
                        if (equipo != "")
                        {
                            // es número
                            try
                            {
                                //si es menor al 1MM es configcodi sino es grulincodi
                                int valor = Convert.ToInt32(equipo);

                                if (valor > ConstantesCortoPlazo.DesfaceGrupoLineaFlujoMuinimo)
                                {
                                    entity.Grulincodi = valor - ConstantesCortoPlazo.DesfaceGrupoLineaFlujoMuinimo;
                                    entity.Indtipo = ConstantesCortoPlazo.CongestionCompuesta;
                                }
                                else if (valor > ConstantesCortoPlazo.ConstanteDesfaseRegionSeguridad)
                                {
                                    entity.Regsegcodi = valor - ConstantesCortoPlazo.ConstanteDesfaseRegionSeguridad;
                                    entity.Indtipo = ConstantesCortoPlazo.CongestionRegionSeguridad;
                                }
                                else if (valor > ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea)
                                {
                                    entity.Grulincodi = valor - ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea;
                                    entity.Indtipo = ConstantesCortoPlazo.CongestionCompuesta;
                                }                                
                                else
                                {
                                    entity.Configcodi = valor;
                                    entity.Indtipo = ConstantesCortoPlazo.CongestionSimple;
                                }
                            }
                            catch
                            {
                                continue;
                            }

                        }
                        else
                        {
                            continue;
                        }


                        try
                        {
                            string horaini = matriz[i][2];
                            entity.Congesfecinicio = DateTime.ParseExact(fecha + " " + horaini, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            string horafin = matriz[i][3];

                            if (horafin != "00:00")
                            {
                                //entity.Congesfecfin = DateTime.ParseExact(horafin, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                                entity.Congesfecfin = DateTime.ParseExact(fecha + " " + horafin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime fechaVigente = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                fechaVigente = fechaVigente.AddDays(1);
                                entity.Congesfecfin = fechaVigente;
                            }
                        }
                        catch
                        {
                            continue;
                        }

                        entity.Congesmotivo = matriz[i][4];
                        entity.Iccodi = null;
                        if (matriz[i][5] != null)
                        {
                            int valorIccodi = 0;
                            if (int.TryParse(matriz[i][5], out valorIccodi))
                            {
                                entity.Iccodi = valorIccodi;
                            }
                        }

                        //Vemos los grupos de despacho asociados

                        if (matriz[i][6] != null)
                        {
                            string[] gruposDespacho = matriz[i][6].Split('#');
                            entity.ListaGrupoDespacho = gruposDespacho.ToList();
                        }

                        //- Fin de contenido agregado

                        entity.Lastuser = base.UserName;
                        entity.Lastdate = DateTime.Now;

                        servicio.SavePrCongestion(entity);
                    }

                    filaFinal++;
                }

                return Json(1);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite obtener la congestión programada en el NCP
        /// </summary>
        /// <param name="fechaini"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerCongestionProgramada(string fecha, int flagHoraTR, string horaTR)
        {
            horaTR = ConstantesHorasOperacion.FlagFiltroTR == flagHoraTR ? null : horaTR;

            string horaMin = !string.IsNullOrEmpty(horaTR) ? horaTR : DateTime.Now.ToString(ConstantesAppServicio.FormatoOnlyHora);
            DateTime fechaTR = DateTime.ParseExact(fecha + " " + horaMin, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);

            //PDO y RDO vigentes
            List<PrCongestionDTO> listaCongestion = servicio.ObtenerCongestionProgramada(fechaTR);

            List<string[]> list = new List<string[]>();

            foreach (PrCongestionDTO item in listaCongestion)
            {
                string[] itemArray = new string[4];
                itemArray[0] = string.Empty;
                //itemArray[1] = item.Fuente + item.Equinomb;

                if (item.Grulincodi != null)
                {
                    itemArray[1] = (ConstantesCortoPlazo.ConstanteDesfaseGrupoLinea + (int)item.Grulincodi).ToString();
                }
                else
                {
                    itemArray[1] = ((int)item.Configcodi).ToString();
                }

                itemArray[2] = (item.Congesfecinicio != null) ? ((DateTime)item.Congesfecinicio).ToString(Constantes.FormatoHoraMinuto) : string.Empty;
                itemArray[3] = (item.Congesfecfin != null) ? ((DateTime)item.Congesfecfin).ToString(Constantes.FormatoHoraMinuto) : string.Empty;
                list.Add(itemArray);
            }

            return Json(list);
        }

        /// <summary>
        /// Convierte una lista de datos en una Matriz Excel Web
        /// </summary>
        /// <param name="data">datos en una lista</param>
        /// <param name="colHead">Cabecera</param>
        /// <param name="nCol">Número de columnas</param>
        /// <param name="filHead">Fila</param>
        /// <param name="nFil">Número de filas</param>
        /// <returns></returns>
        private static string[][] GetMatrizExcel(List<string> data, int colHead, int nCol, int filHead, int nFil)
        {
            var colTot = nCol + colHead;
            var inicio = (colTot) * filHead;
            int col = 0;
            int fila = 0;
            string[][] arreglo = new string[nFil][];
            arreglo[0] = new string[colTot];

            for (int i = inicio; i < data.Count(); i++)
            {
                string valor = data[i];
                if (col == colTot)
                {
                    fila++;
                    col = 0;
                    if (arreglo.Length == fila)
                    {
                        break;
                    }
                    arreglo[fila] = new string[colTot];
                }
                arreglo[fila][col] = valor;
                col++;
            }

            return arreglo;
        }
    }
}
