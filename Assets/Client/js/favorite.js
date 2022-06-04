var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $('.btnLogin').click(function () {
            PNotify.notice({
                title: 'THÔNG BÁO!!',
                text: 'Bạn vui lòng đăng nhập để thích món ăn này.'
            });
        });

        $('.btnFavorite').click(function () {
            var islike = $(this).children().hasClass('fa-heart-o');
            if (islike) {
                $(this).children().first().removeClass('fa-heart-o');
                $(this).children().first().addClass('fa-heart');
                var count = $(this).children().last().text();
                if (count == "") {
                    $(this).children().last().text("1");
                } else {
                    var Soluong = parseInt(count) + 1;
                    $(this).children().last().text(Soluong);
                }
                $.ajax({
                    data: {
                        User_ID: $(this).data('userid'),
                        Food_ID: $(this).data('foodid'),
                        isLike: true
                    },
                    url: '/Food/AddFavorite',
                    dataType: 'Json',
                    type: 'POST',
                    success: function () { }
                })


            }
            else {
                $(this).children().first().removeClass('fa-heart');
                $(this).children().first().addClass('fa-heart-o');

                var count = $(this).children().last().text();
                var Soluong = parseInt(count) - 1;
                $(this).children().last().text(Soluong);
                if (Soluong == 0) {
                    $(this).children().last().text("");
                } else {
                    $(this).children().last().text(Soluong);
                }
                $.ajax({
                    data: {
                        User_ID: $(this).data('userid'),
                        Food_ID: $(this).data('foodid'),
                        isLike: false
                    },
                    url: '/Food/AddFavorite',
                    dataType: 'Json',
                    type: 'POST',
                    success: function () { }
                })

            }


        });
    }
}
common.init();