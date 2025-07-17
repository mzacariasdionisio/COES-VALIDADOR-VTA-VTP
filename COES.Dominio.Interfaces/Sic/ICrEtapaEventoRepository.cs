using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ICrEtapaEventoRepository
    {
        int save(CrEtapaEventoDTO entity);
        CrEtapaEventoDTO ObtenerCrEtapaEvento(int crevencodi, int cretapa);
        void Update(CrEtapaEventoDTO entity);
        List<CrEtapaEventoDTO> ListarCrEtapaEvento(int crevencodi);
        void Delete(int cretapacodi);
        List<CrEtapaEventoDTO> ObtenerCriterioxEtapaEvento(int crevencodi);
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int SaveR(CrEtapaEventoDTO entity, IDbConnection conn, DbTransaction tran);
    }
}
