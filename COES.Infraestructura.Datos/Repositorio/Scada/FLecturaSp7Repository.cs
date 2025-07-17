using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla F_LECTURA_SP7
    /// </summary>
    public class FLecturaSp7Repository: RepositoryBase, IFLecturaSp7Repository
    {
        public FLecturaSp7Repository(string strConn): base(strConn)
        {
        }

        FLecturaSp7Helper helper = new FLecturaSp7Helper();

        public void Save(FLecturaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, entity.Fechahora);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);
            dbProvider.AddInParameter(command, helper.Vsf, DbType.Decimal, entity.Vsf);
            dbProvider.AddInParameter(command, helper.Maximo, DbType.Decimal, entity.Maximo);
            dbProvider.AddInParameter(command, helper.Minimo, DbType.Decimal, entity.Minimo);
            dbProvider.AddInParameter(command, helper.Voltaje, DbType.Decimal, entity.Voltaje);
            dbProvider.AddInParameter(command, helper.Num, DbType.Int32, entity.Num);
            dbProvider.AddInParameter(command, helper.Desv, DbType.Decimal, entity.Desv);
            dbProvider.AddInParameter(command, helper.H0, DbType.Decimal, entity.H0);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.Devsec, DbType.Decimal, entity.Devsec);
            dbProvider.AddInParameter(command, helper.Frecspusucreacion, DbType.String, entity.Frecspusucreacion);
            dbProvider.AddInParameter(command, helper.Frecspfeccreacion, DbType.DateTime, entity.Frecspfeccreacion);
            dbProvider.AddInParameter(command, helper.Frecspusumodificacion, DbType.String, entity.Frecspusumodificacion);
            dbProvider.AddInParameter(command, helper.Frecspfecmodificacion, DbType.DateTime, entity.Frecspfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(FLecturaSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vsf, DbType.Decimal, entity.Vsf);
            dbProvider.AddInParameter(command, helper.Maximo, DbType.Decimal, entity.Maximo);
            dbProvider.AddInParameter(command, helper.Minimo, DbType.Decimal, entity.Minimo);
            dbProvider.AddInParameter(command, helper.Voltaje, DbType.Decimal, entity.Voltaje);
            dbProvider.AddInParameter(command, helper.Num, DbType.Int32, entity.Num);
            dbProvider.AddInParameter(command, helper.Desv, DbType.Decimal, entity.Desv);
            dbProvider.AddInParameter(command, helper.H0, DbType.Decimal, entity.H0);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.Devsec, DbType.Decimal, entity.Devsec);
            dbProvider.AddInParameter(command, helper.Frecspusucreacion, DbType.String, entity.Frecspusucreacion);
            dbProvider.AddInParameter(command, helper.Frecspfeccreacion, DbType.DateTime, entity.Frecspfeccreacion);
            dbProvider.AddInParameter(command, helper.Frecspusumodificacion, DbType.String, entity.Frecspusumodificacion);
            dbProvider.AddInParameter(command, helper.Frecspfecmodificacion, DbType.DateTime, entity.Frecspfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, entity.Fechahora);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fechahora, int gpscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, fechahora);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, gpscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FLecturaSp7DTO GetById(DateTime fechahora, int gpscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fechahora, DbType.DateTime, fechahora);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, gpscodi);
            FLecturaSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FLecturaSp7DTO> List()
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
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

        public List<FLecturaSp7DTO> GetByCriteria()
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
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


        public List<FLecturaSp7DTO> BuscarOperaciones(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListadoGrafico, gpsCodi,
                fechaHoraIni.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaHoraFin.ToString(ConstantesBase.FormatoFechaExtendido)
                );
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime FechahoraBase = Convert.ToDateTime("2000-01-01");

                    FLecturaSp7DTO entity = new FLecturaSp7DTO();

                    int iFechahora = dr.GetOrdinal(this.helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) FechahoraBase = dr.GetDateTime(iFechahora);

                    //int iGpscodi = dr.GetOrdinal(this.helper.Gpscodi);
                    //if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));
                     
                    int iH0 = dr.GetOrdinal(this.helper.H0);
                    if (!dr.IsDBNull(iH0)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(0), H0 = dr.GetDecimal(iH0) });
                    
                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(1), H0 = dr.GetDecimal(iH1) });
                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(2), H0 = dr.GetDecimal(iH2) });
                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(3), H0 = dr.GetDecimal(iH3) });
                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(4), H0 = dr.GetDecimal(iH4) });
                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(5), H0 = dr.GetDecimal(iH5) });
                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(6), H0 = dr.GetDecimal(iH6) });
                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(7), H0 = dr.GetDecimal(iH7) });
                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(8), H0 = dr.GetDecimal(iH8) });
                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(9), H0 = dr.GetDecimal(iH9) });
                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(10), H0 = dr.GetDecimal(iH10) });
                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(11), H0 = dr.GetDecimal(iH11) });
                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(12), H0 = dr.GetDecimal(iH12) });
                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(13), H0 = dr.GetDecimal(iH13) });
                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(14), H0 = dr.GetDecimal(iH14) });
                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(15), H0 = dr.GetDecimal(iH15) });
                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(16), H0 = dr.GetDecimal(iH16) });
                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(17), H0 = dr.GetDecimal(iH17) });
                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(18), H0 = dr.GetDecimal(iH18) });
                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(19), H0 = dr.GetDecimal(iH19) });
                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(20), H0 = dr.GetDecimal(iH20) });
                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(21), H0 = dr.GetDecimal(iH21) });
                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(22), H0 = dr.GetDecimal(iH22) });
                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(23), H0 = dr.GetDecimal(iH23) });
                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(24), H0 = dr.GetDecimal(iH24) });
                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(25), H0 = dr.GetDecimal(iH25) });
                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(26), H0 = dr.GetDecimal(iH26) });
                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(27), H0 = dr.GetDecimal(iH27) });
                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(28), H0 = dr.GetDecimal(iH28) });
                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(29), H0 = dr.GetDecimal(iH29) });
                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(30), H0 = dr.GetDecimal(iH30) });
                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(31), H0 = dr.GetDecimal(iH31) });
                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(32), H0 = dr.GetDecimal(iH32) });
                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(33), H0 = dr.GetDecimal(iH33) });
                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(34), H0 = dr.GetDecimal(iH34) });
                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(35), H0 = dr.GetDecimal(iH35) });
                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(36), H0 = dr.GetDecimal(iH36) });
                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(37), H0 = dr.GetDecimal(iH37) });
                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(38), H0 = dr.GetDecimal(iH38) });
                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(39), H0 = dr.GetDecimal(iH39) });
                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(40), H0 = dr.GetDecimal(iH40) });
                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(41), H0 = dr.GetDecimal(iH41) });
                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(42), H0 = dr.GetDecimal(iH42) });
                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(43), H0 = dr.GetDecimal(iH43) });
                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(44), H0 = dr.GetDecimal(iH44) });
                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(45), H0 = dr.GetDecimal(iH45) });
                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(46), H0 = dr.GetDecimal(iH46) });
                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(47), H0 = dr.GetDecimal(iH47) });
                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(48), H0 = dr.GetDecimal(iH48) });
                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(49), H0 = dr.GetDecimal(iH49) });
                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(50), H0 = dr.GetDecimal(iH50) });
                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(51), H0 = dr.GetDecimal(iH51) });
                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(52), H0 = dr.GetDecimal(iH52) });
                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(53), H0 = dr.GetDecimal(iH53) });
                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(54), H0 = dr.GetDecimal(iH54) });
                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(55), H0 = dr.GetDecimal(iH55) });
                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(56), H0 = dr.GetDecimal(iH56) });
                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(57), H0 = dr.GetDecimal(iH57) });
                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(58), H0 = dr.GetDecimal(iH58) });
                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(59), H0 = dr.GetDecimal(iH59) });

                    /*

                    int iH1 = dr.GetOrdinal(this.helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(this.helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(this.helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(this.helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(this.helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(this.helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(this.helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(this.helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(this.helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(this.helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(this.helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(this.helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(this.helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(this.helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(this.helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(this.helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(this.helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(this.helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(this.helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(this.helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(this.helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(this.helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(this.helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(this.helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(this.helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(this.helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(this.helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(this.helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(this.helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(this.helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(this.helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(this.helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(this.helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(this.helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(this.helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(this.helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(this.helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(this.helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(this.helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(this.helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(this.helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(this.helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(this.helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(this.helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(this.helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(this.helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(this.helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(this.helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    int iH49 = dr.GetOrdinal(this.helper.H49);
                    if (!dr.IsDBNull(iH49)) entity.H49 = dr.GetDecimal(iH49);

                    int iH50 = dr.GetOrdinal(this.helper.H50);
                    if (!dr.IsDBNull(iH50)) entity.H50 = dr.GetDecimal(iH50);

                    int iH51 = dr.GetOrdinal(this.helper.H51);
                    if (!dr.IsDBNull(iH51)) entity.H51 = dr.GetDecimal(iH51);

                    int iH52 = dr.GetOrdinal(this.helper.H52);
                    if (!dr.IsDBNull(iH52)) entity.H52 = dr.GetDecimal(iH52);

                    int iH53 = dr.GetOrdinal(this.helper.H53);
                    if (!dr.IsDBNull(iH53)) entity.H53 = dr.GetDecimal(iH53);

                    int iH54 = dr.GetOrdinal(this.helper.H54);
                    if (!dr.IsDBNull(iH54)) entity.H54 = dr.GetDecimal(iH54);

                    int iH55 = dr.GetOrdinal(this.helper.H55);
                    if (!dr.IsDBNull(iH55)) entity.H55 = dr.GetDecimal(iH55);

                    int iH56 = dr.GetOrdinal(this.helper.H56);
                    if (!dr.IsDBNull(iH56)) entity.H56 = dr.GetDecimal(iH56);

                    int iH57 = dr.GetOrdinal(this.helper.H57);
                    if (!dr.IsDBNull(iH57)) entity.H57 = dr.GetDecimal(iH57);

                    int iH58 = dr.GetOrdinal(this.helper.H58);
                    if (!dr.IsDBNull(iH58)) entity.H58 = dr.GetDecimal(iH58);

                    int iH59 = dr.GetOrdinal(this.helper.H59);
                    if (!dr.IsDBNull(iH59)) entity.H59 = dr.GetDecimal(iH59);


                    entitys.Add(entity);*/
                }
            }

            return entitys;
        }


        public List<FLecturaSp7DTO> ObtenerMaximaFrecuencia(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
            String sql = String.Format(this.helper.MaximaFrecuencia, gpsCodi,
                fechaHoraIni.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaHoraFin.ToString(ConstantesBase.FormatoFechaExtendido)
                );
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime FechahoraBase = Convert.ToDateTime("2000-01-01");
                    
                    FLecturaSp7DTO entity = new FLecturaSp7DTO();

                    int iFechahora = dr.GetOrdinal(this.helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) FechahoraBase = dr.GetDateTime(iFechahora);

                    decimal Maximo = 60;
                    int iMaximo = dr.GetOrdinal(this.helper.Maximo);
                    if (!dr.IsDBNull(iMaximo)) { Maximo = dr.GetDecimal(iMaximo); };


                    var registro = dr;

                    for (var j = 0; j <= 59; j++)
                    {
                        decimal? valor = 0;
                        //valor = (decimal?)registro.GetType().GetProperty("H" + (j).ToString()).GetValue(registro, null);
                        valor = dr.GetDecimal(dr.GetOrdinal("H"+j));
                        
                        

                        if (valor == Maximo)
                        {
                            entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(j), H0 = valor });

                        }
                    }


                }
            }

            return entitys;
        }


        public List<FLecturaSp7DTO> ObtenerMinimaFrecuencia(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
            String sql = String.Format(this.helper.MinimaFrecuencia, gpsCodi,
                fechaHoraIni.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaHoraFin.ToString(ConstantesBase.FormatoFechaExtendido)
                );
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime FechahoraBase = Convert.ToDateTime("2000-01-01");

                    FLecturaSp7DTO entity = new FLecturaSp7DTO();

                    int iFechahora = dr.GetOrdinal(this.helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) FechahoraBase = dr.GetDateTime(iFechahora);

                    decimal Minimo = 60;
                    int iMinimo = dr.GetOrdinal(this.helper.Minimo);
                    if (!dr.IsDBNull(iMinimo)) { Minimo = dr.GetDecimal(iMinimo); };


                    var registro = dr;

                    for (var j = 0; j <= 59; j++)
                    {
                        decimal? valor = 0;
                        //valor = (decimal?)registro.GetType().GetProperty("H" + (j).ToString()).GetValue(registro, null);
                        valor = dr.GetDecimal(dr.GetOrdinal("H" + j));



                        if (valor == Minimo)
                        {
                            entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(j), H0 = valor });

                        }
                    }


                }
            }

            return entitys;
        }


        public List<FLecturaSp7DTO> ObtenerSubitaFrecuencia(int gpsCodi, string transgresiones, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
            String sql = String.Format(this.helper.SubitaFrecuencia, gpsCodi,
                fechaHoraIni.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaHoraFin.ToString(ConstantesBase.FormatoFechaExtendido),
                transgresiones
                );
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime FechahoraBase = Convert.ToDateTime("2000-01-01");

                    FLecturaSp7DTO entity = new FLecturaSp7DTO();

                    int iFechahora = dr.GetOrdinal(this.helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) FechahoraBase = dr.GetDateTime(iFechahora);

                    //decimal Minimo = 60;
                    //int iMinimo = dr.GetOrdinal(this.helper.Minimo);
                    //if (!dr.IsDBNull(iMinimo)) { Minimo = dr.GetDecimal(iMinimo); };


                    //int iH0 = dr.GetOrdinal(this.helper.H0);
                    //if (!dr.IsDBNull(iH0)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(0), H0 = dr.GetDecimal(iH0) });

                    int iVsf = dr.GetOrdinal(this.helper.Vsf);
                    if (!dr.IsDBNull(iVsf)) entitys.Add( new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(59), H0 = dr.GetDecimal(iVsf) });



                }
            }

            return entitys;
        }




        public List<FLecturaSp7DTO> ObtenerSostenidaFrecuencia(int gpsCodi, string transgresiones, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            int Multiplo=15;

            //calculando fechahora inicial y final múltiplo de 15 minutos
            ConfiguraHora(ref fechaHoraIni,  Multiplo);
            ConfiguraHora(ref fechaHoraFin, Multiplo);

            fechaHoraFin = fechaHoraFin.AddMinutes(Multiplo);


            bool PrimerRegistro = false;
            decimal SumDesv = 0;
            decimal SumNum = 0;
            int Conteo = 0;
            
            DateTime FechaHoraRangoIni = Convert.ToDateTime("2000-01-01");


            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
            String sql = String.Format(this.helper.TramaFrecuencia, gpsCodi,
                fechaHoraIni.ToString(ConstantesBase.FormatoFechaExtendido),
                fechaHoraFin.ToString(ConstantesBase.FormatoFechaExtendido),
                transgresiones
                );
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DateTime FechahoraBase = Convert.ToDateTime("2000-01-01");
                    Decimal Desv = 0;
                    Decimal Num = 0;


                    FLecturaSp7DTO entity = new FLecturaSp7DTO();

                    int iFechahora = dr.GetOrdinal(this.helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) FechahoraBase = dr.GetDateTime(iFechahora);

                    int iDesv = dr.GetOrdinal(this.helper.Desv);
                    if (!dr.IsDBNull(iDesv)) Desv = dr.GetDecimal(iDesv);

                    int iNum = dr.GetOrdinal(this.helper.Num);
                    if (!dr.IsDBNull(iNum)) Num = dr.GetDecimal(iNum);


                    if (!PrimerRegistro)
                    {
                        ConfiguraHora(ref FechahoraBase, Multiplo);
                        FechaHoraRangoIni = FechahoraBase;
                        PrimerRegistro = true;
                        SumDesv = Desv;
                        SumNum = Num;
                        Conteo=1;
                    }
                    else
                    {
                        if (FechaHoraRangoIni <= FechahoraBase && FechahoraBase < FechaHoraRangoIni.AddMinutes(Multiplo))
                        {
                            SumDesv += Desv;
                            SumNum += Num;
                            Conteo++;
                        }
                        else
                        {
                            if (SumNum != 0)
                            {
                                decimal Factor = SumDesv / SumNum * 100;

                                if(
                                    (transgresiones == "S" && Math.Abs(Factor)> (decimal)0.600000)
                                    ||
                                    (transgresiones == "T")
                                    )
                                //escribe dato calculado
                                entitys.Add(new FLecturaSp7DTO
                                {
                                    Fechahora = FechaHoraRangoIni.AddMinutes(Multiplo).AddSeconds(-1),
                                    H0 = Factor
                                });

                            }

                            //halla nuevo rango y almacena nuevo dato
                            ConfiguraHora(ref FechahoraBase, Multiplo);
                            FechaHoraRangoIni = FechahoraBase;                            
                            SumDesv = Desv;
                            SumNum = Num;
                            Conteo = 1;

                        }


                    }


                    //decimal Minimo = 60;
                    //int iMinimo = dr.GetOrdinal(this.helper.Minimo);
                    //if (!dr.IsDBNull(iMinimo)) { Minimo = dr.GetDecimal(iMinimo); };


                    //int iH0 = dr.GetOrdinal(this.helper.H0);
                    //if (!dr.IsDBNull(iH0)) entitys.Add(new FLecturaSp7DTO { Fechahora = FechahoraBase.AddSeconds(0), H0 = dr.GetDecimal(iH0) });

                   


                }

                //escribe ultimo tramo
                if (SumNum != 0)
                {
                    decimal Factor = SumDesv / SumNum * 100;

                    if (
                        (transgresiones == "S" && Math.Abs(Factor) > (decimal)0.600000)
                        ||
                        (transgresiones == "T")
                        )
                        //escribe dato calculado
                        entitys.Add(new FLecturaSp7DTO
                        {
                            Fechahora = FechaHoraRangoIni.AddMinutes(Multiplo).AddSeconds(-1),
                            H0 = Factor
                        });

                }


            }

            

            return entitys;
        }




        public int ObtenerNroFilas(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin)
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();
            String sql = String.Format(this.helper.TotalRegistros, gpsCodi, fechaHoraIni.ToString(ConstantesBase.FormatoFechaExtendido), fechaHoraFin.ToString(ConstantesBase.FormatoFechaExtendido));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }


        private void ConfiguraHora(ref DateTime FechaIni, int Multiplo)
        {

            DateTime FechaHora = FechaIni;

            int Minuto = FechaHora.Minute - FechaHora.Minute % Multiplo;

            TimeSpan Hora = new TimeSpan(FechaHora.Hour, Minuto, 0);

            FechaHora = FechaHora.Date + Hora;
            FechaIni = FechaHora;


        }

        #region FIT - SGOCOES func A - Web Service Add In
        public List<FLecturaSp7DTO> ObtenerConsultaTablaFrecuencia(bool zeroH, DateTime fechaInicio, DateTime fechaFin, int gpscodi)
        {
            List<FLecturaSp7DTO> entitys = new List<FLecturaSp7DTO>();

            String query = String.Format((zeroH ? helper.SqlGetFromTablaFrecuencia : helper.SqlGetFromTablaFrecuenciaWithZeroH),
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                gpscodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);                        

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    FLecturaSp7DTO entity = new FLecturaSp7DTO();

                    int iFechahora = dr.GetOrdinal(helper.Fechahora);
                    if (!dr.IsDBNull(iFechahora)) entity.Fechahora = dr.GetDateTime(iFechahora);

                    int iDevsec = dr.GetOrdinal(helper.Devsec);
                    if (!dr.IsDBNull(iDevsec)) entity.Devsec = dr.GetDecimal(iDevsec);

                    int iH0 = dr.GetOrdinal(helper.H0);
                    if (!dr.IsDBNull(iH0)) entity.H0 = dr.GetDecimal(iH0);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
