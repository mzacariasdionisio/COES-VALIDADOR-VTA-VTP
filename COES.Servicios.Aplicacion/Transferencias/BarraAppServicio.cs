using COES.Base.Core;
using COES.Dominio.DTO.Enum;
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
    /// Clases con métodos del módulo Barra
    /// </summary>
    public class BarraAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BarraAppServicio));

        /// <summary>
        /// Permite insertar o actualizar la información de la entidad Barra
        /// </summary>
        /// <param name="BarraDTO">Entidad de la barra</param>
        /// <returns>Retorna el iBarrCodi nuevo o actualizado</returns>
        public int SaveOrUpdateBarra(BarraDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.BarrCodi == 0)
                {
                    id = FactoryTransferencia.GetBarraRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetBarraRepository().Update(entity);
                    id = entity.BarrCodi;
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
        /// Elimina una barra en base al iBarrCodi
        /// </summary>
        /// <param name="iBarrCodi">Código de la barra</param>
        /// <returns>Retorna el iBarrCodi eliminado</returns>
        public int DeleteBarra(int iBarrCodi, string username)
        {
            try
            {
                FactoryTransferencia.GetBarraRepository().Delete(iBarrCodi);
                FactoryTransferencia.GetBarraRepository().Delete_UpdateAuditoria(iBarrCodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return iBarrCodi;
        }

        /// <summary>
        /// Permite obtener la entidad barra en base al iBarrCodi
        /// </summary>
        /// <param name="iBarrCodi">Código de la barra</param>
        /// <returns>entidad BarraDTO</returns>
        public BarraDTO GetByIdBarra(int iBarrCodi)
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().GetById(iBarrCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }

        }

        /// <summary>
        /// Permite listar todas las barras
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListBarras()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().List();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Listar las barras de suministros relacionadas con transferencia
        /// </summary>
        /// <param name="barrCodiTra">Código de la barra transferencia</param>
        /// <returns></returns>
        public List<BarraDTO> ListarBarrasSuministrosRelacionada(int barrCodiTra)
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListarBarrasSuministrosRelacionada(barrCodiTra);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }

        }


        /// <summary>
        /// Permite listar todas las barras de transferencia
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaBarraTransferencia()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaBarraTransferencia();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras de transferencia
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListarBarraSuministro()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaBarraSuministro();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite realizar búsquedas de barras en base al sBarrNombre
        /// </summary>
        /// <param name="sBarrNombre"></param>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> BuscarBarra(string sBarrNombre)
        {
            try
            {
                string barrCodi = ConstantesAppServicio.ParametroDefecto;
                return FactoryTransferencia.GetBarraRepository().GetByCriteria(sBarrNombre, barrCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }

        }

        /// <summary>
        /// Permite listar todas las barras de la tabla trn_barra interceptado con la tabra trn_codigo_entrega
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaInterCodEnt()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCodEnt();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras generadoras de la tabla trn_barra interceptado con la tabra TRN_CODIGO_RETIRO_SOLICITUD
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaInterCoReSo()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCoReSo();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Lista los codigo de barra suministro generados
        /// </summary>
        /// <param name="genEmprCodi"></param>
        /// <param name="clienEmprCodi"></param>
        /// <returns></returns>
        public List<BarraDTO> ListaInterCoReSoByEmpr(int genEmprCodi, int clienEmprCodi)
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCoReSoByEmpr(genEmprCodi, clienEmprCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genEmprCodi"></param>
        /// <param name="clienEmprCodi"></param>
        /// <param name="enumComparativoEntregaRetiros"></param>
        /// <returns></returns>
        public List<BarraDTO> ListaBarraRetirosEntregaEmpresa(string tipoInfoCodigo, int periCodi, int genEmprCodi, int clienEmprCodi, string enumComparativoEntregaRetiros)
        {
            try
            {

                if (tipoInfoCodigo == "1" && EnumComparativoEntregaRetiros.Retiro == enumComparativoEntregaRetiros)
                    return FactoryTransferencia.GetBarraRepository().ListaBarraRetirosEmpresa(genEmprCodi, clienEmprCodi);
                else if (tipoInfoCodigo == "1" && EnumComparativoEntregaRetiros.Entrega == enumComparativoEntregaRetiros)
                    return FactoryTransferencia.GetBarraRepository().ListaBarraEntregaEmpresa(genEmprCodi, clienEmprCodi);
                else if (tipoInfoCodigo == "2" )
                    return FactoryTransferencia.GetBarraRepository().ListaBarraEmpresaValorizados(genEmprCodi, enumComparativoEntregaRetiros, periCodi);
                else
                    return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genEmprCodi"></param>
        /// <param name="clienEmprCodi"></param>
        /// <returns></returns>
        public List<BarraDTO> ListaInterCoReGeByEmpr(int genEmprCodi, int clienEmprCodi)
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCoReGeByEmpr(genEmprCodi, clienEmprCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genEmprCodi"></param>
        /// <param name="clienEmprCodi"></param>
        /// <returns></returns>
        public List<BarraDTO> ListarTodasLasBarras(int genEmprCodi, int clienEmprCodi)
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListarTodasLasBarras(genEmprCodi, clienEmprCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras generadoras de la tabla trn_barra interceptado con la tabla  VTP_CODIGO_RETIRO_SOL_DET
        /// </summary>
        /// <param name="barrCodiTrans"></param>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaInterCoReSoDt(int? barrCodiTrans)
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCoReSoDt(barrCodiTrans);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras de la tabla trn_barra interceptado con la tabra TRN_CODIGO_RETIRO_SINCONTRATO
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaInterCoReSC()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCoReSC();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras de la tabla trn_barra interceptado con la tabla TRN_VALOR_TRANS
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaInterValorTrans()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterValorTrans();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras de la tabla TRN_BARRA interceptado con la vista VW_EQ_AREA
        /// </summary>
        /// <returns>Lista de BarraDTO [incluido: areanomb]</returns>
        public List<BarraDTO> ListVista()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListVista();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras de la tabla trn_barra interceptado con la tabra trn_codigo_infobase
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaInterCodInfoBase()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListaInterCodInfoBase();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar todas las barras de transferencia, con el flag ¿Reporte?=SI
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListBarrasTransferenciaByReporte()
        {
            try
            {
                return FactoryTransferencia.GetBarraRepository().ListBarrasTransferenciaByReporte();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite obtener un registro de una barras al compararlo con BarrNombre
        /// </summary>
        /// <param name="sBarrNombre"></param>
        /// <returns>BarraDTO</returns>
        public BarraDTO GetByBarra(string sBarrNombre)
        {
            BarraDTO result = new BarraDTO();
            try
            {
                result = FactoryTransferencia.GetBarraRepository().GetByBarra(sBarrNombre);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return result;
        }

        /// <summary>
        /// Permite insertar la información de la entidad BarraRelacion
        /// </summary>
        /// <param name="entity">Entidad de la relacion barra</param>
        /// <returns>Retorna el número de registros insertados</returns>
        public int RegistrarRelacionBarra(BarraRelacionDTO entity)
        {
            int id = 0;
            try
            {
                if (entity.BareCodi == 0)
                {
                    id = FactoryTransferencia.GetBarraRelacionRepository().Save(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return id;
        }

        /// <summary>
        /// Permite eliminar la relación entre la barra de trasnferencia y la barra de suministro
        /// </summary>
        /// <param name="entity">Entidad de la relacion barra</param>
        /// <returns>Retorna el número de registros eliminados</returns>
        public int EliminarRelacionBarra(BarraRelacionDTO entity)
        {
            int id = 0;
            try
            {
                if (entity.BareCodi != 0)
                {
                    id = FactoryTransferencia.GetBarraRelacionRepository().Delete(entity);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return id;
        }

        /// <summary>
        /// Permite eliminar la relación entre la barra de trasnferencia y la barra de suministro
        /// </summary>
        /// <param name="idBarrTra">Entidad de la relacion barra</param>
        /// <returns>Retorna el número de registros eliminados</returns>
        public List<BarraRelacionDTO> ListaRelacion(int idBarrTra)
        {
            try
            {
                return FactoryTransferencia.GetBarraRelacionRepository().ListaRelacion(idBarrTra);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }

        }

        /// <summary>
        /// Permite eliminar la relación entre la barra de trasnferencia y la barra de suministro
        /// </summary>
        /// <param name="entity">Entidad de la relacion barra</param>
        /// <returns>Retorna el número de registros eliminados</returns>
        public bool ExisteRelacionBarra(BarraRelacionDTO entity)
        {
            bool result = false;
            try
            {
                result = FactoryTransferencia.GetBarraRelacionRepository().ExisteRelacionBarra(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return result;
        }

        /// Permite listar todas las barras de transferencia
        /// </summary>
        /// <returns>Lista de BarraDTO</returns>
        public List<BarraDTO> ListaBarrasActivas()
        {
            return FactoryTransferencia.GetBarraRepository().ListaBarrasActivas();
        }

        #region SIOSEIN-PRIE-2021
        /// <summary>
        /// Permite obtener un registro de una barras al compararlo con su codigo Osinergmin
        /// </summary>
        /// <param name="osinergCodi"></param>
        /// <returns>BarraDTO</returns>
        public BarraDTO GetBarraAreaxOsinergmin(string osinergCodi)
        {
            return FactoryTransferencia.GetBarraRepository().GetBarraAreaxOsinergmin(osinergCodi);
        }

        #endregion
    }
}
