using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_SALDO_RECALCULO
    /// </summary>
    public interface ISaldoRecalculoRepository
    {
        int Save(SaldoRecalculoDTO entity);
        void Delete(int iPeriCodi, int iRecaCodi);
        int GetByPericodiDestino(int iPeriCodi, int iRecaCodi);
        void UpdatePericodiDestino(int iPeriCodi, int iRecaCodi, int iPeriCodiDestino);
    }
}
