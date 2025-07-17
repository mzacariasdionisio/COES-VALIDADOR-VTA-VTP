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
    /// Clase de acceso a datos de la tabla VTP_PEAJE_INGRESO
    /// </summary>
    public class VtpPeajeIngresoRepository : RepositoryBase, IVtpPeajeIngresoRepository
    {
        public VtpPeajeIngresoRepository(string strConn)
            : base(strConn)
        {
        }

        VtpPeajeIngresoHelper helper = new VtpPeajeIngresoHelper();

        public int Save(VtpPeajeIngresoDTO entity)
        {
            if (entity.Pingcodi == 0)
            {
                DbCommand cmd = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                dbProvider.AddInParameter(cmd, helper.Pericodi, DbType.Int32, entity.Pericodi);
                dbProvider.AddInParameter(cmd, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
                object result = dbProvider.ExecuteScalar(cmd);
                if (result != null) entity.Pingcodi = Convert.ToInt32(result);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingtipo, DbType.String, entity.Pingtipo);
            dbProvider.AddInParameter(command, helper.Pingnombre, DbType.String, entity.Pingnombre);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, entity.Rrpecodi);
            dbProvider.AddInParameter(command, helper.Pingpago, DbType.String, entity.Pingpago);
            dbProvider.AddInParameter(command, helper.Pingtransmision, DbType.String, entity.Pingtransmision);
            dbProvider.AddInParameter(command, helper.Pingcodigo, DbType.String, entity.Pingcodigo);
            dbProvider.AddInParameter(command, helper.Pingpeajemensual, DbType.Decimal, entity.Pingpeajemensual);
            dbProvider.AddInParameter(command, helper.Pingtarimensual, DbType.Decimal, entity.Pingtarimensual);
            dbProvider.AddInParameter(command, helper.Pingregulado, DbType.Decimal, entity.Pingregulado);
            dbProvider.AddInParameter(command, helper.Pinglibre, DbType.Decimal, entity.Pinglibre);
            dbProvider.AddInParameter(command, helper.Pinggranusuario, DbType.Decimal, entity.Pinggranusuario);
            dbProvider.AddInParameter(command, helper.Pingporctregulado, DbType.Decimal, entity.Pingporctregulado);
            dbProvider.AddInParameter(command, helper.Pingporctlibre, DbType.Decimal, entity.Pingporctlibre);
            dbProvider.AddInParameter(command, helper.Pingporctgranusuario, DbType.Decimal, entity.Pingporctgranusuario);
            dbProvider.AddInParameter(command, helper.Pingusucreacion, DbType.String, entity.Pingusucreacion);
            dbProvider.AddInParameter(command, helper.Pingfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Pingusumodificacion, DbType.String, entity.Pingusumodificacion);
            dbProvider.AddInParameter(command, helper.Pingfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return entity.Pingcodi;
        }

        public void Update(VtpPeajeIngresoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pingtipo, DbType.String, entity.Pingtipo);
            dbProvider.AddInParameter(command, helper.Pingnombre, DbType.String, entity.Pingnombre);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rrpecodi, DbType.Int32, entity.Rrpecodi);
            dbProvider.AddInParameter(command, helper.Pingpago, DbType.String, entity.Pingpago);
            dbProvider.AddInParameter(command, helper.Pingtransmision, DbType.String, entity.Pingtransmision);
            dbProvider.AddInParameter(command, helper.Pingcodigo, DbType.String, entity.Pingcodigo);
            dbProvider.AddInParameter(command, helper.Pingusumodificacion, DbType.String, entity.Pingusumodificacion);
            dbProvider.AddInParameter(command, helper.Pingfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateDesarrollo(VtpPeajeIngresoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateDesarrollo);
            dbProvider.AddInParameter(command, helper.Pingpeajemensual, DbType.Decimal, entity.Pingpeajemensual);
            dbProvider.AddInParameter(command, helper.Pingtarimensual, DbType.Decimal, entity.Pingtarimensual);
            dbProvider.AddInParameter(command, helper.Pingregulado, DbType.Decimal, entity.Pingregulado);
            dbProvider.AddInParameter(command, helper.Pinglibre, DbType.Decimal, entity.Pinglibre);
            dbProvider.AddInParameter(command, helper.Pinggranusuario, DbType.Decimal, entity.Pinggranusuario);
            dbProvider.AddInParameter(command, helper.Pingporctregulado, DbType.Decimal, entity.Pingporctregulado);
            dbProvider.AddInParameter(command, helper.Pingporctlibre, DbType.Decimal, entity.Pingporctlibre);
            dbProvider.AddInParameter(command, helper.Pingporctgranusuario, DbType.Decimal, entity.Pingporctgranusuario);
            dbProvider.AddInParameter(command, helper.Pingusumodificacion, DbType.String, entity.Pingusumodificacion);
            dbProvider.AddInParameter(command, helper.Pingfecmodificacion, DbType.DateTime, DateTime.Now);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, entity.Recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, entity.Pingcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int recpotcodi, int pingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByCriteria(int pericodi, int recpotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByCriteria);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtpPeajeIngresoDTO GetById(int pericodi, int recpotcodi, int pingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            VtpPeajeIngresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtpPeajeIngresoDTO> GetByEmpresaGeneradora(int pericodi, int recpotcodi, int emprcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public VtpPeajeIngresoDTO GetByIdView(int pericodi, int recpotcodi, int pingcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingcodi, DbType.Int32, pingcodi);
            VtpPeajeIngresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRrpenombre = dr.GetOrdinal(this.helper.Rrpenombre);
                    if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);
                }
            }

            return entity;
        }

        public List<VtpPeajeIngresoDTO> List()
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
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

        public List<VtpPeajeIngresoDTO> GetByCriteria()
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
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

        public VtpPeajeIngresoDTO GetByNombreIngresoTarifario(int pericodi, int recpotcodi, string pingnombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByNombreIngresoTarifario);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pingnombre, DbType.String, pingnombre);
            VtpPeajeIngresoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRrpenombre = dr.GetOrdinal(this.helper.Rrpenombre);
                    if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);
                }
            }

            return entity;
        }

        public List<VtpPeajeIngresoDTO> ListView(int pericodi, int recpotcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListView);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeIngresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRrpenombre = dr.GetOrdinal(this.helper.Rrpenombre);
                    if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeIngresoDTO> ListPagoSi(int pericodi, int recpotcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPagoSi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeIngresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRrpenombre = dr.GetOrdinal(this.helper.Rrpenombre);
                    if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeIngresoDTO> ListTransmisionSi(int pericodi, int recpotcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTransmisionSi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeIngresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRrpenombre = dr.GetOrdinal(this.helper.Rrpenombre);
                    if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeIngresoDTO> ListCargo(int pericodi, int recpotcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCargo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeIngresoDTO entity = new VtpPeajeIngresoDTO();

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPingtransmision = dr.GetOrdinal(this.helper.Pingtransmision);
                    if (!dr.IsDBNull(iPingtransmision)) entity.Pingtransmision = dr.GetString(iPingtransmision);

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    int iPingregulado = dr.GetOrdinal(this.helper.Pingregulado);
                    if (!dr.IsDBNull(iPingregulado)) entity.Pingregulado = dr.GetDecimal(iPingregulado);

                    int iPinglibre = dr.GetOrdinal(this.helper.Pinglibre);
                    if (!dr.IsDBNull(iPinglibre)) entity.Pinglibre = dr.GetDecimal(iPinglibre);

                    int iPinggranusuario = dr.GetOrdinal(this.helper.Pinggranusuario);
                    if (!dr.IsDBNull(iPinggranusuario)) entity.Pinggranusuario = dr.GetDecimal(iPinggranusuario);

                    int iPingporctregulado = dr.GetOrdinal(this.helper.Pingporctregulado);
                    if (!dr.IsDBNull(iPingporctregulado)) entity.Pingporctregulado = dr.GetDecimal(iPingporctregulado);

                    int iPingporctlibre = dr.GetOrdinal(this.helper.Pingporctlibre);
                    if (!dr.IsDBNull(iPingporctlibre)) entity.Pingporctlibre = dr.GetDecimal(iPingporctlibre);

                    int iPingporctgranusuario = dr.GetOrdinal(this.helper.Pingporctgranusuario);
                    if (!dr.IsDBNull(iPingporctgranusuario)) entity.Pingporctgranusuario = dr.GetDecimal(iPingporctgranusuario);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VtpPeajeIngresoDTO> ListIngresoTarifarioMensual(int pericodi, int recpotcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListIngresoTarifarioMensual);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.Int32, recpotcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeIngresoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRrpenombre = dr.GetOrdinal(this.helper.Rrpenombre);
                    if (!dr.IsDBNull(iRrpenombre)) entity.Rrpenombre = dr.GetString(iRrpenombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region PrimasRER.2023
        public List<VtpPeajeIngresoDTO> ListCargoPrimaRER(int emprcodi)
        {
            List<VtpPeajeIngresoDTO> entitys = new List<VtpPeajeIngresoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCargoPrimaRER);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtpPeajeIngresoDTO entity = new VtpPeajeIngresoDTO();

                    int iPingcodi = dr.GetOrdinal(this.helper.Pingcodi);
                    if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

                    int iPingtipo = dr.GetOrdinal(this.helper.Pingtipo);
                    if (!dr.IsDBNull(iPingtipo)) entity.Pingtipo = dr.GetString(iPingtipo);

                    int iPingnombre = dr.GetOrdinal(this.helper.Pingnombre);
                    if (!dr.IsDBNull(iPingnombre)) entity.Pingnombre = dr.GetString(iPingnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
