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
    /// Clase de acceso a datos de la tabla RER_CENTRAL
    /// </summary>
    public class RerCentralRepository : RepositoryBase, IRerCentralRepository
    {
        public RerCentralRepository(string strConn)
            : base(strConn)
        {
        }

        RerCentralHelper helper = new RerCentralHelper();

        public int Save(RerCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Rercenestado, DbType.String, entity.Rercenestado);
            dbProvider.AddInParameter(command, helper.Rercenfechainicio, DbType.DateTime, entity.Rercenfechainicio);
            dbProvider.AddInParameter(command, helper.Rercenfechafin, DbType.DateTime, entity.Rercenfechafin);
            dbProvider.AddInParameter(command, helper.Rercenenergadj, DbType.Decimal, entity.Rercenenergadj);
            dbProvider.AddInParameter(command, helper.Rercenprecbase, DbType.Decimal, entity.Rercenprecbase);
            dbProvider.AddInParameter(command, helper.Rerceninflabase, DbType.Decimal, entity.Rerceninflabase);
            dbProvider.AddInParameter(command, helper.Rercendesccontrato, DbType.String, entity.Rercendesccontrato);
            dbProvider.AddInParameter(command, helper.Codentcodi, DbType.Int32, entity.Codentcodi);
            dbProvider.AddInParameter(command, helper.Pingnombre, DbType.String, entity.Pingnombre);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Rercenusucreacion, DbType.String, entity.Rercenusucreacion);
            dbProvider.AddInParameter(command, helper.Rercenfeccreacion, DbType.DateTime, entity.Rercenfeccreacion);
            dbProvider.AddInParameter(command, helper.Rercenusumodificacion, DbType.String, entity.Rercenusumodificacion);
            dbProvider.AddInParameter(command, helper.Rercenfecmodificacion, DbType.DateTime, entity.Rercenfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerCentralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Rercenestado, DbType.String, entity.Rercenestado);
            dbProvider.AddInParameter(command, helper.Rercenfechainicio, DbType.DateTime, entity.Rercenfechainicio);
            dbProvider.AddInParameter(command, helper.Rercenfechafin, DbType.DateTime, entity.Rercenfechafin);
            dbProvider.AddInParameter(command, helper.Rercenenergadj, DbType.Decimal, entity.Rercenenergadj);
            dbProvider.AddInParameter(command, helper.Rercenprecbase, DbType.Decimal, entity.Rercenprecbase);
            dbProvider.AddInParameter(command, helper.Rerceninflabase, DbType.Decimal, entity.Rerceninflabase);
            dbProvider.AddInParameter(command, helper.Rercendesccontrato, DbType.String, entity.Rercendesccontrato);
            dbProvider.AddInParameter(command, helper.Codentcodi, DbType.Int32, entity.Codentcodi);
            dbProvider.AddInParameter(command, helper.Pingnombre, DbType.String, entity.Pingnombre);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Rercenusucreacion, DbType.String, entity.Rercenusucreacion);
            dbProvider.AddInParameter(command, helper.Rercenfeccreacion, DbType.DateTime, entity.Rercenfeccreacion);
            dbProvider.AddInParameter(command, helper.Rercenusumodificacion, DbType.String, entity.Rercenusumodificacion);
            dbProvider.AddInParameter(command, helper.Rercenfecmodificacion, DbType.DateTime, entity.Rercenfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, entity.Rercencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerCenCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, rerCenCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerCentralDTO GetById(int rerCenCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rercencodi, DbType.Int32, rerCenCodi);
            RerCentralDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerCentralDTO> List()
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerCentralDTO> ListNombreCentralEmpresaBarra()
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListNombreCentralEmpresaBarra);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RerCentralDTO> ListByFiltros(int equicodi, int emprcodi, int ptomedicodi, string fechaini, string fechafin, string estado, string codEntrega, int barrcodi)
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            string query = string.Format(helper.SqlListByFiltros, emprcodi, equicodi, ptomedicodi, fechaini, fechafin, estado, codEntrega, barrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RerCentralDTO> ListCentralREREmpresas()
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCentralREREmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = new RerCentralDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }


        public List<RerCentralDTO> ListByEmprcodi(int emprcodi)
        {
            List<RerCentralDTO> entitys = new List<RerCentralDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByEmprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<RerCentralDTO> ListByEquiEmprFecha(int rercencodi, int equicodi, int emprcodi, string fechaini, string fechafin)
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            string query = string.Format(helper.SqlListByEquiEmprFecha,rercencodi, emprcodi, equicodi, fechaini, fechafin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RerCentralDTO> ListByFechasEstado(string rercenfechainicio, string rercenfechafin, string rercenestado)
        {
            string query = string.Format(helper.SqlListByFechasEstado, rercenfechainicio, rercenfechafin, rercenestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }
        //CU21
        public List<RerCentralDTO> ListCentralByFecha(DateTime dRerCenFecha)
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            string sfecha = dRerCenFecha.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlListCentralByFecha, sfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = new RerCentralDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iCodentcodi = dr.GetOrdinal(helper.Codentcodi);
                    if (!dr.IsDBNull(iCodentcodi)) entity.Codentcodi = Convert.ToInt32(dr.GetValue(iCodentcodi));

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPingnombre = dr.GetOrdinal(helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RerCentralDTO> ListCodigoEntregaYBarraTransferencia()
        {
            string query = string.Format(helper.SqlListCodigoEntregaYBarraTransferencia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = new RerCentralDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    entities.Add(entity);
                }
            }

            return entities;
        }


        public List<RerCentralDTO> ListCentralByIds(string idsCentrales)
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            string query = string.Format(helper.SqlListCentralByIds, idsCentrales);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        public List<RerCentralDTO> ListCentralByFechaLVTP(DateTime dRerCenFecha)
        {
            List<RerCentralDTO> entities = new List<RerCentralDTO>();

            string sfecha = dRerCenFecha.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlListCentralByFechaLVTP, sfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerCentralDTO entity = new RerCentralDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iCodentcodi = dr.GetOrdinal(helper.Codentcodi);
                    if (!dr.IsDBNull(iCodentcodi)) entity.Codentcodi = Convert.ToInt32(dr.GetValue(iCodentcodi));

                    int iCodentcodigo = dr.GetOrdinal(helper.Codentcodigo);
                    if (!dr.IsDBNull(iCodentcodigo)) entity.Codentcodigo = dr.GetString(iCodentcodigo);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entities.Add(entity);
                }
            }

            return entities;
        }

    }
}
