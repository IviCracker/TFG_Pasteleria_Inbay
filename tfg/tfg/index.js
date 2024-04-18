document.addEventListener("DOMContentLoaded", function () {
    const cartIcon = document.querySelector('.cart-icon');
    const cartPanel = document.querySelector('.cart-panel');
    const mainContent = document.querySelector('main');
    const modal = document.getElementById('modal');
    const closeBtn = document.querySelector('.close');

    // Evento al hacer clic en el icono del carrito
    cartIcon.addEventListener('click', () => {
        cartPanel.classList.toggle('active'); // Toggle añade o quita la clase 'active'
        adjustMainContentWidth(); // Ajusta el ancho del contenido principal
    });

    // Evento al hacer clic en el botón de cierre
    closeBtn.addEventListener('click', () => {
        closeModal(); // Cierra el modal
    });

    // Evento al hacer clic en una imagen de producto
    const productos = document.querySelectorAll('.producto img');
    productos.forEach(producto => {
        producto.addEventListener('click', () => {
            openModal(producto.alt, producto.src); // Abre el modal con la información del producto
        });
    });

    // Ajusta el ancho del contenido principal según el estado del panel del carrito
    function adjustMainContentWidth() {
        if (cartPanel.classList.contains('active')) {
            mainContent.style.width = `calc(100% - ${cartPanel.offsetWidth}px)`; // Resta el ancho del panel del carrito
            mainContent.style.marginRight = `${cartPanel.offsetWidth}px`; // Añade un margen igual al ancho del panel del carrito
        } else {
            mainContent.style.width = '100%'; // Vuelve a ocupar el 100% del ancho de la pantalla
            mainContent.style.marginRight = '0'; // Quita el margen
        }
    }

    // Abre el modal con la información del producto
    // Abre el modal con la información del producto y carga la información del producto
    function openModal(nombre, descripcion, precio, stock) {
        document.getElementById("modalProductName").innerText = nombre;
        document.getElementById("modalDescription").innerText = descripcion;
        document.getElementById("modalPrice").innerText = "Precio: $" + precio;
        document.getElementById("modalStock").innerText = "Stock: " + stock;

        // Aquí abres el modal
        var nombreProducto = event.target.getAttribute('data-nombre');

        // Llamar a la función CargarInformacionProducto con el nombre del producto seleccionado
        var informacionProducto = CargarInformacionProducto(nombreProducto);
    }

        // Actualizar contenido del modal
        modalImage.src = imagenUrl;
        modalImage.alt = nombreProducto;
        modalProductName.textContent = nombreProducto;

        modal.style.display = "block"; // Muestra el modal

        // Llamar a CargarInformacionProducto para cargar la información del producto
        cargarInformacionProducto(nombreProducto);
    }

    function cargarInformacionProducto(nombreProducto) {
        // Crear un objeto XMLHttpRequest
        var xhr = new XMLHttpRequest();

        // Configurar la solicitud AJAX
        xhr.open("GET", "index.aspx?nombreProducto=" + encodeURIComponent(nombreProducto), true);

        // Configurar la función de callback cuando la solicitud se complete
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                // Obtener la respuesta del servidor
                var response = xhr.responseText;

                // Actualizar el contenido del modal con la respuesta del servidor
                document.getElementById("modalDescription").innerText = response.descripcion;
                document.getElementById("modalPrice").innerText = "Precio: $" + response.precio;
                document.getElementById("modalStock").innerText = "Stock: " + response.stock;
            }
        };

        // Enviar la solicitud AJAX al servidor
        xhr.send();
    }



    // Cierra el modal
    function closeModal() {
        modal.style.display = "none"; // Oculta el modal
    }
});
