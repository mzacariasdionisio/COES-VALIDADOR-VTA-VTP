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
    public class CopiarInformacionAppServicio: AppServicioBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCopia"></param>
        /// <returns></returns>
        public CopiarInformacionDTO GetBydId(int IdCopia)
        {
            return FactoryReportesFrecuencia.GetCopiarInformacionRepository().GetById(IdCopia);
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<CopiarInformacionDTO> GetListaCopiarInformacion(string FechaInicial, string FechaFinal, string CodEquipoOrigen, string CodEquipoDestino)
        {
            var resultado = FactoryReportesFrecuencia.GetCopiarInformacionRepository().GetListaCopiarInformacion(FechaInicial, FechaFinal, CodEquipoOrigen, CodEquipoDestino);
            return resultado;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<CopiarInformacionDTO> SaveUpdate(CopiarInformacionDTO entity)
        {
            ResultadoDTO<CopiarInformacionDTO> resultado = new ResultadoDTO<CopiarInformacionDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCopiarInformacionRepository().SaveUpdate(entity);
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
        public ResultadoDTO<CopiarInformacionDTO> Eliminar(CopiarInformacionDTO entity)
        {
            ResultadoDTO<CopiarInformacionDTO> resultado = new ResultadoDTO<CopiarInformacionDTO>();
            resultado.Data = FactoryReportesFrecuencia.GetCopiarInformacionRepository().Eliminar(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }









    }
}
