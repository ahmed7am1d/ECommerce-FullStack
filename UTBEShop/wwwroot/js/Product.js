//#region localVaraibles + BASKET ITEMS
var numberOfBasketItems = sessionStorage.getItem("basketItems") > 0 ? sessionStorage.getItem("basketItems") : 0;
document.getElementById("basketItems").innerText = numberOfBasketItems;
$("#total_price").text(parseFloat(sessionStorage.getItem("totalCartCost") ? sessionStorage.getItem("totalCartCost") : 0 ).toLocaleString("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 2,
    maximumFractionDigits: 4
}));
//#endregion 

//#region add to cart session

function Buy(productId, urlAction, outElementId, locale) {

    jQuery.ajax({

        type: "POST",
        url: urlAction,
        data: { productId: productId },
        dataType: "text",
        success: function (totalPrice) {
            numberOfBasketItems = +numberOfBasketItems + 1;
            sessionStorage.setItem("basketItems", numberOfBasketItems);
            numberOfBasketItems = sessionStorage.getItem("basketItems");
            ChnageTotalBasketAmount(+numberOfBasketItems);
            ChangeTotalPriceInformation(outElementId, locale, totalPrice);
        },
        error: function (req, status, error) {
            $(outElementId).text('error during buying!');
        }

    });
    ChnageTotalBasketAmount(numberOfBasketItems);

}


function ChnageTotalBasketAmount(amount) {
    document.getElementById("basketItems").innerText = amount;
}




function ChangeTotalPriceInformation(outElementId, locale, totalPrice) {
    sessionStorage.setItem("totalCartCost", totalPrice);
    $(outElementId).text(parseFloat(totalPrice).toLocaleString(locale, {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 2,
        maximumFractionDigits: 4
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

//#region reset basket items

function resetBasketItems() {
    numberOfBasketItems = 0;
    sessionStorage.setItem("basketItems", 0);
    sessionStorage.removeItem("totalCartCost");
}
//#endregion 