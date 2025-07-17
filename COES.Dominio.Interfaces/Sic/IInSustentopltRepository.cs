using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_SUSTENTOPLT
    /// </summary>
    public interface IInSustentopltRepository
    {
        int Save(InSustentopltDTO entity);
        void Update(InSustentopltDTO entity);
        void Delete(int inpstcodi);
        InSustentopltDTO GetById(int inpstcodi);
        InSustentopltDTO GetVigenteByTipo(int tipo);
        List<InSustentopltDTO> List();
        List<InSustentopltDTO> GetByCriteria();
        int Save(InSustentopltDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateEstadoPlantilla(InSustentopltDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
