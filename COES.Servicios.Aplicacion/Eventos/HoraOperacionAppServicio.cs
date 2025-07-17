using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;

namespace COES.Servicios.Aplicacion.Eventos
{
    public class HoraOperacionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite grabar los datos 
        /// </summary>
        /// <param name="entitys"></param>
        public void GrabarDatos(List<IeodCuadroDTO> entitys, DateTime fecha)
        {
            try
            {
                FactorySic.ObtenerIeodCuadroDao().GrabarDatos(entitys, fecha);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite consultar los datos cargados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<IeodCuadroDTO> Consultar(DateTime fecha)
        {
            return FactorySic.ObtenerIeodCuadroDao().GetByCriteria(fecha, fecha);
        }

        /// <summary>
        /// Permite obtener el reporte
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<IeodCuadroDTO> ObtenerReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.ObtenerIeodCuadroDao().ObtenerReporte(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Permite obtener la configuracion
        /// </summary>
        /// <returns></returns>
        public List<IeodCuadroDTO> GetConfiguracion()
        {
            return FactorySic.ObtenerIeodCuadroDao().GetConfiguracionEmpresa();
        }

        /// <summary>
        /// Permite determinar si existen datos del rsf
        /// </summary>
        /// <param name="equiCodi"></param>
        /// <param name="hora"></param>
        /// <returns></returns>
        public bool ExisteRegistro(int ptoMediCodi, DateTime horaInicio, DateTime horaFin)
        {
            List<IeodCuadroDTO> list = FactorySic.ObtenerIeodCuadroDao().ValidarExistenciaRegistro(ptoMediCodi, horaInicio, horaFin);

            if (list.Count > 0) return true;

            return false;            
        }
    }

}
