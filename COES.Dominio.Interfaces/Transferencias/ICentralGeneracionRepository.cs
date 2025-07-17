using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la vista VW_EQ_CENTRAL_GENERACION
    /// </summary>
    public interface ICentralGeneracionRepository : IRepositoryBase
    {
        List<CentralGeneracionDTO> List();
        List<CentralGeneracionDTO> ListaInterCodEnt();
        List<CentralGeneracionDTO> ListaCentralByEmpresa(int emprcodi); // PrimasRER.2023
        List<CentralGeneracionDTO> ListaCentralUnidadByEmpresa(int emprcodi); // PrimasRER.2023
        List<CentralGeneracionDTO> ListaInterCodInfoBase();
        List<CentralGeneracionDTO> ListInfoBase();
        CentralGeneracionDTO GetByCentGeneNombre(string sCentGeneNombre);
        List<CentralGeneracionDTO> Unidad();
        List<CentralGeneracionDTO> UnidadCentral(int equicodicen);
        List<CentralGeneracionDTO> ListEmpresaCentralGeneracion();
        CentralGeneracionDTO GetByCentGeneNombVsEN(string CentGeneNomb, int strecacodi);
        CentralGeneracionDTO GetByCentGeneTermoelectricaNombre(string CentGeneNomb);

        EqEquipoDTO GetByCentGeneNombreEquipo(string CentGeneNomb, int vcrecacodi);

        EqEquipoDTO GetByCentGeneUniNombreEquipo(string CentGeneNomb, int equicodi, int vcrecacodi);

        EqEquipoDTO GetByCentGeneNombreEquipoCenUni(string CentGeneNomb, int vcrecacodi);

        EqEquipoDTO GetByEquicodi(int equicodi);

        EqEquipoDTO GetByEquiNomb(string equinomb);

        //List<EqEquipoDTO> GetByEquiPadre(int equicodi);
        
        List<EqEquipoDTO> ListarEquiposPorEmpresa(int emprcodi); // SIOSEIN-PRIE-2021
    }
}
