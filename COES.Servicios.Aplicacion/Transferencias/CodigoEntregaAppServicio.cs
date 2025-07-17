using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// Clases con métodos del módulo CodigoEntrega
    /// </summary>
    public  class CodigoEntregaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CodigoEntregaAppServicio));

        /// <summary>
        /// Permite grabar o actualizar un  CodigoEntregaDTO en base a la entidad
        /// </summary>
        /// <param name="entity">Entidad de CodigoEntregaDTO</param>
        /// <returns>Retorna el iCodEntCodi nuevo o actualizado</returns>
        public int SaveOrUpdateCodigoEntrega(CodigoEntregaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.CodiEntrCodi == 0)
                {
                    id = FactoryTransferencia.GetCodigoEntregaRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetCodigoEntregaRepository().Update(entity);
                    id = entity.CodiEntrCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un CodigoEntrega en base al iCodEntCodi
        /// </summary>
        /// <param name="iCodEntCodi">Código de la tabla TRN_CODIGO_ENTREGA</param>
        /// <returns>Retorna el iCodEntCodi eliminado</returns>
        public int DeleteCodigoEntrega(int iCodEntCodi)
        {
            try
            {
                FactoryTransferencia.GetCodigoEntregaRepository().Delete(iCodEntCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return iCodEntCodi;
        }

        /// <summary>
        /// Permite obtener el CodigoEntrega en base al iCodEntCodi
        /// </summary>
        /// <param name="iCodEntCodi">Código de la tabla TRN_CODIGO_ENTREGA</param>
        /// <returns>CodigoEntregaDTO</returns>
        public CodigoEntregaDTO GetByIdCodigoEntra(int iCodEntCodi)
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().GetById(iCodEntCodi);
        }

        /// <summary>
        /// Permite listar todos los codigoEntrega
        /// </summary>
        /// <returns>Lista de CodigoEntregaDTO</returns>
        public List<CodigoEntregaDTO> ListCodigoEntrega()
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas de CodigoEntrega en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sEmprNombre">Nombre de la Empresa</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sEquiNomb">Nombre de la Central de generación</param>        
        /// <param name="dCodEntFechaInicio">Fecha en que inicia el código de entrega, puede ser nulo</param>        
        /// <param name="dCodEntFechaFin">Fecha en que concluye el código de entrega, puede ser nulo</param>        
        /// <param name="sCodEntEstado">Estado en que se encuentra el Código de Entrega</param>        
        /// <returns>Lista de CodigoEntregaDTO</returns>
        public List<CodigoEntregaDTO> BuscarCodigoEntrega(string sEmprNombre, string sBarrBarraTransferencia, string sEquiNomb, DateTime? dCodEntFechaInicio, DateTime? dCodEntFechaFin, string sCodEntEstado,string codentrga, int NroPagina, int PageSizeCodigoEntrega)
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().GetByCriteria(sEmprNombre, sBarrBarraTransferencia, sEquiNomb, dCodEntFechaInicio, dCodEntFechaFin, sCodEntEstado,codentrga, NroPagina, PageSizeCodigoEntrega);
        }

        /// <summary>
        /// Permite calcular el numero de filas de la consulta
        /// </summary>
        /// <param name="sEmprNombre">Nombre de la Empresa</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sEquiNomb">Nombre de la Central de generación</param>        
        /// <param name="dCodEntFechaInicio">Fecha en que inicia el código de entrega, puede ser nulo</param>        
        /// <param name="dCodEntFechaFin">Fecha en que concluye el código de entrega, puede ser nulo</param>        
        /// <param name="sCodEntEstado">Estado en que se encuentra el Código de Entrega</param>        
        /// <returns>Numero de filas de la consulta</returns>
        public int ObtenerNroFilasCodigoEntrega(string sEmprNombre, string sBarrBarraTransferencia, string sEquiNomb, DateTime? dCodEntFechaInicio, DateTime? dCodEntFechaFin, string sCodEntEstado,string codentrega)
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().ObtenerNroRegistros(sEmprNombre, sBarrBarraTransferencia, sEquiNomb, dCodEntFechaInicio, dCodEntFechaFin, sCodEntEstado, codentrega);
        }

        /// <summary>
        /// Permite obtener un codigoentrega  mediante su sCodEntCodigo
        /// </summary>
        /// <param name="sCodEntCodigo">Código asignado de la tabla</param>
        /// <returns>CodigoEntregaDTO</returns>
        public CodigoEntregaDTO GetByCodigoEntregaCodigo(string sCodEntCodigo)
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().GetByCodiEntrCodigo(sCodEntCodigo);
        }

        /// <summary>
        /// Permite obtener un codigoentrega de una empresa mediante su sCodEntCodigo
        /// </summary>
        /// <param name="sCodEntCodigo">Código asignado de la tabla</param>
        /// <returns>CodigoEntregaDTO</returns>
        public CodigoEntregaDTO GetByCodigoEntregaEmpresaCodigo(int iEmprCodi, string sCodEntCodigo)
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().GetByCodiEntrEmpresaCodigo(iEmprCodi, sCodEntCodigo);
        }

        /// <summary>
        /// Permite obtener un codigoentrega vigente en el periodo
        /// </summary>
        /// <param name="iPericodi">Periodo de valorización</param>
        /// <param name="sCodigo">Código de Entrega asignado</param>
        /// <returns>CodigoEntregaDTO</returns>
        public CodigoEntregaDTO CodigoEntregaVigenteByPeriodo(int iPericodi, string sCodigo)
        {
            return FactoryTransferencia.GetCodigoEntregaRepository().CodigoEntregaVigenteByPeriodo(iPericodi, sCodigo);
        }

        /// <summary>
        /// Permite grabar los datos de manera temporal
        /// </summary>
        /// <param name="periodo">Periodo de valorización</param>
        /// <param name="version">Versión de recalculo</param>
        /// <param name="empresa">Identificador de la empres</param>
        /// <param name="usuario">User en sesión</param>
        /// <param name="trnenvcodi">Identificador de Envio</param>
        /// <param name="entitys"></param>
        public List<DatosTransferencia> GrabarEntregaRetiro(List<DatosTransferencia> entitys, int periodo, int version, int empresa, string usuario, int trnenvcodi)
        {
            try
            {
                return FactoryTransferencia.GetDatosTransferenciaRepository().SaveEntregaRetiro(entitys, periodo, version, empresa, usuario, trnenvcodi);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //ASSETEC 202001
        /// <summary>
        /// Permite grabar los datos de manera temporal asociado a un envio
        /// </summary>
        /// <param name="entitys">Lista de entidades</param>
        /// <param name="periodo">Id Mes de valorizacion</param>
        /// <param name="version">Id Recalculo</param>
        /// <param name="empresa">Identificador de empresa</param>
        /// <param name="usuario">login user</param>
        /// <param name="trnenvcodi">Identificador de envio</param>
        /// <param name="testado">ACT / INA</param>
        public List<DatosTransferencia> GrabarEntregaRetiroEnvio(List<DatosTransferencia> entitys, int periodo, int version, int empresa, string usuario, int trnenvcodi, string testado)
        {
            try
            {
                return FactoryTransferencia.GetDatosTransferenciaRepository().SaveEntregaRetiroEnvio(entitys, periodo, version, empresa, usuario, trnenvcodi, testado);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite grabar los datos de manera temporal asociado a un envio
        /// </summary>
        /// <param name="entitys">Lista de entidades</param>
        /// <param name="periodo">Id Mes de valorizacion</param>
        /// <param name="version">Id Recalculo</param>
        /// <param name="listaEmpresa">lista de identificador de empresa</param>
        /// <param name="usuario">login user</param>
        /// <param name="trnenvcodi">Identificador de envio</param>
        /// <param name="testado">ACT / INA</param>
        public List<DatosTransferencia> GrabarModeloEnvio(List<DatosTransferencia> entitys, int periodo, int version, string listaEmpresa, string usuario, int trnenvcodi, int trnmodcodi, string testado)
        {
            try
            {
                return FactoryTransferencia.GetDatosTransferenciaRepository().SaveModeloEnvio(entitys, periodo, version, listaEmpresa, usuario, trnenvcodi, trnmodcodi, testado);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trnenvcodi"></param>
        /// <param name="periodo"></param>
        /// <param name="version"></param>
        public void UpdateRetirosInactivo(int trnenvcodi, int periodo, int version)
        {
            try
            {
                FactoryTransferencia.GetDatosTransferenciaRepository().UpdateRetirosInactivo(trnenvcodi, periodo, version);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
