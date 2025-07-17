using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_ARCHIVO
    /// </summary>
    public interface IInArchivoRepository
    {
        int GetMaxId();
        int Save(InArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int inarchcodi, IDbConnection conn, DbTransaction tran);
        InArchivoDTO GetById(int inarchcodi);
        List<InArchivoDTO> List();
        List<InArchivoDTO> GetByCriteria(int Infvercodi, string infmmhoja);
        List<InArchivoDTO> ListByIntervencion(string intercodis);
        List<InArchivoDTO> ListByMensaje(string msgcodis);
        List<InArchivoDTO> ListBySustento(string instcodis);
        List<InArchivoDTO> ListarArchivoSinFormato(int tipo, string prefijo);
    }
}
