<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="tfg.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-JBGTE8PV6Y"></script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'G-JBGTE8PV6Y');
    </script>
    <!-- Google Tag Manager -->
    <script>(function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start':
                    new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
                    'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', 'GTM-5XMKPH6Q');</script>
    <!-- End Google Tag Manager -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>Pasteleria Inbay TFG - Ivan Almendros</title>
    <script type="application/ld+json">
    {
      "@context" : "https://schema.org",
      "@type" : "WebSite",
      "name" : "Pasteleria Inbay Trabajo de Grado - Ivan Almendros",
      "url" : "https://proyectopasteleriainbay.bsite.net/",
      "potentialAction": {
        "@type": "SearchAction",
        "target": {
          "@type": "EntryPoint",
          "urlTemplate": "https://proyectopasteleriainbay.bsite.net/search?q={search_term_string}"
        },
        "query-input": "required name=search_term_string"
      }
    }
    </script>
    <meta name="description" content="Disfruta de la mejor reposteria en Madrid. En Pasteleria Inbay creamos tortas y postres artesanales.">


    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="default.css" />
    <link rel="stylesheet" href="/Estilos/Footer.css" />

    <script src="default.js"></script>
    <script>
        document.querySelectorAll('.imagen-producto-carrito').forEach(function (producto) {
            producto.addEventListener('click', function () {
                var enlace = this.querySelector('a');
                if (enlace) {
                    window.location.href = enlace.getAttribute('data-url');
                }
            });
        });



    </script>

    <link rel="stylesheet" href="fonts/icomoon/style.css" />
    <link rel="stylesheet" href="css/owl.carousel.min.css" />
    <link rel="stylesheet" href="css/style.css" />
</head>



<body>
    <!-- Google Tag Manager (noscript) -->
    <noscript>
        <iframe src="https://www.googletagmanager.com/ns.html?id=GTM-5XMKPH6Q"
            height="0" width="0" style="display: none; visibility: hidden"></iframe>
    </noscript>
    <!-- End Google Tag Manager (noscript) -->
    <form id="form1" runat="server">
        <div class="usuario">
            <ul>
                <% if (Session["UsuarioActual"] != null)
                    { %>
                <li><a href="Paginas/Registro.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;<%= Session["UsuarioActual"] %></a></li>
                <% }
                    else
                    { %>
                <li><a href="Paginas/Registro.aspx">
                    <ion-icon name="lock-open"></ion-icon>
                    &nbsp;Registro</a></li>
                <% } %>
                <li><a href="/Paginas/MiCuenta.aspx">
                    <ion-icon name="person"></ion-icon>
                    &nbsp;Mi cuenta</a></li>
            </ul>
        </div>
        <div>
            <main>

                <header>
                    <div class="logo">
                        <a href="default.aspx">
                            <img src="imagenes/logoInbay4.png" width="150" /></a>
                    </div>
                    <input type="checkbox" id="menu-bar" />

                    <label for="menu-bar">Menu</label>

                    <nav class="navbar">
                        <ul>
                            <li><a href="default.aspx">Inicio</a></li>
                            <li><a href="Paginas/Productos.aspx">Productos</a>
                                <ul class="submenu" id="productos">
                                </ul>
                            </li>
                            <li><a href="Paginas/Nosotros.aspx">Nosotros</a></li>
                            <li><a href="Paginas/Contacto.aspx">Contacto</a>
                                <ul class="submenu" id="contacto">
                                </ul>
                            </li>

                        </ul>
                    </nav>

                    <div class="search-cart-container">
                        <div class="search-container">
                            <input type="text" placeholder="Buscar..." class="search-input" />
                            <asp:Button ID="searchButton" runat="server" Text="Buscar" CssClass="search-button" />

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
                <div style="text-align: center">
                    <img src="imagenes/portadaFinal.png" style="height: 80%;" />
                </div>

                <br />
                <br />

                <div class="productos-destacados">
                    <h2 style="text-align: center; font-size: 26px;">Productos Destacados</h2>
                    <div id="productosDestacadosContainer" runat="server" class="productos-container">
                        <!-- Aquí se cargarán dinámicamente los productos desde el servidor -->
                    </div>
                </div>







                <div class="ubicacion-horario">
                    <img src="imagenes/horario1.png" />
                </div>



                <div class="elfsight-app-1224c210-7a6c-478b-a82a-764cf5fa5110" data-elfsight-app-lazy>&nbsp</div>


            </main>
        </div>
        <div id="subfooter" class="subfooter">
            <p>¡Ven a nuestra pastelería y descubre un mundo de sabores exquisitos! Desde deliciosos croissants recién horneados hasta irresistibles tartas caseras.</p>
            <p>Prueba nuestras creaciones artesanales hechas con amor y los ingredientes más frescos. ¡Haz tu día más dulce con nosotros!</p>
            <a href="/Paginas/Contacto.aspx" class="subfooter-link">¡Contáctanos para más información!</a>
        </div>

        <footer>
            <div class="container-footer w-container">
                <div class="w-row">
                    <div class="footer-column w-clearfix w-col w-col-4">

                        <img src="imagenes/logoInbay4Vacio.png" alt="" width="200" class="failory-logo-image">
                    </div>
                    <div class="footer-column w-col w-col-8">
                        <div class="w-row">
                            <div class="w-col w-col-8">
                                <div class="w-row">
                                    <div class="w-col w-col-7 w-col-small-6 w-col-tiny-7">
                                        <h3 class="footer-titles">Informacion</h3>
                                        <p class="footer-links">
                                            <a href="default.aspx"><span class="footer-link">Inicio<br />
                                            </span></a>
                                            <a href="Paginas/Productos.aspx"><span class="footer-link">Productos<br />
                                            </span></a>
                                            <a href="Paginas/Nosotros.aspx"><span class="footer-link">Nosotros<br />
                                            </span></a>
                                            <a href="Paginas/Contacto.aspx"><span class="footer-link">Contacto<br />
                                            </span></a>
                                            <a href="Paginas/HacerPedido.aspx"><span class="footer-link">Hacer Pedido</span></a><strong><br />
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
    <script src="https://static.elfsight.com/platform/platform.js" data-use-service-core defer></script>
    <script src='https://widgets.sociablekit.com/google-reviews/widget.js' async defer></script>
    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <script src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.js"></script>
    <script src="js/jquery-3.3.1.min.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/owl.carousel.min.js"></script>
    <script src="js/main.js"></script>
    <script>

</script>
</body>
</html>






