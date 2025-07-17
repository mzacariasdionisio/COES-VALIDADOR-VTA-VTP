using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnConfigbarraRepository
    {
        void Save(PrnConfigbarraDTO entity);
        void Update(PrnConfigbarraDTO entity);
        void Delete(int grupocodi, DateTime cfgbarfecha);
        List<PrnConfigbarraDTO> List();
        PrnConfigbarraDTO GetById(int grupocodi, DateTime cfgbarfecha);
        List<PrnConfigbarraDTO> ParametrosList(string fecdesde, string fechasta, string lstbarras);
        PrnConfigbarraDTO GetConfiguracion(int grupocodi, string fecha, int defid, string deffecha);
    }
}
