(function (cjs, an) {

    var p; // shortcut to reference prototypes
    var lib = {}; var ss = {}; var img = {};
    lib.ssMetadata = [
        { name: "InfografiaAnimadaAnimatefinalv3_atlas_", frames: [[0, 3262, 1338, 780], [1340, 3262, 1338, 780], [2680, 3262, 1338, 780], [5360, 3101, 1007, 771], [4883, 782, 1007, 771], [0, 0, 4881, 3260], [4883, 2328, 1007, 771], [4883, 0, 1338, 780], [4020, 3262, 1338, 780], [4883, 1555, 1007, 771]] }
    ];


    // symbols:



    (lib.CostosMarginales = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(0);
    }).prototype = p = new cjs.Sprite();



    (lib.fallasportipodeequipo = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(1);
    }).prototype = p = new cjs.Sprite();



    (lib.hidroelectrica = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(2);
    }).prototype = p = new cjs.Sprite();



    (lib.LineasdeTransmision = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(3);
    }).prototype = p = new cjs.Sprite();



    (lib.MaximaDemanda = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(4);
    }).prototype = p = new cjs.Sprite();



    (lib.ojala2 = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(5);
    }).prototype = p = new cjs.Sprite();



    (lib.participaciondeusuarioslibresdedemanda = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(6);
    }).prototype = p = new cjs.Sprite();



    (lib.produccionconenergiasrenovables2 = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(7);
    }).prototype = p = new cjs.Sprite();



    (lib.termoelectrica = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(8);
    }).prototype = p = new cjs.Sprite();



    (lib.valorizaciondetransferencias = function () {
        this.spriteSheet = ss["InfografiaAnimadaAnimatefinalv3_atlas_"];
        this.gotoAndStop(9);
    }).prototype = p = new cjs.Sprite();
    // helper functions:

    function mc_symbol_clone() {
        var clone = this._cloneProps(new this.constructor(this.mode, this.startPosition, this.loop));
        clone.gotoAndStop(this.currentFrame);
        clone.paused = this.paused;
        clone.framerate = this.framerate;
        return clone;
    }

    function getMCSymbolPrototype(symbol, nominalBounds, frameBounds) {
        var prototype = cjs.extend(symbol, cjs.MovieClip);
        prototype.clone = mc_symbol_clone;
        prototype.nominalBounds = nominalBounds;
        prototype.frameBounds = frameBounds;
        return prototype;
    }


    (lib.PALO = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#EBE8DE").s().p("AhoAlIAAgxQAAgKAHgHQAHgHAJAAICiAAQAKAAAHAHQAHAHAAAKIAAAxg");
        this.shape.setTransform(-0.1, 40.8);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#8C8EA2").s().p("AgtA1QgIAAgGgGQgHgHAAgIIAAg/QAAgJAHgGQAGgGAIAAIBaAAQAJAAAGAGQAHAGAAAJIAAA/QAAAIgHAHQgGAGgJAAg");
        this.shape_1.setTransform(-0.1, 39.1);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#EBE8DE").s().p("AguHgIAcu/IAlAAIAcO/g");
        this.shape_2.setTransform(-0.2, -4.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-10.6, -52.2, 21, 96.7);


    (lib.NUBES2 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgVQgVgUAQgSQAOgQAjgDQAbgDAfAFQAQADAgAOQgdgTgHgVQgFgPAHgNQAIgNASgHQAggKAeAPQAXAMAKAeQAFAPAAARIABgjQAFgVAXgCQAWgBAIAPQAFAJAAAQQAAgHACgMQAFgYAMgRQAOgTAagHQAagHAUAIQAWAKADAUQACASgLAOQgMAPgIAIQAPgLAPgDQAQgCALAGQAQAKgIAMQgHALgSAJQAegKAjADQAkAEAFASQAGAUgaANQgXAKgcAAQlSAAhPgCg");
        this.shape.setTransform(49.5, 73.4);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgVQgVgUAQgSQAOgQAjgDQAbgDAeAFQARADAfAOQgHgFgIgIQgQgPgEgNQgFgOAHgNQAIgNASgHQAfgKAeAPQAYAMAKAeQAFAPAAARIABgjQAFgVAXgCQAWgBAIAPQAFAJAAAQQAAgIACgLQAFgZALgQQAPgTAagHQAZgHAVAIQAWAJACAUQADASgLAOQgQAUgEAEQAPgLAPgDQAQgCALAGQAQAJgIANQgHALgSAJQAegKAiADQAlAEAFASQAGAUgaANQgXAKgcAAQlSAAhPgCg");
        this.shape_1.setTransform(-103.4, 80.2);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgUQgUgUAQgTQAOgQAigDQAcgDAeAGQAQACAgAOQgdgTgHgVQgEgPAHgNQAHgNATgHQAegKAfAPQAXAMAKAeQAFAPABAPIAAghQAFgVAXgCQAWgBAJAPQAEAJAAAQIACgTQAFgYAMgRQAOgTAbgHQAZgHAUAIQAWAKADAUQADARgMAPQgLAPgJAIQAPgLAQgDQAPgCAMAGQAPAKgIAMQgHALgSAJQAfgKAiADQAlAEAFASQAFAUgaANQgWAKgdAAQlRAAhQgCg");
        this.shape_2.setTransform(383.4, 84.7);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgVQgUgTAQgTQAOgQAigDQAbgDAfAFQAQADAgAOQgdgTgHgVQgEgPAHgNQAHgNATgHQAfgKAeAPQAXAMAKAeQAFAPABAPIAAghQAFgVAXgCQAWgBAJAPQAEAJAAAQIACgTQAFgYAMgRQAOgTAbgHQAZgHAUAIQAWAKADAUQADARgMAPQgLAPgJAIQAPgLAQgDQAPgCAMAGQAPAKgIAMQgHALgSAJQAegKAjADQAlAEAFASQAFAUgaANQgWAKgdAAQlRAAhQgCg");
        this.shape_3.setTransform(230.2, 68);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgUQgVgUAQgSQAOgRAjgDQAagCAfAFQARACAfAOQgcgTgHgVQgFgPAHgMQAIgOASgGQAfgLAeAQQAYAMAKAdQAFAPAAARIABgjQAFgVAXgBQAWgCAIAQQAFAIAAAQQAAgHACgMQAFgYALgRQAPgTAagHQAZgHAVAJQAWAJACAUQADARgLAPQgQATgEAEQAPgLAPgCQAQgDALAHQAQAJgIAMQgHALgSAJQAfgKAiAEQAkADAFASQAGAUgaANQgWAKgdAAQlSAAhPgCg");
        this.shape_4.setTransform(-242.8, 64.4);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#DBF1FB").s().p("AjTBbQgtgBgWgWQgVgUAQgTQAPgRAjgDQAbgCAgAFQARADAgAOQgdgUgHgWQgFgPAHgNQAIgOATgGQAfgLAgAQQAYAMAKAfQAFAPABAPIAAghQAFgWAYgCQAXgBAIAQQAFAJAAAQQAAgIACgMQAFgZAMgQQAPgVAbgHQAagHAVAJQAWAKADAUQADASgMAPQgPATgFAFQAPgLAQgDQAQgDAMAHQAQAKgIANQgHALgTAJQAfgLAjAEQAmAEAFASQAGAVgbANQgXALgdAAQlaAAhSgCg");
        this.shape_5.setTransform(-392.1, 84);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgVQgVgUAQgSQAOgQAjgDQAbgDAeAFQARADAfAOQgcgTgHgWQgFgOAHgNQAIgNASgHQAfgKAeAPQAYAMAKAeQAFAPAAAPIABghQAEgVAYgCQAWgBAIAPQAFAJAAAQIACgTQAEgZAMgQQAPgTAagHQAZgHAVAIQAWAJACAUQADASgLAOQgQAUgEAEQAPgLAPgDQAQgCALAGQAQAJgIANQgHALgSAJQAegKAiADQAlAEAFASQAGAUgaANQgXAKgcAAQlRAAhQgCg");
        this.shape_6.setTransform(191.1, -63);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgUQgVgUAQgTQAOgQAjgDQAbgDAeAGQARACAfAOQgcgTgHgVQgFgPAHgMQAIgOASgGQAfgLAeAQQAYAMAKAdQAFAPAAARIABgjQAFgVAXgCQAWgBAIAPQAFAJAAAQQAAgHACgMQAFgYALgRQAPgTAagHQAZgHAVAJQAWAJACAUQADARgLAPQgMAPgIAIQAPgLAPgCQAQgDALAGQAQAKgIAMQgHALgSAJQAfgKAiAEQAkADAFASQAGAUgaANQgWAKgdAAQlSAAhPgCg");
        this.shape_7.setTransform(104.9, -74.7);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgUQgUgUAQgTQAOgQAigDQAcgDAeAGQAQACAgAOQgdgTgHgVQgEgPAHgNQAHgNATgHQAfgKAeAPQAXAMAKAeQAFAPABAPIAAghQAFgVAXgCQAWgBAIAPQAFAJAAAQIACgTQAFgYAMgRQAOgTAbgHQAZgHAUAIQAWAKADAUQACASgLAOQgPATgFAEQAPgLAPgDQAQgCALAGQAQAKgIAMQgHALgSAJQAegKAjADQAlAEAEASQAGAUgaANQgWAKgdAAQlRAAhQgCg");
        this.shape_8.setTransform(271.4, -52.6);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgUQgVgUAQgTQAOgQAjgDQAcgDAeAGQAQACAgAOQgdgTgHgVQgEgPAHgNQAHgNATgHQAfgKAeAPQAjASAEA4IAAgjQAFgVAXgCQAWgBAIAPQAFAJAAAQQAAgHACgMQAFgYAMgRQAOgTAagHQAagHAUAIQAWAKADAUQACARgLAPIgUAXQAPgLAPgDQAQgCALAGQAQAKgIAMQgHALgSAJQAegKAjADQAkAEAFASQAGAUgaANQgWAKgdAAQlRAAhQgCg");
        this.shape_9.setTransform(385.6, -52.6);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgVQgUgTAQgTQAOgQAigDQAbgDAfAFQAQADAgAOQgdgTgHgWQgEgOAHgNQAHgNATgHQAegKAfAPQAXAMAKAeQAFAPABAPIAAghQAFgVAXgCQAWgBAIAPQAFAJAAAQIACgTQAFgZAMgQQAOgTAbgHQAZgHAUAIQAWAKADAUQACASgLANQgLAPgJAJQAPgLAPgDQAQgCALAGQAQAJgIANQgHALgSAJQAfgKAiADQAlAEAEASQAGAUgaANQgWAKgdAAQlRAAhQgCg");
        this.shape_10.setTransform(109, -45.5);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#DBF1FB").s().p("AjOBYQgrgBgWgUQgVgUAQgTQAOgQAjgDQAbgDAfAFQAQADAgAOQgdgTgHgVQgFgPAHgNQAIgNASgHQAggKAdAPQAYAMAKAeQAFAPAAARIABgjQAFgVAXgCQAWgBAIAPQAFAJAAAQQAAgHACgMQAFgYAMgRQAOgTAagHQAagHAUAIQAWAKACAUQADARgLAPQgMAPgIAIQAPgLAPgDQAQgCALAGQAQAKgIAMQgHALgSAJQAegKAjADQAkAEAFASQAGAUgaANQgWAKgdAAQlSAAhPgCg");
        this.shape_11.setTransform(-75.2, -69.8);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#DBF1FB").s().p("AACBcIjVgCQgtgBgWgVQgVgUAQgTQAPgRAjgDQAcgDAfAGQARADAhAOQgegUgHgWQgFgPAIgNQAHgOATgGQAfgLAgAQQAYAMAKAeQAEAMABAMQAAgRACgJQAEgWAYgCQAXgBAIAQQAFAJAAAQQAAgHACgNQAFgZAMgRQAPgUAbgHQAagHAVAJQAWAJADAVQADASgMAPQgPATgFAFQAPgLAQgDQAQgDAMAHQAQAKgIAMQgHAMgTAJQAggLAiAEQAmAEAFASQAGAVgbANQgXALgdAAIjXgBg");
        this.shape_12.setTransform(-224.5, -50.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-421.2, -83.7, 835.1, 177.5);


    (lib.HU1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#EAE9E5").s().p("Ah6BdQgGgDgDgFIgDgEQgJgRAQgVQgIgFgDgGQgIgOALgSQAKgSAXgMIAQgHQgEgKAHgMQAIgMAPgIQANgIAOAAQAOABAHAHIAJgGQAWgMAVABQAVACAIAOIACAIQAWgKATACQAUACAHAOQAIAOgLASQgKATgXAMQgSAJgTAAQgQAcgiASQgcAPgbABQgLAKgLAGQgXAMgUgBIgEAAQgHAAgHgEg");
        this.shape.setTransform(2.2, 2.1, 1, 1, 0.5);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-12.1, -7.5, 28.6, 19.3);


    (lib.COOES = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#1E2431").s().p("AgaBBQgKgEgOgLIAWgVQAPAPANAAQAGAAAFgEQAGgEAAgHQAAgDgDgEQgEgGgOgFQgPgHgHgEQgQgLAAgSQAAgMAHgJQAPgSAaAAQANAAALAFQAFACAOALIgVAUIgHgHQgHgFgIAAQgGAAgDADQgFADAAAGQAAAJAUAJQAPAFAJAGQAPALAAAQQAAASgPAMIgKAHQgMAGgPAAQgNAAgMgEg");
        this.shape.setTransform(31.6, 6);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#1E2431").s().p("AgpBBIAAiBIBTAAIAAAZIgzAAIAAAaIAyAAIAAAYIgyAAIAAAcIAzAAIAAAag");
        this.shape_1.setTransform(21.4, 6.1);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#1E2431").s().p("AgwA4QgfgTAAgjQABgPAGgOQAJgTAUgLQATgLAYAAQAIAAALACQAaAFARARQARAUABAZQgBAPgGANQgSAog3AAQgdAAgTgNgAAAgoQgQAAgMAIQgTAOABATQAAAJADAHQAGAMAMAHQANAGAOAAQAQAAAMgIQARgNAAgUQAAgQgMgLQgMgOgVAAIgCAAg");
        this.shape_2.setTransform(7.6, 6);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#1E2431").s().p("AgeA2QgbgTAAgjQAAgTAJgQIAKgNQALgKAOgFQAMgEAQAAQAQAAAQAEIALAEIAAAhQgSgPgVAAQgMAAgJAEQgWAMAAAaQAAAJADAHQADAKAIAGQALAKASAAIALgBQANgEAIgEIAHgFIAAAfQgWAIgUAAQgcAAgSgOg");
        this.shape_3.setTransform(-7.2, 6);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#FFC613").s().p("AgGACIgIgPQAHAJATAPIADADIgVgMg");
        this.shape_4.setTransform(-23.6, 3.1);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#FFC613").s().p("AgMADIAAgLQAMAFANACIAAAKQgMgBgNgFg");
        this.shape_5.setTransform(-16.8, 8.4);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#FFC613").s().p("AgHgNIAKAAQABANAEALIgFAAIgDAAQAAABAAAAQAAAAgBAAQAAABAAAAQgBABAAAAQgEgNgBgOg");
        this.shape_6.setTransform(-25.1, -0.7);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#FFC613").s().p("AgMAEIAAgNQANAFAMABIAAANQgQgDgJgDg");
        this.shape_7.setTransform(-16.8, 10.4);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#FFC613").s().p("AgJADQgCgCgBgGQALADAOACQgCAGgLAAQgFAAgEgDg");
        this.shape_8.setTransform(-16.8, 14.4);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#FFC613").s().p("AgMABIAAgLQAMAHANADIAAALQgMgCgNgIg");
        this.shape_9.setTransform(-16.8, 5.1);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#FFC613").s().p("AgBgBIABgGIACAPQgDgGAAgDg");
        this.shape_10.setTransform(-28.5, -0.6);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#FFC613").s().p("AgMAGIAAgQIAZAGIAAAPQgNgCgMgDg");
        this.shape_11.setTransform(-16.8, 12.5);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#FFC613").s().p("AgMACIAAgLQAOAHALACIAAAKQgNgDgMgFg");
        this.shape_12.setTransform(-16.8, 6.8);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#FFC613").s().p("AgHAIQgEgQgBgNQADgCAFABIAFAAQABAYALAVQgJgFgLgKg");
        this.shape_13.setTransform(-26.6, 0.3);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#FFC613").s().p("AgBANQgGgNAAgMIAIAAQABANAGAMg");
        this.shape_14.setTransform(-20.4, -0.8);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#FFC613").s().p("AgJgNIAFAAQAIAAAEADQACAEAAAHIAAANQgQgJgDgSg");
        this.shape_15.setTransform(-16.6, -0.6);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#FFC613").s().p("AgFAEQgIgKgCgOIAJAAQADAWATAJIAAAKQgNgFgIgMg");
        this.shape_16.setTransform(-17.2, 0);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#FFC613").s().p("AgMAAIAAgLQAMALANACIAAAKQgOgEgLgIg");
        this.shape_17.setTransform(-16.8, 3.5);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#FFC613").s().p("AgCANQgEgNgBgMIAKAAQABAOAEALg");
        this.shape_18.setTransform(-23.5, -0.8);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#FFC613").s().p("AgCANIAAgOIgLAAQgHgNgCgNIALAAQABAOAIAMIACABQAIALAPAGIAAALQgOgFgLgKg");
        this.shape_19.setTransform(-17.8, 0.7);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#FFC613").s().p("AgCANQgFgNAAgMIAJAAQACAOAFALg");
        this.shape_20.setTransform(-21.9, -0.8);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#FFC613").s().p("AgVAFQAOgRAWgOIAIANIgVAPIgCgCQgBgHgFAAQgHgCgEAhIgCADg");
        this.shape_21.setTransform(-23.8, -14);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#FFC613").s().p("AgIANQABgOADgLIANAAQgEANAAAMg");
        this.shape_22.setTransform(-27, -4.7);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#FFC613").s().p("AgHANQABgMADgNIALAAQgEAMgBANg");
        this.shape_23.setTransform(-25.1, -4.7);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#FFC613").s().p("AgNACQAKgIAKgHIAHAMQgMAGgIAJg");
        this.shape_24.setTransform(-21.9, -13.5);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#FFC613").s().p("AgBgDIACgBIAHgBIgPALQADgGADgDg");
        this.shape_25.setTransform(-25, -18.5);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#FFC613").s().p("AgEAqQgGAAgEgCQACgoATghIAIgIIgHAOQgIAQgCAMQgBAKAHAEQgEAQAAALg");
        this.shape_26.setTransform(-28.5, -7.6);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#FFC613").s().p("AgPgCQAOgMAJgGIAJAOQgRAKgQASIABgYg");
        this.shape_27.setTransform(-24.4, -16.6);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#FFC613").s().p("AgFAAQAGgPAIgLQgJAZgDAbQgJgIAHgSg");
        this.shape_28.setTransform(-30.3, -6.7);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#FFC613").s().p("AgMADQAGgFANgKIAGAKQgMAGgHAJg");
        this.shape_29.setTransform(-20.9, -11.8);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#FFC613").s().p("AgNASQADgXATgMIAEAHQgOAKgDASg");
        this.shape_30.setTransform(-17.4, -5.2);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#FFC613").s().p("AgJANQACgQANgJIABABIADAMQABAMgQAAg");
        this.shape_31.setTransform(-16.6, -4.7);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.f("#FFC613").s().p("AgQAYQABgMAHgNIAFAAIgDgDQAGgLAMgIIAFAJQgUAOgDAYg");
        this.shape_32.setTransform(-18.4, -5.8);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f("#FFC613").s().p("AgLAFQAJgKAJgHIAFAIQgLAGgHALg");
        this.shape_33.setTransform(-18.5, -7.8);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f("#FFC613").s().p("AgMADIATgPIAGAJQgMAHgHAJg");
        this.shape_34.setTransform(-20.1, -10.5);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f("#FFC613").s().p("AgHANQABgNAEgMIAKAAQgEALgBAOg");
        this.shape_35.setTransform(-23.5, -4.7);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f("#FFC613").s().p("AgHANQAAgNAFgMIALAAQgFALgCAOg");
        this.shape_36.setTransform(-21.9, -4.7);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#FFC613").s().p("AgLAEQAIgKAKgGIAGAJQgMAFgHALg");
        this.shape_37.setTransform(-19.3, -9.1);

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f("#FFC613").s().p("AgHANQAAgMAGgNIAJAAQgGAMgBANg");
        this.shape_38.setTransform(-20.4, -4.7);

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#FFC613").s().p("AgPACIAHgMQALAEANAGIgHALQgLgGgNgDg");
        this.shape_39.setTransform(-8.7, -15.4);

        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.f("#FFC613").s().p("AgOAAQAOgHAIgDIAHAMQgMADgKAGg");
        this.shape_40.setTransform(-18.5, -15.4);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#FFC613").s().p("AgOAAQAMgFALgDIAGAJQgNACgKAGg");
        this.shape_41.setTransform(-17.6, -13.7);

        this.shape_42 = new cjs.Shape();
        this.shape_42.graphics.f("#FFC613").s().p("AgPAAQAOgHAOgFQgCAHAFAIQgKADgNAHg");
        this.shape_42.setTransform(-19.6, -17.3);

        this.shape_43 = new cjs.Shape();
        this.shape_43.graphics.f("#FFC613").s().p("AgegBQAFgLAWABQAXAAAMAGQggADgdAOQgDgHACgGg");
        this.shape_43.setTransform(-18.6, -19.2);

        this.shape_44 = new cjs.Shape();
        this.shape_44.graphics.f("#FFC613").s().p("AgJgCQAGgFAHAFQAFACABAFg");
        this.shape_44.setTransform(-6.9, -18.6);

        this.shape_45 = new cjs.Shape();
        this.shape_45.graphics.f("#FFC613").s().p("AgOABIAGgJQALADAMAFIgGAJQgJgFgOgDg");
        this.shape_45.setTransform(-9.7, -13.7);

        this.shape_46 = new cjs.Shape();
        this.shape_46.graphics.f("#FFC613").s().p("AgPACIAGgJQANACAMAFIgGAIQgKgFgPgBg");
        this.shape_46.setTransform(-11.3, -11);

        this.shape_47 = new cjs.Shape();
        this.shape_47.graphics.f("#FFC613").s().p("AAAAEQgHAAgFABIgFgHQAJgCAIAAQAKAAAIACIgFAHQgFgBgIAAg");
        this.shape_47.setTransform(-13.6, -7.4);

        this.shape_48 = new cjs.Shape();
        this.shape_48.graphics.f("#FFC613").s().p("AgLgBQAFgCAGAAQAHAAAFACQgEAFgIAAQgHAAgEgFg");
        this.shape_48.setTransform(-13.6, -6.4);

        this.shape_49 = new cjs.Shape();
        this.shape_49.graphics.f("#FFC613").s().p("AAAACQgMAAgNAFIgFgHQAOgGAMAAIAEAHIAFgHQANAAANAGIgFAHQgOgFgMAAg");
        this.shape_49.setTransform(-13.6, -9.7);

        this.shape_50 = new cjs.Shape();
        this.shape_50.graphics.f("#FFC613").s().p("AgPACIAIgMIABgBIAWAKIgCAEIgGAJQgMgGgLgEg");
        this.shape_50.setTransform(-7.6, -17.2);

        this.shape_51 = new cjs.Shape();
        this.shape_51.graphics.f("#FFC613").s().p("AgOAAQAMgFAMgDIAFAJQgLACgNAGg");
        this.shape_51.setTransform(-16.7, -12.4);

        this.shape_52 = new cjs.Shape();
        this.shape_52.graphics.f("#FFC613").s().p("AgOAAQAMgFAMgCIAFAJQgOABgKAFg");
        this.shape_52.setTransform(-15.9, -11);

        this.shape_53 = new cjs.Shape();
        this.shape_53.graphics.f("#FFC613").s().p("AgOABIAGgJQALADAMAFIgFAJQgMgGgMgCg");
        this.shape_53.setTransform(-10.5, -12.4);

        this.shape_54 = new cjs.Shape();
        this.shape_54.graphics.f("#FFC613").s().p("AAAADQgHAAgMAEIgFgIQANgFALAAQALAAAOAFIgGAIQgKgEgJAAg");
        this.shape_54.setTransform(-13.6, -8.6);

        this.shape_55 = new cjs.Shape();
        this.shape_55.graphics.f("#FFC613").s().p("AgNgBIAHgMQAHAFANAKIgHAMQgJgJgLgGg");
        this.shape_55.setTransform(-5.4, -13.5);

        this.shape_56 = new cjs.Shape();
        this.shape_56.graphics.f("#FFC613").s().p("AgEAOQgBgOgDgKIADAAQAFAAAEgDQAEANABAOg");
        this.shape_56.setTransform(-0.2, -4.8);

        this.shape_57 = new cjs.Shape();
        this.shape_57.graphics.f("#FFC613").s().p("AgIgCIgBgBIAPAAIAEAHQgGgDgMgDg");
        this.shape_57.setTransform(-1.6, -9.1);

        this.shape_58 = new cjs.Shape();
        this.shape_58.graphics.f("#FFC613").s().p("AgMgDIAGgJQALAIAIAHIgGAKQgIgKgLgGg");
        this.shape_58.setTransform(-6.3, -11.8);

        this.shape_59 = new cjs.Shape();
        this.shape_59.graphics.f("#FFC613").s().p("AAAAeQAAgSgEgOQAEgHgJgHIgGgNQAMACAHAEQAKAZACAcIgFAAg");
        this.shape_59.setTransform(1.3, -6.4);

        this.shape_60 = new cjs.Shape();
        this.shape_60.graphics.f("#FFC613").s().p("AgRgOQAMgEAOASIAKAQQgRgRgTgNg");
        this.shape_60.setTransform(-1.1, -17.5);

        this.shape_61 = new cjs.Shape();
        this.shape_61.graphics.f("#FFC613").s().p("AgFgVQAMAMgBAVQAAAGgFAEQgBgXgFgUg");
        this.shape_61.setTransform(3.3, -5.9);

        this.shape_62 = new cjs.Shape();
        this.shape_62.graphics.f("#FFC613").s().p("AAZAWQgFgNgOgJQgHgEgFACQgNgLgKgGIAFgJIAFgFQAbASAVAaQABALgCAMQAAgFgDgHg");
        this.shape_62.setTransform(-1.6, -15.3);

        this.shape_63 = new cjs.Shape();
        this.shape_63.graphics.f("#FFC613").s().p("AgNAAIAIgOIATAPIAAABIgIAMIgTgOg");
        this.shape_63.setTransform(-4.3, -15.3);

        this.shape_64 = new cjs.Shape();
        this.shape_64.graphics.f("#FFC613").s().p("AAEASQgDgSgOgKIAFgHQATANACAWg");
        this.shape_64.setTransform(-9.8, -5.2);

        this.shape_65 = new cjs.Shape();
        this.shape_65.graphics.f("#FFC613").s().p("AgLgFIAFgHQAKAIAIAJIgFAIQgHgMgLgGg");
        this.shape_65.setTransform(-8.7, -7.8);

        this.shape_66 = new cjs.Shape();
        this.shape_66.graphics.f("#FFC613").s().p("AgCANQgBgJgEgQIALAAQADANABAMg");
        this.shape_66.setTransform(-2.1, -4.7);

        this.shape_67 = new cjs.Shape();
        this.shape_67.graphics.f("#FFC613").s().p("AAIANQgHgBgEgDQgLgHAIgNIABgBQANAKACAPg");
        this.shape_67.setTransform(-10.7, -4.7);

        this.shape_68 = new cjs.Shape();
        this.shape_68.graphics.f("#FFC613").s().p("AAHAYQgDgYgUgOIAFgJQALAIAHALIgCADIAEAAQAHAMABANg");
        this.shape_68.setTransform(-8.9, -5.8);

        this.shape_69 = new cjs.Shape();
        this.shape_69.graphics.f("#FFC613").s().p("AAAANQgBgNgGgMIAJAAQAFALABAOg");
        this.shape_69.setTransform(-6.8, -4.7);

        this.shape_70 = new cjs.Shape();
        this.shape_70.graphics.f("#FFC613").s().p("AgLgDIAFgJQAJAHAKAIIgGAKQgGgIgMgIg");
        this.shape_70.setTransform(-7.1, -10.5);

        this.shape_71 = new cjs.Shape();
        this.shape_71.graphics.f("#FFC613").s().p("AgBANQgBgNgFgMIAKAAQAEALABAOg");
        this.shape_71.setTransform(-5.3, -4.7);

        this.shape_72 = new cjs.Shape();
        this.shape_72.graphics.f("#FFC613").s().p("AgCANQAAgMgFgNIAKAAQAEAMABANg");
        this.shape_72.setTransform(-3.7, -4.7);

        this.shape_73 = new cjs.Shape();
        this.shape_73.graphics.f("#FFC613").s().p("AgLgDIAFgJQAKAGAJAKIgGAJQgHgLgLgFg");
        this.shape_73.setTransform(-7.9, -9.1);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_73 }, { t: this.shape_72 }, { t: this.shape_71 }, { t: this.shape_70 }, { t: this.shape_69 }, { t: this.shape_68 }, { t: this.shape_67 }, { t: this.shape_66 }, { t: this.shape_65 }, { t: this.shape_64 }, { t: this.shape_63 }, { t: this.shape_62 }, { t: this.shape_61 }, { t: this.shape_60 }, { t: this.shape_59 }, { t: this.shape_58 }, { t: this.shape_57 }, { t: this.shape_56 }, { t: this.shape_55 }, { t: this.shape_54 }, { t: this.shape_53 }, { t: this.shape_52 }, { t: this.shape_51 }, { t: this.shape_50 }, { t: this.shape_49 }, { t: this.shape_48 }, { t: this.shape_47 }, { t: this.shape_46 }, { t: this.shape_45 }, { t: this.shape_44 }, { t: this.shape_43 }, { t: this.shape_42 }, { t: this.shape_41 }, { t: this.shape_40 }, { t: this.shape_39 }, { t: this.shape_38 }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-31.2, -20.4, 67.9, 35.4);


    (lib.cables = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f().s("#3F8899").ss(2).p("AEsm9QhaEAioEOQjLFJiSAe");
        this.shape.setTransform(-127.9, 24.8);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f().s("#3F8899").ss(2).p("ADAnYIgLBAQgRBUghBlQhpFDjbFy");
        this.shape_1.setTransform(-156.3, 28.8);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f().s("#3F8899").ss(2).p("ABbr2QAeCjAGAuQAOB+gNCMQggFijmKv");
        this.shape_2.setTransform(-340.2, -47.9);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f().s("#3F8899").ss(2).p("Ah1LqQBJjbA/kpQCBpQgomC");
        this.shape_3.setTransform(-359, -40.4);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f().s("#3F8899").ss(2).p("AqwllIBhB7QB7CUCFB0QGoF5Fpg9QAagDC3hLQjfgfjvhcQnfi3hWkz");
        this.shape_4.setTransform(278.7, -36.6);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f().s("#3F8899").ss(2).p("ACxqhIhOGWIhHEoQhcFihwEh");
        this.shape_5.setTransform(259.5, -138.4);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f().s("#3F8899").ss(2).p("AD2qpIj+JjIhKDpQhZEZhJDv");
        this.shape_6.setTransform(234.3, -140.7);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f().s("#3F8899").ss(2).p("AqvleIBYBmQB1B7CQBpQHOFPI7Ah");
        this.shape_7.setTransform(-167.4, -54.6);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f().s("#3F8899").ss(2).p("AqtlhICmB5QDMCRC7BzQJYF0DZg3");
        this.shape_8.setTransform(-206.5, -54.3);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f().s("#3F8899").ss(2).p("AsrjoQFWF1KhBJQDSAXDbgKQBtgFBDgK");
        this.shape_9.setTransform(127.4, -6);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f().s("#3F8899").ss(2).p("Aq+jFQIyEsHeBIQDwAkCAgY");
        this.shape_10.setTransform(155.5, -9.5);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f().s("#3F8899").ss(2).p("AxhgzQKGE1NcisQENg2ECheQCBgwBLgl");
        this.shape_11.setTransform(-26.5, -14.6);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f().s("#3F8899").ss(2).p("ArNAWQIXBhH2hWQD8grCQg/");
        this.shape_12.setTransform(-25.4, -22);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f().s("#3F8899").ss(2).p("AE5EFQiDgRj0kMQhNhUhPhjIhAhSIAjBiQAuB1AyBhQChE5CTAA");
        this.shape_13.setTransform(-467.8, -231.8);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#72513B").s().p("AgIAJQgEgEgBgFQABgFAEgEQADgDAFAAQAGAAADADQAEAEAAAFQAAAFgEAEQgDAEgGAAQgFAAgDgEg");
        this.shape_14.setTransform(267.8, -179.3);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#010202").s().p("AgOAPQgGgHAAgIQAAgHAGgHQAGgGAIAAQAIAAAHAGQAGAHAAAHQAAAIgGAHQgHAGgIAAQgIAAgGgGg");
        this.shape_15.setTransform(277.2, -206.7);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#010202").s().p("AgOAPQgGgHAAgIQAAgHAGgHQAGgGAIAAQAIAAAHAGQAGAHAAAHQAAAIgGAHQgHAGgIAAQgIAAgGgGg");
        this.shape_16.setTransform(259, -206.7);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#010202").s().p("AgGAbIAAg1IANAAIAAA1g");
        this.shape_17.setTransform(268.5, -210.9);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#010202").s().p("AgygVIAJgOIBcA5IgJAOg");
        this.shape_18.setTransform(272.5, -211.1);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#010202").s().p("AgyAWIBcg5IAJAOIhdA5g");
        this.shape_19.setTransform(263.9, -211.1);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#72513B").s().p("AgUArIg2hPIBKA5IBLg/IhFBVg");
        this.shape_20.setTransform(268.6, -206.3);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#72513B").s().p("AgNCBIAAkBIAaAAIAAEBg");
        this.shape_21.setTransform(267.8, -192.3);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f().s("#3F8899").ss(2).p("AjuhVIBGAzQBRA0AxANQBPAVDKAg");
        this.shape_22.setTransform(253, -216.7);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f().s("#3F8899").ss(2).p("AjfhYIBEA8QBPA+A5ASQA4ASBiAKQAxAFAlAC");
        this.shape_23.setTransform(237.6, -216.2);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#72513B").s().p("AgGAHQgDgDAAgEQAAgDADgDQADgDADAAQAEAAADADQADADAAADQAAAEgDADQgDADgEAAQgDAAgDgDg");
        this.shape_24.setTransform(221.8, -205);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#010202").s().p("AgKALQgFgFAAgGQAAgGAFgEQAEgFAGAAQAGAAAFAFQAFAEAAAGQAAAGgFAFQgFAFgGAAQgGAAgEgFg");
        this.shape_25.setTransform(228.8, -225.6);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#010202").s().p("AgKALQgFgFAAgGQAAgGAFgEQAEgFAGAAQAGAAAFAFQAFAEAAAGQAAAGgFAFQgFAFgGAAQgGAAgEgFg");
        this.shape_26.setTransform(215.2, -225.6);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#010202").s().p("AgEAUIAAgnIAJAAIAAAng");
        this.shape_27.setTransform(222.3, -228.7);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#010202").s().p("AglgQIAGgKIBFArIgGAKg");
        this.shape_28.setTransform(225.3, -228.9);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#010202").s().p("AglARIBEgrIAHAKIhFArg");
        this.shape_29.setTransform(218.9, -228.9);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#72513B").s().p("AgPAhIgpg8IA4ArIA5gvIg0BAg");
        this.shape_30.setTransform(222.4, -225.2);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#72513B").s().p("AgJBhIAAjBIATAAIAADBg");
        this.shape_31.setTransform(221.8, -214.8);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.f().s("#3F8899").ss(2).p("AgsA8QgCgLABgRQADggATgaQAUgZAagGQAOgEAJAC");
        this.shape_32.setTransform(229.6, -231.9);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f().s("#3F8899").ss(2).p("Ag8AvQABgMAGgQQANgeAagSQAZgSAcABQAOABAIAE");
        this.shape_33.setTransform(221.5, -230.8);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f().s("#3F8899").ss(2).p("ABEmfIANBUQANBpgDBnQgMFMisDK");
        this.shape_34.setTransform(-265.7, -130.8);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f().s("#3F8899").ss(2).p("AhUovIAwCBQA1CfAfCVQBjHdisDK");
        this.shape_35.setTransform(-233.5, -145.5);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFgBgEAEQgEADgBAFQAAAFADAEg");
        this.shape_36.setTransform(-255.1, -65.4);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgEAAgFQABgFAEgDQAEgEAFABQAFABADAEQADAEAAAFQgBAFgEADQgEADgEAAIgBAAg");
        this.shape_37.setTransform(-255.1, -65.4);

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f().s("#636363").p("AgJAIQADADAFACQAFAAAEgDQAEgEABgFQABgEgEgEQgDgEgFgBQgFgBgEAEQgEADgBAFQAAAEADAFg");
        this.shape_38.setTransform(-256.8, -66.7);

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#636363").s().p("AgBAMQgFgBgDgDQgDgFAAgEQABgFAEgDQAEgEAFABQAFABADAEQAEAFgBADQgBAFgEAEQgDADgEAAIgCgBg");
        this.shape_39.setTransform(-256.8, -66.7);

        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.f().s("#636363").p("AgJAIQADAEAFABQAEABAEgEQAEgDABgFQABgFgDgEQgDgEgFgBQgFAAgEADQgEADgBAFQgBAFAEAEg");
        this.shape_40.setTransform(-258.2, -68.3);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgEAAgFQABgFAEgDQAEgDAFAAQAFABADAEQADAEAAAFQgBAFgFADQgDADgEAAIgBAAg");
        this.shape_41.setTransform(-258.2, -68.3);

        this.shape_42 = new cjs.Shape();
        this.shape_42.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQABgFgEgEQgDgEgFgBQgEAAgFADQgEADAAAFQgBAFADAEg");
        this.shape_42.setTransform(-259.3, -69.8);

        this.shape_43 = new cjs.Shape();
        this.shape_43.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgEAAgFQABgFAEgDQAFgDAEAAQAFABADAEQAEAEgCAFQAAAFgEADQgEADgEAAIgBAAg");
        this.shape_43.setTransform(-259.3, -69.8);

        this.shape_44 = new cjs.Shape();
        this.shape_44.graphics.f().s("#636363").p("AgJAIQADAEAFABQAEAAAFgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFAAgEADQgEADgBAFQgBAFAEAEg");
        this.shape_44.setTransform(-260.5, -71.3);

        this.shape_45 = new cjs.Shape();
        this.shape_45.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgEgEABgFQABgFAEgDQAEgDAFAAQAFABADAEQADAEAAAFQgBAFgEADQgEADgEAAIgBAAg");
        this.shape_45.setTransform(-260.5, -71.3);

        this.shape_46 = new cjs.Shape();
        this.shape_46.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFABgEgEQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAFgDAEg");
        this.shape_46.setTransform(-255.1, -65.5);

        this.shape_47 = new cjs.Shape();
        this.shape_47.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgEADgFQADgEAFgBQAFAAAEADQAEADABAFQAAAEgDAEQgDAFgFAAIgCABQgEAAgDgDg");
        this.shape_47.setTransform(-255.1, -65.5);

        this.shape_48 = new cjs.Shape();
        this.shape_48.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEg");
        this.shape_48.setTransform(-253.4, -66.8);

        this.shape_49 = new cjs.Shape();
        this.shape_49.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_49.setTransform(-253.4, -66.8);

        this.shape_50 = new cjs.Shape();
        this.shape_50.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQgBgFAEgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEg");
        this.shape_50.setTransform(-252, -68.4);

        this.shape_51 = new cjs.Shape();
        this.shape_51.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQgBgFAEgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_51.setTransform(-252, -68.4);

        this.shape_52 = new cjs.Shape();
        this.shape_52.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQgBgFAEgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFg");
        this.shape_52.setTransform(-250.8, -69.9);

        this.shape_53 = new cjs.Shape();
        this.shape_53.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQgBgFAEgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_53.setTransform(-250.8, -69.9);

        this.shape_54 = new cjs.Shape();
        this.shape_54.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQgBgFAEgEQADgEAFgBQAFAAAEADQADADACAFQABAFgEAEg");
        this.shape_54.setTransform(-249.6, -71.4);

        this.shape_55 = new cjs.Shape();
        this.shape_55.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAEAAAEADQAFADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_55.setTransform(-249.6, -71.4);

        this.shape_56 = new cjs.Shape();
        this.shape_56.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgFAAgDgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_56.setTransform(-231.7, -67.5);

        this.shape_57 = new cjs.Shape();
        this.shape_57.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgFAAgDgEg");
        this.shape_57.setTransform(-231.7, -67.5);

        this.shape_58 = new cjs.Shape();
        this.shape_58.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQADgEAFAAQAFAAAEAEQAEAEAAAEg");
        this.shape_58.setTransform(-231.7, -69.4);

        this.shape_59 = new cjs.Shape();
        this.shape_59.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQADgEAFAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_59.setTransform(-231.7, -69.4);

        this.shape_60 = new cjs.Shape();
        this.shape_60.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_60.setTransform(-231.7, -71.3);

        this.shape_61 = new cjs.Shape();
        this.shape_61.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_61.setTransform(-231.7, -71.3);

        this.shape_62 = new cjs.Shape();
        this.shape_62.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_62.setTransform(-279.9, -67.5);

        this.shape_63 = new cjs.Shape();
        this.shape_63.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgDAEAAQAFAAAEADQAEAEAAAEQAAAFgEAEQgEAEgFgBQgEABgEgEg");
        this.shape_63.setTransform(-279.9, -67.5);

        this.shape_64 = new cjs.Shape();
        this.shape_64.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_64.setTransform(-279.9, -69.4);

        this.shape_65 = new cjs.Shape();
        this.shape_65.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_65.setTransform(-279.9, -69.4);

        this.shape_66 = new cjs.Shape();
        this.shape_66.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_66.setTransform(-279.9, -71.3);

        this.shape_67 = new cjs.Shape();
        this.shape_67.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_67.setTransform(-279.9, -71.3);

        this.shape_68 = new cjs.Shape();
        this.shape_68.graphics.f().s("#636363").p("AgGgYIgTAvIAZgGIAZAEIgYgugAgnAiIAbhBIASgDIAiBBIgogFg");
        this.shape_68.setTransform(-255.1, -10);

        this.shape_69 = new cjs.Shape();
        this.shape_69.graphics.f("#636363").s().p("AgMgeIASgEIAiBBIgogFIgnAJgAgFgXIgUAuIAZgFIAZADIgXgtg");
        this.shape_69.setTransform(-255.1, -10);

        this.shape_70 = new cjs.Shape();
        this.shape_70.graphics.f().s("#636363").p("AgCA8IgBh3IAHAAIABB3g");
        this.shape_70.setTransform(-255.2, -7.4);

        this.shape_71 = new cjs.Shape();
        this.shape_71.graphics.f("#636363").s().p("AgCA8IgBh3IAHAAIABB3g");
        this.shape_71.setTransform(-255.2, -7.4);

        this.shape_72 = new cjs.Shape();
        this.shape_72.graphics.f().s("#636363").p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_72.setTransform(-248.3, -17.7);

        this.shape_73 = new cjs.Shape();
        this.shape_73.graphics.f("#636363").s().p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_73.setTransform(-248.3, -17.7);

        this.shape_74 = new cjs.Shape();
        this.shape_74.graphics.f().s("#636363").p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_74.setTransform(-262.7, -17.4);

        this.shape_75 = new cjs.Shape();
        this.shape_75.graphics.f("#636363").s().p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_75.setTransform(-262.7, -17.4);

        this.shape_76 = new cjs.Shape();
        this.shape_76.graphics.f().s("#636363").p("AgDAxIgBhiIAIAAIABBig");
        this.shape_76.setTransform(-255.5, -17.9);

        this.shape_77 = new cjs.Shape();
        this.shape_77.graphics.f("#636363").s().p("AgEgxIAIAAIABBiIgIAAg");
        this.shape_77.setTransform(-255.5, -17.9);

        this.shape_78 = new cjs.Shape();
        this.shape_78.graphics.f().s("#636363").p("AANAxIg8hLIBTgVIgQBTIAcgJIADAIgAgggVIAsA3IANhFg");
        this.shape_78.setTransform(-244, -11);

        this.shape_79 = new cjs.Shape();
        this.shape_79.graphics.f("#636363").s().p("AgwgaIBTgVIgQBSIAcgIIACAIIglANgAgigVIAtA3IANhGg");
        this.shape_79.setTransform(-243.9, -10.9);

        this.shape_80 = new cjs.Shape();
        this.shape_80.graphics.f().s("#636363").p("AgFA6IgqgKIAWhMIAAgCIABABIADgMIADAOIBAAXIgxA9QgBABgBAAgAgOgRIALA9IAjgsgAglAqIAaAGIgLg2g");
        this.shape_80.setTransform(-267.3, -12.7);

        this.shape_81 = new cjs.Shape();
        this.shape_81.graphics.f("#636363").s().p("AguAoIAWhMIAAgCIABABIADgMIADAOIBAAXIgxA9IgCABgAgkAiIAaAGIgLg2gAgCAkIAjgsIgugRg");
        this.shape_81.setTransform(-267.4, -11.9);

        this.shape_82 = new cjs.Shape();
        this.shape_82.graphics.f().s("#636363").p("Ag0BRIg2h1IABgEIACgBQADgBAHAHQAKAJApAnIAYAYIBthjIhRCLIgEgDIgYgZgAgMArIAUAVIAxhTgAhagVIApBYIAZgYQgVgWgtgqg");
        this.shape_82.setTransform(-253.6, -51.8);

        this.shape_83 = new cjs.Shape();
        this.shape_83.graphics.f("#636363").s().p("AhigtIABgEIACgBQACgBAIAIIAzAvIAYAYIBthjIhRCLIgFgDIgXgZIgiAggAgqA6IAagXIhChBgAgEAjIATAUIAyhTg");
        this.shape_83.setTransform(-254.4, -50.9);

        this.shape_84 = new cjs.Shape();
        this.shape_84.graphics.f().s("#636363").p("Ah3AYIAwBXICNgDIAyhTIh/iAgAhMB3Ig1hgIB6iKICJCLIg2BaQgBACgDAAg");
        this.shape_84.setTransform(-255.6, -24.7);

        this.shape_85 = new cjs.Shape();
        this.shape_85.graphics.f("#636363").s().p("AiBAWIB6iLICJCMIg2BaQgBACgDAAIiUACgAh3AXIAwBWICNgCIAyhTIh/iAg");
        this.shape_85.setTransform(-255.6, -24.5);

        this.shape_86 = new cjs.Shape();
        this.shape_86.graphics.f().s("#636363").p("AhThsIBUBxIBUhxgAhQB1IBNhpIhgiBIDIAAIhfCBIBKBlgAABAUIhABZIB/gEg");
        this.shape_86.setTransform(-255.5, -24.3);

        this.shape_87 = new cjs.Shape();
        this.shape_87.graphics.f("#636363").s().p("AgEANIhgiCIDJAAIhfCCIBKBkIigAEgAhABtICAgEIg/hVgAABAFIBUhxIioAAg");
        this.shape_87.setTransform(-255.5, -24.3);

        this.shape_88 = new cjs.Shape();
        this.shape_88.graphics.f().s("#636363").p("AB8AFIj3gBIAAgIID3ACg");
        this.shape_88.setTransform(-255.6, -22.3);

        this.shape_89 = new cjs.Shape();
        this.shape_89.graphics.f("#636363").s().p("Ah8AEIAAgIID5ACIAAAHg");
        this.shape_89.setTransform(-255.6, -22.3);

        this.shape_90 = new cjs.Shape();
        this.shape_90.graphics.f().s("#636363").p("ABxAEIjhAAIAAgHIDhAAg");
        this.shape_90.setTransform(-255.3, -30.1);

        this.shape_91 = new cjs.Shape();
        this.shape_91.graphics.f("#636363").s().p("AhwAEIAAgHIDhAAIAAAHg");
        this.shape_91.setTransform(-255.3, -30.1);

        this.shape_92 = new cjs.Shape();
        this.shape_92.graphics.f().s("#636363").p("ABXAEIitAAIAAgHICtAAg");
        this.shape_92.setTransform(-255.5, -43.4);

        this.shape_93 = new cjs.Shape();
        this.shape_93.graphics.f("#636363").s().p("AhWAEIAAgHICtAAIAAAHg");
        this.shape_93.setTransform(-255.5, -43.4);

        this.shape_94 = new cjs.Shape();
        this.shape_94.graphics.f().s("#636363").p("AhJBpIBJB2IBEh5IBlB4IhpngIiDAAIhnHUgAC5D+Ih0iKIhFB7IhKh5IhuB4IBun5ICQAAg");
        this.shape_94.setTransform(-255.2, -24.3);

        this.shape_95 = new cjs.Shape();
        this.shape_95.graphics.f("#636363").s().p("ABGB7IhGB7IhKh5IhuB4IBun5ICRAAIByIJgAAADmIBEh5IBmB4IhqngIiDAAIhmHUIBhhpg");
        this.shape_95.setTransform(-255.2, -25);

        this.shape_96 = new cjs.Shape();
        this.shape_96.graphics.f().s("#636363").p("AgJDrIg2idIg9h2IguiVIAIgDIAuCVIA8B2IAwCLIB+jzIAqimIgPgvIAbAAIAAAJIgQAAIAMAmIgrCqg");
        this.shape_96.setTransform(-255.4, -58.5);

        this.shape_97 = new cjs.Shape();
        this.shape_97.graphics.f("#636363").s().p("AhABRIg8h1IgviWIAIgCIAvCVIA8B2IAwCLIB9jzIAqimIgOgvIAbAAIAAAIIgQAAIAMAnIgrCqIiHEEg");
        this.shape_97.setTransform(-255.4, -58.8);

        this.shape_98 = new cjs.Shape();
        this.shape_98.graphics.f().s("#636363").p("AghBDIgHgFIBPhyIgUB1IgIgCIAOhPg");
        this.shape_98.setTransform(-240.1, -84.5);

        this.shape_99 = new cjs.Shape();
        this.shape_99.graphics.f("#636363").s().p("AgnA3IBPhyIgVB1IgIgCIAPhPIg7BTg");
        this.shape_99.setTransform(-240.1, -83.8);

        this.shape_100 = new cjs.Shape();
        this.shape_100.graphics.f().s("#636363").p("AgBAXIgIgCIAKgkIgOAAIAAgJIAaAAg");
        this.shape_100.setTransform(-271.6, -80.3);

        this.shape_101 = new cjs.Shape();
        this.shape_101.graphics.f("#636363").s().p("AgJAWIALglIgOAAIAAgIIAaAAIgOAvg");
        this.shape_101.setTransform(-271.6, -80.3);

        this.shape_102 = new cjs.Shape();
        this.shape_102.graphics.f().s("#636363").p("AAjBDIg6hTIAOBPIgIACIgVh1IBPByg");
        this.shape_102.setTransform(-271.2, -84.5);

        this.shape_103 = new cjs.Shape();
        this.shape_103.graphics.f("#636363").s().p("AgYgXIAOBPIgIACIgVh1IBPByIgGAFg");
        this.shape_103.setTransform(-271.1, -83.8);

        this.shape_104 = new cjs.Shape();
        this.shape_104.graphics.f().s("#636363").p("AB0AgIgzgxIg/AxIg1gxIhCAsIg2gzIAGgGIAxAuIBCgsIA1AxIA/gxIAyAwIAygyIAGAGg");
        this.shape_104.setTransform(-255.7, -75.2);

        this.shape_105 = new cjs.Shape();
        this.shape_105.graphics.f("#636363").s().p("ABBgRIg/AxIg1gxIhCArIg2gyIAGgGIAxAuIBCgsIA1AxIA/gxIAyAwIAygyIAGAGIg4A4g");
        this.shape_105.setTransform(-255.7, -75.2);

        this.shape_106 = new cjs.Shape();
        this.shape_106.graphics.f().s("#636363").p("AjpAXIHSAAIhBgtIlPAAgAEEAgIoHAAIBZg+IFUAAg");
        this.shape_106.setTransform(-255.7, -75.3);

        this.shape_107 = new cjs.Shape();
        this.shape_107.graphics.f("#636363").s().p("AkDAfIBZg+IFUAAIBaA+gAjpAXIHSAAIhBgtIlPAAg");
        this.shape_107.setTransform(-255.7, -75.3);

        this.shape_108 = new cjs.Shape();
        this.shape_108.graphics.f("#72513B").s().p("AgQARQgHgHAAgKQAAgJAHgHQAHgHAJAAQAKAAAHAHQAHAHAAAJQAAAKgHAHQgHAHgKAAQgJAAgHgHg");
        this.shape_108.setTransform(225.8, -19.2);

        this.shape_109 = new cjs.Shape();
        this.shape_109.graphics.f("#010202").s().p("AgaAbQgKgLgBgQQABgPAKgLQAMgMAOAAQAPAAALAMQALALAAAPQAAAQgLALQgLAMgPAAQgOAAgMgMg");
        this.shape_109.setTransform(242.5, -69.7);

        this.shape_110 = new cjs.Shape();
        this.shape_110.graphics.f("#010202").s().p("AgaAbQgLgLAAgQQAAgPALgLQALgMAPAAQAPAAALAMQALALAAAPQAAAQgLALQgLAMgPAAQgPAAgLgMg");
        this.shape_110.setTransform(210, -69.7);

        this.shape_111 = new cjs.Shape();
        this.shape_111.graphics.f("#010202").s().p("AgLAxIAAhhIAXAAIAABhg");
        this.shape_111.setTransform(227, -77.3);

        this.shape_112 = new cjs.Shape();
        this.shape_112.graphics.f("#010202").s().p("AhagoIAPgZICmBrIgPAYg");
        this.shape_112.setTransform(234.1, -77.8);

        this.shape_113 = new cjs.Shape();
        this.shape_113.graphics.f("#010202").s().p("AhaAqICmhrIAPAZIimBqg");
        this.shape_113.setTransform(218.8, -77.8);

        this.shape_114 = new cjs.Shape();
        this.shape_114.graphics.f("#72513B").s().p("AglBQIhhiUQAGAGBAAzIA/AxICHh1Ih8Cfg");
        this.shape_114.setTransform(227.2, -68.8);

        this.shape_115 = new cjs.Shape();
        this.shape_115.graphics.f("#72513B").s().p("AgXDvIAAncIAvAAIAAHcg");
        this.shape_115.setTransform(225.8, -43.1);

        this.shape_116 = new cjs.Shape();
        this.shape_116.graphics.f("#72513B").s().p("AgQAQQgGgGgBgKQABgIAGgHQAIgHAIAAQAJAAAIAHQAGAHABAIQgBAKgGAGQgHAHgKAAQgIAAgIgHg");
        this.shape_116.setTransform(248.9, -100);

        this.shape_117 = new cjs.Shape();
        this.shape_117.graphics.f("#010202").s().p("AgYAaQgMgLAAgPQAAgOAMgLQAKgKAOgBQAPABALAKQALALAAAOQAAAPgLALQgLAKgPAAQgOAAgKgKg");
        this.shape_117.setTransform(265.2, -147.7);

        this.shape_118 = new cjs.Shape();
        this.shape_118.graphics.f("#010202").s().p("AgZAaQgLgLAAgPQAAgOALgLQALgKAOgBQAPABALAKQALALAAAOQAAAPgLALQgLAKgPAAQgOAAgLgKg");
        this.shape_118.setTransform(233.4, -147.7);

        this.shape_119 = new cjs.Shape();
        this.shape_119.graphics.f("#010202").s().p("AgLAvIAAhcIAXAAIAABcg");
        this.shape_119.setTransform(250.1, -155);

        this.shape_120 = new cjs.Shape();
        this.shape_120.graphics.f("#010202").s().p("AhXgmIAOgXIChBkIgOAXg");
        this.shape_120.setTransform(257, -155.4);

        this.shape_121 = new cjs.Shape();
        this.shape_121.graphics.f("#010202").s().p("AhXAnIChhkIAOAXIihBkg");
        this.shape_121.setTransform(242.1, -155.4);

        this.shape_122 = new cjs.Shape();
        this.shape_122.graphics.f("#72513B").s().p("AgkBMIheiMQAEAFBAAxIA+AuICDhuIh5CWg");
        this.shape_122.setTransform(250.2, -146.9);

        this.shape_123 = new cjs.Shape();
        this.shape_123.graphics.f("#72513B").s().p("AgXDiIAAnDIAvAAIAAHDg");
        this.shape_123.setTransform(248.9, -122.6);

        this.shape_124 = new cjs.Shape();
        this.shape_124.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFAAgEADQgEADgBAFQAAAFADAEg");
        this.shape_124.setTransform(66.2, -5.1);

        this.shape_125 = new cjs.Shape();
        this.shape_125.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgEAAgFQABgFAEgDQAEgDAFAAQAFABADAEQADAEAAAFQgBAFgEADQgEADgEAAIgBAAg");
        this.shape_125.setTransform(66.2, -5.1);

        this.shape_126 = new cjs.Shape();
        this.shape_126.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQABgFgEgEQgDgEgFgBQgFgBgEAEQgEADgBAFQgBAEAEAFg");
        this.shape_126.setTransform(64.5, -6.3);

        this.shape_127 = new cjs.Shape();
        this.shape_127.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgFABgEQAAgFAEgDQAEgEAFABQAFABADAEQAEAEgCAFQAAAFgEADQgEADgEAAIgBAAg");
        this.shape_127.setTransform(64.5, -6.3);

        this.shape_128 = new cjs.Shape();
        this.shape_128.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQABgFgEgEQgDgEgFgBQgFgBgEAEQgEADgBAFQgBAEAEAFg");
        this.shape_128.setTransform(63.1, -7.9);

        this.shape_129 = new cjs.Shape();
        this.shape_129.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgFAAgEQABgFAFgDQAEgEAEABQAFABADAEQADAEAAAFQgBAFgFADQgDADgEAAIgBAAg");
        this.shape_129.setTransform(63.1, -7.9);

        this.shape_130 = new cjs.Shape();
        this.shape_130.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFABAEgEQAEgDABgFQABgFgEgEQgDgEgFgBQgFgBgEAEQgEADgBAFQgBAFAEAEg");
        this.shape_130.setTransform(62, -9.4);

        this.shape_131 = new cjs.Shape();
        this.shape_131.graphics.f("#636363").s().p("AgBANQgFgBgDgFQgEgEABgEQABgFAFgDQADgDAFAAQAFABADAFQAEAEgCAEQAAAFgEADQgEADgEAAIgBAAg");
        this.shape_131.setTransform(62, -9.4);

        this.shape_132 = new cjs.Shape();
        this.shape_132.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFgBgEAEQgEADgBAFQAAAEADAFg");
        this.shape_132.setTransform(60.8, -10.9);

        this.shape_133 = new cjs.Shape();
        this.shape_133.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgFAAgEQABgFAEgDQAEgEAFABQAFABADAEQADAEAAAFQgBAFgEADQgEADgEAAIgBAAg");
        this.shape_133.setTransform(60.8, -10.9);

        this.shape_134 = new cjs.Shape();
        this.shape_134.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQgBgFAEgEQADgEAFgBQAFAAAEADQADADACAFQABAFgEAEg");
        this.shape_134.setTransform(66.3, -5.2);

        this.shape_135 = new cjs.Shape();
        this.shape_135.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAEAAAEADQAFADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_135.setTransform(66.3, -5.2);

        this.shape_136 = new cjs.Shape();
        this.shape_136.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFg");
        this.shape_136.setTransform(67.9, -6.4);

        this.shape_137 = new cjs.Shape();
        this.shape_137.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgEADgFQADgEAFAAQAFgCAEAEQAEADABAFQAAAFgDADQgDAFgFABIgCAAQgEAAgDgDg");
        this.shape_137.setTransform(67.9, -6.4);

        this.shape_138 = new cjs.Shape();
        this.shape_138.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFABgEgEQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAFgDAEg");
        this.shape_138.setTransform(69.3, -8);

        this.shape_139 = new cjs.Shape();
        this.shape_139.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFAAQAFgBAEADQAEADABAFQAAAFgDAEQgDAEgFAAIgCABQgEAAgDgDg");
        this.shape_139.setTransform(69.3, -8);

        this.shape_140 = new cjs.Shape();
        this.shape_140.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEg");
        this.shape_140.setTransform(70.5, -9.5);

        this.shape_141 = new cjs.Shape();
        this.shape_141.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_141.setTransform(70.5, -9.5);

        this.shape_142 = new cjs.Shape();
        this.shape_142.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgEABgFgEQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQABAFgEAEg");
        this.shape_142.setTransform(71.7, -11.1);

        this.shape_143 = new cjs.Shape();
        this.shape_143.graphics.f("#636363").s().p("AgGAKQgFgDgBgFQAAgFADgEQADgEAFgBQAEAAAEADQAFADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgDgDg");
        this.shape_143.setTransform(71.7, -11.1);

        this.shape_144 = new cjs.Shape();
        this.shape_144.graphics.f().s("#636363").p("AANAAQAAAGgEADQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_144.setTransform(89.6, -7.1);

        this.shape_145 = new cjs.Shape();
        this.shape_145.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAGgEADQgEAEgFAAQgEAAgEgEg");
        this.shape_145.setTransform(89.6, -7.1);

        this.shape_146 = new cjs.Shape();
        this.shape_146.graphics.f().s("#636363").p("AANAAQAAAGgEADQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_146.setTransform(89.6, -9);

        this.shape_147 = new cjs.Shape();
        this.shape_147.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAGgEADQgEAEgFAAQgEAAgEgEg");
        this.shape_147.setTransform(89.6, -9);

        this.shape_148 = new cjs.Shape();
        this.shape_148.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEADAAAFg");
        this.shape_148.setTransform(89.6, -10.9);

        this.shape_149 = new cjs.Shape();
        this.shape_149.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEADAAAFQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_149.setTransform(89.6, -10.9);

        this.shape_150 = new cjs.Shape();
        this.shape_150.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgFAAgDgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_150.setTransform(41.4, -7.1);

        this.shape_151 = new cjs.Shape();
        this.shape_151.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgFAAgDgEg");
        this.shape_151.setTransform(41.4, -7.1);

        this.shape_152 = new cjs.Shape();
        this.shape_152.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQADgEAFAAQAFAAAEAEQAEAEAAAEg");
        this.shape_152.setTransform(41.4, -9);

        this.shape_153 = new cjs.Shape();
        this.shape_153.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQADgEAFAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_153.setTransform(41.4, -9);

        this.shape_154 = new cjs.Shape();
        this.shape_154.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_154.setTransform(41.4, -11);

        this.shape_155 = new cjs.Shape();
        this.shape_155.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_155.setTransform(41.4, -11);

        this.shape_156 = new cjs.Shape();
        this.shape_156.graphics.f().s("#636363").p("AgGgYIgUAuIAZgFIAaADIgYgtgAgnAiIAbhBIARgDIAiBAIgogFg");
        this.shape_156.setTransform(66.2, 50.4);

        this.shape_157 = new cjs.Shape();
        this.shape_157.graphics.f("#636363").s().p("AgMgeIASgDIAiBAIgogFIgnAIgAgGgXIgTAuIAZgFIAZADIgYgtg");
        this.shape_157.setTransform(66.2, 50.4);

        this.shape_158 = new cjs.Shape();
        this.shape_158.graphics.f().s("#636363").p("AgCA9IgBh4IAHAAIABB4g");
        this.shape_158.setTransform(66, 53);

        this.shape_159 = new cjs.Shape();
        this.shape_159.graphics.f("#636363").s().p("AgCA8IgBh3IAHAAIABB3g");
        this.shape_159.setTransform(66.1, 53);

        this.shape_160 = new cjs.Shape();
        this.shape_160.graphics.f().s("#636363").p("AgDAxIgBhiIAIAAIABBig");
        this.shape_160.setTransform(73, 42.7);

        this.shape_161 = new cjs.Shape();
        this.shape_161.graphics.f("#636363").s().p("AgEgxIAIAAIABBiIgIAAg");
        this.shape_161.setTransform(73, 42.7);

        this.shape_162 = new cjs.Shape();
        this.shape_162.graphics.f().s("#636363").p("AgDAxIgBhiIAHAAIABBig");
        this.shape_162.setTransform(58.6, 43);

        this.shape_163 = new cjs.Shape();
        this.shape_163.graphics.f("#636363").s().p("AgEgwIAIAAIABBhIgIABg");
        this.shape_163.setTransform(58.6, 43);

        this.shape_164 = new cjs.Shape();
        this.shape_164.graphics.f().s("#636363").p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_164.setTransform(65.8, 42.4);

        this.shape_165 = new cjs.Shape();
        this.shape_165.graphics.f("#636363").s().p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_165.setTransform(65.8, 42.4);

        this.shape_166 = new cjs.Shape();
        this.shape_166.graphics.f().s("#636363").p("AgggVIAsA3QADgTALgzgAANAxIg8hLIBTgVIgBAHQgOBKAAABIAbgIIADAIg");
        this.shape_166.setTransform(77.3, 49.4);

        this.shape_167 = new cjs.Shape();
        this.shape_167.graphics.f("#636363").s().p("AgwgaIBTgVIgCAGIgOBMIAcgJIACAIIglAOgAghgVIAsA3IANhGg");
        this.shape_167.setTransform(77.4, 49.5);

        this.shape_168 = new cjs.Shape();
        this.shape_168.graphics.f().s("#636363").p("AgOgSIALA9IAjgsgAgFA5IgqgJIAWhMIAAgDIABABIADgMIADAOIBAAXIgxA9QAAABgCAAgAglAqIAaAGIgLg3g");
        this.shape_168.setTransform(54, 47.7);

        this.shape_169 = new cjs.Shape();
        this.shape_169.graphics.f("#636363").s().p("AguApIAWhNIAAgCIABAAIADgLIADANIBAAYIgxA9QAAAAAAAAQAAAAgBABQAAAAAAAAQgBAAAAAAgAgkAiIAaAHIgLg3gAgCAkIAjgsIgugRg");
        this.shape_169.setTransform(53.9, 48.5);

        this.shape_170 = new cjs.Shape();
        this.shape_170.graphics.f().s("#636363").p("AgMAsIAUAUIAxhTgAg0BRIg2h1IABgEIACgBQADgBAIAHQAJAJApAnIAZAZIBshkIhRCMIgcgdgAhagUIApBXIAZgYg");
        this.shape_170.setTransform(67.7, 8.5);

        this.shape_171 = new cjs.Shape();
        this.shape_171.graphics.f("#636363").s().p("AhigtIABgDIACgBQADgBAHAGIAzAxIAZAYIBshkIhRCMIgcgcIgiAfgAgqA6IAagYIhCg/gAgEAjIAUAUIAxhTg");
        this.shape_171.setTransform(66.9, 9.4);

        this.shape_172 = new cjs.Shape();
        this.shape_172.graphics.f().s("#636363").p("Ah4AXIAxBXICNgCIAyhTIh/iAgAhMB3Ig2hhIB6iJICKCLIg2BaQgCACgCAAg");
        this.shape_172.setTransform(65.8, 35.7);

        this.shape_173 = new cjs.Shape();
        this.shape_173.graphics.f("#636363").s().p("AiCAVIB6iKICKCMIg2BaQAAAAgBABQAAAAgBAAQAAABgBAAQAAAAgBAAIiUACgAh3AWIAwBXICNgDIAyhSIh/iAg");
        this.shape_173.setTransform(65.8, 35.9);

        this.shape_174 = new cjs.Shape();
        this.shape_174.graphics.f().s("#636363").p("AhThtIBTByIBUhygAAAATIhABZIB/gDgAhQB1IBMhpIhgiBIDJAAIhfCBIBKBkg");
        this.shape_174.setTransform(65.8, 36.1);

        this.shape_175 = new cjs.Shape();
        this.shape_175.graphics.f("#636363").s().p("AgEANIhgiCIDJAAIhfCCIBKBkIigAFgAhABtIB/gEIg/hVgAAAAFIBUhyIinAAg");
        this.shape_175.setTransform(65.8, 36.1);

        this.shape_176 = new cjs.Shape();
        this.shape_176.graphics.f().s("#636363").p("AB9AFIj4gBIAAgIID4ABg");
        this.shape_176.setTransform(65.7, 38.1);

        this.shape_177 = new cjs.Shape();
        this.shape_177.graphics.f("#636363").s().p("Ah8AEIAAgIID5ACIAAAHg");
        this.shape_177.setTransform(65.7, 38.1);

        this.shape_178 = new cjs.Shape();
        this.shape_178.graphics.f().s("#636363").p("ABxAEIjhAAIAAgHIDhAAg");
        this.shape_178.setTransform(66, 30.3);

        this.shape_179 = new cjs.Shape();
        this.shape_179.graphics.f("#636363").s().p("AhwAEIAAgHIDhAAIAAAHg");
        this.shape_179.setTransform(66, 30.3);

        this.shape_180 = new cjs.Shape();
        this.shape_180.graphics.f().s("#636363").p("ABXAEIitAAIAAgHICtAAg");
        this.shape_180.setTransform(65.8, 16.9);

        this.shape_181 = new cjs.Shape();
        this.shape_181.graphics.f("#636363").s().p("AhWAEIAAgHICtAAIAAAHg");
        this.shape_181.setTransform(65.8, 16.9);

        this.shape_182 = new cjs.Shape();
        this.shape_182.graphics.f().s("#636363").p("AhJBpIBJB2IBEh5IBlB5IhpnhIiDAAIhnHUgAC5D+Ih0iKIhFB7IhKh4IhuB3IBun4ICQAAg");
        this.shape_182.setTransform(66.1, 36);

        this.shape_183 = new cjs.Shape();
        this.shape_183.graphics.f("#636363").s().p("ABGB7IhGB7IhKh4IhuB3IBun4ICRAAIByIIgAAADmIBEh5IBmB4IhqngIiDAAIhmHUIBhhqg");
        this.shape_183.setTransform(66.1, 35.4);

        this.shape_184 = new cjs.Shape();
        this.shape_184.graphics.f().s("#636363").p("AgJDrIg2ieIg9h1IguiWIAIgCIAuCVIA8B1IAxCMIB9jzIAqimIgPgvIAbAAIAAAJIgPAAIALAmIgrCqg");
        this.shape_184.setTransform(65.9, 1.9);

        this.shape_185 = new cjs.Shape();
        this.shape_185.graphics.f("#636363").s().p("AhABRIg8h1IgviWIAIgDIAvCWIA8B1IAwCLIB9jyIAqimIgOgvIAbAAIAAAJIgQAAIAMAmIgrCqIiHEEg");
        this.shape_185.setTransform(65.9, 1.6);

        this.shape_186 = new cjs.Shape();
        this.shape_186.graphics.f().s("#636363").p("AghBDIgHgFIBPhxIgUB0IgIgBIAOhQg");
        this.shape_186.setTransform(81.2, -24.1);

        this.shape_187 = new cjs.Shape();
        this.shape_187.graphics.f("#636363").s().p("AgnA3IBPhyIgVB1IgIgCIAPhQIg7BUg");
        this.shape_187.setTransform(81.2, -23.4);

        this.shape_188 = new cjs.Shape();
        this.shape_188.graphics.f().s("#636363").p("AgBAXIgIgCIALgkIgPAAIAAgJIAaAAg");
        this.shape_188.setTransform(49.7, -19.9);

        this.shape_189 = new cjs.Shape();
        this.shape_189.graphics.f("#636363").s().p("AgJAWIALgkIgOAAIAAgJIAaAAIgOAvg");
        this.shape_189.setTransform(49.7, -19.9);

        this.shape_190 = new cjs.Shape();
        this.shape_190.graphics.f().s("#636363").p("AAiBDIg6hTIAOBQIgIABIgUh0IBPBxg");
        this.shape_190.setTransform(50.2, -24.1);

        this.shape_191 = new cjs.Shape();
        this.shape_191.graphics.f("#636363").s().p("AgYgYIANBQIgHACIgVh1IBPByIgHAFg");
        this.shape_191.setTransform(50.3, -23.4);

        this.shape_192 = new cjs.Shape();
        this.shape_192.graphics.f().s("#636363").p("AB0AgIgygxIhAAwIg1gwIhBAsIg3g0IAGgGIAxAvIBCgsIA1AwIA/gwIAyAwIAygzIAGAGg");
        this.shape_192.setTransform(65.6, -14.8);

        this.shape_193 = new cjs.Shape();
        this.shape_193.graphics.f("#636363").s().p("ABCgRIhAAwIg1gwIhCAsIg2g0IAGgGIAxAvIBCgsIA1AwIA/gwIAyAwIAygzIAGAGIg4A5g");
        this.shape_193.setTransform(65.6, -14.8);

        this.shape_194 = new cjs.Shape();
        this.shape_194.graphics.f().s("#636363").p("AjoAXIHRAAIhBgtIlPAAgAEEAfIoHAAIBZg9IFUAAg");
        this.shape_194.setTransform(65.6, -15);

        this.shape_195 = new cjs.Shape();
        this.shape_195.graphics.f("#636363").s().p("AkDAfIBZg9IFUAAIBaA9gAjoAXIHRAAIhBgtIlPAAg");
        this.shape_195.setTransform(65.6, -15);

        this.shape_196 = new cjs.Shape();
        this.shape_196.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFgBgEAEQgEADgBAFQAAAEADAFg");
        this.shape_196.setTransform(-117.1, 5.8);

        this.shape_197 = new cjs.Shape();
        this.shape_197.graphics.f("#636363").s().p("AgBANQgFgBgDgFQgDgEAAgEQABgFAEgDQAEgDAFABQAFAAADAFQADAEAAAEQgBAFgEADQgDADgEAAIgCAAg");
        this.shape_197.setTransform(-117.1, 5.8);

        this.shape_198 = new cjs.Shape();
        this.shape_198.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQADgDACgFQABgFgEgEQgDgEgFgBQgFAAgEADQgEADgBAFQgBAFAEAEg");
        this.shape_198.setTransform(-118.8, 4.5);

        this.shape_199 = new cjs.Shape();
        this.shape_199.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgEgEABgFQABgFAEgDQAEgDAFAAQAFABADAEQADAEAAAFQgCAFgEADQgDADgEAAIgBAAg");
        this.shape_199.setTransform(-118.8, 4.5);

        this.shape_200 = new cjs.Shape();
        this.shape_200.graphics.f().s("#636363").p("AgJAIQADAEAFABQAEAAAFgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFAAgEADQgEADgBAFQgBAFAEAEg");
        this.shape_200.setTransform(-120.2, 3);

        this.shape_201 = new cjs.Shape();
        this.shape_201.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgEgEABgFQABgFAEgDQAFgDAEAAQAFABADAEQAEAEgCAFQAAAFgEADQgEADgEAAIgBAAg");
        this.shape_201.setTransform(-120.2, 3);

        this.shape_202 = new cjs.Shape();
        this.shape_202.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQABgFgEgEQgDgEgFgBQgEAAgFADQgEADgBAFQAAAFADAEg");
        this.shape_202.setTransform(-121.3, 1.5);

        this.shape_203 = new cjs.Shape();
        this.shape_203.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgDgEABgFQAAgFAEgDQAEgDAFAAQAFABADAEQADAEgBAFQgBAFgDADQgEADgEAAIgBAAg");
        this.shape_203.setTransform(-121.3, 1.5);

        this.shape_204 = new cjs.Shape();
        this.shape_204.graphics.f().s("#636363").p("AgJAIQADAEAFABQAFAAAEgDQAEgDABgFQAAgFgDgEQgDgEgFgBQgFAAgEADQgEADgBAFQgBAFAEAEg");
        this.shape_204.setTransform(-122.5, -0.1);

        this.shape_205 = new cjs.Shape();
        this.shape_205.graphics.f("#636363").s().p("AgBANQgFgBgDgEQgEgEABgFQABgFAEgDQAEgDAFAAQAFABADAEQADAEAAAFQgBAFgEADQgEADgEAAIgBAAg");
        this.shape_205.setTransform(-122.5, -0.1);

        this.shape_206 = new cjs.Shape();
        this.shape_206.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEg");
        this.shape_206.setTransform(-117.1, 5.7);

        this.shape_207 = new cjs.Shape();
        this.shape_207.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAFAAAEADQAEADABAFQAAAFgDAEQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_207.setTransform(-117.1, 5.7);

        this.shape_208 = new cjs.Shape();
        this.shape_208.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFg");
        this.shape_208.setTransform(-115.4, 4.4);

        this.shape_209 = new cjs.Shape();
        this.shape_209.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_209.setTransform(-115.4, 4.4);

        this.shape_210 = new cjs.Shape();
        this.shape_210.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFg");
        this.shape_210.setTransform(-114, 2.9);

        this.shape_211 = new cjs.Shape();
        this.shape_211.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAEgDAFQgDAEgFABIgCAAQgDAAgEgDg");
        this.shape_211.setTransform(-114, 2.9);

        this.shape_212 = new cjs.Shape();
        this.shape_212.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFABgEgEQgEgDgBgFQAAgFADgEQADgEAFgBQAFgBAEAEQAEADABAFQAAAFgDAEg");
        this.shape_212.setTransform(-112.8, 1.4);

        this.shape_213 = new cjs.Shape();
        this.shape_213.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQAAgFADgEQADgEAFAAQAFgCAEAEQAEADABAFQAAAEgDAFQgDAEgFAAIgCABQgEAAgDgDg");
        this.shape_213.setTransform(-112.8, 1.4);

        this.shape_214 = new cjs.Shape();
        this.shape_214.graphics.f().s("#636363").p("AAKAIQgDAEgFABQgFAAgEgDQgEgDgBgFQgBgFAEgEQADgEAFgBQAFgBAEAEQAEADABAFQABAEgEAFg");
        this.shape_214.setTransform(-111.6, -0.2);

        this.shape_215 = new cjs.Shape();
        this.shape_215.graphics.f("#636363").s().p("AgHAKQgEgDgBgFQgBgFAEgEQADgEAFgBQAFgBAEAEQAEADAAAFQACAEgEAFQgDAEgFABIgBAAQgEAAgEgDg");
        this.shape_215.setTransform(-111.6, -0.2);

        this.shape_216 = new cjs.Shape();
        this.shape_216.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_216.setTransform(-93.7, 3.8);

        this.shape_217 = new cjs.Shape();
        this.shape_217.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_217.setTransform(-93.7, 3.8);

        this.shape_218 = new cjs.Shape();
        this.shape_218.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_218.setTransform(-93.7, 1.9);

        this.shape_219 = new cjs.Shape();
        this.shape_219.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_219.setTransform(-93.7, 1.9);

        this.shape_220 = new cjs.Shape();
        this.shape_220.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_220.setTransform(-93.7, -0.1);

        this.shape_221 = new cjs.Shape();
        this.shape_221.graphics.f("#636363").s().p("AgIAJQgEgEAAgFQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_221.setTransform(-93.7, -0.1);

        this.shape_222 = new cjs.Shape();
        this.shape_222.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_222.setTransform(-141.9, 3.7);

        this.shape_223 = new cjs.Shape();
        this.shape_223.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_223.setTransform(-141.9, 3.7);

        this.shape_224 = new cjs.Shape();
        this.shape_224.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_224.setTransform(-141.9, 1.8);

        this.shape_225 = new cjs.Shape();
        this.shape_225.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_225.setTransform(-141.9, 1.8);

        this.shape_226 = new cjs.Shape();
        this.shape_226.graphics.f().s("#636363").p("AANAAQAAAFgEAEQgEAEgFAAQgEAAgEgEQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEg");
        this.shape_226.setTransform(-141.9, -0.1);

        this.shape_227 = new cjs.Shape();
        this.shape_227.graphics.f("#636363").s().p("AgIAJQgEgDAAgGQAAgEAEgEQAEgEAEAAQAFAAAEAEQAEAEAAAEQAAAFgEAEQgEAEgFAAQgEAAgEgEg");
        this.shape_227.setTransform(-141.9, -0.1);

        this.shape_228 = new cjs.Shape();
        this.shape_228.graphics.f().s("#636363").p("AgGgYIgTAvIAZgGIAZADIgYgtgAgnAiIAbhBIASgDIAiBBIgogFg");
        this.shape_228.setTransform(-117.1, 61.3);

        this.shape_229 = new cjs.Shape();
        this.shape_229.graphics.f("#636363").s().p("AgMgfIASgDIAiBBIgogEIgnAIgAgGgXIgTAvIAZgGIAaADIgYgtg");
        this.shape_229.setTransform(-117.1, 61.2);

        this.shape_230 = new cjs.Shape();
        this.shape_230.graphics.f().s("#636363").p("AgCA8IgBh4IAHAAIABB4g");
        this.shape_230.setTransform(-117.3, 63.9);

        this.shape_231 = new cjs.Shape();
        this.shape_231.graphics.f("#636363").s().p("AgEg7IAHAAIABB3IgHAAg");
        this.shape_231.setTransform(-117.2, 63.9);

        this.shape_232 = new cjs.Shape();
        this.shape_232.graphics.f().s("#636363").p("AgDAyIgBhiIAIAAIABBig");
        this.shape_232.setTransform(-110.3, 53.5);

        this.shape_233 = new cjs.Shape();
        this.shape_233.graphics.f("#636363").s().p("AgDAyIgBhiIAIAAIABBig");
        this.shape_233.setTransform(-110.3, 53.5);

        this.shape_234 = new cjs.Shape();
        this.shape_234.graphics.f().s("#636363").p("AgDAyIgBhiIAHAAIABBig");
        this.shape_234.setTransform(-124.7, 53.8);

        this.shape_235 = new cjs.Shape();
        this.shape_235.graphics.f("#636363").s().p("AgDAxIgBhhIAHgBIABBig");
        this.shape_235.setTransform(-124.7, 53.8);

        this.shape_236 = new cjs.Shape();
        this.shape_236.graphics.f().s("#636363").p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_236.setTransform(-117.5, 53.3);

        this.shape_237 = new cjs.Shape();
        this.shape_237.graphics.f("#636363").s().p("AgDAxIgBhhIAIAAIABBhg");
        this.shape_237.setTransform(-117.5, 53.3);

        this.shape_238 = new cjs.Shape();
        this.shape_238.graphics.f().s("#636363").p("AgggVIAsA3IANhFgAANAxIg8hLIBTgVIgQBTIAcgJIADAIg");
        this.shape_238.setTransform(-106, 60.3);

        this.shape_239 = new cjs.Shape();
        this.shape_239.graphics.f("#636363").s().p("AgwgaIBTgVIgQBSIAcgIIACAHIglAOgAgigVIAtA3IANhGg");
        this.shape_239.setTransform(-105.9, 60.3);

        this.shape_240 = new cjs.Shape();
        this.shape_240.graphics.f().s("#636363").p("AgFA6IgqgKIAWhMIAAgCIABAAIADgLIADAOIBAAXIgxA9QgBABgBAAgAglAqIAaAGIgLg2gAgOgRIALA9IAjgsg");
        this.shape_240.setTransform(-129.3, 58.5);

        this.shape_241 = new cjs.Shape();
        this.shape_241.graphics.f("#636363").s().p("AguAoIAWhMIAAgCIABAAIADgLIADAOIBAAXIgxA9IgCABgAgkAiIAaAGIgLg2gAgCAkIAjgsIgugRg");
        this.shape_241.setTransform(-129.4, 59.3);

        this.shape_242 = new cjs.Shape();
        this.shape_242.graphics.f().s("#636363").p("Ag0BRIg2h1IABgEIACAAQADgBAHAGQAJAIAqAoIAZAZIBshkIhRCMIgcgdgAgMAsIAUAUIAxhTgAhagUIApBXIAZgYg");
        this.shape_242.setTransform(-115.6, 19.4);

        this.shape_243 = new cjs.Shape();
        this.shape_243.graphics.f("#636363").s().p("AhigsIAAgFIADAAQACgBAIAHIAyAvIAaAZIBshkIhRCMIgcgdIgiAggAgqA6IAagXIhChAgAgEAjIAUAUIAwhSg");
        this.shape_243.setTransform(-116.4, 20.3);

        this.shape_244 = new cjs.Shape();
        this.shape_244.graphics.f().s("#636363").p("Ah4AYIAxBWICNgCIAyhTIh/iAgAhMB3Ig2hhIB6iJICKCLIg2BaQgCACgCAAg");
        this.shape_244.setTransform(-117.5, 46.6);

        this.shape_245 = new cjs.Shape();
        this.shape_245.graphics.f("#636363").s().p("AiBAVIB5iJICLCLIg3BZQAAABgBABQAAAAgBAAQAAABgBAAQAAAAgBAAIiUADgAh3AWIAwBXICNgCIAyhTIh/iAg");
        this.shape_245.setTransform(-117.5, 46.7);

        this.shape_246 = new cjs.Shape();
        this.shape_246.graphics.f().s("#636363").p("AhQB1IBMhpIhgiBIDJAAIhfCBIBKBlgAAAATIhABZIB/gDgAhThsIBTBxIBUhxg");
        this.shape_246.setTransform(-117.5, 46.9);

        this.shape_247 = new cjs.Shape();
        this.shape_247.graphics.f("#636363").s().p("AgEAMIhgiBIDJAAIhfCBIBKBlIigAEgAhABsIB/gDIg/hWgAAAAGIBUhyIinAAg");
        this.shape_247.setTransform(-117.5, 46.9);

        this.shape_248 = new cjs.Shape();
        this.shape_248.graphics.f().s("#636363").p("AB9AFIj5gBIAAgIID5ABg");
        this.shape_248.setTransform(-117.6, 49);

        this.shape_249 = new cjs.Shape();
        this.shape_249.graphics.f("#636363").s().p("Ah8ADIAAgHID5ABIAAAIg");
        this.shape_249.setTransform(-117.6, 49);

        this.shape_250 = new cjs.Shape();
        this.shape_250.graphics.f().s("#636363").p("ABxAEIjhAAIAAgHIDhAAg");
        this.shape_250.setTransform(-117.3, 41.1);

        this.shape_251 = new cjs.Shape();
        this.shape_251.graphics.f("#636363").s().p("AhwAEIAAgHIDhAAIAAAHg");
        this.shape_251.setTransform(-117.3, 41.1);

        this.shape_252 = new cjs.Shape();
        this.shape_252.graphics.f().s("#636363").p("ABXAEIitAAIAAgHICtAAg");
        this.shape_252.setTransform(-117.5, 27.8);

        this.shape_253 = new cjs.Shape();
        this.shape_253.graphics.f("#636363").s().p("AhWAEIAAgHICtAAIAAAHg");
        this.shape_253.setTransform(-117.5, 27.8);

        this.shape_254 = new cjs.Shape();
        this.shape_254.graphics.f().s("#636363").p("AhJBpIBJB2IBEh6IBlB5IhpngIiDAAIhnHUgAC5D+Ih0iKIhFB7IhKh5IhuB4IBun5ICQAAg");
        this.shape_254.setTransform(-117.2, 46.9);

        this.shape_255 = new cjs.Shape();
        this.shape_255.graphics.f("#636363").s().p("ABGB7IhGB7IhKh5IhuB4IBun5ICQAAIBzIJgAAADmIBEh6IBlB5IhpngIiDAAIhnHUIBhhpg");
        this.shape_255.setTransform(-117.2, 46.2);

        this.shape_256 = new cjs.Shape();
        this.shape_256.graphics.f().s("#636363").p("AgJDrIg2ieIg9h1IguiWIAIgCIAuCVIA8B1IAwCMIB+jzIAqimIgPgvIAbAAIAAAJIgQAAIAMAmIgrCqg");
        this.shape_256.setTransform(-117.4, 12.8);

        this.shape_257 = new cjs.Shape();
        this.shape_257.graphics.f("#636363").s().p("AhABRIg8h1IgviWIAIgCIAvCUIA8B2IAwCMIB9jzIAqimIgOgvIAbAAIAAAIIgQAAIAMAmIgrCqIiHEFg");
        this.shape_257.setTransform(-117.4, 12.4);

        this.shape_258 = new cjs.Shape();
        this.shape_258.graphics.f().s("#636363").p("AghBDIgHgFIBPhxIgUB0IgIgBIAOhQg");
        this.shape_258.setTransform(-102.1, -13.3);

        this.shape_259 = new cjs.Shape();
        this.shape_259.graphics.f("#636363").s().p("AgnA3IBPhxIgUB0IgIgBIANhQIg5BSg");
        this.shape_259.setTransform(-102.1, -12.5);

        this.shape_260 = new cjs.Shape();
        this.shape_260.graphics.f().s("#636363").p("AgBAXIgIgCIAKgkIgOAAIAAgJIAaAAg");
        this.shape_260.setTransform(-133.6, -9);

        this.shape_261 = new cjs.Shape();
        this.shape_261.graphics.f("#636363").s().p("AgJAVIALgkIgOAAIAAgIIAaAAIgPAvg");
        this.shape_261.setTransform(-133.6, -9.1);

        this.shape_262 = new cjs.Shape();
        this.shape_262.graphics.f().s("#636363").p("AAiBDIg6hTIAOBQIgIABIgUh0IBPBxg");
        this.shape_262.setTransform(-133.1, -13.3);

        this.shape_263 = new cjs.Shape();
        this.shape_263.graphics.f("#636363").s().p("AgZgXIAPBQIgIABIgVh0IBPBxIgGAEg");
        this.shape_263.setTransform(-133, -12.5);

        this.shape_264 = new cjs.Shape();
        this.shape_264.graphics.f().s("#636363").p("AB0AfIgzgwIg/AwIg1gxIhCAsIg2gzIAGgGIAxAuIBBgrIA1AwIBAgwIAyAwIAygzIAGAGg");
        this.shape_264.setTransform(-117.7, -3.9);

        this.shape_265 = new cjs.Shape();
        this.shape_265.graphics.f("#636363").s().p("ABBgRIg/AwIg1gxIhCAsIg2gzIAGgFIAxAuIBCgsIA1AwIA/gwIAyAwIAygyIAGAFIg4A5g");
        this.shape_265.setTransform(-117.7, -3.9);

        this.shape_266 = new cjs.Shape();
        this.shape_266.graphics.f().s("#636363").p("AjpAXIHSAAIhBgtIlPAAgAEEAgIoHAAIBZg+IFUAAg");
        this.shape_266.setTransform(-117.7, -4.1);

        this.shape_267 = new cjs.Shape();
        this.shape_267.graphics.f("#636363").s().p("AkDAgIBZg+IFUAAIBaA+gAjpAXIHSAAIhBgtIlPAAg");
        this.shape_267.setTransform(-117.7, -4.1);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_267 }, { t: this.shape_266 }, { t: this.shape_265 }, { t: this.shape_264 }, { t: this.shape_263 }, { t: this.shape_262 }, { t: this.shape_261 }, { t: this.shape_260 }, { t: this.shape_259 }, { t: this.shape_258 }, { t: this.shape_257 }, { t: this.shape_256 }, { t: this.shape_255 }, { t: this.shape_254 }, { t: this.shape_253 }, { t: this.shape_252 }, { t: this.shape_251 }, { t: this.shape_250 }, { t: this.shape_249 }, { t: this.shape_248 }, { t: this.shape_247 }, { t: this.shape_246 }, { t: this.shape_245 }, { t: this.shape_244 }, { t: this.shape_243 }, { t: this.shape_242 }, { t: this.shape_241 }, { t: this.shape_240 }, { t: this.shape_239 }, { t: this.shape_238 }, { t: this.shape_237 }, { t: this.shape_236 }, { t: this.shape_235 }, { t: this.shape_234 }, { t: this.shape_233 }, { t: this.shape_232 }, { t: this.shape_231 }, { t: this.shape_230 }, { t: this.shape_229 }, { t: this.shape_228 }, { t: this.shape_227 }, { t: this.shape_226 }, { t: this.shape_225 }, { t: this.shape_224 }, { t: this.shape_223 }, { t: this.shape_222 }, { t: this.shape_221 }, { t: this.shape_220 }, { t: this.shape_219 }, { t: this.shape_218 }, { t: this.shape_217 }, { t: this.shape_216 }, { t: this.shape_215 }, { t: this.shape_214 }, { t: this.shape_213 }, { t: this.shape_212 }, { t: this.shape_211 }, { t: this.shape_210 }, { t: this.shape_209 }, { t: this.shape_208 }, { t: this.shape_207 }, { t: this.shape_206 }, { t: this.shape_205 }, { t: this.shape_204 }, { t: this.shape_203 }, { t: this.shape_202 }, { t: this.shape_201 }, { t: this.shape_200 }, { t: this.shape_199 }, { t: this.shape_198 }, { t: this.shape_197 }, { t: this.shape_196 }, { t: this.shape_195 }, { t: this.shape_194 }, { t: this.shape_193 }, { t: this.shape_192 }, { t: this.shape_191 }, { t: this.shape_190 }, { t: this.shape_189 }, { t: this.shape_188 }, { t: this.shape_187 }, { t: this.shape_186 }, { t: this.shape_185 }, { t: this.shape_184 }, { t: this.shape_183 }, { t: this.shape_182 }, { t: this.shape_181 }, { t: this.shape_180 }, { t: this.shape_179 }, { t: this.shape_178 }, { t: this.shape_177 }, { t: this.shape_176 }, { t: this.shape_175 }, { t: this.shape_174 }, { t: this.shape_173 }, { t: this.shape_172 }, { t: this.shape_171 }, { t: this.shape_170 }, { t: this.shape_169 }, { t: this.shape_168 }, { t: this.shape_167 }, { t: this.shape_166 }, { t: this.shape_165 }, { t: this.shape_164 }, { t: this.shape_163 }, { t: this.shape_162 }, { t: this.shape_161 }, { t: this.shape_160 }, { t: this.shape_159 }, { t: this.shape_158 }, { t: this.shape_157 }, { t: this.shape_156 }, { t: this.shape_155 }, { t: this.shape_154 }, { t: this.shape_153 }, { t: this.shape_152 }, { t: this.shape_151 }, { t: this.shape_150 }, { t: this.shape_149 }, { t: this.shape_148 }, { t: this.shape_147 }, { t: this.shape_146 }, { t: this.shape_145 }, { t: this.shape_144 }, { t: this.shape_143 }, { t: this.shape_142 }, { t: this.shape_141 }, { t: this.shape_140 }, { t: this.shape_139 }, { t: this.shape_138 }, { t: this.shape_137 }, { t: this.shape_136 }, { t: this.shape_135 }, { t: this.shape_134 }, { t: this.shape_133 }, { t: this.shape_132 }, { t: this.shape_131 }, { t: this.shape_130 }, { t: this.shape_129 }, { t: this.shape_128 }, { t: this.shape_127 }, { t: this.shape_126 }, { t: this.shape_125 }, { t: this.shape_124 }, { t: this.shape_123 }, { t: this.shape_122 }, { t: this.shape_121 }, { t: this.shape_120 }, { t: this.shape_119 }, { t: this.shape_118 }, { t: this.shape_117 }, { t: this.shape_116 }, { t: this.shape_115 }, { t: this.shape_114 }, { t: this.shape_113 }, { t: this.shape_112 }, { t: this.shape_111 }, { t: this.shape_110 }, { t: this.shape_109 }, { t: this.shape_108 }, { t: this.shape_107 }, { t: this.shape_106 }, { t: this.shape_105 }, { t: this.shape_104 }, { t: this.shape_103 }, { t: this.shape_102 }, { t: this.shape_101 }, { t: this.shape_100 }, { t: this.shape_99 }, { t: this.shape_98 }, { t: this.shape_97 }, { t: this.shape_96 }, { t: this.shape_95 }, { t: this.shape_94 }, { t: this.shape_93 }, { t: this.shape_92 }, { t: this.shape_91 }, { t: this.shape_90 }, { t: this.shape_89 }, { t: this.shape_88 }, { t: this.shape_87 }, { t: this.shape_86 }, { t: this.shape_85 }, { t: this.shape_84 }, { t: this.shape_83 }, { t: this.shape_82 }, { t: this.shape_81 }, { t: this.shape_80 }, { t: this.shape_79 }, { t: this.shape_78 }, { t: this.shape_77 }, { t: this.shape_76 }, { t: this.shape_75 }, { t: this.shape_74 }, { t: this.shape_73 }, { t: this.shape_72 }, { t: this.shape_71 }, { t: this.shape_70 }, { t: this.shape_69 }, { t: this.shape_68 }, { t: this.shape_67 }, { t: this.shape_66 }, { t: this.shape_65 }, { t: this.shape_64 }, { t: this.shape_63 }, { t: this.shape_62 }, { t: this.shape_61 }, { t: this.shape_60 }, { t: this.shape_59 }, { t: this.shape_58 }, { t: this.shape_57 }, { t: this.shape_56 }, { t: this.shape_55 }, { t: this.shape_54 }, { t: this.shape_53 }, { t: this.shape_52 }, { t: this.shape_51 }, { t: this.shape_50 }, { t: this.shape_49 }, { t: this.shape_48 }, { t: this.shape_47 }, { t: this.shape_46 }, { t: this.shape_45 }, { t: this.shape_44 }, { t: this.shape_43 }, { t: this.shape_42 }, { t: this.shape_41 }, { t: this.shape_40 }, { t: this.shape_39 }, { t: this.shape_38 }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-499.2, -266.4, 847.6, 343.2);


    (lib.ASTA = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#F9B561").s().p("AgBAQQgHgCgEgFQgDgFABgFQABgHAFgEQAFgDAGABQAGABAEAFQAEAFgBAGQgCAGgFAEQgEADgFAAIgBAAg");
        this.shape.setTransform(-7.5, -1.9);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#E35265").s().p("AgDAXQgJgBgGgIQgGgIACgJQABgKAIgFQAIgGAJACQAKABAFAIQAGAIgCAJQgBAJgIAGQgHAFgHAAIgDgBg");
        this.shape_1.setTransform(-7.5, -1.9);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#489AA0").s().p("ABXCXIjMkpIARgMIDaEeIgBAfg");
        this.shape_2.setTransform(-19.2, -17.8);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#489AA0").s().p("ABXCXIjMkpIARgMIDaEeIgBAfg");
        this.shape_3.setTransform(-19.2, -17.8);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#489AA0").s().p("ABXCXIjMkpIARgMIDaEeIgBAfg");
        this.shape_4.setTransform(-19.2, -17.8);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#489AA0").s().p("AhWCqICKlMIAbgQIAIAfIiaFGg");
        this.shape_5.setTransform(-15.4, 15.9);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#489AA0").s().p("AhWCqICKlMIAbgQIAIAfIiaFGg");
        this.shape_6.setTransform(-15.4, 15.9);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#489AA0").s().p("Ai/ARIAWgUIFngdIACAUIllAtg");
        this.shape_7.setTransform(11.8, -3.6);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#489AA0").s().p("Ai/ARIAWgUIFngdIACAUIllAtg");
        this.shape_8.setTransform(11.8, -3.6);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-30.9, -33.7, 62, 67.5);


    (lib.AGUU2 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#0B96CC").s().p("AgwAfIADgSIADgDIADgBQAPgDAOAFIADACQAGAFgFgGIgDgOQABgEADgEQAFgHANgBQAGgBAGAFIAFADIAGgSQACgCAGAAQACAAABAAQABAAABAAQAAAAAAAAQAAABAAAAQgDAEgDALQgCAJgIADQgFACgEgBIgJgGQgGgEgDAEQgDACACAJQACAIgDAEQgCADgFACQgHACgLgFQgGgDgEAAIgDAOQAAACgHACIgEAAQgBAAAAAAQgBAAgBAAQAAAAAAgBQAAAAAAAAg");
        this.shape.setTransform(9.2, 0.2, 1, 1, 28.7);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#0B96CC").s().p("AgvAVIACgNQABgCAKgCQAIgBAKACIAJADIAAgEQgBgIACgCQAEgFAJgCQAKgCAGADIAEACQADAEAAgFIAEgIQACgCAHAAQABgBACAAQABAAAAABQABAAAAAAQAAABgBAAQgCACgBAGQgCAGgCACQgDADgHABQgGAAgFgBIgEgCIgGgDQgBgBAAAAQgBAAAAABQgBAAAAABQAAABgBABQAAACABAEQABAEgCADQgCADgFABQgHADgMgEIgKgDIgCAIQAAADgHABIgFABQAAAAgBgBQAAAAAAAAQgBAAAAAAQAAgBAAAAg");
        this.shape_1.setTransform(-11.3, -0.1, 1, 1, 28.7);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-16.6, -1.6, 31.7, 3.5);


    (lib.agua3 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#0B96CC").s().p("AgvAVIACgNQABgCAEgBIAGgBQALgCAQAFIAAgEQgBgHACgDQADgDAKgDQAKgCAGADQACAAACACQADADAAgFIAEgHQACgCAHAAQAHgBgCACQgDACgBAGQgBAGgDACQgDADgHABQgHAAgEgBIgEgDIgGgCQgCgBgCAEQAAACABAEQABAEgCADQgBACgGACQgHADgMgEIgKgDIgCAIQAAADgHABIgFAAQAAAAgBAAQAAAAgBAAQAAAAAAAAQAAgBAAAAg");
        this.shape.setTransform(16.9, 3.5, 1, 1, 48);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#0B96CC").s().p("AgvAVIADgNQAAgEAKAAQAIgBAKADIAJACIAAgEQgBgIACgCQAEgEAJgCQAKgCAGACIAFADQACADAAgFIAFgIQABgBAHgBQAHgBgCACQgDACgBAGQgBAGgDACQgCACgIACQgHAAgDgBIgFgCIgGgDQgBAAAAAAQgBAAAAAAQgBABAAAAQAAABAAABIAAAHQABAEgCADIgGAEQgIACgMgEIgKgDIgBAJQgBACgHABIgFABQAAAAgBgBQAAAAgBAAQAAAAAAAAQAAgBAAAAg");
        this.shape_1.setTransform(-4.6, -5, 1, 1, 48);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#0B96CC").s().p("AgwAfIADgSIAGgEQAOgDAOAFIAEACIACABIgBgCQgDgMAAgBQABgFADgDQAFgIANgBQAGgBAGAFIAFADIAGgRQABgCAHgBQACAAABAAQABAAABAAQAAAAAAABQAAAAAAABQgDADgDALQgCAIgIADQgFADgEgBIgJgGQgGgEgEAEQgCADACAIQACAJgDADQgDAEgEABQgHACgLgFQgGgDgEAAIgDAOQgBADgHABIgEAAQgBAAAAAAQgBAAAAAAQAAAAAAgBQAAAAAAAAg");
        this.shape_2.setTransform(-23, -11.4, 1, 1, 48);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-28.6, -13.9, 50.4, 20);


    (lib.AGU1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#0B96CC").s().p("AhzA9IAKgpQABgEAQgEQAYgDAMADQAJACAPAKIAJAFQADgIAAgOQABgaAMgJQANgJARgCQAVgCANAKQAIAHADAEIADgGQADgEADgMQADgJAFgGQADgEAOgCQANgBgEAFQgGAGgJAaQgHATgSAFQgLADgJgBQgFgCgDgEQgEgFgDgBQgFgGgKACQgRgKADARQgDAHABAPQACAPgHAHQgGAHgNADQgQAEgWgOQgOgIgLgCIgJAfQgBAGgOACIgIABQgGAAABgDg");
        this.shape.setTransform(78.9, -31.2);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#0B96CC").s().p("AhzA9IAKgpQABgFAQgCQAWgEAOADQAJACAPAKIAJAFQADgIAAgNQABgaAMgJQAMgKASgCQAVgCANAKQAIAHADAEIADgGQADgEADgLQADgKAFgGQAEgEANgBQANgCgEAFQgGAGgJAaQgHATgSAFQgMADgIgBQgFgCgDgEIgHgGQgFgGgKACQgRgLADATQgDAGACAQQABAOgHAHQgGAHgNADQgQAEgWgOQgOgIgLgCIgJAgQgBAFgOACIgIABQgGAAABgDg");
        this.shape_1.setTransform(33.1, -9.9);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#0B96CC").s().p("AhzA9IALgpQABgFAQgCQAVgEAPADQAIACAPAKIAJAFQADgIAAgNQABgaAMgJQAMgKATgCQAUgCANAKQAIAHADAEIADgGQADgEADgLQADgKAFgGQAEgEANgBQANgCgEAFQgGAHgJAaQgHASgSAFQgNADgGgBQgGgCgDgEIgHgGQgEgGgKACQgTgKAEASQgDAGACAQQABAOgHAIQgFAGgOADQgPAEgXgNQgOgIgLgDIgJAgQgBAFgOACIgIABQgGAAABgDg");
        this.shape_2.setTransform(-25.5, 16);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#0B96CC").s().p("AhzA9IAKgpQABgFAQgDQAYgDAMADQAJACAPAKIAJAFQADgIAAgOQABgaAMgJQALgIATgDQAUgDAOALQAIAHADAEIADgGQADgFADgLQADgJAFgGQAEgEANgCQANgBgEAEQgGAHgJAaQgHATgSAFQgNADgHgBQgFgCgDgEQgEgFgDgBQgFgGgKACQgRgKADARQgDAGACAQQABAPgHAHQgFAGgOAEQgQAEgWgOQgOgIgLgCIgJAfQgBAFgOADIgIABQgGAAABgDg");
        this.shape_3.setTransform(-68.3, 35.5);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-79.9, -37.6, 170.5, 79.5);


    (lib.Path_52 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#4B6A96").s().p("AgXCnIAAlNIAuAAIAAFNg");
        this.shape.setTransform(2.4, 16.7);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_52, new cjs.Rectangle(0, 0, 4.7, 33.4), null);


    (lib.Path_51 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#4B6A96").s().p("AgXCMIAAkXIAuAAIAAEXg");
        this.shape.setTransform(2.4, 14);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_51, new cjs.Rectangle(0, 0, 4.7, 28.1), null);


    (lib.Path_50 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#4B6A96").s().p("AgXCMIAAkXIAuAAIAAEXg");
        this.shape.setTransform(2.4, 14);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_50, new cjs.Rectangle(0, 0, 4.7, 28.1), null);


    (lib.Path_49 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_49, new cjs.Rectangle(0, 0, 2.6, 1.5), null);


    (lib.Path_48 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_48, new cjs.Rectangle(0, 0, 2.6, 1.5), null);


    (lib.Path_47 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_47, new cjs.Rectangle(0, 0, 2.7, 1.6), null);


    (lib.Path_46 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_46, new cjs.Rectangle(0, 0, 2.7, 1.5), null);


    (lib.Path_45 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_45, new cjs.Rectangle(0, 0, 2.6, 1.5), null);


    (lib.Path_44 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_44, new cjs.Rectangle(0, 0, 2.6, 1.5), null);


    (lib.Path_43 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_43, new cjs.Rectangle(0, 0, 2.7, 1.5), null);


    (lib.Path_42 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape.setTransform(1.3, 0.8);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_42, new cjs.Rectangle(0, 0, 2.7, 1.5), null);


    (lib.Path_41 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AggAGIAqgYIAXANIgqAYg");
        this.shape.setTransform(3.3, 1.9);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_41, new cjs.Rectangle(0, 0, 6.6, 3.8), null);


    (lib.Path_40 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAegQIAPAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_40, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_39 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_39, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_38 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_38, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_37 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_37, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_36 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_36, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_35 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_35, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_34 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_34, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_33 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_33, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_32 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_32, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_31 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_31, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_30 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_30, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_29 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#D4D5D2").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape.setTransform(2.3, 1.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_29, new cjs.Rectangle(0, 0, 4.6, 2.7), null);


    (lib.Path_28_0 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#CED4DB").s().p("AkkAFIEtitIEcCjIkuCug");
        this.shape.setTransform(29.3, 16.9);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_28_0, new cjs.Rectangle(0, 0, 58.7, 33.9), null);


    (lib.Path_12_0 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#A8A9A7").s().p("AgqABIArgZIAqAYIgrAZg");
        this.shape.setTransform(4.3, 2.5);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_12_0, new cjs.Rectangle(0, 0, 8.6, 5), null);


    (lib.Path_7_0 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#4F5D60").s().p("AhFEiIAApDICLAAIAAJDg");
        this.shape.setTransform(7, 29);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_7_0, new cjs.Rectangle(0, 0, 14, 58), null);


    (lib.Path_2 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#5F595F").s().p("AggAEQALgKARAAIApAAIgCAKQgqAAgdADg");
        this.shape.setTransform(3.7, 0.7);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_2, new cjs.Rectangle(0, 0, 7.4, 1.5), null);


    (lib.Path_1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.lf(["#D3C3C4", "#D3C3C4"], [0.012, 1], -0.1, 0, 0.2, 0).s().p("AAAAXIAAgtIABAAIAAAtg");
        this.shape.setTransform(0.1, 2.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_1, new cjs.Rectangle(0, 0, 0.3, 4.7), null);


    (lib.Path_0 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.lf(["#D3C3C4", "#D3C3C4"], [0.012, 1], -0.2, 0, 0.3, 0).s().p("AgCAPIACgeIADABIgDAdg");
        this.shape.setTransform(0.3, 1.6);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path_0, new cjs.Rectangle(0, 0, 0.5, 3.1), null);


    (lib.Path = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#5F595F").s().p("AABAFIgmgBIABgKIA7ABIAPAMIglgCg");
        this.shape.setTransform(3.8, 0.7);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Path, new cjs.Rectangle(0, 0, 7.6, 1.5), null);


    (lib.Group_100 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.lf(["#D3C3C4", "#D3C3C4"], [0.012, 1], -1.2, 0, 1.3, 0).s().p("AgLAXQAAgJAJgGQAFgFAHgCIAAgXIACAAIAAAZIgBABQgTAGgBANg");
        this.shape.setTransform(1.2, 2.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Group_100, new cjs.Rectangle(0, 0, 2.5, 4.6), null);


    (lib.Group_2_1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.lf(["#D3C3C4", "#D3C3C4"], [0.012, 1], -0.1, 0, 0.2, 0).s().p("AAAAXIAAgtIABAAIAAAtg");
        this.shape.setTransform(0.1, 2.3);

        this.timeline.addTween(cjs.Tween.get(this.shape).wait(1));

    }).prototype = getMCSymbolPrototype(lib.Group_2_1, new cjs.Rectangle(0, 0, 0.3, 4.7), null);


    (lib.ClipGroup_54 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2 (mask)
        var mask = new cjs.Shape();
        mask._off = true;
        mask.graphics.p("AgIAOQgIgCgEgGQgDgGADgEQAEgGAIgCQAIgCAIACQAJACAEAFQADAFgDAFQgFAGgIACIgIABIgIAAg");
        mask.setTransform(3, 2.5);

    }).prototype = getMCSymbolPrototype(lib.ClipGroup_54, null, null);


    (lib.btn_variacion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgPAbQgHgDgEgIQgEgHAAgJQAAgJAEgHQAEgHAHgEQAHgEAIABQAJgBAHAEQAHAEAEAHQAEAHAAAJQAAAJgEAHQgEAIgHADQgHAFgJAAQgIAAgHgFgAgLgOQgEAFAAAJQAAAKAEAFQAEAEAHAAQAHAAAEgEQAFgFAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape.setTransform(30.2, -30);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgdAsIAAhFIgBgRIAOAAIABALQADgGAFgDQAFgDAGAAQAIAAAGAEQAGAEAEAHQADAIAAAJQAAAJgDAGQgDAIgHADQgGAEgIAAQgFAAgFgDQgFgDgDgFIAAAkgAgKgaQgEAFAAAKQAAAJAEAEQAEAFAGAAQAIAAAEgFQAEgEAAgJQAAgJgEgGQgEgGgHAAQgHAAgEAGg");
        this.shape_1.setTransform(23, -28.7);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgHAsIAAg8IAPAAIAAA8gAgIgcIAAgPIAQAAIAAAPg");
        this.shape_2.setTransform(17.6, -31.3);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgbAGIAAgkIAPAAIAAAkQAAAGADAEQACADAGAAQAGAAAEgFQADgEAAgHIAAghIAQAAIAAA8IgPAAIAAgKQgDAFgFADQgEADgGAAQgWAAAAgZg");
        this.shape_3.setTransform(12.5, -29.9);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AAPAsIAAgkQgDAFgFAEQgFACgGAAQgHAAgGgEQgHgDgDgIQgDgGAAgJQAAgJADgIQAEgHAGgEQAGgEAHAAQAHAAAFADQAFADADAFIABgKIAOAAIgBARIAABFgAgLgaQgEAGAAAJQAAAJAEAEQAEAFAHAAQAHAAAFgFQADgEAAgJQAAgKgDgFQgFgGgHAAQgHAAgEAGg");
        this.shape_4.setTransform(5.1, -28.7);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgcArIAAhVIA5AAIAAANIgpAAIAAAXIAmAAIAAALIgmAAIAAAaIApAAIAAAMg");
        this.shape_5.setTransform(-1.9, -31.2);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgSAXQgJgIAAgPQAAgIADgIQAEgHAHgEQAGgDAIAAQAMAAAIAIQAHAHAAAOIAAACIgpAAQABAJAFAGQAEAEAHAAQAKAAAJgGIAEAKQgEAEgGABQgHADgHAAQgNAAgIgJgAgHgRQgEAEgBAHIAbAAQAAgHgEgEQgEgDgGAAQgEAAgEADg");
        this.shape_6.setTransform(-12.3, -30);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgQApQgHgEgDgHQgDgIgBgJQABgJADgHQADgHAGgDQAGgEAIAAQAFAAAGACQAFADADAFIAAglIAOAAIAABYIgOAAIAAgKQgDAFgFADQgFADgGAAQgIAAgFgEgAgKgBQgFAFAAAJQAAAJAFAGQAEAFAGAAQAIAAADgFQAFgFAAgKQAAgJgEgEQgFgGgHAAQgGAAgEAFg");
        this.shape_7.setTransform(-19.5, -31.3);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgPAbQgHgDgEgIQgEgHAAgJQAAgJAEgHQAEgHAHgEQAHgEAIABQAJgBAHAEQAHAEAEAHQAEAHAAAJQAAAJgEAHQgEAIgHADQgHAFgJAAQgIAAgHgFgAgLgOQgEAFAAAJQAAAKAEAFQAEAEAHAAQAHAAAEgEQAFgFAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_8.setTransform(44.3, -48.4);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AgdAsIAAhFIgBgRIAOAAIABALQADgGAFgDQAFgDAGAAQAIAAAGAEQAGAEAEAHQADAIAAAJQAAAJgDAGQgDAIgHADQgGAEgIAAQgFAAgFgDQgFgDgDgFIAAAkgAgKgaQgEAFAAAKQAAAJAEAEQAEAFAGAAQAIAAAEgFQAEgEAAgJQAAgJgEgGQgEgGgHAAQgHAAgEAGg");
        this.shape_9.setTransform(37.1, -47.2);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgGAsIAAg8IAOAAIAAA8gAgHgcIAAgPIAQAAIAAAPg");
        this.shape_10.setTransform(31.7, -49.8);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgHArIAAhIIgcAAIAAgNIBHAAIAAANIgcAAIAABIg");
        this.shape_11.setTransform(26.4, -49.7);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgSAfIAAgrIgBgRIAOAAIABALQACgGAEgDQAFgDAHAAIAGABIAAAOQgEgCgEAAQgIAAgEAEQgDAFAAAHIAAAgg");
        this.shape_12.setTransform(17.4, -48.5);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgPAbQgHgDgEgIQgEgHAAgJQAAgJAEgHQAEgHAHgEQAHgEAIABQAJgBAHAEQAHAEAEAHQAEAHAAAJQAAAJgEAHQgEAIgHADQgHAFgJAAQgIAAgHgFgAgLgOQgEAFAAAJQAAAKAEAFQAEAEAHAAQAHAAAEgEQAFgFAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_13.setTransform(11.2, -48.4);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AgdAsIAAhFIgBgRIAOAAIABALQADgGAFgDQAFgDAGAAQAIAAAGAEQAGAEAEAHQADAIAAAJQAAAJgDAGQgDAIgHADQgGAEgIAAQgFAAgFgDQgFgDgDgFIAAAkgAgKgaQgEAFAAAKQAAAJAEAEQAEAFAGAAQAIAAAEgFQAEgEAAgJQAAgJgEgGQgEgGgHAAQgHAAgEAGg");
        this.shape_14.setTransform(4, -47.2);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgZAYIAFgLQAJAIAMgBQAFAAADgCQADgBAAgDQAAgBAAgBQAAAAAAgBQAAAAgBgBQAAAAgBgBIgHgCIgJgDQgQgDAAgNQAAgFADgEQADgFAFgCQAGgCAGAAQAHAAAGACQAGACAFADIgFAKQgJgGgKAAQgEAAgDABQgDACAAAEQAAAAAAABQABABAAAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAFAAAGQAAAJgHAEQgHAFgLABQgQAAgKgIg");
        this.shape_15.setTransform(-6.2, -48.4);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AgRAbQgGgDgEgIQgDgHAAgJQAAgIADgIQAEgHAHgEQAFgDAIAAQAGAAAFACQAFADADAFIAAgJIAPAAIAAA8IgPAAIAAgKQgDAFgFACQgFAEgGAAQgIAAgGgFgAgLgNQgDAFAAAIQAAAJADAGQAFAEAGAAQAIAAAEgEQAEgGAAgJQAAgJgEgEQgEgGgIAAQgGAAgFAGg");
        this.shape_16.setTransform(-13, -48.4);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgHAsIAAhXIAPAAIAABXg");
        this.shape_17.setTransform(-18.1, -49.8);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgGAsIAAhXIANAAIAABXg");
        this.shape_18.setTransform(-21.2, -49.8);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgRAbQgGgDgDgIQgDgHgBgJQABgIADgIQADgHAHgEQAFgDAIAAQAGAAAFACQAFADADAFIAAgJIAOAAIAAA8IgOAAIAAgKQgDAFgFACQgFAEgGAAQgIAAgGgFgAgKgNQgFAFAAAIQAAAJAFAGQAEAEAGAAQAHAAAFgEQAEgGAAgJQAAgJgEgEQgFgGgHAAQgGAAgEAGg");
        this.shape_19.setTransform(-26.7, -48.4);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgbArIAAhVIA3AAIAAANIgoAAIAAAXIAlAAIAAAMIglAAIAAAlg");
        this.shape_20.setTransform(-33.4, -49.7);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_21.setTransform(4.8, -16.8);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_22.setTransform(4.5, -39.7);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_23.setTransform(19, 19.4);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_24.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_24, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_23, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_24, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_23, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_24, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_23, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_24, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_23, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_valorizacion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgZAYIAFgLQAJAIAMAAQAFAAADgDQADgBAAgDQAAgBAAgBQAAAAAAgBQgBAAAAgBQAAAAgBgBIgHgDIgJgCQgQgEAAgMQAAgGADgDQADgFAFgDQAGgBAGAAQAHAAAGABQAGACAFAEIgFAKQgJgGgKgBQgEAAgDACQgDACAAAEQAAAAAAABQABABAAAAQAAABAAAAQABABAAAAQACACAEABIAJACQAJACAFADQAEAEAAAHQAAAJgHAEQgHAGgLAAQgQgBgKgHg");
        this.shape.setTransform(65.7, -39.7);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgRAbQgGgDgDgIQgDgGgBgKQABgIADgHQADgHAHgFQAFgDAIAAQAGgBAFADQAFADADAFIAAgJIAOAAIAAA8IgOAAIAAgKQgDAFgFADQgFADgGAAQgIAAgGgFgAgKgNQgFAFAAAIQAAAKAFAEQAEAFAGABQAHgBAFgFQAEgEAAgKQAAgJgEgEQgFgGgHAAQgGAAgEAGg");
        this.shape_1.setTransform(58.8, -39.7);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgHAsIAAg7IAPAAIAAA7gAgIgdIAAgOIAQAAIAAAOg");
        this.shape_2.setTransform(53.7, -41);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgMAbQgHgDgDgHQgEgHAAgKQAAgIAEgHQAEgHAHgFQAHgDAJAAQAGAAAGABQAGACAEAEIgEALQgEgDgFgCQgEgCgEAAQgHAAgFAGQgEAEAAAJQAAAKAEAEQAFAFAHABQAEAAAEgCQAFgBAEgEIAEALQgEAEgGACQgGACgHAAQgJAAgHgFg");
        this.shape_3.setTransform(49.3, -39.7);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AAOAfIAAgkQAAgGgDgEQgDgEgFABQgGgBgFAFQgEAFAAAGIAAAiIgPAAIAAgrIgBgQIAPAAIABAKQADgGAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_4.setTransform(42.4, -39.7);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgTAXQgIgIAAgPQAAgIAEgHQADgHAHgFQAGgDAIAAQANgBAHAJQAHAHAAAOIAAACIgoAAQAAAKAFAEQADAFAJAAQAKAAAIgGIAEAKQgEAEgHACQgGACgGAAQgOAAgJgJgAgHgRQgEAEgBAHIAbAAQgBgHgDgEQgEgDgFAAQgGAAgDADg");
        this.shape_5.setTransform(35.6, -39.7);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgSAfIAAgrIgBgQIAOAAIABAKQADgGAEgDQAEgDAHAAIAGABIAAANQgEgBgEAAQgIAAgEAEQgDAFAAAHIAAAgg");
        this.shape_6.setTransform(30.3, -39.7);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgSAXQgJgIAAgPQAAgIADgHQAEgHAHgFQAGgDAIAAQAMgBAIAJQAHAHAAAOIAAACIgpAAQABAKAEAEQAFAFAHAAQAKAAAJgGIAEAKQgEAEgGACQgHACgHAAQgNAAgIgJgAgHgRQgEAEgBAHIAbAAQgBgHgDgEQgEgDgGAAQgEAAgEADg");
        this.shape_7.setTransform(24.3, -39.7);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgJAtIAAgxIgLAAIAAgLIALAAIAAgGQABgLAFgFQAFgHAKABQAGgBADACIAAAMIgGgBQgKAAABALIAAAFIANAAIAAALIgNAAIAAAxg");
        this.shape_8.setTransform(18.7, -41.1);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AgZAYIAFgLQAJAIAMAAQAFAAADgDQADgBAAgDQAAgBAAgBQAAAAAAgBQAAAAgBgBQAAAAgBgBIgHgDIgJgCQgQgEAAgMQAAgGADgDQADgFAFgDQAGgBAGAAQAHAAAGABQAGACAFAEIgFAKQgJgGgKgBQgEAAgDACQgDACAAAEQAAAAAAABQAAABABAAQAAABAAAAQABABAAAAQACACAEABIAJACQAJACAFADQAEAEAAAHQAAAJgHAEQgHAGgLAAQgQgBgKgHg");
        this.shape_9.setTransform(13.5, -39.7);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AAOAfIAAgkQAAgGgDgEQgDgEgFABQgGgBgFAFQgEAFAAAGIAAAiIgPAAIAAgrIgBgQIAPAAIABAKQADgGAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_10.setTransform(6.9, -39.7);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgRAbQgGgDgDgIQgDgGgBgKQABgIADgHQADgHAHgFQAFgDAIAAQAGgBAFADQAFADADAFIAAgJIAOAAIAAA8IgOAAIAAgKQgDAFgFADQgFADgGAAQgIAAgGgFgAgKgNQgFAFAAAIQAAAKAFAEQAEAFAGABQAHgBAFgFQAEgEAAgKQAAgJgEgEQgFgGgHAAQgGAAgEAGg");
        this.shape_11.setTransform(-0.6, -39.7);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgSAfIAAgrIgBgQIAPAAIABAKQACgGADgDQAGgDAGAAIAGABIgBANQgDgBgEAAQgIAAgEAEQgDAFAAAHIAAAgg");
        this.shape_12.setTransform(-6.1, -39.7);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgHArIAAhIIgcAAIAAgNIBHAAIAAANIgcAAIAABIg");
        this.shape_13.setTransform(-12.6, -40.9);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AgSAXQgJgIAAgPQAAgIADgHQAEgHAHgFQAHgDAHAAQAMgBAIAJQAHAHAAAOIAAACIgpAAQABAKAFAEQAEAFAHAAQAKAAAJgGIAEAKQgEAEgGACQgHACgHAAQgNAAgIgJgAgHgRQgEAEgBAHIAbAAQAAgHgEgEQgEgDgFAAQgFAAgEADg");
        this.shape_14.setTransform(67.3, -58.1);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgQApQgHgEgDgHQgEgIAAgJQAAgJAEgHQADgHAGgDQAHgEAHAAQAFAAAGACQAFADACAFIAAglIAPAAIAABYIgOAAIAAgKQgDAFgFADQgFADgGAAQgIAAgFgEgAgLgBQgEAFABAJQgBAJAEAGQAFAFAGAAQAIAAAEgFQAEgFAAgKQAAgJgEgEQgFgGgHAAQgGAAgFAFg");
        this.shape_15.setTransform(60.1, -59.5);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AAOAfIAAgkQAAgGgDgEQgDgEgFABQgGgBgFAFQgEAFAAAGIAAAiIgPAAIAAgrIgBgQIAPAAIABAKQADgGAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_16.setTransform(49.5, -58.2);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgPArQgHgDgEgIQgEgHAAgJQAAgKAEgHQAEgHAHgDQAHgEAIAAQAJAAAHAEQAHADAEAHQAEAHAAAKQAAAJgEAHQgEAIgHADQgHAEgJAAQgIAAgHgEgAgLAAQgEAGAAAKQAAAKAEAEQAEAFAHAAQAHAAAEgFQAFgEAAgKQAAgKgFgGQgEgDgHAAQgHAAgEADgAgEgVIALgZIAPAAIgRAZg");
        this.shape_17.setTransform(42.4, -59.7);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgHAsIAAg7IAPAAIAAA7gAgHgdIAAgOIAQAAIAAAOg");
        this.shape_18.setTransform(37.3, -59.5);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgMAbQgHgDgDgHQgEgHAAgKQAAgIAEgHQAEgHAHgFQAHgDAJAAQAGAAAGABQAGACAEAEIgEALQgEgDgFgCQgEgCgEAAQgHAAgFAGQgEAEAAAJQAAAKAEAEQAFAFAHABQAEAAAEgCQAFgBAEgEIAEALQgEAEgGACQgGACgHAAQgJAAgHgFg");
        this.shape_19.setTransform(32.9, -58.1);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgRAbQgGgDgEgIQgCgGAAgKQAAgIACgHQAEgHAGgFQAHgDAHAAQAGgBAFADQAFADADAFIAAgJIAPAAIAAA8IgPAAIAAgKQgDAFgFADQgFADgGAAQgHAAgHgFgAgKgNQgEAFgBAIQABAKAEAEQADAFAHABQAHgBAFgFQAEgEAAgKQAAgJgEgEQgFgGgHAAQgHAAgDAGg");
        this.shape_20.setTransform(25.8, -58.1);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#005495").s().p("AgZAeIAAgKIAggmIggAAIAAgLIAyAAIAAAKIghAmIAiAAIAAALg");
        this.shape_21.setTransform(19.2, -58.1);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#005495").s().p("AgHAsIAAg7IAPAAIAAA7gAgIgdIAAgOIARAAIAAAOg");
        this.shape_22.setTransform(14.5, -59.5);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#005495").s().p("AgSAfIAAgrIgBgQIAOAAIABAKQADgGAEgDQAEgDAHAAIAGABIAAANQgEgBgEAAQgIAAgEAEQgDAFAAAHIAAAgg");
        this.shape_23.setTransform(10.9, -58.2);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#005495").s().p("AgPAbQgHgDgEgIQgEgGAAgKQAAgIAEgIQAEgHAHgEQAHgDAIAAQAJAAAHADQAHAEAEAHQAEAIAAAIQAAAKgEAGQgEAIgHADQgHAFgJAAQgIAAgHgFgAgLgOQgEAFAAAJQAAAKAEAEQAEAFAHABQAHgBAEgFQAFgEAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_24.setTransform(4.7, -58.1);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#005495").s().p("AgGAsIAAhXIANAAIAABXg");
        this.shape_25.setTransform(-0.4, -59.5);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#005495").s().p("AgRAbQgGgDgDgIQgDgGAAgKQAAgIADgHQADgHAGgFQAGgDAIAAQAGgBAFADQAFADADAFIAAgJIAOAAIAAA8IgOAAIAAgKQgDAFgFADQgFADgGAAQgIAAgGgFgAgKgNQgFAFAAAIQAAAKAFAEQAEAFAGABQAHgBAFgFQAEgEAAgKQAAgJgEgEQgFgGgHAAQgGAAgEAGg");
        this.shape_26.setTransform(-5.9, -58.1);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#005495").s().p("AgFArIgmhVIAQAAIAbBBIAdhBIAPAAIgmBVg");
        this.shape_27.setTransform(-13.8, -59.4);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_28.setTransform(25.8, -25.3);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_29.setTransform(25.5, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_30.setTransform(19, 19.4);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_31.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_31, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_30, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_31, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_30, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_31, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_30, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_31, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_30, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_transmicion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AAOAfIAAgjQAAgIgDgDQgDgDgFgBQgGABgFAEQgEAEAAAIIAAAhIgPAAIAAgqIgBgSIAPAAIABAKQADgFAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape.setTransform(50.4, -39.8);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgPArQgHgEgEgHQgEgHAAgKQAAgJAEgHQAEgGAHgFQAHgDAIAAQAJAAAHADQAHAFAEAGQAEAHAAAJQAAAKgEAHQgEAHgHAEQgHAEgJAAQgIAAgHgEgAgLABQgEAFAAAJQAAALAEAEQAEAFAHAAQAHAAAEgFQAFgEAAgLQAAgJgFgFQgEgFgHAAQgHAAgEAFgAgEgWIALgYIAPAAIgRAYg");
        this.shape_1.setTransform(43.3, -41.3);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgGAsIAAg8IANAAIAAA8gAgIgcIAAgPIAQAAIAAAPg");
        this.shape_2.setTransform(38.2, -41.1);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgZAYIAFgLQAJAHAMAAQAFAAADgBQADgCAAgEQAAAAAAgBQAAAAAAgBQAAAAgBgBQAAgBgBAAIgHgCIgJgDQgQgDAAgNQAAgGADgEQADgEAFgCQAGgDAGAAQAHAAAGADQAGABAFAEIgFAKQgJgHgKABQgEAAgDACQgDABAAADQAAABAAABQAAABABAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAEAAAHQAAAIgHAGQgHAEgLAAQgQABgKgIg");
        this.shape_3.setTransform(33.6, -39.7);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AgHAsIAAg8IAOAAIAAA8gAgIgcIAAgPIAQAAIAAAPg");
        this.shape_4.setTransform(29.1, -41.1);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AAfAfIAAgjQAAgIgCgDQgCgDgFgBQgHABgDAEQgEAEAAAIIAAAhIgOAAIAAgjQAAgIgDgDQgBgDgGgBQgGABgEAEQgDAEgBAIIAAAhIgPAAIAAgqIgBgSIAPAAIABAKQADgGAFgDQAEgCAHAAQANAAAEALQADgFAFgDQAFgDAHAAQALAAAEAGQAFAGABANIAAAkg");
        this.shape_5.setTransform(22.1, -39.8);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgZAYIAFgLQAJAHAMAAQAFAAADgBQADgCAAgEQAAAAAAgBQAAAAAAgBQgBAAAAgBQAAgBgBAAIgHgCIgJgDQgQgDAAgNQAAgGADgEQADgEAFgCQAGgDAGAAQAHAAAGADQAGABAFAEIgFAKQgJgHgKABQgEAAgDACQgDABAAADQAAABAAABQAAABABAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAEAAAHQAAAIgHAGQgHAEgLAAQgQABgKgIg");
        this.shape_6.setTransform(13.7, -39.7);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AAOAfIAAgjQAAgIgDgDQgDgDgFgBQgGABgFAEQgEAEAAAIIAAAhIgPAAIAAgqIgBgSIAPAAIABAKQADgFAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_7.setTransform(7, -39.8);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgRAcQgGgEgEgHQgDgHAAgJQAAgJADgIQAEgHAHgDQAFgFAIAAQAGAAAFADQAFADADAFIAAgJIAPAAIAAA7IgPAAIAAgJQgDAFgFACQgFADgGAAQgIAAgGgDgAgLgOQgDAGAAAJQAAAJADAEQAFAGAGgBQAIABAEgGQAEgEAAgKQAAgIgEgGQgEgFgIAAQgGAAgFAFg");
        this.shape_8.setTransform(-0.4, -39.7);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AgSAfIAAgqIgBgSIAOAAIABALQACgGAEgDQAFgDAHAAIAGABIgBANQgDgBgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_9.setTransform(-6, -39.8);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgHArIAAhIIgcAAIAAgNIBHAAIAAANIgcAAIAABIg");
        this.shape_10.setTransform(-12.4, -41);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgSAXQgJgJAAgOQAAgIADgIQAEgHAHgDQAHgFAHAAQAMABAIAHQAHAJAAAOIAAABIgoAAQAAAJAEAGQAFAEAIAAQAJAAAJgHIAEALQgEADgHACQgGACgGAAQgOABgIgJgAgHgQQgEADgBAHIAbAAQgBgHgDgDQgDgEgHAAQgEAAgEAEg");
        this.shape_11.setTransform(42.9, -58.2);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgRApQgGgEgEgHQgCgIAAgJQAAgJACgHQAEgHAGgDQAHgEAHAAQAGAAAFACQAFADADAFIAAglIAPAAIAABYIgPAAIAAgKQgDAFgFADQgFADgGAAQgHAAgHgEgAgKgBQgEAFgBAJQABAJAEAGQADAFAHAAQAHAAAEgFQAFgFAAgKQAAgJgEgEQgFgGgHAAQgHAAgDAFg");
        this.shape_12.setTransform(35.7, -59.5);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgZAYIAFgLQAJAHAMAAQAFAAADgBQADgCAAgEQAAAAAAgBQAAAAAAgBQgBAAAAgBQAAgBgBAAIgHgCIgJgDQgQgDAAgNQAAgGADgEQADgEAFgCQAGgDAGAAQAHAAAGADQAGABAFAEIgFAKQgJgHgKABQgEAAgDACQgDABAAADQAAABAAABQAAABABAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAEAAAHQAAAIgHAGQgHAEgLAAQgQABgKgIg");
        this.shape_13.setTransform(25.8, -58.2);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AgRAcQgGgEgEgHQgCgHAAgJQAAgJACgIQAEgHAGgDQAHgFAHAAQAGAAAFADQAFADADAFIAAgJIAOAAIAAA7IgOAAIAAgJQgDAFgFACQgFADgGAAQgHAAgHgDgAgKgOQgFAGAAAJQAAAJAFAEQADAGAHgBQAIABAEgGQAEgEAAgKQAAgIgEgGQgEgFgIAAQgHAAgDAFg");
        this.shape_14.setTransform(19, -58.2);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgSAXQgJgJAAgOQAAgIADgIQAEgHAHgDQAGgFAIAAQAMABAIAHQAHAJAAAOIAAABIgpAAQABAJAEAGQAFAEAHAAQAKAAAJgHIAEALQgEADgGACQgHACgHAAQgNABgIgJgAgHgQQgEADgBAHIAbAAQgBgHgDgDQgEgEgGAAQgEAAgEAEg");
        this.shape_15.setTransform(12.1, -58.2);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AAOAfIAAgjQAAgIgDgDQgDgDgFgBQgGABgFAEQgEAEAAAIIAAAhIgPAAIAAgqIgBgSIAPAAIABAKQADgFAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_16.setTransform(5.1, -58.2);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgOAuIAAg7IAOAAIAAA7gAgMgVIAMgYIAPAAIgQAYg");
        this.shape_17.setTransform(0.8, -59.8);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgaArIAAhVIAPAAIAABIIAmAAIAAANg");
        this.shape_18.setTransform(-4.5, -59.4);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_19.setTransform(18.5, -25.3);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_20.setTransform(18.3, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_21.setTransform(19, 19.4);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_22.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_22, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_21, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_22, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_21, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_22, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_21, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_22, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_21, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_termo = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgRAcQgGgEgEgHQgCgIAAgIQAAgJACgHQAEgIAGgDQAHgFAHAAQAGABAFACQAFADADAFIAAgJIAPAAIAAA7IgPAAIAAgJQgDAFgFADQgFACgGAAQgIAAgGgDgAgLgOQgDAGAAAJQAAAIADAFQAEAGAHAAQAHAAAFgGQAEgEAAgKQAAgIgEgGQgFgFgHAAQgHAAgEAFg");
        this.shape.setTransform(56.1, -38.5);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgMAcQgHgEgDgHQgEgHAAgJQAAgJAEgHQAEgIAHgDQAHgFAJAAQAGAAAGACQAGADAEADIgEAKQgEgDgFgBQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAKAEAEQAFAGAHAAQAEgBAEgBQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgDg");
        this.shape_1.setTransform(49.6, -38.5);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgGAsIAAg7IANAAIAAA7gAgIgdIAAgOIAQAAIAAAOg");
        this.shape_2.setTransform(44.9, -39.8);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgSAfIAAgqIgBgRIAOAAIABAKQADgGADgDQAGgDAGAAIAGABIgBANQgDgBgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_3.setTransform(41.4, -38.5);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AgDAjQgGgGAAgLIAAgbIgMAAIAAgLIAMAAIAAgPIAOgFIAAAUIAQAAIAAALIgQAAIAAAbQAAAKAKAAIAGgBIAAAMQgEACgGAAQgKAAgEgGg");
        this.shape_4.setTransform(36.3, -39.4);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgMAcQgHgEgDgHQgEgHAAgJQAAgJAEgHQAEgIAHgDQAHgFAJAAQAGAAAGACQAGADAEADIgEAKQgEgDgFgBQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAKAEAEQAFAGAHAAQAEgBAEgBQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgDg");
        this.shape_5.setTransform(31.2, -38.5);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgTAnQgIgJAAgPQAAgIAEgHQADgHAHgEQAGgEAIAAQANAAAHAIQAHAHAAAOIAAACIgpAAQABAKAFAFQADAFAIgBQALABAIgHIAEALQgEADgHACQgGACgGAAQgOAAgJgIgAgHgBQgEADgBAGIAbAAQgBgGgDgDQgEgDgFAAQgGAAgDADgAgDgWIALgYIAPAAIgRAYg");
        this.shape_6.setTransform(24.8, -40);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgGAsIAAhXIANAAIAABXg");
        this.shape_7.setTransform(19.9, -39.9);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgSAXQgJgJAAgOQAAgIADgHQAEgIAHgDQAHgFAHAAQAMABAIAHQAHAJAAAOIAAABIgpAAQABAKAFAEQAEAFAHAAQAKAAAJgHIAEALQgEADgGACQgHACgHAAQgNAAgIgIgAgHgQQgEADgBAHIAbAAQAAgHgEgDQgEgEgFAAQgFAAgEAEg");
        this.shape_8.setTransform(15.1, -38.5);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AgPAcQgHgEgEgHQgEgIAAgJQAAgIAEgIQAEgHAHgEQAHgDAIgBQAJABAHADQAHAEAEAHQAEAIAAAIQAAAJgEAIQgEAHgHAEQgHADgJAAQgIAAgHgDgAgLgOQgEAFAAAJQAAAKAEAEQAEAGAHAAQAHAAAEgGQAFgEAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_9.setTransform(8.2, -38.5);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AAfAfIAAgjQABgIgDgDQgCgDgFgBQgGABgEAEQgEAEAAAIIAAAhIgOAAIAAgjQAAgIgCgDQgDgDgFgBQgHABgDAEQgDAEgBAIIAAAhIgOAAIAAgqIgCgRIAPAAIABAJQADgGAEgDQAFgCAHAAQANAAAEALQADgFAFgDQAGgDAGAAQALAAAEAGQAGAGAAANIAAAkg");
        this.shape_10.setTransform(-0.8, -38.5);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgSAfIAAgqIgBgRIAOAAIABAKQADgGADgDQAGgDAGAAIAGABIgBANQgDgBgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_11.setTransform(-8.1, -38.5);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgTAXQgIgJAAgOQAAgIAEgHQADgIAHgDQAHgFAHAAQANABAHAHQAHAJAAAOIAAABIgoAAQAAAKAEAEQAFAFAIAAQAKAAAIgHIAEALQgEADgHACQgGACgGAAQgOAAgJgIgAgHgQQgEADgBAHIAbAAQgBgHgDgDQgDgEgHAAQgFAAgDAEg");
        this.shape_12.setTransform(-14.1, -38.5);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgHArIAAhIIgcAAIAAgNIBHAAIAAANIgcAAIAABIg");
        this.shape_13.setTransform(-21.2, -39.7);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AAOAfIAAgjQAAgIgDgDQgDgDgFgBQgGABgFAEQgEAEAAAIIAAAhIgPAAIAAgqIgBgRIAPAAIABAJQADgFAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_14.setTransform(45.8, -57);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgPArQgHgDgEgIQgEgHAAgKQAAgJAEgHQAEgHAHgEQAHgDAIAAQAJAAAHADQAHAEAEAHQAEAHAAAJQAAAKgEAHQgEAIgHADQgHAEgJAAQgIAAgHgEgAgLABQgEAEAAAKQAAALAEAEQAEAFAHAAQAHAAAEgFQAFgEAAgLQAAgKgFgEQgEgFgHAAQgHAAgEAFgAgEgWIALgYIAPAAIgRAYg");
        this.shape_15.setTransform(38.7, -58.5);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AgHAsIAAg7IAPAAIAAA7gAgHgdIAAgOIAQAAIAAAOg");
        this.shape_16.setTransform(33.6, -58.3);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgMAcQgHgEgDgHQgEgHAAgJQAAgJAEgHQAEgIAHgDQAHgFAJAAQAGAAAGACQAGADAEADIgEAKQgEgDgFgBQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAKAEAEQAFAGAHAAQAEgBAEgBQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgDg");
        this.shape_17.setTransform(29.2, -56.9);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgMAcQgHgEgDgHQgEgHAAgJQAAgJAEgHQAEgIAHgDQAHgFAJAAQAGAAAGACQAGADAEADIgEAKQgEgDgFgBQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAKAEAEQAFAGAHAAQAEgBAEgBQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgDg");
        this.shape_18.setTransform(23.1, -56.9);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgbAGIAAgkIAPAAIAAAkQAAAGADAEQACADAGAAQAGAAAEgFQADgEAAgHIAAghIAQAAIAAA8IgPAAIAAgKQgDAFgFADQgEADgGAAQgWAAAAgZg");
        this.shape_19.setTransform(16.4, -56.9);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgRApQgGgEgEgHQgDgIAAgJQAAgJADgHQAEgHAGgDQAGgEAIAAQAGAAAEACQAGADACAFIAAglIAQAAIAABYIgPAAIAAgKQgDAFgFADQgFADgGAAQgHAAgHgEgAgLgBQgDAFAAAJQAAAJADAGQAEAFAHAAQAHAAAFgFQAEgFAAgKQAAgJgEgEQgFgGgHAAQgHAAgEAFg");
        this.shape_20.setTransform(8.9, -58.3);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#005495").s().p("AgPAcQgHgEgEgHQgEgIAAgJQAAgIAEgIQAEgHAHgEQAHgDAIgBQAJABAHADQAHAEAEAHQAEAIAAAIQAAAJgEAIQgEAHgHAEQgHADgJAAQgIAAgHgDgAgLgOQgEAFAAAJQAAAKAEAEQAEAGAHAAQAHAAAEgGQAFgEAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_21.setTransform(1.8, -56.9);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#005495").s().p("AgSAfIAAgqIgBgRIAOAAIABAKQACgGAEgDQAFgDAHAAIAGABIgBANQgDgBgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_22.setTransform(-3.7, -57);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#005495").s().p("AggArIAAhVIAkAAQANAAAIAHQAIAHAAALQAAAMgIAFQgHAHgOAAIgVAAIAAAkgAgRgEIATAAQAIAAAEgDQAFgEgBgGQAAgGgDgDQgFgEgIAAIgTAAg");
        this.shape_23.setTransform(-10.2, -58.2);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_24.setTransform(18.5, -25.3);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_25.setTransform(18.3, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_26.setTransform(19, 19.4);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_27.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_27, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_26, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_27, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_26, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_27, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_26, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_27, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_26, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_renovables = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgUAUIADgJQAIAGAKAAQAEAAADgCQAAAAABAAQAAgBABAAQAAgBAAAAQAAgBAAAAQAAgBAAgBQAAAAAAgBQAAAAgBgBQAAAAgBgBIgFgCIgIgBQgOgDAAgLQAAgEADgEQADgEAEgBQAFgCAFAAQAGAAAFACQAFABADADIgDAIQgIgFgIAAQgDAAgDACQAAAAgBAAQAAABAAAAQAAABgBABQAAAAAAABQAAABAAAAQAAAAABABQAAAAAAABQAAAAABABQAAAAAAAAQABABAAAAQABAAABAAQAAAAABABIAIABQAIACADADQADADAAAGQAAAHgFAEQgGAFgJgBQgNABgIgHg");
        this.shape.setTransform(44.1, -38.7);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgPAUQgIgIAAgMQAAgHADgGQAEgFAFgEQAGgDAGAAQAKAAAHAGQAFAHAAAMIAAABIgiAAQABAIAEAEQAEAEAFgBQAJAAAHgEIADAIQgDADgFABQgGACgFAAQgMAAgGgGgAgGgNQgDADgBAFIAWAAQAAgFgDgDQgDgDgEAAQgEAAgEADg");
        this.shape_1.setTransform(38.9, -38.7);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgFAlIAAhJIALAAIAABJg");
        this.shape_2.setTransform(34.9, -39.8);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgGAjQgEgDgDgEIAAAIIgLAAIAAhIIALAAIAAAeQADgEAFgCQAEgDAEAAQAGAAAGAEQAFADACAGQADAFAAAIQAAAHgDAGQgDAGgFAEQgEADgHAAQgEAAgFgCgAgJAAQgDADgBAIQABAIADAEQAEAEAFAAQAGAAAEgEQADgFAAgHQAAgIgDgDQgEgFgGAAQgGAAgDAFg");
        this.shape_3.setTransform(30.6, -39.8);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AgOAXQgFgDgCgGQgDgGgBgHQABgIADgGQACgFAGgEQAFgDAGAAQAFAAADACQAFADADAEIAAgIIALAAIAAAxIgLAAIAAgIQgDAEgFADQgDACgFAAQgGAAgGgDgAgJgLQgDAEAAAIQAAAHADAFQAEADAFAAQAHABADgFQADgEABgIQgBgHgDgEQgDgFgHABQgFgBgEAFg");
        this.shape_4.setTransform(24.1, -38.7);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgFAZIgVgxIANAAIANAjIAPgjIAMAAIgWAxg");
        this.shape_5.setTransform(18.4, -38.7);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgNAXQgFgDgEgGQgCgGAAgIQAAgHACgGQAEgGAFgDQAHgDAGAAQAIAAAFADQAHADACAGQADAGABAHQgBAIgDAGQgCAGgHADQgFADgIAAQgGAAgHgDgAgIgMQgEAFAAAHQAAAIAEAFQADADAFAAQAHAAADgDQADgFAAgIQAAgHgDgFQgDgDgHAAQgFAAgDADg");
        this.shape_6.setTransform(12.8, -38.7);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AALAaIAAgeQAAgGgCgCQgDgDgEAAQgFAAgEAEQgDADAAAHIAAAbIgMAAIAAgjIgBgPIAMAAIABAJQACgFAFgDQADgCAGAAQASAAAAAVIAAAeg");
        this.shape_7.setTransform(6.8, -38.7);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgQAUQgGgIAAgMQAAgHADgGQACgFAHgEQAFgDAGAAQALAAAFAGQAHAHAAAMIAAABIgiAAQAAAIAEAEQADAEAHgBQAIAAAHgEIAEAIQgEADgFABQgGACgFAAQgLAAgIgGgAgGgNQgDADgBAFIAXAAQgBgFgDgDQgDgDgFAAQgEAAgDADg");
        this.shape_8.setTransform(1.1, -38.7);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AAQAjIgJgXQgBgDgDgCIgFgBIgOAAIAAAdIgMAAIAAhGIAdAAQAMABAGAEQAHAGAAAKQAAAHgEAFQgDAEgIACQAHABACAIIAJAWgAgQgDIAQAAQAGAAAEgDQADgCAAgGQAAgFgDgDQgDgCgHAAIgQAAg");
        this.shape_9.setTransform(-5, -39.7);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgUAUIADgJQAIAGAKAAQAEAAADgBQAAgBABAAQAAgBABAAQAAgBAAAAQAAgBAAgBQAAAAAAgBQAAAAAAgBQAAAAgBgBQAAAAgBAAIgFgCIgIgDQgOgCAAgKQAAgFADgEQADgEAEgCQAFgCAFAAQAGAAAFACQAFACADADIgDAJQgIgGgIAAQgDAAgDABQAAABgBAAQAAABAAAAQgBABAAABQAAAAAAABQAAAAAAABQAAAAABABQAAAAAAABQAAAAABAAQAAABAAAAQABAAAAABQABAAABAAQAAAAABAAIAIADQAIACADACQADADAAAGQAAAHgFAEQgGAEgJAAQgNAAgIgGg");
        this.shape_10.setTransform(65.1, -54.4);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgOAXQgFgDgDgGQgCgGAAgIQAAgGACgGQADgHAGgDQAEgDAHgBQAEABAEACQAFADACADIAAgHIAMAAIAAAyIgMAAIAAgJQgCAEgFADQgEACgEAAQgGAAgGgDgAgJgLQgDAFAAAGQAAAIADAEQAEAFAGAAQAGgBADgEQADgEAAgIQAAgHgDgEQgEgEgGgBQgFABgEAEg");
        this.shape_11.setTransform(59.4, -54.4);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgLAnIAAgyIALAAIAAAygAgJgRIAJgVIAMAAIgNAVg");
        this.shape_12.setTransform(55.8, -55.7);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgTAgIACgJQAJAFAIAAQANAAAAgOIAAgKQgCAEgEADQgFACgEAAQgHAAgFgDQgFgDgDgGQgDgEAAgIQAAgHADgGQADgGAFgDQAFgDAHAAQAEAAAFACQAEADACAEIAAgIIANAAIAAAwQAAAMgHAGQgGAGgNAAQgKAAgJgFgAgJgWQgDAEAAAHQAAAHADAEQAEAEAFAAQAGAAAEgEQADgEAAgHQAAgHgDgEQgEgEgGAAQgFAAgEAEg");
        this.shape_13.setTransform(50.6, -53.3);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AgPAaIAAgjIgBgPIAMAAIABAJQACgFADgCQAEgDAFAAIAGABIAAALIgHgBQgHAAgDAEQgDADABAGIAAAbg");
        this.shape_14.setTransform(46, -54.4);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgPATQgIgHABgMQAAgHACgFQADgHAGgDQAGgDAGgBQALAAAFAIQAHAGgBAMIAAABIgiAAQABAIAEAEQADADAHABQAIAAAHgGIAEAJQgEADgFACQgFABgGAAQgMABgGgIgAgGgOQgDADgBAGIAXAAQgBgGgDgDQgCgDgFAAQgFAAgDADg");
        this.shape_15.setTransform(41, -54.4);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AALAaIAAgeQAAgFgCgDQgDgDgEAAQgFAAgEAEQgDAEAAAFIAAAcIgMAAIAAgjIgBgOIAMAAIABAIQACgFAFgCQADgDAGAAQASAAAAAVIAAAeg");
        this.shape_16.setTransform(35.2, -54.4);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgXAkIAAhHIAvAAIAAAKIgiAAIAAAUIAfAAIAAAJIgfAAIAAAWIAiAAIAAAKg");
        this.shape_17.setTransform(29.4, -55.4);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AALAaIAAgeQAAgFgCgDQgDgDgEAAQgFAAgEAEQgDAEAAAFIAAAcIgMAAIAAgjIgBgOIAMAAIABAIQACgFAFgCQADgDAGAAQASAAAAAVIAAAeg");
        this.shape_18.setTransform(20.4, -54.4);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgNAkQgFgDgEgGQgCgGAAgIQAAgIACgFQAEgGAFgDQAHgEAGAAQAIAAAFAEQAHADACAGQADAFAAAIQAAAIgDAGQgCAGgHADQgFADgIAAQgGAAgHgDgAgIAAQgEAFAAAIQAAAIAEAEQADAEAFAAQAHAAADgEQADgEAAgIQAAgIgDgFQgDgDgHAAQgFAAgDADgAgDgSIAJgUIANAAIgPAUg");
        this.shape_19.setTransform(14.5, -55.7);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgFAlIAAgyIALAAIAAAygAgGgXIAAgNIANAAIAAANg");
        this.shape_20.setTransform(10.3, -55.5);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#005495").s().p("AgJAXQgGgDgDgGQgDgGAAgIQAAgGADgGQADgHAGgDQAGgDAHgBQAFAAAFACQAFACADADIgDAJIgHgEIgHgCQgGABgEAEQgDAEAAAHQAAAIADAEQAEAFAGAAIAHgCIAHgEIADAJQgDADgFACQgFABgGAAQgGAAgGgDg");
        this.shape_21.setTransform(6.5, -54.4);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#005495").s().p("AgJAXQgGgDgDgGQgDgGAAgIQAAgGADgGQADgHAGgDQAGgDAHgBQAFAAAFACQAFACADADIgDAJIgHgEIgHgCQgGABgEAEQgDAEAAAHQAAAIADAEQAEAFAGAAIAHgCIAHgEIADAJQgDADgFACQgFABgGAAQgGAAgGgDg");
        this.shape_22.setTransform(1.5, -54.4);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#005495").s().p("AgWAFIAAgeIAMAAIAAAeQAAAFACADQADADAEAAQAFAAADgEQADgEAAgGIAAgbIANAAIAAAyIgMAAIAAgIQgDAEgEACQgEADgEAAQgSAAAAgVg");
        this.shape_23.setTransform(-4.1, -54.3);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#005495").s().p("AgNAiQgGgEgDgGQgCgGAAgHQAAgIACgFQADgGAFgDQAGgEAGAAQAEAAAEADQAFACACAEIAAgeIAMAAIAABIIgMAAIAAgIQgCAEgFADQgEACgEAAQgHAAgEgDgAgJAAQgDADAAAIQAAAHADAFQAEAEAFAAQAGAAAEgEQADgEAAgIQAAgIgDgDQgDgFgGAAQgGAAgEAFg");
        this.shape_24.setTransform(-10.3, -55.5);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#005495").s().p("AgNAXQgFgDgEgGQgCgGAAgIQAAgHACgGQAEgGAFgDQAGgDAHgBQAIABAFADQAGADADAGQADAGAAAHQAAAIgDAGQgDAGgGADQgFADgIAAQgHAAgGgDgAgJgLQgDAEAAAHQAAAIADAEQAEAFAFAAQAGAAAEgFQADgEAAgIQAAgHgDgEQgEgFgGAAQgFAAgEAFg");
        this.shape_25.setTransform(-16.2, -54.4);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#005495").s().p("AgOAaIAAgjIgBgPIALAAIABAJQACgFADgCQAEgDAGAAIAEABIAAALIgGgBQgGAAgEAEQgCADgBAGIAAAbg");
        this.shape_26.setTransform(-20.8, -54.4);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#005495").s().p("AgbAkIAAhHIAeAAQAMABAGAFQAHAGAAAJQAAAJgHAFQgGAGgMAAIgRAAIAAAegAgOgDIAQAAQAHAAADgDQADgDAAgFQAAgFgDgDQgDgCgHgBIgQAAg");
        this.shape_27.setTransform(-26.2, -55.4);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_28.setTransform(18.5, -25.3);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_29.setTransform(18.3, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_30.setTransform(19, 19.4);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_31.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_31, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_30, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_31, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_30, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_31, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_30, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_31, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_30, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_regresar = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#FFFFFF").s().p("AhXC9QhNgkgahRIAxgPQATA6A4AbQA4AbA6gXQAygTAZgxQAZgxgNgzQgQg8g2geQg3geg6ATIAAAAIANAnIh/gbIBXhhIALAkQBQgaBKApQBLApAUBTQARBFghBCQgiBBhDAbQglAPgjAAQgqAAgpgUg");
        this.shape.setTransform(39.2, 38.8, 0.738, 0.738);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.lf(["#00679A", "#046597", "#0F618E", "#175A80", "#1C506D", "#1B4256", "#15323C", "#0D2328"], [0, 0.165, 0.318, 0.467, 0.612, 0.761, 0.902, 1], -3.1, -18.9, 9.8, 21.3).s().p("AirDHQiDg5g9iAQA9hBBVg0QBXg2BnghQBjggBkgHQBhgIBXAQQAbCNhJB8QhKB9iKAsQg7ATg6AAQhNAAhLghg");
        this.shape_1.setTransform(39.8, 49.5, 0.738, 0.738);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.lf(["#004D7A", "#2A719B"], [0, 1], -11.2, -25.4, 2, 15.8).s().p("AlnDiQgwiWBIiMQBJiNCWgwQCWgxCNBJQCMBIAxCXQAGARAFAbQhXgQhhAIQhkAHhjAgQhnAhhXA2QhVA0g9BCQgMgbgHgVg");
        this.shape_2.setTransform(38.1, 30.4, 0.738, 0.738);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_2, p: { scaleX: 0.738, scaleY: 0.738, y: 30.4, x: 38.1 } }, { t: this.shape_1, p: { scaleX: 0.738, scaleY: 0.738, x: 39.8, y: 49.5 } }, { t: this.shape, p: { scaleX: 0.738, scaleY: 0.738, x: 39.2, y: 38.8 } }] }).to({ state: [{ t: this.shape_2, p: { scaleX: 1.091, scaleY: 1.091, y: 26.5, x: 38.1 } }, { t: this.shape_1, p: { scaleX: 1.091, scaleY: 1.091, x: 40.6, y: 54.9 } }, { t: this.shape, p: { scaleX: 1.091, scaleY: 1.091, x: 39.6, y: 39 } }] }, 1).to({ state: [{ t: this.shape_2, p: { scaleX: 0.817, scaleY: 0.817, y: 29.4, x: 38.1 } }, { t: this.shape_1, p: { scaleX: 0.817, scaleY: 0.817, x: 39.9, y: 50.7 } }, { t: this.shape, p: { scaleX: 0.817, scaleY: 0.817, x: 39.3, y: 38.8 } }] }, 1).to({ state: [{ t: this.shape_2, p: { scaleX: 1, scaleY: 1, y: 27.4, x: 37.9 } }, { t: this.shape_1, p: { scaleX: 1, scaleY: 1, x: 40.2, y: 53.4 } }, { t: this.shape, p: { scaleX: 1, scaleY: 1, x: 39.4, y: 38.8 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(10.2, 10.2, 56.5, 56.5);


    (lib.btn_participacion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgNAVQgEgDgCgGQgDgFAAgGQAAgHADgFQACgGAFgDQAEgDAHAAQADAAAEACQAEACACAEIAAgHIALAAIAAAtIgLAAIAAgIQgCAFgEABQgEACgDAAQgGAAgGgCgAgHgKQgDAEAAAHQAAAGADAEQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape.setTransform(43.1, -33.1);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgMAfQgFgDgCgGQgDgFAAgHQAAgHADgFQACgFAFgDQAEgDAHAAQADAAAEACQAEACACAEIAAgbIALAAIAABBIgLAAIAAgIQgCAEgEACQgEACgDAAQgGAAgFgCgAgIAAQgCADAAAHQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgHgDgDQgDgEgGAAQgFAAgDAEg");
        this.shape_1.setTransform(37.5, -34.1);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AAKAXIAAgaQAAgFgCgDQgCgCgFAAQgDAAgEADQgDADAAAFIAAAZIgLAAIAAgfIAAgNIAKAAIAAAHQADgEAEgCQADgCAFAAQAQAAABASIAAAbg");
        this.shape_2.setTransform(32.1, -33.1);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgNAVQgEgDgCgGQgDgFAAgGQAAgHADgFQACgGAFgDQAEgDAHAAQADAAAEACQAEACACAEIAAgHIALAAIAAAtIgLAAIAAgIQgCAFgEABQgEACgDAAQgGAAgGgCgAgIgKQgCAEAAAHQAAAGACAEQAEAEAEAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgFAAgDAEg");
        this.shape_3.setTransform(26.5, -33.1);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AAYAXIAAgaQAAgGgCgCQgCgCgEAAQgFAAgCADQgDADAAAGIAAAYIgKAAIAAgaQAAgGgCgCQgCgCgEAAQgFAAgCADQgDADAAAGIAAAYIgLAAIAAgfIgBgNIALAAIAAAHQACgEAEgCQAEgCAFAAQAJAAADAIQADgEAEgCQAEgCAFAAQAHAAAEAEQAEAFAAAJIAAAbg");
        this.shape_4.setTransform(19.8, -33.1);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgOASQgGgHAAgLQAAgGADgFQACgGAFgDQAFgDAGAAQAJAAAGAHQAFAFAAALIAAABIgeAAQAAAHADADQAEAEAFAAQAIAAAGgFIADAIQgDACgFACQgEABgFAAQgKAAgHgFgAgFgMQgDADgBAFIAUAAQAAgGgDgCQgCgCgFgBQgDABgDACg");
        this.shape_5.setTransform(13.3, -33.1);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgbAgIAAg/IAXAAQAPAAAJAIQAIAJAAAOQAAAPgIAIQgJAJgPAAgAgQAWIAMAAQAVABAAgXQAAgVgVgBIgMAAg");
        this.shape_6.setTransform(7.5, -34);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgNAVQgEgDgDgFQgCgGAAgHQAAgFACgGQADgGAFgDQAEgCAGAAQAEAAAEACQAEABACAFIAAgHIALAAIAAAsIgLAAIAAgIQgCAFgEACQgEACgEAAQgFgBgGgCgAgHgKQgDAEgBAGQABAIADADQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_7.setTransform(66.8, -48.3);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgFAhIAAhBIALAAIAABBg");
        this.shape_8.setTransform(63, -49.3);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AAKAXIAAgaQAAgFgCgDQgCgCgEAAQgFAAgCADQgEADAAAFIAAAZIgLAAIAAgfIAAgNIAKAAIAAAHQADgEAEgCQADgCAFAAQARAAAAASIAAAbg");
        this.shape_9.setTransform(56.6, -48.4);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgOARQgGgGAAgLQAAgGADgFQACgGAFgDQAFgCAGAAQAJAAAGAFQAFAHAAAKIAAABIgeAAQAAAHADAEQAEADAFAAQAIAAAGgFIADAIQgDACgFACQgEABgFABQgKAAgHgHgAgFgMQgDADgBAFIAUAAQAAgFgDgDQgCgCgFAAQgDAAgDACg");
        this.shape_10.setTransform(51.4, -48.3);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgSASIADgIQAHAFAJAAQAEAAACgBQAAAAABgBQAAAAABAAQAAgBAAAAQAAgBAAgBQAAAAAAgBQAAAAAAgBQAAAAgBAAQAAgBAAAAIgGgCIgGgCQgNgCAAgKQAAgEADgDQACgDAEgCQAFgBAEAAQAFAAAEABQAFABADADIgDAIQgHgFgHAAIgFABQAAAAgBABQAAAAgBABQAAAAAAABQAAAAAAABQAAABAAAAQAAAAAAABQAAAAABABQAAAAAAABIAFACIAGABQAHACADABQADAEAAAFQAAAGgFAEQgFADgIABQgMgBgHgFg");
        this.shape_11.setTransform(44.2, -48.3);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgOARQgGgGAAgLQAAgGADgFQACgGAFgDQAFgCAGAAQAJAAAGAFQAFAHAAAKIAAABIgeAAQAAAHADAEQAEADAFAAQAIAAAGgFIADAIQgDACgFACQgEABgFABQgKAAgHgHgAgFgMQgDADgBAFIAUAAQAAgFgDgDQgCgCgFAAQgDAAgDACg");
        this.shape_12.setTransform(39.5, -48.3);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgNAXIAAgfIgBgNIALAAIABAIQABgFADgCQAEgCAEAAIAFAAIAAALIgGgBQgGAAgDADQgCADAAAGIAAAXg");
        this.shape_13.setTransform(35.5, -48.4);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AgFAfQgEgCgCgEIAAAIIgLAAIAAhBIALAAIAAAbQADgEADgCQAEgCAEAAQAGAAAEADQAFADACAFQADAFAAAHQAAAHgDAFQgCAGgFADQgEACgGAAQgEAAgEgCgAgHAAQgEADAAAHQAAAHAEAEQADAEAEAAQAFAAAEgEQADgEAAgHQAAgHgDgDQgEgEgFAAQgEAAgDAEg");
        this.shape_14.setTransform(30.8, -49.3);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgFAhIAAgsIALAAIAAAsgAgFgVIAAgLIALAAIAAALg");
        this.shape_15.setTransform(26.7, -49.3);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AgUAgIAAg/IAMAAIAAA2IAdAAIAAAJg");
        this.shape_16.setTransform(23.3, -49.2);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgSASIADgIQAHAFAJAAQAEAAACgBQAAAAABgBQAAAAABAAQAAgBAAAAQAAgBAAgBQAAAAAAgBQAAAAAAgBQAAAAgBAAQAAgBAAAAIgGgCIgGgCQgNgCAAgKQAAgEADgDQACgDAEgCQAFgBAEAAQAFAAAEABQAFABADADIgDAIQgHgFgHAAIgFABQAAAAgBABQAAAAgBABQAAAAAAABQAAAAAAABQAAABAAAAQAAAAAAABQAAAAABABQAAAAAAABIAFACIAGABQAHACADABQADAEAAAFQAAAGgFAEQgFADgIABQgMgBgHgFg");
        this.shape_17.setTransform(15.8, -48.3);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgLAVQgFgDgDgFQgDgGAAgHQAAgGADgFQADgGAFgDQAGgCAFAAQAHAAAFACQAGADACAGQADAFAAAGQAAAHgDAGQgCAFgGADQgFACgHABQgFgBgGgCgAgIgKQgCAEAAAGQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgFAAgDAEg");
        this.shape_18.setTransform(10.9, -48.3);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgFAhIAAgsIALAAIAAAsgAgFgVIAAgLIALAAIAAALg");
        this.shape_19.setTransform(7, -49.3);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgNAXIAAgfIgBgNIALAAIABAIQABgFADgCQAEgCAEAAIAFAAIAAALIgGgBQgGAAgDADQgCADAAAGIAAAXg");
        this.shape_20.setTransform(4.3, -48.4);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#005495").s().p("AgNAVQgEgDgCgFQgDgGAAgHQAAgFADgGQACgGAFgDQAEgCAHAAQADAAAEACQAEABACAFIAAgHIALAAIAAAsIgLAAIAAgIQgCAFgEACQgEACgDAAQgGgBgGgCgAgHgKQgDAEAAAGQAAAIADADQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_21.setTransform(-0.6, -48.3);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#005495").s().p("AgUAEIAAgaIALAAIAAAaQAAAFACADQACACAFAAQADAAADgDQADgEAAgFIAAgYIAMAAIAAAsIgLAAIAAgGQgCADgEACQgEACgDAAQgRAAAAgTg");
        this.shape_22.setTransform(-5.9, -48.3);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#005495").s().p("AgSASIADgIQAHAFAJAAQAEAAACgBQAAAAABgBQAAAAABAAQAAgBAAAAQAAgBAAgBQAAAAAAgBQAAAAAAgBQAAAAgBAAQAAgBAAAAIgGgCIgGgCQgNgCAAgKQAAgEADgDQACgDAEgCQAFgBAEAAQAFAAAEABQAFABADADIgDAIQgHgFgHAAIgFABQAAAAgBABQAAAAgBABQAAAAAAABQAAAAAAABQAAABAAAAQAAAAAAABQAAAAABABQAAAAAAABIAFACIAGABQAHACADABQADAEAAAFQAAAGgFAEQgFADgIABQgMgBgHgFg");
        this.shape_23.setTransform(-10.9, -48.3);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#005495").s().p("AgTAaQgHgIAAgNIAAglIAMAAIAAAmQAAAJAEAEQAEAEAGAAQAIAAAEgEQADgEAAgJIAAgmIAMAAIAAAlQAAANgHAIQgHAGgNAAQgMAAgHgGg");
        this.shape_24.setTransform(-16.4, -49.2);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#005495").s().p("AgOASQgGgHAAgLQAAgGADgFQACgFAFgDQAFgEAGAAQAJAAAGAHQAFAFAAALIAAABIgeAAQAAAHADADQAEAEAFAAQAIAAAGgFIADAIQgDADgFABQgEABgFAAQgKAAgHgFgAgFgMQgDADgBAFIAUAAQAAgGgDgCQgCgCgFgBQgDABgDACg");
        this.shape_25.setTransform(57.3, -62.7);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#005495").s().p("AgMAfQgFgDgCgGQgDgFAAgHQAAgHADgFQACgFAFgDQAEgDAHAAQADAAAEACQAEACACAEIAAgbIALAAIAABBIgLAAIAAgIQgCAEgEACQgEACgDAAQgGAAgFgCgAgIAAQgCADAAAHQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgHgDgDQgDgEgGAAQgFAAgDAEg");
        this.shape_26.setTransform(51.9, -63.7);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#005495").s().p("AAKAXIAAgaQAAgFgCgDQgCgCgFAAQgEAAgDADQgCADAAAFIAAAZIgMAAIAAgfIAAgNIAKAAIABAHQACgEAEgCQADgCAFAAQAQAAAAASIAAAbg");
        this.shape_27.setTransform(44.1, -62.7);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#005495").s().p("AgLAhQgFgDgDgGQgDgFAAgHQAAgHADgFQADgFAFgDQAGgDAFAAQAHAAAFADQAGADACAFQADAFAAAHQAAAHgDAFQgCAGgGADQgFACgHAAQgFAAgGgCgAgIABQgCAEAAAHQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgHgDgEQgDgDgGAAQgFAAgDADgAgCgQIAIgSIALAAIgNASg");
        this.shape_28.setTransform(38.7, -63.9);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#005495").s().p("AgFAhIAAgtIALAAIAAAtgAgFgVIAAgLIALAAIAAALg");
        this.shape_29.setTransform(34.8, -63.7);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#005495").s().p("AgIAVQgGgDgCgGQgDgFAAgGQAAgHADgFQADgFAFgDQAFgEAGAAQAFABAFABQAEABADADIgDAIQgDgDgDgBIgHgBQgFABgDADQgEAEAAAGQAAAHAEAEQADAEAFAAQAEAAADgCQADAAADgDIADAIQgDADgEABQgFABgFAAQgGAAgFgCg");
        this.shape_30.setTransform(31.5, -62.7);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#005495").s().p("AgNAVQgEgDgDgGQgCgFAAgGQAAgHACgFQADgFAFgDQAFgEAFAAQAEAAAEACQAEADACADIAAgHIALAAIAAAtIgLAAIAAgHQgCADgEACQgEACgEAAQgGAAgFgCgAgHgKQgEAEAAAHQAAAGAEAEQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_31.setTransform(26.2, -62.7);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.f("#005495").s().p("AgWAhIAAgzIAAgNIAKAAIABAIQACgEAEgDQAEgCAEAAQAGAAAFADQAEADADAFQACAGAAAHQAAAHgCAEQgDAFgEAEQgFACgGAAQgEAAgEgCQgDgCgCgEIAAAbgAgHgUQgEAEAAAHQAAAIAEADQADADAEAAQAGAAADgDQADgDAAgHQAAgIgDgEQgDgDgGAAQgEAAgDADg");
        this.shape_32.setTransform(20.8, -61.7);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f("#005495").s().p("AgFAhIAAgtIALAAIAAAtgAgFgVIAAgLIALAAIAAALg");
        this.shape_33.setTransform(16.8, -63.7);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f("#005495").s().p("AgIAVQgGgDgCgGQgDgFAAgGQAAgHADgFQADgFAFgDQAFgEAGAAQAFABAFABQAEABADADIgDAIQgDgDgDgBIgHgBQgFABgDADQgEAEAAAGQAAAHAEAEQADAEAFAAQAEAAADgCQADAAADgDIADAIQgDADgEABQgFABgFAAQgGAAgFgCg");
        this.shape_34.setTransform(13.4, -62.7);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f("#005495").s().p("AgFAhIAAgtIALAAIAAAtgAgFgVIAAgLIALAAIAAALg");
        this.shape_35.setTransform(9.9, -63.7);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f("#005495").s().p("AgCAaQgEgEAAgIIAAgUIgJAAIAAgJIAJAAIAAgLIAKgDIAAAOIAMAAIAAAJIgMAAIAAAUQAAAHAHAAIAFgBIAAAJIgHABQgIAAgDgEg");
        this.shape_36.setTransform(7, -63.4);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#005495").s().p("AgNAXIAAgfIgBgNIALAAIABAIQABgFADgCQAEgCAEAAIAFAAIAAALIgGgBQgGAAgDADQgCADAAAGIAAAXg");
        this.shape_37.setTransform(3.8, -62.7);

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f("#005495").s().p("AgNAVQgEgDgDgGQgCgFAAgGQAAgHACgFQADgFAFgDQAEgEAGAAQAEAAAEACQAEADACADIAAgHIALAAIAAAtIgLAAIAAgHQgCADgEACQgEACgEAAQgFAAgGgCgAgHgKQgDAEgBAHQABAGADAEQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_38.setTransform(-1.1, -62.7);

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#005495").s().p("AgYAgIAAg/IAbAAQAKAAAGAFQAGAFAAAIQAAAJgGAEQgGAGgKgBIgPAAIAAAbgAgMgDIAOAAQAGAAADgCQADgDAAgFQAAgEgDgCQgDgDgGAAIgOAAg");
        this.shape_39.setTransform(-6.6, -63.6);

        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_40.setTransform(24.5, -25.3);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_41.setTransform(24.3, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_41 }, { t: this.shape_40 }, { t: this.shape_39 }, { t: this.shape_38 }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_42 = new cjs.Shape();
        this.shape_42.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_42.setTransform(19, 19.4);

        this.shape_43 = new cjs.Shape();
        this.shape_43.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_43.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_43, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_42, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_43, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_42, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_43, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_42, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_43, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_42, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_hidro = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Layer 1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgRAbQgGgDgDgHQgEgIAAgJQAAgIAEgIQADgGAHgEQAFgEAIgBQAGAAAFADQAFADADAFIAAgJIAOAAIAAA7IgOAAIAAgJQgDAFgFACQgFADgGAAQgHAAgHgEgAgLgOQgEAGABAIQgBAJAEAGQAFAEAGAAQAHAAAFgEQAEgGAAgJQAAgIgEgGQgFgFgHAAQgGAAgFAFg");
        this.shape.setTransform(58.3, -38.7);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgMAbQgHgDgDgHQgEgHAAgKQAAgIAEgIQAEgGAHgEQAHgEAJgBQAGABAGACQAGACAEADIgEAKQgEgCgFgCQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAJAEAGQAFAEAHAAQAEABAEgCQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgEg");
        this.shape_1.setTransform(51.8, -38.7);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgHAsIAAg8IAPAAIAAA8gAgHgcIAAgPIAQAAIAAAPg");
        this.shape_2.setTransform(47.1, -40.1);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgSAfIAAgrIgBgRIAPAAIABALQABgGAFgDQAFgDAGAAIAGABIAAAOQgEgCgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_3.setTransform(43.6, -38.8);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AgDAjQgGgGAAgLIAAgbIgLAAIAAgLIALAAIAAgPIAOgFIAAAUIARAAIAAALIgRAAIAAAbQAAAKAKAAIAGgBIAAAMQgDACgGAAQgKAAgFgGg");
        this.shape_4.setTransform(38.5, -39.7);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgMAbQgHgDgDgHQgEgHAAgKQAAgIAEgIQAEgGAHgEQAHgEAJgBQAGABAGACQAGACAEADIgEAKQgEgCgFgCQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAJAEAGQAFAEAHAAQAEABAEgCQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgEg");
        this.shape_5.setTransform(33.4, -38.7);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgSAmQgJgIAAgPQAAgIADgHQAEgHAHgEQAGgEAIAAQAMAAAIAIQAHAHAAAOIAAADIgpAAQABAJAFAFQAEAFAHAAQAKAAAJgHIAEAKQgEAEgGACQgHACgHAAQgNAAgIgJgAgHgBQgEACgBAIIAbAAQgBgIgDgCQgEgDgGgBQgEABgEADgAgCgWIALgYIAPAAIgSAYg");
        this.shape_6.setTransform(27, -40.3);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgHAsIAAhXIAPAAIAABXg");
        this.shape_7.setTransform(22.1, -40.1);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgSAXQgJgIAAgPQAAgIADgIQAEgGAHgEQAHgEAHgBQAMABAIAHQAHAJAAAOIAAABIgoAAQAAAJAEAGQAFAEAIAAQAJAAAJgHIAEALQgEADgHACQgGACgGAAQgOABgIgJgAgHgRQgEAEgBAHIAbAAQgBgHgDgEQgDgDgHAAQgEAAgEADg");
        this.shape_8.setTransform(17.3, -38.7);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AgPAbQgHgDgEgHQgEgIAAgJQAAgIAEgIQAEgHAHgEQAHgEAIAAQAJAAAHAEQAHAEAEAHQAEAIAAAIQAAAJgEAIQgEAHgHADQgHAEgJAAQgIAAgHgEgAgLgOQgEAFAAAJQAAAKAEAFQAEAEAHAAQAHAAAEgEQAFgFAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_9.setTransform(10.4, -38.7);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgSAfIAAgrIgBgRIAPAAIABALQABgGAFgDQAFgDAGAAIAGABIAAAOQgEgCgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_10.setTransform(4.9, -38.8);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgRApQgGgEgEgHQgCgIAAgJQAAgJACgHQAEgHAGgDQAHgEAHAAQAGAAAFACQAFADADAFIAAglIAOAAIAABYIgOAAIAAgKQgDAFgFADQgFADgGAAQgHAAgHgEgAgKgBQgFAFAAAJQAAAJAFAGQADAFAHAAQAHAAAEgFQAFgFAAgKQAAgJgEgEQgEgGgIAAQgHAAgDAFg");
        this.shape_11.setTransform(-1.7, -40.1);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgGAsIAAg8IANAAIAAA8gAgIgcIAAgPIAQAAIAAAPg");
        this.shape_12.setTransform(-6.8, -40.1);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AAWArIAAglIgrAAIAAAlIgPAAIAAhVIAPAAIAAAkIArAAIAAgkIAPAAIAABVg");
        this.shape_13.setTransform(-13, -40);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AAOAfIAAgjQAAgIgDgDQgDgDgFgBQgGABgFAEQgEAFAAAGIAAAiIgPAAIAAgrIgBgRIAPAAIABAKQADgFAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_14.setTransform(50.5, -57.2);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgPArQgHgEgEgHQgEgHAAgKQAAgJAEgGQAEgHAHgFQAHgDAIAAQAJAAAHADQAHAFAEAHQAEAGAAAJQAAAKgEAHQgEAHgHAEQgHAEgJAAQgIAAgHgEgAgLABQgEAFAAAJQAAAKAEAFQAEAFAHAAQAHAAAEgFQAFgFAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFgAgEgWIALgYIAPAAIgRAYg");
        this.shape_15.setTransform(43.4, -58.7);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AgHAsIAAg8IAPAAIAAA8gAgIgcIAAgPIAQAAIAAAPg");
        this.shape_16.setTransform(38.3, -58.5);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgMAbQgHgDgDgHQgEgHAAgKQAAgIAEgIQAEgGAHgEQAHgEAJgBQAGABAGACQAGACAEADIgEAKQgEgCgFgCQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAJAEAGQAFAEAHAAQAEABAEgCQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgEg");
        this.shape_17.setTransform(33.9, -57.2);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgMAbQgHgDgDgHQgEgHAAgKQAAgIAEgIQAEgGAHgEQAHgEAJgBQAGABAGACQAGACAEADIgEAKQgEgCgFgCQgEgCgEAAQgHAAgFAFQgEAGAAAIQAAAJAEAGQAFAEAHAAQAEABAEgCQAFgCAEgDIAEALQgEADgGACQgGACgHAAQgJAAgHgEg");
        this.shape_18.setTransform(27.8, -57.2);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgbAGIAAgkIAPAAIAAAkQAAAGADAEQACADAGAAQAGAAAEgFQADgEAAgHIAAghIAQAAIAAA8IgPAAIAAgKQgDAFgFADQgEADgGAAQgWAAAAgZg");
        this.shape_19.setTransform(21.1, -57.1);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgQApQgHgEgDgHQgEgIAAgJQAAgJAEgHQADgHAGgDQAHgEAHAAQAFAAAFACQAGADACAFIAAglIAPAAIAABYIgOAAIAAgKQgDAFgFADQgFADgGAAQgIAAgFgEgAgLgBQgDAFAAAJQAAAJADAGQAFAFAGAAQAIAAAEgFQAEgFAAgKQAAgJgEgEQgEgGgIAAQgGAAgFAFg");
        this.shape_20.setTransform(13.6, -58.5);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#005495").s().p("AgPAbQgHgDgEgHQgEgIAAgJQAAgIAEgIQAEgHAHgEQAHgEAIAAQAJAAAHAEQAHAEAEAHQAEAIAAAIQAAAJgEAIQgEAHgHADQgHAEgJAAQgIAAgHgEgAgLgOQgEAFAAAJQAAAKAEAFQAEAEAHAAQAHAAAEgEQAFgFAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_21.setTransform(6.5, -57.2);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#005495").s().p("AgSAfIAAgrIgBgRIAOAAIACALQACgGADgDQAGgDAGAAIAGABIgBAOQgDgCgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_22.setTransform(1, -57.2);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#005495").s().p("AggArIAAhVIAkAAQAOAAAHAHQAIAHAAALQAAAMgIAFQgIAHgNAAIgVAAIAAAkgAgRgEIATAAQAIAAAEgDQAFgEgBgGQAAgGgEgDQgEgEgIAAIgTAAg");
        this.shape_23.setTransform(-5.5, -58.4);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Layer 1
        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_24.setTransform(23.3, -25.5);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_25.setTransform(23.1, -48.4);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_25 }, { t: this.shape_24 }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_26.setTransform(19, 19.4);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_27.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_27, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_26, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_27, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_26, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_27, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_26, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_27, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_26, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_demanda = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgNAVQgEgDgCgFQgDgGAAgHQAAgFADgGQACgGAFgDQAEgCAHAAQADAAAEACQAEABACAFIAAgHIALAAIAAAsIgLAAIAAgIQgCAFgEACQgEACgDAAQgGgBgGgCgAgIgKQgCAEAAAGQAAAIACADQAEAEAEAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgFAAgDAEg");
        this.shape.setTransform(31, -32.6);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgKAjIAAgsIAKAAIAAAsgAgIgQIAIgSIALAAIgMASg");
        this.shape_1.setTransform(27.7, -33.8);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgRAdIABgJQAJAFAHAAQAMAAAAgMIAAgJQgCADgEADQgEACgEAAQgGAAgEgDQgFgDgDgFQgCgEAAgHQAAgGACgGQADgFAFgDQAEgCAGAAQAEAAAEACQAEACACAEIAAgHIALAAIAAArQAAAKgGAGQgFAFgMAAQgJAAgIgEgAgIgUQgDAEAAAGQAAAHADADQAEADAEAAQAGAAADgDQADgDAAgHQAAgGgDgEQgDgEgGAAQgEAAgEAEg");
        this.shape_2.setTransform(23.1, -31.6);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgNAXIAAgfIgBgNIALAAIABAIQABgFADgCQAEgCAEAAIAFAAIAAALIgGgBQgGAAgDADQgCADAAAGIAAAXg");
        this.shape_3.setTransform(18.9, -32.6);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AgOARQgGgGAAgLQAAgGADgFQACgGAFgDQAFgCAGAAQAJAAAGAFQAFAHAAAKIAAABIgeAAQAAAHADAEQAEADAFAAQAIAAAGgFIADAIQgDACgFACQgEABgFABQgKAAgHgHgAgFgMQgDADgBAFIAUAAQAAgFgDgDQgCgCgFAAQgDAAgDACg");
        this.shape_4.setTransform(14.4, -32.6);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AAKAXIAAgaQAAgFgCgDQgCgCgFAAQgDAAgEADQgDADAAAFIAAAZIgLAAIAAgfIAAgNIAKAAIAAAHQADgEAEgCQADgCAFAAQAQAAABASIAAAbg");
        this.shape_5.setTransform(9.2, -32.6);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgVAgIAAg/IArAAIAAAJIgfAAIAAASIAdAAIAAAIIgdAAIAAATIAfAAIAAAJg");
        this.shape_6.setTransform(4, -33.5);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgOASQgGgHAAgLQAAgGADgFQACgFAFgDQAFgEAGAAQAJAAAGAHQAFAFAAALIAAABIgeAAQAAAHADADQAEAEAFAAQAIAAAGgFIADAIQgDADgFABQgEABgFAAQgKAAgHgFgAgFgMQgDADgBAFIAUAAQAAgGgDgCQgCgCgFgBQgDABgDACg");
        this.shape_7.setTransform(57.9, -47.8);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgMAfQgFgDgCgGQgDgFAAgHQAAgHADgFQACgFAEgDQAGgDAGAAQADAAAEACQAEACACAEIAAgbIALAAIAABBIgLAAIAAgIQgCAEgEACQgEACgDAAQgHAAgEgCgAgHAAQgDADAAAHQAAAHADAEQADAEAEAAQAGAAADgEQADgEAAgHQAAgHgDgDQgDgEgFAAQgGAAgCAEg");
        this.shape_8.setTransform(52.5, -48.8);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AgFAhIAAhBIALAAIAABBg");
        this.shape_9.setTransform(46.2, -48.8);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgNAVQgEgDgCgGQgDgFAAgGQAAgHADgFQACgFAFgDQAEgEAHAAQADAAAEACQAEADACADIAAgHIALAAIAAAtIgLAAIAAgHQgCADgEACQgEACgDAAQgGAAgGgCgAgHgKQgDAEAAAHQAAAGADAEQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_10.setTransform(42.2, -47.8);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgCAaQgEgEgBgIIAAgUIgIAAIAAgJIAIAAIAAgLIALgDIAAAOIAMAAIAAAJIgMAAIAAAUQAAAHAHAAIAFgBIAAAJIgHABQgIAAgDgEg");
        this.shape_11.setTransform(37.8, -48.5);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgLAVQgFgDgDgGQgDgEAAgIQAAgGADgGQADgFAFgCQAGgEAFAAQAHAAAFAEQAFACAEAFQACAGAAAGQAAAIgCAEQgEAGgFADQgFACgHAAQgFAAgGgCgAgHgKQgEAEAAAGQAAAHAEAEQADAEAEAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_12.setTransform(33.4, -47.8);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgFAgIAAg2IgVAAIAAgJIA1AAIAAAJIgVAAIAAA2g");
        this.shape_13.setTransform(28, -48.7);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AAKAXIAAgaQAAgFgCgDQgCgCgEAAQgEAAgDADQgDADAAAFIAAAZIgMAAIAAgfIgBgNIALAAIABAHQACgEAEgCQADgCAFAAQAQAAAAASIAAAbg");
        this.shape_14.setTransform(20, -47.9);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgLAhQgFgDgDgGQgDgFAAgHQAAgHADgFQADgFAFgDQAFgDAGAAQAHAAAFADQAFADADAFQADAFAAAHQAAAHgDAFQgDAGgFADQgFACgHAAQgGAAgFgCgAgHABQgDAEgBAHQABAHADAEQADAEAEAAQAGAAADgEQADgEAAgHQAAgHgDgEQgDgDgGAAQgEAAgDADgAgDgQIAIgSIAMAAIgNASg");
        this.shape_15.setTransform(14.6, -49);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#005495").s().p("AgFAhIAAgtIALAAIAAAtgAgFgVIAAgLIALAAIAAALg");
        this.shape_16.setTransform(10.8, -48.8);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#005495").s().p("AgIAVQgGgDgCgGQgDgFAAgGQAAgHADgFQADgFAFgDQAFgEAGAAQAFABAFABQAEABADADIgDAIQgDgDgDgBIgHgBQgFABgDADQgEAEAAAGQAAAHAEAEQADAEAFAAQAEAAADgCQADAAADgDIADAIQgDADgEABQgFABgFAAQgGAAgFgCg");
        this.shape_17.setTransform(7.4, -47.8);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#005495").s().p("AgIAVQgGgDgCgGQgDgFAAgGQAAgHADgFQADgFAFgDQAFgEAGAAQAFABAFABQAEABADADIgDAIQgDgDgDgBIgHgBQgFABgDADQgEAEAAAGQAAAHAEAEQADAEAFAAQAEAAADgCQADAAADgDIADAIQgDADgEABQgFABgFAAQgGAAgFgCg");
        this.shape_18.setTransform(2.9, -47.8);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#005495").s().p("AgUAFIAAgbIALAAIAAAbQAAAEACADQACACAFAAQADAAADgDQADgDAAgFIAAgZIAMAAIAAAtIgLAAIAAgIQgCAEgEACQgEACgDAAQgRAAAAgSg");
        this.shape_19.setTransform(-2.2, -47.8);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#005495").s().p("AgMAfQgFgDgDgGQgCgFAAgHQAAgHACgFQADgFAFgDQAFgDAFAAQAEAAAEACQAEACACAEIAAgbIALAAIAABBIgLAAIAAgIQgCAEgEACQgEACgEAAQgFAAgFgCgAgIAAQgDADAAAHQAAAHADAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgHgDgDQgDgEgGAAQgEAAgEAEg");
        this.shape_20.setTransform(-7.7, -48.8);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#005495").s().p("AgLAVQgFgDgDgGQgDgEAAgIQAAgGADgGQADgFAFgCQAGgEAFAAQAHAAAFAEQAGACACAFQADAGAAAGQAAAIgDAEQgCAGgGADQgFACgHAAQgFAAgGgCgAgIgKQgCAEAAAGQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgFAAgDAEg");
        this.shape_21.setTransform(-13.1, -47.8);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#005495").s().p("AgNAXIAAgfIgBgNIALAAIABAIQABgFADgCQAEgCAEAAIAFAAIAAALIgGgBQgGAAgDADQgCADAAAGIAAAXg");
        this.shape_22.setTransform(-17.3, -47.9);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#005495").s().p("AgYAgIAAg/IAbAAQAKAAAGAFQAGAFAAAIQAAAJgGAEQgGAGgKgBIgPAAIAAAbgAgMgDIAOAAQAGAAADgCQADgDAAgFQAAgEgDgCQgDgDgGAAIgOAAg");
        this.shape_23.setTransform(-22.1, -48.7);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#005495").s().p("AgVAZQAGgCADgDQAEgCACgEIABgDIgSgrIALAAIAMAgIANggIALAAIgUAwQgEAIgEAEQgHADgIACg");
        this.shape_24.setTransform(58.1, -61.1);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#005495").s().p("AgNAVQgEgDgCgFQgDgGAAgHQAAgFADgGQACgGAFgDQAEgCAHAAQADAAAEABQAEACACAFIAAgHIALAAIAAAsIgLAAIAAgIQgCAEgEADQgEABgDAAQgGAAgGgCgAgHgKQgDAEAAAGQAAAIADADQACAEAGAAQAFAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgEAAgDAEg");
        this.shape_25.setTransform(50.3, -62.2);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#005495").s().p("AgMAfQgFgDgCgGQgDgFAAgHQAAgHADgFQACgFAFgDQAEgDAHAAQADAAAEACQAEACACAEIAAgbIALAAIAABBIgLAAIAAgIQgCAEgEACQgEACgDAAQgGAAgFgCgAgIAAQgCADAAAHQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgHgDgDQgDgEgGAAQgFAAgDAEg");
        this.shape_26.setTransform(44.7, -63.2);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#005495").s().p("AAKAXIAAgaQAAgFgCgDQgCgCgFAAQgDAAgEADQgDADAAAFIAAAZIgLAAIAAgfIAAgNIAKAAIABAHQACgEAEgCQADgCAFAAQAQAAABASIAAAbg");
        this.shape_27.setTransform(39.3, -62.2);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#005495").s().p("AgNAVQgEgDgCgFQgDgGAAgHQAAgFADgGQACgGAFgDQAEgCAHAAQADAAAEABQAEACACAFIAAgHIALAAIAAAsIgLAAIAAgIQgCAEgEADQgEABgDAAQgHAAgFgCgAgIgKQgCAEAAAGQAAAIACADQAEAEAEAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgFAAgDAEg");
        this.shape_28.setTransform(33.7, -62.2);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#005495").s().p("AAYAXIAAgaQAAgGgCgCQgCgCgEAAQgFAAgCADQgDADAAAGIAAAYIgKAAIAAgaQAAgGgCgCQgCgCgEAAQgFAAgCADQgDADAAAGIAAAYIgLAAIAAgfIgBgNIALAAIAAAHQACgEAEgCQAEgCAFAAQAJAAADAIQADgEAEgCQAEgCAFAAQAHAAAEAEQAEAFAAAJIAAAbg");
        this.shape_29.setTransform(27, -62.2);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#005495").s().p("AgOARQgGgGAAgLQAAgGADgFQACgGAFgDQAFgCAGAAQAJAAAGAFQAFAHAAAKIAAABIgeAAQAAAHADAEQAEADAFAAQAIAAAGgFIADAIQgDACgFACQgEABgFAAQgKABgHgHgAgFgMQgDADgBAFIAUAAQAAgFgDgDQgCgCgFAAQgDAAgDACg");
        this.shape_30.setTransform(20.5, -62.2);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#005495").s().p("AgbAgIAAg/IAXAAQAPAAAJAJQAIAHAAAPQAAAPgIAIQgJAJgPAAgAgQAXIAMAAQAVAAAAgXQAAgWgVABIgMAAg");
        this.shape_31.setTransform(14.7, -63.1);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.f("#005495").s().p("AgNAVQgEgDgCgFQgDgGAAgHQAAgFADgGQACgGAFgDQAEgCAHAAQADAAAEABQAEACACAFIAAgHIALAAIAAAsIgLAAIAAgIQgCAEgEADQgEABgDAAQgHAAgFgCgAgIgKQgCAEAAAGQAAAIACADQAEAEAEAAQAGAAADgEQADgEAAgHQAAgGgDgEQgDgEgGAAQgFAAgDAEg");
        this.shape_32.setTransform(5.8, -62.2);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f("#005495").s().p("AAYAXIAAgaQAAgGgCgCQgCgCgEAAQgFAAgCADQgDADAAAGIAAAYIgKAAIAAgaQAAgGgCgCQgCgCgEAAQgFAAgCADQgDADAAAGIAAAYIgLAAIAAgfIgBgNIALAAIAAAHQACgEAEgCQAEgCAFAAQAJAAADAIQADgEAEgCQAEgCAFAAQAHAAAEAEQAEAFAAAJIAAAbg");
        this.shape_33.setTransform(-0.9, -62.2);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f("#005495").s().p("AgFAhIAAgsIALAAIAAAsgAgFgVIAAgLIALAAIAAALg");
        this.shape_34.setTransform(-6.1, -63.2);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f("#005495").s().p("AAMAXIgMgQIgLAQIgOAAIATgXIgRgWIANAAIAKAOIALgOIANAAIgSAWIAUAXg");
        this.shape_35.setTransform(-9.8, -62.2);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f("#005495").s().p("AgMAhQgFgDgCgGQgDgFAAgHQAAgHADgFQACgFAFgDQAFgDAGAAQADAAAEACQAEACACAEIAAgHIALAAIAAAtIgLAAIAAgIQgCAEgEACQgEACgDAAQgHAAgEgCgAgIABQgCAEAAAHQAAAHACAEQADAEAFAAQAGAAADgEQADgEAAgHQAAgHgDgEQgDgDgGAAQgFAAgDADgAgBgQIAIgSIALAAIgNASg");
        this.shape_36.setTransform(-15.1, -63.4);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#005495").s().p("AAVAgIAAgnIgRAnIgHAAIgRgmIAAAmIgKAAIAAg/IAJAAIAVAxIAWgxIAJAAIAAA/g");
        this.shape_37.setTransform(-21.6, -63.1);

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_38.setTransform(18.5, -25.3);

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_39.setTransform(18.3, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_39 }, { t: this.shape_38 }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_40.setTransform(19, 19.4);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_41.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_41, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_40, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_41, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_40, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_41, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_40, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_41, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_40, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.btn_costo = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_2
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#005495").s().p("AgZAYIAFgLQAJAHAMAAQAFAAADgBQADgCAAgEQAAAAAAgBQAAAAAAgBQAAAAgBgBQAAgBgBAAIgHgDIgJgCQgQgEAAgMQAAgFADgFQADgEAFgDQAGgCAGAAQAHAAAGACQAGADAFADIgFAKQgJgHgKAAQgEABgDACQgDACAAACQAAABAAABQAAABABAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAFAAAGQAAAIgHAGQgHAEgLAAQgQAAgKgHg");
        this.shape.setTransform(47.6, -38.5);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#005495").s().p("AgSAXQgJgJAAgOQAAgIADgHQAEgIAHgDQAGgFAIAAQAMABAIAHQAHAJAAAOIAAABIgpAAQABAKAFAEQAEAFAHAAQAKAAAJgHIAEALQgEADgGACQgHACgHAAQgNAAgIgIgAgHgQQgEADgBAHIAbAAQgBgHgDgDQgEgEgGAAQgEAAgEAEg");
        this.shape_1.setTransform(41.4, -38.5);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#005495").s().p("AgHAsIAAhXIAPAAIAABXg");
        this.shape_2.setTransform(36.5, -39.9);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#005495").s().p("AgRAcQgGgEgEgHQgDgIAAgIQAAgJADgHQAEgIAHgDQAFgFAIAAQAGABAFACQAFADADAFIAAgJIAPAAIAAA7IgPAAIAAgJQgDAFgFADQgFACgGAAQgIAAgGgDgAgLgOQgDAGAAAJQAAAIADAFQAFAGAGAAQAIAAAEgGQAEgEAAgKQAAgIgEgGQgEgFgIAAQgGAAgFAFg");
        this.shape_3.setTransform(31.1, -38.5);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#005495").s().p("AAOAfIAAgjQAAgIgDgDQgDgDgFgBQgGABgFAEQgEAEAAAIIAAAhIgPAAIAAgqIgBgRIAPAAIABAJQADgFAFgDQAEgDAHAAQAWAAAAAZIAAAkg");
        this.shape_4.setTransform(23.8, -38.5);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#005495").s().p("AgHAsIAAg7IAOAAIAAA7gAgIgdIAAgOIAQAAIAAAOg");
        this.shape_5.setTransform(18.7, -39.8);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#005495").s().p("AgXAmIACgLQALAGAKAAQAQAAAAgQIAAgMQgDAFgFADQgFADgGAAQgIAAgGgEQgGgEgEgHQgDgGAAgIQAAgJADgHQAEgHAGgEQAGgDAIgBQAGABAFACQAFADADAFIAAgJIAPAAIAAA6QAAANgIAIQgIAHgPAAQgNABgKgHgAgLgbQgEAFAAAJQAAAIAEAFQAFAEAGAAQAIAAAEgEQAEgFAAgIQAAgJgEgFQgEgFgIAAQgGAAgFAFg");
        this.shape_6.setTransform(13.2, -37.2);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#005495").s().p("AgSAfIAAgqIgBgRIAPAAIAAAKQACgGAFgDQAEgDAHAAIAGABIAAANQgEgBgEAAQgIAAgEAFQgDAEAAAHIAAAgg");
        this.shape_7.setTransform(7.7, -38.5);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#005495").s().p("AgRAcQgGgEgEgHQgCgIAAgIQAAgJACgHQAEgIAGgDQAHgFAHAAQAGABAFACQAFADADAFIAAgJIAPAAIAAA7IgPAAIAAgJQgDAFgFADQgFACgGAAQgHAAgHgDgAgKgOQgEAGgBAJQABAIAEAFQADAGAHAAQAHAAAFgGQAEgEAAgKQAAgIgEgGQgFgFgHAAQgHAAgDAFg");
        this.shape_8.setTransform(1.1, -38.5);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#005495").s().p("AAcArIAAg0IgXA0IgJAAIgXg0IAAA0IgOAAIAAhVIANAAIAcBCIAdhCIANAAIAABVg");
        this.shape_9.setTransform(-7.6, -39.7);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#005495").s().p("AgZAYIAFgLQAJAHAMAAQAFAAADgBQADgCAAgEQAAAAAAgBQAAAAAAgBQgBAAAAgBQAAgBgBAAIgHgDIgJgCQgQgEAAgMQAAgFADgFQADgEAFgDQAGgCAGAAQAHAAAGACQAGADAFADIgFAKQgJgHgKAAQgEABgDACQgDACAAACQAAABAAABQABABAAAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAFAAAGQAAAIgHAGQgHAEgLAAQgQAAgKgHg");
        this.shape_10.setTransform(35.4, -56.9);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#005495").s().p("AgPAcQgHgEgEgHQgEgIAAgJQAAgIAEgIQAEgHAHgEQAHgDAIgBQAJABAHADQAHAEAEAHQAEAIAAAIQAAAJgEAIQgEAHgHAEQgHADgJAAQgIAAgHgDgAgLgOQgEAFAAAJQAAAKAEAEQAEAGAHAAQAHAAAEgGQAFgEAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_11.setTransform(28.9, -56.9);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#005495").s().p("AgDAjQgGgGAAgLIAAgbIgLAAIAAgLIALAAIAAgPIAOgFIAAAUIAQAAIAAALIgQAAIAAAbQAAAKAKAAIAGgBIAAAMQgDACgHAAQgJAAgFgGg");
        this.shape_12.setTransform(23, -57.9);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#005495").s().p("AgZAYIAFgLQAJAHAMAAQAFAAADgBQADgCAAgEQAAAAAAgBQAAAAAAgBQgBAAAAgBQAAgBgBAAIgHgDIgJgCQgQgEAAgMQAAgFADgFQADgEAFgDQAGgCAGAAQAHAAAGACQAGADAFADIgFAKQgJgHgKAAQgEABgDACQgDACAAACQAAABAAABQABABAAAAQAAABAAAAQABABAAAAQACABAEABIAJADQAJACAFADQAEAFAAAGQAAAIgHAGQgHAEgLAAQgQAAgKgHg");
        this.shape_13.setTransform(17.8, -56.9);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#005495").s().p("AgPAcQgHgEgEgHQgEgIAAgJQAAgIAEgIQAEgHAHgEQAHgDAIgBQAJABAHADQAHAEAEAHQAEAIAAAIQAAAJgEAIQgEAHgHAEQgHADgJAAQgIAAgHgDgAgLgOQgEAFAAAJQAAAKAEAEQAEAGAHAAQAHAAAEgGQAFgEAAgKQAAgJgFgFQgEgFgHAAQgHAAgEAFg");
        this.shape_14.setTransform(11.3, -56.9);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#005495").s().p("AgPAmQgJgFgFgKQgFgKAAgNQAAgMAFgKQAFgKAJgFQAKgGALAAQAJAAAIADQAHADAFAEIgFAMIgLgHQgGgCgHAAQgLAAgHAIQgGAIAAAOQAAAPAGAIQAHAIALAAQAHAAAGgCQAFgCAGgFIAFAMQgFAEgHADQgIADgJAAQgLAAgKgGg");
        this.shape_15.setTransform(3.7, -58.2);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#FFFFFF").s().p("AhJBIQAQg2gUg1QgLgdAdgZQAbgXApgGQAsgGAaASQAfAWgIAxQgPBWhiAsQgfANgkAIIgYAEQAQgGANgqg");
        this.shape_16.setTransform(18.5, -25.3);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#FFFFFF").s().p("AmSDcQgyAAgigjQgkgjAAgyIAAjHQAAgyAkgjQAigjAyAAIMlAAQAxAAAjAjQAkAjgBAyIAADHQABAygkAjQgjAjgxAAg");
        this.shape_17.setTransform(18.3, -48.2);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [] }).to({ state: [{ t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }, 1).to({ state: [] }, 1).wait(2));

        // Capa_1
        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.lf(["#020303", "#4E4E4E", "#231F20"], [0, 0.506, 1], -10.7, 10, 9.3, -9.9).s().p("AgHAYIhJAKIB7i2IgvCBIBVgOIiLC2g");
        this.shape_18.setTransform(19, 19.4);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#FFC815").s().p("AiECFQg3g3AAhOQAAhMA3g4QA3g3BNAAQBOAAA3A3QA3A4AABMQAABOg3A3Qg3A3hOAAQhNAAg3g3g");
        this.shape_19.setTransform(18.8, 18.8);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_19, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_18, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }).to({ state: [{ t: this.shape_19, p: { scaleX: 1.312, scaleY: 1.312, x: 18.9, y: 18.8 } }, { t: this.shape_18, p: { scaleX: 1.312, scaleY: 1.312, x: 19.1, y: 19.7 } }] }, 1).to({ state: [{ t: this.shape_19, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 18.9 } }, { t: this.shape_18, p: { scaleX: 0.445, scaleY: 0.445, x: 19, y: 19.2 } }] }, 1).to({ state: [{ t: this.shape_19, p: { scaleX: 1, scaleY: 1, x: 18.8, y: 18.8 } }, { t: this.shape_18, p: { scaleX: 1, scaleY: 1, x: 19, y: 19.4 } }] }, 1).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(0, 0, 37.6, 37.5);


    (lib.NUBES = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.instance = new lib.NUBES2("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(16.5, -8.4);
        this.instance.alpha = 0;

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(1).to({ regX: -3.7, regY: 5, x: 13.6, y: -3.4, alpha: 0.1 }, 0).wait(1).to({ x: 14.5, alpha: 0.2 }, 0).wait(1).to({ x: 15.3, alpha: 0.3 }, 0).wait(1).to({ x: 16.2, alpha: 0.4 }, 0).wait(1).to({ x: 17, alpha: 0.5 }, 0).wait(1).to({ x: 17.8, alpha: 0.6 }, 0).wait(1).to({ x: 18.7, alpha: 0.7 }, 0).wait(1).to({ x: 19.5, alpha: 0.8 }, 0).wait(1).to({ x: 20.4, alpha: 0.9 }, 0).wait(1).to({ regX: 0, regY: 0, x: 24.9, y: -8.4, alpha: 1 }, 0).wait(1).to({ regX: -3.7, regY: 5, x: 22, y: -3.4 }, 0).wait(1).to({ x: 22.9 }, 0).wait(1).to({ x: 23.7 }, 0).wait(1).to({ x: 24.6 }, 0).wait(1).to({ x: 25.4 }, 0).wait(1).to({ x: 26.2 }, 0).wait(1).to({ x: 27.1 }, 0).wait(1).to({ x: 27.9 }, 0).wait(1).to({ x: 28.8 }, 0).wait(1).to({ x: 29.6 }, 0).wait(1).to({ x: 30.4 }, 0).wait(1).to({ x: 31.3 }, 0).wait(1).to({ x: 32.1 }, 0).wait(1).to({ x: 33 }, 0).wait(1).to({ x: 33.8 }, 0).wait(1).to({ x: 34.7 }, 0).wait(1).to({ x: 35.5 }, 0).wait(1).to({ x: 36.3 }, 0).wait(1).to({ x: 37.2 }, 0).wait(1).to({ x: 38 }, 0).wait(1).to({ x: 38.9 }, 0).wait(1).to({ x: 39.7 }, 0).wait(1).to({ x: 40.5 }, 0).wait(1).to({ x: 41.4 }, 0).wait(1).to({ x: 42.2 }, 0).wait(1).to({ x: 43.1 }, 0).wait(1).to({ x: 43.9 }, 0).wait(1).to({ x: 44.7 }, 0).wait(1).to({ x: 45.6 }, 0).wait(1).to({ x: 46.4 }, 0).wait(1).to({ x: 47.3 }, 0).wait(1).to({ x: 48.1 }, 0).wait(1).to({ x: 48.9 }, 0).wait(1).to({ x: 49.8 }, 0).wait(1).to({ x: 50.6 }, 0).wait(1).to({ x: 51.5 }, 0).wait(1).to({ x: 52.3 }, 0).wait(1).to({ x: 53.2 }, 0).wait(1).to({ x: 54 }, 0).wait(1).to({ x: 54.8 }, 0).wait(1).to({ x: 55.7 }, 0).wait(1).to({ x: 56.5 }, 0).wait(1).to({ x: 57.4 }, 0).wait(1).to({ x: 58.2 }, 0).wait(1).to({ x: 59 }, 0).wait(1).to({ x: 59.9 }, 0).wait(1).to({ x: 60.7 }, 0).wait(1).to({ x: 61.6 }, 0).wait(1).to({ regX: 0, regY: 0, x: 66.1, y: -8.4 }, 0).wait(1).to({ regX: -3.7, regY: 5, x: 63.2, y: -3.4, alpha: 0.9 }, 0).wait(1).to({ x: 64.1, alpha: 0.8 }, 0).wait(1).to({ x: 64.9, alpha: 0.7 }, 0).wait(1).to({ x: 65.8, alpha: 0.6 }, 0).wait(1).to({ x: 66.6, alpha: 0.5 }, 0).wait(1).to({ x: 67.4, alpha: 0.4 }, 0).wait(1).to({ x: 68.3, alpha: 0.3 }, 0).wait(1).to({ x: 69.1, alpha: 0.2 }, 0).wait(1).to({ x: 70, alpha: 0.1 }, 0).wait(1).to({ regX: 0, regY: 0, x: 74.5, y: -8.4, alpha: 0 }, 0).wait(11));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-553.5, -139, 1199.8, 729.1);


    (lib.mc_variacion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(373.7, 185.7);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.fallasportipodeequipo();
        this.instance.parent = this;
        this.instance.setTransform(-495, -288, 0.74, 0.74);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_variacion, new cjs.Rectangle(-495, -288, 990, 577), null);


    (lib.mc_valorizacion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(2, 90, 0.991, 0.991);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.valorizaciondetransferencias();
        this.instance.parent = this;
        this.instance.setTransform(-495, -288, 0.627, 0.628);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_valorizacion, new cjs.Rectangle(-495, -288, 631, 484), null);


    (lib.mc_transmicion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(2, 90, 0.991, 0.991);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.LineasdeTransmision();
        this.instance.parent = this;
        this.instance.setTransform(-495, -289, 0.627, 0.628);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_transmicion, new cjs.Rectangle(-495, -289, 631, 484), null);


    (lib.mc_termo = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(373, 185);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.termoelectrica();
        this.instance.parent = this;
        this.instance.setTransform(-495, -288, 0.74, 0.74);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_termo, new cjs.Rectangle(-495, -288, 990, 577), null);


    (lib.mc_renovables = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(373, 185);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.produccionconenergiasrenovables2();
        this.instance.parent = this;
        this.instance.setTransform(-495, -289, 0.74, 0.74);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_renovables, new cjs.Rectangle(-495, -289, 990, 577), null);


    (lib.mc_participacion = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(2, 90, 0.991, 0.991);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.participaciondeusuarioslibresdedemanda();
        this.instance.parent = this;
        this.instance.setTransform(-503, -289, 0.627, 0.628);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_participacion, new cjs.Rectangle(-503, -289, 631, 484), null);


    (lib.mc_hidro = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(373, 185);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.hidroelectrica();
        this.instance.parent = this;
        this.instance.setTransform(-495, -288, 0.74, 0.74);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_hidro, new cjs.Rectangle(-495, -288, 990, 577), null);


    (lib.mc_demanda = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(2, 90, 0.991, 0.991);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.MaximaDemanda();
        this.instance.parent = this;
        this.instance.setTransform(-495, -289, 0.627, 0.628);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_demanda, new cjs.Rectangle(-495, -289, 631, 484), null);


    (lib.mc_costo = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            this.btn_regresar.on("click", onClickRegresar.bind(this));

            function onClickRegresar(e) {
                //console.log("Click en Regresar");

                this.parent.clickAgain();
                this.parent.removeAllChildren();
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(1));

        // Capa_1
        this.btn_regresar = new lib.btn_regresar();
        this.btn_regresar.name = "btn_regresar";
        this.btn_regresar.parent = this;
        this.btn_regresar.setTransform(373, 185);
        new cjs.ButtonHelper(this.btn_regresar, 0, 1, 2, false, new lib.btn_regresar(), 3);

        this.instance = new lib.CostosMarginales();
        this.instance.parent = this;
        this.instance.setTransform(-488, -290, 0.74, 0.74);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance }, { t: this.btn_regresar }] }).wait(1));

    }).prototype = getMCSymbolPrototype(lib.mc_costo, new cjs.Rectangle(-488, -290, 990, 577), null);


    (lib.molino = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // ASTA
        this.instance = new lib.ASTA("synched", 0);
        this.instance.parent = this;
        this.instance.setTransform(0.3, -35.4, 1, 1, 0, 0, 0, -7.4, -1.8);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(1).to({ regX: 0, regY: 0, rotation: 18, x: 6.7, y: -31.5 }, 0).wait(1).to({ rotation: 36, x: 5.2, y: -29.6 }, 0).wait(1).to({ rotation: 54, x: 3.1, y: -28.4 }, 0).wait(1).to({ rotation: 72, x: 0.8, y: -27.9 }, 0).wait(1).to({ rotation: 90, x: -1.5, y: -28 }, 0).wait(1).to({ rotation: 108, x: -3.7, y: -29 }, 0).wait(1).to({ rotation: 126, x: -5.6, y: -30.5 }, 0).wait(1).to({ rotation: 144, x: -6.8, y: -32.6 }, 0).wait(1).to({ rotation: 162, x: -7.3, y: -34.9 }, 0).wait(1).to({ rotation: 180, x: -7.1, y: -37.2 }, 0).wait(1).to({ rotation: 198, x: -6.2, y: -39.4 }, 0).wait(1).to({ rotation: 216, x: -4.7, y: -41.3 }, 0).wait(1).to({ rotation: 234, x: -2.6, y: -42.5 }, 0).wait(1).to({ rotation: 252, x: -0.3, y: -43 }, 0).wait(1).to({ rotation: 270, x: 2.1, y: -42.8 }, 0).wait(1).to({ rotation: 288, x: 4.2, y: -41.9 }, 0).wait(1).to({ rotation: 306, x: 6.1, y: -40.4 }, 0).wait(1).to({ rotation: 324, x: 7.3, y: -38.3 }, 0).wait(1).to({ rotation: 342, x: 7.8, y: -36 }, 0).wait(1).to({ regX: -7.4, regY: -1.8, rotation: 360, x: 0.3, y: -35.4 }, 0).wait(1));

        // PALO
        this.instance_1 = new lib.PALO("single", 0);
        this.instance_1.parent = this;
        this.instance_1.setTransform(0.1, 16.2);

        this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(21));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-23.3, -67.4, 62, 128);


    (lib.LOGOCOES2 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.instance = new lib.COOES("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(-3.5, -1.1);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(1).to({ regX: 2.7, regY: -2.7, scaleX: 1, scaleY: 1, x: -0.8, y: -3.8 }, 0).wait(1).to({ scaleX: 1, scaleY: 1 }, 0).wait(1).to({ scaleX: 0.99, scaleY: 0.99 }, 0).wait(1).to({ scaleX: 0.98, scaleY: 0.98, x: -0.9 }, 0).wait(1).to({ scaleX: 0.97, scaleY: 0.97, y: -3.7 }, 0).wait(1).to({ scaleX: 0.93, scaleY: 0.93, x: -1, y: -3.6 }, 0).wait(1).to({ scaleX: 0.9, scaleY: 0.9, x: -1.1, y: -3.5 }, 0).wait(1).to({ scaleX: 0.88, scaleY: 0.88, x: -1.2 }, 0).wait(1).to({ scaleX: 0.87, scaleY: 0.87 }, 0).wait(1).to({ scaleX: 0.87, scaleY: 0.87 }, 0).wait(1).to({ scaleX: 0.86, scaleY: 0.86 }, 0).wait(1).to({ regX: 0, regY: 0, x: -3.5, y: -1.1 }, 0).wait(1).to({ regX: 2.7, regY: -2.7, x: -1.2, y: -3.5 }, 0).wait(1).to({ startPosition: 0 }, 0).wait(1).to({ regX: 0, regY: 0, x: -3.6, y: -1.1 }, 0).wait(1).to({ regX: 2.7, regY: -2.7, scaleX: 0.86, scaleY: 0.86, x: -1.2, y: -3.4 }, 0).wait(1).to({ scaleX: 0.87, scaleY: 0.87 }, 0).wait(1).to({ scaleX: 0.87, scaleY: 0.87 }, 0).wait(1).to({ scaleX: 0.89, scaleY: 0.89, x: -1.1, y: -3.5 }, 0).wait(1).to({ scaleX: 0.91, scaleY: 0.91 }, 0).wait(1).to({ scaleX: 0.94, scaleY: 0.94, x: -1, y: -3.6 }, 0).wait(1).to({ scaleX: 0.97, scaleY: 0.97, x: -0.9, y: -3.7 }, 0).wait(1).to({ scaleX: 0.99, scaleY: 0.99 }, 0).wait(1).to({ scaleX: 0.99, scaleY: 0.99, x: -0.8, y: -3.8 }, 0).wait(1).to({ scaleX: 1, scaleY: 1 }, 0).wait(1).to({ scaleX: 1, scaleY: 1 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 1, scaleY: 1, x: -3.5, y: -1.1 }, 0).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-34.8, -21.5, 67.9, 35.4);


    (lib.HUMO = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1 copia copia
        this.instance = new lib.HU1("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(1.9, -8.9);
        this.instance.alpha = 0;
        this.instance._off = true;

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(23).to({ _off: false }, 0).wait(1).to({ regX: 2.2, regY: 2.1, scaleX: 0.99, scaleY: 0.99, x: 5, y: -7.6, alpha: 0.125 }, 0).wait(1).to({ scaleX: 0.98, scaleY: 0.98, x: 5.9, y: -8.4, alpha: 0.25 }, 0).wait(1).to({ scaleX: 0.97, scaleY: 0.97, x: 6.8, y: -9.2, alpha: 0.375 }, 0).wait(1).to({ scaleX: 0.96, scaleY: 0.96, x: 7.7, y: -10, alpha: 0.5 }, 0).wait(1).to({ scaleX: 0.95, scaleY: 0.95, x: 8.6, y: -10.8, alpha: 0.625 }, 0).wait(1).to({ scaleX: 0.94, scaleY: 0.94, x: 9.4, y: -11.6, alpha: 0.75 }, 0).wait(1).to({ scaleX: 0.93, scaleY: 0.93, x: 10.3, y: -12.4, alpha: 0.875 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 0.92, scaleY: 0.92, x: 9.2, y: -15.2, alpha: 1 }, 0).wait(1).to({ regX: 2.2, regY: 2.1, scaleX: 0.91, scaleY: 0.91, x: 12.5, y: -14.4, alpha: 0.95 }, 0).wait(1).to({ scaleX: 0.9, scaleY: 0.9, x: 13.8, y: -15.5, alpha: 0.9 }, 0).wait(1).to({ scaleX: 0.88, scaleY: 0.88, x: 15.1, y: -16.7, alpha: 0.85 }, 0).wait(1).to({ scaleX: 0.87, scaleY: 0.87, x: 16.3, y: -17.9, alpha: 0.8 }, 0).wait(1).to({ scaleX: 0.85, scaleY: 0.85, x: 17.6, y: -19, alpha: 0.75 }, 0).wait(1).to({ scaleX: 0.84, scaleY: 0.84, x: 18.9, y: -20.2, alpha: 0.7 }, 0).wait(1).to({ scaleX: 0.83, scaleY: 0.83, x: 20.1, y: -21.3, alpha: 0.65 }, 0).wait(1).to({ scaleX: 0.81, scaleY: 0.81, x: 21.4, y: -22.5, alpha: 0.6 }, 0).wait(1).to({ scaleX: 0.8, scaleY: 0.8, x: 22.7, y: -23.6, alpha: 0.55 }, 0).wait(1).to({ scaleX: 0.79, scaleY: 0.79, x: 24, y: -24.8, alpha: 0.5 }, 0).wait(1).to({ scaleX: 0.77, scaleY: 0.77, x: 25.3, y: -26, alpha: 0.45 }, 0).wait(1).to({ scaleX: 0.76, scaleY: 0.76, x: 26.5, y: -27.1, alpha: 0.4 }, 0).wait(1).to({ scaleX: 0.74, scaleY: 0.74, x: 27.8, y: -28.3, alpha: 0.35 }, 0).wait(1).to({ scaleX: 0.73, scaleY: 0.73, x: 29.1, y: -29.4, alpha: 0.3 }, 0).wait(1).to({ scaleX: 0.72, scaleY: 0.72, x: 30.3, y: -30.6, alpha: 0.25 }, 0).wait(1).to({ scaleX: 0.7, scaleY: 0.7, x: 31.6, y: -31.7, alpha: 0.2 }, 0).wait(1).to({ scaleX: 0.69, scaleY: 0.69, x: 32.9, y: -32.9, alpha: 0.15 }, 0).wait(1).to({ scaleX: 0.67, scaleY: 0.67, x: 34.2, y: -34, alpha: 0.1 }, 0).wait(1).to({ scaleX: 0.66, scaleY: 0.66, x: 35.5, y: -35.2, alpha: 0.05 }, 0).wait(1).to({ regX: 0.1, regY: 0, scaleX: 0.65, scaleY: 0.65, x: 35.3, y: -37.7, alpha: 0 }, 0).wait(9));

        // Capa_1 copia
        this.instance_1 = new lib.HU1("single", 0);
        this.instance_1.parent = this;
        this.instance_1.setTransform(1.9, -8.9);
        this.instance_1.alpha = 0;
        this.instance_1._off = true;

        this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(11).to({ _off: false }, 0).wait(1).to({ regX: 2.2, regY: 2.1, scaleX: 0.99, scaleY: 0.99, x: 5.1, y: -7.6, alpha: 0.167 }, 0).wait(1).to({ scaleX: 0.98, scaleY: 0.98, x: 6, y: -8.5, alpha: 0.333 }, 0).wait(1).to({ scaleX: 0.97, scaleY: 0.97, x: 7, y: -9.4, alpha: 0.5 }, 0).wait(1).to({ scaleX: 0.96, scaleY: 0.96, x: 7.9, y: -10.3, alpha: 0.667 }, 0).wait(1).to({ scaleX: 0.95, scaleY: 0.95, x: 8.9, y: -11.1, alpha: 0.833 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 0.94, scaleY: 0.94, x: 7.8, y: -13.9, alpha: 1 }, 0).wait(1).to({ regX: 2.2, regY: 2.1, scaleX: 0.93, scaleY: 0.93, x: 11.1, y: -13.1, alpha: 0.952 }, 0).wait(1).to({ scaleX: 0.91, scaleY: 0.91, x: 12.4, y: -14.3, alpha: 0.905 }, 0).wait(1).to({ scaleX: 0.9, scaleY: 0.9, x: 13.6, y: -15.4, alpha: 0.857 }, 0).wait(1).to({ scaleX: 0.88, scaleY: 0.88, x: 14.9, y: -16.6, alpha: 0.81 }, 0).wait(1).to({ scaleX: 0.87, scaleY: 0.87, x: 16.2, y: -17.8, alpha: 0.762 }, 0).wait(1).to({ scaleX: 0.86, scaleY: 0.86, x: 17.5, y: -18.9, alpha: 0.714 }, 0).wait(1).to({ scaleX: 0.84, scaleY: 0.84, x: 18.8, y: -20.1, alpha: 0.667 }, 0).wait(1).to({ scaleX: 0.83, scaleY: 0.83, x: 20, y: -21.2, alpha: 0.619 }, 0).wait(1).to({ scaleX: 0.81, scaleY: 0.81, x: 21.3, y: -22.4, alpha: 0.571 }, 0).wait(1).to({ scaleX: 0.8, scaleY: 0.8, x: 22.6, y: -23.5, alpha: 0.524 }, 0).wait(1).to({ scaleX: 0.79, scaleY: 0.79, x: 23.9, y: -24.7, alpha: 0.476 }, 0).wait(1).to({ scaleX: 0.77, scaleY: 0.77, x: 25.2, y: -25.9, alpha: 0.429 }, 0).wait(1).to({ scaleX: 0.76, scaleY: 0.76, x: 26.4, y: -27, alpha: 0.381 }, 0).wait(1).to({ scaleX: 0.74, scaleY: 0.74, x: 27.8, y: -28.2, alpha: 0.333 }, 0).wait(1).to({ scaleX: 0.73, scaleY: 0.73, x: 29, y: -29.3, alpha: 0.286 }, 0).wait(1).to({ scaleX: 0.72, scaleY: 0.72, x: 30.3, y: -30.5, alpha: 0.238 }, 0).wait(1).to({ scaleX: 0.7, scaleY: 0.7, x: 31.6, y: -31.7, alpha: 0.19 }, 0).wait(1).to({ scaleX: 0.69, scaleY: 0.69, x: 32.8, y: -32.8, alpha: 0.143 }, 0).wait(1).to({ scaleX: 0.67, scaleY: 0.67, x: 34.2, y: -34, alpha: 0.095 }, 0).wait(1).to({ scaleX: 0.66, scaleY: 0.66, x: 35.4, y: -35.1, alpha: 0.048 }, 0).wait(1).to({ regX: 0.1, regY: 0, scaleX: 0.65, scaleY: 0.65, x: 35.3, y: -37.7, alpha: 0 }, 0).wait(22));

        // Capa_1
        this.instance_2 = new lib.HU1("single", 0);
        this.instance_2.parent = this;
        this.instance_2.setTransform(1.9, -8.9);
        this.instance_2.alpha = 0;

        this.timeline.addTween(cjs.Tween.get(this.instance_2).wait(1).to({ regX: 2.2, regY: 2.1, scaleX: 0.99, scaleY: 0.99, x: 5.2, y: -7.7, alpha: 0.22 }, 0).wait(1).to({ scaleX: 0.98, scaleY: 0.98, x: 6.2, y: -8.7, alpha: 0.439 }, 0).wait(1).to({ scaleX: 0.97, scaleY: 0.97, x: 7.2, y: -9.6, alpha: 0.659 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 0.96, scaleY: 0.96, x: 6.2, y: -12.5, alpha: 0.879 }, 0).wait(1).to({ regX: 2.2, regY: 2.1, scaleX: 0.94, scaleY: 0.94, x: 9.5, y: -11.6, alpha: 0.841 }, 0).wait(1).to({ scaleX: 0.93, scaleY: 0.93, x: 10.7, y: -12.7, alpha: 0.802 }, 0).wait(1).to({ scaleX: 0.92, scaleY: 0.92, x: 12, y: -13.9, alpha: 0.764 }, 0).wait(1).to({ scaleX: 0.9, scaleY: 0.9, x: 13.2, y: -15, alpha: 0.726 }, 0).wait(1).to({ scaleX: 0.89, scaleY: 0.89, x: 14.4, y: -16.1, alpha: 0.688 }, 0).wait(1).to({ scaleX: 0.88, scaleY: 0.88, x: 15.7, y: -17.2, alpha: 0.65 }, 0).wait(1).to({ scaleX: 0.86, scaleY: 0.86, x: 16.9, y: -18.4, alpha: 0.611 }, 0).wait(1).to({ scaleX: 0.85, scaleY: 0.85, x: 18.1, y: -19.5, alpha: 0.573 }, 0).wait(1).to({ scaleX: 0.84, scaleY: 0.84, x: 19.4, y: -20.6, alpha: 0.535 }, 0).wait(1).to({ scaleX: 0.82, scaleY: 0.82, x: 20.6, y: -21.8, alpha: 0.497 }, 0).wait(1).to({ scaleX: 0.81, scaleY: 0.81, x: 21.9, y: -22.9, alpha: 0.459 }, 0).wait(1).to({ scaleX: 0.8, scaleY: 0.8, x: 23.1, y: -24, alpha: 0.42 }, 0).wait(1).to({ scaleX: 0.78, scaleY: 0.78, x: 24.3, y: -25.1, alpha: 0.382 }, 0).wait(1).to({ scaleX: 0.77, scaleY: 0.77, x: 25.6, y: -26.2, alpha: 0.344 }, 0).wait(1).to({ scaleX: 0.75, scaleY: 0.75, x: 26.8, y: -27.3, alpha: 0.306 }, 0).wait(1).to({ scaleX: 0.74, scaleY: 0.74, x: 28.1, y: -28.5, alpha: 0.267 }, 0).wait(1).to({ scaleX: 0.73, scaleY: 0.73, x: 29.3, y: -29.6, alpha: 0.229 }, 0).wait(1).to({ scaleX: 0.71, scaleY: 0.71, x: 30.5, y: -30.7, alpha: 0.191 }, 0).wait(1).to({ scaleX: 0.7, scaleY: 0.7, x: 31.8, y: -31.9, alpha: 0.153 }, 0).wait(1).to({ scaleX: 0.69, scaleY: 0.69, x: 33, y: -33, alpha: 0.115 }, 0).wait(1).to({ scaleX: 0.67, scaleY: 0.67, x: 34.3, y: -34.1, alpha: 0.076 }, 0).wait(1).to({ scaleX: 0.66, scaleY: 0.66, x: 35.5, y: -35.2, alpha: 0.038 }, 0).wait(1).to({ regX: 0.1, regY: 0, scaleX: 0.65, scaleY: 0.65, x: 35.3, y: -37.7, alpha: 0 }, 0).wait(33));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-10.1, -16.5, 28.6, 19.3);


    (lib.FAB1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#265C6D").s().p("AgPAzQgHgHAAgJIAAhEQAAgKAHgGQAGgHAJAAQAJAAAHAHQAHAGAAAKIAABEQAAAJgHAHQgHAGgJAAQgJAAgGgGg");
        this.shape.setTransform(-30.3, -30.9);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#265C6D").s().p("AgPAzQgHgHAAgJIAAhEQAAgKAHgGQAGgHAJAAQAJAAAHAHQAHAGAAAKIAABEQAAAJgHAHQgHAGgJAAQgJAAgGgGg");
        this.shape_1.setTransform(-48.4, -31.4);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#265C6D").s().p("AgPAyQgHgGAAgKIAAhEQAAgJAHgHQAHgGAIAAQAKAAAGAGQAHAHAAAJIAABEQAAAKgHAGQgGAHgKAAQgIAAgHgHg");
        this.shape_2.setTransform(-67.5, -32.1);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#706B5E").s().p("AgoAfIAAg9IBRAAIAAA9g");
        this.shape_3.setTransform(-32.8, -13.5);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#706B5E").s().p("AgoAfIAAg9IBRAAIAAA9g");
        this.shape_4.setTransform(-48.4, -13.5);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#706B5E").s().p("AgoAfIAAg9IBRAAIAAA9g");
        this.shape_5.setTransform(-63.8, -13.5);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#265C6D").s().p("AmMAcIAAg4IMZAAIAAA4g");
        this.shape_6.setTransform(-51.3, 1.5);

        this.instance = new lib.Path_7_0();
        this.instance.parent = this;
        this.instance.setTransform(-18.4, 4.6, 1, 1, 0, 0, 0, 7, 29);
        this.instance.alpha = 0.422;

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#5395AA").s().p("AmDCRIAAkhIMHAAIAAEhg");
        this.shape_7.setTransform(-50.4, 19.1);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#265C6D").s().p("AlJAWIAAgrIKTAAIAAArg");
        this.shape_8.setTransform(-44.6, -26.7);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#444036").s().p("AhjB8IAAj3IDGAAIAAD3g");
        this.shape_9.setTransform(42.1, 21.1);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#706B5E").s().p("AhiAfIAAg8IDGAAIAAA8g");
        this.shape_10.setTransform(74, -11.9);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#706B5E").s().p("AhjAfIAAg8IDGAAIAAA8g");
        this.shape_11.setTransform(42.1, -11.9);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#706B5E").s().p("AhiAfIAAg8IDFAAIAAA8g");
        this.shape_12.setTransform(9.9, -11.9);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#706B5E").s().p("AmjAfIAAg9INHAAIAAA9g");
        this.shape_13.setTransform(42, -3);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#F4DA9A").s().p("AoYF5IAApRIEKigIAWCgID4igIANCgIE2igIALCgIDKAAIAAJRg");
        this.shape_14.setTransform(42.2, -4.1);

        this.instance_1 = new lib.Path_52();
        this.instance_1.parent = this;
        this.instance_1.setTransform(61, -46.9, 1, 1, 0, 0, 0, 2.4, 16.7);
        this.instance_1.alpha = 0.57;

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#444036").s().p("AguAxIAAhhIBdAAIAABhg");
        this.shape_15.setTransform(58.6, -68.3);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#706B5E").s().p("AguDXIAAmtIBdAAIAAGtg");
        this.shape_16.setTransform(58.6, -51.7);

        this.instance_2 = new lib.Path_51();
        this.instance_2.parent = this;
        this.instance_2.setTransform(32.7, -45.4, 1, 1, 0, 0, 0, 2.4, 14);
        this.instance_2.alpha = 0.57;

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#444036").s().p("AguApIAAhRIBdAAIAABRg");
        this.shape_17.setTransform(30.2, -63.3);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#706B5E").s().p("AguC0IAAlnIBdAAIAAFng");
        this.shape_18.setTransform(30.2, -49.3);

        this.instance_3 = new lib.Path_50();
        this.instance_3.parent = this;
        this.instance_3.setTransform(3.9, -37.8, 1, 1, 0, 0, 0, 2.4, 14);
        this.instance_3.alpha = 0.57;

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#444036").s().p("AguApIAAhRIBdAAIAABRg");
        this.shape_19.setTransform(1.5, -55.7);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#706B5E").s().p("AguC0IAAlnIBdAAIAAFng");
        this.shape_20.setTransform(1.5, -41.8);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#846E3E").s().p("AgiiRIAvAAIAWEXIhFAMg");
        this.shape_21.setTransform(77, -28);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#846E3E").s().p("AgiCSIAAkjIAwAAIAVEjg");
        this.shape_22.setTransform(45.1, -28);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#846E3E").s().p("AgviRIAwAAIAvEYIhfALg");
        this.shape_23.setTransform(19.2, -28);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#87CCE6").s().p("Ak2B2IAAjrIJtAAIAADrg");
        this.shape_24.setTransform(-43, -13.4);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.instance_3 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.instance_2 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.instance_1 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.instance }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-91, -73.1, 186.9, 106.7);


    (lib.ELEC2 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#F69FA1").s().p("AgDAAIADgBIAEABIgEACg");
        this.shape.setTransform(9, -94.8);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#8B231B").s().p("AgBAIIAAgRIADACIAAARg");
        this.shape_1.setTransform(8.8, -93.8);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#CD4935").s().p("AgBgHIADgCIAAAQIgDADg");
        this.shape_2.setTransform(9.2, -93.7);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#8E8B8E").s().p("AgCAAIACAAIADAAIgDABg");
        this.shape_3.setTransform(9, -93.1);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#444546").s().p("AAAAkIAAhIIABABIAABIg");
        this.shape_4.setTransform(8.9, -89.4);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#737374").s().p("AAAgjIACgBIAABIIgCABg");
        this.shape_5.setTransform(9.2, -89.4);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#F69FA1").s().p("AgDAAIADgBIAEABIgEACg");
        this.shape_6.setTransform(16.4, -90.7);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#8B231B").s().p("AgBAHIAAgQIADACIAAARg");
        this.shape_7.setTransform(16.2, -89.7);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#CD4935").s().p("AgBgHIADgCIAAARIgDACg");
        this.shape_8.setTransform(16.6, -89.7);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#8E8B8E").s().p("AgCAAIACAAIADAAIgDABg");
        this.shape_9.setTransform(16.3, -89);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#444546").s().p("AAAAkIAAhIIABABIAABIg");
        this.shape_10.setTransform(16.2, -85.3);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#737374").s().p("AAAgjIABgBIAABIIgBABg");
        this.shape_11.setTransform(16.5, -85.3);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#F69FA1").s().p("AgDAAIADgBIAEABIgEACg");
        this.shape_12.setTransform(42.6, -76.6);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#8B231B").s().p("AgBAIIAAgRIADACIAAARg");
        this.shape_13.setTransform(42.4, -75.5);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#CD4935").s().p("AgBgHIADgCIAAARIgDACg");
        this.shape_14.setTransform(42.8, -75.5);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#8E8B8E").s().p("AgCAAIACAAIADAAIgDABg");
        this.shape_15.setTransform(42.5, -74.9);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#444546").s().p("AgBAkIAAhIIACABIAABIg");
        this.shape_16.setTransform(42.4, -71.2);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#737374").s().p("AAAgjIABgBIAABIIgBABg");
        this.shape_17.setTransform(42.7, -71.2);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#F69FA1").s().p("AgDAAIADgCIAEACIgEADg");
        this.shape_18.setTransform(49.9, -72.5);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#8B231B").s().p("AgBAIIAAgRIADACIAAARg");
        this.shape_19.setTransform(49.7, -71.5);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#CD4935").s().p("AgBgHIADgCIAAARIgDACg");
        this.shape_20.setTransform(50.1, -71.5);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#8E8B8E").s().p("AgCAAIACgBIADABIgDACg");
        this.shape_21.setTransform(49.9, -70.8);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#444546").s().p("AAAAkIAAhIIABACIAABHg");
        this.shape_22.setTransform(49.8, -67.1);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#737374").s().p("AAAgiIABgCIAABIIgBABg");
        this.shape_23.setTransform(50, -67.1);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#E7E3E3").s().p("AgMADIAQgJIAJAEIgQAJg");
        this.shape_24.setTransform(25.6, 82.5);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#919191").s().p("AgDAEIAAgMIAIAFIAAAMg");
        this.shape_25.setTransform(24.8, 83.6);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#C5C6C8").s().p("AgHgBIAPgKIAAANIgPAKg");
        this.shape_26.setTransform(26.1, 83.4);

        this.instance = new lib.Path_49();
        this.instance.parent = this;
        this.instance.setTransform(24.7, 83.3, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#E7E3E3").s().p("AgMACIAQgIIAJAFIgQAJg");
        this.shape_27.setTransform(23.8, 81.4);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#919191").s().p("AgDAEIAAgMIAIAFIAAAMg");
        this.shape_28.setTransform(23, 82.6);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#C5C6C8").s().p("AgHgBIAPgJIAAAMIgPAJg");
        this.shape_29.setTransform(24.3, 82.3);

        this.instance_1 = new lib.Path_48();
        this.instance_1.parent = this;
        this.instance_1.setTransform(22.9, 82.3, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#E7E3E3").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape_30.setTransform(28, 81.1);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#919191").s().p("AgEAEIAAgMIAIAFIAAAMg");
        this.shape_31.setTransform(27.2, 82.2);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.f("#C5C6C8").s().p("AgIAAIARgKIAAALIgRAKg");
        this.shape_32.setTransform(28.5, 82);

        this.instance_2 = new lib.Path_47();
        this.instance_2.parent = this;
        this.instance_2.setTransform(27.1, 81.9, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f("#E7E3E3").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape_33.setTransform(26.2, 80);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f("#919191").s().p("AgEAEIAAgMIAIAFIAAAMg");
        this.shape_34.setTransform(25.4, 81.2);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f("#C5C6C8").s().p("AgIgBIARgJIAAALIgRAKg");
        this.shape_35.setTransform(26.7, 80.9);

        this.instance_3 = new lib.Path_46();
        this.instance_3.parent = this;
        this.instance_3.setTransform(25.3, 80.9, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f("#9FA7AB").s().p("AgegLIAAgHIA9AiIgHADg");
        this.shape_36.setTransform(28.8, 80.4);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#BDCACF").s().p("AAAgLIg2AeIgGgDIA8giIA9AiIgGADg");
        this.shape_37.setTransform(25.7, 80.4);

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f("#929296").s().p("Ag8AAIA8giIA9AiIg9Akg");
        this.shape_38.setTransform(25.7, 82);

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#DFE1EB").s().p("AhLAAIBLgrIBMArIhMAsg");
        this.shape_39.setTransform(25.7, 81.9);

        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.f("#5685AE").s().p("AglgQIAAgKIBLArIAAAKg");
        this.shape_40.setTransform(21.9, 84.7);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#8AD3F5").s().p("AglguIA8AwIAPgIIAAAJIhLAsg");
        this.shape_41.setTransform(29.5, 82.7);

        this.shape_42 = new cjs.Shape();
        this.shape_42.graphics.f("#A2ACB3").s().p("AgcgPIADgCIA2AfIAAAEg");
        this.shape_42.setTransform(-2.4, 70.5);

        this.shape_43 = new cjs.Shape();
        this.shape_43.graphics.f("#4E575C").s().p("AgcgJIAAgOIADABIAAALIA2AfIAAAEg");
        this.shape_43.setTransform(-2.4, 69.9);

        this.shape_44 = new cjs.Shape();
        this.shape_44.graphics.lf(["#223063", "#445E72"], [0.188, 0.941], 0.1, 16.4, -0.1, -21.7).s().p("AgcgJIAAgOIA5AhIAAAOg");
        this.shape_44.setTransform(-2.4, 69.9);

        this.shape_45 = new cjs.Shape();
        this.shape_45.graphics.f("#7B7B7B").s().p("AghgGIAAgaIBDAnIAAAag");
        this.shape_45.setTransform(-2.4, 69.9);

        this.shape_46 = new cjs.Shape();
        this.shape_46.graphics.f("#A2ACB3").s().p("AgcgPIADgCIA2AfIAAAEg");
        this.shape_46.setTransform(9.3, 77.3);

        this.shape_47 = new cjs.Shape();
        this.shape_47.graphics.f("#4E575C").s().p("AgcgJIAAgOIADABIAAALIA2AfIAAAEg");
        this.shape_47.setTransform(9.3, 76.6);

        this.shape_48 = new cjs.Shape();
        this.shape_48.graphics.lf(["#223063", "#445E72"], [0.188, 0.941], 0.1, 16.4, -0.1, -21.7).s().p("AgcgJIAAgOIA5AhIAAAOg");
        this.shape_48.setTransform(9.3, 76.6);

        this.shape_49 = new cjs.Shape();
        this.shape_49.graphics.f("#7B7B7B").s().p("AghgGIAAgaIBDAnIAAAag");
        this.shape_49.setTransform(9.3, 76.6);

        this.shape_50 = new cjs.Shape();
        this.shape_50.graphics.f("#E7F6FD").s().p("AgPAHIAdgQIACABIgfASg");
        this.shape_50.setTransform(29.3, 87.8);

        this.shape_51 = new cjs.Shape();
        this.shape_51.graphics.f("#8D9DA8").s().p("AgPALIAdgQIAAgHIACAAIAAAIIgfASg");
        this.shape_51.setTransform(29.3, 87.4);

        this.shape_52 = new cjs.Shape();
        this.shape_52.graphics.lf(["#2E54A5", "#709BBF"], [0.188, 0.941], 0, -12.1, 0.1, 9.1).s().p("AgPAGIAfgSIAAAIIgfASg");
        this.shape_52.setTransform(29.3, 87.4);

        this.shape_53 = new cjs.Shape();
        this.shape_53.graphics.f("#C8C7C6").s().p("AgSAEIAlgVIAAAOIglAVg");
        this.shape_53.setTransform(29.3, 87.4);

        this.shape_54 = new cjs.Shape();
        this.shape_54.graphics.f("#E7F6FD").s().p("AgcAOIA2gfIADACIg5Ahg");
        this.shape_54.setTransform(22.8, 77.2);

        this.shape_55 = new cjs.Shape();
        this.shape_55.graphics.f("#8D9DA8").s().p("AgcAUIA2gfIAAgLIADgBIAAAOIg5Ahg");
        this.shape_55.setTransform(22.8, 76.5);

        this.shape_56 = new cjs.Shape();
        this.shape_56.graphics.lf(["#2E54A5", "#709BBF"], [0.188, 0.941], -0.1, -21.7, 0.1, 16.4).s().p("AgcAKIA5ghIAAAOIg5Ahg");
        this.shape_56.setTransform(22.8, 76.5);

        this.shape_57 = new cjs.Shape();
        this.shape_57.graphics.f("#C8C7C6").s().p("AghAHIBDgnIAAAaIhDAng");
        this.shape_57.setTransform(22.8, 76.5);

        this.shape_58 = new cjs.Shape();
        this.shape_58.graphics.f("#343C59").s().p("Ah/ADIAAiZID/CUIAACZg");
        this.shape_58.setTransform(3.6, 77.3);

        this.shape_59 = new cjs.Shape();
        this.shape_59.graphics.f("#DFE1EB").s().p("AhEAAIBEgnIBFAnIhFAog");
        this.shape_59.setTransform(25.7, 82.4);

        this.shape_60 = new cjs.Shape();
        this.shape_60.graphics.f("#343C59").s().p("AgiAXIAAhVIBEAoIAABVg");
        this.shape_60.setTransform(22.3, 88.7);

        this.shape_61 = new cjs.Shape();
        this.shape_61.graphics.f("#7C8392").s().p("AgigWIBFgoIAABVIhFAog");
        this.shape_61.setTransform(29.2, 88.7);

        this.shape_62 = new cjs.Shape();
        this.shape_62.graphics.f("#7C8392").s().p("Ag9goIB7hHIAACYIh7BIg");
        this.shape_62.setTransform(22.5, 81.1);

        this.shape_63 = new cjs.Shape();
        this.shape_63.graphics.f("#E7F6FD").s().p("AhAAjIB8hHIAEACIh6BHg");
        this.shape_63.setTransform(22.3, 70.3);

        this.shape_64 = new cjs.Shape();
        this.shape_64.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_64.setTransform(22.5, 71.9);

        this.shape_65 = new cjs.Shape();
        this.shape_65.graphics.f("#46718B").s().p("AgIAGIAMgVIAGADIAAAcg");
        this.shape_65.setTransform(15.4, 75.4);

        this.shape_66 = new cjs.Shape();
        this.shape_66.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEACIh6BHg");
        this.shape_66.setTransform(20.3, 69.2);

        this.shape_67 = new cjs.Shape();
        this.shape_67.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_67.setTransform(20.5, 70.8);

        this.shape_68 = new cjs.Shape();
        this.shape_68.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_68.setTransform(13.4, 74.3);

        this.shape_69 = new cjs.Shape();
        this.shape_69.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh7BHg");
        this.shape_69.setTransform(18.3, 68);

        this.shape_70 = new cjs.Shape();
        this.shape_70.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_70.setTransform(18.6, 69.7);

        this.shape_71 = new cjs.Shape();
        this.shape_71.graphics.f("#46718B").s().p("AgIAFIANgUIAEACIAAAdg");
        this.shape_71.setTransform(11.4, 73.1);

        this.shape_72 = new cjs.Shape();
        this.shape_72.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAGACIh8BHg");
        this.shape_72.setTransform(16.4, 66.9);

        this.shape_73 = new cjs.Shape();
        this.shape_73.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_73.setTransform(16.6, 68.5);

        this.shape_74 = new cjs.Shape();
        this.shape_74.graphics.f("#46718B").s().p("AgJAFIAOgUIAEACIAAAdg");
        this.shape_74.setTransform(9.5, 72);

        this.shape_75 = new cjs.Shape();
        this.shape_75.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFADIh6BGg");
        this.shape_75.setTransform(14.4, 65.7);

        this.shape_76 = new cjs.Shape();
        this.shape_76.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_76.setTransform(14.6, 67.4);

        this.shape_77 = new cjs.Shape();
        this.shape_77.graphics.f("#46718B").s().p("AgJAFIAOgUIAEACIAAAdg");
        this.shape_77.setTransform(7.5, 70.8);

        this.shape_78 = new cjs.Shape();
        this.shape_78.graphics.f("#E7F6FD").s().p("Ag/AiIB6hGIAFACIh7BHg");
        this.shape_78.setTransform(12.4, 64.6);

        this.shape_79 = new cjs.Shape();
        this.shape_79.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_79.setTransform(12.7, 66.2);

        this.shape_80 = new cjs.Shape();
        this.shape_80.graphics.f("#46718B").s().p("AgIAFIANgUIAEADIAAAcg");
        this.shape_80.setTransform(5.5, 69.7);

        this.shape_81 = new cjs.Shape();
        this.shape_81.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEACIh6BHg");
        this.shape_81.setTransform(10.4, 63.5);

        this.shape_82 = new cjs.Shape();
        this.shape_82.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_82.setTransform(10.7, 65.1);

        this.shape_83 = new cjs.Shape();
        this.shape_83.graphics.f("#46718B").s().p("AgJAGIANgVIAGACIAAAdg");
        this.shape_83.setTransform(3.6, 68.6);

        this.shape_84 = new cjs.Shape();
        this.shape_84.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh6BHg");
        this.shape_84.setTransform(8.5, 62.3);

        this.shape_85 = new cjs.Shape();
        this.shape_85.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_85.setTransform(8.7, 64);

        this.shape_86 = new cjs.Shape();
        this.shape_86.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_86.setTransform(1.6, 67.4);

        this.shape_87 = new cjs.Shape();
        this.shape_87.graphics.f("#E7F6FD").s().p("AhAAjIB8hHIAEACIh6BHg");
        this.shape_87.setTransform(6.5, 61.2);

        this.shape_88 = new cjs.Shape();
        this.shape_88.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_88.setTransform(6.8, 62.8);

        this.shape_89 = new cjs.Shape();
        this.shape_89.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_89.setTransform(-0.4, 66.3);

        this.shape_90 = new cjs.Shape();
        this.shape_90.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEACIh6BHg");
        this.shape_90.setTransform(4.5, 60);

        this.shape_91 = new cjs.Shape();
        this.shape_91.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_91.setTransform(4.8, 61.7);

        this.shape_92 = new cjs.Shape();
        this.shape_92.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_92.setTransform(-2.3, 65.1);

        this.shape_93 = new cjs.Shape();
        this.shape_93.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh7BHg");
        this.shape_93.setTransform(2.6, 58.9);

        this.shape_94 = new cjs.Shape();
        this.shape_94.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_94.setTransform(2.8, 60.5);

        this.shape_95 = new cjs.Shape();
        this.shape_95.graphics.f("#46718B").s().p("AgIAFIANgUIAEACIAAAdg");
        this.shape_95.setTransform(-4.3, 64);

        this.shape_96 = new cjs.Shape();
        this.shape_96.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAGACIh8BHg");
        this.shape_96.setTransform(0.6, 57.8);

        this.shape_97 = new cjs.Shape();
        this.shape_97.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_97.setTransform(0.9, 59.4);

        this.shape_98 = new cjs.Shape();
        this.shape_98.graphics.f("#46718B").s().p("AgJAFIAOgUIAEADIAAAcg");
        this.shape_98.setTransform(-6.3, 62.8);

        this.shape_99 = new cjs.Shape();
        this.shape_99.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh6BHg");
        this.shape_99.setTransform(-1.4, 56.6);

        this.shape_100 = new cjs.Shape();
        this.shape_100.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_100.setTransform(-1.1, 58.2);

        this.shape_101 = new cjs.Shape();
        this.shape_101.graphics.f("#46718B").s().p("AgJAFIAOgUIAEACIAAAdg");
        this.shape_101.setTransform(-8.2, 61.7);

        this.shape_102 = new cjs.Shape();
        this.shape_102.graphics.f("#E7E3E3").s().p("AgMADIAQgJIAJAFIgQAIg");
        this.shape_102.setTransform(-16.3, 58.3);

        this.shape_103 = new cjs.Shape();
        this.shape_103.graphics.f("#919191").s().p("AgEAEIAAgMIAJAFIAAAMg");
        this.shape_103.setTransform(-17.1, 59.4);

        this.shape_104 = new cjs.Shape();
        this.shape_104.graphics.f("#C5C6C8").s().p("AgHgBIAQgJIAAALIgQAKg");
        this.shape_104.setTransform(-15.8, 59.2);

        this.instance_4 = new lib.Path_45();
        this.instance_4.parent = this;
        this.instance_4.setTransform(-17.2, 59.1, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_105 = new cjs.Shape();
        this.shape_105.graphics.f("#E7E3E3").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape_105.setTransform(-18.1, 57.2);

        this.shape_106 = new cjs.Shape();
        this.shape_106.graphics.f("#919191").s().p("AgEAEIAAgMIAIAFIAAAMg");
        this.shape_106.setTransform(-18.9, 58.4);

        this.shape_107 = new cjs.Shape();
        this.shape_107.graphics.f("#C5C6C8").s().p("AgIgBIARgKIAAANIgRAKg");
        this.shape_107.setTransform(-17.6, 58.2);

        this.instance_5 = new lib.Path_44();
        this.instance_5.parent = this;
        this.instance_5.setTransform(-19, 58.1, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_108 = new cjs.Shape();
        this.shape_108.graphics.f("#E7E3E3").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape_108.setTransform(-13.9, 56.9);

        this.shape_109 = new cjs.Shape();
        this.shape_109.graphics.f("#919191").s().p("AgDAEIAAgMIAIAFIAAAMg");
        this.shape_109.setTransform(-14.7, 58);

        this.shape_110 = new cjs.Shape();
        this.shape_110.graphics.f("#C5C6C8").s().p("AgIAAIARgKIAAALIgRAKg");
        this.shape_110.setTransform(-13.4, 57.8);

        this.instance_6 = new lib.Path_43();
        this.instance_6.parent = this;
        this.instance_6.setTransform(-14.8, 57.7, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_111 = new cjs.Shape();
        this.shape_111.graphics.f("#E7E3E3").s().p("AgMACIAQgJIAJAGIgQAJg");
        this.shape_111.setTransform(-15.7, 55.8);

        this.shape_112 = new cjs.Shape();
        this.shape_112.graphics.f("#919191").s().p("AgDAEIAAgMIAIAFIAAAMg");
        this.shape_112.setTransform(-16.5, 57);

        this.shape_113 = new cjs.Shape();
        this.shape_113.graphics.f("#C5C6C8").s().p("AgIAAIARgKIAAALIgRAKg");
        this.shape_113.setTransform(-15.2, 56.8);

        this.instance_7 = new lib.Path_42();
        this.instance_7.parent = this;
        this.instance_7.setTransform(-16.6, 56.7, 1, 1, 0, 0, 0, 1.3, 0.8);

        this.shape_114 = new cjs.Shape();
        this.shape_114.graphics.f("#E7E3E3").s().p("AggAGIAqgYIAXANIgqAYg");
        this.shape_114.setTransform(62.3, 17.9);

        this.shape_115 = new cjs.Shape();
        this.shape_115.graphics.f("#919191").s().p("AgKAKIAAggIAVANIAAAgg");
        this.shape_115.setTransform(60.2, 20.8);

        this.shape_116 = new cjs.Shape();
        this.shape_116.graphics.f("#C5C6C8").s().p("AgUgDIApgYIAAAeIgpAag");
        this.shape_116.setTransform(63.5, 20.2);

        this.instance_8 = new lib.Path_41();
        this.instance_8.parent = this;
        this.instance_8.setTransform(60.1, 19.9, 1, 1, 0, 0, 0, 3.3, 1.9);

        this.shape_117 = new cjs.Shape();
        this.shape_117.graphics.f("#9FA7AB").s().p("AgegLIAAgHIA9AiIgHADg");
        this.shape_117.setTransform(-13.1, 56.2);

        this.shape_118 = new cjs.Shape();
        this.shape_118.graphics.f("#BDCACF").s().p("AAAgLIg2AeIgGgDIA8giIA9AiIgGADg");
        this.shape_118.setTransform(-16.2, 56.2);

        this.shape_119 = new cjs.Shape();
        this.shape_119.graphics.f("#929296").s().p("Ag8AAIA8giIA9AiIg9Akg");
        this.shape_119.setTransform(-16.2, 57.8);

        this.shape_120 = new cjs.Shape();
        this.shape_120.graphics.f("#DFE1EB").s().p("AhLAAIBLgrIBMArIhMAsg");
        this.shape_120.setTransform(-16.2, 57.8);

        this.shape_121 = new cjs.Shape();
        this.shape_121.graphics.f("#5685AE").s().p("AglgQIAAgKIBLArIAAAKg");
        this.shape_121.setTransform(-20, 60.5);

        this.shape_122 = new cjs.Shape();
        this.shape_122.graphics.f("#8AD3F5").s().p("AglguIA7AwIAQgIIAAAJIhLAsg");
        this.shape_122.setTransform(-12.3, 58.5);

        this.shape_123 = new cjs.Shape();
        this.shape_123.graphics.f("#A2ACB3").s().p("AgcgPIADgCIA2AfIAAAEg");
        this.shape_123.setTransform(-44.3, 46.3);

        this.shape_124 = new cjs.Shape();
        this.shape_124.graphics.f("#4E575C").s().p("AgcgJIAAgOIADABIAAALIA2AfIAAAEg");
        this.shape_124.setTransform(-44.3, 45.7);

        this.shape_125 = new cjs.Shape();
        this.shape_125.graphics.lf(["#223063", "#445E72"], [0.188, 0.941], 0.1, 16.5, -0.1, -21.6).s().p("AgcgJIAAgOIA5AhIAAAOg");
        this.shape_125.setTransform(-44.3, 45.7);

        this.shape_126 = new cjs.Shape();
        this.shape_126.graphics.f("#7B7B7B").s().p("AghgGIAAgaIBDAnIAAAag");
        this.shape_126.setTransform(-44.3, 45.7);

        this.shape_127 = new cjs.Shape();
        this.shape_127.graphics.f("#A2ACB3").s().p("AgcgPIADgCIA2AfIAAAEg");
        this.shape_127.setTransform(-32.6, 53.1);

        this.shape_128 = new cjs.Shape();
        this.shape_128.graphics.f("#4E575C").s().p("AgcgJIAAgOIADABIAAALIA2AfIAAAEg");
        this.shape_128.setTransform(-32.6, 52.4);

        this.shape_129 = new cjs.Shape();
        this.shape_129.graphics.lf(["#223063", "#445E72"], [0.188, 0.941], 0.1, 16.4, -0.1, -21.7).s().p("AgcgJIAAgOIA5AhIAAAOg");
        this.shape_129.setTransform(-32.6, 52.4);

        this.shape_130 = new cjs.Shape();
        this.shape_130.graphics.f("#7B7B7B").s().p("AghgGIAAgaIBDAnIAAAag");
        this.shape_130.setTransform(-32.6, 52.4);

        this.shape_131 = new cjs.Shape();
        this.shape_131.graphics.f("#E7F6FD").s().p("AgPAIIAdgRIACABIgfASg");
        this.shape_131.setTransform(-12.6, 63.6);

        this.shape_132 = new cjs.Shape();
        this.shape_132.graphics.f("#8D9DA8").s().p("AgPAMIAdgRIAAgHIACgBIAAAJIgfARg");
        this.shape_132.setTransform(-12.6, 63.2);

        this.shape_133 = new cjs.Shape();
        this.shape_133.graphics.lf(["#2E54A5", "#709BBF"], [0.188, 0.941], 0, -12, 0.1, 9.2).s().p("AgPAFIAfgSIAAAJIgfARg");
        this.shape_133.setTransform(-12.6, 63.2);

        this.shape_134 = new cjs.Shape();
        this.shape_134.graphics.f("#C8C7C6").s().p("AgSAEIAlgVIAAAOIglAVg");
        this.shape_134.setTransform(-12.6, 63.2);

        this.shape_135 = new cjs.Shape();
        this.shape_135.graphics.f("#E7F6FD").s().p("AgcAOIA2gfIADACIg5Ahg");
        this.shape_135.setTransform(-19.1, 53);

        this.shape_136 = new cjs.Shape();
        this.shape_136.graphics.f("#8D9DA8").s().p("AgcAUIA2gfIAAgLIADgBIAAAOIg5Ahg");
        this.shape_136.setTransform(-19.1, 52.3);

        this.shape_137 = new cjs.Shape();
        this.shape_137.graphics.lf(["#2E54A5", "#709BBF"], [0.188, 0.941], -0.1, -21.7, 0.1, 16.4).s().p("AgcAKIA5ghIAAAOIg5Ahg");
        this.shape_137.setTransform(-19.1, 52.3);

        this.shape_138 = new cjs.Shape();
        this.shape_138.graphics.f("#C8C7C6").s().p("AghAHIBDgnIAAAaIhDAng");
        this.shape_138.setTransform(-19.1, 52.3);

        this.shape_139 = new cjs.Shape();
        this.shape_139.graphics.f("#BDCACF").s().p("AhLAgICYhXIAAAYIiYBYg");
        this.shape_139.setTransform(20.6, -3.6);

        this.shape_140 = new cjs.Shape();
        this.shape_140.graphics.f("#E7E3E3").s().p("AhEAlICChKIAHABIiCBKg");
        this.shape_140.setTransform(20.2, -4.9);

        this.shape_141 = new cjs.Shape();
        this.shape_141.graphics.f("#E7E3E3").s().p("AhvAkICYhYIBHARIiYBYg");
        this.shape_141.setTransform(17, -5.7);

        this.shape_142 = new cjs.Shape();
        this.shape_142.graphics.f("#9FA7AB").s().p("AgjgUIBHARIAAAYg");
        this.shape_142.setTransform(9.3, 0);

        this.shape_143 = new cjs.Shape();
        this.shape_143.graphics.f("#BDCACF").s().p("AhMAgICZhXIAAAYIiZBXg");
        this.shape_143.setTransform(13.4, -7.7);

        this.shape_144 = new cjs.Shape();
        this.shape_144.graphics.f("#E7E3E3").s().p("AhEAlICChLIAHACIiCBKg");
        this.shape_144.setTransform(13.1, -9);

        this.shape_145 = new cjs.Shape();
        this.shape_145.graphics.f("#E7E3E3").s().p("AhwAjICZhXIBIARIiZBYg");
        this.shape_145.setTransform(9.9, -9.8);

        this.shape_146 = new cjs.Shape();
        this.shape_146.graphics.f("#9FA7AB").s().p("AgjgUIBHARIAAAYg");
        this.shape_146.setTransform(2.2, -4.2);

        this.shape_147 = new cjs.Shape();
        this.shape_147.graphics.f("#BDCACF").s().p("AhMAgICZhXIAAAYIiZBXg");
        this.shape_147.setTransform(6.3, -11.9);

        this.shape_148 = new cjs.Shape();
        this.shape_148.graphics.f("#E7E3E3").s().p("AhEAlICChKIAHABIiCBKg");
        this.shape_148.setTransform(5.9, -13.2);

        this.shape_149 = new cjs.Shape();
        this.shape_149.graphics.f("#E7E3E3").s().p("AhvAkICYhYIBHASIiYBXg");
        this.shape_149.setTransform(2.7, -14);

        this.shape_150 = new cjs.Shape();
        this.shape_150.graphics.f("#9FA7AB").s().p("AgjgUIBGASIAAAXg");
        this.shape_150.setTransform(-4.9, -8.4);

        this.shape_151 = new cjs.Shape();
        this.shape_151.graphics.f("#BDCACF").s().p("AhMAgICYhYIAAAZIiYBYg");
        this.shape_151.setTransform(-0.8, -16.1);

        this.shape_152 = new cjs.Shape();
        this.shape_152.graphics.f("#E7E3E3").s().p("AhEAlICChKIAHACIiCBJg");
        this.shape_152.setTransform(-1.2, -17.4);

        this.shape_153 = new cjs.Shape();
        this.shape_153.graphics.f("#E7E3E3").s().p("AhvAkICYhYIBHARIiYBYg");
        this.shape_153.setTransform(-4.4, -18.2);

        this.shape_154 = new cjs.Shape();
        this.shape_154.graphics.f("#9FA7AB").s().p("AgjgUIBHARIAAAYg");
        this.shape_154.setTransform(-12.1, -12.5);

        this.shape_155 = new cjs.Shape();
        this.shape_155.graphics.f("#6892A6").s().p("AgIAOQgKgDgDgHQgCgEADgFQAEgGAIgCQAIgDAJACQALADADAHQACAFgEAEQgDAGgJADQgFACgEAAIgIgCgAgHgNQgJADgDAFQgEAFAEAFQADAGAJACQAHACAJgCQAIgDAEgFQADgFgDgFQgEgFgIgDIgJgBIgHABg");
        this.shape_155.setTransform(38.7, -7.7);

        this.instance_9 = new lib.ClipGroup_54();
        this.instance_9.parent = this;
        this.instance_9.setTransform(38.4, -8.2, 1, 1, 0, 0, 0, 2.6, 2);

        this.shape_156 = new cjs.Shape();
        this.shape_156.graphics.f("#D9D9D8").s().p("AAAACQgBAAAAgBQgBAAAAAAQAAgBAAAAQAAAAAAAAQAAAAABgBQAAAAABAAQAAAAAAAAQABAAAAAAQABAAAAABQABAAAAAAQAAAAAAAAQAAAAAAABQAAAAgBAAQAAABAAAAQAAAAgBAAQAAAAAAAAIgBAAg");
        this.shape_156.setTransform(38.7, -7.7);

        this.shape_157 = new cjs.Shape();
        this.shape_157.graphics.f("#D2D2D2").s().p("AgDAGQgBAAAAgBQgBAAAAAAQAAgBAAAAQAAAAAAgBIADgDIAIgGIgEAIIgCAEIAAABIgDgBg");
        this.shape_157.setTransform(38.1, -7.1);

        this.shape_158 = new cjs.Shape();
        this.shape_158.graphics.f("#D2D2D2").s().p("AgBgBQABgGAEACQABAAAAABQABAAAAAAQAAAAAAABQAAAAAAABIgDACIgIAHQABgGADgCg");
        this.shape_158.setTransform(39.3, -8.4);

        this.shape_159 = new cjs.Shape();
        this.shape_159.graphics.f("#D2D2D2").s().p("AgDABIgFgBQAAAAAAAAQgBAAAAAAQAAAAAAgBQAAAAABgBQACgCAGACIAKAGIgNgDg");
        this.shape_159.setTransform(37.7, -8.1);

        this.shape_160 = new cjs.Shape();
        this.shape_160.graphics.f("#D2D2D2").s().p("AABACIgKgFIANACQAHABgCACQAAABgBAAQAAAAAAABQgBAAAAAAQgBAAAAAAQgCAAgDgCg");
        this.shape_160.setTransform(39.7, -7.3);

        this.shape_161 = new cjs.Shape();
        this.shape_161.graphics.f("#767676").s().p("AgGALQgHgCgCgEQgDgFADgDQADgFAGgCQAGgCAHACQAHACADAEQACAEgCAEQgDAFgHACIgHABIgGgBg");
        this.shape_161.setTransform(38.8, -7.4);

        this.shape_162 = new cjs.Shape();
        this.shape_162.graphics.f("#4F4F50").s().p("AgHAOQgJgCgEgGQgDgGADgEQAEgGAJgCQAHgCAJACQAIACAEAFQADAFgDAFQgEAGgIACIgJABIgHAAg");
        this.shape_162.setTransform(38.8, -7.7);

        this.shape_163 = new cjs.Shape();
        this.shape_163.graphics.f("#BDCACF").s().p("AgUADIApgXIAAASIgpAYg");
        this.shape_163.setTransform(40.9, -5.4);

        this.shape_164 = new cjs.Shape();
        this.shape_164.graphics.f("#9FA7AB").s().p("AgVgCIAAgTIArAYIAAATg");
        this.shape_164.setTransform(36.6, -5.5);

        this.shape_165 = new cjs.Shape();
        this.shape_165.graphics.f("#E7E3E3").s().p("AgqAAIAqgYIArAZIgqAYg");
        this.shape_165.setTransform(38.7, -7.6);

        this.instance_10 = new lib.Path_12_0();
        this.instance_10.parent = this;
        this.instance_10.setTransform(36.5, -7.1, 1, 1, 0, 0, 0, 4.3, 2.5);

        this.shape_166 = new cjs.Shape();
        this.shape_166.graphics.f("#343C59").s().p("Ah+ADIAAiZID9CUIAACZg");
        this.shape_166.setTransform(-38.3, 53.1);

        this.shape_167 = new cjs.Shape();
        this.shape_167.graphics.f("#DFE1EB").s().p("AhEAAIBEgnIBFAnIhFAog");
        this.shape_167.setTransform(-16.2, 58.2);

        this.shape_168 = new cjs.Shape();
        this.shape_168.graphics.f("#343C59").s().p("AgiAWIAAhUIBFAoIAABVg");
        this.shape_168.setTransform(-19.6, 64.5);

        this.shape_169 = new cjs.Shape();
        this.shape_169.graphics.f("#7C8392").s().p("AgigWIBFgoIAABUIhFApg");
        this.shape_169.setTransform(-12.7, 64.5);

        this.shape_170 = new cjs.Shape();
        this.shape_170.graphics.f("#7C8392").s().p("Ag9goIB7hIIAACZIh7BIg");
        this.shape_170.setTransform(-19.4, 56.9);

        this.shape_171 = new cjs.Shape();
        this.shape_171.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh7BHg");
        this.shape_171.setTransform(-19.6, 46.1);

        this.shape_172 = new cjs.Shape();
        this.shape_172.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_172.setTransform(-19.4, 47.8);

        this.shape_173 = new cjs.Shape();
        this.shape_173.graphics.f("#46718B").s().p("AgIAFIANgUIAEACIAAAdg");
        this.shape_173.setTransform(-26.5, 51.2);

        this.shape_174 = new cjs.Shape();
        this.shape_174.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEACIh7BHg");
        this.shape_174.setTransform(-21.6, 45);

        this.shape_175 = new cjs.Shape();
        this.shape_175.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_175.setTransform(-21.3, 46.6);

        this.shape_176 = new cjs.Shape();
        this.shape_176.graphics.f("#46718B").s().p("AgJAFIAOgUIAEACIAAAdg");
        this.shape_176.setTransform(-28.5, 50.1);

        this.shape_177 = new cjs.Shape();
        this.shape_177.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh6BHg");
        this.shape_177.setTransform(-23.6, 43.8);

        this.shape_178 = new cjs.Shape();
        this.shape_178.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_178.setTransform(-23.3, 45.5);

        this.shape_179 = new cjs.Shape();
        this.shape_179.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_179.setTransform(-30.4, 48.9);

        this.shape_180 = new cjs.Shape();
        this.shape_180.graphics.f("#E7F6FD").s().p("AhAAjIB8hHIAEADIh6BHg");
        this.shape_180.setTransform(-25.5, 42.7);

        this.shape_181 = new cjs.Shape();
        this.shape_181.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_181.setTransform(-25.3, 44.3);

        this.shape_182 = new cjs.Shape();
        this.shape_182.graphics.f("#46718B").s().p("AgJAFIANgUIAGADIAAAcg");
        this.shape_182.setTransform(-32.4, 47.8);

        this.shape_183 = new cjs.Shape();
        this.shape_183.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEACIh6BHg");
        this.shape_183.setTransform(-27.5, 41.6);

        this.shape_184 = new cjs.Shape();
        this.shape_184.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_184.setTransform(-27.3, 43.2);

        this.shape_185 = new cjs.Shape();
        this.shape_185.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_185.setTransform(-34.4, 46.7);

        this.shape_186 = new cjs.Shape();
        this.shape_186.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh7BHg");
        this.shape_186.setTransform(-29.5, 40.4);

        this.shape_187 = new cjs.Shape();
        this.shape_187.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_187.setTransform(-29.2, 42);

        this.shape_188 = new cjs.Shape();
        this.shape_188.graphics.f("#46718B").s().p("AgIAFIANgUIAEACIAAAdg");
        this.shape_188.setTransform(-36.4, 45.5);

        this.shape_189 = new cjs.Shape();
        this.shape_189.graphics.f("#E7F6FD").s().p("AhAAjIB7hHIAGACIh8BHg");
        this.shape_189.setTransform(-31.4, 39.3);

        this.shape_190 = new cjs.Shape();
        this.shape_190.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_190.setTransform(-31.2, 40.9);

        this.shape_191 = new cjs.Shape();
        this.shape_191.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_191.setTransform(-38.3, 44.4);

        this.shape_192 = new cjs.Shape();
        this.shape_192.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh6BHg");
        this.shape_192.setTransform(-33.4, 38.1);

        this.shape_193 = new cjs.Shape();
        this.shape_193.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_193.setTransform(-33.2, 39.8);

        this.shape_194 = new cjs.Shape();
        this.shape_194.graphics.f("#46718B").s().p("AgJAFIAOgUIAEACIAAAdg");
        this.shape_194.setTransform(-40.3, 43.2);

        this.shape_195 = new cjs.Shape();
        this.shape_195.graphics.f("#E7F6FD").s().p("Ag/AjIB6hHIAFACIh7BHg");
        this.shape_195.setTransform(-35.4, 37);

        this.shape_196 = new cjs.Shape();
        this.shape_196.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_196.setTransform(-35.1, 38.6);

        this.shape_197 = new cjs.Shape();
        this.shape_197.graphics.f("#46718B").s().p("AgIAFIANgUIAEACIAAAdg");
        this.shape_197.setTransform(-42.3, 42.1);

        this.shape_198 = new cjs.Shape();
        this.shape_198.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEADIh6BGg");
        this.shape_198.setTransform(-37.4, 35.8);

        this.shape_199 = new cjs.Shape();
        this.shape_199.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_199.setTransform(-37.1, 37.5);

        this.shape_200 = new cjs.Shape();
        this.shape_200.graphics.f("#46718B").s().p("AgJAFIAOgUIAEACIAAAdg");
        this.shape_200.setTransform(-44.2, 40.9);

        this.shape_201 = new cjs.Shape();
        this.shape_201.graphics.f("#E7F6FD").s().p("Ag/AiIB6hGIAFACIh6BHg");
        this.shape_201.setTransform(-39.3, 34.7);

        this.shape_202 = new cjs.Shape();
        this.shape_202.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_202.setTransform(-39.1, 36.3);

        this.shape_203 = new cjs.Shape();
        this.shape_203.graphics.f("#46718B").s().p("AgIAFIAMgUIAGADIAAAcg");
        this.shape_203.setTransform(-46.2, 39.8);

        this.shape_204 = new cjs.Shape();
        this.shape_204.graphics.f("#E7F6FD").s().p("AhAAjIB8hHIAEACIh6BHg");
        this.shape_204.setTransform(-41.3, 33.6);

        this.shape_205 = new cjs.Shape();
        this.shape_205.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_205.setTransform(-41, 35.2);

        this.shape_206 = new cjs.Shape();
        this.shape_206.graphics.f("#46718B").s().p("AgJAFIANgUIAGACIAAAdg");
        this.shape_206.setTransform(-48.2, 38.7);

        this.shape_207 = new cjs.Shape();
        this.shape_207.graphics.f("#E7F6FD").s().p("Ag/AjIB7hHIAEACIh6BHg");
        this.shape_207.setTransform(-43.3, 32.4);

        this.shape_208 = new cjs.Shape();
        this.shape_208.graphics.f("#BAD1E0").s().p("Ag9AVIB7hHIAAAeIh7BHg");
        this.shape_208.setTransform(-43, 34.1);

        this.shape_209 = new cjs.Shape();
        this.shape_209.graphics.f("#46718B").s().p("AgIAFIAMgUIAGACIAAAdg");
        this.shape_209.setTransform(-50.1, 37.5);

        this.shape_210 = new cjs.Shape();
        this.shape_210.graphics.f("#F2F3FA").s().p("AAAAWQgOAAgMgHQgLgGAAgJQAAgIALgHQALgGAPAAQAPAAAMAHQALAGAAAIQAAAJgLAHQgKAGgOAAIgDAAg");
        this.shape_210.setTransform(45.9, 27);

        this.shape_211 = new cjs.Shape();
        this.shape_211.graphics.f("#DFE1EB").s().p("AgjAVQgQgJAAgMQAAgMAPgIQAPgJAVAAQAVAAAPAJQAPAJAAALQABANgPAIQgPAJgWAAQgUgBgPgIg");
        this.shape_211.setTransform(45.9, 28.1);

        this.shape_212 = new cjs.Shape();
        this.shape_212.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.1, 0, 5.2, 0).s().p("AgMCAQgLgCgMgGQgQgKABgNIAAAAIAAjhIBlAAIAADhQADAOgRAJQgPAJgTAAIgPgBg");
        this.shape_212.setTransform(45.9, 41);

        this.shape_213 = new cjs.Shape();
        this.shape_213.graphics.f("#ACACAC").s().p("ABGAMQgGgIgMgHIgDgBQgVgMgcAAQgeAAgWANQgSAKgDAOIAAgDQAAgQAVgLQAWgNAeAAQAcAAAVAMIADACQARAIAEANIABAHIAAABIgBABQAAgEgDgGg");
        this.shape_213.setTransform(46.1, 49.6);

        this.shape_214 = new cjs.Shape();
        this.shape_214.graphics.f("#565564").s().p("Ag2AUQgKgGgGgJIAGAEQAVANAgAAQAWAAASgHIALgFQATgKABgOQgBgGgCgFIADABIgEgIQALAJADAKIgCgBQADAGAAAEQgCAOgSALIgLAFQgTAHgVAAQgfAAgXgNg");
        this.shape_214.setTransform(46.3, 52.3);

        this.shape_215 = new cjs.Shape();
        this.shape_215.graphics.f("#565564").s().p("AgLAZIgegSQADgNARgKQAVgMAeAAQAJAAABACQACACAAAGIgCAMIgCAHQgCAEgHAIQgIAJgHAEQgGADgGAAQgGAAgHgEg");
        this.shape_215.setTransform(43, 50.2);

        this.shape_216 = new cjs.Shape();
        this.shape_216.graphics.f("#7E7F82").s().p("AgzAdQgTgMgCgPIAAgCQAAgQAUgMQAWgMAeAAQAcAAAVALIAEACQAQAJAEANIgDAAQADAFABAEQgCAPgSAKIgMAFQgTAHgVAAQgfAAgWgMg");
        this.shape_216.setTransform(46.1, 51.4);

        this.shape_217 = new cjs.Shape();
        this.shape_217.graphics.f("#9FA7AB").s().p("Ajsh6IAAgTIHaERIgRAJg");
        this.shape_217.setTransform(24.6, -20.5);

        this.shape_218 = new cjs.Shape();
        this.shape_218.graphics.f("#BDCACF").s().p("AiqBYIFVjEIAAAUIlVDFg");
        this.shape_218.setTransform(35.6, 3.8);

        this.shape_219 = new cjs.Shape();
        this.shape_219.graphics.f("#BDCACF").s().p("AhVh6IkeClIgRgJIEvivIHaESIgRAJg");
        this.shape_219.setTransform(9.5, -20.5);

        this.shape_220 = new cjs.Shape();
        this.shape_220.graphics.f("#929296").s().p("AmEgwIEviwIHaESIkvCug");
        this.shape_220.setTransform(9.5, -12.2);

        this.shape_221 = new cjs.Shape();
        this.shape_221.graphics.f("#DFE1EB").s().p("AmugyIFVjGIIJEsIlWDFg");
        this.shape_221.setTransform(9.5, -12.3);

        this.shape_222 = new cjs.Shape();
        this.shape_222.graphics.f("#9FA7AB").s().p("AkEiLIAAgVIIJEsIAAAUg");
        this.shape_222.setTransform(-7.6, -1.3);

        this.shape_223 = new cjs.Shape();
        this.shape_223.graphics.f("#5685AE").s().p("ABwEdIAAlrIjgiCIAAFqIg6ghIAAm3IA6AhIAAABIEbCkIAAG3g");
        this.shape_223.setTransform(1.4, 26.8);

        this.shape_224 = new cjs.Shape();
        this.shape_224.graphics.f("#8AD3F5").s().p("AgVjOIArgaIAAG3IgrAag");
        this.shape_224.setTransform(-7.6, 19);

        this.shape_225 = new cjs.Shape();
        this.shape_225.graphics.f("#98D9E6").s().p("Aiqh4IEbikIAAgBIA5ghIAAG3Ig5AhIAAlqIjgCCIAAFrIg7Aig");
        this.shape_225.setTransform(35.6, 26.8);

        this.shape_226 = new cjs.Shape();
        this.shape_226.graphics.f("#5685AE").s().p("AgVDPIAAm3IArAaIAAG3g");
        this.shape_226.setTransform(44.6, 19);

        this.shape_227 = new cjs.Shape();
        this.shape_227.graphics.f("#6B727B").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_227.setTransform(2.4, 14.5);

        this.shape_228 = new cjs.Shape();
        this.shape_228.graphics.f("#6B727B").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_228.setTransform(2.4, 21);

        this.shape_229 = new cjs.Shape();
        this.shape_229.graphics.f("#6B727B").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_229.setTransform(2.4, 27.5);

        this.shape_230 = new cjs.Shape();
        this.shape_230.graphics.f("#6B727B").s().p("AifhSIAAgSIE/C4IAAARg");
        this.shape_230.setTransform(2.4, 33.9);

        this.shape_231 = new cjs.Shape();
        this.shape_231.graphics.f("#6B727B").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_231.setTransform(2.4, 40.4);

        this.shape_232 = new cjs.Shape();
        this.shape_232.graphics.f("#676767").s().p("AgBi5IACACIABFxIgCAAg");
        this.shape_232.setTransform(-3.7, 21.9);

        this.shape_233 = new cjs.Shape();
        this.shape_233.graphics.f("#676767").s().p("AgBi5IACABIABFxIgDABg");
        this.shape_233.setTransform(-1.1, 23.4);

        this.shape_234 = new cjs.Shape();
        this.shape_234.graphics.f("#676767").s().p("AgBi5IACACIABFwIgCABg");
        this.shape_234.setTransform(1.5, 24.9);

        this.shape_235 = new cjs.Shape();
        this.shape_235.graphics.f("#676767").s().p("AgBi5IACACIABFxIgDAAg");
        this.shape_235.setTransform(4.1, 26.4);

        this.shape_236 = new cjs.Shape();
        this.shape_236.graphics.f("#676767").s().p("AgBi5IACABIABFxIgCABg");
        this.shape_236.setTransform(6.7, 27.9);

        this.shape_237 = new cjs.Shape();
        this.shape_237.graphics.f("#676767").s().p("AgBi5IACACIABFwIgCABg");
        this.shape_237.setTransform(9.3, 29.4);

        this.shape_238 = new cjs.Shape();
        this.shape_238.graphics.f("#676767").s().p("AgBi5IACACIABFxIgCAAg");
        this.shape_238.setTransform(11.9, 30.9);

        this.shape_239 = new cjs.Shape();
        this.shape_239.graphics.f("#444546").s().p("AgBC4IAAlxIACACIABFxg");
        this.shape_239.setTransform(-4, 21.9);

        this.shape_240 = new cjs.Shape();
        this.shape_240.graphics.f("#444546").s().p("AAAC4IgBlxIACACIABFxg");
        this.shape_240.setTransform(-1.4, 23.4);

        this.shape_241 = new cjs.Shape();
        this.shape_241.graphics.f("#444546").s().p("AAAC4IgBlxIACACIABFxg");
        this.shape_241.setTransform(1.2, 24.9);

        this.shape_242 = new cjs.Shape();
        this.shape_242.graphics.f("#444546").s().p("AAAC4IgBlxIACACIABFxg");
        this.shape_242.setTransform(3.8, 26.4);

        this.shape_243 = new cjs.Shape();
        this.shape_243.graphics.f("#444546").s().p("AAAC4IgBlxIACACIABFxg");
        this.shape_243.setTransform(6.4, 27.9);

        this.shape_244 = new cjs.Shape();
        this.shape_244.graphics.f("#444546").s().p("AAAC4IgBlxIACACIABFxg");
        this.shape_244.setTransform(9, 29.4);

        this.shape_245 = new cjs.Shape();
        this.shape_245.graphics.f("#444546").s().p("AAAC4IgBlxIACACIABFxg");
        this.shape_245.setTransform(11.6, 30.9);

        this.shape_246 = new cjs.Shape();
        this.shape_246.graphics.f("#979FA3").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_246.setTransform(2.6, 14.3);

        this.shape_247 = new cjs.Shape();
        this.shape_247.graphics.f("#979FA3").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_247.setTransform(2.6, 20.8);

        this.shape_248 = new cjs.Shape();
        this.shape_248.graphics.f("#979FA3").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_248.setTransform(2.6, 27.3);

        this.shape_249 = new cjs.Shape();
        this.shape_249.graphics.f("#979FA3").s().p("AifhTIAAgSIE/C5IAAASg");
        this.shape_249.setTransform(2.6, 33.8);

        this.shape_250 = new cjs.Shape();
        this.shape_250.graphics.f("#979FA3").s().p("AifhSIAAgSIE/C4IAAARg");
        this.shape_250.setTransform(2.6, 40.2);

        this.shape_251 = new cjs.Shape();
        this.shape_251.graphics.f("#87929D").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_251.setTransform(34.9, 14.5);

        this.shape_252 = new cjs.Shape();
        this.shape_252.graphics.f("#87929D").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_252.setTransform(34.9, 21);

        this.shape_253 = new cjs.Shape();
        this.shape_253.graphics.f("#87929D").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_253.setTransform(34.9, 27.5);

        this.shape_254 = new cjs.Shape();
        this.shape_254.graphics.f("#87929D").s().p("AifBUIE/i4IAAASIk/C3g");
        this.shape_254.setTransform(34.9, 33.9);

        this.shape_255 = new cjs.Shape();
        this.shape_255.graphics.f("#87929D").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_255.setTransform(34.9, 40.4);

        this.shape_256 = new cjs.Shape();
        this.shape_256.graphics.f("#676767").s().p("AgBC6IABlxIACgCIgBFzg");
        this.shape_256.setTransform(41, 21.9);

        this.shape_257 = new cjs.Shape();
        this.shape_257.graphics.f("#676767").s().p("AgBC5IABlxIACgBIgBFzg");
        this.shape_257.setTransform(38.4, 23.4);

        this.shape_258 = new cjs.Shape();
        this.shape_258.graphics.f("#676767").s().p("AgBC5IABlwIACgCIgBFzg");
        this.shape_258.setTransform(35.8, 24.9);

        this.shape_259 = new cjs.Shape();
        this.shape_259.graphics.f("#676767").s().p("AgBC6IABlxIACgCIgBFzg");
        this.shape_259.setTransform(33.2, 26.4);

        this.shape_260 = new cjs.Shape();
        this.shape_260.graphics.f("#676767").s().p("AgBC5IABlxIACgBIgBFzg");
        this.shape_260.setTransform(30.6, 27.9);

        this.shape_261 = new cjs.Shape();
        this.shape_261.graphics.f("#676767").s().p("AgBC5IABlwIACgCIgBFzg");
        this.shape_261.setTransform(28, 29.4);

        this.shape_262 = new cjs.Shape();
        this.shape_262.graphics.f("#676767").s().p("AgBC6IABlxIACgCIgBFzg");
        this.shape_262.setTransform(25.4, 30.9);

        this.shape_263 = new cjs.Shape();
        this.shape_263.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_263.setTransform(41.3, 21.9);

        this.shape_264 = new cjs.Shape();
        this.shape_264.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_264.setTransform(38.7, 23.4);

        this.shape_265 = new cjs.Shape();
        this.shape_265.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_265.setTransform(36.1, 24.9);

        this.shape_266 = new cjs.Shape();
        this.shape_266.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_266.setTransform(33.5, 26.4);

        this.shape_267 = new cjs.Shape();
        this.shape_267.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_267.setTransform(30.9, 27.9);

        this.shape_268 = new cjs.Shape();
        this.shape_268.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_268.setTransform(28.3, 29.4);

        this.shape_269 = new cjs.Shape();
        this.shape_269.graphics.f("#B4B6B8").s().p("AAAi3IACgCIgBFxIgCACg");
        this.shape_269.setTransform(25.7, 30.9);

        this.shape_270 = new cjs.Shape();
        this.shape_270.graphics.f("#D5E0E6").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_270.setTransform(34.7, 14.3);

        this.shape_271 = new cjs.Shape();
        this.shape_271.graphics.f("#D5E0E6").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_271.setTransform(34.7, 20.8);

        this.shape_272 = new cjs.Shape();
        this.shape_272.graphics.f("#D5E0E6").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_272.setTransform(34.7, 27.3);

        this.shape_273 = new cjs.Shape();
        this.shape_273.graphics.f("#D5E0E6").s().p("AifBUIE/i5IAAASIk/C5g");
        this.shape_273.setTransform(34.7, 33.8);

        this.shape_274 = new cjs.Shape();
        this.shape_274.graphics.f("#D5E0E6").s().p("AifBUIE/i4IAAASIk/C3g");
        this.shape_274.setTransform(34.7, 40.2);

        this.shape_275 = new cjs.Shape();
        this.shape_275.graphics.f("#FFFFFF").s().p("AifhnIE/i6IAAGKIk/C5g");
        this.shape_275.setTransform(34.9, 24.3);

        this.shape_276 = new cjs.Shape();
        this.shape_276.graphics.lf(["#266382", "#3B6F8F", "#6490B0", "#80ADCD", "#94C2E3", "#9BCBEE"], [0.161, 0.243, 0.447, 0.627, 0.776, 0.875], 4.4, -42.2, -1.7, 17.2).s().p("AifhnIE/i6IAAGKIk/C5g");
        this.shape_276.setTransform(34.9, 24.3);

        this.shape_277 = new cjs.Shape();
        this.shape_277.graphics.f("#2D4586").s().p("ACZEeIAAkzIgogYIAAgaIjgiCIAAFNIg7ghIAAmKIFVDFIAAGKg");
        this.shape_277.setTransform(-2.7, 21.2);

        this.shape_278 = new cjs.Shape();
        this.shape_278.graphics.f("#3B7493").s().p("ADAE0IAAk0IjgiBIAAEzIjdh/IAAmKIH7ElIAAGKg");
        this.shape_278.setTransform(-6.5, 18.9);

        this.shape_279 = new cjs.Shape();
        this.shape_279.graphics.f("#FFFFFF").s().p("AhwBZIAAkzIDgCCIAAEzg");
        this.shape_279.setTransform(1.4, 27.8);

        this.shape_280 = new cjs.Shape();
        this.shape_280.graphics.f("#9FA7AB").s().p("AhGAmICNhRIAAALIiDBMg");
        this.shape_280.setTransform(30.5, 2);

        this.shape_281 = new cjs.Shape();
        this.shape_281.graphics.f("#9FA7AB").s().p("AjNhwIAAgMIGcDtIAAAMg");
        this.shape_281.setTransform(41.6, 18.4);

        this.shape_282 = new cjs.Shape();
        this.shape_282.graphics.f("#9FA7AB").s().p("Ah7hnIiEBMIgKgGICOhRIGEDgIgJAFg");
        this.shape_282.setTransform(50, 9.1);

        this.shape_283 = new cjs.Shape();
        this.shape_283.graphics.f("#929296").s().p("AkJhHICOhRIGEDgIiNBRg");
        this.shape_283.setTransform(50, 12.9);

        this.shape_284 = new cjs.Shape();
        this.shape_284.graphics.f("#DFE1EB").s().p("AkihFICphiIGcDtIipBig");
        this.shape_284.setTransform(50, 12.9);

        this.shape_285 = new cjs.Shape();
        this.shape_285.graphics.f("#BDCACF").s().p("AhTArICnhhIAAAMIinBhg");
        this.shape_285.setTransform(70.7, 25.5);

        this.shape_286 = new cjs.Shape();
        this.shape_286.graphics.f("#3C5396").s().p("AjHguIAAiJIGPDmIAACKg");
        this.shape_286.setTransform(42, 25.8);

        this.shape_287 = new cjs.Shape();
        this.shape_287.graphics.f("#6EB6CC").s().p("AhPgVICghdIAACIIigBdg");
        this.shape_287.setTransform(70, 32.7);

        this.instance_11 = new lib.Path_28_0();
        this.instance_11.parent = this;
        this.instance_11.setTransform(-30.2, -17.8, 1, 1, 0, 0, 0, 29.3, 16.9);

        this.shape_288 = new cjs.Shape();
        this.shape_288.graphics.lf(["#BD3938", "#E24747", "#E24747", "#F48076", "#F48076", "#F8AFA2"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgbAbIgLgFQgMgIgDgKIAAgiQADALAMAHIALAFQAUAHAVgEQAOgDAKgFIAFgDQAFgFADgFIACgFIABAAIAAAeQAAAEgDAFQgCAEgGAFIgFADQgLAHgNACIgPABQgOAAgMgEg");
        this.shape_288.setTransform(46.3, -42);

        this.shape_289 = new cjs.Shape();
        this.shape_289.graphics.lf(["#BD3938", "#E24747", "#E24747", "#F48076", "#F48076", "#F8AFA2"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgbAbIgLgFQgMgIgDgKIAAghQADAKAMAHIALAFQAUAHAVgEQAMgCAMgGIAFgDQAFgFADgFIACgFIABAAIAAAeQAAAEgDAFQgCAEgGAFIgFAEQgKAFgOADIgPABQgNAAgNgEg");
        this.shape_289.setTransform(46.3, -53.8);

        this.shape_290 = new cjs.Shape();
        this.shape_290.graphics.f("#B7B9BB").s().p("AgYAVQgOgIAAgLQAAgKAPgIQAMgIASAAQASgBAOAGIg0Atg");
        this.shape_290.setTransform(45.4, -65.9);

        this.shape_291 = new cjs.Shape();
        this.shape_291.graphics.f("#8D8F92").s().p("AghAUQgOgIAAgLQAAgLAOgIQAOgIATAAQAUAAAOAIQAOAIAAAKQAAALgOAIQgOAIgUAAQgTAAgOgHg");
        this.shape_291.setTransform(46.3, -65.7);

        this.shape_292 = new cjs.Shape();
        this.shape_292.graphics.f("#EFEFF0").s().p("AglAWQgQgKAAgMQAAgMAPgJQAQgJAWAAQAWAAAQAJQAQAJAAAMQAAANgQAJQgPAJgXAAQgVAAgQgJg");
        this.shape_292.setTransform(46.3, -65.6);

        this.shape_293 = new cjs.Shape();
        this.shape_293.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgEFlIgBgBIgCAAIgDAAIgDAAIgEgBIgBAAIgBgBIgBAAIgEgBIgMgFIgEgDIAAAAIgBgBIgCgBIgCgCIgEgFIgBgCQgCgEAAgFIAAgCIgBAAIAAqnIADAJQADAFAGAFIAFADQAJAGAOACQAVAEAUgHIALgFQAMgHADgKIAAKiQACAPgRAKIgLAFIgBAAQgGACgGABIgHABg");
        this.shape_293.setTransform(46.3, -18.2);

        this.shape_294 = new cjs.Shape();
        this.shape_294.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgNA3QgNgBgKgHIgFgDQgGgEgDgGIgCgFIgBAAIAAhVIBrAAIAABWQgDAKgMAIIgLAFQgNAEgNAAIgPgCg");
        this.shape_294.setTransform(46.3, -59.9);

        this.shape_295 = new cjs.Shape();
        this.shape_295.graphics.lf(["#BD3938", "#E24747", "#E24747", "#F48076", "#F48076", "#F8AFA2"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgbAbIgLgFQgMgHgCgLIAAghQACAKAMAHIALAFQAUAHAVgEQAOgDAKgFIAEgDQAHgFACgFIACgFIABAAIAAAeQAAAEgDAFQgCAEgHAFIgEADQgJAGgPADIgPABQgOAAgMgEg");
        this.shape_295.setTransform(12.7, -60.1);

        this.shape_296 = new cjs.Shape();
        this.shape_296.graphics.lf(["#BD3938", "#E24747", "#E24747", "#F48076", "#F48076", "#F8AFA2"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgbAbQgFgCgGgDQgMgHgCgLIAAghQACAKAMAHIALAFQAUAHAVgEQANgCALgGIAEgDQAHgFACgFIACgFIABAAIAAAeQAAAEgDAFQgCAEgHAFIgEAEQgKAFgOADIgPABQgNAAgNgEg");
        this.shape_296.setTransform(12.7, -71.9);

        this.shape_297 = new cjs.Shape();
        this.shape_297.graphics.f("#B7B9BB").s().p("AgYAVQgOgIAAgLQABgKAOgIQANgIARAAQASgBAOAGIgzAtg");
        this.shape_297.setTransform(11.7, -84);

        this.shape_298 = new cjs.Shape();
        this.shape_298.graphics.f("#8D8F92").s().p("AghATQgOgHAAgMQAAgKAOgIQAPgIASAAQAUgBAOAIQAOAJAAAKQAAALgOAIQgPAIgTAAIgCAAQgSAAgNgIg");
        this.shape_298.setTransform(12.7, -83.8);

        this.shape_299 = new cjs.Shape();
        this.shape_299.graphics.f("#EFEFF0").s().p("AglAWQgQgKAAgMQAAgMAQgJQAPgJAWAAQAWAAAQAJQAQAJAAAMQAAANgQAJQgPAJgXAAQgVAAgQgJg");
        this.shape_299.setTransform(12.7, -83.7);

        this.shape_300 = new cjs.Shape();
        this.shape_300.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgEFlIgBgBIgDAAIgCAAIgDAAIgEgBIgCAAIAAgBIgBAAIgEgBIgNgFIgDgDIgBAAIAAgBIgBAAIgBgBIgCgCIgFgFIAAgCQgDgFAAgEIAAqpQAAAEADAFQACAFAGAFIAFADQAJAGAPACQAVAEAUgHQAFgBAFgEQANgHACgKIAAKiQADAPgSAKIgKAFIgBAAIgMADIgHABg");
        this.shape_300.setTransform(12.7, -36.3);

        this.shape_301 = new cjs.Shape();
        this.shape_301.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.3, 0, 5.4, 0).s().p("AgNA3QgNgBgLgHIgEgDQgGgEgDgGIgCgFIAAAAIAAhVIBqAAIAABWQgDALgMAHIgLAFQgNAEgNAAQgHAAgIgCg");
        this.shape_301.setTransform(12.7, -78);

        this.shape_302 = new cjs.Shape();
        this.shape_302.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape_302.setTransform(-44.7, 0.1);

        this.shape_303 = new cjs.Shape();
        this.shape_303.graphics.f("#919191").s().p("AgHAGIAAgVIAPAJIAAAWg");
        this.shape_303.setTransform(-46.3, 2.1);

        this.shape_304 = new cjs.Shape();
        this.shape_304.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_304.setTransform(-44, 1.7);

        this.instance_12 = new lib.Path_40();
        this.instance_12.parent = this;
        this.instance_12.setTransform(-46.3, 1.4, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_305 = new cjs.Shape();
        this.shape_305.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape_305.setTransform(-47.9, -1.7);

        this.shape_306 = new cjs.Shape();
        this.shape_306.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_306.setTransform(-49.4, 0.3);

        this.shape_307 = new cjs.Shape();
        this.shape_307.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_307.setTransform(-47.1, -0.1);

        this.instance_13 = new lib.Path_39();
        this.instance_13.parent = this;
        this.instance_13.setTransform(-49.5, -0.4, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_308 = new cjs.Shape();
        this.shape_308.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgeAQg");
        this.shape_308.setTransform(-40.5, -2.4);

        this.shape_309 = new cjs.Shape();
        this.shape_309.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_309.setTransform(-42, -0.4);

        this.shape_310 = new cjs.Shape();
        this.shape_310.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_310.setTransform(-39.7, -0.8);

        this.instance_14 = new lib.Path_38();
        this.instance_14.parent = this;
        this.instance_14.setTransform(-42.1, -1, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_311 = new cjs.Shape();
        this.shape_311.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_311.setTransform(-43.7, -4.2);

        this.shape_312 = new cjs.Shape();
        this.shape_312.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_312.setTransform(-45.2, -2.2);

        this.shape_313 = new cjs.Shape();
        this.shape_313.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_313.setTransform(-42.9, -2.6);

        this.instance_15 = new lib.Path_37();
        this.instance_15.parent = this;
        this.instance_15.setTransform(-45.2, -2.8, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_314 = new cjs.Shape();
        this.shape_314.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_314.setTransform(-51.1, -3.6);

        this.shape_315 = new cjs.Shape();
        this.shape_315.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_315.setTransform(-52.6, -1.5);

        this.shape_316 = new cjs.Shape();
        this.shape_316.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_316.setTransform(-50.3, -1.9);

        this.instance_16 = new lib.Path_36();
        this.instance_16.parent = this;
        this.instance_16.setTransform(-52.6, -2.2, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_317 = new cjs.Shape();
        this.shape_317.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_317.setTransform(-54.2, -5.4);

        this.shape_318 = new cjs.Shape();
        this.shape_318.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_318.setTransform(-55.7, -3.4);

        this.shape_319 = new cjs.Shape();
        this.shape_319.graphics.f("#C5C6C8").s().p("AgOgBIAdgSIAAAWIgdARg");
        this.shape_319.setTransform(-53.4, -3.8);

        this.instance_17 = new lib.Path_35();
        this.instance_17.parent = this;
        this.instance_17.setTransform(-55.8, -4, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_320 = new cjs.Shape();
        this.shape_320.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_320.setTransform(-46.8, -6);

        this.shape_321 = new cjs.Shape();
        this.shape_321.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_321.setTransform(-48.3, -4);

        this.shape_322 = new cjs.Shape();
        this.shape_322.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_322.setTransform(-46, -4.4);

        this.instance_18 = new lib.Path_34();
        this.instance_18.parent = this;
        this.instance_18.setTransform(-48.4, -4.7, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_323 = new cjs.Shape();
        this.shape_323.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_323.setTransform(-50, -7.8);

        this.shape_324 = new cjs.Shape();
        this.shape_324.graphics.f("#919191").s().p("AgHAHIAAgWIAPAKIAAAVg");
        this.shape_324.setTransform(-51.5, -5.8);

        this.shape_325 = new cjs.Shape();
        this.shape_325.graphics.f("#C5C6C8").s().p("AgOgBIAdgSIAAAVIgdASg");
        this.shape_325.setTransform(-49.2, -6.2);

        this.instance_19 = new lib.Path_33();
        this.instance_19.parent = this;
        this.instance_19.setTransform(-51.5, -6.5, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_326 = new cjs.Shape();
        this.shape_326.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_326.setTransform(-57.4, -7.2);

        this.shape_327 = new cjs.Shape();
        this.shape_327.graphics.f("#919191").s().p("AgHAHIAAgWIAPAKIAAAVg");
        this.shape_327.setTransform(-58.9, -5.2);

        this.shape_328 = new cjs.Shape();
        this.shape_328.graphics.f("#C5C6C8").s().p("AgOgBIAdgSIAAAVIgdASg");
        this.shape_328.setTransform(-56.6, -5.6);

        this.instance_20 = new lib.Path_32();
        this.instance_20.parent = this;
        this.instance_20.setTransform(-58.9, -5.8, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_329 = new cjs.Shape();
        this.shape_329.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_329.setTransform(-60.5, -9);

        this.shape_330 = new cjs.Shape();
        this.shape_330.graphics.f("#919191").s().p("AgHAGIAAgVIAPAJIAAAWg");
        this.shape_330.setTransform(-62, -7);

        this.shape_331 = new cjs.Shape();
        this.shape_331.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_331.setTransform(-59.7, -7.4);

        this.instance_21 = new lib.Path_31();
        this.instance_21.parent = this;
        this.instance_21.setTransform(-62.1, -7.7, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_332 = new cjs.Shape();
        this.shape_332.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_332.setTransform(-53.1, -9.7);

        this.shape_333 = new cjs.Shape();
        this.shape_333.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_333.setTransform(-54.6, -7.6);

        this.shape_334 = new cjs.Shape();
        this.shape_334.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_334.setTransform(-52.3, -8.1);

        this.instance_22 = new lib.Path_30();
        this.instance_22.parent = this;
        this.instance_22.setTransform(-54.7, -8.3, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_335 = new cjs.Shape();
        this.shape_335.graphics.f("#E7E3E3").s().p("AgWAEIAdgQIAQAJIgdAQg");
        this.shape_335.setTransform(-56.3, -11.5);

        this.shape_336 = new cjs.Shape();
        this.shape_336.graphics.f("#919191").s().p("AgHAHIAAgWIAPAJIAAAWg");
        this.shape_336.setTransform(-57.8, -9.5);

        this.shape_337 = new cjs.Shape();
        this.shape_337.graphics.f("#C5C6C8").s().p("AgOgCIAdgRIAAAWIgdARg");
        this.shape_337.setTransform(-55.5, -9.9);

        this.instance_23 = new lib.Path_29();
        this.instance_23.parent = this;
        this.instance_23.setTransform(-57.8, -10.1, 1, 1, 0, 0, 0, 2.3, 1.3);

        this.shape_338 = new cjs.Shape();
        this.shape_338.graphics.f("#9FA7AB").s().p("Ah1g1IAAgTIDrCIIgQAJg");
        this.shape_338.setTransform(7.2, -38.9);

        this.shape_339 = new cjs.Shape();
        this.shape_339.graphics.f("#BDCACF").s().p("AlZC+IKzmPIAAAUIqzGPg");
        this.shape_339.setTransform(-11.3, -11.3);

        this.shape_340 = new cjs.Shape();
        this.shape_340.graphics.f("#BDCACF").s().p("Am8C4IKMl4IDtCJIgQAJIjdh/Ip8Fug");
        this.shape_340.setTransform(-25.5, -26.9);

        this.shape_341 = new cjs.Shape();
        this.shape_341.graphics.f("#929296").s().p("Am8B4IKMl4IDtCJIqMF4g");
        this.shape_341.setTransform(-25.5, -20.5);

        this.shape_342 = new cjs.Shape();
        this.shape_342.graphics.f("#DFE1EB").s().p("AnnB2IK0mPIEbCkIq0GPg");
        this.shape_342.setTransform(-25.5, -20.5);

        this.shape_343 = new cjs.Shape();
        this.shape_343.graphics.f("#9FA7AB").s().p("AiNhHIAAgUIEbCjIAAAUg");
        this.shape_343.setTransform(-60.1, 0.5);

        this.shape_344 = new cjs.Shape();
        this.shape_344.graphics.f("#6EB8CC").s().p("AlPBOIKfmCIAADnIqfGCg");
        this.shape_344.setTransform(-12, 1);

        this.shape_345 = new cjs.Shape();
        this.shape_345.graphics.f("#3A7293").s().p("AiGAmIAAjmIENCbIAADmg");
        this.shape_345.setTransform(-59, 12.6);

        this.shape_346 = new cjs.Shape();
        this.shape_346.graphics.f("#F2F3FA").s().p("AgaAPQgLgHAAgIQAAgIALgGQALgHAPAAQAQAAALAHQALAGAAAIQAAAJgLAGQgLAHgQAAQgPAAgLgHg");
        this.shape_346.setTransform(79.6, 0.3);

        this.shape_347 = new cjs.Shape();
        this.shape_347.graphics.f("#DFE1EB").s().p("AgjAVQgPgJAAgMQgBgMAPgIQAPgJAVAAQAVAAAPAJQAPAJABALQAAANgPAIQgPAJgWAAQgUAAgPgJg");
        this.shape_347.setTransform(79.6, 1.3);

        this.shape_348 = new cjs.Shape();
        this.shape_348.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.1, 0, 5.2, 0).s().p("AgMCAQgNgCgKgGQgRgKACgNIAAjhIBlAAIAADhQADAOgRAKQgQAIgUAAIgNgBg");
        this.shape_348.setTransform(79.6, 14.2);

        this.shape_349 = new cjs.Shape();
        this.shape_349.graphics.f("#ACACAC").s().p("ABGAMQgFgJgMgGIgDgBQgVgLgdAAQgfAAgVAMQgSAJgCAPIgBgDQAAgQAVgLQAVgNAfAAQAcAAAWAMIADACQAQAIAEANIABAHIAAACQgBgFgDgFg");
        this.shape_349.setTransform(79.7, 22.8);

        this.shape_350 = new cjs.Shape();
        this.shape_350.graphics.f("#565564").s().p("Ag2AUQgKgGgGgJIAGAEQAWANAfAAQAVAAATgHIALgFQASgKADgPQgBgEgDgGIADABQgBgEgDgEQALAJADAKIgDgBQAEAFAAAFQgCAOgSAKIgMAGQgSAHgVAAQgeAAgYgNg");
        this.shape_350.setTransform(80, 25.6);

        this.shape_351 = new cjs.Shape();
        this.shape_351.graphics.f("#565564").s().p("AgKAZIgfgSQADgNARgKQAVgMAeAAQAIAAACACQACABAAAHQAAAGgCAGIgCAHQgDAHgGAFQgIAJgHAEQgGADgFAAQgGAAgHgEg");
        this.shape_351.setTransform(76.6, 23.5);

        this.shape_352 = new cjs.Shape();
        this.shape_352.graphics.f("#7E7F82").s().p("AgzAdQgUgMgBgOIAAgBIgBgCQAAgQAVgMQAVgNAfAAQAcAAAWAMIADACQAQAJAEANIgDgBQADAFABAEQgDAQgSAKIgLAGQgSAGgXAAQgdAAgXgMg");
        this.shape_352.setTransform(79.7, 24.7);

        this.shape_353 = new cjs.Shape();
        this.shape_353.graphics.f("#F2F3FA").s().p("AgaAPQgLgGAAgJQAAgIALgGQALgHAPAAQAPABALAGQALAHABAIQAAAIgLAGQgLAHgQAAQgOAAgMgHg");
        this.shape_353.setTransform(69.2, -5.5);

        this.shape_354 = new cjs.Shape();
        this.shape_354.graphics.f("#DFE1EB").s().p("AgjAVQgQgJAAgMQAAgMAPgIQAPgJAVAAQAVABAPAIQAPAJABALQAAANgPAIQgPAJgWAAQgUAAgPgJg");
        this.shape_354.setTransform(69.2, -4.5);

        this.shape_355 = new cjs.Shape();
        this.shape_355.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.1, 0, 5.2, 0).s().p("AgLCAQgPgDgJgFQgQgKABgNIAAAAIAAjhIBlAAIAADhQADAOgRAJQgPAJgUAAIgNgBg");
        this.shape_355.setTransform(69.1, 8.4);

        this.shape_356 = new cjs.Shape();
        this.shape_356.graphics.f("#ACACAC").s().p("ABGAMQgGgJgMgFIgDgCQgVgMgcAAQgeAAgWANQgTAJgBAPIgBgDQAAgQAVgLQAVgMAfAAQAcAAAVALIADABQARAJAEANIABAGIAAAEQgBgFgDgGg");
        this.shape_356.setTransform(69.3, 17);

        this.shape_357 = new cjs.Shape();
        this.shape_357.graphics.f("#565564").s().p("Ag2AUQgLgGgFgIIAGADQAXAOAeAAQAVgBATgGIALgGQATgKABgPQAAgEgDgGIADABIgEgIQALAJADAKIgDgBQAEAGAAAFQgCAOgSAJIgMAGQgSAHgVAAQggAAgWgNg");
        this.shape_357.setTransform(69.6, 19.7);

        this.shape_358 = new cjs.Shape();
        this.shape_358.graphics.f("#565564").s().p("AgLAZIgegSQAEgNAQgKQAVgMAeAAQAJAAABACQACACAAAGIgCANIgCAGIgJAMQgIAKgHADQgGADgFAAQgGAAgIgEg");
        this.shape_358.setTransform(66.2, 17.6);

        this.shape_359 = new cjs.Shape();
        this.shape_359.graphics.f("#7E7F82").s().p("AgzAcQgUgLgBgPIgBgCQAAgQAVgMQAVgMAfAAQAcAAAVALIADABQARAKAEANIgDgBQADAGABAEQgDAPgSAKIgLAFQgTAHgWAAQgeAAgWgNg");
        this.shape_359.setTransform(69.3, 18.9);

        this.shape_360 = new cjs.Shape();
        this.shape_360.graphics.f("#F2F3FA").s().p("AABAWQgPAAgLgHQgMgGAAgJQAAgIALgHQALgGAPAAQAPAAAMAHQALAGAAAIQAAAJgLAHQgKAGgOAAIgCAAg");
        this.shape_360.setTransform(58.8, -11.4);

        this.shape_361 = new cjs.Shape();
        this.shape_361.graphics.f("#DFE1EB").s().p("AgjAVQgQgJAAgMQABgMAPgIQAPgJAUAAQAVAAAPAJQAPAJABALQgBANgOAIQgPAJgVAAQgVAAgPgJg");
        this.shape_361.setTransform(58.8, -10.3);

        this.shape_362 = new cjs.Shape();
        this.shape_362.graphics.lf(["#AAC3CF", "#CCD9E2", "#CCD9E2", "#DFEBF5", "#DFEBF5", "#FDFEFF"], [0.161, 0.165, 0.561, 0.561, 0.875, 0.875], -5.1, 0, 5.2, 0).s().p("AgMCAQgLgCgMgGQgQgKABgNIAAAAIAAjhIBlAAIAADhQADAOgRAJQgPAJgTAAIgPgBg");
        this.shape_362.setTransform(58.7, 2.6);

        this.shape_363 = new cjs.Shape();
        this.shape_363.graphics.f("#ACACAC").s().p("ABGANQgGgKgMgFIgDgCQgVgMgcAAQgeAAgWANQgSAKgDAOIAAgDQAAgQAVgLQAWgMAeAAQAcAAAVALIADABQARAJAEANIABAGIAAADIgBABQgBgHgCgDg");
        this.shape_363.setTransform(58.9, 11.2);

        this.shape_364 = new cjs.Shape();
        this.shape_364.graphics.f("#565564").s().p("Ag2AUQgLgGgFgJIAGAEQAWANAfAAQAWAAASgHIAMgFQASgKABgOQgBgGgCgFIADABIgEgIQAMAJACAKIgCgBQACAEABAGQgBAOgTALIgLAFQgSAHgWAAQggAAgWgNg");
        this.shape_364.setTransform(59.2, 13.9);

        this.shape_365 = new cjs.Shape();
        this.shape_365.graphics.f("#565564").s().p("AgLAZIgegSQACgNASgKQAVgMAeAAQAJAAABACQACACAAAGIgCAMIgCAHQgCAEgHAIQgKAKgFADQgGADgGAAQgGAAgHgEg");
        this.shape_365.setTransform(55.8, 11.8);

        this.shape_366 = new cjs.Shape();
        this.shape_366.graphics.f("#7E7F82").s().p("AgzAdQgTgMgDgPIAAgCQABgQAUgMQAWgMAeAAQAcAAAWALIACACQARAJAEANIgDAAQADAEABAFQgCAPgTAKIgLAGQgSAGgWAAQgfAAgWgMg");
        this.shape_366.setTransform(58.9, 13);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_366 }, { t: this.shape_365 }, { t: this.shape_364 }, { t: this.shape_363 }, { t: this.shape_362 }, { t: this.shape_361 }, { t: this.shape_360 }, { t: this.shape_359 }, { t: this.shape_358 }, { t: this.shape_357 }, { t: this.shape_356 }, { t: this.shape_355 }, { t: this.shape_354 }, { t: this.shape_353 }, { t: this.shape_352 }, { t: this.shape_351 }, { t: this.shape_350 }, { t: this.shape_349 }, { t: this.shape_348 }, { t: this.shape_347 }, { t: this.shape_346 }, { t: this.shape_345 }, { t: this.shape_344 }, { t: this.shape_343 }, { t: this.shape_342 }, { t: this.shape_341 }, { t: this.shape_340 }, { t: this.shape_339 }, { t: this.shape_338 }, { t: this.instance_23 }, { t: this.shape_337 }, { t: this.shape_336 }, { t: this.shape_335 }, { t: this.instance_22 }, { t: this.shape_334 }, { t: this.shape_333 }, { t: this.shape_332 }, { t: this.instance_21 }, { t: this.shape_331 }, { t: this.shape_330 }, { t: this.shape_329 }, { t: this.instance_20 }, { t: this.shape_328 }, { t: this.shape_327 }, { t: this.shape_326 }, { t: this.instance_19 }, { t: this.shape_325 }, { t: this.shape_324 }, { t: this.shape_323 }, { t: this.instance_18 }, { t: this.shape_322 }, { t: this.shape_321 }, { t: this.shape_320 }, { t: this.instance_17 }, { t: this.shape_319 }, { t: this.shape_318 }, { t: this.shape_317 }, { t: this.instance_16 }, { t: this.shape_316 }, { t: this.shape_315 }, { t: this.shape_314 }, { t: this.instance_15 }, { t: this.shape_313 }, { t: this.shape_312 }, { t: this.shape_311 }, { t: this.instance_14 }, { t: this.shape_310 }, { t: this.shape_309 }, { t: this.shape_308 }, { t: this.instance_13 }, { t: this.shape_307 }, { t: this.shape_306 }, { t: this.shape_305 }, { t: this.instance_12 }, { t: this.shape_304 }, { t: this.shape_303 }, { t: this.shape_302 }, { t: this.shape_301 }, { t: this.shape_300 }, { t: this.shape_299 }, { t: this.shape_298 }, { t: this.shape_297 }, { t: this.shape_296 }, { t: this.shape_295 }, { t: this.shape_294 }, { t: this.shape_293 }, { t: this.shape_292 }, { t: this.shape_291 }, { t: this.shape_290 }, { t: this.shape_289 }, { t: this.shape_288 }, { t: this.instance_11 }, { t: this.shape_287 }, { t: this.shape_286 }, { t: this.shape_285 }, { t: this.shape_284 }, { t: this.shape_283 }, { t: this.shape_282 }, { t: this.shape_281 }, { t: this.shape_280 }, { t: this.shape_279 }, { t: this.shape_278 }, { t: this.shape_277 }, { t: this.shape_276 }, { t: this.shape_275 }, { t: this.shape_274 }, { t: this.shape_273 }, { t: this.shape_272 }, { t: this.shape_271 }, { t: this.shape_270 }, { t: this.shape_269 }, { t: this.shape_268 }, { t: this.shape_267 }, { t: this.shape_266 }, { t: this.shape_265 }, { t: this.shape_264 }, { t: this.shape_263 }, { t: this.shape_262 }, { t: this.shape_261 }, { t: this.shape_260 }, { t: this.shape_259 }, { t: this.shape_258 }, { t: this.shape_257 }, { t: this.shape_256 }, { t: this.shape_255 }, { t: this.shape_254 }, { t: this.shape_253 }, { t: this.shape_252 }, { t: this.shape_251 }, { t: this.shape_250 }, { t: this.shape_249 }, { t: this.shape_248 }, { t: this.shape_247 }, { t: this.shape_246 }, { t: this.shape_245 }, { t: this.shape_244 }, { t: this.shape_243 }, { t: this.shape_242 }, { t: this.shape_241 }, { t: this.shape_240 }, { t: this.shape_239 }, { t: this.shape_238 }, { t: this.shape_237 }, { t: this.shape_236 }, { t: this.shape_235 }, { t: this.shape_234 }, { t: this.shape_233 }, { t: this.shape_232 }, { t: this.shape_231 }, { t: this.shape_230 }, { t: this.shape_229 }, { t: this.shape_228 }, { t: this.shape_227 }, { t: this.shape_226 }, { t: this.shape_225 }, { t: this.shape_224 }, { t: this.shape_223 }, { t: this.shape_222 }, { t: this.shape_221 }, { t: this.shape_220 }, { t: this.shape_219 }, { t: this.shape_218 }, { t: this.shape_217 }, { t: this.shape_216 }, { t: this.shape_215 }, { t: this.shape_214 }, { t: this.shape_213 }, { t: this.shape_212 }, { t: this.shape_211 }, { t: this.shape_210 }, { t: this.shape_209 }, { t: this.shape_208 }, { t: this.shape_207 }, { t: this.shape_206 }, { t: this.shape_205 }, { t: this.shape_204 }, { t: this.shape_203 }, { t: this.shape_202 }, { t: this.shape_201 }, { t: this.shape_200 }, { t: this.shape_199 }, { t: this.shape_198 }, { t: this.shape_197 }, { t: this.shape_196 }, { t: this.shape_195 }, { t: this.shape_194 }, { t: this.shape_193 }, { t: this.shape_192 }, { t: this.shape_191 }, { t: this.shape_190 }, { t: this.shape_189 }, { t: this.shape_188 }, { t: this.shape_187 }, { t: this.shape_186 }, { t: this.shape_185 }, { t: this.shape_184 }, { t: this.shape_183 }, { t: this.shape_182 }, { t: this.shape_181 }, { t: this.shape_180 }, { t: this.shape_179 }, { t: this.shape_178 }, { t: this.shape_177 }, { t: this.shape_176 }, { t: this.shape_175 }, { t: this.shape_174 }, { t: this.shape_173 }, { t: this.shape_172 }, { t: this.shape_171 }, { t: this.shape_170 }, { t: this.shape_169 }, { t: this.shape_168 }, { t: this.shape_167 }, { t: this.shape_166 }, { t: this.instance_10 }, { t: this.shape_165 }, { t: this.shape_164 }, { t: this.shape_163 }, { t: this.shape_162 }, { t: this.shape_161 }, { t: this.shape_160 }, { t: this.shape_159 }, { t: this.shape_158 }, { t: this.shape_157 }, { t: this.shape_156 }, { t: this.instance_9 }, { t: this.shape_155 }, { t: this.shape_154 }, { t: this.shape_153 }, { t: this.shape_152 }, { t: this.shape_151 }, { t: this.shape_150 }, { t: this.shape_149 }, { t: this.shape_148 }, { t: this.shape_147 }, { t: this.shape_146 }, { t: this.shape_145 }, { t: this.shape_144 }, { t: this.shape_143 }, { t: this.shape_142 }, { t: this.shape_141 }, { t: this.shape_140 }, { t: this.shape_139 }, { t: this.shape_138 }, { t: this.shape_137 }, { t: this.shape_136 }, { t: this.shape_135 }, { t: this.shape_134 }, { t: this.shape_133 }, { t: this.shape_132 }, { t: this.shape_131 }, { t: this.shape_130 }, { t: this.shape_129 }, { t: this.shape_128 }, { t: this.shape_127 }, { t: this.shape_126 }, { t: this.shape_125 }, { t: this.shape_124 }, { t: this.shape_123 }, { t: this.shape_122 }, { t: this.shape_121 }, { t: this.shape_120 }, { t: this.shape_119 }, { t: this.shape_118 }, { t: this.shape_117 }, { t: this.instance_8 }, { t: this.shape_116 }, { t: this.shape_115 }, { t: this.shape_114 }, { t: this.instance_7 }, { t: this.shape_113 }, { t: this.shape_112 }, { t: this.shape_111 }, { t: this.instance_6 }, { t: this.shape_110 }, { t: this.shape_109 }, { t: this.shape_108 }, { t: this.instance_5 }, { t: this.shape_107 }, { t: this.shape_106 }, { t: this.shape_105 }, { t: this.instance_4 }, { t: this.shape_104 }, { t: this.shape_103 }, { t: this.shape_102 }, { t: this.shape_101 }, { t: this.shape_100 }, { t: this.shape_99 }, { t: this.shape_98 }, { t: this.shape_97 }, { t: this.shape_96 }, { t: this.shape_95 }, { t: this.shape_94 }, { t: this.shape_93 }, { t: this.shape_92 }, { t: this.shape_91 }, { t: this.shape_90 }, { t: this.shape_89 }, { t: this.shape_88 }, { t: this.shape_87 }, { t: this.shape_86 }, { t: this.shape_85 }, { t: this.shape_84 }, { t: this.shape_83 }, { t: this.shape_82 }, { t: this.shape_81 }, { t: this.shape_80 }, { t: this.shape_79 }, { t: this.shape_78 }, { t: this.shape_77 }, { t: this.shape_76 }, { t: this.shape_75 }, { t: this.shape_74 }, { t: this.shape_73 }, { t: this.shape_72 }, { t: this.shape_71 }, { t: this.shape_70 }, { t: this.shape_69 }, { t: this.shape_68 }, { t: this.shape_67 }, { t: this.shape_66 }, { t: this.shape_65 }, { t: this.shape_64 }, { t: this.shape_63 }, { t: this.shape_62 }, { t: this.shape_61 }, { t: this.shape_60 }, { t: this.shape_59 }, { t: this.shape_58 }, { t: this.shape_57 }, { t: this.shape_56 }, { t: this.shape_55 }, { t: this.shape_54 }, { t: this.shape_53 }, { t: this.shape_52 }, { t: this.shape_51 }, { t: this.shape_50 }, { t: this.shape_49 }, { t: this.shape_48 }, { t: this.shape_47 }, { t: this.shape_46 }, { t: this.shape_45 }, { t: this.shape_44 }, { t: this.shape_43 }, { t: this.shape_42 }, { t: this.shape_41 }, { t: this.shape_40 }, { t: this.shape_39 }, { t: this.shape_38 }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.instance_3 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.instance_2 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.instance_1 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.instance }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-74.3, -95, 161.4, 190);


    (lib.ELEC = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.instance = new lib.ELEC2("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(-6.4, 0);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(6).to({ startPosition: 0 }, 0).wait(1).to({ regX: 6.4, scaleX: 1, scaleY: 1, x: 0, y: 0.2 }, 0).wait(1).to({ scaleX: 1.01, scaleY: 0.99, x: 0.1, y: 0.9 }, 0).wait(1).to({ scaleX: 1.03, scaleY: 0.96, x: 0.2, y: 3.7 }, 0).wait(1).to({ scaleX: 1.05, scaleY: 0.93, x: 0.3, y: 6.5 }, 0).wait(1).to({ scaleX: 1.05, scaleY: 0.93, x: 0.4, y: 7.2 }, 0).wait(1).to({ regX: 0, regY: 0.1, scaleX: 1.06, scaleY: 0.92, x: -6.4, y: 7.4 }, 0).wait(1).to({ regX: 6.4, regY: 0, scaleX: 1.05, scaleY: 0.93, x: 0.4, y: 6.7 }, 0).wait(1).to({ scaleX: 1.04, scaleY: 0.96, x: 0.3, y: 3.6 }, 0).wait(1).to({ scaleX: 0.99, scaleY: 1.06, x: -0.2, y: -6 }, 0).wait(1).to({ scaleX: 0.97, scaleY: 1.1, x: -0.3, y: -9.1 }, 0).wait(1).to({ regX: -0.1, regY: -0.1, scaleX: 0.97, scaleY: 1.1, x: -6.5, y: -9.5 }, 0).wait(1).to({ regX: 6.4, regY: 0, scaleX: 0.97, scaleY: 1.1, x: -0.2, y: -8.9 }, 0).wait(1).to({ scaleX: 0.98, scaleY: 1.05, x: 0, y: -4.7 }, 0).wait(1).to({ scaleX: 1, scaleY: 1.01, x: 0.1, y: -0.4 }, 0).wait(1).to({ regX: 0, scaleX: 1, scaleY: 1, x: -6.4, y: 0 }, 0).wait(8));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-80.7, -95, 161.4, 190);


    (lib.CARRIS = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#5F112E").s().p("AheAgQgGgEAAgGIgKguIADgBIAKAuQACALALAAIB1gDQAIAAACgIQAMgjApAAIABAAQAEABAEgEQACgDAAgEIABgKIAEAAIAAALQgBAFgEAEQgFAEgFAAQgogBgLAiQgCAKgKgBIh2ADQgGAAgEgDg");
        this.shape.setTransform(26.4, -0.9);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#F15522").s().p("AgHAGIAAgKIAOgBIABALg");
        this.shape_1.setTransform(39.8, -2);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#9A9A9B").s().p("AgIACIAIgEIAJACQgFAEgEAAIgIgCg");
        this.shape_2.setTransform(36.1, 4.4);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#9A9A9B").s().p("AgDAAIAGgJIABAHQAAAGgEAFg");
        this.shape_3.setTransform(37.4, 3.3);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#9A9A9B").s().p("AgDgFIAAgCIAHAGIgBAJQgFgFgBgIg");
        this.shape_4.setTransform(34.6, 3.6);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#9A9A9B").s().p("AgCAEIgFgIIAAAAQAKgBAFAKg");
        this.shape_5.setTransform(36.7, 1.7);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#9A9A9B").s().p("AAHgFIgEAJIgJACQAEgJAJgCg");
        this.shape_6.setTransform(34.9, 1.8);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#F4F3F4").s().p("AgFAHQgDgDAAgEQAAgHAIgBQAEAAACACQADADAAADQAAAJgIAAIgBAAQgCAAgDgCg");
        this.shape_7.setTransform(35.9, 3);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#DDDDDD").s().p("AgNAPQgGgGAAgIQgBgIAGgGQAGgGAIAAQAIAAAGAGQAGAFAAAIQAAAIgGAGQgGAGgIAAIAAAAQgHAAgGgFg");
        this.shape_8.setTransform(36, 3);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#9A9A9B").s().p("AgIACIAIgEIAJACQgEAEgFAAIgIgCg");
        this.shape_9.setTransform(11, 4.4);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#9A9A9B").s().p("AgDAAIAGgJIABAHQAAAGgEAFg");
        this.shape_10.setTransform(12.3, 3.3);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#9A9A9B").s().p("AgDgFIAAgCIAHAGIgBAJQgGgFAAgIg");
        this.shape_11.setTransform(9.5, 3.6);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#9A9A9B").s().p("AgCAEIgFgIIAAAAQAKgBAFAKg");
        this.shape_12.setTransform(11.6, 1.7);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#9A9A9B").s().p("AAHgFIgEAJIgJACQAEgJAJgCg");
        this.shape_13.setTransform(9.8, 1.8);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#F4F3F4").s().p("AgIAAQAAgDACgCQADgDADAAQAEAAACACQADADAAADQAAADgCADQgDADgEAAIAAAAQgHAAgBgJg");
        this.shape_14.setTransform(10.8, 3);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#DDDDDD").s().p("AgNAPQgGgGAAgJQgBgHAGgGQAGgGAIAAQAIAAAGAGQAGAFAAAIQAAAIgGAGQgGAGgIAAIAAAAQgIAAgFgFg");
        this.shape_15.setTransform(10.9, 3);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#323431").s().p("AgLACIAAgDIAXAAIAAADg");
        this.shape_16.setTransform(22.1, -3.5);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#323431").s().p("AgLADIAAgFIAXAAIAAAFg");
        this.shape_17.setTransform(34.7, -3.8);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.lf(["#D24047", "#CF4047", "#C53D48", "#B53948", "#9F3347", "#872B45"], [0.129, 0.467, 0.643, 0.784, 0.902, 1], 1, 0, -0.9, 0).s().p("AgIAYIAJgvIAJAAIgJAvg");
        this.shape_18.setTransform(25.6, -6.8);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#EAE9E9").s().p("AgLAHIAJgIQACgCADgBIAGgCQABAAAAAAQABAAAAAAQABABAAAAQAAABAAABQAAAEgDADQgEADgIAAg");
        this.shape_19.setTransform(6.9, -2.8);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#323431").s().p("AgRAKIAOgMIAIgFIAHgCQAGgBgBAFQAAAHgEADQgGAEgMABg");
        this.shape_20.setTransform(7.1, -2.8);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#5F595F").s().p("AhwAPQAlgSARgEQAZgGA6gBQA+gBALAIQALAGAEAKQgUgGgfgDQg7gEg+AJIgaAGQgUAEgGAAIgBAAg");
        this.shape_21.setTransform(25.6, -7.5);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#323431").s().p("AhiAGQAkgTATgEQAZgFA5gBQA+gBAMAHQALAHAFANQADAHAAAFIkIAKg");
        this.shape_22.setTransform(24.1, -6.6);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#C1BEC0").s().p("AgNAOQgGgFAAgJQAAgHAGgGQAFgGAIAAQAIAAAGAGQAGAGAAAHQAAAJgGAFQgGAGgIAAQgHAAgGgGg");
        this.shape_23.setTransform(36, 3);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#323431").s().p("AgWAXQgKgJAAgOQAAgNAKgKQAKgJAMAAQAOAAAJAJQAKAKAAANQAAAOgKAJQgJAKgOAAQgMAAgKgKg");
        this.shape_24.setTransform(36, 3);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#C1BEC0").s().p("AgNAOQgGgFAAgJQAAgHAGgGQAFgGAIAAQAIAAAGAGQAGAGAAAHQAAAJgGAFQgGAGgIAAQgHAAgGgGg");
        this.shape_25.setTransform(10.9, 3);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#323431").s().p("AgWAXQgKgJAAgOQAAgNAKgKQAJgJANAAQAOAAAJAJQAKAKAAANQAAAOgKAJQgJAKgOAAQgNAAgJgKg");
        this.shape_26.setTransform(10.9, 3);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#D24047").s().p("ACqBDQACgcgTgKIgTgFQgbAAgHAWQgEALACAKIioAAQAAgbgVgKIgVgGQgiAAgLAWIgDAVIgQAAQgEgLAAgNIABgLQAEgSA0gfQAsgaAegLQAdgKA/gCQBKgDAhAPQANAGAGAPQAEAIAEAUQAHAigFAmg");
        this.shape_27.setTransform(22.6, -3.7);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#1C1E1B").s().p("AguAgQgFgvA6gPQAbgHAJAiQAFARgBASg");
        this.shape_28.setTransform(35.4, -0.2);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#1C1E1B").s().p("Ag5AgQgGgvA7gPQAagHAVAiQALARAFASg");
        this.shape_29.setTransform(11.4, -0.2);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#1C1E1B").s().p("AisAIQgIAAABgGIACgFQAHgCCqgBQCogBAHABQAGACgCAFQgBAHgGAAg");
        this.shape_30.setTransform(22.7, 3.4);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.lf(["#D3C3C4", "#D3C3C4"], [0.012, 1], -1.1, 0, 1.2, 0).s().p("AgLAUIAQgnIAHAAIgMAng");
        this.shape_31.setTransform(-28.7, -3.9);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.lf(["#D3C3C4", "#D3C3C4"], [0.012, 1], -1.1, 0, 1.2, 0).s().p("AABAUIgLgnIAGAAIAPAng");
        this.shape_32.setTransform(-15.3, -3.9);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f("#EAE9E9").s().p("AgIgDIAJgBQAHgCAFAAQgBAHgJADQgHADgIAAg");
        this.shape_33.setTransform(-37.7, -0.1);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f("#252525").s().p("AgKgEIALgDQAJgCAGABQgBAJgLAEQgLAEgIAAg");
        this.shape_34.setTransform(-37.5, -0.1);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f("#F15522").s().p("AgGAFIAAgJIANAAIAAAJg");
        this.shape_35.setTransform(-7.5, -1);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f("#323431").s().p("AgIACIAAgDIARAAIAAADg");
        this.shape_36.setTransform(-16.9, -1.3);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#323431").s().p("AgIACIAAgDIARAAIAAADg");
        this.shape_37.setTransform(-24.3, -1.3);

        this.instance = new lib.Group_100();
        this.instance.parent = this;
        this.instance.setTransform(-16.4, 0.7, 1, 1, 0, 0, 0, 1.2, 2.3);

        this.instance_1 = new lib.Path_1();
        this.instance_1.parent = this;
        this.instance_1.setTransform(-21.6, 0.6, 1, 1, 0, 0, 0, 0.1, 2.3);

        this.instance_2 = new lib.Group_2_1();
        this.instance_2.parent = this;
        this.instance_2.setTransform(-22.6, 0.6, 1, 1, 0, 0, 0, 0.1, 2.3);

        this.instance_3 = new lib.Path_0();
        this.instance_3.parent = this;
        this.instance_3.setTransform(-31.1, -0.3, 1, 1, 0, 0, 0, 0.3, 1.6);

        this.instance_4 = new lib.Path();
        this.instance_4.parent = this;
        this.instance_4.setTransform(-17.6, -5, 1, 1, 0, 0, 0, 3.8, 0.7);
        this.instance_4.alpha = 0.52;

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f("#323431").s().p("AgxASIABgjIA8ABIAmAig");
        this.shape_38.setTransform(-16.5, -3.9);

        this.instance_5 = new lib.Path_2();
        this.instance_5.parent = this;
        this.instance_5.setTransform(-25.9, -5, 1, 1, 0, 0, 0, 3.6, 0.7);
        this.instance_5.alpha = 0.52;

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#323431").s().p("AguASIAZgYQALgLAQAAIApAAIgDAjg");
        this.shape_39.setTransform(-26.9, -3.9);

        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.f("#9A9A9B").s().p("AgGAAIAHgCIAFAEIgFABQgDgBgEgCg");
        this.shape_40.setTransform(-14.1, 5.1);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#9A9A9B").s().p("AgCgCIABgEIAEAGIgCAHQgDgEAAgFg");
        this.shape_41.setTransform(-15, 4.3);

        this.shape_42 = new cjs.Shape();
        this.shape_42.graphics.f("#9A9A9B").s().p("AgCAAIAFgFIAAACQAAAFgEAEg");
        this.shape_42.setTransform(-13, 4.5);

        this.shape_43 = new cjs.Shape();
        this.shape_43.graphics.f("#9A9A9B").s().p("AAGgCIgEAFIgHABQAEgHAHABg");
        this.shape_43.setTransform(-14.6, 3.1);

        this.shape_44 = new cjs.Shape();
        this.shape_44.graphics.f("#9A9A9B").s().p("AgBACIgDgGQAGACADAGg");
        this.shape_44.setTransform(-13.3, 3.2);

        this.shape_45 = new cjs.Shape();
        this.shape_45.graphics.f("#F4F3F4").s().p("AgFAAQgBgGAGABQAGgBAAAGQAAAGgGAAQgGAAABgGg");
        this.shape_45.setTransform(-14, 4.1);

        this.shape_46 = new cjs.Shape();
        this.shape_46.graphics.f("#DDDDDD").s().p("AgJAKQgFgEAAgGQABgFAEgEQAEgFAFAAQAGAAAFAFQAEAEAAAFQgBAGgEAFQgEAEgGAAQgFgBgEgEg");
        this.shape_46.setTransform(-14, 4);

        this.shape_47 = new cjs.Shape();
        this.shape_47.graphics.f("#6D6E6E").s().p("AgJAKQgFgEAAgGQAAgFAFgEQAEgFAFAAQAGAAAEAFQAFAEAAAFQAAAGgFAEQgEAFgGAAQgFAAgEgFg");
        this.shape_47.setTransform(-14, 4);

        this.shape_48 = new cjs.Shape();
        this.shape_48.graphics.f("#252525").s().p("AgQARQgHgHAAgKQAAgIAHgIQAHgHAJAAQAKAAAHAHQAHAHAAAJQAAAKgHAHQgHAHgKAAQgJAAgHgHg");
        this.shape_48.setTransform(-14, 4);

        this.shape_49 = new cjs.Shape();
        this.shape_49.graphics.f("#9A9A9B").s().p("AgGAAIAGgCIAHAEIgHABQgCgBgEgCg");
        this.shape_49.setTransform(-31.7, 5.1);

        this.shape_50 = new cjs.Shape();
        this.shape_50.graphics.f("#9A9A9B").s().p("AgCgCIABgEIAEAGIgCAHQgDgEAAgFg");
        this.shape_50.setTransform(-32.6, 4.3);

        this.shape_51 = new cjs.Shape();
        this.shape_51.graphics.f("#9A9A9B").s().p("AgCAAIAFgFIAAACQAAAFgEAEg");
        this.shape_51.setTransform(-30.6, 4.5);

        this.shape_52 = new cjs.Shape();
        this.shape_52.graphics.f("#9A9A9B").s().p("AAGgCIAAAAIgEAFIgHABQAEgHAHABg");
        this.shape_52.setTransform(-32.2, 3.1);

        this.shape_53 = new cjs.Shape();
        this.shape_53.graphics.f("#9A9A9B").s().p("AgBACIgDgGQAGACADAGg");
        this.shape_53.setTransform(-30.9, 3.2);

        this.shape_54 = new cjs.Shape();
        this.shape_54.graphics.f("#F4F3F4").s().p("AgGAAQABgGAFABQAHgBAAAGQAAAGgHAAQgFAAgBgGg");
        this.shape_54.setTransform(-31.6, 4.1);

        this.shape_55 = new cjs.Shape();
        this.shape_55.graphics.f("#DDDDDD").s().p("AgKAKQgEgEAAgGQAAgFAFgEQAEgFAFAAQAGAAAEAFQAFAEgBAFQAAAGgEAFQgEAEgGAAQgGgBgEgEg");
        this.shape_55.setTransform(-31.6, 4);

        this.shape_56 = new cjs.Shape();
        this.shape_56.graphics.f("#6D6E6E").s().p("AgJAKQgFgEABgGQgBgFAFgEQAEgFAFAAQAGAAAEAFQAFAEAAAFQAAAGgFAEQgEAFgGAAQgFAAgEgFg");
        this.shape_56.setTransform(-31.6, 4);

        this.shape_57 = new cjs.Shape();
        this.shape_57.graphics.f("#252525").s().p("AgQARQgHgHAAgKQAAgIAHgIQAHgHAJAAQAKAAAHAHQAHAHAAAJQAAAKgHAHQgHAHgKAAQgJAAgHgHg");
        this.shape_57.setTransform(-31.6, 4);

        this.shape_58 = new cjs.Shape();
        this.shape_58.graphics.f("#1C1E1B").s().p("AicAGIgDgCIgBgDIABgCIABgBQAFgBCZgBQCZgBAFABQAEABgBAEQgBAFgFAAg");
        this.shape_58.setTransform(-22.9, 3.8);

        this.shape_59 = new cjs.Shape();
        this.shape_59.graphics.f("#1B5F82").s().p("AB6A3QgCgagfAAQgdAAgBAaIhzAAQgBgbgVAAQgVAAgRAbIgkgCIgHgHQgEgGABgGQACgPAFgMQADgEAegHQAQgEAJgDQADgBArgmQADgDA8AAQA6gBAIABQAJABAsApIAdAFQAGAHgGA2g");
        this.shape_59.setTransform(-23, -1.3);

        this.shape_60 = new cjs.Shape();
        this.shape_60.graphics.f("#252525").s().p("Ah6AUIAAgnID1AFIAAAig");
        this.shape_60.setTransform(-22.4, 2.3);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_60 }, { t: this.shape_59 }, { t: this.shape_58 }, { t: this.shape_57 }, { t: this.shape_56 }, { t: this.shape_55 }, { t: this.shape_54 }, { t: this.shape_53 }, { t: this.shape_52 }, { t: this.shape_51 }, { t: this.shape_50 }, { t: this.shape_49 }, { t: this.shape_48 }, { t: this.shape_47 }, { t: this.shape_46 }, { t: this.shape_45 }, { t: this.shape_44 }, { t: this.shape_43 }, { t: this.shape_42 }, { t: this.shape_41 }, { t: this.shape_40 }, { t: this.shape_39 }, { t: this.instance_5 }, { t: this.shape_38 }, { t: this.instance_4 }, { t: this.instance_3 }, { t: this.instance_2 }, { t: this.instance_1 }, { t: this.instance }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-39.3, -10.5, 80, 16.9);


    (lib.AGUU1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.instance = new lib.agua3("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(6.3, 2.1, 1, 1, -23.3);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-25, -2.7, 53.5, 5.2);


    (lib.agua1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1 copia
        this.instance = new lib.AGU1("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(-40, 33.4);
        this.instance.alpha = 0;

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(1).to({ regX: 5.3, regY: 2.1, x: -31.8, y: 33.9, alpha: 0.333 }, 0).wait(1).to({ x: -28.8, y: 32.3, alpha: 0.667 }, 0).wait(1).to({ regX: 0, regY: 0, x: -31.2, y: 28.6, alpha: 1 }, 0).wait(1).to({ regX: 5.3, regY: 2.1, x: -22.9, y: 29.1 }, 0).wait(1).to({ x: -20, y: 27.5 }, 0).wait(1).to({ x: -17, y: 25.9 }, 0).wait(1).to({ x: -14.1, y: 24.3 }, 0).wait(1).to({ x: -11.1, y: 22.7 }, 0).wait(1).to({ x: -8.2, y: 21.1 }, 0).wait(1).to({ x: -5.2, y: 19.4 }, 0).wait(1).to({ x: -2.3, y: 17.8 }, 0).wait(1).to({ x: 0.7, y: 16.2 }, 0).wait(1).to({ x: 3.6, y: 14.6 }, 0).wait(1).to({ x: 6.5, y: 13 }, 0).wait(1).to({ regX: 0, regY: 0, x: 4.2, y: 9.3 }, 0).wait(1).to({ regX: 5.3, regY: 2.1, x: 12.4, y: 9.8, alpha: 0.75 }, 0).wait(1).to({ x: 15.4, y: 8.2, alpha: 0.5 }, 0).wait(1).to({ x: 18.3, y: 6.6, alpha: 0.25 }, 0).wait(1).to({ regX: 0, regY: 0, x: 16, y: 2.9, alpha: 0 }, 0).wait(3));

        // Capa_1
        this.instance_1 = new lib.AGU1("single", 0);
        this.instance_1.parent = this;
        this.instance_1.setTransform(-35.8, 16.9);
        this.instance_1.alpha = 0;

        this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(1).to({ regX: 5.3, regY: 2.1, x: -28.2, y: 17.7, alpha: 0.333 }, 0).wait(1).to({ x: -25.8, y: 16.4, alpha: 0.667 }, 0).wait(1).to({ regX: 0, regY: 0, x: -28.8, y: 13.1, alpha: 1 }, 0).wait(1).to({ regX: 5.3, regY: 2.1, x: -21.2, y: 13.9 }, 0).wait(1).to({ x: -18.8, y: 12.6 }, 0).wait(1).to({ x: -16.5, y: 11.3 }, 0).wait(1).to({ x: -14.2, y: 10.1 }, 0).wait(1).to({ x: -11.9, y: 8.8 }, 0).wait(1).to({ x: -9.5, y: 7.5 }, 0).wait(1).to({ x: -7.2, y: 6.2 }, 0).wait(1).to({ x: -4.9, y: 5 }, 0).wait(1).to({ x: -2.5, y: 3.7 }, 0).wait(1).to({ x: -0.2, y: 2.4 }, 0).wait(1).to({ x: 2.1, y: 1.2 }, 0).wait(1).to({ regX: 0, regY: 0, x: -0.9, y: -2.2 }, 0).wait(1).to({ regX: 5.3, regY: 2.1, x: 6.7, y: -1.4, alpha: 0.75 }, 0).wait(1).to({ x: 9.1, y: -2.7, alpha: 0.5 }, 0).wait(1).to({ x: 11.4, y: -3.9, alpha: 0.25 }, 0).wait(1).to({ regX: 0, regY: 0, x: 8.4, y: -7.3, alpha: 0 }, 0).wait(3));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-120, -20.7, 174.7, 96);


    (lib.MASAGUA = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // AGUU2
        this.instance = new lib.AGUU2("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(1.1, 0.6, 1, 1, -28.7);
        this.instance.alpha = 0;

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(1).to({ regX: -0.8, regY: 0.2, x: 0.5, y: 1.2, alpha: 0.231 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 1, scaleY: 1, rotation: -28.6, x: 1.3, y: 0.7, alpha: 0.988 }, 0).wait(1).to({ regX: -0.8, regY: 0.2, x: 0.8, y: 1.5, alpha: 0.974 }, 0).wait(1).to({ x: 1.1, y: 1.8, alpha: 0.95 }, 0).wait(1).to({ x: 1.4, y: 2.3, alpha: 0.916 }, 0).wait(1).to({ x: 1.9, y: 3, alpha: 0.867 }, 0).wait(1).to({ x: 2.7, y: 4, alpha: 0.797 }, 0).wait(1).to({ x: 3.7, y: 5.3, alpha: 0.7 }, 0).wait(1).to({ x: 5.1, y: 7.1, alpha: 0.571 }, 0).wait(1).to({ x: 6.6, y: 9.1, alpha: 0.427 }, 0).wait(1).to({ scaleX: 1, scaleY: 1, x: 8, y: 10.9, alpha: 0.299 }, 0).wait(1).to({ rotation: -28.7, x: 9, y: 12.3, alpha: 0.202 }, 0).wait(1).to({ x: 9.8, y: 13.3, alpha: 0.132 }, 0).wait(1).to({ x: 10.3, y: 14, alpha: 0.083 }, 0).wait(1).to({ x: 10.6, y: 14.4, alpha: 0.048 }, 0).wait(1).to({ x: 10.9, y: 14.8, alpha: 0.025 }, 0).wait(1).to({ x: 11, y: 15, alpha: 0.01 }, 0).wait(1).to({ x: 11.1, y: 15.1, alpha: 0.002 }, 0).wait(1).to({ regX: 0, regY: 0, x: 11.8, y: 14.5, alpha: 0 }, 0).wait(6));

        // AGUU1
        this.instance_1 = new lib.AGUU1("single", 0);
        this.instance_1.parent = this;
        this.instance_1.setTransform(-5.6, -3.3, 1, 1, -24.7);
        this.instance_1.alpha = 0;

        this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(1).to({ regX: 1.4, regY: -0.5, x: -4.5, y: -4.3, alpha: 0.061 }, 0).wait(1).to({ x: -4.4, y: -4.2, alpha: 0.245 }, 0).wait(1).to({ scaleX: 1, scaleY: 1, rotation: -24.6, x: -4.2, y: -4.1, alpha: 0.554 }, 0).wait(1).to({ regX: 0, regY: 0, x: -5.1, y: -2.9, alpha: 0.988 }, 0).wait(1).to({ regX: 1.4, regY: -0.5, x: -3.7, y: -3.7, alpha: 0.974 }, 0).wait(1).to({ x: -3.4, y: -3.4, alpha: 0.955 }, 0).wait(1).to({ x: -3, y: -3, alpha: 0.934 }, 0).wait(1).to({ x: -2.5, y: -2.6, alpha: 0.909 }, 0).wait(1).to({ x: -2, y: -2.2, alpha: 0.881 }, 0).wait(1).to({ x: -1.4, y: -1.7, alpha: 0.849 }, 0).wait(1).to({ x: -0.7, y: -1.1, alpha: 0.813 }, 0).wait(1).to({ x: 0, y: -0.5, alpha: 0.774 }, 0).wait(1).to({ x: 0.9, y: 0.2, alpha: 0.731 }, 0).wait(1).to({ x: 1.7, y: 0.9, alpha: 0.684 }, 0).wait(1).to({ x: 2.6, y: 1.7, alpha: 0.633 }, 0).wait(1).to({ x: 3.7, y: 2.6, alpha: 0.579 }, 0).wait(1).to({ x: 4.8, y: 3.5, alpha: 0.521 }, 0).wait(1).to({ x: 5.9, y: 4.5, alpha: 0.459 }, 0).wait(1).to({ x: 7.2, y: 5.6, alpha: 0.393 }, 0).wait(1).to({ scaleX: 1, scaleY: 1, x: 8.5, y: 6.7, alpha: 0.322 }, 0).wait(1).to({ rotation: -24.7, x: 9.9, y: 7.9, alpha: 0.248 }, 0).wait(1).to({ x: 11.4, y: 9.1, alpha: 0.17 }, 0).wait(1).to({ x: 13, y: 10.4, alpha: 0.087 }, 0).wait(1).to({ regX: 0, regY: 0, x: 13.5, y: 12.9, alpha: 0 }, 0).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-27.7, -16.4, 47.5, 25.2);


    (lib.FABRIC = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.instance = new lib.FAB1("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(-8.3, 0);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(4).to({ startPosition: 0 }, 0).wait(1).to({ regX: 2.4, regY: -19.8, scaleX: 1, scaleY: 1, x: -5.9, y: -19.7 }, 0).wait(1).to({ scaleX: 1.01, scaleY: 0.99, y: -19.2 }, 0).wait(1).to({ scaleX: 1.03, scaleY: 0.96, x: -5.8, y: -17.4 }, 0).wait(1).to({ scaleX: 1.06, scaleY: 0.92, x: -5.7, y: -15.6 }, 0).wait(1).to({ scaleX: 1.06, scaleY: 0.91, x: -5.8, y: -15.1 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 1.06, scaleY: 0.91, x: -8.3, y: 3.1 }, 0).wait(1).to({ regX: 2.4, regY: -19.8, scaleX: 1.06, scaleY: 0.92, x: -5.7, y: -15.4 }, 0).wait(1).to({ scaleX: 1.03, scaleY: 0.97, x: -5.8, y: -17.9 }, 0).wait(1).to({ scaleX: 0.96, scaleY: 1.11, x: -6, y: -25.5 }, 0).wait(1).to({ scaleX: 0.94, scaleY: 1.16, x: -6.1, y: -28.1 }, 0).wait(1).to({ regX: -0.1, regY: 0, scaleX: 0.93, scaleY: 1.17, x: -8.4, y: -5.5 }, 0).wait(1).to({ regX: 2.4, regY: -19.8, scaleX: 0.93, scaleY: 1.16, x: -6, y: -28.2 }, 0).wait(1).to({ scaleX: 0.95, scaleY: 1.13, y: -26.6 }, 0).wait(1).to({ scaleX: 0.99, scaleY: 1.04, x: -5.8, y: -21.6 }, 0).wait(1).to({ scaleX: 1, scaleY: 1.01, x: -5.7, y: -20 }, 0).wait(1).to({ regX: 0, regY: 0, scaleX: 1, scaleY: 1, x: -8.3, y: 0 }, 0).wait(5));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-99.3, -73.1, 186.9, 106.7);


    (lib.carros = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // Capa_1
        this.instance = new lib.CARRIS("single", 0);
        this.instance.parent = this;
        this.instance.setTransform(30.9, -3.5);
        this.instance.alpha = 0.012;
        this.instance._off = true;

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(3).to({ _off: false }, 0).wait(1).to({ regX: 0.7, regY: -2, x: 28.7, y: -5.5, alpha: 0.072 }, 0).wait(1).to({ x: 25.9, alpha: 0.131 }, 0).wait(1).to({ x: 23.1, alpha: 0.189 }, 0).wait(1).to({ x: 20.5, alpha: 0.244 }, 0).wait(1).to({ x: 18, alpha: 0.296 }, 0).wait(1).to({ x: 15.7, alpha: 0.345 }, 0).wait(1).to({ x: 13.6, alpha: 0.39 }, 0).wait(1).to({ x: 11.6, alpha: 0.432 }, 0).wait(1).to({ x: 9.7, alpha: 0.471 }, 0).wait(1).to({ x: 8, alpha: 0.507 }, 0).wait(1).to({ x: 6.5, alpha: 0.539 }, 0).wait(1).to({ x: 5, alpha: 0.57 }, 0).wait(1).to({ x: 3.7, alpha: 0.598 }, 0).wait(1).to({ x: 2.5, alpha: 0.624 }, 0).wait(1).to({ x: 1.3, alpha: 0.648 }, 0).wait(1).to({ x: 0.3, alpha: 0.671 }, 0).wait(1).to({ x: -0.7, alpha: 0.692 }, 0).wait(1).to({ x: -1.7, alpha: 0.712 }, 0).wait(1).to({ x: -2.6, alpha: 0.731 }, 0).wait(1).to({ x: -3.4, alpha: 0.749 }, 0).wait(1).to({ x: -4.2, alpha: 0.766 }, 0).wait(1).to({ x: -5, alpha: 0.782 }, 0).wait(1).to({ x: -5.7, alpha: 0.798 }, 0).wait(1).to({ x: -6.4, alpha: 0.812 }, 0).wait(1).to({ x: -7.1, alpha: 0.827 }, 0).wait(1).to({ x: -7.8, alpha: 0.84 }, 0).wait(1).to({ x: -8.4, alpha: 0.853 }, 0).wait(1).to({ x: -9, alpha: 0.866 }, 0).wait(1).to({ x: -9.6, alpha: 0.878 }, 0).wait(1).to({ x: -10.1, alpha: 0.89 }, 0).wait(1).to({ x: -10.7, alpha: 0.901 }, 0).wait(1).to({ x: -11.2, alpha: 0.912 }, 0).wait(1).to({ x: -11.7, alpha: 0.923 }, 0).wait(1).to({ x: -12.2, alpha: 0.934 }, 0).wait(1).to({ x: -12.7, alpha: 0.944 }, 0).wait(1).to({ x: -13.2, alpha: 0.954 }, 0).wait(1).to({ x: -13.6, alpha: 0.963 }, 0).wait(1).to({ x: -14.1, alpha: 0.973 }, 0).wait(1).to({ x: -14.5, alpha: 0.982 }, 0).wait(1).to({ x: -14.9, alpha: 0.991 }, 0).wait(1).to({ regX: 0, regY: 0, x: -16.1, y: -3.5, alpha: 1 }, 0).wait(1));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = null;


    (lib.ESCENA1 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // LOGOCOES2
        this.instance = new lib.LOGOCOES2("synched", 0);
        this.instance.parent = this;
        this.instance.setTransform(112, -142.3);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(80));

        // carros
        this.instance_1 = new lib.carros("synched", 0);
        this.instance_1.parent = this;
        this.instance_1.setTransform(178, -99.6, 1, 1, 0, 0, 0, -248.3, 0);

        this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(80));

        // cables
        this.instance_2 = new lib.cables("single", 0);
        this.instance_2.parent = this;
        this.instance_2.setTransform(93.4, 110.5);

        this.timeline.addTween(cjs.Tween.get(this.instance_2).wait(80));

        // Capa_7
        this.shape = new cjs.Shape();
        this.shape.graphics.f("#0077A5").s().p("AhGClQgagKgVgTIATgtQAsAjAnAAQAjAAAUggQATghABg8QgJAVgXANQgTANgcABQgcgBgXgOQgVgNgOgYQgNgaAAgfQAAghAPgaQANgaAZgOQAZgPAfAAQA8AAAiAsQAiAsgBBRQAAA3gPApQgQAogeAXQgeAVgoAAQgcAAgdgKgAguhsQgPASgBAcQABAdAPAQQAPARAZAAQAZAAAQgRQAQgRAAgcQAAgcgQgSQgRgRgYAAQgYAAgQARg");
        this.shape.setTransform(275.9, -323);

        this.shape_1 = new cjs.Shape();
        this.shape_1.graphics.f("#0077A5").s().p("AhjCrIAAgzIBHAAIAAjbIhCAqIAAg3IBag6IAhAAIAAEiIBHAAIAAAzg");
        this.shape_1.setTransform(250.1, -323);

        this.shape_2 = new cjs.Shape();
        this.shape_2.graphics.f("#0077A5").s().p("AhXCCQgegsAAhWQAAhVAegsQAegsA5AAQA6AAAeAsQAeAsAABVQAABVgeAsQgeAug6gBQg5AAgegsgAgthfQgOAfAABAQAABBAOAfQAPAeAeAAQAgAAAOgeQAOgeAAhCQAAhBgOgeQgOgeggAAQgeAAgPAeg");
        this.shape_2.setTransform(221.4, -323);

        this.shape_3 = new cjs.Shape();
        this.shape_3.graphics.f("#0077A5").s().p("AhnCtIAAguIBth9QAUgWAJgSQAJgSAAgTQAAgYgNgLQgNgMgXAAQgqgBguAkIgTgtQATgSAegLQAfgLAdAAQAwAAAcAaQAdAZAAAqQAAAegMAaQgMAYgdAgIhOBaICOAAIAAAyg");
        this.shape_3.setTransform(194, -323.2);

        this.shape_4 = new cjs.Shape();
        this.shape_4.graphics.f("#0077A5").s().p("AA0B8IAAiRQgBgdgJgNQgLgNgWAAQgYAAgRASQgPARAAAdIAACIIg5AAIAAisQAAghgDgkIA1AAIADAoQANgXAUgLQATgMAaAAQBRAAAABjIAACUg");
        this.shape_4.setTransform(154.2, -318.4);

        this.shape_5 = new cjs.Shape();
        this.shape_5.graphics.f("#0077A5").s().p("Ag8CtQgbgQgOgcQgPgdAAgmQAAgmAPgcQAPgdAagPQAagPAiAAQAjAAAaAPQAbAPAOAdQAOAcAAAmQAAAmgOAdQgOAcgbAQQgaAPgjAAQghAAgbgPgAgqADQgQAUAAAnQAAAnAPAUQAPAUAcAAQAdAAAPgUQAPgUAAgnQAAgngPgUQgQgUgcAAQgbAAgPAUgAgRhYIAshjIA7AAIhEBjg");
        this.shape_5.setTransform(127.1, -324.4);

        this.shape_6 = new cjs.Shape();
        this.shape_6.graphics.f("#0077A5").s().p("AgbCwIAAjxIA3AAIAADxgAgeh0IAAg7IA+AAIAAA7g");
        this.shape_6.setTransform(107.7, -323.5);

        this.shape_7 = new cjs.Shape();
        this.shape_7.graphics.f("#0077A5").s().p("AguBvQgagQgPgbQgOgdABglQAAgmAOgcQAPgcAbgRQAcgQAhAAQAZAAAXAIQAYAIAMAPIgPAqQgPgNgRgGQgPgHgQABQgfAAgRAUQgRAUAAAlQAAAnARAUQARAUAfAAQAQAAAPgHQARgGAPgNIAPAqQgNAOgYAJQgXAIgaAAQgiAAgbgPg");
        this.shape_7.setTransform(90.9, -318.2);

        this.shape_8 = new cjs.Shape();
        this.shape_8.graphics.f("#0077A5").s().p("AhDBvQgXgQgMgcQgNgbAAgmQAAgkANgeQANgcAXgRQAXgQAeAAQAXAAAVAMQATALAKATIAAgkIA4AAIAADxIg4AAIAAgmQgKAUgTALQgUALgYAAQgeAAgYgPgAgqg5QgQAWAAAlQAAAkAQAVQAPAUAcAAQAcAAAQgUQAPgWAAglQAAgkgPgVQgQgVgcAAQgcAAgPAVg");
        this.shape_8.setTransform(64.2, -318.2);

        this.shape_9 = new cjs.Shape();
        this.shape_9.graphics.f("#0077A5").s().p("AhHB8IAAitQABgogEgcIA1AAIAEAqQAJgWASgOQATgMAXAAQAMAAALAEIgBA2QgPgGgPAAQgcAAgQASQgOASAAAdIAACCg");
        this.shape_9.setTransform(43.7, -318.3);

        this.shape_10 = new cjs.Shape();
        this.shape_10.graphics.f("#0077A5").s().p("AhKBdQgggiAAg7QAAgjAOgdQANgbAbgSQAagQAeAAQAvAAAcAhQAcAgAAA5IAAAIIidAAQACAnAQARQARATAfAAQAnAAAfgaIARApQgRAOgYAJQgXAIgZAAQg4AAggghgAgehDQgPANgEAbIBrAAQgDgcgOgNQgNgOgWAAQgVAAgPAPg");
        this.shape_10.setTransform(21, -318.2);

        this.shape_11 = new cjs.Shape();
        this.shape_11.graphics.f("#0077A5").s().p("AhyCwIAAkUQABgogEgdIA1AAIAEAqQALgYATgLQAVgNAaAAQAcAAAYAQQAXARANAcQANAeAAAlQAAAkgNAcQgNAcgWAQQgYAPgdAAQgZAAgTgLQgTgLgLgUIAACOgAgqhrQgPAUgBAmQAAAnAQATQAPAUAcAAQAdAAAPgUQAQgTAAglQAAgngQgVQgPgVgdAAQgbAAgQAVg");
        this.shape_11.setTransform(-5.1, -313.2);

        this.shape_12 = new cjs.Shape();
        this.shape_12.graphics.f("#0077A5").s().p("AhQCaQgjgWgTgnQgTgmAAg3QAAg1ATgnQASgnAjgWQAigVAvAAQAwAAAiAVQAjAVATAoQASAoAAA0QAAA2gTAnQgSAogjAVQgiAVgwAAQguAAgigVgAhFhdQgZAhAAA8QAAA9AZAgQAZAhAsAAQAuAAAYghQAZggAAg9QAAg8gZggQgZghgtABQgsAAgZAfg");
        this.shape_12.setTransform(-37.6, -323);

        this.shape_13 = new cjs.Shape();
        this.shape_13.graphics.f("#0077A5").s().p("AhKBdQgggiAAg7QAAgkAOgcQAOgcAagRQAagQAfAAQAuAAAcAhQAcAfAAA6IAAAIIidAAQABAnARARQARATAfAAQAmAAAggaIARApQgRAOgXAJQgYAIgZAAQg4AAggghgAgehDQgOANgFAbIBqAAQgDgbgMgOQgOgOgWAAQgWAAgOAPg");
        this.shape_13.setTransform(-80.3, -318.2);

        this.shape_14 = new cjs.Shape();
        this.shape_14.graphics.f("#0077A5").s().p("AhCCjQgXgQgNgdQgNgeAAglQAAgkANgcQAMgcAXgQQAXgQAfAAQAYAAATALQATALAKATIAAiRIA5AAIAAFgIg4AAIAAgnQgKAUgUALQgTALgYAAQgeAAgXgPgAgqgFQgQATAAAlQAAAnAQAUQAQAVAbAAQAcAAAQgUQAPgVAAgmQAAgmgPgTQgQgVgcAAQgbAAgQAVg");
        this.shape_14.setTransform(-107.5, -323.4);

        this.shape_15 = new cjs.Shape();
        this.shape_15.graphics.f("#0077A5").s().p("AhCBvQgYgQgMgcQgNgdAAgkQAAgkANgeQAMgbAYgSQAXgQAeAAQAXAAAUAMQAUALAKATIAAgkIA4AAIAADxIg4AAIAAgmQgKAUgUALQgTALgYAAQgfAAgWgPgAgqg5QgQAVAAAmQAAAlAQAUQAPAUAcAAQAdAAAPgUQAPgVAAgmQAAglgPgUQgPgVgdAAQgcAAgPAVg");
        this.shape_15.setTransform(-148.1, -318.2);

        this.shape_16 = new cjs.Shape();
        this.shape_16.graphics.f("#0077A5").s().p("AguBvQgagQgOgbQgPgdAAglQAAglAPgdQAOgcAcgRQAbgQAjAAQAZAAAWAIQAXAIAOAPIgQAqQgQgOgPgFQgQgHgRABQgdAAgRAUQgSAVAAAkQAAAmARAVQARAUAeAAQARAAAQgHQAPgGAQgNIAQAqQgOAPgYAIQgWAIgbAAQgiAAgbgPg");
        this.shape_16.setTransform(-172.4, -318.2);

        this.shape_17 = new cjs.Shape();
        this.shape_17.graphics.f("#0077A5").s().p("AgbCwIAAjxIA3AAIAADxgAgeh0IAAg7IA+AAIAAA7g");
        this.shape_17.setTransform(-190.3, -323.5);

        this.shape_18 = new cjs.Shape();
        this.shape_18.graphics.f("#0077A5").s().p("AgPCLQgUgWAAgsIAAhuIgtAAIAAgtIAtAAIAAg7IA3gUIAABPIA9AAIAAAtIg9AAIAABrQAAArAmAAQAJAAAOgEIAAAwQgOAFgXAAQgnAAgUgXg");
        this.shape_18.setTransform(-205.1, -321.9);

        this.shape_19 = new cjs.Shape();
        this.shape_19.graphics.f("#0077A5").s().p("AhgBfIAQgqQAlAdAuAAQAUAAALgHQAMgHAAgOQAAgKgIgHQgJgHgRgEIglgJQg/gOAAg0QAAgXAMgRQAMgRAVgKQAUgJAcAAQAZAAAWAIQAWAHATAQIgRAoQgjgbgkAAQgTABgLAHQgLAIAAAOQAAAKAHAGQAHAHAQAEIAlAJQAiAIARAQQAPASAAAaQAAAigaATQgZAUgsAAQg9AAglgfg");
        this.shape_19.setTransform(-224.7, -318.2);

        this.shape_20 = new cjs.Shape();
        this.shape_20.graphics.f("#0077A5").s().p("Ag4C6IAAjxIA4AAIAADxgAguhXIAthiIA6AAIhCBig");
        this.shape_20.setTransform(-239.1, -324.5);

        this.shape_21 = new cjs.Shape();
        this.shape_21.graphics.f("#0077A5").s().p("AhCCjQgYgRgMgcQgNgeAAglQAAgkANgcQAMgcAYgQQAWgQAfAAQAYAAATALQATALAKATIAAiRIA5AAIAAFgIg4AAIAAgnQgKAUgUALQgTALgYAAQgeAAgXgPgAgqgFQgQATAAAlQAAAnAQAUQAQAVAbAAQAcAAAQgUQAPgUAAgnQAAgmgPgTQgQgVgcAAQgbAAgQAVg");
        this.shape_21.setTransform(-262.6, -323.4);

        this.shape_22 = new cjs.Shape();
        this.shape_22.graphics.f("#0077A5").s().p("AhCBvQgYgQgMgcQgNgdAAgkQAAgkANgeQANgcAXgRQAYgQAdAAQAXAAAUAMQAUALAKATIAAgkIA4AAIAADxIg4AAIAAgmQgKAUgUALQgTALgYAAQgeAAgXgPgAgqg5QgQAVAAAmQAAAlAQAUQAPAUAcAAQAdAAAPgUQAPgVAAgmQAAglgPgUQgPgVgdAAQgcAAgPAVg");
        this.shape_22.setTransform(-290.7, -318.2);

        this.shape_23 = new cjs.Shape();
        this.shape_23.graphics.f("#0077A5").s().p("AgPCLQgVgWAAgsIAAhuIgsAAIAAgtIAsAAIAAg7IA4gUIAABPIA9AAIAAAtIg9AAIAABrQAAArAlAAQAKAAAOgEIAAAwQgOAFgXAAQgnAAgUgXg");
        this.shape_23.setTransform(-313, -321.9);

        this.shape_24 = new cjs.Shape();
        this.shape_24.graphics.f("#0077A5").s().p("AhgBfIAQgqQAlAdAuAAQAUAAAMgHQALgIAAgNQAAgKgIgHQgJgHgRgEIgmgJQg+gOAAg0QAAgXAMgRQALgRAWgKQAUgJAcAAQAZAAAXAIQAVAHATAQIgRAoQgjgbgkAAQgTABgLAHQgLAJAAANQAAAKAHAGQAGAHARAEIAmAJQAhAIAQAQQAQARAAAbQAAAigZATQgaAUgsAAQg9AAglgfg");
        this.shape_24.setTransform(-332.6, -318.2);

        this.shape_25 = new cjs.Shape();
        this.shape_25.graphics.f("#0077A5").s().p("AhrCrIAAlVIDXAAIAAAxIieAAIAABfICUAAIAAAvIiUAAIAABlICeAAIAAAxg");
        this.shape_25.setTransform(-356.6, -323);

        this.shape_26 = new cjs.Shape();
        this.shape_26.graphics.f("#1E2431").s().p("AhDClQgbgJghgcIA2g2QAUATAKAIQAUAMAVAAQARAAANgLQAOgLABgRQgBgKgGgIQgJgOgngOQgkgQgTgMQgqgdAAguQAAgcATgaQAkgtBEAAQAhAAAeANQAYAKAXAWIg1A0IgSgSQgRgNgVAAQgPgBgLAJQgMAJAAANQAAARAVAMQAGAGAaAKQAaAKANAHQALAFAMAIQAlAbgBArQABAuglAeQgQAMgLAGQgeAOgmgBQgkAAgcgJg");
        this.shape_26.setTransform(495.9, -306.6);

        this.shape_27 = new cjs.Shape();
        this.shape_27.graphics.f("#1E2431").s().p("AhrCmIAAlLIDXAAIAABAIiFAAIAABCICBAAIAABAIiBAAIAABHICEAAIAABCg");
        this.shape_27.setTransform(470, -306.6);

        this.shape_28 = new cjs.Shape();
        this.shape_28.graphics.f("#1E2431").s().p("Ah9CNQgdgTgTgbQgcgnAAgzQAAgoAQgiQAXgxAzgcQAxgcA9AAQAZAAAYAFQBBANArAsQAuAwAABCQAAAngQAhQgYAygzAaQgvAZhAAAQhJAAg0gigAABhoQgrAAgfAWQgRAMgLAQQgSAZAAAeQAAAXAKAUQAOAeAgARQAeAQAmAAQAqAAAdgVQAVgPALgXQALgWAAgZQAAgogcgeQgkgjg1AAIgBAAg");
        this.shape_28.setTransform(435, -306.6);

        this.shape_29 = new cjs.Shape();
        this.shape_29.graphics.f("#1E2431").s().p("AhPCKQgZgTgSgcQgZgpAAgxQAAgyAXgoQANgUAOgNQAcgbAigMQAigMAlAAQAqAAAoAMIAeALIAABSQgwgmg1AAQgfAAgWALQgXAMgPAUQgUAcAAAlQAAAWAIAUQAJAYAUAQQAcAYAwAAQAOAAAOgCQAagEAbgQIASgMIAABOIgpANQgiAJgiAAQhHAAgvgkg");
        this.shape_29.setTransform(397.6, -306.6);

        this.shape_30 = new cjs.Shape();
        this.shape_30.graphics.f("#010202").s().p("AgRAFQgMgUgHgUQAQAXAyAnIAHAIQgXgKgfgUg");
        this.shape_30.setTransform(355.8, -314.1);

        this.shape_31 = new cjs.Shape();
        this.shape_31.graphics.f("#010202").s().p("AgfAHIAAgeQAeANAiAHIAAAbQghgFgfgMg");
        this.shape_31.setTransform(373.1, -300.5);

        this.shape_32 = new cjs.Shape();
        this.shape_32.graphics.f("#010202").s().p("AgTgjIAbAAQADAiAJAeIgOAAIgFABQgGABgCAEQgJghgDglg");
        this.shape_32.setTransform(352, -323.7);

        this.shape_33 = new cjs.Shape();
        this.shape_33.graphics.f("#010202").s().p("AgfALIAAgkQAfAMAhAFIAAAiQghgEgfgLg");
        this.shape_33.setTransform(373.1, -295.6);

        this.shape_34 = new cjs.Shape();
        this.shape_34.graphics.f("#010202").s().p("AgXAIQgGgHgBgQQAeAIAfAEQgCALgIAEQgGAEgOAAQgQAAgIgIg");
        this.shape_34.setTransform(373, -285.3);

        this.shape_35 = new cjs.Shape();
        this.shape_35.graphics.f("#010202").s().p("AgfADIAAgeQAdAUAjAHIAAAcQgigIgegRg");
        this.shape_35.setTransform(373.1, -308.9);

        this.shape_36 = new cjs.Shape();
        this.shape_36.graphics.f("#010202").s().p("AgFgEIABgFQACgGACgFQABAVAFAUQgKgOgBgLg");
        this.shape_36.setTransform(343.5, -323.5);

        this.shape_37 = new cjs.Shape();
        this.shape_37.graphics.f("#010202").s().p("AgfAPIAAgpQAgAKAgAEIAAAnQgegDgigJg");
        this.shape_37.setTransform(373.1, -290.1);

        this.shape_38 = new cjs.Shape();
        this.shape_38.graphics.f("#010202").s().p("AgfAEIAAgcQAfARAhAGIAAAaQghgHgfgOg");
        this.shape_38.setTransform(373.1, -304.6);

        this.shape_39 = new cjs.Shape();
        this.shape_39.graphics.f("#010202").s().p("AgSAWQgLgmgCgnQAJgEALAAIAOAAQADA+AaA5QgfgUgTgSg");
        this.shape_39.setTransform(348.2, -321.3);

        this.shape_40 = new cjs.Shape();
        this.shape_40.graphics.f("#010202").s().p("AgDAgQgOgdgDgiIAWAAQADAgAQAfg");
        this.shape_40.setTransform(363.9, -324);

        this.shape_41 = new cjs.Shape();
        this.shape_41.graphics.f("#010202").s().p("AgHAHQgPgTgDgYIAMAAQAXAAAKAKQAHAIAAATIAAAkQgWgMgMgSg");
        this.shape_41.setTransform(373.7, -323.5);

        this.shape_42 = new cjs.Shape();
        this.shape_42.graphics.f("#010202").s().p("AgNAMQgXgcgEgkIAWAAQAEAaAQAWQAPAVAZALIAAAZQgigNgVgcg");
        this.shape_42.setTransform(372.2, -321.9);

        this.shape_43 = new cjs.Shape();
        this.shape_43.graphics.f("#010202").s().p("AgfAAIAAgdQAcAaAkAJIAAAYQgigJgegVg");
        this.shape_43.setTransform(373.1, -313.1);

        this.shape_44 = new cjs.Shape();
        this.shape_44.graphics.f("#010202").s().p("AgHAgQgJgegDghIAZAAQADAhALAeg");
        this.shape_44.setTransform(356, -324);

        this.shape_45 = new cjs.Shape();
        this.shape_45.graphics.f("#010202").s().p("AgGAgIAAglIgeAAQgRgdgEgjIAaAAQAFAnAYAeQAXAeAlANIAAAbQglgLgbgbg");
        this.shape_45.setTransform(370.5, -320.2);

        this.shape_46 = new cjs.Shape();
        this.shape_46.graphics.f("#010202").s().p("AgGAgQgMgggCgfIAZAAQADAhANAeg");
        this.shape_46.setTransform(360, -324);

        this.shape_47 = new cjs.Shape();
        this.shape_47.graphics.f("#010202").s().p("Ag5AMQAqgvA0ghIAVAjQgcARgZAVIgDgFQgIgTgKgCQgUgEgIBVIgGAJIgHg5g");
        this.shape_47.setTransform(355.3, -357.5);

        this.shape_48 = new cjs.Shape();
        this.shape_48.graphics.f("#010202").s().p("AgVAhQAAghAJggIAIABIAaAAQgIAggBAgg");
        this.shape_48.setTransform(347.2, -333.8);

        this.shape_49 = new cjs.Shape();
        this.shape_49.graphics.f("#010202").s().p("AgTAgQACghAJgeIAcAAQgJAdgDAig");
        this.shape_49.setTransform(352, -333.7);

        this.shape_50 = new cjs.Shape();
        this.shape_50.graphics.f("#010202").s().p("AgiAFQAZgXAagQIASAeQgaAPgZAYg");
        this.shape_50.setTransform(360.2, -356.1);

        this.shape_51 = new cjs.Shape();
        this.shape_51.graphics.f("#010202").s().p("AgCgLIAGgCQAJgDAHACQgXAQgQANQAGgTALgHg");
        this.shape_51.setTransform(352.2, -368.9);

        this.shape_52 = new cjs.Shape();
        this.shape_52.graphics.f("#010202").s().p("AgOBsQgLAAgNgGQAFhmAyhWIAWgVIgTAkQgUArgGAgQgEAXAUALQgIAggBAmg");
        this.shape_52.setTransform(343.3, -341.3);

        this.shape_53 = new cjs.Shape();
        this.shape_53.graphics.f("#010202").s().p("AgpgHQAcgZAhgVIAJALIAPAZQguAdgpAqQgBghADgcg");
        this.shape_53.setTransform(353.8, -364);

        this.shape_54 = new cjs.Shape();
        this.shape_54.graphics.f("#010202").s().p("AgOACQAPgmAWgeQgaBAgFBGQgYgYASgqg");
        this.shape_54.setTransform(338.7, -338.9);

        this.shape_55 = new cjs.Shape();
        this.shape_55.graphics.f("#010202").s().p("AggAIQAWgUAdgUIAOAYQgcARgWAYg");
        this.shape_55.setTransform(362.6, -352);

        this.shape_56 = new cjs.Shape();
        this.shape_56.graphics.f("#010202").s().p("AghAuQADgcAQgYQAOgXAXgQIALATQglAbgHAtg");
        this.shape_56.setTransform(371.4, -335.1);

        this.shape_57 = new cjs.Shape();
        this.shape_57.graphics.f("#010202").s().p("AgaAiQAHgqAjgZIABADIAJAgQABAggpAAg");
        this.shape_57.setTransform(373.7, -333.9);

        this.shape_58 = new cjs.Shape();
        this.shape_58.graphics.f("#010202").s().p("AgqA8QAEgiAQgdIALAAIgFgJQASgdAcgSIANAVQgZARgPAZQgPAagEAeg");
        this.shape_58.setTransform(369.1, -336.5);

        this.shape_59 = new cjs.Shape();
        this.shape_59.graphics.f("#010202").s().p("AgdAMQATgZAcgUIAMAVQgcARgSAdg");
        this.shape_59.setTransform(368.7, -341.8);

        this.shape_60 = new cjs.Shape();
        this.shape_60.graphics.f("#010202").s().p("AgfAJQAXgXAbgSIANAXQgcARgVAZg");
        this.shape_60.setTransform(364.7, -348.5);

        this.shape_61 = new cjs.Shape();
        this.shape_61.graphics.f("#010202").s().p("AgTAgQADggAJgfIAbAAQgLAfgDAgg");
        this.shape_61.setTransform(356, -333.7);

        this.shape_62 = new cjs.Shape();
        this.shape_62.graphics.f("#010202").s().p("AgUAgQACggALgfIAcAAQgNAegDAhg");
        this.shape_62.setTransform(360, -333.7);

        this.shape_63 = new cjs.Shape();
        this.shape_63.graphics.f("#010202").s().p("AgfAKQAYgaAZgRIAOAXQgdASgTAag");
        this.shape_63.setTransform(366.7, -345.1);

        this.shape_64 = new cjs.Shape();
        this.shape_64.graphics.f("#010202").s().p("AgUAgQADgiAOgdIAYAAQgQAfgDAgg");
        this.shape_64.setTransform(363.9, -333.7);

        this.shape_65 = new cjs.Shape();
        this.shape_65.graphics.f("#010202").s().p("AgmADIASgdQAaAIAhAQIgSAdQgdgPgegJg");
        this.shape_65.setTransform(393.7, -360.9);

        this.shape_66 = new cjs.Shape();
        this.shape_66.graphics.f("#010202").s().p("AgmgCQAagOAhgKIATAeQgfAIgeAPg");
        this.shape_66.setTransform(368.7, -360.9);

        this.shape_67 = new cjs.Shape();
        this.shape_67.graphics.f("#010202").s().p("AglAAQAcgOAggJIAPAYQggAIgdAPg");
        this.shape_67.setTransform(371.1, -356.7);

        this.shape_68 = new cjs.Shape();
        this.shape_68.graphics.f("#010202").s().p("AgogBQAggRAngNQgDARANAVQgfAKgdAPg");
        this.shape_68.setTransform(365.9, -365.7);

        this.shape_69 = new cjs.Shape();
        this.shape_69.graphics.f("#010202").s().p("AhOgDQAMgcA5ABQA6AAAhAPQhVAJhIAlQgJgUAGgOg");
        this.shape_69.setTransform(368.6, -370.5);

        this.shape_70 = new cjs.Shape();
        this.shape_70.graphics.f("#010202").s().p("AgYgHQAPgLATALQAOAIABANQgYgMgZgJg");
        this.shape_70.setTransform(398.2, -369.1);

        this.shape_71 = new cjs.Shape();
        this.shape_71.graphics.f("#010202").s().p("AglABIAPgYQAgAKAdANIgPAYQgdgPgggIg");
        this.shape_71.setTransform(391.2, -356.7);

        this.shape_72 = new cjs.Shape();
        this.shape_72.graphics.f("#010202").s().p("AgmAEIAOgYQAhAFAfAOIgOAWQgfgOghgDg");
        this.shape_72.setTransform(387, -349.8);

        this.shape_73 = new cjs.Shape();
        this.shape_73.graphics.f("#010202").s().p("AAAAIQgQAAgRAGIgMgTQAVgIAYAAQAZAAAVAIIgMATQgRgGgRAAg");
        this.shape_73.setTransform(381.1, -340.7);

        this.shape_74 = new cjs.Shape();
        this.shape_74.graphics.f("#010202").s().p("AgdgEIAAgBQAOgFAPAAQAQAAAOAFQgKAPgUABIAAAAQgTAAgKgPg");
        this.shape_74.setTransform(381.1, -338.1);

        this.shape_75 = new cjs.Shape();
        this.shape_75.graphics.f("#010202").s().p("AAAAFQgiAAgfANIgMgTQAigOAggCIALATIAMgTQAhACAhAOIgMATQgggNgiAAg");
        this.shape_75.setTransform(381.2, -346.5);

        this.shape_76 = new cjs.Shape();
        this.shape_76.graphics.f("#010202").s().p("AgnAFIAUggIABgCQAdALAdAPIgUAhQgYgNgjgMg");
        this.shape_76.setTransform(396.4, -365.5);

        this.shape_77 = new cjs.Shape();
        this.shape_77.graphics.f("#010202").s().p("AglAAQAfgOAegHIAOAXQgfAGgfAOg");
        this.shape_77.setTransform(373.2, -353.3);

        this.shape_78 = new cjs.Shape();
        this.shape_78.graphics.f("#010202").s().p("AgmgBQAegOAhgFIAOAYQghADgfAOg");
        this.shape_78.setTransform(375.3, -349.8);

        this.shape_79 = new cjs.Shape();
        this.shape_79.graphics.f("#010202").s().p("AgmADIAPgYQAeAHAgAOIgOAWQgfgPgggEg");
        this.shape_79.setTransform(389.1, -353.3);

        this.shape_80 = new cjs.Shape();
        this.shape_80.graphics.f("#010202").s().p("AAAAIQgXAAgZAJIgOgVQAegMAgAAQAgAAAeAMIgNAVQgZgJgYAAg");
        this.shape_80.setTransform(381.2, -343.6);

        this.shape_81 = new cjs.Shape();
        this.shape_81.graphics.f("#010202").s().p("AgigEIASgeQAcASAXAUIgSAfQgWgVgdgSg");
        this.shape_81.setTransform(402.1, -356.1);

        this.shape_82 = new cjs.Shape();
        this.shape_82.graphics.f("#010202").s().p("AgMAkQgBgfgIggIAHAAQAMAAAOgIQAJAlABAig");
        this.shape_82.setTransform(415.1, -334.1);

        this.shape_83 = new cjs.Shape();
        this.shape_83.graphics.f("#010202").s().p("AgWgFIgCgEQAUgBATABIAKAUQgUgKgbgGg");
        this.shape_83.setTransform(411.6, -345);

        this.shape_84 = new cjs.Shape();
        this.shape_84.graphics.f("#010202").s().p("AgggIIAPgXQAbARAWAWIgOAYQgXgXgbgRg");
        this.shape_84.setTransform(399.7, -351.9);

        this.shape_85 = new cjs.Shape();
        this.shape_85.graphics.f("#010202").s().p("AgMgFQAOgUgbgTIgOggQAeAGAUALQAaBBADBFIgoABQgBgqgLgng");
        this.shape_85.setTransform(419, -338.1);

        this.shape_86 = new cjs.Shape();
        this.shape_86.graphics.f("#010202").s().p("AgtglQAfgKAjAwQASAZAHAOQgqgtgxggg");
        this.shape_86.setTransform(412.8, -366.4);

        this.shape_87 = new cjs.Shape();
        this.shape_87.graphics.f("#010202").s().p("AgQg3QAhAfAAA3QgBAPgMAKQgDg4gRg3g");
        this.shape_87.setTransform(424.1, -336.9);

        this.shape_88 = new cjs.Shape();
        this.shape_88.graphics.f("#010202").s().p("ABBA4QgOgjgigVQgTgMgPAFQgbgYgegSIANgXQAFgIAGgFQBHAsA0BDQAFAdgFAgQgBgOgHgRg");
        this.shape_88.setTransform(411.7, -360.7);

        this.shape_89 = new cjs.Shape();
        this.shape_89.graphics.f("#010202").s().p("AgjgBIAUgjQAbASAYAUIgBACIgUAgQgVgTgdgSg");
        this.shape_89.setTransform(404.9, -360.7);

        this.shape_90 = new cjs.Shape();
        this.shape_90.graphics.f("#010202").s().p("AALAuQgHgtglgbIAMgTQAWAQAPAXQAPAYADAcg");
        this.shape_90.setTransform(390.8, -335.1);

        this.shape_91 = new cjs.Shape();
        this.shape_91.graphics.f("#010202").s().p("AgdgMIALgVQAaATAWAaIgNAWQgRgcgdgSg");
        this.shape_91.setTransform(393.6, -341.8);

        this.shape_92 = new cjs.Shape();
        this.shape_92.graphics.f("#010202").s().p("AgIAgQgBgfgJggIAbAAQAJAfABAgg");
        this.shape_92.setTransform(410.3, -333.7);

        this.shape_93 = new cjs.Shape();
        this.shape_93.graphics.f("#010202").s().p("AAWAiQgSgCgNgKQgcgSAVgiIABgDQAiAZAHAqg");
        this.shape_93.setTransform(388.6, -333.9);

        this.shape_94 = new cjs.Shape();
        this.shape_94.graphics.f("#010202").s().p("AARA8QgDgegPgaQgQgYgZgSIANgVQAdATARAcIgFAJIAKAAQARAdAEAig");
        this.shape_94.setTransform(393.2, -336.5);

        this.shape_95 = new cjs.Shape();
        this.shape_95.graphics.f("#010202").s().p("AgBAgQgDgigQgdIAZAAQAOAfACAgg");
        this.shape_95.setTransform(398.4, -333.7);

        this.shape_96 = new cjs.Shape();
        this.shape_96.graphics.f("#010202").s().p("AgfgJIAOgXQAaASAXAXIgOAXQgVgYgcgRg");
        this.shape_96.setTransform(397.6, -348.5);

        this.shape_97 = new cjs.Shape();
        this.shape_97.graphics.f("#010202").s().p("AgEAgQgDgjgNgcIAcAAQALAfACAgg");
        this.shape_97.setTransform(402.2, -333.7);

        this.shape_98 = new cjs.Shape();
        this.shape_98.graphics.f("#010202").s().p("AgGAgQgCgggLgfIAbAAQAKAfACAgg");
        this.shape_98.setTransform(406.3, -333.7);

        this.shape_99 = new cjs.Shape();
        this.shape_99.graphics.f("#010202").s().p("AgfgKIAOgXQAbATAWAXIgPAZQgUgbgcgRg");
        this.shape_99.setTransform(395.6, -345.1);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.shape_99 }, { t: this.shape_98 }, { t: this.shape_97 }, { t: this.shape_96 }, { t: this.shape_95 }, { t: this.shape_94 }, { t: this.shape_93 }, { t: this.shape_92 }, { t: this.shape_91 }, { t: this.shape_90 }, { t: this.shape_89 }, { t: this.shape_88 }, { t: this.shape_87 }, { t: this.shape_86 }, { t: this.shape_85 }, { t: this.shape_84 }, { t: this.shape_83 }, { t: this.shape_82 }, { t: this.shape_81 }, { t: this.shape_80 }, { t: this.shape_79 }, { t: this.shape_78 }, { t: this.shape_77 }, { t: this.shape_76 }, { t: this.shape_75 }, { t: this.shape_74 }, { t: this.shape_73 }, { t: this.shape_72 }, { t: this.shape_71 }, { t: this.shape_70 }, { t: this.shape_69 }, { t: this.shape_68 }, { t: this.shape_67 }, { t: this.shape_66 }, { t: this.shape_65 }, { t: this.shape_64 }, { t: this.shape_63 }, { t: this.shape_62 }, { t: this.shape_61 }, { t: this.shape_60 }, { t: this.shape_59 }, { t: this.shape_58 }, { t: this.shape_57 }, { t: this.shape_56 }, { t: this.shape_55 }, { t: this.shape_54 }, { t: this.shape_53 }, { t: this.shape_52 }, { t: this.shape_51 }, { t: this.shape_50 }, { t: this.shape_49 }, { t: this.shape_48 }, { t: this.shape_47 }, { t: this.shape_46 }, { t: this.shape_45 }, { t: this.shape_44 }, { t: this.shape_43 }, { t: this.shape_42 }, { t: this.shape_41 }, { t: this.shape_40 }, { t: this.shape_39 }, { t: this.shape_38 }, { t: this.shape_37 }, { t: this.shape_36 }, { t: this.shape_35 }, { t: this.shape_34 }, { t: this.shape_33 }, { t: this.shape_32 }, { t: this.shape_31 }, { t: this.shape_30 }, { t: this.shape_29 }, { t: this.shape_28 }, { t: this.shape_27 }, { t: this.shape_26 }, { t: this.shape_25 }, { t: this.shape_24 }, { t: this.shape_23 }, { t: this.shape_22 }, { t: this.shape_21 }, { t: this.shape_20 }, { t: this.shape_19 }, { t: this.shape_18 }, { t: this.shape_17 }, { t: this.shape_16 }, { t: this.shape_15 }, { t: this.shape_14 }, { t: this.shape_13 }, { t: this.shape_12 }, { t: this.shape_11 }, { t: this.shape_10 }, { t: this.shape_9 }, { t: this.shape_8 }, { t: this.shape_7 }, { t: this.shape_6 }, { t: this.shape_5 }, { t: this.shape_4 }, { t: this.shape_3 }, { t: this.shape_2 }, { t: this.shape_1 }, { t: this.shape }] }).wait(80));

        // FABRIC
        this.instance_3 = new lib.FABRIC("synched", 0);
        this.instance_3.parent = this;
        this.instance_3.setTransform(455.5, 43.1);

        this.timeline.addTween(cjs.Tween.get(this.instance_3).wait(80));

        // HUMO copia copia
        this.instance_4 = new lib.HUMO("synched", 0);
        this.instance_4.parent = this;
        this.instance_4.setTransform(508, -52.5, 0.5, 0.5, -0.5);
        this.instance_4._off = true;

        this.timeline.addTween(cjs.Tween.get(this.instance_4).wait(39).to({ _off: false }, 0).wait(41));

        // HUMO copia
        this.instance_5 = new lib.HUMO("synched", 0);
        this.instance_5.parent = this;
        this.instance_5.setTransform(474.7, -35.6, 0.5, 0.5, -0.5);
        this.instance_5._off = true;

        this.timeline.addTween(cjs.Tween.get(this.instance_5).wait(18).to({ _off: false }, 0).wait(62));

        // HUMO
        this.instance_6 = new lib.HUMO("synched", 0);
        this.instance_6.parent = this;
        this.instance_6.setTransform(449.6, -30.1, 0.5, 0.5, -0.5);

        this.timeline.addTween(cjs.Tween.get(this.instance_6).wait(80));

        // MASAGUA
        this.instance_7 = new lib.MASAGUA("synched", 0);
        this.instance_7.parent = this;
        this.instance_7.setTransform(-511.5, -154.6);

        this.timeline.addTween(cjs.Tween.get(this.instance_7).wait(80));

        // ELEC
        this.instance_8 = new lib.ELEC("synched", 0);
        this.instance_8.parent = this;
        this.instance_8.setTransform(36.7, 252.5);

        this.timeline.addTween(cjs.Tween.get(this.instance_8).wait(80));

        // molino copia
        this.instance_9 = new lib.molino("synched", 0);
        this.instance_9.parent = this;
        this.instance_9.setTransform(-365.7, 224.9, 1.371, 1.371, 0, 0, 0, -0.1, 0.1);

        this.timeline.addTween(cjs.Tween.get(this.instance_9).wait(80));

        // molino copia
        this.instance_10 = new lib.molino("synched", 6);
        this.instance_10.parent = this;
        this.instance_10.setTransform(-461.2, 215.1);

        this.timeline.addTween(cjs.Tween.get(this.instance_10).wait(80));

        // molino
        this.instance_11 = new lib.molino("synched", 2);
        this.instance_11.parent = this;
        this.instance_11.setTransform(-411.9, 224.1, 0.763, 0.763);

        this.timeline.addTween(cjs.Tween.get(this.instance_11).wait(80));

        // agua1
        this.instance_12 = new lib.agua1("synched", 0);
        this.instance_12.parent = this;
        this.instance_12.setTransform(-493.3, -0.1, 1, 1, 180);

        this.timeline.addTween(cjs.Tween.get(this.instance_12).wait(80));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(-548, -373.6, 1091.1, 721.2);


    // stage content:
    (lib.InfografiaAnimadaAnimatefinalv3 = function (mode, startPosition, loop) {
        this.initialize(mode, startPosition, loop, {});

        // timeline functions:
        this.frame_0 = function () {
            var page = null;

            this.clickAgain = function () {
                canClick = true;
            }

            this.btn_costo.on("click", onClickCosto.bind(this));

            function onClickCosto(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Costo");
                    page = new lib.mc_costo();
                    page.x = 600;
                    page.y = 485;
                    this.addChild(page);
                }
            }

            this.btn_demanda.on("click", onClickDemanda.bind(this));

            function onClickDemanda(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Demanda");
                    page = new lib.mc_demanda();
                    page.x = 780;
                    page.y = 530;
                    this.addChild(page);
                }
            }

            this.btn_hidro.on("click", onClickHidro.bind(this));

            function onClickHidro(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Hidro");
                    page = new lib.mc_hidro();
                    page.x = 600;
                    page.y = 485;
                    this.addChild(page);
                }
            }

            this.btn_participacion.on("click", onClickParticipacion.bind(this));

            function onClickParticipacion(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Participacion");
                    page = new lib.mc_participacion();
                    page.x = 780;
                    page.y = 530;
                    this.addChild(page);
                }
            }

            this.btn_renovables.on("click", onClickRenovables.bind(this));

            function onClickRenovables(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click Renovables");
                    page = new lib.mc_renovables();
                    page.x = 600;
                    page.y = 485;
                    this.addChild(page);
                }
            }

            this.btn_termo.on("click", onClickTermo.bind(this));

            function onClickTermo(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Termo");
                    page = new lib.mc_termo();
                    page.x = 600;
                    page.y = 485;
                    this.addChild(page);
                }
            }

            this.btn_transmicion.on("click", onClickTransmicion.bind(this));

            function onClickTransmicion(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Transmicion");
                    page = new lib.mc_transmicion();
                    page.x = 780;
                    page.y = 530;
                    this.addChild(page);
                }
            }

            this.btn_valorizacion.on("click", onClickValorizacion.bind(this));

            function onClickValorizacion(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Valorizacion");
                    page = new lib.mc_valorizacion();
                    page.x = 780;
                    page.y = 530;
                    this.addChild(page);
                }
            }

            this.btn_variacion.on("click", onClickVariacion.bind(this));

            function onClickVariacion(e) {
                if (canClick) {
                    if (page != null) {
                        this.removeChild(page);
                        page = null;
                    }

                    canClick = false;
                    console.log("Click en Variacion");
                    page = new lib.mc_variacion();
                    page.x = 600;
                    page.y = 485;
                    this.addChild(page);
                }
            }
        }

        // actions tween:
        this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(80));

        // BOTON ENERGIAS RENOVABLES
        this.btn_renovables = new lib.btn_renovables();
        this.btn_renovables.name = "btn_renovables";
        this.btn_renovables.parent = this;
        this.btn_renovables.setTransform(180.2, 742.3);
        new cjs.ButtonHelper(this.btn_renovables, 0, 1, 2, false, new lib.btn_renovables(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_renovables).wait(80));

        // BOTON PRODUCCION ELECTRICA
        this.btn_termo = new lib.btn_termo();
        this.btn_termo.name = "btn_termo";
        this.btn_termo.parent = this;
        this.btn_termo.setTransform(546, 754.2, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_termo, 0, 1, 2, false, new lib.btn_termo(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_termo).wait(80));

        // BOTON MAXIMA DEMANDA
        this.btn_demanda = new lib.btn_demanda();
        this.btn_demanda.name = "btn_demanda";
        this.btn_demanda.parent = this;
        this.btn_demanda.setTransform(594.2, 521.4, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_demanda, 0, 1, 2, false, new lib.btn_demanda(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_demanda).wait(80));

        // BOTON LINEAS DE TRANSMISION
        this.btn_transmicion = new lib.btn_transmicion();
        this.btn_transmicion.name = "btn_transmicion";
        this.btn_transmicion.parent = this;
        this.btn_transmicion.setTransform(771.1, 659.3, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_transmicion, 0, 1, 2, false, new lib.btn_transmicion(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_transmicion).wait(80));

        // BOTON VARIACION
        this.btn_variacion = new lib.btn_variacion();
        this.btn_variacion.name = "btn_variacion";
        this.btn_variacion.parent = this;
        this.btn_variacion.setTransform(1143.4, 692.5, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_variacion, 0, 1, 2, false, new lib.btn_variacion(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_variacion).wait(80));

        // BOTON PARTICIPACION
        this.btn_participacion = new lib.btn_participacion();
        this.btn_participacion.name = "btn_participacion";
        this.btn_participacion.parent = this;
        this.btn_participacion.setTransform(1015.3, 443.7, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_participacion, 0, 1, 2, false, new lib.btn_participacion(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_participacion).wait(80));

        // BOTON COSTO MARGINAL
        this.btn_costo = new lib.btn_costo();
        this.btn_costo.name = "btn_costo";
        this.btn_costo.parent = this;
        this.btn_costo.setTransform(623.1, 316.3, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_costo, 0, 1, 2, false, new lib.btn_costo(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_costo).wait(80));

        // BOTON VALORIZACION DE TRANSFERENCIAS
        this.btn_valorizacion = new lib.btn_valorizacion();
        this.btn_valorizacion.name = "btn_valorizacion";
        this.btn_valorizacion.parent = this;
        this.btn_valorizacion.setTransform(721.6, 258.5, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_valorizacion, 0, 1, 2, false, new lib.btn_valorizacion(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_valorizacion).wait(80));

        // BOTON HIDRO
        this.btn_hidro = new lib.btn_hidro();
        this.btn_hidro.name = "btn_hidro";
        this.btn_hidro.parent = this;
        this.btn_hidro.setTransform(171.2, 322, 1, 1, 0, 0, 0, 18.8, 18.8);
        new cjs.ButtonHelper(this.btn_hidro, 0, 1, 2, false, new lib.btn_hidro(), 3);

        this.timeline.addTween(cjs.Tween.get(this.btn_hidro).wait(80));

        // NUBES
        this.instance = new lib.NUBES("synched", 0);
        this.instance.parent = this;
        this.instance.setTransform(520, 136);

        this.timeline.addTween(cjs.Tween.get(this.instance).wait(80));

        // LOGOCOES2
        this.instance_1 = new lib.ESCENA1("synched", 0);
        this.instance_1.parent = this;
        this.instance_1.setTransform(611.6, 463.9);

        this.instance_2 = new lib.ojala2();
        this.instance_2.parent = this;
        this.instance_2.setTransform(14, 38, 0.24, 0.24);

        this.timeline.addTween(cjs.Tween.get({}).to({ state: [{ t: this.instance_2 }, { t: this.instance_1 }] }).wait(80));

    }).prototype = p = new cjs.MovieClip();
    p.nominalBounds = new cjs.Rectangle(614, 463, 1171, 782.1);
    // library properties:
    lib.properties = {
        id: '7764188A17BEC441908B58278E71CEF9',
        width: 1200,
        height: 850,
        fps: 24,
        color: "#FFFFFF",
        opacity: 1.00,
        manifest: [
            { src: siteRoot + "areas/publicaciones/content/images/InfografiaAnimadaAnimatefinalv3_atlas_.png?1584570490145", id: "InfografiaAnimadaAnimatefinalv3_atlas_" }
        ],
        preloads: []
    };



    // bootstrap callback support:

    (lib.Stage = function (canvas) {
        createjs.Stage.call(this, canvas);
    }).prototype = p = new createjs.Stage();

    p.setAutoPlay = function (autoPlay) {
        this.tickEnabled = autoPlay;
    }
    p.play = function () { this.tickEnabled = true; this.getChildAt(0).gotoAndPlay(this.getTimelinePosition()) }
    p.stop = function (ms) { if (ms) this.seek(ms); this.tickEnabled = false; }
    p.seek = function (ms) { this.tickEnabled = true; this.getChildAt(0).gotoAndStop(lib.properties.fps * ms / 1000); }
    p.getDuration = function () { return this.getChildAt(0).totalFrames / lib.properties.fps * 1000; }

    p.getTimelinePosition = function () { return this.getChildAt(0).currentFrame / lib.properties.fps * 1000; }

    an.bootcompsLoaded = an.bootcompsLoaded || [];
    if (!an.bootstrapListeners) {
        an.bootstrapListeners = [];
    }

    an.bootstrapCallback = function (fnCallback) {
        an.bootstrapListeners.push(fnCallback);
        if (an.bootcompsLoaded.length > 0) {
            for (var i = 0; i < an.bootcompsLoaded.length; ++i) {
                fnCallback(an.bootcompsLoaded[i]);
            }
        }
    };

    an.compositions = an.compositions || {};
    an.compositions['7764188A17BEC441908B58278E71CEF9'] = {
        getStage: function () { return exportRoot.getStage(); },
        getLibrary: function () { return lib; },
        getSpriteSheet: function () { return ss; },
        getImages: function () { return img; }
    };

    an.compositionLoaded = function (id) {
        an.bootcompsLoaded.push(id);
        for (var j = 0; j < an.bootstrapListeners.length; j++) {
            an.bootstrapListeners[j](id);
        }
    }

    an.getComposition = function (id) {
        return an.compositions[id];
    }



})(createjs = createjs || {}, AdobeAn = AdobeAn || {});
var createjs, AdobeAn;