using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_BARRA_URS
    /// </summary>
    public interface ITrnBarraUrsRepository : IRepositoryBase
    {
        int Save(TrnBarraursDTO entity);
        TrnBarraursDTO GetById(int barrcodi, int grupocodi);
        void Delete(int barrcodi, int grupocodi);
        void Delete_UpdateAuditoria(int barrcodi, int grupocodi, string username);
        List<TrnBarraursDTO> List(int id);
        TrnBarraursDTO GetByNombrePrGrupo(string grupocnomb);
        List<TrnBarraursDTO> ListPrGrupo();
        List<TrnBarraursDTO> ListURS();
        List<TrnBarraursDTO> GetEmpresas();
        List<TrnBarraursDTO> ListbyEquicodi(int equicodi);
        TrnBarraursDTO GetByIdGrupoCodi(int grupocodi);
        TrnBarraursDTO GetByIdGrupoCodiTRN(int grupocodi);
        List<TrnBarraursDTO> ListURSCostoMarginal(int pericodi, int vcrecacodi);
    }
}
