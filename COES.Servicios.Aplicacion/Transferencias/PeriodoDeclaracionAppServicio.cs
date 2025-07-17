using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
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


namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// 
    /// </summary>
    public class PeriodoDeclaracionAppServicio : AppServicioBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="peridcCodi"></param>
        /// <returns></returns>
        public PeriodoDeclaracionDTO GetBydId(int peridcCodi)
        {
            return FactoryTransferencia.GetPeriodoDeclaracionRepository().GetById(peridcCodi);
        }

        /// <summary>
        /// Carga una lista desplegable para llenar los combobox con los periodos de declaración
        /// </summary>
        /// <returns></returns>
        public List<PeriodoDeclaracionDTO> GetListaCombobox()
        {
            var resultado = FactoryTransferencia.GetPeriodoDeclaracionRepository().GetListaCombobox();
            return resultado;
        }
        /// <summary>
        ///  Carga una listar los periodos de declaración
        /// </summary>
        /// <returns></returns>
        public List<PeriodoDeclaracionDTO> GetListaPeriodoDeclaracion()
        {
            var resultado = FactoryTransferencia.GetPeriodoDeclaracionRepository().GetListaPeriodoDeclaracion();
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<PeriodoDeclaracionDTO> SaveUpdate(PeriodoDeclaracionDTO entity)
        {
            ResultadoDTO<PeriodoDeclaracionDTO> resultado = new ResultadoDTO<PeriodoDeclaracionDTO>();
            resultado.Data = FactoryTransferencia.GetPeriodoDeclaracionRepository().SaveUpdate(entity);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }

    }
}
