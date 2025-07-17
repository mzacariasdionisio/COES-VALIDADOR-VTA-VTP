using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data; //STS
using System.Data.Common; //STS

namespace COES.Dominio.Interfaces.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene las operaciones con la tabla TRN_PONTENCIA_CONTRATADA
    /// </summary>
    public interface ITrnPotenciaContratadaRepository : IRepositoryBase
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int Save(TrnPotenciaContratadaDTO entity, string usuario, IDbConnection conn, DbTransaction tran, int correlativo);
        List<TrnPotenciaContratadaDTO> GetByCriteria(int idEmpresa, int idPeriodo, int idCliente, int idBarra);
        List<TrnPotenciaContratadaDTO> GetEnvios(int idPeriodo);
        List<TrnPotenciaContratadaDTO> GetPotenciasContratadas(int coresoCodi, int periCodi);
        List<TrnPotenciaContratadaDTO> GetPotenciasContratadasAprobar(int coresoCodi, int periCodi);
        List<TrnPotenciaContratadaDTO> GetListaGrupoAsociadoVTA(List<int> coresoCodi, int pericodi);


        int? GenerarPotenciasAgrupadas(TrnPotenciaContratadaDTO entity);
        int? GenerarCodigosAgrupadosReservados(string userName);
        int? GenerarCargaDatosExcelEnvio(TrnPotenciaContratadaDTO entity);
        void GenerarCargaDatosExcel(TrnPotenciaContratadaDTO entity);
        void GenerarCargaDatos(TrnPotenciaContratadaDTO entity);
        void GenerarCargaDatosAprobar(TrnPotenciaContratadaDTO entity);
        void AprobarSolicitudCambios(TrnPotenciaContratadaDTO entity);
        void RechazarSolicitudCambios(TrnPotenciaContratadaDTO entity);
        void DesagruparPotencias(TrnPotenciaContratadaDTO entity, int omitirExcel = 0);
        void DesactivarPotenciasContratadas(int coresoCodi, int pericodi);
        void DesactivarPotenciasPorBarrSum(int coresoCodi, int coregeCodi, int pericodi);
        void Delete(int pericodi, int emprcodi);

        //assetec 20200604
        void CopiarPotenciasContratadasByPeriodo(int idPeriodoActual, int idPeriodoAnterior, string sUser);
        void DeleteByPeriodo(int pericodi);
    }
}
