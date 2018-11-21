function xjxc() {
    $('#luoxinjianxc').css("display", "block");
    $("#luosczp a").removeAttr('onclick');//去掉a标签中的onclick事件

}
function sczp() {
    $('#luoshangcxc').css("display", "block");
    $("#luoxjxc a").removeAttr('onclick');//去掉a标签中的onclick事件

}
function gbi() {
    //添加a标签中的onclick事件
    var bObj = document.getElementById("sczpian");
    bObj.onclick = sczp;
    $('#luoxinjianxc').css("display", "none");
    $('#picClsDescribe').val("");
    $('#luoname').val("");
    $('#Text1').val("");
}
function sczpgbi() {
    //添加a标签中的onclick事件
    var bObj = document.getElementById("xjxcen");
    bObj.onclick = xjxc;
    $('#luoshangcxc').css("display", "none");
    //清楚filename的内容值
    $('#filename').val("");
    $('#zpmc').val("");
}


$(function () {
    $("#shangchang").change(function () {
        $(this).parents(".uploader").find(".filename").val($(this).val());


    });
});
$(function () {
    $("#File1").change(function () {
        $(this).parents(".uploaderx").find(".filename").val($(this).val());

    });
});