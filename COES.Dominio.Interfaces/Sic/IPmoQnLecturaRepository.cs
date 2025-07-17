using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_QN_LECTURA
    /// </summary>
    public interface IPmoQnLecturaRepository
    {
        int Save(PmoQnLecturaDTO entity);
        void Update(PmoQnLecturaDTO entity);
        void Delete(int qnlectcodi);
        PmoQnLecturaDTO GetById(int qnlectcodi);
        List<PmoQnLecturaDTO> List();
        List<PmoQnLecturaDTO> GetByCriteria();
    }
}
