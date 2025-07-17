using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnServiciosAuxiliaresRepository
    {
        void Save(PrnServiciosAuxiliaresDTO entity);
        void Delete(int prnserauxcodi);
        List<PrnServiciosAuxiliaresDTO> List(); 
        
        // -----------------------------------------------------------------------------------------------------------------
        List<PrnServiciosAuxiliaresDTO> ListBarraFormulas();
        List<PrnServiciosAuxiliaresDTO> GetServiciosAuxiliaresByGrupo(int PrGrupo);
        List<MePerfilRuleDTO> ListFormulas();
        List<MePerfilRuleDTO> ListFormulasRelaciones(int Grupocodi);
        void DeleteRelaciones(int grupoCodi);
        // -----------------------------------------------------------------------------------------------------------------
    }
}
