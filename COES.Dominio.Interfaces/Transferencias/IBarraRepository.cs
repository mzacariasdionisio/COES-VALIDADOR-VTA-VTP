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
    /// Interface de acceso a datos de la tabla TRN_BARRA
    /// </summary>
    public interface IBarraRepository : IRepositoryBase
    {
        int Save(BarraDTO entity);
        void Update(BarraDTO entity);
        void Delete(System.Int32 id);
        void Delete_UpdateAuditoria(System.Int32 id, string username);
        BarraDTO GetById(System.Int32 id);
        List<BarraDTO> List();
        List<BarraDTO> ListarBarrasSuministrosRelacionada(int barrcodiTra);
        List<BarraDTO> ListaBarraTransferencia();
        List<BarraDTO> ListaBarraSuministro();
        List<BarraDTO> GetByCriteria(string nombre, string barracodi);
        List<BarraDTO> ListaInterCodEnt();
        List<BarraDTO> ListaInterCoReSo();
        List<BarraDTO> ListaInterCoReSoDt(int? barracoditrans);
        List<BarraDTO> ListaInterCoReSoByEmpr(int genemprcodi, int clienemprcodi);
        List<BarraDTO> ListaInterCoReGeByEmpr(int genemprcodi, int clienemprcodi);
        List<BarraDTO> ListaBarraRetirosEmpresa(int genEmprCodi, int clienEmprCodi);
        List<BarraDTO> ListaBarraEntregaEmpresa(int genEmprCodi, int clienEmprCodi);
        List<BarraDTO> ListaBarraEmpresaValorizados(int genEmprCodi, string flag, int periCodi);
        List<BarraDTO> ListarTodasLasBarras(int genEmprCodi, int clienEmprCodi);
        List<BarraDTO> ListaInterCoReSC();
        List<BarraDTO> ListaInterValorTrans();
        List<BarraDTO> ListVista();
        List<BarraDTO> ListaInterCodInfoBase();
        List<BarraDTO> ListBarrasTransferenciaByReporte();
        BarraDTO GetByBarra(string sBarrNombre);
        List<BarraDTO> ListarBarraReporteDTR();
        BarraDTO ObtenerBarraDTR(int barraCodi);

        // Inicio de Agregado - Sistema de Compensaciones
        List<BarraDTO> ListarBarras();
        // Fin de Agregado - Sistema de Compensaciones

        #region SIOSEIN
        List<BarraDTO> GetListaBarraArea(string barras);
        #endregion

        #region MonitoreoMME
        List<BarraDTO> ListarGrupoBarraEjec();
        #endregion

        #region siosein2
        List<BarraDTO> ListaCentralxBarra();
        #endregion

        List<BarraDTO> ListaBarrasActivas();

        #region SIOSEIN-PRIE-2021
        BarraDTO GetBarraAreaxOsinergmin(string osinergCodi);
        #endregion

        //CPPA-ASSETEC-2024
        List<BarraDTO> FiltroBarrasTransIntegrantes(int revision);
        List<BarraDTO> ListaBarrasTransFormato();
    }
}
