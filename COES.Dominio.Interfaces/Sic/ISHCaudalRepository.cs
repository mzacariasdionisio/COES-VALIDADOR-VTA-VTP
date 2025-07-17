using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SH_CAUDAL
    /// </summary>
    public interface ISHCaudalRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(SHCaudalDTO entity);
        int Update(SHCaudalDTO entity);
        int SaveDetalle(SHCaudalDetalleDTO entity);
        SHCaudalDTO GetCaudalActivo(SHCaudalDTO entity);
        List<SHCaudalDTO> List(SHCaudalDTO entity);
        List<SHCaudalDetalleDTO> ListDetalle(SHCaudalDTO entity);

    }
}
