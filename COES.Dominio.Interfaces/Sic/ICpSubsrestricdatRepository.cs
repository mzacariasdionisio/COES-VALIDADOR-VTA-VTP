using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_SUBSRESTRICDAT
    /// </summary>
    public interface ICpSubsrestricdatRepository
    {
        List<CpSubrestricdatDTO> ListarDatosRestriccion(int topcodi, int restriccodi);
        List<CpSubrestricdatDTO> List(int topcodi);
        //YUpana Continuo
        List<CpSubrestricdatDTO> ListadeSubRestriccionCategoria(int topcodi, int catcodi);
        List<CpSubrestricdatDTO> ListarRecursosEnSubRestriccion(int pTopologia, int sRestriccion);
        List<CpSubrestricdatDTO> ListarDatosSubRestriccion(int topcodi, int srestcodi);
        void CrearCopia(int topcodi1, int topcodi2);
    }
}
