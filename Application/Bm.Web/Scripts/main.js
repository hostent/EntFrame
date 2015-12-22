
//这里配置文件的依赖
require.config({

    paths: {
        'jquery': 'jquery/jquery-1.10.2',
        'valid': 'jquery/jquery.validate',
        'demoPlunin': 'DemoPlunin1/demoPlunin'
    },
    shim: {
        'valid': {
            deps: ['jquery'],
            exports: 'valid'
        },
        'demoPlunin': {
            exports: 'demoPlunin'
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
