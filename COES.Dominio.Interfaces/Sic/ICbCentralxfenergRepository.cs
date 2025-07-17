using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_CENTRALXFENERG
    /// </summary>
    public interface ICbCentralxfenergRepository
    {
        int Save(CbCentralxfenergDTO entity);
        void Update(CbCentralxfenergDTO entity);
        void Delete(int cbcxfecodi);
        CbCentralxfenergDTO GetById(int cbcxfecodi);
        CbCentralxfenergDTO GetByFenergYGrupocodi(int fenergcodi, int grupocodi);
        List<CbCentralxfenergDTO> List();
        List<CbCentralxfenergDTO> GetByCriteria(string estcomcodis);
    }
}
