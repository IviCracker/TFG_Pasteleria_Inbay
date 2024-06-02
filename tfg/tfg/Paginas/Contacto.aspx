<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contacto.aspx.cs" Inherits="tfg.Paginas.Contacto" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Contacto</title>


    <link rel="stylesheet" href="../index.css" />
    <link rel="stylesheet" href="../Estilos/Footer.css" />

    <script src="../index.js"></script>
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
        .main-content-container {
            width: 80%;
            margin: 0 auto;
            padding: 20px;
        }

        .content-section-wrapper {
            margin-bottom: 40px;
            text-align: center;
        }

            .content-section-wrapper h2 {
                color: #d35400;
                text-align: center;
                margin-bottom: 20px;
            }

        .contact-info {
            text-align: center;
        }

            .contact-info p {
                margin: 10px 0;
            }

            .contact-info a {
                color: #d35400;
                text-decoration: none;
            }

                .contact-info a:hover {
                    text-decoration: underline;
                }

        .social-media-icons {
            text-align: center;
            margin-top: 20px;
        }

            .social-media-icons a {
                display: inline-block;
                margin: 0 20px;
                transition: transform 0.3s ease;
            }

            .social-media-icons .social-icon {
                width: 60px;
                height: 60px;
                border-radius: 50%;
                box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
            }

            .social-media-icons a:hover {
                transform: scale(1.1);
            }
            .wide-iframe {
            width: 100%;
            max-width: 1000px; /* Puedes ajustar esto según tus necesidades */
            height: 450px; /* Puedes ajustar esto según tus necesidades */
            border: 0;
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
                            <ul class="submenu" id="productos">
                                <li><a href="---------------------">Panes</a></li>
                                <li><a href="---------------------">Bollería</a></li>
                                <li><a href="---------------------">Pastelería</a></li>
                                <li><a href="---------------------">Tartas</a></li>
                            </ul>
                        </li>
                        <li><a href="Nosotros.aspx">Nosotros</a></li>
                        <li><a href="Contacto.aspx">Contacto</a>
                            <ul class="submenu" id="contacto">
                                <li><a href="---------------------">Dónde estamos</a></li>
                            </ul>
                        </li>
                        <li><a href="HacerPedido.aspx">Hacer Pedido</a></li>
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
                    <% if (Session["UsuarioActual"] != null)
                        { %>
                    <asp:Button ID="verCarritoBtn" runat="server" Text="Realizar pago" OnClick="VerCarritoBtn_Click" CssClass="ver-carrito-btn" />

                    <% } %>
                </div>


            </header>

        </div>
        <div class="main-content-container">
            <div class="content-section-wrapper">
                <h2>Información de Contacto</h2>
                <div class="contact-info">
                    <p><strong>Dirección:</strong> Calle Capitán Blanco Argibay, 43, Tetuán, 28029 Madrid, España</p>
                    <p><strong>Teléfono:</strong> <a href="tel:+34915722017">+34 915 72 20 17</a></p>
                    <p><strong>Email:</strong> <a href="mailto:pasteleriainbay@gmail.com">pasteleriainbay@gmail.com</a></p>
                </div>
            </div>

            <div class="content-section-wrapper">
                <h2>Horario de Atención</h2>
                <p><strong>Lunes a Viernes:</strong> 8:30 AM - 2:30 PM y 5:00 PM - 8:00 PM</p>
                <p><strong>Sábado y Domingo:</strong> 8:30 AM - 2:40 PM</p>
            </div>

            <div class="content-section-wrapper">
                <h2>Encuéntranos Aquí</h2>
                <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3035.4373172422574!2d-3.7024960238887856!3d40.46558927143171!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0xd422908f13cc4bd%3A0x81b0e405c7a9816b!2sPasteler%C3%ADa%20Inbay!5e0!3m2!1ses!2ses!4v1717365833203!5m2!1ses!2ses" width="600" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade" class="wide-iframe"></iframe>            </div>

                <div class="content-section-wrapper">
                    <h2>Redes Sociales</h2>
                    <div class="social-media-icons">
                        <a href="https://www.facebook.com/pasteleriainbay" target="_blank">
                            <img src="../imagenes/logoFacebook.png" alt="Facebook" class="social-icon" /></a>
                        <a href="https://www.instagram.com/pasteleriainbay" target="_blank">
                            <img src="../imagenes/logoInstagram.png" alt="Instagram" class="social-icon" /></a>

                    </div>
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
