using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_EQUICANAL
    /// </summary>
    public interface IEqEquicanalRepository
    {
        int Save(EqEquicanalDTO entity);
        void Update(EqEquicanalDTO entity);
        void Delete(int areacode, int canalcodi, int equicodi, int tipoinfocodi);
        void Delete_UpdateAuditoria(int areacode, int canalcodi, int equicodi, int tipoinfocodi, string user);
        EqEquicanalDTO GetById(int areacode, int canalcodi, int equicodi, int tipoinfocodi);
        List<EqEquicanalDTO> List();
        List<EqEquicanalDTO> GetByCriteria();

        List<EqEquicanalDTO> ListarEquivalencia(int areacode, int idEmpresa, int idFamilia, int medida);
    }
}
