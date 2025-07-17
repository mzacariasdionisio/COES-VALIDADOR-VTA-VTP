using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ICrEmpresaSolicitanteRepository
    {
        List<CrEmpresaSolicitanteDTO> ListrEmpresaSolicitante(int cretapacodi);
        void Save(CrEmpresaSolicitanteDTO entity);
        bool ValidarEmpresaSolicitante(int cretapacodi, int emprcodi);
        void Delete(int crsolemprcodi);
        CrEmpresaSolicitanteDTO ObtenerEmpresaSolicitante(int crsolemprcodi);
        void Update(int crsolemprcodi, string argumentos, string desicion);
        void DeleteSolicitantexEtapa(int cretapacodi);
        List<CrEmpresaSolicitanteDTO> SqlObtenerEmpresaSolicitantexEvento(int CREVENCODI);
    }
}
