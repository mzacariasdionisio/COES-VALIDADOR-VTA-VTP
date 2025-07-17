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
    public class TransferenciaEntregaDetalleRepository : RepositoryBase, ITransferenciaEntregaDetalleRepository
    {
        public TransferenciaEntregaDetalleRepository(string strConn) : base(strConn)
        {
        }

        TransferenciaEntregaDetalleHelper helper = new TransferenciaEntregaDetalleHelper();

        public int Save(TransferenciaEntregaDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.TRANENTRCODI, DbType.Int32, entity.TranEntrCodi);
            dbProvider.AddInParameter(command, helper.TRANENTRDETACODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.TRANENTRDETAVERSION, DbType.Int32, entity.TranEntrDetaVersion);
            dbProvider.AddInParameter(command, helper.TRANENTRDETADIA, DbType.Int32, entity.TranEntrDetaDia);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAPROMDIA, DbType.Decimal, entity.TranEntrDetaPromDia);
            dbProvider.AddInParameter(command, helper.TRANENTRDETASUMADIA, DbType.Decimal, entity.TranEntrDetaSumaDia);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH1, DbType.Double, entity.TranEntrDetah1);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH2, DbType.Double, entity.TranEntrDetah2);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH3, DbType.Double, entity.TranEntrDetah3);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH4, DbType.Double, entity.TranEntrDetah4);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH5, DbType.Double, entity.TranEntrDetah5);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH6, DbType.Double, entity.TranEntrDetah6);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH7, DbType.Double, entity.TranEntrDetah7);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH8, DbType.Double, entity.TranEntrDetah8);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH9, DbType.Double, entity.TranEntrDetah9);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH10, DbType.Double, entity.TranEntrDetah10);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH11, DbType.Double, entity.TranEntrDetah11);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH12, DbType.Double, entity.TranEntrDetah12);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH13, DbType.Double, entity.TranEntrDetah13);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH14, DbType.Double, entity.TranEntrDetah14);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH15, DbType.Double, entity.TranEntrDetah15);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH16, DbType.Double, entity.TranEntrDetah16);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH17, DbType.Double, entity.TranEntrDetah17);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH18, DbType.Double, entity.TranEntrDetah18);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH19, DbType.Double, entity.TranEntrDetah19);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH20, DbType.Double, entity.TranEntrDetah20);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH21, DbType.Double, entity.TranEntrDetah21);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH22, DbType.Double, entity.TranEntrDetah22);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH23, DbType.Double, entity.TranEntrDetah23);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH24, DbType.Double, entity.TranEntrDetah24);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH25, DbType.Double, entity.TranEntrDetah25);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH26, DbType.Double, entity.TranEntrDetah26);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH27, DbType.Double, entity.TranEntrDetah27);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH28, DbType.Double, entity.TranEntrDetah28);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH29, DbType.Double, entity.TranEntrDetah29);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH30, DbType.Double, entity.TranEntrDetah30);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH31, DbType.Double, entity.TranEntrDetah31);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH32, DbType.Double, entity.TranEntrDetah32);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH33, DbType.Double, entity.TranEntrDetah33);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH34, DbType.Double, entity.TranEntrDetah34);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH35, DbType.Double, entity.TranEntrDetah35);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH36, DbType.Double, entity.TranEntrDetah36);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH37, DbType.Double, entity.TranEntrDetah37);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH38, DbType.Double, entity.TranEntrDetah38);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH39, DbType.Double, entity.TranEntrDetah39);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH40, DbType.Double, entity.TranEntrDetah40);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH41, DbType.Double, entity.TranEntrDetah41);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH42, DbType.Double, entity.TranEntrDetah42);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH43, DbType.Double, entity.TranEntrDetah43);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH44, DbType.Double, entity.TranEntrDetah44);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH45, DbType.Double, entity.TranEntrDetah45);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH46, DbType.Double, entity.TranEntrDetah46);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH47, DbType.Double, entity.TranEntrDetah47);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH48, DbType.Double, entity.TranEntrDetah48);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH49, DbType.Double, entity.TranEntrDetah49);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH50, DbType.Double, entity.TranEntrDetah50);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH51, DbType.Double, entity.TranEntrDetah51);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH52, DbType.Double, entity.TranEntrDetah52);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH53, DbType.Double, entity.TranEntrDetah53);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH54, DbType.Double, entity.TranEntrDetah54);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH55, DbType.Double, entity.TranEntrDetah55);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH56, DbType.Double, entity.TranEntrDetah56);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH57, DbType.Double, entity.TranEntrDetah57);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH58, DbType.Double, entity.TranEntrDetah58);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH59, DbType.Double, entity.TranEntrDetah59);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH60, DbType.Double, entity.TranEntrDetah60);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH61, DbType.Double, entity.TranEntrDetah61);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH62, DbType.Double, entity.TranEntrDetah62);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH63, DbType.Double, entity.TranEntrDetah63);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH64, DbType.Double, entity.TranEntrDetah64);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH65, DbType.Double, entity.TranEntrDetah65);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH66, DbType.Double, entity.TranEntrDetah66);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH67, DbType.Double, entity.TranEntrDetah67);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH68, DbType.Double, entity.TranEntrDetah68);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH69, DbType.Double, entity.TranEntrDetah69);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH70, DbType.Double, entity.TranEntrDetah70);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH71, DbType.Double, entity.TranEntrDetah71);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH72, DbType.Double, entity.TranEntrDetah72);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH73, DbType.Double, entity.TranEntrDetah73);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH74, DbType.Double, entity.TranEntrDetah74);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH75, DbType.Double, entity.TranEntrDetah75);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH76, DbType.Double, entity.TranEntrDetah76);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH77, DbType.Double, entity.TranEntrDetah77);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH78, DbType.Double, entity.TranEntrDetah78);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH79, DbType.Double, entity.TranEntrDetah79);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH80, DbType.Double, entity.TranEntrDetah80);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH81, DbType.Double, entity.TranEntrDetah81);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH82, DbType.Double, entity.TranEntrDetah82);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH83, DbType.Double, entity.TranEntrDetah83);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH84, DbType.Double, entity.TranEntrDetah84);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH85, DbType.Double, entity.TranEntrDetah85);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH86, DbType.Double, entity.TranEntrDetah86);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH87, DbType.Double, entity.TranEntrDetah87);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH88, DbType.Double, entity.TranEntrDetah88);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH89, DbType.Double, entity.TranEntrDetah89);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH90, DbType.Double, entity.TranEntrDetah90);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH91, DbType.Double, entity.TranEntrDetah91);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH92, DbType.Double, entity.TranEntrDetah92);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH93, DbType.Double, entity.TranEntrDetah93);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH94, DbType.Double, entity.TranEntrDetah94);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH95, DbType.Double, entity.TranEntrDetah95);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAH96, DbType.Double, entity.TranEntrDetah96);
            dbProvider.AddInParameter(command, helper.TENTDEUSERNAME, DbType.String, entity.TentdeUserName);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAFECINS, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TransferenciaEntregaDetalleDTO entity)
        {

        }

        public void Delete(int pericodi, int version, string sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, sCodigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public TransferenciaEntregaDetalleDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.TRANENTRDETACODI, DbType.Int32, id);
            TransferenciaEntregaDetalleDTO entity = null;

            return entity;
        }

        public List<TransferenciaEntregaDetalleDTO> List(int emprcodi, int pericodi)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();

                    int iTENTCODIGO = dr.GetOrdinal(helper.TENTCODIGO);
                    if (!dr.IsDBNull(iTENTCODIGO)) entity.TentCodigo = dr.GetString(iTENTCODIGO);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TransferenciaEntregaDetalleDTO> GetByCriteria(int emprcodi, int pericodi, string codientrcodi, int version)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, codientrcodi);
            dbProvider.AddInParameter(command, helper.CODIENTRCODIGO, DbType.String, codientrcodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);

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

        public List<TransferenciaEntregaDetalleDTO> GetByPeriodoVersion(int pericodi, int version)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByPeriodoVersion);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();

                    int iEMPRCODI = dr.GetOrdinal(this.helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    int iBARRCODI = dr.GetOrdinal(this.helper.BARRCODI);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iCODIENTRCODIGO = dr.GetOrdinal(this.helper.CODIENTRCODIGO);
                    if (!dr.IsDBNull(iCODIENTRCODIGO)) entity.CodiEntrCodigo = dr.GetString(iCODIENTRCODIGO);

                    int iCENTGENECODI = dr.GetOrdinal(this.helper.CENTGENECODI);
                    if (!dr.IsDBNull(iCENTGENECODI)) entity.CentGeneCodi = dr.GetInt32(iCENTGENECODI);

                    int iPERICODI = dr.GetOrdinal(this.helper.PERICODI);
                    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

                    int iTRANENTRVERSION = dr.GetOrdinal(this.helper.TRANENTRVERSION);
                    if (!dr.IsDBNull(iTRANENTRVERSION)) entity.TranentrVersion = dr.GetInt32(iTRANENTRVERSION);

                    int iTIPOINFORMACION = dr.GetOrdinal(this.helper.TRANENTRTIPOINFORMACION);
                    if (!dr.IsDBNull(iTIPOINFORMACION)) entity.TranEntrTipoInformacion = dr.GetString(iTIPOINFORMACION);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<TransferenciaEntregaDetalleDTO> ListaTransferenciaEntrPorPericodiYVersion(int pericodi, int version)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            var query = string.Format(helper.SqlListTransEntrPorPericodiYVersion, pericodi, version);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    var entity = helper.Create(dr);

                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    int iNOMBEMPRESA = dr.GetOrdinal(helper.NOMBEMPRESA);
                    if (!dr.IsDBNull(iNOMBEMPRESA)) entity.NombEmpresa = dr.GetString(iNOMBEMPRESA);

                    int iBARRCODI = dr.GetOrdinal(helper.BARRCODI);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iBARRNOMBRE = dr.GetOrdinal(helper.BARRNOMBRE);
                    if (!dr.IsDBNull(iBARRNOMBRE)) entity.NombBarra = dr.GetString(iBARRNOMBRE);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<TransferenciaEntregaDetalleDTO> BalanceEnergiaActiva(int pericodi, int version, Int32? barrcodi, Int32? emprcodi)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBalanceEnergiaActiva);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();

                    int iBARRCODI = dr.GetOrdinal(this.helper.BARRCODI);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iNOMBBARRA = dr.GetOrdinal(this.helper.NOMBBARRA);
                    if (!dr.IsDBNull(iNOMBBARRA)) entity.NombBarra = dr.GetString(iNOMBBARRA);

                    int iTENTCODIGO = dr.GetOrdinal(this.helper.TENTCODIGO);
                    if (!dr.IsDBNull(iTENTCODIGO)) entity.TentCodigo = dr.GetString(iTENTCODIGO);

                    int iTRANENTRTIPOINFORMACION = dr.GetOrdinal(this.helper.TRANENTRTIPOINFORMACION);
                    if (!dr.IsDBNull(iTRANENTRTIPOINFORMACION)) entity.TranEntrTipoInformacion = dr.GetString(iTRANENTRTIPOINFORMACION);

                    int iTENTDEUSERNAME = dr.GetOrdinal(this.helper.TENTDEUSERNAME);
                    if (!dr.IsDBNull(iTENTDEUSERNAME)) entity.TentdeUserName = dr.GetString(iTENTDEUSERNAME);

                    int iENERGIA = dr.GetOrdinal(this.helper.ENERGIA);
                    if (!dr.IsDBNull(iENERGIA)) entity.Energia = dr.GetDecimal(iENERGIA);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TransferenciaEntregaDetalleDTO> ListByTransferenciaEntrega(int iTEntCodi, int iTRetVersion)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByTransferenciaEntrega);

            dbProvider.AddInParameter(command, helper.TRANENTRCODI, DbType.Int32, iTEntCodi);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAVERSION, DbType.Int32, iTRetVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        public void DeleteListaTransferenciaEntregaDetalle(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaTransferenciaEntregaDetalle);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRDETAVERSION, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<TransferenciaEntregaDetalleDTO> ListTransEntrReti(int iEmprcodi, int iBarrcodi, int iPericodi, int iVersion, string iFlagtipo)
        {
            List<TransferenciaEntregaDetalleDTO> entitys = new List<TransferenciaEntregaDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTransEntrTransReti);

            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprcodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrcodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprcodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrcodi);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, iVersion);
            dbProvider.AddInParameter(command, helper.FLAGTIPO, DbType.String, iFlagtipo);
            dbProvider.AddInParameter(command, helper.FLAGTIPO, DbType.String, iFlagtipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();

                    int iTRANENTRCODI = dr.GetOrdinal(helper.TRANENTRCODI);
                    if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranEntrCodi = dr.GetInt32(iTRANENTRCODI);

                    int iTRANENTRDETACODI = dr.GetOrdinal(helper.TRANENTRDETACODI);
                    if (!dr.IsDBNull(iTRANENTRDETACODI)) entity.TranEntrDetaCodi = dr.GetDecimal(iTRANENTRDETACODI);

                    int iTRANENTRDETAVERSION = dr.GetOrdinal(helper.TRANENTRDETAVERSION);
                    if (!dr.IsDBNull(iTRANENTRDETAVERSION)) entity.TranEntrDetaVersion = dr.GetInt32(iTRANENTRDETAVERSION);

                    int iTRANENTRDETADIA = dr.GetOrdinal(helper.TRANENTRDETADIA);
                    if (!dr.IsDBNull(iTRANENTRDETADIA)) entity.TranEntrDetaDia = dr.GetInt32(iTRANENTRDETADIA);

                    int iTRANENTRDETAPROMDIA = dr.GetOrdinal(helper.TRANENTRDETAPROMDIA);
                    if (!dr.IsDBNull(iTRANENTRDETAPROMDIA)) entity.TranEntrDetaPromDia = dr.GetDecimal(iTRANENTRDETAPROMDIA);

                    int iTRANENTRDETASUMADIA = dr.GetOrdinal(helper.TRANENTRDETASUMADIA);
                    if (!dr.IsDBNull(iTRANENTRDETASUMADIA)) entity.TranEntrDetaSumaDia = dr.GetDecimal(iTRANENTRDETASUMADIA);

                    int iTRANENTRDETAH1 = dr.GetOrdinal(helper.TRANENTRDETAH1);
                    if (!dr.IsDBNull(iTRANENTRDETAH1)) entity.TranEntrDetah1 = dr.GetDecimal(iTRANENTRDETAH1);

                    int iTRANENTRDETAH2 = dr.GetOrdinal(helper.TRANENTRDETAH2);
                    if (!dr.IsDBNull(iTRANENTRDETAH2)) entity.TranEntrDetah2 = dr.GetDecimal(iTRANENTRDETAH2);

                    int iTRANENTRDETAH3 = dr.GetOrdinal(helper.TRANENTRDETAH3);
                    if (!dr.IsDBNull(iTRANENTRDETAH3)) entity.TranEntrDetah3 = dr.GetDecimal(iTRANENTRDETAH3);

                    int iTRANENTRDETAH4 = dr.GetOrdinal(helper.TRANENTRDETAH4);
                    if (!dr.IsDBNull(iTRANENTRDETAH4)) entity.TranEntrDetah4 = dr.GetDecimal(iTRANENTRDETAH4);

                    int iTRANENTRDETAH5 = dr.GetOrdinal(helper.TRANENTRDETAH5);
                    if (!dr.IsDBNull(iTRANENTRDETAH5)) entity.TranEntrDetah5 = dr.GetDecimal(iTRANENTRDETAH5);

                    int iTRANENTRDETAH6 = dr.GetOrdinal(helper.TRANENTRDETAH6);
                    if (!dr.IsDBNull(iTRANENTRDETAH6)) entity.TranEntrDetah6 = dr.GetDecimal(iTRANENTRDETAH6);

                    int iTRANENTRDETAH7 = dr.GetOrdinal(helper.TRANENTRDETAH7);
                    if (!dr.IsDBNull(iTRANENTRDETAH7)) entity.TranEntrDetah7 = dr.GetDecimal(iTRANENTRDETAH7);

                    int iTRANENTRDETAH8 = dr.GetOrdinal(helper.TRANENTRDETAH8);
                    if (!dr.IsDBNull(iTRANENTRDETAH8)) entity.TranEntrDetah8 = dr.GetDecimal(iTRANENTRDETAH8);

                    int iTRANENTRDETAH9 = dr.GetOrdinal(helper.TRANENTRDETAH9);
                    if (!dr.IsDBNull(iTRANENTRDETAH9)) entity.TranEntrDetah9 = dr.GetDecimal(iTRANENTRDETAH9);

                    int iTRANENTRDETAH10 = dr.GetOrdinal(helper.TRANENTRDETAH10);
                    if (!dr.IsDBNull(iTRANENTRDETAH10)) entity.TranEntrDetah10 = dr.GetDecimal(iTRANENTRDETAH10);

                    int iTRANENTRDETAH11 = dr.GetOrdinal(helper.TRANENTRDETAH11);
                    if (!dr.IsDBNull(iTRANENTRDETAH11)) entity.TranEntrDetah11 = dr.GetDecimal(iTRANENTRDETAH11);

                    int iTRANENTRDETAH12 = dr.GetOrdinal(helper.TRANENTRDETAH12);
                    if (!dr.IsDBNull(iTRANENTRDETAH12)) entity.TranEntrDetah12 = dr.GetDecimal(iTRANENTRDETAH12);

                    int iTRANENTRDETAH13 = dr.GetOrdinal(helper.TRANENTRDETAH13);
                    if (!dr.IsDBNull(iTRANENTRDETAH13)) entity.TranEntrDetah13 = dr.GetDecimal(iTRANENTRDETAH13);

                    int iTRANENTRDETAH14 = dr.GetOrdinal(helper.TRANENTRDETAH14);
                    if (!dr.IsDBNull(iTRANENTRDETAH14)) entity.TranEntrDetah14 = dr.GetDecimal(iTRANENTRDETAH14);

                    int iTRANENTRDETAH15 = dr.GetOrdinal(helper.TRANENTRDETAH15);
                    if (!dr.IsDBNull(iTRANENTRDETAH15)) entity.TranEntrDetah15 = dr.GetDecimal(iTRANENTRDETAH15);

                    int iTRANENTRDETAH16 = dr.GetOrdinal(helper.TRANENTRDETAH16);
                    if (!dr.IsDBNull(iTRANENTRDETAH16)) entity.TranEntrDetah16 = dr.GetDecimal(iTRANENTRDETAH16);

                    int iTRANENTRDETAH17 = dr.GetOrdinal(helper.TRANENTRDETAH17);
                    if (!dr.IsDBNull(iTRANENTRDETAH17)) entity.TranEntrDetah17 = dr.GetDecimal(iTRANENTRDETAH17);

                    int iTRANENTRDETAH18 = dr.GetOrdinal(helper.TRANENTRDETAH18);
                    if (!dr.IsDBNull(iTRANENTRDETAH18)) entity.TranEntrDetah18 = dr.GetDecimal(iTRANENTRDETAH18);

                    int iTRANENTRDETAH19 = dr.GetOrdinal(helper.TRANENTRDETAH19);
                    if (!dr.IsDBNull(iTRANENTRDETAH19)) entity.TranEntrDetah19 = dr.GetDecimal(iTRANENTRDETAH19);

                    int iTRANENTRDETAH20 = dr.GetOrdinal(helper.TRANENTRDETAH20);
                    if (!dr.IsDBNull(iTRANENTRDETAH20)) entity.TranEntrDetah20 = dr.GetDecimal(iTRANENTRDETAH20);

                    int iTRANENTRDETAH21 = dr.GetOrdinal(helper.TRANENTRDETAH21);
                    if (!dr.IsDBNull(iTRANENTRDETAH21)) entity.TranEntrDetah21 = dr.GetDecimal(iTRANENTRDETAH21);

                    int iTRANENTRDETAH22 = dr.GetOrdinal(helper.TRANENTRDETAH22);
                    if (!dr.IsDBNull(iTRANENTRDETAH22)) entity.TranEntrDetah22 = dr.GetDecimal(iTRANENTRDETAH22);

                    int iTRANENTRDETAH23 = dr.GetOrdinal(helper.TRANENTRDETAH23);
                    if (!dr.IsDBNull(iTRANENTRDETAH23)) entity.TranEntrDetah23 = dr.GetDecimal(iTRANENTRDETAH23);

                    int iTRANENTRDETAH24 = dr.GetOrdinal(helper.TRANENTRDETAH24);
                    if (!dr.IsDBNull(iTRANENTRDETAH24)) entity.TranEntrDetah24 = dr.GetDecimal(iTRANENTRDETAH24);

                    int iTRANENTRDETAH25 = dr.GetOrdinal(helper.TRANENTRDETAH25);
                    if (!dr.IsDBNull(iTRANENTRDETAH25)) entity.TranEntrDetah25 = dr.GetDecimal(iTRANENTRDETAH25);

                    int iTRANENTRDETAH26 = dr.GetOrdinal(helper.TRANENTRDETAH26);
                    if (!dr.IsDBNull(iTRANENTRDETAH26)) entity.TranEntrDetah26 = dr.GetDecimal(iTRANENTRDETAH26);

                    int iTRANENTRDETAH27 = dr.GetOrdinal(helper.TRANENTRDETAH27);
                    if (!dr.IsDBNull(iTRANENTRDETAH27)) entity.TranEntrDetah27 = dr.GetDecimal(iTRANENTRDETAH27);

                    int iTRANENTRDETAH28 = dr.GetOrdinal(helper.TRANENTRDETAH28);
                    if (!dr.IsDBNull(iTRANENTRDETAH28)) entity.TranEntrDetah28 = dr.GetDecimal(iTRANENTRDETAH28);

                    int iTRANENTRDETAH29 = dr.GetOrdinal(helper.TRANENTRDETAH29);
                    if (!dr.IsDBNull(iTRANENTRDETAH29)) entity.TranEntrDetah29 = dr.GetDecimal(iTRANENTRDETAH29);

                    int iTRANENTRDETAH30 = dr.GetOrdinal(helper.TRANENTRDETAH30);
                    if (!dr.IsDBNull(iTRANENTRDETAH30)) entity.TranEntrDetah30 = dr.GetDecimal(iTRANENTRDETAH30);

                    int iTRANENTRDETAH31 = dr.GetOrdinal(helper.TRANENTRDETAH31);
                    if (!dr.IsDBNull(iTRANENTRDETAH31)) entity.TranEntrDetah31 = dr.GetDecimal(iTRANENTRDETAH31);

                    int iTRANENTRDETAH32 = dr.GetOrdinal(helper.TRANENTRDETAH32);
                    if (!dr.IsDBNull(iTRANENTRDETAH32)) entity.TranEntrDetah32 = dr.GetDecimal(iTRANENTRDETAH32);

                    int iTRANENTRDETAH33 = dr.GetOrdinal(helper.TRANENTRDETAH33);
                    if (!dr.IsDBNull(iTRANENTRDETAH33)) entity.TranEntrDetah33 = dr.GetDecimal(iTRANENTRDETAH33);

                    int iTRANENTRDETAH34 = dr.GetOrdinal(helper.TRANENTRDETAH34);
                    if (!dr.IsDBNull(iTRANENTRDETAH34)) entity.TranEntrDetah34 = dr.GetDecimal(iTRANENTRDETAH34);

                    int iTRANENTRDETAH35 = dr.GetOrdinal(helper.TRANENTRDETAH35);
                    if (!dr.IsDBNull(iTRANENTRDETAH35)) entity.TranEntrDetah35 = dr.GetDecimal(iTRANENTRDETAH35);

                    int iTRANENTRDETAH36 = dr.GetOrdinal(helper.TRANENTRDETAH36);
                    if (!dr.IsDBNull(iTRANENTRDETAH36)) entity.TranEntrDetah36 = dr.GetDecimal(iTRANENTRDETAH36);

                    int iTRANENTRDETAH37 = dr.GetOrdinal(helper.TRANENTRDETAH37);
                    if (!dr.IsDBNull(iTRANENTRDETAH37)) entity.TranEntrDetah37 = dr.GetDecimal(iTRANENTRDETAH37);

                    int iTRANENTRDETAH38 = dr.GetOrdinal(helper.TRANENTRDETAH38);
                    if (!dr.IsDBNull(iTRANENTRDETAH38)) entity.TranEntrDetah38 = dr.GetDecimal(iTRANENTRDETAH38);

                    int iTRANENTRDETAH39 = dr.GetOrdinal(helper.TRANENTRDETAH39);
                    if (!dr.IsDBNull(iTRANENTRDETAH39)) entity.TranEntrDetah39 = dr.GetDecimal(iTRANENTRDETAH39);

                    int iTRANENTRDETAH40 = dr.GetOrdinal(helper.TRANENTRDETAH40);
                    if (!dr.IsDBNull(iTRANENTRDETAH40)) entity.TranEntrDetah40 = dr.GetDecimal(iTRANENTRDETAH40);

                    int iTRANENTRDETAH41 = dr.GetOrdinal(helper.TRANENTRDETAH41);
                    if (!dr.IsDBNull(iTRANENTRDETAH41)) entity.TranEntrDetah41 = dr.GetDecimal(iTRANENTRDETAH41);

                    int iTRANENTRDETAH42 = dr.GetOrdinal(helper.TRANENTRDETAH42);
                    if (!dr.IsDBNull(iTRANENTRDETAH42)) entity.TranEntrDetah42 = dr.GetDecimal(iTRANENTRDETAH42);

                    int iTRANENTRDETAH43 = dr.GetOrdinal(helper.TRANENTRDETAH43);
                    if (!dr.IsDBNull(iTRANENTRDETAH43)) entity.TranEntrDetah43 = dr.GetDecimal(iTRANENTRDETAH43);

                    int iTRANENTRDETAH44 = dr.GetOrdinal(helper.TRANENTRDETAH44);
                    if (!dr.IsDBNull(iTRANENTRDETAH44)) entity.TranEntrDetah44 = dr.GetDecimal(iTRANENTRDETAH44);

                    int iTRANENTRDETAH45 = dr.GetOrdinal(helper.TRANENTRDETAH45);
                    if (!dr.IsDBNull(iTRANENTRDETAH45)) entity.TranEntrDetah45 = dr.GetDecimal(iTRANENTRDETAH45);

                    int iTRANENTRDETAH46 = dr.GetOrdinal(helper.TRANENTRDETAH46);
                    if (!dr.IsDBNull(iTRANENTRDETAH46)) entity.TranEntrDetah46 = dr.GetDecimal(iTRANENTRDETAH46);

                    int iTRANENTRDETAH47 = dr.GetOrdinal(helper.TRANENTRDETAH47);
                    if (!dr.IsDBNull(iTRANENTRDETAH47)) entity.TranEntrDetah47 = dr.GetDecimal(iTRANENTRDETAH47);

                    int iTRANENTRDETAH48 = dr.GetOrdinal(helper.TRANENTRDETAH48);
                    if (!dr.IsDBNull(iTRANENTRDETAH48)) entity.TranEntrDetah48 = dr.GetDecimal(iTRANENTRDETAH48);

                    int iTRANENTRDETAH49 = dr.GetOrdinal(helper.TRANENTRDETAH49);
                    if (!dr.IsDBNull(iTRANENTRDETAH49)) entity.TranEntrDetah49 = dr.GetDecimal(iTRANENTRDETAH49);

                    int iTRANENTRDETAH50 = dr.GetOrdinal(helper.TRANENTRDETAH50);
                    if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah50 = dr.GetDecimal(iTRANENTRDETAH50);

                    int iTRANENTRDETAH51 = dr.GetOrdinal(helper.TRANENTRDETAH51);
                    if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah51 = dr.GetDecimal(iTRANENTRDETAH51);

                    int iTRANENTRDETAH52 = dr.GetOrdinal(helper.TRANENTRDETAH52);
                    if (!dr.IsDBNull(iTRANENTRDETAH52)) entity.TranEntrDetah52 = dr.GetDecimal(iTRANENTRDETAH52);

                    int iTRANENTRDETAH53 = dr.GetOrdinal(helper.TRANENTRDETAH53);
                    if (!dr.IsDBNull(iTRANENTRDETAH53)) entity.TranEntrDetah53 = dr.GetDecimal(iTRANENTRDETAH53);

                    int iTRANENTRDETAH54 = dr.GetOrdinal(helper.TRANENTRDETAH54);
                    if (!dr.IsDBNull(iTRANENTRDETAH54)) entity.TranEntrDetah54 = dr.GetDecimal(iTRANENTRDETAH54);

                    int iTRANENTRDETAH55 = dr.GetOrdinal(helper.TRANENTRDETAH55);
                    if (!dr.IsDBNull(iTRANENTRDETAH55)) entity.TranEntrDetah55 = dr.GetDecimal(iTRANENTRDETAH55);

                    int iTRANENTRDETAH56 = dr.GetOrdinal(helper.TRANENTRDETAH56);
                    if (!dr.IsDBNull(iTRANENTRDETAH56)) entity.TranEntrDetah56 = dr.GetDecimal(iTRANENTRDETAH56);

                    int iTRANENTRDETAH57 = dr.GetOrdinal(helper.TRANENTRDETAH57);
                    if (!dr.IsDBNull(iTRANENTRDETAH57)) entity.TranEntrDetah57 = dr.GetDecimal(iTRANENTRDETAH57);

                    int iTRANENTRDETAH58 = dr.GetOrdinal(helper.TRANENTRDETAH58);
                    if (!dr.IsDBNull(iTRANENTRDETAH58)) entity.TranEntrDetah58 = dr.GetDecimal(iTRANENTRDETAH58);

                    int iTRANENTRDETAH59 = dr.GetOrdinal(helper.TRANENTRDETAH59);
                    if (!dr.IsDBNull(iTRANENTRDETAH59)) entity.TranEntrDetah59 = dr.GetDecimal(iTRANENTRDETAH59);

                    int iTRANENTRDETAH60 = dr.GetOrdinal(helper.TRANENTRDETAH60);
                    if (!dr.IsDBNull(iTRANENTRDETAH60)) entity.TranEntrDetah60 = dr.GetDecimal(iTRANENTRDETAH60);

                    int iTRANENTRDETAH61 = dr.GetOrdinal(helper.TRANENTRDETAH61);
                    if (!dr.IsDBNull(iTRANENTRDETAH61)) entity.TranEntrDetah61 = dr.GetDecimal(iTRANENTRDETAH61);

                    int iTRANENTRDETAH62 = dr.GetOrdinal(helper.TRANENTRDETAH62);
                    if (!dr.IsDBNull(iTRANENTRDETAH62)) entity.TranEntrDetah62 = dr.GetDecimal(iTRANENTRDETAH62);

                    int iTRANENTRDETAH63 = dr.GetOrdinal(helper.TRANENTRDETAH63);
                    if (!dr.IsDBNull(iTRANENTRDETAH63)) entity.TranEntrDetah63 = dr.GetDecimal(iTRANENTRDETAH63);

                    int iTRANENTRDETAH64 = dr.GetOrdinal(helper.TRANENTRDETAH64);
                    if (!dr.IsDBNull(iTRANENTRDETAH64)) entity.TranEntrDetah64 = dr.GetDecimal(iTRANENTRDETAH64);

                    int iTRANENTRDETAH65 = dr.GetOrdinal(helper.TRANENTRDETAH65);
                    if (!dr.IsDBNull(iTRANENTRDETAH65)) entity.TranEntrDetah65 = dr.GetDecimal(iTRANENTRDETAH65);

                    int iTRANENTRDETAH66 = dr.GetOrdinal(helper.TRANENTRDETAH66);
                    if (!dr.IsDBNull(iTRANENTRDETAH66)) entity.TranEntrDetah66 = dr.GetDecimal(iTRANENTRDETAH66);

                    int iTRANENTRDETAH67 = dr.GetOrdinal(helper.TRANENTRDETAH67);
                    if (!dr.IsDBNull(iTRANENTRDETAH67)) entity.TranEntrDetah67 = dr.GetDecimal(iTRANENTRDETAH67);

                    int iTRANENTRDETAH68 = dr.GetOrdinal(helper.TRANENTRDETAH68);
                    if (!dr.IsDBNull(iTRANENTRDETAH68)) entity.TranEntrDetah68 = dr.GetDecimal(iTRANENTRDETAH68);

                    int iTRANENTRDETAH69 = dr.GetOrdinal(helper.TRANENTRDETAH69);
                    if (!dr.IsDBNull(iTRANENTRDETAH69)) entity.TranEntrDetah69 = dr.GetDecimal(iTRANENTRDETAH69);

                    int iTRANENTRDETAH70 = dr.GetOrdinal(helper.TRANENTRDETAH70);
                    if (!dr.IsDBNull(iTRANENTRDETAH70)) entity.TranEntrDetah70 = dr.GetDecimal(iTRANENTRDETAH70);

                    int iTRANENTRDETAH71 = dr.GetOrdinal(helper.TRANENTRDETAH71);
                    if (!dr.IsDBNull(iTRANENTRDETAH71)) entity.TranEntrDetah71 = dr.GetDecimal(iTRANENTRDETAH71);

                    int iTRANENTRDETAH72 = dr.GetOrdinal(helper.TRANENTRDETAH72);
                    if (!dr.IsDBNull(iTRANENTRDETAH72)) entity.TranEntrDetah72 = dr.GetDecimal(iTRANENTRDETAH72);

                    int iTRANENTRDETAH73 = dr.GetOrdinal(helper.TRANENTRDETAH73);
                    if (!dr.IsDBNull(iTRANENTRDETAH73)) entity.TranEntrDetah73 = dr.GetDecimal(iTRANENTRDETAH73);

                    int iTRANENTRDETAH74 = dr.GetOrdinal(helper.TRANENTRDETAH74);
                    if (!dr.IsDBNull(iTRANENTRDETAH74)) entity.TranEntrDetah74 = dr.GetDecimal(iTRANENTRDETAH74);

                    int iTRANENTRDETAH75 = dr.GetOrdinal(helper.TRANENTRDETAH75);
                    if (!dr.IsDBNull(iTRANENTRDETAH75)) entity.TranEntrDetah75 = dr.GetDecimal(iTRANENTRDETAH75);

                    int iTRANENTRDETAH76 = dr.GetOrdinal(helper.TRANENTRDETAH76);
                    if (!dr.IsDBNull(iTRANENTRDETAH76)) entity.TranEntrDetah76 = dr.GetDecimal(iTRANENTRDETAH76);

                    int iTRANENTRDETAH77 = dr.GetOrdinal(helper.TRANENTRDETAH77);
                    if (!dr.IsDBNull(iTRANENTRDETAH77)) entity.TranEntrDetah77 = dr.GetDecimal(iTRANENTRDETAH77);

                    int iTRANENTRDETAH78 = dr.GetOrdinal(helper.TRANENTRDETAH78);
                    if (!dr.IsDBNull(iTRANENTRDETAH78)) entity.TranEntrDetah78 = dr.GetDecimal(iTRANENTRDETAH78);

                    int iTRANENTRDETAH79 = dr.GetOrdinal(helper.TRANENTRDETAH79);
                    if (!dr.IsDBNull(iTRANENTRDETAH79)) entity.TranEntrDetah79 = dr.GetDecimal(iTRANENTRDETAH79);

                    int iTRANENTRDETAH80 = dr.GetOrdinal(helper.TRANENTRDETAH80);
                    if (!dr.IsDBNull(iTRANENTRDETAH80)) entity.TranEntrDetah80 = dr.GetDecimal(iTRANENTRDETAH80);

                    int iTRANENTRDETAH81 = dr.GetOrdinal(helper.TRANENTRDETAH81);
                    if (!dr.IsDBNull(iTRANENTRDETAH81)) entity.TranEntrDetah81 = dr.GetDecimal(iTRANENTRDETAH81);

                    int iTRANENTRDETAH82 = dr.GetOrdinal(helper.TRANENTRDETAH82);
                    if (!dr.IsDBNull(iTRANENTRDETAH82)) entity.TranEntrDetah82 = dr.GetDecimal(iTRANENTRDETAH82);

                    int iTRANENTRDETAH83 = dr.GetOrdinal(helper.TRANENTRDETAH83);
                    if (!dr.IsDBNull(iTRANENTRDETAH83)) entity.TranEntrDetah83 = dr.GetDecimal(iTRANENTRDETAH83);

                    int iTRANENTRDETAH84 = dr.GetOrdinal(helper.TRANENTRDETAH84);
                    if (!dr.IsDBNull(iTRANENTRDETAH84)) entity.TranEntrDetah84 = dr.GetDecimal(iTRANENTRDETAH84);

                    int iTRANENTRDETAH85 = dr.GetOrdinal(helper.TRANENTRDETAH85);
                    if (!dr.IsDBNull(iTRANENTRDETAH85)) entity.TranEntrDetah85 = dr.GetDecimal(iTRANENTRDETAH85);

                    int iTRANENTRDETAH86 = dr.GetOrdinal(helper.TRANENTRDETAH86);
                    if (!dr.IsDBNull(iTRANENTRDETAH86)) entity.TranEntrDetah86 = dr.GetDecimal(iTRANENTRDETAH86);

                    int iTRANENTRDETAH87 = dr.GetOrdinal(helper.TRANENTRDETAH87);
                    if (!dr.IsDBNull(iTRANENTRDETAH87)) entity.TranEntrDetah87 = dr.GetDecimal(iTRANENTRDETAH87);

                    int iTRANENTRDETAH88 = dr.GetOrdinal(helper.TRANENTRDETAH88);
                    if (!dr.IsDBNull(iTRANENTRDETAH88)) entity.TranEntrDetah88 = dr.GetDecimal(iTRANENTRDETAH88);

                    int iTRANENTRDETAH89 = dr.GetOrdinal(helper.TRANENTRDETAH89);
                    if (!dr.IsDBNull(iTRANENTRDETAH89)) entity.TranEntrDetah89 = dr.GetDecimal(iTRANENTRDETAH89);

                    int iTRANENTRDETAH90 = dr.GetOrdinal(helper.TRANENTRDETAH90);
                    if (!dr.IsDBNull(iTRANENTRDETAH90)) entity.TranEntrDetah90 = dr.GetDecimal(iTRANENTRDETAH90);

                    int iTRANENTRDETAH91 = dr.GetOrdinal(helper.TRANENTRDETAH91);
                    if (!dr.IsDBNull(iTRANENTRDETAH91)) entity.TranEntrDetah91 = dr.GetDecimal(iTRANENTRDETAH91);

                    int iTRANENTRDETAH92 = dr.GetOrdinal(helper.TRANENTRDETAH92);
                    if (!dr.IsDBNull(iTRANENTRDETAH92)) entity.TranEntrDetah92 = dr.GetDecimal(iTRANENTRDETAH92);

                    int iTRANENTRDETAH93 = dr.GetOrdinal(helper.TRANENTRDETAH93);
                    if (!dr.IsDBNull(iTRANENTRDETAH93)) entity.TranEntrDetah93 = dr.GetDecimal(iTRANENTRDETAH93);

                    int iTRANENTRDETAH94 = dr.GetOrdinal(helper.TRANENTRDETAH94);
                    if (!dr.IsDBNull(iTRANENTRDETAH94)) entity.TranEntrDetah94 = dr.GetDecimal(iTRANENTRDETAH94);

                    int iTRANENTRDETAH95 = dr.GetOrdinal(helper.TRANENTRDETAH95);
                    if (!dr.IsDBNull(iTRANENTRDETAH95)) entity.TranEntrDetah95 = dr.GetDecimal(iTRANENTRDETAH95);

                    int iTRANENTRDETAH96 = dr.GetOrdinal(helper.TRANENTRDETAH96);
                    if (!dr.IsDBNull(iTRANENTRDETAH96)) entity.TranEntrDetah96 = dr.GetDecimal(iTRANENTRDETAH96);

                    int iTENTDEUSERNAME = dr.GetOrdinal(helper.TENTDEUSERNAME);
                    if (!dr.IsDBNull(iTENTDEUSERNAME)) entity.TentdeUserName = dr.GetString(iTENTDEUSERNAME);

                    int iTRANENTRDETAFECINS = dr.GetOrdinal(helper.TRANENTRDETAFECINS);
                    if (!dr.IsDBNull(iTRANENTRDETAFECINS)) entity.TranEntrDetaFecIns = dr.GetDateTime(iTRANENTRDETAFECINS);

                    int iTRANENTRDETAFECACT = dr.GetOrdinal(helper.TRANENTRDETAFECACT);
                    if (!dr.IsDBNull(iTRANENTRDETAFECACT)) entity.TranEntrDetaFecAct = dr.GetDateTime(iTRANENTRDETAFECACT);

                    int iCODIENTRCODIGO = dr.GetOrdinal(helper.CODIENTRCODIGO);
                    if (!dr.IsDBNull(iCODIENTRCODIGO)) entity.CodiEntrCodigo = dr.GetString(iCODIENTRCODIGO);

                    int iFLAGTIPO = dr.GetOrdinal(helper.FLAGTIPO);
                    if (!dr.IsDBNull(iFLAGTIPO)) entity.FlagTipo = dr.GetString(iFLAGTIPO);

                    int iTIPOINFORMACION = dr.GetOrdinal(helper.TIPOINFORMACION);
                    if (!dr.IsDBNull(iTIPOINFORMACION)) entity.TranEntrTipoInformacion = dr.GetString(iTIPOINFORMACION);

                    int iNOMBEMPRESA = dr.GetOrdinal(helper.NOMBEMPRESA);
                    if (!dr.IsDBNull(iNOMBEMPRESA)) entity.NombEmpresa = dr.GetString(iNOMBEMPRESA);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<ExportExcelDTO> ListarCodigoReportado(int emprcodi, int pericodi, int recacodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigoReportado);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.TENTCODIGO, DbType.Int32, recacodi);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.TENTCODIGO, DbType.Int32, recacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ExportExcelDTO entity = new ExportExcelDTO();

                    int iTENTCODIGO = dr.GetOrdinal(helper.TENTCODIGO);
                    if (!dr.IsDBNull(iTENTCODIGO)) entity.CodiEntreRetiCodigo = dr.GetString(iTENTCODIGO);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public void BulkInsert(List<TrnTransEntregaDetalleBullk> entitys)
        {
            dbProvider.AddColumnMapping(helper.TRANENTRCODI, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETACODI, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAVERSION, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANENTRDETADIA, DbType.Int32);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAPROMDIA, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETASUMADIA, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAH96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.TENTDEUSERNAME, DbType.String);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAFECINS, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.TRANENTRDETAFECACT, DbType.DateTime);


            dbProvider.BulkInsert<TrnTransEntregaDetalleBullk>(entitys, helper.TableName);
        }

        public TransferenciaEntregaDetalleDTO GetDemandaByCodVtea(int pericodi, int version, string codvtea, int dia)
        {
            TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDemandaByCodVtea);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TENTCODIGO, DbType.String, codvtea);
            dbProvider.AddInParameter(command, helper.TRANENTRDETADIA, DbType.Int32, dia);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iTRANENTRCODI = dr.GetOrdinal(helper.TRANENTRCODI);
                    if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranEntrCodi = dr.GetInt32(iTRANENTRCODI);

                    int iTRANENTRDETADIA = dr.GetOrdinal(helper.TRANENTRDETADIA);
                    if (!dr.IsDBNull(iTRANENTRDETADIA)) entity.TranEntrDetaDia = dr.GetInt32(iTRANENTRDETADIA);

                    int iTRANENTRDETAH1 = dr.GetOrdinal(helper.TRANENTRDETAH1);
                    if (!dr.IsDBNull(iTRANENTRDETAH1)) entity.TranEntrDetah1 = dr.GetDecimal(iTRANENTRDETAH1);

                    int iTRANENTRDETAH2 = dr.GetOrdinal(helper.TRANENTRDETAH2);
                    if (!dr.IsDBNull(iTRANENTRDETAH2)) entity.TranEntrDetah2 = dr.GetDecimal(iTRANENTRDETAH2);

                    int iTRANENTRDETAH3 = dr.GetOrdinal(helper.TRANENTRDETAH3);
                    if (!dr.IsDBNull(iTRANENTRDETAH3)) entity.TranEntrDetah3 = dr.GetDecimal(iTRANENTRDETAH3);

                    int iTRANENTRDETAH4 = dr.GetOrdinal(helper.TRANENTRDETAH4);
                    if (!dr.IsDBNull(iTRANENTRDETAH4)) entity.TranEntrDetah4 = dr.GetDecimal(iTRANENTRDETAH4);

                    int iTRANENTRDETAH5 = dr.GetOrdinal(helper.TRANENTRDETAH5);
                    if (!dr.IsDBNull(iTRANENTRDETAH5)) entity.TranEntrDetah5 = dr.GetDecimal(iTRANENTRDETAH5);

                    int iTRANENTRDETAH6 = dr.GetOrdinal(helper.TRANENTRDETAH6);
                    if (!dr.IsDBNull(iTRANENTRDETAH6)) entity.TranEntrDetah6 = dr.GetDecimal(iTRANENTRDETAH6);

                    int iTRANENTRDETAH7 = dr.GetOrdinal(helper.TRANENTRDETAH7);
                    if (!dr.IsDBNull(iTRANENTRDETAH7)) entity.TranEntrDetah7 = dr.GetDecimal(iTRANENTRDETAH7);

                    int iTRANENTRDETAH8 = dr.GetOrdinal(helper.TRANENTRDETAH8);
                    if (!dr.IsDBNull(iTRANENTRDETAH8)) entity.TranEntrDetah8 = dr.GetDecimal(iTRANENTRDETAH8);

                    int iTRANENTRDETAH9 = dr.GetOrdinal(helper.TRANENTRDETAH9);
                    if (!dr.IsDBNull(iTRANENTRDETAH9)) entity.TranEntrDetah9 = dr.GetDecimal(iTRANENTRDETAH9);

                    int iTRANENTRDETAH10 = dr.GetOrdinal(helper.TRANENTRDETAH10);
                    if (!dr.IsDBNull(iTRANENTRDETAH10)) entity.TranEntrDetah10 = dr.GetDecimal(iTRANENTRDETAH10);

                    int iTRANENTRDETAH11 = dr.GetOrdinal(helper.TRANENTRDETAH11);
                    if (!dr.IsDBNull(iTRANENTRDETAH11)) entity.TranEntrDetah11 = dr.GetDecimal(iTRANENTRDETAH11);

                    int iTRANENTRDETAH12 = dr.GetOrdinal(helper.TRANENTRDETAH12);
                    if (!dr.IsDBNull(iTRANENTRDETAH12)) entity.TranEntrDetah12 = dr.GetDecimal(iTRANENTRDETAH12);

                    int iTRANENTRDETAH13 = dr.GetOrdinal(helper.TRANENTRDETAH13);
                    if (!dr.IsDBNull(iTRANENTRDETAH13)) entity.TranEntrDetah13 = dr.GetDecimal(iTRANENTRDETAH13);

                    int iTRANENTRDETAH14 = dr.GetOrdinal(helper.TRANENTRDETAH14);
                    if (!dr.IsDBNull(iTRANENTRDETAH14)) entity.TranEntrDetah14 = dr.GetDecimal(iTRANENTRDETAH14);

                    int iTRANENTRDETAH15 = dr.GetOrdinal(helper.TRANENTRDETAH15);
                    if (!dr.IsDBNull(iTRANENTRDETAH15)) entity.TranEntrDetah15 = dr.GetDecimal(iTRANENTRDETAH15);

                    int iTRANENTRDETAH16 = dr.GetOrdinal(helper.TRANENTRDETAH16);
                    if (!dr.IsDBNull(iTRANENTRDETAH16)) entity.TranEntrDetah16 = dr.GetDecimal(iTRANENTRDETAH16);

                    int iTRANENTRDETAH17 = dr.GetOrdinal(helper.TRANENTRDETAH17);
                    if (!dr.IsDBNull(iTRANENTRDETAH17)) entity.TranEntrDetah17 = dr.GetDecimal(iTRANENTRDETAH17);

                    int iTRANENTRDETAH18 = dr.GetOrdinal(helper.TRANENTRDETAH18);
                    if (!dr.IsDBNull(iTRANENTRDETAH18)) entity.TranEntrDetah18 = dr.GetDecimal(iTRANENTRDETAH18);

                    int iTRANENTRDETAH19 = dr.GetOrdinal(helper.TRANENTRDETAH19);
                    if (!dr.IsDBNull(iTRANENTRDETAH19)) entity.TranEntrDetah19 = dr.GetDecimal(iTRANENTRDETAH19);

                    int iTRANENTRDETAH20 = dr.GetOrdinal(helper.TRANENTRDETAH20);
                    if (!dr.IsDBNull(iTRANENTRDETAH20)) entity.TranEntrDetah20 = dr.GetDecimal(iTRANENTRDETAH20);

                    int iTRANENTRDETAH21 = dr.GetOrdinal(helper.TRANENTRDETAH21);
                    if (!dr.IsDBNull(iTRANENTRDETAH21)) entity.TranEntrDetah21 = dr.GetDecimal(iTRANENTRDETAH21);

                    int iTRANENTRDETAH22 = dr.GetOrdinal(helper.TRANENTRDETAH22);
                    if (!dr.IsDBNull(iTRANENTRDETAH22)) entity.TranEntrDetah22 = dr.GetDecimal(iTRANENTRDETAH22);

                    int iTRANENTRDETAH23 = dr.GetOrdinal(helper.TRANENTRDETAH23);
                    if (!dr.IsDBNull(iTRANENTRDETAH23)) entity.TranEntrDetah23 = dr.GetDecimal(iTRANENTRDETAH23);

                    int iTRANENTRDETAH24 = dr.GetOrdinal(helper.TRANENTRDETAH24);
                    if (!dr.IsDBNull(iTRANENTRDETAH24)) entity.TranEntrDetah24 = dr.GetDecimal(iTRANENTRDETAH24);

                    int iTRANENTRDETAH25 = dr.GetOrdinal(helper.TRANENTRDETAH25);
                    if (!dr.IsDBNull(iTRANENTRDETAH25)) entity.TranEntrDetah25 = dr.GetDecimal(iTRANENTRDETAH25);

                    int iTRANENTRDETAH26 = dr.GetOrdinal(helper.TRANENTRDETAH26);
                    if (!dr.IsDBNull(iTRANENTRDETAH26)) entity.TranEntrDetah26 = dr.GetDecimal(iTRANENTRDETAH26);

                    int iTRANENTRDETAH27 = dr.GetOrdinal(helper.TRANENTRDETAH27);
                    if (!dr.IsDBNull(iTRANENTRDETAH27)) entity.TranEntrDetah27 = dr.GetDecimal(iTRANENTRDETAH27);

                    int iTRANENTRDETAH28 = dr.GetOrdinal(helper.TRANENTRDETAH28);
                    if (!dr.IsDBNull(iTRANENTRDETAH28)) entity.TranEntrDetah28 = dr.GetDecimal(iTRANENTRDETAH28);

                    int iTRANENTRDETAH29 = dr.GetOrdinal(helper.TRANENTRDETAH29);
                    if (!dr.IsDBNull(iTRANENTRDETAH29)) entity.TranEntrDetah29 = dr.GetDecimal(iTRANENTRDETAH29);

                    int iTRANENTRDETAH30 = dr.GetOrdinal(helper.TRANENTRDETAH30);
                    if (!dr.IsDBNull(iTRANENTRDETAH30)) entity.TranEntrDetah30 = dr.GetDecimal(iTRANENTRDETAH30);

                    int iTRANENTRDETAH31 = dr.GetOrdinal(helper.TRANENTRDETAH31);
                    if (!dr.IsDBNull(iTRANENTRDETAH31)) entity.TranEntrDetah31 = dr.GetDecimal(iTRANENTRDETAH31);

                    int iTRANENTRDETAH32 = dr.GetOrdinal(helper.TRANENTRDETAH32);
                    if (!dr.IsDBNull(iTRANENTRDETAH32)) entity.TranEntrDetah32 = dr.GetDecimal(iTRANENTRDETAH32);

                    int iTRANENTRDETAH33 = dr.GetOrdinal(helper.TRANENTRDETAH33);
                    if (!dr.IsDBNull(iTRANENTRDETAH33)) entity.TranEntrDetah33 = dr.GetDecimal(iTRANENTRDETAH33);

                    int iTRANENTRDETAH34 = dr.GetOrdinal(helper.TRANENTRDETAH34);
                    if (!dr.IsDBNull(iTRANENTRDETAH34)) entity.TranEntrDetah34 = dr.GetDecimal(iTRANENTRDETAH34);

                    int iTRANENTRDETAH35 = dr.GetOrdinal(helper.TRANENTRDETAH35);
                    if (!dr.IsDBNull(iTRANENTRDETAH35)) entity.TranEntrDetah35 = dr.GetDecimal(iTRANENTRDETAH35);

                    int iTRANENTRDETAH36 = dr.GetOrdinal(helper.TRANENTRDETAH36);
                    if (!dr.IsDBNull(iTRANENTRDETAH36)) entity.TranEntrDetah36 = dr.GetDecimal(iTRANENTRDETAH36);

                    int iTRANENTRDETAH37 = dr.GetOrdinal(helper.TRANENTRDETAH37);
                    if (!dr.IsDBNull(iTRANENTRDETAH37)) entity.TranEntrDetah37 = dr.GetDecimal(iTRANENTRDETAH37);

                    int iTRANENTRDETAH38 = dr.GetOrdinal(helper.TRANENTRDETAH38);
                    if (!dr.IsDBNull(iTRANENTRDETAH38)) entity.TranEntrDetah38 = dr.GetDecimal(iTRANENTRDETAH38);

                    int iTRANENTRDETAH39 = dr.GetOrdinal(helper.TRANENTRDETAH39);
                    if (!dr.IsDBNull(iTRANENTRDETAH39)) entity.TranEntrDetah39 = dr.GetDecimal(iTRANENTRDETAH39);

                    int iTRANENTRDETAH40 = dr.GetOrdinal(helper.TRANENTRDETAH40);
                    if (!dr.IsDBNull(iTRANENTRDETAH40)) entity.TranEntrDetah40 = dr.GetDecimal(iTRANENTRDETAH40);

                    int iTRANENTRDETAH41 = dr.GetOrdinal(helper.TRANENTRDETAH41);
                    if (!dr.IsDBNull(iTRANENTRDETAH41)) entity.TranEntrDetah41 = dr.GetDecimal(iTRANENTRDETAH41);

                    int iTRANENTRDETAH42 = dr.GetOrdinal(helper.TRANENTRDETAH42);
                    if (!dr.IsDBNull(iTRANENTRDETAH42)) entity.TranEntrDetah42 = dr.GetDecimal(iTRANENTRDETAH42);

                    int iTRANENTRDETAH43 = dr.GetOrdinal(helper.TRANENTRDETAH43);
                    if (!dr.IsDBNull(iTRANENTRDETAH43)) entity.TranEntrDetah43 = dr.GetDecimal(iTRANENTRDETAH43);

                    int iTRANENTRDETAH44 = dr.GetOrdinal(helper.TRANENTRDETAH44);
                    if (!dr.IsDBNull(iTRANENTRDETAH44)) entity.TranEntrDetah44 = dr.GetDecimal(iTRANENTRDETAH44);

                    int iTRANENTRDETAH45 = dr.GetOrdinal(helper.TRANENTRDETAH45);
                    if (!dr.IsDBNull(iTRANENTRDETAH45)) entity.TranEntrDetah45 = dr.GetDecimal(iTRANENTRDETAH45);

                    int iTRANENTRDETAH46 = dr.GetOrdinal(helper.TRANENTRDETAH46);
                    if (!dr.IsDBNull(iTRANENTRDETAH46)) entity.TranEntrDetah46 = dr.GetDecimal(iTRANENTRDETAH46);

                    int iTRANENTRDETAH47 = dr.GetOrdinal(helper.TRANENTRDETAH47);
                    if (!dr.IsDBNull(iTRANENTRDETAH47)) entity.TranEntrDetah47 = dr.GetDecimal(iTRANENTRDETAH47);

                    int iTRANENTRDETAH48 = dr.GetOrdinal(helper.TRANENTRDETAH48);
                    if (!dr.IsDBNull(iTRANENTRDETAH48)) entity.TranEntrDetah48 = dr.GetDecimal(iTRANENTRDETAH48);

                    int iTRANENTRDETAH49 = dr.GetOrdinal(helper.TRANENTRDETAH49);
                    if (!dr.IsDBNull(iTRANENTRDETAH49)) entity.TranEntrDetah49 = dr.GetDecimal(iTRANENTRDETAH49);

                    int iTRANENTRDETAH50 = dr.GetOrdinal(helper.TRANENTRDETAH50);
                    if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah50 = dr.GetDecimal(iTRANENTRDETAH50);

                    int iTRANENTRDETAH51 = dr.GetOrdinal(helper.TRANENTRDETAH51);
                    if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah51 = dr.GetDecimal(iTRANENTRDETAH51);

                    int iTRANENTRDETAH52 = dr.GetOrdinal(helper.TRANENTRDETAH52);
                    if (!dr.IsDBNull(iTRANENTRDETAH52)) entity.TranEntrDetah52 = dr.GetDecimal(iTRANENTRDETAH52);

                    int iTRANENTRDETAH53 = dr.GetOrdinal(helper.TRANENTRDETAH53);
                    if (!dr.IsDBNull(iTRANENTRDETAH53)) entity.TranEntrDetah53 = dr.GetDecimal(iTRANENTRDETAH53);

                    int iTRANENTRDETAH54 = dr.GetOrdinal(helper.TRANENTRDETAH54);
                    if (!dr.IsDBNull(iTRANENTRDETAH54)) entity.TranEntrDetah54 = dr.GetDecimal(iTRANENTRDETAH54);

                    int iTRANENTRDETAH55 = dr.GetOrdinal(helper.TRANENTRDETAH55);
                    if (!dr.IsDBNull(iTRANENTRDETAH55)) entity.TranEntrDetah55 = dr.GetDecimal(iTRANENTRDETAH55);

                    int iTRANENTRDETAH56 = dr.GetOrdinal(helper.TRANENTRDETAH56);
                    if (!dr.IsDBNull(iTRANENTRDETAH56)) entity.TranEntrDetah56 = dr.GetDecimal(iTRANENTRDETAH56);

                    int iTRANENTRDETAH57 = dr.GetOrdinal(helper.TRANENTRDETAH57);
                    if (!dr.IsDBNull(iTRANENTRDETAH57)) entity.TranEntrDetah57 = dr.GetDecimal(iTRANENTRDETAH57);

                    int iTRANENTRDETAH58 = dr.GetOrdinal(helper.TRANENTRDETAH58);
                    if (!dr.IsDBNull(iTRANENTRDETAH58)) entity.TranEntrDetah58 = dr.GetDecimal(iTRANENTRDETAH58);

                    int iTRANENTRDETAH59 = dr.GetOrdinal(helper.TRANENTRDETAH59);
                    if (!dr.IsDBNull(iTRANENTRDETAH59)) entity.TranEntrDetah59 = dr.GetDecimal(iTRANENTRDETAH59);

                    int iTRANENTRDETAH60 = dr.GetOrdinal(helper.TRANENTRDETAH60);
                    if (!dr.IsDBNull(iTRANENTRDETAH60)) entity.TranEntrDetah60 = dr.GetDecimal(iTRANENTRDETAH60);

                    int iTRANENTRDETAH61 = dr.GetOrdinal(helper.TRANENTRDETAH61);
                    if (!dr.IsDBNull(iTRANENTRDETAH61)) entity.TranEntrDetah61 = dr.GetDecimal(iTRANENTRDETAH61);

                    int iTRANENTRDETAH62 = dr.GetOrdinal(helper.TRANENTRDETAH62);
                    if (!dr.IsDBNull(iTRANENTRDETAH62)) entity.TranEntrDetah62 = dr.GetDecimal(iTRANENTRDETAH62);

                    int iTRANENTRDETAH63 = dr.GetOrdinal(helper.TRANENTRDETAH63);
                    if (!dr.IsDBNull(iTRANENTRDETAH63)) entity.TranEntrDetah63 = dr.GetDecimal(iTRANENTRDETAH63);

                    int iTRANENTRDETAH64 = dr.GetOrdinal(helper.TRANENTRDETAH64);
                    if (!dr.IsDBNull(iTRANENTRDETAH64)) entity.TranEntrDetah64 = dr.GetDecimal(iTRANENTRDETAH64);

                    int iTRANENTRDETAH65 = dr.GetOrdinal(helper.TRANENTRDETAH65);
                    if (!dr.IsDBNull(iTRANENTRDETAH65)) entity.TranEntrDetah65 = dr.GetDecimal(iTRANENTRDETAH65);

                    int iTRANENTRDETAH66 = dr.GetOrdinal(helper.TRANENTRDETAH66);
                    if (!dr.IsDBNull(iTRANENTRDETAH66)) entity.TranEntrDetah66 = dr.GetDecimal(iTRANENTRDETAH66);

                    int iTRANENTRDETAH67 = dr.GetOrdinal(helper.TRANENTRDETAH67);
                    if (!dr.IsDBNull(iTRANENTRDETAH67)) entity.TranEntrDetah67 = dr.GetDecimal(iTRANENTRDETAH67);

                    int iTRANENTRDETAH68 = dr.GetOrdinal(helper.TRANENTRDETAH68);
                    if (!dr.IsDBNull(iTRANENTRDETAH68)) entity.TranEntrDetah68 = dr.GetDecimal(iTRANENTRDETAH68);

                    int iTRANENTRDETAH69 = dr.GetOrdinal(helper.TRANENTRDETAH69);
                    if (!dr.IsDBNull(iTRANENTRDETAH69)) entity.TranEntrDetah69 = dr.GetDecimal(iTRANENTRDETAH69);

                    int iTRANENTRDETAH70 = dr.GetOrdinal(helper.TRANENTRDETAH70);
                    if (!dr.IsDBNull(iTRANENTRDETAH70)) entity.TranEntrDetah70 = dr.GetDecimal(iTRANENTRDETAH70);

                    int iTRANENTRDETAH71 = dr.GetOrdinal(helper.TRANENTRDETAH71);
                    if (!dr.IsDBNull(iTRANENTRDETAH71)) entity.TranEntrDetah71 = dr.GetDecimal(iTRANENTRDETAH71);

                    int iTRANENTRDETAH72 = dr.GetOrdinal(helper.TRANENTRDETAH72);
                    if (!dr.IsDBNull(iTRANENTRDETAH72)) entity.TranEntrDetah72 = dr.GetDecimal(iTRANENTRDETAH72);

                    int iTRANENTRDETAH73 = dr.GetOrdinal(helper.TRANENTRDETAH73);
                    if (!dr.IsDBNull(iTRANENTRDETAH73)) entity.TranEntrDetah73 = dr.GetDecimal(iTRANENTRDETAH73);

                    int iTRANENTRDETAH74 = dr.GetOrdinal(helper.TRANENTRDETAH74);
                    if (!dr.IsDBNull(iTRANENTRDETAH74)) entity.TranEntrDetah74 = dr.GetDecimal(iTRANENTRDETAH74);

                    int iTRANENTRDETAH75 = dr.GetOrdinal(helper.TRANENTRDETAH75);
                    if (!dr.IsDBNull(iTRANENTRDETAH75)) entity.TranEntrDetah75 = dr.GetDecimal(iTRANENTRDETAH75);

                    int iTRANENTRDETAH76 = dr.GetOrdinal(helper.TRANENTRDETAH76);
                    if (!dr.IsDBNull(iTRANENTRDETAH76)) entity.TranEntrDetah76 = dr.GetDecimal(iTRANENTRDETAH76);

                    int iTRANENTRDETAH77 = dr.GetOrdinal(helper.TRANENTRDETAH77);
                    if (!dr.IsDBNull(iTRANENTRDETAH77)) entity.TranEntrDetah77 = dr.GetDecimal(iTRANENTRDETAH77);

                    int iTRANENTRDETAH78 = dr.GetOrdinal(helper.TRANENTRDETAH78);
                    if (!dr.IsDBNull(iTRANENTRDETAH78)) entity.TranEntrDetah78 = dr.GetDecimal(iTRANENTRDETAH78);

                    int iTRANENTRDETAH79 = dr.GetOrdinal(helper.TRANENTRDETAH79);
                    if (!dr.IsDBNull(iTRANENTRDETAH79)) entity.TranEntrDetah79 = dr.GetDecimal(iTRANENTRDETAH79);

                    int iTRANENTRDETAH80 = dr.GetOrdinal(helper.TRANENTRDETAH80);
                    if (!dr.IsDBNull(iTRANENTRDETAH80)) entity.TranEntrDetah80 = dr.GetDecimal(iTRANENTRDETAH80);

                    int iTRANENTRDETAH81 = dr.GetOrdinal(helper.TRANENTRDETAH81);
                    if (!dr.IsDBNull(iTRANENTRDETAH81)) entity.TranEntrDetah81 = dr.GetDecimal(iTRANENTRDETAH81);

                    int iTRANENTRDETAH82 = dr.GetOrdinal(helper.TRANENTRDETAH82);
                    if (!dr.IsDBNull(iTRANENTRDETAH82)) entity.TranEntrDetah82 = dr.GetDecimal(iTRANENTRDETAH82);

                    int iTRANENTRDETAH83 = dr.GetOrdinal(helper.TRANENTRDETAH83);
                    if (!dr.IsDBNull(iTRANENTRDETAH83)) entity.TranEntrDetah83 = dr.GetDecimal(iTRANENTRDETAH83);

                    int iTRANENTRDETAH84 = dr.GetOrdinal(helper.TRANENTRDETAH84);
                    if (!dr.IsDBNull(iTRANENTRDETAH84)) entity.TranEntrDetah84 = dr.GetDecimal(iTRANENTRDETAH84);

                    int iTRANENTRDETAH85 = dr.GetOrdinal(helper.TRANENTRDETAH85);
                    if (!dr.IsDBNull(iTRANENTRDETAH85)) entity.TranEntrDetah85 = dr.GetDecimal(iTRANENTRDETAH85);

                    int iTRANENTRDETAH86 = dr.GetOrdinal(helper.TRANENTRDETAH86);
                    if (!dr.IsDBNull(iTRANENTRDETAH86)) entity.TranEntrDetah86 = dr.GetDecimal(iTRANENTRDETAH86);

                    int iTRANENTRDETAH87 = dr.GetOrdinal(helper.TRANENTRDETAH87);
                    if (!dr.IsDBNull(iTRANENTRDETAH87)) entity.TranEntrDetah87 = dr.GetDecimal(iTRANENTRDETAH87);

                    int iTRANENTRDETAH88 = dr.GetOrdinal(helper.TRANENTRDETAH88);
                    if (!dr.IsDBNull(iTRANENTRDETAH88)) entity.TranEntrDetah88 = dr.GetDecimal(iTRANENTRDETAH88);

                    int iTRANENTRDETAH89 = dr.GetOrdinal(helper.TRANENTRDETAH89);
                    if (!dr.IsDBNull(iTRANENTRDETAH89)) entity.TranEntrDetah89 = dr.GetDecimal(iTRANENTRDETAH89);

                    int iTRANENTRDETAH90 = dr.GetOrdinal(helper.TRANENTRDETAH90);
                    if (!dr.IsDBNull(iTRANENTRDETAH90)) entity.TranEntrDetah90 = dr.GetDecimal(iTRANENTRDETAH90);

                    int iTRANENTRDETAH91 = dr.GetOrdinal(helper.TRANENTRDETAH91);
                    if (!dr.IsDBNull(iTRANENTRDETAH91)) entity.TranEntrDetah91 = dr.GetDecimal(iTRANENTRDETAH91);

                    int iTRANENTRDETAH92 = dr.GetOrdinal(helper.TRANENTRDETAH92);
                    if (!dr.IsDBNull(iTRANENTRDETAH92)) entity.TranEntrDetah92 = dr.GetDecimal(iTRANENTRDETAH92);

                    int iTRANENTRDETAH93 = dr.GetOrdinal(helper.TRANENTRDETAH93);
                    if (!dr.IsDBNull(iTRANENTRDETAH93)) entity.TranEntrDetah93 = dr.GetDecimal(iTRANENTRDETAH93);

                    int iTRANENTRDETAH94 = dr.GetOrdinal(helper.TRANENTRDETAH94);
                    if (!dr.IsDBNull(iTRANENTRDETAH94)) entity.TranEntrDetah94 = dr.GetDecimal(iTRANENTRDETAH94);

                    int iTRANENTRDETAH95 = dr.GetOrdinal(helper.TRANENTRDETAH95);
                    if (!dr.IsDBNull(iTRANENTRDETAH95)) entity.TranEntrDetah95 = dr.GetDecimal(iTRANENTRDETAH95);

                    int iTRANENTRDETAH96 = dr.GetOrdinal(helper.TRANENTRDETAH96);
                    if (!dr.IsDBNull(iTRANENTRDETAH96)) entity.TranEntrDetah96 = dr.GetDecimal(iTRANENTRDETAH96);

                }
                return entity;
            }

        }
        public TransferenciaEntregaDetalleDTO GetDemandaByCodVteaEmpresa(int pericodi, int version, string codvtea, int dia, int emprcodi)
        {
            TransferenciaEntregaDetalleDTO entity = new TransferenciaEntregaDetalleDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetDemandaByCodVteaEmpresa);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.TENTCODIGO, DbType.String, codvtea);
            dbProvider.AddInParameter(command, helper.TRANENTRDETADIA, DbType.Int32, dia);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.TRANENTRVERSION, DbType.Int32, version);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iTRANENTRCODI = dr.GetOrdinal(helper.TRANENTRCODI);
                    if (!dr.IsDBNull(iTRANENTRCODI)) entity.TranEntrCodi = dr.GetInt32(iTRANENTRCODI);

                    int iTRANENTRDETADIA = dr.GetOrdinal(helper.TRANENTRDETADIA);
                    if (!dr.IsDBNull(iTRANENTRDETADIA)) entity.TranEntrDetaDia = dr.GetInt32(iTRANENTRDETADIA);

                    int iTRANENTRDETAH1 = dr.GetOrdinal(helper.TRANENTRDETAH1);
                    if (!dr.IsDBNull(iTRANENTRDETAH1)) entity.TranEntrDetah1 = dr.GetDecimal(iTRANENTRDETAH1);

                    int iTRANENTRDETAH2 = dr.GetOrdinal(helper.TRANENTRDETAH2);
                    if (!dr.IsDBNull(iTRANENTRDETAH2)) entity.TranEntrDetah2 = dr.GetDecimal(iTRANENTRDETAH2);

                    int iTRANENTRDETAH3 = dr.GetOrdinal(helper.TRANENTRDETAH3);
                    if (!dr.IsDBNull(iTRANENTRDETAH3)) entity.TranEntrDetah3 = dr.GetDecimal(iTRANENTRDETAH3);

                    int iTRANENTRDETAH4 = dr.GetOrdinal(helper.TRANENTRDETAH4);
                    if (!dr.IsDBNull(iTRANENTRDETAH4)) entity.TranEntrDetah4 = dr.GetDecimal(iTRANENTRDETAH4);

                    int iTRANENTRDETAH5 = dr.GetOrdinal(helper.TRANENTRDETAH5);
                    if (!dr.IsDBNull(iTRANENTRDETAH5)) entity.TranEntrDetah5 = dr.GetDecimal(iTRANENTRDETAH5);

                    int iTRANENTRDETAH6 = dr.GetOrdinal(helper.TRANENTRDETAH6);
                    if (!dr.IsDBNull(iTRANENTRDETAH6)) entity.TranEntrDetah6 = dr.GetDecimal(iTRANENTRDETAH6);

                    int iTRANENTRDETAH7 = dr.GetOrdinal(helper.TRANENTRDETAH7);
                    if (!dr.IsDBNull(iTRANENTRDETAH7)) entity.TranEntrDetah7 = dr.GetDecimal(iTRANENTRDETAH7);

                    int iTRANENTRDETAH8 = dr.GetOrdinal(helper.TRANENTRDETAH8);
                    if (!dr.IsDBNull(iTRANENTRDETAH8)) entity.TranEntrDetah8 = dr.GetDecimal(iTRANENTRDETAH8);

                    int iTRANENTRDETAH9 = dr.GetOrdinal(helper.TRANENTRDETAH9);
                    if (!dr.IsDBNull(iTRANENTRDETAH9)) entity.TranEntrDetah9 = dr.GetDecimal(iTRANENTRDETAH9);

                    int iTRANENTRDETAH10 = dr.GetOrdinal(helper.TRANENTRDETAH10);
                    if (!dr.IsDBNull(iTRANENTRDETAH10)) entity.TranEntrDetah10 = dr.GetDecimal(iTRANENTRDETAH10);

                    int iTRANENTRDETAH11 = dr.GetOrdinal(helper.TRANENTRDETAH11);
                    if (!dr.IsDBNull(iTRANENTRDETAH11)) entity.TranEntrDetah11 = dr.GetDecimal(iTRANENTRDETAH11);

                    int iTRANENTRDETAH12 = dr.GetOrdinal(helper.TRANENTRDETAH12);
                    if (!dr.IsDBNull(iTRANENTRDETAH12)) entity.TranEntrDetah12 = dr.GetDecimal(iTRANENTRDETAH12);

                    int iTRANENTRDETAH13 = dr.GetOrdinal(helper.TRANENTRDETAH13);
                    if (!dr.IsDBNull(iTRANENTRDETAH13)) entity.TranEntrDetah13 = dr.GetDecimal(iTRANENTRDETAH13);

                    int iTRANENTRDETAH14 = dr.GetOrdinal(helper.TRANENTRDETAH14);
                    if (!dr.IsDBNull(iTRANENTRDETAH14)) entity.TranEntrDetah14 = dr.GetDecimal(iTRANENTRDETAH14);

                    int iTRANENTRDETAH15 = dr.GetOrdinal(helper.TRANENTRDETAH15);
                    if (!dr.IsDBNull(iTRANENTRDETAH15)) entity.TranEntrDetah15 = dr.GetDecimal(iTRANENTRDETAH15);

                    int iTRANENTRDETAH16 = dr.GetOrdinal(helper.TRANENTRDETAH16);
                    if (!dr.IsDBNull(iTRANENTRDETAH16)) entity.TranEntrDetah16 = dr.GetDecimal(iTRANENTRDETAH16);

                    int iTRANENTRDETAH17 = dr.GetOrdinal(helper.TRANENTRDETAH17);
                    if (!dr.IsDBNull(iTRANENTRDETAH17)) entity.TranEntrDetah17 = dr.GetDecimal(iTRANENTRDETAH17);

                    int iTRANENTRDETAH18 = dr.GetOrdinal(helper.TRANENTRDETAH18);
                    if (!dr.IsDBNull(iTRANENTRDETAH18)) entity.TranEntrDetah18 = dr.GetDecimal(iTRANENTRDETAH18);

                    int iTRANENTRDETAH19 = dr.GetOrdinal(helper.TRANENTRDETAH19);
                    if (!dr.IsDBNull(iTRANENTRDETAH19)) entity.TranEntrDetah19 = dr.GetDecimal(iTRANENTRDETAH19);

                    int iTRANENTRDETAH20 = dr.GetOrdinal(helper.TRANENTRDETAH20);
                    if (!dr.IsDBNull(iTRANENTRDETAH20)) entity.TranEntrDetah20 = dr.GetDecimal(iTRANENTRDETAH20);

                    int iTRANENTRDETAH21 = dr.GetOrdinal(helper.TRANENTRDETAH21);
                    if (!dr.IsDBNull(iTRANENTRDETAH21)) entity.TranEntrDetah21 = dr.GetDecimal(iTRANENTRDETAH21);

                    int iTRANENTRDETAH22 = dr.GetOrdinal(helper.TRANENTRDETAH22);
                    if (!dr.IsDBNull(iTRANENTRDETAH22)) entity.TranEntrDetah22 = dr.GetDecimal(iTRANENTRDETAH22);

                    int iTRANENTRDETAH23 = dr.GetOrdinal(helper.TRANENTRDETAH23);
                    if (!dr.IsDBNull(iTRANENTRDETAH23)) entity.TranEntrDetah23 = dr.GetDecimal(iTRANENTRDETAH23);

                    int iTRANENTRDETAH24 = dr.GetOrdinal(helper.TRANENTRDETAH24);
                    if (!dr.IsDBNull(iTRANENTRDETAH24)) entity.TranEntrDetah24 = dr.GetDecimal(iTRANENTRDETAH24);

                    int iTRANENTRDETAH25 = dr.GetOrdinal(helper.TRANENTRDETAH25);
                    if (!dr.IsDBNull(iTRANENTRDETAH25)) entity.TranEntrDetah25 = dr.GetDecimal(iTRANENTRDETAH25);

                    int iTRANENTRDETAH26 = dr.GetOrdinal(helper.TRANENTRDETAH26);
                    if (!dr.IsDBNull(iTRANENTRDETAH26)) entity.TranEntrDetah26 = dr.GetDecimal(iTRANENTRDETAH26);

                    int iTRANENTRDETAH27 = dr.GetOrdinal(helper.TRANENTRDETAH27);
                    if (!dr.IsDBNull(iTRANENTRDETAH27)) entity.TranEntrDetah27 = dr.GetDecimal(iTRANENTRDETAH27);

                    int iTRANENTRDETAH28 = dr.GetOrdinal(helper.TRANENTRDETAH28);
                    if (!dr.IsDBNull(iTRANENTRDETAH28)) entity.TranEntrDetah28 = dr.GetDecimal(iTRANENTRDETAH28);

                    int iTRANENTRDETAH29 = dr.GetOrdinal(helper.TRANENTRDETAH29);
                    if (!dr.IsDBNull(iTRANENTRDETAH29)) entity.TranEntrDetah29 = dr.GetDecimal(iTRANENTRDETAH29);

                    int iTRANENTRDETAH30 = dr.GetOrdinal(helper.TRANENTRDETAH30);
                    if (!dr.IsDBNull(iTRANENTRDETAH30)) entity.TranEntrDetah30 = dr.GetDecimal(iTRANENTRDETAH30);

                    int iTRANENTRDETAH31 = dr.GetOrdinal(helper.TRANENTRDETAH31);
                    if (!dr.IsDBNull(iTRANENTRDETAH31)) entity.TranEntrDetah31 = dr.GetDecimal(iTRANENTRDETAH31);

                    int iTRANENTRDETAH32 = dr.GetOrdinal(helper.TRANENTRDETAH32);
                    if (!dr.IsDBNull(iTRANENTRDETAH32)) entity.TranEntrDetah32 = dr.GetDecimal(iTRANENTRDETAH32);

                    int iTRANENTRDETAH33 = dr.GetOrdinal(helper.TRANENTRDETAH33);
                    if (!dr.IsDBNull(iTRANENTRDETAH33)) entity.TranEntrDetah33 = dr.GetDecimal(iTRANENTRDETAH33);

                    int iTRANENTRDETAH34 = dr.GetOrdinal(helper.TRANENTRDETAH34);
                    if (!dr.IsDBNull(iTRANENTRDETAH34)) entity.TranEntrDetah34 = dr.GetDecimal(iTRANENTRDETAH34);

                    int iTRANENTRDETAH35 = dr.GetOrdinal(helper.TRANENTRDETAH35);
                    if (!dr.IsDBNull(iTRANENTRDETAH35)) entity.TranEntrDetah35 = dr.GetDecimal(iTRANENTRDETAH35);

                    int iTRANENTRDETAH36 = dr.GetOrdinal(helper.TRANENTRDETAH36);
                    if (!dr.IsDBNull(iTRANENTRDETAH36)) entity.TranEntrDetah36 = dr.GetDecimal(iTRANENTRDETAH36);

                    int iTRANENTRDETAH37 = dr.GetOrdinal(helper.TRANENTRDETAH37);
                    if (!dr.IsDBNull(iTRANENTRDETAH37)) entity.TranEntrDetah37 = dr.GetDecimal(iTRANENTRDETAH37);

                    int iTRANENTRDETAH38 = dr.GetOrdinal(helper.TRANENTRDETAH38);
                    if (!dr.IsDBNull(iTRANENTRDETAH38)) entity.TranEntrDetah38 = dr.GetDecimal(iTRANENTRDETAH38);

                    int iTRANENTRDETAH39 = dr.GetOrdinal(helper.TRANENTRDETAH39);
                    if (!dr.IsDBNull(iTRANENTRDETAH39)) entity.TranEntrDetah39 = dr.GetDecimal(iTRANENTRDETAH39);

                    int iTRANENTRDETAH40 = dr.GetOrdinal(helper.TRANENTRDETAH40);
                    if (!dr.IsDBNull(iTRANENTRDETAH40)) entity.TranEntrDetah40 = dr.GetDecimal(iTRANENTRDETAH40);

                    int iTRANENTRDETAH41 = dr.GetOrdinal(helper.TRANENTRDETAH41);
                    if (!dr.IsDBNull(iTRANENTRDETAH41)) entity.TranEntrDetah41 = dr.GetDecimal(iTRANENTRDETAH41);

                    int iTRANENTRDETAH42 = dr.GetOrdinal(helper.TRANENTRDETAH42);
                    if (!dr.IsDBNull(iTRANENTRDETAH42)) entity.TranEntrDetah42 = dr.GetDecimal(iTRANENTRDETAH42);

                    int iTRANENTRDETAH43 = dr.GetOrdinal(helper.TRANENTRDETAH43);
                    if (!dr.IsDBNull(iTRANENTRDETAH43)) entity.TranEntrDetah43 = dr.GetDecimal(iTRANENTRDETAH43);

                    int iTRANENTRDETAH44 = dr.GetOrdinal(helper.TRANENTRDETAH44);
                    if (!dr.IsDBNull(iTRANENTRDETAH44)) entity.TranEntrDetah44 = dr.GetDecimal(iTRANENTRDETAH44);

                    int iTRANENTRDETAH45 = dr.GetOrdinal(helper.TRANENTRDETAH45);
                    if (!dr.IsDBNull(iTRANENTRDETAH45)) entity.TranEntrDetah45 = dr.GetDecimal(iTRANENTRDETAH45);

                    int iTRANENTRDETAH46 = dr.GetOrdinal(helper.TRANENTRDETAH46);
                    if (!dr.IsDBNull(iTRANENTRDETAH46)) entity.TranEntrDetah46 = dr.GetDecimal(iTRANENTRDETAH46);

                    int iTRANENTRDETAH47 = dr.GetOrdinal(helper.TRANENTRDETAH47);
                    if (!dr.IsDBNull(iTRANENTRDETAH47)) entity.TranEntrDetah47 = dr.GetDecimal(iTRANENTRDETAH47);

                    int iTRANENTRDETAH48 = dr.GetOrdinal(helper.TRANENTRDETAH48);
                    if (!dr.IsDBNull(iTRANENTRDETAH48)) entity.TranEntrDetah48 = dr.GetDecimal(iTRANENTRDETAH48);

                    int iTRANENTRDETAH49 = dr.GetOrdinal(helper.TRANENTRDETAH49);
                    if (!dr.IsDBNull(iTRANENTRDETAH49)) entity.TranEntrDetah49 = dr.GetDecimal(iTRANENTRDETAH49);

                    int iTRANENTRDETAH50 = dr.GetOrdinal(helper.TRANENTRDETAH50);
                    if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah50 = dr.GetDecimal(iTRANENTRDETAH50);

                    int iTRANENTRDETAH51 = dr.GetOrdinal(helper.TRANENTRDETAH51);
                    if (!dr.IsDBNull(iTRANENTRDETAH50)) entity.TranEntrDetah51 = dr.GetDecimal(iTRANENTRDETAH51);

                    int iTRANENTRDETAH52 = dr.GetOrdinal(helper.TRANENTRDETAH52);
                    if (!dr.IsDBNull(iTRANENTRDETAH52)) entity.TranEntrDetah52 = dr.GetDecimal(iTRANENTRDETAH52);

                    int iTRANENTRDETAH53 = dr.GetOrdinal(helper.TRANENTRDETAH53);
                    if (!dr.IsDBNull(iTRANENTRDETAH53)) entity.TranEntrDetah53 = dr.GetDecimal(iTRANENTRDETAH53);

                    int iTRANENTRDETAH54 = dr.GetOrdinal(helper.TRANENTRDETAH54);
                    if (!dr.IsDBNull(iTRANENTRDETAH54)) entity.TranEntrDetah54 = dr.GetDecimal(iTRANENTRDETAH54);

                    int iTRANENTRDETAH55 = dr.GetOrdinal(helper.TRANENTRDETAH55);
                    if (!dr.IsDBNull(iTRANENTRDETAH55)) entity.TranEntrDetah55 = dr.GetDecimal(iTRANENTRDETAH55);

                    int iTRANENTRDETAH56 = dr.GetOrdinal(helper.TRANENTRDETAH56);
                    if (!dr.IsDBNull(iTRANENTRDETAH56)) entity.TranEntrDetah56 = dr.GetDecimal(iTRANENTRDETAH56);

                    int iTRANENTRDETAH57 = dr.GetOrdinal(helper.TRANENTRDETAH57);
                    if (!dr.IsDBNull(iTRANENTRDETAH57)) entity.TranEntrDetah57 = dr.GetDecimal(iTRANENTRDETAH57);

                    int iTRANENTRDETAH58 = dr.GetOrdinal(helper.TRANENTRDETAH58);
                    if (!dr.IsDBNull(iTRANENTRDETAH58)) entity.TranEntrDetah58 = dr.GetDecimal(iTRANENTRDETAH58);

                    int iTRANENTRDETAH59 = dr.GetOrdinal(helper.TRANENTRDETAH59);
                    if (!dr.IsDBNull(iTRANENTRDETAH59)) entity.TranEntrDetah59 = dr.GetDecimal(iTRANENTRDETAH59);

                    int iTRANENTRDETAH60 = dr.GetOrdinal(helper.TRANENTRDETAH60);
                    if (!dr.IsDBNull(iTRANENTRDETAH60)) entity.TranEntrDetah60 = dr.GetDecimal(iTRANENTRDETAH60);

                    int iTRANENTRDETAH61 = dr.GetOrdinal(helper.TRANENTRDETAH61);
                    if (!dr.IsDBNull(iTRANENTRDETAH61)) entity.TranEntrDetah61 = dr.GetDecimal(iTRANENTRDETAH61);

                    int iTRANENTRDETAH62 = dr.GetOrdinal(helper.TRANENTRDETAH62);
                    if (!dr.IsDBNull(iTRANENTRDETAH62)) entity.TranEntrDetah62 = dr.GetDecimal(iTRANENTRDETAH62);

                    int iTRANENTRDETAH63 = dr.GetOrdinal(helper.TRANENTRDETAH63);
                    if (!dr.IsDBNull(iTRANENTRDETAH63)) entity.TranEntrDetah63 = dr.GetDecimal(iTRANENTRDETAH63);

                    int iTRANENTRDETAH64 = dr.GetOrdinal(helper.TRANENTRDETAH64);
                    if (!dr.IsDBNull(iTRANENTRDETAH64)) entity.TranEntrDetah64 = dr.GetDecimal(iTRANENTRDETAH64);

                    int iTRANENTRDETAH65 = dr.GetOrdinal(helper.TRANENTRDETAH65);
                    if (!dr.IsDBNull(iTRANENTRDETAH65)) entity.TranEntrDetah65 = dr.GetDecimal(iTRANENTRDETAH65);

                    int iTRANENTRDETAH66 = dr.GetOrdinal(helper.TRANENTRDETAH66);
                    if (!dr.IsDBNull(iTRANENTRDETAH66)) entity.TranEntrDetah66 = dr.GetDecimal(iTRANENTRDETAH66);

                    int iTRANENTRDETAH67 = dr.GetOrdinal(helper.TRANENTRDETAH67);
                    if (!dr.IsDBNull(iTRANENTRDETAH67)) entity.TranEntrDetah67 = dr.GetDecimal(iTRANENTRDETAH67);

                    int iTRANENTRDETAH68 = dr.GetOrdinal(helper.TRANENTRDETAH68);
                    if (!dr.IsDBNull(iTRANENTRDETAH68)) entity.TranEntrDetah68 = dr.GetDecimal(iTRANENTRDETAH68);

                    int iTRANENTRDETAH69 = dr.GetOrdinal(helper.TRANENTRDETAH69);
                    if (!dr.IsDBNull(iTRANENTRDETAH69)) entity.TranEntrDetah69 = dr.GetDecimal(iTRANENTRDETAH69);

                    int iTRANENTRDETAH70 = dr.GetOrdinal(helper.TRANENTRDETAH70);
                    if (!dr.IsDBNull(iTRANENTRDETAH70)) entity.TranEntrDetah70 = dr.GetDecimal(iTRANENTRDETAH70);

                    int iTRANENTRDETAH71 = dr.GetOrdinal(helper.TRANENTRDETAH71);
                    if (!dr.IsDBNull(iTRANENTRDETAH71)) entity.TranEntrDetah71 = dr.GetDecimal(iTRANENTRDETAH71);

                    int iTRANENTRDETAH72 = dr.GetOrdinal(helper.TRANENTRDETAH72);
                    if (!dr.IsDBNull(iTRANENTRDETAH72)) entity.TranEntrDetah72 = dr.GetDecimal(iTRANENTRDETAH72);

                    int iTRANENTRDETAH73 = dr.GetOrdinal(helper.TRANENTRDETAH73);
                    if (!dr.IsDBNull(iTRANENTRDETAH73)) entity.TranEntrDetah73 = dr.GetDecimal(iTRANENTRDETAH73);

                    int iTRANENTRDETAH74 = dr.GetOrdinal(helper.TRANENTRDETAH74);
                    if (!dr.IsDBNull(iTRANENTRDETAH74)) entity.TranEntrDetah74 = dr.GetDecimal(iTRANENTRDETAH74);

                    int iTRANENTRDETAH75 = dr.GetOrdinal(helper.TRANENTRDETAH75);
                    if (!dr.IsDBNull(iTRANENTRDETAH75)) entity.TranEntrDetah75 = dr.GetDecimal(iTRANENTRDETAH75);

                    int iTRANENTRDETAH76 = dr.GetOrdinal(helper.TRANENTRDETAH76);
                    if (!dr.IsDBNull(iTRANENTRDETAH76)) entity.TranEntrDetah76 = dr.GetDecimal(iTRANENTRDETAH76);

                    int iTRANENTRDETAH77 = dr.GetOrdinal(helper.TRANENTRDETAH77);
                    if (!dr.IsDBNull(iTRANENTRDETAH77)) entity.TranEntrDetah77 = dr.GetDecimal(iTRANENTRDETAH77);

                    int iTRANENTRDETAH78 = dr.GetOrdinal(helper.TRANENTRDETAH78);
                    if (!dr.IsDBNull(iTRANENTRDETAH78)) entity.TranEntrDetah78 = dr.GetDecimal(iTRANENTRDETAH78);

                    int iTRANENTRDETAH79 = dr.GetOrdinal(helper.TRANENTRDETAH79);
                    if (!dr.IsDBNull(iTRANENTRDETAH79)) entity.TranEntrDetah79 = dr.GetDecimal(iTRANENTRDETAH79);

                    int iTRANENTRDETAH80 = dr.GetOrdinal(helper.TRANENTRDETAH80);
                    if (!dr.IsDBNull(iTRANENTRDETAH80)) entity.TranEntrDetah80 = dr.GetDecimal(iTRANENTRDETAH80);

                    int iTRANENTRDETAH81 = dr.GetOrdinal(helper.TRANENTRDETAH81);
                    if (!dr.IsDBNull(iTRANENTRDETAH81)) entity.TranEntrDetah81 = dr.GetDecimal(iTRANENTRDETAH81);

                    int iTRANENTRDETAH82 = dr.GetOrdinal(helper.TRANENTRDETAH82);
                    if (!dr.IsDBNull(iTRANENTRDETAH82)) entity.TranEntrDetah82 = dr.GetDecimal(iTRANENTRDETAH82);

                    int iTRANENTRDETAH83 = dr.GetOrdinal(helper.TRANENTRDETAH83);
                    if (!dr.IsDBNull(iTRANENTRDETAH83)) entity.TranEntrDetah83 = dr.GetDecimal(iTRANENTRDETAH83);

                    int iTRANENTRDETAH84 = dr.GetOrdinal(helper.TRANENTRDETAH84);
                    if (!dr.IsDBNull(iTRANENTRDETAH84)) entity.TranEntrDetah84 = dr.GetDecimal(iTRANENTRDETAH84);

                    int iTRANENTRDETAH85 = dr.GetOrdinal(helper.TRANENTRDETAH85);
                    if (!dr.IsDBNull(iTRANENTRDETAH85)) entity.TranEntrDetah85 = dr.GetDecimal(iTRANENTRDETAH85);

                    int iTRANENTRDETAH86 = dr.GetOrdinal(helper.TRANENTRDETAH86);
                    if (!dr.IsDBNull(iTRANENTRDETAH86)) entity.TranEntrDetah86 = dr.GetDecimal(iTRANENTRDETAH86);

                    int iTRANENTRDETAH87 = dr.GetOrdinal(helper.TRANENTRDETAH87);
                    if (!dr.IsDBNull(iTRANENTRDETAH87)) entity.TranEntrDetah87 = dr.GetDecimal(iTRANENTRDETAH87);

                    int iTRANENTRDETAH88 = dr.GetOrdinal(helper.TRANENTRDETAH88);
                    if (!dr.IsDBNull(iTRANENTRDETAH88)) entity.TranEntrDetah88 = dr.GetDecimal(iTRANENTRDETAH88);

                    int iTRANENTRDETAH89 = dr.GetOrdinal(helper.TRANENTRDETAH89);
                    if (!dr.IsDBNull(iTRANENTRDETAH89)) entity.TranEntrDetah89 = dr.GetDecimal(iTRANENTRDETAH89);

                    int iTRANENTRDETAH90 = dr.GetOrdinal(helper.TRANENTRDETAH90);
                    if (!dr.IsDBNull(iTRANENTRDETAH90)) entity.TranEntrDetah90 = dr.GetDecimal(iTRANENTRDETAH90);

                    int iTRANENTRDETAH91 = dr.GetOrdinal(helper.TRANENTRDETAH91);
                    if (!dr.IsDBNull(iTRANENTRDETAH91)) entity.TranEntrDetah91 = dr.GetDecimal(iTRANENTRDETAH91);

                    int iTRANENTRDETAH92 = dr.GetOrdinal(helper.TRANENTRDETAH92);
                    if (!dr.IsDBNull(iTRANENTRDETAH92)) entity.TranEntrDetah92 = dr.GetDecimal(iTRANENTRDETAH92);

                    int iTRANENTRDETAH93 = dr.GetOrdinal(helper.TRANENTRDETAH93);
                    if (!dr.IsDBNull(iTRANENTRDETAH93)) entity.TranEntrDetah93 = dr.GetDecimal(iTRANENTRDETAH93);

                    int iTRANENTRDETAH94 = dr.GetOrdinal(helper.TRANENTRDETAH94);
                    if (!dr.IsDBNull(iTRANENTRDETAH94)) entity.TranEntrDetah94 = dr.GetDecimal(iTRANENTRDETAH94);

                    int iTRANENTRDETAH95 = dr.GetOrdinal(helper.TRANENTRDETAH95);
                    if (!dr.IsDBNull(iTRANENTRDETAH95)) entity.TranEntrDetah95 = dr.GetDecimal(iTRANENTRDETAH95);

                    int iTRANENTRDETAH96 = dr.GetOrdinal(helper.TRANENTRDETAH96);
                    if (!dr.IsDBNull(iTRANENTRDETAH96)) entity.TranEntrDetah96 = dr.GetDecimal(iTRANENTRDETAH96);

                }
                return entity;
            }

        }
    }
}
