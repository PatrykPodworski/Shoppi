$().ready(() => updateCart());

function updateCart() {
    $.ajax({
        type: 'GET',
        url: $('.js-cart-badge').data('url'),
        success: (data) => updateCartBadge(data.numberOfProducts)
    })
}

function updateCartBadge(numberOfProducts) {
    var cartBadge = $('.js-cart-badge');
    if (numberOfProducts == 0) {
        cartBadge.empty();
    }
    else {
        cartBadge.html(numberOfProducts);
    }
};

$('.js-sign-out').click((e) => $.ajax({
    type: 'POST',
    url: $(e.target).data('url'),
    success: (data) => {
        $('.js-my-account').remove();
        $('.js-login-menu').after(data);
    }
}));