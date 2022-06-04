var menu = {
    init: function () {
        menu.regEvents();
    },
    regEvents: function () {
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
                        window.location.href = "/Order";
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
                            window.location.href = "/Order";
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
                            window.location.href = "/Order";
                        }
                    }
                });
            }            
        });
    }
}

menu.init();