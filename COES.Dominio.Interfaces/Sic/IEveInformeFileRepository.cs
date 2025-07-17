using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INFORME_FILE
    /// </summary>
    public interface IEveInformeFileRepository
    {
        int Save(EveInformeFileDTO entity);
        void Update(int idFile, string descripcion);
        void Delete(int eveninffilecodi);
        EveInformeFileDTO GetById(int eveninffilecodi);
        List<EveInformeFileDTO> List();
        List<EveInformeFileDTO> GetByCriteria();
        List<EveInformeFileDTO> ObtenerFilesInformeEvento(int idInforme);
    }
}
