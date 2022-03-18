$(() => {
    $("button[name='btnAddToImportOrder']").on("click", function (e) {
        var productId = $(e.currentTarget).attr("data-productid");
        var token = $("input[name=__RequestVerificationToken]").val();
        $.ajax({
            url: '/ImportOrders/Create?handler=AddToCart',
            method: 'post',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", token);
            },
            data: {
                productId: productId
            },
            success: (result) => {
                generateIODTableBody(result);
                toastr.options.showEasing = 'swing';
                toastr.success('Item added!', "Success");
            },
            error: (error) => {
                console.log(error);
            }
        });
    });
});

function saveCartItem(productId) {
    var price = $(`input[name='iodPrice'][data-productid=${productId}]`).val();
    var quantity = $(`input[name='iodQuantity'][data-productid=${productId}]`).val();
    var distributor = $(`input[name='iodDistributor'][data-productid=${productId}]`).val();

    if (checkIOD(price, quantity, distributor)) {
        var token = $("input[name=__RequestVerificationToken]").val();
        $.ajax({
            url: '/ImportOrders/Create?handler=SaveCartItem',
            method: 'post',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", token);
            },
            data: {
                productId: productId,
                price: price,
                quantity: quantity,
                distributor: distributor
            },
            success: (result) => {
                generateIODTableBody(result);
                toastr.options.showEasing = 'swing';
                toastr.success('Item saved!', "Success");
            },
            error: (error) => {
                console.log(error);
            }
        });
    }
}

function checkIOD(price, quantity, distributor) {
    if (price < 0 || price > 79228162514264337593543950335) {
        alert("Price is required to be a positive decimal number!!");
        return false;
    }

    if (quantity < 0 || quantity > 2147483647) {
        alert("Quantity is required to be a positive integer number!!");
        return false;
    }

    return true;
}

function removeFromCart(productId) {
    var token = $("input[name=__RequestVerificationToken]").val();

    $.ajax({
        url: '/ImportOrders/Create?handler=RemoveFromCart',
        method: 'post',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", token);
        },
        data: {
            productId: productId
        },
        success: (result) => {
            generateIODTableBody(result);
            toastr.options.showEasing = 'swing';
            toastr.success('Item removed!', "Success");
        },
        error: (error) => {
            console.log(error);
        }
    });
};

function generateIODTableBody(result) {
    var tr = "";
    $.each(result, function (k, v) {
        tr += `<tr>
<td>
${v.name}
</td>
                    <td>
<input class="form-control" type="number"
                                       min=0 max=79228162514264337593543950335
                                       data-productid="${v.productId}"
                                       name="iodPrice" value="${v.price}" />
</td>
                    <td>
<input class="form-control" type="number"
                                       min=0 max=2147483647
                                       data-productid="${v.productId}"
                                       name="iodQuantity" value="${v.quantity}" />
</td>
                    <td>
<input class="form-control" type="text"
                                       data-productid="${v.productId}"
                                       name="iodDistributor" value="${v.distributor}" />
</td>
                    <td>
                        <div class="row">
                            <button data-productid=${v.productId} name="btnSaveIOD"
                                    class="btn btn-success text-white"
                                    onclick="saveCartItem('${v.productId}')"
                                >
                                <i class="fa-solid fa-floppy-disk"></i>
                            </button>
                            <button data-productid=${v.productId} name="btnDeleteIOD"
                                    class="btn btn-outline-danger"
                                    onclick="removeFromCart('${v.productId}')"
                                >
                                <i class="fa-solid fa-trash-can"></i>
                            </button>
                        </div>
                    </td>
                </tr>`;
    });
    $("#importOrderDetailsBody").html(tr);
}