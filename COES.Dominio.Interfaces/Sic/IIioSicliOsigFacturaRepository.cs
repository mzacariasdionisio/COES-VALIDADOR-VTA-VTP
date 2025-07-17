using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_SICLI_OSIG_FACTURA
    /// </summary>
    public interface IIioSicliOsigFacturaRepository
    {
        int Save(IioSicliOsigFacturaDTO entity);
        
        void Delete(string periodo);

        int GetMaxId();

        int GetCountTotal(string periodo);

        int GetCountTotalFactura(string periodo);

        int GetCountTotalRuc(string periodo);
        

        int GetCountTotalFacturaRuc(string periodo);

        IDataReader ListRepCompCliente(string periodo);

        IDataReader ListRepCompEmpresa(string periodo);

        IDataReader ListRepCompHistorico(DateTime periodoInicio, DateTime periodoFin);
    }
}
