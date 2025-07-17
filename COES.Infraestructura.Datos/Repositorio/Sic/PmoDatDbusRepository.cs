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
    public class PmoDatDbusRepository : RepositoryBase, IPmoDatDbusRepository
    {
        public PmoDatDbusRepository(string strConn)
            : base(strConn)
        {
        }


        PmoDatDbusHelper helper = new PmoDatDbusHelper();

        public List<PmoDatDbusDTO> ListDbus()
        {
            List<PmoDatDbusDTO> entitys = new List<PmoDatDbusDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbusDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmoDatDbusDTO> ListDatDbus()
        {
            List<PmoDatDbusDTO> entitys = new List<PmoDatDbusDTO>();
            string queryString = string.Format(helper.SqlGetDat);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatDbusDTO entity = new PmoDatDbusDTO();
                    int iNum = dr.GetOrdinal(helper.Num);
                    if (!dr.IsDBNull(iNum)) entity.Num = dr.GetString(iNum);

                    int iTp = dr.GetOrdinal(helper.Tp);
                    if (!dr.IsDBNull(iTp)) entity.Tp = dr.GetString(iTp);

                    int iNombreB = dr.GetOrdinal(helper.NombreB);
                    if (!dr.IsDBNull(iNombreB)) entity.NombreB = dr.GetString(iNombreB);

                    int iId = dr.GetOrdinal(helper.Id);
                    if (!dr.IsDBNull(iId)) entity.Id = dr.GetString(iId);

                    int iNumeral = dr.GetOrdinal(helper.Numeral);
                    if (!dr.IsDBNull(iNumeral)) entity.Numeral = dr.GetString(iNumeral);

                    int iTg = dr.GetOrdinal(helper.Tg);
                    if (!dr.IsDBNull(iTg)) entity.Tg = dr.GetInt32(iTg);

                    int iPlnt = dr.GetOrdinal(helper.Plnt);
                    if (!dr.IsDBNull(iPlnt)) entity.Plnt = dr.GetString(iPlnt);

                    int iNombreGener = dr.GetOrdinal(helper.NombreGener);
                    if (!dr.IsDBNull(iNombreGener)) entity.NombreGener = dr.GetString(iNombreGener);

                    int iArea = dr.GetOrdinal(helper.Area);
                    if (!dr.IsDBNull(iArea)) entity.Area = dr.GetString(iArea);

                    int iPer1 = dr.GetOrdinal(helper.Per1);
                    if (!dr.IsDBNull(iPer1)) entity.Per1 = dr.GetString(iPer1);

                    int iPloa1 = dr.GetOrdinal(helper.Ploa1);
                    if (!dr.IsDBNull(iPloa1)) entity.Ploa1 = dr.GetString(iPloa1);

                    int iPind1 = dr.GetOrdinal(helper.Pind1);
                    if (!dr.IsDBNull(iPind1)) entity.Pind1 = dr.GetString(iPind1);

                    int iPerF1 = dr.GetOrdinal(helper.PerF1);
                    if (!dr.IsDBNull(iPerF1)) entity.PerF1 = dr.GetString(iPerF1);

                    int iPer2 = dr.GetOrdinal(helper.Per2);
                    if (!dr.IsDBNull(iPer2)) entity.Per2 = dr.GetString(iPer2);

                    int iPloa2 = dr.GetOrdinal(helper.Ploa2);
                    if (!dr.IsDBNull(iPloa2)) entity.Ploa2 = dr.GetString(iPloa2);

                    int iPind2 = dr.GetOrdinal(helper.Pind2);
                    if (!dr.IsDBNull(iPind2)) entity.Pind2 = dr.GetString(iPind2);

                    int iPerF2 = dr.GetOrdinal(helper.PerF2);
                    if (!dr.IsDBNull(iPerF2)) entity.PerF2 = dr.GetString(iPerF2);

                    int iPer3 = dr.GetOrdinal(helper.Per3);
                    if (!dr.IsDBNull(iPer3)) entity.Per3 = dr.GetString(iPer3);

                    int iPloa3 = dr.GetOrdinal(helper.Ploa3);
                    if (!dr.IsDBNull(iPloa3)) entity.Ploa3 = dr.GetString(iPloa3);

                    int iPind3 = dr.GetOrdinal(helper.Pind3);
                    if (!dr.IsDBNull(iPind3)) entity.Pind3 = dr.GetString(iPind3);

                    int iPerF3 = dr.GetOrdinal(helper.PerF3);
                    if (!dr.IsDBNull(iPerF3)) entity.PerF3 = dr.GetString(iPerF3);

                    int iPer4 = dr.GetOrdinal(helper.Per4);
                    if (!dr.IsDBNull(iPer4)) entity.Per4 = dr.GetString(iPer4);

                    int iPloa4 = dr.GetOrdinal(helper.Ploa4);
                    if (!dr.IsDBNull(iPloa4)) entity.Ploa4 = dr.GetString(iPloa4);

                    int iPind4 = dr.GetOrdinal(helper.Pind4);
                    if (!dr.IsDBNull(iPind4)) entity.Pind4 = dr.GetString(iPind4);

                    int iPerF4 = dr.GetOrdinal(helper.PerF4);
                    if (!dr.IsDBNull(iPerF4)) entity.PerF4 = dr.GetString(iPerF4);

                    int iPer5 = dr.GetOrdinal(helper.Per5);
                    if (!dr.IsDBNull(iPer5)) entity.Per5 = dr.GetString(iPer5);

                    int iPloa5 = dr.GetOrdinal(helper.Ploa5);
                    if (!dr.IsDBNull(iPloa5)) entity.Ploa5 = dr.GetString(iPloa5); //20190308 - NET: Adecuaciones a los archivos .DAT

                    int iPind5 = dr.GetOrdinal(helper.Pind5);
                    if (!dr.IsDBNull(iPind5)) entity.Pind5 = dr.GetString(iPind5);

                    int iPerF5 = dr.GetOrdinal(helper.PerF5);
                    if (!dr.IsDBNull(iPerF5)) entity.PerF5 = dr.GetString(iPerF5);

                    int iIcca = dr.GetOrdinal(helper.Icca);
                    if (!dr.IsDBNull(iIcca)) entity.Icca = dr.GetString(iIcca);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CountDatDbus(int periCodi)
        {
            string queryString = string.Format(helper.SqlGetCount);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int Cantidad = 0;

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

        public void GenerateDat()
        {
            string queryString = string.Format(helper.SqlGenerateDat);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            var result = dbProvider.ExecuteNonQuery(command);
        }
    }
}
