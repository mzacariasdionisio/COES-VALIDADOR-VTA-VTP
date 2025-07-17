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
    /// Clase de acceso a datos de la tabla SI_CAMBIO_TURNO
    /// </summary>
    public class SiCambioTurnoRepository: RepositoryBase, ISiCambioTurnoRepository
    {
        public SiCambioTurnoRepository(string strConn): base(strConn)
        {
        }

        SiCambioTurnoHelper helper = new SiCambioTurnoHelper();

        public int Save(SiCambioTurnoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Coordinadorresp, DbType.Int32, entity.Coordinadorresp);
            dbProvider.AddInParameter(command, helper.Turno, DbType.Int32, entity.Turno);
            dbProvider.AddInParameter(command, helper.Fecturno, DbType.DateTime, entity.Fecturno);
            dbProvider.AddInParameter(command, helper.Coordinadorrecibe, DbType.String, entity.Coordinadorrecibe);
            dbProvider.AddInParameter(command, helper.Especialistarecibe, DbType.String, entity.Especialistarecibe);
            dbProvider.AddInParameter(command, helper.Analistarecibe, DbType.String, entity.Analistarecibe);
            dbProvider.AddInParameter(command, helper.Emsoperativo, DbType.String, entity.Emsoperativo);
            dbProvider.AddInParameter(command, helper.Emsobservaciones, DbType.String, entity.Emsobservaciones);
            dbProvider.AddInParameter(command, helper.Horaentregaturno, DbType.String, entity.Horaentregaturno);
            dbProvider.AddInParameter(command, helper.Casosinreserva, DbType.String, entity.CasoSinReserva);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiCambioTurnoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Coordinadorresp, DbType.Int32, entity.Coordinadorresp);
            dbProvider.AddInParameter(command, helper.Turno, DbType.Int32, entity.Turno);
            dbProvider.AddInParameter(command, helper.Fecturno, DbType.DateTime, entity.Fecturno);
            dbProvider.AddInParameter(command, helper.Coordinadorrecibe, DbType.String, entity.Coordinadorrecibe);
            dbProvider.AddInParameter(command, helper.Especialistarecibe, DbType.String, entity.Especialistarecibe);
            dbProvider.AddInParameter(command, helper.Analistarecibe, DbType.String, entity.Analistarecibe);            
            dbProvider.AddInParameter(command, helper.Emsoperativo, DbType.String, entity.Emsoperativo);
            dbProvider.AddInParameter(command, helper.Emsobservaciones, DbType.String, entity.Emsobservaciones);
            dbProvider.AddInParameter(command, helper.Horaentregaturno, DbType.String, entity.Horaentregaturno);
            dbProvider.AddInParameter(command, helper.Casosinreserva, DbType.String, entity.CasoSinReserva);
            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.String, entity.Cambioturnocodi);            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cambioturnocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, cambioturnocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiCambioTurnoDTO GetById(int cambioturnocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cambioturnocodi, DbType.Int32, cambioturnocodi);
            SiCambioTurnoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiCambioTurnoDTO> List()
        {
            List<SiCambioTurnoDTO> entitys = new List<SiCambioTurnoDTO>();
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

        public List<SiCambioTurnoDTO> GetByCriteria()
        {
            List<SiCambioTurnoDTO> entitys = new List<SiCambioTurnoDTO>();
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

        public List<SiCambioTurnoDTO> ObtenerResponsables()
        {
            List<SiCambioTurnoDTO> entitys = new List<SiCambioTurnoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerResponsables);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiCambioTurnoDTO entity = new SiCambioTurnoDTO();

                    int iPercodi = dr.GetOrdinal(helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

                    int iPernomb = dr.GetOrdinal(helper.Pernomb);
                    if (!dr.IsDBNull(iPernomb)) entity.Pernomb = dr.GetString(iPernomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int VerificarExistencia(int turno, DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerificarExistencia);
            dbProvider.AddInParameter(command, helper.Turno, DbType.Int32, turno);
            dbProvider.AddInParameter(command, helper.Fecturno, DbType.Date, fecha);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }


        public List<string> ObtenerModosOperacion()
        {
            List<string> list = new List<string>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerModosOperacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iTexto = dr.GetOrdinal(helper.Texto);
                    if (!dr.IsDBNull(iTexto)) list.Add(dr.GetString(iTexto));
                }
            }

            return list;
        }

    }
}
