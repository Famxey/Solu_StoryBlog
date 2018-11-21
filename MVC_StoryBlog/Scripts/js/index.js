function turnPics() {
    var index = 0;
    var stop = false;
    var $li = $(".banner").find("#imgs li");
    var $pageIndex = $(".banner").find("#numbers li");
    $pageIndex.eq(index).addClass("number_over")
    .stop(true, true)
    .siblings()
    .removeClass("number_over");
    setInterval(function () {
        if (stop) return;
        index++;
        if (index >= $li.length) {
            index = 0;
        }
        $li.eq(index)
        .stop(true, true)
        .fadeIn("slow")
        .siblings().fadeOut("slow");
        $pageIndex.eq(index).addClass("number_over")
   .stop(true, true)
   .siblings()
   .removeClass("number_over");

    }, 3000);


    $pageIndex.mouseover(function () {
        stop = true;
        index = $pageIndex.index($(this));
        $li.eq(index)
         .stop(true, true)
         .fadeIn("slow")
         .siblings().fadeOut("slow");
        $(this).addClass("number_over")
   .stop(true, true)
   .siblings()
   .removeClass("number_over");
    }).mouseout(function () { stop = false;});
}
function leftScroll() {
    var stop = false;
    var marginLeft = 0;
    setInterval(function () {
        if (stop) return;
        $(".tgjlx").find("li").
            first().animate({ "margin-left": marginLeft-- }, 0, function () {
                var $first = $(this);
                if (!$first.is(":animated")) {
                    if ((-marginLeft) > $first.width() + 1 + 18) {
                        $first.css({ "margin-left": "0" }).appendTo($(".tgjlx ul"));
                        marginLeft = 0;
                    }
                }
            });
    }, 20);

    $(".tgjlx ul").mouseover(function () {
        stop = true;
    }).mouseout(function () {
        stop = false;
    });
}



$(function () {
    turnPics(); 
    leftScroll();
});