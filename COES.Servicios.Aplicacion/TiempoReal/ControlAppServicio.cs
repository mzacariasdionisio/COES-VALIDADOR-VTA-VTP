using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.TiempoReal
{
    /// <summary>
    /// Clases con métodos del módulo AGC
    /// </summary>
    public class ControlAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ControlAppServicio));

        #region Métodos Tabla AGC_CONTROL

        /// <summary>
        /// Inserta un registro de la tabla AGC_CONTROL
        /// </summary>
        public void SaveAgcControl(AgcControlDTO entity)
        {
            try
            {
                FactorySic.GetAgcControlRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AGC_CONTROL
        /// </summary>
        public void UpdateAgcControl(AgcControlDTO entity)
        {
            try
            {
                FactorySic.GetAgcControlRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla AGC_CONTROL
        /// </summary>
        public void DeleteAgcControl(int agcccodi)
        {
            try
            {
                FactorySic.GetAgcControlRepository().Delete(agcccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla AGC_CONTROL
        /// </summary>
        public AgcControlDTO GetByIdAgcControl(int agcccodi)
        {
            return FactorySic.GetAgcControlRepository().GetById(agcccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AGC_CONTROL
        /// </summary>
        public List<AgcControlDTO> ListAgcControls(string estado, int nroPage, int pageSize)
        {
            return FactorySic.GetAgcControlRepository().List(estado, nroPage, pageSize);
        }



        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla AGC_CONTROL
        /// </summary>
        public int ObtenerNroFilas(string estado)
        {
            return FactorySic.GetAgcControlRepository().ObtenerNroFilas(estado);
        }



        /// <summary>
        /// Permite realizar búsquedas en la tabla AgcControl
        /// </summary>
        public List<AgcControlDTO> GetByCriteriaAgcControls()
        {
            return FactorySic.GetAgcControlRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla AGC_CONTROL
        /// </summary>
        public int SaveAgcControlId(AgcControlDTO entity)
        {
            return FactorySic.GetAgcControlRepository().SaveAgcControlId(entity);
        }               

        /// <summary>
        /// permite obtener los puntos de medición para el AGC
        /// </summary>       
        public List<MePtomedicionDTO> ListarPotencia(string familia, int idOriglectura, string control)
        {
            return FactorySic.GetMePtomedicionRepository().ListarPotencia(familia, idOriglectura, control);
        }
        
        /// <summary>
        /// permite obtener los datos de un puntos de medición para el AGC
        /// </summary>       
        public MePtomedicionDTO ListarPotenciaEquipo(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().ListarPotenciaEquipo(ptomedicodi);
        }
        
        /// <summary>
        /// permite obtener los puntos de costo variable para el AGC
        /// </summary>       
        public List<MePtomedicionDTO> ListarCostoVariableAGC()
        {
            return FactorySic.GetMePtomedicionRepository().ListarCostoVariableAGC();
        }

        /// <summary>
        /// permite obtener el tipo de grupo
        /// </summary>       
        public string ListarTipoGrupo(string puntoMedicion)
        {
            return FactorySic.GetMePtomedicionRepository().ListarTipoGrupo(puntoMedicion);
        }

        /// <summary>
        /// permite obtener datos referidos a un punto de costo variable para el AGC
        /// </summary>       
        public MePtomedicionDTO GetByIdAgc(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetByIdAgc(ptomedicodi);
        }

        /// <summary>
        /// permite obtener puntos de medición referidos al AGC
        /// </summary>       
        public List<MePtomedicionDTO> ListarControlCentralizado()
        {
            return FactorySic.GetMePtomedicionRepository().ListarControlCentralizado();
        }        

        #endregion


        #region Métodos Tabla AGC_CONTROL_PUNTO

        /// <summary>
        /// Inserta un registro de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public void SaveAgcControlPunto(AgcControlPuntoDTO entity)
        {
            try
            {
                FactorySic.GetAgcControlPuntoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public void UpdateAgcControlPunto(AgcControlPuntoDTO entity)
        {
            try
            {
                FactorySic.GetAgcControlPuntoRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los registro de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public void DeleteAgcControlPunto(int agcccodi)
        {
            try
            {
                FactorySic.GetAgcControlPuntoRepository().Delete(agcccodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener los registro de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public List<AgcControlPuntoDTO> GetByIdAgcControlPunto(int agcccodi)
        {
            return FactorySic.GetAgcControlPuntoRepository().GetById(agcccodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public List<AgcControlPuntoDTO> ListAgcControlPuntos()
        {
            return FactorySic.GetAgcControlPuntoRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla AgcControlPunto
        /// </summary>
        public List<AgcControlPuntoDTO> GetByCriteriaAgcControlPuntos()
        {
            return FactorySic.GetAgcControlPuntoRepository().GetByCriteria();
        }

        /// <summary>
        /// Graba los datos de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public int SaveAgcControlPuntoId(AgcControlPuntoDTO entity)
        {
            return FactorySic.GetAgcControlPuntoRepository().SaveAgcControlPuntoId(entity);
        }
        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public List<AgcControlPuntoDTO> BuscarOperaciones(int agccCodi, int ptomediCodi, int equiCodi, int nroPage, int pageSize)
        {
            return FactorySic.GetAgcControlPuntoRepository().BuscarOperaciones(agccCodi, ptomediCodi, equiCodi, nroPage, pageSize);
        }

        /// <summary>
        /// Obtiene el numero de filas de acuerdo a filtro de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public int ObtenerNroFilas(int agccCodi, int ptomediCodi, int equiCodi)
        {
            return FactorySic.GetAgcControlPuntoRepository().ObtenerNroFilas(agccCodi, ptomediCodi, equiCodi);
        }

        /// <summary>
        /// Obtiene las operaciones de acuerdo a grupo agccCodi de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public List<AgcControlPuntoDTO> ObtenerPorControl(int agccCodi)
        {
            return FactorySic.GetAgcControlPuntoRepository().ObtenerPorControl(agccCodi);
        }


        #endregion


        /// <summary>
        /// Busca las operaciones de acuerdo a filtro de la tabla PR_REPCV
        /// </summary>
        public List<PrRepcvDTO> ObtenerReporte(string repFechaIni, string repFechaFin)
        {
            return FactorySic.GetPrRepcvRepository().ObtenerReporte(repFechaIni, repFechaFin);
        }
    }
}
