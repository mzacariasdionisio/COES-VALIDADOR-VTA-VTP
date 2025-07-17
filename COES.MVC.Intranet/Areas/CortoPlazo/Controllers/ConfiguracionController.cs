using COES.Dominio.DTO.Sic;
//using COES.MVC.Intranet.Areas.Coordinacion.Models;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.OperacionesVarias;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{

    public class ConfiguracionController : BaseController
    {
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        EquipamientoAppServicio servEquipo = new EquipamientoAppServicio();

        #region Relacion de Generadores

        public ActionResult Index()
        {
            return View();
        }

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
                model.ListaAdicionalTNA = this.servicio.ObtenerEquiposTNAAdicionales(idRelacion);

                //- Linea agregada movisoft 25.02.2021
                if (string.IsNullOrEmpty(model.Entidad.Indgeneracionrer))
                {
                    model.Entidad.Indgeneracionrer = Constantes.NO;
                }
            }
            else
            {
                model.Entidad = new EqRelacionDTO();
                model.ListaEquipo = new List<EqEquipoDTO>();
                model.Entidad.Emprcodi = -1;
                //- Linea agregada movisoft 25.02.2021
                model.Entidad.Indgeneracionrer = Constantes.NO;
                model.ListaAdicionalTNA = new List<EqRelacionTnaDTO>();
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
                    //- Modificación movisoft 10.03.2021
                    //Codincp = model.Codncp,
                    //Nombrencp = model.Nombrencp,
                    //- Fin Modificación movisoft 10.03.2021
                    Descripcion = model.Descripcion,
                    Estado = model.Estado,
                    Nombretna = model.NombreTna,
                    ///- Linea agregada movisoft 25.02.2021
                    Indgeneracionrer = model.RER,

                    #region Ticket 2022-004245
                    Indnomodeladatna = model.IndNoModedada,
                    #endregion
                    Indtnaadicional = model.MasEquiposTNA,
                    EquipoAdicionalTNA = model.EquipoAdicional,
                    Usucreacion = base.UserName
                };               

                int result = this.servicio.SaveEqRelacion(entity);

               

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult RelacionListGlobal()
        {
            RelacionModel model = new RelacionModel();
            model.ListaEmpresa = this.servicio.ListarEmpresasRelacion();
            return View(model);
        }

        /// <summary>
        /// Obtener los datos de generadores en vista Excel
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataGenerador(int empresa, string estado)
        {
            RelacionModel model = new RelacionModel();
            model.ListaRelacion = this.servicio.GetByCriteriaEqRelacions(empresa, estado);
            int registrosTotal = 7 + (model.ListaRelacion.Count == 0 ? 1 : model.ListaRelacion.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 7);
            int indice = 0;

            foreach (var item in model.ListaRelacion)
            {
                data[indice][0] = item.Relacioncodi.ToString();

                if (item.Equicodi == null)
                    item.Equicodi = -1;

                data[indice][1] = (item.Equicodi != null) ? item.Equicodi.ToString() : "-1";
                data[indice][2] = (item.Nombarra != null) ? item.Nombarra.ToString() : "";
                data[indice][3] = (item.Idgener != null) ? item.Idgener.ToString() : "";
                data[indice][4] = (item.Codincp != null) ? item.Codincp.ToString() : "";
                data[indice][5] = (item.Nombrencp != null) ? item.Nombrencp.ToString() : "";
                data[indice][6] = (item.Estado != null) ? item.Estado.ToString() : "";

                indice++;
            }

            model.Datos = data;
            model.Registro = indice;

            return Json(model);
        }

        /// <summary>
        /// Permite obtener los generadores
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListaGenerador()
        {
            RelacionModel model = new RelacionModel();
            model.ListaRelacion = servicio.ListEqRelacions().OrderBy(x => x.Relacioncodi).ToList();
            model.ListaEquipo = this.servicio.ListarEquiposPorEmpresa(-1);
            return Json(model);
        }

        /// <summary>
        /// Permite grabar las relaciones
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarListGlobal(string dataExcel)
        {
            try
            {
                List<EqRelacionDTO> ListaGrupoLinea = servicio.ListEqRelacions();
                List<EqEquipoDTO> listaEquipoGen = this.servicio.ListarEquiposPorEmpresa(-1);
                int nroColumnas = 7;
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
                        EqRelacionDTO entity = new EqRelacionDTO();
                        entity.Equicodi = -1;
                        entity.Relacioncodi = (!string.IsNullOrEmpty(matriz[i][0])) ? int.Parse(matriz[i][0]) : 0;
                        string equipo = matriz[i][1];
                        int idEquipo = 0;
                        int idNcp = 0;

                        if (int.TryParse(equipo, out idEquipo))
                            entity.Equicodi = idEquipo;
                        else
                        {
                            int equicodi = (listaEquipoGen.Where(x => x.Equinomb == equipo).ToList())[0].Equicodi;
                            entity.Equicodi = equicodi;
                        }

                        entity.Nombarra = matriz[i][2];
                        entity.Idgener = matriz[i][3];
                        if (int.TryParse(matriz[i][4], out idNcp))
                            entity.Codincp = idNcp;
                        entity.Nombrencp = matriz[i][5];
                        string estado = matriz[i][6];

                        if (estado == "ACTIVO" || estado == "NO ACTIVO")
                            entity.Estado = estado;
                        else
                            entity.Estado = "ACTIVO";

                        entity.Indfuente = "G";
                        entity.Lastuser = base.UserName;
                        entity.Lastdate = DateTime.Now;

                        servicio.SaveEqRelacion(entity);
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

        #region Ticket 2022-004245

        [HttpPost]
        public PartialViewResult RelacionConfiguracion(int relacionCodi)
        {
            RelacionModel model = new RelacionModel();
            model.Relacioncodi = relacionCodi;

            List<CmGeneradorPotenciagenDTO> listPotencia = new List<CmGeneradorPotenciagenDTO>();
            List<CmGeneradorBarraemsDTO> listBarra = new List<CmGeneradorBarraemsDTO>();
            this.servicio.ObtenerDatosEquipoNoModelado(relacionCodi, out listPotencia, out listBarra);
            model.ListaPotencia = listPotencia;
            model.ListaBarra = listBarra;
            model.ListaBarraEMS = this.servicio.ObtenerBarrasEMS();

            return PartialView(model);
        }

        /// <summary>
        /// Permite almacenar los datos de la configuracion de la unidad
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarConfiguracionUnidad(RelacionModel model)
        {
            return Json(this.servicio.GrabarDatosEquipoNoModelado(model.Relacioncodi, model.CodigosBarra, model.CodigosPotencia, base.UserName));
        }

        #endregion

        #endregion

        #region Lineas de Transmision

        /// <summary>
        /// Muestra la pantalla de lineas
        /// </summary>
        /// <returns></returns>
        public ActionResult Linea()
        {
            LineaModel model = new LineaModel();
            model.ListEmpresa = this.servicio.ObtenerEmpresaFiltroLinea();           
            return View(model);
        }

        /// <summary>
        /// Lista los equipos por empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLineas(int emprcodi, int famcodi)
        {
            RelacionModel model = new RelacionModel();
            List<EqEquipoDTO> entitys = this.servicio.ListarEquipoLineaPorEmpresa(emprcodi, famcodi);
            SelectList list = new SelectList(entitys, EntidadPropiedad.Equicodi, EntidadPropiedad.Equinomb);

            return Json(list);
        }

        /// <summary>
        /// Permite listar las lineas configuradas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult LineaList(int idEmpresa, string estado, int idGrupo, int idFamilia)
        {
            LineaModel model = new LineaModel();
            model.ListLinea = this.servicio.GetByCriteriaEqCongestionConfigs(idEmpresa, estado, idGrupo, idFamilia);
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar una equivalencia de lineas
        /// </summary>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LineaDelete(int idLinea)
        {
            try
            {
                this.servicio.DeleteEqCongestionConfig(idLinea);
                return Json(1);
            }
            catch
            {
                return Json(1);
            }
        }

        /// <summary>
        /// Permite listar la edición
        /// </summary>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult LineaEdit(int idLinea)
        {
            LineaModel model = new LineaModel();
            model.ListEmpresa = this.servicio.ListEmpresasLineas();
            model.ListGrupoLinea = new List<EqGrupoLineaDTO>();


            if (idLinea == 0)
            {
                model.Entidad = new EqCongestionConfigDTO();
                model.ListEquipo = new List<EqEquipoDTO>();
                model.Entidad.Emprcodi = -1;
                model.Entidad.Grulincodi = -1;
                model.Entidad.Famcodi = ConstantesCortoPlazo.IdLineaTransmision;
                model.Entidad.Estado = ConstantesCortoPlazo.EstadoActivo;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdEqCongestionConfig(idLinea);
                model.ListEquipo = this.servicio.ListarEquipoLineaPorEmpresa(model.Entidad.Emprcodi, (int)model.Entidad.Famcodi);

                if (model.Entidad.Grulincodi == null)
                {
                    model.Entidad.Grulincodi = -1;
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos de la configuracion de la linea
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LineaSave(LineaModel model)
        {
            LineaModel mod = new LineaModel();
            try
            {
                base.ValidarSesionJsonResult();

                EqCongestionConfigDTO entity = new EqCongestionConfigDTO
                {
                    Configcodi = model.Lineacodi,
                    Grulincodi = (model.Grupolineacodi != -1) ? model.Grupolineacodi : null,

                    Estado = model.Estado,
                    Equicodi = model.Equicodi,
                    Lastuser = base.UserName,
                    Lastdate = DateTime.Now,
                    Nombrencp = model.Nombrencp,
                    Codincp = model.Codncp,
                    Nodobarra1 = model.Nodobarra1,
                    Nodobarra2 = model.Nodobarra2,
                    Nodobarra3 = model.Nodobarra3,
                    Idems = model.Idems,
                    Nombretna1 = model.Nombretna1,
                    Nombretna2 = model.Nombretna2,
                    Nombretna3 = model.Nombretna3,
                    Canalcodi = model.Canalcodi
                };

                int resultado = this.servicio.SaveEqCongestionConfig(entity);
                mod.Resultado = resultado.ToString();
                
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                mod.Resultado = "-1";
                mod.Mensaje = ex.Message;
                mod.Detalle = ex.StackTrace;
            }

            return Json(mod);
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>        
        public ActionResult LineaListGlobal()
        {
            LineaModel model = new LineaModel();
            model.ListEmpresa = this.servicio.ObtenerEmpresaFiltroLinea();
            model.ListGrupoLinea = new List<EqGrupoLineaDTO>();
            return View(model);
        }

        /// <summary>
        /// Permite obtener datos de linea-trafo 2D y 3D. También considera listaod de Lineas y grupos de líneas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDataLineaTrafoGrupo(int empresa, string estado, int grupo, int familia)
        {
            LineaModel model = new LineaModel();

            List<EqGrupoLineaDTO> listGrupos = new List<EqGrupoLineaDTO>();
            model.ListLinea = this.servicio.GetByCriteriaEqCongestionConfigs(empresa, estado, grupo, familia).ToList().OrderBy(x => x.Configcodi).ToList();
            model.ListLineaEquipo = this.servicio.ListarEquipoLineaPorEmpresa(empresa, ConstantesCortoPlazo.IdLineaTransmision);
            model.ListTrafo2dEquipo = this.servicio.ListarEquipoLineaPorEmpresa(empresa, ConstantesCortoPlazo.IdTrafo2D);
            model.ListTrafo3dEquipo = this.servicio.ListarEquipoLineaPorEmpresa(empresa, ConstantesCortoPlazo.IdTrafo3D);
            model.ListConjuntoLinea = listGrupos.Select(x => x.Grulinnombre).ToList();
            model.ListGrupoLinea = listGrupos;

            int registrosTotal = 7 + (model.ListLinea.Count == 0 ? 1 : model.ListLinea.Count);
            string[][] data = new string[registrosTotal + 1][];
            data = CalculoHelper.PintarCeldas(data, 10);
            int indice = 0;

            foreach (var item in model.ListLinea)
            {
                data[indice][0] = item.Configcodi.ToString();
                if (item.Equicodi == null)
                    item.Equicodi = -1;
                data[indice][1] = (item.Equicodi != null) ? item.Equicodi.ToString() : "-1";
                data[indice][2] = (item.Nodobarra1 == null) ? "" : item.Nodobarra1.ToString();
                data[indice][3] = (item.Nodobarra2 == null) ? "" : item.Nodobarra2.ToString();
                data[indice][4] = (item.Nodobarra3 == null) ? "" : item.Nodobarra3.ToString();
                data[indice][5] = (item.Idems == null) ? "" : item.Idems.ToString();
                data[indice][6] = (item.Codincp == null) ? "" : item.Codincp.ToString();
                data[indice][7] = (item.Nombrencp == null) ? "" : item.Nombrencp.ToString();
                string gruponomb = string.Empty;
                if (listGrupos.Where(x => x.Grulincodi == item.Grulincodi).Count() > 0)
                    gruponomb = (item.Grulincodi == null) ? "" : (listGrupos.Where(x => x.Grulincodi == item.Grulincodi).ToList())[0].Grulinnombre;
                data[indice][8] = gruponomb;
                data[indice][9] = (item.Estado == null) ? "" : item.Estado.ToString();
                indice++;
            }

            model.Datos = data;
            model.Registro = indice;


            return new JsonResult { Data = model, MaxJsonLength = Int32.MaxValue };
        }

        /// <summary>
        /// Permite grabar las lineas
        /// </summary>        
        /// <param name="dataExcel">Daros "excel"</param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarLineaGlobal(string dataExcel)
        {
            try
            {
                List<EqEquipoDTO> listaEquipoLin = servEquipo.ListadoEquipoNombre(ConstantesAppServicio.ParametroDefecto).Where(
                x => x.Famcodi == 8 || x.Famcodi == 9 || x.Famcodi == 10 || x.Equicodi == -1).ToList();

                List<EqGrupoLineaDTO> listaGrupoLinea = new List<EqGrupoLineaDTO>();
                int nroColumnas = 10;
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
                    {
                        return Json(1);
                    }
                    else
                    {
                        EqCongestionConfigDTO entity = new EqCongestionConfigDTO();

                        int idConfig = 0;
                        int idEquipo = -1;
                        int idNcp = 0;
                        if (int.TryParse(matriz[i][0], out idConfig))
                            entity.Configcodi = idConfig;


                        string equipo = matriz[i][1];
                        if (int.TryParse(equipo, out idEquipo))
                            entity.Equicodi = idEquipo;
                        else
                        {
                            if (listaEquipoLin.Where(x => x.Equinomb == equipo).Count() > 0)
                            {
                                int equicodi = (listaEquipoLin.Where(x => x.Equinomb == equipo).ToList())[0].Equicodi;
                                entity.Equicodi = equicodi;
                            }
                        }

                        entity.Nodobarra1 = matriz[i][2];
                        entity.Nodobarra2 = matriz[i][3];
                        entity.Nodobarra3 = matriz[i][4];
                        entity.Idems = matriz[i][5];

                        if (int.TryParse(matriz[i][6], out idNcp))
                            entity.Codincp = idNcp;

                        entity.Nombrencp = matriz[i][7];
                        string grupoLinea = matriz[i][8];

                        if (equipo != string.Empty)
                        {
                            if (listaGrupoLinea.Where(x => x.Grulinnombre == grupoLinea).Count() > 0)
                            {
                                int grulincodi = (listaGrupoLinea.Where(x => x.Grulinnombre == grupoLinea).ToList())[0].Grulincodi;
                                entity.Grulincodi = grulincodi;
                            }
                        }

                        entity.Estado = matriz[i][9];
                        entity.Lastuser = base.UserName;
                        entity.Lastdate = DateTime.Now;

                        servicio.SaveEqCongestionConfig(entity);
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

        #region Grupo de Lineas

        /// <summary>
        /// Permite eliminar un grupo de lineas
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupoLineaDelete(int idGrupo)
        {
            try
            {
                int resultado = this.servicio.DeleteEqGrupoLinea(idGrupo);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Lista los grupos de lineas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoLineaList(string tipo)
        {
            LineaModel model = new LineaModel();
            model.ListGrupoLinea = this.servicio.GetByCriteriaEqGrupoLineas(tipo);
            model.TipoGrupo = tipo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el detalle del grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult GrupoLineaEdit(int idGrupo, string tipo)
        {
            GrupoLineaModel model = new GrupoLineaModel();
            int[] familias = new int[1];
            familias[0] = ConstantesCortoPlazo.IdFamiliaGrupo;
            model.ListaEquipo = (new EquipamientoAppServicio()).ListarEquipoxFamilias(familias);

            if (idGrupo == 0)
            {
                model.Entidad = new EqGrupoLineaDTO();
                model.Entidad.Grulintipo = tipo;

            }
            else
            {
                model.Entidad = this.servicio.GetByIdEqGrupoLinea(idGrupo);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los grupos de lineas
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupoLineaSave(GrupoLineaModel model)
        {
            try
            {
                EqGrupoLineaDTO entity = new EqGrupoLineaDTO
                {
                    Grulincodi = model.Codigo,
                    Grulinnombre = model.Nombre,
                    Grulinvallintrans = model.Limite,
                    Grulinporlimtrans = model.Porcentaje,
                    Grulinestado = model.Estado,
                    Nombrencp = model.NombreNcp,
                    Codincp = model.CodigoNcp,
                    Grulintipo = model.TipoGrupo,
                    Equicodi = model.Equipo
                };

                this.servicio.SaveEqGrupoLinea(entity);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        #endregion

        #region Configuracion de Enlaces

        /// <summary>
        /// Permite mostrar la pantalla de enlaces del conjunto
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnlaceIndex(int idGrupo)
        {
            GrupoLineaModel model = new GrupoLineaModel();
            model.ListaLinea = this.servicio.GetByCriteriaEqCongestionConfigs(-1, (-1).ToString(), -1,
                ConstantesCortoPlazo.IdLineaTransmision).OrderBy(x => x.Nodobarra1).ToList();
            model.Codigo = idGrupo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite listar los lineas de un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnlaceList(int idGrupo)
        {
            GrupoLineaModel model = new GrupoLineaModel();
            model.ListaLinea = this.servicio.ObtenerLineasPorGrupo(idGrupo);
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del enlace
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnlaceSave(int idGrupo, int idLinea)
        {
            try
            {
                CmConjuntoenlaceDTO entity = new CmConjuntoenlaceDTO
                {
                    Configcodi = idLinea,
                    Grulincodi = idGrupo,
                    Lastdate = DateTime.Now,
                    Lastuser = base.UserName
                };

                int resultado = this.servicio.SaveCmConjuntoenlace(entity);

                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar un enlace de un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnlaceDelete(int idGrupo, int idLinea)
        {
            try
            {
                this.servicio.DeleteCmConjuntoenlace(idGrupo, idLinea);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        #endregion

        #region Regiones de Seguridad

        /// <summary>
        /// Permite eliminar un grupo de lineas
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegionSeguridadDelete(int idRegion)
        {
            try
            {
                this.servicio.DeleteCmRegionseguridad(idRegion);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Lista los grupos de lineas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RegionSeguridadList()
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.Listado = this.servicio.GetByCriteriaCmRegionseguridads();
            return PartialView(model);
        }

        /// <summary>
        /// Permite visualizar el detalle del grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RegionSeguridadEdit(int idRegion)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();

            if (idRegion == 0)
            {
                model.Entidad = new CmRegionseguridadDTO();
                model.Entidad.Regsegestado = Constantes.EstadoActivo;
            }
            else
            {
                model.Entidad = this.servicio.GetByIdCmRegionseguridad(idRegion);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los grupos de lineas
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegionSeguridadSave(RegionSeguridadModel model)
        {
            try
            {
                CmRegionseguridadDTO entity = new CmRegionseguridadDTO
                {
                    Regsegcodi = model.Codigo,
                    Regsegnombre = model.Nombre,
                    Regsegvalorm = model.ValorM,
                    Regsegdirec = model.Direccion,
                    Regsegestado = model.Estado,
                    Regsegusucreacion = base.UserName,
                    Regsegusumodificacion = base.UserName,
                    Regsegfeccreacion = DateTime.Now,
                    Regsegfecmodificacion = DateTime.Now
                };

                this.servicio.SaveCmRegionseguridad(entity);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite mostrar la pantalla de enlaces del conjunto
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnlaceRegionSeguridadIndex(int idRegion)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.Codigo = idRegion;
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener el listado de equipos
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEquiposRegionSeguridad(int tipo)
        {
            try
            {
                return Json(this.servicio.ObtenerEquiposConjunto(tipo));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite listar los lineas de un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EnlaceRegionSeguridadList(int idRegion)
        {
            RegionSeguridadModel model = new RegionSeguridadModel();
            model.ListaDetalle = this.servicio.GetByCriteriaCmRegionseguridadDetalles(idRegion);
            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos del enlace
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnlaceRegionSeguridadSave(int idRegion, int idEquipo)
        {
            try
            {
                CmRegionseguridadDetalleDTO entity = new CmRegionseguridadDetalleDTO
                {
                    Regsegcodi = idRegion,
                    Equicodi = idEquipo,
                    Regsegusucreacion = base.UserName,
                    Regsegfeccreacion = DateTime.Now
                };

                int resultado = this.servicio.SaveCmRegionseguridadDetalle(entity);

                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite eliminar un enlace de un grupo
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idLinea"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnlaceRegionSeguridadDelete(int idRegion, int idEquipo)
        {
            try
            {
                this.servicio.DeleteCmRegionseguridadDetalle(idRegion, idEquipo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        #endregion

        #region Umbrales para comparativo

        /// <summary>
        /// muestra vista principal de parámetros umbrales
        /// </summary>
        /// <returns></returns>
        public ActionResult Umbral() 
        {
            UmbralModel model = new UmbralModel();
            model.Entidad = this.servicio.OtenerUmbralConfiguracion(base.UserName);
            return View(model);
        }

        /// <summary>
        /// Actualizar parámetros umbrales
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarUmbral(UmbralModel modelo) 
        {
            UmbralModel model = new UmbralModel();
            try
            {
                base.ValidarSesionJsonResult();

                CmUmbralComparacionDTO entity;
                var umbraldto = servicio.GetByIdUmbralComparacion();
                entity = umbraldto != null ? umbraldto : new CmUmbralComparacionDTO();

                if (modelo.NumTab == Servicios.Aplicacion.CortoPlazo.Helper.ConstantesCortoPlazo.TapUmbralesComparativos)
                {
                    entity.Cmumcocodi = modelo.Codigo;
                    entity.Cmumcohopdesp = modelo.UmbralHOP;
                    entity.Cmumcoemsdesp = modelo.UmbralGeneracionEMS;
                    entity.Cmuncodemanda = modelo.UmbralDemandaEMS;
                    entity.Cmumcoci = modelo.UmbralCI;
                    entity.Cmuncousumodificacion = base.UserName;
                }
                else
                {
                    if (modelo.NumTab == Servicios.Aplicacion.CortoPlazo.Helper.ConstantesCortoPlazo.TapAnguloOptimo)
                    {
                        entity.Cmumcocodi = modelo.Codigo;
                        entity.Cmumconumiter = modelo.UmbralNumIter;
                        entity.Cmumcovarang = modelo.UmbralVarAng;
                        entity.Cmuncousumodificacion = base.UserName;
                    }
                }

                this.servicio.GrabarUmbralComparacion(entity);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return Json(model);
        }

        #endregion
    }
}
