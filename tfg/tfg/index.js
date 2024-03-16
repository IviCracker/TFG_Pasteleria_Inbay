document.addEventListener("DOMContentLoaded", function () {
    const cartIcon = document.querySelector('.cart-icon');
    const cartPanel = document.querySelector('.cart-panel');
    const mainContent = document.querySelector('main');

    // Evento al hacer clic en el icono del carrito
    cartIcon.addEventListener('click', () => {
        cartPanel.classList.toggle('active'); // Toggle añade o quita la clase 'active'
        adjustMainContentWidth(); // Ajusta el ancho del contenido principal
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
});
