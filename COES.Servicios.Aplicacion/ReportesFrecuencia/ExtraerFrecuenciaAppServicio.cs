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
    public class ExtraerFrecuenciaAppServicio : AppServicioBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCarga"></param>
        /// <returns></returns>
        public ExtraerFrecuenciaDTO GetBydId(int IdCarga)
        {
            return FactoryReportesFrecuencia.GetExtraerFrecuenciaRepository().GetById(IdCarga);
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<ExtraerFrecuenciaDTO> GetListaExtraerFrecuencia(string FechaInicial, string FechaFinal)
        {
            var resultado = FactoryReportesFrecuencia.GetExtraerFrecuenciaRepository().GetListaExtraerFrecuencia(FechaInicial, FechaFinal);
            return resultado;
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<LecturaVirtualDTO> GetListaMilisegundos(int IdCarga)
        {
            var resultado = FactoryReportesFrecuencia.GetExtraerFrecuenciaRepository().GetListaMilisegundos(IdCarga);
            return resultado;
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public ResultadoDTO<ExtraerFrecuenciaDTO> Save(ExtraerFrecuenciaDTO entity)
        {
            ResultadoDTO<ExtraerFrecuenciaDTO> resultado = new ResultadoDTO<ExtraerFrecuenciaDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetExtraerFrecuenciaRepository().SaveUpdate(entity);
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
        /*public ResultadoDTO<CargaVirtualDTO> SaveUpdate(CargaVirtualDTO entity)
        {
            ResultadoDTO<CargaVirtualDTO> resultado = new ResultadoDTO<CargaVirtualDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveUpdate(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /*public ResultadoDTO<LecturaVirtualDTO> SaveLecturaVirtual(LecturaVirtualDTO entity)
        {
            ResultadoDTO<LecturaVirtualDTO> resultado = new ResultadoDTO<LecturaVirtualDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveLecturaVirtual(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }*/







    }
}
