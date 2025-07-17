using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_COMPMENSUALELE
    /// </summary>
    public interface IStCompmensualeleRepository
    {
        int Save(StCompmensualeleDTO entity);
        void Update(StCompmensualeleDTO entity);
        void Delete(int cmpmelcodi);
        void DeleteStCompmensualEleVersion(int id);
        StCompmensualeleDTO GetById(int cmpmencodi, int stcompcodi);
        List<StCompmensualeleDTO> List();
        List<StCompmensualeleDTO> GetByCriteria(int strecacodi);
        List<StCompmensualeleDTO> ListStCompMenElePorID(int Cmpmencodi);
        StCompmensualeleDTO GetByIdStCompMensualEle(int strecacodi, int stcompcodi, int stcntgcodi);
    }
}
