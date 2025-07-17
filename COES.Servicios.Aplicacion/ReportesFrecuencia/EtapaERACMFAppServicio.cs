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
    public class EtapaERAAppServicio : AppServicioBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EtapaCodi"></param>
        /// <returns></returns>
        public EtapaERADTO GetBydId(int EtapaCodi)
        {
            return FactoryReportesFrecuencia.GetEtapaERARepository().GetById(EtapaCodi);
        }

        /// <summary>
        ///  Carga una listar de los equipos GPS
        /// </summary>
        /// <returns></returns>
        public List<EtapaERADTO> GetListaEtapas()
        {
            var resultado = FactoryReportesFrecuencia.GetEtapaERARepository().GetListaEtapas();
            return resultado;
        }

        /// <summary>
        ///  Obtener el ultimo codigo generado de la tabla me_gps
        /// </summary>
        /// <returns></returns>
        public int GetUltimoCodigoGenerado()
        {
            int resultado = FactoryReportesFrecuencia.GetEtapaERARepository().GetUltimoCodigoGenerado();
            return resultado;
        }

        /// <summary>
        ///  Validar si el nombre de etapa existe
        /// </summary>
        /// <returns></returns>
        public int ValidarNombreEtapa(EtapaERADTO entity)
        {
            int resultado = FactoryReportesFrecuencia.GetEtapaERARepository().ValidarNombreEtapa(entity);
            return resultado;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<EtapaERADTO> SaveUpdate(EtapaERADTO entity)
        {
            ResultadoDTO<EtapaERADTO> resultado = new ResultadoDTO<EtapaERADTO>();
            resultado.Data = FactoryReportesFrecuencia.GetEtapaERARepository().SaveUpdate(entity);
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
        public ResultadoDTO<EtapaERADTO> Eliminar(EtapaERADTO entity)
        {
            ResultadoDTO<EtapaERADTO> resultado = new ResultadoDTO<EtapaERADTO>();
            resultado.Data = FactoryReportesFrecuencia.GetEtapaERARepository().Eliminar(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }

    }
}
