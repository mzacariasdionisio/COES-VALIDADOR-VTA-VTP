using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEveTiposNumeralRepository
    {
        int Save(EveTiposNumeralDTO entity);
        List<EveTiposNumeralDTO> List(string estado);
        void Delete(int evetipnumcodi);
        void Update(EveTiposNumeralDTO entity);
    }
}
