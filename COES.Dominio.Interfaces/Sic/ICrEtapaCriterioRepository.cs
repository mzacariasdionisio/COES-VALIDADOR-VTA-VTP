using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ICrEtapaCriterioRepository
    {
        int Save(CrEtapaCriterioDTO entity);
        List<CrEtapaCriterioDTO> ListaCriteriosEtapa(int cretapacricodi);
        void Delete(int cretapacodi);
        List<CrEtapaCriterioDTO> ListaCriteriosEvento(int cretapacodi);
    }
}
