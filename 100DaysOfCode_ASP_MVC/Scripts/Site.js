$("#btnBuscar").on("click", function (e) {
    e.preventDefault();
    var valor = $("#btnBuscar").val();
    if (valor == "Buscar") {
        $($("form")[0]).attr("action", "/Producto");
        $("form")[0].submit();
    } else {
        window.location.href = "/Producto";
    }
});

$("#imagen").on("change", function () {
    var imagen = $("#imagen")[0].files[0];
    var url = window.URL.createObjectURL(imagen);
    $("#imgProducto")[0].src = url;
});