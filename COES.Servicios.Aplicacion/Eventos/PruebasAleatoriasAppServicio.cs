using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.PruebasAleatorias
{
    /// <summary>
    /// Clases con métodos del módulo PruebasAleatorias
    /// </summary>
    public class PruebasAleatoriasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PruebasAleatoriasAppServicio));

        #region Métodos Tabla EVE_PALEATORIA

        /// <summary>
        /// Inserta un registro de la tabla EVE_PALEATORIA
        /// </summary>
        public void SaveEvePaleatoria(EvePaleatoriaDTO entity)
        {
            try
            {
                FactorySic.GetEvePaleatoriaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla EVE_PALEATORIA
        /// </summary>
        public void UpdateEvePaleatoria(EvePaleatoriaDTO entity)
        {
            try
            {
                FactorySic.GetEvePaleatoriaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla EVE_PALEATORIA
        /// </summary>
        public void Eliminar(DateTime pafecha)
        {
            try
            {
                FactorySic.GetEvePaleatoriaRepository().Delete(pafecha);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla EVE_PALEATORIA
        /// </summary>
        public EvePaleatoriaDTO GetByIdEvePaleatoria(DateTime pafecha)
        {
            return FactorySic.GetEvePaleatoriaRepository().GetById(pafecha);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla EVE_PALEATORIA
        /// </summary>
        public List<EvePaleatoriaDTO> ListEvePaleatorias()
        {
            return FactorySic.GetEvePaleatoriaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla EvePaleatoria
        /// </summary>
        public List<EvePaleatoriaDTO> GetByCriteriaEvePaleatorias()
        {
            return FactorySic.GetEvePaleatoriaRepository().GetByCriteria();
        }

        /// <summary>
        /// Permite listar el programador
        /// </summary>
        /// <returns></returns>
        public List<SiPersonaDTO> ListarProgramador()
        {
            return FactorySic.GetEvePaleatoriaRepository().ListProgramador();
        }

        /// <summary>
        /// Permite obtener el número de filas de las Pruebas aleatorias
        /// </summary>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public int ObtenerNroFilas(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEvePaleatoriaRepository().ObtenerNroFilas(fechaInicio, fechaFinal, nroPage, pageSize);
        }

        /// <summary>
        /// Permite buscar las operaciones de las Pruebas aleatorias
        /// </summary>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFinal">Fecha final</param>
        /// <param name="nroPage">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns></returns>
        public List<EvePaleatoriaDTO> BuscarOperaciones(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            return FactorySic.GetEvePaleatoriaRepository().BuscarOperaciones(fechaInicio, fechaFinal, nroPage, pageSize);
        }


        /// <summary>
        /// Obtiene el nombre del programador de las pruebas aleatorias
        /// </summary>
        /// <param name="fecha">Fecha de sorteo</param>
        /// <returns>Nombre del programador</returns>
        public string ProgramadorPrueba(DateTime fecha)
        {
            return FactorySic.GetEvePaleatoriaRepository().ProgramadorPrueba(fecha);
        }


        #endregion

    }
}
