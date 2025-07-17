using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_CONTACTO
    /// </summary>
    public interface IWbContactoRepository
    {
        int Save(WbContactoDTO entity);
        void Update(WbContactoDTO entity);
        void Delete(int contaccodi);
        WbContactoDTO GetById(int contaccodi, string fuente);
        List<WbContactoDTO> List();
        List<WbContactoDTO> GetByCriteria(int? idTipoEmpresa, int? idEmpresa, string fuente, int? idComite, int? idProceso, int? idComiteLista);

        List<SiEmpresaDTO> ObtenerEmpresasContacto();
    }
}
