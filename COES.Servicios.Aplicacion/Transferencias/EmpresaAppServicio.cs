using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using log4net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.Servicios.Aplicacion.Transferencias
{

    public class EmpresaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmpresaAppServicio));

        /// <summary>
        /// Actualizar abreviatura
        /// </summary>
        /// <param name="iEmprCodi"></param>
        /// <returns></returns>
        public ResultadoDTO<EmpresaDTO> SaveUpdateAbreaviatura(EmpresaDTO parametro)
        {
            ResultadoDTO<EmpresaDTO> resultado = new ResultadoDTO<EmpresaDTO>();
            resultado.Data = FactoryTransferencia.GetEmpresaRepository().SaveUpdateAbreaviatura(parametro);
            if (!string.IsNullOrEmpty(resultado.Data.Mensaje))
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultado.Data.Mensaje;
            }
            return resultado;
        }


        /// <summary>
        /// Permite obtener la empresa en base al id en la vista vw_trn_empresa_str
        /// </summary>
        /// <param name="iEmprCodi">Codigo de la empresa</param>
        /// <returns>EmpresaDTO</returns>
        public EmpresaDTO GetByIdEmpresa(int iEmprCodi)
        {
            return FactoryTransferencia.GetEmpresaRepository().GetById(iEmprCodi);
        }

        /// <summary>
        /// Permite encontrar a una empresa de la vista vw_si_empresa
        /// </summary>
        /// <param name="sEmprNomb">Nombre de la empresa</param>
        /// <returns>EmpresaDTO</returns>       
        public EmpresaDTO GetByNombre(string sEmprNomb)
        {
            try
            {
                return FactoryTransferencia.GetEmpresaRepository().GetByNombre(sEmprNomb);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }
        public EmpresaDTO GetByNombreEstado(string sEmprNomb, int iPeriCodi)
        {
            return FactoryTransferencia.GetEmpresaRepository().GetByNombreEstado(sEmprNomb, iPeriCodi);
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_si_empresa
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListEmpresas()
        {
            return FactoryTransferencia.GetEmpresaRepository().List();
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_si_empresa por generadoras
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListEmpresasGeneradoras()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListGeneradoras();
        }

        /// <summary>
        /// Permite realizar búsquedas de empresas en base al nombre en la vista vw_si_empresa
        /// </summary>
        /// <param name="sEmprNomb">Nombre de la empresa</param>
        /// <returns>Lista de EmpresaDTO</returns>
        public List<EmpresaDTO> BuscarEmpresas(string sEmprNomb)
        {
            return FactoryTransferencia.GetEmpresaRepository().GetByCriteria(sEmprNomb);
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_trn_empresa_str
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListEmpresasSTR()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListEmpresasSTR();
        }
        /// <summary>
        /// Permite hacer una consulta de los datos de la empresa
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public BeanEmpresa ConsultarPorRUC(string ruc)
        {
            return (new ServicioSunat()).ObtenerDatosSunat(ruc);
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_si_empresa interceptado con la tabra trn_codigo_entrega
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListaInterCodEnt()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterCodEnt();
        }

        /// <summary>
        /// Permite listar todas las empresas generadoras de la vista vw_si_empresa interceptado con la tabra TRN_CODIGO_RETIRO_SOLICITUD
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>         
        public List<EmpresaDTO> ListaInterCoReSoGen()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterCoReSoGen();
        }

        /// <summary>
        /// Permite listar todas los clientes de la vista vw_si_empresa interceptado con la tabra TRN_CODIGO_RETIRO_SOLICITUD
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListaInterCoReSoCli()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterCoReSoCli();
        }
        /// <summary>
        /// Lista los clientes asociados a la empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<EmpresaDTO> ListaInterCoReSoCliPorEmpresa(int emprcodi)
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterCoReSoCliPorEmpresa(emprcodi);
        }

        public List<EmpresaDTO> ListaRetiroCliente(int emprcodi)
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaRetiroCliente(emprcodi);
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_trn_empresa_str interceptado con la tabra TRN_CODIGO_RETIRO_SINCONTRATO
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListaInterCoReSC()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterCoReSC();
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_trn_empresa_str interceptado con la tabra TRN_VALOR_TRANS
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListaInterValorTrans()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterValorTrans();
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_trn_empresa_str interceptado con la tabra TRN_CODIGO_INFOBASE
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListaInterCodInfoBase()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListaInterCodInfoBase();
        }

        /// <summary>
        /// Permite listar todas las empresas de la vista vw_si_empresa interceptado con la tabra TRN_CODIGO_INFOBASE,TRN_CODIGO_ENTREGA,TRN_CODIGO_RETIRO_SOLICITUD,TRN_CODIGO_RETIRO_SINCONTRATO
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListaEmpresasCombo()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListEmpresasCombo();
        }

        /// <summary>
        /// Lista las empresas activas para los combos de vtp
        /// </summary>
        /// <returns></returns>
        public List<EmpresaDTO> ListarEmpresasComboActivos()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListarEmpresasComboActivos();
        }


        /// <summary>
        /// Permite listar todas las empresas de la vista vw_si_empresa interceptado con la tabra TRN_CODIGO_ENTREGA,TRN_CODIGO_RETIRO_SOLICITUD,TRN_CODIGO_RETIRO_SINCONTRATO
        /// </summary>
        /// <returns>Lista de EmpresaDTO</returns>        
        public List<EmpresaDTO> ListInterCodEntregaRetiro()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListInterCodEntregaRetiro();
        }

        public List<EmpresaDTO> ListInterCodEntregaRetiroxPeriodo(int pericodi, int version)
        {
            return FactoryTransferencia.GetEmpresaRepository().ListInterCodEntregaRetiroxPeriodo(pericodi, version);
        }

        /// <summary>
        /// Permite buscar las empresas que tienen ptos configurados
        /// </summary>
        /// <returns></returns>
        public List<EmpresaDTO> ListaConfPtosMMExEmpresa()
        {
            return FactoryTransferencia.GetEmpresaRepository().ListEmpresasConfPtoMME();
        }

        /// <summary>
        /// Permite encontrar a una empresa de la vista vw_si_empresa
        /// </summary>
        /// <param name="sEmprNomb">Nombre de la empresa</param>
        /// <returns>EmpresaDTO</returns>       
        public EmpresaDTO GetByNombreSic(string sEmprNomb)
        {
            return FactoryTransferencia.GetEmpresaRepository().GetByNombreSic(sEmprNomb);
        }
    }
}
