using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetRegHojaDRepository
    {

        List<DetRegHojaDDTO> GetDetRegHojaDFichaCCodi(string proyCodi);

        bool SaveDetRegHojaD(DetRegHojaDDTO detRegHojaDDTO);

        bool DeleteDetRegHojaDById(int id, string usuario);

        int GetLastDetRegHojaDId();

        DetRegHojaDDTO GetDetRegHojaDById(string id);

        bool UpdateDetRegHojaD(DetRegHojaDDTO detRegHojaDDTO);

    }
}
