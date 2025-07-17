using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITransferenciaInformacionBaseRepository
    {
        /// <summary>
        /// Interface de acceso a datos de la tabla TRN_TRANS_ENTREGA
        /// </summary>
        int Save(TransferenciaInformacionBaseDTO entity);
        void Update(TransferenciaInformacionBaseDTO entity);
        void UpdateCodigo(TransferenciaInformacionBaseDTO entity);
        void Delete(int pericodi, int version, string sCodigo);
        void DeleteListaTransferenciaInfoBase(int pericodi, int version);
        TransferenciaInformacionBaseDTO GetById(System.Int32 id);
        List<TransferenciaInformacionBaseDTO> List(int emprcodi, int pericodi, int version);
        List<TransferenciaInformacionBaseDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
        TransferenciaInformacionBaseDTO GetTransferenciaInfoBaseByCodigo(int iEmprCodi, int iPeriCodi, int iTEntVersion, string sCodigo);
        TransferenciaInformacionBaseDTO GetTransferenciaInfoBaseByCodigoEnvio(int iEmprCodi, int iPeriCodi, int iTEntVersion, int trnenvcodi, string sCodigo);
    }
}
