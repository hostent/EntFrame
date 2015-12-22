
//这里配置文件的依赖
require.config({

    paths: {
        'jquery': 'jquery-2.1.4.min',         
        'String': 'String',
        'valid': 'gvalid'
    },
    shim: {       
        'valid': {
            deps: ['String', 'jquery'],
            exports: 'valid'
        },
    },
    waitSeconds: 150     
});

//Rout js
require(['jquery'], function ($) {

    page = $("script[data-js]").attr("data-js");

    require([page], function (a) {
    });

});
