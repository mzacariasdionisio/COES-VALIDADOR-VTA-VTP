using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetRegHojaASubestRepository
    {

        List<DetRegHojaASubestDTO> GetDetRegHojaASubestFichaCCodi( int fichaCCodi);
        
        bool SaveDetRegHojaASubest(DetRegHojaASubestDTO detRegHojaASubestDTO);

        bool DeleteDetRegHojaASubestById(int id, string usuario);

        int GetLastDetRegHojaASubestId();

        DetRegHojaASubestDTO GetDetRegHojaASubestById(int id);

        bool UpdateDetRegHojaASubest(DetRegHojaASubestDTO detRegHojaASubestDTO);


    }
}
