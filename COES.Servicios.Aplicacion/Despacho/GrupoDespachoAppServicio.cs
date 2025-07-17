using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace COES.Servicios.Aplicacion.Despacho
{
    /// <summary>
    /// Clase para manejo de logica del arbol de grupos de despacho
    /// </summary>
    public class GrupoDespachoAppServicio
    {
        /// <summary>
        /// Codigo concepto de indicador de curva CMgN
        /// </summary>
        int Concepcodi = 503;

        /// <summary>
        /// Permite obtener las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerListaEmpresas()
        {
            return (new IEODAppServicio()).ListarEmpresasTienenCentralGenxTipoEmpresa(ConstantesAppServicio.ParametroDefecto);
        }

        /// <summary>
        /// Permite obtener las centrales por tipo
        /// </summary>
        /// <param name="tipoGrupo"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerCentrales(string tipoGrupo, string empresas)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = (-1).ToString();
            return FactorySic.GetPrGrupoRepository().ObtenerCentralesPorTipo(tipoGrupo, empresas);
        }
        /// <summary>
        /// Permite obtener las centrales por despacho
        /// <param name="empresas"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="pageSize"></param>
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListGruposDespachoPorCentral(string empresas, int nroPaginas, int pageSize)
        {

            return FactorySic.GetPrGrupoRepository().ListaGruposxDespacho(empresas, nroPaginas, pageSize);
        }

        public List<SiFuenteenergiaDTO> ListFuente()
        {
            return FactorySic.GetSiFuenteenergiaRepository().List();
        }

        public List<PrTipogrupoDTO> ListaTipoGrupo()
        {
            return FactorySic.GetPrTipogrupoRepository().List();
        }

        /// <summary>
        /// Permite obtener la cantidad de datos la lista de despacho
        /// <param name="idEmpresa"></param>
        /// <param name="sestado"></param>
        /// </summary>
        /// <returns></returns>
        public int ListGruposDespachoPorCentralXFiltro(string empresas)
        {
            return FactorySic.GetPrGrupoRepository().GruposxDespachoXFiltro(empresas);
        }

        public List<EmpresaDTO> ListarEmpresas()
        {
            return FactorySic.ObtenerEventoDao().ListarEmpresas();
        }

        public List <PrGrupoDTO> ListarGrupos()
        {
            return FactorySic.GetPrGrupoRepository().ListarGrupoDespacho();
        }


        public PrGrupoDTO GetByIdPrGrupo(int grupocodi)
        {
            return FactorySic.GetPrGrupoRepository().GetById(grupocodi);
        }
        


        /// <summary>
        /// Permite obtener el arbol de grupos
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerArbolGrupos(string empresas, string tipoCentral, string central)
        {
            if (string.IsNullOrEmpty(empresas)) empresas = (-1).ToString();
            List<PrGrupoDTO> list = FactorySic.GetPrGrupoRepository().ObtenerArbolGrupoDespacho(empresas, tipoCentral);

            List<int> centrales = new List<int>();
            if (!string.IsNullOrEmpty(central))
            {
                centrales = central.Split(ConstantesAppServicio.CaracterComa).Select(i => int.Parse(i)).ToList();
            }

            if (centrales.Count > 0)
            {
                list.RemoveAll(x => !centrales.Contains(x.Grupocodi) && x.Grupopadre == -1);
            }

            return list;
        }

        /// <summary>
        /// Permite filtrar los modos de operación
        /// </summary>
        /// <param name="list"></param>
        /// <param name="centrales"></param>
        /// <returns></returns>
        private List<int> FiltrarModosOperacion(List<PrGrupoDTO> list, List<int> centrales)
        {
            List<int> result = new List<int>();
            List<PrGrupoDTO> padres = list.Where(x => x.Grupopadre == -1 && centrales.Contains(x.Grupocodi)).ToList();

            foreach (PrGrupoDTO item in padres)
            {
                List<PrGrupoDTO> grupos = list.Where(x => x.Grupopadre == item.Grupocodi).ToList();

                foreach (PrGrupoDTO grupo in grupos)
                {
                    List<PrGrupoDTO> modos = list.Where(x => x.Grupopadre == grupo.Grupocodi).ToList();
                    result.AddRange(modos.Select(x => x.Grupocodi).Distinct().ToList());
                }
            }

            return result; 
        }

        /// <summary>
        /// Permite obtener los modos de operacion dependiendo
        /// </summary>
        /// <param name="list"></param>
        /// <param name="grupocodi"></param>
        /// <param name="catecodi"></param>
        /// <returns></returns>
        private List<int> FiltrarModosPorGrupo(List<PrGrupoDTO> list, int grupocodi, int catecodi)
        {
            List<int> result = new List<int>();

            if (catecodi == ConstantesDespacho.CategoriaCentralTermica)
            {
                List<int> padres = list.Where(x => x.Grupopadre == grupocodi).Select(x => x.Grupocodi).Distinct().ToList();
                foreach (int padre in padres)
                {
                    result.AddRange(list.Where(x => x.Grupopadre == padre).Select(x => x.Grupocodi).Distinct().ToList());
                }
            }
            else if (catecodi == ConstantesDespacho.CategoriaGrupoTermico)
            {

                result = list.Where(x => x.Grupopadre == grupocodi).Select(x => x.Grupocodi).Distinct().ToList();

            }
            else if (catecodi == ConstantesDespacho.CategoriaModoTermico)
            {

                result = list.Where(x => x.Grupocodi == grupocodi).Select(x => x.Grupocodi).Distinct().ToList();
            }

            return result;
        }

        // <summary>
        /// Permite actualizar el dato del tipo de curva a utilizar en los CMgN
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="tipo"></param>
        public void ActualizarCurvaCMgN(int idGrupo, string tipo, string lastUser)
        {
            FactorySic.GetPrGrupodatRepository().ActualizarParametro(idGrupo, this.Concepcodi, tipo, lastUser);
        }

        /// <summary>
        /// Permite obtener los valores para las curvas
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CurvaConsumo> ObtenerPametrosCurva(string empresas, string idgrupos, DateTime fecha, string tipoCentral, string central)
        {
            n_parameter parameter = new n_parameter();
            List<CurvaConsumo> result = new List<CurvaConsumo>();
            if (string.IsNullOrEmpty(empresas)) empresas = (-1).ToString();
            List<PrGrupodatDTO> list = FactorySic.GetPrGrupodatRepository().ObtenerParametrosCurvasConsumoCombustible(
                empresas, fecha);

            // Debemos filtrar los grupos seleccionados del arbol
            if (!string.IsNullOrEmpty(idgrupos))
            {
                string[] campos = idgrupos.Split(ConstantesAppServicio.CaracterComa);
                int idGrupo = Convert.ToInt32(campos[0]);
                int cateCodi = Convert.ToInt32(campos[1]);
                List<PrGrupoDTO> listGrupos = FactorySic.GetPrGrupoRepository().ObtenerArbolGrupoDespacho(empresas, tipoCentral);
                List<int> modosOperacion = this.FiltrarModosPorGrupo(listGrupos, idGrupo, cateCodi);
                list.RemoveAll(x => !modosOperacion.Contains(x.Grupocodi));
            }
            else
            {
                List<int> centrales = new List<int>();
                if (!string.IsNullOrEmpty(central))
                {
                    centrales = central.Split(ConstantesAppServicio.CaracterComa).Select(i => int.Parse(i)).ToList();

                    if (centrales.Count > 0)
                    {
                        List<PrGrupoDTO> listGrupos = FactorySic.GetPrGrupoRepository().ObtenerArbolGrupoDespacho(empresas, tipoCentral);
                        List<int> modosOperacion = this.FiltrarModosOperacion(listGrupos, centrales);
                        list.RemoveAll(x => !modosOperacion.Contains(x.Grupocodi));
                    }
                }
            }

            var grupos = list.Select(x => new { x.Grupocodi, x.GrupoNomb, x.Curvcodi }).Distinct().ToList();

            int[] categorias = { 243, 14, 175, 176, 177, 178, 179, 180, 181, 182, 183, 708, 709 };

            foreach (var grupo in grupos)
            {
                CurvaConsumo item = new CurvaConsumo();
                item.Grupocodi = grupo.Grupocodi;
                item.Gruponomb = grupo.GrupoNomb;
                item.Curvcodi = grupo.Curvcodi;

                #region Obtencion de puntos

                SerieCurva serieGrupo = new SerieCurva();

                List<PrGrupodatDTO> subList = list.Where(x => x.Grupocodi == grupo.Grupocodi).ToList();

                foreach (PrGrupodatDTO itemConcepto in subList)
                {
                    if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                        parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                }
                PrGrupodatDTO categoriaCurva = subList.Where(x => x.Concepcodi == categorias[0]).FirstOrDefault();
                PrGrupodatDTO tipoCurva = subList.Where(x => x.Concepcodi == this.Concepcodi).FirstOrDefault();
                if (tipoCurva!=null) {
                    item.Indcurva = tipoCurva.Formuladat;
                }
                
                // Leemos los puntos de la curva de consumo
                List<PuntoCurva> puntosConsumo = new List<PuntoCurva>();

                if (categoriaCurva != null)
                {
                    string[] strPuntos = categoriaCurva.Formuladat.Split(ConstantesAppServicio.CaracterNumeral);

                    foreach (string strPunto in strPuntos)
                    {
                        string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                        if (strCorrdenada.Length == 2)
                        {
                            decimal x = 0;
                            decimal y = 0;

                            if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                            {
                                puntosConsumo.Add(new PuntoCurva
                                {
                                    PuntoX = x,
                                    PuntoY = y
                                });
                            }
                        }
                    }
                }

                serieGrupo.SerieConsumo = puntosConsumo;

                // Leemos los puntos de los ensayos de potencia
                List<PuntoCurva> puntosEnsayo = new List<PuntoCurva>();

                for (int i = 1; i < categorias.Length - 1; i += 2)
                {
                    PrGrupodatDTO puntox = subList.Where(x => x.Concepcodi == categorias[i]).FirstOrDefault();
                    PrGrupodatDTO puntoy = subList.Where(x => x.Concepcodi == categorias[i + 1]).FirstOrDefault();

                    if (puntox != null && puntoy != null)
                    {
                        decimal x = 0;
                        decimal y = 0;
                        if (decimal.TryParse(puntox.Formuladat, out x) && decimal.TryParse(Convert.ToString(parameter.GetEvaluate(puntoy.Formuladat)), out y))
                        {
                            puntosEnsayo.Add(new PuntoCurva
                            {
                                PuntoX = x,
                                PuntoY = y
                            });
                        }
                    }
                }

                serieGrupo.SerieEnsayo = puntosEnsayo;

                if (puntosEnsayo.Count > 0 || puntosConsumo.Count > 0)
                {
                    item.ListaSerie = serieGrupo;
                    result.Add(item);
                }

                #endregion
            }

            return result;
        }

        #region Ajustar Curva Consumo Combustible

        /// <summary>
        /// Permite obtener los valores para las curvas por grupocodi
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CurvaConsumo> ObtenerPametrosCurvaPorGrupoCodi(string grupocodi, DateTime fecha)
        {
            List<CurvaConsumo> result = new List<CurvaConsumo>();
            List<PrGrupodatDTO> list = FactorySic.GetPrGrupodatRepository().ObtenerParametrosCurvasConsumoCombustibleporGrupoCodi(grupocodi, fecha);
            int[] categorias = { 243, 14, 175, 176, 177, 178, 179, 180, 181, 182, 183, 708, 709 };


            if (list.Count() > 0)
            {

                for (int i = 0; i < list.Count(); i++)
                {

                    CurvaConsumo item = new CurvaConsumo();

                    //para un mismo grupo codi, todos los resultados tienen igual el codigo y el nombre
                    item.Grupocodi = list[i].Grupocodi;
                    item.Gruponomb = list[i].GrupoNomb;
                    item.Fechadat = list[i].Fechadat.ToString();
                    item.Fechaact = list[i].Fechaact.ToString(); ;
                    item.Lastuser = list[i].Lastuser;

                    SerieCurva serieGrupo = new SerieCurva();
                    PrGrupodatDTO categoriaCurva = list[i];
                    List<PuntoCurva> puntosConsumo = new List<PuntoCurva>();

                    if (categoriaCurva != null)
                    {
                        string[] strPuntos = categoriaCurva.Formuladat.Split(ConstantesAppServicio.CaracterNumeral);

                        foreach (string strPunto in strPuntos)
                        {
                            string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                            if (strCorrdenada.Length == 2)
                            {
                                decimal x = 0;
                                decimal y = 0;

                                if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                                {
                                    puntosConsumo.Add(new PuntoCurva
                                    {
                                        PuntoX = x,
                                        PuntoY = y
                                    });
                                }
                            }
                        }
                    }

                    serieGrupo.SerieConsumo = puntosConsumo;

                    // Leemos los puntos de los ensayos de potencia
                    List<PuntoCurva> puntosEnsayo = new List<PuntoCurva>();

                    serieGrupo.SerieEnsayo = puntosEnsayo;

                    if (puntosEnsayo.Count > 0 || puntosConsumo.Count > 0)
                    {
                        item.ListaSerie = serieGrupo;
                        result.Add(item);
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Devuelve un listado de fechas
        /// </summary>
        /// <param name="fecha">fecha consulta</param>
        /// <returns></returns>
        public List<ReporteCostoIncrementalDTO> ListarCostoIncremental(DateTime fecha)
        {
            List<ReporteCostoIncrementalDTO> entitys = new List<ReporteCostoIncrementalDTO>();

            List<PrGrupoDTO> listGrupo = FactorySic.GetPrGrupoRepository().ListaModosOperacionActivos();
            var listaFormulasGenerales = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

            foreach (PrGrupoDTO item in listGrupo)
            {
                #region Obteniendo las propiedades de las térmicas

                ReporteCostoIncrementalDTO entity = ObtenerReporteCostoIncremental(fecha, listaFormulasGenerales, item);

                entitys.Add(entity);

                #endregion

            }

            return entitys;
        }

        /// <summary>
        /// Devuelve la lista de reporte de costos incrementales segun una lista de grupocodis (Horas de operación)
        /// </summary>
        /// <param name="grupoCods"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<ReporteCostoIncrementalDTO> ListarTodosCI(List<int> grupoCods, DateTime fecha)
        {
            // Obtener datos de cabecera
            fecha = fecha.Date;

            int repcv = 0;
            List<PrRepcvDTO> listaRepcv = FactorySic.GetPrRepcvRepository().GetByCriteria(fecha.AddMonths(-1), fecha).ToList();

            if (listaRepcv.Any())
            {
                // Nos quedamos con una sola cabecera
                repcv = listaRepcv.OrderBy(p => p.Reptipo).ThenByDescending(p => p.Repnomb.Substring(p.Repnomb.Length)).ThenByDescending(p => p.Lastdate).Take(1).Single().Repcodi;
            }

            // Obtener datos de detalle
            List<PrCvariablesDTO> listaCVariables = FactorySic.GetPrCvariablesRepository().GetCostosVariablesPorRepCv(repcv);

            // Filtros para quedarnos sólo con los Modos de Operación activos
            List<PrGrupoDTO> listGruposActivos = FactorySic.GetPrGrupoRepository().ListaModosOperacionActivos();
            listaCVariables = listaCVariables.Where(p => listGruposActivos.Select(q => q.Grupocodi).Contains(p.Grupocodi)).ToList();
            
            // Preguntamos si hay información en la BD
            if (listaCVariables.Any(p => p.CIncremental1 > 0))
            {
                List<ReporteCostoIncrementalDTO> lista = new List<ReporteCostoIncrementalDTO>();

                n_parameter parameter = new n_parameter();
                double CCombXArr = parameter.GetEvaluate(ConstantesCortoPlazo.CcombXArr);
                double CCombXPar = parameter.GetEvaluate(ConstantesCortoPlazo.CcombXPar);
                double TCambio = parameter.GetEvaluate(ConstantesCortoPlazo.TCambio);

                foreach (var item in listaCVariables)
                {
                    lista.Add(new ReporteCostoIncrementalDTO()
                    {
                        CEC = item.CecSi.ToString(),
                        Pe = item.Pe.ToString(),
                        Rendimiento = item.RendSi.ToString(),
                        Precio = item.Ccomb.ToString(),
                        CVNC = item.Cvnc.ToString(),
                        CVC = item.Cvc.ToString(),
                        CV = (item.Cvnc + item.Cvc).ToString(),
                        GrupoModoOperacion = item.Gruponomb,
                        Empresa = item.Emprnomb,
                        TipoCombustible = item.TipoCombustible,
                        Cincrem1 = Convert.ToDouble(item.CIncremental1),
                        Cincrem2 = Convert.ToDouble(item.CIncremental2),
                        Cincrem3 = Convert.ToDouble(item.CIncremental3),
                        Tramo1 = item.Tramo1,
                        Tramo2 = item.Tramo2,
                        Tramo3 = item.Tramo3,
                        Pe1 = item.Pe1 ?? 0,
                        Pe2 = item.Pe2 ?? 0,
                        Pe3 = item.Pe3 ?? 0,
                        Pe4 = item.Pe4 ?? 0,
                        Grupocodi = item.Grupocodi,
                        //Grupopadre = item.Grupo,
                        TipoGenerRer = item.TipoGenerRer,
                        Grupotipocogen = item.Grupotipocogen,
                        CCombXArr = CCombXArr,
                        CCombXPar = CCombXPar,
                        TCambio = TCambio
                    });
                }

                return lista.OrderBy(p => p.Empresa).ThenBy(p => p.Grupocodi).ToList();
            }
            else
            {
                ConcurrentBag<ReporteCostoIncrementalDTO> entitys = new ConcurrentBag<ReporteCostoIncrementalDTO>();

                List<PrGrupoDTO> listGrupo = FactorySic.GetPrGrupoRepository().ListaModosOperacion();

                /////////////////
                listGrupo = listGrupo.Where(p => listGruposActivos.Select(q => q.Grupocodi).Contains(p.Grupocodi)).ToList();
                /////////////////
                
                List<PrGrupoDTO> listaFiltrada;

                if (grupoCods.Any())
                {
                    listaFiltrada = listGrupo.Where(x => grupoCods.Contains(x.Grupocodi)).ToList();
                }
                else
                {
                    listaFiltrada = listGrupo;
                }

                var listaFormulasGenerales = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

                ReporteCostoIncrementalDTO entity;

                Parallel.ForEach(listaFiltrada, new ParallelOptions { MaxDegreeOfParallelism = 500 }, (elementPrGrupoDTO, state) =>
                {
                    #region Obteniendo las propiedades de las térmicas

                    entity = ObtenerReporteCostoIncremental(fecha, listaFormulasGenerales, elementPrGrupoDTO);

                    entitys.Add(entity);

                    #endregion

                });

                return entitys.ToList();
            }
        }

        /// <summary>
        /// Obtener costo incremental de un modo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaFormulasGenerales"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ReporteCostoIncrementalDTO ObtenerReporteCostoIncremental(DateTime fecha, List<PrGrupodatDTO> listaFormulasGenerales, PrGrupoDTO item)
        {
            string idsGrupos = item.Grupocodi.ToString();
            if (item.Grupopadre > 0)
            {
                PrGrupoDTO padre = FactorySic.GetPrGrupoRepository().GetById((int)item.Grupopadre);
                idsGrupos = idsGrupos + ConstantesAppServicio.CaracterComa.ToString() + item.Grupopadre + ConstantesAppServicio.CaracterComa.ToString() + padre.Grupopadre;
            }

            List<PrGrupodatDTO> listFormulas = new List<PrGrupodatDTO>();
            listFormulas.AddRange(listaFormulasGenerales);
            listFormulas.AddRange(FactorySic.GetPrGrupodatRepository().ObtenerParametroModoOperacion(idsGrupos, fecha));

            // Declaramos el objeto parameter para calcular los valores
            n_parameter parameter = new n_parameter();
            foreach (PrGrupodatDTO itemConcepto in listFormulas)
            {
                if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                    parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
            }

            // Con esto sacamos las propiedades para el modo de operación como son curva de consumo, costo combustible, CV OYM

            double CEC = parameter.GetEvaluate(ConstantesDespacho.CEC);
            double PE = parameter.GetEvaluate(ConstantesDespacho.PE);
            double Rendimiento = parameter.GetEvaluate(ConstantesDespacho.Rendimiento);
            double CostoCombustible = parameter.GetEvaluate(ConstantesDespacho.CostoCombustible);
            double CVNC = parameter.GetEvaluate(ConstantesDespacho.CVNC);
            double CVC = parameter.GetEvaluate(ConstantesDespacho.CVC);
            string tipoCombustible = listFormulas.FirstOrDefault(x => x.Concepabrev == ConstantesDespacho.TipComb)?.Formuladat; //- Tipo de combustible
            double factorConversion = parameter.GetEvaluate(ConstantesCortoPlazo.PropFactorConversion); //PCI_SI       
            double CCombXArr = parameter.GetEvaluate(ConstantesCortoPlazo.CcombXArr);
            double CCombXPar = parameter.GetEvaluate(ConstantesCortoPlazo.CcombXPar);
            double TCambio = parameter.GetEvaluate(ConstantesCortoPlazo.TCambio);


            //- Factor de conversion
            string formulacostoCombustible = listFormulas.FirstOrDefault(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoCombustible)?.Formuladat;
            double formulaPCICostoCombustibleVarios = Convert.ToDouble(listFormulas.FirstOrDefault(x => x.Concepcodi == ConstantesDespacho.ConcepCodi_PCIPVariosSI)?.Formuladat);
            
            double factor = 1;
            if (!string.IsNullOrEmpty(formulacostoCombustible))
                if (formulacostoCombustible.ToUpper() == ConstantesCortoPlazo.UnidadCostoCombustible.ToUpper())
                    factor = factorConversion / 1000000;

            if (formulaPCICostoCombustibleVarios > 0)
                factor = formulaPCICostoCombustibleVarios / 1000000;

            //- Obteniendo los datos de la curva
            List<CoordenadaConsumo> curva = new List<CoordenadaConsumo>();
            string curvaAjustada = listFormulas.FirstOrDefault(x => x.Concepabrev == ConstantesCortoPlazo.PropCurvaAjustadaSPR)?.Formuladat; //- CoordConsumComb

            if (!string.IsNullOrEmpty(curvaAjustada))
            {
                string[] strPuntos = curvaAjustada.Split(ConstantesAppServicio.CaracterNumeral);

                foreach (string strPunto in strPuntos)
                {
                    string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                    if (strCorrdenada.Length == 2)
                    {
                        decimal x = 0;
                        decimal y = 0;

                        if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                        {
                            curva.Add(new CoordenadaConsumo
                            {
                                Potencia = x,
                                Consumo = y
                            });
                        }
                    }
                }
            }

            //cacular los tramos

            decimal f1 = 0;
            decimal f2 = 0;
            decimal f3 = 0;
            decimal f4 = 0;

            decimal p1 = 0;
            decimal p2 = 0;
            decimal p3 = 0;
            decimal p4 = 0;

            double m1 = 0.0;
            double m2 = 0.0;
            double m3 = 0.0;

            double cincremental1 = 0.0;
            double cincremental2 = 0.0;
            double cincremental3 = 0.0;

            string tramo1 = "";
            string tramo2 = "";
            string tramo3 = "";

            int i = 0;
            foreach (var cordenada in curva)
            {
                i += 1;
                switch (i)
                {
                    case 1:
                        {
                            f1 = cordenada.Consumo;
                            p1 = cordenada.Potencia;
                        }
                        break;
                    case 2:
                        {
                            f2 = cordenada.Consumo;
                            p2 = cordenada.Potencia;

                            m1 = (double)((f2 - f1) / (p2 - p1));

                            tramo1 = "[" + p1.ToString() + " - " + p2.ToString() + "]";

                            //if (tipoCombustible == "Gas Natural")
                            //    cincremental1 = 1000 * CVNC + m1 * CostoCombustible;
                            //else
                            //    cincremental1 = 1000 * CVNC + (1 / 1000000)* m1 * CostoCombustible * factorConversion;

                            cincremental1 = 1000 * CVNC + m1 * CostoCombustible * factor;

                        }
                        break;
                    case 3:
                        {
                            f3 = cordenada.Consumo;
                            p3 = cordenada.Potencia;

                            m2 = (double)((f3 - f2) / (p3 - p2));

                            tramo2 = "[" + p2.ToString() + " - " + p3.ToString() + "]";

                            //if (tipoCombustible == "Gas Natural")
                            //    cincremental2 = 1000 * CVNC + m2 * CostoCombustible;
                            //else
                            //    cincremental2 = 1000 * CVNC + (1 / 1000000) * m2 * CostoCombustible * factorConversion;

                            cincremental2 = 1000 * CVNC + m2 * CostoCombustible * factor;

                        }
                        break;
                    case 4:
                        {
                            f4 = cordenada.Consumo;
                            p4 = cordenada.Potencia;

                            m3 = (double)((f4 - f3) / (p4 - p3));

                            tramo3 = "[" + p3.ToString() + " - " + p4.ToString() + "]";

                            //if (tipoCombustible == "Gas Natural")
                            //    cincremental3 = 1000 * CVNC + m3 * CostoCombustible;
                            //else
                            //    cincremental3 = 1000 * CVNC + (1 / 1000000) * m3 * CostoCombustible * factorConversion;

                            cincremental3 = 1000 * CVNC + m3 * CostoCombustible * factor;

                        }
                        break;
                }
            }

            ReporteCostoIncrementalDTO entity = new ReporteCostoIncrementalDTO
            {
                CEC = CEC.ToString(),
                Pe = PE.ToString(),
                Rendimiento = Rendimiento.ToString(),
                Precio = CostoCombustible.ToString(),
                CVNC = CVNC.ToString(),
                CVC = CVC.ToString(),
                CV = (CVNC + CVC).ToString(),
                GrupoModoOperacion = item.Gruponomb,
                Empresa = item.Emprnomb,
                TipoCombustible = tipoCombustible,
                Cincrem1 = cincremental1,
                Cincrem2 = cincremental2,
                Cincrem3 = cincremental3,
                Tramo1 = tramo1,
                Tramo2 = tramo2,
                Tramo3 = tramo3,
                Pe1 = p1,
                Pe2 = p2,
                Pe3 = p3,
                Pe4 = p4,
                Grupocodi = item.Grupocodi,
                Grupopadre = item.Grupopadre,
                TipoGenerRer = item.TipoGenerRer,
                Grupotipocogen = item.Grupotipocogen,
                CCombXArr = CCombXArr,
                CCombXPar = CCombXPar,
                TCambio = TCambio
            };

            return entity;
        }


        public List<ReporteCostoIncrementalDTO> GetPropiedadCostoIncremental(List<PrGrupoDTO> listGrupo, List<PrGrupodatDTO> listaFormulasGenerales, List<PrGrupodatDTO> listaFormulasModoOperacion)
        {
            List<ReporteCostoIncrementalDTO> entitys = new List<ReporteCostoIncrementalDTO>();
            //var listaFormulasGenerales = FactorySic.GetPrGrupodatRepository().ObtenerParametroGeneral(fecha);

            //var listaFormulasModoOperacion = FactorySic.GetPrGrupodatRepository().ObtenerParametroModoOperacion(string.Join(",", lstGruposAll.Distinct()), fecha);

            foreach (var grupo in listGrupo)
            {
                var lstGrupos = new List<int> { grupo.Grupocodi, grupo.Grupopadre ?? 0, grupo.GrupoCentral };
                List<PrGrupodatDTO> listFormulas = new List<PrGrupodatDTO>();
                listFormulas.AddRange(listaFormulasGenerales);
                listFormulas.AddRange(listaFormulasModoOperacion.Where(x => lstGrupos.Contains(x.Grupocodi)));


                // Declaramos el objeto parameter para calcular los valores
                n_parameter parameter = new n_parameter();
                foreach (PrGrupodatDTO itemConcepto in listFormulas)
                {
                    if (!string.IsNullOrEmpty(itemConcepto.Concepabrev) && !string.IsNullOrEmpty(itemConcepto.Formuladat))
                        parameter.SetData(itemConcepto.Concepabrev, itemConcepto.Formuladat);
                }

                // Con esto sacamos las propiedades para el modo de operación como son curva de consumo, costo combustible, CV OYM

                double CostoCombustible = parameter.GetEvaluate(ConstantesDespacho.CostoCombustible);
                double CVNC = parameter.GetEvaluate(ConstantesDespacho.CVNC);
                double CVC = parameter.GetEvaluate(ConstantesDespacho.CVC);
                string tipoCombustible = listFormulas.FirstOrDefault(x => x.Concepabrev == ConstantesDespacho.TipComb)?.Formuladat; //- Tipo de combustible
                double factorConversion = parameter.GetEvaluate(ConstantesCortoPlazo.PropFactorConversion); //PCI_SI       


                //- Factor de conversion
                string formulacostoCombustible = listFormulas.FirstOrDefault(x => x.Concepabrev == ConstantesCortoPlazo.PropCostoCombustible)?.Formuladat;
                double factor = 1;
                if (!string.IsNullOrEmpty(formulacostoCombustible))
                    if (formulacostoCombustible.ToUpper() == ConstantesCortoPlazo.UnidadCostoCombustible.ToUpper())
                        factor = factorConversion / 1000000;

                //- Obteniendo los datos de la curva
                string formulaCurvaAjustada = listFormulas.FirstOrDefault(x => x.Concepabrev == ConstantesCortoPlazo.PropCurvaAjustadaSPR)?.Formuladat; //- CoordConsumComb
                List<CoordenadaConsumo> curva = ObtenerCurvaAjustadaSRP(formulaCurvaAjustada);

                //cacular CIs y tramos
                var cIncreTramo = ObtenerCIncrementalYTramo(CostoCombustible, CVNC, factor, curva);

                ReporteCostoIncrementalDTO entity = new ReporteCostoIncrementalDTO
                {
                    Precio = CostoCombustible.ToString(),
                    CVNC = CVNC.ToString(),
                    CVC = CVC.ToString(),
                    CV = (CVNC + CVC).ToString(),
                    GrupoModoOperacion = grupo.Gruponomb,
                    Empresa = grupo.Emprnomb,
                    TipoCombustible = tipoCombustible,
                    Cincrem1 = cIncreTramo.Cincrem1,
                    Cincrem2 = cIncreTramo.Cincrem2,
                    Cincrem3 = cIncreTramo.Cincrem3,
                    Tramo1 = cIncreTramo.Tramo1,
                    Tramo2 = cIncreTramo.Tramo2,
                    Tramo3 = cIncreTramo.Tramo3,
                    Grupocodi = grupo.Grupocodi,
                    Grupopadre = grupo.Grupopadre,
                    TipoGenerRer = grupo.TipoGenerRer,
                    Grupotipocogen = grupo.Grupotipocogen,
                };

                entitys.Add(entity);

            }

            return entitys;
        }


        /// <summary>
        /// Retorna objeto ReporteCostoIncrementalDTO con costo incremental y tramo
        /// </summary>
        /// <param name="CostoCombustible">Costo combustible</param>
        /// <param name="CVNC"><CVNC/param>
        /// <param name="factor">Factor</param>
        /// <param name="curva">Lista coordenada consumo</param>
        /// <returns></returns>
        private ReporteCostoIncrementalDTO ObtenerCIncrementalYTramo(double CostoCombustible, double CVNC, double factor, List<CoordenadaConsumo> curva)
        {
            ReporteCostoIncrementalDTO costoIncremental = new ReporteCostoIncrementalDTO();
            for (int item = 1; item < curva.Count(); item++)
            {
                var coordenadaAnt = curva[item - 1];
                var coordenadaAct = curva[item];

                var pendiente = (double)((coordenadaAct.Consumo - coordenadaAnt.Consumo) / (coordenadaAct.Potencia - coordenadaAnt.Potencia));
                var valCostoInc = 1000 * CVNC + pendiente * CostoCombustible * factor;
                var tramo = string.Format("[{0}-{1}]", coordenadaAnt.Potencia, coordenadaAct.Potencia);
                switch (item)
                {
                    case 1:
                        costoIncremental.Cincrem1 = valCostoInc;
                        costoIncremental.Tramo1 = tramo;
                        break;
                    case 2:
                        costoIncremental.Cincrem2 = valCostoInc;
                        costoIncremental.Tramo2 = tramo;
                        break;
                    case 3:
                        costoIncremental.Cincrem3 = valCostoInc;
                        costoIncremental.Tramo3 = tramo;
                        break;
                    default:
                        break;
                }
            }
            return costoIncremental;
        }

        /// <summary>
        /// Retorna listado de coordenada consumo
        /// </summary>
        /// <param name="curvaAjustada">Formula curva ajustada</param>
        /// <returns></returns>
        private List<CoordenadaConsumo> ObtenerCurvaAjustadaSRP(string curvaAjustada)
        {
            List<CoordenadaConsumo> curva = new List<CoordenadaConsumo>();
            if (!string.IsNullOrEmpty(curvaAjustada))
            {
                string[] strPuntos = curvaAjustada.Split(ConstantesAppServicio.CaracterNumeral);

                foreach (string strPunto in strPuntos)
                {
                    string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                    if (strCorrdenada.Length == 2)
                    {
                        decimal x = 0;
                        decimal y = 0;

                        if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                        {
                            curva.Add(new CoordenadaConsumo
                            {
                                Potencia = x,
                                Consumo = y
                            });
                        }
                    }
                }
            }

            return curva;
        }

        /// <summary>
        /// Permite obtener los valores para las curvas por grupocodi
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<CurvaConsumo> ObtenerPametrosCurvaPorFecha(DateTime fecha)
        {
            List<CurvaConsumo> result = new List<CurvaConsumo>();

            List<PrGrupodatDTO> list = FactorySic.GetPrGrupodatRepository().ObtenerParametrosCurvasConsumoCombustibleporFecha(
                fecha);


            var grupos = list.Select(x => new { x.Grupocodi, x.GrupoNomb, x.GrupocodiNCP }).Distinct().ToList();


            int[] categorias = { 243, 14, 175, 176, 177, 178, 179, 180, 181, 182, 183, 708, 709 };

            foreach (var grupo in grupos)
            {
                CurvaConsumo item = new CurvaConsumo();
                item.Grupocodi = grupo.Grupocodi;
                item.Gruponomb = grupo.GrupoNomb;
                item.GrupocodiNCP = grupo.GrupocodiNCP;

                #region Obtencion de puntos

                SerieCurva serieGrupo = new SerieCurva();

                List<PrGrupodatDTO> subList = list.Where(x => x.Grupocodi == grupo.Grupocodi).ToList();
                PrGrupodatDTO categoriaCurva = subList.Where(x => x.Concepcodi == categorias[0]).FirstOrDefault();
                PrGrupodatDTO tipoCurva = subList.Where(x => x.Concepcodi == this.Concepcodi).FirstOrDefault();
                item.Indcurva = tipoCurva.Formuladat;

                // Leemos los puntos de la curva de consumo
                List<PuntoCurva> puntosConsumo = new List<PuntoCurva>();

                if (categoriaCurva != null)
                {
                    string[] strPuntos = categoriaCurva.Formuladat.Split(ConstantesAppServicio.CaracterNumeral);

                    foreach (string strPunto in strPuntos)
                    {
                        string[] strCorrdenada = strPunto.Split(ConstantesAppServicio.CaracterPorcentaje);

                        if (strCorrdenada.Length == 2)
                        {
                            decimal x = 0;
                            decimal y = 0;

                            if (decimal.TryParse(strCorrdenada[0], out x) && decimal.TryParse(strCorrdenada[1], out y))
                            {
                                puntosConsumo.Add(new PuntoCurva
                                {
                                    PuntoX = x,
                                    PuntoY = y
                                });
                            }
                        }
                    }
                }

                serieGrupo.SerieConsumo = puntosConsumo;

                // Leemos los puntos de los ensayos de potencia
                List<PuntoCurva> puntosEnsayo = new List<PuntoCurva>();

                for (int i = 1; i < categorias.Length - 1; i += 2)
                {
                    PrGrupodatDTO puntox = subList.Where(x => x.Concepcodi == categorias[i]).FirstOrDefault();
                    PrGrupodatDTO puntoy = subList.Where(x => x.Concepcodi == categorias[i + 1]).FirstOrDefault();

                    if (puntox != null && puntoy != null)
                    {
                        decimal x = 0;
                        decimal y = 0;
                        if (decimal.TryParse(puntox.Formuladat, out x) && decimal.TryParse(puntoy.Formuladat, out y))
                        {
                            puntosEnsayo.Add(new PuntoCurva
                            {
                                PuntoX = x,
                                PuntoY = y
                            });
                        }
                    }
                }

                serieGrupo.SerieEnsayo = puntosEnsayo;

                if (puntosEnsayo.Count > 0 || puntosConsumo.Count > 0)
                {
                    item.ListaSerie = serieGrupo;
                    result.Add(item);
                }

                #endregion

            }
            return result;
        }


        public string SaveGrupodat(PrGrupodatDTO entity)
        {
            try
            {
                (new DespachoAppServicio()).ActualizarGrupodat(false, entity);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public int ObtenerCodigoNCP(int grupoCodi)
        {
            return FactorySic.GetPrGrupoRepository().ObtenerNCP(grupoCodi);
        }

        public void ActualizarCodigoNCP(int grupoCodiNCP, int grupoCodi)
        {
            FactorySic.GetPrGrupoRepository().UpdateNCP(grupoCodiNCP, grupoCodi);
        }

        public PrGrupodatDTO BuscaIDCurvaPrincipal(int curvcodi)
        {
            return FactorySic.GetPrGrupodatRepository().BuscaIDCurvaPrincipal(curvcodi);

        }

        /// <summary>
        /// Permite obtener la fecha en la que se puede editar una curva de consumo
        /// </summary>
        /// <param name="grupoCodi"></param>
        /// <returns></returns>
        public string ObtenerFechaEdicion(int grupoCodi)
        {
            return FactorySic.GetPrGrupodatRepository().ObtenerFechaEdicionCurva(grupoCodi);
        }

        #endregion

    }
}


/// <summary>
/// Maneja las series por cada grupo
/// </summary>
public class CurvaConsumo
{
    public int Grupocodi { get; set; }
    public string Gruponomb { get; set; }
    public SerieCurva ListaSerie { get; set; }
    public string Indcurva { get; set; }
    public string Lastuser { get; set; }
    public string Fechadat { get; set; }
    public string Fechaact { get; set; }
    public int GrupocodiNCP { get; set; }
    public int Curvcodi { get; set; }
}



/// <summary>
/// Serie por cada modo de operación
/// </summary>
public class SerieCurva
{
    public List<PuntoCurva> SerieConsumo { get; set; }
    public List<PuntoCurva> SerieEnsayo { get; set; }
}

/// <summary>
/// Pares ordenados de las series
/// </summary>
public class PuntoCurva
{
    public decimal PuntoX { get; set; }
    public decimal PuntoY { get; set; }
}
