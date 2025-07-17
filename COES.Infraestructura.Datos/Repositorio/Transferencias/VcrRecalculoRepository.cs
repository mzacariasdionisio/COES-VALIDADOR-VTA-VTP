using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_RECALCULO
    /// </summary>
    public class VcrRecalculoRepository: RepositoryBase, IVcrRecalculoRepository
    {
        public VcrRecalculoRepository(string strConn): base(strConn)
        {
        }

        VcrRecalculoHelper helper = new VcrRecalculoHelper();

        public int Save(VcrRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecanombre, DbType.String, entity.Vcrecanombre);
            dbProvider.AddInParameter(command, helper.Vcrecaversion, DbType.Int32, entity.Vcrecaversion);
            dbProvider.AddInParameter(command, helper.Vcrecakcalidad, DbType.Decimal, entity.Vcrecakcalidad);
            dbProvider.AddInParameter(command, helper.Vcrecapaosinergmin, DbType.Decimal, entity.Vcrecapaosinergmin);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);
            dbProvider.AddInParameter(command, helper.Vcrecacomentario, DbType.String, entity.Vcrecacomentario);
            dbProvider.AddInParameter(command, helper.Vcrecaestado, DbType.String, entity.Vcrecaestado);
            dbProvider.AddInParameter(command, helper.Vcrecacodidestino, DbType.Int32, entity.Vcrecacodidestino);
            dbProvider.AddInParameter(command, helper.Vcrecausucreacion, DbType.String, entity.Vcrecausucreacion);
            dbProvider.AddInParameter(command, helper.Vcrecafeccreacion, DbType.DateTime, entity.Vcrecafeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrecausumodificacion, DbType.String, entity.Vcrecausumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrecafecmodificacion, DbType.DateTime, entity.Vcrecafecmodificacion);
            //202012
            dbProvider.AddInParameter(command, helper.Vcrecaresaprimsig, DbType.Decimal, entity.Vcrecaresaprimsig);
            dbProvider.AddInParameter(command, helper.Vcrecacostoprns, DbType.Decimal, entity.Vcrecacostoprns);
            dbProvider.AddInParameter(command, helper.Vcrecafactcumpl, DbType.Decimal, entity.Vcrecafactcumpl);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecanombre, DbType.String, entity.Vcrecanombre);
            dbProvider.AddInParameter(command, helper.Vcrecaversion, DbType.Int32, entity.Vcrecaversion);
            dbProvider.AddInParameter(command, helper.Vcrecakcalidad, DbType.Decimal, entity.Vcrecakcalidad);
            dbProvider.AddInParameter(command, helper.Vcrecapaosinergmin, DbType.Decimal, entity.Vcrecapaosinergmin);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);
            dbProvider.AddInParameter(command, helper.Vcrecacomentario, DbType.String, entity.Vcrecacomentario);
            dbProvider.AddInParameter(command, helper.Vcrecaestado, DbType.String, entity.Vcrecaestado);
            dbProvider.AddInParameter(command, helper.Vcrecacodidestino, DbType.Int32, entity.Vcrecacodidestino);
            dbProvider.AddInParameter(command, helper.Vcrecausucreacion, DbType.String, entity.Vcrecausucreacion);
            dbProvider.AddInParameter(command, helper.Vcrecafeccreacion, DbType.DateTime, entity.Vcrecafeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrecausumodificacion, DbType.String, entity.Vcrecausumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrecafecmodificacion, DbType.DateTime, entity.Vcrecafecmodificacion);
            //202012
            dbProvider.AddInParameter(command, helper.Vcrecaresaprimsig, DbType.Decimal, entity.Vcrecaresaprimsig);
            dbProvider.AddInParameter(command, helper.Vcrecacostoprns, DbType.Decimal, entity.Vcrecacostoprns);
            dbProvider.AddInParameter(command, helper.Vcrecafactcumpl, DbType.Decimal, entity.Vcrecafactcumpl);
            //--
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrRecalculoDTO GetById(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            VcrRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrRecalculoDTO GetByIncumplimiento(int vcrinccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIncumplimiento);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            VcrRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrRecalculoDTO GetByIdView(int pericodi, int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            VcrRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                    int iPerinombredestino = dr.GetOrdinal(this.helper.Perinombredestino);
                    if (!dr.IsDBNull(iPerinombredestino)) entity.Perinombredestino = dr.GetString(iPerinombredestino);
                }
            }

            return entity;
        }

        public VcrRecalculoDTO GetByIdViewIndex(int pericodi, int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdViewIndex);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            VcrRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                    int iVcrdsrnombre = dr.GetOrdinal(this.helper.Vcrdsrnombre);
                    if (!dr.IsDBNull(iVcrdsrnombre)) entity.Vcrdsrnombre = dr.GetString(iVcrdsrnombre);

                    int iVcrincnombre = dr.GetOrdinal(this.helper.Vcrincnombre);
                    if (!dr.IsDBNull(iVcrincnombre)) entity.Vcrincnombre = dr.GetString(iVcrincnombre);
                }
            }

            return entity;
        }

        public VcrRecalculoDTO GetByIdUpDate(int pericodi, int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdUpDate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, vcrecacodi);
            VcrRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrRecalculoDTO> List(int pericodi)
        {
            List<VcrRecalculoDTO> entitys = new List<VcrRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrRecalculoDTO> ListReg(int vcrecacodi)
        {
            List<VcrRecalculoDTO> entitys = new List<VcrRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListInsert);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.String, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrRecalculoDTO entity = helper.Create(dr);

                    int iPerionombre = dr.GetOrdinal(this.helper.Vcrecanombre);
                    if (!dr.IsDBNull(iPerionombre)) entity.Perinombre = dr.GetString(iPerionombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrRecalculoDTO> ListAllRecalculos()
        {
            List<VcrRecalculoDTO> entitys = new List<VcrRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAllView);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrRecalculoDTO entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    int iRecanombre = dr.GetOrdinal(this.helper.Recanombre);
                    if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

                    int iVcrdsrnombre = dr.GetOrdinal(this.helper.Vcrdsrnombre);
                    if (!dr.IsDBNull(iVcrdsrnombre)) entity.Vcrdsrnombre = dr.GetString(iVcrdsrnombre);

                    int iVcrincnombre = dr.GetOrdinal(this.helper.Vcrincnombre);
                    if (!dr.IsDBNull(iVcrincnombre)) entity.Vcrincnombre = dr.GetString(iVcrincnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrRecalculoDTO> GetByCriteria()
        {
            List<VcrRecalculoDTO> entitys = new List<VcrRecalculoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

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
