using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CB_FICHA_ITEM
    /// </summary>
    public class CbFichaItemRepository : RepositoryBase, ICbFichaItemRepository
    {
        public CbFichaItemRepository(string strConn) : base(strConn)
        {
        }

        CbFichaItemHelper helper = new CbFichaItemHelper();

        public int Save(CbFichaItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cbftcodi, DbType.Int32, entity.Cbftcodi);
            dbProvider.AddInParameter(command, helper.Cbftitcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbftitesseccion, DbType.String, entity.Cbftitesseccion);
            dbProvider.AddInParameter(command, helper.Cbftitnombre, DbType.String, entity.Cbftitnombre);
            dbProvider.AddInParameter(command, helper.Cbftitnumeral, DbType.String, entity.Cbftitnumeral);
            dbProvider.AddInParameter(command, helper.Cbftitformula, DbType.String, entity.Cbftitformula);
            dbProvider.AddInParameter(command, helper.Cbftitinstructivo, DbType.String, entity.Cbftitinstructivo);
            dbProvider.AddInParameter(command, helper.Cbftittipodato, DbType.String, entity.Cbftittipodato);
            dbProvider.AddInParameter(command, helper.Cbftitabrev, DbType.String, entity.Cbftitabrev);
            dbProvider.AddInParameter(command, helper.Cbftitconfidencial, DbType.String, entity.Cbftitconfidencial);
            dbProvider.AddInParameter(command, helper.Cbftittipocelda, DbType.String, entity.Cbftittipocelda);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo1, DbType.String, entity.Cbftitceldatipo1);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo2, DbType.String, entity.Cbftitceldatipo2);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo3, DbType.String, entity.Cbftitceldatipo3);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo4, DbType.String, entity.Cbftitceldatipo4);
            dbProvider.AddInParameter(command, helper.Cbftitcnp0, DbType.Int32, entity.Cbftitcnp0);
            dbProvider.AddInParameter(command, helper.Cbftitcnp1, DbType.Int32, entity.Cbftitcnp1);
            dbProvider.AddInParameter(command, helper.Cbftitcnp2, DbType.Int32, entity.Cbftitcnp2);
            dbProvider.AddInParameter(command, helper.Cbftitcnp3, DbType.Int32, entity.Cbftitcnp3);
            dbProvider.AddInParameter(command, helper.Cbftitcnp4, DbType.Int32, entity.Cbftitcnp4);
            dbProvider.AddInParameter(command, helper.Cbftitcnp5, DbType.Int32, entity.Cbftitcnp5);
            dbProvider.AddInParameter(command, helper.Cbftitcnp6, DbType.Int32, entity.Cbftitcnp6);
            dbProvider.AddInParameter(command, helper.Cbftitoperacion, DbType.String, entity.Cbftitoperacion);
            dbProvider.AddInParameter(command, helper.Cbftitactivo, DbType.Int32, entity.Cbftitactivo);
            dbProvider.AddInParameter(command, helper.Cbftitusucreacion, DbType.String, entity.Cbftitusucreacion);
            dbProvider.AddInParameter(command, helper.Cbftitfeccreacion, DbType.DateTime, entity.Cbftitfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbftitusumodificacion, DbType.String, entity.Cbftitusumodificacion);
            dbProvider.AddInParameter(command, helper.Cbftitfecmodificacion, DbType.DateTime, entity.Cbftitfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CbFichaItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cbftcodi, DbType.Int32, entity.Cbftcodi);
            dbProvider.AddInParameter(command, helper.Ccombcodi, DbType.Int32, entity.Ccombcodi);
            dbProvider.AddInParameter(command, helper.Cbftitesseccion, DbType.String, entity.Cbftitesseccion);
            dbProvider.AddInParameter(command, helper.Cbftitnombre, DbType.String, entity.Cbftitnombre);
            dbProvider.AddInParameter(command, helper.Cbftitnumeral, DbType.String, entity.Cbftitnumeral);
            dbProvider.AddInParameter(command, helper.Cbftitformula, DbType.String, entity.Cbftitformula);
            dbProvider.AddInParameter(command, helper.Cbftitinstructivo, DbType.String, entity.Cbftitinstructivo);
            dbProvider.AddInParameter(command, helper.Cbftittipodato, DbType.String, entity.Cbftittipodato);
            dbProvider.AddInParameter(command, helper.Cbftitabrev, DbType.String, entity.Cbftitabrev);
            dbProvider.AddInParameter(command, helper.Cbftitconfidencial, DbType.String, entity.Cbftitconfidencial);
            dbProvider.AddInParameter(command, helper.Cbftittipocelda, DbType.String, entity.Cbftittipocelda);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo1, DbType.String, entity.Cbftitceldatipo1);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo2, DbType.String, entity.Cbftitceldatipo2);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo3, DbType.String, entity.Cbftitceldatipo3);
            dbProvider.AddInParameter(command, helper.Cbftitceldatipo4, DbType.String, entity.Cbftitceldatipo4);
            dbProvider.AddInParameter(command, helper.Cbftitcnp0, DbType.Int32, entity.Cbftitcnp0);
            dbProvider.AddInParameter(command, helper.Cbftitcnp1, DbType.Int32, entity.Cbftitcnp1);
            dbProvider.AddInParameter(command, helper.Cbftitcnp2, DbType.Int32, entity.Cbftitcnp2);
            dbProvider.AddInParameter(command, helper.Cbftitcnp3, DbType.Int32, entity.Cbftitcnp3);
            dbProvider.AddInParameter(command, helper.Cbftitcnp4, DbType.Int32, entity.Cbftitcnp4);
            dbProvider.AddInParameter(command, helper.Cbftitcnp5, DbType.Int32, entity.Cbftitcnp5);
            dbProvider.AddInParameter(command, helper.Cbftitcnp6, DbType.Int32, entity.Cbftitcnp6);
            dbProvider.AddInParameter(command, helper.Cbftitoperacion, DbType.String, entity.Cbftitoperacion);
            dbProvider.AddInParameter(command, helper.Cbftitactivo, DbType.Int32, entity.Cbftitactivo);
            dbProvider.AddInParameter(command, helper.Cbftitusucreacion, DbType.String, entity.Cbftitusucreacion);
            dbProvider.AddInParameter(command, helper.Cbftitfeccreacion, DbType.DateTime, entity.Cbftitfeccreacion);
            dbProvider.AddInParameter(command, helper.Cbftitusumodificacion, DbType.String, entity.Cbftitusumodificacion);
            dbProvider.AddInParameter(command, helper.Cbftitfecmodificacion, DbType.DateTime, entity.Cbftitfecmodificacion);

            dbProvider.AddInParameter(command, helper.Cbftitcodi, DbType.Int32, entity.Cbftitcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cbftitcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cbftitcodi, DbType.Int32, cbftitcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CbFichaItemDTO GetById(int cbftitcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cbftitcodi, DbType.Int32, cbftitcodi);
            CbFichaItemDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CbFichaItemDTO> List()
        {
            List<CbFichaItemDTO> entitys = new List<CbFichaItemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CbFichaItemDTO> GetByCriteria(int cbftcodi)
        {
            List<CbFichaItemDTO> entitys = new List<CbFichaItemDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, cbftcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CbFichaItemDTO entity = helper.Create(dr);

                    int iCcombnombre = dr.GetOrdinal(helper.Ccombnombre);
                    if (!dr.IsDBNull(iCcombnombre)) entity.Ccombnombre = dr.GetString(iCcombnombre);

                    int iCcombunidad = dr.GetOrdinal(helper.Ccombunidad);
                    if (!dr.IsDBNull(iCcombunidad)) entity.Ccombunidad = dr.GetString(iCcombunidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
