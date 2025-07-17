using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class InfoDesviacionRepository : RepositoryBase, IInfoDesviacionRepository
    {
        public InfoDesviacionRepository(string strConn) : base(strConn)
        {
        }

        InfoDesviacionHelper helper = new InfoDesviacionHelper();

        public List<InfoDesviacionDTO> GetByListaCodigo(int iPeriCodi, int iVersion, int iBarrCodi)
        {
            List<InfoDesviacionDTO> entitys = new List<InfoDesviacionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListaCodigo);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, iBarrCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public InfoDesviacionDTO GetByEnergiaByBarraCodigo(int iPeriCodi, int iVersion, int iBarrCodi, string sCodigo)
        {
            InfoDesviacionDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEnergiaByBarraCodigo);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, iBarrCodi);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, sCodigo);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

    }
}
