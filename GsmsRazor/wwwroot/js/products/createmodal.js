$(() => {
    $("#createCategoryModal").on('hidden.bs.modal', function (event) {
        $("#NewProduct_CategoryId").prop("selectedIndex", 0);
    });

    $("#NewProduct_CategoryId").on('change', function () {
        if (this.value == 'addNewCategory') {
            $('#createCategoryModal').modal('show');
        }
    });

    $("#btnCreateCategory").on('click', function () {
        var categoryName = $("#NewProduct_Category_Name").val();
        if (!categoryName) {
            $("#createCategoryModal").find("form").find("span").text("Category name must not be empty");
            return;
        }
        if (categoryName.length < 6) {
            $("#createCategoryModal").find("form").find("span").text("Category name is required at least 6 characters!!!");
            return;
        }
        if (categoryName.length > 32) {
            $("#createCategoryModal").find("form").find("span").text("Category name is limited to 32 characters!!");
            return;
        }
        if (!/^[^@#$%^*()\[\]{}]+$/.test(categoryName)) {
            $("#createCategoryModal").find("form").find("span").text("Category name contains invalid characters!!");
            return;
        }
        var token = $("#createCategoryModal input[name=__RequestVerificationToken]").val();
        $.ajax({
            url: '/Products?handler=CreateCategory',
            type: 'post',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", token);
            },
            data: JSON.stringify({
                Name: categoryName
            }),
            success: (result) => {
                $("#NewProduct_Category_Name").text("");

                $("#NewProduct_CategoryId option").eq(0)
                    .before($("<option></option>").val(result.id).text(result.name));
                $("#NewProduct_CategoryId").prop("selectedIndex", 0);

                $('#createCategoryModal').modal('hide');
                toastr.options.showEasing = 'swing';
                toastr.success('Category created successfully!', "Success");
            },
            error: (error) => {
                console.log(error);
                $("#createCategoryModal").find("form").find("span").text(error);
            }
        });
    });
});