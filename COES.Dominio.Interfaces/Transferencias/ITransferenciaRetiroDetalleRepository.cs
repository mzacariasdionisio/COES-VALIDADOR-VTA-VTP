using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_TRANS_RETIRO_DETALLE
    /// </summary>
    public interface ITransferenciaRetiroDetalleRepository
    {
        int Save(TransferenciaRetiroDetalleDTO entity);
        void Update(TransferenciaRetiroDetalleDTO entity);
        void Delete(int pericodi, int version, string sCodigo);
        void DeleteListaTransferenciaRetiroDetalle(int iPeriCodi, int iVersion);
        void DeleteListaTransferenciaRetiroDetalleEmpresa(int iPeriCodi, int iVersion, int IEmprCodi);
        TransferenciaRetiroDetalleDTO GetById(System.Int32 id);
        List<TransferenciaRetiroDetalleDTO> List(int emprcodi, int pericodi);
        List<TransferenciaRetiroDetalleDTO> GetByCriteria(int emprcodi, int pericodi, string solicodireticodigo, int version);
        List<TransferenciaRetiroDetalleDTO> GetByPeriodoVersion(int pericodi, int version);
        List<TransferenciaRetiroDetalleDTO> ListByTransferenciaRetiro(int iTRetCodi, int iTRetVersio);
        void BulkInsert(List<TrnTransRetiroDetalleBullk> entitys);
        int GetCodigoGenerado();
        List<TransferenciaRetiroDetalleDTO> ListaTransferenciaRetiPorPericodiYVersion(int periodo, int versionRetiro);

        //ASSETEC -- GRAN USUARIO - 28/11
        List<TransferenciaRetiroDetalleDTO> ListByTransferenciaRetiroDay(int iTRetCodi);
        TransferenciaRetiroDetalleDTO GetDemandaRetiroByCodVtea(int pericodi, int version, string codvtea, int dia);
        TransferenciaRetiroDetalleDTO GetDemandaRetiroByCodVteaEmpresa(int pericodi, int version, string codvtea, int dia, int emprcodi);
    }
}
