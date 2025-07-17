var controlador = siteRoot + 'hidrologia/envio/';
var ENVIO_EN_PLAZO = "P";
var ENVIO_FUERA_PLAZO = "F";
var ENVIO_PLAZO_DESHABILITADO = "D";
var HABILITAR_EDITAR_CARGA_DATOS = true;
var VER_ULTIMO_ENVIO = false;
var VER_ENVIO = false;
var listFormatCodi = [];
var listFormatPeriodo = [];
var listFormatDescrip = [];
var listaPtos = null;
var uploader;
var hot;
var hotOptions;
var evtHot;
var grillaBD;
var editable = true;
var nFilasFor2 = 0;
var periodo = {
    anho: 2015,
    semana: 1
};
var maxCadena = 255;
var formato;
var tipo1 = "tipo1";
var tipo2 = "tipo2";
var validaInicial = true;
var listErrores = [];
var errorNoNumero = 2;
var errorLimInferior = 3;
var errorLimSuperior = 4;
var errorRangoFecha = 5;
var errorCrucePeriodo = 6;
var formatoVertim = 41;
var formatoDescarg = 42;
var errores = [
    {
        tipo: 'BLANCO',
        Descripcion: 'BLANCO',
        total: 0,
        codigo: 0,
        Background_color: '',
        Color: ''
    },
    {
        tipo: 'NUMERO',
        Descripcion: 'NÚMERO',
        total: 0,
        codigo: 1,
        Background_color: 'white',
        Color: ''
    },
    {
        tipo: 'NONUMERO',
        Descripcion: 'NO NÚMERO',
        total: 0,
        codigo: 2,
        BackgroundColor: 'red',
        Color: ''
    },
    {
        tipo: 'LIMINF',
        Descripcion: "LIM. INFERIOR",
        total: 0,
        codigo: 3,
        BackgroundColor: 'orange',
        Color: ''
    },
    {
        tipo: 'LIMSUP',
        Descripcion: 'LIMITE SUPERIOR',
        total: 0,
        codigo: 4,
        BackgroundColor: 'yellow',
        Color: ''
    },
    {
        tipo: 'RANFEC',
        Descripcion: 'RANGO DE FECHA INVALIDO',
        total: 0,
        codigo: 5,
        BackgroundColor: '#FF4C42',
        Color: 'white'
    },
    {
        tipo: 'CRUPER',
        Descripcion: 'CRUCE EN PERIODOS',
        total: 0,
        codigo: 6,
        BackgroundColor: 'Wheat',
        Color: 'black'
    },

];
