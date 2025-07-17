using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IMeEnvioDetMensajeRepository
    {
        int GetMaxId();
        void SaveEnvioDetMensaje(MeEnvioDetMensajeDTO entity, IDbConnection conn, DbTransaction tran);

        // ------------------------------------ 02-04-2019 -------------------------------------------
        // Funcion para eliminar fisicamente el registro de tabla ME_ENVIODETMENSAJE
        // -------------------------------------------------------------------------------------------
        void EliminarEnvDetMsgXEnvDetCodi(int envdetcodi, IDbConnection conn, DbTransaction tran);
        // -------------------------------------------------------------------------------------------    
    }
}
