document.addEventListener('DOMContentLoaded', (event) => {
    let h = $('#site-nav').height();
    let f = $('footer').height();
    $('#bodydiv').css("padding-top", h +20);
    $('#bodydiv').css("padding-bottom", f);
});
