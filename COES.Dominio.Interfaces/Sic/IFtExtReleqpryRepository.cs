using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_RELEQPRY
    /// </summary>
    public interface IFtExtReleqpryRepository
    {
        int Save(FtExtReleqpryDTO entity);
        void Update(FtExtReleqpryDTO entity);
        void Delete(int ftreqpcodi);
        FtExtReleqpryDTO GetById(int ftreqpcodi);
        List<FtExtReleqpryDTO> List();
        List<FtExtReleqpryDTO> GetByCriteria();
        List<FtExtReleqpryDTO> ListarPorEquipo(int equicodi);
        List<FtExtReleqpryDTO> ListarPorGrupo(int grupocodi);
        int Save(FtExtReleqpryDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtReleqpryDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<FtExtReleqpryDTO> ListarSoloEquipos();
        List<FtExtReleqpryDTO> ListarSoloGrupos();
        List<FtExtReleqpryDTO> ListarPorEmpresaPropietariaYProyecto(int ftprycodi, int emprcodi);
        List<FtExtReleqpryDTO> ListarPorEmpresaCopropietariaYProyecto(int ftprycodi, int emprcodi);
    }
}
