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
    /// Clase de acceso a datos de la tabla EVE_INFORMEFALLA_N2
    /// </summary>
    public class EveInformefallaN2Repository: RepositoryBase, IEveInformefallaN2Repository
    {
        public EveInformefallaN2Repository(string strConn): base(strConn)
        {
        }

        EveInformefallaN2Helper helper = new EveInformefallaN2Helper();

        public int SaveEvento(EveInformefallaN2DTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);
            //int id = 1;
            //if (result != null)id = Convert.ToInt32(result);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveEvento);

            dbProvider.AddInParameter(command, helper.Eveninfn2codi, DbType.Int32, entity.Eveninfn2codi);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, entity.Evenanio);
            dbProvider.AddInParameter(command, helper.Evenn2corr, DbType.Int32, entity.Evenn2corr);
            dbProvider.AddInParameter(command, helper.Eveninfn2lastuser, DbType.String, entity.Eveninfn2lastuser);
            dbProvider.AddInParameter(command, helper.Eveninfn2lastdate, DbType.DateTime, entity.Eveninfn2lastdate);
            dbProvider.AddInParameter(command, helper.Eveninfpin2emitido, DbType.String, entity.Eveninfpin2emitido);
            dbProvider.AddInParameter(command, helper.Eveninffn2emitido, DbType.String, entity.Eveninffn2emitido);
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasipi, DbType.Int32, entity.Eveninfplazodiasipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasif, DbType.Int32, entity.Eveninfplazodiasif);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraipi, DbType.Int32, entity.Eveninfplazohoraipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraif, DbType.Int32, entity.Eveninfplazohoraif);
            dbProvider.AddInParameter(command, helper.Eveninfplazominipi, DbType.Int32, entity.Eveninfplazominipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazominif, DbType.Int32, entity.Eveninfplazominif);
            dbProvider.ExecuteNonQuery(command);
            return entity.Eveninfn2codi;
        }

        public int Save(EveInformefallaN2DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveninfn2codi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, entity.Evenanio);
            dbProvider.AddInParameter(command, helper.Evenn2corr, DbType.Int32, entity.Evenn2corr);
            dbProvider.AddInParameter(command, helper.Eveninfpin2fechemis, DbType.DateTime, entity.Eveninfpin2fechemis);
            dbProvider.AddInParameter(command, helper.Eveninfpin2emitido, DbType.String, entity.Eveninfpin2emitido);
            dbProvider.AddInParameter(command, helper.Eveninfpin2elab, DbType.String, entity.Eveninfpin2elab);
            dbProvider.AddInParameter(command, helper.Eveninffn2emitido, DbType.String, entity.Eveninffn2emitido);
            dbProvider.AddInParameter(command, helper.Eveninffn2elab, DbType.String, entity.Eveninffn2elab);
            dbProvider.AddInParameter(command, helper.Eveninfn2lastuser, DbType.String, entity.Eveninfn2lastuser);
            dbProvider.AddInParameter(command, helper.Eveninfn2lastdate, DbType.DateTime, entity.Eveninfn2lastdate);
            dbProvider.AddInParameter(command, helper.Eveninffn2fechemis, DbType.DateTime, entity.Eveninffn2fechemis);
            dbProvider.AddInParameter(command, helper.EvenipiEN2emitido, DbType.String, entity.EvenipiEN2emitido);
            dbProvider.AddInParameter(command, helper.EvenipiEN2elab, DbType.String, entity.EvenipiEN2elab);
            dbProvider.AddInParameter(command, helper.EvenipiEN2fechem, DbType.DateTime, entity.EvenipiEN2fechem);
            dbProvider.AddInParameter(command, helper.EvenifEN2emitido, DbType.String, entity.EvenifEN2emitido);
            dbProvider.AddInParameter(command, helper.EvenifEN2elab, DbType.String, entity.EvenifEN2elab);
            dbProvider.AddInParameter(command, helper.EvenifEN2fechem, DbType.DateTime, entity.EvenifEN2fechem);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveInformefallaN2DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, entity.Evenanio);
            dbProvider.AddInParameter(command, helper.Evenn2corr, DbType.Int32, entity.Evenn2corr);
            dbProvider.AddInParameter(command, helper.Eveninfpin2fechemis, DbType.DateTime, entity.Eveninfpin2fechemis);
            dbProvider.AddInParameter(command, helper.Eveninfpin2emitido, DbType.String, entity.Eveninfpin2emitido);
            dbProvider.AddInParameter(command, helper.Eveninfpin2elab, DbType.String, entity.Eveninfpin2elab);
            dbProvider.AddInParameter(command, helper.Eveninffn2emitido, DbType.String, entity.Eveninffn2emitido);
            dbProvider.AddInParameter(command, helper.Eveninffn2elab, DbType.String, entity.Eveninffn2elab);
            dbProvider.AddInParameter(command, helper.Eveninfn2lastuser, DbType.String, entity.Eveninfn2lastuser);
            dbProvider.AddInParameter(command, helper.Eveninfn2lastdate, DbType.DateTime, entity.Eveninfn2lastdate);
            dbProvider.AddInParameter(command, helper.Eveninffn2fechemis, DbType.DateTime, entity.Eveninffn2fechemis);
            dbProvider.AddInParameter(command, helper.EvenipiEN2emitido, DbType.String, entity.EvenipiEN2emitido);
            dbProvider.AddInParameter(command, helper.EvenipiEN2elab, DbType.String, entity.EvenipiEN2elab);
            dbProvider.AddInParameter(command, helper.EvenipiEN2fechem, DbType.DateTime, entity.EvenipiEN2fechem);
            dbProvider.AddInParameter(command, helper.EvenifEN2emitido, DbType.String, entity.EvenifEN2emitido);
            dbProvider.AddInParameter(command, helper.EvenifEN2elab, DbType.String, entity.EvenifEN2elab);
            dbProvider.AddInParameter(command, helper.EvenifEN2fechem, DbType.DateTime, entity.EvenifEN2fechem);
            dbProvider.AddInParameter(command, helper.Eveninfn2codi, DbType.Int32, entity.Eveninfn2codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int eveninfn2codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Eveninfn2codi, DbType.Int32, eveninfn2codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveInformefallaN2DTO GetById(int eveninfn2codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Eveninfn2codi, DbType.Int32, eveninfn2codi);
            EveInformefallaN2DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {                    
                    entity = new EveInformefallaN2DTO();
                    
                    int iEveninfn2codi = dr.GetOrdinal(this.helper.Eveninfn2codi);
                    if (!dr.IsDBNull(iEveninfn2codi)) entity.Eveninfn2codi = Convert.ToInt32(dr.GetValue(iEveninfn2codi));

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEvenanio = dr.GetOrdinal(this.helper.Evenanio);
                    if (!dr.IsDBNull(iEvenanio)) entity.Evenanio = Convert.ToInt32(dr.GetValue(iEvenanio));

                    int iEvenn2corr = dr.GetOrdinal(this.helper.Evenn2corr);
                    if (!dr.IsDBNull(iEvenn2corr)) entity.Evenn2corr = Convert.ToInt32(dr.GetValue(iEvenn2corr));

                    int iEveninfpin2fechemis = dr.GetOrdinal(this.helper.Eveninfpin2fechemis);
                    if (!dr.IsDBNull(iEveninfpin2fechemis)) entity.Eveninfpin2fechemis = dr.GetDateTime(iEveninfpin2fechemis);

                    int iEveninfpin2emitido = dr.GetOrdinal(this.helper.Eveninfpin2emitido);
                    if (!dr.IsDBNull(iEveninfpin2emitido)) entity.Eveninfpin2emitido = dr.GetString(iEveninfpin2emitido);

                    int iEveninfpin2elab = dr.GetOrdinal(this.helper.Eveninfpin2elab);
                    if (!dr.IsDBNull(iEveninfpin2elab)) entity.Eveninfpin2elab = dr.GetString(iEveninfpin2elab);

                    int iEveninffn2emitido = dr.GetOrdinal(this.helper.Eveninffn2emitido);
                    if (!dr.IsDBNull(iEveninffn2emitido)) entity.Eveninffn2emitido = dr.GetString(iEveninffn2emitido);

                    int iEveninffn2elab = dr.GetOrdinal(this.helper.Eveninffn2elab);
                    if (!dr.IsDBNull(iEveninffn2elab)) entity.Eveninffn2elab = dr.GetString(iEveninffn2elab);

                    int iEveninfn2lastuser = dr.GetOrdinal(this.helper.Eveninfn2lastuser);
                    if (!dr.IsDBNull(iEveninfn2lastuser)) entity.Eveninfn2lastuser = dr.GetString(iEveninfn2lastuser);

                    int iEveninfn2lastdate = dr.GetOrdinal(this.helper.Eveninfn2lastdate);
                    if (!dr.IsDBNull(iEveninfn2lastdate)) entity.Eveninfn2lastdate = dr.GetDateTime(iEveninfn2lastdate);

                    int iEveninffn2fechemis = dr.GetOrdinal(this.helper.Eveninffn2fechemis);
                    if (!dr.IsDBNull(iEveninffn2fechemis)) entity.Eveninffn2fechemis = dr.GetDateTime(iEveninffn2fechemis);

                    int iEvenipiEN2emitido = dr.GetOrdinal(this.helper.EvenipiEN2emitido);
                    if (!dr.IsDBNull(iEvenipiEN2emitido)) entity.EvenipiEN2emitido = dr.GetString(iEvenipiEN2emitido);

                    int iEvenipiEN2elab = dr.GetOrdinal(this.helper.EvenipiEN2elab);
                    if (!dr.IsDBNull(iEvenipiEN2elab)) entity.EvenipiEN2elab = dr.GetString(iEvenipiEN2elab);

                    int iEvenipiEN2fechem = dr.GetOrdinal(this.helper.EvenipiEN2fechem);
                    if (!dr.IsDBNull(iEvenipiEN2fechem)) entity.EvenipiEN2fechem = dr.GetDateTime(iEvenipiEN2fechem);

                    int iEvenifEN2emitido = dr.GetOrdinal(this.helper.EvenifEN2emitido);
                    if (!dr.IsDBNull(iEvenifEN2emitido)) entity.EvenifEN2emitido = dr.GetString(iEvenifEN2emitido);

                    int iEvenifEN2elab = dr.GetOrdinal(this.helper.EvenifEN2elab);
                    if (!dr.IsDBNull(iEvenifEN2elab)) entity.EvenifEN2elab = dr.GetString(iEvenifEN2elab);

                    int iEvenifEN2fechem = dr.GetOrdinal(this.helper.EvenifEN2fechem);
                    if (!dr.IsDBNull(iEvenifEN2fechem)) entity.EvenifEN2fechem = dr.GetDateTime(iEvenifEN2fechem);
                                                            
                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                    
                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    
                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    
                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);
                                        
                    int iEvenmwindisp = dr.GetOrdinal(this.helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = Convert.ToDecimal(dr.GetValue(iEvenmwindisp));
                                        
                    int iTareaabrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);
                }
            }

            return entity;
        }

        public List<EveInformefallaN2DTO> List()
        {
            List<EveInformefallaN2DTO> entitys = new List<EveInformefallaN2DTO>();
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

        public List<EveInformefallaN2DTO> GetByCriteria()
        {
            List<EveInformefallaN2DTO> entitys = new List<EveInformefallaN2DTO>();
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

        public int ValidarInformeFallaN2(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarInformeFallaN2);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public void EliminarInformeFallaN2(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarInformeFallaN2);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ObtenerCorrelativoInformeFallaN2(int nroAnio, out int correlativo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCorrelativoInformeFallaN2);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, nroAnio);
           
            correlativo = 0;          

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {                  
                    int iEvencorr = dr.GetOrdinal(helper.Evenn2corr);
                    if (!dr.IsDBNull(iEvencorr)) correlativo = Convert.ToInt32(dr.GetValue(iEvencorr));                    
                }
            }
        }               
        

        public List<EveInformefallaN2DTO> BuscarOperaciones(string infEmitido, int emprCodi, string equiAbrev, DateTime fechaIni, DateTime fechaFin,
            int nroPage, int pageSize)
        {
            List<EveInformefallaN2DTO> entitys = new List<EveInformefallaN2DTO>();
            String sql = String.Format(this.helper.ObtenerListado, infEmitido, emprCodi,
               equiAbrev, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformefallaN2DTO entity = new EveInformefallaN2DTO();

                    int iEveninfn2codi = dr.GetOrdinal(this.helper.Eveninfn2codi);
                    if (!dr.IsDBNull(iEveninfn2codi)) entity.Eveninfn2codi = Convert.ToInt32(dr.GetValue(iEveninfn2codi));
                
                    int iEvenn2corr = dr.GetOrdinal(this.helper.Evenn2corr);
                    if (!dr.IsDBNull(iEvenn2corr)) entity.Evenn2corr = Convert.ToInt32(dr.GetValue(iEvenn2corr));
                                                       
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                                        
                    int iTareaabrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                                        
                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);
                                        
                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenmwindisp = dr.GetOrdinal(this.helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = Convert.ToDecimal(dr.GetValue(iEvenmwindisp));
                                        
                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iObsprelimini = dr.GetOrdinal(this.helper.ObsPrelimIni);
                    if (!dr.IsDBNull(iObsprelimini)) entity.Obsprelimini = dr.GetString(iObsprelimini);
                                       
                    int iObsfinal = dr.GetOrdinal(this.helper.ObsFinal);
                    if (!dr.IsDBNull(iObsfinal)) entity.Obsfinal = dr.GetString(iObsfinal);

                    int iEveninfn2lastuser = dr.GetOrdinal(this.helper.Eveninfn2lastuser);
                    if (!dr.IsDBNull(iEveninfn2lastuser)) entity.Eveninfn2lastuser = dr.GetString(iEveninfn2lastuser);

                    int iEveninfn2lastdate = dr.GetOrdinal(this.helper.Eveninfn2lastdate);
                    if (!dr.IsDBNull(iEveninfn2lastdate)) entity.Eveninfn2lastdate = dr.GetDateTime(iEveninfn2lastdate);
                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = dr.GetInt32(iEvencodi);
           
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string infEmitido, int emprCodi, string equiAbrev, DateTime fechaIni, DateTime fechaFin)
        {
            List<EveInformefallaN2DTO> entitys = new List<EveInformefallaN2DTO>();
            String sql = String.Format(this.helper.TotalRegistros, infEmitido, emprCodi,
              equiAbrev, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public EveInformefallaN2DTO MostrarEventoInformeFallaN2(int evencodi)
        {
            EveInformefallaN2DTO entity = new EveInformefallaN2DTO();
            String sql = String.Format(this.helper.SqlListarEventoInformeFallaN2, evencodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iEveninfn2codi = dr.GetOrdinal(this.helper.Eveninfn2codi);
                    if (!dr.IsDBNull(iEveninfn2codi)) entity.Eveninfn2codi = Convert.ToInt32(dr.GetValue(iEveninfn2codi));

                    int iEveninfplazodiasipi = dr.GetOrdinal(this.helper.Eveninfplazodiasipi);
                    if (!dr.IsDBNull(iEveninfplazodiasipi)) entity.Eveninfplazodiasipi = Convert.ToInt32(dr.GetValue(iEveninfplazodiasipi));

                    int iEveninfplazodiasif = dr.GetOrdinal(this.helper.Eveninfplazodiasif);
                    if (!dr.IsDBNull(iEveninfplazodiasif)) entity.Eveninfplazodiasif = Convert.ToInt32(dr.GetValue(iEveninfplazodiasif));

                    int iEveninfplazohoraipi = dr.GetOrdinal(this.helper.Eveninfplazohoraipi);
                    if (!dr.IsDBNull(iEveninfplazohoraipi)) entity.Eveninfplazohoraipi = Convert.ToInt32(dr.GetValue(iEveninfplazohoraipi));

                    int iEveninfplazohoraif = dr.GetOrdinal(this.helper.Eveninfplazohoraif);
                    if (!dr.IsDBNull(iEveninfplazohoraif)) entity.Eveninfplazohoraif = Convert.ToInt32(dr.GetValue(iEveninfplazohoraif));

                    int iEveninfplazominipi = dr.GetOrdinal(this.helper.Eveninfplazominipi);
                    if (!dr.IsDBNull(iEveninfplazominipi)) entity.Eveninfplazominipi = Convert.ToInt32(dr.GetValue(iEveninfplazominipi));

                    int iEveninfplazominif = dr.GetOrdinal(this.helper.Eveninfplazominif);
                    if (!dr.IsDBNull(iEveninfplazominif)) entity.Eveninfplazominif = Convert.ToInt32(dr.GetValue(iEveninfplazominif));

                    int iEvenn2corr = dr.GetOrdinal(this.helper.Evenn2corr);
                    if (!dr.IsDBNull(iEvenn2corr)) entity.Evenn2corr = Convert.ToInt32(dr.GetValue(iEvenn2corr));

                }
            }

            return entity;
        }


        public void ActualizarAmpliacionN2(EveInformefallaN2DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarAmpliacion);
           
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasipi, DbType.Int32, entity.Eveninfplazodiasipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasif, DbType.Int32, entity.Eveninfplazodiasif);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraipi, DbType.Int32, entity.Eveninfplazohoraipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraif, DbType.Int32, entity.Eveninfplazohoraif);
            dbProvider.AddInParameter(command, helper.Eveninfplazominipi, DbType.Int32, entity.Eveninfplazominipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazominif, DbType.Int32, entity.Eveninfplazominif);
            dbProvider.AddInParameter(command, helper.Eveninfn2codi, DbType.Int32, entity.Eveninfn2codi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
