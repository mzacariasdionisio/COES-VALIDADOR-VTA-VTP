using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_CVARIABLES
    /// </summary>
    public interface IPrCvariablesRepository
    {
        void Update(PrCvariablesDTO entity);
        void Delete();
        PrCvariablesDTO GetById();
        List<PrCvariablesDTO> ListPrCvariabless(int id);
        List<PrCvariablesDTO> GetByCriteria();
        List<PrCvariablesDTO> GetCostosVariablesPorRepCv(int repcvCodi);
        List<PrCvariablesDTO> ObtenerCVariablePorRepcodiYCatecodi(string repcodi, string catecodi, string fenergcodi);
        void EliminarCostosVariablesPorRepCv(int repcvCodi);
        void EjecutarComandoCv(string strComando);

        #region MonitoreoMME
        List<PrCvariablesDTO> ListCostoVariablesxRangoFecha(DateTime dfecIniMes, DateTime dfecFinMes);
        #endregion
    }
}

