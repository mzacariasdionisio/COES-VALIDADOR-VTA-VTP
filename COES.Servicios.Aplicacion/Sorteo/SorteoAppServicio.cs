using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Sorteo
{
    public class SorteoAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SorteoAppServicio));

        #region Métodos Tabla SI_SORTEO (Legal)

        /// <summary>
        /// Permite listar los registros de una empresa de la tabla 
        /// </summary>
        public List<SiListarAreasDTO> GetByListarAreas()
        {
            return FactorySic.GetSiListarAreasSorteoRepository().List();
        }

        public List<PrLogsorteoDTO> GetByIdPrLogsorteo(DateTime logfecha)
        {
            return FactorySic.GetPrLogsorteoRepository().GetByCriteria(logfecha);
        }

        /// <summary>
        /// Actualiza un registro de la tabla 
        /// </summary>
        public int InsertPrLogSorteo(PrLogsorteoDTO entity)
        {
            try
            {
                return FactorySic.GetPrLogsorteoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<PrLogsorteoDTO> GetByeq_equipo()
        {
            return FactorySic.GetPrLogsorteoRepository().eq_equipo();
        }

        public List<PrLogsorteoDTO> GetByeq_central()
        {
            return FactorySic.GetPrLogsorteoRepository().eq_central();
        }

        public List<PrLogsorteoDTO> GetByeve_mantto()
        {
            return FactorySic.GetPrLogsorteoRepository().eve_mantto();
        }

        public List<PrLogsorteoDTO> GetByeve_indisponibilidad()
        {
            return FactorySic.GetPrLogsorteoRepository().eve_indisponibilidad();
        }

        public List<PrLogsorteoDTO> GetByeve_horaoperacion()
        {
            return FactorySic.GetPrLogsorteoRepository().eve_horaoperacion();
        }

        public List<PrLogsorteoDTO> GetByeve_pruebaunidad()
        {
            return FactorySic.GetPrLogsorteoRepository().eve_pruebaunidad();
        }
        public List<Prequipos_validosDTO> GetByequipos_validos(int i_codigo)
        {
            return FactorySic.GetPrLogsorteoRepository().equipos_validos(i_codigo);
        }

        public List<Prequipos_validosDTO> GetByeve_mantto_calderos(int i_codigo, DateTime Today, DateTime tomorrow, DateTime mediodia)
        {
            return FactorySic.GetPrLogsorteoRepository().eve_mantto_calderos(i_codigo, Today, tomorrow, mediodia);
        }

        public int TotalConteoTipo(string tipo, DateTime logfecha)
        {
            return FactorySic.GetPrLogsorteoRepository().TotalConteoTipo(tipo, logfecha);
        }

        public bool eliminarLogSorteo(DateTime logfecha)
        {
            var delete = FactorySic.GetPrLogsorteoRepository().DeleteEquipo(logfecha);
            return delete;
        }

        public int InsertPrSorteo(int equicodi, DateTime fecha, string prueba)
        {
            try
            {
                int emprcodi = (FactorySic.GetEqEquipoRepository().GetById(equicodi).Emprcodi).GetValueOrDefault(-1);
                return FactorySic.GetPrLogsorteoRepository().InsertPrSorteo(equicodi, fecha, prueba, emprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public int TotalConteoTipoXEQ(DateTime logfecha)
        {
            return FactorySic.GetPrLogsorteoRepository().TotalConteoTipoXEQ(logfecha);
        }

        public int DiasFaltantes(DateTime logfecha)
        {
            return FactorySic.GetPrLogsorteoRepository().DiasFaltantes(logfecha);
        }
        
        #endregion
    }
}
