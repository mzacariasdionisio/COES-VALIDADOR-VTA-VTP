using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RNT_CONFIGURACION
    /// </summary>
    public interface IRntConfiguracionRepository
    {
        int Save(RntConfiguracionDTO entity);
        void Update(RntConfiguracionDTO entity);
        void Delete(int configcodi);
        RntConfiguracionDTO GetById(int configcodi);
        List<RntConfiguracionDTO> List();
        List<RntConfiguracionDTO> ListNivelTension();
        List<RntConfiguracionDTO> ListComboNivelTension();
        RntConfiguracionDTO ListParametroRep(string parametro, string valor);
        List<RntConfiguracionDTO> ListComboConfiguracion(string parametro);
        List<RntConfiguracionDTO> GetListParametroRep(string atributo, string parametro);
        List<RntConfiguracionDTO> GetComboParametro(string atributo);
        List<RntConfiguracionDTO> GetByCriteria();
    }
}
