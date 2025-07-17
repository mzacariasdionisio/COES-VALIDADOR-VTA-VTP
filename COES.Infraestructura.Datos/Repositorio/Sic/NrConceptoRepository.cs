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
    /// Clase de acceso a datos de la tabla NR_CONCEPTO
    /// </summary>
    public class NrConceptoRepository: RepositoryBase, INrConceptoRepository
    {
        public NrConceptoRepository(string strConn): base(strConn)
        {
        }

        NrConceptoHelper helper = new NrConceptoHelper();

        public int Save(NrConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, entity.Nrsmodcodi);
            dbProvider.AddInParameter(command, helper.Nrcptabrev, DbType.String, entity.Nrcptabrev);
            dbProvider.AddInParameter(command, helper.Nrcptdescripcion, DbType.String, entity.Nrcptdescripcion);
            dbProvider.AddInParameter(command, helper.Nrcptorden, DbType.Int32, entity.Nrcptorden);
            dbProvider.AddInParameter(command, helper.Nrcpteliminado, DbType.String, entity.Nrcpteliminado);
            dbProvider.AddInParameter(command, helper.Nrcptpadre, DbType.Int32, entity.Nrcptpadre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrsmodcodi, DbType.Int32, entity.Nrsmodcodi);
            dbProvider.AddInParameter(command, helper.Nrcptabrev, DbType.String, entity.Nrcptabrev);
            dbProvider.AddInParameter(command, helper.Nrcptdescripcion, DbType.String, entity.Nrcptdescripcion);
            dbProvider.AddInParameter(command, helper.Nrcptorden, DbType.Int32, entity.Nrcptorden);
            dbProvider.AddInParameter(command, helper.Nrcpteliminado, DbType.String, entity.Nrcpteliminado);
            dbProvider.AddInParameter(command, helper.Nrcptpadre, DbType.Int32, entity.Nrcptpadre);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, entity.Nrcptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrcptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, nrcptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public NrConceptoDTO GetById(int nrcptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, nrcptcodi);
            NrConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }



        public List<NrConceptoDTO> List()
        {
            List<NrConceptoDTO> entitys = new List<NrConceptoDTO>();
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

        public List<NrConceptoDTO> ListSubModuloConcepto()
        {
            List<NrConceptoDTO> entitys = new List<NrConceptoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaSubModuloConcepto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<NrConceptoDTO> GetByCriteria()
        {
            List<NrConceptoDTO> entitys = new List<NrConceptoDTO>();
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
        /// Graba los datos de la tabla NR_CONCEPTO
        /// </summary>
        public int SaveNrConceptoId(NrConceptoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrcptcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrcptcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<NrConceptoDTO> BuscarOperaciones(int nrsmodCodi, int nroPage, int pageSize)
        {
            List<NrConceptoDTO> entitys = new List<NrConceptoDTO>();
            String sql = String.Format(this.helper.ObtenerListado, nrsmodCodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrConceptoDTO entity = new NrConceptoDTO();

                    int iNrcptcodi = dr.GetOrdinal(this.helper.Nrcptcodi);
                    if (!dr.IsDBNull(iNrcptcodi)) entity.Nrcptcodi = Convert.ToInt32(dr.GetValue(iNrcptcodi));

                    int iNrsmodcodi = dr.GetOrdinal(this.helper.Nrsmodcodi);
                    if (!dr.IsDBNull(iNrsmodcodi)) entity.Nrsmodcodi = Convert.ToInt32(dr.GetValue(iNrsmodcodi));

                    int iNrcptabrev = dr.GetOrdinal(this.helper.Nrcptabrev);
                    if (!dr.IsDBNull(iNrcptabrev)) entity.Nrcptabrev = dr.GetString(iNrcptabrev);

                    int iNrcptdescripcion = dr.GetOrdinal(this.helper.Nrcptdescripcion);
                    if (!dr.IsDBNull(iNrcptdescripcion)) entity.Nrcptdescripcion = dr.GetString(iNrcptdescripcion);

                    int iNrcptorden = dr.GetOrdinal(this.helper.Nrcptorden);
                    if (!dr.IsDBNull(iNrcptorden)) entity.Nrcptorden = Convert.ToInt32(dr.GetValue(iNrcptorden));

                    int iNrcpteliminado = dr.GetOrdinal(this.helper.Nrcpteliminado);
                    if (!dr.IsDBNull(iNrcpteliminado)) entity.Nrcpteliminado = dr.GetString(iNrcpteliminado);

                    int iNrcptpadre = dr.GetOrdinal(this.helper.Nrcptpadre);
                    if (!dr.IsDBNull(iNrcptpadre)) entity.Nrcptpadre = Convert.ToInt32(dr.GetValue(iNrcptpadre));

                    int iNrsmodnombre = dr.GetOrdinal(this.helper.Nrsmodnombre);
                    if (!dr.IsDBNull(iNrsmodnombre)) entity.Nrsmodnombre = dr.GetString(iNrsmodnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int nrsmodCodi)
        {
            String sql = String.Format(this.helper.TotalRegistros, nrsmodCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
