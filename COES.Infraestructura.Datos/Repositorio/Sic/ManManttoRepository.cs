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
    /// Clase de acceso a datos de la tabla MAN_MANTTO
    /// </summary>
    public class ManManttoRepository : RepositoryBase, IManManttoRepository
    {
        public ManManttoRepository(string strConn)
            : base(strConn)
        {
        }

        ManManttoHelper helper = new ManManttoHelper();

        public int Save(ManManttoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mancodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Emprcodireporta, DbType.Int32, entity.Emprcodireporta);
            dbProvider.AddInParameter(command, helper.Evenini, DbType.DateTime, entity.Evenini);
            dbProvider.AddInParameter(command, helper.Evenpreini, DbType.DateTime, entity.Evenpreini);
            dbProvider.AddInParameter(command, helper.Evenfin, DbType.DateTime, entity.Evenfin);
            dbProvider.AddInParameter(command, helper.Evenprefin, DbType.DateTime, entity.Evenprefin);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Evenmwindisp, DbType.Decimal, entity.Evenmwindisp);
            dbProvider.AddInParameter(command, helper.Evenpadre, DbType.Int32, entity.Evenpadre);
            dbProvider.AddInParameter(command, helper.Evenindispo, DbType.String, entity.Evenindispo);
            dbProvider.AddInParameter(command, helper.Eveninterrup, DbType.String, entity.Eveninterrup);
            dbProvider.AddInParameter(command, helper.Eventipoprog, DbType.String, entity.Eventipoprog);
            dbProvider.AddInParameter(command, helper.Evendescrip, DbType.String, entity.Evendescrip);
            dbProvider.AddInParameter(command, helper.Evenobsrv, DbType.String, entity.Evenobsrv);
            dbProvider.AddInParameter(command, helper.Evenestado, DbType.String, entity.Evenestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenprocesado, DbType.Int32, entity.Evenprocesado);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, entity.Manttocodi);
            dbProvider.AddInParameter(command, helper.Isfiles, DbType.String, entity.Isfiles);
            dbProvider.AddInParameter(command, helper.Created, DbType.DateTime, entity.Created);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ManManttoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mancodi, DbType.Int32, entity.Mancodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Emprcodireporta, DbType.Int32, entity.Emprcodireporta);
            dbProvider.AddInParameter(command, helper.Evenini, DbType.DateTime, entity.Evenini);
            dbProvider.AddInParameter(command, helper.Evenpreini, DbType.DateTime, entity.Evenpreini);
            dbProvider.AddInParameter(command, helper.Evenfin, DbType.DateTime, entity.Evenfin);
            dbProvider.AddInParameter(command, helper.Evenprefin, DbType.DateTime, entity.Evenprefin);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Evenmwindisp, DbType.Decimal, entity.Evenmwindisp);
            dbProvider.AddInParameter(command, helper.Evenpadre, DbType.Int32, entity.Evenpadre);
            dbProvider.AddInParameter(command, helper.Evenindispo, DbType.String, entity.Evenindispo);
            dbProvider.AddInParameter(command, helper.Eveninterrup, DbType.String, entity.Eveninterrup);
            dbProvider.AddInParameter(command, helper.Eventipoprog, DbType.String, entity.Eventipoprog);
            dbProvider.AddInParameter(command, helper.Evendescrip, DbType.String, entity.Evendescrip);
            dbProvider.AddInParameter(command, helper.Evenobsrv, DbType.String, entity.Evenobsrv);
            dbProvider.AddInParameter(command, helper.Evenestado, DbType.String, entity.Evenestado);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenprocesado, DbType.Int32, entity.Evenprocesado);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, entity.Manttocodi);
            dbProvider.AddInParameter(command, helper.Isfiles, DbType.String, entity.Isfiles);
            dbProvider.AddInParameter(command, helper.Created, DbType.DateTime, entity.Created);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mancodi, DbType.Int32, mancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ManManttoDTO GetById(int mancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mancodi, DbType.Int32, mancodi);
            ManManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ManManttoDTO> List()
        {
            List<ManManttoDTO> entitys = new List<ManManttoDTO>();
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

        public List<ManManttoDTO> GetByCriteria()
        {
            List<ManManttoDTO> entitys = new List<ManManttoDTO>();
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


        public List<ManManttoDTO> ListManttoPorEquipoFecha(int equicodi, DateTime fecha)
        {
            var sCommand = string.Format(helper.SqlMantenimietosPorEquipoFecha, equicodi, fecha.ToString("yyyy-MM-dd"));
            var entitys = new List<ManManttoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

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
