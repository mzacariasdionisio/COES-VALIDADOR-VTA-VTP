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
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Servicios.Aplicacion.Scada;
using COES.Servicios.Aplicacion.Scada.Helper;
using System.IO;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class ConsultaPrefiltradoController : Controller
    {
        public List<FormulaItem> ListaItemFormula = new List<FormulaItem>();
        PronosticoDemandaAppServicio servicioPronostico = new PronosticoDemandaAppServicio();
        DemandaPOAppServicio servicio = new DemandaPOAppServicio();
        PerfilScadaServicio servicioScada = new PerfilScadaServicio();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult FormulaSpl()
        {
            List<DpoFiltro> cargas = new List<DpoFiltro>();
            PrnVersiongrpDTO verBase = new PrnVersiongrpDTO()
            {
                Vergrpcodi = -1,
                Vergrpnomb = "Versión Base"
            };
            List<PrnVersiongrpDTO> versiones = this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos);
            versiones.Insert(0, verBase);

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                TipoMensaje = "info",
                Mensaje = "Módulo de consulta de Fórmulas SPL",
                ListaCargas = cargas,
                ListaVersiones = versiones,//this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos),
                ListaVersionesPop = versiones,
                ListaFormulas = this.servicio.ListaVersiones(),// new List<DpoVersionRelacionDTO>()
                ListaFormulasDpo = new List<MePerfilRuleDTO>()
            };
            return PartialView(model);
        }

        public PartialViewResult DatosSicli()
        {
            List<DpoRelSplFormulaDTO> listaBarras = this.servicio.ListaBarrasxVersion(0);
            foreach (var item in listaBarras)
            {
                //formulas vegetativas
                if (item.Ptomedicodifveg != null)
                {
                    MePerfilRuleDTO veg = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodifveg);
                    this.DescomponerFormula(veg.Prruformula);
                }
                //formulas usuarios libres
                if (item.Ptomedicodiful != null)
                {
                    MePerfilRuleDTO ul = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodiful);
                    this.DescomponerFormula(ul.Prruformula);
                }
            }

            List<int> sicli = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSicli).Select(x => x.Codigo).ToList();
            string ptoSICLI = string.Join(", ", sicli.Distinct().ToList());

            PrnVersiongrpDTO verBase = new PrnVersiongrpDTO()
            {
                Vergrpcodi = -1,
                Vergrpnomb = "Versión Base"
            };
            List<PrnVersiongrpDTO> versiones = this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos);
            versiones.Insert(0, verBase);

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                TipoMensaje = "info",
                Mensaje = "Módulo de consulta de Datos SICLI",
                ListaVersiones = versiones,//this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos),
                ListaPuntosSICLI = (string.IsNullOrEmpty(ptoSICLI))
                                          ? new List<MePtomedicionDTO>()
                                          : this.servicio.ListaPuntoMedicionByLista(ptoSICLI)
            };
            return PartialView(model);
        }

        public PartialViewResult DatosSirpit()
        {
            List<DpoRelSplFormulaDTO> listaBarras = this.servicio.ListaBarrasxVersion(0);
            foreach (var item in listaBarras)
            {
                //formulas vegetativas
                if (item.Ptomedicodifveg != null)
                {
                    MePerfilRuleDTO veg = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodifveg);
                    this.DescomponerFormula(veg.Prruformula);
                }
                //formulas usuarios libres
                if (item.Ptomedicodiful != null)
                {
                    MePerfilRuleDTO ul = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodiful);
                    this.DescomponerFormula(ul.Prruformula);
                }
            }

            List<int> sirpit = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSirpit).Select(x => x.Codigo).ToList();
            string ptoSIRPIT = string.Join(", ", sirpit.Distinct().ToList());

            PrnVersiongrpDTO verBase = new PrnVersiongrpDTO()
            {
                Vergrpcodi = -1,
                Vergrpnomb = "Versión Base"
            };
            List<PrnVersiongrpDTO> versiones = this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos);
            versiones.Insert(0, verBase);

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                TipoMensaje = "info",
                Mensaje = "Módulo de consulta de Datos SIRPIT",
                ListaVersiones = versiones,
                ListaTransformadores = (string.IsNullOrEmpty(ptoSIRPIT))
                                              ? new List<DpoTrafoBarraDTO>()
                                              : this.servicio.ListTrafoBarraById(ptoSIRPIT)
            };
            return PartialView(model);
        }

        public PartialViewResult DatosTna()
        {
            List<DpoRelSplFormulaDTO> listaBarras = this.servicio.ListaBarrasxVersion(0);
            foreach (var item in listaBarras)
            {
                //formulas vegetativas
                if (item.Ptomedicodifveg != null)
                {
                    MePerfilRuleDTO veg = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodifveg);
                    this.DescomponerFormula(veg.Prruformula);
                }
                //formulas usuarios libres
                if (item.Ptomedicodiful != null)
                {
                    MePerfilRuleDTO ul = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodiful);
                    this.DescomponerFormula(ul.Prruformula);
                }
            }

            List<int> tna = ListaItemFormula.Where(x => x.Tipo == ConstantesProdem.OrigenTnaDpo).Select(x => x.Codigo).ToList();
            string ptoTNA = string.Join(", ", tna.Distinct().ToList());

            PrnVersiongrpDTO verBase = new PrnVersiongrpDTO()
            {
                Vergrpcodi = -1,
                Vergrpnomb = "Versión Base"
            };
            List<PrnVersiongrpDTO> versiones = this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos);
            versiones.Insert(0, verBase);

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                TipoMensaje = "info",
                Mensaje = "Módulo de consulta de Datos TNA",
                ListaPuntosTNA = (string.IsNullOrEmpty(ptoTNA))
                                        ? new List<MePtomedicionDTO>()
                                        : this.servicio.ListaPuntoMedicionByLista(ptoTNA),
                ListaVersiones = versiones,//this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos),
            };
            return PartialView(model);
        }

        /// <summary>
        /// Permite actualizar la lista de Formulas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecargaFormula()
        {
            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                ListaFormulas = this.servicio.ListaVersiones(),
                ListaCargas = new List<DpoFiltro>()
            };
            return Json(model);
        }

        /// <summary>
        /// Permite actualizar la lista de Cargas segun la formulaDPO seleccionada
        /// </summary>
        /// <param name="formulaDpo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecargaCarga(List<int> formulaDpo, int fuente)
        {
            List<DpoFiltro> cargas = new List<DpoFiltro>();

            foreach (var item in formulaDpo)
            {
                MePerfilRuleDTO formula = this.servicioScada.GetByIdMePerfilRule(item);
                this.DescomponerFormula(formula.Prruformula);
            }

            #region Datos por Tipo
            //TNA
            List<int> tna = ListaItemFormula.Where(x => x.Tipo == ConstantesProdem.OrigenTnaDpo).Select(x => x.Codigo).ToList();
            string ptoTNA = string.Join(", ", tna.Distinct().ToList());
            //SICLI
            List<int> sicli = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSicli).Select(x => x.Codigo).ToList();
            string ptoSICLI = string.Join(", ", sicli.Distinct().ToList());
            //SIRPIT
            List<int> sirpit = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSirpit).Select(x => x.Codigo).ToList();
            string ptoSIRPIT = string.Join(", ", sirpit.Distinct().ToList());
            #endregion

            List<MePtomedicionDTO> listado;
            switch (fuente)
            {
                case ConstantesDpo.fuenteSIRPIT:
                    if (!string.IsNullOrEmpty(ptoSIRPIT))
                    {
                        List<DpoTrafoBarraDTO> transformadores = this.servicio.ListTrafoBarraById(ptoSIRPIT);
                        cargas = transformadores.Select(x => new DpoFiltro
                        {
                            filtroCodigo = x.Tnfbarcodi.ToString(),
                            filtroNombre = x.Tnfbartnfcodi + " - " + x.Tnfbarbarnombre + " - " + x.Tnfbarbartension

                        }).ToList();
                    }
                    break;
                case ConstantesDpo.fuenteSICLI:
                    if (!string.IsNullOrEmpty(ptoSICLI))
                    {
                        listado = this.servicio.ListaPuntoMedicionByLista(ptoSICLI);
                        cargas = listado.Select(x => new DpoFiltro
                        {
                            filtroCodigo = x.Ptomedicodi.ToString(),
                            filtroNombre = x.Ptomedidesc

                        }).ToList();
                    }
                    break;
                case ConstantesDpo.fuenteTNA:
                    if (!string.IsNullOrEmpty(ptoTNA))
                    {
                        listado = this.servicio.ListaPuntoMedicionByLista(ptoTNA);
                        cargas = listado.Select(x => new DpoFiltro
                        {
                            filtroCodigo = x.Ptomedicodi.ToString(),
                            filtroNombre = x.Ptomedidesc

                        }).ToList();
                    }
                    break;
                case ConstantesDpo.fuenteFormulaSPL:
                    if (!string.IsNullOrEmpty(ptoTNA))
                    {
                        List<MePtomedicionDTO> ftna = this.servicio.ListaPuntoMedicionByLista(ptoTNA);
                        cargas.AddRange(ftna.Select(x => new DpoFiltro
                        {
                            filtroCodigo = ConstantesDpo.PrefijoTNA + x.Ptomedicodi.ToString(),
                            filtroNombre = x.Ptomedidesc

                        }).ToList());
                    }
                    if (!string.IsNullOrEmpty(ptoSICLI))
                    {
                        List<MePtomedicionDTO> fsicli = this.servicio.ListaPuntoMedicionByLista(ptoSICLI);
                        cargas.AddRange(fsicli.Select(x => new DpoFiltro
                        {
                            filtroCodigo = ConstantesDpo.PrefijoSICLI + x.Ptomedicodi.ToString(),
                            filtroNombre = x.Ptomedidesc

                        }).ToList());
                    }
                    if (!string.IsNullOrEmpty(ptoSIRPIT))
                    {
                        List<DpoTrafoBarraDTO> fsirpit = this.servicio.ListTrafoBarraById(ptoSIRPIT);
                        cargas.AddRange(fsirpit.Select(x => new DpoFiltro
                        {
                            filtroCodigo = ConstantesDpo.PrefijoSIRPIT + x.Tnfbarcodi.ToString(),
                            filtroNombre = x.Tnfbartnfcodi + " - " + x.Tnfbarbarnombre + " - " + x.Tnfbarbartension

                        }).ToList());
                    }
                    break;
            }

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                ListaCargas = cargas,
            };

            return Json(model);
        }

        /// <summary>
        /// Permite actualizar la lista de Areas demanda
        /// </summary>
        /// <param name="relacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecargaAreaDemanda(int relacion)
        {
            List<DpoRelSplFormulaDTO> listaRelaciones = this.servicio.ListaBarrasxVersion(relacion)
                                                                .Where(x => x.Splfrmarea != null)
                                                                .ToList();
            List<DpoRelSplFormulaDTO> listaAreaDemanda = listaRelaciones.Select(x => new DpoRelSplFormulaDTO
            {
                Splfrmarea = x.Splfrmarea,
                Splareanombre = x.Splareanombre
            }).ToList();
            List<DpoRelSplFormulaDTO> result = listaAreaDemanda.GroupBy(x => new { x.Splfrmarea, x.Splareanombre })
                                                .Select(y => y.First())
                                                .ToList();
            if (result.Count == 1 && result.FirstOrDefault().Splfrmarea == null)
            {
                result = new List<DpoRelSplFormulaDTO>();
            }

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                ListaCargas = new List<DpoFiltro>(),
                ListaAreaDemanda = result,
                ListaFormulasDpo = new List<MePerfilRuleDTO>()
            };

            return Json(model);
        }

        /// <summary>
        /// Permite actualizar la lista de Formulas DPO
        /// </summary>
        /// <param name="relacion"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecargaFormulas(int relacion, int area)
        {
            List<DpoFiltro> cargas = new List<DpoFiltro>();
            List<DpoRelSplFormulaDTO> listaRelaciones = this.servicio.ListaBarrasxVersion(relacion)
                                                            .Where(x => x.Splfrmarea == area).ToList();
            List<MePerfilRuleDTO> listaFormulasDPO = new List<MePerfilRuleDTO>();
            MePerfilRuleDTO entity;
            foreach (var item in listaRelaciones)
            {
                if (item.Ptomedicodifveg != null)
                {
                    entity = new MePerfilRuleDTO();
                    entity.Prrucodi = (int)item.Ptomedicodifveg;
                    entity.Prruabrev = item.Nombvegetativa;
                    listaFormulasDPO.Add(entity);
                }
                if (item.Ptomedicodiful != null)
                {
                    entity = new MePerfilRuleDTO();
                    entity.Prrucodi = (int)item.Ptomedicodiful;
                    entity.Prruabrev = item.Nombindustrial;
                    listaFormulasDPO.Add(entity);
                }
            }
            List<MePerfilRuleDTO> result = listaFormulasDPO.GroupBy(x => new { x.Prrucodi, x.Prruabrev })
                                                .Select(y => y.First())
                                                .ToList();

            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                ListaCargas = new List<DpoFiltro>(),
                ListaFormulasDpo = result
            };

            return Json(model);
        }

        /// <summary>
        /// Permite consultar la data a mostrar en las graficas
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="version"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="relacion"></param>
        /// <param name="carga"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarData(string anio, string[] mes, int version, int fuente, int variable,
                                        string relacion, List<string> carga, int formula)
        {
            List<DpoConsultaPrefiltrado> data = new List<DpoConsultaPrefiltrado>();
            //Para las fuentes SIRPIT, SICLI y TNA
            if (fuente != 4)
            {
                //Version Base
                if (version == -1)
                {
                    data = this.servicio.ConsultarDataFiltrada(anio, mes, version, fuente, variable, relacion, carga);
                }
                //Otras versiones
                else {
                    data = this.servicio.ConsultarDataVersionFiltrada(anio, mes, version, fuente, variable, relacion, carga);
                }
            }
            //Para FORMULAS SPL
            else {
                List<DpoConsultaPrefiltrado> temporal = new List<DpoConsultaPrefiltrado>();
                List<string> tna = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoTNA))
                                        .Select(y => y.Replace(ConstantesDpo.PrefijoTNA, "")).ToList();
                List<string> sirpit = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoSIRPIT))
                                           .Select(y => y.Replace(ConstantesDpo.PrefijoSIRPIT, "")).ToList();
                List<string> sicli = carga.Where(x => x.StartsWith(ConstantesDpo.PrefijoSICLI))
                                          .Select(y => y.Replace(ConstantesDpo.PrefijoSICLI, "")).ToList();

                if (version == -1)
                {
                    temporal.AddRange(this.servicio.ConsultarDataFiltrada(anio, mes, version, ConstantesDpo.fuenteSIRPIT, variable,
                                                                          relacion, sirpit));
                    temporal.AddRange(this.servicio.ConsultarDataFiltrada(anio, mes, version, ConstantesDpo.fuenteSICLI, variable,
                                                                          relacion, sicli));
                    temporal.AddRange(this.servicio.ConsultarDataFiltrada(anio, mes, version, ConstantesDpo.fuenteTNA, variable,
                                                                          relacion, tna));
                }
                else {
                    temporal.AddRange(this.servicio.ConsultarDataVersionFiltrada(anio, mes, version, ConstantesDpo.fuenteSIRPIT,
                                                                                 variable, relacion, sirpit));
                    temporal.AddRange(this.servicio.ConsultarDataVersionFiltrada(anio, mes, version, ConstantesDpo.fuenteSICLI,
                                                                                 variable, relacion, sicli));
                    temporal.AddRange(this.servicio.ConsultarDataVersionFiltrada(anio, mes, version, ConstantesDpo.fuenteTNA,
                                                                                 variable, relacion, tna));
                }

                foreach (var item in mes)
                {
                    int m = Int32.Parse(item);
                    List<DpoConsultaPrefiltrado> t = temporal.Where(x => x.id == m).ToList();
                    decimal[] sumData = new decimal[t[0].dias];
                    if (t.Count > 0) {
                        sumData = t.Select(objeto => objeto.data)
                                             .Aggregate((acumulado, arreglo) =>
                                             acumulado.Zip(arreglo, (a, b) => a + b).ToArray());
                    }
                    DpoConsultaPrefiltrado e = new DpoConsultaPrefiltrado()
                    {
                        id = t[0].id,
                        dias = t[0].dias,
                        diasHora = t[0].diasHora,//{"Enero", "Febrero"}
                        name = t[0].name,
                        data = sumData
                    };
                    data.Add(e);
                }
            }


            //MePerfilRuleDTO datosFormula = this.servicioScada.GetByIdMePerfilRule((int)formula);
            //this.DescomponerFormula(datosFormula.Prruformula);
            //List<DpoConsultaPrefiltrado> resultFormula = this.servicio.CalculoFormula(ListaItemFormula, Int32.Parse(anio), mes, variable, version);
            MePerfilRuleDTO datosFormula = new MePerfilRuleDTO();
            List<DpoConsultaPrefiltrado> resultFormula = new List<DpoConsultaPrefiltrado>();
            ////Se debe calcular la formula
            if (relacion != "N")
            {
                datosFormula = this.servicioScada.GetByIdMePerfilRule((int)formula);
                this.DescomponerFormula(datosFormula.Prruformula);
                resultFormula = this.servicio.CalculoFormula(ListaItemFormula, Int32.Parse(anio), mes, variable, version);
            }

            //List<DpoConsultaPrefiltrado> resultFormula = this.servicio.CalculoFormula(ListaItemFormula, Int32.Parse(anio), mes, variable, version);

            // Se debe recorrer data y analizarla para por dia obtener un: Maximo y Minimo
            DpoConsultaPrefiltrado maximo = new DpoConsultaPrefiltrado();
            DpoConsultaPrefiltrado minimo = new DpoConsultaPrefiltrado();
            List<int> posiciones = new List<int>();
            this.servicio.CalculoMaximoMinimo(data, out maximo, out minimo, out posiciones);
            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                ListaData = data,
                ListaMaximo = maximo,
                ListaMinimo = minimo,
                ListaPosiciones = posiciones,
                ListaAlgoritmo = resultFormula
            };

            return Json(model);
        }

        /// <summary>
        /// Permite corregir la data en base a un metodo
        /// </summary>
        /// <param name="metodo"></param>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="version"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="relacion"></param>
        /// <param name="carga"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CorregirData(int metodo, int anio, string[] mes, int fuente, int variable,
                                     int relacion, List<string> carga, int version, int area, int formulaDpo)
        {
            List<DpoConsultaPrefiltrado> dataMeses = new List<DpoConsultaPrefiltrado>();
            List<DpoMedicion96DTO> data = new List<DpoMedicion96DTO>();
            data = this.servicio.ConsultaDatosPorPunto(anio, mes, fuente, variable, relacion, carga,
                                                       metodo, version, User.Identity.Name);

            List<DpoRelSplFormulaDTO> listaBarras = this.servicio.ListaBarrasxVersion(relacion)
                                                                .Where(x => x.Splfrmarea == area && 
                                                                            (x.Ptomedicodifveg == formulaDpo 
                                                                            || x.Ptomedicodiful == formulaDpo))
                                                                .ToList();
            List<DpoMedicion96DTO> dataCorregida = new List<DpoMedicion96DTO>();
            List<FormulaItem> ListaFormula = new List<FormulaItem>();
            string mensajeBarras = string.Empty;
            string mensajeTipo = string.Empty;
            List<string> barras = new List<string>();

            foreach (var item in listaBarras)
            {
                ListaItemFormula.Clear();
                //formulas vegetativas
                if (item.Ptomedicodifveg == formulaDpo)
                {
                    MePerfilRuleDTO veg = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodifveg);
                    this.DescomponerFormula(veg.Prruformula);
                }
                //formulas usuarios libres
                if (item.Ptomedicodiful == formulaDpo)
                {
                    MePerfilRuleDTO ul = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodiful);
                    this.DescomponerFormula(ul.Prruformula);
                }

                ListaFormula.AddRange(ListaItemFormula);
                #region Datos por Tipo
                //TNA
                List<int> tna = ListaItemFormula.Where(x => x.Tipo == ConstantesProdem.OrigenTnaDpo).Select(x => x.Codigo).ToList();
                string ptoTNA = string.Join(", ", tna.Distinct().ToList());
                //SICLI
                List<int> sicli = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSicli).Select(x => x.Codigo).ToList();
                string ptoSICLI = string.Join(", ", sicli.Distinct().ToList());
                //SIRPIT
                List<int> sirpit = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSirpit).Select(x => x.Codigo).ToList();
                string ptoSIRPIT = string.Join(", ", sirpit.Distinct().ToList());
                #endregion

                #region Validacion de relacion barra - punto
                //Inicio - Armando las fechas de consulta para el hsitorico
                DateTime tempFechaInicio = new DateTime(anio, Int32.Parse(mes.First()), 1);
                DateTime fechaInicio = tempFechaInicio.AddMonths(-12);//AJUSTAR A LOS MESES QUE SE DESEA TENER DE HISTORICO
                DateTime fechaFin = new DateTime(anio, Int32.Parse(mes.Last()), DateTime.DaysInMonth(anio, Int32.Parse(mes.Last())));
                //Fin - Armando las fechas de consulta para el hsitorico

                DpoRelacionPtoBarraDTO dPunto = this.servicio.GetPuntoById(item.Splfrmcodi);

                if (metodo == 3 && dPunto.Ptomedicodi == 0)
                {
                    barras.Add(item.Gruponomb);
                }
                #endregion

                dataCorregida.AddRange(this.servicio.CorrecionDatosFormulas(metodo, data, tna, sicli, sirpit, fuente,
                                                                            item.Splfrmcodi, anio, mes, variable));
            }

            //mensaje para informar las barras no relaciondas a puntos
            if (barras.Count > 0)
            {
                mensajeBarras = "La Barra: " + string.Join(", ", barras.Distinct().ToList()) + " no esta relacionada a un punto TNA";
                mensajeTipo = "warning";
            }
            else {
                mensajeBarras = "Correccion realizada...";
                mensajeTipo = "success";
            }

            var result = dataCorregida
                         .GroupBy(x => x.Dpomedfecha)
                         .Select(y =>
                         {
                            var dto = new DpoMedicion96DTO { Dpomedfecha = y.Key };
                            for (int i = 1; i <= 96; i++)
                            {
                                string propertyName = "H" + i;
                                decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                            }
                            return dto;
                         })
                         .ToList();

            foreach (var item in mes)
            {
                int parseMes = Int32.Parse(item);
                DateTime lastDay = new DateTime(anio, parseMes,
                                                DateTime.DaysInMonth(anio, parseMes));
                DateTime firstDay = new DateTime(anio, parseMes, 1);
                List<decimal> dataMes = new List<decimal>();
                List<string> fechas = new List<string>();
                while (firstDay <= lastDay)
                {
                    fechas.AddRange(this.servicio.ListaFechaHora96(firstDay));
                    var temporal = result.Where(x => x.Dpomedfecha == firstDay).FirstOrDefault();
                    if (temporal != null)
                    {
                        dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                    }
                    else
                    {
                        dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                    }
                    firstDay = firstDay.AddDays(1);
                }
                DpoConsultaPrefiltrado entity = new DpoConsultaPrefiltrado()
                {
                    id = parseMes,
                    dias = DateTime.DaysInMonth(anio, parseMes),
                    diasHora = fechas,
                    name = DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                    data = dataMes.ToArray(),
                };
                dataMeses.Add(entity);
            }

            //Se debe calcular la formula
            //MePerfilRuleDTO datosFormula = this.servicioScada.GetByIdMePerfilRule((int)formulaDpo);
            //this.DescomponerFormula(datosFormula.Prruformula);
            //List<DpoConsultaPrefiltrado> resultFormula = this.servicio.CalculoFormula(ListaItemFormula, anio, mes, variable, version);
            List<DpoConsultaPrefiltrado> resultFormula = this.servicio.CalculoFormulaCorregida(ListaFormula, anio, mes, variable, version, dataCorregida);

            // Se debe recorrer data y analizarla para por dia obtener un: Maximo y Minimo
            DpoConsultaPrefiltrado maximo = new DpoConsultaPrefiltrado();
            DpoConsultaPrefiltrado minimo = new DpoConsultaPrefiltrado();
            List<int> posiciones = new List<int>();
            this.servicio.CalculoMaximoMinimo(dataMeses, out maximo, out minimo, out posiciones);
            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel()
            {
                ListaData = dataMeses,
                ListaMaximo = maximo,
                ListaMinimo = minimo,
                ListaPosiciones = posiciones,
                ListaAlgoritmo = resultFormula,
                Mensaje = mensajeBarras,
                TipoMensaje = mensajeTipo
            };

            return Json(model);
        }

        /// <summary>
        /// Permite grabar data corregida como una  nueva versión o actualizar una existente
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="fuente"></param>
        /// <param name="variable"></param>
        /// <param name="formula"></param>
        /// <param name="carga"></param>
        /// <param name="metodo"></param>
        /// <param name="versionOrigen"></param>
        /// <param name="versionDestino"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarData(int anio, string[] mes, int fuente, int variable, 
                                     int relacion, List<string> carga, int metodo, 
                                     int versionOrigen, string versionDestino, int opcion, 
                                     int area, int formulaDpo)
        {
            ConsultaPrefiltradoModel model = new ConsultaPrefiltradoModel();
            List<DpoConsultaPrefiltrado> dataMeses = new List<DpoConsultaPrefiltrado>();
            string meses = string.Join(", ", mes.Select(x => $"'{x}'"));
            List<DpoMedicion96DTO> data = new List<DpoMedicion96DTO>();
            try
            {
                //Consulta de la data
                data = this.servicio.ConsultaCargas96(anio, meses, fuente, variable, carga, versionOrigen, User.Identity.Name);

                //Correcion de la data
                List<DpoRelSplFormulaDTO> listaBarras = this.servicio.ListaBarrasxVersion(relacion)
                                                                      .Where(x => x.Splfrmarea == area)
                                                                      .ToList();
                List<DpoMedicion96DTO> dataCorregida = new List<DpoMedicion96DTO>();
                List<FormulaItem> ListaFormula = new List<FormulaItem>();

                foreach (var item in listaBarras)
                {
                    ListaItemFormula.Clear();
                    //formulas vegetativas
                    if (item.Ptomedicodifveg == formulaDpo)
                    {
                        MePerfilRuleDTO veg = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodifveg);
                        this.DescomponerFormula(veg.Prruformula);
                    }
                    //formulas usuarios libres
                    if (item.Ptomedicodiful == formulaDpo)
                    {
                        MePerfilRuleDTO ul = this.servicioScada.GetByIdMePerfilRule((int)item.Ptomedicodiful);
                        this.DescomponerFormula(ul.Prruformula);
                    }

                    ListaFormula.AddRange(ListaItemFormula);
                    #region Datos por Tipo
                    //TNA
                    List<int> tna = ListaItemFormula.Where(x => x.Tipo == ConstantesProdem.OrigenTnaDpo).Select(x => x.Codigo).ToList();
                    string ptoTNA = string.Join(", ", tna.Distinct().ToList());
                    //SICLI
                    List<int> sicli = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSicli).Select(x => x.Codigo).ToList();
                    string ptoSICLI = string.Join(", ", sicli.Distinct().ToList());
                    //SIRPIT
                    List<int> sirpit = ListaItemFormula.Where(x => x.Tipo == ConstantesDpo.OrigenSirpit).Select(x => x.Codigo).ToList();
                    string ptoSIRPIT = string.Join(", ", sirpit.Distinct().ToList());
                    #endregion

                    dataCorregida.AddRange(this.servicio.CorrecionDatosFormulas(metodo, data, tna, sicli, sirpit, fuente,
                                                                                item.Splfrmcodi, anio, mes, variable));
                }

                var result = dataCorregida
                                .GroupBy(x => x.Dpomedfecha)
                                .Select(y =>
                                {
                                    var dto = new DpoMedicion96DTO { Dpomedfecha = y.Key };
                                    for (int i = 1; i <= 96; i++)
                                    {
                                        string propertyName = "H" + i;
                                        decimal? sum = y.Sum(x => (decimal?)x.GetType().GetProperty(propertyName)?.GetValue(x));
                                        dto.GetType().GetProperty(propertyName)?.SetValue(dto, sum);
                                    }
                                    return dto;
                                })
                                .ToList();

                foreach (var item in mes)
                {
                    int parseMes = Int32.Parse(item);
                    DateTime lastDay = new DateTime(anio, parseMes,
                                                    DateTime.DaysInMonth(anio, parseMes));
                    DateTime firstDay = new DateTime(anio, parseMes, 1);
                    List<decimal> dataMes = new List<decimal>();
                    List<string> fechas = new List<string>();
                    while (firstDay <= lastDay)
                    {
                        fechas.AddRange(this.servicio.ListaFechaHora96(firstDay));
                        var temporal = result.Where(x => x.Dpomedfecha == firstDay).FirstOrDefault();
                        if (temporal != null)
                        {
                            dataMes.AddRange(UtilProdem.ConvertirMedicionEnArreglo(ConstantesProdem.Itv15min, temporal).ToList());

                        }
                        else
                        {
                            dataMes.AddRange(Enumerable.Repeat(0m, ConstantesProdem.Itv15min).ToArray().ToList());
                        }
                        firstDay = firstDay.AddDays(1);
                    }
                    DpoConsultaPrefiltrado entity = new DpoConsultaPrefiltrado()
                    {
                        id = parseMes,
                        dias = DateTime.DaysInMonth(anio, parseMes),
                        diasHora = fechas,
                        name = DateTimeFormatInfo.CurrentInfo.GetMonthName(parseMes),
                        data = dataMes.ToArray(),
                    };
                    dataMeses.Add(entity);
                }

                //Se debe calcular la formula
                //MePerfilRuleDTO datosFormula = this.servicioScada.GetByIdMePerfilRule((int)formulaDpo);
                //this.DescomponerFormula(datosFormula.Prruformula);
                List<DpoConsultaPrefiltrado> resultFormula = this.servicio.CalculoFormula(ListaItemFormula, anio, mes, variable, versionOrigen);

                // Se debe recorrer data y analizarla para por dia obtener un: Maximo y Minimo
                DpoConsultaPrefiltrado maximo = new DpoConsultaPrefiltrado();
                DpoConsultaPrefiltrado minimo = new DpoConsultaPrefiltrado();
                List<int> posiciones = new List<int>();
                this.servicio.CalculoMaximoMinimo(dataMeses, out maximo, out minimo, out posiciones);
                model.ListaData = dataMeses;
                model.ListaMaximo = maximo;
                model.ListaMinimo = minimo;
                model.ListaAlgoritmo = resultFormula;

                //Grabado de la data
                this.servicio.SaveDatosCorregidos96(dataCorregida, versionDestino, opcion, User.Identity.Name);
                model.Mensaje = "Se registro la relacion con exito...";
                model.TipoMensaje = "success";
                PrnVersiongrpDTO verBase = new PrnVersiongrpDTO() {
                    Vergrpcodi = -1,
                    Vergrpnomb = "Versión Base"
                };
                List<PrnVersiongrpDTO> versiones = this.servicio.ListVersionByArea(ConstantesDpo.datosCorregidos);
                model.ListaVersionesPop = versiones;
                versiones.Insert(0, verBase);
                model.ListaVersiones = versiones;
                return Json(model);
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message + " - " + ex.StackTrace;
                model.TipoMensaje = "error";
                return Json(model);
            }
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
        /// Permite exportar la data de los meses mostrados en la grilla
        /// </summary>
        /// <param name="data"></param>
        /// <param name="version"></param>
        /// <param name="variable"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarData(List<DpoFormulaCorregidaFormmato> data, string version, string variable, string formula)
        {
            string pathFile = ConfigurationManager.AppSettings[ConstantesDpo.MedidorDemandaPO].ToString();
            string filename = "Reporte Fórmulas";
            string result = this.servicio.ExportarFormulasCSV(data, pathFile, filename, version, variable, formula);

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