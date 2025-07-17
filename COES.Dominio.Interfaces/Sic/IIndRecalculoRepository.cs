using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_RECALCULO
    /// </summary>
    public interface IIndRecalculoRepository
    {
        int Save(IndRecalculoDTO entity);
        void Update(IndRecalculoDTO entity);
        void Delete(int irecacodi);
        IndRecalculoDTO GetById(int irecacodi);
        List<IndRecalculoDTO> List();
        List<IndRecalculoDTO> GetByCriteria(int ipericodi);
        List<IndRecalculoDTO> ListXMes(int anio, int mes);
    }
}
