using COES.Base.Core;
using COES.Dominio.DTO.Enum;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// 
    /// </summary>
    public class ActualizarTrasEmpFusionAppServicio : AppServicioBase
    {
        AuditoriaProcesoAppServicio servicioAuditoria = new AuditoriaProcesoAppServicio();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosSobrantes(int? pericodi, string usuario)
        {
            _ = new List<ActualizarTrasEmpFusionDTO>();
            var lSaldosSobrantes = FactoryTransferencia.GetActualizarTrasEmpFusionRepository().GetListaSaldosSobrantes(pericodi);
            FactoryTransferencia.GetActualizarTrasEmpFusionRepository().DeleteSaldosSobrantes();

            foreach (var l in lSaldosSobrantes)
            {
                l.SalsoUsuCreacion = usuario;
                l.SalsoUsuModificacion = usuario;
                l.PeriCodiDes = Convert.ToInt32(pericodi);
                l.SalsoVTEAVTP = "1";
                FactoryTransferencia.GetActualizarTrasEmpFusionRepository().SaveUpdate(l);
            }

            List<ActualizarTrasEmpFusionDTO> resultado = FactoryTransferencia.GetActualizarTrasEmpFusionRepository().GetLista(pericodi, "1");
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosSobrantesVTP(int? pericodi, string usuario)
        {
            _ = new List<ActualizarTrasEmpFusionDTO>();
            var lSaldosSobrantes = FactoryTransferencia.GetActualizarTrasEmpFusionRepository().GetListaSaldosSobrantesVTP(pericodi);
            FactoryTransferencia.GetActualizarTrasEmpFusionRepository().DeleteSaldosSobrantes();

            foreach (var l in lSaldosSobrantes)
            {
                l.SalsoUsuCreacion = usuario;
                l.SalsoUsuModificacion = usuario;
                l.PeriCodiDes = Convert.ToInt32(pericodi);
                l.SalsoVTEAVTP = "2";
                FactoryTransferencia.GetActualizarTrasEmpFusionRepository().SaveUpdate(l);
            }

            List<ActualizarTrasEmpFusionDTO> resultado = FactoryTransferencia.GetActualizarTrasEmpFusionRepository().GetListaVTP(pericodi, "2");
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public ResultadoDTO<int> SaveOrUpdateSaldos(int? pericodi, string items, string usuario)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
            Ids = Ids.Skip(1).ToArray();
            foreach (string Id in Ids)
            {
                ActualizarTrasEmpFusionDTO entidad = new ActualizarTrasEmpFusionDTO();
                entidad.SalsoCodi = Convert.ToInt32(Id);
                entidad.SalsoUsuModificacion = usuario;

                FactoryTransferencia.GetActualizarTrasEmpFusionRepository().SaveUpdateSaldos(entidad);
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTEA.EjecutarSaldosSobrantes;
                objAuditoria.Estdcodi = (int)EVtpEstados.SaldoSobranteTrasladado;
                objAuditoria.Audproproceso = "Proceso traslados de saldos VTEA";
                objAuditoria.Audprodescripcion = "Se trasladó satisfactoriamente el saldo del ID : " + entidad.SalsoCodi + " - cantidad de errores - 0";
                objAuditoria.Audprousucreacion = usuario;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                _ = servicioAuditoria.save(objAuditoria);

                #endregion
            }
            return resultado;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="items"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public ResultadoDTO<int> SaveOrUpdateSaldosVTP(int? pericodi, string items, string usuario)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            string[] Ids = items.Split(ConstantesAppServicio.CaracterComa);
            Ids = Ids.Skip(1).ToArray();
            foreach (string Id in Ids)
            {
                ActualizarTrasEmpFusionDTO entidad = new ActualizarTrasEmpFusionDTO();
                entidad.SalsoCodi = Convert.ToInt32(Id);
                entidad.SalsoUsuModificacion = usuario;
                FactoryTransferencia.GetActualizarTrasEmpFusionRepository().SaveUpdateSaldosVTP(entidad);
                #region AuditoriaProceso

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GenerarReporteSaldosSobrantes;
                objAuditoria.Estdcodi = (int)EVtpEstados.SaldoSobranteTrasladado;
                objAuditoria.Audproproceso = "Proceso traslados de saldos VTP";
                objAuditoria.Audprodescripcion = "Se trasladó satisfactoriamente el saldo del ID : " + entidad.SalsoCodi + " - cantidad de errores - 0";
                objAuditoria.Audprousucreacion = usuario;
                objAuditoria.Audprofeccreacion = DateTime.Now;

                _ = servicioAuditoria.save(objAuditoria);

                #endregion
            }
            return resultado;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosNoIdentificados(int? pericodi)
        {
            var lSaldosNoIdentificados = FactoryTransferencia.GetActualizarTrasEmpFusionRepository().GetListaSaldosNoIdentificados(pericodi);
            return lSaldosNoIdentificados;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public List<ActualizarTrasEmpFusionDTO> GetListaSaldosNoIdentificadosVTP(int? pericodi)
        {
            var lSaldosNoIdentificados = FactoryTransferencia.GetActualizarTrasEmpFusionRepository().GetListaSaldosNoIdentificadosVTP(pericodi);
            return lSaldosNoIdentificados;
        }
    }
}
