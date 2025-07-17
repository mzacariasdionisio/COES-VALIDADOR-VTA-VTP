using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_VALOR_TRANS
    /// </summary>
    public interface IValorTransferenciaRepository
    {
        int Save(ValorTransferenciaDTO entity);
        void Delete(System.Int32 PeriCodi, System.Int32 ValoTranVersion);
        List<ValorTransferenciaDTO> List(int pericodi, int version);
        List<ValorTransferenciaDTO> GetByCriteria(int? empcodi, int? barrcodi, int? pericodi, int? tipoemprcodi, int? version, string flagEntrReti); //int pericodi, string barrcodi
        List<ValorTransferenciaDTO> GetTotalByTipoFlag(int pericodi, int version);
        List<ValorTransferenciaDTO> GetValorEmpresa(int pericodi, int version);
        decimal GetSaldoEmpresa(int pericodi, int version, int emprcodi);
        List<ValorTransferenciaDTO> GetBalanceEnergia(int pericodi, int version);
        List<ValorTransferenciaDTO> GetBalanceValorTransferencia(int pericodi, int version);
        List<ValorTransferenciaDTO> GetValorTransferencia(int iPeriCodi, int iVTranVersion, int iEmpcodi, int iBarrcodi, string sTranFlag);
        List<ValorTransferenciaDTO> GetDesviacionRetiros(int iPeriCodi, int iPeriCodiAnterior);
        void BulkInsert(List<TrnValorTransBullk> entitys);
        int GetCodigoGenerado();
        //ASSETEC - 20181001
        void GrabarValorizacionEntrega(int pericodi, int version, string user, int iVtrancodi);
        void GrabarValorizacionRetiro(int pericodi, int version, string user, int iVtrancodi);

        List<ValorTransferenciaDTO> ListarValorTransferenciaUltVersionXEmpresaYTipoflag(int pericodi, int iVtrancodi);
        int? GetMaxVersion(int pericodi);
        List<ValorTransferenciaDTO> GetValorTransferenciaAgrpBarra(string barracodi, int periCodi, int vtranversion, string vtranflag);
        List<ValorTransferenciaDTO> ObtenerListaValoresTransferencia(int pericodi, int vtranversion, string vtranflag, string emprcodi);

        List<ValorTransferenciaDTO> ListarCodigosValorizados(int pericodi, int version, int? empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, DateTime? fechaIni, DateTime? fechaFin);
        List<ValorTransferenciaDTO> ListarCodigosValorizadosTransferencia(int pericodi, int version, int? empcodi, int? barrcodi, string flagEntrReti, DateTime? fechaIni, DateTime? fechaFin);
        List<ValorTransferenciaDTO> ListarCodigosValorizadosGrafica(int pericodi, int version, int? empcodi, string codigos);
        DataTable ListarCodigosValorizadosGrafica_New(int pericodi, int version, int? empcodi, string codigos, DateTime FechaInicio, DateTime FechaFin);
        DataTable ListarCodigosValorizadosGraficaTransferencia_New(int pericodi, int version, int? empcodi, string codigos, DateTime FechaInicio, DateTime FechaFin);//int pericodi, string barrcodi



        DataTable ListarCodigosValorizadosGraficaTotal(int pericodi, int version, int? empcodi, string codigos, DateTime? FechaInicio, DateTime? FechaFin);
        DataTable ListarCodigosValorizadosGraficaTotalTransferencia(int pericodi, int version, int? empcodi, string codigos, DateTime? FechaInicio, DateTime? FechaFin);

        List<ValorTransferenciaDTO> ListarCodigos(int EmprCodi);
        List<EmpresaDTO> ListarEmpresasAsociadas(ComparacionEntregaRetirosFiltroDTO parametro);
        List<ValorTransferenciaDTO> ListarComparativo(int pericodi, int version,
            int? empcodi, int? cliemprcodi, int? barrcodi, string flagEntrReti, string dias, string codigos);

        DataTable ListarComparativoEntregaRetiroValo(ComparacionEntregaRetirosFiltroDTO parametro);
        DataTable ListarComparativoEntregaRetiroValoTransferencia(ComparacionEntregaRetirosFiltroDTO parametro);
        DataTable ListarComparativoEntregaRetiroValorDET(ComparacionEntregaRetirosFiltroDTO parametro);
        DataTable ListarComparativoEntregaRetiroValorDETTransferencia(ComparacionEntregaRetirosFiltroDTO parametro);


        //CU21
        List<ValorTransferenciaDTO> ListarEnergiaEntregaDetalle(int iPeriCodi, int iVTranVersion, int iCodEntCodi);
        List<ValorTransferenciaDTO> ListarEnergiaRetiroDetalle(int iPeriCodi, int iVTranVersion, string listaCodigosRetiro);
        List<ValorTransferenciaDTO> ListarValorEnergiaEntregaDetalle(int iPeriCodi, int iVTranVersion, int iCodEntCodi);
        List<ValorTransferenciaDTO> ListarValorEnergiaRetiroDetalle(int iPeriCodi, int iVTranVersion, string listaCodigosRetiro); 
    }
}
