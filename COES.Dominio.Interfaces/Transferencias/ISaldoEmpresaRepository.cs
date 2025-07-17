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
    public interface ISaldoEmpresaRepository
    {
        int Save(SaldoEmpresaDTO entity);
        void Update(SaldoEmpresaDTO entity);
        void Delete(int pericodi, int version);
    }
}

