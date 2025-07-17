using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPmoDatDbusRepository
    {
        List<PmoDatDbusDTO> ListDbus();
        List<PmoDatDbusDTO> ListDatDbus();
        void GenerateDat();
        int CountDatDbus(int periCodi);
    }
}
