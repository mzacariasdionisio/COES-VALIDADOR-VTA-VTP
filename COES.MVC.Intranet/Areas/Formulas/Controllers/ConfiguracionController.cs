using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Formulas.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Scada;
using COES.Servicios.Aplicacion.Scada.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using System.IO;
using OfficeOpenXml;

namespace COES.MVC.Intranet.Areas.Formulas.Controllers
{
    public class ConfiguracionController : Controller
    {
        /// <summary>
        /// Instancia de objeto para acceso a datos
        /// </summary>
        PerfilScadaServicio servicio = new PerfilScadaServicio();

        public List<FormulaItem> ListaItemFormula = new List<FormulaItem>();
        PronosticoDemandaAppServicio servicioPronostico = new PronosticoDemandaAppServicio();
        DemandaPOAppServicio servicioDemandaPO = new DemandaPOAppServicio();

        /// <summary>
        /// Obtiene la lista completa de puntos
        /// </summary>
        public List<MePerfilRuleDTO> ListaPuntos
        {
            get
            {
                return (Session[DatosSesion.ListaPuntoFormula] != null) ?
                    (List<MePerfilRuleDTO>)Session[DatosSesion.ListaPuntoFormula] : new List<MePerfilRuleDTO>();
            }
            set { Session[DatosSesion.ListaPuntoFormula] = value; }
        }

        /// <summary>
        /// Action de inicio de la pagina
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConfiguracionPerfilModel model = new ConfiguracionPerfilModel();
            model.ListaAreaOperativa = this.servicio.ObtenerListaAreaOperativa();

            return View(model);
        }

        /// <summary>
        /// Generar reporte Excel 
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpGet]
        public void GenerarReporteExcel()
        {
            int row = 1;
            String archivoDescripcion = "formulas.xlsx";
            FileInfo newFile = new FileInfo("C:\\tmp\\" + archivoDescripcion);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo("C:\\tmp\\" + archivoDescripcion);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                if (ws != null)
                {
                    int pos = 0;
                    ws.Cells[row, 1].Value = "SUBESTACIÓN";
                    ws.Cells[row, 2].Value = "ÁREA OPERATIVA";
                    ws.Cells[row, 3].Value = "CONSTANTE";
                    ws.Cells[row, 4].Value = "FUENTE";
                    ws.Cells[row, 5].Value = "PUNTO";

                    List<MePerfilRuleDTO> ListaFormulas = this.servicio.GetByCriteriaMePerfilRules(-2, "-1");
                    foreach (var formula in ListaFormulas)
                    {
                        this.ListaItemFormula.Clear();
                        
                        try
                        {
                            List<FormulaItem> data = getListadoDeFormulas(formula.Prrucodi);
                            foreach (FormulaItem item in data)
                            {
                                row++;
                                ws.Cells[row, 1].Value = (formula.Prruabrev != null) ? formula.Prruabrev.ToString() : string.Empty;
                                ws.Cells[row, 2].Value = (formula.Prrudetalle != null) ? formula.Prrudetalle : string.Empty;
                                ws.Cells[row, 3].Value = item.Constante;
                                ws.Cells[row, 4].Value = (item.Descripcion != null) ? item.Descripcion : string.Empty;
                                ws.Cells[row, 5].Value = (item.PuntoNombre != null) ? item.PuntoNombre : string.Empty;

                            }

                        }catch (Exception ex) {
                            throw new Exception(ex.Message);
                        }

                        pos++;
                    }
                }
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite buscar las fórmulas que puede visualizar el usuario
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string areaOperativa)
        {
            ConfiguracionPerfilModel model = new ConfiguracionPerfilModel();
            int areaCode = -2;
            if (Session[DatosSesion.SesionUsuario] != null)
                areaCode = (int)((COES.MVC.Intranet.SeguridadServicio.UserDTO)Session[DatosSesion.SesionUsuario]).AreaCode;
            if (string.IsNullOrEmpty(areaOperativa)) areaOperativa = (-1).ToString();
            model.ListaFormulas = this.servicio.GetByCriteriaMePerfilRules(areaCode, areaOperativa);

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar una formula
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servicio.EliminarFormula(id, User.Identity.Name);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Habilita el formulario para crear una nueva fórmula
        /// </summary>
        /// <returns></returns>
        public ActionResult Nuevo(int? id)
        {
            ConfiguracionPerfilModel model = new ConfiguracionPerfilModel();
            model.ListaAreaOperativa = this.servicio.ObtenerListaAreaOperativa();

            //Assetec.PRODEM.E3 - 20211125# Inicio
            model.ListaPuntosTnaIEOD = this.servicioPronostico
                .ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaIeod);
            model.ListaPuntosTnaSCO = this.servicioPronostico
                .ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaSco);
            //Assetec.PRODEM.E3 - 20211125# Fin
            //Assetec.PRODEM.E3 - 20230419# Inicio
            model.ListaPuntosTnaDPO = this.servicioPronostico
                .ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaSco).OrderBy(x => x.Ptomedidesc).ToList();
            //Assetec.PRODEM.E3 - 20230419# Fin
            int[] ids = { 1, 3, 4, 5, 7, 8, 12, 17 };
            model.ListaAreas = (new SeguridadServicio.SeguridadServicioClient()).ObtenerAreasCOES().Where(x => ids.Contains(x.AreaCode)).ToList();
            if (id == null) id = 0;

            if (id == 0)
            {
                model.IdsAreas = new List<int>();
                model.ListaItems = new List<FormulaItem>();
                model.IdFormula = 0;
            }
            else
            {
                MePerfilRuleDTO entity = this.servicio.GetByIdMePerfilRule((int)id);
                model.SubEstacion = entity.Prruabrev;
                model.AreaOperativa = entity.Prrudetalle;
                model.IdsAreas = entity.IdRoles;
                model.IdFormula = (int)id;
                model.UsuarioCreacion = entity.Prrufirstuser;
                model.UsuarioModificacion = entity.Prrulastuser;
                model.FechaCreacion = (entity.Prrufirstdate != null) ? ((DateTime)entity.Prrufirstdate).ToString(Constantes.FormatoFechaFull)
                    : string.Empty;
                model.FechaModificacion = (entity.Prrulastdate != null) ? ((DateTime)entity.Prrulastdate).ToString(Constantes.FormatoFechaFull)
                    : string.Empty;

                this.DescomponerFormula(entity.Prruformula);
                this.ListaItemFormula.Reverse();
                List<FormulaItem> formulas = this.ListaItemFormula;

                foreach (FormulaItem item in formulas)
                {
                    if (item.Tipo == ConstanteFormulaScada.OrigenScada.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenScada;
                        item.Descripcion = ConstanteFormulaScada.TextoSCADA;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenScada,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenPR5.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenPR5;
                        item.Descripcion = ConstanteFormulaScada.TextoPR5;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenPR5,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenEjecutado.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenEjecutado;
                        item.Descripcion = ConstanteFormulaScada.TextoEjecutado;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenEjecutado,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenDemandaDiaria.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenDemandaDiaria;
                        item.Descripcion = ConstanteFormulaScada.TextoDemandaDiaria;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenDemandaDiaria,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenDemandaMensual.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenDemandaMensual;
                        item.Descripcion = ConstanteFormulaScada.TextoDemandaMensual;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenDemandaMensual,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenMedidoresGeneracion.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenMedidoresGeneracion;
                        item.Descripcion = ConstanteFormulaScada.TextoMedidoresGeneracion;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenMedidoresGeneracion,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenScadaSP7.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenScadaSP7;
                        item.Descripcion = ConstanteFormulaScada.TextoScadaSP7;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenScadaSP7,
                            item.Codigo);
                    }
                    else if(item.Tipo == ConstanteFormulaScada.OrigenPR16.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenPR16;
                        item.Descripcion = ConstanteFormulaScada.TextoPR16;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenPR16,
                            item.Codigo);
                    }
                    //Assetec.PRODEM.E3 - 20211125# Inicio
                    else if (item.Tipo == ConstantesProdem.OrigenTnaSco)
                    {
                        item.Tipo = ConstantesProdem.OrigenTnaSco;
                        item.Descripcion = ConstantesProdem.TextoTnaSco;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    else if (item.Tipo == ConstantesProdem.OrigenTnaIeod)
                    {
                        item.Tipo = ConstantesProdem.OrigenTnaIeod;
                        item.Descripcion = ConstantesProdem.TextoTnaIeod;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    //Assetec.PRODEM.E3 - 20211125# Fin
                    //Assetec.PRODEM.E3 - 20220308# Inicio
                    else if (item.Tipo == ConstantesProdem.OrigenServiciosAuxiliares)
                    {
                        item.Tipo = ConstantesProdem.OrigenServiciosAuxiliares;
                        item.Descripcion = ConstantesProdem.TextoServiciosAuxiliares;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstantesProdem.OrigenServiciosAuxiliares,
                            item.Codigo);
                    }
                    //Assetec.PRODEM.E3 - 20220308# Fin
                    //Assetec.DemandaDPO.I2 - 20230502# Inicio
                    else if (item.Tipo == ConstantesProdem.OrigenTnaDpo)
                    {
                        item.Tipo = ConstantesProdem.OrigenTnaDpo;
                        item.Descripcion = ConstantesProdem.TextoTnaDpo;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    //Assetec.DemandaDPO.I2 - 20230502# Fin
                    //Assetec.DemandaDPO.I2 - 20230502# Inicio
                    else if (item.Tipo == ConstantesDpo.OrigenSirpit)
                    {
                        item.Tipo = ConstantesDpo.OrigenSirpit;
                        item.Descripcion = ConstantesDpo.TextoSirpit;
                        DpoTrafoBarraDTO trafoBarra = this.servicioDemandaPO.ListaTrafoBarra()
                                                                            .Where(x => x.Tnfbarcodi == item.Codigo)
                                                                            .FirstOrDefault();
                        item.PuntoNombre = trafoBarra.Tnfbartnfcodi + " - " + trafoBarra.Tnfbarbarnombre + " - " + trafoBarra.Tnfbarbartension;
                    }
                    //Assetec.DemandaDPO.I2 - 20230502# Fin
                    //Assetec.DemandaDPO.I2 - 20230515# Inicio
                    else if (item.Tipo == ConstantesDpo.OrigenSicli)
                    {
                        item.Tipo = ConstantesDpo.OrigenSicli;
                        item.Descripcion = ConstantesDpo.TextoSicli;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    //Assetec.DemandaDPO.I2 - 20230515# Fin
                }

                model.ListaItems = formulas;
            }

            return View(model);
        }

        /// <summary>
        /// Permite descomponer las formulas
        /// </summary>
        /// <param name="formula"></param>
        private void DescomponerFormula(string formula)
        {
            if (formula.Length > 0)
            {
                List<TipoItemFormula> items = new List<TipoItemFormula>();
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenScada), Tipo = ConstanteFormulaScada.OrigenScada });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenEjecutado), Tipo = ConstanteFormulaScada.OrigenEjecutado });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenDemandaDiaria), Tipo = ConstanteFormulaScada.OrigenDemandaDiaria });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenDemandaMensual), Tipo = ConstanteFormulaScada.OrigenDemandaMensual });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenMedidoresGeneracion), Tipo = ConstanteFormulaScada.OrigenMedidoresGeneracion });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenPR5), Tipo = ConstanteFormulaScada.OrigenPR5 });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenScadaSP7), Tipo = ConstanteFormulaScada.OrigenScadaSP7 });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstanteFormulaScada.OrigenPR16), Tipo = ConstanteFormulaScada.OrigenPR16 });
                //Assetec.PRODEM.E3 - 20211125# Inicio
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstantesProdem.OrigenTnaSco), Tipo = ConstantesProdem.OrigenTnaSco });
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstantesProdem.OrigenTnaIeod), Tipo = ConstantesProdem.OrigenTnaIeod });
                //Assetec.PRODEM.E3 - 20211125# Fin
                //Assetec.PRODEM.E3 - 20220308# Inicio
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstantesProdem.OrigenServiciosAuxiliares), Tipo = ConstantesProdem.OrigenServiciosAuxiliares });
                //Assetec.PRODEM.E3 - 20220308# Fin
                //Assetec.DEMANDADPO - 20230419# Inicio
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstantesProdem.OrigenTnaDpo), Tipo = ConstantesProdem.OrigenTnaDpo });
                //Assetec.DEMANDADPO - 20230419# Fin
                //Assetec.DEMANDADPO - 20230502# Inicio
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstantesDpo.OrigenSirpit), Tipo = ConstantesDpo.OrigenSirpit });
                //Assetec.DEMANDADPO - 20230502# Fin
                //Assetec.DEMANDADPO - 20231505# Inicio
                items.Add(new TipoItemFormula { Orden = formula.LastIndexOf(ConstantesDpo.OrigenSicli), Tipo = ConstantesDpo.OrigenSicli });
                //Assetec.DEMANDADPO - 20231505# Fin

                List<TipoItemFormula> itemsOrder = items.OrderByDescending(x => x.Orden).ToList();
                int pos = itemsOrder[0].Orden;
                char tipo = Convert.ToChar(itemsOrder[0].Tipo);

                string punto = formula.Substring(pos + 1, formula.Length - pos - 1);
                int posIni = formula.LastIndexOf(ConstanteFormulaScada.SeparadorFormula);
                string operador = formula.Substring(posIni + 1, pos - posIni - 1);

                this.ListaItemFormula.Add(new FormulaItem
                {
                    Codigo = int.Parse(punto),
                    Tipo = tipo.ToString(),
                    Constante = decimal.Parse(operador)
                });

                if (posIni >= 0)
                {
                    string newFormula = formula.Substring(0, posIni);
                    this.DescomponerFormula(newFormula);
                }
            }
        }

        /// <summary>
        /// Permite mostrar las fuentes para obtener las fórmulas
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(string indicador)
        {
            List<MePerfilRuleDTO> listPuntos = this.servicio.ObtenerPuntosPorFuente(indicador);
            this.ListaPuntos = listPuntos;
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();

            var items = listPuntos.Select(x => new { x.EmprCodi, x.EmprNomb }).Distinct().ToList();
            foreach (var item in items)
                entitys.Add(new MePerfilRuleDTO { EmprCodi = item.EmprCodi, EmprNomb = item.EmprNomb });
            entitys.OrderBy(x => x.EmprNomb).ToList();
            SelectList list = new SelectList(entitys.OrderBy(x => x.EmprNomb), EntidadPropiedad.EmprCodi, EntidadPropiedad.EmprNomb);

            return Json(list);
        }

        /// <summary>
        /// Permite cargar los puntos de la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarPuntos(int idEmpresa, int idSubEstacion)
        {
            IList<MePerfilRuleDTO> entitys = this.ListaPuntos.Where(x => x.EmprCodi == idEmpresa && x.Areacodi == idSubEstacion).ToList();
            SelectList list = new SelectList(entitys, EntidadPropiedad.PtoMediCodi, EntidadPropiedad.PtoNomb);
            return Json(list);
        }

        /// <summary>
        /// Carga las subestaciones de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarSubEstacion(int idEmpresa)
        {
            IList<MePerfilRuleDTO> entitys = this.ListaPuntos.Where(x => x.EmprCodi == idEmpresa).ToList();
            var items = entitys.Select(x => new { x.Areacodi, x.AreaNomb }).Distinct().ToList();
            List<MePerfilRuleDTO> listAreas = new List<MePerfilRuleDTO>();

            foreach (var item in items)
            {
                listAreas.Add(new MePerfilRuleDTO { Areacodi = item.Areacodi, AreaNomb = item.AreaNomb });
            }

            SelectList list = new SelectList(listAreas, EntidadPropiedad.Areacodi, EntidadPropiedad.AreaNomb);

            return Json(list);
        }

        /// <summary>
        /// Permite grabar la fórmula
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="subEstacion"></param>
        /// <param name="area"></param>
        /// <param name="roles"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(int idFormula, string subEstacion, string areaOperativa, string roles, string items)
        {
            try
            {
                List<string> idRoles = roles.Split(Constantes.CaracterComa).ToList();
                string formula = items.Remove(items.Length - 1, 1);

                MePerfilRuleDTO entity = new MePerfilRuleDTO();
                entity.Prruabrev = subEstacion;
                entity.Prrudetalle = areaOperativa;
                entity.Prruformula = items;
                entity.Prrulastuser = User.Identity.Name;
                entity.Prrufirstuser = User.Identity.Name;
                entity.Prruactiva = Constantes.SI;
                entity.Prrucodi = idFormula;
                entity.Prrupref = string.Empty;
                int id = this.servicio.GrabarMePerfilRule(entity, idRoles);

                return Json(id);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <returns></returns>
        public List<FormulaItem> getListadoDeFormulas(int id)
        {
            ConfiguracionPerfilModel model = new ConfiguracionPerfilModel();
            model.ListaAreaOperativa = this.servicio.ObtenerListaAreaOperativa();

            //Assetec.PRODEM.E3 - 20211125# Inicio
            model.ListaPuntosTnaIEOD = this.servicioPronostico
                .ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaIeod);
            model.ListaPuntosTnaSCO = this.servicioPronostico
                .ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaSco);
            //Assetec.PRODEM.E3 - 20211125# Fin
            //Assetec.PRODEM.E3 - 20230419# Inicio
            model.ListaPuntosTnaDPO = this.servicioPronostico
                .ListPtomedicionByOriglectcodi(ConstantesProdem.OriglectcodiTnaSco).OrderBy(x => x.Ptomedidesc).ToList();
            //Assetec.PRODEM.E3 - 20230419# Fin
            int[] ids = { 1, 3, 4, 5, 7, 8, 12, 17 };
            model.ListaAreas = (new SeguridadServicio.SeguridadServicioClient()).ObtenerAreasCOES().Where(x => ids.Contains(x.AreaCode)).ToList();
            if (id == null) id = 0;

            if (id == 0)
            {
                model.IdsAreas = new List<int>();
                model.ListaItems = new List<FormulaItem>();
                model.IdFormula = 0;
            }
            else
            {
                MePerfilRuleDTO entity = this.servicio.GetByIdMePerfilRule((int)id);
                model.SubEstacion = entity.Prruabrev;
                model.AreaOperativa = entity.Prrudetalle;
                model.IdsAreas = entity.IdRoles;
                model.IdFormula = (int)id;
                model.UsuarioCreacion = entity.Prrufirstuser;
                model.UsuarioModificacion = entity.Prrulastuser;
                model.FechaCreacion = (entity.Prrufirstdate != null) ? ((DateTime)entity.Prrufirstdate).ToString(Constantes.FormatoFechaFull)
                    : string.Empty;
                model.FechaModificacion = (entity.Prrulastdate != null) ? ((DateTime)entity.Prrulastdate).ToString(Constantes.FormatoFechaFull)
                    : string.Empty;

                this.DescomponerFormula(entity.Prruformula);
                this.ListaItemFormula.Reverse();
                List<FormulaItem> formulas = this.ListaItemFormula;

                foreach (FormulaItem item in formulas)
                {
                    if (item.Tipo == ConstanteFormulaScada.OrigenScada.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenScada;
                        item.Descripcion = ConstanteFormulaScada.TextoSCADA;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenScada,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenPR5.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenPR5;
                        item.Descripcion = ConstanteFormulaScada.TextoPR5;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenPR5,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenEjecutado.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenEjecutado;
                        item.Descripcion = ConstanteFormulaScada.TextoEjecutado;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenEjecutado,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenDemandaDiaria.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenDemandaDiaria;
                        item.Descripcion = ConstanteFormulaScada.TextoDemandaDiaria;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenDemandaDiaria,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenDemandaMensual.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenDemandaMensual;
                        item.Descripcion = ConstanteFormulaScada.TextoDemandaMensual;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenDemandaMensual,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenMedidoresGeneracion.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenMedidoresGeneracion;
                        item.Descripcion = ConstanteFormulaScada.TextoMedidoresGeneracion;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenMedidoresGeneracion,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenScadaSP7.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenScadaSP7;
                        item.Descripcion = ConstanteFormulaScada.TextoScadaSP7;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenScadaSP7,
                            item.Codigo);
                    }
                    else if (item.Tipo == ConstanteFormulaScada.OrigenPR16.ToString())
                    {
                        item.Tipo = ConstanteFormulaScada.OrigenPR16;
                        item.Descripcion = ConstanteFormulaScada.TextoPR16;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstanteFormulaScada.OrigenPR16,
                            item.Codigo);
                    }
                    //Assetec.PRODEM.E3 - 20211125# Inicio
                    else if (item.Tipo == ConstantesProdem.OrigenTnaSco)
                    {
                        item.Tipo = ConstantesProdem.OrigenTnaSco;
                        item.Descripcion = ConstantesProdem.TextoTnaSco;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    else if (item.Tipo == ConstantesProdem.OrigenTnaIeod)
                    {
                        item.Tipo = ConstantesProdem.OrigenTnaIeod;
                        item.Descripcion = ConstantesProdem.TextoTnaIeod;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    //Assetec.PRODEM.E3 - 20211125# Fin
                    //Assetec.PRODEM.E3 - 20220308# Inicio
                    else if (item.Tipo == ConstantesProdem.OrigenServiciosAuxiliares)
                    {
                        item.Tipo = ConstantesProdem.OrigenServiciosAuxiliares;
                        item.Descripcion = ConstantesProdem.TextoServiciosAuxiliares;
                        item.PuntoNombre = this.servicio.ObtenerNombrePunto(ConstantesProdem.OrigenServiciosAuxiliares,
                            item.Codigo);
                    }
                    //Assetec.PRODEM.E3 - 20220308# Fin
                    //Assetec.DemandaDPO.I2 - 20230502# Inicio
                    else if (item.Tipo == ConstantesProdem.OrigenTnaDpo)
                    {
                        item.Tipo = ConstantesProdem.OrigenTnaDpo;
                        item.Descripcion = ConstantesProdem.TextoTnaDpo;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    //Assetec.DemandaDPO.I2 - 20230502# Fin
                    //Assetec.DemandaDPO.I2 - 20230502# Inicio
                    else if (item.Tipo == ConstantesDpo.OrigenSirpit)
                    {
                        item.Tipo = ConstantesDpo.OrigenSirpit;
                        item.Descripcion = ConstantesDpo.TextoSirpit;
                        DpoTrafoBarraDTO trafoBarra = this.servicioDemandaPO.ListaTrafoBarra()
                                                                            .Where(x => x.Tnfbarcodi == item.Codigo)
                                                                            .FirstOrDefault();
                        item.PuntoNombre = trafoBarra.Tnfbartnfcodi + " - " + trafoBarra.Tnfbarbarnombre + " - " + trafoBarra.Tnfbarbartension;
                    }
                    //Assetec.DemandaDPO.I2 - 20230502# Fin
                    //Assetec.DemandaDPO.I2 - 20230515# Inicio
                    else if (item.Tipo == ConstantesDpo.OrigenSicli)
                    {
                        item.Tipo = ConstantesDpo.OrigenSicli;
                        item.Descripcion = ConstantesDpo.TextoSicli;
                        item.PuntoNombre = this.servicioPronostico.GetByIdMePtomedicion(item.Codigo).Ptomedidesc;
                    }
                    //Assetec.DemandaDPO.I2 - 20230515# Fin
                }

                model.ListaItems = formulas;
            }

            return model.ListaItems;
        }

        #region DemandaDPO - Iteracion 2
        /// <summary>
        /// Permite cargar las subestaciones para el origen sirpit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarSubestacionSirpit()
        {
            List<DpoSubestacionDTO> entitys = this.servicioDemandaPO.ListaSubestacionesDPO();

            return Json(entitys);
        }

        /// <summary>
        /// Permite cargar las empresas para el filtro sicli
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresaSicli()
        {
            List<SiEmpresaDTO> entitys = this.servicioDemandaPO.ListaEmpresasByTipo(ConstantesDpo.EmpresaTipoUL + "," + ConstantesDpo.EmpresaTipoGen);
            var JsonResult = Json(entitys);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Permite cargar los equipos para el filtro sicli
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEquipoSicli(int idEmpresa)
        {
            List<EqEquipoDTO> entitys = this.servicioDemandaPO.ListaEquipoByEmpresa(idEmpresa);
            var JsonResult = Json(entitys);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Permite cargar los puntos para el filtro sicli
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarPuntoSicli(int idEmpresa)
        {
            List<MePtomedicionDTO> entitys = this.servicioDemandaPO.ListaPuntoSicliByEmpresa(ConstantesDpo.OrigenLecturaDemanda, idEmpresa);
            var JsonResult = Json(entitys);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            return JsonResult;
        }

        /// <summary>
        /// Permite cargar las subestaciones para el origen sirpit
        /// </summary>
        /// <param name="idSubestacion">Identificador de la subetacion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarTransformadorSirpit(int idSubestacion)
        {
            List<DpoTransformadorDTO> entitys = this.servicioDemandaPO.ListaTransformadorBySubestacion(idSubestacion);

            return Json(entitys);
        }

        /// <summary>
        /// Permite cargar las barras para el origen sirpit segun el trafo
        /// </summary>
        /// <param name="idTransformador">Identificador del transformador</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarBarraSirpit(string idTransformador)
        {
            List<DpoTrafoBarraDTO> entitys = this.servicioDemandaPO.ListaTrafoBarra()
                                                                   .Where(x => x.Tnfbartnfcodi == idTransformador)
                                                                   .ToList();

            return Json(entitys);
        }
        #endregion
    }
}
