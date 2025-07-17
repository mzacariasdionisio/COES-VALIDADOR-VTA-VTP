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
    /// Clase de acceso a datos de la tabla VCE_LOG_CARGA_DET
    /// </summary>
    public class VceLogCargaDetRepository: RepositoryBase, IVceLogCargaDetRepository
    {
        public VceLogCargaDetRepository(string strConn): base(strConn)
        {
        }

        VceLogCargaDetHelper helper = new VceLogCargaDetHelper();

        public void Save(VceLogCargaDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crlcdnroregistros, DbType.Int32, entity.Crlcdnroregistros);
            dbProvider.AddInParameter(command, helper.Crlcdusuimport, DbType.String, entity.Crlcdusuimport);
            dbProvider.AddInParameter(command, helper.Crlcdhoraimport, DbType.DateTime, entity.Crlcdhoraimport);
            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, entity.Crlcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceLogCargaDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crlcdnroregistros, DbType.Int32, entity.Crlcdnroregistros);
            dbProvider.AddInParameter(command, helper.Crlcdusuimport, DbType.String, entity.Crlcdusuimport);
            dbProvider.AddInParameter(command, helper.Crlcdhoraimport, DbType.DateTime, entity.Crlcdhoraimport);
            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, entity.Crlcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime crlcdhoraimport, int crlcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crlcdhoraimport, DbType.DateTime, crlcdhoraimport);
            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, crlcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteDetPeriodo(int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteDetPeriodo);

            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VceLogCargaDetDTO GetById(DateTime crlcdhoraimport, int crlcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crlcdhoraimport, DbType.DateTime, crlcdhoraimport);
            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, crlcccodi);
            VceLogCargaDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceLogCargaDetDTO> List()
        {
            List<VceLogCargaDetDTO> entitys = new List<VceLogCargaDetDTO>();
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

        public List<VceLogCargaDetDTO> GetByCriteria()
        {
            List<VceLogCargaDetDTO> entitys = new List<VceLogCargaDetDTO>();
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

        //- conpensaciones.JDEL - Inicio 03/01/2017: Cambio para atender el requerimiento.

        public List<VceLogCargaDetDTO> ListDetalle(int pecacodi)
        {
            List<VceLogCargaDetDTO> entitys = new List<VceLogCargaDetDTO>();
            string queryString = string.Format(helper.SqlListDetalle, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceLogCargaDetDTO entity = new VceLogCargaDetDTO();

                    int iCrlcccodi = dr.GetOrdinal(helper.Crlcccodi);
                    if (!dr.IsDBNull(iCrlcccodi)) entity.Crlcccodi = dr.GetInt32(iCrlcccodi);

                    int ipecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(ipecacodi)) entity.PecaCodi = dr.GetInt32(ipecacodi);

                    int iVceLcbNombTabla = dr.GetOrdinal(helper.Crlccnombtabla);
                    if (!dr.IsDBNull(iVceLcbNombTabla)) entity.Crlccnombtabla = dr.GetString(iVceLcbNombTabla);

                    int iVceLcbEntidad = dr.GetOrdinal(helper.Crlccentidad);
                    if (!dr.IsDBNull(iVceLcbEntidad)) entity.Crlccentidad = dr.GetString(iVceLcbEntidad);

                    int iVceLcbOrden = dr.GetOrdinal(helper.Crlccorden);
                    if (!dr.IsDBNull(iVceLcbOrden)) entity.Crlccorden = dr.GetInt32(iVceLcbOrden);

                    int iFecultactualizacion = dr.GetOrdinal(helper.Fecultactualizacion);
                    if (!dr.IsDBNull(iFecultactualizacion)) entity.Fecultactualizacion = dr.GetDateTime(iFecultactualizacion);

                    int iCrlcdhoraimport = dr.GetOrdinal(helper.Crlcdhoraimport);
                    if (!dr.IsDBNull(iCrlcdhoraimport)) entity.Crlcdhoraimport = dr.GetDateTime(iCrlcdhoraimport);

                    int iCrlcdusuimport = dr.GetOrdinal(helper.Crlcdusuimport);
                    if (!dr.IsDBNull(iCrlcdusuimport)) entity.Crlcdusuimport = dr.GetString(iCrlcdusuimport);

                    int iCrlcdnroregistros = dr.GetOrdinal(helper.Crlcdnroregistros);
                    if (!dr.IsDBNull(iCrlcdnroregistros)) entity.Crlcdnroregistros = dr.GetInt32(iCrlcdnroregistros);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void SaveDetalle(int codigo, string usuario, string tabla, int pecacodi)
        {
            string queryString = string.Format(helper.SqlSaveDetalle, codigo, usuario, tabla, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        //- JDEL Fin
        
    }
}
