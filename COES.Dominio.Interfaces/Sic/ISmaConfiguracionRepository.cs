using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_CONFIGURACION
    /// </summary>
    public interface ISmaConfiguracionRepository
    {
        int Save(SmaConfiguracionDTO entity);
        void Update(SmaConfiguracionDTO entity);

        void Delete(string user, int confsmcorrelativo);
        SmaConfiguracionDTO GetById(int confsmcorrelativo);
        List<SmaConfiguracionDTO> List();
        List<SmaConfiguracionDTO> GetByAtributo(string atributo);
        SmaConfiguracionDTO GetValor(string atributo, string parametro);

        SmaConfiguracionDTO GetValorxID(int correlativo);

    }
}
