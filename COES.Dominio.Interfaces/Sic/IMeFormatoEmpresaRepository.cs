using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_FORMATO_EMPRESA
    /// </summary>
    public interface IMeFormatoEmpresaRepository
    {
        void Save(MeFormatoEmpresaDTO entity);
        void Update(MeFormatoEmpresaDTO entity);
        void Delete(int formatcodi, int emprcodi);
        MeFormatoEmpresaDTO GetById(int formatcodi, int emprcodi);
        List<MeFormatoEmpresaDTO> List();
        List<MeFormatoEmpresaDTO> GetByCriteria();

        //- remision-pr16.JDEL - Inicio 19/05/2016: Cambio para atender el requerimiento del PR16.
        List<MeFormatoEmpresaDTO> ObtenerListaPeriodoEnvio(string fecha, int empresa, int formato);
        //- JDEL Fin
        
    }
}
