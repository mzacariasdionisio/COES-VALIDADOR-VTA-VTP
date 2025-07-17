using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ICodigoRetiroRelacionEquivalenciasRepository : IRepositoryBase
    {
        int Save(CodigoRetiroRelacionDTO entity);
        int Update(CodigoRetiroRelacionDTO entity);
        List<CodigoRetiroRelacionDetalleDTO> ListarRelacionCodigoRetiros(int nroPagina, int pageSize,
            int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo);
        List<CodigoRetiroRelacionDetalleDTO> ListarRelacionCodigoRetirosPorCodigo(int retrelcodi);
        int TotalRecordsRelacionCodigoRetiros(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo);
        CodigoRetiroRelacionDTO GetById(int id);
        decimal GetPoteCoincidenteByCodigoVtp(int pericodi, int recpotcodi, string codVTP, int emprcodi);

    }
}
