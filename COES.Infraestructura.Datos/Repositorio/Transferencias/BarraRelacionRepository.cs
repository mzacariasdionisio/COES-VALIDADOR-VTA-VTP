using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class BarraRelacionRepository: RepositoryBase,IBarraRelacionRepository
    {
        public BarraRelacionRepository(string strConn) : base(strConn)
        {
        }

        BarraRelacionHelper helper = new BarraRelacionHelper();

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
        public int Save(BarraRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Barecodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, entity.BareBarrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, entity.BareBarrCodiSum);
            dbProvider.AddInParameter(command, helper.Bareestado, DbType.String, entity.BareEstado);
            dbProvider.AddInParameter(command, helper.Bareusucreacion, DbType.String, entity.BareUsuarioRegistro);
            dbProvider.AddInParameter(command, helper.Barefeccreacion, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int Delete(BarraRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Bareestado, DbType.String, entity.BareEstado);
            dbProvider.AddInParameter(command, helper.Bareusumodificacion, DbType.String, entity.BareUsuarioRegistro);
            dbProvider.AddInParameter(command, helper.Barefecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Barecodi, DbType.Int32, entity.BareCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<BarraRelacionDTO> ListaRelacion(int id)
        {
            List<BarraRelacionDTO> entitys = new List<BarraRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaRelacion);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraRelacionDTO entity = new BarraRelacionDTO();

                    int iBarrcodi = dr.GetOrdinal(helper.Barecodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BareCodi = dr.GetInt32(iBarrcodi);

                    int iBarrcoditra = dr.GetOrdinal(helper.Barrcoditra);
                    if (!dr.IsDBNull(iBarrcoditra)) entity.BareBarrCodiTra = dr.GetInt32(iBarrcoditra);

                    int iBarrcodisum = dr.GetOrdinal(helper.Barrcodisum);
                    if (!dr.IsDBNull(iBarrcodisum)) entity.BareBarrCodiSum = dr.GetInt32(iBarrcodisum);

                    int iBarrnombsum = dr.GetOrdinal(helper.Barrnombsum);
                    if (!dr.IsDBNull(iBarrnombsum)) entity.BarrNombSum = dr.GetString(iBarrnombsum);

                    int iBarrTension = dr.GetOrdinal(helper.BarrTension);
                    if (!dr.IsDBNull(iBarrTension)) entity.BarrTension = dr.GetString(iBarrTension);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public bool ExisteRelacionBarra(BarraRelacionDTO entity)
        {
            List<BarraRelacionDTO> entitys = new List<BarraRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlExisteRelacionBarra);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, entity.BareBarrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, entity.BareBarrCodiSum);

            int result = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    result = dr.GetInt32(0);
                }
            }

            return result>0;
        }

    }
}
