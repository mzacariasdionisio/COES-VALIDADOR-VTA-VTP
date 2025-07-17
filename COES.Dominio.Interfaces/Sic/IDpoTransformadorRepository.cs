using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoTransformadorRepository
    {
        void Save(DpoTransformadorDTO entity);
        void Update(DpoTransformadorDTO entity);
        void Delete(string id);
        List<DpoTransformadorDTO> List();
        List<DpoTransformadorDTO> ListTransformadorBySubestacion(int codigo);
        DpoTransformadorDTO GetById(int id);
        List<DpoTransformadorDTO> ListTransformadorByList(string codigo);
        List<DpoTransformadorDTO> ListTransformadorByListExcel(string codigo);
        void UpdateTransformadoresSirpit(string inicio, string fin);
    }
}
