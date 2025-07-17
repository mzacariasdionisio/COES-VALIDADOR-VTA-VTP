using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_PROYECTO
    /// </summary>
    public interface IFtExtProyectoRepository
    {
        int Save(FtExtProyectoDTO entity);
        void Update(FtExtProyectoDTO entity);
        void Delete(int ftprycodi);
        FtExtProyectoDTO GetById(int ftprycodi);
        List<FtExtProyectoDTO> List();
        List<FtExtProyectoDTO> GetByCriteria();
        List<FtExtProyectoDTO> ListarProyectosPorRangoYEmpresa(string empresa, DateTime fechaIni, DateTime fechaFin);
        List<FtExtProyectoDTO> ListarProyectosSinCodigoEOPorAnio(int year);
        List<FtExtProyectoDTO> ListarPorEstado(string estado);
        List<FtExtProyectoDTO> ListarGrupo(string ftprycodis);
        List<FtExtProyectoDTO> ListarPorEmpresaYEtapa(int emprcodi, int ftetcodi, string feepryestado);
    }

}
