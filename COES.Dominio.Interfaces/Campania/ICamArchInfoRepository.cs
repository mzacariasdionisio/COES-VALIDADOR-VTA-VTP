using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamArchInfoRepository
    {

        List<ArchivoInfoDTO> GetArchivoInfoProyCodi(int proyCodi, int secccodi);

        List<ArchivoInfoDTO> GetArchivoInfoByProyCodi(int proyCodi);

        ArchivoInfoDTO GetArchivoInfoNombreArchivo(string nombre);

        bool SaveArchivoInfo(ArchivoInfoDTO ArchivoInfo);

        bool DeleteArchivoInfoById(int id, string usuario);

        int GetLastArchivoInfoId();

        ArchivoInfoDTO GetArchivoInfoById(int id);

        bool UpdateArchivoInfoByProyCodi(int proyCodi, string ruta);

        bool UpdateArchivoInfo(ArchivoInfoDTO ArchivoInfoDTO);

        List<ArchivoInfoDTO> GetArchivoInfoProyCodiNom(int proyCodi, int secccodi, string nombre);
    }
}
