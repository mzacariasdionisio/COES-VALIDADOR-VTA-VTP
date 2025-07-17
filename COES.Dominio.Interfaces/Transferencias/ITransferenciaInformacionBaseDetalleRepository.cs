using COES.Dominio.DTO;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITransferenciaInformacionBaseDetalleRepository
    {
        /// <summary>
        /// Interface de acceso a datos de la tabla TRN_TRANS_ENTREGA
        /// </summary>
        int Save(TransferenciaInformacionBaseDetalleDTO entity);
        void Update(TransferenciaInformacionBaseDetalleDTO entity);
        void Delete(int pericodi, int version, string sCodigo);
        void DeleteListaTransferenciaInfoBaseDetalle(int iPeriCodi, int iVersion);
        TransferenciaInformacionBaseDetalleDTO GetById(System.Int32 id);
        List<TransferenciaInformacionBaseDetalleDTO> List(int emprcodi, int pericodi);
        List<TransferenciaInformacionBaseDetalleDTO> GetByCriteria(int emprcodi, int pericodi, string solicodireticodigo, int version);
        List<TransferenciaInformacionBaseDetalleDTO> GetByPeriodoVersion(int pericodi, int version);
        List<TransferenciaInformacionBaseDetalleDTO> ListByTransferenciaInfoBase(int iTInfbCodi);
    }
}
