using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Infraestructura.Datos.Repositorio.Transferencias;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using System.Data;


namespace COES.Servicios.Aplicacion.ReportesFrecuencia
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipoGPSAppServicio : AppServicioBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="peridcCodi"></param>
        /// <returns></returns>
        public EquipoGPSDTO GetBydId(int gpsCodi)
        {
            return FactoryReportesFrecuencia.GetEquipoGPSRepository().GetById(gpsCodi);
        }

        /// <summary>
        /// Carga una lista desplegable para llenar los combobox con los periodos de declaración
        /// </summary>
        /// <returns></returns>
        /*public List<PeriodoDeclaracionDTO> GetListaCombobox()
        {
            var resultado = FactoryTransferencia.GetPeriodoDeclaracionRepository().GetListaCombobox();
            return resultado;
        }*/
        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<EquipoGPSDTO> GetListaEquipoGPS()
        {
            var resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().GetListaEquipoGPS();
            return resultado;
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS por Filtro
        /// </summary>
        /// <returns></returns>
        public List<EquipoGPSDTO> GetListaEquipoGPSPorFiltro(int codEquipo, string IndOficial)
        {
            var resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().GetListaEquipoGPSPorFiltro(codEquipo, IndOficial);
            return resultado;
        }

        /// <summary>
        ///  Obtener el ultimo codigo generado de la tabla me_gps
        /// </summary>
        /// <returns></returns>
        public int GetUltimoCodigoGenerado()
        {
            int resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().GetUltimoCodigoGenerado();
            return resultado;
        }

        /// <summary>
        ///  Obtener el numero de registros de la tabla f_lectura para el equipo GPS
        /// </summary>
        /// <returns></returns>
        public int GetNumeroRegistrosPorEquipo(int gpsCodi)
        {
            int resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().GetNumeroRegistrosPorEquipo(gpsCodi);
            return resultado;
        }

        /// <summary>
        ///  Validar si el nombre del Equipo existe
        /// </summary>
        /// <returns></returns>
        public int ValidarNombreEquipoGPS(EquipoGPSDTO entity)
        {
            int resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().ValidarNombreEquipoGPS(entity);
            return resultado;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<EquipoGPSDTO> SaveUpdate(EquipoGPSDTO entity)
        {
            ResultadoDTO<EquipoGPSDTO> resultado = new ResultadoDTO<EquipoGPSDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetEquipoGPSRepository().SaveUpdate(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<EquipoGPSDTO> Eliminar(EquipoGPSDTO entity)
        {
            ResultadoDTO<EquipoGPSDTO> resultado = new ResultadoDTO<EquipoGPSDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetEquipoGPSRepository().Eliminar(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }

    }
}
