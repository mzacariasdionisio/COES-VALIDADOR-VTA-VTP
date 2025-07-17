function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

function getFormattedDate(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

function number_format(number, decimal_pos, decimal_sep, thousand_sep) {
    //number = parseFloat(numberstr);
    var ts = (thousand_sep == null ? ',' : thousand_sep)
        , ds = (decimal_sep == null ? '.' : decimal_sep)
        , dp = (decimal_pos == null ? 2 : decimal_pos)

        , n = Math.abs(Math.ceil(number)).toString()

        , i = n.length % 3
        , f = n.substr(0, i)
        ;

    if (number < 0) f = '-' + f;

    for (; i < n.length; i += 3) {
        if (i != 0) f += ts;
        f += n.substr(i, 3);
    }

    if (dp > 0)
        f += ds + parseFloat(number).toFixed(dp).split('.')[1]

    return f;
}


function formatFloat(num, casasDec, sepDecimal, sepMilhar) {
    if (num == 0) {
        var cerosDer = '';
        while (cerosDer.length < casasDec)
            cerosDer = '0' + cerosDer;

        return "0" + sepDecimal + cerosDer;
    }

    if (num < 0) {
        num = -num;
        sinal = -1;
    } else
        sinal = 1;
    var resposta = "";
    var part = "";
    if (num != Math.floor(num)) // decimal values present
    {
        part = Math.round((num - Math.floor(num)) * Math.pow(10, casasDec)).toString(); // transforms decimal part into integer (rounded)
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
            num = Math.floor(num);
        } else
            num = Math.round(num);
    } // end of decimal part
    else {
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
        }
    }

    var numBefWhile = num; //A veces no aparece el número cero (0) antes del punto decimal
    while (num > 0) // integer part
    {
        part = (num - Math.floor(num / 1000) * 1000).toString(); // part = three less significant digits
        num = Math.floor(num / 1000);
        if (num > 0)
            while (part.length < 3) // 123.023.123  if sepMilhar = '.'
                part = '0' + part; // 023
        resposta = part + resposta;
        if (num > 0)
            resposta = sepMilhar + resposta;
    }
    if (sinal < 0)
        resposta = '-' + ((numBefWhile == 0) ? '0' : '') + resposta;
    else {
        if (numBefWhile == 0)
            resposta = '0' + resposta;
    }
    return resposta;
}

function getFechaFromMes(fecha, mes) {
    if (mes.length == 7) {
        return "01/" + mes.substr(0, 2) + "/" + mes.substr(3, 4);
    }

    return fecha;
}