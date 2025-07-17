using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_TABLA_SYNC
    /// </summary>
    public interface IIioTablaSyncRepository
    {
        List<IioTablaSyncDTO> List(int pseincodi);

        string GetPath(string periodo, string query, string delimitador, string path, string tabla, string nombreCortoTabla, int idEnvio);

        IDataReader GetDataReader(string periodo, string query);

    }
}