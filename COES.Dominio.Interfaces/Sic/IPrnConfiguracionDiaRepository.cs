using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnConfiguracionDiaRepository
    {
        void Save(PrnConfiguracionDiaDTO entity);
        void Update(PrnConfiguracionDiaDTO entity);
        void Delete(int id);
        List<PrnConfiguracionDiaDTO> List();
        List<PrnConfiguracionDiaDTO> ObtenerPorRango(string fechaIni, string fechaFin);

    }
}
