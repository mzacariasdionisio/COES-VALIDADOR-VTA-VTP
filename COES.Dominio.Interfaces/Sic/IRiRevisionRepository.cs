using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;
namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_REVISION
    /// </summary>
    public interface IRiRevisionRepository
    {
        int Save(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateEstadoRegistroInactivo(RiRevisionDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int revicodi);
        RiRevisionDTO GetById(int revicodi);
        List<RiRevisionDTO> List();
        List<RiRevisionDTO> GetByCriteria();
        List<SiEmpresaDTO> ListByEstadoAndTipEmp(string estado, int tipemprcodi, string nombre, int page, int pagesize);
        int ObtenerTotalListByEstadoAndTipEmp(string estado, int tipemprcodi, string nombre);
        int DarConformidad(int revicodi);
        int DarNotificar(int revicodi);
        int DarTerminar(int revicodi);
        int RevAsistente(int revicodi);
        RiRevisionDTO GetByEtapa(int etrvcodi);
    }
}
