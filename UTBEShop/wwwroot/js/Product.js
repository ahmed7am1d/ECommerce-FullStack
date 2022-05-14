//#region localVaraibles
var numberOfBasketItems = 0;

//#endregion 

//#region add to cart session
function Buy(productId, urlAction, outElementId, locale) {
    console.log(urlAction);
    console.log(outElementId);
    jQuery.ajax({

        type: "POST",
        url: urlAction,
        data: { productId: productId },
        dataType: "text",
        success: function (totalPrice) {
            numberOfBasketItems = numberOfBasketItems + 1;
            $("#basketItems").load = numberOfBasketItems;
            ChangeTotalPriceInformation(outElementId, locale, totalPrice);
        },
        error: function (req, status, error) {
            $(outElementId).text('error during buying!');
        }

    });
}




function ChangeTotalPriceInformation(outElementId, locale, totalPrice) {
    $(outElementId).text(parseFloat(totalPrice).toLocaleString(locale, {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 2,
        maximumFractionDigits:4

    }));
}

//#endregion

//#region load basket items 

var basketIcon = document.querySelector("#basketIcon");
var overlayDark = document.querySelector(".overlayDarkPopOut");


function ShowBasketItems(urlAction) {
    var dropDownBasketContent = document.querySelector('.dropDownBasketContent');

    if (basketIcon.classList.contains("clicked")) {
        dropDownBasketContent.style.display = "none";
        basketIcon.classList.remove("clicked");
    }
    else {
        basketIcon.classList.add("clicked");
        
        jQuery.ajax({

            type: "POST",
            url: urlAction,
            contentType: "application/json;charset=utf-8",
            dataType: "html",
            success: function (data) {
                $("#basketIcon").html(data);
            },
            error: function () {
                $("#basketIcon").text('error during buying!');
            }

        });
    }
        
}




//#endregion 
