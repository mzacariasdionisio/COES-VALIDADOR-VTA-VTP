using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICHATECNICA
    /// </summary>
    public interface IFtFichaTecnicaRepository
    {
        int Save(FtFichaTecnicaDTO entity);
        void Update(FtFichaTecnicaDTO entity);
        void Delete(FtFichaTecnicaDTO entity);
        FtFichaTecnicaDTO GetById(int fteccodi);
        List<FtFichaTecnicaDTO> List();
        List<FtFichaTecnicaDTO> GetByCriteria(string estado);
        FtFichaTecnicaDTO GetFichaMaestraPrincipal(int ambiente);
    }
}
