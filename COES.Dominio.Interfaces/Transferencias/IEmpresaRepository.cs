using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la vista VW_SI_EMPRESA
    /// </summary>
    public interface IEmpresaRepository : IRepositoryBase
    {
        EmpresaDTO SaveUpdateAbreaviatura(EmpresaDTO entity);
        EmpresaDTO GetById(System.Int32 id);
        EmpresaDTO GetByNombre(string sEmprNomb);
        EmpresaDTO GetByNombreEstado(string sEmprNomb , int iPeriCodi);
        List<EmpresaDTO> List();
        List<EmpresaDTO> GetByCriteria(string nombre);
        List<EmpresaDTO> ListEmpresasSTR();
        List<EmpresaDTO> ListaInterCodEnt();
        List<EmpresaDTO> ListaInterCoReSoGen();
        List<EmpresaDTO> ListaInterCoReSoCli();
        List<EmpresaDTO> ListaInterCoReSoCliPorEmpresa(int emprcodi);
        List<EmpresaDTO> ListaRetiroCliente(int emprcodi);
        List<EmpresaDTO> ListaInterCoReSC();
        List<EmpresaDTO> ListaInterValorTrans();
        List<EmpresaDTO> ListaInterCodInfoBase();
        List<EmpresaDTO> ListEmpresasCombo();
        List<EmpresaDTO> ListarEmpresasComboActivos();
        List<EmpresaDTO> ListInterCodEntregaRetiro();

        List<EmpresaDTO> ListInterCodEntregaRetiroxPeriodo(int iPeriCodi, int version);
        List<EmpresaDTO> ListEmpresasConfPtoMME();
        EmpresaDTO GetByNombreSic(string sEmprNomb);
        List<EmpresaDTO> ListGeneradoras();
    }
}
