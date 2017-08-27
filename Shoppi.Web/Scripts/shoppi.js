$('.js-sign-out').click((e) => $.ajax({
    type: 'POST',
    url: $(e.target).data('url'),
    success: (data) => {
        $('.js-my-account').remove();
        $('.js-login-menu').after(data);
    }
}));