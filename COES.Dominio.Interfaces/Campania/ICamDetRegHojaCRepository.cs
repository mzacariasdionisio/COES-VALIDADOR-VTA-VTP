using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetRegHojaCRepository
    {

        List<DetRegHojaCDTO> GetDetRegHojaCFichaCCodi( int fichaCCodi);
        
        bool SaveDetRegHojaC(DetRegHojaCDTO detRegHojaCDTO);

        bool DeleteDetRegHojaCById(int id, string usuario);

        int GetLastDetRegHojaCId();

        DetRegHojaCDTO GetDetRegHojaCById(int id);

        bool UpdateDetRegHojaC(DetRegHojaCDTO detRegHojaCDTO);


    }
}
