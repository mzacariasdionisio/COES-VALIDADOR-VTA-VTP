using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_TRANS_ENTREGA_DETALLE
    /// </summary>
    public interface ITransferenciaEntregaDetalleRepository
    {
        int Save(TransferenciaEntregaDetalleDTO entity);
        void Update(TransferenciaEntregaDetalleDTO entity);
        void Delete(int pericodi, int version, string sCodigo);
        TransferenciaEntregaDetalleDTO GetById(System.Int32 id);
        List<TransferenciaEntregaDetalleDTO> List(int emprcodi, int pericodi);
        List<TransferenciaEntregaDetalleDTO> GetByCriteria(int emprcodi, int pericodi, string codientrcodi, int version);
        List<TransferenciaEntregaDetalleDTO> GetByPeriodoVersion(int pericodi, int version);
        List<TransferenciaEntregaDetalleDTO> BalanceEnergiaActiva(int pericodi, int version, Int32? barrcodi, Int32? emprcodi);
        List<TransferenciaEntregaDetalleDTO> ListByTransferenciaEntrega(int iTEntCodi, int iTRetVersion);
        void DeleteListaTransferenciaEntregaDetalle(int iPeriCodi, int iVersion);
        List<TransferenciaEntregaDetalleDTO> ListTransEntrReti(int iEmprcodi, int iBarrcodi, int iPericodi, int iVersion, string iFlagtipo);
        List<TransferenciaEntregaDetalleDTO> ListaTransferenciaEntrPorPericodiYVersion(int pericodi, int version);

        List<ExportExcelDTO> ListarCodigoReportado(int emprcodi, int pericodi, int recacodi);
        int GetCodigoGenerado();
        void BulkInsert(List<TrnTransEntregaDetalleBullk> entitys);

        TransferenciaEntregaDetalleDTO GetDemandaByCodVtea(int pericodi, int version, string codvtea, int dia);
        TransferenciaEntregaDetalleDTO GetDemandaByCodVteaEmpresa(int pericodi, int version, string codvtea, int dia, int emprcodi);
    }
}
