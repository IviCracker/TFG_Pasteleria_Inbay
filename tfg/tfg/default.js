function mostrarDetalleProducto(nombre, imagenUrl, precio, valoracion) {
    // Codificar los parámetros de la URL para pasarlos a la página de detalle
    var encodedNombre = encodeURIComponent(nombre);
    var encodedImagenUrl = encodeURIComponent(imagenUrl);
    var encodedPrecio = encodeURIComponent(precio.toFixed(2)); // Asegúrate de que el precio tiene dos decimales
    var encodedValoracion = encodeURIComponent(valoracion.toFixed(1)); // Asegúrate de que la valoración tiene un decimal

    // Construir la URL de la página de detalle del producto con los parámetros codificados
    var url = "DetalleProducto.aspx?nombre=" + encodedNombre + "&imagenUrl=" + encodedImagenUrl + "&precio=" + encodedPrecio + "&valoracion=" + encodedValoracion;

    // Redirigir a la página de detalle del producto
    window.location.href = url;
}

document.addEventListener('DOMContentLoaded', function () {
    const cartIcon = document.getElementById('cartIcon');
    const cartPanel = document.getElementById('cartPanel');
    const closeCartPanel = document.getElementById('closeCartPanel');

    cartIcon.addEventListener('click', function () {
        cartPanel.classList.toggle('show');
    });

    closeCartPanel.addEventListener('click', function () {
        cartPanel.classList.remove('show'); // Quita la clase 'show' para cerrar el panel
    });
});


