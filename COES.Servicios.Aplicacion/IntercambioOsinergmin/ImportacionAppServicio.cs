using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper;
using log4net;
using Org.BouncyCastle.Security;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin
{
    /// <summary>
    /// Flujo de Importacion de la data del SICLI a COES
    /// </summary>
    public class ImportacionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ImportacionAppServicio));

        /// <summary>
        /// Listar anios 
        /// </summary>
        /// <returns></returns>
        public List<string> PeriodoListAnios()
        {
            try
            {
                var anios = FactorySic.GetPeriodoSicliRepository().ListAnios();
                if (!anios.Contains(DateTime.Today.Year.ToString()))
                    anios.Add(DateTime.Today.Year.ToString());

                return anios;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Lista de periodos sicli
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<IioPeriodoSicliDTO> PeriodoGetByCriteria(string anio)
        {
            try
            {
                return FactorySic.GetPeriodoSicliRepository().GetByCriteria(anio);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Periodo por ID
        /// </summary>
        /// <param name="iioPeriodoSicliDTO"></param>
        /// <returns></returns>
        public IioPeriodoSicliDTO PeriodoGetById(IioPeriodoSicliDTO iioPeriodoSicliDTO)
        {
            try
            {
                return FactorySic.GetPeriodoSicliRepository().GetById(iioPeriodoSicliDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar periodos del sicli
        /// </summary>
        /// <param name="scoPeriodoRemisionDto"></param>
        /// <returns></returns>
        public string PeriodoSave(IioPeriodoSicliDTO iioPeriodoSicliDto)
        {
            try
            {
                FactorySic.GetPeriodoSicliRepository().Save(iioPeriodoSicliDto);
                return iioPeriodoSicliDto.PsicliAnioMesPerrem;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar periodo del sicli
        /// </summary>
        /// <param name="scoPeriodoRemisionDto"></param>
        /// <returns></returns>
        public string PeriodoUpdate(IioPeriodoSicliDTO iioPeriodoSicliDto)
        {
            try
            {
                FactorySic.GetPeriodoSicliRepository().Update(iioPeriodoSicliDto);
                return iioPeriodoSicliDto.PsicliAnioMesPerrem;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #region Métodos para tabla ControlImportación

        /// <summary>
        /// Guardar control de Importacion
        /// </summary>
        /// <param name="iioControlImportacionDTO"></param>
        /// <returns></returns>
        public int ControlImportacionSave(IioControlImportacionDTO iioControlImportacionDTO)
        {
            try
            {
                int id;
                if (iioControlImportacionDTO.Rcimcodi == 0)
                {
                    id = FactorySic.GetControlImportacionRepository().Save(iioControlImportacionDTO);
                }
                else
                {
                    FactorySic.GetControlImportacionRepository().Update(iioControlImportacionDTO);
                    id = iioControlImportacionDTO.Rcimcodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el control de importación
        /// </summary>
        /// <param name="iioControlImportacionDTO"></param>
        /// <returns></returns>
        public IioControlImportacionDTO ControlImportacionGetByCriteria(IioControlImportacionDTO iioControlImportacionDTO)
        {
            try
            {
                return FactorySic.GetControlImportacionRepository().GetByCriteria(iioControlImportacionDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int CantidadRegistrosImportacion(int periodo)
        {
            try
            {
                return FactorySic.GetControlImportacionRepository().GetCantidadRegistros(periodo);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int GetMaxIdControlImportacion()
        {
            try
            {
                return FactorySic.GetControlImportacionRepository().GetMaxId();

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void GrabarMasivoControlImportacion(List<IioControlImportacionDTO> entitys)
        {
            try
            {
                FactorySic.GetControlImportacionRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<IioControlImportacionDTO> ListByTabla(int periodo, string tabla)
        {
            try
            {
                return FactorySic.GetControlImportacionRepository().ListByTabla(periodo, tabla);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public IioControlImportacionDTO GetByEmpresaTabla(int periodo, string tabla, string empresa)
        {
            try
            {
                return FactorySic.GetControlImportacionRepository().GetByEmpresaTabla(periodo, tabla, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        #region Métodos IIO_TMP_CONSUMO

        /// <summary>
        /// Graba de forma masiva en la tabla IIO_TMP_CONSUMO
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarMasivoTmpConsumo(List<IioTmpConsumoDTO> entitys)
        {
            try
            {
                FactorySic.GetTmpConsumoRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar la información de la Tabla IIO_TMP_CONSUMO
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="uconempcodi"></param>
        public void TmpConsumoDelete()
        {
            try
            {
                FactorySic.GetTmpConsumoRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Migrar la información a la tabla ME_MEDICION96
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="tipoInfoCodi"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public string MigrateMeMedicion96(int lectCodi, int tipoInfoCodi, string periodo)
        {
            try
            {
                return FactorySic.GetTmpConsumoRepository().MigrateMeMedicion96(lectCodi, tipoInfoCodi, periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos IIO_FACTURA

        /// <summary>
        /// Graba de forma masiva en la tabla IIO_TMP_CONSUMO
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarMasivoFactura(List<IioFacturaDTO> entitys)
        {
            try
            {
#warning HDT.09.07.2017 - Inicio:  Comentado temporalmente.
                FactorySic.GetFacturaRepository().BulkInsert(entitys);
                //FactorySic.GetFacturaRepository().Save(entitys);
#warning HDT Fin
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void FacturaDelete(int psiclicodi, int emprcodi)
        {
            try
            {
                FactorySic.GetFacturaRepository().Delete(psiclicodi, emprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #endregion


        //- alpha.JDEL - Inicio 03/11/2016: Cambio para atender el requerimiento.


        public SiEmpresaDTO GetSiEmpresaByCodOsinergmin(string codOsinergmin)
        {
            try
            {
                return FactorySic.GetSiEmpresaRepository().GetByCodigoOsinergmin(codOsinergmin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 14/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la empresa de acuerdo con el RUC.
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public SiEmpresaDTO GetUsuarioLibreByRuc(string ruc)
        {
            try
            {
                return FactorySic.GetSiEmpresaRepository().GetUsuarioLibreByRuc(ruc);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public EqEquipoDTO GetEqEquipoByCodOsinergmin(string codOsinergmin)
        {
            try
            {
                return FactorySic.GetEqEquipoRepository().GetByCodOsinergmin(codOsinergmin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 16/08/2017: Cambio para atender el requerimiento. 
        public EqEquipoDTO GetEqEquipo(string codOsinergmin, int codigoFamilia)
        {
            try
            {
                return FactorySic.GetEqEquipoRepository().GetEqEquipo(codOsinergmin, codigoFamilia);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public PrBarraDTO GetPrBarraByCodOsinergmin(string codOsinergmin)
        {
            try
            {
                return FactorySic.GetPrBarraRepository().GetByCodOsinergmin(codOsinergmin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- JDEL Fin


        //- alpha.HDT - Inicio 10/04/2017: Cambio para atender el requerimiento.
        /// <summary>
        /// Obtiene la lista de incidentes que reportan la duplicación en la configuración del COES.
        /// </summary>
        /// <returns></returns>
        public List<IioLogImportacionIncidenteDTO> GetDuplicadosConfiguracionCOES()
        {
            try
            {
                return FactorySic.GetIioLogImportacionRepository().GetDuplicadosConfiguracionCOES();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite actualizar la columna IIO_TMP_CONSUMO.PTOMEDICODI para realizar la migración desde la tabla en cuestión
        /// hacia la tabla ME_MEDICION96.
        /// </summary>
        public void UpdatePtoMediCodiTmpConsumo()
        {
            try
            {
                FactorySic.GetTmpConsumoRepository().UpdatePtoMediCodiTmpConsumo();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de incidentes en la tabla IIO_TMP_CONSUMO que no tienen puntos de medición.
        /// </summary>
        /// <returns></returns>
        public List<IioLogImportacionIncidenteDTO> GetIncidentesSinPuntoMedicionCOES()
        {
            try
            {
                return FactorySic.GetIioLogImportacionRepository().GetIncidentesSinPuntoMedicionCOES();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener el correlativo disponible para el registro del log de la importación Sicli.
        /// </summary>
        /// <returns></returns>
        public int GetCorrelativoDisponibleLogImportacionSicli()
        {
            try
            {
                return FactorySic.GetIioLogImportacionRepository().GetCorrelativoDisponibleLogImportacionSicli();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        //- alpha.HDT - 12/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite eliminar las incidencias de importación de una importación determinada.
        /// </summary>
        /// <param name="Rcimcodi"></param>
        /// <param name="periodo"></param>
        public void EliminarIncidenciasImportacionSicli(int Rcimcodi, string periodo)
        {
            try
            {
                FactorySic.GetIioLogImportacionRepository().EliminarIncidenciasImportacionSicli(Rcimcodi, periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 12/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener las inidencias de importación para un código de importación determinado.
        /// </summary>
        /// <param name="rcImCodi"></param>
        /// <returns></returns>
        public List<IioLogImportacionDTO> GetIncidenciasImportacion(int rcImCodi)
        {
            try
            {
                return FactorySic.GetIioLogImportacionRepository().GetIncidenciasImportacion(rcImCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 12/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo registro de lo de importación.
        /// </summary>
        /// <param name="oIioLogImportacionDTO"></param>
        public void SaveIioLogImportacion(IioLogImportacionDTO oIioLogImportacionDTO)
        {
            try
            {
                FactorySic.GetIioLogImportacionRepository().SaveIioLogImportacion(oIioLogImportacionDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite generar el reporte de la tabla en cuestión.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="tabla"></param>
        /// <param name="empresasIn"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="rutaLogo"></param>
        /// <returns></returns>
        public String GenerarReporteTabla(string periodo, string tabla, string empresasIn, string rutaArchivo, string rutaLogo, string fechaDia)
        {
            string nombreArchivo = string.Empty;

            try 
	        {
                nombreArchivo = rutaArchivo + string.Format(ConstantesIio.ReporteDatosImportacionFileName, tabla);

                if (tabla == "Tabla04")
                {
                    List<IioTabla04DTO>
                        lIioTabla04DTO = FactorySic.GetIioLogImportacionRepository().GetDatosTabla04(periodo, empresasIn, fechaDia);

                    ExcelDocument.GenerarReporteTabla04ImportacionSicli(nombreArchivo, lIioTabla04DTO);		
                }
                else if (tabla == "Tabla05") {
                    List<IioTabla05DTO>
                        lIioTabla05DTO = FactorySic.GetIioLogImportacionRepository().GetDatosTabla05(periodo, empresasIn);

                    ExcelDocument.GenerarReporteTabla05ImportacionSicli(nombreArchivo, lIioTabla05DTO);		
                }
	        }
	        catch (Exception)
	        {
		        throw;
	        }

            return nombreArchivo;
        }

        //- alpha.HDT - 08/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo registro en la tabla de barras del COES.
        /// </summary>
        /// <param name="bar"></param>
        public int InsertarBarra(PrBarraDTO bar)
        {
            try
            {
                return FactorySic.GetPrBarraRepository().InsertarBarra(bar);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 12/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener las incidencias de la importación relacionadas con suministros.
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="empresasIn"></param>
        /// <returns></returns>
        public List<IioLogImportacionDTO> GetIncidenciasImportacionSuministro(string periodo, string empresasIn)
        {
            try
            {
                return FactorySic.GetIioLogImportacionRepository().GetIncidenciasImportacionSuministro(periodo, empresasIn);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 13/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la lista de áreas activas.
        /// </summary>
        /// <returns></returns>
        public List<EqAreaDTO> GetAreasSubestacion()
        {
            try
            {
                return FactorySic.GetEqAreaRepository().ListSubEstacion();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 22/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo punto de medición.
        /// </summary>
        /// <param name="eqEquipoDTO"></param>
        /// <returns></returns>
        public int InsertarEquipo(EqEquipoDTO eqEquipoDTO)
        {
            try
            {
                return FactorySic.GetEqEquipoRepository().Save(eqEquipoDTO);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 22/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo equipo.
        /// </summary>
        /// <param name="mePtomedicionDTO"></param>
        /// <returns></returns>
        public int InsertarPuntoMedicion(MePtomedicionDTO mePtomedicionDTO)
        {
            try
            {
                return FactorySic.GetMePtomedicionRepository().Save(mePtomedicionDTO);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 18/08/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener una lista de puntos de medición del equipo pasada como parámetro.
        /// </summary>
        /// <param name="equiCodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerPuntoMedicionPorEquipo(int equiCodi)
        {
            try
            {
                return FactorySic.GetMePtomedicionRepository().GetByIdEquipo(equiCodi);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener una lista de puntos de medición del equipo pasada como parámetro.
        /// </summary>
        /// <param name="equiCodi"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO> ObtenerPuntoMedicionPorEquipoUsuarioLibre(int equiCodi, int emprcodisuministrador)
        {
            try
            {
                return FactorySic.GetMePtomedicionRepository().GetByIdEquipoUsuarioLibre(equiCodi, emprcodisuministrador);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 22/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite eliminar un registro del incidente de importación.
        /// </summary>
        /// <param name="ulogCodi"></param>
        public void EliminarIncidenteImportacion(int ulogCodi)
        {
            try
            {
                FactorySic.GetIioLogImportacionRepository().Delete(ulogCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 10/10/2017: Cambio para atender el requerimiento. 
        public void InsertarHojaPtoMed(MeHojaptomedDTO meHojaptomedDTO, int emprCodi)
        {
            try
            {
                FactorySic.GetMeHojaptomedRepository().Save(meHojaptomedDTO, emprCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- alpha.HDT - 10/10/2017: Cambio para atender el requerimiento. 
        public void InsertarPtoSuministrador(MePtosuministradorDTO mePtosuministradorDTO)
        {
            try
            {
                FactorySic.GetMePtosuministradorRepository().Save(mePtosuministradorDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //- pr16.HDT - 01/04/2018: Cambio para atender el requerimiento. 
        public IioPeriodoSicliDTO PeriodoGetByCodigo(int pSicliCodi)
        {
            try
            {
                return FactorySic.GetPeriodoSicliRepository().PeriodoGetByCodigo(pSicliCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #region Mejoras PR16
        //Mejoras PR16
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ObtenerSicliOsigFacturaMaximoId()
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().GetMaxId();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        public void DeleteSicliOsigFactura(string periodo)
        {
            try
            {
                FactorySic.GetIioSicliOsigFacturaRepository().Delete(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iioSicliOsigFacturaDTO"></param>
        public void SaveSicliOsigFactura(IioSicliOsigFacturaDTO iioSicliOsigFacturaDTO)
        {
            try
            {
                FactorySic.GetIioSicliOsigFacturaRepository().Save(iioSicliOsigFacturaDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public int GetCountTotal(string periodo)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().GetCountTotal(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int GetCountTotalRuc(string periodo)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().GetCountTotalRuc(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public int GetCountTotalFactura(string periodo)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().GetCountTotalFactura(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int GetCountTotalFacturaRuc(string periodo)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().GetCountTotalFacturaRuc(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public System.Data.IDataReader ListRepCompCliente(string periodo)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().ListRepCompCliente(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public System.Data.IDataReader ListRepCompEmpresa(string periodo)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().ListRepCompEmpresa(periodo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public System.Data.IDataReader ListRepCompHistorico(DateTime periodoInicio, DateTime periodoFin)
        {
            try
            {
                return FactorySic.GetIioSicliOsigFacturaRepository().ListRepCompHistorico(periodoInicio, periodoFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public IioPeriodoSicliDTO PeriodoGetByCodigo(string periodoSicli)
        {
            try
            {
                return FactorySic.GetPeriodoSicliRepository().PeriodoGetByCodigo(periodoSicli);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Nuevas Tablas Importacion Osinergmin - Metodos tabla IIO_OSIG_CONSUMO_UL

        /// <summary>
        /// Eliminar la información de la Tabla IIO_OSIG_CONSUMO_UL
        /// </summary>

        public void OsigConsumoDelete(int psiclicodi, string empresa)
        {
            try
            {
                FactorySic.GetOsigConsumoRepository().Delete(psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Graba de forma masiva en la tabla IIO_OSIG_CONSUMO_UL
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarMasivoOsigConsumo(List<IioOsigConsumoUlDTO> entitys)
        {
            try
            {
                FactorySic.GetOsigConsumoRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar desde la tabla IIO_TMP_CONSUMO hacia la tabla IIO_OSIG_CONSUMO
        /// </summary>
        /// <param name="usuario"></param>
        public void SaveOsigConsumo(string usuario)
        {
            try
            {
                FactorySic.GetOsigConsumoRepository().SaveOsigConsumo(usuario);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Permite actualizar la columna IIO_OSIG_CONSUMO_UL.PTOMEDICODI para realizar la migración desde la tabla en cuestión
        /// hacia la tabla ME_MEDICION96.
        /// </summary>
        public void UpdatePtoMediCodiOsigConsumo(int emprcodisuministrador, int psiclicodi, string empresa)
        {
            try
            {
                FactorySic.GetOsigConsumoRepository().UpdatePtoMediCodiOsigConsumo(emprcodisuministrador, psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite migrar informacion a la tabla ME_MEDICION96
        /// </summary>
        /// <param name="lectCodi"></param>
        /// <param name="tipoInfoCodi"></param>
        /// <param name="periodo"></param>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public void MigrateMeMedicion96OsigConsumo(int lectCodi, int tipoInfoCodi, string periodo, int psiclicodi, string empresa)
        {
            try
            {
                FactorySic.GetOsigConsumoRepository().MigrateMeMedicion96(lectCodi, tipoInfoCodi, periodo, psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite validar los registro de la tabla IIO_OSIG_CONSUMO_UL antes de migrar a la tabla ME_MEDICION96
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public string ValidarMigracionMeMedicion96(int psiclicodi, string empresa)
        {
            var resp = string.Empty;
            try
            {
                resp = FactorySic.GetOsigConsumoRepository().ValidarMigracionMeMedicion96(psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return resp;
        }
        /// <summary>
        /// Permite generar el Log de Importacion cuando no hay punto de medicion
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="periodo"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        /// <param name="empresas"></param>
        public void GenerarOsigConsumoLogImportacionPtoMedicion(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {
            try
            {
                FactorySic.GetOsigConsumoRepository().GenerarOsigConsumoLogImportacionPtoMedicion(psiclicodi, periodo, usuario, tabla, empresas);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Métodos IIO_OSIG_FACTURA_UL

        /// <summary>
        /// Graba de forma masiva en la tabla IIO_OSIG_FACTURA_UL
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarMasivoOsigFactura(List<IioOsigFacturaUlDTO> entitys)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="ulfactcodempresa"></param>
        public void OsigFacturaDelete(int psiclicodi, string ulfactcodempresa)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().Delete(psiclicodi, ulfactcodempresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar los puntos de medicion en la tabla IIO_OSIG_FACTURA_UL
        /// </summary>
        /// <param name="emprcodisuministrador"></param>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        public void UpdatePtoMediCodiOsigFactura(int emprcodisuministrador, int psiclicodi, string empresa)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().UpdatePtoMediCodiOsigFactura(emprcodisuministrador, psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar los codigos de empresa en la tabla IIO_OSIG_FACTURA_UL
        /// </summary>
        /// <param name="emprcodisuministrador"></param>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        public void UpdateEmprcodiOsigFactura(int emprcodisuministrador, int psiclicodi, string empresa)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().UpdateEmprcodiOsigFactura(emprcodisuministrador, psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para validar si no hay puntos de medicion asignados a los registros migrados de Osinergmin Tabla 5
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        public string ValidarOsigFacturaPuntoMedicion(int psiclicodi, string empresa)
        {
            var resultado = "";
            try
            {
                resultado = FactorySic.GetOsigFacturaRepository().ValidarOsigFacturaPuntoMedicion(psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Permite Migrar registros de la tabla IIO_OSIG_FACTURA_UL hacia la tabla IIO_FACTURA
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        public void SaveIioFactura(int psiclicodi, string empresa)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().SaveIioFactura(psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el Log de Importacion cuando no hay empresa
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="periodo"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        /// <param name="empresas"></param>
        public void GenerarOsigFacturaLogImportacionEmpresa(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().GenerarOsigFacturaLogImportacionEmpresa(psiclicodi, periodo, usuario, tabla, empresas);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite generar el Log de Importacion cuando no hay punto de medicion
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="periodo"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        /// <param name="empresas"></param>
        public void GenerarOsigFacturaLogImportacionPtoMedicion(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().GenerarOsigFacturaLogImportacionPtoMedicion(psiclicodi, periodo, usuario, tabla, empresas);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el flag de errores a no OK en la tabla IIO_CONTROL_IMPORTACION 
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresas"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        public void ActualizarControlImportacionNoOK(int psiclicodi, string empresas, string usuario, string tabla)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().ActualizarControlImportacionNoOK(psiclicodi, empresas, usuario, tabla);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el flag de errores a OK en la tabla IIO_CONTROL_IMPORTACION
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresas"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        public void ActualizarControlImportacionOK(int psiclicodi, string empresas, string usuario, string tabla)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().ActualizarControlImportacionOK(psiclicodi, empresas, usuario, tabla);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza cantidad de registros tabla IIO_CONTROL_IMPORTACION
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresas"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        public void ActualizarRegistrosImportacion(int psiclicodi, string empresas, string usuario, string tabla)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().ActualizarRegistrosImportacion(psiclicodi, empresas, usuario, tabla);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizamos Fecha Sincornizacion Coes
        /// </summary>
        /// <param name="psiclicodi"></param>
        public void ActualizarPeriodoFechaSincCoes(int psiclicodi)
        {
            try
            {
                FactorySic.GetOsigFacturaRepository().ActualizarPeridoFechaSincCoes(psiclicodi);
                //return iioPeriodoSicliDto.PsicliAnioMesPerrem;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validacion de Tablas Empresas pendientes de procesar
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public int ValidarOsigFacturaTablaEmpresas(int psiclicodi, string empresa)
        {
            var resultado = 0;
            try
            {
                resultado = FactorySic.GetOsigFacturaRepository().ValidarOsigFacturaTablaEmpresas(psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        #endregion

        #region Métodos IIO_OSIG_SUMINISTRO_UL

        /// <summary>
        /// Graba de forma masiva en la tabla IIO_OSIG_SUMINISTRO_UL
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarMasivoOsigSuministro(List<IioOsigSuministroUlDTO> entitys)
        {
            try
            {
                FactorySic.GetOsigSuministroRepository().BulkInsert(entitys);
                

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="ulfactcodempresa"></param>
        public void OsigSuministroDelete(int psiclicodi, string ulfactcodempresa)
        {
            try
            {
                FactorySic.GetOsigSuministroRepository().Delete(psiclicodi, ulfactcodempresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los equipos en la tabla IIO_OSIG_SUMINISTRO
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="ulfactcodempresa"></param>
        public void UpdateOsigSuministro(int psiclicodi, string ulfactcodempresa)
        {
            try
            {
                FactorySic.GetOsigSuministroRepository().UpdateOsigSuministro(psiclicodi, ulfactcodempresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para validar si no hay equipos asignados a los suministros migrados de Osinergmin
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="empresa"></param>
        public string ValidarOsigSuministroEquipos(int psiclicodi, string empresa)
        {
            var resultado = "";
            try
            {
                resultado = FactorySic.GetOsigSuministroRepository().ValidarOsigSuministroEquipos(psiclicodi, empresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Permite generar el Log de Importacion cuando no hay empresa
        /// </summary>
        /// <param name="psiclicodi"></param>
        /// <param name="periodo"></param>
        /// <param name="usuario"></param>
        /// <param name="tabla"></param>
        /// <param name="empresas"></param>
        public void GenerarOsigSuministroLogImportacionEquipo(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {
            try
            {
                FactorySic.GetOsigSuministroRepository().GenerarOsigSuministroLogImportacionEquipo(psiclicodi, periodo, usuario, tabla, empresas);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}