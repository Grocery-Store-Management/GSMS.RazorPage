$(".AddToCart").click(function () {
    var productId = $(this).closest('tr').find('.ProductId').val();
    $.ajax({
        url: `/Sale?handler=AddToCart`,
        method: "get",
        data: {
            Id: productId,
        },
        success: (result) => {
            var tr = "";
            var totalPrice = 0;
            $.each(result, function (k, v) {
                totalPrice += v.price * v.quantity;
                tr += `<tr>

                                <td>${v.productName}</td>

                                <td>

                                    <input onchange="QuantityChange('${v.productId}', this)" class="CartItemQuantity" style="width:50px" min="1" type="number" value="${v.quantity}" />
                                        <input type="hidden" value="${v.productId}" class="CartItemProductId" />
                                    </td>
                                    <td>
                                        <a onclick="RemoveFromCart('${v.productId}')" class="btn btn-outline-danger"><i class="fa-solid fa-trash-can"></i></a>
                                    </td>
                                </tr>`
            });
            $("#ReceiptBody").html(tr);
            $("#TotalPrice").text(totalPrice);
        },
        error: (error) => {
            console.log(error)
        }
    })
});

function QuantityChange(productId, element) {
    var quantity = element.value;
    var productId = productId;

    $.ajax({
        url: `/Sale?handler=QuantityChange`,
        method: "get",
        data: {
            quantity: quantity,
            productId: productId,
        },
        success: (result) => {
            var tr = "";
            var totalPrice = 0;
            $.each(result, function (k, v) {
                totalPrice += v.price * v.quantity;
                tr += `<tr>

                                <td>${v.productName}</td>

                                <td>

                                    <input onchange="QuantityChange('${v.productId}', this)" class="CartItemQuantity" style="width:50px" min="1" type="number" value="${v.quantity}" />

                                    <input type="hidden" value="${v.productId}" class="CartItemProductId" />

                                </td>

                                <td>

                                    <a onclick="RemoveFromCart('${v.productId}')" class="btn btn-outline-danger"><i class="fa-solid fa-trash-can"></i></a>

                                </td>

                            </tr>`
            });
            $("#ReceiptBody").html(tr);
            $("#TotalPrice").text(totalPrice);
        },
        error: (error) => {
            console.log(error)
        }
    })
}

function RemoveFromCart(productId) {
    $.ajax({
        url: `/Sale?handler=RemoveFromCart`,
        method: "get",
        data: {
            productId: productId,
        },
        success: (result) => {
            var tr = "";
            var totalPrice = 0;
            $.each(result, function (k, v) {
                totalPrice += v.price * v.quantity;
                tr += `<tr>

                                <td>${v.productName}</td>

                                <td>

                                    <input onchange="QuantityChange('${v.productId}', this)" class="CartItemQuantity" style="width:50px" min="1" type="number" value="${v.quantity}" />

                                    <input type="hidden" value="${v.productId}" class="CartItemProductId" />

                                </td>

                                <td>

                                    <a onclick="RemoveFromCart('${v.productId}')" class="btn btn--danger"><i class="fa-solid fa-trash-can"></i></a>

                                </td>

                            </tr>`
            });
            $("#ReceiptBody").html(tr);
            $("#TotalPrice").text(totalPrice);
        },
        error: (error) => {
            console.log(error)
        }
    })
}