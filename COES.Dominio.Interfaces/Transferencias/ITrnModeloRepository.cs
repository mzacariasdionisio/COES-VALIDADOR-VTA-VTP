using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene las operaciones con las tablas TRN_MODELO y TRN_MODELO_RETIRO
    /// </summary>
    public interface ITrnModeloRepository : IRepositoryBase
    {
        #region Metodos Basicos Trn_Modelo
        int SaveTrnModelo(TrnModeloDTO entity);
        int UpdateTrnModelo(TrnModeloDTO entity);
        int DeleteTrnModelo(int id);
        List<TrnModeloDTO> ListTrnModelo();
        List<TrnModeloDTO> ListTrnModeloByEmpresa(int emprcodi);
        #endregion

        #region Metodos Adicionales Trn_Modelo
        List<TrnModeloDTO> ListarComboTrnModelo();
        #endregion

        #region Metodos Basicos Trn_Modelo_Retiro
        int SaveTrnModeloRetiro(TrnModeloRetiroDTO entity);
        int UpdateTrnModeloRetiro(TrnModeloRetiroDTO entity);
        int DeleteTrnModeloRetiro(int id);
        List<TrnModeloRetiroDTO> ListTrnModeloRetiro(int idModelo);
        #endregion

        #region Metodos Adicionales Trn_Modelo_Retiro 
        List<BarraDTO> ListarComboBarras();
        List<CodigoRetiroDTO> ListComboCodigoSolicitudRetiro(int idBarra);
        bool TieneCodigosRetiroModelo(int idModelo);
        #endregion
    }
}
