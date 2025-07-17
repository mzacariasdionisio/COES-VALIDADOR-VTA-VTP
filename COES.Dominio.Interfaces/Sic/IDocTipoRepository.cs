using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla DOC_TIPO
    /// </summary>
    public interface IDocTipoRepository
    {
        List<DocTipoDTO> List();
        List<DocTipoDTO> GetByCriteria();
        DocTipoDTO GetById(int tipoDoc);
    }
}
