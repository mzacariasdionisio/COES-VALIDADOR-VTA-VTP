using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
   public class TramiteRepository: RepositoryBase,ITramiteRepository
    {
        public TramiteRepository(string strConn) : base(strConn)
        {
        }

        TramiteHelper helper = new TramiteHelper();

        public int Save(TramiteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tramcodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.Usuacoescodi, DbType.String, entity.UsuaCoesCodi);
            dbProvider.AddInParameter(command, helper.Usuaseincodi, DbType.String, entity.UsuaSeinCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.Tipotramcodi, DbType.Int32, entity.TipoTramcodi);
            dbProvider.AddInParameter(command, helper.Tramcorrinf, DbType.String, entity.TramCorrInf);
            dbProvider.AddInParameter(command, helper.Tramdescripcion, DbType.String, entity.TramDescripcion);
            dbProvider.AddInParameter(command, helper.Tramrespuesta, DbType.String, entity.TramRespuesta);
            dbProvider.AddInParameter(command, helper.Tramfecreg, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Tramfecins, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Tramfecact, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Tramfecact, DbType.Int32, entity.TramVersion);
            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TramiteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Usuacoescodi, DbType.String, entity.UsuaCoesCodi);
            dbProvider.AddInParameter(command, helper.Tramcorrinf, DbType.String, entity.TramCorrInf);
            dbProvider.AddInParameter(command, helper.Tramrespuesta, DbType.String, entity.TramRespuesta);
            dbProvider.AddInParameter(command, helper.Tramfecres, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Tramfecact, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Tramcodi, DbType.Int32, entity.TramCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaTramite(System.Int32 iPeriCod, System.Int32 iTrmVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTramite);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCod);
            dbProvider.AddInParameter(command, helper.Tramversion, DbType.Int32, iTrmVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public TramiteDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper .SqlGetById);

            dbProvider.AddInParameter(command, helper.Tramcodi, DbType.Int32, id);
            TramiteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TramiteDTO> List()
        {
            List<TramiteDTO> entitys = new List<TramiteDTO>();
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

        public List<TramiteDTO> GetByCriteria(DateTime? fecha, string corrinf, int periodo,int version)
        {
            List<TramiteDTO> entitys = new List<TramiteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Tramfecreg, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Tramfecreg, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Tramcorrinf, DbType.String, corrinf);
            dbProvider.AddInParameter(command, helper.Tramcorrinf, DbType.String, corrinf);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, periodo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, periodo);
            dbProvider.AddInParameter(command, helper.Tramversion, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.Tramversion, DbType.Int32, version);
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
    }
}
