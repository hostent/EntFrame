/**
 * Created by Administrator on 2015/12/16.
 */
$(function(){
    function hasPlaceholderSupport() {
        var input = document.createElement('input');
        return ('placeholder' in input);
    }
    var firstNameBox = $('first_name'),
        message = firstNameBox.get('placeholder');
    firstNameBox.addEvents({
        focus: function () {
            if (firstNameBox.value == message) {
                searchBox.value = '';
            }
        },
        blur: function () {
            if (firstNameBox.value == '') {
                searchBox.value = message;
            }
        }
    });
});
