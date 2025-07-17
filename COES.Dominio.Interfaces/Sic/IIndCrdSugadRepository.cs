using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndCrdSugadRepository
    {
        int Save(IndCrdSugadDTO entity);
        void UpdateIndCrdSugad(IndCrdSugadDTO entity);
        void UpdateIndCrdEstado(IndCrdSugadDTO entity);
        IndCrdSugadDTO GetById(int crdsgdcodi);
        List<IndCrdSugadDTO> ListCrdSugadByCabecera(int indcbrcodi);
        List<IndCrdSugadDTO> ListCrdSugadJoinCabecera(int emprcodi, int ipericodi, int indcbrtipo);
        List<IndCrdSugadDTO> ListByCriteria(int ipericodi, string emprcodi, string indcbrtipo, string equicodicentral, string equicodiunidad, string grupocodi, string famcodi, string crdsgdtipo);
        List<IndCrdSugadDTO> ListCumplimientoDiario(int emprcodi, int ipericodi, int indcbrtipo);
    }
}
