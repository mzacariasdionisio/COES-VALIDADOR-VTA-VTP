// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 21/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

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

    public class VceArrparGrupoCabRepository : RepositoryBase, IVceArrparGrupoCabRepository
    {
        VceArrparGrupoCabHelper helper = new VceArrparGrupoCabHelper();

        public VceArrparGrupoCabRepository(string strConn)
            : base(strConn)
        {
        }

        public void Save(VceArrparGrupoCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apgcfccodi, DbType.String, entity.Apgcfccodi);

            //- Otros:
            dbProvider.AddInParameter(command, helper.Apgcabccbef, DbType.Decimal, entity.Apgcabccbef);
            dbProvider.AddInParameter(command, helper.Apgcabccmarr, DbType.Decimal, entity.Apgcabccmarr);
            dbProvider.AddInParameter(command, helper.Apgcabccadic, DbType.Decimal, entity.Apgcabccadic);
            dbProvider.AddInParameter(command, helper.Apgcabflagcalcmanual, DbType.String, entity.Apgcabflagcalcmanual);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceArrparGrupoCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Apgcabccbef, DbType.Decimal, entity.Apgcabccbef);
            dbProvider.AddInParameter(command, helper.Apgcabccmarr, DbType.Decimal, entity.Apgcabccmarr);
            dbProvider.AddInParameter(command, helper.Apgcabccadic, DbType.Decimal, entity.Apgcabccadic);
            dbProvider.AddInParameter(command, helper.Apgcabflagcalcmanual, DbType.String, entity.Apgcabflagcalcmanual);
            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apgcfccodi, DbType.String, entity.Apgcfccodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(VceArrparGrupoCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apgcfccodi, DbType.String, entity.Apgcfccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteEditCalculo(VceArrparGrupoCabDTO entity)
        {
            try
            {
                DbCommand command1 = dbProvider.GetSqlStringCommand(helper.SqlDeleteDetalle);

                //- Eliminar el detalle.
                dbProvider.AddInParameter(command1, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
                dbProvider.AddInParameter(command1, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
                dbProvider.ExecuteNonQuery(command1);

                //- Luego de eliminar el detalle eliminamos la cabecera.
                DbCommand command2 = dbProvider.GetSqlStringCommand(helper.SqlDeleteCabecera);
                dbProvider.AddInParameter(command2, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
                dbProvider.AddInParameter(command2, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
                dbProvider.ExecuteNonQuery(command2);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<VceArrparGrupoCabDTO> List()
        {
            throw new NotImplementedException();
        }

        public List<VceArrparGrupoCabDTO> GetByCriteria()
        {
            throw new NotImplementedException();
        }

        public List<VceArrparGrupoCabDTO> GetByPeriod(int codPeriodo)
        {
            List<VceArrparGrupoCabDTO> entities = new List<VceArrparGrupoCabDTO>();

            string queryString = string.Format(helper.SqlGetListaPorPeriodo, codPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceArrparGrupoCabDTO entity = new VceArrparGrupoCabDTO();

                    int iPecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iApgcfccodi = dr.GetOrdinal(helper.Apgcfccodi);
                    if (!dr.IsDBNull(iApgcfccodi)) entity.Apgcfccodi = dr.GetString(iApgcfccodi);

                    int iApgcabccbef = dr.GetOrdinal(helper.Apgcabccbef);
                    if (!dr.IsDBNull(iApgcabccbef)) entity.Apgcabccbef = dr.GetDecimal(iApgcabccbef);

                    int iApgcabccmarr = dr.GetOrdinal(helper.Apgcabccmarr);
                    if (!dr.IsDBNull(iApgcabccmarr)) entity.Apgcabccmarr = dr.GetDecimal(iApgcabccmarr);

                    int iApgcabccadic = dr.GetOrdinal(helper.Apgcabccadic);
                    if (!dr.IsDBNull(iApgcabccadic)) entity.Apgcabccadic = dr.GetDecimal(iApgcabccadic);

                    int iApgcabflagcalcmanual = dr.GetOrdinal(helper.Apgcabflagcalcmanual);
                    if (!dr.IsDBNull(iApgcabflagcalcmanual)) entity.Apgcabflagcalcmanual = dr.GetString(iApgcabflagcalcmanual);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public VceArrparGrupoCabDTO GetById(int PecaCodi, int GrupoCodi, string Apgcfccodi)
        {
            VceArrparGrupoCabDTO entity = new VceArrparGrupoCabDTO();

            string queryString = string.Format(helper.SqlGetById, PecaCodi, GrupoCodi, Apgcfccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entity = helper.Create(dr);
                    int iPecacodi = dr.GetOrdinal(helper.Pecacodi);
                    if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = dr.GetInt32(iPecacodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iApgcfccodi = dr.GetOrdinal(helper.Apgcfccodi);
                    if (!dr.IsDBNull(iApgcfccodi)) entity.Apgcfccodi = dr.GetString(iApgcfccodi);

                    int iApgcabccbef = dr.GetOrdinal(helper.Apgcabccbef);
                    if (!dr.IsDBNull(iApgcabccbef)) entity.Apgcabccbef = dr.GetDecimal(iApgcabccbef);

                    int iApgcabccmarr = dr.GetOrdinal(helper.Apgcabccmarr);
                    if (!dr.IsDBNull(iApgcabccmarr)) entity.Apgcabccmarr = dr.GetDecimal(iApgcabccmarr);

                    int iApgcabccadic = dr.GetOrdinal(helper.Apgcabccadic);
                    if (!dr.IsDBNull(iApgcabccadic)) entity.Apgcabccadic = dr.GetDecimal(iApgcabccadic);

                    int iApgcabflagcalcmanual = dr.GetOrdinal(helper.Apgcabflagcalcmanual);
                    if (!dr.IsDBNull(iApgcabflagcalcmanual)) entity.Apgcabflagcalcmanual = dr.GetString(iApgcabflagcalcmanual);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    
                }
            }

            return entity;
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen)
        {
            try
            {
                //- Grabar regitros de cabecera.
                string queryString = string.Format(helper.SqlSaveCabeceraFromOtherVersion, pecacodiDestino, pecacodiOrigen);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);

                //- Grabar registros de detalle
                queryString = string.Format(helper.SqlSaveDetalleFromOtherVersion, pecacodiDestino, pecacodiOrigen);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteByVersion(int pecacodi)
        {
            try
            {
                //- Eliminar registros de detalle
                string queryString = string.Format(helper.SqlDeleteDetalleByVersion, pecacodi);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);

                //- Eliminar regitros de cabecera.
                queryString = string.Format(helper.SqlDeleteCabeceraByVersion, pecacodi);
                command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public List<VceArrparGrupoCabDTO> ListByGroupCompArrpar(int pecacodi, string Apgcfccodi)
        {
            List<VceArrparGrupoCabDTO> entitys = new List<VceArrparGrupoCabDTO>();

            string sql = String.Format(helper.SqlListByGroupCompArrpar, pecacodi, Apgcfccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceArrparGrupoCabDTO entity = new VceArrparGrupoCabDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iApgcabccbef = dr.GetOrdinal(helper.Apgcabccbef);
                    if (!dr.IsDBNull(iApgcabccbef)) entity.Apgcabccbef = dr.GetDecimal(iApgcabccbef);

                    int iApgcabccmarr = dr.GetOrdinal(helper.Apgcabccmarr);
                    if (!dr.IsDBNull(iApgcabccmarr)) entity.Apgcabccmarr = dr.GetDecimal(iApgcabccmarr);

                    int iApgcabccadic = dr.GetOrdinal(helper.Apgcabccadic);
                    if (!dr.IsDBNull(iApgcabccadic)) entity.Apgcabccadic = dr.GetDecimal(iApgcabccadic);

                    int iApgcabtotal = dr.GetOrdinal(helper.Apgcabtotal);
                    if (!dr.IsDBNull(iApgcabtotal)) entity.Apgcabtotal = dr.GetDecimal(iApgcabtotal);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }

}
