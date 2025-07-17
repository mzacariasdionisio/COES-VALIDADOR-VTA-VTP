using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VARepository
    {
        List<CuestionarioH2VADTO> GetCuestionarioH2VACodi(int h2vaCodi);
        bool SaveCuestionarioH2VA(CuestionarioH2VADTO cuestionarioH2VADTO);
        bool DeleteCuestionarioH2VAById(int id, string usuario);
        int GetLastCuestionarioH2VAId();
        CuestionarioH2VADTO GetCuestionarioH2VAById(int id);

        List<CuestionarioH2VADTO> GetFormatoH2VAByFilter(string plancodi, string empresa, string estado);
    }
}