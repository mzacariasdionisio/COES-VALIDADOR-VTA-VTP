using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class TransferenciaRetiroDetalleRepository : RepositoryBase, ITransferenciaRetiroDetalleRepository
    {
        public TransferenciaRetiroDetalleRepository(string strConn)
            : base(strConn)
        {
        }

        TransferenciaRetiroDetalleHelper helper = new TransferenciaRetiroDetalleHelper();

        public int Save(TransferenciaRetiroDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.TRANRETICODI, DbType.Int32, entity.TranRetiCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIDETACODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.TRANRETIDETAVERSION, DbType.Int32, entity.TranRetiDetaVersion);
            dbProvider.AddInParameter(command, helper.TRANRETIDETADIA, DbType.Int32, entity.TranRetiDetaDia);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAPROMDIA, DbType.Decimal, entity.TranRetiDetaPromDia);
            dbProvider.AddInParameter(command, helper.TRANRETIDETASUMADIA, DbType.Decimal, entity.TranRetiDetaSumaDia);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH1, DbType.Double, entity.TranRetiDetah1);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH2, DbType.Double, entity.TranRetiDetah2);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH3, DbType.Double, entity.TranRetiDetah3);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH4, DbType.Double, entity.TranRetiDetah4);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH5, DbType.Double, entity.TranRetiDetah5);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH6, DbType.Double, entity.TranRetiDetah6);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH7, DbType.Double, entity.TranRetiDetah7);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH8, DbType.Double, entity.TranRetiDetah8);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH9, DbType.Double, entity.TranRetiDetah9);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH10, DbType.Double, entity.TranRetiDetah10);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH11, DbType.Double, entity.TranRetiDetah11);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH12, DbType.Double, entity.TranRetiDetah12);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH13, DbType.Double, entity.TranRetiDetah13);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH14, DbType.Double, entity.TranRetiDetah14);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH15, DbType.Double, entity.TranRetiDetah15);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH16, DbType.Double, entity.TranRetiDetah16);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH17, DbType.Double, entity.TranRetiDetah17);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH18, DbType.Double, entity.TranRetiDetah18);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH19, DbType.Double, entity.TranRetiDetah19);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH20, DbType.Double, entity.TranRetiDetah20);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH21, DbType.Double, entity.TranRetiDetah21);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH22, DbType.Double, entity.TranRetiDetah22);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH23, DbType.Double, entity.TranRetiDetah23);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH24, DbType.Double, entity.TranRetiDetah24);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH25, DbType.Double, entity.TranRetiDetah25);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH26, DbType.Double, entity.TranRetiDetah26);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH27, DbType.Double, entity.TranRetiDetah27);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH28, DbType.Double, entity.TranRetiDetah28);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH29, DbType.Double, entity.TranRetiDetah29);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH30, DbType.Double, entity.TranRetiDetah30);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH31, DbType.Double, entity.TranRetiDetah31);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH32, DbType.Double, entity.TranRetiDetah32);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH33, DbType.Double, entity.TranRetiDetah33);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH34, DbType.Double, entity.TranRetiDetah34);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH35, DbType.Double, entity.TranRetiDetah35);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH36, DbType.Double, entity.TranRetiDetah36);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH37, DbType.Double, entity.TranRetiDetah37);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH38, DbType.Double, entity.TranRetiDetah38);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH39, DbType.Double, entity.TranRetiDetah39);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH40, DbType.Double, entity.TranRetiDetah40);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH41, DbType.Double, entity.TranRetiDetah41);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH42, DbType.Double, entity.TranRetiDetah42);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH43, DbType.Double, entity.TranRetiDetah43);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH44, DbType.Double, entity.TranRetiDetah44);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH45, DbType.Double, entity.TranRetiDetah45);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH46, DbType.Double, entity.TranRetiDetah46);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH47, DbType.Double, entity.TranRetiDetah47);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH48, DbType.Double, entity.TranRetiDetah48);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH49, DbType.Double, entity.TranRetiDetah49);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH50, DbType.Double, entity.TranRetiDetah50);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH51, DbType.Double, entity.TranRetiDetah51);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH52, DbType.Double, entity.TranRetiDetah52);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH53, DbType.Double, entity.TranRetiDetah53);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH54, DbType.Double, entity.TranRetiDetah54);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH55, DbType.Double, entity.TranRetiDetah55);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH56, DbType.Double, entity.TranRetiDetah56);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH57, DbType.Double, entity.TranRetiDetah57);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH58, DbType.Double, entity.TranRetiDetah58);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH59, DbType.Double, entity.TranRetiDetah59);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH60, DbType.Double, entity.TranRetiDetah60);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH61, DbType.Double, entity.TranRetiDetah61);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH62, DbType.Double, entity.TranRetiDetah62);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH63, DbType.Double, entity.TranRetiDetah63);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH64, DbType.Double, entity.TranRetiDetah64);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH65, DbType.Double, entity.TranRetiDetah65);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH66, DbType.Double, entity.TranRetiDetah66);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH67, DbType.Double, entity.TranRetiDetah67);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH68, DbType.Double, entity.TranRetiDetah68);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH69, DbType.Double, entity.TranRetiDetah69);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH70, DbType.Double, entity.TranRetiDetah70);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH71, DbType.Double, entity.TranRetiDetah71);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH72, DbType.Double, entity.TranRetiDetah72);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH73, DbType.Double, entity.TranRetiDetah73);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH74, DbType.Double, entity.TranRetiDetah74);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH75, DbType.Double, entity.TranRetiDetah75);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH76, DbType.Double, entity.TranRetiDetah76);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH77, DbType.Double, entity.TranRetiDetah77);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH78, DbType.Double, entity.TranRetiDetah78);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH79, DbType.Double, entity.TranRetiDetah79);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH80, DbType.Double, entity.TranRetiDetah80);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH81, DbType.Double, entity.TranRetiDetah81);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH82, DbType.Double, entity.TranRetiDetah82);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH83, DbType.Double, entity.TranRetiDetah83);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH84, DbType.Double, entity.TranRetiDetah84);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH85, DbType.Double, entity.TranRetiDetah85);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH86, DbType.Double, entity.TranRetiDetah86);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH87, DbType.Double, entity.TranRetiDetah87);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH88, DbType.Double, entity.TranRetiDetah88);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH89, DbType.Double, entity.TranRetiDetah89);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH90, DbType.Double, entity.TranRetiDetah90);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH91, DbType.Double, entity.TranRetiDetah91);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH92, DbType.Double, entity.TranRetiDetah92);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH93, DbType.Double, entity.TranRetiDetah93);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH94, DbType.Double, entity.TranRetiDetah94);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH95, DbType.Double, entity.TranRetiDetah95);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAH96, DbType.Double, entity.TranRetiDetah96);
            dbProvider.AddInParameter(command, helper.TRETDEUSERNAME, DbType.String, entity.TretdeUserName);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAFECINS, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TransferenciaRetiroDetalleDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Areanombre, DbType.String, entity.Areanombre);
            //dbProvider.AddInParameter(command, helper.Areaestado, DbType.String, "ACT");
            //dbProvider.AddInParameter(command, helper.Areafecact, DbType.DateTime, DateTime.Now);
            //dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);

            //dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pericodi, int version, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, sCodigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public TransferenciaRetiroDetalleDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TRANRETICODI, DbType.Int32, id);
            TransferenciaRetiroDetalleDTO entity = null;

            //using (IDataReader dr = dbProvider.ExecuteReader(command))
            //{
            //    if (dr.Read())
            //    {
            //        entity = helper.Create(dr);
            //    }
            //}

            return entity;
        }

        public List<TransferenciaRetiroDetalleDTO> List(int emprcodi, int pericodi)
        {
            List<TransferenciaRetiroDetalleDTO> entitys = new List<TransferenciaRetiroDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaRetiroDetalleDTO entity = new TransferenciaRetiroDetalleDTO();

                    int iSOLICODIRETICODIGO = dr.GetOrdinal(helper.SOLICODIRETICODIGO);
                    if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<TransferenciaRetiroDetalleDTO> GetByCriteria(int emprcodi, int pericodi, string solicodireticodigo, int version)
        {
            List<TransferenciaRetiroDetalleDTO> entitys = new List<TransferenciaRetiroDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, solicodireticodigo);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, solicodireticodigo);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);

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

        public List<TransferenciaRetiroDetalleDTO> GetByPeriodoVersion(int pericodi, int version)
        {
            List<TransferenciaRetiroDetalleDTO> entitys = new List<TransferenciaRetiroDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaRetiroDetalleDTO entity = new TransferenciaRetiroDetalleDTO();

                    int iEMPRCODI = dr.GetOrdinal(this.helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = dr.GetInt32(iEMPRCODI);

                    int iBARRCODI = dr.GetOrdinal(this.helper.BARRCODI);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iSOLICODIRETICODIGO = dr.GetOrdinal(this.helper.SOLICODIRETICODIGO);
                    if (!dr.IsDBNull(iSOLICODIRETICODIGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSOLICODIRETICODIGO);

                    int iCLICODI = dr.GetOrdinal(this.helper.CLICODI);
                    if (!dr.IsDBNull(iCLICODI)) entity.CliCodi = dr.GetInt32(iCLICODI);

                    int iPERICODI = dr.GetOrdinal(this.helper.PERICODI);
                    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

                    int iTRANRETIVERSION = dr.GetOrdinal(this.helper.TRANRETIVERSION);
                    if (!dr.IsDBNull(iTRANRETIVERSION)) entity.TretVersion = dr.GetInt32(iTRANRETIVERSION);

                    int iTRANRETITIPOINFORMACION = dr.GetOrdinal(this.helper.TRANRETITIPOINFORMACION);
                    if (!dr.IsDBNull(iTRANRETITIPOINFORMACION)) entity.TranRetiTipoInformacion = dr.GetString(iTRANRETITIPOINFORMACION);

                    int iTRETTABLA = dr.GetOrdinal(this.helper.TRETTABLA);
                    if (!dr.IsDBNull(iTRETTABLA)) entity.TretTabla = dr.GetString(iTRETTABLA);


                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<TransferenciaRetiroDetalleDTO> ListByTransferenciaRetiro(int iTRetCodi, int iTRetVersion)
        {
            List<TransferenciaRetiroDetalleDTO> entitys = new List<TransferenciaRetiroDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByTransferenciaRetiro);

            dbProvider.AddInParameter(command, helper.TRANRETICODI, DbType.Int32, iTRetCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIDETAVERSION, DbType.Int32, iTRetVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public void DeleteListaTransferenciaRetiroDetalle(int iPeriCodi, int iVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaRetiroDetalle);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iVersion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaTransferenciaRetiroDetalleEmpresa(int iPeriCodi, int iVersion, int iEmprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaRetiroDetalleEmpresa);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void BulkInsert(List<TrnTransRetiroDetalleBullk> entitys)
        {
            dbProvider.AddColumnMapping(helper.TRANRETICODI, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANRETIDETACODI, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAVERSION, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANRETIDETADIA, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAPROMDIA, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETASUMADIA, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAH96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRETDEUSERNAME, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAFECINS, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.TRANRETIDETAFECACT, DbType.DateTime);

            dbProvider.BulkInsert<TrnTransRetiroDetalleBullk>(entitys, helper.TableName);
        }

        public List<TransferenciaRetiroDetalleDTO> ListaTransferenciaRetiPorPericodiYVersion(int pericodi, int tretversion)
        {
            List<TransferenciaRetiroDetalleDTO> entitys = new List<TransferenciaRetiroDetalleDTO>();
            var query = string.Format(helper.SqlListaTransRetiroDetallePorPeriodoYVersion, pericodi, tretversion);

            using (DbCommand command = dbProvider.GetSqlStringCommand(query))
            {
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        var entity = new TransferenciaRetiroDetalleDTO();

                        int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                        if (!dr.IsDBNull(iEMPRCODI)) entity.Emprcodi = dr.GetInt32(iEMPRCODI);

                        int iEMPRNOMB = dr.GetOrdinal(helper.EMPRNOMB);
                        if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.GetString(iEMPRNOMB);

                        int iTRANRETIDETASUMADIA = dr.GetOrdinal(helper.TRANRETIDETASUMADIA);
                        if (!dr.IsDBNull(iTRANRETIDETASUMADIA)) entity.TranRetiDetaSumaDia = dr.GetDecimal(iTRANRETIDETASUMADIA);

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;

        }

        //ASSETEC - GRAN USUARIO - 28/11
        public List<TransferenciaRetiroDetalleDTO> ListByTransferenciaRetiroDay(int iTRetCodi)
        {
            List<TransferenciaRetiroDetalleDTO> entitys = new List<TransferenciaRetiroDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByTransferenciaRetiroDay);

            dbProvider.AddInParameter(command, helper.TRANRETICODI, DbType.Int32, iTRetCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public TransferenciaRetiroDetalleDTO GetDemandaRetiroByCodVtea(int pericodi, int version, string codvtea, int dia)
        {
            TransferenciaRetiroDetalleDTO entity = new TransferenciaRetiroDetalleDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDemandaRetiroByCodVtea);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, codvtea);
            dbProvider.AddInParameter(command, helper.TRANRETIDETADIA, DbType.Int32, dia);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iTRANENTRCODI = dr.GetOrdinal(helper.TRANRETICODI);
                    if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranRetiCodi = dr.GetInt32(iTRANENTRCODI);

                    int iTRANENTRDETADIA = dr.GetOrdinal(helper.TRANRETIDETADIA);
                    if (!dr.IsDBNull(iTRANENTRDETADIA)) entity.TranRetiDetaDia = dr.GetInt32(iTRANENTRDETADIA);

                    int iTRANRETIDETAH1 = dr.GetOrdinal(helper.TRANRETIDETAH1);
                    if (!dr.IsDBNull(iTRANRETIDETAH1)) entity.TranRetiDetah1 = dr.GetDecimal(iTRANRETIDETAH1);

                    int iTRANRETIDETAH2 = dr.GetOrdinal(helper.TRANRETIDETAH2);
                    if (!dr.IsDBNull(iTRANRETIDETAH2)) entity.TranRetiDetah2 = dr.GetDecimal(iTRANRETIDETAH2);

                    int iTRANRETIDETAH3 = dr.GetOrdinal(helper.TRANRETIDETAH3);
                    if (!dr.IsDBNull(iTRANRETIDETAH3)) entity.TranRetiDetah3 = dr.GetDecimal(iTRANRETIDETAH3);

                    int iTRANRETIDETAH4 = dr.GetOrdinal(helper.TRANRETIDETAH4);
                    if (!dr.IsDBNull(iTRANRETIDETAH4)) entity.TranRetiDetah4 = dr.GetDecimal(iTRANRETIDETAH4);

                    int iTRANRETIDETAH5 = dr.GetOrdinal(helper.TRANRETIDETAH5);
                    if (!dr.IsDBNull(iTRANRETIDETAH5)) entity.TranRetiDetah5 = dr.GetDecimal(iTRANRETIDETAH5);

                    int iTRANRETIDETAH6 = dr.GetOrdinal(helper.TRANRETIDETAH6);
                    if (!dr.IsDBNull(iTRANRETIDETAH6)) entity.TranRetiDetah6 = dr.GetDecimal(iTRANRETIDETAH6);

                    int iTRANRETIDETAH7 = dr.GetOrdinal(helper.TRANRETIDETAH7);
                    if (!dr.IsDBNull(iTRANRETIDETAH7)) entity.TranRetiDetah7 = dr.GetDecimal(iTRANRETIDETAH7);

                    int iTRANRETIDETAH8 = dr.GetOrdinal(helper.TRANRETIDETAH8);
                    if (!dr.IsDBNull(iTRANRETIDETAH8)) entity.TranRetiDetah8 = dr.GetDecimal(iTRANRETIDETAH8);

                    int iTRANRETIDETAH9 = dr.GetOrdinal(helper.TRANRETIDETAH9);
                    if (!dr.IsDBNull(iTRANRETIDETAH9)) entity.TranRetiDetah9 = dr.GetDecimal(iTRANRETIDETAH9);

                    int iTRANRETIDETAH10 = dr.GetOrdinal(helper.TRANRETIDETAH10);
                    if (!dr.IsDBNull(iTRANRETIDETAH10)) entity.TranRetiDetah10 = dr.GetDecimal(iTRANRETIDETAH10);

                    int iTRANRETIDETAH11 = dr.GetOrdinal(helper.TRANRETIDETAH11);
                    if (!dr.IsDBNull(iTRANRETIDETAH11)) entity.TranRetiDetah11 = dr.GetDecimal(iTRANRETIDETAH11);

                    int iTRANRETIDETAH12 = dr.GetOrdinal(helper.TRANRETIDETAH12);
                    if (!dr.IsDBNull(iTRANRETIDETAH12)) entity.TranRetiDetah12 = dr.GetDecimal(iTRANRETIDETAH12);

                    int iTRANRETIDETAH13 = dr.GetOrdinal(helper.TRANRETIDETAH13);
                    if (!dr.IsDBNull(iTRANRETIDETAH13)) entity.TranRetiDetah13 = dr.GetDecimal(iTRANRETIDETAH13);

                    int iTRANRETIDETAH14 = dr.GetOrdinal(helper.TRANRETIDETAH14);
                    if (!dr.IsDBNull(iTRANRETIDETAH14)) entity.TranRetiDetah14 = dr.GetDecimal(iTRANRETIDETAH14);

                    int iTRANRETIDETAH15 = dr.GetOrdinal(helper.TRANRETIDETAH15);
                    if (!dr.IsDBNull(iTRANRETIDETAH15)) entity.TranRetiDetah15 = dr.GetDecimal(iTRANRETIDETAH15);

                    int iTRANRETIDETAH16 = dr.GetOrdinal(helper.TRANRETIDETAH16);
                    if (!dr.IsDBNull(iTRANRETIDETAH16)) entity.TranRetiDetah16 = dr.GetDecimal(iTRANRETIDETAH16);

                    int iTRANRETIDETAH17 = dr.GetOrdinal(helper.TRANRETIDETAH17);
                    if (!dr.IsDBNull(iTRANRETIDETAH17)) entity.TranRetiDetah17 = dr.GetDecimal(iTRANRETIDETAH17);

                    int iTRANRETIDETAH18 = dr.GetOrdinal(helper.TRANRETIDETAH18);
                    if (!dr.IsDBNull(iTRANRETIDETAH18)) entity.TranRetiDetah18 = dr.GetDecimal(iTRANRETIDETAH18);

                    int iTRANRETIDETAH19 = dr.GetOrdinal(helper.TRANRETIDETAH19);
                    if (!dr.IsDBNull(iTRANRETIDETAH19)) entity.TranRetiDetah19 = dr.GetDecimal(iTRANRETIDETAH19);

                    int iTRANRETIDETAH20 = dr.GetOrdinal(helper.TRANRETIDETAH20);
                    if (!dr.IsDBNull(iTRANRETIDETAH20)) entity.TranRetiDetah20 = dr.GetDecimal(iTRANRETIDETAH20);

                    int iTRANRETIDETAH21 = dr.GetOrdinal(helper.TRANRETIDETAH21);
                    if (!dr.IsDBNull(iTRANRETIDETAH21)) entity.TranRetiDetah21 = dr.GetDecimal(iTRANRETIDETAH21);

                    int iTRANRETIDETAH22 = dr.GetOrdinal(helper.TRANRETIDETAH22);
                    if (!dr.IsDBNull(iTRANRETIDETAH22)) entity.TranRetiDetah22 = dr.GetDecimal(iTRANRETIDETAH22);

                    int iTRANRETIDETAH23 = dr.GetOrdinal(helper.TRANRETIDETAH23);
                    if (!dr.IsDBNull(iTRANRETIDETAH23)) entity.TranRetiDetah23 = dr.GetDecimal(iTRANRETIDETAH23);

                    int iTRANRETIDETAH24 = dr.GetOrdinal(helper.TRANRETIDETAH24);
                    if (!dr.IsDBNull(iTRANRETIDETAH24)) entity.TranRetiDetah24 = dr.GetDecimal(iTRANRETIDETAH24);

                    int iTRANRETIDETAH25 = dr.GetOrdinal(helper.TRANRETIDETAH25);
                    if (!dr.IsDBNull(iTRANRETIDETAH25)) entity.TranRetiDetah25 = dr.GetDecimal(iTRANRETIDETAH25);

                    int iTRANRETIDETAH26 = dr.GetOrdinal(helper.TRANRETIDETAH26);
                    if (!dr.IsDBNull(iTRANRETIDETAH26)) entity.TranRetiDetah26 = dr.GetDecimal(iTRANRETIDETAH26);

                    int iTRANRETIDETAH27 = dr.GetOrdinal(helper.TRANRETIDETAH27);
                    if (!dr.IsDBNull(iTRANRETIDETAH27)) entity.TranRetiDetah27 = dr.GetDecimal(iTRANRETIDETAH27);

                    int iTRANRETIDETAH28 = dr.GetOrdinal(helper.TRANRETIDETAH28);
                    if (!dr.IsDBNull(iTRANRETIDETAH28)) entity.TranRetiDetah28 = dr.GetDecimal(iTRANRETIDETAH28);

                    int iTRANRETIDETAH29 = dr.GetOrdinal(helper.TRANRETIDETAH29);
                    if (!dr.IsDBNull(iTRANRETIDETAH29)) entity.TranRetiDetah29 = dr.GetDecimal(iTRANRETIDETAH29);

                    int iTRANRETIDETAH30 = dr.GetOrdinal(helper.TRANRETIDETAH30);
                    if (!dr.IsDBNull(iTRANRETIDETAH30)) entity.TranRetiDetah30 = dr.GetDecimal(iTRANRETIDETAH30);

                    int iTRANRETIDETAH31 = dr.GetOrdinal(helper.TRANRETIDETAH31);
                    if (!dr.IsDBNull(iTRANRETIDETAH31)) entity.TranRetiDetah31 = dr.GetDecimal(iTRANRETIDETAH31);

                    int iTRANRETIDETAH32 = dr.GetOrdinal(helper.TRANRETIDETAH32);
                    if (!dr.IsDBNull(iTRANRETIDETAH32)) entity.TranRetiDetah32 = dr.GetDecimal(iTRANRETIDETAH32);

                    int iTRANRETIDETAH33 = dr.GetOrdinal(helper.TRANRETIDETAH33);
                    if (!dr.IsDBNull(iTRANRETIDETAH33)) entity.TranRetiDetah33 = dr.GetDecimal(iTRANRETIDETAH33);

                    int iTRANRETIDETAH34 = dr.GetOrdinal(helper.TRANRETIDETAH34);
                    if (!dr.IsDBNull(iTRANRETIDETAH34)) entity.TranRetiDetah34 = dr.GetDecimal(iTRANRETIDETAH34);

                    int iTRANRETIDETAH35 = dr.GetOrdinal(helper.TRANRETIDETAH35);
                    if (!dr.IsDBNull(iTRANRETIDETAH35)) entity.TranRetiDetah35 = dr.GetDecimal(iTRANRETIDETAH35);

                    int iTRANRETIDETAH36 = dr.GetOrdinal(helper.TRANRETIDETAH36);
                    if (!dr.IsDBNull(iTRANRETIDETAH36)) entity.TranRetiDetah36 = dr.GetDecimal(iTRANRETIDETAH36);

                    int iTRANRETIDETAH37 = dr.GetOrdinal(helper.TRANRETIDETAH37);
                    if (!dr.IsDBNull(iTRANRETIDETAH37)) entity.TranRetiDetah37 = dr.GetDecimal(iTRANRETIDETAH37);

                    int iTRANRETIDETAH38 = dr.GetOrdinal(helper.TRANRETIDETAH38);
                    if (!dr.IsDBNull(iTRANRETIDETAH38)) entity.TranRetiDetah38 = dr.GetDecimal(iTRANRETIDETAH38);

                    int iTRANRETIDETAH39 = dr.GetOrdinal(helper.TRANRETIDETAH39);
                    if (!dr.IsDBNull(iTRANRETIDETAH39)) entity.TranRetiDetah39 = dr.GetDecimal(iTRANRETIDETAH39);

                    int iTRANRETIDETAH40 = dr.GetOrdinal(helper.TRANRETIDETAH40);
                    if (!dr.IsDBNull(iTRANRETIDETAH40)) entity.TranRetiDetah40 = dr.GetDecimal(iTRANRETIDETAH40);

                    int iTRANRETIDETAH41 = dr.GetOrdinal(helper.TRANRETIDETAH41);
                    if (!dr.IsDBNull(iTRANRETIDETAH41)) entity.TranRetiDetah41 = dr.GetDecimal(iTRANRETIDETAH41);

                    int iTRANRETIDETAH42 = dr.GetOrdinal(helper.TRANRETIDETAH42);
                    if (!dr.IsDBNull(iTRANRETIDETAH42)) entity.TranRetiDetah42 = dr.GetDecimal(iTRANRETIDETAH42);

                    int iTRANRETIDETAH43 = dr.GetOrdinal(helper.TRANRETIDETAH43);
                    if (!dr.IsDBNull(iTRANRETIDETAH43)) entity.TranRetiDetah43 = dr.GetDecimal(iTRANRETIDETAH43);

                    int iTRANRETIDETAH44 = dr.GetOrdinal(helper.TRANRETIDETAH44);
                    if (!dr.IsDBNull(iTRANRETIDETAH44)) entity.TranRetiDetah44 = dr.GetDecimal(iTRANRETIDETAH44);

                    int iTRANRETIDETAH45 = dr.GetOrdinal(helper.TRANRETIDETAH45);
                    if (!dr.IsDBNull(iTRANRETIDETAH45)) entity.TranRetiDetah45 = dr.GetDecimal(iTRANRETIDETAH45);

                    int iTRANRETIDETAH46 = dr.GetOrdinal(helper.TRANRETIDETAH46);
                    if (!dr.IsDBNull(iTRANRETIDETAH46)) entity.TranRetiDetah46 = dr.GetDecimal(iTRANRETIDETAH46);

                    int iTRANRETIDETAH47 = dr.GetOrdinal(helper.TRANRETIDETAH47);
                    if (!dr.IsDBNull(iTRANRETIDETAH47)) entity.TranRetiDetah47 = dr.GetDecimal(iTRANRETIDETAH47);

                    int iTRANRETIDETAH48 = dr.GetOrdinal(helper.TRANRETIDETAH48);
                    if (!dr.IsDBNull(iTRANRETIDETAH48)) entity.TranRetiDetah48 = dr.GetDecimal(iTRANRETIDETAH48);

                    int iTRANRETIDETAH49 = dr.GetOrdinal(helper.TRANRETIDETAH49);
                    if (!dr.IsDBNull(iTRANRETIDETAH49)) entity.TranRetiDetah49 = dr.GetDecimal(iTRANRETIDETAH49);

                    int iTRANRETIDETAH50 = dr.GetOrdinal(helper.TRANRETIDETAH50);
                    if (!dr.IsDBNull(iTRANRETIDETAH50)) entity.TranRetiDetah50 = dr.GetDecimal(iTRANRETIDETAH50);

                    int iTRANRETIDETAH51 = dr.GetOrdinal(helper.TRANRETIDETAH51);
                    if (!dr.IsDBNull(iTRANRETIDETAH50)) entity.TranRetiDetah51 = dr.GetDecimal(iTRANRETIDETAH51);

                    int iTRANRETIDETAH52 = dr.GetOrdinal(helper.TRANRETIDETAH52);
                    if (!dr.IsDBNull(iTRANRETIDETAH52)) entity.TranRetiDetah52 = dr.GetDecimal(iTRANRETIDETAH52);

                    int iTRANRETIDETAH53 = dr.GetOrdinal(helper.TRANRETIDETAH53);
                    if (!dr.IsDBNull(iTRANRETIDETAH53)) entity.TranRetiDetah53 = dr.GetDecimal(iTRANRETIDETAH53);

                    int iTRANRETIDETAH54 = dr.GetOrdinal(helper.TRANRETIDETAH54);
                    if (!dr.IsDBNull(iTRANRETIDETAH54)) entity.TranRetiDetah54 = dr.GetDecimal(iTRANRETIDETAH54);

                    int iTRANRETIDETAH55 = dr.GetOrdinal(helper.TRANRETIDETAH55);
                    if (!dr.IsDBNull(iTRANRETIDETAH55)) entity.TranRetiDetah55 = dr.GetDecimal(iTRANRETIDETAH55);

                    int iTRANRETIDETAH56 = dr.GetOrdinal(helper.TRANRETIDETAH56);
                    if (!dr.IsDBNull(iTRANRETIDETAH56)) entity.TranRetiDetah56 = dr.GetDecimal(iTRANRETIDETAH56);

                    int iTRANRETIDETAH57 = dr.GetOrdinal(helper.TRANRETIDETAH57);
                    if (!dr.IsDBNull(iTRANRETIDETAH57)) entity.TranRetiDetah57 = dr.GetDecimal(iTRANRETIDETAH57);

                    int iTRANRETIDETAH58 = dr.GetOrdinal(helper.TRANRETIDETAH58);
                    if (!dr.IsDBNull(iTRANRETIDETAH58)) entity.TranRetiDetah58 = dr.GetDecimal(iTRANRETIDETAH58);

                    int iTRANRETIDETAH59 = dr.GetOrdinal(helper.TRANRETIDETAH59);
                    if (!dr.IsDBNull(iTRANRETIDETAH59)) entity.TranRetiDetah59 = dr.GetDecimal(iTRANRETIDETAH59);

                    int iTRANRETIDETAH60 = dr.GetOrdinal(helper.TRANRETIDETAH60);
                    if (!dr.IsDBNull(iTRANRETIDETAH60)) entity.TranRetiDetah60 = dr.GetDecimal(iTRANRETIDETAH60);

                    int iTRANRETIDETAH61 = dr.GetOrdinal(helper.TRANRETIDETAH61);
                    if (!dr.IsDBNull(iTRANRETIDETAH61)) entity.TranRetiDetah61 = dr.GetDecimal(iTRANRETIDETAH61);

                    int iTRANRETIDETAH62 = dr.GetOrdinal(helper.TRANRETIDETAH62);
                    if (!dr.IsDBNull(iTRANRETIDETAH62)) entity.TranRetiDetah62 = dr.GetDecimal(iTRANRETIDETAH62);

                    int iTRANRETIDETAH63 = dr.GetOrdinal(helper.TRANRETIDETAH63);
                    if (!dr.IsDBNull(iTRANRETIDETAH63)) entity.TranRetiDetah63 = dr.GetDecimal(iTRANRETIDETAH63);

                    int iTRANRETIDETAH64 = dr.GetOrdinal(helper.TRANRETIDETAH64);
                    if (!dr.IsDBNull(iTRANRETIDETAH64)) entity.TranRetiDetah64 = dr.GetDecimal(iTRANRETIDETAH64);

                    int iTRANRETIDETAH65 = dr.GetOrdinal(helper.TRANRETIDETAH65);
                    if (!dr.IsDBNull(iTRANRETIDETAH65)) entity.TranRetiDetah65 = dr.GetDecimal(iTRANRETIDETAH65);

                    int iTRANRETIDETAH66 = dr.GetOrdinal(helper.TRANRETIDETAH66);
                    if (!dr.IsDBNull(iTRANRETIDETAH66)) entity.TranRetiDetah66 = dr.GetDecimal(iTRANRETIDETAH66);

                    int iTRANRETIDETAH67 = dr.GetOrdinal(helper.TRANRETIDETAH67);
                    if (!dr.IsDBNull(iTRANRETIDETAH67)) entity.TranRetiDetah67 = dr.GetDecimal(iTRANRETIDETAH67);

                    int iTRANRETIDETAH68 = dr.GetOrdinal(helper.TRANRETIDETAH68);
                    if (!dr.IsDBNull(iTRANRETIDETAH68)) entity.TranRetiDetah68 = dr.GetDecimal(iTRANRETIDETAH68);

                    int iTRANRETIDETAH69 = dr.GetOrdinal(helper.TRANRETIDETAH69);
                    if (!dr.IsDBNull(iTRANRETIDETAH69)) entity.TranRetiDetah69 = dr.GetDecimal(iTRANRETIDETAH69);

                    int iTRANRETIDETAH70 = dr.GetOrdinal(helper.TRANRETIDETAH70);
                    if (!dr.IsDBNull(iTRANRETIDETAH70)) entity.TranRetiDetah70 = dr.GetDecimal(iTRANRETIDETAH70);

                    int iTRANRETIDETAH71 = dr.GetOrdinal(helper.TRANRETIDETAH71);
                    if (!dr.IsDBNull(iTRANRETIDETAH71)) entity.TranRetiDetah71 = dr.GetDecimal(iTRANRETIDETAH71);

                    int iTRANRETIDETAH72 = dr.GetOrdinal(helper.TRANRETIDETAH72);
                    if (!dr.IsDBNull(iTRANRETIDETAH72)) entity.TranRetiDetah72 = dr.GetDecimal(iTRANRETIDETAH72);

                    int iTRANRETIDETAH73 = dr.GetOrdinal(helper.TRANRETIDETAH73);
                    if (!dr.IsDBNull(iTRANRETIDETAH73)) entity.TranRetiDetah73 = dr.GetDecimal(iTRANRETIDETAH73);

                    int iTRANRETIDETAH74 = dr.GetOrdinal(helper.TRANRETIDETAH74);
                    if (!dr.IsDBNull(iTRANRETIDETAH74)) entity.TranRetiDetah74 = dr.GetDecimal(iTRANRETIDETAH74);

                    int iTRANRETIDETAH75 = dr.GetOrdinal(helper.TRANRETIDETAH75);
                    if (!dr.IsDBNull(iTRANRETIDETAH75)) entity.TranRetiDetah75 = dr.GetDecimal(iTRANRETIDETAH75);

                    int iTRANRETIDETAH76 = dr.GetOrdinal(helper.TRANRETIDETAH76);
                    if (!dr.IsDBNull(iTRANRETIDETAH76)) entity.TranRetiDetah76 = dr.GetDecimal(iTRANRETIDETAH76);

                    int iTRANRETIDETAH77 = dr.GetOrdinal(helper.TRANRETIDETAH77);
                    if (!dr.IsDBNull(iTRANRETIDETAH77)) entity.TranRetiDetah77 = dr.GetDecimal(iTRANRETIDETAH77);

                    int iTRANRETIDETAH78 = dr.GetOrdinal(helper.TRANRETIDETAH78);
                    if (!dr.IsDBNull(iTRANRETIDETAH78)) entity.TranRetiDetah78 = dr.GetDecimal(iTRANRETIDETAH78);

                    int iTRANRETIDETAH79 = dr.GetOrdinal(helper.TRANRETIDETAH79);
                    if (!dr.IsDBNull(iTRANRETIDETAH79)) entity.TranRetiDetah79 = dr.GetDecimal(iTRANRETIDETAH79);

                    int iTRANRETIDETAH80 = dr.GetOrdinal(helper.TRANRETIDETAH80);
                    if (!dr.IsDBNull(iTRANRETIDETAH80)) entity.TranRetiDetah80 = dr.GetDecimal(iTRANRETIDETAH80);

                    int iTRANRETIDETAH81 = dr.GetOrdinal(helper.TRANRETIDETAH81);
                    if (!dr.IsDBNull(iTRANRETIDETAH81)) entity.TranRetiDetah81 = dr.GetDecimal(iTRANRETIDETAH81);

                    int iTRANRETIDETAH82 = dr.GetOrdinal(helper.TRANRETIDETAH82);
                    if (!dr.IsDBNull(iTRANRETIDETAH82)) entity.TranRetiDetah82 = dr.GetDecimal(iTRANRETIDETAH82);

                    int iTRANRETIDETAH83 = dr.GetOrdinal(helper.TRANRETIDETAH83);
                    if (!dr.IsDBNull(iTRANRETIDETAH83)) entity.TranRetiDetah83 = dr.GetDecimal(iTRANRETIDETAH83);

                    int iTRANRETIDETAH84 = dr.GetOrdinal(helper.TRANRETIDETAH84);
                    if (!dr.IsDBNull(iTRANRETIDETAH84)) entity.TranRetiDetah84 = dr.GetDecimal(iTRANRETIDETAH84);

                    int iTRANRETIDETAH85 = dr.GetOrdinal(helper.TRANRETIDETAH85);
                    if (!dr.IsDBNull(iTRANRETIDETAH85)) entity.TranRetiDetah85 = dr.GetDecimal(iTRANRETIDETAH85);

                    int iTRANRETIDETAH86 = dr.GetOrdinal(helper.TRANRETIDETAH86);
                    if (!dr.IsDBNull(iTRANRETIDETAH86)) entity.TranRetiDetah86 = dr.GetDecimal(iTRANRETIDETAH86);

                    int iTRANRETIDETAH87 = dr.GetOrdinal(helper.TRANRETIDETAH87);
                    if (!dr.IsDBNull(iTRANRETIDETAH87)) entity.TranRetiDetah87 = dr.GetDecimal(iTRANRETIDETAH87);

                    int iTRANRETIDETAH88 = dr.GetOrdinal(helper.TRANRETIDETAH88);
                    if (!dr.IsDBNull(iTRANRETIDETAH88)) entity.TranRetiDetah88 = dr.GetDecimal(iTRANRETIDETAH88);

                    int iTRANRETIDETAH89 = dr.GetOrdinal(helper.TRANRETIDETAH89);
                    if (!dr.IsDBNull(iTRANRETIDETAH89)) entity.TranRetiDetah89 = dr.GetDecimal(iTRANRETIDETAH89);

                    int iTRANRETIDETAH90 = dr.GetOrdinal(helper.TRANRETIDETAH90);
                    if (!dr.IsDBNull(iTRANRETIDETAH90)) entity.TranRetiDetah90 = dr.GetDecimal(iTRANRETIDETAH90);

                    int iTRANRETIDETAH91 = dr.GetOrdinal(helper.TRANRETIDETAH91);
                    if (!dr.IsDBNull(iTRANRETIDETAH91)) entity.TranRetiDetah91 = dr.GetDecimal(iTRANRETIDETAH91);

                    int iTRANRETIDETAH92 = dr.GetOrdinal(helper.TRANRETIDETAH92);
                    if (!dr.IsDBNull(iTRANRETIDETAH92)) entity.TranRetiDetah92 = dr.GetDecimal(iTRANRETIDETAH92);

                    int iTRANRETIDETAH93 = dr.GetOrdinal(helper.TRANRETIDETAH93);
                    if (!dr.IsDBNull(iTRANRETIDETAH93)) entity.TranRetiDetah93 = dr.GetDecimal(iTRANRETIDETAH93);

                    int iTRANRETIDETAH94 = dr.GetOrdinal(helper.TRANRETIDETAH94);
                    if (!dr.IsDBNull(iTRANRETIDETAH94)) entity.TranRetiDetah94 = dr.GetDecimal(iTRANRETIDETAH94);

                    int iTRANRETIDETAH95 = dr.GetOrdinal(helper.TRANRETIDETAH95);
                    if (!dr.IsDBNull(iTRANRETIDETAH95)) entity.TranRetiDetah95 = dr.GetDecimal(iTRANRETIDETAH95);

                    int iTRANRETIDETAH96 = dr.GetOrdinal(helper.TRANRETIDETAH96);
                    if (!dr.IsDBNull(iTRANRETIDETAH96)) entity.TranRetiDetah96 = dr.GetDecimal(iTRANRETIDETAH96);

                }
                return entity;
            }

        }
        public TransferenciaRetiroDetalleDTO GetDemandaRetiroByCodVteaEmpresa(int pericodi, int version, string codvtea, int dia, int emprcodi)
        {
            TransferenciaRetiroDetalleDTO entity = new TransferenciaRetiroDetalleDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDemandaRetiroByCodVteaEmpresa);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.SOLICODIRETICODIGO, DbType.String, codvtea);
            dbProvider.AddInParameter(command, helper.TRANRETIDETADIA, DbType.Int32, dia);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANRETIVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iTRANENTRCODI = dr.GetOrdinal(helper.TRANRETICODI);
                    if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranRetiCodi = dr.GetInt32(iTRANENTRCODI);

                    int iTRANENTRDETADIA = dr.GetOrdinal(helper.TRANRETIDETADIA);
                    if (!dr.IsDBNull(iTRANENTRDETADIA)) entity.TranRetiDetaDia = dr.GetInt32(iTRANENTRDETADIA);

                    int iTRANRETIDETAH1 = dr.GetOrdinal(helper.TRANRETIDETAH1);
                    if (!dr.IsDBNull(iTRANRETIDETAH1)) entity.TranRetiDetah1 = dr.GetDecimal(iTRANRETIDETAH1);

                    int iTRANRETIDETAH2 = dr.GetOrdinal(helper.TRANRETIDETAH2);
                    if (!dr.IsDBNull(iTRANRETIDETAH2)) entity.TranRetiDetah2 = dr.GetDecimal(iTRANRETIDETAH2);

                    int iTRANRETIDETAH3 = dr.GetOrdinal(helper.TRANRETIDETAH3);
                    if (!dr.IsDBNull(iTRANRETIDETAH3)) entity.TranRetiDetah3 = dr.GetDecimal(iTRANRETIDETAH3);

                    int iTRANRETIDETAH4 = dr.GetOrdinal(helper.TRANRETIDETAH4);
                    if (!dr.IsDBNull(iTRANRETIDETAH4)) entity.TranRetiDetah4 = dr.GetDecimal(iTRANRETIDETAH4);

                    int iTRANRETIDETAH5 = dr.GetOrdinal(helper.TRANRETIDETAH5);
                    if (!dr.IsDBNull(iTRANRETIDETAH5)) entity.TranRetiDetah5 = dr.GetDecimal(iTRANRETIDETAH5);

                    int iTRANRETIDETAH6 = dr.GetOrdinal(helper.TRANRETIDETAH6);
                    if (!dr.IsDBNull(iTRANRETIDETAH6)) entity.TranRetiDetah6 = dr.GetDecimal(iTRANRETIDETAH6);

                    int iTRANRETIDETAH7 = dr.GetOrdinal(helper.TRANRETIDETAH7);
                    if (!dr.IsDBNull(iTRANRETIDETAH7)) entity.TranRetiDetah7 = dr.GetDecimal(iTRANRETIDETAH7);

                    int iTRANRETIDETAH8 = dr.GetOrdinal(helper.TRANRETIDETAH8);
                    if (!dr.IsDBNull(iTRANRETIDETAH8)) entity.TranRetiDetah8 = dr.GetDecimal(iTRANRETIDETAH8);

                    int iTRANRETIDETAH9 = dr.GetOrdinal(helper.TRANRETIDETAH9);
                    if (!dr.IsDBNull(iTRANRETIDETAH9)) entity.TranRetiDetah9 = dr.GetDecimal(iTRANRETIDETAH9);

                    int iTRANRETIDETAH10 = dr.GetOrdinal(helper.TRANRETIDETAH10);
                    if (!dr.IsDBNull(iTRANRETIDETAH10)) entity.TranRetiDetah10 = dr.GetDecimal(iTRANRETIDETAH10);

                    int iTRANRETIDETAH11 = dr.GetOrdinal(helper.TRANRETIDETAH11);
                    if (!dr.IsDBNull(iTRANRETIDETAH11)) entity.TranRetiDetah11 = dr.GetDecimal(iTRANRETIDETAH11);

                    int iTRANRETIDETAH12 = dr.GetOrdinal(helper.TRANRETIDETAH12);
                    if (!dr.IsDBNull(iTRANRETIDETAH12)) entity.TranRetiDetah12 = dr.GetDecimal(iTRANRETIDETAH12);

                    int iTRANRETIDETAH13 = dr.GetOrdinal(helper.TRANRETIDETAH13);
                    if (!dr.IsDBNull(iTRANRETIDETAH13)) entity.TranRetiDetah13 = dr.GetDecimal(iTRANRETIDETAH13);

                    int iTRANRETIDETAH14 = dr.GetOrdinal(helper.TRANRETIDETAH14);
                    if (!dr.IsDBNull(iTRANRETIDETAH14)) entity.TranRetiDetah14 = dr.GetDecimal(iTRANRETIDETAH14);

                    int iTRANRETIDETAH15 = dr.GetOrdinal(helper.TRANRETIDETAH15);
                    if (!dr.IsDBNull(iTRANRETIDETAH15)) entity.TranRetiDetah15 = dr.GetDecimal(iTRANRETIDETAH15);

                    int iTRANRETIDETAH16 = dr.GetOrdinal(helper.TRANRETIDETAH16);
                    if (!dr.IsDBNull(iTRANRETIDETAH16)) entity.TranRetiDetah16 = dr.GetDecimal(iTRANRETIDETAH16);

                    int iTRANRETIDETAH17 = dr.GetOrdinal(helper.TRANRETIDETAH17);
                    if (!dr.IsDBNull(iTRANRETIDETAH17)) entity.TranRetiDetah17 = dr.GetDecimal(iTRANRETIDETAH17);

                    int iTRANRETIDETAH18 = dr.GetOrdinal(helper.TRANRETIDETAH18);
                    if (!dr.IsDBNull(iTRANRETIDETAH18)) entity.TranRetiDetah18 = dr.GetDecimal(iTRANRETIDETAH18);

                    int iTRANRETIDETAH19 = dr.GetOrdinal(helper.TRANRETIDETAH19);
                    if (!dr.IsDBNull(iTRANRETIDETAH19)) entity.TranRetiDetah19 = dr.GetDecimal(iTRANRETIDETAH19);

                    int iTRANRETIDETAH20 = dr.GetOrdinal(helper.TRANRETIDETAH20);
                    if (!dr.IsDBNull(iTRANRETIDETAH20)) entity.TranRetiDetah20 = dr.GetDecimal(iTRANRETIDETAH20);

                    int iTRANRETIDETAH21 = dr.GetOrdinal(helper.TRANRETIDETAH21);
                    if (!dr.IsDBNull(iTRANRETIDETAH21)) entity.TranRetiDetah21 = dr.GetDecimal(iTRANRETIDETAH21);

                    int iTRANRETIDETAH22 = dr.GetOrdinal(helper.TRANRETIDETAH22);
                    if (!dr.IsDBNull(iTRANRETIDETAH22)) entity.TranRetiDetah22 = dr.GetDecimal(iTRANRETIDETAH22);

                    int iTRANRETIDETAH23 = dr.GetOrdinal(helper.TRANRETIDETAH23);
                    if (!dr.IsDBNull(iTRANRETIDETAH23)) entity.TranRetiDetah23 = dr.GetDecimal(iTRANRETIDETAH23);

                    int iTRANRETIDETAH24 = dr.GetOrdinal(helper.TRANRETIDETAH24);
                    if (!dr.IsDBNull(iTRANRETIDETAH24)) entity.TranRetiDetah24 = dr.GetDecimal(iTRANRETIDETAH24);

                    int iTRANRETIDETAH25 = dr.GetOrdinal(helper.TRANRETIDETAH25);
                    if (!dr.IsDBNull(iTRANRETIDETAH25)) entity.TranRetiDetah25 = dr.GetDecimal(iTRANRETIDETAH25);

                    int iTRANRETIDETAH26 = dr.GetOrdinal(helper.TRANRETIDETAH26);
                    if (!dr.IsDBNull(iTRANRETIDETAH26)) entity.TranRetiDetah26 = dr.GetDecimal(iTRANRETIDETAH26);

                    int iTRANRETIDETAH27 = dr.GetOrdinal(helper.TRANRETIDETAH27);
                    if (!dr.IsDBNull(iTRANRETIDETAH27)) entity.TranRetiDetah27 = dr.GetDecimal(iTRANRETIDETAH27);

                    int iTRANRETIDETAH28 = dr.GetOrdinal(helper.TRANRETIDETAH28);
                    if (!dr.IsDBNull(iTRANRETIDETAH28)) entity.TranRetiDetah28 = dr.GetDecimal(iTRANRETIDETAH28);

                    int iTRANRETIDETAH29 = dr.GetOrdinal(helper.TRANRETIDETAH29);
                    if (!dr.IsDBNull(iTRANRETIDETAH29)) entity.TranRetiDetah29 = dr.GetDecimal(iTRANRETIDETAH29);

                    int iTRANRETIDETAH30 = dr.GetOrdinal(helper.TRANRETIDETAH30);
                    if (!dr.IsDBNull(iTRANRETIDETAH30)) entity.TranRetiDetah30 = dr.GetDecimal(iTRANRETIDETAH30);

                    int iTRANRETIDETAH31 = dr.GetOrdinal(helper.TRANRETIDETAH31);
                    if (!dr.IsDBNull(iTRANRETIDETAH31)) entity.TranRetiDetah31 = dr.GetDecimal(iTRANRETIDETAH31);

                    int iTRANRETIDETAH32 = dr.GetOrdinal(helper.TRANRETIDETAH32);
                    if (!dr.IsDBNull(iTRANRETIDETAH32)) entity.TranRetiDetah32 = dr.GetDecimal(iTRANRETIDETAH32);

                    int iTRANRETIDETAH33 = dr.GetOrdinal(helper.TRANRETIDETAH33);
                    if (!dr.IsDBNull(iTRANRETIDETAH33)) entity.TranRetiDetah33 = dr.GetDecimal(iTRANRETIDETAH33);

                    int iTRANRETIDETAH34 = dr.GetOrdinal(helper.TRANRETIDETAH34);
                    if (!dr.IsDBNull(iTRANRETIDETAH34)) entity.TranRetiDetah34 = dr.GetDecimal(iTRANRETIDETAH34);

                    int iTRANRETIDETAH35 = dr.GetOrdinal(helper.TRANRETIDETAH35);
                    if (!dr.IsDBNull(iTRANRETIDETAH35)) entity.TranRetiDetah35 = dr.GetDecimal(iTRANRETIDETAH35);

                    int iTRANRETIDETAH36 = dr.GetOrdinal(helper.TRANRETIDETAH36);
                    if (!dr.IsDBNull(iTRANRETIDETAH36)) entity.TranRetiDetah36 = dr.GetDecimal(iTRANRETIDETAH36);

                    int iTRANRETIDETAH37 = dr.GetOrdinal(helper.TRANRETIDETAH37);
                    if (!dr.IsDBNull(iTRANRETIDETAH37)) entity.TranRetiDetah37 = dr.GetDecimal(iTRANRETIDETAH37);

                    int iTRANRETIDETAH38 = dr.GetOrdinal(helper.TRANRETIDETAH38);
                    if (!dr.IsDBNull(iTRANRETIDETAH38)) entity.TranRetiDetah38 = dr.GetDecimal(iTRANRETIDETAH38);

                    int iTRANRETIDETAH39 = dr.GetOrdinal(helper.TRANRETIDETAH39);
                    if (!dr.IsDBNull(iTRANRETIDETAH39)) entity.TranRetiDetah39 = dr.GetDecimal(iTRANRETIDETAH39);

                    int iTRANRETIDETAH40 = dr.GetOrdinal(helper.TRANRETIDETAH40);
                    if (!dr.IsDBNull(iTRANRETIDETAH40)) entity.TranRetiDetah40 = dr.GetDecimal(iTRANRETIDETAH40);

                    int iTRANRETIDETAH41 = dr.GetOrdinal(helper.TRANRETIDETAH41);
                    if (!dr.IsDBNull(iTRANRETIDETAH41)) entity.TranRetiDetah41 = dr.GetDecimal(iTRANRETIDETAH41);

                    int iTRANRETIDETAH42 = dr.GetOrdinal(helper.TRANRETIDETAH42);
                    if (!dr.IsDBNull(iTRANRETIDETAH42)) entity.TranRetiDetah42 = dr.GetDecimal(iTRANRETIDETAH42);

                    int iTRANRETIDETAH43 = dr.GetOrdinal(helper.TRANRETIDETAH43);
                    if (!dr.IsDBNull(iTRANRETIDETAH43)) entity.TranRetiDetah43 = dr.GetDecimal(iTRANRETIDETAH43);

                    int iTRANRETIDETAH44 = dr.GetOrdinal(helper.TRANRETIDETAH44);
                    if (!dr.IsDBNull(iTRANRETIDETAH44)) entity.TranRetiDetah44 = dr.GetDecimal(iTRANRETIDETAH44);

                    int iTRANRETIDETAH45 = dr.GetOrdinal(helper.TRANRETIDETAH45);
                    if (!dr.IsDBNull(iTRANRETIDETAH45)) entity.TranRetiDetah45 = dr.GetDecimal(iTRANRETIDETAH45);

                    int iTRANRETIDETAH46 = dr.GetOrdinal(helper.TRANRETIDETAH46);
                    if (!dr.IsDBNull(iTRANRETIDETAH46)) entity.TranRetiDetah46 = dr.GetDecimal(iTRANRETIDETAH46);

                    int iTRANRETIDETAH47 = dr.GetOrdinal(helper.TRANRETIDETAH47);
                    if (!dr.IsDBNull(iTRANRETIDETAH47)) entity.TranRetiDetah47 = dr.GetDecimal(iTRANRETIDETAH47);

                    int iTRANRETIDETAH48 = dr.GetOrdinal(helper.TRANRETIDETAH48);
                    if (!dr.IsDBNull(iTRANRETIDETAH48)) entity.TranRetiDetah48 = dr.GetDecimal(iTRANRETIDETAH48);

                    int iTRANRETIDETAH49 = dr.GetOrdinal(helper.TRANRETIDETAH49);
                    if (!dr.IsDBNull(iTRANRETIDETAH49)) entity.TranRetiDetah49 = dr.GetDecimal(iTRANRETIDETAH49);

                    int iTRANRETIDETAH50 = dr.GetOrdinal(helper.TRANRETIDETAH50);
                    if (!dr.IsDBNull(iTRANRETIDETAH50)) entity.TranRetiDetah50 = dr.GetDecimal(iTRANRETIDETAH50);

                    int iTRANRETIDETAH51 = dr.GetOrdinal(helper.TRANRETIDETAH51);
                    if (!dr.IsDBNull(iTRANRETIDETAH50)) entity.TranRetiDetah51 = dr.GetDecimal(iTRANRETIDETAH51);

                    int iTRANRETIDETAH52 = dr.GetOrdinal(helper.TRANRETIDETAH52);
                    if (!dr.IsDBNull(iTRANRETIDETAH52)) entity.TranRetiDetah52 = dr.GetDecimal(iTRANRETIDETAH52);

                    int iTRANRETIDETAH53 = dr.GetOrdinal(helper.TRANRETIDETAH53);
                    if (!dr.IsDBNull(iTRANRETIDETAH53)) entity.TranRetiDetah53 = dr.GetDecimal(iTRANRETIDETAH53);

                    int iTRANRETIDETAH54 = dr.GetOrdinal(helper.TRANRETIDETAH54);
                    if (!dr.IsDBNull(iTRANRETIDETAH54)) entity.TranRetiDetah54 = dr.GetDecimal(iTRANRETIDETAH54);

                    int iTRANRETIDETAH55 = dr.GetOrdinal(helper.TRANRETIDETAH55);
                    if (!dr.IsDBNull(iTRANRETIDETAH55)) entity.TranRetiDetah55 = dr.GetDecimal(iTRANRETIDETAH55);

                    int iTRANRETIDETAH56 = dr.GetOrdinal(helper.TRANRETIDETAH56);
                    if (!dr.IsDBNull(iTRANRETIDETAH56)) entity.TranRetiDetah56 = dr.GetDecimal(iTRANRETIDETAH56);

                    int iTRANRETIDETAH57 = dr.GetOrdinal(helper.TRANRETIDETAH57);
                    if (!dr.IsDBNull(iTRANRETIDETAH57)) entity.TranRetiDetah57 = dr.GetDecimal(iTRANRETIDETAH57);

                    int iTRANRETIDETAH58 = dr.GetOrdinal(helper.TRANRETIDETAH58);
                    if (!dr.IsDBNull(iTRANRETIDETAH58)) entity.TranRetiDetah58 = dr.GetDecimal(iTRANRETIDETAH58);

                    int iTRANRETIDETAH59 = dr.GetOrdinal(helper.TRANRETIDETAH59);
                    if (!dr.IsDBNull(iTRANRETIDETAH59)) entity.TranRetiDetah59 = dr.GetDecimal(iTRANRETIDETAH59);

                    int iTRANRETIDETAH60 = dr.GetOrdinal(helper.TRANRETIDETAH60);
                    if (!dr.IsDBNull(iTRANRETIDETAH60)) entity.TranRetiDetah60 = dr.GetDecimal(iTRANRETIDETAH60);

                    int iTRANRETIDETAH61 = dr.GetOrdinal(helper.TRANRETIDETAH61);
                    if (!dr.IsDBNull(iTRANRETIDETAH61)) entity.TranRetiDetah61 = dr.GetDecimal(iTRANRETIDETAH61);

                    int iTRANRETIDETAH62 = dr.GetOrdinal(helper.TRANRETIDETAH62);
                    if (!dr.IsDBNull(iTRANRETIDETAH62)) entity.TranRetiDetah62 = dr.GetDecimal(iTRANRETIDETAH62);

                    int iTRANRETIDETAH63 = dr.GetOrdinal(helper.TRANRETIDETAH63);
                    if (!dr.IsDBNull(iTRANRETIDETAH63)) entity.TranRetiDetah63 = dr.GetDecimal(iTRANRETIDETAH63);

                    int iTRANRETIDETAH64 = dr.GetOrdinal(helper.TRANRETIDETAH64);
                    if (!dr.IsDBNull(iTRANRETIDETAH64)) entity.TranRetiDetah64 = dr.GetDecimal(iTRANRETIDETAH64);

                    int iTRANRETIDETAH65 = dr.GetOrdinal(helper.TRANRETIDETAH65);
                    if (!dr.IsDBNull(iTRANRETIDETAH65)) entity.TranRetiDetah65 = dr.GetDecimal(iTRANRETIDETAH65);

                    int iTRANRETIDETAH66 = dr.GetOrdinal(helper.TRANRETIDETAH66);
                    if (!dr.IsDBNull(iTRANRETIDETAH66)) entity.TranRetiDetah66 = dr.GetDecimal(iTRANRETIDETAH66);

                    int iTRANRETIDETAH67 = dr.GetOrdinal(helper.TRANRETIDETAH67);
                    if (!dr.IsDBNull(iTRANRETIDETAH67)) entity.TranRetiDetah67 = dr.GetDecimal(iTRANRETIDETAH67);

                    int iTRANRETIDETAH68 = dr.GetOrdinal(helper.TRANRETIDETAH68);
                    if (!dr.IsDBNull(iTRANRETIDETAH68)) entity.TranRetiDetah68 = dr.GetDecimal(iTRANRETIDETAH68);

                    int iTRANRETIDETAH69 = dr.GetOrdinal(helper.TRANRETIDETAH69);
                    if (!dr.IsDBNull(iTRANRETIDETAH69)) entity.TranRetiDetah69 = dr.GetDecimal(iTRANRETIDETAH69);

                    int iTRANRETIDETAH70 = dr.GetOrdinal(helper.TRANRETIDETAH70);
                    if (!dr.IsDBNull(iTRANRETIDETAH70)) entity.TranRetiDetah70 = dr.GetDecimal(iTRANRETIDETAH70);

                    int iTRANRETIDETAH71 = dr.GetOrdinal(helper.TRANRETIDETAH71);
                    if (!dr.IsDBNull(iTRANRETIDETAH71)) entity.TranRetiDetah71 = dr.GetDecimal(iTRANRETIDETAH71);

                    int iTRANRETIDETAH72 = dr.GetOrdinal(helper.TRANRETIDETAH72);
                    if (!dr.IsDBNull(iTRANRETIDETAH72)) entity.TranRetiDetah72 = dr.GetDecimal(iTRANRETIDETAH72);

                    int iTRANRETIDETAH73 = dr.GetOrdinal(helper.TRANRETIDETAH73);
                    if (!dr.IsDBNull(iTRANRETIDETAH73)) entity.TranRetiDetah73 = dr.GetDecimal(iTRANRETIDETAH73);

                    int iTRANRETIDETAH74 = dr.GetOrdinal(helper.TRANRETIDETAH74);
                    if (!dr.IsDBNull(iTRANRETIDETAH74)) entity.TranRetiDetah74 = dr.GetDecimal(iTRANRETIDETAH74);

                    int iTRANRETIDETAH75 = dr.GetOrdinal(helper.TRANRETIDETAH75);
                    if (!dr.IsDBNull(iTRANRETIDETAH75)) entity.TranRetiDetah75 = dr.GetDecimal(iTRANRETIDETAH75);

                    int iTRANRETIDETAH76 = dr.GetOrdinal(helper.TRANRETIDETAH76);
                    if (!dr.IsDBNull(iTRANRETIDETAH76)) entity.TranRetiDetah76 = dr.GetDecimal(iTRANRETIDETAH76);

                    int iTRANRETIDETAH77 = dr.GetOrdinal(helper.TRANRETIDETAH77);
                    if (!dr.IsDBNull(iTRANRETIDETAH77)) entity.TranRetiDetah77 = dr.GetDecimal(iTRANRETIDETAH77);

                    int iTRANRETIDETAH78 = dr.GetOrdinal(helper.TRANRETIDETAH78);
                    if (!dr.IsDBNull(iTRANRETIDETAH78)) entity.TranRetiDetah78 = dr.GetDecimal(iTRANRETIDETAH78);

                    int iTRANRETIDETAH79 = dr.GetOrdinal(helper.TRANRETIDETAH79);
                    if (!dr.IsDBNull(iTRANRETIDETAH79)) entity.TranRetiDetah79 = dr.GetDecimal(iTRANRETIDETAH79);

                    int iTRANRETIDETAH80 = dr.GetOrdinal(helper.TRANRETIDETAH80);
                    if (!dr.IsDBNull(iTRANRETIDETAH80)) entity.TranRetiDetah80 = dr.GetDecimal(iTRANRETIDETAH80);

                    int iTRANRETIDETAH81 = dr.GetOrdinal(helper.TRANRETIDETAH81);
                    if (!dr.IsDBNull(iTRANRETIDETAH81)) entity.TranRetiDetah81 = dr.GetDecimal(iTRANRETIDETAH81);

                    int iTRANRETIDETAH82 = dr.GetOrdinal(helper.TRANRETIDETAH82);
                    if (!dr.IsDBNull(iTRANRETIDETAH82)) entity.TranRetiDetah82 = dr.GetDecimal(iTRANRETIDETAH82);

                    int iTRANRETIDETAH83 = dr.GetOrdinal(helper.TRANRETIDETAH83);
                    if (!dr.IsDBNull(iTRANRETIDETAH83)) entity.TranRetiDetah83 = dr.GetDecimal(iTRANRETIDETAH83);

                    int iTRANRETIDETAH84 = dr.GetOrdinal(helper.TRANRETIDETAH84);
                    if (!dr.IsDBNull(iTRANRETIDETAH84)) entity.TranRetiDetah84 = dr.GetDecimal(iTRANRETIDETAH84);

                    int iTRANRETIDETAH85 = dr.GetOrdinal(helper.TRANRETIDETAH85);
                    if (!dr.IsDBNull(iTRANRETIDETAH85)) entity.TranRetiDetah85 = dr.GetDecimal(iTRANRETIDETAH85);

                    int iTRANRETIDETAH86 = dr.GetOrdinal(helper.TRANRETIDETAH86);
                    if (!dr.IsDBNull(iTRANRETIDETAH86)) entity.TranRetiDetah86 = dr.GetDecimal(iTRANRETIDETAH86);

                    int iTRANRETIDETAH87 = dr.GetOrdinal(helper.TRANRETIDETAH87);
                    if (!dr.IsDBNull(iTRANRETIDETAH87)) entity.TranRetiDetah87 = dr.GetDecimal(iTRANRETIDETAH87);

                    int iTRANRETIDETAH88 = dr.GetOrdinal(helper.TRANRETIDETAH88);
                    if (!dr.IsDBNull(iTRANRETIDETAH88)) entity.TranRetiDetah88 = dr.GetDecimal(iTRANRETIDETAH88);

                    int iTRANRETIDETAH89 = dr.GetOrdinal(helper.TRANRETIDETAH89);
                    if (!dr.IsDBNull(iTRANRETIDETAH89)) entity.TranRetiDetah89 = dr.GetDecimal(iTRANRETIDETAH89);

                    int iTRANRETIDETAH90 = dr.GetOrdinal(helper.TRANRETIDETAH90);
                    if (!dr.IsDBNull(iTRANRETIDETAH90)) entity.TranRetiDetah90 = dr.GetDecimal(iTRANRETIDETAH90);

                    int iTRANRETIDETAH91 = dr.GetOrdinal(helper.TRANRETIDETAH91);
                    if (!dr.IsDBNull(iTRANRETIDETAH91)) entity.TranRetiDetah91 = dr.GetDecimal(iTRANRETIDETAH91);

                    int iTRANRETIDETAH92 = dr.GetOrdinal(helper.TRANRETIDETAH92);
                    if (!dr.IsDBNull(iTRANRETIDETAH92)) entity.TranRetiDetah92 = dr.GetDecimal(iTRANRETIDETAH92);

                    int iTRANRETIDETAH93 = dr.GetOrdinal(helper.TRANRETIDETAH93);
                    if (!dr.IsDBNull(iTRANRETIDETAH93)) entity.TranRetiDetah93 = dr.GetDecimal(iTRANRETIDETAH93);

                    int iTRANRETIDETAH94 = dr.GetOrdinal(helper.TRANRETIDETAH94);
                    if (!dr.IsDBNull(iTRANRETIDETAH94)) entity.TranRetiDetah94 = dr.GetDecimal(iTRANRETIDETAH94);

                    int iTRANRETIDETAH95 = dr.GetOrdinal(helper.TRANRETIDETAH95);
                    if (!dr.IsDBNull(iTRANRETIDETAH95)) entity.TranRetiDetah95 = dr.GetDecimal(iTRANRETIDETAH95);

                    int iTRANRETIDETAH96 = dr.GetOrdinal(helper.TRANRETIDETAH96);
                    if (!dr.IsDBNull(iTRANRETIDETAH96)) entity.TranRetiDetah96 = dr.GetDecimal(iTRANRETIDETAH96);

                }
                return entity;
            }

        }
    }
}
