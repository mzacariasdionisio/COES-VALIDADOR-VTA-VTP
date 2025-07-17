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
    public class SolicitudCodigoDetalleRepository : RepositoryBase, ISolicitudCodigoDetalleRepository
    {
        public SolicitudCodigoDetalleRepository(string strConn) : base(strConn)
        {
        }

        SolicitudCodigoDetalleHelper helper = new SolicitudCodigoDetalleHelper();

        public int Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Solcodretdetcodi, DbType.Int32, id);

            int res = dbProvider.ExecuteNonQuery(command);

            if (res > 0)
            {
                DbCommand command2 = dbProvider.GetSqlStringCommand(helper.SqlDelete);
                dbProvider.AddInParameter(command2, helper.Solcodretdetcodi, DbType.Int32, id);
            }

            return res;
        }


        public List<SolicitudCodigoDetalleDTO> ListaRelacion(int barrCodiTra)
        {
            List<SolicitudCodigoDetalleDTO> entitys = new List<SolicitudCodigoDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaRelacion);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barrCodiTra);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SolicitudCodigoDetalleDTO entity = new SolicitudCodigoDetalleDTO();

                    int iBARECODI = dr.GetOrdinal(helper.Barecodi);
                    if (!dr.IsDBNull(iBARECODI)) entity.BarraRelacionCodi = dr.GetInt32(iBARECODI);

                    int iBARRCODITRA = dr.GetOrdinal(helper.Barrcoditra);
                    if (!dr.IsDBNull(iBARRCODITRA)) entity.Barracoditra = dr.GetInt32(iBARRCODITRA);

                    int iBARRCODISUM = dr.GetOrdinal(helper.Barrcodisum);
                    if (!dr.IsDBNull(iBARRCODISUM)) entity.Barracodisum = dr.GetInt32(iBARRCODISUM);

                    int iBARRNOMBSUM = dr.GetOrdinal(helper.Barrnombsum);
                    if (!dr.IsDBNull(iBARRNOMBSUM)) entity.Barranomsum = dr.GetString(iBARRNOMBSUM);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Save(SolicitudCodigoDetalleDTO entity)
        {
            int id = GetMaxId();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Solcodretdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Solcodretcodi, DbType.Int32, entity.Coresocodi);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, entity.Barracoditra);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, entity.Barracodisum);
            dbProvider.AddInParameter(command, helper.Solcodretdetnroregistro, DbType.Int32, entity.Coresdnumregistro);
            dbProvider.AddInParameter(command, helper.Solcodretdetusucreacion, DbType.String, entity.Coresdusuarioregistro);
            dbProvider.AddInParameter(command, helper.Solcodretdetfecregistro, DbType.DateTime, DateTime.Now.Date);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public int Update(SolicitudCodigoDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Solcodretdetnroregistro, DbType.Int32, entity.Coresdnumregistro);
            dbProvider.AddInParameter(command, helper.Solcodgenusumodificacion, DbType.String, entity.Coresdusuarioregistro);
            dbProvider.AddInParameter(command, helper.Solcodretdetfecmodificacion, DbType.DateTime, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.Solcodretdetcodi, DbType.Int32, entity.Coresdcodi);

            return dbProvider.ExecuteNonQuery(command); ;
        }

        public int GetMaxId()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int SaveSubDetalle(CodigoGeneradoDTO entity)
        {
            int id = GetMaxIdGenerado();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveGenerado);
            dbProvider.AddInParameter(command, helper.Solcodgencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Solcodretdetcodi, DbType.Int32, entity.Coresdcodi);
            dbProvider.AddInParameter(command, helper.Solcodgenestado, DbType.String, entity.Coregeestado);
            dbProvider.AddInParameter(command, helper.Solcodgencodvtp, DbType.String, entity.Coregecodigovtp);
            dbProvider.AddInParameter(command, helper.Solcodgenusucreacion, DbType.String, entity.Coregeusuregistro);
            dbProvider.AddInParameter(command, helper.Solcodgenfecregistro, DbType.DateTime, DateTime.Now.Date);
            dbProvider.ExecuteNonQuery(command); ;
            return id;
        }

        public int GetMaxIdGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int DeleteGenerado(int id, string usuario, string codEstado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteGenerado);
            dbProvider.AddInParameter(command, helper.Solcodgenestado, DbType.String, codEstado);
            dbProvider.AddInParameter(command, helper.Solcodgenusumodificacion, DbType.String, usuario);
            dbProvider.AddInParameter(command, helper.Solcodgenfecmodificacion, DbType.DateTime, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.Solcodgencodi, DbType.Int32, id);
            return dbProvider.ExecuteNonQuery(command);
        }


        public int GetMaxIdBR()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdBR);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<BarraDTO> ListarBarraSuministro()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarBarraSuministro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entity = new BarraDTO();

                    int iBARRCODI = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombre = dr.GetString(iBARRNOMBRE);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int SaveBR(BarraRelacionDTO entity)
        {
            int id = GetMaxIdBR();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveBR);
            dbProvider.AddInParameter(command, helper.Barecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, entity.BareBarrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.String, entity.BareBarrCodiSum);
            dbProvider.AddInParameter(command, helper.Bareusucreacion, DbType.String, entity.BareUsuarioRegistro);
            dbProvider.AddInParameter(command, helper.Barefeccreacion, DbType.DateTime, DateTime.Now.Date);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int DeleteBR(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteBR);
            dbProvider.AddInParameter(command, helper.Barecodi, DbType.Int32, id);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<SolicitudCodigoDetalleDTO> ListarDetalle(int id)
        {
            List<SolicitudCodigoDetalleDTO> entitys = new List<SolicitudCodigoDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarDetalle);
            dbProvider.AddInParameter(command, helper.Solcodretcodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SolicitudCodigoDetalleDTO entity = new SolicitudCodigoDetalleDTO();

                    int iSOLCODRETDETCODI = dr.GetOrdinal(helper.Solcodretdetcodi);
                    if (!dr.IsDBNull(iSOLCODRETDETCODI)) entity.Coresdcodi = dr.GetInt32(iSOLCODRETDETCODI);

                    int iSOLCODGENCODI = dr.GetOrdinal(helper.Solcodgencodi);
                    if (!dr.IsDBNull(iSOLCODGENCODI)) entity.Coregecodigo = dr.GetInt32(iSOLCODGENCODI);

                    int iBARRCODISUM = dr.GetOrdinal(helper.Barrcodisum);
                    if (!dr.IsDBNull(iBARRCODISUM)) entity.Barracodisum = dr.GetInt32(iBARRCODISUM);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.Barranomsum = dr.GetString(iBARRNOMBRE);

                    int iSOLCODRETDETNROREGISTRO = dr.GetOrdinal(helper.Solcodretdetnroregistro);
                    if (!dr.IsDBNull(iSOLCODRETDETNROREGISTRO)) entity.Coresdnumregistro = dr.GetInt32(iSOLCODRETDETNROREGISTRO);

                    int iSOLCODGENCODVTP = dr.GetOrdinal(helper.Solcodgencodvtp);
                    if (!dr.IsDBNull(iSOLCODGENCODVTP)) entity.Codigovtp = dr.GetString(iSOLCODGENCODVTP);

                    int iSOLCODGENESTADO = dr.GetOrdinal(helper.Solcodgenestado);
                    if (!dr.IsDBNull(iSOLCODGENESTADO)) entity.coregeestado = dr.GetString(iSOLCODGENESTADO);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroCodigosGenerados(int id)
        {
            int NroRegistros = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecordsGenerado);
            dbProvider.AddInParameter(command, helper.Solcodretdetcodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public int SolicitarBajarGenerado(CodigoGeneradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSolicitarBajarGenerado);

            dbProvider.AddInParameter(command, helper.Solcodgenestado, DbType.String, entity.Coregeestado);
            dbProvider.AddInParameter(command, helper.Solcodgenusumodificacion, DbType.String, entity.Coregeusuregistro);
            dbProvider.AddInParameter(command, helper.Solcodgenfecmodificacion, DbType.Date, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Solcodgencodi, DbType.Int32, entity.Coregecodi);
            return dbProvider.ExecuteNonQuery(command);
        }

        public CodigoGeneradoDTO GetByIdGenerado(int id)
        {
            CodigoGeneradoDTO entity = new CodigoGeneradoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdGenerado);
            dbProvider.AddInParameter(command, helper.Solcodgencodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    int iSOLCODGENCODI = dr.GetOrdinal(helper.Solcodgencodi);
                    if (!dr.IsDBNull(iSOLCODGENCODI)) entity.Coregecodi = dr.GetInt32(iSOLCODGENCODI);

                    int iSOLCODRETDETCODI = dr.GetOrdinal(helper.Solcodretdetcodi);
                    if (!dr.IsDBNull(iSOLCODRETDETCODI)) entity.Coresdcodi = dr.GetInt32(iSOLCODRETDETCODI);

                    int iSOLCODGENESTADO = dr.GetOrdinal(helper.Solcodgenestado);
                    if (!dr.IsDBNull(iSOLCODGENESTADO)) entity.Coregeestado = dr.GetString(iSOLCODGENESTADO);

                    int iSOLCODGENCODVTP = dr.GetOrdinal(helper.Solcodgencodvtp);
                    if (!dr.IsDBNull(iSOLCODGENCODVTP)) entity.Coregecodigovtp = dr.GetString(iSOLCODGENCODVTP);

                    if (dr[helper.Solcodretcodi] != null)
                    {
                        int iSOLCODRETCODI = dr.GetOrdinal(helper.Solcodretcodi);
                        if (!dr.IsDBNull(iSOLCODRETCODI)) entity.Coresocodi = dr.GetInt32(iSOLCODRETCODI);
                    }
                    if (dr[helper.Barrnombre] != null)
                    {
                        int iBARRNOMBRE = dr.GetOrdinal(helper.Barrnombre);
                        if (!dr.IsDBNull(iBARRNOMBRE)) entity.BarrNombSum = dr.GetString(iBARRNOMBRE);
                    }

                }
            }

            return entity;
        }

    }
}
