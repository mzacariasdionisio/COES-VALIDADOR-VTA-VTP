using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene las operaciones con la tabla TRN_ENVIO
    /// </summary>
    public interface ITrnEnvioRepository : IRepositoryBase
    {
        #region Metodos Basicos Trn_Envio
        int Save(TrnEnvioDTO entity);
        int Update(TrnEnvioDTO entity);
        int Delete(int id);
        TrnEnvioDTO GetById(int trnenvcodi);
        List<TrnEnvioDTO> List(int pericodi, int recacodi, int emprcodi, string tipoinfocodi, int trnmodcodi);
        List<TrnEnvioDTO> ListIntranet(int pericodi, int recacodi, int emprcodi, string tipoinfocodi, string trnenvplazo, string trnenvliqvt);
        List<TrnEnvioDTO> GetByCriteria(int idEmpresa, int idPeriodo);
        #endregion

        #region Metodos Adicionales Trn_Envio
        TrnEnvioDTO GetByIdPeriodoEmpresa(int pericodi, int recacodi, int emprcodi, string trnenvtipinf, int trnmodcodi);
        int UpdateByCriteriaTrnEnvio(int pericodi, int recacodi, int emprcodi, int trnmodcodi, string trnenvtipinf, string suser);
        void UpdateTrnEnvioLiquidacion(TrnEnvioDTO dtoTrnEnvio);
        void UpdateEntregaLiquidacion(ExportExcelDTO dtoEntrega);
        void UpdateRetiroLiquidacion(ExportExcelDTO dtoRetiro, int trnenvcodi, int trnmodcodi, string suser);
        #endregion
    }
}