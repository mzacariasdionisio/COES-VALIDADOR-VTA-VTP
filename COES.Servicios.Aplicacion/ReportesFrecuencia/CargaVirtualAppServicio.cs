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
    public class CargaVirtualAppServicio : AppServicioBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCarga"></param>
        /// <returns></returns>
        public CargaVirtualDTO GetBydId(int IdCarga)
        {
            return FactoryReportesFrecuencia.GetCargaVirtualRepository().GetById(IdCarga);
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<CargaVirtualDTO> GetListaCargaVirtual(string FechaInicial, string FechaFinal, string CodEquipo)
        {
            var resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().GetListaCargaVirtual(FechaInicial, FechaFinal, CodEquipo);
            return resultado;
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<LecturaVirtualDTO> GetListaLecturaVirtual(int IdCarga)
        {
            var resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().GetListaLecturaVirtual(IdCarga);
            return resultado;
        }

        /// <summary>
        ///  Carga una lista de Empresas para Carga Virtual
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> GetListaEmpresasCargaVirtual()
        {
            var resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().GetListaEmpresasCargaVirtual();
            return resultado;
        }

        /// <summary>
        ///  Carga una lista de Empresas para Carga Virtual
        /// </summary>
        /// <returns></returns>
        public List<CentralDTO> GetListaCentralPorEmpresa(int CodEmpresa)
        {
            var resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().GetListaCentralPorEmpresa(CodEmpresa);
            return resultado;
        }

        /// <summary>
        ///  Carga una lista de Empresas para Carga Virtual
        /// </summary>
        /// <returns></returns>
        public List<UnidadDTO> GetListaUnidadPorCentralEmpresa(int CodEmpresa, string Central)
        {
            var resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().GetListaUnidadPorCentralEmpresa(CodEmpresa, Central);
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<CargaVirtualDTO> SaveUpdate(CargaVirtualDTO entity)
        {
            ResultadoDTO<CargaVirtualDTO> resultado = new ResultadoDTO<CargaVirtualDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveUpdate(entity);
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
        public ResultadoDTO<CargaVirtualDTO> SaveUpdateExterno(CargaVirtualDTO entity)
        {
            ResultadoDTO<CargaVirtualDTO> resultado = new ResultadoDTO<CargaVirtualDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveUpdateExterno(entity);
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
        public ResultadoDTO<LecturaVirtualDTO> SaveLecturaVirtual(LecturaVirtualDTO entity)
        {
            ResultadoDTO<LecturaVirtualDTO> resultado = new ResultadoDTO<LecturaVirtualDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveLecturaVirtual(entity);
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
        public string SaveLecturaVirtualString(LecturaVirtualDTO entity)
        {
            string resultado = string.Empty;
            resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveLecturaVirtualString(entity);
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<LecturaDTO> SaveLectura(LecturaDTO entity)
        {
            ResultadoDTO<LecturaDTO> resultado = new ResultadoDTO<LecturaDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveLectura(entity);
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
        public string SaveLecturaString(LecturaDTO entity)
        {
            string resultado = string.Empty;
            resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveLecturaString(entity);
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int SaveLecturaQuery(string query)
        {
            int resultado = 0;
            resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().SaveLecturaQuery(query);
            return resultado;
        }



        /// <summary>
        ///  Carga una listar de los equipos GPS por Filtro
        /// </summary>
        /// <returns></returns>
        /*public List<EquipoGPSDTO> GetListaEquipoGPSPorFiltro(int codEquipo, string IndOficial)
        {
            var resultado = FactoryReportesFrecuencia.GetEquipoGPSRepository().GetListaEquipoGPSPorFiltro(codEquipo, IndOficial);
            return resultado;
        }*/

        /// <summary>
        ///  Obtener el ultimo codigo generado de la tabla me_gps
        /// </summary>
        /// <returns></returns>
        /*public int GetUltimoCodigoGenerado()
        {
            int resultado = FactoryReportesFrecuencia.GetCargaVirtualRepository().GetUltimoCodigoGenerado();
            return resultado;
        }*/

        /// <summary>
        ///  Obtener el numero de registros de la tabla f_lectura para el equipo GPS
        /// </summary>
        /// <returns></returns>
        /*public int GetNumeroRegistrosPorEquipo(int gpsCodi)
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
        }*/





        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /*public ResultadoDTO<EquipoGPSDTO> Eliminar(EquipoGPSDTO entity)
        {
            ResultadoDTO<EquipoGPSDTO> resultado = new ResultadoDTO<EquipoGPSDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetEquipoGPSRepository().Eliminar(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }*/

    }
}
