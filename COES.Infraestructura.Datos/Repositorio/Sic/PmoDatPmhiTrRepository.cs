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
    public class PmoDatPmhiTrRepository : RepositoryBase, IPmoDatPmhiTrRepository
    {
        public PmoDatPmhiTrRepository(string strConn)
            : base(strConn)
        {
        }

        PmoDatPmhiTrHelper helper = new PmoDatPmhiTrHelper();

        public int Save(PmoDatPmhiTrDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PmPmhtCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PmPeriCodi);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.PmPmhtAnhio, DbType.Int32, entity.PmPmhtAnhio);
            dbProvider.AddInParameter(command, helper.PmPmhtTipo, DbType.String, entity.PmPmhtTipo);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp01, DbType.Decimal, entity.PmPmhtDisp01);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp02, DbType.Decimal, entity.PmPmhtDisp02);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp03, DbType.Decimal, entity.PmPmhtDisp03);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp04, DbType.Decimal, entity.PmPmhtDisp04);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp05, DbType.Decimal, entity.PmPmhtDisp05);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp06, DbType.Decimal, entity.PmPmhtDisp06);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp07, DbType.Decimal, entity.PmPmhtDisp07);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp08, DbType.Decimal, entity.PmPmhtDisp08);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp09, DbType.Decimal, entity.PmPmhtDisp09);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp10, DbType.Decimal, entity.PmPmhtDisp10);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp11, DbType.Decimal, entity.PmPmhtDisp11);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp12, DbType.Decimal, entity.PmPmhtDisp12);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp13, DbType.Decimal, entity.PmPmhtDisp13);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp14, DbType.Decimal, entity.PmPmhtDisp14);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp15, DbType.Decimal, entity.PmPmhtDisp15);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp16, DbType.Decimal, entity.PmPmhtDisp16);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp17, DbType.Decimal, entity.PmPmhtDisp17);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp18, DbType.Decimal, entity.PmPmhtDisp18);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp19, DbType.Decimal, entity.PmPmhtDisp19);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp20, DbType.Decimal, entity.PmPmhtDisp20);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp21, DbType.Decimal, entity.PmPmhtDisp21);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp22, DbType.Decimal, entity.PmPmhtDisp22);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp23, DbType.Decimal, entity.PmPmhtDisp23);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp24, DbType.Decimal, entity.PmPmhtDisp24);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp25, DbType.Decimal, entity.PmPmhtDisp25);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp26, DbType.Decimal, entity.PmPmhtDisp26);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp27, DbType.Decimal, entity.PmPmhtDisp27);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp28, DbType.Decimal, entity.PmPmhtDisp28);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp29, DbType.Decimal, entity.PmPmhtDisp29);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp30, DbType.Decimal, entity.PmPmhtDisp30);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp31, DbType.Decimal, entity.PmPmhtDisp31);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp32, DbType.Decimal, entity.PmPmhtDisp32);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp33, DbType.Decimal, entity.PmPmhtDisp33);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp34, DbType.Decimal, entity.PmPmhtDisp34);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp35, DbType.Decimal, entity.PmPmhtDisp35);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp36, DbType.Decimal, entity.PmPmhtDisp36);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp37, DbType.Decimal, entity.PmPmhtDisp37);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp38, DbType.Decimal, entity.PmPmhtDisp38);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp39, DbType.Decimal, entity.PmPmhtDisp39);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp40, DbType.Decimal, entity.PmPmhtDisp40);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp41, DbType.Decimal, entity.PmPmhtDisp41);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp42, DbType.Decimal, entity.PmPmhtDisp42);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp43, DbType.Decimal, entity.PmPmhtDisp43);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp44, DbType.Decimal, entity.PmPmhtDisp44);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp45, DbType.Decimal, entity.PmPmhtDisp45);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp46, DbType.Decimal, entity.PmPmhtDisp46);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp47, DbType.Decimal, entity.PmPmhtDisp47);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp48, DbType.Decimal, entity.PmPmhtDisp48);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp49, DbType.Decimal, entity.PmPmhtDisp49);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp50, DbType.Decimal, entity.PmPmhtDisp50);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp51, DbType.Decimal, entity.PmPmhtDisp51);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp52, DbType.Decimal, entity.PmPmhtDisp52);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp53, DbType.Decimal, entity.PmPmhtDisp53);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public int Update(PmoDatPmhiTrDTO entity)
        {
            List<PmoDatPmhiTrDTO> entitys = new List<PmoDatPmhiTrDTO>();
            string queryString = string.Format(helper.SqlUpdate);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp01, DbType.Decimal, entity.PmPmhtDisp01);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp02, DbType.Decimal, entity.PmPmhtDisp02);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp03, DbType.Decimal, entity.PmPmhtDisp03);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp04, DbType.Decimal, entity.PmPmhtDisp04);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp05, DbType.Decimal, entity.PmPmhtDisp05);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp06, DbType.Decimal, entity.PmPmhtDisp06);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp07, DbType.Decimal, entity.PmPmhtDisp07);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp08, DbType.Decimal, entity.PmPmhtDisp08);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp09, DbType.Decimal, entity.PmPmhtDisp09);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp10, DbType.Decimal, entity.PmPmhtDisp10);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp11, DbType.Decimal, entity.PmPmhtDisp11);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp12, DbType.Decimal, entity.PmPmhtDisp12);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp13, DbType.Decimal, entity.PmPmhtDisp13);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp14, DbType.Decimal, entity.PmPmhtDisp14);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp15, DbType.Decimal, entity.PmPmhtDisp15);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp16, DbType.Decimal, entity.PmPmhtDisp16);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp17, DbType.Decimal, entity.PmPmhtDisp17);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp18, DbType.Decimal, entity.PmPmhtDisp18);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp19, DbType.Decimal, entity.PmPmhtDisp19);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp20, DbType.Decimal, entity.PmPmhtDisp20);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp21, DbType.Decimal, entity.PmPmhtDisp21);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp22, DbType.Decimal, entity.PmPmhtDisp22);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp23, DbType.Decimal, entity.PmPmhtDisp23);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp24, DbType.Decimal, entity.PmPmhtDisp24);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp25, DbType.Decimal, entity.PmPmhtDisp25);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp26, DbType.Decimal, entity.PmPmhtDisp26);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp27, DbType.Decimal, entity.PmPmhtDisp27);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp28, DbType.Decimal, entity.PmPmhtDisp28);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp29, DbType.Decimal, entity.PmPmhtDisp29);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp30, DbType.Decimal, entity.PmPmhtDisp30);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp31, DbType.Decimal, entity.PmPmhtDisp31);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp32, DbType.Decimal, entity.PmPmhtDisp32);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp33, DbType.Decimal, entity.PmPmhtDisp33);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp34, DbType.Decimal, entity.PmPmhtDisp34);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp35, DbType.Decimal, entity.PmPmhtDisp35);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp36, DbType.Decimal, entity.PmPmhtDisp36);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp37, DbType.Decimal, entity.PmPmhtDisp37);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp38, DbType.Decimal, entity.PmPmhtDisp38);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp39, DbType.Decimal, entity.PmPmhtDisp39);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp40, DbType.Decimal, entity.PmPmhtDisp40);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp41, DbType.Decimal, entity.PmPmhtDisp41);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp42, DbType.Decimal, entity.PmPmhtDisp42);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp43, DbType.Decimal, entity.PmPmhtDisp43);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp44, DbType.Decimal, entity.PmPmhtDisp44);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp45, DbType.Decimal, entity.PmPmhtDisp45);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp46, DbType.Decimal, entity.PmPmhtDisp46);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp47, DbType.Decimal, entity.PmPmhtDisp47);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp48, DbType.Decimal, entity.PmPmhtDisp48);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp49, DbType.Decimal, entity.PmPmhtDisp49);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp50, DbType.Decimal, entity.PmPmhtDisp50);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp51, DbType.Decimal, entity.PmPmhtDisp51);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp52, DbType.Decimal, entity.PmPmhtDisp52);
            dbProvider.AddInParameter(command, helper.PmPmhtDisp53, DbType.Decimal, entity.PmPmhtDisp53);

            dbProvider.AddInParameter(command, helper.PmPmhtCodi, DbType.Int32, entity.PmPmhtCodi);
            var id = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<PmoDatPmhiTrDTO> ListDatPmhiTr(int codigoPeriodo, string tipo)
        {
            List<PmoDatPmhiTrDTO> entitys = new List<PmoDatPmhiTrDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, codigoPeriodo);
            dbProvider.AddInParameter(command, helper.PmPmhtTipo, DbType.String, tipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatPmhiTrDTO entity = helper.Create(dr);

                    int iSddpnum = dr.GetOrdinal(helper.Sddpnum);
                    if (!dr.IsDBNull(iSddpnum)) entity.Sddpnum = Convert.ToInt32(dr.GetValue(iSddpnum));

                    int iSddpnomb = dr.GetOrdinal(helper.Sddpnomb);
                    if (!dr.IsDBNull(iSddpnomb)) entity.Sddpnomb = dr.GetString(iSddpnomb);

                    int iPlanta = dr.GetOrdinal(helper.Planta);
                    if (!dr.IsDBNull(iPlanta)) entity.Planta = dr.GetString(iPlanta);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CountDatPmhiTr(int periCodi, string tipo)
        {
            string queryString = string.Format(helper.SqlGetCount);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int Cantidad = 0;
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);
            dbProvider.AddInParameter(command, helper.PmPmhtTipo, DbType.String, tipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCant = dr.GetOrdinal(helper.Cant);
                    if (!dr.IsDBNull(iCant)) Cantidad = dr.GetInt32(iCant);
                }
            }

            return Cantidad;
        }

    }
}
