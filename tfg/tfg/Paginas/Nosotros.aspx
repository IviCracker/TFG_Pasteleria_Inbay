<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nosotros.aspx.cs" Inherits="tfg.Paginas.Nosotros" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-JBGTE8PV6Y"></script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'G-JBGTE8PV6Y');
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nosotros</title>


    <link rel="stylesheet" href="../default.css" />
    <link rel="stylesheet" href="../Estilos/Footer.css" />

    <script src="../default.js"></script>
    <script>
        document.querySelectorAll('.imagen-producto').forEach(function (producto) {
            producto.addEventListener('click', function () {
                var enlace = this.querySelector('a');
                if (enlace) {
                    window.location.href = enlace.getAttribute('data-url');
                }
            });
        });



    </script>



    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function mostrarDetalleProducto(nombre, imagenUrl, precio, valoracion) {
            // Codificar los parámetros de la URL para pasarlos a la página de detalle
            var encodedNombre = encodeURIComponent(nombre);
            var encodedImagenUrl = encodeURIComponent(imagenUrl);
            // Convertir el precio a formato de cadena con dos decimales

            var encodedPrecio = encodeURIComponent(precio);
            var encodedValoracion = encodeURIComponent(valoracion);

            // Construir la URL de la página de detalle del producto con los parámetros codificados
            var url = "DetalleProducto.aspx?nombre=" + encodedNombre + "&imagenUrl=" + encodedImagenUrl + "&precio=" + encodedPrecio + "&valoracion=" + encodedValoracion;

            // Redirigir a la página de detalle del producto
            window.location.href = url;
        }


    </script>
    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>
    <script src="../js/jquery-3.3.1.min.js"></script>
    <script src="../js/popper.min.js"></script>
    <script src="../js/owl.carousel.min.js"></script>
    <script src="../js/main.js"></script>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 0;
            color: #333;
        }

        .main-content-container {
            width: 80%;
            margin: 0 auto;
            padding: 20px;
        }

        .content-section-wrapper {
            margin-bottom: 40px;
        }

            .content-section-wrapper img {
                max-width: 100%;
                height: auto;
                display: block;
                margin: 0 auto 20px;
            }

            .content-section-wrapper h2 {
                color: #d35400;
                text-align: center;
                margin-bottom: 20px;
            }
    </style>
</head>




<body>
    <form id="form1" runat="server">


        <div class="usuario">
            <ul>
                <% if (Session["UsuarioActual"] != null)
                    { %>
                <li><a href="Registro.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;<%= Session["UsuarioActual"] %></a></li>
                <% }
                    else
                    { %>
                <li><a href="Registro.aspx">
                    <ion-icon name="lock-open"></ion-icon>
                    &nbsp;Registro</a></li>
                <% } %>
                <li><a href="MiCuenta.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;Mi cuenta</a></li>
            </ul>
        </div>
        <div>


            <header>
                <div class="logo">
                    <a href="../default.aspx">
                        <img src="../imagenes/logoInbay4.png" width="150" /></a>
                </div>
                <input type="checkbox" id="menu-bar" />
                <label for="menu-bar">Menu</label>
                <nav class="navbar">
                    <ul>
                        <li><a href="../default.aspx">Inicio</a></li>
                        <li><a href="Productos.aspx">Productos</a>

                        </li>
                        <li><a href="Nosotros.aspx">Nosotros</a></li>
                        <li><a href="Contacto.aspx">Contacto</a>
                            <ul class="submenu" id="contacto">
                            </ul>
                        </li>

                    </ul>
                </nav>

                <div class="search-cart-container">
                    <div class="search-container">
                        <input type="text" placeholder="Buscar..." class="search-input" />
                        <button type="submit" class="search-button">Buscar</button>
                    </div>

                </div>
                <div class="cart-icon" id="cartIcon">
                    <ion-icon name="cart"></ion-icon>
                </div>
                <div class="cart-panel" id="cartPanel">
                    <button type="button" class="close-btn" id="closeCartPanel">&times;</button>

                    <h2>Carrito de compra</h2>
                    <% if (Session["UsuarioActual"] == null)
                        { %>
                    <p>Inicia sesión antes de ver tu carrito</p>
                    <% } %>

                    <div id="productosCarritoContainer" runat="server" class="productos-carrito">
                        <!-- Aquí se cargarán dinámicamente los productos desde el servidor -->
                    </div>
                    <!--quiero que aqui llames a un codigo en c# que muestre un div con la informacion del total (precio total a pagar, de cada producto por la cantidad de veces), y 1 boton que sea, ver carro, que lleve a una pagina que se llama compra.aspx-->
                    <div id="cartInfoContainer" runat="server">
                    </div>
                    <!-- Aquí se mostrará la información del carrito -->
                    <% 
                        if (Session["UsuarioActual"] != null)
                        {
                            int idCliente = ObtenerIdCliente(); // Supongamos que tienes una función para obtener el ID del cliente

                            if (comprobarCarrito(idCliente) > 0)
                            {
                    %>
                                <asp:Button ID="verCarritoBtn" runat="server" Text="Realizar pago" OnClick="VerCarritoBtn_Click" CssClass="ver-carrito-btn" />
                    <% 
                            }
                        }
                    %>
                </div>


            </header>

        </div>
        <div class="main-content-container">
            <div class="content-section-wrapper">
                <h2>Bienvenidos a Pastelería Inbay</h2>
                <img src="ruta_a_imagen_bienvenidos.jpg" alt="Bienvenidos a Pastelería Inbay">
                <p>En Pastelería Inbay, la pasión por la repostería se entrelaza con la tradición y la innovación para crear experiencias culinarias inolvidables. Fundada en el corazón de nuestra querida ciudad, Inbay es más que una pastelería; es un lugar donde los sueños dulces se hacen realidad.</p>
            </div>

            <div class="content-section-wrapper">
                <h2>Nuestra Historia</h2>
                <img src="ruta_a_imagen_historia.jpg" alt="Nuestra Historia">
                <p>La historia de Inbay comienza hace más de tres décadas, cuando nuestra fundadora, Doña Isabel Bayona, decidió compartir su amor por la repostería con el mundo. Inspirada por las recetas de su abuela y su inigualable talento para combinar ingredientes, Doña Isabel abrió las puertas de Inbay con un objetivo claro: crear delicias que deleiten todos los sentidos.</p>
                <p>Desde entonces, hemos crecido y evolucionado, pero nuestra misión sigue siendo la misma. Hoy, la segunda generación de la familia Bayona continúa con el legado, incorporando técnicas modernas y nuevos sabores, pero siempre manteniendo la esencia que nos ha distinguido desde el principio.</p>
            </div>

            <div class="content-section-wrapper">
                <h2>Nuestra Filosofía</h2>
                <img src="ruta_a_imagen_filosofia.jpg" alt="Nuestra Filosofía">
                <p>En Pastelería Inbay, creemos que la repostería es un arte. Cada uno de nuestros productos es elaborado con dedicación y atención al detalle, utilizando solo los mejores ingredientes. Nos enorgullece ofrecer productos frescos, hechos a mano, y libres de conservantes artificiales.</p>
                <p>Nuestra filosofía se basa en tres pilares fundamentales:</p>
                <ul>
                    <li><strong>Calidad</strong>: Seleccionamos cuidadosamente cada ingrediente, desde la harina hasta el chocolate, asegurándonos de que cumplan con nuestros altos estándares de calidad.</li>
                    <li><strong>Creatividad</strong>: Nos encanta experimentar con nuevos sabores y técnicas para sorprender y deleitar a nuestros clientes. Ya sea a través de nuestras tartas personalizadas, nuestros macarons de temporada o nuestras innovadoras combinaciones de sabores, siempre estamos buscando formas de innovar.</li>
                    <li><strong>Tradición</strong>: Aunque estamos abiertos a la innovación, nunca olvidamos nuestras raíces. Nuestras recetas clásicas, como el pastel de tres leches y la tarta de manzana, siguen siendo un pilar fundamental de nuestro repertorio.</li>
                </ul>
            </div>

            <div class="content-section-wrapper">
                <h2>Nuestro Compromiso</h2>
                <img src="ruta_a_imagen_compromiso.jpg" alt="Nuestro Compromiso">
                <p>En Inbay, nos comprometemos no solo a satisfacer, sino a superar las expectativas de nuestros clientes. Sabemos que detrás de cada pedido hay una ocasión especial, un momento para recordar, y nos sentimos honrados de ser parte de esos momentos. Desde celebraciones familiares hasta grandes eventos corporativos, trabajamos de la mano con nuestros clientes para crear productos que no solo sean deliciosos, sino también memorables.</p>
            </div>

            <div class="content-section-wrapper">
                <h2>Nuestros Productos</h2>
                <img src="ruta_a_imagen_productos.jpg" alt="Nuestros Productos">
                <p>Nuestra amplia gama de productos incluye:</p>
                <ul>
                    <li><strong>Pasteles y tartas</strong>: Desde los clásicos pasteles de cumpleaños hasta elegantes tartas de bodas, cada creación es una obra de arte.</li>
                    <li><strong>Panadería artesanal</strong>: Ofrecemos una variedad de panes frescos, incluyendo nuestras famosas baguettes y croissants.</li>
                    <li><strong>Postres individuales</strong>: Macarons, cupcakes, eclairs y más, perfectos para cualquier antojo.</li>
                    <li><strong>Opciones personalizadas</strong>: Trabajamos con nuestros clientes para crear productos personalizados que se adapten a sus necesidades y preferencias.</li>
                </ul>
            </div>

            <div class="content-section-wrapper">
                <h2>Visítanos</h2>
                <img src="ruta_a_imagen_visitanos.jpg" alt="Visítanos">
                <p>Te invitamos a visitar nuestra pastelería y descubrir por ti mismo el mundo de sabores que hemos creado. Nuestro equipo de reposteros y panaderos estará encantado de atenderte y ayudarte a encontrar la delicia perfecta para cualquier ocasión.</p>
            </div>

            <div class="content-section-wrapper">
                <h2>Únete a Nuestra Familia</h2>
                <img src="ruta_a_imagen_familia.jpg" alt="Únete a Nuestra Familia">
                <p>En Inbay, consideramos a nuestros clientes como parte de nuestra gran familia. Nos encanta escuchar sus historias, recibir sus comentarios y ser parte de sus celebraciones. Síguenos en nuestras redes sociales para estar al tanto de nuestras últimas creaciones y promociones especiales.</p>
                <p>Gracias por ser parte de nuestra historia. En Pastelería Inbay, cada día es una nueva oportunidad para endulzar tu vida.</p>
            </div>
        </div>


        <%--<div id="subfooter" class="subfooter">
            <p>¡Ven a nuestra pastelería y descubre un mundo de sabores exquisitos! Desde deliciosos croissants recién horneados hasta irresistibles tartas caseras, ¡te esperamos con los brazos abiertos!</p>
            <p>No te pierdas nuestras ofertas especiales de la semana y eventos exclusivos. ¡Haz tu día más dulce con nosotros!</p>
            <a href="/Paginas/Contacto.aspx" class="subfooter-link">¡Contáctanos para más información!</a>
        </div>--%>

        <footer>
            <div class="container-footer w-container">
                <div class="w-row">
                    <div class="footer-column w-clearfix w-col w-col-4">

                        <img src="../imagenes/logoInbay4Vacio.png" alt="" width="200" class="failory-logo-image">
                    </div>
                    <div class="footer-column w-col w-col-8">
                        <div class="w-row">
                            <div class="w-col w-col-8">
                                <div class="w-row">
                                    <div class="w-col w-col-7 w-col-small-6 w-col-tiny-7">
                                        <h3 class="footer-titles">Informacion</h3>
                                        <p class="footer-links">
                                            <a href="../default.aspx"><span class="footer-link">Inicio<br />
                                            </span></a>
                                            <a href="Productos.aspx"><span class="footer-link">Productos<br />
                                            </span></a>
                                            <a href="Nosotros.aspx"><span class="footer-link">Nosotros<br />
                                            </span></a>
                                            <a href="Contacto.aspx"><span class="footer-link">Contacto<br />
                                            </span></a>
                                            <a href="HacerPedido.aspx"><span class="footer-link">Hacer Pedido</span></a><strong><br />
                                            </strong>
                                        </p>
                                    </div>
                                    <div class="w-col w-col-5 w-col-small-6 w-col-tiny-5">
                                        <h3 class="footer-titles">Otros</h3>
                                        <p class="footer-links">
                                            <a href="https://g.co/kgs/eR36i2S"><span class="footer-link">Pasteleria Inbay<br />
                                            </span></a>
                                            <a href="tel:915722017"><span class="footer-link">915 72 20 17<br />
                                            </span></a>
                                            <a href="https://maps.app.goo.gl/GMDvFgaLnnw97igS6"><span class="footer-link" style="white-space: nowrap; display: block;">Calle Capitan Blanco<br />
                                                Argibay 43<br />
                                            </span></a>
                                            <strong>
                                                <br />
                                            </strong>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="column-center-mobile w-col w-col-4">
                                <h3 class="footer-titles">Siguenos en nuestras redes sociales!</h3>
                                <a href="https://www.instagram.com/pasteleriainbay/" target="_blank" class="footer-social-network-icons w-inline-block">
                                    <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbfe70fcf5a0514c5b1da_Instagram%20Icon.svg" width="20" alt="Instagram icon" /></a><a href="https://www.facebook.com/pasteleriainbay/?locale=es_ES" target="_blank" class="footer-social-network-icons w-inline-block">
                                        <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbe42e1e6034fdaba46f6_Facebook%20Icon.svg" width="20" alt="Facebook Icon" /></a>
                                <p class="footer-description">
                                    Enviame un correo a: <a href="mailto:pasteleriainbay@gmail.com"><strong class="link-email-footer">PasteleriaInbay@gmail.com</strong></a><br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
        <div>
            <h6>COPYRIGHT 2024 - TODOS LOS DERECHOS RESERVADOS. DISEÑO POR <a href="https://www.linkedin.com/in/ivan-almendros-lozano/">IVÁN ALMENDROS</a></h6>
        </div>
    </form>
</body>
</html>
