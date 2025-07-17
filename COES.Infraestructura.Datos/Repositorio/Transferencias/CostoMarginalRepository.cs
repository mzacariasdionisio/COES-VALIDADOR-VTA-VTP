using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class CostoMarginalRepository : RepositoryBase, ICostoMarginalRepository
    {
        public CostoMarginalRepository(string strConn)
            : base(strConn)
        {
        }

        CostoMarginalHelper helper = new CostoMarginalHelper();

        public int Save(CostoMarginalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            int IdCosMarCodi = GetCodigoGenerado();
            dbProvider.AddInParameter(command, helper.CosMarCodi, DbType.Int32, IdCosMarCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CosMarBarraTransferencia, DbType.String, entity.CosMarBarraTransferencia);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.FacPerCodi, DbType.Int32, entity.FacPerCodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, entity.CosMarVersion);
            dbProvider.AddInParameter(command, helper.CosMarDia, DbType.Int32, entity.CosMarDia);
            dbProvider.AddInParameter(command, helper.CosMar1, DbType.Double, entity.CosMar1);
            dbProvider.AddInParameter(command, helper.CosMar2, DbType.Double, entity.CosMar2);
            dbProvider.AddInParameter(command, helper.CosMar3, DbType.Double, entity.CosMar3);
            dbProvider.AddInParameter(command, helper.CosMar4, DbType.Double, entity.CosMar4);
            dbProvider.AddInParameter(command, helper.CosMar5, DbType.Double, entity.CosMar5);
            dbProvider.AddInParameter(command, helper.CosMar6, DbType.Double, entity.CosMar6);
            dbProvider.AddInParameter(command, helper.CosMar7, DbType.Double, entity.CosMar7);
            dbProvider.AddInParameter(command, helper.CosMar8, DbType.Double, entity.CosMar8);
            dbProvider.AddInParameter(command, helper.CosMar9, DbType.Double, entity.CosMar9);
            dbProvider.AddInParameter(command, helper.CosMar10, DbType.Double, entity.CosMar10);
            dbProvider.AddInParameter(command, helper.CosMar11, DbType.Double, entity.CosMar11);
            dbProvider.AddInParameter(command, helper.CosMar12, DbType.Double, entity.CosMar12);
            dbProvider.AddInParameter(command, helper.CosMar13, DbType.Double, entity.CosMar13);
            dbProvider.AddInParameter(command, helper.CosMar14, DbType.Double, entity.CosMar14);
            dbProvider.AddInParameter(command, helper.CosMar15, DbType.Double, entity.CosMar15);
            dbProvider.AddInParameter(command, helper.CosMar16, DbType.Double, entity.CosMar16);
            dbProvider.AddInParameter(command, helper.CosMar17, DbType.Double, entity.CosMar17);
            dbProvider.AddInParameter(command, helper.CosMar18, DbType.Double, entity.CosMar18);
            dbProvider.AddInParameter(command, helper.CosMar19, DbType.Double, entity.CosMar19);
            dbProvider.AddInParameter(command, helper.CosMar20, DbType.Double, entity.CosMar20);
            dbProvider.AddInParameter(command, helper.CosMar21, DbType.Double, entity.CosMar21);
            dbProvider.AddInParameter(command, helper.CosMar22, DbType.Double, entity.CosMar22);
            dbProvider.AddInParameter(command, helper.CosMar23, DbType.Double, entity.CosMar23);
            dbProvider.AddInParameter(command, helper.CosMar24, DbType.Double, entity.CosMar24);
            dbProvider.AddInParameter(command, helper.CosMar25, DbType.Double, entity.CosMar25);
            dbProvider.AddInParameter(command, helper.CosMar26, DbType.Double, entity.CosMar26);
            dbProvider.AddInParameter(command, helper.CosMar27, DbType.Double, entity.CosMar27);
            dbProvider.AddInParameter(command, helper.CosMar28, DbType.Double, entity.CosMar28);
            dbProvider.AddInParameter(command, helper.CosMar29, DbType.Double, entity.CosMar29);
            dbProvider.AddInParameter(command, helper.CosMar30, DbType.Double, entity.CosMar30);
            dbProvider.AddInParameter(command, helper.CosMar31, DbType.Double, entity.CosMar31);
            dbProvider.AddInParameter(command, helper.CosMar32, DbType.Double, entity.CosMar32);
            dbProvider.AddInParameter(command, helper.CosMar33, DbType.Double, entity.CosMar33);
            dbProvider.AddInParameter(command, helper.CosMar34, DbType.Double, entity.CosMar34);
            dbProvider.AddInParameter(command, helper.CosMar35, DbType.Double, entity.CosMar35);
            dbProvider.AddInParameter(command, helper.CosMar36, DbType.Double, entity.CosMar36);
            dbProvider.AddInParameter(command, helper.CosMar37, DbType.Double, entity.CosMar37);
            dbProvider.AddInParameter(command, helper.CosMar38, DbType.Double, entity.CosMar38);
            dbProvider.AddInParameter(command, helper.CosMar39, DbType.Double, entity.CosMar39);
            dbProvider.AddInParameter(command, helper.CosMar40, DbType.Double, entity.CosMar40);
            dbProvider.AddInParameter(command, helper.CosMar41, DbType.Double, entity.CosMar41);
            dbProvider.AddInParameter(command, helper.CosMar42, DbType.Double, entity.CosMar42);
            dbProvider.AddInParameter(command, helper.CosMar43, DbType.Double, entity.CosMar43);
            dbProvider.AddInParameter(command, helper.CosMar44, DbType.Double, entity.CosMar44);
            dbProvider.AddInParameter(command, helper.CosMar45, DbType.Double, entity.CosMar45);
            dbProvider.AddInParameter(command, helper.CosMar46, DbType.Double, entity.CosMar46);
            dbProvider.AddInParameter(command, helper.CosMar47, DbType.Double, entity.CosMar47);
            dbProvider.AddInParameter(command, helper.CosMar48, DbType.Double, entity.CosMar48);
            dbProvider.AddInParameter(command, helper.CosMar49, DbType.Double, entity.CosMar49);
            dbProvider.AddInParameter(command, helper.CosMar50, DbType.Double, entity.CosMar50);
            dbProvider.AddInParameter(command, helper.CosMar51, DbType.Double, entity.CosMar51);
            dbProvider.AddInParameter(command, helper.CosMar52, DbType.Double, entity.CosMar52);
            dbProvider.AddInParameter(command, helper.CosMar53, DbType.Double, entity.CosMar53);
            dbProvider.AddInParameter(command, helper.CosMar54, DbType.Double, entity.CosMar54);
            dbProvider.AddInParameter(command, helper.CosMar55, DbType.Double, entity.CosMar55);
            dbProvider.AddInParameter(command, helper.CosMar56, DbType.Double, entity.CosMar56);
            dbProvider.AddInParameter(command, helper.CosMar57, DbType.Double, entity.CosMar57);
            dbProvider.AddInParameter(command, helper.CosMar58, DbType.Double, entity.CosMar58);
            dbProvider.AddInParameter(command, helper.CosMar59, DbType.Double, entity.CosMar59);
            dbProvider.AddInParameter(command, helper.CosMar60, DbType.Double, entity.CosMar60);
            dbProvider.AddInParameter(command, helper.CosMar61, DbType.Double, entity.CosMar61);
            dbProvider.AddInParameter(command, helper.CosMar62, DbType.Double, entity.CosMar62);
            dbProvider.AddInParameter(command, helper.CosMar63, DbType.Double, entity.CosMar63);
            dbProvider.AddInParameter(command, helper.CosMar64, DbType.Double, entity.CosMar64);
            dbProvider.AddInParameter(command, helper.CosMar65, DbType.Double, entity.CosMar65);
            dbProvider.AddInParameter(command, helper.CosMar66, DbType.Double, entity.CosMar66);
            dbProvider.AddInParameter(command, helper.CosMar67, DbType.Double, entity.CosMar67);
            dbProvider.AddInParameter(command, helper.CosMar68, DbType.Double, entity.CosMar68);
            dbProvider.AddInParameter(command, helper.CosMar69, DbType.Double, entity.CosMar69);
            dbProvider.AddInParameter(command, helper.CosMar70, DbType.Double, entity.CosMar70);
            dbProvider.AddInParameter(command, helper.CosMar71, DbType.Double, entity.CosMar71);
            dbProvider.AddInParameter(command, helper.CosMar72, DbType.Double, entity.CosMar72);
            dbProvider.AddInParameter(command, helper.CosMar73, DbType.Double, entity.CosMar73);
            dbProvider.AddInParameter(command, helper.CosMar74, DbType.Double, entity.CosMar74);
            dbProvider.AddInParameter(command, helper.CosMar75, DbType.Double, entity.CosMar75);
            dbProvider.AddInParameter(command, helper.CosMar76, DbType.Double, entity.CosMar76);
            dbProvider.AddInParameter(command, helper.CosMar77, DbType.Double, entity.CosMar77);
            dbProvider.AddInParameter(command, helper.CosMar78, DbType.Double, entity.CosMar78);
            dbProvider.AddInParameter(command, helper.CosMar79, DbType.Double, entity.CosMar79);
            dbProvider.AddInParameter(command, helper.CosMar80, DbType.Double, entity.CosMar80);
            dbProvider.AddInParameter(command, helper.CosMar81, DbType.Double, entity.CosMar81);
            dbProvider.AddInParameter(command, helper.CosMar82, DbType.Double, entity.CosMar82);
            dbProvider.AddInParameter(command, helper.CosMar83, DbType.Double, entity.CosMar83);
            dbProvider.AddInParameter(command, helper.CosMar84, DbType.Double, entity.CosMar84);
            dbProvider.AddInParameter(command, helper.CosMar85, DbType.Double, entity.CosMar85);
            dbProvider.AddInParameter(command, helper.CosMar86, DbType.Double, entity.CosMar86);
            dbProvider.AddInParameter(command, helper.CosMar87, DbType.Double, entity.CosMar87);
            dbProvider.AddInParameter(command, helper.CosMar88, DbType.Double, entity.CosMar88);
            dbProvider.AddInParameter(command, helper.CosMar89, DbType.Double, entity.CosMar89);
            dbProvider.AddInParameter(command, helper.CosMar90, DbType.Double, entity.CosMar90);
            dbProvider.AddInParameter(command, helper.CosMar91, DbType.Double, entity.CosMar91);
            dbProvider.AddInParameter(command, helper.CosMar92, DbType.Double, entity.CosMar92);
            dbProvider.AddInParameter(command, helper.CosMar93, DbType.Double, entity.CosMar93);
            dbProvider.AddInParameter(command, helper.CosMar94, DbType.Double, entity.CosMar94);
            dbProvider.AddInParameter(command, helper.CosMar95, DbType.Double, entity.CosMar95);
            dbProvider.AddInParameter(command, helper.CosMar96, DbType.Double, entity.CosMar96);
            dbProvider.AddInParameter(command, helper.CosMarPromedioDia, DbType.Double, entity.CosMarPromedioDia);
            dbProvider.AddInParameter(command, helper.CosMarUserName, DbType.String, entity.CosMarUserName);
            dbProvider.AddInParameter(command, helper.CosMarFecIns, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return IdCosMarCodi;
        }

        public void Delete(System.Int32 PeriCod, System.Int32 CosMarVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCod);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, CosMarVersion);
            dbProvider.ExecuteNonQuery(command);
    
            DbCommand command2 = dbProvider.GetSqlStringCommand(helper.SqlDeleteCongene);
            dbProvider.AddInParameter(command2, helper.PeriCodi, DbType.Int32, PeriCod);
            dbProvider.AddInParameter(command2, helper.CosMarVersion, DbType.Int32, CosMarVersion);
            dbProvider.ExecuteNonQuery(command2);
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public int GetCodigoGeneradoDec()
        {
            int newId = -1;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGeneradoDec);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<CostoMarginalDTO> List(int pericodi, int version)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, version);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();

                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iCosMarBarraTransferencia = dr.GetOrdinal(this.helper.CosMarBarraTransferencia);
                    if (!dr.IsDBNull(iCosMarBarraTransferencia)) entity.CosMarBarraTransferencia = dr.GetString(iCosMarBarraTransferencia);

                    entitys.Add(entity);

                }
            }

            return entitys;

        }

        public List<CostoMarginalDTO> GetByCriteria(int pericodi, string barrcodi)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barrcodi);
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
        /// Reporte-Consulta los costos marginales
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="version"></param>
        /// <returns></returns>

        public List<CostoMarginalDTO> GetConsultaCostosMarginales(CostoMarginalDTO parametro)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetConsultaCostosMarginales);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, parametro.PeriCodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, parametro.CosMarVersion);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, parametro.BarrCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entidad = new CostoMarginalDTO();
                    entidad.CosMarCodi = Convert.ToInt32(dr["COSMARCODI"].ToString());
                    entidad.PeriCodi = Convert.ToInt32(dr["PERICODI"].ToString());
                    entidad.BarrCodi = Convert.ToInt32(dr["BARRCODI"].ToString());
                    entidad.FacPerCodi = Convert.ToInt32(dr["FACPERCODI"].ToString());
                    entidad.CosMarVersion = Convert.ToInt32(dr["COSMARVERSION"].ToString());
                    entidad.CosMarDia = Convert.ToInt32(dr["COSMARDIA"].ToString());
                    entidad.Barrnombre = Convert.ToString(dr["COSMARBARRATRANSFERENCIA"].ToString());
                    entidad.CosMarPromedioDia = Convert.ToDecimal(dr["COSMARPROMEDIODIA"].ToString());
                    entidad.CosMar1 = Convert.ToDecimal(dr["COSMAR1"].ToString());
                    entidad.CosMar2 = Convert.ToDecimal(dr["COSMAR2"].ToString());
                    entidad.CosMar3 = Convert.ToDecimal(dr["COSMAR3"].ToString());
                    entidad.CosMar4 = Convert.ToDecimal(dr["COSMAR4"].ToString());
                    entidad.CosMar5 = Convert.ToDecimal(dr["COSMAR5"].ToString());
                    entidad.CosMar6 = Convert.ToDecimal(dr["COSMAR6"].ToString());
                    entidad.CosMar7 = Convert.ToDecimal(dr["COSMAR7"].ToString());
                    entidad.CosMar8 = Convert.ToDecimal(dr["COSMAR8"].ToString());
                    entidad.CosMar9 = Convert.ToDecimal(dr["COSMAR9"].ToString());
                    entidad.CosMar10 = Convert.ToDecimal(dr["COSMAR10"].ToString());
                    entidad.CosMar11 = Convert.ToDecimal(dr["COSMAR11"].ToString());
                    entidad.CosMar12 = Convert.ToDecimal(dr["COSMAR12"].ToString());
                    entidad.CosMar13 = Convert.ToDecimal(dr["COSMAR13"].ToString());
                    entidad.CosMar14 = Convert.ToDecimal(dr["COSMAR14"].ToString());
                    entidad.CosMar15 = Convert.ToDecimal(dr["COSMAR15"].ToString());
                    entidad.CosMar16 = Convert.ToDecimal(dr["COSMAR16"].ToString());
                    entidad.CosMar17 = Convert.ToDecimal(dr["COSMAR17"].ToString());
                    entidad.CosMar18 = Convert.ToDecimal(dr["COSMAR18"].ToString());
                    entidad.CosMar19 = Convert.ToDecimal(dr["COSMAR19"].ToString());
                    entidad.CosMar20 = Convert.ToDecimal(dr["COSMAR20"].ToString());
                    entidad.CosMar21 = Convert.ToDecimal(dr["COSMAR21"].ToString());
                    entidad.CosMar22 = Convert.ToDecimal(dr["COSMAR22"].ToString());
                    entidad.CosMar23 = Convert.ToDecimal(dr["COSMAR23"].ToString());
                    entidad.CosMar24 = Convert.ToDecimal(dr["COSMAR24"].ToString());
                    entidad.CosMar25 = Convert.ToDecimal(dr["COSMAR25"].ToString());
                    entidad.CosMar26 = Convert.ToDecimal(dr["COSMAR26"].ToString());
                    entidad.CosMar27 = Convert.ToDecimal(dr["COSMAR27"].ToString());
                    entidad.CosMar28 = Convert.ToDecimal(dr["COSMAR28"].ToString());
                    entidad.CosMar29 = Convert.ToDecimal(dr["COSMAR29"].ToString());
                    entidad.CosMar30 = Convert.ToDecimal(dr["COSMAR30"].ToString());
                    entidad.CosMar31 = Convert.ToDecimal(dr["COSMAR31"].ToString());
                    entidad.CosMar32 = Convert.ToDecimal(dr["COSMAR32"].ToString());
                    entidad.CosMar33 = Convert.ToDecimal(dr["COSMAR33"].ToString());
                    entidad.CosMar34 = Convert.ToDecimal(dr["COSMAR34"].ToString());
                    entidad.CosMar35 = Convert.ToDecimal(dr["COSMAR35"].ToString());
                    entidad.CosMar36 = Convert.ToDecimal(dr["COSMAR36"].ToString());
                    entidad.CosMar37 = Convert.ToDecimal(dr["COSMAR37"].ToString());
                    entidad.CosMar38 = Convert.ToDecimal(dr["COSMAR38"].ToString());
                    entidad.CosMar39 = Convert.ToDecimal(dr["COSMAR39"].ToString());
                    entidad.CosMar40 = Convert.ToDecimal(dr["COSMAR40"].ToString());
                    entidad.CosMar41 = Convert.ToDecimal(dr["COSMAR41"].ToString());
                    entidad.CosMar42 = Convert.ToDecimal(dr["COSMAR42"].ToString());
                    entidad.CosMar43 = Convert.ToDecimal(dr["COSMAR43"].ToString());
                    entidad.CosMar44 = Convert.ToDecimal(dr["COSMAR44"].ToString());
                    entidad.CosMar45 = Convert.ToDecimal(dr["COSMAR45"].ToString());
                    entidad.CosMar46 = Convert.ToDecimal(dr["COSMAR46"].ToString());
                    entidad.CosMar47 = Convert.ToDecimal(dr["COSMAR47"].ToString());
                    entidad.CosMar48 = Convert.ToDecimal(dr["COSMAR48"].ToString());
                    entidad.CosMar49 = Convert.ToDecimal(dr["COSMAR49"].ToString());
                    entidad.CosMar50 = Convert.ToDecimal(dr["COSMAR50"].ToString());
                    entidad.CosMar51 = Convert.ToDecimal(dr["COSMAR51"].ToString());
                    entidad.CosMar52 = Convert.ToDecimal(dr["COSMAR52"].ToString());
                    entidad.CosMar53 = Convert.ToDecimal(dr["COSMAR53"].ToString());
                    entidad.CosMar54 = Convert.ToDecimal(dr["COSMAR54"].ToString());
                    entidad.CosMar55 = Convert.ToDecimal(dr["COSMAR55"].ToString());
                    entidad.CosMar56 = Convert.ToDecimal(dr["COSMAR56"].ToString());
                    entidad.CosMar57 = Convert.ToDecimal(dr["COSMAR57"].ToString());
                    entidad.CosMar58 = Convert.ToDecimal(dr["COSMAR58"].ToString());
                    entidad.CosMar59 = Convert.ToDecimal(dr["COSMAR59"].ToString());
                    entidad.CosMar60 = Convert.ToDecimal(dr["COSMAR60"].ToString());
                    entidad.CosMar61 = Convert.ToDecimal(dr["COSMAR61"].ToString());
                    entidad.CosMar62 = Convert.ToDecimal(dr["COSMAR62"].ToString());
                    entidad.CosMar63 = Convert.ToDecimal(dr["COSMAR63"].ToString());
                    entidad.CosMar64 = Convert.ToDecimal(dr["COSMAR64"].ToString());
                    entidad.CosMar65 = Convert.ToDecimal(dr["COSMAR65"].ToString());
                    entidad.CosMar66 = Convert.ToDecimal(dr["COSMAR66"].ToString());
                    entidad.CosMar67 = Convert.ToDecimal(dr["COSMAR67"].ToString());
                    entidad.CosMar68 = Convert.ToDecimal(dr["COSMAR68"].ToString());
                    entidad.CosMar69 = Convert.ToDecimal(dr["COSMAR69"].ToString());
                    entidad.CosMar70 = Convert.ToDecimal(dr["COSMAR70"].ToString());
                    entidad.CosMar71 = Convert.ToDecimal(dr["COSMAR71"].ToString());
                    entidad.CosMar72 = Convert.ToDecimal(dr["COSMAR72"].ToString());
                    entidad.CosMar73 = Convert.ToDecimal(dr["COSMAR73"].ToString());
                    entidad.CosMar74 = Convert.ToDecimal(dr["COSMAR74"].ToString());
                    entidad.CosMar75 = Convert.ToDecimal(dr["COSMAR75"].ToString());
                    entidad.CosMar76 = Convert.ToDecimal(dr["COSMAR76"].ToString());
                    entidad.CosMar77 = Convert.ToDecimal(dr["COSMAR77"].ToString());
                    entidad.CosMar78 = Convert.ToDecimal(dr["COSMAR78"].ToString());
                    entidad.CosMar79 = Convert.ToDecimal(dr["COSMAR79"].ToString());
                    entidad.CosMar80 = Convert.ToDecimal(dr["COSMAR80"].ToString());
                    entidad.CosMar81 = Convert.ToDecimal(dr["COSMAR81"].ToString());
                    entidad.CosMar82 = Convert.ToDecimal(dr["COSMAR82"].ToString());
                    entidad.CosMar83 = Convert.ToDecimal(dr["COSMAR83"].ToString());
                    entidad.CosMar84 = Convert.ToDecimal(dr["COSMAR84"].ToString());
                    entidad.CosMar85 = Convert.ToDecimal(dr["COSMAR85"].ToString());
                    entidad.CosMar86 = Convert.ToDecimal(dr["COSMAR86"].ToString());
                    entidad.CosMar87 = Convert.ToDecimal(dr["COSMAR87"].ToString());
                    entidad.CosMar88 = Convert.ToDecimal(dr["COSMAR88"].ToString());
                    entidad.CosMar89 = Convert.ToDecimal(dr["COSMAR89"].ToString());
                    entidad.CosMar90 = Convert.ToDecimal(dr["COSMAR90"].ToString());
                    entidad.CosMar91 = Convert.ToDecimal(dr["COSMAR91"].ToString());
                    entidad.CosMar92 = Convert.ToDecimal(dr["COSMAR92"].ToString());
                    entidad.CosMar93 = Convert.ToDecimal(dr["COSMAR93"].ToString());
                    entidad.CosMar94 = Convert.ToDecimal(dr["COSMAR94"].ToString());
                    entidad.CosMar95 = Convert.ToDecimal(dr["COSMAR95"].ToString());
                    entidad.CosMar96 = Convert.ToDecimal(dr["COSMAR96"].ToString());
                    entidad.CosMarUserName = Convert.ToString(dr["COSMARUSERNAME"].ToString());
                    entidad.CosMarFecIns = Convert.ToDateTime(dr["COSMARFECINS"].ToString());

                    entitys.Add(entidad);
                }
            }

            return entitys;
        }
        public List<CostoMarginalDTO> GetBarrasMarginales(int pericodi, int version)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBarrasMarginales);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, version);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();
                    entity.BarrCodi =Convert.ToInt32( dr["barrcodi"].ToString());
                    entity.CosMarBarraTransferencia = dr["cosmarbarratransferencia"].ToString();
                    entitys.Add(entity);

                }
            }

            return entitys;

        }



        public List<CostoMarginalDTO> ListByBarrPeriodoVersion(int barrcodi, int pericodi, int costmargversion)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            string sqlQuery = string.Format(this.helper.SqlListByBarraPeriodoVers, barrcodi, pericodi, costmargversion);

            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));

                }
            }

            return entitys;
        }

        public List<CostoMarginalDTO> GetByCodigo(int? pericodi)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigo);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    entitys.Add(helper.Create(dr));


                }
            }
            return entitys;
        }

        public List<CostoMarginalDTO> GetByBarraTransferencia(int pericodi, int cosmarversion)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByBarraTransferencia);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, cosmarversion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();

                    int iBarrCodi = dr.GetOrdinal(helper.BarrCodi);
                    if (!dr.IsDBNull(iBarrCodi)) entity.BarrCodi = dr.GetInt32(iBarrCodi);

                    int iCosMarBarraTransferencia = dr.GetOrdinal(helper.CosMarBarraTransferencia);
                    if (!dr.IsDBNull(iCosMarBarraTransferencia)) entity.CosMarBarraTransferencia = dr.GetString(iCosMarBarraTransferencia);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CostoMarginalDTO> ListByFactorPerdida(int iFacPerCodi)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByFactorPerdida);

            dbProvider.AddInParameter(command, helper.FacPerCodi, DbType.Int32, iFacPerCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    entitys.Add(helper.Create(dr));

                }
            }
            return entitys;
        }

        public List<CostoMarginalDTO> ListByReporte(int iPeriCodi, int iCostMargVersion)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByReporte);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, iCostMargVersion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();

                    int iBARRCODI = dr.GetOrdinal(this.helper.BarrCodi);
                    if (!dr.IsDBNull(iBARRCODI)) entity.BarrCodi = dr.GetInt32(iBARRCODI);

                    int iCosMarBarraTransferencia = dr.GetOrdinal(this.helper.CosMarBarraTransferencia);
                    if (!dr.IsDBNull(iCosMarBarraTransferencia)) entity.CosMarBarraTransferencia = dr.GetString(iCosMarBarraTransferencia);

                    entitys.Add(entity);

                }
            }

            return entitys;

        }

        public List<CostoMarginalDTO> ObtenerReporteCostoMarginalDTR(int barracodi, int pericodi, int version)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerReporteCostoMarginalDTR);

            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, barracodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();

                    for (int i = 1; i <= 96; i++)
                    {
                        int index = dr.GetOrdinal(helper.H + i);
                        if (!dr.IsDBNull(index)) entity.GetType().GetProperty(helper.CosMar + i).SetValue(entity, dr.GetDecimal(index));
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        // Inicio de Cambio 31/05/2017 - Sistema de Compensaciones
        // DSH 05-07-2017 : Se cambio por requerimiento
        public List<ComboCompensaciones> ListCostoMarginalVersion(int pericodi)
        {
            List<ComboCompensaciones> entitys = new List<ComboCompensaciones>();

            string queryString = string.Format(helper.SqlListByPeriCodi, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ComboCompensaciones entity = new ComboCompensaciones();

                    int iRecaCodi = dr.GetOrdinal(helper.RecaCodi);
                    if (!dr.IsDBNull(iRecaCodi)) entity.id = dr.GetValue(iRecaCodi).ToString();

                    int iRecaNombre = dr.GetOrdinal(helper.RecaNombre);
                    if (!dr.IsDBNull(iRecaNombre)) entity.name = dr.GetValue(iRecaNombre).ToString();

                    entitys.Add(entity);
                }
            }
            return entitys;

        }
        // Fin de Agregado - Sistema de Compensaciones

        public CostoMarginalDTO GetByIdPorBarraDia(int PeriCodi, int iCostMargVersion, int BarrCodi, int CosMarDia)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPorURS);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, iCostMargVersion);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, BarrCodi);
            dbProvider.AddInParameter(command, helper.CosMarDia, DbType.Int32, CosMarDia);
            CostoMarginalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void BulkInsert(List<TrnCostoMarginalBullk> entitys)
        {
            dbProvider.AddColumnMapping(helper.CosMarCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.PeriCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.BarrCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.FacPerCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CosMarVersion, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CosMarDia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.CosMarBarraTransferencia, DbType.String);
            dbProvider.AddColumnMapping(helper.CosMarPromedioDia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMar96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.CosMarUserName, DbType.String);
            dbProvider.AddColumnMapping(helper.CosMarFecIns, DbType.DateTime);

            dbProvider.BulkInsert<TrnCostoMarginalBullk>(entitys, helper.TableName);
        }

        #region MonitoreoMME

        public List<CostoMarginalDTO> ListCostoMarginalWithGrupo(int pericodi, int version)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            string queryString = string.Format(helper.SqlListCostoMarginalWithGrupo, pericodi, version);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();
                    entity = helper.Create(dr);

                    int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iBarrNombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrNombre)) entity.Barrnombre = dr.GetString(iBarrNombre);

                    int iCosmarDia = dr.GetOrdinal(this.helper.CosMarDia);
                    if (!dr.IsDBNull(iCosmarDia)) entity.CosMarDia = Convert.ToInt32(dr.GetValue(iCosmarDia));

                    int iGrupoCodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = Convert.ToInt32(dr.GetValue(iGrupoCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN2

        public List<CostoMarginalDTO> ListCostoMarginalByPeriodoVersionZona(int periodo, int version, string zona)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();

            var queryString = string.Format(helper.SqlListCostoMarginalByPeriodoVersionZona, periodo, version, zona);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iBarrzarea = dr.GetOrdinal(helper.Barrzarea);
                    if (!dr.IsDBNull(iBarrzarea)) entity.Barrzarea = dr.GetInt32(iBarrzarea);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region AJUSTE COSTOS MARGINALES

        public void AjustarCostosMarginales(string CosMarRec, int CosMarDiaRec, int PericodiRec, int CosMarVerRec, string CosMarEnt, int CosMarDiaEnt, int PericodiEnt, int CosMarVerEnt)
        {
            var queryString = string.Format(helper.SqlAjustarCostosMarginales, CosMarRec, CosMarDiaRec, PericodiRec, CosMarVerRec, CosMarEnt, CosMarDiaEnt, PericodiEnt, CosMarVerEnt);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        //CU21
        public List<CostoMarginalDTO> ListarByCodigoEntrega(int iPeriCodi, int iRecaCodi, int iCodEntCodi)
        {
            List<CostoMarginalDTO> entitys = new List<CostoMarginalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarByCodigoEntrega);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.CosMarVersion, DbType.Int32, iRecaCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, iCodEntCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CostoMarginalDTO entity = new CostoMarginalDTO();

                    int iCosMarDia = dr.GetOrdinal(helper.CosMarDia);
                    if (!dr.IsDBNull(iCosMarDia)) entity.CosMarDia = dr.GetInt32(iCosMarDia);

                    int iCosMar1 = dr.GetOrdinal(helper.CosMar1);
                    if (!dr.IsDBNull(iCosMar1)) entity.CosMar1 = dr.GetDecimal(iCosMar1);

                    int iCosMar2 = dr.GetOrdinal(helper.CosMar2);
                    if (!dr.IsDBNull(iCosMar2)) entity.CosMar2 = dr.GetDecimal(iCosMar2);

                    int iCosMar3 = dr.GetOrdinal(helper.CosMar3);
                    if (!dr.IsDBNull(iCosMar3)) entity.CosMar3 = dr.GetDecimal(iCosMar3);

                    int iCosMar4 = dr.GetOrdinal(helper.CosMar4);
                    if (!dr.IsDBNull(iCosMar4)) entity.CosMar4 = dr.GetDecimal(iCosMar4);

                    int iCosMar5 = dr.GetOrdinal(helper.CosMar5);
                    if (!dr.IsDBNull(iCosMar5)) entity.CosMar5 = dr.GetDecimal(iCosMar5);

                    int iCosMar6 = dr.GetOrdinal(helper.CosMar6);
                    if (!dr.IsDBNull(iCosMar6)) entity.CosMar6 = dr.GetDecimal(iCosMar6);

                    int iCosMar7 = dr.GetOrdinal(helper.CosMar7);
                    if (!dr.IsDBNull(iCosMar7)) entity.CosMar7 = dr.GetDecimal(iCosMar7);

                    int iCosMar8 = dr.GetOrdinal(helper.CosMar8);
                    if (!dr.IsDBNull(iCosMar8)) entity.CosMar8 = dr.GetDecimal(iCosMar8);

                    int iCosMar9 = dr.GetOrdinal(helper.CosMar9);
                    if (!dr.IsDBNull(iCosMar9)) entity.CosMar9 = dr.GetDecimal(iCosMar9);

                    int iCosMar10 = dr.GetOrdinal(helper.CosMar10);
                    if (!dr.IsDBNull(iCosMar10)) entity.CosMar10 = dr.GetDecimal(iCosMar10);

                    int iCosMar11 = dr.GetOrdinal(helper.CosMar11);
                    if (!dr.IsDBNull(iCosMar11)) entity.CosMar11 = dr.GetDecimal(iCosMar11);

                    int iCosMar12 = dr.GetOrdinal(helper.CosMar12);
                    if (!dr.IsDBNull(iCosMar12)) entity.CosMar12 = dr.GetDecimal(iCosMar12);

                    int iCosMar13 = dr.GetOrdinal(helper.CosMar13);
                    if (!dr.IsDBNull(iCosMar13)) entity.CosMar13 = dr.GetDecimal(iCosMar13);

                    int iCosMar14 = dr.GetOrdinal(helper.CosMar14);
                    if (!dr.IsDBNull(iCosMar14)) entity.CosMar14 = dr.GetDecimal(iCosMar14);

                    int iCosMar15 = dr.GetOrdinal(helper.CosMar15);
                    if (!dr.IsDBNull(iCosMar15)) entity.CosMar15 = dr.GetDecimal(iCosMar15);

                    int iCosMar16 = dr.GetOrdinal(helper.CosMar16);
                    if (!dr.IsDBNull(iCosMar16)) entity.CosMar16 = dr.GetDecimal(iCosMar16);

                    int iCosMar17 = dr.GetOrdinal(helper.CosMar17);
                    if (!dr.IsDBNull(iCosMar17)) entity.CosMar17 = dr.GetDecimal(iCosMar17);

                    int iCosMar18 = dr.GetOrdinal(helper.CosMar18);
                    if (!dr.IsDBNull(iCosMar18)) entity.CosMar18 = dr.GetDecimal(iCosMar18);

                    int iCosMar19 = dr.GetOrdinal(helper.CosMar19);
                    if (!dr.IsDBNull(iCosMar19)) entity.CosMar19 = dr.GetDecimal(iCosMar19);

                    int iCosMar20 = dr.GetOrdinal(helper.CosMar20);
                    if (!dr.IsDBNull(iCosMar20)) entity.CosMar20 = dr.GetDecimal(iCosMar20);

                    int iCosMar21 = dr.GetOrdinal(helper.CosMar21);
                    if (!dr.IsDBNull(iCosMar21)) entity.CosMar21 = dr.GetDecimal(iCosMar21);

                    int iCosMar22 = dr.GetOrdinal(helper.CosMar22);
                    if (!dr.IsDBNull(iCosMar22)) entity.CosMar22 = dr.GetDecimal(iCosMar22);

                    int iCosMar23 = dr.GetOrdinal(helper.CosMar23);
                    if (!dr.IsDBNull(iCosMar23)) entity.CosMar23 = dr.GetDecimal(iCosMar23);

                    int iCosMar24 = dr.GetOrdinal(helper.CosMar24);
                    if (!dr.IsDBNull(iCosMar24)) entity.CosMar24 = dr.GetDecimal(iCosMar24);

                    int iCosMar25 = dr.GetOrdinal(helper.CosMar25);
                    if (!dr.IsDBNull(iCosMar25)) entity.CosMar25 = dr.GetDecimal(iCosMar25);

                    int iCosMar26 = dr.GetOrdinal(helper.CosMar26);
                    if (!dr.IsDBNull(iCosMar26)) entity.CosMar26 = dr.GetDecimal(iCosMar26);

                    int iCosMar27 = dr.GetOrdinal(helper.CosMar27);
                    if (!dr.IsDBNull(iCosMar27)) entity.CosMar27 = dr.GetDecimal(iCosMar27);

                    int iCosMar28 = dr.GetOrdinal(helper.CosMar28);
                    if (!dr.IsDBNull(iCosMar28)) entity.CosMar28 = dr.GetDecimal(iCosMar28);

                    int iCosMar29 = dr.GetOrdinal(helper.CosMar29);
                    if (!dr.IsDBNull(iCosMar29)) entity.CosMar29 = dr.GetDecimal(iCosMar29);

                    int iCosMar30 = dr.GetOrdinal(helper.CosMar30);
                    if (!dr.IsDBNull(iCosMar30)) entity.CosMar30 = dr.GetDecimal(iCosMar30);

                    int iCosMar31 = dr.GetOrdinal(helper.CosMar31);
                    if (!dr.IsDBNull(iCosMar31)) entity.CosMar31 = dr.GetDecimal(iCosMar31);

                    int iCosMar32 = dr.GetOrdinal(helper.CosMar32);
                    if (!dr.IsDBNull(iCosMar32)) entity.CosMar32 = dr.GetDecimal(iCosMar32);

                    int iCosMar33 = dr.GetOrdinal(helper.CosMar33);
                    if (!dr.IsDBNull(iCosMar33)) entity.CosMar33 = dr.GetDecimal(iCosMar33);

                    int iCosMar34 = dr.GetOrdinal(helper.CosMar34);
                    if (!dr.IsDBNull(iCosMar34)) entity.CosMar34 = dr.GetDecimal(iCosMar34);

                    int iCosMar35 = dr.GetOrdinal(helper.CosMar35);
                    if (!dr.IsDBNull(iCosMar35)) entity.CosMar35 = dr.GetDecimal(iCosMar35);

                    int iCosMar36 = dr.GetOrdinal(helper.CosMar36);
                    if (!dr.IsDBNull(iCosMar36)) entity.CosMar36 = dr.GetDecimal(iCosMar36);

                    int iCosMar37 = dr.GetOrdinal(helper.CosMar37);
                    if (!dr.IsDBNull(iCosMar37)) entity.CosMar37 = dr.GetDecimal(iCosMar37);

                    int iCosMar38 = dr.GetOrdinal(helper.CosMar38);
                    if (!dr.IsDBNull(iCosMar38)) entity.CosMar38 = dr.GetDecimal(iCosMar38);

                    int iCosMar39 = dr.GetOrdinal(helper.CosMar39);
                    if (!dr.IsDBNull(iCosMar39)) entity.CosMar39 = dr.GetDecimal(iCosMar39);

                    int iCosMar40 = dr.GetOrdinal(helper.CosMar40);
                    if (!dr.IsDBNull(iCosMar40)) entity.CosMar40 = dr.GetDecimal(iCosMar40);

                    int iCosMar41 = dr.GetOrdinal(helper.CosMar41);
                    if (!dr.IsDBNull(iCosMar41)) entity.CosMar41 = dr.GetDecimal(iCosMar41);

                    int iCosMar42 = dr.GetOrdinal(helper.CosMar42);
                    if (!dr.IsDBNull(iCosMar42)) entity.CosMar42 = dr.GetDecimal(iCosMar42);

                    int iCosMar43 = dr.GetOrdinal(helper.CosMar43);
                    if (!dr.IsDBNull(iCosMar43)) entity.CosMar43 = dr.GetDecimal(iCosMar43);

                    int iCosMar44 = dr.GetOrdinal(helper.CosMar44);
                    if (!dr.IsDBNull(iCosMar44)) entity.CosMar44 = dr.GetDecimal(iCosMar44);

                    int iCosMar45 = dr.GetOrdinal(helper.CosMar45);
                    if (!dr.IsDBNull(iCosMar45)) entity.CosMar45 = dr.GetDecimal(iCosMar45);

                    int iCosMar46 = dr.GetOrdinal(helper.CosMar46);
                    if (!dr.IsDBNull(iCosMar46)) entity.CosMar46 = dr.GetDecimal(iCosMar46);

                    int iCosMar47 = dr.GetOrdinal(helper.CosMar47);
                    if (!dr.IsDBNull(iCosMar47)) entity.CosMar47 = dr.GetDecimal(iCosMar47);

                    int iCosMar48 = dr.GetOrdinal(helper.CosMar48);
                    if (!dr.IsDBNull(iCosMar48)) entity.CosMar48 = dr.GetDecimal(iCosMar48);

                    int iCosMar49 = dr.GetOrdinal(helper.CosMar49);
                    if (!dr.IsDBNull(iCosMar49)) entity.CosMar49 = dr.GetDecimal(iCosMar49);

                    int iCosMar50 = dr.GetOrdinal(helper.CosMar50);
                    if (!dr.IsDBNull(iCosMar50)) entity.CosMar50 = dr.GetDecimal(iCosMar50);

                    int iCosMar51 = dr.GetOrdinal(helper.CosMar51);
                    if (!dr.IsDBNull(iCosMar51)) entity.CosMar51 = dr.GetDecimal(iCosMar51);

                    int iCosMar52 = dr.GetOrdinal(helper.CosMar52);
                    if (!dr.IsDBNull(iCosMar52)) entity.CosMar52 = dr.GetDecimal(iCosMar52);

                    int iCosMar53 = dr.GetOrdinal(helper.CosMar53);
                    if (!dr.IsDBNull(iCosMar53)) entity.CosMar53 = dr.GetDecimal(iCosMar53);

                    int iCosMar54 = dr.GetOrdinal(helper.CosMar54);
                    if (!dr.IsDBNull(iCosMar54)) entity.CosMar54 = dr.GetDecimal(iCosMar54);

                    int iCosMar55 = dr.GetOrdinal(helper.CosMar55);
                    if (!dr.IsDBNull(iCosMar55)) entity.CosMar55 = dr.GetDecimal(iCosMar55);

                    int iCosMar56 = dr.GetOrdinal(helper.CosMar56);
                    if (!dr.IsDBNull(iCosMar56)) entity.CosMar56 = dr.GetDecimal(iCosMar56);

                    int iCosMar57 = dr.GetOrdinal(helper.CosMar57);
                    if (!dr.IsDBNull(iCosMar57)) entity.CosMar57 = dr.GetDecimal(iCosMar57);

                    int iCosMar58 = dr.GetOrdinal(helper.CosMar58);
                    if (!dr.IsDBNull(iCosMar58)) entity.CosMar58 = dr.GetDecimal(iCosMar58);

                    int iCosMar59 = dr.GetOrdinal(helper.CosMar59);
                    if (!dr.IsDBNull(iCosMar59)) entity.CosMar59 = dr.GetDecimal(iCosMar59);

                    int iCosMar60 = dr.GetOrdinal(helper.CosMar60);
                    if (!dr.IsDBNull(iCosMar60)) entity.CosMar60 = dr.GetDecimal(iCosMar60);

                    int iCosMar61 = dr.GetOrdinal(helper.CosMar61);
                    if (!dr.IsDBNull(iCosMar61)) entity.CosMar61 = dr.GetDecimal(iCosMar61);

                    int iCosMar62 = dr.GetOrdinal(helper.CosMar62);
                    if (!dr.IsDBNull(iCosMar62)) entity.CosMar62 = dr.GetDecimal(iCosMar62);

                    int iCosMar63 = dr.GetOrdinal(helper.CosMar63);
                    if (!dr.IsDBNull(iCosMar63)) entity.CosMar63 = dr.GetDecimal(iCosMar63);

                    int iCosMar64 = dr.GetOrdinal(helper.CosMar64);
                    if (!dr.IsDBNull(iCosMar64)) entity.CosMar64 = dr.GetDecimal(iCosMar64);

                    int iCosMar65 = dr.GetOrdinal(helper.CosMar65);
                    if (!dr.IsDBNull(iCosMar65)) entity.CosMar65 = dr.GetDecimal(iCosMar65);

                    int iCosMar66 = dr.GetOrdinal(helper.CosMar66);
                    if (!dr.IsDBNull(iCosMar66)) entity.CosMar66 = dr.GetDecimal(iCosMar66);

                    int iCosMar67 = dr.GetOrdinal(helper.CosMar67);
                    if (!dr.IsDBNull(iCosMar67)) entity.CosMar67 = dr.GetDecimal(iCosMar67);

                    int iCosMar68 = dr.GetOrdinal(helper.CosMar68);
                    if (!dr.IsDBNull(iCosMar68)) entity.CosMar68 = dr.GetDecimal(iCosMar68);

                    int iCosMar69 = dr.GetOrdinal(helper.CosMar69);
                    if (!dr.IsDBNull(iCosMar6)) entity.CosMar69 = dr.GetDecimal(iCosMar69);

                    int iCosMar70 = dr.GetOrdinal(helper.CosMar70);
                    if (!dr.IsDBNull(iCosMar70)) entity.CosMar70 = dr.GetDecimal(iCosMar70);

                    int iCosMar71 = dr.GetOrdinal(helper.CosMar71);
                    if (!dr.IsDBNull(iCosMar71)) entity.CosMar71 = dr.GetDecimal(iCosMar71);

                    int iCosMar72 = dr.GetOrdinal(helper.CosMar72);
                    if (!dr.IsDBNull(iCosMar72)) entity.CosMar72 = dr.GetDecimal(iCosMar72);

                    int iCosMar73 = dr.GetOrdinal(helper.CosMar73);
                    if (!dr.IsDBNull(iCosMar73)) entity.CosMar73 = dr.GetDecimal(iCosMar73);

                    int iCosMar74 = dr.GetOrdinal(helper.CosMar74);
                    if (!dr.IsDBNull(iCosMar74)) entity.CosMar74 = dr.GetDecimal(iCosMar74);

                    int iCosMar75 = dr.GetOrdinal(helper.CosMar75);
                    if (!dr.IsDBNull(iCosMar75)) entity.CosMar75 = dr.GetDecimal(iCosMar75);

                    int iCosMar76 = dr.GetOrdinal(helper.CosMar76);
                    if (!dr.IsDBNull(iCosMar76)) entity.CosMar76 = dr.GetDecimal(iCosMar76);

                    int iCosMar77 = dr.GetOrdinal(helper.CosMar77);
                    if (!dr.IsDBNull(iCosMar77)) entity.CosMar77 = dr.GetDecimal(iCosMar77);

                    int iCosMar78 = dr.GetOrdinal(helper.CosMar78);
                    if (!dr.IsDBNull(iCosMar78)) entity.CosMar78 = dr.GetDecimal(iCosMar78);

                    int iCosMar79 = dr.GetOrdinal(helper.CosMar79);
                    if (!dr.IsDBNull(iCosMar79)) entity.CosMar79 = dr.GetDecimal(iCosMar79);

                    int iCosMar80 = dr.GetOrdinal(helper.CosMar80);
                    if (!dr.IsDBNull(iCosMar80)) entity.CosMar80 = dr.GetDecimal(iCosMar80);

                    int iCosMar81 = dr.GetOrdinal(helper.CosMar81);
                    if (!dr.IsDBNull(iCosMar81)) entity.CosMar81 = dr.GetDecimal(iCosMar81);

                    int iCosMar82 = dr.GetOrdinal(helper.CosMar82);
                    if (!dr.IsDBNull(iCosMar82)) entity.CosMar82 = dr.GetDecimal(iCosMar82);

                    int iCosMar83 = dr.GetOrdinal(helper.CosMar83);
                    if (!dr.IsDBNull(iCosMar83)) entity.CosMar83 = dr.GetDecimal(iCosMar83);

                    int iCosMar84 = dr.GetOrdinal(helper.CosMar84);
                    if (!dr.IsDBNull(iCosMar84)) entity.CosMar84 = dr.GetDecimal(iCosMar84);

                    int iCosMar85 = dr.GetOrdinal(helper.CosMar85);
                    if (!dr.IsDBNull(iCosMar85)) entity.CosMar85 = dr.GetDecimal(iCosMar85);

                    int iCosMar86 = dr.GetOrdinal(helper.CosMar86);
                    if (!dr.IsDBNull(iCosMar86)) entity.CosMar86 = dr.GetDecimal(iCosMar86);

                    int iCosMar87 = dr.GetOrdinal(helper.CosMar87);
                    if (!dr.IsDBNull(iCosMar87)) entity.CosMar87 = dr.GetDecimal(iCosMar87);

                    int iCosMar88 = dr.GetOrdinal(helper.CosMar88);
                    if (!dr.IsDBNull(iCosMar88)) entity.CosMar88 = dr.GetDecimal(iCosMar88);

                    int iCosMar89 = dr.GetOrdinal(helper.CosMar89);
                    if (!dr.IsDBNull(iCosMar89)) entity.CosMar89 = dr.GetDecimal(iCosMar89);

                    int iCosMar90 = dr.GetOrdinal(helper.CosMar90);
                    if (!dr.IsDBNull(iCosMar90)) entity.CosMar90 = dr.GetDecimal(iCosMar90);

                    int iCosMar91 = dr.GetOrdinal(helper.CosMar91);
                    if (!dr.IsDBNull(iCosMar91)) entity.CosMar91 = dr.GetDecimal(iCosMar91);

                    int iCosMar92 = dr.GetOrdinal(helper.CosMar92);
                    if (!dr.IsDBNull(iCosMar92)) entity.CosMar92 = dr.GetDecimal(iCosMar92);

                    int iCosMar93 = dr.GetOrdinal(helper.CosMar93);
                    if (!dr.IsDBNull(iCosMar93)) entity.CosMar93 = dr.GetDecimal(iCosMar93);

                    int iCosMar94 = dr.GetOrdinal(helper.CosMar94);
                    if (!dr.IsDBNull(iCosMar94)) entity.CosMar94 = dr.GetDecimal(iCosMar94);

                    int iCosMar95 = dr.GetOrdinal(helper.CosMar95);
                    if (!dr.IsDBNull(iCosMar95)) entity.CosMar95 = dr.GetDecimal(iCosMar95);

                    int iCosMar96 = dr.GetOrdinal(helper.CosMar96);
                    if (!dr.IsDBNull(iCosMar96)) entity.CosMar96 = dr.GetDecimal(iCosMar96);

                    int iCosMarPromedioDia = dr.GetOrdinal(helper.CosMarPromedioDia);
                    if (!dr.IsDBNull(iCosMarPromedioDia)) entity.CosMarTotalDia = dr.GetDecimal(iCosMarPromedioDia);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}
