using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PRUEBA
    /// </summary>
    public interface ISiPruebaRepository
    {
       
        void Update(SiPruebaDTO entity);
        void Delete(string pruebacodi);
        SiPruebaDTO GetById(string pruebacodi);
        List<SiPruebaDTO> List();
        List<SiPruebaDTO> GetByCriteria();
        List<SiPruebaDTO> BuscarPorNombre(string nombre);
    }
}
