using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoTrafoBarraRepository
    {
        void Save(DpoTrafoBarraDTO entity);
        List<DpoTrafoBarraDTO> List();
        DpoTrafoBarraDTO GetById(int id);
        List<DpoTrafoBarraDTO> ListTrafoBarraById(string codigo);
    }
}
