using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Compensacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Servicios.Aplicacion.Equipamiento;

namespace COES.Servicios.Aplicacion.Compensacion
{
    public class CompensacionAppServicio : AppServicioBase
    {

        #region Metodos de la Tabla VCE_PERIODO_CALCULO

        /// <summary>
        /// Permite listar todos los registros de la tabla VCE_PERIODO_CALCULO
        /// </summary>
        public List<VcePeriodoCalculoDTO> ListVersiones()
        {
            return FactorySic.GetVcePeriodoCalculoRepository().List();
        }

        /// <summary>
        /// Pemite obtener los datos de una version del Periodo por el Id (Pecacodi)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VcePeriodoCalculoDTO getVersionPeriodoById(int id)
        {
            return FactorySic.GetVcePeriodoCalculoRepository().GetById(id);
        }

        /// <summary>
        /// Permite obtener la versiones del Periodo por un Año-Mes (PeriAnioMes)
        /// </summary>
        /// <returns></returns>
        public List<VcePeriodoCalculoDTO> getVersionesPeridoByAnioMes(int iAnioMes)
        {
            return FactorySic.GetVcePeriodoCalculoRepository().GetByAnioMes(iAnioMes);
        }

        /// <summary>
        /// Permite obtener la versiones del Periodo por el Id Periodo (PeriCodi)
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        public List<VcePeriodoCalculoDTO> getVersionesPeridoByIdPeriodo(int iPericodi)
        {
            return FactorySic.GetVcePeriodoCalculoRepository().GetByIdPeriodo(iPericodi);
        }

        /// <summary>
        /// Actualizar los datos de una version del Periodo 
        /// </summary>
        /// <param name="vcePeriodoCalculoDTO"></param>
        public void UpdateVcePeriodoCalculo(VcePeriodoCalculoDTO vcePeriodoCalculoDTO)
        {
            try
            {
                FactorySic.GetVcePeriodoCalculoRepository().Update(vcePeriodoCalculoDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void SaveVcePeriodoCalculo(VcePeriodoCalculoDTO entity)
        {
            try
            {
                int idGen = FactorySic.GetVcePeriodoCalculoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void DeleteVcePeriodoCalculo(int peracodi)
        {
            try
            {
                FactorySic.GetVcePeriodoCalculoRepository().Delete(peracodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void DeleteVceLogCargaDet(int pecacodi)
        {
            try
            {
                FactorySic.GetVceLogCargaDetRepository().DeleteDetPeriodo(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void DeleteVceLogCargaCab(int pecacodi)
        {
            try
            {
                FactorySic.GetVceLogCargaCabRepository().DeleteCabPeriodo(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public Int32 ObtenerNroCalculosActivosPeriodo(int pericodi)
        {
            try
            {
                int cont = FactorySic.GetVcePeriodoCalculoRepository().ObtenerNroCalculosActivosPeriodo(pericodi);
                return cont;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public Int32 GetIdAnteriorCalculo(int pecacodi)
        {
            try
            {
                int cont = FactorySic.GetVcePeriodoCalculoRepository().GetIdAnteriorCalculo(pecacodi);
                return cont;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public Int32 GetIdAnteriorConfig(int pecacodi)
        {
            try
            {
                int cont = FactorySic.GetVcePeriodoCalculoRepository().GetIdAnteriorConfig(pecacodi);
                return cont;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        #endregion


        #region Métodos Tabla TRN_PERIODO

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_PERIODO
        /// </summary>
        public List<PeriodoDTO> List()
        {
            return FactorySic.GetTrnPeriodoRepository().List();
        }

        /// <summary>
        /// Obtener el objeto Periodo por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PeriodoDTO getPeriodoById(int id)
        {
            return FactorySic.GetTrnPeriodoRepository().GetById(id);
        }

        //- compensaciones.HDT - 05/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener el periodo con datos calculados para el procesamiento de la compensación
        /// especial Ilo2.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PeriodoDTO getPeriodoByIdProcesa(int id)
        {
            return FactorySic.GetTrnPeriodoRepository().GetPeriodoByIdProcesa(id);
        }

        //- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
        public List<VceArrparIncredGenDTO> getListVceArrparIncred(int idPeriodo)
        {
            return FactorySic.GetVceArrparIncredGenRepository().GetByPeriod(idPeriodo);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_PERIODO y su descripcion
        /// </summary>
        public List<PeriodoDTO> ListarPeriodosTC()
        {
            return FactorySic.GetTrnPeriodoRepository().ListarPeriodosTC();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_PERIODO y su correspondiente último periodo de cálculo
        /// </summary>
        public List<PeriodoDTO> ListarPeriodosCompensacion()
        {
            return FactorySic.GetTrnPeriodoRepository().ListarPeriodosCompensacion();
        }

        public void UpdateTrnPeriodo(PeriodoDTO trnPeriodoDTO)
        {
            try
            {
                FactorySic.GetTrnPeriodoRepository().Update(trnPeriodoDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos Tabla VCE_ENERGIA

        public void SaveEnergia(int pecacodi, string fechaini, string fechafin)
        {
            try
            {
                FactorySic.GetVceEnergiaRepository().SaveFromMeMedicion96(pecacodi, fechaini, fechafin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteEnergia(string fechaini, string fechafin)
        {
            try
            {
                FactorySic.GetVceEnergiaRepository().DeletexFecha(fechaini, fechafin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<VceEnergiaDTO> ListarEnergia(int registros, int pecacodi)
        {
            return FactorySic.GetVceEnergiaRepository().ListByCriteria(registros, pecacodi);
        }

        #endregion


        #region Métodos Tabla VCE_HORA_OPERACION

        public void SaveHoraOperacion(int pecacodi, string fechaini, string fechafin)
        {
            try
            {
                FactorySic.GetVceHoraOperacionRepository().SaveByRango(pecacodi, fechaini, fechafin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public VceHoraOperacionDTO GetByIdHoraOperacion(int pecacodi, int hopcodi)
        {
            return FactorySic.GetVceHoraOperacionRepository().GetById(hopcodi, pecacodi);

        }

        public VceHoraOperacionDTO GetDataByIdHoraOperacion(int pecacodi, int hopcodi)
        {
            return FactorySic.GetVceHoraOperacionRepository().GetDataById(hopcodi, pecacodi);

        }

        public void DeleteHoraOperacion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceHoraOperacionRepository().DeleteById(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteRangoHoraOperacion(int pecacodi, int hopcodi)
        {
            try
            {
                FactorySic.GetVceHoraOperacionRepository().Delete(hopcodi, pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateRangoHoraOperacion(VceHoraOperacionDTO oVceHoraOperacionDTO)
        {
            try
            {
                FactorySic.GetVceHoraOperacionRepository().UpdateRangoHora(oVceHoraOperacionDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<VceHoraOperacionDTO> ListarHoraOperacionFiltro(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string arranque, string parada)
        {
            return FactorySic.GetVceHoraOperacionRepository().ListFiltro(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, arranque, parada);
        }

        public List<VceHoraOperacionDTO> ListarHoraOperacion(int pecacodi)
        {
            return FactorySic.GetVceHoraOperacionRepository().ListById(pecacodi);
        }

        // DSH 08-10-2017 : Se agrego por requerimiento
        public List<VceHoraOperacionDTO> ListarVerificarHoras(int pecacodi)
        {
            return FactorySic.GetVceHoraOperacionRepository().ListVerificarHoras(pecacodi);
        }

        // DSH 06-05-2017,06-05-2017 : Se agrego por requerimiento
        public void SaveVceHoraOperacionFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            try
            {
                FactorySic.GetVceHoraOperacionRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);

                int id = GetMinIdVceLogCargaCab(pecacodiDestino, "VCE_HORA_OPERACION");

                if (id > 0)
                {
                    SaveVceLogCargaDet(id, usuCreacion, "VCE_HORA_OPERACION", pecacodiDestino);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #region Métodos Tabla VCE_COSTO_MARGINAL
        public List<string> lstBodyCostoMarginal(int pecacodi, int cosmarversion)
        {
            return FactorySic.GetVceCostoMarginalRepository().LstBodyCostoMarginal(pecacodi, cosmarversion);
        }

        public List<string> lstHeadCostoMarginal(int pecacodi)
        {
            return FactorySic.GetVceCostoMarginalRepository().LstHeadCostoMarginal(pecacodi);
        }
        #endregion

        #region Métodos Tabla VCE_TEXTO_REPORTE

        public void SaveTextoReporte(VceTextoReporteDTO oVceTextoReporteDTO)
        {
            try
            {
                FactorySic.GetVceTextoReporteRepository().Save(oVceTextoReporteDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateTextoReporte(VceTextoReporteDTO oVceTextoReporteDTO)
        {
            try
            {
                FactorySic.GetVceTextoReporteRepository().Update(oVceTextoReporteDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteTextoReporte(VceTextoReporteDTO oVceTextoReporteDTO)
        {
            try
            {
                FactorySic.GetVceTextoReporteRepository().Delete(oVceTextoReporteDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public VceTextoReporteDTO getTextoReporteById(int PecaCodi, string Txtrepcodreporte, string Txtrepcodtexto)
        {
            return FactorySic.GetVceTextoReporteRepository().GetById(PecaCodi, Txtrepcodreporte, Txtrepcodtexto);
        }

        public List<VceTextoReporteDTO> ListTextoReporte(int pecacodi)
        {
            return FactorySic.GetVceTextoReporteRepository().ListByPeriodo(pecacodi);
        }

        // DSH 27-06-2017 : Se agrego por requerimiento
        public void DeleteVceTextoReporteByVersion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceTextoReporteRepository().DeleteByVersion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void SaveVceTextoReporteFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            try
            {
                FactorySic.GetVceTextoReporteRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen, usuCreacion);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }


        }

        #endregion


        /// <summary>
        /// Permite obtener el resultado de la formula de un conjunto de grupos
        /// </summary>
        /// <param name="idsGrupos">Lista de códigos de los grupos</param>
        /// <param name="concepto">Concepto a evaluar</param>
        /// <param name="fechaDatos">Fecha de los datos de los conceptos</param>
        /// <param name="tipoCambioCompensacion">Tipo de cambio que ser{a utilizado en reemplazo del que exista en la BD</param>
        /// <returns></returns>
        public List<PrGrupodatDTO> ObtenerResultadoFormula(List<int> idsGrupos, int concepto, DateTime fechaDatos, String tipoCambio)
        {
            List<PrGrupodatDTO> result = new List<PrGrupodatDTO>();

            // Listamos los parámetros generales
            List<PrGrupodatDTO> conceptosBase = FactorySic.GetPrGrupodatRepository().ParametrosGeneralesPorFecha(fechaDatos);

            List<PrGrupodatDTO> conceptoTC = conceptosBase.Where(x => x.Concepcodi == 1).Distinct().ToList();
            if (conceptoTC.Count > 0)
                conceptoTC[0].Formuladat = tipoCambio;


            // Parámetros de los grupos
            List<PrGrupodatDTO> conceptos = FactorySic.GetPrGrupodatRepository().ObtenerParametroPorModoOperacionPorFecha(string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsGrupos), fechaDatos);

            foreach (int id in idsGrupos)
            {
                // Obtener la formula a evaluar
 
                string formula = conceptos.Where(x => x.Concepcodi == concepto && x.Grupocodi == id).Select(x => x.Concepabrev).Distinct().FirstOrDefault();


                if (!string.IsNullOrEmpty(formula))
                {
                    // Instanciamos la clase n_parameter encargada de obtener el valor de las formulas
                    n_parameter parameter = new n_parameter();

                    // Obtenemos los conceptos pertenecientes al grupo
                    List<PrGrupodatDTO> conceptosGrupo = conceptos.Where(x => x.Grupocodi == id).Distinct().ToList();

                    // Llenamos los conceptos base al objeto n_parameter
                    foreach (PrGrupodatDTO itemConcepto in conceptosBase)
                        parameter.SetData(itemConcepto.Concepabrev, "("+itemConcepto.Formuladat+")");

                    // Llenamos los conceptos del grupo al objeto n_parameter
                    foreach (PrGrupodatDTO itemConcepto in conceptosGrupo)
                        parameter.SetData(itemConcepto.Concepabrev, "("+itemConcepto.Formuladat+")");

                    double valor = parameter.GetEvaluate(formula);


                    // Agregamos el resultado a a la lista
                    PrGrupodatDTO entity = new PrGrupodatDTO();
                    entity.Grupocodi = id;
                    entity.Valor = (decimal)valor;
                    entity.Formuladat = formula;
                    result.Add(entity);
                }
            }

            return result;
        }

        public List<int> lstGrupos()
        {
            return FactorySic.GetVceCostoMarginalRepository().LstGrupos();
        }


        #region Métodos Tabla VCE_DAT_CALCULO



        public List<VceDatcalculoDTO> ListarDatCalculo(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            return FactorySic.GetVceDatCalculoRepository().ListByFiltro(pecacodi, empresa, central, grupo, modo);
        }

        public VceDatcalculoDTO VceDatCalculoGetById(int pecacodi, int grupocodi)
        {
            return FactorySic.GetVceDatCalculoRepository().GetByIdGrupo(pecacodi, grupocodi);
        }

        public void UpdateVceDatCalculo(VceDatcalculoDTO entity)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite configurar los parámetros para el cálculo.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="periAnioMes"></param>
        /// <param name="pecaTipoCambio"></param>
        public void ConfigurarParametroCalculo(int pecacodi, ref string periAnioMes, ref string pecaTipoCambio)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().ConfigurarParametroCalculo(pecacodi, ref periAnioMes, ref pecaTipoCambio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public void PoblarRegistroSinCalculos(int pecacodi, string perianiomes)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().PoblarRegistroSinCalculos(pecacodi, perianiomes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<VceDatcalculoDTO> ObtenerRegistroSinCalculos(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ObtenerRegistroSinCalculos(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<VceDatcalculoDTO> ObtenerRegistroSinCalculosPotenciaEfectica(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ObtenerRegistroSinCalculos(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<DateTime> ObtenerDistintasFechasModificacion(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ObtenerDistintasFechasModificacion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public List<DateTime> ObtenerDistintasFechasModificacionPotenciaEfectiva(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ObtenerDistintasFechasModificacionPotenciaEfectiva(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<int> ObtenerDistintosIdGrupo(int pecacodi, int fenergcodi, string cfgdccondsql)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ObtenerDistintosIdGrupo(pecacodi, fenergcodi, cfgdccondsql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<int> ObtenerDistintosIdGrupo(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ObtenerDistintosIdGrupo(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void SaveCalculo(int pecacodi, List<VceCfgDatCalculoDTO> lVceCfgDatCalculoDTO)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().SaveCalculo(pecacodi, lVceCfgDatCalculoDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Actualiza los datos post cálculo realizado.
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="pecatipocambio"></param>
        public void ActualizarDatosPosCalculo(int pecacodi, string pecatipocambio)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().ActualizarDatosPosCalculo(pecacodi, pecatipocambio);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteCalculo(int pecacodi)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().DeleteCalculo(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// Obtener Listado de Horas de Operación
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GenerarFormatoHorasOperacion(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string arranque, string parada, string path)
        {
            string fileName = string.Empty;
            
            List<VceHoraOperacionDTO> ListaHorasOperacion = this.ListarHoraOperacionFiltro(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, arranque, parada);

            fileName = "Reporte Horas de Operación.xlsx";
            ExcelDocument.GenerarFormatoExcelHorasOperacion(path + fileName, pecacodi, ListaHorasOperacion);

            return fileName;
        }

        public string GenerarFormatoVerificarHorasOperacion(int pecacodi, string path)
        {
            string fileName = string.Empty;

            List<VceHoraOperacionDTO> ListaVerificarHorasOperacion = this.ListarVerificarHoras(pecacodi);

            fileName = "Reporte Verificar Horas de Operación.xlsx";
            ExcelDocument.GenerarFormatoExcelVerificarHorasOperacion(path + fileName, pecacodi, ListaVerificarHorasOperacion);

            return fileName;
        }

        public string GenerarFormatoEnergia(int pecacodi, string path)
        {
            string fileName = string.Empty;

            List<VceEnergiaDTO> ListaEnergia = this.ListarEnergia(10000, pecacodi);

            fileName = "Reporte Energia.xlsx";
            ExcelDocument.GenerarFormatoExcelEnergia(path + fileName, ListaEnergia);

            return fileName;
        }

        public string GenerarFormatoCostosVariables(int pecacodi, string path)
        {
            string fileName = string.Empty;

            List<VceDatcalculoDTO> ListarCalculo = this.ListarDatCalculo(pecacodi, "", "", "", "");

            fileName = "Reporte Costos Variables.xlsx";
            ExcelDocument.GenerarFormatoExcelCostosVariables(path + fileName, pecacodi, ListarCalculo);

            return fileName;
        }

        public string GenerarFormatoCostosVariablesFiltro(int pecacodi, string empresa, string central, string grupo, string modo, string path)
        {
            string fileName = string.Empty;

            List<VceDatcalculoDTO> ListarCalculo = this.ListarDatCalculo(pecacodi, empresa, central, grupo, modo);

            fileName = "Reporte Costos Variables.xlsx";
            ExcelDocument.GenerarFormatoExcelCostosVariables(path + fileName, pecacodi, ListarCalculo);

            return fileName;
        }

        // Cambios DSH 30-03-2017 - Generar formato de Compensaciones Regulares
        public string GenerarFormatoCompensacionesRegulares(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string tipocalculo, string path)
        {
            string fileName = string.Empty;

            List<VceCompBajaeficDTO> ListaCompensacionesRegulares = this.ListCompensacionesRegulares(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, tipocalculo);

            fileName = "Reporte Compensaciones Regulares.xlsx";
            ExcelDocument.GenerarFormatoExcelCompensacionesRegulares(path + fileName, pecacodi, ListaCompensacionesRegulares);

            return fileName;
        }

        // Cambios DSH 30-03-2017 - Generar formato de Compensaciones Especiales
        public string GenerarFormatoCompensacionesEspeciales(int pecacodi, string empresa, string central, string grupo, string modo, string path)
        {
            string fileName = string.Empty;

            List<VceCompRegularDetDTO> ListaCompensacionesEspeciales = this.ListCompensacionesEspeciales(pecacodi, empresa, central, grupo, modo);

            fileName = "Reporte Compensaciones Especiales.xlsx";
            ExcelDocument.GenerarFormatoExcelCompensacionesEspeciales(path + fileName, pecacodi, ListaCompensacionesEspeciales);

            return fileName;
        }

        // DSH 20-04-2017 - Generar formato de Arranques y Paradas
        public string GenerarFormatoArranquesParadas(int pecacodi, string empresa, string central, string grupo, string modo, string path)
        {
            string fileName = string.Empty;

            IDataReader dr = this.ListCompensacionArrPar(pecacodi, empresa, central, grupo, modo);
            IDataReader drCab = this.ListCabCompensacionArrPar(pecacodi);

            fileName = "Reporte Detalle Arranques y Paradas.xlsx";
            ExcelDocument.GenerarFormatoExcelDetalleArranquesParadas(path + fileName, pecacodi, dr, drCab);

            return fileName;
        }

        // DSH 20-06-2017 - Generar formato de Pto. Grupo
        public string GenerarFormatoPtoGrupo(int pecacodi, string path)
        {
            string fileName = string.Empty;

            List<string> ListGrillaHead = this.lstGrillaHead(pecacodi);
            List<ComboCompensaciones> ListGrillaBody = this.lstGrillaBody(pecacodi);

            fileName = "Reporte Asociación de Puntos de Medición.xlsx";

            ExcelDocument.GenerarFormatoExcelPtoGrupo(path + fileName, pecacodi, ListGrillaHead, ListGrillaBody);

            return fileName;
        }

        // DSH 26-04-2017 : se agrego por requerimiento
        public string DescargarFormatoCompensacionManual(string path)
        {
            string fileName = string.Empty;

            fileName = "Formato de Carga Compensaciones Manuales.xlsx";
            ExcelDocument.DescargarFormatoCargaCompensacionManual(path + fileName);

            return fileName;
        }

        // DSH 12-05-2017 : se agrego por requerimiento
        public string DescargarFormatoIncrementoReduccion(string path)
        {
            string fileName = string.Empty;

            fileName = "Formato de Carga Incrementos y Reducciones.xlsx";
            ExcelDocument.DescargarFormatoCargaIncrementoReduccion(path + fileName);

            return fileName;
        }

       
        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_BARRA
        /// </summary>
        public List<BarraDTO> ListBarra()
        {
            return FactorySic.GetTrnBarraRepository().ListarBarras();
        }

        /// <summary>
        /// Editor
        /// </summary>
        /// <returns></returns>
        public List<string> lstGrillaHead(int pecacodi)
        {
            return FactorySic.GetMePtomedicionRepository().LstGrillaHead(pecacodi);

        }

        public List<ComboCompensaciones> lstGrillaBody(int pecacodi)
        {
            return FactorySic.GetMePtomedicionRepository().LstGrillaBody(pecacodi);
        }

        public List<ComboCompensaciones> ListCostoMarginalVersion(int pericodi)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().ListCostoMarginalVersion(pericodi);
        }

        public List<VceLogCargaDetDTO> ListEntidades(int pecacodi)
        {
            return FactorySic.GetVceLogCargaDetRepository().ListDetalle(pecacodi);
        }

        public List<MePtomedicionDTO> ListPtoMedicionCompensaciones(int ptoMediCodi, int pecacodi)
        {
            return FactorySic.GetMePtomedicionRepository().ListPtoMedicionCompensaciones(ptoMediCodi, pecacodi);
        }

        public List<VcePtomedModopeDTO> ListVcePtomedModope(int pecacodi, int ptoMediCodi)
        {
            return FactorySic.GetVcePtomedModopeRepository().ListById(pecacodi, ptoMediCodi);
        }

        public List<PrGrupodatDTO> ListaModosOperacion(int ptoMediCodi, int pecacodi)
        {
            return FactorySic.GetPrGrupodatRepository().ListaModosOperacion(ptoMediCodi, pecacodi);
        }

        public void SaveVcePtomedModope(VcePtomedModopeDTO entity)
        {
            try
            {
                FactorySic.GetVcePtomedModopeRepository().SaveByEntity(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteVcePtomedModope(int pecacodi, int ptomedicodi, int grupocodi)
        {
            try
            {
                FactorySic.GetVcePtomedModopeRepository().DeleteByEntity(pecacodi, ptomedicodi, grupocodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 05-05-2017 : Se agrego por requerimiento
        public void DeleteVcePtomedModopeByVersion(int pecacodi)
        {
            try
            {
                FactorySic.GetVcePtomedModopeRepository().DeleteByVersion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 06-05-2017 : Se agrego por requerimiento
        public void SaveVcePtomedModopeFromOtherversion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            FactorySic.GetVcePtomedModopeRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen, usuCreacion);
        }

        public List<EveSubcausaeventoDTO> ListTipoOperacion(int pecacodi)
        {
            return FactorySic.GetEveSubcausaeventoRepository().ListTipoOperacion(pecacodi);
        }

        public List<SiEmpresaDTO> ListaEmpresasCompensacion()
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasCompensacion();
        }

        public List<PrGrupodatDTO> ListaCentral(int emprcodi)
        {
            return FactorySic.GetPrGrupodatRepository().ListaCentral(emprcodi);
        }

        // DSH 19-06-2017 Inicio de Actualizacion
        public List<PrGrupodatDTO> ListaGrupo(int emprcodi, int grupopadre)
        {
            return FactorySic.GetPrGrupodatRepository().ListaGrupo(emprcodi, grupopadre);
        }
        public List<PrGrupodatDTO> ListaModo(int emprcodi, int grupopadre)
        {
            return FactorySic.GetPrGrupodatRepository().ListaModo(emprcodi, grupopadre);
        }

        public List<EveSubcausaeventoDTO> ListSubCausaEvento(int pecacodi)
        {
            return FactorySic.GetEveSubcausaeventoRepository().ListSubCausaEvento(pecacodi);
        }

        // Fin de Actualizacion

        /// <summary>
        /// Lista de Compensaciones Regulares
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="grupo"></param>
        /// <param name="modo"></param>
        /// <param name="tipo"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <returns></returns>
        public List<VceCompBajaeficDTO> ListCompensacionesRegulares(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fechaini, string fechafin, string tipocalculo)
        {
            return FactorySic.GetVceCompBajaeficRepository().ListCompensacionesRegulares(pecacodi, empresa, central, grupo, modo, tipo, fechaini, fechafin, tipocalculo);
        }

        /// <summary>
        /// Lista de Compensaciones Especiales
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="grupo"></param>
        /// <param name="modo"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<VceCompRegularDetDTO> ListCompensacionesEspeciales(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            return FactorySic.GetVceCompRegularDetRepository().ListCompensacionesEspeciales(pecacodi, empresa, central, grupo, modo);
        }


        public string GenerarFormatoDatosCalculo(int pecacodi, string path, string grupo, string lista)
        {
            string fileName = string.Empty;

            if (grupo.Equals("grupo1"))
            {
                fileName = "Reporte Compensaciones por Costo Variable.xlsx";
            }

            if (grupo.Equals("grupo2"))
            {
                fileName = "Reporte Compensaciones por Arranques y Paradas.xlsx";
            }

            ExcelDocument.GenerarFormatoExcelDatosCalculo(path + fileName, pecacodi, grupo, lista);

            return fileName;
        }

        public List<VceCostoVariableDTO> ListaCostoVariable(int pecacodi)
        {
            return FactorySic.GetVceCostoVariableRepository().ListCostoVariable(pecacodi);
        }

        public void ProcesarCompensacionEspecial(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().ProcesarCompensacionEspecial(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #region Compensaciones MME
        public void ProcesarCompensacionMME(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().ProcesarCompensacionMME(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Lista de Compensaciones Especiales
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="grupo"></param>
        /// <param name="modo"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<VceCompRegularDetDTO> ListCompensacionesMME(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            return FactorySic.GetVceCompRegularDetRepository().ListCompensacionesMME(pecacodi, empresa, central, grupo, modo);
        }

        public string GenerarFormatoCompensacionesMME(int pecacodi, string empresa, string central, string grupo, string modo, string path)
        {
            string fileName = string.Empty;

            List<VceCompRegularDetDTO> ListaCompensacionesMME = this.ListCompensacionesMME(pecacodi, empresa, central, grupo, modo);

            fileName = "Reporte Compensaciones MME.xlsx";
            ExcelDocument.GenerarFormatoExcelCompensacionesEspeciales(path + fileName, pecacodi, ListaCompensacionesMME);

            return fileName;
        }
        #endregion


        public void ProcesarCompensacionRegular(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().ProcesarCompensacionRegular(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int ValidarVcePtomedModope(int pecacodi, int ptomedicodi, int grupocodi)
        {
            return FactorySic.GetVcePtomedModopeRepository().Validar(pecacodi, ptomedicodi, grupocodi);
        }

        public void InitMesValorizacion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceLogCargaCabRepository().Init(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteCompensacionCab(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().DeleteCompensacionManual(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public void DeleteCompensacionDet(int pecacodi, int grupocodi, DateTime crdethora)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().DeleteCompensacionManual(pecacodi, grupocodi, crdethora);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public string LeerVceCompBajaefic(string path, int pecacodi)
        {
            return ExcelDocument.CargaManual(path, pecacodi);
        }

        public string LeerVceArrparIncredGen(string path, int pericodi, int pecacodi, string usuario)
        {
            return ExcelDocument.CargaManualIncred(path, pericodi, pecacodi, usuario);
        }

        public int GetSubCasusaCodi(string desc)
        {
            return FactorySic.GetEveSubcausaeventoRepository().GetSubCasusaCodi(desc);
        }

        public int GetGrupoCodi(string desc)
        {
            return FactorySic.GetPrGrupodatRepository().GetGrupoCodi(desc);
        }

        public void SaveVceCompBajaefic(VceCompBajaeficDTO entity)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().SaveByEntity(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void SaveVceCompBajaeficFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public void SaveVceCompRegularDet(VceCompRegularDetDTO entity)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().SaveEntity(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void SaveVceCompRegularDetFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuaCreacion)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 24-04-2017 : Se agrego por requerimiento
        public VceCompBajaeficDTO getVceCompBajaeficById(DateTime crcbehorfin, DateTime crcbehorini, int subcausacodi, int grupocodi, int pecacodi)
        {
            return FactorySic.GetVceCompBajaeficRepository().GetById(crcbehorfin, crcbehorini, subcausacodi, grupocodi, pecacodi);
        }

        // DSH 24-04-2017 : Se agrego por requerimiento
        public void DeleteCompBajaefic(DateTime crcbehorfin, DateTime crcbehorini, int subcausacodi, int grupocodi, int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().Delete(crcbehorfin, crcbehorini, subcausacodi, grupocodi, pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 05-05-2017 : Se agrego por requerimiento 
        public void DeleteCompBajaeficByVersion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().DeleteByVersion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteCompBajaeficByVersionCalculoAutomatico(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompBajaeficRepository().DeleteByVersionTipoCalculoAutomatico(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 25-04-2017,06-05-2017 : Se agrego por requerimiento
        public void DeleteCompRegularDet(int pecacodi, int grupocodi, int subcausacodi, DateTime crcbehorini, DateTime crcbehorfin)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().DeleteByGroup(pecacodi, grupocodi, subcausacodi, crcbehorini, crcbehorfin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteCompRegularDetByVersion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().DeleteByVersion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteCompRegularDetByVersionCalculoAutomatico(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().DeleteByVersionTipoCalculoAutomatico(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        // DSH 30-04-2017 : Se agrego por requeriminto
        public List<VceCompBajaeficDTO> ListCompensacionOperacionInflexibilidad(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceCompBajaeficRepository().ListCompensacionOperacionInflexibilidad(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<VceCompBajaeficDTO> ListCompensacionOperacionSeguridad(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceCompBajaeficRepository().ListCompensacionOperacionSeguridad(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public List<VceCompBajaeficDTO> ListCompensacionOperacionRSF(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceCompBajaeficRepository().ListCompensacionOperacionRSF(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public List<VceCompBajaeficDTO> ListCompensacionRegulacionTension(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceCompBajaeficRepository().ListCompensacionRegulacionTension(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 04-05-2017 : Se agrego por requerimiento
        public List<VceArrparGrupoCabDTO> ListByGroupCompArrpar(int pecacodi, string Apgcfccodi)
        {
            try
            {
                return FactorySic.GetVceArrparGrupoCaRepository().ListByGroupCompArrpar(pecacodi, Apgcfccodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        // DSH 0-05-2017 : Se agrego por requerimiento
        public int GetMinIdVceLogCargaCab(int pecacodi, string nombTabla)
        {
            try
            {
                return FactorySic.GetVceLogCargaCabRepository().GetMinIdByVersion(pecacodi, nombTabla);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //CAMBIOS MIGUEL

        //CAMBIOS JOSE

        public void SaveVceLogCargaDet(int codigo, string usuario, string tabla, int pecacodi)
        {
            try
            {
                FactorySic.GetVceLogCargaDetRepository().SaveDetalle(codigo, usuario, tabla, pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void LlenarCostosVariables(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCostoVariableRepository().LlenarCostoVariable(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void LlenarMedeneriaGrupo(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompRegularDetRepository().LlenarMedeneriaGrupo(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public IDataReader ListCompensacionArrPar(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ListCompensacionArrPar(pecacodi, empresa, central, grupo, modo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public IDataReader ListCabCompensacionArrPar(int pecacodi)
        {
            try
            {
                return FactorySic.GetVceDatCalculoRepository().ListCabCompensacionArrPar(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public List<VceCfgDatCalculoDTO> ObtenerCfgsCampo(string cfgdctipoval)
        {
            return FactorySic.GetVceCfgDatCalculoRepository().ObtenerCfgsCampo(cfgdctipoval);
        }

        //- compensaciones.JDEL - 05/03/2017: Cambio para atender el requerimiento. 
        public void SaveArranquesParadas(int pecacodi)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().SaveArranquesParadas(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public void ActualizarTipoCombustible(int pecacodi, string fechaModificacion)
        {
            FactorySic.GetVceDatCalculoRepository().ActualizarTipoCombustible(pecacodi, fechaModificacion);
        }

        // DSH 06-05-2017 : Se agrego por requerimiento
        public void SaveVceDatCalculoFromOtherversion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            try
            {
                FactorySic.GetVceDatCalculoRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);

                int id = GetMinIdVceLogCargaCab(pecacodiDestino, "VCE_DATCALCULO");

                if (id > 0)
                {
                    SaveVceLogCargaDet(id, usuCreacion, "VCE_DATCALCULO", pecacodiDestino);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }

        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public List<PrGrupodatDTO> getModosOperacion(string tipocatecodi)
        {
            return FactorySic.GetPrGrupodatRepository().GetModosOperacion(tipocatecodi);
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public VceArrparIncredGenDTO ObtenerIncrementoReduccion(int pecacodi, int GrupoCodi, string ApinrefechaDesc)
        {
            return FactorySic.GetVceArrparIncredGenRepository().GetById(pecacodi, GrupoCodi, ApinrefechaDesc);
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public void CrearIncrementoReduccion(VceArrparIncredGenDTO oVceArrparIncredGenDTO)
        {
            FactorySic.GetVceArrparIncredGenRepository().Save(oVceArrparIncredGenDTO);
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public void GuardarIncrementoReduccion(VceArrparIncredGenDTO oVceArrparIncredGenDTO)
        {
            FactorySic.GetVceArrparIncredGenRepository().Update(oVceArrparIncredGenDTO);
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public void EliminarIncrementoReduccion(VceArrparIncredGenDTO oVceArrparIncredGenDTO)
        {
            FactorySic.GetVceArrparIncredGenRepository().Delete(oVceArrparIncredGenDTO);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public List<VceArrparGrupoCabDTO> getListVceArrparGrupoCabDTO(int periodo)
        {
            return FactorySic.GetVceArrparGrupoCaRepository().GetByPeriod(periodo);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public VceArrparGrupoCabDTO ObtenerCalculoManual(int pecacodi, int Grupocodi, string Apgcfccodi)
        {
            return FactorySic.GetVceArrparGrupoCaRepository().GetById(pecacodi, Grupocodi, Apgcfccodi);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public void CrearCalculoManual(VceArrparGrupoCabDTO oVceArrparGrupoCabDTO)
        {
            FactorySic.GetVceArrparGrupoCaRepository().Save(oVceArrparGrupoCabDTO);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public void ActualizarCalculoManual(VceArrparGrupoCabDTO oVceArrparGrupoCabDTO)
        {
            FactorySic.GetVceArrparGrupoCaRepository().Update(oVceArrparGrupoCabDTO);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public void EliminarCalculoManual(VceArrparGrupoCabDTO oVceArrparGrupoCabDTO)
        {
            FactorySic.GetVceArrparGrupoCaRepository().DeleteEditCalculo(oVceArrparGrupoCabDTO);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public List<VceArrparCompEspDTO> getListVceArrparCompEspDTO(int periodo)
        {
            return FactorySic.GetVceArrparCompEspRepository().GetByPeriod(periodo);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public List<VceArrparTipoOperaDTO> getTiposOperacion(string tipo)
        {
            return FactorySic.GetVceArrparTipoOperaRepository().ListByType(tipo);
        }


        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public VceArrparRampaCfgDTO obtenerRangoInferiorPar(int Grupocodi, string Aptopsubtipo, decimal Apespcargafinal)
        {
            return FactorySic.GetVceArrparRampaCfgRepository().ObtenerRangoInferiorPar(Grupocodi, Aptopsubtipo, Apespcargafinal);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public VceArrparRampaCfgDTO obtenerRangoSuperiorPar(int Grupocodi, string Aptopsubtipo, decimal Apespcargafinal)
        {
            return FactorySic.GetVceArrparRampaCfgRepository().ObtenerRangoSuperiorPar(Grupocodi, Aptopsubtipo, Apespcargafinal);
        }


        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public VceArrparRampaCfgDTO obtenerRangoInferiorArr(int Grupocodi, string Aptopsubtipo, decimal Apespcargafinal)
        {
            return FactorySic.GetVceArrparRampaCfgRepository().ObtenerRangoInferiorArr(Grupocodi, Aptopsubtipo, Apespcargafinal);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public VceArrparRampaCfgDTO obtenerRangoSuperiorArr(int Grupocodi, string Aptopsubtipo, decimal Apespcargafinal)
        {
            return FactorySic.GetVceArrparRampaCfgRepository().ObtenerRangoSuperiorArr(Grupocodi, Aptopsubtipo, Apespcargafinal);
        }


        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public int getConceptoVceArrparTipoOpera(string Apesptipo, string Aptopsubtipo)
        {
            return FactorySic.GetVceArrparTipoOperaRepository().getConceptoVceArrparTipoOpera(Apesptipo, Aptopsubtipo);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public VceArrparCompEspDTO getVceArrparCompEsp(int periodo, int grupo, string fechaDesc, string apstocodi)
        {
            return FactorySic.GetVceArrparCompEspRepository().GetById(periodo, grupo, fechaDesc, apstocodi);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public void eliminarVceArrparCompEsp(VceArrparCompEspDTO oVceArrparCompEspDTO)
        {
            FactorySic.GetVceArrparCompEspRepository().Delete(oVceArrparCompEspDTO);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public void crearVceArrparCompEsp(VceArrparCompEspDTO oVceArrparCompEspDTO)
        {
            FactorySic.GetVceArrparCompEspRepository().Save(oVceArrparCompEspDTO);
        }

        // DSH 06-05-2017 : Se agrego por requerimiento
        public void SaveVceArrparGrupoCabFromOtherversion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            FactorySic.GetVceArrparGrupoCaRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);
        }

        // DSH 05-05-2017 : Se agrego por requerimiento
        public void DeleteVceArrparGrupoCabByVersion(int pecacodi)
        {
            FactorySic.GetVceArrparGrupoCaRepository().DeleteByVersion(pecacodi);
        }

        // DSH 06-05-2017 : Se agrego por requerimiento
        public void SaveVceCostoVariableFromOtherversion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            FactorySic.GetVceCostoVariableRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);
        }

        // DSH 05-05-2017 : Se agrego por requerimiento
        public void DeleteVceCostoVariableByVersion(int pecacodi)
        {
            FactorySic.GetVceCostoVariableRepository().DeleteByVersion(pecacodi);
        }

        // DSH 06-05-2017 : Se agrego por requerimiento
        public void SaveVceEnergiaFromOtherversion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            FactorySic.GetVceEnergiaRepository().SaveFromOtherVersion(pecacodiDestino, pecacodiOrigen);

            int id = GetMinIdVceLogCargaCab(pecacodiDestino, "VCE_ENERGIA");

            if (id > 0)
            {
                SaveVceLogCargaDet(id, usuCreacion, "VCE_ENERGIA", pecacodiDestino);
            }
        }

        // DSH 05-05-2017 : Se agrego por requerimiento
        public void DeleteVceEnergiaByVersion(int pecacodi)
        {
            FactorySic.GetVceEnergiaRepository().DeleteByVersion(pecacodi);
        }

        public IDataReader ListCompensacionOperacionMME(int pecacodi, List<EveSubcausaeventoDTO> subCausaEvento)
        {
            try
            {
                return FactorySic.GetVceCompBajaeficRepository().ListCompensacionOperacionMME(pecacodi, subCausaEvento);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public IDataReader ListCompensacionDiarioMME(int pecacodi, List<PrGrupoDTO> grupoCompensacion)
        {
            try
            {
                return FactorySic.GetVceCompBajaeficRepository().ListCompensacionDiarioMME(pecacodi, grupoCompensacion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //Cambio atender requerimiento
        public List<PrGrupoDTO> ListModoOperacion(int pecacodi)
        {
            var list = new List<PrGrupoDTO>();
            try
            {
                var periodo = FactorySic.GetVcePeriodoCalculoRepository().GetPeriodo(pecacodi);
                var periodoMaximo = FactorySic.GetVcePeriodoCalculoRepository().GetPeriodoMaximo(periodo.PeriCodi);
                return FactorySic.GetPrGrupoRepository().GetListaModosOperacion(periodo.PeriAnioMes,pecacodi,periodoMaximo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
           
        }

        public int UpdateCompensacionInforme(int pericodi, string nombreInforme)
        {
            var res = 0;
            try
            {

                res = FactorySic.GetVcePeriodoCalculoRepository().UpdateCompensacionInforme(pericodi, nombreInforme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return res;
        }

        public List<PrGrupoDTO> GetListaModosIds(string fechadatos)
        {
            var list = new List<PrGrupoDTO>();
            try
            {

                return FactorySic.GetPrGrupoRepository().GetListaModosIds(fechadatos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public void SaveVceGrupoExcluido(VceGrupoExcluidoDTO entity)
        {
            try
            {
                int idGen = FactorySic.GetVceGrupoExcluidoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteVceGrupoExcluido(int pecacodi, int grupocodi)
        {
            try
            {
                FactorySic.GetVceGrupoExcluidoRepository().Delete(pecacodi, grupocodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteVceGrupoExcluidoByVersion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceGrupoExcluidoRepository().DeleteByVersion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<VceCfgValidaConceptoDTO> ListValidaConceptos()
        {
            return FactorySic.GetVceCfgValidaConceptoRepository().ListValidaConceptos();
        }

        public VceCfgValidaConceptoDTO GetVceCfgValidaConceptoById(int crcvalcodi)
        {
            return FactorySic.GetVceCfgValidaConceptoRepository().GetById(crcvalcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPO
        /// </summary>
        public PrGrupoDTO GetByIdPrGrupo(int grupocodi)
        {
            return FactorySic.GetPrGrupoRepository().GetById(grupocodi);
        }
        public List<PrGrupodatDTO> ListarAsignacionBarraModoOperacion(int grupocodi)
        {
            return FactorySic.GetPrGrupodatRepository().ListarAsignacionBarraModoOperacion(grupocodi);
        }

        public List<PrBarraDTO> ListarBarras()
        {
         return FactorySic.GetPrBarraRepository().List();

        }

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPODAT
        /// </summary>
        public PrGrupodatDTO GetByIdPrGrupodat(DateTime fechadat, int concepcodi, int grupocodi, int deleted)
        {
            return FactorySic.GetPrGrupodatRepository().GetById(fechadat, concepcodi, grupocodi, deleted);
        }

        /// <summary>
        /// Inserta un registro de la tabla PR_GRUPODAT
        /// </summary>
        public void SavePrGrupodat(PrGrupodatDTO entity)
        {
            (new DespachoAppServicio()).ActualizarGrupodat(false, entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla PR_GRUPODAT
        /// </summary>
        public void UpdatePrGrupodat(PrGrupodatDTO entity)
        {
            (new DespachoAppServicio()).ActualizarGrupodat(false, entity);
        }

        //Compensacion Manual MME 20/04/2020
        /// <summary>
        /// Elimina Registro Tabla VCE_COMP_MME_DET_MANUAL
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="grupocodi"></param>
        /// <param name="cmmedmhora"></param>
        public void DeleteCompensacionManual(int pecacodi, int grupocodi, DateTime cmmedmhora)
        {
            try
            {
                FactorySic.GetVceCompMMEDetManualRepository().DeleteCompensacionManual(pecacodi, grupocodi, cmmedmhora);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina Registro Tabla VCE_COMP_MME_DET_MANUAL por version
        /// </summary>
        /// <param name="pecacodi"></param>
        public void DeleteCompensacionManualByVersion(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompMMEDetManualRepository().DeleteCompensacionManualByVersion(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda Registro Tabla VCE_COMP_MME_DET_MANUAL
        /// </summary>
        /// <param name="entity"></param>
        public void SaveCompMMEDetManual(VceCompMMEDetManualDetDTO entity)
        {
            try
            {
                FactorySic.GetVceCompMMEDetManualRepository().SaveEntity(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza registro en la tabla VCE_COMP_REGULAR_DET desde VCE_COMP_MME_DET_MANUAL
        /// </summary>
        /// <param name="pecacodi"></param>
        public void UpdateCompensacionDet(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompMMEDetManualRepository().UpdateCompensacionDet(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta registro en la tabla VCE_COMP_REGULAR_DET desde VCE_COMP_MME_DET_MANUAL
        /// </summary>
        /// <param name="pecacodi"></param>
        public void SaveCompensacionDet(int pecacodi)
        {
            try
            {
                FactorySic.GetVceCompMMEDetManualRepository().SaveCompensacionDet(pecacodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista registros de la tabla VCE_COMP_MME_DET_MANUAL
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="grupo"></param>
        /// <param name="modo"></param>
        /// <param name="tipo"></param>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="tipocalculo"></param>
        /// <returns></returns>
        public List<VceCompMMEDetManualDetDTO> ListCompensacionesManuales(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fechaini, string fechafin, string tipocalculo)
        {
            try
            {
                return FactorySic.GetVceCompMMEDetManualRepository().ListCompensacionesManuales( pecacodi,  empresa,  central,  grupo,  modo,  tipo,  fechaini,  fechafin,  tipocalculo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
