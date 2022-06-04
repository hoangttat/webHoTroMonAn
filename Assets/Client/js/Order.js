var order = {
    init: function () {
        order.regEvents();
    },
    regEvents: function () {

        //lấy ngày, giờ, số lượng khách khi khách k chọn món ăn
        $('.btn-Order').off('click').on('click', function () {
            var list = [];
            list.push({
                BookDate: $('#date-now').val(),
                Quantity: $('.quantity').val(),
                Time: $('.AtTime').val()
            });           

            $.ajax({
                url: 'Home/BookCustomer',
                data: {Cus_book: JSON.stringify(list)},
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/dat-ban";
                    }
                }
            });
        });

        //lấy ngày, giờ, số lượng khách khi khách  có chọn món ăn
        $('.btnOrder').off('click').on('click', function () {
            var list = [];
            list.push({
                BookDate: $('#date-now').val(),
                Quantity: $('.quantity').val(),
                Time: $('.AtTime').val()
            });

            $.ajax({
                url: 'Home/BookCustomer',
                data: { Cus_book: JSON.stringify(list) },
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/dat-ban-menu";
                    }
                }
            });
        });


        $('.btn-Edit').off('click').on('click', function () {
            var listfood = $('.txtQuantity');
            var arr = [];
            $.each(listfood, function (i, item) {
                arr.push({
                    quantity: $(item).val(),
                    food: {
                        ID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: 'Order/Edit',
                data: { EditFood: JSON.stringify(arr) },
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/dat-ban-menu";
                    }
                }
            })
        });

        $('.btn-Delete').off('.click').on('click', function () {
            var conf = confirm("Bạn có thực sự muốn xóa món ăn này ra khỏi thực đơn của bạn??");
            if (conf == true) {
                $.ajax({
                    url: 'Order/Delete',
                    data: { id: $(this).data('id') },
                    dataType: 'Json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/dat-ban-menu";
                        }
                    }
                });
            }
        });


        $('#btnDeleteAll').off('click').on('click', function () {
            var conf = confirm("Bạn thực sự muốn xóa thực đơn mà bạn đã chọn ??");
            if (conf == true) {
                $.ajax({
                    url: 'Order/DeleteAll',
                    dataType: 'Json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = "/dat-ban";
                        }
                    }
                });
            }
        });

        $('.btn-book').off('click').on('click', function () {
            var list1 = [];
            var list2 = [];
            var giveFood = $('.txtQuantity');
            var giveCus = $('.order');
            $.each(giveFood, function (i, item) {                
                list1.push({
                    Food_ID: $(item).data('id'),
                    Count: $(item).val(),
                    Price: $(item).data('price')
                });
            });

            $.each(giveCus, function (i, item) {
                list2.push({
                    Full_Name: $('.name').val(),
                    Phone: $('.sodt').val(),
                    Email: $('.email').val(),
                    Description: $('.noidung').val(),
                    DateBook: $('.datepicker').val(),
                    TimeBook: $('.AtTime').val(),
                    Count_people: $('.quantity').val()
                });
            });

            $.ajax({
                url: 'Order/SaveMenu',
                data: { Menu: JSON.stringify(list1), customer: JSON.stringify(list2) },
                dataType: 'Json',
                type: 'POST',
                success: function (res) {
                    if (status.res == true) {                        
                        window.location.href = "/";
                    }
                }
            });
        });
    }
}

order.init();

$(function () {
    //nếu không có thao tác gì thì ẩn đi
    $('#AlertBox').removeClass('hide');

    //Sau khi hiển thị lên thì delay 1s và cuộn lên trên sử dụng slideup
    $('#AlertBox').delay(5000).slideUp(500);
});