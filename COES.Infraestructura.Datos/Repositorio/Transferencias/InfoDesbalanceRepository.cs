using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class InfoDesbalanceRepository : RepositoryBase, IInfoDesbalanceRepository
    {                 
        public InfoDesbalanceRepository(string strConn) : base(strConn)
        {
        }

        InfoDesbalanceHelper helper = new InfoDesbalanceHelper();

        public List<InfoDesbalanceDTO> GetByListaBarrasTrans(int iPeriCodi, int iVersion)
        {
            List<InfoDesbalanceDTO> entitys = new List<InfoDesbalanceDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListaBarrasTrans);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, iVersion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InfoDesbalanceDTO entity = new InfoDesbalanceDTO();
                    int iBarrCodi = dr.GetOrdinal(helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);
                    int iBarrTransferencia = dr.GetOrdinal(helper.BarrTransferencia);
                    if (!dr.IsDBNull(iBarrTransferencia)) entity.BarrTransferencia = dr.GetString(iBarrTransferencia);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<InfoDesbalanceDTO> GetByListaInfoDesbalanceByBarra(int iPeriCodi, int iVersion, int iBarrCodi)
        {
            List<InfoDesbalanceDTO> entitys = new List<InfoDesbalanceDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListaInfoDesbalanceByBarra);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
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
 
    }
}
