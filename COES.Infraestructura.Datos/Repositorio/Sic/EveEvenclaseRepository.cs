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
    /// Clase de acceso a datos de la tabla EVE_EVENCLASE
    /// </summary>
    public class EveEvenclaseRepository: RepositoryBase, IEveEvenclaseRepository
    {
        public EveEvenclaseRepository(string strConn): base(strConn)
        {
        }

        EveEvenclaseHelper helper = new EveEvenclaseHelper();

        public void Save(EveEvenclaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Evenclasedesc, DbType.String, entity.Evenclasedesc);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Evenclasetipo, DbType.String, entity.Evenclasetipo);
            dbProvider.AddInParameter(command, helper.Evenclaseabrev, DbType.String, entity.Evenclaseabrev);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EveEvenclaseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Evenclasedesc, DbType.String, entity.Evenclasedesc);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Evenclasetipo, DbType.String, entity.Evenclasetipo);
            dbProvider.AddInParameter(command, helper.Evenclaseabrev, DbType.String, entity.Evenclaseabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int evenclasecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, evenclasecodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, evenclasecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveEvenclaseDTO GetById(int evenclasecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, evenclasecodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, evenclasecodi);
            EveEvenclaseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveEvenclaseDTO> List()
        {
            List<EveEvenclaseDTO> entitys = new List<EveEvenclaseDTO>();
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

        public List<EveEvenclaseDTO> GetByCriteria()
        {
            List<EveEvenclaseDTO> entitys = new List<EveEvenclaseDTO>();
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


        #region INTERVENCIONES 

        public List<EveEvenclaseDTO> ListarComboTiposProgramaciones(int iEscenario)
        {
            List<EveEvenclaseDTO> entitys = new List<EveEvenclaseDTO>();
            DbCommand command = null;

            if (iEscenario == 1) // 1 = Mantenimiento
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTiposProgramacionesMantenimiento);
            else if (iEscenario == 2) // 2 = Consulta
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTiposProgramacionesConsulta);
            else if (iEscenario == 3) // 3 = Para los demas Reportes
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTiposProgramacionesReporte);
            else if (iEscenario == 4) // 4 = Consulta Cruzadas
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTiposProgramacionesConsultaCruzadas);
            else if (iEscenario == 5) // 5 = Reportes de Intervenciones Mayores
                command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTiposProgramacionesReporteIntervencionesMayores);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveEvenclaseDTO entity = new EveEvenclaseDTO();
                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
