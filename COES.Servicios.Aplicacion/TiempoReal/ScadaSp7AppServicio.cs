using System;
using System.Collections.Generic;
using System.Linq;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using COES.Dominio.DTO.Scada;
using OfficeOpenXml;
using System.IO;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Framework.Base.Tools;

namespace COES.Servicios.Aplicacion.TiempoReal
{
    /// <summary>
    /// Clases con métodos del módulo Mediciones
    /// </summary>
    public class ScadaSp7AppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ScadaSp7AppServicio));

        #region Métodos Tabla ME_SCADA_SP7

        /// <summary>
        /// Inserta un registro de la tabla ME_SCADA_SP7
        /// </summary>
        public void SaveMeScadaSp7(MeScadaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetMeScadaSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_SCADA_SP7
        /// </summary>
        public void UpdateMeScadaSp7(MeScadaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetMeScadaSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_SCADA_SP7
        /// </summary>
        public void DeleteMeScadaSp7(int canalcodi, DateTime medifecha)
        {
            try
            {
                FactoryScada.GetMeScadaSp7Repository().Delete(canalcodi, medifecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_SCADA_SP7
        /// </summary>
        public MeScadaSp7DTO GetByIdMeScadaSp7(int canalcodi, DateTime medifecha)
        {
            return FactoryScada.GetMeScadaSp7Repository().GetById(canalcodi, medifecha);
        }

        /// <summary>
        /// Valida si un registro existe en la tabla ME_SCADA_SP7
        /// </summary>
        public bool GetSiExisteRegistroPorFechaYCanal(int canalcodi, DateTime fecha)
        {
            return FactoryScada.GetMeScadaSp7Repository().SiExisteRegistroPorFechaYCanal(canalcodi, fecha);
        }

        /// <summary>
        /// Agregar registro en la tabla ME_SCADA_SP7 por fecha y bloque
        /// </summary>
        public void AgregarRegistroPorFechaYBloque(int canalcodi, DateTime fecha, string bloque, string usuario, decimal valor)
        {
            FactoryScada.GetMeScadaSp7Repository().AgregarRegistroPorFechaYBloque(canalcodi, fecha, bloque, usuario, valor);
        }

        /// <summary>
        /// Actualizar registro en la tabla ME_SCADA_SP7 por fecha y bloque
        /// </summary>
        public void ActualizarRegistroPorFechaYBloque(int canalcodi, DateTime fecha, string bloque, string usuario, decimal valor)
        {
            FactoryScada.GetMeScadaSp7Repository().ActualizarRegistroPorFechaYBloque(canalcodi, fecha, bloque, usuario, valor);
        }

        /// <summary>
        /// Actualizar registro en la tabla ME_SCADA_SP7 por fecha y bloque
        /// </summary>
        public void ActualizarRegistrosNulosPorFechaYBloque(DateTime fecha, string bloque)
        {
            FactoryScada.GetMeScadaSp7Repository().ActualizarRegistrosNulosPorFechaYBloque(fecha, bloque);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_SCADA_SP7
        /// </summary>
        public List<MeScadaSp7DTO> ListMeScadaSp7s()
        {
            return FactoryScada.GetMeScadaSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeScadaSp7
        /// </summary>
        public List<MeScadaSp7DTO> GetByCriteriaMeScadaSp7s(DateTime fechaIni, DateTime fechaFin, string canalcodi)
        {
            return FactoryScada.GetMeScadaSp7Repository().GetByCriteria(fechaIni, fechaFin, canalcodi);
        }
               
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla ME_SCADA_SP7
        /// </summary>
        public List<MeScadaSp7DTO> BuscarOperaciones(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin, int nroPage, int pageSize)
        {
            return FactoryScada.GetMeScadaSp7Repository().BuscarOperaciones(ssee, zonaCodi, mediFechaIni, mediFechaFin, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla ME_SCADA_SP7
        /// </summary>
        public int ObtenerNroFilas(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin)
        {
            return FactoryScada.GetMeScadaSp7Repository().ObtenerNroFilas(ssee, zonaCodi, mediFechaIni, mediFechaFin);
        }
        
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla ME_SCADA_SP7
        /// </summary>
        public List<MeScadaSp7DTO> BuscarOperacionesReporte(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin)
        {
            return FactoryScada.GetMeScadaSp7Repository().BuscarOperacionesReporte(ssee, zonaCodi, mediFechaIni, mediFechaFin);
        }


        #endregion

        #region Métodos Tabla ME_SCADA_PTOFILTRO_SP7

        /// <summary>
        /// Inserta un registro de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public void SaveMeScadaPtofiltroSp7(MeScadaPtofiltroSp7DTO entity)
        {
            try
            {
                FactoryScada.GetMeScadaPtofiltroSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public void UpdateMeScadaPtofiltroSp7(MeScadaPtofiltroSp7DTO entity)
        {
            try
            {
                FactoryScada.GetMeScadaPtofiltroSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public void DeleteMeScadaPtofiltroSp7(int scdpficodi)
        {
            try
            {
                FactoryScada.GetMeScadaPtofiltroSp7Repository().Delete(scdpficodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina los registro de la tabla ME_SCADA_PTOFILTRO_SP7 según filtro
        /// </summary>
        public void DeleteFiltroMeScadaPtofiltroSp7(int filtrocodi)
        {
            try
            {
                FactoryScada.GetMeScadaPtofiltroSp7Repository().DeleteFiltro(filtrocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public MeScadaPtofiltroSp7DTO GetByIdMeScadaPtofiltroSp7(int scdpficodi)
        {
            return FactoryScada.GetMeScadaPtofiltroSp7Repository().GetById(scdpficodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public List<MeScadaPtofiltroSp7DTO> ListMeScadaPtofiltroSp7s()
        {
            return FactoryScada.GetMeScadaPtofiltroSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeScadaPtofiltroSp7
        /// </summary>
        public List<MeScadaPtofiltroSp7DTO> GetByCriteriaMeScadaPtofiltroSp7s()
        {
            return FactoryScada.GetMeScadaPtofiltroSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla ME_SCADA_PTOFILTRO_SP7
        /// </summary>
        public int SaveMeScadaPtofiltroSp7Id(MeScadaPtofiltroSp7DTO entity)
        {
            return FactoryScada.GetMeScadaPtofiltroSp7Repository().SaveMeScadaPtofiltroSp7Id(entity);
        }


        #endregion

        #region Métodos Tabla TR_ZONA_SP7

        /// <summary>
        /// Inserta un registro de la tabla TR_ZONA_SP7
        /// </summary>
        public void SaveTrZonaSp7(TrZonaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrZonaSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TR_ZONA_SP7
        /// </summary>
        public void UpdateTrZonaSp7(TrZonaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrZonaSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_ZONA_SP7
        /// </summary>
        public void DeleteTrZonaSp7(int zonacodi)
        {
            try
            {
                FactoryScada.GetTrZonaSp7Repository().Delete(zonacodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_ZONA_SP7
        /// </summary>
        public TrZonaSp7DTO GetByIdTrZonaSp7(int zonacodi)
        {
            return FactoryScada.GetTrZonaSp7Repository().GetById(zonacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_ZONA_SP7
        /// </summary>
        public List<TrZonaSp7DTO> ListTrZonaSp7s()
        {
            return FactoryScada.GetTrZonaSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrZonaSp7
        /// </summary>
        public List<TrZonaSp7DTO> GetByCriteriaTrZonaSp7s(string emprcodi)
        {
            return FactoryScada.GetTrZonaSp7Repository().GetByCriteria(emprcodi);
        }

        /// <summary>
        /// Graba los datos de la tabla TR_ZONA_SP7
        /// </summary>
        public int SaveTrZonaSp7Id(TrZonaSp7DTO entity)
        {
            return FactoryScada.GetTrZonaSp7Repository().SaveTrZonaSp7Id(entity);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_ZONA_SP7 por empresa
        /// </summary>
        public List<TrZonaSp7DTO> ListTrZonaSp7sByEmpresa(int emprcodi)
        {
            var lista = FactoryScada.GetTrZonaSp7Repository().ListByEmpresa(emprcodi);
            foreach (var reg in lista)
            {
                reg.Zonanomb = reg.Zonanomb != null ? reg.Zonanomb.Trim() : string.Empty;
            }
            return lista;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_ZONA_SP7 por empresa
        /// </summary>
        public List<TrZonaSp7DTO> ListTrZonaSp7sByEmpresaBdTreal(int emprcodi)
        {
            var lista = FactoryScada.GetTrZonaSp7BdTrealRepository().ListByEmpresa(emprcodi);
            foreach (var reg in lista)
            {
                reg.Zonanomb = reg.Zonanomb != null ? reg.Zonanomb.Trim() : string.Empty;
            }
            return lista;
        }

        #endregion

        #region Métodos Tabla ME_SCADA_FILTRO_SP7

        /// <summary>
        /// Inserta un registro de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public void SaveMeScadaFiltroSp7(MeScadaFiltroSp7DTO entity)
        {
            try
            {
                FactoryScada.GetMeScadaFiltroSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public void UpdateMeScadaFiltroSp7(MeScadaFiltroSp7DTO entity)
        {
            try
            {
                FactoryScada.GetMeScadaFiltroSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public void DeleteMeScadaFiltroSp7(int filtrocodi)
        {
            try
            {
                FactoryScada.GetMeScadaFiltroSp7Repository().Delete(filtrocodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public MeScadaFiltroSp7DTO GetByIdMeScadaFiltroSp7(int filtrocodi)
        {
            return FactoryScada.GetMeScadaFiltroSp7Repository().GetById(filtrocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public List<MeScadaFiltroSp7DTO> ListMeScadaFiltroSp7s()
        {
            return FactoryScada.GetMeScadaFiltroSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeScadaFiltroSp7
        /// </summary>
        public List<MeScadaFiltroSp7DTO> GetByCriteriaMeScadaFiltroSp7s()
        {
            return FactoryScada.GetMeScadaFiltroSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public int SaveMeScadaFiltroSp7Id(MeScadaFiltroSp7DTO entity)
        {
            return FactoryScada.GetMeScadaFiltroSp7Repository().SaveMeScadaFiltroSp7Id(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public List<MeScadaFiltroSp7DTO> BuscarOperaciones(string filtro, string creador, string modificador, int nroPage, int pageSize)
        {
            return FactoryScada.GetMeScadaFiltroSp7Repository().BuscarOperaciones(filtro, creador, modificador, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public int ObtenerNroFilas(string filtro, string creador, string modificador)
        {
            return FactoryScada.GetMeScadaFiltroSp7Repository().ObtenerNroFilas(filtro, creador, modificador);
        }

        #endregion

        #region Métodos Tabla TR_CANAL_SP7

        /// <summary>
        /// Permite obtener un registro de la tabla TR_CANAL_SP7
        /// </summary>
        public TrCanalSp7DTO GetByIdTrCanalSp7(int canalcodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetById(canalcodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TR_CANAL_SP7
        /// </summary>
        public TrCanalSp7DTO GetByIdTrCanalSp7BdTreal(int canalcodi)
        {
            return FactoryScada.GetTrCanalSp7BdTrealRepository().GetByIdBdTreal(canalcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_CANAL_SP7
        /// </summary>
        public List<TrCanalSp7DTO> ListTrCanalSp7s()
        {
            return FactoryScada.GetTrCanalSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrCanalSp7
        /// </summary>
        public List<TrCanalSp7DTO> GetByCriteriaTrCanalSp7s()
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByCriteria();
        }

        /// <summary>
        /// Permite obtener varios registros de la tabla TR_CANAL_SP7 filtrados por codigo
        /// </summary>
        public List<TrCanalSp7DTO> GetByIdsTrCanalSp7(string canalcodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByIds(canalcodi);
        }

        /// <summary>
        /// Permite obtener varios registros de la tabla TR_CANAL_SP7 filtrado por nombre
        /// </summary>
        public List<TrCanalSp7DTO> GetByCanalnombTrCanalSp7(string canalnomb)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByCanalnomb(canalnomb);
        }

        /// <summary>
        /// Permite obtener varios registros de la tabla TR_CANAL_SP7 filtrado por zona
        /// </summary>
        public List<TrCanalSp7DTO> GetByZonaTrCanalSp7(int zonacodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByZona(zonacodi);
        }

        /// <summary>
        /// Permite obtener varios registros de la tabla TR_CANAL_SP7 filtrado por zona y del tipo analógico
        /// </summary>
        public List<TrCanalSp7DTO> GetByZonaAnalogicoTrCanalSp7(int zonacodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByZonaAnalogico(zonacodi);
        }


        /// <summary>
        /// Permite obtener varios registros de la tabla TR_CANAL_SP7 de acuerdo a filtros personalizados por el usuario
        /// </summary>
        public List<TrCanalSp7DTO> GetByFiltroTrCanalSp7(int filtrocodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetByFiltro(filtrocodi);
        }

        /// <summary>
        /// Listar canala por zona y unidad
        /// </summary>
        /// <param name="tipoPunto"></param>
        /// <param name="zonacodi"></param>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<TrCanalSp7DTO> ListTrCanalSp7sByZonaAndUnidad(string tipoPunto, int emprcodi, int zonacodi, string unidad)
        {
            var lista = FactoryScada.GetTrCanalSp7BdTrealRepository().ListByZonaAndUnidad(tipoPunto, emprcodi, zonacodi, unidad);
            foreach (var reg in lista)
            {
                reg.Canalnomb = reg.Canalnomb != null ? reg.Canalnomb.Trim() : string.Empty;
                reg.Canalabrev = reg.Canalabrev != null ? reg.Canalabrev.Trim() : string.Empty;
            }
            return lista;
        }

        public List<TrCanalSp7DTO> ListTrCanalSp7sByIds(string canalcodis)
        {
            List<TrCanalSp7DTO> listaBD = new List<TrCanalSp7DTO>();
            if (string.IsNullOrEmpty(canalcodis))
                return listaBD;

            var listaCanalcodi = canalcodis.Split(',').Select(x => int.Parse(x)).ToList();

            //consultar cada rango
            int maxElementosPorSublista = 500;
            for (int i = 0; i < listaCanalcodi.Count; i += maxElementosPorSublista)
            {
                List<int> sublista = listaCanalcodi.GetRange(i, Math.Min(maxElementosPorSublista, listaCanalcodi.Count - i));

                listaBD.AddRange(FactoryScada.GetTrCanalSp7BdTrealRepository().GetByCriteriaBdTreal(string.Join(",", sublista)));
            }

            return listaBD;
        }

        #endregion

        #region Métodos Tabla F_LECTURA_SP7

        /// <summary>
        /// Inserta un registro de la tabla F_LECTURA_SP7
        /// </summary>
        public void SaveFLecturaSp7(FLecturaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetFLecturaSp7Repository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla F_LECTURA_SP7
        /// </summary>
        public void UpdateFLecturaSp7(FLecturaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetFLecturaSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla F_LECTURA_SP7
        /// </summary>
        public void DeleteFLecturaSp7(DateTime fechahora, int gpscodi)
        {
            try
            {
                FactoryScada.GetFLecturaSp7Repository().Delete(fechahora, gpscodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla F_LECTURA_SP7
        /// </summary>
        public FLecturaSp7DTO GetByIdFLecturaSp7(DateTime fechahora, int gpscodi)
        {
            return FactoryScada.GetFLecturaSp7Repository().GetById(fechahora, gpscodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla F_LECTURA_SP7
        /// </summary>
        public List<FLecturaSp7DTO> ListFLecturaSp7s()
        {
            return FactoryScada.GetFLecturaSp7Repository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla FLecturaSp7
        /// </summary>
        public List<FLecturaSp7DTO> GetByCriteriaFLecturaSp7s()
        {
            return FactoryScada.GetFLecturaSp7Repository().GetByCriteria();
        }
        
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla F_LECTURA_SP7
        /// </summary>
        public List<FLecturaSp7DTO> BuscarOperaciones(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            return FactoryScada.GetFLecturaSp7Repository().BuscarOperaciones(gpsCodi, fechaHoraIni, fechaHoraFin);
        }        

        public List<FLecturaSp7DTO> ObtenerMaximaFrecuencia(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            return FactoryScada.GetFLecturaSp7Repository().ObtenerMaximaFrecuencia(gpsCodi, fechaHoraIni, fechaHoraFin);
        }

        public List<FLecturaSp7DTO> ObtenerMinimaFrecuencia(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            return FactoryScada.GetFLecturaSp7Repository().ObtenerMinimaFrecuencia(gpsCodi, fechaHoraIni, fechaHoraFin);
        }

        public List<FLecturaSp7DTO> ObtenerSubitaFrecuencia(int gpsCodi, string transgresiones, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            return FactoryScada.GetFLecturaSp7Repository().ObtenerSubitaFrecuencia(gpsCodi, transgresiones, fechaHoraIni, fechaHoraFin);
        }

        public List<FLecturaSp7DTO> ObtenerSostenidaFrecuencia(int gpsCodi, string transgresiones, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            return FactoryScada.GetFLecturaSp7Repository().ObtenerSostenidaFrecuencia(gpsCodi, transgresiones, fechaHoraIni, fechaHoraFin);
        }


        #endregion

        #region Métodos Tabla TR_EMPRESA_SP7

        /// <summary>
        /// Actualiza un registro de la tabla TR_EMPRESA_SP7
        /// </summary>
        public void UpdateTrEmpresaSp7(TrEmpresaSp7DTO entity)
        {
            try
            {
                FactoryScada.GetTrEmpresaSp7Repository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TR_EMPRESA_SP7
        /// </summary>
        public void DeleteTrEmpresaSp7()
        {
            try
            {
                FactoryScada.GetTrEmpresaSp7Repository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }      

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_EMPRESA_SP7
        /// </summary>
        public List<TrEmpresaSp7DTO> ListTrEmpresaSp7s()
        {
            var lista = FactoryScada.GetTrEmpresaSp7Repository().List();
            foreach (var reg in lista)
            {
                reg.Emprenomb = reg.Emprenomb != null ? reg.Emprenomb.Trim() : string.Empty;
            }
            return lista;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrEmpresaSp7
        /// </summary>
        public List<TrEmpresaSp7DTO> GetByCriteriaTrEmpresaSp7s()
        {
            return FactoryScada.GetTrEmpresaSp7Repository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla ME_ENVIO

        /// <summary>
        /// Permite obtener un registro de la tabla ME_ENVIO
        /// </summary>
        public MeEnvioDTO GetByIdMeEnvio(int idEnvio)
        {
            return FactorySic.GetMeEnvioRepository().GetById(idEnvio);
        }

        /// <summary>
        /// Inserta un registro de la tabla ME_ENVIO
        /// </summary>
        public int SaveMeEnvio(MeEnvioDTO entity)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void UpdateMeEnvioDesc(MeEnvioDTO reg)
        {
            try
            {
                FactorySic.GetMeEnvioRepository().Update3(reg);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Mejoras IEOD
        /// <summary>
        /// Permite listar todos los registros de la tabla TR_EMPRESA_SP7
        /// </summary>
        public List<TrEmpresaSp7DTO> ListarEmpresaCanal()
        {
            var lista = FactoryScada.GetTrEmpresaSp7Repository().ListarEmpresaCanal();
            foreach (var reg in lista)
            {
                reg.Emprenomb = reg.Emprenomb != null ? reg.Emprenomb.Trim() : string.Empty;
            }
            return lista;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TR_EMPRESA_SP7 de la bd TREAL
        /// </summary>
        public List<TrEmpresaSp7DTO> ListarEmpresaCanalBdTreal()
        {
            //lista BD SICOES
            List<SiEmpresaDTO> listaEmp = FactorySic.GetSiEmpresaRepository().ListarEmpresaScada();

            //lista BD TREAL
            var lista = FactoryScada.GetTrEmpresaSp7BdTrealRepository().ListarEmpresaCanalBdTreal();

            foreach (var reg in lista)
            {
                var objEmpSicoes = listaEmp.Find(x => x.Scadacodi == reg.Emprcodi);
                if (objEmpSicoes != null) reg.Emprenomb = objEmpSicoes.Emprnomb;

                reg.Emprenomb = reg.Emprenomb != null ? reg.Emprenomb.Trim() : string.Empty;
            }

            return lista.OrderBy(x => x.Emprenomb).ToList();
        }

        public List<TrCanalSp7DTO> ListarUnidadPorZona(int zonacodi)
        {
            return FactoryScada.GetTrCanalSp7BdTrealRepository().ListarUnidadPorZona(zonacodi);
        }

        #endregion

        #region FIT - SGOCOES func A - Web Service - Add In
        public List<FLecturaSp7DTO> ObtenerConsultaTablaFrecuencia(bool zeroH, DateTime fechaInicio, DateTime fechaFin, int gpscodi)
        {
            return FactoryScada.GetFLecturaSp7Repository().ObtenerConsultaTablaFrecuencia(zeroH, fechaInicio, fechaFin, gpscodi);
        }
        #endregion

        #region REQ 2023-000106 Reporte Tiempo real para Estadística 

        /// <summary>
        /// Generación masiva de archivos
        /// </summary>
        /// <param name="enviocodi"></param>
        /// <param name="anio"></param>
        /// <param name="ruta"></param>
        /// <param name="tipoArchivo"></param>
        /// <param name="listaEquiposCorrectos"></param>
        public void GenerarArchivoEstadisticaTiempoReal(int enviocodi, int anio, string ruta, int tipoArchivo, List<TrEstadisticaEquipo> listaEquiposCorrectos)
        {
            //eliminar carpeta
            string carpetaTrabajo = ruta + ConstantesTiempoReal.FolderEstadistica;
            FileServer.DeleteFolderAlter("", carpetaTrabajo);
            FileServer.CreateFolder("", ConstantesTiempoReal.FolderEstadistica, ruta);

            //parametros
            DateTime fechaIni = new DateTime(anio, 1, 1);
            DateTime fechaFin = fechaIni.AddYears(1).AddDays(-1);

            int numArchivos = listaEquiposCorrectos.Count();
            decimal porcentaje = 1.0m / (numArchivos + 0.0m);
            decimal avanceActual = 0;

            MeEnvioDTO reg = GetByIdMeEnvio(enviocodi);

            foreach (var objEquipo in listaEquiposCorrectos)
            {
                //obtener data
                string canalcodis = "";
                if (ConstantesTiempoReal.TipoArchivoLineaTrafo == tipoArchivo) canalcodis = string.Format("{0},{1},{2},{3}", objEquipo.CanalcodiMW, objEquipo.CanalcodiMVar, objEquipo.SCanalcodiKV, objEquipo.CanalcodiA);
                if (ConstantesTiempoReal.TipoArchivoBarra == tipoArchivo) canalcodis = string.Format("{0}", objEquipo.CanalcodiKV);
                List<MeScadaSp7DTO> listaData = GetByCriteriaMeScadaSp7s(fechaIni, fechaFin, canalcodis);

                //guardar archivo
                GenerarExcelArchivoEstadisticaEquipo(carpetaTrabajo, tipoArchivo, objEquipo, fechaIni, fechaFin, listaData);

                //actualizar avance
                avanceActual += porcentaje;
                reg.Enviodesc = MathHelper.Round(avanceActual * 100, 0).ToString();
                UpdateMeEnvioDesc(reg);
            }

            //setear porcentaje al 100%
            reg.Enviodesc = "100";
            UpdateMeEnvioDesc(reg);
        }

        private void GenerarExcelArchivoEstadisticaEquipo(string ruta, int tipoArchivo, TrEstadisticaEquipo objEquipo,
                                            DateTime fechaIni, DateTime fechaFin, List<MeScadaSp7DTO> listaData)
        {
            string archivo = string.Format("{0}_{1}.xlsx", objEquipo.Nro, objEquipo.Equinomb);
            string rutaFile = ruta + archivo;
            string nameWs = string.Format("{0}_{1}", objEquipo.Nro, objEquipo.Equinomb);
            if (nameWs.Length > 30) nameWs = nameWs.Substring(0, 30);

            FileInfo newFile = new FileInfo(rutaFile);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaFile);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                if (ConstantesTiempoReal.TipoArchivoLineaTrafo == tipoArchivo)
                {
                    GenerarHojaExcelDetalleEquipoLinea(xlPackage, nameWs, objEquipo, fechaIni, fechaFin, listaData);
                    xlPackage.Save();
                }
                else
                {
                    GenerarHojaExcelDetalleEquipoBarra(xlPackage, nameWs, objEquipo, fechaIni, fechaFin, listaData);
                    xlPackage.Save();
                }
            }
        }

        private void GenerarHojaExcelDetalleEquipoLinea(ExcelPackage xlPackage, string nameWS, TrEstadisticaEquipo objEquipo,
                                            DateTime fechaIni, DateTime fechaFin, List<MeScadaSp7DTO> listaData)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            #region  Filtros y Cabecera

            int rowTitulo = 2;
            int rowCodigo = rowTitulo + 3;
            int rowUnidad = rowCodigo + 1;
            int colFecha = 2;
            int colMW = colFecha + 1;
            int colMvar = colMW + 1;
            int colKV = colMvar + 1;
            int colA = colKV + 1;

            ws.Cells[rowTitulo, colFecha].Value = "Equipo";
            ws.Cells[rowTitulo, colFecha + 1].Value = objEquipo.Equinomb;

            ws.Cells[rowCodigo, colFecha].Value = "Codigo ICCP";
            ws.Cells[rowCodigo, colMW].Value = objEquipo.CanalcodiMW;
            ws.Cells[rowCodigo, colMvar].Value = objEquipo.CanalcodiMVar;
            ws.Cells[rowCodigo, colKV].Value = objEquipo.CanalcodiKV;
            ws.Cells[rowCodigo, colA].Value = objEquipo.CanalcodiA;
            ws.Cells[rowUnidad, colFecha].Value = "Fecha / Hora";
            ws.Cells[rowUnidad, colMW].Value = "MW";
            ws.Cells[rowUnidad, colMvar].Value = "MVAR";
            ws.Cells[rowUnidad, colKV].Value = "KV";
            ws.Cells[rowUnidad, colA].Value = "A";

            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowCodigo, colFecha, rowUnidad, colA, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowCodigo, colFecha, rowUnidad, colA, "Centro");

            ws.Column(colFecha).Width = 16;
            ws.Column(colMW).Width = 9;
            ws.Column(colMvar).Width = 9;
            ws.Column(colKV).Width = 9;
            ws.Column(colA).Width = 9;

            #endregion

            #region Cuerpo

            int rowData = rowUnidad + 1;

            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                var listaXDia = listaData.Where(x => x.Medifecha == fecha).ToList();
                var objMW = listaXDia.Find(x => x.Canalcodi == objEquipo.CanalcodiMW) ?? new MeScadaSp7DTO();
                var objMVar = listaXDia.Find(x => x.Canalcodi == objEquipo.CanalcodiMVar) ?? new MeScadaSp7DTO();
                var objKV = listaXDia.Find(x => x.Canalcodi == objEquipo.CanalcodiKV) ?? new MeScadaSp7DTO();
                var objA = listaXDia.Find(x => x.Canalcodi == objEquipo.CanalcodiA) ?? new MeScadaSp7DTO();

                for (int h = 1; h <= 96; h++)
                {
                    ws.Cells[rowData, colFecha].Value = fecha.AddMinutes(h * 15).ToString(ConstantesAppServicio.FormatoFechaFull);

                    var valorMW = (decimal?)objMW.GetType().GetProperty("H" + h).GetValue(objMW, null);
                    var valorMvar = (decimal?)objMVar.GetType().GetProperty("H" + h).GetValue(objMVar, null);
                    var valorKv = (decimal?)objKV.GetType().GetProperty("H" + h).GetValue(objKV, null);
                    var valorA = (decimal?)objA.GetType().GetProperty("H" + h).GetValue(objA, null);

                    ws.Cells[rowData, colMW].Value = valorMW;
                    ws.Cells[rowData, colMvar].Value = valorMvar;
                    ws.Cells[rowData, colKV].Value = valorKv;
                    ws.Cells[rowData, colA].Value = valorA;

                    rowData++;
                }
            }

            #endregion

            ws.View.FreezePanes(rowUnidad + 1, colFecha + 1);
        }

        private void GenerarHojaExcelDetalleEquipoBarra(ExcelPackage xlPackage, string nameWS, TrEstadisticaEquipo objEquipo,
                                            DateTime fechaIni, DateTime fechaFin, List<MeScadaSp7DTO> listaData)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];

            #region  Filtros y Cabecera

            int rowTitulo = 2;
            int rowCodigo = rowTitulo + 3;
            int rowUnidad = rowCodigo + 1;
            int colFecha = 2;
            int colKV = colFecha + 1;

            ws.Cells[rowTitulo, colFecha].Value = "Equipo";
            ws.Cells[rowTitulo, colFecha + 1].Value = objEquipo.Equinomb;

            ws.Cells[rowCodigo, colFecha].Value = "Codigo ICCP";
            ws.Cells[rowCodigo, colKV].Value = objEquipo.CanalcodiKV;
            ws.Cells[rowUnidad, colFecha].Value = "Fecha / Hora";
            ws.Cells[rowUnidad, colKV].Value = "KV";

            UtilExcel.CeldasExcelAlinearVerticalmente(ws, rowCodigo, colFecha, rowUnidad, colKV, "Centro");
            UtilExcel.CeldasExcelAlinearHorizontalmente(ws, rowCodigo, colFecha, rowUnidad, colKV, "Centro");

            ws.Column(colFecha).Width = 16;
            ws.Column(colKV).Width = 9;

            #endregion

            #region Cuerpo

            int rowData = rowUnidad + 1;

            for (DateTime fecha = fechaIni; fecha <= fechaFin; fecha = fecha.AddDays(1))
            {
                var listaXDia = listaData.Where(x => x.Medifecha == fecha).ToList();
                var objKV = listaXDia.Find(x => x.Canalcodi == objEquipo.CanalcodiKV) ?? new MeScadaSp7DTO();

                for (int h = 1; h <= 96; h++)
                {
                    ws.Cells[rowData, colFecha].Value = fecha.AddMinutes(h * 15).ToString(ConstantesAppServicio.FormatoFechaFull);

                    var valorKv = (decimal?)objKV.GetType().GetProperty("H" + h).GetValue(objKV, null);

                    ws.Cells[rowData, colKV].Value = valorKv;

                    rowData++;
                }
            }

            #endregion

            ws.View.FreezePanes(rowUnidad + 1, colFecha + 1);
        }

        /// <summary>
        /// Generar archivos en archivo rar
        /// </summary>
        /// <param name="rutaCarpeta"></param>
        /// <param name="anio"></param>
        /// <param name="nombreFile"></param>
        public void GenerarArchivosSalidaProcesoArchivoEstadisticaZip(string rutaCarpeta, int anio, out string nombreFile)
        {
            try
            {
                nombreFile = "ESTADISTICA_TIEMPO_REAL_" + anio + ".zip";
                var rutaZip = rutaCarpeta + nombreFile;

                if (File.Exists(rutaZip)) File.Delete(rutaZip);
                FileServer.CreateZipFromDirectory(ConstantesTiempoReal.FolderEstadistica, rutaZip, rutaCarpeta);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #region Verificar procesamiento masivo

        /// <summary>
        /// GetNuevoEnvioProcesoArchivoEstadistica
        /// </summary>
        /// <param name="fecha1Mes"></param>
        /// <param name="fechaInicioProceso"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int GetNuevoEnvioProcesoArchivoEstadistica(DateTime fecha1Mes, DateTime fechaInicioProceso, string usuario)
        {
            MeEnvioDTO meEnvioDTO = new MeEnvioDTO();
            meEnvioDTO.Enviofecha = fechaInicioProceso;
            meEnvioDTO.Envioplazo = "P";
            meEnvioDTO.Userlogin = usuario;
            meEnvioDTO.Lastuser = usuario;
            meEnvioDTO.Emprcodi = 1; //COES
            meEnvioDTO.Enviofechaperiodo = fecha1Mes;
            meEnvioDTO.Formatcodi = ConstantesTiempoReal.FormatoProcesoArchivoEstadistica;
            meEnvioDTO.Enviofechaini = fecha1Mes;
            meEnvioDTO.Enviofechafin = fecha1Mes.AddMonths(1).AddDays(-1);
            meEnvioDTO.Estenvcodi = ParametrosEnvio.EnvioAprobado;
            meEnvioDTO.Enviodesc = "0";

            return SaveMeEnvio(meEnvioDTO);
        }

        /// <summary>
        /// NoExisteProcesoProcesoArchivoEstadisticaEnProceso
        /// </summary>
        /// <returns></returns>
        public int NoExisteProcesoProcesoArchivoEstadisticaEnProceso()
        {
            int resultado;
            MeEnvioDTO version = GetUltimoEnvioProcesoArchivoEstadistica(null);

            if (version != null)
            {
                decimal valorPorcentaje = 0;
                if (string.IsNullOrEmpty(version.Enviodesc)) version.Enviodesc = "100";
                decimal.TryParse(version.Enviodesc ?? "", out valorPorcentaje);

                if (valorPorcentaje != 100 && valorPorcentaje != -1)
                {
                    resultado = 0;
                }
                else
                {
                    resultado = 1;
                }
            }
            else
            {
                resultado = 1;
            }

            return resultado;
        }

        /// <summary>
        /// GetUltimoEnvioProcesoArchivoEstadistica
        /// </summary>
        /// <param name="fecha1Mes"></param>
        /// <returns></returns>
        public MeEnvioDTO GetUltimoEnvioProcesoArchivoEstadistica(DateTime? fecha1Mes)
        {
            DateTime f1 = fecha1Mes != null ? fecha1Mes.Value : DateTime.Today.AddYears(-3);
            DateTime f2 = fecha1Mes != null ? fecha1Mes.Value : DateTime.Today.AddYears(1);

            MeEnvioDTO version = (new FormatoMedicionAppServicio()).GetListaMultipleMeEnviosXLS("1", ConstantesAppServicio.ParametroDefecto, ConstantesTiempoReal.FormatoProcesoArchivoEstadistica.ToString()
                                                                                , ConstantesAppServicio.ParametroDefecto, f1, f2)
                                                                            .OrderByDescending(x => x.Enviocodi).FirstOrDefault();
            if (version != null)
            {
                version.EnviofechaDesc = version.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);
            }
            return version;
        }

        /// <summary>
        ///  Al iniciar el servidor de la Intranet buscará si una generación quedo pendiente de terminar
        /// </summary>
        public void VerificarUltimaProcesoArchivoEstadistica()
        {
            try
            {
                MeEnvioDTO version = GetUltimoEnvioProcesoArchivoEstadistica(null);

                if (version != null)
                {
                    decimal valorPorcentaje = 0;
                    if (string.IsNullOrEmpty(version.Enviodesc)) version.Enviodesc = "100";
                    decimal.TryParse(version.Enviodesc ?? "", out valorPorcentaje);

                    if (valorPorcentaje != 100 && valorPorcentaje != -1)
                    {
                        string msj = "Ocurrió un error cuando se realizaba el proceso, se terminó cuando estaba al " + version.Enviodesc + "% al detenerse el servidor.\nEl servidor inició nuevamente a las " + DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);

                        int enviocodi = version.Enviocodi;
                        version.Enviodesc = "-1";
                        UpdateMeEnvioDesc(version);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        #endregion

        #region Importación de códigos

        /// <summary>
        /// Validar Archivo Estadistica Codigo SP7
        /// </summary>
        /// <param name="tipoArchivo"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="listaEquiposCorrectos"></param>
        /// <param name="listaEquiposErroneos"></param>
        /// <param name="listaObservaciones"></param>
        public void ValidarArchivoEstadisticaCodigoSP7(int tipoArchivo, string path, string fileName,
                                               out List<TrEstadisticaEquipo> listaEquiposCorrectos,
                                               out List<TrEstadisticaEquipo> listaEquiposErroneos,
                                               out List<TrEstadisticaLog> listaObservaciones)
        {
            listaEquiposCorrectos = new List<TrEstadisticaEquipo>();
            listaEquiposErroneos = new List<TrEstadisticaEquipo>();
            listaObservaciones = new List<TrEstadisticaLog>();

            //Validación de archivo
            string extension = (System.IO.Path.GetExtension(fileName) ?? "").ToLower();

            List<string> lExtensionPermitido = new List<string>() { ".xlsx" };
            if (!lExtensionPermitido.Contains(extension))
            {
                listaObservaciones.Add(new TrEstadisticaLog()
                {
                    Observacion = "Está cargando un archivo de extensión no permitida. Debe ingresar un archivo " + string.Join(", ", lExtensionPermitido)
                });
            }
            else
            {
                List<TrCanalSp7DTO> listaCanalSp7 = ListTrCanalSp7s();

                //filas excel
                List<TrEstadisticaEquipo> listaFilaMacro = new List<TrEstadisticaEquipo>();
                if (ConstantesTiempoReal.TipoArchivoLineaTrafo == tipoArchivo) listaFilaMacro = ImportArchivoLineaToDataTable(path + fileName);
                if (ConstantesTiempoReal.TipoArchivoBarra == tipoArchivo) listaFilaMacro = ImportArchivoBarraToDataTable(path + fileName);

                foreach (var regFila in listaFilaMacro)
                {
                    //Validaciones al leer la macro (comprobar si los datos del excel son validos)
                    string mensajeValidacion = "";
                    if (ConstantesTiempoReal.TipoArchivoLineaTrafo == tipoArchivo) mensajeValidacion = this.ValidarLecturaExcelLineaCodigoSP7(regFila, listaCanalSp7);
                    if (ConstantesTiempoReal.TipoArchivoBarra == tipoArchivo) mensajeValidacion = this.ValidarLecturaExcelBarraCodigoSP7(regFila, listaCanalSp7);

                    regFila.EsFilaCorrecta = mensajeValidacion == "";
                    if (!regFila.EsFilaCorrecta)
                    {
                        listaObservaciones.Add(new TrEstadisticaLog()
                        {
                            Fila = regFila.Row,
                            Observacion = mensajeValidacion
                        });
                    }
                }

                listaEquiposCorrectos = listaFilaMacro.Where(x => x.EsFilaCorrecta).ToList();
                listaEquiposErroneos = listaFilaMacro.Where(x => !x.EsFilaCorrecta).ToList();

                //si no existen observaciones puede ser que el archivo no tenga datos
                if (!listaFilaMacro.Any())
                {
                    listaObservaciones.Add(new TrEstadisticaLog()
                    {
                        Observacion = "El documento no tiene registros."
                    });
                }
            }
        }

        private string ValidarLecturaExcelLineaCodigoSP7(TrEstadisticaEquipo filaExcel, List<TrCanalSp7DTO> listaCanalSp7)
        {
            string columnNro = "N°: ";
            string columnEquipo = "Equipo: ";
            string columnMW = "MW: ";
            string columnMVAR = "MVAR: ";
            string columnKV = "KV: ";
            string columnA = "A: ";

            List<string> lMsgValidacion = new List<string>();

            // Validar Nombre propiedad
            if (String.IsNullOrEmpty(filaExcel.SNro))
            {
                lMsgValidacion.Add(columnNro + "Esta vacío o en blanco");
            }

            if (String.IsNullOrEmpty(filaExcel.SEquinomb))
            {
                lMsgValidacion.Add(columnEquipo + "Esta vacío o en blanco");
            }

            lMsgValidacion.Add(GetMensajeColumnaCanal(columnMW, filaExcel.SCanalcodiMW, filaExcel.CanalcodiMW, listaCanalSp7));
            lMsgValidacion.Add(GetMensajeColumnaCanal(columnMVAR, filaExcel.SCanalcodiMVar, filaExcel.CanalcodiMVar, listaCanalSp7));
            lMsgValidacion.Add(GetMensajeColumnaCanal(columnKV, filaExcel.SCanalcodiKV, filaExcel.CanalcodiKV, listaCanalSp7));
            lMsgValidacion.Add(GetMensajeColumnaCanal(columnA, filaExcel.SCanalcodiA, filaExcel.CanalcodiA, listaCanalSp7));

            return string.Join(". ", lMsgValidacion.Where(x => x != ""));
        }

        private string ValidarLecturaExcelBarraCodigoSP7(TrEstadisticaEquipo filaExcel, List<TrCanalSp7DTO> listaCanalSp7)
        {
            string columnNro = "N°: ";
            string columnEquipo = "Equipo: ";
            string columnKV = "KV: ";

            List<string> lMsgValidacion = new List<string>();

            // Validar Nombre propiedad
            if (String.IsNullOrEmpty(filaExcel.SNro))
            {
                lMsgValidacion.Add(columnNro + "Esta vacío o en blanco");
            }

            if (String.IsNullOrEmpty(filaExcel.SEquinomb))
            {
                lMsgValidacion.Add(columnEquipo + "Esta vacío o en blanco");
            }

            lMsgValidacion.Add(GetMensajeColumnaCanal(columnKV, filaExcel.SCanalcodiKV, filaExcel.CanalcodiKV, listaCanalSp7));

            return string.Join(". ", lMsgValidacion.Where(x => x != ""));
        }

        private string GetMensajeColumnaCanal(string nombreCol, string celda, int canalcodi, List<TrCanalSp7DTO> listaCanalSp7)
        {
            if (String.IsNullOrEmpty(celda))
            {
                return nombreCol + "Esta vacío o en blanco";
            }
            else if (canalcodi <= 0)
            {
                return nombreCol + "No es número válido";
            }
            else
            {
                var obj = listaCanalSp7.Find(x => x.Canalcodi == canalcodi);
                if (obj == null)
                {
                    return nombreCol + string.Format("Código de canal SP7 {0} no existe", canalcodi);
                }
            }

            return "";
        }

        private static List<TrEstadisticaEquipo> ImportArchivoLineaToDataTable(string filePath)
        {
            List<TrEstadisticaEquipo> listaMacro = new List<TrEstadisticaEquipo>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexNro = 2;
            int indexNombreEquipo = 3;
            int indexCodigoMW = 11;
            int indexCodigMVar = 12;
            int indexCodigoKV = 13;
            int indexCodigoA = 14;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 3; //excel Linea Transformadores

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var sNro = string.Empty;
                    if (worksheet.Cells[row, indexNro].Value != null) sNro = worksheet.Cells[row, indexNro].Value.ToString();

                    var sNombreEquipo = string.Empty;
                    if (worksheet.Cells[row, indexNombreEquipo].Value != null) sNombreEquipo = worksheet.Cells[row, indexNombreEquipo].Value.ToString();

                    if (string.IsNullOrEmpty(sNro) && string.IsNullOrEmpty(sNombreEquipo))
                    {
                        continue;
                    }

                    var sCodigoMW = string.Empty;
                    if (worksheet.Cells[row, indexCodigoMW].Value != null) sCodigoMW = worksheet.Cells[row, indexCodigoMW].Value.ToString();

                    var sCodigMVar = string.Empty;
                    if (worksheet.Cells[row, indexCodigMVar].Value != null) sCodigMVar = worksheet.Cells[row, indexCodigMVar].Value.ToString();

                    var sCodigoKV = string.Empty;
                    if (worksheet.Cells[row, indexCodigoKV].Value != null) sCodigoKV = worksheet.Cells[row, indexCodigoKV].Value.ToString();

                    var sCodigoA = string.Empty;
                    if (worksheet.Cells[row, indexCodigoA].Value != null) sCodigoA = worksheet.Cells[row, indexCodigoA].Value.ToString();


                    int nro = 0;
                    string equinomb = "";
                    string tension = "";
                    int canalcodiMW = 0;
                    int canalcodiMVar = 0;
                    int canalcodiKV = 0;
                    int canalcodiA = 0;
                    try
                    {
                        sNro = (sNro ?? "").Trim();
                        equinomb = (sNombreEquipo ?? "").Trim();
                        sCodigoMW = (sCodigoMW ?? "").Trim();
                        sCodigMVar = (sCodigMVar ?? "").Trim();
                        sCodigoKV = (sCodigoKV ?? "").Trim();
                        sCodigoA = (sCodigoA ?? "").Trim();

                        Int32.TryParse(sNro, out nro);
                        Int32.TryParse(sCodigoMW, out canalcodiMW);
                        Int32.TryParse(sCodigMVar, out canalcodiMVar);
                        Int32.TryParse(sCodigoKV, out canalcodiKV);
                        Int32.TryParse(sCodigoA, out canalcodiA);
                    }
                    catch (Exception ex)
                    {
                        //No es necesario registrar el error de la conversión de datos de todas las celdas (pueden ocurrir como un máximo de 28 mil líneas de log para archivos de 7000 líneas de datos).
                    }

                    var regFila = new TrEstadisticaEquipo()
                    {
                        Row = row,
                        SNro = sNro,
                        SEquinomb = sNombreEquipo,
                        SCanalcodiMW = sCodigoMW,
                        SCanalcodiMVar = sCodigMVar,
                        SCanalcodiKV = sCodigoKV,
                        SCanalcodiA = sCodigoA,

                        Nro = nro,
                        Equinomb = equinomb,
                        Tension = tension,
                        CanalcodiMW = canalcodiMW,
                        CanalcodiMVar = canalcodiMVar,
                        CanalcodiKV = canalcodiKV,
                        CanalcodiA = canalcodiA,
                    };

                    listaMacro.Add(regFila);
                }
            }

            return listaMacro;
        }

        private static List<TrEstadisticaEquipo> ImportArchivoBarraToDataTable(string filePath)
        {
            List<TrEstadisticaEquipo> listaMacro = new List<TrEstadisticaEquipo>();

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexNro = 2;
            int indexNombreEquipo = 3;
            int indexCodigoKV = 5;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.First();

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowStart = 3; //excel Linea Transformadores

                // place all the data into DataTable
                for (int row = rowStart; row <= endCell.Row; row++)
                {
                    var sNro = string.Empty;
                    if (worksheet.Cells[row, indexNro].Value != null) sNro = worksheet.Cells[row, indexNro].Value.ToString();

                    var sNombreEquipo = string.Empty;
                    if (worksheet.Cells[row, indexNombreEquipo].Value != null) sNombreEquipo = worksheet.Cells[row, indexNombreEquipo].Value.ToString();

                    if (string.IsNullOrEmpty(sNro) && string.IsNullOrEmpty(sNombreEquipo))
                    {
                        continue;
                    }

                    var sCodigoKV = string.Empty;
                    if (worksheet.Cells[row, indexCodigoKV].Value != null) sCodigoKV = worksheet.Cells[row, indexCodigoKV].Value.ToString();

                    int nro = 0;
                    string equinomb = "";
                    int canalcodiKV = 0;
                    try
                    {
                        sNro = (sNro ?? "").Trim();
                        equinomb = (sNombreEquipo ?? "").Trim();
                        sCodigoKV = (sCodigoKV ?? "").Trim();

                        Int32.TryParse(sNro, out nro);
                        Int32.TryParse(sCodigoKV, out canalcodiKV);
                    }
                    catch (Exception ex)
                    {
                        //No es necesario registrar el error de la conversión de datos de todas las celdas (pueden ocurrir como un máximo de 28 mil líneas de log para archivos de 7000 líneas de datos).
                    }

                    var regFila = new TrEstadisticaEquipo()
                    {
                        Row = row,
                        SNro = sNro,
                        SEquinomb = sNombreEquipo,
                        SCanalcodiKV = sCodigoKV,

                        Nro = nro,
                        Equinomb = equinomb,
                        CanalcodiKV = canalcodiKV,
                    };

                    listaMacro.Add(regFila);
                }
            }

            return listaMacro;
        }

        /// <summary>
        /// Archivo Excel de Log de errores
        /// </summary>
        /// <param name="pathLogo"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="fechaLog"></param>
        /// <param name="usuario"></param>
        /// <param name="listaLogFilas"></param>
        public void GenerarExcelLogErrores(string pathLogo, string rutaArchivo, DateTime fechaLog, string usuario, List<TrEstadisticaLog> listaLogFilas)
        {
            FileInfo newFile = new FileInfo(rutaArchivo);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                this.GenerarHojaExcelLog(xlPackage, pathLogo, "Observaciones", "LOG DE ERRORES", usuario, fechaLog, listaLogFilas);
                xlPackage.Save();
            }
        }

        private void GenerarHojaExcelLog(ExcelPackage xlPackage, string pathLogo, string nameWS, string nombreFormato, string usuario, DateTime fechaConsulta, List<TrEstadisticaLog> listaLog)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(nameWS);
            ws = xlPackage.Workbook.Worksheets[nameWS];
            //ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Log de Errores");

            //Logo
            UtilExcel.AddImageLocal(ws, 1, 0, pathLogo);

            //Armando tabla de identificador de usuario y fecha de modificación
            int row = 4;
            ws.Cells[row, 1].Value = nombreFormato;
            UtilExcel.CeldasExcelAgrupar(ws, row, 1, row, 2);
            UtilExcel.SetFormatoCelda(ws, row, 1, row, 2, "Centro", "Centro", "#FFFFFF", "#227ABE", "Calibri", 10, true);

            ws.Cells[++row, 1].Value = "Usuario: " + usuario;
            ws.Cells[++row, 1].Value = "Fecha y Hora: " + fechaConsulta.ToString(ConstantesAppServicio.FormatoFechaFull2);

            int rowCab = 8;
            ws.Cells[rowCab, 1].Value = "Fila";
            ws.Cells[rowCab, 2].Value = "Observación";

            UtilExcel.SetFormatoCelda(ws, rowCab, 1, rowCab, 2, "Centro", "Centro", "#FFFFFF", "#227ABE", "Calibri", 10, true);

            row = rowCab + 1;
            foreach (var item in listaLog)
            {
                ws.Cells[row, 1].Value = item.Fila;
                ws.Cells[row, 2].Value = item.Observacion;
                UtilExcel.CeldasExcelAlinearHorizontalmente(ws, row, 1, row, 2, "Centro");

                row++;
            }

            int rowFin = row > rowCab + 1 ? row - 1 : row;

            ws.Column(1).Width = 10; //Linea
            ws.Column(2).Width = 70; //Observación

            ws.View.FreezePanes(8 + 1, 1 + 1);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Clase que representa a la fila del archivo excel importado
    /// </summary>
    public class TrEstadisticaEquipo 
    {
        public int Nro { get; set; }
        public string Equinomb { get; set; }
        public string Tension { get; set; }
        public int CanalcodiMW { get; set; }
        public int CanalcodiMVar { get; set; }
        public int CanalcodiKV { get; set; }
        public int CanalcodiA { get; set; }

        public int Row { get; set; }
        public string SNro { get; set; }
        public string SEquinomb { get; set; }
        public string STension { get; set; }
        public string SCanalcodiMW { get; set; }
        public string SCanalcodiMVar { get; set; }
        public string SCanalcodiKV { get; set; }
        public string SCanalcodiA { get; set; }

        public bool EsFilaCorrecta { get; set; }
    }

    public class TrEstadisticaLog 
    {
        public int? Fila { get; set; }
        public string Observacion { get; set; }
    }
}
