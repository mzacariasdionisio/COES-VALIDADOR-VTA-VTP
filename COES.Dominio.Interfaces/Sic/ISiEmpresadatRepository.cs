using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_EMPRESADAT
    /// </summary>
    public interface ISiEmpresadatRepository
    {
        void Save(SiEmpresadatDTO entity);
        void Update(SiEmpresadatDTO entity);
        void Delete(DateTime empdatfecha, int consiscodi, int emprcodi);
        void Delete_UpdateAuditoria(DateTime empdatfecha, int consiscodi, int emprcodi, string username);
        SiEmpresadatDTO GetById(DateTime empdatfecha, int consiscodi, int emprcodi);
        List<SiEmpresadatDTO> List();
        List<SiEmpresadatDTO> GetByCriteria();
        List<SiEmpresadatDTO> ListByEmpresaYConcepto(DateTime fechaInicio, DateTime fechaFin, string empresas, string conceptos);
    }
}
