using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_BLOB
    /// </summary>
    public interface IWbBlobRepository
    {
        int Save(WbBlobDTO entity);
        void Update(WbBlobDTO entity);
        void Delete(int blobcodi);
        WbBlobDTO GetById(int blobcodi);
        WbBlobDTO ObtenerBlobByUrl(string url);
        WbBlobDTO ObtenerBlobByUrl2(string url, int idFuente);
        WbBlobDTO ObtenerPorPadre(int blobcodi);
        List<WbBlobDTO> List();
        List<WbBlobDTO> GetByCriteria();

        List<WbBlobDTO> ObtenerByUrlParcial(string url, DateTime fechaInicio, DateTime fechaFin);

    }
}
