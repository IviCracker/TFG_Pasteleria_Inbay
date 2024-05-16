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
