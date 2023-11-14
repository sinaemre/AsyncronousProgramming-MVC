//fadeOut() => Yapıların görünümlerinin yok olması
$(function () {
    setTimeout(() => {
        $("div.alert.notification").fadeOut();
    }, 2000);
});