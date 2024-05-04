<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="tfg.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pasteleria Inbay TFG</title>

    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="index.css" />
    <link rel="stylesheet" href="/Estilos/Footer.css" />

    <script src="index.js"></script>

    <link rel="stylesheet" href="fonts/icomoon/style.css" />
    <link rel="stylesheet" href="css/owl.carousel.min.css" />
    <link rel="stylesheet" href="css/style.css" />

</head>



<body>
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
                        <a href="index.aspx">
                            <img src="imagenes/logoInbay4.png" width="150" /></a>
                    </div>
                    <input type="checkbox" id="menu-bar" />
                    <label for="menu-bar">Menu</label>
                    <nav class="navbar">
                        <ul>
                            <li><a href="index.aspx">Inicio</a></li>
                            <li><a href="Paginas/Productos.aspx">Productos</a>
                                <ul class="submenu" id="productos">
                                    <li><a href="#">Panes</a></li>
                                    <li><a href="#">Bollería</a></li>
                                    <li><a href="#">Pastelería</a></li>
                                    <li><a href="#">Tartas</a></li>
                                </ul>
                            </li>
                            <li><a href="#">Nosotros</a></li>
                            <li><a href="#" onclick="mostrarSubMenu('contacto')">Contacto</a>
                                <ul class="submenu" id="contacto">
                                    <li><a href="#">Dónde estamos</a></li>
                                </ul>
                            </li>
                            <li><a href="#" onclick="cambiarVista(4)">Hacer Pedido</a></li>
                        </ul>
                    </nav>

                    <div class="search-cart-container">
                        <div class="search-container">
                            <input type="text" placeholder="Buscar..." class="search-input" />
                            <button type="submit" class="search-button">Buscar</button>
                        </div>

                    </div>
                    <div class="cart-icon">
                        <ion-icon size="large" name="cart-outline"></ion-icon>
                    </div>
                    <div class="cart-panel">
                        <h2>Carrito de compra</h2>
                        <ul>
                            <li>Producto 1</li>
                            <li>Producto 2</li>
                            <li>Producto 3</li>
                        </ul>
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
                    <h2>Horario y Ubicación</h2>
                    <p><strong>Horario de Apertura:</strong></p>
                    <ul>
                        <li>Lunes - Viernes: 8:30 AM - 2:30 PM, 5:00 PM - 8:00 PM</li>
                        <li>Sábado - Domingo: 8:30 AM - 2:30 PM</li>
                    </ul>
                </div>



                <div class="elfsight-app-1224c210-7a6c-478b-a82a-764cf5fa5110" data-elfsight-app-lazy>&nbsp</div>


            </main>
        </div>
        <div id="subfooter" class="subfooter">
            <p>¡Ven a nuestra pastelería y descubre un mundo de sabores exquisitos! Desde deliciosos croissants recién horneados hasta irresistibles tartas caseras, ¡te esperamos con los brazos abiertos!</p>
            <p>No te pierdas nuestras ofertas especiales de la semana y eventos exclusivos. ¡Haz tu día más dulce con nosotros!</p>
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
                                            <a href="" <span class="footer-link">Inicio<br />
                                            </span></a><a href=""><span class="footer-link">Productos<br />
                                            </span><a href=""><span class="footer-link">Nosotros<br />
                                            </span></a><a href=""><span class="footer-link">Contacto<br />
                                            </span></a><a href=""><span class="footer-link">Hacer Pedido</span></a><strong><br />
                                            </strong>
                                        </p>
                                    </div>
                                    <div class="w-col w-col-5 w-col-small-6 w-col-tiny-5">
                                        <h3 class="footer-titles">Otros</h3>
                                        <p class="footer-links">
                                            <a href=""><span class="footer-link">Pasteleria Inbay<br/></span></a>
                                            <a href=""><span class="footer-link">915 72 20 17<br/></span></a>
                                            <a href=""><span class="footer-link">Calle Capitan Blanco Argibay 43<br/></span></a>
                                            <strong><br/>
                                            </strong>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="column-center-mobile w-col w-col-4">
                                <h3 class="footer-titles">Siguenos en nuestras redes sociales!</h3>
                                <a href="" target="_blank" class="footer-social-network-icons w-inline-block">
                                    <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbf0a2f2b67e3b3ba079c_Twitter%20Icon.svg" width="20" alt="Twitter icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block">
                                        <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbfe70fcf5a0514c5b1da_Instagram%20Icon.svg" width="20" alt="Instagram icon"></a><a href="https://www.instagram.com/pasteleriainbay/" target="_blank" class="footer-social-network-icons w-inline-block">
                                            <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dbe42e1e6034fdaba46f6_Facebook%20Icon.svg" width="20" alt="Facebook Icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block">
                                                <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dc0002f2b676eb4ba0869_LinkedIn%20Icon.svg" width="20" alt="LinkedIn Icon"></a><a href="" target="_blank" class="footer-social-network-icons w-inline-block">
                                                    <img class="imagenesRedesSociales" src="https://uploads-ssl.webflow.com/5966ea9a9217ca534caf139f/5c8dc0112f2b6739c9ba0871_Pinterest%20Icon.svg" width="20" alt="Pinterest Icon"></a>
                                <p class="footer-description">
                                    Enviame un correo a: <a href=""><strong class="link-email-footer">PasteleriaInbay@gmail.com</strong></a><br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
        <div>
            <h6>COPYRIGHT 2024 - TODOS LOS DERECHOS RESERVADOS. DISEÑO POR POR <a href="https://www.linkedin.com/in/ivan-almendros-lozano/">IVÁN ALMENDROS</a></h6>
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


