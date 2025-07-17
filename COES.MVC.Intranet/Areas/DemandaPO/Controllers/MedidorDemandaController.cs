using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Scada;
using COES.Servicios.Aplicacion.Scada.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class MedidorDemandaController : Controller
    {
        public List<FormulaItem> ListaItemFormula = new List<FormulaItem>();
        DemandaPOAppServicio servicio = new DemandaPOAppServicio();
        PerfilScadaServicio servicioScada = new PerfilScadaServicio();

        public ActionResult Index()
        {
            MedidorDemandaModel model = new MedidorDemandaModel() 
            {
                FechaIni = DateTime.Now.ToString(ConstantesDpo.FormatoFecha),
                FechaFin = DateTime.Now.AddDays(1).ToString(ConstantesDpo.FormatoFecha),
                ListaBarraSPL = this.servicio.ListaBarraSpl(),
                ListaVersion = this.servicio.ListaVersiones()
            };

            return View(model);
        }

        /// <summary>
        /// Lista las barras SPL que pertenecen a una version
        /// </summary>
        /// <param name="version">identificador de la version</param>
        /// <returns></returns>
        public JsonResult ListaBarrasxVersion(int version)
        {
            RelacionBarraModel model = new RelacionBarraModel()
            {
                ListaBarras = this.servicio.ListaBarrasPorVersion(version)
            };
            //model.ListaBarras = this.servicio.ListaBarrasPorVersion(idVersion);
            return Json(model);
        }

        /// <summary>
        /// Genera las listas y objetos para el popup de relacion
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerarRelacion()
        {
            RelacionBarraModel model = new RelacionBarraModel();
            model.ListaBarrasSPL = this.servicio.ListaBarraSpl().Where(x => x.Barsplestado == "A").ToList();
            return Json(model);
        }

        /// <summary>
        /// Permite consultar la data a mostrar en las graficas
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <param name="fuente"></param>
        /// <param name="version"></param>
        /// <param name="barra"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarData(string inicio, string fin, int fuente, int version, string barra,
                                        int parametro)
        {
            List<DpoDemandaScoDTO> demandaSCO = new List<DpoDemandaScoDTO>();
            List<DpoGrillaDatos96> datos = new List<DpoGrillaDatos96>();
            List<DpoRelSplFormulaDTO> entity = this.servicio.ListaBarrasPorVersion(version);
            List<DpoRelSplFormulaDTO> listaFormulas = entity;
            if (barra != "-1")
            {
                listaFormulas = entity.Where(x => x.Barsplcodi == Int32.Parse(barra)).ToList();
            }
            //DpoRelSplFormulaDTO formulas = entity.Where(x => x.Barsplcodi == Int32.Parse(barra)).FirstOrDefault();
            //formulas vegetativas
            foreach (var formulas in listaFormulas)
            {
                if (formulas.Ptomedicodifveg != null)
                {
                    MePerfilRuleDTO veg = this.servicioScada.GetByIdMePerfilRule((int)formulas.Ptomedicodifveg);
                    this.DescomponerFormula(veg.Prruformula);
                }
                //formulas usuarios libres
                if (formulas.Ptomedicodiful != null)
                {
                    MePerfilRuleDTO ul = this.servicioScada.GetByIdMePerfilRule((int)formulas.Ptomedicodiful);
                    this.DescomponerFormula(ul.Prruformula);
                }
            }

            switch (fuente)
            {
                case ConstantesDpo.fuenteSIRPIT:
                    List<int> sirpit = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSirpit).Select(x => x.Codigo).Distinct().ToList();
                    //List<string> trafoExcel = this.servicio.ListaTransformadoresDPO()
                    //                                            .Where(x => sirpit.Contains(x.Dpotnfcodi))
                    //                                            .Select(x => x.Dpotnfcodiexcel)
                    //                                            .ToList();
                    if (sirpit.Count > 0) 
                    {
                        string cargaSIRPIT = string.Join(", ", sirpit);
                        List<DpoDatos96DTO> listaDatosSirpit = this.servicio.ListMedidorDemandaSirpit(cargaSIRPIT, inicio, fin, parametro);
                        foreach (DpoDatos96DTO d in listaDatosSirpit)
                        {
                            DpoGrillaDatos96 grilla = new DpoGrillaDatos96();
                            grilla.Fecha = d.Dpodatfecha.ToString(ConstantesDpo.FormatoFecha);
                            grilla.NombreTransformador = d.Dpodattnfcodi;
                            grilla.NombreBarra = d.Tnfbarbarcodi;
                            grilla.NombrePunto = "No";
                            grilla.Total = d.Meditotal;
                            for (int i = 1; i <= 96; i++)
                            {
                                var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                grilla.GetType().GetProperty("H" + i.ToString()).SetValue(grilla, (decimal)dValor);
                            }
                            datos.Add(grilla);
                        }
                    }
                    break;

                case ConstantesDpo.fuenteSICLI:
                    List<int> sicli = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSicli).Select(x => x.Codigo).Distinct().ToList();
                    if (sicli.Count() > 0) 
                    {
                        string cargaSICLI = string.Join(",", sicli);
                        List<IioTabla04DTO> listaDatosSicli = this.servicio.ListMedidorDemandaSicli(cargaSICLI, inicio, fin, parametro);
                        foreach (IioTabla04DTO d in listaDatosSicli)
                        {
                            DpoGrillaDatos96 grilla = new DpoGrillaDatos96();
                            grilla.Fecha = d.FechaMedicion.ToString(ConstantesDpo.FormatoFecha);
                            grilla.NombrePunto = d.NombrePtoMedicion;
                            grilla.NombreTransformador = "No";
                            grilla.NombreBarra = "No";
                            grilla.Total = d.Meditotal;
                            for (int i = 1; i <= 96; i++)
                            {
                                var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                grilla.GetType().GetProperty("H" + i.ToString()).SetValue(grilla, (decimal)dValor);
                            }
                            datos.Add(grilla);
                        }
                    }
                    break;

                case ConstantesDpo.fuenteTNA:
                    string tipos = (parametro == 1) ? ConstantesDpo.OrigenActiva : ConstantesDpo.OrigenReactiva;
                    List<int> tna = ListaItemFormula.Where(x => x.Tipo == ConstantesProdem.OrigenTnaDpo).Select(x => x.Codigo).Distinct().ToList();
                    if (tna.Count() > 0) 
                    {
                        string cargaTNA = string.Join(",", tna);
                        List<DpoDemandaScoDTO> listaDatosTna = this.servicio.ListMedidorDemandaTna(cargaTNA, inicio, fin, tipos);
                        foreach (DpoDemandaScoDTO d in listaDatosTna)
                        {
                            DpoGrillaDatos96 grilla = new DpoGrillaDatos96();
                            grilla.Fecha = d.Medifecha.ToString(ConstantesDpo.FormatoFecha);
                            grilla.NombrePunto = d.Ptomedidesc;
                            grilla.NombreTransformador = "No";
                            grilla.NombreBarra = "No";
                            grilla.Total = d.Meditotal;
                            for (int i = 1; i <= 96; i++)
                            {
                                var dValor = d.GetType().GetProperty("H" + (i).ToString()).GetValue(d, null);
                                grilla.GetType().GetProperty("H" + i.ToString()).SetValue(grilla, (decimal)dValor);
                            }
                            datos.Add(grilla);
                        }
                    }
                    break;
            }
            return Json(datos);
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
        /// Exporta la data 
        /// </summary>
        /// <param name="cabecera">cabecera de la tabla</param>
        /// <param name="dato">datos de la tabla</param>
        /// <returns></returns>
        public JsonResult ExportarData(string[] cabecera, List<DpoGrillaDatos96> dato)
        {
            RelacionBarraModel model = new RelacionBarraModel();
            //string pathFile = @"D:\";
            string pathFile = ConfigurationManager.AppSettings[ConstantesDpo.MedidorDemandaPO].ToString();
            string filename = "Reporte Medidor de Demanda";
            string result = this.servicio.ExportarMedidorDemandaCSV(cabecera, dato, pathFile, filename);
            return Json(result);
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string path = @"D:\";
            string path = ConfigurationManager.AppSettings[ConstantesDpo.MedidorDemandaPO].ToString() + file;
            string app = Constantes.AppCSV;
            return File(path, app, sFecha + "_" + file);
        }
    }
}