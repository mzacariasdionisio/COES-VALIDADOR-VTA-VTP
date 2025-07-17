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
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_BARRA
    /// </summary>
    public class BarraRepository : RepositoryBase, IBarraRepository
    {
        public BarraRepository(string strConn) : base(strConn)
        {
        }

        BarraHelper helper = new BarraHelper();

        public int Save(BarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.Barrnombre, DbType.String, entity.BarrNombre);
            dbProvider.AddInParameter(command, helper.Barrtension, DbType.String, entity.BarrTension);
            dbProvider.AddInParameter(command, helper.Barrpuntosumirer, DbType.String, entity.BarrPuntoSumirer);
            dbProvider.AddInParameter(command, helper.Barrbarrabgr, DbType.String, entity.BarrBarraBgr);
            dbProvider.AddInParameter(command, helper.Barrestado, DbType.String, entity.BarrEstado);
            dbProvider.AddInParameter(command, helper.Barrflagbarrtran, DbType.String, entity.BarrFlagBarrTran);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, entity.BarrNombBarrTran);
            dbProvider.AddInParameter(command, helper.Barrflagdesblance, DbType.String, entity.BarrFlagDesbalance);
            dbProvider.AddInParameter(command, helper.Barrusername, DbType.String, entity.BarrUserName);
            dbProvider.AddInParameter(command, helper.Barrfecins, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Barrfactorperdida, DbType.Decimal, entity.BarrFactorPerdida);
            dbProvider.AddInParameter(command, helper.OsinergCodi, DbType.String, entity.OsinergCodi);
            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(BarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barrnombre, DbType.String, entity.BarrNombre);
            dbProvider.AddInParameter(command, helper.Barrtension, DbType.String, entity.BarrTension);
            dbProvider.AddInParameter(command, helper.Barrpuntosumirer, DbType.String, entity.BarrPuntoSumirer);
            dbProvider.AddInParameter(command, helper.Barrbarrabgr, DbType.String, entity.BarrBarraBgr);
            dbProvider.AddInParameter(command, helper.Barrestado, DbType.String, entity.BarrEstado);
            dbProvider.AddInParameter(command, helper.Barrflagbarrtran, DbType.String, entity.BarrFlagBarrTran);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, entity.BarrNombBarrTran);
            dbProvider.AddInParameter(command, helper.Barrflagdesblance, DbType.String, entity.BarrFlagDesbalance);
            dbProvider.AddInParameter(command, helper.Barrfecact, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Barrfactorperdida, DbType.Decimal, entity.BarrFactorPerdida);
            dbProvider.AddInParameter(command, helper.OsinergCodi, DbType.String, entity.OsinergCodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.BarrCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(System.Int32 id, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Barrusername, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public BarraDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, id);
            BarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<BarraDTO> List()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iOsinergCodi = dr.GetOrdinal(helper.OsinergCodi);
                    if (!dr.IsDBNull(iOsinergCodi)) entity.OsinergCodi = dr.GetString(iOsinergCodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListarBarrasSuministrosRelacionada(int barrCodiTra)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarBarrasSuministrosRelacionada);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barrCodiTra);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListVista()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVista);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO barra = helper.Create(dr);
                    int iAreaNombre = dr.GetOrdinal(helper.AreaNombre);
                    if (!dr.IsDBNull(iAreaNombre)) barra.AreaNombre = dr.GetString(iAreaNombre);
                    entitys.Add(barra);
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaBarraTransferencia()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarraTransferencia);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaBarraSuministro()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarraSuministro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> GetByCriteria(string barrNombre, string barrCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            string query = string.Format(helper.SqlGetByCriteria, barrNombre, barrCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddInParameter(command, helper.Barrnombre, DbType.String, Barrnombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<BarraDTO> ListaInterCodEnt()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCodEnt);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaInterCoReSo()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaInterCoReSoByEmpr(int genEmprCodi, int clienEmprCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSoByEmpr);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, clienEmprCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<BarraDTO> ListaBarraRetirosEmpresa(int genEmprCodi, int clienEmprCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarraRetirosEmpresa);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, clienEmprCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaBarraEntregaEmpresa(int genEmprCodi, int clienEmprCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarraEntregaEmpresa);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaBarraEmpresaValorizados(int genEmprCodi, string flag, int periCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarraEmpresaValorizados);
            dbProvider.AddInParameter(command, "flag", DbType.String,flag);
            dbProvider.AddInParameter(command, "genemprcodi", DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, "pericodi", DbType.Int32, periCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaInterCoReGeByEmpr(int genEmprCodi, int clienEmprCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReGeByEmpr);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, clienEmprCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListarTodasLasBarras(int genEmprCodi, int clienEmprCodi)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarTodasLasBarras);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, clienEmprCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<BarraDTO> ListaInterCoReSoDt(int? barraCodiTrans)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSoDt);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barraCodiTrans);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaInterCoReSC()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCoReSC);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaInterValorTrans()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterValorTrans);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListaInterCodInfoBase()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCodInfoBase);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<BarraDTO> ListBarrasTransferenciaByReporte()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarrasTransferenciaByReporte);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iOsinergCodi = dr.GetOrdinal(helper.OsinergCodi);
                    if (!dr.IsDBNull(iOsinergCodi)) entity.OsinergCodi = dr.GetString(iOsinergCodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public BarraDTO GetByBarra(string sBarrNombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByBarra);
            dbProvider.AddInParameter(command, helper.Barrnombre, DbType.String, sBarrNombre);
            BarraDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<BarraDTO> ListarBarraReporteDTR()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarBarraReporteDTR);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entity = new BarraDTO();

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrnombbarrtran);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.BarrNombBarrTran = dr.GetString(iBarrbarratransferencia);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public BarraDTO ObtenerBarraDTR(int barraCodi)
        {
            BarraDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerBarraDTR);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, barraCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new BarraDTO();

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrnombbarrtran);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.BarrNombBarrTran = dr.GetString(iBarrbarratransferencia);
                }
            }

            return entity;
        }

        // Inicio de Agregado - Sistema de Compensaciones
        public List<BarraDTO> ListarBarras()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByBarraCompensacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entity = new BarraDTO();

                    int iBarrCodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iBarrBarraTransferencia = dr.GetOrdinal(helper.Barrnombbarrtran);
                    if (!dr.IsDBNull(iBarrBarraTransferencia)) entity.BarrNombBarrTran = dr.GetString(iBarrBarraTransferencia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        // Fin de Agregado - Sistema de Compensaciones


        #region SIOSEIN
        public List<BarraDTO> GetListaBarraArea(string barras)
        {
            List<BarraDTO> entitys = new List<BarraDTO>();

            string sql = String.Format(helper.SqlGetListaBarraArea, barras);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entity = helper.Create(dr);

                    int iAreaNombre = dr.GetOrdinal(helper.AreaNombre);
                    if (!dr.IsDBNull(iAreaNombre)) entity.AreaNombre = dr.GetString(iAreaNombre);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region MonitoreoMME
        public List<BarraDTO> ListarGrupoBarraEjec()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarGrupoBarraEjec);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entity = new BarraDTO();

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region siosein2
        public List<BarraDTO> ListaCentralxBarra()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCentralxBarra);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BarraDTO entity = new BarraDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);

                    int iBarrcodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        public List<BarraDTO> ListaBarrasActivas()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarrasActivas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region SIOSEIN-PRIE-2021
        public BarraDTO GetBarraAreaxOsinergmin(string osinergCodi)
        {
            BarraDTO entity = null;

            string sql = String.Format(helper.SqlGetBarraAreaByOsinerming, osinergCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new BarraDTO();

                    int iOsinergCodi = dr.GetOrdinal(helper.OsinergCodi);
                    if (!dr.IsDBNull(iOsinergCodi)) entity.OsinergCodi = dr.GetString(iOsinergCodi);

                    int iBarrCodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = Convert.ToInt32(dr.GetValue(iBarrCodi));

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);


                    int iBarrTension = dr.GetOrdinal(helper.Barrtension);
                    if (!dr.IsDBNull(iBarrTension)) entity.BarrTension = dr.GetString(iBarrTension);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaNombre = dr.GetOrdinal(helper.AreaNombre);
                    if (!dr.IsDBNull(iAreaNombre)) entity.AreaNombre = dr.GetString(iAreaNombre);

                }
            }
            return entity;
        }
        #endregion

        #region CPPA-2024
        public List<BarraDTO> FiltroBarrasTransIntegrantes(int revision)
        {
            BarraDTO entity = new BarraDTO();
            List<BarraDTO> entitys = new List<BarraDTO>();
            string query = string.Format(helper.SqlFiltroBarrasTransIntegrantes, revision);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new BarraDTO();

                    int iBarrCodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.BarrBarraTransferencia = dr.GetString(iBarrbarratransferencia);

                    int iBarrNombreConcatenado = dr.GetOrdinal(helper.Barrnombreconcatenado);
                    if (!dr.IsDBNull(iBarrNombreConcatenado)) entity.BarrNombreConcatenado = dr.GetString(iBarrNombreConcatenado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<BarraDTO> ListaBarrasTransFormato()
        {
            BarraDTO entity = new BarraDTO();
            List<BarraDTO> entitys = new List<BarraDTO>();
            string query = string.Format(helper.SqlListaBarrasTransFormato);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new BarraDTO();

                    int iBarrCodi = dr.GetOrdinal(helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iBarrNombreConcatenado = dr.GetOrdinal(helper.Barrnombreconcatenado);
                    if (!dr.IsDBNull(iBarrNombreConcatenado)) entity.BarrNombreConcatenado = dr.GetString(iBarrNombreConcatenado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion
    }
}
