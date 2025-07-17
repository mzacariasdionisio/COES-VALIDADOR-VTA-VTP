using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_SALDO_EMPRESA
    /// </summary>
    public interface ISaldoCodigoRetiroSCRepository
    {
        int Save(SaldoCodigoRetiroscDTO entity);
        void Update(SaldoCodigoRetiroscDTO entity);
        void Delete(int pericodi, int version);
    }
}

