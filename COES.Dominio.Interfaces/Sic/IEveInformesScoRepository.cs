using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INFORMES_SCO
    /// </summary>
    public interface IEveInformesScoRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(EveInformesScoDTO entity);
        List<EveInformesScoDTO> List(int Evencodi,int Envetapainforme);
        List<EveInformesScoDTO> ListInformesSco(int evencodi, int envetapainforme);
        void ActualizarInformePortalWeb(int eveinfcodi, string PortalWeb, string eveinfcodigo);
        EveInformesScoDTO ObtenerInformeSco(int eveinfcodi);
    }
}
