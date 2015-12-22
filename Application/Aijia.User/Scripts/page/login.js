
//页面作用域
var scrop_login = {}


define(['jquery'], function ($) {




    //包上这个，表示文档加载完成后才执行，page on load
    (function () {

        


    })(window);



    $("input[name=useName]").change(function () {

        $.post('login/HasRegedit', { phoneNum: $(this).val() }, function (data) {
            if (data.Tag != 1) {
                $(".z-err").show();
                $(".u-err").val(data.Message);
            }


        });

    });

 

    scrop_login.onLogin = function () {
        

      


    }






});
