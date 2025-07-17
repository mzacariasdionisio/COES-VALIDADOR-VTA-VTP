using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamArchObsRepository
    {

        List<ArchivoObsDTO> GetArchivoObsByObsId(int observacion, string tipo);

        ArchivoObsDTO GetArchivoObsNombreArchivo(string nombre);

        bool SaveArchivoObs(ArchivoObsDTO ArchivoObs);

        bool DeleteArchivoObsById(int id, string usuario);

        int GetLastArchivoObsId();

        ArchivoObsDTO GetArchivoObsById(int id);

        bool UpdateArchivoObs(ArchivoObsDTO ArchivoObsDTO);

        List<ArchivoObsDTO> GetArchivoObsProyCodiNom(int observacionId, string tipo, string nombre);
    }
}
