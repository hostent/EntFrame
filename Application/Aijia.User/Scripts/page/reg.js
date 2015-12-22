
//页面作用域
var scrop_reg = {}


define(['jquery', 'String', 'valid'], function ($, String, valid) {




    //包上这个，表示文档加载完成后才执行，page on load
    (function () {
        scrop_reg.gvalid = $("#commentForm").gvalid(
             {
                 onValided: function (errorList) {

                     $(".none").show();
                     $("#tit").html("");

                     if (errorList.length > 0) {
                         $("#tit").html(errorList[0].msg);
                     }
                     if(errorList.length==0)
                     {
                         $(".none").hide();
                     }
                 },
                 isFocus: false

             });

    })(window);


    scrop_reg.onReg = function () {

        if (scrop_reg.gvalid.valid()) {
            alert("开始注册");
        }
    }





});
