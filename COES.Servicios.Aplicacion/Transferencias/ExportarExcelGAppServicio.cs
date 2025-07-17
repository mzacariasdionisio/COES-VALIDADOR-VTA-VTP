using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class ExportarExcelGAppServicio : AppServicioBase
    {
        TransferenciasAppServicio servTransf = new TransferenciasAppServicio();
        /// <summary>
        /// Permite realizar búsquedas de Cuadro para exportar en base al periodo y version
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<ExportExcelDTO> BuscarValoresParaCuadro(int pericodi, int version)
        {
            return FactoryTransferencia.GetExportExcelRepository().GetByPeriVer(pericodi, version);
        }

        /// <summary>
        /// Permite realizar búsquedas de compensaciones en base la empresa periodo y version
        /// </summary>
        /// <param name="emprcodi","pericodi","version"></param>
        /// <returns></returns>
        public List<ExportExcelDTO> BuscarCompensacion(int emprcodi, int pericodi, int version)
        {
            return FactoryTransferencia.GetExportExcelRepository().GetByEmprPeriVersion(emprcodi, pericodi, version);
        }

        /// <summary>
        /// Permite realizar búsquedas de pagos de empresas en base al empresa periodo y version
        /// </summary>
        /// <param name="nombre","pericodi","version"></param>
        /// <returns></returns>
        public List<EmpresaPagoDTO> BuscarPagosMatriz(int emprcodi, int pericodi, int version)
        {
            return FactoryTransferencia.GetExportExcelRepository().GetMatrizByPeriVersion(emprcodi, pericodi, version);
        }

        /// <summary>
        /// Permite realizar búsquedas de pagos de empresas en base al periodo y version
        /// </summary>
        /// <param name="pericodi","version"></param>
        /// <returns></returns>
        public List<EmpresaPagoDTO> BuscarEmpresasPagMatriz(int pericodi, int version)
        {
            return FactoryTransferencia.GetExportExcelRepository().GetMatrizEmprByPeriVersion(pericodi, version);
        }
               
        /// <summary>
        /// Permite listar todas los códigos de entrega y retiro (con contrato) que estan habilitados en el periodo / EXTRANET
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iEmprCodi">Código de la Empresa</param>
        /// <returns>Lista de ExportExcelDTO</returns>
        public List<ExportExcelDTO> BuscarCodigoRetiro(int iPeriCodi, int iEmprCodi)
        {
            return FactoryTransferencia.GetIEnvioInformacionRepository().GetByCriteria(iPeriCodi, iEmprCodi);
        }

        /// <summary>
        /// Permite listar todas los códigos de entrega y retiro (con y sin contrato) que estan habilitados en el periodo /INTRANET
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iEmprCodi">Código de la Empresa</param>
        /// <param name="iBarrCodi">Código de la Barra de Transferencia</param>
        /// <returns>Lista de ExportExcelDTO</returns>
        public List<ExportExcelDTO> BuscarCodigoRetiroVistaTodo(int iPeriCodi, int iEmprCodi, int iBarrCodi)
        {
            var emprNew = 0;
            var empresasTTIe = servTransf.ListTrnMigracionDTI().ToList();
            var empresa = empresasTTIe.Where(x => x.Emprcodiorigen == iEmprCodi).ToList();
            if (empresa.Count > 0) emprNew = empresa.First().Emprcodidestino;
            List<ExportExcelDTO> list = new List<ExportExcelDTO>();
            list = FactoryTransferencia.GetIEnvioInformacionRepository().ListVistaTodo(iPeriCodi, iEmprCodi, iBarrCodi);
            if(list.Count < 1) list = FactoryTransferencia.GetIEnvioInformacionRepository().ListVistaTodo(iPeriCodi, emprNew, iBarrCodi);
            if (empresa.Count > 0)
            {

            }
            return list;
        }

        /// <summary>
        /// Permite listar todas los códigos de información base que estan habilitados en el periodo / EXTRANET
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iEmprCodi">Código de la Empresa</param>
        /// <returns>Lista de ExportExcelDTO</returns>
        public List<ExportExcelDTO> GetByListCodigoInfoBase(int iPeriCodi, int iEmprCodi)
        {
            return FactoryTransferencia.GetIEnvioInformacionRepository().GetByListCodigoInfoBase(iPeriCodi, iEmprCodi);
        }

        /* ASSETEC 202001 */
        /// <summary>
        /// Permite listar todas los códigos de información base por Envio
        /// </summary>
        /// <param name="trnenvcodi">Código de envio</param>
        /// <returns>Lista de ExportExcelDTO</returns>
        public List<ExportExcelDTO> GetByListCodigoInfoBaseByEnvio(int trnenvcodi)
        {
            return FactoryTransferencia.GetIEnvioInformacionRepository().GetByListCodigoInfoBaseByEnvio(trnenvcodi);
        }

        /// <summary>
        /// Permite listar todas los códigos de un Modelo que estan habilitados en el periodo /INTRANET
        /// </summary>
        /// <param name="pericodi">Código del Periodo</param>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="trnmodcodi">Código del modelo de la Empresa</param>
        /// <returns>Lista de ExportExcelDTO</returns>
        public List<ExportExcelDTO> GetByListCodigoModelo(int pericodi, int emprcodi, int trnmodcodi)
        {
            return FactoryTransferencia.GetIEnvioInformacionRepository().GetByListCodigoModelo(pericodi, emprcodi, trnmodcodi);
        }

        /// <summary>
        /// Permite listar todas los códigos de un Modelo que estan habilitados en el periodo /INTRANET
        /// </summary>
        /// <param name="pericodi">Código del Periodo</param>
        /// <param name="emprcodi">Código de la Empresa</param>
        /// <param name="trnmodcodi">Código del modelo de la Empresa</param>
        /// <returns>Lista de ExportExcelDTO</returns>
        public List<ExportExcelDTO> GetByListCodigoModeloVTA(int pericodi, int emprcodi, int trnmodcodi)
        {
            return FactoryTransferencia.GetIEnvioInformacionRepository().GetByListCodigoModeloVTA(pericodi, emprcodi, trnmodcodi);
        }
    }
}
