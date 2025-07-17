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
    /// Clase de acceso a datos de la tabla AGC_CONTROL_PUNTO
    /// </summary>
    public class AgcControlPuntoRepository: RepositoryBase, IAgcControlPuntoRepository
    {
        public AgcControlPuntoRepository(string strConn): base(strConn)
        {
        }

        AgcControlPuntoHelper helper = new AgcControlPuntoHelper();

        public int Save(AgcControlPuntoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Agccpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, entity.Agcccodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Agccpb2, DbType.String, entity.Agccpb2);
            dbProvider.AddInParameter(command, helper.Agccpb3, DbType.String, entity.Agccpb3);
            dbProvider.AddInParameter(command, helper.Agccpusucreacion, DbType.String, entity.Agccpusucreacion);
            dbProvider.AddInParameter(command, helper.Agccpfeccreacion, DbType.DateTime, entity.Agccpfeccreacion);
            dbProvider.AddInParameter(command, helper.Agccpusumodificacion, DbType.String, entity.Agccpusumodificacion);
            dbProvider.AddInParameter(command, helper.Agccpfecmodificacion, DbType.DateTime, entity.Agccpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AgcControlPuntoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, entity.Agcccodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Agccpb2, DbType.String, entity.Agccpb2);
            dbProvider.AddInParameter(command, helper.Agccpb3, DbType.String, entity.Agccpb3);
            dbProvider.AddInParameter(command, helper.Agccpusucreacion, DbType.String, entity.Agccpusucreacion);
            dbProvider.AddInParameter(command, helper.Agccpfeccreacion, DbType.DateTime, entity.Agccpfeccreacion);
            dbProvider.AddInParameter(command, helper.Agccpusumodificacion, DbType.String, entity.Agccpusumodificacion);
            dbProvider.AddInParameter(command, helper.Agccpfecmodificacion, DbType.DateTime, entity.Agccpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Agccpcodi, DbType.Int32, entity.Agccpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int agcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, agcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<AgcControlPuntoDTO> GetById(int agcccodi)
        {
            List<AgcControlPuntoDTO> entitys = new List<AgcControlPuntoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Agccpcodi, DbType.Int32, agcccodi);
            //AgcControlPuntoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entity = helper.Create(dr);
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AgcControlPuntoDTO> List()
        {
            List<AgcControlPuntoDTO> entitys = new List<AgcControlPuntoDTO>();
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

        public List<AgcControlPuntoDTO> GetByCriteria()
        {
            List<AgcControlPuntoDTO> entitys = new List<AgcControlPuntoDTO>();
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
        /// <summary>
        /// Graba los datos de la tabla AGC_CONTROL_PUNTO
        /// </summary>
        public int SaveAgcControlPuntoId(AgcControlPuntoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Agccpcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Agccpcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<AgcControlPuntoDTO> BuscarOperaciones(int agccCodi,int ptomediCodi,int equiCodi, int nroPage, int pageSize)
        {
            List<AgcControlPuntoDTO> entitys = new List<AgcControlPuntoDTO>();
            String sql = String.Format(this.helper.ObtenerListado, agccCodi,ptomediCodi,equiCodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AgcControlPuntoDTO entity = new AgcControlPuntoDTO();

                    int iAgccpcodi = dr.GetOrdinal(this.helper.Agccpcodi);
                    if (!dr.IsDBNull(iAgccpcodi)) entity.Agccpcodi = Convert.ToInt32(dr.GetValue(iAgccpcodi));

                    int iAgcccodi = dr.GetOrdinal(this.helper.Agcccodi);
                    if (!dr.IsDBNull(iAgcccodi)) entity.Agcccodi = Convert.ToInt32(dr.GetValue(iAgcccodi));

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAgccpb2 = dr.GetOrdinal(this.helper.Agccpb2);
                    if (!dr.IsDBNull(iAgccpb2)) entity.Agccpb2 = dr.GetString(iAgccpb2);

                    int iAgccpb3 = dr.GetOrdinal(this.helper.Agccpb3);
                    if (!dr.IsDBNull(iAgccpb3)) entity.Agccpb3 = dr.GetString(iAgccpb3);

                    int iAgccpusucreacion = dr.GetOrdinal(this.helper.Agccpusucreacion);
                    if (!dr.IsDBNull(iAgccpusucreacion)) entity.Agccpusucreacion = dr.GetString(iAgccpusucreacion);

                    int iAgccpfeccreacion = dr.GetOrdinal(this.helper.Agccpfeccreacion);
                    if (!dr.IsDBNull(iAgccpfeccreacion)) entity.Agccpfeccreacion = dr.GetDateTime(iAgccpfeccreacion);

                    int iAgccpusumodificacion = dr.GetOrdinal(this.helper.Agccpusumodificacion);
                    if (!dr.IsDBNull(iAgccpusumodificacion)) entity.Agccpusumodificacion = dr.GetString(iAgccpusumodificacion);

                    int iAgccpfecmodificacion = dr.GetOrdinal(this.helper.Agccpfecmodificacion);
                    if (!dr.IsDBNull(iAgccpfecmodificacion)) entity.Agccpfecmodificacion = dr.GetDateTime(iAgccpfecmodificacion);

                    int iAgcctipo = dr.GetOrdinal(this.helper.Agcctipo);
                    if (!dr.IsDBNull(iAgcctipo)) entity.Agcctipo = dr.GetString(iAgcctipo);

                    int iPtomedibarranomb = dr.GetOrdinal(this.helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int agccCodi,int ptomediCodi,int equiCodi)
        {
            List<AgcControlPuntoDTO> entitys = new List<AgcControlPuntoDTO>();
            String sql = String.Format(this.helper.TotalRegistros, agccCodi,ptomediCodi,equiCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        public List<AgcControlPuntoDTO> ObtenerPorControl(int agccCodi)
        {
            List<AgcControlPuntoDTO> entitys = new List<AgcControlPuntoDTO>();
            String sql = String.Format(this.helper.SqlObtenerPorControl, agccCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
