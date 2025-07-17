using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Servicios.Aplicacion.Siosein2;

namespace COES.Servicios.Aplicacion.IEOD
{
    /// <summary>
    /// Clases con métodos del módulo General
    /// </summary>
    public class AdministracionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AdministracionAppServicio));

        #region Métodos Tabla TRN_BARRA_AREA

        /// <summary>
        /// Inserta un registro de la tabla TRN_BARRA_AREA
        /// </summary>
        public void SaveTrnBarraArea(TrnBarraAreaDTO entity)
        {
            try
            {
                FactorySic.GetTrnBarraAreaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_BARRA_AREA
        /// </summary>
        public void UpdateTrnBarraArea(TrnBarraAreaDTO entity)
        {
            try
            {
                FactorySic.GetTrnBarraAreaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_BARRA_AREA
        /// </summary>
        public void DeleteTrnBarraArea(int bararecodi)
        {
            try
            {
                FactorySic.GetTrnBarraAreaRepository().Delete(bararecodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_BARRA_AREA
        /// </summary>
        public TrnBarraAreaDTO GetByIdTrnBarraArea(int bararecodi)
        {
            return FactorySic.GetTrnBarraAreaRepository().GetById(bararecodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_BARRA_AREA
        /// </summary>
        public List<TrnBarraAreaDTO> ListTrnBarraAreas()
        {
            return FactorySic.GetTrnBarraAreaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrnBarraArea
        /// </summary>
        public List<TrnBarraAreaDTO> GetByCriteriaTrnBarraAreas()
        {
            return FactorySic.GetTrnBarraAreaRepository().GetByCriteria();
        }

        #endregion

        
        public List<TrnBarraAreaDTO> ListarBarraAreas()
        {
            var Lista = this.ListTrnBarraAreas();
            foreach(var item in Lista)
            {
                item.sFechaCreacion = item.Bararefeccreacion?.ToString(ConstantesRestricc.FormatoFechaHora);
                item.Bararearea = (item.Bararearea == ConstantesIEOD.CharN) ? ConstantesIEOD.strNorte : (item.Bararearea == ConstantesIEOD.CharC) ? ConstantesIEOD.strCentro : ConstantesIEOD.strSur;
                item.Barareejecutiva = item.Barareejecutiva == ConstantesIEOD.CharS ? ConstantesIEOD.strSi : ConstantesIEOD.strNo;
            }
            return Lista;
        }

        
        public void GuardarDatosBarra(TrnBarraAreaDTO objBarra, int accion, string usuario)
        {
            TrnBarraAreaDTO objGuardar = new TrnBarraAreaDTO();
            TrnBarraAreaDTO objEditado = new TrnBarraAreaDTO();  
            if (accion == ConstantesIEOD.AccionNuevo)
            {
                objGuardar = objBarra;
                objGuardar.Barareusucreacion = usuario;
                objGuardar.Bararefeccreacion = DateTime.Now;
                objGuardar.Barareestado = "A";
                SaveTrnBarraArea(objGuardar);             
            }
            else
            {                
                objEditado = GetByIdTrnBarraArea(objBarra.Bararecodi);
                objGuardar = objBarra;
                objGuardar.Barareestado = objEditado.Barareestado;
                objGuardar.Barareusucreacion = objEditado.Barareusucreacion;
                objGuardar.Bararefeccreacion = objEditado.Bararefeccreacion;
                objGuardar.Barareusumodificacion = usuario;
                objGuardar.Bararefecmodificacion = DateTime.Now;
                UpdateTrnBarraArea(objGuardar);
            }
        }

        /// <summary>
        /// Permite obtener las agrupaciones de las barras
        /// </summary>
        /// <param name="zona"></param>
        /// <returns></returns>
        public List<IeeBarrazonaDTO> ObtenerAgrupacionBarraPorZona(int zona)
        {
            return FactorySic.GetIeeBarrazonaRepository().ObtenerAgrupacionPorZona(zona);
        }

        /// <summary>
        /// Permite obtener las barras que pertenecen a un grupo
        /// </summary>
        /// <param name="zona"></param>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        public List<IeeBarrazonaDTO> ObtenerBarrasPorAgrupacion(int zona, string agrupacion)
        {
            return FactorySic.GetIeeBarrazonaRepository().ObtenerBarrasPorAgrupacion(zona, agrupacion);
        }

        /// <summary>
        /// Permite eliminar una agrupación de barras
        /// </summary>
        /// <param name="zona"></param>
        /// <param name="agrupacion"></param>
        /// <returns></returns>
        public int EliminarAgrupacion(int zona, string agrupacion)
        {
            try
            {
                FactorySic.GetIeeBarrazonaRepository().EliminarAgrupacion(zona, agrupacion);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite almacenar la agrupación de barras
        /// </summary>
        /// <param name="zona"></param>
        /// <param name="agrupacion"></param>
        /// <param name="nombre"></param>
        /// <param name="barras"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GrabarAgrupacionBarra(int zona, string agrupacion, string nombre, string barras, string username)
        {
            try
            {
                List<int> ids = barras.Split(',').Select(int.Parse).ToList();
                DateTime fecha = DateTime.Now;

                if (string.IsNullOrEmpty(agrupacion))
                {
                    bool flagExistente = FactorySic.GetIeeBarrazonaRepository().ValidarExistencia(zona, nombre);

                    if (flagExistente)
                    {
                        return 2;
                    }
                    else
                    {
                        
                        foreach(int id in ids)
                        {
                            IeeBarrazonaDTO entity = new IeeBarrazonaDTO();
                            entity.Barrcodi = id;
                            entity.Barrzdesc = nombre;
                            entity.Mrepcodi = zona;
                            entity.Barrzarea = this.ObtenerBarrazPorZona(zona);
                            entity.Barrzusumodificacion = username;
                            entity.Barrzfecmodificacion = fecha;
                            FactorySic.GetIeeBarrazonaRepository().Save(entity);
                        }
                    }
                }
                else
                {
                    bool flagExistente = FactorySic.GetIeeBarrazonaRepository().ValidarExistenciaEdicion(zona, nombre, agrupacion);

                    if (flagExistente)
                    {
                        return 2;
                    }
                    else
                    {
                        FactorySic.GetIeeBarrazonaRepository().EliminarAgrupacion(zona, agrupacion);

                        foreach (int id in ids)
                        {
                            IeeBarrazonaDTO entity = new IeeBarrazonaDTO();
                            entity.Barrcodi = id;
                            entity.Barrzdesc = nombre;
                            entity.Mrepcodi = zona;
                            entity.Barrzarea = this.ObtenerBarrazPorZona(zona);
                            entity.Barrzusumodificacion = username;
                            entity.Barrzfecmodificacion = fecha;
                            FactorySic.GetIeeBarrazonaRepository().Save(entity);
                        }
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return -1;
            }
        }

        /// <summary>
        /// Permite obtener el código de zona
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public int ObtenerBarrazPorZona(int tipo)
        {
            int barrz = 0;
            switch (tipo)
            {
                case ConstantesSiosein2.MrepcodiDemandaEnergiaZonaNorte:
                    {
                        barrz = 1;
                        break;
                    }
                case ConstantesSiosein2.MrepcodiDemandaEnergiaZonaCentro:
                    {
                        barrz = 2;
                        break;
                    }
                case ConstantesSiosein2.MrepcodiDemandaEnergiaZonaSur:
                    {
                        barrz = 3;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return barrz;
        }

    }
}
