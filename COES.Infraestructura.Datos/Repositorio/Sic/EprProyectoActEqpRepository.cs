using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public class EprProyectoActEqpRepository : RepositoryBase, IEprProyectoActEqpRepository
    {
        public EprProyectoActEqpRepository(string strConn) : base(strConn)
        {
        }

        EprProyectoActEqpHelper helper = new EprProyectoActEqpHelper();

        public int Save(EprProyectoActEqpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Epproysgcodi, DbType.Int32, entity.Epproysgcodi);
            dbProvider.AddInParameter(command, helper.Eppproyflgtieneeo, DbType.String, entity.Eppproyflgtieneeo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Epproynemotecnico, DbType.String, entity.Epproynemotecnico);
            dbProvider.AddInParameter(command, helper.Epproynomb, DbType.String, entity.Epproynomb);
            dbProvider.AddInParameter(command, helper.Epproyfecregistro, DbType.String, entity.Epproyfecregistro);
            dbProvider.AddInParameter(command, helper.Epproydescripcion, DbType.String, entity.Epproydescripcion);
            dbProvider.AddInParameter(command, helper.Epproyestregistro, DbType.String, entity.Epproyestregistro);
            dbProvider.AddInParameter(command, helper.Epproyusucreacion, DbType.String, entity.Epproyusucreacion);
       
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EprProyectoActEqpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Epproysgcodi, DbType.Int32, entity.Epproysgcodi);
            dbProvider.AddInParameter(command, helper.Eppproyflgtieneeo, DbType.String, entity.Eppproyflgtieneeo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Epproynemotecnico, DbType.String, entity.Epproynemotecnico);
            dbProvider.AddInParameter(command, helper.Epproynomb, DbType.String, entity.Epproynomb);
            dbProvider.AddInParameter(command, helper.Epproyfecregistro, DbType.String, entity.Epproyfecregistro);
            dbProvider.AddInParameter(command, helper.Epproydescripcion, DbType.String, entity.Epproydescripcion);
            dbProvider.AddInParameter(command, helper.Epproyestregistro, DbType.String, entity.Epproyestregistro);
            dbProvider.AddInParameter(command, helper.Eppproyusumodificacion, DbType.String, entity.Eppproyusumodificacion);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, entity.Epproycodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(EprProyectoActEqpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            dbProvider.AddInParameter(command, helper.Epproyestregistro, DbType.String, entity.Epproyestregistro);
            dbProvider.AddInParameter(command, helper.Eppproyusumodificacion, DbType.String, entity.Eppproyusumodificacion);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, entity.Epproycodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EprProyectoActEqpDTO GetById(int epproycodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, epproycodi);
            EprProyectoActEqpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EprProyectoActEqpDTO> List(int epproysgcodi, string epproynomb, string epproyfecregistroIni, string epproyfecregistroFin)
        {
            List<EprProyectoActEqpDTO> entitys = new List<EprProyectoActEqpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarProyecto);
            dbProvider.AddInParameter(command, helper.Epproysgcodi, DbType.Int32, epproysgcodi);
            dbProvider.AddInParameter(command, helper.Epproysgcodi, DbType.Int32, epproysgcodi);
            dbProvider.AddInParameter(command, helper.Epproynomb, DbType.String, epproynomb);
            dbProvider.AddInParameter(command, helper.EpproyfecregistroIni, DbType.String, epproyfecregistroIni);
            dbProvider.AddInParameter(command, helper.EpproyfecregistroIni, DbType.String, epproyfecregistroIni);
            dbProvider.AddInParameter(command, helper.EpproyfecregistroFin, DbType.String, epproyfecregistroFin);
            dbProvider.AddInParameter(command, helper.EpproyfecregistroFin, DbType.String, epproyfecregistroFin);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EprProyectoActEqpDTO> ListProyectoProyectoActualizacion(int equicodi)
        {
            List<EprProyectoActEqpDTO> entitys = new List<EprProyectoActEqpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListProyectoProyectoActualizacion);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, equicodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprProyectoActEqpDTO entity = new EprProyectoActEqpDTO();

                    int iEpproycodi = dr.GetOrdinal(helper.Epproycodi);
                    if (!dr.IsDBNull(iEpproycodi)) entity.Epproycodi = Convert.ToInt32(dr.GetValue(iEpproycodi));

                    int iEpproynomb = dr.GetOrdinal(helper.Epproynomb);
                    if (!dr.IsDBNull(iEpproynomb)) entity.Epproynomb = Convert.ToString(dr.GetValue(iEpproynomb));

                    int iEpproyfecregistro = dr.GetOrdinal(helper.Epproyfecregistro);
                    if (!dr.IsDBNull(iEpproyfecregistro)) entity.Epproyfecregistro = Convert.ToString(dr.GetValue(iEpproyfecregistro));


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprProyectoActEqpDTO> ListMaestroProyecto()
        {
            List<EprProyectoActEqpDTO> entitys = new List<EprProyectoActEqpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMaestroProyecto);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprProyectoActEqpDTO entity = new EprProyectoActEqpDTO();

                    int iEpproycodi = dr.GetOrdinal(helper.Epproydescripcion);
                    if (!dr.IsDBNull(iEpproycodi)) entity.Epproydescripcion = dr.GetValue(iEpproycodi).ToString();

                    int iEpproynemotecnico = dr.GetOrdinal(helper.Epproynemotecnico);
                    if (!dr.IsDBNull(iEpproynemotecnico)) entity.Epproynemotecnico = dr.GetValue(iEpproynemotecnico).ToString();

                    int iEpproynomb = dr.GetOrdinal(helper.Epproynomb);
                    if (!dr.IsDBNull(iEpproynomb)) entity.Epproynomb = dr.GetValue(iEpproynomb).ToString();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetValue(iEmprnomb).ToString();

                    int iEpproyfecregistro = dr.GetOrdinal(helper.Epproyfecregistro);
                    if (!dr.IsDBNull(iEpproyfecregistro)) entity.Epproyfecregistro = dr.GetValue(iEpproyfecregistro).ToString();


                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public EprProyectoActEqpDTO ValidarProyectoActualizacionPorRele(int epproycodi)
        {
            EprProyectoActEqpDTO entity = new EprProyectoActEqpDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ValidarProyectoPorRele);
            dbProvider.AddInParameter(command, "EPPROYCODI ", DbType.Int32, epproycodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    int sNroEquipo = dr.GetOrdinal("NRO_EQUIPO");
                    if (!dr.IsDBNull(sNroEquipo)) entity.NroEquipo = Convert.ToInt32(dr.GetValue(sNroEquipo));

                    int iNroPropiedades = dr.GetOrdinal("NRO_PROPIEDADES");
                    if (!dr.IsDBNull(iNroPropiedades)) entity.NroPropiedades = Convert.ToInt32(dr.GetValue(iNroPropiedades));

                }
            }
            return entity;
        }


    }
}
