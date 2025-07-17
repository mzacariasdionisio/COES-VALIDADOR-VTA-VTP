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
    /// Clase de acceso a datos de la tabla AGC_CONTROL
    /// </summary>
    public class AgcControlRepository: RepositoryBase, IAgcControlRepository
    {
        public AgcControlRepository(string strConn): base(strConn)
        {
        }

        AgcControlHelper helper = new AgcControlHelper();

        public int Save(AgcControlDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Agcctipo, DbType.String, entity.Agcctipo);
            dbProvider.AddInParameter(command, helper.Agccdescrip, DbType.String, entity.Agccdescrip);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Agccb2, DbType.String, entity.Agccb2);
            dbProvider.AddInParameter(command, helper.Agccb3, DbType.String, entity.Agccb3);
            dbProvider.AddInParameter(command, helper.Agccvalido, DbType.String, entity.Agccvalido);
            dbProvider.AddInParameter(command, helper.Agccusucreacion, DbType.String, entity.Agccusucreacion);
            dbProvider.AddInParameter(command, helper.Agccfeccreacion, DbType.DateTime, entity.Agccfeccreacion);
            dbProvider.AddInParameter(command, helper.Agccusumodificacion, DbType.String, entity.Agccusumodificacion);
            dbProvider.AddInParameter(command, helper.Agccfecmodificacion, DbType.DateTime, entity.Agccfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AgcControlDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Agcctipo, DbType.String, entity.Agcctipo);
            dbProvider.AddInParameter(command, helper.Agccdescrip, DbType.String, entity.Agccdescrip);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Agccb2, DbType.String, entity.Agccb2);
            dbProvider.AddInParameter(command, helper.Agccb3, DbType.String, entity.Agccb3);
            dbProvider.AddInParameter(command, helper.Agccvalido, DbType.String, entity.Agccvalido);
            dbProvider.AddInParameter(command, helper.Agccusucreacion, DbType.String, entity.Agccusucreacion);
            dbProvider.AddInParameter(command, helper.Agccfeccreacion, DbType.DateTime, entity.Agccfeccreacion);
            dbProvider.AddInParameter(command, helper.Agccusumodificacion, DbType.String, entity.Agccusumodificacion);
            dbProvider.AddInParameter(command, helper.Agccfecmodificacion, DbType.DateTime, entity.Agccfecmodificacion);
            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, entity.Agcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int agcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, agcccodi);

            dbProvider.ExecuteNonQuery(command);
        }





        public AgcControlDTO GetById(int agcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Agcccodi, DbType.Int32, agcccodi);
            AgcControlDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AgcControlDTO> List(string estado, int nroPage, int pageSize)
        {
            
            List<AgcControlDTO> entitys = new List<AgcControlDTO>();

            String sql = String.Format(this.helper.SqlList, estado, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);


            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }






        public List<AgcControlDTO> GetByCriteria()
        {
            List<AgcControlDTO> entitys = new List<AgcControlDTO>();
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
        /// <summary>
        /// Graba los datos de la tabla AGC_CONTROL
        /// </summary>
        public int SaveAgcControlId(AgcControlDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Agcccodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Agcccodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /*
        /// <summary>
        /// Actualiza los datos de la tabla ME_PTOMEDICION del AGC
        /// </summary>
        public void UpdateMePtoMedicion(MePtomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMePtomedicion);

            dbProvider.AddInParameter(command, helper.Ptomedielenomb, DbType.String, entity.Ptomedielenomb);
            dbProvider.AddInParameter(command, helper.Ptomedibarranomb, DbType.String, entity.Ptomedibarranomb);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);


            dbProvider.ExecuteNonQuery(command);
        }*/

        /*
        /// <summary>
        /// Actualiza los datos de la tabla ME_PTOMEDICION del AGC relacionado a Costo Variable
        /// </summary>
        public void UpdateMePtoMedicionCVariable(MePtomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateMePtomedicionCVariable);

            
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.String, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Ptomedibarranomb, DbType.String, entity.Ptomedibarranomb);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);


            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);


            dbProvider.ExecuteNonQuery(command);
        }*/
        


        public int ObtenerNroFilas(string estado)
        {
            String sql = String.Format(this.helper.TotalRegistros, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }



    }
}
