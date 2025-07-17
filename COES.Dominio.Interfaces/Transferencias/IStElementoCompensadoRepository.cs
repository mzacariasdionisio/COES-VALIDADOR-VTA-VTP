using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_ELEMENTO_COMPENSADO
    /// </summary>
    public interface IStElementoCompensadoRepository
    {
        int Save(StElementoCompensadoDTO entity);
        void Update(StElementoCompensadoDTO entity);
        void Delete(int strecacodi);
        StElementoCompensadoDTO GetById(int elecmpcodi);
        List<StElementoCompensadoDTO> List();
        List<StElementoCompensadoDTO> GetByCriteria();
    }
}
