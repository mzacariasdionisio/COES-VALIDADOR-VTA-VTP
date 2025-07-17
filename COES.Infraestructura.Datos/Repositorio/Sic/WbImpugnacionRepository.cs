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
    /// Clase de acceso a datos de la tabla WB_IMPUGNACION
    /// </summary>
    public class WbImpugnacionRepository: RepositoryBase, IWbImpugnacionRepository
    {
        public WbImpugnacionRepository(string strConn): base(strConn)
        {
        }

        WbImpugnacionHelper helper = new WbImpugnacionHelper();

        public int Save(WbImpugnacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Impgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Impgnombre, DbType.String, entity.Impgnombre);
            dbProvider.AddInParameter(command, helper.Impgtitulo, DbType.String, entity.Impgtitulo);
            dbProvider.AddInParameter(command, helper.Impgnumeromes, DbType.Int32, entity.Impgnumeromes);
            dbProvider.AddInParameter(command, helper.Impgregsgdoc, DbType.String, entity.Impgregsgdoc);
            dbProvider.AddInParameter(command, helper.Impginpugnante, DbType.String, entity.Impginpugnante);
            dbProvider.AddInParameter(command, helper.Impgdescinpugnad, DbType.String, entity.Impgdescinpugnad);
            dbProvider.AddInParameter(command, helper.Impgpetitorio, DbType.String, entity.Impgpetitorio);
            dbProvider.AddInParameter(command, helper.Impgfechrecep, DbType.DateTime, entity.Impgfechrecep);
            dbProvider.AddInParameter(command, helper.Impgfechpubli, DbType.DateTime, entity.Impgfechpubli);
            dbProvider.AddInParameter(command, helper.Impgplazincorp, DbType.DateTime, entity.Impgplazincorp);
            dbProvider.AddInParameter(command, helper.Impgincorpresent, DbType.String, entity.Impgincorpresent);
            dbProvider.AddInParameter(command, helper.Impgdescdirecc, DbType.String, entity.Impgdescdirecc);
            dbProvider.AddInParameter(command, helper.Impgfechdesc, DbType.DateTime, entity.Impgfechdesc);
            dbProvider.AddInParameter(command, helper.Impgdiastotaten, DbType.Int32, entity.Impgdiastotaten);
            dbProvider.AddInParameter(command, helper.Impgusuariocreacion, DbType.String, entity.Impgusuariocreacion);
            dbProvider.AddInParameter(command, helper.Impgrutaarch, DbType.String, entity.Impgrutaarch);
            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, entity.Timpgcodi);
            dbProvider.AddInParameter(command, helper.Impgfechacreacion, DbType.DateTime, entity.Impgfechacreacion);
            dbProvider.AddInParameter(command, helper.Impgusuarioupdate, DbType.String, entity.Impgusuarioupdate);
            dbProvider.AddInParameter(command, helper.Impgfechaupdate, DbType.DateTime, entity.Impgfechaupdate);
            dbProvider.AddInParameter(command, helper.Impgmesanio, DbType.DateTime, entity.Impgmesanio);
            dbProvider.AddInParameter(command, helper.Impgextension, DbType.String, entity.Impgextension);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbImpugnacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Impgnombre, DbType.String, entity.Impgnombre);
            dbProvider.AddInParameter(command, helper.Impgtitulo, DbType.String, entity.Impgtitulo);
            dbProvider.AddInParameter(command, helper.Impgnumeromes, DbType.Int32, entity.Impgnumeromes);
            dbProvider.AddInParameter(command, helper.Impgregsgdoc, DbType.String, entity.Impgregsgdoc);
            dbProvider.AddInParameter(command, helper.Impginpugnante, DbType.String, entity.Impginpugnante);
            dbProvider.AddInParameter(command, helper.Impgdescinpugnad, DbType.String, entity.Impgdescinpugnad);
            dbProvider.AddInParameter(command, helper.Impgpetitorio, DbType.String, entity.Impgpetitorio);
            dbProvider.AddInParameter(command, helper.Impgfechrecep, DbType.DateTime, entity.Impgfechrecep);
            dbProvider.AddInParameter(command, helper.Impgfechpubli, DbType.DateTime, entity.Impgfechpubli);
            dbProvider.AddInParameter(command, helper.Impgplazincorp, DbType.DateTime, entity.Impgplazincorp);
            dbProvider.AddInParameter(command, helper.Impgincorpresent, DbType.String, entity.Impgincorpresent);
            dbProvider.AddInParameter(command, helper.Impgdescdirecc, DbType.String, entity.Impgdescdirecc);
            dbProvider.AddInParameter(command, helper.Impgfechdesc, DbType.DateTime, entity.Impgfechdesc);
            dbProvider.AddInParameter(command, helper.Impgdiastotaten, DbType.Int32, entity.Impgdiastotaten);
            dbProvider.AddInParameter(command, helper.Impgrutaarch, DbType.String, entity.Impgrutaarch);
            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, entity.Timpgcodi);
            dbProvider.AddInParameter(command, helper.Impgusuarioupdate, DbType.String, entity.Impgusuarioupdate);
            dbProvider.AddInParameter(command, helper.Impgfechaupdate, DbType.DateTime, entity.Impgfechaupdate);
            dbProvider.AddInParameter(command, helper.Impgmesanio, DbType.DateTime, entity.Impgmesanio);
            dbProvider.AddInParameter(command, helper.Impgextension, DbType.String, entity.Impgextension);
            dbProvider.AddInParameter(command, helper.Impgcodi, DbType.Int32, entity.Impgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int impgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Impgcodi, DbType.Int32, impgcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbImpugnacionDTO GetById(int impgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Impgcodi, DbType.Int32, impgcodi);

            WbImpugnacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbImpugnacionDTO> List()
        {
            List<WbImpugnacionDTO> entitys = new List<WbImpugnacionDTO>();
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

        public List<WbImpugnacionDTO> GetByCriteria(int codigoTipo, DateTime fecha)
        {
            List<WbImpugnacionDTO> entitys = new List<WbImpugnacionDTO>();

            string query = String.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.Timpgcodi, DbType.Int32, codigoTipo);

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
