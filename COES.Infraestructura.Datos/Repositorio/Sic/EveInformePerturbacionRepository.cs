using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class EveInformePerturbacionRepository : RepositoryBase
    {
        public EveInformePerturbacionRepository(string strConn)
            : base(strConn)
        {
        }

        PerturbacionHelper helper = new PerturbacionHelper();

        public List<InformePerturbacionDTO> ObtenerInformePorEvento(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(this.helper.SqlGetById);
            dbProvider.AddInParameter(command, this.helper.EVENCODI, DbType.Int32, idEvento);

            List<InformePerturbacionDTO> entitys = new List<InformePerturbacionDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            { 
                while(dr.Read())
                {
                    entitys.Add( this.helper.Create(dr));
                }
            }            

            foreach (InformePerturbacionDTO item in entitys)
            {
                if (item.EQUICODI != null)
                {
                    command.Parameters.Clear();
                    command = dbProvider.GetSqlStringCommand(this.helper.SqlGetItemArea);
                    dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, item.EQUICODI);

                    using (IDataReader dr = dbProvider.ExecuteReader(command))
                    {
                        if (dr.Read())
                        {
                            int iEquiNomb = dr.GetOrdinal(this.helper.EQUINOMB);
                            if (!dr.IsDBNull(iEquiNomb)) item.EQUINOMB = dr.GetString(iEquiNomb);

                            int iEquiAbrev = dr.GetOrdinal(this.helper.EQUIABREV);
                            if (!dr.IsDBNull(iEquiAbrev)) item.SUBESTACION = dr.GetString(iEquiAbrev);                            
                        }
                    }     
                }

                if (item.INTERRUPTORCODI != null)
                {
                    command.Parameters.Clear();
                    command = dbProvider.GetSqlStringCommand(helper.SqlGetNombreEquipo);
                    dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, item.INTERRUPTORCODI);

                    using (IDataReader dr = dbProvider.ExecuteReader(command))
                    {
                        if (dr.Read())
                        {
                            int iEquiNomb = dr.GetOrdinal(this.helper.EQUINOMB);
                            if (!dr.IsDBNull(iEquiNomb)) item.INTERRUPTORNOMB = dr.GetString(iEquiNomb);
                        }
                    }
                }

                if (item.SUBCAUSACODI != null)
                {
                    command.Parameters.Clear();
                    command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCausaDesc);
                    dbProvider.AddInParameter(command, helper.SUBCAUSACODI, DbType.Int32, item.SUBCAUSACODI);

                    using (IDataReader dr = dbProvider.ExecuteReader(command))
                    {
                        if (dr.Read())
                        {
                            int iSUBCAUSADESC = dr.GetOrdinal(this.helper.SUBCAUSADESC);
                            if (!dr.IsDBNull(iSUBCAUSADESC)) item.SUBCAUSADESC = dr.GetString(iSUBCAUSADESC);
                        }
                    }                   
                }
            }

            return entitys;
        }

        public void EliminarInforme(int idEvento)
        {
            try
            {
                string query = string.Format(helper.SqlEliminarPorEvento, idEvento);
                DbCommand command = dbProvider.GetSqlStringCommand(query);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void GrabarInforme(List<InformePerturbacionDTO> entitys)
        {
            try
            {
                int id = 1;

                DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
                object result = dbProvider.ExecuteScalar(command);
                if (result != null)
                {
                    id = Convert.ToInt32(result);
                }                

                foreach (InformePerturbacionDTO entity in entitys)
                {
                    command.Parameters.Clear();
                    command = dbProvider.GetSqlStringCommand(helper.SqlSave);

                    dbProvider.AddInParameter(command, helper.PERTURBACIONCODI, DbType.Int32, id);
                    dbProvider.AddInParameter(command, helper.EVENCODI, DbType.Int32, entity.EVENCODI);
                    dbProvider.AddInParameter(command, helper.SUBCAUSACODI, DbType.Int32, entity.SUBCAUSACODI);
                    dbProvider.AddInParameter(command, helper.ITEMTIPO, DbType.String, entity.ITEMTIPO);
                    dbProvider.AddInParameter(command, helper.ITEMTIME, DbType.String, entity.ITEMTIME);
                    dbProvider.AddInParameter(command, helper.ITEMDESCRIPCION, DbType.String, entity.ITEMDESCRIPCION);
                    dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, entity.EQUICODI);
                    dbProvider.AddInParameter(command, helper.INTERRUPTORCODI, DbType.Int32, entity.INTERRUPTORCODI);
                    dbProvider.AddInParameter(command, helper.ITEMSENALIZACION, DbType.String, entity.ITEMSENALIZACION);
                    dbProvider.AddInParameter(command, helper.ITEMAC, DbType.String, entity.ITEMAC);
                    dbProvider.AddInParameter(command, helper.ITEMORDEN, DbType.Decimal, entity.ITEMORDEN);
                    dbProvider.AddInParameter(command, helper.LASTDATE, DbType.DateTime, entity.LASTDATE);
                    dbProvider.AddInParameter(command, helper.LASTUSER, DbType.String, entity.LASTUSER);

                    dbProvider.ExecuteNonQuery(command);

                    id++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
