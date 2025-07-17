using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PERFIL_RULE
    /// </summary>
    public interface IMePerfilRuleRepository
    {
        int Save(MePerfilRuleDTO entity);
        void Update(MePerfilRuleDTO entity);
        void Delete(int prrucodi, string username);
        MePerfilRuleDTO GetById(int prrucodi);
        List<MePerfilRuleDTO> List();
        List<MePerfilRuleDTO> GetByCriteria(int area, string areaOperativa);
        List<MePerfilRuleDTO> GetByCriteriaPorUsuario(int area, string areaOperativa, string UserLog);
        List<MePerfilRuleDTO> ObtenerPuntosEjecutado();
        List<MePerfilRuleDTO> ObtenerPuntosDemanda();
        List<MePerfilRuleDTO> ObtenerPuntosScada();
        List<MePerfilRuleDTO> ObtenerPuntosScadaSP7();
        List<MePerfilRuleDTO> ObtenerPuntosMedidoresGeneracion();
        List<MePerfilRuleDTO> ObtenePuntosDemandaULyD();
        string ObtenerNombrePunto(int ptoMediCodi);
        string ObtenerNombrePuntoScada(int ptoMediCodi);
        string ObtenerNombrePuntoScadaSP7(int ptoMediCodi);
        string ObtenerNombrePuntoDemanda(int ptoMediCodi);
        string ObtenerNombrePuntoMedidoresGeneracion(int ptoMediCodi);
        string ObtenerNombrePuntoPR16(int ptoMediCodi);

        #region PR5
        List<MePerfilRuleDTO> ObtenerPuntosPR5(int origlectcodi);
        string ObtenerNombrePuntoPR5(int ptoMediCodi);
        #endregion

        //Assetec 20220308
        List<MePerfilRuleDTO> ObtenerPuntosServiciosAuxiliares();
    }
}
