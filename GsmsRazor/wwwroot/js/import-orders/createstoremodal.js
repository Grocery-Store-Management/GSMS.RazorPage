$(() => {
    $("#createStoreModal").on('hidden.bs.modal', function (event) {
        $("#ImportOrder_StoreId").prop("selectedIndex", 0);
    });

    $("#ImportOrder_StoreId").on('change', function () {
        if (this.value == 'addNewStore') {
            $('#createStoreModal').modal('show');
        }
    });
    $("#btnCreateStore").on('click', function () {
        var storeName = $("#ImportOrder_Store_Name").val();
        if (!storeName) {
            $("#createStoreModal").find("form").find("span").text("Store name is required!!");
            return;
        }
        if (storeName.length < 6) {
            $("#createStoreModal").find("form").find("span").text("Store name is required at least 6 characters!!!");
            return;
        }
        if (storeName.length > 32) {
            $("#createStoreModal").find("form").find("span").text("Store name is limited to 32 characters!!");
            return;
        }
        if (!/^[^@#$%^*()\[\]{}]+$/.test(storeName)) {
            $("#createStoreModal").find("form").find("span").text("Store name contains invalid characters!!");
            return;
        }
        var token = $("#createStoreModal input[name=__RequestVerificationToken]").val();
        $.ajax({
            url: '/ImportOrders/Create?handler=CreateStore',
            type: 'post',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", token);
            },
            data: JSON.stringify({
                Name: storeName
            }),
            success: (result) => {
                $("#ImportOrder_Store_Name").text("");

                $("#ImportOrder_StoreId option").eq(0)
                    .before($("<option></option>").val(result.id).text(result.name));
                $("#ImportOrder_StoreId").prop("selectedIndex", 0);

                $('#createStoreModal').modal('hide');
                toastr.options.showEasing = 'swing';
                toastr.success('Store created successfully!', "Success");
            },
            error: (error) => {
                console.log(error);
                $("#createStoreModal").find("form").find("span").text(error);
            }
        });
    })
})