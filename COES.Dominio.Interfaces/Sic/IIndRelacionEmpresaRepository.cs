using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndRelacionEmpresaRepository
    {
        int Save(IndRelacionEmpresaDTO entity);
        void Update(IndRelacionEmpresaDTO entity);
        void Delete(int relempcodi);
        IndRelacionEmpresaDTO GetById(int relempcodi);
        List<IndRelacionEmpresaDTO> List();
        List<IndRelacionEmpresaDTO> ListByIdEmpresa(int emprcodi);
        List<IndRelacionEmpresaDTO> GetByIdCentral(int equicodicentral);
        List<IndRelacionEmpresaDTO> GetByIdUnidad(int equicodiunidad);

        List<IndRelacionEmpresaDTO> ListCentral();

        List<IndRelacionEmpresaDTO> ListUnidad(int equicodicentral);
        List<IndRelacionEmpresaDTO> ListGaseoducto();


        List<SiEmpresaDTO> ListEmpresas(string tipoemprcodi);
        List<SiEmpresaDTO> ListEmpresasConGaseoducto(string tipoemprcodi);
        List<EqEquipoDTO> ListCentrales(string emprcodi);
        List<EqEquipoDTO> ListCentralesConGaseoducto(string emprcodi);
        List<EqEquipoDTO> ListUnidades(string emprcodi, string equicodicentral);
        List<PrGrupoDTO> ListGrupos(string emprcodi, string equicodicentral, string equicodiunidad);
        List<IndRelacionEmpresaDTO> ListUnidadNombres(string emprcodi, string equicodicentral);
        List<IndRelacionEmpresaDTO> ListUnidadNombresConGaseoducto(string emprcodi, string equicodicentral);
        List<IndRelacionEmpresaDTO> GetByCriteria(string relempcodi, string emprcodi, string equicodicentral);
        List<IndRelacionEmpresaDTO> GetByCriteria2(string emprcodi, string equicodicentral, string equicodiunidad, string grupocodi, string famcodi);

        List<PrGrupoDTO> ListPrGrupoForCN2();
        List<PrGrupodatDTO> ListPrGrupodatByCriteria(string grupocodi, string concepcodi, DateTime fechadat);
        List<PrGrupoEquipoValDTO> ListPrGrupoEquipoValByCriteria(string concepcodi, string equicodi, string grupocodi, DateTime greqvafechadat);
    }
}
