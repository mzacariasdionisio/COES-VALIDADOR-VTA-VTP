using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaDRepository
    {
        List<RegHojaDDTO> GetRegHojaDProyCodi(int proyCodi);

        bool SaveRegHojaD(RegHojaDDTO regHojaDDTO);

        bool DeleteRegHojaDById(int id, string usuario);

        int GetLastRegHojaDId();

        List<RegHojaDDTO> GetRegHojaDById(int id);

        bool UpdateRegHojaD(RegHojaDDTO regHojaDDTO);

        bool DeleteRegHojaDById2(int id, string usuario);

        List<RegHojaDDTO> GetRegHojaDByFilter(string plancodi, string empresa, string estado);
    }
}
