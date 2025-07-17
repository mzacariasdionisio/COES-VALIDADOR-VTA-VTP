using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla MAN_REGISTRO
    /// </summary>
    public class ManRegistroRepository : RepositoryBase, IManRegistroRepository
    {
        public ManRegistroRepository(string strConn)
            : base(strConn)
        {
        }

        ManRegistroHelper helper = new ManRegistroHelper();

        public int Save(ManRegistroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Regabrev, DbType.String, entity.Regabrev);
            dbProvider.AddInParameter(command, helper.Regnomb, DbType.String, entity.Regnomb);
            dbProvider.AddInParameter(command, helper.Fechaini, DbType.DateTime, entity.Fechaini);
            dbProvider.AddInParameter(command, helper.Fechafin, DbType.DateTime, entity.Fechafin);
            dbProvider.AddInParameter(command, helper.Tregcodi, DbType.Int32, entity.Tregcodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, entity.Version);
            dbProvider.AddInParameter(command, helper.Sololectura, DbType.Int32, entity.Sololectura);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Fechalim, DbType.DateTime, entity.Fechalim);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ManRegistroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Regabrev, DbType.String, entity.Regabrev);
            dbProvider.AddInParameter(command, helper.Regnomb, DbType.String, entity.Regnomb);
            dbProvider.AddInParameter(command, helper.Fechaini, DbType.DateTime, entity.Fechaini);
            dbProvider.AddInParameter(command, helper.Fechafin, DbType.DateTime, entity.Fechafin);
            dbProvider.AddInParameter(command, helper.Tregcodi, DbType.Int32, entity.Tregcodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, entity.Version);
            dbProvider.AddInParameter(command, helper.Sololectura, DbType.Int32, entity.Sololectura);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Fechalim, DbType.DateTime, entity.Fechalim);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, regcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ManRegistroDTO GetById(int regcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, regcodi);
            ManRegistroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ManRegistroDTO> List()
        {
            List<ManRegistroDTO> entitys = new List<ManRegistroDTO>();
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

        public List<ManRegistroDTO> GetByCriteria()
        {
            List<ManRegistroDTO> entitys = new List<ManRegistroDTO>();
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

        public int ObtenerNroFilasManRegistroxTipo(int? idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, idEvento);
            object result = dbProvider.ExecuteScalar(command);
            int rows = 0;
            if (result != null) rows = Convert.ToInt32(result);

            return rows;
        }

        public List<ManRegistroDTO> BuscarManRegistro(int idEvento, int nroPagina, int nroFilas)
        {
            String query = String.Format(helper.SqlBuscarManRegistro, idEvento, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<ManRegistroDTO> entitys = new List<ManRegistroDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ManRegistroDTO entity = new ManRegistroDTO();

                    //int iRegNomb = dr.GetOrdinal(helper.Regnomb);
                    //if (!dr.IsDBNull(iRegNomb)) entity.Regnomb = dr.GetString(iRegNomb);

                    //int iFamAbrev = dr.GetOrdinal(helper.FAMABREV);
                    //if (!dr.IsDBNull(iFamAbrev)) entity.FAMABREV = dr.GetString(iFamAbrev);

                    //int iEquiAbrev = dr.GetOrdinal(helper.EQUIABREV);
                    //if (!dr.IsDBNull(iEquiAbrev)) entity.EQUIABREV = dr.GetString(iEquiAbrev);

                    //int iTareaAbrev = dr.GetOrdinal(helper.TAREAABREV);
                    //if (!dr.IsDBNull(iTareaAbrev)) entity.TAREAABREV = dr.GetString(iTareaAbrev);

                    //int iEquiCodi = dr.GetOrdinal(helper.EQUICODI);
                    //if (!dr.IsDBNull(iEquiCodi)) entity.EQUICODI = Convert.ToInt16(dr.GetValue(iEquiCodi));

                    //int iEmprCodi = dr.GetOrdinal(helper.EMPRCODI);
                    //if (!dr.IsDBNull(iEmprCodi)) entity.EMPRCODI = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
