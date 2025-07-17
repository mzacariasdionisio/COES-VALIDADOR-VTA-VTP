using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnVersiongrpReporsitory
    {
        void Save(PrnVersiongrpDTO entity);
        void Delete(int vergrpcodi);
        void Update(PrnVersiongrpDTO entity);
        List<PrnVersiongrpDTO> List();
        PrnVersiongrpDTO GetById(int vergrpcodi);
        PrnVersiongrpDTO GetByName(string vergrpnomb);
        int SaveGetId(PrnVersiongrpDTO entity);

        #region PRODEM3 - Mejoras 40 horas
        List<PrnVersiongrpDTO> ListVersionesPronosticoPorFecha(string fechaIni,
            string fechaFin);
        #endregion

        #region Demanda PO
        List<PrnVersiongrpDTO> ListVersionByArea(string area);
        List<PrnVersiongrpDTO> ListVersionByAreaFecha(
            string vergrpareausuaria, string fechaIni,
            string fechaFin);
        PrnVersiongrpDTO GetByNameArea(string vergrpnomb,
            string area);
        #endregion

    }
}
