using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ICrEventoRepository
    {
        void Update(CrEventoDTO entity);
        List<CrEventoDTO> ListCrEventos(CrEventoDTO oEventoDTO);
        CrEventoDTO GetById(int crevencodi);
        int Save(CrEventoDTO entity);
        CrEventoDTO GetByAfecodi(int afecodi);
        void Delete(int crevencodi);
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int SaveR(CrEventoDTO entity, IDbConnection conn, DbTransaction tran);
    }
}
