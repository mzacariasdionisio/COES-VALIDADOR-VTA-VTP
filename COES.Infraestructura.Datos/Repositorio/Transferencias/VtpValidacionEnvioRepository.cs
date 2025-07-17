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
    /// Clase de acceso a datos de la tabla VTP_VALIDACION_ENVIO
    /// </summary>
    public class VtpValidacionEnvioRepository : RepositoryBase, IVtpValidacionEnvioRepository
    {
        public VtpValidacionEnvioRepository(string strConn) : base(strConn)
        {
        }

        VtpValidacionEnvioHelper helper = new VtpValidacionEnvioHelper();

        public int Save(VtpValidacionEnvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.VaenCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PegrCodi, DbType.Int32, entity.PegrCodi);
            dbProvider.AddInParameter(command, helper.PegrdCodi, DbType.Int32, entity.PegrdCodi);
            dbProvider.AddInParameter(command, helper.VaenTipoValidacion, DbType.String, entity.VaenTipoValidacion);
            dbProvider.AddInParameter(command, helper.VaenNomCliente, DbType.String, entity.VaenNomCliente);
            dbProvider.AddInParameter(command, helper.VaenCodVtea, DbType.String, entity.VaenCodVtea);
            dbProvider.AddInParameter(command, helper.VaenCodVtp, DbType.String, entity.VaenCodVtp);
            dbProvider.AddInParameter(command, helper.VaenBarraTra, DbType.String, entity.VaenBarraTra);
            dbProvider.AddInParameter(command, helper.VaenBarraSum, DbType.String, entity.VaenBarraSum);
            dbProvider.AddInParameter(command, helper.VaenValorVtea, DbType.Decimal, entity.VaenValorVtea);
            dbProvider.AddInParameter(command, helper.VaenValorVtp, DbType.Decimal, entity.VaenValorVtp);
            dbProvider.AddInParameter(command, helper.VaenValorReportado, DbType.Decimal, entity.VaenValorReportado);
            dbProvider.AddInParameter(command, helper.VaenValorCoes, DbType.Decimal, entity.VaenValorCoes);
            dbProvider.AddInParameter(command, helper.VaenVariacion, DbType.Decimal, entity.VaenVariacion);
            dbProvider.AddInParameter(command, helper.VaenRevisionAnterior, DbType.Decimal, entity.VaenRevisionAnterior);
            dbProvider.AddInParameter(command, helper.VaenPrecioPotencia, DbType.Decimal, entity.VaenPrecioPotencia);
            dbProvider.AddInParameter(command, helper.VaenPeajeUnitario, DbType.Decimal, entity.VaenPeajeUnitario);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<VtpValidacionEnvioDTO> GetValidacionEnvioByPegrcodi(int pegrcodi)
        {
            List<VtpValidacionEnvioDTO> entitys = new List<VtpValidacionEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetValidacionEnvioByPegrcodi);
            dbProvider.AddInParameter(command, helper.PegrCodi, DbType.Int32, pegrcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpValidacionEnvioDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
