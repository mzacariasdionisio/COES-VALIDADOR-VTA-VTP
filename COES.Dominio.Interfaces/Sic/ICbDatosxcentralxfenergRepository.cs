using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_DATOSXCENTRALXFENERG
    /// </summary>
    public interface ICbDatosxcentralxfenergRepository
    {
        int Save(CbDatosxcentralxfenergDTO entity);
        void Update(CbDatosxcentralxfenergDTO entity);
        void Delete(int cbdatcodi);
        CbDatosxcentralxfenergDTO GetById(int cbdatcodi);
        List<CbDatosxcentralxfenergDTO> List();
        List<CbDatosxcentralxfenergDTO> GetByCriteria(int cbcxfecodi, string ccombcodis);
    }
}
