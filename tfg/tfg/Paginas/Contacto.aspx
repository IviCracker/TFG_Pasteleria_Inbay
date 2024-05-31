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

            .content-section-wrapper img, .content-section-wrapper iframe {
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

        .contact-info {
            display: flex;
            flex-direction: column;
            align-items: center;
            text-align: center;
        }

            .contact-info p {
                margin: 10px 0;
            }

        .contact-form-container {
            display: flex;
            flex-direction: column;
            align-items: center;
        }

            .contact-form-container form {
                width: 100%;
                max-width: 600px;
            }

            .contact-form-container label {
                display: block;
                margin-bottom: 5px;
                font-weight: bold;
            }

            .contact-form-container input, .contact-form-container textarea {
                width: 100%;
                padding: 10px;
                margin-bottom: 15px;
                border: 1px solid #ccc;
                border-radius: 5px;
            }

                .contact-form-container input[type="submit"] {
                    background-color: #d35400;
                    color: white;
                    border: none;
                    cursor: pointer;
                    padding: 15px;
                    font-size: 16px;
                }

        .social-media-icons {
            text-align: center;
            margin-top: 20px;
        }

            .social-media-icons a {
    display: inline-block;
    margin: 0 20px; /* Aumenta el margen para separar más los íconos */
    transition: transform 0.3s ease;
}

.social-media-icons a img {
    width: 60px; /* Aumenta el tamaño de los íconos */
    height: 60px; /* Aumenta el tamaño de los íconos */
    border-radius: 50%;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
}

                .social-media-icons a:hover {
                    transform: scale(1.1);
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
                    <p><strong>Dirección:</strong> Calle de la Pastelería, 123, 28000 Madrid, España</p>
                    <p><strong>Teléfono:</strong> +34 912 345 678</p>
                    <p><strong>Email:</strong> contacto@pasteleriainbay.com</p>
                </div>
            </div>

            <div class="content-section-wrapper">
                <h2>Horario de Atención</h2>
                <p>Lunes a Viernes: 8:00 AM - 8:00 PM</p>
                <p>Sábado: 9:00 AM - 5:00 PM</p>
                <p>Domingo: Cerrado</p>
            </div>

            <div class="content-section-wrapper">
                <h2>Formulario de Contacto</h2>
                <div class="contact-form-container">
                    <!-- Formulario de Contacto -->
                    <form action="enviar_formulario.php" method="post">
                        <label for="nombre">Nombre:</label>
                        <input type="text" id="nombre" name="nombre" required>

                        <label for="email">Email:</label>
                        <input type="email" id="email" name="email" required>

                        <label for="telefono">Teléfono:</label>
                        <input type="tel" id="telefono" name="telefono" required>

                        <label for="asunto">Asunto:</label>
                        <input type="text" id="asunto" name="asunto" required>

                        <label for="mensaje">Mensaje:</label>
                        <textarea id="mensaje" name="mensaje" rows="4" required></textarea>

                        <input type="submit" value="Enviar">
                    </form>
                </div>
            </div>

            <div class="content-section-wrapper">
                <h2>Encuéntranos Aquí</h2>
                <!-- Mapa de Google -->
                <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3151.835434509437!2d144.95373531531596!3d-37.81627937975145!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x6ad642af0f11fd81%3A0xf577a243b2d5f40b!2sCalle%20de%20la%20Pasteler%C3%ADa%2C%20123%2C%2028000%20Madrid%2C%20Espa%C3%B1a!5e0!3m2!1ses!2sus!4v1622729838694!5m2!1ses!2sus" width="600" height="450" style="border: 0;" allowfullscreen="" loading="lazy"></iframe>
            </div>

            <div class="content-section-wrapper">
                <h2>Redes Sociales</h2>
                <!-- Íconos de Redes Sociales -->
                <div class="social-media-icons">
                    <a href="https://www.facebook.com/pasteleriainbay" target="_blank">
                        <img src="ruta_a_icono_facebook.png" alt="Facebook"></a>
                    <a href="https://www.instagram.com/pasteleriainbay" target="_blank">
                        <img src="ruta_a_icono_instagram.png" alt="Instagram"></a>
                    <a href="https://www.twitter.com/pasteleriainbay" target="_blank">
                        <img src="ruta_a_icono_twitter.png" alt="Twitter"></a>
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
